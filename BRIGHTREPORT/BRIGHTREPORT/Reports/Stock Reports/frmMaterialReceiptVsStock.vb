Imports System.Data.OleDb
Public Class frmMaterialReceiptVsStock
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim dtMetal As New DataTable
    Dim Specificformat As Boolean = False
    Dim viewtype As String = ""
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        ResizeToolStripMenuItem.Checked = False
        gridView.DataSource = Nothing
        gridHead.DataSource = Nothing
        lblTitle.Text = ""
        Me.Refresh()
        If dtpTo.Value < dtpFrom.Value Then MsgBox("ToDate Should not Less then FromDate", MsgBoxStyle.Information) : dtpTo.Focus() : Exit Sub
        Dim MetalId As String = ""
        Dim chkCostId As String
        If ChkCmbMetal.Text = "ALL" Then
            MetalId = ""
        Else
            MetalId = GetQryStringForSp(ChkCmbMetal.Text, cnAdminDb & "..METALMAST", "METALID", "METALNAME", False)
        End If
        chkCostId = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        If chkCostId = "" Then chkCostId = "ALL"
        Dim TEMPTABLE As String = "TEMPTABLEDB..TEMP" & Trim(Guid.NewGuid.ToString.Substring(0, 8)) & "RECLOTTAG"
        strSql = "IF OBJECT_ID('" & TEMPTABLE & "') IS NOT NULL DROP TABLE " & TEMPTABLE
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_MATERIALRECEIPTVSSTOCK"
        strSql += vbCrLf + "   @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + "  ,@TEMPTABLE='" & TEMPTABLE & "'"
        strSql += vbCrLf + "  ,@FROMDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + "  ,@TODATE='" & dtpTo.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + "  ,@COSTID='" & chkCostId & "'"
        strSql += vbCrLf + "  ,@SUMMARY='" & IIf(chkSummary.Checked, "Y", "N") & "'"
        strSql += vbCrLf + "  ,@METALID='" & MetalId & "'"
        strSql += vbCrLf + "  ,@STARTDATE='" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  ,@TRANNO='" & IIf(chkSummary.Checked, "", txtTranNo_NUM.Text.ToString.Trim) & "'"
        dsGridView = New DataSet
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        If dsGridView.Tables(0).Rows.Count > 0 Then
            gridView.DataSource = dsGridView.Tables(0)
            funcGridStyle()
            pnlGridHeading.Visible = True
            Dim TITLE As String
            TITLE = " MATERIAL RECEIPT VS STOCK  " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " : " & chkCmbCostCentre.Text, "")
            lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            gridView.Select()
        Else
            MsgBox("No Records Found.")
            lblTitle.Text = ""
        End If
        Prop_Sets()
    End Sub
    Private Sub frmTrailBal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmTrailBal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGridHeading.Visible = False
        ''CostCentre
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT 'ALL' METALNAME"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbMetal, dtMetal, "METALNAME")
        btnNew_Click(Me, New EventArgs)
        dtpFrom.Focus()
        Prop_Gets()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        pnlGridTot.Visible = False
        chkSummary.Checked = True
        dtpFrom.Focus()
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.X) Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        End If
    End Sub
    Private Sub gridDetailView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Visible = False
            gridView.Focus()
        End If
    End Sub
    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Function FuncGridHead()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader

            If chkSummary.Checked = False Then
                .Columns.Add("TRANDATE~TRANNO~PARTICULAR~RPCS~RGRSWT~RNETWT~RDIAWT", GetType(String))
                .Columns.Add("ISSDATE~ISSNO~IPCS~IGRSWT~INETWT~IDIAWT", GetType(String))
                .Columns.Add("LOTNO~LOTDATE~LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT", GetType(String))
                .Columns.Add("RECDATE~TAGNO~TITEMNAME~TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS", GetType(String))
            Else
                .Columns.Add("SNO~PARTICULAR~TRANNO~TRANDATE", GetType(String))
                .Columns.Add("RPCS~RGRSWT~RNETWT~RDIAWT", GetType(String))
                .Columns.Add("IPCS~IGRSWT~INETWT~IDIAWT", GetType(String))
                .Columns.Add("LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT", GetType(String))
                .Columns.Add("TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS", GetType(String))
            End If
            .Columns.Add("SCROLL", GetType(String))

            If chkSummary.Checked = False Then
                .Columns("TRANDATE~TRANNO~PARTICULAR~RPCS~RGRSWT~RNETWT~RDIAWT").Caption = "RECEIPT"
                .Columns("ISSDATE~ISSNO~IPCS~IGRSWT~INETWT~IDIAWT").Caption = "ISSUE"
                .Columns("LOTNO~LOTDATE~LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT").Caption = "LOT"
                .Columns("RECDATE~TAGNO~TITEMNAME~TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS").Caption = "TAG/NONTAG"
            Else
                .Columns("SNO~PARTICULAR~TRANNO~TRANDATE").Caption = "PARTICULAR"
                .Columns("RPCS~RGRSWT~RNETWT~RDIAWT").Caption = "RECEIPT"
                .Columns("IPCS~IGRSWT~INETWT~IDIAWT").Caption = "ISSUE"
                .Columns("LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT").Caption = "LOT"
                .Columns("TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS").Caption = "TAG/NONTAG"
            End If
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = Nothing
        gridHead.DataSource = dtMergeHeader
    End Function
    Function funcGridStyle() As Integer
        With gridView
            If .Columns.Contains("RSNO") Then .Columns("RSNO").Visible = False
            If .Columns.Contains("LOTSNO") Then .Columns("LOTSNO").Visible = False
            If .Columns.Contains("LITEMNAME") Then .Columns("LITEMNAME").Visible = True
            If .Columns.Contains("TTRANDATE") Then .Columns("TTRANDATE").Visible = False
            If .Columns.Contains("METAL") Then .Columns("METAL").Visible = False
            If .Columns.Contains("CATCODE") Then .Columns("CATCODE").Visible = False
            If .Columns.Contains("DPCS") Then .Columns("DPCS").Visible = False
            If .Columns.Contains("DGRSWT") Then .Columns("DGRSWT").Visible = False
            If .Columns.Contains("DNETWT") Then .Columns("DNETWT").Visible = False
            If .Columns.Contains("DDIAWT") Then .Columns("DDIAWT").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            If chkSummary.Checked = False Then
                .Columns("LOTSNO").Visible = False
                .Columns("ISNO").Visible = False
                .Columns("TAGNO").Visible = False
                .Columns("TTRANNO").Visible = False
                .Columns("RECDATE").Visible = True
                .Columns("TITEMNAME").Visible = True
                .Columns("TITEMNAME").Width = 130
                .Columns("TITEMNAME").HeaderText = "PARTICULAR"
                .Columns("LOTNO").Width = 60
                .Columns("LOTDATE").Width = 80
                .Columns("LOTDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                .Columns("LITEMNAME").Width = 130
                .Columns("LITEMNAME").HeaderText = "PARTICULAR"
                .Columns("RECDATE").Width = 80
                .Columns("RECDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                .Columns("TAGNO").Width = 60
                .Columns("ISSNO").HeaderText = "TRANNO"
                .Columns("ISSDATE").HeaderText = "TRANDATE"
                .Columns("ISSDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                If .Columns.Contains("RUPDATE") Then .Columns("RUPDATE").Visible = False
                If .Columns.Contains("LUPDATE") Then .Columns("LUPDATE").Visible = False
                If .Columns.Contains("TUPDATE") Then .Columns("TUPDATE").Visible = False
                If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
            Else
                '.Columns("RUPDATE").DefaultCellStyle.Format = "dd-MM-yyyy hh:mm tt"
                '.Columns("LUPDATE").DefaultCellStyle.Format = "dd-MM-yyyy hh:mm tt"
                '.Columns("TUPDATE").DefaultCellStyle.Format = "dd-MM-yyyy hh:mm tt"
                '.Columns("RUPDATE").HeaderText = "UPDATED"
                '.Columns("LUPDATE").HeaderText = "UPDATED"
                '.Columns("TUPDATE").HeaderText = "UPDATED"
                '.Columns("LDIFF").HeaderText = "DIFF[D:H:M]"
                '.Columns("TDIFF").HeaderText = "DIFF[D:H:M]"
                .Columns("LITEMNAME").Width = 130
                .Columns("LITEMNAME").HeaderText = "PARTICULAR"
            End If
            .Columns("TRANNO").Visible = True
            .Columns("PARTICULAR").Width = 130
            .Columns("TRANDATE").Width = 80
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("PARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            For J As Integer = 0 To .Rows.Count - 1
                If .Rows(J).Cells("REMARKS").Value.ToString = "CANCEL" Then
                    .Rows(J).DefaultCellStyle.ForeColor = Color.Red
                End If
            Next
            Dim colhead(3) As String
            colhead(0) = "R"
            colhead(1) = "L"
            colhead(2) = "T"
            colhead(3) = "I"
            For i As Integer = 0 To colhead.Length - 1
                With .Columns(colhead(i) & "PCS")
                    .HeaderText = "PCS"
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns(colhead(i) & "GRSWT")
                    .HeaderText = "GRSWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With

                With .Columns(colhead(i) & "NETWT")
                    .HeaderText = "NETWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
                With .Columns(colhead(i) & "DIAWT")
                    .HeaderText = "DIAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
            Next
            For Each dgvRow As DataGridViewRow In gridView.Rows
                Select Case dgvRow.Cells("COLHEAD").Value.ToString
                    Case "G"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "T"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "S"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                End Select
            Next
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        funcGridHeaderStyle()
        Me.gridView.Refresh()
    End Function
    Function funcGridHeaderStyle() As Integer
        FuncGridHead()
        With gridHead
            If chkSummary.Checked = False Then
                With .Columns("TRANDATE~TRANNO~PARTICULAR~RPCS~RGRSWT~RNETWT~RDIAWT")
                    .Width = gridView.Columns("PARTICULAR").Width + gridView.Columns("TRANDATE").Width + gridView.Columns("TRANNO").Width + gridView.Columns("RPCS").Width + gridView.Columns("RGRSWT").Width + gridView.Columns("RNETWT").Width + gridView.Columns("RDIAWT").Width
                    .HeaderText = "RECEIPT"
                End With
                With .Columns("ISSDATE~ISSNO~IPCS~IGRSWT~INETWT~IDIAWT")
                    .Width = gridView.Columns("ISSDATE").Width + gridView.Columns("ISSNO").Width + gridView.Columns("IPCS").Width + gridView.Columns("IGRSWT").Width + gridView.Columns("INETWT").Width + gridView.Columns("IDIAWT").Width
                    .HeaderText = "ISSUE"
                End With
                With .Columns("LOTNO~LOTDATE~LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT")
                    .Width = gridView.Columns("LOTNO").Width + gridView.Columns("LOTDATE").Width + gridView.Columns("LITEMNAME").Width + gridView.Columns("LPCS").Width + gridView.Columns("LGRSWT").Width + gridView.Columns("LNETWT").Width + gridView.Columns("LDIAWT").Width
                    .HeaderText = "LOT"
                End With
                With .Columns("RECDATE~TAGNO~TITEMNAME~TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS")
                    .Width = gridView.Columns("RECDATE").Width + gridView.Columns("TAGNO").Width + gridView.Columns("TITEMNAME").Width + gridView.Columns("TPCS").Width + gridView.Columns("TGRSWT").Width + gridView.Columns("TNETWT").Width + gridView.Columns("TDIAWT").Width + gridView.Columns("REMARKS").Width
                    .HeaderText = "TAG/NONTAG"
                End With
            Else
                With .Columns("SNO~PARTICULAR~TRANNO~TRANDATE")
                    .Width = gridView.Columns("SNO").Width + gridView.Columns("PARTICULAR").Width + gridView.Columns("TRANDATE").Width + gridView.Columns("TRANNO").Width
                    .HeaderText = ""
                End With
                With .Columns("RPCS~RGRSWT~RNETWT~RDIAWT")
                    .Width = gridView.Columns("RPCS").Width + gridView.Columns("RGRSWT").Width + gridView.Columns("RNETWT").Width + gridView.Columns("RDIAWT").Width
                    .HeaderText = "RECEIPT"
                End With
                With .Columns("IPCS~IGRSWT~INETWT~IDIAWT")
                    .Width = gridView.Columns("IPCS").Width + gridView.Columns("IGRSWT").Width + gridView.Columns("INETWT").Width + gridView.Columns("IDIAWT").Width
                    .HeaderText = "ISSUE"
                End With
                With .Columns("LITEMNAME~LPCS~LGRSWT~LNETWT~LDIAWT")
                    .Width = gridView.Columns("LITEMNAME").Width + gridView.Columns("LPCS").Width + gridView.Columns("LGRSWT").Width + gridView.Columns("LNETWT").Width + gridView.Columns("LDIAWT").Width
                    .HeaderText = "LOT"
                End With
                With .Columns("TPCS~TGRSWT~TNETWT~TDIAWT~REMARKS")
                    .Width = gridView.Columns("TPCS").Width + gridView.Columns("TGRSWT").Width + gridView.Columns("TNETWT").Width + gridView.Columns("REMARKS").Width + gridView.Columns("TDIAWT").Width
                    .HeaderText = "TAG/NONTAG"
                End With
            End If
            With .Columns("SCROLL")
                .Width = 0
                .HeaderText = ""
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        'ResizeToolStripMenuItem.Checked = True
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
            funcGridHeaderStyle()
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmMaterialReceiptVsStock_Properties
        GetChecked_CheckedList(ChkCmbMetal, obj.p_chkCmbMetal)
        obj.p_ChkSummary = chkSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmMaterialReceiptVsStock_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmMaterialReceiptVsStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmMaterialReceiptVsStock_Properties))
        SetChecked_CheckedList(ChkCmbMetal, obj.p_chkCmbMetal, "ALL")
        chkSummary.Checked = obj.p_ChkSummary
    End Sub

    Private Sub chkSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSummary.CheckedChanged
        If chkSummary.Checked Then
            txtTranNo_NUM.Text = ""
            txtTranNo_NUM.Visible = False
            lblRecNo.Visible = False
        Else
            txtTranNo_NUM.Visible = True
            lblRecNo.Visible = True
        End If
    End Sub
End Class
Public Class frmMaterialReceiptVsStock_Properties
    Private ChkSummary As Boolean = False
    Public Property p_ChkSummary() As Boolean
        Get
            Return ChkSummary
        End Get
        Set(ByVal value As Boolean)
            ChkSummary = value
        End Set
    End Property
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property
End Class

