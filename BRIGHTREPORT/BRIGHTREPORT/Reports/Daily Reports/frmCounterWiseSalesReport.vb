#Region "Header Files"

Imports System.Data.OleDb
Imports System.Threading
Imports System.IO

#End Region

Public Class frmCounterWiseSalesReport

#Region "Variable Declaration"

    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
    Dim dtCompany As New DataTable

#End Region

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
            .dGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
        Me.lblTitle.Text = ""
        dGrid.DataSource = Nothing
        dtpFrmDate.Focus()
    End Sub
    Private Sub Heading()
        til = Me.Text & " Date : " & Me.dtpFrmDate.Value.ToString("dd-MM-yyyy")
        til += " To " & Me.dtpToDate.Value.ToString("dd-MM-yyyy") & ""
        til += " At " & Me.chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
#End Region
#Region "Form Event and Button Event"
    Private Sub frmCounterWiseReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                Call btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                Call btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                Call btnView_Search_Click(btnView_Search, Nothing)
                Exit Select
            Case Keys.X
                Call btnExcel_Click(btnExcel, Nothing)
                Exit Select
            Case Keys.P
                Call btnPrint_Click(btnPrint, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub frmCounterWiseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strsql = "SELECT 'ALL'ITEMCTRNAME,1 RESULT "
        strsql += " UNION ALL"
        strsql += " SELECT ITEMCTRNAME,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER  WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += " ORDER BY RESULT,ITEMCTRNAME"
        dtOptions = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtOptions)
        BrighttechPack.GlobalMethods.FillCombo(cmbCounterName, dtOptions, "ITEMCTRNAME", True)
        cmbCounterName.Text = "ALL"
        Call FuncNew()
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
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        Call FuncNew()
    End Sub
#End Region
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

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        Dim CompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
        dGrid.DataSource = Nothing
        strsql = " SELECT '   ' +CONVERT(VARCHAR(50),I.TRANDATE,103) AS PARTICULAR,I.TRANDATE,IT.ITEMCTRNAME,COUNT(I.TRANNO) AS BILLNUMBER"
        strsql += vbCrLf + " ,CONVERT(DECIMAL(10,3),COALESCE( SUM(ISNULL(I.GRSWT,0))/COUNT(I.TRANNO), 0)) AS AVGGRSWT,1 RESULT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IT  ON I.ITEMCTRID =IT.ITEMCTRID "
        strsql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "' "
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then strsql += vbCrLf + " AND IT.ITEMCTRNAME = '" & cmbCounterName.Text & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += vbCrLf + " AND I.COSTID IN ('" & CHKCOSTID.Replace(",", "','") & "')"
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then strsql += vbCrLf + " AND I.COMPANYID IN ('" & CompanyId.Replace(",", "','") & "')"
        strsql += vbCrLf + " GROUP BY IT.ITEMCTRNAME,I.TRANDATE"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT DISTINCT IT.ITEMCTRNAME AS PARTICULAR,NULL TRANDATE,IT.ITEMCTRNAME,NULL AS BILLNUMBER"
        strsql += vbCrLf + " ,NULL AS AVGGRSWT,0 RESULT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IT  ON I.ITEMCTRID =IT.ITEMCTRID "
        strsql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "' "
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then strsql += vbCrLf + " AND IT.ITEMCTRNAME = '" & cmbCounterName.Text & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += vbCrLf + " AND I.COSTID IN ('" & CHKCOSTID.Replace(",", "','") & "')"
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then strsql += vbCrLf + " AND I.COMPANYID IN ('" & CompanyId.Replace(",", "','") & "')"
        strsql += vbCrLf + " ORDER BY IT.ITEMCTRNAME,RESULT,TRANDATE"
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = ds.Tables(0)
            Call Heading()
            dGrid.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            dGrid.Columns("ITEMCTRNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            dGrid.Columns("BILLNUMBER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("AVGGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("ITEMCTRNAME").Visible = False
            dGrid.Columns("TRANDATE").Visible = False
            dGrid.Columns("RESULT").Visible = False
            dGrid.Columns("ITEMCTRNAME").HeaderText = "COUNTER"
            dGrid.Columns("BILLNUMBER").HeaderText = "NO OF BILLS"
            dGrid.Columns("AVGGRSWT").HeaderText = "AVG GRSWT"
            dGrid.Columns("TRANDATE").Width = 80
            dGrid.Columns("ITEMCTRNAME").Width = 100
            dGrid.Columns("BILLNUMBER").Width = 80
            dGrid.Columns("AVGGRSWT").Width = 80
        Else
            dGrid.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.", MsgBoxStyle.Information, "Information")
        End If
    End Sub
End Class