Imports System.Data.OleDb
Public Class frmSalesReturn_new
    Dim strSql As String = Nothing
    Dim dtTemp As New DataTable
    Dim ftrIssue As String
    Dim ftrReceipt As String
    Dim ftrStoneStr As String
    Dim dsGridView As New DataSet
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtGridView As New DataTable
    Dim dt As New DataTable

    Private Sub frmSalesReturn_new_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Me.Cursor = Cursors.WaitCursor
        gridView.DataSource = Nothing
        Dim CAT As String = GetSqlValue(cn, "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbcategory.Text & "'")
        strSql = "IF OBJECT_ID('TEMPTABLEDB..COUNTERSRTN','U')IS NOT NULL DROP TABLE  TEMPTABLEDB..COUNTERSRTN "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "SELECT "
        strSql += vbCrLf + "CONVERT(VARCHAR(15),TRANDATE,103)TRANDATE,GRSWT,NETWT,CONVERT(NUMERIC(15,2),RATE)RATE,AMOUNT,TAX,NETAMT,"
        If rbtdetail.Checked Then
            strSql += vbCrLf + "(SELECT  EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID =X.EMPID)SALESMAN ,"
            strSql += vbCrLf + "(SELECT  ALIASNAME  FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID =X.EMPID)ALIASMAN ,"
            strSql += vbCrLf + "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =X.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " CONVERT(NUMERIC(15,2),PERCENTAGE)PERCENTAGE,"
        End If
        strSql += vbCrLf + " (SELECT CATNAME  FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE  =X.CATCODE )CATEGORY"
        If rbtCounter.Checked Then
            strSql += vbCrLf + ",(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID =X.ITEMCTRID)ITEMCOUNTER "
        End If
        strSql += vbCrLf + " ,'1'AS RESULT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..COUNTERSRTN "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT TRANDATE ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,RATE,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(AMOUNT+TAX)NETAMT "
        If rbtSummary.Checked Then
            strSql += vbCrLf + " ,CATCODE"
        ElseIf rbtdetail.Checked Then
            strSql += vbCrLf + ",EMPID,ITEMID,CATCODE "
        End If
        If rbtCounter.Checked Then
            strSql += vbCrLf + " ,CATCODE,ITEMCTRID"
        End If
        If rbtdetail.Checked Then
            strSql += vbCrLf + ",  CASE WHEN SUM(GRSWT) <> 0 THEN (SUM(AMOUNT)/SUM(GRSWT)) END PERCENTAGE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & " ..RECEIPT  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(CANCEL,'')='' AND TRANTYPE='SR'"
        If cmbcategory.Text <> "ALL" Then
            strSql += vbCrLf + " AND CATCODE='" & CAT & "'"
        End If
        strSql += vbCrLf + " GROUP BY TRANDATE ,RATE  "
        If rbtSummary.Checked Then
            strSql += vbCrLf + " ,CATCODE"
        ElseIf rbtdetail.Checked Then
            strSql += vbCrLf + ",EMPID ,ITEMID ,CATCODE "
        End If
        If rbtCounter.Checked Then
            strSql += vbCrLf + " ,CATCODE,ITEMCTRID"
        End If
        strSql += vbCrLf + ")"
        strSql += vbCrLf + "X"
        strSql += vbCrLf + "ORDER BY TRANDATE "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "ALTER TABLE TEMPTABLEDB..COUNTERSRTN ALTER COLUMN TRANDATE VARCHAR(25)"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If rbtCounter.Checked Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..COUNTERSRTN(TRANDATE,ITEMCOUNTER,GRSWT ,NETWT ,RESULT )"
            strSql += vbCrLf + "SELECT DISTINCT ITEMCOUNTER,ITEMCOUNTER,NULL AS GRSWT ,NULL  AS NETWT,0 AS RESULT FROM TEMPTABLEDB..COUNTERSRTN WHERE RESULT ='1'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..COUNTERSRTN(TRANDATE,ITEMCOUNTER ,GRSWT,NETWT,AMOUNT,TAX,NETAMT  ,RESULT )"
            strSql += vbCrLf + "Select  DISTINCT 'SUB TOTAL' AS ITEMCOUNTER, ITEMCOUNTER,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(NETAMT)NETAMT,2 AS RESULT FROM TEMPTABLEDB..COUNTERSRTN WHERE RESULT ='1'"
            strSql += vbCrLf + "GROUP BY ITEMCOUNTER"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..COUNTERSRTN(TRANDATE,ITEMCOUNTER,GRSWT,NETWT,AMOUNT,TAX,NETAMT ,RESULT )"
            strSql += vbCrLf + "SELECT 'GRAND TOTAL' AS TRANDATE, 'ZZZZ' AS ITEMCOUNTER,SUM(GRSWT)PCS,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(NETAMT)NETAMT,3 AS RESULT FROM TEMPTABLEDB..COUNTERSRTN WHERE RESULT ='1'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        If rbtCounter.Checked = False Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..COUNTERSRTN(TRANDATE,GRSWT,NETWT,AMOUNT,TAX,NETAMT ,RESULT )"
            strSql += vbCrLf + "SELECT 'GRAND TOTAL' AS TRANDATE, SUM(GRSWT)PCS,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(NETAMT)NETAMT,3 AS RESULT FROM TEMPTABLEDB..COUNTERSRTN WHERE RESULT ='1'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        If rbtCounter.Checked Then
            strSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..COUNTERSRTN ORDER BY ITEMCOUNTER, RESULT "
        Else
            strSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..COUNTERSRTN ORDER BY  RESULT "
        End If

        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count - 1 > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            GridViewFormat()
            Dim title As String
            title = " SALES RETURN  FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If rbtCounter.Checked Then
                title = title + " COUNTER WISE"
            ElseIf rbtSummary.Checked Then
                title = title + " SUMMARY VIEW"
            Else
                title = title + " DETAIL VIEW"
            End If
            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            lblTitle.Visible = True
            strSql = " SELECT MAX(TRANNO) TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE IN ('SR') AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND COMPANYID='" & cnCompanyId & "' AND COSTID='" & cnCostId & "' "
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                lblEndBillNo.Text = objGPack.GetSqlValue(strSql, , "").ToString
            End If

            strSql = " SELECT MIN(TRANNO) TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE IN ('SR') AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND COMPANYID='" & cnCompanyId & "' AND COSTID='" & cnCostId & "' "
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                lblStartBillNo.Text = objGPack.GetSqlValue(strSql, , "").ToString
            End If

            strSql = " SELECT COUNT(*) TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE IN ('SR') AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND COMPANYID='" & cnCompanyId & "' AND COSTID='" & cnCostId & "' "
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                lblTotalBills.Text = objGPack.GetSqlValue(strSql, , "").ToString
            End If

            strSql = " SELECT COUNT(*) TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE IN ('SR') AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'' AND COMPANYID='" & cnCompanyId & "' AND COSTID='" & cnCostId & "' "
            If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
                lblCancelBills.Text = objGPack.GetSqlValue(strSql, , "").ToString
            End If
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            dtpFrom.Focus()
        End If
        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = GetServerDate()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If gridView.RowCount > 0 Then
            If e.KeyChar = Chr(Keys.Enter) Then
            ElseIf e.KeyChar = Chr(Keys.Escape) Then

            End If
            If UCase(e.KeyChar) = "X" Then
                btnExcel_Click(Me, New EventArgs)
            End If
            If UCase(e.KeyChar) = "P" Then
                btnPrint_Click(Me, New EventArgs)
            End If
        End If
    End Sub
    Private Sub GridViewFormat()
        gridView.Columns("TAX").HeaderText = "VAT"
        gridView.Columns("NETAMT").HeaderText = "NET AMT"
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("CATEGORY").Visible = False
        If rbtdetail.Checked Then
            gridView.Columns("ALIASMAN").HeaderText = "SALES MAN"
            gridView.Columns("SALESMAN").Visible = False
            gridView.Columns("ITEMNAME").Width = 150
            gridView.Columns("PERCENTAGE").HeaderText = "%"
            gridView.Columns("PERCENTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
        If rbtSummary.Checked Then
            gridView.Columns("TAX").Visible = False
            gridView.Columns("AMOUNT").Visible = False
        End If
        If rbtCounter.Checked Then
            gridView.Columns("ITEMCOUNTER").Visible = False
        End If
        gridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("NETAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For Each gv As DataGridViewRow In gridView.Rows
            With gv
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                Select Case .Cells("RESULT").Value
                    Case "0"
                        .DefaultCellStyle.BackColor = Color.LightGreen
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "2"
                        .DefaultCellStyle.BackColor = Color.Lavender
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "3"
                        .DefaultCellStyle.BackColor = Color.Wheat
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next


    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmSalesReturn_new_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''CATEGORY
        strSql = vbCrLf + " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CATNAME,CONVERT(VARCHAR,CATCODE),2 RESULT FROM " & cnAdminDb & "..CATEGORY"
        strSql += vbCrLf + " ORDER BY RESULT,CATNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        cmbcategory.Items.Clear()
        For Each Row As DataRow In dt.Rows
            cmbcategory.Items.Add(Row.Item("CATNAME").ToString)
        Next
        cmbcategory.Text = "ALL"
        btnNew_Click(Me, e)
    End Sub
    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
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
End Class

