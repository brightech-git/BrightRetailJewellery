Imports System.Data.OleDb
Public Class WTrialSummary
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim StrSql As String
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub VIEW(ByVal ASONDATE As Date, ByVal strRate As String, ByVal GRD As DataGridView)
        Try
            StrSql = "EXEC " & cnStockDb & "..SP_RPT_WTRIALSUMMARY"
            StrSql += " @ASONDATE='" & ASONDATE & "',"
            StrSql += " @CURRATE='" & Val(strRate) & "',"
            If optSummarry.Checked = True Then
                StrSql += " @SUMMARY='Y'"
            Else
                StrSql += " @SUMMARY='N'"
            End If
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                dtpDateOne.Focus()
                Exit Sub
            End If
            'lblTitle.Visible = True
            With GRD
                .DataSource = Nothing
                .DataSource = dt

                .Columns("LCOLHEAD").Visible = False
                .Columns("LRESULT").Visible = False

                .Columns("RCOLHEAD").Visible = False
                .Columns("RRESULT").Visible = False

                .Columns("LPARTICULAR").Width = 200
                .Columns("RPARTICULAR").Width = 200

                .Columns("LCLSWT").Width = 100
                .Columns("LCOLHEAD").Width = 0
                .Columns("LRESULT").Width = 0
                '
                .Columns("RCLSWT").Width = 100
                .Columns("RCOLHEAD").Width = 0
                .Columns("RRESULT").Width = 0

                .Columns("LCLSWT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LCOLHEAD").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LRESULT").SortMode = DataGridViewColumnSortMode.NotSortable


                .Columns("RCLSWT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RCOLHEAD").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RRESULT").SortMode = DataGridViewColumnSortMode.NotSortable

             
                .Columns("LCLSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RCLSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            
                .Columns("LCLSWT").HeaderText = "WT/AMT"
                .Columns("RCLSWT").HeaderText = "WT/AMT"
                GridViewFormat(GRD)
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            MsgBox(ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Call VIEW(dtpDateOne.Value, Val(txtRateOne.Text), gridViewFirst)
        Call VIEW(dtpDateSecond.Value, Val(txtRateSecond.Text), gridViewSecond)
    End Sub
    Function GridViewFormat(ByVal GRD As DataGridView) As Integer
        For Each dgvRow As DataGridViewRow In GRD.Rows
            With dgvRow
                Select Case .Cells("LCOLHEAD").Value.ToString
                    Case "T"
                        .Cells(0).Style.BackColor = reportHeadStyle.BackColor
                        .Cells(0).Style.Font = reportHeadStyle.Font
                    Case "T1"
                        .Cells(0).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(0).Style.Font = reportHeadStyle2.Font
                        .Cells(1).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(1).Style.Font = reportHeadStyle2.Font
                    Case "S"
                        .Cells(0).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(0).Style.Font = reportSubTotalStyle.Font
                        .Cells(1).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(1).Style.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
                Select Case .Cells("RCOLHEAD").Value.ToString
                    Case "T"
                        .Cells(4).Style.BackColor = reportHeadStyle.BackColor
                        .Cells(4).Style.Font = reportHeadStyle.Font
                    Case "T1"
                      
                    Case "S"
                        .Cells(4).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(4).Style.Font = reportSubTotalStyle.Font
                        .Cells(5).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(5).Style.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    
    Private Sub WTrialSummary_KEYPRESS(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewFirst.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Trial Summary", gridViewFirst, BrightPosting.GExport.GExportType.Export)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Trial Summary", gridViewSecond, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridViewFirst.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Trial Summary", gridViewFirst, BrightPosting.GExport.GExportType.Print)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Trial Summary", gridViewSecond, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub frmTransactionReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpDateOne.Value = Today
        dtpDateSecond.Value = Today
        dtpDateOne.Select()
    End Sub
End Class