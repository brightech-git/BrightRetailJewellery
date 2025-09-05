Imports System.Data.OleDb
Public Class frmSalesReturnAbstract
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
        strSql += " where ctlId = '" & field & "'"
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
        ''--GENERATE SEP A/C
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME"
        Qry += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID   "
        Qry += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH   "
        Qry += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        Qry += " ,TRANDATE,TRANNO,TRANTYPE  "
        Qry += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT  ,SUM(VAT)VAT "
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL  ,SUM(STNWT)STNWT ,SUM(DIAWT)DIAWT ,CUSTOMER ,'1'RESULT ,'1'SALETYPE  "
        Qry += " FROM   "
        Qry += " ("
        Qry += " SELECT X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO "
        Qry += " ,(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO = ISSSNO AND COMPANYID = '" & strCompanyId & "')TRANDATE"
        Qry += " ,SUM(X.AMOUNT)AMOUNT "
        Qry += " ,ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND PAYMODE = 'PV' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT   "
        Qry += " ,SUM(X.PCS)PCS ,SUM(X.WT)AS GRSWT ,SUM(X.WT)AS NETWT ,SUM(X.STNWT)STNWT "
        Qry += " ,SUM(X.DIAWT)DIAWT ,X.TRANTYPE ,X.BATCHNO,'1'RESULT   "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO)),'')AS CUSTOMER   "
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
        Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND COMPANYID = '" & strCompanyId & "' AND TRANTYPE = 'SR' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE "
        Qry += " )AS X"
        Qry += " GROUP BY X.CATCODE,X.CATNAME,X.TRANNO,X.METALID,X.METALNAME,X.ISSSNO,X.BATCHNO,X.TRANTYPE,X.PCS"
        Qry += " )AS SEPDIA GROUP BY CATCODE,CATNAME,METALID,METALNAME,TRANDATE,TRANNO,TRANTYPE,CUSTOMER "
        ''--DEDUCT SEP A/C AMT
        Qry += " UNION ALL"
        Qry += " SELECT CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE "
        Qry += " ,0 PCS,0 GRSWT,0 NETWT,SUM(AMOUNT)AMOUNT "
        Qry += " ,SUM(VAT)VAT"
        Qry += " ,SUM(AMOUNT)+SUM(VAT)AS TOTAL ,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT,CUSTOMER ,'1'RESULT ,'1'SALETYPE "
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
        Qry += " ,ISNULL((SELECT -1*AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND PAYMODE = 'PV' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT  "
        Qry += " ,SUM(X.STNWT)STNWT"
        Qry += " ,SUM(X.DIAWT)DIAWT"
        Qry += " ,I.TRANTYPE ,I.BATCHNO,'1'RESULT  "
        Qry += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER  "
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
        Qry += " FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE 1=1 AND COMPANYID = '" & strCompanyId & "' AND TRANTYPE = 'SR' AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) = '" & diaType & "' "
        Qry += strFtr1
        Qry += " GROUP BY CATCODE,TRANNO,STNITEMID,ISSSNO,BATCHNO,TRANTYPE"
        Qry += " )AS X," & cnStockDb & "..RECEIPT AS I"
        Qry += strFtr
        Qry += " AND X.ISSSNO = I.SNO"
        Qry += " GROUP BY I.TRANDATE,I.SNO,I.CATCODE,I.TRANNO,I.ITEMID,I.TRANTYPE,I.BATCHNO,X.BATCHNO,X.CATCODE,X.TRANNO"
        Qry += " )AS SEPDIA"
        Qry += " GROUP BY"
        Qry += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE,CUSTOMER"
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
        strSql += " DROP TABLE TEMP" & systemId & "ABSTEMP"
        strSql += " SELECT"
        strSql += " CATCODE,CATNAME,METALID,METALNAME,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANNO,TRANTYPE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = X.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND PAYMODE = 'PV' AND TRANNO = X.TRANNO AND ACCODE = (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)),0)AS VAT"
        strSql += " ,SUM(AMOUNT)AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,CUSTOMER"
        strSql += " ,'1'RESULT"
        strSql += " ,'1'SALETYPE"
        strSql += " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strSql += " INTO TEMP" & systemId & "ABSTEMP"
        strSql += " FROM "
        strSql += " ("
        strSql += " SELECT"
        strSql += " CATCODE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strSql += " ,ITEMID"
        strSql += " ,(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALID"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))AS METALNAME"
        strSql += " ,CONVERT(INT,CONVERT(VARCHAR,DATEPART(YEAR,TRANDATE))+REPLICATE('0',2-LEN((DATEPART(MONTH,TRANDATE)))) + CONVERT(VARCHAR,DATEPART(MONTH,TRANDATE)))AS MMONTHID"
        'strSql += " ,DATEPART(MONTH,TRANDATE)AS MMONTHID"
        strSql += " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += " ,CONVERT(VARCHAR,TRANDATE,103)AS UUPDATED"
        strSql += " ,TRANDATE"
        strSql += " ,convert(varchar,TRANNO)TRANNO"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)STNWT"
        strSql += " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND COMPANYID = '" & strCompanyId & "') AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)DIAWT"
        strSql += " ,ISNULL((SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)),'')AS CUSTOMER"
        strSql += " ,TRANTYPE,BATCHNO"
        strSql += " ,'1'RESULT"
        strSql += " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += strFtr
        strSql += " AND TRANTYPE = 'SR'"
        strSql += " GROUP BY TRANNO,CATCODE,ITEMID,TRANDATE,BATCHNO,TRANTYPE,TRANDATE"
        strSql += " )AS X"
        strSql += " GROUP BY CATCODE,CATNAME,METALID,METALNAME,BATCHNO,TRANNO,MMONTHID,MMONTH,UUPDATED,TRANDATE,TRANTYPE,CUSTOMER"
        strSql += funcSepStnAc()
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABS')>0"
        strSql += " DROP TABLE TEMP" & systemId & "ABS"
        strSql += " SELECT "
        If rbtSummary.Checked = True Then
            strSql += " CATCODE,'   '+CATNAME CATNAME,' 'ITEMID,METALID,METALNAME,' 'MMONTHID,' 'MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtMonth.Checked = True Then
            strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,MMONTHID,'   '+MMONTH MMONTH,' 'UUPDATED,' 'TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtDate.Checked = True Then
            strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,'   '+UUPDATED UUPDATED,TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtBillNo.Checked = True Then
            strSql += " CATCODE,CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME,' 'MMONTHID,' 'MMONTH,UUPDATED,TRANDATE,'   '+TRANNO TRANNO,TRANTYPE"
        End If
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(VAT)VAT"
        strSql += " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        If rbtBillNo.Checked = True Then
            strSql += " ,CUSTOMER"
        Else
            strSql += " ,' 'CUSTOMER"
        End If
        strSql += " ,'1'RESULT"
        strSql += " ,SALETYPE"
        strSql += " ,CONVERT(VARCHAR(1),'') COLHEAD"
        strSql += " INTO TEMP" & systemId & "ABS"
        strSql += " FROM "
        strSql += " TEMP" & systemId & "ABSTEMP"
        'strSql += " AS Z"
        If rbtSummary.Checked = True Then
            strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,METALNAME,METALID"
        ElseIf rbtMonth.Checked = True Then
            strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,MMONTHID,MMONTH"
        ElseIf rbtDate.Checked = True Then
            strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,UUPDATED,TRANDATE"
        ElseIf rbtBillNo.Checked = True Then
            strSql += " GROUP BY SALETYPE,CATCODE,CATNAME,TRANNO,CUSTOMER,TRANTYPE,UUPDATED,TRANDATE"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcAbsSub() As Integer
        ''SUMMARY WISE SUBTOTAL
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSSUB')>0"
        strSql += " DROP TABLE TEMP" & systemId & "ABSSUB"
        strSql += " SELECT"
        If rbtSummary.Checked = True Then
            strSql += " ' ' CATCODE,' 'CATNAME,' 'ITEMID,METALID,'SUB TOTAL'METALNAME"
            strSql += " ,' 'MMONTHID"
            strSql += " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtMonth.Checked = True Then
            strSql += " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
            strSql += " ,' 'MMONTHID"
            strSql += " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtDate.Checked = True Then
            strSql += " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
            strSql += " ,' 'MMONTHID"
            strSql += " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        ElseIf rbtBillNo.Checked = True Then
            strSql += " CATCODE,'SUB TOTAL'CATNAME,' 'ITEMID,' 'METALID,' 'METALNAME"
            strSql += " ,' 'MMONTHID"
            strSql += " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        End If
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(VAT)VAT"
        strSql += " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,' 'CUSTOMER"
        strSql += " ,'2'RESULT"
        strSql += " ,' 'SALETYPE"
        strSql += " ,CONVERT(VARCHAR(1),'S') COLHEAD"
        strSql += " INTO TEMP" & systemId & "ABSSUB"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT * FROM TEMP" & systemId & "ABS"
        strSql += " )AS Z"
        If rbtSummary.Checked = True Then
            ''strSql += " GROUP BY CATCODE,METALNAME,METALID"
            strSql += " GROUP BY METALID"
        ElseIf rbtMonth.Checked = True Then
            strSql += " GROUP BY CATCODE"
        ElseIf rbtDate.Checked = True Then
            strSql += " GROUP BY CATCODE"
        ElseIf rbtBillNo.Checked = True Then
            strSql += " GROUP BY CATCODE"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcAbsGrand() As Integer
        ''SUMMARY WISE SUBTOTAL
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ABSGRAND')>0"
        strSql += " DROP TABLE TEMP" & systemId & "ABSGRAND"
        strSql += " SELECT"
        strSql += " 'ZZZZ'CATCODE,'GRAND TOTAL'CATNAME,' 'ITEMID,'ZZZZ'METALID,'ZZZZ'METALNAME"
        strSql += " ,' 'MMONTHID"
        strSql += " ,' 'MMONTH,' 'UUPDATED,NULL TRANDATE,' 'TRANNO,' 'TRANTYPE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,SUM(VAT)VAT"
        strSql += " ,SUM(AMOUNT)+SUM(VAT) AS TOTAL"
        strSql += " ,SUM(STNWT)STNWT"
        strSql += " ,SUM(DIAWT)DIAWT"
        strSql += " ,' 'CUSTOMER"
        strSql += " ,'3'RESULT"
        strSql += " ,' 'SALETYPE"
        strSql += " ,CONVERT(VARCHAR(1),'G') COLHEAD"
        strSql += " INTO TEMP" & systemId & "ABSGRAND"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT * FROM TEMP" & systemId & "ABSSUB"
        strSql += " )AS Z"
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
            .Columns("TRANTYPE").Visible = False

            For cnt As Integer = 0 To gridView.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("UUPDATED").Visible = rbtBillNo.Checked
            .Columns("UUPDATED").HeaderText = "TRANDATE"
            With .Columns("PARTICULAR")
                .Width = 250
                If rbtSummary.Checked = True Then
                    .HeaderText = "CATEGORY"
                ElseIf rbtMonth.Checked = True Then
                    .HeaderText = "MONTH"
                ElseIf rbtDate.Checked = True Then
                    .HeaderText = "DATE"
                Else
                    .HeaderText = "BILL NO"
                End If
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
                .Visible = rbtBillNo.Checked
            End With
            With .Columns("ADDRESS")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = rbtBillNo.Checked
            End With
            With .Columns("PHONENO")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = rbtBillNo.Checked
            End With
        End With
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function

    Function funcFiltration() As String
        Dim Qry As String = Nothing
        'Qry = " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        'If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then
        '    Qry += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        'End If
        'Qry += " AND ISNULL(CANCEL,'') <> 'Y'"
        'If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
        '    Qry += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        'End If
        'Qry += " AND COMPANYID = '" & strCompanyId & "' "
        Return Qry
    End Function

    Private Sub ReturnAbs()
        Prop_Sets()
        gridView.DataSource = Nothing
        Me.Refresh()
        Dim selCatCode As String = Nothing
        If cmbCategory.Text = "ALL" Then
            selCatCode = "ALL"
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(cmbCategory.Text) & ")"
            Dim dtCat As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCat)
            If dtCat.Rows.Count > 0 Then
                For i As Integer = 0 To dtCat.Rows.Count - 1
                    selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
                Next
                If selCatCode <> "" Then
                    selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                End If
            End If
        End If
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_ABSTRACT_SASRPU"
        strSql += vbCrLf + " @SUMMARYWISE = '" & IIf(rbtSummary.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@MONTHWISE = '" & IIf(rbtMonth.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@DATEWISE = '" & IIf(rbtDate.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@TRANNOWISE = '" & IIf(rbtBillNo.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@WITHSR = 'N'"
        strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@RPTTYPE = 'SR'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
        strSql += vbCrLf + " ,@CATNAME = ''"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@VA = '" & IIf(chkVA.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@CHITDBID = '" & cnChitCompanyid & "'"
        strSql += vbCrLf + " ,@PURTYPE='SR'"
        strSql += vbCrLf + " ,@WITSTN = 'N'"
        strSql += vbCrLf + " ,@PUREMC = 'N'"
        strSql += vbCrLf + " ,@WITHPR = 'N'"
        strSql += vbCrLf + " ,@WITHTRANNO = 'N'"
        strSql += vbCrLf + " ,@AFTERDISC = 'N'"
        strSql += vbCrLf + " ,@WITHITEM = 'N'"
        strSql += vbCrLf + " ,@STKTYPE = 'A'"
        strSql += vbCrLf + " ,@BILLPREFIX = 'N'"
        strSql += vbCrLf + " ,@DBNAME = '" & cnStockDb & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET PCS = NULL WHERE PCS = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET GRSWT = NULL WHERE GRSWT = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET NETWT = NULL WHERE NETWT = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMOUNT = NULL WHERE AMOUNT = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TAX = NULL WHERE TAX = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TOTAL = NULL WHERE TOTAL = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET STNCT = NULL WHERE STNCT = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET STNGT = NULL WHERE STNGT = 0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET DIAWT = NULL WHERE DIAWT = 0"
        If rbtBillNo.Checked And chkVA.Checked Then
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMT = NULL WHERE AMT = 0"
            strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET CREDIT = NULL WHERE CREDIT = 0"
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
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        With gridView
            If .Columns.Contains("TINNO") Then .Columns("TINNO").Visible = False
            If .Columns.Contains("ITEMID") Then .Columns("ITEMID").Visible = False
            If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            If .Columns.Contains("COSTNAME") Then .Columns("COSTNAME").Visible = False
            If .Columns.Contains("COSTID") Then .Columns("COSTID").Visible = False
            If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("MMONTHID").Visible = False
            .Columns("MMONTHNAME").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("TRANNO").Visible = rbtBillNo.Checked
            .Columns("METALNAME").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("CUSTOMER").Visible = rbtBillNo.Checked
            .Columns("ADDRESS").Visible = rbtBillNo.Checked And Not chkVA.Checked
            .Columns("PHONENO").Visible = rbtBillNo.Checked And Not chkVA.Checked
            .Columns("PCS").Visible = chkPcs.Checked
            .Columns("GRSWT").Visible = chkGrsWt.Checked
            .Columns("NETWT").Visible = chkNetWt.Checked
            .Columns("ADDRESS1").Visible = False
            .Columns("ADDRESS2").Visible = False
            .Columns("ADDRESS3").Visible = False
            .Columns("AREA").Visible = False
            .Columns("CITY").Visible = False
            .Columns("BILLNO").Visible = rbtBillNo.Checked
            .Columns("BILLDATE").Visible = rbtBillNo.Checked

            .Columns("PARTICULAR").Width = 250
            .Columns("TRANNO").Width = 70
            .Columns("BILLNO").Width = 70
            .Columns("BILLDATE").Width = 100
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("AMOUNT").Width = 100
            .Columns("TAX").Width = 80
            .Columns("TOTAL").Width = 100
            .Columns("STNCT").Width = 75
            .Columns("STNGT").Width = 75
            .Columns("DIAWT").Width = 75
            .Columns("CUSTOMER").Width = 120
            .Columns("ADDRESS").Width = 150
            .Columns("PHONENO").Width = 100

            .Columns("SC").Width = 80
            .Columns("SC").HeaderText = "CESS"
            .Columns("BILLNO").HeaderText = "SALES" & vbCrLf & " BILLNO"
            .Columns("BILLDATE").HeaderText = "SALES" & vbCrLf & " BILLDATE"



            ' .Columns("CUSTOMER").Visible = rbtBillNo.Checked
        End With
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView)



        Dim TITLE As String
        If rbtSummary.Checked = True Then
            TITLE = "SUMMARY WISE"
        ElseIf rbtMonth.Checked = True Then
            TITLE = "MONTH WISE"
        ElseIf rbtDate.Checked = True Then
            TITLE = "DATE WISE"
        Else
            TITLE = "BILLNO WISE"
        End If
        TITLE += " SALES RETURN ABSTRACT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
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
        pnlHeading.Visible = False
        ReturnAbs()
        Exit Sub
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
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case dgvRow.Cells("COLHEAD").Value.ToString
                    Case "S"
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        dgvRow.Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        dgvRow.Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        dgvRow.DefaultCellStyle.Font = reportTotalStyle.Font
                        dgvRow.DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub frmSalesReturnAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)

        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
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
        'chkPcs.Checked = True
        'chkGrsWt.Checked = True
        'chkNetWt.Checked = True
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
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
        Dim obj As New frmSalesReturnAbstract_Properties
        obj.p_chkPcs = chkPcs.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtMonth = rbtMonth.Checked
        obj.p_rbtDate = rbtDate.Checked
        obj.p_rbtBillNo = rbtBillNo.Checked
        obj.p_chkVA = chkVA.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSalesReturnAbstract_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesReturnAbstract_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesReturnAbstract_Properties))
        chkPcs.Checked = obj.p_chkPcs
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        rbtSummary.Checked = obj.p_rbtSummary
        rbtMonth.Checked = obj.p_rbtMonth
        rbtDate.Checked = obj.p_rbtDate
        rbtBillNo.Checked = obj.p_rbtBillNo
        chkVA.Checked = obj.p_chkVA
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
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
End Class

Public Class frmSalesReturnAbstract_Properties
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