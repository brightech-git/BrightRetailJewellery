Imports System.Data.OleDb
Imports System.IO
Public Class ReSendTransaction
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Tablename As New List(Of String)
    Private _Cn As OleDbConnection
    Private _Tran As OleDbTransaction = Nothing
    Dim Syncdb As String = cnAdminDb

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ReSendTransaction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ReSendTransaction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me._Tran = tran
        Me._Cn = cn
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        Dim SenderId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        StrSql = " SELECT COSTID "
        StrSql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
        StrSql += " WHERE COSTID <> '" & SenderId & "' AND MAIN = 'Y' AND isnull(ACTIVE,'Y') = 'Y' "
        objGPack.FillCombo(StrSql, cmbCostId)
        If cmbCostId.Items.Count > 0 Then
            cmbCostId.SelectedIndex = 0
        End If

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        Tablename.Add("ISSUE")
        Tablename.Add("ISSSTONE")
        Tablename.Add("RECEIPT")
        Tablename.Add("RECEIPTSTONE")
        Tablename.Add("OUTSTANDING")
        Tablename.Add("ACCTRAN")
        Tablename.Add("TAXTRAN")
        Tablename.Add("ITEMDETAIL")
        Tablename.Add("BRSINFO")
        Tablename.Add("PERSONALINFO")
        Tablename.Add("CUSTOMERINFO")
        Tablename.Add("ORMAST")
        Tablename.Add("ORIRDETAIL")
        Tablename.Add("ITEMTAG")

    End Sub
   

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

        If cmbCostId.Text = "" Then
            MsgBox("Cost Id should not empty", MsgBoxStyle.Information)
            cmbCostId.Focus()
            Exit Sub
        End If
        Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        Dim toCostId As String = cmbCostId.Text
        Try
            Dim Qry As String = ""
            For i As Integer = 0 To Tablename.Count - 1
                If Tablename.Item(i).ToString = "PERSONALINFO" Then
                    Try
                        _Tran = Nothing
                        _Tran = cn.BeginTransaction
                        If chkRemoveExisting.Checked = True Then
                            StrSql = " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                            StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & ".." & Tablename.Item(i).ToString & " WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'))"
                            StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                            StrSql += " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"


                            Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                            Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                            Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                            Cmd.ExecuteNonQuery()
                        End If
                        InsertQry(cnAdminDb, Tablename.Item(i).ToString, "TRANDATE", Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId, "ACCTRAN")
                        _Tran.Commit()
                        _Tran = Nothing
                    Catch ex As Exception
                        If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                    End Try
                ElseIf Tablename.Item(i).ToString = "ITEMTAG" Then
                    Try
                        _Tran = Nothing
                        _Tran = cn.BeginTransaction
                        UpdateIntoItemtag(Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId)
                        _Tran.Commit()
                        _Tran = Nothing
                    Catch ex As Exception
                        If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                    End Try
                Else
                    If Tablename.Item(i).ToString = "CUSTOMERINFO" Then
                        Try
                            _Tran = Nothing
                            _Tran = cn.BeginTransaction
                            If chkRemoveExisting.Checked = True Then
                                Qry = ""
                                StrSql = " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & ".." & Tablename.Item(i).ToString & " WHERE SUBSTRING(PSNO,1,2)='" & frmCostId & "' AND COSTID='" & frmCostId & "'"
                                StrSql += vbCrLf + " AND BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE  TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "' )"
                                StrSql += vbCrLf + " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                Cmd.ExecuteNonQuery()
                            End If
                            InsertQry(cnAdminDb, Tablename.Item(i).ToString, "BATCHNO", Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId, "ACCTRAN")
                            _Tran.Commit()
                            _Tran = Nothing
                        Catch ex As Exception
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        End Try
                    ElseIf Tablename.Item(i).ToString = "ORMAST" Then
                        Try
                            _Tran = Nothing
                            _Tran = cn.BeginTransaction
                            If chkRemoveExisting.Checked = True Then
                                Qry = ""
                                StrSql = " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & ".." & Tablename.Item(i).ToString & " WHERE ORDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                                StrSql += vbCrLf + " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                Cmd.ExecuteNonQuery()
                            End If
                            InsertQry(cnAdminDb, Tablename.Item(i).ToString, "ORDATE", Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId)
                            _Tran.Commit()
                            _Tran = Nothing
                        Catch ex As Exception
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        End Try
                    ElseIf Tablename.Item(i).ToString = "ORIRDETAIL" Or Tablename.Item(i).ToString = "OUTSTANDING" Then
                        Try
                            _Tran = Nothing
                            _Tran = cn.BeginTransaction
                            If chkRemoveExisting.Checked = True Then
                                Qry = ""
                                StrSql = " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & ".." & Tablename.Item(i).ToString & " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                                StrSql += vbCrLf + " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                Cmd.ExecuteNonQuery()
                            End If
                            InsertQry(cnAdminDb, Tablename.Item(i).ToString, "TRANDATE", Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId)
                            _Tran.Commit()
                            _Tran = Nothing
                        Catch ex As Exception
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        End Try
                    Else
                        Try
                            _Tran = Nothing
                            _Tran = cn.BeginTransaction
                            If chkRemoveExisting.Checked = True Then
                                If Tablename.Item(i).ToString = "ITEMDETAIL" Then
                                    Qry = ""
                                    StrSql = " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                    StrSql += vbCrLf + " DELETE FROM " & cnStockDb & ".." & Tablename.Item(i).ToString & " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                    StrSql += vbCrLf + " AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                                    StrSql += vbCrLf + " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                    Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                    Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                    Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                    Cmd.ExecuteNonQuery()
                                ElseIf Tablename.Item(i).ToString = "ACCTRAN" Then
                                    Qry = ""
                                    StrSql = " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                    StrSql += vbCrLf + " DELETE FROM " & cnStockDb & ".." & Tablename.Item(i).ToString & " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                    StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                                    StrSql += vbCrLf + " AND FROMFLAG<>'C' AND TRANNO<>9999 "
                                    StrSql += vbCrLf + " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                    Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                    Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                    Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                    Cmd.ExecuteNonQuery()
                                    Qry = ""
                                    'StrSql = vbCrLf + "DELETE FROM " & cnStockDb & ".." & Tablename.Item(i).ToString & " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                    'StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND FROMFLAG='C'"

                                    'Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                    'Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                    'Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                    'Cmd.ExecuteNonQuery()
                                Else
                                    Qry = ""
                                    StrSql = " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','DISABLE','DEL'"
                                    StrSql += vbCrLf + " DELETE FROM " & cnStockDb & ".." & Tablename.Item(i).ToString & " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
                                    StrSql += vbCrLf + " AND COSTID='" & frmCostId & "' AND SUBSTRING(SNO,1,2)='" & frmCostId & "'"
                                    StrSql += vbCrLf + " EXEC " & cnStockDb & "..DISABLEENABLE_AUDITTRG '" & Tablename.Item(i).ToString & "','ENABLE','DEL'"

                                    Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                                    Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                                    Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                                    Cmd.ExecuteNonQuery()
                                End If
                            End If
                            InsertQry(cnStockDb, Tablename.Item(i).ToString, "TRANDATE", Format(dtpFrom.Value, "yyyy-MM-dd"), Format(dtpTo.Value, "yyyy-MM-dd"), frmCostId, toCostId)
                            _Tran.Commit()
                            _Tran = Nothing
                        Catch ex As Exception
                            If _Tran.Connection IsNot Nothing Then _Tran.Rollback()
                        End Try
                    End If
                End If
            Next

            MsgBox("Resend Transaction Completed", MsgBoxStyle.Information)
            cmbCostId.SelectedIndex = 0
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
        Catch ex As Exception
            If _Tran IsNot Nothing Then _Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub InsertQry(ByVal dbname As String, ByVal TableName As String, ByVal CondColName As String, ByVal CondColValue1 As String, ByVal CondColValue2 As String, ByVal FromCostId As String, ByVal ToCostId As String, Optional ByVal TableName2 As String = Nothing)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        If TableName2 = Nothing Then
            StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
            StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " WHERE COSTID = '" & FromCostId & "' AND " & CondColName & " BETWEEN '" & CondColValue1 & "'  AND '" & CondColValue2 & "'"
            If TableName = "ACCTRAN" Then StrSql += vbCrLf + "  AND FROMFLAG<>'C' AND TRANNO<>9999 "
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()
        Else
            If TableName = "PERSONALINFO" Then
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE  " & CondColName & " BETWEEN '" & CondColValue1 & "'  AND '" & CondColValue2 & "' AND COSTID = '" & FromCostId & "'))"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                Cmd.ExecuteNonQuery()
            ElseIf TableName = "CUSTOMERINFO" Then
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " WHERE " & CondColName & " IN (SELECT " & CondColName & " FROM " & cnStockDb & ".." & TableName2 & " WHERE TRANDATE BETWEEN '" & CondColValue1 & "'  AND '" & CondColValue2 & "' AND COSTID = '" & FromCostId & "')"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                Cmd.ExecuteNonQuery()
            Else
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " WHERE " & CondColName & " IN (SELECT " & CondColName & " FROM " & dbname & ".." & TableName2 & " WHERE  COSTID = '" & FromCostId & "' AND TRANDATE BETWEEN '" & CondColValue1 & "'  AND '" & CondColValue2 & "')"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

        Dim mtempqrytb As String = "TEMPQRYTB"
        StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        StrSql += vbCrLf + " @DBNAME = '" & dbname & "',@TABLENAME = 'INS_" & TableName & "',@MASK_TABLENAME = '" & TableName & "',@TEMPTABLE='" & mtempqrytb & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub UpdateIntoItemtag(ByVal CondColValue1 As String, ByVal CondColValue2 As String, ByVal FromCostId As String, ByVal ToCostId As String)
        Dim Qry As String = ""
        Dim dtTemp As New DataTable
        Strsql = "SELECT TRANNO,TRANDATE,TRANTYPE,BATCHNO,COMPANYID,ITEMID,TAGNO,PCS,GRSWT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & CondColValue1 & "' AND '" & CondColValue2 & "'"
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        StrSql += vbCrLf + " AND COSTID='" & FromCostId & "'"
        StrSql += vbCrLf + " AND TRANTYPE IN('SA','MI')"
        StrSql += vbCrLf + " AND ISNULL(TAGNO,'')<>''"
        Cmd = New OleDbCommand(Strsql, _Cn, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtTemp)
        For Cnt As Integer = 0 To dtTemp.Rows.Count - 1
            Qry = " UPDATE " & cnAdminDb & "..ITEMTAG "
            Qry += " SET ISSDATE = '" & Format(dtTemp.Rows(Cnt).Item("TRANDATE"), "yyyy-MM-dd") & "'"
            Qry += " ,ISSREFNO = '" & Val(dtTemp.Rows(Cnt).Item("TRANNO").ToString) & "'"
            Qry += " ,ISSPCS = " & Val(dtTemp.Rows(Cnt).Item("PCS").ToString) & ""
            Qry += " ,ISSWT = " & Val(dtTemp.Rows(Cnt).Item("GRSWT").ToString) & ""
            Qry += " ,TOFLAG = '" & dtTemp.Rows(Cnt).Item("TRANTYPE").ToString & "'"
            Qry += " ,BATCHNO = '" & dtTemp.Rows(Cnt).Item("BATCHNO").ToString & "'"
            Qry += " ,COMPANYID = '" & dtTemp.Rows(Cnt).Item("COMPANYID").ToString & "'"
            Qry += " WHERE ITEMID = '" & Val(dtTemp.Rows(Cnt).Item("ITEMID").ToString) & "' "
            Qry += " AND TAGNO = '" & dtTemp.Rows(Cnt).Item("TAGNO").ToString & "'"
            Qry += " AND ISNULL(ISSDATE,'')=''"
            If Qry <> "" Then
                StrSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
                StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                Cmd.ExecuteNonQuery()
            End If
            Qry = ""
        Next
    End Sub
End Class