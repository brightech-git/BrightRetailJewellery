Imports System.Data.OleDb
Public Class W_DailyRateReport
    Dim objGridShower As frmwGridDispDia
    Dim Strsql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim strMetalName As String

    Private Sub W_DailyRateReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        End If
    End Sub

    Private Sub W_DailyRateReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        Call Load_Metal()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub Load_Metal()
        Strsql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  Order by MetalName "
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            cmbMetal.Items.Add(ro.Item("metalname").ToString)
        Next
    End Sub
    Private Function VIEW()
        strMetalName = cmbMetal.Text
        Strsql = vbCrLf + " Exec " + cnStockDb + "..SP_RPT_WDAILYRATE '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "','" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + ",'" + strMetalName + "'," & Val(txtRate.Text)
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

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        
        Dim dtGrid As New DataTable("SUMMARY")
        dtGrid = VIEW()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmwGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf DataGridView_ColWidth_Changed
       
        Dim tit As String = strMetalName + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        dtGrid.TableName = "SUMMARY"
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = dtGrid 'objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.gridViewHeader.Visible = False

        FormatGridColumns(objGridShower.gridView, False, False, , False)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))



    End Sub
    Private Sub DataGridView_ColWidth_Changed(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        Dim dtHead As New DataTable
        objGridShower.gridViewHeader.Visible = True
        Strsql = "SELECT ''[PARTICULAR],''[CRNETWT~CRAMOUNT~CRRATE~CINETWT~CIAMOUNT~CIRATE],"
        Strsql = Strsql & " ''[MRNETWT~MRMC~MINETWT~MIMC],''SCROLL"

        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtHead)
        objGridShower.gridViewHeader.DataSource = dtHead

        objGridShower.gridViewHeader.Columns("PARTICULAR").HeaderText = ""
        objGridShower.gridViewHeader.Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width

        objGridShower.gridViewHeader.Columns("CRNETWT~CRAMOUNT~CRRATE~CINETWT~CIAMOUNT~CIRATE").HeaderText = "CASH BALANCE"
        objGridShower.gridViewHeader.Columns("CRNETWT~CRAMOUNT~CRRATE~CINETWT~CIAMOUNT~CIRATE").Width = objGridShower.gridView.Columns("CRNETWT").Width + objGridShower.gridView.Columns("CRAMOUNT").Width + _
            objGridShower.gridView.Columns("CRRATE").Width + objGridShower.gridView.Columns("CINETWT").Width + _
            objGridShower.gridView.Columns("CIAMOUNT").Width + objGridShower.gridView.Columns("CIRATE").Width



        objGridShower.gridViewHeader.Columns("MRNETWT~MRMC~MINETWT~MIMC").HeaderText = "METAL BALANCE"
        objGridShower.gridViewHeader.Columns("MRNETWT~MRMC~MINETWT~MIMC").Width = objGridShower.gridView.Columns("MRNETWT").Width + _
           objGridShower.gridView.Columns("MRMC").Width + objGridShower.gridView.Columns("MINETWT").Width + _
            objGridShower.gridView.Columns("MIMC").Width


        objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        objGridShower.gridViewHeader.Columns("SCROLL").Visible = False

        dtHead = New DataTable

        objGridShower.gridViewSubHeader.Visible = True
        Strsql = "SELECT ''[PARTICULAR],''[CRNETWT~CRAMOUNT~CRRATE],''[CINETWT~CIAMOUNT~CIRATE],"
        Strsql = Strsql & " ''[MRNETWT~MRMC],''[MINETWT~MIMC],''SCROLL"

        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtHead)
        objGridShower.gridViewSubHeader.DataSource = dtHead

        objGridShower.gridViewSubHeader.Columns("PARTICULAR").HeaderText = ""

        objGridShower.gridViewSubHeader.Columns("CRNETWT~CRAMOUNT~CRRATE").HeaderText = "SALES"
        objGridShower.gridViewSubHeader.Columns("CINETWT~CIAMOUNT~CIRATE").HeaderText = "PURCHASE"

        objGridShower.gridViewSubHeader.Columns("MRNETWT~MRMC").HeaderText = "SALES"
        objGridShower.gridViewSubHeader.Columns("MINETWT~MIMC").HeaderText = "PURCHASE"

        objGridShower.gridViewSubHeader.Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width
        objGridShower.gridViewSubHeader.Columns("CRNETWT~CRAMOUNT~CRRATE").Width = objGridShower.gridView.Columns("CRNETWT").Width + objGridShower.gridView.Columns("CRAMOUNT").Width + _
        objGridShower.gridView.Columns("CRRATE").Width

        objGridShower.gridViewSubHeader.Columns("CINETWT~CIAMOUNT~CIRATE").Width = objGridShower.gridView.Columns("CINETWT").Width + _
            objGridShower.gridView.Columns("CIAMOUNT").Width + objGridShower.gridView.Columns("CIRATE").Width

        objGridShower.gridViewSubHeader.Columns("MRNETWT~MRMC").Width = objGridShower.gridView.Columns("MRNETWT").Width + _
           objGridShower.gridView.Columns("MRMC").Width


        objGridShower.gridViewSubHeader.Columns("MINETWT~MIMC").Width = objGridShower.gridView.Columns("MINETWT").Width + _
           objGridShower.gridView.Columns("MIMC").Width
        
        objGridShower.gridViewSubHeader.Columns("SCROLL").HeaderText = ""
        objGridShower.gridViewSubHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        objGridShower.gridViewSubHeader.Columns("SCROLL").Visible = False


    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").HeaderText = "TRANDATE"
            .Columns("TRANDATE").HeaderText = "TRANDATE"

            .Columns("CRNETWT").HeaderText = "NET WT"
            .Columns("CRAMOUNT").HeaderText = "AMOUNT"
            .Columns("CRRATE").HeaderText = "RATE"

            .Columns("CINETWT").HeaderText = "NETWT"
            .Columns("CIAMOUNT").HeaderText = "AMOUNT"
            .Columns("CIRATE").HeaderText = "RATE"

            .Columns("MRNETWT").HeaderText = "NETWT"
            .Columns("MRMC").HeaderText = "MC"
            .Columns("MINETWT").HeaderText = "NETWT"
            .Columns("MIMC").HeaderText = "MC"

            .Columns("RESULT").HeaderText = "RESULT"
            .Columns("COLHEAD").HeaderText = "COLHEAD"
            .Columns("KEYNO").HeaderText = "KEYNO"

            .Columns("PARTICULAR").Width = 200
            .Columns("TRANDATE").Visible = False

            .Columns("CRNETWT").Width = 80
            .Columns("CRAMOUNT").Width = 80
            .Columns("CRRATE").Width = 80

            .Columns("CINETWT").Width = 80
            .Columns("CIAMOUNT").Width = 80
            .Columns("CIRATE").Width = 80

            .Columns("MRNETWT").Width = 80
            .Columns("MRMC").Width = 80
            .Columns("MINETWT").Width = 80
            .Columns("MIMC").Width = 80

            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False


            'Call DataGridView_ColWidth_Changed(objGridShower.gridViewHeader, )

        End With

       

        'objGridShower.gridViewHeader.Visible = True
        'With objGridShower.gridViewHeader
        '    .RowCount = 1
        '    .ColumnCount = 2
        '    .Columns(0).Name = "CASH BALANCE"
        '    .Columns(1).Name = "METAL BALANCE"

        '    .Columns(0).Width = dgv.Columns("PARTICULAR").Width + dgv.Columns("TRANDATE").Width + _
        '        dgv.Columns("CRNETWT").Width + dgv.Columns("CRAMOUNT").Width + dgv.Columns("CIRATE").Width

        '    .Columns(1).Width = dgv.Columns("MRNETWT").Width + dgv.Columns("MRMC").Width + dgv.Columns("MINETWT").Width + dgv.Columns("MIMC").Width

        'End With
    End Sub
End Class