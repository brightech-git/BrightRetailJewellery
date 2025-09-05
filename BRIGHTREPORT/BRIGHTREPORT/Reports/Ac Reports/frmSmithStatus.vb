Imports System.Data.OleDb
Public Class frmSmithStatus
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtcostCentre As DataTable
    Dim dtcompany As DataTable


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False
        strSql = vbCrLf + " EXECUTE " & cnAdminDb & "..SP_RPT_SMITHSTATUSRPT"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@TEMPTAB = 'TEMP123'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCmbCompany.Text = "ALL" Then
            strSql += vbCrLf + ",@COMPANYID = 'ALL'"
        Else
            strSql += vbCrLf + ",@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
        End If

        If ChkCmbCostCenter.Text = "ALL" Then
            strSql += vbCrLf + ",@COSTID = 'ALL'"
        Else
            strSql += vbCrLf + ",@COSTID = '" & GetSelectedCostId(ChkCmbCostCenter, False) & "'"
        End If
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        Dim dss As New DataSet

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dss)
        Dim dtGrid As New DataTable

        If dss.Tables.Contains("TABLE") Then
            dtGrid = dss.Tables(0)
        Else
            dtGrid = dss.Tables(1)
        End If

        Dim dtGrid1 As New DataTable
        dtGrid1 = dtGrid.Copy
        If Not dtGrid1.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnSearch.Enabled = True
            Exit Sub
        End If
        DataGridView1.DataSource = dtGrid
        DataGridView1.Columns("PARTICULAR").Width = 300
        TabControl1.SelectedTab = TabPage2
        DataGridView1.Focus()
        'btnSearch.Enabled = True
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub frmOrderStatusName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If TabControl1.SelectedTab.Name = TabPage2.Name Then
                TabControl1.SelectedTab = TabPage1
                btnSearch.Enabled = True
            End If
            dtpFrom.Focus()
        End If
    End Sub

    Private Sub frmOrderStatusName_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TabControl1.ItemSize = New System.Drawing.Size(1, 1)
        Me.TabControl1.Region = New Region(New RectangleF(Me.TabPage1.Left, Me.TabPage1.Top, Me.TabPage1.Width, Me.TabPage1.Height))
        fNew()
    End Sub
    Function fNew()
        dtpTo.Value = GetServerDate()
        dtpFrom.Focus()
        btnSearch.Enabled = True
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtcompany, "COMPANYNAME", , strCompanyName)

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtcostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcostCentre)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCostCenter, dtcostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then ChkCmbCostCenter.Enabled = False
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TabControl1.SelectedTab.Name = TabPage2.Name Then
            TabControl1.SelectedTab = TabPage1
            btnSearch.Enabled = True
        End If
    End Sub
    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If DataGridView1.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Entry Type wise BreakUp", DataGridView1, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If DataGridView1.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Entry Type wise BreakUp", DataGridView1, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class