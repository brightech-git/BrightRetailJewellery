Imports System.Data.OleDb
Public Class frmDiaSalesRpt
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtt As DataTable

    Private Sub frmDiaSalesRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDiaSalesRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
    End Sub
    Function funcNew()
        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCenter.Enabled = True
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCenter, True)
            cmbCostCenter.Items.Add("ALL")
            cmbCostCenter.SelectedIndex = 0
            cmbCostCenter.Text = "ALL"
        Else
            cmbCostCenter.Enabled = False
        End If
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY ITEMNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(cmbItemName, dt, "ITEMNAME", True)
        cmbItemName.Items.Add("ALL")
        cmbItemName.Text = "ALL"
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY COMPANYNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dt, "COMPANYNAME", , strCompanyName)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        chkCmbCompany.Select()
    End Function

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click, ViewToolStripMenuItem.Click
        Try
            gridView.DataSource = Nothing
            gridView.Refresh()
            AutoResizeToolStripMenuItem.Checked = False
            strSql = " EXEC " & cnAdminDb & "..PROC_DIAMONDSALES "
            strSql += vbCrLf + " @COMPANYNAME ='" & chkCmbCompany.Text & "' "
            strSql += vbCrLf + " ,@COSTNAME =" & IIf(cmbCostCenter.Text = "" Or cmbCostCenter.Text = "ALL", "'ALL'", GetQryString(cmbCostCenter.Text)) & ""
            strSql += vbCrLf + " ,@DBNAME ='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ITEMNAME =" & IIf(cmbItemName.Text = "" Or cmbItemName.Text = "ALL", "'ALL'", GetQryString(cmbItemName.Text)) & ""
            strSql += vbCrLf + " ,@SUBITEMNAME =" & IIf(chkcmbSubItemName.Text = "" Or chkcmbSubItemName.Text = "ALL", "'ALL'", GetQryString(chkcmbSubItemName.Text)) & ""
            strSql += vbCrLf + " ,@SYSID ='" & systemId & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPDIASALES" & systemId & " ORDER BY RESULT,TRANDATE,TRANNO"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            Dim dt As DataTable
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    gridStyle()
                    .Focus()
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End With
            Else
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
                btnNew.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Function gridStyle()
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("ITEMNAME").Width = 220
        gridView.Columns("DIAPCS").HeaderText = "DIAMOND_PCS"
        gridView.Columns("DIAWT").HeaderText = "DIAMOND_WT"
        gridView.Columns("DIAAMT").HeaderText = "DIAMOND_AMT"
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "MM/dd/yyyy"
        With gridView
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
                If col.Name <> "TRANDATE" And col.Name <> "ITEMNAME" Then
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
        End With
        For Each Gv As DataGridViewRow In gridView.Rows
            With Gv
                Select Case .Cells("RESULT").Value.ToString
                    Case "1"
                        .DefaultCellStyle = reportTotalStyle
                End Select
            End With
        Next
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click, ExportToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "DIAMOND SALES REPORT", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub cmbItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbItemName.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If cmbItemName.Text = "" Or cmbItemName.Text = "ALL" Then
                    If cmbItemName.Text = "" Then cmbItemName.Text = "ALL"
                    strSql = " SELECT 'ALL' SUBITEMNAME,0 SUBITEMID,1 RESULT"
                Else
                    strSql = " SELECT 'ALL' SUBITEMNAME,0 SUBITEMID,1 RESULT"
                    strSql += " UNION ALL"
                    strSql += " SELECT SUBITEMNAME,SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST  "
                    strSql += " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text.ToString & "' ) "
                    strSql += " ORDER BY RESULT,SUBITEMNAME"
                End If
                Dim DT As DataTable
                DT = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DT)
                BrighttechPack.GlobalMethods.FillCombo(chkcmbSubItemName, DT, "SUBITEMNAME", , "ALL")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, PrintToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "DIAMOND SALES REPORT", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
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