Imports System.IO
Imports system.Data.OleDb


Public Class BillCollection
    Public objSoftKeys As New SoftKeys
    Dim StrSql As String
    Public LockAdjustment As Boolean
    Public ObjConvertValue As New BillConvertValue
    Public BalWeight As Double
    Public BalAmount As Double
    Public SettleWeight As Decimal
    Public SettleAmount As Decimal

    Private _Accode As String
    Private Tranno As Integer
    Private Batchno As String
    Private BillCostId As String
    Private BillCashCounterId As String
    Private BillDate As Date
    Private _PureCatCode As String
    Private FromEntry As String

    Dim objCreditCard As New frmCreditCardAdj
    Dim objChitCard As New frmChitAdj
    Dim objAdvance As New frmAdvanceAdj(BillCostId)
    Dim objCheaque As New frmChequeAdj
    Dim da As OleDbDataAdapter

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        _PureCatCode = "00001"

        objCreditCard = New frmCreditCardAdj
        objChitCard = New frmChitAdj
        objCheaque = New frmChequeAdj
    End Sub
    Private Sub BillCollection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.F4 Then
            txtCash_AMT.Focus()
        ElseIf e.KeyCode = Keys.F5 Then
            txtDiscAmount_AMT.Focus()
        ElseIf e.KeyCode = Keys.F6 Then
            txtAdjHandlingCharge_AMT.Select()
        ElseIf e.KeyCode = Keys.F7 Then
            txtAdjCreditCard_AMT.Select()
        ElseIf e.KeyCode = Keys.F8 Then
            txtAdjCheque_AMT.Select()
        ElseIf e.KeyCode = Keys.F9 Then
            txtAdjChitCard_AMT.Select()
        End If
    End Sub

    Private Sub BillCollection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEmpId_MAN.Focused Then Exit Sub
            If txtCash_AMT.Focused _
            Or txtDiscAmount_AMT.Focused _
            Or txtAdjHandlingCharge_AMT.Focused _
            Or txtAdjCreditCard_AMT.Focused _
            Or txtAdjCheque_AMT.Focused _
            Or txtAdjChitCard_AMT.Focused Then
                txtBalAmount_AMT.Focus()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub CalcFinalAmount()
        Dim finalAmt As Double = Nothing
        finalAmt = Val(txtCutWeight_WET.Text) * Val(txtCutRate_AMT.Text)
        txtCutAmount_AMT.Text = IIf(finalAmt <> 0, Format(finalAmt, "0.00"), "")
        CalcBalWtAmt()
    End Sub
    Private Sub CalcTotalWeight()
        Dim totWt As Double = Nothing
        totWt = Val(txtDueWeight_WET.Text) + Val(ObjConvertValue.txtConvertWeight_WET.Text)
        If chkDiscount.Checked Then
            totWt -= Val(txtDiscWeight_WET.Text)
        Else
            totWt += Val(txtDiscWeight_WET.Text)
        End If
        txtTotWeight_WET.Text = IIf(totWt <> 0, Format(totWt, "0.000"), "")
        CalcBalWtAmt()
    End Sub
    Public Sub CalcBalWtAmt()
        Dim balWt As Double = Nothing
        Dim balAmt As Double = Nothing
        balWt = Val(txtTotWeight_WET.Text) + Val(ObjConvertValue.txtConvertWeight_WET.Text)
        If Val(txtCutRate_AMT.Text) > 0 Then
            balWt -= Val(txtCutWeight_WET.Text)
        End If
        balAmt = Val(txtCutAmount_AMT.Text)
        If chkDiscount.Checked Then
            balAmt -= Val(txtDiscAmount_AMT.Text)
        Else
            balAmt += Val(txtDiscAmount_AMT.Text)
        End If
        balAmt = balAmt - Val(txtCash_AMT.Text) + Val(txtAdjHandlingCharge_AMT.Text) - Val(txtAdjCreditCard_AMT.Text) - Val(txtAdjCheque_AMT.Text) - Val(txtAdjChitCard_AMT.Text)
        balAmt += Val(txtDueAmount_AMT.Text) - Val(ObjConvertValue.txtConvertValue_AMT.Text)
        txtBalWeight_WET.Text = IIf(balWt <> 0, Format(balWt, "0.000"), "")
        txtBalAmount_AMT.Text = IIf(balAmt <> 0, Format(CalcRoundoffAmt(balAmt, objSoftKeys.RoundOff_Final), "0.00"), "")
        BalWeight = balWt
        BalAmount = balAmt
    End Sub

    Private Sub chkConvertAmount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkConvertAmount.CheckedChanged
        If chkConvertAmount.Checked Then

        Else
            ObjConvertValue.txtConvertWeight_WET.Clear()
            ObjConvertValue.txtConvertRate_AMT.Clear()
            ObjConvertValue.txtConvertValue_AMT.Clear()
        End If
        CalcTotalWeight()
    End Sub

    Private Sub txtDueAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueAmount_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtDueAmount_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueAmount_AMT.TextChanged
        If Val(txtDueAmount_AMT.Text) <> 0 Then
            chkConvertAmount.Visible = True
        Else
            chkConvertAmount.Visible = False
        End If

        CalcTotalWeight()
    End Sub

    Private Sub txtDueWeight_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueWeight_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtDueWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueWeight_WET.TextChanged
        CalcTotalWeight()
    End Sub

    Private Sub txtDiscWeight_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscWeight_WET.GotFocus
        If Val(txtDueWeight_WET.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtDiscWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscWeight_WET.TextChanged
        CalcTotalWeight()
    End Sub

    Private Sub txtTotWeight_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotWeight_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTotWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotWeight_WET.TextChanged
        txtCutWeight_WET.Clear()
        txtCutRate_AMT.Clear()
    End Sub

    Private Sub txtCutWeight_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutWeight_WET.GotFocus
        If Val(txtTotWeight_WET.Text) = 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtCutRate_AMT.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
        If Val(txtCutRate_AMT.Text) > 0 And Val(txtCutWeight_WET.Text) = 0 Then
            txtCutWeight_WET.Text = txtTotWeight_WET.Text
        End If
    End Sub

    Private Sub txtCutWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutWeight_WET.LostFocus
        If Val(txtCutWeight_WET.Text) <> 0 Then
            If Val(txtTotWeight_WET.Text) < 0 And Val(txtCutWeight_WET.Text) > 0 Then
                txtCutWeight_WET.Text = Format(-1 * Val(txtCutWeight_WET.Text), "0.000")
            ElseIf Val(txtTotWeight_WET.Text) > 0 And Val(txtCutWeight_WET.Text) < 0 Then
                txtCutWeight_WET.Text = Format(-1 * Val(txtCutWeight_WET.Text), "0.000")
            End If
            If Val(txtCutWeight_WET.Text) = 0 Then txtCutWeight_WET.Clear()
        End If
        If Val(txtTotWeight_WET.Text) > 0 Then
            If Val(txtTotWeight_WET.Text) < Val(txtCutWeight_WET.Text) Then
                txtCutWeight_WET.Clear()
            End If
        ElseIf Val(txtTotWeight_WET.Text) < 0 Then
            If Val(txtTotWeight_WET.Text) > Val(txtCutWeight_WET.Text) Then
                txtCutWeight_WET.Clear()
            End If
        End If
    End Sub

    Private Sub txtCutWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutWeight_WET.TextChanged
        CalcFinalAmount()
    End Sub

    Private Sub txtCutRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutRate_AMT.GotFocus
        If Val(txtTotWeight_WET.Text) = 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If chkConvertAmount.Checked Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCutRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtCutRate_AMT.Text) > 0 Then
                txtCutRate_AMT.Clear()
                txtCutWeight_WET.Clear()
            End If
        End If
    End Sub

    Private Sub txtRate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutRate_AMT.TextChanged
        CalcFinalAmount()
    End Sub

    Private Sub CalcBalWtAmt(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    txtDiscAmount_AMT.TextChanged _
    , txtAdjCheque_AMT.TextChanged _
    , txtAdjChitCard_AMT.TextChanged _
    , txtAdjCreditCard_AMT.TextChanged _
    , txtCash_AMT.TextChanged _
    , txtAdjHandlingCharge_AMT.TextChanged
        CalcBalWtAmt()
    End Sub


    Private Sub txtDiscPer_PER_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPer_PER.GotFocus
        If Val(txtDueWeight_WET.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtDiscPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscPer_PER.TextChanged
        Dim disc As Double = Nothing
        If Val(txtDiscPer_PER.Text) > 0 Then
            disc = Val(txtDueWeight_WET.Text) * (Val(txtDiscPer_PER.Text) / 100)
        End If
        txtDiscWeight_WET.Text = IIf(disc <> 0, Format(disc, "0.000"), "")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Math.Abs(BalAmount - Val(txtBalAmount_AMT.Text)) > 5 Then
            MsgBox("Invalid Balance Amount", MsgBoxStyle.Information)
            txtBalAmount_AMT.Text = IIf(BalAmount <> 0, Format(BalAmount, "0.00"), "")
            txtBalAmount_AMT.Select()
            Exit Sub
        End If
        If Math.Abs(BalWeight - Val(txtBalWeight_WET.Text)) > 5 Then
            MsgBox("Invalid Balance Weight", MsgBoxStyle.Information)
            txtBalWeight_WET.Text = IIf(BalWeight <> 0, Format(BalWeight, "0.00"), "")
            txtBalWeight_WET.Select()
            Exit Sub
        End If
        If Val(txtCutWeight_WET.Text) = 0 Then
            txtCutRate_AMT.Clear()
        End If
        If txtEmpName.Text = "" Then
            txtEmpId_MAN.Select()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtBalAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBalAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Math.Abs(BalAmount - Val(txtBalAmount_AMT.Text)) > 5 Then
                MsgBox("Invalid Balance Amount", MsgBoxStyle.Information)
                txtBalAmount_AMT.Text = IIf(BalAmount <> 0, Format(BalAmount, "0.00"), "")
                txtBalAmount_AMT.Select()
            End If
        End If
    End Sub

    Private Sub txtBalWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBalWeight_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Math.Abs(BalWeight - Val(txtBalWeight_WET.Text)) > 5 Then
                MsgBox("Invalid Balance Weight", MsgBoxStyle.Information)
                txtBalWeight_WET.Text = IIf(BalWeight <> 0, Format(BalWeight, "0.00"), "")
                txtBalWeight_WET.Select()
            End If
        End If
    End Sub

    Private Sub chkDiscount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDiscount.CheckedChanged
        If chkDiscount.Checked Then
            chkDiscount.Text = "Disc Wt"
            lblDiscount.Text = "Disc Amount"
        Else
            chkDiscount.Text = "Lp Wt"
            lblDiscount.Text = "Lp Amount"
        End If
        CalcTotalWeight()
    End Sub

    Private Sub chkConvertAmount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkConvertAmount.GotFocus
        If Val(txtDueAmount_AMT.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtCutAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutAmount_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub chkConvertAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkConvertAmount.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not chkConvertAmount.Checked Then Exit Sub
            ObjConvertValue.txtConvertValue_AMT.Text = txtDueAmount_AMT.Text
            ObjConvertValue.ShowDialog()
            If Val(ObjConvertValue.txtConvertWeight_WET.Text) = 0 Then
                chkConvertAmount.Checked = False
            End If
            CalcBalWtAmt()
            Me.SelectNextControl(chkConvertAmount, True, True, True, True)
        End If
    End Sub

    Private Sub txtBalAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBalAmount_AMT.LostFocus
        If Val(txtBalAmount_AMT.Text) <> 0 Then
            If BalAmount < 0 And Val(txtBalAmount_AMT.Text) > 0 Then
                txtBalAmount_AMT.Text = Format(-1 * Val(txtBalAmount_AMT.Text), "0.00")
            ElseIf BalAmount > 0 And Val(txtBalAmount_AMT.Text) < 0 Then
                txtBalAmount_AMT.Text = Format(-1 * Val(txtBalAmount_AMT.Text), "0.00")
            End If
            If Val(txtBalAmount_AMT.Text) = 0 Then txtBalAmount_AMT.Clear()
        End If
    End Sub

    Private Sub txtBalWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBalWeight_WET.LostFocus
        If Val(txtBalWeight_WET.Text) <> 0 Then
            If BalWeight < 0 And Val(txtBalWeight_WET.Text) > 0 Then
                txtBalWeight_WET.Text = Format(-1 * Val(txtBalWeight_WET.Text), "0.00")
            ElseIf BalWeight > 0 And Val(txtBalWeight_WET.Text) < 0 Then
                txtBalWeight_WET.Text = Format(-1 * Val(txtBalWeight_WET.Text), "0.00")
            End If
            If Val(txtBalWeight_WET.Text) = 0 Then txtBalWeight_WET.Clear()
        End If
    End Sub

    Private Sub txtEmpId_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmpId_MAN.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadEmpId(txtEmpId_MAN)
        End If
    End Sub

    Private Sub LoadEmpId(ByVal txtEmpBox As TextBox)
        StrSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        StrSql += " WHERE ACTIVE = 'Y'"
        Dim Row As DataRow = BrighttechPack.SearchDialog.Show_R("Select EmpName", StrSql, cn, 1)
        If Not Row Is Nothing Then
            txtEmpBox.Text = Row.Item("EMPID").ToString
            txtEmpName.Text = Row.Item("EMPNAME").ToString
            txtEmpBox.SelectAll()
        Else
            txtEmpBox.Clear()
            txtEmpName.Clear()
        End If
    End Sub

    Private Sub txtEmpId_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmpId_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEmpId_MAN.Text = "" Then
                LoadEmpId(txtEmpId_MAN)
                Exit Sub
            ElseIf Not Val(objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_MAN.Text) & " AND ACTIVE = 'Y'")) > 0 Then
                LoadEmpId(txtEmpId_MAN)
                Exit Sub
            Else
                StrSql = " SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
                StrSql += " WHERE ACTIVE = 'Y' AND EMPID = " & Val(txtEmpId_MAN.Text) & ""
                txtEmpName.Text = objGPack.GetSqlValue(StrSql)
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub Lock_Adjustment(ByVal sender As Object, ByVal e As EventArgs) Handles _
    chkDiscount.GotFocus _
    , txtDiscPer_PER.GotFocus _
    , txtDiscWeight_WET.GotFocus _
    , chkConvertAmount.GotFocus _
    , txtTotWeight_WET.GotFocus _
    , txtCutRate_AMT.GotFocus _
    , txtCutWeight_WET.GotFocus _
    , txtCutAmount_AMT.GotFocus _
    , txtDiscAmount_AMT.GotFocus _
    , txtBalWeight_WET.GotFocus _
    , txtBalAmount_AMT.GotFocus _
    , txtCash_AMT.GotFocus
        If LockAdjustment Then
            txtEmpId_MAN.Select()
        End If
    End Sub



    'Private Sub BillCollection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    chkDiscount.Enabled = Not LockAdjustment
    '    txtDiscPer_PER.Enabled = Not LockAdjustment
    '    txtDiscWeight_WET.Enabled = Not LockAdjustment
    '    chkConvertAmount.Enabled = Not LockAdjustment
    '    txtTotWeight_WET.Enabled = Not LockAdjustment
    '    txtCutRate_AMT.Enabled = Not LockAdjustment
    '    txtCutWeight_WET.Enabled = Not LockAdjustment
    '    txtCutAmount_AMT.Enabled = Not LockAdjustment
    '    txtDiscAmount_AMT.Enabled = Not LockAdjustment
    '    txtBalWeight_WET.Enabled = Not LockAdjustment
    '    txtBalAmount_AMT.Enabled = Not LockAdjustment
    'End Sub

    Private Sub txtEmpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtCutWeight_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCutWeight_WET.Leave
        If Val(txtCutWeight_WET.Text) = 0 Then
            txtCutRate_AMT.Clear()
        End If
    End Sub

    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer, _
    ByVal tranMode As String, _
    ByVal accode As String, _
    ByVal amount As Double, _
    ByVal pcs As Integer, _
    ByVal grsWT As Double, _
    ByVal netWT As Double, _
    ByVal pureWT As Double, _
    ByVal payMode As String, _
    ByVal contra As String, _
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As Date = Nothing, _
    Optional ByVal chqCardNo As String = Nothing, _
    Optional ByVal chqDate As Date = Nothing, _
    Optional ByVal chqCardId As Integer = Nothing, _
    Optional ByVal chqCardRef As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing, _
    Optional ByVal TranFlag As String = Nothing)
        If amount = 0 Then Exit Sub
        If TranFlag = Nothing Then TranFlag = IIf(Val(txtCutRate_AMT.Text) <> 0, "A", "W")
        Dim DtAccTran As New DataTable
        Dim Row As DataRow
        DtAccTran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", cn, tran)
        Row = DtAccTran.NewRow
        Row!SNO = GetNewSno(TranSnoType.ACCTRANCODE, tran)
        Row!TRANNO = tNo
        Row!TRANDATE = GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
        Row!TRANMODE = tranMode
        Row!ACCODE = accode
        Row!AMOUNT = Math.Abs(amount)
        Row!PCS = Math.Abs(pcs)
        Row!GRSWT = Math.Abs(grsWT)
        Row!NETWT = Math.Abs(netWT)
        Row!PUREWT = Math.Abs(pureWT)
        Row!REFNO = refNo
        If refDate <> Nothing Then Row!REFDATE = refDate
        Row!PAYMODE = payMode
        Row!CHQCARDNO = chqCardNo
        Row!CARDID = chqCardId
        Row!CHQCARDREF = chqCardRef
        If chqDate <> Nothing Then Row!CHQDATE = chqDate
        Row!BRSFLAG = DBNull.Value
        Row!RELIASEDATE = DBNull.Value
        Row!FROMFLAG = "P"
        Row!REMARK1 = Remark1
        Row!REMARK2 = Remark2
        Row!CONTRA = contra
        Row!BATCHNO = Batchno
        Row!USERID = userId
        Row!UPDATED = Today.Date.ToString("yyyy-MM-dd")
        Row!UPTIME = Date.Now.ToLongTimeString
        Row!SYSTEMID = systemId
        Row!CASHID = BillCashCounterId
        Row!COSTID = BillCostId
        Row!APPVER = VERSION
        Row!COMPANYID = strCompanyId
        Row!TRANSFERED = DBNull.Value
        Row!WT_ENTORDER = DBNull.Value
        Row!RATE = Val(txtCutRate_AMT.Text)
        Row!TRANFLAG = TranFlag
        Row!PCODE = _Accode
        DtAccTran.Rows.Add(Row)
        InsertData(SyncMode.Transaction, DtAccTran, cn, tran, BillCostId)
    End Sub

    Public Sub InsertIntoIssueReceipt( _
    ByVal tableName As String _
    , ByVal tranType As String, ByVal TYPE As WholeSaleBill_ShortEntry.TranType, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double _
    , ByVal pureWt As Double, ByVal wast As Double _
    , ByVal mc As Double, ByVal rate As Double, ByVal amount As Double _
    , ByVal vat As Double, ByVal stnAmt As Double, ByVal miscAmt As Double, ByVal tranflag As String)
        Dim issSno As String

        If UCase(tableName) = "ISSUE" Then
            issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
        Else
            issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
        End If
        If rate = 0 Then
            rate = Val(txtCutRate_AMT.Text)
        End If
        Dim DtIssRec As New DataTable
        DtIssRec = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, UCase(tableName), cn, tran)

        StrSql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCategory.Text.ToString & "' "
        _PureCatCode = objGPack.GetSqlValue(StrSql, , "", tran)
        If _PureCatCode = "" Then _PureCatCode = "00001"

        If CATCODE = "" Then CATCODE = _PureCatCode
        Dim Row As DataRow = DtIssRec.NewRow
        Row!SNO = issSno
        Row!TRANNO = Tranno
        Row!TRANDATE = GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
        Row!TRANTYPE = tranType
        Row!PCS = Math.Abs(pcs)
        Row!GRSWT = Math.Abs(grsWt)
        Row!NETWT = Math.Abs(netWT)
        Row!LESSWT = DBNull.Value
        Row!PUREWT = Math.Abs(pureWt)
        Row!TAGNO = DBNull.Value
        Row!ITEMID = DBNull.Value
        Row!SUBITEMID = DBNull.Value
        Row!WASTPER = DBNull.Value
        Row!WASTAGE = wast
        Row!MCGRM = DBNull.Value
        Row!MCHARGE = Math.Abs(mc)
        Row!AMOUNT = Math.Abs(amount)
        Row!RATE = rate
        Row!BOARDRATE = DBNull.Value
        Row!SALEMODE = DBNull.Value
        Row!GRSNET = "N"
        Row!TRANSTATUS = DBNull.Value
        Row!REFNO = DBNull.Value
        Row!REFDATE = DBNull.Value
        Row!COSTID = BillCostId
        Row!COMPANYID = strCompanyId
        Row!FLAG = DBNull.Value
        Row!EMPID = Val(txtEmpId_MAN.Text)
        Row!PURITY = DBNull.Value
        Row!CATCODE = CATCODE
        Row!BATCHNO = Batchno
        Row!REMARK1 = txtRemark.Text
        Row!USERID = userId
        Row!UPDATED = Today.Date.ToString("yyyy-MM-dd")
        Row!UPTIME = Date.Now.ToLongTimeString
        Row!SYSTEMID = systemId
        Row!CASHID = BillCashCounterId
        Row!TRANFLAG = tranflag
        Row!ACCODE = _Accode
        DtIssRec.Rows.Add(Row)
        InsertData(SyncMode.Transaction, DtIssRec, cn, tran, BillCostId)
    End Sub

    Public Sub SaveCollection(ByVal FromEntry As String, ByVal cmbTrantype As String, ByVal BillDate As Date, ByVal _Accode As String, ByVal TranNo As Integer, ByVal Batchno As String, ByVal BillCostId As String, ByVal BillCashCounterId As String, ByVal settledWt As Decimal, ByVal settledAmt As Decimal, Optional ByVal runno As String = "")
        Me.FromEntry = FromEntry
        Me.BillDate = BillDate
        Me._Accode = _Accode
        Me.Tranno = TranNo
        Me.Batchno = Batchno
        Me.BillCostId = BillCostId
        Me.BillCashCounterId = BillCashCounterId
        Me.SettleAmount = settledAmt
        Me.SettleWeight = settledWt
        Dim puType As String = "PU"
        Dim saType As String = "SA"
        If FromEntry = "SMI" Then
            Select Case cmbTrantype
                Case WholeSaleBill_ShortEntry.EntryMode.Issue.ToString, WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString
                    puType = "RRE" : saType = "IIS"
                Case WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString, WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString
                    puType = "RAP" : saType = "IAP"
            End Select
        End If
        ''Rate Cut Posting
        If Val(txtCutRate_AMT.Text) <> 0 Then
            If Val(txtCutWeight_WET.Text) > 0 Then
                InsertIntoIssueReceipt("RECEIPT", puType, WholeSaleBill_ShortEntry.TranType.Purchase, "", 0, 0, 0, Val(txtCutWeight_WET.Text) _
                , 0, 0, Val(txtCutRate_AMT.Text), Val(txtCutAmount_AMT.Text), 0, 0, 0, "AC")
                InsertIntoAccTran(TranNo, "C", "SAAC", Val(txtCutAmount_AMT.Text) _
                , 0, 0, 0, Val(txtCutWeight_WET.Text), "SA", _Accode, , , _
                , , , , txtRemark.Text, , "AC")
                InsertIntoAccTran(TranNo, "D", _Accode, Val(txtCutAmount_AMT.Text) _
                , 0, 0, 0, Val(txtCutWeight_WET.Text), "SA", "SAAC", , , _
                , , , , txtRemark.Text, , )
            Else
                InsertIntoIssueReceipt("ISSUE", saType, WholeSaleBill_ShortEntry.TranType.Sales, "", 0, 0, 0, Val(txtCutWeight_WET.Text) _
                , 0, 0, Val(txtCutRate_AMT.Text), Val(txtCutAmount_AMT.Text), 0, 0, 0, "AC")
                InsertIntoAccTran(TranNo, "D", "PUAC", Val(txtCutAmount_AMT.Text) _
                , 0, 0, 0, Val(txtCutWeight_WET.Text), "PU", _Accode, , , _
                , , , , txtRemark.Text, , "AC")
                InsertIntoAccTran(TranNo, "C", _Accode, Val(txtCutAmount_AMT.Text) _
                , 0, 0, 0, Val(txtCutWeight_WET.Text), "PU", "PUAC", , , _
                , , , , txtRemark.Text, , )
            End If

        End If
        ''MISCAMT Posting
        Dim MiscAmt As Decimal = Val(txtDueAmount_AMT.Text) - SettleAmount - Val(ObjConvertValue.txtConvertValue_AMT.Text)
        If MiscAmt <> 0 Then
            If MiscAmt > 0 Then
                InsertIntoAccTran(TranNo, "C", "MISCREC", MiscAmt, 0, 0, 0, 0, "PU", _Accode, , , , , , , txtRemark.Text, , "MC")
                InsertIntoAccTran(TranNo, "D", _Accode, MiscAmt, 0, 0, 0, 0, "PU", "MISCREC", , , , , , , txtRemark.Text, , )
            Else
                InsertIntoAccTran(TranNo, "D", "MISCREC", MiscAmt, 0, 0, 0, 0, "SA", _Accode, , , , , , , txtRemark.Text, , "MC")
                InsertIntoAccTran(TranNo, "C", _Accode, MiscAmt, 0, 0, 0, 0, "SA", "MISCREC", , , , , , , txtRemark.Text, , )
            End If
        End If
        ''MISC CONVERT VALUE POSTING
        Dim MiscWt As Decimal = Val(ObjConvertValue.txtConvertWeight_WET.Text)
        If MiscWt <> 0 Then
            If MiscWt > 0 Then
                InsertIntoIssueReceipt("ISSUE", saType, WholeSaleBill_ShortEntry.TranType.Sales, "", 0, 0, 0, MiscWt, 0, 0, Val(ObjConvertValue.txtConvertRate_AMT.Text), Val(ObjConvertValue.txtConvertValue_AMT.Text), 0, 0, 0, "MC")
            Else
                InsertIntoIssueReceipt("RECEIPT", puType, WholeSaleBill_ShortEntry.TranType.Purchase, "", 0, 0, 0, MiscWt, 0, 0, Val(ObjConvertValue.txtConvertRate_AMT.Text), Val(ObjConvertValue.txtConvertValue_AMT.Text), 0, 0, 0, "MC")
            End If
        End If
        ''Disc Weight Posting
        If Val(txtDiscWeight_WET.Text) <> 0 Then
            If chkDiscount.Checked Then
                ''Discount
                InsertIntoIssueReceipt("RECEIPT", puType, WholeSaleBill_ShortEntry.TranType.Purchase, "", 0, 0, 0, Val(txtDiscWeight_WET.Text), 0, 0, 0, 0, 0, 0, 0, "DI")
            Else
                ''Lp
                InsertIntoIssueReceipt("ISSUE", saType, WholeSaleBill_ShortEntry.TranType.Sales, "", 0, 0, 0, Val(txtDiscWeight_WET.Text), 0, 0, 0, 0, 0, 0, 0, "LP")
            End If
        End If
        If Val(txtDiscAmount_AMT.Text) <> 0 Then
            If chkDiscount.Checked Then
                ''Discount
                InsertIntoAccTran(TranNo, "D", "DISC", Val(txtDiscAmount_AMT.Text), 0, 0, 0, 0, "DI", _Accode, , , , , , , txtRemark.Text, , "DI")
                InsertIntoAccTran(TranNo, "C", _Accode, Val(txtDiscAmount_AMT.Text), 0, 0, 0, 0, "DI", "DISC", , , , , , , txtRemark.Text, , "DI")
            Else
                ''Lp
                InsertIntoAccTran(TranNo, "C", "LATEPAY", Val(txtDiscAmount_AMT.Text), 0, 0, 0, 0, "LP", _Accode, , , , , , , txtRemark.Text, , "LP")
                InsertIntoAccTran(TranNo, "D", _Accode, Val(txtDiscAmount_AMT.Text), 0, 0, 0, 0, "LP", "LATEPAY", , , , , , , txtRemark.Text, , "LP")
            End If
        End If

        ''BALANCE
        If Not (cmbTrantype = WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString _
        And cmbTrantype = WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString) Then
            Dim bAmt As Decimal = Val(txtBalAmount_AMT.Text) - Val(settledAmt)
            Dim bWt As Decimal = Val(txtBalWeight_WET.Text) - Val(settledWt)
            If bWt <> 0 Then
                InsertIntoOustanding(TranNo, "D", runno, 0, IIf(bWt > 0, "P", "R") _
                , "DU", , , , , , , , , , , , , , , , bWt)
            End If
            If bAmt <> 0 Then
                InsertIntoOustanding(TranNo, "D", runno, bAmt, IIf(bAmt > 0, "P", "R") _
                , "DU", , , , , , , , , , , , , , , , )
            End If
            bAmt = Math.Round(BalAmount - Val(txtBalAmount_AMT.Text), 2)
            bWt = Math.Round(BalWeight - Val(txtBalWeight_WET.Text), 3)
            ''Round Off
            If bWt <> 0 Then
                If bWt < 0 Then
                    InsertIntoIssueReceipt("ISSUE", saType, WholeSaleBill_ShortEntry.TranType.Sales, "", 0, 0, 0, bWt, 0, 0, 0, 0, 0, 0, 0, "RO")
                Else
                    InsertIntoIssueReceipt("RECEIPT", puType, WholeSaleBill_ShortEntry.TranType.Purchase, "", 0, 0, 0, bWt, 0, 0, 0, 0, 0, 0, 0, "RO")
                End If
            End If
            If bAmt <> 0 Then
                If bAmt < 0 Then
                    InsertIntoAccTran(TranNo, "C", "RNDOFF", bAmt, 0, 0, 0, 0, "RO", _Accode, , , , , , , txtRemark.Text, , "RO")
                    InsertIntoAccTran(TranNo, "D", _Accode, bAmt, 0, 0, 0, 0, "RO", "RNDOFF", , , , , , , txtRemark.Text, , "RO")
                Else
                    InsertIntoAccTran(TranNo, "D", "RNDOFF", bAmt, 0, 0, 0, 0, "RO", _Accode, , , , , , , txtRemark.Text, , "RO")
                    InsertIntoAccTran(TranNo, "C", _Accode, bAmt, 0, 0, 0, 0, "RO", "RNDOFF", , , , , , , txtRemark.Text, , "RO")
                End If
            End If
        End If
        If Val(txtCash_AMT.Text) <> 0 Then
            ''CASH
            InsertIntoAccTran(TranNo, IIf(Val(txtCash_AMT.Text) > 0, "D", "C"), "CASH", Val(txtCash_AMT.Text), 0, 0, 0, 0, "CA", _Accode, , , , , , , txtRemark.Text, , "CA")
            InsertIntoAccTran(TranNo, IIf(Val(txtCash_AMT.Text) > 0, "C", "D"), _Accode, Val(txtCash_AMT.Text), 0, 0, 0, 0, "CA", "CASH", , , , , , , txtRemark.Text, , "CA")
        End If
        ' ''SCHEME TRANS
        If Val(txtAdjChitCard_AMT.Text) <> 0 Then
            objChitCard.Tranflag = "SS"
            objChitCard.InsertChitCardDetail(Batchno, TranNo, BillDate, BillCashCounterId, BillCostId, "", tran, "P", False)
            InsertIntoAccTran(TranNo, IIf(Val(txtAdjChitCard_AMT.Text) > 0, "C", "D"), _Accode, Val(txtAdjChitCard_AMT.Text), 0, 0, 0, 0, "SS", "", , , , , , , txtRemark.Text, , "SS")
        End If
        ''CreditCard Trans
        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, 0, "CC", _Accode, _
            , , ro!CARDNO.ToString, ro!DATE.ToString, _
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            ro!APPNO.ToString, txtRemark.Text, , "CC" _
            )
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
            _Accode _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, 0, "CC", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            , , ro!CARDNO.ToString, ro!DATE.ToString, _
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            ro!APPNO.ToString, txtRemark.Text, , "CC" _
            )

            Dim commision As Double = Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, 0, "BC", "BANKC", _
                , , , , , , txtRemark.Text, , "BC")
                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
                "BANKC" _
                , commision, 0, 0, 0, 0, "BC", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                , , , , , , txtRemark.Text, , "BC")

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                    objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , sCharge, 0, 0, 0, 0, "BS", "BANKS", _
                    , , , , , , txtRemark.Text, , "BS")
                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
                    "BANKS" _
                    , sCharge, 0, 0, 0, 0, "BS", objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
                    , , , , , , txtRemark.Text, , "BS")
                End If
            End If
        Next
        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), _
            Val(ro!AMOUNT.ToString), 0, 0, 0, 0, "CH", _Accode, _
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString, txtRemark.Text, , "CH")
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
            _Accode, _
            Val(ro!AMOUNT.ToString), 0, 0, 0, 0, "CH", objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), _
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString, txtRemark.Text, , "CH")
        Next
        ''Handling Charges
        If Val(txtAdjHandlingCharge_AMT.Text) <> 0 Then
            InsertIntoAccTran(TranNo, IIf(Val(txtAdjHandlingCharge_AMT.Text) > 0, "C", "D"), _
            "HANDC", Val(txtAdjHandlingCharge_AMT.Text), 0, 0, 0, 0, "HC", _Accode, , , , , , , txtRemark.Text, , "HC")
            InsertIntoAccTran(TranNo, IIf(Val(txtAdjHandlingCharge_AMT.Text) > 0, "D", "C"), _
            _Accode, Val(txtAdjHandlingCharge_AMT.Text), 0, 0, 0, 0, "HC", "HANDC", , , , , , , txtRemark.Text, , "HC")
        End If
    End Sub
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
     Optional ByVal refDate As Date = Nothing, _
     Optional ByVal purity As Double = Nothing, _
     Optional ByVal proId As Integer = Nothing, _
     Optional ByVal dueDate As Date = Nothing, _
     Optional ByVal Remark1 As String = Nothing, _
     Optional ByVal Remark2 As String = Nothing, _
     Optional ByVal Accode As String = Nothing, _
     Optional ByVal Flag As String = Nothing, _
     Optional ByVal EmpId As Integer = Nothing, _
     Optional ByVal PureWt As Double = Nothing _
         )
        If Amount = 0 And GrsWt = 0 And PureWt = 0 Then Exit Sub
        Dim dtOutStanding As New DataTable
        dtOutStanding = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "OUTSTANDING", cn, tran)
        Dim Row As DataRow = dtOutStanding.NewRow
        If Accode = "" Then Accode = _Accode
        Row!SNO = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        Row!TRANNO = tNo
        Row!TRANDATE = GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
        Row!TRANTYPE = tType
        Row!RUNNO = RunNo
        Row!AMOUNT = Math.Abs(Amount)
        Row!GRSWT = Math.Abs(GrsWt)
        Row!NETWT = Math.Abs(NetWt)
        Row!PUREWT = Math.Abs(PureWt)
        Row!RECPAY = RecPay
        Row!REFNO = refNo
        If refDate.Year <> 1 Then Row!REFDATE = refDate
        Row!EMPID = EmpId
        Row!PURITY = purity
        Row!CATCODE = CatCode
        Row!BATCHNO = Batchno
        Row!USERID = userId
        Row!UPDATED = Today.Date.ToString("yyyy-MM-dd")
        Row!UPTIME = Date.Now.ToLongTimeString
        Row!RATE = Val(txtCutRate_AMT.Text)
        Row!VALUE = Value
        Row!CASHID = BillCashCounterId
        Row!REMARK1 = Remark1
        Row!REMARK2 = Remark2
        Row!ACCODE = Accode
        Row!CTRANCODE = proId
        If dueDate.Year <> 1 Then Row!DUEDATE = dueDate
        Row!APPVER = VERSION
        Row!COMPANYID = strCompanyId
        Row!COSTID = BillCostId
        Row!FROMFLAG = "P"
        Row!FLAG = Flag
        Row!PAYMODE = Paymode
        Row!TRANFLAG = IIf(Val(txtCutRate_AMT.Text) <> 0, "A", "W")
        dtOutStanding.Rows.Add(Row)
        InsertData(SyncMode.Transaction, dtOutStanding, cn, tran, BillCostId)
    End Sub

    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = Me.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = grpCollection.BackgroundColor
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtBalAmount_AMT.Focus()
    End Sub

    Private Sub txtAdjCreditCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCreditCard_AMT.GotFocus
        If objCreditCard.Visible Then Exit Sub
        objCreditCard.BackColor = Me.BackColor
        objCreditCard.StartPosition = FormStartPosition.CenterScreen
        objCreditCard.grpCreditCard.BackgroundColor = grpCollection.BackgroundColor
        objCreditCard.ShowDialog()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
        txtBalAmount_AMT.Focus()
    End Sub
    Private Sub txtAdjChitCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.GotFocus
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper <> "Y" Then
            txtBalAmount_AMT.Focus()
            Exit Sub
        End If
        If objChitCard.Visible Then Exit Sub
        objChitCard.BackColor = Me.BackColor
        objChitCard.StartPosition = FormStartPosition.CenterScreen
        objChitCard.grpCHIT.BackgroundColor = grpCollection.BackgroundColor
        Select Case objChitCard.ShowDialog
            Case Windows.Forms.DialogResult.OK
                Dim chitAmt As Double = Val(objChitCard.gridCHITCardTotal.Rows(0).Cells("TOTAL").Value.ToString)
                txtAdjChitCard_AMT.Text = IIf(chitAmt <> 0, Format(chitAmt, "0.00"), Nothing)
                txtBalAmount_AMT.Focus()
            Case Windows.Forms.DialogResult.Abort
                objChitCard = New frmChitAdj
                txtBalAmount_AMT.Focus()
        End Select
    End Sub

    Private Sub BillCollection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtCat As DataTable
        StrSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP IN ('B','L') ORDER BY DISPLAYORDER"
        dtCat = New DataTable
        dtCat.Columns.Add("KEYNO_HIDE", GetType(Integer))
        dtCat.Columns("KEYNO_HIDE").AutoIncrement = True
        dtCat.Columns("KEYNO_HIDE").AutoIncrementSeed = 0
        dtCat.Columns("KEYNO_HIDE").AutoIncrementStep = 1
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCat)
        cmbCategory.Items.Clear()
        objGPack.FillCombo(StrSql, cmbCategory)
    End Sub
End Class