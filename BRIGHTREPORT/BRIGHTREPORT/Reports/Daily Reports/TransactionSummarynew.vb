Imports System.Data.OleDb
Public Class TransactionSummarynew
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim StrCostFiltration As String
    Dim strFilter As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dsGrid As DataSet
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
    Dim SelectedCompany As String
    Dim Authorize As Boolean = False
    Dim Save As Boolean = False

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnStockDb & "..SoftControlTran"
        strSql += " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function


    Function funcGridViewStyle() As Integer
        With gridView
            
            With .Columns("NETWT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If rbtNetWt.Checked = True Or rbtBoth.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
            End With
            With .Columns("AMOUNT")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("VAT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TOT_recipt")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            
        End With
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

    End Function

    Private Sub Report()
        Try
            Dim GstFlag As Boolean = funcGstView(dtpFrom.Value)
            If Not chkLstCashCounter.CheckedItems.Count > 0 Then chkCashCounterSelectAll.Checked = True
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
            If Not chkLstNodeId.CheckedItems.Count > 0 Then chkSystemIdSelectAll.Checked = True
            Me.Cursor = Cursors.WaitCursor
            Dim SELECTEDCASHCOUNTER = GetChecked_CheckedList(chkLstCashCounter, False)
            Dim SELECTEDCOSTCENTRE = GetChecked_CheckedList(chkLstCostCentre, False)
            Dim SELECTEDSYSTEMID = GetChecked_CheckedList(chkLstNodeId, False)
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            If chkLstCashCounter.CheckedItems.Count = chkLstCashCounter.Items.Count Then
                SELECTEDCASHCOUNTER = "ALL"
            End If

            'Dim SELECTEDSYSTEMID As String = GetChecked_CheckedList(chkLstNodeId, False)
            strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_TRAN_SUMMARY_NEW"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@DBADMIN = '" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & SELECTEDCOSTCENTRE & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
            strSql += vbCrLf + " ,@SYSTEMID = '" & SELECTEDSYSTEMID & "'"
            strSql += vbCrLf + " ,@CASHNAME = '" & SELECTEDCASHCOUNTER & "'"
            strSql += vbCrLf + " ,@CASHOPENING = '" & IIf(chkWithCashOpen.Checked = True, "Y", "N") & "'"

            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 1 Then
                btnView_Search.Enabled = True
                MsgBox("Transaction Not Available", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim title As String
            title = "TRANSACTION SUMMARY NEW DETAILS FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If SELECTEDCOSTCENTRE <> "" Then title += " COSTCENTRE [" + SELECTEDCOSTCENTRE + "]"
            tabView.Show()
            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Visible = True
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            FillGridGroupStyle(gridView, "PARTICULAR")
            BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
            With gridView
                .Columns("PARTICULAR").Width = 350
                .Columns("GRSWT").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("NETWT").Visible = False
                If rbtGrossWt.Checked = True Then
                    .Columns("GRSWT").Visible = True

                ElseIf rbtNetWt.Checked = True Then
                    .Columns("NETWT").Visible = True
                ElseIf rbtBoth.Checked = True Then
                    .Columns("GRSWT").Visible = True
                    .Columns("NETWT").Visible = True
                End If
                .Columns("GRSWT").Width = 100
                .Columns("GRSWT").HeaderText = "GRSWT"
                .Columns("NETWT").Width = 100
                .Columns("NETWT").HeaderText = "NETWT"
                '.Columns("SELAMOUNT").Width = 100
                '.Columns("SELAMOUNT").HeaderText = "SALE AMOUNT"
                .Columns("VAT").Width = 80
                If GstFlag Then
                    .Columns("VAT").HeaderText = "GST"
                End If
                .Columns("TOT_PAYMENT").Width = 120
                .Columns("TOT_RECIPT").Width = 120
                .Columns("COLHEAD").Visible = False
                .Columns("KEYNO").Visible = False
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            If dt.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
            btnView_Search.Enabled = True
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            btnNew_Click(Me, New EventArgs)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Report()
        Prop_Sets()
    End Sub

    Private Sub frmTransactionDetailed_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmTransactionDetailed_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grp1.Location = New Point((ScreenWid - grp1.Width) / 2, ((ScreenHit - 128) - grp1.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        'If Authorize = False Then
        '    PnlFields.Enabled = False
        '    btnSave_OWN.Enabled = False
        'End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        SepStnPost = funcGetValues("SEPSTNPOST", "N")
        SepDiaPost = funcGetValues("SEPDIAPOST", "N")
        SepPrePost = funcGetValues("SEPPREPOST", "N")
        'chkCompanySelectAll.Checked = False
        'chkCostCentreSelectAll.Checked = False
        'chkSystemIdSelectAll.Checked = False

        lblTitle.Visible = False
        lblTitle.Text = ""
        gridView.DataSource = Nothing
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        'rbtGrossWt.Checked = True
        'chkWithPcs.Checked = False
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    ''Private Sub gridview_cellformatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case "T"
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.Font = reportHeadStyle.Font
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.ForeColor = Color.Red
    ''        Case "G"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''    End Select
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("CATNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .Cells("CATNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells("CATNAME").Style.ForeColor = Color.Red
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    'Function funcGridGroup(ByVal dt As DataTable, Optional ByVal SourceCol As Integer = 0, Optional ByVal DestCol As Integer = 1, Optional ByVal WithStoneField As Boolean = False, Optional ByVal sortFieldAlter As Boolean = False, Optional ByVal secondDestCol As Integer = -1) As DataSet
    '    Dim column1ToString As String = "#@$^&*@#!@#$#@!"
    '    Dim column2ToString As String = "##%&%@#!!@%#$#!"
    '    Dim ds As New DataSet
    '    ds.Clear()
    '    Dim subTotalDt As New DataTable("SubTotal")
    '    Dim titleDt As New DataTable("Title")
    '    subTotalDt.Clear()
    '    titleDt.Clear()
    '    titleDt.Columns.Add("Title")
    '    subTotalDt.Columns.Add("SubTotal")

    '    Dim tempDt As New DataTable("Result")
    '    tempDt.Clear()
    '    tempDt.Columns.Add("PARTICULAR", GetType(String))
    '    For cnt As Integer = 0 To dt.Columns.Count - 1
    '        tempDt.Columns.Add(dt.Columns(cnt).ColumnName, GetType(String))
    '    Next
    '    Dim ro As DataRow = Nothing
    '    Dim roSubTotal As DataRow = Nothing
    '    Dim roTitle As DataRow = Nothing
    '    Select Case secondDestCol
    '        Case -1

    '        Case Is <> -1

    '    End Select
    '    For rowIndex As Integer = 0 To dt.Rows.Count - 1
    '        ro = tempDt.NewRow
    '        With dt.Rows(rowIndex)
    '            If .Item(SourceCol).ToString <> "SUB TOTAL" And .Item(SourceCol).ToString <> "GRAND TOTAL" And .Item(DestCol).ToString <> "SUB TOTAL" And .Item(DestCol).ToString <> "GRAND TOTAL" And .Item(SourceCol).ToString <> "SALES DETAILS" And .Item(SourceCol).ToString <> "SALES[SA]" And .Item(SourceCol).ToString <> "SALES RETURN DETAILS" And .Item(SourceCol).ToString <> "SALES RETURN[SR]" And .Item(SourceCol).ToString <> "RECEIPT DETAILS" And .Item(SourceCol).ToString <> "RECEIPT[REC]" And .Item(SourceCol).ToString <> "PAYMENT DETAILS" And .Item(SourceCol).ToString <> "PAYMENTS[PAY]" And .Item(SourceCol).ToString <> "PURCHASE DETAILS" And .Item(SourceCol).ToString <> "PURCHASE[PUR]" And .Item(SourceCol).ToString <> "OTHERS" And .Item(SourceCol).ToString <> "DISCOUNT[DI]" And .Item(SourceCol).ToString <> "ROUND OFF[RO]" And .Item(SourceCol).ToString <> "HANDLING CHARGES[HC]" And .Item(SourceCol).ToString <> "TOTAL" And .Item(SourceCol).ToString <> "SA-SR+REC-PAY-PU-DI-RO+HC" And .Item(SourceCol).ToString <> "PAY COLLECTION" And .Item(SourceCol).ToString <> "CASH[CA]" And .Item(SourceCol).ToString <> "CREDIT CARD[CC]" And .Item(SourceCol).ToString <> "CHEQUE[CH]" And .Item(SourceCol).ToString <> "SAVINGS[SS]" And .Item(SourceCol).ToString <> "CA+CC+CH+SS" Then
    '                If column1ToString <> .Item(SourceCol).ToString Then
    '                    If WithStoneField = True Then
    '                        If .Item("Stone").ToString <> "2" Then
    '                            ro(0) = .Item(SourceCol).ToString
    '                            For cnt As Integer = 1 To dt.Columns.Count - 1
    '                                ro(cnt) = ""
    '                            Next
    '                            column1ToString = .Item(SourceCol).ToString
    '                            tempDt.Rows.Add(ro)
    '                            ''Adding Title Index
    '                            roTitle = titleDt.NewRow
    '                            roTitle("Title") = rowIndex + titleDt.Rows.Count
    '                            titleDt.Rows.Add(roTitle)
    '                        End If
    '                    Else
    '                        ro(0) = .Item(SourceCol).ToString
    '                        For cnt As Integer = 1 To dt.Columns.Count - 1
    '                            ro(cnt) = ""
    '                        Next
    '                        column1ToString = .Item(SourceCol).ToString
    '                        tempDt.Rows.Add(ro)
    '                        ''Adding Title Index
    '                        roTitle = titleDt.NewRow
    '                        roTitle("Title") = rowIndex + titleDt.Rows.Count
    '                        titleDt.Rows.Add(roTitle)
    '                    End If
    '                End If

    '            End If
    '            If .Item(SourceCol).ToString = "SUB TOTAL" Or .Item(DestCol).ToString = "SUB TOTAL" Or .Item(SourceCol).ToString = "SALES[SA]" Or .Item(SourceCol).ToString <> "SALES RETURN[SR]" Or .Item(SourceCol).ToString <> "RECEIPT[REC]" Or .Item(SourceCol).ToString <> "PAYMENTS[PAY]" Or .Item(SourceCol).ToString <> "PURCHASE[PUR]" Then
    '                ''Adding Group SubTotal Index into SubTotal Table
    '                roSubTotal = subTotalDt.NewRow
    '                roSubTotal("SubTotal") = rowIndex + titleDt.Rows.Count
    '                subTotalDt.Rows.Add(roSubTotal)
    '            End If
    '            ro = tempDt.NewRow
    '            If sortFieldAlter = False Then
    '                ro(0) = .Item(SourceCol).ToString
    '                If Trim(.Item(DestCol).ToString) <> "" Then
    '                    ro(0) = .Item(DestCol).ToString
    '                End If
    '                For cnt As Integer = 0 To dt.Columns.Count - 1
    '                    ro(cnt + 1) = .Item(cnt).ToString
    '                Next
    '            Else
    '                ro(0) = .Item(DestCol).ToString
    '                For cnt As Integer = 0 To dt.Columns.Count - 1
    '                    ro(cnt + 1) = .Item(cnt).ToString
    '                Next
    '            End If
    '            tempDt.Rows.Add(ro)
    '        End With
    '    Next
    '    ds.Tables.Add(tempDt)
    '    ds.Tables.Add(subTotalDt)
    '    ds.Tables.Add(titleDt)
    '    Return ds
    'End Function

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus

        LoadCostName(chkLstCostCentre, False)
        ProcAddCashCounter()
        ProcAddNodeId()
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
    End Sub

    Private Sub ProcAddCashCounter()
        chkLstCashCounter.Items.Clear()
        strSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        da = New OleDbDataAdapter(strSql, cn)
        dtCashCounter = New DataTable
        da.Fill(dtCashCounter)
        If dtCashCounter.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
                chkLstCashCounter.Items.Add(dtCashCounter.Rows(cnt).Item(1).ToString)
            Next
        End If
    End Sub

    Private Sub ProcAddNodeId()
        Try


            chkLstNodeId.Items.Clear()
            strSql = "SELECT DISTINCT SYSTEMID FROM ( "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE UNION ALL "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  UNION ALL "
            strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN )X "
            strSql += " ORDER BY SYSTEMID "
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    chkLstNodeId.Items.Add(dt.Rows(cnt).Item(0).ToString)
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkCashCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCashCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCashCounter, chkCashCounterSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkLstCashCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCashCounter.LostFocus
        If Not chkLstCashCounter.CheckedItems.Count > 0 Then
            chkCashCounterSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.LostFocus
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstNodeId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstNodeId.LostFocus
        If Not chkLstNodeId.CheckedItems.Count > 0 Then
            chkSystemIdSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkSystemIdSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSystemIdSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstNodeId, chkSystemIdSelectAll.Checked)
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



    Private Sub Prop_Sets()
        Dim obj As New TransactionSummarynew_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkSystemIdSelectAll = chkSystemIdSelectAll.Checked
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_chkCashCounterSelectAll = chkCashCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        obj.p_rbtGrossWt = rbtGrossWt.Checked
        obj.p_rbtNetWt = rbtNetWt.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_chkWithPcs = chkWithPcs.Checked
        obj.p_chkWithCashOpen = chkWithCashOpen.Checked
        SetSettingsObj(obj, Me.Name, GetType(TransactionSummarynew_Properties), Save)
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New TransactionSummarynew_Properties
        GetSettingsObj(obj, Me.Name, GetType(TransactionSummarynew_Properties), IIf(Authorize = False, True, False))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        chkSystemIdSelectAll.Checked = obj.p_chkSystemIdSelectAll
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, Nothing)
        chkCashCounterSelectAll.Checked = obj.p_chkCashCounterSelectAll
        SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, Nothing)
        rbtGrossWt.Checked = obj.p_rbtGrossWt
        rbtNetWt.Checked = obj.p_rbtNetWt
        rbtBoth.Checked = obj.p_rbtBoth
        chkWithPcs.Checked = obj.p_chkWithPcs
        chkWithCashOpen.Checked = obj.p_chkWithCashOpen
    End Sub

    Private Sub btnSave_OWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub

    Private Sub rbtGrossWt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGrossWt.CheckedChanged

    End Sub
End Class


Public Class TransactionSummarynew_Properties

    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkSystemIdSelectAll As Boolean = False
    Public Property p_chkSystemIdSelectAll() As Boolean
        Get
            Return chkSystemIdSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkSystemIdSelectAll = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private chkCashCounterSelectAll As Boolean = False
    Public Property p_chkCashCounterSelectAll() As Boolean
        Get
            Return chkCashCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCashCounterSelectAll = value
        End Set
    End Property
    Private chkWithCashOpen As Boolean = False
    Public Property p_chkWithCashOpen() As Boolean
        Get
            Return chkWithCashOpen
        End Get
        Set(ByVal value As Boolean)
            chkWithCashOpen = value
        End Set
    End Property
    Private chkLstCashCounter As New List(Of String)
    Public Property p_chkLstCashCounter() As List(Of String)
        Get
            Return chkLstCashCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCashCounter = value
        End Set
    End Property

    Private rbtGrossWt As Boolean = True
    Public Property p_rbtGrossWt() As Boolean
        Get
            Return rbtGrossWt
        End Get
        Set(ByVal value As Boolean)
            rbtGrossWt = value
        End Set
    End Property
    Private rbtNetWt As Boolean = False
    Public Property p_rbtNetWt() As Boolean
        Get
            Return rbtNetWt
        End Get
        Set(ByVal value As Boolean)
            rbtNetWt = value
        End Set
    End Property
    Private rbtBoth As Boolean = False
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private chkWithPcs As Boolean = False
    Public Property p_chkWithPcs() As Boolean
        Get
            Return chkWithPcs
        End Get
        Set(ByVal value As Boolean)
            chkWithPcs = value
        End Set
    End Property
End Class