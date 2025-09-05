Imports System.IO
Imports System.Data.OleDb
Public Class frmTagReceiptView
    Dim strSql As String
    Dim Cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim dtDesigner As New DataTable

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        funcView()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmTagReceiptView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagReceiptView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.X) Or AscW(e.KeyChar) = 120 Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY RESULT,ITEMNAME"
        Dim dtItem As DataTable = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function
    Private Sub frmTagReceiptView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcNew()
    End Sub
    Private Sub funcView()
        gridTotalView.DataSource = Nothing
        gridTotalView.Refresh()
        Dim DesignerId As String = ""
        Dim CostId As String = ""
        If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then
            strSql = "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & CmbDesigner.Text & "'"
            DesignerId = objGPack.GetSqlValue(strSql, "DESIGNERID", 0).ToString
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            CostId = objGPack.GetSqlValue(strSql, "COSTID", 0).ToString
        End If
        If txtLotNoTo_NUM.Text = "" Then
            txtLotNoTo_NUM.Text = txtLotNoFrom_NUM.Text
        End If
        Dim itemids As String = ""
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & ")"
            Dim dtitemid As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitemid)
            itemids = ""
            For cnt As Integer = 0 To dtitemid.Rows.Count - 1
                itemids += dtitemid.Rows(cnt).Item("ITEMID").ToString
                If cnt < dtitemid.Rows.Count - 1 Then
                    itemids += ","
                End If
            Next
        End If

        strSql = "EXEC " & cnAdminDb & "..PROC_ITEMRECEIPTVIEW"
        strSql += vbCrLf + " @FROMDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COSTID='" & CostId & "'"
        strSql += vbCrLf + " ,@LOTFROMNO='" & txtLotNoFrom_NUM.Text & "'"
        strSql += vbCrLf + " ,@LOTTONO='" & txtLotNoTo_NUM.Text & "'"
        strSql += vbCrLf + " ,@DESIGNERID='" & DesignerId & "'"
        strSql += vbCrLf + " ,@ITEMIDS='" & itemids.ToString & "'"
        strSql += vbCrLf + " ,@STOCKONLY='" & IIf(chkStockOnly.Checked, "Y", "N") & "'"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(Cmd)
        Dim ds As New DataSet
        da.Fill(ds)
        If ds.Tables.Count > 0 Then
            dt = ds.Tables(0)
            gridTotalView.DataSource = dt
            With gridTotalView
                .Columns("SNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("SLNO").HeaderText = "SNO"
                FormatGridColumns(gridTotalView, False)
                FillGridGroupStyle_KeyNoWise(gridTotalView)
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("PURMC").DefaultCellStyle.Format = "0.00"
                .Columns("PURAMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("PURRATE").DefaultCellStyle.Format = "0.00"
                .Columns("MCGRM").DefaultCellStyle.Format = "0.00"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            lblReportTitle.Text = "TAGED ITEM RECEIPT VIEW"
            lblReportTitle.Text += " FROM " + dtpFrom.Value.ToString("dd-MM-yyyy") + " TO " + dtpTo.Value.ToString("dd-MM-yyyy")
            lblReportTitle.Text += IIf(cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL", " :" & cmbCostCentre.Text, "")
        Else
            MsgBox("Record not Found...", MsgBoxStyle.Information)
            gridTotalView.DataSource = Nothing
            gridTotalView.Refresh()
            dtpFrom.Select()
        End If
    End Sub
    Private Sub funcNew()
        txtLotNoFrom_NUM.Clear()
        txtLotNoTo_NUM.Clear()
        lblReportTitle.Text = ""
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        If _IsCostCentre Then
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
        If cmbCostCentre.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY COSTNAME"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False)
            cmbCostCentre.Text = "ALL"
        End If
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(VARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(CmbDesigner, dtDesigner, "DESIGNERNAME", )
        CmbDesigner.Text = "ALL"
        funcLoadItemName()
        dtpFrom.Select()
        gridTotalView.DataSource = Nothing
        gridTotalView.Refresh()
    End Sub

End Class