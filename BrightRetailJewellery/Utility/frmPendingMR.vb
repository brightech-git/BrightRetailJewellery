Imports System.Data.OleDb
Public Class frmPendingMR
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim dtCompanyName As DataTable
    Dim CheckState As Boolean
    Private WithEvents txtCheckedItems As New TextBox
    Dim LOCK_MRMI_TRSDATE As Boolean = IIf(GetAdmindbSoftValue("LOCK_MRMI_TRSDATE", "Y") = "Y", True, False)
    Dim PENDTRF_SNOBASE As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_SNOBASE", "N") = "Y", True, False)

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtTranNo.Clear()
        txtTranNo.Focus()
        lblHelp.Text = "LOCK_MRMI_TRSDATE:" & IIf(LOCK_MRMI_TRSDATE = True, "Y", "N")
        txtCheckedItems_TextChanged(Me, New EventArgs)
        If LOCK_MRMI_TRSDATE = False Then
            lblTranDate.Enabled = True
            dtpTransfer.Enabled = True
            dtpTransfer.Value = GetEntryDate(GetServerDate())
        End If
        'REFRESH COMPANY NAME
        GiritechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , "ALL")
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
        StrSql += vbCrLf + " ,CATNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,PCS INT"
        StrSql += vbCrLf + " ,GRSWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,NETWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,DIAPCS INT"
        StrSql += vbCrLf + " ,DIAWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,STNPCS INT"
        StrSql += vbCrLf + " ,STNWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,PREPCS INT"
        StrSql += vbCrLf + " ,PREWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,AMOUNT NUMERIC(15,2)"
        StrSql += vbCrLf + " ,TEMP_ACNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,BATCHNO VARCHAR(15),ORD INT,COMPANYID VARCHAR(3)"
        StrSql += vbCrLf + " ,PAYMODE VARCHAR(10),CTLID VARCHAR(50),RESULT INT"
        StrSql += vbCrLf + " ,TYPE VARCHAR(12)"
        StrSql += vbCrLf + " ,SNO VARCHAR(20)"
        StrSql += vbCrLf + " ,COSTNAME VARCHAR(100)"
        StrSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO MASTER..TEMP_ACC"
        StrSql += vbCrLf + " (TRANNO,TRANDATE,CATNAME,ACNAME,PCS,GRSWT"
        StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT"
        StrSql += vbCrLf + " ,BATCHNO,PAYMODE,CTLID,COMPANYID,RESULT,TYPE,SNO,COSTNAME)"
        StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,C.CATNAME,H.ACNAME"
        StrSql += vbCrLf + " ,SUM(PCS) PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
        StrSql += vbCrLf + " ,SUM(NETWT) NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
        StrSql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
        StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,T.COMPANYID"
        StrSql += vbCrLf + " ,0"
        StrSql += vbCrLf + " ,REMARK2,T.SNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ACCODE=T.ACCODE)COSTNAME"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TRECEIPT AS T"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = T.CATCODE"
        StrSql += vbCrLf + " WHERE LEN(T.TRANTYPE)>2"
        If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TRFNO='" & txtTranNo.Text & "'"
        If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
            StrSql += vbCrLf + " AND T.COMPANYID IN"
            StrSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
        End If
        StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,C.CATNAME,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID,T.SNO,T.REMARK2,T.ACCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " IF(SELECT COUNT(*) FROM MASTER..TEMP_ACC)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + "    INSERT INTO MASTER..TEMP_ACC"
        StrSql += vbCrLf + "    (CATNAME,PCS,GRSWT"
        StrSql += vbCrLf + "    ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT,ORD,RESULT)"
        StrSql += vbCrLf + "    SELECT 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),SUM(AMOUNT),9,2"
        StrSql += vbCrLf + "    FROM MASTER..TEMP_ACC"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " UPDATE MASTER..TEMP_ACC SET PCS=NULL WHERE PCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET STNPCS=NULL WHERE STNPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET DIAPCS=NULL WHERE DIAPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET PREPCS=NULL WHERE PREPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET GRSWT=NULL WHERE GRSWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET NETWT=NULL WHERE NETWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET DIAWT=NULL WHERE DIAWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET STNWT=NULL WHERE STNWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET PREWT=NULL WHERE PREWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET AMOUNT=NULL WHERE AMOUNT=0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * FROM MASTER..TEMP_ACC"
        StrSql += vbCrLf + " ORDER BY RESULT,TRANDATE,SUBSTRING(BATCHNO,8,10),TRANNO,AMOUNT,ORD"
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
            For Each dgvRow As DataGridViewRow In Dgv.Rows
                Select Case dgvRow.Cells("RESULT").Value.ToString
                    Case "2"
                        dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                        dgvRow.DefaultCellStyle.ForeColor = Color.Red
                        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            Next
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).ReadOnly = True
            Next
            For CNT As Integer = 9 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("APPROVED").ReadOnly = False
            .Columns("APPROVED").HeaderText = "CHECK"
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").Visible = True
            .Columns("DIAWT").Visible = True
            .Columns("STNPCS").Visible = True
            .Columns("STNWT").Visible = True
            .Columns("PREPCS").Visible = True
            .Columns("PREWT").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("ACNAME").Visible = False
            .Columns("TYPE").Visible = True
            .Columns("COSTNAME").Visible = True
            .Columns("SNO").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
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
        'LOAD COMPANY       
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPNAYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompanyName = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCompanyName)
        GiritechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv.CellValueChanged
        If CheckState = False Then Exit Sub
        Dim bool As Boolean = CType(IIf(IsDBNull(Dgv.CurrentRow.Cells("APPROVED").Value), False, Dgv.CurrentRow.Cells("APPROVED").Value), Boolean)
        Dim Sno As String = Nothing
        Dim BatNo As String = Nothing
        Dim dv As New DataView
        BatNo = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        If PENDTRF_SNOBASE <> False Then
            Sno = Dgv.CurrentRow.Cells("SNO").Value.ToString
            dv = New DataView
            dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "' AND SNO='" & Sno & "'"
        Else
            dv = New DataView
            dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "'"
        End If
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

    Private Function GetBillControlValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction, ByVal CompanyId As String, Optional ByVal Nextiter As Boolean = False) As String
        If strBCostid <> Nothing And Not Nextiter Then
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & CompanyId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran))
        Else
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & CompanyId & "'", , , tran))
        End If

    End Function

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim TranDate As Date = GetEntryDate(GetServerDate)
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "APPROVED = 'TRUE' AND RESULT=0"
        Dim dt As New DataTable
        dt = dv.ToTable
        dt = dt.DefaultView.ToTable(True, New String() {"BATCHNO", "CTLID", "COMPANYID"})
        Dim batchno As String = Nothing
        Dim NewBatchNo As String = Nothing
        Dim CtlId As String = Nothing
        Dim TranNo As Integer = Nothing
        Dim CostId As String = Nothing
        Dim tCompanyId As String = Nothing
        Dim billcontrolid As String = "GEN-SM-INTREC"
        StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then StrSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(StrSql, , , tran)) <> "Y" Then
            billcontrolid = "GEN-STKREFNO"
        End If
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
        If UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran)) = "Y" Then
            billcontrolid = "GEN-SM-REC"
        End If
        CtlId = billcontrolid
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For cnt As Integer = 0 To dt.Rows.Count - 1
                'CtlId = dt.Rows(cnt).Item("CTLID").ToString
                batchno = dt.Rows(cnt).Item("BATCHNO").ToString
                tCompanyId = dt.Rows(cnt).Item("COMPANYID").ToString
                NewBatchNo = GetNewBatchno(cnCostId, IIf(LOCK_MRMI_TRSDATE, TranDate, dtpTransfer.Value), tran)
GenBillNo:
                TranNo = Val(GetBillControlValue(CtlId, tran, tCompanyId))
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                StrSql += " WHERE CTLID = '" & CtlId & "' AND COMPANYID = '" & tCompanyId & "'"
                StrSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Not Cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If
                TranNo += 1
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ACCTRAN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_OUTSTANDING') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSUE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPT') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPTSTONE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT * INTO " & cnStockDb & "..TEMP_APP_ACCTRAN FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_OUTSTANDING FROM " & cnAdminDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSUE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPT FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
                If LOCK_MRMI_TRSDATE = False Then
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                Else
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                End If
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

                'StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE SET ISSSNO="
                'StrSql += vbCrLf + " (SELECT TOP 1 SNO FROM " & cnStockDb & "..TEMP_APP_RECEIPT WHERE BATCHNO = '" & NewBatchNo & "')"
                'StrSql += vbCrLf + " WHERE BATCHNO='" & NewBatchNo & "'"

                StrSql = vbCrLf + " UPDATE TS SET ISSSNO=T.SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_RECEIPTSTONE TS," & cnStockDb & "..TEMP_APP_RECEIPT T"
                StrSql += vbCrLf + " WHERE T.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.ISSSNO=T.ISSNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTSTONECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPTSTONE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPTSTONE'"
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
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPTSTONE',@MASK_TABLENAME = 'RECEIPTSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " DELETE FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)

                StrSql = " DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Receipt Transfer Completed", MsgBoxStyle.Information)
            Dim Pbatchno As String = NewBatchNo
            Dim Pbilldate As Date
            If LOCK_MRMI_TRSDATE = False Then
                Pbilldate = dtpTransfer.Value
            Else
                Pbilldate = TranDate
            End If
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":RIN")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & Pbatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":RIN" & ";" & _
                    LSet("BATCHNO", 15) & ":" & Pbatchno & ";" & _
                    LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtTranNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = "SELECT DISTINCT TRFNO FROM " & cnStockDb & "..TRECEIPT"
            Dim TrfNo As String = GiritechPack.SearchDialog.Show("Select Transfer No", StrSql, cn, 0)
            If TrfNo <> "" Then
                txtTranNo.Text = TrfNo
                txtTranNo.SelectAll()
            End If
        End If
    End Sub
End Class