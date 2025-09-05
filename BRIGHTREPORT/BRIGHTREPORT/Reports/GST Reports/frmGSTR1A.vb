Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Imports Microsoft.Office.Interop
Imports Microsoft.Office
Imports Newtonsoft.Json

Public Class frmGSTR1A
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim B2B_E_INVOICE As Boolean = IIf(GetAdmindbSoftValue("B2B_E_INVOICE", "N") = "Y", True, False)
    Dim GST_EINV_CLIENTID As String = GetAdmindbSoftValue("GST_EINV_CLIENTID", "",, True)
    Dim GST_EINV_USERNAME As String = GetAdmindbSoftValue("GST_EINV_USERNAME", "",, True)
    Dim GST_EINV_PASSWORD As String = GetAdmindbSoftValue("GST_EINV_PASSWORD", "",, True)
    Dim GST_EINV_CLIENTSECRET As String = GetAdmindbSoftValue("GST_EINV_CLIENTSECRET", "",, True)
    Dim GST_EINV_URL As String = GetAdmindbSoftValue("GST_EINV_URL", "",, True)
    Dim GST_EINV_PUBLICKEY As String = GetAdmindbSoftValue("GST_EINV_PUBLICKEY", "",, True)
    Dim GST_EINV_OFFLINE_JSON As Boolean = IIf(GetAdmindbSoftValue("GST_EINV_OFFLINE_JSON", "N") = "Y", True, False)
    Dim _COSTCENTREMAINTAIN As Boolean = IIf(GetAdmindbSoftValue("COSTCENTRE", "N") = "Y", True, False)
    Dim _GROUPBYCOSTCENTRE As Boolean = IIf(GetAdmindbSoftValue("GSTR1_B2B_GROUPBYCOSTCENTRE", "N") = "Y", True, False)
    Dim chkCount As Integer = 0
    Dim GST_EINV_MASTER_AUTOUPLOAD As Boolean = IIf(GetAdmindbSoftValue("GST_EINV_MASTER_AUTOUPLOAD", "N") = "Y", True, False)
    Dim GST_EINV_MASTER_EXPORT_GSTIN As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_GSTIN", "",, True)
    Dim GST_EINV_MASTER_EXPORT_STATECODE As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_STATECODE", "",, True)
    Dim GST_EINV_MASTER_EXPORT_PINCODE As String = GetAdmindbSoftValue("GST_EINV_MASTER_EXPORT_PINCODE", "",, True)
    Private Sub SalesAbs()
        Try
            lblFindHelp.Visible = True
            Prop_Sets()
            gridView.DataSource = Nothing
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Dim Opt As String = "B2CS"
            Dim _Metal As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text.ToString & "'")
            Dim RDSEPBILLCHECK As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID='GEN-GENSEPREPAIRBILLNO'")
            Me.Refresh()
            If rbtB2B.Checked Then Opt = "B2B"
            If rbtB2CL.Checked Then Opt = "B2CL"
            If rbtB2CS.Checked Then Opt = "B2CS"
            If rbtCDNR.Checked Then Opt = "CDNR"
            If rbtCDNRU.Checked Then Opt = "CDNRU"
            If rbtCDNRUL.Checked Then Opt = "CDNRUS"
            If rbtExp.Checked Then Opt = "EXP"
            If rbtAT.Checked Then Opt = "AT"
            If rbtATADJ.Checked Then Opt = "ATADJ"
            If rbtEXEM.Checked Then Opt = "EXEM"
            If rbtHSN.Checked Then Opt = "HSN"
            If rbtDocs.Checked Then Opt = "DOCS"
            If rbtChit.Checked Then Opt = "CC"
            If rbtSS.Checked Then Opt = "SS"
            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR1A"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@MONTH = '" & dtpFrom.Value.Month & "'"
            strSql += vbCrLf + " ,@YEAR = '" & dtpFrom.Value.Year & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@OPTION = '" & Opt & "'"
            strSql += vbCrLf + " ,@STATEID = '" & CompanyStateId & "'"
            strSql += vbCrLf + " ,@ADVGST = '" & IIf(ChkAdvGst.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHSR = '" & IIf(ChkSR.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@BILLPREFIXDB='" & IIf(ChkBillPrefic.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@GSTDATE = '" & GstDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & _Metal & "'"
            If chkCmbCostCentre.Text = "ALL" And rbtB2B.Checked And _COSTCENTREMAINTAIN And _GROUPBYCOSTCENTRE Then
                strSql += vbCrLf + " ,@GROUPBYCOSTCENTRE = '" & IIf(chkGroupByCostcentre.Checked, "Y", "N") & "'"
            Else
                strSql += vbCrLf + " ,@GROUPBYCOSTCENTRE = 'N'"
            End If
            strSql += vbCrLf + " ,@RDSEPBILLCHECK = '" & RDSEPBILLCHECK & "'"
            strSql += vbCrLf + " ,@DATECHECK = '" & IIf(chkdate.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@DATEFROM = '" & dtpbFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@DATETO = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@B2BPRJB='" & IIf(chkb2bprjb.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@HSNDISC='" & IIf(chkhsndesc.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            Dim dtGrid As New DataTable
            Dim ds As New DataSet
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds)
            If ds.Tables.Count = 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                lblFindHelp.Visible = False
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid = ds.Tables(0)
            If dtGrid.Rows.Count = 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                lblFindHelp.Visible = False
                dtpFrom.Select()
                Exit Sub
            End If

            If rbtHSN.Checked Then
                dtGrid.Columns.Remove("KEYNO")
                dtGrid.Columns.Add("KEYNO")
                For cnt As Integer = 0 To dtGrid.Rows.Count - 1
                    dtGrid.Rows(cnt).Item("KEYNO") = cnt
                Next
            End If

            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)
            If rbtB2B.Checked Then
                FillGridGroupStyle_KeyNoWise(gridView, "GSTNO")
            ElseIf rbtB2CL.Checked Then
                FillGridGroupStyle_KeyNoWise(gridView, "TRANNO")
            ElseIf rbtAT.Checked Or rbtATADJ.Checked Then
                FillGridGroupStyle_KeyNoWise(gridView, "STATE")
            ElseIf rbtHSN.Checked Or rbtDocs.Checked Then
                FillGridGroupStyle_KeyNoWise(gridView, "HSN")
            Else
                FillGridGroupStyle_KeyNoWise(gridView, "GSTNO")
            End If
            With gridView
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                If .Columns.Contains("BILLTYPE") Then .Columns("BILLTYPE").Visible = False
                If .Columns.Contains("COSTID") Then .Columns("COSTID").Visible = False
                If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
                If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
                If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("REFDATE") Then .Columns("REFDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("GSTNO") Then .Columns("GSTNO").HeaderText = "GSTIN/UIN of Recipient"
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Invoice Number"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Invoice Date"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Invoice Value"
                If .Columns.Contains("STATE") Then .Columns("STATE").HeaderText = "Place of Supply"
                If .Columns.Contains("RCM") Then .Columns("RCM").HeaderText = "Reverse Charge"
                If .Columns.Contains("INVTYPE") Then .Columns("INVTYPE").HeaderText = "Invoice Type"
                If .Columns.Contains("ECOMGSTNO") Then .Columns("ECOMGSTNO").HeaderText = "E-Commerce GSTIN"
                If .Columns.Contains("RATE") Then .Columns("RATE").HeaderText = "Rate"
                If .Columns.Contains("TAX") Then .Columns("TAX").HeaderText = "Taxable Value"
                If .Columns.Contains("CESS") Then .Columns("CESS").HeaderText = "Cess Amount"
                If .Columns.Contains("RCM") Then .Columns("RCM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("REFNO") Then .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MMM-yyyy"
                If .Columns.Contains("REFDATE") Then .Columns("REFDATE").DefaultCellStyle.Format = "dd-MMM-yyyy"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TAX") Then .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CESS") Then .Columns("CESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGST") Then .Columns("IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGST") Then .Columns("CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGST") Then .Columns("SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("QTY") Then .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SNOFROM") Then .Columns("SNOFROM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SNOTO") Then .Columns("SNOTO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TOTAL") Then .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CANCEL") Then .Columns("CANCEL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If rbtB2CL.Checked Then
                    If .Columns.Contains("GSTNO") Then .Columns("GSTNO").Visible = False
                    If .Columns.Contains("INVTYPE") Then .Columns("INVTYPE").Visible = False
                    If .Columns.Contains("RCM") Then .Columns("RCM").Visible = False
                    If .Columns.Contains("ECOMGSTNO") Then .Columns("ECOMGSTNO").Visible = False
                ElseIf rbtB2CS.Checked Then
                    If .Columns.Contains("GSTNO") Then .Columns("GSTNO").HeaderText = "Type"
                ElseIf rbtCDNR.Checked Or rbtCDNRU.Checked Or rbtCDNRUL.Checked Then
                    If rbtCDNRU.Checked Or rbtCDNRUL.Checked Then
                        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Note/Refund Voucher Number"
                        If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Note/Refund Voucher date"
                        If .Columns.Contains("REFNO") Then .Columns("REFNO").HeaderText = "Invoice/Advance Receipt Number"
                        If .Columns.Contains("REFDATE") Then .Columns("REFDATE").HeaderText = "Invoice/Advance Receipt Date"
                    Else
                        If .Columns.Contains("REFNO") Then .Columns("REFNO").HeaderText = "Note/Refund Voucher Number"
                        If .Columns.Contains("REFDATE") Then .Columns("REFDATE").HeaderText = "Note/Refund Voucher date"
                        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Invoice/Advance Receipt Number"
                        If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Invoice/Advance Receipt Date"
                    End If
                    If .Columns.Contains("DOCTYPE") Then .Columns("DOCTYPE").HeaderText = "Document Type"
                    If .Columns.Contains("REASON") Then .Columns("REASON").HeaderText = "Reason For Issuing document"
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Note/Refund Voucher Value"
                    If .Columns.Contains("DOCTYPE") Then .Columns("DOCTYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    If .Columns.Contains("PREGST") Then .Columns("PREGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                ElseIf rbtAT.Checked Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Gross Advance Received"
                ElseIf rbtATADJ.Checked Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Gross Advance Adjusted"
                ElseIf rbtHSN.Checked Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Total Value"
                    If .Columns.Contains("TAX") Then .Columns("TAX").HeaderText = "Taxable Value"
                    If .Columns.Contains("IGST") Then .Columns("IGST").HeaderText = "Integrated Tax Amount"
                    If .Columns.Contains("CGST") Then .Columns("CGST").HeaderText = "Central Tax Amount"
                    If .Columns.Contains("SGST") Then .Columns("SGST").HeaderText = "State/UT Tax Amount"
                    If .Columns.Contains("DESCRIPTION") Then .Columns("DESCRIPTION").HeaderText = "Description"
                    If .Columns.Contains("QTY") Then .Columns("QTY").HeaderText = "Total Quantity"
                ElseIf rbtDocs.Checked Then
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "Nature  of Document"
                    If .Columns.Contains("SNOFROM") Then .Columns("SNOFROM").HeaderText = "Sr. No. From"
                    If .Columns.Contains("SNOTO") Then .Columns("SNOTO").HeaderText = "Sr. No. To"
                    If .Columns.Contains("TOTAL") Then .Columns("TOTAL").HeaderText = "Total Number"
                    If .Columns.Contains("CANCEL") Then .Columns("CANCEL").HeaderText = "Cancelled"
                End If
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            If rbtHSN.Checked Then
                If gridView.Columns.Contains("RATE") Then gridView.Columns("RATE").Visible = chkHsnRate.Checked
            End If
            Dim TITLE As String = ""
            If rbtB2B.Checked Then
                TITLE += " B2B"
            ElseIf rbtB2CL.Checked Then
                TITLE += " B2C LARGE"
            ElseIf rbtB2CS.Checked Then
                TITLE += " B2C SMALL"
            ElseIf rbtCDNR.Checked Then
                TITLE += " CREDIT NOTE [REGISTERED]"
            ElseIf rbtCDNRU.Checked Then
                TITLE += " CREDIT NOTE [UNREGISTERED]"
            ElseIf rbtCDNRUL.Checked Then
                TITLE += " CREDIT NOTE [UNREGISTERED SMALL]"
            ElseIf rbtExp.Checked Then
                TITLE += " EXPORTS"
            ElseIf rbtAT.Checked Then
                TITLE += " ADVANCE RECEIVED"
            ElseIf rbtATADJ.Checked Then
                TITLE += " ADVANCE ADJUSTED"
            ElseIf rbtChit.Checked Then
                TITLE += " SCHEME COLLECTION"
            ElseIf rbtSS.Checked Then
                TITLE += " SCHEME ADJUSTED"
            ElseIf rbtEXEM.Checked Then
                TITLE += " EXEMPTED"
            ElseIf rbtHSN.Checked Then
                TITLE += " HSN"
            End If
            TITLE += " GST REPORT FOR TAX PERIOD " & dtpFrom.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            If rbtCDNR.Checked Or rbtCDNRU.Checked Or rbtCDNRUL.Checked Then
                TITLE += " [EXCLUDE VAT BILLS]"
            End If
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub

    Private Sub SalesAbsAll()

        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Int16, j As Int16
        xlApp = New Excel.ApplicationClass
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = CType(xlWorkBook.Sheets.Add(Count:=14), Excel.Worksheet)
        'Try
        Prop_Sets()
        gridView.DataSource = Nothing
        Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim RDSEPBILLCHECK As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID='GEN-GENSEPREPAIRBILLNO'")
        Dim OptAr As New List(Of String)

        OptAr.Add("EXP")
        OptAr.Add("EXEM")
        OptAr.Add("HSN")
        OptAr.Add("DOCS")
        OptAr.Add("CC")
        OptAr.Add("SS")
        OptAr.Add("AT")
        OptAr.Add("ATADJ")
        OptAr.Add("CDNR")
        OptAr.Add("CDNRU")
        OptAr.Add("CDNRUS")
        OptAr.Add("B2B")
        OptAr.Add("B2CL")
        OptAr.Add("B2CS")

        Dim cnt As Integer = 0
        For Each opt As String In OptAr
            cnt += 1

            Dim TITLE As String = ""
            If opt = "B2B" Then
                TITLE += " B2B"
            ElseIf opt = "B2CL" Then
                TITLE += " B2C LARGE"
            ElseIf opt = "B2CS" Then
                TITLE += " B2C SMALL"
            ElseIf opt = "CDNR" Then
                TITLE += " CREDIT NOTE [REGISTERED]"
            ElseIf opt = "CDNRU" Then
                TITLE += " CREDIT NOTE [UNREGISTERED]"
            ElseIf opt = "CDNRUS" Then
                TITLE += " CREDIT NOTE [UNREGISTERED SMALL]"
            ElseIf opt = "EXP" Then
                TITLE += " EXPORTS"
            ElseIf opt = "AT" Then
                TITLE += " ADVANCE RECEIVED"
            ElseIf opt = "ATADJ" Then
                TITLE += " ADVANCE ADJUSTED"
            ElseIf opt = "CC" Then
                TITLE += " SCHEME COLLECTION"
            ElseIf opt = "SS" Then
                TITLE += " SCHEME ADJUSTED"
            ElseIf opt = "EXEM" Then
                TITLE += " EXEMPTED"
            ElseIf opt = "HSN" Then
                TITLE += " HSN"
            ElseIf opt = "DOCS" Then
                TITLE += " DOCUMENTS"
            End If
            TITLE += " GST REPORT FOR TAX PERIOD " & dtpFrom.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            If rbtCDNR.Checked Or rbtCDNRU.Checked Or rbtCDNRUL.Checked Then
                TITLE += " [EXCLUDE VAT BILLS]"
            End If
            Dim _Metal As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text.ToString & "'")
            xlWorkSheet = xlWorkBook.Sheets("sheet" & cnt & "")
            xlWorkSheet.Name = opt
            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR1A"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@MONTH = '" & dtpFrom.Value.Month & "'"
            strSql += vbCrLf + " ,@YEAR = '" & dtpFrom.Value.Year & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@OPTION = '" & opt & "'"
            strSql += vbCrLf + " ,@STATEID = '" & CompanyStateId & "'"
            strSql += vbCrLf + " ,@ADVGST = '" & IIf(ChkAdvGst.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHSR = '" & IIf(ChkSR.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@BILLPREFIXDB='" & IIf(ChkBillPrefic.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@GSTDATE = '" & GstDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & _Metal & "'"
            strSql += vbCrLf + " ,@GROUPBYCOSTCENTRE = 'N'"
            strSql += vbCrLf + " ,@RDSEPBILLCHECK = '" & RDSEPBILLCHECK & "'"
            strSql += vbCrLf + " ,@DATECHECK = '" & IIf(chkdate.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@DATEFROM = '" & dtpbFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@DATETO = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@B2BPRJB='" & IIf(chkb2bprjb.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@HSNDISC='" & IIf(chkhsndesc.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 100000000
            Dim dtGrid As New DataTable
            da = New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            'If ds.Tables.Count = 0 Then
            '    MsgBox("Record Not Found", MsgBoxStyle.Information)
            '    dtpFrom.Select()
            '    Exit Sub
            'End If
            If ds.Tables.Count = 0 Then
                Continue For
            End If
            dtGrid = ds.Tables(0)
            'If dtGrid.Rows.Count = 0 Then
            '    MsgBox("Record Not Found", MsgBoxStyle.Information)
            '    dtpFrom.Select()
            '    Exit Sub
            'End If
            gridView.DataSource = Nothing
            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)

            If opt = "B2B" Then
                FillGridGroupStyle_KeyNoWise(gridView, "GSTNO")
            ElseIf opt = "B2CL" Then
                FillGridGroupStyle_KeyNoWise(gridView, "TRANNO")
            ElseIf opt = "AT" Or opt = "ATADJ" Then
                FillGridGroupStyle_KeyNoWise(gridView, "STATE")
            ElseIf opt = "HSN" Or opt = "DOCS" Then
                FillGridGroupStyle_KeyNoWise(gridView, "HSN")
            Else
                FillGridGroupStyle_KeyNoWise(gridView, "GSTNO")
            End If
            With gridView
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                If .Columns.Contains("BILLTYPE") Then .Columns("BILLTYPE").Visible = False
                If .Columns.Contains("COSTID") Then .Columns("COSTID").Visible = False
                If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
                If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
                If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("REFDATE") Then .Columns("REFDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("GSTNO") Then .Columns("GSTNO").HeaderText = "GSTIN/UIN of Recipient"
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Invoice Number"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Invoice Date"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Invoice Value"
                If .Columns.Contains("STATE") Then .Columns("STATE").HeaderText = "Place of Supply"
                If .Columns.Contains("RCM") Then .Columns("RCM").HeaderText = "Reverse Charge"
                If .Columns.Contains("INVTYPE") Then .Columns("INVTYPE").HeaderText = "Invoice Type"
                If .Columns.Contains("ECOMGSTNO") Then .Columns("ECOMGSTNO").HeaderText = "E-Commerce GSTIN"
                If .Columns.Contains("RATE") Then .Columns("RATE").HeaderText = "Rate"
                If .Columns.Contains("TAX") Then .Columns("TAX").HeaderText = "Taxable Value"
                If .Columns.Contains("CESS") Then .Columns("CESS").HeaderText = "Cess Amount"
                If .Columns.Contains("RCM") Then .Columns("RCM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("REFNO") Then .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MMM-yyyy"
                If .Columns.Contains("REFDATE") Then .Columns("REFDATE").DefaultCellStyle.Format = "dd-MMM-yyyy"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TAX") Then .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CESS") Then .Columns("CESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGST") Then .Columns("IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGST") Then .Columns("CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGST") Then .Columns("SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("QTY") Then .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SNOFROM") Then .Columns("SNOFROM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SNOTO") Then .Columns("SNOTO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TOTAL") Then .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CANCEL") Then .Columns("CANCEL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If opt = "B2CL" Then
                    If .Columns.Contains("GSTNO") Then .Columns("GSTNO").Visible = False
                    If .Columns.Contains("INVTYPE") Then .Columns("INVTYPE").Visible = False
                    If .Columns.Contains("RCM") Then .Columns("RCM").Visible = False
                    If .Columns.Contains("ECOMGSTNO") Then .Columns("ECOMGSTNO").Visible = False
                ElseIf opt = "B2CS" Then
                    If .Columns.Contains("GSTNO") Then .Columns("GSTNO").HeaderText = "Type"
                ElseIf opt = "CDNR" Or opt = "CDNRU" Or opt = "CDNRUS" Then
                    If opt = "CDNRU" Or opt = "CDNRUS" Then
                        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Note/Refund Voucher Number"
                        If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Note/Refund Voucher date"
                        If .Columns.Contains("REFNO") Then .Columns("REFNO").HeaderText = "Invoice/Advance Receipt Number"
                        If .Columns.Contains("REFDATE") Then .Columns("REFDATE").HeaderText = "Invoice/Advance Receipt Date"
                    Else
                        If .Columns.Contains("REFNO") Then .Columns("REFNO").HeaderText = "Note/Refund Voucher Number"
                        If .Columns.Contains("REFDATE") Then .Columns("REFDATE").HeaderText = "Note/Refund Voucher date"
                        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Invoice/Advance Receipt Number"
                        If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Invoice/Advance Receipt Date"
                    End If
                    If .Columns.Contains("DOCTYPE") Then .Columns("DOCTYPE").HeaderText = "Document Type"
                    If .Columns.Contains("REASON") Then .Columns("REASON").HeaderText = "Reason For Issuing document"
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Note/Refund Voucher Value"
                    If .Columns.Contains("DOCTYPE") Then .Columns("DOCTYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    If .Columns.Contains("PREGST") Then .Columns("PREGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                ElseIf opt = "AT" Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Gross Advance Received"
                ElseIf opt = "ATADJ" Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Gross Advance Adjusted"
                ElseIf opt = "HSN" Then
                    If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Total Value"
                    If .Columns.Contains("TAX") Then .Columns("TAX").HeaderText = "Taxable Value"
                    If .Columns.Contains("IGST") Then .Columns("IGST").HeaderText = "Integrated Tax Amount"
                    If .Columns.Contains("CGST") Then .Columns("CGST").HeaderText = "Central Tax Amount"
                    If .Columns.Contains("SGST") Then .Columns("SGST").HeaderText = "State/UT Tax Amount"
                    If .Columns.Contains("DESCRIPTION") Then .Columns("DESCRIPTION").HeaderText = "Description"
                    If .Columns.Contains("QTY") Then .Columns("QTY").HeaderText = "Total Quantity"
                ElseIf opt = "DOCS" Then
                    If .Columns.Contains("HSN") Then .Columns("HSN").HeaderText = "Nature  of Document"
                    If .Columns.Contains("SNOFROM") Then .Columns("SNOFROM").HeaderText = "Sr. No. From"
                    If .Columns.Contains("SNOTO") Then .Columns("SNOTO").HeaderText = "Sr. No. To"
                    If .Columns.Contains("TOTAL") Then .Columns("TOTAL").HeaderText = "Total Number"
                    If .Columns.Contains("CANCEL") Then .Columns("CANCEL").HeaderText = "Cancelled"
                End If
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With

            Dim columnsCount As Integer = gridView.Columns.Count
            'For Each column As DataGridViewColumn In gridView.Columns
            '    If column.Visible = False Then Continue For
            '    xlWorkSheet.Cells(1, column.Index + 1).Value = column.HeaderText
            'Next



            Dim sp() As String = TITLE.Split(Chr(Keys.Enter))
            For Each s As String In sp
                Dim _Range As Excel.Range = xlWorkSheet.Cells(1, 1)
                With _Range
                    s = s.Replace(Chr(Keys.Enter), "")
                    If s <> "" Then
                        If Asc(Mid(s, 1, 1)) = 10 Then
                            s = s.Replace(Chr(Asc(Mid(s, 1, 1))), "")
                        End If
                    End If
                    .Merge()
                    '.HAlign = 1
                    '.VAlign = 2
                    .Value = s.Trim
                    .Font.Size = 9
                    .Font.Bold = True
                    .WrapText = False
                    .Interior.ColorIndex = 50
                    .Font.ColorIndex = 2
                End With
            Next

            For i = 0 To gridView.RowCount - 1

                For j = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(j).Visible = False Then Continue For
                    Dim fBold As Boolean = False
                    If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "R" Or gridView.Rows(i).Cells("COLHEAD").Value.ToString = "R1" Or gridView.Rows(i).Cells("COLHEAD").Value.ToString = "R2" Then
                        fBold = True
                    End If
                    Dim rn As Excel.Range = xlWorkSheet.Cells(i + 2, j + 1)
                    rn.Value = gridView(j, i).Value.ToString()
                    rn.Font.Color = gridView(j, i).Style.ForeColor.ToArgb
                    'If Not gridView(j, i).Style.Font Is Nothing Then
                    '    rn.Font.Bold = gridView(j, i).Style.Font.Bold
                    'End If

                    If fBold Then
                        rn.Font.Bold = True
                        rn.Interior.Color = Color.LightGoldenrodYellow
                    End If

                    'rn.ColumnWidth = AutoSize

                    'rn.Font.Color = gridView(j, i).Style.ForeColor
                    'rn.ColumnWidth = gridView.Columns(j).Width


                    'If gridView(j, i).Style.BackColor.IsKnownColor = False Then
                    '    rn.Interior.Color = Color.WhiteSmoke
                    'Else
                    '    rn.Interior.Color = gridView(j, i).Style.BackColor.ToArgb
                    'End If

                    'If gridView(j, i).Style.ForeColor.IsKnownColor = False Then
                    '    rn.Font.Color = Color.Black
                    'Else
                    '    rn.Font.Color = gridView(j, i).Style.ForeColor
                    'End If

                    'rn.WrapText = True
                    'xlWorkSheet.Cells(i + 1, j + 1) = gridView(j, i).Value.ToString()

                Next
            Next

        Next

        TryCast(xlWorkBook.Sheets("Sheet15"), Excel.Worksheet).Delete()
        TryCast(xlWorkBook.Sheets("Sheet16"), Excel.Worksheet).Delete()
        TryCast(xlWorkBook.Sheets("Sheet17"), Excel.Worksheet).Delete()


        '   Dim OpenDialog As New OpenFileDialog
        '   Dim Str As String
        '   Str = "(*.xls) | *.xls"
        '   Str += "|(*.xlsx) | *.xlsx"
        '   Str += "|(*.csv) | *.csv"
        '   If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
        '       Dim path As String = OpenDialog.FileName
        '       If path <> "" Then
        '           xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
        'Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
        '       End If
        '   End If

        Dim OpenDialog As New SaveFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        Str += "|(*.csv) | *.csv"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
     Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
        End If


        xlWorkBook.Close(True, misValue, misValue)
        xlApp.Quit()

        releaseObject(xlWorkSheet)
        releaseObject(xlWorkBook)
        releaseObject(xlApp)

        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        '    xlWorkBook.Close(True, misValue, misValue)
        '    xlApp.Quit()

        '    releaseObject(xlWorkSheet)
        '    releaseObject(xlWorkBook)
        '    releaseObject(xlApp)
        '    Exit Sub
        'End Try
    End Sub




    Private Sub SalesAbsJSON()
        gridView.DataSource = Nothing
        Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim OptAr As New List(Of String)
        OptAr.Add("B2B")
        OptAr.Add("B2CS")
        OptAr.Add("EXP")
        OptAr.Add("HSN")

        Dim ds As New DataSet
        Dim dsr As New DataSet

        Dim cnt As Integer = 0
        For Each opt As String In OptAr
            Dim _Metal As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text.ToString & "'")
            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR1A"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@MONTH = '" & dtpFrom.Value.Month & "'"
            strSql += vbCrLf + " ,@YEAR = '" & dtpFrom.Value.Year & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@OPTION = '" & opt & "'"
            strSql += vbCrLf + " ,@STATEID = '" & CompanyStateId & "'"
            strSql += vbCrLf + " ,@ADVGST = '" & IIf(ChkAdvGst.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHSR = '" & IIf(ChkSR.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@BILLPREFIXDB='" & IIf(ChkBillPrefic.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@GSTDATE = '" & GstDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & _Metal & "'"
            strSql += vbCrLf + " ,@GROUPBYCOSTCENTRE = 'N'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 100000000
            Dim dtGrid As DataTable
            da = New OleDbDataAdapter(cmd)
            dtGrid = New DataTable
            da.Fill(ds, opt)
        Next

        Try
            Dim jsn As New CallApi.CreGSTR1
            Dim gstno As String = ""
            strSql = "SELECT GSTNO FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" + strCompanyId + "'"
            gstno = objGPack.GetSqlValue(strSql,, "")
            jsn.FuncGSTR1(gstno, dtpFrom.Value.Month.ToString("00") + dtpFrom.Value.Year.ToString("0000"), ds)
            MsgBox("JSON File created.")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub




    'Private Sub releaseObject(ByVal obj As Object)
    '    Try
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
    '        obj = Nothing
    '    Catch ex As Exception
    '        obj = Nothing
    '        MessageBox.Show("Exception Occured while releasing object " + ex.ToString())
    '    Finally
    '        GC.Collect()
    '    End Try
    'End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        chkCount = 0
        pnlHeading.Visible = False
        If rbtAll.Checked Then
            SalesAbsAll()
        Else
            SalesAbs()
        End If
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
        If _COSTCENTREMAINTAIN Then
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If
        chkGroupByCostcentre.Visible = False
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
        'dtpFrom.Value = GetServerDate()
        If GetAdmindbSoftValue("ENTRYDATE", "Y", tran) = "Y" Then
            Dim dd As Date = CType(GetSqlValue(cn, "SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),101))"), Date)
            Try
                dtpFrom.Value = Format(dd, "yyyy-MM-dd")
            Catch ex As Exception

            End Try
        Else
            If Today.Date > cnTranToDate.Date Then
                dtpFrom.Value = cnTranToDate.Date
            ElseIf Today.Date < cnTranFromDate.Date Then
                dtpFrom.Value = cnTranFromDate.Date
            Else
                dtpFrom.Value = Today.Date
            End If
        End If
        dtpbFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        chkdate.Checked = False
        dtpbFrom.Enabled = False
        dtpTo.Enabled = False
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        chkCount = 0
        Prop_Gets()
        lblFindHelp.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Function B2B_Address_Validator(ByVal btchNo As String, ByVal Billno As String) As Boolean
        If GetSqlValue(cn, "SELECT ISNULL(EINVOICETYPE,'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'") <> "" And GST_EINV_MASTER_AUTOUPLOAD _
            And GetSqlValue(cn, "SELECT ISNULL(GSTNO,'')GSTNO FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO _
                        WHERE BATCHNO='" & btchNo & "')") = "" Then
            strSql = vbCrLf + " SELECT ISNULL(ACCODE,'') ACCODE,PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL"
            strSql += vbCrLf + " ,'" & GST_EINV_MASTER_EXPORT_GSTIN.ToString & "' GSTNO,'" & GST_EINV_MASTER_EXPORT_PINCODE.ToString & "' PINCODE,'" & GST_EINV_MASTER_EXPORT_STATECODE.ToString & "' STATECODE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "')  "
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtBuyer As New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Columns(k).ColumnName.ToString = "ACCODE" And dtBuyer.Rows(0)("ACCODE").ToString = "" Then Continue For
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
        Else
            strSql = vbCrLf + " SELECT ISNULL(ACCODE,'') ACCODE,PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') AND ISNULL(ACCODE,'')='' "
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtBuyer As New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                If dtBuyer.Rows(0)("ACCODE").ToString <> "" Then GoTo ACCODECHK
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Columns(k).ColumnName.ToString = "ACCODE" And dtBuyer.Rows(0)("ACCODE").ToString = "" Then Continue For
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
ACCODECHK:
            strSql = vbCrLf + " SELECT ACNAME PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,EMAILID EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE  "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE IN (SELECT ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "' UNION ALL SELECT ACCODE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & btchNo & "')"
            da = New OleDbDataAdapter(strSql, cn)
            dtBuyer = New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
        End If

        Return False
    End Function


    'Function B2B_Upload(ByVal _Batchno As String)
    '    'If Not rbtB2B.Checked = True Then Exit Function
    '    If rbtB2B.Checked = True Then
    '    ElseIf rbtCDNR.Checked = True Then
    '    Else
    '        Exit Function
    '    End If

    '    Dim batchno As String = _Batchno 'gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString
    '    Try
    '        If B2B_E_INVOICE Or GST_EINV_OFFLINE_JSON Then
    '            Dim btchNo As String = _Batchno 'gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString
    '            'strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID)STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
    '            strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,CONVERT(VARCHAR,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID))STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
    '            da = New OleDbDataAdapter(strSql, cn)
    '            Dim dtComp As New DataTable
    '            Dim dtItem As New DataTable
    '            Dim dtBuyer As New DataTable
    '            da.Fill(dtComp)
    '            Dim GST_chk As Boolean = True
    '            If dtComp.Rows(0)("GSTNO").ToString() = "" Then
    '                MsgBox("Invalid GSTNO")
    '                GST_chk = False
    '            End If
    '            If GST_EINV_OFFLINE_JSON = False Then
    '                If GST_EINV_URL.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_URL")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_CLIENTID.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_CLIENTID")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_USERNAME.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_USERNAME")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_PASSWORD.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_PASSWORD")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_CLIENTSECRET.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_CLIENTSECRET")
    '                    GST_chk = False
    '                End If
    '                If GST_EINV_PUBLICKEY.ToString = "" Then
    '                    MsgBox("Invalid GST_EINV_PUBLICKEY")
    '                    GST_chk = False
    '                End If
    '            End If
    '            If dtComp.Rows.Count > 0 And GST_chk = True Then

    '                strSql = vbCrLf + " SELECT "
    '                strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.ITEMNAME,A.TAGNO,ISNULL(B.HSN,'')HSN,A.PCS PCS,ISNULL(A.GRSWT,0)GRSWT,A.SALEMODE,ISNULL(AMOUNT,0)+ISNULL(TAX,0) TOTALAMT,AMOUNT"
    '                strSql += vbCrLf + " ,ISNULL(TAX,0)TAX,ISNULL(DISCOUNT,0)DISCOUNT,ISNULL(C.S_IGSTTAX,0) SALESTAX,A.TRANTYPE," 'ISNULL(C.CESSPER,0)CESSTAX"
    '                strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
    '                strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
    '                strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
    '                strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN COSTID + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN A.TRANTYPE='SR' THEN  COSTID + '/SR/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"
    '                If rbtCDNR.Checked = True Then
    '                    strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..RECEIPT AS A "
    '                ElseIf rbtB2B.Checked = True Then
    '                    strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ISSUE AS A "
    '                End If
    '                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
    '                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
    '                If rbtCDNR.Checked = True Then
    '                    strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SR')"
    '                ElseIf rbtB2B.Checked = True Then
    '                    strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SA','RD','OD','IIN')"
    '                End If
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtItem)

    '                strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE "
    '                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "')"
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtBuyer)

    '                If dtItem.Rows.Count = 0 Then
    '                    Exit Function
    '                End If

    '                If dtBuyer.Rows.Count = 0 Then
    '                    strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,phoneno PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
    '                    strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE "
    '                    strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "')"
    '                    da = New OleDbDataAdapter(strSql, cn)
    '                    da.Fill(dtBuyer)
    '                    If dtBuyer.Rows.Count = 0 Then
    '                        Exit Function
    '                    End If
    '                End If

    '                If dtBuyer.Rows(0)("GSTNO").ToString = "" Then
    '                    MsgBox("Party Does not have GST No")
    '                    Exit Function
    '                End If

    '                Dim crd As New CallApi.B2BInv.CREDENTIALS()
    '                crd.GSTNO = dtComp.Rows(0)("GSTNO").ToString()
    '                crd.CLIENTID = GST_EINV_CLIENTID
    '                crd.USERNAME = GST_EINV_USERNAME
    '                crd.PASSWORD = GST_EINV_PASSWORD
    '                crd.SECERET = GST_EINV_CLIENTSECRET
    '                crd.PublicKey = GST_EINV_PUBLICKEY

    '                Dim _api As New CallApi.PushData
    '                Dim cls As New CallApi.B2BInv.Para
    '                cls._COMPANY = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.COMPANY)(dtComp)(0)
    '                cls._BUYER = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.BUYER)(dtBuyer)(0)
    '                cls._ITEM = CallApi.B2BInv.ConvertDataTable(Of CallApi.B2BInv.ITEM)(dtItem)
    '                cls._CREDENTIALS = crd

    '                _api.apiurl = GST_EINV_URL
    '                If GST_EINV_OFFLINE_JSON Then
    '                    Dim vv As New CallApi.Offline
    '                    Dim dd = vv.clsJon(dtComp.Rows(0), dtBuyer.Rows(0), dtItem)
    '                    Exit Function
    '                End If

    '                ''Exit Function

    '                Dim res As List(Of String) = _api.Callapijson(JsonConvert.SerializeObject(cls))
    '                If res.Count = 3 Then
    '                    If res(2) <> "" Then
    '                        MessageBox.Show(res(2), "Error from GST Portal")
    '                    Else
    '                        strSql = "INSERT INTO " & cnStockDb & "..EINVTRAN(BATCHNO,IRN,QRDATA) SELECT '" & btchNo & "','" & res(0) & "','" & res(1) & "'"
    '                        cmd = New OleDbCommand(strSql, cn)
    '                        cmd.ExecuteNonQuery()
    '                        ''ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

    '                        Dim objp As New BrighttechPack.Methods
    '                        MsgBox("Uploaded")
    '                    End If
    '                Else
    '                    MessageBox.Show("Bill not updated in GST portal " & IIf(res.Count > 1, res(0), ""), "Error from GST Portal")
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Function


    Function B2B_Upload_Multi()

        Dim ofln As New CallApi.Offline
        Dim incls As New CallApi.Offline.InputClass
        Dim _incls As New List(Of CallApi.Offline.InputClass)

        'If Not rbtB2B.Checked = True Then Exit Function
        If rbtB2B.Checked = True Then
        ElseIf rbtCDNR.Checked = True Then
        Else
            Exit Function
        End If

        Try
            For Each dr As DataGridViewRow In gridView.Rows
                incls = New CallApi.Offline.InputClass
                If dr.DefaultCellStyle.BackColor = Color.Lavender Then
                    Dim batchno As String = dr.Cells("BATCHNO").Value.ToString
                    If B2B_E_INVOICE Or GST_EINV_OFFLINE_JSON Then
                        Dim btchNo As String = batchno
                        strSql = vbCrLf + " SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,EMAIL,GSTNO,ISNULL(AREACODE,0)AREACODE,CONVERT(VARCHAR,(SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATEID=A.STATEID))STATECODE  FROM " & cnAdminDb & "..COMPANY AS A WHERE COMPANYID='" & strCompanyId & "'"
                        da = New OleDbDataAdapter(strSql, cn)
                        Dim dtComp As New DataTable
                        Dim dtItem As New DataTable
                        Dim dtBuyer As New DataTable
                        da.Fill(dtComp)
                        Dim GST_chk As Boolean = True
                        If dtComp.Rows(0)("GSTNO").ToString() = "" Then
                            MsgBox("Invalid GSTNO")
                            GST_chk = False
                        End If

                        If dtComp.Rows.Count > 0 And GST_chk = True Then
                            strSql = vbCrLf + " SELECT "
                            strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY SNO)SNO,B.ITEMNAME,A.TAGNO,ISNULL(B.HSN,'')HSN,A.PCS PCS,ISNULL(A.GRSWT,0)GRSWT,A.SALEMODE,ISNULL(AMOUNT,0)+ISNULL(TAX,0) TOTALAMT,AMOUNT"
                            strSql += vbCrLf + " ,ISNULL(TAX,0)TAX,ISNULL(DISCOUNT,0)DISCOUNT,ISNULL(C.S_IGSTTAX,0) SALESTAX,A.TRANTYPE," 'ISNULL(C.CESSPER,0)CESSTAX"
                            strSql += vbCrLf + "  CONVERT(NUMERIC(15,2),CASE WHEN ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0) =0"
                            strSql += vbCrLf + "  THEN 0 ELSE ISNULL(NULL,0) END) CESSTAX"
                            strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CG'),0)CGST"
                            strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='SG'),0)SGST"
                            strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='IG'),0)IGST"
                            strSql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO=A.SNO AND ISNULL(STUDDED,'')<>'Y' AND TAXID='CS'),0)CESS"
                            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=A.BATCHNO AND ACCODE='TCS'),0))TCS"
                            'strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN COSTID + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN A.TRANTYPE='SR' THEN  COSTID + '/SR/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"
                            strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO)  WHEN A.TRANTYPE='IPU' THEN (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/IPU/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') THEN  (CASE WHEN ISNULL(COSTID,'')='' THEN A.COMPANYID ELSE COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END TRANNO,"
                            strSql += vbCrLf + " ISNULL(B.ISSERVICE,'N') ISSERVICE,"
                            If rbtCDNR.Checked = True Then
                                strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..RECEIPT AS A "
                            ElseIf rbtB2B.Checked = True Then
                                strSql += vbCrLf + " REPLACE(CONVERT(VARCHAR(12),TRANDATE,105),'-','/')TRANDATE,CONVERT(NUMERIC(15,2),RATE)RATE,BATCHNO FROM " & cnStockDb & "..ISSUE AS A "
                            End If
                            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
                            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
                            If rbtCDNR.Checked = True Then
                                strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SR')"
                            ElseIf rbtB2B.Checked = True Then
                                strSql += vbCrLf + " WHERE BATCHNO='" & btchNo & "' AND A.TRANTYPE IN ('SA','RD','OD','IIN')"
                            End If
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtItem)

                            strSql = vbCrLf + " SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE "
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') AND ISNULL(ACCODE,'')=''"
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtBuyer)

                            If dtBuyer.Rows.Count = 0 Then
                                strSql = "  SELECT acname PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,phoneno PHONERES,emailid EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,"
                                strSql += " CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATEid=A.STATEid),0))STATECODE "
                                strSql += " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "')"
                                da = New OleDbDataAdapter(strSql, cn)
                                da.Fill(dtBuyer)
                            End If

                            If dtBuyer.Rows(0)("GSTNO").ToString = "" Then
                                MsgBox("Party Does not have GST No")
                                Exit Function
                            End If
                            incls.drBuyer = dtBuyer.Rows(0)
                            incls.drComp = dtComp.Rows(0)
                            incls.dtItem = dtItem
                            _incls.Add(incls)
                        End If
                    End If

                End If
            Next

            If GST_EINV_OFFLINE_JSON Then
                Dim vv As New CallApi.Offline
                Dim dd = vv.clsJonMulti(_incls)
                Exit Function
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "A" Then
            If rbtB2B.Checked = True Then
            ElseIf rbtCDNR.Checked = True Then
            Else
                Exit Sub
            End If
            For Each dr As DataGridViewRow In gridView.Rows
                If dr.Cells("IRN").Value.ToString <> "" Then
                    Continue For
                End If
                If dr.Cells("RESULT").Value.ToString = "5" Then
                    dr.DefaultCellStyle.BackColor = Color.Lavender
                End If
            Next
        ElseIf UCase(e.KeyChar) = Chr(Keys.Space) Then
            If rbtB2B.Checked = True Then
            ElseIf rbtCDNR.Checked = True Then
            Else
                Exit Sub
            End If
            If gridView.Item("IRN", gridView.CurrentRow.Index).Value.ToString <> "" Then
                MsgBox("IRN already generated.")
                Exit Sub
            End If
            If B2B_Address_Validator(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, gridView.Item("TRANNO", gridView.CurrentRow.Index).Value.ToString) = False Then
                Exit Sub
            End If
            If gridView.Item("RESULT", gridView.CurrentRow.Index).Value.ToString = "5" Then
                If gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Lavender Then
                    gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Empty
                    chkCount -= 1
                Else
                    gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Lavender
                    chkCount += 1
                End If
            End If
        ElseIf UCase(e.KeyChar) = "G" Then
            Dim _Type As String = "SA"
            If rbtB2B.Checked = True Then
                _Type = "SA"
            ElseIf rbtCDNR.Checked = True Then
                _Type = "SR"
            ElseIf rbtExp.Checked = True Then
                _Type = "SA"
            Else
                Exit Sub
            End If
            If chkCount >= 1 Then
                B2B_Upload_Multi()
                Exit Sub
            End If
            If gridView.Item("IRN", gridView.CurrentRow.Index).Value.ToString <> "" Then
                MsgBox("IRN already generated.")
                Exit Sub
            End If
            If B2B_Address_Validator(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString) = False Then
                Exit Sub
            End If
            If MessageBox.Show("Do you want upload Bill to Portal", "GST", MessageBoxButtons.YesNo) = DialogResult.No Then
                Exit Sub
            End If
            Dim arp As New ClearTaxInv
            arp.B2B_Upload(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, _Type)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmGSTR1A_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR1A_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR1A_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR1A_Properties))
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

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub rbtATADJ_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtATADJ.CheckedChanged
        ChkAdvGst.Visible = rbtATADJ.Checked
    End Sub

    Private Sub rbtHSN_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtHSN.CheckedChanged, rbtB2CS.CheckedChanged, rbtB2B.CheckedChanged, rbtB2CL.CheckedChanged
        Dim chk As RadioButton = CType(sender, Control)
        ChkSR.Visible = chk.Checked
        chkHsnRate.Visible = rbtHSN.Checked
        chkhsndesc.Visible = rbtHSN.Checked

        chkHsnRate.Checked = False
        chkhsndesc.Checked = False
        If chkCmbCostCentre.Text = "ALL" And rbtB2B.Checked And _COSTCENTREMAINTAIN And _GROUPBYCOSTCENTRE Then
            chkGroupByCostcentre.Visible = True
            chkGroupByCostcentre.Checked = False
        Else
            chkGroupByCostcentre.Visible = False
            chkGroupByCostcentre.Checked = False
        End If
        If rbtB2B.Checked = True Or rbtB2CL.Checked = True Or rbtCDNR.Checked = True Or rbtCDNRU.Checked = True Then
            ChkBillPrefic.Visible = True
            If rbtB2B.Checked = True Then
                chkb2bprjb.Visible = True
            Else
                chkb2bprjb.Visible = False
            End If
        End If
    End Sub

    Private Sub rbtDocs_CheckedChanged(sender As Object, e As EventArgs) Handles rbtDocs.CheckedChanged
        Dim chk As RadioButton = CType(sender, Control)
        ChkBillPrefic.Visible = chk.Checked
    End Sub

    Private Sub rbtAll_CheckedChanged(sender As Object, e As EventArgs) Handles rbtAll.CheckedChanged
        If rbtAll.Checked Then
            btnView_Search.Text = "&Generate"
        Else
            btnView_Search.Text = "&View"
        End If
    End Sub

    Private Sub btnJson_Click(sender As Object, e As EventArgs) Handles btnJson.Click
        SalesAbsJSON()
    End Sub

    Private Sub chkCmbCostCentre_Leave(sender As Object, e As EventArgs) Handles chkCmbCostCentre.Leave
        If chkCmbCostCentre.Text = "ALL" And rbtB2B.Checked And _COSTCENTREMAINTAIN And _GROUPBYCOSTCENTRE Then
            chkGroupByCostcentre.Visible = True
        Else
            chkGroupByCostcentre.Visible = False
            chkGroupByCostcentre.Checked = False
        End If
    End Sub

    Private Sub rbtCDNRUL_CheckedChanged(sender As Object, e As EventArgs) Handles rbtCDNRUL.CheckedChanged
        ChkBillPrefic.Visible = rbtCDNRUL.Checked
    End Sub

    Private Sub rbtCDNR_CheckedChanged(sender As Object, e As EventArgs) Handles rbtCDNR.CheckedChanged
        ChkBillPrefic.Visible = rbtCDNR.Checked
    End Sub

    Private Sub rbtExp_CheckedChanged(sender As Object, e As EventArgs) Handles rbtExp.CheckedChanged
        ChkBillPrefic.Visible = rbtExp.Checked
    End Sub

    Private Sub rbtCDNRU_CheckedChanged(sender As Object, e As EventArgs) Handles rbtCDNRU.CheckedChanged
        ChkBillPrefic.Visible = rbtCDNRU.Checked
    End Sub

    Private Sub FindToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView)
        objSearch.ShowDialog()
    End Sub

    Private Sub chkdate_CheckedChanged(sender As Object, e As EventArgs) Handles chkdate.CheckedChanged
        If chkdate.Checked = True Then
            dtpFrom.Enabled = False
            dtpbFrom.Enabled = True
            dtpTo.Enabled = True
        Else
            dtpFrom.Enabled = True
            dtpbFrom.Enabled = False
            dtpTo.Enabled = False
        End If
    End Sub


End Class

Public Class frmGSTR1A_Properties
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