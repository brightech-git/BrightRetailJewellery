Imports System.Data.OleDb
Public Class frmItemWiseReport
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGrid As New DataSet
    Dim dt As DataTable
    Dim SelectedCompanyId As String
    Dim dtCostCentre As New DataTable
    Dim strTitle As String

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, strTitle, gridView, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, strTitle, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub frmItemWiseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmItemWiseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        LoadCompany(chkLstCompany)
        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub LoadMetal()
        Try
            strSql = "select 'ALL' METALID,'ALL' METALNAME"
            strSql += vbCrLf + "union"
            strSql += vbCrLf + "select METALID,METALNAME from " & cnAdminDb & "..METALMAST where 1=1 and ACTIVE = 'Y'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            cmbMetal.DataSource = Nothing
            cmbMetal.DataSource = dt
            cmbMetal.DisplayMember = "METALNAME"
            cmbMetal.ValueMember = "METALID"
            cmbMetal.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        btnView_Search.Enabled = True
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Focus()
        LoadMetal()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        Dim selectcostid As String = IIf(chkcmbcostcentre.Text <> "ALL", GetSelectedCostId(chkcmbcostcentre, False), "ALL")
        gridView.DataSource = Nothing
        Me.Refresh()
        btnView_Search.Enabled = False
        If selectcostid = "''" Then selectcostid = "ALL"
        strSql = " EXEC " & cnAdminDb & "..RPT_ITEMWISE_NEW"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COSTID = '" & selectcostid & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSum.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@METALID = '" & cmbMetal.SelectedValue & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim dtGrid As New DataTable
        If rbtSum.Checked Then
            strSql = "SELECT *  FROM TEMPTABLEDB..TEMPISSUE ORDER BY TYPE,COUNTER,RESULT"
        Else
            strSql = "SELECT *  FROM TEMPTABLEDB..TEMPISSUE ORDER BY TYPE,COUNTER,ITEMNAME,RESULT"
        End If
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridView.DataSource = Nothing
        gridView.DataSource = DtGrid
        FormatGridColumns(gridView, False, , , False)
        FillGridGroupStyle_KeyNoWise(gridView)
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("TYPE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            .Columns("COUNTER").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        btnView_Search.Enabled = True
        strTitle = "ITEM WISE SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class