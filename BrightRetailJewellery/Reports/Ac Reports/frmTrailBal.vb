Imports System.Data.OleDb

Public Class frmTrailBal
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim Specificformat As Boolean = False
    Dim viewtype As String = ""
    Dim dumpDt As New DataTable
    Dim TrailBalGrpWise As Boolean = IIf(GetAdmindbSoftValue("REP_TRAILBAL_GRPWISE", "N") = "Y", True, False)

    Function funcSubTotal() As Integer
        'SUBTOTAL
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SUBTRAILBAL') > 0"
        strSql += " DROP TABLE TEMP" & systemId & "SUBTRAILBAL"
        strSql += " DECLARE @FROMDATE AS SMALLDATETIME"
        strSql += " DECLARE @TODATE AS SMALLDATETIME"
        If chkAsonDate.Checked = True Then
            strSql += " SELECT @FROMDATE = '" & cnTranFromDate & "'"
            strSql += " SELECT @TODATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += " SELECT @FROMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += " SELECT "
        strSql += " ACCODE"
        strSql += " ,ACGRPCODE"
        strSql += " ,ACGRPNAME"
        strSql += " ,ACGRPNAME TEMPACGRPNAME"
        strSql += " ,ACNAME"
        strSql += " ,CASE WHEN (ODEBIT - OCREDIT) > 0 THEN (ODEBIT - OCREDIT) ELSE 0 END AS ODEBIT"
        strSql += " ,CASE WHEN (OCREDIT - ODEBIT) > 0 THEN (OCREDIT - ODEBIT) ELSE 0 END AS OCREDIT"
        strSql += " ,TDEBIT"
        strSql += " ,TCREDIT"
        strSql += " ,CASE WHEN (CDEBIT - CCREDIT) > 0 THEN (CDEBIT - CCREDIT) ELSE 0 END AS CDEBIT"
        strSql += " ,CASE WHEN (CCREDIT - CDEBIT) > 0 THEN (CCREDIT - CDEBIT) ELSE 0 END AS CCREDIT"
        strSql += " ,'2'RESULT, 'S'COLHEAD"
        strSql += " INTO TEMP" & systemId & "SUBTRAILBAL"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " ACCODE"
        strSql += " ,ACGRPCODE"
        strSql += " ,ACGRPNAME"
        strSql += " ,TEMPACGRPNAME"
        strSql += " ,ACNAME"
        strSql += " ,SUM(CASE WHEN SEP = 'OPENING' THEN DEBIT ELSE 0 END)AS ODEBIT"
        strSql += " ,SUM(CASE WHEN SEP = 'OPENING' THEN CREDIT ELSE 0 END)AS OCREDIT"
        strSql += " ,SUM(CASE WHEN SEP = 'TRAN' THEN DEBIT ELSE 0 END)AS TDEBIT"
        strSql += " ,SUM(CASE WHEN SEP = 'TRAN' THEN CREDIT ELSE 0 END)AS TCREDIT"
        strSql += " ,SUM(DEBIT)CDEBIT"
        strSql += " ,SUM(CREDIT)CCREDIT"
        strSql += " ,'2'RESULT, 'S'COLHEAD"
        strSql += " FROM "
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'OPENING' AS SEP"
        strSql += " ,' 'ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,'ZZZZSUB TOTAL' AS TEMPACGRPNAME "
        strSql += " ,' 'ACNAME"
        strSql += " ,SUM(DEBIT)AS DEBIT"
        strSql += " ,SUM(CREDIT)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..OPENTRAILBALANCE AS T"
        strSql += " WHERE 1=1"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " 'OPENING' AS SEP"
        strSql += " ,' 'ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,'ZZZZSUB TOTAL' AS TEMPACGRPNAME ,''ACNAME"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END)AS DEBIT"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE < @FROMDATE"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " 'TRAN' AS SEP"
        strSql += " ,' 'ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,'ZZZZSUB TOTAL' AS TEMPACGRPNAME "
        strSql += " ,' 'ACNAME"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END)AS DEBIT"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE BETWEEN @FROMDATE AND @TODATE"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " )AS X"
        strSql += " GROUP BY ACCODE,ACGRPCODE,ACGRPNAME,TEMPACGRPNAME,ACNAME"
        strSql += " )AS Y"
        strSql += " ORDER BY ACGRPCODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function funcFiltration() As String
        Dim qry As String = Nothing
        qry += " AND COMPANYID = '" & strCompanyId & "' "
        Return qry
    End Function
    Function funcTrialBal() As Integer
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TRAILBAL') > 0"
        strSql += " DROP TABLE TEMP" & systemId & "TRAILBAL"
        strSql += " DECLARE @FROMDATE AS SMALLDATETIME"
        strSql += " DECLARE @TODATE AS SMALLDATETIME"
        If chkAsonDate.Checked = True Then
            strSql += " SELECT @FROMDATE = '" & cnTranFromDate & "'"
            strSql += " SELECT @TODATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += " SELECT @FROMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += " SELECT "
        strSql += " ACCODE"
        strSql += " ,ACGRPCODE"
        strSql += " ,ACGRPNAME"
        strSql += " ,TEMPACGRPNAME"
        strSql += " ,ACNAME"
        strSql += " ,CASE WHEN (ODEBIT - OCREDIT) > 0 THEN (ODEBIT - OCREDIT) ELSE 0 END AS ODEBIT"
        strSql += " ,CASE WHEN (OCREDIT - ODEBIT) > 0 THEN (OCREDIT - ODEBIT) ELSE 0 END AS OCREDIT"
        strSql += " ,TDEBIT"
        strSql += " ,TCREDIT"
        strSql += " ,CASE WHEN (CDEBIT - CCREDIT) > 0 THEN (CDEBIT - CCREDIT) ELSE 0 END AS CDEBIT"
        strSql += " ,CASE WHEN (CCREDIT - CDEBIT) > 0 THEN (CCREDIT - CDEBIT) ELSE 0 END AS CCREDIT"
        strSql += " ,'1'RESULT, ' 'COLHEAD"
        strSql += " INTO TEMP" & systemId & "TRAILBAL"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " ACCODE"
        strSql += " ,ACGRPCODE"
        strSql += " ,ACGRPNAME"
        strSql += " ,TEMPACGRPNAME"
        strSql += " ,ACNAME"
        strSql += " ,SUM(CASE WHEN SEP = 'OPENING' THEN DEBIT ELSE 0 END)AS ODEBIT"
        strSql += " ,SUM(CASE WHEN SEP = 'OPENING' THEN CREDIT ELSE 0 END)AS OCREDIT"
        strSql += " ,SUM(CASE WHEN SEP = 'TRAN' THEN DEBIT ELSE 0 END)AS TDEBIT"
        strSql += " ,SUM(CASE WHEN SEP = 'TRAN' THEN CREDIT ELSE 0 END)AS TCREDIT"
        strSql += " ,SUM(DEBIT) CDEBIT"
        strSql += " ,SUM(CREDIT)CCREDIT"
        strSql += " ,'1'RESULT, ' 'COLHEAD"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " 'OPENING' AS SEP"
        strSql += " ,ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS TEMPACGRPNAME "
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)ACNAME"
        strSql += " ,SUM(DEBIT)AS DEBIT"
        strSql += " ,SUM(CREDIT)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..OPENTRAILBALANCE AS T"
        strSql += " WHERE 1=1"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " 'OPENING' AS SEP"
        strSql += " ,ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS TEMPACGRPNAME "
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)ACNAME"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END)AS DEBIT"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE < @FROMDATE"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " 'TRAN' AS SEP"
        strSql += " ,ACCODE"
        strSql += " ,(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACGRPCODE"
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME "
        strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE =  "
        strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS TEMPACGRPNAME "
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)ACNAME"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END)AS DEBIT"
        strSql += " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END)AS CREDIT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE BETWEEN @FROMDATE AND @TODATE"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += ftrStr
        strSql += " GROUP BY ACCODE"
        strSql += " )AS X"
        strSql += " GROUP BY ACCODE,ACGRPCODE,ACGRPNAME,ACNAME,TEMPACGRPNAME"
        strSql += " )AS Y"
        strSql += " ORDER BY ACCODE,RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function funcGrandTotal() As Integer
        ''GRAND TOTAL
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GRANDTRAILBAL') > 0"
        strSql += " DROP TABLE TEMP" & systemId & "GRANDTRAILBAL"
        strSql += " DECLARE @FROMDATE AS SMALLDATETIME"
        strSql += " DECLARE @TODATE AS SMALLDATETIME"
        If chkAsonDate.Checked = True Then
            strSql += " SELECT @FROMDATE = '" & cnTranFromDate & "'"
            strSql += " SELECT @TODATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += " SELECT @FROMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += " SELECT "
        strSql += " ' 'ACCODE"
        strSql += " ,' 'ACGRPCODE"
        strSql += " ,'  TOTAL'ACGRPNAME"
        strSql += " ,'ZZZZZGRAND TOTAL'TEMPACGRPNAME"
        strSql += " ,'  TOTAL'ACNAME"
        strSql += " ,SUM(CASE WHEN (ODEBIT - OCREDIT) > 0 THEN (ODEBIT - OCREDIT) ELSE 0 END) AS ODEBIT"
        strSql += " ,SUM(CASE WHEN (OCREDIT - ODEBIT) > 0 THEN (OCREDIT - ODEBIT) ELSE 0 END) AS OCREDIT"
        strSql += " ,SUM(TDEBIT)TDEBIT"
        strSql += " ,SUM(TCREDIT)TCREDIT"
        strSql += " ,SUM(CASE WHEN (CDEBIT - CCREDIT) > 0 THEN (CDEBIT - CCREDIT) ELSE 0 END) AS CDEBIT"
        strSql += " ,SUM(CASE WHEN (CCREDIT - CDEBIT) > 0 THEN (CCREDIT - CDEBIT) ELSE 0 END) AS CCREDIT"
        strSql += " ,'3'RESULT, 'G'COLHEAD"
        strSql += " INTO TEMP" & systemId & "GRANDTRAILBAL"
        strSql += " FROM"
        If cmbRequired.Text = "DETAILED" Then
            strSql += " TEMP" & systemId & "TRAILBAL"
        Else
            strSql += " TEMP" & systemId & "SUBTRAILBAL"
        End If
        strSql += " ORDER BY ACGRPCODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcDifference() As Integer
        'GRANDTOTAL DIFFERENCE
        strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GRANDTRAILBAL')>0 "
        strSql += " BEGIN"
        strSql += " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
        strSql += " 'TEMP" & systemId & "DIFFTRAILBAL') DROP TABLE TEMP" & systemId & "DIFFTRAILBAL"
        strSql += " SELECT ' ' ACCODE, ' ' ACGRPCODE,'  DIFFERENCE' ACGRPNAME, 'ZZZZZZDIFFERENCE' TEMPACGRPNAME,'  DIFFERENCE' ACNAME,"
        strSql += " CASE WHEN (ODEBIT-OCREDIT)>0 THEN (ODEBIT-OCREDIT) ELSE NULL END ODEBIT,"
        strSql += " CASE WHEN (OCREDIT-ODEBIT)>0 THEN (OCREDIT-ODEBIT) ELSE NULL END OCREDIT,"
        strSql += " CASE WHEN (TDEBIT-TCREDIT)>0 THEN (TDEBIT-TCREDIT) ELSE NULL END TDEBIT,"
        strSql += " CASE WHEN (TCREDIT-TDEBIT)>0 THEN (TCREDIT-TDEBIT) ELSE NULL END TCREDIT,"
        strSql += " CASE WHEN (CDEBIT-CCREDIT)>0 THEN (CDEBIT-CCREDIT) ELSE NULL END CDEBIT,"
        strSql += " CASE WHEN (CCREDIT-CDEBIT)>0 THEN (CCREDIT-CDEBIT) ELSE NULL END CCREDIT"
        strSql += " ,'3'RESULT, 'G'COLHEAD"
        strSql += " INTO TEMP" & systemId & "DIFFTRAILBAL "
        strSql += " FROM TEMP" & systemId & "GRANDTRAILBAL "
        strSql += " End"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function funcGridStyleNew() As Integer
        With gridView
            .Columns("MAINGRP").Visible = False
            .Columns("GRPNAME").Visible = False
            .Columns("ACGRPCODE").Visible = False
            .Columns("ACMAINCODE").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("DISPORDER").Visible = False
            .Columns("GRPLEDGER").Visible = False
            .Columns("GRPTYPE").Visible = False
            .Columns("DISPCAPTION").Visible = False
            .Columns("PARTICULAR").Width = 280
            .Columns("PARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
            With .Columns("ODEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("OCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            If chkOpening.Checked = True Then
                .Columns("ODEBIT").Visible = True
                .Columns("ODEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("OCREDIT").Visible = True
                .Columns("OCREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            Else
                .Columns("ODEBIT").Visible = False
                .Columns("OCREDIT").Visible = False
            End If
            With .Columns("TDEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("TCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            If chkTransaction.Checked = True Then
                .Columns("TDEBIT").Visible = True
                .Columns("TDEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("TCREDIT").Visible = True
                .Columns("TCREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            Else
                .Columns("TDEBIT").Visible = False
                .Columns("TCREDIT").Visible = False
            End If
            With .Columns("CDEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("CCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        'tmpWidth = CType(Math.Ceiling(leng / TotalWidth * TotalWidth * _
        ' (e.MarginBounds.Width / TotalWidth)), Int16)
        Dim totalWid As Integer = 20
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            If Not gridView.Columns(cnt).Visible Then Continue For
            totalWid += gridView.Columns(cnt).Width
        Next
        Dim tmpWid As Integer = 0
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            If Not gridView.Columns(cnt).Visible Then Continue For
            tmpWid = CType(Math.Ceiling((gridView.Columns(cnt).Width) / totalWid * totalWid * _
            (gridView.Width / totalWid)), Int16)
            gridView.Columns(cnt).Width = tmpWid
            'totalWid += gridView.Columns(cnt).Width
        Next
        gridView.Columns("PARTICULAR").Visible = True
        funcGridHeaderStyle()
        funcGridTotalStyle()
    End Function
    Function funcGridStyle() As Integer
        With gridView
            .Columns("TEMPACGRPNAME").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("ACNAME").Visible = False
            .Columns("ACGRPNAME").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("DISPORDER").Visible = False
            .Columns("SCHEDULECODE").Visible = False
            If .Columns.Contains("ACCODE") Then .Columns("ACCODE").Visible = False
            If Specificformat = True Then
                .Columns("MAINGRPNAME").Visible = False
            End If

            .Columns("PARTICULAR").Width = 280
            .Columns("PARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
            With .Columns("ODEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("OCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            If chkOpening.Checked = True Then
                .Columns("ODEBIT").Visible = True
                .Columns("ODEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("OCREDIT").Visible = True
                .Columns("OCREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            Else
                .Columns("ODEBIT").Visible = False
                .Columns("OCREDIT").Visible = False
            End If
            With .Columns("TDEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("TCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            If chkTransaction.Checked = True Then
                .Columns("TDEBIT").Visible = True
                .Columns("TDEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("TCREDIT").Visible = True
                .Columns("TCREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            Else
                .Columns("TDEBIT").Visible = False
                .Columns("TCREDIT").Visible = False
            End If
            With .Columns("CDEBIT")
                .HeaderText = "DEBIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("CCREDIT")
                .HeaderText = "CREDIT"
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        'tmpWidth = CType(Math.Ceiling(leng / TotalWidth * TotalWidth * _
        ' (e.MarginBounds.Width / TotalWidth)), Int16)
        Dim totalWid As Integer = 20
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            If Not gridView.Columns(cnt).Visible Then Continue For
            totalWid += gridView.Columns(cnt).Width
        Next
        Dim tmpWid As Integer = 0
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            If Not gridView.Columns(cnt).Visible Then Continue For
            tmpWid = CType(Math.Ceiling((gridView.Columns(cnt).Width) / totalWid * totalWid * _
            (gridView.Width / totalWid)), Int16)
            gridView.Columns(cnt).Width = tmpWid
            'totalWid += gridView.Columns(cnt).Width
        Next
        funcGridHeaderStyle()
        funcGridTotalStyle()
    End Function
    Function funcGridHeaderStyle() As Integer
        With gridHead
            With .Columns("PARTICULAR")
                .Width = gridView.Columns("PARTICULAR").Width
                .HeaderText = ""
                .Visible = True
            End With
            With .Columns("CCREDIT~CDEBIT")
                .Width = gridView.Columns("CDEBIT").Width + gridView.Columns("CCREDIT").Width
                .HeaderText = "CLOSING"
            End With
            With .Columns("TCREDIT~TDEBIT")
                If chkTransaction.Checked = True Then
                    .Width = gridView.Columns("TDEBIT").Width + gridView.Columns("TCREDIT").Width
                    .Visible = True
                    .HeaderText = "TRANSACT"
                Else
                    .Visible = False
                End If
            End With
            With .Columns("OCREDIT~ODEBIT")
                If chkOpening.Checked = True Then
                    .Width = gridView.Columns("ODEBIT").Width + gridView.Columns("OCREDIT").Width
                    .Visible = True
                    .HeaderText = "OPENING"
                Else
                    .Visible = False
                End If
            End With
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        With gridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        ftrStr = funcFiltration()
        If gridView.DataSource IsNot Nothing Then CType(gridView.DataSource, DataTable).Rows.Clear()
        If gridTot.DataSource IsNot Nothing Then CType(gridTot.DataSource, DataTable).Rows.Clear()
        ResizeToolStripMenuItem.Checked = False
        lblFindHelp.Visible = False
        lblHelp.Visible = False
        Me.Refresh()

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_A') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_A "
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_B') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_B "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If chkwithtotal.Checked = False And chkmonthwise.Checked = False Then
            GeneralView()
        ElseIf chkmonthwise.Checked Then
            MonthWiseView()
        ElseIf chkwithtotal.Checked Then
            CostCenterWiseView()
        End If
        
    End Sub
    Private Sub GeneralView()
        If TrailBalGrpWise Then
            If cmbRequired.Text = "DETAILED" Then funcNewFormat() : Exit Sub
        End If
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_A') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_A"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_B') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_B"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        If Specificformat = True Then
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRIALBALANCE_NEW"
        Else
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRIALBALANCE"
        End If

        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@GROUPBY = '" & IIf(cmbOrderBy.Text = "ACC GROUP", "G", "N") & "'"
        strSql += vbCrLf + " ,@RPT_TYPE = '" & IIf(cmbRequired.Text = "DETAILED", "D", "S") & "'"
        strSql += vbCrLf + " ,@WITHOPENING = '" & IIf(Not chkOpening.Checked And Not chkAsonDate.Checked, "N", "Y") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"

        strSql += vbCrLf + " ,@DISPCOL = '" & IIf(chkOpening.Checked, "O", "") & IIf(chkTransaction.Checked, "T", "") & "'"
        If Specificformat = False Then
            strSql += vbCrLf + " ,@SYSID = '" & systemId.ToString & userId.ToString & "'"
        End If

        'strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRIALBALANCE"
        'strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        'strSql += vbCrLf + " ,@GROUPBY = '" & IIf(cmbOrderBy.Text = "ACC GROUP", "G", "N") & "'"
        'strSql += vbCrLf + " ,@RPT_TYPE = '" & IIf(cmbRequired.Text = "DETAILED", "D", "S") & "'"
        'strSql += vbCrLf + " ,@WITHOPENING = '" & IIf(Not chkOpening.Checked And Not chkAsonDate.Checked, "N", "Y") & "'"
        'strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        gridView.DataSource = Nothing
        gridView.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise(gridView)

        Dim openDiff As Decimal = Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)
        Dim tranDiff As Decimal = Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)
        Dim closeDiff As Decimal = Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)
        strSql = " SELECT 'TOTAL'TOTAL"
        Dim NUL As String = "NULL"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString)) & " ODEBIT, " & IIf(Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)) & " OCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString)) & " TDEBIT, " & IIf(Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)) & " TCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString)) & " CDEBIT, " & IIf(Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)) & " CCREDIT"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'DIFFERENCE'TOTAL"
        strSql += vbCrLf + "  ,NULL ODEBIT,NULL OCREDIT"
        strSql += vbCrLf + "  ,NULL TDEBIT,NULL TCREDIT"
        strSql += vbCrLf + "  ,NULL CDEBIT,NULL CCREDIT"
        Dim dtTot As New DataTable
        dtTot.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTot)
        If dtTot.Rows.Count > 0 Then
            gridTot.DataSource = dtTot
            With gridTot
                With .Columns("Total")
                    .HeaderText = "Total"
                    .Width = 280
                    .Visible = True
                End With
                With .Columns("ODEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("OCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                If chkOpening.Checked = True Then
                    .Columns("ODEBIT").Visible = True
                    .Columns("OCREDIT").Visible = True
                Else
                    .Columns("ODEBIT").Visible = False
                    .Columns("OCREDIT").Visible = False
                End If
                With .Columns("TDEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("TCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                If chkTransaction.Checked = True Then
                    .Columns("TDEBIT").Visible = True
                    .Columns("TCREDIT").Visible = True
                Else
                    .Columns("TDEBIT").Visible = False
                    .Columns("TCREDIT").Visible = False
                End If
                With .Columns("CDEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("CCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
            End With
        End If
        For rwIndex As Integer = 0 To gridTot.RowCount - 1
            For colIndex As Integer = 0 To gridTot.ColumnCount - 1
                gridTot.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("verdana", 8.25!, FontStyle.Bold)
                gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionBackColor = Color.White
                gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionForeColor = Color.Black
                '========================================================================
            Next colIndex
        Next rwIndex
        For ColIndex As Integer = 2 To 6 Step 2
            gridTot.Rows(1).Cells(ColIndex).Style.BackColor = Color.LightGreen
        Next ColIndex
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OCREDIT~ODEBIT", GetType(String))
            .Columns.Add("TCREDIT~TDEBIT", GetType(String))
            .Columns.Add("CCREDIT~CDEBIT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OCREDIT~ODEBIT").Caption = "OPENING"
            .Columns("TCREDIT~TDEBIT").Caption = "TRANSACT"
            .Columns("CCREDIT~CDEBIT").Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = dtMergeHeader
        funcGridHeaderStyle()
        funcGridStyle()
        gridTot.Visible = True
        pnlGridHeading.Visible = True
        btnView_Search.Enabled = True
        lblTitle.Text = "TRIAL BALANCE"
        If dtpTo.Enabled = False Then
            lblTitle.Text += " AS ON " & dtpFrom.Text & ""
        Else
            lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        If pnlCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
        gridView.Focus()

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridHead
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub funcNewFormat()
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_A') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_A"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TRAILBAL_B') IS NOT NULL DROP TABLE TEMPTABLEDB..TRAILBAL_B"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        If Specificformat = True Then
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRIALBALANCE_NEW"
        Else
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TRIALBALANCE"
        End If
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@GROUPBY = '" & IIf(cmbOrderBy.Text = "ACC GROUP", "G", "N") & "'"
        strSql += vbCrLf + " ,@RPT_TYPE = '" & IIf(cmbRequired.Text = "DETAILED", "D", "S") & "'"
        strSql += vbCrLf + " ,@WITHOPENING = '" & IIf(Not chkOpening.Checked And Not chkAsonDate.Checked, "N", "Y") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@DISPCOL = '" & IIf(chkOpening.Checked, "O", "") & IIf(chkTransaction.Checked, "T", "") & "'"
        If Specificformat = False Then
            strSql += vbCrLf + " ,@SYSID = '" & systemId.ToString & userId.ToString & "'"
        End If
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        gridView.DataSource = Nothing
        Dim dtBal As New DataTable
        strSql = " SELECT"
        strSql += vbCrLf + "" & cnAdminDb & ".[DBO].GETGROUPNAME(AC.ACMAINCODE) MAINGRP"
        strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=AC.ACMAINCODE)GRPNAME"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(20),ACGRPCODE)ACGRPCODE,ACMAINCODE"
        strSql += vbCrLf + " ,ACGRPNAME AS PARTICULAR"
        strSql += vbCrLf + " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
        strSql += vbCrLf + "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
        strSql += vbCrLf + " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
        strSql += vbCrLf + "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
        strSql += vbCrLf + "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
        strSql += vbCrLf + " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS ODEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS OCREDIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS TDEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS TCREDIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS CDEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS CCREDIT"
        strSql += vbCrLf + " ,'T ' AS COLHEAD"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ACGROUP AC"
        strSql += vbCrLf + " WHERE ACMAINSUB = '1' "
        'strSql += " AND ACGRPNAME='LIABILITIES' "
        strSql += " ORDER BY DISPORDER "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid1 As New DataTable
        dtGrid1.Clear()
        da.Fill(dtGrid1)
        dtBal = funcFillDataTable(dtGrid1)
        Dim dtClone As New DataTable
        Dim dtCloneFinal As New DataTable
        dtClone = dtBal.Copy
        Dim Cnt As Integer = dtBal.Rows.Count - 1
        For i As Integer = 0 To Cnt
            Dim AcgrpCode As String
            If dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "G" Or dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "G1" Then
                Dim Filtt As String
                If dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "G" Then Filtt = "COLHEAD='G'" Else Filtt = "COLHEAD='G' AND PARTICULAR='GRAND DIFF'"
                Dim Drs() As DataRow = dtGrid.Select(Filtt, Nothing)
                If Drs.Length > 0 Then
                    dtBal.Rows(i).Item("ODEBIT") = Drs(0).Item("ODEBIT")
                    dtBal.Rows(i).Item("OCREDIT") = Drs(0).Item("OCREDIT")
                    dtBal.Rows(i).Item("TDEBIT") = Drs(0).Item("TDEBIT")
                    dtBal.Rows(i).Item("TCREDIT") = Drs(0).Item("TCREDIT")
                    dtBal.Rows(i).Item("CDEBIT") = Drs(0).Item("CDEBIT")
                    dtBal.Rows(i).Item("CCREDIT") = Drs(0).Item("CCREDIT")
                End If
            End If
            If dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "D" Then
                AcgrpCode = dtBal.Rows(i).Item("ACGRPCODE").ToString
                Dim Filt As String = "ACCODE='" & AcgrpCode & "'"
                Dim Dr() As DataRow = dtGrid.Select(Filt, Nothing)
                If Dr.Length > 0 Then
                    dtBal.Rows(i).Item("ODEBIT") = Dr(0).Item("ODEBIT")
                    dtBal.Rows(i).Item("OCREDIT") = Dr(0).Item("OCREDIT")
                    dtBal.Rows(i).Item("TDEBIT") = Dr(0).Item("TDEBIT")
                    dtBal.Rows(i).Item("TCREDIT") = Dr(0).Item("TCREDIT")
                    dtBal.Rows(i).Item("CDEBIT") = Dr(0).Item("CDEBIT")
                    dtBal.Rows(i).Item("CCREDIT") = Dr(0).Item("CCREDIT")
                Else
                    dtBal.Rows(i).Delete()
                End If
            ElseIf dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "T" Then
                If i <> Cnt - 1 Then
                    If Val(dtBal.Rows(i).Item("ACMAINCODE").ToString.Trim) <> Val(dtBal.Rows(i + 1).Item("ACMAINCODE").ToString.Trim) Then
                        dtBal.Rows(i).Delete()
                    End If
                End If
            ElseIf dtBal.Rows(i).Item("COLHEAD").ToString.Trim = "T1" Then
                'If i <> Cnt - 1 Then
                '    If Val(dtBal.Rows(i).Item("ACMAINCODE").ToString.Trim) = Val(dtBal.Rows(i + 1).Item("ACMAINCODE").ToString.Trim) Then
                '        dtBal.Rows(i).Delete()
                '    End If
                'End If
            End If
        Next
        dtBal.AcceptChanges()
        dtCloneFinal = dtBal.Copy
        Dim CntFinal As Integer = dtCloneFinal.Rows.Count - 1
        For kk As Integer = 0 To CntFinal
            If kk = CntFinal Then Exit For
            If kk <> CntFinal - 1 Then
                If dtCloneFinal.Rows(kk).Item("COLHEAD").ToString.Trim = "T1" And dtCloneFinal.Rows(kk + 1).Item("COLHEAD").ToString.Trim = "T1" Then
                    If Val(dtBal.Rows(kk).Item("ACMAINCODE").ToString.Trim) = Val(dtBal.Rows(kk + 1).Item("ACMAINCODE").ToString.Trim) Then
                        dtCloneFinal.Rows(kk).Delete()
                    End If
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal Then Exit For
            If jj <> CntFinal - 1 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T" And dtCloneFinal.Rows(jj + 1).Item("COLHEAD").ToString.Trim = "T" Then
                    dtCloneFinal.Rows(jj).Delete()
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal - 1 Then Exit For
            If jj <> CntFinal - 2 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T" And dtCloneFinal.Rows(jj + 1).Item("COLHEAD").ToString.Trim = "T1" And dtCloneFinal.Rows(jj + 2).Item("COLHEAD").ToString.Trim = "T" Then
                    dtCloneFinal.Rows(jj).Delete()
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal - 1 Then Exit For
            If jj <> CntFinal - 2 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T1" And dtCloneFinal.Rows(jj + 1).Item("COLHEAD").ToString.Trim = "T" Then
                    dtCloneFinal.Rows(jj).Delete()
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal - 1 Then Exit For
            If jj <> CntFinal - 2 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T1" And dtCloneFinal.Rows(jj + 1).Item("COLHEAD").ToString.Trim = "T1" Then
                    Dim Part As String = dtCloneFinal.Rows(jj).Item("PARTICULAR").ToString
                    Dim Part1 As String = dtCloneFinal.Rows(jj + 1).Item("PARTICULAR").ToString
                    If (Part.Length - LTrim(Part).Length) > (Part1.Length - LTrim(Part1).Length) Then
                        dtCloneFinal.Rows(jj).Delete()
                    End If
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal - 1 Then Exit For
            If jj <> CntFinal - 2 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T1" And dtCloneFinal.Rows(jj + 1).Item("COLHEAD").ToString.Trim = "T1" Then
                    If dtCloneFinal.Rows(jj).Item("ACMAINCODE").ToString.Trim = dtCloneFinal.Rows(jj + 1).Item("ACMAINCODE").ToString.Trim Then
                        dtCloneFinal.Rows(jj).Delete()
                    End If
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        CntFinal = dtCloneFinal.Rows.Count - 1
        For jj As Integer = 0 To CntFinal
            If jj = CntFinal - 1 Then Exit For
            If jj <> CntFinal - 2 Then
                If dtCloneFinal.Rows(jj).Item("COLHEAD").ToString.Trim = "T" Then
                    dtCloneFinal.Rows(jj).Delete()
                End If
            End If
        Next
        dtCloneFinal.AcceptChanges()
        Dim ObjGrouper As BrighttechPack.DataGridViewGrouper
        ObjGrouper = New BrighttechPack.DataGridViewGrouper(gridView, dtCloneFinal)
        ObjGrouper.pColumns_Group.Add("MAINGRP")

        ObjGrouper.pColumns_Sum.Add("ODEBIT")
        ObjGrouper.pColumns_Sum.Add("OCREDIT")
        ObjGrouper.pColumns_Sum.Add("TDEBIT")
        ObjGrouper.pColumns_Sum.Add("TCREDIT")
        ObjGrouper.pColumns_Sum.Add("CDEBIT")
        ObjGrouper.pColumns_Sum.Add("CCREDIT")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sum_FilterString = "COLHEAD = 'D' "
        ObjGrouper.p_Filter_Empty_Groups.Add("MAINGRP")
        ObjGrouper.pGrandTotal = False
        ObjGrouper.pIssSort = False
        ObjGrouper.GroupDgv()
        'gridView.DataSource = dtCloneFinal
        'Dim dtSource As New DataTable
        'dtSource = dtBal.Copy
        'dtSource.Columns.Add("KEYNO", GetType(Integer))
        'dtSource.Columns("KEYNO").AutoIncrement = True
        'dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        'dtSource.Columns("KEYNO").AutoIncrementStep = 1
        'Dim ObjGrouper As BrighttechPack.DataGridViewGrouper
        'ObjGrouper = New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        'ObjGrouper.pColumns_Group.Add("GRPNAME")
        'ObjGrouper.pColumns_Sum.Add("ODEBIT")
        'ObjGrouper.pColumns_Sum.Add("OCREDIT")
        'ObjGrouper.pColumns_Sum.Add("TDEBIT")
        'ObjGrouper.pColumns_Sum.Add("TCREDIT")
        'ObjGrouper.pColumns_Sum.Add("CDEBIT")
        'ObjGrouper.pColumns_Sum.Add("CCREDIT")
        'ObjGrouper.pColName_Particular = "PARTICULAR"
        'ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        'ObjGrouper.pColumns_Sum_FilterString = "COLHEAD='T1'"
        'ObjGrouper.GroupDgv()
        FillGridGroupStyle(gridView)
        Dim openDiff As Decimal = Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)
        Dim tranDiff As Decimal = Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)
        Dim closeDiff As Decimal = Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)
        strSql = " SELECT 'TOTAL'TOTAL"
        Dim NUL As String = "NULL"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString)) & " ODEBIT, " & IIf(Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)) & " OCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString)) & " TDEBIT, " & IIf(Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)) & " TCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString)) & " CDEBIT, " & IIf(Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)) & " CCREDIT"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'DIFFERENCE'TOTAL"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("ODEBIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("ODEBIT", gridView.RowCount - 1).Value.ToString)) & " ODEBIT, " & IIf(Val(gridView.Item("OCREDIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("OCREDIT", gridView.RowCount - 1).Value.ToString)) & " OCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("TDEBIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("TDEBIT", gridView.RowCount - 1).Value.ToString)) & " TDEBIT, " & IIf(Val(gridView.Item("TCREDIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("TCREDIT", gridView.RowCount - 1).Value.ToString)) & " TCREDIT"
        strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("CDEBIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("CDEBIT", gridView.RowCount - 1).Value.ToString)) & " CDEBIT, " & IIf(Val(gridView.Item("CCREDIT", gridView.RowCount - 1).Value.ToString) = 0, NUL, Val(gridView.Item("CCREDIT", gridView.RowCount - 1).Value.ToString)) & " CCREDIT"
        Dim dtTot As New DataTable
        dtTot.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTot)
        If dtTot.Rows.Count > 0 Then
            gridTot.DataSource = dtTot
            With gridTot
                With .Columns("Total")
                    .HeaderText = "Total"
                    .Width = 280
                    .Visible = True
                End With
                With .Columns("ODEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("OCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                If chkOpening.Checked = True Then
                    .Columns("ODEBIT").Visible = True
                    .Columns("OCREDIT").Visible = True
                Else
                    .Columns("ODEBIT").Visible = False
                    .Columns("OCREDIT").Visible = False
                End If
                With .Columns("TDEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("TCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                If chkTransaction.Checked = True Then
                    .Columns("TDEBIT").Visible = True
                    .Columns("TCREDIT").Visible = True
                Else
                    .Columns("TDEBIT").Visible = False
                    .Columns("TCREDIT").Visible = False
                End If
                With .Columns("CDEBIT")
                    .HeaderText = "DEBIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
                With .Columns("CCREDIT")
                    .HeaderText = "CREDIT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
            End With
        End If
        For rwIndex As Integer = 0 To gridTot.RowCount - 1
            For colIndex As Integer = 0 To gridTot.ColumnCount - 1
                gridTot.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("verdana", 8.25!, FontStyle.Bold)
                gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionBackColor = Color.White
                gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionForeColor = Color.Black
                '========================================================================
            Next colIndex
        Next rwIndex
        For ColIndex As Integer = 2 To 6 Step 2
            gridTot.Rows(1).Cells(ColIndex).Style.BackColor = Color.LightGreen
        Next ColIndex
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OCREDIT~ODEBIT", GetType(String))
            .Columns.Add("TCREDIT~TDEBIT", GetType(String))
            .Columns.Add("CCREDIT~CDEBIT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OCREDIT~ODEBIT").Caption = "OPENING"
            .Columns("TCREDIT~TDEBIT").Caption = "TRANSACT"
            .Columns("CCREDIT~CDEBIT").Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = dtMergeHeader
        funcGridHeaderStyle()
        funcGridStyleNew()
        gridTot.Visible = True
        pnlGridHeading.Visible = True
        btnView_Search.Enabled = True
        lblTitle.Text = "TRIAL BALANCE"
        If dtpTo.Enabled = False Then
            lblTitle.Text += " AS ON " & dtpFrom.Text & ""
        Else
            lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        If pnlCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
        gridView.Focus()
    End Sub
    Function funcFillDataTable(ByVal temp As DataTable) As DataTable
        Dim dt2 As New DataTable
        dumpDt = temp.Copy
        dumpDt.Rows.Clear()
        Dim row As DataRow = Nothing
        Dim AcMainSub As Integer = 2
        funcAcSubMain(temp, AcMainSub)
        strSql = " SELECT "
        strSql += " ''MAINGRP"
        strSql += " ,''GRPNAME"
        strSql += " ,0 ACGRPCODE,0 ACMAINCODE"
        strSql += " ,'GRAND TOTAL' AS PARTICULAR"
        strSql += " ,'' AS GRPLEDGER"
        strSql += " ,'' AS GRPTYPE"
        strSql += " ,0 DISPORDER,0 AS DISPCAPTION"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS ODEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS OCREDIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS TDEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS TCREDIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS CDEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS CCREDIT"
        strSql += " ,'G' AS COLHEAD"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " ''MAINGRP"
        strSql += " ,''GRPNAME"
        strSql += " ,0 ACGRPCODE,0 ACMAINCODE"
        strSql += " ,'GRAND DIFF' AS PARTICULAR"
        strSql += " ,'' AS GRPLEDGER"
        strSql += " ,'' AS GRPTYPE"
        strSql += " ,0 DISPORDER,0 AS DISPCAPTION"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS ODEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS OCREDIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS TDEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS TCREDIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS CDEBIT"
        strSql += " ,CONVERT(NUMERIC(20,2),NULL) AS CCREDIT"
        strSql += " ,'G1' AS COLHEAD"
        da = New OleDbDataAdapter(strSql, cn)
        dt2 = New DataTable
        da.Fill(dt2)
        For i As Integer = 0 To dt2.Rows.Count - 1
            dumpDt.ImportRow(dt2.Rows(i))
        Next
        Return dumpDt
    End Function
    Function funcAcSubMain(ByVal dt As DataTable, ByVal AcMainSub As Integer) As DataTable
        For i As Integer = 0 To dt.Rows.Count - 1
            dumpDt.ImportRow(dt.Rows(i))
            strSql = " SELECT "
            strSql += vbCrLf + "" & cnAdminDb & ".[DBO].GETGROUPNAME(AC.ACMAINCODE) MAINGRP"
            strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=AC.ACMAINCODE)GRPNAME"
            strSql += vbCrLf + " ,ACGRPCODE,ACMAINCODE"
            strSql += vbCrLf + " ,CASE WHEN ACMAINSUB = '1' THEN ACGRPNAME "
            strSql += vbCrLf + " ELSE SPACE(" & AcMainSub & "*2)+ACGRPNAME END  AS PARTICULAR"
            strSql += vbCrLf + " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
            strSql += vbCrLf + "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
            strSql += vbCrLf + " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
            strSql += vbCrLf + "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
            strSql += vbCrLf + "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
            strSql += vbCrLf + " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS ODEBIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS OCREDIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS TDEBIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS TCREDIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS CDEBIT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AS CCREDIT"
            strSql += vbCrLf + " ,'T1' AS COLHEAD"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACGROUP AC WHERE ACMAINCODE = '" & dt.Rows(i).Item("ACGRPCODE").ToString & "' AND ACMAINSUB = " & AcMainSub
            strSql += vbCrLf + " ORDER BY DISPORDER,ACGRPNAME"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dt2 As New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                strSql = " SELECT "
                strSql += vbCrLf + "" & cnAdminDb & ".[DBO].GETGROUPNAME(AC.ACMAINCODE) MAINGRP"
                strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=AC.ACMAINCODE)GRPNAME"
                strSql += vbCrLf + " ,AH.ACCODE ACGRPCODE,AC.ACMAINCODE"
                strSql += vbCrLf + " ,CASE WHEN ACMAINSUB = '1' THEN SPACE(" & AcMainSub & "*2)+ACNAME "
                strSql += vbCrLf + " ELSE SPACE(" & AcMainSub & "*2)+ACNAME END  AS PARTICULAR"
                strSql += vbCrLf + " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
                strSql += vbCrLf + "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
                strSql += vbCrLf + " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
                strSql += vbCrLf + "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
                strSql += vbCrLf + "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
                strSql += vbCrLf + " ,AH.DISPORDER,SCHEDULECODE AS DISPCAPTION"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS ODEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS OCREDIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS TDEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS TCREDIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS CDEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS CCREDIT"
                strSql += vbCrLf + " ,'D ' AS COLHEAD"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ACGROUP AC"
                strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AH ON AH.ACGRPCODE=AC.ACGRPCODE"
                strSql += vbCrLf + " WHERE AC.ACGRPCODE = '" & dt.Rows(i).Item("ACGRPCODE").ToString & "' "
                strSql += vbCrLf + " ORDER BY AH.DISPORDER,AH.ACNAME"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt3 As New DataTable
                da.Fill(dt3)
                For J As Integer = 0 To dt3.Rows.Count - 1
                    dumpDt.ImportRow(dt3.Rows(J))
                Next
                funcAcSubMain(dt2, AcMainSub + 1)
            Else
                strSql = " SELECT "
                strSql += vbCrLf + "" & cnAdminDb & ".[DBO].GETGROUPNAME(AC.ACMAINCODE) MAINGRP"
                strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=AC.ACMAINCODE)GRPNAME"
                strSql += vbCrLf + " ,AH.ACCODE ACGRPCODE,AC.ACMAINCODE"
                strSql += vbCrLf + " ,CASE WHEN ACMAINSUB = '1' THEN SPACE(" & AcMainSub & "*2)+ACNAME "
                strSql += vbCrLf + " ELSE SPACE(" & AcMainSub & "*2)+ACNAME END  AS PARTICULAR"
                strSql += vbCrLf + " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
                strSql += vbCrLf + "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
                strSql += vbCrLf + " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
                strSql += vbCrLf + "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
                strSql += vbCrLf + "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
                strSql += vbCrLf + " ,AH.DISPORDER,SCHEDULECODE AS DISPCAPTION"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS ODEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS OCREDIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS TDEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS TCREDIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS CDEBIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),0) AS CCREDIT"
                strSql += vbCrLf + " ,'D ' AS COLHEAD"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ACGROUP AC"
                strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AH ON AH.ACGRPCODE=AC.ACGRPCODE"
                strSql += vbCrLf + " WHERE AC.ACGRPCODE = '" & dt.Rows(i).Item("ACGRPCODE").ToString & "' "
                strSql += vbCrLf + " ORDER BY AH.DISPORDER,AH.ACNAME"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt3 As New DataTable
                da.Fill(dt3)
                For J As Integer = 0 To dt3.Rows.Count - 1
                    dumpDt.ImportRow(dt3.Rows(J))
                Next
            End If
        Next
    End Function
    Public Sub FillGridGroupStyle(ByVal gridview As DataGridView)
        For Each dgvRow As DataGridViewRow In gridview.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString.Trim = "T" Then
                dgvRow.Cells("PARTICULAR").Style.ForeColor = Color.Blue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString.Trim = "T1" Then
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString.Trim = "G" Then
                dgvRow.DefaultCellStyle = reportTotalStyle
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString.Trim = "G1" Then
                dgvRow.DefaultCellStyle = reportTotalStyle
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
    End Sub
    Private Sub CostCenterWiseView()
        Dim costids As String = "ALL"
        If chkCmbCostCentre.Text <> "ALL" Then
            costids = GetSelectedCostId(chkCmbCostCentre, True)
        End If


        strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_TRIALBALANCECOSTWISE"
        strSql += vbCrLf + "@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + ",@DBNAME ='" & cnStockDb.ToString & "'"
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + ",@COSTID  =""" & costids & """"
        
        strSql += vbCrLf + " ,@GROUPBY = '" & IIf(cmbOrderBy.Text = "ACC GROUP", "G", "N") & "'"
        strSql += vbCrLf + " ,@RPT_TYPE = '" & IIf(cmbRequired.Text = "DETAILED", "D", "S") & "'"
        strSql += vbCrLf + " ,@WITHOPENING = '" & IIf(Not chkOpening.Checked And Not chkAsonDate.Checked, "N", "Y") & "'"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        gridView.DataSource = Nothing
        gridView.DataSource = dtGrid
        If cmbOrderBy.Text <> "ACC GROUP" Then
            gridView.Columns("ACGRPNAME").Visible = False
            gridView.Columns("ACNAME").Visible = False
            gridView.Columns("accode").Visible = False
        Else
            gridView.Columns("ACGRPNAME").Visible = False
            gridView.Columns("TEMPACGRPNAME").Visible = False
        End If
        gridView.Columns("PARTICULAR").Width = 280
        gridView.Columns("KEYNO").Visible = False
        gridView.Columns("DISPORDER").Visible = False
        gridView.Columns("MAINGRPNAME").Visible = False
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("COLHEAD").Visible = False

        Dim dtcost As New DataTable("COST")
        Dim dttype As New DataTable("TYPE")
        Dim hro() As String

        Dim typero(5) As String
        typero(0) = "ODEBIT"
        typero(1) = "OCREDIT"
        typero(2) = "TDEBIT"
        typero(3) = "TCREDIT"
        typero(4) = "CDEBIT"
        typero(5) = "CCREDIT"
        For i As Integer = 0 To gridView.Columns.Count - 1
            If chkOpening.Checked = False Then
                If gridView.Columns(i).HeaderText.Contains("ODEBIT") Then gridView.Columns(i).Visible = False
                If gridView.Columns(i).HeaderText.Contains("OCREDIT") Then gridView.Columns(i).Visible = False
            End If
            If chkTransaction.Checked = False Then
                If gridView.Columns(i).HeaderText.Contains("TDEBIT") Then gridView.Columns(i).Visible = False
                If gridView.Columns(i).HeaderText.Contains("TCREDIT") Then gridView.Columns(i).Visible = False
            End If
            If costids <> "ALL" And costids.Contains(",") = False Then
                If gridView.Columns(i).HeaderText.Contains("TOTAL") Then gridView.Columns(i).Visible = False
            End If

            If gridView.Columns(i).HeaderText.Contains("ODEBIT") Or gridView.Columns(i).HeaderText.Contains("TDEBIT") Or gridView.Columns(i).HeaderText.Contains("CDEBIT") _
            Or gridView.Columns(i).HeaderText.Contains("OCREDIT") Or gridView.Columns(i).HeaderText.Contains("TCREDIT") Or gridView.Columns(i).HeaderText.Contains("CCREDIT") Then
                With gridView.Columns(i)
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "#,##0.00"
                End With
            End If
        Next
        With dtcost
            .Columns.Add("PARTICULAR", GetType(String))
            dttype.Columns.Add("PARTICULAR", GetType(String))
            Dim cdt As New DataTable
            strSql = ""
            If costids = "ALL" Or costids.Contains(",") = True Then strSql = "SELECT 'TOTAL' COSTID,0 RESULT UNION ALL"
            strSql += " SELECT DISTINCT COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            If costids <> "ALL" Then
                strSql += " WHERE COSTID IN(" & costids & ")"
            End If
            strSql += " ORDER BY RESULT,COSTID"
            cdt = New DataTable
            cdt = GetSqlTable(strSql, cn)
            If cdt.Rows.Count > 0 Then
                costids = ""
                For i As Integer = 0 To cdt.Rows.Count - 1
                    costids += cdt.Rows(i).Item("COSTID").ToString
                    If i < cdt.Rows.Count - 1 Then costids += ","
                Next
            End If
            'End If
            If costids <> "ALL" Then
                costids = costids.Replace("'", "")
                hro = costids.Split(",")
            End If
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                For k As Integer = 0 To typero.Length - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & typero(k).ToString) Then
                            txt += gridView.Columns(j).HeaderText
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                    Next
                Next
                If txt <> "" Then
                    Dim caption As String
                    If hro(i).ToString = "TOTAL" Then
                        caption = "TOTAL"
                    Else
                        caption = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN('" & hro(i).ToString & "')").ToString
                    End If
                    .Columns.Add(txt, GetType(String))
                    .Columns(txt).Caption = caption
                End If
            Next
            dtcost.Columns.Add("SCROLL", GetType(String))
            dttype.Columns.Add("SCROLL", GetType(String))
            dtcost.Columns("PARTICULAR").Caption = ""
        End With

        With gridHead
            gridHead.DataSource = Nothing
            .DataSource = dtcost
            .Columns("PARTICULAR").HeaderText = ""
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                Dim typwidth As Integer = 0
                For k As Integer = 0 To typero.Length - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & typero(k).ToString) Then
                            txt += gridView.Columns(j).HeaderText
                            If gridView.Columns(j).Visible = True Then typwidth += gridView.Columns(j).Width
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                    Next
                Next
                If txt <> "" Then
                    Dim caption As String
                    If hro(i).ToString = "TOTAL" Then
                        caption = "TOTAL"
                    Else
                        caption = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN('" & hro(i).ToString & "')").ToString
                    End If
                    .Columns(txt).HeaderText = caption
                    .Columns(txt).Width = typwidth
                End If

            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            
            gridHead.Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            gridHead.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").HeaderText = "PARTICULAR"
            If colWid >= gridView.Width Then
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        End With

        For i As Integer = 0 To gridView.Columns.Count - 1
            If gridView.Columns(i).HeaderText.Contains("ODEBIT") Then gridView.Columns(i).HeaderText = "OPN_DEBIT"
            If gridView.Columns(i).HeaderText.Contains("OCREDIT") Then gridView.Columns(i).HeaderText = "OPN_CREDIT"
            If gridView.Columns(i).HeaderText.Contains("TDEBIT") Then gridView.Columns(i).HeaderText = "TRAN_DEBIT"
            If gridView.Columns(i).HeaderText.Contains("TCREDIT") Then gridView.Columns(i).HeaderText = "TRAN_CREDIT"
            If gridView.Columns(i).HeaderText.Contains("CDEBIT") Then gridView.Columns(i).HeaderText = "CLS_DEBIT"
            If gridView.Columns(i).HeaderText.Contains("CCREDIT") Then gridView.Columns(i).HeaderText = "CLS_CREDIT"
            gridView.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            
        Next

        For i As Integer = 0 To gridView.RowCount - 1
            If gridView.Rows(i).Cells("COLHEAD").Value = "T" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
       
        gridView.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        pnlGridHeading.Visible = True
        btnView_Search.Enabled = True
        lblTitle.Text = "TRIAL BALANCE"
        If dtpTo.Enabled = False Then
            lblTitle.Text += " AS ON " & dtpFrom.Text & ""
        Else
            lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        gridTot.Visible = False
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        If pnlCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
        gridView.Focus()
    End Sub

    Private Sub MonthWiseView()
        Dim costids As String = "ALL"
        If chkCmbCostCentre.Text <> "ALL" Then
            costids = GetSelectedCostId(chkCmbCostCentre, True)
        End If

        strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_TRIALBALANCEMONTHWISE"
        strSql += vbCrLf + "@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + ",@DBNAME ='" & cnStockDb.ToString & "'"
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            'strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + ",@COSTID  =""" & costids & """"
        If cmbOrderBy.Text <> "ACC GROUP" Then
            strSql += vbCrLf + " ,@GROUPBY = 'N'"
            strSql += vbCrLf + " ,@RPT_TYPE = 'D'"
        Else
            strSql += vbCrLf + " ,@GROUPBY = '" & IIf(cmbOrderBy.Text = "ACC GROUP" And cmbRequired.Text <> "DETAILED", "G", "N") & "'"
            strSql += vbCrLf + " ,@RPT_TYPE = 'S'"
            'strSql += vbCrLf + " ,@RPT_TYPE = '" & IIf(cmbRequired.Text = "DETAILED", "D", "S") & "'"
        End If
        
        strSql += vbCrLf + " ,@WITHOPENING = '" & IIf(Not chkOpening.Checked And Not chkAsonDate.Checked, "N", "Y") & "'"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        gridView.DataSource = Nothing
        gridView.DataSource = dtGrid


        If gridView.Columns.Contains("ACNAME") Then gridView.Columns("ACNAME").Visible = False
        If gridView.Columns.Contains("ACCODE") Then gridView.Columns("ACCODE").Visible = False
        If gridView.Columns.Contains("ACGRPNAME") Then gridView.Columns("ACGRPNAME").Visible = False
        If gridView.Columns.Contains("TEMPACGRPNAME") Then gridView.Columns("TEMPACGRPNAME").Visible = False

        gridView.Columns("PARTICULAR").Width = 280
        gridView.Columns("OPENING").Width = 200
        gridView.Columns("CLOSING").Width = 200
        gridView.Columns("KEYNO").Visible = False
        gridView.Columns("DISPORDER").Visible = False
        gridView.Columns("MAINGRPNAME").Visible = False
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("COLHEAD").Visible = False
        gridView.Columns("TRAN").Visible = False
        gridView.Columns("OPEN").Visible = False
        gridView.Columns("CLS").Visible = False

        Dim dtcost As New DataTable("COST")
        Dim dttype As New DataTable("TYPE")
        Dim typwidth As Integer = 0
        Dim txt As String = ""
        With dtcost
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPENING", GetType(String))
            strSql = "SELECT DISTINCT MNTH FROM TEMPTABLEDB..TRAILBAL_B where isnull(mnth,'')<>'OPN'"
            Dim mdt As New DataTable
            mdt = GetSqlTable(strSql, cn)
            If mdt.Rows.Count > 0 Then

                For i As Integer = 0 To mdt.Rows.Count - 1
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(mdt.Rows(i).Item("MNTH").ToString & "-TRAN") Then
                            typwidth += gridView.Columns(j).Width
                            txt += gridView.Columns(j).HeaderText
                            gridView.Columns(j).HeaderText = mdt.Rows(i).Item("MNTH").ToString
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                    Next
                Next
                .Columns.Add(txt, GetType(String))
                .Columns(txt).Caption = "Transcation"
            End If
            dtcost.Columns.Add("CLOSING", GetType(String))
            dtcost.Columns.Add("SCROLL", GetType(String))
            dtcost.Columns("PARTICULAR").Caption = ""
        End With

        With gridHead
            gridHead.DataSource = Nothing
            .DataSource = dtcost
            .Columns("PARTICULAR").HeaderText = ""
            .Columns("OPENING").HeaderText = "OPENING"
            .Columns("OPENING").Width = gridView.Columns("OPENING").Width
            .Columns("CLOSING").HeaderText = "CLOSING"
            .Columns("CLOSING").Width = gridView.Columns("CLOSING").Width
            If txt <> "" Then
                .Columns(txt).Width = typwidth
                .Columns(txt).HeaderText = "Transcation"
            End If
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            gridHead.Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            gridHead.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").HeaderText = "PARTICULAR"
            If colWid >= gridView.Width Then
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        End With

        For i As Integer = 0 To gridView.Columns.Count - 1
            If gridView.Columns(i).HeaderText.Contains("CLOSING") Then gridView.Columns(i).DisplayIndex = gridView.ColumnCount - 1
            'If gridView.Columns(i).HeaderText.Contains("OCREDIT") Then gridView.Columns(i).HeaderText = "OPN_CREDIT"
            'If gridView.Columns(i).HeaderText.Contains("TDEBIT") Then gridView.Columns(i).HeaderText = "TRAN_DEBIT"
            'If gridView.Columns(i).HeaderText.Contains("TCREDIT") Then gridView.Columns(i).HeaderText = "TRAN_CREDIT"
            'If gridView.Columns(i).HeaderText.Contains("CDEBIT") Then gridView.Columns(i).HeaderText = "CLS_DEBIT"
            'If gridView.Columns(i).HeaderText.Contains("CCREDIT") Then gridView.Columns(i).HeaderText = "CLS_CREDIT"
            gridView.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight

        Next

        For i As Integer = 0 To gridView.RowCount - 1
            If gridView.Rows(i).Cells("COLHEAD").Value = "T" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next

        gridView.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        pnlGridHeading.Visible = True
        btnView_Search.Enabled = True
        lblTitle.Text = "TRIAL BALANCE"
        If dtpTo.Enabled = False Then
            lblTitle.Text += " AS ON " & dtpFrom.Text & ""
        Else
            lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        gridTot.Visible = False
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        If pnlCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
        gridView.Focus()
    End Sub
    Private Sub Old_btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim openDiff As Double
        Dim tranDiff As Double
        Dim closeDiff As Double
        Try
            'Me.Cursor = Cursors.WaitCursor
            btnView_Search.Enabled = False
            dsGridView.Clear()
            lblTitle.Text = ""
            gridView.DataSource = Nothing
            ftrStr = funcFiltration()

            strSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' "
            strSql += vbCrLf + "  AND NAME = 'TEMP" & systemId & "TRAILBALANCE')"
            strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "TRAILBALANCE"
            strSql += vbCrLf + "  CREATE TABLE TEMP" & systemId & "TRAILBALANCE("
            strSql += vbCrLf + "  ACCODE VARCHAR(10),"
            strSql += vbCrLf + "  ACGRPCODE VARCHAR(20),"
            strSql += vbCrLf + "  ACGRPNAME VARCHAR(40),"
            strSql += vbCrLf + "  TEMPACGRPNAME VARCHAR(40),"
            strSql += vbCrLf + "  ACNAME VARCHAR(55),"
            strSql += vbCrLf + "  ODEBIT NUMERIC(38,2),"
            strSql += vbCrLf + "  OCREDIT NUMERIC(38,2),"
            strSql += vbCrLf + "  TDEBIT NUMERIC(38,2),"
            strSql += vbCrLf + "  TCREDIT NUMERIC(38,2),"
            strSql += vbCrLf + "  CDEBIT NUMERIC(38,2),"
            strSql += vbCrLf + "  CCREDIT NUMERIC(38,2),"
            strSql += vbCrLf + "  COLHEAD VARCHAR(1),"
            strSql += vbCrLf + "  SNO INT IDENTITY(1,1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If cmbRequired.Text = "DETAILED" Then
                funcTrialBal()
                If cmbOrderBy.Text <> "A/C NAME" Then
                    funcSubTotal()
                End If
            Else
                funcSubTotal()
            End If
            funcGrandTotal()
            funcDifference()
            If cmbRequired.Text = "DETAILED" Then
                strSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FINALREP') "
                strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "FINALREP"
                strSql += vbCrLf + "  SELECT ACCODE,ACGRPCODE,ACGRPNAME,TEMPACGRPNAME,ACNAME"
                strSql += vbCrLf + "  ,CASE WHEN ODEBIT = 0 THEN NULL ELSE ODEBIT END ODEBIT,CASE WHEN OCREDIT = 0 THEN NULL ELSE OCREDIT END OCREDIT"
                strSql += vbCrLf + "  ,CASE WHEN TDEBIT  = 0 THEN NULL ELSE TDEBIT END TDEBIT,CASE WHEN TCREDIT  = 0 THEN NULL ELSE TCREDIT END TCREDIT"
                strSql += vbCrLf + "  ,CASE WHEN CDEBIT  = 0 THEN NULL ELSE CDEBIT END CDEBIT,CASE WHEN CCREDIT  = 0 THEN NULL ELSE CCREDIT END CCREDIT"
                strSql += vbCrLf + "  ,RESULT,COLHEAD"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "FINALREP FROM"
                strSql += vbCrLf + "  ("
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "TRAILBAL"
                If cmbOrderBy.Text <> "A/C NAME" Then
                    strSql += vbCrLf + "  UNION ALL"
                    strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "SUBTRAILBAL"
                End If
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "GRANDTRAILBAL"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "DIFFTRAILBAL"
                strSql += vbCrLf + " )X"
                If cmbOrderBy.Text <> "A/C NAME" Then
                    strSql += vbCrLf + "  ORDER BY TEMPACGRPNAME,RESULT"
                Else
                    strSql += vbCrLf + "  ORDER BY RESULT,ACNAME"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FINALREP') "
                strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "FINALREP"
                strSql += vbCrLf + "  SELECT ACCODE,ACGRPCODE,ACGRPNAME,TEMPACGRPNAME,ACNAME"
                strSql += vbCrLf + "  ,CASE WHEN ODEBIT = 0 THEN NULL ELSE ODEBIT END ODEBIT,CASE WHEN OCREDIT = 0 THEN NULL ELSE OCREDIT END OCREDIT"
                strSql += vbCrLf + "  ,CASE WHEN TDEBIT  = 0 THEN NULL ELSE TDEBIT END TDEBIT,CASE WHEN TCREDIT  = 0 THEN NULL ELSE TCREDIT END TCREDIT"
                strSql += vbCrLf + "  ,CASE WHEN CDEBIT  = 0 THEN NULL ELSE CDEBIT END CDEBIT,CASE WHEN CCREDIT  = 0 THEN NULL ELSE CCREDIT END CCREDIT"
                strSql += vbCrLf + "  ,RESULT,COLHEAD"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "FINALREP FROM"
                strSql += vbCrLf + "  ("
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "SUBTRAILBAL"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "GRANDTRAILBAL"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT * FROM TEMP" & systemId & "DIFFTRAILBAL"
                strSql += vbCrLf + " )X"
                strSql += vbCrLf + "  ORDER BY TEMPACGRPNAME,RESULT"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            If cmbRequired.Text = "DETAILED" And cmbOrderBy.Text = "ACC GROUP" Then
                strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FINALREP')> 0 "
                strSql += vbCrLf + "  BEGIN"
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "FINALREP (ACNAME,TEMPACGRPNAME,COLHEAD,RESULT) "
                strSql += vbCrLf + "  SELECT DISTINCT ACGRPNAME ACNAME, ACGRPNAME TEMPACGRPNAME, 'T', 0 FROM "
                strSql += vbCrLf + "  TEMP" & systemId & "FINALREP WHERE ACGRPNAME NOT IN ('  DIFFERENCE','  TOTAL') "
                strSql += vbCrLf + "  END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "FINALREP')> 0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "TRAILBALANCE (ACCODE, ACGRPCODE, ACGRPNAME, "
            strSql += vbCrLf + "  TEMPACGRPNAME, ACNAME,ODEBIT, OCREDIT, TDEBIT, TCREDIT, "
            strSql += vbCrLf + "  CDEBIT, CCREDIT, COLHEAD)"
            strSql += vbCrLf + "  SELECT ACCODE, ACGRPCODE, ACGRPNAME, TEMPACGRPNAME, "
            strSql += vbCrLf + "  ACNAME,ODEBIT, OCREDIT, TDEBIT, TCREDIT, CDEBIT, "
            strSql += vbCrLf + "  CCREDIT, COLHEAD FROM TEMP" & systemId & "FINALREP "
            strSql += vbCrLf + "  ORDER BY TEMPACGRPNAME,RESULT,ACNAME"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If cmbRequired.Text = "SUMMARY" Then
                strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "TRAILBALANCE')> 0 "
                strSql += vbCrLf + "  BEGIN"
                strSql += vbCrLf + "  UPDATE TEMP" & systemId & "TRAILBALANCE SET COLHEAD = ' ' WHERE COLHEAD = 'S'"
                strSql += vbCrLf + "  END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT ACCODE, ACGRPCODE, ACGRPNAME, TEMPACGRPNAME, "
                strSql += vbCrLf + "  ACGRPNAME ACNAME"
                If chkAsonDate.Checked = False And chkOpening.Checked = False Then
                    strSql += vbCrLf + "  ,ODEBIT, OCREDIT, TDEBIT, TCREDIT, TDEBIT CDEBIT, TCREDIT CCREDIT"
                Else
                    strSql += vbCrLf + "  ,ODEBIT, OCREDIT, TDEBIT, TCREDIT, CDEBIT, CCREDIT"
                End If
                strSql += vbCrLf + "  ,COLHEAD FROM TEMP" & systemId & "TRAILBALANCE ORDER BY SNO "
            Else
                strSql = " SELECT ACCODE, ACGRPCODE, ACGRPNAME, TEMPACGRPNAME, "
                strSql += vbCrLf + "  ACNAME"
                If chkAsonDate.Checked = False And chkOpening.Checked = False Then
                    strSql += vbCrLf + "  ,ODEBIT, OCREDIT, TDEBIT, TCREDIT, TDEBIT CDEBIT, TCREDIT CCREDIT"
                    strSql += vbCrLf + "  ,COLHEAD FROM TEMP" & systemId & "TRAILBALANCE "
                    strSql += vbCrLf + "  WHERE "
                    strSql += vbCrLf + "  NOT(ISNULL(COLHEAD,'') <> 'T' AND ISNULL(TDEBIT,0) = 0 AND ISNULL(TCREDIT,0) = 0"
                    If chkOpening.Checked Then
                        strSql += vbCrLf + "  AND ISNULL(ODEBIT,0) = 0 AND ISNULL(OCREDIT,0) = 0"
                    End If
                    If chkTransaction.Checked Then
                        strSql += vbCrLf + "  AND ISNULL(TDEBIT,0) = 0 AND ISNULL(TCREDIT,0) = 0"
                    End If
                    strSql += vbCrLf + "  )"
                    strSql += vbCrLf + "  ORDER BY SNO,ACNAME "
                Else
                    strSql += vbCrLf + "  ,ODEBIT, OCREDIT, TDEBIT, TCREDIT, CDEBIT, CCREDIT"
                    strSql += vbCrLf + "  ,COLHEAD FROM TEMP" & systemId & "TRAILBALANCE "
                    strSql += vbCrLf + "  WHERE "
                    strSql += vbCrLf + "  NOT(ISNULL(COLHEAD,'') <>  'T' AND ISNULL(CDEBIT,0) = 0 AND ISNULL(CCREDIT,0) = 0"
                    If chkOpening.Checked Then
                        strSql += vbCrLf + "  AND ISNULL(ODEBIT,0) = 0 AND ISNULL(OCREDIT,0) = 0"
                    End If
                    If chkTransaction.Checked Then
                        strSql += vbCrLf + "  AND ISNULL(TDEBIT,0) = 0 AND ISNULL(TCREDIT,0) = 0"
                    End If
                    strSql += vbCrLf + "  )"
                    strSql += vbCrLf + "  ORDER BY SNO,ACNAME "
                End If
            End If

            Dim dtGrid As New DataTable
            dtGrid.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                pnlGridHeading.Visible = False
                gridView.DataSource = Nothing
                'gridTot.DataSource = Nothing
                btnView_Search.Enabled = True
                Exit Sub
            End If
            If cmbRequired.Text = "SUMMARY" Then
                gridView.DataSource = dtGrid
                gridView.Columns("ACNAME").HeaderText = "ACGRPNAME"
            ElseIf cmbOrderBy.Text = "A/C NAME" Then
                gridView.DataSource = dtGrid
            Else
                gridView.DataSource = dtGrid
            End If
            GridViewFormat()
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("ACCODE").Visible = False
            gridView.Columns("ACGRPCODE").Visible = False
            gridView.Columns("ACGRPNAME").Visible = False
            gridView.Columns("TEMPACGRPNAME").Visible = False

            'funcGridStyle()

            openDiff = Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)
            tranDiff = Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)
            closeDiff = Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) - Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)
            strSql = " SELECT 'TOTAL'TOTAL"
            Dim NUL As String = "NULL"
            strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("ODEBIT", gridView.RowCount - 2).Value.ToString)) & " ODEBIT, " & IIf(Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("OCREDIT", gridView.RowCount - 2).Value.ToString)) & " OCREDIT"
            strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TDEBIT", gridView.RowCount - 2).Value.ToString)) & " TDEBIT, " & IIf(Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("TCREDIT", gridView.RowCount - 2).Value.ToString)) & " TCREDIT"
            strSql += vbCrLf + "  ," & IIf(Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CDEBIT", gridView.RowCount - 2).Value.ToString)) & " CDEBIT, " & IIf(Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString) = 0, NUL, Val(gridView.Item("CCREDIT", gridView.RowCount - 2).Value.ToString)) & " CCREDIT"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 'DIFFERENCE'TOTAL"
            strSql += vbCrLf + "  ,NULL ODEBIT,NULL OCREDIT"
            strSql += vbCrLf + "  ,NULL TDEBIT,NULL TCREDIT"
            strSql += vbCrLf + "  ,NULL CDEBIT,NULL CCREDIT"
            Dim dtTot As New DataTable
            dtTot.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTot)
            If dtTot.Rows.Count > 0 Then
                gridTot.DataSource = dtTot
                With gridTot
                    With .Columns("Total")
                        .HeaderText = "Total"
                        .Width = 280
                        .Visible = True
                    End With
                    With .Columns("ODEBIT")
                        .HeaderText = "DEBIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                    With .Columns("OCREDIT")
                        .HeaderText = "CREDIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                    If chkOpening.Checked = True Then
                        .Columns("ODEBIT").Visible = True
                        .Columns("OCREDIT").Visible = True
                    Else
                        .Columns("ODEBIT").Visible = False
                        .Columns("OCREDIT").Visible = False
                    End If
                    With .Columns("TDEBIT")
                        .HeaderText = "DEBIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                    With .Columns("TCREDIT")
                        .HeaderText = "CREDIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                    If chkTransaction.Checked = True Then
                        .Columns("TDEBIT").Visible = True
                        .Columns("TCREDIT").Visible = True
                    Else
                        .Columns("TDEBIT").Visible = False
                        .Columns("TCREDIT").Visible = False
                    End If
                    With .Columns("CDEBIT")
                        .HeaderText = "DEBIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                    With .Columns("CCREDIT")
                        .HeaderText = "CREDIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .DefaultCellStyle.Format = "#,##0.00"
                    End With
                End With
            End If
            If dsGridView.Tables.Contains("TITLE") = True Then
                For rwIndex As Integer = 0 To gridView.RowCount - 1
                    For colIndex As Integer = 0 To gridView.ColumnCount - 1
                        If colIndex = 0 Then
                            For CNT As Integer = 0 To dsGridView.Tables("TITLE").Rows.Count - 1
                                If rwIndex = dsGridView.Tables("TITLE").Rows(CNT).Item("TITLE") Then
                                    gridView.Rows(rwIndex).Cells(colIndex).Style.BackColor = Color.LightBlue
                                    gridView.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("Verdana", 8, FontStyle.Bold)
                                End If
                            Next CNT
                        End If
                        For CNT As Integer = 0 To dsGridView.Tables("SUBTOTAL").Rows.Count - 1
                            If rwIndex = dsGridView.Tables("SUBTOTAL").Rows(CNT).Item("SUBTOTAL") Then
                                'gridView.Rows(rwIndex).Cells(colIndex).Style.BackColor = Color.White
                                gridView.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("Verdana", 8, FontStyle.Bold)
                            End If
                        Next CNT
                    Next colIndex
                Next rwIndex
            End If
            For colIndex As Integer = 0 To gridView.ColumnCount - 1
                gridView.Rows(gridView.RowCount - 1).Cells(colIndex).Style.Font = New Font("Verdana", 8.25!, FontStyle.Bold)
                gridView.Rows(gridView.RowCount - 1).Cells(colIndex).Style.BackColor = Color.LightYellow
            Next
            'gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            'gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            For rwIndex As Integer = 0 To gridTot.RowCount - 1
                For colIndex As Integer = 0 To gridTot.ColumnCount - 1
                    '========================================================================
                    '17.09.08 modified
                    'gridTot.Rows(rwIndex).Cells(colIndex).Style.Font = New Font(gridTot.Rows(rwIndex).Cells(colIndex).Style.Font.FontFamily, gridTot.Rows(rwIndex).Cells(colIndex).Style.Font.Size, FontStyle.Bold)
                    gridTot.Rows(rwIndex).Cells(colIndex).Style.Font = New Font("verdana", 8.25!, FontStyle.Bold)
                    gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionBackColor = Color.White
                    gridTot.Rows(rwIndex).Cells(colIndex).Style.SelectionForeColor = Color.Black
                    '========================================================================
                Next colIndex
            Next rwIndex
            '========================================================================
            '17.09.08 modified
            For ColIndex As Integer = 2 To 6 Step 2
                gridTot.Rows(1).Cells(ColIndex).Style.BackColor = Color.LightGreen
            Next ColIndex
            '========================================================================
            ''heading Grid

            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("ACNAME", GetType(String))
                .Columns.Add("OCREDIT~ODEBIT", GetType(String))
                .Columns.Add("TCREDIT~TDEBIT", GetType(String))
                .Columns.Add("CCREDIT~CDEBIT", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("ACNAME").Caption = ""
                .Columns("OCREDIT~ODEBIT").Caption = "OPENING"
                .Columns("TCREDIT~TDEBIT").Caption = "TRANSACT"
                .Columns("CCREDIT~CDEBIT").Caption = "CLOSING"
                .Columns("SCROLL").Caption = ""
            End With

            'strSql = " SELECT 'ACNAME'ACNAME,'OPENING'OPENING,'TRANSACT'TRANSACT,'CLOSING'CLOSING where 1<>1"
            'Dim dtHead As New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtHead)
            'gridHead.DataSource = dtHead

            gridHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
            funcGridStyle()

            pnlGridHeading.Visible = True
            btnView_Search.Enabled = True
            lblTitle.Text = "TRIAL BALANCE"
            If dtpTo.Enabled = False Then
                lblTitle.Text += " AS ON " & dtpFrom.Text & ""
            Else
                lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            End If
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.Focus()

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            With gridHead
                If colWid >= gridView.Width Then
                    .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                    .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                Else
                    .Columns("SCROLL").Visible = False
                End If
            End With
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Arrow
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub frmTrailBal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTrailBal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGridHeading.Visible = False
        chkAsonDate.Checked = False
        chkoldformat.Visible = False
        ''viewtype = GetAdmindbSoftValue("CCWISE_FINRPT", "N")
        ''CostCentre
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            pnlCostCentre.Visible = True
        Else
            pnlCostCentre.Visible = False
        End If
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        cmbRequired.Items.Clear()
        cmbRequired.Items.Add("DETAILED")
        cmbRequired.Items.Add("SUMMARY")
        cmbRequired.Text = "DETAILED"
        btnNew_Click(Me, New EventArgs)
        chkAsonDate.Select()
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            chkAsonDate.Text = "&As OnDate"
            pnlDate.Enabled = False
        Else
            chkAsonDate.Text = "&Date From"
            pnlDate.Enabled = True
        End If
    End Sub

    Private Sub cmbRequired_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRequired.SelectedIndexChanged
        Dim Cmb As String = cmbOrderBy.Text
        cmbOrderBy.Items.Clear()
        If cmbRequired.Text = "DETAILED" Then
            cmbOrderBy.Items.Add("A/C NAME")
            cmbOrderBy.Items.Add("ACC GROUP")
            cmbOrderBy.Text = IIf(Cmb <> "", Cmb, "A/C NAME")
        Else
            cmbOrderBy.Items.Add("ACC GROUP")
            cmbOrderBy.Text = "ACC GROUP"
        End If

    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        gridTot.DataSource = Nothing
        btnView_Search.Enabled = True
        ' chkAsonDate.Checked = True
        'chkMore.Checked = False
        'cmbOrderBy.Text = "ACC GROUP"
        'cmbRequired.Text = "DETAILED"
        Prop_Gets()
        chkAsonDate.Select()
        lblFindHelp.Visible = False
        lblHelp.Visible = False
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case "T"
    ''            gridView.Rows(e.RowIndex).Cells("ACNAME").Style.BackColor = reportHeadStyle.BackColor
    ''            gridView.Rows(e.RowIndex).Cells("ACNAME").Style.Font = reportHeadStyle.Font
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        Case "G"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''    End Select
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case dgvRow.Cells("COLHEAD").Value.ToString
                    Case "T"
                        dgvRow.Cells("ACNAME").Style.BackColor = reportHeadStyle.BackColor
                        dgvRow.Cells("ACNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        dgvRow.DefaultCellStyle.Font = reportTotalStyle.Font
                        dgvRow.DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub funcGridTotalStyle()
        If gridTot.ColumnCount > 0 And (chkwithtotal.Checked = False And chkmonthwise.Checked = False) Then
            gridTot.Columns("Total").Width = gridView.Columns("PARTICULAR").Width
            gridTot.Columns("ODEBIT").Width = gridView.Columns("ODEBIT").Width
            gridTot.Columns("OCREDIT").Width = gridView.Columns("OCREDIT").Width
            gridTot.Columns("TDEBIT").Width = gridView.Columns("TDEBIT").Width
            gridTot.Columns("TCREDIT").Width = gridView.Columns("TCREDIT").Width
            gridTot.Columns("CDEBIT").Width = gridView.Columns("CDEBIT").Width
            gridTot.Columns("CCREDIT").Width = gridView.Columns("CCREDIT").Width
        End If
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridHead.ColumnCount > 0 And (chkwithtotal.Checked = False And chkmonthwise.Checked = False) Then
            gridHead.Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            gridHead.Columns("OCREDIT~ODEBIT").Width = gridView.Columns("ODEBIT").Width + gridView.Columns("OCREDIT").Width
            gridHead.Columns("TCREDIT~TDEBIT").Width = gridView.Columns("TDEBIT").Width + gridView.Columns("TCREDIT").Width
            gridHead.Columns("CCREDIT~CDEBIT").Width = gridView.Columns("CDEBIT").Width + gridView.Columns("CCREDIT").Width
            gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End If
        funcGridTotalStyle()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblFindHelp.Visible = True
        lblHelp.Visible = True
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblFindHelp.Visible = False
        lblHelp.Visible = False
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.X) Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
            '=====================================================
            '170908 modified
        ElseIf e.KeyChar = Chr(Keys.P) Or e.KeyChar = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
            '=======================================================
        ElseIf UCase(e.KeyChar) = "D" Then
            'If cmbRequired.Text = "SUMMARY" Then
            '    If gridView.CurrentRow.Index < gridView.RowCount - 2 Then
            '        If gridView.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
            '        Dim strlbltitledet As String
            '        Dim boolOpenCheck As New Boolean
            '        Dim boolTransCheck As New Boolean
            '        boolOpenCheck = IIf(chkOpening.Checked, True, False)
            '        boolTransCheck = IIf(chkTransaction.Checked, True, False)
            '        strlbltitledet = "TRIAL BALANCE DETAILS "
            '        If dtpTo.Enabled = False Then
            '            strlbltitledet += " AS ON " & dtpFrom.Text & ""
            '        Else
            '            strlbltitledet += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            '        End If
            '        strlbltitledet += " FOR " & gridView.Item("TEMPAcGrpName", gridView.CurrentRow.Index).Value.ToString
            '        funcTrialBal()
            '        Dim objDetal As New frmTrialBalanceDetail(gridView.CurrentRow.Cells("ACNAME").Value.ToString, strlbltitledet, boolOpenCheck, boolTransCheck)
            '        objDetal.ShowDialog()
            '    End If
            'End If

            'If gridView.CurrentRow.Index < gridView.RowCount - 2 Then
            '    If gridView.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
            '    Dim objDetal As New frmAccountsLedger()
            '    objDetal.cmbAcGroup.Text = gridView.CurrentRow.Cells("ACGRPNAME").Value.ToString.Trim
            '    objDetal.cmbAcName.Text = gridView.CurrentRow.Cells("PARTICULAR").Value.ToString.Trim
            '    objDetal.chkCmbCostCentre.Text = chkCmbCostCentre.Text
            '    objDetal.chkCmbCompany.Text = strCompanyName
            '    objDetal.flag = True
            '    objDetal.ExCalling = True
            '    objDetal.chkMultiSelect.Checked = False
            '    If chkAsonDate.Checked Then
            '        objDetal.dtpFrom.Value = cnTranFromDate.Date.ToString("yyyy-MM-dd")
            '        objDetal.dtpTo.Value = dtpFrom.Value
            '    Else
            '        objDetal.dtpFrom.Value = dtpFrom.Value
            '        objDetal.dtpTo.Value = dtpTo.Value
            '    End If
            '    objDetal.btnView_Search_Click(Me, New EventArgs)
            '    objDetal.ShowDialog()
            'End If
        ElseIf UCase(e.KeyChar) = "L" Then
            If cmbRequired.Text = "DETAILED" Then
                With frmAccountsLedger
                    If TrailBalGrpWise Then
                        If gridView.Rows(gridView.CurrentRow.Index).Cells("COLHEAD").Value.ToString.Trim <> "D" Then Exit Sub
                    Else
                        If gridView.Rows(gridView.CurrentRow.Index).Cells("COLHEAD").Value.ToString.Trim <> "" Then Exit Sub
                    End If
                    .tabMain.SelectedTab = .tabView
                    .ExCalling = True
                    Main.funcShow(frmAccountsLedger)
                    .chkCmbCostCentre.Text = chkCmbCostCentre.Text
                    .chkCmbCompany.Text = strCompanyName
                    .chkCmbCompany.Text = strCompanyName
                    Dim AcName As String
                    If TrailBalGrpWise Then
                        AcName = gridView.Rows(gridView.CurrentRow.Index).Cells("ACGRPCODE").Value.ToString.Trim
                        strSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & AcName & "'"
                        AcName = objGPack.GetSqlValue(strSql)
                    Else
                        AcName = gridView.Rows(gridView.CurrentRow.Index).Cells("ACNAME").Value.ToString
                    End If
                    strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP"
                    strSql += " WHERE ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
                    .cmbAcGroup.Text = objGPack.GetSqlValue(strSql)
                    .cmbAcName.Text = AcName
                    If chkAsonDate.Checked Then
                        .dtpFrom.Value = cnTranFromDate
                        .dtpTo.Value = dtpFrom.Value
                    Else
                        .dtpFrom.Value = dtpFrom.Value
                        .dtpTo.Value = dtpTo.Value
                    End If

                    .btnNew.Enabled = False
                    .flag = True
                    .btnView_Search_Click(Me, New EventArgs)
                End With
            Else
                MsgBox("Ledger View Available for Detailed View", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub gridDetailView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Visible = False
            gridView.Focus()
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub chkMore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMore.CheckedChanged
        Panel1.Visible = chkMore.Checked
        If chkMore.Checked = False Then
            cmbOrderBy.Text = "ACC GROUP"
            cmbRequired.Text = "DETAILED"
            chkOpening.Checked = False
            chkTransaction.Checked = False
        End If
    End Sub

    Private Sub Panel1_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.VisibleChanged
        Panel1.Enabled = Panel1.Visible
    End Sub

    Private Sub cmbCostCentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            btnView_Search.Select()
        End If
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
        Dim obj As New frmTrailBal_Properties
        obj.p_chkAsonDate = chkAsonDate.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbOrderBy = cmbOrderBy.Text
        obj.p_cmbRequired = cmbRequired.Text
        obj.p_chkOpening = chkOpening.Checked
        obj.p_chkTransaction = chkTransaction.Checked
        obj.p_chkMore = chkMore.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTrailBal_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTrailBal_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTrailBal_Properties))
        chkAsonDate.Checked = obj.p_chkAsonDate
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbOrderBy.Text = obj.p_cmbOrderBy
        cmbRequired.Text = obj.p_cmbRequired
        chkOpening.Checked = obj.p_chkOpening
        chkTransaction.Checked = obj.p_chkTransaction
        chkMore.Checked = obj.p_chkMore
    End Sub

    Private Sub chkoldformat_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkoldformat.CheckedChanged
        If chkoldformat.Checked = True Then
            Specificformat = True
        Else
            Specificformat = False
        End If
    End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView)
        objSearch.ShowDialog()
    End Sub
End Class

Public Class frmTrailBal_Properties
    Private chkAsonDate As Boolean = True
    Public Property p_chkAsonDate() As Boolean
        Get
            Return chkAsonDate
        End Get
        Set(ByVal value As Boolean)
            chkAsonDate = value
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
    Private cmbOrderBy As String = "ACC GROUP"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
    Private cmbRequired As String = "DETAILED"
    Public Property p_cmbRequired() As String
        Get
            Return cmbRequired
        End Get
        Set(ByVal value As String)
            cmbRequired = value
        End Set
    End Property
    Private chkOpening As Boolean = True
    Public Property p_chkOpening() As Boolean
        Get
            Return chkOpening
        End Get
        Set(ByVal value As Boolean)
            chkOpening = False
        End Set
    End Property
    Private chkTransaction As Boolean = False
    Public Property p_chkTransaction() As Boolean
        Get
            Return chkTransaction
        End Get
        Set(ByVal value As Boolean)
            chkTransaction = False
        End Set
    End Property
    Private chkMore As Boolean = False
    Public Property p_chkMore() As Boolean
        Get
            Return chkMore
        End Get
        Set(ByVal value As Boolean)
            chkMore = False
        End Set
    End Property
End Class