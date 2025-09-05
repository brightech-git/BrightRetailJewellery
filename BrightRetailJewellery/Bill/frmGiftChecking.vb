Imports System.Data.OleDb
Public Class frmGiftChecking
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGridView As DataTable
    Dim defalutDestination As String
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim IS_IMAGE_TRF As Boolean = IIf(GetAdmindbSoftValue("STKTRANWITHIMAGE", "N") = "Y", True, False)
    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String

    Private Sub frmGiftTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not txtLotNo.Focused Then
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            If txtLotNo.Text = "" Then Exit Sub
        End If
    End Sub

    Private Sub frmGiftTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtMetal As New DataTable
        Dim dtCounter As New DataTable
        defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        dtGridView = New DataTable
        dtGridView.Columns.Add("SNO", GetType(String))
        dtGridView.Columns.Add("TRANDATE", GetType(String))
        dtGridView.Columns.Add("RUNNO", GetType(String))
        dtGridView.Columns.Add("CARDID", GetType(Integer))
        dtGridView.Columns.Add("QTY", GetType(Integer))
        dtGridView.Columns.Add("AMOUNT", GetType(Double))
        dtGridView.Columns.Add("COSTID", GetType(String))
        dtGridView.Columns.Add("TCOSTID", GetType(String))
        dtGridView.Columns.Add("DUEDATE", GetType(String))
        dtGridView.Columns.Add("BATCHNO", GetType(String))
        dtGridView.Columns.Add("STATUS", GetType(String))
        dtGridView.Columns.Add("COLHEAD", GetType(String))
        'dtGridView.Columns.Add("ROID", GetType(Integer))
        'dtGridView.Columns("ROID").AutoIncrement = True
        'dtGridView.Columns("ROID").AutoIncrementSeed = 1
        'dtGridView.Columns("ROID").AutoIncrementStep = 1
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            '.Columns("ROID").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtGridView.Rows.Clear()
        chkRecDate.Checked = True
        chkRecDate.Checked = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtGridView.Rows.Clear()
        If _IsCostCentre Then cmbCostCentre.Enabled = True Else cmbCostCentre.Enabled = False
        If cmbCostCentre.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
            strSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
            strSql += " ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre)
            cmbCostCentre.Text = cnCostName
        End If
        'strSql = " SELECT SNO"
        'strSql += vbCrLf + " ,TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
        'strSql += vbCrLf + " WHERE 1=0"
        'Dim dtTemp As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtTemp)
        dtGridView.Clear()
        AddGrandTotal()
    End Sub

    Private Sub AddGrandTotal()
        For Each _dr As DataRow In dtGridView.Select("TRANDATE='GRAND TOTAL'")
            dtGridView.Rows.Remove(_dr)
            dtGridView.AcceptChanges()
        Next
        For Each _dr As DataRow In dtGridView.Select("TRANDATE='MARKED'")
            dtGridView.Rows.Remove(_dr)
            dtGridView.AcceptChanges()
        Next
        For Each _dr As DataRow In dtGridView.Select("TRANDATE='UNMARKED'")
            dtGridView.Rows.Remove(_dr)
            dtGridView.AcceptChanges()
        Next
        Dim Ro As DataRow = Nothing
        Dim QTY As Integer = Val(dtGridView.Compute("SUM(QTY)", "COLHEAD <> 'G' or COLHEAD IS NULL AND STATUS='MARKED'").ToString)
        Dim AMOUNT As Decimal = Val(dtGridView.Compute("SUM(AMOUNT)", "COLHEAD <> 'G' or COLHEAD IS NULL AND STATUS='MARKED'").ToString)
        Ro = dtGridView.NewRow
        Ro.Item("TRANDATE") = "MARKED"
        Ro.Item("QTY") = IIf(QTY <> 0, QTY, DBNull.Value)
        Ro.Item("AMOUNT") = IIf(AMOUNT <> 0, Format(AMOUNT, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then
            dtGridView.Rows.Add(Ro)
        End If
        QTY = Val(dtGridView.Compute("SUM(QTY)", "COLHEAD <> 'G' or COLHEAD IS NULL AND STATUS='UNMARKED'").ToString)
        AMOUNT = Val(dtGridView.Compute("SUM(AMOUNT)", "COLHEAD <> 'G' or COLHEAD IS NULL AND STATUS='UNMARKED'").ToString)
        Ro = dtGridView.NewRow
        Ro.Item("TRANDATE") = "UNMARKED"
        Ro.Item("QTY") = IIf(QTY <> 0, QTY, DBNull.Value)
        Ro.Item("AMOUNT") = IIf(AMOUNT <> 0, Format(AMOUNT, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then
            dtGridView.Rows.Add(Ro)
        End If
        QTY = Val(dtGridView.Compute("SUM(QTY)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        AMOUNT = Val(dtGridView.Compute("SUM(AMOUNT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Ro = dtGridView.NewRow
        Ro.Item("TRANDATE") = "GRAND TOTAL"
        Ro.Item("QTY") = IIf(QTY <> 0, QTY, DBNull.Value)
        Ro.Item("AMOUNT") = IIf(AMOUNT <> 0, Format(AMOUNT, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then
            dtGridView.Rows.Add(Ro)
        End If
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        If gridView.Rows.Count > 0 Then gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
        If gridView.Rows.Count > 1 Then gridView.Rows(gridView.RowCount - 2).DefaultCellStyle.BackColor = Color.LightBlue
        If gridView.Rows.Count > 2 Then gridView.Rows(gridView.RowCount - 3).DefaultCellStyle.BackColor = Color.Lavender
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkRecDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRecDate.CheckedChanged
        dtpDate.Enabled = chkRecDate.Checked
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        AddGrandTotal()
    End Sub


    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If objGPack.Validator_Check(Me) Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        BillDate = GetEntryDate(GetServerDate)
        Try
            For Each ro As DataRow In dtGridView.Rows
                strSql = "UPDATE " & cnStockDb & "..GVTRAN SET CHECKED = ''"
                strSql += " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            Next
            btnSearch_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.RowCount > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Gift Voucher", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridView.RowCount > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Gift Voucher", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selMetalId As String = Nothing
        Dim selCounterId As String = Nothing
        If objGPack.Validator_Check(Me) Then Exit Sub
        Dim _RunNo As String = txtLotNo.Text.ToString
        If _RunNo.StartsWith("GV") = False Then
            _RunNo = _RunNo.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
            _RunNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & _RunNo
        End If
        strSql = " SELECT SNO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
        strSql += vbCrLf + " ,CASE WHEN CHECKED='Y' THEN 'MARKED' ELSE 'UNMARKED' END STATUS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
        strSql += vbCrLf + " WHERE 1=1"
        If dtpDate.Enabled Then strSql += vbcrlf + "  AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(TOFLAG,'')=''"
        If rbtMarked.Checked Then
            strSql += vbCrLf + " AND ISNULL(CHECKED,'N')='Y'"
        ElseIf rbtPending.Checked Then
            strSql += vbCrLf + " AND ISNULL(CHECKED,'N')<>'Y'"
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND COSTID=(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "')"
        End If
        If txtLotNo.Text.ToString <> "" Then strSql += vbCrLf + " AND RUNNO='" & _RunNo & "'"
        Dim dtTemp As New DataTable
        'dtGridView.Clear()
        gridView.DataSource = Nothing
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)

        If Not dtTemp.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each ro As DataRow In dtTemp.Rows
            Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
            If RowCol.Length = 0 Then
                dtGridView.ImportRow(ro)
            End If
        Next
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        AddGrandTotal()
    End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.MistyRose
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "N"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "M"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function

    Private Sub txtLotNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim selMetalId As String = Nothing
            Dim selCounterId As String = Nothing
            If objGPack.Validator_Check(Me) Then Exit Sub
            If txtLotNo.Text.ToString = "" Then MsgBox("Voucher No should not empty..", MsgBoxStyle.Information) : txtLotNo.Focus() : Exit Sub
            Dim _RunNo As String = txtLotNo.Text.ToString
            If _RunNo.StartsWith("GV") = False Then
                _RunNo = _RunNo.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
                _RunNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & _RunNo
            End If

            strSql = " SELECT SNO"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
            strSql += vbCrLf + " ,'MARKED'  AS STATUS"
            strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
            strSql += vbCrLf + " WHERE 1=1"
            If dtpDate.Enabled Then strSql += vbCrLf + "  AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'')=''"
            strSql += vbCrLf + " AND ISNULL(CHECKED,'')=''"
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID=(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "')"
            End If
            If txtLotNo.Text.ToString <> "" Then strSql += vbCrLf + " AND RUNNO='" & _RunNo & "'"
            Dim dtTemp As New DataTable
            gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)

            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            Else
                strSql = "UPDATE " & cnStockDb & "..GVTRAN SET CHECKED = 'Y'"
                strSql += " WHERE SNO='" & dtTemp.Rows(0).Item("SNO").ToString & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If
            For Each ro As DataRow In dtTemp.Rows
                Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
                If RowCol.Length = 0 Then
                    dtGridView.ImportRow(ro)
                End If
            Next
            dtGridView.AcceptChanges()
            gridView.DataSource = dtGridView
            With gridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            AddGrandTotal()
        End If
    End Sub
End Class
