Imports System.Data.OleDb
Public Class frmOpeningTrailBalance
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim costId As String = Nothing
    Dim lastData As Object

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmOpeningTrailBalance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FillDatatable()
        costId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "'")
        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPOPENTBAL')>0"
        strSql += " DROP TABLE TEMPTABLEDB..TEMPOPENTBAL"
        strSql += " SELECT ACNAME"
        strSql += " ,CASE WHEN T.DEBIT = 0 THEN NULL ELSE T.DEBIT END DEBIT"
        strSql += " ,CASE WHEN T.CREDIT = 0 THEN NULL ELSE T.CREDIT END CREDIT"
        strSql += " ,CONVERT(VARCHAR(7),H.ACCODE)ACCODE,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = H.ACGRPCODE)AS ACGRPNAME"
        strSql += " ,1 RESULT,ADDRESS1,ADDRESS2"
        strSql += " INTO TEMPTABLEDB..TEMPOPENTBAL"
        strSql += " FROM " & cnAdminDb & "..ACHEAD AS H "
        strSql += " LEFT OUTER JOIN " & cnstockdb & "..OPENTRAILBALANCE AS T ON H.ACCODE = T.ACCODE "
        strSql += " AND T.COMPANYID = '" & strCompanyId & "'"
        If cmbCostCenter.Enabled And cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL" Then
            strSql += " AND ISNULL(T.COSTID,'') = '" & costId & "'"
        End If
        strSql += " WHERE 1=1"
        If chkEmptyTrans.Checked Then
            strSql += " AND T.DEBIT <> 0 OR T.CREDIT <> 0"
        End If
        strSql += GetAcNameQryFilteration("H")
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMPTABLEDB..TEMPOPENTBAL(ACNAME,ACGRPNAME,RESULT)"
        strSql += " SELECT DISTINCT ACGRPNAME,ACGRPNAME,0 RESULT FROM TEMPTABLEDB..TEMPOPENTBAL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMPTABLEDB..TEMPOPENTBAL(ACGRPNAME,ACNAME,RESULT)"
        strSql += " SELECT 'ZZZZZ'ACGRPNAME,'TOTAL'ACNAME,2 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'ZZZZZ'ACGRPNAME,'BALANCE'ACNAME,3 RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM TEMPTABLEDB..TEMPOPENTBAL"
        strSql += " WHERE 1 = 1"
        If cmbAcGroup_OWN.Text <> "ALL" Then
            Dim acGrpCode As String = objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup_OWN.Text & "'")
            strSql += " AND (ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '" & acGrpCode & "')"
            strSql += " OR RESULT IN (2,3) OR ACNAME = '" & cmbAcGroup_OWN.Text & "')"
        End If
        If cmbAcName_OWN.Text <> "ALL" Then
            strSql += " AND (ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName_OWN.Text & "')"
            strSql += " OR RESULT IN (0,2,3))"
        End If
        strSql += " ORDER BY ACGRPNAME,RESULT,ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView = New DataTable
        da.Fill(dtGridView)
        gridView_OWN.DataSource = dtGridView
        CalcTotal()
        'ftrGridView()
        GridStyle(gridView_OWN)
    End Sub

    Private Sub GridStyle(ByVal dgv As DataGridView)
        With dgv
            .Columns("ACNAME").Width = 576
            .Columns("ACNAME").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("ACNAME").ReadOnly = True
            .Columns("DEBIT").Width = 200
            .Columns("DEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DEBIT").DefaultCellStyle.Format = "#,##,###.00"
            .Columns("DEBIT").ReadOnly = False
            .Columns("CREDIT").Width = 200
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("CREDIT").DefaultCellStyle.Format = "#,##,###.00"
            .Columns("CREDIT").ReadOnly = False
            For cnt As Integer = 3 To .ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Sub

    Private Sub frmOpeningTrailBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView_OWN.RowTemplate.Resizable = DataGridViewTriState.False
        gridView_OWN.BorderStyle = BorderStyle.None
        gridView_OWN.RowHeadersVisible = False
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView_OWN.RowTemplate.Height = 21
        gridView_OWN.Font = New Font("VERDANA", 9, FontStyle.Regular)
        gridView_OWN.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        cmbAcGroup_OWN.Items.Clear()
        cmbAcGroup_OWN.Items.Add("ALL")
        strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        objGPack.FillCombo(strSql, cmbAcGroup_OWN, False, False)
        cmbAcGroup_OWN.Text = "ALL"
        LoadAcName()

        cmbCostCenter.Text = ""
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            cmbCostCenter.Enabled = True
            cmbCostCenter.Items.Clear()
            cmbCostCenter.Items.Add("ALL")
            objGPack.FillCombo("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY COSTNAME", cmbCostCenter, False, False)
        Else
            cmbCostCenter.Enabled = False
        End If
        gridViewHeader.ColumnHeadersVisible = False
        gridViewHeader.RowTemplate.Height = 21
        gridViewHeader.Font = New Font("VERDANA", 9, FontStyle.Regular)
        Dim dtGridViewHeader As New DataTable
        dtGridViewHeader.Columns.Add("ACNAME", GetType(String))
        dtGridViewHeader.Columns.Add("DEBIT", GetType(Double))
        dtGridViewHeader.Columns.Add("CREDIT", GetType(Double))
        dtGridViewHeader.Rows.Add()
        dtGridViewHeader.Rows.Add()
        gridViewHeader.DataSource = dtGridViewHeader
        gridViewHeader.Rows(0).Cells("ACNAME").Value = "TOTAL"
        gridViewHeader.Rows(1).Cells("ACNAME").Value = "DIFFERENCE"
        gridViewHeader.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        gridViewHeader.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        gridViewHeader.DefaultCellStyle.ForeColor = Color.Black
        gridViewHeader.DefaultCellStyle.SelectionForeColor = Color.Black
        gridViewHeader.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        GridStyle(gridViewHeader)
        gridViewHeader.Enabled = False
        lblFindHelp.Visible = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveIntoTrailBalance()
        Dim rwIndex As Integer = gridView_OWN.CurrentCell.RowIndex
        Dim debit As Double = 0
        Dim credit As Double = 0
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DEBIT" Then
            debit = Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString)
        Else
            credit = Val(gridView_OWN.CurrentRow.Cells("CREDIT").Value.ToString)
        End If
        Dim insOutstdt As Boolean = False
        Dim amt As Double = IIf(debit > 0, _
        debit, _
        credit)
        If amt = 0 Then GoTo AFTEROUTSTDT
        Dim objOutstDet As New frmOpeningTrailBalOutStDt(amt, gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString)
        objOutstDet.COSTID = costId
        objOutstDet.type = IIf(debit > 0, frmOpeningTrailBalOutStDt.EntryType.Debit, frmOpeningTrailBalOutStDt.EntryType.Credit)
        With gridView_OWN.Rows(rwIndex)
            If objGPack.GetSqlValue("SELECT OUTSTANDING FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & .Cells("ACNAME").Value.ToString & "'") = "Y" Then
                If objOutstDet.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    gridView_OWN.CurrentCell.Value = lastData
                    GoTo AFTERSAVE
                Else
                    insOutstdt = True
                End If
            End If
        End With
AFTEROUTSTDT:
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            With gridView_OWN.Rows(rwIndex)
                If insOutstdt Then
                    strSql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING"
                    strSql += " WHERE ACCODE = '" & .Cells("ACCODE").Value.ToString & "'"
                    strSql += " AND ISNULL(COSTID,'') = '" & costId & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND FROMFLAG = 'O'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                End If
                If Val(.Cells("DEBIT").Value.ToString) = 0 And Val(.Cells("CREDIT").Value.ToString) = 0 Then
                    strSql = "DELETE FROM " & cnStockDb & "..OPENTRAILBALANCE"
                    strSql += " WHERE ACCODE = '" & .Cells("ACCODE").Value.ToString & "'"
                    strSql += " AND ISNULL(COSTID,'') = '" & costId & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                    tran.Commit()
                    GoTo AFTERSAVE
                End If

                Dim batchno As String = GetNewBatchno(cnCostId, GetEntryDate(GetServerDate(tran), tran), tran)
                If insOutstdt Then
                    For Each ro As DataRow In objOutstDet.dtGridView.Rows
                        Dim recPay As String = Nothing
                        Dim payMode As String = Nothing
                        If credit > 0 Then
                            Select Case ro!REFTYPE.ToString
                                Case "DUE"
                                    recPay = "P"
                                    payMode = "DP"
                                Case "DUE REC"
                                    recPay = "R"
                                    payMode = "DR"
                                Case "ADVANCE"
                                    recPay = "R"
                                    payMode = "AR"
                            End Select
                        Else
                            Select Case ro!REFTYPE.ToString
                                Case "DUE"
                                    recPay = "R"
                                    payMode = "DR"
                                Case "DUE REC"
                                    recPay = "P"
                                    payMode = "DP"
                                Case "ADVANCE"
                                    recPay = "P"
                                    payMode = "AA"
                            End Select
                        End If
                        InsertIntoOustanding(batchno, ro!TRANNO, Mid(ro!RUNNO.ToString, 1, 1), GetCostId(costId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, Val(ro!AMOUNT.ToString), recPay _
                        , ro!TRANDATE, payMode, , , , , , , , , , , ro!REMARK.ToString, , .Cells("ACCODE").Value.ToString)
                    Next
                    InsertIntoPersonalinfo(tran, batchno, .Cells("ACCODE").Value.ToString, GetEntryDate(GetServerDate(tran), tran) _
                    , .Cells("ACNAME").Value.ToString, .Cells("ADDRESS1").Value.ToString, .Cells("ADDRESS2").Value.ToString)
                End If
                strSql = "SELECT ACCODE FROM " & cnStockDb & "..OPENTRAILBALANCE"
                strSql += " WHERE ACCODE = '" & .Cells("ACCODE").Value.ToString & "'"
                strSql += " AND ISNULL(COSTID,'') = '" & costId & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                    ''UPDATE
                    strSql = " UPDATE " & cnStockDb & "..OPENTRAILBALANCE SET"
                    strSql += " ACCODE = '" & .Cells("ACCODE").Value.ToString & "'"
                    strSql += " ,DEBIT = " & debit & ""
                    strSql += " ,CREDIT = " & credit & ""
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
                    strSql += " WHERE ACCODE = '" & .Cells("ACCODE").Value.ToString & "'"
                    strSql += " AND ISNULL(COSTID,'') = '" & costId & "'"
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
                    strSql += " '" & .Cells("ACCODE").Value.ToString & "'" 'ACCODE
                    strSql += " ," & debit & "" 'DEBIT
                    strSql += " ," & credit & "" 'CREDIT
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
            End With
            tran.Commit()
            Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
                Case "DEBIT"
                    gridView_OWN.Rows(rwIndex).Cells("CREDIT").Value = DBNull.Value
                Case "CREDIT"
                    gridView_OWN.Rows(rwIndex).Cells("DEBIT").Value = DBNull.Value
            End Select
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
AFTERSAVE:
    End Sub

    Private Sub InsertIntoOustanding _
   ( _
   ByVal BATCHNO As String, ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
   ByVal RecPay As String, _
   ByVal recDate As Date, _
   ByVal PAYMODE As String, _
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
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & GetEntryDate(recDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
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
        strSql += " ,'" & PAYMODE & "'" 'PAYMODE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
    End Sub

    Private Sub CalcTotal()
        Dim dv As DataView = CType(gridView_OWN.DataSource, DataTable).DefaultView
        Dim debit As Object = dv.ToTable.Compute("SUM(DEBIT)", "RESULT NOT IN (2,3)")
        Dim credit As Object = dv.ToTable.Compute("SUM(CREDIT)", "RESULT NOT IN (2,3)")
        Dim bal As Double = Val(debit.ToString) - Val(credit.ToString)
        gridViewHeader.Rows(0).Cells("DEBIT").Value = IIf(Val(debit.ToString) <> 0, debit.ToString, DBNull.Value)
        gridViewHeader.Rows(0).Cells("CREDIT").Value = IIf(Val(credit.ToString) <> 0, credit.ToString, DBNull.Value)
        gridViewHeader.Rows(1).Cells("CREDIT").Value = IIf(bal > 0, bal, DBNull.Value)
        gridViewHeader.Rows(1).Cells("DEBIT").Value = IIf(bal < 0, -1 * bal, DBNull.Value)
        If gridView_OWN.RowCount > 2 Then
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).Cells("DEBIT").Value = IIf(Val(debit.ToString) <> 0, debit.ToString, DBNull.Value)
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).Cells("CREDIT").Value = IIf(Val(credit.ToString) <> 0, credit.ToString, DBNull.Value)
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("CREDIT").Value = IIf(bal > 0, bal, DBNull.Value)
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("DEBIT").Value = IIf(bal < 0, -1 * bal, DBNull.Value)
        End If
    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView_OWN.CellBeginEdit
        lastData = gridView_OWN.CurrentCell.Value
    End Sub

    'Private Sub ftrGridView()
    '    Dim dv As New DataView
    '    Dim ftrStr As String = ""
    '    'If cmbAcGroup_OWN.Text <> "ALL" Then
    '    '    ftrStr += "ACGRPNAME = '" & cmbAcGroup_OWN.Text & "'"
    '    '    ftrStr += " OR ACGRPNAME = 'ZZZZZ'"
    '    'End If
    '    'If cmbAcName_OWN.Text <> "ALL" Then
    '    '    ftrStr = "RESULT = '0'"
    '    '    '            ftrStr = "ACNAME = '" & objGPack.GetSqlValue("SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName_OWN.Text & "')") & "'"
    '    '    ftrStr += " OR ACNAME = '" & cmbAcName_OWN.Text & "'"
    '    '    ftrStr += " OR RESULT = '2'"
    '    '    'ftrStr += " OR ACGRPNAME = 'ZZZZZ'"
    '    'End If
    '    dv = dtGridView.DefaultView
    '    dv.RowFilter = ftrStr
    '    gridView.DataSource = dv.ToTable
    '    CalcTotal()
    'End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellEndEdit
        If cmbCostCenter.Enabled Then
            If cmbCostCenter.Text = "" Or cmbCostCenter.Text = "ALL" Then
                gridView_OWN.CurrentCell.Value = lastData
                Exit Sub
            End If
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not Val(gridView_OWN.CurrentCell.Value.ToString) > 0 Then
            gridView_OWN.CurrentCell.Value = DBNull.Value
        End If
        SaveIntoTrailBalance()
        CalcTotal()
    End Sub

    Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView_OWN.CellFormatting
        Select Case gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString
            Case "0" 'TITLE
                gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 10, FontStyle.Bold)
                gridView_OWN.Rows(e.RowIndex).Cells("ACNAME").Style.BackColor = Color.LightBlue
                gridView_OWN.Rows(e.RowIndex).Cells("ACNAME").Style.ForeColor = Color.Black
                gridView_OWN.Rows(e.RowIndex).Cells("ACNAME").Style.SelectionBackColor = Color.LightBlue
                gridView_OWN.Rows(e.RowIndex).Cells("ACNAME").Style.SelectionForeColor = Color.Black
                gridView_OWN.Rows(e.RowIndex).ReadOnly = True
            Case "1" 'TRAN
                Select Case gridView_OWN.Columns(e.ColumnIndex).Name
                    Case "DEBIT"
                        If Val(e.Value.ToString) > 0 Then
                            gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
                        Else
                            gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White
                        End If
                    Case "CREDIT"
                        If Val(e.Value.ToString) > 0 Then
                            gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LavenderBlush
                        Else
                            gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White
                        End If
                End Select
            Case "2" 'TOTAL,
                gridView_OWN.Rows(e.RowIndex).ReadOnly = True
                gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            Case "3" 'BALANCE
                gridView_OWN.Rows(e.RowIndex).ReadOnly = True
                gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End Select
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView_OWN.ColumnWidthChanged
        gridViewHeader.Columns("ACNAME").Width = gridView_OWN.Columns("ACNAME").Width
        gridViewHeader.Columns("DEBIT").Width = gridView_OWN.Columns("DEBIT").Width
        gridViewHeader.Columns("CREDIT").Width = gridView_OWN.Columns("CREDIT").Width
    End Sub

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView_OWN.DataError

    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView_OWN.EditingControlShowing
        Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
            Case "DEBIT", "CREDIT"
                Dim tb As TextBox = CType(e.Control, TextBox)
                AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
        End Select
    End Sub

    Private Sub txtAmt_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = "-" Then
            e.Handled = True
            Exit Sub
        End If
        AmountValidation(sender, e)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        FillDatatable()
        If gridView_OWN.RowCount > 0 Then
            gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("DEBIT")
            gridView_OWN.Focus()
        End If
    End Sub

    Private Sub LoadAcName()
        cmbAcName_OWN.Items.Clear()
        cmbAcName_OWN.Items.Add("ALL")
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        If cmbAcGroup_OWN.Text <> "ALL" And cmbAcGroup_OWN.Text <> "" Then
            strSql += " AND ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup_OWN.Text & "')"
        End If
        strSql += GetAcNameQryFilteration()
        objGPack.FillCombo(strSql, cmbAcName_OWN, False, False)
        cmbAcName_OWN.Text = "ALL"
    End Sub

    Private Sub cmbAcGroup_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcGroup_OWN.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbAcGroup_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup_OWN.LostFocus
        LoadAcName()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblFindHelp.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView_OWN.RowCount > 0 Then
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells(gridView_OWN.CurrentCell.ColumnIndex)
            End If
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        cmbCostCenter.Text = cnCostName
        gridView_OWN.DataSource = Nothing
        dtGridView = New DataTable
        cmbAcGroup_OWN.Text = "ALL"
        cmbAcName_OWN.Text = "ALL"
        If cmbCostCenter.Enabled Then
            cmbCostCenter.Focus()
        Else
            FillDatatable()
            Me.SelectNextControl(cmbCostCenter, True, True, True, True)
        End If

    End Sub

    Private Sub cmbAcName_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName_OWN.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbCostCenter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCenter.LostFocus
        If cmbCostCenter.Text <> "" And cmbCostCenter.Items.Contains(cmbCostCenter.Text) Then
            FillDatatable()
        Else
            gridView_OWN.DataSource = Nothing
            gridViewHeader.Rows(0).Cells("DEBIT").Value = DBNull.Value
            gridViewHeader.Rows(0).Cells("CREDIT").Value = DBNull.Value
            gridViewHeader.Rows(1).Cells("DEBIT").Value = DBNull.Value
            gridViewHeader.Rows(1).Cells("CREDIT").Value = DBNull.Value
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Opening TrailBalance", gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Opening TrailBalance", gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN)
        objSearch.ShowDialog()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblFindHelp.Visible = False
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
        strSql += " ,'" & GetEntryDate(billdate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
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
End Class