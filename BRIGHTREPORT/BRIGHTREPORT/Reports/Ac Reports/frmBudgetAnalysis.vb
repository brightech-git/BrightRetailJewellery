Imports System.Data.OleDb
Public Class frmBudgetAnalysis
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtAcName As New DataTable
    Public dtFilteration As New DataTable
    Public FormReSize As Boolean = True
    Public FormReLocation As Boolean = True

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        fnew()
    End Sub
    Function fnew()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbAcGroup_Man.SelectedIndex = 0
        gridview.DataSource = Nothing
        dtpFrom.Focus()
    End Function

    Private Sub frmBudgetAnalysis_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub frmBudgetAnalysis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtcompany, "COMPANYNAME", , "ALL")

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            Dim dtCostCentre As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If

        strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,'1' RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACGRPNAME,CONVERT(VARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
        strSql += " ORDER BY RESULT,ACGRPNAME"
        objGPack.FillCombo(strSql, cmbAcGroup_Man, , True)
        btnNew_Click(Me, New EventArgs)

    End Sub

    Private Sub cmbAcname_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname.Enter
        ''ACNAME
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        If cmbAcGroup_Man.Text <> "ALL" Then
            strSql += " WHERE ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN ('" & cmbAcGroup_Man.Text & "'))"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        objGPack.FillCombo(strSql, cmbAcname, , True)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL", GetSelectedCompanyId(chkcmbCompany, False), "")
        Dim SelectedCostid As String = ""
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCostid = GetSelectedCostId(chkCmbCostCentre, False)
        End If
        Dim SelectedAcgrpId As String = ""
        If cmbAcGroup_Man.Text <> "ALL" Then
            strSql = "SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN ('" & cmbAcGroup_Man.Text & "')"
            SelectedAcgrpId = GetSqlValue(cn, strSql)
        End If
        Dim SelectedAccode As String = ""
        If cmbAcname.Text <> "ALL" Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcname.Text & "'"
            SelectedAccode = GetSqlValue(cn, strSql)
        End If


        strSql = " EXEC " & cnAdminDb & "..SP_RPT_BUDGETANLYSIS"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "' "
        strSql += vbCrLf + " ,@TEMPTBL='TEMP" & systemId & "IBUDGET'"
        strSql += vbCrLf + " ,@MONTH='" & Val(dtpFrom.Value.ToString("MM")) & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COSTID ='" & SelectedCostid & "'"
        strSql += vbCrLf + " ,@ACGRPID = '" & SelectedAcgrpId & "'"
        strSql += vbCrLf + " ,@ACCCODE = '" & SelectedAccode & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & selectcompid & "'"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim ds As New DataSet
        Dim dtGrid As New DataTable
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        gridview.DataSource = Nothing
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim IntBalance As Decimal = 0
        Dim Balance As Decimal = 0

        gridview.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise1(gridview, "PARTICULAR")
        DataGridView_SummaryFormatting(gridview)
    End Sub
    Public Sub FillGridGroupStyle_KeyNoWise1(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(cnt).Item("COLHEAD").ToString = "T1" Then
                gridView.Rows(cnt).Cells("PARTICULAR").Style.ForeColor = Color.Blue
                gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
            If dt.Rows(cnt).Item("COLHEAD").ToString = "G" Then
                gridView.Rows(cnt).Cells("PARTICULAR").Style.ForeColor = Color.Green
                gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            '.Columns("INTDEBIT").DefaultCellStyle.Format = "#,##0.00"

            .Columns("RESULT").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("ACGROUPID").Visible = False
            .Columns("ACMGROUPID").Visible = False
            .Columns("COLHEAD").Visible = False

            .Columns("PARTICULAR").Width = 200
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub



    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "INTEREST ANALYSIS", gridview, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "INTEREST ANALYSIS", gridview, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridview.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then
                    wid += gridView.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(gridView.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
            Me.Size = New Size(wid, Me.Height)
        End If
        If FormReLocation Then SetCenterLocation(Me)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridview.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function
End Class
Public Class Interest_Properties
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
End Class