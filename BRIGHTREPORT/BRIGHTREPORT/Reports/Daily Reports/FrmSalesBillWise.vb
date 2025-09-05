Imports System.Data.OleDb
Public Class FrmSalesBillWise
    Dim strSql As String
    Dim dtCompany As New DataTable
    Dim cmd As OleDbCommand
    Dim DS As DataSet
    Dim DT As DataTable
    Dim da As OleDbDataAdapter

    Private Sub FrmSalesBillWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FrmSalesBillWise_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FuncLoad()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        FuncLoad()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub
    Private Sub FuncLoad()
        gridView.DataSource = Nothing
        lblTitle.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        txtbillno_NUM.Text = ""
        strSql = " SELECT 'ALL' COMPANYNAME,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        objGPack.FillCombo(strSql, CmbCompany, True)
        CmbCompany.Text = "ALL"
        lblTitle.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim SelectedCompid As String = ""
        If CmbCompany.Text <> "ALL" Then
            strSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME='" & CmbCompany.Text & "'"
            SelectedCompid = objGPack.GetSqlValue(strSql, "COMPANYID")
        End If
        If SelectedCompid = "" Then SelectedCompid = "ALL"
        If rbtFormat1.Checked Then
            strSql = " EXEC " & cnAdminDb & "..RPT_SALESBILLDETAIL"
            strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + ",@FROMDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + ",@TODATE='" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + ",@STRCOMPANYID='" & SelectedCompid & "'"
        Else
            strSql = " EXEC " & cnAdminDb & "..RPT_SALESBILLDETAIL_ED"
            strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + ",@FROMDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + ",@TODATE='" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + ",@STRCOMPANYID='" & SelectedCompid & "'"
        End If
        Dim ds As New DataSet
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds)
        dt = ds.Tables(0)
        If ds.Tables(0).Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            GridViewFormat()
            lblTitle.Visible = True
            lblTitle.Text = "SALES DETAIL REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            lblTitle.Visible = False
            dtpFrom.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub GridViewFormat()
        gridView.Columns("TYPE").Visible = False
        gridView.Columns("SNO").Visible = False
        gridView.Columns("BATCHNO").Visible = False
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("DIARATE").HeaderText = "DIAMOND PER CARAT"
        gridView.Columns("DIAPCS").HeaderText = "DIAMOND PIECES"
        gridView.Columns("DIAWT").HeaderText = "DIAMOND WEIGHT"
        gridView.Columns("STNRATE").HeaderText = "STONE PRICE"
        gridView.Columns("STNPCS").HeaderText = "STONE PIECES"
        gridView.Columns("STNWT").HeaderText = "STONE WEIGHT"
        gridView.Columns("NAME").HeaderText = "CUSTOMER NAME"
        If gridView.Columns.Contains("LOYALITYNO") Then gridView.Columns("LOYALITYNO").HeaderText = "LOYALITY NUMBER"
        'gridView.Columns("PREVILEGEID").HeaderText = "LOYALITY NO"

        'For Each gv As DataGridViewRow In gridView.Rows
        '    With gv
        '        Select Case .Cells("RESULT").Value.ToString
        '            Case "2"
        '                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '                .DefaultCellStyle.BackColor = Color.LemonChiffon
        '        End Select
        '    End With
        'Next
        FormatGridColumns(gridView, False, False, True, False)
        AutoResizeToolStripMenuItem.Checked = True
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
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

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class