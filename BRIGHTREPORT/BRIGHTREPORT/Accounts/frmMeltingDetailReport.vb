Imports System.Data.OleDb
Public Class frmMeltingDetailReport
    Dim strSql As String
    Dim dtCompany As New DataTable
    Dim dtOptions As New DataTable
    Dim dtAcName As New DataTable
    Dim dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim DS As DataSet
    Dim DT As DataTable
    Dim da As OleDbDataAdapter
    Dim PENDTRF_NETCALC As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_NETCALC", "N") = "Y", True, False)
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridViewHeader.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpTranDateFrom.Value = GetServerDate()
        dtpTranDateTo.Value = GetServerDate()
        FuncCostCenterLoad()
        chkPurchase.Checked = False
        dtpTranDateFrom.Enabled = False
        dtpTranDateTo.Enabled = False
        CmbTrantype.Text = "ALL"
        dtpFrom.Select()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHeader)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHeader)
    End Sub
    Private Sub FuncCostCenterLoad()

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"

        strSql += " ORDER BY RESULT,ACNAME"
        dtAcName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtAcName, "ACNAME", , "ALL")
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
        lblTitle.Visible = False
        CmbTrantype.Items.Clear()
        CmbTrantype.Items.Add("ALL")
        CmbTrantype.Items.Add("MISC ISSUE")
        CmbTrantype.Items.Add("PARTLY SALE")
        CmbTrantype.Items.Add("PURCHASE")
        CmbTrantype.Items.Add("SALES RETURN")
        CmbTrantype.Text = "ALL"
        chkPurchase.Checked = False
        dtpTranDateFrom.Enabled = False
        dtpTranDateTo.Enabled = False
        dtpFrom.Focus()
    End Sub
    Private Sub frmMeltingDetailReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FuncCostCenterLoad()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        AutoResizeToolStripMenuItem.Checked = False
        gridView.DataSource = Nothing
        gridViewHeader.DataSource = Nothing
        Dim SelectedCostId As String = GetSelectedCostId(chkCmbCostCentre, False)
        Dim selectedAccCode As String = GetSelectedAccCode(chkCmbAcName, False)
        Dim StrTrantype As String = "ALL"
        If CmbTrantype.Text <> "ALL" Then
            If CmbTrantype.Text = "MISC ISSUE" Then
                StrTrantype = "MI"
            ElseIf CmbTrantype.Text = "PARTLY SALE" Then
                StrTrantype = "SA"
            ElseIf CmbTrantype.Text = "PURCHASE" Then
                StrTrantype = "PU"
            ElseIf CmbTrantype.Text = "SALES RETURN" Then
                StrTrantype = "SR"
            End If
        End If
        Dim bagno As String
        bagno = txtmelting.Text
        strSql = "EXEC " & cnStockDb & "..[SP_RPT_MELTINGDETAIL]"
        strSql += vbCrLf + " @cnAdminDb='" & cnAdminDb & "'"
        strSql += vbCrLf + ",@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & systemId & "MELTINGDETAIL'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
        strSql += vbCrLf + ",@ACCODE='" & selectedAccCode & "'"
        strSql += vbCrLf + ",@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + ",@BAGNO = '" & txtmelting.Text & "'"
        strSql += vbCrLf + ",@FROMTRANDATE = '" & IIf(chkPurchase.Checked, dtpTranDateFrom.Value.ToString("yyyy-MM-dd"), "") & "'"
        strSql += vbCrLf + ",@TOTRANDATE = '" & IIf(chkPurchase.Checked, dtpTranDateTo.Value.ToString("yyyy-MM-dd"), "") & "'"
        strSql += vbCrLf + ",@TRANTYPE = '" & StrTrantype.ToString & "'"
        strSql += vbCrLf + ",@GROUPBY = '" & IIf(chkGroupByCatname.Checked, "Y", "N") & "'"
        da = New OleDbDataAdapter(strSql, cn)
        DS = New DataSet()
        DT = New DataTable()
        da.Fill(DS)
        DT = DS.Tables(0)
        If DT.Rows.Count - 2 < 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            gridViewHeader.DataSource = Nothing
            dtpFrom.Focus()
        Else
            gridView.DataSource = Nothing
            'gridView.DataSource = DT
            gridView.DataSource = DT
            GridViewFormat()
            ' gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            lblTitle.Visible = True
            lblTitle.Text = "MELTING DETAIL REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            lblTitle.Text += "  AT " & chkCmbCostCentre.Text & ""
        End If
    End Sub

    Private Sub frmMeltingDetailReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub GridViewFormat()
        'headGrid()
        GridViewHeaderStyle()

        gridView.Columns("DORDER").Visible = False
        gridView.Columns("RESULT").Visible = False
        If gridView.Columns.Contains("CATNAME") Then gridView.Columns("CATNAME").Visible = False
        'gridView.Columns("MR_PUREWT").Visible = False
        ' gridView.Columns("PI_ACNAME").Visible = False

        gridView.Columns("BILLDATE").ReadOnly = True
        gridView.Columns("BILLDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("BILLDATE").HeaderText = "BILL DATE"

        gridView.Columns("BAG_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("BAG_GRSWT").ReadOnly = True
        gridView.Columns("BAG_GRSWT").HeaderText = "GRSWT"

        gridView.Columns("MI_TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("MI_TRANDATE").ReadOnly = True
        gridView.Columns("MI_TRANDATE").HeaderText = "TRAN DATE"
        gridView.Columns("MI_TRANNO").ReadOnly = True
        gridView.Columns("MI_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("MI_ACNAME").Width = 150
        gridView.Columns("MI_ACNAME").ReadOnly = True
        gridView.Columns("MI_ACNAME").HeaderText = "ISSUE TO"
        gridView.Columns("MI_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_GRSWT").ReadOnly = True
        gridView.Columns("MI_GRSWT").HeaderText = "GRSWT"
        gridView.Columns("MI_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_NETWT").ReadOnly = True
        gridView.Columns("MI_NETWT").HeaderText = "NETWT"
        gridView.Columns("MI_PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_PUREWT").ReadOnly = True
        gridView.Columns("MI_PUREWT").Visible = False
        gridView.Columns("MI_PUREWT").HeaderText = "PUREWT"
        gridView.Columns("MI_TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_TOUCH").ReadOnly = True
        gridView.Columns("MI_TOUCH").HeaderText = "TOUCH"
        gridView.Columns("MI_PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_PURITY").ReadOnly = True
        gridView.Columns("MI_PURITY").HeaderText = "PURITY"
        gridView.Columns("MI_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MI_AMOUNT").ReadOnly = True
        gridView.Columns("MI_AMOUNT").HeaderText = "AMOUNT"
        gridView.Columns("MI_REMARKS").ReadOnly = True
        gridView.Columns("MI_REMARKS").HeaderText = "REMARKS"

        gridView.Columns("MR_TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("MR_TRANDATE").ReadOnly = True
        gridView.Columns("MR_TRANDATE").HeaderText = "TRAN DATE"
        gridView.Columns("MR_TRANNO").ReadOnly = True
        gridView.Columns("MR_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("MR_RECDWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_RECDWT").ReadOnly = True
        gridView.Columns("MR_RECDWT").HeaderText = "RECDWT"
        gridView.Columns("MR_MELTLOSS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_MELTLOSS").ReadOnly = True
        gridView.Columns("MR_MELTLOSS").HeaderText = "MELTLOSS"
        gridView.Columns("MR_TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_TOUCH").ReadOnly = True
        gridView.Columns("MR_TOUCH").HeaderText = "TOUCH"
        gridView.Columns("MR_SAMPLEWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_SAMPLEWT").ReadOnly = True
        gridView.Columns("MR_SAMPLEWT").HeaderText = "SAMPLEWT"
        gridView.Columns("MR_SCRAPWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_SCRAPWT").ReadOnly = True
        gridView.Columns("MR_SCRAPWT").HeaderText = "SCRAPWT"
        gridView.Columns("MR_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_NETWT").ReadOnly = True
        gridView.Columns("MR_NETWT").HeaderText = "NETWT"
        gridView.Columns("MR_RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_RATE").ReadOnly = True
        gridView.Columns("MR_RATE").HeaderText = "RATE"
        gridView.Columns("MR_CHARGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_CHARGE").ReadOnly = True
        gridView.Columns("MR_CHARGE").HeaderText = "MC"
        gridView.Columns("MR_PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_PURITY").ReadOnly = True
        gridView.Columns("MR_PURITY").HeaderText = "PURITY"
        gridView.Columns("MR_PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_PUREWT").ReadOnly = True
        gridView.Columns("MR_PUREWT").Visible = True
        gridView.Columns("MR_PUREWT").HeaderText = "PUREWT"
        gridView.Columns("MR_LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_LESSWT").ReadOnly = True
        gridView.Columns("MR_LESSWT").Visible = True
        gridView.Columns("MR_LESSWT").HeaderText = "LESSWT"
        gridView.Columns("MR_BAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MR_BAGWT").ReadOnly = True
        gridView.Columns("MR_BAGWT").Visible = True
        gridView.Columns("MR_BAGWT").HeaderText = "BAGWT"

        gridView.Columns("PI_TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("PI_TRANDATE").ReadOnly = True
        gridView.Columns("PI_TRANDATE").HeaderText = "TRAN DATE"
        gridView.Columns("PI_TRANNO").ReadOnly = True
        gridView.Columns("PI_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("PI_ACNAME").Width = 150
        gridView.Columns("PI_ACNAME").ReadOnly = True
        gridView.Columns("PI_ACNAME").HeaderText = "ISSUE TO"
        gridView.Columns("PI_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PI_GRSWT").ReadOnly = True
        gridView.Columns("PI_GRSWT").HeaderText = "GRSWT"
        gridView.Columns("PI_PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PI_PUREWT").ReadOnly = True
        gridView.Columns("PI_PUREWT").HeaderText = "PUREWT"
        gridView.Columns("PI_PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PI_PURITY").ReadOnly = True
        gridView.Columns("PI_PURITY").HeaderText = "PURITY"

        gridView.Columns("PR_TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("PR_TRANDATE").ReadOnly = True
        gridView.Columns("PR_TRANDATE").HeaderText = "TRAN DATE"
        gridView.Columns("PR_TRANNO").ReadOnly = True
        gridView.Columns("PR_TRANNO").HeaderText = "TRANNO"
        gridView.Columns("PR_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PR_GRSWT").ReadOnly = True
        gridView.Columns("PR_GRSWT").HeaderText = "GRSWT"
        gridView.Columns("PR_PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PR_PUREWT").ReadOnly = True
        gridView.Columns("PR_PUREWT").HeaderText = "PUREWT"
        gridView.Columns("PR_PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("PR_PURITY").ReadOnly = True
        gridView.Columns("PR_PURITY").HeaderText = "PURITY"
        funcColWidth()
        For Each gv As DataGridViewRow In gridView.Rows
            With gv
                Select Case .Cells("BAGNO").Value.ToString
                    Case "TOTAL"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.BackColor = Color.Wheat
                End Select
            End With
        Next
        'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        FormatGridColumns(gridView, False, False, False, False)
    End Sub
    Function headGrid() As Integer
        Dim dtHead As DataTable
        dtHead = Get_Header()
        With gridViewHeader
            .DataSource = dtHead
            With .Columns("PARTICULARS")
                .Width = gridView.Columns("BAGNO").Width + gridView.Columns("BAG_GRSWT").Width
                .HeaderText = "PARTICULARS"

            End With
            With .Columns("MELTINGISSUE")
                gridView.Columns("MI_ACNAME").Width = 200
                .Width = gridView.Columns("MI_TRANDATE").Width + gridView.Columns("MI_TRANNO").Width + gridView.Columns("MI_ACNAME").Width + gridView.Columns("MI_GRSWT").Width
                .HeaderText = "MELTING ISSUE"
            End With
            With .Columns("MELTINGRECEIPT")
                .Width = gridView.Columns("MR_TRANDATE").Width + gridView.Columns("MR_TRANNO").Width + gridView.Columns("MR_RECDWT").Width + gridView.Columns("MR_MELTLOSS").Width + gridView.Columns("MR_TOUCH").Width + gridView.Columns("MR_SAMPLEWT").Width + gridView.Columns("MR_SCRAPWT").Width + gridView.Columns("MR_NETWT").Width + gridView.Columns("MR_RATE").Width + gridView.Columns("MR_CHARGE").Width
                .HeaderText = "MELTING RECEIPT"
            End With
            With .Columns("PURIFICATIONISSUE")
                .Width = gridView.Columns("PI_TRANDATE").Width + gridView.Columns("PI_TRANNO").Width + gridView.Columns("PI_GRSWT").Width
                .HeaderText = "PURIFICATION ISSUE"
            End With
            With .Columns("PURIFICATIONRECEIPT")
                .Width = gridView.Columns("PR_TRANDATE").Width + gridView.Columns("PR_TRANNO").Width + gridView.Columns("PR_GRSWT").Width + gridView.Columns("PR_PUREWT").Width + gridView.Columns("PR_PURITY").Width
                .HeaderText = "PURIFICATION RECEIPT "
            End With
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function
    Public Function Get_Header()
        ''DETIAL Grid heading 
        strSql = " SELECT 'BAGNO~BAG_GRSWT'PARTICULARS,'MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT'MELTINGISSUE,'MR_TRANNO~MR_TRANDATE~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_PUREWT~MR_RATE~MR_CHARGE'MELTINGRECEIPT,'PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT'PURIFICATIONISSUE,'PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY 'PURIFICATIONRECEIPT where 1<>1" ','SCROLL'SCROLL

        Dim dtHead As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        Return dtHead
    End Function
    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function
    Public Function GetSelectedAccCode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function
    Private Sub AutoResizeToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
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
        'headGrid()
        GridViewHeaderStyle()
    End Sub

    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHeader.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("BILLDATE~BAGNO~BAG_GRSWT", GetType(String))
            .Columns.Add("MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT~MI_NETWT~MI_PUREWT~MI_TOUCH~MI_PURITY~MI_AMOUNT~MI_REMARKS", GetType(String))
            .Columns.Add("MR_TRANNO~MR_TRANDATE~MR_LESSWT~MR_BAGWT~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_RATE~MR_CHARGE~MR_PUREWT~MR_PURITY", GetType(String))
            .Columns.Add("PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT~PI_PUREWT~PI_PURITY", GetType(String))
            .Columns.Add("PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY", GetType(String))
            .Columns.Add("SCROLL", GetType(String))

            .Columns("BILLDATE~BAGNO~BAG_GRSWT").Caption = "PARTICULARS"
            .Columns("MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT~MI_NETWT~MI_PUREWT~MI_TOUCH~MI_PURITY~MI_AMOUNT~MI_REMARKS").Caption = "MELTING ISSUE"
            .Columns("MR_TRANNO~MR_TRANDATE~MR_LESSWT~MR_BAGWT~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_RATE~MR_CHARGE~MR_PUREWT~MR_PURITY").Caption = "MELTING RECEIPT"
            .Columns("PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT~PI_PUREWT~PI_PURITY").Caption = "PURIFICATION ISSUE"
            .Columns("PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY").Caption = "PURIFICATION RECEIPT"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHeader
            .DataSource = dtMergeHeader
            .Columns("BILLDATE~BAGNO~BAG_GRSWT").HeaderText = "PARTICULARS"
            .Columns("MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT~MI_NETWT~MI_PUREWT~MI_TOUCH~MI_PURITY~MI_AMOUNT~MI_REMARKS").HeaderText = "MELTING ISSUE"
            .Columns("MR_TRANNO~MR_TRANDATE~MR_LESSWT~MR_BAGWT~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_RATE~MR_CHARGE~MR_PUREWT~MR_PURITY").HeaderText = "MELTING RECEIPT"
            .Columns("PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT~PI_PUREWT~PI_PURITY").HeaderText = "PURIFICATION ISSUE"
            .Columns("PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY").HeaderText = "PURIFICATION RECEIPT"
            .Columns("SCROLL").HeaderText = ""
            '.Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Function funcColWidth() As Integer
        With gridViewHeader
            .Columns("BILLDATE~BAGNO~BAG_GRSWT").Width =
            IIf(gridView.Columns("BAGNO").Visible, gridView.Columns("BAGNO").Width, 0) _
            + IIf(gridView.Columns("BAG_GRSWT").Visible, gridView.Columns("BAG_GRSWT").Width, 0) _
            + IIf(gridView.Columns("BILLDATE").Visible, gridView.Columns("BILLDATE").Width, 0)
            .Columns("BILLDATE~BAGNO~BAG_GRSWT").HeaderText = "PARTICULARS"

            .Columns("MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT~MI_NETWT~MI_PUREWT~MI_TOUCH~MI_PURITY~MI_AMOUNT~MI_REMARKS").Width =
            IIf(gridView.Columns("MI_TRANNO").Visible, gridView.Columns("MI_TRANNO").Width, 0) _
            + IIf(gridView.Columns("MI_TRANDATE").Visible, gridView.Columns("MI_TRANDATE").Width, 0) _
            + IIf(gridView.Columns("MI_ACNAME").Visible, gridView.Columns("MI_ACNAME").Width, 0) _
            + IIf(gridView.Columns("MI_GRSWT").Visible, gridView.Columns("MI_GRSWT").Width, 0) _
            + IIf(gridView.Columns("MI_NETWT").Visible, gridView.Columns("MI_NETWT").Width, 0) _
            + IIf(gridView.Columns("MI_PUREWT").Visible, gridView.Columns("MI_PUREWT").Width, 0) _
            + IIf(gridView.Columns("MI_TOUCH").Visible, gridView.Columns("MI_TOUCH").Width, 0) _
            + IIf(gridView.Columns("MI_PURITY").Visible, gridView.Columns("MI_PURITY").Width, 0) _
            + IIf(gridView.Columns("MI_AMOUNT").Visible, gridView.Columns("MI_AMOUNT").Width, 0) _
            + IIf(gridView.Columns("MI_REMARKS").Visible, gridView.Columns("MI_REMARKS").Width, 0)
            .Columns("MI_TRANNO~MI_TRANDATE~MI_ACNAME~MI_GRSWT~MI_NETWT~MI_PUREWT~MI_TOUCH~MI_PURITY~MI_AMOUNT~MI_REMARKS").HeaderText = "MELTING ISSUE"

            .Columns("MR_TRANNO~MR_TRANDATE~MR_LESSWT~MR_BAGWT~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_RATE~MR_CHARGE~MR_PUREWT~MR_PURITY").Width =
            IIf(gridView.Columns("MR_TRANNO").Visible, gridView.Columns("MR_TRANNO").Width, 0) _
            + IIf(gridView.Columns("MR_TRANDATE").Visible, gridView.Columns("MR_TRANDATE").Width, 0) _
            + IIf(gridView.Columns("MR_RECDWT").Visible, gridView.Columns("MR_RECDWT").Width, 0) _
            + IIf(gridView.Columns("MR_MELTLOSS").Visible, gridView.Columns("MR_MELTLOSS").Width, 0) _
            + IIf(gridView.Columns("MR_TOUCH").Visible, gridView.Columns("MR_TOUCH").Width, 0) _
            + IIf(gridView.Columns("MR_SAMPLEWT").Visible, gridView.Columns("MR_SAMPLEWT").Width, 0) _
            + IIf(gridView.Columns("MR_SCRAPWT").Visible, gridView.Columns("MR_SCRAPWT").Width, 0) _
            + IIf(gridView.Columns("MR_NETWT").Visible, gridView.Columns("MR_NETWT").Width, 0) _
            + IIf(gridView.Columns("MR_RATE").Visible, gridView.Columns("MR_RATE").Width, 0) _
            + IIf(gridView.Columns("MR_CHARGE").Visible, gridView.Columns("MR_CHARGE").Width, 0) _
            + IIf(gridView.Columns("MR_PUREWT").Visible, gridView.Columns("MR_PUREWT").Width, 0) _
            + IIf(gridView.Columns("MR_PURITY").Visible, gridView.Columns("MR_PURITY").Width, 0) _
            + IIf(gridView.Columns("MR_LESSWT").Visible, gridView.Columns("MR_LESSWT").Width, 0) _
            + IIf(gridView.Columns("MR_BAGWT").Visible, gridView.Columns("MR_BAGWT").Width, 0)
            .Columns("MR_TRANNO~MR_TRANDATE~MR_LESSWT~MR_BAGWT~MR_RECDWT~MR_MELTLOSS~MR_TOUCH~MR_SAMPLEWT~MR_SCRAPWT~MR_NETWT~MR_RATE~MR_CHARGE~MR_PUREWT~MR_PURITY").HeaderText = "MELTING RECEIPT"

            .Columns("PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT~PI_PUREWT~PI_PURITY").Width =
            IIf(gridView.Columns("PI_ACNAME").Visible, gridView.Columns("PI_ACNAME").Width, 0) _
          + IIf(gridView.Columns("PI_TRANNO").Visible, gridView.Columns("PI_TRANNO").Width, 0) _
          + IIf(gridView.Columns("PI_TRANDATE").Visible, gridView.Columns("PI_TRANDATE").Width, 0) _
          + IIf(gridView.Columns("PI_GRSWT").Visible, gridView.Columns("PI_GRSWT").Width, 0) _
          + IIf(gridView.Columns("PI_PUREWT").Visible, gridView.Columns("PI_PUREWT").Width, 0) _
          + IIf(gridView.Columns("PI_PURITY").Visible, gridView.Columns("PI_PURITY").Width, 0)
            .Columns("PI_ACNAME~PI_TRANNO~PI_TRANDATE~PI_GRSWT~PI_PUREWT~PI_PURITY").HeaderText = "PURIFICATION ISSUE"

            .Columns("PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY").Width =
          IIf(gridView.Columns("PR_TRANNO").Visible, gridView.Columns("PR_TRANNO").Width, 0) _
          + IIf(gridView.Columns("PR_TRANDATE").Visible, gridView.Columns("PR_TRANDATE").Width, 0) _
          + IIf(gridView.Columns("PR_GRSWT").Visible, gridView.Columns("PR_GRSWT").Width, 0) _
           + IIf(gridView.Columns("PR_PUREWT").Visible, gridView.Columns("PR_PUREWT").Width, 0) _
          + IIf(gridView.Columns("PR_PURITY").Visible, gridView.Columns("PR_PURITY").Width, 0)
            .Columns("PR_TRANNO~PR_TRANDATE~PR_GRSWT~PR_PUREWT~PR_PURITY").HeaderText = "PURIFICATION RECEIPT"


        End With
    End Function

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub chkPurchase_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchase.CheckedChanged
        dtpTranDateFrom.Enabled = chkPurchase.Checked
        dtpTranDateTo.Enabled = chkPurchase.Checked
        dtpFrom.Enabled = Not chkPurchase.Checked
        dtpTo.Enabled = Not chkPurchase.Checked
    End Sub
End Class