Imports System.Data.OleDb
Public Class WRateCut
    Dim da As OleDbDataAdapter
    Dim strSql As String = Nothing
    Dim dtTemp As DataTable

    Dim acCode As String = Nothing
    Dim catCode As String = Nothing
    Dim tranNo As Integer
    Dim batchNo As String
    Dim tran As OleDbTransaction
    Dim cmd As OleDbCommand
    Dim fromFlag As String
    Dim _Costid As String
    Dim _CostcenterMaintain As Boolean = IIf(GetAdmindbSoftValue("COSTCENTRE") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        LoadCostcentre()
        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACTYPE IN ('G','D')"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbPartyName_MAN)
        btnNew_Click(Me, New EventArgs)
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE ACCTDISPLAY = 'Y'"
        'strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbCatName_MAN)
        btnNew_Click(Me, New EventArgs)

    End Sub
    Private Sub CalcValue()
        Dim value As Double = Nothing
        If rbtAmt2Wt.Checked Then
            If Val(txtRate.Text) > 0 Then
                value = Val(txtConvertVal.Text) / Val(txtRate.Text)
            End If
            txtValue.Text = IIf(value <> 0, Format(value, "0.000"), "")
        Else '' Wt2Amt Checked
            value = Val(txtConvertVal.Text) * Val(txtRate.Text)
            txtValue.Text = IIf(value <> 0, Format(value, "0.00"), "")
        End If
    End Sub
    Private Sub InsertReceipt(ByVal TRANFLAG As String, ByVal Weight As Double, ByVal AMOUNT As Double, ByVal METALID As String)
        strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
        strSql += " ("
        strSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
        strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
        strSql += " ,ITEMID,SUBITEMID,WASTPER"
        strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
        strSql += " ,REFNO,REFDATE,COSTID"
        strSql += " ,COMPANYID,EMPID"
        strSql += " ,PURITY,CATCODE,OCATCODE,METALID"
        strSql += " ,ACCODE,BATCHNO,REMARK1"
        strSql += " ,REMARK2"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,VATEXM,TOUCH,TAX"
        strSql += " ,TRANFLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.RECEIPTCODE, tran) & "'" ''SNO
        strSql += " ," & tranNo & "" 'TRANNO VAL(.ITEM("TRANNO", CNT).VALUE.TOSTRING)
        strSql += " ,'" & Format(dtpBilldate.Value.Date, "yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'RRC'" 'TRANTYPE
        strSql += " ,0" 'PCS
        strSql += " ," & Math.Abs(Weight) & "" 'PUREWT '0
        strSql += " ," & Math.Abs(Weight) & "" 'PUREWT '0"
        strSql += " ,0" 'LESSWT
        strSql += " ," & Math.Abs(Weight) & "" 'PUREWT '0
        strSql += " ,0" 'ITEMID
        strSql += " ,0" 'SUBITEMID
        strSql += " ,0" 'WASTPER
        strSql += " ,0" 'WASTAGE
        strSql += " ,0" 'MCGRM
        strSql += " ,0" 'MCHARGE
        strSql += " ," & Math.Abs(AMOUNT) & "" 'AMOUNT
        strSql += " ," & Val(txtRate.Text) & "" 'RATE
        strSql += " ,0" 'BOARDRATE
        strSql += " ,''" 'SALEMODE
        strSql += " ,'N'" 'GRSNET
        strSql += " ,''" 'REFNO ''
        strSql += " ,NULL" 'REFDATE 
        strSql += " ,'" & _Costid & "'" 'COSTID 
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,0" 'EMPID
        strSql += " ," & Val(txtTouch.Text) & "" 'PURITY
        strSql += " ,'" & catCode & "'" 'CATCODE
        strSql += " ,''" 'OCATCODE
        strSql += " ,'" & METALID & "'"
        strSql += " ,'" & acCode & "'" 'ACCODE

        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,'" & txtRemark.Text & "'" 'REMARK1
        strSql += " ,''" 'REMARK2
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Format(Today.Date, "yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,''" 'VATEXM
        strSql += " ," & Val(txtTouch.Text) & "" 'TOUCH
        strSql += " ,0" 'TAX
        strSql += " ,'" & TRANFLAG & "'" 'TRANFLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, _Costid)
        cmd = Nothing
        strSql = Nothing
    End Sub

    Private Sub InsertIssue(ByVal TRANFLAG As String, ByVal WEIGHT As Double, ByVal AMOUNT As Double, ByVal METALID As String)
        strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
        strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
        strSql += " ,ITEMID,SUBITEMID,WASTPER"
        strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
        strSql += " ,COSTID,COMPANYID,EMPID,PURITY"
        strSql += " ,CATCODE,OCATCODE,METALID"
        strSql += " ,ACCODE,BATCHNO,REMARK1"
        strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,VATEXM,TOUCH,TAX"
        strSql += " ,TRANFLAG"
        strSql += " )"
        strSql += " VALUES("
        strSql += " '" & GetNewSno(TranSnoType.ISSUECODE, tran) & "'" ''SNO
        strSql += " ," & tranNo & "" ''TranNo
        strSql += " ,'" & Format(dtpBilldate.Value.Date, "yyyy-MM-dd") & "'" 'TranDate
        strSql += " ,'IRC'" 'TranType
        strSql += " ,0" 'Pcs
        strSql += " ," & Math.Abs(WEIGHT) & "" 'GRSWT '0
        strSql += " ," & Math.Abs(WEIGHT) & "" 'NETWT '0
        strSql += " ,0" 'LessWt
        strSql += " , " & Math.Abs(WEIGHT) & "" 'PureWt '
        strSql += " ,0" 'ItemId
        strSql += " ,0" 'SubItemId
        strSql += " ,0" 'WastPer
        strSql += " ,0" 'Wastage
        strSql += " ,0" 'McGrm
        strSql += " ,0" 'MCharge
        strSql += " ," & Math.Abs(AMOUNT) & "" 'Amount
        strSql += " ," & Val(txtRate.Text) & "" 'Rate
        strSql += " ,0" 'BoardRate
        strSql += " ,''" 'SaleMode
        strSql += " ,'N'" 'GrsNet
        strSql += " ,'" & _Costid & "'" 'CostId 
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,0" 'EmpId
        strSql += " ," & Val(txtTouch.Text) & "" 'Purity
        strSql += " ,'" & catCode & "'" 'CatCode
        strSql += " ,''" 'OCatCode
        strSql += " ,'" & METALID & "'" 'Metalid
        strSql += " ,'" & acCode & "'" 'AcCode
        strSql += " ,'" & batchNo & "'" 'BatchNo
        strSql += " ,'" & txtRemark.Text & "'" 'Remark1
        strSql += " ,'' " 'Remark2
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Format(Today.Date, "yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'SystemId
        strSql += " ,''" 'CashId
        strSql += " ,''" 'VATEXM
        strSql += " ," & Val(txtTouch.Text) & "" 'TOUCH
        strSql += " ,0" 'TAX
        strSql += " ,'" & TRANFLAG & "'" 'TRANFLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, _Costid)
    End Sub

    Private Sub InsertAccTran(ByVal TranMode As String, ByVal Accode As String, ByVal PureWt As Double, ByVal Amount As Double, ByVal PayMode As String, ByVal Contra As String, ByVal TranFlag As String)
        Dim MREMARK As String = "Convert " & Math.Abs(PureWt).ToString + "g @ " & txtRate.Text
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT,PUREWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,RATE,TRANFLAG,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tranNo & "" 'TRANNO 
        strSql += " ,'" & Format(dtpBilldate.Value.Date, "yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & TranMode & "'" 'TRANMODE
        strSql += " ,'" & Accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ,0" 'PCS
        strSql += " ,0" 'GRSWT
        strSql += " ,0" 'NETWT
        strSql += " ," & Math.Abs(PureWt) & "" 'PUREWT
        strSql += " ,''" 'REFNO
        strSql += " ,NULL" 'REFDATE
        strSql += " ,'" & PayMode & "'" 'PAYMODE
        strSql += " ,''" 'CHQCARDNO
        strSql += " ,0" 'CARDID
        strSql += " ,''" 'CHQCARDREF
        strSql += " ,NULL" 'CHQDATE
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'" & fromFlag & "'" 'FROMFLAG
        strSql += " ,'" & txtRemark.Text & "'" 'REMARK1
        strSql += " ,'" & MREMARK & "'"
        strSql += " ,'" & Contra & "'" 'CONTRA
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Format(Today.Date, "yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & _Costid & "'" 'COSTID
        strSql += " ,''" 'VATEXM
        strSql += " ," & Val(txtRate.Text) & "" 'RATE
        strSql += " ,'" & TranFlag & "'" 'TRANFLAG
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, _Costid)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub cmbPartyName_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbPartyName_MAN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbPartyName_MAN.Text = "" Then
                MsgBox("Ac Name Should not empty", MsgBoxStyle.Information)
                cmbPartyName_MAN.Select()
                Exit Sub
            End If
            If Not cmbPartyName_MAN.Items.Contains(cmbPartyName_MAN.Text) Then
                MsgBox("Invalid Ac Name", MsgBoxStyle.Information)
                cmbPartyName_MAN.Select()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        Dim serverDate As Date = GetEntryDate(GetServerDate(tran))
        dtpBilldate.Value = serverDate
        rbtAmt2Wt.Checked = True
        strSql = " SELECT MAX(PURITY) AS  PURITY FROM " & cnAdminDb & "..PURITYMAST"
        txtTouch.Text = Format(Val(objGPack.GetSqlValue(strSql)), "0.00")
        LoadCostcentre()
        ''CLEAR VARIBLES
        acCode = Nothing
        batchNo = Nothing
        tranNo = Nothing

        dtpBilldate.Select()
    End Sub
    Private Sub LoadCostcentre()
        strSql = " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),1 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        Dim dtCostCentre As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostCentre, "COSTNAME", , cnCostName)
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkcmbCostcentre.Enabled = False
        If _CostcenterMaintain Then
            _Costid = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkcmbCostcentre.Text & "'")
            _Costid = IIf(_Costid <> "", _Costid, cnCostId)
        Else
            chkcmbCostcentre.Enabled = False
            _Costid = cnCostId
        End If
    End Sub
    Private Sub frmRateCut_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtAmtBal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmtBal.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtWtBal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWtBal.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub rbtAmt2Wt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAmt2Wt.CheckedChanged
        If rbtAmt2Wt.Checked Then
            lblConvertVal.Text = "Amount"
            lblValue.Text = "Weight"
        End If
    End Sub

    Private Sub rbtWt2Amt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtWt2Amt.CheckedChanged
        If rbtWt2Amt.Checked Then
            lblConvertVal.Text = "Weight"
            lblValue.Text = "Amount"
        End If
    End Sub

    Private Sub txtConvertVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtConvertVal.KeyPress
        If e.KeyChar = Chr(Keys.Back) Then
            Exit Sub
        End If
        If rbtAmt2Wt.Checked Then
            Dim keyChar As String
            keyChar = e.KeyChar
            If AscW(keyChar) = 46 Then
                If CType(sender, TextBox).Text.Contains(".") Then
                    e.Handled = True
                End If
            End If
            If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 And keyChar <> Chr(Keys.Escape) And keyChar <> "-" Then
                e.Handled = True
                MsgBox("Digits only Allowed 1 to 9")
                CType(sender, TextBox).Focus()
            End If
            If CType(sender, TextBox).Text.Contains(".") Then
                Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
                Dim sp() As String = CType(sender, TextBox).Text.Split(".")
                Dim curPos As Integer = CType(sender, TextBox).SelectionStart
                If sp.Length >= 2 Then
                    If curPos >= dotPos Then
                        If sp(1).Length > 1 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        Else
            Dim keyChar As String
            keyChar = e.KeyChar
            If AscW(keyChar) = 46 Then
                If CType(sender, TextBox).Text.Contains(".") Then
                    e.Handled = True
                End If
            End If
            If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 And keyChar <> Chr(Keys.Escape) And keyChar <> "-" Then
                e.Handled = True
                MsgBox("Digits only Allowed 1 to 9")
                CType(sender, TextBox).Focus()
            End If
            If CType(sender, TextBox).Text.Contains(".") Then
                Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
                Dim sp() As String = CType(sender, TextBox).Text.Split(".")
                Dim curPos As Integer = CType(sender, TextBox).SelectionStart
                If sp.Length >= 2 Then
                    If curPos >= dotPos Then
                        If sp(1).Length > 2 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtConvertVal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConvertVal.LostFocus
        If rbtAmt2Wt.Checked Then
            If CType(sender, TextBox).Text <> "" Then
                CType(sender, TextBox).Text = Format(Val(CType(sender, TextBox).Text), "0.00")
            End If
        Else
            If CType(sender, TextBox).Text <> "" Then
                CType(sender, TextBox).Text = Format(Val(CType(sender, TextBox).Text), "0.000")
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtValue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValue.GotFocus
        CalcValue()
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        CalcValue()
        ''Validation
        Dim serverDate1 As Date = GetEntryDate(GetServerDate())
        Dim DATECHANGEALLOW As Boolean = True
        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then DATECHANGEALLOW = False

        Dim glbDate As Date = GetAdmindbSoftValue("GLOBALDATEVAL", serverDate1)
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL'")

        If RestrictDays.Contains(",") = False Then
            If Not DATECHANGEALLOW Then RestrictDays = "0"
            If Not (dtpBilldate.Value >= serverDate1.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                dtpBilldate.Focus()
                Exit Sub
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If Not DATECHANGEALLOW Then mondiv = "0"
            If closeday > serverDate1.Day Then
                Dim mindate As Date = serverDate1.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpBilldate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpBilldate.Focus()
                    Exit Sub
                End If
            Else
                Dim mindate As Date = serverDate1.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpBilldate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpBilldate.Focus()
                    Exit Sub
                End If
            End If
        End If

        If Not Val(txtConvertVal.Text) > 0 Then
            MsgBox("Invalid " + lblConvertVal.Text)
            txtConvertVal.Select()
            Exit Sub
        End If
        If Not Val(txtRate.Text) > 0 Then
            MsgBox("Invalid Rate")
            txtRate.Select()
            Exit Sub
        End If
        If Not Val(txtValue.Text) > 0 Then
            MsgBox("Invalid Value")
            txtValue.Select()
            Exit Sub
        End If
        If Val(txtTouch.Text) > 112 Then
            MsgBox("Invalid Touch", MsgBoxStyle.Information)
            txtTouch.Select()
            Exit Sub
        End If
        If acCode = "" And cmbPartyName_MAN.Text <> "" Then
            acCode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName_MAN.Text & "'")
        End If
        If _CostcenterMaintain Then
            _Costid = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkcmbCostcentre.Text & "'")
            _Costid = IIf(_Costid <> "", _Costid, cnCostId)
        Else
            _Costid = cnCostId
        End If
        Try
            Dim Metalid As String
            'catCode = "00001"
            Dim Catdr As DataRow
            Catdr = GetSqlRow("select METALID,CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCatName_MAN.Text & "'", cn)
            If Not Catdr Is Nothing Then
                Metalid = Catdr.Item(0)
                catCode = Catdr.Item(1)
            Else
                MsgBox("Select Category", MsgBoxStyle.Information)
                cmbCatName_MAN.Select()
                Exit Sub
            End If
            tran = cn.BeginTransaction
            Dim billcontrolid As String = "GEN-RATECUT"
            Dim IRCCHECK As String = ""
            strSql = " SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "' AND COSTID ='" & _Costid & "'"
            IRCCHECK = objGPack.GetSqlValue(strSql, , , tran)
            If IRCCHECK = "Y" Then
                strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "' AND COSTID = '" & _Costid & "'"
                tranNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                strSql = "select tranno from " & cnStockDb & "..ISSUE where trantype='IRC' and costid = '" & _Costid & "' and tranno='" & tranNo + 1 & "'"
                IRCCHECK = Val(objGPack.GetSqlValue(strSql, , , tran))
                If Val(IRCCHECK) = 0 Then
                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo + 1 & "' "
                    strSql += " WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND COSTID = '" & _Costid & "'"
                    strSql += " AND CONVERT(INT,CTLTEXT) = " & tranNo & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    tranNo = tranNo + 1
                End If
            Else
                tranNo = 0
            End If
            If Val(tranNo) = 0 Then
                tranNo = GetMaxTranNo(dtpBilldate.Value.ToString("yyyy-MM-dd"), tran)
            End If

            batchNo = GetNewBatchno(_Costid, dtpBilldate.Value.ToString("yyyy-MM-dd"), tran)
            Dim weight As Decimal = Nothing
            Dim Amount As Decimal = Nothing
            If rbtWt2Amt.Checked Then
                weight = Val(txtConvertVal.Text)
                Amount = Val(txtValue.Text)
            Else
                weight = Val(txtValue.Text)
                Amount = Val(txtConvertVal.Text)
            End If
            ''Insert Values Into Receipt
            Dim TranType As String = ""
            If rbtReceipt.Checked Then
                If rbtAmt2Wt.Checked Then
                    fromFlag = "T"
                    InsertReceipt("AC", weight, Amount, Metalid)
                    InsertAccTran("D", acCode, weight, Amount, "JE", "PUAC", "AC")
                    InsertAccTran("C", "PUAC", weight, Amount, "JE", acCode, "AC")
                    TranType = "RRC"
                Else
                    fromFlag = "W"
                    InsertIssue("WC", weight, Amount, Metalid)
                    InsertAccTran("C", acCode, weight, Amount, "JE", "PUAC", "AC")
                    InsertAccTran("D", "PUAC", weight, Amount, "JE", acCode, "AC")
                    TranType = "IRC"
                End If
            Else
                ''payment
                If rbtAmt2Wt.Checked Then
                    fromFlag = "T"
                    InsertIssue("AC", weight, Amount, Metalid)
                    InsertAccTran("C", acCode, weight, Amount, "JE", "PUAC", "AC")
                    InsertAccTran("D", "PUAC", weight, Amount, "JE", acCode, "AC")
                    TranType = "IRC"
                Else
                    fromFlag = "W"
                    InsertReceipt("WC", weight, Amount, Metalid)
                    InsertAccTran("D", acCode, weight, Amount, "JE", "PUAC", "AC")
                    InsertAccTran("C", "PUAC", weight, Amount, "JE", acCode, "AC")
                    TranType = "RRC"
                End If
            End If
            tran.Commit()
            tran = Nothing
            MsgBox("TranNo  :" + tranNo.ToString + vbCrLf + "Batchno     :" + batchNo + vbCrLf + "Generated..")
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Try
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":" & TranType & "")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & batchNo)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(dtpBilldate.Value.Date, "yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":" & TranType & "" & ";" &
                        LSet("BATCHNO", 15) & ":" & batchNo & ";" &
                        LSet("TRANDATE", 15) & ":" & Format(dtpBilldate.Value.Date, "yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            Catch ex As Exception

            End Try
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
            End If
            MsgBox("Message     :" + ex.Message + vbCrLf + "Stack Trace     :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbPartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPartyName_MAN.LostFocus
        acCode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName_MAN.Text & "'")
        Dim dt As DataTable
        strSql = vbCrLf + "  SELECT SUM(WEIGHT)WEIGHT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT CONVERT(NUMERIC(15,3),0)WEIGHT,DEBIT-CREDIT AMOUNT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ACCODE = '" & acCode & "'"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE -1*PUREWT END WEIGHT,0 AMOUNT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT WHERE ACCODE = '" & acCode & "'"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT PUREWT,0 AMOUNT "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE WHERE ACCODE = '" & acCode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('AI','IAP','IPU')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT -1*PUREWT,0 AMOUNT "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT WHERE ACCODE = '" & acCode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('AR','RAP','RPU')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 0 WEIGHT,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE = '" & acCode & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  )X"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        txtAmtBal.Clear()
        txtWtBal.Clear()
        Dim pWt As Decimal = 0
        Dim pAmt As Decimal = 0
        If dt.Rows.Count > 0 Then
            pWt = Val(dt.Rows(0).Item("WEIGHT").ToString)
            pAmt = Val(dt.Rows(0).Item("AMOUNT").ToString)
        End If
        If pWt > 0 Then
            txtWtBal.Text = Format(pWt, "0.000") & " Dr"
        ElseIf pWt < 0 Then
            txtWtBal.Text = Format(Math.Abs(pWt), "0.000") & " Cr"
        Else
            txtWtBal.Text = ""
        End If
        If pAmt > 0 Then
            txtAmtBal.Text = Format(pAmt, "0.00") & " Dr"
        ElseIf pAmt < 0 Then
            txtAmtBal.Text = Format(Math.Abs(pAmt), "0.00") & " Cr"
        Else
            txtAmtBal.Text = ""
        End If
    End Sub
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
    Optional ByVal refDate As Date = Nothing,
    Optional ByVal purity As Double = Nothing,
    Optional ByVal proId As Integer = Nothing,
    Optional ByVal dueDate As Date = Nothing,
    Optional ByVal Remark1 As String = Nothing,
    Optional ByVal Remark2 As String = Nothing,
    Optional ByVal Accode As String = Nothing,
    Optional ByVal Flag As String = Nothing,
    Optional ByVal EmpId As Integer = Nothing,
    Optional ByVal PureWt As Double = Nothing,
    Optional ByVal TranFlag As String = Nothing
        )
        If Amount = 0 And GrsWt = 0 And PureWt = 0 Then Exit Sub
        Dim dtOutStanding As New DataTable
        dtOutStanding = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "OUTSTANDING", cn, tran)
        Dim Row As DataRow = dtOutStanding.NewRow
        If Accode = "" Then Accode = Accode
        Row!SNO = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        Row!TRANNO = tNo
        Row!TRANDATE = GetEntryDate(dtpBilldate.Value.ToString("yyyy-MM-dd"), tran).ToString("yyyy-MM-dd")
        Row!TRANTYPE = tType
        Row!RUNNO = RunNo
        Row!AMOUNT = Math.Abs(Amount)
        Row!GRSWT = Math.Abs(GrsWt)
        Row!NETWT = Math.Abs(NetWt)
        Row!PUREWT = Math.Abs(PureWt)
        Row!RECPAY = RecPay
        Row!REFNO = refNo
        Row!REFDATE = refDate
        Row!EMPID = EmpId
        Row!PURITY = purity
        Row!CATCODE = CatCode
        Row!BATCHNO = batchNo
        Row!USERID = userId
        Row!UPDATED = Today.Date.ToString("yyyy-MM-dd")
        Row!UPTIME = Date.Now.ToLongTimeString
        Row!RATE = Val(txtRate.Text)
        Row!VALUE = Value
        Row!CASHID = ""
        Row!REMARK1 = Remark1
        Row!REMARK2 = Remark2
        Row!ACCODE = Accode
        Row!CTRANCODE = proId
        Row!DUEDATE = dueDate
        Row!APPVER = VERSION
        Row!COMPANYID = strCompanyId
        Row!COSTID = _Costid
        Row!FROMFLAG = fromFlag
        Row!FLAG = Flag
        Row!PAYMODE = Paymode
        Row!TRANFLAG = TranFlag
        dtOutStanding.Rows.Add(Row)
        InsertData(SyncMode.Transaction, dtOutStanding, cn, tran, _Costid)
    End Sub

    Private Sub cmbCatName_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCatName_MAN.KeyDown
        If e.KeyCode = Keys.Enter Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbCatName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCatName_MAN.KeyPress

    End Sub



    Private Sub cmbCatName_MAN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCatName_MAN.SelectedIndexChanged

    End Sub

    Private Sub cmbPartyName_MAN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartyName_MAN.SelectedIndexChanged

    End Sub

    Private Sub WRateCut_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub chkcmbCostcentre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkcmbCostcentre.SelectedIndexChanged
        If _CostcenterMaintain Then
            _Costid = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkcmbCostcentre.Text & "'")
            _Costid = IIf(_Costid <> "", _Costid, cnCostId)
        Else
            _Costid = cnCostId
        End If
    End Sub
End Class