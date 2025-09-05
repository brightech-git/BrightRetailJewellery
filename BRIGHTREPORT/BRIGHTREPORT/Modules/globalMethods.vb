Imports System.Data.OleDb
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
Imports Newtonsoft.Json
Imports System.Globalization

Public Class ClearTaxInv
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String = Nothing
    Dim dt As New DataTable
    Dim B2B_E_INVOICE As Boolean = IIf(GetAdmindbSoftValue("B2B_E_INVOICE", "N") = "Y", True, False)
    Dim GST_EINV_CLIENTID As String = GetAdmindbSoftValue("GST_EINV_CLIENTID", "",, True)
    Dim GST_EINV_USERNAME As String = GetAdmindbSoftValue("GST_EINV_USERNAME", "",, True)
    Dim GST_EINV_PASSWORD As String = GetAdmindbSoftValue("GST_EINV_PASSWORD", "",, True)
    Dim GST_EINV_CLIENTSECRET As String = GetAdmindbSoftValue("GST_EINV_CLIENTSECRET", "",, True)
    Dim GST_EINV_URL As String = GetAdmindbSoftValue("GST_EINV_URL", "",, True)
    Dim GST_EINV_PUBLICKEY As String = GetAdmindbSoftValue("GST_EINV_PUBLICKEY", "",, True)
    Dim GST_EINV_OFFLINE_JSON As Boolean = IIf(GetAdmindbSoftValue("GST_EINV_OFFLINE_JSON", "N") = "Y", True, False)
    Dim GST_EINV_OWNERID As String = GetAdmindbSoftValue("GST_EINV_OWNERID", "",, True)
    Dim GST_EINV_AUTHTOKEN As String = GetAdmindbSoftValue("GST_EINV_AUTHTOKEN", "",, True)
    Dim GST_EINV_MASTER_AUTOUPLOAD As Boolean = IIf(GetAdmindbSoftValue("GST_EINV_MASTER_AUTOUPLOAD", "N") = "Y", True, False)
    Dim GST_EINV_MASTER_POST_IP As String = GetAdmindbSoftValue("GST_EINV_MASTER_POST_IP", "",, True)
    Dim GST_EINV_MASTER_EXPORT_GSTIN As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_GSTIN", "",, True)
    Dim GST_EINV_MASTER_EXPORT_STATECODE As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_STATECODE", "",, True)
    Dim GST_EINV_MASTER_EXPORT_PINCODE As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_PINCODE", "",, True)
    Dim ukculinfo As New CultureInfo("en-GB")

    Public Function B2B_Upload(ByVal _Batchno As String, ByVal _Type As String, Optional ByVal _Accode As String = "")
        If String.IsNullOrEmpty(GST_EINV_OWNERID) And GST_EINV_MASTER_AUTOUPLOAD = False Then
            MsgBox("Config Failed")
            Exit Function
        End If
        GST_EINV_OWNERID = GetAdmindbSoftValue("GST_EINV_OWNERID_" & strCompanyId.ToString, "",, True)
        If GST_EINV_OWNERID = "" Then
            GST_EINV_OWNERID = GetAdmindbSoftValue("GST_EINV_OWNERID", "",, True)
        End If
        Dim cc As New CallApi.Offline.GovtRes
        Dim cc1 As New CallApi.MasterTax.GovtRes
        Dim batchno As String = _Batchno
        Try
            If B2B_E_INVOICE Or GST_EINV_OFFLINE_JSON Or GST_EINV_MASTER_AUTOUPLOAD Then
                Dim btchNo As String = _Batchno
                strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,CONVERT(VARCHAR,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID))STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dtComp As New DataTable
                Dim dtItem As New DataTable
                Dim dtBuyer As New DataTable
                da.Fill(dtComp)
                Dim GST_chk As Boolean = True
                If dtComp.Rows(0)("GSTNO").ToString() = "" Then
                    MsgBox("Invalid GSTNO")
                    GST_chk = False
                End If
                If GST_EINV_OFFLINE_JSON = False And GST_EINV_MASTER_AUTOUPLOAD = False Then
                    If GST_EINV_URL.ToString = "" Then
                        MsgBox("Invalid GST_EINV_URL")
                        GST_chk = False
                    End If
                    If GST_EINV_CLIENTID.ToString = "" Then
                        MsgBox("Invalid GST_EINV_CLIENTID")
                        GST_chk = False
                    End If
                    If GST_EINV_USERNAME.ToString = "" Then
                        MsgBox("Invalid GST_EINV_USERNAME")
                        GST_chk = False
                    End If
                    If GST_EINV_PASSWORD.ToString = "" Then
                        MsgBox("Invalid GST_EINV_PASSWORD")
                        GST_chk = False
                    End If
                    If GST_EINV_CLIENTSECRET.ToString = "" Then
                        MsgBox("Invalid GST_EINV_CLIENTSECRET")
                        GST_chk = False
                    End If
                    If GST_EINV_PUBLICKEY.ToString = "" Then
                        MsgBox("Invalid GST_EINV_PUBLICKEY")
                        GST_chk = False
                    End If
                End If
                If dtComp.Rows.Count > 0 And GST_chk = True Then
                    If _Type = "DN" Or _Type = "CN" And _Accode <> "" And GST_EINV_OFFLINE_JSON Then
                        strSql = vbCrLf + " SELECT "
                        strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.CATNAME ITEMNAME,'' TAGNO,B.HSN HSN,0 PCS,CONVERT(NUMERIC(15,3),0) GRSWT,'P' SALEMODE"
                        strSql += vbCrLf + " ,ISNULL(AMOUNT,0)+ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO And ISNULL(STUDDED,'')<>'Y'),0) TOTALAMT"
                        strSql += vbCrLf + " ,AMOUNT"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO And ISNULL(STUDDED,'')<>'Y'),0) TAX"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) DISCOUNT,ISNULL(B.S_IGSTTAX,0) SALESTAX,A.PAYMODE TRANTYPE,"
                        strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO And ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
                        strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)TCS"
                        strSql += vbCrLf + " ,(CASE WHEN A.PAYMODE='CN' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/CN/' + CONVERT(VARCHAR,TRANNO) WHEN A.PAYMODE='DN' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/DN/'  + CONVERT(VARCHAR,TRANNO) ELSE CONVERT(VARCHAR,TRANNO) END )"
                        strSql += vbCrLf + "  TRANNO,"
                        strSql += vbCrLf + " 'N' ISSERVICE,"
                        strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ACCTRAN AS A "
                        strSql += vbCrLf + " LEFT JOIN "
                        strSql += vbCrLf + " (SELECT TOP 1 * FROM " & cnAdminDb & "..CATEGORY C WHERE P_SGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "') "
                        strSql += vbCrLf + " OR P_CGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "')"
                        strSql += vbCrLf + " OR P_IGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "')"
                        strSql += vbCrLf + " OR S_SGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "')"
                        strSql += vbCrLf + " OR S_CGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "')"
                        strSql += vbCrLf + " OR S_IGSTID IN(SELECT ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO ='" & btchNo & "')) AS B ON 1=1"
                        If _Type = "CN" Then
                            strSql += vbCrLf + " WHERE A.BATCHNO='" & btchNo & "' AND A.PAYMODE IN ('CN')"
                        ElseIf _Type = "DN" Then
                            strSql += vbCrLf + " WHERE A.BATCHNO='" & btchNo & "' AND A.PAYMODE IN ('DN')"
                        End If
                        strSql += vbCrLf + " AND A.ACCODE='" & _Accode & "'"
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtItem)
                    Else
                        strSql = vbCrLf + " SELECT "
                        strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.ITEMNAME,A.TAGNO,ISNULL(B.HSN,'')HSN,A.PCS PCS,ISNULL(A.GRSWT,0)GRSWT,A.SALEMODE,ISNULL(AMOUNT,0)+ISNULL(TAX,0) TOTALAMT,AMOUNT"
                        strSql += vbCrLf + " ,ISNULL(TAX,0)TAX,ISNULL(DISCOUNT,0)DISCOUNT,ISNULL(C.S_IGSTTAX,0) SALESTAX,A.TRANTYPE," 'ISNULL(C.CESSPER,0)CESSTAX"
                        strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
                        strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
                        strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=A.BATCHNO AND ACCODE='TCS'),0))TCS"
                        'strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') THEN  (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"

                        strSql += vbCrLf + " ,( CASE WHEN ISNULL(A.BILLPREFIX,'')<>'' THEN A.BILLPREFIX + '/' + CONVERT(VARCHAR,A.TRANNO) ELSE"
                        strSql += vbCrLf + " (CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN A.TRANTYPE='IPU' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/IPU/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') THEN  (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END )"
                        strSql += vbCrLf + " END ) TRANNO,"
                        strSql += vbCrLf + " ISNULL(B.ISSERVICE,'N') ISSERVICE,"

                        If _Type = "SR" Then
                            strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..RECEIPT AS A "
                        ElseIf _Type = "SA" Or _Type = "OD" Or _Type = "RD" Then
                            strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ISSUE AS A "
                        End If
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
                        If _Type = "SR" Then
                            strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SR')"
                        ElseIf _Type = "SA" Or _Type = "OD" Or _Type = "RD" Then
                            strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SA','RD','OD','IIN','IPU')"
                        End If
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtItem)
                    End If
                    If dtItem.Rows.Count = 0 Then
                        Exit Function
                    End If
                    If _Type = "DN" Or _Type = "CN" Then
                        strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
                        strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE"
                        strSql += vbCrLf + " ,'B2B' EINVTYPE"
                        strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE='" & _Accode & "'"
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtBuyer)
                        If dtBuyer.Rows.Count = 0 Then
                            Exit Function
                        End If
                        If dtBuyer.Rows.Count = 0 Then
                            MsgBox("Party Does not Exist")
                            Exit Function
                        End If
                    ElseIf GetSqlValue(cn, "SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'") <> "" And GST_EINV_MASTER_AUTOUPLOAD _
                    And GetSqlValue(cn, "SELECT ISNULL(GSTNO,'')GSTNO FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO _
                        WHERE BATCHNO='" & btchNo & "')") = "" Then
                        If GST_EINV_MASTER_EXPORT_GSTIN.ToString = "" Then
                            MsgBox("Export Gstin is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        If GST_EINV_MASTER_EXPORT_STATECODE.ToString = "" Then
                            MsgBox("Export stateCode is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        If GST_EINV_MASTER_EXPORT_PINCODE.ToString = "" Then
                            MsgBox("Export Pincode is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL"
                        strSql += vbCrLf + " ,'" & GST_EINV_MASTER_EXPORT_GSTIN.ToString & "' GSTNO,'" & GST_EINV_MASTER_EXPORT_PINCODE.ToString & "' PINCODE,'" & GST_EINV_MASTER_EXPORT_STATECODE.ToString & "' STATECODE "
                        strSql += vbCrLf + " ,(SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "')EINVTYPE"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') "
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtBuyer)

                        If dtBuyer.Rows.Count = 0 Then
                            MsgBox("Party Does not Exist")
                            Exit Function
                        End If
                    Else
                        If GST_EINV_MASTER_AUTOUPLOAD Then
                            strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE"
                            strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                            strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') "
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtBuyer)

                        Else
                            strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE"
                            strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                            strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') AND ISNULL(ACCODE,'')=''"
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtBuyer)

                            If dtBuyer.Rows.Count = 0 Then
                                strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
                                strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE"
                                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                                strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                                strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "')"
                                da = New OleDbDataAdapter(strSql, cn)
                                da.Fill(dtBuyer)
                                If dtBuyer.Rows.Count = 0 Then
                                    Exit Function
                                End If
                            End If
                        End If

                        If dtBuyer.Rows(0)("GSTNO").ToString = "" Then
                            MsgBox("Party Does not have GST No")
                            Exit Function
                        End If
                    End If



                    Dim crd As New CallApi.B2BInv.CREDENTIALS()
                    crd.GSTNO = dtComp.Rows(0)("GSTNO").ToString()
                    crd.CLIENTID = GST_EINV_CLIENTID
                    crd.USERNAME = GST_EINV_USERNAME
                    crd.PASSWORD = GST_EINV_PASSWORD
                    crd.SECERET = GST_EINV_CLIENTSECRET
                    crd.PublicKey = GST_EINV_PUBLICKEY

                    Dim _api As New CallApi.PushData
                    Dim cls As New CallApi.B2BInv.Para
                    cls._COMPANY = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.COMPANY)(dtComp)(0)
                    cls._BUYER = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.BUYER)(dtBuyer)(0)
                    cls._ITEM = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.ITEM)(dtItem)
                    cls._CREDENTIALS = crd

                    _api.apiurl = GST_EINV_URL
                    If GST_EINV_MASTER_AUTOUPLOAD Then
                        Dim vv As New CallApi.MasterTax
                        Dim incls As New CallApi.MasterTax.InputClass
                        Dim _incls As New List(Of CallApi.MasterTax.InputClass)
                        incls.drBuyer = dtBuyer.Rows(0)
                        incls.drComp = dtComp.Rows(0)
                        incls.dtItem = dtItem
                        _incls.Add(incls)

                        cc1 = vv.clsJonMasterTax(_incls, GST_EINV_URL.ToString, GST_EINV_USERNAME.ToString, GST_EINV_PASSWORD.ToString, GST_EINV_CLIENTID.ToString, GST_EINV_CLIENTSECRET.ToString, GST_EINV_MASTER_POST_IP.ToString)
                        If cc1 Is Nothing Then
                            Exit Function
                        End If
                        If cc1.status_cd.ToUpper = "SUCESS" Or cc1.status_cd.ToUpper = "1" Then
                            Dim tempackdate As Date = Date.Parse(cc1.data.AckDt.ToString, ukculinfo.DateTimeFormat)
                            strSql = " INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA,ACKNO,ACKDATE,TRANTYPE) SELECT '" & btchNo & "','" & cc1.data.Irn.ToString & "'"
                            strSql += " ,'" & cc1.data.SignedQRCode.ToString & "','" & cc1.data.AckNo.ToString & "','" & tempackdate.ToString("yyyy-MM-dd") & "','" & _Type.ToString & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            Dim msgStr As String = "Uploaded " + vbCrLf
                            msgStr += "Ack No : " + vbCrLf + cc1.data.AckNo.ToString + vbCrLf
                            MsgBox(msgStr)
                            Exit Function
                        Else
                            Dim msgStr As String = "Message from GST Portal " + vbCrLf
                            msgStr += cc1.status_desc + vbCrLf
                            ''For Each sr As CallApi.MasterTax.ErrorDetail In cc1.govt_response.ErrorDetails
                            ''    msgStr += sr.error_code + " : " + sr.error_message + vbCrLf
                            ''Next
                            MsgBox(msgStr)
                        End If
                        Exit Function
                    End If
                    If GST_EINV_OFFLINE_JSON Then
                        Dim vv As New CallApi.Offline
                        Dim incls As New CallApi.Offline.InputClass
                        Dim _incls As New List(Of CallApi.Offline.InputClass)
                        incls.drBuyer = dtBuyer.Rows(0)
                        incls.drComp = dtComp.Rows(0)
                        incls.dtItem = dtItem
                        _incls.Add(incls)


                        cc = vv.clsJonClearTax(_incls, GST_EINV_OWNERID, GST_EINV_URL, GST_EINV_AUTHTOKEN)
                        If cc Is Nothing Then
                            Exit Function
                        End If
                        If cc.document_status = "IRN_GENERATED" Then
                            Dim tempackdate As Date = Date.Parse(cc.govt_response.AckDt.ToString, ukculinfo.DateTimeFormat)
                            strSql = " INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA,ACKNO,ACKDATE,TRANTYPE) SELECT '" & btchNo & "', '" & cc.govt_response.Irn.ToString & "'"
                            strSql += ",'" & cc.govt_response.SignedQRCode.ToString & "','" & cc.govt_response.AckNo.ToString & "','" & tempackdate.ToString("yyyy-MM-dd") & "','" & _Type.ToString & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            Dim msgStr As String = "Uploaded " + vbCrLf
                            msgStr += "Ack No : " + vbCrLf + cc.govt_response.AckNo.ToString + vbCrLf
                            MsgBox(msgStr)
                            Exit Function
                        Else
                            Dim msgStr As String = "Message from GST Portal " + vbCrLf
                            msgStr += cc.document_status + vbCrLf
                            For Each sr As CallApi.Offline.ErrorDetail In cc.govt_response.ErrorDetails
                                msgStr += sr.error_code + " : " + sr.error_message + vbCrLf
                            Next
                            MsgBox(msgStr)
                        End If
                        Exit Function
                    End If


                    Dim res As List(Of String) = _api.Callapijson(JsonConvert.SerializeObject(cls))
                    If res.Count = 3 Then
                        If res(2) <> "" Then
                            MessageBox.Show(res(2), "Error from GST Portal")
                        Else
                            strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA) SELECT '" & btchNo & "','" & res(0) & "','" & res(1) & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            MsgBox("Uploaded")
                        End If
                    Else
                        MessageBox.Show("Bill not updated in GST portal " & IIf(res.Count > 1, res(0), ""), "Error from GST Portal")
                    End If

                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function B2B_Upload_19_may_2022(ByVal _Batchno As String, ByVal _Type As String)
        If String.IsNullOrEmpty(GST_EINV_OWNERID) And GST_EINV_MASTER_AUTOUPLOAD = False Then
            MsgBox("Config Failed")
            Exit Function
        End If
        GST_EINV_OWNERID = GetAdmindbSoftValue("GST_EINV_OWNERID_" & strCompanyId.ToString, "",, True)
        If GST_EINV_OWNERID = "" Then
            GST_EINV_OWNERID = GetAdmindbSoftValue("GST_EINV_OWNERID", "",, True)
        End If
        Dim cc As New CallApi.Offline.GovtRes
        Dim cc1 As New CallApi.MasterTax.GovtRes
        Dim batchno As String = _Batchno
        Try
            If B2B_E_INVOICE Or GST_EINV_OFFLINE_JSON Or GST_EINV_MASTER_AUTOUPLOAD Then
                Dim btchNo As String = _Batchno
                strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,CONVERT(VARCHAR,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID))STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dtComp As New DataTable
                Dim dtItem As New DataTable
                Dim dtBuyer As New DataTable
                da.Fill(dtComp)
                Dim GST_chk As Boolean = True
                If dtComp.Rows(0)("GSTNO").ToString() = "" Then
                    MsgBox("Invalid GSTNO")
                    GST_chk = False
                End If
                If GST_EINV_OFFLINE_JSON = False And GST_EINV_MASTER_AUTOUPLOAD = False Then
                    If GST_EINV_URL.ToString = "" Then
                        MsgBox("Invalid GST_EINV_URL")
                        GST_chk = False
                    End If
                    If GST_EINV_CLIENTID.ToString = "" Then
                        MsgBox("Invalid GST_EINV_CLIENTID")
                        GST_chk = False
                    End If
                    If GST_EINV_USERNAME.ToString = "" Then
                        MsgBox("Invalid GST_EINV_USERNAME")
                        GST_chk = False
                    End If
                    If GST_EINV_PASSWORD.ToString = "" Then
                        MsgBox("Invalid GST_EINV_PASSWORD")
                        GST_chk = False
                    End If
                    If GST_EINV_CLIENTSECRET.ToString = "" Then
                        MsgBox("Invalid GST_EINV_CLIENTSECRET")
                        GST_chk = False
                    End If
                    If GST_EINV_PUBLICKEY.ToString = "" Then
                        MsgBox("Invalid GST_EINV_PUBLICKEY")
                        GST_chk = False
                    End If
                End If
                If dtComp.Rows.Count > 0 And GST_chk = True Then

                    strSql = vbCrLf + " SELECT "
                    strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.ITEMNAME,A.TAGNO,ISNULL(B.HSN,'')HSN,A.PCS PCS,ISNULL(A.GRSWT,0)GRSWT,A.SALEMODE,ISNULL(AMOUNT,0)+ISNULL(TAX,0) TOTALAMT,AMOUNT"
                    strSql += vbCrLf + " ,ISNULL(TAX,0)TAX,ISNULL(DISCOUNT,0)DISCOUNT,ISNULL(C.S_IGSTTAX,0) SALESTAX,A.TRANTYPE," 'ISNULL(C.CESSPER,0)CESSTAX"
                    strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
                    strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
                    strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
                    strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
                    strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
                    strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=A.BATCHNO AND ACCODE='TCS'),0))TCS"
                    'strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') THEN  (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"

                    strSql += vbCrLf + " ,( CASE WHEN ISNULL(A.BILLPREFIX,'')<>'' THEN A.BILLPREFIX + '/' + CONVERT(VARCHAR,A.TRANNO) ELSE"
                    strSql += vbCrLf + " (CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN A.TRANTYPE='IPU' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/IPU/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') THEN  (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END )"
                    strSql += vbCrLf + " END ) TRANNO,"
                    strSql += vbCrLf + " ISNULL(B.ISSERVICE,'N') ISSERVICE,"

                    If _Type = "SR" Then
                        strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..RECEIPT AS A "
                    ElseIf _Type = "SA" Or _Type = "OD" Or _Type = "RD" Then
                        strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ISSUE AS A "
                    End If
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
                    If _Type = "SR" Then
                        strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SR')"
                    ElseIf _Type = "SA" Or _Type = "OD" Or _Type = "RD" Then
                        strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SA','RD','OD','IIN','IPU')"
                    End If
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtItem)

                    If dtItem.Rows.Count = 0 Then
                        Exit Function
                    End If

                    If GetSqlValue(cn, "SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'") <> "" And GST_EINV_MASTER_AUTOUPLOAD _
                        And GetSqlValue(cn, "SELECT ISNULL(GSTNO,'')GSTNO FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO _
                        WHERE BATCHNO='" & btchNo & "')") = "" Then
                        If GST_EINV_MASTER_EXPORT_GSTIN.ToString = "" Then
                            MsgBox("Export Gstin is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        If GST_EINV_MASTER_EXPORT_STATECODE.ToString = "" Then
                            MsgBox("Export stateCode is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        If GST_EINV_MASTER_EXPORT_PINCODE.ToString = "" Then
                            MsgBox("Export Pincode is empty", MsgBoxStyle.Information)
                            Exit Function
                        End If

                        strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL"
                        strSql += vbCrLf + " ,'" & GST_EINV_MASTER_EXPORT_GSTIN.ToString & "' GSTNO,'" & GST_EINV_MASTER_EXPORT_PINCODE.ToString & "' PINCODE,'" & GST_EINV_MASTER_EXPORT_STATECODE.ToString & "' STATECODE "
                        strSql += vbCrLf + " ,(SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "')EINVTYPE"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') "
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtBuyer)

                        If dtBuyer.Rows.Count = 0 Then
                            MsgBox("Party Does not Exist")
                            Exit Function
                        End If
                    Else
                        If GST_EINV_MASTER_AUTOUPLOAD Then
                            strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE"
                            strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                            strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') "
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtBuyer)

                        Else
                            strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE"
                            strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                            strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') AND ISNULL(ACCODE,'')=''"
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtBuyer)

                            If dtBuyer.Rows.Count = 0 Then
                                strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
                                strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE"
                                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'),'') <> '' THEN  "
                                strSql += vbCrLf + " (SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') ELSE 'B2B' END EINVTYPE"
                                strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "')"
                                da = New OleDbDataAdapter(strSql, cn)
                                da.Fill(dtBuyer)
                                If dtBuyer.Rows.Count = 0 Then
                                    Exit Function
                                End If
                            End If
                        End If

                        If dtBuyer.Rows(0)("GSTNO").ToString = "" Then
                            MsgBox("Party Does not have GST No")
                            Exit Function
                        End If
                    End If



                    Dim crd As New CallApi.B2BInv.CREDENTIALS()
                    crd.GSTNO = dtComp.Rows(0)("GSTNO").ToString()
                    crd.CLIENTID = GST_EINV_CLIENTID
                    crd.USERNAME = GST_EINV_USERNAME
                    crd.PASSWORD = GST_EINV_PASSWORD
                    crd.SECERET = GST_EINV_CLIENTSECRET
                    crd.PublicKey = GST_EINV_PUBLICKEY

                    Dim _api As New CallApi.PushData
                    Dim cls As New CallApi.B2BInv.Para
                    cls._COMPANY = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.COMPANY)(dtComp)(0)
                    cls._BUYER = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.BUYER)(dtBuyer)(0)
                    cls._ITEM = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.ITEM)(dtItem)
                    cls._CREDENTIALS = crd

                    _api.apiurl = GST_EINV_URL
                    If GST_EINV_MASTER_AUTOUPLOAD Then
                        Dim vv As New CallApi.MasterTax
                        Dim incls As New CallApi.MasterTax.InputClass
                        Dim _incls As New List(Of CallApi.MasterTax.InputClass)
                        incls.drBuyer = dtBuyer.Rows(0)
                        incls.drComp = dtComp.Rows(0)
                        incls.dtItem = dtItem
                        _incls.Add(incls)

                        cc1 = vv.clsJonMasterTax(_incls, GST_EINV_URL.ToString, GST_EINV_USERNAME.ToString, GST_EINV_PASSWORD.ToString, GST_EINV_CLIENTID.ToString, GST_EINV_CLIENTSECRET.ToString, GST_EINV_MASTER_POST_IP.ToString)
                        If cc1 Is Nothing Then
                            Exit Function
                        End If
                        If cc1.status_cd.ToUpper = "SUCESS" Or cc1.status_cd.ToUpper = "1" Then
                            strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA,ACKNO,TRANTYPE) SELECT '" & btchNo & "','" & cc1.data.Irn.ToString & "','" & cc1.data.SignedQRCode.ToString & "','" & cc1.data.AckNo.ToString & "','" & _Type.ToString & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            Dim msgStr As String = "Uploaded " + vbCrLf
                            msgStr += "Ack No : " + vbCrLf + cc1.data.AckNo.ToString + vbCrLf
                            MsgBox(msgStr)
                            Exit Function
                        Else
                            Dim msgStr As String = "Message from GST Portal " + vbCrLf
                            msgStr += cc1.status_desc + vbCrLf
                            ''For Each sr As CallApi.MasterTax.ErrorDetail In cc1.govt_response.ErrorDetails
                            ''    msgStr += sr.error_code + " : " + sr.error_message + vbCrLf
                            ''Next
                            MsgBox(msgStr)
                        End If
                        Exit Function
                    End If
                    If GST_EINV_OFFLINE_JSON Then
                        Dim vv As New CallApi.Offline
                        Dim incls As New CallApi.Offline.InputClass
                        Dim _incls As New List(Of CallApi.Offline.InputClass)
                        incls.drBuyer = dtBuyer.Rows(0)
                        incls.drComp = dtComp.Rows(0)
                        incls.dtItem = dtItem
                        _incls.Add(incls)


                        cc = vv.clsJonClearTax(_incls, GST_EINV_OWNERID, GST_EINV_URL, GST_EINV_AUTHTOKEN)
                        If cc Is Nothing Then
                            Exit Function
                        End If
                        If cc.document_status = "IRN_GENERATED" Then
                            strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA,ACKNO,TRANTYPE) SELECT '" & btchNo & "','" & cc.govt_response.Irn.ToString & "','" & cc.govt_response.SignedQRCode.ToString & "','" & cc.govt_response.AckNo.ToString & "','" & _Type.ToString & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            Dim msgStr As String = "Uploaded " + vbCrLf
                            msgStr += "Ack No : " + vbCrLf + cc.govt_response.AckNo.ToString + vbCrLf
                            MsgBox(msgStr)
                            Exit Function
                        Else
                            Dim msgStr As String = "Message from GST Portal " + vbCrLf
                            msgStr += cc.document_status + vbCrLf
                            For Each sr As CallApi.Offline.ErrorDetail In cc.govt_response.ErrorDetails
                                msgStr += sr.error_code + " : " + sr.error_message + vbCrLf
                            Next
                            MsgBox(msgStr)
                        End If
                        Exit Function
                    End If


                    Dim res As List(Of String) = _api.Callapijson(JsonConvert.SerializeObject(cls))
                    If res.Count = 3 Then
                        If res(2) <> "" Then
                            MessageBox.Show(res(2), "Error from GST Portal")
                        Else
                            strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA) SELECT '" & btchNo & "','" & res(0) & "','" & res(1) & "'"
                            'cmd = New OleDbCommand(strSql, cn)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            'cmd.ExecuteNonQuery()
                            Dim objp As New BrighttechPack.Methods
                            MsgBox("Uploaded")
                        End If
                    Else
                        MessageBox.Show("Bill not updated in GST portal " & IIf(res.Count > 1, res(0), ""), "Error from GST Portal")
                    End If

                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

End Class





Module globalMethods
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String = Nothing
    Dim dt As New DataTable
    Dim B2B_E_INVOICE As Boolean = IIf(GetAdmindbSoftValue("B2B_E_INVOICE", "N") = "Y", True, False)
    Dim GST_EINV_CLIENTID As String = GetAdmindbSoftValue("GST_EINV_CLIENTID", "",, True)
    Dim GST_EINV_USERNAME As String = GetAdmindbSoftValue("GST_EINV_USERNAME", "",, True)
    Dim GST_EINV_PASSWORD As String = GetAdmindbSoftValue("GST_EINV_PASSWORD", "",, True)
    Dim GST_EINV_CLIENTSECRET As String = GetAdmindbSoftValue("GST_EINV_CLIENTSECRET", "",, True)
    Dim GST_EINV_URL As String = GetAdmindbSoftValue("GST_EINV_URL", "",, True)
    Dim GST_EINV_PUBLICKEY As String = GetAdmindbSoftValue("GST_EINV_PUBLICKEY", "",, True)
    Dim GST_EINV_OFFLINE_JSON As Boolean = IIf(GetAdmindbSoftValue("GST_EINV_OFFLINE_JSON", "N") = "Y", True, False)

    <DllImport("advapi32.DLL", SetLastError:=True)>
    Public Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String,
        ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer,
        ByRef phToken As IntPtr) As Integer
    End Function



    'Public Function B2B_Upload(ByVal _Batchno As String, ByVal _Type As String)
    '    Dim cc As New CallApi.Offline.GovtRes
    '    Dim batchno As String = _Batchno
    '    Try
    '        If B2B_E_INVOICE Or GST_EINV_OFFLINE_JSON Then
    '            Dim btchNo As String = _Batchno
    '            strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,CONVERT(VARCHAR,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID))STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
    '            da = New OleDbDataAdapter(strSql, cn)
    '            Dim dtComp As New DataTable
    '            Dim dtItem As New DataTable
    '            Dim dtBuyer As New DataTable
    '            da.Fill(dtComp)
    '            Dim GST_chk As Boolean = True
    '            If dtComp.Rows(0)("GSTNO").ToString() = "" Then
    '                MsgBox("Invalid GSTNO")
    '                GST_chk = False
    '            End If
    '            If GST_EINV_OFFLINE_JSON = False Then
    '                If GST_EINV_URL.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_URL")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_CLIENTID.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_CLIENTID")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_USERNAME.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_USERNAME")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_PASSWORD.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_PASSWORD")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_CLIENTSECRET.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_CLIENTSECRET")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_PUBLICKEY.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_PUBLICKEY")
    '                    GST_chk = False
    '                End If
    '            End If
    '            If dtComp.Rows.Count > 0 And GST_chk = True Then

    '                strSql = vbCrLf + " SELECT "
    '                strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.ITEMNAME,A.TAGNO,ISNULL(B.HSN,'')HSN,A.PCS PCS,ISNULL(A.GRSWT,0)GRSWT,A.SALEMODE,ISNULL(AMOUNT,0)+ISNULL(TAX,0) TOTALAMT,AMOUNT"
    '                strSql += vbCrLf + " ,ISNULL(TAX,0)TAX,ISNULL(DISCOUNT,0)DISCOUNT,ISNULL(C.S_IGSTTAX,0) SALESTAX,A.TRANTYPE," 'ISNULL(C.CESSPER,0)CESSTAX"
    '                strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
    '                strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
    '                strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN COSTID + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN A.TRANTYPE='SR' THEN  COSTID + '/SR/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"
    '                If _Type = "SR" Then
    '                    strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..RECEIPT AS A "
    '                ElseIf _Type = "SA" Then
    '                    strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ISSUE AS A "
    '                End If
    '                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
    '                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
    '                If _Type = "SR" Then
    '                    strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SR')"
    '                ElseIf _Type = "SA" Then
    '                    strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SA','RD','OD','IIN')"
    '                End If
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtItem)

    '                strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE "
    '                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "')"
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtBuyer)

    '                If dtItem.Rows.Count = 0 Then
    '                    Exit Function
    '                End If

    '                If dtBuyer.Rows.Count = 0 Then
    '                    strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,phoneno PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
    '                    strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE "
    '                    strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "')"
    '                    da = New OleDbDataAdapter(strSql, cn)
    '                    da.Fill(dtBuyer)
    '                    If dtBuyer.Rows.Count = 0 Then
    '                        Exit Function
    '                    End If
    '                End If

    '                If dtBuyer.Rows(0)("GSTNO").ToString = "" Then
    '                    MsgBox("Party Does not have GST No")
    '                    Exit Function
    '                End If

    '                Dim crd As New CallApi.B2BInv.CREDENTIALS()
    '                crd.GSTNO = dtComp.Rows(0)("GSTNO").ToString()
    '                crd.CLIENTID = GST_EINV_CLIENTID
    '                crd.USERNAME = GST_EINV_USERNAME
    '                crd.PASSWORD = GST_EINV_PASSWORD
    '                crd.SECERET = GST_EINV_CLIENTSECRET
    '                crd.PublicKey = GST_EINV_PUBLICKEY

    '                Dim _api As New CallApi.PushData
    '                Dim cls As New CallApi.B2BInv.Para
    '                cls._COMPANY = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.COMPANY)(dtComp)(0)
    '                cls._BUYER = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.BUYER)(dtBuyer)(0)
    '                cls._ITEM = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.ITEM)(dtItem)
    '                cls._CREDENTIALS = crd

    '                _api.apiurl = GST_EINV_URL
    '                If GST_EINV_OFFLINE_JSON Then
    '                    Dim vv As New CallApi.Offline
    '                    Dim incls As New CallApi.Offline.InputClass
    '                    Dim _incls As New List(Of CallApi.Offline.InputClass)
    '                    incls.drBuyer = dtBuyer.Rows(0)
    '                    incls.drComp = dtComp.Rows(0)
    '                    incls.dtItem = dtItem
    '                    _incls.Add(incls)

    '                    cc = vv.clsJonClearTax(_incls)
    '                    If cc.document_status = "IRN_GENERATED" Then
    '                        strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA) SELECT '" & btchNo & "','" & cc.govt_response.Irn.ToString & "','" & cc.govt_response.SignedQRCode.ToString & "'"
    '                        cmd = New OleDbCommand(strSql, cn)
    '                        cmd.ExecuteNonQuery()
    '                        Dim objp As New BrighttechPack.Methods
    '                        Dim msgStr As String = "Uploaded " + vbCrLf
    '                        msgStr += "Ack No : " + vbCrLf + cc.govt_response.AckNo + vbCrLf
    '                        MsgBox(msgStr)
    '                    Else
    '                        Dim msgStr As String = "Message from GST Portal " + vbCrLf
    '                        msgStr += cc.document_status + vbCrLf
    '                        For Each sr As CallApi.Offline.ErrorDetail In cc.govt_response.ErrorDetails
    '                            msgStr += sr.error_code + " : " + sr.error_message + vbCrLf
    '                        Next
    '                        MsgBox(msgStr)
    '                    End If
    '                    Exit Function
    '                End If


    '                Dim res As List(Of String) = _api.Callapijson(JsonConvert.SerializeObject(cls))
    '                If res.Count = 3 Then
    '                    If res(2) <> "" Then
    '                        MessageBox.Show(res(2), "Error from GST Portal")
    '                    Else
    '                        strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA) SELECT '" & btchNo & "','" & res(0) & "','" & res(1) & "'"
    '                        cmd = New OleDbCommand(strSql, cn)
    '                        cmd.ExecuteNonQuery()
    '                        Dim objp As New BrighttechPack.Methods
    '                        MsgBox("Uploaded")
    '                    End If
    '                Else
    '                    MessageBox.Show("Bill not updated in GST portal " & IIf(res.Count > 1, res(0), ""), "Error from GST Portal")
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Function

    Public Sub PrintStockTransfer(ByVal RefNo As String, ByVal RefDate As Date)
        If RefNo = "" Then Exit Sub
        strSql = " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)aS COSTNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
        strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        Dim tCostName As String = objGPack.GetSqlValue(strSql)
        If tCostName = "" Then
            Exit Sub
        End If
        Dim UserName As String = Nothing
        Dim UpTime As String = Nothing
        strSql = " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION"
        strSql += vbCrLf + " SELECT TOP 1 CASE WHEN UPTIME IS NOT NULL THEN SUBSTRING(CONVERT(VARCHAR,UPTIME,113),13,5) ELSE '' END AS UPTIME"
        strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE REFNO = '" & RefNo & "' AND REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
        Dim DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            UserName = DtTemp.Rows(0).Item("USERNAME").ToString
            UpTime = DtTemp.Rows(0).Item("UPTIME").ToString
            If UpTime = "00:00" Then UpTime = ""
        End If


        Dim Detail As Boolean = False
        If MessageBox.Show("Do you want Detailed Stock Transfer Print?", "Stock Transfer Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Detail = True
        End If
        Dim vDtTranTitle1 As New BrightPosting.GDatatable
        vDtTranTitle1.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle1.Columns.Add("DES1", GetType(String))
        Dim Ro As BrightPosting.GDataRow = Nothing
        Ro = vDtTranTitle1.NewRow
        Ro.Item("DES1") = cnCompanyName
        vDtTranTitle1.Rows.Add(Ro)
        vDtTranTitle1.pRowHeight = 21
        vDtTranTitle1.pColHeaderVisible = False
        vDtTranTitle1.pCellBorder = False
        vDtTranTitle1.pContentFont = New Font("VERDANA", 12, FontStyle.Bold)

        Dim vDtTranTitle2 As New BrightPosting.GDatatable
        vDtTranTitle2.pTableContentAlignment = StringAlignment.Center
        vDtTranTitle2.Columns.Add("DES1", GetType(String))


        Ro = vDtTranTitle2.NewRow
        Ro.Item("DES1") = "STOCK TRANSFER FROM " & cnCostName & " TO " & tCostName
        vDtTranTitle2.Rows.Add(Ro)
        vDtTranTitle2.pRowHeight = 21
        vDtTranTitle2.pColHeaderVisible = False
        vDtTranTitle2.pCellBorder = False
        vDtTranTitle2.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        Dim vDtTranTitle3 As New BrightPosting.GDatatable
        vDtTranTitle3.pTableContentAlignment = StringAlignment.Near
        vDtTranTitle3.Columns.Add("DES1", GetType(String))
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "USER NAME : " & UserName
        vDtTranTitle3.Rows.Add(Ro)
        Ro = vDtTranTitle3.NewRow
        Ro.Item("DES1") = "TRASFER NO : " & RefNo & " DATE : " & RefDate.ToString("dd/MM/yyyy") & IIf(UpTime <> "", " " & UpTime, "")
        vDtTranTitle3.Rows.Add(Ro)
        vDtTranTitle3.pRowHeight = 21
        vDtTranTitle3.pColHeaderVisible = False
        vDtTranTitle3.pCellBorder = False
        vDtTranTitle3.pContentFont = New Font("verdana", 10, FontStyle.Bold)

        Dim vDtTranInfo1 As New BrightPosting.GDatatable
        Dim vDtTranINfo2 As New BrightPosting.GDatatable
        If Detail Then
            strSql = vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN RATE <> 0 THEN T.RATE ELSE NULL END) AS RATE"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN T.SALEMODE IN ('R','F') AND T.SALVALUE <> 0 THEN T.SALVALUE ELSE NULL END) AS SALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(vDtTranInfo1)
            vDtTranInfo1.Columns("ITEM").Caption = 150
            vDtTranInfo1.Columns("TAGNO").Caption = 70
            vDtTranInfo1.Columns("PCS").Caption = 60
            vDtTranInfo1.Columns("GRSWT").Caption = 70
            vDtTranInfo1.Columns("NETWT").Caption = 70
            vDtTranInfo1.Columns("DIAPCS").Caption = 70
            vDtTranInfo1.Columns("DIAWT").Caption = 80
            vDtTranInfo1.Columns("RATE").Caption = 70
            vDtTranInfo1.Columns("SALVALUE").Caption = 90
            vDtTranInfo1.pRowHeight = 21
            vDtTranInfo1.pColHeaderVisible = True
            vDtTranInfo1.pCellBorder = True
            vDtTranInfo1.pCellBorderColor = Pens.LightGray
            vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

            vDtTranINfo2 = vDtTranInfo1.Clone
            Ro = vDtTranINfo2.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
            Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
            Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
            Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
            Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
            Ro.Item("SALVALUE") = Val(vDtTranInfo1.Compute("SUM(SALVALUE)", String.Empty).ToString)
            vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
            vDtTranINfo2.Rows.Add(Ro)
            vDtTranINfo2.pRowHeight = 21
            vDtTranINfo2.pColHeaderVisible = False
            vDtTranINfo2.pCellBorder = True
            vDtTranINfo2.pCellBorderColor = Pens.LightGray
            vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
        Else
            strSql = vbCrLf + "  SELECT ITEM,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT IM.ITEMNAME AS ITEM,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  WHERE T.REFNO = '" & RefNo & "' AND T.REFDATE = '" & RefDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  )X "
            strSql += vbCrLf + "  GROUP BY ITEM"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(vDtTranInfo1)
            vDtTranInfo1.Columns("ITEM").Caption = 200
            vDtTranInfo1.Columns("PCS").Caption = 60
            vDtTranInfo1.Columns("GRSWT").Caption = 80
            vDtTranInfo1.Columns("NETWT").Caption = 80
            vDtTranInfo1.Columns("DIAPCS").Caption = 60
            vDtTranInfo1.Columns("DIAWT").Caption = 80
            vDtTranInfo1.pRowHeight = 21
            vDtTranInfo1.pColHeaderVisible = True
            vDtTranInfo1.pCellBorder = True
            vDtTranInfo1.pCellBorderColor = Pens.LightGray
            vDtTranInfo1.pContentFont = New Font("verdana", 8, FontStyle.Regular)

            vDtTranINfo2 = vDtTranInfo1.Clone
            Ro = vDtTranINfo2.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("PCS") = Val(vDtTranInfo1.Compute("SUM(PCS)", String.Empty).ToString)
            Ro.Item("GRSWT") = Val(vDtTranInfo1.Compute("SUM(GRSWT)", String.Empty).ToString)
            Ro.Item("NETWT") = Val(vDtTranInfo1.Compute("SUM(NETWT)", String.Empty).ToString)
            Ro.Item("DIAPCS") = Val(vDtTranInfo1.Compute("SUM(DIAPCS)", String.Empty).ToString)
            Ro.Item("DIAWT") = Val(vDtTranInfo1.Compute("SUM(DIAWT)", String.Empty).ToString)
            vDtTranINfo2.pTableBackColor = Color.LightGoldenrodYellow
            vDtTranINfo2.Rows.Add(Ro)
            vDtTranINfo2.pRowHeight = 21
            vDtTranINfo2.pColHeaderVisible = False
            vDtTranINfo2.pCellBorder = True
            vDtTranINfo2.pCellBorderColor = Pens.LightGray
            vDtTranINfo2.pContentFont = New Font("verdana", 8, FontStyle.Bold)
        End If

        Dim vDtTranFooter1 As New BrightPosting.GDatatable
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Near
        vDtTranFooter1.Columns.Add("DES1", GetType(String))
        vDtTranFooter1.Columns.Add("DES2", GetType(String))
        vDtTranFooter1.Columns.Add("DES3", GetType(String))
        vDtTranFooter1.Rows.Add()

        Ro = vDtTranFooter1.NewRow
        Ro.Item("DES1") = "TRANSFERED BY"
        Ro.Item("DES2") = "TRANSIT BY"
        Ro.Item("DES3") = "RECEIVED BY"
        'vDtTranFooter1.Columns("DES1").Caption = 400
        'vDtTranFooter1.Columns("DES2").Caption = 100
        vDtTranFooter1.Rows.Add(Ro)
        vDtTranFooter1.pRowHeight = 21
        vDtTranFooter1.pColHeaderVisible = False
        vDtTranFooter1.pCellBorder = False
        vDtTranFooter1.pContentFont = New Font("verdana", 10, FontStyle.Bold)
        vDtTranFooter1.pTableContentAlignment = StringAlignment.Center

        Dim lstSource As New List(Of Object)
        lstSource.Clear()
        lstSource.Add(vDtTranTitle1)
        lstSource.Add(vDtTranTitle2)
        lstSource.Add(vDtTranTitle3)
        lstSource.Add(vDtTranInfo1)
        lstSource.Add(vDtTranINfo2)
        lstSource.Add(vDtTranFooter1)
        Dim obj As New BrightPosting.GListPrinter(lstSource)
        obj.Print()
    End Sub
    Public Function Budgetcheck(ByVal accode As String, ByVal trandate As DateTime, Optional ByVal tranmode As String = "") As String
        Dim bal As Double
        Dim CREDIT As Double
        Dim retval As String
        retval = ""
        strSql = " SELECT isnull(sum(CASE WHEN TRANMODE='D'THEN isnull(AMOUNT,0) ELSE -1* isnull(AMOUNT,0)  END),0) FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE='" & accode & "'"
        bal = Val(GetSqlValue(cn, strSql).ToString)
        strSql = "DECLARE @GIVENDATE SMALLDATETIME"
        strSql += vbCrLf + "SELECT @GIVENDATE='" & Format(trandate, "yyyy-MM-dd") & "'"
        strSql += "SELECT  BUDVALUE,TRANMODE FROM " & cnStockDb & "..BUDGETCONTROL WHERE ACCODE='" & accode & "'"
        strSql += vbCrLf + " AND @GIVENDATE>=BUDEFFFROM AND @GIVENDATE<=BUDEFFTO"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If bal < 0 And dt.Rows(0).Item("TRANMODE") = "C" Then
                retval = (Val(dt.Rows(0).Item("BUDVALUE").ToString) + bal).ToString + " Cr"
            ElseIf bal > 0 And dt.Rows(0).Item("TRANMODE") = "D" Then

                retval = (Val(dt.Rows(0).Item("BUDVALUE").ToString) - bal).ToString + " Dr"

            End If
        End If
        Return retval
    End Function
    'Public Function GlbCalcMaxMinValues(ByVal Type As String, ByVal wt As Decimal, ByVal CostCentre As String, _
    '    Optional ByVal TableCode As String = "", _
    '    Optional ByVal Item As String = "", Optional ByVal SubItem As String = "", _
    '    Optional ByVal Designer As String = "", _
    '    Optional ByVal ItemType As String = "" _
    '    ) As String
    '    Select Case Type
    '        Case "T"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE TABLECODE = '" & TableCode & "'"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "I"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')"
    '            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')),0)"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "D"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')"
    '            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item & "')),0)"
    '            strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer & "'),0)"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '        Case "P"
    '            strSql = " DECLARE @WT FLOAT"
    '            strSql += vbCrLf + " SET @WT = " & wt & ""
    '            strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
    '            strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
    '            strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & ItemType & "')"
    '            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
    '            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
    '            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CostCentre & "'),'')"
    '    End Select
    'End Function

    Public Function GetuserPwd(ByVal Optid As Integer, ByVal xCostId As String, Optional ByVal xUserid As Integer = Nothing, Optional ByVal xCtrid As Integer = 0) As Integer

        Dim Sqlqry As String = "Select Pwdid from " & cnAdminDb & "..PWDMASTER where PWDOPTIONID =" & Optid & " AND PWDSTATUS NOT IN('C','E')"
        If xCostId <> "" Then Sqlqry += " AND COSTID = '" & xCostId & "'"
        If xUserid <> 0 Then Sqlqry += " AND PWDUSERID = " & xUserid
        Sqlqry += " AND PWDDATE='" & GetEntryDate(GetServerDate) & "'"
        'If xCtrid <> 0 Then Sqlqry += " AND PWDCOUNTERID= " & xCtrid
        Return Val(objGPack.GetSqlValue(Sqlqry, , , tran))
    End Function

    Public Sub LogonServer()
        Dim ServerSystemName As String = GetAdmindbSoftValue("SERVER_NAME")
        Dim ServerAdminName As String = GetAdmindbSoftValue("SERVER_ACCNAME")
        Dim ServerPwd As String = GetAdmindbSoftValue("SERVER_ACCPWD")
        If ServerSystemName = "" Or ServerAdminName = "" Or ServerPwd = "" Then
            Exit Sub
        End If
        ServerPwd = BrighttechPack.Methods.Decrypt(ServerPwd)
        Dim admin_token As IntPtr
        Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim wid_admin As WindowsIdentity = Nothing
        Dim wic As WindowsImpersonationContext = Nothing
        If LogonUser(ServerAdminName, ServerSystemName, ServerPwd, IIf(My.Computer.Name.ToUpper = ServerSystemName, 3, 9), 0, admin_token) <> 0 Then '3
            wid_admin = New WindowsIdentity(admin_token)
            wic = wid_admin.Impersonate()
        End If
    End Sub

    Public Enum Modules
        Stock = 0
        Bill = 1
        Estimation = 2
        Accounts = 3
        OrderRepair = 4
        SavingsScheme = 5
        StoreManagement = 6
    End Enum

    Public Enum SyncMode
        Master = 0
        Stock = 1
        Transaction = 2
    End Enum

    Public Function checkEmailId(ByVal txt As TextBox) As Boolean
        Dim pattern As String = "^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$"
        Dim match As System.Text.RegularExpressions.Match
        match = System.Text.RegularExpressions.Regex.Match(txt.Text.Trim(), pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Not match.Success Then
            MsgBox("Enter valid Email address")
            txt.Focus()
            Return False
        End If
        Return True
    End Function

    Public Enum TranSnoType
        ISSUECODE = 0
        ISSSTONECODE = 1
        ISSMISCCODE = 2
        OUTSTANDINGCODE = 3
        ACCTRANCODE = 4
        RECEIPTCODE = 5
        RECEIPTSTONECODE = 6
        RECEIPTMISCCODE = 7
        PERSONALINFOCODE = 8
        REQITEMCODE = 9
        OPENWEIGHTCODE = 10
        OPENITEMCODE = 11
        CHEQUEBOOKCODE = 12
        ESTISSUECODE = 13
        ESTISSSTONECODE = 14
        ESTISSMISCCODE = 15
        ESTRECEIPTCODE = 16
        ESTRECEIPTSTONECODE = 17
        ESTPERSONALINFOCODE = 18
        ISSMETALCODE = 19
        RECEIPTMETALCODE = 20
        ESTISSMETALCODE = 21
        ESTRECEIPTMISCCODE = 22
        ESTRECEIPTMETALCODE = 23
        ITEMLOTCODE = 24
        ITEMDETAILCODE = 25
        CTRANSFERCODE = 26
        ORMASTCODE = 27
        ORSTONECODE = 28
        ORSAMPLECODE = 29
        ORIRDETAILCODE = 30
        ITEMTAGCODE = 31
        ITEMTAGSTONECODE = 32
        ITEMTAGMISCCHARCODE = 33
        ITEMTAGMETALCODE = 34
        ITEMNONTAGCODE = 35
        ITEMNONTAGSTONECODE = 36
        TISSUECODE = 37
        TRECEIPTCODE = 38
        TACCTRANCODE = 39
        TOUTSTANDINGCODE = 40
        TISSSTONECODE = 41
        TISSMISCCODE = 42
        TRECEIPTSTONECODE = 43
        TRECEIPTMISCCODE = 44
        BRSINFOCODE = 45
        CHQPRINT_FORMAT = 46
        STKREORDERCODE = 47
        BRS_ACCTRANCODE = 48
        GVTRANCODE = 49
        STOCK_CHKREPORT = 57
        PMTRANCODE = 58
        WITEMTAGCODE = 59
        WITEMTAGSTONECODE = 60
        MIMRRFIDCODE = 61
        PRIVILEGETRANCODE = 62
        MR_ORDERCODE = 63
        MR_ORDERSTONECODE = 64
        ADJTRANCODE = 65
        TAXTRANCODE = 66
        ESTTAXTRANCODE = 67
        GSTREGISTERCODE = 68
    End Enum

    Public Enum ListControl
        Combo = 0
        ListBox = 1
    End Enum
    Public Sub ColorChange(ByVal ctrl As Object, ByVal fClr As Color, ByVal frmColor As Color)
        For Each c As Object In ctrl.Controls
            If TypeOf c Is CodeVendor.Controls.Grouper Then
                CType(c, CodeVendor.Controls.Grouper).BackgroundColor = fClr
                CType(c, CodeVendor.Controls.Grouper).BackgroundGradientColor = fClr
                CType(c, CodeVendor.Controls.Grouper).BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
                ColorChange(c, fClr, frmColor)
            ElseIf CType(c, Control).Controls.Count > 0 Then
                CType(c, Control).BackColor = frmColor
                ColorChange(c, fClr, frmColor)
            End If
        Next
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
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(i).Resizable = DataGridViewTriState.False 
            Next
        End With
    End Sub
    Function GetRate_Purity(ByVal DDate As Date, ByVal PURITYID As String, Optional ByVal tran As OleDbTransaction = Nothing) As String
        Dim rate As Double = Nothing
        Dim sql As String = Nothing
        sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
        sql += vbCrLf + "  AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        sql += vbCrLf + "  AND METALID = "
        sql += vbCrLf + "  (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "      WHERE PURITYID = '" & PURITYID & "')"
        sql += vbCrLf + "  AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "      WHERE PURITYID = '" & PURITYID & "')"
        sql += vbCrLf + "  ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(sql, , , tran))
        Return rate.ToString
    End Function

    Function GetRate(ByVal DDate As Date, ByVal CatCode As String, Optional ByVal tran As OleDbTransaction = Nothing) As String
        Dim rate As Double = Nothing
        Dim sql As String = Nothing
        sql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        sql += vbCrLf + "  WHERE RDATE = '" & DDate.ToString("yyyy-MM-dd") & "'"
        sql += vbCrLf + "  AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        sql += vbCrLf + " AND METALID = "
        sql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        sql += vbCrLf + "     WHERE CATCODE = '" & CatCode & "')"
        sql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST "
        sql += vbCrLf + "     WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
        sql += vbCrLf + "        WHERE CATCODE = '" & CatCode & "'))"
        sql += vbCrLf + " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(sql, , , tran))
        Return rate.ToString
    End Function

    'Function funcGetAdminDbTableId(ByVal ctlId As String, ByRef tran As OleDbTransaction) As String
    '    Dim Sql As String = Nothing
    '    Dim dtCode As New DataTable
    '    Dim cmd As OleDbCommand
    '    Dim code As String = Nothing
    '    Sql = "SELECT ISNULL(MAX(CTLTEXT),0)+1 AS CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL "
    '    Sql += " WHERE CTLID = '" & ctlId & "'"
    '    cmd = New OleDbCommand(Sql, cn, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(dtCode)
    '    If dtCode.Rows.Count > 0 Then
    '        code = Val(dtCode.Rows(0).Item("CTLTEXT").ToString)
    '        Sql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & code & "' WHERE CTLID = '" & ctlId & "'"
    '        cmd = New OleDbCommand(Sql, cn, tran)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    Return code
    'End Function
    'Function funcGetTranDbName(ByVal cmpId As String, ByVal strcat As String, ByVal tranDate As Date) As String
    '    Dim dbName As String = Nothing
    '    Dim frmYear As String
    '    Dim toYear As String
    '    Dim month As Integer = tranDate.Month
    '    If month > 3 Then
    '        frmYear = Mid(tranDate.Year.ToString, 3, 2)
    '    Else
    '        frmYear = Mid((tranDate.Year - 1).ToString, 3, 2)
    '    End If
    '    toYear = Val(frmYear) + 1
    '    If toYear < 10 Then
    '        toYear = "0" + toYear
    '    End If
    '    dbName = cmpId + strcat + frmYear + toYear
    '    Return dbName
    'End Function

    Function funcGridStyle(ByVal obj As DataGridView) As Integer
        obj.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window
        obj.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText
        obj.RowTemplate.Resizable = DataGridViewTriState.False
        obj.RowHeadersVisible = False
        obj.BackgroundColor = grdBackGroundColor
        obj.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        obj.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        obj.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        obj.RowTemplate.Height = 18
        obj.Font = New Font("VERDANA", 8, FontStyle.Regular)
    End Function
    'Function funcGetBoardRate(ByVal Billdate As Date, Optional ByVal tran As OleDbTransaction = Nothing) As String
    '    Dim strsql As String
    '    Dim cmd As OleDbCommand
    '    Dim da As OleDbDataAdapter
    '    Dim rate As Double = Nothing
    '    strsql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST"
    '    strsql += " WHERE RDATE = '" & Billdate.Date.ToString("yyyy-MM-dd") & "'"
    '    strsql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = '" & Billdate.Date.ToString("yyyy-MM-dd") & "')"
    '    strsql += " ORDER BY PURITY DESC"
    '    If tran Is Nothing Then
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Else
    '        cmd = New OleDbCommand(strsql, cn, tran)
    '    End If
    '    da = New OleDbDataAdapter(cmd)
    '    Dim dt As New DataTable
    '    da.Fill(dt)
    '    If dt.Rows.Count > 0 Then
    '        rate = Val(dt.Rows(0).Item("SRATE").ToString)
    '    End If
    '    Return IIf(rate > 0, Format(rate, "0.00"), "")
    'End Function
    'Function objGPack.DupCheck(ByVal qry As String, Optional ByVal TRAN As OleDbTransaction = Nothing) As Boolean
    '    Dim dt As New DataTable
    '    Dim cmd As OleDbCommand = Nothing
    '    If TRAN Is Nothing Then
    '        da = New OleDbDataAdapter(qry, cn)
    '    Else
    '        cmd = New OleDbCommand(qry, cn, TRAN)
    '        da = New OleDbDataAdapter(cmd)
    '    End If
    '    da.Fill(dt)
    '    If dt.Rows.Count > 0 Then
    '        Return True
    '    End If
    '    Return False
    'End Function
    Function funcCheckCostCentreStatusFalse() As Boolean
        Dim strSql As String = Nothing
        Dim dt As New DataTable
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return True
        End If
    End Function
    Function funcFiltVatExm(ByVal FiltVatExm As String) As String
        Dim str As String = Nothing
        If FiltVatExm = "F1" Then
            str += " "
        ElseIf FiltVatExm = "F2" Then
            str += " AND VATEXM='Y'"
        Else
            str += " AND VATEXM='N'"
        End If
        Return str
    End Function
    Function funcSampleActivate() As String
        Dim strSampleActivate As String = Nothing
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='SAMPLEACTIVATE'", "CTLTEXT", "N")) = "Y" Then
            strSampleActivate = "Y"
        Else
            strSampleActivate = "N"
        End If
        Return strSampleActivate
    End Function
    Function funcOpenGrid(ByVal str As String, ByVal grid As DataGridView) As Integer
        Dim da As New OleDbDataAdapter
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        grid.DataSource = dt
    End Function
    Function funcSetNumberStyle(ByVal num As String, ByVal maxDigit As Integer) As String
        Dim temp As String = Nothing
        For cnt As Integer = 1 To maxDigit - num.Length
            temp += "0"
        Next
        num = temp + num
        Return num
    End Function
    'Function funcGetTrandbTableId(ByVal ctlId As String, ByRef tran As OleDbTransaction) As Integer
    '    Dim Sql As String = Nothing
    '    Dim dtCode As New DataTable
    '    Dim cmd As OleDbCommand
    '    Dim code As Integer = Nothing
    '    Sql = "SELECT ISNULL(MAX(CTLTEXT),0)+1 AS CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN "
    '    Sql += " WHERE CTLID = '" & ctlId & "'"
    '    cmd = New OleDbCommand(Sql, cn, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(dtCode)
    '    If dtCode.Rows.Count > 0 Then
    '        code = Val(dtCode.Rows(0).Item("CTLTEXT").ToString)
    '        Sql = " UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT = '" & code & "' WHERE CTLID = '" & ctlId & "' AND CTLMODULE = 'X'"
    '        cmd = New OleDbCommand(Sql, cn, tran)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    Return code
    'End Function
    Private Function GetListStr(ByVal list As List(Of String)) As String
        Dim _RetStr As String = Nothing
        For cnt As Integer = 0 To list.Count - 1
            _RetStr += "'" & list(cnt).ToUpper & "'"
            If cnt <> list.Count - 1 Then _RetStr += ","
        Next
        Return _RetStr
    End Function
    Public Function DeleteItem(ByVal mode As SyncMode, ByVal colName As List(Of String) _
    , ByVal delQry As String, ByVal checkStr As String _
    , ByVal neglectTable As String) As Boolean
        strSql = " SELECT CNAME "
        strSql += " ,'" & cnAdminDb & "..' AS DBNAME,(SELECT NAME FROM " & cnAdminDb & "..SYSOBJECTS WHERE ID = T.ID)AS TNAME"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT NAME CNAME,ID FROM " & cnAdminDb & "..SYSCOLUMNS WHERE NAME IN ("
        strSql += GetListStr(colName)
        strSql += " )"
        strSql += " )T"
        strSql += " UNION ALL"
        strSql += " SELECT CNAME "
        strSql += " ,'" & cnStockDb & "..' AS DBNAME,(SELECT NAME FROM " & cnStockDb & "..SYSOBJECTS WHERE ID = T.ID)AS TNAME"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT NAME CNAME,ID FROM " & cnStockDb & "..SYSCOLUMNS WHERE NAME IN ("
        strSql += GetListStr(colName)
        strSql += " )"
        strSql += " )T"
        Dim _DtTableColl As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(_DtTableColl)
        Dim _chkQry As String = Nothing
        For cnt As Integer = 0 To _DtTableColl.Rows.Count - 1
            If _DtTableColl.Rows(cnt).Item("TNAME").ToString.ToUpper = neglectTable.ToUpper Then Continue For
            _chkQry += " SELECT DISTINCT " & _DtTableColl.Rows(cnt).Item("CNAME").ToString
            _chkQry += " FROM " & _DtTableColl.Rows(cnt).Item("DBNAME").ToString _
                        & _DtTableColl.Rows(cnt).Item("TNAME").ToString + vbCrLf
            _chkQry += " WHERE " & _DtTableColl.Rows(cnt).Item("CNAME").ToString & " = "
            _chkQry += " '" & checkStr & "'"
            If cnt <> _DtTableColl.Rows.Count - 1 Then _chkQry += " UNION ALL"
        Next
        If _chkQry.EndsWith("UNION ALL") Then
            _chkQry = Left(_chkQry, _chkQry.Length - 9)
        End If
        If _chkQry <> Nothing Then Return DeleteItem(mode, _chkQry, delQry)
    End Function
    Public Function DeleteItem(ByVal type As SyncMode, ByVal checkQry As String, ByVal delQry As String) As Boolean
        Try
            '' ProgressBarShow()
            '' ProgressBarStep("Availability Checking..", 20)
            If objGPack.DupCheck(checkQry) Then
                '' ProgressBarHide()
                MsgBox("This Item Already Used. So Cannot Delete this Item")
            Else
                ''ProgressBarHide()
                If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
                    ''ProgressBarShow()
                    ''ProgressBarStep("Deleting..", 20)
                    Try
                        tran = Nothing
                        tran = cn.BeginTransaction
                        'ExecQuery(type, delQry, cn, tran, , True)
                        ExecQuery(type, delQry, cn, tran, , , , , True)
                        tran.Commit()
                        tran = Nothing
                        MsgBox("Successfully Deleted..")
                        Return True
                    Catch ex As Exception
                        If tran IsNot Nothing Then tran.Rollback()
                        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    End Try
                End If
            End If
        Catch ex As Exception
            MsgBox("Message : " + ex.Message + vbCrLf + "Stack Trace    : " + ex.StackTrace)
        Finally
            ''ProgressBarHide()
        End Try
    End Function
    Public Function IsActive(ByVal ctrl As Control) As Boolean
        If TypeOf (ctrl) Is DataGridView Then
            If ctrl.Focused Then Return True
        Else
            For cnt As Integer = 0 To ctrl.Controls.Count - 1
                If TypeOf (ctrl.Controls(cnt)) Is GroupBox Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is Panel Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is SplitContainer Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is TabControl Then
                    Return IsActive(ctrl.Controls(cnt))
                ElseIf TypeOf (ctrl.Controls(cnt)) Is TabPage Then
                    Return IsActive(ctrl.Controls(cnt))
                Else
                    If ctrl.Controls(cnt).Focused Then Return True
                    'Else
                    '    If ctrl.Focused Then Return True
                End If
            Next
        End If
    End Function

    Public Sub ComboScript(ByRef cmb As ComboBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = cmb.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (cmb.Items.Count - 1)
                If UCase((sTempString & Mid$(cmb.Items.Item(iLoop),
                  Len(sTempString) + 1))) = UCase(cmb.Items.Item(iLoop)) Then
                    cmb.SelectedIndex = iLoop
                    cmb.Text = cmb.Items.Item(iLoop)
                    cmb.SelectionStart = Len(sTempString)
                    cmb.SelectionLength = Len(cmb.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        cmb.Text = sComboText
                        cmb.SelectionStart = Len(cmb.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    '    Public Function GetEstNoValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction) As Integer
    'GetNewBillNo:
    '        Dim NewBillNo As Integer = Nothing
    '        strSql = " SELECT 'CHECK' FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTBILLNO'"
    '        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
    '            strSql = " SELECT 'CHECK' FROM TEMP" & systemId & "ESTBILLNO WHERE CTLID = '" & BillControlId & "'"
    '            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
    '                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM TEMP" & systemId & "ESTBILLNO WHERE CTLID = '" & BillControlId & "'"
    '                NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
    '            Else 'NEWBILLNO GENERATING HERE
    '                GoTo GenerateNewBillNo
    '            End If
    '        Else 'TEMPBILLNO NOT EXIST
    'GenerateNewBillNo:
    '            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & _MainCompId & "'"
    '            NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
    '            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
    '            strSql += " WHERE CTLID ='" & BillControlId & "' AND COMPANYID = '" & _MainCompId & "'"
    '            strSql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
    '            cmd = New OleDbCommand(strSql, cn, tran)
    '            If cmd.ExecuteNonQuery() = 0 Then
    '                GoTo GetNewBillNo
    '            End If
    '            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTNO')>0"
    '            strSql += " 	BEGIN"
    '            strSql += " 	INSERT INTO TEMP" & systemId & "ESTNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & NewBillNo + 1 & "'CTLTEXT"
    '            strSql += " 	END"
    '            strSql += " ELSE"
    '            strSql += " 	BEGIN "
    '            strSql += " 	CREATE TABLE TEMP" & systemId & "ESTNO(SNO INT IDENTITY(1,1),CTLID VARCHAR(50),CTLTEXT VARCHAR(200))"
    '            strSql += " 	INSERT INTO TEMP" & systemId & "ESTNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & NewBillNo + 1 & "'CTLTEXT"
    '            strSql += " 	END"
    '            cmd = New OleDbCommand(strSql, cn, tran)
    '            cmd.ExecuteNonQuery()
    '        End If
    '        Return NewBillNo + 1
    '    End Function

    'Public Sub InsertBillNo(ByVal BillControlId As String, ByVal BillNo As Integer, ByVal tran As OleDbTransaction)
    '    strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')>0"
    '    strSql += " 	BEGIN"
    '    strSql += " 	INSERT INTO TEMP" & systemId & "BILLNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & BillNo & "'CTLTEXT"
    '    strSql += " 	END"
    '    strSql += " ELSE"
    '    strSql += " 	BEGIN "
    '    strSql += " 	CREATE TABLE TEMP" & systemId & "BILLNO(SNO INT IDENTITY(1,1),CTLID VARCHAR(50),CTLTEXT VARCHAR(200))"
    '    strSql += " 	INSERT INTO TEMP" & systemId & "BILLNO(CTLID,CTLTEXT)SELECT '" & BillControlId & "' CTLID,'" & BillNo & "'CTLTEXT"
    '    strSql += " 	END"
    '    cmd = New OleDbCommand(strSql, cn, tran)
    '    cmd.ExecuteNonQuery()
    'End Sub


    '    Public Function GetBillNoValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction) As Integer
    'GetNewBillNo:
    '        Dim NewBillNo As Integer = Nothing
    '        strSql = " SELECT 'CHECK' FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO'"
    '        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
    '            strSql = " SELECT 'CHECK' FROM TEMP" & systemId & "BILLNO WHERE CTLID = '" & BillControlId & "'"
    '            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
    '                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM TEMP" & systemId & "BILLNO WHERE CTLID = '" & BillControlId & "'"
    '                NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
    '                Return NewBillNo
    '            Else 'NEWBILLNO GENERATING HERE
    '                GoTo GenerateNewBillNo
    '            End If
    '        Else 'TEMPBILLNO NOT EXIST
    'GenerateNewBillNo:
    '            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'"
    '            NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
    '            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
    '            strSql += " WHERE CTLID ='" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'"
    '            strSql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
    '            cmd = New OleDbCommand(strSql, cn, tran)
    '            If cmd.ExecuteNonQuery() = 0 Then
    '                GoTo GetNewBillNo
    '            End If
    '            InsertBillNo(BillControlId, NewBillNo + 1, tran)
    '            Return NewBillNo + 1
    '        End If
    '    End Function

    Public Function GetBillControlValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & strCompanyId & "'", , , tran))
    End Function

    Public Sub InsertIntoBillControl(
      ByVal ctlId As String, ByVal ctlName As String, ByVal ctlType As String _
      , ByVal ctlMode As String, ByVal ctlText As String, ByVal ctlModule As String, Optional ByVal TRAN As OleDbTransaction = Nothing)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT 1 FROM " & cnStockDb & "..TBILLCONTROL WHERE CTLID = '" & ctlId & "' AND COMPANYID = '" & strCompanyId & "'"
        If TRAN Is Nothing Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Else
            cmd = New OleDbCommand(strSql, cn, TRAN)
        End If
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Exit Sub
        End If
        strSql = " INSERT INTO " & cnStockDb & "..TBILLCONTROL"
        strSql += " (CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODE,CTLMODULE,COMPANYID)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & ctlId & "','" & ctlName & "','" & ctlType & "'"
        strSql += " ,'" & ctlText & "','" & ctlMode & "','" & ctlModule & "'"
        strSql += " ,'" & strCompanyId & "'"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If TRAN IsNot Nothing Then cmd.Transaction = TRAN
        cmd.ExecuteNonQuery()
    End Sub

    'Public Sub TextScript(ByRef txt As TextBox, ByRef lstSearch As ListBox, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Dim sComboText As String = ""
    '    Dim iLoop As Integer
    '    Dim sTempString As String
    '    If e.KeyCode >= 65 And e.KeyCode <= 90 Then
    '        'only look at letters A-Z
    '        sTempString = txt.Text
    '        If Len(sTempString) = 1 Then sComboText = sTempString
    '        For iLoop = 0 To (lstSearch.Items.Count - 1)
    '            If UCase((sTempString & Mid$(lstSearch.Items.Item(iLoop), _
    '              Len(sTempString) + 1))) = UCase(lstSearch.Items.Item(iLoop)) Then
    '                lstSearch.SelectedIndex = iLoop
    '                txt.Text = lstSearch.Items.Item(iLoop)
    '                txt.SelectionStart = Len(sTempString)
    '                txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
    '                sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
    '                Exit For
    '            Else
    '                If InStr(UCase(sTempString), UCase(sComboText)) Then
    '                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
    '                    + 1)
    '                    txt.Text = sComboText
    '                    txt.SelectionStart = Len(txt.Text)
    '                Else
    '                    sComboText = sTempString
    '                End If
    '            End If
    '        Next iLoop
    '    End If
    'End Sub

    Public Sub TextScript(ByRef txt As TextBox, ByRef lstSearch As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        Dim cmblst As Object = Nothing
        If TypeOf lstSearch Is ComboBox Then
            cmblst = New ComboBox
            cmblst = lstSearch
        ElseIf TypeOf lstSearch Is ListBox Then
            cmblst = New ListBox
            cmblst = lstSearch
        End If
        'CType(lstSearch.items.item(0), DataRowView).Item(0).ToString
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = txt.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (cmblst.Items.Count - 1)
                'If UCase((sTempString & Mid$(CType(lstSearch.items.item(0), DataRowView).Item(0).ToString, _
                'Len(sTempString) + 1))) = UCase(CType(lstSearch.items.item(0), DataRowView).Item(0).ToString) Then
                '    lstSearch.SelectedIndex = iLoop
                '    txt.Text = lstSearch.Items.Item(iLoop)
                '    txt.SelectionStart = Len(sTempString)
                '    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                '    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                '    Exit For
                'Else
                '    If InStr(UCase(sTempString), UCase(sComboText)) Then
                '        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                '        + 1)
                '        txt.Text = sComboText
                '        txt.SelectionStart = Len(txt.Text)
                '    Else
                '        sComboText = sTempString
                '    End If
                'End If

                If UCase((sTempString & Mid$(cmblst.Items.Item(iLoop),
                  Len(sTempString) + 1))) = UCase(cmblst.Items.Item(iLoop)) Then
                    cmblst.SelectedIndex = iLoop
                    txt.Text = cmblst.Items.Item(iLoop)
                    txt.SelectionStart = Len(sTempString)
                    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        txt.Text = sComboText
                        txt.SelectionStart = Len(txt.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    Function funcGridViewGrouping(ByVal dt As DataTable, Optional ByVal SourceCol As Integer = 0, Optional ByVal DestCol As Integer = 1, Optional ByVal WithStoneField As Boolean = False, Optional ByVal sortFieldAlter As Boolean = False, Optional ByVal secondDestCol As Integer = -1, Optional ByVal RESULT As Boolean = False) As DataSet
        Dim column1ToString As String = "#@$^&*@#!@#$#@!"
        Dim column2ToString As String = "##%&%@#!!@%#$#!"
        Dim ds As New DataSet
        ds.Clear()
        Dim subTotalDt As New DataTable("SubTotal")
        Dim titleDt As New DataTable("Title")
        subTotalDt.Clear()
        titleDt.Clear()
        titleDt.Columns.Add("Title")
        subTotalDt.Columns.Add("SubTotal")

        Dim tempDt As New DataTable("Result")
        tempDt.Clear()
        tempDt.Columns.Add("PARTICULAR", GetType(String))
        For cnt As Integer = 0 To dt.Columns.Count - 1
            tempDt.Columns.Add(dt.Columns(cnt).ColumnName, GetType(String))
        Next
        Dim ro As DataRow = Nothing
        Dim roSubTotal As DataRow = Nothing
        Dim roTitle As DataRow = Nothing
        Select Case secondDestCol
            Case -1

            Case Is <> -1

        End Select
        For rowIndex As Integer = 0 To dt.Rows.Count - 1
            ro = tempDt.NewRow
            With dt.Rows(rowIndex)
                If RESULT = True Then
                    If .Item("RESULT").ToString = "2" Then
                        .Item(SourceCol) = "SUB TOTAL"
                    End If
                    If .Item("RESULT").ToString = "3" Then
                        .Item(SourceCol) = "GRAND TOTAL"
                    End If
                End If
                If .Item(SourceCol).ToString <> "SUB TOTAL" And .Item(SourceCol).ToString <> "GRAND TOTAL" And .Item(DestCol).ToString <> "SUB TOTAL" And .Item(DestCol).ToString <> "GRAND TOTAL" Then
                    If column1ToString <> .Item(SourceCol).ToString Then
                        If WithStoneField = True Then
                            If .Item("Stone").ToString <> "2" Then
                                ro(0) = .Item(SourceCol).ToString
                                For cnt As Integer = 1 To dt.Columns.Count - 1
                                    ro(cnt) = ""
                                Next
                                column1ToString = .Item(SourceCol).ToString
                                tempDt.Rows.Add(ro)
                                ''Adding Title Index
                                roTitle = titleDt.NewRow
                                roTitle("Title") = rowIndex + titleDt.Rows.Count
                                titleDt.Rows.Add(roTitle)
                            End If
                        Else
                            ro(0) = .Item(SourceCol).ToString
                            For cnt As Integer = 1 To dt.Columns.Count - 1
                                ro(cnt) = ""
                            Next
                            column1ToString = .Item(SourceCol).ToString
                            tempDt.Rows.Add(ro)
                            ''Adding Title Index
                            roTitle = titleDt.NewRow
                            roTitle("Title") = rowIndex + titleDt.Rows.Count
                            titleDt.Rows.Add(roTitle)
                        End If
                    End If

                End If
                If .Item(SourceCol).ToString = "SUB TOTAL" Or .Item(DestCol).ToString = "SUB TOTAL" Then
                    ''Adding Group SubTotal Index into SubTotal Table
                    roSubTotal = subTotalDt.NewRow
                    roSubTotal("SubTotal") = rowIndex + titleDt.Rows.Count
                    subTotalDt.Rows.Add(roSubTotal)
                End If
                ro = tempDt.NewRow
                If sortFieldAlter = False Then
                    ro(0) = .Item(SourceCol).ToString
                    If Trim(.Item(DestCol).ToString) <> "" Then
                        ro(0) = .Item(DestCol).ToString
                    End If
                    For cnt As Integer = 0 To dt.Columns.Count - 1
                        ro(cnt + 1) = .Item(cnt).ToString
                    Next
                Else
                    ro(0) = .Item(DestCol).ToString
                    For cnt As Integer = 0 To dt.Columns.Count - 1
                        ro(cnt + 1) = .Item(cnt).ToString
                    Next
                End If
                tempDt.Rows.Add(ro)
            End With
        Next
        ds.Tables.Add(tempDt)
        ds.Tables.Add(subTotalDt)
        ds.Tables.Add(titleDt)
        Return ds
    End Function
    Public Sub AmountValidation(ByVal sender As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal amtRnd As Integer = 2)
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", "."
            Case ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space), Chr(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > amtRnd - 1 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub WeightValidation(ByVal sender As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal wtRnd As Integer = 3)
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", "."
            Case ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space), Chr(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > wtRnd - 1 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub
    Public Function GetServerDate(Optional ByVal tran As OleDbTransaction = Nothing) As String
        If GetAdmindbSoftValue("ENTRYDATE", "Y", tran) = "Y" Then
            Dim dd As Date = CType(GetSqlValue(cn, "SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),101))"), Date)
            Return Format(dd, "yyyy-MM-dd")
        Else
            Return Format(Today.Date, "yyyy-MM-dd")
        End If
    End Function

    Public Function GetServerTime(Optional ByVal tran As OleDbTransaction = Nothing) As Date
        If GetAdmindbSoftValue("ENTRYDATE", "Y", tran) = "Y" Then
            Return CType(objGPack.GetSqlValue("SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),108))", , , tran), Date)
        Else
            Return Date.Now.ToLongTimeString
        End If
    End Function



    Public Function GetQryStringForSp(ByVal Source As String, ByVal ChkTable As String, ByVal SelectCol As String, ByVal ChkCol As String, Optional ByVal WithQuotes As Boolean = False, Optional ByVal sep As String = ",") As String
        Dim ret As String = ""
        If Source = "" Or Source.ToUpper = "ALL" Then Return Source
        Dim Qry As String = ""
        Dim sp() As String = Source.ToString.Split(sep)
        For Each s As String In sp
            Qry = " SELECT " & SelectCol & " FROM " & ChkTable & " WHERE " & ChkCol & " = '" & Trim(s) & "'"
            If WithQuotes Then ret += "'"
            ret += objGPack.GetSqlValue(Qry)
            If WithQuotes Then ret += "'"
            ret += ","
        Next
        If ret <> "" Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function
    Public Function GetQryString(ByVal lst As List(Of String), Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To lst.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += lst.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function

    Public Function GetQryString(ByVal Source As String, Optional ByVal sep As String = ",") As String
        Dim ret As String = ""
        Dim sp() As String = Source.ToString.Split(sep)
        For Each s As String In sp
            ret += "'" & Trim(s) & "',"
        Next
        If ret <> "" Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function



#Region "Get Batchno & BillNo & Sno"

    Public Function GetTranDbSoftControlValue(ByVal ctlId As String, Optional ByVal upDate As Boolean = True, Optional ByVal trn As OleDbTransaction = Nothing) As String
        Dim ctlValue As Integer
GETCTLVALUE:
        strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..SOFTCONTROLTRAN WHERE CTLID = '" & ctlId & "'"
        ctlValue = Val(objGPack.GetSqlValue(strSql, , , trn))
        If upDate Then
            strSql = "UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT = " & ctlValue + 1 & " "
            strSql += " WHERE CTLID = '" & ctlId & "' AND CONVERT(INT,CTLTEXT) = " & ctlValue & ""
            cmd = New OleDbCommand(strSql, cn, trn)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GETCTLVALUE
            End If
        End If
        Return (ctlValue + 1).ToString
    End Function


    Public Function GetAdmindbSoftValue(ByVal ctlId As String, Optional ByVal defValue As String = "", Optional ByVal ttran As OleDbTransaction = Nothing, Optional ByVal CaseSensitive As Boolean = False) As String
        Dim strSql As String = Nothing
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & ctlId & "'"
        Dim sqlvalue As String = GetSqlValue(cn, strSql)
        If sqlvalue Is Nothing Then Return defValue Else Return sqlvalue
    End Function

    Public Function GetAdmindbSoftValueAll(Optional ByVal ctrlstring As String = "") As DataTable
        ctrlstring = Replace(ctrlstring, "Where ctlid in (", "")
        ctrlstring = Replace(ctrlstring, ")", "")
        If Not ctrlstring Is Nothing Then ctrlstring = """" & ctrlstring & """" Else ctrlstring = "''"
        Dim strsql As String = " EXEC " & cnAdminDb & "..SP_GETCONTROLS @DBNAME='" & cnAdminDb & "',@IDS=" & ctrlstring
        Dim cmd As OleDbCommand = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtsoft As New DataTable
        dtsoft = dss.Tables(0)

        '        Dim strSql As String = Nothing
        'Dim dtsoft1 As New DataTable
        'strsql = " SELECT CTLID,CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL "
        'If ctrlstring <> "" Then strsql = strsql & ctrlstring
        'da = New OleDbDataAdapter(strsql, cn)

        'da.Fill(dtsoft1)

        Return dtsoft
    End Function

    Public Function GetAdmindbSoftValuefromDt(ByVal dt As DataTable, ByVal softid As String, ByVal softval As String) As String
        If Not dt Is Nothing Then
            Dim mQrystr As String = "CTLID='" & softid & "'"
            Dim drow() As DataRow = Nothing
            drow = dt.Select(mQrystr)
            If drow.Length > 0 And Not (drow Is Nothing) Then Return drow(0).Item("CTLTEXT").ToString Else Return softval
        End If
    End Function


    Public Function GetActualEntryDate() As Date
        strSql = " SELECT MAX(TRANDATE) FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'') = '' AND LEN(TRANTYPE) = 2"
        strSql += vbCrLf + " UNION SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'') = ''  AND LEN(TRANTYPE) = 2"
        strSql += vbCrLf + " UNION SELECT MAX(TRANDATE)TRANDATE FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'') = '' AND FROMFLAG NOT IN ('A','S','')"
        strSql += vbCrLf + " )X"
        Dim EntryDAte As Date = GetEntryDate(GetServerDate)
        Dim Dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Dt)
        If Dt.Rows.Count > 0 Then
            If Dt.Rows(0).Item(0).ToString <> "" Then
                EntryDAte = CType(Dt.Rows(0).Item(0), Date)
            End If
        End If
        Return EntryDAte
    End Function

    Public Function GetEntryDate(ByRef defDate As Date, Optional ByVal ttran As OleDbTransaction = Nothing) As Date
        If GetAdmindbSoftValue("GLOBALDATE", , ttran).ToUpper = "Y" Then
            Try
                defDate = GetAdmindbSoftValue("GLOBALDATEVAL", GetServerDate(ttran), ttran)
            Catch ex As Exception
                defDate = GetServerDate(ttran)
            End Try
        End If
        Return defDate
    End Function

    'Public Sub SetEntryDate(ByRef dtp As DateTimePicker)
    '    If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
    '        dtp.Value = GetAdmindbSoftValue("GLOBALDATEVAL", GetServerDate())
    '    End If
    'End Sub

    Public Function CheckEntryDate(ByVal actualDate As Date) As Boolean
        If CheckTrialDate(GetEntryDate(actualDate)) = False Then
            Return True
        End If
        If actualDate <> GetEntryDate(actualDate) Then
            If MessageBox.Show("Entry date changed" & vbCrLf & "This entry should affect on " & GetEntryDate(actualDate).ToString("dd/MM/yyyy") + vbCrLf + "Do you wish to Continue?", "Date Mismatch", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                Return True
            End If
        End If
    End Function

    Public Function GetForm(ByVal frmName As String, Optional ByVal assemblyType As String = "exe") As Form
        Dim sp() As String = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(",")
        Dim appName As String = sp(0)

        Dim obj_handle As System.Runtime.Remoting.ObjectHandle
        obj_handle = Activator.CreateInstanceFrom(appName & "." & assemblyType, appName & "." & frmName.Replace(".vb", "").Trim)

        Dim frm As New Form
        frm = CType(CType(obj_handle.Unwrap, Object), System.Windows.Forms.Form)
        Return frm
    End Function

    Private Function getTableName(ByVal qry As String) As String
        Dim tableName As String = Nothing
        Dim ch(2) As Char
        ch(0) = " "
        ch(1) = "("
        Dim str() As String = qry.ToUpper.Split(ch)
        For Each st As String In str
            If st.Contains(".DBO.") Then
                tableName = Mid(st, InStr(st, ".DBO.") + 5)
                Exit For
            ElseIf st.Contains("..") Then
                tableName = Mid(st, InStr(st, "..") + 2)
                Exit For
            End If
        Next
        Return tableName
    End Function
    Public Function Exec(ByVal qry As String _
  , ByVal cn As OleDbConnection _
  , ByVal toId As String _
  , ByVal imagePath As String _
  , Optional ByRef tran As OleDbTransaction = Nothing _
  , Optional ByVal imagePathCtrlId As String = Nothing _
  , Optional ByVal _SyncDb As String = Nothing) As Boolean
        Dim syncdb As String = cnAdminDb
        Dim uprefix As String = Left(cnAdminDb, 3)
        If GetAdmindbSoftValueNew("SYNC-SEPDATADB", cn, "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbCheckernew(uprefix + usuffix, cn, tran) <> 0 Then syncdb = uprefix + usuffix
        End If
        If _SyncDb <> Nothing Then syncdb = _SyncDb

        strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ('" & cnCostId & "','" & toId & "','" & qry & "'"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        If imagePath <> Nothing Then
            If IO.File.Exists(imagePath) Then
                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ('" & cnCostId & "','" & toId & "',?,?,'" & imagePathCtrlId & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                Dim fileStr As New IO.FileStream(imagePath, IO.FileMode.Open, IO.FileAccess.Read)
                Dim reader As New IO.BinaryReader(fileStr)
                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                fileStr.Read(result, 0, result.Length)
                fileStr.Close()
                cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                Dim fInfo As New IO.FileInfo(imagePath)
                cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                cmd.ExecuteNonQuery()
            End If
        End If
    End Function
    Public Function GetAdmindbSoftValueNew(ByVal ctlId As String, ByVal CCn As OleDbConnection, Optional ByVal defValue As String = "", Optional ByVal ttran As OleDbTransaction = Nothing) As String
        Dim strSql As String = Nothing
        Dim CtlText As String = ""
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & ctlId & "'"
        If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        CtlText = BrighttechPack.GlobalMethods.GetSqlValue(CCn, strSql, , defValue, ttran)
        If CtlText.ToString.Trim.Length >= 1 Then
            Return CtlText.ToString.Trim.ToUpper
        Else
            Return CtlText.ToString.Trim
        End If
    End Function
    Function DbCheckernew(ByVal dbname As String, ByVal CCn As OleDbConnection, ByVal tran As OleDbTransaction) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select name from master..sysdatabases where name = '" & dbname & "'"
        cmd = New OleDbCommand(strSql, CCn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds, "sysDatabases")
        If ds.Tables("sysDatabases").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Public Function Exec_Safi(ByVal qry As String _
    , ByVal cn As OleDbConnection _
    , ByVal toId As String _
    , ByVal imagePath As String _
    , Optional ByRef tran As OleDbTransaction = Nothing) As Boolean
        strSql = " INSERT INTO " & cnAdminDb & "..SENDSYNC(FROMID,TOID,SQLTEXT"
        If imagePath <> Nothing Then
            If IO.File.Exists(imagePath) Then
                strSql += ",TAGIMAGE,TAGIMAGENAME"
            Else
                imagePath = Nothing
            End If
        End If
        strSql += " )"
        strSql += " VALUES"
        strSql += " (?,?,?"
        If imagePath <> Nothing Then strSql += ",?,?"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.Parameters.AddWithValue("@FROMID", cnCostId)
        cmd.Parameters.AddWithValue("@TOID", toId)
        cmd.Parameters.AddWithValue("@SQLTEXT", qry)
        If imagePath <> Nothing Then
            Dim fileStr As New IO.FileStream(imagePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim reader As New IO.BinaryReader(fileStr)
            Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
            fileStr.Read(result, 0, result.Length)
            fileStr.Close()
            cmd.Parameters.AddWithValue("@TAGIMAGE", result)

            Dim fInfo As New IO.FileInfo(imagePath)
            cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
        End If
        cmd.ExecuteNonQuery()
    End Function

    Public Function SetCenterLocation(ByRef f As Control) As Point
        f.Location = New Point((ScreenWid - f.Width) / 2, ((ScreenHit - 128) - f.Height) / 2)
    End Function
    Public Function ExecQuery(ByVal mode As SyncMode _
           , ByVal qry As String _
           , ByVal cn As OleDbConnection _
           , Optional ByRef tran As OleDbTransaction = Nothing _
           , Optional ByVal toId As String = Nothing _
           , Optional ByVal stateId As String = Nothing _
           , Optional ByVal imagePath As String = Nothing _
           , Optional ByVal replaceTableName As String = Nothing _
           , Optional ByVal LocalExecution As Boolean = True _
           , Optional ByVal Sync As Boolean = True _
           , Optional ByVal ImagePathCtrlId As String = Nothing _
           , Optional ByVal Mastercheck As Boolean = True
               ) As Boolean
        If Not cnCostId.Length > 0 And _HasCostcentre Then
            If tran IsNot Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox("Default CostId doesnt set. Please Set COSTID in Softcontrol and You must Restart your current application", MsgBoxStyle.Information)
            Application.Restart()
        End If
        Dim tableName As String = UCase(getTableName(qry))
        If Mastercheck Then
            If mode = SyncMode.Master And _SyncTo <> "" Then
                If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'Y'", , , tran).Length > 0 Then
                    If Not tran Is Nothing Then tran.Rollback()
                    tran = Nothing
                    MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
                    Return False
                End If
            End If
        End If
        Dim OriginalQry As String = qry
        Dim cmd As OleDbCommand
        If LocalExecution Then
            cmd = New OleDbCommand(qry, cn)
            If Not tran Is Nothing Then cmd.Transaction = tran
            cmd.ExecuteNonQuery()
        End If
        If Sync = False Then Return True
        If Not objGPack.GetSqlValue("SELECT ISNULL(CTLTEXT,'') FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & strCompanyId & "_SYNC' AND CTLTEXT = 'Y'", , , tran).Length > 0 Then Return True
        Dim strSql As String = Nothing
        If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'N'", , , tran).Length > 0 Then
            Return True
        End If
        If replaceTableName <> Nothing Then qry = qry.Replace(tableName, replaceTableName)
        If mode = SyncMode.Master Then
            Dim dtCostId As New DataTable
            strSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE where COSTID <> '" & cnCostId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            If Not tran Is Nothing Then cmd.Transaction = tran
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCostId)
            For Each ro As DataRow In dtCostId.Rows
                If stateId <> Nothing And ro!STATEID.ToString <> stateId Then Continue For
                Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
            Next
        ElseIf mode = SyncMode.Stock Then
            If GetAdmindbSoftValue("SYNC-STOCK", "N", tran) = "Y" Then
                Dim dtCostId As New DataTable
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
                strSql += " WHERE COSTID <> '" & cnCostId & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                If Not tran Is Nothing Then cmd.Transaction = tran
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtCostId)
                For Each ro As DataRow In dtCostId.Rows
                    If ro!COSTID.ToString = cnCostId Then Continue For
                    If ro!COSTID.ToString = toId Then
                        Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    Else
                        Exec(OriginalQry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    End If
                Next
            Else
                If toId <> Nothing Then
                    If toId = cnCostId Then Return True
                    Exec(qry.Replace("'", "''"), cn, toId, imagePath, tran, ImagePathCtrlId)
                End If
            End If
        ElseIf mode = SyncMode.Transaction Then
            If toId <> Nothing Then
                Dim syncTo As String = _SyncTo
                If syncTo = "" Then syncTo = toId
                If syncTo = cnCostId Then Return True
                Exec(qry.Replace("'", "''"), cn, syncTo, imagePath, tran, ImagePathCtrlId)
            End If
        End If
        Return True
    End Function
    Public Function ExecQuery_Safi(ByVal mode As SyncMode _
       , ByVal qry As String _
       , ByVal cn As OleDbConnection _
       , Optional ByRef tran As OleDbTransaction = Nothing _
       , Optional ByVal toId As String = Nothing _
       , Optional ByVal stateId As String = Nothing _
       , Optional ByVal imagePath As String = Nothing _
       , Optional ByVal replaceTableName As String = Nothing _
       , Optional ByVal LocalExecution As Boolean = True _
       , Optional ByVal ImagePathCtrlId As String = Nothing
       ) As Boolean
        If Not cnCostId.Length > 0 And objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'", , "N", tran).ToUpper = "Y" Then
            If tran IsNot Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox("Default CostId doesnt set. Please Set COSTID in Softcontrol and You must Restart your current application", MsgBoxStyle.Information)
            Application.Restart()
        End If
        Dim tableName As String = UCase(getTableName(qry))
        If mode = SyncMode.Master And GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'Y'", , , tran).Length > 0 Then
                If Not tran Is Nothing Then tran.Rollback()
                tran = Nothing
                MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Dim OriginalQry As String = qry
        Dim cmd As OleDbCommand
        If LocalExecution Then
            cmd = New OleDbCommand(qry, cn)
            If Not tran Is Nothing Then cmd.Transaction = tran
            cmd.ExecuteNonQuery()
        End If
        Dim strSql As String = Nothing
        If objGPack.GetSqlValue("SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE TABLENAME = '" & tableName & "' AND SYNC = 'N'", , , tran).Length > 0 Then
            Return True
        End If
        If replaceTableName <> Nothing Then qry = qry.Replace(tableName, replaceTableName)

        If mode = SyncMode.Master Then
            Dim dtCostId As New DataTable
            strSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE where COSTID <> '" & cnCostId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            If Not tran Is Nothing Then cmd.Transaction = tran
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCostId)
            For Each ro As DataRow In dtCostId.Rows
                If stateId <> Nothing And ro!STATEID.ToString <> stateId Then Continue For
                Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)

            Next
        ElseIf mode = SyncMode.Stock Then
            If GetAdmindbSoftValue("SYNC-STOCK", "N", tran) = "Y" Then
                Dim dtCostId As New DataTable
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
                strSql += " WHERE COSTID <> '" & cnCostId & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                If Not tran Is Nothing Then cmd.Transaction = tran
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtCostId)
                For Each ro As DataRow In dtCostId.Rows
                    If ro!COSTID.ToString = cnCostId Then Continue For
                    If ro!COSTID.ToString = toId Then
                        Exec(qry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    Else
                        Exec(OriginalQry.Replace("'", "''"), cn, ro!COSTID.ToString, imagePath, tran, ImagePathCtrlId)
                    End If
                Next
            Else
                If toId <> Nothing Then
                    If toId = cnCostId Then Return True
                    Exec(qry.Replace("'", "''"), cn, toId, imagePath, tran, ImagePathCtrlId)
                End If
            End If
        ElseIf mode = SyncMode.Transaction Then
            If toId <> Nothing Then
                Dim syncTo As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO' AND CTLTEXT <> ''", , toId, tran)
                If syncTo = cnCostId Then Return True
                Exec(qry.Replace("'", "''"), cn, syncTo, imagePath, tran, ImagePathCtrlId)
            End If
        End If
        Return True
    End Function

    Public Sub FillCheckedListBox(ByVal qry As String, ByVal chkLst As CheckedListBox, Optional ByVal clear As Boolean = True, Optional ByVal Check As Boolean = False)
        chkLst.Items.Clear()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLst.Items.Add(ro(0).ToString)
            chkLst.SetItemChecked(chkLst.Items.Count - 1, Check)
        Next
    End Sub


    Public Function GetChecked_CheckedList(ByVal chkLst As CheckedListBox, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += chkLst.CheckedItems.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function

    Public Function GetChecked_CheckedListid(ByVal chkLst As CheckedListBox, ByVal Retid As String, ByVal chkcol As String, ByVal chktable As String, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            strSql = "SELECT " & Retid & " FROM " & chktable & " WHERE " & chkcol & "  ='" & chkLst.CheckedItems.Item(cnt).ToString & "'"
            retStr += GetSqlValue(cn, strSql).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function


    Public Function GetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += chkLst.CheckedItems.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function

    Public Function GetChecked_CheckedListid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal Retid As String, ByVal chkcol As String, ByVal chktable As String, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            If withSingleQt Then retStr += "'"
            strSql = "SELECT " & Retid & " FROM " & chktable & " WHERE " & chkcol & "  ='" & chkLst.CheckedItems.Item(cnt).ToString & "'"
            retStr += GetSqlValue(cn, strSql)
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function


    Public Sub FillGridGroupStyle(ByVal gridview As DataGridView, Optional ByVal ParticularColName As String = "")
        Dim ind As Integer = 0
        If ParticularColName <> "" Then
            If gridview.Columns.Contains(ParticularColName) Then
                ind = gridview.Columns(ParticularColName).Index
            End If
        End If
        For Each dgvRow As DataGridViewRow In gridview.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T1" Then
                dgvRow.Cells(ind).Style.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T2" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T3" Then
                dgvRow.Cells(ind).Style.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S1" Then
                dgvRow.DefaultCellStyle.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S2" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "G" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
    End Sub


    Public Sub FillGridGroupStyle_KeyNoWise(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        If gridView.Columns.Contains("KEYNO") = False Then Exit Sub
        If gridView.Columns.Contains("COLHEAD") = False Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        Dim rowTitle() As DataRow = Nothing
        rowTitle = dt.Select("COLHEAD = 'T'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle1
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'N'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.ForeColor = Color.OrangeRed
        Next
        rowTitle = dt.Select("COLHEAD = 'S'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        Next
        rowTitle = dt.Select("COLHEAD = 'S1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle1
        Next
        rowTitle = dt.Select("COLHEAD = 'S2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle2
        Next
        rowTitle = dt.Select("COLHEAD = 'G'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
        Next
        rowTitle = dt.Select("COLHEAD = 'H'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportHigLightStyle
        Next
        rowTitle = dt.Select("COLHEAD = 'R'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportGSTStyle
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'R1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportGSTStyle
        Next
        rowTitle = dt.Select("COLHEAD = 'R2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportGSTHeadStyle
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub
    Public Function CalcRoundoffAmt(ByVal amt As Double, ByVal type As String) As Double
        Select Case type
            Case "L"
                Return Math.Floor(amt)
            Case "F"
                If Math.Abs(amt - Math.Floor(amt)) >= 0.5 Then
                    Return Math.Ceiling(amt)
                Else
                    Return Math.Floor(amt)
                End If
            Case "H"
                Return Math.Ceiling(amt)
            Case Else
                Return amt
        End Select
        Return amt
    End Function
    Public Sub CheckAndInsertsoftcontrol(ByVal ctlid As String, ByVal ctlname As String, ByVal ctltype As String, ByVal ctltext As String, ByVal ctlmodule As String)
        strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='" & ctlid & "'"
        Dim cnt As Integer = GetSqlValue(cn, strSql)
        If cnt <= 0 Then
            strSql = " INSERT INTO " & cnAdminDb & "..SOFTCONTROL(CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODULE,COSTID)VALUES('" & ctlid & "','" & ctlname & "','" & ctltype & "','" & ctltext & "','" & ctlmodule & "','" & cnCostId & "')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
    End Sub
    Public Function InsertData(
    ByVal Mode As SyncMode _
    , ByVal Dt As DataTable _
    , ByVal Con As OleDbConnection _
    , Optional ByVal Tran As OleDbTransaction = Nothing _
    , Optional ByVal toId As String = Nothing _
    , Optional ByVal stateId As String = Nothing _
    , Optional ByVal imagePath As String = Nothing _
    , Optional ByVal replaceTableName As String = Nothing _
    , Optional ByVal LocalExecution As Boolean = True _
    , Optional ByVal Sync As Boolean = True
    ) As Boolean
        Try
            Dim Qry As String = ""
            Dim _Column As String = ""
            Dim _Values As String = ""
            For Each Row As DataRow In Dt.Rows
                Qry = "INSERT INTO " & Row.Table.TableName & " ( "
                _Column = ""
                _Values = ""
                For Each dCol As DataColumn In Row.Table.Columns
                    _Column = _Column & "," + dCol.ColumnName
                    If dCol.DataType.Name = "String" Then
                        _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
                    ElseIf dCol.DataType.Name = "DateTime" Then
                        If IsDBNull(Row.Item(dCol.ColumnName)) Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "01/01/0001 12:00:00 AM" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "1/1/0001 12:00:00 AM" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "01/01/0001" Then
                            _Values = _Values & "," + "NULL" + ""
                        ElseIf Row.Item(dCol.ColumnName).ToString = "1/1/0001" Then
                            _Values = _Values & "," + "NULL" + ""

                        Else
                            _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                        End If
                    ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name _
                    Or dCol.DataType.Name = GetType(Int16).Name Or dCol.DataType.Name = GetType(Int32).Name Or dCol.DataType.Name = GetType(Int64).Name Or dCol.DataType.Name = GetType(Integer).Name Then
                        _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
                    ElseIf dCol.DataType.Name = "Boolean" Then
                        _Values = _Values & "," + "NULL" + ""
                    Else
                        _Values = _Values & "," & Row.Item(dCol.ColumnName)
                    End If
                Next
                _Column = _Column & ")"
                _Column = _Column.Substring(1)
                _Values = _Values.Substring(1)
                Qry = (Qry + _Column & " VALUES (") + _Values & ")"
                ExecQuery(Mode, Qry, Con, Tran, toId, stateId, imagePath, replaceTableName, LocalExecution, Sync)
            Next
            Return True
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
    Public Function InsertQry(ByVal Row As DataRow, Optional ByVal DB As String = Nothing) As String
        Dim qry As String = ""
        qry = "INSERT INTO " & IIf(Not DB Is Nothing, DB & "..", "") & Row.Table.TableName & " ( "
        Dim _Column As String = ""
        Dim _Values As String = ""
        For Each dCol As DataColumn In Row.Table.Columns
            _Column = _Column & "," + dCol.ColumnName
            If dCol.DataType.Name = "String" Then
                _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
            ElseIf dCol.DataType.Name = "DateTime" Then
                If IsDBNull(Row.Item(dCol.ColumnName)) Then
                    _Values = _Values & "," + "NULL" + ""
                Else
                    _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                End If
            ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name _
            Or dCol.DataType.Name = GetType(Int16).Name Or dCol.DataType.Name = GetType(Int32).Name Or dCol.DataType.Name = GetType(Int64).Name Or dCol.DataType.Name = GetType(Integer).Name Then
                _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
            Else
                _Values = _Values & "," & Row.Item(dCol.ColumnName)
            End If
        Next
        _Column = _Column & ")"
        _Column = _Column.Substring(1)
        _Values = _Values.Substring(1)
        qry = (qry + _Column & " VALUES (") + _Values & ")"
        Return qry
    End Function
    Public Function GetStockCompId() As String
        If cnCentStock Then
            Return _MainCompId
        Else
            Return strCompanyId
        End If
    End Function
    Public Function CheckTrialDate(ByVal dat As Date) As Boolean
        If _Demo Then
            If dat > _DemoExpiredDate Then
                MsgBox("Your 30 days trail period expired. Please try evaluate version", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function

    Public Function CheckBckDays(ByVal Userid As Integer, ByVal Menuname As String, ByVal dtdate As Date) As Boolean
        Dim mserverdate As Date = GetServerDate()
        If dtdate < mserverdate Then
            If Userid = 999 Then Return True
            Dim mdifdays As Integer = Math.Abs(DateDiff(DateInterval.Day, mserverdate, dtdate))
            If Userid = 0 Or Menuname = "" Then Return True
            Dim ro() As DataRow = _DtUserRights.Select("ACCESSID LIKE '" & Menuname & "%'")
            If ro.Length = 0 Then Return True
            Dim menuid As String = ro(0).Item(0).ToString
            strSql = "select isnull(RT.VIEWDAYS,0) from " & cnAdminDb & "..ROLETRAN RT INNER JOIN " & cnAdminDb & "..USERROLE UR ON RT.ROLEID =UR.ROLEID WHERE UR.USERID = " & Userid & " AND MENUID ='" & menuid & "'"
            Dim mdays As Decimal = Val(objGPack.GetSqlValue(strSql))
            If mdays > 0 Then
                If mdifdays >= mdays Then MsgBox("(Days Exceed)Please check given date") : Return False Else Return True
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Public Function CheckDate(ByVal dat As Date) As Boolean
        If Not (dat >= cnTranFromDate And dat <= cnTranToDate) Then
            MsgBox("Invalid Entry Date", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function
    Public Function GetColumnNames(ByVal dbName As String, ByVal tblName As String, Optional ByVal ttran As OleDbTransaction = Nothing) As String
        Dim retStr As String = Nothing
        strSql = " SELECT NAME FROM " & dbName & "..SYSCOLUMNS WHERE ID = "
        strSql += " (SELECT ID FROM " & dbName & "..SYSOBJECTS WHERE NAME = '" & tblName & "')"
        Dim dtTEmp As New DataTable
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If Not ttran Is Nothing Then cmd.Transaction = ttran
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTEmp)
        For cnt As Integer = 0 To dtTEmp.Rows.Count - 1
            retStr += dtTEmp.Rows(cnt).Item("NAME").ToString
            If cnt <> dtTEmp.Rows.Count - 1 Then
                retStr += ","
            End If
        Next
        Return retStr
    End Function


    Public Sub LoadCompany(ByRef chkLstBox As CheckedListBox)
        chkLstBox.Items.Clear()
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,COMPANYNAME"
        Dim dtCompany As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        For Each ro As DataRow In dtCompany.Rows
            chkLstBox.Items.Add(ro("COMPANYNAME").ToString)
            If ro("COMPANYNAME").ToString = strCompanyName Then chkLstBox.SetItemChecked(chkLstBox.Items.Count - 1, True)
        Next
    End Sub

    Public Sub CheckedListCompany_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim chklst As CheckedListBox = CType(sender, CheckedListBox)
        If chklst.Items.Count > 0 Then
            If Not chklst.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chklst.Items.Count - 1
                    If chklst.Items.Item(cnt).ToString = strCompanyName Then
                        chklst.SetItemChecked(cnt, True)
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
    Public Function GetSelectedCompanyId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += GetSqlValue(cn, "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            If WithQuotes Then retStr += "'"
            retStr += "" & strCompanyId & ""
            If WithQuotes Then retStr += "'"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCompanyId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Public Function GetSelectedCatcode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Public Function GetSelectedAccountId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function

    Public Function GetSelectedMetalid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += GetSqlValue(cn, "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function
    Public Function GetSelectedCostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Function GetSelectedCostIddouble(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "''"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "''"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCostId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelecteditemid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Function GetSelectedSubitemid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Function GetSelectedDesignerid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Sub LoadCostName(ByVal chkLstCostCentre As BrighttechPack.CheckedComboBox, Optional ByVal withAll As Boolean = True)
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                If withAll Then
                    chkLstCostCentre.Items.Add("ALL", True)
                End If
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                Next
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub


    Public Sub LoadCostName(ByVal chkLstCostCentre As CheckedListBox, Optional ByVal withAll As Boolean = True)
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                If withAll Then
                    chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                End If
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    If cnCostName = dt.Rows(cnt).Item(0).ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub
    Public Function FormatNumberStyle(ByVal noOfDecimal As Integer) As String
        Dim retStr As String = "0"
        If noOfDecimal > 0 Then retStr += "."
        For cnt As Integer = 1 To noOfDecimal
            retStr += "0"
        Next
        Return retStr
    End Function
    Public Function FormatStringCustom(ByVal orgStr As String, ByVal spaceChar As Char, ByVal leng As Integer) As String
        Dim retStr As String = ""
        For cnt As Integer = 1 To leng - orgStr.Length
            retStr += spaceChar
        Next
        Return retStr & orgStr
    End Function

    Public Sub AutoImageSizer(ByVal bmp As Bitmap, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.Image = bmp.Clone
            AutoImageSizer(picBox, pSizeMode)
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
        End Try
    End Sub

    Public Sub AutoImageSizer(ByVal ImagePath As String, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.Image = Nothing
            picBox.SizeMode = pSizeMode
            If System.IO.File.Exists(ImagePath) Then
                Dim fStream As IO.FileStream
                fStream = New IO.FileStream(ImagePath, IO.FileMode.Open, IO.FileAccess.Read)
                picBox.Image = Image.FromStream(fStream)
                fStream.Close()
            Else
                picBox.Image = My.Resources.no_photo
            End If
            AutoImageSizer(picBox, pSizeMode)
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
        End Try
    End Sub

    Public Sub AutoImageSizer(ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
        Try
            picBox.SizeMode = pSizeMode
            Dim imgOrg As Bitmap
            Dim imgShow As Bitmap
            Dim g As Graphics
            Dim divideBy, divideByH, divideByW As Double
            imgOrg = DirectCast(picBox.Image, Bitmap)

            divideByW = imgOrg.Width / picBox.Width
            divideByH = imgOrg.Height / picBox.Height
            If divideByW > 1 Or divideByH > 1 Then
                If divideByW > divideByH Then
                    divideBy = divideByW
                Else
                    divideBy = divideByH
                End If

                imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            Else
                imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
                g = Graphics.FromImage(imgShow)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
                g.Dispose()
            End If
            imgOrg.Dispose()
            picBox.Image = imgShow
        Catch ex As Exception
            picBox.Image = My.Resources.no_photo
            'MsgBox(ex.ToString)
        End Try
    End Sub

    'Public Sub AutosizeImage(ByVal ImagePath As String, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
    '    Try
    '        picBox.Image = Nothing
    '        picBox.SizeMode = pSizeMode
    '        If System.IO.File.Exists(ImagePath) Then
    '            Dim imgOrg As Bitmap
    '            Dim imgShow As Bitmap
    '            Dim g As Graphics
    '            Dim divideBy, divideByH, divideByW As Double
    '            imgOrg = DirectCast(Bitmap.FromFile(ImagePath), Bitmap)

    '            divideByW = imgOrg.Width / picBox.Width
    '            divideByH = imgOrg.Height / picBox.Height
    '            If divideByW > 1 Or divideByH > 1 Then
    '                If divideByW > divideByH Then
    '                    divideBy = divideByW
    '                Else
    '                    divideBy = divideByH
    '                End If

    '                imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
    '                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '                g = Graphics.FromImage(imgShow)
    '                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '                g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '                g.Dispose()
    '            Else
    '                imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
    '                imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '                g = Graphics.FromImage(imgShow)
    '                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '                g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '                g.Dispose()
    '            End If
    '            imgOrg.Dispose()

    '            picBox.Image = imgShow
    '        Else
    '            picBox.Image = My.Resources.no_photo
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub
    Public Function GetNewBatchno(ByVal CostId As String, ByVal BillDate As Date, ByVal Transact As OleDbTransaction) As String
        Dim StrSql As String = Nothing
        StrSql = " DECLARE @RETBATCHVALUE VARCHAR(15)"
        StrSql += " EXEC " & cnStockDb & "..GET_BATCHNO"
        StrSql += " @COSTID = '" & CostId & "'"
        StrSql += " ,@BILLDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@COMPANYID = '" & strCompanyId & "'"
        StrSql += " ,@RETVALUE = @RETBATCHVALUE OUTPUT"
        StrSql += " SELECT @RETBATCHVALUE"
        cmd = New OleDbCommand(StrSql, cn, Transact)
        Dim reader As OleDbDataReader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(reader)
        Return dt.Rows(0).Item(0).ToString
        ''Return objGPack.GetSqlValue(StrSql, , , Transact) ''commented on 27-JULY-2021
    End Function
    Public Function GetNewSno(ByVal CtlId As TranSnoType, ByVal Transact As OleDbTransaction, Optional ByVal ProcName As String = "GET_TRANSNO_TRAN", Optional ByVal CompId As String = Nothing, Optional ByVal ProcDbName As String = Nothing) As String
        If ProcDbName = Nothing Then ProcDbName = cnStockDb
        Dim StrSql As String = Nothing
        StrSql = " DECLARE @RETSNOVALUE VARCHAR(15)"
        StrSql += " EXEC " & ProcDbName & ".." & ProcName
        StrSql += " @COSTID = '" & cnCostId & "'"
        StrSql += " ,@CTLID = '" & CtlId.ToString & "'"
        StrSql += " ,@CHECKTABLENAME = '" & CtlId.ToString.ToUpper.Replace("CODE", "") & "'"
        StrSql += " ,@COMPANYID = '" & IIf(CompId = Nothing, strCompanyId, CompId) & "'"
        StrSql += " ,@RETVALUE = @RETSNOVALUE OUTPUT"
        StrSql += " SELECT @RETSNOVALUE"
        Return objGPack.GetSqlValue(StrSql, , , Transact)
    End Function
    Public Function GetMaxTranNo(ByVal BillDate As Date, ByVal Transact As OleDbTransaction) As Integer
        Dim Strsql As String
        Strsql = " DECLARE @RE VARCHAR(50)"
        Strsql += " EXEC " & cnStockDb & "..GET_TRANNO_MAX"
        Strsql += " @BILLDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
        Strsql += " ,@RETVALUE = @RE OUTPUT"
        Strsql += " SELECT @RE AS TRANNO"
        Return Val(objGPack.GetSqlValue(Strsql, , , Transact))
    End Function
#End Region

#Region "GridDispDiaMethods"
    Public Function SetGroupHeadColWid(ByVal HeadCol As DataGridViewColumn, ByVal Grid As DataGridView) As Integer
        Dim wid As Integer = 0
        Dim vis As Boolean = False
        Dim cNames() As String = HeadCol.Name.Split("~")
        For Each s As String In cNames
            If Grid.Columns.Contains(s) Then
                wid += IIf(Grid.Columns(s).Visible, Grid.Columns(s).Width, 0)
            Else
                wid = HeadCol.Width
            End If
        Next
        If Not HeadCol.Name.Contains("~") Then
            HeadCol.HeaderText = ""
        End If
        Return wid
    End Function

    Public Function GetTagSaleValue(ByVal ItemId As Integer, ByVal TagNo As String, ByVal ItemRate As Decimal) As Decimal
        Dim dtTagInfo As New DataTable
        Dim TagSalValue As Decimal = Nothing
        strSql = vbCrLf + " SELECT T.GRSWT,P.PURGRSNET,P.PURNETWT,P.PURWASTAGE,P.PURTOUCH,P.PURMC"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(PURAMT) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = P.TAGSNO),0)PURSTNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(PURAMOUNT) FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = P.TAGSNO),0)PURMISCAMT"
        strSql += vbCrLf + " ,T.SALVALUE,P.PURVALUE,T.ADD_VA_PER"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + " WHERE T.ITEMID =  " & ItemId & "  AND T.TAGNO = '" & TagNo & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagInfo)
        If Not dtTagInfo.Rows.Count > 0 Then Return 0
        Dim RoPur As DataRow = dtTagInfo.Rows(0)
        TagSalValue = Val(RoPur!SALVALUE.ToString)

        If Val(RoPur!PURVALUE.ToString) <> 0 And Val(RoPur!ADD_VA_PER.ToString) <> 0 Then
            Dim purValue As Double = Nothing
            purValue = (IIf(RoPur!PURGRSNET.ToString = "G", Val(RoPur!GRSWT.ToString), Val(RoPur!PURNETWT.ToString)) + Val(RoPur!PURWASTAGE.ToString)) _
            * IIf(Val(RoPur!PURTOUCH.ToString) > 0, (Val(RoPur!PURTOUCH.ToString) / 100), 1)
            purValue = purValue * ItemRate
            purValue += Val(RoPur!PURSTNAMT.ToString) + Val(RoPur!PURMISCAMT.ToString) + Val(RoPur!PURMC.ToString)

            Dim AddVaAmt = purValue * (Val(RoPur!ADD_VA_PER.ToString) / 100)
            purValue = purValue + AddVaAmt
            If TagSalValue < purValue Then
                TagSalValue = purValue
            End If
        End If
        Return TagSalValue
    End Function

    Public Sub SetGridHeadColWidth(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Width = SetGroupHeadColWid(.Columns(cnt), f.gridView)
            Next
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
#End Region

    Public Function GetSubItemQry(ByVal SelectColumns() As String, Optional ByVal ItemId As Integer = 0) As String
        Dim SelCol As String = Nothing
        For Each s As String In SelectColumns
            SelCol += s & ","
        Next
        SelCol = Mid(SelCol, 1, SelCol.Length - 1)
        Dim Str As String
        Str = "SELECT " & SelCol & " FROM " & cnAdminDb & "..SUBITEMMAST"
        Str += " WHERE ITEMID = " & ItemId
        Str += " AND ACTIVE = 'Y'"
        Str += " ORDER BY "
        If _SubItemOrderByName Then
            Str += " SUBITEMNAME"
        Else
            Str += " DISPLAYORDER,SUBITEMNAME"
        End If
        Return Str
    End Function

    Public Function GetAcNameQryFilteration(Optional ByVal AliasName As String = Nothing) As String
        Dim Str As String = Nothing
        Str += " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') <> 'N'"
        Str += " AND (ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') = ''"
        Str += " OR ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') LIKE '%" & strCompanyId & "%')"
        Return Str
    End Function


    Public Function GetItemQryFilteration(Optional ByVal ModuleId As String = "A", Optional ByVal AliasName As String = Nothing) As String
        ModuleId = ModuleId.ToUpper
        Dim Str As String = Nothing
        Str = " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') = 'Y'"
        Str += " AND (ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') = ''"
        Str += " OR ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "COMPANYID,'') LIKE '%" & strCompanyId & "%')"
        If ModuleId = "S" And cnCentStock Then
            Str = " AND ISNULL(" & IIf(AliasName <> "", AliasName & ".", "") & "ACTIVE,'Y') = 'Y'"
        End If
        Return Str
    End Function

    Public Sub BillNoRegenerator()
        If objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y'") = "" Then Exit Sub
        If MessageBox.Show("Sure, You want to reset billno?", "BillNo Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> DialogResult.Yes Then
            Exit Sub
        End If
        'Dim objSecret As New frmAdminPassword()
        'If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
        '    Exit Sub
        'End If
        strSql = vbCrLf + " UPDATE " & cnStockDb & "..BILLCONTROL "
        strSql += vbCrLf + " SET CTLTEXT = ''"
        strSql += vbCrLf + " WHERE CTLTYPE = 'N' "
        strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y')"
        strSql += vbCrLf + " AND CTLID NOT IN"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " 'GEN-CRSALESBILLNO'"
        strSql += vbCrLf + " ,'GEN-APPISSBILLNO'"
        strSql += vbCrLf + " ,'GEN-APPRECBILLNO'"
        strSql += vbCrLf + " ,'GEN-ADVANCEBILLNO'"
        strSql += vbCrLf + " ,'ORDERNO','REPAIRNO'"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " UPDATE " & cnStockDb & "..BILLCONTROL "
        strSql += vbCrLf + " SET CTLTEXT = '" + Mid(Format(GetServerDate()), 6, 2) + Mid(Format(GetServerDate()), 9, 2) + "0'"
        strSql += vbCrLf + " WHERE CTLTYPE = 'N' "
        strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REGENBILLNO' AND CTLTEXT = 'Y')"
        strSql += vbCrLf + " AND CTLID IN"
        strSql += vbCrLf + " ('ORDERNO','REPAIRNO')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        MsgBox("Bill information reset successfully", MsgBoxStyle.Information)
    End Sub


    Public Sub SetGlobalVariables()
        strSql = " EXEC " & cnAdminDb & "..SP_GETCONTROLS @DBNAME='" & cnAdminDb & "',@IDS=''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtSoftKeys As DataTable
        dtSoftKeys = dss.Tables(0)

        'Dim Is_Productdisc As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "POS_DISC_OPT", "N")
        _IsWholeSaleType = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ISWHOLESALE", "N") = "Y", True, False)
        _HideBackOffice = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "HIDEBACKOFFICE", "N") = "Y", True, False)
        _CounterWiseCashMaintanance = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COUNT-CASH", "N") = "Y", True, False)
        _SubItemOrderByName = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDER_SUBITEM", "Y") = "Y", True, False)
        cnCentStock = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "CENT-STOCK", "Y") = "Y", True, False)
        cnChitTrandb = GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDBPREFIX", "") & "SH0708"
        cnChitCompanyid = GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDBPREFIX", "")
        _MainCompId = GetAdmindbSoftValuefromDt(dtSoftKeys, "COMPANYID", "")
        cnCostId = GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTID", "")
        cnHOCostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'")
        _CashOpening = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "RPTWITH_CASH", "N") = "Y", True, False)
        DiaRnd = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-DIA", 4))
        EXE_WITH_PARAM = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "EXE_WITH_PARAM", "N") = "Y", True, False)
        _SyncTo = GetAdmindbSoftValuefromDt(dtSoftKeys, "SYNC-TO", "")
        _HasCostcentre = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTCENTRE", "N") = "Y", True, False)
        COSTCENTRE_SINGLE = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "COSTCENTRE_SINGLE", "N") = "Y", True, False)
        PIC_ITEMWISE = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "PIC_ITEMWISE", "N") = "Y", True, False)
        PICPATH = GetAdmindbSoftValuefromDt(dtSoftKeys, "PICPATH", "") : PICPATH = PICPATH & IIf(PICPATH.EndsWith("\") = False, "\", "")
        WTAMTOPT = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "WTAMTOPT", "N") = "Y", True, False)
    End Sub
    Public Function GetSqlValue(ByVal Cn As OleDb.OleDbConnection, ByVal Qry As String) As Object
        Dim Obj As Object = Nothing
        Dim Da As OleDb.OleDbDataAdapter
        Dim DtTemp As New DataTable
        Da = New OleDb.OleDbDataAdapter(Qry, Cn)
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Obj = DtTemp.Rows(0).Item(0)
        End If
        Return Obj
    End Function

    Public Sub SetSettingsObj(ByVal Obj As Object, ByVal frmName As String, ByVal ObjType As Type, Optional ByVal Authorize As Boolean = False)
        Dim SettingsFileName As String = IO.Path.GetTempPath & "\RPT_" & frmName & ".xml"
        Dim objStreamWriter As New IO.StreamWriter(SettingsFileName)
        Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
        x.Serialize(objStreamWriter, Obj)
        objStreamWriter.Close()

        Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Open, IO.FileAccess.Read)
        Dim reader As New IO.BinaryReader(fileStr)
        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
        fileStr.Read(result, 0, result.Length)
        fileStr.Close()

        strSql = " DELETE FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        If Authorize Then
            strSql += " AND USERID = 99999"
        Else
            strSql += " AND USERID = " & userId & ""
        End If
        cmd = New OleDb.OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim fiINfo As New IO.FileInfo(SettingsFileName)
        strSql = " INSERT INTO " & cnAdminDb & "..APPDATA"
        strSql += vbCrLf + " (FILENAMES"
        strSql += vbCrLf + " ,CONTENT,USERID"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  VALUES"
        strSql += vbCrLf + "  (?,?,?)"
        cmd = New OleDb.OleDbCommand(strSql, cn)
        cmd.Parameters.AddWithValue("@FILENAMES", fiINfo.Name)
        cmd.Parameters.AddWithValue("@CONTENT", result)
        If Authorize Then
            cmd.Parameters.AddWithValue("@USERID", 99999)
        Else
            cmd.Parameters.AddWithValue("@USERID", userId)
        End If
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub GetSettingsObj(ByRef Obj As Object, ByVal frmName As String, ByVal ObjType As Type, Optional ByVal Authorize As Boolean = False)
        Dim SettingsFileName As String = IO.Path.GetTempPath & "\RPT_" & frmName & ".xml"
        strSql = " SELECT CONTENT FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        If Authorize Then
            strSql += " AND USERID = 99999"
        Else
            strSql += " AND USERID = " & userId & ""
        End If
        Dim filContent As Object = GetSqlValue(cn, strSql)
        If filContent IsNot Nothing Then
            Dim myByte() As Byte = filContent
            Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            fileStr.Write(myByte, 0, myByte.Length)
            fileStr.Close()

            Dim fInfo As New IO.FileInfo(SettingsFileName)
            Dim objStreamReader As New IO.StreamReader(SettingsFileName)
            Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
            Obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
            Exit Sub
        End If

        strSql = " SELECT CONTENT FROM " & cnAdminDb & "..APPDATA WHERE FILENAMES = 'RPT_" & frmName & ".xml'"
        strSql += " AND ISNULL(USERID,0) = 0"
        filContent = GetSqlValue(cn, strSql)
        If filContent IsNot Nothing Then
            Dim myByte() As Byte = filContent
            Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Create, IO.FileAccess.ReadWrite)
            fileStr.Write(myByte, 0, myByte.Length)
            fileStr.Close()

            Dim fInfo As New IO.FileInfo(SettingsFileName)
            Dim objStreamReader As New IO.StreamReader(SettingsFileName)
            Dim x As New System.Xml.Serialization.XmlSerializer(ObjType)
            Obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
        End If
    End Sub
    Public Sub GetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal CheckedItems As List(Of String))
        CheckedItems.Clear()
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            CheckedItems.Add(chkLst.CheckedItems.Item(cnt).ToString)
        Next
    End Sub
    Public Sub GetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal CheckedItems As List(Of String))
        CheckedItems.Clear()
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            CheckedItems.Add(chkLst.CheckedItems.Item(cnt).ToString)
        Next
    End Sub
    Public Sub SetChecked_CheckedList(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal CheckedItems As List(Of String), ByVal DefCheckVal As String)
        If CheckedItems.Count > 0 Then DefCheckVal = Nothing
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            If CheckedItems.Contains(chkLst.Items.Item(cnt).ToString) Or (chkLst.Items.Item(cnt).ToString = DefCheckVal And DefCheckVal <> Nothing) Then
                chkLst.SetItemChecked(cnt, True)
            Else
                chkLst.SetItemChecked(cnt, False)
            End If
        Next
    End Sub
    Public Sub SetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal CheckedItems As List(Of String), ByVal DefCheckVal As String)
        If CheckedItems.Count > 0 Then DefCheckVal = Nothing
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            If CheckedItems.Contains(chkLst.Items.Item(cnt).ToString) Or (chkLst.Items.Item(cnt).ToString = DefCheckVal And DefCheckVal <> Nothing) Then
                chkLst.SetItemChecked(cnt, True)
            Else
                chkLst.SetItemChecked(cnt, False)
            End If
        Next
    End Sub

    Public Sub SetChecked_CheckedList(ByVal chkLst As CheckedListBox, ByVal status As Boolean)
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            chkLst.SetItemChecked(cnt, status)
        Next
    End Sub

    Public Function GetCostId(ByVal CCostId As String) As String
        If CCostId = Nothing Then CCostId = ""
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 2 - CCostId.ToString.Length
            RetStr += "0"
        Next
        RetStr += CCostId
        Return RetStr
    End Function
    Public Function GetCompanyId(ByVal CompanyId As String) As String
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 3 - CompanyId.Length
            RetStr += "0"
        Next
        RetStr += CompanyId
        Return RetStr
    End Function
    Public Function GetBillDate(ByRef BillDate As Date) As Boolean
        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then
            BillDate = GetAdmindbSoftValue("GLOBALDATEVAL", BillDate)
            Return True
        Else
            Dim RetBilldate As Date
            RetBilldate = GetServerDate()
            If BillDate <> RetBilldate Then
                Select Case MessageBox.Show("Billdate and Entrydate both are different, Do you want to continue with " & BillDate.ToString("dd/MM/yyyy") & "?", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    Case DialogResult.Yes
                        Return True
                    Case DialogResult.No
                        BillDate = RetBilldate
                        Return True
                End Select
            Else
                Return True
            End If
        End If
        Return False
    End Function
    Public Function GetSqlTable(ByVal StrSql As String, ByVal lCn As OleDbConnection, Optional ByVal Tran As OleDbTransaction = Nothing) As DataTable
        Dim Cmd As New OleDbCommand(StrSql, lCn)
        If Tran IsNot Nothing Then Cmd.Transaction = Tran
        Dim Da As New OleDbDataAdapter(Cmd)
        Dim DtTemp As New DataTable
        Da.Fill(DtTemp)
        Return DtTemp
    End Function
    Public Function GetSqlRow(ByVal StrSql As String, ByVal lCn As OleDbConnection, Optional ByVal Tran As OleDbTransaction = Nothing) As DataRow
        Dim Dr As DataRow = Nothing
        Dim Cmd As New OleDbCommand(StrSql, lCn)
        If Tran IsNot Nothing Then Cmd.Transaction = Tran
        Dim Da As New OleDbDataAdapter(Cmd)
        Dim DtTemp As New DataTable
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Dr = DtTemp.Rows(0)
        End If
        Return Dr
    End Function

    Function funcComboChecker(ByVal field As String, ByVal cmb As ComboBox, Optional ByVal emptyCheck As Boolean = True) As Boolean
        If cmb.Text.Contains("'") Then
            MsgBox("Single Quote Not Allowed", MsgBoxStyle.Information)
            Return True
        End If
        If emptyCheck Then
            If cmb.Text = "" Then
                MsgBox(field + " Should Not Empty", MsgBoxStyle.Information)
                cmb.Focus()
                Return True
            End If
        End If
        If cmb.Items.Count > 0 Then
            If cmb.Text <> "" Then
                If cmb.Items.Contains(cmb.Text) = False Then
                    MsgBox("Invalid " + field, MsgBoxStyle.Information)
                    cmb.Focus()
                    Return True
                End If
            End If
        End If
    End Function

    Public Sub LoadDesigner(ByRef chkLstBox As CheckedListBox)
        chkLstBox.Items.Clear()
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DESIGNERNAME"
        Dim dtDesigner As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        For Each ro As DataRow In dtDesigner.Rows
            chkLstBox.Items.Add(ro("DESIGNERNAME").ToString)
            If ro("DESIGNERNAME").ToString = strDesignerName Then chkLstBox.SetItemChecked(chkLstBox.Items.Count - 1, True)
        Next
    End Sub
    Public Function ConvertDataGridViewToHTML(ByVal dgv As DataGridView, Optional ByVal dgvHdr As DataGridView = Nothing)
        Dim html As String = ""
        html = "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
        If dgvHdr.Columns.Count > 0 Then
            html += "<table border=1 align=center id=tblprint style=""width: " & dgvHdr.Width & "px; height: " & dgvHdr.Height & "px"">"
            html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt ; width: " & dgvHdr.Width & "px; height: " & dgvHdr.Height & "px"">"
            For i As Integer = 0 To dgvHdr.Columns.Count - 1
                If dgvHdr.Columns(i).Visible = False Then Continue For
                html += "<td style=""width: " & dgvHdr.Columns(i).Width & "px; height: " & dgvHdr.Height & "px"">" + dgvHdr.Columns(i).HeaderText + "</td>"
            Next
            html += "</tr>"
            html += "</table>"
        End If
        html += "<table border=1 align=center id=tblprint style=""width: " & dgv.Width & "px; height: " & dgv.Height & """>"
        html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt"">"
        For i As Integer = 0 To dgv.Columns.Count - 1
            If dgv.Columns(i).Visible = True Then
                html += "<td>" + dgv.Columns(i).HeaderText + "</td>"
            Else
                'html += "<td style=""Display:None"">" + dgv.Columns(i).HeaderText + "</td>"
            End If
        Next
        html += "</tr>"
        For i As Integer = 0 To dgv.Rows.Count - 1
            html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt; width: " & dgv.Width & "px; height: " & dgv.Rows(i).Height & "px""> "
            For j As Integer = 0 To dgv.Columns.Count - 1
                If dgv.Columns(j).Visible = True Then
                    Dim strCol As String = ""
                    strCol = dgv.Rows(i).Cells(j).Value.ToString()
                    Dim intFontBold As Integer = FontStyle.Bold
                    If intFontBold = 1 Then
                        strCol = "<b>" & strCol & "</b>"
                    Else
                        strCol = strCol
                    End If
                    If Val(dgv.Rows(i).Cells(j).Value.ToString()) = 0 Then
                        html += "<td style=""width: " & dgv.Columns(j).Width & "px; height: " & dgv.Rows(i).Height & "px"">" + strCol + "</td>"
                    Else
                        html += "<td style=""width: " & dgv.Columns(j).Width & "px; height: " & dgv.Rows(i).Height & "px"" align=""right"">" + strCol + "</td>"
                    End If
                End If
            Next
            html += "</tr>"
        Next
        html += "</table>"
        Return html
    End Function
    'Public Function usrpwdok(ByVal PWDCTRL As String, ByVal IS_USERLEVELPWD As Boolean) As Boolean

    '    If IS_USERLEVELPWD Then
    '        Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
    '        Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
    '        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
    '        If Optionid = 0 Then pwdpass = True
    '        If userId <> 999 And Optionid <> 0 Then
    '            pwdid = GetuserPwd(Optionid, cnCostId, userId)
    '            If pwdid <> 0 Then
    '                Dim objUpwd As New frmUserPassword(pwdid)
    '                If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then pwdpass = False Else pwdpass = True
    '            End If
    '        Else
    '            pwdpass = True
    '        End If
    '        Return pwdpass
    '    Else
    '        Return True
    '    End If
    '    Return False
    'End Function
    Public Function funcGstView(ByVal rptDate As Date) As Boolean
        If rptDate >= GstDate Then
            Return True
        Else
            Return False
        End If
    End Function

    Function funcGenPwd(ByVal optionId As Integer, ByVal MobileNo As String, ByVal Msg As String, Optional ByVal _rep As Boolean = False) As Integer
        If MobileNo = "" Then MsgBox("MobileNo not Updated...") : Exit Function
        Dim _r As Random = New Random
        Dim pwdid As Int32
        strSql = "SELECT ISNULL(MAX(PWDID),0) FROM " & cnAdminDb & "..PWDMASTER"
        pwdid = Val(GetSqlValue(cn, strSql)) + 1
        Dim anew As Long
        For ii As Integer = 0 To Len(pwdid) - 1
            anew += _r.Next()
        Next
        Dim empy As Int32 = 0
        anew = Mid(anew, 1, 4)
        If _rep Then
            Msg = Msg.Replace("<OTP>", anew)
        End If
        If MobileNo.Contains(",") Then
            Dim Mob() As String = MobileNo.Split(",")
            For i As Integer = 0 To Mob.Length - 1
                strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='E',PWDCLSDATE= '" + GetServerDate() + "',PWDCLSTIME='" & GetServerTime() + "'"
                strSql += " WHERE REMARKS='" & Mob(i) & "'"
                strSql += " AND PWDOPTIONID='" & optionId & "'"
                strSql += " AND PWDSTATUS='N' "
                ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)

                strSql = "INSERT INTO " & cnAdminDb & "..PWDMASTER(PWDID,COSTID,PWDDATE,PWDTIME,PWDUSERID,"
                strSql += vbCrLf + "PASSWORD,PWDOPTIONID,PWDEXPIRY,PWDSTATUS,PWDCLSDATE,PWDCLSTIME,CRUSERID,REMARKS)"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "" & pwdid & "" 'PWDID
                strSql += vbCrLf + ",'" & cnCostId & "'"
                strSql += vbCrLf + ",'" & GetEntryDate(GetServerDate) & "'" 'PWDDATE
                strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'PWDTIME
                strSql += vbCrLf + ",'" & userId & "'" 'PWDUSERID
                strSql += vbCrLf + ",'" & BrighttechPack.Methods.Encrypt(anew) & "'" 'PASSWORD
                strSql += vbCrLf + ",'" & optionId & "'" 'PWDOPTIONID
                strSql += vbCrLf + "," & empy & "" 'PWDEXPIRY
                strSql += vbCrLf + ",'N'" 'PWDSTATUS
                strSql += vbCrLf + ",''" 'PWDCLSDATE
                strSql += vbCrLf + ",''" 'PWDCLSTIME
                strSql += vbCrLf + "," & userId & "" 'CRUSERID
                strSql += vbCrLf + ",'" & Mob(i) & "'"
                strSql += vbCrLf + ")" 'REMARKS
                ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
                If _rep Then
                    SmsSend(Msg, Mob(i))
                Else
                    SmsSend(Msg, Mob(i))
                End If
            Next
        Else
            strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='E',PWDCLSDATE= '" + GetServerDate() + "',PWDCLSTIME='" & GetServerTime() + "'"
            strSql += " WHERE REMARKS='" & MobileNo & "'"
            strSql += " AND PWDOPTIONID='" & optionId & "'"
            strSql += " AND PWDSTATUS='N' "
            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)

            strSql = "INSERT INTO " & cnAdminDb & "..PWDMASTER(PWDID,COSTID,PWDDATE,PWDTIME,PWDUSERID,"
            strSql += vbCrLf + "PASSWORD,PWDOPTIONID,PWDEXPIRY,PWDSTATUS,PWDCLSDATE,PWDCLSTIME,CRUSERID,REMARKS)"
            strSql += vbCrLf + "VALUES("
            strSql += vbCrLf + "" & pwdid & "" 'PWDID
            strSql += vbCrLf + ",'" & cnCostId & "'"
            strSql += vbCrLf + ",'" & GetEntryDate(GetServerDate) & "'" 'PWDDATE
            strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'PWDTIME
            strSql += vbCrLf + ",'" & userId & "'" 'PWDUSERID
            strSql += vbCrLf + ",'" & BrighttechPack.Methods.Encrypt(anew) & "'" 'PASSWORD
            strSql += vbCrLf + ",'" & optionId & "'" 'PWDOPTIONID
            strSql += vbCrLf + "," & empy & "" 'PWDEXPIRY
            strSql += vbCrLf + ",'N'" 'PWDSTATUS
            strSql += vbCrLf + ",''" 'PWDCLSDATE
            strSql += vbCrLf + ",''" 'PWDCLSTIME
            strSql += vbCrLf + "," & userId & "" 'CRUSERID
            strSql += vbCrLf + ",'" & MobileNo & "'" 'REMARKS
            strSql += vbCrLf + ")"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
            If _rep Then
                SmsSend(Msg, MobileNo)
            Else
                SmsSend(Msg, MobileNo)
            End If
        End If
    End Function
    Public Function SmsSend(ByVal Msg As String, ByVal Mobile As String) As Boolean
        If Msg <> "" And Mobile.Length = 10 Then
            strSql = "SELECT COUNT(*)CNT FROM SYSDATABASES WHERE NAME='AKSHAYASMSDB'"
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                strSql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,EXPIRYDATE,UPDATED)"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Mobile.Trim & "',N'" & Msg & "','N'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ) "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                Return True
            Else
                MsgBox("AkhayaSmsDb Not Found", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function
End Module
