Imports System.Data.OleDb
Public Class FRM_TAGWISEPROFIT
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim Cmd As OleDbCommand

    Private Sub FRM_TAGWISEPROFIT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FRM_TAGWISEPROFIT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If rdbValAdded.Checked Then
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFIT_VALADDED"
            StrSql += " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@TRANNO = " & Val(txtTranNo_NUM.Text) & ""
        Else
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFIT"
            StrSql += " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(StrSql, cn)
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
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
        objGridShower.Text = "TAG WISE PROFIT"
        Dim tit As String = "TAG WISE PROFIT ANALYSIS" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        Dim ObjGrouper As New GiritechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
        ObjGrouper.pColumns_Group.Add("TRANTYPE")
        ObjGrouper.pColumns_Group.Add("DESIGNER")
        ObjGrouper.pColumns_Sum.Add("PCS")
        ObjGrouper.pColumns_Sum.Add("GRSWT")
        ObjGrouper.pColumns_Sum.Add("NETWT")
        ObjGrouper.pColumns_Sum.Add("DIAPCS")
        ObjGrouper.pColumns_Sum.Add("DIAWT")
        ObjGrouper.pColumns_Sum.Add("WASTAGE")
        ObjGrouper.pColumns_Sum.Add("MCHARGE")
        ObjGrouper.pColumns_Sum.Add("AMOUNT")
        ObjGrouper.pColumns_Sum.Add("PURVALUE_CALC")
        ObjGrouper.pColumns_Sum.Add("PURVALUE")
        ObjGrouper.pColumns_Sum.Add("DIFF_STKRATE")
        ObjGrouper.pColumns_Sum.Add("DIFF_ISSRATE")
        ObjGrouper.pColName_Particular = "ITEMNAME"
        ObjGrouper.pColName_ReplaceWithParticular = "ITEMNAME"
        If rdbValAdded.Checked Then
            ObjGrouper.pColumns_Sort = "TRANDATE,TRANNO,ITEMNAME"
        End If

        'ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        If objGridShower.gridView.RowCount > 0 Then
            objGridShower.gridView.Rows.RemoveAt(objGridShower.gridView.Rows.Count - 1)
        End If
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BATCHNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("DESIGNER").Visible = False
            .Columns("PURVALUE").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("SEP").Visible = False
            .Columns("DIFF_STKRATE").Visible = False
            .Columns("PURVALUE").HeaderText = "PURVALUE @STKRATE"
            .Columns("PURVALUE_CALC").HeaderText = "PURVALUE @ISSRATE"

            .Columns("DIFF_STKRATE").HeaderText = "PROFIT @STKRATE"
            .Columns("DIFF_ISSRATE").HeaderText = "PROFIT @ISSRATE"
            If rdbValAdded.Checked Then
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            '.Columns("BILLTYPE").HeaderText = "TYPE"
            '.Columns("BILLTYPE").Width = 40
            '.Columns("TRANNO").Width = 60
            '.Columns("TRANDATE").Width = 80
            '.Columns("TAGNO").Width = 70
            '.Columns("ITEMNAME").Width = 150
            '.Columns("PCS").Width = 60
            '.Columns("GRSWT").Width = 80
            '.Columns("NETWT").Width = 80
            '.Columns("GRSNET").Width = 40
            '.Columns("RATE").Width = 100
            '.Columns("TOUCH").Width = 60
            '.Columns("TAGVALUE").Width = 100
            '.Columns("AMOUNT").Width = 100
            '.Columns("DIFF").Width = 100
            '.Columns("DIFFPER").Width = 70
            '.Columns("DAYS").Width = 60
            '.Columns("SALESPERSON").Width = 120
            '.Columns("KEYNO").Visible = False
            '.Columns("COLHEAD").Visible = False
            '.Columns("RESULT").Visible = False
            'FormatGridColumns(dgv, False, False, , False)
            '.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub rdbValAdded_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbValAdded.CheckedChanged
        txtTranNo_NUM.Enabled = rdbValAdded.Checked
    End Sub
End Class