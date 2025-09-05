Imports System.Data.OleDb
Public Class frmBillChitReceipt
    Dim strSql As String
    Dim cmd As OleDbCommand
    Public chitDb As String
    Dim noOfIns As Integer
    Dim _Entrefno As String = ""
    Public dtGridChit As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        chitDb = GetAdmindbSoftValue("CHITDBPREFIX")
        With dtGridChit.Columns
            .Add("GROUPCODE", GetType(String))
            .Add("REGNO", GetType(Integer))
            .Add("NOOFINS", GetType(Integer))
            .Add("AMOUNT", GetType(Double))
        End With
        gridView.DataSource = dtGridChit
        gridView.ColumnHeadersVisible = False
        With gridView
            .Columns("GROUPCODE").Width = txtGrpCode.Width
            .Columns("REGNO").Width = txtRegNo_NUM.Width
            .Columns("NOOFINS").Width = txtNoOfIns_NUM.Width
            .Columns("NOOFINS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NOOFINS").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").Width = txtTotAmount_AMT.Width
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub
    Private Sub frmBillChitReceipt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmBillChitReceipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTotAmount_AMT.Focused Then Exit Sub
            If txtRegNo_NUM.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Function CloseCheck() As Boolean
        strSql = " SELECT 'CHECK' FROM " & chitDb & "SAVINGS..SCHEMEMAST"
        strSql += " WHERE GROUPCODE = '" & txtGrpCode.Text & "' AND REGNO = " & Val(txtRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(DOCLOSE,'') <> ''"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Already Closed")
        End If
    End Function


    Private Sub CustomerDetail()
        ClearCustomerInfo()
        Dim dtCustomerInfo As New DataTable
        ''Gets Customer Info
        strSql = " SELECT "
        strSql += " CASE WHEN P.TITLE <> '' THEN P.TITLE + ' ' ELSE '' END "
        strSql += " + CASE WHEN P.INITIAL <> '' THEN P.INITIAL + '.' ELSE '' END"
        strSql += " + P.PNAME AS PNAME,"
        strSql += " P.SNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.AREA,P.CITY,P.PINCODE,P.PHONERES,P.PHONERES2,P.MOBILE,P.MOBILE2"
        strSql += " FROM " & chitDb & "SAVINGS..SCHEMEMAST AS M INNER JOIN " & chitDb & "SAVINGS..PERSONALINFO AS P ON P.PERSONALID = M.SNO"
        strSql += " WHERE M.GROUPCODE = '" & txtGrpCode.Text & "' AND M.REGNO = " & Val(txtRegNo_NUM.Text) & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCustomerInfo)
        If Not dtCustomerInfo.Rows.Count > 0 Then
            MsgBox("Invalid Customer Info", MsgBoxStyle.Information)
            txtGrpCode.Focus()
            Exit Sub
        Else
            With dtCustomerInfo.Rows(0)
                lblName.Text = .Item("PNAME")
                If .Item("SNAME").ToString <> "" Then lblAddress.Text += .Item("SNAME").ToString + vbCrLf
                If .Item("DOORNO").ToString <> "" Then lblAddress.Text += .Item("DOORNO").ToString + vbCrLf
                If .Item("ADDRESS1").ToString <> "" Then lblAddress.Text += .Item("ADDRESS1").ToString + vbCrLf
                If .Item("ADDRESS2").ToString <> "" Then lblAddress.Text += .Item("ADDRESS2").ToString + vbCrLf
                If .Item("AREA").ToString <> "" Then lblAddress.Text += .Item("AREA").ToString + vbCrLf
                If .Item("CITY").ToString <> "" Then lblAddress.Text += .Item("CITY").ToString + vbCrLf
                If .Item("PINCODE").ToString <> "" Then lblAddress.Text += .Item("PINCODE").ToString + vbCrLf
                If .Item("PHONERES").ToString <> "" Then lblAddress.Text += .Item("PHONERES").ToString + vbCrLf
                If .Item("PHONERES2").ToString <> "" Then lblAddress.Text += .Item("PHONERES2").ToString + vbCrLf
                If .Item("MOBILE").ToString <> "" Then lblAddress.Text += .Item("MOBILE").ToString + vbCrLf
                If .Item("MOBILE2").ToString <> "" Then lblAddress.Text += .Item("MOBILE2").ToString
            End With
        End If
        ''Close Details
        strSql = " SELECT CONVERT(VARCHAR(12),DOCLOSE,103) AS DOCLOSE,"
        strSql += " CASE WHEN ISNULL(CLOSETYPE,'') = 'B' THEN 'BILL'"
        strSql += " WHEN ISNULL(CLOSETYPE,'') = 'C' THEN 'CASH' END AS CLOSETYPE"
        strSql += " ,BILLNO,DOCLOSE BILLDATE "
        strSql += " FROM " & chitDb & "SAVINGS..SCHEMEMAST"
        strSql += " WHERE GROUPCODE = '" & txtGrpCode.Text & "' AND REGNO = " & Val(txtRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(DOCLOSE,'') <> ''"
        Dim dtCloseInfo As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCloseInfo)
        If dtCloseInfo.Rows.Count > 0 Then
            With dtCloseInfo.Rows(0)
                MsgBox("Already Closed !!!" + vbCrLf _
                               + "Close Date : " + .Item("DOCLOSE").ToString + vbCrLf _
                               + "Close Type : " + .Item("CLOSETYPE").ToString + vbCrLf _
                               + "Bill No    : " + .Item("BILLNO").ToString + vbCrLf _
                               + "Bill Date  : " + .Item("BILLDATE").ToString)
            End With
            Exit Sub
        End If
        ''CustomerInfo
        strSql = " SELECT TOP 1 AMOUNT,RDATE,INSTALLMENT FROM " & chitDb & "SH0708..SCHEMETRAN"
        strSql += " WHERE GROUPCODE = '" & txtGrpCode.Text & "' AND REGNO = " & Val(txtRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(CANCEL,'') = ''  AND ISNULL(CHEQUERETDATE,'')= '' "
        strSql += " ORDER BY RDATE DESC ,INSTALLMENT DESC"
        dtCustomerInfo = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCustomerInfo)
        If dtCustomerInfo.Rows.Count > 0 Then
            With dtCustomerInfo.Rows(0)
                strSql = " SELECT ISNULL(SUM(AMOUNT),0) AS AMOUNT FROM " & chitDb & "SH0708..SCHEMETRAN WHERE "
                strSql += " GROUPCODE = '" & txtGrpCode.Text & "' AND REGNO = " & Val(txtRegNo_NUM.Text) & ""
                strSql += " AND ISNULL(CANCEL,'') = '' AND ISNULL(CHEQUERETDATE,'')=''"
                lblRecvAmt.Text = Format(Val(objGPack.GetSqlValue(strSql)), "0.00")
                lblRecvWt.Text = ""
                lblInsAmt.Text = Format(.Item("AMOUNT"), "0.00")
                lblPaid.Text = .Item("INSTALLMENT").ToString

                strSql = vbCrLf + " SELECT INSTALMENT FROM " & chitDb & "SAVINGS..SCHEME S "
                strSql += vbCrLf + " WHERE EXISTS (SELECT 1 FROM " & chitDb & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = '" & txtGrpCode.Text & "' AND REGNO = " & Val(txtRegNo_NUM.Text) & " AND SCHEMEID = S.SCHEMEID AND COMPANYID = S.COMPANYID)"
                lblTotIns.Text = objGPack.GetSqlValue(strSql)
            End With
        End If
        txtNoOfIns_NUM.Text = "1"
        txtNoOfIns_NUM.Focus()
    End Sub

    Private Sub ClearCustomerInfo()
        txtNoOfIns_NUM.Clear()
        txtTotAmount_AMT.Clear()
        lblTotIns.Text = ""
        lblInsAmt.Text = ""
        lblPaid.Text = ""
        lblRecvAmt.Text = ""
        lblRecvWt.Text = ""
        lblName.Text = ""
        lblAddress.Text = ""
    End Sub


    Private Sub txtGrpCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrpCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGrpCode.Text = "" Then
                MsgBox("Group Code Should not Emtpy", MsgBoxStyle.Information)
                txtGrpCode.Focus()
            End If
        End If
    End Sub

    Private Sub txtRegNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRegNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRegNo_NUM.Text = "" Then
                MsgBox("Regno should not empty", MsgBoxStyle.Information)
            Else
                If Not CheckDup() Then Exit Sub
                CustomerDetail()
            End If
        End If
    End Sub

    Private Sub frmBillChitReceipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtGrpCode.Clear()
        txtRegNo_NUM.Clear()
        ClearCustomerInfo()
    End Sub

    Private Function CheckDup() As Boolean
        For Each ro As DataRow In dtGridChit.Rows
            If ro!GROUPCODE.ToString = txtGrpCode.Text And ro!REGNO.ToString = txtRegNo_NUM.Text Then
                MsgBox("Already load into grid", MsgBoxStyle.Information)
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub txtTotAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTotAmount_AMT.KeyPress
        e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If txtGrpCode.Text = "" Then
                MsgBox("Group Code Should not Emtpy", MsgBoxStyle.Information)
                txtGrpCode.Focus()
                Exit Sub
            End If
            If txtRegNo_NUM.Text = "" Then
                MsgBox("Regno should not empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If Not CheckDup() Then
                Exit Sub
            End If
            If Not Val(txtNoOfIns_NUM.Text) > 0 Then
                MsgBox("Invalid Installment", MsgBoxStyle.Information)
                Exit Sub
            ElseIf (Val(txtNoOfIns_NUM.Text) + Val(lblPaid.Text)) > noOfIns Then
                MsgBox("Pending installment " & (noOfIns - Val(lblPaid.Text)).ToString & " only", MsgBoxStyle.Information)
                txtNoOfIns_NUM.Text = "1"
                txtNoOfIns_NUM.Focus()
                Exit Sub
            End If
            If Val(txtTotAmount_AMT.Text) = 0 Then
                Exit Sub
            End If
            If lblName.Text = "" Then
                MsgBox("Invalid Groupcode or RegNo", MsgBoxStyle.Information)
                txtGrpCode.Focus()
                Exit Sub
            End If
            ''Insertion or Updation
            Dim row As DataRow = Nothing
            If txtRowIndex.Text = "" Then row = dtGridChit.NewRow Else row = dtGridChit.Rows(Val(txtRowIndex.Text))
            row.Item("GROUPCODE") = txtGrpCode.Text
            row.Item("REGNO") = Val(txtRegNo_NUM.Text)
            row.Item("NOOFINS") = Val(txtNoOfIns_NUM.Text)
            row.Item("AMOUNT") = Val(txtTotAmount_AMT.Text)
            If txtRowIndex.Text = "" Then dtGridChit.Rows.Add(row)
            dtGridChit.AcceptChanges()
            txtGrpCode.Clear()
            txtRegNo_NUM.Clear()
            ClearCustomerInfo()
            txtGrpCode.Focus()
        End If
    End Sub

    Private Sub txtNoOfIns_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoOfIns_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtNoOfIns_NUM.Text) > 0 Then
                MsgBox("Invalid Installment", MsgBoxStyle.Information)
                Exit Sub
            ElseIf (Val(txtNoOfIns_NUM.Text) + Val(lblPaid.Text)) > noOfIns Then
                MsgBox("Pending installment " & (noOfIns - Val(lblPaid.Text)).ToString & " only", MsgBoxStyle.Information)
                txtNoOfIns_NUM.Text = "1"
                txtNoOfIns_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtNoOfIns_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoOfIns_NUM.TextChanged
        Dim amt As Double = Val(txtNoOfIns_NUM.Text) * Val(lblInsAmt.Text)
        txtTotAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), Nothing)
    End Sub

    Private Sub txtGrpCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrpCode.LostFocus
        If txtGrpCode.Text = "" Then Exit Sub
        If Not objGPack.GetSqlValue("SELECT 'CHECK' FROM " & chitDb & "SAVINGS..GROUPMAST WHERE GROUPCODE = '" & txtGrpCode.Text & "'").Length > 0 Then
            txtGrpCode.Clear()
            txtGrpCode.Focus()
        Else
            If objGPack.GetSqlValue("SELECT isnull(GROUPCODEFORALLAMOUNT,'') FROM " & chitDb & "SAVINGS..SCHEME WHERE GROUPCODE = '" & txtGrpCode.Text & "'").ToUpper = "Y" Then
                noOfIns = Val(objGPack.GetSqlValue("SELECT INSTALMENT FROM " & chitDb & "SAVINGS..SCHEME WHERE GROUPCODE = '" & txtGrpCode.Text & "'"))
            Else
                noOfIns = Val(objGPack.GetSqlValue("SELECT NOOFINS FROM " & chitDb & "SAVINGS..INSAMOUNT WHERE GROUPCODE = '" & txtGrpCode.Text & "'"))
            End If
        End If
    End Sub

    Private Function InsertChitEntry(ByVal _Rdate As String, ByVal _GrpCode As String, ByVal _RegNo As Integer _
    , ByVal _Accode As String, ByVal _PayMode As Char _
    , ByVal _Tran As OleDbTransaction, ByVal _SystemId As String, ByVal _EmpId As Integer, ByVal ST_ID As String)
        Dim _Weight As Double = Nothing
        Dim _Rate As Double = Nothing
        Dim _ReceiptNo As Integer = Nothing
        Dim _InsAmount As Double = Nothing
        Dim _SchemeId As String
        Dim _CompId As String
        Dim _WeightLedger As Boolean
        Dim _PromptRecNo As Boolean
        Dim _EntRefNon As Integer
        'Dim _Entrefno As String
        Dim _ConRecNo As Boolean
        Dim _Ins As Integer
        _InsAmount = objGPack.GetSqlValue("SELECT TOP 1 AMOUNT FROM " & chitDb & "SH0708..SCHEMETRAN WHERE GROUPCODE = '" & _GrpCode & "' AND REGNO = " & _RegNo & "", , , _Tran)
        strSql = "SELECT SCHEMEID FROM " & chitDb & "SAVINGS..SCHEMEMAST WHERE GROUPCODE= '" & _GrpCode & "' AND REGNO = " & _RegNo & ""
        _SchemeId = objGPack.GetSqlValue(strSql, , , _Tran)
        strSql = "SELECT COMPANYID FROM " & chitDb & "SAVINGS..SCHEMEMAST WHERE GROUPCODE= '" & _GrpCode & "' AND REGNO = " & _RegNo & ""
        _CompId = objGPack.GetSqlValue(strSql, , , _Tran)
        strSql = "SELECT WEIGHTLEDGER FROM " & chitDb & "SAVINGS..SCHEME WHERE SCHEMEID = '" & _SchemeId & "'"
        _WeightLedger = IIf(objGPack.GetSqlValue(strSql, , , _Tran).ToUpper = "Y", True, False)
        _PromptRecNo = IIf(objGPack.GetSqlValue("SELECT PROMPTRECTNO FROM " & chitDb & "SAVINGS..SCHEME WHERE  COMPANYID = '" & _CompId & "'  AND SCHEMEID = '" & _SchemeId & "'", , "N", _Tran) = "Y", True, False)

        '        ''Getting EntRefNo
        'GetEntRegNo:
        '        strSql = " SELECT CONVERT(INTEGER,CTLTEXT) AS CTLTEXT FROM " & chitDb & "SAVINGS..SOFTCONTROL WHERE CTLID = 'ENTREFNO'"
        '        _EntRefNon = Val(objGPack.GetSqlValue(strSql, , , _Tran))
        '        strSql = " UPDATE " & chitDb & "SAVINGS..SOFTCONTROL SET CTLTEXT = '" & _EntRefNon + 1 & "'"
        '        strSql += " WHERE CTLID = 'ENTREFNO' AND CTLTEXT = '" & _EntRefNon & "'"
        '        cmd = New OleDbCommand(strSql, cn, _Tran)
        '        cmd.ExecuteNonQuery()
        '        _Entrefno = GetCostId(cnCostId) + Mid(_Rdate, 3, 2) & _EntRefNon.ToString
        ''Getting ReceiptNo
        If Not _PromptRecNo Then
            strSql = "SELECT ISNULL(CONTRECEIPTNO,'')CONTRECEIPTNO FROM " & chitDb & "SAVINGS..SCHEME "
            strSql += " WHERE COMPANYID = '" & _CompId & "' AND SCHEMEID = '" & _SchemeId & "'"
            _ConRecNo = IIf(objGPack.GetSqlValue(strSql, , , _Tran).ToUpper = "Y", True, False)
            If _ConRecNo Then
                strSql = "SELECT ISNULL(STARTRECEIPTNO,0)STARTRECEIPTNO FROM " & chitDb & "SAVINGS..SCHEME "
                strSql += " WHERE CONTRECEIPTNO= 'Y' AND COMPANYID = '" & _CompId & "' "
                strSql += " AND SCHEMEID = '" & _SchemeId & "'"
                _ReceiptNo = Val(objGPack.GetSqlValue(strSql, , , _Tran))
                strSql = "UPDATE " & chitDb & "SAVINGS..SCHEME SET STARTRECEIPTNO = '" & _ReceiptNo + 1 & "' WHERE "
                strSql += " CONTRECEIPTNO= 'Y' AND COMPANYID = '" & _CompId & "' AND SCHEMEID = '" & _SchemeId & "'"
                cmd = New OleDbCommand(strSql, cn, _Tran)
                cmd.ExecuteNonQuery()
            Else
                strSql = "SELECT CONTRECEIPTNO FROM " & chitDb & "SAVINGS..COMPANY WHERE COMPANYID = '" & _CompId & "'"
                _ConRecNo = IIf(objGPack.GetSqlValue(strSql, , , _Tran).ToUpper = "Y", True, False)
                If _ConRecNo Then
                    strSql = "SELECT STARTRECEIPTNO FROM " & chitDb & "SAVINGS..COMPANY "
                    strSql += " WHERE CONTRECEIPTNO= 'Y' AND COMPANYID = '" & _CompId & "'"
                    _ReceiptNo = Val(objGPack.GetSqlValue(strSql, , , _Tran))
                    strSql = "UPDATE " & chitDb & "SAVINGS..COMPANY SET STARTRECEIPTNO = '" & _ReceiptNo + 1 & "' WHERE "
                    strSql += " CONTRECEIPTNO= 'Y' AND COMPANYID = '" & _CompId & "'"
                    cmd = New OleDbCommand(strSql, cn, _Tran)
                    cmd.ExecuteNonQuery()
                Else
                    strSql = " SELECT ISNULL(RNO,0) AS RNO FROM " & chitDb & "SAVINGS..INSAMOUNT WHERE "
                    strSql += " COMPANYID = '" & _CompId & "' AND SCHEMEID  = " & _SchemeId & ""
                    strSql += " AND AMOUNT = " & _InsAmount & ""
                    _ReceiptNo = Val(objGPack.GetSqlValue(strSql, , , _Tran))
                    strSql = " UPDATE " & chitDb & "SAVINGS..INSAMOUNT SET RNO = " & _ReceiptNo + 1 & ""
                    strSql += "  WHERE COMPANYID = '" & _CompId & "' AND"
                    strSql += " SCHEMEID = " & _SchemeId & " AND"
                    strSql += " AMOUNT = " & _InsAmount & ""
                    cmd = New OleDbCommand(strSql, cn, _Tran)
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If
        ST_ID = GetCostId(cnCostId) + Mid(_Rdate, 3, 2) & ST_ID
        If _WeightLedger Then If _Rate > 0 Then _Weight = _InsAmount / _Rate
        strSql = " SELECT COUNT(*)+1 INSTALLMENT FROM " & chitDb & "SH0708..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & _GrpCode & "' AND REGNO = " & _RegNo & " "
        strSql += " AND ISNULL(CANCEL,'') = '' AND ISNULL(CHEQUERETDATE,'') = ''"
        _Ins = Val(objGPack.GetSqlValue(strSql, , , _Tran))
        InsertIntoSchemeTran(cn, _Tran, _GrpCode, _RegNo, _InsAmount, _Weight _
        , _Rate, _ReceiptNo, _Rdate _
        , _SystemId, _Ins, _EmpId, "", _Entrefno, userId, ST_ID)
        InsertIntoSchemeCollect(cn, _Tran, _GrpCode, _RegNo, _ReceiptNo, _Rdate, _InsAmount _
        , _PayMode, _Accode, _Entrefno, _SystemId, 0 _
        , Nothing, Nothing, Nothing, Nothing, ST_ID)
        Return _Entrefno
    End Function
    Private Sub InsertIntoSchemeTran(ByVal _Cn As OleDbConnection, ByVal _Tran As OleDbTransaction _
     , ByVal _GrpCode As String, ByVal _RegNo As Integer _
    , ByVal _InsAmount As Double, ByVal _Weight As Double _
    , ByVal _Rate As Double, ByVal _ReceiptNo As Integer _
    , ByVal _Rdate As String, ByVal _SystemId As String _
    , ByVal _Installment As Integer, ByVal _EmpId As Integer _
    , ByVal _Remark As String, ByVal _EntRefNo As String _
    , ByVal _UserId As Integer, ByVal _ST_ID As String _
    )
        strSql = " INSERT INTO " & chitDb & "SH0708..SCHEMETRAN"
        strSql += " ("
        strSql += " sno,COSTID,GROUPCODE,REGNO,AMOUNT,WEIGHT"
        strSql += " ,RATE,RECEIPTNO,RDATE,CANCEL"
        strSql += " ,SYSTEMID,INSTALLMENT,EMPID,REMARKS"
        strSql += " ,ENTREFNO,CPERSON,UPDATETIME,USERID"
        strSql += " ,CHEQUERETDATE,BOOKNO,ST_ID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & _EntRefNo & "'" 'SNO
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & _GrpCode & "'" 'GROUPCODE
        strSql += " ," & _RegNo & "" 'REGNO
        strSql += " ," & _InsAmount & "" 'AMOUNT
        strSql += " ," & _Weight & "" 'WEIGHT
        strSql += " ," & _Rate & "" 'RATE
        strSql += " ," & _ReceiptNo & "" 'RECEIPTNO
        strSql += " ,'" & _Rdate & "'" 'RDATE
        strSql += " ,''" 'CANCEL
        strSql += " ,'" & _SystemId & "'" 'SYSTEMID
        strSql += " ," & _Installment & "" 'INSTALLMENT
        strSql += " ," & _EmpId & "" 'EMPID
        strSql += " ,'" & _Remark & "'" 'REMARKS
        strSql += " ,'" & _EntRefNo & "'" 'ENTREFNO
        strSql += ",0" 'CPERSON
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATETIME
        strSql += " ," & _UserId & "" 'USERID"
        strSql += ",NULL" 'CHEQUERETDATE
        strSql += ",''" 'BOOKNO
        strSql += ",'" & _EntRefNo & _ST_ID & "'" 'PK 
        strSql += " )"
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()
    End Sub


    Private Sub InsertIntoSchemeCollect(ByVal _Cn As OleDbConnection, ByVal _Tran As OleDbTransaction _
    , ByVal _GrpCode As String, ByVal _RegNo As Integer _
    , ByVal _ReceiptNo As Integer, ByVal _Rdate As String _
    , ByVal _InsAmount As Double, ByVal _ModePay As Char _
    , ByVal _Accode As String, ByVal _EntRefNo As String _
    , ByVal _SystemId As String, ByVal _UserId As Integer _
    , ByVal _ChqCardNo As String, ByVal _ChqDate As String _
    , ByVal _ChqBank As String, ByVal _ChqBankCode As Integer, ByVal _ST_ID As String _
    )
        strSql = " INSERT INTO " & chitDb & "SH0708..SCHEMECOLLECT"
        strSql += " ("
        strSql += " SNO,COSTID,GROUPCODE,REGNO,RECEIPTNO,RDATE"
        strSql += " ,AMOUNT,MODEPAY,ACCODE,ENTREFNO"
        strSql += " ,CANCEL,SYSTEMID,UPDATETIME,USERID"
        strSql += " ,CHQ_CARDNO,CHQDATE,CHQBANK"
        strSql += " ,CHQBANKCODE,CPERSON,CHEQUERETDATE,BOOKNO,SC_ID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & _EntRefNo & "'" 'PK 
        strSql += " ,'" & cnCostId & "'" 'GROUPCODE
        strSql += " ,'" & _GrpCode & "'" 'GROUPCODE
        strSql += " ," & _RegNo & "" 'REGNO
        strSql += " ," & _ReceiptNo & "" 'RECEIPTNO
        strSql += " ,'" & _Rdate & "'" 'RDATE
        strSql += " ," & _InsAmount & "" 'AMOUNT
        strSql += " ,'" & _ModePay & "'" 'MODEPAY
        strSql += " ,'" & _Accode & "'" 'ACCODE
        strSql += " ,'" & _EntRefNo & "'" 'ENTREFNO"
        strSql += " ,''" 'CANCEL
        strSql += " ,'" & _SystemId & "'" 'SYSTEMID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATETIME
        strSql += " ," & _UserId & "" 'USERID"
        strSql += " ,'" & _ChqCardNo & "'" 'CHQ_CARDNO
        If _ChqDate = Nothing Then strSql += ",NULL" Else strSql += " ,'" & _ChqDate & "'" 'CHQDATE
        strSql += " ,'" & _ChqBank & "'" 'CHQBANK
        strSql += " ," & _ChqBankCode & "" 'CHQBANKCODE
        strSql += ",0" 'CPERSON
        strSql += ",NULL" 'CHEQUERETDATE
        strSql += ",''" 'BOOKNO
        strSql += ",'" & _EntRefNo & _ST_ID & "'" 'PK 
        strSql += " )"
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()
    End Sub

    Public Function Save(ByVal _RDate As String, ByVal _Cn As OleDbConnection, ByVal _Tran As OleDbTransaction, ByVal _Accode As String _
    , ByVal _ModePay As Char, ByVal _SystemId As String, ByVal _EmpId As Integer)
        ''Getting EntRefNo
        Dim _EntRefNon As Integer
GetEntRegNo:
        strSql = " SELECT CONVERT(INTEGER,CTLTEXT) AS CTLTEXT FROM " & chitDb & "SAVINGS..SOFTCONTROL WHERE CTLID = 'ENTREFNO'"
        _EntRefNon = Val(objGPack.GetSqlValue(strSql, , , _Tran))
        strSql = " UPDATE " & chitDb & "SAVINGS..SOFTCONTROL SET CTLTEXT = '" & _EntRefNon + 1 & "'"
        strSql += " WHERE CTLID = 'ENTREFNO' AND CTLTEXT = '" & _EntRefNon & "'"
        cmd = New OleDbCommand(strSql, cn, _Tran)
        cmd.ExecuteNonQuery()
        _Entrefno = GetCostId(cnCostId) + Mid(_RDate, 3, 2) & _EntRefNon.ToString

        For Each ro As DataRow In dtGridChit.Rows
            Dim _i As Integer = Val(ro!NOOFINS.ToString)
            Dim inscnt As Integer = 0
            While _i <> 0
                Dim s As Integer = 0

                Dim ST_ID As String = "O"
                Dim ST_ID_VAL() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I"}
                ST_ID = ST_ID_VAL(inscnt).ToString

                InsertChitEntry(_RDate, ro("GROUPCODE").ToString _
                , Val(ro("REGNO").ToString), _Accode, _ModePay, _Tran, _SystemId, _EmpId, ST_ID)
                _i -= 1
                inscnt += 1
            End While
        Next
        Return _Entrefno
    End Function
End Class