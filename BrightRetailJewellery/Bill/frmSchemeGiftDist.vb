Imports System.Data.OleDb
Public Class frmSchemeGiftDist
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtGv As DataTable
    Dim editBatchno As String = ""
    Dim editBilldate As Date
    Dim giftSchemeNorms As String = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='GIFTSCHEMENORMS'")
    Dim giftSchemeNormsAR As String()
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.        
    End Sub

    Function funcNew()
        objGPack.TextClear(Me)
        cmbGV.Enabled = True
        dtGv = New DataTable
        dtGv.Columns.Add("GIFTNAME")
        dtGv.Columns.Add("ACCODE")
        dtGv.Columns.Add("RUNNO")
        dtGv.Columns.Add("AMOUNT")
        'cmbGV.Items.Clear()
        'strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' ORDER BY NAME"
        'objGPack.FillCombo(strSql, cmbGV, True, False)
        strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & giftSchemeNormsAR(4).ToString & "'"
        cmbGV.Items.Clear()
        cmbGV.Items.Add(objGPack.GetSqlValue(strSql, , ""))
        cmbGV.Text = objGPack.GetSqlValue(strSql, , "")
        txtPercent.Text = giftSchemeNormsAR(1).ToString
        txtDenom_NUM.Text = giftSchemeNormsAR(2).ToString
        txtPeriod.Text = giftSchemeNormsAR(3).ToString
        txtPercent.TextAlign = HorizontalAlignment.Right
        txtDenom_NUM.TextAlign = HorizontalAlignment.Right
        txtPeriod.TextAlign = HorizontalAlignment.Right
        txtAmount.TextAlign = HorizontalAlignment.Right
        gridView.DataSource = Nothing
        cmbGV.Select()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If objGPack.Validator_Check(Me) Then Exit Function
        funcAdd()
    End Function

    Public Shared Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
        strSql += " WHERE CTLID = 'PERSONALINFOCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
        cmd = New OleDbCommand(strSql, cn, tran1)
        If cmd.ExecuteNonQuery() = 0 Then
            GoTo GETNSNO
        End If
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & GetCostId(cnCostId) & (tSno + 1).ToString & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
            GoTo GETNSNO
        End If
        Return GetCostId(cnCostId) & (tSno + 1).ToString
    End Function

    Private Sub Gift_Personalinfoinsert(ByVal estbilldate As Date, ByVal editBatchno As String, ByVal tran As OleDbTransaction, Optional ByVal AcCode As String = "")
        Dim dtAcc As DataTable
        strSql = " SELECT ACCODE,TITLE,INITIAL,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += vbCrLf + "  ,STATE,COUNTRY,PINCODE,PHONENO,MOBILE,EMAILID,FAX,PREVILEGEID,COMPANYID,COSTID,PAN"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & AcCode & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        dtAcc = New DataTable
        da.Fill(dtAcc)
        Dim psno As String = ""
        If dtAcc.Rows.Count > 0 Then
            psno = GetPersonalInfoSno(tran)
            strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  SNO,ACCODE,TRANDATE,TITLE"
            strSql += vbCrLf + "  ,INITIAL,PNAME,DOORNO,ADDRESS1"
            strSql += vbCrLf + "  ,ADDRESS2,ADDRESS3,AREA,CITY"
            strSql += vbCrLf + "  ,STATE,COUNTRY,PINCODE,PHONERES"
            strSql += vbCrLf + "  ,MOBILE,EMAIL,FAX,APPVER"
            strSql += vbCrLf + "  ,PREVILEGEID,COMPANYID,COSTID,PAN"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  VALUES"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  '" & psno & "'" ''SNO
            strSql += vbCrLf + "  ,'" & AcCode & "'" 'ACCODE
            strSql += vbCrLf + "  ,'" & GetEntryDate(editBilldate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("TITLE").ToString & "'" 'TITLE
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("INITIAL").ToString & "'" 'INITIAL
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("ACNAME").ToString & "'" 'PNAME
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("DOORNO").ToString & "'" 'DOORNO
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("ADDRESS1").ToString & "'" 'ADDRESS1
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("ADDRESS2").ToString & "'" 'ADDRESS2
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("ADDRESS3").ToString & "'" 'ADDRESS3
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("AREA").ToString & "'" 'AREA
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("CITY").ToString & "'" 'CITY
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("STATE").ToString & "'" 'STATE
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("COUNTRY").ToString & "'" 'COUNTRY
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("PINCODE").ToString & "'" 'PINCODE
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("PHONENO").ToString & "'" 'PHONERES
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("MOBILE").ToString & "'" 'MOBILE
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("EMAILID").ToString & "'" 'EMAIL
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("FAX").ToString & "'" 'Fax
            strSql += vbCrLf + "  ,'" & VERSION & "'" 'APPVER
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("PREVILEGEID").ToString & "'" 'PREVILEGEID
            strSql += vbCrLf + "  ,'" & strCompanyId & "'" 'COMPANYID
            strSql += vbCrLf + "  ,'" & cnCostId & "'" 'COSTID
            strSql += vbCrLf + "  ,'" & dtAcc.Rows(0)("PAN").ToString & "'" 'PAN
            strSql += vbCrLf + "  )"
            ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId)
        End If
        strSql = ""
        strSql = " IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & editBatchno & "')>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + "  INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += vbCrLf + "  (BATCHNO,PSNO,REMARK1,COSTID,PAN)VALUES"
        strSql += vbCrLf + "  ('" & editBatchno & "','" & psno & "','GV','" & cnCostId & "','')"
        strSql += vbCrLf + " END"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
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
    Optional ByVal Advwtper As Double = Nothing, _
    Optional ByVal tran As OleDbTransaction = Nothing _
        )
        If Amount = 0 And GrsWt = 0 And PureWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text

        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += vbcrlf + "  ("
        strSql += vbcrlf + "  SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += vbcrlf + "  ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
        strSql += vbcrlf + "  ,REFNO,REFDATE,EMPID"
        strSql += vbcrlf + "  ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += vbcrlf + "  ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += vbcrlf + "  ,RATE,ADVFIXWTPER,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
        strSql += vbcrlf + "  VALUES"
        strSql += vbcrlf + "  ("
        strSql += vbcrlf + "  '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += vbcrlf + "  ," & tNo & "" 'TRANNO
        strSql += vbCrLf + "  ,'" & editBilldate & "'" 'TRANDATE
        strSql += vbcrlf + "  ,'" & tType & "'" 'TRANTYPE
        strSql += vbcrlf + "  ,'" & RunNo & "'" 'RUNNO
        strSql += vbcrlf + "  ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += vbcrlf + "  ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += vbcrlf + "  ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += vbcrlf + "  ," & Math.Abs(PureWt) & "" 'PUREWT
        strSql += vbcrlf + "  ,'" & RecPay & "'" 'RECPAY
        strSql += vbcrlf + "  ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += vbcrlf + "  ,'" & refDate & "'" 'REFDATE
        Else
            strSql += vbcrlf + "  ,NULL" 'REFDATE
        End If
        If EmpId <> 0 Then
            strSql += vbcrlf + "  ," & EmpId & "" 'EMPID
        Else
            strSql += vbcrlf + "  ,0" 'EMPID
        End If
        strSql += vbcrlf + "  ,''" 'TRANSTATUS
        strSql += vbcrlf + "  ," & purity & "" 'PURITY
        strSql += vbcrlf + "  ,'" & CatCode & "'" 'CATCODE
        strSql += vbcrlf + "  ,'" & editBatchno & "'" 'BATCHNO
        strSql += vbcrlf + "  ," & userId & "" 'USERID

        strSql += vbcrlf + "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbcrlf + "  ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbcrlf + "  ,'" & systemId & "'" 'SYSTEMID
        strSql += vbcrlf + "  ," & Rate & "" 'RATE
        strSql += vbcrlf + "  ," & Advwtper & "" 'RATE
        strSql += vbcrlf + "  ," & Value & "" 'VALUE
        strSql += vbcrlf + "  ,''"
        strSql += vbcrlf + "  ,'Y'"
        strSql += vbcrlf + "  ,'" & Remark1 & "'" 'REMARK1
        strSql += vbcrlf + "  ,'" & Remark2 & "'" 'REMARK1
        strSql += vbcrlf + "  ,'" & Accode & "'" 'ACCODE
        strSql += vbcrlf + "  ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += vbcrlf + "  ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += vbcrlf + "  ,NULL" 'DUEDATE
        End If
        strSql += vbcrlf + "  ,'" & VERSION & "'" 'APPVER
        strSql += vbcrlf + "  ,'" & strCompanyId & "'" 'COMPANYID
        strSql += vbcrlf + "  ,'" & cnCostId & "'" 'COSTID
        strSql += vbcrlf + "  ,'P'" 'FROMFLAG
        strSql += vbcrlf + "  ,'" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
        strSql += vbCrLf + "  ,'" & Paymode & "'"
        strSql += vbCrLf + "  )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub

    Function funcAdd()
        Dim CardCode As Integer = Nothing
        strSql = " SELECT CARDCODE FROM "
        strSql += " " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "'"
        CardCode = Val(objGPack.GetSqlValue(strSql).ToString)
        If CardCode = 0 Then
            MsgBox("Gift Voucher not found...", MsgBoxStyle.Information)
            cmbGV.Focus()
            Exit Function
        End If
        Dim dt As New DataTable
        dt.Clear()
        Dim duedate As Date
        Dim tran As OleDbTransaction = Nothing
        Try
            Dim Billdate As Date = GetActualEntryDate()
            tran = cn.BeginTransaction()
            editBilldate = Now.Date
            editBatchno = GetNewBatchno(cnCostId, editBilldate, tran)
            Dim tNo As String = GetBillNoValue("GEN-PAYMENTBILLNO", tran)
            tNo = Val(tNo) + 1
            Billdate = editBilldate
            For k As Integer = 0 To Val(txtDenom_NUM.Text.ToString) - 1
                Dim runno As String = GetGvNo(tran, True) 'gridView.Rows(k).Cells("RUNNO").Value.ToString
                Dim tranno As Integer = tNo
                Dim amt As Decimal = Val(txtAmount.Text.ToString)
                strSql = "SELECT DUEDAYS FROM " & cnStockDb & "..GVTRAN WHERE RUNNO='" & runno & "'"
                Dim Due As Integer = Val(objGPack.GetSqlValue(strSql, "DUEDAYS", 0, tran).ToString)
                duedate = DateAdd(DateInterval.Day, Due, editBilldate)
                Billdate = Billdate.AddMonths(Val(txtPeriod.Text.ToString))
                Dim Accode As String = giftSchemeNormsAR(4).ToString
                InsertIntoOustanding(tranno, "GV", runno, amt, "R", "GV", , , , 0, amt, CardCode, , , , Billdate, txtGroupCode_MAN.Text.ToString & txtRegNo_MAN.Text.ToString, "GV", Accode, "S", , , , tran)
                Gift_Personalinfoinsert(editBilldate, editBatchno, tran, Accode)
            Next
            tran.Commit()
            tran.Dispose()
            MsgBox("Saved...", MsgBoxStyle.Information)
            Try
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint.mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":GV")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & editBatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & editBilldate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":GV" & ";" & _
                        LSet("BATCHNO", 15) & ":" & editBatchno & ";" & _
                        LSet("TRANDATE", 15) & ":" & editBilldate.ToString("yyyy-MM-dd") & ";" & _
                        LSet("DUPLICATE", 15) & ":N")
                    End If

                Else

                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If

            Catch ex As Exception

            End Try
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran.Dispose()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        giftSchemeNormsAR = giftSchemeNorms.Split(",")
        If giftSchemeNormsAR.Length >= 5 Then
            'strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & giftSchemeNormsAR(5).ToString & "'"
            'cmbGV.Items.Clear()
            'cmbGV.Items.Add(objGPack.GetSqlValue(strSql, , ""))
            'cmbGV.Text = objGPack.GetSqlValue(strSql, , "")
        Else
            MsgBox("Please set proper SoftControl...")
        End If
        funcGridStyle(gridView)
        btnNew_Click(Me, New EventArgs)
        funcNew()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If funcValidation() = True Then
            funcSave()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub txtDesignerName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then

        End If
    End Sub

    Private Sub txtRegNo_MAN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRegNo_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcValidation()
        End If
    End Sub

    Private Function funcValidation() As Boolean
        gridView.DataSource = Nothing
        If txtGroupCode_MAN.Text = "" Then
            MsgBox("Group Code is empty...", MsgBoxStyle.Information)
            txtGroupCode_MAN.Focus()

            Return False
            Exit Function
        End If
        If txtRegNo_MAN.Text = "" Then
            MsgBox("Reg No is empty...", MsgBoxStyle.Information)
            txtRegNo_MAN.Focus()
            Return False
            Exit Function
        End If
        strSql = " SELECT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE='" & txtGroupCode_MAN.Text.ToString & "' AND REGNO='" & txtRegNo_MAN.Text.ToString & "'"
        Dim strSchemeId As String = objGPack.GetSqlValue(strSql, , "").ToString
        If strSchemeId = "" Then
            MsgBox("Invalid Scheme...", MsgBoxStyle.Information)
            txtGroupCode_MAN.Focus()
            Return False
            Exit Function
        End If
        If strSchemeId <> giftSchemeNormsAR(0).ToString Then
            MsgBox("Invalid Scheme...", MsgBoxStyle.Information)
            txtGroupCode_MAN.Focus()
            Return False
            Exit Function
        End If
        strSql = " SELECT SCHEMENAME FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID='" & strSchemeId & "' "
        Dim strSchemeName As String = objGPack.GetSqlValue(strSql, , "").ToString
        If strSchemeName = "" Then
            MsgBox("Invalid Scheme...", MsgBoxStyle.Information)
            txtSchemeName.Text = ""
            txtGroupCode_MAN.Focus()
            Return False
            Exit Function
        Else
            txtSchemeName.Text = strSchemeName
        End If
        strSql = " SELECT TOP 1 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE REMARK1='" & txtGroupCode_MAN.Text.ToString & txtRegNo_MAN.Text.ToString & "' AND ISNULL(CANCEL,'')='' AND FLAG='S' "
        If Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then
            MsgBox("Gift Voucher already issued...", MsgBoxStyle.Information)
            strSql = vbCrLf + " SELECT TRANDATE,DUEDATE,TRANTYPE,RUNNO,AMOUNT,RECPAY,BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE REMARK1='" & txtGroupCode_MAN.Text.ToString & txtRegNo_MAN.Text.ToString & "' AND ISNULL(CANCEL,'')='' AND FLAG='S' "
            strSql += vbCrLf + " ORDER BY RUNNO"
            dtGv = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGv)
            gridView.DataSource = Nothing
            gridView.DataSource = dtGv
            If gridView.Rows.Count > 0 Then
                gridView.Columns("BATCHNO").Visible = False
                gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If
            txtGroupCode_MAN.Focus()
            Return False
            Exit Function
        End If
        strSql = " SELECT SUM(AMOUNT)AMOUNT FROM " & cnChitCompanyid & "SH0708..SCHEMETRAN WHERE GROUPCODE='" & txtGroupCode_MAN.Text.ToString & "' AND REGNO='" & txtRegNo_MAN.Text.ToString & "' AND INSTALLMENT='1' "
        txtSchAmt.Text = Val(objGPack.GetSqlValue(strSql, , "").ToString)
        strSql = " SELECT MAX(INSTALLMENT)INSTALLMENT FROM " & cnChitCompanyid & "SH0708..SCHEMETRAN WHERE GROUPCODE='" & txtGroupCode_MAN.Text.ToString & "' AND REGNO='" & txtRegNo_MAN.Text.ToString & "' "
        txtInstallment.Text = Val(objGPack.GetSqlValue(strSql, , "").ToString)
        Return True
    End Function

    Private Sub txtSchAmt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSchAmt.TextChanged
        txtAmount.Text = Format(Val(txtSchAmt.Text.ToString) * Val(txtPercent.Text.ToString) / 100, "0.00")
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "D" Then
            Try
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint.mem"
                    Dim tranDate As Date = gridView.CurrentRow.Cells("trandate").Value.ToString
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":GV")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & gridView.CurrentRow.Cells("BATCHNO").Value.ToString)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & tranDate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":GV" & ";" & _
                        LSet("BATCHNO", 15) & ":" & editBatchno & ";" & _
                        LSet("TRANDATE", 15) & ":" & tranDate.ToString("yyyy-MM-dd") & ";" & _
                        LSet("DUPLICATE", 15) & ":N")
                    End If

                Else

                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub
End Class