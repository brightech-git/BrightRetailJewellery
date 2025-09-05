Imports System.Data.OleDb
Public Class frmSalesRegisterBillWise
#Region "Variable"
    Dim dt As DataTable
    Dim Strsql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim PrintTitle As String
    Dim dtCostCentre As New DataTable
#End Region
#Region "Form Load"
    Private Sub frmSalesRegisterBillWise_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesRegisterBillWise_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
#End Region
#Region "Button Events"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Sales Register ", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        Strsql += " UNION ALL"
        Strsql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        Strsql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcompany = New DataTable
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtcompany, "COMPANYNAME", , "ALL")
        Strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        Strsql += " UNION ALL"
        Strsql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        Strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        Strsql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        Strsql += vbCrLf + " UNION ALL"
        Strsql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        Strsql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(Strsql, cn)
        Dim dtMetal As New DataTable()
        da.Fill(dtMetal)
        If dtMetal.Rows.Count > 0 Then
            cmbMetal.Items.Clear()
            For i As Integer = 0 To dtMetal.Rows.Count - 1
                cmbMetal.Items.Add(dtMetal.Rows(i).Item("METALNAME").ToString)
            Next
            cmbMetal.Text = "ALL"
        End If

        Strsql = vbCrLf + " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
        Strsql += vbCrLf + " UNION ALL"
        Strsql += vbCrLf + " SELECT CATNAME,CONVERT(VARCHAR,CATCODE),2 RESULT FROM " & cnAdminDb & "..CATEGORY"
        Strsql += vbCrLf + " ORDER BY RESULT,CATNAME"
        da = New OleDbDataAdapter(Strsql, cn)
        Dim dtcat As New DataTable()
        da.Fill(dtcat)
        If dtcat.Rows.Count > 0 Then
            cmbCategory.Items.Clear()
            For i As Integer = 0 To dtcat.Rows.Count - 1
                cmbCategory.Items.Add(dtcat.Rows(i).Item("CATNAME").ToString)
            Next
            cmbCategory.Text = "ALL"
        End If


        gridView.DataSource = Nothing
        txtCustomer.Text = ""
        dtpFrom.Focus()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridView.DataSource = Nothing
        Dim CompName As String = "ALL"
        Dim CompId As String = "ALL"
        Dim Costid As String = "ALL"
        Dim MetalId As String = "ALL"
        Dim catCode As String = "ALL"
        Dim Dealer As String = "ALL"

        If chkcmbCompany.Text <> "ALL" And chkcmbCompany.Text <> "" Then
            Strsql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkcmbCompany.Text) & ")"
            CompId = GetSqlValue(cn, Strsql)
        End If

        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Strsql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ")"
            Costid = GetSqlValue(cn, Strsql)
        End If


        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            Strsql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN ('" & cmbMetal.Text & "')"
            MetalId = GetSqlValue(cn, Strsql)
        End If

        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            Strsql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN ('" & cmbCategory.Text & "')"
            catCode = GetSqlValue(cn, Strsql)
        End If

        If txtCustomer.Text <> "" Then
            Dealer = txtCustomer.Text
        End If

        Strsql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_SALESREGISTER"
        Strsql += vbCrLf + "@ADMINDB = '" & cnAdminDb & "'"
        Strsql += vbCrLf + ",@DBNAME = '" & cnStockDb & "'"
        Strsql += vbCrLf + ",@FRMDATE = '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + ",@TODATE = '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + ",@COSTID = '" & IIf(Costid <> "ALL", Costid, "ALL") & "'"
        Strsql += vbCrLf + ",@METALID = '" & IIf(MetalId <> "ALL", MetalId, "ALL") & "'"
        Strsql += vbCrLf + ",@CATCODE = '" & IIf(catCode <> "ALL", catCode, "ALL") & "'"
        Strsql += vbCrLf + ",@DEALER = '" & IIf(Dealer <> "ALL", Dealer, "ALL") & "'"
        Strsql += vbCrLf + ",@COMPANYID = '" & IIf(CompId <> "ALL", CompId, "ALL") & "'"
        Strsql += vbCrLf + ",@UID = '" & userId & "'"
        If rbtBillNo.Checked = True Then
            Strsql += vbCrLf + ",@DETAIL = 'Y'"
        End If
        If rbtCategoryWise.Checked = True Then
            Strsql += vbCrLf + ",@DETAIL = 'N'"
        End If
        Strsql += vbCrLf + ",@RETURN = 'N'"
        cmd = New OleDbCommand(Strsql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim ds As New DataSet
        Dim dtGrid As New DataTable
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        gridView.Columns("RESULT").Visible = False
        FormatGridColumns(gridView, False, False, True, False)
        For i As Integer = 0 To gridView.RowCount - 1
            If gridView.Rows(i).Cells("RESULT").Value = "2" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
            End If
        Next
        With gridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Focus()
        End With
    End Sub
#End Region
#Region "User Define Function "
    Private Sub AutoResizeToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            AutoResizeToolStripMenuItem.Checked = True
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
            AutoResizeToolStripMenuItem.Checked = False
        End If
    End Sub
#End Region
End Class