Imports System.Data.OleDb
Imports System.Math
Public Class frmEchallan
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtbankname As New DataTable
    Dim cndefaultaccode As Boolean = False
    Dim cnaccode As String
    Dim dt As New DataTable
    dim cnbankname As String
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewToolStripMenuItem.Click
        If dtpFrom.Value > dtpTo.Value Then MsgBox("Invalid Date", MsgBoxStyle.Information) : dtpTo.Focus() : Exit Sub
        Me.Cursor = Cursors.WaitCursor
        AutoReziseToolStripMenuItem.Checked = False
        gridView.DataSource = Nothing
        gridView.Refresh()
        If gridView.Columns.Contains("TRANSFER") Then gridView.Columns.Remove("TRANSFER")
        strSql = "IF OBJECT_ID('MASTER..TEMPECHALLEN') IS NOT NULL DROP TABLE MASTER..TEMPECHALLEN"
        strSql += vbCrLf + "SELECT  ROW_NUMBER() OVER (ORDER BY HIERARCHYCODE) AS SLNO"
        strSql += vbCrLf + ",SNO,CONVERT(VARCHAR,ENTRYDATE,105)ENTRYDATE"
        strSql += vbCrLf + ",CHQNO,BANK,BRANCH,SUM(AMOUNT) AS AMOUNT, HIERARCHYCODE "
        strSql += vbCrLf + ",CODE1,'' AS COLHEAD,'1' AS RESULT "
        strSql += vbCrLf + "INTO MASTER..TEMPECHALLEN  "
        strSql += vbCrLf + "FROM ("
        strSql += vbCrLf + "SELECT  CONVERT(VARCHAR(15),A.SNO)SNO,A.TRANDATE AS ENTRYDATE, A.CHQCARDNO AS CHQNO,A.CHQCARDREF AS BANK "
        strSql += vbCrLf + ",CONVERT(VARCHAR,NULL) AS BRANCH,A.AMOUNT AS AMOUNT "
        strSql += vbCrLf + ",(SELECT COMPID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID= A.COSTID) AS HIERARCHYCODE "
        strSql += vbCrLf + ",A.COSTID AS CODE1"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN A "
        strSql += vbCrLf + "INNER  JOIN  " & cnAdminDb & "..ACHEAD H ON H.ACCODE=A.ACCODE"
        strSql += vbCrLf + "WHERE A.TRANMODE='D' AND PAYMODE='CH' "
        strSql += vbCrLf + "AND A.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND H.ACNAME='" & cmbBankname.Text & "'  "
        strSql += vbCrLf + "AND ISNULL(CHALLANNO,0)=0"
        strSql += vbCrLf + ")AS X GROUP BY ENTRYDATE,"
        strSql += vbCrLf + "CHQNO,BANK,BRANCH,HIERARCHYCODE,CODE1,SNO"
        strSql += vbCrLf + "ORDER BY HIERARCHYCODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "IF (SELECT COUNT(*) FROM MASTER..TEMPECHALLEN)>0"
        strSql += vbCrLf + "BEGIN"
        strSql += vbCrLf + "	INSERT INTO MASTER..TEMPECHALLEN(ENTRYDATE,CODE1,AMOUNT,RESULT,COLHEAD)"
        strSql += vbCrLf + "	SELECT DISTINCT 'SUB TOTAL',CODE1,SUM(AMOUNT),2,'S'"
        strSql += vbCrLf + "	FROM MASTER..TEMPECHALLEN GROUP BY HIERARCHYCODE,CODE1"
        strSql += vbCrLf + "	INSERT INTO MASTER..TEMPECHALLEN(ENTRYDATE,CODE1,AMOUNT,RESULT,COLHEAD)"
        strSql += vbCrLf + "	SELECT DISTINCT 'TOTAL','ZZ',SUM(AMOUNT),3,'G'"
        strSql += vbCrLf + "	FROM MASTER..TEMPECHALLEN WHERE RESULT=2 "
        strSql += vbCrLf + "END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "SELECT * FROM MASTER..TEMPECHALLEN ORDER BY CODE1,RESULT"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            Me.Cursor = Cursors.Default
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        If dtGrid.Rows.Count > 0 Then
            gridView.DataSource = dtGrid
            Dim colCheckbox As New DataGridViewCheckBoxColumn()
            colCheckbox.Width = 50
            colCheckbox.Name = "TRANSFER"
            gridView.Columns.Add(colCheckbox)
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("CODE1").Visible = False
            Dim Dr() As DataRow = dtGrid.Select("RESULT=3")
            If Dr.Length > 0 Then
                TxtAmount.Text = Dr(0).Item("AMOUNT").ToString
            End If
            FillGridGroupStyle_KeyNoWise(gridView)
            GridViewFormat()
            funcGridViewStyle()
            AutoReziseToolStripMenuItem.Checked = True
            AutoReziseToolStripMenuItem_Click(Me, New EventArgs)
            btnView.Enabled = False
            btnTransfer.Focus()
        Else
            gridView.DataSource = Nothing
            gridView.Refresh()
            Me.Cursor = Cursors.Default
            MsgBox("Record Not Found", MsgBoxStyle.Information)
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Function funcGridViewStyle() As Integer
        With gridView
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                If gridView.ColumnCount - 1 <> cnt Then .Columns(cnt).ReadOnly = True
            Next
            If .Columns.Contains("SLNO") Then
                With .Columns("SLNO")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("CHQNO") Then
                With .Columns("CHQNO")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If

            If .Columns.Contains("BANK") Then
                With .Columns("BANK")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("BRANCH") Then
                With .Columns("BRANCH")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function
    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView.Rows
            With dgvView
                Select Case .Cells("RESULT").Value
                    Case 1
                        .Cells("TRANSFER").Value = CheckState.Checked
                    Case 2
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case 3
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub frmEchallan_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmEchallan_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim datatable As New DataTable
        Dim SqlAd As New OleDbDataAdapter
        strSql = " SELECT ACNAME AS BANKNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACTYPE='B' "
        dtbankname = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtbankname)
        BrighttechPack.GlobalMethods.FillCombo(cmbBankname, dtbankname, "BANKNAME", True)
        Txtslipno.Text = Val(GetBillControlValue("CHALLANNO", tran)) + 1
        dtpFrom.Focus()
        strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='CHQDEPOSITDATE'"
        If objGPack.GetSqlValue(strSql, "CNT", 0) = 0 Then
            strSql = "CREATE TABLE " & cnStockDb & "..CHQDEPOSITDATE(SLIPNO INT,DEPOSITDATE SMALLDATETIME,USERID INT,UPDATED SMALLDATETIME)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click, ReportToolStripMenuItem.Click
        If Val(txtViewSlipNo_NUM.Text) = 0 Then MsgBox("Slip No not Empty", MsgBoxStyle.Information) : txtViewSlipNo_NUM.Focus() : Exit Sub
        strSql = "IF OBJECT_ID('" & cnStockDb & "..TEMPECHALLEN') IS NOT NULL DROP TABLE " & cnStockDb & "..TEMPECHALLEN"
        strSql += vbCrLf + "SELECT  ROW_NUMBER() OVER (ORDER BY HIERARCHYCODE) AS SLNO"
        strSql += vbCrLf + ",CONVERT(VARCHAR,ENTRYDATE,105)ENTRYDATE"
        strSql += vbCrLf + ",CHQNO,BANK,BRANCH,SUM(AMOUNT) AS AMOUNT, HIERARCHYCODE "
        strSql += vbCrLf + ",CODE1,'' AS COLHEAD,'1' AS RESULT "
        strSql += vbCrLf + "FROM ("
        strSql += vbCrLf + "SELECT  CONVERT(VARCHAR(15),A.SNO)SNO,A.TRANDATE AS ENTRYDATE, A.CHQCARDNO AS CHQNO,A.CHQCARDREF AS BANK "
        strSql += vbCrLf + ",CONVERT(VARCHAR,NULL) AS BRANCH,A.AMOUNT AS AMOUNT "
        strSql += vbCrLf + ",(SELECT COMPID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID= A.COSTID)AS HIERARCHYCODE"
        strSql += vbCrLf + ",A.COSTID as CODE1"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN A "
        strSql += vbCrLf + "INNER  JOIN  " & cnAdminDb & "..ACHEAD H ON H.ACCODE=A.ACCODE"
        strSql += vbCrLf + "WHERE A.TRANMODE='D' AND PAYMODE='CH' "
        strSql += vbCrLf + "AND ISNULL(CHALLANNO,0)=" & Val(txtViewSlipNo_NUM.Text)
        strSql += vbCrLf + ")AS X GROUP BY ENTRYDATE,"
        strSql += vbCrLf + "CHQNO,BANK,BRANCH,HIERARCHYCODE,CODE1"
        strSql += vbCrLf + "ORDER BY HIERARCHYCODE"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count = 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            txtViewSlipNo_NUM.Clear()
            txtViewSlipNo_NUM.Focus()
            Exit Sub
        End If
        Dim objEchellan As New DataSetEChellan
        Dim row As DataSetEChellan.ListEChellanRow
        For i As Integer = 0 To dt.Rows.Count - 1
            row = objEchellan.ListEChellan.NewRow
            row.SLNO = Val(dt.Rows(i).Item("SLNO").ToString)
            row.BANK = dt.Rows(i).Item("BANK").ToString
            row.CHQNO = dt.Rows(i).Item("CHQNO").ToString
            row.BRANCH = dt.Rows(i).Item("BRANCH").ToString
            row.AMOUNT = Val(dt.Rows(i).Item("AMOUNT").ToString)
            row.ENTRYDATE = dt.Rows(i).Item("ENTRYDATE").ToString
            row.HIERARCHYCODE = dt.Rows(i).Item("HIERARCHYCODE").ToString
            objEchellan.ListEChellan.AddListEChellanRow(row)
        Next
        Dim objrepcompany As New CrystalDecisions.Shared.ParameterValues
        Dim objrepcompany1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepBank As New CrystalDecisions.Shared.ParameterValues
        Dim objrepBank1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepAcno As New CrystalDecisions.Shared.ParameterValues
        Dim objrepAcno1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepAcType As New CrystalDecisions.Shared.ParameterValues
        Dim objrepAcType1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepDate As New CrystalDecisions.Shared.ParameterValues
        Dim objrepDate1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepDepositDate As New CrystalDecisions.Shared.ParameterValues
        Dim objrepDepositDate1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepAmount As New CrystalDecisions.Shared.ParameterValues
        Dim objrepAmount1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepAmountinwords As New CrystalDecisions.Shared.ParameterValues
        Dim objrepAmountinwords1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepSlipno As New CrystalDecisions.Shared.ParameterValues
        Dim objrepSlipno1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepUser As New CrystalDecisions.Shared.ParameterValues
        Dim objrepUser1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        strSql = "SELECT BANKNAME,BANKACNO,ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
        strSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'')='' AND CHALLANNO=" & Val(txtViewSlipNo_NUM.Text) & " )"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objrepBank1.Value = dt.Rows(0).Item("ACNAME").ToString
            objrepAcno1.Value = dt.Rows(0).Item("BANKACNO").ToString
            objrepAcType1.Value = dt.Rows(0).Item("BANKNAME").ToString
        End If
        objrepDate1.Value = objGPack.GetSqlValue("SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'')='' AND CHALLANNO=" & Val(txtViewSlipNo_NUM.Text), "TRANDATE")
        objrepDepositDate1.Value = objGPack.GetSqlValue("SELECT CONVERT(VARCHAR,DEPOSITDATE,105)DEPOSITDATE FROM " & cnStockDb & "..CHQDEPOSITDATE WHERE SLIPNO=" & Val(txtViewSlipNo_NUM.Text), "DEPOSITDATE").ToString
        objrepAmount1.Value = objGPack.GetSqlValue("SELECT SUM(AMOUNT)AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'')='' AND CHALLANNO=" & Val(txtViewSlipNo_NUM.Text), "AMOUNT")
        objrepcompany1.Value = objGPack.GetSqlValue("SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & cnCompanyId & "'", "COMPANYNAME")
        objrepAmountinwords1.Value = ConvertRupees.RupeesToWord(objrepAmount1.Value)
        objrepSlipno1.Value = txtViewSlipNo_NUM.Text
        objrepUser1.Value = cnUserName

        objrepcompany.Add(objrepcompany1)
        objrepBank.Add(objrepBank1)
        objrepAcno.Add(objrepAcno1)
        objrepAcType.Add(objrepAcType1)
        objrepDate.Add(objrepDate1)
        objrepDepositDate.Add(objrepDepositDate1)
        objrepAmount.Add(objrepAmount1)
        objrepAmountinwords.Add(objrepAmountinwords1)
        objrepSlipno.Add(objrepSlipno1)
        objrepUser.Add(objrepUser1)
        Dim objRPT_Echallan As New RPT_Echallan
        objRPT_Echallan.SetDataSource(objEchellan)
        objRPT_Echallan.SetParameterValue("rptCompany", objrepcompany)
        objRPT_Echallan.SetParameterValue("rptBank", objrepBank)
        objRPT_Echallan.SetParameterValue("rptAcno", objrepAcno)
        objRPT_Echallan.SetParameterValue("rptActype", objrepAcType)
        objRPT_Echallan.SetParameterValue("rptDate", objrepDate)
        objRPT_Echallan.SetParameterValue("rptDepositDate", objrepDepositDate)
        objRPT_Echallan.SetParameterValue("rptAmount", objrepAmount)
        objRPT_Echallan.SetParameterValue("rptWords", objrepAmountinwords)
        objRPT_Echallan.SetParameterValue("rptSlipno", objrepSlipno)
        objRPT_Echallan.SetParameterValue("rptUser", objrepUser)
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objRPT_Echallan
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If MessageBox.Show("Sure U Want to Transfer.?", "Deposit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
            funcTransfer()
        End If
    End Sub
    Private Function funcTransfer()
        Dim TranNo As Integer
        Dim flag As Boolean = False
        Try
            tran = Nothing
            tran = cn.BeginTransaction
GenBillNo:
            TranNo = Val(GetBillControlValue("CHALLANNO", tran))
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
            strSql += " WHERE CTLID = 'CHALLANNO' AND COMPANYID = '" & strCompanyId & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If Not cmd.ExecuteNonQuery() > 0 Then
                GoTo GenBillNo
            End If
GenBillNo1:
            TranNo += 1
            strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..ACCTRAN WHERE CHALLANNO=" & TranNo
            If objGPack.GetSqlValue(strSql, "CNT", , tran) > 0 Then
                GoTo GenBillNo1
            End If
            For I As Integer = 0 To gridView.Rows.Count - 1
                With gridView
                    If CType(.Rows(I).Cells("TRANSFER").Value, Boolean) = False Then
                        Continue For
                    End If
                    Dim Sno As String = .Rows(I).Cells("SNO").Value.ToString
                    strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET CHALLANNO=" & TranNo & " WHERE SNO='" & Sno & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    flag = True
                End With
            Next
            If flag Then
                strSql = "INSERT INTO " & cnStockDb & "..CHQDEPOSITDATE(SLIPNO,DEPOSITDATE,USERID,UPDATED)"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + "" & TranNo
                strSql += vbCrLf + ",'" & Format(dtpDepositDate.Value, "yyyy-MM-dd") & "'"
                strSql += vbCrLf + "," & userId
                strSql += vbCrLf + ",'" & Format(Now, "yyyy-MM-dd HH:mm:sss") & "'"
                strSql += vbCrLf + " )"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            tran.Commit()
            tran = Nothing
            If flag Then MsgBox("Slip No " & TranNo & " Created" & vbCrLf & "Cheque Deposited Successfully.", MsgBoxStyle.Information)
            flag = False
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub cmbBankname_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBankname.Leave
        If cmbBankname.Text.Trim = "" Then
            MsgBox("Bank Name Should Not Empty.", MsgBoxStyle.Information)
            cmbBankname.Select()
            Exit Sub
        End If
        strSql = "SELECT BANKACNO FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbBankname.Text & "'"
        TxtAcno.Text = objGPack.GetSqlValue(strSql, "BANKACNO", "")
        strSql = "SELECT BANKNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbBankname.Text & "'"
        TxtActype.Text = objGPack.GetSqlValue(strSql, "BANKNAME", "")
    End Sub

    Private Sub TxtAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtAmount.TextChanged
        If TxtAmount.Text.Trim = "" Then Exit Sub
        LblWords.Text = ConvertRupees.RupeesToWord(TxtAmount.Text)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub funcNew()
        btnView.Enabled = True
        strSql = " SELECT ACNAME AS BANKNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACTYPE='B' "
        dtbankname = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtbankname)
        BrighttechPack.GlobalMethods.FillCombo(cmbBankname, dtbankname, "BANKNAME", True)
        Txtslipno.Text = Val(GetBillControlValue("CHALLANNO", tran)) + 1
        dtpFrom.Focus()
        LblWords.Text = ""
        TxtAcno.Clear()
        TxtActype.Clear()
        TxtAmount.Clear()
        txtViewSlipNo_NUM.Clear()
        dtpFrom.Value = Now
        dtpTo.Value = Now
        dtpDepositDate.Value = Now
        If gridView.Columns.Contains("TRANSFER") Then gridView.Columns.Remove("TRANSFER")
        gridView.DataSource = Nothing
        gridView.Refresh()
    End Sub
End Class