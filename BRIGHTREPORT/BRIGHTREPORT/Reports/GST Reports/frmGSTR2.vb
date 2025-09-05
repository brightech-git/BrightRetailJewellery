Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmGSTR2
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet

    Dim dtCostCentre As New DataTable

    Private Sub SalesAbs()
        Try
            Prop_Sets()
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Me.Refresh()

            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR2"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@STATE = '" & CompanyState & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If Chk5.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_5 ORDER BY TRANDATE,TRANNO"
            ElseIf Chk6.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_6 ORDER BY TRANDATE,TRANNO"
            ElseIf Chk7.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2 ORDER BY TRANDATE,TRANNO,TRANTYPE"
            ElseIf Chk8.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_8 "
            ElseIf Chk10.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_10 ORDER BY " + IIf(rbtDateWise.Checked, "TRANDATE", "BILLDATE") + ",TRANNO"
            ElseIf Chk12.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_12 "
            ElseIf Chk13.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_13 "
            ElseIf Chk14.Checked Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT2_14 "
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT1 ORDER BY " + IIf(rbtDateWise.Checked, "TRANDATE", "BILLDATE") + ",TRANNO"
            End If

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)
            FillGridGroupStyle_KeyNoWise(gridView)
            With gridView
                If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
                If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
                If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
                If .Columns.Contains("NAMEOFSUPPLIER") Then .Columns("NAMEOFSUPPLIER").HeaderText = ""
                If .Columns.Contains("NATURE") Then .Columns("NATURE").HeaderText = "Goods/Services"
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "No"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Date"
                If .Columns.Contains("OINVNO") Then .Columns("OINVNO").HeaderText = "No"
                If .Columns.Contains("OINVDATE") Then .Columns("OINVDATE").HeaderText = "Date"
                If .Columns.Contains("VALUE") Then .Columns("VALUE").HeaderText = "Value"
                If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC"
                If .Columns.Contains("TAXABLE") Then .Columns("TAXABLE").HeaderText = "Taxable Value"
                If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                If .Columns.Contains("ELIGIBLE") Then .Columns("ELIGIBLE").HeaderText = ""
                If .Columns.Contains("STATE") Then .Columns("STATE").HeaderText = ""
                If .Columns.Contains("DIFFERENTIAL") Then .Columns("DIFFERENTIAL").HeaderText = ""
                If .Columns.Contains("NAMEOFSUPPLIER") Then .Columns("NAMEOFSUPPLIER").HeaderText = ""
                If .Columns.Contains("TYPE") Then .Columns("TYPE").HeaderText = ""
                If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = ""
                If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").HeaderText = "IGST"
                If .Columns.Contains("TOTCGST") Then .Columns("TOTCGST").HeaderText = "CGST"
                If .Columns.Contains("TOTSGST") Then .Columns("TOTSGST").HeaderText = "SGST"
                If .Columns.Contains("ITCIGST") Then .Columns("ITCIGST").HeaderText = "IGST"
                If .Columns.Contains("ITCCGST") Then .Columns("ITCCGST").HeaderText = "CGST"
                If .Columns.Contains("ITCSGST") Then .Columns("ITCSGST").HeaderText = "SGST"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                If .Columns.Contains("BILLDATE") Then .Columns("BILLDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                If .Columns.Contains("STATE") Then .Columns("STATE").Width = 150
                If .Columns.Contains("ELIGIBLE") Then .Columns("ELIGIBLE").Width = 150
                If .Columns.Contains("DIFFERENTIAL") Then .Columns("DIFFERENTIAL").Width = 100
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Width = 85
            End With
            pnlGridHead.Visible = True
            With gridView
                If Chk5.Checked Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Value"
                    If .Columns.Contains("TAXABLE") Then .Columns("AMOUNT").HeaderText = "Taxable Value"
                    If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").HeaderText = ""
                    If .Columns.Contains("TOTITC") Then .Columns("TOTITC").HeaderText = ""
                    If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").Width = 150
                    If .Columns.Contains("TOTITC") Then .Columns("TOTITC").Width = 150
                ElseIf Chk6.Checked Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Value"
                    If .Columns.Contains("TAXABLE") Then .Columns("AMOUNT").HeaderText = "Taxable Value"
                    If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").HeaderText = "Total ITC Admissible as input services/none"
                    If .Columns.Contains("TOTITC") Then .Columns("TOTITC").HeaderText = "ITC admissible this month"
                    If .Columns.Contains("TOTIGST") Then .Columns("TOTIGST").Width = 150
                    If .Columns.Contains("TOTITC") Then .Columns("TOTITC").Width = 150
                ElseIf Chk8.Checked Then
                    If .Columns.Contains("TYPE") Then .Columns("TYPE").Width = 150
                    If .Columns.Contains("TYPE") Then .Columns("TYPE").HeaderText = "Description"
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN Code/SAC code"
                    If .Columns.Contains("UNAMOUNT") Then .Columns("UNAMOUNT").HeaderText = "Unregistered Taxable person not included in Table 4 above"
                    If .Columns.Contains("EXEMAMOUNT") Then .Columns("EXEMAMOUNT").HeaderText = "Any exempt supply not included in Table 4 above"
                    If .Columns.Contains("NILAMOUNT") Then .Columns("NILAMOUNT").HeaderText = "Any nil rated supply not included in Table 4 above"
                    If .Columns.Contains("NONGSTAMOUNT") Then .Columns("NONGSTAMOUNT").HeaderText = "Non GST Supply"
                    pnlGridHead.Visible = False
                ElseIf Chk10.Checked Then
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = ""
                    If .Columns.Contains("BILLDATE") Then .Columns("BILLDATE").HeaderText = ""
                    If .Columns.Contains("TDSVALUE") Then .Columns("TDSVALUE").HeaderText = ""
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Value"
                    If .Columns.Contains("TDSVALUE") Then .Columns("TDSVALUE").Width = 125
                    If .Columns.Contains("BILLDATE") Then .Columns("BILLDATE").Width = 125
                ElseIf Chk12.Checked Then
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").HeaderText = "GSTIN/UIN/Name of customer"
                    If .Columns.Contains("STATECODE") Then .Columns("STATECODE").HeaderText = "StateCode"
                    If .Columns.Contains("NO") Then .Columns("NO").HeaderText = "Document No."
                    If .Columns.Contains("DATE") Then .Columns("DATE").HeaderText = "Date Goods/Services"
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "HSN/SAC of supply"
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Amount of advance received/ Value of Supply provided without raising a bill"
                    If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").HeaderText = "Amount"
                    If .Columns.Contains("GSTIN") Then .Columns("GSTIN").Width = 125
                    If .Columns.Contains("STATECODE") Then .Columns("STATECODE").Width = 125
                    If .Columns.Contains("NO") Then .Columns("NO").Width = 125
                    If .Columns.Contains("DATE") Then .Columns("DATE").Width = 125
                ElseIf Chk13.Checked Then
                    If .Columns.Contains("IGRATE") Then .Columns("IGRATE").HeaderText = "Rate"
                    If .Columns.Contains("IGTAX") Then .Columns("IGTAX").HeaderText = "Amount"
                    If .Columns.Contains("CGRATE") Then .Columns("CGRATE").HeaderText = "Rate"
                    If .Columns.Contains("CGTAX") Then .Columns("CGTAX").HeaderText = "Amount"
                    If .Columns.Contains("SGRATE") Then .Columns("SGRATE").HeaderText = "Rate"
                    If .Columns.Contains("SGTAX") Then .Columns("SGTAX").HeaderText = "Amount"
                    If .Columns.Contains("RUNNO") Then .Columns("RUNNO").HeaderText = "Transaction id (A number assigned by the system when tax was paid)"
                    If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = ""
                    If .Columns.Contains("RUNNO") Then .Columns("RUNNO").Width = 100
                ElseIf Chk14.Checked Then
                    If .Columns.Contains("NO") Then .Columns("NO").HeaderText = ""
                    If .Columns.Contains("DESCRYPT") Then .Columns("DESCRYPT").HeaderText = ""
                    If .Columns.Contains("NO") Then .Columns("NO").Width = 100
                    If .Columns.Contains("DESCRYPT") Then .Columns("DESCRYPT").Width = 150
                End If
            End With
            funcHeaderNew()
            FormatGridColumns(gridViewHead, False, False, True, False)
            Dim TITLE As String = ""

            TITLE += " GST REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub
    Public Function funcHeaderNew() As Integer
        If Chk8.Checked Then Exit Function
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                If Chk5.Checked Then
                    .Columns.Add("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("ELIGIBLE", GetType(String))
                    .Columns.Add("TOTIGST", GetType(String))
                    .Columns.Add("TOTITC", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk6.Checked Then
                    .Columns.Add("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("TOTAL", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk10.Checked Then
                    .Columns.Add("GSTIN", GetType(String))
                    .Columns.Add("TRANNO~TRANDATE~AMOUNT", GetType(String))
                    .Columns.Add("BILLDATE", GetType(String))
                    .Columns.Add("TDSVALUE", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk7.Checked Then
                    .Columns.Add("GSTIN", GetType(String))
                    .Columns.Add("TYPE", GetType(String))
                    .Columns.Add("TRANNO~TRANDATE", GetType(String))
                    .Columns.Add("OINVNO~OINVDATE", GetType(String))
                    .Columns.Add("DIFFERENTIAL", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("ELIGIBLE", GetType(String))
                    .Columns.Add("TOTIGST~TOTCGST~TOTSGST", GetType(String))
                    .Columns.Add("ITCIGST~ITCCGST~ITCSGST", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                    .Columns("GSTIN").Caption = "GSTIN"
                    .Columns("TYPE").Caption = "Type of note (Debit / Credit)"
                    .Columns("TRANNO~TRANDATE").Caption = "Debit Note/Credit Note"
                    .Columns("OINVNO~OINVDATE").Caption = "Original Invoice"
                    .Columns("DIFFERENTIAL").Caption = "Differential Value(Plus or Minus)"
                    .Columns("CGSTRATE~CGSTTAX").Caption = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").Caption = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").Caption = "SGST"
                    .Columns("ELIGIBLE").Caption = "Eligibility of ITC "
                    .Columns("TOTIGST~TOTCGST~TOTSGST").Caption = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").Caption = "ITC available this month $"
                ElseIf Chk12.Checked Then
                    .Columns.Add("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                ElseIf Chk13.Checked Then
                    .Columns.Add("TRANNO~RUNNO", GetType(String))
                    .Columns.Add("IGRATE~IGTAX", GetType(String))
                    .Columns.Add("CGRATE~CGTAX", GetType(String))
                    .Columns.Add("SGRATE~SGTAX", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                    .Columns("TRANNO~RUNNO").Caption = "Invoice"
                    .Columns("IGRATE~IGTAX").Caption = "IGST"
                    .Columns("CGRATE~CGTAX").Caption = "CGST"
                    .Columns("SGRATE~SGTAX").Caption = "SGST"
                ElseIf Chk14.Checked Then
                    .Columns.Add("NO", GetType(String))
                    .Columns.Add("DESCRYPT", GetType(String))
                    .Columns.Add("ITCIGST~ITCCGST~ITCSGST", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                Else
                    .Columns.Add("NAMEOFSUPPLIER~GSTNO", GetType(String))
                    .Columns.Add("TRANNO~TRANDATE~BILLNO~BILLDATE~VALUE~NATURE~HSN~TAXABLE", GetType(String))
                    .Columns.Add("IGSTRATE~IGSTTAX", GetType(String))
                    .Columns.Add("CGSTRATE~CGSTTAX", GetType(String))
                    .Columns.Add("SGSTRATE~SGSTTAX", GetType(String))
                    .Columns.Add("STATE", GetType(String))
                    .Columns.Add("ELIGIBLE", GetType(String))
                    .Columns.Add("TOTIGST~TOTCGST~TOTSGST", GetType(String))
                    .Columns.Add("ITCIGST~ITCCGST~ITCSGST", GetType(String))
                    .Columns.Add("SCROLL", GetType(String))
                    .Columns("NAMEOFSUPPLIER~GSTNO").Caption = "GSTIN/Name of Registered supplier"
                    .Columns("TRANNO~TRANDATE~BILLNO~BILLDATE~VALUE~NATURE~HSN~TAXABLE").Caption = "Invoice"
                    .Columns("IGSTRATE~IGSTTAX").Caption = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").Caption = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").Caption = "SGST"
                    .Columns("STATE").Caption = "POS(only if different from the location of recipient)"
                    .Columns("ELIGIBLE").Caption = "Eligibility of ITC as inputs/capital goods/ input services/none"
                    .Columns("TOTIGST~TOTCGST~TOTSGST").Caption = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").Caption = "ITC available this month $"
                End If
            End With
            With gridViewHead
                .DataSource = dtMergeHeader
                If Chk5.Checked Then
                    pnlGridHead.Height = 26
                    .Columns("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE").HeaderText = "Bill of entry/ Import report"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("ELIGIBLE").HeaderText = "Eligibility of ITC as inputs/capital "
                    .Columns("TOTIGST").HeaderText = "Total IGST available as ITC "
                    .Columns("TOTITC").HeaderText = "ITC available this month $"
                ElseIf Chk6.Checked Then
                    pnlGridHead.Height = 18
                    .Columns("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE").HeaderText = "Invoice"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("TOTAL").HeaderText = "ITC Admissibility"
                ElseIf Chk10.Checked Then
                    pnlGridHead.Height = 44
                    .Columns("GSTIN").HeaderText = "GSTIN of Deductor"
                    .Columns("TRANNO~TRANDATE~AMOUNT").HeaderText = "Invoice/Document"
                    .Columns("BILLDATE").HeaderText = "Date of Payment made to the deductee"
                    .Columns("TDSVALUE").HeaderText = "Value on which TDS has been deducted"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "TDS_IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "TDS_CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "TDS_SGST"
                ElseIf Chk7.Checked Then
                    'gridViewHead.ColumnHeadersHeight = 10
                    pnlGridHead.Height = 44
                    .Columns("GSTIN").HeaderText = "GSTIN"
                    .Columns("TYPE").HeaderText = "Type of note (Debit/Credit) "
                    .Columns("TRANNO~TRANDATE").HeaderText = "Debit Note/Credit Note"
                    .Columns("OINVNO~OINVDATE").HeaderText = "Original Invoice"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                    .Columns("DIFFERENTIAL").HeaderText = "Differential Value (Plus or Minus)"
                    .Columns("ELIGIBLE").HeaderText = "Eligibility of ITC"
                    .Columns("TOTIGST~TOTCGST~TOTSGST").HeaderText = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").HeaderText = "ITC available this month $"
                ElseIf Chk8.Checked Then
                    pnlGridHead.Visible = False
                ElseIf Chk12.Checked Then
                    pnlGridHead.Height = 18
                    .Columns("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT").HeaderText = " "
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                ElseIf Chk13.Checked Then
                    pnlGridHead.Height = 18
                    .Columns("TRANNO~RUNNO").HeaderText = "Invoice No."
                    .Columns("IGRATE~IGTAX").HeaderText = "IGST"
                    .Columns("CGRATE~CGTAX").HeaderText = "CGST"
                    .Columns("SGRATE~SGTAX").HeaderText = "SGST"
                ElseIf Chk14.Checked Then
                    pnlGridHead.Height = 18
                    .Columns("NO").HeaderText = "S.No"
                    .Columns("DESCRYPT").HeaderText = "Description"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").HeaderText = "ITC Reversal"
                Else
                    pnlGridHead.Height = 53
                    .Columns("NAMEOFSUPPLIER~GSTNO").HeaderText = "GSTIN/Name of Registered supplier"
                    .Columns("TRANNO~TRANDATE~BILLNO~BILLDATE~VALUE~NATURE~HSN~TAXABLE").HeaderText = "Invoice"
                    .Columns("IGSTRATE~IGSTTAX").HeaderText = "IGST"
                    .Columns("CGSTRATE~CGSTTAX").HeaderText = "CGST"
                    .Columns("SGSTRATE~SGSTTAX").HeaderText = "SGST"
                    .Columns("STATE").HeaderText = "POS(only if different from the location of recipient)"
                    .Columns("ELIGIBLE").HeaderText = "Eligibility of ITC as inputs/capital goods/ input services/none"
                    .Columns("TOTIGST~TOTCGST~TOTSGST").HeaderText = "Total Tax available as ITC"
                    .Columns("ITCIGST~ITCCGST~ITCSGST").HeaderText = "ITC available this month $"
                End If
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                funcColWidth()
                gridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                .Columns("SCROLL").Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Function funcColWidth() As Integer
        With gridViewHead
            If Chk5.Checked Then
                .Columns("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE").Width = gridView.Columns("TRANNO").Width + gridView.Columns("TRANDATE").Width _
                            + gridView.Columns("AMOUNT").Width + gridView.Columns("HSN").Width + gridView.Columns("TAXABLE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("ELIGIBLE").Width = gridView.Columns("ELIGIBLE").Width
                .Columns("TOTIGST").Width = gridView.Columns("TOTIGST").Width
                .Columns("TOTITC").Width = gridView.Columns("TOTITC").Width
            ElseIf Chk6.Checked Then
                .Columns("TRANNO~TRANDATE~AMOUNT~HSN~TAXABLE").Width = gridView.Columns("TRANNO").Width + gridView.Columns("TRANDATE").Width _
                            + gridView.Columns("AMOUNT").Width + gridView.Columns("HSN").Width + gridView.Columns("TAXABLE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("TOTAL").Width = gridView.Columns("TOTIGST").Width + gridView.Columns("TOTITC").Width
            ElseIf Chk10.Checked Then
                .Columns("GSTIN").Width = gridView.Columns("GSTIN").Width
                .Columns("TRANNO~TRANDATE~AMOUNT").Width = gridView.Columns("TRANNO").Width + gridView.Columns("TRANDATE").Width + gridView.Columns("AMOUNT").Width
                .Columns("BILLDATE").Width = gridView.Columns("BILLDATE").Width
                .Columns("TDSVALUE").Width = gridView.Columns("TDSVALUE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
            ElseIf Chk7.Checked Then
                .Columns("GSTIN").Width = gridView.Columns("GSTIN").Width
                .Columns("TYPE").Width = gridView.Columns("TYPE").Width
                .Columns("TRANNO~TRANDATE").Width = gridView.Columns("TRANNO").Width + gridView.Columns("TRANDATE").Width
                .Columns("OINVNO~OINVDATE").Width = gridView.Columns("OINVNO").Width + gridView.Columns("OINVDATE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
                .Columns("ELIGIBLE").Width = gridView.Columns("ELIGIBLE").Width
                .Columns("DIFFERENTIAL").Width = gridView.Columns("DIFFERENTIAL").Width
                .Columns("TOTIGST~TOTCGST~TOTSGST").Width = gridView.Columns("TOTIGST").Width _
                    + gridView.Columns("TOTCGST").Width + gridView.Columns("TOTSGST").Width
                .Columns("ITCIGST~ITCCGST~ITCSGST").Width = gridView.Columns("ITCIGST").Width _
                    + gridView.Columns("ITCCGST").Width + gridView.Columns("ITCSGST").Width
            ElseIf Chk12.Checked Then
                .Columns("GSTIN~STATECODE~NO~DATE~HSN~AMOUNT").Width = gridView.Columns("GSTIN").Width _
                    + gridView.Columns("STATECODE").Width + gridView.Columns("NO").Width + gridView.Columns("DATE").Width _
                    + gridView.Columns("HSN").Width + gridView.Columns("AMOUNT").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
            ElseIf Chk13.Checked Then
                .Columns("TRANNO~RUNNO").Width = gridView.Columns("TRANNO").Width + gridView.Columns("RUNNO").Width
                .Columns("IGRATE~IGTAX").Width = gridView.Columns("IGRATE").Width + gridView.Columns("IGTAX").Width
                .Columns("CGRATE~CGTAX").Width = gridView.Columns("CGRATE").Width + gridView.Columns("CGTAX").Width
                .Columns("SGRATE~SGTAX").Width = gridView.Columns("SGRATE").Width + gridView.Columns("SGTAX").Width
            ElseIf Chk14.Checked Then
                .Columns("NO").Width = gridView.Columns("NO").Width
                .Columns("DESCRYPT").Width = gridView.Columns("DESCRYPT").Width
                .Columns("ITCIGST~ITCCGST~ITCSGST").Width = gridView.Columns("ITCIGST").Width + gridView.Columns("ITCCGST").Width + gridView.Columns("ITCSGST").Width
            ElseIf Chk8.Checked Then

            Else
                .Columns("NAMEOFSUPPLIER~GSTNO").Width = gridView.Columns("NAMEOFSUPPLIER").Width + gridView.Columns("GSTNO").Width
                .Columns("TRANNO~TRANDATE~BILLNO~BILLDATE~VALUE~NATURE~HSN~TAXABLE").Width = gridView.Columns("TRANNO").Width _
                    + gridView.Columns("TRANDATE").Width + gridView.Columns("VALUE").Width _
                    + gridView.Columns("BILLNO").Width + gridView.Columns("BILLDATE").Width _
                    + gridView.Columns("NATURE").Width + gridView.Columns("HSN").Width + gridView.Columns("TAXABLE").Width
                .Columns("IGSTRATE~IGSTTAX").Width = gridView.Columns("IGSTRATE").Width + gridView.Columns("IGSTTAX").Width
                .Columns("CGSTRATE~CGSTTAX").Width = gridView.Columns("CGSTRATE").Width + gridView.Columns("CGSTTAX").Width
                .Columns("SGSTRATE~SGSTTAX").Width = gridView.Columns("SGSTRATE").Width + gridView.Columns("SGSTTAX").Width
                .Columns("STATE").Width = gridView.Columns("STATE").Width
                .Columns("ELIGIBLE").Width = gridView.Columns("ELIGIBLE").Width
                .Columns("TOTIGST~TOTCGST~TOTSGST").Width = gridView.Columns("TOTIGST").Width _
                    + gridView.Columns("TOTCGST").Width + gridView.Columns("TOTSGST").Width
                .Columns("ITCIGST~ITCCGST~ITCSGST").Width = gridView.Columns("ITCIGST").Width _
                    + gridView.Columns("ITCCGST").Width + gridView.Columns("ITCSGST").Width
            End If
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        SalesAbs()
        Exit Sub
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' "
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
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
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmGSTR2_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR2_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR2_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR2_Properties))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
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
        End If
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
                gridViewHead.Columns("SCROLL").Visible = CType(gridViewHead.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridViewHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub Chk5_CheckedChanged(sender As Object, e As EventArgs) Handles _
        Chk5.CheckedChanged, Chk6.CheckedChanged, Chk7.CheckedChanged, Chk8.CheckedChanged, Chk9.CheckedChanged _
        , Chk10.CheckedChanged, Chk11.CheckedChanged, Chk12.CheckedChanged, Chk13.CheckedChanged, Chk14.CheckedChanged _
        , rbtDateWise.CheckedChanged, rbtBillDateWise.CheckedChanged

        If Not Chk5.Checked And Not Chk6.Checked And Not Chk7.Checked And Not Chk8.Checked And Not Chk9.Checked _
             And Not Chk10.Checked And Not Chk11.Checked And Not Chk12.Checked And Not Chk13.Checked And Not Chk14.Checked Then
            Return
        ElseIf Chk10.Checked Then
            Return
        Else
            rbtDateWise.Checked = True
            rbtBillDateWise.Checked = False
        End If
    End Sub
End Class

Public Class frmGSTR2_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
End Class