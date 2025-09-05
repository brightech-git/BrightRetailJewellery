Imports System.Data.OleDb
Public Class frmAcctoOutstanding
    Dim Strsql As String
    Dim da As New OleDbDataAdapter
    Dim dt, dtAdv As New DataTable
    Dim cmd As New OleDbCommand
    Dim Accodes As String
    Dim GenAccode As String
    Dim fromFlag As String = ""
    Private Sub funcGenerate()
        Try
            Accodes = ""
            tran = Nothing
            'tran = cn.BeginTransaction
            Strsql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPACC_OUT1','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPACC_OUT1"
            cmd = New OleDbCommand(Strsql, cn, tran)
            cmd.ExecuteNonQuery()

            Strsql = "DECLARE @ACCODE VARCHAR(7)"
            Strsql += vbCrLf + "DECLARE @TRANMODE VARCHAR(1)"
            Strsql += vbCrLf + "DECLARE @TRANNO INT"
            Strsql += vbCrLf + "DECLARE @BATCHNO VARCHAR(15)"
            Strsql += vbCrLf + "DECLARE @TRANDATE SMALLDATETIME"
            Strsql += vbCrLf + "DECLARE @RUNNO VARCHAR(15)"
            Strsql += vbCrLf + "DECLARE @COMPANYID VARCHAR(3)"
            Strsql += vbCrLf + "DECLARE @COSTID VARCHAR(2)"
            Strsql += vbCrLf + "DECLARE @FROMFLAG VARCHAR(1)"
            Strsql += vbCrLf + "DECLARE @AMOUNT NUMERIC(20,2)"
            Strsql += vbCrLf + "DECLARE @REFDATE SMALLDATETIME"
            Strsql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPACC_OUT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPACC_OUT"
            Strsql += vbCrLf + "SELECT * INTO TEMPTABLEDB..TEMPACC_OUT FROM " & cnAdminDb & "..OUTSTANDING WHERE 1<>1"
            Strsql += vbCrLf + "ALTER TABLE TEMPTABLEDB..TEMPACC_OUT DROP COLUMN SNO"
            Strsql += vbCrLf + "DECLARE CUR CURSOR FOR"
            Strsql += vbCrLf + "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            Strsql += vbCrLf + ",REPLICATE('0',2-LEN(AC.COSTID))+AC.COSTID +AC.COMPANYID+'G'"
            Strsql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            Strsql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            Strsql += vbCrLf + ",SUM(AMOUNT)AMOUNT "
            Strsql += vbCrLf + ",DATEADD(DD,ISNULL(AH.CREDITDAYS,0),TRANDATE)REFDATE"
            Strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN  AC"
            Strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            Strsql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            Strsql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            'Strsql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            If GenAccode <> "" Then Strsql += vbCrLf + "AND AC.ACCODE='" & GenAccode & "'"
            If fromFlag <> "" Then Strsql += vbCrLf + "AND AC.FROMFLAG  IN('" & Replace(fromFlag, ",", "','") & "')"
            Strsql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='C'"
            Strsql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,AC.COSTID,AC.COMPANYID,FROMFLAG,CREDITDAYS"
            Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,AC.COSTID,AC.COMPANYID"
            Strsql += vbCrLf + "OPEN CUR"
            Strsql += vbCrLf + "FETCH NEXT FROM CUR INTO @ACCODE,@TRANMODE,"
            Strsql += vbCrLf + "@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            Strsql += vbCrLf + "WHILE @@FETCH_STATUS <>-1"
            Strsql += vbCrLf + "BEGIN"
            Strsql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT,DUEDATE)"
            Strsql += vbCrLf + "SELECT @ACCODE,'R','T',@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            Strsql += vbCrLf + "FETCH NEXT FROM CUR INTO @ACCODE,@TRANMODE,"
            Strsql += vbCrLf + "@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            Strsql += vbCrLf + "END"
            Strsql += vbCrLf + "CLOSE CUR"
            Strsql += vbCrLf + "DEALLOCATE CUR"
            cmd = New OleDbCommand(Strsql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            Strsql = "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            Strsql += vbCrLf + ",REPLICATE('0',2-LEN(AC.COSTID))+AC.COSTID +AC.COMPANYID+'G'"
            Strsql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            Strsql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            Strsql += vbCrLf + ",SUM(AMOUNT)AMOUNT,SUM(AMOUNT)PENDINGAMOUNT "
            Strsql += vbCrLf + "INTO TEMPTABLEDB..TEMPACC_OUT1 FROM " & cnStockDb & "..ACCTRAN  AC"
            Strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            Strsql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            Strsql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            'Strsql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            If GenAccode <> "" Then Strsql += vbCrLf + "AND AC.ACCODE='" & GenAccode & "'"
            If fromFlag <> "" Then Strsql += vbCrLf + "AND AC.FROMFLAG  IN('" & Replace(fromFlag, ",", "','") & "')"
            Strsql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='D'"
            Strsql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,AC.COSTID,AC.COMPANYID,FROMFLAG"
            'Strsql += vbCrLf + "UNION ALL"
            'Strsql += vbCrLf + "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            'Strsql += vbCrLf + ",REPLICATE('0',2-LEN(AC.COSTID))+AC.COSTID +AC.COMPANYID+'G'"
            'Strsql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            'Strsql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            'Strsql += vbCrLf + ",SUM(AMOUNT)AMOUNT,SUM(AMOUNT)PENDINGAMOUNT "
            'Strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN  AC"
            'Strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            'Strsql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            'Strsql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            'Strsql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            'If GenAccode <> "" Then Strsql += vbCrLf + "AND AC.ACCODE='" & GenAccode & "'"
            'Strsql += vbCrLf + "AND AC.FROMFLAG ='P'"
            'Strsql += vbCrLf + "AND AC.PAYMODE ='DU'"
            'Strsql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='D'"
            'Strsql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,AC.COSTID,AC.COMPANYID,FROMFLAG"
            Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,AC.COSTID,AC.COMPANYID"
            cmd = New OleDbCommand(Strsql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            Strsql = vbCrLf + "SELECT SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            Strsql += vbCrLf + ",RUNNO,ACCODE,COSTID,COMPANYID,TRANDATE "
            Strsql += vbCrLf + "FROM TEMPTABLEDB..TEMPACC_OUT"
            Strsql += vbCrLf + "GROUP BY RUNNO,ACCODE,COSTID,COMPANYID,TRANDATE"
            Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,RUNNO"
            dt = New DataTable
            cmd = New OleDbCommand(Strsql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For I As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(I)
                        Dim AMOUNT As Double = Val(.Item("AMOUNT").ToString)
                        Dim ACCODE As String = .Item("ACCODE").ToString
                        Dim RUNNO As String = .Item("RUNNO").ToString
                        Accodes = Accodes & "'" & ACCODE & "',"
                        Strsql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                        Strsql += vbCrLf + "WHERE ACCODE='" & ACCODE & "'"
                        Strsql += vbCrLf + "AND PENDINGAMOUNT>0"
                        Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                        dtAdv = New DataTable
                        cmd = New OleDbCommand(Strsql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtAdv)
                        If dtAdv.Rows.Count > 0 Then
                            For J As Integer = 0 To dtAdv.Rows.Count - 1
                                If AMOUNT = 0 Then Exit For
                                If AMOUNT < Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) Then
                                    Strsql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                                    Strsql += vbCrLf + " '" & ACCODE & "'"
                                    Strsql += vbCrLf + " ,'P'"
                                    Strsql += vbCrLf + " ,'T'"
                                    Strsql += vbCrLf + " ," & dtAdv.Rows(J).Item("TRANNO").ToString & ""
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("TRANDATE").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & RUNNO & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("FROMFLAG").ToString & "'"
                                    Strsql += vbCrLf + " ," & AMOUNT & ""
                                    Strsql += vbCrLf + ") "
                                    cmd = New OleDbCommand(Strsql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    Strsql = "UPDATE TEMPTABLEDB..TEMPACC_OUT1 SET PENDINGAMOUNT=PENDINGAMOUNT- " & AMOUNT & ""
                                    Strsql += vbCrLf + "WHERE BATCHNO='" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    Strsql += vbCrLf + "AND RUNNO='" & dtAdv.Rows(J).Item("RUNNO").ToString & "'"
                                    Strsql += vbCrLf + "AND COMPANYID='" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    Strsql += vbCrLf + "AND COSTID='" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    Strsql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
                                    cmd = New OleDbCommand(Strsql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    AMOUNT = 0
                                ElseIf AMOUNT >= Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) Then
                                    Strsql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                                    Strsql += vbCrLf + " '" & ACCODE & "'"
                                    Strsql += vbCrLf + " ,'P'"
                                    Strsql += vbCrLf + " ,'T'"
                                    Strsql += vbCrLf + " ," & dtAdv.Rows(J).Item("TRANNO").ToString & ""
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("TRANDATE").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & RUNNO & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    Strsql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("FROMFLAG").ToString & "'"
                                    Strsql += vbCrLf + " ," & Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) & ""
                                    Strsql += vbCrLf + ") "
                                    cmd = New OleDbCommand(Strsql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    Strsql = "UPDATE TEMPTABLEDB..TEMPACC_OUT1 SET PENDINGAMOUNT=PENDINGAMOUNT- " & Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) & ""
                                    Strsql += vbCrLf + "WHERE BATCHNO='" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    Strsql += vbCrLf + "AND RUNNO='" & dtAdv.Rows(J).Item("RUNNO").ToString & "'"
                                    Strsql += vbCrLf + "AND COMPANYID='" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    Strsql += vbCrLf + "AND COSTID='" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    Strsql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
                                    cmd = New OleDbCommand(Strsql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    AMOUNT -= Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString)
                                End If
                            Next
                        End If
                    End With
                Next
                Strsql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                Strsql += vbCrLf + "WHERE 1=1 " 'ACCODE NOT IN(" & Mid(Accodes, 1, Len(Accodes) - 1) & ")"
                Strsql += vbCrLf + "AND PENDINGAMOUNT>0"
                Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                dtAdv = New DataTable
                cmd = New OleDbCommand(Strsql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtAdv)
                If dtAdv.Rows.Count > 0 Then
                    For K As Integer = 0 To dtAdv.Rows.Count - 1
                        Strsql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                        Strsql += vbCrLf + " '" & dtAdv.Rows(K).Item("ACCODE").ToString & "'"
                        Strsql += vbCrLf + " ,'P'"
                        Strsql += vbCrLf + " ,'T'"
                        Strsql += vbCrLf + " ," & dtAdv.Rows(K).Item("TRANNO").ToString & ""
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("BATCHNO").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("TRANDATE").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("RUNNO").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COMPANYID").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COSTID").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("FROMFLAG").ToString & "'"
                        Strsql += vbCrLf + " ," & Val(dtAdv.Rows(K).Item("PENDINGAMOUNT").ToString) & ""
                        Strsql += vbCrLf + ") "
                        cmd = New OleDbCommand(Strsql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            Else
                Strsql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                Strsql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                dtAdv = New DataTable
                cmd = New OleDbCommand(Strsql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtAdv)
                If dtAdv.Rows.Count > 0 Then
                    For K As Integer = 0 To dtAdv.Rows.Count - 1
                        Strsql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                        Strsql += vbCrLf + " '" & dtAdv.Rows(K).Item("ACCODE").ToString & "'"
                        Strsql += vbCrLf + " ,'P'"
                        If dtAdv.Rows(K).Item("FROMFLAG").ToString.ToUpper = "A" Then
                            Strsql += vbCrLf + " ,'T'"
                        Else
                            Strsql += vbCrLf + " ,'D'"
                        End If
                        Strsql += vbCrLf + " ," & dtAdv.Rows(K).Item("TRANNO").ToString & ""
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("BATCHNO").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("TRANDATE").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("RUNNO").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COMPANYID").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COSTID").ToString & "'"
                        Strsql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("FROMFLAG").ToString & "'"
                        Strsql += vbCrLf + " ," & Val(dtAdv.Rows(K).Item("AMOUNT").ToString) & ""
                        Strsql += vbCrLf + ") "
                        cmd = New OleDbCommand(Strsql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If
            Strsql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE APPVER='AUTOGEN' AND FROMFLAG IN('" & Replace(fromFlag, ",", "','") & "')"
            If GenAccode <> "" Then Strsql += vbCrLf + " AND ACCODE='" & GenAccode & "'"
            cmd = New OleDbCommand(Strsql, cn, tran)
            cmd.ExecuteNonQuery()
            Strsql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE APPVER='AUTOGEN' AND FROMFLAG='P' AND PAYMODE='DU'"
            If GenAccode <> "" Then Strsql += vbCrLf + " AND ACCODE='" & GenAccode & "'"
            cmd = New OleDbCommand(Strsql, cn, tran)
            cmd.ExecuteNonQuery()
            If GenAccode <> "" Then
                Strsql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE ACCODE='" & GenAccode & "'"
                cmd = New OleDbCommand(Strsql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            Strsql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER  ORDER BY ENDDATE DESC"
            cmd = New OleDbCommand(Strsql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtAdv)
            If dtAdv.Rows.Count > 0 Then
                For M As Integer = 0 To dtAdv.Rows.Count - 1
                    Strsql = "SELECT 1 AS AVAIL FROM MASTER..SYSDATABASES WHERE NAME='" & dtAdv.Rows(M).Item("DBNAME").ToString & "'"
                    If objGPack.GetSqlValue(Strsql, "AVAIL", "0", tran) = "0" Then Continue For
                    If cnStockDb = dtAdv.Rows(M).Item("DBNAME").ToString Then Continue For
                    Strsql = "SELECT AC.ACCODE,BATCHNO,TRANDATE,FROMFLAG,TRANNO,AC.COSTID,AC.COMPANYID"
                    Strsql += vbCrLf + ",SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1 * AMOUNT END)AMOUNT"
                    Strsql += vbCrLf + ",REPLICATE('0',2-LEN(AC.COSTID))+AC.COSTID +AC.COMPANYID+'G'"
                    Strsql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
                    Strsql += vbCrLf + "FROM " & dtAdv.Rows(M).Item("DBNAME").ToString & "..ACCTRAN AC"
                    Strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AH.ACCODE=AC.ACCODE"
                    Strsql += vbCrLf + "WHERE 1=1"
                    If fromFlag <> "" Then Strsql += vbCrLf + "AND FROMFLAG IN('" & Replace(fromFlag, ",", "','") & "')"
                    If GenAccode <> "" Then Strsql += vbCrLf + "AND AC.ACCODE='" & GenAccode & "'"
                    Strsql += vbCrLf + "AND OUTSTANDING='Y'"
                    Strsql += vbCrLf + "GROUP BY AC.ACCODE,TRANDATE,BATCHNO,FROMFLAG,TRANNO,AC.COSTID,AC.COMPANYID"
                    Strsql += vbCrLf + "HAVING SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1 * AMOUNT END)<>0"
                    Strsql += vbCrLf + "ORDER BY AC.ACCODE,TRANDATE,TRANNO,BATCHNO"
                    cmd = New OleDbCommand(Strsql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        For N As Integer = 0 To dt.Rows.Count - 1
                            Strsql = "INSERT INTO " & cnAdminDb & "..OUTSTANDING(SNO,ACCODE,RECPAY,TRANTYPE,PAYMODE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT,APPVER)VALUES("
                            Strsql += vbCrLf + " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("ACCODE").ToString & "'"
                            If Val(dt.Rows(N).Item("AMOUNT").ToString) < 0 Then
                                Strsql += vbCrLf + " ,'R'"
                            Else
                                Strsql += vbCrLf + " ,'P'"
                            End If
                            Strsql += vbCrLf + " ,'D'"
                            Strsql += vbCrLf + " ,'DU'"
                            Strsql += vbCrLf + " ," & dt.Rows(N).Item("TRANNO").ToString & ""
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("BATCHNO").ToString & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("TRANDATE").ToString & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("RUNNO").ToString & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("COMPANYID").ToString & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("COSTID").ToString & "'"
                            Strsql += vbCrLf + " ,'" & dt.Rows(N).Item("FROMFLAG").ToString & "'"
                            Strsql += vbCrLf + " ," & Val(dt.Rows(N).Item("AMOUNT").ToString) & ""
                            Strsql += vbCrLf + " ,'AUTOGEN'"
                            Strsql += vbCrLf + ") "
                            cmd = New OleDbCommand(Strsql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                Next
            End If

            Strsql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT ORDER BY TRANDATE,TRANNO"
            dt = New DataTable
            cmd = New OleDbCommand(Strsql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'Strsql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE APPVER='AUTOGEN' AND FROMFLAG IN('" & fromFlag & "')"
                'cmd = New OleDbCommand(Strsql, cn, tran)
                'cmd.ExecuteNonQuery()
                For L As Integer = 0 To dt.Rows.Count - 1
                    Strsql = "INSERT INTO " & cnAdminDb & "..OUTSTANDING(SNO,ACCODE,RECPAY,TRANTYPE,PAYMODE,TRANNO,BATCHNO,TRANDATE,DUEDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT,APPVER)VALUES("
                    Strsql += vbCrLf + " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("ACCODE").ToString & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("RECPAY").ToString & "'"
                    Strsql += vbCrLf + " ,'D'"
                    Strsql += vbCrLf + " ,'DU'"
                    Strsql += vbCrLf + " ," & dt.Rows(L).Item("TRANNO").ToString & ""
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("BATCHNO").ToString & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("TRANDATE").ToString & "'"
                    If dt.Rows(L).Item("FROMFLAG").ToString.ToUpper <> "S" Then
                        Strsql += vbCrLf + " ,NULL"
                    Else
                        Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("DUEDATE").ToString & "'"
                    End If
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("RUNNO").ToString & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("COMPANYID").ToString & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("COSTID").ToString & "'"
                    Strsql += vbCrLf + " ,'" & dt.Rows(L).Item("FROMFLAG").ToString & "'"
                    Strsql += vbCrLf + " ," & Val(dt.Rows(L).Item("AMOUNT").ToString) & ""
                    Strsql += vbCrLf + " ,'AUTOGEN'"
                    Strsql += vbCrLf + ") "
                    cmd = New OleDbCommand(Strsql, cn, tran)
                    cmd.ExecuteNonQuery()
                Next
                MsgBox("Record Generated.", MsgBoxStyle.Information)
            Else
                MsgBox("No Record Found", MsgBoxStyle.Information)
            End If
            If Not tran Is Nothing Then
                tran.Commit()
                tran = Nothing
            End If
        Catch ex As Exception
            btnView_Search.Enabled = True
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub frmAcctoOutstanding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmAcctoOutstanding_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        chkAccEntry.Checked = True
        chkMaterialRec.Checked = True
        Strsql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP "
        Strsql += " ORDER BY ACGRPNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(DtAcname)
        CmbAcGroup.Items.Clear()
        BrighttechPack.FillCombo(CmbAcGroup, DtAcname, "ACGRPNAME", True)
        If CmbAcGroup.Items.Contains("SUNDRY CREDITORS") Then CmbAcGroup.Text = "SUNDRY CREDITORS"
        LoadAcName()
    End Sub
    Private Sub LoadAcName()
        Strsql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y')<>'N' "
        If CmbAcGroup.Text <> "" Then Strsql += " AND ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME='" & CmbAcGroup.Text & "')"
        Strsql += " ORDER BY ACNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(DtAcname)
        CmbAcname.Items.Clear()
        BrighttechPack.FillCombo(CmbAcname, DtAcname, "ACNAME", True)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkAccEntry.Checked = False And chkMaterialRec.Checked = False Then
            chkMaterialRec.Checked = True
        End If
        If chkAccEntry.Checked And chkMaterialRec.Checked Then
            fromFlag = "A,S"
        ElseIf chkAccEntry.Checked And chkMaterialRec.Checked = False Then
            fromFlag = "A"
        Else
            fromFlag = "S"
        End If
        If chkAccEntry.Enabled = False And chkMaterialRec.Enabled = False Then fromFlag = ""
        Strsql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text & "'"
        GenAccode = objGPack.GetSqlValue(Strsql, "ACCODE", "")
        If GenAccode = "" Then MsgBox("Select Account.", MsgBoxStyle.Information) : CmbAcname.Focus() : Exit Sub
        If MessageBox.Show("Take Backup before Proceed.?", "Close Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            btnView_Search.Enabled = False
            funcGenerate()
        End If
        btnView_Search.Enabled = True
        GenAccode = ""
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CmbAcGroup_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbAcGroup.SelectedValueChanged
        LoadAcName()
    End Sub
End Class