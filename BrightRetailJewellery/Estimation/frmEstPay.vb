Imports System.Data.OleDb
Public Class frmEstPay
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Public batchNo As String
    Dim CASHID As String
    Dim BANKID As String
    Dim objAdvance As New frmAdvanceAdj(cnCostId)
    Dim objCreditCard As New frmCreditCardAdj
    Dim objCheaque As New frmChequeAdj
    Public objAddressDia As frmAddressDia
    Dim objMultiDiscount As New frmBillMultiDiscount
    Dim objChitCard As New frmChitAdj '(True)
    Public TRANNO As Integer
    Public BillCostId As String
    Public BillDate As Date
    Public BillCashCounterId As String
    Dim VATEXM As String
    Dim RndId As Integer
    Public CashRecd As Boolean = False
    Public dtGrid As New DataTable
    Dim AddressEdit As Boolean = False
    Public TotRecdamount As Decimal = 0
    Public Estpaybatch As String = Nothing
    Public Addr As String
    Public dtadvance As New DataTable
    Public chitadjsave As Boolean = False
    Public OrderNos As String = Nothing
    Public dtReservedItem As New DataTable



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objAddressDia = New frmAddressDia
        cmbRecPay_MAN.Items.Add("CREDIT_SALES")
        cmbRecPay_MAN.Items.Add("CUSTOMER_ADVANCE")
        cmbRecPay_MAN.Items.Add("FURTHER_ADVANCE")
        cmbRecPay_MAN.Items.Add("OTHER_RECEIPTS")
        cmbRecPay_MAN.Items.Add("REPAY")
        cmbRecPay_MAN.Items.Add("OTHER_PAYMENTS")

        CASHID = GetAdmindbSoftValue("CASH", "CASH")
        BANKID = GetAdmindbSoftValue("BANK", "BANK")

    End Sub

    Private Sub CalcChitCard()
        strSql = " SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT FROM MASTER..TEMP" & RndId & "ACCTRAN"
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
        strSql = " SELECT AMOUNT,PAYMODE FROM MASTER..TEMP" & RndId & "ACCTRAN"
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
    End Sub
    Private Sub CalcDiscountInd()
        strSql = " SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'DI' AND ACCODE = 'DISC'"
        Dim amt As Double = Nothing
        amt = Val(objGPack.GetSqlValue(strSql))
        txtAdjIndDiscount.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcDiscount()
        strSql = " SELECT "
        strSql += " (SELECT DISCNAME FROM " & CNADMINDB & "..MULTIDISCOUNT WHERE DISCID = T.ACCODE)AS DISCNAME"
        strSql += " ,AMOUNT"
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN T"
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
        strsql += vbcrlf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strsql += vbcrlf + "  WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS RECEIVEDAMT"
        strsql += vbcrlf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strsql += vbcrlf + "  WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS ADJUSTEDAMT"
        strsql += vbcrlf + "  ,AMOUNT"
        strSql += vbCrLf + "  FROM MASTER..TEMP" & RndId & "OUTSTANDING AS O"
        strSql += vbCrLf + "  WHERE BATCHNO = '" & batchNo & "' AND TRANTYPE = 'A' AND PAYMODE = 'AA'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objAdvance.dtGridAdvance)
        objAdvance.CalcGridAdvanceTotal()
        Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
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
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CC'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCreditCard.dtGridCreditCard)
        objCreditCard.CalcGridCreditCardTotal()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcCheque()
        strSql = " SELECT CHQCARDREF BANKNAME,CHQDATE DATE,CHQCARDNO CHEQUENO,AMOUNT"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)aS DEFAULTBANK"
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CH'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCheaque.dtGridCheque)
        objCheaque.CalcGridChequeTotal()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub
    Private Sub SetAddressDetails()
        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO"
        strsql += vbcrlf + "  ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY"
        strsql += vbcrlf + "  ,PINCODE,EMAIL,FAX,PHONERES,MOBILE "
        strsql += vbcrlf + "  FROM " & cnAdminDb & "..PERSONALINFO"
        strsql += vbcrlf + "  WHERE SNO = (SELECT top 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchNo & "')"
        Dim dtAddress As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAddress)
        If Not dtAddress.Rows.Count > 0 Then Exit Sub
        Dim ro As DataRow = dtAddress.Rows(0)
        With objAddressDia
            '.chkRegularCustomer.Checked = True
            '.chkRegularCustomer.Enabled = False
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
            .txtAddressPhoneRes.Text = ro.Item("PHONERES").ToString
            .txtAddressMobile.Text = ro.Item("MOBILE").ToString
            '.txtAddressRegularSno.Text = ro.Item("SNO").ToString
        End With
    End Sub

    Private Sub frmEditModeOfPayment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _CounterWiseCashMaintanance Then
            CASHID = objGPack.GetSqlValue("SELECT CASHACCODE FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & BillCashCounterId & "' AND ISNULL(CASHACCODE,'') <> ''", , GetAdmindbSoftValue("CASH", "CASH"))
        End If
        Dim CLEARYES As Boolean = False
        If Not dtGrid.Columns.Contains("PARTICULAR") Then dtGrid.Columns.Add("PARTICULAR", GetType(String)) : CLEARYES = True
        If Not dtGrid.Columns.Contains("AMOUNT") Then dtGrid.Columns.Add("AMOUNT", GetType(Decimal))
        If CLEARYES Then CClear()
        If Estpaybatch <> Nothing Then getpaydetails()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        CashRecd = True
        Me.Close()
    End Sub
    Private Sub getpaydetails()
        If Val(txtAdjCheque_AMT.Text) <> 0 Then
            strSql = " SELECT CHQCARDREF BANKNAME,CHQDATE DATE,CHQCARDNO CHEQUENO,AMOUNT,T.ACNAME DEFAULTBANK FROM " & cnStockDb & "..ESTACCTRAN AS E"
            strSql += " LEFT JOIN " & cnAdminDb & "..ACHEAD T ON T.ACCODE =E.ACCODE"
            strSql += " WHERE TRANMODE = 'D' and PAYMODE = 'CH'"
            If Estpaybatch <> "" Then strSql += " AND ESTBATCHNO ='" & Estpaybatch & "'"
            strSql += " AND TRANDATE = '" & BillDate.Date & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(objCheaque.dtGridCheque)
            objCheaque.CalcGridChequeTotal()
        End If
        If Val(txtAdjCreditCard_AMT.Text) <> 0 Then
            strSql = " SELECT CC.name CARDTYPE,CHQDATE DATE,AMOUNT,CHQCARDNO CARDNO,'' APPNO FROM " & cnStockDb & "..ESTACCTRAN AS E"
            strSql += " LEFT JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.CARDCODE =E.CARDID"
            strSql += " WHERE TRANMODE = 'D' and PAYMODE = 'CC'"
            If Estpaybatch <> "" Then strSql += " AND ESTBATCHNO ='" & Estpaybatch & "'"
            strSql += " AND TRANDATE = '" & BillDate.Date & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(objCreditCard.dtGridCreditCard)
            objCreditCard.CalcGridCreditCardTotal()
        End If
        If Val(txtAdjAdvance_AMT.Text) <> 0 Then
            strSql = " SELECT COSTID,SUBSTRING(ISNULL(RUNNO,''),6,20)RUNNO,TRANDATE AS DATE,AMOUNT,COMPANYID,'' APPNO FROM " & cnStockDb & "..ESTOUTSTANDING AS E"
            strSql += " WHERE TRANTYPE = 'A' and PAYMODE = 'AA'"
            If Estpaybatch <> "" Then strSql += " AND BATCHNO ='" & Estpaybatch & "'"
            strSql += " AND TRANDATE = '" & BillDate.Date & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(objAdvance.dtGridAdvance)
            objAdvance.CalcGridAdvanceTotal()
        End If

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
        objAdvance.billcostid = IIf(BillCostId <> "", BillCostId, cnCostId)
        objAdvance.BackColor = Me.BackColor
        objAdvance.StartPosition = FormStartPosition.CenterScreen
        objAdvance.grpAdvance.BackgroundColor = grpAdj.BackgroundColor
        objAdvance.Ordernos = OrderNos
        If objAdvance.ShowDialog() = Windows.Forms.DialogResult.OK Then
            dtadvance = objAdvance.dtGridAdvance.Copy
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

    Private Sub txtAdjChitCard_AMT_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.Enter

        If Val(txtAdjChitCard_AMT.Text) = 0 Then
            If ValidateChitCard() = False Then
                txtAdjCash_AMT.Focus()
                Exit Sub
            End If
        End If
        'Exit Sub
        'objChitCard = New frmChitAdj
        If objChitCard.Visible Then Exit Sub
        objChitCard.BackColor = Me.BackColor
        objChitCard.StartPosition = FormStartPosition.CenterScreen
        'objChitCard.grpCHIT.BackgroundColor = grpHeader.BackgroundColor
        objChitCard.txtCHITCardGrpCode.Select()
        objChitCard.BillDate = BillDate
        objChitCard.dtReservedItem = dtReservedItem.Copy
        Select Case objChitCard.ShowDialog
            Case Windows.Forms.DialogResult.OK
                Dim chitAmt As Double = Val(objChitCard.gridCHITCardTotal.Rows(0).Cells("TOTAL").Value.ToString)
                txtAdjChitCard_AMT.Text = IIf(chitAmt <> 0, Format(chitAmt, "0.00"), Nothing)
                ''GetAddress
                If chitAmt > 0 And objChitCard.gridCHITCard.RowCount > 0 Then
                    If objAddressDia.txtAddressName.Text = "" Then
                        Addr = objChitCard.lblAddress.Text
                        Dim chitMainDb As String = GetAdmindbSoftValue("CHITDBPREFIX") + "SAVINGS"
                        strSql = " SELECT P.TITLE,P.INITIAL,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.AREA"
                        strSql += " ,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.EMAIL,P.FAX,P.PHONERES,P.MOBILE"
                        strSql += " FROM " & chitMainDb & "..PERSONALINFO AS P," & chitMainDb & "..SCHEMEMAST AS S"
                        strSql += " WHERE S.GROUPCODE = '" & objChitCard.gridCHITCard.Rows(0).Cells("GRPCODE").Value.ToString & "'"
                        strSql += " AND S.REGNO = '" & objChitCard.gridCHITCard.Rows(0).Cells("REGNO").Value.ToString & "'"
                        strSql += " AND P.PERSONALID = S.SNO"
                        Dim dtChitInfo As New DataTable
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtChitInfo)
                        If dtChitInfo.Rows.Count > 0 Then
                            With dtChitInfo.Rows(0)
                                objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                                objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                                objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                                objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                                objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                                objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                                objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                                objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                                objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                                objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                                objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                                objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                                objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                                objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                                objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                            End With
                        End If

                    End If
                End If
                txtAdjCash_AMT.Focus()
            Case Windows.Forms.DialogResult.Abort
                ' objChitCard = New frmChitAdj
                txtAdjCash_AMT.Focus()
        End Select
    End Sub

    Private Sub txtAdjChitCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.GotFocus
        txtAdjChitCard_AMT_Enter(sender, e)
        'txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjChitCard_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjChitCard_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Function ValidateChitCard() As Boolean
        Dim VBCLOCK As String = GetAdmindbSoftValue("VBC_" & strCompanyId & "_LOCK", "A")
        If VBCLOCK.ToUpper = "R" Then
            MsgBox("ChitCard Restricted", MsgBoxStyle.Information)
            Return False
        ElseIf VBCLOCK.ToUpper = "W" Then
            If MessageBox.Show("ChitCard Restricted" + vbCrLf + "Do you wish to Continue?", System.Reflection.Assembly.GetExecutingAssembly().GetName.Name.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Return False
            End If
        End If
        Return True
    End Function

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
        'Dim receiveAmt As Double = Val(txtAdjReceive_AMT.Text)
        'Dim roundOff As Double = Nothing
        'roundOff = Math.Abs(receiveAmt) - CalcRoundoffAmt(Math.Abs(receiveAmt))
        'txtAdjReceive_AMT.Text = IIf(receiveAmt <> 0, Format(receiveAmt, "0.00"), Nothing)
        'txtAdjRoundoff_AMT.Text = IIf(roundOff <> 0, Format(roundOff, "0.00"), Nothing)
        'Dim cash As Double = Nothing
        'cash += Val(txtAdjReceive_AMT.Text) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjRoundoff_AMT.Text), -1 * Val(txtAdjRoundoff_AMT.Text)) _
        '        - Val(txtAdjAdvance_AMT.Text) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCredit_AMT.Text), -1 * Val(txtAdjCredit_AMT.Text)) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjGiftVoucher_AMT.Text), -1 * Val(txtAdjGiftVoucher_AMT.Text)) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjChitCard_AMT.Text), -1 * Val(txtAdjChitCard_AMT.Text)) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCheque_AMT.Text), -1 * Val(txtAdjCheque_AMT.Text)) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjCreditCard_AMT.Text), -1 * Val(txtAdjCreditCard_AMT.Text)) _
        '        + IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjHandlingCharge_AMT.Text), -1 * Val(txtAdjHandlingCharge_AMT.Text)) _
        '        - Val(txtAdjDiscount_AMT.Text)

        ''''+ Val(txtAdjSrCredit_AMT.Text) _
        Dim ActRec As Double = Val(txtAdjReceive_AMT.Text) ' + Val(txtAdjSrCredit_AMT.Text)
        Dim cash As Double = Nothing
        cash += Val(txtAdjReceive_AMT.Text) _
              - IIf(Val(txtAdjReceive_AMT.Text) > 0, Val(txtAdjRoundoff_AMT.Text), -1 * Val(txtAdjRoundoff_AMT.Text)) _
              - Val(txtAdjAdvance_AMT.Text) _
              - IIf(ActRec > 0, Val(txtAdjCredit_AMT.Text), -1 * Val(txtAdjCredit_AMT.Text)) _
              - IIf(ActRec > 0, Val(txtAdjGiftVoucher_AMT.Text), -1 * Val(txtAdjGiftVoucher_AMT.Text)) _
              - Val(txtAdjChitCard_AMT.Text) _
              - IIf(ActRec > 0, Val(txtAdjCheque_AMT.Text), -1 * Val(txtAdjCheque_AMT.Text)) _
              - Val(txtAdjCreditCard_AMT.Text) _
              + IIf(ActRec > 0, Val(txtAdjHandlingCharge_AMT.Text), -1 * Val(txtAdjHandlingCharge_AMT.Text)) _
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
        txtAdjCash_AMT.Focus()
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
            If AddressEdit Then
                ShowAddressDia()
            Else
                btnSave.Select()
            End If
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
            '    InsertAccountsDetail()
            CashRecd = True
            totRecdamount = Val(txtAdjCash_AMT.Text) + Val(txtAdjCheque_AMT.Text) + Val(txtAdjChitCard_AMT.Text)
            Me.Close()
            Exit Sub
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Public Function InsertAccountsDetail(ByVal tran As OleDb.OleDbTransaction) As Boolean
        Dim DrContra As String '= objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'C' ORDER BY SUBSTRING(SNO,6,10)", , , tran)
        Dim CrContra As String '= objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'D' ORDER BY SUBSTRING(SNO,6,10)", , , tran)
        Dim outAccode As String '= objAddressDia.txtAddressPartyCode.Text
        ''Cash Transaction
        InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C"), _
        CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", IIf(Val(txtAdjCash_AMT.Text) > 0, DrContra, CrContra))

        ''Advance Trans
        For Each ro As DataRow In objAdvance.dtGridAdvance.Rows
            Dim _AdvAmt As Double = Val(ro!AMOUNT.ToString)
            Dim GstVal As Double = Val(ro!GST.ToString)
            If Val(ro!BALAMOUNT.ToString) > 0 Then _AdvAmt = Val(ro!BALAMOUNT.ToString)
            InsertIntoAccTran(tran, TRANNO, IIf(_AdvAmt > 0, "D", "C"), _
            IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE"), (_AdvAmt + GstVal), 0, 0, 0, "AA", IIf(_AdvAmt > 0, DrContra, CrContra), , , , ro!DATE.ToString)
            If objAddressDia.txtAddressPartyCode.Text = "" Then outAccode = "ADVANCE"
            InsertIntoOustanding(tran, TRANNO, "A", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, _AdvAmt, "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , , , outAccode, ro!COSTID.ToString, GstVal)
        Next



        ''CreditCard Trans
        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows
            InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
            objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, "CC", IIf(Val(txtAdjReceive_AMT.Text) > 0, DrContra, CrContra), _
            , , ro!CARDNO.ToString, ro!DATE.ToString, _
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            ro!APPNO.ToString _
            )

            Dim commision As Double = 0 'Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
                objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, "BC", "BANKC", _
                , , , , )

                InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
                "BANKC" _
                , commision, 0, 0, 0, "BC", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                , , , , )

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
                    objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , sCharge, 0, 0, 0, "BS", "BANKS", _
                    , , , , )

                    InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
                    "BANKS" _
                    , sCharge, 0, 0, 0, "BS", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                    , , , , )
                End If
            End If
        Next

        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), _
            Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", IIf(Val(txtAdjReceive_AMT.Text) > 0, DrContra, CrContra), _
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
        Next

        ''Chit Voucher
        For Each ro As DataRow In objChitCard.dtChitAdj.Rows
            Dim mrem1 As String = ro!amount.ToString & "," & ro!prize.ToString & "," & ro!bonus.ToString & "," & ro!deduction.ToString
            Dim mrem2 As String = ro!weight.ToString
            InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            Val(ro!total.ToString), 0, 0, 0, "SS", IIf(Val(txtAdjReceive_AMT.Text) > 0, DrContra, CrContra), _
             , , ro!REGNO.ToString, , objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), ro!GRPCODE.ToString, mrem1, mrem2)
            chitadjsave = True
        Next

        ' ''Gift Voucher
        'For Each ro As DataRow In dtGridGiftVouhcer.Rows
        '    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
        '    objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
        '    Val(ro!AMOUNT.ToString), 0, 0, 0, "GV", ContraCode, _
        '    , , , , , ro!REMARK.ToString)
        'Next

        ''Handling Charges
        InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
        "HANDC", Val(txtAdjHandlingCharge_AMT.Text), 0, 0, 0, "HC", IIf(Val(txtAdjReceive_AMT.Text) > 0, CrContra, DrContra))
        ''Discount Trans
        Dim mulDisc As Double = Nothing
        If GetAdmindbSoftValue("MULTIDISCOUNT", , tran).ToUpper = "Y" Then
            For Each ro As DataRow In objMultiDiscount.dtGridDisc.Rows
                mulDisc += Val(ro!AMOUNT.ToString)
                InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C"), _
                objGPack.GetSqlValue("SELECT DISCID FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & ro!DISCNAME.ToString & "'", , , tran) _
                , Val(ro!AMOUNT.ToString), 0, 0, 0, "DI", IIf(Val(txtAdjDiscount_AMT.Text) > 0, DrContra, CrContra))
            Next
        End If
        InsertIntoAccTran(tran, TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C") _
        , "DISC", Val(txtAdjDiscount_AMT.Text) - mulDisc, 0, 0, 0, "DI", IIf(Val(txtAdjDiscount_AMT.Text) > 0, DrContra, CrContra))

        ' ''Rountd Off Trans
        'InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
        '"RNDOFF", Val(txtAdjRoundoff_AMT.Text), 0, 0, 0, "RO", ContraCode)

        ''UPDATE CONTRA
        strSql = " UPDATE " & cnStockDb & "..ESTACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ESTACCTRAN WHERE ESTBATCHNO = T.ESTBATCHNO AND TRANMODE = 'C'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ESTACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND ESTBATCHNO = '" & batchNo & "' AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = " UPDATE " & cnStockDb & "..ESTACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ESTACCTRAN WHERE ESTBATCHNO = T.ESTBATCHNO AND TRANMODE = 'D'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ESTACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND ESTBATCHNO = '" & batchNo & "' AND TRANMODE = 'C' AND ISNULL(CONTRA,'') = ''"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Function

    Private Sub InsertIntoOustanding _
    (ByVal tran As OleDb.OleDbTransaction, ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
    ByVal RecPay As String, _
    ByVal Paymode As String, _
    Optional ByVal GrsWt As Double = 0, _
    Optional ByVal NetWt As Double = 0, _
    Optional ByVal CatCode As String = Nothing, _
    Optional ByVal Rate As Double = Nothing, _
    Optional ByVal Value As Double = Nothing, _
    Optional ByVal refNo As String = Nothing, _
    Optional ByVal refDate As String = Nothing, _
    Optional ByVal purity As Double = Nothing, _
    Optional ByVal proId As Integer = Nothing, _
    Optional ByVal dueDate As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing, _
    Optional ByVal Accode As String = Nothing, _
    Optional ByVal _Costid As String = "", _
    Optional ByVal GstVal As Double = Nothing _
        )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        strSql = " INSERT INTO " & cnStockDb & "..ESTOUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,PAYMODE,GSTVAL)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
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
        strSql += " ," & Rate & "" 'RATE
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
        If _Costid <> "" Then
            strSql += " ,'" & _Costid & "'" 'COSTID
        Else
            strSql += " ,'" & BillCostId & "'" 'COSTID
        End If
        strSql += " ,'" & Paymode & "'" ' PAYMODE
        strSql += " ," & Math.Abs(GstVal) & "" 'GSTVAL
        strSql += " )"
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub InsertIntoAccTran _
    (ByVal tran As OleDb.OleDbTransaction, ByVal tNo As Integer, _
    ByVal tranMode As String, _
    ByVal accode As String, _
    ByVal amount As Double, _
    ByVal pcs As Integer, _
    ByVal grsWT As Double, _
    ByVal netWT As Double, _
    ByVal payMode As String, _
    ByVal contra As String, _
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
    Optional ByVal chqCardNo As String = Nothing, _
    Optional ByVal chqDate As String = Nothing, _
    Optional ByVal chqCardId As Integer = Nothing, _
    Optional ByVal chqCardRef As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing _
    )
        If amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ESTACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,ESTBATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
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
        strSql += " ,'P'" 'FROMFLAG
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
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub Initi()
        dtGrid.Rows.Clear()
        dtGrid.Rows.Add()
        dtGrid.Rows.Add()
        dtGrid.Rows.Add()
        dtGrid.Rows.Add()
        dtGrid.Rows.Add()
        dtGrid.Rows(0).Item("PARTICULAR") = "SALES"
        dtGrid.Rows(1).Item("PARTICULAR") = "RETURN"
        dtGrid.Rows(2).Item("PARTICULAR") = "PURCHASE"
        dtGrid.Rows(3).Item("PARTICULAR") = "RECEIPT"
        dtGrid.Rows(4).Item("PARTICULAR") = "PAYMENT"

        objAdvance = New frmAdvanceAdj(BillCostId)
        objCreditCard = New frmCreditCardAdj
        objCheaque = New frmChequeAdj
        objAddressDia = New frmAddressDia
        objMultiDiscount = New frmBillMultiDiscount
    End Sub

    Private Sub CClear()
        Initi()
        'objGPack.TextClear(Me)
        btnSave.Enabled = True

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        CClear()
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub SchemeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SchemeToolStripMenuItem.Click
        txtAdjChitCard_AMT.Focus()
    End Sub
End Class