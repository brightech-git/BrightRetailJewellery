Imports System.Data.OleDb
Public Class DueDateWiseOutstandingRpt
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim colCheckBox As New DataGridViewCheckBoxColumn
    Dim SMS_DUE_REMAINDER As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_DUE_REMAINDER' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString

    Private Sub DueDateWiseOutstandingRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And TabMain.SelectedTab.Name = TabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DueDateWiseOutstandingRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGen.Left, Me.TabGen.Top, Me.TabGen.Width, Me.TabGen.Height))
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    If cnCostName = dt.Rows(cnt).Item(0).ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString, False)
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
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' rbtOrderRunNo.Checked = True
        'chkWithReceiptDetail.Checked = False
        'chkAsOnDate.Checked = True
        funcAddDetItem()
        Prop_Gets()
        chkAsOnDate.Select()
        chkCompanySelectAll.Checked = True
    End Sub

    Function funcAddDetItem() As Integer
        cmbFilterBy.Items.Clear()
        txtFilterCaption.Clear()
        cmbFilterBy.Items.Add("RUNNO")
        Dim DT As New DataTable
        DT = GetSqlTable("SELECT DISTINCT COMPANYNAME FROM " & cnAdminDb & "..COMPANY", cn)
        chkLstCompany.Items.Clear()
        For i As Integer = 0 To DT.Rows.Count - 1
            chkLstCompany.Items.Add(DT.Rows(i).Item(0).ToString)
        Next

        cmbFilterBy.Text = ""
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Cmp_Str_() As String
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, True)
        Dim chkCmpName As String = GetChecked_CheckedList(chkLstCompany, True)
        Prop_Sets()
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_OUTST_LIST') IS NOT NULL DROP TABLE MASTER..TEMP_OUTST_LIST"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(200),NULL) AS PARTICULAR"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),(SELECT AREA FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS AREA"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),(SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS MOBILE"
        strSql += vbCrLf + " ,SUBSTRING(O.RUNNO,6,20)RUNNO,O.TRANNO,O.TRANDATE"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.DUEDATE ELSE NULL END AS [DUEDATE]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.DUEDATE ELSE O.TRANDATE END AS [DUEDATEORDER]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE NULL END AS [DUEAMT]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE NULL END AS [RECAMT]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN X.BALANCE ELSE NULL END AS [BALAMT]"
        strSql += vbCrLf + " ,O.REMARK1 AS NARRATION"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(8000),NULL)OUTSTANDINGSUMMARY"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS GROUP1"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS NAME"
        strSql += vbCrLf + " ,O.COMPANYID,O.COSTID"
        'Newly
        strSql += vbCrLf + "  ,CMP.COMPANYNAME,CMP.PHONE"
        '
        strSql += vbCrLf + " INTO MASTER..TEMP_OUTST_LIST"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strSql += vbCrLf + " INNER JOIN "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT RUNNO,COSTID,COMPANYID"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + " WHERE TRANTYPE ='D' AND ISNULL(CANCEL,'')=''"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " GROUP BY RUNNO,COSTID,COMPANYID"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)>0"
        strSql += vbCrLf + " )X ON X.RUNNO = O.RUNNO AND X.COMPANYID = O.COMPANYID AND X.COSTID = O.COSTID"
        'Newly
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..COMPANY AS CMP ON CMP.COMPANYID = O.COMPANYID"
        '
        strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE DUEDATE "
        strSql += vbCrLf + " BETWEEN '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " AND '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " AND COMPANYID IN(SELECT COMPANYID FROM " + cnAdminDb + "..COMPANY WHERE COMPANYNAME IN (" + chkCmpName + ") ) AND RUNNO = O.RUNNO AND RECPAY = 'P' "
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " )"
        If Not chkWithReceiptDetail.Checked Then strSql += vbCrLf + " AND O.RECPAY <> 'R'"
        strSql += vbCrLf + " AND O.COMPANYID IN(SELECT COMPANYID FROM " + cnAdminDb + "..COMPANY WHERE COMPANYNAME IN (" + chkCmpName + ") )"
        If chkCostName <> "" Then strSql += vbCrLf + " AND O.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        If rbtDealerSmith.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('G','D'))"
        If rbtCustomer.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('C'))"
        If rbtOther.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('O'))"
        If cmbFilterBy.Text <> "" And txtFilterCaption.Text <> "" Then strSql += vbCrLf + " AND O." & cmbFilterBy.Text & " LIKE'%" & txtFilterCaption.Text & "%'"
        If rbtCredit.Checked Then
            strSql += vbCrLf + "AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMDETAIL)"
        ElseIf rbtTobe.Checked Then
            strSql += vbCrLf + "AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMDETAIL)"
        End If
        strSql += vbCrLf + " ORDER BY O.RUNNO,O.DUEDATE,O.TRANDATE"

        strSql += vbCrLf + " UPDATE MASTER..TEMP_OUTST_LIST SET PARTICULAR = NAME"
        'Newly
        If SMS_DUE_REMAINDER <> "" Then
            Dim Template As String = SMS_DUE_REMAINDER
            Template = Template.Replace("<NAME>", "'+ NAME +' ").
            Replace("<DUEAMT>", "'+ CAST(DUEAMT AS VARCHAR(100)) +'").
            Replace("<RECAMT>", "'+ CAST(RECAMT AS VARCHAR(100)) +'").
            Replace("<BALAMT>", "'+ CAST(BALAMT AS VARCHAR(100)) +'")
            strSql += vbCrLf + "UPDATE MASTER..TEMP_OUTST_LIST SET OUTSTANDINGSUMMARY = '" & Template & "'"
        Else
            strSql += vbCrLf + "UPDATE MASTER..TEMP_OUTST_LIST SET OUTSTANDINGSUMMARY = 'Dear ' + NAME + ' Total Outstanding Amount is Rs.' + CAST([BALAMT] AS VARCHAR(100)) + ' is ' + CAST( DUEDATE AS VARCHAR(100)) + ' ' + COMPANYNAME + ' AND ' + PHONE + ' If u paid pls ignore this sms'"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkWithReceiptDetail.Checked = False Then
            strSql = vbCrLf + "  IF OBJECT_ID('MASTER..TEMP_OUTST_LIST_REC') IS NOT NULL DROP TABLE MASTER..TEMP_OUTST_LIST_REC"
            strSql += vbCrLf + "   SELECT  SUBSTRING(O.RUNNO,6,20)RUNNO ,SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE NULL END) AS [RECAMT]"
            strSql += vbCrLf + "  INTO MASTER..TEMP_OUTST_LIST_REC "
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS O INNER JOIN "
            strSql += vbCrLf + "  ( SELECT RUNNO,COSTID,COMPANYID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
            strSql += vbCrLf + "   FROM " & cnAdminDb & "..OUTSTANDING "
            strSql += vbCrLf + "  WHERE TRANTYPE ='D' AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "  GROUP BY RUNNO,COSTID,COMPANYID"
            strSql += vbCrLf + "  HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)>0"
            strSql += vbCrLf + "  )X ON X.RUNNO = O.RUNNO AND X.COMPANYID = O.COMPANYID AND X.COSTID = O.COSTID"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..COMPANY AS CMP ON CMP.COMPANYID = O.COMPANYID"
            strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE DUEDATE "
            strSql += vbCrLf + " BETWEEN '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
            strSql += vbCrLf + " AND '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
            strSql += vbCrLf + " AND COMPANYID IN(SELECT COMPANYID FROM " + cnAdminDb + "..COMPANY WHERE COMPANYNAME IN (" + chkCmpName + ") ) AND RUNNO = O.RUNNO AND RECPAY = 'P' "
            If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " )" + vbCrLf + " AND O.RECPAY = 'R'"
            strSql += vbCrLf + " AND O.COMPANYID IN(SELECT COMPANYID FROM " + cnAdminDb + "..COMPANY WHERE COMPANYNAME IN (" + chkCmpName + ") )"
            If chkCostName <> "" Then strSql += vbCrLf + " AND O.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
            If rbtDealerSmith.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('G','D'))"
            If rbtCustomer.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('C'))"
            If rbtOther.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('O'))"
            If cmbFilterBy.Text <> "" And txtFilterCaption.Text <> "" Then strSql += vbCrLf + " AND O." & cmbFilterBy.Text & " LIKE'%" & txtFilterCaption.Text & "%'"
            strSql += vbCrLf + " GROUP BY O.RUNNO"

            strSql += vbCrLf + "  UPDATE A SET A.[RECAMT] = B.[RECAMT] FROM  TEMP_OUTST_LIST_REC B INNER JOIN TEMP_OUTST_LIST A ON B.RUNNO = A.RUNNO"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If


        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..TEMP_OUTST_LIST)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,0,'T'COLHEAD FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD,[DUEAMT],[RECAMT],[BALAMT])"
        strSql += vbCrLf + " SELECT GROUP1 + ' TOT',GROUP1,3,'S'COLHEAD,SUM([DUEAMT]),SUM([RECAMT]),SUM([BALAMT]) FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1 GROUP BY GROUP1"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD,[DUEAMT],[RECAMT],[BALAMT])"
        strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZZ'GROUP1,4,'G'COLHEAD,SUM([DUEAMT]),SUM([RECAMT]),SUM([BALAMT]) FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkWithReceiptDetail.Checked = True Then
            strSql = vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST (PARTICULAR,AREA,MOBILE,RUNNO,TRANNO,TRANDATE,DUEDATE,DUEDATEORDER,[DUEAMT]"
            strSql += vbCrLf + " ,[RECAMT], [BALAMT],GROUP1,COLHEAD,RESULT,COMPANYID,COSTID,OUTSTANDINGSUMMARY)"
            strSql += vbCrLf + "  SELECT PARTICULAR,AREA,MOBILE,RUNNO,NULL TRANNO,NULL TRANDATE,MAX(DUEDATE),NULL DUEDATEORDER, SUM(ISNULL([DUEAMT],0))"
            strSql += vbCrLf + " , SUM(ISNULL([RECAMT],0)), SUM(ISNULL([BALAMT],0)),GROUP1,COLHEAD,2 RESULT,COMPANYID,COSTID,OUTSTANDINGSUMMARY FROM MASTER..TEMP_OUTST_LIST"
            strSql += vbCrLf + " WHERE RESULT = 1 GROUP BY  PARTICULAR,AREA,MOBILE,RUNNO,GROUP1,COLHEAD,COMPANYID,COSTID,OUTSTANDINGSUMMARY"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " DELETE FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT * FROM MASTER..TEMP_OUTST_LIST"
            strSql += vbCrLf + " ORDER BY GROUP1,RESULT"
            If rbtOrderName.Checked Then strSql += ",NAME"
            If rbtArea.Checked Then strSql += ",AREA"
            If rbtduedate.Checked Then strSql += ",DUEDATEORDER"
            If rbtOrderRunNo.Checked Then
                strSql += " ,CONVERT(INT,SUBSTRING(RUNNO,2,LEN(RUNNO))),TRANDATE"
            End If

        Else
            strSql = vbCrLf + " SELECT * FROM MASTER..TEMP_OUTST_LIST"
            strSql += vbCrLf + " ORDER BY GROUP1,RESULT"
            If rbtOrderName.Checked Then strSql += ",NAME"
            If rbtArea.Checked Then strSql += ",AREA"
            If rbtduedate.Checked Then strSql += ",DUEDATEORDER"
            If rbtOrderRunNo.Checked Then
                strSql += " ,CONVERT(INT,SUBSTRING(RUNNO,2,LEN(RUNNO))),TRANDATE"
            End If
        End If

        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("SELECT", GetType(Boolean))
        dtGrid.Columns.Add(dtCol)
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
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
        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'gridview.RowTemplate.Height = 21
        gridview.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        'objGridShower.Text = "DUE DATE WISE CUSTOMER OUTSTANDING"
        Dim tit As String = "DUE DATE WISE CUSTOMER OUTSTANDING"
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += vbCrLf + IIf(chkCostName <> "", " FOR " & chkCostName.Replace("'", ""), "")
        lblTitle.Text = tit
        gridview.DataSource = Nothing
        gridview.DataSource = dtGrid
        'If gridview.Columns.Contains(colCheckBox) Then
        '    gridview.Columns.Remove(colCheckBox)
        'End If
        'colCheckBox.Width = 100
        'colCheckBox.Name = "Select"
        'gridview.Columns.Add(colCheckBox)
        'DataGridView_SummaryFormatting(gridview)
        TabMain.SelectedTab = TabView
        FunctionGridViewStyle()
        FillGridGroupStyle_KeyNoWise(gridview)
        gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridview.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridview.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None


        'objGridShower.lblTitle.Text = tit
        'objGridShower.StartPosition = FormStartPosition.CenterScreen
        'objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'Newly 14/09/2015
        '''''''''''''''''''''' ME ''''''''''''''''''''
        'If objGridShower.gridView.Columns.Contains(colCheckBox) Then
        '    objGridShower.gridView.Columns.Remove(colCheckBox)
        'End If
        'colCheckBox.Width = 100
        'colCheckBox.Name = "Select"
        'objGridShower.gridView.Columns.Add(colCheckBox)

        'FunctionGridViewStyle()
        '''''''''''''''''''''' ME ''''''''''''''''''''
        'objGridShower.FormReSize = False
        'objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = True
        'objGridShower.lblStatus.Text = "Press [U] for Update DueDate"
        'objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        ''''''''''''''''''''''''''''OLD
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        'objGridShower.gridView.Columns("DUEDATEORDER").Visible = False
        'FormatGridColumns(objGridShower.gridView, False, False, , False)
        ''''''''''''''''''''''''''''''''''''''''
        'objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        'objGridShower.Show()
        'objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        'objGridShower.FormReSize = True
        'objGridShower.FormReLocation = True
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        'Prop_Sets()
        'AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
    End Sub
    'Function Not Used
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).Visible = True
            Next
            For CNT As Integer = 13 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("REC AMT").Visible = chkWithReceiptDetail.Checked
            .Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            'FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If UCase(e.KeyChar) = "U" Then
            Dim Dgv As DataGridView = CType(sender, DataGridView)
            If Dgv.CurrentRow.Cells("DUEDATE").Value.ToString = "" Then Exit Sub
            ''Update
            Dim objUpdator As New DueDateUpdator
            objUpdator.Text = "Update Duedate for " & Dgv.CurrentRow.Cells("PARTICULAR").Value.ToString & " (" & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & ")"
            objUpdator.MinDate = Dgv.CurrentRow.Cells("DUEDATE").Value
            objUpdator.dtpDuedate.Value = Dgv.CurrentRow.Cells("DUEDATE").Value
            If objUpdator.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    strSql = "SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..OUTSTDT_DUEDATE_AUDIT"
                    strSql += vbCrLf + " WHERE "
                    strSql += vbCrLf + " RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    Dim EntrOrder As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..OUTSTDT_DUEDATE_AUDIT"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " RUNNO"
                    strSql += vbCrLf + " ,DUEDATE"
                    strSql += vbCrLf + " ,NARRATION"
                    strSql += vbCrLf + " ,COSTID"
                    strSql += vbCrLf + " ,COMPANYID"
                    strSql += vbCrLf + " ,ENTRYORDER"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'" 'RUNNO
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("DUEDATE").Value & "'" 'DUEDATE
                    strSql += vbCrLf + " ,'" & objUpdator.txtNarration.Text & "'" 'NARRATION
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'" 'COSTID
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'" 'COMPANYID
                    strSql += vbCrLf + " ," & EntrOrder & "" 'entryorder
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " UPDATE " & cnAdminDb & "..OUTSTANDING SET DUEDATE = '" & objUpdator.dtpDuedate.Value.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " WHERE RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Dgv.CurrentRow.Cells("COSTID").Value.ToString)
                    tran.Commit()
                    tran = Nothing
                    Dgv.CurrentRow.Cells("DUEDATE").Value = objUpdator.dtpDuedate.Value
                    Dgv.CurrentRow.Cells("NARRATION").Value = objUpdator.txtNarration.Text
                    MsgBox("Updated Successfully", MsgBoxStyle.Information)
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox(ex.Message)
                End Try
            End If
        End If
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New DueDateWiseOutstandingRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(DueDateWiseOutstandingRpt_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        chkWithReceiptDetail.Checked = obj.p_chkWithReceiptDetail
        rbtOrderRunNo.Checked = obj.p_rbtOrderRunNo
        'rbtOrderName.Checked = obj.p_rbtOrderName
        rbtArea.Checked = obj.p_rbtArea
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New DueDateWiseOutstandingRpt_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        obj.p_chkWithReceiptDetail = chkWithReceiptDetail.Checked
        obj.p_rbtOrderRunNo = rbtOrderRunNo.Checked
        obj.p_rbtOrderName = rbtOrderName.Checked
        obj.p_rbtArea = rbtArea.Checked
        SetSettingsObj(obj, Me.Name, GetType(DueDateWiseOutstandingRpt_Properties))
    End Sub

    Private Sub FunctionGridViewStyle()
        With gridview
            With .Columns("Select")
                .Width = 60
                .HeaderText = "SELECT"
                .DisplayIndex = 0
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = False
            End With

            With .Columns("PARTICULAR")
                .Width = 60
                .HeaderText = "PARTICULAR"
                .DisplayIndex = 1
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("AREA")
                .Width = 60
                .HeaderText = "AREA"
                .DisplayIndex = 2
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("MOBILE")
                .Width = 60
                .HeaderText = "MOBILE"
                .DisplayIndex = 3
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("RUNNO")
                .Width = 60
                .HeaderText = "RUNNO"
                .DisplayIndex = 4
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("TRANNO")
                .Width = 60
                .HeaderText = "TRANNO"
                .DisplayIndex = 5
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("TRANDATE")
                .Width = 60
                .HeaderText = "TRANDATE"
                .DisplayIndex = 6
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .ReadOnly = True
            End With

            With .Columns("DUEDATE")
                .Width = 60
                .HeaderText = "DUEDATE"
                .DisplayIndex = 7
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .ReadOnly = True
            End With

            With .Columns("DUEDATEORDER")
                .Width = 60
                .HeaderText = "DUEDATEORDER"
                .DisplayIndex = 8
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
                .Visible = False
            End With

            With .Columns("DUEAMT")
                .Width = 60
                .HeaderText = "DUEAMT"
                .DisplayIndex = 9
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
            End With

            With .Columns("RECAMT")
                .Width = 60
                .HeaderText = "RECAMT"
                .DisplayIndex = 10
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                'Visible = chkWithReceiptDetail.Checked
            End With

            With .Columns("BALAMT")
                .Width = 60
                .HeaderText = "BALAMT"
                .DisplayIndex = 11
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
            End With

            With .Columns("OUTSTANDINGSUMMARY")
                .Width = 60
                .HeaderText = "OUTSTANDING SUMMARY"
                .DisplayIndex = 13
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
            End With

            With .Columns("KEYNO")
                .Visible = False
            End With

            With .Columns("COMPANYNAME")
                .Visible = False
            End With
            With .Columns("PHONE")
                .Visible = False
            End With
            With .Columns("GROUP1")
                .Visible = False
            End With
            With .Columns("COLHEAD")
                .Visible = False
            End With
            With .Columns("RESULT")
                .Visible = False
            End With
            With .Columns("NAME")
                .Visible = False
            End With
            With .Columns("COMPANYID")
                .Visible = False
            End With
            With .Columns("COSTID")
                .Visible = False
            End With
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Name = "SELECT" Then
                    .Columns(cnt).ReadOnly = False
                Else
                    .Columns(cnt).ReadOnly = True
                End If
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            'For cnt As Integer = 0 To gridview.ColumnCount - 1
            '    gridview.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            'Next
        End With
    End Sub

    Private Sub gridview_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridview.CellFormatting
        Dim temp As String = gridview.Rows(e.RowIndex).Cells("RESULT").Value.ToString
        If gridview.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "S" Then
            gridview.Rows(e.RowIndex).ReadOnly = True
        ElseIf gridview.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "T" Then
            gridview.Rows(e.RowIndex).ReadOnly = True
        ElseIf gridview.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "G" Then
            gridview.Rows(e.RowIndex).ReadOnly = True
        End If
    End Sub

    Private Sub gridview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If UCase(e.KeyChar) = "U" Then
            Dim Dgv As DataGridView = CType(sender, DataGridView)
            If Dgv.CurrentRow.Cells("DUEDATE").Value.ToString = "" Then Exit Sub
            ''Update
            Dim objUpdator As New DueDateUpdator
            objUpdator.Text = "Update Duedate for " & Dgv.CurrentRow.Cells("PARTICULAR").Value.ToString & " (" & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & ")"
            objUpdator.MinDate = Dgv.CurrentRow.Cells("DUEDATE").Value
            objUpdator.dtpDuedate.Value = Dgv.CurrentRow.Cells("DUEDATE").Value
            If objUpdator.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    strSql = "SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..OUTSTDT_DUEDATE_AUDIT"
                    strSql += vbCrLf + " WHERE "
                    strSql += vbCrLf + " RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    Dim EntrOrder As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..OUTSTDT_DUEDATE_AUDIT"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " RUNNO"
                    strSql += vbCrLf + " ,DUEDATE"
                    strSql += vbCrLf + " ,NARRATION"
                    strSql += vbCrLf + " ,COSTID"
                    strSql += vbCrLf + " ,COMPANYID"
                    strSql += vbCrLf + " ,ENTRYORDER"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'" 'RUNNO
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("DUEDATE").Value & "'" 'DUEDATE
                    strSql += vbCrLf + " ,'" & objUpdator.txtNarration.Text & "'" 'NARRATION
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'" 'COSTID
                    strSql += vbCrLf + " ,'" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'" 'COMPANYID
                    strSql += vbCrLf + " ," & EntrOrder & "" 'entryorder
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " UPDATE " & cnAdminDb & "..OUTSTANDING SET DUEDATE = '" & objUpdator.dtpDuedate.Value.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + ",REMARK1='" & objUpdator.txtNarration.Text.ToString & "'"
                    strSql += vbCrLf + " WHERE RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Dgv.CurrentRow.Cells("COSTID").Value.ToString)
                    tran.Commit()
                    tran = Nothing
                    Dgv.CurrentRow.Cells("DUEDATE").Value = objUpdator.dtpDuedate.Value
                    Dgv.CurrentRow.Cells("NARRATION").Value = objUpdator.txtNarration.Text.ToString
                    MsgBox("Updated Successfully", MsgBoxStyle.Information)
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox(ex.Message)
                End Try
            End If
        End If
    End Sub

    Private Sub btnSendSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendSms.Click
        If gridview.RowCount = 0 Then Exit Sub
        Dim Send As Boolean = False
        Dim dt As New DataTable
        dt = gridview.DataSource
        Dim Mobile As String = ""
        Dim Name As String = ""
        Dim TempMsg As String = ""
        Dim Amount As Decimal
        Dim Temp As Boolean = False

        If gridview.Rows.Count > 0 Then
            For i As Integer = 0 To gridview.Rows.Count - 1
                With gridview.Rows(i)
                    If CType(gridview.Rows(i).Cells("SELECT").Value.ToString, String) = "" Then
                        Continue For
                    End If
                    TempMsg = .Cells("OUTSTANDINGSUMMARY").Value.ToString
                    Mobile = .Cells("MOBILE").Value.ToString
                    If SmsSend(TempMsg, Mobile) = True Then
                        Send = True
                    Else
                        Exit For
                    End If
                End With
            Next
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            Return
        End If
        If Send Then MsgBox("Sms Generated.", MsgBoxStyle.Information)
    End Sub
    Public Function SmsSend(ByVal Msg As String, ByVal Mobile As String) As Boolean
        If Msg <> "" And Mobile.Length = 10 Then
            strSql = "SELECT COUNT(*)CNT FROM SYSDATABASES WHERE NAME='AKSHAYASMSDB'"
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                strSql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,EXPIRYDATE,UPDATED)"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Mobile.Trim & "','" & Msg & "','N'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ) "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                Return True
            Else
                MsgBox("AkhayaSmsDb Not Found", MsgBoxStyle.Information)
                Return False
            End If
        Else
            Return True
        End If
    End Function

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        TabMain.SelectedTab = TabGen
        chkAsOnDate.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridview.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridview.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class

Public Class DueDateWiseOutstandingRpt_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
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
    Private chkWithReceiptDetail As Boolean = False
    Public Property p_chkWithReceiptDetail() As Boolean
        Get
            Return chkWithReceiptDetail
        End Get
        Set(ByVal value As Boolean)
            chkWithReceiptDetail = value
        End Set
    End Property
    Private rbtOrderRunNo As Boolean = True
    Public Property p_rbtOrderRunNo() As Boolean
        Get
            Return rbtOrderRunNo
        End Get
        Set(ByVal value As Boolean)
            rbtOrderRunNo = value
        End Set
    End Property
    Private rbtOrderName As Boolean = False
    Public Property p_rbtOrderName() As Boolean
        Get
            Return rbtOrderName
        End Get
        Set(ByVal value As Boolean)
            rbtOrderName = value
        End Set
    End Property
    Private rbtArea As Boolean = False
    Public Property p_rbtArea() As Boolean
        Get
            Return rbtArea
        End Get
        Set(ByVal value As Boolean)
            rbtArea = value
        End Set
    End Property
End Class