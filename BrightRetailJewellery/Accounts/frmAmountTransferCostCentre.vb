Imports System.Data.OleDb
Public Class frmAmountTransferCostCentre
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim Batchno As String = ""
    Dim crAccode As String = ""
    Dim drAccode As String = ""
    Dim Syncdb As String = cnAdminDb
    Dim dtk As New DataTable
    Private Sub frmAmountTranToCostCentre_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
    End Sub

    Private Function funcNew()
        txtTranno_NUM.Text = ""
        lblAcName.Text = ""
        Dgv.DataSource = Nothing
        cmbCostCentre_MAN.Focus()
        If funcCheckCostCentreStatusFalse() Then
            MsgBox("There is no CostCentre to Transfer...", MsgBoxStyle.Information)
            Me.Close()
        End If
        cmbCostCentre_MAN.Items.Clear()
        If cmbCostCentre_MAN.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN ('" & cnCostId & "') ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, False, False)
        End If
    End Function

    Private Function funcSave()
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            Dim Tranno As Integer
            Dim Trfamount As Double = 0
            Tranno = GetBillNoValue("GEN-JOURNAL", tran)
            Batchno = GetNewBatchno(cnCostId, GetServerDate(tran), tran)
            Trfamount = Val(Dgv.CurrentRow.Cells("AMOUNT").Value.ToString)
            Dim Remark1 As String = "REVERSE ENTRY"
            Dim RefBatchno As String = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString
            InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "D", "C"), _
                                    drAccode, Trfamount, "JE", crAccode, Batchno, RefBatchno, , , , , , Remark1, , , , , "")
            InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "C", "D"), _
                            crAccode, Trfamount, "JE", drAccode, Batchno, RefBatchno, , , , , , Remark1, , , , , "")
            strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET TRANSFERED='Y' WHERE BATCHNO='" & RefBatchno & "' "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            MsgBox("Successfully Transfered.", MsgBoxStyle.Information)
            funcNew()
            Exit Function
        Catch ex As Exception
            MsgBox(ex.Message)
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
        End Try
    End Function

    Private Sub Acctran_transfer(ByVal Tranno As String, ByVal Paymode As String, ByVal batchno As String, ByVal fromCostId As String, ByVal Tocostid As String, ByVal AMOUNT As String, ByVal drcode As String, ByVal crcode As String, ByVal Runno As String)
        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPACCTRAN', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPACCTRAN FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim Recpay As String = "R"
        Dim Ttype As String = IIf(AMOUNT > 0, "C", "D")
        strSql = " INSERT INTO " & cnStockDb & "..INS_TEMPACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE"
        strSql += " ,AMOUNT"
        strSql += " ,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,REMARK1,ACCODE,CONTRA,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & Tranno & "" 'TRANNO
        strSql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
        strSql += " ,'" & Ttype & "'" 'TRANTYPE
        strSql += " ," & AMOUNT & "" 'AMOUNT
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'INTERBRANCH TRANSFER'" 'REMARK1
        strSql += " ,'" & drcode & "'" 'ACCODE
        strSql += " ,'" & crcode & "'" 'ACCODE
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Tocostid & "'" 'COSTID
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Paymode & "'"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        Ttype = IIf(AMOUNT > 0, "D", "C")
        strSql = " INSERT INTO " & cnStockDb & "..INS_TEMPACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE"
        strSql += " ,AMOUNT"
        strSql += " ,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,REMARK1,ACCODE,CONTRA,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & Tranno & "" 'TRANNO
        strSql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
        strSql += " ,'" & Ttype & "'" 'TRANTYPE
        strSql += " ," & AMOUNT & "" 'AMOUNT
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'INTERBRANCH TRANSFER'" 'REMARK1
        strSql += " ,'" & crcode & "'" 'ACCODE
        strSql += " ,'" & drcode & "'" 'ACCODE
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Tocostid & "'" 'COSTID
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Paymode & "'"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()


        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        Dim mtempqrytb As String = "TEMPQRYTB"
        strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'INS_TEMPACCTRAN',@MASK_TABLENAME = 'ACCTRAN',@TEMPTABLE='" & mtempqrytb & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        strSql += vbCrLf + " SELECT '" & fromcostid & "','" & tocostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim costids As String = "'" & fromCostId & "','" & Tocostid & "'"
        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID in (" & costids & ") AND MAIN = 'Y'"
        If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "0", tran) = "0" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mainCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran)
            strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            strSql += vbCrLf + " SELECT '" & fromCostId & "','" & mainCostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        'strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        'cmd = New OleDbCommand(strSql, cn, tran)
        'cmd.ExecuteNonQuery()
        'strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        'cmd = New OleDbCommand(strSql, cn, tran)
        'cmd.ExecuteNonQuery()
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

    End Sub

    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer, _
    ByVal tranMode As String, _
    ByVal accode As String, _
    ByVal amount As Double, _
    ByVal payMode As String, _
    ByVal contra As String, _
    ByVal Batchno As String, _
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
    Optional ByVal chqCardNo As String = Nothing, _
    Optional ByVal chqDate As String = Nothing, _
    Optional ByVal chqCardId As Integer = Nothing, _
    Optional ByVal chqCardRef As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing, _
    Optional ByVal baltopay As Double = Nothing, _
    Optional ByVal BillCashCounterId As String = Nothing, _
    Optional ByVal VATEXM As String = Nothing, _
    Optional ByVal Transfered As String = Nothing _
    )
        If amount = 0 Then Exit Sub

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,BALANCE"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " ,DISC_EMPID,TRANSFERED"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & baltopay & "" 'AMOUNT        
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,0" ' DISC_EMPID
        strSql += " ,'" & Transfered & "'" 'TRANSFERED
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = ""
        cmd = Nothing
    End Sub


    Private Function funcValidate() As Boolean
        With Dgv
            drAccode = .CurrentRow.Cells("CONTRA").Value.ToString
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text.ToString().Trim() & "'"
            If drAccode <> objGPack.GetSqlValue(strSql) Then
                drAccode = "" : MsgBox("Invalid Account...", MsgBoxStyle.Information) : Return False : Exit Function
            End If
            If crAccode = "" Then
                crAccode = "CASH"
            End If
            Return True
        End With
    End Function

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If Not Dgv.Rows.Count > 0 Then Exit Sub
        If funcValidate() = False Then Exit Sub
        funcSave()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub


    Private Sub cmbCostCentre_MAN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCostCentre_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
                strSql = " SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text.ToString().Trim() & "'"
                drAccode = objGPack.GetSqlValue(strSql)
                If drAccode = "" Then MsgBox("Please set A/c Name for CostCentre...", MsgBoxStyle.Information) : lblAcName.Text = "" : Exit Sub
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & drAccode & "'"
                lblAcName.Text = objGPack.GetSqlValue(strSql)
            End If
        End If
    End Sub

    Private Sub frmAmountTranToCostCentre_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If cmbCostCentre_MAN.Text.ToString = "" Then
            MsgBox("Costcentre should not be empty..")
            cmbCostCentre_MAN.Focus()
            Exit Sub
        End If

        Dim FromcostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "'")
        Dim CrContra As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "'")
        strSql = " SELECT "
        strSql += vbCrLf + " TRANDATE,TRANNO,AMOUNT,"
        strSql += vbCrLf + " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)ACNAME,"
        strSql += vbCrLf + " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.CONTRA)CONTRANAME,"
        strSql += vbCrLf + " TRANMODE,BATCHNO,ACCODE,CONTRA,COSTID,REMARK1,REMARK2 "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A "
        strSql += vbCrLf + " WHERE BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE 1=1 "
        strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN ('" & cnCostId & "'))"
        strSql += vbCrLf + " AND ISNULL(TRANSFERED,'') <> 'Y' "
        strSql += vbCrLf + " )"
        If txtTranno_NUM.Text.ToString <> "" Then
            strSql += vbCrLf + " AND TRANNO IN (" & txtTranno_NUM.Text.ToString & ") "
        End If
        If cmbCostCentre_MAN.Text.ToString <> "ALL" And cmbCostCentre_MAN.Text.ToString <> "" Then
            strSql += vbCrLf + " AND CONTRA IN ('" & CrContra & "')"
        End If
        strSql += vbCrLf + " AND PAYMODE IN ('JE','TR') AND TRANNO <> 9999 "
        strSql += vbCrLf + " AND TRANMODE ='D' AND ISNULL(REMARK1,'')<>'REVERSE ENTRY' "
        dtk = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtk)
        If dtk.Rows.Count > 0 Then
            Dgv.DataSource = Nothing
            Dgv.DataSource = dtk
            DgvFormat()
        Else
            Dgv.DataSource = Nothing
            MsgBox("Record not found...")
        End If

    End Sub

    Private Function DgvFormat()
        With Dgv
            If .Rows.Count > 0 Then
                If .Columns.Contains("ACCODE") = True Then .Columns("ACCODE").Visible = False
                If .Columns.Contains("CONTRA") = True Then .Columns("CONTRA").Visible = False
                If .Columns.Contains("BATCHNO") = True Then .Columns("BATCHNO").Visible = False
                If .Columns.Contains("COSTID") = True Then .Columns("COSTID").Visible = False
                If .Columns.Contains("TRANMODE") = True Then .Columns("TRANMODE").Visible = False
                If .Columns.Contains("TRANDATE") = True Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End With
    End Function

    Private Sub txtTranno_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranno_NUM.KeyDown

    End Sub

    Private Sub Dgv_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Dgv.KeyPress

    End Sub
End Class