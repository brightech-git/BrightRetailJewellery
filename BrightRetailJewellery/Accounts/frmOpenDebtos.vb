Imports System.Data.OleDb
Public Class frmOpenDebtos
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim cmd As OleDbCommand
    Dim costId As String = ""
    Dim ACCODE As String = ""
    Dim objWeighDet As New frmOpenDebitorsWeight
    Dim objToBe As New frmToBe
    Dim dsPendingItems As New DataSet
    Dim editSno As String = "-1"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtGridView.Columns
            .Add("TRANNO", GetType(Integer))
            .Add("TRANDATE", GetType(Date))
            .Add("TYPE", GetType(String))
            .Add("MODE", GetType(String))
            .Add("REFNO", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("WEIGHT", GetType(Decimal))
            .Add("REMARK", GetType(String))
            .Add("EMPID", GetType(Integer))

            .Add("CATEGORY", GetType(String))
            .Add("PURITY", GetType(Double))
            .Add("RATE", GetType(Double))

            .Add("PNAME", GetType(String))
            .Add("ADDRESS1", GetType(String))
            .Add("ADDRESS2", GetType(String))
            Dim col As New DataColumn("KEYNO", GetType(Integer))
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Add(col)
        End With
        gridView.DataSource = dtGridView
        StyleGridView()
    End Sub

    Private Sub frmOpenDebtos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If e.KeyCode = Keys.Escape And tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        End If
    End Sub
    Private Sub frmOpenDebtos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAccCode.Focused Then Exit Sub
            If txtEmpId_NUM.Focused Then Exit Sub
            If txtWeight_WET.Focused Then Exit Sub
            If txtRemark.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOpenDebtos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        gridOpenView.RowTemplate.Height = 21

        gridView.RowTemplate.Height = 21
        gridView.ColumnHeadersVisible = False

        cmbType.Items.Clear()
        cmbType.Items.Add("RECEIPT")
        cmbType.Items.Add("PAYMENT")
        cmbType.Text = "PAYMENT"
        cmbMode.Items.Clear()
        cmbMode.Items.Add("DUE")
        cmbMode.Items.Add("DUE REC")
        cmbMode.Items.Add("ADVANCE")
        cmbMode.Text = "DUE"

        cmbOpenType.Items.Clear()
        cmbOpenType.Items.Add("BOTH")
        cmbOpenType.Items.Add("PAYMENT")
        cmbOpenType.Items.Add("RECEIPT")

        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        Else
            cmbCostCentre_MAN.Enabled = False
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpOpenFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpOpenFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpOpenTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpOpenTo.MinimumDate = (New DateTimePicker).MinDate

        dtpTrandate.MinimumDate = (New DateTimePicker).MinDate
        dtpTrandate.MaximumDate = (New DateTimePicker).MaxDate

        dtpOpenFrom.Value = GetEntryDate(GetServerDate)
        dtpOpenTo.Value = GetEntryDate(GetServerDate)
        dtpTrandate.Value = GetEntryDate(GetServerDate)

        dtGridView.Rows.Clear()
        dtGridView.AcceptChanges()
        CalcTotal()
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

    Private Sub DeleteOutstDetail(ByVal sno As String)
        ''update
        strSql = " DELETE FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += " WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE SNO = '" & sno & "'))"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

        strSql = " DELETE FROM " & cnAdminDb & "..ITEMDETAIL"
        strSql += " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE SNO = '" & sno & "')"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

        strSql = " DELETE FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += " WHERE SNO = '" & sno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not gridView.RowCount > 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If cmbCostCentre_MAN.Enabled Then
            If Not costId.Length > 0 Then
                MsgBox("Invalid Costcentre", MsgBoxStyle.Information)
                cmbCostCentre_MAN.Focus()
                Exit Sub
            End If
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            Dim Batchno As String = Nothing
            If Not editSno = "-1" Then
                DeleteOutstDetail(editSno)
            End If

            For Each ro As DataRow In dtGridView.Rows
                Batchno = GetNewBatchno(costId, dtpTrandate.Value.ToString("yyyy-MM-dd"), tran)
                Dim runNo As String = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & Mid(ro!MODE.ToString, 1, 1) & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & ro!REFNO.ToString
                Dim recPay As String = Nothing
                Dim paymode As String = Nothing
                If ro!TYPE.ToString = "RECEIPT" And ro!MODE.ToString = "ADVANCE" Then
                    recPay = "R"
                    paymode = "AR"
                ElseIf ro!TYPE.ToString = "PAYMENT" And ro!MODE.ToString = "DUE" Then
                    recPay = "P"
                    paymode = "DU"
                ElseIf ro!TYPE.ToString = "RECEIPT" And ro!MODE.ToString = "DUE REC" Then
                    recPay = "R"
                    paymode = "DU"
                End If
                InsertIntoOustanding(Batchno, ro!TRANNO, Mid(ro!MODE.ToString, 1, 1), runNo, Val(ro!AMOUNT.ToString), recPay _
                , ro!TRANDATE, paymode, Val(ro!WEIGHT.ToString), Val(ro!WEIGHT.ToString) _
                , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro!CATEGORY.ToString & "'", , , tran) _
                , Val(ro!RATE.ToString), , , , Val(ro!PURITY.ToString), , , ro!REMARK.ToString, , ACCODE, Val(ro!EMPID.ToString))

                InsertIntoPersonalinfo(tran, Batchno, ACCODE, ro!TRANDATE, ro!PNAME.ToString, ro!ADDRESS1.ToString, ro!ADDRESS2.ToString)
                If dsPendingItems.Tables.Contains(ro!KEYNO.ToString) Then
                    For Each roPend As DataRow In dsPendingItems.Tables(ro!KEYNO.ToString).Rows
                        InsertIntoItemDetail(tran, roPend!ISSSNO.ToString, Val(ro!TRANNO), "TB", ro!TRANDATE, Val(roPend!pcs.ToString) _
                       , Val(roPend!grswt.ToString), Val(roPend!netwt.ToString), Val(roPend!value.ToString) _
                       , roPend!item.ToString, roPend!remark.ToString, "R", runNo, Batchno)
                    Next
                End If
            Next
            InsertIntoTrailBalance()
            tran.Commit()
            If editSno = "-1" Then
                MsgBox("Saved")
            Else
                MsgBox("Updated")
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Dispose()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub InsertIntoTrailBalance()
        strSql = " SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
        strSql += " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += " WHERE ACCODE = '" & ACCODE & "'"
        strSql += " AND COSTID = '" & costId & "'"
        strSql += " AND FROMFLAG = 'O'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql, , "0", tran))
        SaveIntoOpenTrailBAlance(ACCODE, IIf(amt > 0, amt, 0), IIf(amt > 0, 0, amt))
    End Sub

    Private Sub InsertIntoItemDetail(ByVal tran As OleDbTransaction, ByVal issSno As String, ByVal tranNo As Integer, ByVal tranType As String, ByVal billDate As Date _
, ByVal pcs As Integer, ByVal grswt As Double, ByVal netWt As Double, ByVal value As Double _
, ByVal remark1 As String, ByVal remark2 As String, ByVal recpay As Char, ByVal RUNNO As String, ByVal batchno As String)
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMDETAIL"
        strSql += " ("
        strSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE,"
        strSql += " PCS,GRSWT,NETWT,AMOUNT,REMARK1,"
        strSql += " REMARK2,BATCHNO,CANCEL,FLAG,APPVER,"
        strSql += " COMPANYID,UPDATED,UPTIME,RECPAY"
        strSql += " ,RUNNO"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ITEMDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
        strSql += " ,'" & issSno & "'" 'ISSSNO
        strSql += " ," & tranNo & "" 'TRANNO
        strSql += " ,'" & billDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranType & "'" 'TRANTYPE
        strSql += " ," & pcs & "" 'PCS
        strSql += " ," & grswt & "" 'GRSWT
        strSql += " ," & netWt & "" 'NETWT
        strSql += " ," & value & "" 'AMOUNT
        strSql += " ,'" & remark1 & "'" 'REMARK1
        strSql += " ,'" & remark2 & "'" 'REMARK2
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ,''" 'CANCEL
        strSql += " ,''" 'FLAG
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & recpay & "'" 'RECPAY
        strSql += " ,'" & RUNNO & "'" 'RUNNO
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Sub


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
        If totamt > 0 Then
            lblTotal.Text = "TOTAL : " & Format(totamt, "0.00") & "Dr"
        ElseIf totamt < 0 Then
            lblTotal.Text = "TOTAL : " & Format(-1 * totamt, "0.00") & "Cr"
        Else
            lblTotal.Text = ""
        End If
    End Sub

    Private Sub SaveIntoOpenTrailBAlance(ByVal accode As String, ByVal debit As Double, ByVal credit As Double)
        strSql = "SELECT ACCODE FROM " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += " WHERE ACCODE = '" & accode & "'"
        strSql += " AND COSTID = '" & costId & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
            ''UPDATE
            strSql = " UPDATE " & cnStockDb & "..OPENTRAILBALANCE SET"
            strSql += " ACCODE = '" & accode & "'"
            strSql += " ,DEBIT = " & Math.Abs(debit) & ""
            strSql += " ,CREDIT = " & Math.Abs(credit) & ""
            strSql += " ,COSTID = '" & costId & "'"
            strSql += " ,USERID  = " & userId & ""
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " ,SETTLEMENT = ''"
            strSql += " ,DEBITWT = 0"
            strSql += " ,CREDITWT = 0"
            strSql += " ,APPVER = '" & VERSION & "'"
            strSql += " ,COMPANYID = '" & strCompanyId & "'"
            strSql += " ,APPROVAL = ''"
            strSql += " ,SETSNO = ''"
            strSql += " ,SETREFNO = ''"
            strSql += " WHERE ACCODE = '" & accode & "'"
            strSql += " AND COSTID = '" & costId & "'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
        Else
            ''INSERT
            strSql = " INSERT INTO " & cnStockDb & "..OPENTRAILBALANCE("
            strSql += " ACCODE,DEBIT,CREDIT,COSTID"
            strSql += " ,USERID,UPDATED,UPTIME,SETTLEMENT"
            strSql += " ,DEBITWT,CREDITWT,APPVER"
            strSql += " ,COMPANYID,APPROVAL,SETSNO,SETREFNO"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & accode & "'" 'ACCODE
            strSql += " ," & Math.Abs(debit) & "" 'DEBIT
            strSql += " ," & Math.Abs(credit) & "" 'CREDIT
            strSql += " ,'" & costId & "'" 'COSTID
            strSql += " ," & userId & "" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,''" 'SETTLEMENT
            strSql += " ,0" 'DEBITWT
            strSql += " ,0" 'CREDITWT
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,''" 'APPROVAL
            strSql += " ,''" 'SETSNO
            strSql += " ,''" 'SETREFNO
            strSql += ")"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
        End If
    End Sub

    Private Sub InsertIntoPersonalinfo(ByVal tran As OleDbTransaction, ByVal batchno As String, ByVal accode As String, ByVal billdate As Date _
    , ByVal Pname As String, ByVal address1 As String, ByVal address2 As String)
        Dim psno As String = frmAddressDia.GetPersonalInfoSno(tran)
        strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
        strSql += " ("
        strSql += " SNO,ACCODE,TRANDATE,TITLE"
        strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
        strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
        strSql += " ,MOBILE,EMAIL,FAX,APPVER"
        strSql += " ,PREVILEGEID,COMPANYID,COSTID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & psno & "'" ''SNO
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & billdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,''" 'TITLE
        strSql += " ,''" 'INITIAL
        strSql += " ,'" & Pname & "'" 'PNAME
        strSql += " ,''" 'DOORNO
        strSql += " ,'" & address1 & "'" 'ADDRESS1
        strSql += " ,'" & address2 & "'" 'ADDRESS2
        strSql += " ,''" 'ADDRESS3
        strSql += " ,''" 'AREA
        strSql += " ,''" 'CITY
        strSql += " ,''" 'STATE
        strSql += " ,''" 'COUNTRY
        strSql += " ,''" 'PINCODE
        strSql += " ,''" 'PHONERES
        strSql += " ,''" 'MOBILE
        strSql += " ,''" 'EMAIL
        strSql += " ,''" 'Fax
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,''" 'PREVILEGEID
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & costId & "'" 'COSTID
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran, costId)

        strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += " (BATCHNO,PSNO,COSTID)VALUES"
        strSql += " ('" & batchno & "','" & psno & "','" & costId & "')"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Sub

    Private Sub InsertIntoOustanding _
( _
ByVal BATCHNO As String, ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
ByVal RecPay As String, _
ByVal recDate As Date, _
ByVal paymode As String, _
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
Optional ByVal Empid As Integer = Nothing _
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
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & recDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
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

        strSql += " ," & Empid & "" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & BATCHNO & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,''" 'CASHID
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
        strSql += " ,'" & costId & "'" 'COSTID
        strSql += " ,'O'" 'FROMFLAG
        strSql += " ,'" & paymode & "'" 'PAYMODE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Sub


    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub txtAccCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccCode.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtAccCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAccCode.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadAcc()
        End If
    End Sub

    Private Sub txtAccCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAccCode.Text <> "" And objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccCode.Text & "'").Length > 0 Then
                LoadAccDetails()
            Else
                LoadOutstDetails()
            End If
        End If
    End Sub

    Private Sub txtAccCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccCode.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub LoadAcc()
        strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
        strSql += "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE 1=1"
        strSql += GetAcNameQryFilteration()
        'strSql += "  WHERE ACTYPE <> 'O'"
        txtAccCode.Text = BrighttechPack.SearchDialog.Show("Search Account Code", strSql, cn, 1)
        LoadAccDetails()
    End Sub

    Private Sub LoadAccDetails()
        If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccCode.Text & "'" & GetAcNameQryFilteration()).Length > 0 Then
            Dim dtAddDet As New DataTable
            strSql = " SELECT ACNAME,DOORNO + ' '+ ISNULL(ADDRESS1,'') + ' '+ ISNULL(ADDRESS2,'') AS ADDRESS1,"
            strSql += " ISNULL(ADDRESS3,'') + ' ' + ISNULL(AREA,'') + ' ' + ISNULL(CITY,'') AS ADDRESS2 "
            strSql += " FROM " & cnAdminDb & "..ACHEAD"
            strSql += " WHERE ACCODE = '" & txtAccCode.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAddDet)
            For Each ro As DataRow In dtAddDet.Rows
                txtAccName_MAN.Text = ro!ACNAME.ToString
                txtAddress1.Text = Trim(ro!ADDRESS1.ToString)
                txtAddress2.Text = Trim(ro!ADDRESS2.ToString)
            Next
            txtTranNo_NUM_MAN.Focus()
        Else
            txtAccCode.Clear()
            txtAccName_MAN.Clear()
            txtAddress1.Clear()
            txtAddress2.Clear()
            txtAccName_MAN.Focus()
        End If
        LoadOutstDetails()
    End Sub

    Private Sub LoadOutstDetails()
        dtGridView.Rows.Clear()
        ACCODE = Nothing
        ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccCode.Text & "'", , "DRS")
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
        txtAccName_MAN.Focus()
    End Sub

    Private Sub cmbType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.LostFocus
        cmbMode.Items.Clear()
        If cmbType.Text = "RECEIPT" Then
            cmbMode.Items.Add("ADVANCE")
            cmbMode.Items.Add("DUE REC")
            cmbMode.Text = "ADVANCE"
        Else
            cmbMode.Items.Add("DUE")
            cmbMode.Text = "DUE"
        End If
    End Sub

    Private Sub txtRefNo_NUM_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefNo_NUM_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRefNo_NUM_MAN.Text = "" Then Exit Sub
            checkRefno()
        End If
    End Sub
    Private Function checkRefno() As Boolean
        Dim runNo As String = GetCostId(costId) & GetCompanyId(strCompanyId)
        Select Case cmbMode.Text
            Case "DUE", "DUE REC"
                runNo += "D"
            Case Else
                runNo += "A"
        End Select
        runNo += Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + txtRefNo_NUM_MAN.Text
        strSql = "SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += " WHERE RUNNO = '" & runNo & "'"
        strSql += " AND FROMFLAG <> 'O'"
        If txtRefNo_NUM_MAN.Text = "" Then
            MsgBox("RefNo should not empty", MsgBoxStyle.Information)
            txtRefNo_NUM_MAN.Focus()
            Return True
        ElseIf objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("RefNo already exist", MsgBoxStyle.Information)
            txtRefNo_NUM_MAN.Focus()
            Return True
        End If
    End Function

    Private Sub txtEmpId_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpId_NUM.GotFocus
        Main.ShowHelpText("Press Insert to Help")
    End Sub

    Private Sub LoadEmpId(ByVal txtEmpBox As TextBox)
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim empId As Integer = Val(BrighttechPack.SearchDialog.Show("Select EmpName", strSql, cn, 1))
        If empId > 0 Then
            txtEmpBox.Text = empId
            txtEmpBox.SelectAll()
        End If
    End Sub

    Private Sub txtEmpId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmpId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Insert) Then
            LoadEmpId(txtEmpId_NUM)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If cmbCostCentre_MAN.Enabled Then
                If cmbCostCentre_MAN.Text = "" Then
                    MsgBox("Costcentre should not empty", MsgBoxStyle.Information)
                    cmbCostCentre_MAN.Focus()
                    Exit Sub
                End If
                If Not cmbCostCentre_MAN.Items.Contains(cmbCostCentre_MAN.Text) Then
                    MsgBox("Invalid Costcentre", MsgBoxStyle.Information)
                    cmbCostCentre_MAN.Focus()
                    Exit Sub
                End If
            Else
                cmbCostCentre_MAN.Text = ""
            End If
            If txtAccName_MAN.Text = "" Then
                MsgBox("AccName should not empty", MsgBoxStyle.Information)
                txtAccName_MAN.Focus()
                Exit Sub
            End If
            If Val(txtAmount_AMT.Text) = 0 And Val(txtWeight_WET.Text) = 0 Then
                MsgBox("Amount and Weight should not empty", MsgBoxStyle.Information)
                txtAmount_AMT.Focus()
                Exit Sub
            End If
            If Val(txtWeight_WET.Text) <> 0 And Not objGPack.GetSqlValue("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & objWeighDet.txtCategory.Text & "'").Length > 0 Then
                ShowWeightDetail()
                Exit Sub
            End If
            If txtEmpId_NUM.Text = "" Then
                LoadEmpId(txtEmpId_NUM)
                Exit Sub
            ElseIf Not Val(objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_NUM.Text) & "")) > 0 Then
                LoadEmpId(txtEmpId_NUM)
                Exit Sub
            End If
            Dim ro As DataRow = dtGridView.NewRow
            ro("TRANNO") = txtTranNo_NUM_MAN.Text
            ro("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
            ro("TYPE") = cmbType.Text
            ro("MODE") = cmbMode.Text
            ro("REFNO") = txtRefNo_NUM_MAN.Text
            ro("AMOUNT") = IIf(Val(txtAmount_AMT.Text) <> 0, txtAmount_AMT.Text, DBNull.Value)
            ro("WEIGHT") = IIf(Val(txtWeight_WET.Text) <> 0, txtWeight_WET.Text, DBNull.Value)
            ro("REMARK") = txtRemark.Text
            ro("EMPID") = txtEmpId_NUM.Text

            ro("CATEGORY") = objWeighDet.txtCategory.Text
            ro("PURITY") = Val(objWeighDet.txtPurity_PER.Text)
            ro("RATE") = Val(objWeighDet.txtRate_AMT.Text)

            ro("PNAME") = txtAccName_MAN.Text
            ro("ADDRESS1") = txtAddress1.Text
            ro("ADDRESS2") = txtAddress2.Text
            dtGridView.Rows.Add(ro)
            dtGridView.AcceptChanges()
            If objToBe.dtGridToBe.Rows.Count > 0 Then
                Dim dt As New DataTable
                dt = objToBe.dtGridToBe.Copy
                dt.TableName = gridView.Rows(gridView.RowCount - 1).Cells("KEYNO").Value.ToString
                dsPendingItems.Tables.Add(dt)
            End If

            ''CLEAR
            objGPack.TextClear(grpDetails)
            objToBe = New frmToBe
            CalcTotal()
            txtTranNo_NUM_MAN.Focus()
            If Not editSno = "-1" Then btnSave_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub StyleGridView()
        With gridView
            .Columns("TRANNO").Width = txtTranNo_NUM_MAN.Width + 1
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = dtpTrandate.Width + 1
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TYPE").Width = cmbType.Width + 1
            .Columns("MODE").Width = cmbMode.Width + 1
            .Columns("REFNO").Width = txtRefNo_NUM_MAN.Width + 1
            .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = txtAmount_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("WEIGHT").Width = txtWeight_WET.Width + 1
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").DefaultCellStyle.Format = "0.000"
            .Columns("REMARK").Width = txtRemark.Width + 1
            .Columns("EMPID").Width = txtEmpId_NUM.Width
            .Columns("EMPID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 9 To gridView.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Sub

    Private Sub txtEmpId_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpId_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub skip_focus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtAccName_MAN.GotFocus, txtAddress1.GotFocus, txtAddress2.GotFocus
        If txtAccCode.Text <> "" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        CalcTotal()
        If Not gridView.RowCount > 0 Then txtTranNo_NUM_MAN.Focus()
    End Sub

    Private Sub gridFocusAtDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    txtTranNo_NUM_MAN.KeyDown, txtRefNo_NUM_MAN.KeyDown, txtAmount_AMT.KeyDown, txtWeight_WET.KeyDown, txtRemark.KeyDown, txtEmpId_NUM.KeyDown
        If e.KeyCode = Keys.Down And gridView.RowCount > 0 Then
            gridView.Focus()
        ElseIf e.KeyCode = Keys.Escape And CType(sender, TextBox).Name = txtTranNo_NUM_MAN.Name And gridView.RowCount > 0 Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub ShowToBeDetail()
        objToBe.StartPosition = FormStartPosition.CenterScreen
        objToBe.ShowDialog()
        Me.SelectNextControl(txtRemark, True, True, True, True)
    End Sub

    Private Sub ShowWeightDetail()
        If Val(txtWeight_WET.Text) > 0 Then
            Dim pt As New Point
            pt = New Point(570, 285)
            objWeighDet.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            objWeighDet.StartPosition = FormStartPosition.Manual
            objWeighDet.Location = pt
            objWeighDet.ShowDialog()
        End If
        Me.SelectNextControl(txtWeight_WET, True, True, True, True)
    End Sub

    Private Sub txtWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWeight_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowWeightDetail()
        End If
    End Sub

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus
        costId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
        If txtAccCode.Text <> "" Then LoadOutstDetails()
        txtAccCode.Focus()
    End Sub

    Private Sub txtRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbType.Text = "PAYMENT" Then
                ShowToBeDetail()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        chkOpenDate.Checked = False
        gridOpenView.DataSource = Nothing
        dtpOpenFrom.Value = GetEntryDate(GetServerDate)
        dtpOpenTo.Value = GetEntryDate(GetServerDate)
        lblType.Text = ""
        lblEmpname.Text = ""
        lblAddress1.Text = ""
        lblAddress2.Text = ""
        lblPurity.Text = ""
        lblRate.Text = ""
        cmbOpenType.Text = "BOTH"
        tabMain.SelectedTab = tabView
        chkOpenDate.Focus()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub chkOpenDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOpenDate.CheckedChanged
        dtpOpenFrom.Enabled = chkOpenDate.Checked
        dtpOpenTo.Enabled = chkOpenDate.Checked
    End Sub

    Private Sub btnOpenSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOSearch_View.Click
        gridOpenView.DataSource = Nothing
        strSql = "SELECT * FROM "
        strSql += "("
        strSql += " SELECT "
        strSql += " CASE WHEN RECPAY = 'R' AND SUBSTRING(RUNNO,6,1) = 'D' THEN 'DUE REC'"
        strSql += "       WHEN RECPAY = 'R' AND SUBSTRING(RUNNO,6,1) = 'A' THEN 'ADVANCE'"
        strSql += "       ELSE 'DUE' END MODE"
        strSql += " ,TRANNO,O.TRANDATE"
        strSql += " ,PNAME"
        strSql += " ,AMOUNT,GRSWT,RUNNO"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = O.CATCODE)AS CATNAME"
        strSql += " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = O.EMPID)AS EMPNAME"
        strSql += " ,PURITY,O.BATCHNO,O.SYSTEMID"
        strSql += " ,RATE,VALUE,O.CANCEL,REMARK1"
        strSql += " ,ADDRESS1,ADDRESS2"
        strSql += " ,CASE WHEN RECPAY = 'R' THEN 'RECEIPT' ELSE 'PAYMENT' END AS RECPAY"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        strSql += " ,O.COSTID"
        strSql += " ,O.ACCODE,O.EMPID,O.SNO"
        strSql += " FROM " & cnAdminDb & "..OUTSTANDING AS O," & cnAdminDb & "..PERSONALINFO AS P"
        strSql += " WHERE FROMFLAG = 'O' AND P.SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)"
        strSql += " AND O.COMPANYID = '" & strCompanyId & "'"
        If cmbOpenType.Text = "RECEIPT" Then
            strSql += " AND RECPAY = 'R'"
        ElseIf cmbOpenType.Text = "PAYMENT" Then
            strSql += " AND RECPAY = 'P'"
        End If
        strSql += ")X"
        If txtOpenAccName.Text <> "" Then
            strSql += " WHERE PNAME LIKE '" & txtOpenAccName.Text & "%'"
        End If
        strSql += " ORDER BY TRANDATE,PNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtView As New DataTable
        da.Fill(dtView)
        If Not dtView.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridOpenView.DataSource = dtView
        With gridOpenView
            .Columns("MODE").Width = 80
            With .Columns("TRANNO")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TRANDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("PNAME")
                .HeaderText = "PARTY NAME"
                .Width = 250
            End With
            With .Columns("GRSWT")
                .HeaderText = "WEIGHT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("RUNNO").Width = 80
            .Columns("CATNAME").Width = 245
            For CNT As Integer = 8 To gridOpenView.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
        End With
        gridOpenView.Focus()
    End Sub

    Private Sub gridOpenView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.GotFocus
        lblEdit.Visible = True
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
                    DeleteOutstDetail(.Cells("SNO").Value)
                    InsertIntoTrailBalance()
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Deleted Successfully")
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End With
        End If
    End Sub

    Private Sub gridOpenView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridOpenView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridOpenView.RowCount > 0 Then
                Exit Sub
            End If
            gridOpenView.CurrentCell = gridOpenView.Rows(gridOpenView.CurrentCell.RowIndex).Cells(0)
            ''getDetails
            With gridOpenView.Rows(gridOpenView.CurrentCell.RowIndex)
                cmbCostCentre_MAN.Text = .Cells("COSTNAME").Value.ToString
                costId = .Cells("COSTID").Value.ToString
                txtAccCode.Text = .Cells("ACCODE").Value.ToString
                txtAccName_MAN.Text = .Cells("PNAME").Value.ToString
                txtAddress1.Text = .Cells("ADDRESS1").Value.ToString
                txtAddress2.Text = .Cells("ADDRESS2").Value.ToString

                txtTranNo_NUM_MAN.Text = .Cells("TRANNO").Value
                dtpTrandate.Value = .Cells("TRANDATE").Value
                cmbType.Text = .Cells("RECPAY").Value
                cmbMode.Text = .Cells("MODE").Value
                txtRefNo_NUM_MAN.Text = Mid(.Cells("RUNNO").Value.ToString, 4, .Cells("RUNNO").Value.ToString.Length - 3)
                txtAmount_AMT.Text = .Cells("AMOUNT").Value
                txtWeight_WET.Text = .Cells("GRSWT").Value
                txtRemark.Text = .Cells("REMARK1").Value
                txtEmpId_NUM.Text = .Cells("EMPID").Value
                editSno = .Cells("SNO").Value
                tabMain.SelectedTab = tabGeneral
                txtTranNo_NUM_MAN.Focus()
            End With
        End If
    End Sub

    Private Sub gridOpenView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.LostFocus
        lblEdit.Visible = False
    End Sub

    Private Sub gridOpenView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridOpenView.RowEnter
        With gridOpenView.Rows(e.RowIndex)
            lblType.Text = .Cells("RECPAY").Value.ToString
            lblEmpname.Text = .Cells("EMPNAME").Value.ToString

            lblAddress1.Text = .Cells("ADDRESS1").Value.ToString
            lblAddress2.Text = .Cells("ADDRESS2").Value.ToString

            lblPurity.Text = IIf(Val(.Cells("PURITY").Value.ToString) <> 0, .Cells("PURITY").Value.ToString, Nothing)
            lblRate.Text = IIf(Val(.Cells("RATE").Value.ToString) <> 0, .Cells("RATE").Value.ToString, Nothing)
            lblCostcentre.Text = .Cells("COSTNAME").Value.ToString
        End With
    End Sub

    Private Sub txtRefNo_NUM_MAN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRefNo_NUM_MAN.TextChanged

    End Sub
End Class
