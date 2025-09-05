Imports System.Data.OleDb
Imports System.Data
Public Class MeterialIssue
    Dim strSql As String = Nothing
    Private Sub MeterialIssue_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NewLoad()
    End Sub
    Private Sub NewLoad()
        Dim Dt As DataTable = New DataTable()
        Dt.Columns.Add("Name")
        Dt.Rows.Add("Metal")
        Dt.Rows.Add("Category")
        Dt.Rows.Add("StoneGroup")
        Dt.Rows.Add("Item")
        GiritechPack.FillCombo(chkCmbGropBy, Dt, "NAME", , "StoneGroup")

        strSql = "SELECT '0' ID,'ALL' NAME UNION "
        strSql += "SELECT GROUPID ID,GROUPNAME NAME FROM " + cnAdminDb + "..STONEGROUP WHERE ISNULL(ACTIVE,'Y') <> 'N' ORDER BY ID,NAME"
        GiritechPack.FillCombo(chkCmbStoneGroup, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbStoneGroup.DisplayMember = "NAME"
        chkCmbStoneGroup.ValueMember = "ID"
        If chkCmbStoneGroup.Items.Count = 0 Then
            chkCmbStoneGroup.Enabled = False
        End If
        CompanyLoad()
        CostCentreLoad()
        MetalLoad()
        CategoryLoad()
        ItemLoad()
        SubItemLoad()
    End Sub

    Private Sub CompanyLoad()
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT COMPANYID ID,COMPANYNAME NAME FROM " + cnAdminDb + "..COMPANY "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbCompany, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbCompany.DisplayMember = "NAME"
        chkCmbCompany.ValueMember = "ID"
        'chkCmbCompany.Enabled = chkCmbCompany.Items.Count < 1
    End Sub
    Private Sub CostCentreLoad(Optional Value As String = Nothing)
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT COSTID ID,COSTNAME NAME FROM " + cnAdminDb + "..COSTCENTRE "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        If Value <> Nothing Then
            strSql += vbCrLf + "AND COMPANYID  IN (SELECT COMPANYID FROM " + cnAdminDb + "..COMPANY WHERE COMPANYNAME IN ('" + Value.Replace(",", "','") + "'))"
        End If
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbCostCentre, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbCostCentre.DisplayMember = "NAME"
        chkCmbCostCentre.ValueMember = "ID"
        'chkCmbCostCentre.Enabled = chkCmbCostCentre.Items.Count < 1
    End Sub
    Private Sub MetalLoad()
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT METALID ID,METALNAME NAME FROM " + cnAdminDb + "..METALMAST "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbMetel, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbMetel.DisplayMember = "NAME"
        chkCmbMetel.ValueMember = "ID"
        'chkCmbMetel.Enabled = chkCmbMetel.Items.Count < 1
    End Sub
    Private Sub CategoryLoad(Optional Value As String = Nothing)
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT CATCODE ID,CATNAME NAME FROM " + cnAdminDb + "..CATEGORY "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        If Value <> Nothing Then
            strSql += vbCrLf + "AND METALID IN (SELECT METALID FROM " + cnAdminDb + "..METALMAST WHERE METALNAME IN ('" + Value.Replace(",", "','") + "'))"
        End If
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbCategory, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbCategory.DisplayMember = "NAME"
        chkCmbCategory.ValueMember = "ID"
        'chkCmbCategory.Enabled = chkCmbCategory.Items.Count < 1
    End Sub
    Private Sub ItemLoad(Optional Value As String = Nothing)
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT ITEMID ID,ITEMNAME NAME FROM " + cnAdminDb + "..ITEMMAST "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        If Value <> Nothing Then
            strSql += vbCrLf + "AND CATCODE IN (SELECT CATCODE FROM " + cnAdminDb + "..CATEGORY WHERE CATNAME IN ('" + Value.Replace(",", "','") + "'))"
        End If
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbItem, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbItem.DisplayMember = "NAME"
        chkCmbItem.ValueMember = "ID"
        'chkCmbItem.Enabled = chkCmbItem.Items.Count < 1
    End Sub
    Private Sub SubItemLoad(Optional Value As String = Nothing)
        strSql = ""
        strSql += vbCrLf + "SELECT '0' ID,'ALL' NAME "
        strSql += vbCrLf + "UNION "
        strSql += vbCrLf + "SELECT SUBITEMID ID,SUBITEMNAME NAME FROM " + cnAdminDb + "..SUBITEMMAST "
        strSql += vbCrLf + "WHERE ISNULL(ACTIVE,'Y') <> 'N' "
        If Value <> Nothing Then
            strSql += vbCrLf + "AND ITEMID IN (SELECT ITEMID FROM " + cnAdminDb + "..ITEMMAST WHERE ITEMNAME IN ('" + Value.Replace(",", "','") + "'))"
        End If
        strSql += vbCrLf + "ORDER BY ID,NAME "
        GiritechPack.FillCombo(chkCmbSubItem, GetSqlTable(strSql, cn), "NAME", , "ALL")
        chkCmbSubItem.DisplayMember = "NAME"
        chkCmbSubItem.ValueMember = "ID"
        'chkCmbSubItem.Enabled = chkCmbSubItem.Items.Count < 1
    End Sub
    Private Sub chkCmbCompany_TextChanged(sender As Object, e As EventArgs) Handles chkCmbCompany.TextChanged
        CostCentreLoad(chkCmbCompany.Text)
    End Sub

    Private Sub chkCmbMetel_TextChanged(sender As Object, e As EventArgs) Handles chkCmbMetel.TextChanged
        CategoryLoad(chkCmbMetel.Text)
    End Sub

    Private Sub chkCmbCategory_TextChanged(sender As Object, e As EventArgs) Handles chkCmbCategory.TextChanged
        ItemLoad(chkCmbCategory.Text)
    End Sub

    Private Sub chkCmbItem_TextChanged(sender As Object, e As EventArgs) Handles chkCmbItem.TextChanged
        SubItemLoad(chkCmbItem.Text)
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        NewLoad()
    End Sub

    Private Sub btnView_Search_Click(sender As Object, e As EventArgs) Handles btnView_Search.Click
        strSql = Nothing
        strSql += vbCrLf + "  EXEC " + cnAdminDb + "..SP_RPT_METERIALISSUE"
        strSql += vbCrLf + "  @DBNAME = '" + cnStockDb + "'"
        strSql += vbCrLf + ", @FROMDATE = '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "'"
        strSql += vbCrLf + ", @TODATE = '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
        strSql += vbCrLf + ", @GROUPBY = '" + chkCmbGropBy.Text.Trim + "'"
        strSql += vbCrLf + ", @COMPANY = '" + chkCmbCompany.Text.Trim + "'"
        strSql += vbCrLf + ", @COSTCENTRE = '" + chkCmbCostCentre.Text.Trim + "'"
        strSql += vbCrLf + ", @METAL = '" + chkCmbMetel.Text.Trim + "'"
        strSql += vbCrLf + ", @CATEGORY = '" + chkCmbCategory.Text.Trim + "'"
        strSql += vbCrLf + ", @ITEM = '" + chkCmbItem.Text.Trim + "'"
        strSql += vbCrLf + ", @SUBITEM = '" + chkCmbSubItem.Text.Trim + "'"
        Dim dtGrid As DataTable = New DataTable
        Dim cmd As New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + "SELECT KEYNO,PARTICULAR"
        strSql += vbCrLf + ",O_PCS,O_GRSWT,O_NETWT,O_PUREWT,O_AMOUNT,O_TAX,O_TOTAL"
        strSql += vbCrLf + ",I_PCS,I_GRSWT,I_NETWT,I_PUREWT,I_AMOUNT,I_TAX,I_TOTAL"
        strSql += vbCrLf + ",R_PCS,R_GRSWT,R_NETWT,R_PUREWT,R_AMOUNT,R_TAX,R_TOTAL"
        strSql += vbCrLf + ",C_PCS,C_GRSWT,C_NETWT,C_PUREWT,C_AMOUNT,C_TAX,C_TOTAL"
        strSql += vbCrLf + ",COLHEAD FROM TEMPTABLEDB..TEMP_MI_FINAL ORDER BY KEYNO"
        dtGrid = GetSqlTable(strSql, cn)

        Dim objGridShower As frmGridDispDia = New frmGridDispDia
        objGridShower.Name = Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "Meterial Issue Report "
        objGridShower.lblTitle.Text = "Meterial Issue Report "
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.Show()
        objGridShower.WindowState = FormWindowState.Maximized

        Dim ind As String = "PARTICULAR"
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString = "H" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.ForeColor = Color.Red
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T1" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T2" Then
                dgvRow.Cells(ind).Style.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T3" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T4" Then
                dgvRow.Cells(ind).Style.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S1" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S2" Then
                dgvRow.DefaultCellStyle.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S4" Then
                dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "G" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        objGridShower.gridView.Columns("O_AMOUNT").Visible = False
        objGridShower.gridView.Columns("I_AMOUNT").Visible = False
        objGridShower.gridView.Columns("R_AMOUNT").Visible = False
        objGridShower.gridView.Columns("C_AMOUNT").Visible = False
        objGridShower.gridView.Columns("O_TAX").Visible = False
        objGridShower.gridView.Columns("I_TAX").Visible = False
        objGridShower.gridView.Columns("R_TAX").Visible = False
        objGridShower.gridView.Columns("C_TAX").Visible = False
        objGridShower.gridView.Columns("O_TOTAL").Visible = False
        objGridShower.gridView.Columns("I_TOTAL").Visible = False
        objGridShower.gridView.Columns("R_TOTAL").Visible = False
        objGridShower.gridView.Columns("C_TOTAL").Visible = False

        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        objGridShower.gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In objGridShower.gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

        Dim DtHead As DataTable = New DataTable
        DtHead.Columns.Add("PARTICULAR")
        DtHead.Columns.Add("OPENING")
        DtHead.Columns.Add("ISSUE")
        DtHead.Columns.Add("RECEIPT")
        DtHead.Columns.Add("CLOSING")
        DtHead.Columns.Add("SCROLL")
        objGridShower.gridViewHeader.DataSource = Nothing
        objGridShower.gridViewHeader.DataSource = DtHead
        objGridShower.gridViewHeader.Visible = True

        objGridShower.gridViewHeader.ColumnHeadersBorderStyle = objGridShower.gridView.ColumnHeadersBorderStyle
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        objGridShower.gridViewHeader.ColumnHeadersHeight = objGridShower.gridView.ColumnHeadersHeight
        objGridShower.gridViewHeader.Height = 20
        objGridShower.gridViewHeader.ColumnHeadersHeightSizeMode = objGridShower.gridView.ColumnHeadersHeightSizeMode

        objGridShower.gridViewHeader.Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width
        objGridShower.gridViewHeader.Columns("OPENING").Width = 100
        objGridShower.gridViewHeader.Columns("ISSUE").Width = 100
        objGridShower.gridViewHeader.Columns("RECEIPT").Width = 100
        objGridShower.gridViewHeader.Columns("CLOSING").Width = 100
        objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
        objGridShower.cmbGridShortCut.Enabled = False
        For Each dgvRow As DataGridViewColumn In objGridShower.gridView.Columns
            If dgvRow.Name.Contains("O_") And dgvRow.Visible Then
                dgvRow.HeaderText = dgvRow.Name.Replace("O_", "")
                objGridShower.gridViewHeader.Columns("OPENING").Width += dgvRow.Width
            End If
            If dgvRow.Name.Contains("I_") And dgvRow.Visible Then
                dgvRow.HeaderText = dgvRow.Name.Replace("I_", "")
                objGridShower.gridViewHeader.Columns("ISSUE").Width += dgvRow.Width
            End If
            If dgvRow.Name.Contains("R_") And dgvRow.Visible Then
                dgvRow.HeaderText = dgvRow.Name.Replace("R_", "")
                objGridShower.gridViewHeader.Columns("RECEIPT").Width += dgvRow.Width
            End If
            If dgvRow.Name.Contains("C_") And dgvRow.Visible Then
                dgvRow.HeaderText = dgvRow.Name.Replace("C_", "")
                objGridShower.gridViewHeader.Columns("CLOSING").Width += dgvRow.Width
            End If
        Next
        objGridShower.gridViewHeader.Columns("OPENING").Width -= 100
        objGridShower.gridViewHeader.Columns("ISSUE").Width -= 100
        objGridShower.gridViewHeader.Columns("RECEIPT").Width -= 100
        objGridShower.gridViewHeader.Columns("CLOSING").Width -= 100

    End Sub
End Class