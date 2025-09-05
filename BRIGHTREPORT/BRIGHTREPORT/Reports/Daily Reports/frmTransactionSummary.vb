Imports System.Data.OleDb
Public Class frmTransactionSummary
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim strFilter As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
    Dim SelectedCompany As String

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
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER , "
        Qry += " '1'RESULT ,'1'SALETYPE FROM   "
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
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO)"
        Qry += " ),'')AS CUSTOMER   "
        Qry += " FROM "
        Qry += " ( "
        Qry += " SELECT  CATCODE ,TRANNO ,ISSSNO ,BATCHNO ,TRANTYPE "
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME "
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID "
        Qry += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID   "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
        Qry += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME "
        Qry += " ,SUM(STNAMT)AMOUNT ,SUM(STNPCS)PCS "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'S' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT "
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT "
        Qry += " ,SUM(STNWT)WT"
        Qry += " FROM " & cnStockDb & ".." & StoneTable & " AS S WHERE TRANTYPE = '" & TrType & "' "
        Qry += " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
        Qry += " )AS X"
        Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,"
        Qry += " X.BATCHNO,X.TRANTYPE,X.PCS"
        Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,TRANTYPE,CUSTOMER "
        ''--DEDUCT SEP A/C AMT
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
        Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
        Qry += " ,SUM(VAT)VAT"
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER ,'1'RESULT ,'1'SALETYPE"
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
        Qry += " WHERE BATCHNO = X.BATCHNO AND COMPANYID IN (" & SelectedCompany & ") AND PAYMODE = '" & TaxType & "' AND TRANNO = X.TRANNO AND ACCODE "
        Qry += " = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT  "
        Qry += " ,SUM(X.STNWT)STNWT"
        Qry += " ,SUM(X.DIAWT)DIAWT"
        Qry += " ,I.TRANTYPE ,I.BATCHNO,'1'RESULT  "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO "
        Qry += " WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
        Qry += " FROM"
        Qry += " ("
        Qry += " SELECT "
        Qry += " CATCODE"
        Qry += " ,TRANNO"
        Qry += " ,ISSSNO"
        Qry += " ,BATCHNO"
        Qry += " ,TRANTYPE"
        Qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATNAME"
        Qry += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
        Qry += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALID  "
        Qry += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
        Qry += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID))AS METALNAME  "
        Qry += " ,SUM(STNAMT)AMOUNT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'D' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS STNWT"
        Qry += " ,CASE WHEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST "
        Qry += " WHERE ITEMID = S.STNITEMID) = 'T' THEN SUM(ISNULL(STNWT,0)) ELSE 0 END AS DIAWT"
        Qry += " FROM " & cnStockDb & ".." & StoneTable & " AS S WHERE TRANTYPE = '" & TrType & "' "
        Qry += " AND COMPANYID IN (" & SelectedCompany & ")"
        Qry += " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) "
        Qry += " = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
        Qry += " )AS X," & cnStockDb & ".." & TranTable & " AS I"
        Qry += strFilter
        Qry += " AND X.ISSSNO = I.SNO"
        Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,I.BATCHNO,"
        Qry += " X.BATCHNO,X.CATCODE,X.TRANNO"
        Qry += " )AS SEPDIA"
        Qry += " GROUP BY"
        Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE,CUSTOMER"
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
        ''strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO "
        ''strSql += "  AND COMPANYID IN (" & SelectedCompany & ") AND PAYMODE = '" & TaxType & "' AND TRANNO = X.TRANNO AND ACCODE = "
        ''strSql += " (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT"
        strSql += " , SUM(VAT) VAT "
        strSql += " ,SUM(AMOUNT)AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,CUSTOMER"
        strSql += " ,'1'RESULT"
        strSql += " ,'1'SALETYPE"
        strSql += " INTO TEMP" & systemId & "ABSTEMP"
        strSql += " FROM "
        strSql += " ("
        strSql += " SELECT"
        strSql += " CATCODE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strSql += " ,ITEMID"
        strSql += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
        strSql += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
        strSql += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        strSql += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID"
        strSql += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        strSql += " ,TRANDATE"
        strSql += " ,convert(varchar,TRANNO)TRANNO"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(TAX)VAT "
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
        strSql += " WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & ".." & TranTable & " WHERE "
        strSql += " BATCHNO = I.BATCHNO) AND STNITEMID IN (SELECT ITEMID "
        strSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
        strSql += " WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & ".." & TranTable & " WHERE "
        strSql += " BATCHNO = I.BATCHNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE DIASTONE = 'D')),0)DIAWT"
        strSql += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)"
        strSql += " ),'')AS CUSTOMER"
        strSql += " ,TRANTYPE,BATCHNO"
        strSql += " ,'1'RESULT"
        strSql += " FROM " & cnStockDb & ".." & TranTable & " AS I"
        strSql += strFilter
        strSql += " AND TRANTYPE = '" & TrType & "'"
        strSql += " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,TRANDATE"
        strSql += " )AS X"
        strSql += " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,MMONTHID,"
        strSql += " MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER"
        strSql += funcSepStnAc(TranTable, StoneTable, TrType, TaxType)
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = '" & TempTable & "')>0"
        strSql += " DROP TABLE " & TempTable & ""
        strSql += " SELECT "
        strSql += " CATCODE,'   '+CATNAME CATNAME,' 'ITEMID,METALID,METALNAME,' 'MMONTHID,' 'MMONTH,"
        strSql += " ' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(VAT)VAT"
        strSql += " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,' 'CUSTOMER"
        strSql += " ,'1'RESULT"
        strSql += " ,SALETYPE"
        strSql += " INTO " & TempTable & ""
        strSql += " FROM "
        strSql += " TEMP" & systemId & "ABSTEMP"
        strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,METALNAME,METALID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcFiltration() As Integer
        Dim tempChkItem As String = Nothing : Dim tmpcnt As Integer = 0
        strFilter = " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' AND COMPANYID IN (" & SelectedCompany & ")"

        ''COST CENTRE
        If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then
            strFilter += " AND COSTID = '" & dtCostCentre.Rows(cmbCostCentre.SelectedIndex - 1).Item(0).ToString() & "' "
        End If

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
        If chkLstNodeId.Items.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.GetItemChecked(CNT) = True Then
                        tempChkItem = tempChkItem & " '" & chkLstNodeId.Items.Item(CNT) & "'"
                        tmpcnt += 1
                        If tmpcnt < chkLstNodeId.CheckedItems.Count Then tempChkItem += ","
                    End If
                Next
            End If
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
        strSql += " ,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(VAT) VAT,"
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES'GROUPNAME"
        strSql += " ,'1'GGROUP"
        strSql += " ,'2'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SATRAN"
        strSql += " FROM TEMP" & systemId & "SALESABS"
        strSql += " GROUP BY CATCODE,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''--------------------------------------------------------------------
        ''--SA SUBTOTAL
        'strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        'strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SASUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SASUBTRAN"
        strSql += " SELECT "
        strSql += " 'S'CATCODE"
        strSql += " ,'SALES[SA]'CATNAME"
        strSql += " ,SUM(PCS) PCS,SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES'GROUPNAME"
        strSql += " ,'1'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SASUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT"
        strSql += " ,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
        strSql += " CONVERT(NUMERIC(15,2),VAT)VAT,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "SATRAN"
        strSql += " )X"
        'strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "SATRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'T'CATCODE,'SALES DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,'1'GGROUP,'1'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SATRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SATRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES RETURN'GROUPNAME"
        strSql += " ,'2'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SRTRAN"
        strSql += " FROM TEMP" & systemId & "SALESRETURNABS"
        strSql += " GROUP BY CATCODE,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------
        ''-SR SUBTOTAL
        'strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        'strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SRSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "SRSUBTRAN"
        strSql += " SELECT 'S'CATCODE,'SALES RETURN[SR]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'SALES RETURN'GROUPNAME"
        strSql += " ,'2'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SRSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT"
        strSql += " ,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
        strSql += " CONVERT(NUMERIC(15,2),VAT)VAT,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "SRTRAN"
        strSql += " )X"
        'strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "SRTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'S'CATCODE,'SALES RETURN DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,"
        strSql += " 0 VAT,'SALES'GROUPNAME,'2'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "SRTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'SALES RETURN'GROUPNAME,"
        strSql += " '2'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SRTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRTRAN)>0 "
        strSql += " BEGIN "
        strSql += "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SRSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "SRSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Private Function funcInsertChitCollection() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDB'"
        If (Trim(objGPack.GetSqlValue(strSql, , "N"))) = "Y" Then
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                strSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId
                strSql += "CHITCOLLECTION') DROP TABLE TEMP" & systemId & "CHITCOLLECTION "
                strSql += " SELECT  CASE "
                strSql += " WHEN MODEPAY = 'C' THEN 'CASH' "
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
                strSql += " GROUP BY MODEPAY"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                ''--SCHEME SUB TOTAL
                strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CHITCOLLECTIONTOT')>0"
                strSql += " DROP TABLE TEMP" & systemId & "CHITCOLLECTIONTOT"
                strSql += " SELECT "
                strSql += " 'T'CATCODE,'SCHEME[SCHEME]'CATNAME"
                strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT,"
                strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
                strSql += " ,'RECEIPT'GROUPNAME"
                strSql += " ,'9'GGROUP,'2'RESULT"
                strSql += " ,'S'COLHEAD"
                strSql += " INTO TEMP" & systemId & "CHITCOLLECTIONTOT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT CONVERT(INT,NULL)PCS,CONVERT(NUMERIC(15,3),NULL)GRSWT"
                strSql += " ,CONVERT(NUMERIC(15,3),NULL)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
                strSql += " CONVERT(NUMERIC(15,2),NULL)VAT FROM TEMP" & systemId & "CHITCOLLECTION"
                strSql += " )X"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "CHITCOLLECTION)>0 "
                strSql += " BEGIN "
                strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
                strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
                strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,'SCHEME'GROUPNAME,GGROUP,RESULT,COLHEAD"
                strSql += " FROM TEMP" & systemId & "CHITCOLLECTIONTOT "
                strSql += " UNION ALL"
                strSql += " SELECT ' 'CATCODE,CATNAME,NULL PCS,NULL GRSWT,NULL NETWT,AMOUNT,NULL VAT,AMOUNT TOTAL,'SCHEME'GROUPNAME,'19'GGROUP,'1'RESULT,' 'COLHEAD"
                strSql += " FROM TEMP" & systemId & "CHITCOLLECTION "
                strSql += " SELECT 'H'CATCODE,'SCHEME DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
                strSql += " 'SCHEME'GROUPNAME,'19'GGROUP,'0'RESULT,'T'"
                strSql += " ORDER BY RESULT "
                strSql += " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
    End Function

    Private Sub NewInsertReceipt()
        ''--RECEIPT
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECTRAN_VIEW')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "RECTRAN_VIEW"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CASE WHEN O.PAYMODE IN ('DR','DU') THEN 'DR'"
        strSql += vbCrLf + " WHEN O.PAYMODE = 'MR' THEN 'MR'"
        strSql += vbCrLf + " WHEN O.PAYMODE = 'OR' THEN 'OR'"
        strSql += vbCrLf + " ELSE 'AR' END AS PAYMODE "
        strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " INTO TEMP" & systemId & "RECTRAN_VIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += strFilter
        STRSQL += VBCRLF + "  AND RECPAY = 'R' GROUP BY PAYMODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECTRAN')>0"
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "RECTRAN"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  CATNAME"
        strSql += vbCrLf + "  ,SUM(AMOUNT) AMOUNT"
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)RESULT"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),'RECEIPT')GROUPNAME"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(2),'3')GGROUP"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "RECTRAN"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT 'CREDIT RECEIPT' AS CATNAME,AMOUNT FROM TEMP" & systemId & "RECTRAN_VIEW "
        strSql += vbCrLf + "  WHERE PAYMODE = 'DR'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'MISCELLANEOUS RECEIPT' AS CATNAME,AMOUNT FROM TEMP" & systemId & "RECTRAN_VIEW "
        strSql += vbCrLf + "  WHERE PAYMODE = 'MR'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'ORDER RECEIPT' AS CATNAME,AMOUNT FROM TEMP" & systemId & "RECTRAN_VIEW "
        strSql += vbCrLf + "  WHERE PAYMODE = 'OR'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'ADVANCE RECEIPT' AS CATNAME,AMOUNT FROM TEMP" & systemId & "RECTRAN_VIEW "
        strSql += vbCrLf + "  WHERE PAYMODE NOT IN ('DR','MR','OR')"
        strSql += vbCrLf + "  )X"
        strSql += vbCrLf + "  GROUP BY CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        STRSQL += VBCRLF + "  BEGIN "
        STRSQL += VBCRLF + "  INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += vbCrLf + "  (CATNAME,RESULT,COLHEAD,GGROUP,GROUPNAME)SELECT TOP 1 'RECEIPT DETAILS',0 RESULT,'T' COLHEAD,GGROUP,GROUPNAME FROM TEMP" & systemId & "RECTRAN"
        STRSQL += VBCRLF + "  INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += vbCrLf + "  (CATNAME,RESULT,COLHEAD,AMOUNT,GGROUP,GROUPNAME)"
        strSql += vbCrLf + "  SELECT 'RECEIPT [REC]',2 RESULT,'S' COLHEAD,SUM(AMOUNT),GGROUP,GROUPNAME FROM TEMP" & systemId & "RECTRAN GROUPBY GGROUP,GROUPNAME"
        STRSQL += VBCRLF + "  INSERT INTO TEMP" & systemId & "TRANSUMM"
        STRSQL += VBCRLF + "  (CATCODE,CATNAME,AMOUNT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += vbCrLf + "  SELECT NULL CATCODE,CATNAME,AMOUNT,AMOUNT,GROUPNAME,RESULT,COLHEAD"
        STRSQL += VBCRLF + "  FROM TEMP" & systemId & "RECTRAN "
        strSql += vbCrLf + "  ORDER BY RESULT "
        strSql += vbCrLf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Function funcInsertReceipt() As Integer
        ''--RECEIPT
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "RECTRAN"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'RECEIPT'GROUPNAME"
        strSql += " ,'3'GGROUP,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "RECTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'ADVANCE RECEIPT'CATNAME,'AR' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'AR' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' ' AS CATCODE,'MISCELLANEOUS RECEIPT'CATNAME,'MR' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'MR' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'GIFT VOUCHER RECEIPT'CATNAME,'GR' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'GR' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'DUE RECEIPT'CATNAME,'DR' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DR' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'TOBE RECEIPT'CATNAME,'TP' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'TP' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ORDER RECEIPT'CATNAME,'OR' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'OR' "
        strSql += " )X"
        strSql += " GROUP BY CATCODE,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------------
        ''--RECEIPT SUB TOTAL
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "RECSUBTRAN"
        strSql += " SELECT "
        strSql += " 'T'CATCODE,'RECEIPT[REC]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT,"
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'RECEIPT'GROUPNAME"
        strSql += " ,'3'GGROUP,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "RECSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT"
        strSql += " ,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
        strSql += " CONVERT(NUMERIC(15,2),VAT)VAT,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "RECTRAN"
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'RECEIPT DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,'3'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "RECTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'RECEIPT'GROUPNAME,"
        strSql += " '3'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "RECTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECTRAN)>0 "
        strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM TEMP" & systemId & "RECSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT, "
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PAYMENT'GROUPNAME"
        strSql += " ,'4'GGROUP,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PAYTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'ADVANCE PAYMENT'CATNAME,'AP' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'AP' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' ' AS CATCODE,'ADVANCE ISSUE'CATNAME,'AI' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'AI' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' ' AS CATCODE,'MISCELLANEOUS PAYMENT'CATNAME,'MP' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'MP' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'DUE PAYMENT'CATNAME,'DY' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DY' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'GIFT VOUCHER ISSUE'CATNAME,'GI' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'GI' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'ADVANCE ADJUSTED'CATNAME,'AA' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'AA' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " ' 'CATCODE,'CREDIT ADJUSTED'CATNAME,'DU' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DU' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'TOBE ADJUSTED'CATNAME,'TA' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'TA' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ORDER ADJUSTED'CATNAME,'OA' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'OA' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " ' 'CATCODE,'ORDER RE-PAYMENT'CATNAME,'OP' PAYMODE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,0 VAT,SUM(AMOUNT)TOTAL"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'OP' "
        ''strSql += " GROUP BY ACCODE,PAYMODE"
        strSql += " )X"
        strSql += " GROUP BY CATCODE,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------------
        ''--PAYMENT SUB TOTAL
        'strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        'strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PAYSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PAYSUBTRAN"
        strSql += " SELECT "
        strSql += " 'T'CATCODE,'PAYMENT[PAY]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT,"
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PAYMENT'GROUPNAME"
        strSql += " ,'4'GGROUP,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PAYSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT"
        strSql += " ,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
        strSql += " CONVERT(NUMERIC(15,2),VAT)VAT,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "PAYTRAN"
        strSql += " )X"
        'strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PAYMENT DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,'4'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PAYTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'PAYMENT'GROUPNAME,"
        strSql += " '4'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PAYTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYTRAN)>0 "
        strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PAYSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT,"
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PURCHASE'GROUPNAME"
        strSql += " ,'5'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PUTRAN"
        strSql += " FROM TEMP" & systemId & "PUTCHASEABS"
        strSql += " GROUP BY CATCODE,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ''----------------------------------------------------------------------------
        ''--PURCHASE SUBTOTAL
        'strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        'strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PUSUBTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "PUSUBTRAN"
        strSql += " SELECT 'T'CATCODE,'PURCHASE[PUR]'CATNAME"
        strSql += " , SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT, SUM(AMOUNT) AMOUNT, SUM(VAT) VAT,"
        strSql += " (SUM(AMOUNT)+SUM(VAT)) TOTAL"
        strSql += " ,'PURCHASE'GROUPNAME"
        strSql += " ,'5'GGROUP"
        strSql += " ,'4'RESULT"
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "PUSUBTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT CATCODE,CATNAME,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT"
        strSql += " ,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,"
        strSql += " CONVERT(NUMERIC(15,2),VAT)VAT,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "PUTRAN"
        strSql += " )X"
        'strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PUTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PURCHASE DETAILS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES'GROUPNAME,'5'GGROUP,'2'RESULT,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "PUTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'PURCHASE'GROUPNAME,"
        strSql += " '5'GGROUP,'1'RESULT,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PUTRAN "
        strSql += " ORDER BY RESULT "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUTRAN)>0 "
        strSql += " BEGIN "
        strSql += " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PUSUBTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
        strSql += " FROM TEMP" & systemId & "PUSUBTRAN "
        strSql += " END "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcOthers() As Integer
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "OTHERTRAN')>0"
        strSql += " DROP TABLE TEMP" & systemId & "OTHERTRAN"
        strSql += " SELECT "
        strSql += " CATCODE,CATNAME,PCS,GRSWT,NETWT"
        strSql += " ,AMOUNT,VAT,TOTAL"
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME,GGROUP,RESULT "
        strSql += " ,'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'DISCOUNT[DI]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
        strSql += " ,'OTHER'GROUPNAME,'6'GGROUP,'3'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'DI' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'ROUND OFF[RO]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
        strSql += " ,'OTHER'GROUPNAME,'6'GGROUP,'4'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += strFilter
        strSql += " AND PAYMODE = 'RO' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'T'CATCODE,'HANDLING CHARGES[HC]'CATNAME"
        strSql += " ,ISNULL(SUM(PCS),0)PCS,ISNULL(SUM(GRSWT),0)GRSWT,ISNULL(SUM(NETWT),0)NETWT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) "
        strSql += " ELSE -1*ISNULL(AMOUNT,0) END),0)AS TOTAL"
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
        strSql += " INSERT INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'OTHERS'GROUPNAME,"
        strSql += " '6'GGROUP,'1'RESULT,0,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "OTHERTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "OTHERTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'OTHERS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,'OTHERS'GROUPNAME,"
        strSql += " '6'GGROUP,'2'RESULT,0,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "OTHERTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME,GGROUP,RESULT "
        strSql += " ,'G'COLHEAD"
        strSql += " INTO TEMP" & systemId & "TOTTRAN"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'G'CATCODE"
        strSql += " ,'SA-SR+REC-PAY-PU-DI-RO+HC'CATNAME"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(INT,PCS) ELSE -1*CONVERT(INT,PCS) END) AS PCS"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(NUMERIC(15,3),GRSWT) ELSE -1*CONVERT(NUMERIC(15,3),GRSWT) END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(NUMERIC(15,3),NETWT) ELSE -1*CONVERT(NUMERIC(15,3),NETWT) END) AS NETWT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(NUMERIC(15,2),AMOUNT) ELSE -1*CONVERT(NUMERIC(15,2),AMOUNT) END) AS AMOUNT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(NUMERIC(15,2),VAT) ELSE -1*CONVERT(NUMERIC(15,2),VAT) END) AS VAT"
        strSql += " ,SUM(CASE WHEN CATNAME IN ('SA','REC','HANDLING CHARGES[HC]') "
        strSql += " THEN CONVERT(NUMERIC(15,2),TOTAL) ELSE -1*CONVERT(NUMERIC(15,2),TOTAL) END) AS TOTAL"
        strSql += " ,'GRAND TOTAL'GROUPNAME"
        strSql += " ,'7'GGROUP"
        strSql += " ,'3'RESULT"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT 'S'CATCODE,'SA'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,"
        strSql += " GGROUP,RESULT FROM TEMP" & systemId & "SASUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'SR'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,"
        strSql += " GGROUP,RESULT FROM TEMP" & systemId & "SRSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'REC'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,"
        strSql += " GGROUP,RESULT FROM TEMP" & systemId & "RECSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'PAY'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,"
        strSql += " GGROUP,RESULT FROM TEMP" & systemId & "PAYSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,'PU'CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,"
        strSql += " GGROUP,RESULT FROM TEMP" & systemId & "PUSUBTRAN"
        strSql += " UNION ALL"
        strSql += " SELECT 'S'CATCODE,CATNAME,PCS,GRSWT,NETWT,ISNULL(AMOUNT,0)AMOUNT,VAT,"
        strSql += " ISNULL(TOTAL,0)TOTAL,GROUPNAME,GGROUP,RESULT FROM TEMP" & systemId & "OTHERTRAN "
        strSql += " WHERE RESULT >=3"
        strSql += " )X"
        strSql += " )TOT"
        strSql += " ORDER BY GGROUP"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TOTTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,'7'GGROUP,'1'RESULT,0,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TOTTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'TOTAL'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,'7'GGROUP,'2'RESULT,0,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
        strSql += " ,CONVERT(VARCHAR(20),GROUPNAME)GROUPNAME,GGROUP,RESULT "
        strSql += " ,'S'COLHEAD"
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
        strSql += " AND PAYMODE = 'CA' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'CREDIT CARD[CC]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN "
        strSql += " ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'4'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'CC' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'CHEQUE[CH]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN "
        strSql += " ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'5'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'CH' "
        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'GIFTVOUCHER[GV]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN "
        strSql += " ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'5'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'GV' "

        strSql += " UNION ALL"
        strSql += " SELECT"
        strSql += " 'S'CATCODE,'SAVINGS[SS]'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN "
        strSql += " ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END),0) AS TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'6'RESULT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN "
        strSql += strFilter
        strSql += " AND PAYMODE = 'SS' "
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT"
        strSql += " 'G'CATCODE,'CA+CC+CH+SS'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT"
        strSql += " ,SUM(CONVERT(NUMERIC(15,2),ISNULL(TOTAL,0)))TOTAL"
        strSql += " ,'COL'GROUPNAME,'8'GGROUP,'7'RESULT,'G'"
        strSql += " FROM TEMP" & systemId & "COLTRAN"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT ' 'CATCODE,' 'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,'8'GGROUP,'1'RESULT,0,' '"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "COLTRAN"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,GROUPNAME,GGROUP,RESULT,TOTAL,COLHEAD)"
        strSql += " SELECT 'H'CATCODE,'PAY COLLECTION'CATNAME,0 PCS,0 GRSWT,0 NETWT,0 AMOUNT,0 VAT,"
        strSql += " 'SALES RETURN'GROUPNAME,'8'GGROUP,'2'RESULT,0,'T'"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "COLTRAN)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "TRANSUMM"
        strSql += " (CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD)"
        strSql += " SELECT CATCODE,CATNAME,PCS,GRSWT,NETWT,AMOUNT,VAT,TOTAL,GROUPNAME,GGROUP,RESULT,COLHEAD"
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
            With .Columns("CATNAME")
                .HeaderText = "DESCRIPTION"
                .Width = 350
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
        End With
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Try
            'Me.Cursor = Cursors.WaitCursor
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, True)

            strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
            strSql += " NAME = 'TEMP" & systemId & "TRANSUMM')"
            strSql += " DROP TABLE TEMP" & systemId & "TRANSUMM"
            strSql += " CREATE TABLE TEMP" & systemId & "TRANSUMM("
            strSql += " CATCODE VARCHAR(10),"
            strSql += " CATNAME VARCHAR(50),"
            strSql += " PCS INT, "
            strSql += " GRSWT NUMERIC(15,3),"
            strSql += " NETWT NUMERIC(15,3),"
            strSql += " AMOUNT NUMERIC(15,2),"
            strSql += " VAT NUMERIC(15,2),"
            strSql += " TOTAL NUMERIC(15,2),"
            strSql += " GROUPNAME VARCHAR(50),"
            strSql += " GGROUP VARCHAR(2),"
            strSql += " RESULT INT,"
            strSql += " COLHEAD VARCHAR(1),"
            strSql += " SNO INT IDENTITY)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            funcFiltration()
            strFtr1 = " and ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE " + strFilter + " AND COMPANYID IN (" & SelectedCompany & "))"
            strFtr2 = " and ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT " + strFilter + " AND COMPANYID IN (" & SelectedCompany & "))"
            btnView_Search.Enabled = False
            lblTitle.Visible = False
            gridView.DataSource = Nothing

            funcInsertSales()
            funcInsertSalesReturn()
            ' NewInsertReceipt()
            funcInsertReceipt()
            funcInsertPayment()
            funcInsertPurchase()
            'funcInsertChitCollection()
            funcOthers()

            strSql = " SELECT * FROM TEMP" & systemId & "SATRAN "
            strSql += " UNION ALL "
            strSql += " SELECT * FROM TEMP" & systemId & "SRTRAN "
            strSql += " UNION ALL "
            strSql += " SELECT * FROM TEMP" & systemId & "RECTRAN "
            strSql += " UNION ALL "
            strSql += " SELECT * FROM TEMP" & systemId & "PAYTRAN "
            strSql += " UNION ALL "
            strSql += " SELECT * FROM TEMP" & systemId & "PUTRAN "
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                funcInsertTotal()
                funcInsertCollection()
            End If

            strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TRANSUMM)>0 "
            strSql += " BEGIN "
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET PCS = NULL WHERE ISNULL(PCS,0) = 0"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET GRSWT = NULL WHERE ISNULL(GRSWT,0) = 0"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET NETWT = NULL WHERE ISNULL(NETWT,0) = 0"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = NULL WHERE ISNULL(AMOUNT,0) = 0"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET VAT = NULL WHERE ISNULL(VAT,0) = 0"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET TOTAL = NULL WHERE ISNULL(TOTAL,0) = 0"
            strSql += " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TRANSUMM)>0 "
            strSql += " BEGIN "
            strSql += " ALTER TABLE TEMP" & systemId & "TRANSUMM ALTER COLUMN AMOUNT VARCHAR(20)"
            strSql += " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "TRANSUMM)>0 "
            strSql += " BEGIN "
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ADVANCE RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'DUE RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'GIFT VOUCHER RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'MISCELLANEOUS RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ORDER RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'TOBE RECEIPT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ADVANCE ADJUSTED' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ADVANCE ISSUE' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ADVANCE PAYMENT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ADVANCE ADJUSTED' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'CREDIT ADJUSTED' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'DUE PAYMENT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'GIFT VOUCHER ISSUE' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'MISCELLANEOUS PAYMENT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ORDER ADJUSTED' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'ORDER RE-PAYMENT' AND ISNULL(AMOUNT,'') = ''"
            strSql += " UPDATE TEMP" & systemId & "TRANSUMM SET AMOUNT = '--NIL--' WHERE ISNULL(CATNAME,'') = 'TOBE ADJUSTED' AND ISNULL(AMOUNT,'') = ''"
            strSql += " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "SELECT * FROM TEMP" & systemId & "TRANSUMM ORDER BY SNO "
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                btnView_Search.Enabled = True
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim title As String
            title = "TRANSACTION SUMMURY FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If cmbCostCentre.Text <> "" Then
                title += " COSTCENTRE [" + cmbCostCentre.Text + "]"
            End If
            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Visible = True
            gridView.DataSource = dt
            tabView.Show()
            GridViewFormat()
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            funcGridViewStyle()
            gridView.Focus()
            btnView_Search.Enabled = True
            If dt.Rows.Count > 0 Then tabMain.SelectedTab = tabView : gridView.Focus()
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        SepStnPost = funcGetValues("SEPSTNPOST", "N")
        SepDiaPost = funcGetValues("SEPDIAPOST", "N")
        SepPrePost = funcGetValues("SEPPREPOST", "N")
        chkCompanySelectAll.Checked = False
        lblTitle.Visible = False
        lblTitle.Text = ""
        cmbCostCentre.Text = ""
        gridView.DataSource = Nothing
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtGrossWt.Checked = True
        chkWithPcs.Checked = False
        btnView_Search.Enabled = True
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmTransactionSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
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
        ProcAddCostCentre()
        ProcAddCashCounter()
        ProcAddNodeId()
    End Sub

    Private Sub ProcAddCostCentre()
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                cmbCostCentre.Items.Clear()
                cmbCostCentre.Items.Add("ALL")
                For cnt As Integer = 0 To dtCostCentre.Rows.Count - 1
                    cmbCostCentre.Items.Add(dtCostCentre.Rows(cnt).Item(1).ToString)
                Next
            End If
            cmbCostCentre.Text = "ALL"
            Exit Sub
        End If
        cmbCostCentre.Text = "" : cmbCostCentre.Items.Clear() : cmbCostCentre.Enabled = False
    End Sub

    Private Sub ProcAddCashCounter()
        strSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHID"
        da = New OleDbDataAdapter(strSql, cn)
        dtCashCounter = New DataTable
        da.Fill(dtCashCounter)
        If dtCashCounter.Rows.Count > 0 Then
            chkLstCashCounter.Items.Add("ALL", True)
            For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
                chkLstCashCounter.Items.Add(dtCashCounter.Rows(cnt).Item(1).ToString)
            Next
        Else
            chkLstCashCounter.Items.Clear()
            chkLstCashCounter.Enabled = False
        End If
    End Sub

    Private Sub ProcAddNodeId()
        strSql = "SELECT DISTINCT SYSTEMID FROM ( "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE UNION ALL "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  UNION ALL "
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN )X "
        strSql += " ORDER BY SYSTEMID "
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL", True)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(cnt).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    Private Sub frmTransactionSummary_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        grp1.Location = New Point((ScreenWid - grp1.Width) / 2, ((ScreenHit - 128) - grp1.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
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
End Class
