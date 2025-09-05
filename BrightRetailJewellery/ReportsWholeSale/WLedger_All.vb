Imports System.Data.OleDb
Public Class WLedger_All
    Dim objGridShower As frmGridDispDia
    Dim Strsql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim CustName As String
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        rbtCustomer.Checked = True
        rbtSummary.Checked = True
        dtpFrom.Select()
    End Sub

    Private Sub WLedger_All_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WLedger_All_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub rbtCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCustomer.CheckedChanged
        If rbtCustomer.Checked Then
            Strsql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'C' ORDER BY ACNAME"
        Else
            Strsql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D','I') ORDER BY ACNAME"
        End If
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        chkLstName.Items.Clear()
        For Each ro As DataRow In dt.Rows
            chkLstName.Items.Add(ro.Item("ACNAME").ToString)
            chkLstName.SetItemChecked(chkLstName.Items.Count - 1, chkSelectAll.Checked)
        Next
    End Sub

    Private Sub chkSelectAll_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckStateChanged
        SetChecked_CheckedList(chkLstName, chkSelectAll.Checked)
    End Sub

    Private Function DetailView(ByVal CName As String) As DataTable
        Strsql = vbCrLf + " Exec " + cnStockDb + "..Sp_RPT_WALL_LEDGER_DETAILS  '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        If chkCumulative.Checked = True Then
            Strsql += vbCrLf + ",'Y'"
        Else
            Strsql += vbCrLf + ",'N'"
        End If
        Strsql += vbCrLf + ",'" + CustName + "'"
        Cmd = New OleDbCommand(Strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid.TableName = "DETAILED"
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Return dtGrid
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub
    Private Function SummaryView() As DataTable
        Strsql = vbCrLf + " Exec " + cnStockDb + "..Sp_RPT_WALL_LEDGER_SUMMARY '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        If chkCumulative.Checked = True Then
            Strsql += vbCrLf + ",'Y'"
        Else
            Strsql += vbCrLf + ",'N'"
        End If
        Strsql += vbCrLf + ",'" + CustName + "'"
        If rbtCustomer.Checked = True Then
            Strsql += vbCrLf + ",'Y'"
        Else
            Strsql += vbCrLf + ",'N'"
        End If

        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid.TableName = "SUMMARY"
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Return dtGrid
    End Function
    Private Sub btnView_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not chkLstName.CheckedItems.Count > 0 Then chkSelectAll.Checked = True
        CustName = GetChecked_CheckedList(chkLstName, True)
        CustName = Replace(CustName, "'", "")
        Dim dtGrid As New DataTable
        If rbtSummary.Checked Then
            dtGrid = SummaryView()
        Else
            dtGrid = DetailView(CustName)
        End If
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = IIf(rbtSummary.Checked, "CUSTOMER", "SMITH & DEALER") & " LEDGER VIEW"
        Dim tit As String
        If chkLstName.CheckedItems.Count = 1 Then
            'chkLstName.CheckedItems.Item(1).ToString()
            tit = "LEDGER VIEW FOR  " + chkLstName.CheckedItems.Item(0).ToString() + vbCrLf
        Else
            tit = "LEDGER VIEW FOR ALL " + vbCrLf
        End If
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        If rbtSummary.Checked = True Then
            objGridShower.gridViewHeader.Visible = True
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        Else
            objGridShower.gridViewHeader.Visible = False
        End If
        If rbtSummary.Checked Then
            DataGridView_SummaryFormatting(objGridShower.gridView)
        Else
            DataGridView_DetailFormatting(objGridShower.gridView)
        End If
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))
        Call SetGridHeadColWid(objGridShower.gridViewHeader)
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width
            .Columns("ISSWT~ISSAMT").Width = +objGridShower.gridView.Columns("ISSWT").Width + objGridShower.gridView.Columns("ISSAMT").Width
            .Columns("RECWT~RECAMT").Width = objGridShower.gridView.Columns("RECWT").Width _
                + objGridShower.gridView.Columns("RECAMT").Width
            .Columns("CLSWT~CLSAMT").Width = objGridShower.gridView.Columns("CLSWT").Width _
                + objGridShower.gridView.Columns("CLSAMT").Width
        End With
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        gridviewHead.Visible = True
        Strsql = "SELECT ''[PARTICULAR],''[RECWT~RECAMT],''[ISSWT~ISSAMT],''[CLSWT~CLSAMT]"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("ISSWT~ISSAMT").HeaderText = "ISSUE"
        gridviewHead.Columns("RECWT~RECAMT").HeaderText = "RECEIPT"
        gridviewHead.Columns("CLSWT~CLSAMT").HeaderText = "CLOSING"
    End Sub
    Private Sub DataGridView_DetailFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("TRANDATE").Width = 100
            .Columns("TRANNO").Width = 70
            .Columns("TRANTYPE").Width = 80
            .Columns("PARTICULAR").Width = 200
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("TOUCH").Width = 60
            .Columns("PUREWT").Width = 80
            .Columns("RECWT").Width = 80
            .Columns("ISSWT").Width = 80
            .Columns("RECAMT").Width = 100
            .Columns("ISSAMT").Width = 100

            .Columns("GRSWT").Visible = Not chkCumulative.Checked
            .Columns("NETWT").Visible = Not chkCumulative.Checked
            .Columns("TOUCH").Visible = Not chkCumulative.Checked
            .Columns("PUREWT").Visible = Not chkCumulative.Checked
            

            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("BATCHNO").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TRESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 175
            '.Columns("OPE_WT").Width = 80
            .Columns("RECWT").Width = 80
            .Columns("RECAMT").Width = 80

            .Columns("ISSWT").Width = 80
            .Columns("ISSAMT").Width = 80

            .Columns("CLSWT").Width = 80
            .Columns("CLSAMT").Width = 80


            .Columns("ISSWT").HeaderText = "WEIGHT"
            .Columns("ISSAMT").HeaderText = "AMOUNT"
            .Columns("RECWT").HeaderText = "WEIGHT"
            .Columns("RECAMT").HeaderText = "AMOUNT"
            .Columns("CLSWT").HeaderText = "WEIGHT"
            .Columns("CLSAMT").HeaderText = "AMOUNT"
            
            .Columns("ACNAME").Visible = False
            ' .Columns("ACCODE").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("KEYNO").Visible = False

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)

            GridViewHeaderCreator(objGridShower.gridViewHeader)
            SetGridHeadColWid(objGridShower.gridViewHeader)

        End With
    End Sub

    Private Sub rbtGoldSmith_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGoldSmith.CheckedChanged

    End Sub
End Class
