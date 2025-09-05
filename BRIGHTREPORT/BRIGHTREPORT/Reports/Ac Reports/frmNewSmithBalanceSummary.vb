Imports System.Data.OleDb
Public Class frmNewSmithBalanceSummary
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Sysid As String = ""

    Private Sub FrmNewSmithBalanceSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)

        strSql = vbCrLf + " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCompany As New DataTable()
        da.Fill(dtCompany)
        cmbCompany.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")


        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY RESULT,COSTNAME"
        Dim dtcostcentre As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcostcentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtcostcentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        strSql += " WHERE ACTYPE IN ('G','D','I') AND ISNULL(ACTIVE,'Y') <> 'H'"
        strSql += " ORDER BY RESULT,ACNAME"

        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        BrighttechPack.GlobalMethods.FillCombo(CmbSmith, dtAcc, "ACNAME", )

        Me.TabMain.ItemSize = New Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        Me.TabMain.SelectedTab = TabGeneral
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmNewSmithBalanceSummary_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And TabMain.SelectedTab.Name = TabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        ReziseToolStripMenuItem.Checked = False
        Dim cmbAccName As String = ""
        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbSmith.Text & "'"
        cmbAccName = objGPack.GetSqlValue(strSql, , , )
        If cmbAccName = "" Then MsgBox("Smith Name Should not Empty", MsgBoxStyle.Information) : CmbSmith.Select() : Exit Sub

        strSql = vbCrLf + " EXEC " & cnAdminDb & "..[SP_RPT_SMITHREPORT_SUMMARYFORMAT]"
        strSql += vbCrLf + " @FROMDATE ='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @TODATE ='" & Format(dtpTo.Value, "yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @CNADMINDB  ='" & cnAdminDb & "',"
        strSql += vbCrLf + " @TRANDB  ='" & cnStockDb & "',"
        strSql += vbCrLf + " @COMPANYID ='" & IIf(cmbCompany.Text <> "", GetQryString(cmbCompany.Text).Replace("'", ""), "ALL") & "',"
        strSql += vbCrLf + " @COSTID ='" & IIf(chkCmbCostCentre.Text <> "", GetQryString(chkCmbCostCentre.Text).Replace("'", ""), "ALL") & "',"
        strSql += vbCrLf + " @ACCODE ='" & cmbAccName.ToString.Trim & "',"
        strSql += vbCrLf + " @NODEID ='" & Sysid.ToString.Trim & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid.ToString.Trim & "_NEWSMTABS1"
        strSql += vbCrLf + " ORDER BY RESULT"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim tit As String = " SMITH BALANCE DETAILED REPORT"
        tit += " FROM  " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL" Then
            Dim Cname As String = ""
            If chkCmbCostCentre.Items.Count > 0 And chkCmbCostCentre.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkCmbCostCentre.Items.Count - 1
                    If chkCmbCostCentre.GetItemChecked(CNT) = True Then
                        Cname += "" & chkCmbCostCentre.Items(CNT) + ","
                    End If
                Next
                If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
            End If
            If Cname <> "" Then
                tit += vbCrLf & "COSTCENTRE :"
                tit = tit + Cname
            End If
        End If

        lblTitle.Text = tit.ToString.Trim
        GridView_OWN.DataSource = Nothing
        GridView_OWN.DataSource = dtGrid
        GridView_OWN.Columns("RESULT").Visible = False
        lblPurewtBalValue.Text = Format(Val(dtGrid.Compute("SUM(RPUREWT)", "RESULT=3").ToString) - Val(dtGrid.Compute("SUM(IPUREWT)", "RESULT=3").ToString), "0.000")
        lblAmountBalValue.Text = Format(Val(dtGrid.Compute("SUM(ISS_AMOUNT)", "RESULT=3").ToString) - Val(dtGrid.Compute("SUM(REC_AMOUNT)", "RESULT=3").ToString), "0.00")
        TabMain.SelectedTab = TabView
        DataGridView_SummaryFormatting(GridView_OWN)
        'GridView.Columns("RESULT").Visible = False

    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim cnt As Integer
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next

            .Columns("PARTICULAR").Width = 100
            .Columns("TRANNO").Width = 80
            .Columns("TRANDATE").Width = 80
            .Columns("CNAME").Width = 250
            .Columns("RGRSWT").Width = 130
            .Columns("RPUREWT").Width = 130
            .Columns("IGRSWT").Width = 130
            .Columns("IPUREWT").Width = 130
            .Columns("REC_AMOUNT").Width = 130
            .Columns("ISS_AMOUNT").Width = 130

            .Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("CNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REC_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISS_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("RESULT").Visible = False

            For cntt As Integer = 0 To .Rows.Count - 1
                If Val(.Rows(cntt).Cells("RESULT").Value.ToString) = 3 Then
                    .Rows(cntt).DefaultCellStyle.BackColor = Color.LightBlue
                End If
                If Val(.Rows(cntt).Cells("RESULT").Value.ToString) = 1 Then
                    .Rows(cntt).DefaultCellStyle.BackColor = Color.Lavender
                End If
            Next
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("TRANNO").HeaderText = "TRANNO"
            .Columns("TRANDATE").HeaderText = "TRANDATE"
            .Columns("CNAME").HeaderText = "CATEGORY"
            .Columns("RGRSWT").HeaderText = "RECEIPT_GRSWT"
            .Columns("RPUREWT").HeaderText = "RECEIPT_PUREWT"
            .Columns("IGRSWT").HeaderText = "ISSUE_GRSWT"
            .Columns("IPUREWT").HeaderText = "ISSUE_PUREWT"
            .Columns("REC_AMOUNT").HeaderText = "RECEIPT_AMT"
            .Columns("ISS_AMOUNT").HeaderText = "ISSUE_AMT"
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
        CmbSmith.Text = ""
        cmbCompany.Text = "ALL"
        chkCmbCostCentre.Text = "ALL"
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        TabMain.SelectedTab = TabGeneral
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Me.Cursor = Cursors.WaitCursor
        If GridView_OWN.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Me.Cursor = Cursors.WaitCursor
        If GridView_OWN.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView_OWN.KeyPress
        If UCase(e.KeyChar) = "X" Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub CmbSmith_KeyDown(sender As Object, e As KeyEventArgs) Handles CmbSmith.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class