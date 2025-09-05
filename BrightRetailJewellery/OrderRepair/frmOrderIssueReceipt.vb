Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class frmOrderIssueReceipt
    Dim dtGridView As New DataTable
    Dim strSql As String
    'Dim chkCol As DataGridViewCheckBoxColumn
    Dim fillGrid As Boolean
    Dim tran As OleDbTransaction
    Dim cmd As OleDbCommand
    Dim objReceiptDet As New frmOR_ReceiptDia
    Dim CostId As String
    Dim PdfMail As Boolean = IIf(GetAdmindbSoftValue("PDFMAIL_SMITH", "N") = "Y", True, False)
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim notify_Email As NotifyIcon

    Private Sub frmOrderIssueReceipt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If gridView.Focused Then
                rbtIssue.Focus()
            End If
        End If
    End Sub

    Private Sub frmOrderIssueReceipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmOrderIssueReceipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        ''LOAD COSTCENTRE
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        Else
            cmbCostCentre_MAN.Enabled = False
        End If
        ''Load Designer
        strSql = " SELECT DESIGNERNAME FROM " & CNADMINDB & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN, False, False)

        ''Load ORDERSTATUS
        strSql = "SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ISNULL(SMITH,'')='Y' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, cmbProcess, False, False)

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkCol_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        MsgBox(CType(sender, CheckBox).Checked.ToString)
    End Sub


    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        Select Case gridView.Columns(gridView.CurrentCell.ColumnIndex).Name
            Case "DESIGNERDESCRIPT"
                Dim tb As TextBox = CType(e.Control, TextBox)
                tb.MaxLength = 100
                tb.CharacterCasing = CharacterCasing.Normal
        End Select
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        gridView.DataSource = Nothing
        dtGridView = New DataTable
        Dim dtCol As New DataColumn("CHKCOL", GetType(Boolean))
        dtCol.DefaultValue = chkAll.Checked
        dtGridView.Columns.Add(dtCol)
        Dim designerId As Integer = Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"))
        CostId = cnCostId
        If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
            CostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
        End If
        fillGrid = False
        strSql = " SELECT" + vbCrLf
        strSql += " SUBSTRING(ORNO,6,20)ORNO, ORDATE, DUEDATE, DESCRIPT, PCS, GRSWT,NETWT" + vbCrLf
        strSql += " ,(SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))AS PNAME" + vbCrLf
        strSql += " ,CONVERT(VARCHAR(50),'')AS DESIGNERDESCRIPT" + vbCrLf
        strSql += " ,SNO,BATCHNO,COSTID,'" & IIf(rbtIssue.Checked, "I", "R") & "' AS TYPE" + vbCrLf
        strSql += " ,CONVERT(VARCHAR(8),NULL)RTAGNO,CONVERT(INT,NULL)RPCS,CONVERT(NUMERIC(15,3),NULL)RGRSWT,CONVERT(NUMERIC(15,3),NULL)RNETWT,CONVERT(NUMERIC(15,3),NULL)RWASTAGE,CONVERT(NUMERIC(15,2),NULL)AS RMC" + vbCrLf
        strSql += " ,COMPANYID,ITEMID,CONVERT(NUMERIC(15,3),NULL) RDUSTWT,CONVERT(NUMERIC(15,3),NULL) REXCESSWT"
        strSql += " FROM " & cnadmindb & "..ORMAST AS O " + vbCrLf
        If rbtIssue.Checked Then
            'strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSNO = O.SNO  AND ISNULL(CANCEL,'') = '')" + vbCrLf
            strSql += " WHERE (SELECT ISNULL(SUM(CASE WHEN ORSTATUS='I' THEN ISNULL(GRSWT,0) ELSE -1*(ISNULL(GRSWT,0)) END ),0) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO=O.SNO AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID IN(2,3) )=CONVERT(DECIMAL(15,3),0)"
        Else
            strSql += " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'I'  AND ISNULL(CANCEL,'') = '' AND DESIGNERID = " & designerId & " AND ORDSTATE_ID<>54)" + vbCrLf
            strSql += " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'') = '' AND DESIGNERID = " & designerId & " AND ORDSTATE_ID<>54)" + vbCrLf
        End If
        strSql += " AND ISNULL(ODBATCHNO,'') = ''" + vbCrLf
        strSql += " AND ISNULL(ORDCANCEL,'') = ''" + vbCrLf
        If txtOrderNo.Text <> "" Then
            strSql += " AND ORNO = '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrderNo.Text & "'" + vbCrLf
        End If
        If cmbCostCentre_MAN.Enabled Then
            If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
                strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')" + vbCrLf
            End If
        End If
        If chkOrderDate.Checked Then
            strSql += " AND ORDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'" + vbCrLf
        End If
        If rbtOrder.Checked Then
            strSql += " AND ORTYPE = 'O'" + vbCrLf
        Else
            strSql += " AND ORTYPE = 'R'" + vbCrLf
        End If
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            If cmbCostCentre_MAN.Enabled Then cmbCostCentre_MAN.Select() Else chkOrderDate.Select()
        End If
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            With .Columns("CHKCOL")
                .Width = 20
                .HeaderText = ""
                .ReadOnly = False
            End With
            With .Columns("ORNO")
                .Width = 70
                .ReadOnly = True
            End With
            With .Columns("ORDATE")
                .Width = 80
                .ReadOnly = True
            End With
            With .Columns("DUEDATE")
                .Width = 80
                .ReadOnly = True
            End With
            With .Columns("DESCRIPT")
                .Width = 200
                .ReadOnly = True
            End With
            With .Columns("PCS")
                .Width = 40
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NETWT")
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            '' .Columns("NETWT").Visible = False
            .Columns("RDUSTWT").Visible = False
            .Columns("REXCESSWT").Visible = False
            With .Columns("PNAME")
                .HeaderText = "PARTY NAME"
                .Width = 163
                .ReadOnly = True
            End With
            With .Columns("DESIGNERDESCRIPT")
                .HeaderText = "DESIGNER DESCRIPTION"
                .Width = 280
                .ReadOnly = False
            End With
            With .Columns("RTAGNO")
                .HeaderText = "REC TAGNO"
                .Visible = Not rbtIssue.Checked
                .Width = 70
                .ReadOnly = True
            End With
            With .Columns("RPCS")
                .HeaderText = "REC PCS"
                .Visible = Not rbtIssue.Checked
                .Width = 40
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RGRSWT")
                .HeaderText = "REC GRSWT"
                .Visible = Not rbtIssue.Checked
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("RNETWT")
                .HeaderText = "REC NETWT"
                .Visible = Not rbtIssue.Checked
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("RWASTAGE")
                .HeaderText = "REC WASTAGE"
                .Visible = Not rbtIssue.Checked
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("RMC")
                .HeaderText = "REC MCHARGE"
                .Visible = Not rbtIssue.Checked
                .Width = 70
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            .Columns("TYPE").Visible = False
            .Columns("SNO").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("COMPANYID").Visible = False
            .Columns("ITEMID").Visible = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        'If chkAll.Checked Then
        '    For cnt As Integer = 0 To dtGridView.Rows.Count - 1
        '        gridView.Rows(cnt).Cells(0).Value = chkAll.Checked
        '    Next
        'End If
        fillGrid = True
        gridView.CurrentCell = gridView.FirstDisplayedCell
        gridView.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtGridView.Rows.Clear()
        cmbCostCentre_MAN.Text = ""
        chkOrderDate.Checked = False
        pnlDate.Enabled = False
        rbtOrder.Checked = True
        txtOrderNo.Clear()
        dtGridView.Clear()
        rbtIssue.Checked = True
        cmbDesigner_MAN.Text = ""
        txtManualNo_NUM.Text = ""
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        rbtReceipt_CheckedChanged(Me, New EventArgs)
        chkManualNo_CheckedChanged(Me, New EventArgs)
        If Not ORDER_MULTI_MIMR Then
            cmbProcess.Visible = False
            lblProcess.Visible = False
        Else
            cmbProcess.Visible = True
            lblProcess.Visible = True
        End If
        If cmbCostCentre_MAN.Enabled Then
            cmbCostCentre_MAN.Focus()
        Else
            chkOrderDate.Focus()
        End If
    End Sub


    Private Sub chkOrderDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOrderDate.CheckStateChanged
        If chkOrderDate.Checked Then
            pnlDate.Enabled = True
        Else
            pnlDate.Enabled = False
        End If
    End Sub

    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        For Each dgvRow As DataGridViewRow In gridView.Rows
            dgvRow.Cells("CHKCOL").Value = chkAll.Checked
        Next
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If MessageBox.Show("Sure you want to transfer the selected items", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If chkManualNo.Checked Then
            If Val(txtManualNo_NUM.Text) = 0 Then
                MessageBox.Show("Please enter Billno")
                txtManualNo_NUM.Focus()
                Exit Sub
            End If
            strSql = " SELECT 1 CNTROW FROM " & cnadmindb & "..ORIRDETAIL WHERE ISNULL(TRANNO,'') = '" & txtManualNo_NUM.Text & "'"
            strSql += " AND ISNULL(ORSTATUS,'') = '" & IIf(rbtIssue.Checked, "I", "R") & "'"
            Dim dtManualNo As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtManualNo)
            If dtManualNo.Rows.Count > 0 Then
                If MessageBox.Show("Billno already exits, Do you want allow it?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    txtManualNo_NUM.Focus()
                    Exit Sub
                End If
            End If
        End If

        Dim tranNo As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim batchNo As String = Nothing
        Dim saveFlag As Boolean = False
        If CheckEntryDate(GetServerDate) Then Exit Sub
        ''Validation
        If Not gridView.RowCount > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            btnSearch.Focus()
            Exit Sub
        End If


        strSql = " SELECT DESIGNERID,ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
        Dim desrow As DataRow = GetSqlRow(strSql, cn)
        designerId = Val(desrow.Item(0).ToString)
        Dim Propsmith As String = desrow.Item(1).ToString

        Try
            Dim rowSelected() As DataRow = Nothing
            rowSelected = dtGridView.Select("CHKCOL = 'TRUE'")
            If Not rowSelected.Length > 0 Then
                MsgBox("Select record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            batchNo = GetNewBatchno(cnCostId, GetEntryDate(GetServerDate(tran), tran), tran)
            If chkManualNo.Checked = True Then
                tranNo = txtManualNo_NUM.Text
            Else
                ''Find TranNo
                strSql = " SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                tranNo = Val(objGPack.GetSqlValue(strSql, , , tran))
            End If

            For Each ro As DataRow In rowSelected
                strSql = " INSERT INTO " & cnadmindb & "..ORIRDETAIL"
                strSql += " ("
                strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,WASTAGE,MC,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE,DUSTWT,EXCESSWT"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                strSql += " ,'" & ro.Item("SNO").ToString & "'" 'ORSNO
                strSql += " ," & tranNo & "" 'TRANNO
                strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'TRANDATE
                strSql += " ," & designerId & "" 'DESIGNERID
                If ro.Item("TYPE").ToString = "I" Then
                    strSql += " ," & Val(ro.Item("PCS").ToString) & "" 'PCS
                    strSql += " ," & Val(ro.Item("GRSWT").ToString) & "" 'GRSWT
                    strSql += " ," & Val(ro.Item("NETWT").ToString) & "" 'NETWT
                    strSql += " ,0" 'WASTAGE
                    strSql += " ,0" 'MC
                    strSql += " ,''" 'TAGNO
                    strSql += " ,'" & ro.Item("TYPE").ToString & "'" 'ORSTATUS
                Else 'RECEIPT
                    strSql += " ," & Val(ro.Item("RPCS").ToString) & "" 'PCS
                    strSql += " ," & Val(ro.Item("RGRSWT").ToString) & "" 'GRSWT
                    strSql += " ," & Val(ro.Item("RNETWT").ToString) & "" 'NETWT
                    strSql += " ," & Val(ro.Item("RWASTAGE").ToString) & "" 'MC
                    strSql += " ," & Val(ro.Item("RMC").ToString) & "" 'MC
                    strSql += " ,'" & ro.Item("RTAGNO").ToString & "'" 'TAGNO
                    strSql += " ,'" & ro.Item("TYPE").ToString & "'" 'ORSTATUS
                End If
                strSql += " ,''" 'CANCEL
                strSql += " ,'" & ro.Item("COSTID").ToString & "'" 'COSTID
                strSql += " ,'" & Mid(ro.Item("DESIGNERDESCRIPT").ToString, 1, 100) & "'" 'DESCRIPT
                strSql += " ,'" & GetCostId(ro.Item("COSTID").ToString) & GetCompanyId(strCompanyId) & ro.Item("ORNO").ToString & "'" 'ORNO
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & ro.Item("COMPANYID").ToString & "'" 'COMPANYID
                strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                If ORDER_MULTI_MIMR Then
                    strSql += " ," & Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME='" + cmbProcess.Text + "'", , , tran).ToString) & "" 'ORDSTATE_ID ISSUE TO SMITH
                Else
                    If ro.Item("TYPE").ToString = "I" Then
                        strSql += " ,2" 'ORDSTATE_ID ISSUE TO SMITH
                    Else
                        strSql += " ,3" 'ORDSTATE_ID RECEIVE FROM SMITH
                    End If
                End If
                strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(ro.Item("ITEMID").ToString) & "", , , tran) & "'" 'CATCODE
                If ro.Item("TYPE").ToString = "I" Then
                    strSql += " ,0" 'DUSTWT
                    strSql += " ,0" 'EXCESSWT
                Else
                    strSql += " ," & Val(ro.Item("RDUSTWT").ToString) & "" 'DUSTWT
                    strSql += " ," & Val(ro.Item("REXCESSWT").ToString) & "" 'EXCESSWT
                End If
                strSql += " )"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, ro.Item("COSTID").ToString)
                Dim HeadCostId As String = BrighttechPack.GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'", "COSTID", "", tran)
                If ro.Item("COSTID").ToString <> HeadCostId Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, HeadCostId, , , , False)
                End If
                If Propsmith <> "" Then
                    strSql = "UPDATE " & cnAdminDb & "..ORMAST SET PROPSMITH='" & Propsmith & "' where Sno = '" & ro.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, ro.Item("COSTID").ToString)
                End If
            Next

            ''UPDATING
            If chkManualNo.Checked = False Then
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            tran.Commit()
            tran = Nothing
            MsgBox("Successfully Transfered..")

            Dim pBatchno As String = batchNo
            Dim pBillDate As Date = GetEntryDate(GetServerDate)

            If GetAdmindbSoftValue("PRN_ORIR", "N") = "Y" Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":OIR")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":OIR" & ";" & _
                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" & _
                        LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";" & _
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If

            If PdfMail And rbtIssue.Checked Then
                Dim dsOrRecIss As New DataSet
                'Dim OrDataset As New OrdeIssRec
                Dim EmailId As String = objGPack.GetSqlValue("SELECT EMAILID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID='" & designerId & "')")
                Dim DesignerName As String = objGPack.GetSqlValue("SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID='" & designerId & "'")
                Dim DefaultPicPath As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'OR_PICPATH'")
                If Not DefaultPicPath.EndsWith("\") Then DefaultPicPath = DefaultPicPath + "\"
                Dim Image As String = ""
                strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPORISSUE','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPORISSUE"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = "CREATE TABLE TEMPTABLEDB..TEMPORISSUE(SNO VARCHAR(20),TRANNO VARCHAR(10),TRANDATE SMALLDATETIME,DESCRIPTION VARCHAR(100),PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),IMAGEPATH VARCHAR(300),TMPIMG VARBINARY(MAX),DESIGNERNAME VARCHAR(50))"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                Dim rowSelect() As DataRow = Nothing
                rowSelect = dtGridView.Select("CHKCOL = 'TRUE'")
                For Each ro As DataRow In rowSelect
                    Image = objGPack.GetSqlValue("SELECT PICTFILE FROM " & cnadmindb & "..ORMAST WHERE SNO='" & ro.Item("SNO").ToString & "'")
                    strSql = "INSERT INTO TEMPTABLEDB..TEMPORISSUE("
                    strSql += "SNO,TRANNO,TRANDATE,DESCRIPTION,PCS,GRSWT,NETWT,IMAGEPATH,DESIGNERNAME)VALUES("
                    strSql += " '" & ro.Item("SNO").ToString & "'" 'ORSNO
                    strSql += " ," & tranNo & "" 'TRANNO
                    strSql += " ,'" & Format(GetEntryDate(GetServerDate(tran), tran), "yyyy-MM-dd") & "'" 'TRANDATE
                    strSql += " ,'" & Mid(ro.Item("DESCRIPT").ToString, 1, 100) & "'" 'DESCRIPT
                    strSql += " ," & Val(ro.Item("PCS").ToString) & "" 'PCS
                    strSql += " ," & Val(ro.Item("GRSWT").ToString) & "" 'GRSWT
                    strSql += " ," & Val(ro.Item("NETWT").ToString) & "" 'NETWT
                    strSql += " ,'" & DefaultPicPath + Image & "'" 'NETWT
                    strSql += " ,'" & DesignerName & "')" 'DESIGNERNAME
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                Next
                Dim dtImage As New DataTable
                strSql = " SELECT SNO,IMAGEPATH IMAGE FROM TEMPTABLEDB..TEMPORISSUE"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtImage)
                If dtImage.Rows.Count > 0 Then
                    For Each ro As DataRow In dtImage.Rows
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = ro!IMAGE.ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            If IO.Directory.Exists(Finfo.Directory.FullName) Then
                                Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                                Dim reader As New IO.BinaryReader(fileStr)
                                Dim reslt As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                fileStr.Read(reslt, 0, reslt.Length)
                                fileStr.Close()
                                strSql = " UPDATE TEMPTABLEDB..TEMPORISSUE SET TMPIMG = ? WHERE SNO = '" & ro!SNO.ToString & "'"
                                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                                cmd.Parameters.AddWithValue("@image", reslt)
                                cmd.ExecuteNonQuery()
                            End If
                        End If
                    Next
                    'strSql = " SELECT TRANNO,TRANDATE,DESCRIPTION,PCS,GRSWT,NETWT,DESIGNERNAME,IMAGEPATH IMAGE,CONVERT(VARBINARY(MAX),TMPIMG)TMPIMG FROM MASTER..TEMPORISSUE"
                    'da = New OleDbDataAdapter(strSql, cn)
                    'dtImage = New DataTable
                    'da.Fill(dtImage)
                    'For j As Integer = 0 To dtImage.Rows.Count - 1
                    '    Dim recRow As DataRow = OrDataset.Tables(0).NewRow()
                    '    recRow(0) = dtImage.Rows(j).Item("TRANNO")
                    '    recRow(1) = dtImage.Rows(j).Item("TRANDATE")
                    '    recRow(2) = dtImage.Rows(j).Item("DESCRIPTION")
                    '    recRow(3) = dtImage.Rows(j).Item("PCS")
                    '    recRow(4) = dtImage.Rows(j).Item("GRSWT")
                    '    recRow(5) = dtImage.Rows(j).Item("NETWT")
                    '    recRow(6) = dtImage.Rows(j).Item("DESIGNERNAME")
                    '    'recRow(7) = dtImage.Rows(0).Item("TMPIMG")
                    '    Dim fileDestPath As String = dtImage.Rows(j).Item("IMAGE")
                    '    If (System.IO.File.Exists(fileDestPath)) Then
                    '        Dim fs As FileStream
                    '        Dim br As BinaryReader
                    '        fs = New FileStream(fileDestPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                    '        br = New BinaryReader(fs)
                    '        Dim imgbyte() As Byte
                    '        imgbyte = br.ReadBytes(CType(fs.Length, Integer))
                    '        recRow(7) = imgbyte
                    '    End If
                    '    OrDataset.Tables(0).Rows.Add(recRow)
                    'Next

                    Dim objRpt As New rptOrdIss
                    'objRpt.SetDataSource(OrDataset)
                    Dim objReport As New GiritechReport
                    Dim objRptViewer As New frmReportViewer
                    objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptOrdIss, cnDataSource)
                    notify_Email = New NotifyIcon()
                    notify_Email.Text = "Creating Pdf for " + DesignerName
                    notify_Email.Visible = True
                    notify_Email.Icon = My.Resources.email
                    notify_Email.ShowBalloonTip(20000, "Information", "Creating Pdf for " + DesignerName, ToolTipIcon.Info)
                    Dim CrExportOptions As ExportOptions

                    Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                    Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
                    Dim filePath As String
                    filePath = Application.StartupPath
                    If IO.File.Exists(filePath + "\Image.pdf") Then IO.File.Delete(filePath + "\Image.pdf")
                    CrDiskFileDestinationOptions.DiskFileName = filePath + "\Image.pdf"
                    CrFormatTypeOptions.UsePageRange = False
                    CrExportOptions = objRpt.ExportOptions
                    With CrExportOptions
                        .ExportDestinationType = ExportDestinationType.DiskFile
                        .ExportFormatType = ExportFormatType.PortableDocFormat
                        .DestinationOptions = CrDiskFileDestinationOptions
                        .FormatOptions = CrFormatTypeOptions
                    End With
                    objRpt.Export()
                    objRpt.Close()
                    If EmailId <> "" Then
                        notify_Email.Text = "Email is sending to " + DesignerName
                        notify_Email.ShowBalloonTip(20000, "Email notification", "Email is sending to " + DesignerName, ToolTipIcon.Info)
                        NEWMAILSEND(EmailId, "Dear Sir/Madam,<br/><br/>&nbsp;&nbsp;<br/><br/><br/><br/>Pdf Receipt for Order Issue.&nbsp;<br/><br/>Please find the Attachment . <br/><br/>", filePath + "\Image.pdf")
                        notify_Email.ShowBalloonTip(5000, "Email notification", "Email successfully send to " + DesignerName, ToolTipIcon.Info)
                    End If
                    notify_Email.Visible = False
                    notify_Email.Dispose()
                End If
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            notify_Email.Visible = False
            notify_Email.Dispose()
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox("MESSAGE    :" + ex.Message + vbCrLf + "STACKTRACE  :" + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        If fillGrid = False Then Exit Sub
        If Not gridView.Columns.Count > 0 Then Exit Sub
        If Not gridView.Rows.Count > 0 Then Exit Sub
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        If fillGrid = False Then Exit Sub
        If Not gridView.Columns.Count > 0 Then Exit Sub
        If Not gridView.Rows.Count > 0 Then Exit Sub
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "CHKCOL"
                With gridView.Rows(e.RowIndex)
                    If .Cells("TYPE").Value.ToString <> "R" Then Exit Sub
                    If CType(gridView.CurrentCell.Value, Boolean) Then
                        objReceiptDet.Text = .Cells("ORNO").Value.ToString + " Receipt Detail"
                        objGPack.TextClear(objReceiptDet)
                        objReceiptDet.txtRnetWt_WET.Text = .Cells("NETWT").Value.ToString
                        If objReceiptDet.ShowDialog = Windows.Forms.DialogResult.OK Then
                            .Cells("RTAGNO").Value = objReceiptDet.txtTagNo.Text
                            .Cells("RPCS").Value = Val(objReceiptDet.txtPcs_NUM.Text)
                            .Cells("RGRSWT").Value = Val(objReceiptDet.txtGrsWt_WET.Text)
                            .Cells("RNETWT").Value = Val(objReceiptDet.txtNetWt_WET.Text)
                            .Cells("RWASTAGE").Value = Val(objReceiptDet.txtWastage_WET.Text)
                            .Cells("RMC").Value = Val(objReceiptDet.txtMc_AMT.Text)
                            .Cells("RDUSTWT").Value = Val(objReceiptDet.txtDustWt_WET.Text)
                            .Cells("REXCESSWT").Value = Val(objReceiptDet.txtExcessWt_WET.Text)
                        End If
                    Else
                        .Cells("RTAGNO").Value = DBNull.Value
                        .Cells("RPCS").Value = DBNull.Value
                        .Cells("RGRSWT").Value = DBNull.Value
                        .Cells("RNETWT").Value = DBNull.Value
                        .Cells("RWASTAGE").Value = DBNull.Value
                        .Cells("RMC").Value = DBNull.Value
                        .Cells("RDUSTWT").Value = DBNull.Value
                        .Cells("REXCESSWT").Value = DBNull.Value
                    End If
                End With
        End Select
    End Sub


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked Then
            chkAll.Visible = False
            chkAll.Checked = False
            chkManualNo.Text = "Manual Receipt No"
        Else
            chkAll.Visible = True
            chkManualNo.Text = "Manual Issue No"
        End If
    End Sub

    Private Sub chkManualNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManualNo.CheckedChanged
        txtManualNo_NUM.Visible = chkManualNo.Checked
    End Sub

    Public Function NEWMAILSEND(ByVal ToMail As String, ByVal MESSAGE As String, Optional ByVal Attachpath As String = "")
        If ToMail.Trim = "" Then Return 0 : Exit Function
        If MESSAGE.Trim = "" Then Return 0 : Exit Function
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0

        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Try
            FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            If FromId.Contains("@") = False Or FromId.Contains(".") = False Then
                FromId = objGPack.GetSqlValue("SELECT CTLNAME FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            End If
            Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'")
            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If

            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = "Order Issue"
            mail.Body = MESSAGE
            mail.IsBodyHtml = True
            If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function

    Private Sub txtOrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','V') IS NOT NULL DROP VIEW TEMP" & systemId & "ORMAST"
            strSql += vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','U') IS NOT NULL DROP TABLE TEMP" & systemId & "ORMAST"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            Dim ORDREP As String
            If rbtOrder.Checked Then ORDREP = "O" Else ORDREP = "R"
            strSql = vbCrLf + " CREATE VIEW TEMP" & systemId & "ORMAST"
            strSql += vbCrLf + " AS"
            strSql += vbCrLf + "  SELECT O.ORNO"
            strSql += vbCrLf + "  ,O.ITEMID"
            strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEM"
            strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID AND ITEMID = O.ITEMID)AS SUBITEM"
            strSql += vbCrLf + "  ,(SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = O.ORNO AND ORSNO = O.SNO AND ISNULL(ORSTATUS,'') = 'R' AND ISNULL(CANCEL,'') = '')AS TAGNO"
            strSql += vbCrLf + "  ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.ORRATE,O.SNO"
            strSql += vbCrLf + "  ,O.BATCHNO,EMPID,O.DISCOUNT"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"
            strSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = ''"
            strSql += vbCrLf + " AND ORTYPE = '" & ORDREP & "' AND ISNULL(CANCEL,'') = ''"
            If cmbCostCentre_MAN.Text <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "') "
            End If
            strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))AS NAME"
            strSql += vbCrLf + " ,SUBSTRING(ORNO,6,20)ORNO,ITEMID"
            strSql += vbCrLf + " ,ITEM,SUBITEM,TAGNO"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,RATE,ORRATE,SNO,EMPID,O.DISCOUNT"
            strSql += vbCrLf + " FROM TEMP" & systemId & "ORMAST AS O"
            Dim msgTit As String
            If ORDREP = "R" Then msgTit = "Select RepairItem" Else msgTit = "Select OrderItem"
            Dim OrderRow As DataRow
            OrderRow = BrighttechPack.SearchDialog.Show_R(msgTit, strSql, cn, , , , , , False)
            If Not OrderRow Is Nothing Then
                txtOrderNo.Text = OrderRow.Item("ORNO").ToString
            End If
        End If
    End Sub
End Class