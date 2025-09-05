Imports System.Data.OleDb
Public Class FRM_STOCKVIEW_SUMMARY
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

    Private RptType As RptStyle
    Dim SelFrmDate As Date
    Dim SelToDate As Date
    Dim SelCategory As String
    Dim gsformname As String = GetAdmindbSoftValue("GSFORMNAME", "GS11,GS12")
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

    Private Sub InsGsIssRec(ByVal Tran As String, ByVal RecIss As String, ByVal OrderInfo As Boolean, Optional ByVal InsComments As String = "")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,TRANNO"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If Weight = "" Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END GRSWT"
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END NETWT"
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,I.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'SA' THEN 'SALES'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'PU' THEN 'PURCHASE'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE I.TRANTYPE END AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE" + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")

            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & SelFrmDate.ToString("yyyy-MM-dd") & "' AND '" & SelToDate.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                If ChkRepairDet.Checked Then
                    strSql += vbCrLf + " AND I.TRANTYPE <> 'RD'"
                Else
                    strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL)"
                End If
            Else
                strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE <> 'SA'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            If RptType = RptStyle.TranNo Then
                strSql = " /** " & InsComments & "**/"
                strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & "','U') IS NOT NULL DROP TABLE #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                strSql = vbCrLf + " SELECT "
                strSql += vbCrLf + " CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"

                If Weight = "" Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END GRSWT"
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END NETWT"
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
                End If

                strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS"
                strSql += vbCrLf + " INTO #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
                strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE" + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
                strSql += vbCrLf + " WHERE I.TRANDATE < '" & SelFrmDate.ToString("yyyy-MM-dd") & "'"
                If OrderInfo = False Then
                    If ChkRepairDet.Checked Then
                        strSql += vbCrLf + " AND I.TRANTYPE <> 'RD'"
                    Else
                        strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL)"
                    End If
                Else
                    strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE <> 'SA'"
                End If
                strSql += vbCrLf + ""
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
                strSql += vbCrLf + " ,NULL TRANNO"
                strSql += vbCrLf + " ,SUM(PCS) AS PCS"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
                    strSql += vbCrLf + " ,SUM(NETWT) NETWT"
                Else
                    strSql += vbCrLf + " ,SUM(" & Weight & ") " & Weight & ""
                End If

                strSql += vbCrLf + " ,CATNAME,METAL"
                strSql += vbCrLf + " ,GS"
                strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
                strSql += vbCrLf + " FROM #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & " AS I"
                strSql += vbCrLf + " GROUP BY CATNAME,GS,METAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            Else
                strSql = " /** " & InsComments & "**/"
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " 'OPE' SEP"
                strSql += vbCrLf + " ,TRANNO"
                strSql += vbCrLf + " ," & IIf(RecIss = "I", "-1*", "") & "CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
                If Weight = "" Then
                    strSql += vbCrLf + " ," & IIf(RecIss = "I", "-1*", "") & "CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END GRSWT"
                    strSql += vbCrLf + " ," & IIf(RecIss = "I", "-1*", "") & "CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END NETWT"
                Else
                    strSql += vbCrLf + " ," & IIf(RecIss = "I", "-1*", "") & "CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
                End If
                strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
                strSql += vbCrLf + " ,NULL TRANDATE"
                strSql += vbCrLf + " ,NULL AS ACNAME"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
                strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE" + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
                strSql += vbCrLf + " WHERE I.TRANDATE < '" & SelFrmDate.ToString("yyyy-MM-dd") & "'"
                If OrderInfo = False Then
                    strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL)"
                Else
                    strSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE <> 'SA'"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If
        End If
    End Sub

    Private Sub InsGsStone(ByVal Tran As String, ByVal RecIss As String, Optional ByVal InsComments As String = "")
        Dim Apptrantype As String = ""
        If ChkWithApproval.Checked = False Then
            Apptrantype = "'AI','AR','IAP','RAP'"
        End If
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,S.TRANNO"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
            If Weight = "" Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END GRSWT"
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END NETWT"
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,S.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(S.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'SA' THEN 'SALES'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'PU' THEN 'PURCHASE'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE S.TRANTYPE END AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "") + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & SelFrmDate.ToString("yyyy-MM-dd") & "' AND '" & SelToDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Apptrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            If RptType = RptStyle.TranNo Then
                strSql = " /** " & InsComments & "**/"
                strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & "') IS NOT NULL DROP TABLE #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                strSql = vbCrLf + " SELECT "
                strSql += vbCrLf + " CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
                If Weight = "" Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END GRSWT"
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END NETWT"
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If
                strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
                strSql += vbCrLf + " INTO #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
                strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "") + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + " WHERE S.TRANDATE < '" & SelFrmDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
                If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                    strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                End If
                If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
                If Apptrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Apptrantype & ")"
                If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
                strSql += vbCrLf + ""
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
                strSql += vbCrLf + " ,NULL TRANNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                    strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                Else
                    strSql += vbCrLf + " ,SUM(" & Weight & ")" & Weight & ""
                End If
                strSql += vbCrLf + " ,CATNAME,METAL"
                strSql += vbCrLf + " ,GS"
                strSql += vbCrLf + " ,NULL TRANDATE,CONVERT(VARCHAR(100),NULL) AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME"
                strSql += vbCrLf + " FROM #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
                strSql += vbCrLf + " GROUP BY CATNAME,METAL,GS"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            Else
                strSql = " /** " & InsComments & "**/"
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
                strSql += vbCrLf + " ,S.TRANNO"
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
                If Weight = "" Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END GRSWT"
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END NETWT"
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If

                strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
                strSql += vbCrLf + " ,NULL TRANDATE"
                strSql += vbCrLf + " ,NULL AS ACNAME"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
                strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "") + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + " WHERE S.TRANDATE < '" & SelFrmDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
                If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                    strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                End If
                If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
                If Apptrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Apptrantype & ")"
                If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If
        End If

    End Sub

    Public Enum RptStyle
        Summary = 0
        TranDate = 1
        TranNo = 2
    End Enum


    Private Function DtStock() As DataTable
        Dim dtGrid As New DataTable(RptType.ToString)
        dtGrid.Columns.Add("TABLENAME", GetType(String))
        dtGrid.Columns("TABLENAME").DefaultValue = RptType.ToString
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        strSql = " DELETE FROM TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " DELETE FROM TEMP" & systemId & "GSSTOCK_1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + "  	SELECT "
        If RptType = RptStyle.TranNo Then
            strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + "  	,NULL TRANNO"
            strSql += vbCrLf + "    ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If Weight = "" Then
                strSql += vbCrLf + "    ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END AS GRSWT"
                strSql += vbCrLf + "    ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END AS NETWT"
            Else
                strSql += vbCrLf + "    ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END AS " & Weight & ""
            End If
        Else
            strSql += vbCrLf + "  	'OPE' SEP"
            strSql += vbCrLf + "  	,NULL TRANNO"
            strSql += vbCrLf + "    ,CASE WHEN I.TRANTYPE = 'I' THEN -1*1 ELSE 1 END * CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If Weight = "" Then
                strSql += vbCrLf + "    ,CASE WHEN I.TRANTYPE = 'I' THEN -1*1 ELSE 1 END * CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END AS GRSWT"
                strSql += vbCrLf + "    ,CASE WHEN I.TRANTYPE = 'I' THEN -1*1 ELSE 1 END * CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END AS NETWT"
            Else
                strSql += vbCrLf + "    ,CASE WHEN I.TRANTYPE = 'I' THEN -1*1 ELSE 1 END * CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END AS " & Weight & ""
            End If
        End If
        strSql += vbCrLf + "    ,CA.CATNAME,ME.METALNAME AS METAL"
        strSql += vbCrLf + "    ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
        strSql += vbCrLf + "  	FROM " & cnStockDb & "..OPENWEIGHT I "
        strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
        strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE" + IIf(SelCategory <> "", " AND CA.CATNAME = '" & SelCategory & "'", "")
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
        strSql += vbCrLf + "  	WHERE I.STOCKTYPE = 'C'"
        strSql += vbCrLf + "    AND I.COMPANYID = '" & strCompanyId & "'"
        If (ChkWithApproval.Checked = False And chkCmbTransation.Text = "ALL") Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')<>'A'"
        ElseIf chkCmbTransation.Text = "APPROVAL" Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='A'"
        ElseIf (chkCmbTransation.Text <> "APPROVAL" And chkCmbTransation.Text <> "ALL") Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')<>'A'"
        End If
        If chkCmbTransation.Text = "INTERNAL" Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='I'"
        End If
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
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
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

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
        Select Case RptType
            Case RptStyle.Summary
                strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " SELECT * FROM "
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT"
                strSql += vbCrLf + " CONVERT(VARCHAR(500),CATNAME) PARTICULAR"
                strSql += vbCrLf + " ,NULL TRANNO"
                strSql += vbCrLf + " ,NULL TTRANDATE"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS [OPENING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS [OPENING GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS [OPENING NETWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"

                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE PCS END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"
                Else
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS [OPENING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN " & Weight & " ELSE 0 END) AS [OPENING " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & Weight & " ELSE 0 END) AS [ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE PCS END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & Weight & " ELSE " & Weight & " END) AS [CLOSING " & Weight & "]"
                End If
                

                strSql += vbCrLf + " ,CATNAME"
                strSql += vbCrLf + " ,METAL"
                strSql += vbCrLf + " ,GS,NULL TRANDATE,NULL ACNAME"
                strSql += vbCrLf + " ,NULL ITEMNAME,1 AS RESULT,NULL COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " GROUP BY CATNAME,GS,METAL"
                strSql += vbCrLf + " )X"
                If rbtBoth.Checked Then
                    If Weight = "" Then
                        strSql += vbCrLf + " WHERE NOT ([OPENING GRSWT] = 0 AND [RECEIPT GRSWT] = 0 AND [ISSUE GRSWT] = 0 AND [OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                    Else
                        strSql += vbCrLf + " WHERE NOT ([OPENING " & Weight & "] = 0 AND [RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                    End If
                ElseIf rbtWeight.Checked Then
                    If Weight = "" Then
                        strSql += vbCrLf + " WHERE NOT ([OPENING GRSWT] = 0 AND [RECEIPT GRSWT] = 0 AND [ISSUE GRSWT] = 0)"
                    Else
                        strSql += vbCrLf + " WHERE NOT ([OPENING " & Weight & "] = 0 AND [RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
                    End If
                Else 'PCS
                    strSql += vbCrLf + " WHERE NOT ([OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                End If
                strSql += vbCrLf + "  ORDER BY GS,CATNAME"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT METAL,NULL GS,METAL PARTICULAR,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                strSql += vbCrLf + " "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+ISNULL(GS,''),0.1 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                strSql += vbCrLf + " "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                If Weight = "" Then
                    strSql += vbCrLf + " ,[OPENING GRSWT],[OPENING NETWT],[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[CLOSING GRSWT],[CLOSING NETWT]"
                Else
                    strSql += vbCrLf + " ,[OPENING " & Weight & "],[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[CLOSING " & Weight & "]"
                End If
                strSql += vbCrLf + " ,[OPENING PCS],[RECEIPT PCS],[ISSUE PCS],[CLOSING PCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"

                strSql += vbCrLf + " SELECT METAL,GS,' '+GS+' =>TOTAL'"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM([OPENING GRSWT]),SUM([OPENING NETWT]),SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([CLOSING GRSWT]),SUM([CLOSING NETWT])"
                Else
                    strSql += vbCrLf + " ,SUM([OPENING " & Weight & "]),SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([CLOSING " & Weight & "])"
                End If
                strSql += vbCrLf + " ,SUM([OPENING PCS]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),SUM([CLOSING PCS])"
                strSql += vbCrLf + " ,3 RESULT,'S' COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL,GS"
                strSql += vbCrLf + " "

                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                If Weight = "" Then
                    strSql += vbCrLf + " ,[OPENING GRSWT],[OPENING NETWT],[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[CLOSING GRSWT],[CLOSING NETWT]"
                Else
                    strSql += vbCrLf + " ,[OPENING " & Weight & "],[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[CLOSING " & Weight & "]"
                End If

                strSql += vbCrLf + " ,[OPENING PCS],[RECEIPT PCS],[ISSUE PCS],[CLOSING PCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT METAL,'ZZZZZ',METAL+' =>TOTAL'"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM([OPENING GRSWT]),SUM([OPENING NETWT]),SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([CLOSING GRSWT]),SUM([CLOSING NETWT])"
                Else
                    strSql += vbCrLf + " ,SUM([OPENING " & Weight & "]),SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([CLOSING " & Weight & "])"
                End If
                strSql += vbCrLf + " ,SUM([OPENING PCS]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),SUM([CLOSING PCS])"
                strSql += vbCrLf + " ,4 RESULT,'S' COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL"
                strSql += vbCrLf + " "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                If Weight = "" Then
                    strSql += vbCrLf + " ,[OPENING GRSWT],[OPENING NETWT],[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[CLOSING GRSWT],[CLOSING NETWT]"
                Else
                    strSql += vbCrLf + " ,[OPENING " & Weight & "],[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[CLOSING " & Weight & "]"
                End If

                strSql += vbCrLf + " ,[OPENING PCS],[RECEIPT PCS],[ISSUE PCS],[CLOSING PCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT 'ZZZZZ','ZZZZZ','GRAND TOTAL'"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM([OPENING GRSWT]),SUM([OPENING NETWT]),SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([CLOSING GRSWT]),SUM([CLOSING NETWT])"
                Else
                    strSql += vbCrLf + " ,SUM([OPENING " & Weight & "]),SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([CLOSING " & Weight & "])"
                End If
                strSql += vbCrLf + " ,SUM([OPENING PCS]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),SUM([CLOSING PCS])"
                strSql += vbCrLf + " ,3 RESULT,'G' COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                strSql += vbCrLf + " END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                If Weight = "" Then
                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING GRSWT] = NULL WHERE [OPENING GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT GRSWT] = NULL WHERE [RECEIPT GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE GRSWT] = NULL WHERE [ISSUE GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING GRSWT] = NULL WHERE [CLOSING GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING PCS] = NULL WHERE [OPENING PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                Else
                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING " & Weight & "] = NULL WHERE [OPENING " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING PCS] = NULL WHERE [OPENING PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                End If
                

                strSql = " SELECT * FROM TEMP" & systemId & "GSSTOCK "
                strSql += vbCrLf + "  ORDER BY METAL,GS,RESULT,CATNAME,PARTICULAR"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TABLENAME").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TTRANDATE").SetOrdinal(0)
                dtGrid.Columns("TRANNO").SetOrdinal(1)
                dtGrid.Columns("PARTICULAR").SetOrdinal(2)
            Case RptStyle.TranDate
                strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " SELECT * FROM "
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT"
                strSql += vbCrLf + " CONVERT(VARCHAR,TRANDATE,103) PARTICULAR"
                strSql += vbCrLf + " ,NULL TRANNO"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS [OPENING PCS]"
                If Weight = "" Then
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS [OPENING GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS [OPENING NETWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"

                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE NETWT END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"
                Else
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN " & Weight & " ELSE 0 END) AS [OPENING " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & Weight & " ELSE 0 END) AS [ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & Weight & " END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & Weight & " ELSE " & Weight & " END) AS [CLOSING " & Weight & "]"
                End If
                
                strSql += vbCrLf + " ,METAL+GS CATNAME"
                strSql += vbCrLf + " ,METAL"
                strSql += vbCrLf + " ,GS,TRANDATE,NULL ACNAME"
                strSql += vbCrLf + " ,NULL AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
                strSql += vbCrLf + " GROUP BY TRANDATE,GS,METAL"
                strSql += vbCrLf + " )X"
                If rbtBoth.Checked Then
                    If Weight = "" Then
                        strSql += vbCrLf + " WHERE NOT ([OPENING GRSWT] = 0 AND [OPENING NETWT] = 0 AND [RECEIPT GRSWT] = 0 AND [RECEIPT NETWT] = 0 AND [ISSUE GRSWT] = 0 AND AND [ISSUE NETWT] = 0 AND [OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"

                    Else
                        strSql += vbCrLf + " WHERE NOT ([OPENING " & Weight & "] = 0 AND [RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                    End If
                ElseIf rbtWeight.Checked Then
                    If Weight = "" Then
                        strSql += vbCrLf + " WHERE NOT ([OPENING GRSWT] = 0 AND [RECEIPT GRSWT] = 0 AND [ISSUE GRSWT] = 0)"
                    Else
                        strSql += vbCrLf + " WHERE NOT ([OPENING " & Weight & "] = 0 AND [RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
                    End If
                Else 'PCS
                    strSql += vbCrLf + " WHERE NOT ([OPENING PCS] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                End If
                strSql += vbCrLf + "  ORDER BY GS,CATNAME,TRANDATE"
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                'strSql = " DECLARE @PRECAT VARCHAR(60)"
                'strSql += vbCrLf + "  DECLARE @CATNAME VARCHAR(60)"
                'strSql += vbCrLf + "  DECLARE @RUNCLO_PCS NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @OPE_PCS NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @REC_PCS NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @ISS_PCS NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @RUNCLO_WT NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @OPE_WT NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @REC_WT NUMERIC(15,3)"
                'strSql += vbCrLf + "  DECLARE @ISS_WT NUMERIC(15,3)"
                'strSql += vbCrLf + "  SELECT @RUNCLO_PCS = 0"
                'strSql += vbCrLf + "  SELECT @OPE_PCS = 0"
                'strSql += vbCrLf + "  SELECT @REC_PCS = 0"
                'strSql += vbCrLf + "  SELECT @ISS_PCS = 0"

                'strSql += vbCrLf + "  SELECT @RUNCLO_WT = 0"
                'strSql += vbCrLf + "  SELECT @OPE_WT = 0"
                'strSql += vbCrLf + "  SELECT @REC_WT = 0"
                'strSql += vbCrLf + "  SELECT @ISS_WT = 0"
                'strSql += vbCrLf + "  SELECT @PRECAT = ''"
                'strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT CATNAME,[OPENING " & Weight & "],[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[OPENING PCS],[RECEIPT PCS],[ISSUE PCS] FROM TEMP" & systemId & "GSSTOCK"
                'strSql += vbCrLf + "  OPEN CUR"
                'strSql += vbCrLf + "  WHILE 1=1"
                'strSql += vbCrLf + "  BEGIN"
                'strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @CATNAME,@OPE_WT,@REC_WT,@ISS_WT,@OPE_PCS,@REC_PCS,@ISS_PCS"
                'strSql += vbCrLf + "  IF @@FETCH_STATUS = -1 BREAK"
                'strSql += vbCrLf + "  IF @PRECAT <> @CATNAME"
                'strSql += vbCrLf + "  	BEGIN"
                'strSql += vbCrLf + "  	SELECT @PRECAT = @CATNAME"
                'strSql += vbCrLf + "  	SELECT @RUNCLO_WT = @OPE_WT"
                'strSql += vbCrLf + "  	SELECT @RUNCLO_PCS = @OPE_PCS"
                'strSql += vbCrLf + "  	END"
                'strSql += vbCrLf + "  "
                'strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET "
                'strSql += vbCrLf + "  [OPENING PCS] = @RUNCLO_PCS"
                'strSql += vbCrLf + "  ,[OPENING " & Weight & "] = @RUNCLO_WT"
                'strSql += vbCrLf + "  ,[CLOSING PCS] = @RUNCLO_PCS + @REC_PCS - @ISS_PCS "
                'strSql += vbCrLf + "  ,[CLOSING " & Weight & "] = @RUNCLO_WT + @REC_WT - @ISS_WT "
                'strSql += vbCrLf + "  WHERE CURRENT of CUR"
                'strSql += vbCrLf + "  SELECT @RUNCLO_PCS = @RUNCLO_PCS + @REC_PCS - @ISS_PCS"
                'strSql += vbCrLf + "  SELECT @RUNCLO_WT = @RUNCLO_WT + @REC_WT - @ISS_WT"
                'strSql += vbCrLf + "  END"
                'strSql += vbCrLf + "  CLOSE CUR"
                'strSql += vbCrLf + "  DEALLOCATE CUR"
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                If Weight = "" Then
                    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
                    strSql += vbCrLf + " BEGIN"
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT METAL,NULL GS,METAL PARTICULAR,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+ISNULL(GS,''),0.1 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"

                    strSql += vbCrLf + " SELECT METAL,GS,' '+GS+' =>TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,3 RESULT,'S' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL,GS"
                    strSql += vbCrLf + " "

                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT METAL,'ZZZZZ',METAL+' =>TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,4 RESULT,'S' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT 'ZZZZZ','ZZZZZ','GRAND TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,3 RESULT,'G' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " END"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = 'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING GRSWT] = NULL WHERE [OPENING GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT GRSWT] = NULL WHERE [RECEIPT GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE GRSWT] = NULL WHERE [ISSUE GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING GRSWT] = NULL WHERE [CLOSING GRSWT] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING PCS] = NULL WHERE [OPENING PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                Else
                    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
                    strSql += vbCrLf + " BEGIN"
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT METAL,NULL GS,METAL PARTICULAR,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+ISNULL(GS,''),0.1 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "],[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"

                    strSql += vbCrLf + " SELECT METAL,GS,' '+GS+' =>TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,3 RESULT,'S' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL,GS"
                    strSql += vbCrLf + " "

                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "],[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT METAL,'ZZZZZ',METAL+' =>TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,4 RESULT,'S' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL"
                    strSql += vbCrLf + " "
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR"
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "],[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT PCS],[ISSUE PCS]"
                    strSql += vbCrLf + " ,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT 'ZZZZZ','ZZZZZ','GRAND TOTAL'"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT PCS]),SUM([ISSUE PCS])"
                    strSql += vbCrLf + " ,3 RESULT,'G' COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                    strSql += vbCrLf + " END"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = 'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING " & Weight & "] = NULL WHERE [OPENING " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [OPENING PCS] = NULL WHERE [OPENING PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                End If
                

                strSql = " SELECT * FROM TEMP" & systemId & "GSSTOCK "
                strSql += vbCrLf + "  ORDER BY METAL,GS,RESULT,TRANDATE,PARTICULAR"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                Dim Checker As String = "##gs2932"
                Dim RunWt As Decimal = 0
                Dim RunPcs As Integer = 0
                Dim CheckerFlag As Boolean = False
                For Each Ro As DataRow In dtGrid.Rows
                    If Ro.Item("CATNAME").ToString <> Checker Then
                        Checker = Ro.Item("CATNAME").ToString
                        RunWt = 0 : RunPcs = 0
                        CheckerFlag = True
                    End If
                    If Ro.Item("COLHEAD").ToString <> "" Then Continue For
                    If CheckerFlag Then
                        RunWt = Val(Ro.Item("OPENING " & Weight & "").ToString)
                        RunPcs = Val(Ro.Item("OPENING PCS").ToString)
                    End If
                    Ro.Item("OPENING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                    Ro.Item("OPENING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
                    RunWt += Val(Ro.Item("RECEIPT " & Weight & "").ToString) - Val(Ro.Item("ISSUE " & Weight & "").ToString)
                    RunPcs += Val(Ro.Item("RECEIPT PCS").ToString) - Val(Ro.Item("ISSUE PCS").ToString)
                    Ro.Item("CLOSING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                    Ro.Item("CLOSING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
                    CheckerFlag = False
                Next
                dtGrid.AcceptChanges()
                dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TABLENAME").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TTRANDATE").SetOrdinal(0)
                dtGrid.Columns("TRANNO").SetOrdinal(1)
                dtGrid.Columns("PARTICULAR").SetOrdinal(2)
            Case RptStyle.TranNo
                If Weight = "" Then
                    strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
                    strSql += vbCrLf + " SELECT * FROM "
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " SELECT"
                    strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
                    strSql += vbCrLf + " ,TRANNO"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                    strSql += vbCrLf + " ,NULL AS [OPENING PCS]"
                    strSql += vbCrLf + " ,NULL AS [OPENING GRSWT],NULL AS [OPENING NETWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"

                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"

                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE GRSWT END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"

                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CATNAME", "NULL") & " AS CATNAME"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CONVERT(vARCHAR,NULL)AS GS", "GS") & ",TRANDATE,ACNAME"
                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "ITEMNAME", "NULL") & " AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
                    strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
                    If rbtPcs.Checked Then
                        strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
                    End If
                    strSql += vbCrLf + " )X"
                    If rbtBoth.Checked Then
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT NETWT] = 0 AND [ISSUE NETWT] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                    ElseIf rbtWeight.Checked Then
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT NETWT] = 0 AND [ISSUE NETWT] = 0)"
                    Else
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                        ''PCS
                    End If
                    strSql += vbCrLf + "  ORDER BY GS,CATNAME,ITEMNAME,TRANDATE"
                    strSql += vbCrLf + " ,TRANNO"
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                Else
                    strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
                    strSql += vbCrLf + " SELECT * FROM "
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " SELECT"
                    strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
                    strSql += vbCrLf + " ,TRANNO"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                    strSql += vbCrLf + " ,NULL AS [OPENING PCS]"
                    strSql += vbCrLf + " ,NULL AS [OPENING " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & Weight & " ELSE 0 END) AS [ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & Weight & " END) AS [CLOSING PCS]"
                    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & Weight & " ELSE " & Weight & " END) AS [CLOSING " & Weight & "]"
                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CATNAME", "NULL") & " AS CATNAME"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CONVERT(vARCHAR,NULL)AS GS", "GS") & ",TRANDATE,ACNAME"
                    strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "ITEMNAME", "NULL") & " AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
                    strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
                    If rbtPcs.Checked Then
                        strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
                    End If
                    strSql += vbCrLf + " )X"
                    If rbtBoth.Checked Then
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                    ElseIf rbtWeight.Checked Then
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
                    Else
                        strSql += vbCrLf + " WHERE NOT ([RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
                        ''PCS
                    End If
                    strSql += vbCrLf + "  ORDER BY GS,CATNAME,ITEMNAME,TRANDATE"
                    strSql += vbCrLf + " ,TRANNO"
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                End If
               

                If Weight = "" Then

                    strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
                    strSql += vbCrLf + "   BEGIN"
                    strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 "
                    strSql += vbCrLf + "   AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
                    strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,ITEMNAME,PARTICULAR,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[RECEIPT PCS],[ISSUE PCS],RESULT,COLHEAD)"
                    strSql += vbCrLf + "   SELECT METAL,GS,TRANDATE,CATNAME,ITEMNAME,'   '+'SUB TOTAL',SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),4,'S2'COLHEAD"
                    strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,TRANDATE,CATNAME,ITEMNAME"
                    strSql += vbCrLf + "  "
                    If rbtPcs.Checked = False Then
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,PARTICULAR,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[CLOSING GRSWT],[CLOSING NETWT],[RECEIPT PCS],[ISSUE PCS],[CLOSING PCS],RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT METAL,GS,'" & _MaxDate.ToString("yyyy-MM-dd") & "','  '+GS+'=>TOTAL',SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([RECEIPT GRSWT])-SUM([ISSUE GRSWT]),SUM([RECEIPT NETWT])-SUM([ISSUE NETWT]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),SUM([RECEIPT PCS])-SUM([ISSUE PCS]),3,'S'COLHEAD"
                        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL,GS"
                        strSql += vbCrLf + "  "
                    Else
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,ITEMNAME,TRANDATE,PARTICULAR,[RECEIPT GRSWT],[RECEIPT NETWT],[ISSUE GRSWT],[ISSUE NETWT],[RECEIPT PCS],[ISSUE PCS],RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT',SUM([RECEIPT GRSWT]),SUM([RECEIPT NETWT]),SUM([ISSUE GRSWT]),SUM([ISSUE NETWT]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),3,'S1'COLHEAD"
                        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME,ITEMNAME"
                    End If
                    strSql += vbCrLf + "   END"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT GRSWT] = NULL WHERE [RECEIPT GRSWT] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE GRSWT] = NULL WHERE [ISSUE GRSWT] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING GRSWT] = NULL WHERE [CLOSING GRSWT] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                Else

                    strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
                    strSql += vbCrLf + "   BEGIN"
                    strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
                    strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 "
                    strSql += vbCrLf + "   AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
                    strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,ITEMNAME,PARTICULAR,[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[RECEIPT PCS],[ISSUE PCS],RESULT,COLHEAD)"
                    strSql += vbCrLf + "   SELECT METAL,GS,TRANDATE,CATNAME,ITEMNAME,'   '+'SUB TOTAL',SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),4,'S2'COLHEAD"
                    strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,TRANDATE,CATNAME,ITEMNAME"
                    strSql += vbCrLf + "  "
                    If rbtPcs.Checked = False Then
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,PARTICULAR,[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[CLOSING " & Weight & "],[RECEIPT PCS],[ISSUE PCS],[CLOSING PCS],RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT METAL,GS,'" & _MaxDate.ToString("yyyy-MM-dd") & "','  '+GS+'=>TOTAL',SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([RECEIPT " & Weight & "])-SUM([ISSUE " & Weight & "]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),SUM([RECEIPT PCS])-SUM([ISSUE PCS]),3,'S'COLHEAD"
                        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 GROUP BY METAL,GS"
                        strSql += vbCrLf + "  "
                    Else
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,ITEMNAME,TRANDATE,PARTICULAR,[RECEIPT " & Weight & "],[ISSUE " & Weight & "],[RECEIPT PCS],[ISSUE PCS],RESULT,COLHEAD)"
                        strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT',SUM([RECEIPT " & Weight & "]),SUM([ISSUE " & Weight & "]),SUM([RECEIPT PCS]),SUM([ISSUE PCS]),3,'S1'COLHEAD"
                        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME,ITEMNAME"
                    End If
                    strSql += vbCrLf + "   END"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
                    strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                End If

                strSql = " SELECT * FROM TEMP" & systemId & "GSSTOCK "
                strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,ITEMNAME,TRANDATE,RESULT"
                strSql += vbCrLf + " ,TRANNO"
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                Dim Checker As String = "##gs2932"
                Dim RunWt As Decimal = 0
                Dim RunPcs As Integer = 0
                For Each Ro As DataRow In dtGrid.Rows
                    If Ro.Item("CATNAME").ToString <> Checker Then
                        Checker = Ro.Item("CATNAME").ToString
                        RunWt = 0 : RunPcs = 0
                    End If
                    If Ro.Item("COLHEAD").ToString <> "" Then Continue For
                    Ro.Item("OPENING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                    Ro.Item("OPENING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
                    RunWt += Val(Ro.Item("RECEIPT " & Weight & "").ToString) - Val(Ro.Item("ISSUE " & Weight & "").ToString)
                    RunPcs += Val(Ro.Item("RECEIPT PCS").ToString) - Val(Ro.Item("ISSUE PCS").ToString)
                    Ro.Item("CLOSING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                    Ro.Item("CLOSING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
                Next
                dtGrid.AcceptChanges()
                dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TABLENAME").SetOrdinal(dtGrid.Columns.Count - 1)
                dtGrid.Columns("TTRANDATE").SetOrdinal(0)
                dtGrid.Columns("TRANNO").SetOrdinal(1)
                dtGrid.Columns("PARTICULAR").SetOrdinal(2)
        End Select
        Return dtGrid
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        'Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rbtGrsWeight.Checked Then
            Weight = "GRSWT"
        ElseIf rbtNetWt.Checked Then
            Weight = "NETWT"
        ElseIf rbtBothWt.Checked Then
            Weight = ""
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
        Dim Apptrantype As String = ""

        If chkCmbTransation.Text <> "ALL" And chkCmbTransation.Text <> "" Then
            If chkCmbTransation.Text.Contains("GENERAL") Then
                TranType += "'SA','OD','SR','PU','IIS','IPU','RRE','RPU','AD','RD',"
            End If
            If chkCmbTransation.Text.Contains("MISCISSUE") Then
                TranType += "'MI','IOT','ROT',"
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


        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ORDREP_DEL' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "ORDREP_DEL"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK_1' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "REC"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "REC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'IRC' AND I.TRANDATE <= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        If cmbstocktype.Text = "MANUFACTURING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
        ElseIf cmbstocktype.Text = "EXEMPTED" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
        ElseIf cmbstocktype.Text = "TRADING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
        End If
        If chkWithAlloy.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,AD.TRANDATE,AD.BATCHNO"
            strSql += vbCrLf + " ,ISNULL((SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID=AD.ALLOYID),I.CATCODE) CATCODE"
            strSql += vbCrLf + " ,AD.SNO,I.ACCODE,AD.TRANTYPE,0 PCS,AD.WEIGHT GRSWT,AD.WEIGHT NETWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON AD.ISSSNO=I.SNO"
            strSql += vbCrLf + " WHERE AD.TRANTYPE <> 'IRC' AND AD.TRANDATE <= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Apptrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "REC"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'RRC' AND I.TRANDATE <= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        If cmbstocktype.Text = "MANUFACTURING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
        ElseIf cmbstocktype.Text = "EXEMPTED" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
        ElseIf cmbstocktype.Text = "TRADING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


        strSql = " SELECT DISTINCT BATCHNO INTO TEMP" & systemId & "ORDREP_DEL "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANTYPE IN ('OD') AND ISNULL(CANCEL,'') = ''"
        If cmbstocktype.Text = "MANUFACTURING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
        ElseIf cmbstocktype.Text = "EXEMPTED" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
        ElseIf cmbstocktype.Text = "TRADING" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK"
        strSql += " ("
        strSql += " PARTICULAR VARCHAR(500)"
        strSql += vbCrLf + " ,TRANNO INT,TTRANDATE VARCHAR(10)"
        If Weight = "" Then
            strSql += vbCrLf + " ,[OPENING PCS] INT,[OPENING GRSWT] NUMERIC(15,3),[OPENING NETWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT GRSWT] NUMERIC(15,3),[RECEIPT NETWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE GRSWT] NUMERIC(15,3),[ISSUE NETWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING GRSWT] NUMERIC(15,3),[CLOSING NETWT] NUMERIC(15,3)"


        Else
            strSql += vbCrLf + " ,[OPENING PCS] INT,[OPENING " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & Weight & "] NUMERIC(15,3)"
        End If
        strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
        strSql += vbCrLf + " ,RESULT NUMERIC(10,2),COLHEAD VARCHAR(3)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP VARCHAR(10),TRANNO INT,PCS INT"
        If Weight = "" Then
            strSql += vbCrLf + " ,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3)"
        Else
            strSql += vbCrLf + " ," & Weight & " NUMERIC(15,3)"
        End If
        strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        RptType = RptStyle.Summary
        SelFrmDate = dtpFrom.Value.Date
        SelToDate = dtpTo.Value.Date
        SelCategory = ""
        Dim dtGrid As New DataTable
        dtGrid = DtStock()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "METAL ORNAMENT STOCK VIEW [SUMMARY-CATEGORY WISE]"
        Dim tit As String = "METAL ORNAMENT STOCK VIEW [SUMMARY-CATEGORY WISE]" + vbCrLf
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        If cmbstocktype.Text <> "" And cmbstocktype.Text <> "ALL" Then tit += " STOCK TYPE " & cmbstocktype.Text
        tit = tit + Cname
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        objGridShower.Show()
        Summary_Initiator(objGridShower, dtGrid)
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.pnlFooter.Visible = False
        'objGridShower.FormReLocation = False
        'objGridShower.FormReSize = True
        'DataGridView_Summary(objGridShower.gridView)
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'objGridShower.FormReSize = True
    End Sub
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If Not dgv.RowCount > 0 Then Exit Sub
            If dgv.CurrentRow Is Nothing Then Exit Sub
            Dim f As frmGridDispDia
            f = objGPack.GetParentControl(dgv)
            Select Case dgv.Rows(0).Cells("TABLENAME").Value.ToString
                Case RptStyle.TranNo.ToString
                    f.Text = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANDATE WISE]"
                    Dim tit As String = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANDATE WISE]" + vbCrLf
                    tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                    f.lblTitle.Text = tit
                    TranDate_Initiator(f, f.dsGrid.Tables(RptStyle.TranDate.ToString))
                Case RptStyle.TranDate.ToString
                    f.Text = "METAL ORNAMENT STOCK VIEW [SUMMARY-CATEGORY WISE]"
                    Dim tit As String = "METAL ORNAMENT STOCK VIEW [SUMMARY-CATEGORY WISE]" + vbCrLf
                    tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                    f.lblTitle.Text = tit
                    Summary_Initiator(f, f.dsGrid.Tables(RptStyle.Summary.ToString))
            End Select
            'If dgv.Rows(0).Cells("TABLENAME").Value.ToString = RptStyle.TranNo.ToString Then
            '    Dim f As frmGridDispDia
            '    f = objGPack.GetParentControl(dgv)
            '    f.FormReSize = False
            '    f.gridView.DataSource = Nothing
            '    f.gridView.DataSource = f.dsGrid.Tables(RptStyle.TranDate.ToString)
            '    f.gridView.CurrentCell = f.gridView.FirstDisplayedCell

            '    DataGridView_Trandate(f.gridView)
            '    Dim frmDate As Date = f.GetFilterStr(dtpFrom.Name)
            '    Dim toDate As Date = f.GetFilterStr(dtpTo.Name)
            '    f.Text = "ITEM WISE ISSUE/RECEIPT VIEW"
            '    Dim tit As String = "ITEM WISE ISSUE/RECEIPT SUMMARY VIEW" + vbCrLf
            '    tit += "DATE FROM " + Format(frmDate.Day, "00") + "/" + Format(frmDate.Month, "00") + "/" + frmDate.Year.ToString
            '    tit += " TO " + Format(toDate.Day, "00") + "/" + Format(toDate.Month, "00") + "/" + toDate.Year.ToString
            '    f.lblStatus.Text = "<Press [D] for Detail View>"
            '    f.FormReSize = True
            '    f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            'End If
        End If
    End Sub

    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            Select Case dgv.Rows(0).Cells("TABLENAME").Value.ToString
                Case RptStyle.Summary.ToString
                    If dgv.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
                    Dim f As frmGridDispDia
                    f = objGPack.GetParentControl(dgv)
                    Dim sCat As String = f.gridView.CurrentRow.Cells("CATNAME").Value.ToString
                    If sCat = "" Then Exit Select
                    SelCategory = ""
                    SelFrmDate = dtpFrom.Value.Date
                    SelToDate = dtpTo.Value.Date
                    If MessageBox.Show("Do you want to search then selected Category only?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        SelCategory = sCat
                    End If
                    RptType = RptStyle.TranDate
                    Dim dt As DataTable = DtStock()
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    f.Text = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANDATE WISE]"
                    Dim tit As String = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANDATE WISE]" + IIf(SelCategory <> "", " FOR ", "") & SelCategory + vbCrLf
                    tit += "DATE FROM " + SelFrmDate.ToString("dd/MM/yyyy") + " TO " + SelToDate.ToString("dd/MM/yyyy")
                    f.lblTitle.Text = tit
                    TranDate_Initiator(f, dt)
                Case RptStyle.TranDate.ToString
                    If dgv.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
                    Dim f As frmGridDispDia
                    f = objGPack.GetParentControl(dgv)
                    If f.gridView.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
                    SelFrmDate = dtpFrom.Value.Date
                    SelToDate = dtpTo.Value.Date
                    Dim sDate As Date = f.gridView.CurrentRow.Cells("TRANDATE").Value
                    If MessageBox.Show("Do you want to search then selected Date only?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        SelFrmDate = sDate
                        SelToDate = sDate
                    End If
                    RptType = RptStyle.TranNo
                    Dim dt As DataTable = DtStock()
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    f.Text = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANNO WISE]"
                    Dim tit As String = "METAL ORNAMENT STOCK VIEW [SUMMARY-TRANNO WISE]" + IIf(SelCategory <> "", " FOR ", "") & SelCategory + vbCrLf
                    tit += "DATE FROM " + SelFrmDate.ToString("dd/MM/yyyy") + " TO " + SelToDate.ToString("dd/MM/yyyy")
                    f.lblTitle.Text = tit
                    TranNo_Initiator(f, dt)
            End Select
        End If
    End Sub



    Private Sub FRM_STOCKVIEW_SUMMARY_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        Dim gsnamestr() As String
        If gsformname <> "" Then gsnamestr = Split(gsformname, ",")
        If gsnamestr(0) <> "" Then Gs11Name = gsnamestr(0)
        If gsnamestr(1) <> "" Then Gs12Name = gsnamestr(1)
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
        cmbstocktype.Items.Clear()
        cmbstocktype.Items.Add("ALL")
        cmbstocktype.Items.Add("MANUFACTURING")
        cmbstocktype.Items.Add("TRADING")
        cmbstocktype.Items.Add("EXEMPTED")
        cmbstocktype.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
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

    Private Sub FRM_STOCKVIEW_SUMMARY_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

    Private Sub LoadCategory()
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            Dim StrCatGroup As String = ""
            If cmbCategoryGroup.Text <> "ALL" And cmbCategoryGroup.Text <> "" Then
                StrCatGroup = " AND CGROUPID = (SELECT CGROUPID FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPNAME = '" & cmbCategoryGroup.Text & "')"
            End If
            strSql = ""
            strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " AND ISNULL(GS11,'') = 'Y' AND ISNULL(ACTIVE,'')<>'N'"
            strSql += StrCatGroup
            If strSql <> "" Then strSql += " UNION "
            strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " AND ISNULL(GS12,'') = 'Y' AND ISNULL(ACTIVE,'')<>'N'"
            strSql += StrCatGroup
            If strSql <> "" Then strSql += " UNION "
            strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " AND ISNULL(GS11,'') = '' AND ISNULL(GS12,'') = '' AND ISNULL(ACTIVE,'')<>'N'"
            strSql += StrCatGroup
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

    Private Sub Summary_Initiator(ByVal f As frmGridDispDia, ByVal dt As DataTable)
        If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
        f.dsGrid.Tables.Add(dt)
        f.gridView.DataSource = Nothing
        f.gridView.DataSource = f.dsGrid.Tables(RptStyle.Summary.ToString)
        f.pnlFooter.Visible = True
        f.FormReLocation = False
        f.FormReSize = False
        f.lblStatus.Text = "Press [D] for TranDate View>"
        DataGridView_Trandate(f.gridView)
        FillGridGroupStyle_KeyNoWise(f.gridView)
        f.FormReLocation = True
        f.FormReSize = True
        SelCategory = ""
        SelFrmDate = dtpFrom.Value.Date
        SelToDate = dtpTo.Value.Date
        f.gridView.Select()
    End Sub
    Private Sub TranNo_Initiator(ByVal f As frmGridDispDia, ByVal dt As DataTable)
        If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
        f.dsGrid.Tables.Add(dt)
        f.gridView.DataSource = Nothing
        f.gridView.DataSource = f.dsGrid.Tables(RptStyle.TranNo.ToString)
        f.pnlFooter.Visible = True
        f.FormReLocation = False
        f.FormReSize = False
        f.lblStatus.Text = "<Press [ESCAPE] for TranDate View>"
        DataGridView_Tranno(f.gridView)
        FillGridGroupStyle_KeyNoWise(f.gridView)
        f.FormReLocation = True
        f.FormReSize = True
        f.gridView.Select()
    End Sub

    Private Sub TranDate_Initiator(ByVal f As frmGridDispDia, ByVal dt As DataTable)
        If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
        f.dsGrid.Tables.Add(dt)
        f.gridView.DataSource = Nothing
        f.gridView.DataSource = f.dsGrid.Tables(RptStyle.TranDate.ToString)
        f.pnlFooter.Visible = True
        f.FormReLocation = False
        f.FormReSize = False
        f.lblStatus.Text = "<Press [ESCAPE] for Summary View> Press [D] for TranNo View>"
        DataGridView_Trandate(f.gridView)
        FillGridGroupStyle_KeyNoWise(f.gridView)
        f.FormReLocation = True
        f.FormReSize = True
        f.gridView.Select()
    End Sub

    Private Sub DataGridView_Trandate(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 0 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("PARTICULAR").Visible = True
            .Columns("OPENING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("RECEIPT PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("ISSUE PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("CLOSING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            If Weight = "" Then
                .Columns("OPENING GRSWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT GRSWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE GRSWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING GRSWT").Visible = rbtWeight.Checked Or rbtBoth.Checked

                .Columns("OPENING NETWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT NETWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE NETWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING NETWT").Visible = rbtWeight.Checked Or rbtBoth.Checked
            Else
                .Columns("OPENING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            End If

            .Columns("PARTICULAR").Width = 250
            .Columns("TRANNO").Width = 70
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("OPENING PCS").Width = 70
            .Columns("RECEIPT PCS").Width = 70
            .Columns("ISSUE PCS").Width = 70
            .Columns("CLOSING PCS").Width = 70

            If Weight = "" Then
                .Columns("OPENING GRSWT").Width = 100
                .Columns("RECEIPT GRSWT").Width = 100
                .Columns("ISSUE GRSWT").Width = 100
                .Columns("CLOSING GRSWT").Width = 100
                .Columns("OPENING NETWT").Width = 100
                .Columns("RECEIPT NETWT").Width = 100
                .Columns("ISSUE NETWT").Width = 100
                .Columns("CLOSING NETWT").Width = 100
            Else
                .Columns("OPENING " & Weight & "").Width = 100
                .Columns("RECEIPT " & Weight & "").Width = 100
                .Columns("ISSUE " & Weight & "").Width = 100
                .Columns("CLOSING " & Weight & "").Width = 100
            End If
            


            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("OPENING PCS").HeaderText = "OPE PCS"
            .Columns("RECEIPT PCS").HeaderText = "REC PCS"
            .Columns("ISSUE PCS").HeaderText = "ISS PCS"
            .Columns("CLOSING PCS").HeaderText = "CLO PCS"

            If Weight = "" Then
                .Columns("OPENING GRSWT").HeaderText = "OPE GRSWT"
                .Columns("RECEIPT GRSWT").HeaderText = "REC GRSWT"
                .Columns("ISSUE GRSWT").HeaderText = "ISS GRSWT"
                .Columns("CLOSING GRSWT").HeaderText = "CLO GRSWT"

                .Columns("OPENING NETWT").HeaderText = "OPE NETWT"
                .Columns("RECEIPT NETWT").HeaderText = "REC NETWT"
                .Columns("ISSUE NETWT").HeaderText = "ISS NETWT"
                .Columns("CLOSING NETWT").HeaderText = "CLO NETWT"
            Else
                .Columns("OPENING " & Weight & "").HeaderText = "OPE " & Weight & ""
                .Columns("RECEIPT " & Weight & "").HeaderText = "REC " & Weight & ""
                .Columns("ISSUE " & Weight & "").HeaderText = "ISS " & Weight & ""
                .Columns("CLOSING " & Weight & "").HeaderText = "CLO " & Weight & ""
            End If
            

            .Columns("OPENING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If Weight = "" Then
                .Columns("OPENING GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RECEIPT GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("OPENING NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RECEIPT GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Else
                .Columns("OPENING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RECEIPT " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub DataGridView_Summary(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 0 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("PARTICULAR").Visible = True
            .Columns("OPENING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("RECEIPT PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("ISSUE PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("CLOSING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked

            .Columns("OPENING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            .Columns("RECEIPT " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            .Columns("ISSUE " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            .Columns("CLOSING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked

            .Columns("PARTICULAR").Width = 350
            .Columns("TRANNO").Width = 70
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("OPENING PCS").Width = 70
            .Columns("RECEIPT PCS").Width = 70
            .Columns("ISSUE PCS").Width = 70
            .Columns("CLOSING PCS").Width = 70

            .Columns("OPENING " & Weight & "").Width = 100
            .Columns("RECEIPT " & Weight & "").Width = 100
            .Columns("ISSUE " & Weight & "").Width = 100
            .Columns("CLOSING " & Weight & "").Width = 100


            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("OPENING PCS").HeaderText = "OPE PCS"
            .Columns("RECEIPT PCS").HeaderText = "REC PCS"
            .Columns("ISSUE PCS").HeaderText = "ISS PCS"
            .Columns("CLOSING PCS").HeaderText = "CLO PCS"

            .Columns("OPENING " & Weight & "").HeaderText = "OPE " & Weight & ""
            .Columns("RECEIPT " & Weight & "").HeaderText = "REC " & Weight & ""
            .Columns("ISSUE " & Weight & "").HeaderText = "ISS " & Weight & ""
            .Columns("CLOSING " & Weight & "").HeaderText = "CLO " & Weight & ""

            .Columns("OPENING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("OPENING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub DataGridView_Tranno(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 1 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("TTRANDATE").Visible = True
            .Columns("TRANNO").Visible = True
            .Columns("PARTICULAR").Visible = True
            .Columns("RECEIPT PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("ISSUE PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("CLOSING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("RECEIPT " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            .Columns("ISSUE " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
            .Columns("CLOSING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked

            .Columns("PARTICULAR").Width = 350
            .Columns("TRANNO").Width = 70
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("RECEIPT PCS").Width = 70
            .Columns("ISSUE PCS").Width = 70
            .Columns("CLOSING PCS").Width = 70

            .Columns("RECEIPT " & Weight & "").Width = 100
            .Columns("ISSUE " & Weight & "").Width = 100
            .Columns("CLOSING " & Weight & "").Width = 100


            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT PCS").HeaderText = "REC PCS"
            .Columns("ISSUE PCS").HeaderText = "ISS PCS"
            .Columns("CLOSING PCS").HeaderText = "CLO PCS"

            .Columns("RECEIPT " & Weight & "").HeaderText = "REC " & Weight & ""
            .Columns("ISSUE " & Weight & "").HeaderText = "ISS " & Weight & ""
            .Columns("CLOSING " & Weight & "").HeaderText = "CLO " & Weight & ""


            .Columns("RECEIPT PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("RECEIPT " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub chkGs11_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadCategory()
    End Sub

    Private Sub chkGs12_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadCategory()
    End Sub

    Private Sub chkOthers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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
        Dim obj As New FRM_STOCKVIEW_SUMMARY_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtPcs = rbtPcs.Checked
        obj.p_rbtWeight = rbtWeight.Checked
        obj.p_cmbCategoryGroup = cmbCategoryGroup.Text
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        GetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem)
        GetChecked_CheckedList(chkCmbTransation, obj.p_chkCmbTransation)
        obj.p_rbtGrsWeight = rbtGrsWeight.Checked
        obj.p_rbtNetWt = rbtNetWt.Checked
        SetSettingsObj(obj, Me.Name, GetType(FRM_STOCKVIEW_SUMMARY_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New FRM_STOCKVIEW_SUMMARY_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_STOCKVIEW_SUMMARY_Properties))
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        rbtBoth.Checked = obj.p_rbtBoth
        rbtPcs.Checked = obj.p_rbtPcs
        rbtWeight.Checked = obj.p_rbtWeight
        cmbCategoryGroup.Text = obj.p_cmbCategoryGroup
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        SetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem, "ALL")
        SetChecked_CheckedList(chkCmbTransation, obj.p_chkCmbTransation, "ALL")
        rbtGrsWeight.Checked = obj.p_rbtGrsWeight
        rbtNetWt.Checked = obj.p_rbtNetWt
    End Sub

    Private Sub rbtPcs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtPcs.CheckedChanged
        chkCmbItem.Enabled = rbtPcs.Checked
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
End Class
Public Class FRM_STOCKVIEW_SUMMARY_Properties
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
