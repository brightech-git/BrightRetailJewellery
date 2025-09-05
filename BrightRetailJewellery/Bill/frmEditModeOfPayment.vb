Imports System.Data.OleDb
Public Class frmEditModeOfPayment
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim batchNo As String
    Dim CASHID As String
    Dim BANKID As String
    Dim objAdvance As New frmAdvanceAdj(BillCostId)
    Dim objCreditCard As New frmCreditCardAdj
    Dim objCheaque As New frmChequeAdj
    Dim objAddressDia As frmAddressDia
    Dim objMultiDiscount As New frmBillMultiDiscount

    Dim TRANNO As Integer
    Dim BillCostId As String
    Dim BillDate As Date
    Dim BillCashCounterId As String
    Dim VATEXM As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim GstPer As Decimal
    Dim InterStateBill As Boolean



    Public Sub New(ByVal billDate As Date, ByVal billCostId As String, ByVal batchNo As String, Optional ByVal Edbilltype As String = "SA")

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.batchNo = batchNo
        objAdvance = New frmAdvanceAdj(billCostId)

        objAddressDia = New frmAddressDia(billDate, billCostId, batchNo, False)
        objAddressDia.dtpAddressDueDate.Enabled = False
        Me.BillDate = billDate
        CASHID = GetAdmindbSoftValue("CASH", "CASH")
        BANKID = GetAdmindbSoftValue("BANK", "BANK")
        Edbilltype = Edbilltype
    End Sub

    Private Sub CalcReceive()
        strSql = " SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT WHEN TRANMODE = 'D' AND PAYMODE<>'SV' THEN -1*AMOUNT END) AS AMOUNT"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT AMOUNT,PAYMODE,TRANMODE FROM " & CNSTOCKDB & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE IN "
        strSql += " ("
        strSql += "  'SA','SV','SR','RV','PU','PV'  "
        strSql += "  ,'DR','AR','MR','HR','OR'  "
        strSql += "  ,'DP','AP','MP','HP','HG','HB','HZ','HD','SE'"
        strSql += " )"
        strSql += " )X"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtAdjReceive_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")

        strSql = " SELECT TOP 1 TRANNO,TRANDATE,COSTID,COMPANYID,CASHID,VATEXM FROM " & CNSTOCKDB & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE NOT IN "
        strSql += " ("
        strSql += "  'SA','SV','SR','RV','PU','PV'  "
        strSql += "  ,'DR','AR','MR','HR','OR'  "
        strSql += "  ,'DP','AP','MP','HP','HG','HB','HZ','HD'"
        strSql += " )"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then
            MsgBox("Invalid BillNo", MsgBoxStyle.Information)
            Me.Close()
        End If
        Dim ro As DataRow = dtTemp.Rows(0)
        TRANNO = Val(ro.Item("TRANNO").ToString)
        BillDate = ro.Item("TRANDATE")
        BillCostId = ro.Item("COSTID").ToString
        BillCashCounterId = ro.Item("CASHID").ToString
        VATEXM = ro.Item("VATEXM").ToString
    End Sub
    Private Sub CalcRoundOff()
        strSql = " SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END FROM " & CNSTOCKDB & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'RO'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtAdjRoundoff_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcChitCard()
        strSql = " SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE IN ("
        strSql += " 'SS','CG','CB','CZ','CD'"
        strSql += " )"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtTemp As New DataTable
        da.Fill(dtTemp)
        Dim amt As Double = Nothing
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", String.Empty).ToString)
        txtAdjChitCard_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcOthers()
        strSql = " SELECT AMOUNT,PAYMODE FROM " & CNSTOCKDB & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE IN ("
        strSql += " 'HC'"
        strSql += " ,'DU'"
        strSql += " ,'GV'"
        strSql += " )"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtTemp As New DataTable
        da.Fill(dtTemp)

        Dim amt As Double = Nothing
        ''DU
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", "PAYMODE = 'DU'").ToString)
        txtAdjCredit_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
        'GV'
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", "PAYMODE = 'GV'").ToString)
        txtAdjGiftVoucher_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
        ''HC
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", "PAYMODE = 'HC'").ToString)
        txtAdjHandlingCharge_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")

        strSql = "SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO IN"
        strSql += " (SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE  BATCHNO = '" & batchNo & "')"
        strSql += " AND RECPAY='R' AND ISNULL(CANCEL,'')='' AND TRANTYPE='D'"
        If Val(objGPack.GetSqlValue(strSql).ToString) = 1 Then
            txtAdjCredit_AMT.Enabled = False        'Credit Adjusted
        End If
    End Sub
    Private Sub CalcDiscountInd()
        strSql = " SELECT CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1*AMOUNT END FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'DI' AND ACCODE = 'DISC'"
        Dim amt As Double = Nothing
        amt = Val(objGPack.GetSqlValue(strSql).ToString)
        txtAdjIndDiscount.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcDiscount()
        strSql = " SELECT "
        strSql += " (SELECT DISCNAME FROM " & CNADMINDB & "..MULTIDISCOUNT WHERE DISCID = T.ACCODE)AS DISCNAME"
        strSql += " ,AMOUNT"
        strSql += " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'DI' AND ACCODE <> 'DISC'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objMultiDiscount.dtGridDisc)
        objMultiDiscount.CalcGridDiscTotal()
        Dim discAmt As Double = Val(objMultiDiscount.gridDiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        discAmt += Val(txtAdjIndDiscount.Text)
        txtAdjDiscount_AMT.Text = IIf(discAmt <> 0, Format(discAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcAdvance()
        strSql = " SELECT SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE DATE"
        strSql += VBCRLF + " ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += VBCRLF + " WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS RECEIVEDAMT"
        strSql += VBCRLF + " ,(SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += VBCRLF + " WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS ADJUSTEDAMT"
        strSql += VBCRLF + " ,AMOUNT+ISNULL(GSTVAL,0) AS AMOUNT"
        strSql += VBCRLF + " ,AMOUNT AS BALAMOUNT"
        strSql += VBCRLF + " ,GSTVAL AS GST"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strSql += vbCrLf + " WHERE BATCHNO = '" & batchNo & "' AND TRANTYPE IN ('A','C') AND RECPAY <> 'R' AND BATCHNO NOT IN ( SELECT BATCHNO  FROM " & cnAdminDb & "..ORMAST  WHERE BATCHNO = '" & batchNo & "' )"
        'changed
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objAdvance.dtGridAdvance)
        objAdvance.StyleGridAdvance(objAdvance.gridAdvance)
        objAdvance.StyleGridAdvance(objAdvance.gridAdvanceTotal)
        objAdvance.CalcGridAdvanceTotal()
        Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        'advAmt = advAmt * (-1)
        txtAdjAdvance_AMT.Text = IIf(advAmt <> 0, Format(advAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcCreditCard()
        strSql = " SELECT"
        strSql += " (SELECT NAME FROM " & CNADMINDB & "..CREDITCARD WHERE CARDCODE = T.CARDID)AS CARDTYPE"
        strSql += " ,CHQDATE DATE"
        strSql += " ,AMOUNT"
        strSql += " ,CHQCARDNO AS CARDNO"
        strSql += " ,CONVERT(VARCHAR,'')APPNO"
        strSql += " ,(SELECT COMMISSION FROM " & CNADMINDB & "..CREDITCARD WHERE CARDCODE = T.CARDID)COMMISION"
        strSql += " FROM " & CNSTOCKDB & "..ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CC'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCreditCard.dtGridCreditCard)
        objCreditCard.CalcGridCreditCardTotal()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcCheque()
        strSql = " SELECT CHQCARDREF BANKNAME,CHQDATE DATE,CHQCARDNO CHEQUENO,AMOUNT"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS DEFAULTBANK"
        strSql += " ,(SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODEID = T.FLAG)AS MODE"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CH'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCheaque.dtGridCheque)
        objCheaque.CalcGridChequeTotal()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub
    Private Sub SetAddressDetails()
        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO"
        strSql += " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY"
        strSql += " ,PINCODE,EMAIL,FAX,PHONERES,MOBILE,PAN "
        strSql += " ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)AS PLACE"
        strSql += " FROM " & cnAdminDb & "..PERSONALINFO P"
        strSql += " WHERE SNO = (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchNo & "')"
        Dim dtAddress As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAddress)
        If Not dtAddress.Rows.Count > 0 Then Exit Sub
        Dim ro As DataRow = dtAddress.Rows(0)
        With objAddressDia
            .txtAddressPrevilegeId.Text = ro.Item("PREVILEGEID").ToString
            .txtAddressPartyCode.Text = ro.Item("ACCODE").ToString
            .cmbAddressTitle_OWN.Text = ro.Item("TITLE").ToString
            .txtAddressInitial.Text = ro.Item("INITIAL").ToString
            .txtAddressName.Text = ro.Item("PNAME").ToString
            .txtAddressDoorNo.Text = ro.Item("DOORNO").ToString
            .txtAddress1.Text = ro.Item("ADDRESS1").ToString
            .txtAddress2.Text = ro.Item("ADDRESS2").ToString
            .txtAddress3.Text = ro.Item("ADDRESS3").ToString
            .cmbAddressArea_OWN.Text = ro.Item("AREA").ToString
            .cmbAddressCity_OWN.Text = ro.Item("CITY").ToString
            .cmbAddressState.Text = ro.Item("STATE").ToString
            .cmbAddressCountry_OWN.Text = ro.Item("COUNTRY").ToString
            .txtAddressPincode_NUM.Text = ro.Item("PINCODE").ToString
            .txtAddressEmailId_OWN.Text = ro.Item("EMAIL").ToString
            .txtAddressFax.Text = ro.Item("FAX").ToString
            .txtAddressPan.Text = ro.Item("PAN").ToString
            .txtAddressPhoneRes.Text = ro.Item("PHONERES").ToString
            .txtAddressMobile.Text = ro.Item("MOBILE").ToString
            .txtRemark1.Text = objGPack.GetSqlValue("SELECT remark1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchNo & "'")
            If ro.Item("PLACE").ToString <> CompanyState Then
                InterStateBill = True
            End If
        End With
    End Sub


    Private Sub frmEditModeOfPayment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _CounterWiseCashMaintanance Then
            CASHID = objGPack.GetSqlValue("SELECT CASHACCODE FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & BillCashCounterId & "' AND ISNULL(CASHACCODE,'') <> ''", , GetAdmindbSoftValue("CASH", "CASH"))
        End If
        CalcReceive()
        CalcRoundOff()
        CalcChitCard()
        CalcOthers()
        CalcCreditCard()
        CalcCheque()
        CalcAdvance()
        CalcDiscountInd() 'INDIVIDUAL ITEM DISCOUNT AND BULK DISC
        CalcDiscount() 'MULTI DISCOUNT
        SetAddressDetails()
        Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE", "")
        Dim GstRecAcc() As String
        If GstRecCode.Contains(",") Then
            GstRecAcc = GstRecCode.Split(",")
            If GstRecAcc.Length = 3 Then
                SCode = GstRecAcc(0).ToString
                CCode = GstRecAcc(1).ToString
                ICode = GstRecAcc(2).ToString
            End If
        End If
        GstPer = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'", , "0").ToString)
        txtAdjCash_AMT.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtAdjCreditCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCreditCard_AMT.GotFocus
        If objCreditCard.Visible Then Exit Sub
        objCreditCard.BackColor = Me.BackColor
        objCreditCard.StartPosition = FormStartPosition.CenterScreen
        objCreditCard.grpCreditCard.BackgroundColor = grpAdj.BackgroundColor
        objCreditCard.ShowDialog()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub
    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = Me.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = grpAdj.BackgroundColor
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjAdvance_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvance_AMT.GotFocus
        If objAdvance.Visible Then Exit Sub
        objAdvance.BackColor = Me.BackColor
        objAdvance.StartPosition = FormStartPosition.CenterScreen
        objAdvance.grpAdvance.BackgroundColor = grpAdj.BackgroundColor
        objAdvance.StyleGridAdvance(objAdvance.gridAdvance)
        objAdvance.StyleGridAdvance(objAdvance.gridAdvanceTotal)
        If objAdvance.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If objAddressDia.txtAddressName.Text = "" Then
                GetAdvanceAddress()
            End If
            Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            txtAdjAdvance_AMT.Text = IIf(advAmt <> 0, Format(advAmt, "0.00"), Nothing)
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub GetAdvanceAddress()
        With objAdvance
            objAddressDia.AddressLock = True
            'objAddressDia.chkRegularCustomer.Enabled = False
            'objAddressDia.chkRegularCustomer.Checked = True
            objAddressDia.txtAddressPrevilegeId.Text = .txtAddressPrevilegeId.Text
            objAddressDia.txtAddressPartyCode.Text = .txtAddressPartyCode.Text
            objAddressDia.cmbAddressTitle_OWN.Text = .cmbAddressTitle_OWN.Text
            objAddressDia.txtAddressInitial.Text = .txtAddressInitial.Text
            objAddressDia.txtAddressName.Text = .txtAddressName.Text
            objAddressDia.txtAddressDoorNo.Text = .txtAddressDoorNo.Text
            objAddressDia.txtAddress1.Text = .txtAddress1.Text
            objAddressDia.txtAddress2.Text = .txtAddress2.Text
            objAddressDia.txtAddress3.Text = .txtAddress3.Text
            objAddressDia.cmbAddressArea_OWN.Text = .cmbAddressArea_OWN.Text
            objAddressDia.cmbAddressCity_OWN.Text = .cmbAddressCity_OWN.Text
            objAddressDia.cmbAddressState.Text = .cmbAddressState_OWN.Text
            objAddressDia.cmbAddressCountry_OWN.Text = .cmbAddressCountry_OWN.Text
            objAddressDia.txtAddressPincode_NUM.Text = .txtAddressPincode_NUM.Text
            objAddressDia.txtAddressEmailId_OWN.Text = .txtAddressEmailId_OWN.Text
            objAddressDia.txtAddressFax.Text = .txtAddressFax.Text
            objAddressDia.txtAddressPhoneRes.Text = .txtAddressPhoneRes.Text
            objAddressDia.txtAddressMobile.Text = .txtAddressMobile.Text
            objAddressDia.txtAddressRegularSno.Text = .txtAddressRegularSno.Text

        End With
    End Sub

    Private Sub txtAdjChitCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjChitCard_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjChitCard_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub txtAdjDiscount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjDiscount_AMT.GotFocus
        If GetAdmindbSoftValue("MULTIDISCOUNT").ToUpper <> "Y" Then Exit Sub
        If objMultiDiscount.Visible Then Exit Sub
        objMultiDiscount.BackColor = Me.BackColor
        objMultiDiscount.StartPosition = FormStartPosition.CenterScreen
        objMultiDiscount.grpMultiDiscount.BackgroundColor = grpAdj.BackgroundColor
        objMultiDiscount.ShowDialog()
        objMultiDiscount.CalcGridDiscTotal()
        Dim discAmt As Double = Val(objMultiDiscount.gridDiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        discAmt += Val(txtAdjIndDiscount.Text)
        txtAdjDiscount_AMT.Text = IIf(discAmt <> 0, Format(discAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub

    Private Function CalcRoundoffAmt(ByVal amt As Double) As Double
        Select Case GetAdmindbSoftValue("AMTROUND", "L")
            Case "L"
                Return Math.Ceiling(amt)
            Case "F"
                If amt - Math.Ceiling(amt) <= 0.49 Then
                    Return Math.Ceiling(amt)
                Else
                    Return amt + 1
                End If
            Case "H"
                Return amt + 1
        End Select
        Return amt
    End Function

    Private Sub CalcFinalAmount()
        Dim receiveAmt As Double = Val(txtAdjReceive_AMT.Text)
        'Dim roundOff As Double = Nothing
        'roundOff = Math.Abs(receiveAmt) - CalcRoundoffAmt(Math.Abs(receiveAmt))
        'txtAdjReceive_AMT.Text = IIf(receiveAmt <> 0, Format(receiveAmt, "0.00"), Nothing)
        'txtAdjRoundoff_AMT.Text = IIf(roundOff <> 0, Format(roundOff, "0.00"), Nothing)
        Dim cash As Double = Nothing
        cash += Val(txtAdjReceive_AMT.Text) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjRoundoff_AMT.Text), -1 * Val(txtAdjRoundoff_AMT.Text)) _
                - Val(txtAdjAdvance_AMT.Text) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCredit_AMT.Text), -1 * Val(txtAdjCredit_AMT.Text)) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjGiftVoucher_AMT.Text), -1 * Val(txtAdjGiftVoucher_AMT.Text)) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjChitCard_AMT.Text), -1 * Val(txtAdjChitCard_AMT.Text)) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCheque_AMT.Text), -1 * Val(txtAdjCheque_AMT.Text)) _
                - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCreditCard_AMT.Text), -1 * Val(txtAdjCreditCard_AMT.Text)) _
                + IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjHandlingCharge_AMT.Text), -1 * Val(txtAdjHandlingCharge_AMT.Text)) _
                - Val(txtAdjDiscount_AMT.Text)
        txtAdjCash_AMT.Text = IIf(cash <> 0, Format(cash, "0.00"), Nothing)
    End Sub

    Private Sub txtAdjReceive_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjReceive_AMT.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjRoundoff_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjRoundoff_AMT.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjDiscount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjDiscount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjHandlingCharge_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjHandlingCharge_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjCredit_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCredit_AMT.GotFocus
        'txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjGiftVoucher_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjGiftVoucher_AMT.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtChange(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtAdjReceive_AMT.TextChanged _
    , txtAdjRoundoff_AMT.TextChanged _
    , txtAdjCredit_AMT.TextChanged _
    , txtAdjAdvance_AMT.TextChanged _
    , txtAdjGiftVoucher_AMT.TextChanged _
    , txtAdjChitCard_AMT.TextChanged _
    , txtAdjCreditCard_AMT.TextChanged _
    , txtAdjCheque_AMT.TextChanged _
    , txtAdjHandlingCharge_AMT.TextChanged _
    , txtAdjDiscount_AMT.TextChanged
        CalcFinalAmount()
    End Sub

    Private Sub AdvanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceToolStripMenuItem.Click
        txtAdjAdvance_AMT.Focus()
    End Sub


    Private Sub tStripGiftVouhcer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripGiftVouhcer.Click
        txtAdjGiftVoucher_AMT.Focus()
    End Sub

    Private Sub CashToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashToolStripMenuItem.Click
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub DiscountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscountToolStripMenuItem.Click
        txtAdjDiscount_AMT.Focus()
    End Sub

    Private Sub HandlingChargeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HandlingChargeToolStripMenuItem.Click
        txtAdjHandlingCharge_AMT.Focus()
    End Sub

    Private Sub CreditCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardToolStripMenuItem.Click
        txtAdjCreditCard_AMT.Focus()
    End Sub

    Private Sub ChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
        txtAdjCheque_AMT.Focus()
    End Sub

    Private Sub txtAdjCash_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjCash_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowAddressDia()
            'btnSave.Focus()
        End If
    End Sub

    Private Sub ShowAddressDia()
        If objAddressDia.Visible Then Exit Sub
        objAddressDia.BackColor = Me.BackColor
        objAddressDia.StartPosition = FormStartPosition.Manual
        objAddressDia.Location = New Point(75, 181)
        objAddressDia.MaximizeBox = False
        objAddressDia.grpAddress.BackgroundColor = grpAdj.BackgroundColor
        objAddressDia.dtpAddressDueDate.Select()
        If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            objAddressDia.DeleteAndInsert(tran)
            InsertAccountsDetail()

            ''Check Transaction 
            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & batchNo & "'", , "0", tran))
            If balAmt <> 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing


                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                CalcFinalAmount()
                Exit Sub
            End If
            tran.Commit()
            tran = Nothing
            Dim msg As String = "Updated Successfully.." + vbCrLf
            MsgBox(msg)
            Me.Close()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Function InsertAccountsDetail() As Boolean
        Dim mremark As String = objAddressDia.txtRemark1.Text
        Dim ContraCode As String = Nothing 'objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & CNSTOCKDB & "..ACCTRAN WHERE BATCHNO = '" & BatchNo & "' AND TRANMODE = '" & IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D") & "' ORDER BY SNO", , , tran)
        Dim outAccode As String = objAddressDia.txtAddressPartyCode.Text
        Dim ORChk As String = objGPack.GetSqlValue("SELECT Top 1 PAYMODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & batchNo & "' and PAYMODE ='OR'", , "SA", tran)
        strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE IN ("
        strSql += " 'CA'"
        strSql += " ,'AA'"
        strSql += " ,'CC'"
        strSql += " ,'BC'"
        strSql += " ,'BS'"
        strSql += " ,'CH'"
        strSql += " ,'HC'"
        strSql += " ,'DI'"
        strSql += " ,'AP'"
        If txtAdjCredit_AMT.Enabled = True Then
            strSql += " ,'DU'"
        End If
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

        strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND PAYMODE='SV' AND TRANMODE='D' AND ACCODE ='" & ICode & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND PAYMODE='SV' AND TRANMODE='D' AND ACCODE ='" & CCode & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND PAYMODE='SV' AND TRANMODE='D' AND ACCODE ='" & SCode & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

        strSql = " DELETE FROM " & cnStockDb & "..TAXTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND TRANTYPE='AA'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

        strSql = " DELETE FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND TRANTYPE = 'A' AND RECPAY <> 'R'"
        If ORChk <> "OR" Then
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End If

        strSql = " DELETE FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND TRANTYPE = 'C' AND RECPAY <> 'R'"
        If ORChk <> "OR" Then
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End If

        Dim RunNo As String = ""
        If txtAdjCredit_AMT.Enabled = True Then
            strSql = " SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & batchNo & "'"
            strSql += " AND TRANTYPE = 'D' AND RECPAY = 'P'"
            strSql += " AND ISNULL(CANCEL,'')=''"
            RunNo = objGPack.GetSqlValue(strSql, "RUNNO", "", tran)
            If RunNo <> "" Then
                strSql = " DELETE FROM " & cnAdminDb & "..OUTSTANDING"
                strSql += " WHERE BATCHNO = '" & batchNo & "'"
                strSql += " AND TRANTYPE = 'D' AND RECPAY = 'P'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            End If
            If RunNo = "" Then
                RunNo = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "D" + Mid(Format(BillDate, "dd/MM/yyyy"), 9, 2).ToString + TRANNO.ToString
                strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & RunNo & "' "
                If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                    RunNo = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "D" + Mid(Format(BillDate, "dd/MM/yyyy"), 9, 2).ToString + GetCreditSalesBillNo.ToString
                End If
            End If
        End If

        ''Cash Transaction
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C"),
         CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", ContraCode, , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))



        ''Advance Trans
        For Each ro As DataRow In objAdvance.dtGridAdvance.Rows
            Dim GstVal As Double = Val(ro!GST.ToString)
            Dim _AdvAmt As Double = Val(ro!AMOUNT.ToString)
            If Val(ro!BALAMOUNT.ToString) > 0 Then _AdvAmt = Val(ro!BALAMOUNT.ToString)

            If ro!RUNNO.ToString.Substring(0, 1).ToString = "C" Then
                InsertIntoAccTran(TRANNO, IIf(_AdvAmt > 0, "D", "C"),
            IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "DRS"), _AdvAmt, 0, 0, 0, "AA", ContraCode, , , , ro!DATE.ToString, , , mremark)
            Else
                InsertIntoAccTran(TRANNO, IIf(_AdvAmt > 0, "D", "C"),
            IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE"), _AdvAmt, 0, 0, 0, "AA", ContraCode, , , , ro!DATE.ToString, , , mremark)
            End If

            If InterStateBill Then
                InsertIntoAccTran(TRANNO, IIf(GstVal > 0, "D", "C"),
                    ICode, GstVal, 0, 0, 0, "SV", ContraCode, , , , ro!DATE.ToString, , , mremark)
            Else
                InsertIntoAccTran(TRANNO, IIf(GstVal > 0, "D", "C"),
                    SCode, (GstVal / 2), 0, 0, 0, "SV", ContraCode, , , , ro!DATE.ToString, , , mremark)
                InsertIntoAccTran(TRANNO, IIf(GstVal > 0, "D", "C"),
                    CCode, (GstVal / 2), 0, 0, 0, "SV", ContraCode, , , , ro!DATE.ToString, , , mremark)
            End If

            If objAddressDia.txtAddressPartyCode.Text = "" Then outAccode = "ADVANCE"
            If ro!RUNNO.ToString.Substring(0, 1).ToString = "C" Then
                InsertIntoOustanding(TRANNO, "C", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, _AdvAmt, "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , mremark, , "DRS", GstVal)
            Else
                InsertIntoOustanding(TRANNO, "A", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, _AdvAmt, "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , mremark, , outAccode, GstVal)
            End If
        Next




        ''CreditCard Trans

        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows


            InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"),
            objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, "CC", ContraCode,
            , , ro!CARDNO.ToString, ro!DATE.ToString,
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
            ro!APPNO.ToString, mremark, , IIf(ORChk = "OR", "D", "P")
            )

            Dim commision As Double = Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"),
                objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, "BC", "BANKC",
                , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))

                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"),
                "BANKC" _
                , commision, 0, 0, 0, "BC", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
                , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"),
                    objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , sCharge, 0, 0, 0, "BS", "BANKS",
                    , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))

                    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"),
                    "BANKS" _
                    , sCharge, 0, 0, 0, "BS", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
                    , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))
                End If
            End If

        Next

        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            Dim _Mode As String = ""
            If ro!MODE.ToString <> "" Then
                '_Mode = ro!MODE.ToString.Substring(0, 1)
                _Mode = objGPack.GetSqlValue("SELECT MODEID FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODENAME = '" & ro!MODE.ToString & "'", , , tran).ToString
            End If
            InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"),
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran),
            Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", ContraCode,
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString, mremark, , IIf(ORChk = "OR", "D", "P"), _Mode)
        Next



        ''Handling Charges
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"),
        "HANDC", Val(txtAdjHandlingCharge_AMT.Text), 0, 0, 0, "HC", ContraCode, , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))
        ''Discount Trans
        Dim mulDisc As Double = Nothing
        If GetAdmindbSoftValue("MULTIDISCOUNT", , tran).ToUpper = "Y" Then
            For Each ro As DataRow In objMultiDiscount.dtGridDisc.Rows
                mulDisc += Val(ro!AMOUNT.ToString)
                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C"),
                objGPack.GetSqlValue("SELECT DISCID FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & ro!DISCNAME.ToString & "'", , , tran) _
                , Val(ro!AMOUNT.ToString), 0, 0, 0, "DI", ContraCode, , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))
            Next
        End If
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C") _
        , "DISC", Val(txtAdjDiscount_AMT.Text) - mulDisc, 0, 0, 0, "DI", ContraCode, , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))

        ''CREDIT
        Dim xaccode As String = IIf(objAddressDia.txtAddressPartyCode.Text = "", "DRS", objAddressDia.txtAddressPartyCode.Text)
        If txtAdjCredit_AMT.Enabled = True Then
            InsertIntoAccTran(TRANNO, "D" _
            , xaccode, Val(txtAdjCredit_AMT.Text), 0, 0, 0, "DU", ContraCode, , , , , , , mremark, , IIf(ORChk = "OR", "D", "P"))

            InsertIntoOustanding(TRANNO, "D", RunNo _
            , Val(txtAdjCredit_AMT.Text), "P", "DU", , , , , , ,
            , , , , mremark, , xaccode)
        End If

        ''UPDATE CONTRA
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'C'  AND ISNULL(CONTRA,'') = ''"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Function
    Public Function GetCreditSalesBillNo() As Integer
GetNewBillNo:
        Dim NewBillNo As Integer = Nothing
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = 'GEN-CRSALESBILLNO'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = 'GEN-CRSALESBILLNO'"
                NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                Return NewBillNo
            Else 'NEWBILLNO GENERATING HERE
                GoTo GenerateNewBillNo
            End If
        Else 'TEMPBILLNO NOT EXIST
GenerateNewBillNo:
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-CRSALESBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            NewBillNo = Val(objGPack.GetSqlValue(strSql, , , tran))
ReGenBillNo:
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
            strSql += " WHERE CTLID ='GEN-CRSALESBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GetNewBillNo
            End If
            NewBillNo += 1
        End If
        Dim bPrefix As String = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "D" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & bPrefix & NewBillNo.ToString & "' "
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
            NewBillNo += 1
            GoTo GenerateNewBillNo
        End If
        InsertBillNo("GEN-CRSALESBILLNO", NewBillNo, tran)
        Return NewBillNo
    End Function
    Private Sub InsertIntoOustanding _
    (
    ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
    ByVal RecPay As String,
    ByVal Paymode As String,
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
    Optional ByVal GSTVAL As Double = 0
        )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        Dim Sno As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID,FROMFLAG"
        strSql += " ,RATE,VALUE,CASHID,VATEXM,REMARK1,REMARK2"
        strSql += " ,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,PAYMODE"
        strSql += " ,GSTVAL"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & Sno & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ,0" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'P'"
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & VATEXM & "'" 'VATEXM
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
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & Paymode & "'" ' PAYMODE
        strSql += " ," & Val(GSTVAL) & "" 'GSTVAL
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        If GSTVAL > 0 Then
            If InterStateBill Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO"
                strSql += " )"
                strSql += " VALUES("
                strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += " ," & tNo & "" 'TRANNO
                strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                strSql += " ,'" & Paymode & "'" 'TRANTYPE
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'IG'" 'TAXID
                strSql += " ," & Amount & "" 'AMOUNT
                strSql += " ," & GstPer & "" 'TAXPER
                strSql += " ," & Math.Abs(GSTVAL) & "" 'TAXAMOUNT
                strSql += " ,3" 'TSNO
                strSql += " ,'" & BillCostId & "'" 'COSTID 
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " ,'" & Sno & "'" 'COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            Else
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO"
                strSql += " )"
                strSql += " VALUES("
                strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += " ," & tNo & "" 'TRANNO
                strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                strSql += " ,'" & Paymode & "'" 'TRANTYPE
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'SG'" 'TAXID
                strSql += " ," & Amount & "" 'AMOUNT
                strSql += " ," & GstPer / 2 & "" 'TAXPER
                strSql += " ," & Math.Abs(GSTVAL / 2) & "" 'TAXAMOUNT
                strSql += " ,1" 'TSNO
                strSql += " ,'" & BillCostId & "'" 'COSTID 
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " ,'" & Sno & "'" 'COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO"
                strSql += " )"
                strSql += " VALUES("
                strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += " ," & tNo & "" 'TRANNO
                strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                strSql += " ,'" & Paymode & "'" 'TRANTYPE
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'CG'" 'TAXID
                strSql += " ," & Amount & "" 'AMOUNT
                strSql += " ," & GstPer / 2 & "" 'TAXPER
                strSql += " ," & Math.Abs(GSTVAL / 2) & "" 'TAXAMOUNT
                strSql += " ,2" 'TSNO
                strSql += " ,'" & BillCostId & "'" 'COSTID 
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " ,'" & Sno & "'" 'COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            End If
        End If
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
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
    Optional ByVal chqCardNo As String = Nothing,
    Optional ByVal chqDate As String = Nothing,
    Optional ByVal chqCardId As Integer = Nothing,
    Optional ByVal chqCardRef As String = Nothing,
    Optional ByVal Remark1 As String = Nothing,
    Optional ByVal Remark2 As String = Nothing,
    Optional ByVal FRomflg As String = "P",
    Optional ByVal Flag As String = Nothing)

        If amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,APPVER,COMPANYID,FLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
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
        strSql += " ,'" & FRomflg & "'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & VATEXM & "'" 'VATEXM
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Flag & "'" 'FLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub CreditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditToolStripMenuItem.Click
        txtAdjCredit_AMT.Focus()
    End Sub

    Private Sub txtAdjCredit_AMT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAdjCredit_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub
End Class