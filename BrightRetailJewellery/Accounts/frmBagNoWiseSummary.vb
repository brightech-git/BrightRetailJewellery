Imports System.Data.OleDb
Public Class frmBagNoWiseSummary
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand

    Private Sub frmBagNoWiseSummary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

    End Sub

    Private Sub frmBagNoWiseSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBagNoWiseSummary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        strSql = " SELECT "
        strSql += " BAGNO"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
        strSql += " ,SUM(ISNULL(GRSWT,0))GRSWT,SUM(ISNULL(AMOUNT,0))AMOUNT,'SUMMARY' TABLENAME"
        strSql += " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " AND ISNULL(BAGNO,'') <> ''"
        strSql += " AND TRANTYPE = 'PU'"
        strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        strSql += " GROUP BY BAGNO,CATCODE"
        Dim dtGrid As New DataTable("SUMMARY")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text
        Dim tit As String = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text + vbCrLf
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.lblStatus.Text = "<Press [D] for Duplicate Print> <Press [Alt+D] for Detail View> <Press [C] for Cancel>"
        DataGridView_SummaryFormatting(objGridShower.gridView)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

#Region "GridDiaShower Events"

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "DETAILED" Then
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.FormReSize = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                DataGridView_SummaryFormatting(f.gridView)
                Dim tit As String = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [D] for Duplicate Print> <Press [Alt+D] for Detail View> <Press [C] for Cancel>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            End If
        ElseIf e.Alt And UCase(e.KeyCode) = UCase(Keys.D) Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                Dim bagNo As String = dgv.CurrentRow.Cells("BAGNO").Value.ToString
                Dim dt As DataTable = DetailView(bagNo)
                If Not dt.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                f.dsGrid.Tables.Add(dt)
                f.FormReSize = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
                DataGridView_DetailViewFormatting(f.gridView)
                Dim tit As String = "DETAIL VIEW FOR BAGNO : " + bagNo + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [ESCAPE] for Summary View> <Press [C] for Cancel>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
            End If
        End If
    End Sub

    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                Dim bagNo As String = dgv.CurrentRow.Cells("BAGNO").Value.ToString
                Try
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        Dim write As IO.StreamWriter
                        Dim memfile As String = "\BillPrint.mem"
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("TYPE", 15) & ":BAG")
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & bagNo)
                        write.WriteLine(LSet("TRANDATE", 15) & ":")
                        write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                            LSet("TYPE", 15) & ":BAG" & ";" & _
                            LSet("BATCHNO", 15) & ":" & bagNo & ";" & _
                            LSet("TRANDATE", 15) & ":" & _
                            LSet("DUPLICATE", 15) & ":N")
                        End If

                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                    End If
                Catch ex As Exception
                End Try
            End If
        ElseIf UCase(e.KeyChar) = "C" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim bagNo As String = dgv.CurrentRow.Cells("BAGNO").Value.ToString
            strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..MELTINGDETAIL"
            strSql += " WHERE ISNULL(BAGNO,'') = '" & bagNo & "'"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Cannot cancel this entry. Because issue entry made for this bagno", MsgBoxStyle.Information)
                Exit Sub
            End If
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMLOT"
            strSql += " WHERE CONVERT(VARCHAR(20),ISNULL(LOTNO,'')) = '" & bagNo & "' AND (ISNULL(CPCS,0)<>0 OR ISNULL(CGRSWT,0)<>0)"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Cannot cancel this entry. Because Tag generated for this bagno", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MessageBox.Show("Do you want to Cancel this BagNo Entry", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += " SET BAGNO = '',MELT_RETAG=''"
                strSql += " WHERE ISNULL(BAGNO,'') = '" & bagNo & "'"
                If ExecQuery(SyncMode.Transaction, strSql, cn, , cnCostId) Then
                    MsgBox("Successfully Cancelled..", MsgBoxStyle.Information)
                    Dim f As frmGridDispDia
                    f = objGPack.GetParentControl(dgv)
                    f.Dispose()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Function DetailView(ByVal bagNo As String) As DataTable
        strSql = " SELECT TRANNO,TRANDATE"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,BATCHNO,'DETAILED' TABLENAME,BAGNO"
        strSql += " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += " WHERE ISNULL(BAGNO,'') = '" & bagNo & "'"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += " AND TRANTYPE = 'PU'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        strSql += " GROUP BY TRANNO,TRANDATE,BATCHNO,BAGNO"
        Dim dtGrid As New DataTable("DETAILED")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function

    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BATCHNO").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("BAGNO").Visible = False
            .Columns("TRANNO").Width = 80
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("AMOUNT").Width = 100
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("TABLENAME").Visible = False
            .Columns("BAGNO").Width = 120
            .Columns("CATEGORY").Width = 350
            .Columns("GRSWT").Width = 120
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 120
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub
#End Region
End Class