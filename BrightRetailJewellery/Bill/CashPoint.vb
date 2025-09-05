Imports System.Data.OleDb
Public Class CashPoint
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
    Public BillCostId As String
    Public BillDate As Date
    Public BillCashCounterId As String
    Dim VATEXM As String
    Dim RndId As Integer
    Dim CASHPTREADONLY As String
    Dim dtGrid As New DataTable
    Dim AddressEdit As Boolean = False
    Dim Def_Focus As String = GetAdmindbSoftValue("CASHPT_DEFFOCUS", "CASH")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objAddressDia = New frmAddressDia
        CASHID = GetAdmindbSoftValue("CASH", "CASH")
        BANKID = GetAdmindbSoftValue("BANK", "BANK")
        CASHPTREADONLY = GetAdmindbSoftValue("CASHPTREADONLY", "N")
        If CASHPTREADONLY = "Y" Then
            txtAdjAdvance_AMT.ReadOnly = True
            txtAdjCash_AMT.ReadOnly = True
            txtAdjCheque_AMT.ReadOnly = True
            txtAdjChitCard_AMT.ReadOnly = True
            txtAdjCreditCard_AMT.ReadOnly = True
            txtAdjCredit_AMT.ReadOnly = True
            txtAdjDiscount_AMT.ReadOnly = True
            txtAdjGiftVoucher_AMT.ReadOnly = True
            txtAdjHandlingCharge_AMT.ReadOnly = True
            txtAdjReceive_AMT.ReadOnly = True
            txtAdjRoundoff_AMT.ReadOnly = True
        End If
    End Sub

    Private Sub CalcReceive()
        strSql = " SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
        strsql += vbcrlf + "  FROM"
        strsql += vbcrlf + "  ("
        strSql += vbCrLf + "  SELECT AMOUNT,PAYMODE,TRANMODE FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strsql += vbcrlf + "  WHERE BATCHNO = '" & batchNo & "'"
        strsql += vbcrlf + "  AND PAYMODE IN "
        strsql += vbcrlf + "  ("
        strSql += vbCrLf + "   'SA','SV','SR','RV','PU','PV','SE'  "
        strsql += vbcrlf + "   ,'DR','AR','MR','HR','OR'  "
        strsql += vbcrlf + "   ,'DP','AP','MP','HP','HG','HB','HZ','HD'"
        strsql += vbcrlf + "  )"
        strsql += vbcrlf + "  )X"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql))
        txtAdjReceive_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcRoundOff()
        strSql = " SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'RO'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql))
        txtAdjRoundoff_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
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
    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtBillNo_NUM.Text) = 0 Then Exit Sub
            btnSave.Enabled = False
            Initi()
            strSql = " EXEC " & cnStockDb & "..SP_RPT_CASHPOINT_BILLINFO"
            strSql += vbCrLf + "  @TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,@TRANNO = " & Val(txtBillNo_NUM.Text) & ""
            Dim dtBillInfo As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtBillInfo)
            If Not dtBillInfo.Rows.Count > 0 Then
                MsgBox("Invalid BillNo", MsgBoxStyle.Information)
                txtBillNo_NUM.Select()
                Exit Sub
            End If
            batchNo = ""
            If dtBillInfo.Rows.Count > 1 Then
                batchNo = BrighttechPack.SearchDialog.Show("Select Bill", dtBillInfo, cn, , 2, , , False)
            Else
                batchNo = dtBillInfo.Rows(0).Item("BATCHNO").ToString
            End If
            If batchNo = "" Then
                Exit Sub
            End If
            TRANNO = Val(txtBillNo_NUM.Text)
            ''Checking Companyid
            strSql = " SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND COMPANYID='" & strCompanyId & "'"
            If Val(objGPack.GetSqlValue(strSql, , , )) = 0 Then
                MsgBox("Invalid Bill No", MsgBoxStyle.Information)
                txtBillNo_NUM.Select()
                Exit Sub
            End If

            ''Checking Duplication
            strSql = " SELECT CASHPOINTID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            Dim CashPoint As String = objGPack.GetSqlValue(strSql).ToString
            If CashPoint <> "" Then
                strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & CashPoint & "'"
                MsgBox("This Bill Cash Already Collected from " & objGPack.GetSqlValue(strSql), MsgBoxStyle.Information)
                txtBillNo_NUM.Select()
                Exit Sub
            End If

            objAddressDia = New frmAddressDia(BillDate, BillCostId, batchNo, False)
            objAddressDia.chkSendSms.Visible = False
            strSql = " EXEC " & cnStockDb & "..SP_CASHPOINT_RECPAY"
            strSql += vbCrLf + "  @BATCHNO = '" & batchNo & "'"
            Dim dtRecPay As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtRecPay)
            If dtRecPay.Rows.Count = 0 Then
                MsgBox("Invalid BillInfo", MsgBoxStyle.Information)
                txtBillNo_NUM.Select()
                Exit Sub
            End If
            Dim RecAmt As Decimal = Nothing
            For cnt As Integer = 0 To dtRecPay.Rows.Count - 1
                For Each col As DataColumn In dtGrid.Columns
                    dtGrid.Rows(cnt).Item(col.ColumnName) = dtRecPay.Rows(cnt).Item(col.ColumnName)
                Next
                RecAmt += Val(dtRecPay.Rows(cnt).Item("TAMOUNT").ToString)
            Next
            RecAmt *= -1
            'If RecAmt <> 0 Then
            RndId = (New Random).Next(10000, 99999)
            strSql = " IF OBJECT_ID('MASTER..TEMP" & RndId & "ISSUE') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ISSUE"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "RECEIPT') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "RECEIPT"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "ACCTRAN') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ACCTRAN"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "OUTSTANDING') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "OUTSTANDING"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * INTO MASTER..TEMP" & RndId & "ISSUE FROM " & cnStockDb & "..ISSUE WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            strSql += " SELECT * INTO MASTER..TEMP" & RndId & "RECEIPT FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            strSql += " SELECT * INTO MASTER..TEMP" & RndId & "ACCTRAN FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            strSql += " SELECT * INTO MASTER..TEMP" & RndId & "OUTSTANDING FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            CalcReceive()
            CalcRoundOff()
            CalcChitCard()
            CalcOthers()
            CalcCreditCard()
            CalcCheque()
            CalcAdvance()
            CalcDiscountInd() 'INDIVIDUAL ITEM DISCOUNT AND BULK DISC
            CalcDiscount() 'MULTI DISCOUNT
            strSql = " IF OBJECT_ID('MASTER..TEMP" & RndId & "ISSUE') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ISSUE"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "RECEIPT') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "RECEIPT"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "ACCTRAN') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ACCTRAN"
            strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "OUTSTANDING') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "OUTSTANDING"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            SetAddressDetails()

            btnSave.Enabled = True
            'Else
            'MsgBox("Invalid BillInfo", MsgBoxStyle.Information)
            'txtBillNo_NUM.Select()
            'Exit Sub
            'End If
            If Def_Focus = "DISCOUNT" Then
                txtAdjDiscount_AMT.Select()
                txtAdjDiscount_AMT.SelectAll()
            Else
                txtAdjCash_AMT.Select()
                txtAdjCash_AMT.SelectAll()
            End If
        End If
    End Sub
    Private Sub frmEditModeOfPayment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _CounterWiseCashMaintanance Then
            CASHID = objGPack.GetSqlValue("SELECT CASHACCODE FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & BillCashCounterId & "' AND ISNULL(CASHACCODE,'') <> ''", , GetAdmindbSoftValue("CASH", "CASH"))
        End If
        objAddressDia.dtpAddressDueDate.MinimumDate = BillDate.Date.ToString("yyyy-MM-dd")
        objAddressDia.dtpAddressDueDate.Value = BillDate.Date.ToString("yyyy-MM-dd")
        objAddressDia.dtpAddressDueDate.Enabled = False
        objAddressDia.lblAddressDueDate.Enabled = False

        gridView.ColumnHeadersVisible = False
        gridView.RowHeadersVisible = False
        gridView.RowTemplate.Height = 30
        gridView.BackgroundColor = Color.Lavender
        gridView.BorderStyle = BorderStyle.None
        gridView.DefaultCellStyle.Font = New Font("VERDANA", 11, FontStyle.Bold)
        dtGrid.Columns.Add("PARTICULAR", GetType(String))
        dtGrid.Columns.Add("AMOUNT", GetType(Decimal))
        CClear()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtAdjCreditCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCreditCard_AMT.GotFocus
        If objCreditCard.Visible Then Exit Sub
        If txtAdjCreditCard_AMT.ReadOnly = True Then Exit Sub
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
        If txtAdjCheque_AMT.ReadOnly = True Then Exit Sub
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
        If txtAdjAdvance_AMT.ReadOnly = True Then Exit Sub
        objAdvance.BackColor = Me.BackColor
        objAdvance.StartPosition = FormStartPosition.CenterScreen
        objAdvance.grpAdvance.BackgroundColor = grpAdj.BackgroundColor
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
        If txtAdjDiscount_AMT.ReadOnly = True Then Exit Sub
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
        'txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjGiftVoucher_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjGiftVoucher_AMT.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjCredit_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjCredit_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtAdjCash_AMT.Focus()
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
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjAdvance_AMT.Focus()
    End Sub

    Private Sub tStripGiftVouhcer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripGiftVouhcer.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjGiftVoucher_AMT.Focus()
    End Sub

    Private Sub CashToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashToolStripMenuItem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub DiscountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscountToolStripMenuItem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjDiscount_AMT.Focus()
    End Sub

    Private Sub HandlingChargeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HandlingChargeToolStripMenuItem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjHandlingCharge_AMT.Focus()
    End Sub

    Private Sub CreditCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardToolStripMenuItem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjCreditCard_AMT.Focus()
    End Sub

    Private Sub ChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
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
            ''Checking Companyid
            strSql = " SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND COMPANYID='" & strCompanyId & "'"
            If Val(objGPack.GetSqlValue(strSql, , , )) = 0 Then
                MsgBox("Company mismatch for this Bill No", MsgBoxStyle.Information)
                btnSave.Focus()
                Exit Sub
            End If


            Dim objDenom As Denomination = Nothing
            If GetAdmindbSoftValue("DENOMINATION_CASHPOINT", "N") = "Y" Then
                objDenom = New Denomination
                objDenom.txtBillAmount.Text = Format(Val(txtAdjCash_AMT.Text), "0.00")
                If objDenom.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            If GetAdmindbSoftValue("DENOMINATION_CASHPOINT", "N", tran) = "Y" Then
                strSql = " DELETE FROM " & cnStockDb & "..DENOMTRAN"
                strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                objDenom.InsertDenomTran(batchNo, BillDate, BillCostId, tran)
            End If
            objAddressDia.DeleteAndInsert(tran)
            InsertAccountsDetail()

            'strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CASHID = '" & BillCashCounterId & "',CASHPOINTID = '" & BillCashCounterId & "',USERID='" & userId & "'"
            'strSql += " WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET  CASHPOINTID = '" & BillCashCounterId & "',USERID='" & userId & "'"
            strSql += " WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

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
            Dim msg As String = "Successfully Saved.." + vbCrLf
            MsgBox(msg)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":CXP")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & batchNo)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & BillDate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":CXP" & ";" & _
                    LSet("BATCHNO", 15) & ":" & batchNo & ";" & _
                    LSet("TRANDATE", 15) & ":" & BillDate.ToString("yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If

            'If objAddressDia.chkCreateNewAcc.Checked Then
            '    If objAddressDia.txtAddressPrevilegeId.Text <> "" Then msg += "Previlege Id      : " + objAddressDia.txtAddressPrevilegeId.Text + vbCrLf
            '    msg += "Party Code       : " + objAddressDia.txtAddressPartyCode.Text + vbCrLf
            '    msg += vbCrLf + "Generated.."
            'End If

            CClear()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Function InsertAccountsDetail() As Boolean
        Dim DrContra As String = objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'C' ORDER BY SUBSTRING(SNO,6,10)", , , tran)
        Dim CrContra As String = objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & BillDate.Date.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'D' ORDER BY SUBSTRING(SNO,6,10)", , , tran)
        Dim outAccode As String = objAddressDia.txtAddressPartyCode.Text
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
        strSql += " ,'DU'"
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " select top 1 runno FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND TRANTYPE IN('A','D')"
        Dim ORunNo As String = ""
        ORunNo = objGPack.GetSqlValue(strSql, , , tran)

        strSql = " DELETE FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND TRANTYPE IN('A','D')"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

        ''Cash Transaction
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C"), _
        CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", IIf(Val(txtAdjCash_AMT.Text) > 0, DrContra, CrContra))

        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
        IIf(objAddressDia.txtAddressPartyCode.Text = "", "DRS", objAddressDia.txtAddressPartyCode.Text), _
        Val(txtAdjCredit_AMT.Text), 0, 0, 0, "DU", outAccode)

        If Val(txtAdjCredit_AMT.Text) <> 0 Then

            Dim RunNo As String = ORunNo
            If RunNo = "" Then
                RunNo = "D" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + GetCreditSalesBillNo.ToString
            End If
            If objAddressDia.txtAddressPartyCode.Text = "" Then outAccode = "DRS"
            InsertIntoOustanding(TRANNO, "D", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & RunNo, Val(txtAdjCredit_AMT.Text), IIf(Val(txtAdjReceive_AMT.Text) > 0, "P", "R"), "DU", , , , , , , , , , objAddressDia.dtpAddressDueDate.Value.Date.ToString("yyyy-MM-dd"), , , outAccode)
        End If

        ''Advance Trans
        For Each ro As DataRow In objAdvance.dtGridAdvance.Rows
            InsertIntoAccTran(TRANNO, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE"), Val(ro!AMOUNT.ToString), 0, 0, 0, "AA", IIf(Val(ro!AMOUNT.ToString) > 0, DrContra, CrContra), , , , ro!DATE.ToString)
            If objAddressDia.txtAddressPartyCode.Text = "" Then outAccode = "ADVANCE"
            InsertIntoOustanding(TRANNO, "A", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, Val(ro!AMOUNT.ToString), "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , , , outAccode)
        Next

        '' ''SCHEME TRANS
        'If Val(txtAdjChitCard_AMT.Text) <> 0 Then
        '    If objChitCard.InsertChitCardDetail(BatchNo, TRANNO, BillDate, BillCashCounterId, BillCostId, VATEXM, tran) Then Return True
        'End If

        'InsertIntoAccTRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
        'IIf(objAddressDia.txtAddressPartyCode.Text = "", "DRS", objAddressDia.txtAddressPartyCode.Text), _
        'Val(txtAdjCredit_AMT.Text), 0, 0, 0, "DU", ContraCode)



        ''CreditCard Trans
        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows
            InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
            objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, "CC", IIf(Val(txtAdjReceive_AMT.Text) > 0, DrContra, CrContra), _
            , , ro!CARDNO.ToString, ro!DATE.ToString, _
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            ro!APPNO.ToString _
            )

            Dim commision As Double = Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
                objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, "BC", "BANKC", _
                , , , , )

                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
                "BANKC" _
                , commision, 0, 0, 0, "BC", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                , , , , )

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
                    objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , sCharge, 0, 0, 0, "BS", "BANKS", _
                    , , , , )

                    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
                    "BANKS" _
                    , sCharge, 0, 0, 0, "BS", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                    , , , , )
                End If
            End If
        Next

        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), _
            Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", IIf(Val(txtAdjReceive_AMT.Text) > 0, DrContra, CrContra), _
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
        Next

        ' ''Gift Voucher
        'For Each ro As DataRow In dtGridGiftVouhcer.Rows
        '    InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "D", "C"), _
        '    objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
        '    Val(ro!AMOUNT.ToString), 0, 0, 0, "GV", ContraCode, _
        '    , , , , , ro!REMARK.ToString)
        'Next

        ''Handling Charges
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
        "HANDC", Val(txtAdjHandlingCharge_AMT.Text), 0, 0, 0, "HC", IIf(Val(txtAdjReceive_AMT.Text) > 0, CrContra, DrContra))
        ''Discount Trans
        Dim mulDisc As Double = Nothing
        If GetAdmindbSoftValue("MULTIDISCOUNT", , tran).ToUpper = "Y" Then
            For Each ro As DataRow In objMultiDiscount.dtGridDisc.Rows
                mulDisc += Val(ro!AMOUNT.ToString)
                InsertIntoAccTran(TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C"), _
                objGPack.GetSqlValue("SELECT DISCID FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & ro!DISCNAME.ToString & "'", , , tran) _
                , Val(ro!AMOUNT.ToString), 0, 0, 0, "DI", IIf(Val(txtAdjDiscount_AMT.Text) > 0, DrContra, CrContra))
            Next
        End If
        InsertIntoAccTran(TRANNO, IIf(Val(txtAdjDiscount_AMT.Text) > 0, "D", "C") _
        , "DISC", Val(txtAdjDiscount_AMT.Text) - mulDisc, 0, 0, 0, "DI", IIf(Val(txtAdjDiscount_AMT.Text) > 0, DrContra, CrContra))

        ' ''Rountd Off Trans
        'InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D"), _
        '"RNDOFF", Val(txtAdjRoundoff_AMT.Text), 0, 0, 0, "RO", ContraCode)

        ''UPDATE CONTRA
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & BillDate.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & batchNo & "' AND TRANMODE = 'C' AND ISNULL(CONTRA,'') = ''"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Function

    Public Function GetCreditSalesBillNo() As Integer
GetNewBillNo:
        Dim NewBillNo As Integer = Nothing
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO'"
        If objGPack.GetSqlValue(strsql, , , tran).Length > 0 Then 'TEMPBILLNO EXIST
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = 'GEN-CRSALESBILLNO'"
            If objGPack.GetSqlValue(strsql, , , tran).Length > 0 Then 'BILLNO ALREADY GENERATED
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnAdminDb & "..TEMP" & systemId & "BILLNO WHERE CTLID = 'GEN-CRSALESBILLNO'"
                NewBillNo = Val(objGPack.GetSqlValue(strsql, , , tran))
                Return NewBillNo
            Else 'NEWBILLNO GENERATING HERE
                GoTo GenerateNewBillNo
            End If
        Else 'TEMPBILLNO NOT EXIST
GenerateNewBillNo:
            strsql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-CRSALESBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            NewBillNo = Val(objGPack.GetSqlValue(strsql, , , tran))
ReGenBillNo:
            strsql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NewBillNo + 1).ToString & "'"
            strsql += " WHERE CTLID ='GEN-CRSALESBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            strsql += " AND CONVERT(INT,CTLTEXT) = '" & NewBillNo & "'"
            cmd = New OleDbCommand(strsql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GetNewBillNo
            End If
            NewBillNo += 1
        End If
        Dim bPrefix As String = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "D" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & bPrefix & NewBillNo.ToString & "' "
        ''strSql += " AND RECPAY = 'P'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
            NewBillNo += 1
            GoTo GenerateNewBillNo
        End If
        InsertBillNo("GEN-CRSALESBILLNO", NewBillNo, tran)
        Return NewBillNo
    End Function


    Private Sub InsertIntoOustanding _
    ( _
    ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
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
    Optional ByVal Accode As String = Nothing _
        )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,PAYMODE,FROMFLAG)"
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
        strSql += " ,'P'" ' PAYMODE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Sub

    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer, _
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
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,APPVER,COMPANYID"
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
        strSql += " ,'" & VATEXM & "'" 'VATEXM
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
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
        gridView.DataSource = dtGrid
        gridView.Rows(0).Selected = False
        gridView.Columns("PARTICULAR").Width = 400
        gridView.Columns("AMOUNT").Width = 155

        gridView.Rows(1).Cells("AMOUNT").Style.ForeColor = Color.Red
        gridView.Rows(2).Cells("AMOUNT").Style.ForeColor = Color.Red
        gridView.Rows(4).Cells("AMOUNT").Style.ForeColor = Color.Red

        gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        objAdvance = New frmAdvanceAdj(BillCostId)
        objCreditCard = New frmCreditCardAdj
        objCheaque = New frmChequeAdj
        objAddressDia = New frmAddressDia
        objMultiDiscount = New frmBillMultiDiscount
    End Sub

    Private Sub CClear()
        Initi()
        objGPack.TextClear(Me)
        btnSave.Enabled = True
        txtBillNo_NUM.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        CClear()
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub Label46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label46.Click

    End Sub

    Private Sub credittsmenuitem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles credittsmenuitem.Click
        If CASHPTREADONLY = "Y" Then Exit Sub
        txtAdjCredit_AMT.Focus()
    End Sub
End Class