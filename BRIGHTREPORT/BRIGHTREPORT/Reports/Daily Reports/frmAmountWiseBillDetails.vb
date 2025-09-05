Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Imports System.Net.Mail
Public Class frmAmountWiseBillDetails
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty

    Private Sub frmAmountWiseBillDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                btnview_Click(btnview, Nothing)
                Exit Select
            Case Keys.X
                btnExport_Click(btnExport, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        FuncView()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If DGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DGrid, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub FuncNew()
        With Me
            .lblTitle.Text = ""
            .dtpFrmDate.Value = GetServerDate()
            .dtpToDate.Value = GetServerDate()
            .DGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub Heading()
        til = Me.Text & " Date : " & Me.dtpFrmDate.Value.ToString("dd-MM-yyyy")
        til += " To " & Me.dtpToDate.Value.ToString("dd-MM-yyyy")
        If Val(Me.txtFrmAmt_AMT.Text.Trim()) <> 0 And Val(Me.txtToAmt_AMT.Text.Trim()) <> 0 Then
            til += "  Amount From " & Me.txtFrmAmt_AMT.Text.Trim() & " To " & Me.txtToAmt_AMT.Text.Trim()
        ElseIf Val(Me.txtFrmAmt_AMT.Text.Trim()) <> 0 And Val(Me.txtToAmt_AMT.Text.Trim()) = 0 Then
            til += "  Amount Above From " & Me.txtFrmAmt_AMT.Text.Trim()
        ElseIf Val(Me.txtFrmAmt_AMT.Text.Trim()) = 0 And Val(Me.txtToAmt_AMT.Text.Trim()) <> 0 Then
            til += "  Amount Below To " & Me.txtToAmt_AMT.Text.Trim()
        End If
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
    Private Sub GridColor()
        For Each dgvRow As DataGridViewRow In DGrid.Rows
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case 1
                    dgvRow.DefaultCellStyle.ForeColor = Color.Blue
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
            End Select
        Next
    End Sub
    Private Sub FuncView()
        DGrid.DataSource = Nothing
        DGrid.Refresh()
        Dim Optionid As String = ""
        Dim RptType As String = ""
        Dim RptTable As String = ""
        If rbtnSales.Checked = True Then
            RptType = "SA"
            RptTable = "..ISSUE"
        Else
            RptType = "PU"
            RptTable = "..RECEIPT"
        End If
        AutoResizeToolStripMenuItem.Checked = False
        title = dtpFrmDate.Value.ToString("MM/dd/yyyy")
        Dim frmamount As Integer = Val(txtFrmAmt_AMT.Text.Trim())
        Dim toamount As Integer = Val(txtToAmt_AMT.Text.Trim())
        Dim Cmd As New OleDbCommand
        strsql = " EXEC " & cnAdminDb & "..RPT_AMOUNTWISESALE"
        strsql += vbCrLf + " @DATEFROM = '" & dtpFrmDate.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + " ,@DATETO = '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + " ,@COMPANY = '" & GetQryString(cmbCompany.Text).Replace("'", "") & "'"
        strsql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strsql += vbCrLf + " ,@AMOUNTFROM = '" & frmamount & "'"
        strsql += vbCrLf + " ,@AMOUNTTO = '" & toamount & "'"
        strsql += vbCrLf + " ,@TYPE = '" & RptType & "'"
        strsql += vbCrLf + " ,@WITHCAT='" & IIf(ChkWithCat.Checked, "Y", "N") & "'"
        strsql += vbCrLf + " ,@WITHMOBILE='" & IIf(ChkMobile.Checked, "Y", "N") & "'"
        strsql += vbCrLf + " ,@WITHSUMMARY='" & IIf(ChkSummary.Checked, "Y", "N") & "'"
        strsql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@PANBILLONLY='" & IIf(chkOnlyPanBill.Checked, "Y", "N") & "'"
        Dim ds As New DataSet
        Cmd = New OleDbCommand(strsql, cn)
        Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(Cmd)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            DGrid.DataSource = ds.Tables(0)
            Heading()
            With DGrid
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                FormatGridColumns(DGrid, False, True, False, False)
                If .Columns.Contains("PNAME") Then .Columns("PNAME").HeaderText = "CUSTOMER NAME"

                If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
                If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False
                If .Columns.Contains("TRANDATE") And ChkWithCat.Checked = False Then .Columns("TRANDATE").Visible = False
                If .Columns.Contains("TRANDATE") And ChkMobile.Checked = True Then .Columns("TRANDATE").Visible = True
                If .Columns.Contains("PARTICULAR") And ChkWithCat.Checked = False Then .Columns("PARTICULAR").HeaderText = "TRANDATE"
                If .Columns.Contains("PARTICULAR") And ChkSummary.Checked Then .Columns("PARTICULAR").Visible = False
                If .Columns.Contains("TRANNO") And ChkSummary.Checked Then .Columns("TRANNO").Visible = False
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TAX") Then .Columns("TAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TOTAL") Then .Columns("TOTAL").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CASH") Then .Columns("CASH").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CHEQUE") Then .Columns("CHEQUE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CARD") Then .Columns("CARD").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE") Then .Columns("ADVANCE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CHIT") Then .Columns("CHIT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("OG") Then .Columns("OG").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("OTHERS") Then .Columns("OTHERS").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("DUE") Then .Columns("DUE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_CA") Then .Columns("ADVANCE_CA").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_CH") Then .Columns("ADVANCE_CH").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_CC") Then .Columns("ADVANCE_CC").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_SS") Then .Columns("ADVANCE_SS").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_PU") Then .Columns("ADVANCE_PU").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_SR") Then .Columns("ADVANCE_SR").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("ADVANCE_GV") Then .Columns("ADVANCE_GV").DefaultCellStyle.Format = "0.00"
                If ChkWithCat.Checked Then
                    If .Columns.Contains("MOBILE") Then .Columns("MOBILE").Visible = False
                End If
                If ChkMobile.Checked Then
                    .Columns("MOBILE").Visible = False
                    .Columns("TRANDATE").Visible = False
                End If
                If .Columns.Contains("PARTICULAR") And ChkMobile.Checked Then .Columns("PARTICULAR").HeaderText = "PARTICULAR"
                If ChkSummary.Checked Then
                    .Columns("MOBILE").Visible = False
                    .Columns("PARTICULAR").Visible = True
                    .Columns("PARTICULAR").HeaderText = "PARTICULAR"
                End If
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                If chkAdvDetail.Checked Then
                    If .Columns.Contains("ADVANCE_CA") Then .Columns("ADVANCE_CA").Visible = True : .Columns("ADVANCE_CA").HeaderText = "ADV CASH"
                    If .Columns.Contains("ADVANCE_CH") Then .Columns("ADVANCE_CH").Visible = True : .Columns("ADVANCE_CH").HeaderText = "ADV CHEQUE"
                    If .Columns.Contains("ADVANCE_CC") Then .Columns("ADVANCE_CC").Visible = True : .Columns("ADVANCE_CC").HeaderText = "ADV CARD"
                    If .Columns.Contains("ADVANCE_SS") Then .Columns("ADVANCE_SS").Visible = True : .Columns("ADVANCE_SS").HeaderText = "SCHEME ADV"
                    If .Columns.Contains("ADVANCE_PU") Then .Columns("ADVANCE_PU").Visible = True : .Columns("ADVANCE_PU").HeaderText = "ADV PUR"
                    If .Columns.Contains("ADVANCE_SR") Then .Columns("ADVANCE_SR").Visible = True : .Columns("ADVANCE_SR").HeaderText = "ADV SR"
                    If .Columns.Contains("ADVANCE_GV") Then .Columns("ADVANCE_GV").Visible = True : .Columns("ADVANCE_GV").HeaderText = "ADV GIFT"
                    If .Columns.Contains("ADVANCE") Then .Columns("ADVANCE").Visible = False
                Else
                    If .Columns.Contains("ADVANCE_CA") Then .Columns("ADVANCE_CA").Visible = False : .Columns("ADVANCE_CA").HeaderText = "ADV CASH"
                    If .Columns.Contains("ADVANCE_CH") Then .Columns("ADVANCE_CH").Visible = False : .Columns("ADVANCE_CH").HeaderText = "ADV CHEQUE"
                    If .Columns.Contains("ADVANCE_CC") Then .Columns("ADVANCE_CC").Visible = False : .Columns("ADVANCE_CC").HeaderText = "ADV CARD"
                    If .Columns.Contains("ADVANCE_SS") Then .Columns("ADVANCE_SS").Visible = False : .Columns("ADVANCE_SS").HeaderText = "SCHEME ADV"
                    If .Columns.Contains("ADVANCE_PU") Then .Columns("ADVANCE_PU").Visible = False : .Columns("ADVANCE_PU").HeaderText = "PURCHASE ADV"
                    If .Columns.Contains("ADVANCE_SR") Then .Columns("ADVANCE_SR").Visible = False : .Columns("ADVANCE_SR").HeaderText = "ADV SR"
                    If .Columns.Contains("ADVANCE_GV") Then .Columns("ADVANCE_GV").Visible = False : .Columns("ADVANCE_GV").HeaderText = "ADV GIFT"
                    If .Columns.Contains("ADVANCE") Then .Columns("ADVANCE").Visible = True
                End If
                For J As Integer = 0 To DGrid.Rows.Count - 1
                    If Val(DGrid.Rows(J).Cells("RESULT").Value.ToString) = 0 Then
                        DGrid.Rows(J).Cells("PARTICULAR").Style = reportHeadStyle
                    ElseIf Val(DGrid.Rows(J).Cells("RESULT").Value.ToString) = 2 Then
                        DGrid.Rows(J).DefaultCellStyle = reportSubTotalStyle
                    ElseIf Val(DGrid.Rows(J).Cells("RESULT").Value.ToString) = 3 Then
                        DGrid.Rows(J).DefaultCellStyle = reportTotalStyle
                    End If
                Next

            End With
        Else
            DGrid.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.")
        End If
        funcGridSalesPersonStyle()
    End Sub

    Function funcGridSalesPersonStyle() As Integer
        With DGrid
            'With .Columns("SALESPERSON")
            '    .Visible = True
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '    .Width = 200
            'End With
        End With
    End Function
    Private Sub frmAmountWiseBillDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strsql = vbCrLf + " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        da = New OleDbDataAdapter(strsql, cn)
        Dim dtCompany As New DataTable()
        da.Fill(dtCompany)
        cmbCompany.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        FuncNew()
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If DGrid.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                DGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                DGrid.Invalidate()
                For Each dgvCol As DataGridViewColumn In DGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                DGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In DGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                DGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If DGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DGrid, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub ChkWithCat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkWithCat.CheckedChanged
        If ChkWithCat.Checked Then
            ChkMobile.Checked = False
            ChkSummary.Checked = False
        End If
    End Sub

    Private Sub ChkMobile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMobile.CheckedChanged
        If ChkMobile.Checked Then
            ChkWithCat.Checked = False
            ChkSummary.Checked = False
        End If
    End Sub

    Private Sub ChkSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSummary.CheckedChanged
        If ChkSummary.Checked Then
            ChkMobile.Checked = False
            ChkWithCat.Checked = False
        End If
    End Sub
End Class
