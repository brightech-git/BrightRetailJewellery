Imports System.Data.OleDb
Public Class frmduetran
#Region "VARIABLE DECLARATIONS"
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
#End Region
#Region "FORM EVENTS"
    Private Sub frmduetran_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmduetran_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        GiritechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)
        btnView.Focus()
        btnNew_Click(Me, New EventArgs)
        PnlRange.BackColor = SystemColors.InactiveCaption
    End Sub
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, viewToolStripMenuItem.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.View) Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        funcView()
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, newToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            GiriPosting.GExport.Post(Me.Name, lblTitle.Text, gridView, "", GiriPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, exitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            GiriPosting.GExport.Post(Me.Name, lblTitle.Text, gridView, "", GiriPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        PnlRange.Visible = False
        chkRange.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        PnlRange.Visible = False
        chkRange.Focus()
    End Sub
#End Region
#Region "USER DEFINED FUNCTIONS"
    Private Sub funcGenerate()
        Dim Accodes As String
        Dim fromFlag As String = "S,A"
        Dim dt, dtAdv As New DataTable
        Accodes = ""
        strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPACC_OUT1','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPACC_OUT1"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        If rbtFifo.Checked Then
            strSql = "DECLARE @ACCODE VARCHAR(7)"
            strSql += vbCrLf + "DECLARE @TRANMODE VARCHAR(1)"
            strSql += vbCrLf + "DECLARE @TRANNO INT"
            strSql += vbCrLf + "DECLARE @BATCHNO VARCHAR(15)"
            strSql += vbCrLf + "DECLARE @TRANDATE SMALLDATETIME"
            strSql += vbCrLf + "DECLARE @RUNNO VARCHAR(15)"
            strSql += vbCrLf + "DECLARE @COMPANYID VARCHAR(3)"
            strSql += vbCrLf + "DECLARE @COSTID VARCHAR(2)"
            strSql += vbCrLf + "DECLARE @FROMFLAG VARCHAR(1)"
            strSql += vbCrLf + "DECLARE @AMOUNT NUMERIC(20,2)"
            strSql += vbCrLf + "DECLARE @REFDATE SMALLDATETIME"
            strSql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPACC_OUT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPACC_OUT"
            strSql += vbCrLf + "SELECT * INTO TEMPTABLEDB..TEMPACC_OUT FROM " & cnAdminDb & "..OUTSTANDING WHERE 1<>1"
            strSql += vbCrLf + "ALTER TABLE TEMPTABLEDB..TEMPACC_OUT DROP COLUMN SNO"
            strSql += vbCrLf + "DECLARE CUR CURSOR FOR"
            strSql += vbCrLf + "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            strSql += vbCrLf + ",REPLICATE('0',2-LEN(COSTID))+COSTID +AC.COMPANYID+'G'"
            strSql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            strSql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            strSql += vbCrLf + ",SUM(AMOUNT)AMOUNT "
            strSql += vbCrLf + ",DATEADD(DD,ISNULL(AH.CREDITDAYS,0),TRANDATE)REFDATE"
            strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN  AC"
            strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            strSql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            strSql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                strSql += vbCrLf + "AND AC.ACCODE=(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
            End If
            strSql += vbCrLf + "AND AC.FROMFLAG  IN('" & Replace(fromFlag, ",", "','") & "')"
            strSql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='C'"
            strSql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,COSTID,AC.COMPANYID,FROMFLAG,CREDITDAYS"
            strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,AC.COMPANYID"
            strSql += vbCrLf + "OPEN CUR"
            strSql += vbCrLf + "FETCH NEXT FROM CUR INTO @ACCODE,@TRANMODE,"
            strSql += vbCrLf + "@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            strSql += vbCrLf + "WHILE @@FETCH_STATUS <>-1"
            strSql += vbCrLf + "BEGIN"
            strSql += vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT,DUEDATE)"
            strSql += vbCrLf + "SELECT @ACCODE,'R','D',@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            strSql += vbCrLf + "FETCH NEXT FROM CUR INTO @ACCODE,@TRANMODE,"
            strSql += vbCrLf + "@TRANNO,@BATCHNO,@TRANDATE,@RUNNO,@COMPANYID,@COSTID,@FROMFLAG,@AMOUNT,@REFDATE"
            strSql += vbCrLf + "END"
            strSql += vbCrLf + "CLOSE CUR"
            strSql += vbCrLf + "DEALLOCATE CUR"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            strSql += vbCrLf + ",REPLICATE('0',2-LEN(COSTID))+COSTID +AC.COMPANYID+'G'"
            strSql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            strSql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            strSql += vbCrLf + ",SUM(AMOUNT)AMOUNT,SUM(AMOUNT)PENDINGAMOUNT "
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPACC_OUT1 FROM " & cnStockDb & "..ACCTRAN  AC"
            strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            strSql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            strSql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                strSql += vbCrLf + "AND AC.ACCODE=(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
            End If
            strSql += vbCrLf + "AND AC.FROMFLAG  IN('" & Replace(fromFlag, ",", "','") & "')"
            strSql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='D'"
            strSql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,COSTID,AC.COMPANYID,FROMFLAG"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE"
            strSql += vbCrLf + ",REPLICATE('0',2-LEN(COSTID))+COSTID +AC.COMPANYID+'G'"
            strSql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
            strSql += vbCrLf + ",AC.COMPANYID,AC.COSTID,FROMFLAG"
            strSql += vbCrLf + ",SUM(AMOUNT)AMOUNT,SUM(AMOUNT)PENDINGAMOUNT "
            strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN  AC"
            strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AC.ACCODE =AH.ACCODE "
            strSql += vbCrLf + "WHERE ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "AND ISNULL(AH.OUTSTANDING,'')='Y'"
            strSql += vbCrLf + "AND AH.ACTYPE NOT IN('B','O')"
            If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                strSql += vbCrLf + "AND AC.ACCODE=(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
            End If
            strSql += vbCrLf + "AND AC.FROMFLAG ='P'"
            strSql += vbCrLf + "AND AC.PAYMODE ='DU'"
            strSql += vbCrLf + "AND AC.ACCODE<>'CASH' AND TRANMODE='D'"
            strSql += vbCrLf + "GROUP BY AC.ACCODE,TRANMODE,TRANNO,BATCHNO,TRANDATE,COSTID,AC.COMPANYID,FROMFLAG"
            strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,AC.COMPANYID"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "SELECT SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            strSql += vbCrLf + ",RUNNO,ACCODE,COSTID,COMPANYID,TRANDATE "
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPACC_OUT"
            strSql += vbCrLf + "GROUP BY RUNNO,ACCODE,COSTID,COMPANYID,TRANDATE"
            strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,RUNNO"
            dt = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For I As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(I)
                        Dim AMOUNT As Double = Val(.Item("AMOUNT").ToString)
                        Dim ACCODE As String = .Item("ACCODE").ToString
                        Dim RUNNO As String = .Item("RUNNO").ToString
                        Accodes = Accodes & "'" & ACCODE & "',"
                        strSql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                        strSql += vbCrLf + "WHERE ACCODE='" & ACCODE & "'"
                        strSql += vbCrLf + "AND PENDINGAMOUNT>0"
                        strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                        dtAdv = New DataTable
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtAdv)
                        If dtAdv.Rows.Count > 0 Then
                            For J As Integer = 0 To dtAdv.Rows.Count - 1
                                If AMOUNT = 0 Then Exit For
                                If AMOUNT < Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) Then
                                    strSql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                                    strSql += vbCrLf + " '" & ACCODE & "'"
                                    strSql += vbCrLf + " ,'P'"
                                    strSql += vbCrLf + " ,'D'"
                                    strSql += vbCrLf + " ," & dtAdv.Rows(J).Item("TRANNO").ToString & ""
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("TRANDATE").ToString & "'"
                                    strSql += vbCrLf + " ,'" & RUNNO & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("FROMFLAG").ToString & "'"
                                    strSql += vbCrLf + " ," & AMOUNT & ""
                                    strSql += vbCrLf + ") "
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    strSql = "UPDATE TEMPTABLEDB..TEMPACC_OUT1 SET PENDINGAMOUNT=PENDINGAMOUNT- " & AMOUNT & ""
                                    strSql += vbCrLf + "WHERE BATCHNO='" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    strSql += vbCrLf + "AND RUNNO='" & dtAdv.Rows(J).Item("RUNNO").ToString & "'"
                                    strSql += vbCrLf + "AND COMPANYID='" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    strSql += vbCrLf + "AND COSTID='" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    strSql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    AMOUNT = 0
                                ElseIf AMOUNT >= Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) Then
                                    strSql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                                    strSql += vbCrLf + " '" & ACCODE & "'"
                                    strSql += vbCrLf + " ,'P'"
                                    strSql += vbCrLf + " ,'D'"
                                    strSql += vbCrLf + " ," & dtAdv.Rows(J).Item("TRANNO").ToString & ""
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("TRANDATE").ToString & "'"
                                    strSql += vbCrLf + " ,'" & RUNNO & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    strSql += vbCrLf + " ,'" & dtAdv.Rows(J).Item("FROMFLAG").ToString & "'"
                                    strSql += vbCrLf + " ," & Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) & ""
                                    strSql += vbCrLf + ") "
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    strSql = "UPDATE TEMPTABLEDB..TEMPACC_OUT1 SET PENDINGAMOUNT=PENDINGAMOUNT- " & Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString) & ""
                                    strSql += vbCrLf + "WHERE BATCHNO='" & dtAdv.Rows(J).Item("BATCHNO").ToString & "'"
                                    strSql += vbCrLf + "AND RUNNO='" & dtAdv.Rows(J).Item("RUNNO").ToString & "'"
                                    strSql += vbCrLf + "AND COMPANYID='" & dtAdv.Rows(J).Item("COMPANYID").ToString & "'"
                                    strSql += vbCrLf + "AND COSTID='" & dtAdv.Rows(J).Item("COSTID").ToString & "'"
                                    strSql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                    AMOUNT -= Val(dtAdv.Rows(J).Item("PENDINGAMOUNT").ToString)
                                End If
                            Next
                        End If
                    End With
                Next
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                strSql += vbCrLf + "WHERE 1=1" 'ACCODE NOT IN(" & Mid(Accodes, 1, Len(Accodes) - 1) & ")"
                strSql += vbCrLf + "AND PENDINGAMOUNT>0"
                strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                dtAdv = New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtAdv)
                If dtAdv.Rows.Count > 0 Then
                    For K As Integer = 0 To dtAdv.Rows.Count - 1
                        strSql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                        strSql += vbCrLf + " '" & dtAdv.Rows(K).Item("ACCODE").ToString & "'"
                        strSql += vbCrLf + " ,'P'"
                        strSql += vbCrLf + " ,'D'"
                        strSql += vbCrLf + " ," & dtAdv.Rows(K).Item("TRANNO").ToString & ""
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("BATCHNO").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("TRANDATE").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("RUNNO").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COMPANYID").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COSTID").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("FROMFLAG").ToString & "'"
                        strSql += vbCrLf + " ," & Val(dtAdv.Rows(K).Item("PENDINGAMOUNT").ToString) & ""
                        strSql += vbCrLf + ") "
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT1 "
                strSql += vbCrLf + "ORDER BY ACCODE,TRANDATE,TRANMODE,BATCHNO,TRANNO ,COSTID,COMPANYID "
                dtAdv = New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtAdv)
                If dtAdv.Rows.Count > 0 Then
                    For K As Integer = 0 To dtAdv.Rows.Count - 1
                        strSql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT(ACCODE,RECPAY,TRANTYPE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                        strSql += vbCrLf + " '" & dtAdv.Rows(K).Item("ACCODE").ToString & "'"
                        strSql += vbCrLf + " ,'P'"
                        strSql += vbCrLf + " ,'D'"
                        strSql += vbCrLf + " ," & dtAdv.Rows(K).Item("TRANNO").ToString & ""
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("BATCHNO").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("TRANDATE").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("RUNNO").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COMPANYID").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("COSTID").ToString & "'"
                        strSql += vbCrLf + " ,'" & dtAdv.Rows(K).Item("FROMFLAG").ToString & "'"
                        strSql += vbCrLf + " ," & Val(dtAdv.Rows(K).Item("AMOUNT").ToString) & ""
                        strSql += vbCrLf + ") "
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If
            strSql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER  ORDER BY ENDDATE DESC"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtAdv)
            If dtAdv.Rows.Count > 0 Then
                For M As Integer = 0 To dtAdv.Rows.Count - 1
                    strSql = "SELECT 1 AS AVAIL FROM MASTER..SYSDATABASES WHERE NAME='" & dtAdv.Rows(M).Item("DBNAME").ToString & "'"
                    If objGPack.GetSqlValue(strSql, "AVAIL", "0", tran) = "0" Then Continue For
                    If cnStockDb = dtAdv.Rows(M).Item("DBNAME").ToString Then Continue For
                    strSql = "SELECT AC.ACCODE,BATCHNO,TRANDATE,FROMFLAG,TRANNO,COSTID,AC.COMPANYID"
                    strSql += vbCrLf + ",SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1 * AMOUNT END)AMOUNT"
                    strSql += vbCrLf + ",REPLICATE('0',2-LEN(COSTID))+COSTID +AC.COMPANYID+'G'"
                    strSql += vbCrLf + "+'AA'+ CAST(TRANNO AS VARCHAR)AS RUNNO"
                    strSql += vbCrLf + "FROM " & dtAdv.Rows(M).Item("DBNAME").ToString & "..ACCTRAN AC"
                    strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ACHEAD AH ON AH.ACCODE=AC.ACCODE"
                    strSql += vbCrLf + "WHERE FROMFLAG IN('" & Replace(fromFlag, ",", "','") & "')"
                    strSql += vbCrLf + "AND OUTSTANDING='Y'"
                    strSql += vbCrLf + "GROUP BY AC.ACCODE,TRANDATE,BATCHNO,FROMFLAG,TRANNO,COSTID,AC.COMPANYID"
                    strSql += vbCrLf + "HAVING SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1 * AMOUNT END)<>0"
                    strSql += vbCrLf + "ORDER BY AC.ACCODE,TRANDATE,TRANNO,BATCHNO"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        For N As Integer = 0 To dt.Rows.Count - 1
                            strSql = "INSERT INTO TEMPTABLEDB..TEMPACC_OUT (ACCODE,RECPAY,TRANTYPE,PAYMODE,TRANNO,BATCHNO,TRANDATE,RUNNO,COMPANYID,COSTID,FROMFLAG,AMOUNT)VALUES("
                            strSql += vbCrLf + " '" & dt.Rows(N).Item("ACCODE").ToString & "'"
                            If Val(dt.Rows(N).Item("AMOUNT").ToString) < 0 Then
                                strSql += vbCrLf + " ,'R'"
                            Else
                                strSql += vbCrLf + " ,'P'"
                            End If
                            strSql += vbCrLf + " ,'D'"
                            strSql += vbCrLf + " ,'DU'"
                            strSql += vbCrLf + " ," & dt.Rows(N).Item("TRANNO").ToString & ""
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("BATCHNO").ToString & "'"
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("TRANDATE").ToString & "'"
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("RUNNO").ToString & "'"
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("COMPANYID").ToString & "'"
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("COSTID").ToString & "'"
                            strSql += vbCrLf + " ,'" & dt.Rows(N).Item("FROMFLAG").ToString & "'"
                            strSql += vbCrLf + " ," & Val(dt.Rows(N).Item("AMOUNT").ToString) & ""
                            strSql += vbCrLf + ") "
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                Next
            End If

            strSql = "SELECT * FROM TEMPTABLEDB..TEMPACC_OUT "
            dt = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
        Else
            strSql = "SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE FROMFLAG IN('P','S','A')"
            If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                strSql += vbCrLf + "AND ACCODE IN(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
            End If
            dt = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
        End If

        If dt.Rows.Count > 0 Then
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPRECEIPTAMT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRECEIPTAMT"
            strSql += vbCrLf + "  SELECT "
            If rdbTranDateWise.Checked Then
                strSql += vbCrLf + "  O.TRANDATE"
            Else
                strSql += vbCrLf + "  O.DUEDATE AS TRANDATE"
            End If
            strSql += vbCrLf + "  ,RUNNO"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(400),H.ACNAME)PARTICULAR"
            strSql += vbCrLf + "  ,SUM(AMOUNT)AMOUNT"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),H.ACNAME) AS PNAME"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)ISSAMT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPRECEIPTAMT"
            If rbtFifo.Checked Then
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPACC_OUT AS O"
            Else
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS O"
            End If
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = O.ACCODE "
            strSql += vbCrLf + "  WHERE O.TRANDATE <= '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND O.TRANTYPE = 'D' AND O.RECPAY = 'P' AND ISNULL(O.CANCEL,'') = '' AND RUNNO <> ''"
            If rbtNormal.Checked Then
                If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                    strSql += vbCrLf + "AND FROMFLAG IN('P','S','A')"
                    strSql += vbCrLf + "AND O.ACCODE IN(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
                End If
            End If
            strSql += vbCrLf + "  GROUP BY "
            If rdbTranDateWise.Checked Then
                strSql += vbCrLf + "  O.TRANDATE"
            Else
                strSql += vbCrLf + "  O.DUEDATE "
            End If
            strSql += vbCrLf + "  ,O.RUNNO,ACNAME"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPISSUEAMT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUEAMT"
            strSql += vbCrLf + "  SELECT "
            If rdbTranDateWise.Checked Then
                strSql += vbCrLf + "  O.TRANDATE"
            Else
                strSql += vbCrLf + "  O.DUEDATE AS TRANDATE"
            End If
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(400),H.ACNAME)PARTICULAR"
            strSql += vbCrLf + "  ,SUM(O.AMOUNT)AMOUNT,RUNNO"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)ISSAMT"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),H.ACNAME) AS PNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPISSUEAMT"
            If rbtFifo.Checked Then
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPACC_OUT AS O"
            Else
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS O"
            End If
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = O.ACCODE "
            strSql += vbCrLf + "  WHERE O.TRANDATE <= '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND O.TRANTYPE = 'D' AND O.RECPAY = 'R' AND ISNULL(O.CANCEL,'') = '' AND RUNNO <> ''"
            If rbtNormal.Checked Then
                If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
                    strSql += vbCrLf + "AND FROMFLAG IN('P','S','A')"
                    strSql += vbCrLf + "AND O.ACCODE IN(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
                End If
            End If
            strSql += vbCrLf + "  GROUP BY "
            If rdbTranDateWise.Checked Then
                strSql += vbCrLf + "  O.TRANDATE"
            Else
                strSql += vbCrLf + "  O.DUEDATE"
            End If
            strSql += vbCrLf + "  ,O.RUNNO,ACNAME"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  DECLARE @I_AMT NUMERIC(15,2) "
            strSql += vbCrLf + "  DECLARE @I_TRANDATE SMALLDATETIME "
            strSql += vbCrLf + "  DECLARE @I_PNAME VARCHAR(100)"
            strSql += vbCrLf + "  DECLARE @I_RUNNO VARCHAR(15)"
            strSql += vbCrLf + "  DECLARE @R_AMT NUMERIC(15,2) "
            strSql += vbCrLf + "  DECLARE @R_TRANDATE SMALLDATETIME "
            strSql += vbCrLf + "  DECLARE @TEMPAMT NUMERIC(15,2) "
            strSql += vbCrLf + "  DECLARE I_CURAMT CURSOR FOR SELECT TRANDATE,AMOUNT,PNAME,RUNNO FROM TEMPTABLEDB..TEMPISSUEAMT ORDER BY TRANDATE,RUNNO,PNAME "
            strSql += vbCrLf + "  OPEN I_CURAMT "
            strSql += vbCrLf + "  WHILE 1=1 "
            strSql += vbCrLf + "  BEGIN "
            strSql += vbCrLf + "  FETCH NEXT FROM I_CURAMT INTO @I_TRANDATE,@I_AMT,@I_PNAME,@I_RUNNO"
            strSql += vbCrLf + "  IF @@FETCH_STATUS = -1 BREAK "
            strSql += vbCrLf + "  DECLARE R_CURAMT CURSOR FOR SELECT AMOUNT - ISNULL(ISSAMT,0) AS AMOUNT FROM TEMPTABLEDB..TEMPRECEIPTAMT "
            strSql += vbCrLf + "                   WHERE AMOUNT - ISNULL(ISSAMT,0) > 0 AND PNAME = @I_PNAME AND RUNNO = @I_RUNNO"
            strSql += vbCrLf + "  OPEN R_CURAMT "
            strSql += vbCrLf + "  WHILE 1 = 1 "
            strSql += vbCrLf + "  	BEGIN "
            strSql += vbCrLf + "  	FETCH NEXT FROM R_CURAMT INTO @R_AMT "
            strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK "
            strSql += vbCrLf + "  	SELECT @TEMPAMT = 0 "
            strSql += vbCrLf + "  	IF @I_AMT <> 0 "
            strSql += vbCrLf + "  		BEGIN "
            strSql += vbCrLf + "  		IF @R_AMT > @I_AMT "
            strSql += vbCrLf + "  			BEGIN "
            strSql += vbCrLf + "  			SELECT @TEMPAMT = @I_AMT "
            strSql += vbCrLf + "  			SELECT @I_AMT = 0 "
            strSql += vbCrLf + "  			END "
            strSql += vbCrLf + "  		ELSE "
            strSql += vbCrLf + "  			BEGIN "
            strSql += vbCrLf + "  			SELECT @TEMPAMT = @R_AMT "
            strSql += vbCrLf + "  			SELECT @I_AMT = @I_AMT - @R_AMT "
            strSql += vbCrLf + "  			END "
            strSql += vbCrLf + "  		UPDATE TEMPTABLEDB..TEMPRECEIPTAMT SET ISSAMT = ISNULL(ISSAMT,0) + @TEMPAMT WHERE CURRENT OF R_CURAMT "
            strSql += vbCrLf + "  			IF @I_AMT = 0 BREAK "
            strSql += vbCrLf + "  		END "
            strSql += vbCrLf + "  	END "
            strSql += vbCrLf + "  IF @I_AMT <> 0 "
            strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPRECEIPTAMT(TRANDATE,AMOUNT,ISSAMT,PNAME,RUNNO)VALUES(@I_TRANDATE,0,@I_AMT,@I_PNAME,@I_RUNNO) "
            strSql += vbCrLf + "  CLOSE R_CURAMT "
            strSql += vbCrLf + "  DEALLOCATE R_CURAMT "
            strSql += vbCrLf + "  END "
            strSql += vbCrLf + "  CLOSE I_CURAMT "
            strSql += vbCrLf + "  DEALLOCATE I_CURAMT "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " FROMDAYS INT"
            strSql += vbCrLf + " ,TODAYS INT"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If Val(txtFromDay1.Text) <> 0 And Val(txtToDay1.Text) <> 0 Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay1.Text) & "," & Val(txtToDay1.Text) & ")"
            End If
            If Val(txtFromDay2.Text) <> 0 And Val(txtToDay2.Text) <> 0 Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay2.Text) & "," & Val(txtToDay2.Text) & ")"
            End If
            If Val(txtFromDay3.Text) <> 0 And Val(txtToDay3.Text) <> 0 Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay3.Text) & "," & Val(txtToDay3.Text) & ")"
            End If
            If Val(txtFromDay4.Text) <> 0 And Val(txtToDay4.Text) <> 0 Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay4.Text) & "," & Val(txtToDay4.Text) & ")"
            End If
            If Val(txtFromDay5.Text) <> 0 And Val(txtToDay5.Text) <> 0 Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay5.Text) & "," & Val(txtToDay5.Text) & ")"
            End If
            If Val(txtFromDay6.Text) <> 0 And Val(txtToDay6.Text) <> 0 Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay6.Text) & "," & Val(txtToDay6.Text) & ")"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_DUEAGING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_DUEAGING"
            strSql += vbCrLf + " SELECT DATEDIFF(DD,TRANDATE,'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "') AS DIFF"
            strSql += vbCrLf + " ,TRANDATE,PNAME AS PARTICULAR,AMOUNT,ISSAMT, ISNULL(ISSAMT,0)-AMOUNT BALAMT,RUNNO,PNAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_DUEAGING"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPRECEIPTAMT"
            strSql += vbCrLf + " WHERE  ISNULL(AMOUNT,0)-ISNULL(ISSAMT,0) <> 0"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = "UPDATE  TEMPTABLEDB..TEMP_DUEAGING SET DIFF=0 WHERE DIFF IS NULL"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPAGINGRESULTAMT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPAGINGRESULTAMT"
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMPAGINGRESULTAMT(PARTICULAR VARCHAR(400),TOTDAYS INT,BALAMT NUMERIC(15,2),RESULT INT,COLHEAD VARCHAR(3))"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPAGINGRESULTAMT(PARTICULAR,TOTDAYS,BALAMT,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT PARTICULAR,SUM(DIFF) AS TOTDAYS,SUM(BALAMT)AS BALAMT,1 RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_DUEAGING GROUP BY PARTICULAR"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL',SUM(DIFF) AS TOTDAYS,SUM(BALAMT)AS BALAMT,3 RESULT,'G' COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_DUEAGING"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " DECLARE @STR VARCHAR(8000)"
            strSql += vbCrLf + " DECLARE @FROMDAYS VARCHAR(10)"
            strSql += vbCrLf + " DECLARE @TODAYS VARCHAR(10)"
            strSql += vbCrLf + " DECLARE @MAXDAYS VARCHAR(10)"
            strSql += vbCrLf + " DECLARE @MINDAYS VARCHAR(10)"
            strSql += vbCrLf + " SELECT @MAXDAYS = MAX(TODAYS) FROM TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            strSql += vbCrLf + " SELECT @MINDAYS = MIN(FROMDAYS) FROM TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            strSql += vbCrLf + " DECLARE CUR CURSOR FOR SELECT FROMDAYS,TODAYS FROM TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            strSql += vbCrLf + " OPEN CUR"
            strSql += vbCrLf + " SET @STR = ' ALTER TABLE TEMPTABLEDB..TEMPAGINGRESULTAMT ADD [ < '+@MINDAYS+'] NUMERIC(15,2)'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET [ < '+@MINDAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE PARTICULAR = T.PARTICULAR AND DIFF < '+@MINDAYS+' ) FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 1'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET [ < '+@MINDAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE DIFF < '+@MINDAYS+' ) FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 3'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " FETCH NEXT FROM CUR INTO @FROMDAYS,@TODAYS"
            strSql += vbCrLf + " IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " SET @STR = ' ALTER TABLE TEMPTABLEDB..TEMPAGINGRESULTAMT ADD ['+@FROMDAYS+' To '+@TODAYS+'] NUMERIC(15,2)'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET ['+@FROMDAYS+' To '+@TODAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE PARTICULAR = T.PARTICULAR AND DIFF BETWEEN '+@FROMDAYS+' AND '+@TODAYS+') FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 1'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET ['+@FROMDAYS+' To '+@TODAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE DIFF BETWEEN '+@FROMDAYS+' AND '+@TODAYS+') FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 3'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CUR"
            strSql += vbCrLf + " DEALLOCATE CUR"
            strSql += vbCrLf + " SET @STR = ' ALTER TABLE TEMPTABLEDB..TEMPAGINGRESULTAMT ADD [ > '+@MAXDAYS+'] NUMERIC(15,2)'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET [ > '+@MAXDAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE PARTICULAR = T.PARTICULAR AND DIFF > '+@MAXDAYS+' ) FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 1'"
            strSql += vbCrLf + " EXEC(@STR)"
            strSql += vbCrLf + " SET @STR = ' UPDATE TEMPTABLEDB..TEMPAGINGRESULTAMT SET [ > '+@MAXDAYS+'] = (SELECT SUM(BALAMT) FROM TEMPTABLEDB..TEMP_DUEAGING WHERE DIFF > '+@MAXDAYS+' ) FROM TEMPTABLEDB..TEMPAGINGRESULTAMT AS T WHERE RESULT = 3'"
            strSql += vbCrLf + " EXEC(@STR)"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP_DUE_AGEANALYSIS_DAYS"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPAGINGRESULTAMT ORDER BY RESULT,PARTICULAR"
            dt = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 1 Then
                Dim STR As String
                STR = cmbCompany.Text & vbCrLf & " DUE TRANSACTION REPORT BASED "
                If rbtFifo.Checked Then
                    STR += "FIFO METHOD "
                Else
                    STR += "ACTUAL METHOD "
                End If
                STR += "AS ON " & Format(dtpFrom.Value, "dd-MM-yyyy")
                STR += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
                lblTitle.Text = STR
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    .Columns("TOTDAYS").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("BALAMT").HeaderText = "TOTAL"
                    .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    For I As Integer = 0 To .Columns.Count - 1
                        .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(I).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                    Next
                    .Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    For J As Integer = 0 To .Rows.Count - 1
                        Select Case .Rows(J).Cells("COLHEAD").Value.ToString
                            Case "G"
                                .Rows(J).DefaultCellStyle.BackColor = Color.Wheat
                                .Rows(J).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        End Select
                    Next
                    FormatGridColumns(gridView, False, False, True, False)
                    AutoReziseToolStripMenuItem_Click(Me, New EventArgs)
                End With
            Else
                gridView.DataSource = Nothing
                gridView.Refresh()
                MsgBox("No Record Found", MsgBoxStyle.Information)
            End If
        Else
            gridView.DataSource = Nothing
            gridView.Refresh()
            MsgBox("No Record Found", MsgBoxStyle.Information)
        End If
    End Sub
    Private Sub funcRangeView()
        funcGenerate()
    End Sub
    Private Sub funcView()
        gridView.DataSource = Nothing
        PnlRange.Visible = False
        AutoReziseToolStripMenuItem.Checked = False
        gridView.Refresh()
        Prop_Sets()
        If chkRange.Checked Then
            funcRangeView()
        Else
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPOUTSTANDING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPOUTSTANDING"
            strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMPOUTSTANDING_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPOUTSTANDING_RES"
            strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMPOUTSTANDING_RES1') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPOUTSTANDING_RES1"
            cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
            Dim accode As String = "ALL"
            If cmbAcName_MAN.Text <> "ALL" And cmbAcName_MAN.Text <> "" Then
                strSql = "SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "'"
                accode = objGPack.GetSqlValue(strSql, "ACCODE", "ALL", Nothing)
            End If


            strSql = "EXEC " & cnAdminDb & "..SP_RPT_BILLWISEAGEBASEDONCREDITDAYS "
            strSql += " @RPTDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += ",@ACCODE='" & accode & "'"
            strSql += ",@ACTYPE='" & IIf(rbtcreditor.Checked, "C", "D") & "'"
            Dim ds As New DataSet
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                gridView.DataSource = ds.Tables(0)
                lblTitle.Text = cmbCompany.Text & vbCrLf & " OUTSTANDING REPORT AS ON " & Format(dtpFrom.Value, "dd-MM-yyyy") & IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
                gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
                gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridView.Columns("PARTICULARS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridView.Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridView.Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridView.Columns("REFDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("ADJAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("BALANCEAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                gridView.Columns("DAYDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("CREDITDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("EXSHDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                gridView.Columns("PARTICULARS").Width = 150
                gridView.Columns("TRANNO").Width = 100
                gridView.Columns("REFNO").Width = 120
                gridView.Columns("REFDATE").Width = 120

                gridView.Columns("SNO").Visible = False
                gridView.Columns("RUNNO").Visible = False
                gridView.Columns("TRANDATE").Visible = False
                gridView.Columns("RESULT").Visible = False
                gridView.Columns("DAYDIFF").Width = 80
                gridView.Columns("CREDITDAYS").Width = 80
                gridView.Columns("EXSHDAYS").Width = 80
                For i As Integer = 0 To gridView.Rows.Count - 1
                    If Val(gridView.Rows(i).Cells("RESULT").Value.ToString) = 0 Then
                        gridView.Rows(i).Cells("PARTICULARS").Style.BackColor = Color.LightGreen
                        gridView.Rows(i).Cells("PARTICULARS").Style.ForeColor = Color.Red
                        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                    If Val(gridView.Rows(i).Cells("EXSHDAYS").Value.ToString) < 0 Then
                        gridView.Rows(i).DefaultCellStyle.BackColor = Color.Pink
                        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                    End If
                Next
                gridView.Focus()
                AutoReziseToolStripMenuItem_Click(Me, New EventArgs)
            Else
                MsgBox("Records not found..", MsgBoxStyle.Information)
            End If
        End If
        Exit Sub
        strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "'"
        Dim strCompId As String = GiritechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        Dim strCostId As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strCostId += " AND O.COSTID IN"
            strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If

        strSql = "  IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "duetran')" & vbCrLf
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & userId.ToString() & "DUETRAN" & vbCrLf
        strSql += vbCrLf + "  SELECT TRANDATE,   REFNO,BILLDATE"
        strSql += vbCrLf + " , PARTY"
        If chkBasedOnMasterDays.Checked And chkRange.Checked Then
            strSql += vbCrLf + " , SUM(PENDINGAMT)PENDINGAMT"
        ElseIf chkBasedOnMasterDays.Checked Then
            strSql += vbCrLf + " , PENDINGAMT"
        Else
            strSql += vbCrLf + " , PENDINGAMT"
        End If
        strSql += vbCrLf + " , DUEDATE, OVERDAYS "
        strSql += vbCrLf + " INTO TEMP" & systemId & userId.ToString() & "DUETRAN" & vbCrLf
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CONVERT(NVARCHAR(12), O.TRANDATE,103)  AS TRANDATE,"
        strSql += vbCrLf + " (SELECT TOP 1 REFNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO)REFNO,"
        strSql += vbCrLf + " CONVERT(NVARCHAR(12), DUEDATE,103)  AS BILLDATE"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =O.ACCODE  ) AS PARTY,"
        strSql += vbCrLf + " SUM(O.AMOUNT) - ISNULL((SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING   WHERE RUNNO=O.RUNNO AND TRANTYPE  ='D' AND RECPAY  ='P'),0) PENDINGAMT,"
        If chkBasedOnMasterDays.Checked Then
            If rdbBillDateWise.Checked Then
                strSql += vbCrLf + " CASE WHEN DATEADD(DAY,CREDITDAYS,DUEDATE)<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' THEN CONVERT(NVARCHAR(12), DUEDATE+CREDITDAYS,103) end AS  DUEDATE"
                strSql += vbCrLf + " ,CASE WHEN DATEADD(DAY,CREDITDAYS,DUEDATE)<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' THEN DATEDIFF(DAY,DUEDATE+CREDITDAYS,GETDATE()) end AS OVERDAYS "
            ElseIf rdbTranDateWise.Checked Then
                strSql += vbCrLf + " CASE WHEN DATEADD(DAY,CREDITDAYS,O.TRANDATE)<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' THEN DATEADD(DAY,ISNULL(CREDITDAYS,0),O.TRANDATE) END AS  DUEDATE "
                strSql += vbCrLf + " ,CASE WHEN DATEADD(DAY,CREDITDAYS,O.TRANDATE)<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' THEN DATEDIFF(DAY,O.TRANDATE+CREDITDAYS,GETDATE()) END AS OVERDAYS "
            End If
        Else
            strSql += vbCrLf + " CONVERT(NVARCHAR(12), DUEDATE,103) DUEDATE,"
            'strSql += vbCrLf + " DATEDIFF(DAY,O.TRANDATE,DUEDATE) AS OVERDAYS"
            strSql += vbCrLf + " CASE WHEN DATEDIFF(DAY,DUEDATE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "')>0 THEN  DATEDIFF(DAY,DUEDATE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "') END AS OVERDAYS"
        End If
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING  O"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ACHEAD AC ON O.ACCODE=AC.ACCODE AND TRANDATE<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' and PAYMODE='DU'"
        strSql += vbCrLf + "  WHERE O.TRANDATE <='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        If chkBasedOnMasterDays.Checked Then
            If rdbBillDateWise.Checked Then
                strSql += vbCrLf + " AND CONVERT(NVARCHAR(12),DATEADD(DAY,CREDITDAYS,DUEDATE),102)>'" & dtpFrom.Value.ToString("dd/MM/yyyy") & "'"
            ElseIf rdbTranDateWise.Checked Then
                strSql += vbCrLf + " AND CONVERT(NVARCHAR(12),DATEADD(DAY,CREDITDAYS,O.TRANDATE),102)>'" & dtpFrom.Value.ToString("dd/MM/yyyy") & "'"
            End If
        End If
        strSql += vbCrLf + "  AND O.TRANTYPE  ='D' AND RECPAY  ='R'  AND O.FROMFLAG='S' AND O.PAYMODE='DU'"
        strSql += vbCrLf + "  AND ISNULL(O.COMPANYID,'') = '" & strCompId & "' AND ISNULL(O.CANCEL,'') = '' "
        If cmbAcName_MAN.Text <> "" And cmbAcName_MAN.Text <> "ALL" Then
            strSql += vbCrLf + "  AND ISNULL(O.ACCODE,'') = (SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcName_MAN.Text & "')"
        End If
        strSql += vbCrLf + strCostId
        strSql += vbCrLf + "  GROUP BY O.RUNNO,O.TRANDATE ,O.ACCODE , DUEDATE ,CREDITDAYS,DUEDATE ,BATCHNO"
        strSql += vbCrLf + "  HAVING SUM(O.AMOUNT) - ISNULL((SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING   WHERE RUNNO=O.RUNNO AND TRANTYPE  ='D' AND RECPAY  ='P'),0)  >0"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY TRANDATE,PARTY,DUEDATE,OVERDAYS,BILLDATE,REFNO"
        If chkBasedOnMasterDays.Checked Then
            strSql += vbCrLf + " , PENDINGAMT"
        Else
            strSql += vbCrLf + " , PENDINGAMT"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMP" & systemId & userId.ToString() & "DUETRAN"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable()
        da.Fill(dt)
        gridView.DataSource = dt
        If gridView.RowCount > 0 Then
            lblTitle.Text = cmbCompany.Text & vbCrLf & " DUE TRANSACTION REPORT AS ON " & Format(dtpFrom.Value, "dd-MM-yyyy") & IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Columns("PARTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("BILLDATE").Width = 100
            gridView.Columns("TRANDATE").Width = 100
            gridView.Columns("PARTY").Width = 400
            gridView.Columns("REFNO").Width = 120
            gridView.Columns("PENDINGAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("PENDINGAMT").Width = 100
            gridView.Columns("OVERDAYS").Width = 80
            gridView.Focus()
            AutoReziseToolStripMenuItem_Click(Me, New EventArgs)
        Else
            MsgBox("Records not found..", MsgBoxStyle.Information)
        End If
    End Sub
    Private Sub funcNew()
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        strSql = "SELECT * FROM ("
        strSql += " SELECT 'ALL' AS ACNAME UNION ALL "
        strSql += " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " )X ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAcName_MAN, , False)
        cmbAcName_MAN.Text = "ALL"
        gridView.DataSource = Nothing
        chkRange.Checked = False
        lblTitle.Text = ""
        txtFromDay1.Text = 0
        txtToDay1.Text = 30
        txtFromDay2.Text = 31
        txtToDay2.Text = 60
        txtFromDay3.Text = 61
        txtToDay3.Text = 90
        txtFromDay4.Text = 91
        txtToDay4.Text = 120
        txtFromDay5.Text = 121
        txtToDay5.Text = 150
        txtFromDay6.Text = 151
        txtToDay6.Text = 180
        Prop_Gets()
        dtpFrom.Value = GetServerDate()
        dtpFrom.Focus()
        PnlRange.Visible = False
        chkBasedOnMasterDays.Checked = False
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmduetran_Properties
        obj.p_cmbCompany = cmbCompany.Text
        obj.p_cmbAcname = cmbAcName_MAN.Text
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtfifo = rbtFifo.Checked
        obj.p_rbtNormal = rbtNormal.Checked
        obj.p_txtFromDay1 = txtFromDay1.Text
        obj.p_txtToDay1 = txtToDay1.Text
        obj.p_txtFromDay2 = txtFromDay2.Text
        obj.p_txtToDay2 = txtToDay2.Text
        obj.p_txtFromDay3 = txtFromDay3.Text
        obj.p_txtToDay3 = txtToDay3.Text
        obj.p_txtFromDay4 = txtFromDay4.Text
        obj.p_txtToDay4 = txtToDay4.Text
        obj.p_txtFromDay5 = txtFromDay5.Text
        obj.p_txtToDay5 = txtToDay5.Text
        obj.p_txtFromDay6 = txtFromDay6.Text
        obj.p_txtToDay6 = txtToDay6.Text
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmduetran_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmduetran_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmduetran_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        cmbAcName_MAN.Text = obj.p_cmbAcname
        rbtFifo.Checked = obj.p_rbtfifo
        rbtNormal.Checked = obj.p_rbtNormal
        txtFromDay1.Text = obj.p_txtFromDay1
        txtToDay1.Text = obj.p_txtToDay1
        txtFromDay2.Text = obj.p_txtFromDay2
        txtToDay2.Text = obj.p_txtToDay2
        txtFromDay3.Text = obj.p_txtFromDay3
        txtToDay3.Text = obj.p_txtToDay3
        txtFromDay4.Text = obj.p_txtFromDay4
        txtToDay4.Text = obj.p_txtToDay4
        txtFromDay5.Text = obj.p_txtFromDay5
        txtToDay5.Text = obj.p_txtToDay5
        txtFromDay6.Text = obj.p_txtFromDay6
        txtToDay6.Text = obj.p_txtToDay6
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
    End Sub
    Private Sub GenRangeVAlues()
        Dim DIFF As Integer
        DIFF = Val(txtToDay1.Text) - Val(txtFromDay1.Text)

        txtFromDay2.Text = Val(txtToDay1.Text) + 1
        txtToDay2.Text = Val(txtFromDay2.Text) + DIFF

        txtFromDay3.Text = Val(txtToDay2.Text) + 1
        txtToDay3.Text = Val(txtFromDay3.Text) + DIFF

        txtFromDay4.Text = Val(txtToDay3.Text) + 1
        txtToDay4.Text = Val(txtFromDay4.Text) + DIFF

        txtFromDay5.Text = Val(txtToDay4.Text) + 1
        txtToDay5.Text = Val(txtFromDay5.Text) + DIFF

        txtFromDay6.Text = Val(txtToDay5.Text) + 1
        txtToDay6.Text = Val(txtFromDay6.Text) + DIFF
    End Sub
#End Region
#Region "OTHER EVENTS"
    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar.ToString().ToUpper = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar.ToString().ToUpper = "X" Then
            btnExport_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub chkBasedOnMasterDays_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBasedOnMasterDays.CheckedChanged
        If chkBasedOnMasterDays.Checked Then
            chkRange.Checked = False
            pnlactype.Visible = True
            pnldate.Visible = False
            rdbTranDateWise.Checked = True
            rdbBillDateWise.Enabled = False
        Else
            pnlactype.Visible = False
            pnldate.Visible = True
            rdbBillDateWise.Enabled = True
        End If

    End Sub

    Private Sub chkRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRange.CheckedChanged
        If chkRange.Checked Then
            PnlRange.Visible = True
            txtFromDay1.Focus()
            chkBasedOnMasterDays.Checked = False
            rbtNormal.Visible = True
            rbtFifo.Visible = True
        Else
            PnlRange.Visible = False
            chkBasedOnMasterDays.Checked = True
            rbtNormal.Visible = False
            rbtFifo.Visible = False
        End If
    End Sub

    Private Sub GrpRange_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            GrpRange.Visible = False
        End If
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If AutoReziseToolStripMenuItem.Checked Then
            AutoReziseToolStripMenuItem.Checked = False
        Else
            AutoReziseToolStripMenuItem.Checked = True
        End If
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
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
        End If
    End Sub
#End Region
#Region "TEXTBOX EVENTS"
    Private Sub txtToDay6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay6.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtToDay5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay5.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtToDay4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay4.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtToDay3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay3.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtToDay2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay2.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtToDay1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToDay1.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        ElseIf e.KeyCode = Keys.Enter Then
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtFromDay6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay6.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtFromDay5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay5.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtFromDay4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay4.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtFromDay3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay3.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtFromDay2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay2.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub

    Private Sub txtFromDay1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromDay1.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnView.Focus()
            PnlRange.Visible = False
        End If
    End Sub
    Private Sub txtToDay2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDay2.TextChanged
        If txtToDay2.Text = "" Then
            txtFromDay3.Clear()
            txtToDay3.Clear()
            txtFromDay4.Clear()
            txtToDay4.Clear()
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtToDay3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDay3.TextChanged
        If txtToDay3.Text = "" Then
            txtFromDay4.Clear()
            txtToDay4.Clear()
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtToDay4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDay4.TextChanged
        If txtToDay4.Text = "" Then
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtToDay5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDay5.TextChanged
        If txtToDay5.Text = "" Then
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub
    Private Sub txtFromDay2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay2.TextChanged
        If txtFromDay2.Text = "" Then
            txtToDay2.Clear()
            txtFromDay3.Clear()
            txtToDay3.Clear()
            txtFromDay4.Clear()
            txtToDay4.Clear()
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtFromDay3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay3.TextChanged
        If txtFromDay3.Text = "" Then
            txtToDay3.Clear()
            txtFromDay4.Clear()
            txtToDay4.Clear()
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtFromDay4_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay4.TextChanged
        If txtFromDay4.Text = "" Then
            txtToDay4.Clear()
            txtFromDay5.Clear()
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtFromDay5_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay5.TextChanged
        If txtFromDay5.Text = "" Then
            txtToDay5.Clear()
            txtFromDay6.Clear()
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub

    Private Sub txtFromDay6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay6.TextChanged
        If txtFromDay6.Text = "" Then
            txtToDay6.Clear()
        Else
            GenRangeVAlues()
        End If
    End Sub
#End Region
End Class

Public Class frmduetran_Properties
    Private cmbAcname As String
    Public Property p_cmbAcname() As String
        Get
            Return cmbAcname
        End Get
        Set(ByVal value As String)
            cmbAcname = value
        End Set
    End Property
    Private cmbCompany As String = "ALL"
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkRange As Boolean = True
    Public Property p_chkRange() As Boolean
        Get
            Return chkRange
        End Get
        Set(ByVal value As Boolean)
            chkRange = value
        End Set
    End Property
    Private rbtNormal As Boolean = True
    Public Property p_rbtNormal() As Boolean
        Get
            Return rbtNormal
        End Get
        Set(ByVal value As Boolean)
            rbtNormal = value
        End Set
    End Property
    Private rbtfifo As Boolean = True
    Public Property p_rbtfifo() As Boolean
        Get
            Return rbtfifo
        End Get
        Set(ByVal value As Boolean)
            rbtfifo = value
        End Set
    End Property
    Private txtFromDay4 As String = ""
    Public Property p_txtFromDay4() As String
        Get
            Return txtFromDay4
        End Get
        Set(ByVal value As String)
            txtFromDay4 = value
        End Set
    End Property
    Private txtToDay4 As String = ""
    Public Property p_txtToDay4() As String
        Get
            Return txtToDay4
        End Get
        Set(ByVal value As String)
            txtToDay4 = value
        End Set
    End Property

    Private txtFromDay5 As String = ""
    Public Property p_txtFromDay5() As String
        Get
            Return txtFromDay5
        End Get
        Set(ByVal value As String)
            txtFromDay5 = value
        End Set
    End Property
    Private txtToDay5 As String = ""
    Public Property p_txtToDay5() As String
        Get
            Return txtToDay5
        End Get
        Set(ByVal value As String)
            txtToDay5 = value
        End Set
    End Property
    Private txtFromDay6 As String = ""
    Public Property p_txtFromDay6() As String
        Get
            Return txtFromDay6
        End Get
        Set(ByVal value As String)
            txtFromDay6 = value
        End Set
    End Property
    Private txtToDay6 As String = ""
    Public Property p_txtToDay6() As String
        Get
            Return txtToDay6
        End Get
        Set(ByVal value As String)
            txtToDay6 = value
        End Set
    End Property
    Private txtFromDay1 As String = ""
    Public Property p_txtFromDay1() As String
        Get
            Return txtFromDay1
        End Get
        Set(ByVal value As String)
            txtFromDay1 = value
        End Set
    End Property
    Private txtToDay1 As String = ""
    Public Property p_txtToDay1() As String
        Get
            Return txtToDay1
        End Get
        Set(ByVal value As String)
            txtToDay1 = value
        End Set
    End Property

    Private txtFromDay2 As String = ""
    Public Property p_txtFromDay2() As String
        Get
            Return txtFromDay2
        End Get
        Set(ByVal value As String)
            txtFromDay2 = value
        End Set
    End Property
    Private txtToDay2 As String = ""
    Public Property p_txtToDay2() As String
        Get
            Return txtToDay2
        End Get
        Set(ByVal value As String)
            txtToDay2 = value
        End Set
    End Property
    Private txtFromDay3 As String = ""
    Public Property p_txtFromDay3() As String
        Get
            Return txtFromDay3
        End Get
        Set(ByVal value As String)
            txtFromDay3 = value
        End Set
    End Property
    Private txtToDay3 As String = ""
    Public Property p_txtToDay3() As String
        Get
            Return txtToDay3
        End Get
        Set(ByVal value As String)
            txtToDay3 = value
        End Set
    End Property
End Class