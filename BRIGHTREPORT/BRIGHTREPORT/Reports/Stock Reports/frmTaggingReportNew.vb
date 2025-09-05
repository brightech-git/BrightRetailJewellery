Imports System.Data.OleDb
Public Class frmTaggingReportNew
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
#Region "Common Function"
    Private Sub FuncCostCenterLoad()
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
            .dtpToDate.Value = GetServerDate()
            .DGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub Heading()
        til = Me.Text & " Date : " & Me.dtpFrmDate.Value.ToString("dd-MM-yyyy")
        til += " To " & Me.dtpToDate.Value.ToString("dd-MM-yyyy")
        til += "At " & Me.chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
#End Region
    Private Sub frmTaggingReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FuncCostCenterLoad()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
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
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub frmTaggingReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        dGrid.DataSource = Nothing
        strsql = "SELECT IL.LOTDATE,ISNULL(IL.LOTNO,0) AS LOTNO ,ISNULL(IL.PCS,0) AS LOTPCS"
        strsql += ",ISNULL(IL.GRSWT,0) AS LOTWT,ISNULL(IL.CPCS,0) AS TAGPCS,ISNULL(IL.CGRSWT,0) AS TAGWT"
        strsql += ",(ISNULL(IL.PCS,0) - ISNULL(IL.CPCS,0))AS DIFFPCS"
        strsql += ",(ISNULL(IL.GRSWT,0) - ISNULL(IL.CGRSWT,0))AS DIFFWEIGHT "
        strsql += ",TRANINVNO,D.DESIGNERNAME "
        strsql += "FROM " & cnAdminDb & "..ITEMLOT AS IL "
        strsql += "inner join " & cnAdminDb & "..DESIGNER D ON IL.DESIGNERID = D.DESIGNERID "
        strsql += " WHERE IL.LOTDATE BETWEEN  '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' "
        strsql += " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += " AND IL.COSTID ='" & CHKCOSTID & "'"
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = ds.Tables(0)
            Heading()
            dGrid.Columns("LOTDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            'dGrid.Columns("LOTPCS").Visible = False
            'dGrid.Columns("TAGPCS").Visible = False
            'dGrid.Columns("DIFFPCS").Visible = False
            'dGrid.Columns("DIFFPCS").Visible = False
            dGrid.Columns("LOTNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            dGrid.Columns("LOTPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("TAGPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("LOTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("TAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("DIFFWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("LOTDATE").Width = 80
            dGrid.Columns("LOTNO").Width = 60

            dGrid.Columns("LOTPCS").Width = 60
            dGrid.Columns("TAGPCS").Width = 60
            dGrid.Columns("LOTWT").Width = 60
            dGrid.Columns("TAGWT").Width = 60
            dGrid.Columns("DIFFPCS").Width = 60
            dGrid.Columns("DIFFWEIGHT").Width = 60
            dGrid.Columns("DIFFWEIGHT").HeaderText = "DIFFWT"
            dGrid.Columns("DESIGNERNAME").Width = 150
        Else
            dGrid.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.")
        End If
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