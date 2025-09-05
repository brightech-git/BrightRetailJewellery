Imports System.Data.OleDb
Public Class frmItemVsMetalStockSummary
#Region " VARIABLE"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim dtCostCentre As New DataTable
#End Region
#Region " FORM EVENTS"
    Private Sub frmItemVsMetalStockSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub

    Private Sub frmItemVsMetalStockSummary_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
#Region " USER DEFINE FUNCTION"
    Private Sub gridAutoSize(ByVal dv As DataGridView)
        With dv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Sub
#End Region
#Region " BUTTON EVENTS"
    Private Sub btnView_Search_Click(sender As Object, e As EventArgs) Handles btnView_Search.Click
        Try
            btnView_Search.Enabled = False
            gridView.DataSource = Nothing

            Dim companyId As String = ""
            strsql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany.Text & "' "
            companyId = GetSqlValue(cn, strsql)
            If companyId = "" Then
                companyId = "ALL"
            End If
            dt = New DataTable
            strsql = " "
            strsql += vbCrLf + "EXEC " & cnAdminDb & "..[SP_VIEWITEMVSMETALSTOCKSUMMARY] "
            strsql += vbCrLf + "@DBNAME  = '" & cnStockDb & "'"
            strsql += vbCrLf + ",@ADMINDB  = '" & cnAdminDb & "'"
            strsql += vbCrLf + ",@FROMDATE  = '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + ",@TODATE  = '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + ",@COMPANYID  = '" & companyId & "'"
            strsql += vbCrLf + ",@COSTID  = '" & cmbCostCentre.SelectedValue.ToString & "'"
            strsql += vbCrLf + ",@METALID  = ''"
            strsql += vbCrLf + ",@SYSTEMID  = '" & systemId & "'"
            strsql += vbCrLf + ",@GROUPBY  ='" & cmbGroupBy.Text & "'"
            Dim ds As New DataSet
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds)
            dt = New DataTable
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridView, False, False, True, False)
                    For i As Integer = 0 To .Rows.Count - 1
                        If .Rows(i).Cells("RESULT").Value.ToString = "2" Then
                            .Rows(i).Cells("ITEMNAME").Style.BackColor = Color.Lavender
                        ElseIf .Rows(i).Cells("RESULT").Value.ToString = "3" Then
                            .Rows(i).Cells("ITEMNAME").Style.BackColor = Color.LightBlue
                        ElseIf .Rows(i).Cells("RESULT").Value.ToString = "0" Then
                            .Rows(i).Cells("ITEMNAME").Style.BackColor = Color.LightPink
                        End If
                    Next
                    If .Columns.Contains("METALID") Then
                        .Columns("METALID").Visible = False
                    End If
                    If .Columns.Contains("METALNAME") Then
                        .Columns("METALNAME").Visible = False
                    End If
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("AMOUNT").Frozen = True
                    gridAutoSize(gridView)
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                dtpFrom.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        CmbCompany.Items.Clear()
        CmbCompany.Items.Add("ALL")
        strsql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYID"
        objGPack.FillCombo(strsql, CmbCompany, False)
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        If dtCostCentre.Rows.Count > 0 Then
            cmbCostCentre.DataSource = Nothing
            cmbCostCentre.DataSource = dtCostCentre
            cmbCostCentre.ValueMember = "COSTID"
            cmbCostCentre.DisplayMember = "COSTNAME"
        End If
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        'cmbMetal.Items.Clear()
        'cmbMetal.Items.Add("ALL")
        'strsql = "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  ORDER BY DISPLAYORDER "
        'objGPack.FillCombo(strsql, cmbMetal, False)

        cmbGroupBy.Items.Clear()
        cmbGroupBy.Items.Add("NONE")
        cmbGroupBy.Items.Add("METALWISE")
        cmbGroupBy.Text = "NONE"


        gridView.DataSource = Nothing
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "ITEM VS METAL WISE SUMMARY", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "ITEM VS METAL WISE SUMMARY", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExtToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region
End Class