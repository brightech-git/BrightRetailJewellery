Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesCatSummary
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
            strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESABSTRACT_SALESWISE_SUM"
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

            For i As Integer = 0 To gridView.Rows.Count - 1
                Select Case gridView.Rows(i).Cells("RESULT").Value
                    Case "-1"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.Lavender
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkViolet
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "0"
                        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.LightBlue
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "3"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "4"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkGreen
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "7"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkViolet
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "8"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.DarkViolet
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            Next

            With gridView
                .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CHIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ADVANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AVG").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CRBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CRCARD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("COMM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("COLHEAD").Visible = False
                .Columns("CATNAME").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("METALID").Visible = False
                .Columns("CATNAME").Width = 170
                .Columns("CATNAME").HeaderText = "PARTICULARS"
                .Columns("TAX").HeaderText = "VAT"
                .Columns("CRBALANCE").HeaderText = "CREDIT"
                .Columns("CRCARD").HeaderText = "C.CARD"
                .Columns("AVG").HeaderText = "AVERAGE"
                .Columns("WEIGHT").HeaderText = "GRSWT"
            End With
            If Not ResizeToolStripMenuItem.Checked Then ResizeToolStripMenuItem_Click(Me, New EventArgs)
            'GridViewHeaderStyle()
            lblTitle.Text = "SALES WISE SUMMARY REPORT FROM " & dtpFrom.Value.ToString("dd/MM/yyyy") & " TO " & dtpTo.Value.ToString("dd/MM/yyyy") & ""
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
            .Columns.Add("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT", GetType(String))
            .Columns.Add("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT", GetType(String))
            .Columns.Add("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL", GetType(String))
            .Columns.Add("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Caption = "SALES"
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Caption = "PURCHASE"
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Caption = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Caption = "PAYMENT"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").HeaderText = "SALES"
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").HeaderText = "PURCHASE"
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").HeaderText = "SALESRETURN"
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").HeaderText = "PAYMENT"

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

            .Columns("PARTICULAR").Width = gridView.Columns("BILLNO").Width
            .Columns("PARTICULAR").Width += gridView.Columns("BILLDATE").Width
            'If Chkwithcustomer.Checked Then .Columns("PARTICULAR").Width += gridView.Columns("CUSTOMER").Width

            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width = gridView.Columns("ITEMNAME").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IWEIGHT").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("ISTNWT").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IDIAWT").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IRATE").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IMCHARGE").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IWASTVAL").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("IWASTAGE").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("ITAX").Width
            .Columns("IITEM~IWT~ISTNWT~IDIAWT~IRATE~IMC~IWAST~ITAX~ITOT").Width += gridView.Columns("ITOTAL").Width

            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width = gridView.Columns("PBILLNO").Width
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width += gridView.Columns("PITEMNAME").Width
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width += gridView.Columns("PWEIGHT").Width
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width += gridView.Columns("PSTNWT").Width
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width += gridView.Columns("PDIAWT").Width
            .Columns("PBILLNO~PITEM~PWT~PSTNWT~PDIAWT~PAMT").Width += gridView.Columns("PAMOUNT").Width

            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width = gridView.Columns("SRBILLNO").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRITEMNAME").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRWEIGHT").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRSTNWT").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRDIAWT").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRRATE").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRMCHARGE").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRWASTVAL").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRWASTAGE").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRTAX").Width
            .Columns("SRBILLNO~SRITEM~SRWT~SRSTNWT~SRDIAWT~SRRATE~SRMC~SRWAST~SRTAX~SRTOT~SRBAL").Width += gridView.Columns("SRTOTAL").Width

            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width = gridView.Columns("CASH").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("CRCARD").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("CHIT").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("CHEQUE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("ADVANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("DISCOUNT").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("CRBALANCE").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("ACTOTAL").Width
            .Columns("CASH~CRCARD~CHIT~CHEQUE~ADVANCE~DIS~CRBAL~ACTOT~COUNTRENAME").Width += gridView.Columns("COUNTERNAME").Width

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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text.ToString, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub
End Class
