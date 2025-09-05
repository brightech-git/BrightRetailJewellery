Imports System.Data.OleDb
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class frmPendingMR
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Dim Dasql As SqlDataAdapter
    Dim Cmdsql As SqlCommand

    Dim dt As DataTable
    Dim dtCompanyName As DataTable
    Dim CheckState As Boolean
    Private WithEvents txtCheckedItems As New TextBox
    Dim LOCK_MRMI_TRSDATE As Boolean = IIf(GetAdmindbSoftValue("LOCK_MRMI_TRSDATE", "Y") = "Y", True, False)
    Dim PENDTRF_SNOBASE As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_SNOBASE", "N") = "Y", True, False)
    Dim MFG_RECEIPT As Boolean = IIf(GetAdmindbSoftValue("MFG_RECEIPT", "N") = "Y", True, False)
    Dim Mfg_Con As OleDbConnection
    Dim Mfg_Tran As OleDbTransaction
    Dim cn_Sql As SqlConnection
    Dim MFG_SERVERNAME As String = GetAdmindbSoftValue("MFG_SERVERNAME", "")
    Dim MFG_USERNAME As String = GetAdmindbSoftValue("MFG_USERNAME", "")
    Dim MFG_PWD As String = ""
    Dim MFG_ACCODE As String = GetAdmindbSoftValue("MFG_ACCODE", "")
    Dim MFG_DB As Boolean = IIf(GetAdmindbSoftValue("MFG_DB", "N") = "Y", True, False)
    Dim MFG_CENTDB As Boolean = IIf(GetAdmindbSoftValue("MFG_CENTDB", "N") = "Y", True, False)
    Dim Manufacturing As Boolean = False
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dgv.DataSource = Nothing
        Manufacturing = False
        txtCheckedItems.Clear()
        txtTranNo.Clear()
        txtTranNo.Focus()
        lblHelp.Text = "LOCK_MRMI_TRSDATE:" & IIf(LOCK_MRMI_TRSDATE = True, "Y", "N")
        txtCheckedItems_TextChanged(Me, New EventArgs)
        If LOCK_MRMI_TRSDATE = False Then
            lblTranDate.Enabled = True
            dtpTransfer.Enabled = True
            dtpTransfer.Value = GetEntryDate(GetServerDate())
        End If
        'REFRESH COMPANY NAME
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , IIf(strCompanyName.ToString <> "" And SPECIFICFORMAT = 1, strCompanyName.ToString, "ALL"))
        If MFG_RECEIPT Then dtpTransfer.Enabled = MFG_RECEIPT
        dtpFrom2.Value = GetServerDate()
        dtpTo2.Value = GetServerDate()
        chkWithFromDate.Checked = False
        If chkWithFromDate.Checked Then
            Label2.Visible = True
            dtpfromdate.Visible = True
            dtpTodate.Visible = True
            dtpfromdate.Value = GetEntryDate(GetServerDate())
            dtpTodate.Value = GetEntryDate(GetServerDate())
        Else
            Label2.Visible = False
            dtpfromdate.Visible = False
            dtpTodate.Visible = False
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtCheckedItems_TextChanged(Me, New EventArgs)
        Me.Refresh()
        StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_ACC') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " TRANNO INT"
        StrSql += vbCrLf + " ,TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " ,ACNAME VARCHAR(500)"
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
        StrSql += vbCrLf + " ,SNO VARCHAR(20)"
        StrSql += vbCrLf + " ,COSTNAME VARCHAR(100)"
        StrSql += vbCrLf + " )"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " (TRANNO,TRANDATE,CATNAME,ACNAME,PCS,GRSWT"
        StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT"
        StrSql += vbCrLf + " ,BATCHNO,PAYMODE,CTLID,COMPANYID,RESULT,TYPE,SNO,COSTNAME)"
        StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,C.CATNAME,H.ACNAME"
        StrSql += vbCrLf + " ,SUM(PCS) PCS"
        StrSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
        StrSql += vbCrLf + " ,SUM(NETWT) NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..TRECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
        StrSql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
        StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,T.COMPANYID"
        StrSql += vbCrLf + " ,0"
        StrSql += vbCrLf + " ,REMARK2,T.SNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ACCODE=T.ACCODE)COSTNAME"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TRECEIPT AS T"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = T.CATCODE"
        StrSql += vbCrLf + " WHERE LEN(T.TRANTYPE)>2"
        If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TRFNO='" & txtTranNo.Text & "'"
        If chkWithFromDate.Checked Then StrSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpfromdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTodate.Value, "yyyy-MM-dd") & "'"
        If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
            StrSql += vbCrLf + " AND T.COMPANYID IN"
            StrSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
        End If
        StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,C.CATNAME,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID,T.SNO,T.REMARK2,T.ACCODE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_ACC)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + "    (CATNAME,PCS,GRSWT"
        StrSql += vbCrLf + "    ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT,ORD,RESULT)"
        StrSql += vbCrLf + "    SELECT 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),SUM(AMOUNT),9,2"
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + "    UNION ALL"
        StrSql += vbCrLf + "    SELECT CATNAME + 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),SUM(AMOUNT),NULL,1"
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP_ACC GROUP BY CATNAME"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PCS=NULL WHERE PCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET STNPCS=NULL WHERE STNPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET DIAPCS=NULL WHERE DIAPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PREPCS=NULL WHERE PREPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET GRSWT=NULL WHERE GRSWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET NETWT=NULL WHERE NETWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET DIAWT=NULL WHERE DIAWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET STNWT=NULL WHERE STNWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PREWT=NULL WHERE PREWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET AMOUNT=NULL WHERE AMOUNT=0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " ORDER BY RESULT,ISNULL(ORD,0),TRANDATE,SUBSTRING(BATCHNO,8,10),TRANNO,AMOUNT"
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
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
            .Columns("AMOUNT").Visible = True
            .Columns("ACNAME").Visible = False
            .Columns("TYPE").Visible = True
            .Columns("COSTNAME").Visible = True
            .Columns("SNO").Visible = False
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
        ElseIf e.KeyCode = Keys.Escape And tabMain.SelectedTab.Name = tabView.Name Then
            Button3_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub AccApproval_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AccApproval_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'LOAD COMPANY     
        cnMfgAdminDb = GetAdmindbSoftValue("MFGDBID", "").ToString & "ADMINDB"
        cnMfgStockDb = GetAdmindbSoftValue("MFGDBID", "").ToString & cnStockDb.Replace(cnCompanyId, "")

        MFG_PWD = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MFG_PWD'",, "")

        btnManufacturing.Visible = MFG_RECEIPT
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPNAYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompanyName = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , IIf(strCompanyName.ToString <> "" And SPECIFICFORMAT = 1, strCompanyName.ToString, "ALL"))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv.CellValueChanged
        If CheckState = False Then Exit Sub
        Dim bool As Boolean = CType(IIf(IsDBNull(Dgv.CurrentRow.Cells("APPROVED").Value), False, Dgv.CurrentRow.Cells("APPROVED").Value), Boolean)
        Dim Sno As String = Nothing
        Dim BatNo As String = Nothing
        Dim dv As New DataView
        BatNo = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        If PENDTRF_SNOBASE <> False Then
            Sno = Dgv.CurrentRow.Cells("SNO").Value.ToString
            dv = New DataView
            dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "' AND SNO='" & Sno & "'"
        Else
            dv = New DataView
            dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "'"
        End If
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
        If Val(txtCheckedItems.Text) = 0 Then
            btnTransfer.Enabled = False
        Else
            btnTransfer.Enabled = True
        End If
    End Sub

    Private Function GetBillControlValue(ByVal BillControlId As String, ByVal tran As OleDbTransaction, ByVal CompanyId As String, Optional ByVal Nextiter As Boolean = False) As String
        If strBCostid <> Nothing And Not Nextiter Then
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & CompanyId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran))
        Else
            Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & BillControlId & "' AND COMPANYID = '" & CompanyId & "'", , , tran))
        End If

    End Function

    Private Sub ManufacturingTransfer()
        Dim TranDate As Date = GetEntryDate(GetServerDate)
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "APPROVED = 'TRUE' AND RESULT=0"
        Dim dt As New DataTable
        dt = dv.ToTable
        dt = dt.DefaultView.ToTable(True, New String() {"BATCHNO", "CTLID", "COMPANYID"})
        Dim batchno As String = Nothing
        Dim NewBatchNo As String = Nothing
        Dim CtlId As String = Nothing
        Dim TranNo As Integer = Nothing
        Dim CostId As String = Nothing
        Dim tCompanyId As String = Nothing
        Dim billcontrolid As String = "GEN-SM-INTREC"

        StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        StrSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(StrSql, , , tran)) <> "Y" Then
            billcontrolid = "GEN-STKREFNO"
        End If
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
        If UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran)) = "Y" Then
            billcontrolid = "GEN-SM-REC"
        End If
        CtlId = billcontrolid

        'StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='MFG_ACCODE_" + cnCompanyId.ToString + "' "
        'If UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran).ToString) <> "" Then
        '    MFG_ACCODE = UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran))
        'Else
        '    MFG_ACCODE = MFG_ACCODE
        'End If

        Dim MFG_ACCODE_GET As String = ""
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='MFG_ACCODE_" + strCompanyId.ToString + "' "
        Dim Str_AcCode As String = UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran).ToString)
        If Str_AcCode <> "" Then
            Dim MFG_ACCODEAR() As String
            MFG_ACCODEAR = Str_AcCode.Split(",")
            If MFG_ACCODEAR.Length = 2 Then
                MFG_ACCODE_GET = MFG_ACCODEAR(1)
                MFG_ACCODE = MFG_ACCODEAR(0)
            Else
                MFG_ACCODE = MFG_ACCODE
            End If
        Else
            MFG_ACCODE = MFG_ACCODE
        End If

        If MFG_ACCODE.ToString = "" Then
            MsgBox("Manufacturing account should not be empty")
            Exit Sub
        End If
        StrSql = "SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & MFG_ACCODE & "' "
        If Val(objGPack.GetSqlValue(StrSql,  , "0", tran)) = "0" Then
            MsgBox("Manufacturing account not found")
            Exit Sub
        End If

        Try
            ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
            cn_Sql = New SqlConnection("Data Source=" & ConInfo.lServerName & ";Initial Catalog=" & cnStockDb & ";User ID=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";Password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & "")
            cn_Sql.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Try
            Mfg_Con = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=master;Data Source={0};User Id=" & MFG_USERNAME & ";password=" & MFG_PWD & ";", MFG_SERVERNAME))
            Mfg_Con.Open()
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Dim MfgDB_Accode As String = ""
        StrSql = "SELECT CTLTEXT FROM " & cnMfgAdminDb & "..SOFTCONTROL WHERE CTLID='MFG_ACCODE' "
        MfgDB_Accode = GetSqlValue(Mfg_Con, StrSql)

        Try
            For cnt As Integer = 0 To dt.Rows.Count - 1
                'CtlId = dt.Rows(cnt).Item("CTLID").ToString
                batchno = dt.Rows(cnt).Item("BATCHNO").ToString


                StrSql = " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ACCTRAN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_OUTSTANDING') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSUE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPT') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPTSTONE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT * INTO " & cnStockDb & "..TEMP_APP_ACCTRAN FROM " & cnStockDb & "..TACCTRAN WHERE 1<>1"
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..TEMP_APP_OUTSTANDING FROM " & cnAdminDb & "..TOUTSTANDING WHERE 1<>1"
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSUE FROM " & cnStockDb & "..TISSUE WHERE 1<>1"
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPT FROM " & cnStockDb & "..TRECEIPT WHERE 1<>1"
                StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE 1<>1"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                Dim dtAcctran As New DataTable
                Dim dtOutstanding As New DataTable
                Dim dtIssue As New DataTable
                Dim dtReceipt As New DataTable
                Dim dtReceiptStone As New DataTable
                StrSql = " SELECT * FROM " & cnMfgStockDb & "..ACCTRAN WHERE BATCHNO = '" & batchno & "' AND 1<>1"
                dtAcctran = GetSqlTable(StrSql, Mfg_Con)
                StrSql = " SELECT * FROM " & cnMfgAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & batchno & "' AND 1<>1"
                dtOutstanding = GetSqlTable(StrSql, Mfg_Con)
                StrSql = " SELECT * FROM " & cnMfgStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "' AND 1<>1"
                dtIssue = GetSqlTable(StrSql, Mfg_Con)
                StrSql = " SELECT * FROM " & cnMfgStockDb & "..RECEIPT WHERE BATCHNO = '" & batchno & "' AND 1<>1"
                dtReceipt = GetSqlTable(StrSql, Mfg_Con)
                StrSql = " SELECT * FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE BATCHNO = '" & batchno & "' AND 1<>1"
                dtReceiptStone = GetSqlTable(StrSql, Mfg_Con)

                If MFG_DB Then

                    'StrSql = " SELECT SNO + 'I' SNO,TRANNO,TRANDATE,'RRE' TRANTYPE,PCS,GRSWT,EXTRAWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,WASTPER,WASTAGE,MCGRM,MCHARGE,"
                    'StrSql += vbCrLf + " AMOUNT,RATE,BOARDRATE,SALEMODE,GRSNET,COSTID,COMPANYID,FLAG,PURITY,CATCODE,ACCODE,ALLOY,BATCHNO,REMARK1,'I-R' REMARK2,USERID,UPDATED,UPTIME,STONEUNIT,METALID,TAX,TOUCH"
                    'StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "'"
                    'Cmd = New OleDbCommand(StrSql, Mfg_Con)
                    'Da = New OleDbDataAdapter(Cmd)
                    ''dtReceipt.Rows.Clear()
                    'Da.Fill(dtReceipt)


                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " CONVERT(VARCHAR(15),SNO + 'I') SNO"
                    StrSql += vbCrLf + " ,CONVERT(INT,TRANNO)TRANNO"
                    StrSql += vbCrLf + " ,CONVERT(SMALLDATETIME,TRANDATE)TRANDATE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'RRE') TRANTYPE"
                    StrSql += vbCrLf + " ,CONVERT(INT,PCS)PCS"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),GRSWT)GRSWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),EXTRAWT)EXTRAWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),NETWT)NETWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),LESSWT)LESSWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),PUREWT)PUREWT"
                    StrSql += vbCrLf + " ,CONVERT(INT,ITEMID)ITEMID"
                    StrSql += vbCrLf + " ,CONVERT(INT,SUBITEMID)SUBITEMID"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),WASTPER)WASTPER"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),WASTAGE)WASTAGE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),MCGRM)MCGRM"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),MCHARGE)MCHARGE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),RATE)RATE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),BOARDRATE)BOARDRATE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),SALEMODE)SALEMODE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),GRSNET)GRSNET"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),COSTID)COSTID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),COMPANYID)COMPANYID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),FLAG)FLAG"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURITY)PURITY"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(5),CATCODE)CATCODE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(7),ACCODE)ACCODE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),ALLOY)ALLOY"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),BATCHNO)BATCHNO"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(50),REMARK1)REMARK1"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(50),'I-R') REMARK2"
                    StrSql += vbCrLf + " ,CONVERT(INT,USERID)USERID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),STONEUNIT)STONEUNIT"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),METALID )METALID "
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),TOUCH)TOUCH"
                    StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "'"
                    Cmd = New OleDbCommand(StrSql, Mfg_Con)
                    Da = New OleDbDataAdapter(Cmd)
                    'dtReceipt.Rows.Clear()
                    Da.Fill(dtReceipt)


                    StrSql = " SELECT"
                    StrSql += vbCrLf + " SNO SNO,ISSSNO + 'I' ISSSNO,TRANNO,TRANDATE,TRANTYPE,STNPCS,STNWT,STNRATE,STNAMT,"
                    StrSql += vbCrLf + " STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT,STONEMODE,"
                    StrSql += vbCrLf + " COSTID,COMPANYID,BATCHNO,SYSTEMID,CATCODE"
                    StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSSTONE WHERE BATCHNO = '" & batchno & "'"
                    Cmd = New OleDbCommand(StrSql, Mfg_Con)
                    Da = New OleDbDataAdapter(Cmd)
                    'dtReceiptStone.Rows.Clear()
                    Da.Fill(dtReceiptStone)

                Else


                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " CONVERT(VARCHAR(15),SNO + 'I') SNO"
                    StrSql += vbCrLf + " ,CONVERT(INT,TRANNO)TRANNO"
                    StrSql += vbCrLf + " ,CONVERT(SMALLDATETIME,TRANDATE)TRANDATE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),'RRE') TRANTYPE"
                    StrSql += vbCrLf + " ,CONVERT(INT,PCS)PCS"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),GRSWT)GRSWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),EXTRAWT)EXTRAWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),NETWT)NETWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),LESSWT)LESSWT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),PUREWT)PUREWT"
                    StrSql += vbCrLf + " ,CONVERT(INT,ITEMID)ITEMID"
                    StrSql += vbCrLf + " ,CONVERT(INT,SUBITEMID)SUBITEMID"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),WASTPER)WASTPER"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),WASTAGE)WASTAGE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),MCGRM)MCGRM"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),MCHARGE)MCHARGE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),RATE)RATE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),BOARDRATE)BOARDRATE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),SALEMODE)SALEMODE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),GRSNET)GRSNET"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),COSTID)COSTID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),COMPANYID)COMPANYID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),FLAG)FLAG"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURITY)PURITY"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(5),CATCODE)CATCODE"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(7),ACCODE)ACCODE"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(12,3),ALLOY)ALLOY"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),BATCHNO)BATCHNO"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(50),REMARK1)REMARK1"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(50),'I-R') REMARK2"
                    StrSql += vbCrLf + " ,CONVERT(INT,USERID)USERID"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),STONEUNIT)STONEUNIT"
                    StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),METALID )METALID "
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),TOUCH)TOUCH"
                    StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "'"
                    Cmd = New OleDbCommand(StrSql, Mfg_Con)
                    Da = New OleDbDataAdapter(Cmd)
                    'dtReceipt.Rows.Clear()
                    Da.Fill(dtReceipt)


                    StrSql = " SELECT"
                    StrSql += vbCrLf + " SNO,ISSSNO + 'I' ISSSNO,TRANNO,TRANDATE,'RRE' TRANTYPE,STNPCS,STNWT,STNRATE,STNAMT,"
                    StrSql += vbCrLf + " STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT,STONEMODE,"
                    StrSql += vbCrLf + " COSTID,COMPANYID,BATCHNO,SYSTEMID,CATCODE"
                    StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSSTONE WHERE BATCHNO = '" & batchno & "'"
                    Cmd = New OleDbCommand(StrSql, Mfg_Con)
                    Da = New OleDbDataAdapter(Cmd)
                    'dtReceiptStone.Rows.Clear()
                    Da.Fill(dtReceiptStone)

                End If


                Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                    'Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo.TEMP_APP_ACCTRAN"
                    sqlBulkCopy.WriteToServer(dtAcctran)
                End Using

                Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                    sqlBulkCopy.DestinationTableName = "dbo.TEMP_APP_OUTSTANDING"
                    sqlBulkCopy.WriteToServer(dtOutstanding)
                End Using

                If Not MFG_DB Then
                    Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                        sqlBulkCopy.DestinationTableName = "dbo.TEMP_APP_ISSUE"
                        sqlBulkCopy.WriteToServer(dtIssue)
                    End Using
                End If

                If MFG_DB Then
                    For Each dr As DataRow In dtReceipt.Rows
                        StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TEMP_APP_RECEIPT (SNO, TranNo, TranDate, TRANTYPE, PCS, GRSWT, EXTRAWT, NETWT, LESSWT, PUREWT, ITEMID, SUBITEMID, WASTPER, WASTAGE, MCGRM, MCHARGE, AMOUNT, Rate"
                        StrSql += vbCrLf + " ,BOARDRATE,SALEMODE,GRSNET,COSTID,COMPANYID,FLAG,PURITY,CATCODE,ACCODE,ALLOY,BATCHNO,REMARK1,REMARK2,USERID,STONEUNIT,METALID,TOUCH)"
                        StrSql += vbCrLf + " SELECT"
                        StrSql += vbCrLf + " '" + dr("SNO").ToString + "' SNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANNO").ToString + "' TRANNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANDATE").ToString + "' TRANDATE"
                        StrSql += vbCrLf + " ,'" + dr("TRANTYPE").ToString + "' TRANTYPE"
                        StrSql += vbCrLf + " ,'" + dr("PCS").ToString + "' PCS"
                        StrSql += vbCrLf + " ,'" + dr("GRSWT").ToString + "' GRSWT"
                        StrSql += vbCrLf + " ,'" + dr("EXTRAWT").ToString + "' EXTRAWT"
                        StrSql += vbCrLf + " ,'" + dr("NETWT").ToString + "' NETWT"
                        StrSql += vbCrLf + " ,'" + dr("LESSWT").ToString + "' LESSWT"
                        StrSql += vbCrLf + " ,'" + dr("PUREWT").ToString + "' PUREWT"
                        StrSql += vbCrLf + " ,'" + dr("ITEMID").ToString + "' ITEMID"
                        StrSql += vbCrLf + " ,'" + dr("SUBITEMID").ToString + "' SUBITEMID"
                        StrSql += vbCrLf + " ,'" + dr("WASTPER").ToString + "' WASTPER"
                        StrSql += vbCrLf + " ,'" + dr("WASTAGE").ToString + "' WASTAGE"
                        StrSql += vbCrLf + " ,'" + dr("MCGRM").ToString + "' MCGRM"
                        StrSql += vbCrLf + " ,'" + dr("MCHARGE").ToString + "' MCHARGE"
                        StrSql += vbCrLf + " ,'" + dr("AMOUNT").ToString + "' AMOUNT"
                        StrSql += vbCrLf + " ,'" + dr("RATE").ToString + "' RATE"
                        StrSql += vbCrLf + " ,'" + dr("BOARDRATE").ToString + "' BOARDRATE"
                        StrSql += vbCrLf + " ,'" + dr("SALEMODE").ToString + "' SALEMODE"
                        StrSql += vbCrLf + " ,'" + dr("GRSNET").ToString + "' GRSNET"
                        StrSql += vbCrLf + " ,'" + dr("COSTID").ToString + "' COSTID"
                        StrSql += vbCrLf + " ,'" + dr("COMPANYID").ToString + "' COMPANYID"
                        StrSql += vbCrLf + " ,'" + dr("FLAG").ToString + "' FLAG"
                        StrSql += vbCrLf + " ,'" + dr("PURITY").ToString + "' PURITY"
                        StrSql += vbCrLf + " ,'" + dr("CATCODE").ToString + "' CATCODE"
                        StrSql += vbCrLf + " ,'" + dr("ACCODE").ToString + "' ACCODE"
                        StrSql += vbCrLf + " ,'" + dr("ALLOY").ToString + "' ALLOY"
                        StrSql += vbCrLf + " ,'" + dr("BATCHNO").ToString + "' BATCHNO"
                        StrSql += vbCrLf + " ,'" + dr("REMARK1").ToString + "' REMARK1"
                        StrSql += vbCrLf + " ,'" + dr("REMARK2").ToString + "' REMARK2"
                        StrSql += vbCrLf + " ,'" + dr("USERID").ToString + "' USERID"
                        StrSql += vbCrLf + " ,'" + dr("STONEUNIT").ToString + "' STONEUNIT"
                        StrSql += vbCrLf + " ,'" + dr("METALID").ToString + "' METALID"
                        StrSql += vbCrLf + " ,'" + dr("TOUCH").ToString + "' TOUCH"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    Next

                    For Each dr As DataRow In dtReceiptStone.Rows
                        StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE ("
                        StrSql += vbCrLf + " SNO"
                        StrSql += vbCrLf + " ,ISSSNO"
                        StrSql += vbCrLf + " ,TRANNO"
                        StrSql += vbCrLf + " ,TRANDATE"
                        StrSql += vbCrLf + " ,TRANTYPE"
                        StrSql += vbCrLf + " ,STNPCS"
                        StrSql += vbCrLf + " ,STNWT"
                        StrSql += vbCrLf + " ,STNRATE"
                        StrSql += vbCrLf + " ,STNAMT"
                        StrSql += vbCrLf + " ,STNITEMID"
                        StrSql += vbCrLf + " ,STNSUBITEMID"
                        StrSql += vbCrLf + " ,CALCMODE"
                        StrSql += vbCrLf + " ,STONEUNIT"
                        StrSql += vbCrLf + " ,STONEMODE"
                        StrSql += vbCrLf + " ,COSTID"
                        StrSql += vbCrLf + " ,COMPANYID"
                        StrSql += vbCrLf + " ,BATCHNO"
                        StrSql += vbCrLf + " ,SYSTEMID"
                        StrSql += vbCrLf + " ,CATCODE"
                        StrSql += vbCrLf + " )"

                        StrSql += vbCrLf + " SELECT"
                        StrSql += vbCrLf + " '" + dr("SNO").ToString + "' SNO"
                        StrSql += vbCrLf + " ,'" + dr("ISSSNO").ToString + "' ISSSNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANNO").ToString + "' TRANNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANDATE").ToString + "' TRANDATE"
                        StrSql += vbCrLf + " ,'" + dr("TRANTYPE").ToString + "' TRANTYPE"
                        StrSql += vbCrLf + " ,'" + dr("STNPCS").ToString + "' STNPCS"
                        StrSql += vbCrLf + " ,'" + dr("STNWT").ToString + "' STNWT"
                        StrSql += vbCrLf + " ,'" + dr("STNRATE").ToString + "' STNRATE"
                        StrSql += vbCrLf + " ,'" + dr("STNAMT").ToString + "' STNAMT"
                        StrSql += vbCrLf + " ,'" + dr("STNITEMID").ToString + "' STNITEMID"
                        StrSql += vbCrLf + " ,'" + dr("STNSUBITEMID").ToString + "' STNSUBITEMID"
                        StrSql += vbCrLf + " ,'" + dr("CALCMODE").ToString + "' CALCMODE"
                        StrSql += vbCrLf + " ,'" + dr("STONEUNIT").ToString + "' STONEUNIT"
                        StrSql += vbCrLf + " ,'" + dr("STONEMODE").ToString + "' STONEMODE"
                        StrSql += vbCrLf + " ,'" + dr("COSTID").ToString + "' COSTID"
                        StrSql += vbCrLf + " ,'" + dr("COMPANYID").ToString + "' COMPANYID"
                        StrSql += vbCrLf + " ,'" + dr("BATCHNO").ToString + "' BATCHNO"
                        StrSql += vbCrLf + " ,'" + dr("SYSTEMID").ToString + "' SYSTEMID"
                        StrSql += vbCrLf + " ,'" + dr("CATCODE").ToString + "' CATCODE"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    Next


                Else
                    'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                    '    sqlBulkCopy.DestinationTableName = "dbo.TEMP_APP_RECEIPT"
                    '    sqlBulkCopy.WriteToServer(dtReceipt)
                    'End Using

                    'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                    '    sqlBulkCopy.DestinationTableName = "dbo.TEMP_APP_RECEIPTSTONE"
                    '    sqlBulkCopy.WriteToServer(dtReceiptStone)
                    'End Using


                    For Each dr As DataRow In dtReceipt.Rows
                        StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TEMP_APP_RECEIPT (SNO, TranNo, TranDate, TRANTYPE, PCS, GRSWT, EXTRAWT, NETWT, LESSWT, PUREWT, ITEMID, SUBITEMID, WASTPER, WASTAGE, MCGRM, MCHARGE, AMOUNT, Rate"
                        StrSql += vbCrLf + " ,BOARDRATE,SALEMODE,GRSNET,COSTID,COMPANYID,FLAG,PURITY,CATCODE,ACCODE,ALLOY,BATCHNO,REMARK1,REMARK2,USERID,STONEUNIT,METALID,TOUCH)"
                        StrSql += vbCrLf + " SELECT"
                        StrSql += vbCrLf + " '" + dr("SNO").ToString + "' SNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANNO").ToString + "' TRANNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANDATE").ToString + "' TRANDATE"
                        StrSql += vbCrLf + " ,'" + dr("TRANTYPE").ToString + "' TRANTYPE"
                        StrSql += vbCrLf + " ,'" + dr("PCS").ToString + "' PCS"
                        StrSql += vbCrLf + " ,'" + dr("GRSWT").ToString + "' GRSWT"
                        StrSql += vbCrLf + " ,'" + dr("EXTRAWT").ToString + "' EXTRAWT"
                        StrSql += vbCrLf + " ,'" + dr("NETWT").ToString + "' NETWT"
                        StrSql += vbCrLf + " ,'" + dr("LESSWT").ToString + "' LESSWT"
                        StrSql += vbCrLf + " ,'" + dr("PUREWT").ToString + "' PUREWT"
                        StrSql += vbCrLf + " ,'" + dr("ITEMID").ToString + "' ITEMID"
                        StrSql += vbCrLf + " ,'" + dr("SUBITEMID").ToString + "' SUBITEMID"
                        StrSql += vbCrLf + " ,'" + dr("WASTPER").ToString + "' WASTPER"
                        StrSql += vbCrLf + " ,'" + dr("WASTAGE").ToString + "' WASTAGE"
                        StrSql += vbCrLf + " ,'" + dr("MCGRM").ToString + "' MCGRM"
                        StrSql += vbCrLf + " ,'" + dr("MCHARGE").ToString + "' MCHARGE"
                        StrSql += vbCrLf + " ,'" + dr("AMOUNT").ToString + "' AMOUNT"
                        StrSql += vbCrLf + " ,'" + dr("RATE").ToString + "' RATE"
                        StrSql += vbCrLf + " ,'" + dr("BOARDRATE").ToString + "' BOARDRATE"
                        StrSql += vbCrLf + " ,'" + dr("SALEMODE").ToString + "' SALEMODE"
                        StrSql += vbCrLf + " ,'" + dr("GRSNET").ToString + "' GRSNET"
                        StrSql += vbCrLf + " ,'" + dr("COSTID").ToString + "' COSTID"
                        StrSql += vbCrLf + " ,'" + dr("COMPANYID").ToString + "' COMPANYID"
                        StrSql += vbCrLf + " ,'" + dr("FLAG").ToString + "' FLAG"
                        StrSql += vbCrLf + " ,'" + dr("PURITY").ToString + "' PURITY"
                        StrSql += vbCrLf + " ,'" + dr("CATCODE").ToString + "' CATCODE"
                        StrSql += vbCrLf + " ,'" + dr("ACCODE").ToString + "' ACCODE"
                        StrSql += vbCrLf + " ,'" + dr("ALLOY").ToString + "' ALLOY"
                        StrSql += vbCrLf + " ,'" + dr("BATCHNO").ToString + "' BATCHNO"
                        StrSql += vbCrLf + " ,'" + dr("REMARK1").ToString + "' REMARK1"
                        StrSql += vbCrLf + " ,'" + dr("REMARK2").ToString + "' REMARK2"
                        StrSql += vbCrLf + " ,'" + dr("USERID").ToString + "' USERID"
                        StrSql += vbCrLf + " ,'" + dr("STONEUNIT").ToString + "' STONEUNIT"
                        StrSql += vbCrLf + " ,'" + dr("METALID").ToString + "' METALID"
                        StrSql += vbCrLf + " ,'" + dr("TOUCH").ToString + "' TOUCH"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    Next

                    For Each dr As DataRow In dtReceiptStone.Rows
                        StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE ("
                        StrSql += vbCrLf + " SNO"
                        StrSql += vbCrLf + " ,ISSSNO"
                        StrSql += vbCrLf + " ,TRANNO"
                        StrSql += vbCrLf + " ,TRANDATE"
                        StrSql += vbCrLf + " ,TRANTYPE"
                        StrSql += vbCrLf + " ,STNPCS"
                        StrSql += vbCrLf + " ,STNWT"
                        StrSql += vbCrLf + " ,STNRATE"
                        StrSql += vbCrLf + " ,STNAMT"
                        StrSql += vbCrLf + " ,STNITEMID"
                        StrSql += vbCrLf + " ,STNSUBITEMID"
                        StrSql += vbCrLf + " ,CALCMODE"
                        StrSql += vbCrLf + " ,STONEUNIT"
                        StrSql += vbCrLf + " ,STONEMODE"
                        StrSql += vbCrLf + " ,COSTID"
                        StrSql += vbCrLf + " ,COMPANYID"
                        StrSql += vbCrLf + " ,BATCHNO"
                        StrSql += vbCrLf + " ,SYSTEMID"
                        StrSql += vbCrLf + " ,CATCODE"
                        StrSql += vbCrLf + " )"

                        StrSql += vbCrLf + " SELECT"
                        StrSql += vbCrLf + " '" + dr("SNO").ToString + "' SNO"
                        StrSql += vbCrLf + " ,'" + dr("ISSSNO").ToString + "' ISSSNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANNO").ToString + "' TRANNO"
                        StrSql += vbCrLf + " ,'" + dr("TRANDATE").ToString + "' TRANDATE"
                        StrSql += vbCrLf + " ,'" + dr("TRANTYPE").ToString + "' TRANTYPE"
                        StrSql += vbCrLf + " ,'" + dr("STNPCS").ToString + "' STNPCS"
                        StrSql += vbCrLf + " ,'" + dr("STNWT").ToString + "' STNWT"
                        StrSql += vbCrLf + " ,'" + dr("STNRATE").ToString + "' STNRATE"
                        StrSql += vbCrLf + " ,'" + dr("STNAMT").ToString + "' STNAMT"
                        StrSql += vbCrLf + " ,'" + dr("STNITEMID").ToString + "' STNITEMID"
                        StrSql += vbCrLf + " ,'" + dr("STNSUBITEMID").ToString + "' STNSUBITEMID"
                        StrSql += vbCrLf + " ,'" + dr("CALCMODE").ToString + "' CALCMODE"
                        StrSql += vbCrLf + " ,'" + dr("STONEUNIT").ToString + "' STONEUNIT"
                        StrSql += vbCrLf + " ,'" + dr("STONEMODE").ToString + "' STONEMODE"
                        StrSql += vbCrLf + " ,'" + dr("COSTID").ToString + "' COSTID"
                        StrSql += vbCrLf + " ,'" + dr("COMPANYID").ToString + "' COMPANYID"
                        StrSql += vbCrLf + " ,'" + dr("BATCHNO").ToString + "' BATCHNO"
                        StrSql += vbCrLf + " ,'" + dr("SYSTEMID").ToString + "' SYSTEMID"
                        StrSql += vbCrLf + " ,'" + dr("CATCODE").ToString + "' CATCODE"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    Next

                End If





                Mfg_Tran = Nothing
                If MFG_CENTDB = False Then Mfg_Tran = Mfg_Con.BeginTransaction

                tran = Nothing
                tran = cn.BeginTransaction
                tCompanyId = dt.Rows(cnt).Item("COMPANYID").ToString
                NewBatchNo = GetNewBatchno(cnCostId, IIf(LOCK_MRMI_TRSDATE, TranDate, dtpTransfer.Value), tran)
GenBillNo:
                TranNo = Val(GetBillControlValue(CtlId, tran, tCompanyId))
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                StrSql += vbCrLf + " WHERE CTLID = '" & CtlId & "' AND COMPANYID = '" & tCompanyId & "'"
                StrSql += vbCrLf + " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Not Cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If

                TranNo += 1
                'StrSql = " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ACCTRAN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                'StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_OUTSTANDING') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                'StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSUE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                'StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPT') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                'StrSql += vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPTSTONE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                'Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                'Dim dtAcctran As New DataTable
                'Dim dtOutstanding As New DataTable
                'Dim dtIssue As New DataTable
                'Dim dtReceipt As New DataTable
                'Dim dtReceiptStone As New DataTable

                'StrSql = " SELECT * INTO " & cnStockDb & "..TEMP_APP_ACCTRAN FROM " & cnStockDb & "..TACCTRAN WHERE 1<>1"
                'StrSql += VBCRLF +  " SELECT * INTO " & cnStockDb & "..TEMP_APP_OUTSTANDING FROM " & cnAdminDb & "..TOUTSTANDING WHERE 1<>1"
                'StrSql += VBCRLF +  " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSUE FROM " & cnStockDb & "..TISSUE WHERE 1<>1"
                'StrSql += VBCRLF +  " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPT FROM " & cnStockDb & "..TRECEIPT WHERE 1<>1"
                'StrSql += VBCRLF +  " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE 1<>1"
                'Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                'StrSql = " SELECT * FROM " & cnMfgStockDb & "..ACCTRAN WHERE BATCHNO = '" & batchno & "'"
                'dtAcctran = GetSqlTable(StrSql, Mfg_Con)
                'StrSql = " SELECT * FROM " & cnMfgAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                'dtOutstanding = GetSqlTable(StrSql, Mfg_Con)
                'StrSql = " SELECT * FROM " & cnMfgStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "'"
                'dtIssue = GetSqlTable(StrSql, Mfg_Con)
                'StrSql = " SELECT * FROM " & cnMfgStockDb & "..RECEIPT WHERE BATCHNO = '" & batchno & "'"
                'dtReceipt = GetSqlTable(StrSql, Mfg_Con)
                'StrSql = " SELECT * FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
                'dtReceiptStone = GetSqlTable(StrSql, Mfg_Con)

                'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                '    'Set the database table name.
                '    sqlBulkCopy.DestinationTableName = "TEMP_APP_ACCTRAN"
                '    sqlBulkCopy.WriteToServer(dt)
                'End Using

                'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                '    sqlBulkCopy.DestinationTableName = "TEMP_APP_OUTSTANDING"
                '    SqlBulkCopy.WriteToServer(dt)
                'End Using

                'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                '    sqlBulkCopy.DestinationTableName = "TEMP_APP_ISSUE"
                '    SqlBulkCopy.WriteToServer(dt)
                'End Using

                'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                '    sqlBulkCopy.DestinationTableName = "TEMP_APP_RECEIPT"
                '    SqlBulkCopy.WriteToServer(dt)
                'End Using

                'Using sqlBulkCopy As New SqlBulkCopy(cn_Sql)
                '    sqlBulkCopy.DestinationTableName = "TEMP_APP_RECEIPTSTONE"
                '    SqlBulkCopy.WriteToServer(dt)
                'End Using


                If LOCK_MRMI_TRSDATE = False Then
                    StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += vbCrLf + " SET ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += vbCrLf + " SET ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += vbCrLf + " SET MCGRM='0',MCHARGE='0',AMOUNT='0',ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += vbCrLf + " SET MCGRM='0',MCHARGE='0',AMOUNT='0',ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += vbCrLf + " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                Else
                    StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += vbCrLf + " SET ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += vbCrLf + " SET ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += vbCrLf + " SET MCGRM='0',MCHARGE='0',AMOUNT='0',ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += vbCrLf + " SET MCGRM='0',MCHARGE='0',AMOUNT='0',ACCODE='" & MFG_ACCODE & "',TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += vbCrLf + " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "',COMPANYID='" & strCompanyId & "' WHERE  BATCHNO = '" & batchno & "'"
                End If
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT COSTID FROM "
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += vbCrLf + " UNION"
                StrSql += vbCrLf + " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += vbCrLf + " UNION"
                StrSql += vbCrLf + " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += vbCrLf + " UNION"
                StrSql += vbCrLf + " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += vbCrLf + " )X WHERE ISNULL(COSTID,'') <> ''"
                CostId = objGPack.GetSqlValue(StrSql, "", , tran)

                StrSql = " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                StrSql += vbCrLf + " SET ESTSNO=SNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ACCTRANCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ACCTRAN'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'OUTSTANDINGCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'OUTSTANDING'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPT'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                'StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE SET ISSSNO="
                'StrSql += vbCrLf + " (SELECT TOP 1 SNO FROM " & cnStockDb & "..TEMP_APP_RECEIPT WHERE BATCHNO = '" & NewBatchNo & "')"
                'StrSql += vbCrLf + " WHERE BATCHNO='" & NewBatchNo & "'"



                StrSql = vbCrLf + " UPDATE TS SET BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_RECEIPTSTONE TS," & cnStockDb & "..TEMP_APP_RECEIPT T"
                StrSql += vbCrLf + " WHERE TS.ISSSNO=T.ISSNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


                StrSql = vbCrLf + " UPDATE TS SET ISSSNO=T.SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_RECEIPTSTONE TS," & cnStockDb & "..TEMP_APP_RECEIPT T"
                StrSql += vbCrLf + " WHERE ISSSNO=T.ESTSNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTSTONECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPTSTONE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPTSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_ACCTRAN "
                StrSql += vbCrLf + " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                StrSql += vbCrLf + " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                StrSql += vbCrLf + " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                StrSql += vbCrLf + " SET ESTSNO=SNO,TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                StrSql += vbCrLf + " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                'Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                Dim dtTemp As New DataTable
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ACCTRAN',@MASK_TABLENAME = 'ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_OUTSTANDING',@MASK_TABLENAME = 'OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ISSUE',@MASK_TABLENAME = 'ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPT',@MASK_TABLENAME = 'RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPTSTONE',@MASK_TABLENAME = 'RECEIPTSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                'StrSql = " DELETE FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                'StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                'StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                'StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                'StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                'StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
                'ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)


                If MFG_CENTDB = False Then
                    StrSql = " UPDATE " & cnMfgStockDb & "..ACCTRAN SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgAdminDb & "..OUTSTANDING SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..ISSUE SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPT SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPT SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPTSTONE SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    'ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)
                    Cmd = New OleDbCommand(StrSql, Mfg_Con, Mfg_Tran)
                    Cmd.ExecuteNonQuery()
                End If


                'For k As Integer = 0 To dtReceipt.Rows.Count - 1
                '    StrSql = " INSERT INTO " & cnMfgStockDb & "..ISSUE"
                '    StrSql += VBCRLF +  " ("
                '    StrSql += VBCRLF +  "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                '    StrSql += VBCRLF +  " ,GRSWT,NETWT,LESSWT,PUREWT"
                '    StrSql += VBCRLF +  " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                '    StrSql += VBCRLF +  " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                '    StrSql += VBCRLF +  " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                '    StrSql += VBCRLF +  " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                '    StrSql += VBCRLF +  " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                '    StrSql += VBCRLF +  " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                '    StrSql += VBCRLF +  " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                '    StrSql += VBCRLF +  " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                '    StrSql += VBCRLF +  " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                '    StrSql += VBCRLF +  " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                '    StrSql += VBCRLF +  " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                '    StrSql += VBCRLF +  " ,RESNO"
                '    StrSql += VBCRLF +  " ,SEIVE,BAGNO,RFID,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                '    StrSql += VBCRLF +  " ,STNGRPID )"
                '    StrSql += VBCRLF +  " VALUES("
                '    StrSql += VBCRLF +  " '" & dtReceipt.Rows(k)("SNO").ToString & "'" ''SNO
                '    StrSql += VBCRLF +  " ," & dtReceipt.Rows(k)("TRANNO").ToString & "" 'TRANNO
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("TRANDATE").ToString & "'" 'TRANDATE
                '    StrSql += VBCRLF +  " ,'IIS'" 'TRANTYPE
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("PCS").ToString) & "" 'PCS
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("GRSWT").ToString) & "" 'GRSWT
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("NETWT").ToString) & "" 'NETWT
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("LESSWT").ToString) & "" 'LESSWT
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("PUREWT").ToString) & "" 'PUREWT
                '    StrSql += VBCRLF +  " ,''" 'TAGNO
                '    StrSql += VBCRLF +  " ,0" 'ITEMID
                '    StrSql += VBCRLF +  " ,0" 'SUBITEMID
                '    StrSql += VBCRLF +  " ," & dtReceipt.Rows(k)("WASTPER").ToString & "" 'WASTPER
                '    StrSql += VBCRLF +  " ," & dtReceipt.Rows(k)("WASTAGE").ToString & "" 'WASTAGE
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("MCGRM").ToString) & "" 'MCGRM
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("MCHARGE").ToString) & "" 'MCHARGE
                '    StrSql += VBCRLF +  " ," & Val(0) & "" 'AMOUNT
                '    StrSql += VBCRLF +  " ," & Val(0) & "" 'RATE
                '    StrSql += VBCRLF +  " ," & Val(0) & "" 'BOARDRATE
                '    StrSql += VBCRLF +  " ,''" 'SALEMODE
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("GRSNET").ToString & "'" 'GRSNET
                '    StrSql += VBCRLF +  " ,''" 'TRANSTATUS ''
                '    StrSql += VBCRLF +  " ,NULL" 'REFNO ''
                '    StrSql += VBCRLF +  " ,NULL" 'REFDATE NULL
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("COSTID").ToString & "'" 'COSTID 
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("COMPANYID").ToString & "'" 'COMPANYID
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("TYPE").ToString & "'" 'FLAG
                '    StrSql += VBCRLF +  " ,0" 'EMPID
                '    StrSql += VBCRLF +  " ,0" 'TAGGRSWT
                '    StrSql += VBCRLF +  " ,0" 'TAGNETWT
                '    StrSql += VBCRLF +  " ,0" 'TAGRATEID
                '    StrSql += VBCRLF +  " ,0" 'TAGSVALUE
                '    StrSql += VBCRLF +  " ,''" 'TAGDESIGNER  
                '    StrSql += VBCRLF +  " ,0" 'ITEMCTRID
                '    StrSql += VBCRLF +  " ,0" 'ITEMTYPEID
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("PURITY").ToString) & "" 'PURITY
                '    StrSql += VBCRLF +  " ,''" 'TABLECODE
                '    StrSql += VBCRLF +  " ,''" 'INCENTIVE
                '    StrSql += VBCRLF +  " ,''" 'WEIGHTUNIT
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("CATCODE").ToString & "'" 'CATCODE
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("OCATCODE").ToString & "'" 'OCATCODE
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("ACCODE").ToString & "'" 'ACCODE
                '    StrSql += VBCRLF +  " ,0" 'ALLOY
                '    StrSql += VBCRLF +  " ,'" & batchno & "'" 'BATCHNO
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("REMARK1").ToString & "'" 'REMARK1
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("REMARK2").ToString & "'" 'REMARK2
                '    StrSql += VBCRLF +  " ,'" & userId & "'" 'USERID
                '    StrSql += VBCRLF +  " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                '    StrSql += VBCRLF +  " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                '    StrSql += VBCRLF +  " ,'" & systemId & "'" 'SYSTEMID
                '    StrSql += VBCRLF +  " ,'0'" 'DISCOUNT
                '    StrSql += VBCRLF +  " ,''" 'RUNNO
                '    StrSql += VBCRLF +  " ,'0'" 'CASHID
                '    StrSql += VBCRLF +  " ," & Val(dtReceipt.Rows(k)("TAX").ToString) & "" 'TAX
                '    StrSql += VBCRLF +  " ," & Val(0) & "" 'TDS
                '    StrSql += VBCRLF +  " ,0" 'STNAMT
                '    StrSql += VBCRLF +  " ,0" 'MISCAMT
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("METALID").ToString & "'" 'METALID
                '    StrSql += VBCRLF +  " ,'" & dtReceipt.Rows(k)("STONEUNIT").ToString & "'" 'STONEUNIT
                '    StrSql += VBCRLF +  " ,'" & VERSION & "'" 'APPVER
                '    StrSql += VBCRLF +  " ,'" & Val(dtReceipt.Rows(k)("TOUCH").ToString) & "'" 'APPVER
                '    StrSql += VBCRLF +  " ,0" 'ORDSTATE_ID
                '    StrSql += VBCRLF +  " ,NULL'" 'RESNO
                '    StrSql += VBCRLF +  " ,NULL" 'SEIVE
                '    StrSql += VBCRLF +  " ,NULL" 'BAGNO
                '    StrSql += VBCRLF +  " ,NULL" 'RFID
                '    StrSql += VBCRLF +  " ,NULL" 'CUTID
                '    StrSql += VBCRLF +  " ,NULL" 'COLORID
                '    StrSql += VBCRLF +  " ,NULL" 'CLARITYID
                '    StrSql += VBCRLF +  " ,NULL" 'SHAPEID
                '    StrSql += VBCRLF +  " ,NULL" 'SETTYPEID
                '    StrSql += VBCRLF +  " ,NULL" 'HEIGHT
                '    StrSql += VBCRLF +  " ,NULL" 'WIDTH
                '    StrSql += VBCRLF +  " ,NULL" 'STNGRPID
                '    StrSql += VBCRLF +  " )"
                '    Cmd = New OleDbCommand(StrSql, Mfg_Con)
                '    Cmd.ExecuteNonQuery()
                'Next

                'If False Then
                '    For k As Integer = 0 To dtReceipt.Rows.Count - 1
                '        StrSql = " INSERT INTO " & cnMfgStockDb & "..ISSUE"
                '        StrSql += vbCrLf + " ("
                '        StrSql += vbCrLf + "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                '        StrSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT"
                '        StrSql += vbCrLf + " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                '        StrSql += vbCrLf + " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                '        StrSql += vbCrLf + " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                '        StrSql += vbCrLf + " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                '        StrSql += vbCrLf + " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                '        StrSql += vbCrLf + " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                '        StrSql += vbCrLf + " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                '        StrSql += vbCrLf + " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                '        StrSql += vbCrLf + " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                '        StrSql += vbCrLf + " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                '        StrSql += vbCrLf + " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                '        StrSql += vbCrLf + " ,RESNO"
                '        StrSql += vbCrLf + " ,SEIVE,BAGNO,RFID,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                '        StrSql += vbCrLf + " ,STNGRPID )"
                '        StrSql += vbCrLf + " VALUES("
                '        StrSql += vbCrLf + " '" & dtReceipt.Rows(k)("SNO").ToString & "T'" ''SNO
                '        StrSql += vbCrLf + " ," & dtReceipt.Rows(k)("TRANNO").ToString & "" 'TRANNO
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("TRANDATE").ToString & "'" 'TRANDATE
                '        StrSql += vbCrLf + " ,'IIS'" 'TRANTYPE
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("PCS").ToString) & "" 'PCS
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("GRSWT").ToString) & "" 'GRSWT
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("NETWT").ToString) & "" 'NETWT
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("LESSWT").ToString) & "" 'LESSWT
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("PUREWT").ToString) & "" 'PUREWT
                '        StrSql += vbCrLf + " ,''" 'TAGNO
                '        StrSql += vbCrLf + " ,0" 'ITEMID
                '        StrSql += vbCrLf + " ,0" 'SUBITEMID
                '        StrSql += vbCrLf + " ," & dtReceipt.Rows(k)("WASTPER").ToString & "" 'WASTPER
                '        StrSql += vbCrLf + " ," & dtReceipt.Rows(k)("WASTAGE").ToString & "" 'WASTAGE
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'MCGRM
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'MCHARGE
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'AMOUNT
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'RATE
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'BOARDRATE
                '        StrSql += vbCrLf + " ,''" 'SALEMODE
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("GRSNET").ToString & "'" 'GRSNET
                '        StrSql += vbCrLf + " ,''" 'TRANSTATUS ''
                '        StrSql += vbCrLf + " ,NULL" 'REFNO ''
                '        StrSql += vbCrLf + " ,NULL" 'REFDATE NULL
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("COSTID").ToString & "'" 'COSTID 
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("COMPANYID").ToString & "'" 'COMPANYID
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("FLAG").ToString & "'" 'FLAG
                '        StrSql += vbCrLf + " ,0" 'EMPID
                '        StrSql += vbCrLf + " ,0" 'TAGGRSWT
                '        StrSql += vbCrLf + " ,0" 'TAGNETWT
                '        StrSql += vbCrLf + " ,0" 'TAGRATEID
                '        StrSql += vbCrLf + " ,0" 'TAGSVALUE
                '        StrSql += vbCrLf + " ,''" 'TAGDESIGNER  
                '        StrSql += vbCrLf + " ,0" 'ITEMCTRID
                '        StrSql += vbCrLf + " ,0" 'ITEMTYPEID
                '        StrSql += vbCrLf + " ," & Val(dtReceipt.Rows(k)("PURITY").ToString) & "" 'PURITY
                '        StrSql += vbCrLf + " ,''" 'TABLECODE
                '        StrSql += vbCrLf + " ,''" 'INCENTIVE
                '        StrSql += vbCrLf + " ,''" 'WEIGHTUNIT
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("CATCODE").ToString & "'" 'CATCODE
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("OCATCODE").ToString & "'" 'OCATCODE
                '        StrSql += vbCrLf + " ,'" & MfgDB_Accode & "'" 'ACCODE
                '        StrSql += vbCrLf + " ,0" 'ALLOY
                '        StrSql += vbCrLf + " ,'" & batchno & "'" 'BATCHNO
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("REMARK1").ToString & "'" 'REMARK1
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("REMARK2").ToString & "'" 'REMARK2
                '        StrSql += vbCrLf + " ,'" & userId & "'" 'USERID
                '        StrSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                '        StrSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                '        StrSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                '        StrSql += vbCrLf + " ,'0'" 'DISCOUNT
                '        StrSql += vbCrLf + " ,''" 'RUNNO
                '        StrSql += vbCrLf + " ,'0'" 'CASHID
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'TAX
                '        StrSql += vbCrLf + " ," & Val(0) & "" 'TDS
                '        StrSql += vbCrLf + " ,0" 'STNAMT
                '        StrSql += vbCrLf + " ,0" 'MISCAMT
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("METALID").ToString & "'" 'METALID
                '        StrSql += vbCrLf + " ,'" & dtReceipt.Rows(k)("STONEUNIT").ToString & "'" 'STONEUNIT
                '        StrSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                '        StrSql += vbCrLf + " ,'" & Val(dtReceipt.Rows(k)("TOUCH").ToString) & "'" 'APPVER
                '        StrSql += vbCrLf + " ,0" 'ORDSTATE_ID
                '        StrSql += vbCrLf + " ,NULL" 'RESNO
                '        StrSql += vbCrLf + " ,NULL" 'SEIVE
                '        StrSql += vbCrLf + " ,NULL" 'BAGNO
                '        StrSql += vbCrLf + " ,NULL" 'RFID
                '        StrSql += vbCrLf + " ,NULL" 'CUTID
                '        StrSql += vbCrLf + " ,NULL" 'COLORID
                '        StrSql += vbCrLf + " ,NULL" 'CLARITYID
                '        StrSql += vbCrLf + " ,NULL" 'SHAPEID
                '        StrSql += vbCrLf + " ,NULL" 'SETTYPEID
                '        StrSql += vbCrLf + " ,NULL" 'HEIGHT
                '        StrSql += vbCrLf + " ,NULL" 'WIDTH
                '        StrSql += vbCrLf + " ,NULL" 'STNGRPID
                '        StrSql += vbCrLf + " )"
                '        Cmd = New OleDbCommand(StrSql, Mfg_Con, Mfg_Tran)
                '        Cmd.ExecuteNonQuery()

                '        For j As Integer = 0 To dtReceiptStone.Rows.Count - 1
                '            If dtReceipt.Rows(k)("SNO").ToString <> dtReceiptStone.Rows(j)("ISSSNO").ToString Then Continue For
                '            StrSql = " INSERT INTO " & cnMfgStockDb & "..ISSSTONE"
                '            StrSql += vbCrLf + " ("
                '            StrSql += vbCrLf + " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
                '            StrSql += vbCrLf + " ,STNPCS,STNWT,STNRATE,STNAMT"
                '            StrSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
                '            StrSql += vbCrLf + " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
                '            StrSql += vbCrLf + " ,BATCHNO,SYSTEMID,CATCODE,TAX,APPVER,DISCOUNT"
                '            'If OMaterialType = MaterialType.Receipt Then StrSql += VBCRLF +  " ,JOBISNO"
                '            StrSql += vbCrLf + " ,JOBISNO"
                '            StrSql += ",OCATCODE,SEIVE,STUDDEDUCT"
                '            StrSql += vbCrLf + " )"
                '            StrSql += vbCrLf + " VALUES"
                '            StrSql += vbCrLf + " ("
                '            StrSql += vbCrLf + " '" & dtReceiptStone.Rows(j)("SNO").ToString & "T'" ''SNO
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("ISSSNO").ToString & "T'" 'ISSSNO
                '            StrSql += vbCrLf + " ," & dtReceiptStone.Rows(j)("TRANNO").ToString & "" 'TRANNO
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("TRANDATE").ToString & "'" 'TRANDATE
                '            StrSql += vbCrLf + " ,'IIS'" 'TRANTYPE
                '            StrSql += vbCrLf + " ," & Val(dtReceiptStone.Rows(j)("STNPCS").ToString) & "" 'STNPCS
                '            StrSql += vbCrLf + " ," & Val(dtReceiptStone.Rows(j)("STNWT").ToString) & "" 'STNWT
                '            'StrSql += vbCrLf + " ," & Val(dtReceiptStone.Rows(j)("STNRATE").ToString) & "" 'STNRATE
                '            StrSql += vbCrLf + " ," & Val(0) & "" 'STNRATE
                '            StrSql += vbCrLf + " ,NULL" 'STNAMT
                '            'StrSql += VBCRLF +  " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'STNAMT
                '            StrSql += vbCrLf + " ," & dtReceiptStone.Rows(j)("STNITEMID").ToString & "" 'STNITEMID
                '            StrSql += vbCrLf + " ," & dtReceiptStone.Rows(j)("STNSUBITEMID").ToString & "" 'STNSUBITEMID
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("CALCMODE").ToString & "'" 'CALCMODE
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("STONEUNIT").ToString & "'" 'STONEUNIT
                '            StrSql += vbCrLf + " ,''" 'STONEMODE 
                '            StrSql += vbCrLf + " ,''" 'TRANSTATUS
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("COSTID").ToString & "'" 'COSTID
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("COMPANYID").ToString & "'" 'COMPANYID
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("BATCHNO").ToString & "'" 'BATCHNO
                '            StrSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("CATCODE").ToString & "'" 'CATCODE
                '            'StrSql += vbCrLf + " ," & dtReceiptStone.Rows(j)("TAX").ToString & "" 'TAX
                '            StrSql += vbCrLf + " ," & Val(0) & "" 'TAX
                '            StrSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                '            StrSql += vbCrLf + " ,0" 'DISCOUNT
                '            StrSql += vbCrLf + " ,NULL" 'JOBISNO
                '            StrSql += vbCrLf + " ,'" & dtReceiptStone.Rows(j)("OCATCODE").ToString & "'" 'OCATCODE
                '            StrSql += vbCrLf + " ,NULL" 'SEIVE
                '            StrSql += vbCrLf + " ,''" 'STUDDEDUCT
                '            StrSql += vbCrLf + " )"
                '            Cmd = New OleDbCommand(StrSql, Mfg_Con, Mfg_Tran)
                '            Cmd.ExecuteNonQuery()
                '        Next
                '    Next
                'End If


                StrSql = " DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += vbCrLf + " DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += vbCrLf + " DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += vbCrLf + " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += vbCrLf + " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()

                tran.Commit()
                tran = Nothing

                If MFG_CENTDB Then
                    StrSql = " UPDATE " & cnMfgStockDb & "..ACCTRAN SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgAdminDb & "..OUTSTANDING SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..ISSUE SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPT SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPT SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    StrSql += vbCrLf + " UPDATE " & cnMfgStockDb & "..RECEIPTSTONE SET TRANSFERED='Y' WHERE BATCHNO = '" & batchno & "'"
                    'ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)
                    Cmd = New OleDbCommand(StrSql, Mfg_Con, Mfg_Tran)
                    Cmd.ExecuteNonQuery()
                End If

                If MFG_CENTDB = False Then Mfg_Tran.Commit()
                Mfg_Tran = Nothing
            Next

            MsgBox("Receipt Transfer Completed", MsgBoxStyle.Information)
            Dim Pbatchno As String = NewBatchNo
            Dim Pbilldate As Date
            If LOCK_MRMI_TRSDATE = False Then
                Pbilldate = dtpTransfer.Value
            Else
                Pbilldate = TranDate
            End If
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":RIN")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & Pbatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":RIN" & ";" &
                    LSet("BATCHNO", 15) & ":" & Pbatchno & ";" &
                    LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" &
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            If Mfg_Tran IsNot Nothing Then Mfg_Tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Manufacturing Then ManufacturingTransfer() : Exit Sub
        Dim TranDate As Date = GetEntryDate(GetServerDate)
        Dim dv As New DataView
        dv = CType(Dgv.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "APPROVED = 'TRUE' AND RESULT=0"
        Dim dt As New DataTable
        dt = dv.ToTable
        dt = dt.DefaultView.ToTable(True, New String() {"BATCHNO", "CTLID", "COMPANYID"})
        Dim batchno As String = Nothing
        Dim NewBatchNo As String = Nothing
        Dim CtlId As String = Nothing
        Dim TranNo As Integer = Nothing
        Dim CostId As String = Nothing
        Dim tCompanyId As String = Nothing
        Dim billcontrolid As String = "GEN-SM-INTREC"
        StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then StrSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(StrSql, , , tran)) <> "Y" Then
            billcontrolid = "GEN-STKREFNO"
        End If
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
        If UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran)) = "Y" Then
            billcontrolid = "GEN-SM-REC"
        End If
        CtlId = billcontrolid
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For cnt As Integer = 0 To dt.Rows.Count - 1
                'CtlId = dt.Rows(cnt).Item("CTLID").ToString
                batchno = dt.Rows(cnt).Item("BATCHNO").ToString
                tCompanyId = dt.Rows(cnt).Item("COMPANYID").ToString
                NewBatchNo = GetNewBatchno(cnCostId, IIf(LOCK_MRMI_TRSDATE, TranDate, dtpTransfer.Value), tran)
GenBillNo:
                TranNo = Val(GetBillControlValue(CtlId, tran, tCompanyId))
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                StrSql += " WHERE CTLID = '" & CtlId & "' AND COMPANYID = '" & tCompanyId & "'"
                StrSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Not Cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If
                TranNo += 1
                StrSql = " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ACCTRAN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_OUTSTANDING') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSUE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_ISSSTONE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_ISSSTONE"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPT') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " IF OBJECT_ID('" & cnStockDb & "..TEMP_APP_RECEIPTSTONE') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT * INTO " & cnStockDb & "..TEMP_APP_ACCTRAN FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_OUTSTANDING FROM " & cnAdminDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSUE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                '' newly added on 18-MAY-2022 for vbj
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_ISSSTONE FROM " & cnStockDb & "..TISSSTONE WHERE BATCHNO = '" & batchno & "'"

                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPT FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += " SELECT * INTO " & cnStockDb & "..TEMP_APP_RECEIPTSTONE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"

                If LOCK_MRMI_TRSDATE = False Then
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    '' newly added on 18-MAY-2022 for vbj
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',RESNO =SNO,TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSSTONE "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    '' End newly added on 18-MAY-2022 for vbj
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += " SET TRANDATE = '" & Format(dtpTransfer.Value, "yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                Else
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ACCTRAN "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_OUTSTANDING "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    '' newly added on 18-MAY-2022 for vbj
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSUE "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',RESNO =SNO,TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_ISSSTONE "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    '' End newly added on 18-MAY-2022 for vbj
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPT "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',USERID=" & userId & ",COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                    StrSql += " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE "
                    StrSql += " SET TRANDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANNO = " & TranNo & ",BATCHNO='" & NewBatchNo & "',APPVER='" & VERSION & "',COSTID='" & cnCostId & "' WHERE  BATCHNO = '" & batchno & "'"
                End If
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                StrSql = " SELECT COSTID FROM "
                StrSql += " ("
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " UNION"
                StrSql += " SELECT TOP 1 COSTID FROM " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " )X WHERE ISNULL(COSTID,'') <> ''"
                CostId = objGPack.GetSqlValue(StrSql, "", , tran)
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ACCTRANCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ACCTRAN'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'OUTSTANDINGCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'OUTSTANDING'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTCODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPT'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                'StrSql = vbCrLf + " UPDATE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE SET ISSSNO="
                'StrSql += vbCrLf + " (SELECT TOP 1 SNO FROM " & cnStockDb & "..TEMP_APP_RECEIPT WHERE BATCHNO = '" & NewBatchNo & "')"
                'StrSql += vbCrLf + " WHERE BATCHNO='" & NewBatchNo & "'"

                StrSql = vbCrLf + " UPDATE TS SET ISSSNO=T.SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_RECEIPTSTONE TS," & cnStockDb & "..TEMP_APP_RECEIPT T"
                StrSql += vbCrLf + " WHERE T.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.ISSSNO=T.ISSNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                '' newly added on 18-MAY-2022 for vbj
                StrSql = vbCrLf + " UPDATE TS SET ISSSNO=T.SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_ISSSTONE TS," & cnStockDb & "..TEMP_APP_ISSUE T"
                StrSql += vbCrLf + " WHERE T.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.ISSSNO=T.RESNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " UPDATE TS SET ISSNO=T.SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_RECEIPT TS," & cnStockDb & "..TEMP_APP_ISSUE T"
                StrSql += vbCrLf + " WHERE T.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.BATCHNO='" & NewBatchNo & "'"
                StrSql += vbCrLf + " AND TS.ISSNO=T.RESNO"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                StrSql = vbCrLf + " UPDATE T SET T.RESNO=''"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..TEMP_APP_ISSUE T"
                StrSql += vbCrLf + " WHERE T.BATCHNO='" & NewBatchNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                '' END newly added on 18-MAY-2022 for vbj

                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'RECEIPTSTONECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPTSTONE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_RECEIPTSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                '' newly added on 18-MAY-2022 for vbj
                StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
                StrSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
                StrSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
                StrSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
                StrSql += vbCrLf + " ,@UPD_DBNAME = '" & cnStockDb & "'"
                StrSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_APP_ISSSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                '' END newly added on 18-MAY-2022 for vbj


                Dim dtTemp1 As New DataTable
                StrSql = " SELECT * FROM " & cnStockDb & "..TEMP_APP_ACCTRAN"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp1 = New DataTable : Da.Fill(dtTemp1)


                Dim dtTemp As New DataTable
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ACCTRAN',@MASK_TABLENAME = 'ACCTRAN'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_OUTSTANDING',@MASK_TABLENAME = 'OUTSTANDING'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ISSUE',@MASK_TABLENAME = 'ISSUE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next

                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_ISSSTONE',@MASK_TABLENAME = 'ISSSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next

                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPT',@MASK_TABLENAME = 'RECEIPT'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next
                StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'TEMP_APP_RECEIPTSTONE',@MASK_TABLENAME = 'RECEIPTSTONE'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd) : dtTemp = New DataTable : Da.Fill(dtTemp)
                For Each ro As DataRow In dtTemp.Rows
                    ExecQuery(SyncMode.Transaction, ro.Item(0).ToString, cn, tran, CostId)
                Next

                Dim STAccode As String = objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO='" & batchno.Trim & "'", , , tran).ToString
                Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & STAccode.Trim & "'", , , tran).ToString)
                If GST And StateId <> CompanyStateId Then
                    Dim dttaxtran As DataTable = Nothing
                    StrSql = "SELECT * FROM " & cnStockDb & "..TEMP_APP_RECEIPT WHERE BATCHNO='" & NewBatchNo & "'"
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    Da = New OleDbDataAdapter(Cmd) : dttaxtran = New DataTable : Da.Fill(dttaxtran)
                    If dttaxtran.Rows.Count > 0 Then
                        For count As Integer = 0 To dttaxtran.Rows.Count - 1
                            Dim grsamt As Double = Val(dttaxtran.Rows(count).Item("AMOUNT").ToString)
                            If Val(grsamt) > 0 Then
                                Dim SGST As Double = Nothing
                                Dim CGST As Double = Nothing
                                Dim IGST As Double = Nothing
                                Dim SGSTPER As Decimal
                                Dim CGSTPER As Decimal
                                Dim IGSTPER As Decimal
                                StrSql = "SELECT S_SGSTTAX,S_CGSTTAX,S_IGSTTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE='" & dttaxtran.Rows(count).Item("CATCODE").ToString & "' "
                                Dim dr As DataRow = GetSqlRow(StrSql, cn, tran)
                                If Not dr Is Nothing Then
                                    SGSTPER = Val(dr.Item("S_SGSTTAX").ToString)
                                    CGSTPER = Val(dr.Item("S_CGSTTAX").ToString)
                                    IGSTPER = Val(dr.Item("S_IGSTTAX").ToString)
                                    SGST = 0 : CGST = 0 : IGST = 0
                                    If SGSTPER > 0 Or CGSTPER > 0 Or IGSTPER > 0 Then
                                        grsamt = grsamt
                                        If StateId = CompanyStateId Then
                                            'SGST = Math.Round(Val(gstAmt) * SGSTPER / 100, 2)
                                            'SGST = CalcRoundoffAmt(SGST, objSoftKeys.RoundOff_Vat)
                                            'CGST = Math.Round(Val(gstAmt) * CGSTPER / 100, 2)
                                            'CGST = CalcRoundoffAmt(CGST, objSoftKeys.RoundOff_Vat)
                                        Else
                                            IGST = Math.Round((Val(grsamt) * IGSTPER) / 100, 2)
                                            IGST = CalcRoundoffAmt(IGST, "N")
                                        End If

                                    Else
                                        grsamt = grsamt
                                        SGST = 0
                                        CGST = 0
                                        IGST = 0
                                    End If
                                End If
                                grsamt = Math.Round(Val(grsamt), 2)
                                If Val(IGST) > 0 Then
                                    StrSql = "INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ISSSNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID"
                                    StrSql += " ,COMPANYID,STUDDED)VALUES("
                                    StrSql += " '" & GetNewSnoNew(TranSnoType.TAXTRANCODE, cn, tran) & "'" 'SNO
                                    StrSql += " ,'" & dttaxtran.Rows(count).Item("SNO").ToString & "'" 'ISSSNO
                                    StrSql += " ,''" 'ACCODE
                                    StrSql += " ,''" 'CONTRA
                                    StrSql += " ," & Val(dttaxtran.Rows(count).Item("TRANNO").ToString) & "" 'TRANNO
                                    StrSql += " ,'" & dttaxtran.Rows(count).Item("TRANDATE").ToString & "'" 'TRANDATE
                                    StrSql += " ,'RIN'" 'TRANTYPE
                                    StrSql += " ,'" & NewBatchNo & "'" 'BATCHNO
                                    StrSql += " ,'IG'" 'TAXID
                                    StrSql += " ,'" & grsamt & "'" 'AMOUNT
                                    StrSql += " ,'" & IGSTPER & "'" 'TAXPER
                                    StrSql += " ,'" & IGST & "'" 'TAXAMOUNT
                                    StrSql += " ,''"  'TAXTYPE
                                    StrSql += " ,3" 'TSNO
                                    StrSql += " ,'" & dttaxtran.Rows(count).Item("COSTID").ToString & "'" 'COSTID
                                    StrSql += " ,'" & dttaxtran.Rows(count).Item("COMPANYID").ToString & "'" 'COMPANYID
                                    StrSql += " ,'N'"  'STUDDED
                                    StrSql += " )"
                                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                                End If
                            End If
                        Next
                    End If
                End If


                StrSql = " DELETE FROM " & cnStockDb & "..TACCTRAN WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TOUTSTANDING WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSUE WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPT WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TRECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TISSSTONE WHERE BATCHNO = '" & batchno & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostId)

                StrSql = " DROP TABLE " & cnStockDb & "..TEMP_APP_ACCTRAN"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_OUTSTANDING"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_ISSUE"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPT"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_RECEIPTSTONE"
                StrSql += " DROP TABLE " & cnStockDb & "..TEMP_APP_ISSSTONE"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Receipt Transfer Completed", MsgBoxStyle.Information)
            Dim Pbatchno As String = NewBatchNo
            Dim Pbilldate As Date
            If LOCK_MRMI_TRSDATE = False Then
                Pbilldate = dtpTransfer.Value
            Else
                Pbilldate = TranDate
            End If
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":RIN")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & Pbatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":RIN" & ";" &
                    LSet("BATCHNO", 15) & ":" & Pbatchno & ";" &
                    LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" &
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

#Region "TabPage2"
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        StrSql = "SELECT  TRFNO,PCS,GRSWT,NETWT,REMARK1,REMARK2,"
        StrSql += vbCrLf + " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ACCODE=R.ACCODE)COSTNAME   "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R WHERE 1 = 1 "
        StrSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom2.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo2.Value.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND ISNULL(TRFNO,'')<>'' AND TRANTYPE='RIN'"
        StrSql += vbCrLf + " AND TRFNO IN("
        StrSql += vbCrLf + " SELECT DISTINCT TRFNO FROM " & cnStockDb & "..ISSUE WHERE ISNULL(TRFNO,'')<>'' "
        StrSql += vbCrLf + " AND ISNULL(MELT_RETAG,'')='' AND TRANTYPE<>'IIN' "
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT DISTINCT TRFNO FROM " & cnStockDb & "..RECEIPT WHERE 1 = 1 "
        StrSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom2.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo2.Value.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  ISNULL(TRFNO,'')<>'' "
        StrSql += vbCrLf + " AND ISNULL(MELT_RETAG,'')='' AND TRANTYPE<>'RIN'"
        StrSql += vbCrLf + " ) ORDER BY R.TRANDATE"
        Da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        Da.Fill(dt)
        If dt.Rows.Count > 1 Then
            With gridview
                .DataSource = dt
                For CNT As Integer = 0 To .Columns.Count - 1
                    .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
            End With
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        GridviewStyle()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        gridViewHead.DataSource = Nothing
        gridview.DataSource = Nothing
        tabView.Show()
        tabMain.SelectedTab = tabView
        dtpFrom2.Focus()
    End Sub
    Private Sub gridview_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridview.Scroll
        'Try
        '    If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
        '        gridViewHead.HorizontalScrollingOffset = e.NewValue
        '        gridViewHead.Columns("SCROLL").Visible = CType(gridview.Controls(1), VScrollBar).Visible
        '        gridViewHead.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Information)
        'End Try
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridview.RowCount > 0 Then
            'BrightPosting.GExport.Post(Me.Name, strCompanyName, "CORPORATE PENDING TRANSFERVIEW", gridview, BrightPosting.GExport.GExportType.Print, gridViewHead)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CORPORATE PENDING TRANSFERVIEW", gridview, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridview.Rows.Count > 0 Then
            'BrightPosting.GExport.Post(Me.Name, strCompanyName, "CORPORATE PENDING TRANSFERVIEW", gridview, BrightPosting.GExport.GExportType.Export, gridViewHead)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CORPORATE PENDING TRANSFERVIEW", gridview, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        tabMain.SelectedTab = tabGen
    End Sub
#End Region
#Region "GridView Sytle"
    Private Sub GridviewStyle()
        With gridview
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub
    'Gridview Temporary Invisible
    Private Sub GridviewHeader()
        Dim dtMergeHeader As New DataTable("~MergeTable")
        With dtMergeHeader
            .Columns.Add("TRFNO", GetType(String))
            .Columns.Add("PCS~GRSWT~NETWT", GetType(String))
            .Columns.Add("REMARK1~REMARK2", GetType(String))
            .Columns.Add("COSTNAME", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("TRFNO").Caption = "TRFNO"
            .Columns("PCS~GRSWT~NETWT").Caption = "STOCK"
            .Columns("REMARK1~REMARK2").Caption = "REMARKS"
            .Columns("COSTNAME").Caption = "COSTNAME"
        End With
        With gridViewHead
            .DataSource = dtMergeHeader
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
            .Columns("TRFNO").Width = gridview.Columns("TRFNO").Width
            .Columns("PCS~GRSWT~NETWT").Width = gridview.Columns("PCS").Width + gridview.Columns("GRSWT").Width + gridview.Columns("NETWT").Width
            .Columns("REMARK1~REMARK2").Width = gridview.Columns("REMARK1").Width + gridview.Columns("REMARK2").Width
            .Columns("COSTNAME").Width = gridview.Columns("COSTNAME").Width
            For CNT As Integer = 0 To .Columns.Count - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).Resizable = DataGridViewTriState.False
            Next
            .Columns("TRFNO").HeaderText = "TRFNO"
            .Columns("PCS~GRSWT~NETWT").HeaderText = "DETAILS"
            .Columns("REMARK1~REMARK2").HeaderText = "REMARKS"
            .Columns("COSTNAME").HeaderText = "COSTNAME"
            .RowHeadersVisible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .Height = .ColumnHeadersHeight
            .Columns("SCROLL").Width = 20
            .Columns("SCROLL").HeaderText = ""
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridview.ColumnCount - 1
                If gridview.Columns(cnt).Visible Then colWid += gridview.Columns(cnt).Width
            Next
            If colWid >= gridview.Width Then
                gridViewHead.Columns("SCROLL").Visible = CType(gridview.Controls(1), VScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
            Else
                gridViewHead.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
#End Region

#Region ""
    Private Sub AutoSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoSizeToolStripMenuItem.Click
        If gridview.RowCount > 0 Then
            If AutoSizeToolStripMenuItem.Checked Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
        If Dgv.RowCount > 0 Then
            If AutoSizeToolStripMenuItem.Checked Then
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                Dgv.Invalidate()
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub btnManufacturing_Click(sender As Object, e As EventArgs) Handles btnManufacturing.Click
        If MFG_SERVERNAME.ToString = "" Then
            MsgBox("Manufacturing Server Name not found")
            Exit Sub
        End If
        If MFG_USERNAME.ToString = "" Then
            MsgBox("Manufacturing User Name not found")
            Exit Sub
        End If
        Try
            Mfg_Con = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=master;Data Source={0};User Id=" & MFG_USERNAME & ";password=" & MFG_PWD & ";", MFG_SERVERNAME))
            Mfg_Con.Open()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace + vbCrLf + ex.InnerException.ToString)
            Exit Sub
        End Try


        If Val(GetSqlValue(Mfg_Con, "SELECT 1 FROM SYSDATABASES WHERE NAME='" & cnMfgAdminDb.ToString & "'")) = 0 Then
            MsgBox("Manufacturing Admin DB not found")
            Exit Sub
        End If
        If Val(GetSqlValue(Mfg_Con, "SELECT 1 FROM SYSDATABASES WHERE NAME='" & cnMfgStockDb.ToString & "'")) = 0 Then
            MsgBox("Manufacturing Transaction DB not found")
            Exit Sub
        End If


        Dim MFG_ACCODE_GET As String = ""
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='MFG_ACCODE_" + strCompanyId.ToString + "' "
        Dim Str_AcCode As String = UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "N", tran).ToString)
        If Str_AcCode <> "" Then
            Dim MFG_ACCODEAR() As String
            MFG_ACCODEAR = Str_AcCode.Split(",")
            If MFG_ACCODEAR.Length = 2 Then
                MFG_ACCODE_GET = MFG_ACCODEAR(1)
                MFG_ACCODE = MFG_ACCODEAR(0)
            Else
                MFG_ACCODE = MFG_ACCODE
            End If
        Else
            MFG_ACCODE = MFG_ACCODE
        End If

        Dgv.DataSource = Nothing
        txtCheckedItems.Clear()
        txtCheckedItems_TextChanged(Me, New EventArgs)
        Me.Refresh()
        StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_ACC') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " TRANNO INT"
        StrSql += vbCrLf + " ,TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " ,ACNAME VARCHAR(500)"
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
        StrSql += vbCrLf + " ,SNO VARCHAR(20)"
        StrSql += vbCrLf + " ,COSTNAME VARCHAR(100)"
        StrSql += vbCrLf + " )"
        Cmd = New OleDbCommand(StrSql, Mfg_Con) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If MFG_DB Then
            StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_ACC"
            StrSql += vbCrLf + " (TRANNO,TRANDATE,CATNAME,ACNAME,PCS,GRSWT"
            StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT"
            StrSql += vbCrLf + " ,BATCHNO,PAYMODE,CTLID,COMPANYID,RESULT,TYPE,SNO,COSTNAME)"
            StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,C.CATNAME,H.ACNAME"
            StrSql += vbCrLf + " ,SUM(PCS) PCS"
            StrSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
            StrSql += vbCrLf + " ,SUM(NETWT) NETWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
            StrSql += vbCrLf + " ,0 AMOUNT"
            StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
            StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,'" + strCompanyId + "' COMPANYID"
            StrSql += vbCrLf + " ,0"
            StrSql += vbCrLf + " ,REMARK2,T.SNO"
            'StrSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnMfgAdminDb & "..COSTCENTRE WHERE ACCODE=T.ACCODE)COSTNAME"
            StrSql += vbCrLf + " ,'" & cnCostName & "' COSTNAME"
            StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSUE AS T"
            StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..CATEGORY AS C ON C.CATCODE = T.CATCODE"
            StrSql += vbCrLf + " WHERE LEN(T.TRANTYPE)>2 AND ISNULL(TRANSFERED,'')='' AND ISNULL(T.REMARK2,'')!='I-R' "
            StrSql += vbCrLf + " AND T.TRANDATE='" & dtpTransfer.Value.Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND T.ACCODE='" & IIf(MFG_ACCODE_GET <> "", MFG_ACCODE_GET, MFG_ACCODE) & "'"
            StrSql += vbCrLf + " AND ISNULL(T.CANCEL,'')=''"
            If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TRFNO='" & txtTranNo.Text & "'"
            If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                StrSql += vbCrLf + " AND T.COMPANYID IN"
                StrSql += vbCrLf + "(SELECT COMPANYID FROM " & cnMfgAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
            End If
            StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,C.CATNAME,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID,T.SNO,T.REMARK2,T.ACCODE"

        Else
            'StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_ACC"
            'StrSql += vbCrLf + " (TRANNO,TRANDATE,CATNAME,ACNAME,PCS,GRSWT"
            'StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT"
            'StrSql += vbCrLf + " ,BATCHNO,PAYMODE,CTLID,COMPANYID,RESULT,TYPE,SNO,COSTNAME)"
            'StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,C.CATNAME,H.ACNAME"
            'StrSql += vbCrLf + " ,SUM(PCS) PCS"
            'StrSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
            'StrSql += vbCrLf + " ,SUM(NETWT) NETWT"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
            'StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..RECEIPTSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
            'StrSql += vbCrLf + " ,0 AMOUNT"
            'StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
            'StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,'" + strCompanyId + "' COMPANYID"
            'StrSql += vbCrLf + " ,0"
            'StrSql += vbCrLf + " ,REMARK2,T.SNO"
            ''StrSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnMfgAdminDb & "..COSTCENTRE WHERE ACCODE=T.ACCODE)COSTNAME"
            'StrSql += vbCrLf + " ,'" & cnCostName & "' COSTNAME"
            'StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..RECEIPT AS T"
            'StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            'StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..CATEGORY AS C ON C.CATCODE = T.CATCODE"
            'StrSql += vbCrLf + " WHERE LEN(T.TRANTYPE)>2 AND ISNULL(TRANSFERED,'')='' "
            'StrSql += vbCrLf + " AND T.TRANDATE='" & dtpTransfer.Value.Date.ToString("yyyy-MM-dd") & "'"
            'If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TRFNO='" & txtTranNo.Text & "'"
            'If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
            '    StrSql += vbCrLf + " AND T.COMPANYID IN"
            '    StrSql += vbCrLf + "(SELECT COMPANYID FROM " & cnMfgAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
            'End If
            'StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,C.CATNAME,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID,T.SNO,T.REMARK2,T.ACCODE"

            StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_ACC"
            StrSql += vbCrLf + " (TRANNO,TRANDATE,CATNAME,ACNAME,PCS,GRSWT"
            StrSql += vbCrLf + " ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT"
            StrSql += vbCrLf + " ,BATCHNO,PAYMODE,CTLID,COMPANYID,RESULT,TYPE,SNO,COSTNAME)"
            StrSql += vbCrLf + " SELECT TRANNO,TRANDATE,C.CATNAME,H.ACNAME"
            StrSql += vbCrLf + " ,SUM(PCS) PCS"
            StrSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
            StrSql += vbCrLf + " ,SUM(NETWT) NETWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='D')) DIAWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='S')) STNWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnMfgStockDb & "..ISSSTONE WHERE ISSSNO=T.SNO AND STNITEMID IN(SELECT ITEMID FROM " & cnMfgAdminDb & "..ITEMMAST WHERE DIASTONE='P')) PREWT"
            StrSql += vbCrLf + " ,0 AMOUNT"
            StrSql += vbCrLf + " ,T.BATCHNO,T.TRANTYPE AS PAYMODE"
            StrSql += vbCrLf + " ,CASE WHEN T.TRANTYPE = 'RPU' THEN 'GEN-SM-RECPUR' ELSE 'GEN-SM-REC' END AS CTLID,'" + strCompanyId + "' COMPANYID"
            StrSql += vbCrLf + " ,0"
            StrSql += vbCrLf + " ,REMARK2,T.SNO"
            'StrSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnMfgAdminDb & "..COSTCENTRE WHERE ACCODE=T.ACCODE)COSTNAME"
            StrSql += vbCrLf + " ,'" & cnCostName & "' COSTNAME"
            StrSql += vbCrLf + " FROM " & cnMfgStockDb & "..ISSUE AS T"
            StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..ACHEAD AS H ON H.ACCODE = T.ACCODE"
            StrSql += vbCrLf + " INNER JOIN " & cnMfgAdminDb & "..CATEGORY AS C ON C.CATCODE = T.CATCODE"
            StrSql += vbCrLf + " WHERE LEN(T.TRANTYPE)>2 AND ISNULL(TRANSFERED,'')='' AND ISNULL(T.REMARK2,'')!='I-R' "
            StrSql += vbCrLf + " AND T.TRANDATE='" & dtpTransfer.Value.Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND T.ACCODE='" & IIf(MFG_ACCODE_GET <> "", MFG_ACCODE_GET, MFG_ACCODE) & "'"
            StrSql += vbCrLf + " AND ISNULL(T.CANCEL,'')=''"
            If txtTranNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TRFNO='" & txtTranNo.Text & "'"
            If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                StrSql += vbCrLf + " AND T.COMPANYID IN"
                StrSql += vbCrLf + "(SELECT COMPANYID FROM " & cnMfgAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
            End If
            StrSql += vbCrLf + " GROUP BY T.TRANNO,T.TRANDATE,C.CATNAME,H.ACNAME,T.BATCHNO,T.TRANTYPE,T.COMPANYID,T.SNO,T.REMARK2,T.ACCODE"
        End If
        Cmd = New OleDbCommand(StrSql, Mfg_Con) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_ACC)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + "    (CATNAME,PCS,GRSWT"
        StrSql += vbCrLf + "    ,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,PREPCS,PREWT,AMOUNT,ORD,RESULT)"
        StrSql += vbCrLf + "    SELECT 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),SUM(AMOUNT),9,2"
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + "    UNION ALL"
        StrSql += vbCrLf + "    SELECT CATNAME + 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS)"
        StrSql += vbCrLf + "    ,SUM(DIAWT),SUM(STNPCS),SUM(STNWT),SUM(PREPCS),SUM(PREWT),SUM(AMOUNT),NULL,1"
        StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMP_ACC GROUP BY CATNAME"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, Mfg_Con) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PCS=NULL WHERE PCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET STNPCS=NULL WHERE STNPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET DIAPCS=NULL WHERE DIAPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PREPCS=NULL WHERE PREPCS=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET GRSWT=NULL WHERE GRSWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET NETWT=NULL WHERE NETWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET DIAWT=NULL WHERE DIAWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET STNWT=NULL WHERE STNWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET PREWT=NULL WHERE PREWT=0"
        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_ACC SET AMOUNT=NULL WHERE AMOUNT=0"
        Cmd = New OleDbCommand(StrSql, Mfg_Con) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_ACC"
        StrSql += vbCrLf + " ORDER BY RESULT,ISNULL(ORD,0),TRANDATE,SUBSTRING(BATCHNO,8,10),TRANNO,AMOUNT"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("APPROVED", GetType(Boolean))
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, Mfg_Con)
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
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
            .Columns("AMOUNT").Visible = True
            .Columns("ACNAME").Visible = False
            .Columns("TYPE").Visible = True
            .Columns("COSTNAME").Visible = True
            .Columns("SNO").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Manufacturing = True
        End With
        Dgv.Select()
    End Sub
#End Region

    Private Function GetSqlValueSQL(ByVal Cnn As SqlClient.SqlConnection, ByVal Qry As String) As Object
        Dim Obj As Object = Nothing
        Dim Daa As SqlClient.SqlDataAdapter
        Dim DtTemp As New DataTable
        Daa = New SqlClient.SqlDataAdapter(Qry, Cnn)
        Daa.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Obj = DtTemp.Rows(0).Item(0)
        End If
        Return Obj
    End Function

    Private Function GetSqlTableSQL(ByVal StrSql As String, ByVal lCn As SqlClient.SqlConnection, Optional ByVal Tran As SqlClient.SqlTransaction = Nothing) As DataTable
        Dim CmdSQL As New SqlCommand(StrSql, lCn)
        If Tran IsNot Nothing Then CmdSQL.Transaction = Tran
        Dim Daa As New SqlDataAdapter(CmdSQL)
        Dim DtTemp As New DataTable
        Da.Fill(DtTemp)
        Return DtTemp
    End Function
    Private Sub chkWithToDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkWithFromDate.CheckedChanged
        If chkWithFromDate.Checked Then
            dtpfromdate.Visible = True
            dtpTodate.Visible = True
            chkWithFromDate.Text = "FromDate"
            Label2.Visible = True
        Else
            Label2.Visible = False
            dtpfromdate.Visible = False
            dtpTodate.Visible = False
            chkWithFromDate.Text = "ChkDate"
        End If
    End Sub

End Class