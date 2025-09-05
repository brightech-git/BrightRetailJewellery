Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesRegister
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
    Dim STOCKVIEW_GRSWT_AS_DIAWT As Boolean = IIf(GetAdmindbSoftValue("GRSWT_AS_DIAWT", "N") = "Y", True, False)

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
            Dim selectPsno As String = IIf(cmbCustomer_OWN.Text <> "ALL", cmbCustomer_OWN.SelectedValue.ToString, "ALL")
            gridView.DataSource = Nothing
            Me.Refresh()
            strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESABSTRACT_REGISTER"
            strSql += vbCrLf + "@DBNAME='" & cnAdminDb & "'"
            strSql += vbCrLf + ",@FRMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@COSTID ='" & selectcostid & "'"
            strSql += vbCrLf + ",@COMPANYID ='" & selectcompid & "'"
            strSql += vbCrLf + ",@WITHCUSTOMER='" & IIf(Chkwithcustomer.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@UID='" & userId & "'"
            strSql += vbCrLf + ",@PSNO='" & IIf(selectPsno.ToString <> "" And selectPsno.ToString <> "0", selectPsno.ToString, "ALL") & "'"
            strSql += vbCrLf + ",@VIEWGRSWT_AS_DIAWT='" & IIf(STOCKVIEW_GRSWT_AS_DIAWT, "Y", "N") & "'"
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
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPADDRESS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPADDRESS"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALEABSTRACT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALEABSTRACT"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALEABSTRACT_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALEABSTRACT_RES  "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPPERSONALINFO') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPERSONALINFO"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPPERSONALINFO_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPERSONALINFO_RES"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            dtGrid = dsGridView.Tables(0).Copy()
            gridView.DataSource = dtGrid
            For i As Integer = 0 To gridView.Columns.Count - 1
                If i = 2 Then gridView.Columns(i).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold)
                If i = 14 Then gridView.Columns(i).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold)
                If i = 20 Then gridView.Columns(i).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold)
                If i = 30 Then gridView.Columns(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                If i = 39 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.MintCream
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.DarkMagenta
                    gridView.Columns(i).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold)
                End If
                If i <= 1 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.LightBlue
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.Black
                End If
                If i > 1 And i <= 12 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.Wheat
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.Black
                End If
                If i > 12 And i <= 18 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.LightYellow
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.Black
                End If
                If i > 18 And i <= 29 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.Khaki
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.Black
                End If
                If i > 29 And i <= 38 Then
                    gridView.Columns(i).DefaultCellStyle.BackColor = Color.Pink
                    gridView.Columns(i).DefaultCellStyle.ForeColor = Color.Black
                End If

            Next
            tabMain.SelectedTab = tabView
            For i As Integer = 0 To gridView.Rows.Count - 1
                Select Case gridView.Rows(i).Cells("COLHEAD").Value
                    Case "A"
                        gridView.Rows(i).Cells("CUSTOMER").Style.BackColor = Color.LightGreen
                        gridView.Rows(i).Cells("CUSTOMER").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("CUSTOMER").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.RoyalBlue
                    Case "T"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.White
                    Case "Z"
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                        gridView.Rows(i).Cells("BILLNO").Style.BackColor = Color.Red
                        gridView.Rows(i).Cells("BILLNO").Style.ForeColor = Color.White
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            Next

            With gridView
                .Columns("IWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IMCHARGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IWASTVAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ITAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ITOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRMCHARGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRWASTVAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CASH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CHIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CRCARD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CHEQUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ADVANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CRBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ACTOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SRDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GV").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("IWASTAGE").DefaultCellStyle.Format = "0.00"
                .Columns("SRWASTAGE").DefaultCellStyle.Format = "0.00"

                .Columns("IWASTVAL").DefaultCellStyle.Format = "0.000"
                .Columns("SRWASTVAL").DefaultCellStyle.Format = "0.000"

          

                .Columns("BILLDATE").Frozen = True
                .Columns("BILLNO").Frozen = True
                .Columns("CUSTOMER").Frozen = True
                .Columns("BILLNO").Width = 90
                .Columns("BILLDATE").Width = 75
                .Columns("CUSTOMER").Width = 150

                .Columns("PBILLNO").Width = 60
                .Columns("SRBILLNO").Width = 60
                .Columns("IWEIGHT").Width = 60
                .Columns("IRATE").Width = 60
                .Columns("IMCHARGE").Width = 60
                .Columns("IWASTVAL").Width = 75
                .Columns("IWASTAGE").Width = 90
                .Columns("ITAX").Width = 75
                .Columns("ITOTAL").Width = 75
                .Columns("PWEIGHT").Width = 75
                .Columns("ISTNWT").Width = 75
                .Columns("IDIAWT").Width = 75
                .Columns("PSTNWT").Width = 75
                .Columns("PDIAWT").Width = 75
                .Columns("SRSTNWT").Width = 75
                .Columns("SRDIAWT").Width = 75
                .Columns("PAMOUNT").Width = 75
                .Columns("SRWEIGHT").Width = 75
                .Columns("SRRATE").Width = 75
                .Columns("SRMCHARGE").Width = 75
                .Columns("SRWASTVAL").Width = 75
                .Columns("SRWASTAGE").Width = 90
                .Columns("SRTAX").Width = 75
                .Columns("SRTOTAL").Width = 75
                .Columns("SRBALANCE").Width = 75
                .Columns("CASH").Width = 75
                .Columns("CHIT").Width = 75
                .Columns("CRCARD").Width = 75
                .Columns("CHEQUE").Width = 75
                .Columns("ADVANCE").Width = 75
                .Columns("DISCOUNT").Width = 75
                .Columns("CRBALANCE").Width = 75
                .Columns("ACTOTAL").Width = 75
                .Columns("ITAGNO").Width = 75
                .Columns("GV").Width = 75


                .Columns("GV").HeaderText = "GIFT.VOU"
                .Columns("PBILLNO").HeaderText = "BILLNO"
                .Columns("SRBILLNO").HeaderText = "BILLNO"
                .Columns("ITEMNAME").HeaderText = "NAME"
                .Columns("PITEMNAME").HeaderText = "NAME"
                .Columns("SRITEMNAME").HeaderText = "NAME"
                .Columns("IWEIGHT").HeaderText = "NETWT"
                .Columns("IRATE").HeaderText = "RATE"
                .Columns("CHIT").HeaderText = "SAVINGS"
                .Columns("IMCHARGE").HeaderText = "MCHARGE"
                .Columns("IWASTVAL").HeaderText = "WAST-WT"
                .Columns("IWASTAGE").HeaderText = "WAST-AMT"
                .Columns("ITAX").HeaderText = "TAX"
                .Columns("ITOTAL").HeaderText = "TOTAL"
                .Columns("PWEIGHT").HeaderText = "WEIGHT"
                .Columns("PAMOUNT").HeaderText = "AMOUNT"
                .Columns("SRWEIGHT").HeaderText = "WEIGHT"
                .Columns("SRRATE").HeaderText = "RATE"
                .Columns("SRMCHARGE").HeaderText = "MCHARGE"
                .Columns("SRWASTVAL").HeaderText = "WAST-WT"
                .Columns("SRWASTAGE").HeaderText = "WAST-AMT"
                .Columns("SRTAX").HeaderText = "TAX"
                .Columns("SRTOTAL").HeaderText = "TOTAL"
                .Columns("SRBALANCE").HeaderText = "BALANCE"
                .Columns("ISTNWT").HeaderText = "STNWT"
                .Columns("IDIAWT").HeaderText = "DIAWT"
                .Columns("PSTNWT").HeaderText = "STNWT"
                .Columns("PDIAWT").HeaderText = "DIAWT"
                .Columns("SRSTNWT").HeaderText = "STNWT"
                .Columns("SRDIAWT").HeaderText = "DIAWT"
                .Columns("CRCARD").HeaderText = "CR-CARD"

                .Columns("CRBALANCE").HeaderText = "CR-BALANCE"
                .Columns("ACTOTAL").HeaderText = "TOTAL"
                .Columns("COLHEAD").Visible = False
                .Columns("SRBALANCE").Visible = False
                .Columns("CUSTOMER").Visible = Chkwithcustomer.Checked


                .Columns("COLHEAD").Visible = False
                .Columns("SRBALANCE").Visible = False
                .Columns("CUSTOMER").Visible = Chkwithcustomer.Checked
            End With
            GridViewHeaderStyle()
            lblTitle.Text = "SALES REGISTER REPORT FROM " & dtpFrom.Value.ToString("dd/MM/yyyy") & " TO " & dtpTo.Value.ToString("dd/MM/yyyy") & ""
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COST CENTRE :" & chkCmbCostCentre.Text
            lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            btnView_Search.Enabled = True
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
            If Chkwithcustomer.Checked Then .Columns("PARTICULAR").Width += gridView.Columns("CUSTOMER").Width

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
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT CONVERT(VARCHAR(12),0) AS SNO,'ALL' PNAME "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT CONVERT(VARCHAR(12),SNO) AS SNO,PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN ( "
        strSql += vbCrLf + " SELECT DISTINCT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN ("
        strSql += vbCrLf + " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE 1=1 AND PAYMODE IN ('SA','PU','SR','OD','RD') "
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " )) ORDER BY PNAME"
        Dim dtCust As New DataTable
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtCust)
        cmbCustomer_OWN.DataSource = dtCust
        cmbCustomer_OWN.DisplayMember = "PNAME"
        cmbCustomer_OWN.ValueMember = "SNO"


        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbCustomer_OWN.Text = "ALL"
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

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

   
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            Dim title As String
            title = "DAILY ABSTRACT REPORT"
            title += vbCrLf + Trim(lblTitle.Text)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Print)
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

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        'If gridViewHead.ColumnCount > 0 Then
        '    If spbaserpt = True Then
        '        funcColWidth1()
        '    Else
        '        funcColWidth()
        '    End If
        'End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
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

    Private Sub cmbCustomer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCustomer_OWN.GotFocus
        Dim selectcostid As String = IIf(chkCmbCostCentre.Text <> "ALL", GetSelectedCostId(chkCmbCostCentre, False), "ALL")
        Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL", GetSelectedCompanyId(chkcmbCompany, False), "ALL")
        If selectcostid <> "" And selectcostid <> "ALL" Then
            selectcostid = "'" + selectcostid.ToString.Replace(",", "','") + "'"
        End If
        If selectcompid <> "" And selectcompid <> "ALL" Then
            selectcompid = "'" + selectcompid.ToString.Replace(",", "','") + "'"
        End If
        strSql = " SELECT CONVERT(VARCHAR(12),0) AS SNO,'ALL' PNAME "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT CONVERT(VARCHAR(12),SNO) AS SNO,PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN ( "
        strSql += vbCrLf + " SELECT DISTINCT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN ("
        strSql += vbCrLf + " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE 1=1 AND PAYMODE IN ('SA','PU','SR','OD','RD') "
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If selectcompid <> "" And selectcompid <> "ALL" Then strSql += vbCrLf + " AND COMPANYID IN (" & selectcompid & ") "
        If selectcostid <> "" And selectcostid <> "ALL" Then strSql += vbCrLf + " AND COSTID IN (" & selectcostid & ") "
        strSql += vbCrLf + " )) ORDER BY PNAME"
        Dim dtCust As New DataTable
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtCust)
        cmbCustomer_OWN.DataSource = dtCust
        cmbCustomer_OWN.DisplayMember = "PNAME"
        cmbCustomer_OWN.ValueMember = "SNO"
    End Sub
End Class
