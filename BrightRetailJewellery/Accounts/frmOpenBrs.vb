Imports System.Data.OleDb
Public Class frmOpenBrs

#Region "Variable Declaration"
    Dim strSql, str As String
    Dim dtGridView As New DataTable
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim costId As String = ""
    Dim ACCODE As String = ""
    'Dim Userid As Integer = 0
    Dim objWeighDet As New frmOpenDebitorsWeight
    Dim objToBe As New frmToBe
    Dim dsPendingItems As New DataSet
    Dim editSno As String = "-1"
    Dim Snum As Integer = 0
    Dim sno As String = Nothing
    Dim paymode As String = Nothing
    Dim AcName, Upsno, acd As String
    Dim TranFlag As Boolean = False
    Dim DNAME As String = cnStockDb.ToString
    Dim ro As DataRow
#End Region

#Region "New"
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        With dtGridView.Columns
            Dim SNO As New DataColumn("SNO", GetType(Integer))
            SNO.AutoIncrement = True
            SNO.AutoIncrementSeed = 1
            SNO.AutoIncrementStep = 1
            .Add("SNO", GetType(Integer))
            .Add("TRANNO", GetType(Integer))
            .Add("TRANDATE", GetType(Date))
            .Add("MODE", GetType(String))
            .Add("ACCODE", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("CHEQUENO", GetType(Int64))
            .Add("CHEQUEDATE", GetType(DateTime))
            .Add("BRSFLAG", GetType(String))
            .Add("REALISEDATE", GetType(DateTime))
            .Add("REMARK", GetType(String))
            .Add("USERID", GetType(Integer))
            .Add("UPDATE", GetType(DateTime))
            .Add("UPTIME", GetType(DateTime))
            .Add("SYSTEMID", GetType(String))
            .Add("COSTID", GetType(String))
            .Add("COMPANYID", GetType(String))
        End With

        gridView.DataSource = dtGridView
        StyleGridView()

    End Sub
#End Region
    Private Sub frmOpenBrs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If e.KeyCode = Keys.Escape And tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
            ElseIf e.KeyCode = Keys.Enter And btnSave.Focused = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOpenBrs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        gridOpenView.RowTemplate.Height = 21

        gridView.RowTemplate.Height = 21
        gridView.ColumnHeadersVisible = False

        cmbMode.Items.Clear()
        cmbMode.Items.Add("DEBIT")
        cmbMode.Items.Add("CREDIT")
        cmbMode.Text = "DEBIT"
        cmbMode.DropDownStyle = ComboBoxStyle.DropDownList

        cmbBRSFLAG.Items.Clear()
        cmbBRSFLAG.Items.Add("YES")
        cmbBRSFLAG.Items.Add("NO")
        cmbBRSFLAG.Text = "YES"
        cmbBRSFLAG.DropDownStyle = ComboBoxStyle.DropDownList

        cmbCostCentre_MAN.Enabled = True
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        cmbCostCentre_MAN.Focus()

        'If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
        '    cmbCostCentre_MAN.Enabled = True
        '    strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        '    objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        'Else
        '    cmbCostCentre_MAN.Enabled = False
        'End If
        'IF GetAdmindbSoftValue("
        'CmbAccName.Enabled = True

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACGRPCODE = '2' "
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        objGPack.FillCombo(strSql, CmbAccName)
        objGPack.FillCombo(strSql, cmbAcNameForVw)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)

        dtpOpenFrom.MaximumDate = cnTranFromDate.AddDays(-1)
        dtpOpenFrom.MinimumDate = cnTranFromDate.AddYears(-1)

        dtpOpenTo.MaximumDate = cnTranToDate.AddDays(-1)
        dtpOpenTo.MinimumDate = cnTranFromDate.AddYears(-1)

        dtpTrandate.MaximumDate = cnTranFromDate.AddDays(-1)
        dtpTrandate.MinimumDate = cnTranFromDate.AddYears(-1)

        dtpChequeDate.MaximumDate = cnTranFromDate.AddDays(-1)
        dtpChequeDate.MinimumDate = cnTranFromDate.AddYears(-1)

        dtprealisedate.MinimumDate = (New DateTimePicker).MinDate
        dtprealisedate.MaximumDate = (New DateTimePicker).MaxDate

        dtpOpenFrom.Value = GetEntryDate(cnTranFromDate.AddYears(-1))
        dtpOpenTo.Value = GetEntryDate(cnTranFromDate.AddDays(-1))
        dtpTrandate.Value = GetEntryDate(cnTranFromDate.AddDays(-1))
        dtpChequeDate.Value = GetEntryDate(cnTranFromDate.AddDays(-1))
        dtprealisedate.Value = GetEntryDate(cnTranFromDate.AddDays(+1))

        cmbBRSFLAG.SelectedItem = "YES"
        dtGridView.Rows.Clear()
        dtGridView.AcceptChanges()
        CalcTotal()
        setenablemode(True)
        editSno = "-1"
        tabGeneral.Select()
        If cmbCostCentre_MAN.Enabled Then
            cmbCostCentre_MAN.Focus()
        Else
            Me.SelectNextControl(cmbCostCentre_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Function GRIDLOAD()
        tran = Nothing
        tran = cn.BeginTransaction
        ACCODE = ""
        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAccName.SelectedItem & "'"
        ACCODE = objGPack.GetSqlValue(strSql, , , tran)
        tran.Commit()
        ro = dtGridView.NewRow
        ro("TRANNO") = Val(txtTranNo_NUM.Text.Trim.ToString)
        ro("TRANDATE") = dtpTrandate.Value
        ro("MODE") = cmbMode.Text
        ro("ACCODE") = ACCODE.ToString
        ro("AMOUNT") = IIf(Val(txtAmount_AMT.Text) <> 0, txtAmount_AMT.Text, DBNull.Value)
        ro("CHEQUENO") = IIf(Val(txtChequeNO_NUM.Text) <> 0, txtChequeNO_NUM.Text, DBNull.Value)
        ro("CHEQUEDATE") = dtpChequeDate.Value
        If cmbBRSFLAG.SelectedItem = "YES" Then
            ro("BRSFLAG") = "Y"
            ro("REALISEDATE") = dtprealisedate.Value
        Else
            ro("BRSFLAG") = "N"
            ro("REALISEDATE") = DBNull.Value
        End If
        ro("REMARK") = txtRemark.Text
        ro("USERID") = userId
        ro("UPDATE") = System.DateTime.Now
        ro("UPTIME") = System.DateTime.Now
        ro("COSTID") = GetCostId(costId).ToString
        ' str =systemId System.Environment.GetEnvironmentVariable("NODE-ID")
        ro("SYSTEMID") = systemId ' str.ToString.Trim.Substring(str.Length - 3, Val(str.Length))
        ro("COMPANYID") = GetCompanyId(strCompanyId).ToString

        dtGridView.Rows.Add(ro)
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            .Columns("SNO").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("USERID").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("COMPANYID").Visible = False
            .Columns("SYSTEMID").Visible = False
            .Columns("UPTIME").Visible = False
            .Columns("UPDATE").Visible = False
            .Columns("TRANNO").Width = txtTranNo_NUM.Width
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = dtpTrandate.Width + 2
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("MODE").Width = cmbMode.Width
            .Columns("MODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = txtAmount_AMT.Width
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("CHEQUENO").Width = txtChequeNO_NUM.Width + 2
            .Columns("CHEQUENO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CHEQUENO").DefaultCellStyle.Format = "0.00"
            .Columns("CHEQUEDATE").Width = dtpChequeDate.Width + 2
            .Columns("CHEQUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("BRSFLAG").Width = Val(cmbBRSFLAG.Width) + 1
            .Columns("BRSFLAG").Visible = True
            .Columns("BRSFLAG").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REALISEDATE").Width = Val(dtprealisedate.Width + 1)
            .Columns("REALISEDATE").Visible = True
            .Columns("REALISEDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REMARK").Width = Val(txtRemark.Width) + 2
            .Columns("REMARK").Visible = True
            .Columns("REMARK").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CmbAccName.SelectedItem = "" Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If cmbCostCentre_MAN.Enabled Then
            If Not costId.Length > 0 Then
                MsgBox("Invalid Costcentre", MsgBoxStyle.Information)
                cmbCostCentre_MAN.Focus()
                Exit Sub
            End If
        End If
        Try
            If editSno = "-1" Then
                tran = Nothing
                tran = cn.BeginTransaction
                For Each ro As DataRow In dtGridView.Rows
                    sno = GetNewSno(TranSnoType.BRS_ACCTRANCODE, tran, , strCompanyId, cnStockDb)
                    If ro!MODE.ToString = "DEBIT" Then
                        paymode = "D"
                    ElseIf ro!MODE.ToString = "CRETID" Then
                        paymode = "C"
                    End If
                    If ro!BRSFLAG = "Y" Then
                        InsertIntoBRDACCTRAN(tran _
                             , sno _
                             , Val(ro!TRANNO.ToString) _
                             , ro!TRANDATE _
                             , paymode _
                             , ro!ACCODE.ToString _
                             , Val(ro!AMOUNT.ToString) _
                             , Val(ro!CHEQUENO.ToString) _
                             , ro!CHEQUEDATE _
                             , ro!REMARK _
                             , ro!BRSFLAG _
                             , ro!USERID _
                             , ro!UPDATE _
                             , ro!UPTIME _
                             , ro!SYSTEMID _
                             , ro!COSTID _
                             , ro!COMPANYID _
                            , ro!REALISEDATE)

                    Else
                        InsertIntoBRDACCTRAN(tran _
                                      , ro!SNO.ToString _
                                      , Val(ro!TRANNO.ToString) _
                                      , ro!TRANDATE _
                                      , paymode _
                                      , ro!ACCODE.ToString _
                                      , Val(ro!AMOUNT.ToString) _
                                      , Val(ro!CHEQUENO.ToString) _
                                      , ro!CHEQUEDATE _
                                      , ro!REMARK _
                                      , ro!BRSFLAG _
                                      , ro!USERID _
                                      , ro!UPDATE _
                                      , ro!UPTIME _
                                      , ro!SYSTEMID _
                                      , ro!COSTID _
                                      , ro!COMPANYID)
                    End If
                Next
                tran.Commit()
                MsgBox("Saved")
            Else
                UpdateData()
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Dispose()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

#Region "InsertIntoTrailBalance"
    'Private Sub InsertIntoTrailBalance()
    '    strSql = " SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
    '    strSql += " FROM " & cnAdminDb & "..OUTSTANDING"
    '    strSql += " WHERE ACCODE = '" & ACCODE & "'"
    '    strSql += " AND COSTID = '" & costId & "'"
    '    strSql += " AND FROMFLAG = 'O'"
    '    strSql += " AND COMPANYID = '" & strCompanyId & "'"
    '    Dim amt As Double = Val(objGPack.GetSqlValue(strSql, , "0", tran))
    '    SaveIntoOpenTrailBAlance(ACCODE, IIf(amt > 0, amt, 0), IIf(amt > 0, 0, amt))
    'End Sub
#End Region

    Private Sub UpdateBrsAcctran(ByVal TRAN As OleDbTransaction _
    , ByVal ISSNO As String _
    , ByVal TRANNO As String _
    , ByVal TRANDATE As DateTime _
    , ByVal TRANMODE As String _
    , ByVal AMOUNT As String _
    , ByVal CHEQNO As String _
    , ByVal CHEQDATE As DateTime _
    , ByVal REMARK As String _
    , ByVal BRSFL As String _
    , ByVal UPDATE As DateTime _
    , ByVal UPTIM As DateTime _
    , Optional ByVal REALDATE As DateTime = Nothing)

        strSql = "Update " & cnStockDb & "..BRS_ACCTRAN SET "
        strSql += vbCrLf + "TRANNO ='" & TRANNO & "'"
        strSql += vbCrLf + ",TRANDATE='" & Format(TRANDATE, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",TRANMODE ='" & TRANMODE & "'"
        strSql += vbCrLf + ",AMOUNT ='" & AMOUNT & "'"
        strSql += vbCrLf + ",CHQCARDNO ='" & CHEQNO & "'"
        strSql += vbCrLf + ",CHQDATE ='" & Format(CHEQDATE, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",CHQCARDREF ='" & REMARK & "'"
        strSql += vbCrLf + ",BRSFLAG ='" & BRSFL & "'"
        If REALDATE.Date = Nothing Then
            strSql += vbCrLf + ",RELIASEDATE=NULL"
        Else
            strSql += vbCrLf + ",RELIASEDATE ='" & Format(REALDATE, "yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + ",UPDATED ='" & Format(UPDATE, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",UPTIME ='" & Format(UPTIM, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " WHERE SNO='" & ISSNO & "'"

        ExecQuery(SyncMode.Transaction, strSql, cn, TRAN, costId)
    End Sub

    Private Sub InsertIntoBRDACCTRAN(ByVal TRAN As OleDbTransaction _
    , ByVal ISSNO As String _
    , ByVal TRANNO As Double _
    , ByVal TRANDATE As DateTime _
    , ByVal TRANMODE As String _
    , ByVal ACODE As String _
    , ByVal AMOUNT As Double _
    , ByVal CHEQNO As Double _
    , ByVal CHEQDATE As DateTime _
    , ByVal REMARK As String _
    , ByVal BRSFL As String _
    , ByVal USERID As Integer _
    , ByVal UPDATE As DateTime _
    , ByVal UPTIM As DateTime _
    , ByVal SYSID As String _
    , ByVal COSTID As String _
    , ByVal COMPID As String _
    , Optional ByVal REALDATE As DateTime = Nothing)

        strSql = "INSERT INTO " & cnStockDb & "..BRS_ACCTRAN ("
        strSql += "SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,AMOUNT"
        strSql += ",CHQCARDNO,CHQDATE,CHQCARDREF, BRSFLAG"
        If REALDATE <> Nothing Then strSql += ",RELIASEDATE"
        strSql += ",USERID,UPDATED,UPTIME"
        strSql += ",SYSTEMID,COSTID,COMPANYID"
        strSql += ")"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & ISSNO & "'"
        strSql += " ," & TRANNO & ""
        strSql += " ,'" & Format(TRANDATE, "yyyy-MM-dd") & "'"
        strSql += " ,'" & TRANMODE & "'"
        strSql += " ,'" & ACODE & "'"
        strSql += " ," & AMOUNT & ""
        strSql += " ,'" & CHEQNO & "'"
        strSql += " ,'" & Format(CHEQDATE, "yyyy-MM-dd") & "'"
        strSql += " ,'" & REMARK & "'"
        strSql += " ,'" & BRSFL & "'"
        If REALDATE <> Nothing Then strSql += " ,'" & Format(REALDATE, "yyyy-MM-dd") & "'"
        strSql += " ,'" & USERID & "'"
        strSql += " ,'" & Format(UPDATE, "yyyy-MM-dd") & "'"
        strSql += " ,'" & Format(UPTIM, "yyyy-MM-dd") & "'"
        strSql += " ,'" & SYSID & "'"
        strSql += " ,'" & COSTID & "'"
        strSql += " ,'" & COMPID & "')"

        ExecQuery(SyncMode.Transaction, strSql, cn, TRAN, COSTID)

    End Sub

#Region "InsertIntoItemDetail"

    'Private Sub InsertIntoItemDetail(ByVal tran As OleDbTransaction, ByVal issSno As String, ByVal tranNo As Integer, ByVal tranType As String, ByVal billDate As Date _
    ', ByVal pcs As Integer, ByVal grswt As Double, ByVal netWt As Double, ByVal value As Double _
    ', ByVal remark1 As String, ByVal remark2 As String, ByVal recpay As Char, ByVal RUNNO As String, ByVal batchno As String)
    '        strSql = " INSERT INTO " & cnAdminDb & "..ITEMDETAIL"
    '        strSql += " ("
    '        strSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE,"
    '        strSql += " PCS,GRSWT,NETWT,AMOUNT,REMARK1,"
    '        strSql += " REMARK2,BATCHNO,CANCEL,FLAG,APPVER,"
    '        strSql += " COMPANYID,UPDATED,UPTIME,RECPAY"
    '        strSql += " ,RUNNO"
    '        strSql += " )"
    '        strSql += " VALUES"
    '        strSql += " ("
    '        strSql += " '" & GetNewSno(TranSnoType.ITEMDETAILCODE, tran) & "'" 'SNO
    '        strSql += " ,'" & issSno & "'" 'ISSSNO
    '        strSql += " ," & tranNo & "" 'TRANNO
    '        strSql += " ,'" & billDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
    '        strSql += " ,'" & tranType & "'" 'TRANTYPE
    '        strSql += " ," & pcs & "" 'PCS
    '        strSql += " ," & grswt & "" 'GRSWT
    '        strSql += " ," & netWt & "" 'NETWT
    '        strSql += " ," & value & "" 'AMOUNT
    '        strSql += " ,'" & remark1 & "'" 'REMARK1
    '        strSql += " ,'" & remark2 & "'" 'REMARK2
    '        strSql += " ,'" & batchno & "'" 'BATCHNO
    '        strSql += " ,''" 'CANCEL
    '        strSql += " ,''" 'FLAG
    '        strSql += " ,'" & VERSION & "'" 'APPVER
    '        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
    '        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '        strSql += " ,'" & recpay & "'" 'RECPAY
    '        strSql += " ,'" & RUNNO & "'" 'RUNNO
    '        strSql += " )"
    '        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    '    End Sub

#End Region

    Private Sub CalcTotal()
        Dim totamt As Double = Nothing
        dtGridView.AcceptChanges()
        For Each ro As DataRow In dtGridView.Rows
            If ro!TYPE.ToString = "RECEIPT" And ro!MODE.ToString = "ADVANCE" Then
                totamt -= Val(ro!AMOUNT.ToString)
            ElseIf ro!TYPE.ToString = "PAYMENT" And ro!MODE.ToString = "DUE" Then
                totamt += Val(ro!AMOUNT.ToString)
            ElseIf ro!TYPE.ToString = "RECEIPT" And ro!MODE.ToString = "DUE REC" Then
                totamt -= Val(ro!AMOUNT.ToString)
            End If
        Next
        'If totamt > 0 Then
        '    lblTotal.Text = "TOTAL : " & Format(totamt, "0.00") & "Dr"
        'ElseIf totamt < 0 Then
        '    lblTotal.Text = "TOTAL : " & Format(-1 * totamt, "0.00") & "Cr"
        'Else
        '    lblTotal.Text = ""
        'End If
    End Sub

#Region "SaveIntoOpenTrailBAlance"
    'Private Sub SaveIntoOpenTrailBAlance(ByVal accode As String, ByVal debit As Double, ByVal credit As Double)
    '    strSql = "SELECT ACCODE FROM " & cnStockDb & "..OPENTRAILBALANCE"
    '    strSql += " WHERE ACCODE = '" & accode & "'"
    '    strSql += " AND COSTID = '" & costId & "'"
    '    strSql += " AND COMPANYID = '" & strCompanyId & "'"
    '    If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
    '        ''UPDATE
    '        strSql = " UPDATE " & cnStockDb & "..OPENTRAILBALANCE SET"
    '        strSql += " ACCODE = '" & accode & "'"
    '        strSql += " ,DEBIT = " & Math.Abs(debit) & ""
    '        strSql += " ,CREDIT = " & Math.Abs(credit) & ""
    '        strSql += " ,COSTID = '" & costId & "'"
    '        strSql += " ,USERID  = " & userId & ""
    '        strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
    '        strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
    '        strSql += " ,SETTLEMENT = ''"
    '        strSql += " ,DEBITWT = 0"
    '        strSql += " ,CREDITWT = 0"
    '        strSql += " ,APPVER = '" & VERSION & "'"
    '        strSql += " ,COMPANYID = '" & strCompanyId & "'"
    '        strSql += " ,APPROVAL = ''"
    '        strSql += " ,SETSNO = ''"
    '        strSql += " ,SETREFNO = ''"
    '        strSql += " WHERE ACCODE = '" & accode & "'"
    '        strSql += " AND COSTID = '" & costId & "'"
    '        strSql += " AND COMPANYID = '" & strCompanyId & "'"
    '        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    '    Else
    '        ''INSERT
    '        strSql = " INSERT INTO " & cnStockDb & "..OPENTRAILBALANCE("
    '        strSql += " ACCODE,DEBIT,CREDIT,COSTID"
    '        strSql += " ,USERID,UPDATED,UPTIME,SETTLEMENT"
    '        strSql += " ,DEBITWT,CREDITWT,APPVER"
    '        strSql += " ,COMPANYID,APPROVAL,SETSNO,SETREFNO"
    '        strSql += " )"
    '        strSql += " VALUES"
    '        strSql += " ("
    '        strSql += " '" & accode & "'" 'ACCODE
    '        strSql += " ," & Math.Abs(debit) & "" 'DEBIT
    '        strSql += " ," & Math.Abs(credit) & "" 'CREDIT
    '        strSql += " ,'" & costId & "'" 'COSTID
    '        strSql += " ," & userId & "" 'USERID
    '        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '        strSql += " ,''" 'SETTLEMENT
    '        strSql += " ,0" 'DEBITWT
    '        strSql += " ,0" 'CREDITWT
    '        strSql += " ,'" & VERSION & "'" 'APPVER
    '        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
    '        strSql += " ,''" 'APPROVAL
    '        strSql += " ,''" 'SETSNO
    '        strSql += " ,''" 'SETREFNO
    '        strSql += ")"
    '        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    '    End If
    'End Sub
#End Region

#Region "InsertIntoPersonalinfo"
    'Private Sub InsertIntoPersonalinfo(ByVal tran As OleDbTransaction, ByVal batchno As String, ByVal accode As String, ByVal billdate As Date _
    ', ByVal Pname As String, ByVal address1 As String, ByVal address2 As String)
    '    Dim psno As String = frmAddressDia.GetPersonalInfoSno(tran)
    '    strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
    '    strSql += " ("
    '    strSql += " SNO,ACCODE,TRANDATE,TITLE"
    '    strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
    '    strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
    '    strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
    '    strSql += " ,MOBILE,EMAIL,FAX,APPVER"
    '    strSql += " ,PREVILEGEID,COMPANYID,COSTID"
    '    strSql += " )"
    '    strSql += " VALUES"
    '    strSql += " ("
    '    strSql += " '" & psno & "'" ''SNO
    '    strSql += " ,'" & accode & "'" 'ACCODE
    '    strSql += " ,'" & billdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
    '    strSql += " ,''" 'TITLE
    '    strSql += " ,''" 'INITIAL
    '    strSql += " ,'" & Pname & "'" 'PNAME
    '    strSql += " ,''" 'DOORNO
    '    strSql += " ,'" & address1 & "'" 'ADDRESS1
    '    strSql += " ,'" & address2 & "'" 'ADDRESS2
    '    strSql += " ,''" 'ADDRESS3
    '    strSql += " ,''" 'AREA
    '    strSql += " ,''" 'CITY
    '    strSql += " ,''" 'STATE
    '    strSql += " ,''" 'COUNTRY
    '    strSql += " ,''" 'PINCODE
    '    strSql += " ,''" 'PHONERES
    '    strSql += " ,''" 'MOBILE
    '    strSql += " ,''" 'EMAIL
    '    strSql += " ,''" 'Fax
    '    strSql += " ,'" & VERSION & "'" 'APPVER
    '    strSql += " ,''" 'PREVILEGEID
    '    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
    '    strSql += " ,'" & costId & "'" 'COSTID
    '    strSql += " )"
    '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

    '    strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
    '    strSql += " (BATCHNO,PSNO,COSTID)VALUES"
    '    strSql += " ('" & batchno & "','" & psno & "','" & costId & "')"
    '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    'End Sub
#End Region

#Region "InsertIntoOustanding"
    'Private Sub InsertIntoOustanding _
    '( _
    'ByVal BATCHNO As String, ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
    'ByVal RecPay As String, _
    'ByVal recDate As Date, _
    'ByVal paymode As String, _
    'Optional ByVal GrsWt As Double = 0, _
    'Optional ByVal NetWt As Double = 0, _
    'Optional ByVal CatCode As String = Nothing, _
    'Optional ByVal Rate As Double = Nothing, _
    'Optional ByVal Value As Double = Nothing, _
    'Optional ByVal refNo As String = Nothing, _
    'Optional ByVal refDate As String = Nothing, _
    'Optional ByVal purity As Double = Nothing, _
    'Optional ByVal proId As Integer = Nothing, _
    'Optional ByVal dueDate As String = Nothing, _
    'Optional ByVal Remark1 As String = Nothing, _
    'Optional ByVal Remark2 As String = Nothing, _
    'Optional ByVal Accode As String = Nothing, _
    'Optional ByVal Empid As Integer = Nothing _
    '   )
    '    If Amount = 0 And GrsWt = 0 Then Exit Sub
    '    'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
    '    strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
    '    strSql += " ("
    '    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
    '    strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
    '    strSql += " ,REFNO,REFDATE,EMPID"
    '    strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
    '    strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
    '    strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
    '    strSql += " VALUES"
    '    strSql += " ("
    '    strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran) & "'" ''SNO
    '    strSql += " ," & tNo & "" 'TRANNO
    '    strSql += " ,'" & recDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
    '    strSql += " ,'" & tType & "'" 'TRANTYPE
    '    strSql += " ,'" & RunNo & "'" 'RUNNO
    '    strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
    '    strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
    '    strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
    '    strSql += " ,'" & RecPay & "'" 'RECPAY
    '    strSql += " ,'" & refNo & "'" 'REFNO
    '    If refDate <> Nothing Then
    '        strSql += " ,'" & refDate & "'" 'REFDATE
    '    Else
    '        strSql += " ,NULL" 'REFDATE
    '    End If

    '    strSql += " ," & Empid & "" 'EMPID
    '    strSql += " ,''" 'TRANSTATUS
    '    strSql += " ," & purity & "" 'PURITY
    '    strSql += " ,'" & CatCode & "'" 'CATCODE
    '    strSql += " ,'" & BATCHNO & "'" 'BATCHNO
    '    strSql += " ," & userId & "" 'USERID

    '    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '    strSql += " ,'" & systemId & "'" 'SYSTEMID
    '    strSql += " ," & Rate & "" 'RATE
    '    strSql += " ," & Value & "" 'VALUE
    '    strSql += " ,''" 'CASHID
    '    strSql += " ,'" & Remark1 & "'" 'REMARK1
    '    strSql += " ,'" & Remark2 & "'" 'REMARK1
    '    strSql += " ,'" & Accode & "'" 'ACCODE
    '    strSql += " ," & proId & "" 'CTRANCODE
    '    If dueDate <> Nothing Then
    '        strSql += " ,'" & dueDate & "'" 'DUEDATE
    '    Else
    '        strSql += " ,NULL" 'DUEDATE
    '    End If
    '    strSql += " ,'" & VERSION & "'" 'APPVER
    '    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
    '    strSql += " ,'" & costId & "'" 'COSTID
    '    strSql += " ,'O'" 'FROMFLAG
    '    strSql += " ,'" & paymode & "'" 'PAYMODE
    '    strSql += " )"
    '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    'End Sub
#End Region

#Region "ToolStripMenuItem"
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub
#End Region

#Region "Sir Coding"

    Private Sub txtAccCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtAccCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Insert Then
            LoadAcc()
        End If
    End Sub

    ''Private Sub txtAccCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    ''    If e.KeyChar = Chr(Keys.Enter) Then
    ''        If txtAccCode.Text <> "" And objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccCode.Text & "'").Length > 0 Then
    ''            LoadAccDetails()
    ''        Else
    ''            LoadOutstDetails()
    ''        End If
    ''    End If
    ''End Sub

    Private Sub txtAccCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Main.HideHelpText()
    End Sub

    Private Sub LoadAcc()
        strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
        strSql += "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE 1=1"
        strSql += GetAcNameQryFilteration()
        'strSql += "  WHERE ACTYPE <> 'O'"
        CmbAccName.SelectedItem = BrighttechPack.SearchDialog.Show("Search Account Code", strSql, cn, 1)
        LoadAccDetails()
    End Sub

    Private Sub LoadAccDetails()
        If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & CmbAccName.SelectedItem & "'" & GetAcNameQryFilteration()).Length > 0 Then
            Dim dtAddDet As New DataTable
            strSql = " SELECT ACNAME,DOORNO + ' '+ ISNULL(ADDRESS1,'') + ' '+ ISNULL(ADDRESS2,'') AS ADDRESS1,"
            strSql += " ISNULL(ADDRESS3,'') + ' ' + ISNULL(AREA,'') + ' ' + ISNULL(CITY,'') AS ADDRESS2 "
            strSql += " FROM " & cnAdminDb & "..ACHEAD"
            strSql += " WHERE ACNAME = '" & CmbAccName.SelectedItem & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAddDet)
            ''For Each ro As DataRow In dtAddDet.Rows
            ''    txtAccName_MAN.Text = ro!ACNAME.ToString
            ''    txtAddress1.Text = Trim(ro!ADDRESS1.ToString)
            ''    txtAddress2.Text = Trim(ro!ADDRESS2.ToString)
            ''Next
            txtTranNo_NUM.Focus()
            ''Else
            ''    txtAccCode.Clear()
            ''    txtAccName_MAN.Clear()
            ''    txtAddress1.Clear()
            ''    txtAddress2.Clear()
            ''    txtAccName_MAN.Focus()
        End If
        LoadOutstDetails()
    End Sub

    Private Sub LoadOutstDetails()
        dtGridView.Rows.Clear()
        ACCODE = Nothing
        ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & CmbAccName.SelectedText & "'", , "DRS")
        'strSql = " SELECT TRANNO,TRANDATE,CASE WHEN RECPAY = 'R' THEN 'RECEIPT' ELSE 'PAYMENT' END AS TYPE"
        'strSql += " ,CASE WHEN RECPAY = 'R' AND TRANTYPE = 'A' THEN 'ADVANCE'"
        'strSql += "       WHEN RECPAY = 'R' AND TRANTYPE = 'D' THEN 'DUE REC'"
        'strSql += "       ELSE 'DUE' END AS MODE"
        'strSql += " ,SUBSTRING(RUNNO,4,LEN(RUNNO)) REFNO,CASE WHEN AMOUNT <> 0 THEN AMOUNT ELSE NULL END AMOUNT"
        'strSql += " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END AS WEIGHT,REMARK1 REMARK,EMPID"
        'strSql += " ,(SELECT CATNAME FROM " & cnadmindb & "..CATEGORY WHERE CATCODE = O.CATCODE)AS CATEGORY,PURITY,RATE"
        'strSql += " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        'strSql += " WHERE ACCODE = '" & ACCODE & "'"
        'strSql += " AND FROMFLAG = 'O'"
        'strSql += " AND COMPANYID = '" & strCompanyId & "'"
        'strSql += " AND COSTID = '" & costId & "'"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGridView)
        If dtGridView.Rows.Count > 0 Then gridView.CurrentCell = gridView.Rows(gridView.RowCount - 1).Cells(0)
        CalcTotal()
        ''txtAccName_MAN.Focus()
    End Sub

    Private Sub cmbType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        cmbMode.Items.Clear()
        ''If cmbType.Text = "RECEIPT" Then
        cmbMode.Items.Add("ADVANCE")
        cmbMode.Items.Add("DUE REC")
        cmbMode.Text = "ADVANCE"
        ''Else
        cmbMode.Items.Add("DUE")
        cmbMode.Text = "DUE"
        ''End If
    End Sub

    Private Sub gridView_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserAddedRow
        If editSno = "1" Then
            MsgBox("Pls Save First!")
        End If
    End Sub

    'Private Sub txtEmpId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Insert) Then
    '        LoadEmpId(txtEmpId_NUM)
    '    ElseIf e.KeyChar = Chr(Keys.Enter) Then
    '        If cmbCostCentre_MAN.Enabled Then
    '            If cmbCostCentre_MAN.Text = "" Then
    '                MsgBox("Costcentre should not empty", MsgBoxStyle.Information)
    '                cmbCostCentre_MAN.Focus()
    '                Exit Sub
    '            End If
    '            If Not cmbCostCentre_MAN.Items.Contains(cmbCostCentre_MAN.Text) Then
    '                MsgBox("Invalid Costcentre", MsgBoxStyle.Information)
    '                cmbCostCentre_MAN.Focus()
    '                Exit Sub
    '            End If
    '        Else
    '            cmbCostCentre_MAN.Text = ""
    '        End If
    '        If CmbAccName.SelectedText = "" Then
    '            MsgBox("AccName should not empty", MsgBoxStyle.Information)
    '            CmbAccName.Focus()
    '            Exit Sub
    '        End If
    '        If Val(txtAmount_AMT.Text) = 0 And Val(txtChequeNO.Text) = 0 Then
    '            MsgBox("Amount and Weight should not empty", MsgBoxStyle.Information)
    '            txtAmount_AMT.Focus()
    '            Exit Sub
    '        End If
    '        If Val(txtChequeNO.Text) <> 0 And Not objGPack.GetSqlValue("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & objWeighDet.txtCategory.Text & "'").Length > 0 Then
    '            ShowWeightDetail()
    '            Exit Sub
    '        End If
    '        ''If txtEmpId_NUM.Text = "" Then
    '        ''    LoadEmpId(txtEmpId_NUM)
    '        ''    Exit Sub
    '        ''ElseIf Not Val(objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_NUM.Text) & "")) > 0 Then
    '        ''    LoadEmpId(txtEmpId_NUM)
    '        ''    Exit Sub
    '    End If

    '    ACCODE = ""
    '    strSql = ""
    '    strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCNAME ='" & CmbAccName.SelectedText & "'"
    '    ACCODE = objGPack.GetSqlValue(strSql)
    '    Dim ro As DataRow = dtGridView.NewRow
    '    ro("TRANNO") = txtTranNo_NUM_MAN.Text
    '    ro("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
    '    ro("MODE") = cmbMode.Text
    '    ro("ACCODE") = ACCODE
    '    ro("AMOUNT") = IIf(Val(txtAmount_AMT.Text) <> 0, txtAmount_AMT.Text, DBNull.Value)
    '    ro("CHEQUENO") = IIf(Val(txtChequeNO.Text) <> 0, txtChequeNO.Text, DBNull.Value)
    '    ro("CHEQUEDATE") = IIf(Val(dtpChequeDate.Text) <> 0, dtpChequeDate.Text, DBNull.Value)
    '    ro("REMARK") = ""

    '    If cmbBRSFLAG.SelectedText = "YES" Then
    '        ro("BRSFLAG") = cmbBRSFLAG.SelectedText.ToString()
    '        ro("REALISEDATE") = dtprealisedate.Value.ToString("yyyy-MM-dd")
    '    Else
    '        ro("BRSFLAG") = ""
    '        ro("REALISEDATE") = ""
    '    End If

    '    ro("USERID") = ""
    '    ro("UPDATED") = System.DateTime.Now
    '    ro("UPTIME") = System.DateTime.Now.TimeOfDay
    '    ro("SYSTEMID") = ""
    '    ro("COSTID") = GetCostId(costId)
    '    ro("COMPANYID") = GetCompanyId(strCompanyId)
    '    dtGridView.Rows.Add(ro)
    '    dtGridView.AcceptChanges()
    '    If objToBe.dtGridToBe.Rows.Count > 0 Then
    '        Dim dt As New DataTable
    '        dt = objToBe.dtGridToBe.Copy
    '        dt.TableName = gridView.Rows(gridView.RowCount - 1).Cells("KEYNO").Value.ToString
    '        dsPendingItems.Tables.Add(dt)
    '    End If

    '    ''CLEAR
    '    objGPack.TextClear(grpDetails)
    '    objToBe = New frmToBe
    '    CalcTotal()
    '    txtTranNo_NUM_MAN.Focus()
    '    If Not editSno = "-1" Then btnSave_Click(Me, New EventArgs)
    '    End If
    'End Sub

    'Private Sub txtEmpId_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Main.HideHelpText()
    'End Sub

    'Private Sub skip_focus(ByVal sender As Object, ByVal e As EventArgs)
    '    If txtTranNo_NUM.Text <> "" Then
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        CalcTotal()
        If Not gridView.RowCount > 0 Then txtTranNo_NUM.Focus()
    End Sub

    'Private Sub gridFocusAtDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    'txtTranNo_NUM_MAN.KeyDown, txtAmount_AMT.KeyDown, txtChequeNO.KeyDown
    '    'If e.KeyCode = Keys.Down And gridView.RowCount > 0 Then
    '    '    gridView.Focus()
    '    'ElseIf e.KeyCode = Keys.Escape And CType(sender, TextBox).Name = txtTranNo_NUM_MAN.Name And gridView.RowCount > 0 Then
    '    '    btnSave.Focus()
    '    'End If
    'End Sub

    Private Sub ShowToBeDetail()
        objToBe.StartPosition = FormStartPosition.CenterScreen
        objToBe.ShowDialog()
        Me.SelectNextControl(txtRemark, True, True, True, True)
    End Sub

    Private Sub ShowWeightDetail()
        If Val(txtChequeNO_NUM.Text) > 0 Then
            Dim pt As New Point
            pt = New Point(570, 285)
            objWeighDet.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            objWeighDet.StartPosition = FormStartPosition.Manual
            objWeighDet.Location = pt
            objWeighDet.ShowDialog()
        End If
        Me.SelectNextControl(txtChequeNO_NUM, True, True, True, True)
    End Sub

    Private Sub gridOpenView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.LostFocus
        lblEdit.Visible = False
        lblDelete.Visible = False
    End Sub

    Private Sub gridOpenView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridOpenView.RowEnter
        With gridOpenView.Rows(e.RowIndex)
            'lblType.Text = .Cells("RECPAY").Value.ToString
            'lblEmpname.Text = .Cells("EMPNAME").Value.ToString

            'lblAddress1.Text = .Cells("ADDRESS1").Value.ToString
            'lblAddress2.Text = .Cells("ADDRESS2").Value.ToString

            'lblPurity.Text = IIf(Val(.Cells("PURITY").Value.ToString) <> 0, .Cells("PURITY").Value.ToString, Nothing)
            'lblRate.Text = IIf(Val(.Cells("RATE").Value.ToString) <> 0, .Cells("RATE").Value.ToString, Nothing)
            'lblCostcentre.Text = .Cells("COSTNAME").Value.ToString
        End With
    End Sub
#End Region

    Private Sub StyleGridView()

        With gridView
            .Columns("TRANNO").Width = txtTranNo_NUM.Width + 1
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = dtpTrandate.Width + 1
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("MODE").Width = cmbMode.Width + 1
            .Columns("AMOUNT").Width = txtAmount_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("CHEQUENO").Width = txtChequeNO_NUM.Width + 1
            .Columns("CHEQUENO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CHEQUENO").DefaultCellStyle.Format = "0.00"
            .Columns("CHEQUEDATE").Width = dtpChequeDate.Width + 1
            .Columns("CHEQUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("REMARK").Width = txtRemark.Width + 1
            .Columns("BRSFLAG").Width = cmbBRSFLAG.Width + 1
            .Columns("REALISEDATE").Width = dtprealisedate.Width + 1
            .Columns("REALISEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

            For cnt As Integer = 9 To gridView.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Sub

    Private Sub CmbAccName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbAccName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbAccName.SelectedItem = "" Then
                MsgBox("AccName should not empty", MsgBoxStyle.Information)
                CmbAccName.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbBRSFLAG_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus, CmbAccName.LostFocus
        costId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        chkOpenDate.Checked = False
        gridOpenView.DataSource = Nothing
        tabMain.SelectedTab = tabView
        chkOpenDate.Focus()
        openGridLoad()
    End Sub

    Function openGridLoad()
        Dim dt As New DataTable
        gridOpenView.DataSource = Nothing
        strSql = String.Empty
        strSql = "SELECT TRANNO,TRANDATE"
        strSql += vbCrLf + ",TRANMODE AS MODE,SNO,ACCODE"
        strSql += vbCrLf + ",AMOUNT,CHQCARDNO AS CHEQUENO,CHQDATE "
        strSql += vbCrLf + ",(CASE WHEN BRSFLAG='Y' THEN 'YES' ELSE 'NO' END)BRSFLAG"
        strSql += vbCrLf + ",RELIASEDATE,CHQCARDREF AS REMARK FROM " & cnStockDb & "..BRS_ACCTRAN "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtView As New DataTable
        da.Fill(dtView)
        If Not dtView.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If

        gridOpenView.DataSource = dtView
        gridOpenView.Columns("ACCODE").Visible = False
        gridOpenView.Columns("SNO").Visible = False
        With gridOpenView
            With .Columns("TRANDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("CHQDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("RELIASEDATE")
                .Width = 100
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
        End With
        'With gridOpenView
        '    .Columns("TRANDATE").ValueType(DateTime)
        'End With

    End Function

    Private Sub btnOpenSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOSearch_View.Click
        Dim dt As New DataTable
        gridOpenView.DataSource = Nothing
        strSql = String.Empty
        strSql = "SELECT TRANNO,TRANDATE "
        strSql += vbCrLf + ",TRANMODE AS MODE"
        strSql += vbCrLf + ",AMOUNT,SNO,ACCODE,CHQCARDNO AS CHEQUENO,CHQDATE "
        strSql += vbCrLf + ",(CASE WHEN BRSFLAG='Y' THEN 'YES' ELSE 'NO' END)BRSFLAG"
        strSql += vbCrLf + ",RELIASEDATE ,CHQCARDREF AS REMARK FROM " & cnStockDb & "..BRS_ACCTRAN"
        If cmbAcNameForVw.SelectedItem <> "" Then
            acd = cmbAcNameForVw.SelectedItem.ToString
            strSql += vbCrLf + " WHERE ACCODE =(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD "
            strSql += vbCrLf + " WHERE LTRIM(ACNAME) = '" & acd & "')"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpOpenFrom.Value, "yyyy-MM-dd") & "'AND '" & Format(dtpOpenTo.Value, "yyyy-MM-dd") & "'"
        ElseIf chkOpenDate.Checked Then
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpOpenFrom.Value, "yyyy-MM-dd") & "'AND '" & Format(dtpOpenTo.Value, "yyyy-MM-dd") & "'"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtView As New DataTable
        da.Fill(dtView)
        If Not dtView.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridOpenView.DataSource = dtView
        gridOpenView.Columns("ACCODE").Visible = False
        gridOpenView.Columns("SNO").Visible = False
        With gridOpenView
            With .Columns("TRANDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("CHQDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("RELIASEDATE")
                .Width = 100
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
        End With
    End Sub

    Private Sub gridOpenView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.GotFocus
        lblEdit.Visible = True
        lblDelete.Visible = True
    End Sub

    Private Sub gridOpenView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridOpenView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Do you want to Delete?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
            tran = Nothing
            With gridOpenView.Rows(gridOpenView.CurrentCell.RowIndex)
                Try
                    tran = cn.BeginTransaction
                    DeleteRecord(.Cells("SNO").Value.ToString)
                    'InsertIntoTrailBalance()
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Deleted Successfully")
                    openGridLoad()
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End With
        End If
    End Sub

    Function DeleteRecord(ByVal SNUM As String)
        strSql = "DELETE  FROM " & cnStockDb & "..BRS_ACCTRAN WHERE SNO='" & SNUM & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Function



    'Private Sub gridFocusAtDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    '  txtRemark.KeyDown
    '    If e.KeyCode = Keys.Down And gridView.RowCount > 0 Then
    '        gridView.Focus()
    '    ElseIf e.KeyCode = Keys.Escape And CType(sender, TextBox).Name = txtTranNo_NUM.Name And gridView.RowCount > 0 Then
    '        btnSave.Focus()
    '    End If
    'End Sub

    Function UpdateData()
        Dim updt As DateTime
        Dim uptim As DateTime
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            If editSno = "1" Then
                'ro = dtGridView.NewRow
                updt = System.DateTime.Now
                uptim = System.DateTime.Now
                If cmbMode.SelectedItem.ToString = "DEBIT" Then
                    paymode = "D"
                Else
                    paymode = "C"
                End If
                If ro!BRSFLAG = "Y" Then
                    UpdateBrsAcctran(tran, Upsno, Val(ro!TRANNO.ToString), ro!TRANDATE, paymode _
                             , Val(ro!AMOUNT.ToString), Val(ro!CHEQUENO.ToString), ro!CHEQUEDATE _
                             , ro!REMARK, "Y", ro!UPDATE, ro!UPTIME, ro!REALISEDATE)

                    'tran, Upsno, txtTranNo_NUM.Text.ToString _
                    '      , dtpTrandate.Value, paymode, txtAmount_AMT.Text.Trim.ToString _
                    '      , txtChequeNO_NUM.Text.Trim.ToString, dtpChequeDate.Value _
                    '      , txtRemark.Text.ToString, "Y", updt, uptim, dtprealisedate.Value)
                Else
                    UpdateBrsAcctran(tran, Upsno, Val(ro!TRANNO.ToString), ro!TRANDATE, paymode _
                             , Val(ro!AMOUNT.ToString), Val(ro!CHEQUENO.ToString), ro!CHEQUEDATE _
                             , ro!REMARK, "N", ro!UPDATE, ro!UPTIME)
                End If
                'DeleteOutstDetail(editSno)
                MsgBox("Updated Successfully")
            End If
            editSno = "-1"
            tran.Commit()
            'btnNew_Click(Me, New EventArgs)
            'cmbCostCentre_MAN.Focus()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Dispose()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Function GetAccName(ByVal accode As String)
        strSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE LTRIM(ACCODE) = '" & AcName & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtView As New DataTable
        da.Fill(dtView)
        Return dtView
    End Function

    Private Sub gridOpenView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridOpenView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridOpenView.RowCount > 0 Then
                Exit Sub
            End If
            setenablemode(True)
            gridOpenView.CurrentCell = gridOpenView.Rows(gridOpenView.CurrentCell.RowIndex).Cells(0)
            With gridOpenView.Rows(gridOpenView.CurrentCell.RowIndex)
                dt = New DataTable
                AcName = .Cells("ACCODE").Value.ToString
                dt = GetAccName(AcName)
                If dt.Rows.Count > 0 Then
                    CmbAccName.Text = dt.Rows(0).Item("ACNAME").ToString
                End If
                txtTranNo_NUM.Text = .Cells("TRANNO").Value.ToString
                dtpTrandate.Value = .Cells("TRANDATE").Value
                cmbMode.Text = .Cells("MODE").Value.ToString
                txtAmount_AMT.Text = .Cells("AMOUNT").Value.ToString
                txtChequeNO_NUM.Text = .Cells("CHEQUENO").Value.ToString
                dtpChequeDate.Value = .Cells("CHQDATE").Value
                Upsno = .Cells("SNO").Value.ToString
                If (.Cells("BRSFLAG").Value.ToString) = "YES" Then
                    cmbBRSFLAG.Text = "YES"
                    dtprealisedate.Value = .Cells("RELIASEDATE").Value
                    dtprealisedate.Enabled = True
                Else
                    cmbBRSFLAG.Text = "NO"
                    dtprealisedate.Enabled = False
                End If

                txtRemark.Text = .Cells("REMARK").Value.ToString
                editSno = "1"
                tabMain.SelectedTab = tabGeneral
                txtTranNo_NUM.Focus()
                gridView.DataSource = Nothing
            End With
        End If
    End Sub
    Private Sub txtTranNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranNo_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtTranNo_NUM.Text) = 0 Or txtTranNo_NUM.Text = Nothing Then
                MsgBox("TranNO should not empty", MsgBoxStyle.Information)
                txtTranNo_NUM.Focus()
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtChequeNO_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If Val(txtChequeNO_NUM.Text) = 0 Then
                MsgBox("ChequeNO should not empty", MsgBoxStyle.Information)
                txtChequeNO_NUM.Focus()
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtAmount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAmount_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtAmount_AMT.Text.ToString) = 0 Then
                MsgBox("Amount should not empty", MsgBoxStyle.Information)
                txtAmount_AMT.Focus()
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub cmbMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub
    Function setenablemode(ByVal mode As Boolean)
        txtTranNo_NUM.Enabled = mode
        dtpTrandate.Enabled = mode
        cmbBRSFLAG.Enabled = mode
        cmbMode.Enabled = mode
        txtAmount_AMT.Enabled = mode
        txtChequeNO_NUM.Enabled = mode
        dtpChequeDate.Enabled = mode
        dtprealisedate.Enabled = mode
        txtRemark.Enabled = mode
    End Function
    Private Sub txtRemark_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRemark.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtTranNo_NUM.Text) = 0 Or txtTranNo_NUM.Text = Nothing Then
                MsgBox("TranNO should not empty", MsgBoxStyle.Information)
                txtTranNo_NUM.Focus()
                Exit Sub
            End If
            If Val(txtAmount_AMT.Text.ToString) = 0 Then
                MsgBox("Amount should not empty", MsgBoxStyle.Information)
                txtAmount_AMT.Focus()
                Exit Sub
            End If
            If editSno = "1" Then
                GRIDLOAD()
                objGPack.TextClear(grpDetails)
                cmbMode.SelectedItem = "DEBIT"
                cmbBRSFLAG.SelectedItem = "YES"
                dtpTrandate.Value = cnTranFromDate.AddDays(-1)
                dtpChequeDate.Value = cnTranFromDate.AddDays(-1)
                setenablemode(False)
                btnSave.Focus()
            ElseIf editSno = "-1" Then
                GRIDLOAD()
                objGPack.TextClear(grpDetails)
                cmbMode.SelectedItem = "DEBIT"
                cmbBRSFLAG.SelectedItem = "YES"
                dtpTrandate.Value = cnTranFromDate.AddDays(-1)
                dtpChequeDate.Value = cnTranFromDate.AddDays(-1)
                CmbAccName.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub
    Private Sub cmbBRSFLAG_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBRSFLAG.SelectedIndexChanged
        If Not cmbBRSFLAG.SelectedItem = "NO" Then
            dtprealisedate.Enabled = True
        Else
            dtprealisedate.Enabled = False
        End If
    End Sub
End Class
