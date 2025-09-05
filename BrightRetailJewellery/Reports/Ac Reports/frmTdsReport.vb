Imports System.Data.OleDb

Public Class frmTdsReport
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
        Dim mtemptable As String = "TEMPTABLEDB..TEMP" & Trim(Guid.NewGuid.ToString.Substring(0, 5))
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkaccode As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..ACHEAD", "ACCODE", "ACNAME", True)

        strSql = " EXECUTE " & cnAdminDb & "..AGRN_SP_TDSREPORT"
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
        strSql += vbCrLf + ",@COSTIDS  =""" & IIf(chkCostId = "", "ALL", chkCostId) & """"
        strSql += vbCrLf + ",@ACCODE  =""" & IIf(chkaccode = "", "ALL", chkaccode) & """"

        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        dsGridView = New DataSet
        da.Fill(dsGridView)
        dtGrid = dsGridView.Tables(0)
        If dtGrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtGrid
            'gridView.Columns("SNO").Visible = False
            gridView.Columns("SNO").Width = 50
            gridView.Columns("PARTICULAR").Width = 250
            gridView.Columns("TDSNAME").Visible = False
            gridView.Columns("ACNAME").Visible = False
            gridView.Columns("CODE").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("TRANNO").Visible = False
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("TDSAMOUNT").DefaultCellStyle.Format = "0.00"
            gridView.Columns("TDSAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("TDSPER").DefaultCellStyle.Format = "0.00"
            gridView.Columns("TDSPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            lblTitle.Text = "TDS REPORT"
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
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTIVE='Y'"
        strSql += " ORDER BY RESULT,ACNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbacname_OWN, dtacname, "ACNAME", , "ALL")
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
        GetChecked_CheckedList(chkcmbacname_OWN, obj.p_chkCmbacname)
        obj.p_chktotal = chktotal.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTdsReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTdsReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTdsReport_Properties))
        chkAsonDate.Checked = obj.p_chkAsonDate
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkcmbacname_OWN, obj.p_chkCmbacname, "ALL")
        chktotal.Checked = obj.p_chktotal
    End Sub

    Private Sub btnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerify.Click
        Dim Dv As New DataView
        Dim Dt As New DataTable
        Dim DtVerify As New DataTable()
        DtVerify.Columns.Add(New DataColumn("TRANNO", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("TRANDATE", Type.GetType("System.String")))
        DtVerify.Columns.Add(New DataColumn("ACNAME", Type.GetType("System.String")))
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
                TdsPer = .Item("TDSPER").ToString
                TdsAmt = .Item("TDSAMOUNT").ToString
                If TdsAmt <> ((Amt * TdsPer) / 100) Then
                    Dim dr As DataRow
                    dr = DtVerify.NewRow()
                    dr("TRANNO") = .Item("TRANNO").ToString
                    dr("TRANDATE") = .Item("TRANDATE").ToString
                    dr("ACNAME") = .Item("ACNAME").ToString
                    dr("AMOUNT") = .Item("AMOUNT").ToString
                    dr("TDSPER") = .Item("TDSPER").ToString
                    dr("TDSAMOUNT") = .Item("TDSAMOUNT").ToString
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