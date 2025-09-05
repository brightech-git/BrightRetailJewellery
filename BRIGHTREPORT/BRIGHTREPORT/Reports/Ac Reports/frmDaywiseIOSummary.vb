Imports System.Data.OleDb
Public Class frmDaywiseIOSummary
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim dtMetalMast As New DataTable
    Dim RELIGION As Boolean = IIf(GetAdmindbSoftValue("RELIGION", "N") = "Y", True, False)

    Private Sub PacketWiseStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AddressInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        Call FuncLoad()
    End Sub
    Public Sub FuncLoad()

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        strSql = " SELECT 'ALL' AS METALNAME ,1 AS RESULT UNION ALL"
        strSql += " SELECT METALNAME,2 AS RESULT FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY RESULT,METALNAME"
        dtMetalMast = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetalMast)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbMetalid, dtMetalMast, "METALNAME", , "ALL")

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkCmbCostCentre.Enabled = True
            chkCmbCostCentre.Items.Clear()
            strSql = " SELECT 'ALL' AS COSTNAME ,1 AS RESULT UNION ALL"
            strSql += " SELECT COSTNAME,2 AS RESULT FROM " & cnAdminDb & "..COSTCENTRE ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        Call FuncLoad()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Selectedmetalid As String = GetSelectedMetalid(ChkCmbMetalid, False)
        Dim SelectedCostId As String = GetSelectedCostId(chkCmbCostCentre, False)
        If SelectedCostId = "''" Then SelectedCostId = "ALL"
        If chkCmbCostCentre.Text = "ALL" Then SelectedCostId = "ALL"
        If ChkCmbMetalid.Text = "ALL" Then Selectedmetalid = "ALL"
        If rbtSummary.Checked Then
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_DAYWISEIOSUMMARY"
        Else
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_DAYWISESTOCKDETAIL"
        End If
        strSql += vbCrLf + " @ADMINDBNAME= '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@DBNAME= '" & cnStockDb & "'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@COSTIDS='" & SelectedCostId & "'"
        strSql += vbCrLf + ",@METAL='" & Selectedmetalid & "'"
        strSql += vbCrLf + ",@TEMPTABLE= 'TEMP" & systemId & "DAYWISEIOSUMMARY'"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        Dim ds As New DataSet
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DAYWISEIOSUMMARY"
        Dim tit As String
        tit = "DAY WISE IO SUMMARY DATE FROM " & Format(dtpFrom.Value, "dd-MM-yyyy") & " TO " & Format(dtpTo.Value, "dd-MM-yyyy") & "  "
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = dtGrid ' objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        FormatGridColumns(objGridShower.gridView, False, False, , True)
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))
        'With objGridShower.gridView
        '    .Columns("RESULT").Visible = False
        'End With
        'For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
        '    If dgvRow.Cells("RESULT").Value.ToString = "0" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("RESULT").Value.ToString = "2" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("RESULT").Value.ToString = "4" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("RESULT").Value.ToString = "5" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.Beige
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("RESULT").Value.ToString = "7" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.Beige
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("RESULT").Value.ToString = "8" Then
        '        dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    End If
        'Next
    End Sub
End Class