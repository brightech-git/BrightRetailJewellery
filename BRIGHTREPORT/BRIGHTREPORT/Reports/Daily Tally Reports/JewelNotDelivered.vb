Imports System.Data.OleDb

Public Class JewelNotDelivered
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Private Sub JewelNotDelivered_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtPending.Checked = True
        dtpFrom.Select()
    End Sub

    Private Sub JewelNotDelivered_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If systemId Is Nothing Then systemId = ""
        StrSql = vbCrLf + " EXEC " & cnStockDb & "..PROC_RPT_JEWELNOTDELIVERED"
        StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        If chkAsonDate.Checked = True Then
            StrSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If rbtPending.Checked Then
            StrSql += vbCrLf + " ,@TYPE = 'P'"
        ElseIf rbtReceipt.Checked Then
            StrSql += vbCrLf + " ,@TYPE = 'R'"
        Else
            StrSql += vbCrLf + " ,@TYPE = 'I'"
        End If
        StrSql += vbCrLf + " ,@DBNAME = '" & cnAdminDb & "'"
        StrSql += vbCrLf + " ,@SYSID = '" & systemId & userId.ToString & "'"
        StrSql += vbCrLf + " ,@ASONDATE= '" & IIf(chkAsonDate.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "JEWEL NOT DELIVERED"
        Dim tit As String = "JEWEL NOT DELIVERED "
        If rbtPending.Checked Then
            tit += " [PENDING]"
        ElseIf rbtReceipt.Checked Then
            tit += " [RECEIPT]"
        ElseIf rbtIssue.Checked Then
            tit += " [ISSUE]"
        End If
        If chkAsonDate.Checked Then tit += " AS ON DATE " + dtpTo.Text Else tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("DESCRIPT")))
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)
            .Columns("CASHCOUNTER").Width = 100
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 80
            .Columns("RUNNO").Width = 80
            .Columns("DESCRIPT").Width = 200
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("AMOUNT").Width = 120
            .Columns("REMARK2").Width = 150

            .Columns("TRANNO").Visible = Not rbtPending.Checked
            .Columns("TRANDATE").Visible = Not rbtPending.Checked
            If chkAsonDate.Checked Then
                .Columns("TRANNO").Visible = True
                .Columns("TRANDATE").Visible = True
            End If
            .Columns("CASHCOUNTER").Visible = True
            .Columns("RUNNO").Visible = True
            .Columns("DESCRIPT").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("REMARK2").Visible = True

            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            dtpTo.Enabled = False
            lblFrmDate.Text = "As On Date"
            rbtPending.Checked = True
            rbtIssue.Enabled = False
            rbtReceipt.Enabled = False
        Else
            dtpTo.Enabled = True
            lblFrmDate.Text = "From Date"
            rbtIssue.Enabled = True
            rbtReceipt.Enabled = True
        End If
    End Sub
End Class