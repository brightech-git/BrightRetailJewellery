Imports System.Data.OleDb
Public Class frmNewCustomerOutstanding
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim SelectedCompany As String
    Dim dtItemType As New DataTable
    Dim flagHighlight As Boolean = IIf(GetAdmindbSoftValue("RPT_COLOR", "N") = "Y", True, False)

    Private Sub frmCustomerOutstanding_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCompany(chkLstCompany)
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
        grpOutStanding.Visible = True
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        'dtpFrom.Focus()
        'dtpFrom.Select()
        ChkAsOn.Focus()
        ChkAsOn.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
    'Function GridViewFormat() As Integer
    '    For Each dgvRow As DataGridViewRow In gridView.Rows
    '        With dgvRow
    '            Select Case .Cells("COLHEAD").Value.ToString
    '                Case "G"
    '                    .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    '                    .DefaultCellStyle.Font = reportTotalStyle.Font
    '                Case "T"
    '                    .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    '                    .DefaultCellStyle.Font = reportTotalStyle.Font
    '                Case "S"
    '                    .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    '                    .DefaultCellStyle.Font = reportTotalStyle.Font
    '            End Select
    '        End With
    '    Next
    'End Function
    Function CustStoredProcedure() As Integer
        Dim dtCusDetail As New DataTable
        dtCusDetail.Clear()
        Dim dtCusSummary As New DataTable
        dtCusSummary.Clear()
        gridView.DataSource = Nothing
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        Me.Refresh()
        Dim SummaryDetail As String = Nothing
        If rbtSummaryWise.Checked = True Then
            SummaryDetail = "S"
        ElseIf rbtDetailWise.Checked = True Then
            SummaryDetail = "D"
        End If

        Dim Balance As String = Nothing
        If rbtPending.Checked = True Then
            Balance = "P"
        ElseIf rbtClosed.Checked = True Then
            Balance = "C"
        ElseIf rbtAll.Checked = True Then
            Balance = "B"
        End If

        Dim Order As String = Nothing
        If rbtORunNo.Checked = True Then
            Order = "R"
        ElseIf rbtOBillDate.Checked = True Then
            Order = "D"
        ElseIf rbtOName.Checked = True Then
            Order = "N"
        End If

        Dim TranId As String = ""
        If chkAdvance.Checked Then TranId += "A,"
        If chkOrder.Checked Then TranId += "O,"
        If chkCredit.Checked Then TranId += "D,"
        If chkGeneral.Checked Then TranId += "T,"
        If TranId <> "" Then TranId = Mid(TranId, 1, TranId.Length - 1) Else TranId = "A,D,T,O"


        'If chkAdvance.Checked = True And chkOrder.Checked = True And chkGeneral.Checked = True Then
        '    TranId = "A,T,D"
        'ElseIf chkAdvance.Checked = True And chkOrder.Checked = True Then
        '    TranId = "A,T"
        'ElseIf chkAdvance.Checked = True And chkGeneral.Checked = True Then
        '    TranId = "A,D"
        'ElseIf chkOrder.Checked = True And chkGeneral.Checked = True Then
        '    TranId = "T,D"
        'ElseIf chkAdvance.Checked = True Then
        '    TranId = "A"
        'ElseIf chkOrder.Checked = True Then
        '    TranId = "T"
        'ElseIf chkGeneral.Checked = True Then
        '    TranId = "D"
        'End If

        Dim FilterName As String = Nothing
        If cmbFilterBy.Text = "NONE" Then
            FilterName = ""
        Else
            FilterName = Replace(cmbFilterBy.Text, "'", "''''")
        End If

        strSql = " EXEC " & cnStockDb & "..SP_RPT_CUSTOUTSTANDING"
        strSql += vbCrLf + " @DATEFROM = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@DATETO = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@SummaryDetail= '" & SummaryDetail & "'"
        strSql += vbCrLf + ",@Balance='" & Balance & "'"
        strSql += vbCrLf + ",@ORDERBY ='" & Order & "'"
        strSql += vbCrLf + ",@COSTCENTRE ='" & Replace(cmbCostCentre.Text, "'", "''''") & "'"
        strSql += vbCrLf + ",@FILTERBY ='" & FilterName & "'"
        strSql += vbCrLf + ",@FILTERCAPTION ='" & txtFilterCaption.Text & "'"
        strSql += vbCrLf + ",@NODEID='" & txtNodeId.Text & "'"
        strSql += vbCrLf + ",@TYPEID='" & TranId & "'"
        strSql += vbCrLf + ",@SystemId='" & systemId & "'"
        strSql += vbCrLf + ",@COMPANYID='" & SelectedCompany & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtDetailWise.Checked = True Then
            'Final Query
            strSql = "SELECT RUNNO PARTICULAR,TRANNO AS BILLNO,convert(varchar,TranDate,103) as BILLDATE,CUSTNAME"
            strSql += ",(CASE WHEN DEBIT = 0 THEN NULL ELSE DEBIT END) DEBIT,(CASE WHEN CREDIT = 0 THEN NULL ELSE CREDIT END) CREDIT,PHONE,MOBILE,SALESPERSON,COSTNAME,COLHEAD  FROM MASTER..TEMPFINAL "
            If rbtOBillDate.Checked = True Then
                strSql += " ORDER BY TRANTYPE,RESULT,TRANDATE"
            ElseIf rbtOName.Checked = True Then
                strSql += " ORDER BY TRANTYPE,RESULT,CUSTNAME"
            ElseIf rbtORunNo.Checked = True Then
                strSql += " ORDER BY TRANTYPE,RESULT,RUNNO"
            End If


            gridView.DataSource = Nothing
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(String))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            Dim da1 As New OleDbDataAdapter
            da1 = New OleDbDataAdapter(strSql, cn)
            da1.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Function
            End If
            tabView.Show()
            gridView.DataSource = dtGrid
            FillGridGroupStyle_KeyNoWise(gridView)
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("KEYNO").Visible = False
            gridView.Focus()
        ElseIf rbtSummaryWise.Checked = True Then
            strSql = "SELECT RUNNO PARTICULAR,CUSTNAME"
            strSql += ",DEBIT,CREDIT,BALANCE,PHONE,MOBILE,SALESPERSON,COSTNAME  FROM MASTER..TEMP" & systemId & "SUMFINAL "
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCusDetail)
            gridView.DataSource = dtCusDetail
        End If

        funcDetGridStyle()
        If gridView.Rows.Count < 1 Then
            btnView_Search.Enabled = True
            gridView.DataSource = Nothing
            MessageBox.Show("Records not found ..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Function
        End If
        If gridView.Rows.Count > 0 Then
            lblTitle.Visible = True
            Dim strTitle As String = Nothing
            strTitle = " CUSTOMEROUTSTANDING REPORT"
            If rbtDetailWise.Checked = True Then
                strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            ElseIf rbtSummaryWise.Checked = True Then
                strTitle += " TILL " & dtpFrom.Text & ""
            End If
            If (cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "") Or (txtNodeId.Text <> "ALL" And txtNodeId.Text <> "") Then
                strTitle += " FOR"
            End If
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strTitle += " " & cmbCostCentre.Text & " AND"
            End If
            If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
                strTitle += " " & txtNodeId.Text & " "
            End If
            If Strings.Right(strTitle, 3) = "AND" Then
                strTitle = Strings.Left(strTitle, strTitle.Length - 3)
            End If
            If rbtPending.Checked = True Then
                strTitle += "(PENDING"
            ElseIf rbtClosed.Checked = True Then
                strTitle += "(CLOSED"
            ElseIf rbtAll.Checked = True Then
                strTitle += "(PENDING AND CLOSED"
            End If
            strTitle += " WITH "
            If rbtSummaryWise.Checked = True Then
                strTitle += " SUMMARYWISE) "
            ElseIf rbtDetailWise.Checked = True Then
                strTitle += " DETAILWISE) "
            End If
            lblTitle.Text = strTitle
            lblTitle.Height = gridView.ColumnHeadersHeight
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        End If
    End Function

    Public Function GetSelectedAcGroup(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Private Sub Report()
        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        If Not chkAdvance.Checked And
        Not chkCredit.Checked And
        Not chkOrder.Checked And
        Not chkGeneral.Checked And
        Not chkToBe.Checked And Not chkCRPur.Checked Then
            chkAdvance.Checked = True
            chkCredit.Checked = True
            chkOrder.Checked = True
            chkGeneral.Checked = True
            chkToBe.Checked = True
            chkCRPur.Checked = True
        End If
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP_OUTSTANDING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUTSTANDING          "
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP_CROUTSTANDING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_CROUTSTANDING          "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        Me.Refresh()
        Dim Display As String
        Dim OrderBy As String
        Dim Type As String = ""
        Dim actype As String = "N"
        If rbtPending.Checked Then
            Display = "P"
        ElseIf rbtClosed.Checked Then
            Display = "C"
        Else
            Display = "A"
        End If
        If rbtOName.Checked Then
            OrderBy = "N"
        ElseIf rbtOBillDate.Checked Then
            OrderBy = "B"
        Else
            OrderBy = "R"
        End If
        If chkAdvance.Checked Then Type += "A,"
        If chkCredit.Checked Then Type += "D,"
        If chkOrder.Checked Then Type += "O,"
        If chkGeneral.Checked Then Type += "T,"
        If chkToBe.Checked Then Type += "J,"
        If chkCRPur.Checked Then Type += "C,"

        If chkacfilter.Checked Then
            If rbtdrs.Checked Then actype = "D"
            If rbtadvance.Checked Then actype = "A"
            If rbtorder.Checked Then actype = "O"
        End If

        If chkacfilter.Checked = False And chkCredit.Checked = True Then
            actype = "D"
        End If
        If Type.Length > 1 Then
            actype = ""
        End If

        Type.Remove(Type.Length - 1, 1)
        Dim SysId As String = systemId
        If chkBasedOnAccode.Checked Then
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_NEWCUSTOMEROUTSTANDING_ACCODE"
        Else
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_NEWCUSTOMEROUTSTANDING"
        End If
        strSql += vbCrLf + " @DBNAME= '" & cnStockDb & "'"
        'strSql += vbCrLf + " ,@FROMDATE = '" & IIf(rbtDetailWise.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), "1900-01-01") & "'"
        'strSql += vbCrLf + " ,@TODATE = '" & IIf(rbtDetailWise.Checked, dtpTo.Value.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & IIf(ChkAsOn.Checked, "1900-01-01", dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@TODATE = '" & IIf(ChkAsOn.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@DISPLAY = '" & Display & "'"
        strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSummaryWise.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ORDERBY = '" & OrderBy & "'"
        strSql += vbCrLf + " ,@FILTERBY = '" & cmbFilterBy.Text & "'"
        strSql += vbCrLf + " ,@FILTERCAPTION = '" & txtFilterCaption.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & cmbCostCentre.Text & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        strSql += vbCrLf + " ,@TYPEID = '" & Type & "'"
        If chkBasedOnAccode.Checked = False Then
            strSql += vbCrLf + " ,@GRPAREA = '" & IIf(chkGroupByArea.Checked, "Y", "N") & "'"
        End If
        strSql += vbCrLf + " ,@ACFILTER = '" & actype & "'"
        If chkBasedOnAccode.Checked Then
            strSql += vbCrLf + " ,@SYSID = '" & SysId & "'"
        Else
            strSql += vbCrLf + " ,@SYSID = '" & SysId & "'"
        End If
        If chkCmbAcGrp.Text = "ALL" Then
            strSql += vbCrLf + " ,@ACGROUP = 'ALL'"
        Else
            strSql += vbCrLf + " ,@ACGROUP = '" & GetSelectedAcGroup(chkCmbAcGrp, False) & "'"
        End If
        If chkBasedOnAccode.Checked = False Then
            strSql += vbCrLf + " ,@ALLCODE = '" & IIf(rbtAllCode.Checked, "Y", "N") & "'"
        End If

        If cmbAcName.Text = "ALL" Then
            strSql += vbCrLf + " ,@ACNAME = 'ALL'"
        Else
            strSql += vbCrLf + " ,@ACNAME = '" & cmbAcName.Text & "'"
        End If
        If chkBasedOnAccode.Checked = False Then
            If chkOrder.Checked Then
                strSql += vbCrLf + " ,@ORDERSUMMARY = '" & IIf(chkOrderSummary.Checked, "Y", "N") & "'"
            Else
                strSql += vbCrLf + " ,@ORDERSUMMARY = 'N'"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkBasedOnAccode.Checked = False Then SysId = ""
        If flagHighlight And rbtSummaryWise.Checked And chkBasedOnAccode.Checked = False Then
            strSql = "ALTER TABLE TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ADD DIFF INT"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "UPDATE TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " SET DIFF=DATEDIFF(DD,TRANDATE,'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "') WHERE COLHEAD IS NULL"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        If chkBasedOnAccode.Checked = False And chkGroupByArea.Checked Then
            If OrderBy = "N" Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,AREA,AREAord,NAME"
            ElseIf OrderBy = "B" Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,AREA,AREAord,TRANDATE"
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,AREA,AREAord,RUNNO"
            End If
        Else
            If OrderBy = "N" Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,NAME,CTRANNO"
            ElseIf OrderBy = "B" Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,TRANDATE,TRANNO,CTRANDATE,CTRANNO"
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING" & SysId & " ORDER BY TRANTYPE,RESULT,RUNNO,TRANDATE,TRANNO,CTRANDATE,CTRANNO"
            End If
        End If
        gridView.DataSource = Nothing
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Dim da1 As New OleDbDataAdapter
        da1 = New OleDbDataAdapter(strSql, cn)
        da1.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
            dtpFrom.Focus()
            dtpFrom.Select()
        End If
        tabView.Show()
        dtGrid.Columns("TRANNO").SetOrdinal(2)
        dtGrid.Columns("TRANDATE").SetOrdinal(3)
        dtGrid.Columns("DUEDATE").SetOrdinal(4)
        dtGrid.Columns("DEBIT").SetOrdinal(5)
        dtGrid.Columns("CREDIT").SetOrdinal(6)
        dtGrid.Columns("BALANCE").SetOrdinal(7)
        dtGrid.Columns("NAME").SetOrdinal(8)
        dtGrid.Columns("MOBILE").SetOrdinal(9)
        dtGrid.Columns("CTRANNO").SetOrdinal(10)
        dtGrid.Columns("CTRANDATE").SetOrdinal(11)
        gridView.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise(gridView)
        gridformat()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        'gridView.Columns("SALESPERSON").Visible = Not rbtSummaryWise.Checked
        'gridView.Columns("DISCEMPNAME").Visible = Not rbtSummaryWise.Checked
        gridView.Columns("DISCEMPNAME").HeaderText = "AUTHORIZE"
        gridView.Columns("COSTNAME").Visible = True
        gridView.Columns("COSTID").Visible = False
        'gridView.Columns("TRANDATE").Visible = Not rbtSummaryWise.Checked
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        'gridView.Columns("TRANNO").Visible = Not rbtSummaryWise.Checked
        gridView.Columns("CTRANDATE").HeaderText = "Trandate"
        gridView.Columns("CTRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If rbtSummaryWise.Checked = True And chkBasedOnAccode.Checked = True Then
            gridView.Columns("CTRANDATE").Visible = False
            gridView.Columns("CTRANNO").Visible = False
            gridView.Columns("AMOUNT").Visible = False
        End If
        gridView.Columns("CTRANNO").HeaderText = "Tranno"
        gridView.Columns("BALANCE").Visible = rbtSummaryWise.Checked
        gridView.Columns("TRANTYPE").Visible = False
        gridView.Columns("RUNNO").Visible = False
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("COLHEAD").Visible = False
        gridView.Columns("KEYNO").Visible = False
        gridView.Columns("KNO").Visible = False
        If chkBasedOnAccode.Checked = False Then
            gridView.Columns("AREAORD").Visible = False
            gridView.Columns("AREA").Visible = Not chkGroupByArea.Checked
        End If
        If gridView.Rows.Count > 0 Then
            lblTitle.Visible = True
            Dim strTitle As String = Nothing
            strTitle = " CUSTOMEROUTSTANDING DETAIL REPORT"
            If rbtDetailWise.Checked = True Then
                If ChkAsOn.Checked Then
                    strTitle += " ASON " + dtpFrom.Text
                Else
                    strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                End If
            Else
                strTitle += " ASON " + dtpFrom.Text
            End If
            strTitle += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")
            lblTitle.Text = strTitle
            lblTitle.Height = gridView.ColumnHeadersHeight
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        End If
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub
    Private Sub gridformat()
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                If gridView.Columns.Contains("COLHEAD") = True Then
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "G1"
                            .Cells("PARTICULAR").Style.ForeColor = Color.Red
                            .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            .DefaultCellStyle.BackColor = Color.Wheat
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End Select
                End If
                If flagHighlight And rbtSummaryWise.Checked And chkBasedOnAccode.Checked = False Then
                    Select Case IIf(IsDBNull(.Cells("DIFF").Value), 0, .Cells("DIFF").Value)
                        Case 1 To 30
                            .DefaultCellStyle.BackColor = Color.LightGreen
                        Case 31 To 45
                            .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        Case Is >= 46
                            .DefaultCellStyle.BackColor = Color.LightPink
                    End Select
                End If
            End With
        Next
    End Sub


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Prop_Sets()
        Report()
        Exit Sub
        'CustStoredProcedure()
        'Exit Sub
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'chkCompanySelectAll.Checked = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        'rbtDetailWise.Checked = True
        'rbtORunNo.Checked = True
        'rbtPending.Checked = True
        'rbtGCode.Checked = False
        'rbtGName.Checked = False
        'rbtGRunNo.Checked = False
        'grpGroupBy.Enabled = False
        'lblTitle.Visible = False
        'grbRunNoFocus.Visible = False

        chkacfilter.Checked = False
        funcAddCostCentre()
        funcAddAcGroup()
        funAddAcName()
        funcAddDetItem()
        Prop_Gets()
        chkBasedOnAccode_CheckedChanged(Me, e)
        'dtpFrom.Focus()
        ChkAsOn.Focus()
        txtFilterCaption.Clear()
    End Sub

    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function
    Function funcAddAcGroup() As Integer
        strSql = "SELECT DISTINCT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP order by ACGRPNAME"
        cmbAcGrp.Items.Clear()
        cmbAcGrp.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbAcGrp, False, False)
        cmbAcGrp.Text = "ALL"


        strSql = ""
        strSql = " SELECT 'ALL' ACGRPNAME"
        strSql += " UNION ALL"
        strSql += " SELECT DISTINCT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP order by ACGRPNAME"
        dtItemType = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemType)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcGrp, dtItemType, "ACGRPNAME", , "ALL")
    End Function
    Function funAddAcName() As Integer
        strSql = "SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD order by ACNAME"
        cmbAcName.Items.Clear()
        cmbAcName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbAcName, False, False)
        If cmbAcGrp.Text = "ALL" And chkCmbAcGrp.Text = "ALL" Then
            strSql = " SELECT 'ALL' ACNAME"
            strSql += " UNION ALL"
            strSql += " SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD  order by ACNAME"
            cmbAcName.Text = "ALL"
            dtItemType = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemType)
            objGPack.FillCombo(strSql, cmbAcName, True, False)
            cmbAcName.Text = "ALL"
        Else
            GetSelectedAcGroup(chkCmbAcGrp, True)
            strSql = ""
            strSql = " SELECT 'ALL' ACNAME"
            strSql += " UNION ALL"
            strSql += " SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN ( SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetSelectedAcGroup(chkCmbAcGrp, True) & ") ) order by ACNAME"

            dtItemType = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemType)
            objGPack.FillCombo(strSql, cmbAcName, True, False)
            cmbAcName.Text = "ALL"
        End If


    End Function
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            grbRunNoFocus.Visible = False
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        grbRunNoFocus.Visible = False
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Function funcDetGridStyle() As Integer
        With gridView
            If rbtDetailWise.Checked = True Then
                With .Columns("PARTICULAR")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("BILLNO")
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("BILLDATE")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If rbtSummaryWise.Checked = True Then
                If rbtGRunNo.Checked = True Then
                    With .Columns("PARTICULAR")
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
                'If rbtGCode.Checked = True Then
                '    With .Columns("CODE")
                '        .Width = 70
                '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                '        .SortMode = DataGridViewColumnSortMode.NotSortable
                '    End With
                'End If
                'With .Columns("LASTDATE")
                '    .Width = 80
                '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                '    .SortMode = DataGridViewColumnSortMode.NotSortable
                'End With
            End If
            With .Columns("CUSTNAME")
                .Width = 220
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DEBIT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CREDIT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If rbtSummaryWise.Checked = True Then
                With .Columns("BALANCE")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("PHONE")
                .Width = 95
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("MOBILE")
                .Width = 95
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALESPERSON")
                .Width = 160
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub rdbDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetailWise.CheckedChanged
        If rbtDetailWise.Checked = True Then
            'rbtGCode.Checked = False
            'rbtGName.Checked = False
            'rbtGRunNo.Checked = False
            'grpGroupBy.Enabled = False
            'lblDateFrom.Text = "Date From"
            'lblDateTo.Visible = True
            'dtpTo.Visible = True
            funcAddDetItem()
        Else
            'rbtGCode.Checked = True
            'grpGroupBy.Enabled = True
            funcAddSumCodeItem()
        End If
    End Sub

    Private Sub rdbSummaryWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummaryWise.CheckedChanged
        If rbtSummaryWise.Checked = True Then
            'dtpFrom.Value = GetServerDate(tran)
            'rbtGRunNo.Checked = True
            'grpGroupBy.Enabled = True
            'lblDateFrom.Text = "As On"
            'lblDateTo.Visible = False
            'dtpTo.Visible = False
            funcAddSumCodeItem()
        Else
            'rbtGCode.Checked = False
            'rbtGName.Checked = False
            'rbtGRunNo.Checked = False
            'grpGroupBy.Enabled = False
            funcAddDetItem()
        End If
    End Sub

    Function funcAddDetItem() As Integer
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtDetailWise.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("RUNNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
            cmbFilterBy.Items.Add("SALESPERSON")
            cmbFilterBy.Items.Add("ACCODE")
            cmbFilterBy.Items.Add("REMARK1")
            cmbFilterBy.SelectedIndex = 0
        End If
    End Function

    Private Sub txtFilterCaption_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilterCaption.GotFocus
        If cmbFilterBy.Text = "NONE" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbFilterBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFilterBy.SelectedIndexChanged
        txtFilterCaption.Text = ""
    End Sub

    Private Sub rdbGCode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGCode.CheckedChanged
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtGCode.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("RUNNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
            cmbFilterBy.Items.Add("SALESPERSON")
            cmbFilterBy.Items.Add("ACCODE")
            cmbFilterBy.Items.Add("REMARK1")
            cmbFilterBy.SelectedIndex = 0
        End If
    End Sub

    Function funcAddSumCodeItem() As Integer
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtGRunNo.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("RUNNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
            cmbFilterBy.Items.Add("SALESPERSON")
            cmbFilterBy.Items.Add("ACCODE")
            cmbFilterBy.Items.Add("REMARK1")
            cmbFilterBy.SelectedIndex = 0
        End If
    End Function

    Private Sub rdbGRunNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGRunNo.CheckedChanged
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        funcAddSumCodeItem()
    End Sub

    Private Sub rdbGName_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGName.CheckedChanged
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtGName.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("RUNNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
            cmbFilterBy.Items.Add("SALESPERSON")
            cmbFilterBy.Items.Add("ACCODE")
            cmbFilterBy.Items.Add("REMARK1")
            cmbFilterBy.SelectedIndex = 0
        End If
    End Sub

    Private Sub frmCustomerOutstanding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridCustomerOutstanding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
        If rbtDetailWise.Checked = True Then
            If e.KeyCode = Keys.D Then
                Try
                    Dim dtRunNo As New DataTable
                    dtRunNo.Clear()
                    gridRunNoFocus.DataSource = Nothing
                    strSql = "select TRANNO,TRANDATE,NAME,DEBIT,CREDIT from TEMP" & systemId & "FINAL where RUNNO='" & gridView.CurrentRow.Cells("RUNNO").Value & "' ORDER BY TRANNO"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtRunNo)
                    gridRunNoFocus.DataSource = dtRunNo
                    grbRunNoFocus.Text = gridView.CurrentRow.Cells(0).Value & " " & "DETAILS"
                    grbRunNoFocus.Visible = True
                    With gridRunNoFocus
                        With .Columns("TRANNO")
                            .Width = 70
                            .HeaderText = "BILLNO"
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("TRANDATE")
                            .Width = 80
                            .HeaderText = "BILLDATE"
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("NAME")
                            .Width = 200
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("CREDIT")
                            .Width = 80
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("DEBIT")
                            .Width = 80
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                    End With
                Catch ex As Exception

                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            ElseIf e.KeyCode = Keys.E Then
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    gridView.Focus()
                    Exit Sub
                End If
                Me.GroupBox1.Visible = True
                txtOldRunNo.Text = gridView.CurrentRow.Cells("RUNNO").Value
                txtNewRunNo.Text = ""
                txtNewRunNo.Focus()
            End If
        End If
        gridRunNoFocus.Focus()
    End Sub

    Private Sub gridRunNoFocus_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridRunNoFocus.KeyDown
        If e.KeyCode = Keys.Escape Then
            grbRunNoFocus.Visible = False
            gridView.Visible = True
            gridView.Focus()
        End If
    End Sub

    Private Sub gridRunNoFocus_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridRunNoFocus.LostFocus
        grbRunNoFocus.Visible = False
        gridView.Visible = True
        gridView.Focus()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmNewCustomerOutstanding_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmNewCustomerOutstanding_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkBasedOnAccode.Checked = obj.p_chkBasedOnAccode
        rbtDetailWise.Checked = obj.p_rbtDetailWise
        rbtSummaryWise.Checked = obj.p_rbtSummaryWise
        rbtPending.Checked = obj.p_rbtPending
        rbtClosed.Checked = obj.p_rbtClosed
        rbtAll.Checked = obj.p_rbtAll
        rbtORunNo.Checked = obj.p_rbtORunNo
        rbtOBillDate.Checked = obj.p_rbtOBillDate
        rbtOName.Checked = obj.p_rbtOName
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        cmbFilterBy.Text = obj.p_cmbFilterBy
        txtFilterCaption.Text = obj.p_txtFilterCaption
        txtNodeId.Text = obj.p_txtNodeId
        chkGroupByArea.Checked = obj.p_chkGroupByArea
        chkAdvance.Checked = obj.p_chkAdvance
        chkGeneral.Checked = obj.p_chkGeneral
        chkOrder.Checked = obj.p_chkOrder
        chkCredit.Checked = obj.p_chkCredit
        chkToBe.Checked = obj.p_chkToBe
        chkOrderSummary.Checked = obj.p_chkOrderSummary
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmNewCustomerOutstanding_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkBasedOnAccode = chkBasedOnAccode.Checked
        obj.p_rbtDetailWise = rbtDetailWise.Checked
        obj.p_rbtSummaryWise = rbtSummaryWise.Checked
        obj.p_rbtPending = rbtPending.Checked
        obj.p_rbtClosed = rbtClosed.Checked
        obj.p_rbtAll = rbtAll.Checked
        obj.p_rbtORunNo = rbtORunNo.Checked
        obj.p_rbtOBillDate = rbtOBillDate.Checked
        obj.p_rbtOName = rbtOName.Checked
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_cmbFilterBy = cmbFilterBy.Text
        obj.p_txtFilterCaption = txtFilterCaption.Text
        obj.p_txtNodeId = txtNodeId.Text
        obj.p_chkGroupByArea = chkGroupByArea.Checked
        obj.p_chkAdvance = chkAdvance.Checked
        obj.p_chkGeneral = chkGeneral.Checked
        obj.p_chkOrder = chkOrder.Checked
        obj.p_chkCredit = chkCredit.Checked
        obj.p_chkToBe = chkToBe.Checked
        obj.p_chkOrderSummary = chkOrderSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmNewCustomerOutstanding_Properties))
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub txtNewRunNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewRunNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtNewRunNo.Text <> "" Then
                Dim mnewtype As String = ""
                If cmbType.Text = "Credit" Then mnewtype = "D"
                If cmbType.Text = "Advance" Then mnewtype = "A"
                strSql += vbCrLf + " UPDATE " & cnAdminDb & "..OUTSTANDING"
                strSql += vbCrLf + " SET RUNNO= '" & txtNewRunNo.Text & "'"
                'strSql += vbCrLf + " ,TRANTYPE= '" & mnewtype & "'"
                strSql += vbCrLf + " WHERE RUNNO = '" & txtOldRunNo.Text & "'"
                strSql += vbCrLf + " and TRANNO = '" & gridView.CurrentRow.Cells("TRANNO").Value & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, )
                '                tran.Commit()
                MsgBox("Run No updated", MsgBoxStyle.Information)
                gridView.CurrentRow.Cells("RUNNO").Value = txtNewRunNo.Text
                Me.GroupBox1.Visible = False

            End If
        End If
    End Sub

    Private Sub rbtdrs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtdrs.CheckedChanged
        If rbtdrs.Checked Then
            chkCredit.Checked = True
            chkAdvance.Checked = False
            chkOrder.Checked = False
            chkGeneral.Checked = False
            chkToBe.Checked = False
            chkBasedOnAccode.Checked = True
        End If
    End Sub

    Private Sub rbtadvance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtadvance.CheckedChanged
        If rbtadvance.Checked Then
            chkCredit.Checked = False
            chkAdvance.Checked = True
            chkOrder.Checked = False
            chkGeneral.Checked = False
            chkToBe.Checked = False
            chkBasedOnAccode.Checked = True
        End If
    End Sub

    Private Sub rbtorder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtorder.CheckedChanged
        If rbtorder.Checked Then
            chkCredit.Checked = False
            chkAdvance.Checked = False
            chkOrder.Checked = True
            chkGeneral.Checked = False
            chkToBe.Checked = False
            chkBasedOnAccode.Checked = True
        End If
    End Sub

    Private Sub chkacfilter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkacfilter.CheckedChanged
        If chkacfilter.Checked Then
            rbtdrs.Checked = True
            chkBasedOnAccode.Enabled = False
            grpOutStanding.Enabled = False
            acfiltergrp.Enabled = True
        Else
            For i As Integer = 0 To acfiltergrp.Controls.Count - 1
                CType(acfiltergrp.Controls(i), RadioButton).Checked = False
            Next
            chkBasedOnAccode.Enabled = True
            grpOutStanding.Enabled = True
            acfiltergrp.Enabled = False
        End If
    End Sub

    Private Sub chkBasedOnAccode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBasedOnAccode.CheckedChanged
        'If chkBasedOnAccode.Checked And chkBasedOnAccode.Enabled Then
        '    cmbAcGrp.Enabled = True
        '    chkCmbAcGrp.Enabled = True
        'Else
        '    cmbAcGrp.Enabled = False
        '    chkCmbAcGrp.Enabled = False
        'End If
    End Sub

    Private Sub ChkAsOn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkAsOn.CheckedChanged
        If ChkAsOn.Checked = True Then
            ChkAsOn.Text = "As On"
            dtpFrom.Value = GetServerDate(tran)
            rbtGRunNo.Checked = True
            grpGroupBy.Enabled = True
            'lblDateFrom.Text = "As On"
            lblDateTo.Visible = False
            dtpTo.Visible = False
        Else
            ChkAsOn.Text = "Date From"
            dtpFrom.Value = GetServerDate(tran)
            rbtGRunNo.Checked = True
            grpGroupBy.Enabled = True
            'lblDateFrom.Text = "As On"
            lblDateTo.Visible = True
            dtpTo.Visible = True
        End If

    End Sub

    Private Sub chkCmbAcGrp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkCmbAcGrp.KeyPress
        If AscW(e.KeyChar) = 13 Then
            funAddAcName()
        End If
    End Sub

    Private Sub chkCmbAcGrp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbAcGrp.SelectedIndexChanged
        funAddAcName()
    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

End Class


Public Class frmNewCustomerOutstanding_Properties
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
    Private chkBasedOnAccode As Boolean = False
    Public Property p_chkBasedOnAccode() As Boolean
        Get
            Return chkBasedOnAccode
        End Get
        Set(ByVal value As Boolean)
            chkBasedOnAccode = value
        End Set
    End Property
    Private rbtDetailWise As Boolean = True
    Public Property p_rbtDetailWise() As Boolean
        Get
            Return rbtDetailWise
        End Get
        Set(ByVal value As Boolean)
            rbtDetailWise = value
        End Set
    End Property
    Private rbtSummaryWise As Boolean = False
    Public Property p_rbtSummaryWise() As Boolean
        Get
            Return rbtSummaryWise
        End Get
        Set(ByVal value As Boolean)
            rbtSummaryWise = value
        End Set
    End Property
    Private rbtPending As Boolean = True
    Public Property p_rbtPending() As Boolean
        Get
            Return rbtPending
        End Get
        Set(ByVal value As Boolean)
            rbtPending = value
        End Set
    End Property
    Private rbtClosed As Boolean = False
    Public Property p_rbtClosed() As Boolean
        Get
            Return rbtClosed
        End Get
        Set(ByVal value As Boolean)
            rbtClosed = value
        End Set
    End Property
    Private rbtAll As Boolean = False
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property
    Private rbtORunNo As Boolean = True
    Public Property p_rbtORunNo() As Boolean
        Get
            Return rbtORunNo
        End Get
        Set(ByVal value As Boolean)
            rbtORunNo = value
        End Set
    End Property
    Private rbtOBillDate As Boolean = False
    Public Property p_rbtOBillDate() As Boolean
        Get
            Return rbtOBillDate
        End Get
        Set(ByVal value As Boolean)
            rbtOBillDate = value
        End Set
    End Property
    Private rbtOName As Boolean = False
    Public Property p_rbtOName() As Boolean
        Get
            Return rbtOName
        End Get
        Set(ByVal value As Boolean)
            rbtOName = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private cmbFilterBy As String = "NONE"
    Public Property p_cmbFilterBy() As String
        Get
            Return cmbFilterBy
        End Get
        Set(ByVal value As String)
            cmbFilterBy = value
        End Set
    End Property
    Private txtFilterCaption As String = ""
    Public Property p_txtFilterCaption() As String
        Get
            Return txtFilterCaption
        End Get
        Set(ByVal value As String)
            txtFilterCaption = value
        End Set
    End Property
    Private txtNodeId As String = ""
    Public Property p_txtNodeId() As String
        Get
            Return txtNodeId
        End Get
        Set(ByVal value As String)
            txtNodeId = value
        End Set
    End Property

    Private chkGroupByArea As Boolean = False
    Public Property p_chkGroupByArea() As Boolean
        Get
            Return chkGroupByArea
        End Get
        Set(ByVal value As Boolean)
            chkGroupByArea = value
        End Set
    End Property
    Private chkAdvance As Boolean = False
    Public Property p_chkAdvance() As Boolean
        Get
            Return chkAdvance
        End Get
        Set(ByVal value As Boolean)
            chkAdvance = value
        End Set
    End Property
    Private chkGeneral As Boolean = False
    Public Property p_chkGeneral() As Boolean
        Get
            Return chkGeneral
        End Get
        Set(ByVal value As Boolean)
            chkGeneral = value
        End Set
    End Property
    Private chkOrder As Boolean = False
    Public Property p_chkOrder() As Boolean
        Get
            Return chkOrder
        End Get
        Set(ByVal value As Boolean)
            chkOrder = value
        End Set
    End Property
    Private chkCredit As Boolean = False
    Public Property p_chkCredit() As Boolean
        Get
            Return chkCredit
        End Get
        Set(ByVal value As Boolean)
            chkCredit = value
        End Set
    End Property
    Private chkToBe As Boolean = False
    Public Property p_chkToBe() As Boolean
        Get
            Return chkToBe
        End Get
        Set(ByVal value As Boolean)
            chkToBe = value
        End Set
    End Property


    Private chkOrderSummary As Boolean = False
    Public Property p_chkOrderSummary() As Boolean
        Get
            Return chkOrderSummary
        End Get
        Set(ByVal value As Boolean)
            chkOrderSummary = value
        End Set
    End Property

End Class