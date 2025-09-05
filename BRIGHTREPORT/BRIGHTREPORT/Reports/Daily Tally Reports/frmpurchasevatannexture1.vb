Imports System.Data.OleDb
Public Class frmpurchasevatannexture1
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
        gridview.DataSource = Nothing
        dtpFrom.Focus()
    End Function

    Private Sub InterestAnalysys_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub InterestAnalysys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
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
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
            If strUserCentrailsed <> "Y" Then chkCmbCostCentre.Enabled = False
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If
        btnNew_Click(Me, New EventArgs)

    End Sub

    

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim selectcompid As String = IIf(chkcmbCompany.Text = "ALL", "ALL", GetSelectedCompanyId(chkcmbCompany, False))
        Dim SelectedCostid As String = "ALL"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCostid = GetSelectedCostId(chkCmbCostCentre, False)
        End If
        If rbtannextureI.Checked Then
            AnnextureI(SelectedCostid, selectcompid)
        Else
            AnnextureI(SelectedCostid, selectcompid)
            'AnnextureII(SelectedCostid, selectcompid)
        End If
    End Sub

    Public Sub AnnextureI(ByVal costid As String, ByVal compid As String)
        If rbtannextureI.Checked Then
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_VATFORM_ANNEXURE_1"
            strSql += vbCrLf + " @DBNAME='" & cnStockDb & "' "
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COSTID =""" & costid & """"
            strSql += vbCrLf + " ,@COMPANYIDS = """ & compid & """"
        End If
        
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
        gridview.DataSource = dtGrid
        With gridview
            'HeaderText
            .Columns("SELLERNAME").HeaderText = "Seller Name"
            .Columns("SELLERTINNO").HeaderText = "Seller Tin No."
            .Columns("COMMODITYCODE").HeaderText = "Commodity Code"
            .Columns("CATEGORYCODE").HeaderText = "Category Code"
            .Columns("INVNO").HeaderText = "Inv No."
            .Columns("INVDATE").HeaderText = "Inv Date"
            .Columns("INVVALUE").HeaderText = "Inv Value"
            .Columns("SALESTAX").HeaderText = "Tax Percentage"
            .Columns("TAX").HeaderText = "Tax"

            'Width
            .Columns("SELLERNAME").Width = 260
            .Columns("SELLERTINNO").Width = 120
            .Columns("COMMODITYCODE").Width = 135
            .Columns("CATEGORYCODE").Width = 120
            .Columns("INVNO").Width = 72
            .Columns("INVDATE").Width = 90
            .Columns("INVVALUE").Width = 100
            .Columns("SALESTAX").Width = 90
            .Columns("TAX").Width = 80

            .Columns("SELLERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SELLERTINNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("COMMODITYCODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("CATEGORYCODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("INVNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INVDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("INVVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RESULT").Visible = False
        End With
        FillGridGroupStyle_KeyNoWise1(gridview)
        gridview.Focus()
    End Sub
    Public Sub AnnextureII(ByVal costid As String, ByVal compid As String)
        strSql = " EXEC " & cnAdminDb & "..[AGR_SP_VATFORM_ANNEXURE_2]"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "' "
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYIDS = """ & compid & """"
        strSql += vbCrLf + " ,@COSTIDS =""" & costid & """"
        strSql += vbCrLf + " ,@AREAFILTER = 0"
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
        gridview.DataSource = dtGrid
        With gridview
            'HeaderText
            .Columns("SELLERNAME").HeaderText = "Seller Name"
            .Columns("SELLERTINNO").HeaderText = "Seller Tin No."
            .Columns("COMMODITYCODE").HeaderText = "Commodity Code"
            .Columns("CATEGORYCODE").HeaderText = "Category Code"
            .Columns("INVNO").HeaderText = "Inv No."
            .Columns("INVDATE").HeaderText = "Inv Date"
            .Columns("INVVALUE").HeaderText = "Inv Value"
            .Columns("SALESTAX").HeaderText = "Sales Tax"
            .Columns("TAX").HeaderText = "Tax"

            'Width
            .Columns("SELLERNAME").Width = 260
            .Columns("SELLERTINNO").Width = 120
            .Columns("COMMODITYCODE").Width = 135
            .Columns("CATEGORYCODE").Width = 120
            .Columns("INVNO").Width = 72
            .Columns("INVDATE").Width = 90
            .Columns("INVVALUE").Width = 100
            .Columns("SALESTAX").Width = 90
            .Columns("TAX").Width = 80

            .Columns("SELLERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SELLERTINNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("COMMODITYCODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("CATEGORYCODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("INVNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INVDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("INVVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RESULT").Visible = False
        End With
        FillGridGroupStyle_KeyNoWise1(gridview)
    End Sub
    Public Sub FillGridGroupStyle_KeyNoWise1(ByVal gridView As DataGridView)
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(cnt).Item("RESULT").ToString = "2" Then
                gridView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(cnt).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub
    

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
Public Class Purchasevatannexture1_Properties
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