Imports System.Data.OleDb
Public Class WTransactionReport
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim StrSql As String
    Dim CatName As String
    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub View_PartlySales()
        Dim strFrom As String
        Dim strTo As String
        'Me.Cursor = Cursors.WaitCursor
        strFrom = dtpFrom.Value.ToString("yyyy-MM-dd")
        strTo = dtpTo.Value.ToString("yyyy-MM-dd")
        StrSql = "EXEC " & cnStockDb & "..SP_RPT_PARTLYSALES "
        StrSql += " @FromDate='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
        StrSql += " @Todate='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
        StrSql += " @METALNAME='ALL',"
        StrSql += " @ITEMNAME='ALL' ,"
        StrSql += " @COSTNAME='' ,"
        StrSql += " @NODEID='" & _AppId & ","
        StrSql += " @@SYSTEMID='" & _AppId & ","
        StrSql += " @@CNADMINDB='" & cnAdminDb & "',"
        StrSql += " @@CNSTOCKDB='" & cnStockDb & "',"
        StrSql += " @@COMPANYID='" & strCompanyId & "'"


        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If




    End Sub
    Private Sub View_DailyTrans()
        Try
            Dim strFrom As String
            Dim strTo As String
            'Me.Cursor = Cursors.WaitCursor
            strFrom = dtpFrom.Value.ToString("yyyy-MM-dd")
            strTo = dtpTo.Value.ToString("yyyy-MM-dd")
            StrSql = "EXEC " & cnStockDb & "..SP_RPT_WDailyAbstract "
            StrSql += " @FromDate='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
            StrSql += " @Todate='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
            If optSummarry.Checked = True Then
                StrSql += " @SUMMARY='Y'"
            Else
                StrSql += " @SUMMARY='N'"
            End If
            If chkPartlySales.Checked = True Then
                StrSql += " ,@PARTLYSALES ='Y'"
            Else
                StrSql += " ,@PARTLYSALES ='N'"
            End If
            If chkAppPendings.Checked = True Then
                StrSql += " ,@APPPENDINGS ='Y'"
            Else
                StrSql += " ,@APPPENDINGS ='N'"
            End If
            If chkCanceledBill.Checked = True Then
                StrSql += " ,@CANCELEDBILL ='Y'"
            Else
                StrSql += " ,@CANCELEDBILL ='N'"
            End If
            If chkMetalStcok.Checked = True Then
                StrSql += " ,@METALSTOCK ='Y'"
            Else
                StrSql += " ,@METALSTOCK ='N'"
            End If
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                dtpFrom.Focus()
                Exit Sub
            End If
            lblTitle.Visible = True
            lblTitle.Text = "Daily Transaction Report From " & Format(dtpFrom.Value, "dd/MM/yyyy") & " and " & Format(dtpTo.Value, "dd/MM/yyyy")
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                GridViewFormat()
                .Columns("RCOLHEAD").Visible = False
                .Columns("LCOLHEAD").Visible = False
                .Columns("RRESULT").Visible = False
                .Columns("LRESULT").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("SCROLL").Visible = False
                .Columns("LPARTICULAR").Width = 300
                .Columns("LGRSWEIGHT").Width = 60
                .Columns("LPUREWEIGHT").Width = 60
                .Columns("LMTSAL").Width = 60
                .Columns("LCUTWEIGHT").Width = 60
                .Columns("LAMOUNT").Width = 90
                .Columns("RPARTICULAR").Width = 300
                .Columns("RGRSWEIGHT").Width = 60
                .Columns("RPUREWEIGHT").Width = 60
                .Columns("RMTPUR").Width = 60
                .Columns("RCUTWEIGHT").Width = 60
                .Columns("RAMOUNT").Width = 90
                .Columns("LPARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LGRSWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LPUREWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LMTSAL").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LCUTWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LAMOUNT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RPARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RGRSWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RPUREWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RMTPUR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RCUTWEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RAMOUNT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RPARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LPARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("LPARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns("RPARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns("LGRSWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LPUREWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LMTSAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LCUTWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RGRSWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RPUREWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RMTPUR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RCUTWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LPARTICULAR").HeaderText = "Particular"
                .Columns("RPARTICULAR").HeaderText = "Particular"
                .Columns("LGRSWEIGHT").HeaderText = "GrsWt"
                .Columns("LPUREWEIGHT").HeaderText = "PurWt"
                .Columns("LMTSAL").HeaderText = "MetSal"
                .Columns("LCUTWEIGHT").HeaderText = "CashSal"
                .Columns("LAMOUNT").HeaderText = "Amount"
                .Columns("RGRSWEIGHT").HeaderText = "GrsWt"
                .Columns("RPUREWEIGHT").HeaderText = "PurWt"
                .Columns("RMTPUR").HeaderText = "MetPur"
                .Columns("RCUTWEIGHT").HeaderText = "CashSal"
                .Columns("RAMOUNT").HeaderText = "Amount"
                Call GridViewHeaderCreator(gridViewHeader)
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            MsgBox(ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click

        Call View_DailyTrans()

        'If chkPartlySales.Checked = True Then
        '    ''Call View_PartlySales()
        'End If
        'If chkAppPendings.Checked = True Then
        '    Call View_AppPendings()
        'End If
        'If chkCanceledBill.Checked = True Then
        '    Call View_CanceledBill()
        'End If
        'If chkMetalStcok.Checked = True Then
        '    Call View_MetalStock()
        'End If
    End Sub
    Private Sub View_AppPendings()
        StrSql = vbCrLf + " Select Partyname,"
        StrSql += " Sum(Pcs) as Pcs,Sum(GrsWt) as GrsWt "
        StrSql += vbCrLf + " from ("
        StrSql += vbCrLf + " Select "
        StrSql += vbCrLf + "(Select AcName from " & cnAdminDb & "..ACHEAD where AcCode=T.AcCode) as Partyname , "
        StrSql += vbCrLf + " Pcs,GrsWt "
        StrSql += vbCrLf + " from " & cnStockDb & "..ISSUE AS T WHERE TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " TRANTYPE='AI' and isnull(cancel,'')='' "
        StrSql += vbCrLf + " union all "
        StrSql += vbCrLf + " Select "
        StrSql += vbCrLf + "(Select AcName from " & cnAdminDb & "..ACHEAD where AcCode=T.AcCode) as Partyname , "
        StrSql += vbCrLf + "-1*Pcs,-1*GrsWt "
        StrSql += vbCrLf + " from " & cnStockDb & "..RECEIPT AS T WHERE TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " TRANTYPE='AR' and isnull(cancel,'')='' "
        StrSql += vbCrLf + " ) as xx GROUP BY Partyname Order by Partyname "
        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        'gridAppPending.DataSource = Nothing
        'gridAppPending.DataSource = dt
    End Sub
    Private Sub View_CanceledBill()
        StrSql = vbCrLf + " Select "
        StrSql += vbCrLf + "(Select AcName from " & cnAdminDb & "..ACHEAD where AcCode=T.AcCode) as Partyname , "
        StrSql += vbCrLf + " TranNo, TranDate, "
        StrSql += vbCrLf + "(Select ItemName from " & cnAdminDb & "..ITEMMAST where ItemId=T.ItemId) as ItemName , "
        StrSql += vbCrLf + "TagNo,Pcs,GrsWt,NetWt,"
        StrSql += vbCrLf + "Amount from " & cnStockDb & "..ISSUE AS T WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + "'" + dtpTo.Value.ToString("yyyy-MM-dd") & "'  and isnull(cancel,'')='Y' "
        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        'grdCanceledBill.DataSource = Nothing
        'grdCanceledBill.DataSource = dt

    End Sub
    Private Sub LOAD_CATNAME()
        Strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  CATNAME"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        CatName = ""
        For Each ro As DataRow In dt.Rows
            CatName += ro.Item("CATNAME").ToString & ","
        Next
        ''MsgBox(CatName.Remove(CatName.Length - 1, 1))
        CatName = CatName.Remove(CatName.Length - 1, 1)
    End Sub

    Private Sub View_MetalStock()
        Call LOAD_CATNAME()
        StrSql = vbCrLf + " Exec " + cnStockDb + "..SP_RPT_WMETALSTOCK_SUMMARY '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        StrSql += vbCrLf + ",'" + CatName + "'"
        StrSql += vbCrLf + ",'G'"
        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        'gridViewMetalStock.DataSource = Nothing
        'gridViewMetalStock.DataSource = dt
    End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
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

                        .Cells(2).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(2).Style.Font = reportHeadStyle2.Font

                        .Cells(3).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(3).Style.Font = reportHeadStyle2.Font


                        .Cells(4).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(4).Style.Font = reportHeadStyle2.Font

                        .Cells(5).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(5).Style.Font = reportHeadStyle2.Font


                        .Cells(6).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(6).Style.Font = reportHeadStyle2.Font

                        .Cells(7).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(7).Style.Font = reportHeadStyle2.Font
                    Case "S"
                        '.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        '.DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .Cells(0).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(0).Style.Font = reportSubTotalStyle.Font
                        .Cells(1).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(1).Style.Font = reportSubTotalStyle.Font

                        .Cells(2).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(2).Style.Font = reportSubTotalStyle.Font

                        .Cells(3).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(3).Style.Font = reportSubTotalStyle.Font


                        .Cells(4).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(4).Style.Font = reportSubTotalStyle.Font

                        .Cells(5).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(5).Style.Font = reportSubTotalStyle.Font


                        .Cells(6).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(6).Style.Font = reportSubTotalStyle.Font


                        .Cells(7).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(7).Style.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
                Select Case .Cells("RCOLHEAD").Value.ToString
                    Case "T"
                        .Cells(8).Style.BackColor = reportHeadStyle.BackColor
                        .Cells(8).Style.Font = reportHeadStyle.Font
                    Case "T1"
                        .Cells(8).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(8).Style.Font = reportHeadStyle2.Font
                        .Cells(9).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(9).Style.Font = reportHeadStyle2.Font

                        .Cells(10).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(10).Style.Font = reportHeadStyle2.Font

                        .Cells(11).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(11).Style.Font = reportHeadStyle2.Font


                        .Cells(12).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(12).Style.Font = reportHeadStyle2.Font

                        .Cells(13).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(13).Style.Font = reportHeadStyle2.Font


                        .Cells(14).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(14).Style.Font = reportHeadStyle2.Font

                        .Cells(15).Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells(15).Style.Font = reportHeadStyle2.Font

                    Case "S"

                        .Cells(8).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(8).Style.Font = reportSubTotalStyle.Font

                        .Cells(9).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(9).Style.Font = reportSubTotalStyle.Font

                        .Cells(10).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(10).Style.Font = reportSubTotalStyle.Font


                        .Cells(11).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(11).Style.Font = reportSubTotalStyle.Font

                        .Cells(12).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(12).Style.Font = reportSubTotalStyle.Font

                        .Cells(13).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(13).Style.Font = reportSubTotalStyle.Font

                        .Cells(14).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(14).Style.Font = reportSubTotalStyle.Font


                        .Cells(15).Style.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells(15).Style.Font = reportSubTotalStyle.Font

                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub GridCellAlignment()
        With gridView
            '.Columns("COUNTERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '.Columns("TAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("SALPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("SALWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("PARTLYDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("MISCWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("HOMESALES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("TOTSALES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub
    Private Sub GridColwidthFixing()
        With gridView
            If .Rows.Count > 0 Then
                .Columns("COUNTERNAME").Width = 200
                .Columns("SALPCS").Width = 60
                .Columns("TAGWT").Width = 100
                .Columns("SALWT").Width = 100
                .Columns("PARTLYDIFF").Width = 85
                .Columns("MISCWT").Width = 85
                .Columns("HOMESALES").Width = 85
                .Columns("DIAWT").Width = 70
                .Columns("TOTSALES").Width = 100
                .Columns("AMOUNT").Width = 120
            End If
        End With
    End Sub
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
    End Sub
    Private Sub dtpFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFrom.TextChanged

    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        gridviewHead.Visible = True
        StrSql = "SELECT ''[LPARTICULAR~LGRSWT~LPUREWT~LMTSAL~LCUTWEIGHT~LAMOUNT],''[RPARTICULAR~RGRSWT~RPUREWT~RMTPUR~RCUTWEIGHT~RAMOUNT],''SCROLL"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead

        gridviewHead.Columns("LPARTICULAR~LGRSWT~LPUREWT~LMTSAL~LCUTWEIGHT~LAMOUNT").HeaderText = "RECEIPT"
        gridviewHead.Columns("RPARTICULAR~RGRSWT~RPUREWT~RMTPUR~RCUTWEIGHT~RAMOUNT").HeaderText = "ISSUE"

        gridviewHead.Columns("SCROLL").HeaderText = ""
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
            'MsgBox(gridViewHeader.Columns(0).Name)
            'MsgBox(gridView.Columns(0).Name)
            'MsgBox(gridView.Columns(1).Name)
            'MsgBox(gridView.Columns(2).Name)
            'MsgBox(gridView.Columns(3).Name)



            .Columns("LPARTICULAR~LGRSWT~LPUREWT~LMTSAL~LCUTWEIGHT~LAMOUNT").Width = gridView.Columns("LPARTICULAR").Width + _
                gridView.Columns("LGRSWEIGHT").Width + gridView.Columns("LPUREWEIGHT").Width + gridView.Columns("LMTSAL").Width + gridView.Columns("LCUTWEIGHT").Width + gridView.Columns("LAMOUNT").Width
            .Columns("RPARTICULAR~RGRSWT~RPUREWT~RMTPUR~RCUTWEIGHT~RAMOUNT").Width = gridView.Columns("RPARTICULAR").Width + _
                gridView.Columns("RGRSWEIGHT").Width + gridView.Columns("RPUREWEIGHT").Width + gridView.Columns("RMTPUR").Width + gridView.Columns("RCUTWEIGHT").Width + gridView.Columns("RAMOUNT").Width


            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridViewHeader.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeader.Columns("SCROLL").Visible = False
            End If
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