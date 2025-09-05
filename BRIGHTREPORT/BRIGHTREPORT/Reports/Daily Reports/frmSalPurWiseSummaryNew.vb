Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalPurWiseSummaryNew
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable
    Dim dtItemName As New DataTable
    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim SpecificPrint As Boolean = False

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
        strsql += vbcrlf + " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Private Sub SalesAbs()
        Try
            gridViewHead.DataSource = Nothing
            gridView_OWN.DataSource = Nothing
            Dim selectcostid As String = IIf(chkCmbCostCentre.Text <> "ALL", GetSelectedCostId(chkCmbCostCentre, False), "ALL")
            Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL", GetSelectedCompanyId(chkcmbCompany, False), "ALL")
            Dim selectCashid As String = GetChecked_CheckedList(chkCmbCashCounter, False)
            Dim selectmetal As String = IIf(cmbMetalid.Text <> "ALL", cmbMetalid.Text, "ALL")
            Dim selectItemid As String = GetChecked_CheckedList(chkCmbItemName, False)
            Dim selectSalesid As String = GetChecked_CheckedList(chkCmbSalMan, False)
            gridView_OWN.DataSource = Nothing
            Me.Refresh()
            strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESABSTRACT_SALESBILLWISE"
            strSql += vbCrLf + "@DBNAME='" & cnAdminDb & "'"
            strSql += vbCrLf + ",@FRMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@COSTID ='" & selectcostid & "'"
            strSql += vbCrLf + ",@COMPANYID ='" & selectcompid & "'"
            strSql += vbCrLf + ",@CASHNAME ='" & selectCashid & "'"
            strSql += vbCrLf + ",@EMPNAME ='" & selectSalesid & "'"
            strSql += vbCrLf + ",@UID='" & userId & "'"
            strSql += vbCrLf + ",@ITEMNAME='" & selectItemid & "'"
            strSql += vbCrLf + ",@METALNAME='" & selectmetal & "'"
            strSql += vbCrLf + ",@BILLSUMM='" & IIf(ChkBillwiseTotal.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@DAYWISE='" & IIf(chkDayWise.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            dsGridView = New DataSet()
            da.Fill(dsGridView)
            Dim dtGrid As New DataTable
            If Not dsGridView.Tables.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUE "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPACCTRANN') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPACCTRANN "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCASH') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCASH "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPEMP') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPEMP "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPADDRESS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPADDRESS"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALEABSTRACT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALEABSTRACT"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALEABSTRACT_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALEABSTRACT_RES  "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPPERSONALINFO') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPERSONALINFO"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPPERSONALINFO_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPERSONALINFO_RES"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            dtGrid = dsGridView.Tables(0).Copy()
            gridView_OWN.DataSource = dtGrid
            If chkDayWise.Checked Then
                GridStyle_Daywise(dtGrid.Rows.Count - 1)
            Else
                GridStyle()
            End If
            If Not ResizeToolStripMenuItem.Checked Then ResizeToolStripMenuItem_Click(Me, New EventArgs)
            If chkDayWise.Checked Then
                GridViewHeaderStyle_DayWise()
            Else
                GridViewHeaderStyle()
            End If
            lblTitle.Text = "SALES PURCHASE DETAIL REPORT FROM " & dtpFrom.Value.ToString("dd/MM/yyyy") & " TO " & dtpTo.Value.ToString("dd/MM/yyyy") & ""
            If chkcmbCompany.Text <> "" And chkcmbCompany.Text <> "ALL" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COMPANY :" & chkcmbCompany.Text
            If chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COST CENTRE :" & chkCmbCostCentre.Text
            If chkCmbCashCounter.Text <> "" And chkCmbCashCounter.Text <> "ALL" Then lblTitle.Text = lblTitle.Text & vbCrLf & " CASH COUNTER :" & chkCmbCashCounter.Text
            If chkCmbSalMan.Text <> "" And chkCmbSalMan.Text <> "ALL" Then lblTitle.Text = lblTitle.Text & vbCrLf & " SALES MAN :" & chkCmbSalMan.Text
            lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ResizeToolStripMenuItem.Checked = False
            btnView_Search.Enabled = True
            'FOR RESIZE COLUMN
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView_OWN.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

            gridView_OWN.Focus()
        Catch ex As Exception
            MsgBox("Predefined conditions/dbs are un matched") : Exit Sub
        End Try
    End Sub
    Private Sub GridStyle()
        Dim _StrBatchno1 As String = ""
        Dim _StrBatchno2 As String = ""
        Dim _ChkBatchno As Boolean = False
        For i As Integer = 0 To gridView_OWN.Columns.Count - 1
            gridView_OWN.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        For i As Integer = 0 To gridView_OWN.Rows.Count - 1
            _StrBatchno1 = gridView_OWN.Rows(i).Cells("BATCHNO").Value.ToString

            If _StrBatchno1 <> _StrBatchno2 And gridView_OWN.Rows(i).Cells("RESULT").Value.ToString = "2" And _ChkBatchno = False Then
                _ChkBatchno = True
            ElseIf _StrBatchno1 = _StrBatchno2 And gridView_OWN.Rows(i).Cells("RESULT").Value.ToString = "2" And _ChkBatchno = True Then
                _ChkBatchno = True
            Else
                _ChkBatchno = False
            End If

            Select Case gridView_OWN.Rows(i).Cells("RESULT").Value
                Case "0"
                    gridView_OWN.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "2"
                    gridView_OWN.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "3"
                    gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If gridView_OWN.Columns.Contains("BATCHNO") Then gridView_OWN.Rows(i).Cells("BATCHNO").Value = DBNull.Value
                    If gridView_OWN.Columns.Contains("IBILLDATE") Then gridView_OWN.Rows(i).Cells("IBILLDATE").Value = DBNull.Value
                Case "4"
                    gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "14"
                    gridView_OWN.Rows(i).Cells("ITAGNO").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView_OWN.Rows(i).Cells("ITAGNO").Style.BackColor = Color.MistyRose
                Case "15"
                    gridView_OWN.Rows(i).Cells("ITEMNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView_OWN.Rows(i).Cells("ITEMNAME").Style.BackColor = Color.Lavender
                Case "16"
                    gridView_OWN.Rows(i).Cells("ITAGNO").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView_OWN.Rows(i).Cells("ITEMNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                Case "17"
                    gridView_OWN.Rows(i).Cells("ITEMNAME").Style.Alignment = DataGridViewContentAlignment.MiddleRight
            End Select

            Select Case gridView_OWN.Rows(i).Cells("COLHEAD").Value.ToString
                Case "S"
                    gridView_OWN.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "T"
                    gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "G"
                    gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                    gridView_OWN.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                    gridView_OWN.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End Select

            If _ChkBatchno Then
                gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
            End If
            _StrBatchno2 = gridView_OWN.Rows(i).Cells("BATCHNO").Value.ToString
        Next
        With gridView_OWN
            .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CASH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CRCARD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CHIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ADVANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CRBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IBILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PBILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRBILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("PITEMID").Visible = False
            .Columns("SRITEMID").Visible = False
            '.Columns("BATCHNO").Visible = False
            .Columns("PARTICULAR").Width = 170
            .Columns("PARTICULAR").Frozen = True
            .Columns("IBILLNO").HeaderText = "BILLNO"
            .Columns("IBILLDATE").HeaderText = "BILLDATE"
            .Columns("IBILLDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("IITEMID").HeaderText = "ITEMID"

            .Columns("IPCS").HeaderText = "PCS"
            .Columns("ITAGNO").HeaderText = "TAGNO"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("ITOTAL").HeaderText = "AMOUNT"

            .Columns("PBILLNO").HeaderText = "BILLNO"
            .Columns("PPCS").HeaderText = "PCS"
            .Columns("PGRSWT").HeaderText = "GRSWT"
            .Columns("PNETWT").HeaderText = "NETWT"
            .Columns("PAMOUNT").HeaderText = "AMOUNT"

            .Columns("SRBILLNO").HeaderText = "BILLNO"
            .Columns("SRPCS").HeaderText = "PCS"
            .Columns("SRGRSWT").HeaderText = "GRSWT"
            .Columns("SRNETWT").HeaderText = "NETWT"
            .Columns("SRTOTAL").HeaderText = "AMOUNT"
            .Columns("ACTOTAL").HeaderText = "TOTAL"

        End With
    End Sub
    Private Sub GridStyle_Daywise(dtCount As Integer)
        For i As Integer = 0 To gridView_OWN.Columns.Count - 1
            gridView_OWN.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        gridView_OWN.Rows(dtCount).Cells(0).Style.BackColor = reportHeadStyle.BackColor
        gridView_OWN.Rows(dtCount).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
        gridView_OWN.Rows(dtCount).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        With gridView_OWN
            .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ICGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("PPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("SRPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("CASH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CRCARD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CHIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ADVANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CRBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("IBILLDATE").HeaderText = "BILLDATE"

            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("ICGST").HeaderText = "CGST"
            .Columns("ISGST").HeaderText = "SGST"
            .Columns("ITOTAL").HeaderText = "AMOUNT"

            .Columns("PPCS").HeaderText = "PCS"
            .Columns("PGRSWT").HeaderText = "GRSWT"
            .Columns("PNETWT").HeaderText = "NETWT"
            .Columns("PAMOUNT").HeaderText = "AMOUNT"

            .Columns("SRPCS").HeaderText = "PCS"
            .Columns("SRGRSWT").HeaderText = "GRSWT"
            .Columns("SRNETWT").HeaderText = "NETWT"
            .Columns("SRTOTAL").HeaderText = "AMOUNT"
            .Columns("ACTOTAL").HeaderText = "TOTAL"

        End With
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("BATCHNO", GetType(String))
            .Columns.Add("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL", GetType(String))
            .Columns.Add("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT", GetType(String))
            .Columns.Add("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL", GetType(String))
            .Columns.Add("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("BATCHNO").Caption = ""
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Caption = "SALES"
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Caption = "PURCHASE"
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Caption = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Caption = "PAYMENT"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("BATCHNO").HeaderText = ""
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").HeaderText = "SALES"
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").HeaderText = "PURCHASE"
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").HeaderText = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").HeaderText = "PAYMENT"

            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView_OWN.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
                If gridView_OWN.Columns(cnt).Visible Then colWid += gridView_OWN.Columns(cnt).Width
            Next
            .Columns("PARTICULAR").Frozen = True
            If colWid >= gridView_OWN.Width Then
                .Columns("SCROLL").Visible = CType(gridView_OWN.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub
    Private Sub GridViewHeaderStyle_DayWise()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL", GetType(String))
            .Columns.Add("PPCS~PGRSWT~PNETWT~PAMOUNT", GetType(String))
            .Columns.Add("SRPCS~SRGRSWT~SRNETWT~SRTOTAL", GetType(String))
            .Columns.Add("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL", GetType(String))

            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Caption = "SALES"
            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").Caption = "PURCHASE"
            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Caption = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Caption = "PAYMENT"
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").HeaderText = "SALES"
            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").HeaderText = "PURCHASE"
            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").HeaderText = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").HeaderText = "PAYMENT"

            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth_daywise()
            gridView_OWN.Focus()
        End With

    End Sub
    Function funcColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView_OWN.Columns("PARTICULAR").Width
            .Columns("BATCHNO").Width = gridView_OWN.Columns("BATCHNO").Width
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width = IIf(gridView_OWN.Columns("IBILLDATE").Visible = False, 0, gridView_OWN.Columns("IBILLDATE").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("IBILLNO").Visible = False, 0, gridView_OWN.Columns("IBILLNO").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("ITEMNAME").Visible = False, 0, gridView_OWN.Columns("ITEMNAME").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("IITEMID").Visible = False, 0, gridView_OWN.Columns("IITEMID").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("ITAGNO").Visible = False, 0, gridView_OWN.Columns("ITAGNO").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("IPCS").Visible = False, 0, gridView_OWN.Columns("IPCS").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("IGRSWT").Visible = False, 0, gridView_OWN.Columns("IGRSWT").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("INETWT").Visible = False, 0, gridView_OWN.Columns("INETWT").Width)
            .Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("ITOTAL").Visible = False, 0, gridView_OWN.Columns("ITOTAL").Width)
            '.Columns("IBILLDATE~IBILLNO~IITEMID~ITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += IIf(gridView_OWN.Columns("BATCHNO").Visible = False, 0, gridView_OWN.Columns("BATCHNO").Width)
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Width = IIf(gridView_OWN.Columns("PBILLNO").Visible = False, 0, gridView_OWN.Columns("PBILLNO").Width)
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += IIf(gridView_OWN.Columns("PPCS").Visible = False, 0, gridView_OWN.Columns("PPCS").Width)
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += IIf(gridView_OWN.Columns("PGRSWT").Visible = False, 0, gridView_OWN.Columns("PGRSWT").Width)
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += IIf(gridView_OWN.Columns("PNETWT").Visible = False, 0, gridView_OWN.Columns("PNETWT").Width)
            .Columns("PBILLNO~PITEMID~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += IIf(gridView_OWN.Columns("PAMOUNT").Visible = False, 0, gridView_OWN.Columns("PAMOUNT").Width)
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width = IIf(gridView_OWN.Columns("SRBILLNO").Visible = False, 0, gridView_OWN.Columns("SRBILLNO").Width)
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += IIf(gridView_OWN.Columns("SRPCS").Visible = False, 0, gridView_OWN.Columns("SRPCS").Width)
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += IIf(gridView_OWN.Columns("SRGRSWT").Visible = False, 0, gridView_OWN.Columns("SRGRSWT").Width)
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += IIf(gridView_OWN.Columns("SRNETWT").Visible = False, 0, gridView_OWN.Columns("SRNETWT").Width)
            .Columns("SRBILLNO~SRITEMID~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += IIf(gridView_OWN.Columns("SRTOTAL").Visible = False, 0, gridView_OWN.Columns("SRTOTAL").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width = IIf(gridView_OWN.Columns("CASH").Visible = False, 0, gridView_OWN.Columns("CASH").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("CRCARD").Visible = False, 0, gridView_OWN.Columns("CRCARD").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("CHIT").Visible = False, 0, gridView_OWN.Columns("CHIT").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("CHEQUE").Visible = False, 0, gridView_OWN.Columns("CHEQUE").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("ADVANCE").Visible = False, 0, gridView_OWN.Columns("ADVANCE").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("CRBALANCE").Visible = False, 0, gridView_OWN.Columns("CRBALANCE").Width)
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += IIf(gridView_OWN.Columns("ACTOTAL").Visible = False, 0, gridView_OWN.Columns("ACTOTAL").Width)
            .Columns("SCROLL").Visible = CType(gridView_OWN.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
        End With
    End Function
    Function funcColWidth_daywise() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width = gridView_OWN.Columns("IBILLDATE").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("IPCS").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("IGRSWT").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("INETWT").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("ICGST").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("ISGST").Width
            .Columns("IBILLDATE~IPCS~IGRSWT~INETWT~ICGST~ISGST~ITOTAL").Width += gridView_OWN.Columns("ITOTAL").Width

            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView_OWN.Columns("PPCS").Width
            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView_OWN.Columns("PGRSWT").Width
            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView_OWN.Columns("PNETWT").Width
            .Columns("PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView_OWN.Columns("PAMOUNT").Width

            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView_OWN.Columns("SRPCS").Width
            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView_OWN.Columns("SRGRSWT").Width
            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView_OWN.Columns("SRNETWT").Width
            .Columns("SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView_OWN.Columns("SRTOTAL").Width

            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width = gridView_OWN.Columns("CASH").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("CRCARD").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("CHIT").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("CHEQUE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("ADVANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("CRBALANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView_OWN.Columns("ACTOTAL").Width
        End With
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        SalesAbs()
    End Sub
    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblTitle.Visible = True
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtcompany, "COMPANYNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT 'ALL' CASHNAME,'ALL' CASHID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT CASHNAME,CONVERT(VARCHAR,CASHID),2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER"
        strSql += " ORDER BY RESULT,CASHNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCashCounter, dtCostCentre, "CASHNAME", , "ALL")

        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItemName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , "ALL")

        strSql = " SELECT 'ALL' METALNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += " ORDER BY RESULT,METALNAME"
        objGPack.FillCombo(strSql, cmbMetalid, True, True)


        strSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT EMPNAME,CONVERT(VARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        strSql += " ORDER BY RESULT,EMPNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbSalMan, dtCostCentre, "EMPNAME", , "ALL")

        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView_OWN.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        btnView_Search.Enabled = True
        dtpFrom.Focus()

    End Sub

    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView_OWN.Rows
            With dgvView
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub



    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub


    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView_OWN.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            If chkDayWise.Checked Then
                GridViewHeaderStyle_DayWise()
            Else
                GridViewHeaderStyle()
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        'If gridViewHead.ColumnCount > 0 Then
        '    If spbaserpt = True Then
        '        funcColWidth1()
        '    Else
        '        funcColWidth()
        '    End If
        'End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView_OWN.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub frmSalesRegister_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            tabMain.SelectedTab = tabGen
        End If
    End Sub

    Private Sub btnPrint_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Dim title As String
            title = "DAILY ABSTRACT REPORT"
            title += vbCrLf + Trim(lblTitle.Text)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text.ToString, gridView_OWN, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExport_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_ColumnWidthChanged_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView_OWN.ColumnWidthChanged
        If gridView_OWN.Rows.Count > 0 Then
            If chkDayWise.Checked Then
                GridViewHeaderStyle_DayWise()
            Else
                GridViewHeaderStyle()
            End If
        End If
    End Sub

    Private Sub gridView_Scroll1(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView_OWN.Scroll
        Try
            If Not chkDayWise.Checked Then
                If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                    gridViewHead.HorizontalScrollingOffset = e.NewValue
                    gridViewHead.Columns("SCROLL").Visible = CType(gridView_OWN.Controls(1), VScrollBar).Visible
                    gridViewHead.Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub chkDayWise_CheckedChanged(sender As Object, e As EventArgs)
        If chkDayWise.Checked Then
            ChkBillwiseTotal.Checked = False
        End If
    End Sub

    Private Sub ChkBillwiseTotal_CheckedChanged(sender As Object, e As EventArgs)
        If ChkBillwiseTotal.Checked Then
            chkDayWise.Checked = False
        End If
    End Sub
End Class
