Imports System.Data.OleDb

Public Class frmStaxTdsReport

    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim dtacname As New DataTable
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If gridView.DataSource IsNot Nothing Then CType(gridView.DataSource, DataTable).Rows.Clear()
        If gridTot.DataSource IsNot Nothing Then CType(gridTot.DataSource, DataTable).Rows.Clear()
        Me.Refresh()
        Dim viewtype As String
        Dim mtemptable As String = "TEMPTABLEDB..TEMP" & Trim(Guid.NewGuid.ToString.Substring(0, 5))
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkaccode As String = GetQryStringForSp(chkcmbacname.Text, cnAdminDb & "..ACHEAD", "ACCODE", "ACNAME", True)
        If rbttds.Checked = True Then
            viewtype = "TDS"
        Else
            viewtype = "STAX"
        End If
        Dim all As String
        all = "''" & "all" & "''"

        strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_SERVICETAXREPORT"
        strSql += vbCrLf + "@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + ",@DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTABLE = '" & mtemptable & "'"
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE = '" & cnTranFromDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If chkCostId = "ALL" Then
            strSql += vbCrLf + ",@COSTIDS  ='" & chkCostId & "'"
        Else
            strSql += vbCrLf + ",@COSTIDS  =""" & IIf(chkCostId = "", "'" & "ALL" & "'", chkCostId) & """"
        End If
        If chkaccode = "ALL" Then
            strSql += vbCrLf + ",@ACCODE  ='" & chkaccode & "'"
        Else
            strSql += vbCrLf + ",@ACCODE  =""" & IIf(chkaccode = "", "'" & "ALL" & "'", chkaccode) & """"
        End If
        strSql += vbCrLf + ",@VIEWTYPE  ='" & viewtype & "'"

        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        dsGridView = New DataSet
        da.Fill(dsGridView)
        dtGrid = dsGridView.Tables(0)
        If dtGrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtGrid
            'gridView.Columns("SNO").Width = 50
            'gridView.Columns("PARTICULAR").Width = 250
            gridView.Columns("TAXTYPE").Visible = False
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("CATNAME").Visible = False
            With gridView
                If .Columns.Contains("ACNAME") Then .Columns("ACNAME").Visible = False
                If .Columns.Contains("CONTRA") Then .Columns("CONTRA").Visible = False
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TDSAMOUNT") Then .Columns("TDSAMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TDSAMOUNT") Then .Columns("TDSAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TDSPER") Then .Columns("TDSPER").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TDSPER") Then .Columns("TDSPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("STAXAMOUNT") Then .Columns("STAXAMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("STAXAMOUNT") Then .Columns("STAXAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("STAXPER") Then .Columns("STAXPER").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("STAXPER") Then .Columns("STAXPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("GST") Then .Columns("GST").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("GST") Then .Columns("GST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("TRANDATE") Then
                    .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                    .Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                If .Columns.Contains("TRANNO") Then
                    .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            End With

            gridView.Columns("PARTICULAR").Width = 150
            gridView.Columns("ACNAME").Width = 150
            gridView.Columns("CATNAME").Width = 150
            lblTitle.Text = "STAX & TDS REPORT"
            If dtpTo.Enabled = False Then
                lblTitle.Text += " AS ON " & dtpFrom.Text & ""
            Else
                lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            End If
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            If chkCmbCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
            gridView.Focus()

        Else
            gridView.DataSource = Nothing
            lblTitle.Text = ""
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        For i As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "S" Or gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "T" Then
                gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                gridView.Rows(i).Cells("PARTICULAR").Style.ForeColor = Color.Red
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "C" Then
                gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.DarkGreen
                gridView.Rows(i).Cells("PARTICULAR").Style.ForeColor = Color.White
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
    End Sub

    Private Sub frmTrailBal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmTrailBal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGridHeading.Visible = False
        chkAsonDate.Checked = False
        ''CostCentre
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Visible = True
        Else
            chkCmbCostCentre.Visible = False
        End If
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(VARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTIVE='Y'"
        strSql += " ORDER BY RESULT,ACNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbacname, dtacname, "ACNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
        chkAsonDate.Select()
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            chkAsonDate.Text = "&As OnDate"
            dtpTo.Enabled = False
        Else
            chkAsonDate.Text = "&Date From"
            dtpTo.Enabled = True
        End If
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, Nothing)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        gridTot.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        chkAsonDate.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.X) Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
            '=====================================================
            '170908 modified
        ElseIf e.KeyChar = Chr(Keys.P) Or e.KeyChar = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
            '=======================================================
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, Nothing)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
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
        Dim obj As New frmTdsReport_Properties
        obj.p_chkAsonDate = chkAsonDate.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(chkcmbacname, obj.p_chkCmbacname)
        obj.p_chktotal = chktotal.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTdsReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTdsReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTdsReport_Properties))
        chkAsonDate.Checked = obj.p_chkAsonDate
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkcmbacname, obj.p_chkCmbacname, "ALL")
        chktotal.Checked = obj.p_chktotal
    End Sub

    Private Sub btnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerify.Click
        If gridView.DataSource Is Nothing Then Exit Sub
        Dim Dv As New DataView
        Dim Dt As New DataTable
        Dim DtVerify As New DataTable()
        DtVerify.Columns.Add(New DataColumn("TRANNO", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("TRANDATE", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("ACNAME", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("CONTRA", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("AMOUNT", Type.GetType("System.Double")))
        DtVerify.Columns.Add(New DataColumn("TDSPER", Type.GetType("System.Double")))
        DtVerify.Columns.Add(New DataColumn("TDSAMOUNT", Type.GetType("System.Double")))
        Dv = CType(gridView.DataSource, DataTable).Copy.DefaultView
        Dv.RowFilter = "RESULT =2"
        Dt = Dv.ToTable
        For i As Integer = 0 To Dt.Rows.Count - 1
            Dim Amt, TdsPer, TdsAmt As Decimal
            With Dt.Rows(i)
                Amt = .Item("AMOUNT").ToString
                TdsPer = Val(.Item("TDSPER").ToString)
                TdsAmt = Val(.Item("TDSAMOUNT").ToString)
                If TdsAmt <> ((Amt * TdsPer) / 100) Then
                    Dim dr As DataRow
                    dr = DtVerify.NewRow()
                    dr("TRANNO") = .Item("TRANNO").ToString
                    dr("TRANDATE") = .Item("TRANDATE").ToString
                    dr("ACNAME") = .Item("ACNAME").ToString
                    dr("CONTRA") = .Item("CONTRA").ToString
                    dr("AMOUNT") = .Item("AMOUNT").ToString
                    dr("TDSPER") = Val(.Item("TDSPER").ToString)
                    dr("TDSAMOUNT") = Val(.Item("TDSAMOUNT").ToString)
                    DtVerify.Rows.Add(dr)
                End If
            End With
        Next
        If DtVerify.Rows.Count > 0 Then
            GridViewHelp.DataSource = Nothing
            GridViewHelp.Refresh()
            GridViewHelp.DataSource = DtVerify
            FormatGridColumns(GridViewHelp, False, True, , False)
            GridViewHelp.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            GridViewHelp.Columns("TRANNO").Width = 75
            GridViewHelp.Columns("TDSPER").Width = 75
            GridViewHelp.Columns("AMOUNT").Width = 75
            GridViewHelp.Columns("ACNAME").Width = 175
            GridViewHelp.Columns("CONTRA").Width = 175
            GridViewHelp.Columns("TRANDATE").Width = 100
            PnlRange.Visible = True
        End If
    End Sub

    Private Sub GridViewHelp_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridViewHelp.KeyDown
        If e.KeyCode = Keys.Escape Or e.KeyCode = Keys.Enter Then
            GridViewHelp.DataSource = Nothing
            GridViewHelp.Refresh()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub GrpRange_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GrpRange.KeyDown
        If e.KeyCode = Keys.Escape Or e.KeyCode = Keys.Enter Then
            GridViewHelp.DataSource = Nothing
            GridViewHelp.Refresh()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub cmbType_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub rbttds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbttds.CheckedChanged
        If rbttds.Checked = True Then
            btnVerify.Visible = True
            btngenerate.Visible = True
        Else
            btnVerify.Visible = False
            btngenerate.Visible = False
        End If
    End Sub
    Public Function GetSelectedAccode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCostId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Private Sub btngenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngenerate.Click

        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkaccode As String = GetQryStringForSp(chkcmbacname.Text, cnAdminDb & "..ACHEAD", "ACCODE", "ACNAME", True)

        'PROCEDURE CHANGED BY VIGNESH FOR NAC 28-12-2015

        'strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN "
        'strSql += vbCrLf + " SELECT 0 SNO,ACCODE,TRANNO,TRANDATE,PAYMODE AS TRANTYPE,BATCHNO,TDSCATID AS TAXID,AMOUNT,TDSPER AS TAXPER, "
        'strSql += vbCrLf + " TDSAMOUNT AS TAXAMOUNT,'TD' AS TAXTYPE,1 AS TSNO FROM  " & cnStockDb & "..ACCTRAN WHERE isnull(TDSAMOUNT,0) > 0 "
        'strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnStockDb & "..TAXTRAN ) "
        'If chkAsonDate.Checked = False Then
        '    strSql += vbCrLf + " AND TRANDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'End If
        'If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
        '    strSql += vbCrLf + "  AND COSTID IN ( " & GetSelectedCostId(chkCmbCostCentre, True) & ") "
        'End If
        'If chkcmbacname_OWN.Text <> "ALL" And chkcmbacname_OWN.Text <> "" Then
        '    strSql += vbCrLf + "  AND ACCODE IN ( " & GetSelectedAccode(chkcmbacname_OWN, True) & ") "
        'End If

        strSql = "DELETE FROM " & cnStockDb & "..TAXTRAN WHERE TAXTYPE='TD' "
        strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM  " & cnStockDb & "..ACCTRAN WHERE 1=1 "
        If chkcmbacname.Text <> "ALL" And chkcmbacname.Text <> "" Then
            strSql += vbCrLf + "  AND ACCODE IN ( " & GetSelectedAccode(chkcmbacname, True) & ") "
        End If
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " AND TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND COSTID IN ( " & GetSelectedCostId(chkCmbCostCentre, True) & ") "
        End If
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP999TDSTAXTRAN') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP999TDSTAXTRAN"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 0 SNO,ACCODE,TRANNO,TRANDATE,PAYMODE,BATCHNO,TDSCATID AS TAXID,"
        strSql += vbCrLf + " (CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT,TDSPER AS TAXPER, "
        strSql += vbCrLf + " TDSAMOUNT AS TAXAMOUNT,'TD' AS TAXTYPE,1 AS TSNO, "
        strSql += vbCrLf + " CONTRA, "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)NAME,"
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.CONTRA)CONTRANAME,COMPANYID,ISNULL(COSTID,'')COSTID "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP999TDSTAXTRAN"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..ACCTRAN AS A"
        strSql += vbCrLf + " WHERE 1=1  "
        If chkcmbacname.Text <> "ALL" And chkcmbacname.Text <> "" Then
            strSql += vbCrLf + "  AND ACCODE IN ( " & GetSelectedAccode(chkcmbacname, True) & ") "
        Else
            strSql += vbCrLf + " AND ACCODE IN ("
            strSql += vbCrLf + " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(TDSFLAG,'') = 'Y' AND A.TDSAMOUNT <> 0"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACCODE,'') LIKE '%TDS%' "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT DISTINCT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY"
            strSql += vbCrLf + " ) "
        End If
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnStockDb & "..TAXTRAN )"
        If chkAsonDate.Checked = False Then
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND COSTID IN ( " & GetSelectedCostId(chkCmbCostCentre, True) & ") "
        End If
        strSql += vbCrLf + " AND PAYMODE IN ('JE','TR')"
        strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
        strSql += vbCrLf + " (SNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID,"
        strSql += vbCrLf + " AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,CONTRA,COMPANYID,COSTID)"
        strSql += vbCrLf + " SELECT 0 SNO,T.ACCODE,TRANNO,TRANDATE,PAYMODE AS TRANTYPE,BATCHNO,TDSCATID AS TAXID,"
        strSql += vbCrLf + " AMOUNT,TDSPER AS TAXPER,AMOUNT AS TAXAMOUNT,'TD' AS TAXTYPE,1 AS TSNO,CONTRA,COMPANYID,COSTID "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP999TDSTAXTRAN AS T"
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..TDSCATEGORY AS C ON T.ACCODE=C.ACCODE " FOR SRINIVASA
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..TDSCATEGORY AS C ON T.ACCODE=C.ACCODE "
        strSql += vbCrLf + " WHERE ISNULL(TAXAMOUNT,0)=0 "
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnStockDb & "..TAXTRAN )"
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMP999TDSTAXTRAN WHERE ISNULL(TAXAMOUNT,0)<>0)"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
        strSql += vbCrLf + " (SNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID,"
        strSql += vbCrLf + " AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,CONTRA,COMPANYID,COSTID)"
        strSql += vbCrLf + " SELECT 0 SNO,"
        'strSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID=T.TAXID),"
        strSql += vbCrLf + " CONTRA,"
        strSql += vbCrLf + " TRANNO,TRANDATE,PAYMODE AS TRANTYPE,BATCHNO,TAXID,"
        strSql += vbCrLf + " AMOUNT,TAXPER,TAXAMOUNT,'TD' AS TAXTYPE,1 AS TSNO,"
        strSql += vbCrLf + " ACCODE,"
        strSql += vbCrLf + " COMPANYID,COSTID "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP999TDSTAXTRAN AS T"
        strSql += vbCrLf + " WHERE ISNULL(TAXAMOUNT,0)<>0 "
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnStockDb & "..TAXTRAN )"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = "SELECT COUNT(*)CNT FROM TEMPTABLEDB..TEMP999TDSTAXTRAN WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..TAXTRAN)"
        Dim cntUnPassed As Double = 0
        cntUnPassed = objGPack.GetSqlValue(strSql, "CNT", "0")
        If cntUnPassed > 0 Then
            If MessageBox.Show(cntUnPassed.ToString & " Records not passed." & Environment.NewLine & "Please contact Administrator...", "BrighttechRetailJewellery", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        If MessageBox.Show("TDS and STAX are generated....", "BrighttechRetailJewellery", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If

    End Sub

    Private Sub btnEd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEd.Click
        Try

            Dim minVal As Decimal = Val(GetAdmindbSoftValue("CHECKTRAN_VAL", ".01"))
            tran = cn.BeginTransaction
            strSql = "SELECT BATCHNO ,SUM(ED)ED FROM " & cnStockDb & "..ISSUE "
            strSql += " WHERE ISNULL(ED,0)<>0 "
            strSql += " AND BATCHNO NOT IN(SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE='EXDUTY')"
            strSql += " GROUP BY BATCHNO ORDER BY BATCHNO"
            Dim dt As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim Batchno As String
                    Dim ED As Double
                    With dt.Rows(i)
                        Batchno = .Item("BATCHNO").ToString
                        ED = Val(.Item("ED").ToString)
                        strSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE='SA' AND BATCHNO='" & Batchno & "'"
                        strSql += " AND ACCODE IN(SELECT SALESID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE "
                        strSql += " IN(SELECT CATCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & Batchno & "' AND ISNULL(ED,0)<>0))"
                        strSql += " AND TRANMODE='C'"
                        Dim dtAcc As New DataTable
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtAcc)
                        If dtAcc.Rows.Count > 0 Then
                            InsertIntoAccTran(dtAcc.Rows(0).Item("TRANNO").ToString, "C", "EXDUTY", ED, 0, 0, 0, "SE", dtAcc.Rows(0).Item("CONTRA").ToString, _
                            dtAcc.Rows(0).Item("TRANDATE").ToString, dtAcc.Rows(0).Item("COSTID").ToString, _
                            , , , , , , , , dtAcc.Rows(0).Item("CASHID").ToString, dtAcc.Rows(0).Item("BATCHNO").ToString)

                            strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET ENTREFNO=AMOUNT WHERE  SNO='" & dtAcc.Rows(0).Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()

                            strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET AMOUNT=AMOUNT-" & ED & " WHERE  SNO='" & dtAcc.Rows(0).Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
                            If Math.Abs(balAmt) > minVal Then
                                tran.Rollback() : tran = Nothing
                                MsgBox("RollBacked...", MsgBoxStyle.Information)
                                Exit Sub
                            End If
                        End If
                    End With
                Next
            End If
            tran.Commit()
            tran = Nothing
            MsgBox("Updated...", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            MsgBox(ex.StackTrace & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer, _
    ByVal tranMode As String, _
    ByVal accode As String, _
    ByVal amount As Double, _
    ByVal pcs As Integer, _
    ByVal grsWT As Double, _
    ByVal netWT As Double, _
    ByVal payMode As String, _
    ByVal contra As String, _
    ByVal Billdate As String, _
    ByVal CostId As String, _
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
    Optional ByVal chqCardNo As String = Nothing, _
    Optional ByVal chqDate As String = Nothing, _
    Optional ByVal chqCardId As Integer = Nothing, _
    Optional ByVal chqCardRef As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing, _
    Optional ByVal CashCounterId As String = Nothing, _
Optional ByVal Batchno As String = Nothing)

        If amount = 0 Then Exit Sub
        If accode = "" Then Exit Sub



        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & Billdate & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & Mid(chqCardRef, 1, 50) & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & CashCounterId & "'" 'CASHID
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,'EDREVERSE'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        cmd = Nothing
        strSql = ""
    End Sub
End Class

Public Class frmTdsReport_Properties
    Private chkAsonDate As Boolean = True
    Public Property p_chkAsonDate() As Boolean
        Get
            Return chkAsonDate
        End Get
        Set(ByVal value As Boolean)
            chkAsonDate = value
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
    Private chkCmbacname As New List(Of String)
    Public Property p_chkCmbacname() As List(Of String)
        Get
            Return chkCmbacname
        End Get
        Set(ByVal value As List(Of String))
            chkCmbacname = value
        End Set
    End Property
    Private chktotal As Boolean = False
    Public Property p_chktotal() As Boolean
        Get
            Return chktotal
        End Get
        Set(ByVal value As Boolean)
            chktotal = False
        End Set
    End Property
End Class