Imports System.Data.OleDb
Imports System.Threading
Public Class frmPrevilegetran
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable
    Dim PREV_RECORD As Boolean = IIf(GetAdmindbSoftValue("PREV_RECORD", "N") = "Y", True, False)
    Dim PRETYPE As Boolean = IIf(GetAdmindbSoftValue("PRETYPE", "N") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnStockDb & "..SoftControlTran"
        strSql += vbCrLf + " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Function funcSep(ByVal diaType As String) As String
        Dim Qry As String = Nothing
        'If chkWithSR.Checked = True Then
        '    ''--GENERATE SEP A/C
        '    Qry += " UNION ALL"
        '    Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME"
        '    Qry += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID   "
        '    Qry += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH   "
        '    Qry += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        '    Qry += " ,TRANDATE,TRANNO,TRANTYPE  "
        '    Qry += " ,SUM(-1*PCS)PCS,SUM(-1*GRSWT)GRSWT,SUM(-1*NETWT)NETWT,SUM(-1*AMOUNT)AMOUNT  ,SUM(-1*VAT)VAT "
        '    Qry += " ,SUM(-1*AMOUNT)+SUM(-1*VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'2'SALETYPE  "
        '    Qry += " FROM   "
        '    Qry += " ("
        '    Qry += " SELECT X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO "
        '    Qry += " ,(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO = ISSSNO AND COMPANYID = '" & strCompanyId & "')TRANDATE"
        '    Qry += " ,SUM(X.AMOUNT)AMOUNT "
        '    Qry += " ,ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV' AND TRANNO = X.TRANNO  AND COMPANYID = '" & strCompanyId & "' AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT   "
        '    Qry += " ,SUM(X.PCS)PCS ,SUM(X.WT)AS GRSWT ,SUM(X.WT)AS NETWT ,SUM(X.STNWT)STNWT "
        '    Qry += " ,SUM(X.DIAWT)DIAWT ,X.TRANTYPE ,X.BATCHNO,'1'RESULT   "
        '    Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS CUSTOMER   "
        '    Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        '    Qry += " FROM "
        '    Qry += " ( "
        '    Qry += " SELECT  CATCODE ,TRANNO ,ISSSNO ,BATCHNO ,TRANTYPE "
        '    Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME "
        '    Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID   "
        '    Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME   "
        '    Qry += " ,SUM(STNAMT)AMOUNT ,SUM(STNPCS)PCS "
        '    Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT "
        '    Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT "
        '    Qry += " ,SUM(STNWT)WT"
        '    Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND TRANTYPE = 'SR' AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        '    Qry += strFtr2
        '    Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
        '    Qry += " )AS X"
        '    Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,X.BATCHNO,X.TRANTYPE,X.PCS"
        '    Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO "
        '    ''--DEDUCT SEP A/C AMT
        '    Qry += " UNION ALL"
        '    Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
        '    Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
        '    Qry += " ,SUM(VAT)VAT"
        '    Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'2'SALETYPE "
        '    Qry += " FROM  "
        '    Qry += " ("
        '    Qry += " SELECT "
        '    Qry += " I.CATCODE"
        '    Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME"
        '    Qry += " ,I.TRANNO"
        '    Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        '    Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        '    Qry += " ,I.SNO"
        '    Qry += " ,DATEPART(MONTH,I.TRANDATE)AS MMONTHID  "
        '    Qry += " ,LEFT(DATENAME(MONTH,I.TRANDATE),3)MMONTH  "
        '    Qry += " ,CONVERT(VARCHAR,I.TRANDATE,103)AS UUPDATED ,TRANDATE  "
        '    Qry += " ,SUM(-1*X.AMOUNT)AMOUNT"
        '    Qry += " ,ISNULL((SELECT -1*AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV'  AND COMPANYID = '" & strCompanyId & "' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT  "
        '    Qry += " ,SUM(X.STNWT)STNWT"
        '    Qry += " ,SUM(X.DIAWT)DIAWT"
        '    Qry += " ,I.TRANTYPE ,I.BATCHNO,'1'RESULT  "
        '    Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
        '    Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        '    Qry += " FROM"
        '    Qry += " ("
        '    Qry += " SELECT "
        '    Qry += " CATCODE"
        '    Qry += " ,TRANNO"
        '    Qry += " ,ISSSNO"
        '    Qry += " ,BATCHNO"
        '    Qry += " ,TRANTYPE"
        '    Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME"
        '    Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID  "
        '    Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME  "
        '    Qry += " ,SUM(STNAMT)AMOUNT"
        '    Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT"
        '    Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'T' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT"
        '    Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND TRANTYPE = 'SR' AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        '    Qry += strFtr2
        '    Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
        '    Qry += " )AS X," & cnStockDb & "..RECEIPT AS I"
        '    Qry += strFtr
        '    Qry += " AND X.ISSSNO = I.SNO"
        '    Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,I.BATCHNO,X.BATCHNO,X.CATCODE,X.TRANNO"
        '    Qry += " )AS SEPDIA"
        '    Qry += " GROUP BY"
        '    Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        'End If
        ''--GENERATE SEP A/C
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME"
        Qry += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID   "
        Qry += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH   "
        Qry += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        Qry += " ,TRANDATE,TRANNO,TRANTYPE  "
        Qry += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT  ,SUM(VAT)VAT "
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'1'SALETYPE  "
        Qry += " FROM   "
        Qry += " ("
        Qry += " SELECT X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO "
        Qry += " ,(SELECT TRANDATE FROM " & cnStockDb & "..ISSUE WHERE SNO = ISSSNO AND COMPANYID = '" & strCompanyId & "')TRANDATE"
        Qry += " ,SUM(X.AMOUNT)AMOUNT "
        Qry += " ,ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND PAYMODE = 'SV' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT   "
        Qry += " ,SUM(X.PCS)PCS ,SUM(X.WT)AS GRSWT ,SUM(X.WT)AS NETWT ,SUM(X.STNWT)STNWT "
        Qry += " ,SUM(X.DIAWT)DIAWT ,X.TRANTYPE ,X.BATCHNO,'1'RESULT   "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO)),'')AS CUSTOMER   "
        Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        Qry += " FROM "
        Qry += " ( "
        Qry += " SELECT  CATCODE ,TRANNO ,ISSSNO ,BATCHNO ,TRANTYPE "
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME "
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID   "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME   "
        Qry += " ,SUM(STNAMT)AMOUNT ,SUM(STNPCS)PCS "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'S' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT "
        Qry += " ,SUM(STNWT)WT"
        Qry += " FROM " & cnStockDb & "..ISSSTONE AS S WHERE 1=1 AND TRANTYPE IN ('SA','OD','RD') AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
        Qry += " )AS X"
        Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,X.BATCHNO,X.TRANTYPE,X.PCS"
        Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO "
        ''--DEDUCT SEP A/C AMT
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
        Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
        Qry += " ,SUM(VAT)VAT"
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'1'SALETYPE "
        Qry += " FROM  "
        Qry += " ("
        Qry += " SELECT "
        Qry += " I.CATCODE"
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME"
        Qry += " ,I.TRANNO"
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        Qry += " ,I.SNO"
        Qry += " ,DATEPART(MONTH,I.TRANDATE)AS MMONTHID  "
        Qry += " ,LEFT(DATENAME(MONTH,I.TRANDATE),3)MMONTH  "
        Qry += " ,CONVERT(VARCHAR,I.TRANDATE,103)AS UUPDATED ,TRANDATE  "
        Qry += " ,SUM(-1*X.AMOUNT)AMOUNT"
        Qry += " ,ISNULL((SELECT -1*AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND PAYMODE = 'SV' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT  "
        Qry += " ,SUM(X.STNWT)STNWT"
        Qry += " ,SUM(X.DIAWT)DIAWT"
        Qry += " ,I.TRANTYPE ,I.BATCHNO,'1'RESULT  "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
        Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        Qry += " FROM"
        Qry += " ("
        Qry += " SELECT "
        Qry += " CATCODE"
        Qry += " ,TRANNO"
        Qry += " ,ISSSNO"
        Qry += " ,BATCHNO"
        Qry += " ,TRANTYPE"
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME"
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID  "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME  "
        Qry += " ,SUM(STNAMT)AMOUNT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'T' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT"
        Qry += " FROM " & cnStockDb & "..ISSSTONE AS S WHERE 1=1 AND TRANTYPE IN ('SA','OD','RD') AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
        Qry += " )AS X," & cnStockDb & "..ISSUE AS I"
        Qry += strFtr
        Qry += " AND X.ISSSNO = I.SNO"
        Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,I.BATCHNO,X.BATCHNO,X.CATCODE,X.TRANNO"
        Qry += " )AS SEPDIA"
        Qry += " GROUP BY"
        Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        Return Qry
    End Function

    Function funcSepStnAc() As String
        Dim Qry As String = Nothing
        If SepDiaPost = "Y" Then
            Qry += funcSep("D")
        End If
        If SepStnPost = "Y" Then
            Qry += funcSep("T")
        End If
        If SepPrePost = "Y" Then
            Qry += funcSep("P")
        End If
        Return Qry
    End Function

    Function funcAbstract() As Integer
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSTEMP')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABSTEMP"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,LTRIM(STR(TRANNO))TRANNO,TRANTYPE"
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT"
        strSql += vbCrLf + " ,SUM(AMOUNT)AS TOTAL"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
        strSql += vbCrLf + " ,'1'RESULT"
        strSql += vbCrLf + " ,'1'SALETYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ABSTEMP"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CATCODE"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strSql += vbCrLf + " ,ITEMID"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        strSql += vbCrLf + " ,CONVERT(INT,CONVERT(VARCHAR,DATEPART(YEAR,TRANDATE))+REPLICATE('0',2-LEN((DATEPART(MONTH,TRANDATE)))) + CONVERT(VARCHAR,DATEPART(MONTH,TRANDATE)))AS MMONTHID"
        strSql += vbCrLf + " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        strSql += vbCrLf + " ,TRANDATE"
        strSql += vbCrLf + " ,TRANNO"
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO)  AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'T')),0)AMOUNT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO) AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO) AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)DIAWT"
        strSql += vbCrLf + " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        strSql += vbCrLf + "  ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        strSql += vbCrLf + "  ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        strSql += vbCrLf + " ,TRANTYPE,BATCHNO"
        strSql += vbCrLf + " ,'1'RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += strFtr
        strSql += vbCrLf + " AND TRANTYPE IN ('SA','OD','RD')"
        strSql += vbCrLf + " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,SNO"
        strSql += vbCrLf + " )AS X"
        strSql += vbCrLf + " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        'If chkWithSR.Checked = True Then
        '    ''****************WITH SALES RETURN***************
        '    strsql += vbcrlf + " UNION ALL"
        '    strsql += vbcrlf + " SELECT"
        '    strsql += vbcrlf + " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        '    strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
        '    strsql += vbcrlf + " ,(SUM(AMOUNT)-SUM(STNAMT)) AMOUNT"
        '    strsql += vbcrlf + " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = Y.BATCHNO AND PAYMODE = 'SV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = Y.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = Y.CATCODE)),0)AS VAT"
        '    strsql += vbcrlf + " ,SUM(AMOUNT) AS TOTAL"
        '    strsql += vbcrlf + " ,SUM(STNWT)STNWT"
        '    strsql += vbcrlf + " ,SUM(DIAWT)DIAWT"
        '    strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
        '    strsql += vbcrlf + " ,'1'RESULT"
        '    strsql += vbcrlf + " ,'2'SALETYPE"
        '    strsql += vbcrlf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
        '    strsql += vbcrlf + " FROM "
        '    strsql += vbcrlf + " ("
        '    strsql += vbcrlf + " SELECT"
        '    strsql += vbcrlf + " CATCODE"
        '    strsql += vbcrlf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        '    strsql += vbcrlf + " ,ITEMID"
        '    strsql += vbcrlf + " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        '    strsql += vbcrlf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        '    strsql += vbcrlf + " ,CONVERT(INT,CONVERT(VARCHAR,DATEPART(YEAR,TRANDATE))+REPLICATE('0',2-LEN((DATEPART(MONTH,TRANDATE)))) + CONVERT(VARCHAR,DATEPART(MONTH,TRANDATE)))AS MMONTHID"
        '    'strsql += vbcrlf + " ,DATEPART(MONTH,TRANDATE)AS MMONTHID"
        '    strsql += vbcrlf + " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        '    strsql += vbcrlf + " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        '    strsql += vbcrlf + " ,TRANDATE"
        '    strsql += vbcrlf + " ,TRANNO"
        '    'strsql += vbcrlf + " ,convert(varchar,TRANNO)TRANNO"
        '    strsql += vbcrlf + " ,-1*SUM(PCS) PCS,-1*SUM(GRSWT)GRSWT,-1*SUM(NETWT)NETWT,-1*SUM(AMOUNT)AMOUNT"
        '    strsql += vbcrlf + " ,ISNULL((SELECT -1*SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'PV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = I.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)),0)AS VAT"
        '    strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
        '    strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNAMT"
        '    strsql += vbcrlf + " ,ISNULL((SELECT -1*SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)DIAWT"
        '    strSql += vbCrLf + " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        '    strSql += vbCrLf + "  ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    strSql += vbCrLf + "  ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        '    strsql += vbcrlf + " ,TRANTYPE,BATCHNO"
        '    strsql += vbcrlf + " ,'1'RESULT"
        '    strsql += vbcrlf + " FROM " & cnStockDb & "..RECEIPT AS I"
        '    strSql += strFtr
        '    strsql += vbcrlf + " AND TRANTYPE = 'SR'"
        '    strsql += vbcrlf + " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,TRANDATE"
        '    strsql += vbcrlf + " )AS Y"
        '    strSql += vbCrLf + " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        'End If
        strSql += funcSepStnAc()
        strSql += vbCrLf + " ORDER BY TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 600
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABS')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABS"
        strSql += vbCrLf + " SELECT "
        'If rbtSummary.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,'   '+CATNAME CATNAME,' 'ITEMID,METALID,METALNAME,' 'MMONTHID,' 'MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtMonth.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,MMONTHID,'   '+MMONTH MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtDate.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,'   '+UUPDATED UUPDATED,TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtBillNo.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        'End If
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,SUM(VAT)VAT"
        strSql += vbCrLf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
        'If rbtBillNo.Checked = True Then
        '    strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
        'Else
        strSql += vbCrLf + " ,' 'CUSTOMER,' ' ADDRESS,' ' PHONENO"
        'End If
        strSql += vbCrLf + " ,'1'RESULT"
        strSql += vbCrLf + " ,SALETYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ABS"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " TEMP" & systemId & "ABSTEMP"
        'strsql += vbcrlf + " AS Z"
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY SALETYPE,CATCODE,CATNAME,METALNAME,METALID"
        'ElseIf rbtMonth.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY SALETYPE,CATCODE,CATNAME,MMONTHID,MMONTH"
        'ElseIf rbtDate.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY SALETYPE,CATCODE,CATNAME,UUPDATED,TRANDATE"
        'ElseIf rbtBillNo.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY SALETYPE,CATCODE,CATNAME,TRANNO,CUSTOMER,ADDRESS,PHONENO,TRANTYPE,UUPDATED,TRANDATE"
        '    strSql += vbCrLf + " ORDER BY CONVERT(INT,TRANNO)"
        'End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcAbsSub() As Integer
        ''SUMMARY WISE SUBTOTAL
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSSUB')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABSSUB"
        strSql += vbCrLf + " SELECT"
        'If rbtSummary.Checked = True Then
        strSql += vbCrLf + " ' 'CATCODE,' 'CATNAME,' 'ITEMID,METALID,'SUB TOTAL'METALNAME"
        strSql += vbCrLf + " ,' 'MMONTHID"
        strSql += vbCrLf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtMonth.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
        '    strsql += vbcrlf + " ,' 'MMONTHID"
        '    strsql += vbcrlf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtDate.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
        '    strsql += vbcrlf + " ,' 'MMONTHID"
        '    strsql += vbcrlf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtBillNo.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
        '    strsql += vbcrlf + " ,' 'MMONTHID"
        '    strsql += vbcrlf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'End If
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,SUM(VAT)VAT"
        strSql += vbCrLf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,' 'CUSTOMER,' ' ADDRESS,' ' PHONENO"
        strSql += vbCrLf + " ,'2'RESULT"
        strSql += vbCrLf + " ,' 'SALETYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'S') COLHEAD"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ABSSUB"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "ABS"
        strSql += vbCrLf + " )AS Z"
        'If rbtSummary.Checked = True Then
        strSql += vbCrLf + " GROUP BY METALNAME,METALID"
        'ElseIf rbtMonth.Checked = True Then
        'strSql += vbCrLf + " GROUP BY CATCODE"
        'ElseIf rbtDate.Checked = True Then
        'strSql += vbCrLf + " GROUP BY CATCODE"
        'ElseIf rbtBillNo.Checked = True Then
        'strSql += vbCrLf + " GROUP BY CATCODE"
        'End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcAbsGrand() As Integer
        ''SUMMARY WISE SUBTOTAL
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSGRAND')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABSGRAND"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " 'ZZZZ'CATCODE,'GRAND TOTAL'CATNAME,' 'ITEMID,'ZZZZ'METALID,'ZZZZ'METALNAME"
        strSql += vbCrLf + " ,' 'MMONTHID"
        strSql += vbCrLf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,SUM(VAT)VAT"
        strSql += vbCrLf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,' 'CUSTOMER,' ' ADDRESS,' ' PHONENO"
        strSql += vbCrLf + " ,'3'RESULT"
        strSql += vbCrLf + " ,' 'SALETYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'G') COLHEAD"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ABSGRAND"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "ABSSUB"
        strSql += vbCrLf + " )AS Z"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcGridViewStyle() As Integer
        With gridView
            .Columns("CATCODE").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("MMONTHID").Visible = False
            .Columns("METALNAME").Visible = False
            .Columns("MMONTH").Visible = False
            .Columns("UUPDATED").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("TRANNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("SALETYPE").Visible = False
            .Columns("METALID").Visible = False

            For cnt As Integer = 0 To gridView.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            '.Columns("UUPDATED").Visible = rbtBillNo.Checked
            .Columns("UUPDATED").HeaderText = "TRANDATE"
            'If chkWithSR.Checked = True And rbtBillNo.Checked = True Then
            .Columns("TRANTYPE").Visible = True
            .Columns("TRANTYPE").HeaderText = "TYPE"
            .Columns("TRANTYPE").Width = 40
            'Else
            '.Columns("TRANTYPE").Visible = False
            'End If
            With .Columns("PARTICULAR")
                .Width = 250
                '    If rbtSummary.Checked = True Then
                '.HeaderText = "CATEGORY"
                'ElseIf rbtMonth.Checked = True Then
                '.HeaderText = "MONTH"
                'ElseIf rbtDate.Checked = True Then
                '.HeaderText = "DATE"
                'Else
                .HeaderText = "BILL NO"
                'End If
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                If chkPcs.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                If chkGrsWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                If chkNetWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("AMOUNT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("VAT")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTAL")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("STNWT")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DIAWT")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CUSTOMER")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                '.Visible = rbtBillNo.Checked
            End With
            With .Columns("ADDRESS")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                '.Visible = rbtBillNo.Checked
            End With
            With .Columns("PHONENO")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                '.Visible = rbtBillNo.Checked
            End With
        End With
        'If dsGridView.Tables.Contains("TITLE") = True Then
        '    For rwIndex As Integer = 0 To gridView.RowCount - 1
        '        For colIndex As Integer = 0 To gridView.ColumnCount - 1
        '            If colIndex = 0 Then
        '                For CNT As Integer = 0 To dsGridView.Tables("TITLE").Rows.Count - 1
        '                    If rwIndex = dsGridView.Tables("TITLE").Rows(CNT).Item("TITLE") Then
        '                        gridView.Rows(rwIndex).Cells(colIndex).Style.BackColor = Color.LightBlue
        '                        gridView.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("Verdana", 8, FontStyle.Bold)
        '                    End If
        '                Next
        '            End If
        '            For CNT As Integer = 0 To dsGridView.Tables("SUBTOTAL").Rows.Count - 1
        '                If rwIndex = dsGridView.Tables("SUBTOTAL").Rows(CNT).Item("SUBTOTAL") Then
        '                    'gridView.Rows(rwIndex).Cells(colIndex).Style.BackColor = Color.White
        '                    gridView.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("Verdana", 8, FontStyle.Bold)
        '                End If
        '            Next
        '            'GRAND TOTAL
        '            gridView.Rows(gridView.RowCount - 1).Cells(colIndex).Style.Font = New Font("Verdana", 8, FontStyle.Bold)
        '            gridView.Rows(gridView.RowCount - 1).Cells(colIndex).Style.BackColor = Color.LightYellow
        '        Next
        '    Next
        'End If

        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function

    Function funcFiltration() As String
        'Dim Qry As String = Nothing
        'Qry = " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        'If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then
        '    Qry += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        'End If
        'Qry += " AND ISNULL(CANCEL,'') <> 'Y'"
        'If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
        '    Qry += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        'End If
        'If cmbCategory.Text <> "" And cmbCategory.Text <> "ALL" Then
        '    Qry += " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        'End If
        'Qry += " AND COMPANYID = '" & strCompanyId & "' "
        'Return Qry
        Return ""
    End Function

    Private Sub PRIVTRAN()

        gridView.DataSource = Nothing
        Me.Refresh()
        Dim StartDate As Date
        StartDate = dtpFrom.Value.ToString("yyyy-MM-dd")
        Dim Fromdate As Date = IIf(GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate) = "", StartDate, GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate))

        Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        If PREV_RECORD Then
            strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPLOYALTY','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPLOYALTY"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT (VARCHAR(20),NULL) [PARTICULAR]"
            strSql += vbCrLf + ",CONVERT (VARCHAR(20),NULL) [PREVILEGEID]"
            strSql += vbCrLf + ",CONVERT (SMALLDATETIME ,NULL) [TRANDATE]"
            strSql += vbCrLf + ",CONVERT (INT,NULL) [PCS]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,3),NULL) [GRSWT]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,3),NULL) [NETWT]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,2),NULL) [AMOUNT]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,2),NULL) [TAX]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,2),NULL) [TOTAL]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,2),NULL) [POINTS]"
            strSql += vbCrLf + ",CONVERT (NUMERIC(12,2),NULL) [PVALUE]"
            strSql += vbCrLf + ",CONVERT (INT,NULL) [RESULT]"
            strSql += vbCrLf + ",CONVERT (VARCHAR(1),NULL) [COLHEAD]"
            strSql += vbCrLf + ",CONVERT (VARCHAR(1),NULL) [TRANTYPE]	  "
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPLOYALTY WHERE 1<>1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + " SELECT DISTINCT DBNAME FROM " + cnAdminDb + "..DBMASTER "
            strSql += vbCrLf + " WHERE STARTDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            strSql += vbCrLf + " OR ENDDATE  BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            strSql += vbCrLf + " OR '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' BETWEEN STARTDATE AND ENDDATE"
            strSql += vbCrLf + " OR '" + dtpTo.Value.ToString("yyyy-MM-dd") + "' BETWEEN STARTDATE AND ENDDATE"
            Dim _DA As OleDbDataAdapter = New OleDbDataAdapter(strSql, cn)
            Dim _Dt As DataTable = New DataTable
            _DA.Fill(_Dt)
            For i As Integer = 0 To _Dt.Rows.Count - 1

                strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUERECIPT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUERECIPT"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = vbCrLf + "  SELECT TRANNO, TRANDATE,TRANTYPE,PCS,GRSWT ,NETWT,LESSWT , PUREWT ,AMOUNT,TAX, TAGNO, ITEMID "
                strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID "
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPISSUERECIPT"
                strSql += vbCrLf + "  FROM " + _Dt.Rows(i).Item("DBNAME").ToString + "..ISSUE "
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
                strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select TRANNO, TRANDATE, TRANTYPE, PCS, GRSWT, NETWT, LESSWT, PUREWT, AMOUNT, TAX, TAGNO, ITEMID "
                strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID FROM " + _Dt.Rows(i).Item("DBNAME").ToString + "..RECEIPT "
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
                strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SR') AND BATCHNO NOT IN ("
                strSql += vbCrLf + "  SELECT DISTINCT BATCHNO FROM " + _Dt.Rows(i).Item("DBNAME").ToString + "..ISSUE "
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
                strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD'))"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()


                strSql = vbCrLf + ""
                strSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY"
                strSql += vbCrLf + "SELECT * FROM ("
                strSql += vbCrLf + "SELECT CASE WHEN T.TRANNO IS NULL THEN 'OPENING' "
                strSql += vbCrLf + "WHEN ISNULL(T.TRANNO,'')='9999' THEN (SELECT TOP 1 ISNULL(GROUPCODE,'')+'-'+ISNULL(CONVERT(VARCHAR,REGNO),'') FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE ISNULL(PREVILEGEID,'')=T.PREVILEGEID)"
                strSql += vbCrLf + "ELSE CAST(T.TRANNO AS VARCHAR) END AS PARTICULAR"
                strSql += vbCrLf + ",PREVILEGEID,T.TRANDATE"
                strSql += vbCrLf + ",SUM(I.PCS)PCS"
                strSql += vbCrLf + ",SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(I.AMOUNT)AMOUNT"
                strSql += vbCrLf + ",SUM(I.TAX)TAX"
                strSql += vbCrLf + ",SUM(I.AMOUNT)+ISNULL(SUM(I.TAX),0) AS TOTAL"
                strSql += vbCrLf + ",POINTS,T.PVALUE "
                strSql += vbCrLf + ",CASE WHEN T.TRANNO IS NULL THEN 0 ELSE 1  END AS RESULT"
                strSql += vbCrLf + ",'' AS COLHEAD"
                strSql += vbCrLf + ",CASE WHEN ENTRYTYPE='C' THEN 'C' ELSE 'A' END AS TRANTYPE"
                strSql += vbCrLf + "FROM " & cnAdminDb & "..PRIVILEGETRAN T INNER JOIN  TEMPTABLEDB..TEMPISSUERECIPT I ON I.BATCHNO = T.BATCHNO "
                strSql += vbCrLf + "WHERE  (T.TRANDATE BETWEEN '" & Format(Fromdate, "yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' OR T.TRANDATE IS NULL)"
                strSql += vbCrLf + "AND T.ENTRYTYPE <> 'C'"
                If TXTPREVILEGEID.Text <> "" Then strSql += vbCrLf + "AND PREVILEGEID='" & TXTPREVILEGEID.Text & "' "
                strSql += vbCrLf + "AND ISNULL(PREVILEGEID,'')<>''"
                strSql += vbCrLf + "AND T.TRANTYPE='R'"
                strSql += vbCrLf + "AND ISNULL(T.CANCEL,'')=''"
                strSql += vbCrLf + "GROUP BY POINTS,T.PVALUE,T.TRANNO,PREVILEGEID,T.TRANDATE,T.ENTRYTYPE"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT CASE WHEN T.TRANNO IS NULL THEN 'OPENING' "
                strSql += vbCrLf + "WHEN ISNULL(T.TRANNO,'')='9999' THEN (SELECT TOP 1 ISNULL(GROUPCODE,'')+'-'+ISNULL(CONVERT(VARCHAR,REGNO),'') FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE ISNULL(PREVILEGEID,'')=T.PREVILEGEID)"
                strSql += vbCrLf + "ELSE CAST(T.TRANNO AS VARCHAR) END AS TRANNO"
                strSql += vbCrLf + ",PREVILEGEID,T.TRANDATE"
                strSql += vbCrLf + ",NULL AS PCS"
                strSql += vbCrLf + ",NULL AS GRSWT"
                strSql += vbCrLf + ",NULL AS NETWT"
                strSql += vbCrLf + ",NULL AS AMOUNT"
                strSql += vbCrLf + ",NULL AS TAX"
                strSql += vbCrLf + ",NULL AS TOTAL"
                strSql += vbCrLf + ",POINTS,T.PVALUE "
                strSql += vbCrLf + ",CASE WHEN T.TRANNO IS NULL THEN 0 ELSE 1  END AS RESULT"
                strSql += vbCrLf + ",'' AS COLHEAD,'B' AS TRANTYPE"
                strSql += vbCrLf + "FROM " & cnAdminDb & "..PRIVILEGETRAN T INNER JOIN  TEMPTABLEDB..TEMPISSUERECIPT I ON I.BATCHNO = T.BATCHNO"
                strSql += vbCrLf + "WHERE  (T.TRANDATE BETWEEN '" & Format(Fromdate, "yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' OR T.TRANDATE IS NULL)"
                If TXTPREVILEGEID.Text <> "" Then strSql += vbCrLf + "AND PREVILEGEID='" & TXTPREVILEGEID.Text & "' "
                strSql += vbCrLf + "AND ISNULL(T.PREVILEGEID,'')<>''"
                strSql += vbCrLf + "AND T.TRANTYPE='I'"
                strSql += vbCrLf + "AND ISNULL(T.CANCEL,'')=''"
                strSql += vbCrLf + "AND T.ENTRYTYPE <> 'C'"
                strSql += vbCrLf + "GROUP BY POINTS,T.PVALUE,T.TRANNO,PREVILEGEID,T.TRANDATE,T.ENTRYTYPE"
                strSql += vbCrLf + ") X ORDER BY TRANTYPE,TRANDATE,PARTICULAR,RESULT"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            Next

            strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUERECIPT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUERECIPT"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "  SELECT TRANNO, TRANDATE,TRANTYPE,PCS,GRSWT ,NETWT,LESSWT , PUREWT ,AMOUNT,TAX, TAGNO, ITEMID "
            strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPISSUERECIPT"
            strSql += vbCrLf + "  FROM " + cnStockDb + "..ISSUE "
            strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  Select TRANNO, TRANDATE, TRANTYPE, PCS, GRSWT, NETWT, LESSWT, PUREWT, AMOUNT, TAX, TAGNO, ITEMID "
            strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID FROM " + cnStockDb + "..RECEIPT "
            strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SR') AND BATCHNO NOT IN ("
            strSql += vbCrLf + "  SELECT DISTINCT BATCHNO FROM " + cnStockDb + "..ISSUE "
            strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD'))"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + ""
            strSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY"
            strSql += vbCrLf + "SELECT * FROM ("
            strSql += vbCrLf + "SELECT CASE WHEN T.TRANNO IS NULL THEN 'OPENING' "
            strSql += vbCrLf + "WHEN ISNULL(T.TRANNO,'')='9999' THEN (SELECT TOP 1 ISNULL(GROUPCODE,'')+'-'+ISNULL(CONVERT(VARCHAR,REGNO),'') FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE ISNULL(PREVILEGEID,'')=T.PREVILEGEID)"
            strSql += vbCrLf + "ELSE CAST(T.TRANNO AS VARCHAR) END AS PARTICULAR"
            strSql += vbCrLf + ",PREVILEGEID,T.TRANDATE"
            strSql += vbCrLf + ",SUM(I.PCS)PCS"
            strSql += vbCrLf + ",SUM(I.GRSWT)GRSWT"
            strSql += vbCrLf + ",SUM(I.NETWT)NETWT"
            strSql += vbCrLf + ",SUM(I.AMOUNT)AMOUNT"
            strSql += vbCrLf + ",SUM(I.TAX)TAX"
            strSql += vbCrLf + ",SUM(I.AMOUNT)+ISNULL(SUM(I.TAX),0) AS TOTAL"
            strSql += vbCrLf + ",POINTS,T.PVALUE "
            strSql += vbCrLf + ",CASE WHEN T.TRANNO IS NULL THEN 0 ELSE 1  END AS RESULT"
            strSql += vbCrLf + ",'' AS COLHEAD"
            strSql += vbCrLf + ",CASE WHEN ENTRYTYPE='C' THEN 'C' ELSE 'A' END AS TRANTYPE"
            strSql += vbCrLf + "FROM " & cnAdminDb & "..PRIVILEGETRAN T LEFT JOIN   TEMPTABLEDB..TEMPISSUERECIPT I ON I.BATCHNO = T.BATCHNO "
            strSql += vbCrLf + "WHERE  T.TRANDATE BETWEEN '" & Format(Fromdate, "yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "AND T.ENTRYTYPE = 'C'"
            If TXTPREVILEGEID.Text <> "" Then strSql += vbCrLf + "AND PREVILEGEID='" & TXTPREVILEGEID.Text & "' "
            strSql += vbCrLf + "AND ISNULL(PREVILEGEID,'')<>''"
            strSql += vbCrLf + "AND T.TRANTYPE='R'"
            strSql += vbCrLf + "AND ISNULL(T.CANCEL,'')=''"
            strSql += vbCrLf + "GROUP BY POINTS,T.PVALUE,T.TRANNO,PREVILEGEID,T.TRANDATE,T.ENTRYTYPE"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT CASE WHEN T.TRANNO IS NULL THEN 'OPENING' "
            strSql += vbCrLf + "WHEN ISNULL(T.TRANNO,'')='9999' THEN (SELECT TOP 1 ISNULL(GROUPCODE,'')+'-'+ISNULL(CONVERT(VARCHAR,REGNO),'') FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE ISNULL(PREVILEGEID,'')=T.PREVILEGEID)"
            strSql += vbCrLf + "ELSE CAST(T.TRANNO AS VARCHAR) END AS TRANNO"
            strSql += vbCrLf + ",PREVILEGEID,T.TRANDATE"
            strSql += vbCrLf + ",NULL AS PCS"
            strSql += vbCrLf + ",NULL AS GRSWT"
            strSql += vbCrLf + ",NULL AS NETWT"
            strSql += vbCrLf + ",NULL AS AMOUNT"
            strSql += vbCrLf + ",NULL AS TAX"
            strSql += vbCrLf + ",NULL AS TOTAL"
            strSql += vbCrLf + ",POINTS,T.PVALUE "
            strSql += vbCrLf + ",CASE WHEN T.TRANNO IS NULL THEN 0 ELSE 1  END AS RESULT"
            strSql += vbCrLf + ",'' AS COLHEAD,'B' AS TRANTYPE"
            strSql += vbCrLf + "FROM " & cnAdminDb & "..PRIVILEGETRAN T LEFT JOIN   TEMPTABLEDB..TEMPISSUERECIPT I ON I.BATCHNO = T.BATCHNO"
            strSql += vbCrLf + "WHERE  T.TRANDATE BETWEEN '" & Format(Fromdate, "yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If TXTPREVILEGEID.Text <> "" Then strSql += vbCrLf + "AND PREVILEGEID='" & TXTPREVILEGEID.Text & "' "
            strSql += vbCrLf + "AND ISNULL(T.PREVILEGEID,'')<>''"
            strSql += vbCrLf + "AND T.TRANTYPE='I'"
            strSql += vbCrLf + "AND ISNULL(T.CANCEL,'')=''"
            strSql += vbCrLf + "AND T.ENTRYTYPE = 'C'"
            strSql += vbCrLf + ") X ORDER BY TRANTYPE,TRANDATE,PARTICULAR,RESULT"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            If chkchitOnly.Checked Then
                strSql = "DELETE TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE<>'C'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If
            If chkchit.Checked = False Then
                strSql = "DELETE TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='C'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,TRANTYPE)"
            strSql += vbCrLf + "SELECT TOP 1 'ACCUMULATED',-1,'T','A' FROM TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='A' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,TRANTYPE)"
            strSql += vbCrLf + "SELECT TOP 1 'SAVINGS',-1,'T','C' FROM TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='C' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,TRANTYPE)"
            strSql += vbCrLf + "SELECT TOP 1 'REDEEM',-1,'T','B' FROM TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='B' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,POINTS,PVALUE,TRANTYPE)"
            strSql += vbCrLf + "SELECT 'GRAND TOTAL',2,'S',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(AMOUNT),SUM(TAX),SUM(TOTAL)"
            strSql += vbCrLf + ",SUM(POINTS),SUM(PVALUE),TRANTYPE FROM TEMPTABLEDB..TEMPLOYALTY GROUP BY TRANTYPE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,POINTS,PVALUE,TRANTYPE)"
            strSql += vbCrLf + "SELECT 'BALANCE',3,'G'"
            strSql += vbCrLf + ",SUM(CASE WHEN TRANTYPE='B' THEN -1*POINTS ELSE POINTS END)"
            strSql += vbCrLf + ",SUM(CASE WHEN TRANTYPE='B' THEN -1*PVALUE ELSE PVALUE END)"
            strSql += vbCrLf + ",'Z'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPLOYALTY WHERE RESULT IN(0,1)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPLOYALTY ORDER BY TRANTYPE,RESULT,TRANDATE"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            Prop_Sets()
            If Not dtGrid.Rows.Count > 1 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            With gridView
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TRANTYPE").Visible = False
                .Columns("POINTS").DefaultCellStyle.Format = "0.00"
                .Columns("PVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("POINTS").HeaderText = "POINTS"
                .Columns("PVALUE").HeaderText = "VALUE"
                FormatGridColumns(gridView, False, False, True, False)
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            End With
        Else
            strSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGETRAN"
            strSql += vbCrLf + " @SYSTEMID = '" & Sysid & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & Fromdate & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@PREVILEGEID = '" & TXTPREVILEGEID.Text & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@WithChitPoints = '" & IIf(chkchit.Checked = True, "Y", "N") & "'"
            If PRETYPE Then
                strSql += vbCrLf + " ,@ONLYPREVILEGE = '" & IIf(chkOnlyPrevilege.Checked = True, "Y", "N") & "'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET PCS = NULL WHERE PCS = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET GRSWT = NULL WHERE GRSWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET NETWT = NULL WHERE NETWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET AMOUNT = NULL WHERE AMOUNT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET TAX = NULL WHERE TAX = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET TOTAL = NULL WHERE TOTAL = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET STNWT = NULL WHERE STNWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET DIAWT = NULL WHERE DIAWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET points = NULL WHERE points = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN SET BPOINTVALUES = NULL WHERE BPOINTVALUES = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN ORDER BY SNO"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            Prop_Sets()
            If Not dtGrid.Rows.Count > 1 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            With gridView
                'For cnt As Integer = 0 To .ColumnCount - 1
                '    .Visible = False
                'Next
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                .Columns("METALNAME").Visible = False
                .Columns("PCS").Visible = chkPcs.Checked
                .Columns("GRSWT").Visible = chkGrsWt.Checked
                .Columns("NETWT").Visible = chkNetWt.Checked
                .Columns("POINTS").Visible = chkwithpoints.Checked
                .Columns("BPOINTVALUES").Visible = chkwithpoints.Checked


                .Columns("PARTICULAR").Width = 250
                .Columns("PCS").Width = 60
                .Columns("GRSWT").Width = 70
                .Columns("GRSWT").Width = 80
                .Columns("NETWT").Width = 80
                .Columns("AMOUNT").Width = 100
                .Columns("TAX").Width = 80
                .Columns("TOTAL").Width = 100
                .Columns("STNWT").Width = 75
                .Columns("DIAWT").Width = 75
                .Columns("POINTS").Width = 75
                .Columns("BPOINTVALUES").Width = 75
                .Columns("BPOINTVALUES").HeaderText = "VALUE"
                .Columns("VALUEINWTRAMT").HeaderText = "VALUE TYPE"
                .Columns("SC").Width = 80
                .Columns("SC").HeaderText = "CESS"
                ' .Columns("CUSTOMER").Visible = rbtBillNo.Checked
            End With
            FormatGridColumns(gridView, False, False, True, False)
            gridView.Columns("POINTS").DefaultCellStyle.Format = "0.00"
            gridView.Columns("BPOINTVALUES").DefaultCellStyle.Format = "0.00"
        End If
        gridHeaderview.Visible = False
        FillGridGroupStyle_KeyNoWise(gridView)

        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None


        Dim TITLE As String
        TITLE += lblcustname.Text & "  PREVILEGE TRANSACTION FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += "  AT " & chkCmbCostCentre.Text & ""
        TITLE += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()

    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkPcs.Checked = False And chkGrsWt.Checked = False And chkNetWt.Checked = False Then
            chkGrsWt.Checked = True
        End If
        'If Trim(lblcustname.Text) = "" Or TXTPREVILEGEID.Text = "" Then
        '    MsgBox("Select Valid PrivilegeId...", MsgBoxStyle.Information)
        '    TXTPREVILEGEID.Focus()
        '    Exit Sub
        'End If
        pnlHeading.Visible = False
        If SPECIFICFORMAT = "1" And chkSpecific.Checked Then
            funcPreviledgeSpecific()
        Else
            If chkSummary.Checked = True Then
                funcPreviledgeSummary()
            Else
                PRIVTRAN()
            End If
        End If
        Exit Sub
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        'lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()
    End Sub

    Private Sub frmPrevilegetran_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPrevilegetran_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        If SPECIFICFORMAT = "1" Then
            chkSpecific.Visible = True
        Else
            chkSpecific.Visible = False

        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        SepStnPost = funcGetValues("SEPSTNPOST", "N")
        SepDiaPost = funcGetValues("SEPDIAPOST", "N")
        SepPrePost = funcGetValues("SEPPREPOST", "N")
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        If GetAdmindbSoftValue("PREVILEDGE_DATE") <> "" Then
            dtpFrom.Enabled = False
            Dim StartDate As Date
            StartDate = dtpFrom.Value.ToString("yyyy-MM-dd")
            dtpFrom.Value = IIf(GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate) = "", StartDate, GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate))
        Else
            dtpFrom.Enabled = True
        End If
        chkSummary.Checked = False
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" And SPECIFICFORMAT <> "1" Then
            chkchit.Visible = True
        Else
            chkchit.Visible = False
            chkchit.Checked = False
        End If
        dtpFrom.Focus()
    End Sub

    Function funcPreviledgeSpecific() As Integer
        gridView.DataSource = Nothing
        Dim DtSummery As New DataTable
        strSql = " SELECT P.PREVILEGEID,X.TRANDATE,X.TRANNO,SUM(X.GRSWT) AS GRSWT,P.PNAME,P.MOBILE "
        strSql += vbCrLf + " ,(P.DOORNO+''+P.ADDRESS1+' '+P.ADDRESS2+' '+P.ADDRESS3+' '+P.CITY+' '+P.AREA) AS ADDRESS FROM ( "
        strSql += vbCrLf + " SELECT (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  AS C WHERE C.BATCHNO = S.BATCHNO ) AS PSNO "
        strSql += vbCrLf + " ,TRANNO ,S.TRANDATE,S.GRSWT FROM " & cnStockDb & "..ISSUE AS S "
        strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND S.GRSWT > 0.1 AND ISNULL(CANCEL,'') <> 'Y' "
        strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')  "
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE "
            strSql += vbCrLf + " METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        strSql += vbCrLf + " )AS X , " & cnAdminDb & "..PERSONALINFO AS P"
        strSql += vbCrLf + " WHERE X.PSNO = P.SNO "
        If chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL" Then
            strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ('" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'))"
        End If
        strSql += vbCrLf + " AND PREVILEGEID <> '' "
        If TXTPREVILEGEID.Text <> "" Then strSql += vbCrLf + "AND PREVILEGEID='" & TXTPREVILEGEID.Text & "' "
        strSql += vbCrLf + " GROUP BY PREVILEGEID,X.TRANNO,P.PNAME,P.MOBILE,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3"
        strSql += vbCrLf + " ,P.CITY,P.AREA,X.TRANDATE  ORDER BY SUBSTRING(PREVILEGEID,2,LEN(PREVILEGEID)-1) * 1 "
        strSql += vbCrLf + " "
        DtSummery = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtSummery)

        If Not DtSummery.Rows.Count > 0 Then
            MsgBox("Record Not Found")
            Exit Function
        End If
        gridView.DataSource = Nothing
        gridView.DataSource = DtSummery
        With gridView
            .Columns("PREVILEGEID").Width = 100
            .Columns("GRSWT").Width = 100
            .Columns("PNAME").Width = 200
            .Columns("MOBILE").Width = 100
            .Columns("ADDRESS").Width = 600
            .Columns("TRANNO").Width = 100
            .Columns("TRANDATE").Width = 100
        End With
        FormatGridColumns(gridView, False, True, True, False)
    End Function
    Function funcPreviledgeSummary()
        Dim DtSummery As New DataTable
        strSql = vbCrLf + "IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME= 'TEMPPRIVILEGESUMMERY' AND XTYPE = 'U') > 0 DROP TABLE TEMPPRIVILEGESUMMERY"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = "SELECT TRANDATE,TRANNO, SUM(ISNULL(OPEN_POINTS,0)) OPEN_POINTS, SUM(ISNULL(OPEN_PVALUE,0))OPEN_PVALUE"
        strSql += vbCrLf + ",SUM(ISNULL(SAVINGS_RECPOINTS,0))SAVINGS_RECPOINTS,SUM(ISNULL(SAVINGS_RECPVALUE,0))SAVINGS_RECPVALUE"
        strSql += vbCrLf + ",SUM(ISNULL(SALES_RECPOINTS,0))SALES_RECPOINTS,SUM(ISNULL(SALES_RECPVALUE,0))SALES_RECPVALUE"
        strSql += vbCrLf + ",SUM(ISNULL(ISS_POINTS,0))ISS_POINTS,SUM(ISNULL(ISS_PVALUE,0))ISS_PVALUE "
        strSql += vbCrLf + ",((SUM(ISNULL(OPEN_POINTS,0))+SUM(ISNULL(SAVINGS_RECPOINTS,0))+SUM(ISNULL(SALES_RECPOINTS,0))) - SUM(ISNULL(ISS_POINTS,0))) CLOSE_POINTS"
        strSql += vbCrLf + ",((SUM(ISNULL(OPEN_PVALUE,0))+SUM(ISNULL(SAVINGS_RECPVALUE,0))+SUM(ISNULL(SALES_RECPVALUE,0)))- SUM(ISNULL(ISS_PVALUE,0))) CLOSE_PVALUE,'2' AS RESULT  "
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)SNO"
        strSql += vbCrLf + " INTO TEMPPRIVILEGESUMMERY"
        strSql += vbCrLf + "FROM ("
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "TRANDATE,TRANNO,CONVERT(NUMERIC(15,3),NULL) OPEN_POINTS"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL) OPEN_PVALUE"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' AND ENTRYTYPE = 'C' THEN POINTS END) SAVINGS_RECPOINTS"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' AND ENTRYTYPE = 'C' THEN PVALUE END) SAVINGS_RECPVALUE"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' AND ENTRYTYPE <> 'C' THEN POINTS END) SALES_RECPOINTS "
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' AND ENTRYTYPE <> 'C' THEN PVALUE END) SALES_RECPVALUE "
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='I' AND ENTRYTYPE <> 'C' THEN POINTS END) ISS_POINTS "
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='I' AND ENTRYTYPE <> 'C' THEN PVALUE END) ISS_PVALUE "
        strSql += vbCrLf + "FROM ("
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "ENTRYTYPE,TRANDATE,TRANNO,TRANTYPE"
        strSql += vbCrLf + ", SUM(POINTS) POINTS"
        strSql += vbCrLf + ",SUM(PVALUE) PVALUE"
        strSql += vbCrLf + "FROM " & cnAdminDb & "..PRIVILEGETRAN "
        strSql += vbCrLf + "WHERE (TRANDATE BETWEEN  '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "' OR TRANDATE IS NULL)"
        'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & cnCompanyId & "'"
        'strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + "AND ISNULL(PREVILEGEID,'') <> ''"
        strSql += vbCrLf + "GROUP BY TRANTYPE,ENTRYTYPE,TRANDATE,TRANNO )X GROUP BY TRANDATE,TRANNO"
        strSql += vbCrLf + " )X GROUP BY TRANDATE,TRANNO ORDER BY TRANDATE,TRANNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "  UPDATE TEMPPRIVILEGESUMMERY SET OPEN_POINTS = (SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN POINTS ELSE POINTS*-1 END)"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE TRANDATE < '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "'"
        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '')"
        strSql += vbCrLf + "  ,OPEN_PVALUE = (SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN PVALUE ELSE PVALUE*-1 END)  "
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE TRANDATE < '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "'  "
        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> '')"
        strSql += vbCrLf + "  WHERE SNO=1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "  UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_POINTS = ((ISNULL(OPEN_POINTS,0)+ISNULL(SAVINGS_RECPOINTS,0)+ISNULL(SALES_RECPOINTS,0)) - ISNULL(ISS_POINTS,0)) WHERE SNO=1"
        strSql += vbCrLf + "  UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_PVALUE = ((ISNULL(OPEN_PVALUE,0)+ISNULL(SAVINGS_RECPVALUE,0)+ISNULL(SALES_RECPVALUE,0)) - ISNULL(ISS_PVALUE,0)) WHERE SNO=1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + "  INSERT INTO TEMPPRIVILEGESUMMERY"
        strSql += vbCrLf + "  (TRANDATE,TRANNO,OPEN_POINTS,OPEN_PVALUE,SAVINGS_RECPOINTS,SAVINGS_RECPVALUE,SALES_RECPOINTS,SALES_RECPVALUE"
        strSql += vbCrLf + "  ,ISS_POINTS,ISS_PVALUE,CLOSE_POINTS,CLOSE_PVALUE,RESULT)"
        strSql += vbCrLf + "  SELECT TRANDATE,NULL TRANNO,SUM(OPEN_POINTS)OPEN_POINTS,SUM(OPEN_PVALUE)OPEN_PVALUE,SUM(SAVINGS_RECPOINTS)SAVINGS_RECPOINTS"
        strSql += vbCrLf + "  ,SUM(SAVINGS_RECPVALUE)SAVINGS_RECPVALUE,SUM(SALES_RECPOINTS)SALES_RECPOINTS,SUM(SALES_RECPVALUE)SALES_RECPVALUE,SUM(ISS_POINTS)ISS_POINTS"
        strSql += vbCrLf + "  ,SUM(ISS_PVALUE)ISS_PVALUE,SUM(CLOSE_POINTS)CLOSE_POINTS,SUM(CLOSE_PVALUE)CLOSE_PVALUE,3 AS RESULT FROM TEMPPRIVILEGESUMMERY"
        strSql += vbCrLf + "  GROUP BY TRANDATE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  INSERT INTO TEMPPRIVILEGESUMMERY"
        strSql += vbCrLf + "  (TRANDATE,TRANNO,OPEN_POINTS,OPEN_PVALUE,SAVINGS_RECPOINTS,SAVINGS_RECPVALUE,SALES_RECPOINTS,SALES_RECPVALUE"
        strSql += vbCrLf + "  ,ISS_POINTS,ISS_PVALUE,RESULT)"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "DISTINCT TRANDATE,NULL AS TRANNO,NULL OPEN_POINTS"
        strSql += vbCrLf + ",NULL OPEN_PVALUE"
        strSql += vbCrLf + ",NULL SAVINGS_RECPOINTS"
        strSql += vbCrLf + ",NULL SALES_RECPOINTS"
        strSql += vbCrLf + ",NULL SAVINGS_RECPVALUE"
        strSql += vbCrLf + ",NULL SALES_RECPVALUE"
        strSql += vbCrLf + ",NULL ISS_POINTS"
        strSql += vbCrLf + ",NULL ISS_PVALUE,1 AS RESULT"
        strSql += vbCrLf + "FROM TEMPPRIVILEGESUMMERY "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = "  DECLARE @TRANDATE VARCHAR(20)"
        strSql += vbCrLf + "  DECLARE @SNO INT"
        strSql += vbCrLf + "  DECLARE CUR CURSOR FOR "
        strSql += vbCrLf + "  SELECT TRANDATE,SNO FROM TEMPPRIVILEGESUMMERY WHERE SNO<>1 ORDER BY SNO"
        strSql += vbCrLf + "  OPEN CUR"
        strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @TRANDATE,@SNO"
        strSql += vbCrLf + "  	WHILE @@FETCH_STATUS <> -1		"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  		IF @TRANDATE <> ''"
        strSql += vbCrLf + "  		BEGIN"
        strSql += vbCrLf + "  			/*UPDATE OPENING*/			"
        strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY SET OPEN_POINTS = ISNULL((SELECT CLOSE_POINTS FROM TEMPPRIVILEGESUMMERY WHERE SNO=@SNO-1),0) WHERE TRANDATE = @TRANDATE  AND SNO = @SNO AND RESULT=2"
        strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY SET OPEN_PVALUE = ISNULL((SELECT CLOSE_PVALUE FROM TEMPPRIVILEGESUMMERY WHERE SNO=@SNO-1),0) WHERE TRANDATE = @TRANDATE  AND SNO = @SNO AND RESULT=2"
        strSql += vbCrLf + " /* Update() CLOSING*/"
        strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_POINTS = ((ISNULL(OPEN_POINTS,0)+ISNULL(SAVINGS_RECPOINTS,0)+ISNULL(SALES_RECPOINTS,0)) - ISNULL(ISS_POINTS,0)) WHERE TRANDATE = @TRANDATE  AND SNO = @SNO AND RESULT=2"
        strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_PVALUE = ((ISNULL(OPEN_PVALUE,0)+ISNULL(SAVINGS_RECPVALUE,0)+ISNULL(SALES_RECPVALUE,0)) - ISNULL(ISS_PVALUE,0)) WHERE TRANDATE = @TRANDATE  AND SNO = @SNO AND RESULT=2"
        strSql += vbCrLf + "  			FETCH Next FROM CUR INTO @TRANDATE,@SNO"
        strSql += vbCrLf + "  		End"
        strSql += vbCrLf + "  	End"
        strSql += vbCrLf + "  CLOSE CUR"
        strSql += vbCrLf + "  DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "  UPDATE T SET"
        strSql += vbCrLf + "  OPEN_POINTS=(SELECT TOP 1 OPEN_POINTS FROM TEMPPRIVILEGESUMMERY WHERE RESULT=2 AND TRANDATE=T.TRANDATE ORDER BY SNO)"
        strSql += vbCrLf + "  ,OPEN_PVALUE=(SELECT TOP 1 OPEN_PVALUE FROM TEMPPRIVILEGESUMMERY WHERE RESULT=2 AND TRANDATE=T.TRANDATE ORDER BY SNO)"
        strSql += vbCrLf + "  ,CLOSE_POINTS=(SELECT TOP 1 CLOSE_POINTS FROM TEMPPRIVILEGESUMMERY WHERE RESULT=2 AND TRANDATE=T.TRANDATE ORDER BY SNO DESC)"
        strSql += vbCrLf + "  ,CLOSE_PVALUE=(SELECT TOP 1 CLOSE_PVALUE FROM TEMPPRIVILEGESUMMERY WHERE RESULT=2 AND TRANDATE=T.TRANDATE ORDER BY SNO DESC)"
        strSql += vbCrLf + "  FROM TEMPPRIVILEGESUMMERY AS T WHERE RESULT=3"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()





        ''''''''''''UPDATE OPENING AND CLOSING
        'strSql = vbCrLf + "  DECLARE @TRANDATE VARCHAR(20)"
        'strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT TRANDATE FROM TEMPPRIVILEGESUMMERY "
        'strSql += vbCrLf + "  OPEN CUR"
        'strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @TRANDATE"
        'strSql += vbCrLf + "  	WHILE @@FETCH_STATUS <> -1		"
        'strSql += vbCrLf + "  	BEGIN"
        'strSql += vbCrLf + "  		IF @TRANDATE <> ''"
        'strSql += vbCrLf + "  		BEGIN"
        'strSql += vbCrLf + "  			/*UPDATE OPENING*/			"
        'strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY SET OPEN_POINTS = (SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN POINTS ELSE POINTS*-1 END)FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE TRANDATE < @TRANDATE AND ISNULL(CANCEL,'') = '') WHERE TRANDATE = @TRANDATE "
        'strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY SET OPEN_PVALUE = (SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN PVALUE ELSE PVALUE*-1 END)FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE TRANDATE < @TRANDATE AND ISNULL(CANCEL,'') = '') WHERE TRANDATE = @TRANDATE "
        'strSql += vbCrLf + "            /* Update() CLOSING*/"
        'strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_POINTS = ((ISNULL(OPEN_POINTS,0)+ISNULL(SAVINGS_RECPOINTS,0)+ISNULL(SALES_RECPOINTS,0)) - ISNULL(ISS_POINTS,0)) WHERE TRANDATE = @TRANDATE "
        'strSql += vbCrLf + "  			UPDATE TEMPPRIVILEGESUMMERY Set CLOSE_PVALUE = ((ISNULL(OPEN_PVALUE,0)+ISNULL(SAVINGS_RECPVALUE,0)+ISNULL(SALES_RECPVALUE,0)) - ISNULL(ISS_PVALUE,0)) WHERE TRANDATE = @TRANDATE "
        'strSql += vbCrLf + "  			FETCH Next FROM CUR INTO @TRANDATE"
        'strSql += vbCrLf + "  		End"
        'strSql += vbCrLf + "  	End"
        'strSql += vbCrLf + "  CLOSE CUR"
        'strSql += vbCrLf + "  DEALLOCATE CUR"
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPPRIVILEGESUMMERY SET TRANNO=NULL WHERE TRANNO=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET OPEN_POINTS=NULL WHERE OPEN_POINTS=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET OPEN_PVALUE=NULL WHERE OPEN_PVALUE=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET SAVINGS_RECPOINTS=NULL WHERE SAVINGS_RECPOINTS=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET SAVINGS_RECPVALUE=NULL WHERE SAVINGS_RECPVALUE=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET SALES_RECPOINTS=NULL WHERE SALES_RECPOINTS=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET SALES_RECPVALUE=NULL WHERE SALES_RECPVALUE=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET ISS_POINTS=NULL WHERE ISS_POINTS=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET ISS_PVALUE=NULL WHERE ISS_PVALUE=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET CLOSE_POINTS=NULL WHERE CLOSE_POINTS=0"
        strSql += vbCrLf + " UPDATE TEMPPRIVILEGESUMMERY SET CLOSE_PVALUE=NULL WHERE CLOSE_PVALUE=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "SELECT * FROM TEMPPRIVILEGESUMMERY ORDER BY TRANDATE,RESULT,TRANNO"
        DtSummery = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtSummery)


        DtSummery.Columns.Add("KEYNO", GetType(Integer))
        DtSummery.Columns("KEYNO").AutoIncrement = True
        DtSummery.Columns("KEYNO").AutoIncrementSeed = 0
        DtSummery.Columns("KEYNO").AutoIncrementStep = 1
        DtSummery.Columns("KEYNO").SetOrdinal(DtSummery.Columns.Count - 1)
        gridView.DataSource = DtSummery
        If Not DtSummery.Rows.Count > 0 Then
            MsgBox("Record Not Found")
            Exit Function
        End If
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("OPEN_POINTS").Width = 100
            .Columns("OPEN_PVALUE").Width = 100
            .Columns("SAVINGS_RECPOINTS").Width = 100
            .Columns("SAVINGS_RECPVALUE").Width = 100
            .Columns("SALES_RECPOINTS").Width = 100
            .Columns("SALES_RECPVALUE").Width = 100
            .Columns("ISS_POINTS").Width = 100
            .Columns("ISS_PVALUE").Width = 100
            .Columns("CLOSE_POINTS").Width = 100
            .Columns("CLOSE_PVALUE").Width = 100

            For i As Integer = 0 To .ColumnCount - 1
                If gridView.Columns(i).Name.Contains("POINTS") Then
                    gridView.Columns(i).HeaderText = "POINTS"
                ElseIf gridView.Columns(i).Name.Contains("PVALUE") Then
                    gridView.Columns(i).HeaderText = "VALUE"
                End If
            Next
        End With
        FormatGridColumns(gridView, False, False, True, False)
        funcStyleSummery()
    End Function
    Function funcStyleSummery()
        Dim GridHeader As New DataTable
        GridHeader.Columns.Add("TRANDATE~TRANNO")
        GridHeader.Columns.Add("OPEN_POINTS~OPEN_PVALUE")
        GridHeader.Columns.Add("SAVINGS_RECPOINTS~SAVINGS_RECPVALUE")
        GridHeader.Columns.Add("SALES_RECPOINTS~SALES_RECPVALUE")
        GridHeader.Columns.Add("ISS_POINTS~ISS_PVALUE")
        GridHeader.Columns.Add("CLOSE_POINTS~CLOSE_PVALUE")
        gridHeaderview.DataSource = Nothing
        gridHeaderview.DataSource = GridHeader

        With gridView
            gridHeaderview.Columns("TRANDATE~TRANNO").Width = .Columns("TRANDATE").Width + .Columns("TRANNO").Width
            gridHeaderview.Columns("OPEN_POINTS~OPEN_PVALUE").Width = .Columns("OPEN_POINTS").Width + .Columns("OPEN_PVALUE").Width
            gridHeaderview.Columns("SAVINGS_RECPOINTS~SAVINGS_RECPVALUE").Width = .Columns("SAVINGS_RECPOINTS").Width + .Columns("SAVINGS_RECPVALUE").Width
            gridHeaderview.Columns("SALES_RECPOINTS~SALES_RECPVALUE").Width = .Columns("SALES_RECPOINTS").Width + .Columns("SALES_RECPVALUE").Width
            gridHeaderview.Columns("ISS_POINTS~ISS_PVALUE").Width = .Columns("ISS_POINTS").Width + .Columns("ISS_PVALUE").Width
            gridHeaderview.Columns("CLOSE_POINTS~CLOSE_PVALUE").Width = .Columns("CLOSE_POINTS").Width + .Columns("CLOSE_PVALUE").Width
        End With
        'ro.Item("OPEN_POINTS~OPEN_PVALUE")
        gridHeaderview.Columns("TRANDATE~TRANNO").HeaderText = ""
        gridHeaderview.Columns("OPEN_POINTS~OPEN_PVALUE").HeaderText = "OPENING"
        gridHeaderview.Columns("SAVINGS_RECPOINTS~SAVINGS_RECPVALUE").HeaderText = "SAVINGS RECEIPT"
        gridHeaderview.Columns("SALES_RECPOINTS~SALES_RECPVALUE").HeaderText = "SALES RECEIPT"
        gridHeaderview.Columns("ISS_POINTS~ISS_PVALUE").HeaderText = "REDEEM"
        gridHeaderview.Columns("CLOSE_POINTS~CLOSE_PVALUE").HeaderText = "CLOSING"

        For Each dgvView As DataGridViewRow In gridView.Rows
            With dgvView
                Select Case .Cells("RESULT").Value.ToString
                    Case "1"
                        .Cells("TRANDATE").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("TRANDATE").Style.Font = reportHeadStyle.Font
                    Case "3"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                End Select
            End With
        Next
    End Function
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''                .Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView.Rows
            With dgvView
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            If IsDBNull(gridView.CurrentRow.Cells("PREVILEGEID").Value.ToString) _
            Or IsDBNull(gridView.CurrentRow.Cells("TRANDATE").Value.ToString) _
            Or gridView.CurrentRow.Cells("PREVILEGEID").Value.ToString = "" _
            Or gridView.CurrentRow.Cells("TRANDATE").Value.ToString = "" _
            Or gridView.CurrentRow.Cells("TRANTYPE").Value.ToString <> "A" _
            Then Exit Sub
            Detail_View(gridView.CurrentRow.Cells("PREVILEGEID").Value.ToString,
            Convert.ToDateTime(gridView.CurrentRow.Cells("TRANDATE").Value).ToString("yyyy-MM-dd"),
            gridView.CurrentRow.Cells("PARTICULAR").Value.ToString)
        End If
    End Sub
    Private Sub Detail_View(ByVal P_Id_ As String, ByVal TranDate_ As String, ByVal TranNo_ As String)
        strSql = ""
        strSql += vbCrLf + "Select TOP 1 DBNAME FROM " & cnAdminDb & "..DBMASTER "
        strSql += vbCrLf + "WHERE '" & TranDate_ & "' BETWEEN STARTDATE AND ENDDATE "
        Dim Dt_ As DataTable = New DataTable
        Dim Da_ As OleDbDataAdapter = New OleDbDataAdapter(strSql, cn)
        Da_.Fill(Dt_)

        strSql = ""
        strSql += vbCrLf + "SELECT TRANNO"
        strSql += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEM"
        strSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)SUBITEM"
        strSql += vbCrLf + ",(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID)ITEMTYPE"
        strSql += vbCrLf + ",(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATEGORY"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)PARTYNAME"
        strSql += vbCrLf + ",PCS,CASE WHEN GRSNET = 'N'THEN NETWT ELSE GRSWT END WEIGHT"
        strSql += vbCrLf + ",AMOUNT,TAX,AMOUNT+TAX TOTAL	FROM " & Dt_.Rows(0)(0).ToString & "..ISSUE I WHERE BATCHNO IN ("
        strSql += vbCrLf + "SELECT BATCHNO FROM " & cnAdminDb & "..PRIVILEGETRAN "
        strSql += vbCrLf + "WHERE TRANDATE = '" & TranDate_ & "' AND PREVILEGEID = '" & P_Id_ & "' AND TRANNO = '" & TranNo_ & "')"
        Dim Dt_Det As DataTable = New DataTable
        Dim Da_Det As OleDbDataAdapter = New OleDbDataAdapter(strSql, cn)
        Da_Det.Fill(Dt_Det)

        If Not Dt_Det.Rows.Count > 0 Then Exit Sub
        Dim obj_Det As frmGridDispDia = New frmGridDispDia
        obj_Det.BaseName = "PRIVILEGE DETALIE"
        obj_Det.Text = "PRIVILEGE DETALIE"
        obj_Det.lblTitle.Text = "PRIVILEGE DETALIE " & TranDate_ & " PRIVILEGE ID : " & P_Id_ & " TRANNO : " & TranNo_
        obj_Det.Name = Me.Name
        obj_Det.WindowState = FormWindowState.Normal
        obj_Det.gridView.RowTemplate.Height = 18
        obj_Det.Height = 400
        obj_Det.gridView.DataSource = Dt_Det
        FormatGridColumns(obj_Det.gridView, False, False, True, False)
        obj_Det.Show()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPrevilegetran_Properties
        obj.p_chkPcs = chkPcs.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkwithpoint = chkwithpoints.Checked
        obj.p_chkChit = chkchit.Checked
        obj.p_cmbMetal = cmbMetal.Text
        SetSettingsObj(obj, Me.Name, GetType(frmPrevilegetran_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPrevilegetran_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPrevilegetran_Properties))
        chkPcs.Checked = obj.p_chkPcs
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkwithpoints.Checked = obj.p_chkwithpoint
        chkchit.Checked = obj.p_chkChit
        cmbMetal.Text = obj.p_cmbMetal

    End Sub

    Private Sub TXTPREVILEGEID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TXTPREVILEGEID.KeyDown
        If e.KeyCode = Keys.Insert Then
            custname()
        End If
    End Sub

    Private Sub TXTPREVILEGEID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXTPREVILEGEID.KeyPress
        If e.KeyChar = Chr(Keys.Insert) Then
            If Trim(TXTPREVILEGEID.Text) <> "" Then
                lblcustname.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & TXTPREVILEGEID.Text & "'")
            End If
        End If
    End Sub

    Private Sub custname()
        Dim STRSQL As String
        STRSQL = " SELECT LTRIM(PREVILEGEID)PREVILEGEID,ACCODE ,ACNAME,MOBILE FROM " & cnAdminDb & "..ACHEAD "
        STRSQL += " WHERE  PREVILEGEID <>'' ORDER BY PREVILEGEID,ACCODE,ACNAME"
        Dim priid As String = BrighttechPack.SearchDialog.Show("Search Previledge Customer ", STRSQL, cn, 2)
        If Trim(priid) <> "" Then
            TXTPREVILEGEID.Text = priid
            lblcustname.Text = (objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & priid & "'", , ) & "'")
            TXTPREVILEGEID.SelectAll()
        End If
    End Sub

    Private Sub TXTPREVILEGEID_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TXTPREVILEGEID.Leave
        If Trim(TXTPREVILEGEID.Text) <> "" Then
            lblcustname.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & Trim(TXTPREVILEGEID.Text) & "'", , )
        End If
    End Sub

    Private Sub chkSummary_CheckedChanged(sender As Object, e As EventArgs) Handles chkSummary.CheckedChanged
        If chkSummary.Checked = True Then
            GrpDetailed.Enabled = False
            chkOnlyPrevilege.Enabled = False
            chkOnlyPrevilege.Checked = False
        Else
            GrpDetailed.Enabled = True
            chkOnlyPrevilege.Enabled = True
            chkOnlyPrevilege.Checked = False
        End If
    End Sub

    Private Sub chkSpecific_CheckedChanged(sender As Object, e As EventArgs) Handles chkSpecific.CheckedChanged
        If chkSpecific.Checked Then
            chkSummary.Visible = False
            chkSummary.Checked = False
            chkOnlyPrevilege.Visible = False
            chkOnlyPrevilege.Checked = False
            chkchitOnly.Visible = False
            chkchitOnly.Checked = False
            chkchit.Visible = False
            chkchit.Checked = False
            chkchitOnly.Visible = False
            chkchitOnly.Checked = False
            chkPcs.Visible = False
            chkPcs.Checked = False
            chkwithpoints.Visible = False
            chkwithpoints.Checked = False
            chkGrsWt.Visible = False
            chkGrsWt.Checked = False
            chkNetWt.Visible = False
            chkNetWt.Checked = False
        Else
            chkSummary.Visible = True
            chkOnlyPrevilege.Visible = True
            chkchitOnly.Visible = True
            chkchit.Visible = True
            chkchitOnly.Visible = True
            chkPcs.Visible = True
            chkwithpoints.Visible = True
            chkGrsWt.Visible = True
            chkNetWt.Visible = True
        End If

    End Sub
End Class

Public Class frmPrevilegetran_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkwithpoint As Boolean = True
    Public Property p_chkwithpoint() As Boolean
        Get
            Return chkwithpoint
        End Get
        Set(ByVal value As Boolean)
            chkwithpoint = value
        End Set
    End Property
    Private chkChit As Boolean = True
    Public Property p_chkChit() As Boolean
        Get
            Return chkChit
        End Get
        Set(ByVal value As Boolean)
            chkChit = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
End Class