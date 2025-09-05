Imports System.Data.OleDb
Public Class W_InterestCalculation
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim StrSql As String
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub Load_ACNAME()
        Try
            StrSql = " SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD "
            StrSql += vbCrLf + " WHERE ACGRPCODE NOT IN ('3','4')"
            StrSql += vbCrLf + " ORDER BY ACNAME"
            objGPack.FillCombo(StrSql, cmbPartyName)

            dtpFrom.Value = Today.Date
            dtpTo.Value = Today.Date
            cmbPartyName.Text = ""

            cmbPartyName.Focus()
            cmbPartyName.Select()
            Me.Refresh()
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + vbCrLf + "POSITION :" + ex.StackTrace)
        End Try

    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            'lblTitle.Visible = False
            Dim strFrom As String
            Dim strTo As String

            'Me.Cursor = Cursors.WaitCursor

            strFrom = dtpFrom.Value.ToString("yyyy-MM-dd")
            strTo = dtpTo.Value.ToString("yyyy-MM-dd")

            StrSql = "EXEC " & cnStockDb & "..SP_RPT_WINTERESTCAL "
            If chkReceipt.Checked = True And chkIssue.Checked = True Then
                StrSql += " @RPTOPTION='A',"
            ElseIf chkReceipt.Checked = True Then
                StrSql += " @RPTOPTION='R',"
            ElseIf chkIssue.Checked = True Then
                StrSql += " @RPTOPTION='I',"
            End If
            If optAmt.Checked = True Then
                StrSql += " @CALTYPE='A',"
            Else
                StrSql += " @CALTYPE='W',"
            End If
            StrSql += " @FromDate='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
            StrSql += " @Todate='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
            StrSql += " @Caldate='" & dtpCalcDate.Value.ToString("yyyy-MM-dd") & "',"
            StrSql += " @GRACEPERIOD=" & Val(txtGraceDays.Text) & ","
            StrSql += " @TOTDAYS=" & Val(txtTotDays.Text) & ","
            StrSql += " @AMTCALVAL=" & Val(txtIntRs.Text) & ","
            StrSql += " @WTCALVAL=" & Val(txtIntWt.Text) & ","
            StrSql += " @CUSTNAME='" & cmbPartyName.Text & "'"


            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                dtpFrom.Focus()
                Exit Sub
            End If
            lblTitle.Visible = True
            lblTitle.Text = "Interest Calcultion Date Between  " & Format(dtpFrom.Value, "dd/MM/yyyy") & " and " & Format(dtpTo.Value, "dd/MM/yyyy")
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                'tabView.Show()

                GridViewFormat()

               
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
            
                .Columns("PARTICULAR").Width = 80
                .Columns("METALRECEIPT").Width = 80
                .Columns("METALISSUE").Width = 80
                .Columns("CASHRECEIPT").Width = 80
                .Columns("CASHISSUE").Width = 80
                .Columns("DAYS").Width = 80
                .Columns("METALINT").Width = 80
                .Columns("CASHINT").Width = 80


               

                .Columns("PARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("METALRECEIPT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("METALISSUE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("CASHRECEIPT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("CASHISSUE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("DAYS").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("METALINT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("CASHINT").SortMode = DataGridViewColumnSortMode.NotSortable
              

                .Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft


                .Columns("METALRECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("METALISSUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CASHRECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CASHISSUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("DAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("METALINT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CASHINT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


                .Columns("PARTICULAR").HeaderText = "PARTICULAR"
                .Columns("METALRECEIPT").HeaderText = "RECEIPT"
                .Columns("METALISSUE").HeaderText = "ISSUE"
                .Columns("CASHRECEIPT").HeaderText = "RECEIPT"
                .Columns("CASHISSUE").HeaderText = "ISSUE"

                .Columns("DAYS").HeaderText = "DAYS"
                .Columns("METALINT").HeaderText = "INTR"
                .Columns("CASHINT").HeaderText = "INTR"
             
                Call GridViewHeaderCreator(gridViewHeader)

            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            MsgBox(ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub frmTransactionReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub frmTransactionReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpFrom.Value = Today
        dtpTo.Value = Today
        dtpFrom.Select()
        Call Load_ACNAME()
        optPer.Select()
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        gridviewHead.Visible = True
        StrSql = "SELECT ''[PARTICULAR],''[METALRECEIPT~METALISSUE],''[CASHRECEIPT~CASHISSUE],''[DAYS],''[METALINT~CASHINT]"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead

        gridviewHead.Columns("PARTICULAR").HeaderText = " "
        gridviewHead.Columns("METALRECEIPT~METALISSUE").HeaderText = "METAL"
        gridviewHead.Columns("CASHRECEIPT~CASHISSUE").HeaderText = "CASH"
        gridviewHead.Columns("DAYS").HeaderText = " "
        gridviewHead.Columns("METALINT~CASHINT").HeaderText = "INTEREST"

        ''gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        'Dim f As frmGridDispDia
        'f = objGPack.GetParentControl(gridViewHeader)
        'If gridViewHeader.Visible Then Exit Sub
        If gridViewHeader Is Nothing Then Exit Sub
        If Not gridView.ColumnCount > 0 Then Exit Sub
        If Not gridViewHeader.ColumnCount > 0 Then Exit Sub
        With gridViewHeader
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            .Columns("METALRECEIPT~METALISSUE").Width = gridView.Columns("METALRECEIPT").Width + gridView.Columns("METALISSUE").Width
            .Columns("CASHRECEIPT~CASHISSUE").Width = gridView.Columns("CASHRECEIPT").Width + _
                     gridView.Columns("CASHISSUE").Width
            .Columns("DAYS").Width = gridView.Columns("DAYS").Width
            .Columns("METALINT~CASHINT").Width = gridView.Columns("METALINT").Width + gridView.Columns("CASHINT").Width
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        Call SetGridHeadColWid(gridViewHeader)
    End Sub

    Private Sub lblTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTitle.Click

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class