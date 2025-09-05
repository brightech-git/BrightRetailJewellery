Imports System.Data.OleDb
Public Class frmGiftGenerate
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim batchNo As String
    Dim objGiftDet As New frmGiftDetail

    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()

    Public objSoftKeys As New SoftKeys

    Dim TRANNO As Integer
    Public BillCostId As String
    Public BillDate As Date = GetServerDate()
    Dim VATEXM As String
    Dim RndId As Integer
    Dim CASHPTREADONLY As String
    Dim dtGrid As New DataTable
    Dim dtGridTot As New DataTable
    Dim AddressEdit As Boolean = False
    Dim objMultiDiscount As New frmBillMultiDiscount
    Dim objAdvance As New frmAdvanceAdj(BillCostId)
    Dim objCreditCard As New frmCreditCardAdj
    Dim objCheaque As New frmChequeAdj

    Dim GVVALIDVALUE As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "GV_VALIDVALUE", "0"))
    Dim gvperonsale As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "GV_PRECENTONSALE", 0))
    Dim Rec_Amt As String
    Dim Chit_Amt As String
    Dim GiftVoucher_Amt As String
    Dim Handling_Amt As String
    Dim Credit_Amt As String
    Dim IndDisc_Amt As String
    Dim AdjDisc_Amt As String
    Dim Advance_Amt As String
    Dim CreditCard_Amt As String
    Dim Cheque_Amt As String
    Dim Round_Amt As String
    Dim Cash_amt As String
    Dim RowCount As Integer = 0
    Dim Psno As String = ""

    Private Sub Initi()
        With dtGrid.Columns
            .Add("BILLNO", GetType(String))
            .Add("BILLAMT", GetType(Decimal))
            .Add("CASH", GetType(Decimal))
            .Add("CARD", GetType(Decimal))
            .Add("CHEQUE", GetType(Decimal))
            .Add("SCHEME", GetType(Decimal))
            .Add("ADVANCE", GetType(Decimal))
            .Add("CREDIT", GetType(Decimal))
            .Add("GIFTVOUCHER", GetType(Decimal))
            .Add("HAND_CHARG", GetType(Decimal))
            .Add("DISCOUNT", GetType(Decimal))
            .Add("ELIGIBLE_AMT", GetType(Decimal))
        End With
        gridView.DataSource = dtGrid

        dtGridTot = New DataTable
        dtGridTot = dtGrid.Copy
        dtGridTot.Rows.Clear()
        dtGridTot.Rows.Add()
        gridViewTotal.ColumnHeadersVisible = False
        gridViewTotal.DataSource = dtGridTot

        showgrid(gridView)
        showgrid(gridViewTotal)

    End Sub
    Function showgrid(ByVal gridView As DataGridView)

        With gridView
            .Columns("BILLNO").Width = 70
            .Columns("BILLAMT").Width = 70
            .Columns("CASH").Width = 70
            .Columns("CARD").Width = 70
            .Columns("CHEQUE").Width = 70
            .Columns("SCHEME").Width = 70
            .Columns("ADVANCE").Width = 70
            .Columns("CREDIT").Width = 70
            .Columns("HAND_CHARG").Width = 90
            .Columns("HAND_CHARG").HeaderText = "HANDCHARG"
            .Columns("DISCOUNT").Width = 70
            .Columns("GIFTVOUCHER").Width = 70
            .Columns("GIFTVOUCHER").HeaderText = "ADJ_GIFT"
            .Columns("ELIGIBLE_AMT").Width = 110
            .Columns("ELIGIBLE_AMT").HeaderText = "PAYABLEAMT"

            For i As Integer = 1 To .Columns.Count - 1
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next
        End With
    End Function

    Private Sub CalcReceive()
        strSql = " SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT AMOUNT,PAYMODE,TRANMODE FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strSql += vbCrLf + "  WHERE BATCHNO = '" & batchNo & "'"
        strSql += vbCrLf + "  AND PAYMODE IN "
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "   'SA','SV','SR','RV','PU','PV','SE'  "
        strSql += vbCrLf + "   ,'DR','AR','MR','HR','OR'  "
        strSql += vbCrLf + "   ,'DP','AP','MP','HP','HG','HB','HZ','HD'"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  )X"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql))
        Rec_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcRoundOff()
        strSql = " SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'RO'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql))
        Round_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
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
        Chit_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
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
        Credit_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
        'GV'
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", "PAYMODE = 'GV'").ToString)
        GiftVoucher_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
        ''HC
        amt = Val(dtTemp.Compute("SUM(AMOUNT)", "PAYMODE = 'HC'").ToString)
        Handling_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcDiscountInd()
        strSql = " SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) FROM MASTER..TEMP" & RndId & "ACCTRAN"
        strSql += " WHERE BATCHNO = '" & batchNo & "'"
        strSql += " AND PAYMODE = 'DI' AND ACCODE = 'DISC'"
        Dim amt As Double = Nothing
        amt = Val(objGPack.GetSqlValue(strSql))
        IndDisc_Amt = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub
    Private Sub CalcDiscount()
        strSql = " SELECT "
        strSql += " (SELECT DISCNAME FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCID = T.ACCODE)AS DISCNAME"
        strSql += " ,AMOUNT"
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN T"
        strSql += " WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'DI' AND ACCODE <> 'DISC'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objMultiDiscount.dtGridDisc)
        objMultiDiscount.CalcGridDiscTotal()
        Dim discAmt As Double = Val(objMultiDiscount.gridDiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        discAmt += Val(AdjDisc_Amt)
        AdjDisc_Amt = IIf(discAmt <> 0, Format(discAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcAdvance()
        strSql = " SELECT SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE DATE"
        strSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + "  WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS RECEIVEDAMT"
        strSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + "  WHERE RUNNO = O.RUNNO AND BATCHNO <> O.BATCHNO AND ISNULL(CANCEL,'') = '')AS ADJUSTEDAMT"
        strSql += vbCrLf + "  ,AMOUNT"
        strSql += vbCrLf + "  FROM MASTER..TEMP" & RndId & "OUTSTANDING AS O"
        strSql += vbCrLf + "  WHERE BATCHNO = '" & batchNo & "' AND TRANTYPE = 'A' AND PAYMODE = 'AA'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objAdvance.dtGridAdvance)
        objAdvance.CalcGridAdvanceTotal()
        Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        Advance_Amt = IIf(advAmt <> 0, Format(advAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcCreditCard()
        strSql = " SELECT"
        strSql += " (SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = T.CARDID)AS CARDTYPE"
        strSql += " ,CHQDATE DATE"
        strSql += " ,AMOUNT"
        strSql += " ,CHQCARDNO AS CARDNO"
        strSql += " ,CONVERT(VARCHAR,'')APPNO"
        strSql += " ,(SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = T.CARDID)COMMISION"
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CC'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCreditCard.dtGridCreditCard)
        objCreditCard.CalcGridCreditCardTotal()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        CreditCard_Amt = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcCheque()
        strSql = " SELECT CHQCARDREF BANKNAME,CHQDATE DATE,CHQCARDNO CHEQUENO,AMOUNT"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)aS DEFAULTBANK"
        strSql += " FROM MASTER..TEMP" & RndId & "ACCTRAN AS T WHERE BATCHNO = '" & batchNo & "' AND PAYMODE = 'CH'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objCheaque.dtGridCheque)
        objCheaque.CalcGridChequeTotal()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        Cheque_Amt = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtBillNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBillNo_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            gridView.Focus()
        End If
    End Sub
  
    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtBillNo_NUM.Text) = 0 Then Exit Sub
            For Each ro As DataRow In dtGrid.Rows
                If ro.Item("BILLNO").ToString = txtBillNo_NUM.Text Then
                    MsgBox("This Bill Already Loaded", MsgBoxStyle.Information)
                    txtBillNo_NUM.Clear()
                    Exit Sub
                End If
            Next
            'Initi()
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
                Dim row() As DataRow = dtBillInfo.Select("BATCHNO='" & batchNo & "'", Nothing)
                If Not row Is Nothing Then
                    Psno = row(0).Item("SNO").ToString
                End If
            Else
                batchNo = dtBillInfo.Rows(0).Item("BATCHNO").ToString
                Psno = dtBillInfo.Rows(0).Item("SNO").ToString
            End If
            If batchNo = "" Then
                Exit Sub
            End If
            TRANNO = Val(txtBillNo_NUM.Text)
    
            strSql = " EXEC " & cnStockDb & "..SP_CASHPOINT_RECPAY"
            strSql += vbCrLf + "  @BATCHNO = '" & batchNo & "'"
            Dim dtRecPay As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtRecPay)
            Dim RecAmt As Decimal = Nothing
            For cnt As Integer = 0 To dtRecPay.Rows.Count - 1
                'For Each col As DataColumn In dtGrid.Columns
                '    dtGrid.Rows(cnt).Item(col.ColumnName) = dtRecPay.Rows(cnt).Item(col.ColumnName)
                'Next
                RecAmt += Val(dtRecPay.Rows(cnt).Item("TAMOUNT").ToString)
            Next
            RecAmt *= -1
            If RecAmt <> 0 Then
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
                CalcFinalAmount()

                Dim EligibleAmt As Decimal

                Dim ro As DataRow = Nothing
                ro = dtGrid.NewRow
                ro("BILLNO") = txtBillNo_NUM.Text
                ro("BILLAMT") = IIf(Val(RecAmt) <> 0, Format(Val(RecAmt), "#0.00"), DBNull.Value)
                ro("CASH") = IIf(Val(Cash_amt) <> 0, Format(Val(Cash_amt), "#0.00"), DBNull.Value)
                ro("CARD") = IIf(Val(CreditCard_Amt) <> 0, Format(Val(CreditCard_Amt), "#0.00"), DBNull.Value)
                ro("CHEQUE") = IIf(Val(Cheque_Amt) <> 0, Format(Val(Cheque_Amt), "#0.00"), DBNull.Value)
                ro("SCHEME") = IIf(Val(Chit_Amt) <> 0, Format(Val(Chit_Amt), "#0.00"), DBNull.Value)
                ro("ADVANCE") = IIf(Val(Advance_Amt) <> 0, Format(Val(Advance_Amt), "#0.00"), DBNull.Value)
                ro("CREDIT") = IIf(Val(Credit_Amt) <> 0, Format(Val(Credit_Amt), "#0.00"), DBNull.Value)
                ro("GIFTVOUCHER") = IIf(Val(GiftVoucher_Amt) <> 0, Format(Val(GiftVoucher_Amt), "#0.00"), DBNull.Value)
                ro("HAND_CHARG") = IIf(Val(Handling_Amt) <> 0, Format(Val(Handling_Amt), "#0.00"), DBNull.Value)
                ro("DISCOUNT") = IIf(Val(AdjDisc_Amt) <> 0, Format(Val(AdjDisc_Amt), "#0.00"), DBNull.Value)
                If Val(GiftVoucher_Amt) > 0 Then
                    EligibleAmt = Rec_Amt - (GiftVoucher_Amt * GVVALIDVALUE)
                Else
                    EligibleAmt = Rec_Amt
                End If
                ro("ELIGIBLE_AMT") = IIf(Val(Rec_Amt) <> 0, Format(Val(Rec_Amt), "#0.00"), DBNull.Value)

                dtGrid.Rows.Add(ro)
                dtGrid.AcceptChanges()
                gridView.CurrentCell = gridView.Rows(gridView.RowCount - 1).Cells(1)
                CalcGridTotal()

                'If Val(dtGridTot.Rows(0).Item("ELIGIBLE_AMT").ToString) > 0 Then
                '    btnGenerate.Enabled = True
                'End If
                strSql = " IF OBJECT_ID('MASTER..TEMP" & RndId & "ISSUE') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ISSUE"
                strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "RECEIPT') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "RECEIPT"
                strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "ACCTRAN') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "ACCTRAN"
                strSql += " IF OBJECT_ID('MASTER..TEMP" & RndId & "OUTSTANDING') IS NOT NULL DROP TABLE MASTER..TEMP" & RndId & "OUTSTANDING"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                ClearVal()
                txtBillNo_NUM.Focus()

            Else
                MsgBox("Invalid BillInfo", MsgBoxStyle.Information)
                txtBillNo_NUM.Select()
                Exit Sub
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            'If objGiftDet.Visible Then Exit Sub
            objGiftDet.BackColor = Me.BackColor
            objGiftDet.StartPosition = FormStartPosition.CenterScreen
            objGiftDet.MaximizeBox = False
            Dim amt As Double = Nothing
            amt = Val(dtGridTot.Rows(0).Item("ELIGIBLE_AMT").ToString)

            objGiftDet.txtPayableAmt.Text = amt
            objGiftDet.txtGiftValPer.Text = gvperonsale & " %"
            objGiftDet.txtGiftAmt.Text = CalcRoundoffAmt((amt * (gvperonsale / 100)), objSoftKeys.RoundOff_Gross)
            objGiftDet.cmbGvName.Focus()
            If objGiftDet.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Funcadd()
            Else
                txtBillNo_NUM.Focus()
            End If
        End If
    End Sub

    Private Sub CalcGridTotal()

        Dim TotEligibleAmt As Decimal = Nothing
        Dim TotAmt As Decimal = Nothing
        Dim TotCash_amt As Decimal = Nothing
        Dim TotCreditCard_Amt As Decimal = Nothing
        Dim TotCheque_Amt As Decimal = Nothing
        Dim TotChit_Amt As Decimal = Nothing
        Dim TotAdvance_Amt As Decimal = Nothing
        Dim TotCredit_Amt As Decimal = Nothing
        Dim TotHandling_Amt As Decimal = Nothing
        Dim TotAdjDisc_Amt As Decimal = Nothing
        Dim TotGiftVoucher_Amt As Decimal = Nothing
        Dim TotRound_Amt As Decimal = Nothing


        For i As Integer = 0 To gridView.RowCount - 1
            With gridView.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                TotAmt += Val(.Cells("BILLAMT").Value.ToString)
                TotCash_amt += Val(.Cells("CASH").Value.ToString)
                TotCreditCard_Amt += Val(.Cells("CARD").Value.ToString)
                TotCheque_Amt += Val(.Cells("CHEQUE").Value.ToString)
                TotChit_Amt += Val(.Cells("SCHEME").Value.ToString)
                TotAdvance_Amt += Val(.Cells("ADVANCE").Value.ToString)
                TotCredit_Amt += Val(.Cells("CREDIT").Value.ToString)
                TotHandling_Amt += Val(.Cells("HAND_CHARG").Value.ToString)
                TotAdjDisc_Amt += Val(.Cells("DISCOUNT").Value.ToString)
                TotGiftVoucher_Amt += Val(.Cells("GIFTVOUCHER").Value.ToString)
                TotEligibleAmt += Val(.Cells("ELIGIBLE_AMT").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next

        With gridViewTotal.Rows(0)
            .Cells("BILLNO").Value = "TOTAL"
            .Cells("BILLAMT").Value = IIf(TotAmt <> 0, Format(TotAmt, "#0.00"), DBNull.Value)
            .Cells("CASH").Value = IIf(TotCash_amt <> 0, Format(TotCash_amt, "#0.00"), DBNull.Value)
            .Cells("CARD").Value = IIf(TotCreditCard_Amt <> 0, Format(TotCreditCard_Amt, "#0.00"), DBNull.Value)
            .Cells("CHEQUE").Value = IIf(TotCheque_Amt <> 0, Format(TotCheque_Amt, "#0.00"), DBNull.Value)
            .Cells("SCHEME").Value = IIf(TotChit_Amt <> 0, Format(TotChit_Amt, "#0.00"), DBNull.Value)
            .Cells("ADVANCE").Value = IIf(TotAdvance_Amt <> 0, Format(TotAdvance_Amt, "#0.00"), DBNull.Value)
            .Cells("CREDIT").Value = IIf(TotCredit_Amt <> 0, Format(TotCredit_Amt, "#0.00"), DBNull.Value)
            .Cells("HAND_CHARG").Value = IIf(TotHandling_Amt <> 0, Format(TotHandling_Amt, "#0.00"), DBNull.Value)
            .Cells("DISCOUNT").Value = IIf(TotAdjDisc_Amt <> 0, Format(TotAdjDisc_Amt, "#0.00"), DBNull.Value)
            .Cells("GIFTVOUCHER").Value = IIf(TotGiftVoucher_Amt <> 0, Format(TotGiftVoucher_Amt, "#0.00"), DBNull.Value)
            .Cells("ELIGIBLE_AMT").Value = IIf(TotEligibleAmt <> 0, Format(TotEligibleAmt, "#0.00"), DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.Blue
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub ClearVal()
        Rec_Amt = Nothing
        Chit_Amt = Nothing
        GiftVoucher_Amt = Nothing
        Handling_Amt = Nothing
        Credit_Amt = Nothing
        IndDisc_Amt = Nothing
        AdjDisc_Amt = Nothing
        Advance_Amt = Nothing
        CreditCard_Amt = Nothing
        Cheque_Amt = Nothing
        Round_Amt = Nothing
        objAdvance = New frmAdvanceAdj(BillCostId)
        objCreditCard = New frmCreditCardAdj
        objCheaque = New frmChequeAdj
        objMultiDiscount = New frmBillMultiDiscount
        txtBillNo_NUM.Text = ""
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Public Function CalcRoundoffAmt(ByVal amt As Double, ByVal type As String) As Double
        Select Case type
            Case "L"
                Return Math.Floor(amt)
            Case "F"
                If Math.Abs(amt - Math.Floor(amt)) >= 0.5 Then
                    Return Math.Ceiling(amt)
                Else
                    Return Math.Floor(amt)
                End If
            Case "H"
                Return Math.Ceiling(amt)
            Case Else
                Return amt
        End Select
        Return amt
    End Function

    Private Sub CalcFinalAmount()
        Dim ActRec As Double = Val(Rec_Amt) ' + Val(txtAdjSrCredit_AMT.Text)
        Dim cash As Double = Nothing
        cash += Val(Rec_Amt) _
              - IIf(Val(Rec_Amt) > 0, Val(Round_Amt), -1 * Val(Round_Amt)) _
              - Val(Advance_Amt) _
              - IIf(ActRec > 0, Val(Credit_Amt), -1 * Val(Credit_Amt)) _
              - IIf(ActRec > 0, Val(GiftVoucher_Amt), -1 * Val(GiftVoucher_Amt)) _
              - Val(Chit_Amt) _
              - IIf(ActRec > 0, Val(Cheque_Amt), -1 * Val(Cheque_Amt)) _
              - Val(CreditCard_Amt) _
              + IIf(ActRec > 0, Val(Handling_Amt), -1 * Val(Handling_Amt)) _
              - Val(AdjDisc_Amt)
        Cash_amt = IIf(cash <> 0, Format(cash, "0.00"), Nothing)
    End Sub

    Private Sub CClear()
        gridView.DataSource = Nothing
        gridViewTotal.DataSource = Nothing
        dtGrid = New DataTable
        dtGridTot = New DataTable
        objGPack.TextClear(Me)
        'cmbGvName.Text = ""
        Initi()
        txtBillNo_NUM.Select()
        Psno = ""
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        CClear()
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGrid.AcceptChanges()
        CalcGridTotal()
    End Sub

    Private Sub gridView_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridView.UserDeletingRow
        If gridView.Rows(e.Row.Index).Cells("BILLNO").Value.ToString <> "" Then
            For Each ro As DataRow In dtGrid.Rows
                If ro("BILLNO") = e.Row.Cells("BILLNO").Value Then
                    ro.Delete()
                End If
            Next
            dtGrid.AcceptChanges()
        End If
    End Sub

    Private Sub frmGiftGenerate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtBillNo_NUM.Focused Then Exit Sub
            'If btnGenerate.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub BtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnNew.Click
        CClear()
    End Sub

    Private Sub frmGiftGenerate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CClear()
    End Sub

    Function Funcadd()
        Try

            If objGiftDet.cmbGvName.Text = "" Then
                MsgBox("Select GiftVoucherName")
                If objGiftDet.Visible Then Exit Function
                objGiftDet.BackColor = Me.BackColor
                objGiftDet.StartPosition = FormStartPosition.CenterScreen
                objGiftDet.MaximizeBox = False
                If objGiftDet.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                    Exit Function
                End If
            End If
            tran = Nothing
            tran = cn.BeginTransaction()

            Dim gvitem As DataRow = GetSqlRow("SELECT CARDCODE,ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & objGiftDet.cmbGvName.Text & "'", cn, tran)
            Dim GVRACCODE As String = gvitem.Item(1).ToString
            Dim Gvid As String = gvitem.Item(0).ToString

            Dim amt As Double = Nothing
            amt = Val(dtGridTot.Rows(0).Item("ELIGIBLE_AMT").ToString)

            Dim runno As String = GetGvNo(tran, True)
            Dim TNO As String = Nothing
            strSql = " SELECT CTLTEXT AS GVNOS FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GVNUMBER'"
            TNO = objGPack.GetSqlValue(strSql, , "1", tran)
            Dim gvamt As Double = Nothing
            gvamt = Val(objGiftDet.txtGiftAmt.Text)

            InsertIntoOustanding(TNO, "GV", runno, gvamt, "R", "GV", , , , 0, gvamt, Gvid, , , , , "", , GVRACCODE)
            tran.Commit()
            tran = Nothing
            Dim msg As String
            msg = "Gift Voucher BillNo : " + TNO.ToString + vbCrLf
            msg += vbCrLf + "Generated.."
            MsgBox(msg, MsgBoxStyle.Information)

            CClear()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
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
 Optional ByVal Accode As String = Nothing, _
 Optional ByVal Flag As String = Nothing, _
 Optional ByVal EmpId As Integer = Nothing, _
 Optional ByVal PureWt As Double = Nothing, _
 Optional ByVal Advwtper As Double = Nothing _
     )
        batchNo = GetNewBatchno(cnCostId, BillDate, tran)

        If Amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,ADVFIXWTPER,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
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
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Advwtper & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,''" 'CASHID
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
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
        strSql += " ,'" & Paymode & "'"
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

        strSql = "INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += " (PSNO,BATCHNO,COSTID)"
        strSql += " VALUES("
        strSql += " '" & Psno & "'" 'PSNO
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Sub
End Class