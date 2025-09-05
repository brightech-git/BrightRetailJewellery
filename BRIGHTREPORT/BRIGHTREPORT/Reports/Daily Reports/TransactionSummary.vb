Imports System.Data.OleDb
Public Class TransactionSummary
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim StrCostFiltration As String
    Dim strFilter As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dsGrid As DataSet
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
    Dim SelectedCompany As String
    Dim Authorize As Boolean = False
    Dim Save As Boolean = False

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnStockDb & "..SoftControlTran"
        strSql += " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Function funcSep(ByVal TranTable As String, ByVal StoneTable As String, ByVal diaType As String, ByVal TrType As String, ByVal TaxType As String) As String
        Dim Qry As String = Nothing
        ''--GENERATE SEP A/C
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME"
        Qry += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID   "
        Qry += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH   "
        Qry += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        Qry += " ,TRANDATE,TRANNO,TRANTYPE  "
        Qry += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT  ,SUM(VAT)VAT "
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER ,"
        Qry += " ' 'BATCHNO,'1'RESULT ,'1'SALETYPE  "
        Qry += " FROM   "
        Qry += " ("
        Qry += " SELECT X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO "
        Qry += " ,(SELECT TRANDATE FROM " & cnStockDb & ".." & TranTable & " WHERE SNO = ISSSNO AND COMPANYID IN (" & SelectedCompany & "))TRANDATE"
        Qry += " ,SUM(X.AMOUNT)AMOUNT "
        Qry += " ,ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO "
        Qry += " AND COMPANYID IN (" & SelectedCompany & ")"
        Qry += " AND PAYMODE = '" & TaxType & "' AND TRANNO = X.TRANNO AND ACCODE = "
        Qry += " (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT   "
        Qry += " ,SUM(X.PCS)PCS ,SUM(X.WT)AS GRSWT ,SUM(X.WT)AS NETWT ,SUM(X.STNWT)STNWT "
        Qry += " ,SUM(X.DIAWT)DIAWT ,X.TRANTYPE ,X.BATCHNO,'1'RESULT   "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO "
        Qry += " WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO))AS CUSTOMER   "
        Qry += " FROM "
        Qry += " ( "
        Qry += " SELECT  CATCODE ,TRANNO ,ISSSNO ,BATCHNO ,TRANTYPE "
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME "
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID   "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        Qry += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID))AS METALNAME   "
        Qry += " ,SUM(STNAMT)AMOUNT ,SUM(STNPCS)PCS "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'S' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT "
        Qry += " ,SUM(STNWT)WT"
        Qry += " FROM " & cnStockDb & ".." & StoneTable & " AS S WHERE 1=1 AND TRANTYPE = '" & TrType & "' "
        Qry += " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
        Qry += " )AS X"
        Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,"
        Qry += " X.BATCHNO,X.TRANTYPE,X.PCS"
        Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,"
        Qry += " TRANTYPE,CUSTOMER,BATCHNO "
        ''--DEDUCT SEP A/C AMT
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
        Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
        Qry += " ,SUM(VAT)VAT"
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER ,"
        Qry += " ' 'BATCHNO,'1'RESULT ,'1'SALETYPE "
        Qry += " FROM  "
        Qry += " ("
        Qry += " SELECT "
        Qry += " I.CATCODE"
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME"
        Qry += " ,I.TRANNO"
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        Qry += " ,I.SNO"
        Qry += " ,DATEPART(MONTH,I.TRANDATE)AS MMONTHID  "
        Qry += " ,LEFT(DATENAME(MONTH,I.TRANDATE),3)MMONTH  "
        Qry += " ,CONVERT(VARCHAR,I.TRANDATE,103)AS UUPDATED ,TRANDATE  "
        Qry += " ,SUM(-1*X.AMOUNT)AMOUNT"
        Qry += " ,ISNULL((SELECT -1*AMOUNT FROM " & cnStockDb & "..ACCTRAN "
        Qry += " WHERE BATCHNO = X.BATCHNO AND PAYMODE = '" & TaxType & "' AND TRANNO = X.TRANNO "
        Qry += " AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY "
        Qry += " WHERE CATCODE = X.CATCODE) AND COMPANYID IN (" & SelectedCompany & ")),0)AS VAT  "
        Qry += " ,SUM(X.STNWT)STNWT"
        Qry += " ,SUM(X.DIAWT)DIAWT"
        Qry += " ,I.TRANTYPE ,I.BATCHNO,'1'RESULT  "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO "
        Qry += " WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS CUSTOMER  "
        Qry += " FROM"
        Qry += " ("
        Qry += " SELECT "
        Qry += " CATCODE"
        Qry += " ,TRANNO"
        Qry += " ,ISSSNO"
        Qry += " ,BATCHNO"
        Qry += " ,TRANTYPE"
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME"
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID  "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME  "
        Qry += " ,SUM(STNAMT)AMOUNT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'T' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT"
        Qry += " FROM " & cnStockDb & ".." & StoneTable & " AS S WHERE 1=1 AND TRANTYPE = '" & TrType & "' "
        Qry += " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
        Qry += " )AS X," & cnStockDb & ".." & TranTable & " AS I"
        Qry += strFilter
        Qry += " AND X.ISSSNO = I.SNO"
        Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,"
        Qry += " I.BATCHNO,X.BATCHNO,X.CATCODE,X.TRANNO"
        Qry += " )AS SEPDIA"
        Qry += " GROUP BY"
        Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,"
        Qry += " TRANNO,TRANTYPE,CUSTOMER,BATCHNO"
        Return Qry
    End Function

    Function funcSepStnAc(ByVal TranTable As String, ByVal StoneTable As String, ByVal TrType As String, ByVal TaxType As String) As String
        Dim Qry As String = Nothing
        If SepDiaPost = "Y" Then
            Qry += funcSep(TranTable, StoneTable, "D", TrType, TaxType)
        End If
        If SepStnPost = "Y" Then
            Qry += funcSep(TranTable, StoneTable, "T", TrType, TaxType)
        End If
        If SepPrePost = "Y" Then
            Qry += funcSep(TranTable, StoneTable, "P", TrType, TaxType)
        End If
        Return Qry
    End Function

    Function funcAbstract(ByVal TempTable As String, ByVal TranTable As String, ByVal StoneTable As String, ByVal TrType As String, ByVal TaxType As String) As Integer
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSTEMP')>0"
        strSql += " DROP TABLE TEMP" & systemId & "ABSTEMP"
        strSql += " SELECT"
        strSql += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = X.BATCHNO AND PAYMODE = '" & TaxType & "' AND TRANNO = X.TRANNO "
        strSql += " AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE CATCODE = X.CATCODE) AND COMPANYID IN (" & SelectedCompany & ")),0)AS VAT"
        strSql += " ,SUM(AMOUNT)AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,CUSTOMER"
        strSql += " ,BATCHNO"
        strSql += " ,'1'RESULT"
        strSql += " ,'1'SALETYPE"
        strSql += " INTO TEMP" & systemId & "ABSTEMP"
        strSql += " FROM "
        strSql += " ("
        strSql += " SELECT"
        strSql += " CATCODE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strSql += " ,ITEMID"
        strSql += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ITEMID = I.ITEMID))AS METALID"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ITEMID = I.ITEMID))AS METALNAME"
        strSql += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID"
        strSql += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        strSql += " ,TRANDATE"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
        strSql += " WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & ".." & TranTable & " WHERE "
        strSql += " BATCHNO = I.BATCHNO AND COMPANYID IN (" & SelectedCompany & ")) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE DIASTONE = 'S') AND COMPANYID IN (" & SelectedCompany & ")),0)STNWT"
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
        strSql += " WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & ".." & TranTable & " WHERE "
        strSql += " BATCHNO = I.BATCHNO AND COMPANYID IN (" & SelectedCompany & ")) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE DIASTONE = 'D') AND COMPANYID IN (" & SelectedCompany & ")),0)DIAWT"
        strSql += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += " WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        strSql += " ,TRANTYPE,BATCHNO"
        strSql += " ,'1'RESULT"
        strSql += " FROM " & cnStockDb & ".." & TranTable & " AS I"
        strSql += strFilter
        strSql += " AND TRANTYPE = '" & TrType & "'"
        strSql += " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,TRANDATE"
        strSql += " )AS X"
        strSql += " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,"
        strSql += " MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER"
        strSql += funcSepStnAc(TranTable, StoneTable, TrType, TaxType)
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 300
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = '" & TempTable & "')>0"
        strSql += " DROP TABLE " & TempTable & ""
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,UUPDATED,"
        strSql += " TRANDATE,'   '+TRANNO TRANNO,TRANTYPE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(VAT)VAT"
        strSql += " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,CUSTOMER"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'CA' AND COMPANYID IN (" & SelectedCompany & ")),0)AS CASH"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'RO' AND COMPANYID IN (" & SelectedCompany & ")),0)AS ROUND"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'CC' AND COMPANYID IN (" & SelectedCompany & ")),0)AS CREDITCARD"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'CH' AND COMPANYID IN (" & SelectedCompany & ")),0)AS CHEQUE"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'SS' AND COMPANYID IN (" & SelectedCompany & ")),0)AS CHITCARD"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'DI' AND COMPANYID IN (" & SelectedCompany & ")),0)AS DISCOUNT"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN "
        strSql += " WHERE BATCHNO = T.BATCHNO AND PAYMODE = 'HC' AND COMPANYID IN (" & SelectedCompany & ")),0)AS HANDLING"
        strSql += " ,'1'RESULT"
        strSql += " ,SALETYPE"
        strSql += " INTO " & TempTable & ""
        strSql += " FROM "
        strSql += " TEMP" & systemId & "ABSTEMP AS T"
        strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,METALNAME,METALID,UUPDATED,"
        strSql += " TRANDATE,TRANNO,TRANTYPE,CUSTOMER,BATCHNO"

        ''PAYMENT DETAILS
        ''CREDIT CARD DETAILS
        'strSql += " UNION ALL"
        'strSql += " SELECT "
        'strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,"
        'strSql += " UUPDATED,T.TRANDATE,'   '+T.TRANNO TRANNO,TRANTYPE"
        'strSql += " ,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT"
        'strSql += " ,0 VAT"
        'strSql += " ,0 TOTAL"
        'strSql += " ,0 STNWT"
        'strSql += " ,0 DIAWT"
        'strSql += " ,'CREDITCARD NO:'+ ACC.CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        'strSql += " ,0 CASH"
        'strSql += " ,0 ROUND"
        'strSql += " ,0 CREDITCARD"
        'strSql += " ,0 CHEQUE"
        'strSql += " ,0 CHITCARD"
        'strSql += " ,0 DISCOUNT"
        'strSql += " ,0 HANDLING"
        'strSql += " ,'1'RESULT"
        'strSql += " ,SALETYPE"
        'strSql += " FROM "
        'strSql += " TEMP" & systemId & "ABSTEMP AS T," & cnStockDb & "..ACCTRAN AS ACC"
        'strSql += " WHERE T.BATCHNO = ACC.BATCHNO AND T.TRANNO = ACC.TRANNO AND ACC.PAYMODE = 'CC'"
        'strSql += " GROUP BY CATCODE,CATNAME,UUPDATED,T.TRANDATE,T.TRANNO,TRANTYPE,SALETYPE,ACC.CHQCARDNO"

        ' ''CHEQUE  DETAILS
        'strSql += " UNION ALL"
        'strSql += " SELECT "
        'strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,"
        'strSql += " UUPDATED,T.TRANDATE,'   '+T.TRANNO TRANNO,TRANTYPE"
        'strSql += " ,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT"
        'strSql += " ,0 VAT"
        'strSql += " ,0 TOTAL"
        'strSql += " ,0 STNWT"
        'strSql += " ,0 DIAWT"
        'strSql += " ,'CHEQUE NO:'+ ACC.CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        'strSql += " ,0 CASH"
        'strSql += " ,0 ROUND"
        'strSql += " ,0 CREDITCARD"
        'strSql += " ,0 CHEQUE"
        'strSql += " ,0 CHITCARD"
        'strSql += " ,0 DISCOUNT"
        'strSql += " ,0 HANDLING"
        'strSql += " ,'1'RESULT"
        'strSql += " ,SALETYPE"
        'strSql += " FROM "
        'strSql += " TEMP" & systemId & "ABSTEMP AS T," & cnStockDb & "..ACCTRAN AS ACC"
        'strSql += " WHERE T.BATCHNO = ACC.BATCHNO AND T.TRANNO = ACC.TRANNO AND ACC.PAYMODE = 'CH'"
        'strSql += " GROUP BY CATCODE,CATNAME,UUPDATED,T.TRANDATE,T.TRANNO,TRANTYPE,SALETYPE,ACC.CHQCARDNO"

        ''SCHEME CARD DETAILS
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,"
        strSql += " UUPDATED,T.TRANDATE,'   '+T.TRANNO TRANNO,TRANTYPE"
        strSql += " ,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT"
        strSql += " ,0 VAT"
        strSql += " ,0 TOTAL"
        strSql += " ,0 STNWT"
        strSql += " ,0 DIAWT"
        strSql += " ,'CHITCARD NO:'+ ACC.CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,0 CASH"
        strSql += " ,0 ROUND"
        strSql += " ,0 CREDITCARD"
        strSql += " ,0 CHEQUE"
        strSql += " ,0 CHITCARD"
        strSql += " ,0 DISCOUNT"
        strSql += " ,0 HANDLING"
        strSql += " ,'1'RESULT"
        strSql += " ,SALETYPE"
        strSql += " FROM "
        strSql += " TEMP" & systemId & "ABSTEMP AS T," & cnStockDb & "..ACCTRAN AS ACC"
        strSql += " WHERE T.BATCHNO = ACC.BATCHNO AND T.TRANNO = ACC.TRANNO AND ACC.PAYMODE = 'SS'"
        strSql += " AND COMPANYID IN (" & SelectedCompany & ")"
        strSql += " GROUP BY CATCODE,CATNAME,UUPDATED,T.TRANDATE,T.TRANNO,TRANTYPE,SALETYPE,ACC.CHQCARDNO"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcFiltration() As Integer
        Dim tempChkItem As String = Nothing : Dim tmpcnt As Integer = 0
        StrCostFiltration = ""
        strFilter = " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompany & ")"

        ' ''COST CENTRE
        'If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then
        '    strFilter += " AND COSTID = '" & dtCostCentre.Rows(cmbCostCentre.SelectedIndex - 1).Item(0).ToString() & "' "
        '    StrCostFiltration += " AND COSTID = '" & dtCostCentre.Rows(cmbCostCentre.SelectedIndex - 1).Item(0).ToString() & "' "
        'End If

        ''CASH COUNTER
        tempChkItem = ""
        If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                If chkLstCashCounter.GetItemChecked(CNT) = True Then
                    tempChkItem += " '" & dtCashCounter.Rows(CNT - 1).Item(0) + "'"
                    tmpcnt += 1
                    If tmpcnt < chkLstCashCounter.CheckedItems.Count Then tempChkItem += ","
                End If
            Next
        Else
            tempChkItem = ""
        End If
        If tempChkItem <> "" Then strFilter += " AND CASHID IN (" & tempChkItem & ") "


        ''NODE ID
        tempChkItem = "" : tmpcnt = 0
        If chkLstNodeId.Items.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                If chkLstNodeId.GetItemChecked(CNT) = True Then
                    tempChkItem = tempChkItem & " '" & chkLstNodeId.Items.Item(CNT) & "'"
                    tmpcnt += 1
                    If tmpcnt < chkLstNodeId.CheckedItems.Count Then tempChkItem += ","
                End If
            Next
        Else
            tempChkItem = ""
        End If
        If tempChkItem <> "" Then strFilter += " AND SYSTEMID IN (" & tempChkItem & ")"

        strFilter += " AND ISNULL(CANCEL,'') <> 'Y'"
    End Function

    Function funcInsertSales() As Integer
        funcAbstract("TEMP" & systemId & "SALESABS", "ISSUE", "ISSSTONE", "SA", "SV")
        ''--SA
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SATRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SATRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES'GROUPNAME"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'1'GGROUP"
        strSql += " ,'2'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SATRAN"
        strSql += " FROM TEMP" & systemId & "SALESABS"
        strSql += " GROUP BY CATCODE,CATNAME,TRANNO,CUSTOMER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''--------------------------------------------------------------------
        ''--SA SUBTOTAL
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SASUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SASUBTRAN"
        strSql += " SELECT "
        strSql += " 'T'CATCODE"
        strSql += " ,'SALES[SA]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES'GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'1'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SASUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT"
        strSql += " ,NETWT,AMOUNT,VAT,GROUPNAME"
        strSql += " ,CASH"
        strSql += " ,ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT FROM TEMP" & systemId & "SATRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "SATRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'SALES DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'1'GGROUP,'1'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "SATRAN(CATNAME,RESULT,GROUPNAME,GGROUP,COLHEAD) "
        strSql += " SELECT DISTINCT CATNAME,'1'RESULT,'SALES',1,' ' "
        strSql += " FROM TEMP" & systemId & "SATRAN WHERE RESULT <> 1 "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,"
        strSql += " (CASE WHEN RESULT=1 THEN CATNAME ELSE TRANNO END) PARTICULAR, "
        strSql += " PCS, GRSWT, NETWT, AMOUNT, VAT, TOTAL, GROUPNAME, TRANNO, CUSTOMER, "
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SATRAN "
        strSql += " ORDER BY COLHEAD DESC ,CATNAME,RESULT  "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SASUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Function
    Function funcInsertSalesReturn() As Integer
        funcAbstract("TEMP" & systemId & "SALESRETURNABS", "RECEIPT", "RECEIPTSTONE", "SR", "PV")
        ''--SR
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SRTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SRTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES RETURN'GROUPNAME"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'2'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SRTRAN"
        strSql += " FROM TEMP" & systemId & "SALESRETURNABS"
        strSql += " GROUP BY CATCODE,CATNAME,TRANNO,CUSTOMER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------
        ''-SR SUBTOTAL
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SRSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SRSUBTRAN"
        strSql += " SELECT 'T'CATCODE,'SALES RETURN[SR]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES RETURN'GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'2'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SRSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT"
        strSql += " ,NETWT,AMOUNT,VAT,GROUPNAME"
        strSql += " ,CASH"
        strSql += " ,ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT FROM TEMP" & systemId & "SRTRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "SRTRAN(CATNAME,RESULT,GROUPNAME,GGROUP,COLHEAD) "
        strSql += " SELECT DISTINCT CATNAME,'2'RESULT,'SALES RETURN',2,' ' "
        strSql += " FROM TEMP" & systemId & "SRTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "SRTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'SALES RETURN DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,"
        strSql += " 0 VAT,'SALES'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'2'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'2'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,"
        strSql += " (CASE WHEN RESULT=2 THEN CATNAME ELSE TRANNO END) PARTICULAR, "
        strSql += " PCS, GRSWT, NETWT, AMOUNT, VAT, TOTAL, GROUPNAME, TRANNO, CUSTOMER, "
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SRTRAN "
        strSql += " ORDER BY COLHEAD DESC ,CATNAME,RESULT  "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SRSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcInsertReceipt() As Integer
        ''--RECEIPT
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "RECTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, SUM(AMOUNT) AS TOTAL"
        strSql += " ,'RECEIPTS'GROUPNAME"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'3'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "RECTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'ADVANCE RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'AR' THEN SUM(CASE WHEN TRANMODE = 'C' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'AR' THEN SUM(CASE WHEN TRANMODE = 'C' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'AR' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' ' AS CATCODE,'MISCELLANEOUS RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'MR' THEN SUM(CASE WHEN TRANMODE = 'C' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'MR' THEN SUM(CASE WHEN TRANMODE = 'C' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' "
        strSql += " THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'MR' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'GIFT VOUCHER RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'GR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'GR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'GR' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'DUE RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'DR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'DR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'DR' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'TOBE RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'TP' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'TP' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'TP' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ORDER RECEIPT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'OR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'OR' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'OR' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"
        strSql += " )X"
        strSql += " GROUP BY CATCODE,CATNAME,TRANNO,CUSTOMER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''PAYMENT DETAILS
        ''CREDIT CARD DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'RECEIPTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CREDITCARD:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'3'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'CC'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''CHEQUE DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'RECEIPTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CHEQUE:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'3'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'CH'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''CHITCARD DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'RECEIPTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CHITCARD:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'3'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'SS'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------------
        ''--RECEIPT SUB TOTAL
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "RECSUBTRAN"
        strSql += " SELECT 'T'CATCODE,'RECEIPT[REC]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, SUM(AMOUNT)  AS TOTAL"
        strSql += " ,'RECEIPTS'GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'3'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "RECSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT"
        strSql += " ,NETWT"
        strSql += " ,AMOUNT"
        strSql += " ,VAT"
        strSql += " ,GROUPNAME"
        strSql += " ,CASH"
        strSql += " ,ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT FROM TEMP" & systemId & "RECTRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "RECTRAN(CATNAME,RESULT,GROUPNAME,GGROUP,COLHEAD,CATCODE) "
        strSql += " SELECT DISTINCT CATNAME,'2'RESULT,'PURCHASE',3,' ',' ' "
        strSql += " FROM TEMP" & systemId & "RECTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'RECEIPT DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'RECEIPT'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'3'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'RECEIPT'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'3'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,"
        strSql += " (CASE WHEN RESULT=2 THEN CATNAME ELSE TRANNO END) PARTICULAR, "
        strSql += " PCS, GRSWT, NETWT, AMOUNT, VAT, TOTAL, GROUPNAME, TRANNO, CUSTOMER, "
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECTRAN "
        strSql += " ORDER BY COLHEAD DESC ,CATNAME,RESULT  "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcInsertPayment() As Integer
        ''--PAYMENT
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PAYTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PAYTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, SUM(AMOUNT)  AS TOTAL"
        strSql += " ,'PAYMENTS'GROUPNAME"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'4'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PAYTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'ADVANCE PAYMENT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'AP' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'AP' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'AP' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' ' AS CATCODE,'ADVANCE ISSUE'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'AI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'AI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'AI' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'MISCELLANEOUS PAYMENT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'MP' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'MP' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(AMOUNT) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'MP' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'DUE PAYMENT'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'DY' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'DY' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'DY' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'GIFT VOUCHER ISSUE'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'GI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'GI' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'GI' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ADVANCE ADJUSTED'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'AA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'AA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'AA' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'CREDIT BILL(DUE)'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'DU' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'DU' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DU'"
        ''strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'DU')"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'TOBE ADJUSTED'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'TA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'TA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'TA' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ORDER ADJUSTED'CATNAME"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,0 VAT"
        strSql += " ,CASE WHEN PAYMODE = 'OA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END AMOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'OA' THEN SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END TOTAL"
        strSql += " ,CASE WHEN PAYMODE = 'CA' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CASH"
        strSql += " ,CASE WHEN PAYMODE = 'CC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CREDITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'CH' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHEQUE"
        strSql += " ,CASE WHEN PAYMODE = 'SS' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END CHITCARD"
        strSql += " ,CASE WHEN PAYMODE = 'RO' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END ROUND"
        strSql += " ,CASE WHEN PAYMODE = 'DI' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END DISCOUNT"
        strSql += " ,CASE WHEN PAYMODE = 'HC' THEN SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) ELSE 0 END HANDLING"
        strSql += " ,TRANNO"
        strSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO))AS CUSTOMER"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE = 'OA' AND COMPANYID IN (" & SelectedCompany & "))"
        strSql += " GROUP BY PAYMODE,TRANNO,BATCHNO"

        strSql += " )X"
        strSql += " GROUP BY CATCODE,CATNAME,TRANNO,CUSTOMER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''PAYMENT DETAILS
        ''CREDIT CARD DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'PAYMENTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CREDITCARD:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'4'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'CC'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''CHEQUE DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'PAYMENTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CHEQUE:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'4'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'CH'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''CHITCARD DETAILS
        strSql = "INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " SELECT CATCODE,CATNAME"
        strSql += " ,'0'PCS,'0'GRSWT,'0'NETWT,'0'AMOUNT,'0'VAT,'0'AS TOTAL"
        strSql += " ,'PAYMENTS'GROUPNAME"
        strSql += " ,T.TRANNO"
        strSql += " ,'CHITCARD:'+CHQCARDNO + '[RS:'+CONVERT(VARCHAR,SUM(ACC.AMOUNT))+']' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,'4'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYTRAN T," & cnStockDb & "..ACCTRAN ACC"
        strSql += strFilter
        strSql += " AND T.TRANNO = ACC.TRANNO AND PAYMODE = 'SS'"
        strSql += " GROUP BY CATCODE,CATNAME,T.TRANNO,ACC.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------------
        ''--PAYMENT SUB TOTAL
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PAYSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PAYSUBTRAN"
        strSql += " SELECT 'T'CATCODE,'PAYMENTS[PAY]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, SUM(AMOUNT) AS TOTAL"
        strSql += " ,'PAYMENTS'GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'4'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PAYSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT"
        strSql += " ,NETWT"
        strSql += " ,AMOUNT"
        strSql += " ,VAT"
        strSql += " ,GROUPNAME"
        strSql += " ,CASH"
        strSql += " ,ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT FROM TEMP" & systemId & "PAYTRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PAYTRAN(CATNAME,RESULT,GROUPNAME,GGROUP,COLHEAD,CATCODE) "
        strSql += " SELECT DISTINCT CATNAME,'2'RESULT,'PURCHASE',4,' ',' ' "
        strSql += " FROM TEMP" & systemId & "PAYTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PAYMENT DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,"
        strSql += " 0 VAT,'PAYMENT'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'4'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,ROUND,"
        strSql += " CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'PAYMENT'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'4'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,"
        strSql += " (CASE WHEN RESULT=2 THEN CATNAME ELSE TRANNO END) PARTICULAR, "
        strSql += " PCS, GRSWT, NETWT, AMOUNT, VAT, TOTAL, GROUPNAME, TRANNO, CUSTOMER, "
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYTRAN "
        strSql += " ORDER BY COLHEAD DESC ,CATNAME,RESULT  "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcInsertPurchase() As Integer
        ''--PU
        funcAbstract("TEMP" & systemId & "PUTCHASEABS", "RECEIPT", "RECEIPTSTONE", "PU", "PV")
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PUTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PUTRAN"
        strSql += " SELECT ISNULL(CATCODE,'')CATCODE,ISNULL(CATNAME,'')CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PURCHASE'GROUPNAME"
        strSql += " ,CONVERT(VARCHAR,TRANNO)TRANNO"
        strSql += " ,CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'5'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PUTRAN"
        strSql += " FROM TEMP" & systemId & "PUTCHASEABS"
        strSql += " GROUP BY CATCODE,CATNAME,TRANNO,CUSTOMER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------
        ''--PURCHASE SUBTOTAL
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PUSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PUSUBTRAN"
        strSql += " SELECT 'T'CATCODE,'PURCHASE[PUR]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, "
        strSql += " SUM(VAT) VAT, (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PURCHASE'GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'CUSTOMER"
        strSql += " , SUM(CASH) CASH"
        strSql += " , SUM(ROUND) ROUND"
        strSql += " , SUM(CREDITCARD) CREDITCARD"
        strSql += " , SUM(CHEQUE) CHEQUE"
        strSql += " , SUM(CHITCARD) CHITCARD"
        strSql += " , SUM(DISCOUNT) DISCOUNT"
        strSql += " , SUM(HANDLING) HANDLING"
        strSql += " ,'5'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PUSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT"
        strSql += " ,NETWT,AMOUNT,VAT,GROUPNAME"
        strSql += " ,CASH"
        strSql += " ,ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT FROM TEMP" & systemId & "PUTRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PUTRAN(CATNAME,RESULT,GROUPNAME,GGROUP,COLHEAD,CATCODE) "
        strSql += " SELECT DISTINCT CATNAME,'2'RESULT,'PURCHASE',5,' ',' ' "
        strSql += " FROM TEMP" & systemId & "PUTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "PUTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PURCHASE DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,"
        strSql += " 0 VAT,'SALES'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'5'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'PURCHASE'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'5'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,"
        strSql += " (CASE WHEN RESULT=2 THEN CATNAME ELSE TRANNO END) PARTICULAR, "
        strSql += " PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PUTRAN "
        strSql += " ORDER BY COLHEAD DESC ,CATNAME,RESULT  "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PUSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Private Sub funcInsertChitCollection()
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDB'"
        If (Trim(objGPack.GetSqlValue(strSql, , "N"))) = "Y" Then
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                strSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE TEMP" & systemId & "CHITCOLLECTION "
                strSql += " SELECT  CASE "
                strSql += " WHEN MODEPAY = 'C' THEN 'SCHEME CASH' "
                strSql += " WHEN MODEPAY IN('Q','D') THEN 'CHEQUE' "
                strSql += " WHEN MODEPAY = 'R' THEN 'CREDITCARD'"
                strSql += " WHEN MODEPAY = 'E' THEN 'ETRANSFER'"
                strSql += " WHEN MODEPAY = 'O' THEN 'OTHERS'"
                strSql += " END CATNAME,"
                strSql += " SUM(AMOUNT) AS AMOUNT"
                strSql += " INTO TEMP" & systemId & "CHITCOLLECTION FROM " & cnChitTrandb & "..SCHEMECOLLECT "
                strSql += " WHERE RDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
                strSql += " '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
                strSql += " ISNULL(CANCEL,'') <> 'Y'"
                strSql += StrCostFiltration
                strSql += "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
                strSql += "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID = '" & strCompanyId & "')) )"
                strSql += " GROUP BY MODEPAY"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "CHITCOLLECTION)>0 "
                strSql += " BEGIN "
                strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
                strSql += " (CATNAME,GROUPNAME,COLHEAD)"
                strSql += " SELECT 'SCHEME COLLECTION','SCHEMECOLLECTION','T'"
                strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
                strSql += " (CATNAME,AMOUNT,TOTAL,GROUPNAME)"
                strSql += " SELECT CATNAME,AMOUNT,AMOUNT,'SCHEMECOLLECTION'GROUPNAME"
                strSql += " FROM TEMP" & systemId & "CHITCOLLECTION "
                strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
                strSql += " (CATNAME,AMOUNT,TOTAL,GROUPNAME,COLHEAD)"
                strSql += " SELECT 'SCHEME COLLECTION[SS]',SUM(AMOUNT),SUM(AMOUNT),'SCHEMECOLLECTION'GROUPNAME,'S'COLHEAD"
                strSql += " FROM TEMP" & systemId & "CHITCOLLECTION "
                strSql += " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
    End Sub

    Function funcOthers() As Integer
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "OTHERTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "OTHERTRAN"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,PCS,GRSWT,NETWT"
        strSql += " ,AMOUNT,VAT,TOTAL"
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME"
        strSql += " ,' ' TRANNO"
        strSql += " ,' ' AS CUSTOMER"
        strSql += " ,'0'CASH"
        strSql += " ,'0'ROUND"
        strSql += " ,'0'CREDITCARD"
        strSql += " ,'0'CHEQUE"
        strSql += " ,'0'CHITCARD"
        strSql += " ,'0'DISCOUNT"
        strSql += " ,'0'HANDLING"
        strSql += " ,GGROUP,RESULT "
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'DISCOUNT[DI]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
        strSql += " ,'OTHER'GROUPNAME,'6'GGROUP,'3'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DI' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'ROUND OFF[RO]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
        strSql += " ,'OTHER'GROUPNAME,'6'GGROUP,'4'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'RO' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'HANDLING CHARGES[HC]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
        strSql += " ,'OTHER'GROUPNAME,'6'GGROUP,'5'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'HC' "
        strSql += " )AS X"
        strSql += " ORDER BY GGROUP,RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "OTHERTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,CUSTOMER,TRANNO,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'OTHERS'GROUPNAME,' 'CUSTOMER,' 'TRANNO,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'6'GGROUP,'1'RESULT,0,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "OTHERTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,CUSTOMER,TRANNO,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'OTHERS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'OTHERS'GROUPNAME,' 'CUSTOMER,' 'TRANNO,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'6'GGROUP,'2'RESULT,0,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "OTHERTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "OTHERTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcInsertTotal() As Integer
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TOTTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "TOTTRAN"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,PCS,GRSWT,NETWT"
        strSql += " ,AMOUNT,VAT,TOTAL"
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME"
        strSql += " ,' ' TRANNO"
        strSql += " ,' ' AS CUSTOMER"
        strSql += " , CASH"
        strSql += " , ROUND"
        strSql += " ,CREDITCARD"
        strSql += " ,CHEQUE"
        strSql += " ,CHITCARD"
        strSql += " ,DISCOUNT"
        strSql += " ,HANDLING"
        strSql += " ,GGROUP,RESULT "
        strSql += " ,'G'COLHEAD"
        strSql += " INTO TEMP" & systemId & "TOTTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'G'CATCODE"
        strSql += " ,'SA-SR+REC-PAY-PU-DI-RO+HC+SS'CATNAME"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN GRSWT ELSE -1* GRSWT END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  NETWT ELSE -1* NETWT END) AS NETWT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  AMOUNT  ELSE -1* AMOUNT END) AS AMOUNT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  VAT ELSE -1* VAT END) AS VAT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  TOTAL ELSE -1* TOTAL END) AS TOTAL"

        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  CASH ELSE -1* CASH END) AS CASH"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  CREDITCARD ELSE -1* CREDITCARD END) AS CREDITCARD"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  CHEQUE ELSE -1* CHEQUE END) AS CHEQUE"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  CHITCARD ELSE -1* CHITCARD END) AS CHITCARD"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  DISCOUNT ELSE -1* DISCOUNT END) AS DISCOUNT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  ROUND ELSE -1* ROUND END) AS ROUND"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') OR GROUPNAME = 'CHITCOLLECTION' THEN  HANDLING ELSE -1* HANDLING END) AS HANDLING"
        strSql += " ,'GRAND TOTAL'GROUPNAME"
        strSql += " ,'7'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT 'S'CATCODE,'SA'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "SASUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'SR'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "SRSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'REC'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "RECSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'PAY'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "PAYSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'PU'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "PUSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,CATNAME,PCS,GRSWT,NETWT,ISNULL(AMOUNT,0)AMOUNT,VAT,ISNULL(TOTAL,0)TOTAL,GROUPNAME,CASH,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,ROUND,HANDLING,GGROUP,RESULT FROM TEMP" & systemId & "OTHERTRAN WHERE RESULT >=3"
        strSql += " )X"
        strSql += " )TOT"
        strSql += " ORDER BY GGROUP"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TOTTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'SALES'GROUPNAME,"
        strSql += " ' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,0 CHITCARD,0 DISCOUNT,"
        strSql += " 0 HANDLING,'7'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "TOTTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'TOTAL'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'7'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "TOTTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcInsertCollection() As Integer
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "COLTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "COLTRAN"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,PCS,GRSWT,NETWT"
        strSql += " ,AMOUNT,VAT,TOTAL"
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME"
        strSql += " ,' 'TRANNO"
        strSql += " ,' 'AS CUSTOMER"
        strSql += " ,'0' CASH"
        strSql += " ,'0' ROUND"
        strSql += " ,'0' CREDITCARD"
        strSql += " ,'0' CHEQUE"
        strSql += " ,'0' CHITCARD"
        strSql += " ,'0' DISCOUNT"
        strSql += " ,'0' HANDLING"
        strSql += " ,GGROUP,RESULT "
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "COLTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'CASH[CA]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN "
        strSql += " ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'3'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'CA'"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'CREDIT CARD[CC]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'4'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'CC'"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'CHEQUE[CH]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'5'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'CH'"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'SAVINGS[SS]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'6'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'SS'"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT"
        strSql += " 'G'CATCODE,'CA+CC+CH+SS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,SUM(ISNULL(TOTAL,0))TOTAL"
        strSql += " ,'COL'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'8'GGROUP,'7'RESULT,'G'"
        strSql += " FROM TEMP" & systemId & "COLTRAN"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,"
        strSql += " 0 CHEQUE,0 CHITCARD,0 DISCOUNT,0 HANDLING,'8'GGROUP,'1'RESULT,0,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PAY COLLECTION'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,' 'TRANNO,' 'CUSTOMER,0 CASH,0 ROUND,0 CREDITCARD,0 CHEQUE,"
        strSql += " 0 CHITCARD,0 DISCOUNT,0 HANDLING,'8'GGROUP,'2'RESULT,0,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANDET"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,CASH,"
        strSql += " ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,TRANNO,CUSTOMER,"
        strSql += " CASH,ROUND,CREDITCARD,CHEQUE,CHITCARD,DISCOUNT,HANDLING,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "COLTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcGridViewStyle() As Integer
        With gridView
            .Columns("GROUPNAME").Visible = False
            .Columns("GGROUP").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("CATCODE").Visible = False
            .Columns("TRANNO").Visible = False
            With .Columns("CATNAME")
                .HeaderText = "DESCRIPTION"
                .Width = 250
                .Visible = True
            End With

            With .Columns("PCS")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If chkWithPcs.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("GRSWT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If rbtGrossWt.Checked = True Or rbtBoth.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("NETWT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If rbtNetWt.Checked = True Or rbtBoth.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("AMOUNT")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("VAT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TOTAL")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CUSTOMER")
                .Width = 200
            End With
            With .Columns("CASH")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CREDITCARD")
                .Width = 80
                .HeaderText = "CREDIT CARD"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CHEQUE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CHITCARD")
                .HeaderText = "SCHEME"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("DISCOUNT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("ROUND")
                .HeaderText = "ROUND OFF"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("HANDLING")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

    End Function

    Private Sub Report()
        Try
            If Not chkLstCashCounter.CheckedItems.Count > 0 Then chkCashCounterSelectAll.Checked = True
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
            If Not chkLstNodeId.CheckedItems.Count > 0 Then chkSystemIdSelectAll.Checked = True
            'Me.Cursor = Cursors.WaitCursor
            Dim SelectedCashCounter = GetChecked_CheckedList(chkLstCashCounter, False)
            Dim SelectedCostCentre = GetChecked_CheckedList(chkLstCostCentre, False)
            Dim SelectedSystemId = GetChecked_CheckedList(chkLstNodeId, False)
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            If chkLstCashCounter.CheckedItems.Count = chkLstCashCounter.Items.Count Then
                SelectedCashCounter += ",ALL"
            End If
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTRANSUMMARY','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTRANSUMMARY"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALES','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALES"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPPAYMENT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPAYMENT"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPRECEIPT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRECEIPT"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Dim GstFlag As Boolean = funcGstView(dtpFrom.Value)
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRANSACTIONSUMMARY"
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & SelectedCostCentre & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
            strSql += vbCrLf + " ,@SYSTEMID = '" & SelectedSystemId & "'"
            strSql += vbCrLf + " ,@CASHNAME = '" & SelectedCashCounter & "'"
            strSql += vbCrLf + " ,@CASHOPENING = '" & IIf(chkWithCashOpen.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + " ,@AR_OTHERCOSTCENTRE = '" & IIf(chkarothercostcenter.Checked = True, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                btnView_Search.Enabled = True
                MsgBox("Transaction Not Available", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim title As String
            title = "TRANSACTION SUMMARY FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If SelectedCostCentre <> "" Then title += " COSTCENTRE [" + SelectedCostCentre + "]"
            If SelectedCashCounter <> "" Then title += " CASHCOUNTER [" + SelectedCashCounter + "]"
            tabView.Show()
            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Visible = True
            gridView.DataSource = dt
            FillGridGroupStyle_KeyNoWise(gridView)
            BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
            With gridView
                .Columns("PARTICULAR").Width = 350
                .Columns("PCS").Width = 80
                .Columns("GRSWT").Width = 100
                .Columns("NETWT").Width = 100
                .Columns("AMOUNT").Width = 100
                .Columns("TAX").Width = 80
                .Columns("TOTAL").Width = 120

                If Chk_withdmdwt.Checked = False Then
                    .Columns("DIAWT").Visible = False
                End If

                .Columns("PCS").Visible = chkWithPcs.Checked
                If rbtGrossWt.Checked Or rbtBoth.Checked Then .Columns("GRSWT").Visible = True Else .Columns("GRSWT").Visible = False
                If rbtNetWt.Checked Or rbtBoth.Checked Then .Columns("NETWT").Visible = True Else .Columns("NETWT").Visible = False

                .Columns("COLHEAD").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("SYM").Visible = False
                If GstFlag Then
                    .Columns("ED").Visible = False
                    .Columns("TAX").HeaderText = "GST"
                End If
            End With


            If dt.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
            btnView_Search.Enabled = True
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            btnNew_Click(Me, New EventArgs)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Report()
        Prop_Sets()
    End Sub

    Private Sub frmTransactionDetailed_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmTransactionDetailed_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grp1.Location = New Point((ScreenWid - grp1.Width) / 2, ((ScreenHit - 128) - grp1.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If Authorize = False Then
            PnlFields.Enabled = False
            btnSave_OWN.Enabled = False
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        SepStnPost = funcGetValues("SEPSTNPOST", "N")
        SepDiaPost = funcGetValues("SEPDIAPOST", "N")
        SepPrePost = funcGetValues("SEPPREPOST", "N")
        'chkCompanySelectAll.Checked = False
        'chkCostCentreSelectAll.Checked = False
        'chkSystemIdSelectAll.Checked = False

        lblTitle.Visible = False
        lblTitle.Text = ""
        gridView.DataSource = Nothing
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        'rbtGrossWt.Checked = True
        'chkWithPcs.Checked = False
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    ''Private Sub gridview_cellformatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case "T"
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.Font = reportHeadStyle.Font
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.ForeColor = Color.Red
    ''        Case "G"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''    End Select
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("CATNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells("CATNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells("CATNAME").Style.ForeColor = Color.Red
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

    Function funcGridGroup(ByVal dt As DataTable, Optional ByVal SourceCol As Integer = 0, Optional ByVal DestCol As Integer = 1, Optional ByVal WithStoneField As Boolean = False, Optional ByVal sortFieldAlter As Boolean = False, Optional ByVal secondDestCol As Integer = -1) As DataSet
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
                If .Item(SourceCol).ToString <> "SUB TOTAL" And .Item(SourceCol).ToString <> "GRAND TOTAL" And .Item(DestCol).ToString <> "SUB TOTAL" And .Item(DestCol).ToString <> "GRAND TOTAL" And .Item(SourceCol).ToString <> "SALES DETAILS" And .Item(SourceCol).ToString <> "SALES[SA]" And .Item(SourceCol).ToString <> "SALES RETURN DETAILS" And .Item(SourceCol).ToString <> "SALES RETURN[SR]" And .Item(SourceCol).ToString <> "RECEIPT DETAILS" And .Item(SourceCol).ToString <> "RECEIPT[REC]" And .Item(SourceCol).ToString <> "PAYMENT DETAILS" And .Item(SourceCol).ToString <> "PAYMENTS[PAY]" And .Item(SourceCol).ToString <> "PURCHASE DETAILS" And .Item(SourceCol).ToString <> "PURCHASE[PUR]" And .Item(SourceCol).ToString <> "OTHERS" And .Item(SourceCol).ToString <> "DISCOUNT[DI]" And .Item(SourceCol).ToString <> "ROUND OFF[RO]" And .Item(SourceCol).ToString <> "HANDLING CHARGES[HC]" And .Item(SourceCol).ToString <> "TOTAL" And .Item(SourceCol).ToString <> "SA-SR+REC-PAY-PU-DI-RO+HC" And .Item(SourceCol).ToString <> "PAY COLLECTION" And .Item(SourceCol).ToString <> "CASH[CA]" And .Item(SourceCol).ToString <> "CREDIT CARD[CC]" And .Item(SourceCol).ToString <> "CHEQUE[CH]" And .Item(SourceCol).ToString <> "SAVINGS[SS]" And .Item(SourceCol).ToString <> "CA+CC+CH+SS" Then
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
                If .Item(SourceCol).ToString = "SUB TOTAL" Or .Item(DestCol).ToString = "SUB TOTAL" Or .Item(SourceCol).ToString = "SALES[SA]" Or .Item(SourceCol).ToString <> "SALES RETURN[SR]" Or .Item(SourceCol).ToString <> "RECEIPT[REC]" Or .Item(SourceCol).ToString <> "PAYMENTS[PAY]" Or .Item(SourceCol).ToString <> "PURCHASE[PUR]" Then
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus

        LoadCostName(chkLstCostCentre, False)
        ProcAddCashCounter()
        ProcAddNodeId()
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
    End Sub

    Private Sub ProcAddCashCounter()
        chkLstCashCounter.Items.Clear()
        strSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        da = New OleDbDataAdapter(strSql, cn)
        dtCashCounter = New DataTable
        da.Fill(dtCashCounter)
        If dtCashCounter.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
                chkLstCashCounter.Items.Add(dtCashCounter.Rows(cnt).Item(1).ToString)
            Next
        End If
    End Sub

    Private Sub ProcAddNodeId()
        Try

        
            chkLstNodeId.Items.Clear()
            strSql = "SELECT DISTINCT SYSTEMID FROM ( "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE UNION ALL "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  UNION ALL "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN )X "
            strSql += " ORDER BY SYSTEMID "
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    chkLstNodeId.Items.Add(dt.Rows(cnt).Item(0).ToString)
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkCashCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCashCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCashCounter, chkCashCounterSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkLstCashCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCashCounter.LostFocus
        If Not chkLstCashCounter.CheckedItems.Count > 0 Then
            chkCashCounterSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.LostFocus
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstNodeId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstNodeId.LostFocus
        If Not chkLstNodeId.CheckedItems.Count > 0 Then
            chkSystemIdSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkSystemIdSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSystemIdSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstNodeId, chkSystemIdSelectAll.Checked)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub



    Private Sub Prop_Sets()
        Dim obj As New TransactionSummary_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkSystemIdSelectAll = chkSystemIdSelectAll.Checked
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_chkCashCounterSelectAll = chkCashCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        obj.p_rbtGrossWt = rbtGrossWt.Checked
        obj.p_rbtNetWt = rbtNetWt.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_chkWithPcs = chkWithPcs.Checked
        obj.p_chkWithCashOpen = chkWithCashOpen.Checked
        obj.p_chkarothercostcenter = chkarothercostcenter.Checked
        SetSettingsObj(obj, Me.Name, GetType(TransactionSummary_Properties), Save)
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New TransactionSummary_Properties
        GetSettingsObj(obj, Me.Name, GetType(TransactionSummary_Properties), IIf(Authorize = False, True, False))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        chkSystemIdSelectAll.Checked = obj.p_chkSystemIdSelectAll
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, Nothing)
        chkCashCounterSelectAll.Checked = obj.p_chkCashCounterSelectAll
        SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, Nothing)
        rbtGrossWt.Checked = obj.p_rbtGrossWt
        rbtNetWt.Checked = obj.p_rbtNetWt
        rbtBoth.Checked = obj.p_rbtBoth
        chkWithPcs.Checked = obj.p_chkWithPcs
        chkWithCashOpen.Checked = obj.p_chkWithCashOpen
        chkarothercostcenter.Checked = obj.p_chkarothercostcenter
    End Sub

    Private Sub btnSave_OWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub
End Class


Public Class TransactionSummary_Properties

    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
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
    Private chkSystemIdSelectAll As Boolean = False
    Public Property p_chkSystemIdSelectAll() As Boolean
        Get
            Return chkSystemIdSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkSystemIdSelectAll = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private chkCashCounterSelectAll As Boolean = False
    Public Property p_chkCashCounterSelectAll() As Boolean
        Get
            Return chkCashCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCashCounterSelectAll = value
        End Set
    End Property
    Private chkWithCashOpen As Boolean = False
    Public Property p_chkWithCashOpen() As Boolean
        Get
            Return chkWithCashOpen
        End Get
        Set(ByVal value As Boolean)
            chkWithCashOpen = value
        End Set
    End Property
    Private chkarothercostcenter As Boolean = False
    Public Property P_chkarothercostcenter() As Boolean
        Get
            Return chkarothercostcenter
        End Get
        Set(ByVal value As Boolean)
            chkarothercostcenter = value
        End Set
    End Property
    Private chkLstCashCounter As New List(Of String)
    Public Property p_chkLstCashCounter() As List(Of String)
        Get
            Return chkLstCashCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCashCounter = value
        End Set
    End Property

    Private rbtGrossWt As Boolean = True
    Public Property p_rbtGrossWt() As Boolean
        Get
            Return rbtGrossWt
        End Get
        Set(ByVal value As Boolean)
            rbtGrossWt = value
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
    Private rbtBoth As Boolean = False
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private chkWithPcs As Boolean = False
    Public Property p_chkWithPcs() As Boolean
        Get
            Return chkWithPcs
        End Get
        Set(ByVal value As Boolean)
            chkWithPcs = value
        End Set
    End Property
End Class