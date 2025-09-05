Imports System.Data.OleDb
Public Class ManuFacturingIssRec_ORDERINFO
    Private StrSql As String
    Private Da As OleDbDataAdapter
    Public OrderNo As String
    Dim MIMR_allowClosedOrder As Boolean = IIf(GetAdmindbSoftValue("MIMR_ALLOWCLOSEDORDER", "N") = "Y", True, False)
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim MI_ALLOW_MULTI_ISSUE As Boolean = IIf(GetAdmindbSoftValue("MI_ALLOW_MULTI_ISSUE", "N") = "Y", True, False)
    'Public BillCostId As String
    Public Sub New(ByVal OrderNo As String, ByVal BillCostId As String, ByVal MI As Boolean, Optional ByVal Accode As String = "", Optional ByVal Type As String = Nothing)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.OrderNo = OrderNo
        Dim DtOrderINfo As New DataTable
        DtOrderINfo.Columns.Add("CHECK", GetType(Boolean))
        DtOrderINfo.Columns.Add("STYLENO", GetType(String))
        DtOrderINfo.Columns.Add("ITEM", GetType(String))
        DtOrderINfo.Columns.Add("SUBITEM", GetType(String))
        DtOrderINfo.Columns.Add("PCS", GetType(Integer))
        DtOrderINfo.Columns.Add("GRSWT", GetType(Decimal))
        DtOrderINfo.Columns.Add("NETWT", GetType(Decimal))
        DtOrderINfo.Columns.Add("DIAPCS", GetType(Integer))
        DtOrderINfo.Columns.Add("DIAWT", GetType(Decimal))
        DtOrderINfo.Columns.Add("STATUS", GetType(String))
        DtOrderINfo.Columns.Add("WASTAGE", GetType(String))
        DtOrderINfo.Columns.Add("MC", GetType(String))
        DtOrderINfo.Columns.Add("SNO", GetType(String))
        DtOrderINfo.Columns("CHECK").DefaultValue = CType(False, Boolean)
        DtOrderINfo.AcceptChanges()
        If MIMR_allowClosedOrder Then
            StrSql = vbCrLf + " SELECT * FROM ( "
            StrSql += vbCrLf + " SELECT SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
            StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
            StrSql += vbCrLf + " ,OM.PCS--ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
            StrSql += vbCrLf + " ,OM.GRSWT--ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
            StrSql += vbCrLf + " ,OM.NETWT--ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            StrSql += vbCrLf + " ,OM.SNO,OM.WAST WASTAGE,OM.MC"
            StrSql += vbCrLf + " ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
            StrSql += vbCrLf + "   WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
            StrSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS "
            StrSql += vbCrLf + "   WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL "
            StrSql += vbCrLf + "   WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
            StrSql += vbCrLf + "   END AS STATUS"
            StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ENTRYORDER"
            StrSql += vbCrLf + " , 0 ISMULTIPLE"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS OM"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
            'StrSql += vbCrLf + " WHERE OM.ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "" & OrderNo & "'"
            StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
            StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
            StrSql += vbCrLf + " AND ISNULL(OM.COMPANYID,'') = " & strCompanyId & "'"
            StrSql += vbCrLf + " AND ISNULL(OM.COSTID,'') = " & BillCostId & "'"
            StrSql += vbCrLf + " )X   "
        Else
            If ORDER_MULTI_MIMR = False Then
                StrSql = vbCrLf + " SELECT * FROM ( "
                StrSql += vbCrLf + " SELECT SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
                StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                StrSql += vbCrLf + " ,OM.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
                StrSql += vbCrLf + " ,OM.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
                StrSql += vbCrLf + " ,OM.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
                StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                StrSql += vbCrLf + " ,OM.SNO,OM.WAST WASTAGE,OM.MC"
                StrSql += vbCrLf + " ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
                StrSql += vbCrLf + "   WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
                StrSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS "
                StrSql += vbCrLf + "   WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL "
                StrSql += vbCrLf + "   WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
                StrSql += vbCrLf + "   END AS STATUS"
                StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ENTRYORDER"
                StrSql += vbCrLf + " , 0 ISMULTIPLE"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS OM"
                StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
                'StrSql += vbCrLf + " WHERE OM.ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "" & OrderNo & "'"
                StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                StrSql += vbCrLf + " AND OM.COSTID = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND OM.COMPANYID = '" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                StrSql += vbCrLf + " )X WHERE 1=1  "
                If ORDER_MULTI_MIMR = False Then
                    StrSql += vbCrLf + " AND GRSWT > 0 "
                End If
                If MI Then
                    StrSql += vbCrLf + " AND STATUS <> 'ISSUE TO SMITH'  "
                Else
                    StrSql += vbCrLf + " AND STATUS <> 'RECEIVED FROM SMITH'  "
                End If
            Else
                StrSql = vbCrLf + " SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST AS OM WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "' "
                StrSql += vbCrLf + " AND OM.COSTID = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND OM.COMPANYID = '" & strCompanyId & "' And ISNULL(CANCEL,'') = ''"
                StrSql += vbCrLf + " ) And ISNULL(CANCEL,'') = ''"
                StrSql += vbCrLf + " AND ORDSTATE_ID NOT IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ISNULL(SMITH,'') = 'Y')"
                StrSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                If MI And MI_ALLOW_MULTI_ISSUE Then
                    StrSql += vbCrLf + " AND ORSTATUS = 'R' AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE "
                    StrSql += vbCrLf + "  METALID " & IIf(Type = "S", "IN ('T','D')", "NOT IN ('T','D')") & ")"
                End If
                StrSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "' "
                If Val(objGPack.GetSqlValue(StrSql, "").ToString) = 0 Then
                    StrSql = vbCrLf + " SELECT * FROM ( "
                    StrSql += vbCrLf + " SELECT SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
                    StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + " ,OM.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
                    StrSql += vbCrLf + " ,OM.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
                    StrSql += vbCrLf + " ,OM.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
                    StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    StrSql += vbCrLf + " ,OM.SNO,OM.WAST WASTAGE,OM.MC"
                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ACCODE"
                    StrSql += vbCrLf + " ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
                    StrSql += vbCrLf + "   WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
                    StrSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS "
                    StrSql += vbCrLf + "   WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL "
                    StrSql += vbCrLf + "   WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
                    StrSql += vbCrLf + "   END AS STATUS"
                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ENTRYORDER"
                    StrSql += vbCrLf + " , '' ORIRSNO,0 ISMULTIPLE "
                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS OM"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
                    'StrSql += vbCrLf + " WHERE OM.ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "" & OrderNo & "'"
                    StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                    StrSql += vbCrLf + " AND OM.COSTID = '" & BillCostId & "'"
                    StrSql += vbCrLf + " AND OM.COMPANYID = '" & strCompanyId & "'"
                    StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                    StrSql += vbCrLf + " )X WHERE 1=1  "
                    If MI Then
                        If Not MI_ALLOW_MULTI_ISSUE Then
                            StrSql += vbCrLf + " AND STATUS <> 'ISSUE TO SMITH'  "
                        End If
                    Else
                        StrSql += vbCrLf + " AND STATUS <> 'RECEIVED FROM SMITH'  "
                    End If
                    'If Accode <> "" Then
                    '    StrSql += vbCrLf + " AND ACCODE = '" & Accode & "'  "
                    'End If
                Else
                    'StrSql = vbCrLf + " SELECT * FROM ( "
                    'StrSql += vbCrLf + " SELECT SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
                    'StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    'StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 PCS FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) PCS"
                    'StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 GRSWT FROM " & cnAdminDb & "..ORIRDETAIL O WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54  ORDER BY ENTRYORDER DESC),0) GRSWT "
                    'StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1  NETWT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) NETWT"
                    'StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 SNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ORIRSNO"
                    'StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    'StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    'StrSql += vbCrLf + " ,OM.SNO,OM.WAST WASTAGE,OM.MC"
                    'StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ACCODE"
                    'StrSql += vbCrLf + " ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
                    'StrSql += vbCrLf + "   WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
                    'StrSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS "
                    'StrSql += vbCrLf + "   WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL "
                    'StrSql += vbCrLf + "   WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
                    'StrSql += vbCrLf + "   END AS STATUS"
                    'StrSql += vbCrLf + " ,(SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL  "
                    'StrSql += vbCrLf + "  WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC) ORDSTATE_ID "
                    'StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS OM"
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
                    ''StrSql += vbCrLf + " WHERE OM.ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "" & OrderNo & "'"
                    'StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                    'StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                    'StrSql += vbCrLf + " )X WHERE 1=1  "
                    'If MI Then
                    '    StrSql += vbCrLf + " AND STATUS <> 'ISSUE TO SMITH'  "
                    'Else
                    '    StrSql += vbCrLf + " AND STATUS <> 'RECEIVED FROM SMITH'  "
                    '    If Accode <> "" Then
                    '        StrSql += vbCrLf + " AND ACCODE = '" & Accode & "'  "
                    '    End If
                    'End If
                    'Dim MaxEntryorder As Integer = 0
                    'StrSql = vbCrLf + " SELECT isnull(MAX(ENTRYORDER),0)ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL AS O "
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORMAST OM ON O.ORSNO = OM.SNO AND ISNULL(OM.CANCEL ,'') = '' "
                    'StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                    'StrSql += vbCrLf + " AND O.ORDSTATE_ID <> 54 AND O.ORSTATUS = '" & IIf(MI, "I", "R") & "' AND ISNULL(O.CANCEL,'') = ''"
                    'StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                    'StrSql += vbCrLf + " AND O.COSTID = '" & BillCostId & "'"
                    'StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                    'If Not MI Then
                    '    If Accode <> "" Then
                    '        StrSql += vbCrLf + " AND O.ACCODE = '" & Accode & "'  "
                    '    End If
                    'End If
                    'StrSql += vbCrLf + " ORDER BY ENTRYORDER DESC"
                    'MaxEntryorder = Val(GetSqlValue(cn, StrSql))

                    'Commented On 2019-11-23 by Sudharsan'
                    'StrSql = vbCrLf + " SELECT "
                    'StrSql += vbCrLf + " SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
                    'StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    'StrSql += vbCrLf + " ,SUM(O.PCS) PCS,SUM(O.GRSWT) GRSWT,SUM(O.NETWT)NETWT,O.SNO ORIRSNO"
                    'StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    'StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    'StrSql += vbCrLf + " ,OM.SNO,SUM(OM.WAST) WASTAGE,SUM(OM.MC)MC,O.ACCODE "
                    'StrSql += vbCrLf + " ,(SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = O.ORDSTATE_ID) STATUS"
                    'StrSql += vbCrLf + " ,O.ENTRYORDER "
                    'StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORIRDETAIL AS O "
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORMAST OM ON O.ORSNO = OM.SNO AND ISNULL(OM.CANCEL ,'') = '' "
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = O.CATCODE"
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
                    'StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                    'StrSql += vbCrLf + " AND O.SNO NOT IN (SELECT ISNULL(REFSNO,'') REFSNO FROM " & cnAdminDb & "..ORIRDETAIL   "
                    'StrSql += vbCrLf + " WHERE ORNO = O.ORNO AND ORSNO = O.ORSNO AND ISNULL(CANCEL,'') = '')"
                    'StrSql += vbCrLf + " AND O.ORDSTATE_ID <> 54 AND O.ORSTATUS = '" & IIf(MI, "R", "I") & "' AND ISNULL(O.CANCEL,'') = ''"
                    'StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                    'StrSql += vbCrLf + " AND O.COSTID = '" & BillCostId & "'"
                    'StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                    'If Not MI Then
                    '    If Accode <> "" Then
                    '        StrSql += vbCrLf + " AND O.ACCODE = '" & Accode & "'  "
                    '    End If
                    'End If
                    'StrSql += vbCrLf + " AND C.METALID " & IIf(Type = "S", "IN ('T','D')", "NOT IN ('T','D')")
                    'StrSql += vbCrLf + " GROUP BY OM.ORNO,OM.STYLENO,OM.SNO,O.ACCODE,O.SNO,O.ORDSTATE_ID,O.ENTRYORDER" 'Commented On 2019-11-23 by Sudharsan'
                    'StrSql += vbCrLf + " ,IM.ITEMNAME,SM.SUBITEMNAME"
                    'StrSql += vbCrLf + " ORDER BY O.ENTRYORDER DESC"

                    StrSql = vbCrLf + "SELECT SNO,ORDERNO,ORNO,STYLENO,ITEM,SUBITEM, ACCODE"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  PCS ELSE PCS*-1 END )) PCS"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  GRSWT  ELSE GRSWT*-1 END )) GRSWT"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  NETWT ELSE NETWT*-1 END )) NETWT"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  DIAPCS ELSE DIAPCS*-1 END )) DIAPCS"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  DIAWT ELSE DIAWT*-1 END )) DIAWT"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  WASTAGE ELSE WASTAGE*-1 END )) WASTAGE"
                    StrSql += vbCrLf + ",SUM((CASE WHEN ORSTATUS = 'I' THEN  MC ELSE MC*-1 END )) MC "
                    StrSql += vbCrLf + ",CATCODE , CATNAME CATEGORY ,ITEMID , ITEMNAME ITEM,SUBITEMID,SUBITEMNAME SUBITEM,METALID ,METALNAME METAL "
                    StrSql += vbCrLf + " ,MAX(ENTRYORDER) ENTRYORDER,1 ISMULTIPLE"
                    StrSql += vbCrLf + " FROM ("
                    StrSql += vbCrLf + " SELECT "
                    StrSql += vbCrLf + " SUBSTRING(OM.ORNO,6,20)ORDERNO,OM.ORNO,OM.STYLENO"
                    StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + " ,C.CATCODE , C.CATNAME,IM.ITEMID , IM.ITEMNAME,SM.SUBITEMID,SM.SUBITEMNAME "
                    StrSql += vbCrLf + " ,SUM(O.PCS) PCS,SUM(O.GRSWT) GRSWT,SUM(O.NETWT)NETWT,O.SNO ORIRSNO"
                    StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    StrSql += vbCrLf + " ,OM.SNO,SUM(OM.WAST) WASTAGE,SUM(OM.MC)MC,O.ACCODE "
                    StrSql += vbCrLf + " ,(SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = O.ORDSTATE_ID) STATUS"
                    StrSql += vbCrLf + " ,O.ENTRYORDER "
                    StrSql += vbCrLf + " ,O.ORSTATUS ,M.METALID ,M.METALNAME "
                    StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORIRDETAIL AS O "
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORMAST OM ON O.ORSNO = OM.SNO AND ISNULL(OM.CANCEL ,'') = '' "
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = O.CATCODE"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS M ON C.METALID = M.METALID"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
                    StrSql += vbCrLf + " WHERE SUBSTRING(OM.ORNO,6,20) = '" & OrderNo & "'"
                    StrSql += vbCrLf + " AND O.ORDSTATE_ID <> 54 AND ISNULL(O.CANCEL,'') = ''"
                    StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
                    StrSql += vbCrLf + " AND O.COSTID = '" & BillCostId & "'"
                    StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                    StrSql += vbCrLf + " AND C.METALID NOT IN ('T','D')"
                    StrSql += vbCrLf + " GROUP BY OM.ORNO,OM.STYLENO,OM.SNO,O.ACCODE,O.SNO,O.ORDSTATE_ID,O.ENTRYORDER,O.ORSTATUS "
                    StrSql += vbCrLf + " ,C.CATCODE , C.CATNAME,IM.ITEMID ,SM.SUBITEMID ,M.METALID ,M.METALNAME "
                    StrSql += vbCrLf + " ,IM.ITEMNAME,SM.SUBITEMNAME)X"
                    StrSql += vbCrLf + " WHERE 1=1"
                    If Not MI Then
                        If Accode <> "" Then
                            StrSql += vbCrLf + " AND X.ACCODE = '" & Accode & "'  "
                        End If
                    End If
                    StrSql += vbCrLf + " GROUP BY SNO,ORDERNO, ORNO,STYLENO,ITEM,SUBITEM,ACCODE "
                    StrSql += vbCrLf + ",CATCODE , CATNAME,ITEMID , ITEMNAME,SUBITEMID,SUBITEMNAME,METALID ,METALNAME "

                End If
            End If
        End If
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtOrderINfo)
        Dim DTorders As New DataTable
        DTorders = DtOrderINfo.Clone
        For iii As Integer = 0 To DtOrderINfo.Rows.Count - 1
            If Val(DtOrderINfo.Rows(iii).Item("PCS").ToString) > 0 Or Val(DtOrderINfo.Rows(iii).Item("GRSWT").ToString) > 0 Or Val(DtOrderINfo.Rows(iii).Item("NETWT").ToString) > 0 Then
                Dim dnrow As DataRow = Nothing
                dnrow = DTorders.NewRow
                For ij As Integer = 0 To DTorders.Columns.Count - 1
                    dnrow(ij) = DtOrderINfo.Rows(iii).Item(ij)
                Next
                DTorders.Rows.Add(dnrow)
            End If
        Next

        DgvOrder.DataSource = DTorders
        'If DgvOrder.Rows.Count > 0 Then
        '    For Each dgr As DataGridViewRow In DgvOrder.Rows
        '        Select Case dgr.Cells("STATUS").Value
        '            Case "ISSUE TO SMITH"
        '                dgr.DefaultCellStyle.ForeColor = Color.RoyalBlue
        '            Case "ISSUE TO SMITH"
        '                dgr.DefaultCellStyle.ForeColor = Color.Red
        '        End Select
        '    Next
        'End If
        For cnt As Integer = 0 To DgvOrder.Columns.Count - 1
            DgvOrder.Columns(cnt).ReadOnly = True
            DgvOrder.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable

        Next
        DgvOrder.Columns("CHECK").ReadOnly = False
        With DgvOrder
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .Columns("STYLENO").Width = 80
            .Columns("ITEM").Width = 120
            .Columns("PCS").Width = 50
            .Columns("GRSWT").Width = 70
            .Columns("NETWT").Width = 70
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 70
            .Columns("STATUS").Width = 150

            .Columns("ORNO").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("SNO").Visible = False
            .Columns("WASTAGE").Visible = False
            .Columns("MC").Visible = False

            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim row() As DataRow = CType(DgvOrder.DataSource, DataTable).Select("CHECK = 'TRUE'")
        If row IsNot Nothing Then
            If row.Length > 0 Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MsgBox("Empty selection not allowed", MsgBoxStyle.Information)
            End If
        Else
            MsgBox("Empty selection not allowed", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ManuFacturingIssRec_ORDERINFO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.A Then
                If DgvOrder.RowCount > 0 Then
                    Dim bool As Boolean = CType(DgvOrder.Rows(0).Cells("CHECK").Value, Boolean)
                    For cnt As Integer = 0 To DgvOrder.RowCount - 1
                        DgvOrder.Rows(cnt).Cells("CHECK").Value = Not bool
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub ManuFacturingIssRec_ORDERINFO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DgvOrder.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        DgvOrder.Select()
        If DgvOrder.RowCount > 0 Then
            DgvOrder.CurrentCell = DgvOrder.Rows(0).Cells("CHECK")
        End If
    End Sub

    Private Sub DgvOrder_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DgvOrder.CurrentCellDirtyStateChanged
        DgvOrder.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub DgvOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvOrder.KeyDown
        If e.KeyCode = Keys.Enter Then
            'e.Handled = True
        End If
    End Sub

    Private Sub DgvOrder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DgvOrder.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If DgvOrder.RowCount > 0 Then
                DgvOrder.CurrentCell = DgvOrder.Rows(DgvOrder.CurrentRow.Index).Cells("CHECK")
            End If
            If DgvOrder.Rows.Count - 1 = DgvOrder.CurrentRow.Index Then
                btnOk.Select()
            End If
        End If
    End Sub
End Class