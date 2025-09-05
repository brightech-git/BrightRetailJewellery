Imports System.Data.OleDb
Public Class frmAvgSalesReport
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
    Dim dtCompany As New DataTable
#Region "Common Function"
    Private Sub FuncCostCenterLoad()

        strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
    End Sub
    Private Sub FuncNew()
        With Me
            .lblTitle.Text = ""
            .dtpFrmDate.Value = GetServerDate()
            .dGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub Heading()
        til = Me.Text & " Date : " & Me.dtpFrmDate.Value.ToString("dd-MM-yyyy") & "'"
        til += " To " & Me.dtpToDate.Value.ToString("dd-MM-yyyy") & ""
        til += " At " & Me.chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
#End Region
    Private Sub frmAvgReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call FuncCostCenterLoad()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If dGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dGrid, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If dGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dGrid, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        Call FuncNew()
    End Sub
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        Dim CompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
        dGrid.DataSource = Nothing
        strsql = " SELECT DISTINCT TRANDATE,' ' + ' ' + COUNT(TRANNO) AS BILLNUMBER,SUM(GRSWT) AS GRSWT,"
        strsql += vbCrLf + " SUM(NETWT) AS NETWT,CONVERT(DECIMAL(10,3),COALESCE( SUM(ISNULL(GRSWT,0))/COUNT(TRANNO), 0) )"
        strsql += vbCrLf + " AS AVGGRSWT,CONVERT(DECIMAL(10,3),COALESCE( SUM(ISNULL(NETWT,0))/COUNT(TRANNO) , 0) )"
        strsql += vbCrLf + " AS AVGNETWT FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE TRANDATE BETWEEN  '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' "
        strsql += vbCrLf + " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE ='SA'"
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += " AND COSTID IN ('" & CHKCOSTID.Replace(",", "','") & "')"
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then strsql += " AND COMPANYID IN('" & CompanyId.Replace(",", "','") & "')"
        strsql += " GROUP BY TRANDATE ORDER BY TRANDATE"
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = ds.Tables(0)
            Call Heading()
            dGrid.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            dGrid.Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dGrid.Columns("BILLNUMBER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            dGrid.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("AvgGrswt").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("AvgGrswt").DefaultCellStyle.Format = "#,##0.000"
            dGrid.Columns("AvgNetwt").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("AvgNetwt").DefaultCellStyle.Format = "#,##0.000"
            dGrid.Columns("AvgNetwt").Visible = False
            dGrid.Columns("NETWT").Visible = False
            dGrid.Columns("BILLNUMBER").HeaderText = "NO.BILL"
            dGrid.Columns("GRSWT").HeaderText = "TOTAL GRSWT"
            dGrid.Columns("AvgGrswt").HeaderText = "AVG GRSWT"

            dGrid.Columns("TRANDATE").Width = 80
            dGrid.Columns("BILLNUMBER").Width = 80
            dGrid.Columns("GRSWT").Width = 100
            dGrid.Columns("AvgGrswt").Width = 100
        Else
            dGrid.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.")
        End If
    End Sub

    Private Sub frmAvgReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                btnview_Click(btnview, Nothing)
                Exit Select
            Case Keys.X
                btnExcel_Click(btnExcel, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If dGrid.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                dGrid.Invalidate()
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class