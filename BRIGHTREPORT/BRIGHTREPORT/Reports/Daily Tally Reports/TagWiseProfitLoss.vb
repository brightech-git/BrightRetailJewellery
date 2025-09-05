Imports System.Data.OleDb
Public Class TagWiseProfitLoss
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String

    Private Sub TagWiseProfitLoss_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub comboxAddLoad(ByVal employee As Boolean)
        cmbGroupBy.Items.Clear()
        cmbGroupBy.Items.Add("NONE")
        cmbGroupBy.Items.Add("ITEM")
        If employee = True Then
            cmbGroupBy.Items.Add("EMPLOYEE")
        End If
        cmbGroupBy.Text = "NONE"
    End Sub

    Private Sub TagWiseProfitLoss_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DTMETAL As New DataTable
        LoadCompany(chkLstCompany)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(StrSql, chkLstCostCentre)
            da = New OleDbDataAdapter(StrSql, cn)
            Dim dtCostCentre As New DataTable
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                'chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, "ALL", False))
                For i As Integer = 0 To dtCostCentre.Rows.Count - 1
                    If cnCostName = dtCostCentre.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            Else
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If

        chklistMetal.Items.Clear()
        StrSql = "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,METALNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DTMETAL)

        chklistMetal.Items.Add("ALL")
        For Each RO As DataRow In DTMETAL.Rows
            chklistMetal.Items.Add(RO("METALNAME").ToString)
        Next
        chkMetalSelectAll.Checked = True
        chklistMetal.SetItemChecked(0, True)

        comboxAddLoad(True)
        chkFormat1.Checked = True
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' cmbGroupBy.Text = "NONE"
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If

        If chklistMetal.Items.Count > 0 Then
            chkMetalSelectAll.Checked = True
        Else
            chkMetalSelectAll.Checked = False
        End If

        Dim chkMetalName As String = GetChecked_CheckedList(chklistMetal, False)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)

        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        If chkFormat2.Checked Then
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFITLOSS_FORMAT2"
        ElseIf rbtFormat3.Checked Then
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFITLOSS_FORMAT3"
            'ElseIf rbtFormat3.Checked Then
            '    StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFITLOSS_FORMAT4"
        Else
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFITLOSS"
        End If
        StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        StrSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
        StrSql += vbCrLf + " ,@METALNAME = '" & chkMetalName & "'"
        StrSql += vbCrLf + " ,@GROUPBY = '" & Mid(cmbGroupBy.Text, 1, 1) & "'"
        If chkFormat2.Checked Then
            StrSql += vbCrLf + " ,@WASTBASED = '" & IIf(chkPurchaseWt.Checked, "T", " ") & "'"
        End If
        If rbtFormat3.Checked Then
            StrSql += vbCrLf + " ,@GP = '" & IIf(chkGP.Checked, "Y", "N") & "'"
        End If
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        StrSql = " SELECT * FROM MASTER..TEMP_TAGWISEPROFITLOSS ORDER BY KEYNO"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "TAGWISE PROFIT & LOSS"
        Dim tit As String = "TAGWISE PROFIT & LOSS FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.Show()
        objGridShower.FormReSize = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        If chkFormat2.Checked Then
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
        End If
        If rbtFormat3.Checked Then
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged3
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator3(objGridShower.gridViewHeader)
            'SetGridHeadColWid3(CType(sender, DataGridView))
        End If

        Prop_Sets()
    End Sub

    Private Sub gridView_ColumnWidthChanged3(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid3(CType(sender, DataGridView))
    End Sub

    Public Sub GridViewHeaderCreator3(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        StrSql = "SELECT ''[PARTICULAR~SUBITEM]"
        StrSql += " ,''[PURGRSWT~PURNETWT~PURWASTPER~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~PURAMOUNT]"
        StrSql += " ,''[TRANDATE~TRANNO~TAGNO~GRSWT~NETWT~WASTPER~WASTAGE~MCHARGE~STNWT~STNAMT~AMOUNT]"
        StrSql += " ,''[PROFITPER~PROFITWEIGHT~PROFITAMOUNT]"
        StrSql += " ,''SCROLL"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = Nothing
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~SUBITEM").HeaderText = ""
        gridviewHead.Columns("PURGRSWT~PURNETWT~PURWASTPER~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~PURAMOUNT").HeaderText = "PURCHASE"
        gridviewHead.Columns("TRANDATE~TRANNO~TAGNO~GRSWT~NETWT~WASTPER~WASTAGE~MCHARGE~STNWT~STNAMT~AMOUNT").HeaderText = "SALES"
        gridviewHead.Columns("PROFITPER~PROFITWEIGHT~PROFITAMOUNT").HeaderText = "PROFIT"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        SetGridHeadColWid3(gridviewHead)
    End Sub
    Private Sub SetGridHeadColWid3(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("PARTICULAR~SUBITEM").Width = f.gridView.Columns("PARTICULAR").Width + f.gridView.Columns("SUBITEM").Width

            .Columns("PURGRSWT~PURNETWT~PURWASTPER~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~PURAMOUNT").Width = f.gridView.Columns("PURGRSWT").Width _
                                                    + f.gridView.Columns("PURNETWT").Width _
                                                    + f.gridView.Columns("PURWASTPER").Width + f.gridView.Columns("PURWASTAGE").Width _
                                                    + f.gridView.Columns("PURMC").Width _
                                                    + f.gridView.Columns("PURSTNWT").Width _
                                                    + f.gridView.Columns("PURSTNAMT").Width _
                                                    + f.gridView.Columns("PURAMOUNT").Width

            .Columns("TRANDATE~TRANNO~TAGNO~GRSWT~NETWT~WASTPER~WASTAGE~MCHARGE~STNWT~STNAMT~AMOUNT").Width = f.gridView.Columns("TRANDATE").Width _
                                                   + f.gridView.Columns("TRANNO").Width _
                                                   + f.gridView.Columns("TAGNO").Width _
                                                   + f.gridView.Columns("GRSWT").Width _
                                                   + f.gridView.Columns("NETWT").Width _
                                                   + f.gridView.Columns("WASTAGE").Width + f.gridView.Columns("WASTPER").Width _
                                                   + f.gridView.Columns("MCHARGE").Width _
                                                   + f.gridView.Columns("STNWT").Width _
                                                   + f.gridView.Columns("STNAMT").Width _
                                                   + f.gridView.Columns("AMOUNT").Width

            .Columns("PROFITPER~PROFITWEIGHT~PROFITAMOUNT").Width = f.gridView.Columns("PROFITPER").Width + f.gridView.Columns("PROFITWEIGHT").Width _
                                                    + f.gridView.Columns("PROFITAMOUNT").Width

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub

    Public Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        StrSql = "SELECT ''[PARTICULAR~SUBITEM~TRANDATE~TRANNO~TAGNO]"
        StrSql += " ,''[SALEWT~SALEWASTAGE~SALEMC~STNWT~STNAMT~TOTALSALEWT]"
        StrSql += " ,''[PURWT~PURTOUCH~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~TOTALPURWT]"
        StrSql += " ,''[PROFITWT~PROFITAMT]"
        StrSql += " ,''SCROLL"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = Nothing
        gridviewHead.DataSource = dtHead
        'gridviewHead.Columns("PARTICULAR~SUBITEM~TRANDATE~TRANNO~TAGNO").DefaultCellStyle.BackColor = Color.LavenderBlush
        gridviewHead.Columns("PARTICULAR~SUBITEM~TRANDATE~TRANNO~TAGNO").HeaderText = ""
        gridviewHead.Columns("SALEWT~SALEWASTAGE~SALEMC~STNWT~STNAMT~TOTALSALEWT").HeaderText = "SALES"
        gridviewHead.Columns("PURWT~PURTOUCH~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~TOTALPURWT").HeaderText = "PURCHASE"
        'gridviewHead.Columns("PURWT~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~TOTALPURWT").DefaultCellStyle.BackColor = Color.LavenderBlush
        gridviewHead.Columns("PROFITWT~PROFITAMT").HeaderText = "PROFIT"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        SetGridHeadColWid(gridviewHead)
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("PARTICULAR~SUBITEM~TRANDATE~TRANNO~TAGNO").Width = f.gridView.Columns("PARTICULAR").Width _
                                                    + f.gridView.Columns("SUBITEM").Width _
                                                    + f.gridView.Columns("TRANDATE").Width _
                                                    + f.gridView.Columns("TRANNO").Width _
                                                    + f.gridView.Columns("TAGNO").Width
            .Columns("SALEWT~SALEWASTAGE~SALEMC~STNWT~STNAMT~TOTALSALEWT").Width = f.gridView.Columns("SALEWT").Width _
                                                   + f.gridView.Columns("SALEWASTAGE").Width _
                                                   + f.gridView.Columns("SALEMC").Width _
                                                   + f.gridView.Columns("STNWT").Width _
                                                   + f.gridView.Columns("STNAMT").Width _
                                                   + f.gridView.Columns("TOTALSALEWT").Width
            .Columns("PURWT~PURTOUCH~PURWASTAGE~PURMC~PURSTNWT~PURSTNAMT~TOTALPURWT").Width = f.gridView.Columns("PURWT").Width _
                                                    + f.gridView.Columns("PURTOUCH").Width _
                                                    + f.gridView.Columns("PURWASTAGE").Width _
                                                    + f.gridView.Columns("PURMC").Width _
                                                    + f.gridView.Columns("PURSTNWT").Width _
                                                    + f.gridView.Columns("PURSTNAMT").Width _
                                                    + f.gridView.Columns("TOTALPURWT").Width
            .Columns("PROFITWT~PROFITAMT").Width = f.gridView.Columns("PROFITWT").Width _
                                                    + f.gridView.Columns("PROFITAMT").Width

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            'For Each dgvCol As DataGridViewColumn In dgv.Columns
            '    dgvCol.Visible = True
            '    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            'Next
            FormatGridColumns(dgv, False, , , False)
            .Columns("PARTICULAR").Width = 160
            .Columns("SUBITEM").Width = 150
            .Columns("TRANDATE").Width = 75
            .Columns("TRANNO").Width = 63
            .Columns("TAGNO").Width = 80
            If .Columns.Contains("SALEWT") Then .Columns("SALEWT").HeaderText = "WEIGHT"
            If .Columns.Contains("SALEWASTAGE") Then .Columns("SALEWASTAGE").HeaderText = "WASTAGE"
            If .Columns.Contains("SALEMC") Then .Columns("SALEMC").HeaderText = "MC"
            If .Columns.Contains("TOTALSALEWT") Then .Columns("TOTALSALEWT").HeaderText = "TOTALWT"
            If .Columns.Contains("PURWT") Then .Columns("PURWT").HeaderText = "WEIGHT"
            If .Columns.Contains("PURWASTAGE") Then .Columns("PURWASTAGE").HeaderText = "WASTAGE"
            If .Columns.Contains("PURMC") Then .Columns("PURMC").HeaderText = "MC"
            If .Columns.Contains("TOTALPURWT") Then .Columns("TOTALPURWT").HeaderText = "TOTALWT"
            If .Columns.Contains("PROFITWT") Then .Columns("PROFITWT").HeaderText = "WEIGHT"
            If .Columns.Contains("PROFITAMT") Then .Columns("PROFITAMT").HeaderText = "AMOUNT"
            If .Columns.Contains("PROFITPER") Then .Columns("PROFITPER").HeaderText = "%"
            If .Columns.Contains("PCS") Then .Columns("PCS").Width = 60
            If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Width = 90
            If .Columns.Contains("NETWT") Then .Columns("NETWT").Width = 90
            If .Columns.Contains("DIAPCS") Then .Columns("DIAPCS").Width = 60
            If .Columns.Contains("DIAWT") Then .Columns("DIAWT").Width = 90
            If .Columns.Contains("DIAAMT") Then .Columns("DIAAMT").Width = 90
            If .Columns.Contains("SALVALUE") Then .Columns("SALVALUE").Width = 120
            If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").Width = 120
            If .Columns.Contains("DIFFVALUE") Then .Columns("DIFFVALUE").Width = 120
            If .Columns.Contains("SALVALUE") Then .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("DIFFVALUE") Then .Columns("DIFFVALUE").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PURSTNAMT") Then .Columns("PURSTNAMT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("SALEMC") Then .Columns("SALEMC").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PURMC") Then .Columns("PURMC").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PROFITAMT") Then .Columns("PROFITAMT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PROFITPER") Then .Columns("PROFITPER").DefaultCellStyle.Format = "0.00"

            If .Columns.Contains("PURWASTPER") Then .Columns("PURWASTPER").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("PURWASTPER") Then .Columns("PURWASTPER").HeaderText = "WAST %"

            If .Columns.Contains("WASTPER") Then .Columns("WASTPER").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("WASTPER") Then .Columns("WASTPER").HeaderText = "WAST %"


            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            For cnt As Integer = 20 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            If cmbGroupBy.Text = "NONE" Then
                .Columns("PARTICULAR").HeaderText = "ITEM"
                .Columns("SUBITEM").Visible = True
            Else
                .Columns("PARTICULAR").HeaderText = "PARTICULAR"
                .Columns("SUBITEM").Visible = False
            End If
            If .Columns.Contains("PROFITAMOUNT") Then .Columns("PROFITAMOUNT").Visible = True
            If .Columns.Contains("PROFITWEIGHT") Then .Columns("PROFITWEIGHT").Visible = True
            If .Columns.Contains("PROFITPER") Then .Columns("PROFITPER").Visible = True
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub



    Private Sub Prop_Gets()
        Dim obj As New TagWiseProfitLoss_Properties
        GetSettingsObj(obj, Me.Name, GetType(TagWiseProfitLoss_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        cmbGroupBy.Text = obj.p_cmbGroupBy
        chkFormat1.Checked = obj.p_ChkFormat1
        chkFormat2.Checked = obj.p_ChkFormat2
        chkGP.Checked = obj.p_chkGP
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New TagWiseProfitLoss_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_cmbGroupBy = cmbGroupBy.Text
        obj.p_ChkFormat1 = chkFormat1.Checked
        obj.p_ChkFormat2 = chkFormat2.Checked
        obj.p_chkGP = chkGP.Checked
        SetSettingsObj(obj, Me.Name, GetType(TagWiseProfitLoss_Properties))
    End Sub


    Private Sub chkLstCostCentre_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.SelectedIndexChanged

    End Sub

    Private Sub chkFormat2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFormat2.CheckedChanged, chkFormat1.CheckedChanged
        If chkFormat2.Checked Then
            chkPurchaseWt.Visible = True
        Else
            chkPurchaseWt.Visible = False
        End If
        comboxAddLoad(False)
    End Sub

    Private Sub rbtFormat3_CheckedChanged(sender As Object, e As EventArgs) Handles rbtFormat3.CheckedChanged
        comboxAddLoad(True)
        If rbtFormat3.Checked Then
            chkGP.Enabled = True
        Else
            chkGP.Enabled = False
        End If
    End Sub
End Class
Public Class TagWiseProfitLoss_Properties
    Private chkGP As Boolean = False
    Public Property p_chkGP() As Boolean
        Get
            Return chkGP
        End Get
        Set(ByVal value As Boolean)
            chkGP = value
        End Set
    End Property


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
    Private cmbGroupBy As String = "NONE"
    Public Property p_cmbGroupBy() As String
        Get
            Return cmbGroupBy
        End Get
        Set(ByVal value As String)
            cmbGroupBy = value
        End Set
    End Property
    Private ChkFormat1 As Boolean = True
    Public Property p_ChkFormat1() As Boolean
        Get
            Return ChkFormat1
        End Get
        Set(ByVal value As Boolean)
            ChkFormat1 = value
        End Set
    End Property
    Private ChkFormat2 As Boolean = False
    Public Property p_ChkFormat2() As Boolean
        Get
            Return ChkFormat2
        End Get
        Set(ByVal value As Boolean)
            ChkFormat2 = value
        End Set
    End Property
End Class