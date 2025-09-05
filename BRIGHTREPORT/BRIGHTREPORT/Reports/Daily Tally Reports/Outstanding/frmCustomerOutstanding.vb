Imports System.Data.OleDb
Public Class frmCustomerOutstanding
    'CALNO 061212 VASANTH, CLIENT-KAMESWARI
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim SelectedCompany As String
    Dim grpcc As Integer

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
        chkBasedOnAccode.Focus()
        chkBasedOnAccode.Select()
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
        If chkCreditPurchase.Checked Then TranId += "D,"
        If chkGeneral.Checked Then TranId += "T,"
        If TranId <> "" Then TranId = Mid(TranId, 1, TranId.Length - 1) Else TranId = "A,D,T,O"

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

    Private Sub Report()
        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        If Not chkAdvance.Checked And
        Not chkCredit.Checked And
        Not chkCreditPurchase.Checked And
        Not chkOrder.Checked And
        Not chkGeneral.Checked And
        Not chkGiftVoucher.Checked And
        Not chkToBe.Checked Then
            chkAdvance.Checked = True
            chkCredit.Checked = True
            chkOrder.Checked = True
            chkGeneral.Checked = True
            chkGiftVoucher.Checked = True
            chkToBe.Checked = True
        End If
        Dim selectcostid As String = ""
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        If cmbCostCentre.Text <> "" Then
            If cmbCostCentre.Text = "ALL" Then
                selectcostid = ""
            Else
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..CostCentre WHERE COSTNAME = '" & cmbCostCentre.Text & "'"
                selectcostid = objGPack.GetSqlValue(strSql, , "")
            End If
        End If
        Me.Refresh()
        Dim Display As String
        Dim OrderBy As String
        Dim Type As String = ""
        Dim Tobe As String = ""

        If rbtPending.Checked Then
            Display = "P"
        ElseIf rbtClosed.Checked Then
            Display = "C"
        ElseIf rbtReceived.Checked = True Then
            Display = "R"
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
        If chkToBe.Checked And chkCredit.Checked Then
            Tobe = "B"
        ElseIf chkToBe.Checked And chkCredit.Checked = False Then
            Tobe = "Y"
        ElseIf chkToBe.Checked = False Then
            Tobe = "N"
        End If
        Dim Accode As String = ""
        If rbtDrs.Checked Then
            If chkAdvance.Checked Then Accode = ",ADVANCE"
            If chkOrder.Checked Then Accode = Accode & ",ADVORD"
            If chkCredit.Checked Or chkCreditPurchase.Checked Or chkToBe.Checked Then Accode = Accode & ",DRS"
            Accode = Mid(Accode, 2)
            ' Accode = Replace(Accode, ",", "','")
        End If
        If chkAdvance.Checked Then Type += "A,"
        If chkCredit.Checked Then Type += "D,"
        If chkCreditPurchase.Checked Then Type += "C,"
        'CALNO 061212
        If chkToBe.Checked Then Type += "J,"
        If chkOrder.Checked Then Type += "O,"
        If chkGeneral.Checked Then Type += "T,"
        If chkGiftVoucher.Checked Then Type += "GV,"
        Type.Remove(Type.Length - 1, 1)
        Dim FilterName As String = Nothing
        If cmbFilterBy.Text = "NONE" Then
            FilterName = ""
        ElseIf cmbFilterBy.Text = "NAME" Then
            FilterName = "PNAME"
        Else
            FilterName = Replace(cmbFilterBy.Text, "'", "''''")
        End If
        Dim CrPurchase As String = ""
        If chkCreditPurchase.Checked And chkCredit.Checked = False Then
            CrPurchase = "Y"
        ElseIf chkCreditPurchase.Checked And chkCredit.Checked Then
            CrPurchase = "B"
        ElseIf chkCreditPurchase.Checked = False And chkCredit.Checked Then
            CrPurchase = "N"
        End If
        strSql = "IF  EXISTS (SELECT * FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP_OUTSTANDING') DROP TABLE  TEMPTABLEDB..TEMP_OUTSTANDING"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If chkBasedOnAccode.Checked And chkCreditRpt.Checked = False Then
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_CUSTOMEROUTSTANDING_ACCODE"

        ElseIf chkCreditRpt.Checked Then
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_CUSTOMER_CREDIT"
            strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@DISPLAY = '" & Display & "'"
            strSql += vbCrLf + " ,@ORDERBY = '" & OrderBy & "'"
            strSql += vbCrLf + " ,@FILTERBY = '" & FilterName & "'"
            strSql += vbCrLf + " ,@FILTERCAPTION = '" & txtFilterCaption.Text & "'"
            strSql += vbCrLf + " ,@COSTID = '" & selectcostid & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
            strSql += vbCrLf + " ,@TYPEID = '" & Mid(Type, 1, Len(Type) - 1) & "'"
            strSql += vbCrLf + " ,@GRPAREA = '" & IIf(chkGroupByArea.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@TOBE = '" & Tobe & "'"
            strSql += vbCrLf + " ,@FORMAT1 = '" & IIf(chkformat.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@CPUR='" & CrPurchase & "'"
            strSql += vbCrLf + " ,@ACCODE='" & Accode & "'"
            GoTo EXESP
        Else
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_CUSTOMEROUTSTANDING"
            strSql += vbCrLf + " @GRPCC='" & IIf(chkgrbcc.Checked, "Y", "N") & "',"
        End If
        strSql += vbCrLf + " @FROMDATE = '" & IIf(rbtDetailWise.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), "1900-01-01") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & IIf(rbtDetailWise.Checked, dtpTo.Value.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@DISPLAY = '" & Display & "'"
        strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSummaryWise.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ORDERBY = '" & OrderBy & "'"
        strSql += vbCrLf + " ,@FILTERBY = '" & cmbFilterBy.Text & "'"
        strSql += vbCrLf + " ,@FILTERCAPTION = '" & txtFilterCaption.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & cmbCostCentre.Text & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        strSql += vbCrLf + " ,@TYPEID = '" & Type & "'"
        If chkBasedOnAccode.Checked Then
            strSql += vbCrLf + " ,@CRPUR = '" & CrPurchase & "'"
        End If
        'strSql += vbCrLf + " ,@CRPUR = '" & CrPurchase & "'"
        strSql += vbCrLf + " ,@ACCODE='" & Accode & "'"
        If chkBasedOnAccode.Checked = False Then
            strSql += vbCrLf + " ,@GRPAREA = '" & IIf(chkGroupByArea.Checked, "Y", "N") & "'"
        End If

EXESP:
        If chkCreditRpt.Checked Then
            strSql += vbCrLf + " ,@GROUPBY = '" & IIf(rbtORunNo.Checked, "N", "Y") & "'"
        End If

        gridView.DataSource = Nothing
        Dim dtGrid As New DataTable
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds)
        If ds.Tables.Count > 0 Then
            dtGrid = ds.Tables(0)
        End If

        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        gridformat()
        'BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        FormatGridColumns(gridView)
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        If ChKWithGst.Checked = False Then
            If gridView.Columns.Contains("GST") Then gridView.Columns("GST").Visible = False
            If gridView.Columns.Contains("TOTAL") Then gridView.Columns("TOTAL").Visible = False
        End If
        If ChkWithWt.checked = False Then
            If gridView.Columns.Contains("DEBIT_WT") = True Then gridView.Columns("DEBIT_WT").Visible = False
            If gridView.Columns.Contains("CREDIT_WT") = True Then gridView.Columns("CREDIT_WT").Visible = False
            If gridView.Columns.Contains("BALANCE_WT") = True Then gridView.Columns("BALANCE_WT").Visible = False
        Else
            If gridView.Columns.Contains("BALANCE_WT") = True Then gridView.Columns("BALANCE_WT").HeaderText = "BALANCE"
        End If
        If ChkWithSlNo.Checked = False Then
            If gridView.Columns.Contains("SNO") = True Then gridView.Columns("SNO").Visible = False
        End If
        If ChkWithAddr.Checked = False Then
            If gridView.Columns.Contains("ADDRESS1") = True Then gridView.Columns("ADDRESS1").Visible = False
            If gridView.Columns.Contains("ADDRESS2") = True Then gridView.Columns("ADDRESS2").Visible = False
            If gridView.Columns.Contains("CITY") = True Then gridView.Columns("CITY").Visible = False
        End If
        If gridView.Columns.Contains("REMARK1") = True Then gridView.Columns("REMARK1").Visible = False
        If gridView.Columns.Contains("REMARK2") = True Then gridView.Columns("REMARK2").Visible = False
        If ChkWithRemark.Checked Then
            If gridView.Columns.Contains("REMARK1") = True Then gridView.Columns("REMARK1").Visible = True
            If gridView.Columns.Contains("REMARK2") = True Then gridView.Columns("REMARK2").Visible = True
        End If
        If gridView.Columns.Contains("SALESPERSON") = True Then gridView.Columns("SALESPERSON").Visible = Not rbtSummaryWise.Checked
        If gridView.Columns.Contains("COSTNAME") = True Then gridView.Columns("COSTNAME").Visible = True
        If gridView.Columns.Contains("COSTID") = True Then gridView.Columns("COSTID").Visible = False
        If gridView.Columns.Contains("LASTTRANDATE") = True Then gridView.Columns("LASTTRANDATE").Visible = rbtSummaryWise.Checked
        If gridView.Columns.Contains("TRANDATE") = True Then gridView.Columns("TRANDATE").Visible = Not rbtSummaryWise.Checked
        If gridView.Columns.Contains("TRANDATE") = True Then gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

        If gridView.Columns.Contains("TRANDATE1") = True Then gridView.Columns("TRANDATE1").Visible = rbtSummaryWise.Checked
        If gridView.Columns.Contains("TRANDATE1") = True Then gridView.Columns("TRANDATE1").HeaderText = "BILLDATE"
        If gridView.Columns.Contains("TRANNO") = True Then gridView.Columns("TRANNO").Visible = Not rbtSummaryWise.Checked
        If gridView.Columns.Contains("BALANCE") = True Then gridView.Columns("BALANCE").Visible = rbtSummaryWise.Checked
        If gridView.Columns.Contains("TRANTYPE") = True Then gridView.Columns("TRANTYPE").Visible = False
        If gridView.Columns.Contains("RUNNO") = True Then gridView.Columns("RUNNO").Visible = False
        If gridView.Columns.Contains("RESULT") = True Then gridView.Columns("RESULT").Visible = False
        If gridView.Columns.Contains("COLHEAD") = True Then gridView.Columns("COLHEAD").Visible = False
        If gridView.Columns.Contains("PAYMODE") = True Then gridView.Columns("PAYMODE").Visible = False
        If gridView.Columns.Contains("TRANNO") = True Then gridView.Columns("TRANNO").Visible = True
        If gridView.Columns.Contains("KNO") = True Then gridView.Columns("KNO").Visible = False
        If gridView.Columns.Contains("DUEDATE") = True Then gridView.Columns("DUEDATE").Visible = True
        If gridView.Columns.Contains("DUEDATE") = True Then gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("COSTNAME") = True Then gridView.Columns("COSTNAME").Visible = False
        gridView.Columns("TRANNO").HeaderText = "BILL NO"
        If chkBasedOnAccode.Checked = False Then
            If gridView.Columns.Contains("AREAORD") = True Then gridView.Columns("AREAORD").Visible = False
            If gridView.Columns.Contains("AREA") = True Then gridView.Columns("AREA").Visible = Not chkGroupByArea.Checked
        End If
        If chkCreditRpt.Checked Then
            If chkformat.Checked = True Then
                If gridView.Columns.Contains("SALESPERSON") = True Then gridView.Columns("SALESPERSON").Visible = False
                If gridView.Columns.Contains("COSTNAME") = True Then gridView.Columns("COSTNAME").Visible = False
                If gridView.Columns.Contains("COMPANYID") = True Then gridView.Columns("COMPANYID").Visible = False
                If gridView.Columns.Contains("LASTTRANDATE") = True Then gridView.Columns("LASTTRANDATE").Visible = False
                If gridView.Columns.Contains("AREA") = True Then gridView.Columns("AREA").Visible = False
                If gridView.Columns.Contains("MOBILE") = True Then gridView.Columns("MOBILE").Visible = False
                If gridView.Columns.Contains("ADDRESS1") = True Then gridView.Columns("ADDRESS1").Visible = False
                If gridView.Columns.Contains("ADDRESS2") = True Then gridView.Columns("ADDRESS2").Visible = False
                If gridView.Columns.Contains("ADDRESS3") = True Then gridView.Columns("ADDRESS3").Visible = False
                If gridView.Columns.Contains("BALANCE") = True Then gridView.Columns("BALANCE").Visible = True
                If gridView.Columns.Contains("CITY") = True Then gridView.Columns("CITY").Visible = False
                If gridView.Columns.Contains("ADDORD") = True Then gridView.Columns("ADDORD").Visible = False
                If gridView.Columns.Contains("DOORNO") = True Then gridView.Columns("DOORNO").Visible = False
                If gridView.Columns.Contains("ADDORD") = True Then gridView.Columns("ADDORD").Visible = False
                If gridView.Columns.Contains("TRANDATE") = True Then gridView.Columns("TRANDATE").Visible = False
                If gridView.Columns.Contains("GRTOTAL") = True Then gridView.Columns("GRTOTAL").Visible = False
                If gridView.Columns.Contains("TRANNO1") = True Then gridView.Columns("TRANNO1").HeaderText = "BILLNO"
                If gridView.Columns.Contains("PARTICULAR") = True Then gridView.Columns("PARTICULAR").HeaderText = "TRANNO"
                If gridView.Columns.Contains("BILLAMOUNT") = True Then gridView.Columns("BILLAMOUNT").HeaderText = "BILL AMOUNT"
                If gridView.Columns.Contains("RECDAMOUNT") = True Then gridView.Columns("RECDAMOUNT").HeaderText = "REC AMOUNT"
                If gridView.Columns.Contains("CREDITRECEIPT") = True Then gridView.Columns("CREDITRECEIPT").HeaderText = "CREDIT RECEIPT"
                If gridView.Columns.Contains("NAME") = True Then gridView.Columns("NAME").HeaderText = "PARTY NAME/ADDRESS"
                If gridView.Columns.Contains("CATNAME") = True Then gridView.Columns("CATNAME").HeaderText = "BILL TYPE"
                If gridView.Columns.Contains("RECBILLNO") = True Then gridView.Columns("RECBILLNO").HeaderText = "REC BILLNO"
                If gridView.Columns.Contains("RECBILLDATE") = True Then gridView.Columns("RECBILLDATE").HeaderText = "REC BILLDATE"
                If gridView.Columns.Contains("BILLNO") = True Then gridView.Columns("BILLNO").Visible = False
                If gridView.Columns.Contains("TRDATE") = True Then gridView.Columns("TRDATE").Visible = False
                If gridView.Columns.Contains("PARTICULAR") = True Then gridView.Columns("PARTICULAR").Visible = False
                If gridView.Columns.Contains("DUEDATE") = True Then gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            Else
                If gridView.Columns.Contains("SALESPERSON") = True Then gridView.Columns("SALESPERSON").Visible = False
                If gridView.Columns.Contains("COSTNAME") = True Then gridView.Columns("COSTNAME").Visible = False
                If gridView.Columns.Contains("COMPANYID") = True Then gridView.Columns("COMPANYID").Visible = False
                If gridView.Columns.Contains("LASTTRANDATE") = True Then gridView.Columns("LASTTRANDATE").Visible = False
                If gridView.Columns.Contains("AREA") = True Then gridView.Columns("AREA").Visible = False
                If gridView.Columns.Contains("MOBILE") = True Then gridView.Columns("MOBILE").Visible = False
                If gridView.Columns.Contains("ADDRESS1") = True Then gridView.Columns("ADDRESS1").Visible = False
                If gridView.Columns.Contains("ADDRESS2") = True Then gridView.Columns("ADDRESS2").Visible = False
                If gridView.Columns.Contains("ADDRESS3") = True Then gridView.Columns("ADDRESS3").Visible = False
                If gridView.Columns.Contains("BALANCE") = True Then gridView.Columns("BALANCE").Visible = True
                If gridView.Columns.Contains("CITY") = True Then gridView.Columns("CITY").Visible = False
                If gridView.Columns.Contains("ADDORD") = True Then gridView.Columns("ADDORD").Visible = False
                If gridView.Columns.Contains("DOORNO") = True Then gridView.Columns("DOORNO").Visible = False
                If gridView.Columns.Contains("ADDORD") = True Then gridView.Columns("ADDORD").Visible = False
                If gridView.Columns.Contains("TRANDATE") = True Then gridView.Columns("TRANDATE").Visible = False
                If gridView.Columns.Contains("GRTOTAL") = True Then gridView.Columns("GRTOTAL").Visible = False
                If gridView.Columns.Contains("TRANNO1") = True Then gridView.Columns("TRANNO1").HeaderText = "BILLNO"
                If gridView.Columns.Contains("PARTICULAR") = True Then gridView.Columns("PARTICULAR").HeaderText = "TRANNO"
                If gridView.Columns.Contains("DEBIT") = True Then gridView.Columns("DEBIT").HeaderText = "PAID AMOUNT"
                If gridView.Columns.Contains("CREDIT") = True Then gridView.Columns("CREDIT").HeaderText = "REC AMOUNT"
                If gridView.Columns.Contains("NAME") = True Then gridView.Columns("NAME").HeaderText = "PARTY NAME"
                If gridView.Columns.Contains("BILLNO") = True Then gridView.Columns("BILLNO").Visible = False
                If gridView.Columns.Contains("TRDATE") = True Then gridView.Columns("TRDATE").Visible = False
                If gridView.Columns.Contains("CATNAME") = True Then gridView.Columns("CATNAME").Visible = False
                If gridView.Columns.Contains("DUEDATE") = True Then gridView.Columns("DUEDATE").Visible = False
            End If

        End If
        If gridView.Rows.Count > 0 Then
            lblTitle.Visible = True
            Dim strTitle As String = Nothing
            strTitle = " CUSTOMER OUTSTANDING REPORT"
            If rbtDetailWise.Checked = True Then
                strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            Else
                strTitle += " AS ON  " + dtpFrom.Text
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
        Dim SNO As Integer = 1
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                If gridView.Columns.Contains("COLHEAD") = True Then
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "T"
                            .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                            .Cells("PARTICULAR").Style.ForeColor = Color.Red
                            .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        Case "S"
                            .DefaultCellStyle.ForeColor = Color.Blue
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        Case "G"
                            .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                            .Cells("PARTICULAR").Style.ForeColor = Color.Red
                            .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            .DefaultCellStyle.BackColor = Color.LightYellow
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        Case "G1", "S2"
                            .Cells("PARTICULAR").Style.ForeColor = Color.Red
                            .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            .DefaultCellStyle.BackColor = Color.Wheat
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End Select
                End If
                If gridView.Columns.Contains("RESULT") = True And gridView.Columns.Contains("SNO") = True And ChkWithSlNo.Checked Then
                    If Val(.Cells("RESULT").Value.ToString) = 1 Then
                        .Cells("SNO").Value = SNO
                        SNO += 1
                    ElseIf Val(.Cells("RESULT").Value.ToString) = 2 Then
                        SNO = 1
                    End If
                End If
            End With
        Next
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Prop_Sets()
        Report()
        Exit Sub
        CustStoredProcedure()
        Exit Sub

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
        funcAddCostCentre()
        funcAddDetItem()
        Prop_Gets()
        chkBasedOnAccode.Focus()
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
                With .Columns("TRANNO")
                    .Width = 80
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
            rbtGCode.Checked = False
            rbtGName.Checked = False
            rbtGRunNo.Checked = False
            grpGroupBy.Enabled = False
            lblDateFrom.Text = "Date From"
            lblDateTo.Visible = True
            dtpTo.Visible = True
            funcAddDetItem()
        Else
            rbtGCode.Checked = True
            grpGroupBy.Enabled = True
            funcAddSumCodeItem()
        End If
    End Sub

    Private Sub rdbSummaryWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummaryWise.CheckedChanged
        If rbtSummaryWise.Checked = True Then
            dtpFrom.Value = GetServerDate(tran)
            rbtGRunNo.Checked = True
            grpGroupBy.Enabled = True
            lblDateFrom.Text = "As On"
            lblDateTo.Visible = False
            dtpTo.Visible = False
            funcAddSumCodeItem()
        Else
            rbtGCode.Checked = False
            rbtGName.Checked = False
            rbtGRunNo.Checked = False
            grpGroupBy.Enabled = False
            lblDateFrom.Text = "From Date"
            lblDateTo.Visible = True
            dtpTo.Visible = True
            funcAddDetItem()
        End If
    End Sub

    Function funcAddDetItem() As Integer
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtDetailWise.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("TRANNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
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
            cmbFilterBy.Items.Add("TRANNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
            cmbFilterBy.Items.Add("REMARK1")
            cmbFilterBy.SelectedIndex = 0
        End If
    End Sub

    Function funcAddSumCodeItem() As Integer
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        If rbtGRunNo.Checked = True Then
            cmbFilterBy.Items.Add("NONE")
            cmbFilterBy.Items.Add("TRANNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
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
            cmbFilterBy.Items.Add("TRANNO")
            cmbFilterBy.Items.Add("NAME")
            cmbFilterBy.Items.Add("MOBILE")
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
                    strSql = "select TRANNO,TRANDATE,NAME,DEBIT,CREDIT from TEMPTABLEDB..TEMP_OUTSTANDING where RUNNO='" & gridView.CurrentRow.Cells("RUNNO").Value & "' ORDER BY TRANNO"
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
        Dim obj As New frmCustomerOutstanding_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCustomerOutstanding_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkBasedOnAccode.Checked = obj.p_chkBasedOnAccode
        rbtDetailWise.Checked = obj.p_rbtDetailWise
        rbtSummaryWise.Checked = obj.p_rbtSummaryWise
        rbtPending.Checked = obj.p_rbtPending
        optAllCode.Checked = obj.p_optAllCode
        rbtDrs.Checked = obj.p_rbtDrs
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
        ChkWithRemark.Checked = obj.p_chkWithRemark
        ChkWithAddr.Checked = obj.p_chkWithAddr
        ChkWithSlNo.Checked = obj.p_chkWithSno
        chkgrbcc.Checked = obj.p_chkgrbcc
        ChkWithWt.Checked = obj.p_chkWithWt
        ChKWithGst.Checked = obj.p_chkWithGST
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCustomerOutstanding_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkBasedOnAccode = chkBasedOnAccode.Checked
        obj.p_rbtDetailWise = rbtDetailWise.Checked
        obj.p_rbtSummaryWise = rbtSummaryWise.Checked
        obj.p_rbtPending = rbtPending.Checked
        obj.p_rbtDrs = rbtDrs.Checked
        obj.p_optAllCode = optAllCode.Checked
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
        obj.p_chkWithRemark = ChkWithRemark.Checked
        obj.p_chkWithAddr = ChkWithAddr.Checked
        obj.p_chkWithSno = ChkWithSlNo.Checked
        obj.p_chkgrbcc = chkgrbcc.Checked
        obj.p_chkWithWt = ChkWithWt.Checked
        obj.p_chkWithGST = ChKWithGst.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCustomerOutstanding_Properties))
    End Sub

    Private Sub chkCreditRpt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCreditRpt.CheckedChanged
        If chkCreditRpt.Checked = True Then
            chkBasedOnAccode.Checked = False
            rbtSummaryWise.Enabled = False
            rbtSummaryWise.Checked = False
            lblDateFrom.Text = "From Date"
            lblDateTo.Visible = True
            dtpTo.Visible = True
        Else
            rbtSummaryWise.Enabled = True
            rbtSummaryWise.Checked = True
            lblDateFrom.Text = "As On"
            lblDateTo.Visible = False
            dtpTo.Visible = False
        End If
    End Sub

    Private Sub chkBasedOnAccode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBasedOnAccode.CheckedChanged
        If chkBasedOnAccode.Checked = True Then
            chkCreditRpt.Checked = False
        End If
    End Sub

    Private Sub chkCredit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCredit.CheckedChanged
        If chkCredit.Checked And chkOrder.Checked Then rbtDrs.Text = "Drs./Advance Only"
        If chkCredit.Checked And Not chkOrder.Checked Then rbtDrs.Text = "Drs. Only"
        If Not chkCredit.Checked And chkAdvance.Checked And Not chkOrder.Checked Then rbtDrs.Text = "Advance Only"
    End Sub

    Private Sub chkOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOrder.CheckedChanged
        If chkCredit.Checked And chkOrder.Checked Then rbtDrs.Text = "Drs./Advord Only"
        If Not chkCredit.Checked And chkOrder.Checked Then rbtDrs.Text = "Advord Only"
        If Not chkCredit.Checked And chkAdvance.Checked And Not chkOrder.Checked Then rbtDrs.Text = "Advance Only"
    End Sub

    Private Sub chkAdvance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdvance.CheckedChanged
        If chkCredit.Checked And chkOrder.Checked Then rbtDrs.Text = "Drs./Advance Only"
        If chkCredit.Checked And Not chkOrder.Checked And Not chkAdvance.Checked Then rbtDrs.Text = "Drs. Only"
        If Not chkCredit.Checked And chkAdvance.Checked And Not chkOrder.Checked Then rbtDrs.Text = "Advance Only"
    End Sub
    Private Sub chkGroupByArea_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGroupByArea.CheckedChanged
        If chkGroupByArea.Checked Then
            chkgrbcc.Enabled = False
            chkgrbcc.Checked = False
        Else
            chkgrbcc.Enabled = True
        End If
    End Sub
    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal ColFormat As Boolean = False, Optional ByVal ColReadOnly As Boolean = True, Optional ByVal ColSort As Boolean = False)
        With grid
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).ReadOnly = ColReadOnly
                If .Columns(i).ValueType IsNot Nothing Then
                    If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "0.000"
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "0.00"
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                    End If
                End If
                ' If Not ColSort Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
    End Sub
End Class


Public Class frmCustomerOutstanding_Properties
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
    Private optAllCode As Boolean = True
    Public Property p_optAllCode() As Boolean
        Get
            Return optAllCode
        End Get
        Set(ByVal value As Boolean)
            optAllCode = value
        End Set
    End Property
    Private rbtDrs As Boolean = False
    Public Property p_rbtDrs() As Boolean
        Get
            Return rbtDrs
        End Get
        Set(ByVal value As Boolean)
            rbtDrs = value
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
    Private chkWithRemark As Boolean = False
    Public Property p_chkWithRemark() As Boolean
        Get
            Return chkWithRemark
        End Get
        Set(ByVal value As Boolean)
            chkWithRemark = value
        End Set
    End Property
    Private chkWithSno As Boolean = False
    Public Property p_chkWithSno() As Boolean
        Get
            Return chkWithSno
        End Get
        Set(ByVal value As Boolean)
            chkWithSno = value
        End Set
    End Property
    Private chkWithAddr As Boolean = False
    Public Property p_chkWithAddr() As Boolean
        Get
            Return chkWithAddr
        End Get
        Set(ByVal value As Boolean)
            chkWithAddr = value
        End Set
    End Property
    Private chkgrbcc As Boolean = False
    Public Property p_chkgrbcc() As Boolean
        Get
            Return chkgrbcc
        End Get
        Set(ByVal value As Boolean)
            chkgrbcc = value
        End Set
    End Property
    Private chkWithWt As Boolean = False
    Public Property p_chkWithWt() As Boolean
        Get
            Return chkWithWt
        End Get
        Set(ByVal value As Boolean)
            chkWithWt = value
        End Set
    End Property
    Private chkWithGST As Boolean = False
    Public Property p_chkWithGST() As Boolean
        Get
            Return chkWithGST
        End Get
        Set(ByVal value As Boolean)
            chkWithGST = value
        End Set
    End Property
End Class