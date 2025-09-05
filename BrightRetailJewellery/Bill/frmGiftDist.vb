Imports System.Data.OleDb
Public Class frmGiftDist
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtGv As DataTable
    Dim editBatchno As String = ""
    Dim editBilldate As Date

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.        
    End Sub

    Function funcNew()
        dtGv = New DataTable
        dtGv.Columns.Add("GIFTNAME")
        dtGv.Columns.Add("ACCODE")
        dtGv.Columns.Add("RUNNO")
        dtGv.Columns.Add("AMOUNT")
        cmbGV.Items.Clear()
        strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbGV, True, False)
        objGPack.TextClear(Me)
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
        strSql += vbcrlf + "  ,'" & editBilldate & "'" 'TRANDATE
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
            For k As Integer = 0 To gridView.Rows.Count - 1
                tran = cn.BeginTransaction()
                editBilldate = Now.Date
                editBatchno = GetNewBatchno(cnCostId, editBilldate, tran)
                Dim runno As String = gridView.Rows(k).Cells("RUNNO").Value.ToString
                Dim tranno As Integer = Val(Mid(gridView.Rows(k).Cells("RUNNO").Value.ToString, 8))
                If gridView.Rows(k).Cells("RUNNO").Value.ToString.StartsWith("GV") Then
                    tranno = Val(Mid(gridView.Rows(k).Cells("RUNNO").Value.ToString, 7))
                End If
                Dim amt As Decimal = Val(gridView.Rows(k).Cells("AMOUNT").Value.ToString)

                strSql = "SELECT DUEDAYS FROM " & cnStockDb & "..GVTRAN WHERE RUNNO='" & runno & "'"
                Dim Due As Integer = Val(objGPack.GetSqlValue(strSql, "DUEDAYS", 0, tran).ToString)
                duedate = DateAdd(DateInterval.Day, Due, editBilldate)
                InsertIntoOustanding(tranno, "GV", runno, amt, "R", "GV", , , , 0, amt, CardCode, , , , duedate, "GV", , gridView.Rows(k).Cells("ACCODE").Value.ToString, , , , , tran)
                Gift_Personalinfoinsert(editBilldate, editBatchno, tran, gridView.Rows(k).Cells("ACCODE").Value.ToString)
                tran.Commit()
            Next
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        btnNew_Click(Me, New EventArgs)
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
        funcSave()
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

    Private Sub txtVoucherno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVoucherno.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = "  SELECT "
            strSql += vbCrLf + "  (SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE=T.CARDID)NAME "
            strSql += vbCrLf + "  ,RUNNO,QTY,AMOUNT"
            strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID)COSTNAME "
            strSql += vbCrLf + "  FROM  " & cnStockDb & "..GVTRAN T"
            strSql += vbCrLf + "  WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO=T.RUNNO) "
            strSql += vbCrLf + "  AND COMPANYID='" & strCompanyId & "'"
            strSql += vbCrLf + "  AND COSTID='" & cnCostId & "'"
            If cmbGV.Text <> "" Then
                strSql += vbCrLf + "  AND CARDID=(SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME='" & cmbGV.Text & "')"
            End If
            strSql += vbCrLf + "  ORDER BY RUNNO"
            Dim Voucher As String = BrighttechPack.SearchDialog.Show("Find Gift Voucher", strSql, cn, 1, 1, , txtVoucherno.Text)
            If Voucher <> "" Then
                txtVoucherno.Text = Voucher
            Else
                txtVoucherno.Focus()
                txtVoucherno.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtVoucherno_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVoucherno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim vouNo As String = txtVoucherno.Text.ToString
            Dim _VouAmt As String = ""
            If txtVoucherno.Text.ToString = "" Then MsgBox("Voucher No should not empty..", MsgBoxStyle.Information) : Exit Sub
            vouNo = vouNo.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
            vouNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & vouNo

            For Each gv As DataRow In dtGv.Rows
                If gv!RUNNO = vouNo Then
                    strSql = "SELECT RUNNO FROM " & cnStockDb & "..GVTRAN WHERE RUNNO LIKE '%" & txtVoucherno.Text.ToString & "'"
                    vouNo = objGPack.GetSqlValue(strSql, , "")
                End If
            Next

            If vouNo = "" Then Exit Sub
            strSql = "SELECT AMOUNT FROM " & cnStockDb & "..GVTRAN WHERE RUNNO LIKE '" & vouNo & "'"
            _VouAmt = objGPack.GetSqlValue(strSql, , "")
            If _VouAmt = "" Then
                strSql = "SELECT RUNNO FROM " & cnStockDb & "..GVTRAN WHERE RUNNO LIKE '%" & txtVoucherno.Text.ToString & "'"
                vouNo = objGPack.GetSqlValue(strSql, , "")
                If vouNo = "" Then
                    MsgBox("Voucher Not Valid...")
                    txtVoucherno.Clear()
                    txtVoucherno.Focus()
                    Exit Sub
                Else
                    strSql = "SELECT AMOUNT FROM " & cnStockDb & "..GVTRAN WHERE RUNNO LIKE '" & vouNo & "'"
                    _VouAmt = objGPack.GetSqlValue(strSql, , "")
                End If
            End If
            strSql = "SELECT AMOUNT FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & vouNo & "'"
            If objGPack.GetSqlValue(strSql, , "") <> "" Then
                MsgBox("Voucher Not Valid...")
                txtVoucherno.Clear()
                txtVoucherno.Focus()
                Exit Sub
            End If
            For Each gv As DataRow In dtGv.Rows
                If gv!RUNNO = vouNo Then
                    MsgBox("Voucher No already loaded...")
                    txtVoucherno.Clear()
                    gridView.DataSource = Nothing
                    gridView.DataSource = dtGv
                    txtVoucherno.Focus()
                    Exit Sub
                End If
            Next
            Dim dr As DataRow
            dr = dtGv.NewRow
            dr!GIFTNAME = cmbGV.Text.ToString
            dr!ACCODE = txtAccode.Text.ToString
            dr!RUNNO = vouNo
            dr!AMOUNT = _VouAmt
            dtGv.Rows.Add(dr)
            gridView.DataSource = Nothing
            gridView.DataSource = dtGv
            If gridView.Rows.Count > 0 Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            txtVoucherno.Text = ""
        End If
    End Sub

    Private Sub txtAccode_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAccode.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = "  SELECT "
            strSql += vbCrLf + "  ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,MOBILE,PHONENO "
            strSql += vbCrLf + "  FROM  " & cnAdminDb & "..ACHEAD "
            strSql += vbCrLf + "  WHERE ISNULL(ACTYPE,'')='C' "
            'strSql += vbCrLf + "  WHERE ISNULL(OUTSTANDING,'')='Y' "
            'strSql += vbCrLf + "  AND ACGRPCODE IN (SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME='SUNDRY DEBTORS')"
            strSql += vbCrLf + "  ORDER BY ACNAME"
            Dim AcCode As String = BrighttechPack.SearchDialog.Show("Find A/c Name", strSql, cn, 1, , , txtAccode.Text)
            If AcCode <> "" Then
                txtAccode.Text = AcCode
            Else
                txtAccode.Focus()
                txtAccode.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtAccode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtVoucherno.Focus()
        End If
    End Sub

    Private Sub cmbGV_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbGV.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAccode.Focus()
        End If
    End Sub

End Class