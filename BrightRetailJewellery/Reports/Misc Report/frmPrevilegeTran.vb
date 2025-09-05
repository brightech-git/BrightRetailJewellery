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

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnStockDb & "..SoftControlTran"
        strsql += vbcrlf + " where ctlId = '" & field & "'"
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
        '    Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS CUSTOMER   "
        '    Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
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
        '    Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
        '    Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
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
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO)),'')AS CUSTOMER   "
        Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
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
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
        Qry += " ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        Qry += " ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
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
        strsql += vbcrlf + " DROP TABLE TEMP" & systemId & "ABSTEMP"
        strsql += vbcrlf + " SELECT"
        strsql += vbcrlf + " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,LTRIM(STR(TRANNO))TRANNO,TRANTYPE"
        strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strsql += vbcrlf + " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND PAYMODE = 'SV' AND COMPANYID = '" & strCompanyId & "' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT"
        strsql += vbcrlf + " ,SUM(AMOUNT)AS TOTAL"
        strsql += vbcrlf + " ,SUM(STNWT)STNWT"
        strsql += vbcrlf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO"
        strsql += vbcrlf + " ,'1'RESULT"
        strsql += vbcrlf + " ,'1'SALETYPE"
        strsql += vbcrlf + " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strsql += vbcrlf + " INTO TEMP" & systemId & "ABSTEMP"
        strsql += vbcrlf + " FROM "
        strsql += vbcrlf + " ("
        strsql += vbcrlf + " SELECT"
        strsql += vbcrlf + " CATCODE"
        strsql += vbcrlf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strsql += vbcrlf + " ,ITEMID"
        strsql += vbcrlf + " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        strsql += vbcrlf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        strsql += vbcrlf + " ,CONVERT(INT,CONVERT(VARCHAR,DATEPART(YEAR,TRANDATE))+REPLICATE('0',2-LEN((DATEPART(MONTH,TRANDATE)))) + CONVERT(VARCHAR,DATEPART(MONTH,TRANDATE)))AS MMONTHID"
        strsql += vbcrlf + " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strsql += vbcrlf + " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        strsql += vbcrlf + " ,TRANDATE"
        strsql += vbcrlf + " ,TRANNO"
        strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO)  AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE <> 'T')),0)AMOUNT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO) AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
        strsql += vbcrlf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO IN (I.SNO) AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)DIAWT"
        strSql += vbCrLf + " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        strSql += vbCrLf + "  ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        strSql += vbCrLf + "  ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
        strsql += vbcrlf + " ,TRANTYPE,BATCHNO"
        strsql += vbcrlf + " ,'1'RESULT"
        strsql += vbcrlf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += strFtr
        strsql += vbcrlf + " AND TRANTYPE IN ('SA','OD','RD')"
        strsql += vbcrlf + " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,SNO"
        strsql += vbcrlf + " )AS X"
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
        '    strSql += vbCrLf + " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        '    strSql += vbCrLf + "  ,ISNULL((SELECT ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') + ISNULL(ADDRESS2,'') FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS ADDRESS   "
        '    strSql += vbCrLf + "  ,ISNULL((SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO),'')AS PHONENO   "
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
        strsql += vbcrlf + " ORDER BY TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 600
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABS')>0"
        strsql += vbcrlf + " DROP TABLE TEMP" & systemId & "ABS"
        strsql += vbcrlf + " SELECT "
        'If rbtSummary.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,'   '+CATNAME CATNAME,' 'ITEMID,METALID,METALNAME,' 'MMONTHID,' 'MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtMonth.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,MMONTHID,'   '+MMONTH MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtDate.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,'   '+UUPDATED UUPDATED,TRANDATE,' 'TRANNO,' 'TRANTYPE"
        'ElseIf rbtBillNo.Checked = True Then
        '    strsql += vbcrlf + " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        'End If
        strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strsql += vbcrlf + " ,SUM(VAT)VAT"
        strsql += vbcrlf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strsql += vbcrlf + " ,SUM(STNWT)STNWT"
        strsql += vbcrlf + " ,SUM(DIAWT)DIAWT"
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
        strsql += vbcrlf + " DROP TABLE TEMP" & systemId & "ABSSUB"
        strsql += vbcrlf + " SELECT"
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
        strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strsql += vbcrlf + " ,SUM(VAT)VAT"
        strsql += vbcrlf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strsql += vbcrlf + " ,SUM(STNWT)STNWT"
        strsql += vbcrlf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,' 'CUSTOMER,' ' ADDRESS,' ' PHONENO"
        strsql += vbcrlf + " ,'2'RESULT"
        strsql += vbcrlf + " ,' 'SALETYPE"
        strsql += vbcrlf + " ,CONVERT(VARCHAR(1),'S') COLHEAD"
        strsql += vbcrlf + " INTO TEMP" & systemId & "ABSSUB"
        strsql += vbcrlf + " FROM"
        strsql += vbcrlf + " ("
        strsql += vbcrlf + " SELECT * FROM TEMP" & systemId & "ABS"
        strsql += vbcrlf + " )AS Z"
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
        strsql += vbcrlf + " DROP TABLE TEMP" & systemId & "ABSGRAND"
        strsql += vbcrlf + " SELECT"
        strsql += vbcrlf + " 'ZZZZ'CATCODE,'GRAND TOTAL'CATNAME,' 'ITEMID,'ZZZZ'METALID,'ZZZZ'METALNAME"
        strsql += vbcrlf + " ,' 'MMONTHID"
        strsql += vbcrlf + " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        strsql += vbcrlf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strsql += vbcrlf + " ,SUM(VAT)VAT"
        strsql += vbcrlf + " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strsql += vbcrlf + " ,SUM(STNWT)STNWT"
        strsql += vbcrlf + " ,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + " ,' 'CUSTOMER,' ' ADDRESS,' ' PHONENO"
        strsql += vbcrlf + " ,'3'RESULT"
        strsql += vbcrlf + " ,' 'SALETYPE"
        strsql += vbcrlf + " ,CONVERT(VARCHAR(1),'G') COLHEAD"
        strsql += vbcrlf + " INTO TEMP" & systemId & "ABSGRAND"
        strsql += vbcrlf + " FROM"
        strsql += vbcrlf + " ("
        strsql += vbcrlf + " SELECT * FROM TEMP" & systemId & "ABSSUB"
        strsql += vbcrlf + " )AS Z"
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
    End Function

    Private Sub PRIVTRAN()
        'Prop_Sets()
        gridView.DataSource = Nothing
        Me.Refresh()
        strSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGETRAN"
        strSql += vbCrLf + " @SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@PREVILEGEID = '" & TXTPREVILEGEID.Text & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@WithChitPoints = '" & IIf(chkchit.Checked = True, "Y", "N") & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET PCS = NULL WHERE PCS = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET GRSWT = NULL WHERE GRSWT = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET NETWT = NULL WHERE NETWT = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET AMOUNT = NULL WHERE AMOUNT = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET TAX = NULL WHERE TAX = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET TOTAL = NULL WHERE TOTAL = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET STNWT = NULL WHERE STNWT = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET DIAWT = NULL WHERE DIAWT = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET points = NULL WHERE points = 0"
        strSql += vbCrLf + "UPDATE MASTER..TEMP" & systemId & "PREVILEGETRAN SET BPOINTVALUES = NULL WHERE BPOINTVALUES = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM MASTER..TEMP" & systemId & "PREVILEGETRAN ORDER BY SNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
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
            .Columns("SC").Width = 80
            .Columns("SC").HeaderText = "CESS"


            ' .Columns("CUSTOMER").Visible = rbtBillNo.Checked
        End With
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView)



        Dim TITLE As String
        TITLE += lblcustname.Text & "  PREVILEGE TRANSACTION FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += "  AT " & chkCmbCostCentre.Text & ""
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
        If Trim(lblcustname.Text) = "" Then
            MsgBox("select Valid PrivilegeId ", MsgBoxStyle.Information)
            TXTPREVILEGEID.Focus()
            Exit Sub
        End If
        pnlHeading.Visible = False
        PRIVTRAN()
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
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
            chkchit.Visible = True
        Else
            chkchit.Visible = False
        End If


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
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
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
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Export)
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
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        If cmbMetal.Text <> "" Then
            'cmbCategory.Items.Clear()
            'cmbCategory.Items.Add("ALL")
            'strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "')"
            'objGPack.FillCombo(strSql, cmbCategory, False)
            'cmbCategory.Text = "ALL"
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
        'obj.p_chkWithSR = chkWithSR.Checked
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        'obj.p_cmbCategory = cmbCategory.Text
        'bj.p_rbtSummary = rbtSummary.Checked
        'obj.p_rbtMonth = rbtMonth.Checked
        'obj.p_rbtDate = rbtDate.Checked
        'obj.p_rbtBillNo = rbtBillNo.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPrevilegetran_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPrevilegetran_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPrevilegetran_Properties))
        chkPcs.Checked = obj.p_chkPcs
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        'chkWithSR.Checked = obj.p_chkWithSR
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        'cmbCategory.Text = obj.p_cmbCategory
        'rbtSummary.Checked = obj.p_rbtSummary
        'rbtMonth.Checked = obj.p_rbtMonth
        'rbtDate.Checked = obj.p_rbtDate
        'rbtBillNo.Checked = obj.p_rbtBillNo
    End Sub

    Private Sub TXTPREVILEGEID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXTPREVILEGEID.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Trim(TXTPREVILEGEID.Text) = "" Then
                custname()
            Else
                lblcustname.Text = objGPack.GetSqlValue("SELECT acname FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & TXTPREVILEGEID.Text & "'")
            End If
        End If
    End Sub

    Private Sub TXTPREVILEGEID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXTPREVILEGEID.TextChanged

    End Sub

    Private Sub custname()
        Dim STRSQL As String
        STRSQL = " SELECT PREVILEGEID,ACCODE ,acname FROM " & cnAdminDb & "..ACHEAD "
        STRSQL += " where  PREVILEGEID <>'' GROUP BY PREVILEGEID,ACCODE ,acname "
        Dim priid As String = GiritechPack.SearchDialog.Show("Select CUSTOMER", STRSQL, cn, 2)
        If Trim(priid) <> "" Then
            TXTPREVILEGEID.Text = priid
            lblcustname.Text = (objGPack.GetSqlValue("SELECT acname FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & priid & "'", , ) & "'")
            TXTPREVILEGEID.SelectAll()
        End If
    End Sub

    Private Sub chkwithpoints_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkwithpoints.CheckedChanged

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