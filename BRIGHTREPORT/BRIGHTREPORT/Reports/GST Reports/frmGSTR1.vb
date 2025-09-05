Imports System.Data.OleDb
Imports System.Threading
Imports System.IO


Public Class frmGSTR1
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Private Sub funcSumm()
        strSql = " EXEC " & cnAdminDb & "..PROC_GSTR1"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " EXEC " & cnAdminDb & "..PROC_GSTR2"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@STATE = '" & CompanyState & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT SUM(CNT)CNT,SUM(TAXABLE)TAXABLE"
        strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
        strSql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT FROM ("
        strSql += vbCrLf + "SELECT COUNT(*)CNT"
        strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
        strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
        strSql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT "
        strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT "
        strSql += vbCrLf + "WHERE A='R- Regular B2B Invoices'"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT COUNT(*)CNT"
        strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
        strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
        strSql += vbCrLf + ",NULL CESSAMOUNT "
        strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT2 )X"
        Dim dtB2B As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtB2B)
        Dim Sql As String
        Dim TotRcCnt As Integer = 0
        Dim TotRcVal As Decimal = 0
        Dim TotIgst As Decimal = 0
        Dim TotCgst As Decimal = 0
        Dim TotSgst As Decimal = 0
        Dim TotCess As Decimal = 0
        If dtB2B.Rows.Count > 0 Then
            With dtB2B.Rows(0)
                TotRcCnt = Val(.Item("CNT").ToString)
                TotRcVal = Format(Val(.Item("TAXABLE").ToString), "0.00")
                TotIgst = Format(Val(.Item("IGSTTAX").ToString), "0.00")
                TotCgst = Format(Val(.Item("CGSTTAX").ToString), "0.00")
                TotSgst = Format(Val(.Item("SGSTTAX").ToString), "0.00")
                TotCess = Format(Val(.Item("CESSAMOUNT").ToString), "0.00")
            End With
        End If

        strSql = vbCrLf + "SELECT 'B2B/ B2BA/ CDNR/ CDNRA Section Summary' AS A ,CONVERT(VARCHAR(50),NULL) AS B,1 AS TYPE,'T' AS COLHEAD,1 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Return Section' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,1 AS TYPE,'' AS COLHEAD,2 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Record Count' AS A," & TotRcCnt & " AS B,1 AS TYPE,'' AS COLHEAD,3 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total records value' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,1 AS TYPE,'' AS COLHEAD,4 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total IGST' AS A,CONVERT(NUMERIC(15,2)," & TotIgst & ") AS B,1 AS TYPE,'' AS COLHEAD,5 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total CGST' AS A,CONVERT(NUMERIC(15,2)," & TotCgst & ") AS B,1 AS TYPE,'' AS COLHEAD,6 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total SGST' AS A,CONVERT(NUMERIC(15,2)," & TotSgst & ") AS B,1 AS TYPE,'' AS COLHEAD,7 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Cess' AS A,CONVERT(NUMERIC(15,2)," & TotCess & ") AS B,1 AS TYPE,'' AS COLHEAD,8 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total taxable value of records' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,1 AS TYPE,'' AS COLHEAD,9 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Counter Party Summary' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,1 AS TYPE,'' AS COLHEAD,10 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT NULL AS A,CONVERT(NUMERIC(15,2),NULL) AS B,1 AS TYPE,'' AS COLHEAD,0 AS RESULT"

        Sql = vbCrLf + "SELECT COUNT(*)CNT"
        Sql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
        Sql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
        Sql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT "
        Sql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
        Sql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT "
        Sql += vbCrLf + "WHERE A='R- Regular B2C Invoices'"
        Sql += vbCrLf + "AND TOTALVALUE>=250000"
        dtB2B = New DataTable
        da = New OleDbDataAdapter(Sql, cn)
        da.Fill(dtB2B)
        If dtB2B.Rows.Count > 0 Then
            With dtB2B.Rows(0)
                TotRcCnt = Val(.Item("CNT").ToString)
                TotRcVal = Format(Val(.Item("TAXABLE").ToString), "0.00")
                TotIgst = Format(Val(.Item("IGSTTAX").ToString), "0.00")
                TotCgst = Format(Val(.Item("CGSTTAX").ToString), "0.00")
                TotSgst = Format(Val(.Item("SGSTTAX").ToString), "0.00")
                TotCess = Format(Val(.Item("CESSAMOUNT").ToString), "0.00")
            End With
        End If
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'B2CL / B2CLA Section Summary' AS A ,CONVERT(NUMERIC(15,2),NULL) AS B,2 AS TYPE,'T' AS COLHEAD,1 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Return Section' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,2 AS TYPE,'' AS COLHEAD,2 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Record Count' AS A," & TotRcCnt & " AS B,2 AS TYPE,'' AS COLHEAD,3 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total records value' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,2 AS TYPE,'' AS COLHEAD,4 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total IGST' AS A,CONVERT(NUMERIC(15,2)," & TotIgst & ") AS B,2 AS TYPE,'' AS COLHEAD,5 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total CGST' AS A,CONVERT(NUMERIC(15,2)," & TotCgst & ") AS B,2 AS TYPE,'' AS COLHEAD,6 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total SGST' AS A,CONVERT(NUMERIC(15,2)," & TotSgst & ") AS B,2 AS TYPE,'' AS COLHEAD,7 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Cess' AS A,CONVERT(NUMERIC(15,2)," & TotCess & ") AS B,2 AS TYPE,'' AS COLHEAD,8 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total taxable value of records' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,2 AS TYPE,'' AS COLHEAD,9 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Counter Party Summary' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,2 AS TYPE,'' AS COLHEAD,10 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT NULL AS A,CONVERT(NUMERIC(15,2),NULL) AS B,2 AS TYPE,'' AS COLHEAD,0 AS RESULT"

        Sql = vbCrLf + "SELECT COUNT(*)CNT"
        Sql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
        Sql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
        Sql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT "
        Sql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
        Sql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT "
        Sql += vbCrLf + "WHERE A='R- Regular B2C Invoices'"
        Sql += vbCrLf + "AND TOTALVALUE<250000"
        dtB2B = New DataTable
        da = New OleDbDataAdapter(Sql, cn)
        da.Fill(dtB2B)
        If dtB2B.Rows.Count > 0 Then
            With dtB2B.Rows(0)
                TotRcCnt = Val(.Item("CNT").ToString)
                TotRcVal = Format(Val(.Item("TAXABLE").ToString), "0.00")
                TotIgst = Format(Val(.Item("IGSTTAX").ToString), "0.00")
                TotCgst = Format(Val(.Item("CGSTTAX").ToString), "0.00")
                TotSgst = Format(Val(.Item("SGSTTAX").ToString), "0.00")
                TotCess = Format(Val(.Item("CESSAMOUNT").ToString), "0.00")
            End With
        End If
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'B2CS / B2CSA' AS A ,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'T' AS COLHEAD,1 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Return Section' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,2 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Record Count' AS A,CONVERT(NUMERIC(15,2)," & TotRcCnt & ") AS B,3 AS TYPE,'' AS COLHEAD,3 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total records value' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,3 AS TYPE,'' AS COLHEAD,4 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total IGST' AS A,CONVERT(NUMERIC(15,2)," & TotIgst & ") AS B,3 AS TYPE,'' AS COLHEAD,5 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total CGST' AS A,CONVERT(NUMERIC(15,2)," & TotCgst & ") AS B,3 AS TYPE,'' AS COLHEAD,6 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total SGST' AS A,CONVERT(NUMERIC(15,2)," & TotSgst & ") AS B,3 AS TYPE,'' AS COLHEAD,7 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Cess' AS A,CONVERT(NUMERIC(15,2)," & TotCess & ") AS B,3 AS TYPE,'' AS COLHEAD,8 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total taxable value of records' AS A,CONVERT(NUMERIC(15,2)," & TotRcVal & ") AS B,3 AS TYPE,'' AS COLHEAD,9 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Counter Party Summary' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,10 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT NULL AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,0 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'NIL Section Summary	' AS A ,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'T' AS COLHEAD,1 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Return Section' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,2 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Invoice Check sum value ' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,3 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Nil rated outward supplies' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,4 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Exempted outward supplies' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,5 AS RESULT"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 'Total Non GST outward supplies' AS A,CONVERT(NUMERIC(15,2),NULL) AS B,3 AS TYPE,'' AS COLHEAD,6 AS RESULT"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView)
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("TYPE").Visible = False
            .Columns("A").HeaderText = "A"
            .Columns("B").HeaderText = "B"
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Columns("B").Width = 100
        End With
        Dim TITLE As String = ""
        TITLE += " GST REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += "  AT " & chkCmbCostCentre.Text & ""
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()
    End Sub
    Private Sub SalesAbs()
        Try
            Prop_Sets()
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Me.Refresh()

            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR1"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkCountSumm.Checked Then
                strSql = " EXEC " & cnAdminDb & "..PROC_GSTR2"
                strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
                strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                strSql += vbCrLf + " ,@STATE = '" & CompanyState & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + "SELECT GSTNO,COUNT(*)CNT"
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
                strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
                strSql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT "
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
                strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT "
                strSql += vbCrLf + "WHERE A='R- Regular B2B Invoices'"
                strSql += vbCrLf + "GROUP BY GSTNO"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT GSTIN AS GSTNO,COUNT(*)CNT"
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
                strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
                strSql += vbCrLf + ",NULL CESSAMOUNT "
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
                strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT2 "
                strSql += vbCrLf + "GROUP BY GSTIN"
            ElseIf ChkStateSum.Checked Then
                strSql = vbCrLf + "SELECT CSTATE,COUNT(*)CNT"
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TAXABLE"
                strSql += vbCrLf + ",SUM(IGSTTAX)IGSTTAX,SUM(CGSTTAX)CGSTTAX,SUM(SGSTTAX)SGSTTAX"
                strSql += vbCrLf + ",SUM(CESSAMOUNT)CESSAMOUNT "
                strSql += vbCrLf + ",ISNULL(SUM(IGSTTAX),0)+ISNULL(SUM(CGSTTAX),0)+ISNULL(SUM(SGSTTAX),0) AS TOTALTAXABLE"
                strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPGSTRPT "
                strSql += vbCrLf + "GROUP BY CSTATE"
            ElseIf Chk6.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_6 ORDER BY TRANDATE,TRANNO"
            ElseIf Chk7.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_7"
            ElseIf ChkCRDR.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_8 ORDER BY TRANDATE,TRANNO,TRANTYPE"
            ElseIf Chk12.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_12 "
            ElseIf Chk9.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_9 "
            ElseIf Chk10.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_10 "
            ElseIf Chk11.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_11 "
            ElseIf Chk13.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_13 "
            ElseIf Chk14.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_14 "
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT ORDER BY INVDATE,INVNO"
            End If

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)
            FillGridGroupStyle_KeyNoWise(gridView)
            With gridView
                pnlGridHead.Visible = False
                .Columns("KEYNO").Visible = False
                If .Columns.Contains("TOTALVALUE") Then
                    .Columns("TOTALVALUE").Visible = False
                End If
                If Chk6.Checked Then
                    .Columns("BATCHNO").Visible = False
                    .Columns("STATE").HeaderText = "Recipient's State code"
                    .Columns("PNAME").HeaderText = "Name of the recipient"
                    .Columns("NATURE").HeaderText = "Goods/Services"
                    .Columns("HSN").HeaderText = "HSN/SAC"
                    .Columns("AMOUNT").HeaderText = "Taxable Value"
                    .Columns("DIFFERENTIAL").HeaderText = "POS(only if different from the location of recipient)"
                    .Columns("TAXPAID").HeaderText = "Tax on this Invoice is paid under provisional assessment"
                    .Columns("TRANNO").HeaderText = "No"
                    .Columns("TRANDATE").HeaderText = "Date"
                    .Columns("IGSTRATE").HeaderText = "Rate"
                    .Columns("IGSTTAX").HeaderText = "Amount"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                ElseIf Chk7.Checked Then
                    .Columns("NATURE").HeaderText = "Goods/Services"
                    .Columns("HSN").HeaderText = "HSN/SAC"
                    .Columns("IGSTRATE").HeaderText = "Rate"
                    .Columns("IGSTTAX").HeaderText = "Amount"
                    .Columns("CGSTRATE").HeaderText = "Rate"
                    .Columns("CGSTTAX").HeaderText = "Amount"
                    .Columns("SGSTRATE").HeaderText = "Rate"
                    .Columns("SGSTTAX").HeaderText = "Amount"
                    .Columns("STATE").HeaderText = "State code(Place of Supply)"
                    .Columns("AMOUNT").HeaderText = "Aggregate Taxable Value"
                    .Columns("TAXPAID").HeaderText = "Tax on this Invoice is paid under provisional assessment"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                ElseIf ChkCountSumm.Checked Then
                    .Columns("GSTNO").HeaderText = "Supplier TIN for B2B & CDN"
                    .Columns("CNT").HeaderText = "Total Record Count"
                    .Columns("TAXABLE").HeaderText = "Total records value"
                    .Columns("IGSTTAX").HeaderText = "Total IGST"
                    .Columns("CGSTTAX").HeaderText = "Total CGST"
                    .Columns("SGSTTAX").HeaderText = "Total SGST"
                    .Columns("CESSAMOUNT").HeaderText = "Total Cess"
                    .Columns("TOTALTAXABLE").HeaderText = "Total taxable value of records"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                ElseIf ChkStateSum.Checked Then
                    .Columns("CSTATE").HeaderText = "State Code"
                    .Columns("CNT").HeaderText = "Total Record Count"
                    .Columns("TAXABLE").HeaderText = "Total records value"
                    .Columns("IGSTTAX").HeaderText = "Total IGST"
                    .Columns("CGSTTAX").HeaderText = "Total CGST"
                    .Columns("SGSTTAX").HeaderText = "Total SGST"
                    .Columns("CESSAMOUNT").HeaderText = "Total Cess"
                    .Columns("TOTALTAXABLE").HeaderText = "Total taxable value of records"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                ElseIf Chk9.Checked Then
                    .Columns("A").HeaderText = ""
                    .Columns("NATURE").HeaderText = "Goods/Services"
                    .Columns("NILL").HeaderText = "Nil Rated"
                    .Columns("EXEM").HeaderText = "Exempted"
                    .Columns("NONGST").HeaderText = "Non GST supplies"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                ElseIf Chk10.Checked Then
                    If .Columns.Contains("PARTICULAR") Then .Columns("PARTICULAR").HeaderText = ""
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC"
                    If .Columns.Contains("TAXABLE") Then .Columns("TAXABLE").HeaderText = "Taxable Value"
                    If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("OINVNO") Then .Columns("OINVNO").HeaderText = "No"
                    If .Columns.Contains("OINVDATE") Then .Columns("OINVDATE").HeaderText = "Date"
                    If .Columns.Contains("NATURE") Then .Columns("NATURE").HeaderText = "Goods/Services"
                    If .Columns.Contains("INVALUE") Then .Columns("INVALUE").HeaderText = "Value"
                    If .Columns.Contains("SHIPBILLNO") Then .Columns("SHIPBILLNO").HeaderText = "No"
                    If .Columns.Contains("SHIPBILLDATE") Then .Columns("SHIPBILLDATE").HeaderText = "Date"
                    .Columns("TAXPAID").HeaderText = ""
                    pnlGridHead.Visible = True
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    .Columns("TAXPAID").Width = 100
                    funcHeaderNew()
                ElseIf Chk11.Checked Then
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = "GSTIN/UIN/Name of customer"
                    If .Columns.Contains("STATECODE") Then .Columns("STATECODE").HeaderText = "StateCode"
                    If .Columns.Contains("NO") Then .Columns("NO").HeaderText = "Document No."
                    If .Columns.Contains("DATE") Then .Columns("DATE").HeaderText = "Date Goods/Services"
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC of supply"
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Amount of advance received/ Value of Supply provided without raising a bill"
                    If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                    pnlGridHead.Visible = True
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    '.Columns("TAXPAID").Width = 100
                    funcHeaderNew()
                ElseIf ChkCRDR.Checked Then
                    pnlGridHead.Visible = True
                    .Columns("BATCHNO").Visible = False
                    .Columns("KEYNO").Visible = False
                    If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
                    If .Columns.Contains("NAMEOFSUPPLIER") Then .Columns("NAMEOFSUPPLIER").HeaderText = ""
                    If .Columns.Contains("NATURE") Then .Columns("NATURE").HeaderText = "Goods/Services"
                    If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "No"
                    If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Date"
                    If .Columns.Contains("OINVNO") Then .Columns("OINVNO").HeaderText = "No"
                    If .Columns.Contains("OINVDATE") Then .Columns("OINVDATE").HeaderText = "Date"
                    If .Columns.Contains("VALUE") Then .Columns("VALUE").HeaderText = "Value"
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC"
                    If .Columns.Contains("TAXABLE") Then .Columns("TAXABLE").HeaderText = "Taxable Value"
                    If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("ELIGIBLE") Then .Columns("ELIGIBLE").HeaderText = ""
                    If .Columns.Contains("STATE") Then .Columns("STATE").HeaderText = ""
                    If .Columns.Contains("DIFFERENTIAL") Then .Columns("DIFFERENTIAL").HeaderText = ""
                    If .Columns.Contains("NAMEOFSUPPLIER") Then .Columns("NAMEOFSUPPLIER").HeaderText = ""
                    If .Columns.Contains("TYPE") Then .Columns("TYPE").HeaderText = ""
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = ""
                    If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").HeaderText = "IGST"
                    If .Columns.Contains("TOTCGST") Then .Columns("TOTCGST").HeaderText = "CGST"
                    If .Columns.Contains("TOTSGST") Then .Columns("TOTSGST").HeaderText = "SGST"
                    If .Columns.Contains("ITCIGST") Then .Columns("ITCIGST").HeaderText = "IGST"
                    If .Columns.Contains("ITCCGST") Then .Columns("ITCCGST").HeaderText = "CGST"
                    If .Columns.Contains("ITCSGST") Then .Columns("ITCSGST").HeaderText = "SGST"
                    If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    If .Columns.Contains("STATE") Then .Columns("STATE").Width = 150
                    If .Columns.Contains("ELIGIBLE") Then .Columns("ELIGIBLE").Width = 150
                    If .Columns.Contains("DIFFERENTIAL") Then .Columns("DIFFERENTIAL").Width = 100
                    If .Columns.Contains("TYPE") Then .Columns("TYPE").Width = 85
                    funcHeaderNew()
                ElseIf Chk12.Checked Then
                    pnlGridHead.Visible = True
                    If .Columns.Contains("IGRATE") Then .Columns("IGRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGTAX") Then .Columns("IGTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGRATE") Then .Columns("CGRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGTAX") Then .Columns("CGTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGRATE") Then .Columns("SGRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGTAX") Then .Columns("SGTAX").HeaderText = "Amount"
                    If .Columns.Contains("RUNNO") Then .Columns("RUNNO").HeaderText = "Transaction id (A number assigned by the system when tax was paid)"
                    If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = ""
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    funcHeaderNew()
                ElseIf Chk13.Checked Then
                    pnlGridHead.Visible = True
                    If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("NO") Then .Columns("NO").HeaderText = "Invoice No."
                    If .Columns.Contains("DATE") Then .Columns("DATE").HeaderText = "Date"
                    If .Columns.Contains("ID") Then .Columns("ID").HeaderText = "Merchant ID issued by ecommerce operator"
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = "GSTIN of ecommerce portal"
                    If .Columns.Contains("VALUE") Then .Columns("VALUE").HeaderText = "GrossValue of supplies "
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Taxable Value"
                    If .Columns.Contains("NATURE") Then .Columns("NATURE").HeaderText = "Goods(G)/Services (S)"
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    funcHeaderNew()
                ElseIf Chk14.Checked Then
                    .Columns("NO").HeaderText = "S.No."
                    .Columns("SLNO").HeaderText = "No. Series number of invoices"
                    .Columns("FROMNO").HeaderText = "From"
                    .Columns("TONO").HeaderText = "To"
                    .Columns("TOTAL").HeaderText = "Total number of invoices"
                    .Columns("NOFCANCEL").HeaderText = "Number of cancelled invoices"
                    .Columns("NETTOTAL").HeaderText = "Net Number of invoices issued"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                Else
                    .Columns("BATCHNO").Visible = False
                    If .Columns.Contains("COUNTRY") Then .Columns("COUNTRY").Visible = False
                    .Columns("A").HeaderText = "Invoice Type"
                    .Columns("B").HeaderText = "Is this an Original Invoice or Ammendment Invoice"
                    .Columns("C").HeaderText = "Is this Advance received without you having given an Invoice  for this supply"
                    .Columns("D").HeaderText = "Is this Advance amount received in earlier tax period and adjusted against the supplies being shown in this tax period"
                    .Columns("E").HeaderText = "Is this a Zero Rated Supply or Deemed Export Including Exports out of India, Supplies to SEZ unit/ and SEZ developer, Deemed Exports"
                    .Columns("F").HeaderText = "Is this supply related to E-Commerce Operator?"
                    .Columns("G").HeaderText = "Is this a Credit Note / Debit Note /Refund Voucher?"
                    .Columns("NAME").HeaderText = "Name of Receipient"
                    .Columns("NATURE").HeaderText = "Nature of Supply"
                    .Columns("INVNO").HeaderText = "Invoice number"
                    .Columns("INVDATE").HeaderText = "Invoice date"
                    .Columns("GSTNO").HeaderText = "GSTIN / UIN of recipient"
                    .Columns("CSTATE").HeaderText = "State of receipient of Invoice"
                    .Columns("DSTATE").HeaderText = "State of supply of goods / services (may be different than state of receipient)"
                    .Columns("RCHARGE").HeaderText = "Is reverse charge mechanism applicable?"
                    .Columns("PROV_ASS").HeaderText = "Is Provisional assessment Applicable"
                    .Columns("INVALUE").HeaderText = "Invoice Value"
                    .Columns("OINVNO").HeaderText = "Original Invoice Number"
                    .Columns("OINVDATE").HeaderText = "Original Invoice Date"
                    .Columns("SNO").HeaderText = "Sr No for Item Details"
                    .Columns("TAXABLE").HeaderText = "Taxable value"
                    .Columns("RATE").HeaderText = "Rate"
                    .Columns("IGSTRATE").HeaderText = "IGST Rate"
                    .Columns("IGSTTAX").HeaderText = "IGST Tax Amount"
                    .Columns("CGSTRATE").HeaderText = "CGST Rate"
                    .Columns("CGSTTAX").HeaderText = "CGST Tax Amount"
                    .Columns("SGSTRATE").HeaderText = "SGST Rate"
                    .Columns("SGSTTAX").HeaderText = "SGST Tax Amount"
                    .Columns("CESSRATE").HeaderText = "Cess Rate"
                    .Columns("CESSAMOUNT").HeaderText = "Cess Amount"
                    .Columns("HSN").HeaderText = "HSN or SAC of Goods or Services"
                    .Columns("ITEMDESC").HeaderText = "Description of goods sold"
                    .Columns("UQC").HeaderText = "UQC (Unit of Measure) of goods sold"
                    .Columns("QTYOFGOODS").HeaderText = "Quantity of goods sold"
                    .Columns("GSTIN_ECOM").HeaderText = "GSTIN of E-commerce Operator (if applicable)"
                    .Columns("ADVREC").HeaderText = "Amount of Advance received"
                    .Columns("ADVADJ").HeaderText = "Amount of Advance to be adjusted"
                    .Columns("PAYMENTOFGST").HeaderText = "With/Without payment of GST"
                    .Columns("SHIPBILLNO").HeaderText = "Shipping Bill No. or Bill of Export No"
                    .Columns("SHIPBILLDATE").HeaderText = "Shipping Bill Date. or Bill of Export Date"
                    .Columns("CRVOUCHER").HeaderText = "Credit or Debit or Refund Voucher?"
                    .Columns("CRNO").HeaderText = "Credit / Debit Note Number"
                    .Columns("CRDATE").HeaderText = "Credit / Debit Note Date"
                    .Columns("INVDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("OINVDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("CRDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    If .Columns.Contains("ADVRECDATE") Then .Columns("ADVRECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    If .Columns.Contains("ADVRECDATE") Then .Columns("ADVRECDATE").HeaderText = "Date of Advance received"
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End If
            End With

            Dim TITLE As String = ""
            TITLE += " GST REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub
    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                If ChkCRDR.Checked Then
                    .Columns.Add("GSTIN", GetType(String))
                    .Columns.Add("TYPE", GetType(String))
                    .Columns.Add("TRANNO~TRANDATE", GetType(String))
                    .Columns.Add("OINVNO~OINVDATE", GetType(String))
                    .Columns.Add("DIFFERENTIAL", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("ELIGIBLE", GetType(String))
                    .Columns.Add("TOTIGST~TOTCGST~TOTSGST", GetType(String))
                    .Columns.Add("ITCIGST~ITCCGST~ITCSGST", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                    .Columns("GSTIN").Caption = "GSTIN"
                    .Columns("TYPE").Caption = "Type of note (Debit / Credit)"
                    .Columns("TRANNO~TRANDATE").Caption = "Debit Note/Credit Note"
                    .Columns("OINVNO~OINVDATE").Caption = "Original Invoice"
                    .Columns("DIFFERENTIAL").Caption = "Differential Value(Plus or Minus)"
                    .Columns("CGSTRATE~CGSTTAX").Caption = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").Caption = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").Caption = "SGST"
                    .Columns("ELIGIBLE").Caption = "Eligibility of ITC "
                    .Columns("TOTIGST~TOTCGST~TOTSGST").Caption = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").Caption = "ITC available this month $"
                ElseIf Chk10.Checked Then
                    .Columns.Add("PARTICULAR", GetType(String))
                    .Columns.Add("INVNO~INVDATE~INVALUE~NATURE~HSN~TAXABLE", GetType(String))
                    .Columns.Add("SHIPBILLNO~SHIPBILLDATE", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("TAXPAID", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk11.Checked Then
                    .Columns.Add("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk13.Checked Then
                    .Columns.Add("NO~DATE~ID~GSTIN~VALUE~AMOUNT~NATURE~HSN", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("PLACE", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                Else
                    .Columns.Add("TRANNO~RUNNO", GetType(String))
                    .Columns.Add("IGRATE~IGTAX", GetType(String))
                    .Columns.Add("CGRATE~CGTAX", GetType(String))
                    .Columns.Add("SGRATE~SGTAX", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                    .Columns("TRANNO~RUNNO").Caption = "Invoice"
                    .Columns("IGRATE~IGTAX").Caption = "IGST"
                    .Columns("CGRATE~CGTAX").Caption = "CGST"
                    .Columns("SGRATE~SGTAX").Caption = "SGST"
                End If
            End With
            With gridViewHead
                .DataSource = dtMergeHeader
                If ChkCRDR.Checked Then
                    'gridViewHead.ColumnHeadersHeight = 10
                    pnlGridHead.Height = 44
                    .Columns("GSTIN").HeaderText = "GSTIN"
                    .Columns("TYPE").HeaderText = "Type of note (Debit/Credit) "
                    .Columns("TRANNO~TRANDATE").HeaderText = "Debit Note/Credit Note"
                    .Columns("OINVNO~OINVDATE").HeaderText = "Original Invoice"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                    .Columns("DIFFERENTIAL").HeaderText = "Differential Value (Plus or Minus)"
                    .Columns("ELIGIBLE").HeaderText = "Eligibility of ITC"
                    .Columns("TOTIGST~TOTCGST~TOTSGST").HeaderText = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").HeaderText = "ITC available this month $"
                ElseIf Chk10.Checked Then
                    pnlGridHead.Height = 44
                    .Columns("PARTICULAR").HeaderText = "Description"
                    .Columns("INVNO~INVDATE~INVALUE~NATURE~HSN~TAXABLE").HeaderText = "Invoice"
                    .Columns("SHIPBILLNO~SHIPBILLDATE").HeaderText = "Shipping bill/bill of export"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                    .Columns("TAXPAID").HeaderText = "Tax on this Invoice is paid under provisional assessment "
                ElseIf Chk11.Checked Then
                    .Columns("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT").HeaderText = " "
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                ElseIf Chk13.Checked Then
                    .Columns("NO~DATE~ID~GSTIN~VALUE~AMOUNT~NATURE~HSN").HeaderText = " "
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                Else
                    pnlGridHead.Height = 18
                    .Columns("TRANNO~RUNNO").HeaderText = "Invoice No."
                    .Columns("IGRATE~IGTAX").HeaderText = "IGST"
                    .Columns("CGRATE~CGTAX").HeaderText = "CGST"
                    .Columns("SGRATE~SGTAX").HeaderText = "SGST"
                End If
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                funcColWidth()
                gridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                .Columns("SCROLL").Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Function funcColWidth() As Integer
        With gridViewHead
            If ChkCRDR.Checked Then
                .Columns("GSTIN").Width = gridView.Columns("GSTIN").Width
                .Columns("TYPE").Width = gridView.Columns("TYPE").Width
                .Columns("TRANNO~TRANDATE").Width = gridView.Columns("TRANNO").Width + gridView.Columns("TRANDATE").Width
                .Columns("OINVNO~OINVDATE").Width = gridView.Columns("OINVNO").Width + gridView.Columns("OINVDATE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
                .Columns("ELIGIBLE").Width = gridView.Columns("ELIGIBLE").Width
                .Columns("DIFFERENTIAL").Width = gridView.Columns("DIFFERENTIAL").Width
                .Columns("TOTIGST~TOTCGST~TOTSGST").Width = gridView.Columns("TOTIGST").Width _
                    + gridView.Columns("TOTCGST").Width + gridView.Columns("TOTSGST").Width
                .Columns("ITCIGST~ITCCGST~ITCSGST").Width = gridView.Columns("ITCIGST").Width _
                    + gridView.Columns("ITCCGST").Width + gridView.Columns("ITCSGST").Width
            ElseIf Chk10.Checked Then
                .Columns("INVNO~INVDATE~INVALUE~NATURE~HSN~TAXABLE").Width = gridView.Columns("INVNO").Width _
                    + gridView.Columns("INVDATE").Width + gridView.Columns("INVALUE").Width + gridView.Columns("NATURE").Width _
                    + gridView.Columns("HSN").Width + gridView.Columns("TAXABLE").Width
                .Columns("SHIPBILLNO~SHIPBILLDATE").Width = gridView.Columns("SHIPBILLNO").Width + gridView.Columns("SHIPBILLDATE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
                .Columns("TAXPAID").Width = gridView.Columns("TAXPAID").Width
                .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            ElseIf Chk11.Checked Then
                .Columns("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT").Width = gridView.Columns("GSTIN").Width _
                    + gridView.Columns("STATECODE").Width + gridView.Columns("NO").Width + gridView.Columns("DATE").Width _
                    + gridView.Columns("HSN").Width + gridView.Columns("AMOUNT").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
            ElseIf Chk13.Checked Then
                .Columns("NO~DATE~ID~GSTIN~VALUE~AMOUNT~NATURE~HSN").Width = gridView.Columns("NO").Width _
                + gridView.Columns("DATE").Width + gridView.Columns("ID").Width + gridView.Columns("GSTIN").Width _
                + gridView.Columns("VALUE").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("NATURE").Width _
                + gridView.Columns("HSN").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
                .Columns("PLACE").Width = gridView.Columns("PLACE").Width
            Else
                .Columns("TRANNO~RUNNO").Width = gridView.Columns("TRANNO").Width + gridView.Columns("RUNNO").Width
                .Columns("IGRATE~IGTAX").Width = gridView.Columns("IGRATE").Width + gridView.Columns("IGTAX").Width
                .Columns("CGRATE~CGTAX").Width = gridView.Columns("CGRATE").Width + gridView.Columns("CGTAX").Width
                .Columns("SGRATE~SGTAX").Width = gridView.Columns("SGRATE").Width + gridView.Columns("SGTAX").Width
            End If
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        If ChkSumm.Checked Then
            funcSumm()
        Else
            SalesAbs()
        End If
        Exit Sub
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' "
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
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
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
        Dim obj As New frmGSTR1_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR1_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR1_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR1_Properties))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
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
        End If
    End Sub

    Private Sub ChkStateSum_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkStateSum.CheckedChanged
        If ChkStateSum.Checked Then
            ChkCountSumm.Checked = False
        End If
    End Sub

    Private Sub ChkCountSumm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkCountSumm.CheckedChanged
        If ChkCountSumm.Checked Then
            ChkStateSum.Checked = False
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridViewHead.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridViewHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnExcel_Upd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel_Upd.Click
        If gridView.Rows.Count = 0 Then MsgBox("No Record Found", MsgBoxStyle.Information) : Exit Sub
        Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        If xlApp Is Nothing Then
            MessageBox.Show("Excel is not properly installed!!")
            Return
        End If
        xlApp.DisplayAlerts = False
        Dim filePath As String = "e:\test.xlsx"
        Dim xlWorkBook As Excel.Workbook = xlApp.Workbooks.Open(filePath, 0, False, 5, "", "",
         False, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", True, False, 0,
         True, False, False)

        'Dim worksheets As Excel.Sheets = xlWorkBook.Worksheets
        'Dim xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        'xlNewSheet.Name = "dtGst6"
        'xlNewSheet.Cells(1, 1) = "New sheet content"


        Dim dtGst6 As New DataTable
        Dim dtGst7 As New DataTable
        Dim dtGst12 As New DataTable
        Dim dtGst9 As New DataTable
        Dim dtGst10 As New DataTable
        Dim dtGst11 As New DataTable
        Dim dtGst13 As New DataTable
        Dim dtGst14 As New DataTable
        Dim dtGstCRDR As New DataTable
        Dim dtGst As New DataTable
        Dim gridTemp As New DataGridView
        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_6 ORDER BY TRANDATE,TRANNO"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst6)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_7"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst7)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_8 ORDER BY TRANDATE,TRANNO,TRANTYPE"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGstCRDR)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_12 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst12)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_9 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst9)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_10 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst10)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_11 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst11)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_13 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst13)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1_14 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst14)

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT ORDER BY INVDATE,INVNO"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGst)


        Dim worksheets As Excel.Sheets = xlWorkBook.Worksheets
        Dim xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        Dim colIndex As Integer = 0
        Dim rowIndex As Integer = 0

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst6"
        colIndex = 0
        rowIndex = 0
        gridTemp.DataSource = Nothing
        gridTemp.DataSource = dtGst6
        For Each dc As DataColumn In dtGst6.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst6.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst6.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst7"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst7.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst7.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst7.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst12"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst12.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst12.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst12.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst9"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst9.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst9.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst9.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst10"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst10.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst10.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst10.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next


        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst11"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst11.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst11.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst11.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst13"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst13.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst13.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst13.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst14"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst14.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst14.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst14.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next


        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGstCRDR"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGstCRDR.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGstCRDR.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGstCRDR.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next

        xlNewSheet = DirectCast(worksheets.Add(worksheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
        xlNewSheet.Name = "dtGst"
        colIndex = 0
        rowIndex = 0
        For Each dc As DataColumn In dtGst.Columns
            colIndex = colIndex + 1
            xlNewSheet.Cells(1, colIndex) = dc.ColumnName
        Next
        For Each dr As DataRow In dtGst.Rows
            rowIndex = rowIndex + 1
            colIndex = 0
            For Each dc As DataColumn In dtGst.Columns
                colIndex = colIndex + 1
                xlNewSheet.Cells(rowIndex + 1, colIndex) = dr(dc.ColumnName)
            Next
        Next



        xlNewSheet.Select()
        xlWorkBook.Save()
        xlWorkBook.Close()

        releaseObject(xlNewSheet)
        releaseObject(worksheets)
        releaseObject(xlWorkBook)
        releaseObject(xlApp)

        MessageBox.Show("New Worksheet Created!")
    End Sub



    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

End Class

Public Class frmGSTR1_Properties
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
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
End Class