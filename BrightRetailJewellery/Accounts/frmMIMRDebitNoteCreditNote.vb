Imports System.Data.OleDb
Public Class frmMIMRDebitNoteCreditNote
#Region "Variable"
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim _Tran As OleDbTransaction = Nothing
    Dim dt As New DataTable
    Dim Tranno As Integer = 0
    Dim Batchno As String = ""
#End Region

#Region " Constructor"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        dtpTrandate.Value = GetServerDate()

        strsql = " SELECT ACCODE,ACNAME FROM ( "
        strsql += vbCrLf + " SELECT '' ACCODE, '' ACNAME,0 RESULT "
        strsql += vbCrLf + " UNION ALL "
        strsql += vbCrLf + " SELECT ACCODE,ACNAME,1 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strsql += vbCrLf + " )X ORDER BY RESULT,ACNAME "
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbAcname_OWN.DataSource = Nothing
            cmbAcname_OWN.DataSource = dt
            cmbAcname_OWN.ValueMember = "ACCODE"
            cmbAcname_OWN.DisplayMember = "ACNAME"
        End If
        dt = New DataTable
        strsql = ""
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMRDebitNoteCreditNote_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmMIMRDebitNoteCreditNote_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        End If
    End Sub
#End Region

#Region " User Define"
    Private Sub INSERTTDS(ByVal TDSAmt As Double, ByVal accode As String _
                          , ByVal TdsContra As String, ByVal payMode As String _
                          , ByVal amt As Double _
                          , ByVal TDSPerNew As String, ByVal TTdsAccode As String _
                          , ByVal tNo As Integer, ByVal TdsRemark As String)
        If TDSAmt > 0 Then
            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strsql += " ("
            strsql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            strsql += " )"
            strsql += " VALUES("
            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            strsql += " ,'" & accode & "'"
            strsql += " ,'" & TdsContra & "'"
            strsql += " ," & Tranno & "" 'TRANNO
            strsql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
            strsql += " ,'" & payMode & "'"
            strsql += " ,'" & Batchno & "'" 'BATCHNO
            strsql += " ,'TD'" 'TAXID
            strsql += " ," & amt & "" 'AMOUNT
            strsql += " ," & TDSPerNew & "" 'TAXPER 'Replace TdsPer to TDSPerNew
            strsql += " ," & TDSAmt
            strsql += " ,'TD'"
            strsql += " ,1" 'TSNO
            strsql += " ,'" & cnCostId & "'"
            strsql += " ,'" & strCompanyId & "'"
            strsql += " )"
            ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnCostId)
            If TTdsAccode <> "" Then
                accode = TTdsAccode
            Else
                accode = "TDS"
            End If
        End If
    End Sub
    Private Sub InsertIntoAccTran _
        (ByVal tNo As Integer,
        ByVal tranMode As String,
        ByVal accode As String,
        ByVal amount As Double,
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
        Optional ByVal VATEXM As String = Nothing,
        Optional ByVal Transfered As String = Nothing,
        Optional ByVal Wt_EntOrder As String = Nothing
        )
        If amount = 0 Then Exit Sub

        strsql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strsql += " ("
        strsql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strsql += " ,AMOUNT,BALANCE"
        strsql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strsql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strsql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strsql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strsql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strsql += " ,DISC_EMPID,TRANSFERED,WT_ENTORDER"
        strsql += " )"
        strsql += " VALUES"
        strsql += " ("
        strsql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strsql += " ," & tNo & "" 'TRANNO 
        strsql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE'GetServerDate(tran)
        strsql += " ,'" & tranMode & "'" 'TRANMODE
        strsql += " ,'" & accode & "'" 'ACCODE
        strsql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strsql += " ," & baltopay & "" 'AMOUNT        
        strsql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strsql += " ,NULL" 'REFDATE
        Else
            strsql += " ,'" & refDate & "'" 'REFDATE
        End If
        strsql += " ,'" & payMode & "'" 'PAYMODE
        strsql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strsql += " ," & chqCardId & "" 'CARDID
        strsql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strsql += " ,NULL" 'CHQDATE
        Else
            strsql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strsql += " ,''" 'BRSFLAG
        strsql += " ,NULL" 'RELIASEDATE
        strsql += " ,'A'" 'FROMFLAG
        strsql += " ,'" & Remark1 & "'" 'REMARK1
        strsql += " ,'" & Remark2 & "'" 'REMARK2
        strsql += " ,'" & contra & "'" 'CONTRA
        strsql += " ,'" & Batchno & "'" 'BATCHNO
        strsql += " ,'" & userId & "'" 'USERID
        strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += " ,'" & systemId & "'" 'SYSTEMID
        strsql += " ,'" & BillCashCounterId & "'" 'CASHID
        strsql += " ,'" & cnCostId & "'" 'COSTID
        strsql += " ,'" & VERSION & "'" 'APPVER
        strsql += " ,'" & strCompanyId & "'" 'COMPANYID
        strsql += " ,0" ' DISC_EMPID
        strsql += " ,'" & Transfered & "'" 'TRANSFERED
        strsql += " ,'" & Val(Wt_EntOrder) & "'" 'TRANSFERED
        strsql += " )"
        ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnCostId)
        strsql = ""
        cmd = Nothing
    End Sub
    Private Sub INSERTTAXTRAN(ByVal ISSSNO As String, ByVal Amt As Double, ByVal accode As String _
                          , ByVal TdsContra As String, ByVal payMode As String _
                          , ByVal TotalAmt As Double _
                          , ByVal TDSPerNew As String _
                          , ByVal Taxid As String, ByVal taxType As String, ByVal Tsno As Integer)
        If Amt > 0 Then
            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strsql += " ("
            strsql += " SNO,ISSSNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            strsql += " )"
            strsql += " VALUES("
            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            strsql += " ,'" & ISSSNO & "' "
            strsql += " ,'" & accode & "'"
            strsql += " ,'" & TdsContra & "'"
            strsql += " ," & Tranno & "" 'TRANNO
            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strsql += " ,'" & payMode & "'"
            strsql += " ,'" & Batchno & "'" 'BATCHNO
            strsql += " ,'" & Taxid & "'" 'TAXID
            strsql += " ," & TotalAmt & "" 'AMOUNT
            strsql += " ," & TDSPerNew & "" 'TAXPER 'Replace TdsPer to TDSPerNew
            strsql += " ," & Amt
            strsql += " ,'" & taxType & "'" 'TAXTYPE
            strsql += " ," & Tsno & "" 'TSNO
            strsql += " ,'" & cnCostId & "'"
            strsql += " ,'" & strCompanyId & "'"
            strsql += " )"
            ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnCostId)
        End If
    End Sub
#End Region

#Region " Button Event"
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If Val(txtCDNet.Text) <> 0 Then
            If MsgBox("Are you sure Do you want Generate ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                If lblRateFixedAccode.Text = "" Then
                    Exit Sub
                End If

                Dim chkRateAccode As Integer = 0
                strsql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & lblRateFixedAccode.Text & "'"
                chkRateAccode = Val(objGPack.GetSqlValue(strsql).ToString)
                If chkRateAccode = 0 Then
                    MsgBox("Accode should not empty", MsgBoxStyle.Information)
                    Exit Sub
                End If

                If Val(txtCDCGst.Text) <> 0 And Val(txtCDSGst.Text) <> 0 And Val(txtCDIGst.Text) <> 0 Then
                    MsgBox("Invalid Type", MsgBoxStyle.Information)
                    txtInvoiceNo.Focus()
                    txtInvoiceNo.SelectAll()
                    Exit Sub
                End If
                If Val(txtCuNet.Text) = 0 Then
                    MsgBox("Current value should not empty", MsgBoxStyle.Information)
                    txtInvoiceNo.Focus()
                    txtInvoiceNo.SelectAll()
                    Exit Sub
                End If
                Dim drGstRegAlready As DataRow = Nothing
                strsql = " SELECT COUNT(*)CNT,TRANNO,TRANDATE,TRANTYPE "
                strsql += vbCrLf + " FROM " & cnStockDb & "..GSTREGISTER "
                strsql += vbCrLf + " WHERE ACCODE = '" & cmbAcname_OWN.SelectedValue.ToString & "' "
                strsql += vbCrLf + " AND REFNO = '" & Val(txtInvoiceNo.Text) & "' "
                strsql += vbCrLf + " AND REFDATE = '" & Format(dtpTrandate.Value.Date, "yyyy-MM-dd") & "' "
                strsql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strsql += vbCrLf + " AND ISNULL(TRANTYPE,'') IN ('PD','PC') "
                strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,TRANTYPE"
                drGstRegAlready = GetSqlRow(strsql, cn)
                If Not drGstRegAlready Is Nothing Then
                    If Val(drGstRegAlready.Item("CNT").ToString) > 0 Then
                        MsgBox("Already voucher Generate " & vbCrLf & drGstRegAlready.Item("TRANNO") & vbCrLf & Format(drGstRegAlready.Item("TRANDATE"), "dd-MM-yyyy") & vbCrLf & drGstRegAlready.Item("TRANTYPE"), MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
                Try
                    Me.Cursor = Cursors.WaitCursor
                    btnGenerate.Enabled = False
                    tran = Nothing
                    tran = cn.BeginTransaction
                    Dim Sno As String = GetNewSno(TranSnoType.GSTREGISTERCODE, tran)
                    Batchno = ""
                    Batchno = GetNewBatchno(cnCostId, GetServerDate(tran), tran)
                    If Batchno.Trim = "" Then
                        tran.Rollback()
                        tran.Dispose()
                        tran = Nothing
                        MsgBox("Empty Batchno should Not allowed ", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    Dim gstTrantype As String = ""
                    If lblCNoteOrDNote.Text = "DEBIT NOTE" Then
                        gstTrantype = "PD"
                        Tranno = Tranno = GetBillNoValueGstRegister("GEN-GSTDNREGNO", tran)
                    ElseIf lblCNoteOrDNote.Text = "CREDIT NOTE" Then
                        gstTrantype = "PC"
                        Tranno = GetBillNoValueGstRegister("GEN-GSTCNREGNO", tran)
                    End If
                    If gstTrantype.Trim = "" Then
                        tran.Rollback()
                        tran.Dispose()
                        tran = Nothing
                        MsgBox("Empty Trantype should Not allowed ", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    If Tranno = 0 Then
                        tran.Rollback()
                        tran.Dispose()
                        tran = Nothing
                        MsgBox("Empty Tranno should Not allowed ", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    Dim stateid As String = ""
                    Dim description As String = lblRateFixedAcName.Text.Trim '"GENERAL EXPENSES"
                    Dim accode As String = cmbAcname_OWN.SelectedValue.ToString
                    strsql = " SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & accode & "'"
                    stateid = objGPack.GetSqlValue(strsql,,, tran).ToString
                    Dim Amt As Double = Math.Abs(Val(txtCDValue.Text))
                    Dim cgst As Double = Math.Abs(Val(txtCDCGst.Text))
                    Dim sgst As Double = Math.Abs(Val(txtCDSGst.Text))
                    Dim igst As Double = Math.Abs(Val(txtCDIGst.Text))
                    Dim tdsval As Double = Math.Abs(Val(txtcdTDS.Text))
                    Dim cgstper As Double = Val(lblCGSTPer.Text)
                    Dim sgstper As Double = Val(lblSGSTPer.Text)
                    Dim igstper As Double = Val(lblIGSTPer.Text)
                    Dim tdsper As Double = Math.Abs(Val(lblTDSPer.Text))
                    strsql = " INSERT INTO " & cnStockDb & "..GSTREGISTER(SNO,BATCHNO,TRANDATE,TRANNO,DESCRIPTION,HSN,PCS,WEIGHT,RATE,AMOUNT,"
                    strsql += vbCrLf + " TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,STATEID "
                    strsql += vbCrLf + " ,ACCODE,SUPNAME,REFNO,REFDATE,GSTCLAIM,TRANTYPE,COMPANYID,COSTID,TDS_PER,TDS,TCS_PER,TCS"
                    strsql += vbCrLf + " ,UPTIME"
                    strsql += vbCrLf + " ,USERID"
                    strsql += vbCrLf + " ,SYSTEMID,REMARKS"
                    strsql += vbCrLf + " )"
                    strsql += vbCrLf + " VALUES "
                    strsql += vbCrLf + "  ( "
                    strsql += vbCrLf + " '" & Sno & "'"
                    strsql += vbCrLf + " ,'" & Batchno & "'"
                    strsql += vbCrLf + " ,'" & GetServerDate(tran) & "'"
                    strsql += vbCrLf + " , '" & Tranno & "' " 'txtTranno.Text
                    strsql += vbCrLf + " ,'" & description & "'"
                    strsql += vbCrLf + " ,'000000'" 'HSNCODE
                    strsql += vbCrLf + " ,'0'" 'PCS
                    strsql += vbCrLf + " ,'0'" 'WEIGHT
                    strsql += vbCrLf + " ,'" & Val(txtNewRateInclusive_RATE.Text) & "'"
                    strsql += vbCrLf + " ,'" & Amt & "'"
                    strsql += vbCrLf + " ,'" & Val(lblCGSTPer.Text) + Val(lblSGSTPer.Text) + Val(lblIGSTPer.Text) & "'" 'TAXPER
                    strsql += vbCrLf + " ,'" & Val(cgst + sgst + igst) & "'"
                    strsql += vbCrLf + " ,'" & Val(lblSGSTPer.Text) & "'" 'SGSTPER
                    strsql += vbCrLf + " ,'" & sgst & "' "
                    strsql += vbCrLf + " ,'" & Val(lblCGSTPer.Text) & "'" 'CGSTPER
                    strsql += vbCrLf + " ,'" & cgst & "'"
                    strsql += vbCrLf + " ,'" & Val(lblIGSTPer.Text) & "'" 'IGSTPER
                    strsql += vbCrLf + " ,'" & igst & "'"
                    strsql += vbCrLf + " ,'" & stateid & "'"
                    strsql += vbCrLf + " ,'" & accode & "'"
                    strsql += vbCrLf + " ,''" 'SUPNAME
                    strsql += vbCrLf + " ,'" & txtInvoiceNo.Text.ToString() & "'" ' REFNO
                    strsql += vbCrLf + " ,'" & dtpTrandate.Value.Date.ToString("yyyy-MM-dd") & "'" 'REFDATE
                    strsql += vbCrLf + " ,'Y' " 'GST CLAIM
                    strsql += vbCrLf + " , '" & gstTrantype & "'"
                    strsql += vbCrLf + " , '" & strCompanyId & "'"
                    strsql += vbCrLf + " , '" & cnCostId & "'"
                    strsql += vbCrLf + " ,'" & Val(tdsper) & "'" 'TDSPER
                    strsql += vbCrLf + " ,'" & Val(tdsval) & "'" 'TDSVAL
                    strsql += vbCrLf + " ,'0'" 'TCSPER
                    strsql += vbCrLf + " ,'0'" 'TCS
                    strsql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'"
                    strsql += vbCrLf + " ,'" & userId & "'"
                    strsql += vbCrLf + " ,'" & systemId & "'"
                    strsql += vbCrLf + " ,'MIMR'"
                    strsql += vbCrLf + ")"
                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnHOCostId)

                    INSERTTAXTRAN(Sno, Val(cgst), accode, "", gstTrantype, Val(Amt), Val(cgstper), "CG", gstTrantype, 1)
                    INSERTTAXTRAN(Sno, Val(sgst), accode, "", gstTrantype, Val(Amt), Val(sgstper), "CG", gstTrantype, 2)
                    INSERTTAXTRAN(Sno, Val(igst), accode, "", gstTrantype, Val(Amt), Val(igstper), "IG", gstTrantype, 3)

                    Dim crn As String = ""
                    Dim drn As String = ""
                    If gstTrantype = "PD" Then 'DN
                        crn = "D"
                        drn = "C"
                    Else 'PE,JE,CN,RI
                        crn = "C"
                        drn = "D"
                    End If

                    Dim drAccode As String = ""
                    Dim Trfamount As Double = 0
                    'strsql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & description & "' AND ACTYPE = 'E'"
                    drAccode = lblRateFixedAccode.Text 'objGPack.GetSqlValue(strsql, "",, tran)
                    'CREDIT
                    Trfamount = Amt + cgst + sgst + igst - tdsval
                    InsertIntoAccTran(Tranno, crn, accode, Trfamount, gstTrantype, drAccode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    'DEBIT
                    InsertIntoAccTran(Tranno, drn, drAccode, Amt, gstTrantype, accode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    InsertIntoAccTran(Tranno, drn, "CGST", cgst, gstTrantype, accode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    InsertIntoAccTran(Tranno, drn, "SGST", sgst, gstTrantype, accode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    InsertIntoAccTran(Tranno, drn, "IGST", igst, gstTrantype, accode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    'TDS
                    InsertIntoAccTran(Tranno, crn, "TDS", tdsval, gstTrantype, accode, Batchno, txtInvoiceNo.Text.ToString(), dtpTrandate.Value.Date.ToString("yyyy-MM-dd"), , , , , "", "", , , , "", "")
                    INSERTTDS(Val(tdsval), "TDS", accode, gstTrantype, Amt + cgst + sgst + igst, tdsper, "TDS", Tranno, "")
                    Dim debCrNotTally As String = ""
                    Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
                    If Math.Abs(balAmt) > 0 Then
                        tran.Rollback()
                        tran.Dispose()
                        tran = Nothing
                        MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Saved " & vbCrLf & Tranno, MsgBoxStyle.Information)
                    Me.Close()
                Catch ex As Exception
                    If Not tran Is Nothing Then
                        tran.Rollback()
                        tran = Nothing
                        MessageBox.Show(ex.ToString)
                    Else
                        MessageBox.Show(ex.ToString)
                    End If
                Finally
                    btnGenerate.Enabled = True
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtInvoiceNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            strsql = " SELECT DISTINCT ISNULL(A.ACCODE,'')ACCODE,B.ACNAME "
            strsql += vbCrLf + " ,ISNULL(A.CANCEL,'')CANCEL "
            strsql += vbCrLf + " ,ISNULL(A.TRANSTATUS,'')TRANSTATUS "
            strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS A "
            strsql += vbCrLf + " ," & cnAdminDb & "..ACHEAD AS B  "
            strsql += vbCrLf + " WHERE A.ACCODE= B.ACCODE "
            strsql += vbCrLf + " And A.TRANDATE ='" & Format(dtpTrandate.Value.Date, "yyyy-MM-dd") & "'  "
            strsql += vbCrLf + " AND A.TRANNO ='" & Val(txtInvoiceNo.Text) & "' "
            strsql += vbCrLf + " AND A.TRANTYPE = 'RPU'"
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("CANCEL").ToString = "Y" Then
                    MsgBox("This bill already cancelled", MsgBoxStyle.Information)
                    txtInvoiceNo.Focus()
                    txtInvoiceNo.SelectAll()
                    Exit Sub
                End If
                cmbAcname_OWN.Text = dt.Rows(0).Item("ACNAME").ToString
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
            End If
        End If
    End Sub
#End Region
End Class