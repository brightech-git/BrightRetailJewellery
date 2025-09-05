Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalPurWiseSummary
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
            gridView.DataSource = Nothing
            Dim selectcostid As String = IIf(chkCmbCostCentre.Text <> "ALL", GetSelectedCostId(chkCmbCostCentre, False), "ALL")
            Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL", GetSelectedCompanyId(chkcmbCompany, False), "ALL")
            Dim selectCashid As String = GetChecked_CheckedList(chkCmbCashCounter, False)
            Dim selectSalesid As String = GetChecked_CheckedList(chkCmbSalMan, False)
            gridView.DataSource = Nothing
            Me.Refresh()
            strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESABSTRACT_SALESWISE"
            strSql += vbCrLf + "@DBNAME='" & cnAdminDb & "'"
            strSql += vbCrLf + ",@FRMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@COSTID ='" & selectcostid & "'"
            strSql += vbCrLf + ",@COMPANYID ='" & selectcompid & "'"
            strSql += vbCrLf + ",@CASHNAME ='" & selectCashid & "'"
            strSql += vbCrLf + ",@EMPNAME ='" & selectSalesid & "'"
            strSql += vbCrLf + ",@UID='" & userId & "'"
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
            gridView.DataSource = dtGrid

            Dim _StrBatchno1 As String = ""
            Dim _StrBatchno2 As String = ""
            Dim _ChkBatchno As Boolean = False
            For i As Integer = 0 To gridView.Columns.Count - 1
                gridView.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            For i As Integer = 0 To gridView.Rows.Count - 1
                _StrBatchno1 = gridView.Rows(i).Cells("BATCHNO").Value.ToString

                If _StrBatchno1 <> _StrBatchno2 And gridView.Rows(i).Cells("RESULT").Value.ToString = "2" And _ChkBatchno = False Then
                    _ChkBatchno = True
                ElseIf _StrBatchno1 = _StrBatchno2 And gridView.Rows(i).Cells("RESULT").Value.ToString = "2" And _ChkBatchno = True Then
                    _ChkBatchno = True
                Else
                    _ChkBatchno = False
                End If

                Select Case gridView.Rows(i).Cells("RESULT").Value
                    Case "1"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        gridView.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "3"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "4"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "5"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        gridView.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "7"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "8"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        gridView.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "10"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "11"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "13"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        gridView.Rows(i).DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "16"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "17"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select

                If _ChkBatchno Then
                    gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                End If
                _StrBatchno2 = gridView.Rows(i).Cells("BATCHNO").Value.ToString
            Next

            With gridView
                .Columns("IITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
                .Columns("COLHEAD").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("PITEMID").Visible = False
                .Columns("SRITEMID").Visible = False
                .Columns("BATCHNO").Visible = False
                .Columns("PARTICULAR").Width = 170
                .Columns("PARTICULAR").Frozen = True
                .Columns("CHEQUE").HeaderText = "CHQ"
                .Columns("ADVANCE").HeaderText = "ADV"
                .Columns("CRCARD").HeaderText = "CC"
                .Columns("CASH").HeaderText = "RECV"
                .Columns("CRBALANCE").HeaderText = "BAL"
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
            If Not ResizeToolStripMenuItem.Checked Then ResizeToolStripMenuItem_Click(Me, New EventArgs)
            GridViewHeaderStyle()
            lblTitle.Text = "SALES PURCHASE DETAIL REPORT FROM " & dtpFrom.Value.ToString("dd/MM/yyyy") & " TO " & dtpTo.Value.ToString("dd/MM/yyyy") & ""
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COST CENTRE :" & chkCmbCostCentre.Text
            lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ResizeToolStripMenuItem.Checked = False
            btnView_Search.Enabled = True
            'FOR RESIZE COLUMN
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

            gridView.Focus()
        Catch ex As Exception
            MsgBox("Predefined conditions/dbs are un matched") : Exit Sub
        End Try
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL", GetType(String))
            .Columns.Add("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT", GetType(String))
            .Columns.Add("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL", GetType(String))
            .Columns.Add("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Caption = "SALES"
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Caption = "PURCHASE"
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Caption = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Caption = "PAYMENT"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").HeaderText = "SALES"
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").HeaderText = "PURCHASE"
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").HeaderText = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").HeaderText = "PAYMENT"

            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            .Columns("PARTICULAR").Frozen = True
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub
    Function funcColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width = gridView.Columns("IBILLDATE").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("IBILLNO").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("ITEMNAME").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("IITEMID").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("ITAGNO").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("IPCS").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("IGRSWT").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("INETWT").Width
            .Columns("IBILLDATE~IBILLNO~ITEMNAME~IITEMID~ITAGNO~IPCS~IGRSWT~INETWT~ITOTAL").Width += gridView.Columns("ITOTAL").Width
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Width = gridView.Columns("PBILLNO").Width
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView.Columns("PPCS").Width
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView.Columns("PGRSWT").Width
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView.Columns("PNETWT").Width
            .Columns("PBILLNO~PPCS~PGRSWT~PNETWT~PAMOUNT").Width += gridView.Columns("PAMOUNT").Width
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width = gridView.Columns("SRBILLNO").Width
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView.Columns("SRPCS").Width
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView.Columns("SRGRSWT").Width
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView.Columns("SRNETWT").Width
            .Columns("SRBILLNO~SRPCS~SRGRSWT~SRNETWT~SRTOTAL").Width += gridView.Columns("SRTOTAL").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width = gridView.Columns("CASH").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("CRCARD").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("CHIT").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("CHEQUE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("ADVANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("CRBALANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~CRBALANCE~ACTOTAL").Width += gridView.Columns("ACTOTAL").Width
            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
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
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        btnView_Search.Enabled = True
        dtpFrom.Focus()

    End Sub

    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView.Rows
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
            GridViewHeaderStyle()
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
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
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
        If gridView.Rows.Count > 0 Then
            Dim title As String
            title = "DAILY ABSTRACT REPORT"
            title += vbCrLf + Trim(lblTitle.Text)
            If MsgBox("Custom Print?.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                CustomPrint()
            Else
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text.ToString, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
            End If
        End If
    End Sub

    Private Sub CustomPrint()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim write As StreamWriter
        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        Dim lineprn As String = Space(60)
        'write.WriteLine(Chr(27) + "M")
        write.WriteLine(Chr(27) + "M" + Chr(15))
        write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
        write.WriteLine("DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
        write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
        Dim No As String = "No"
        Dim Desc As String = "Description"
        Dim IPcs As String = "Pcs"
        Dim IGrswt As String = "Wt."
        Dim ITotal As String = "Sales"

        Dim PGrswt As String = "Old Wt"
        Dim PTotal As String = "Value"

        Dim SRGrswt As String = "SR Wt"
        Dim SRTotal As String = "Value"

        Dim Cash As String = "Recv"
        Dim Chit As String = "Chit"
        Dim Chq As String = "Chq"
        Dim CCard As String = "CC"
        Dim Advance As String = "Adv"
        Dim Credit As String = "Bal"
        Dim lineCnt As Integer = 1
        No = RSet(No, 4)
        Desc = LSet(Desc, 15)
        IPcs = RSet(IPcs, 5)
        IGrswt = RSet(IGrswt, 8)
        ITotal = RSet(ITotal, 8)
        PGrswt = RSet(PGrswt, 8)
        PTotal = RSet(PTotal, 8)
        SRGrswt = RSet(SRGrswt, 8)
        SRTotal = RSet(SRTotal, 8)
        Cash = RSet(Cash, 8)
        Chit = RSet(Chit, 8)
        Chq = RSet(Chq, 8)
        CCard = RSet(CCard, 8)
        Advance = RSet(Advance, 8)
        Credit = RSet(Credit, 8)

        write.WriteLine(No & Space(1) & Desc & Space(1) & IPcs & Space(1) & IGrswt & Space(1) & ITotal & Space(1) & PGrswt & Space(1) & PTotal & _
                 Space(1) & SRGrswt & Space(1) & SRTotal & _
                 Space(1) & Cash & Space(1) & Chit & Space(1) & Chq & Space(1) & CCard & Space(1) & Advance & Space(1) & Credit)
        write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
        For i As Integer = 0 To gridView.Rows.Count - 1
            With gridView.Rows(i)

                If .Cells("RESULT").Value.ToString = "2" Then
                    No = RSet(.Cells("IBILLNO").Value.ToString, 4)
                    Desc = LSet(Mid(.Cells("ITAGNO").Value.ToString, 1, 15), 15)
                Else
                    No = ""
                    Desc = LSet(Mid(.Cells("PARTICULAR").Value.ToString, 1, 15), 15)
                End If

                IPcs = .Cells("IPcs").Value.ToString
                If .Cells("RESULT").Value.ToString = "13" Or .Cells("RESULT").Value.ToString = "14" Or .Cells("RESULT").Value.ToString = "15" Or .Cells("RESULT").Value.ToString = "16" Or .Cells("RESULT").Value.ToString = "17" Then
                    If .Cells("COLHEAD").Value.ToString = "G" Or .Cells("COLHEAD").Value.ToString = "T" Then
                        IGrswt = .Cells("IBILLNO").Value.ToString
                        ITotal = .Cells("ITEMNAME").Value.ToString
                        PGrswt = .Cells("ITAGNO").Value.ToString
                    Else
                        IGrswt = IIf(.Cells("IBILLNO").Value.ToString <> "", Format(Val(.Cells("IBILLNO").Value.ToString), "0"), "") '.Cells("IBILLNO").Value.ToString
                        ITotal = IIf(.Cells("ITEMNAME").Value.ToString <> "", Format(Val(.Cells("ITEMNAME").Value.ToString), "0"), "") ' .Cells("ITEMNAME").Value.ToString
                        PGrswt = IIf(.Cells("ITAGNO").Value.ToString <> "", Format(Val(.Cells("ITAGNO").Value.ToString), "0"), "") ' .Cells("ITAGNO").Value.ToString
                    End If

                Else
                    IGrswt = IIf(.Cells("IGrswt").Value.ToString <> "", Format(Val(.Cells("IGrswt").Value.ToString), "0.000"), "") ' .Cells("IGrswt").Value.ToString
                    ITotal = IIf(.Cells("ITOTAL").Value.ToString <> "", Format(Val(.Cells("ITOTAL").Value.ToString), "0"), "") ' .Cells("ITOTAL").Value.ToString
                    PGrswt = IIf(.Cells("PGrswt").Value.ToString <> "", Format(Val(.Cells("PGrswt").Value.ToString), "0.000"), "") ' .Cells("PGrswt").Value.ToString
                End If
                PTotal = IIf(.Cells("PAMOUNT").Value.ToString <> "", Format(Val(.Cells("PAMOUNT").Value.ToString), "0"), "") ' .Cells("PTOTAL").Value.ToString
                SRGrswt = IIf(.Cells("SRGRSWT").Value.ToString <> "", Format(Val(.Cells("SRGRSWT").Value.ToString), "0.000"), "") ' .Cells("SRGRSWT").Value.ToString
                SRTotal = IIf(.Cells("SRTOTAL").Value.ToString <> "", Format(Val(.Cells("SRTOTAL").Value.ToString), "0"), "") ' .Cells("SRTOTAL").Value.ToString
                Cash = IIf(.Cells("CASH").Value.ToString <> "", Format(Val(.Cells("CASH").Value.ToString), "0"), "") ' .Cells("CASH").Value.ToString
                Chit = IIf(.Cells("CHIT").Value.ToString <> "", Format(Val(.Cells("CHIT").Value.ToString), "0"), "") ' .Cells("CHIT").Value.ToString
                Chq = IIf(.Cells("CHEQUE").Value.ToString <> "", Format(Val(.Cells("CHEQUE").Value.ToString), "0"), "") ' .Cells("CHEQUE").Value.ToString
                CCard = IIf(.Cells("CRCARD").Value.ToString <> "", Format(Val(.Cells("CRCARD").Value.ToString), "0"), "") ' .Cells("CRCARD").Value.ToString
                Advance = IIf(.Cells("ADVANCE").Value.ToString <> "", Format(Val(.Cells("ADVANCE").Value.ToString), "0"), "") ' .Cells("ADVANCE").Value.ToString
                Credit = IIf(.Cells("CRBALANCE").Value.ToString <> "", Format(Val(.Cells("CRBALANCE").Value.ToString), "0"), "") ' .Cells("CRBALANCE").Value.ToString
                No = RSet(No, 4)
                Desc = LSet(Desc, 15)
                IPcs = RSet(IPcs, 5)
                IGrswt = RSet(IGrswt, 8)
                ITotal = RSet(ITotal, 8)
                PGrswt = RSet(PGrswt, 8)
                PTotal = RSet(PTotal, 8)
                SRGrswt = RSet(SRGrswt, 8)
                SRTotal = RSet(SRTotal, 8)
                Cash = RSet(Cash, 8)
                Chit = RSet(Chit, 8)
                Chq = RSet(Chq, 8)
                CCard = RSet(CCard, 8)
                Advance = RSet(Advance, 8)
                Credit = RSet(Credit, 8)
                If lineCnt <= 66 Then
                    If .Cells("COLHEAD").Value.ToString = "G" Or .Cells("COLHEAD").Value.ToString = "T" Then
                        write.WriteLine(No & Space(1) & Desc & Space(1) & IPcs & Space(1) & IGrswt & Space(1) & ITotal & Space(1) & PGrswt & Space(1) & PTotal & _
                                 Space(1) & SRGrswt & Space(1) & SRTotal & _
                                 Space(1) & Cash & Space(1) & Chit & Space(1) & Chq & Space(1) & CCard & Space(1) & Advance & Space(1) & Credit)
                    Else
                        write.WriteLine(No & Space(1) & Desc & Space(1) & IPcs & Space(1) & IGrswt & Space(1) & ITotal & Space(1) & PGrswt & Space(1) & PTotal & _
                                                         Space(1) & SRGrswt & Space(1) & SRTotal & _
                                                         Space(1) & Cash & Space(1) & Chit & Space(1) & Chq & Space(1) & CCard & Space(1) & Advance & Space(1) & Credit)
                    End If
                    lineCnt += 1
                Else
                    write.WriteLine(Chr(12))
                    write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
                    write.WriteLine("DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
                    write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
                    lineCnt = 0
                End If
            End With
        Next
        write.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------")
        For i As Integer = 0 To 4
            write.WriteLine()
        Next
        write.Close()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.BAT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.BAT")
        End If
        Dim writebat As StreamWriter

        Dim PrnName As String = ""
        Dim CondId As String = ""
        Try
            CondId = "'AGOLDREPORT40COLUMN" & Environ("NODE-ID").ToString & "'"
        Catch ex As Exception
            MsgBox("Set Node-Id", MsgBoxStyle.Information)
            Exit Sub
        End Try
        writebat = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.BAT")
        strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
        Dim dt As New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count <> 0 Then
            PrnName = dt.Rows(0).Item("CTLTEXT").ToString
        Else
            PrnName = "PRN"
        End If
        writebat.WriteLine("TYPE " & Application.StartupPath & "\SUMMARYPRINT.TXT>" & PrnName)
        writebat.Flush()
        writebat.Close()
        Shell(Application.StartupPath & "\SUMMARYPRINT.BAT")
    End Sub



    Private Sub btnExport_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_ColumnWidthChanged_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridView.Rows.Count > 0 Then
            GridViewHeaderStyle()
        End If
    End Sub

    Private Sub gridView_Scroll1(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
