Imports System.Data.OleDb
Public Class frmMeltingReceipt
    Dim dtMelting As New DataTable
    Dim strSql As String
    Dim dtPendingBagNo As New DataTable
    Dim cmd As OleDbCommand
    Dim tranNo As Integer = Nothing
    Dim batchNo As String = Nothing
    Dim cancelFlag As Boolean = False
    'Dim objTouchPureWt As New BillTouchPureWt
    Dim mtxtSaPureWt_WET As Decimal = 0
    Dim mRecLessWt As Decimal = 0
    Dim RGrswtasNetwt As Boolean = IIf(GetAdmindbSoftValue("RGRSWTASNETWT", "Y") = "Y", True, False)
    Dim MRMI_MANUALNO As Boolean = IIf(GetAdmindbSoftValue("MRMI_MANUALNO", "Y") = "Y", True, False)
    Dim MELTING_MC_ACCODE As String = GetAdmindbSoftValue("MELTING_MC_ACCODE", "MELTMC")
    Dim objManualBill As frmManualBillNoGen
    Dim flagupdate As Boolean = False
    Dim flagupdateTranno As String = ""
    Dim flagupdateBatchno As String = ""


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
        dtMelting.Columns.Add("LESSWT", GetType(Decimal))
        dtMelting.Columns.Add("NETWT", GetType(Decimal))
        dtMelting.Columns.Add("SAMPLEWT", GetType(Decimal))
        dtMelting.Columns.Add("SCRAPWT", GetType(Decimal))
        dtMelting.Columns.Add("VALUE", GetType(Decimal))
        dtMelting.Columns.Add("WASTAGE", GetType(Decimal))
        dtMelting.Columns.Add("MC", GetType(Decimal))
        dtMelting.Columns.Add("TOUCHPER", GetType(Decimal))
        dtMelting.Columns.Add("PUREWT", GetType(Decimal))
        dtMelting.Columns.Add("REMARK1", GetType(String))
        dtMelting.Columns.Add("REMARK2", GetType(String))
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

    Private Sub CalcRate()
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbReceivedCat.Text & "'")
        Dim buRate As Double = Val(GetRate_Purity(dtpDate.Value, objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = '" & metalId & "' AND PURITY = 100")))
        Dim rate As Double = buRate * (Val(txtPurity_PER.Text) / 100)
        If rate = 0 Then Exit Sub
        txtRate_AMT.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        cancelFlag = False
        objGPack.TextClear(Me)

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D') "
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbSmith_OWN)

        ''Load Company Name       
        strSql = " SELECT 'ALL' COMPANYNAME "
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, CmbCompany_OWN, , True)
        CmbCompany_OWN.Text = strCompanyName

        dtMelting.Rows.Clear()
        dtMelting.AcceptChanges()
        btnSave.Enabled = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        lblBullionRate.Text = "Bullion Rate : " & Format(Val(GetRate_Purity(dtpDate.Value, objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = 'G' AND PURITY = 100"))), "0.00")
        tabMain.SelectedTab = tabGeneral
        flagupdate = False
        flagupdateTranno = ""
        flagupdateBatchno = ""

        dtpDate.Select()
        dtpDate.Enabled = True
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
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmMeltingIssue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtBagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBagNo.GotFocus
        If flagupdate = True Then
            Exit Sub
        End If
        If gridViewPendingBagNo.Rows.Count > 0 Then
            gridViewPendingBagNo.Visible = True
            gridViewPendingBagNo.Select()
        ElseIf btnSave.Enabled = True And GridView.RowCount > 0 Then
            btnSave.Focus()
        Else
            cmbSmith_OWN.Select()
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
                lblMetal.Text = gridViewPendingBagNo.CurrentRow.Cells("METAL").Value.ToString
                ' txtCategory.Text = gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString
                ' cmbReceivedCat.Text = gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString
                'txtPurity_PER.Text = Format(Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString & "')")), "0.00")
                CalcRate()
                'txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString
                If Val(gridViewPendingBagNo.CurrentRow.Cells("BAGWT").Value.ToString) > 0 Then
                    txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("BAGWT").Value.ToString
                Else
                    txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString
                End If
                'txtLess_WET.Text = Val(gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString & "") - Val(gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString & "")
                'txtReceivedWt_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString
                txtReceivedWt_WET.Text = Val(gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString) - Val(txtLess_WET.Text.ToString())
                'txtNetWt_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString
                Me.SelectNextControl(txtBagNo, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub gridViewPendingBagNo_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewPendingBagNo.LostFocus
        gridViewPendingBagNo.Visible = False
    End Sub

    Private Sub txtCategory_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_AMT.GotFocus
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

        Dim app_Achead As String = objGPack.GetSqlValue("SELECT ISNULL(ACTIVE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , Nothing)
        If app_Achead = "" Then
            MsgBox("Partycode not approve", MsgBoxStyle.Information)
            cmbSmith_OWN.Focus()
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
        ro("ICATEGORY") = cmbReceivedCat.Text
        ro("RCATEGORY") = cmbReceivedCat.Text
        ro("PURITY") = Val(txtPurity_PER.Text)
        ro("RATE") = Val(txtRate_AMT.Text)
        ro("PCS") = 0
        ro("SAMPLEWT") = Val(txtSampleWt_WET.Text)
        ro("SCRAPWT") = Val(txtScrapwt_WET.Text)
        ro("WEIGHT") = Val(txtWeight_WET.Text)
        ro("GRSWT") = Val(txtReceivedWt_WET.Text)
        ro("NETWT") = Val(txtNetWt_WET.Text)
        ro("LESSWT") = Val(txtLess_WET.Text)
        ro("VALUE") = mtxtSaPureWt_WET * Val(txtRate_AMT.Text)
        ro("WASTAGE") = Val(txtWastage_WET.Text)
        ro("MC") = Val(txtMcharge_AMT.Text)
        ro("TOUCHPER") = Val(txtTouchper.Text)
        ro("PUREWT") = mtxtSaPureWt_WET
        ro("REMARK1") = txtRemark1.Text
        ro("REMARK2") = txtRemark2.Text

        dtMelting.Rows.Add(ro)
        dtMelting.AcceptChanges()
        txtBagNo.Clear()
        'txtCategory.Clear()
        txtPurity_PER.Clear()
        txtRate_AMT.Clear()
        txtWeight_WET.Clear()
        txtNetWt_WET.Clear()
        txtSampleWt_WET.Clear()
        txtScrapwt_WET.Clear()
        txtWastage_WET.Clear()
        txtReceivedWt_WET.Clear()
        txtTouchper.Clear()
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
        If dt.Rows.Count > 0 Then
            If bagNo.Length > 0 Then
                bagNo = Mid(bagNo, 1, bagNo.Length - 1)
                dt.DefaultView.RowFilter = "BAGNO NOT IN (" + bagNo + ")"
            End If
            dt.AcceptChanges()
            gridViewPendingBagNo.DataSource = dt
        End If
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

    Private Sub cmbSmith_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSmith_OWN.GotFocus, cmbReceivedCat.GotFocus
        If cmbSmith_OWN.Focused And GridView.RowCount > 0 Then SendKeys.Send("{TAB}")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        If lblMetal.Text <> "" Then strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & lblMetal.Text & "')"
        strSql += " ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbReceivedCat)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not btnSave.Enabled Then Exit Sub
        Try
            If MRMI_MANUALNO Then
                objManualBill = New frmManualBillNoGen
                objManualBill.ShowDialog()
                If Val(objManualBill.txtBillNo_NUM.Text.ToString) = 0 Then MsgBox("Tran No not valid...", MsgBoxStyle.Information) : Exit Sub
            End If

            tran = Nothing
            tran = cn.BeginTransaction()

GenBillNo:
            If flagupdate = True Then

                strSql = " DELETE " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'RRE' AND BATCHNO = '" & flagupdateBatchno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE " & cnStockDb & "..MELTINGDETAIL "
                strSql += vbCrLf + " WHERE TRANTYPE = 'MR' AND "
                strSql += vbCrLf + " RECISS = 'R' "
                strSql += vbCrLf + " AND BATCHNO = '" & flagupdateBatchno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                If MELTING_MC_ACCODE <> "" Then
                    strSql = " DELETE " & cnStockDb & "..ACCTRAN "
                    strSql += vbCrLf + "  WHERE BATCHNO = '" & flagupdateBatchno & "' "
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If

                tranNo = flagupdateTranno

            Else
                tranNo = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-REC'  AND COMPANYID = '" & strCompanyId & "'", , , tran))
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo + 1 & "' WHERE CTLID = 'GEN-SM-REC'  AND COMPANYID = '" & strCompanyId & "'"
                strSql += " and CONVERT(INT,CTLTEXT) = " & tranNo & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                If Not cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If
                If MRMI_MANUALNO Then
                    If Val(objManualBill.txtBillNo_NUM.Text.ToString) <> 0 Then
                        tranNo = Val(objManualBill.txtBillNo_NUM.Text.ToString)
                    End If
                Else
                    tranNo += 1
                End If
            End If

            batchNo = GetNewBatchno(cnCostId, dtpDate.Value.ToString("yyyy-MM-dd"), tran)
            For cnt As Integer = 0 To GridView.RowCount - 1
                InsertPUDetails(cnt)
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
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":SMR;" &
                    LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                    LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";" &
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
    Public Sub InsertPUDetails(ByVal index As Integer)
        With GridView.Rows(index)
            Dim issSno As String = GetNewSno(TranSnoType.RECEIPTCODE, tran)
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("RCATEGORY").Value.ToString & "'", , , tran)
            Dim metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("RCATEGORY").Value.ToString & "'", , , tran)
            Dim wast As Double = Nothing
            Dim wastPer As Double = Nothing
            Dim alloy As Double = Nothing
            Dim type As String = objGPack.GetSqlValue("SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("RCATEGORY").Value.ToString & "')", , , tran)
            strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
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
            strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
            strSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO,MELTWT,KAFTTOUCH,DUSTWT,OTHERAMT"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & issSno & "'" ''SNO
            strSql += " ," & tranNo & "" 'TRANNO
            strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'RRE'" 'TRANTYPE
            strSql += " ,0" 'PCS
            strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
            strSql += " ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '0
            strSql += " ,''" 'TAGNO
            strSql += " ,0" 'ITEMID
            strSql += " ,0" 'SUBITEMID
            strSql += " ," & wastPer & "" 'WASTPER
            strSql += " ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
            strSql += " ,0" 'MCGRM
            strSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
            strSql += " ,0" '&  Val(.Cells("VALUE").Value.ToString) & "" 'AMOUNT
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
            strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , tran) & "'" 'ACCODE
            strSql += " ," & alloy & "" 'ALLOY
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
            strSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,0" 'DISCOUNT
            strSql += " ,''" 'RUNNO
            strSql += " ,''" 'CASHID
            strSql += " ,0" 'TAX
            strSql += " ,0" 'STONEAMT
            strSql += " ,0" 'MISCAMT
            strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE= '" & catCode & "'", , , tran) & "'" 'METALID
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
            strSql += " ,'" & Val(.Cells("SAMPLEWT").Value.ToString) & "'" 'KAFTWT
            strSql += " ,'" & Val(.Cells("Touchper").Value.ToString) & "'" 'KAFTTOUCH
            strSql += " ,'" & Val(.Cells("SCRAPWT").Value.ToString) & "'" 'SCRAP WT
            strSql += " ,'" & Val(.Cells("WEIGHT").Value.ToString) & "'" 'OTHERAMT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " INSERT INTO " & cnStockDb & "..MELTINGDETAIL"
            strSql += " (TRANNO,TRANDATE,RECISS,TRANTYPE,CATCODE,ACCODE,METALID,PCS,GRSWT,NETWT,RATE,AMOUNT"
            strSql += " ,BAGNO,BATCHNO,APPVER,COMPANYID,USERID,UPDATED"
            strSql += " ,UPTIME,CANCEL,BAGWT)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " " & tranNo & "" 'TRANNO
            strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'trandate
            strSql += " ,'R','MR'" 'RECISS
            strSql += " ,'" & catCode & "'" 'CATCODE
            strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , tran) & "'"
            strSql += " ,'" & metalid & "'" 'CATCODE
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
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
            strSql += " ,'" & Val(.Cells("WEIGHT").Value.ToString) & "'" 'OTHERAMT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            If MELTING_MC_ACCODE <> "" And Val(txtMcharge_AMT.Text.ToString) > 0 Then
                Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                strSql = " INSERT INTO " & cnStockDb & ".." & "ACCTRAN"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                strSql += " ,REFNO,REFDATE,PAYMODE"
                strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
                strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                strSql += " ,UPTIME,SYSTEMID,COSTID,APPVER,COMPANYID"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Sno & "'" ''SNO
                strSql += " ," & tranNo & "" 'TRANNO 
                strSql += " ,'" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ,'C'" 'TRANMODE
                strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , tran) & "'" 'ACCODE
                strSql += " ," & Math.Abs(Val(txtMcharge_AMT.Text.ToString)) & "" 'AMOUNT
                strSql += " ,0" 'PCS
                strSql += " ,0" 'GRSWT
                strSql += " ,0" 'NETWT
                strSql += " ,'0'" 'REFNO
                strSql += " ,NULL" 'REFDATE
                strSql += " ,'TR'" 'PAYMODE         
                strSql += " ,NULL" 'RELIASEDATE
                strSql += " ,'S'" 'FROMFLAG
                strSql += " ,'MELTING RECEIPT'" 'REMARK1
                strSql += " ,''" 'REMARK2
                strSql += " ,'" & MELTING_MC_ACCODE & "'" 'CONTRA
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'" & userId & "'" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & cnCostId & "'" 'COSTID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                strSql = ""

                Sno = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                strSql = " INSERT INTO " & cnStockDb & ".." & "ACCTRAN"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                strSql += " ,REFNO,REFDATE,PAYMODE"
                strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
                strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                strSql += " ,UPTIME,SYSTEMID,COSTID,APPVER,COMPANYID"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Sno & "'" ''SNO
                strSql += " ," & tranNo & "" 'TRANNO 
                strSql += " ,'" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ,'D'" 'TRANMODE
                strSql += " ,'" & MELTING_MC_ACCODE & "'" 'ACCODE
                strSql += " ," & Math.Abs(Val(txtMcharge_AMT.Text.ToString)) & "" 'AMOUNT
                strSql += " ,0" 'PCS
                strSql += " ,0" 'GRSWT
                strSql += " ,0" 'NETWT
                strSql += " ,'0'" 'REFNO
                strSql += " ,NULL" 'REFDATE
                strSql += " ,'TR'" 'PAYMODE         
                strSql += " ,NULL" 'RELIASEDATE
                strSql += " ,'S'" 'FROMFLAG
                strSql += " ,'MELTING RECEIPT'" 'REMARK1
                strSql += " ,''" 'REMARK2
                strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , tran) & "'" 'CONTRA
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'" & userId & "'" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & cnCostId & "'" 'COSTID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                strSql = ""
            End If
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
        If UCase(e.KeyChar) = "C" Then
            strSql = "SELECT 'CHECK' FROM " & cnStockDb & "..MELTINGDETAIL "
            strSql += " WHERE ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
            strSql += " AND RECISS = 'I' AND ISNULL(CANCEL,'') = '' AND TRANTYPE='PI'"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Purification Issue made against this bagno." & vbCrLf & "Cannot cancel this entry.", MsgBoxStyle.Information)
                Exit Sub
            End If
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
                    btnNew_Click(Me, New EventArgs)
                    btnOpen_Click(Me, New EventArgs)
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox("Message : " + ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End If
        End If
    End Sub

    Private Sub txtPurity_PER_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurity_PER.LostFocus
        CalcRate()
    End Sub

    Private Sub txtWeight_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWeight_WET.KeyDown
        txtReceivedWt_WET.Text = Val(txtWeight_WET.Text) - Val(txtLess_WET.Text) '- Val(txtWastage_WET.Text)
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
    Private Sub txtWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_WET.GotFocus
        '    SendKeys.Send("{TAB}")
    End Sub
    Private Sub cmbSmith_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSmith_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If flagupdate = True Then
                Exit Sub
            End If
            strSql = " SELECT * FROM ("
            strSql += vbCrLf + " SELECT BAGNO,METAL"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN BAGWT ELSE -1*BAGWT END) AS BAGWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN PUREWT ELSE -1*PUREWT END) AS PUREWT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,M.METALNAME METAL "
            strSql += vbCrLf + " ,SUM(ISNULL(BAGWT,0)) BAGWT"
            strSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
            strSql += vbCrLf + " ,(SELECT SUM(PUREWT) FROM  " & cnStockDb & "..ISSUE WHERE BAGNO=R.BAGNO AND ISNULL(CANCEL,'')='' AND TRANTYPE = 'IIS') PUREWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..MELTINGDETAIL R" + vbCrLf
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = R.METALID"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''" + vbCrLf
            strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND R.TRANTYPE = 'MI'"
            If CmbCompany_OWN.Text <> "" And CmbCompany_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_OWN.Text & "')"
            'strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " GROUP BY R.BAGNO,METALNAME"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'I' SEP,BAGNO,M.METALNAME METAL"
            strSql += vbCrLf + " ,SUM(ISNULL(BAGWT,0)) BAGWT"
            strSql += vbCrLf + " ,SUM(GRSWT)+(SELECT SUM(WASTAGE) FROM  " & cnStockDb & "..RECEIPT WHERE BAGNO=R.BAGNO AND ISNULL(CANCEL,'')='' AND TRANTYPE = 'RRE') GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)+(SELECT SUM(WASTAGE+MELTWT+DUSTWT) FROM  " & cnStockDb & "..RECEIPT WHERE BAGNO=R.BAGNO AND ISNULL(CANCEL,'')='' AND TRANTYPE = 'RRE') NETWT"
            'strSql += " ,SUM(NETWT)+(SELECT SUM(WASTAGE) FROM  " & cnStockDb & "..RECEIPT WHERE BAGNO=R.BAGNO AND ISNULL(CANCEL,'')='' AND TRANTYPE = 'RRE') NETWT"
            strSql += vbCrLf + " ,(SELECT SUM(PUREWT) FROM  " & cnStockDb & "..RECEIPT WHERE BAGNO=R.BAGNO AND ISNULL(CANCEL,'')='' AND TRANTYPE = 'RRE') PUREWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..MELTINGDETAIL R" + vbCrLf
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = R.METALID"

            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''" + vbCrLf
            strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''  AND R.TRANTYPE = 'MR'"
            If CmbCompany_OWN.Text <> "" And CmbCompany_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_OWN.Text & "')"
            'strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " GROUP BY R.BAGNO,METALNAME"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " GROUP BY X.BAGNO,X.METAL" + vbCrLf
            strSql += vbCrLf + " )Y WHERE GRSWT <> 0 AND BAGWT <> 0"
            da = New OleDbDataAdapter(strSql, cn)
            dtPendingBagNo = New DataTable
            da.Fill(dtPendingBagNo)
            dtPendingBagNo.AcceptChanges()
            If Not dtPendingBagNo.Rows.Count > 0 Then
                MsgBox("There is no bag(s) issued for this smith", MsgBoxStyle.Information)
                cmbSmith_OWN.Select()
                Exit Sub
            End If
            gridViewPendingBagNo.BorderStyle = BorderStyle.Fixed3D
            gridViewPendingBagNo.BackgroundColor = Color.White
            gridViewPendingBagNo.Visible = True
            gridViewPendingBagNo.DataSource = dtPendingBagNo.Copy
            gridViewPendingBagNo.GridColor = Color.White
            With gridViewPendingBagNo
                .Columns("BAGNO").Width = 50
                .Columns("METAL").Width = 60
                '.Columns("CATEGORY").Width = 150
                .Columns("BAGWT").Width = 80
                .Columns("BAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").Width = 80
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").Width = 80
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PUREWT").Width = 80
                .Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End If
    End Sub

    Private Sub CalcPureWt()
        Dim wt As Decimal = Val(txtNetWt_WET.Text)
        wt = (wt * Val(txtTouchper.Text)) / 100
        mtxtSaPureWt_WET = Format(IIf(wt <> 0, wt, 0), "0.000")
        txtPurewt_WET.Text = mtxtSaPureWt_WET
    End Sub

    Private Sub CalcNetWt()
        If RGrswtasNetwt Then
            txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text)
            Exit Sub
        End If
        txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtSampleWt_WET.Text) - Val(txtScrapwt_WET.Text) ' - Val(txtWastage_WET.Text)
    End Sub

    Private Sub txtSampleWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSampleWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim receivewt As String = Nothing
            'txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtLess_WET.Text) - Val(txtWastage_WET.Text) - Val(txtSampleWt_WET.Text) - Val(txtScrapwt_WET.Text)
            CalcNetWt()
        End If
    End Sub

    Private Sub cmbReceivedCat_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceivedCat.TextChanged
        Dim Qry As String = "SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID in (SELECT top 1 PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbReceivedCat.Text & "')"
        'txtPurity_PER.Text = Format(Val(objGPack.GetSqlValue(Qry)), "0.00")
        'txtTouchper.Text = Format(Val(objGPack.GetSqlValue(Qry)), "0.00")
        CalcRate()
    End Sub

    Private Sub txtWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_WET.TextChanged
        'txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtWastage_WET.Text) - Val(txtSampleWt_WET.Text) - Val(txtScrapwt_WET.Text)
        CalcNetWt()
        CalcLessWt()
        CalcPureWt()
    End Sub

    Private Sub txtWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_WET.TextChanged
        txtReceivedWt_WET.Text = Val(txtWeight_WET.Text) - Val(txtLess_WET.Text) - Val(txtWastage_WET.Text)
        'txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtWastage_WET.Text) - Val(txtSampleWt_WET.Text) - Val(txtScrapwt_WET.Text)
        CalcNetWt()
        CalcLessWt()
        CalcPureWt()
    End Sub

    Private Sub txtTouchper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouchper.TextChanged
        CalcPureWt()
    End Sub

    Private Sub txtReceivedWt_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceivedWt_WET.TextChanged
        'txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text & "") - Val(txtWastage_WET.Text & "") - Val(txtSampleWt_WET.Text & "") - Val(txtScrapwt_WET.Text & "")
        CalcNetWt()
        CalcPureWt()
    End Sub

    Private Sub txtNetWt_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNetWt_WET.TextChanged
        'txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtWastage_WET.Text) - Val(txtSampleWt_WET.Text) - Val(txtScrapwt_WET.Text)
        CalcNetWt()
        CalcPureWt()
    End Sub

    Private Function CalcLessWt()
        If RGrswtasNetwt Then
            txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text)
            Exit Function
        End If
        txtNetWt_WET.Text = Val(txtReceivedWt_WET.Text) - Val(txtLess_WET.Text)
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewOpen.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MELTING RECEIPT", gridViewOpen, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridViewOpen.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MELTING RECEIPT", gridViewOpen, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub txtMcharge_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMcharge_AMT.TextChanged

    End Sub

    Private Sub gridViewOpen_KeyDown(sender As Object, e As KeyEventArgs) Handles gridViewOpen.KeyDown
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            If gridViewOpen.Rows.Count > 0 Then
                strSql = vbCrLf + " SELECT A.TRANNO,A.TRANDATE"
                strSql += vbCrLf + " , (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =  A.ACCODE) SMITHNAME"
                strSql += vbCrLf + " , (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = A.CATCODE) CATNAME"
                strSql += vbCrLf + " , A.BATCHNO,A.BAGNO,A.PURITY"
                strSql += vbCrLf + " , A.GRSWT,A.NETWT,A.LESSWT"
                strSql += vbCrLf + " , A.MELTWT AS SAMPLEWT"
                strSql += vbCrLf + " , A.DUSTWT AS SCRAPWT"
                strSql += vbCrLf + " , A.KAFTTOUCH AS TOUCHPER"
                strSql += vbCrLf + " , A.PUREWT"
                strSql += vbCrLf + " , A.RATE,A.BOARDRATE"
                strSql += vbCrLf + " , A.MCHARGE,A.WASTAGE "
                strSql += vbCrLf + " , A.AMOUNT,A.REMARK1,A.REMARK2"
                strSql += vbCrLf + " , COUNT(*) CNT"
                strSql += vbCrLf + " , A.OTHERAMT"
                strSql += vbCrLf + " ,(SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID= A.COMPANYID) COMPANYNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS A"
                strSql += vbCrLf + "  ," & cnStockDb & "..MELTINGDETAIL AS B"
                strSql += vbCrLf + "  WHERE A.BATCHNO = B.BATCHNO And "
                strSql += vbCrLf + "  A.TRANTYPE = 'RRE' "
                strSql += vbCrLf + "  And A.BATCHNO = '" & gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                strSql += vbCrLf + "  GROUP BY A.TRANNO,A.TRANDATE,A.ACCODE,A.CATCODE,A.BATCHNO"
                strSql += vbCrLf + " ,A.BAGNO,A.PURITY,A.GRSWT,A.NETWT,A.LESSWT,A.MELTWT,A.DUSTWT,A.KAFTTOUCH,A.PUREWT"
                strSql += vbCrLf + " ,A.RATE,A.BOARDRATE,A.MCHARGE,A.WASTAGE,A.AMOUNT,A.REMARK1,A.REMARK2,A.OTHERAMT,A.COMPANYID"
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If Val(dt.Rows(0).Item("CNT").ToString) = 1 Then
                        dtpDate.Value = dt.Rows(0).Item("TRANDATE")

                        txtBagNo.Text = dt.Rows(0).Item("BAGNO").ToString
                        cmbReceivedCat.Text = dt.Rows(0).Item("CATNAME").ToString

                        txtPurity_PER.Text = dt.Rows(0).Item("PURITY").ToString
                        txtLess_WET.Text = dt.Rows(0).Item("LESSWT").ToString

                        txtWeight_WET.Text = dt.Rows(0).Item("OTHERAMT").ToString
                        txtWastage_WET.Text = dt.Rows(0).Item("WASTAGE").ToString

                        txtReceivedWt_WET.Text = dt.Rows(0).Item("GRSWT").ToString
                        txtTouchper.Text = dt.Rows(0).Item("TOUCHPER").ToString

                        txtSampleWt_WET.Text = dt.Rows(0).Item("SAMPLEWT").ToString
                        txtScrapwt_WET.Text = dt.Rows(0).Item("SCRAPWT").ToString
                        txtNetWt_WET.Text = dt.Rows(0).Item("NETWT").ToString

                        txtRate_AMT.Text = dt.Rows(0).Item("RATE").ToString
                        txtPurewt_WET.Text = dt.Rows(0).Item("PUREWT").ToString

                        txtMcharge_AMT.Text = dt.Rows(0).Item("MCHARGE").ToString
                        txtRemark1.Text = dt.Rows(0).Item("REMARK1").ToString
                        txtRemark2.Text = dt.Rows(0).Item("REMARK2").ToString
                        flagupdateBatchno = dt.Rows(0).Item("BATCHNO").ToString
                        flagupdateTranno = dt.Rows(0).Item("TRANNO").ToString
                        flagupdate = True
                        dtpDate.Enabled = False
                        CmbCompany_OWN.Text = dt.Rows(0).Item("COMPANYNAME").ToString
                        cmbSmith_OWN.Text = dt.Rows(0).Item("SMITHNAME").ToString
                        txtBagNo.Focus()
                        tabMain.SelectedTab = tabGeneral

                    Else
                        MsgBox("more than one cann't edit", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                Else
                    MsgBox("No Record found", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Private Sub txtScrapwt_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtScrapwt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcNetWt()
        End If
    End Sub
End Class