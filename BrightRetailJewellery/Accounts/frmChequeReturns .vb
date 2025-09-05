Imports System.Data.OleDb

Public Class frmChequeReturns
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtpGridFlag As Boolean
    Dim preTranDb As String
    Dim curTranDb As String
    Dim objSearch As Object
    Dim dtCostCentre As New DataTable
    Dim EditMode As Boolean = False
    Dim commonTranDb As New ArrayList()
    Dim dtGrid As New DataTable()
    Dim FindSearchFormat As Integer = IIf(GetAdmindbSoftValue("FINDSEARCH", "O") = "N", 1, -1)
    Dim objRemark As New frmBillRemark

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim tDate As Date = cnTranToDate  'Convert.ToDateTime(GetServerDate())
        If tDate.Month > 3 Then
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 1, 3, 2) + Mid(tDate.Year, 3, 2)
        Else
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 2, 3, 2) + Mid(tDate.Year - 1, 3, 2)
        End If
        strSql = " Select DBNAME from " & cnAdminDb & "..DBMASTER "
        Dim dtName As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtName)
        If dtName.Rows.Count > 0 Then
            For i As Integer = 0 To dtName.Rows.Count - 1
                commonTranDb.Add(dtName.Rows(i).Item("DBNAME"))
            Next
        End If

        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & preTranDb & "'").Length > 0 Then
            preTranDb = Nothing
        End If
    End Sub

    Private Sub LoadAcname()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE actype  = 'B' "
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        cmbBank_MAN.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbBank_MAN, False, False)
        cmbBank_MAN.SelectedIndex = 0
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbBank_MAN.Text = "ALL"
        gridView_OWN.DataSource = Nothing
        dtpTranDateFrom.Value = GetServerDate()
        dtpTranDateTo.Value = GetServerDate()
        dtpChequeDateFrom.Value = GetServerDate()
        dtpChequeDateTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        LoadAcname()
        cmbBank_MAN.Select()
    End Sub

    Private Sub frmBankReconciliation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub

    Private Sub frmBankReconciliation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBankReconciliation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView_OWN.RowTemplate.Height = 21
        dtpChequeDateFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpChequeDateFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpChequeDateTo.MinimumDate = (New DateTimePicker).MinDate
        dtpChequeDateTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpTranDateFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpTranDateFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTranDateTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTranDateTo.MaximumDate = (New DateTimePicker).MaxDate
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
        CheckBox1_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtView As New DataTable
        strSql = "IF OBJECT_ID('TEMPDB..ACRECONS') IS NOT NULL DROP TABLE TEMPDB..ACRECONS"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        Dim dtDb As New DataTable
        strSql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER ORDER BY ENDDATE DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)

        strSql = vbCrLf + "  SELECT * INTO TEMPDB..ACRECONS FROM ( "
        For k As Integer = 0 To dtDb.Rows.Count - 1
            If k > 1 Then Exit For
            Dim _cnStockDb As String = dtDb.Rows(k)("DBNAME").ToString
            If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & _cnStockDb & "'").Length > 0 Then
                Continue For
            End If
            If k > 0 Then strSql += vbCrLf + "  UNION ALL "
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE,TRANNO"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "  WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "  WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "  WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT"
            strSql += vbCrLf + "  ,(SELECT TOP 1 TRANDATE FROM " & _cnStockDb & "..ACCTRAN WHERE REFDATE=T.TRANDATE AND REFNO=convert(varchar(7),T.TRANNO) AND CHQCARDNO=T.CHQCARDNO) RELIASEDATE"
            strSql += vbCrLf + "  ,SNO,BATCHNO,TRANDATE,'" & _cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "  SELECT TOP 1 SNO FROM " & _cnStockDb & "..BRSINFO WHERE  TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
            strSql += vbCrLf + "  AND ACCODE = T.ACCODE AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "  AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'') AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "  AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'') AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "  AND COMPANYID = T.COMPANYID AND AMOUNT = T.AMOUNT),''))BRSSNO"
            strSql += vbCrLf + "  ,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  FROM " & _cnStockDb & "..ACCTRAN AS T WHERE "
            If chkTranDate.Checked Then
                strSql += vbCrLf + "TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
            Else
                strSql += vbCrLf + "CHQDATE BETWEEN '" & dtpChequeDateFrom.Value.Date & "' AND '" & dtpChequeDateTo.Value.Date & "'"
            End If
            strSql += vbCrLf + "  AND RELIASEDATE IS NULL AND ISNULL(REFDATE,'')='' AND ISNULL(REFNO,'')='' "
            If cmbBank_MAN.Text <> "ALL" And cmbBank_MAN.Text <> "" Then
                strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            Else
                strSql += vbCrLf + "  AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = 2)"
            End If
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            If txtChqNo.Text <> "" Then
                strSql += vbCrLf + "AND CHQCARDNO ='" & txtChqNo.Text & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        Next
        strSql += vbCrLf + " )X "

        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        'strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
        'strSql += vbCrLf + " FROM TEMPDB..ACRECONS AS T"
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
        'strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & cnStockDb & "'"
        'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        'strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = 'SCHEME'"
        'strSql += vbCrLf + " WHERE FROMFLAG = 'C'"
        'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPDB..ACRECONS "
        If chkTranDate.Checked Then
            strSql += vbCrLf + " ORDER BY TRANDATE "
        Else
            strSql += vbCrLf + " ORDER BY CHQDATE "
        End If
        ' da = New OleDbDataAdapter(strSql, cn)
        ' da.Fill(dtView)


        da = New OleDbDataAdapter(strSql, cn)
        dtGrid.Clear()
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpTranDateFrom.Select()
            Exit Sub
        End If
        gridView_OWN.DataSource = dtGrid
        gridView_OWN.Columns("ACCODE").HeaderText = "BANK NAME"
        gridView_OWN.Columns("RELIASEDATE").HeaderText = "RETURN DATE"
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTranDate.CheckedChanged
        If chkTranDate.Checked Then

            dtpTranDateFrom.Enabled = True
            Label5.Enabled = True
            dtpTranDateTo.Enabled = True
            Label7.Enabled = False
            dtpChequeDateFrom.Enabled = False
            Label8.Enabled = False
            dtpChequeDateTo.Enabled = False
        Else

            dtpTranDateFrom.Enabled = False
            Label5.Enabled = False
            dtpTranDateTo.Enabled = False
            Label7.Enabled = True
            dtpChequeDateFrom.Enabled = True
            Label8.Enabled = True
            dtpChequeDateTo.Enabled = True
        End If
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If e.Control Then
            If e.KeyCode = Keys.F Then
                'objSearch.Show() 
                If objSearch IsNot Nothing Then
                    objSearch = Nothing
                End If
                objSearch = New frmGridSearch(gridView_OWN, Val(FindSearchFormat))
                objSearch.Show()

            ElseIf e.KeyCode = Keys.S Then
                If objSearch IsNot Nothing Then
                    CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
                End If
            End If
        End If
        If e.KeyCode = Keys.N Then
            If objSearch IsNot Nothing Then
                CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
            End If
        End If
        If e.KeyCode = Keys.R Then
            e.Handled = True
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
            'ElseIf e.KeyCode = Keys.Delete Then
            'If gridView_OWN.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
            'If MessageBox.Show("Sure you want to unrealize?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            '    UpdateUnRealise()
            'End If
        End If
    End Sub

    Private Sub dtpGridRealise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpGridRealise.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dtpGridRealise.Visible = False
            dtpGridFlag = False
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
            If dtpGridRealise.Value.Date < CType(gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANDATE").Value, Date) Then
                MsgBox("Invalid Date" + vbCrLf + "Return date must greater than trandate", MsgBoxStyle.Information)

                Exit Sub
            End If
            UpdateReturnchq()
            gridView_OWN.Select()
        ElseIf UCase(e.KeyChar) = Chr(Keys.E) Or UCase(e.KeyChar) = Chr(Keys.C) Then
            gridView_KeyPress(dtpGridRealise, e)
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
        If UCase(e.KeyChar) = Chr(Keys.R) Then
            Dim pt As Point = gridView_OWN.Location
            strSql = " SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN WHERE "
            strSql += vbCrLf + " REFNO ='" & gridView_OWN.CurrentRow.Cells("TRANNO").Value & "' AND REFDATE='" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
            strSql += vbCrLf + " AND CHQCARDNO = '" & gridView_OWN.CurrentRow.Cells("CHQCARDNO").Value.ToString & "'" 'Newly Add
            If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                MsgBox("Already Returned", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtpGridRealise.Visible = True
            pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("RELIASEDATE").Index, gridView_OWN.CurrentRow.Index, False).Location
            If gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString <> "" Then
                dtpGridRealise.MinDate = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
                dtpGridRealise.Value = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
            Else
                dtpGridRealise.MinDate = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
                dtpGridRealise.Value = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
            End If
            dtpGridRealise.Location = pt
            dtpGridFlag = True
            dtpGridRealise.Focus()
            'ElseIf UCase(e.KeyChar) = Chr(Keys.C) Then
            '    Dim pt As Point = gridView_OWN.Location
            '    dtpGridRealise.Visible = True
            '    pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("RELIASEDATE").Index, gridView_OWN.CurrentRow.Index, False).Location
            '    dtpGridRealise.MinDate = System.DateTime.Now.ToString()
            '    dtpGridRealise.Value = System.DateTime.Now.ToString()
            '    dtpGridRealise.Location = pt
            '    dtpGridFlag = True
            '    dtpGridRealise.Focus()
        End If
    End Sub

    Private Sub UpdateReturnchq()
        Try
            objRemark = New frmBillRemark
            objRemark.Text = "Cheque Return Remark"
            objRemark.cmbRemark1_OWN.SelectAll()
            objRemark.cmbRemark1_OWN.Text = "CHEQUE RETURN"
            If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            Dim Tranno As Integer
            strSql = " SELECT TYPE,DISPLAYTEXT,SNO  FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = 'JE'"
            Dim dritem As DataRow = GetSqlRow(strSql, cn)
            Dim type As String = dritem.Item(0)
            Dim dispText As String = dritem.Item(1)
            Dim mnuId As String = dritem.Item(2)

            Dim ctrlId As String = "GEN-JE"
            strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID = '" & ctrlId & "'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"

            If Not objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox(dispText & " Billno Generation Controlid Not Found", MsgBoxStyle.Information)
            End If
            Dim Isfirst As Boolean = True
GenBillNo:
            Tranno = Val(GetBillControlValue(ctrlId, tran, Not Isfirst))
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Tranno + 1 & "'"
            strSql += " WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & Tranno & "'"
            If strBCostid <> Nothing And Isfirst Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If Not cmd.ExecuteNonQuery() > 0 Then
                Isfirst = False
                GoTo GenBillNo
            End If
            Tranno += 1
            Dim BatchNo As String = GetNewBatchno(cnCostId, dtpGridRealise.Value.ToString("yyyy-MM-dd"), tran)
            Dim vouchType As Integer = 2

            Dim Chqretcode As String = objGPack.GetSqlValue("select ctltext from " & cnAdminDb & "..Softcontrol where ctlid = 'CHQRET_ACCODE'", , "CHQRET").ToString
            If gridView_OWN.CurrentRow.Cells("CONTRA").Value.ToString <> "" Then Chqretcode = gridView_OWN.CurrentRow.Cells("CONTRA").Value.ToString
            tran = Nothing
            tran = cn.BeginTransaction
            With gridView_OWN.CurrentRow
                InsertintoCustomerinfo(.Cells("BATCHNO").Value.ToString, BatchNo)
                If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO='" & .Cells("BATCHNO").Value.ToString & "'", "", "", tran)) > 0 Then
                    InsertIntoOustanding(.Cells("BATCHNO").Value.ToString, Tranno, BatchNo, Val(.Cells("AMOUNT").Value.ToString))
                Else
                    InsertIntoOustandingNew(.Cells("BATCHNO").Value.ToString, Tranno, BatchNo, Val(.Cells("AMOUNT").Value.ToString), Tranno, .Cells("COSTID").Value.ToString)
                End If
                InsertIntoAccTran(BatchNo, Tranno, dtpGridRealise.Value.Date.ToString("yyyy-MM-dd"), "C", .Cells("Accode").Value.ToString, Val(.Cells("AMOUNT").Value.ToString), "JE", _
                                Chqretcode, cnCostId, .Cells("TRANNO").Value.ToString, .Cells("TRANDATE").Value.ToString, .Cells("CHQCARDNO").Value.ToString, _
                                .Cells("CHQDATE").Value.ToString, .Cells("CARDID").Value.ToString, .Cells("CHQCARDREF").Value.ToString, objRemark.cmbRemark1_OWN.Text, objRemark.cmbRemark2_OWN.Text)

                InsertIntoAccTran(BatchNo, Tranno, dtpGridRealise.Value.Date.ToString("yyyy-MM-dd"), "D", Chqretcode, Val(.Cells("AMOUNT").Value.ToString), "JE", _
                                .Cells("Accode").Value.ToString, cnCostId, .Cells("TRANNO").Value.ToString, .Cells("TRANDATE").Value.ToString, .Cells("CHQCARDNO").Value.ToString, _
                                .Cells("CHQDATE").Value.ToString, .Cells("CARDID").Value.ToString, .Cells("CHQCARDREF").Value.ToString, objRemark.cmbRemark1_OWN.Text, objRemark.cmbRemark2_OWN.Text)

            End With

            'strSql = " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..ACCTRAN SET RELIASEDATE = '" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
            'strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
            'strSql += vbCrLf + "  AND ENTREFNO = '" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value & "'"
            'strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
            'strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"

            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.CurrentRow.Cells("COSTID").Value.ToString)
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            gridView_OWN.CurrentRow.Cells("RELIASEDATE").Value = dtpGridRealise.Value.Date

        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub


    Private Sub InsertIntoAccTran _
  (ByVal Batchno As String, ByVal TNo As Integer, _
  ByVal Trandate As String, _
  ByVal TranMode As String, _
  ByVal Accode As String, _
  ByVal Amount As Double, _
  ByVal PayMode As String, _
  ByVal Contra As String, _
  ByVal Costid As String, _
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
  Optional ByVal chqCardNo As String = Nothing, _
  Optional ByVal chqDate As String = Nothing, _
  Optional ByVal chqCardId As Integer = Nothing, _
  Optional ByVal chqCardRef As String = Nothing, _
  Optional ByVal Remark1 As String = Nothing, _
  Optional ByVal Remark2 As String = Nothing, _
  Optional ByVal fLAG As String = Nothing, _
  Optional ByVal SAccode As String = "" _
  )
        If Amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & TNo & "" 'TRANNO 
        strSql += " ,'" & Trandate & "'" 'TRANDATE
        strSql += " ,'" & TranMode & "'" 'TRANMODE
        strSql += " ,'" & Accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT

        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & PayMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'A'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & Costid & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, Costid)
        strSql = ""
        cmd = Nothing
    End Sub
    Private Sub InsertIntoOustanding(ByVal BATCHNO As String, ByVal newTranno As Integer, ByVal newbatchno As String, ByVal chqamt As Double)
        strSql = " SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO='" & BATCHNO & "' AND ISNULL(CANCEL,'')=''"
        Dim Dtoutst As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(Dtoutst)
        For k As Integer = 0 To Dtoutst.Rows.Count - 1
            Dim Paymode As String = ""
            Dim Trantype As String = ""
            If Dtoutst.Rows(k).Item("RECPAY").ToString = "R" Then
                If Dtoutst.Rows(k).Item("PAYMODE").ToString = "DR" Then
                    Paymode = "DP"
                ElseIf Dtoutst.Rows(k).Item("PAYMODE").ToString = "AR" Or Dtoutst.Rows(k).Item("PAYMODE").ToString = "OR" Then
                    Paymode = "AP"
                    Trantype = "A"
                ElseIf Dtoutst.Rows(k).Item("PAYMODE").ToString = "MR" Then
                    Paymode = "MP"
                Else
                    Paymode = Dtoutst.Rows(k).Item("PAYMODE").ToString
                End If
            Else
                If Dtoutst.Rows(k).Item("PAYMODE").ToString = "DP" Then
                    Paymode = "DR"
                ElseIf Dtoutst.Rows(k).Item("PAYMODE").ToString = "AP" Then
                    Paymode = "AR"
                    Trantype = "A"
                ElseIf Dtoutst.Rows(k).Item("PAYMODE").ToString = "MP" Then
                    Paymode = "MR"
                Else
                    Paymode = Dtoutst.Rows(k).Item("PAYMODE").ToString
                End If
            End If
            Dim recpay As String = IIf(Dtoutst.Rows(k).Item("RECPAY").ToString = "R", "P", "R")
            If chqamt = 0 Then chqamt = Math.Abs(Val(Dtoutst.Rows(k).Item("AMOUNT").ToString))
            If Val(Dtoutst.Rows(k).Item("AMOUNT").ToString) = 0 And Val(Dtoutst.Rows(k).Item("GRSWT").ToString) = 0 And Val(Dtoutst.Rows(k).Item("PUREWT").ToString) = 0 Then Exit Sub
            strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY,REFNO,REFDATE,EMPID,TRANSTATUS"
            strSql += " ,PURITY,CATCODE,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,RATE,ADVFIXWTPER,VALUE,CASHID,REMARK1"
            strSql += " ,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE,SETTLEBATCHNO)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            strSql += " ," & newTranno & "" 'TRANNO
            strSql += " ,'" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            If Trantype <> "" Then
                strSql += " ,'" & Trantype & "'" 'TRANTYPE
            Else
                strSql += " ,'D'" '" & Dtoutst.Rows(k).Item("TRANTYPE").ToString & "'" 'TRANTYPE
            End If
            strSql += " ,'" & Dtoutst.Rows(k).Item("RUNNO").ToString & "'" 'RUNNO
            strSql += " ," & chqamt & "" 'AMOUNT
            strSql += " ," & Math.Abs(Val(Dtoutst.Rows(k).Item("GRSWT").ToString)) & "" 'GRSWT
            strSql += " ," & Math.Abs(Val(Dtoutst.Rows(k).Item("NETWT").ToString)) & "" 'NETWT
            strSql += " ," & Math.Abs(Val(Dtoutst.Rows(k).Item("PUREWT").ToString)) & "" 'PUREWT
            strSql += " ,'P'" '" & recpay & "'" 'RECPAY
            strSql += " ,''" 'REFNO
            strSql += " ,NULL" 'REFDATE
            strSql += " ,0" 'EMPID
            strSql += " ,'" & Dtoutst.Rows(k).Item("TRANSTATUS").ToString & "'" 'TRANSTATUS
            strSql += " ," & Val(Dtoutst.Rows(k).Item("PURITY").ToString) & "" 'PURITY
            strSql += " ,'" & Dtoutst.Rows(k).Item("CATCODE").ToString & "'" 'CATCODE
            strSql += " ,'" & newbatchno & "'" 'BATCHNO
            strSql += " ," & userId & "" 'USERID

            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ," & Val(Dtoutst.Rows(k).Item("RATE").ToString) & "" 'RATE
            strSql += " ," & Val(Dtoutst.Rows(k).Item("ADVFIXWTPER").ToString) & "" 'ADVFIXWTPER
            strSql += " ," & Val(Dtoutst.Rows(k).Item("VALUE").ToString) & "" 'VALUE
            strSql += " ,'" & Dtoutst.Rows(k).Item("CASHID").ToString & "'" 'CASHID
            strSql += " ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
            strSql += " ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK1
            strSql += " ,'" & Dtoutst.Rows(k).Item("ACCODE").ToString & "'" 'ACCODE
            strSql += " ," & Val(Dtoutst.Rows(k).Item("CTRANCODE").ToString) & "" 'CTRANCODE
            strSql += " ,NULL" 'DUEDATE
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & Dtoutst.Rows(k).Item("COSTID").ToString & "'" 'COSTID
            strSql += " ,'P'" 'FROMFLAG
            strSql += " ,'" & Dtoutst.Rows(k).Item("FLAG").ToString & "'" 'FLAG FOR ORDER ADVANCE REPAY
            strSql += " ,'" & Paymode & "'" 'PAYMODE
            strSql += " ,'" & Dtoutst.Rows(k).Item("SETTLEBATCHNO").ToString & "'" 'SETTLEBATCHNO
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, Dtoutst.Rows(k).Item("COSTID").ToString)
        Next
    End Sub
    Private Sub InsertIntoOustandingNew(ByVal BATCHNO As String _
            , ByVal newTranno As Integer, ByVal newbatchno As String _
            , ByVal chqamt As Double, ByVal TranNo As Integer, ByVal CostId As String)

        Dim Runno As String
XX:
        Runno = GetCostId(CostId) & GetCompanyId(strCompanyId) & "D" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + TranNo.ToString
        strSql = " SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & Runno & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            TranNo += 1
            GoTo XX
        End If

        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO,AMOUNT,GRSWT"
        strSql += " ,NETWT,PUREWT,RECPAY,REFNO,REFDATE,EMPID"
        strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,REMARK1"
        strSql += " ,REMARK2,ACCODE,DUEDATE,APPVER"
        strSql += " ,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & newTranno & "" 'TRANNO
        strSql += " ,'" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'D'" 'TRANTYPE
        strSql += " ,'" & Runno & "'" 'RUNNO
        strSql += " ," & chqamt & "" 'AMOUNT
        strSql += " ,0"  'GRSWT
        strSql += " ,0"  'NETWT
        strSql += " ,0"  'PUREWT
        strSql += " ,'P'" 'RECPAY
        strSql += " ,''" 'REFNO
        strSql += " ,NULL" 'REFDATE
        strSql += " ,0" 'EMPID
        strSql += " ,'" & newbatchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
        strSql += " ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK1
        strSql += " ,'DRS'" 'ACCODE
        strSql += " ,NULL" 'DUEDATE
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,'P'" 'FROMFLAG
        strSql += " ,'DU'"  'PAYMODE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
    End Sub
    Private Sub InsertintoCustomerinfo(ByVal Oldbatchno As String, ByVal Newbatchno As String)
        strSql = " SELECT * FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & Oldbatchno & "'"
        Dim DtCusInfo As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtCusInfo)
        If DtCusInfo.Rows.Count > 0 Then
            strSql = " IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & Newbatchno & "')>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
            strSql += vbCrLf + " (BATCHNO,PSNO,REMARK1,COSTID,PAN,DUEDATE)VALUES"
            strSql += vbCrLf + " ('" & Newbatchno & "','" & DtCusInfo.Rows(0).Item("PSNO").ToString & "','" & DtCusInfo.Rows(0).Item("REMARK1").ToString & "','" & DtCusInfo.Rows(0).Item("COSTID").ToString & "','" & DtCusInfo.Rows(0).Item("PAN").ToString & "',NULL)"
            strSql += vbCrLf + " END"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, DtCusInfo.Rows(0).Item("COSTID").ToString)
        End If
    End Sub

    Private Sub gridView_OWN_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellContentClick

    End Sub
End Class