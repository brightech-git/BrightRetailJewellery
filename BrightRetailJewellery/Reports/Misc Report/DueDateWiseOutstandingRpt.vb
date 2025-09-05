Imports System.Data.OleDb
Public Class DueDateWiseOutstandingRpt
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub DueDateWiseOutstandingRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DueDateWiseOutstandingRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
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
        Prop_Gets()
        chkAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, True)
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_OUTST_LIST') IS NOT NULL DROP TABLE MASTER..TEMP_OUTST_LIST"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(200),NULL) AS PARTICULAR"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),(SELECT AREA FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS AREA"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),(SELECT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS MOBILE"
        strSql += vbCrLf + " ,SUBSTRING(O.RUNNO,6,20)RUNNO"
        strSql += vbCrLf + " ,O.TRANNO,O.TRANDATE"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.DUEDATE ELSE NULL END AS [DUEDATE]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.DUEDATE ELSE O.TRANDATE END AS [DUEDATEORDER]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE NULL END AS [DUE AMT]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE NULL END AS [REC AMT]"
        strSql += vbCrLf + " ,CASE WHEN O.RECPAY = 'P' THEN X.BALANCE ELSE NULL END AS [BAL AMT]"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnadmindb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS GROUP1"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))) AS NAME"
        strSql += vbCrLf + " ,O.COMPANYID,O.COSTID"
        strSql += vbCrLf + " INTO MASTER..TEMP_OUTST_LIST"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OUTSTANDING AS O"
        strSql += vbCrLf + " INNER JOIN "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT RUNNO,COSTID,COMPANYID"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..OUTSTANDING "
        strSql += vbCrLf + " WHERE TRANTYPE ='D'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " GROUP BY RUNNO,COSTID,COMPANYID"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)>0"
        strSql += vbCrLf + " )X ON X.RUNNO = O.RUNNO AND X.COMPANYID = O.COMPANYID AND X.COSTID = O.COSTID"
        strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & cnStockDb & "..OUTSTANDING WHERE DUEDATE "
        strSql += vbCrLf + " BETWEEN '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " AND '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "' AND RUNNO = O.RUNNO AND RECPAY = 'P' "
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " )"
        If Not chkWithReceiptDetail.Checked Then strSql += vbCrLf + " AND O.RECPAY <> 'R'"
        strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND O.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        If rbtDealerSmith.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('G','D'))"
        If rbtCustomer.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('C'))"
        If rbtOther.Checked Then strSql += vbCrLf + " AND O.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('O'))"
        strSql += vbCrLf + " ORDER BY O.RUNNO,O.DUEDATE,O.TRANDATE"

        strSql += vbCrLf + " UPDATE MASTER..TEMP_OUTST_LIST SET PARTICULAR = NAME"

        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..TEMP_OUTST_LIST)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,0,'T'COLHEAD FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD,[DUE AMT],[REC AMT],[BAL AMT])"
        strSql += vbCrLf + " SELECT GROUP1 + ' TOT',GROUP1,2,'S'COLHEAD,SUM([DUE AMT]),SUM([REC AMT]),SUM([BAL AMT]) FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1 GROUP BY GROUP1"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_OUTST_LIST(PARTICULAR,GROUP1,RESULT,COLHEAD,[DUE AMT],[REC AMT],[BAL AMT])"
        strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZZ'GROUP1,3,'G'COLHEAD,SUM([DUE AMT]),SUM([REC AMT]),SUM([BAL AMT]) FROM MASTER..TEMP_OUTST_LIST WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT * FROM MASTER..TEMP_OUTST_LIST"
        strSql += vbCrLf + " ORDER BY GROUP1,RESULT"
        If rbtOrderName.Checked Then strSql += ",NAME"
        If rbtArea.Checked Then strSql += ",AREA"
        If rbtduedate.Checked Then strSql += ",DUEDATEORDER"
        strSql += " ,CONVERT(INT,SUBSTRING(RUNNO,9,LEN(RUNNO))),TRANDATE"
        Dim dtGrid As New DataTable
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
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DUE DATE WISE CUSTOMER OUTSTANDING"
        Dim tit As String = "DUE DATE WISE CUSTOMER OUTSTANDING"
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += vbCrLf + IIf(chkCostName <> "", " FOR " & chkCostName.Replace("'", ""), "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.lblStatus.Text = "Press [U] for Update DueDate"
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridView.Columns("DUEDATEORDER").Visible = False
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Show()
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).Visible = True
            Next
            For CNT As Integer = 9 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("REC AMT").Visible = chkWithReceiptDetail.Checked
            .Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
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
                    strSql = "SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnStockDb & "..OUTSTDT_DUEDATE_AUDIT"
                    strSql += vbCrLf + " WHERE "
                    strSql += vbCrLf + " RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    Dim EntrOrder As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnStockDb & "..OUTSTDT_DUEDATE_AUDIT"
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
                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..OUTSTANDING SET DUEDATE = '" & objUpdator.dtpDuedate.Value.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " WHERE RUNNO = '" & GetCostId(Dgv.CurrentRow.Cells("COSTID").Value.ToString) & GetCompanyId(Dgv.CurrentRow.Cells("COMPANYID").Value.ToString) & Dgv.CurrentRow.Cells("RUNNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND COSTID = '" & Dgv.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    strSql += vbCrLf + " AND COMPANYID = '" & Dgv.CurrentRow.Cells("COMPANYID").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Dgv.CurrentRow.Cells("COSTID").Value.ToString)
                    tran.Commit()
                    tran = Nothing
                    Dgv.CurrentRow.Cells("DUEDATE").Value = objUpdator.dtpDuedate.Value
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
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        chkWithReceiptDetail.Checked = obj.p_chkWithReceiptDetail
        rbtOrderRunNo.Checked = obj.p_rbtOrderRunNo
        'rbtOrderName.Checked = obj.p_rbtOrderName
        rbtArea.Checked = obj.p_rbtArea
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New DueDateWiseOutstandingRpt_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        obj.p_chkWithReceiptDetail = chkWithReceiptDetail.Checked
        obj.p_rbtOrderRunNo = rbtOrderRunNo.Checked
        obj.p_rbtOrderName = rbtOrderName.Checked
        obj.p_rbtArea = rbtArea.Checked
        SetSettingsObj(obj, Me.Name, GetType(DueDateWiseOutstandingRpt_Properties))
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