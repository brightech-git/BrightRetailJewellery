Imports System.Data.OleDb
Imports System.Math
Public Class frmQCHMReport
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewToolStripMenuItem.Click
        Dim dtResult As New DataTable
        Dim y As Integer = 0
        Try
            Me.Cursor = Cursors.WaitCursor
            AutoReziseToolStripMenuItem.Checked = False
            gridView.DataSource = Nothing
            gridView.Refresh()
            strSql = $"	select CATNAME,TRANNO,PCS,GRSWT,BATCHNO from {cnStockDb}..RECEIPT a	"
            strSql &= vbCrLf & $"	inner join {cnAdminDb}..CATEGORY b on a.CATCODE = b.CATCODE 	"
            strSql &= vbCrLf & "	where 1=1 	"
            strSql &= vbCrLf & "	and a.TRANTYPE = 'RPU'	"
            strSql &= vbCrLf & $"	and a.TRANDATE <= '{dtpAsOn.Value.ToString("yyyy-MM-dd")}'	"
            strSql &= vbCrLf & "    order by a.Tranno"
            Dim receipt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(receipt)
            If Not receipt.Rows.Count > 0 Then
                Me.Cursor = Cursors.Default
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtResult = FilldtTemplate()
            For Each dr As DataRow In receipt.Rows
                Dim row As DataRow = dtResult.NewRow
                row("R_CATNAME") = dr("CATNAME") : row("R_TRANNO") = dr("TRANNO") : row("R_PCS") = dr("PCS") : row("R_GRSWT") = dr("GRSWT")
                dtResult.Rows.Add(row)
                row = dtResult.NewRow
                dtResult.Rows.Add(row)
                y = dtResult.Rows.Count - 1

                strSql = $"	select 'ISSUE' [TYPE],a.TRANNO,a.PCS,a.GRSWT,a.BATCHNO,e.EMPNAME from {cnStockDb}..QCISSUE a "
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..EMPMASTER e on e.EMPID = a.HMEMPID 	"
                strSql &= vbCrLf & "	where 1=1	"
                strSql &= vbCrLf & $"	and a.SNO = '{dr("BATCHNO")}'	"
                strSql &= vbCrLf & "	union	"
                strSql &= vbCrLf & "	select case when a.RETURNSTATUS = 'R' then 'RETURN' else 'RECEIPT' end [TYPE],	"
                strSql &= vbCrLf & $"	a.TRANNO,a.PCS,a.GRSWT,a.BATCHNO,e.EMPNAME from {cnStockDb}..QCRECEIPT a	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCISSUE b on b.BATCHNO = a.SNO 	"
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..EMPMASTER e on e.EMPID = a.HMEMPID 	"
                strSql &= vbCrLf & "	where 1=1	"
                strSql &= vbCrLf & $"	and b.SNO = '{dr("BATCHNO")}'	"
                Dim qc As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(qc)
                If qc.Rows.Count > 0 Then
                    For i As Integer = 1 To qc.Rows.Count - 1
                        Dim emptyRow As DataRow = dtResult.NewRow()
                        dtResult.Rows.Add(emptyRow)
                    Next
                    Dim z As Integer = 0
                    For x As Integer = y To dtResult.Rows.Count - 1
                        dtResult.Rows(x)("QC_TYPE") = qc.Rows(z)("TYPE") : dtResult.Rows(x)("QC_TRANNO") = qc.Rows(z)("TRANNO")
                        dtResult.Rows(x)("QC_PCS") = qc.Rows(z)("PCS") : dtResult.Rows(x)("QC_GRSWT") = qc.Rows(z)("GRSWT")
                        dtResult.Rows(x)("QC_EMPLOYEE") = qc.Rows(z)("EMPNAME")
                        z = z + 1
                    Next
                Else
                    Continue For
                End If

                strSql = $"	select 'ISSUE' [TYPE],c.TRANNO,c.PCS,c.GRSWT,d.HALLMARKNAME,e.EMPNAME from {cnStockDb}..QCISSUE a	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCRECEIPT b on b.SNO = a.BATCHNO	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCISSUE c on c.SNO = b.BATCHNO	"
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..HALLMARK d on d.HALLMARKID = c.HALLMARKID 	"
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..EMPMASTER e on e.EMPID = c.HMEMPID 	"
                strSql &= vbCrLf & "	where 1=1	"
                strSql &= vbCrLf & $"	and a.SNO = '{dr("BATCHNO")}'	"
                strSql &= vbCrLf & "	union	"
                strSql &= vbCrLf & "	select case when f.RETURNSTATUS = 'R' then 'RETURN' else 'RECEIPT' end [TYPE],	"
                strSql &= vbCrLf & $"	f.TRANNO,f.PCS,f.GRSWT,d.HALLMARKNAME,e.EMPNAME from {cnStockDb}..QCISSUE a	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCRECEIPT b on b.SNO = a.BATCHNO	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCISSUE c on c.SNO = b.BATCHNO	"
                strSql &= vbCrLf & $"	inner join {cnStockDb}..QCRECEIPT f on f.SNO = c.BATCHNO 	"
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..HALLMARK d on d.HALLMARKID = f.HALLMARKID 	"
                strSql &= vbCrLf & $"	inner join {cnAdminDb}..EMPMASTER e on e.EMPID = f.HMEMPID 	"
                strSql &= vbCrLf & "	where 1=1	"
                strSql &= vbCrLf & $"	and a.SNO = '{dr("BATCHNO")}'	"
                Dim hm As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(hm)
                If hm.Rows.Count > 0 Then
                    Dim xtraRows As Integer = hm.Rows.Count - qc.Rows.Count
                    If xtraRows >= 1 Then
                        For i As Integer = 0 To xtraRows - 1
                            Dim emptyRow As DataRow = dtResult.NewRow()
                            dtResult.Rows.Add(emptyRow)
                        Next
                    End If
                    Dim z As Integer = 0
                    For x As Integer = y To dtResult.Rows.Count - 1
                        dtResult.Rows(x)("HM_TYPE") = hm.Rows(z)("TYPE") : dtResult.Rows(x)("HM_TRANNO") = hm.Rows(z)("TRANNO")
                        dtResult.Rows(x)("HM_PCS") = hm.Rows(z)("PCS") : dtResult.Rows(x)("HM_GRSWT") = hm.Rows(z)("GRSWT")
                        dtResult.Rows(x)("HM_HALLMARK") = hm.Rows(z)("HALLMARKNAME") : dtResult.Rows(x)("HM_EMPLOYEE") = hm.Rows(z)("EMPNAME")
                        z = z + 1
                        If z = hm.Rows.Count Then Exit For
                    Next
                Else
                    Continue For
                End If
            Next
            gridView.DataSource = dtResult
            gridView.ReadOnly = True
            gridStyle()

            'With gridView
            '    .DataSource = dtResult
            '    .Dock = DockStyle.Fill
            '    .AllowUserToAddRows = False
            '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            'End With
            'gridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            Dim baseColor As Color
            For i As Integer = 0 To gridView.Columns.Count - 1
                Dim column As DataGridViewColumn = gridView.Columns(i)
                column.SortMode = DataGridViewColumnSortMode.NotSortable
                If column.Name.StartsWith("R_") Then
                    baseColor = colorPalette(0 Mod colorPalette.Length)
                    column.DefaultCellStyle.BackColor = GetLightColor(baseColor, 0.5)
                    column.DefaultCellStyle.ForeColor = Color.Black
                ElseIf column.Name.StartsWith("QC_") Then
                    baseColor = colorPalette(1 Mod colorPalette.Length)
                    column.DefaultCellStyle.BackColor = GetLightColor(baseColor, 0.5)
                    column.DefaultCellStyle.ForeColor = Color.Black
                ElseIf column.Name.StartsWith("HM_") Then
                    baseColor = colorPalette(2 Mod colorPalette.Length)
                    column.DefaultCellStyle.BackColor = GetLightColor(baseColor, 0.5)
                    column.DefaultCellStyle.ForeColor = Color.Black
                End If
            Next

            Me.Cursor = Cursors.Default
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Function GetLightColor(baseColor As Color, factor As Double) As Color
        Dim r As Integer = Math.Min(255, CInt(baseColor.R + (255 - baseColor.R) * factor))
        Dim g As Integer = Math.Min(255, CInt(baseColor.G + (255 - baseColor.G) * factor))
        Dim b As Integer = Math.Min(255, CInt(baseColor.B + (255 - baseColor.B) * factor))
        Return Color.FromArgb(255, r, g, b)
    End Function
    Private Function FilldtTemplate() As DataTable
        Dim dtTemplate As New DataTable
        dtTemplate.Columns.Add("R_CATNAME", GetType(String))
        dtTemplate.Columns.Add("R_TRANNO", GetType(String))
        dtTemplate.Columns.Add("R_PCS", GetType(Integer))
        dtTemplate.Columns.Add("R_GRSWT", GetType(Decimal))

        dtTemplate.Columns.Add("QC_TYPE", GetType(String))
        dtTemplate.Columns.Add("QC_TRANNO", GetType(String))
        dtTemplate.Columns.Add("QC_PCS", GetType(Integer))
        dtTemplate.Columns.Add("QC_GRSWT", GetType(Decimal))
        dtTemplate.Columns.Add("QC_EMPLOYEE", GetType(String))

        dtTemplate.Columns.Add("HM_TYPE", GetType(String))
        dtTemplate.Columns.Add("HM_TRANNO", GetType(String))
        dtTemplate.Columns.Add("HM_PCS", GetType(Integer))
        dtTemplate.Columns.Add("HM_GRSWT", GetType(Decimal))
        dtTemplate.Columns.Add("HM_HALLMARK", GetType(String))
        dtTemplate.Columns.Add("HM_EMPLOYEE", GetType(String))
        Return dtTemplate
    End Function
    Private Sub frmQCHMReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExToolStripMenuItem.Click
        Me.Close()
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
            GridViewHeaderStyleDetail()
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub funcNew()
        btnView.Enabled = True
        gridView.DataSource = Nothing
        gridView.Refresh()
    End Sub
    Dim colorPalette As Color() = {
    Color.LightBlue,
    Color.LightCoral,
    Color.LightGreen,
    Color.LightGoldenrodYellow,
    Color.LightPink
    }
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridView.Rows.Count > 0 Then
            Dim title As String = "Quality Check \ Hall Mark" & vbCrLf & "As on : " & dtpAsOn.Value.ToString("yyyy-MM-dd")
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            Dim title As String = "Quality Check \ Hall Mark" & vbCrLf & "As on : " & dtpAsOn.Value.ToString("yyyy-MM-dd")
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub
    Private Sub GridViewHeaderStyleDetail()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strAPPROVAL As String = ""
        With dtMergeHeader
            .Columns.Add("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT", GetType(String))
            .Columns.Add("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE", GetType(String))
            .Columns.Add("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE", GetType(String))

            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT").Caption = "RECEIPT"
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE").Caption = "QUALITY CHECKING"
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE").Caption = "HALL MARKING"
        End With

        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidthDetail()
        End With
    End Sub
    Function funcColWidthDetail() As Integer
        Dim strSepApp As String = ""
        With gridViewHead
            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT").HeaderText = "RECEIPT"
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE").HeaderText = "QUALITY CHECKING"
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE").HeaderText = "HALL MARKING"

            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT" & strSepApp).Width = gridView.Columns("R_CATNAME").Width
            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT" & strSepApp).Width += gridView.Columns("R_TRANNO").Width
            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT" & strSepApp).Width += gridView.Columns("R_PCS").Width
            .Columns("R_CATNAME~R_TRANNO~R_PCS~R_GRSWT" & strSepApp).Width += gridView.Columns("R_GRSWT").Width

            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE" & strSepApp).Width += gridView.Columns("QC_TYPE").Width
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE" & strSepApp).Width += gridView.Columns("QC_TRANNO").Width
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE" & strSepApp).Width += gridView.Columns("QC_PCS").Width
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE" & strSepApp).Width += gridView.Columns("QC_GRSWT").Width
            .Columns("QC_TYPE~QC_TRANNO~QC_PCS~QC_GRSWT~QC_EMPLOYEE" & strSepApp).Width += gridView.Columns("QC_EMPLOYEE").Width

            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_TYPE").Width
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_TRANNO").Width
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_PCS").Width
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_GRSWT").Width
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_HALLMARK").Width
            .Columns("HM_TYPE~HM_TRANNO~HM_PCS~HM_GRSWT~HM_HALLMARK~HM_EMPLOYEE" & strSepApp).Width += gridView.Columns("HM_EMPLOYEE").Width
        End With
    End Function
    Private Sub gridStyle()
        gridView.Columns("R_CATNAME").HeaderText = "CATEGORY"
        gridView.Columns("R_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("R_PCS").HeaderText = "PCS"
        gridView.Columns("R_GRSWT").HeaderText = "GRSWT"

        gridView.Columns("QC_TYPE").HeaderText = "TYPE"
        gridView.Columns("QC_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("QC_PCS").HeaderText = "PCS"
        gridView.Columns("QC_GRSWT").HeaderText = "GRSWT"
        gridView.Columns("QC_EMPLOYEE").HeaderText = "EMPLOYEE"

        gridView.Columns("HM_TYPE").HeaderText = "TYPE"
        gridView.Columns("HM_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("HM_PCS").HeaderText = "PCS"
        gridView.Columns("HM_GRSWT").HeaderText = "GRSWT"
        gridView.Columns("HM_HALLMARK").HeaderText = "HALLMARK"
        gridView.Columns("HM_EMPLOYEE").HeaderText = "EMPLOYEE"

        GridViewHeaderStyleDetail()
    End Sub
End Class