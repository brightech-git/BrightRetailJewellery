Imports System.Data.OleDb
Public Class frmDiscountReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        txttopDisc.Text = ""
        gridView.DataSource = Nothing
        gridView.Refresh()
        dtpFrom.Focus()
    End Sub

    Private Sub frmDiscountReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub frmDiscountReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtCostCentre As DataTable
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If
        LoadCompany(chkLstCompany)

        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("NONE")
        cmbOrderBy.Items.Add("DISC HIGH TO LOW")
        cmbOrderBy.Items.Add("DISC LOW TO HIGH")


        strSql = " SELECT 'ALL' METALNAME,1 RESULT UNION ALL SELECT METALNAME,2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += " ORDER BY METALNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtCostCentre, "METALNAME", , "ALL")
        chkSpecificFormat2.Checked = False
        chkSpecificFormat3.Checked = True
        grpSpecific2.Visible = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New Discount_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        obj.p_chkGroupbyBill = ChkGrpBillWise.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(Discount_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New Discount_Properties
        GetSettingsObj(obj, Me.Name, GetType(Discount_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        ChkGrpBillWise.Checked = obj.p_chkGroupbyBill
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        pnlHeading.Visible = False
        Dim SelectedCosts As String = ""
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCosts = GetSelectedCostId(chkCmbCostCentre, False)
        End If
        If chkSpecificFormat.Checked And chkSpecificFormat2.Checked Then
            MsgBox("Please select one Specific Format...", MsgBoxStyle.Information)
            chkSpecificFormat2.Checked = False
            grpSpecific2.Visible = False
            Exit Sub
        End If
        Dim MetalId As String = ""
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            MetalId = GetChecked_CheckedList(chkCmbMetal, False)
            Dim METAL() As String
            METAL = MetalId.Split(",")
            MetalId = ""
            For jj As Integer = 0 To METAL.Length - 1
                MetalId += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & METAL(jj).ToString & "'") + ","
            Next
            MetalId = Mid(MetalId, 1, Len(MetalId) - 1)
        End If

        If chkSpecificFormat2.Checked Then
            strSql = " EXEC " & cnStockDb & "..PROC_DISCOUNT_FORMAT2"
            strSql += vbCrLf + " @ADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TEMPTABLE='TEMP" & systemId & "DISCOUNT '"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
            strSql += vbCrLf + " ,@COSTID = '" & IIf(SelectedCosts = "", "ALL", SelectedCosts) & "'"
            strSql += vbCrLf + " ,@METALID = '" & IIf(MetalId = "", "ALL", MetalId) & "'"
            strSql += vbCrLf + " ,@GROUPBY = '" & IIf(ChkGrpBillWise.Checked, "Y", "N") & "'"
            If cmbOrderBy.Text.ToString.Trim = "DISC HIGH TO LOW" Then
                strSql += vbCrLf + " ,@DISCORDER='H'"
            ElseIf cmbOrderBy.Text.ToString.Trim = "DISC LOW TO HIGH" Then
                strSql += vbCrLf + " ,@DISCORDER='L'"
            Else
                strSql += vbCrLf + " ,@DISCORDER='N'"
            End If
            strSql += vbCrLf + " ,@TOP='" & Val(txttopDisc.Text.ToString & "") & "'"
            strSql += vbCrLf + " ,@WITHEX = '" & IIf(chkSpecificFormat.Checked, "Y", "N") & "'"
        ElseIf chkSpecificFormat.Checked Then
            strSql = " EXEC " & cnStockDb & "..PROC_DISCOUNT"
            strSql += vbCrLf + " @ADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TEMPTABLE='TEMP" & systemId & "DISCOUNT '"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
            strSql += vbCrLf + " ,@COSTID = '" & IIf(SelectedCosts = "", "ALL", SelectedCosts) & "'"
            strSql += vbCrLf + " ,@METALID = '" & IIf(MetalId = "", "ALL", MetalId) & "'"
            strSql += vbCrLf + " ,@GROUPBY = '" & IIf(ChkGrpBillWise.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHEX = '" & IIf(chkSpecificFormat.Checked, "Y", "N") & "'"
        Else
            strSql = " EXEC " & cnStockDb & "..PROC_DISCOUNT_FORMAT3"
            strSql += vbCrLf + " @ADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TEMPTABLE='TEMP" & systemId & "DISCOUNT '"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
            strSql += vbCrLf + " ,@COSTID = '" & IIf(SelectedCosts = "", "ALL", SelectedCosts) & "'"
            strSql += vbCrLf + " ,@METALID = '" & IIf(MetalId = "", "ALL", MetalId) & "'"
            strSql += vbCrLf + " ,@GROUPBY = '" & IIf(ChkGrpBillWise.Checked, "Y", "N") & "'"            
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        'da = New OleDbDataAdapter(strSql, cn)
        Dim ds As New DataSet
        Dim dtGrid As New DataTable
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        gridview.DataSource = Nothing
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnNew.Focus()
            Exit Sub
        End If
        Dim str As String = "DISCOUNT REPORT FROM " & Format(dtpFrom.Value, "dd-MM-yyyy") & " TO " & Format(dtpTo.Value, "dd-MM-yyyy")
        lblTitle.Text = str
        pnlHeading.Visible = True
        gridview.DataSource = dtGrid
        For Each dgvRow As DataGridViewRow In gridview.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportSubTotalStyle.BackColor
                    Case "H"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("BILLNO").Style.BackColor = Color.LightGreen
                        .Cells("BILLNO").Style.ForeColor = Color.DarkRed
                End Select
            End With
        Next
        With gridview
            funcGridViewStyle()
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Select()
        End With
        Prop_Sets()
    End Sub
 
    Public Function funcGridViewStyle() As Integer
        With gridview
            For cnt As Integer = 0 To gridview.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            If .Columns.Contains("BILLNO") Then
                With .Columns("BILLNO")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("BILLDATE") Then
                With .Columns("BILLDATE")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                End With
            End If
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If .Columns.Contains("METAL") Then
                With .Columns("METAL")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("GRSWT") Then
                With .Columns("GRSWT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("NETWT") Then
                With .Columns("NETWT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("DISCOUNT") Then
                With .Columns("DISCOUNT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("TOTALDISC") Then
                With .Columns("TOTALDISC")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("FINALDISC") Then
                With .Columns("FINALDISC")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("OFFERDISC") Then
                With .Columns("OFFERDISC")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("DISCPER") Then
                With .Columns("DISCPER")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("TAGVALUE") Then
                With .Columns("TAGVALUE")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("PCS") Then
                With .Columns("PCS")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If chkSpecificFormat.Checked Then
                If .Columns.Contains("DISCLIMITAMT") Then
                    With .Columns("DISCLIMITAMT")
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
                If .Columns.Contains("DISCALLOWAMT") Then
                    With .Columns("DISCALLOWAMT")
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
            End If
        End With

        gridview.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        End If
    End Sub


    Private Sub chkSpecificFormat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSpecificFormat.CheckedChanged
        If chkSpecificFormat.Checked = True Then
            ChkGrpBillWise.Enabled = False
            chkSpecificFormat3.Checked = False
            chkSpecificFormat2.Checked = False
        Else
            ChkGrpBillWise.Enabled = True
        End If
    End Sub

    Private Sub ChkGrpBillWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkGrpBillWise.CheckedChanged
        If ChkGrpBillWise.Checked = True Then
            chkSpecificFormat.Enabled = False
        Else
            chkSpecificFormat.Enabled = True
        End If
    End Sub

    Private Sub chkSpecificFormat2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSpecificFormat2.CheckedChanged
        If chkSpecificFormat2.Checked Then
            grpSpecific2.Visible = True
            chkSpecificFormat3.Checked = False
            chkSpecificFormat.Checked = False
        Else
            grpSpecific2.Visible = False
        End If
    End Sub

    Private Sub cmbOrderBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrderBy.SelectedIndexChanged

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub chkSpecificFormat3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSpecificFormat3.CheckedChanged
        If chkSpecificFormat3.Checked = True Then
            chkSpecificFormat.Checked = False
            chkSpecificFormat2.Checked = False
        End If
    End Sub
End Class

Public Class Discount_Properties
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
    Private chkGroupbyBill As Boolean = False
    Public Property p_chkGroupbyBill() As Boolean
        Get
            Return chkGroupbyBill
        End Get
        Set(ByVal value As Boolean)
            chkGroupbyBill = value
        End Set
    End Property
End Class