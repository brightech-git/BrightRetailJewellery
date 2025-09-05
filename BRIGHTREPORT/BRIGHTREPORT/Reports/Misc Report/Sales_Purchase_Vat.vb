Imports System.Data.OleDb
Public Class Sales_Purchase_Vat
    '280613 LocalOutstation Filter For WhiteFire
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Private Sub Sales_Purchase_Vat_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub Sales_Purchase_Vat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        rbtPurchaseVat.Enabled = True
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtSalesVat.Checked = True
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim LOCALOUT As String = ""
        If rbtLocal.Checked = True Then
            LOCALOUT = "L"
        ElseIf rbtOutstation.Checked = True Then
            LOCALOUT = "O"
        End If
        If rbtSalesVat.Checked Then
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_SALESVAT"
        Else
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_PURCHASEVAT_CU"
        End If
        StrSql += " @DATEFROM = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@DATETO = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@DBNAME = '" & cnAdminDb & "'"
        '280613
        StrSql += " ,@LOCALOUT = '" & LOCALOUT & "'"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Da = New OleDbDataAdapter(StrSql, cn)
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        Da.Fill(dtGrid)
        dtGrid.Select("", "")
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
        objGridShower.Text = IIf(rbtSalesVat.Checked, "SALES", "PURCHASE") & " VAT"
        Dim tit As String = IIf(rbtSalesVat.Checked, "SALES", "PURCHASE") & " VAT" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            .Columns("KEYNO").Visible = False
            .Columns("SERIAL_NO").HeaderText = "SNO"
            FormatGridColumns(dgv, False, False, , False)
            '.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            'FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
End Class