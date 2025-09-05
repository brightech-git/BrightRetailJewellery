Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesBillnowise
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
    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim SpecificPrint As Boolean = False

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
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
        If chkWithSR.Checked = True Then
            ''--GENERATE SEP A/C
            Qry += " UNION ALL"
            Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME"
            Qry += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID   "
            Qry += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH   "
            Qry += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
            Qry += " ,TRANDATE,TRANNO,TRANTYPE  "
            Qry += " ,SUM(-1*PCS)PCS,SUM(-1*GRSWT)GRSWT,SUM(-1*NETWT)NETWT,SUM(-1*AMOUNT)AMOUNT  ,SUM(-1*VAT)VAT "
            Qry += " ,SUM(-1*AMOUNT)+SUM(-1*VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'2'SALETYPE  "
            Qry += " FROM   "
            Qry += " ("
            Qry += " SELECT X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO "
            Qry += " ,(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO = ISSSNO AND COMPANYID = '" & strCompanyId & "')TRANDATE"
            Qry += " ,SUM(X.AMOUNT)AMOUNT "
            Qry += " ,ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV' AND TRANNO = X.TRANNO  AND COMPANYID = '" & strCompanyId & "' AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT   "
            Qry += " ,SUM(X.PCS)PCS ,SUM(X.WT)AS GRSWT ,SUM(X.WT)AS NETWT ,SUM(X.STNWT)STNWT "
            Qry += " ,SUM(X.DIAWT)DIAWT ,X.TRANTYPE ,X.BATCHNO,'1'RESULT   "
            Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS CUSTOMER   "
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
            Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND TRANTYPE = 'SR' AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
            Qry += strFtr2
            Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
            Qry += " )AS X"
            Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,X.BATCHNO,X.TRANTYPE,X.PCS"
            Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO "
            ''--DEDUCT SEP A/C AMT
            Qry += " UNION ALL"
            Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
            Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
            Qry += " ,SUM(VAT)VAT"
            Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER,ADDRESS,PHONENO ,'1'RESULT ,'2'SALETYPE "
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
            Qry += " ,ISNULL((SELECT -1*AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV'  AND COMPANYID = '" & strCompanyId & "' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT  "
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
            Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND TRANTYPE = 'SR' AND COMPANYID = '" & strCompanyId & "' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
            Qry += strFtr2
            Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
            Qry += " )AS X," & cnStockDb & "..RECEIPT AS I"
            Qry += strFtr
            Qry += " AND X.ISSSNO = I.SNO"
            Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,I.BATCHNO,X.BATCHNO,X.CATCODE,X.TRANNO"
            Qry += " )AS SEPDIA"
            Qry += " GROUP BY"
            Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        End If
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
        If chkWithSR.Checked = True Then
            ''****************WITH SALES RETURN***************
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,(SUM(AMOUNT)-SUM(STNAMT)) AMOUNT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = Y.BATCHNO AND PAYMODE = 'SV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = Y.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = Y.CATCODE)),0)AS VAT"
            strSql += vbCrLf + " ,SUM(AMOUNT) AS TOTAL"
            strSql += vbCrLf + " ,SUM(STNWT)STNWT"
            strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
            strSql += vbCrLf + " ,'1'RESULT"
            strSql += vbCrLf + " ,'2'SALETYPE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CATCODE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
            strSql += vbCrLf + " ,CONVERT(INT,CONVERT(VARCHAR,DATEPART(YEAR,TRANDATE))+REPLICATE('0',2-LEN((DATEPART(MONTH,TRANDATE)))) + CONVERT(VARCHAR,DATEPART(MONTH,TRANDATE)))AS MMONTHID"
            'strsql += vbcrlf + " ,DATEPART(MONTH,TRANDATE)AS MMONTHID"
            strSql += vbCrLf + " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
            strSql += vbCrLf + " ,TRANDATE"
            strSql += vbCrLf + " ,TRANNO"
            'strsql += vbcrlf + " ,convert(varchar,TRANNO)TRANNO"
            strSql += vbCrLf + " ,-1*SUM(PCS) PCS,-1*SUM(GRSWT)GRSWT,-1*SUM(NETWT)NETWT,-1*SUM(AMOUNT)AMOUNT"
            strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE = 'PV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = I.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)),0)AS VAT"
            strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNAMT"
            strSql += vbCrLf + " ,ISNULL((SELECT -1*SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)DIAWT"
            strSql += vbCrLf + " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
            strSql += vbCrLf + "  ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
            strSql += vbCrLf + "  ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
            strSql += vbCrLf + " ,TRANTYPE,BATCHNO"
            strSql += vbCrLf + " ,'1'RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
            strSql += strFtr
            strSql += vbCrLf + " AND TRANTYPE = 'SR'"
            strSql += vbCrLf + " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,TRANDATE"
            strSql += vbCrLf + " )AS Y"
            strSql += vbCrLf + " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER,ADDRESS,PHONENO"
        End If
        strSql += funcSepStnAc()
        strSql += vbCrLf + " ORDER BY TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 600
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABS')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABS"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,SUM(VAT)VAT"
        strSql += vbCrLf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
        strSql += vbCrLf + " ,'1'RESULT"
        strSql += vbCrLf + " ,SALETYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ABS"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " TEMP" & systemId & "ABSTEMP"
        'strsql += vbcrlf + " AS Z"
        strSql += vbCrLf + " GROUP BY SALETYPE,CATCODE,CATNAME,TRANNO,CUSTOMER,ADDRESS,PHONENO,TRANTYPE,UUPDATED,TRANDATE"
        strSql += vbCrLf + " ORDER BY CONVERT(INT,TRANNO)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcAbsSub() As Integer
        ''SUMMARY WISE SUBTOTAL
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSSUB')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ABSSUB"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
        strSql += vbCrLf + " ,' 'MMONTHID"
        strSql += vbCrLf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
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
        strSql += vbCrLf + " GROUP BY CATCODE"
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
            .Columns("UUPDATED").Visible = True
            .Columns("UUPDATED").HeaderText = "TRANDATE"
            If chkWithSR.Checked = True Then
                .Columns("TRANTYPE").Visible = True
                .Columns("TRANTYPE").HeaderText = "TYPE"
                .Columns("TRANTYPE").Width = 40
            Else
                .Columns("TRANTYPE").Visible = False
            End If
            With .Columns("PARTICULAR")
                .Width = 250
                .HeaderText = "BILL NO"
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
                If ChkStnWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("DIAWT")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                If ChkDIAwt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("CUSTOMER")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
            End With
            With .Columns("ADDRESS")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("PHONENO")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
        End With

        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function



    Private Sub SalesAbs()
        Try

            Dim selCatCode As String = Nothing
            If cmbCategory.Text = "ALL" Then
                selCatCode = "ALL"
            ElseIf cmbMetal.Text <> "" Then
                Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(cmbCategory.Text) & ")"
                Dim dtCat As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCat)
                If dtCat.Rows.Count > 0 Then
                    'selCatCode = "'"
                    For i As Integer = 0 To dtCat.Rows.Count - 1
                        selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
                        'selCatCode += dtItem.Rows(i).Item("ITEMID").ToString
                    Next
                    If selCatCode <> "" Then
                        selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                    End If
                    'selCatCode += "'"
                End If
            End If

            Prop_Sets()
            gridView.DataSource = Nothing
            Me.Refresh()
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPABSSSUMMARY_SA') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPABSSSUMMARY_SA          "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            '28-09-2012 CHANGE ON ALL SP ,SALES RETURN WITH STONE DETAIL , SELECT PCS,GRSWT,AMOUNT FROM ISSSTONE TABLE

            strSql = " EXEC " & cnAdminDb & "..SP_RPT_ABSTRACT_SALES"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TRANNOWISE = 'Y'"
            strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
            strSql += vbCrLf + " ,@WITHSR = '" & IIf(chkWithSR.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@RPTTYPE = 'SA'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
            strSql += vbCrLf + " ,@CATNAME = '" & cmbCategory.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@VA = '" & IIf(chkVA.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@CHITDBID = '" & cnChitCompanyid & "'"
            strSql += vbCrLf + " ,@PURTYPE = ''"
            strSql += vbCrLf + " ,@WITSTN = '" & IIf(chkwithstustone.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@PUREMC = '" & IIf(ChkSepPureMc.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@CUSTOMER = '" & CmbCustomer.Text & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET PCS = NULL WHERE PCS = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET GRSWT = NULL WHERE GRSWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET NETWT = NULL WHERE NETWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMOUNT = NULL WHERE AMOUNT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TAX = NULL WHERE TAX = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TOTAL = NULL WHERE TOTAL = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET STNWT = NULL WHERE STNWT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET DIAWT = NULL WHERE DIAWT = 0"

            If chkVA.Checked Then
                strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMT = NULL WHERE AMT = 0"
                strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET CREDIT = NULL WHERE CREDIT = 0"
                ''strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TRANNO = CONVERT(DECIMAL(10,0),TRANNO) WHERE TRANNO <> 0"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA ORDER BY SNO"

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            If dtGrid.Columns.Contains("BATCHNO") = True Then dtGrid.Columns.Remove("BATCHNO")
            If dtGrid.Columns.Contains("NSNO1") = True Then dtGrid.Columns.Remove("NSNO1")
            If dtGrid.Columns.Contains("NSNO") = True Then dtGrid.Columns.Remove("NSNO")
            If dtGrid.Columns.Contains("TRANTYPE") = True Then dtGrid.Columns.Remove("TRANTYPE")
            If chkDiscount.Checked = False Then
                If dtGrid.Columns.Contains("DISCOUNT") = True Then dtGrid.Columns.Remove("DISCOUNT")
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
                .Columns("MMONTHID").Visible = False
                .Columns("MMONTHNAME").Visible = False
                .Columns("TRANDATE").Visible = False
                .Columns("TRANNO").Visible = True
                .Columns("METALNAME").Visible = False
                .Columns("CATNAME").Visible = False
                .Columns("CUSTOMER").Visible = True
                .Columns("ADDRESS").Visible = False
                .Columns("PHONENO").Visible = False
                .Columns("PCS").Visible = chkPcs.Checked
                .Columns("GRSWT").Visible = chkGrsWt.Checked
                .Columns("NETWT").Visible = chkNetWt.Checked
                .Columns("STNWT").Visible = ChkStnWt.Checked
                .Columns("DIAWT").Visible = ChkDIAwt.Checked
                If .Columns.Contains("ECS") Then .Columns("ECS").Visible = False
                .Columns("PARTICULAR").Visible = Chkparticular.Checked
                .Columns("BILLNO").Visible = False
                .Columns("BILLDATE").Visible = True
                .Columns("ADDRESS1").Visible = False
                .Columns("ADDRESS2").Visible = False
                .Columns("ADDRESS3").Visible = False
                .Columns("AREA").Visible = False
                .Columns("CITY").Visible = False
                .Columns("PARTICULAR").Width = 250
                .Columns("TRANNO").Width = 70
                .Columns("TRANNO").HeaderText = "BILLNO"
                .Columns("PCS").Width = 60
                .Columns("GRSWT").Width = 70
                .Columns("GRSWT").Width = 80
                .Columns("NETWT").Width = 80
                .Columns("CUSTOMER").Width = 120
                .Columns("CUSTOMER").Visible = False
                .Columns("ADDRESS").Width = 150
                .Columns("PHONENO").Width = 100
                If .Columns.Contains("AMT") = True Then .Columns("AMT").Width = 100
                .Columns("TAX").Width = 80
                .Columns("TOTAL").Width = 100
                .Columns("STNWT").Width = 75
                .Columns("DIAWT").Width = 75
                .Columns("SC").Width = 80
                .Columns("SC").HeaderText = "CESS"
                .Columns("SC").Visible = False
                .Columns("EMPNAME").HeaderText = "EMPLOYEE"
                If chkMC.Checked Then
                    .Columns("MCHARGE").Visible = True
                Else
                    .Columns("MCHARGE").Visible = False
                End If

            End With
            FormatGridColumns(gridView, False, False, True, False)
            FillGridGroupStyle_KeyNoWise(gridView)



            Dim TITLE As String
            TITLE = "BILLNO WISE"
            TITLE += " SALES ABSTRACT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            If chkWithSR.Checked = True Then
                TITLE += " WITH SALES RETURN"
            End If
            TITLE += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox("Predefined conditions/dbs are un matched") : Exit Sub
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkPcs.Checked = False And chkGrsWt.Checked = False And chkNetWt.Checked = False Then
            chkGrsWt.Checked = True
        End If
        pnlHeading.Visible = False
        SalesAbs()
        If SpecificPrint = True Then
            btn_dPrint.Visible = True
        Else
            btn_dPrint.Visible = False
        End If

        Exit Sub
    End Sub

    Private Sub frmSalesAbstract_Format2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Format2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        'da = New OleDbDataAdapter(strSql, cn)
        'Dim dt As New DataTable
        'dt.Clear()
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    cmbCostCentre.Items.Clear()
        '    cmbCostCentre.Items.Add("ALL")
        '    strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        '    objGPack.FillCombo(strSql, cmbCostCentre, False)
        '    cmbCostCentre.Text = "ALL"
        'Else
        '    cmbCostCentre.Enabled = False
        'End If

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
        strSql = " SELECT DISTINCT NAME,RESULT FROM ("
        strSql += " SELECT 'ALL' NAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DISTINCT ACNAME AS NAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('C','D','G')"
        strSql += " ) X ORDER BY RESULT,NAME"
        objGPack.FillCombo(strSql, CmbCustomer, False, True)
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
        CmbCustomer.Text = "ALL"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'rbtSummary.Checked = True
        'cmbCostCentre.Text = "ALL"
        'cmbMetal.Text = "ALL"
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        'chkWithSR.Checked = False
        'chkPcs.Checked = True
        'chkGrsWt.Checked = True
        'chkNetWt.Checked = True
        btnView_Search.Enabled = True
        Prop_Sets()
        dtpFrom.Focus()
        dtpFrom.Select()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

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
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        If cmbMetal.Text <> "" Then
            strSql = vbCrLf + " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CATNAME,CONVERT(VARCHAR,CATCODE),2 RESULT FROM " & cnAdminDb & "..CATEGORY"
            strSql += vbCrLf + " ORDER BY RESULT,CATNAME"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtCat As New DataTable()
            da.Fill(dtCat)
            cmbCategory.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(cmbCategory, dtCat, "CATNAME", , "ALL")
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSalesAbstract_Format2_Properties
        obj.p_chkPcs = chkPcs.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkWithSR = chkWithSR.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_chkVA = chkVA.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSalesAbstract_Format2_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesAbstract_Format2_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesAbstract_Format2_Properties))
        chkPcs.Checked = obj.p_chkPcs
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkWithSR.Checked = obj.p_chkWithSR
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbCategory.Text = obj.p_cmbCategory
        chkVA.Checked = obj.p_chkVA
    End Sub

    Private Sub Chkmore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chkmore.CheckedChanged
        If Chkmore.Checked = True Then
            GBmore.Visible = True
        Else
            GBmore.Visible = False
        End If
    End Sub


    Private Sub btn_dPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_dPrint.Click
        If gridView.Rows.Count > 0 Then
            DetailPrint()
        End If
    End Sub

    Function DetailPrint()
        Dim CompanyName, Address1, Address2, Address3, Phone As String
        Dim dtprint As New DataTable
        Dim i As Integer
        Dim dt As New DataTable
        Dim mremark As String
        Dim mode As String
        Dim dateflag As Boolean = False

        dtprint.Clear()
        dtprint = gridView.DataSource

        strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        CompanyName = dt.Rows(0).Item("COMPANYNAME").ToString
        Address1 = dt.Rows(0).Item("ADDRESS1").ToString
        Address2 = dt.Rows(0).Item("ADDRESS2").ToString
        Address3 = dt.Rows(0).Item("ADDRESS3").ToString


        FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
        PgNo = 0
        line = 0
        'strprint = Chr(27) + "M"
        'FileWrite.WriteLine(strprint)
        strprint = Chr(15)
        FileWrite.WriteLine(strprint)
        Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(17) : Dim str9 As String = Space(12)
        Dim str10 As String = Space(6) : Dim str11 As String = Space(12)

        If dtprint.Rows.Count > 0 Then
            Printheader(CompanyName, Address1, Address2, Address3)
            For i = 0 To dtprint.Rows.Count - 1
                mremark = ""
                mode = ""
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
                    str1 = LSet(dtprint.Rows(i).Item("TRANNO").ToString, 8)
                Else
                    str1 = LSet("Total:", 8)
                End If
                If chkPcs.Checked = True Then
                    str1a = LSet(dtprint.Rows(i).Item("PCS").ToString, 5)
                Else
                    str1a = LSet("", 5)
                End If

                If chkGrsWt.Checked = True Then
                    str2 = RSet(dtprint.Rows(i).Item("GRSWT").ToString, 8)
                Else
                    str2 = RSet("", 8)
                End If
                If chkNetWt.Checked = True Then
                    str3 = RSet(dtprint.Rows(i).Item("NETWT").ToString, 8)
                Else
                    str3 = RSet("", 8)
                End If
                str4 = LSet(dtprint.Rows(i).Item("CUSTOMER").ToString, 35)
                If Val(dtprint.Rows(i).Item("AMOUNT").ToString) = 0 Then
                    str5 = RSet(" ", 12)
                Else
                    str5 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("AMOUNT").ToString), "F").ToString).ToString, 12)
                End If
                If Val(dtprint.Rows(i).Item("TAX").ToString) = 0 Then
                    str6 = RSet(" ", 7)
                Else
                    str6 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("TAX").ToString), "F").ToString).ToString, 7)
                End If

                If (Val(RSet(str5.ToString, 12)) + Val(RSet(str6.ToString, 7))) = 0 Then
                    str7 = RSet(" ", 12)
                Else
                    str7 = RSet((Val(str5) + Val(str6)).ToString, 12)
                End If

                str8 = LSet("", 19)
                If dtprint.Columns.Contains("CREDIT") = True Then str9 = RSet(dtprint.Rows(i).Item("CREDIT").ToString, 12) Else str9 = Space(12)
                If dtprint.Columns.Contains("MODE") = True Then mode = dtprint.Rows(i).Item("MODE").ToString
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
                    If mode = "CASH" Then
                        str10 = LSet("CASH", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CHEQUE" Then
                        str10 = LSet("CHEQUE", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "SALES" Then
                        str10 = LSet("SA", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CREDITCARD" Then
                        str10 = LSet("C CARD", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CHIT" Then
                        str10 = LSet("CHIT", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "ADVANCE" Then
                        str10 = LSet("ADV", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "PURCHASE" Then
                        str10 = LSet("PU", 6)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    Else
                        str10 = LSet("", 6)
                        str11 = RSet("", 12)
                    End If
                Else
                    str10 = LSet("", 6)
                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                End If
                'str10 = LSet(dtprint.Rows(i).Item("MODE").ToString, 4)
                'If mode <> "" Then
                '    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                'Else
                '    str11 = RSet("", 12)
                'End If
                If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                    strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If

                If str1 <> LSet("", 8) Then
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "SUB TOTAL" Then
                    strprint = str1 + Space(1) + str1a + Space(1) + str2 + Space(2) + str3 + Space(3) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(2) + str10 + str11 + Space(2) + str9
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If

                If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
                    strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                End If
                If line >= 61 Then
                    strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                    Printheader(CompanyName, Address1, Address2, Address3)
                End If
            Next
        End If
        FileWrite.Close()
        line += 1
        Dim frmPrinterSelect As New frmPrinterSelect
        frmPrinterSelect.Show()
    End Function

    Function Printheader(ByVal CompanyName As String, Optional ByVal Address1 As String = Nothing, Optional ByVal Address2 As String = Nothing, Optional ByVal Address3 As String = Nothing) As Integer
        PgNo += 1
        line = 0
        Dim TITLE As String = Nothing
        Dim category As String = Nothing
        'If cmbMetal.Text <> "ALL" Then

        If cmbCategory.Text <> "ALL" Then
            TITLE = "SALES REGISTER FOR " & cmbCategory.Text & " FROM " & dtpFrom.Value & " TO " & dtpTo.Value
            'category = " - " & cmbCategory.Text
            'End If
        Else
            TITLE = "SALES REGISTER FROM " & dtpFrom.Value & " TO " & dtpTo.Value
        End If
        strprint = Space((140 - Len(CompanyName)) / 2) + CompanyName
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(Address1 & "," & Address2 & "," & Address3))) / 2) + Address1 + Address2 + Address3
        FileWrite.WriteLine(strprint) : line += 1
        'strprint = "               ---------------------------------------------------------------------------"
        'FileWrite.WriteLine(Trim(strprint)) : line += 1
        Dim period As String
        period = ("For the Period  from " & dtpFrom.Value.Date.ToString("dd/MM/yyyy") & " to " & dtpTo.Value.Date.ToString("dd/MM/yyyy"))
        'strprint = Space((140 - Len(lblTitle.Text)) / 2) + lblTitle.Text
        'FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(TITLE))) / 2) + TITLE
        FileWrite.WriteLine(strprint) : line += 1
        strprint = (Space(136) + "Pg #" & PgNo)
        FileWrite.WriteLine(strprint) : line = line + 1
        strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1

        Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(19) : Dim str9 As String = Space(12)
        Dim str10 As String = Space(6) : Dim str11 As String = Space(12)


        str1 = LSet("Bill No.", 8)
        If chkPcs.Checked = True Then
            str1a = RSet("Pcs.", 5)
        Else
            str1a = RSet("", 5)
        End If
        If chkGrsWt.Checked = True Then
            str2 = RSet("Gwt.", 8)
        Else
            str2 = RSet("", 8)
        End If

        If chkNetWt.Checked = True Then
            str3 = RSet("Nwt.", 8)
        Else
            str3 = RSet("", 8)
        End If
        str4 = LSet("Customer Name And Address ", 35)
        str5 = RSet("Amount.", 12)
        str6 = RSet("Vat.", 7)
        str7 = RSet("Bill Amt.", 12)
        str8 = LSet("Receipt Details", 19)
        str9 = RSet("CREDIT ", 12)
        str10 = LSet("Mode", 6)
        str11 = RSet("Amt", 12)
        strprint = str1 + str1a + Space(1) + str2 + Space(2) + str3 + Space(4) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(3) + str8 + Space(2) + str9
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space(112) + str10 + str11
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
    End Function

    Private Sub CmbCustomer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbCustomer.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
            strSql += "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('C')"
            strSql += GetAcNameQryFilteration()
            strSql += "  ORDER BY CRDATE DESC"
            CmbCustomer.Text = BrighttechPack.SearchDialog.Show("Search Party Account Code", strSql, cn, 1, 1, , , , , )
        End If
    End Sub

    Private Sub CmbCustomer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbCustomer.KeyPress
        If e.KeyChar = Chr(Keys.Insert) Then
            strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
            strSql += "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('C')"
            strSql += GetAcNameQryFilteration()
            strSql += "  ORDER BY CRDATE DESC"
            CmbCustomer.Text = BrighttechPack.SearchDialog.Show("Search Party Account Code", strSql, cn, 1, 1, , , , , )
        End If
    End Sub
End Class

Public Class frmSalesAbstract_Format2_Properties
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
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
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
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
End Class