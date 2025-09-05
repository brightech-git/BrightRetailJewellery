Imports System.Data.OleDb
Public Class frmPurifyrec
    Dim dtMelting As New DataTable
    Dim strSql As String
    Dim dtPendingBagNo As New DataTable
    Dim cmd As OleDbCommand
    Dim tranNo As Integer = Nothing
    Dim batchNo As String = Nothing
    Dim cancelFlag As Boolean = False
    'Dim objTouchPureWt As New BillTouchPureWt
    Dim mtxtSaPureWt_WET As Decimal = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dtMelting = New DataTable
        dtMelting.Columns.Add("BAGNO", GetType(String))
        dtMelting.Columns.Add("ICATEGORY", GetType(String))
        dtMelting.Columns.Add("RCATEGORY", GetType(String))
        dtMelting.Columns.Add("PURITY", GetType(Decimal))
        dtMelting.Columns.Add("RATE", GetType(Decimal))
        dtMelting.Columns.Add("PCS", GetType(Integer))
        dtMelting.Columns.Add("WEIGHT", GetType(Decimal))
        dtMelting.Columns.Add("GRSWT", GetType(Decimal))
        dtMelting.Columns.Add("NETWT", GetType(Decimal))
        dtMelting.Columns.Add("SAMPLEWT", GetType(Decimal))
        dtMelting.Columns.Add("SCRAPWT", GetType(Decimal))
        dtMelting.Columns.Add("VALUE", GetType(Decimal))
        dtMelting.Columns.Add("WASTAGE", GetType(Decimal))
        dtMelting.Columns.Add("MC", GetType(Decimal))
        dtMelting.Columns.Add("TOUCHPER", GetType(Decimal))
        dtMelting.Columns.Add("PUREWT", GetType(Decimal))
        dtMelting.AcceptChanges()
        GridView.DataSource = dtMelting
        GridView.Columns("ICATEGORY").MinimumWidth = 150
        GridView.Columns("RCATEGORY").MinimumWidth = 200
        GridView.Columns("PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("SAMPLEWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("ICATEGORY").HeaderText = "ISSUED CATEGORY"
        GridView.Columns("RCATEGORY").HeaderText = "RECEIVED CATEGORY"
        GridView.Columns("WEIGHT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("NETWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("SAMPLEWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("SCRAPWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("WASTAGE").Visible = False
        GridView.Columns("MC").Visible = False
        GridView.Columns("TOUCHPER").Visible = False
        GridView.Columns("PUREWT").Visible = False
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        cancelFlag = False
        objGPack.TextClear(Me)

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '5' "
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbSmith_MAN)


        dtMelting.Rows.Clear()
        dtMelting.AcceptChanges()
        btnSave.Enabled = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        lblBullionRate.Text = "Bullion Rate : " & Format(Val(GetRate_Purity(dtpDate.Value, objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = 'G' AND PURITY = 100"))), "0.00")
        tabMain.SelectedTab = tabGeneral
        dtpDate.Select()


    End Sub

    Private Sub frmMeltingIssue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBAck_Click(Me, New EventArgs)
                Exit Sub
            End If
            If gridViewPendingBagNo.Focused Then
                If btnSave.Enabled Then
                    btnSave.Focus()
                Else
                    dtpDate.Select()
                End If
            End If
        End If
    End Sub

    Private Sub frmMeltingIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridViewPendingBagNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmMeltingIssue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtBagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBagNo.GotFocus
        If gridViewPendingBagNo.Rows.Count > 0 Then
            gridViewPendingBagNo.Visible = True
            gridViewPendingBagNo.Select()
        ElseIf btnSave.Enabled = True And GridView.RowCount > 0 Then
            btnSave.Focus()
        Else
            cmbSmith_MAN.Select()
            MsgBox("There is no more bag(s) to be receipt", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub gridViewPendingBagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewPendingBagNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridViewPendingBagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewPendingBagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridViewPendingBagNo.RowCount > 0 Then
                gridViewPendingBagNo.CurrentCell = gridViewPendingBagNo.Rows(gridViewPendingBagNo.CurrentRow.Index).Cells(0)
                txtBagNo.Text = gridViewPendingBagNo.CurrentRow.Cells("BAGNO").Value.ToString
                txtCategory.Text = gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString

                txtPurity_PER.Text = Format(Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString & "')")), "0.00")

                txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString
                'txtNetWt_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString
                Me.SelectNextControl(txtBagNo, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub gridViewPendingBagNo_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewPendingBagNo.LostFocus
        gridViewPendingBagNo.Visible = False
    End Sub

    Private Sub txtCategory_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCategory.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ''validation
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtBagNo.Text = "" Then
            MsgBox("Bag No should not Empty", MsgBoxStyle.Information)
            txtBagNo.Select()
            Exit Sub
        End If
        'If Val(txtRate_AMT.Text) = 0 Then
        '    MsgBox("Bulliion Rate Should not Empty", MsgBoxStyle.Information)
        '    Exit Sub
        'End If
        If Val(txtWeight_WET.Text) = 0 Then
            MsgBox("Weight should not Empty", MsgBoxStyle.Information)
            txtWeight_WET.Focus()
            Exit Sub
        End If
        Dim ro As DataRow = dtMelting.NewRow
        ro("BAGNO") = txtBagNo.Text
        ro("ICATEGORY") = txtCategory.Text

        ro("PURITY") = Val(txtPurity_PER.Text)

        ro("PCS") = 0


        ro("WEIGHT") = Val(txtWeight_WET.Text)
        ro("GRSWT") = Val(txtReceivedWt_WET.Text)
        ro("PUREWT") = mtxtSaPureWt_WET

        dtMelting.Rows.Add(ro)
        dtMelting.AcceptChanges()
        txtBagNo.Clear()
        txtCategory.Clear()
        txtPurity_PER.Clear()
        txtWeight_WET.Clear()
        txtReceivedWt_WET.Clear()
        mtxtSaPureWt_WET = 0
        txtRemark1.Clear()
        txtRemark2.Clear()
        btnSave.Enabled = True
        RefreshPendingBagNoView()
        If gridViewPendingBagNo.RowCount > 0 Then
            txtBagNo.Select()
        Else
            btnSave.Select()
        End If

    End Sub

    Private Sub RefreshPendingBagNoView()
        Dim bagNo As String = ""
        For Each Row As DataRow In dtMelting.Rows
            bagNo += "'" & Row("BAGNO").ToString & "',"
        Next
        Dim dt As New DataTable
        dt = dtPendingBagNo.Copy
        If bagNo.Length > 0 Then
            bagNo = Mid(bagNo, 1, bagNo.Length - 1)
            dt.DefaultView.RowFilter = "BAGNO NOT IN (" + bagNo + ")"
        End If
        dt.AcceptChanges()
        gridViewPendingBagNo.DataSource = dt
    End Sub

    Private Sub GridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles GridView.UserDeletedRow
        dtMelting.AcceptChanges()
        RefreshPendingBagNoView()
        If Not GridView.RowCount > 0 Then
            btnSave.Enabled = False
            dtpDate.Select()
        End If
    End Sub

    Private Sub dtpDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If GridView.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbSmith_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSmith_MAN.GotFocus
        If GridView.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not btnSave.Enabled Then Exit Sub
        Try

            tran = Nothing
            tran = cn.BeginTransaction()

GenBillNo:
            tranNo = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'", , , tran))
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo + 1 & "' WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'"
            strSql += " and CONVERT(INT,CTLTEXT) = " & tranNo & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            If Not cmd.ExecuteNonQuery() > 0 Then
                GoTo GenBillNo
            End If
            tranNo += 1

            batchNo = GetNewBatchno(cnCostId, dtpDate.Value.ToString("yyyy-MM-dd"), tran)
            For cnt As Integer = 0 To GridView.RowCount - 1
                InsertissDetails(cnt)
            Next
            tran.Commit()
            tran = Nothing
            MsgBox(tranNo & " Generated..")
            btnNew_Click(Me, New EventArgs)
            Dim pBatchno As String = batchNo
            Dim pBillDate As Date = dtpDate.Value.Date.ToString("yyyy-MM-dd")
            btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":SMI")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":SMR;" & _
                    LSet("BATCHNO", 15) & ":" & pBatchno & ";" & _
                    LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Public Sub InsertIssDetails(ByVal index As Integer)
        With GridView.Rows(index)
            Dim issSno As String = GetNewSno(TranSnoType.ISSUECODE, tran)
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("RCATEGORY").Value.ToString & "'", , , tran)
            Dim wast As Double = Nothing
            Dim wastPer As Double = Nothing
            Dim alloy As Double = Nothing
            Dim type As String = objGPack.GetSqlValue("SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("RCATEGORY").Value.ToString & "')", , , tran)
            strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,VATEXM,TAX"
            strSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & issSno & "'" ''SNO
            strSql += " ," & tranNo & "" 'TRANNO
            strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'IMP'" 'TRANTYPE
            strSql += " ,0" 'PCS
            strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ,0" 'LESSWT
            strSql += " ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '0
            strSql += " ,''" 'TAGNO
            strSql += " ,0" 'ITEMID
            strSql += " ,0" 'SUBITEMID
            strSql += " ," & wastPer & "" 'WASTPER
            strSql += " ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
            strSql += " ,0" 'MCGRM
            strSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
            strSql += " ," & Val(.Cells("VALUE").Value.ToString) & "" 'AMOUNT
            strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
            strSql += " ,''" 'SALEMODE
            strSql += " ,'N'" 'GRSNET
            strSql += " ,''" 'TRANSTATUS ''
            strSql += " ,''" 'REFNO ''
            strSql += " ,NULL" 'REFDATE NULL
            strSql += " ,'" & cnCostId & "'" 'COSTID 
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            Select Case type
                Case "O"
                    strSql += " ,'O'" 'FLAG 
                Case "M"
                    strSql += " ,'M'" 'FLAG 
                Case Else
                    strSql += " ,'T'" 'FLAG 
            End Select
            strSql += " ,0" 'EMPID
            strSql += " ,0" 'TAGGRSWT
            strSql += " ,0" 'TAGNETWT
            strSql += " ,0" 'TAGRATEID
            strSql += " ,0" 'TAGSVALUE
            strSql += " ,''" 'TAGDESIGNER  
            strSql += " ,0" 'ITEMCTRID
            strSql += " ,0" 'ITEMTYPEID
            strSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
            strSql += " ,''" 'TABLECODE
            strSql += " ,''" 'INCENTIVE
            strSql += " ,''" 'WEIGHTUNIT
            strSql += " ,'" & catCode & "'" 'CATCODE
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("ICATEGORY").Value.ToString & "'", , , tran) & "'" 'OCATCODE
            strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_MAN.Text & "'", , , tran) & "'" 'ACCODE
            strSql += " ," & alloy & "" 'ALLOY
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,''" 'REMARK1
            strSql += " ,''" 'REMARK2
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,0" 'DISCOUNT
            strSql += " ,''" 'RUNNO
            strSql += " ,''" 'CASHID
            strSql += " ,'Y'" 'VATEXM
            strSql += " ,0" 'TAX
            strSql += " ,0" 'STONEAMT
            strSql += " ,0" 'MISCAMT
            strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE= '" & catCode & "'", , , tran) & "'" 'METALID
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " INSERT INTO " & cnStockDb & "..MELTINGDETAIL"
            strSql += " (TRANNO,TRANDATE,RECISS,CATCODE,PCS,GRSWT,NETWT,RATE,AMOUNT"
            strSql += " ,BAGNO,BATCHNO,APPVER,COMPANYID,USERID,UPDATED"
            strSql += " ,UPTIME,CANCEL)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " " & tranNo & "" 'TRANNO
            strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'trandate
            strSql += " ,'I'" 'RECISS
            strSql += " ,'" & catCode & "'" 'CATCODE
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            strSql += " ," & Val(.Cells("WEIGHT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += " ," & Val(.Cells("VALUE").Value.ToString) & "" 'AMOUNT
            strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,''" 'CANCEL
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End With
    End Sub

    Private Sub CallGrid()
        strSql = " SELECT BAGNO,TRANDATE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATEGORY"
        strSql += " , GRSWT,NETWT,RATE,AMOUNT VALUE,BATCHNO"
        strSql += " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += " WHERE ISNULL(BAGNO,'') <> ''"
        strSql += " AND TRANTYPE = 'RRE'"
        strSql += " AND ISNULL(CANCEL,'') = ''"

        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridViewOpen.DataSource = dt
        If gridViewOpen.RowCount > 0 Then
            With gridViewOpen
                .Columns("BATCHNO").Visible = False
                .Columns("BAGNO").Width = 100
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("CATEGORY").Width = 250
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not btnOpen.Enabled Then Exit Sub
        CallGrid()
        If gridViewOpen.RowCount > 0 Then
            tabMain.SelectedTab = tabView
            gridViewOpen.Select()
        Else
            MsgBox("There is no record", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnBAck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBAck.Click
        If tabMain.SelectedTab.Name = tabView.Name Then
            If cancelFlag Then
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If
            tabMain.SelectedTab = tabGeneral
            dtpDate.Select()
        End If
    End Sub

    Private Sub btnSave_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.EnabledChanged
        btnOpen.Enabled = Not btnSave.Enabled
    End Sub

    Private Sub gridViewOpen_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewOpen.KeyPress
        If UCase(e.KeyChar) = "C" Then
            If MessageBox.Show("Do you want to cancel this entry", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    strSql = " UPDATE " & cnStockDb & "..RECEIPT SET CANCEL = 'Y' "
                    strSql += " WHERE TRANDATE = '" & gridViewOpen.CurrentRow.Cells("TRANDATE").Value & "' "
                    strSql += " AND  ISNULL(BATCHNO,'') = '" & gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " AND  ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    strSql = " UPDATE " & cnStockDb & "..MELTINGDETAIL SET CANCEL = 'Y' "
                    strSql += " WHERE  ISNULL(BATCHNO,'') = '" & gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " AND  ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    tran.Commit()
                    tran = Nothing
                    cancelFlag = True
                    MsgBox("Successfully Cancelled.", MsgBoxStyle.Information)
                    btnOpen_Click(Me, New EventArgs)
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox("Message : " + ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End If
        End If
    End Sub


    Private Sub txtWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_WET.LostFocus
        If gridViewPendingBagNo.RowCount = 0 Then Exit Sub

        If Val(gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString) < Val(txtWeight_WET.Text) Then
            MsgBox("Weight should not exceed " & gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString, MsgBoxStyle.Information)
            txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString
            txtWeight_WET.Select()
            Exit Sub
        End If
        'Dim wast As Double = Val(gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString) - Val(txtWeight_WET.Text)
        'txtWastage_WET.Text = IIf(wast <> 0, Format(wast, "0.000"), "")
    End Sub

    Private Sub txtWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        '    SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbSmith_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSmith_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then




            'strSql = vbCrLf + " SELECT BAGNO,METAL,CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            'strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            'strSql += vbCrLf + " FROM"
            'strSql += vbCrLf + " ("
            'strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            'strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            'strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            'strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            'strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            'strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            'strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            'strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'RRE'"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            'strSql += vbCrLf + " UNION ALL"
            'strSql += vbCrLf + " SELECT 'I' SEP,BAGNO"
            'strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            'strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            'strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            'strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            'strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            'strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            'strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'IMP'"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            'strSql += vbCrLf + " )X"
            'strSql += vbCrLf + " GROUP BY BAGNO,CATEGORY,METAL"
            'strSql += vbCrLf + " having SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 or SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) > 0 "
            'da = New OleDbDataAdapter(strSql, cn)
            'dtPendingBagNo = New DataTable
            'da.Fill(dtPendingBagNo)
            'dtPendingBagNo.AcceptChanges()




            strSql = " SELECT * FROM "
            strSql += " ("
            strSql += " SELECT BAGNO" + vbCrLf
            strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL" + vbCrLf
            strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY" + vbCrLf
            strSql += " ,SUM(GRSWT) - ISNULL((SELECT SUM(ISNULL(GRSWT,0)) FROM " & cnStockDb & "..MELTINGDETAIL WHERE BAGNO = R.BAGNO AND RECISS = 'R' AND ISNULL(CANCEL,'') = ''),0) GRSWT" + vbCrLf
            strSql += " ,SUM(NETWT) - ISNULL((SELECT SUM(ISNULL(NETWT,0)) FROM " & cnStockDb & "..MELTINGDETAIL WHERE BAGNO = R.BAGNO AND RECISS = 'R' AND ISNULL(CANCEL,'') = ''),0) NETWT" + vbCrLf
            strSql += " FROM " & cnStockDb & "..ISSUE R" + vbCrLf
            strSql += " WHERE ISNULL(BAGNO,'') <> ''" + vbCrLf
            strSql += " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_MAN.Text & "')"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += " GROUP BY BAGNO,CATCODE" + vbCrLf
            strSql += " )X"
            strSql += " WHERE GRSWT <> 0"
            da = New OleDbDataAdapter(strSql, cn)
            dtPendingBagNo = New DataTable
            da.Fill(dtPendingBagNo)
            dtPendingBagNo.AcceptChanges()
            If Not dtPendingBagNo.Rows.Count > 0 Then
                MsgBox("There is no bag(s) issued for this smith", MsgBoxStyle.Information)
                cmbSmith_MAN.Select()
                Exit Sub
            End If
            gridViewPendingBagNo.BorderStyle = BorderStyle.Fixed3D
            gridViewPendingBagNo.BackgroundColor = Color.White
            gridViewPendingBagNo.Visible = False
            gridViewPendingBagNo.DataSource = dtPendingBagNo.Copy
            gridViewPendingBagNo.GridColor = Color.White
            With gridViewPendingBagNo
                .Columns("BAGNO").Width = 50
                .Columns("METAL").Width = 60
                .Columns("CATEGORY").Width = 150
                .Columns("GRSWT").Width = 80
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").Width = 80
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End If
    End Sub

    Private Sub CalcPureWt()
        Dim wt As Decimal = Val(txtReceivedWt_WET.Text)
        '       wt = (wt * Val(txtTouchper.Text)) / 100
        mtxtSaPureWt_WET = IIf(wt <> 0, wt, 0)
        txtPurewt_WET.Text = mtxtSaPureWt_WET
    End Sub









    Private Sub txtTouchper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcPureWt()
    End Sub

    Private Sub txtReceivedWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceivedWt_WET.GotFocus
        txtReceivedWt_WET.Text = txtWeight_WET.Text
    End Sub

    Private Sub txtReceivedWt_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceivedWt_WET.TextChanged

    End Sub
End Class