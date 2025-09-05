Imports System.Data.OleDb
Public Class frmOutstanding_Transfer
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim BillCostId As String
    Dim dtGridView As DataTable
    Public withOrderDetail As Boolean = False
    Public withRepairDetail As Boolean = False
    Public dtGridAdvance As New DataTable
    Dim dtitem As New DataTable
    Public BillDate As Date = GetServerDate(tran)
    Dim Syncdb As String = cnAdminDb
    Dim ISMAINT_CENTRETURN As Boolean = IIf(GetAdmindbSoftValue("MAINT_CENTRETURN", "N") = "Y", True, False)
    Dim COSTCENTRE_SINGLE As Boolean = IIf(GetAdmindbSoftValue("COSTCENTRE_SINGLE", "N") = "Y", True, False)
    Dim _CostId As String = ""
    Dim _SingledbRunno As String = ""

    Private Sub txtRunno_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRunno_MAN.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub frmOutstanding_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOutstanding_Transfer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        funcNew()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Function funcNew()
        strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN('" & cnCostId & "')"
        objGPack.FillCombo(strSql, cmbTocostCentre, True, False)
        strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        objGPack.FillCombo(strSql, cmbFromcostCentre, True, False)
        txtRunno_MAN.Text = ""
        GridView.DataSource = ""
        cmbTocostCentre.Text = ""
        cmbFromcostCentre.Text = ""
        _SingledbRunno = ""
        btnTransfer.Enabled = False
        rbtAdvance.Focus()
        dtpSRBillDate.Value = GetServerDate()
        If ISMAINT_CENTRETURN = False Then
            rbtAdvance.Checked = True
            rbtSR.Visible = False
        End If
        rbtAdvance_CheckedChanged(Me, New EventArgs)
    End Function

    Function Gettranno(ByVal trantype As String)
        If trantype = "RECEIPT" Then
            Return GetBillNoValue("GEN-RECEIPTBILLNO", tran)
        ElseIf trantype = "PAYMENT" Then
            Return GetBillNoValue("GEN-PAYMENTBILLNO", tran)
        ElseIf trantype = "OrderRepayment" Then
            Return GetBillNoValue("GEN-ORDREPAYMENT", tran)
        End If
    End Function

    Private Sub InsertDetailsTran(ByVal DbName As String, ByVal FromTableName As String, ByVal ToTableName As String, ByVal CondColName As String, ByVal CondColValue As String, ByVal FromCostId As String, ByVal ToCostId As String, ByVal _Tran As OleDbTransaction)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        strSql = " IF OBJECT_ID('" & DbName & "..INS_" & FromTableName & "', 'U') IS NOT NULL DROP TABLE " & DbName & "..INS_" & FromTableName & ""
        strSql += vbCrLf + " SELECT * INTO " & DbName & "..INS_" & FromTableName & " FROM " & DbName & ".." & FromTableName & " WHERE " & CondColName & " = '" & CondColValue & "'"
        cmd = New OleDbCommand(strSql, cn, _Tran)
        cmd.ExecuteNonQuery()
        strSql = " UPDATE " & DbName & "..INS_" & FromTableName & " SET COSTID = '" & FromCostId & "'"
        cmd = New OleDbCommand(strSql, cn, _Tran)
        cmd.ExecuteNonQuery()
        strSql = " EXEC " & DbName & "..INSERTQRYGENERATOR_TABLE "
        strSql += vbCrLf + " @DBNAME = '" & DbName & "',@TABLENAME = 'INS_" & FromTableName & "',@MASK_TABLENAME = '" & ToTableName & "'"
        cmd = New OleDbCommand(strSql, cn, _Tran)
        da = New OleDbDataAdapter(cmd)
        dtTemp = New DataTable
        da.Fill(dtTemp)
        Qry = ""
        For Each ro As DataRow In dtTemp.Rows
            Qry = ro.Item(0).ToString
            strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            strSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            cmd = New OleDbCommand(strSql, cn, _Tran)
            cmd.ExecuteNonQuery()
        Next
        strSql = " DROP TABLE " & DbName & "..INS_" & FromTableName & ""
        cmd = New OleDbCommand(strSql, cn, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Try
            If GridView.Rows.Count > 0 Then
                _SingledbRunno = ""
                If rbtSR.Checked Then
                    Dim SReturnDb As String
                    Dim TocostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbTocostCentre.Text & "'")
                    strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpSRBillDate.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
                    SReturnDb = objGPack.GetSqlValue(strSql)
                    strSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..SYSDATABASES WHERE NAME = '" & SReturnDb & "'"
                    If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then
                        MsgBox(SReturnDb & " Database not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    tran = Nothing
                    tran = cn.BeginTransaction()
                    For i As Integer = 0 To GridView.Rows.Count - 1
                        Dim Sno As String = GridView.Rows(i).Cells("SNO").Value.ToString
                        InsertDetailsTran(SReturnDb, "ISSUE", "ISSUE", "SNO", Sno, cnCostId, TocostId, tran)
                        InsertDetailsTran(SReturnDb, "ISSSTONE", "ISSSTONE", "ISSSNO", Sno, cnCostId, TocostId, tran)
                        strSql = " UPDATE " & SReturnDb & "..ISSUE SET REFDATE = '" & GetServerDate(tran) & "' "
                        strSql += " WHERE SNO = '" & Sno & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    Next
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Successfully Transfered.", MsgBoxStyle.Information)
                    funcNew()
                    Exit Sub
                ElseIf rbtGiftVoucher.Checked Then
                    strSql = " SELECT DISTINCT(SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN"
                    strSql += vbCrLf + " (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=O.BATCHNO))ACNAME,"
                    strSql += vbCrLf + " CONVERT(VARCHAR(12),TRANDATE,103)TRANDATE,TRANNO,BATCHNO "
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O WHERE RUNNO='" & GridView.Rows(0).Cells("RUNNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    Dim dtk As New DataTable
                    Dim strRemark1 As String = ""
                    da.Fill(dtk)
                    If dtk.Rows.Count > 0 Then
                        strRemark1 = dtk.Rows(0)("ACNAME").ToString & " " & dtk.Rows(0)("TRANDATE").ToString & " [" & dtk.Rows(0)("TRANNO").ToString & "]"
                    End If
                    Dim batchno, Runno, Recpay, Paymode As String
                    Dim Trfamount As Double = 0
                    Dim TrfGrswt As Double = 0
                    Dim TrfNetwt As Double = 0
                    Dim Tranno As Integer
                    Dim advtrfTranno As String
                    Dim tempStrbcostid As String = ""

                    Dim Tocostid As String
                    If GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim = "RECEIPT" Then
                        Recpay = "R"
                    Else
                        Recpay = "P"
                    End If

                    Dim dtOutSt As New DataTable
                    strSql = "SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & GridView.Rows(0).Cells("RUNNO").Value.ToString & "' AND RECPAY = '" & Recpay & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtOutSt)

                    Runno = GridView.Rows(0).Cells("RUNNO").Value.ToString
                    Trfamount = Val(GridView.Rows(0).Cells("AMOUNT").Value.ToString)
                    TrfGrswt = Val(GridView.Rows(0).Cells("GRSWT").Value.ToString)
                    TrfNetwt = Val(GridView.Rows(0).Cells("NETWT").Value.ToString)
                    If Recpay = "R" Then Recpay = "P" Else Recpay = "R"
                    Dim rate As Double = Nothing
                    rate = Val(GetRate(GetEntryDate(BillDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & dtOutSt.Rows(0).Item("CATCODE").ToString & "'")))

                    If GetAdmindbSoftValue("ADV_TRANSFER_ENTRYDATE", "N", tran) = "Y" Then
                        BillDate = GetServerDate(tran)
                    End If

                    Dim PSNO As String = "'"
                    PSNO = objGPack.GetSqlValue("SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & dtOutSt.Rows(0).Item("BATCHNO").ToString & "'", , "")
                    Paymode = "GV"
                    Tocostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & GridView.Rows(0).Cells("TO_COSTCENTER").Value.ToString & "'", , "")
                    Dim Trfaccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , )


                    If Trfaccode.ToString = "" Then
                        If MsgBox("Cost centre account code is blank" & vbCrLf & "Blank code restricted, Please check", MsgBoxStyle.OkOnly) = MsgBoxResult.Ok Then Exit Sub
                    End If
                    Dim Trftaccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , )

                    'Trfaccode
                    If objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , ) <> "" Then
                        Trfaccode = objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , )
                    End If
                    'Trftaccode
                    If objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , ) <> "" Then
                        Trftaccode = objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , )
                    End If
                    tran = Nothing
                    tran = cn.BeginTransaction()
                    If GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim = "RECEIPT" And COSTCENTRE_SINGLE Then
                        strBCostid = cnCostId
                        Tranno = Val(GetBillControlValue("GEN-PAYMENTBILLNO", tran, False).ToString) + 1
                        strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & Val(Tranno) & "' WHERE "
                        strSql += " CTLID ='GEN-PAYMENTBILLNO' AND COMPANYID='" & strCompanyId.ToString.Trim & "' AND COSTID='" & cnCostId.ToString.Trim & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        tempStrbcostid = strBCostid
                        strBCostid = Tocostid
                        advtrfTranno = ""
                        advtrfTranno = Val(GetBillControlValue("GEN-RECEIPTBILLNO", tran, False).ToString) + 1
                        If Val(advtrfTranno) > 0 Then
                            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & Val(advtrfTranno) & "' WHERE "
                            strSql += " CTLID ='GEN-RECEIPTBILLNO' AND COMPANYID='" & strCompanyId.ToString.Trim & "' AND COSTID='" & strBCostid.ToString.Trim & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        End If
                        strBCostid = tempStrbcostid
                    Else
                        Tranno = Gettranno(GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim)
                    End If
                    batchno = GetNewBatchno(cnCostId, BillDate, tran)
                    If COSTCENTRE_SINGLE = True Then
                        Outstanding_transfer_SingleDB(Runno, dtOutSt.Rows(0).Item("BATCHNO").ToString, dtOutSt.Rows(0).Item("COSTID").ToString, Tocostid, Trfamount, Val(advtrfTranno.ToString), batchno)
                    Else
                        Outstanding_transfer(Runno, dtOutSt.Rows(0).Item("BATCHNO").ToString, dtOutSt.Rows(0).Item("COSTID").ToString, Tocostid, Trfamount)
                    End If

                    InsertIntoOustanding(Tranno, dtOutSt.Rows(0).Item("TRANTYPE").ToString, Runno, Trfamount _
                    , Recpay, Paymode, batchno, TrfGrswt, TrfNetwt, dtOutSt.Rows(0).Item("CATCODE").ToString _
                    , rate, Trfamount.ToString, dtOutSt.Rows(0).Item("REFNO").ToString, dtOutSt.Rows(0).Item("REFDATE").ToString _
                    , Val(dtOutSt.Rows(0).Item("PURITY").ToString), Val(dtOutSt.Rows(0).Item("CTRANCODE").ToString), , , , dtOutSt.Rows(0).Item("ACCODE").ToString, , 0, Val(dtOutSt.Rows(0).Item("PUREWT").ToString) _
                    , Val(dtOutSt.Rows(0).Item("ADVFIXWTPER").ToString), dtOutSt.Rows(0).Item("CASHID").ToString, PSNO)

                    Dim Drcontra As String
                    Dim Crcontra As String
                    Dim mcusname As String = ""
                    Paymode = "GV"
                    Drcontra = IIf(dtOutSt.Rows(0).Item("ACCODE").ToString <> "", dtOutSt.Rows(0).Item("ACCODE").ToString, GetAdmindbSoftValue("GIFT_ACCODE", "", tran))
                    Crcontra = Trfaccode.ToString

                    If COSTCENTRE_SINGLE = True Then
                        If Val(advtrfTranno) > 0 Then
                            Acctran_transfer_SingleDB(advtrfTranno, "GV", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                        Else
                            Acctran_transfer_SingleDB(Tranno, "GV", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                        End If
                    Else
                        Acctran_transfer(Tranno, "GV", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                    End If

                    InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "D", "C"),
                    Drcontra, Trfamount, 0, 0, 0, "GV", Crcontra, batchno, , , , , , , strRemark1, )

                    InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "C", "D"),
                                    Crcontra, Trfamount, 0, 0, 0, "GV", Drcontra, batchno, , , , , , , strRemark1, )
                    tran.Commit()
                    tran = Nothing
                    If COSTCENTRE_SINGLE = True Then
                        If _SingledbRunno.ToString <> "" Then
                            _SingledbRunno = _SingledbRunno.Substring(0, Val(_SingledbRunno.Length) - 1)
                            MsgBox("Oustanding Successfully Transfer " + vbCrLf + "RunNo : " & _SingledbRunno.ToString, MsgBoxStyle.Information)
                        Else
                            MsgBox("Oustanding Successfully Transfer ", MsgBoxStyle.Information)
                        End If

                    Else
                        MsgBox("Oustanding Successfully Transfer ", MsgBoxStyle.Information)
                    End If

                    funcNew()
                    Exit Sub
                Else

                    strSql = " SELECT DISTINCT(SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN"
                    strSql += vbCrLf + " (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=O.BATCHNO))ACNAME,"
                    strSql += vbCrLf + " CONVERT(VARCHAR(12),TRANDATE,103)TRANDATE,TRANNO,BATCHNO "
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O WHERE RUNNO='" & GridView.Rows(0).Cells("RUNNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    Dim dtk As New DataTable
                    Dim strRemark1 As String = ""
                    da.Fill(dtk)
                    If dtk.Rows.Count > 0 Then
                        strRemark1 = dtk.Rows(0)("ACNAME").ToString & " " & dtk.Rows(0)("TRANDATE").ToString & " [" & dtk.Rows(0)("TRANNO").ToString & "]"
                    End If
                    Dim batchno, Runno, Recpay, Paymode As String
                    Dim Trfamount As Double = 0
                    Dim TrfGrswt As Double = 0
                    Dim TrfNetwt As Double = 0
                    Dim Tranno As Integer
                    Dim advtrfTranno As String
                    Dim tempStrbcostid As String = ""

                    Dim Tocostid As String
                    If GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim = "RECEIPT" Then
                        Recpay = "R"
                    Else
                        Recpay = "P"
                    End If

                    Dim dtOutSt As New DataTable
                    strSql = "SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & GridView.Rows(0).Cells("RUNNO").Value.ToString & "' AND RECPAY = '" & Recpay & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtOutSt)

                    Runno = GridView.Rows(0).Cells("RUNNO").Value.ToString
                    Trfamount = Val(GridView.Rows(0).Cells("AMOUNT").Value.ToString)
                    TrfGrswt = Val(GridView.Rows(0).Cells("GRSWT").Value.ToString)
                    TrfNetwt = Val(GridView.Rows(0).Cells("NETWT").Value.ToString)
                    If Recpay = "R" Then Recpay = "P" Else Recpay = "R"
                    Dim rate As Double = Nothing
                    rate = Val(GetRate(GetEntryDate(BillDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & dtOutSt.Rows(0).Item("CATCODE").ToString & "'")))

                    If GetAdmindbSoftValue("ADV_TRANSFER_ENTRYDATE", "N", tran) = "Y" Then
                        BillDate = GetServerDate(tran)
                    End If

                    Dim PSNO As String = "'"
                    PSNO = objGPack.GetSqlValue("SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & dtOutSt.Rows(0).Item("BATCHNO").ToString & "'", , "")


                    If GridView.Rows(0).Cells("TRANTYPE").Value.ToString = "ADVANCE" Then
                        Paymode = "A" & Recpay
                    ElseIf GridView.Rows(0).Cells("TRANTYPE").Value.ToString = "CREDIT" Then
                        Paymode = "D" & Recpay
                    ElseIf GridView.Rows(0).Cells("TRANTYPE").Value.ToString = "ORDER" Then
                        Paymode = "O" & Recpay
                    End If
                    Tocostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & GridView.Rows(0).Cells("TO_COSTCENTER").Value.ToString & "'", , "")
                    Dim Trfaccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , )


                    If Trfaccode.ToString = "" Then
                        If MsgBox("Cost centre account code is blank" & vbCrLf & "Blank code restricted, Please check", MsgBoxStyle.OkOnly) = MsgBoxResult.Ok Then Exit Sub
                    End If
                    Dim Trftaccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , )


                    'Trfaccode
                    If GridView.Rows(0).Cells("TRANTYPE").Value.ToString.Trim = "CREDIT" Then
                        If objGPack.GetSqlValue("SELECT CRACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , ) <> "" Then
                            Trfaccode = objGPack.GetSqlValue("SELECT CRACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , )
                        End If
                    ElseIf GridView.Rows(0).Cells("TRANTYPE").Value.ToString.Trim = "ADVANCE" Then
                        If objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , ) <> "" Then
                            Trfaccode = objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , )
                        End If
                    End If
                    'Trftaccode
                    If GridView.Rows(0).Cells("TRANTYPE").Value.ToString.Trim = "CREDIT" Then
                        If objGPack.GetSqlValue("SELECT CRACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , ) <> "" Then
                            Trftaccode = objGPack.GetSqlValue("SELECT CRACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , )
                        End If
                    ElseIf GridView.Rows(0).Cells("TRANTYPE").Value.ToString.Trim = "ADVANCE" Then
                        If objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & Tocostid & "'", , , ) <> "" Then
                            Trftaccode = objGPack.GetSqlValue("SELECT ADVACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'", , , )
                        End If
                    End If
                    tran = Nothing
                    tran = cn.BeginTransaction()
                    If GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim = "RECEIPT" And COSTCENTRE_SINGLE Then
                        strBCostid = cnCostId
                        Tranno = Val(GetBillControlValue("GEN-PAYMENTBILLNO", tran, False).ToString) + 1
                        strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & Val(Tranno) & "' WHERE "
                        strSql += " CTLID ='GEN-PAYMENTBILLNO' AND COMPANYID='" & strCompanyId.ToString.Trim & "' AND COSTID='" & cnCostId.ToString.Trim & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        tempStrbcostid = strBCostid
                        strBCostid = Tocostid
                        advtrfTranno = ""
                        advtrfTranno = Val(GetBillControlValue("GEN-RECEIPTBILLNO", tran, False).ToString) + 1
                        If Val(advtrfTranno) > 0 Then
                            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & Val(advtrfTranno) & "' WHERE "
                            strSql += " CTLID ='GEN-RECEIPTBILLNO' AND COMPANYID='" & strCompanyId.ToString.Trim & "' AND COSTID='" & strBCostid.ToString.Trim & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        End If
                        strBCostid = tempStrbcostid
                    Else
                        Tranno = Gettranno(GridView.Rows(0).Cells("RECPAY").Value.ToString.Trim)
                    End If
                    batchno = GetNewBatchno(cnCostId, BillDate, tran)
                    If COSTCENTRE_SINGLE = True Then
                        Outstanding_transfer_SingleDB(Runno, dtOutSt.Rows(0).Item("BATCHNO").ToString, dtOutSt.Rows(0).Item("COSTID").ToString, Tocostid, Trfamount, Val(advtrfTranno.ToString), batchno)
                    Else
                        Outstanding_transfer(Runno, dtOutSt.Rows(0).Item("BATCHNO").ToString, dtOutSt.Rows(0).Item("COSTID").ToString, Tocostid, Trfamount)
                    End If

                    InsertIntoOustanding(Tranno, dtOutSt.Rows(0).Item("TRANTYPE").ToString, Runno, Trfamount _
                    , Recpay, Paymode, batchno, TrfGrswt, TrfNetwt, dtOutSt.Rows(0).Item("CATCODE").ToString _
                    , rate, Trfamount.ToString, dtOutSt.Rows(0).Item("REFNO").ToString, dtOutSt.Rows(0).Item("REFDATE").ToString _
                    , Val(dtOutSt.Rows(0).Item("PURITY").ToString), Val(dtOutSt.Rows(0).Item("CTRANCODE").ToString), , , , dtOutSt.Rows(0).Item("ACCODE").ToString, , 0, Val(dtOutSt.Rows(0).Item("PUREWT").ToString) _
                    , Val(dtOutSt.Rows(0).Item("ADVFIXWTPER").ToString), dtOutSt.Rows(0).Item("CASHID").ToString, PSNO)

                    Dim Drcontra As String
                    Dim Crcontra As String
                    Dim mcusname As String = ""
                    If GridView.Rows(0).Cells("TRANTYPE").Value.ToString = "ADVANCE" Then
                        Paymode = "A" & Recpay
                        Drcontra = IIf(dtOutSt.Rows(0).Item("ACCODE").ToString <> "", dtOutSt.Rows(0).Item("ACCODE").ToString, "ADVANCE")
                        Crcontra = Trfaccode.ToString

                        If COSTCENTRE_SINGLE = True Then
                            If Val(advtrfTranno) > 0 Then
                                Acctran_transfer_SingleDB(advtrfTranno, "AR", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                            Else
                                Acctran_transfer_SingleDB(Tranno, "AR", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                            End If
                        Else
                            Acctran_transfer(Tranno, "AR", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                        End If

                        InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "D", "C"),
                        Drcontra, Trfamount, 0, 0, 0, "AA", Crcontra, batchno, , , , , , , strRemark1)

                        InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "C", "D"),
                                        Crcontra, Trfamount, 0, 0, 0, "AA", Drcontra, batchno, , , , , , , strRemark1)

                    ElseIf GridView.Rows(0).Cells("TRANTYPE").Value.ToString = "CREDIT" Then
                        Paymode = "D" & Recpay
                        Drcontra = IIf(dtOutSt.Rows(0).Item("ACCODE").ToString <> "", dtOutSt.Rows(0).Item("ACCODE").ToString, "DRS")
                        Crcontra = Trfaccode.ToString
                        If COSTCENTRE_SINGLE = True Then
                            Acctran_transfer_SingleDB(Tranno, "DU", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                        Else
                            Acctran_transfer(Tranno, "DU", batchno, cnCostId, Tocostid, Trfamount, Drcontra, Trftaccode, Runno)
                        End If

                        InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "C", "D"),
                        Drcontra, Trfamount, 0, 0, 0, Paymode, Crcontra, batchno, , , , , , , strRemark1)
                        InsertIntoAccTran(Tranno, IIf(Trfamount > 0, "D", "C"),
                                       Crcontra, Trfamount, 0, 0, 0, Paymode, Drcontra, batchno, , , , , , , strRemark1)
                    End If
                    tran.Commit()
                    tran = Nothing
                    If COSTCENTRE_SINGLE = True Then
                        If _SingledbRunno.ToString <> "" Then
                            _SingledbRunno = _SingledbRunno.Substring(0, Val(_SingledbRunno.Length) - 1)
                            MsgBox("Oustanding Successfully Transfer " + vbCrLf + "RunNo : " & _SingledbRunno.ToString, MsgBoxStyle.Information)
                        Else
                            MsgBox("Oustanding Successfully Transfer ", MsgBoxStyle.Information)
                        End If

                    Else
                        MsgBox("Oustanding Successfully Transfer ", MsgBoxStyle.Information)
                    End If

                    funcNew()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
        End Try
    End Sub

    Private Sub Acctran_transfer(ByVal Tranno As String, ByVal Paymode As String, ByVal batchno As String, ByVal fromCostId As String, ByVal Tocostid As String, ByVal AMOUNT As String, ByVal drcode As String, ByVal crcode As String, ByVal Runno As String)
        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPACCTRAN', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPACCTRAN FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim Recpay As String = "R"
        Dim Ttype As String = ""
        If Paymode = "DU" Then
            Ttype = IIf(AMOUNT < 0, "C", "D")
        Else
            Ttype = IIf(AMOUNT > 0, "C", "D")
        End If
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
        strSql += " ,'" & BillDate & "'" 'TRANDATE
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
        If Paymode = "DU" Then
            Ttype = IIf(AMOUNT < 0, "D", "C")
        Else
            Ttype = IIf(AMOUNT > 0, "D", "C")
        End If
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
        strSql += " ,'" & BillDate & "'" 'TRANDATE
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
        strSql += vbCrLf + " SELECT '" & fromCostId & "','" & Tocostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
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
        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

    End Sub
    Private Sub Acctran_transfer_SingleDB(ByVal Tranno As String, ByVal Paymode As String, ByVal batchno As String, ByVal fromCostId As String, ByVal Tocostid As String, ByVal AMOUNT As String, ByVal drcode As String, ByVal crcode As String, ByVal Runno As String)
        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPACCTRAN', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPACCTRAN FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim Recpay As String = "R"
        Dim Ttype As String = ""
        If Paymode = "DU" Then
            Ttype = IIf(AMOUNT < 0, "C", "D")
        Else
            Ttype = IIf(AMOUNT > 0, "C", "D")
        End If
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
        strSql += " ,'" & BillDate & "'" 'TRANDATE
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
        If Paymode = "DU" Then
            Ttype = IIf(AMOUNT < 0, "D", "C")
        Else
            Ttype = IIf(AMOUNT > 0, "D", "C")
        End If
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
        strSql += " ,'" & BillDate & "'" 'TRANDATE
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

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + " SELECT * FROM " & cnStockDb & "..INS_TEMPACCTRAN"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPACCTRAN"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

    End Sub

    Private Sub Outstanding_transfer_SingleDB(ByVal runno As String, ByVal batchno As String, ByVal fromcostid As String, ByVal tocostid As String, ByVal tamount As Double, ByVal paytranno As Integer, ByVal newbatchno As String)

        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPOUTSTANDING', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPOUTSTANDING"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPOUTSTANDING FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO='" & batchno & "' AND RUNNO ='" & runno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        If _SingledbRunno.ToString = "" Then _SingledbRunno = ""
        Dim newsno As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        Dim _temprunno As String = ""
        strSql = " UPDATE " & cnStockDb & "..INS_TEMPOUTSTANDING SET AMOUNT = " & tamount & ",SNO = '" & newsno & "'"
        strSql += vbCrLf + " ,COSTID='" & tocostid.Trim & "', TRANSTATUS = 'T', TRANDATE='" & BillDate & "'"
        If runno <> "" And Val(paytranno.ToString.Trim) > 0 Then _temprunno = runno.Substring(0, 8) & Val(paytranno.ToString.Trim)
        strSql += vbCrLf + " ,RUNNO='" & _temprunno.Replace(fromcostid, tocostid).ToString & "'"
        If Val(paytranno.ToString.Trim) > 0 Then
            strSql += vbCrLf + " ,TRANNO='" & Val(paytranno.ToString.Trim) & "' "
        End If
        If newbatchno.ToString.Trim <> "" Then
            strSql += vbCrLf + " ,BATCHNO='" & newbatchno & "' "
        End If
        If GetAdmindbSoftValue("ADV_TRANSFER_ENTRYDATE", "N", tran) = "Y" Then
            strSql += vbCrLf + " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "',UPTIME='" & Date.Now.ToLongTimeString & "'   "
        End If
        strSql += vbCrLf + " WHERE BATCHNO='" & batchno & "' AND RUNNO ='" & runno & "' "
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        _SingledbRunno += _temprunno.Replace(fromcostid, tocostid).ToString & ","


        Dim DtTempAccode1 As New DataTable
        'DtTempAccode1 = New DataTable
        strSql = "SELECT * FROM " & cnStockDb & "..INS_TEMPOUTSTANDING "
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTempAccode1)

        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " SELECT * FROM " & cnStockDb & "..INS_TEMPOUTSTANDING"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPOUTSTANDING"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub Outstanding_transfer(ByVal runno As String, ByVal batchno As String, ByVal fromcostid As String, ByVal tocostid As String, ByVal tamount As Double)

        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPOUTSTANDING', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPOUTSTANDING"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPOUTSTANDING FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO='" & batchno & "' AND RUNNO ='" & runno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim newsno As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        strSql = " UPDATE " & cnStockDb & "..INS_TEMPOUTSTANDING SET AMOUNT = " & tamount & ",SNO = '" & newsno & "'"
        strSql += vbCrLf + " ,COSTID='" & tocostid.Trim & "', TRANSTATUS = 'T', TRANDATE='" & BillDate & "'"
        If GetAdmindbSoftValue("ADV_TRANSFER_ENTRYDATE", "N", tran) = "Y" Then
            strSql += vbCrLf + " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "',UPTIME='" & Date.Now.ToLongTimeString & "'   "
        End If
        strSql += vbCrLf + " WHERE BATCHNO='" & batchno & "' AND RUNNO ='" & runno & "' "
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        Dim mtempqrytb As String = "TEMPQRYTB"
        strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_TEMPOUTSTANDING',@MASK_TABLENAME = 'OUTSTANDING',@TEMPTABLE='" & mtempqrytb & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        strSql += vbCrLf + " SELECT '" & fromcostid & "','" & tocostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim costids As String = "'" & fromcostid & "','" & tocostid & "'"
        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID in (" & costids & ") AND MAIN = 'Y'"
        If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "0", tran) = "0" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mainCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran)
            strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            strSql += vbCrLf + " SELECT '" & fromcostid & "','" & mainCostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPOUTSTANDING"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()


        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPCUSTOMERINFO', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPCUSTOMERINFO"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPCUSTOMERINFO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & batchno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " UPDATE " & cnStockDb & "..INS_TEMPCUSTOMERINFO SET costid='" & tocostid.Trim & "' WHERE BATCHNO='" & batchno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Qry = Nothing
        dtTemp = New DataTable
        mtempqrytb = "TEMPQRYTB"
        strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_TEMPCUSTOMERINFO',@MASK_TABLENAME = 'CUSTOMERINFO',@TEMPTABLE='" & mtempqrytb & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        strSql += vbCrLf + " SELECT '" & fromcostid & "','" & tocostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        costids = "'" & fromcostid & "','" & tocostid & "'"
        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID in (" & costids & ") AND MAIN = 'Y'"
        If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "0", tran) = "0" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mainCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran)
            strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            strSql += vbCrLf + " SELECT '" & fromcostid & "','" & mainCostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPCUSTOMERINFO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()


        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_TEMPERSONALINFO', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_TEMPERSONALINFO"
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_TEMPERSONALINFO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & batchno & "')"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " UPDATE " & cnStockDb & "..INS_TEMPERSONALINFO SET COSTID='" & tocostid.Trim & "' where SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & batchno & "')"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Qry = Nothing
        dtTemp = New DataTable
        mtempqrytb = "TEMPQRYTB"
        strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_TEMPERSONALINFO',@MASK_TABLENAME = 'PERSONALINFO',@TEMPTABLE='" & mtempqrytb & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        strSql += vbCrLf + " SELECT '" & fromcostid & "','" & tocostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        costids = "'" & fromcostid & "','" & tocostid & "'"
        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID in (" & costids & ") AND MAIN = 'Y'"
        If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "0", tran) = "0" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mainCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran)
            strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            strSql += vbCrLf + " SELECT '" & fromcostid & "','" & mainCostid & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If

        strSql = " DROP TABLE " & cnStockDb & "..INS_TEMPERSONALINFO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()



    End Sub
    Private Sub InsertIntoOustanding _
 (
 ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
 ByVal RecPay As String,
 ByVal Paymode As String,
 ByVal Batchno As String,
 Optional ByVal GrsWt As Double = 0,
 Optional ByVal NetWt As Double = 0,
 Optional ByVal CatCode As String = Nothing,
 Optional ByVal Rate As Double = Nothing,
 Optional ByVal Value As Double = Nothing,
 Optional ByVal refNo As String = Nothing,
 Optional ByVal refDate As String = Nothing,
 Optional ByVal purity As Double = Nothing,
 Optional ByVal proId As Integer = Nothing,
 Optional ByVal dueDate As String = Nothing,
 Optional ByVal Remark1 As String = Nothing,
 Optional ByVal Remark2 As String = Nothing,
 Optional ByVal Accode As String = Nothing,
 Optional ByVal Flag As String = Nothing,
 Optional ByVal EmpId As Integer = Nothing,
 Optional ByVal PureWt As Double = Nothing,
 Optional ByVal Advwtper As Double = Nothing,
 Optional ByVal BillCashCounterId As String = Nothing,
  Optional ByVal PSNO As String = Nothing
      )
        If Amount = 0 And GrsWt = 0 And PureWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,ADVFIXWTPER,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & BillDate & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ," & Math.Abs(PureWt) & "" 'PUREWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If
        If EmpId <> 0 Then
            strSql += " ," & EmpId & "" 'EMPID
        Else
            strSql += " ,0" 'EMPID
        End If
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Advwtper & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK1
        strSql += " ,'" & Accode & "'" 'ACCODE
        strSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
        strSql += " ,'" & Paymode & "'"
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

        strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += " (BATCHNO,PSNO,REMARK1,COSTID,PAN)VALUES"
        strSql += " ('" & Batchno & "','" & PSNO & "','','" & cnCostId & "','')"

        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub
    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer,
    ByVal tranMode As String,
    ByVal accode As String,
    ByVal amount As Double,
    ByVal pcs As Integer,
    ByVal grsWT As Double,
    ByVal netWT As Double,
    ByVal payMode As String,
    ByVal contra As String,
    ByVal Batchno As String,
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
    Optional ByVal chqCardNo As String = Nothing,
    Optional ByVal chqDate As String = Nothing,
    Optional ByVal chqCardId As Integer = Nothing,
    Optional ByVal chqCardRef As String = Nothing,
    Optional ByVal Remark1 As String = Nothing,
    Optional ByVal Remark2 As String = Nothing,
    Optional ByVal baltopay As Double = Nothing,
    Optional ByVal BillCashCounterId As String = Nothing,
    Optional ByVal VATEXM As String = Nothing
    )
        If amount = 0 Then Exit Sub

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,BALANCE,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " ,DISC_EMPID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & BillDate & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & baltopay & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
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
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = ""
        cmd = Nothing
    End Sub
    Private Sub txtRunno_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRunno_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And rbtSR.Checked = False And rbtGiftVoucher.Checked = False Then
            Dim RNO As String
            If cmbFromcostCentre.Text <> "ALL" And cmbFromcostCentre.Text <> "" Then
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbFromcostCentre.Text.ToString & "'"
                _CostId = objGPack.GetSqlValue(strSql, "COSTID", "")
            End If
            RNO = IIf(_CostId <> "", _CostId, cnCostId) + strCompanyId + txtRunno_MAN.Text.ToString.Replace(cnCostId + strCompanyId, "")
            strSql = " SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO ='" & RNO & "' HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) <> 0"
            If objGPack.GetSqlValue(strSql, , "0") = "0" Then
                MsgBox("This Refno Already Tally", MsgBoxStyle.Information)
                txtRunno_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtRunno_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRunno_MAN.KeyDown
        If rbtSR.Checked Then
            If e.KeyCode = Keys.Insert Then
                Dim SReturnDb As String
                Dim NOFRETDAYS As Integer = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "SALERETURNDAYS", "0"))
                strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpSRBillDate.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
                SReturnDb = objGPack.GetSqlValue(strSql)
                strSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..SYSDATABASES WHERE NAME = '" & SReturnDb & "'"
                If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then
                    MsgBox(SReturnDb & " Database not found", MsgBoxStyle.Information)
                    SReturnDb = cnStockDb
                    dtpSRBillDate.Select()
                    Exit Sub
                End If
                strSql = " SELECT TRANNO AS BILLNO,CONVERT(VARCHAR,TRANDATE,103)AS BILLDATE"
                strSql += vbCrLf + " ,ITEMID"
                strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
                strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
                strSql += vbCrLf + " ,GRSWT,AMOUNT,SNO,BATCHNO"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
                strSql += vbCrLf + " WHERE I.TRANDATE = '" & dtpSRBillDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND (ISNULL(REFNO,'') = '' OR REFNO='0') AND REFDATE IS NULL"
                If NOFRETDAYS > 0 Then
                    Dim mdate1 As Date = DateAdd(DateInterval.Day, NOFRETDAYS * (-1), BillDate)
                    strSql += vbCrLf + " AND  I.TRANDATE >= '" & mdate1.ToString("yyyy-MM-dd") & "'"
                End If
                strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')"
                If txtRunno_MAN.Text <> "" Then
                    strSql += vbCrLf + " AND I.TRANNO = " & Val(txtRunno_MAN.Text) & ""
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
                strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " ORDER BY I.TRANDATE,TRANNO,SNO"
                Dim row As DataRow = Nothing
                row = BrighttechPack.SearchDialog.Show_R("Search Reference No", strSql, cn)
                If Not row Is Nothing Then
                    With row
                        txtRunno_MAN.Text = .Item("BILLNO").ToString
                    End With
                End If
            End If
        ElseIf rbtGiftVoucher.Checked Then
            If e.KeyCode = Keys.Insert Then
                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
                strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
                strSql += vbCrLf + " AS"
                strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = " SELECT RUNNO REFNO"
                strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
                strSql += vbCrLf + " FROM"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strSql += vbCrLf + " )X"
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
                strSql += vbCrLf + " WHERE X.TRANTYPE='GV'"
                strSql += vbCrLf + " ORDER BY X.TRANTYPE"
                Dim row As DataRow = Nothing
                row = BrighttechPack.SearchDialog.Show_R("Search Reference No", strSql, cn, 9, , , , , , , False)
                If Not row Is Nothing Then
                    With row
                        For Each roAdv As DataRow In dtGridAdvance.Rows
                            If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                                MsgBox("Already Exist in this Ref No", MsgBoxStyle.Information)
                                txtRunno_MAN.Select()
                                txtRunno_MAN.SelectAll()
                                Exit Sub
                            End If
                        Next
                        txtRunno_MAN.Text = .Item("REFNO").ToString
                    End With
                End If
            End If
        Else
            If e.KeyCode = Keys.Insert Then
                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
                strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
                strSql += vbCrLf + " AS"
                strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
                strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
                strSql += vbCrLf + " FROM"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strSql += vbCrLf + " )X"
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
                strSql += vbCrLf + " ORDER BY X.TRANTYPE"
                Dim row As DataRow = Nothing
                row = BrighttechPack.SearchDialog.Show_R("Search Reference No", strSql, cn, 9, , , , , , , False)
                If Not row Is Nothing Then
                    With row
                        For Each roAdv As DataRow In dtGridAdvance.Rows
                            If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                                MsgBox("Already Exist in this Ref No", MsgBoxStyle.Information)
                                txtRunno_MAN.Select()
                                txtRunno_MAN.SelectAll()
                                Exit Sub
                            End If
                        Next
                        txtRunno_MAN.Text = .Item("REFNO").ToString
                    End With
                End If
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If cmbFromcostCentre.Text.Trim = "" Then MsgBox("From CostCentre Empty", MsgBoxStyle.Information) : cmbFromcostCentre.Focus() : Exit Sub
        If cmbTocostCentre.Text.Trim = "" Then MsgBox("To CostCentre Empty", MsgBoxStyle.Information) : cmbTocostCentre.Focus() : Exit Sub
        If rbtSR.Checked Then
            If txtRunno_MAN.Text.Trim = "" Then MsgBox("Please Enter the TranNo", MsgBoxStyle.Information) : Exit Sub
            For cnt As Integer = 0 To GridView.Rows.Count - 1
                If Val(txtRunno_MAN.Text) = Val(GridView.Rows(cnt).Cells("BILLNO").Value.ToString) Then
                    MsgBox("Already Loaded. ", MsgBoxStyle.Information)
                    txtRunno_MAN.Select()
                    txtRunno_MAN.SelectAll()
                    Exit Sub
                End If
            Next
            Dim SReturnDb As String
            Dim NOFRETDAYS As Integer = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "SALERETURNDAYS", "0"))
            strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpSRBillDate.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
            SReturnDb = objGPack.GetSqlValue(strSql)
            strSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..SYSDATABASES WHERE NAME = '" & SReturnDb & "'"
            If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then
                MsgBox(SReturnDb & " Database not found", MsgBoxStyle.Information)
                SReturnDb = cnStockDb
                dtpSRBillDate.Select()
                Exit Sub
            End If
            strSql = " SELECT TRANNO AS BILLNO,CONVERT(VARCHAR,TRANDATE,103)AS BILLDATE"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
            strSql += vbCrLf + " ,GRSWT,AMOUNT,SNO,BATCHNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " WHERE I.TRANDATE = '" & dtpSRBillDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND (ISNULL(REFNO,'') = '' OR REFNO='0') AND REFDATE IS NULL"
            If NOFRETDAYS > 0 Then
                Dim mdate1 As Date = DateAdd(DateInterval.Day, NOFRETDAYS * (-1), BillDate)
                strSql += vbCrLf + " AND  I.TRANDATE >= '" & mdate1.ToString("yyyy-MM-dd") & "'"
            End If
            strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')"
            strSql += vbCrLf + " AND I.TRANNO = " & Val(txtRunno_MAN.Text) & ""
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
            strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ORDER BY I.TRANDATE,TRANNO,SNO"
            dtitem = New DataTable
            Dim dtCol As DataColumn
            dtCol = New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = True
            dtitem.Columns.Add(dtCol)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitem)
            If Not dtitem.Rows.Count > 0 Then
                btnTransfer.Enabled = False
                MsgBox("TranNo Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim dtTemp As New DataTable
            If GridView.Rows.Count > 0 Then
                If dtitem.Rows.Count > 0 Then
                    dtTemp = GridView.DataSource
                    dtTemp.Merge(dtitem)
                End If
            Else
                dtTemp = dtitem.Copy
            End If
            GridView.DataSource = dtTemp
            With GridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End With
            GridView.Columns("CHECK").ReadOnly = False
            btnTransfer.Enabled = True
            txtRunno_MAN.SelectAll()
            txtRunno_MAN.Focus()
        ElseIf rbtGiftVoucher.Checked Then
            Dim RUNNO1 As String
            If cmbFromcostCentre.Text <> "ALL" And cmbFromcostCentre.Text <> "" Then
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbFromcostCentre.Text.ToString & "'"
                _CostId = objGPack.GetSqlValue(strSql, "COSTID", "")
            End If
            RUNNO1 = txtRunno_MAN.Text.ToString()
            dtitem = New DataTable
            Dim dtCol As DataColumn
            dtCol = New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = True
            dtitem.Columns.Add(dtCol)

            strSql = " SELECT RUNNO, 'GIFT VOUCHER'  AS TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN 'RECEIPT 'ELSE 'PAYMENT' END AS RECPAY"
            strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS AMOUNT"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END AS NETWT"
            strSql += vbCrLf + " ,X.COSTNAME AS COSTNAME,'" & cmbTocostCentre.Text & "' AS TO_COSTCENTER  "
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,C.COSTNAME,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID=C.COSTID "
            strSql += vbCrLf + " WHERE RUNNO='" & RUNNO1 & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " GROUP BY RUNNO,C.COSTNAME,O.COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,C.COSTNAME,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID=C.COSTID "
            strSql += vbCrLf + " WHERE RUNNO='" & RUNNO1 & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " GROUP BY RUNNO,C.COSTNAME,O.COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " ORDER BY X.TRANTYPE"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitem)

            If Not dtitem.Rows.Count > 0 Then
                btnTransfer.Enabled = False
                MsgBox("This Refno Already Tally or wrong Refno", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim dtTemp As New DataTable
            If GridView.Rows.Count > 0 Then
                If dtitem.Rows.Count > 0 Then
                    dtTemp = GridView.DataSource
                    For k As Integer = 0 To dtTemp.Rows.Count - 1
                        If dtitem.Select("RUNNO='" & dtTemp.Rows(k)("RUNNO").ToString & "'").Length > 0 Then
                            For Each rw As DataRow In dtitem.Select("RUNNO='" & dtTemp.Rows(k)("RUNNO").ToString & "'")
                                rw.Delete()
                                dtitem.AcceptChanges()
                            Next
                        End If
                    Next
                    dtTemp.Merge(dtitem)
                End If
            Else
                dtTemp = dtitem.Copy
            End If
            GridView.DataSource = dtTemp
            With GridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End With
            GridView.Columns("CHECK").ReadOnly = False
            btnTransfer.Enabled = True
            txtRunno_MAN.SelectAll()
            txtRunno_MAN.Focus()

        Else
            Dim RUNNO1 As String
            If cmbFromcostCentre.Text <> "ALL" And cmbFromcostCentre.Text <> "" Then
                strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbFromcostCentre.Text.ToString & "'"
                _CostId = objGPack.GetSqlValue(strSql, "COSTID", "")
            End If
            RUNNO1 = IIf(_CostId <> "", _CostId, cnCostId) + strCompanyId + txtRunno_MAN.Text.ToString().Replace(cnCostId + strCompanyId, "")
            dtitem = New DataTable
            Dim dtCol As DataColumn
            dtCol = New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = True
            dtitem.Columns.Add(dtCol)

            strSql = " SELECT RUNNO, CASE WHEN SUBSTRING(RUNNO,6,1) IN ('A','B') THEN 'ADVANCE' WHEN SUBSTRING(RUNNO,6,1) ='O' THEN 'ORDER' ELSE 'CREDIT' END AS TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN 'RECEIPT 'ELSE 'PAYMENT' END AS RECPAY"
            strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS AMOUNT"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END AS NETWT"
            strSql += vbCrLf + " ,X.COSTNAME AS COSTNAME,'" & cmbTocostCentre.Text & "' AS TO_COSTCENTER  "
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,C.COSTNAME,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID=C.COSTID "
            strSql += vbCrLf + " WHERE RUNNO='" & RUNNO1 & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " GROUP BY RUNNO,C.COSTNAME,O.COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & cnCostId & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,C.COSTNAME,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID=C.COSTID "
            strSql += vbCrLf + " WHERE RUNNO='" & RUNNO1 & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " GROUP BY RUNNO,C.COSTNAME,O.COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " ORDER BY X.TRANTYPE"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtitem)

            If Not dtitem.Rows.Count > 0 Then
                btnTransfer.Enabled = False
                MsgBox("This Refno Already Tally or wrong Refno", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim dtTemp As New DataTable
            If GridView.Rows.Count > 0 Then
                If dtitem.Rows.Count > 0 Then
                    dtTemp = GridView.DataSource
                    For k As Integer = 0 To dtTemp.Rows.Count - 1
                        If dtitem.Select("RUNNO='" & dtTemp.Rows(k)("RUNNO").ToString & "'").Length > 0 Then
                            For Each rw As DataRow In dtitem.Select("RUNNO='" & dtTemp.Rows(k)("RUNNO").ToString & "'")
                                rw.Delete()
                                dtitem.AcceptChanges()
                            Next
                        End If
                    Next
                    dtTemp.Merge(dtitem)
                End If
            Else
                dtTemp = dtitem.Copy
            End If
            GridView.DataSource = dtTemp
            With GridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End With
            GridView.Columns("CHECK").ReadOnly = False
            btnTransfer.Enabled = True
            txtRunno_MAN.SelectAll()
            txtRunno_MAN.Focus()
        End If
    End Sub

    Private Sub rbtAdvance_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAdvance.CheckedChanged
        GridView.DataSource = Nothing
        GridView.Refresh()
        If rbtAdvance.Checked Then
            dtpSRBillDate.Enabled = False
            'txtRunno_MAN.Location = New System.Drawing.Point(114, 174)
        Else
            dtpSRBillDate.Enabled = True
            'dtpSRBillDate.Location = New System.Drawing.Point(114, 74)
            'txtRunno_MAN.Location = New System.Drawing.Point(218, 74)
        End If
    End Sub

    Private Sub rbtGiftVoucher_CheckedChanged(sender As Object, e As EventArgs) Handles rbtGiftVoucher.CheckedChanged
        GridView.DataSource = Nothing
        GridView.Refresh()
        If rbtGiftVoucher.Checked Then
            dtpSRBillDate.Enabled = False
        Else
            dtpSRBillDate.Enabled = True
        End If
    End Sub
End Class