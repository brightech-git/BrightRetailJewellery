Imports System.Data.OleDb
Public Class AutoOrder
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim batchNo As String
    Dim TRANNO As Integer
    Public BillCostId As String
    Public BillDate As Date
    Public BillCashCounterId As String
    Dim VATEXM As String
    Dim RndId As Integer
    Public MANBILLNO As Boolean = False

    Dim objManualBill As frmManualBillNoGen
    Dim dtGrid As New DataTable
    Dim AddressEdit As Boolean = False
    Public OrderNo As String
    Public pcsflag As Boolean = False
    Dim itemid, subitemid As Double
    Dim pcs As Integer
    Dim propsmith As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        cmbItemName_Man.Items.Clear()
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast Order by ItemName"
        objGPack.FillCombo(strSql, cmbItemName_Man, False, False)

        cmbSubItemName_Own.Items.Clear()
        strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItemName_Man.Text & "') ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItemName_Own, False, False)

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in ('D','G')"
        objGPack.FillCombo(strSql, cmbPartyName, , False)
        MANBILLNO = IIf(GetAdmindbSoftValue("MANORDNO", "N") = "Y", True, False)
        BillDate = DateTime.Now

    End Sub

    

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not MANBILLNO Then
            If Not CheckDate(BillDate) Then Exit Sub
            If CheckEntryDate(BillDate) Then Exit Sub
        End If
        If cmbItemName_Man.Text = "" Then
            MsgBox("Item Name Should Not Empty", MsgBoxStyle.Information)
            cmbItemName_Man.Focus()
            Exit Sub
        End If
        strSql = " select Subitem from " & cnAdminDb & "..ItemMast where itemname = '" & cmbItemName_Man.Text & "'"
        If "" & objGPack.GetSqlValue(strSql) = "Y" Then
            If cmbSubItemName_Own.Text = "" Then
                MsgBox("SubItem Name Should Not Empty", MsgBoxStyle.Information)
                cmbSubItemName_Own.Focus()
                Exit Sub
            End If
        End If
        If cmbPartyName.Text = "" Then
            MsgBox("Party Name Should Not Empty", MsgBoxStyle.Information)
            cmbPartyName.Focus()
            Exit Sub
        End If
        If Val(txtPiece_NUM.Text) <= 0 Then
            MsgBox("PCS Should Not Zero", MsgBoxStyle.Information)
            txtPiece_NUM.Focus()
            Exit Sub
        End If

        strSql = " select accode from " & cnAdminDb & "..Achead where Acname = '" & cmbPartyName.Text & "'"
        propsmith = "" & objGPack.GetSqlValue(strSql)
        If MANBILLNO Then
            objManualBill = New frmManualBillNoGen
            frmManualBillNoGen.Text = "Manual Order No"
            objGPack.Validator_Object(objManualBill)
ReEnterBillNO:
            If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                TRANNO = Val(objManualBill.txtBillNo_NUM.Text)
                strSql = " SELECT DISTINCT ORNO FROM " & cnadmindb & "..ORMAST"
                strSql += " WHERE ORNO = '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TRANNO.ToString & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                strSql += " UNION"
                strSql += " SELECT DISTINCT RUNNO FROM " & cnAdminDb & "..OUTSTANDING"
                strSql += " WHERE RUNNO = '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TRANNO.ToString & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("Order BillNo Already Exist", MsgBoxStyle.Information)
                    GoTo ReEnterBillNO
                End If
            Else
                btnSave.Focus()
                Me.Close()
                Exit Sub
            End If
        End If
        Me.Refresh()
        Try

            If cmbItemName_Man.Text.ToString <> "" Then
                strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_Man.Text.ToString & "'"
                itemid = Val(GetSqlValue(cn, strSql))
            End If
            If cmbSubItemName_Own.Text.ToString <> "" Then
                strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItemName_Own.Text.ToString & "'"
                subitemid = Val(GetSqlValue(cn, strSql))
            End If

            tran = Nothing
            tran = cn.BeginTransaction()
            batchNo = GetNewBatchno(cnCostId, BillDate, tran)

GETORDERNO:
            If Not MANBILLNO Then
                strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'ORDERNO' AND COMPANYID = '" & strCompanyId & "'"
                TRANNO = Val(objGPack.GetSqlValue(strSql, , , tran))

                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TRANNO + 1 & "' "
                strSql += " WHERE CTLID = 'ORDERNO' AND COMPANYID = '" & strCompanyId & "'"
                strSql += " AND CONVERT(INT,CTLTEXT) = " & TRANNO & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GETORDERNO
                End If
                strSql = " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORMAST"
                strSql += " WHERE ORNO = '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & Val(TRANNO + 1).ToString & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    GoTo GETORDERNO
                End If
                TRANNO = TRANNO + 1
            End If
            OrderNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TRANNO
            InsertOrderDetail(itemid, subitemid)
            tran.Commit()
            tran = Nothing
            MsgBox("OrderNo : " & Mid(OrderNo, 6, 20))
            Dim pBatchno As String = batchNo
            Dim pBilldate As Date = BillDate.ToString("yyyy-MM-dd")

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub



    Private Sub InsertOrderDetail(ByVal itemid As Double, ByVal subitemid As Double)

        If txtPiece_NUM.Text.ToString < 0 Then
            txtPiece_NUM.Text = ""
        End If
        Dim orSno As String = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")
        strSql = " INSERT INTO " & cnAdminDb & "..ORMAST"
        strSql += " ("
        strSql += " SNO,ORNO,ORDATE,REMDATE,DUEDATE,ORTYPE,COMPANYID,ORRATE"
        strSql += " ,ORMODE,ITEMID,SUBITEMID,DESCRIPT,PCS,GRSWT,NETWT"
        strSql += " ,SIZEID,RATE,NATURE,MCGRM,MC,WASTPER,WAST"
        strSql += " ,COMMPER,COMM,OTHERAMT,CANCEL,ORVALUE,COSTID,BATCHNO"
        strSql += " ,CORNO,PROPSMITH,PICTFILE,EMPID"
        strSql += " ,USERID,UPDATED,UPTIME,APPVER"
        strSql += " ,TAX,SC,ADSC,ITEMTYPEID,SIZENO,STYLENO,DISCOUNT"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & orSno & "'" 'SNO
        strSql += " ,'" & OrderNo & "'" 'ORNO
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
        strSql += " ,'" & BillDate.Date.ToString("yyyy-MM-dd") & "'" 'REMDATE
        strSql += " ,'" & BillDate.Date.ToString("yyyy-MM-dd") & "'" 'DUEDATE
        strSql += " ,'O'"
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'C'"
        strSql += " ,'O'"
        strSql += " ," & Val("" & itemid)
        strSql += " ," & Val("" & subitemid)
        strSql += " ,''" 'DESCRIPT
        strSql += " ," & Val(txtPiece_NUM.Text.ToString) & "" 'PCS
        strSql += " ," & Val(txtWeight_WET.Text.ToString) & "" 'GRSWT
        strSql += " ," & Val(txtWeight_WET.Text.ToString) & "" 'NETWT
        strSql += " ,0"  'SIZEID
        strSql += " ,0" 'RATE
        strSql += " ,''" 'NATURE
        strSql += " ,0" 'MCGRM
        strSql += " ,0" 'MC
        strSql += " ,0" 'WASTPER
        strSql += " ,0" 'WAST
        strSql += " ,0"
        strSql += " ,0"
        strSql += " ,0"
        strSql += " ,''"
        strSql += " ,0"
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,0" 'CORNO
        strSql += " ,'" & propsmith & "'"
        strSql += " ,''" 'PICTFILE
        strSql += " ,0"
        strSql += " ," & userId & ""
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,'" & Date.Now.ToLongTimeString & "'"
        strSql += " ,'" & VERSION & "'"
        strSql += " ,0" 'TAX
        strSql += " ,0" 'SC
        strSql += " ,0" 'ADSC
        strSql += " ,0" ' ITEMTYPE
        strSql += " ,''" 'SIZENO
        strSql += " ,''" 'STYLENO
        strSql += " ,0"  'DISCOUNT
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)


        If cmbPartyName.Text <> "" Then

            Dim Isno As String = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
            Dim tranno As Integer = 0
            strSql = " SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            tranno = Val(objGPack.GetSqlValue(strSql, , , tran))
            Dim DESIGNERID As Integer = 0
            strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
            strSql += " ("
            strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,ORNO,COMPANYID,COSTID"
            strSql += " ,TAGNO,PROID,DESCRIPT,PCS,GRSWT,NETWT"
            strSql += " ,MC,WASTAGE,BATCHNO"
            strSql += " ,ENTRYORDER,ORDSTATE_ID"
            strSql += " ,USERID,UPDATED,UPTIME,APPVER,ORSTATUS"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & Isno & "'" 'SNO
            strSql += " ,'" & orSno & "'" 'ORsNO
            strSql += " ,'" & tranno & "'" 'ORNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
            strSql += " ," & DESIGNERID & ""
            strSql += " ,'" & OrderNo & "'" 'ORNO
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ,''"
            strSql += " ," & itemid
            strSql += " ,''"
            strSql += " ," & Val(txtPiece_NUM.Text) & "" 'PCS
            strSql += " ," & Val(txtWeight_WET.Text) & "" 'GRSWT
            strSql += " ," & Val(txtWeight_WET.Text) & "" 'NETWT
            strSql += " ,0" 'MC
            strSql += " ,0" 'WAST
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,0" 'CORNO
            strSql += " ,2" 'ORDSTATE_ID
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & VERSION & "'"
            strSql += " ,'I'" 'ORTYPE
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            Dim Rsno As String = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
            strSql = Replace(strSql, "'" + Isno + "'", "'" + Rsno + "'")
            strSql = Replace(strSql, ",'I' )", ",'R' )")
            strSql = Replace(strSql, ",0,2", ",0,4")
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            Me.Close()
        End If
        ''Stone
        'For Each stRow As DataRow In dtStoneDetails.Rows
        '    If .Cells("KEYNO").Value = stRow("KEYNO") Then
        '        InsertStoneDetails(stRow, orSno)
        '    End If
        'Next

        ''Sample
        'For Each samRow As DataRow In dtSampleDetails.Rows
        '    If .Cells("KEYNO").Value = samRow("KEYNO") Then
        '        InsertSampleDetails(samRow, orSno)
        '    End If
        'Next


    End Sub
    Private Sub btnExit_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub AutoOrder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txtPiece_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPiece_NUM.GotFocus
       
    End Sub
    Private Sub txtPiece_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPiece_NUM.TextChanged
        
    End Sub

    Private Sub AutoOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pcs = Val(txtPiece_NUM.Text)
    End Sub

    Private Sub txtPiece_NUM_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPiece_NUM.Leave
        If Val(txtPiece_NUM.Text) > pcs Then
            txtPiece_NUM.Text = pcs
            txtPiece_NUM.Focus()
        End If
    End Sub

    Private Sub rdbAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbAll.CheckedChanged
        If rdbAll.Checked Then
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in ('D','G')"
            objGPack.FillCombo(strSql, cmbPartyName, , False)
        End If
    End Sub

    Private Sub rdbSelected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbSelected.CheckedChanged
        If rdbSelected.Checked Then
            cmbPartyName.Items.Clear()
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..SMITHSUBITEMDETAIL WHERE SUBITEMID=(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItemName_Own.Text & "')"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtAcc As New DataTable
            da.Fill(dtAcc)
            If dtAcc.Rows.Count > 0 Then
                For j As Integer = 0 To dtAcc.Rows.Count - 1
                    strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & dtAcc.Rows(j).Item("ACCODE").ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim dtAcName As New DataTable
                    da.Fill(dtAcName)
                    If dtAcName.Rows.Count > 0 Then
                        For i As Integer = 0 To dtAcName.Rows.Count - 1
                            cmbPartyName.Items.Add(dtAcName.Rows(i).Item("ACNAME").ToString)
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub txtWeight_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_WET.GotFocus
        If cmbItemName_Man.Text <> "" And cmbSubItemName_Own.Text <> "" And Val(txtPiece_NUM.Text) > 0 Then
            Dim itemid As Integer
            Dim Grswt As String
            itemid = Val(GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_Man.Text & "'"))

            strSql = "SELECT ISNULL(GRSWT,0) GRSWT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & itemid & "' AND SUBITEMNAME='" & cmbSubItemName_Own.Text & "'"
            Grswt = GetSqlValue(cn, strSql)
            txtWeight_WET.Text = Val(Grswt) * Val(txtPiece_NUM.Text)
        End If
    End Sub
End Class