Imports System.Data.OleDb
Public Class AccApproval
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim CheckState As Boolean
    Private WithEvents txtCheckedItems As New TextBox


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dgv.DataSource = Nothing
        dtpTrandate.Value = GetEntryDate(GetServerDate())
        cmbEntryType.Text = "ALL"
        txtCheckedItems.Clear()
        txtCheckedItems_TextChanged(Me, New EventArgs)
        dtpTrandate.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtCheckedItems_TextChanged(Me, New EventArgs)
        Me.Refresh()
        StrSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_ACC') IS NOT NULL DROP TABLE MASTER..TEMP_ACC"
        StrSql += vbCrLf + " CREATE TABLE MASTER..TEMP_ACC"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " TRANNO INT"
        StrSql += vbCrLf + " ,TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " ,ACNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,DEBIT NUMERIC(15,2)"
        StrSql += vbCrLf + " ,CREDIT NUMERIC(15,2)"
        StrSql += vbCrLf + " ,ISSUE NUMERIC(15,3)"
        StrSql += vbCrLf + " ,RECEIPT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,TEMP_ACNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,BATCHNO VARCHAR(15),AMOUNT NUMERIC(15,2),ORD INT,COMPANYID VARCHAR(3)"
        StrSql += vbCrLf + " ,PAYMODE VARCHAR(10),CTLID VARCHAR(50),RESULT INT"
        StrSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        If cmbEntryType.Text <> "ISSUE (METAL/DEALER)" And cmbEntryType.Text <> "RECEIPT (METAL/DEALER)" Then
            StrSql = vbCrLf + " INSERT INTO MASTER..TEMP_ACC"
            StrSql += vbCrLf + " (TRANNO,TRANDATE,ACNAME,DEBIT,CREDIT,BATCHNO,AMOUNT,ORD,PAYMODE,CTLID,COMPANYID)"
            StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,H.ACNAME"
            StrSql += vbCrLf + " ,CASE WHEN T.TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS DEBIT"
            StrSql += vbCrLf + " ,CASE WHEN T.TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS CREDIT"
            StrSql += vbCrLf + " ,T.BATCHNO,T.AMOUNT"
            StrSql += vbCrLf + " ,CASE WHEN T.PAYMODE IN ('CR','DN') AND T.TRANMODE = 'C' THEN 1 WHEN T.PAYMODE IN ('CR','DN') AND T.TRANMODE = 'D' THEN 2"
            StrSql += vbCrLf + " WHEN T.PAYMODE IN ('CP','CN') AND T.TRANMODE = 'D' THEN 1 WHEN T.PAYMODE IN ('CP','CN') AND T.TRANMODE = 'C' THEN 2"
            StrSql += vbCrLf + " ELSE 0 END AS ORD,T.PAYMODE"
            StrSql += vbCrLf + " ,'GEN-' + T.PAYMODE AS CTLID,T.COMPANYID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..TACCTRAN AS T"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            StrSql += vbCrLf + " WHERE T.TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND FROMFLAG = 'A'"
            If cmbEntryType.Text <> "ALL" And cmbEntryType.Text <> "" Then
                StrSql += vbCrLf + " AND PAYMODE = (SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE CAPTION = '" & cmbEntryType.Text & "')"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        If cmbEntryType.Text = "ALL" Or cmbEntryType.Text = "" Or cmbEntryType.Text = "ISSUE (METAL/DEALER)" Then
            StrSql = vbCrLf + " INSERT INTO MASTER..TEMP_ACC"
            StrSql += vbCrLf + " (TRANNO,TRANDATE,ACNAME,ISSUE,BATCHNO,PAYMODE,CTLID,COMPANYID)"
            StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,H.ACNAME"
            StrSql += vbCrLf + " ,SUM(GRSWT) ISSUE"
            StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
            StrSql += vbCrLf + " ,'GEN-SM-ISS' AS CTLID,T.COMPANYID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..TISSUE AS T"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            StrSql += vbCrLf + " WHERE T.TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND LEN(T.TRANTYPE)>2"
            StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        If cmbEntryType.Text = "ALL" Or cmbEntryType.Text = "" Or cmbEntryType.Text = "RECEIPT (METAL/DEALER)" Then
            StrSql = vbCrLf + " INSERT INTO MASTER..TEMP_ACC"
            StrSql += vbCrLf + " (TRANNO,TRANDATE,ACNAME,RECEIPT,BATCHNO,PAYMODE,CTLID,COMPANYID)"
            StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,H.ACNAME"
            StrSql += vbCrLf + " ,SUM(GRSWT) RECEIPT"
            StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
            StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,T.COMPANYID"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..TRECEIPT AS T"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            StrSql += vbCrLf + " WHERE T.TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND LEN(T.TRANTYPE)>2"
            StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If

        StrSql = vbCrLf + " SELECT * FROM MASTER..TEMP_ACC"
        StrSql += vbCrLf + " ORDER BY TRANDATE,SUBSTRING(BATCHNO,8,10),TRANNO,AMOUNT,ORD,DEBIT"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("APPROVED", GetType(Boolean))
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("APPROVED").SetOrdinal(0)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        Dgv.DataSource = dtGrid
        With Dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).ReadOnly = True
            Next
            For CNT As Integer = 8 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("APPROVED").ReadOnly = False
            .Columns("APPROVED").Width = 70
            .Columns("TRANNO").Width = 60
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("ACNAME").Width = 350
            .Columns("DEBIT").Width = 100
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").Width = 100
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE").Width = 100
            .Columns("ISSUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT").Width = 100
            .Columns("RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        Dgv.Select()
    End Sub

    Private Sub AccApproval_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub AccApproval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AccApproval_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbEntryType.Items.Clear()
        StrSql = " SELECT CAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER"
        cmbEntryType.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbEntryType, False, False)
        cmbEntryType.Items.Add("ISSUE (METAL/DEALER)")
        cmbEntryType.Items.Add("RECEIPT (METAL/DEALER)")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv.CellValueChanged
        If CheckState = False Then Exit Sub
        Dim bool As Boolean = CType(Dgv.CurrentRow.Cells("APPROVED").Value, Boolean)
        Dim batchno As String = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "BATCHNO = '" & batchno & "'"
        Dim dt As New DataTable
        dt = dv.ToTable
        CheckState = True
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If bool Then
                txtCheckedItems.Text = Val(txtCheckedItems.Text) + 1
                Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            Else
                txtCheckedItems.Text = Val(txtCheckedItems.Text) - 1
                Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.White
            End If
            Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).Cells("APPROVED").Value = bool
        Next
    End Sub

    Private Sub Dgv_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv.CurrentCellDirtyStateChanged
        CheckState = True
        Dgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub txtCheckedItems_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCheckedItems.TextChanged
        If Val(txtCheckedItems.Text) = 0 Then
            btnTransfer.Enabled = False
        Else
            btnTransfer.Enabled = True
        End If
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim TranDate As Date = GetEntryDate(GetServerDate)
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "APPROVED = 'TRUE'"
        Dim dt As New DataTable
        dt = dv.ToTable
        dt = dt.DefaultView.ToTable(True, New String() {"BATCHNO", "CTLID", "COMPANYID"})
        Dim batchno As String = Nothing
        Dim CtlId As String = Nothing
        Dim TranNo As Integer = Nothing
        Dim CostId As String = Nothing
        Dim tCompanyId As String = Nothing
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For cnt As Integer = 0 To dt.Rows.Count - 1
                CtlId = dt.Rows(cnt).Item("CTLID").ToString
                batchno = dt.Rows(cnt).Item("BATCHNO").ToString
                tCompanyId = dt.Rows(cnt).Item("COMPANYID").ToString
                Dim Isfirst As Boolean = True
GenBillNo:
                TranNo = Val(GetBillControlValue(CtlId, tran, Not Isfirst))
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                StrSql += " WHERE CTLID = '" & CtlId & "' AND COMPANYID = '" & tCompanyId & "'"
                If strBCostid <> Nothing And Isfirst Then StrSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
                StrSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Not Cmd.ExecuteNonQuery() > 0 Then
                    Isfirst = False
                    GoTo GenBillNo
                End If
                TranNo += 1
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ACCTRAN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_OUTSTANDING') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSUE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPT') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT * INTO " & cnStockDb & "..TEMP_APP_ACCTRAN FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_OUTSTANDING FROM " & cnStockDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSUE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPT FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & " WHERE  BATCHNO = '" & batchno & "'"
                StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & " WHERE  BATCHNO = '" & batchno & "'"
                StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & " WHERE  BATCHNO = '" & batchno & "'"
                StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & " WHERE  BATCHNO = '" & batchno & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT COSTID FROM "
                StrSql += " ("
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " )X WHERE ISNULL(COSTID,'') <> ''"
                CostId = objGPack.GetSqlValue(StrSql, "", , tran)
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ACCTRANCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ACCTRAN'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'OUTSTANDINGCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'OUTSTANDING'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPT'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                Dim dtTemp As New DataTable
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ACCTRAN',@MASK_TABLENAME = 'ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_OUTSTANDING',@MASK_TABLENAME = 'OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ISSUE',@MASK_TABLENAME = 'ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPT',@MASK_TABLENAME = 'RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " DELETE FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)

                StrSql = " DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Approval Completed", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub
End Class