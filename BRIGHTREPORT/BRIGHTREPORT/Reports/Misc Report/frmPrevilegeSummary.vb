Imports System.Data.OleDb
Imports System.Threading
Public Class frmPrevilegeSummary
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dt As New DataTable
    Dim SmsVia As String = "Web"
    Dim SmsFlag As Boolean = False
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable
    Dim SMS_PREV_SUM As String = objGPack.GetSqlValue("SELECT TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME = 'SMS_PREV_SUM'")
    Dim PREV_RECORD As Boolean = IIf(GetAdmindbSoftValue("PREV_RECORD", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
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
            End With

            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
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
    Private Sub PRIVILEGETRAN()
        Dim Title As String
        gridView.DataSource = Nothing
        strSql = vbCrLf + "DECLARE  @FROMDATE AS VARCHAR(15)"
        strSql += vbCrLf + "DECLARE  @TODATE AS VARCHAR(15)"
        strSql += vbCrLf + "SET @FROMDATE = '" & dtpFrom_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
        If chkASD.Checked Then
            strSql += vbCrLf + "SET @TODATE = '" & dtpFrom_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "SET @TODATE = '" & dtpTo_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If

        strSql += vbCrLf + "IF (SELECT COUNT(ID) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PRIVILEGETRAN ' AND XTYPE = 'U') > 0 DROP TABLE TEMP" & systemId & "PRIVILEGETRAN"
        strSql += vbCrLf + "IF (SELECT COUNT(ID) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PRIVILEGETRAN1' AND XTYPE = 'U') > 0 DROP TABLE TEMP" & systemId & "PRIVILEGETRAN1"
        strSql += vbCrLf + "IF (SELECT COUNT(ID) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PRIVILEGETRAN2' AND XTYPE = 'U') > 0 DROP TABLE TEMP" & systemId & "PRIVILEGETRAN2"
        strSql += vbCrLf + "Select "
        strSql += vbCrLf + "PREVILEGEID"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) ACNAME "
        strSql += vbCrLf + ",(SELECT MOBILE  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) MOBILE "
        strSql += vbCrLf + ",TRANTYPE,ISNULL(ENTRYTYPE ,'A') ENTRYTYPE"
        strSql += vbCrLf + ",TRANDATE "
        strSql += vbCrLf + ",SUM(ISNULL(POINTS,0)) POINTS"
        strSql += vbCrLf + ",SUM(ISNULL(PVALUE,0)) PVALUE"
        strSql += vbCrLf + ",CASE WHEN ISNULL(TRANDATE,'1900-01-01') < @FROMDATE AND TRANTYPE = 'I'  THEN 'OPEISS'"
        strSql += vbCrLf + "	  WHEN ISNULL(TRANDATE,'1900-01-01') < @FROMDATE AND TRANTYPE = 'R'  THEN 'OPEREC'"
        strSql += vbCrLf + "	  WHEN ISNULL(TRANDATE,'1900-01-01') BETWEEN @FROMDATE AND @TODATE AND TRANTYPE = 'I' THEN 'ISS'"
        strSql += vbCrLf + "	  WHEN ISNULL(TRANDATE,'1900-01-01') BETWEEN @FROMDATE AND @TODATE AND TRANTYPE = 'R' THEN 'REC' END ISSTYPE"
        strSql += vbCrLf + ",(SELECT ACNAME  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) NAME "
        strSql += vbCrLf + ",(SELECT DOORNO  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) DOORNO "
        strSql += vbCrLf + ",(SELECT ADDRESS1  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) ADDRESS1 "
        strSql += vbCrLf + ",(SELECT ADDRESS2  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) ADDRESS2 "
        strSql += vbCrLf + ",(SELECT ADDRESS3  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) ADDRESS3 "
        strSql += vbCrLf + ",(SELECT AREA  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) AREA "
        strSql += vbCrLf + ",(SELECT CITY  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) CITY "
        strSql += vbCrLf + ",(SELECT STATE  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) STATE "
        strSql += vbCrLf + ",(SELECT MOBILE  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) MOBILENO "
        strSql += vbCrLf + ",(SELECT EMAILID  FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = P.PREVILEGEID ) EMAIL "
        strSql += vbCrLf + " INTO TEMP" & systemId & "PRIVILEGETRAN"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILEGETRAN P"
        strSql += vbCrLf + " WHERE ISNULL(TRANDATE,'1900-01-01') <= ''+@TODATE+''"
        strSql += vbCrLf + IIf(txtPrevilegeId.Text <> "", " AND PREVILEGEID = '" & txtPrevilegeId.Text & "'", "")
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + " AND ISNULL(PREVILEGEID,'') <> ''"
        strSql += vbCrLf + " GROUP BY PREVILEGEID,TRANDATE, TRANTYPE ,ENTRYTYPE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT PREVILEGEID,ACNAME,MOBILE"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'OPEREC'  THEN ISNULL(POINTS,0) WHEN ISSTYPE = 'OPEISS' THEN ISNULL(POINTS,0)*-1 END),0)OPE_POINTS "
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'OPEREC'  THEN ISNULL(PVALUE,0) WHEN ISSTYPE = 'OPEISS' THEN ISNULL(PVALUE,0)*-1 END),0)OPE_PVALUE "
        If chkchit.Checked Then
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') = 'C' THEN ISNULL(POINTS,0) END),0)SAVINGS_REC_POINTS"
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') = 'C' THEN ISNULL(PVALUE,0) END),0)SAVINGS_REC_PVALUE "
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') <> 'C' THEN ISNULL(POINTS,0) END),0)SALES_REC_POINTS"
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') <> 'C' THEN ISNULL(PVALUE,0) END),0)SALES_REC_PVALUE "
        Else
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') <> 'C' THEN ISNULL(POINTS,0) END),0)SALES_REC_POINTS"
            strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'REC' AND ISNULL(ENTRYTYPE,'') <> 'C' THEN ISNULL(PVALUE,0) END),0)SALES_REC_PVALUE "
        End If
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'ISS' THEN ISNULL(POINTS,0) END),0)ISS_POINTS"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN ISSTYPE = 'ISS' THEN ISNULL(PVALUE,0) END),0)ISS_PVALUE "
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),0)CLOSE_POINTS"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),0)CLOSE_PVALUE"
        strSql += vbCrLf + ",1 RESULT,CONVERT(VARCHAR(15),'') COLHEAD"
        strSql += vbCrLf + " ,NAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,MOBILENO,EMAIL"
        strSql += vbCrLf + " INTO TEMP" & systemId & "PRIVILEGETRAN1"
        strSql += vbCrLf + " FROM TEMP" & systemId & "PRIVILEGETRAN "
        strSql += vbCrLf + " GROUP BY  PREVILEGEID, ACNAME, MOBILE,NAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,MOBILENO,EMAIL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''''''''''''CLOSING UPDATE

        strSql = vbCrLf + "Update TEMP" & systemId & "PRIVILEGETRAN1  Set CLOSE_POINTS = ISNULL(OPE_POINTS, 0)+(ISNULL(SALES_REC_POINTS,0)+" & IIf(chkchit.Checked = True, "ISNULL(SAVINGS_REC_POINTS,0)", "0") & "-ISNULL(ISS_POINTS,0))"
        strSql += vbCrLf + "Update TEMP" & systemId & "PRIVILEGETRAN1  Set CLOSE_PVALUE = ISNULL(OPE_PVALUE, 0)+(ISNULL(SALES_REC_PVALUE,0)+" & IIf(chkchit.Checked = True, "ISNULL(SAVINGS_REC_PVALUE,0)", "0") & "-ISNULL(ISS_PVALUE,0))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim TempTable As String = Nothing
        If chkfromtorange.Checked And Val(txtFrom_Point_NUM.Text) <> 0 And Val(txtTo_Point_NUM.Text) <> 0 Then
            strSql = "SELECT *  INTO TEMP" & systemId & "PRIVILEGETRAN2 FROM TEMP" & systemId & "PRIVILEGETRAN1"
            strSql += vbCrLf + " WHERE (ISNULL(OPE_POINTS, 0)+(ISNULL(SALES_REC_POINTS,0)+" & IIf(chkchit.Checked = True, "ISNULL(SAVINGS_REC_POINTS,0)", "0") & ")) BETWEEN " & Val(txtFrom_Point_NUM.Text) & " And " & Val(txtTo_Point_NUM.Text) & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            TempTable = "TEMP" & systemId & "PRIVILEGETRAN2"
        ElseIf chkfromtorange.Checked And Val(txtTo_Point_NUM.Text) = 0 Then
            MsgBox("To Range Should Not Be Zero") : Exit Sub
        Else
            TempTable = "TEMP" & systemId & "PRIVILEGETRAN1"
        End If

        strSql = vbCrLf + "IF (SELECT COUNT(*) FROM " & TempTable & " )> 0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO " & TempTable & " "
        strSql += vbCrLf + " (ACNAME, OPE_POINTS, OPE_PVALUE, " & IIf(chkchit.Checked = True, "SAVINGS_REC_POINTS, SAVINGS_REC_PVALUE, SALES_REC_POINTS, SALES_REC_PVALUE,", " SALES_REC_POINTS, SALES_REC_PVALUE,") & " ISS_POINTS, ISS_PVALUE, CLOSE_POINTS, CLOSE_PVALUE,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 'TOTAL',SUM(ISNULL(OPE_POINTS,0)),SUM(ISNULL(OPE_PVALUE,0)),"
        If chkchit.Checked Then
            strSql += vbCrLf + " ISNULL(SUM(SAVINGS_REC_POINTS),0),ISNULL(SUM(SAVINGS_REC_PVALUE),0),ISNULL(SUM(SALES_REC_POINTS),0),ISNULL(SUM(SALES_REC_PVALUE),0)"
        Else
            strSql += vbCrLf + " ISNULL(SUM(SALES_REC_POINTS),0),ISNULL(SUM(SALES_REC_PVALUE),0)"
        End If
        strSql += vbCrLf + " ,ISNULL(SUM(ISS_POINTS),0),ISNULL(SUM(ISS_PVALUE),0),ISNULL(SUM(CLOSE_POINTS),0),ISNULL(SUM(CLOSE_PVALUE),0),2,'S' FROM " & TempTable & " "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN OPE_POINTS NUMERIC(15,2) NULL"
        strSql += vbCrLf + " ALTER TABLE " & TempTable & " ALTER COLUMN OPE_PVALUE NUMERIC(15,2) NULL"
        strSql += vbCrLf + " ALTER TABLE " & TempTable & " ALTER COLUMN SALES_REC_POINTS NUMERIC(15,2) NULL"
        strSql += vbCrLf + " ALTER TABLE " & TempTable & " ALTER COLUMN SALES_REC_PVALUE NUMERIC(15,2) NULL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " UPDATE " & TempTable & " SET OPE_POINTS = NULL WHERE OPE_POINTS = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET OPE_PVALUE = NULL WHERE OPE_PVALUE = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET SALES_REC_POINTS = NULL WHERE SALES_REC_POINTS = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET SALES_REC_PVALUE = NULL WHERE SALES_REC_PVALUE = 0"
        If chkchit.Checked Then
            strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN SAVINGS_REC_POINTS NUMERIC(15,2) NULL"
            strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN SAVINGS_REC_PVALUE NUMERIC(15,2) NULL"
            strSql += vbCrLf + " UPDATE " & TempTable & " SET SAVINGS_REC_POINTS = NULL WHERE SAVINGS_REC_POINTS = 0"
            strSql += vbCrLf + " UPDATE " & TempTable & " SET SAVINGS_REC_PVALUE = NULL WHERE SAVINGS_REC_PVALUE = 0"
        End If
        strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN ISS_POINTS NUMERIC(15,2) NULL"
        strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN ISS_PVALUE NUMERIC(15,2) NULL"
        strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN CLOSE_POINTS NUMERIC(15,2) NULL"
        strSql += vbCrLf + "ALTER TABLE " & TempTable & " ALTER COLUMN CLOSE_PVALUE NUMERIC(15,2) NULL"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET ISS_POINTS = NULL WHERE ISS_POINTS = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET ISS_PVALUE = NULL WHERE ISS_PVALUE = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET CLOSE_POINTS = NULL WHERE CLOSE_POINTS = 0"
        strSql += vbCrLf + " UPDATE " & TempTable & " SET CLOSE_PVALUE = NULL WHERE CLOSE_PVALUE = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT PREVILEGEID,ACNAME,MOBILE"
        If chkReceiptSummary.Checked = False Then
            strSql += vbCrLf + ",OPE_POINTS,OPE_PVALUE"
            If chkchit.Checked Then
                strSql += vbCrLf + " ,SAVINGS_REC_POINTS,SAVINGS_REC_PVALUE"
                strSql += vbCrLf + " ,SALES_REC_POINTS,SALES_REC_PVALUE"
            Else
                strSql += vbCrLf + " ,SALES_REC_POINTS,SALES_REC_PVALUE"
            End If
        Else
            If chkchit.Checked Then
                strSql += vbCrLf + " ,(ISNULL(OPE_POINTS,0)"
                strSql += vbCrLf + " +ISNULL(SAVINGS_REC_POINTS,0)"
                strSql += vbCrLf + " +ISNULL(SALES_REC_POINTS,0))REC_POINTS"
            Else
                strSql += vbCrLf + " ,(ISNULL(OPE_POINTS,0)"
                strSql += vbCrLf + " +ISNULL(SALES_REC_POINTS,0))REC_POINTS"
            End If
            If chkchit.Checked Then
                strSql += vbCrLf + " ,(ISNULL(OPE_PVALUE,0)"
                strSql += vbCrLf + " +ISNULL(SAVINGS_REC_PVALUE,0)"
                strSql += vbCrLf + " +ISNULL(SALES_REC_PVALUE,0))REC_PVALUE"
            Else
                strSql += vbCrLf + " ,(ISNULL(OPE_POINTS,0)"
                strSql += vbCrLf + " +ISNULL(SALES_REC_PVALUE,0))REC_PVALUE"
            End If
        End If
        strSql += vbCrLf + " ,ISS_POINTS,ISS_PVALUE,CLOSE_POINTS,CLOSE_PVALUE"
        If chkWithAddress.Checked Then
            strSql += vbCrLf + " ,NAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,MOBILENO,EMAIL "
        End If
        strSql += vbCrLf + " ,COLHEAD,RESULT"
        strSql += vbCrLf + " FROM " & TempTable & "  ORDER BY RESULT,"
        strSql += vbCrLf + " LEFT(ACNAME, PATINDEX('%[0-9]%',PREVILEGEID)-1), "
        strSql += vbCrLf + " CONVERT(BIGINT, SUBSTRING(PREVILEGEID, PATINDEX('%[0-9]%',PREVILEGEID),LEN(PREVILEGEID)))"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtview As New DataTable
        dtview.Columns.Add("KEYNO", GetType(Integer))
        dtview.Columns("KEYNO").AutoIncrement = True
        dtview.Columns("KEYNO").AutoIncrementSeed = 0
        dtview.Columns("KEYNO").AutoIncrementStep = 1
        da.Fill(dtview)
        Title = "Previledge Summary Report"
        If chkchit.Checked Then Title += " With Scheme"
        If chkASD.Checked Then
            Title += " AsOn Date " & dtpFrom_OWN.Value.ToString("dd-MM-yyyy") & ""
        Else
            Title += " From Date " & dtpFrom_OWN.Value.ToString("dd-MM-yyyy") & " To " & dtpTo_OWN.Value.ToString("dd-MM-yyyy") & ""
        End If
        lblTitle.Text = Title
        If dtview.Rows.Count > 0 Then
            gridView.DataSource = dtview
        Else
            MsgBox("Record Not Found")
            Exit Sub
        End If
        FormatGridColumns(gridView, False, False, True, False)
        If gridView.Rows.Count > 0 Then
            With gridView
                If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
                If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
                If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
                For i As Integer = 0 To .ColumnCount - 1
                    If gridView.Columns(i).Name.Contains("POINTS") Then
                        gridView.Columns(i).HeaderText = "POINTS"
                    ElseIf gridView.Columns(i).Name.Contains("PVALUE") Then
                        gridView.Columns(i).HeaderText = "VALUE"
                    End If
                Next
                For i As Integer = 0 To .ColumnCount - 1
                    If .Columns(i).Name = "ACNAME" Then
                        .Columns(i).Width = 150
                    Else
                        .Columns(i).Width = 100
                    End If
                Next
            End With
        End If

        GridViewFormat()
        funcStyleSummery()
        gridView.Columns(0).Frozen = True
    End Sub
    Function funcStyleSummery()
        Dim GridHeader As New DataTable
        GridHeader.Columns.Add("PREVILEGEID~ACNAME~MOBILE")
        If chkReceiptSummary.Checked Then
            GridHeader.Columns.Add("REC_POINTS~REC_PVALUE")
        Else
            GridHeader.Columns.Add("OPE_POINTS~OPE_PVALUE")
            If chkchit.Checked Then
                GridHeader.Columns.Add("SAVINGS_REC_POINTS~SAVINGS_REC_PVALUE")
            End If
            GridHeader.Columns.Add("SALES_REC_POINTS~SALES_REC_PVALUE")
        End If
        GridHeader.Columns.Add("ISS_POINTS~ISS_PVALUE")
        GridHeader.Columns.Add("CLOSE_POINTS~CLOSE_PVALUE")
        If chkWithAddress.Checked Then
            GridHeader.Columns.Add("NAME~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~MOBILE~EMAIL")
            'GridHeader.Columns.Add("DOORNO")
            'GridHeader.Columns.Add("ADDRESS1")
            'GridHeader.Columns.Add("ADDRESS2")
            'GridHeader.Columns.Add("ADDRESS3")
            'GridHeader.Columns.Add("AREA")
            'GridHeader.Columns.Add("CITY")
            'GridHeader.Columns.Add("STATE")
            'GridHeader.Columns.Add("MOBILENO")
            'GridHeader.Columns.Add("EMAIL")
        End If
        GridHeader.Columns.Add("SCROLL", GetType(String))
        gridViewHead.DataSource = Nothing
        gridViewHead.DataSource = GridHeader

        With gridView
            gridViewHead.Columns("PREVILEGEID~ACNAME~MOBILE").Width = .Columns("PREVILEGEID").Width + .Columns("ACNAME").Width + .Columns("MOBILE").Width
            If chkReceiptSummary.Checked Then
                gridViewHead.Columns("REC_POINTS~REC_PVALUE").Width = .Columns("REC_POINTS").Width + .Columns("REC_PVALUE").Width
            Else
                gridViewHead.Columns("OPE_POINTS~OPE_PVALUE").Width = .Columns("OPE_POINTS").Width + .Columns("OPE_PVALUE").Width
                If chkchit.Checked Then
                    gridViewHead.Columns("SAVINGS_REC_POINTS~SAVINGS_REC_PVALUE").Width = .Columns("SAVINGS_REC_POINTS").Width + .Columns("SAVINGS_REC_PVALUE").Width
                End If
                gridViewHead.Columns("SALES_REC_POINTS~SALES_REC_PVALUE").Width = .Columns("SALES_REC_POINTS").Width + .Columns("SALES_REC_PVALUE").Width
            End If
            gridViewHead.Columns("ISS_POINTS~ISS_PVALUE").Width = .Columns("ISS_POINTS").Width + .Columns("ISS_PVALUE").Width
            gridViewHead.Columns("CLOSE_POINTS~CLOSE_PVALUE").Width = .Columns("CLOSE_POINTS").Width + .Columns("CLOSE_PVALUE").Width
            If chkWithAddress.Checked Then
                gridViewHead.Columns("NAME~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~MOBILE~EMAIL").Width = gridView.Columns("NAME").Width _
                + gridView.Columns("DOORNO").Width _
                + gridView.Columns("ADDRESS1").Width _
                + gridView.Columns("ADDRESS2").Width _
                + gridView.Columns("ADDRESS3").Width _
                + gridView.Columns("AREA").Width _
                + gridView.Columns("CITY").Width _
                + gridView.Columns("STATE").Width _
                + gridView.Columns("MOBILENO").Width _
                + gridView.Columns("EMAIL").Width
            End If
        End With
        'ro.Item("OPEN_POINTS~OPEN_PVALUE")
        gridViewHead.Columns("PREVILEGEID~ACNAME~MOBILE").HeaderText = "PARTICULARS"
        If chkReceiptSummary.Checked Then
            gridViewHead.Columns("REC_POINTS~REC_PVALUE").HeaderText = "RECEIPT"
        Else
            gridViewHead.Columns("OPE_POINTS~OPE_PVALUE").HeaderText = "OPENING"
            If chkchit.Checked Then
                gridViewHead.Columns("SAVINGS_REC_POINTS~SAVINGS_REC_PVALUE").HeaderText = "SAVINGS RECEIPT"
            End If
            gridViewHead.Columns("SALES_REC_POINTS~SALES_REC_PVALUE").HeaderText = "SALES RECEIPT"
        End If

        gridViewHead.Columns("ISS_POINTS~ISS_PVALUE").HeaderText = "REDEEM"
        gridViewHead.Columns("CLOSE_POINTS~CLOSE_PVALUE").HeaderText = "CLOSING"
        If chkWithAddress.Checked Then
            gridViewHead.Columns("NAME~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~MOBILE~EMAIL").HeaderText = "ADDRESS"
            'gridViewHead.Columns("DOORNO").HeaderText = ""
            'gridViewHead.Columns("ADDRESS1").HeaderText = ""
            'gridViewHead.Columns("ADDRESS2").HeaderText = ""
            'gridViewHead.Columns("ADDRESS3").HeaderText = ""
            'gridViewHead.Columns("AREA").HeaderText = ""
            'gridViewHead.Columns("CITY").HeaderText = ""
            'gridViewHead.Columns("STATE").HeaderText = ""
            'gridViewHead.Columns("MOBILENO").HeaderText = ""
            'gridViewHead.Columns("EMAIL").HeaderText = ""
        End If
        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        If colWid >= gridView.Width Then
            gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        Else
            gridViewHead.Columns("SCROLL").Visible = False
        End If
        gridViewHead.Columns("SCROLL").HeaderText = ""
    End Function
    Private Sub PreViledgeDetail(ByVal PreviledgeId As String)
        Dim StartDate As Date
        StartDate = dtpFrom_OWN.Value.ToString("yyyy-MM-dd")
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

            If chkASD.Checked Then
                strSql = "SELECT DISTINCT DBNAME FROM " + cnAdminDb + "..DBMASTER WHERE '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' > ENDDATE "
            Else
                strSql = "SELECT DISTINCT DBNAME FROM " + cnAdminDb + "..DBMASTER WHERE '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "' > STARTDATE  AND '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' < ENDDATE  "
            End If

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
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select TRANNO, TRANDATE, TRANTYPE, PCS, GRSWT, NETWT, LESSWT, PUREWT, AMOUNT, TAX, TAGNO, ITEMID "
                strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID FROM " + _Dt.Rows(i).Item("DBNAME").ToString + "..RECEIPT "
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SR') AND BATCHNO NOT IN ("
                strSql += vbCrLf + "  SELECT DISTINCT BATCHNO FROM " + _Dt.Rows(i).Item("DBNAME").ToString + "..ISSUE "
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
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
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  T.TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  T.TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + "AND T.ENTRYTYPE <> 'C'"
                strSql += vbCrLf + "AND PREVILEGEID='" & PreviledgeId & "' "
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
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  T.TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  T.TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + "AND PREVILEGEID='" & PreviledgeId & "' "
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
            If chkASD.Checked Then
                strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
            Else
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
            End If
            strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  Select TRANNO, TRANDATE, TRANTYPE, PCS, GRSWT, NETWT, LESSWT, PUREWT, AMOUNT, TAX, TAGNO, ITEMID "
            strSql += vbCrLf + "  ,SUBITEMID,BATCHNO ,COSTID , COMPANYID , USERID, EMPID FROM " + cnStockDb + "..RECEIPT "
            If chkASD.Checked Then
                strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
            Else
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SR') AND BATCHNO NOT IN ("
            strSql += vbCrLf + "  SELECT DISTINCT BATCHNO FROM " + cnStockDb + "..ISSUE "
            If chkASD.Checked Then
                strSql += vbCrLf + "  WHERE  TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
            Else
                strSql += vbCrLf + "  WHERE  TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
            End If
            strSql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD'))"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            If chkchit.Checked Then
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
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  T.TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  T.TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + "AND T.ENTRYTYPE = 'C'"
                strSql += vbCrLf + "AND PREVILEGEID='" & PreviledgeId & "' "
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
                If chkASD.Checked Then
                    strSql += vbCrLf + "  WHERE  T.TRANDATE <= '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
                Else
                    strSql += vbCrLf + "  WHERE  T.TRANDATE BETWEEN '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
                End If
                strSql += vbCrLf + "AND PREVILEGEID='" & PreviledgeId & "' "
                strSql += vbCrLf + "AND ISNULL(T.PREVILEGEID,'')<>''"
                strSql += vbCrLf + "AND T.TRANTYPE='I'"
                strSql += vbCrLf + "AND ISNULL(T.CANCEL,'')=''"
                strSql += vbCrLf + "AND T.ENTRYTYPE = 'C'"
                strSql += vbCrLf + ") X ORDER BY TRANTYPE,TRANDATE,PARTICULAR,RESULT"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,TRANTYPE)"
            strSql += vbCrLf + "SELECT TOP 1 'ACCUMULATED',-1,'T','A' FROM TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='A' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            If chkchit.Checked Then
                strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPLOYALTY(PARTICULAR,RESULT,COLHEAD,TRANTYPE)"
                strSql += vbCrLf + "SELECT TOP 1 'SAVINGS',-1,'T','C' FROM TEMPTABLEDB..TEMPLOYALTY WHERE TRANTYPE='C' "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

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
        End If
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
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "PREVILEDGE DETAIL FOR CUSTOMER"
        Dim tit As String = "PREVILEDGE DETAIL FOR CUSTOMER"
        If chkASD.Checked = False Then tit += " FROM " & dtpFrom_OWN.Text & " TO " & dtpTo_OWN.Text Else tit += " AS ON " & dtpFrom_OWN.Text
        If chkchit.Checked = False Then tit += " WITH OUT SAVINGS" Else tit += " WITH SAVINGS"
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.gridView.DataSource = dtGrid
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = True
        objGridShower.formuser = userId
        objGridShower.WindowState = FormWindowState.Normal
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.gridViewHeader.Visible = True
        With objGridShower.gridView
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("POINTS").DefaultCellStyle.Format = "0.00"
            .Columns("PVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("POINTS").HeaderText = "POINTS"
            .Columns("PVALUE").HeaderText = "VALUE"
            FormatGridColumns(objGridShower.gridView, False, False, True, False)
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        End With
        FormatGridColumns(objGridShower.gridView, False, , , False)
        objGridShower.Show()
        If objGridShower.gridView.Columns.Contains("ACTYPE") Then objGridShower.gridView.Columns("ACTYPE").Visible = False
        If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
    End Sub
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            objGridShower.Close()
        End If
    End Sub
    Private Sub PRIVTRAN()
        'Prop_Sets()
        gridView.DataSource = Nothing
        Me.Refresh()
        If PREV_RECORD Then
            PRIVILEGETRAN()
            Exit Sub
        Else
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            strSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGESUMMARY"
            strSql += vbCrLf + "  @SYSTEMID = '" & Sysid & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom_OWN.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo_OWN.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@WithChitPoints = '" & IIf(chkchit.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WithFromTO = '" & IIf(chkfromtorange.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + " ,@FromPoints = '" & txtFrom_Point_NUM.Text.ToString() & "'"
            strSql += vbCrLf + " ,@ToPoints = '" & txtTo_Point_NUM.Text.ToString() & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET OPPOINTS = NULL WHERE OPPOINTS = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET CURPOINTS = NULL WHERE CURPOINTS = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET REDEEMPOINTS = NULL WHERE REDEEMPOINTS = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET CLOSINGPOINTS = NULL WHERE CLOSINGPOINTS = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET OPPOINTSVALUE = NULL WHERE OPPOINTSVALUE = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET CURPOINTSVALUE = NULL WHERE CURPOINTSVALUE = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET REDEEMPOINTSVALUE = NULL WHERE REDEEMPOINTSVALUE = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET CLOSINGPOINTSVALUE = NULL WHERE CLOSINGPOINTSVALUE = 0"
            strSql += vbCrLf + "UPDATE MASTER..TEMP" & Sysid & "PREVILEGESUMMARY SET CLOSINGPOINTSVALUE = NULL WHERE CLOSINGPOINTSVALUE = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM MASTER..TEMP" & Sysid & "PREVILEGESUMMARY ORDER BY SNO"
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
                ''For cnt As Integer = 0 To .ColumnCount - 1
                ''    .Visible = False
                ''Next
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False


                .Columns("PARTICULAR").Width = 150
                .Columns("CUSTOMER").Width = 150
                .Columns("OPPOINTS").Width = 80
                .Columns("OPPOINTS").HeaderText = "OPE - Pts"
                .Columns("CURPOINTS").Width = 80
                .Columns("CURPOINTS").HeaderText = "CUR - Pts"
                .Columns("REDEEMPOINTS").Width = 80
                .Columns("REDEEMPOINTS").HeaderText = "RED - Pts"
                .Columns("CLOSINGPOINTS").Width = 80
                .Columns("CLOSINGPOINTS").HeaderText = "CLO - Pts"
                .Columns("OPPOINTSVALUE").Width = 80
                .Columns("OPPOINTSVALUE").HeaderText = "OPE - Val"
                .Columns("CURPOINTSVALUE").Width = 80
                .Columns("CURPOINTSVALUE").HeaderText = "CUR - Val"
                .Columns("REDEEMPOINTSVALUE").Width = 80
                .Columns("REDEEMPOINTSVALUE").HeaderText = "RED- Val"
                .Columns("CLOSINGPOINTSVALUE").Width = 80
                .Columns("CLOSINGPOINTSVALUE").HeaderText = "CLO - Val"

                ' .Columns("CUSTOMER").Visible = rbtBillNo.Checked
            End With
        End If
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView)

        Dim TITLE As String
        TITLE += lblcustname.Text & "  PREVILEGE SUMMARY FROM " & dtpFrom_OWN.Text & " TO " & dtpTo_OWN.Text & ""
        'TITLE += "  AT " & chkCmbCostCentre.Text & ""
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        btnView_Search.Enabled = True
        gridView.Focus()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        PRIVTRAN()
        btnView_Search.Enabled = True
        Exit Sub
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        'lblTitle.Text = TITLE
        gridView.Focus()
    End Sub

    Private Sub frmPrevilegeSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPrevilegeSummary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
            chkchit.Visible = True
        Else
            chkchit.Visible = False
        End If
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
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
        dtpFrom_OWN.Value = GetServerDate()
        dtpTo_OWN.Value = GetServerDate()
        'rbtSummary.Checked = True
        'cmbCostCentre.Text = "ALL"
        'cmbMetal.Text = "ALL"
        grpRange.Visible = False
        gridView.DataSource = Nothing
        txtMsg_OWN.Visible = False
        chkASD.Checked = True
        btnView_Search.Enabled = True
        txtPrevilegeId.Text = ""
        Prop_Sets()
        dtpFrom_OWN.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
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
            If gridView.DataSource IsNot Nothing Then
                If gridView.Rows.Count > 0 Then
                    If gridView.CurrentRow.Cells("PREVILEGEID").Value.ToString <> "" Then
                        PreViledgeDetail(gridView.CurrentRow.Cells("PREVILEGEID").Value.ToString)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub
    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo_OWN.MinimumDate = dtpFrom_OWN.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPrevilegeSummary_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmPrevilegeSummary_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPrevilegeSummary_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPrevilegeSummary_Properties))
    End Sub
    Private Sub chkfromtorange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkfromtorange.CheckedChanged
        If chkfromtorange.Checked Then
            grpRange.Visible = True
            txtFrom_Point_NUM.Enabled = True
            txtTo_Point_NUM.Enabled = True
        Else
            grpRange.Visible = False
            txtFrom_Point_NUM.Enabled = False
            txtTo_Point_NUM.Enabled = False
        End If
    End Sub

    Private Sub dtpTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpTo_OWN.TextChanged

    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        'If lstToMobileNo_OWN.Items.Count = 0 Then MsgBox("To Mobile No's Empty.", MsgBoxStyle.Information) : Exit Sub
        If PREV_RECORD Then
            If funcDbChecker("AKSHAYASMSDB") = 0 Then MsgBox("AkshayaSms Database Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
            If funcTableChecker("AKSHAYASMSDB", "SMSDATA") = 0 Then MsgBox("SmsData Table Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
            Dim TEMP_SMS_PREV_SUM As String
            If txtMsg_OWN.Text.Trim <> "" Then SMS_PREV_SUM = txtMsg_OWN.Text
            If gridView.RowCount > 0 Then
                    For i As Integer = 0 To gridView.RowCount - 1
                        TEMP_SMS_PREV_SUM = ""
                        TEMP_SMS_PREV_SUM = SMS_PREV_SUM
                        TEMP_SMS_PREV_SUM = Replace(TEMP_SMS_PREV_SUM, "<PREVILEGEID>", gridView.Rows(i).Cells("PREVILEGEID").Value.ToString)
                        TEMP_SMS_PREV_SUM = Replace(TEMP_SMS_PREV_SUM, "<BALPOINTS>", gridView.Rows(i).Cells("CLOSE_POINTS").Value.ToString)
                        TEMP_SMS_PREV_SUM = Replace(TEMP_SMS_PREV_SUM, "<BALPVALUE>", gridView.Rows(i).Cells("CLOSE_PVALUE").Value.ToString)
                        If gridView.Rows(i).Cells("MOBILE").Value.ToString <> "" Then
                            strSql = " INSERT INTO AKSHAYASMSDB..SMSDATA ([MOBILENO],[MESSAGES],[STATUS]) "
                            strSql += " VALUES('" & gridView.Rows(i).Cells("MOBILE").Value.ToString & "','" & TEMP_SMS_PREV_SUM & "','') "
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If
                MsgBox("SMS SEND", MsgBoxStyle.Information)
            Else
                If (txtMsg_OWN.Text).Length = 0 Then MsgBox("Message Empty.", MsgBoxStyle.Information) : txtMsg_OWN.Focus() : Exit Sub
            If chksms.Checked Then
                If funcDbChecker("AKSHAYASMSDB") = 0 Then MsgBox("AkshayaSms Database Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
                If funcTableChecker("AKSHAYASMSDB", "SMSDATA") = 0 Then MsgBox("SmsData Table Not found for Send Sms.", MsgBoxStyle.Information) : Exit Sub
                funcInsertAkshayaSmsTable()
            Else
                MsgBox("Please Select SMSSEND Option.", MsgBoxStyle.Information) : Exit Sub
            End If
            If SmsVia = "AkshayaSms" And SmsFlag Then
                MsgBox("Sms Generated,Please run Alert Sms Application!", MsgBoxStyle.Information)
                txtMsg_OWN.Text = ""

            End If
        End If
    End Sub
    Private Sub funcInsertAkshayaSmsTable()
        dt = CType(gridView.DataSource, DataTable).Copy
        Dim dv As DataView
        dv = dt.DefaultView
        Dim mobileno As String = ""
        For Each ro As DataRow In dv.ToTable.Rows
            mobileno = ro.Item("MOBILE").ToString
            If mobileno <> "" Then
                strSql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,UPDATED)VALUES('" & mobileno & "','" & txtMsg_OWN.Text & "','','" & Date.Now & "') "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                SmsFlag = True
            End If
        Next
        SmsVia = "AkshayaSms"
    End Sub
    Function funcDbChecker(ByVal dbname As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & dbname & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function funcTableChecker(ByVal dbname As String, ByVal TblName As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT NAME FROM " & dbname & "..SYSOBJECTS WHERE NAME = '" & TblName & "' AND XTYPE='U'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function

    Private Sub chksms_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chksms.CheckedChanged
        If chksms.Checked = True Then
            txtMsg_OWN.Visible = True
        Else
            txtMsg_OWN.Visible = False
        End If
    End Sub

    Private Sub chkASD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkASD.CheckedChanged
        If chkASD.Checked Then
            lblTo.Visible = False
            dtpTo_OWN.Visible = False
        Else
            lblTo.Visible = True
            dtpTo_OWN.Visible = True
        End If
    End Sub

    Private Sub txtPrevilegeId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPrevilegeId.KeyDown

    End Sub

    Private Sub txtPrevilegeId_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrevilegeId.KeyPress
        Dim NotAllowedKeys As String = "`~!@#$%^&*()_-+=/*<>?:'[]{}|\"
        e.Handled = NotAllowedKeys.Contains(e.KeyChar)
        If NotAllowedKeys.Contains(e.KeyChar.ToString) Then MsgBox("Special Char Not Allowed")
    End Sub

    Private Sub gridView_Scroll(sender As Object, e As ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class

Public Class frmPrevilegeSummary_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkchit As Boolean = True
    Public Property p_chkchit() As Boolean
        Get
            Return chkchit
        End Get
        Set(ByVal value As Boolean)
            chkchit = value
        End Set
    End Property
    Private chksms As Boolean = False
    Public Property p_chksms() As Boolean
        Get
            Return chksms
        End Get
        Set(ByVal value As Boolean)
            chksms = value
        End Set
    End Property
    Private chkfromtorange As Boolean = False
    Public Property p_chkfromtorange() As Boolean
        Get
            Return chkfromtorange
        End Get
        Set(ByVal value As Boolean)
            chkfromtorange = value
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


