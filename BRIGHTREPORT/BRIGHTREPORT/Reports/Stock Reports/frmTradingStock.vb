Imports System.Data
Imports System.Data.OleDb
Public Class frmTradingStock
#Region "VARIABLE"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim dtCompany As DataTable
    Dim dtMetal As DataTable
    Dim dtCostCentre As DataTable
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblPrint.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblPrint.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        cmbMetalType.Items.Clear()
        strsql = " Select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        cmbMetalType.Items.Add("ALL")
        objGPack.FillCombo(strsql, cmbMetalType, False)
        cmbMetalType.Text = "ALL"

        'cmbCatName.Items.Clear()
        'strsql = " Select CATNAME from " & cnAdminDb & "..CATEGORY order by CATNAME"
        'cmbCatName.Items.Add("ALL")
        'objGPack.FillCombo(strsql, cmbCatName, False)
        'cmbCatName.Text = "ALL"

        lblPrint.Text = "..."
        gridView.DataSource = Nothing
        dtpFrom.Focus()
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridView.DataSource = Nothing
        Dim metalid As String = "ALL"
        Dim catcode As String = "ALL"
        If cmbMetalType.Text <> "ALL" Then
            strsql = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME  = '" & cmbMetalType.Text & "'"
            metalid = GetSqlValue(cn, strsql).ToString
        End If
        lblPrint.Text = "..."
        'If cmbCatName.Text <> "ALL" Then
        '    strsql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCatName.Text & "'"
        '    catcode = GetSqlValue(cn, strsql).ToString
        'End If
        dt = New DataTable
        Dim dsGridView As New DataSet
        strsql = vbCrLf + " EXEC " & cnAdminDb & "..[TRADINGSTOCKPURCHASE] "
        strsql += vbCrLf + "@FROMDATE = '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + ",@TODATE = '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + ",@COMPANYID = '" & GetQryString(chkCmbCompany.Text).Replace("'", "") & "'"
        strsql += vbCrLf + ",@DBNAME = '" & cnStockDb & "'"
        strsql += vbCrLf + ",@ADMINDB = '" & cnAdminDb & "'"
        strsql += vbCrLf + ",@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strsql += vbCrLf + ",@METALID = '" & metalid & "'"
        strsql += vbCrLf + ",@CATCODE = '" & catcode & "'"
        strsql += vbCrLf + ",@TEMPTABLEDB = 'TEMPTABLEDB..TEMPTRANDINGSTOCK'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.CommandTimeout = 2000
        'cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dt = dsGridView.Tables(0)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridView, False, False, , False)
                AutoResizeToolStripMenuItem_Click(Me, New System.EventArgs)
                lblPrint.Text = "TRADING STOCK PURCHASE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & " " & cmbMetalType.Text
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
    End Sub
#End Region
#Region "combox Events"
    Private Sub cmbMetalType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetalType.SelectedIndexChanged
        'If cmbMetalType.Text <> "ALL" And cmbMetalType.Text <> "" Then
        '    Dim MetalId As String = ""
        '    strsql = " SELECT METALID FROM " & cnAdminDb & "..metalMast WHERE METALNAME = '" & cmbMetalType.Text & "'"
        '    MetalId = GetSqlValue(cn, strsql)
        '    cmbCatName.Items.Clear()
        '    strsql = " Select CATNAME from " & cnAdminDb & "..CATEGORY where metalid = '" & MetalId & "' order by CATNAME"
        '    objGPack.FillCombo(strsql, cmbCatName, False)
        'Else
        '    cmbCatName.Items.Clear()
        '    strsql = " Select CATNAME from " & cnAdminDb & "..CATEGORY order by CATNAME"
        '    cmbCatName.Items.Add("ALL")
        '    objGPack.FillCombo(strsql, cmbCatName, False)
        '    cmbCatName.Text = "ALL"
        'End If
    End Sub
#End Region
#Region "FORM LOAD"
    Private Sub frmTradingStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmTradingStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
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
        End If
    End Sub
#End Region
End Class