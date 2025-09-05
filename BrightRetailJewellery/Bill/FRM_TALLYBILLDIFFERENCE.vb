Imports System.Data.OleDb

Public Class FRM_TALLYBILLDIFFERENCE
    Dim StrSql As String
    Dim GEN_GENSEPRETURN As Boolean = False
    Private TRANNO As Integer = 0
    Dim dt, dtpay As New DataTable
    Dim da As New OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim tranflag As String = ""
    Dim trandate As DateTime
    Dim batchno As String = ""
    Dim OldAccode As String
    Dim amt As Decimal
    Dim acdt As New DataTable
    Public Sub New(ByVal tranflagg As String, ByVal tranno As String, ByVal CREDIT As Decimal, ByVal DEBIT As Decimal, ByVal accode As String _
    , ByVal batchno As String, ByVal trandate As DateTime, ByVal diffamt As Decimal, Optional ByVal paymode As String = Nothing, Optional ByVal SNo As String = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' objGPack.Validator_Object(Me)
        Me.Name = "Tally Billdifference"
        Me.tranflag = tranflagg
        Me.trandate = trandate
        Me.batchno = batchno
        Me.amt = IIf(CREDIT <> 0, CREDIT, DEBIT)
        If diffamt < 0 Then cmbmode.Text = "Credit" Else cmbmode.Text = "Debit"
        txttranno.Text = tranno
        txtSno.Text = SNo
        txtamt_NUM.Text = Math.Abs(Val(IIf(tranflag = "I", diffamt, amt)))


        StrSql = " SELECT DISTINCT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbacname.DataSource = Nothing
            cmbacname.DataSource = dt
            cmbacname.DisplayMember = "ACNAME"
            cmbacname.ValueMember = "ACCODE"
            cmbacname.SelectedValue = accode
        End If
        OldAccode = accode
        'StrSql = " SELECT DISTINCT PAYMODE FROM " & cnStockDb & "..ACCTRAN ORDER BY PAYMODE "
        'dtpay = New DataTable
        'da = New OleDbDataAdapter(StrSql, cn)
        'da.Fill(dtpay)
        'If dtpay.Rows.Count > 0 Then
        '    cmbpaymode.DataSource = Nothing
        '    cmbpaymode.DataSource = dtpay
        '    cmbpaymode.DisplayMember = "paymode"
        'End If
        funcLoadCombo()
        txtamt_NUM.Enabled = True
        cmbpaymode.Text = GetPaymode(paymode)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Save()
    End Sub

    private Sub Save()
        Try
            StrSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & batchno & "' and tranno='" & txttranno.Text.ToString & "'"
            StrSql += vbCrLf + " AND TRANDATE='" & trandate.ToString("yyyy/MM/dd") & "' AND AMOUNT='" & amt & "' "
            acdt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(acdt)
            If acdt.Rows.Count < 0 Then Exit Sub
            tran = Nothing
            tran = cn.BeginTransaction()
            With acdt.Rows(0)
                If tranflag = "I" Then
                    StrSql = " INSERT INTO " & cnStockDb & ".." & "ACCTRAN" & ""
                    StrSql += " ("
                    StrSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
                    StrSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                    StrSql += " ,REFNO,PAYMODE,CHQCARDNO"
                    StrSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
                    StrSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
                    StrSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                    StrSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,APPVER,COMPANYID,WT_ENTORDER"
                    ',TDSCATID,TDSPER,TDSAMOUNT
                    StrSql += " ,FLAG)"
                    StrSql += " VALUES"
                    StrSql += " ("
                    StrSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
                    StrSql += " ," & txttranno.Text.ToString & "" 'TRANNO 
                    StrSql += " ,'" & trandate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    StrSql += " ,'" & IIf(cmbmode.Text = "Credit", "C", "D") & "'" 'TRANMODE
                    StrSql += " ,'" & cmbacname.SelectedValue.ToString & "'" 'ACCODE
                    StrSql += " ,'" & IIf(IsDBNull(.Item("SACCODE").ToString), "", .Item("SACCODE").ToString) & "'" 'ACCODE
                    StrSql += " ," & Math.Abs(Val(txtamt_NUM.Text.ToString)) & "" 'AMOUNT
                    StrSql += " ," & Math.Abs(Val(.Item("PCS").ToString)) & "" 'PCS
                    StrSql += " ," & Math.Abs(Val(.Item("GRSWT").ToString)) & "" 'GRSWT
                    StrSql += " ," & Math.Abs(Val(.Item("NETWT").ToString)) & "" 'NETWT
                    StrSql += " ,'" & IIf(IsDBNull(.Item("REFNO").ToString), "", .Item("REFNO").ToString) & "'" 'REFNO
                    StrSql += " ,'" & cmbpaymode.SelectedValue.ToString & "'" 'PAYMODE
                    StrSql += " ,''" 'CHQCARDNO
                    StrSql += " ,''" 'CARDID
                    StrSql += " ,''" 'CHQCARDREF
                    StrSql += " ,NULL" 'CHQDATE
                    StrSql += " ,''" 'BRSFLAG
                    StrSql += " ,NULL" 'RELIASEDATE
                    StrSql += " ,'" & IIf(IsDBNull(.Item("FROMFLAG").ToString), "", .Item("FROMFLAG").ToString) & "'" 'FROMFLAG
                    StrSql += " ,'" & IIf(IsDBNull(.Item("REMARK1").ToString), "", .Item("REMARK1").ToString) & "'" 'REMARK1
                    StrSql += " ,'" & IIf(IsDBNull(.Item("REMARK2").ToString), "", .Item("REMARK2").ToString) & "'" 'REMARK2
                    StrSql += " ,'" & IIf(IsDBNull(.Item("CONTRA").ToString), "", .Item("CONTRA").ToString) & "'" 'CONTRA
                    StrSql += " ,'" & batchno & "'" 'BATCHNO
                    StrSql += " ,'" & userId & "'" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & .Item("SYSTEMID").ToString & "'" 'SYSTEMID
                    StrSql += " ,'" & IIf(IsDBNull(.Item("CASHID").ToString), "", .Item("CASHID").ToString) & "'" 'CASHID
                    StrSql += " ,'" & IIf(IsDBNull(.Item("COSTID").ToString), "", .Item("COSTID").ToString) & "'" 'COSTID
                    StrSql += " ,'" & IIf(IsDBNull(.Item("VATEXM").ToString), "", .Item("VATEXM").ToString) & "'" 'VATEXM
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,''" 'WT_ENTORDER
                    StrSql += " ,'" & IIf(IsDBNull(.Item("FROMFLAG").ToString), "", .Item("FROMFLAG").ToString) & "'" 'FLAG
                    StrSql += " )"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, .Item("COSTID").ToString)
                ElseIf tranflag = "U" Then
                    StrSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                    StrSql += vbCrLf + " ACCODE='" & cmbacname.SelectedValue.ToString & "'"
                    StrSql += vbCrLf + " ,AMOUNT=" & Val(txtamt_NUM.Text.ToString) & ""
                    StrSql += vbCrLf + " ,TRANMODE='" & IIf(cmbmode.Text = "Credit", "C", "D") & "'"
                    StrSql += vbCrLf + " ,PAYMODE='" & cmbpaymode.SelectedValue.ToString & "'"
                    StrSql += vbCrLf + " WHERE SNO='" & txtSno.Text.ToString & "'"
                    StrSql += vbCrLf + " AND BATCHNO='" & batchno & "' AND TRANNO='" & txttranno.Text.ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, .Item("COSTID").ToString)
                    StrSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                    StrSql += vbCrLf + " CONTRA='" & cmbacname.SelectedValue.ToString & "'"
                    StrSql += vbCrLf + " WHERE CONTRA='" & OldAccode & "'"
                    StrSql += vbCrLf + " AND BATCHNO='" & batchno & "' AND TRANNO='" & txttranno.Text.ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, .Item("COSTID").ToString)

                    StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET"
                    StrSql += vbCrLf + " ACCODE='" & cmbacname.SelectedValue.ToString & "'"
                    StrSql += vbCrLf + " WHERE ACCODE='" & OldAccode & "'"
                    StrSql += vbCrLf + " AND BATCHNO='" & batchno & "' AND TRANNO='" & txttranno.Text.ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, .Item("COSTID").ToString)
                End If

            End With
            StrSql = ""
            tran.Commit()
            tran = Nothing
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            tran.Rollback()
            tran.Dispose()
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            MsgBox(ex.Message)
        Finally
            btnCancel_Click(Me, New EventArgs)
        End Try
    End Sub

    Private Sub FRM_TALLYBILLDIFFERENCE_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter And cmbmode.Focused = False And cmbpaymode.Focused = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub cmbmode_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbmode.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmbpaymode.Focus()
            cmbpaymode.SelectAll()
        End If
    End Sub

    Private Sub cmbpaymode_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbpaymode.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOk.Focus()
        End If
    End Sub
    Private Function GetPaymode(ByVal pay As String)
        Select Case pay
            Case "JE"
                Return "JOURNAL ENTRY"
            Case "AA"
                Return "ADVANCE ADJUSTED"
            Case "AR"
                Return "ADVANCE RECEIPTS"
            Case "AP"
                Return "ADVANCE PAYMENTS"
            Case "SA"
                Return "SALES"
            Case "SR"
                Return "SALESRETURN"
            Case "AI"
                Return "APPROVAL ISSUE"
            Case "CA"
                Return "CASH ON HAND"
            Case "CR"
                Return "PAYMENT ENTRY"
            Case "CP"
                Return "CASH PAID"
            Case "PU"
                Return "PURCHASE"
            Case "TI"
                Return "TRANSFER"
            Case "MI"
                Return "MISCISSUE"
            Case "SV"
                Return "SALES VAT"
            Case "PV"
                Return "PURCHASE VAT"
            Case "OR"
                Return "ORDER RECEIPT"
            Case "DI"
                Return "DISCOUNT"
            Case "RO"
                Return "ROUND OFF"
            Case "RV"
                Return "RETURN VAT"
            Case "SS"
                Return "CHIT CARD"
            Case "CC"
                Return "CREDIT CARD"
            Case "CD"
                Return "CHIT DEDUCTION"
            Case "CB"
                Return "CHIT BONUS"
            Case "DU"
                Return "TOBE RECEIPTS"
            Case "DR"
                Return "CREDIT RECEIPTS"
            Case "HC"
                Return "HANDLING CHARGES"
            Case "MR"
                Return "OTHER RECEIPTS"
            Case "MP"
                Return "OTHER PAYMENTS"
            Case "DN"
                Return "DEBIT NOTE"
            Case "CN"
                Return "CREDIT NOTE"
            Case "CT"
                Return "CHIT POSTING"
            Case "CH"
                Return "CHEQUE ENTRY"
            Case "TR"
                Return "MATERIAL RECEIPTS"
        End Select
    End Function
    Private Sub funcLoadCombo()
        Dim dtPay As New DataTable
        dtPay.Columns.Add("PAYMODE", Type.GetType("System.String"))
        dtPay.Columns.Add("PAY", Type.GetType("System.String"))
        Dim Paymode(32) As String
        Dim Pay(32) As String
        Paymode(0) = "SALES" : Pay(0) = "SA"
        Paymode(1) = "SALES RETURN" : Pay(1) = "SR"
        Paymode(2) = "ADVANCE ADJUSTED" : Pay(2) = "AA"
        Paymode(3) = "ADVANCE RECEIPTS" : Pay(3) = "AR"
        Paymode(4) = "ADVANCE PAYMENTS" : Pay(4) = "AP"
        Paymode(5) = "APPROVAL ISSUE" : Pay(5) = "AI"
        Paymode(6) = "CASH ON HAND" : Pay(6) = "CA"
        Paymode(7) = "PAYMENT ENTRY" : Pay(7) = "CR"
        Paymode(8) = "JOURNAL ENTRY" : Pay(8) = "JE"
        Paymode(9) = "CASH PAID" : Pay(9) = "CP"
        Paymode(10) = "PURCHASE" : Pay(10) = "PU"
        Paymode(11) = "TRANSFER" : Pay(11) = "TI"
        Paymode(12) = "MISCISSUE" : Pay(12) = "MI"
        Paymode(13) = "SALESVAT" : Pay(13) = "SV"
        Paymode(14) = "PURCHASEVAT" : Pay(14) = "PV"
        Paymode(15) = "ORDER RECEIPT" : Pay(15) = "OR"
        Paymode(16) = "DISCOUNT" : Pay(16) = "DI"
        Paymode(17) = "ROUND OFF" : Pay(17) = "RO"
        Paymode(18) = "RETURNVAT" : Pay(18) = "RV"
        Paymode(19) = "CHIT CARD" : Pay(19) = "SS"
        Paymode(20) = "CREDIT CARD" : Pay(20) = "CC"
        Paymode(21) = "CHIT DEDUCTION" : Pay(21) = "CD"
        Paymode(22) = "CHIT BONUS" : Pay(22) = "CB"
        Paymode(23) = "CREDIT RECEIPTS" : Pay(23) = "DR"
        Paymode(24) = "TOBE RECEIPTS" : Pay(24) = "DU"
        Paymode(25) = "HANDLING CHARGES" : Pay(25) = "HC"
        Paymode(26) = "OTHER RECEIPTS" : Pay(26) = "MR"
        Paymode(27) = "OTHER PAYMENTS" : Pay(27) = "MP"
        Paymode(28) = "CREDIT NOTE" : Pay(28) = "CN"
        Paymode(29) = "DEBIT NOTE" : Pay(29) = "DN"
        Paymode(30) = "CHIT POSTING" : Pay(30) = "CT"
        Paymode(31) = "MATERIAL RECEIPTS" : Pay(31) = "TR"
        Paymode(32) = "CHEQUE ENTRY" : Pay(32) = "CH"
        Dim dr As DataRow
        For i As Integer = 0 To Paymode.Length - 1
            dr = dtPay.NewRow
            dr("PAYMODE") = Paymode(i).ToString
            dr("PAY") = Pay(i).ToString
            dtPay.Rows.Add(dr)
        Next
        cmbpaymode.DataSource = dtPay
        cmbpaymode.DisplayMember = "PAYMODE"
        cmbpaymode.ValueMember = "PAY"
    End Sub
End Class