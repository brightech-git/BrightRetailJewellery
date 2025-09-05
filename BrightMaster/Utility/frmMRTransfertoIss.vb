Imports System.Data.OleDb
Public Class frmMRTransfertoIss
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim CheckState As Boolean
    Private WithEvents txtCheckedItems As New TextBox

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtTranNo.Clear()
        txtTranNo.Focus()  
        dtpTransfer.Value = GetEntryDate(GetServerDate())
        StrSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
        StrSql += " WHERE ACTYPE IN ('G','D','I')"
        StrSql += GetAcNameQryFilteration()
        StrSql += " ORDER BY ACNAME"
        Dim dtAcName As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtAcName)
        cmbAcName.DataSource = dtAcName
        cmbAcName.ValueMember = "ACCODE"
        cmbAcName.DisplayMember = "ACNAME"
        cmbAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbAcName.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtTranNo.Text = "" Then MsgBox("Refno is Empty", MsgBoxStyle.Information) : txtTranNo.Focus() : Exit Sub
        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtCheckedItems_TextChanged(Me, New EventArgs)
        Me.Refresh()
        StrSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_ACC') IS NOT NULL DROP TABLE MASTER..TEMP_ACC"
        StrSql += vbCrLf + " CREATE TABLE MASTER..TEMP_ACC"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " TRANNO INT"
        StrSql += vbCrLf + " ,TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " ,ITEMID INT  NULL"
        StrSql += vbCrLf + " ,SUBITEMID INT NULL"
        StrSql += vbCrLf + " ,TAGNO VARCHAR(12)"
        StrSql += vbCrLf + " ,CATNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,PCS INT"
        StrSql += vbCrLf + " ,GRSWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,NETWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,DIAPCS INT"
        StrSql += vbCrLf + " ,DIAWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,STNPCS INT"
        StrSql += vbCrLf + " ,STNWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,PREPCS INT"
        StrSql += vbCrLf + " ,PREWT NUMERIC(15,3)"
        StrSql += vbCrLf + " ,AMOUNT NUMERIC(15,2)"
        StrSql += vbCrLf + " ,TEMP_ACNAME VARCHAR(500)"
        StrSql += vbCrLf + " ,BATCHNO VARCHAR(15),ORD INT,COMPANYID VARCHAR(3)"
        StrSql += vbCrLf + " ,PAYMODE VARCHAR(10),CTLID VARCHAR(50),RESULT INT"
        StrSql += vbCrLf + " ,TYPE VARCHAR(12)"
        StrSql += vbCrLf + " )"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO MASTER..TEMP_ACC"
        StrSql += vbCrLf + " (TRANNO,TRANDATE,ITEMID,SUBITEMID,TAGNO,CATNAME,PCS,GRSWT"
        StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT"
        StrSql += vbCrLf + " ,RESULT)"
        StrSql += vbCrLf + "  SELECT DISTINCT "
        StrSql += vbCrLf + "  (SELECT  TOP 1 TRANNO FROM  " & cnStockDb & "..RECEIPT WHERE REFNO=T.TRANINVNO) AS TRANNO"
        StrSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE REFNO=T.TRANINVNO) AS TRANDATE"
        StrSql += vbCrLf + " ,T.ITEMID,T.SUBITEMID,T.TAGNO"
        StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))CATNAME"
        StrSql += vbCrLf + " ,SUM(T.PCS) PCS"
        StrSql += vbCrLf + " ,SUM(T.GRSWT) GRSWT"
        StrSql += vbCrLf + " ,SUM(T.NETWT) NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
        StrSql += vbCrLf + " ,0 AS RESULT"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        StrSql += vbCrLf + " WHERE T.ISSDATE IS NULL  "
        StrSql += vbCrLf + " AND T.COSTID='" & cnCostId & "'"
        If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND T.TRANINVNO='" & txtTranNo.Text & "'"
        StrSql += vbCrLf + " AND T.TRANINVNO NOT IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='MI' AND COSTID='" & cnCostId & "')"
        StrSql += vbCrLf + " GROUP BY T.SNO,T.TAGNO,T.ITEMID,T.SUBITEMID,T.TRANINVNO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " IF(SELECT COUNT(*) FROM MASTER..TEMP_ACC)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + "    INSERT INTO MASTER..TEMP_ACC"
        StrSql += vbCrLf + "    (CATNAME,PCS,GRSWT"
        StrSql += vbCrLf + "    ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,ORD,RESULT)"
        StrSql += vbCrLf + "    SELECT 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),9,2"
        StrSql += vbCrLf + "    FROM MASTER..TEMP_ACC"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " UPDATE MASTER..TEMP_ACC SET PCS=NULL WHERE PCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET STNPCS=NULL WHERE STNPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET DIAPCS=NULL WHERE DIAPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET PREPCS=NULL WHERE PREPCS=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET GRSWT=NULL WHERE GRSWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET NETWT=NULL WHERE NETWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET DIAWT=NULL WHERE DIAWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET STNWT=NULL WHERE STNWT=0"
        StrSql += vbCrLf + " UPDATE MASTER..TEMP_ACC SET PREWT=NULL WHERE PREWT=0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * FROM MASTER..TEMP_ACC"
        StrSql += vbCrLf + " ORDER BY RESULT,TRANDATE,SUBSTRING(BATCHNO,8,10),TRANNO,ORD,TAGNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("APPROVED", GetType(Boolean))
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("APPROVED").SetOrdinal(0)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        Dgv.DataSource = dtGrid
        With Dgv
            For Each dgvRow As DataGridViewRow In Dgv.Rows
                Select Case dgvRow.Cells("RESULT").Value.ToString
                    Case "2"
                        dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                        dgvRow.DefaultCellStyle.ForeColor = Color.Red
                        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            Next
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).ReadOnly = True
            Next
            For CNT As Integer = 9 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("APPROVED").ReadOnly = False
            .Columns("APPROVED").HeaderText = "CHECK"
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").Visible = True
            .Columns("DIAWT").Visible = True
            .Columns("STNPCS").Visible = True
            .Columns("STNWT").Visible = True
            .Columns("PREPCS").Visible = True
            .Columns("PREWT").Visible = True
            .Columns("AMOUNT").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        Dgv.Select()
    End Sub

    Private Sub AccApproval_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub AccApproval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AccApproval_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv.CellValueChanged
        If CheckState = False Then Exit Sub
        Dim bool As Boolean = CType(IIf(IsDBNull(Dgv.CurrentRow.Cells("APPROVED").Value), False, Dgv.CurrentRow.Cells("APPROVED").Value), Boolean)
        Dim batchno As String = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "BATCHNO = '" & batchno & "'"
        Dim dt As New DataTable
        dt = dv.ToTable
        CheckState = True
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If bool Then
                txtCheckedItems.Text = Val(txtCheckedItems.Text) + 1
                Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            Else
                txtCheckedItems.Text = Val(txtCheckedItems.Text) - 1
                Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.White
            End If
            Dgv.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).Cells("APPROVED").Value = bool
        Next
    End Sub

    Private Sub Dgv_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv.CurrentCellDirtyStateChanged
        CheckState = True
        Dgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub txtCheckedItems_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCheckedItems.TextChanged
        'If Val(txtCheckedItems.Text) = 0 Then
        '    btnTransfer.Enabled = False
        'Else
        '    btnTransfer.Enabled = True
        'End If
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim TranDate As Date = GetEntryDate(GetServerDate)
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "APPROVED = 'TRUE' AND RESULT=0"
        Dim dt As New DataTable
        dt = dv.ToTable
        Dim batchno As String = Nothing
        Dim NewBatchNo As String = Nothing
        Dim CtlId As String = Nothing
        Dim TranNo As Integer = Nothing
        Dim CostId As String = Nothing
        Dim tCompanyId As String = Nothing
        Dim billcontrolid As String = "GEN-SM-ISS"
        CtlId = billcontrolid
        Dim _Accode As String
        _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            batchno = GetNewBatchno(cnCostId, dtpTransfer.Value.ToString("yyyy-MM-dd"), tran)
            StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            StrSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()
            'GenBillNo:
            TranNo = GetBillNoValue("GEN-MISCISSBILLNO", tran)
            'TranNo = Val(GetBillControlValue(CtlId, tran))
            'StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
            'StrSql += " WHERE CTLID = '" & CtlId & "' AND COMPANYID = '" & tCompanyId & "'"
            'StrSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
            'Cmd = New OleDbCommand(StrSql, cn, tran)
            'If Not Cmd.ExecuteNonQuery() > 0 Then
            '    GoTo GenBillNo
            'End If
            'TranNo += 1
            For cnt As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(cnt)
                    Dim issSno, CatCode, MetalId As String
                    tCompanyId = .Item("COMPANYID").ToString
                    issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
                    CatCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran)
                    MetalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran)
                    StrSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                    StrSql += " ("
                    StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                    StrSql += " ,GRSWT,NETWT"
                    'StrSql += " ,LESSWT,PUREWT"
                    StrSql += " ,TAGNO,ITEMID,SUBITEMID"
                    StrSql += " ,SALEMODE,GRSNET"
                    StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                    StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                    StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                    StrSql += " ,ITEMCTRID,ITEMTYPEID,TABLECODE"
                    StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                    StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                    StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                    StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER)"
                    StrSql += " VALUES("
                    StrSql += " '" & issSno & "'" ''SNO
                    StrSql += " ," & TranNo & "" 'TRANNO
                    StrSql += " ,'" & dtpTransfer.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    StrSql += " ,'MI'" 'TRANTYPE
                    StrSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
                    StrSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
                    StrSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                    'StrSql += " ," & Val(.Item("LESSWT").ToString) & "" 'LESSWT
                    'StrSql += " ," & Val(.Item("PUREWT").ToString) & "" 'PUREWT
                    StrSql += " ,'" & .Item("TAGNO").ToString & "'" 'TAGNO
                    StrSql += " ," & Val(.Item("ITEMID").ToString)   'ITEMID
                    StrSql += " ," & Val(.Item("SUBITEMID").ToString)  'SUBITEMID
                    StrSql += " ,'W'" 'SALEMODE
                    StrSql += " ,'G'" 'GRSNET
                    StrSql += " ,''" 'TRANSTATUS ''
                    StrSql += " ,'" & txtTranNo.Text & "'" 'REFNO ''
                    StrSql += " ,NULL" 'REFDATE NULL
                    StrSql += " ,'" & cnCostId & "'" 'COSTID 
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,''" 'FLAG
                    StrSql += " ,0" 'EMPID
                    StrSql += " ,0" 'TAGGRSWT
                    StrSql += " ,0" 'TAGNETWT
                    StrSql += " ,0" 'TAGRATEID
                    StrSql += " ,0" 'TAGSVALUE
                    StrSql += " ,''" 'TAGDESIGNER  
                    StrSql += " ,0" 'ITEMCTRID
                    StrSql += " ,0"  'ITEMTYPEID
                    StrSql += " ,''" 'TABLECODE
                    StrSql += " ,''" 'INCENTIVE
                    StrSql += " ,''" 'WEIGHTUNIT
                    StrSql += " ,'" & catCode & "'" 'CATCODE
                    StrSql += " ,'" & CatCode & "'" 'OCATCODE
                    StrSql += " ,'" & _Accode & "'" 'ACCODE
                    StrSql += " ,0" 'ALLOY
                    StrSql += " ,'" & batchno & "'" 'BATCHNO
                    StrSql += " ,''"   'REMARK1
                    StrSql += " ,''"   'REMARK2
                    StrSql += " ,'" & userId & "'" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += " ,0" 'DISCOUNT
                    StrSql += " ,''" 'RUNNO
                    StrSql += " ,''" 'CASHID
                    StrSql += " ,0" 'TAX
                    StrSql += " ,0" 'TDS
                    StrSql += " ,0" 'STNAMT
                    StrSql += " ,0" 'MISCAMT
                    StrSql += " ,'" & MetalId & "'" 'METALID
                    StrSql += " ,''" 'STONEUNIT
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " )"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)

                    StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG "
                    StrSql += " SET ISSDATE = '" & dtpTransfer.Value.ToString("yyyy-MM-dd") & "'"
                    StrSql += " ,ISSREFNO = '" & TranNo & "'"
                    StrSql += " ,ISSPCS = " & Val(.Item("PCS").ToString) & ""
                    StrSql += " ,ISSWT = " & Val(.Item("GRSWT").ToString) & ""
                    StrSql += " ,TOFLAG = 'MI'"
                    StrSql += " ,BATCHNO = '" & batchno & "'"
                    StrSql += " ,COMPANYID = '" & strCompanyId & "'"
                    StrSql += " WHERE ITEMID = '" & Val(.Item("ITEMID").ToString) & "' "
                    StrSql += " AND TAGNO = '" & .Item("TAGNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)

                    Dim StnItemid, StnSubItemid, StnPcs As Integer
                    Dim StnUnit, StnCalType, StnCatcode As String
                    Dim StnWt, StnRate, StnAmt As Decimal
                    If Val(.Item("STNPCS").ToString) > 0 Or Val(.Item("STNWT").ToString) > 0 Then
                        StrSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO='" & .Item("TAGNO").ToString & "'"
                        StrSql += " AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')"
                        Dim dtStn As New DataTable
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Da = New OleDbDataAdapter(Cmd)
                        Da.Fill(dtStn)
                        If dtStn.Rows.Count > 0 Then
                            For J As Integer = 0 To dtStn.Rows.Count - 1
                                With dtStn.Rows(J)
                                    StnItemid = Val(.Item("STNITEMID").ToString)
                                    StnSubItemid = Val(.Item("STNSUBITEMID").ToString)
                                    StnPcs = Val(.Item("STNPCS").ToString)
                                    StnWt = Val(.Item("STNWT").ToString)
                                    StnUnit = .Item("STONEUNIT").ToString
                                    StnCalType = .Item("CALCMODE").ToString
                                    StnRate = Val(.Item("STNRATE").ToString)
                                    StnAmt = Val(.Item("STNAMT").ToString)
                                    StrSql = "SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & StnItemid
                                    StnCatcode = objGPack.GetSqlValue(StrSql, "CATCODE", "", tran).ToString
                                    InsertStoneDetails(issSno, TranNo, StnItemid, StnSubItemid _
                                    , StnPcs, StnWt, StnUnit, StnCalType, StnRate, StnAmt, StnCatcode, batchno)
                                End With
                            Next
                        End If
                    End If
                    If Val(.Item("DIAPCS").ToString) > 0 Or Val(.Item("DIAWT").ToString) > 0 Then
                        StrSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO='" & .Item("TAGNO").ToString & "'"
                        StrSql += " AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')"
                        Dim dtStn As New DataTable
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Da = New OleDbDataAdapter(Cmd)
                        Da.Fill(dtStn)
                        If dtStn.Rows.Count > 0 Then
                            For J As Integer = 0 To dtStn.Rows.Count - 1
                                With dtStn.Rows(J)
                                    StnItemid = Val(.Item("STNITEMID").ToString)
                                    StnSubItemid = Val(.Item("STNSUBITEMID").ToString)
                                    StnPcs = Val(.Item("STNPCS").ToString)
                                    StnWt = Val(.Item("STNWT").ToString)
                                    StnUnit = .Item("STONEUNIT").ToString
                                    StnCalType = .Item("CALCMODE").ToString
                                    StnRate = Val(.Item("STNRATE").ToString)
                                    StnAmt = Val(.Item("STNAMT").ToString)
                                    StrSql = "SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & StnItemid
                                    StnCatcode = objGPack.GetSqlValue(StrSql, "CATCODE", "", tran).ToString
                                    InsertStoneDetails(issSno, TranNo, StnItemid, StnSubItemid _
                                    , StnPcs, StnWt, StnUnit, StnCalType, StnRate, StnAmt, StnCatcode, batchno)
                                End With
                            Next
                        End If
                    End If
                    If Val(.Item("PREPCS").ToString) > 0 Or Val(.Item("PREWT").ToString) > 0 Then
                        StrSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO='" & .Item("TAGNO").ToString & "'"
                        StrSql += " AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')"
                        Dim dtStn As New DataTable
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Da = New OleDbDataAdapter(Cmd)
                        Da.Fill(dtStn)
                        If dtStn.Rows.Count > 0 Then
                            For J As Integer = 0 To dtStn.Rows.Count - 1
                                With dtStn.Rows(J)
                                    StnItemid = Val(.Item("STNITEMID").ToString)
                                    StnSubItemid = Val(.Item("STNSUBITEMID").ToString)
                                    StnPcs = Val(.Item("STNPCS").ToString)
                                    StnWt = Val(.Item("STNWT").ToString)
                                    StnUnit = .Item("STONEUNIT").ToString
                                    StnCalType = .Item("CALCMODE").ToString
                                    StnRate = Val(.Item("STNRATE").ToString)
                                    StnAmt = Val(.Item("STNAMT").ToString)
                                    StrSql = "SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & StnItemid
                                    StnCatcode = objGPack.GetSqlValue(StrSql, "CATCODE", "", tran).ToString
                                    InsertStoneDetails(issSno, TranNo, StnItemid, StnSubItemid _
                                    , StnPcs, StnWt, StnUnit, StnCalType, StnRate, StnAmt, StnCatcode, batchno)
                                End With
                            Next
                        End If
                    End If
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("TranNo " & TranNo.ToString & " Generated", MsgBoxStyle.Information)
            Dim Pbatchno As String = batchno
            Dim Pbilldate As Date
            Pbilldate = dtpTransfer.Value
            
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":POS")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & Pbatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":IIS" & ";" & _
                    LSet("BATCHNO", 15) & ":" & Pbatchno & ";" & _
                    LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub InsertStoneDetails(ByVal IssSno As String, ByVal TNO As Double _
    , ByVal StnItemid As Integer, ByVal StnSubItemid As Integer, ByVal StnPcs As Integer _
    , ByVal StnWt As Double, ByVal StnUnit As String, ByVal StnCalType As String _
    , ByVal StnRate As Double, ByVal StnAmt As Double _
    , ByVal StnCatCode As String, ByVal BatchNo As String)

        Dim Sno As String = GetNewSno(TranSnoType.ISSSTONECODE, tran)
        StrSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
        StrSql += " ("
        StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        StrSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        StrSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        StrSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        StrSql += " ,BATCHNO,SYSTEMID,CATCODE,APPVER"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & Sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTransfer.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'MI'" 'TRANTYPE 
        StrSql += " ," & StnPcs & "" 'STNPCS
        StrSql += " ," & StnWt & "" 'STNWT
        StrSql += " ," & StnRate & "" 'STNRATE
        StrSql += " ," & StnAmt & "" 'STNAMT
        StrSql += " ," & StnItemid & "" 'STNITEMID
        StrSql += " ," & StnSubItemid & "" 'STNSUBITEMID
        StrSql += " ,'" & StnCalType & "'" 'CALCMODE
        StrSql += " ,'" & StnUnit & "'" 'STONEUNIT
        StrSql += " ,''" 'STONEMODE 
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ,'" & cnCostId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & StnCatCode & "'" 'CATCODE
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
    End Sub

    Private Sub txtTranNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = "SELECT DISTINCT TRFNO FROM " & cnStockDb & "..TRECEIPT"
            Dim TrfNo As String = BrighttechPack.SearchDialog.Show("Select Transfer No", StrSql, cn, 0)
            If TrfNo <> "" Then
                txtTranNo.Text = TrfNo
                txtTranNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub cmbAcName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAcName.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        For i As Integer = 0 To Dgv.RowCount - 1
            Dgv.Rows(i).Cells("APPROVED").Value = ChkAll.Checked
        Next
    End Sub
End Class