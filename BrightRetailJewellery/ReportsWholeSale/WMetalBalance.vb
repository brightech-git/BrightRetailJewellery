Imports System.Data.OleDb
Public Class WMetalBalance
    Dim objGridShower As frmGridDispDia
    Dim Strsql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim CatName As String
    Private Sub MetalBalance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub MetalBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        Call LOAD_CATNAME()
    End Sub
    Private Sub LOAD_CATNAME()
        Strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  CATNAME"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        chkCatName.Items.Clear()
        For Each ro As DataRow In dt.Rows
            chkCatName.Items.Add(ro.Item("CATNAME").ToString)
            chkCatName.SetItemChecked(chkCatName.Items.Count - 1, chkSelectAll.Checked)
        Next
    End Sub
    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked = True Then
            Call LOAD_CATNAME()
        End If
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkCatName.CheckedItems.Count <= 0 Then
            chkSelectAll.Checked = True
        End If
        CatName = GetChecked_CheckedList(chkCatName, True)
        CatName = Replace(CatName, "'", "")
        Dim dtGrid As New DataTable("SUMMARY")
        dtGrid = VIEW()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        Dim tit As String = "METAL BALANCE" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        dtGrid.TableName = "SUMMARY"
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.lblStatus.Text = "<Press [D] for Detail View>"
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))

    End Sub
    Private Function VIEW()


        Strsql = vbCrLf + " Exec " + cnStockDb + "..SP_RPT_WMETALSTOCK_SUMMARY '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + ",'" + CatName + "'"
        If rbtGrsWt.Checked = True Then
            Strsql += vbCrLf + ",'G'"
        ElseIf rbtNetWt.Checked = True Then
            Strsql += vbCrLf + ",'N'"
        ElseIf rbtPureWt.Checked = True Then
            Strsql += vbCrLf + ",'P'"
        End If
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Return dtGrid
    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtpFrom.Select()
    End Sub
#Region "GridDiaShower Events"
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "DETAILED" Then
                Dim CATNAME As String = dgv.CurrentRow.Cells("PARTICULAR").Value.ToString
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.FormReSize = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                DataGridView_SummaryFormatting(f.gridView)
                FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
                Dim tit As String = "CATEGORY WISE SUMMARY FOR " + CATNAME + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [D] for Detail View>"
                f.FormReSize = True
                'f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            End If
        End If
    End Sub

    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                Dim CATNAME As String = dgv.CurrentRow.Cells("PARTICULAR").Value.ToString
                Dim dt As DataTable = DetailView(CATNAME)
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
                FillGridGroupStyle_KeyNoWise(f.gridView)
                Dim tit As String = "DETAIL VIEW FOR CATEGORY : " + CATNAME + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [ESCAPE] for Summary View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()

            End If
        End If

    End Sub
    Private Function DetailView(ByVal CATNAME As String) As DataTable
        Strsql = " EXEC " & cnStockDb & "..SP_RPT_WMETALSTOCK_DETAILS '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "',"
        Strsql += vbCrLf + "'" & CATNAME & "'"
        If rbtGrsWt.Checked = True Then
            Strsql += vbCrLf + ",'G'"
        ElseIf rbtNetWt.Checked = True Then
            Strsql += vbCrLf + ",'N'"
        ElseIf rbtPureWt.Checked = True Then
            Strsql += vbCrLf + ",'P'"
        End If
        Dim dtGrid As New DataTable("DETAILED")
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtGrid)
        Return dtGrid
    End Function

    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("COLHEAD").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TRANNO").Width = 80
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            '.Columns("OPWT").Width = 100
            .Columns("RECWT").Width = 100
            .Columns("ISSWT").Width = 100
            '.Columns("CLWT").Width = 100
            '.Columns("OPWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("CLWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("OPWT").HeaderText = "OPENING"
            .Columns("ISSWT").HeaderText = "ISSUE"
            .Columns("RECWT").HeaderText = "RECEIPT"
            '.Columns("CLWT").HeaderText = "CLOSING"
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 200
            .Columns("OPWT").Width = 80
            .Columns("ISSWT").Width = 80
            .Columns("RECWT").Width = 80
            .Columns("CLWT").Width = 80
            .Columns("KEYNO").Width = 80
            .Columns("KEYNO").Visible = False
            .Columns("TABLENAME").Visible = False



            .Columns("OPWT").HeaderText = "OPENING"
            .Columns("ISSWT").HeaderText = "ISSUE"
            .Columns("RECWT").HeaderText = "RECEIPT"
            .Columns("CLWT").HeaderText = "CLOSING"



            .Columns("COLHEAD").Visible = False


            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
#End Region

End Class