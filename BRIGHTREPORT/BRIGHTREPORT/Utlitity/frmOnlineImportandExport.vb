Imports System.Data.OleDb
Public Class frmOnlineImportandExport
#Region "VARIABLE"
    Public api_dbname As String
    Public api_servername As String
    Public api_username As String
    Public api_password As String
    Public api_onlinecn As OleDbConnection
    Dim strsql As String = ""
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
    Dim _onlinetran As OleDbTransaction = Nothing
    Dim da As OleDbDataAdapter
    Dim SAVINGSADMINDB As String = "" & strCompanyId & "SAVINGS"
    Dim SAVINGSTRANDB As String = "" & strCompanyId & "SH0708"
    Dim dtTotalCount As Integer = 0
    Dim count As Integer = 0
#End Region
#Region "FORM LOAD"
    Private Sub frmOnlineImportandExport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmOnlineImportandExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funnew()
        connectionString()
    End Sub
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtTotalCount = 0
        count = 0
        _tran = Nothing
        _onlinetran = Nothing
        'chkAll.Checked = True
    End Sub

    Private Function importSearch(ByVal groupcode As String, ByVal regno As String) As String
        Dim Qry1 As String = ""
        If groupcode = "" And regno = "" Then
            Return Qry1
        End If
        Qry1 = "AND GROUPCODE = '" & groupcode & "' AND REGNO = '" & regno & "'"
        Return Qry1
    End Function

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            Dim Searchgroupcode As String = ""
            Dim Searchregno As String = ""

            If valid() = False Then
                Exit Sub
            End If
            _tran = Nothing
            _onlinetran = Nothing
            _onlinetran = api_onlinecn.BeginTransaction
            _tran = cn.BeginTransaction
            count = 0
            dtTotalCount = 0
            If chkAll.Checked = True Or chkSchemeMast.Checked = True Then
                '/***PERSONALINFO***/
                count = 0
                dtTotalCount = 0
                strsql = vbCrLf + " SELECT ROWID PERSONALID ,PNAME"
                strsql += vbCrLf + " FROM " & api_dbname & "..SCHEMEMAST"
                strsql += vbCrLf + " WHERE ISNULL(TRANSFERED,'') = ''"
                strsql += vbCrLf + " " & importSearch(Searchgroupcode, Searchregno) & ""
                cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                cmd.ExecuteNonQuery()
                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "PERSONALINFO"
                    Dim Qry As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + InsertQry(ro, SAVINGSADMINDB)
                        dtTotalCount += 1
                        count += 1
                        If (count > 0 And ((count Mod 1000) = 0)) Or (dt.Rows.Count - dtTotalCount) = 0 Then
                            cmd = New OleDbCommand(Qry, cn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                            count = 0
                            Qry = ""
                        End If
                    Next
                End If
                '/***SCHEMEMAST***/
                count = 0
                dtTotalCount = 0
                strsql = vbCrLf + " SELECT COSTID,ROWID SNO,COMPANYID,SCHEMEID,GROUPCODE,REGNO,DOCLOSE"
                strsql += vbCrLf + " FROM " & api_dbname & "..SCHEMEMAST "
                strsql += vbCrLf + " WHERE ISNULL(TRANSFERED,'') = ''"
                strsql += vbCrLf + " " & importSearch(Searchgroupcode, Searchregno) & ""
                cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEMEMAST"
                    Dim Qry As String = ""
                    Dim groupcode As String = ""
                    Dim regno As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + InsertQry(ro, SAVINGSADMINDB)
                        groupcode += ",'" & ro.Item("GROUPCODE").ToString & "'"
                        regno += "," & ro.Item("REGNO").ToString
                        count += 1
                        dtTotalCount += 1
                        If (count > 0 And (count Mod 1000) = 0) Or (dt.Rows.Count - dtTotalCount) = 0 Then
                            cmd = New OleDbCommand(Qry, cn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()

                            strsql = " UPDATE " & api_dbname & "..SCHEMEMAST SET TRANSFERED = 'Y'"
                            strsql += " WHERE GROUPCODE IN (" & groupcode.Trim(",") & ")"
                            strsql += " AND REGNO IN (" & regno.Trim(",") & ")"
                            cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                            cmd.ExecuteNonQuery()

                            count = 0
                            groupcode = ""
                            regno = ""
                            Qry = ""
                        End If
                    Next
                End If
            End If

            count = 0
            dtTotalCount = 0
            If chkAll.Checked = True Or chkSchemeTran.Checked = True Then
                count = 0
                dtTotalCount = 0
                strsql = vbCrLf + " SELECT ROWID SNO"
                strsql += vbCrLf + " ,COSTID"
                strsql += vbCrLf + " ,GROUPCODE"
                strsql += vbCrLf + " ,REGNO"
                strsql += vbCrLf + " ,sum(AMOUNT)amount"
                strsql += vbCrLf + " ,sum(WEIGHT) weight"
                strsql += vbCrLf + " ,sum(SWEIGHT) Sweight"
                strsql += vbCrLf + " ,sum(GRSWT) grswt"
                strsql += vbCrLf + " ,RATE"
                strsql += vbCrLf + " ,SRATE"
                strsql += vbCrLf + " ,RECEIPTNO"
                strsql += vbCrLf + " ,RDATE"
                'strsql += vbCrLf + " ,ACCODE,MODEPAY"
                strsql += vbCrLf + " ,CANCEL"
                strsql += vbCrLf + " ,SYSTEMID"
                strsql += vbCrLf + " ,INSTALLMENT"
                strsql += vbCrLf + " ,REMARKS"
                strsql += vbCrLf + " ,ENTREFNO"
                strsql += vbCrLf + " ,CHEQUERETDATE"
                strsql += vbCrLf + " ,BOOKNO"
                strsql += vbCrLf + " FROM " & api_dbname & "..SCHEMETRAN"
                strsql += vbCrLf + " WHERE ISNULL(TRANSFERED,'') = ''"
                strsql += vbCrLf + " " & importSearch(Searchgroupcode, Searchregno) & ""
                strsql += vbCrLf + " group by ROWID,COSTID,GROUPCODE,REGNO"
                strsql += vbCrLf + " ,RATE,SRATE,RECEIPTNO,RDATE"
                ' strsql += vbCrLf + " ,ACCODE,MODEPAY "
                strsql += vbCrLf + " , CANCEL,SYSTEMID,INSTALLMENT,REMARKS,ENTREFNO,CHEQUERETDATE,BOOKNO"
                cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                cmd.ExecuteNonQuery()
                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEMETRAN"
                    Dim Qry As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + InsertQry(ro, SAVINGSTRANDB)
                        count += 1
                        dtTotalCount += 1
                        If (count > 0 And (count Mod 1000) = 0) Or (dtTotalCount - dt.Rows.Count) = 0 Then
                            cmd = New OleDbCommand(Qry, cn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                            count = 0
                            Qry = ""
                        End If
                    Next
                End If
                count = 0
                dtTotalCount = 0
                strsql = vbCrLf + " select "
                strsql += vbCrLf + " ROWID SNO"
                strsql += vbCrLf + " ,GroupCode"
                strsql += vbCrLf + " ,RegNo"
                strsql += vbCrLf + " ,ReceiptNo"
                strsql += vbCrLf + " ,Rdate"
                strsql += vbCrLf + " ,Amount"
                strsql += vbCrLf + " ,modepay"
                strsql += vbCrLf + " ,Accode"
                strsql += vbCrLf + " ,EntRefNo"
                strsql += vbCrLf + " ,Cancel"
                strsql += vbCrLf + " ,SystemId"
                strsql += vbCrLf + " ,ChequeRetDate"
                strsql += vbCrLf + " ,BookNo"
                strsql += vbCrLf + " ,COSTID"
                strsql += vbCrLf + " from " & api_dbname & "..SCHEMETRAN "
                strsql += vbCrLf + " WHERE ISNULL(TRANSFERED,'') = ''"
                strsql += vbCrLf + " " & importSearch(Searchgroupcode, Searchregno) & ""
                cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEMECOLLECT"
                    Dim Qry As String = ""
                    Dim groupcode As String = ""
                    Dim regno As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + InsertQry(ro, SAVINGSTRANDB)
                        groupcode += ",'" & ro.Item("GROUPCODE").ToString & "'"
                        regno += "," & ro.Item("REGNO").ToString
                        count += 1
                        dtTotalCount += 1
                        If (count > 0 And (count Mod 1000) = 0) Or (dt.Rows.Count - dtTotalCount) = 0 Then
                            cmd = New OleDbCommand(Qry, cn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                            strsql = " UPDATE " & api_dbname & "..SCHEMEMAST SET TRANSFERED = 'Y'"
                            strsql += " WHERE GROUPCODE IN (" & groupcode.Trim(",") & ")"
                            strsql += " AND REGNO IN (" & regno.Trim(",") & ")"
                            cmd = New OleDbCommand(strsql, api_onlinecn, _onlinetran)
                            cmd.ExecuteNonQuery()
                            count = 0
                            Qry = ""
                            groupcode = ""
                            regno = ""
                        End If
                    Next
                End If
            End If
            _tran.Commit()
            _onlinetran.Commit()
            _tran = Nothing
            _onlinetran = Nothing
            btnNew_Click(Me, New System.EventArgs)
            MsgBox("imported successfully", MsgBoxStyle.Information)
        Catch ex As Exception
            If (Not _tran Is Nothing) Or (Not _onlinetran Is Nothing) Then
                _tran.Rollback()
                _onlinetran.Rollback()
                _tran = Nothing
                _onlinetran = Nothing
                MessageBox.Show(ex.ToString)
                Exit Sub
            Else
                MessageBox.Show(ex.ToString)
                Exit Sub
            End If
        End Try
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            If valid() = False Then
                Exit Sub
            End If
            _tran = Nothing
            _tran = api_onlinecn.BeginTransaction
            dtTotalCount = 0
            count = 0
            If chkAll.Checked = True Or chkCompany.Checked = True Then
                Dim strcompany As String = GetColumnNames_N(SAVINGSADMINDB, "COMPANY", Nothing, cn)
                strsql = "SELECT " & strcompany & " FROM " & SAVINGSADMINDB & "..COMPANY "
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "COMPANY"
                    For Each ro As DataRow In dt.Rows
                        If funcCheckDBSNO_Server2(ro.Item("COMPANYID"), "COMPANY", "COMPANYID", api_onlinecn, _tran, "", "", api_dbname) = False Then
                            cmd = New OleDbCommand(InsertQry(ro, api_dbname), api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If
            End If
            dtTotalCount = 0
            count = 0
            If chkAll.Checked = True Or chkScheme.Checked = True Then
                Dim strInsAmount As String = GetColumnNames_N(SAVINGSADMINDB, "SCHEME", Nothing, cn)
                strsql = "SELECT " & strInsAmount & " FROM "
                strsql += vbCrLf + " " & SAVINGSADMINDB & "..SCHEME "
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEME"
                    For Each ro As DataRow In dt.Rows
                        If funcCheckDBSNO_Server2(ro.Item("SCHEMEID"), "SCHEME", "SCHEMEID", api_onlinecn, _tran, "", "", api_dbname) = False Then
                            cmd = New OleDbCommand(InsertQry(ro, api_dbname), api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If
            End If
            dtTotalCount = 0
            count = 0
            If chkAll.Checked = True Or chkInsamount.Checked = True Then
                Dim strInsAmount As String = GetColumnNames_N(SAVINGSADMINDB, "INSAMOUNT", Nothing, cn)
                strsql = "SELECT " & strInsAmount & " FROM " & SAVINGSADMINDB & "..INSAMOUNT "
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "INSAMOUNT"
                    For Each ro As DataRow In dt.Rows
                        If funcCheckDBSNO_Server2(ro.Item("GROUPCODE").ToString, "INSAMOUNT", "GROUPCODE", api_onlinecn, _tran, "BONUS", ro.Item("BONUS").ToString, api_dbname) = False Then
                            cmd = New OleDbCommand(InsertQry(ro, api_dbname), api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If
            End If
            dtTotalCount = 0
            count = 0
            If chkAll.Checked = True Or chkRate.Checked = True Then
                Dim MaxDateRate As String = ""
                strsql = " SELECT REPLACE(CONVERT(VARCHAR(15),MAX(RDATE),102),'.','-') RDATE FROM " & cnAdminDb & "..RATEMAST "
                MaxDateRate = GetSqlValue(cn, strsql).ToString
                strsql = vbCrLf + " SELECT METALID,RDATE,RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST where RDATE in (select MAX(RDATE) from " & cnAdminDb & "..RATEMAST) "
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "RATEMAST"
                    strsql = "delete " & api_dbname & "..ratemast where rdate = '" & MaxDateRate & "'"
                    cmd = New OleDbCommand(strsql, api_onlinecn, _tran)
                    cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    For Each ro As DataRow In dt.Rows
                        cmd = New OleDbCommand(InsertQry(ro, api_dbname), api_onlinecn, _tran)
                        cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If
            dtTotalCount = 0
            count = 0
            If chkAll.Checked = True Or chkSchemeMast.Checked = True Then
                'strsql = vbCrLf + " SELECT S.COSTID,SNO,COMPANYID,SCHEMEID,GROUPCODE,REGNO,JOINDATE,REMARK,TOTALINS,S.APPVER"
                'strsql += vbCrLf + " ,TITLE,INITIAL,PNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES"
                'strsql += vbCrLf + " ,MOBILE,EMAIL,S.UPDATETIME,S.USERID,PHONERES2,MOBILE2,NEEDSMS1,NEEDSMS2,DOB,ANNIVERSARY_DATE "
                'strsql += vbCrLf + " FROM " & SAVINGSADMINDB & "..SCHEMEMAST AS S"
                'strsql += vbCrLf + " ," & SAVINGSADMINDB & "..PERSONALINFO  AS P"
                'strsql += vbCrLf + " WHERE S.SNO = P.PERSONALID"
                strsql = vbCrLf + " SELECT S.COSTID"
                strsql += vbCrLf + " ,SNO ROWID"
                strsql += vbCrLf + " ,COMPANYID,SCHEMEID"
                strsql += vbCrLf + " ,GROUPCODE"
                strsql += vbCrLf + " ,REGNO"
                strsql += vbCrLf + " ,S.APPVER"
                strsql += vbCrLf + " ,'' UMOBILENO"
                strsql += vbCrLf + " ,'' ACTIVE"
                strsql += vbCrLf + " ,PNAME"
                strsql += vbCrLf + " ,MOBILE AS MOBILENO"
                strsql += vbCrLf + " ,MOBILE2 AS ALTERNATENO"
                strsql += vbCrLf + " ,'" & Format(Today.Now, "yyyy-MM-dd") & "' UPDATED"
                strsql += vbCrLf + " ,'" & Now.ToLongTimeString & "' UPTIME "
                strsql += vbCrLf + " ,'' TEMPOTP"
                strsql += vbCrLf + " FROM " & SAVINGSADMINDB & "..SCHEMEMAST AS S"
                strsql += vbCrLf + " ," & SAVINGSADMINDB & "..PERSONALINFO  AS P"
                strsql += vbCrLf + " WHERE S.SNO = P.PERSONALID"
                ' strsql += vbCrLf + " and GROUPCODE = 'GSP 1' AND REGNO = '311'"
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEMEMAST"
                    'strsql = "DELETE " & api_dbname & "..SCHEMEMAST "
                    'cmd = New OleDbCommand(strsql, api_onlinecn, _tran)
                    'cmd.CommandTimeout = 1000
                    'cmd.ExecuteNonQuery() don't use this Query
                    Dim Qry As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + " IF NOT EXISTS (SELECT * FROM " & api_dbname & "..SCHEMEMAST WHERE GROUPCODE = '" & ro.Item("GROUPCODE").ToString & "' AND REGNO = " & ro.Item("REGNO").ToString & ")"
                        Qry += vbCrLf + " BEGIN "
                        Qry += vbCrLf + InsertQry(ro, api_dbname)
                        Qry += vbCrLf + " END "
                        count += 1
                        dtTotalCount += 1
                        If (count > 0 And (count Mod 1000) = 0) Or (dt.Rows.Count - dtTotalCount) = 0 Then
                            cmd = New OleDbCommand(Qry, api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                            count = 0
                            Qry = ""
                        End If
                    Next
                End If
            End If
            dtTotalCount = 0
            count = 0
          
            If chkAll.Checked = True Or chkSchemeTran.Checked = True Then
                strsql = "IF OBJECT_ID('TEMPTABLEDB..TEMPSCHEMETRAN','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSCHEMETRAN"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
                strsql = "CREATE TABLE TEMPTABLEDB..TEMPSCHEMETRAN(GROUPCODE VARCHAR(12), REGNO INT)"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
                Dim dt1 As DataTable
                strsql = "SELECT DISTINCT GROUPCODE,REGNO FROM " & api_dbname & "..SCHEMEMAST WHERE ISNULL(ACTIVE,'') = 'N' "
                cmd = New OleDbCommand(strsql, api_onlinecn, _tran)
                cmd.ExecuteNonQuery()
                dt1 = New DataTable
                da = New OleDbDataAdapter(cmd)
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    strsql = ""
                    For i As Integer = 0 To dt1.Rows.Count - 1
                        strsql += "INSERT INTO TEMPTABLEDB..TEMPSCHEMETRAN(GROUPCODE,REGNO) VALUES "
                        strsql += vbCrLf + " ("
                        strsql += vbCrLf + " '" & dt1.Rows(i).Item("GROUPCODE").ToString & "'"
                        strsql += vbCrLf + " ," & dt1.Rows(i).Item("REGNO").ToString & ""
                        strsql += vbCrLf + " )"
                        If ((i Mod 1000) = 0 And i > 0) Or (i = dt1.Rows.Count - 1) Then
                            cmd = New OleDbCommand(strsql, cn)
                            cmd.ExecuteNonQuery()
                            strsql = ""
                        End If
                    Next
                End If

                strsql = vbCrLf + " SELECT C.SNO ROWID,T.COSTID,T.GROUPCODE,T.REGNO"
                strsql += vbCrLf + " ,C.AMOUNT"
                strsql += vbCrLf + " ,CASE WHEN T.RATE <> 0 THEN  round(C.AMOUNT/T.RATE,3) ELSE T.WEIGHT END [WEIGHT] "
                strsql += vbCrLf + " ,SWEIGHT"
                strsql += vbCrLf + " ,CASE WHEN T.RATE <> 0 THEN  round(C.AMOUNT/T.RATE,3) ELSE T.WEIGHT END GRSWT"
                strsql += vbCrLf + " ,RATE"
                strsql += vbCrLf + " ,SRATE"
                strsql += vbCrLf + " ,T.RECEIPTNO,T.RDATE,ACCODE "
                strsql += vbCrLf + " ,C.MODEPAY,T.CANCEL"
                strsql += vbCrLf + " ,T.SYSTEMID,INSTALLMENT,REMARKS,T.ENTREFNO,T.CHEQUERETDATE"
                strsql += vbCrLf + " ,T.BOOKNO, '' AS TRANSFERED"
                strsql += vbCrLf + " FROM " & SAVINGSTRANDB & "..SCHEMETRAN AS T"
                strsql += vbCrLf + " ," & SAVINGSTRANDB & "..SCHEMECOLLECT AS C"
                strsql += vbCrLf + " ,TEMPTABLEDB..TEMPSCHEMETRAN AS TS"
                strsql += vbCrLf + " WHERE "
                strsql += vbCrLf + " T.EntRefNo = C.EntRefNo "
                strsql += vbCrLf + " AND T.GROUPCODE = C.GROUPCODE"
                strsql += vbCrLf + " AND T.REGNO = C.REGNO"
                strsql += vbCrLf + " AND TS.GROUPCODE = T.GROUPCODE"
                strsql += vbCrLf + " AND TS.REGNO = T.REGNO"
                'strsql += vbCrLf + " AND T.GROUPCODE = 'GSP 1' AND T.REGNO = 311"
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    dt.TableName = "SCHEMETRAN"

                    'strsql = "DELETE " & api_dbname & "..SCHEMETRAN"
                    'cmd = New OleDbCommand(strsql, api_onlinecn, _tran)
                    'cmd.ExecuteNonQuery()

                    Dim Qry As String = ""
                    Dim Groupcode As String = ""
                    Dim Regno As String = ""
                    For Each ro As DataRow In dt.Rows
                        Qry += vbCrLf + " IF NOT EXISTS (SELECT * FROM " & api_dbname & "..SCHEMETRAN WHERE GROUPCODE = '" & ro.Item("GROUPCODE").ToString & "' AND REGNO = " & ro.Item("REGNO").ToString & " AND ROWID = '" & ro.Item("ROWID").ToString & "')"
                        Qry += vbCrLf + " BEGIN "
                        Qry += vbCrLf + InsertQry(ro, api_dbname)
                        Qry += vbCrLf + " END "
                        Groupcode += ",'" & ro.Item("GROUPCODE").ToString & "'"
                        Regno += "," & ro.Item("REGNO").ToString & ""
                        count += 1
                        dtTotalCount += 1
                        If (count > 0 And (count Mod 1000) = 0) Or (dt.Rows.Count - dtTotalCount) = 0 Then
                            cmd = New OleDbCommand(Qry, api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()

                            Qry = ""
                            Qry = " UPDATE " & api_dbname & "..SCHEMEMAST SET ACTIVE = 'Y'"
                            Qry += vbCrLf + "  WHERE "
                            Qry += vbCrLf + " GROUPCODE IN (" & Groupcode.Trim(",") & " )"
                            Qry += vbCrLf + " AND REGNO IN (" & Regno.Trim(",") & ") "
                            cmd = New OleDbCommand(Qry, api_onlinecn, _tran)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()

                            count = 0
                            Qry = ""
                            Groupcode = ""
                            Regno = ""
                        End If
                    Next
                End If
            End If
            _tran.Commit()
            _tran = Nothing
            btnNew_Click(Me, New System.EventArgs)
            MsgBox("Exported successfully", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not _tran Is Nothing Then
                _tran.Rollback()
                _tran = Nothing
                MessageBox.Show(ex.ToString)
                Exit Sub
            Else
                MessageBox.Show(ex.ToString)
                Exit Sub
            End If
        End Try

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
#End Region
#Region "USER DEFINE FUNCTION "

    Public Function InsertQry(ByVal Row As DataRow, Optional ByVal DB As String = Nothing) As String
        Dim qry As String = ""
        qry = "INSERT INTO " & IIf(Not DB Is Nothing, DB & "..", "") & Row.Table.TableName & " ( "
        Dim _Column As String = ""
        Dim _Values As String = ""
        For Each dCol As DataColumn In Row.Table.Columns
            _Column = _Column & "," + dCol.ColumnName
            If dCol.DataType.Name = "String" Then
                _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
            ElseIf dCol.DataType.Name = "DateTime" Then
                If IsDBNull(Row.Item(dCol.ColumnName)) Then
                    _Values = _Values & "," + "NULL" + ""
                Else
                    _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                End If
            ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name _
            Or dCol.DataType.Name = GetType(Int16).Name Or dCol.DataType.Name = GetType(Int32).Name _
            Or dCol.DataType.Name = GetType(Int64).Name Or dCol.DataType.Name = GetType(Integer).Name Then

                _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
            Else
                _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
            End If
        Next
        _Column = _Column & ")"
        _Column = _Column.Substring(1)
        _Values = _Values.Substring(1)
        qry = (qry + _Column & " VALUES (") + _Values & ")"
        Return qry
    End Function

    Public Function GetColumnNames_N(ByVal dbName As String, ByVal tblName As String _
    , ByVal ttran As OleDbTransaction, ByVal connectionstring As OleDbConnection) As String
        Dim retStr As String = Nothing
        strsql = "SELECT COLUMN_NAME AS NAME FROM " & dbName & ".INFORMATION_SCHEMA.COLUMNS"
        strsql += " WHERE TABLE_NAME = '" & tblName & "' "
        Dim dtTEmp As New DataTable
        cmd = New OleDbCommand(strsql, connectionstring) : cmd.CommandTimeout = 1000
        If Not ttran Is Nothing Then cmd.Transaction = ttran
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTEmp)
        For cnt As Integer = 0 To dtTEmp.Rows.Count - 1
            retStr += dtTEmp.Rows(cnt).Item("NAME").ToString
            If cnt <> dtTEmp.Rows.Count - 1 Then
                retStr += ","
            End If
        Next
        Return retStr
    End Function

    Private Function funcCheckDBSNO_Server2(ByVal RowName1 As String, ByVal TableName As String, _
    ByVal columnName As String _
    , ByVal con As OleDbConnection _
    , ByVal _tran As OleDbTransaction _
    , ByVal columnname2 As String, ByVal rowname2 As String _
    , ByVal databasename As String _
    ) As Boolean
        Dim dbsql As String = ""
        dbsql = " SELECT COUNT(*)CNT FROM " & databasename & ".." & TableName & " WHERE " & columnName & " = '" & RowName1 & "' "
        If columnname2 <> "" And rowname2 <> "" Then
            dbsql += " AND " & columnname2 & " = '" & rowname2 & "'"
        End If
        Dim dt As New DataTable
        cmd = New OleDbCommand(dbsql, con, _tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If Val(dt.Rows(0).Item("CNT").ToString) = 0 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function


    Private Sub funnew()
        Dim chitdbprefix As String = ""
        strsql = "select CTLTEXT from " & cnAdminDb & "..SOFTCONTROL where CTLID = 'CHITDBPREFIX'"
        chitdbprefix = GetSqlValue(cn, strsql)
        SAVINGSADMINDB = "" & chitdbprefix & "SAVINGS"
        SAVINGSTRANDB = "" & chitdbprefix & "SH0708"
        strsql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'API_DBNAME'"
        api_dbname = GetSqlValue(cn, strsql)
        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'API_SERVER'"
        api_servername = GetSqlValue(cn, strsql)
        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'API_USER'"
        api_username = GetSqlValue(cn, strsql)
        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'API_PASSWORD'"
        api_password = GetSqlValue(cn, strsql)
        If api_dbname = "" Or api_servername = "" Or api_username = "" Or api_password = "" Then
            MsgBox("onlinedbname empty kindly contact admin", MsgBoxStyle.Information)
            Me.Close()
            Exit Sub
        End If
        If chitdbprefix = "" Then
            MsgBox("chit dbprefix empty", MsgBoxStyle.Information)
            ' Me.Close()
        End If
    End Sub

    Private Sub connectionString()
        Try
            api_onlinecn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & api_dbname & ";Data Source={0};User Id=" & api_username & ";password=" & api_password & ";", api_servername))
            api_onlinecn.Open()
        Catch ex As Exception
            MsgBox("Connection Problem" + vbCrLf + ex.Message)
        End Try
    End Sub
#End Region
#Region "CHECKED EVENTS"
    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        If chkAll.Checked = True Then
            chkScheme.Enabled = False
            chkInsamount.Enabled = False
            chkCompany.Enabled = False
            chkSchemeMast.Enabled = False
            chkSchemeTran.Enabled = False
            chkRate.Enabled = False

            chkScheme.Checked = False
            chkInsamount.Checked = False
            chkCompany.Checked = False
            chkSchemeMast.Checked = False
            chkSchemeTran.Checked = False
            chkRate.Checked = False
        Else
            chkScheme.Enabled = True
            chkInsamount.Enabled = True
            chkCompany.Enabled = True
            chkSchemeMast.Enabled = True
            chkSchemeTran.Enabled = True
            chkRate.Enabled = True
        End If
    End Sub
#End Region
#Region "USER DEFINE FUNCTION"
    Private Function valid() As Boolean
        If chkAll.Checked = False And
                chkCompany.Checked = False And
                chkScheme.Checked = False And
                chkInsamount.Checked = False And
                chkSchemeMast.Checked = False And
                chkSchemeTran.Checked = False And
                chkRate.Checked = False Then
            MsgBox("select any one option", MsgBoxStyle.Information)
            Return False
        Else
            Return True
        End If


    End Function
#End Region
End Class