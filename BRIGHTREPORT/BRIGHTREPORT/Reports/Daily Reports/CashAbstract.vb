Imports System.Data.OleDb
Imports System.Runtime.Serialization.Formatters.Soap
Public Class CashAbstract
    '03 SHERIFF 25-10-12
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim CASHABS_MI_VAL As Boolean = IIf(GetAdmindbSoftValue("CASHABS_MI_VAL", "Y") = "Y", True, False)
    Dim CASHABSTRACT_CUSTOM As Boolean = IIf(GetAdmindbSoftValue("CASHABSTRACT_CUSTOM", "N") = "Y", True, False)


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        'chkCompanySelectAll.Checked = False
        'chkCostCentreSelectAll.Checked = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        If CASHABSTRACT_CUSTOM = False Then
            chkChitInfo.Checked = IIf(GetAdmindbSoftValue("CHITDB", "N") = "Y", True, False)
        End If
        chkCashOpening.Checked = _CashOpening
        chkWithCatgoryGroup.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub CashAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub CashAbstract_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If

        chkLstCashCounter.Enabled = True
        chkLstCashCounter.Items.Clear()
        strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        Dim dtcash As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcash)
        For i As Integer = 0 To dtcash.Rows.Count - 1
            chkLstCashCounter.Items.Add(dtcash.Rows(i).Item("CASHNAME").ToString, False)
        Next

        LoadCompany(chkLstCompany)
        cmbGroupBy.Items.Add("CATEGORY")
        cmbGroupBy.Items.Add("ITEMGROUP")
        cmbGroupBy.Items.Add("COUNTER")
        cmbGroupBy.Items.Add("TRANNO")
        cmbGroupBy.Text = "CATEGORY"

        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '03
        Dim MiscIssue As String = Nothing
        '03
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkChitInfo.Checked Then
            strSql = " SELECT name cnt FROM MASTER.DBO.SYSDATABASES WHERE NAME = '" & cnChitTrandb & "'"
            If Not objGPack.DupCheck(strSql) Then MsgBox("SCHEME Database Not Found", MsgBoxStyle.Information) : Exit Sub
        End If
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If

        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)

        Dim SELECTEDCASHCOUNTER = GetChecked_CheckedList(chkLstCashCounter, False)
        If chkLstCashCounter.CheckedItems.Count = chkLstCashCounter.Items.Count Then
            SELECTEDCASHCOUNTER = "ALL"
        End If
        '03 
        If chkMiscIssue.Checked Then
            MiscIssue = "Y"
        ElseIf chkMiscIssue.Checked = False Then
            MiscIssue = "N"
        End If
        '03
        Dim _Tcnt As Integer = 0
        Dim _SYSTEMID As String = systemId.ToString
        Try
XXX:
            If cmbGroupBy.Text <> "ITEMGROUP" Then
                If CASHABSTRACT_CUSTOM Then
                    strSql = " EXEC " & cnStockDb & "..SP_CASHABSTRACT_CUSTOM"
                Else
                    strSql = " EXEC " & cnStockDb & "..SP_CASHABSTRACT_NEW"
                    _SYSTEMID = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                End If
                strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
                strSql += vbCrLf + " ,@NODEID = '" & txtSystemId.Text & "'"
                strSql += vbCrLf + " ,@SYSTEMID = '" & _SYSTEMID.ToString & "'"
                strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
                strSql += vbCrLf + " ,@CASHOPENING = '" & IIf(chkCashOpening.Checked, "Y", "N") & "'"
                strSql += vbCrLf + " ,@CHITDBEXSISTS = '" & IIf(chkChitInfo.Checked, "Y", "N") & "'"
                If chksubitem.Checked = True Then
                    strSql += vbCrLf + " ,@SUBITEM = 'Y'"
                Else
                    strSql += vbCrLf + " ,@SUBITEM = 'N'"
                End If
                strSql += vbCrLf + " ,@GROUPBY = '" & cmbGroupBy.Text & "'"
                'strSql += vbCrLf + " ,@GROUPBYCOUNTER = " & IIf(cmbGroupBy.Text = "COUNTER", "'Y'", "'N'")
                '03 
                strSql += vbCrLf + " ,@MISCISSUE='" & MiscIssue & "'"
                strSql += vbCrLf + " ,@MISCVALUE='" & IIf(CASHABS_MI_VAL, "Y", "N") & "'"
                strSql += vbCrLf + " ,@BANKDET = '" & IIf(chkBank.Checked, "Y", "N") & "'"
                strSql += vbCrLf + " ,@WITHGROUPBYCATEGORY = '" & IIf(chkWithCatgoryGroup.Checked, "Y", "N") & "'"
            ElseIf cmbGroupBy.Text = "ITEMGROUP" Then
                strSql = " EXEC " & cnStockDb & "..SP_CASHABSTRACTGROUP_NEW"
                strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
                strSql += vbCrLf + " ,@NODEID = '" & txtSystemId.Text & "'"
                strSql += vbCrLf + " ,@SYSTEMID = '" & _SYSTEMID.ToString & "'"
                strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
                strSql += vbCrLf + " ,@CASHOPENING = '" & IIf(chkCashOpening.Checked, "Y", "N") & "'"
                strSql += vbCrLf + " ,@CHITDBEXSISTS = '" & IIf(chkChitInfo.Checked, "Y", "N") & "'"
                If chksubitem.Checked = True Then
                    strSql += vbCrLf + " ,@SUBITEM = 'Y'"
                Else
                    strSql += vbCrLf + " ,@SUBITEM = 'N'"
                End If
            End If
            strSql += vbCrLf + " ,@PARTLY = '" & IIf(ChkPartBreakup.Checked, "Y", "N") & "'"
            If cmbGroupBy.Text <> "ITEMGROUP" Then
                strSql += vbCrLf + " ,@GRSNET = '" & IIf(rbtGrswt.Checked, "G", "N") & "'"
                strSql += vbCrLf + " ,@WITHOUTDISC = '" & IIf(chkWithDisc.Checked, "Y", "N") & "'"
                strSql += vbCrLf + " ,@BANKOPENING = '" & IIf(chkBankOpening.Checked = True, "Y", "N") & "'"
                strSql += vbCrLf + " ,@CASHNAME = '" & SELECTEDCASHCOUNTER & "'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            _Tcnt = _Tcnt + 1
            If _Tcnt > 6 Then
                MsgBox("Predefined conditions/dbs are un matched", MsgBoxStyle.Information) : Exit Sub
            Else
                GoTo XXX
            End If
        End Try

        If cmbGroupBy.Text = "CATEGORY" And ChkMetalGrp.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY(PARTICULAR,ITEM,CATNAME,RESULT,DISPORDER,COLHEAD)"
            strSql += vbCrLf + " Select 'METAL','ZZZ','ZZZ',1,.5,'T'"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY(PARTICULAR,ITEM,CATNAME,RESULT,DISPORDER,GRSWT,NETWT)"
            strSql += vbCrLf + " SELECT DISTINCT CATNAME, 'ZZZ','ZZZ',1,1,SUM(GRSWT),SUM(NETWT) FROM("
            strSql += vbCrLf + " SELECT DISTINCT (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID="
            strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY  WHERE  CATNAME=T.CATNAME))CATNAME"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(GRSAMOUNT)GRSAMOUNT"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY T WHERE T.CATNAME IS NOT NULL AND DISPORDER IS NULL AND RESULT=1"
            strSql += vbCrLf + " AND ISNULL(STUDDED,'')<>'Y' "
            strSql += vbCrLf + " GROUP BY CATNAME)X GROUP BY CATNAME"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY(PARTICULAR,ITEM,CATNAME,RESULT,DISPORDER,GRSWT,NETWT,GRSAMOUNT)"
            strSql += vbCrLf + " SELECT DISTINCT 'STONE / DIAMOND' CATNAME, 'ZZZ','ZZZ',1,1,SUM(GRSWT),SUM(NETWT),SUM(GRSAMOUNT)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY T WHERE T.CATNAME IS NOT NULL AND DISPORDER IS NULL AND RESULT=1 AND ISNULL(STUDDED,'')='Y' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        If cmbGroupBy.Text = "CATEGORY" Then
            strSql = " SELECT * FROM " & tempdbname & "..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY ORDER BY DISPORDER,CATNAME,ITEM,RESULT,STUDDED"
        ElseIf cmbGroupBy.Text = "TRANNO" Then
            strSql = " SELECT * FROM " & tempdbname & "..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY ORDER BY DISPORDER,CATNAME,ITEM,RESULT,STUDDED"
        ElseIf cmbGroupBy.Text = "COUNTER" Then
            strSql = " SELECT * FROM " & tempdbname & "..TEMP" & _SYSTEMID.ToString & "CASHSUMMARY ORDER BY DISPORDER,COUNTERNAME,RESULT,CATNAME,ITEM,SALRATE,STUDDED,SUBITEM"
        ElseIf cmbGroupBy.Text = "ITEMGROUP" Then
            strSql = " SELECT * FROM " & tempdbname & "..TEMPGRP" & _SYSTEMID.ToString & "CASHSUMMARY ORDER BY DISPORDER,GROUPNAME,ITEM,RESULT,STUDDED"
        End If

        ' strSql = " SELECT * FROM MASTER..TEMP" & systemId & "CASHSUMMARY ORDER BY DISPORDER,CATNAME,ITEM,RESULT,STUDDED"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
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
        objGridShower.Text = "CASH ABSTRACT"
        Dim tit As String = "CASH ABSTRACT FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        If txtSystemId.Text.ToString <> "" Then tit += " NODE - " + txtSystemId.Text.ToString
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
        objGridShower.formuser = userId
        objGridShower.Show()
        Dim Salrate As Decimal = 0
        Dim Diffrate As Decimal = 0
        Dim RatePer As Decimal = 0
        Dim Count As Integer = 0
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            With dgvRow


                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = Color.Green
                        .Cells("PARTICULAR").Style.ForeColor = Color.White
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "C"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "I"
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.ForeColor = Color.Blue
                    Case "G"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.BackColor = Color.LightYellow
                        .DefaultCellStyle.ForeColor = Color.Black
                End Select
                If cmbGroupBy.Text <> "ITEMGROUP" Then
                    Select Case .Cells("RESULT").Value.ToString
                        Case 1
                            Salrate += Val(.Cells("SALRATE").Value.ToString)
                            RatePer += Val(.Cells("RATEPER").Value.ToString)
                            Diffrate += Val(.Cells("DIFFNETWT").Value.ToString)
                            Count += 1
                            If Val(.Cells("DISPORDER").Value.ToString) = 2 Then
                                If Val(.Cells("GRSWT").Value.ToString) <> 0 And rbtGrswt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT").Value.ToString) / Val(.Cells("GRSWT").Value.ToString), "0.00")
                                If Val(.Cells("NETWT").Value.ToString) <> 0 And rbtNetwt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT").Value.ToString) / Val(.Cells("NETWT").Value.ToString), "0.00")
                            End If
                        Case 2
                            If Salrate <> 0 And Count <> 0 Then
                                If Val(.Cells("GRSWT").Value.ToString) <> 0 And rbtGrswt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT").Value.ToString) / Val(.Cells("GRSWT").Value.ToString), "0.00")
                                If Val(.Cells("NETWT").Value.ToString) <> 0 And rbtNetwt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT").Value.ToString) / Val(.Cells("NETWT").Value.ToString), "0.00")
                                If RatePer <> 0 Then .Cells("RATEPER").Value = Format((RatePer / Count), "0.00")
                                If Diffrate <> 0 Then .Cells("DIFFNETWT").Value = Format((Diffrate / Count), "0.00")
                                RatePer = 0
                                Count = 0
                                Salrate = 0
                                Diffrate = 0
                            End If
                            'If Val(.Cells("DISPORDER").Value.ToString) = 2 Then
                            '    If Val(.Cells("GRSWT").Value.ToString) <> 0 And rbtGrswt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT".ToString).Value) / Val(.Cells("GRSWT").Value.ToString), "0.00")
                            '    If Val(.Cells("NETWT").Value.ToString) <> 0 And rbtNetwt.Checked Then .Cells("SALRATE").Value = Format(Val(.Cells("AMOUNT".ToString).Value) / Val(.Cells("NETWT").Value.ToString), "0.00")
                            'End If
                    End Select
                End If


            End With
        Next


        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            With dgvRow
                If objGridShower.gridView.Columns.Contains("STUDDED") = True Then
                    Select Case .Cells("STUDDED").Value.ToString
                        Case "Y"
                            .DefaultCellStyle.BackColor = Color.Lavender
                    End Select
                End If
                If objGridShower.gridView.Columns.Contains("AMOUNT") = True Then
                    If .Cells("PARTICULAR").Value.ToString.Equals("CASH") Then
                        .Cells("AMOUNT").Style.BackColor = Color.LightGreen
                    End If
                End If
                If objGridShower.gridView.Columns.Contains("AMOUNT") = True Then
                    If .Cells("PARTICULAR").Value.ToString.Equals("OLD GOLD") Then
                        .Cells("AMOUNT").Style.BackColor = Color.LightGreen
                    End If
                End If
            End With
        Next


        objGridShower.gridView.Columns("RESULT").Visible = False
        objGridShower.gridView.Columns("ITEM").Visible = False
        objGridShower.gridView.Columns("SUBITEM").Visible = False
        If objGridShower.gridView.Columns.Contains("DISPORDER") Then objGridShower.gridView.Columns("DISPORDER").Visible = False
        If objGridShower.gridView.Columns.Contains("GROUPNAME") Then objGridShower.gridView.Columns("GROUPNAME").Visible = False

        '  FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Prop_Sets()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PARTICULAR").Width = 300
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("AMOUNT").Width = 100
            .Columns("SALRATE").Width = 100
            .Columns("DIFFRATE").Width = 100
            If .Columns.Contains("DISCOUNT") Then
                If chkWithDisc.Checked = False Then
                    .Columns("DISCOUNT").Visible = False
                    If .Columns.Contains("FIN_DISCOUNT") Then
                        .Columns("FIN_DISCOUNT").Visible = False
                        .Columns("GRSAMOUNT").Visible = False
                    End If
                Else
                    If .Columns.Contains("FIN_DISCOUNT") Then
                        .Columns("FIN_DISCOUNT").HeaderText = "B4DISCOUNT"
                        .Columns("GRSAMOUNT").HeaderText = "AMOUNT"
                        .Columns("AMOUNT").HeaderText = "TOTAL"
                    End If
                End If
            End If
            For cnt As Integer = 14 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            FormatGridColumns(dgv, False, False, , False)
            If .Columns.Contains("PURVALUE") Then
                .Columns("PURVALUE").Visible = Not chksubitem.Checked
            End If
            If .Columns.Contains("DIFFVALUE") Then
                .Columns("DIFFVALUE").Visible = Not chksubitem.Checked
            End If
            If .Columns.Contains("VALUEPER") Then
                .Columns("VALUEPER").Visible = Not chksubitem.Checked
                .Columns("VALUEPER").HeaderText = "VALUE %"
            End If
            .Columns("RATEPER").Visible = True
            .Columns("RATEPER").HeaderText = "RATE %"
            .Columns("AVGRATE").Visible = False
            .Columns("SALRATE").Visible = True
            If rbtGrswt.Checked Then
                .Columns("DIFFRATE").Visible = True
                .Columns("DIFFRATE").HeaderText = "DIFFRATE"
                .Columns("DIFFNETWT").Visible = False
            ElseIf rbtNetwt.Checked Then
                .Columns("DIFFNETWT").Visible = True
                .Columns("DIFFNETWT").HeaderText = "DIFFRATE"
                .Columns("DIFFRATE").Visible = False
            End If
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New CashAbstract_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMiscIssue = chkMiscIssue.Checked
        obj.p_txtSystemId = txtSystemId.Text
        obj.p_chkCashOpening = chkCashOpening.Checked
        obj.p_chkChitInfo = chkChitInfo.Checked
        obj.p_chkPartBreakup = ChkPartBreakup.Checked
        obj.p_chkWithDisc = chkWithDisc.Checked
        obj.p_ChkMetalGrp = ChkMetalGrp.Checked
        obj.p_chkBank = chkBank.Checked
        obj.p_chksubitem = chksubitem.Checked
        obj.p_rbtGrswt = rbtGrswt.Checked
        obj.p_rbtNetwt = rbtNetwt.Checked
        SetSettingsObj(obj, Me.Name, GetType(CashAbstract_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New CashAbstract_Properties
        GetSettingsObj(obj, Me.Name, GetType(CashAbstract_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        txtSystemId.Text = obj.p_txtSystemId
        chkMiscIssue.Checked = obj.p_chkMiscIssue
        chkCashOpening.Checked = obj.p_chkCashOpening
        chkChitInfo.Checked = obj.p_chkChitInfo
        ChkPartBreakup.Checked = obj.p_chkPartBreakup
        chkWithDisc.Checked = obj.p_chkWithDisc
        ChkMetalGrp.Checked = obj.p_ChkMetalGrp
        chkBank.Checked = obj.p_chkBank
        chksubitem.Checked = obj.p_chksubitem
        rbtGrswt.Checked = obj.p_rbtGrswt
        rbtNetwt.Checked = obj.p_rbtNetwt

    End Sub

    Private Sub cmbGroupBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroupBy.SelectedIndexChanged
        If cmbGroupBy.Text = "CATEGORY" Then ChkMetalGrp.Visible = True Else ChkMetalGrp.Visible = False
        If cmbGroupBy.Text = "COUNTER" Then chkWithCatgoryGroup.Visible = True Else chkWithCatgoryGroup.Visible = False : chkWithCatgoryGroup.Checked = False
    End Sub

    Private Sub chkCashCounter_CheckedChanged(sender As Object, e As EventArgs) Handles chkCashCounterAll.CheckedChanged
        SetChecked_CheckedList(chkLstCashCounter, chkCashCounterAll.Checked)
    End Sub
End Class
Public Class CashAbstract_Properties
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
    Private chkMiscIssue As Boolean = True
    Public Property p_chkMiscIssue() As Boolean
        Get
            Return chkMiscIssue
        End Get
        Set(ByVal value As Boolean)
            chkMiscIssue = value
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
    Private txtSystemId As String = ""
    Public Property p_txtSystemId() As String
        Get
            Return txtSystemId
        End Get
        Set(ByVal value As String)
            txtSystemId = value
        End Set
    End Property
    Private chkCashOpening As Boolean = False
    Public Property p_chkCashOpening() As Boolean
        Get
            Return chkCashOpening
        End Get
        Set(ByVal value As Boolean)
            chkCashOpening = value
        End Set
    End Property
    Private chkChitInfo As Boolean = False
    Public Property p_chkChitInfo() As Boolean
        Get
            Return chkChitInfo
        End Get
        Set(ByVal value As Boolean)
            chkChitInfo = value
        End Set
    End Property
    Private chkPartBreakup As Boolean = False
    Public Property p_chkPartBreakup() As Boolean
        Get
            Return chkPartBreakup
        End Get
        Set(ByVal value As Boolean)
            chkPartBreakup = value
        End Set
    End Property
    Private chkBank As Boolean = True
    Public Property p_chkBank() As Boolean
        Get
            Return chkBank
        End Get
        Set(ByVal value As Boolean)
            chkBank = value
        End Set
    End Property
    Private chksubitem As Boolean = True
    Public Property p_chksubitem() As Boolean
        Get
            Return chksubitem
        End Get
        Set(ByVal value As Boolean)
            chksubitem = value
        End Set
    End Property
    Private ChkMetalGrp As Boolean = True
    Public Property p_ChkMetalGrp() As Boolean
        Get
            Return ChkMetalGrp
        End Get
        Set(ByVal value As Boolean)
            ChkMetalGrp = value
        End Set
    End Property
    Private chkWithDisc As Boolean = True
    Public Property p_chkWithDisc() As Boolean
        Get
            Return chkWithDisc
        End Get
        Set(ByVal value As Boolean)
            chkWithDisc = value
        End Set
    End Property
    Private rbtGrswt As Boolean = True
    Public Property p_rbtGrswt() As Boolean
        Get
            Return rbtGrswt
        End Get
        Set(ByVal value As Boolean)
            rbtGrswt = value
        End Set
    End Property
    Private rbtNetwt As Boolean = True
    Public Property p_rbtNetwt() As Boolean
        Get
            Return rbtNetwt
        End Get
        Set(ByVal value As Boolean)
            rbtNetwt = value
        End Set
    End Property
End Class