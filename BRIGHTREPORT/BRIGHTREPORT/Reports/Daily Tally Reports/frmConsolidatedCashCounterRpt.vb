Imports System.Data.OleDb
Public Class frmConsolidatedCashCounterRpt
#Region " Variable"
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As DataTable
#End Region

#Region "Form Load"
    Private Sub frmConsolidatedCashCounterRpt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub

    Private Sub frmConsolidatedCashCounterRpt_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        End If
    End Sub
#End Region

#Region "Button Event"
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridView_Own.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CONSOLIDATION CASH COUNTER From " & dtpFrom.Text & " To " & dtpTo.Text, gridView_Own, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridView_Own.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CONSOLIDATION CASH COUNTER From " & dtpFrom.Text & " To " & dtpTo.Text, gridView_Own, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView_Own.DataSource = Nothing
    End Sub

    Private Sub btnView_Search_Click(sender As Object, e As EventArgs) Handles btnView_Search.Click


        Dim ObjGrouper As BrighttechPack.DataGridViewGrouper

        strsql = ""
        strsql += vbCrLf + "/*Receipt*/"
        strsql += vbCrLf + " SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + " ,'SALES' PARTICULAR"
        strsql += vbCrLf + ", SUM(AMOUNT+TAX) TOTAL"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + " AND TRANTYPE IN ('SA')"
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + " GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + ",'ADVANCE RECIEPTS' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING  AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'R'"
        strsql += vbCrLf + "AND PAYMODE = 'AR'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "AND ISNULL(TRANTYPE,'')<>'RECEIVED'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + ",'CREDIT RECIEPTS' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'R'"
        strsql += vbCrLf + "AND PAYMODE = 'DR'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + ",'OTHER RECIEPTS' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'R'"
        strsql += vbCrLf + "AND PAYMODE = 'OR'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + ",'MISC RECIEPTS' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'R'"
        strsql += vbCrLf + "AND PAYMODE = 'MR'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + ",'REPAIR RECIEPTS' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'R'"
        strsql += vbCrLf + "AND PAYMODE = 'RE'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + " , 'SAVINGS SCHEME' PARTICULAR "
        strsql += vbCrLf + " , SUM(AMOUNT) AMOUNT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A WHERE"
        strsql += vbCrLf + "TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "AND PAYMODE IN ('SS','CB','CZ','CG','CD')"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + " SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME"
        strsql += vbCrLf + " ,'Customer Cheques Returned'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'C' "
        strsql += vbCrLf + "AND PAYMODE = 'JE'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + " ,'Cheques Issued to Customer'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'C' "
        strsql += vbCrLf + "AND PAYMODE = 'CH'"
        strsql += vbCrLf + "AND ISNULL(FLAG,'') = 'C'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + " ,'RGTS/NEFT Made To Customer'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'C' "
        strsql += vbCrLf + "AND PAYMODE = 'CH'"
        strsql += vbCrLf + "AND ISNULL(FLAG,'') <> 'C'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "/*Receipt*/"

        strsql += vbCrLf + "/*Payment*/"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + " ,'Credit & Debit Cards'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT "
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'D' "
        strsql += vbCrLf + "AND PAYMODE = 'CC'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME  "
        strsql += vbCrLf + " ,'Cheques & Dds'  PARTICULAR"
        strsql += vbCrLf + ", SUM(AMOUNT) AMOUNT "
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'D' "
        strsql += vbCrLf + "AND PAYMODE = 'CH'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME  "
        strsql += vbCrLf + " ,'Sales Return'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX) STOTAL"
        strsql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND TRANTYPE IN ('SR')"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + " ,'Customer Purchase'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX) STOTAL"
        strsql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND TRANTYPE IN ('PU')"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + ",'Advance Adjusted' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'P'"
        strsql += vbCrLf + "AND PAYMODE = 'AA'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + ",'ADVANCE REPAID' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND RECPAY  = 'P'"
        strsql += vbCrLf + "AND PAYMODE = 'AP'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME  "
        strsql += vbCrLf + " ,'Cheque Return in Reciepts'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT "
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'C' "
        strsql += vbCrLf + "AND PAYMODE = 'JE'"
        strsql += vbCrLf + "AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "')"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME  "
        strsql += vbCrLf + " ,'Cheque Return in Sales'  PARTICULAR"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND TRANMODE = 'C' "
        strsql += vbCrLf + "AND PAYMODE = 'JE'"
        strsql += vbCrLf + "AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "')"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME "
        strsql += vbCrLf + ",'Credit Bills In Sales' PARTICULAR "
        strsql += vbCrLf + ",SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND PAYMODE = 'DP'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"
        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + "SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =A.COSTID) COSTNAME  "
        strsql += vbCrLf + " ,'Scheme Adjusted'  PARTICULAR"
        strsql += vbCrLf + ",SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AMOUNT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'  "
        strsql += vbCrLf + "AND PAYMODE IN ('CT')"
        strsql += vbCrLf + "AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "')"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') <> 'Y'"
        strsql += vbCrLf + "GROUP BY COSTID"

        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim dtSource As New DataTable
            dtSource = dt.Copy
            'dtSource.Columns.Add("KEYNO", GetType(Integer))
            'dtSource.Columns("KEYNO").AutoIncrement = True
            'dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            'dtSource.Columns("KEYNO").AutoIncrementStep = 1
            ObjGrouper = New BrighttechPack.DataGridViewGrouper(gridView_Own, dtSource)
            ObjGrouper.pColumns_Group.Add("PARTICULAR")
            ObjGrouper.pColumns_Sum.Add("TOTAL")
            ObjGrouper.pColName_Particular = "PARTICULAR"
            ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"

            ObjGrouper.pIssSort = False
            ObjGrouper.GroupDgv()
            Dim ind As Integer = gridView_Own.RowCount - 1
            CType(gridView_Own.DataSource, DataTable).Rows.Add()
            gridView_Own.Columns("PARTICULAR").Visible = True
            Dim dtNew As New DataTable
            dtNew = gridView_Own.DataSource
            For i As Integer = 0 To dtNew.Rows.Count - 1
                If dtNew.Rows(i).Item("COLHEAD").ToString = "" Then
                    dtNew.Rows(i).Item("PARTICULAR") = dtNew.Rows(i).Item("COSTNAME").ToString
                End If
            Next
            With gridView_Own
                .DataSource = Nothing
                .DataSource = dtNew
                gridView_Own.Columns("COLHEAD").Visible = False
                gridView_Own.Columns("COSTNAME").Visible = False
                FormatGridColumns(gridView_Own, False, False, True, False)
                gridAutoSize(gridView_Own)
                For i As Integer = 0 To gridView_Own.Rows.Count - 1
                    If gridView_Own.Rows(i).Cells("COLHEAD").Value.ToString = "S" Then
                        gridView_Own.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                    ElseIf gridView_Own.Rows(i).Cells("COLHEAD").Value.ToString = "T" Then
                        gridView_Own.Rows(i).DefaultCellStyle.BackColor = Color.LightPink
                    ElseIf gridView_Own.Rows(i).Cells("COLHEAD").Value.ToString = "G" Then
                        gridView_Own.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                    End If
                Next
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            gridView_Own.DataSource = Nothing
        End If
    End Sub
#End Region

#Region " Gridview Event"
    Private Sub gridAutoSize(ByVal dv As DataGridView)
        With dv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Sub
#End Region
End Class