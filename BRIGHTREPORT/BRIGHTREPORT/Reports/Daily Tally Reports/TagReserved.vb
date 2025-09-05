Imports System.Data.OleDb

Public Class TagReserved
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TagReserved_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        chkASD.Focus()
    End Sub

    Private Sub TagReserved_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        LoadCompany(chkLstCompany)
        funcLoadCostCentre()
        btnNew_Click(Me, New EventArgs)
    End Sub
    Function funcLoadCostCentre() As Integer
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString)
            Next
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim SelectedCompany As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim SelectedCostCentre As String = GetChecked_CheckedList(chkLstCostCentre, False)
        Dim Status As Char
        If rbtAll.Checked Then
            Status = "A"
        ElseIf rbtPending.Checked Then
            Status = "P"
        ElseIf rbtDelivered.Checked Then
            Status = "D"
        End If
        StrSql = vbCrLf + " EXEC " & cnStockDb & "..RPT_TAGRESERVED"
        StrSql += vbCrLf + " @ASD = '" & IIf(chkASD.Checked, "Y", "N") & "' "
        StrSql += vbCrLf + " ,@FROMDATE = '" & IIf(chkASD.Checked, dtpFrom.Value.AddDays(1).ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        StrSql += vbCrLf + " ,@COSTNAME = '" & SelectedCostCentre & "'"
        StrSql += vbCrLf + " ,@WITHCOUNTER = '" & IIf(chkCounter.Checked, "Y", "N") & "'"
        StrSql += vbCrLf + " ,@WITHDIRECT = '" & IIf(ChkInclBK.Checked, "Y", "N") & "'"
        StrSql += vbCrLf + " ,@STATUS = '" & Status & "' "
        StrSql += vbCrLf + " ,@ITEMWISE = '" & IIf(chkItemWise.Checked, "Y", "N") & "' "
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not chkItemWise.Checked Then
            dtGrid.Columns.Remove("COUNTERNAME")
            dtGrid.Columns.Remove("COUNTERNAME1")
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
        objGridShower.Text = "TAG RESERVED"
        Dim tit As String = "TAG RESERVED "
        tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        '' ''objGridShower.gridView.Columns("Status").Visible = False
        '' ''For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
        '' ''    If objGridShower.gridView.Rows(i).Cells("Status").Value.ToString = "D" Then
        '' ''        objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LavenderBlush
        '' ''    End If
        '' ''Next
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        If Not chkItemWise.Checked Then
            DataGridView_SummaryFormatting(objGridShower.gridView)
        End If
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("DESCRIPT")))
        objGridShower.Show()


        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        objGridShower.gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In objGridShower.gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        objGridShower.FormReSize = True

        If Not chkItemWise.Checked Then
            For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView.Rows(i).Cells("Status").Value.ToString = "D" Then
                    objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LavenderBlush
                End If
            Next
        Else
            objGridShower.gridView.Columns("RESULT").Visible = False
            objGridShower.gridView.Columns("COLHEAD").Visible = False
            objGridShower.gridView.Columns("KEYNO").Visible = False
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 100
            '.Columns("RUNNO").Width = 60
            .Columns("CUSTOMER").Width = 200
            .Columns("ITEMNAME").Width = 200
            .Columns("tAGNO").Width = 60
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("AMOUNT").Width = 120
            '.Columns("REMARK2").Width = 150


            .Columns("TRANDATE1").Visible = False
            .Columns("RESULT").Visible = False
            '.Columns("itemid").Visible = False
            .Columns("TRANNO1").Visible = False
            .Columns("CUSTOMER1").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False


            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"

            .Columns("Status").Visible = False
           
        End With
    End Sub

    Private Sub chkLstCostCentre_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Label_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Label_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chkAllCostCentre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllCostCentre.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkAllCostCentre.Checked)
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkASD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkASD.CheckedChanged
        If chkASD.Checked Then
            lbToDate.Visible = False
            dtpTo.Visible = False
        Else
            lbToDate.Visible = True
            dtpTo.Visible = True
        End If
    End Sub
End Class