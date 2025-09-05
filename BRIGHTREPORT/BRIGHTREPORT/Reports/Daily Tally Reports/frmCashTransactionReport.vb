Imports System.Data.OleDb
Public Class frmCashTransactionReport
    'CALID 061112-1 : CLIENT JMT: ALTER BY SATHYA
    'calno 061112-1 sathya client JMT - option for acc entry/order entry/to be entry
    Dim dtCashTran As New DataTable
    Dim strItemFilter As String = Nothing
    Dim strSql As String
    Dim cmd As OleDbCommand
    Public dtFilteration As New DataTable
    Dim SelectedCompany As String
    Public FormReSize As Boolean = True
    Public FormReLocation As Boolean = True
    Dim objMonthSummary, objLedgerSummary As New frmGridDispDia

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'chkCompanySelectAll.Checked = False
        btnNew.Enabled = False
        funcNew()
        If cmbCashTransactionType.Items.Count > 0 Then
            cmbCashTransactionType.SelectedIndex = 0
        End If
        If cmbMCashTransactionType.Items.Count > 0 Then
            cmbMCashTransactionType.SelectedIndex = 0
        End If
        ' txtNodeId.Text = ""

        'cmbOrderBy.Items.Clear()
        'cmbOrderBy.Items.Add("BILLDATE")
        'cmbOrderBy.Items.Add("BILLNO")
        'cmbOrderBy.SelectedIndex = 0
        rbtNormal.Checked = True
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Select()
        btnNew.Enabled = True
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub frmCashTransactionReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmCashTransactionReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCompany(chkLstCompany)
        LoadCostName(chkCmbCostcentre)
        funcAddTransactionType()
        Dim dtacc As New DataTable
        strSql = " SELECT 'ALL' NAME,'' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME NAME,ACCODE ACCODE,2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strSql += " ORDER BY RESULT,NAME"
        Dim dtItemType As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacc)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcchead, dtacc, "NAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Function funcNew() As Integer
        Try
            dtCashTran.Clear()
            lblTitle.Text = "TITLE"
            strSql = "select ''BILLNO,''BILLDATE,''TRANNAME,''RECEIPT,''PAYMENT,"
            strSql += "''PARTICULARS,''REMARKS,''SYSTEMID,''ADDRESS,''BATCHNO,''USERNAME,''RESULT WHERE 1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCashTran)
            gridView_OWN.DataSource = dtCashTran
            funcGridStyle()
            lblTitle.Height = gridView_OWN.ColumnHeadersHeight
            txtRemark.Text = ""
            txtNodeId.Text = ""
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView_OWN
            .Columns("BATCHNO").Visible = False
            .Columns("RESULT").Visible = False
            With .Columns("BILLNO")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("BILLDATE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANNAME")
                .Width = 140
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RECEIPT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PAYMENT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PARTICULARS")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("REMARKS")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("USERNAME")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SYSTEMID")
                .Width = 52
                .HeaderText = "SYSTEMID"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADDRESS")
                .Width = 250
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        'Call function for add datas into Grid GridCashTransaction
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        funcAddData()
        Prop_Sets()
    End Sub

    Function funcAddTransactionType() As Integer
        strSql = "SELECT DISTINCT PRONAME FROM " & cnAdminDb & "..PROCESSTYPE WHERE ISNULL(PROMODULE,'') = 'P' ORDER BY PRONAME"
        cmbCashTransactionType.Items.Clear()
        cmbCashTransactionType.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCashTransactionType, False, False)
        cmbCashTransactionType.Text = "ALL"

        strSql = "SELECT DISTINCT PRONAME FROM " & cnAdminDb & "..PROCESSTYPE WHERE PROID IN (SELECT UPROID FROM " & cnAdminDb & "..PROCESSTYPE) ORDER BY PRONAME"
        cmbMCashTransactionType.Items.Clear()
        cmbMCashTransactionType.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMCashTransactionType, False, False)
        cmbMCashTransactionType.Text = "ALL"

    End Function

    Function funcAddData() As Integer
        Try
            btnView_Search.Enabled = False
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            Dim selectedaccode As String = GetSelectedAccountId(chkCmbAcchead, True)
            Dim selectedCostId As String = GetSelectedCostId(chkCmbCostcentre, False)
            dtCashTran.Clear()
            lblTitle.Text = "TITLE"
            Me.Refresh()
            ''cmd.CommandType = CommandType.StoredProcedure
            If selectedCostId = "''" Then
                selectedCostId = ""
            End If
            strSql = " EXECUTE " & cnStockDb & "..SP_RPT_CASHTRANSACTION"
            '061112-1
            strSql += vbCrLf + "  @DBNAME = '" & cnAdminDb & "'"
            '061112-1
            strSql += vbCrLf + "  ,@DATEFROM = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,@DATETO ='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,@REMARK='" & txtRemark.Text & "'"
            strSql += vbCrLf + "  ,@NODEID='" & Replace(txtNodeId.Text, "'", "''''") & "'"
            strSql += vbCrLf + "  ,@CASHTRAN='" & Replace(cmbCashTransactionType.Text, "'", "''''") & "'"
            strSql += vbCrLf + "  ,@MCASHTRAN='" & Replace(cmbMCashTransactionType.Text, "'", "''''") & "'"
            strSql += vbCrLf + "  ,@SystemId='" & systemId & "'"
            strSql += vbCrLf + "  ,@COMPANYID='" & SelectedCompany & "'"
            strSql += vbCrLf + "  ,@ACCODE=" & selectedaccode & ""
            strSql += vbCrLf + "  ,@COSTID='" & selectedCostId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'Final Query 
            strSql = " select ISNULL(CONVERT(VARCHAR,TRANNO),'')BILLNO"
            strSql += vbCrLf + " ,ISNULL(CONVERT(VARCHAR,TRANDATE,103),'') BILLDATE"
            strSql += vbCrLf + " ,ISNULL(TRANNAME,'')TRANNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,RECEIPT) RECEIPT"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,PAYMENT) PAYMENT"
            strSql += vbCrLf + " ,ISNULL(PARTICULARS,'')PARTICULARS"
            strSql += vbCrLf + " ,ISNULL(REMARKS,'')REMARKS"
            strSql += vbCrLf + " ,ISNULL(SYSTEMID,'')SYSTEMID"
            strSql += vbCrLf + " ,ISNULL(ADDRESS,'')ADDRESS"
            strSql += vbCrLf + " ,ISNULL(BATCHNO,'')BATCHNO"
            strSql += vbCrLf + " ,ISNULL(USERNAME,'')USERNAME"
            strSql += vbCrLf + " ,ISNULL(RESULT,'')RESULT"
            strSql += vbCrLf + "  from TEMP" & systemId & "CASHTRAN "
            strSql += vbCrLf + "  ORDER BY MTRANNAME,RESULT,BATCHNO,TRANDATE,TRANNO"
            dtCashTran = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCashTran)
            If dtCashTran.Rows.Count < 1 Then
                MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnView_Search.Enabled = True
                btnView_Search.Focus()
                Exit Function
            End If

            strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CASHTRANSACTION')"
            strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "CASHTRANSACTION"
            strSql += vbCrLf + "  CREATE TABLE TEMP" & systemId & "CASHTRANSACTION("
            strSql += vbCrLf + "  BILLNO INT,"
            strSql += vbCrLf + "  BILLDATE SMALLDATETIME,"
            strSql += vbCrLf + "  MTRANNAME VARCHAR(50),"
            strSql += vbCrLf + "  TRANNAME VARCHAR(50),"
            strSql += vbCrLf + "  RECEIPT NUMERIC(15,2),"
            strSql += vbCrLf + "  PAYMENT NUMERIC(15,2),"
            strSql += vbCrLf + "  PARTICULARS VARCHAR(50),"
            strSql += vbCrLf + "  REMARKS VARCHAR(100),"
            strSql += vbCrLf + "  SYSTEMID VARCHAR(3),"
            strSql += vbCrLf + "  ADDRESS VARCHAR(180),"
            strSql += vbCrLf + "  BATCHNO VARCHAR(20),"
            strSql += vbCrLf + "  USERNAME VARCHAR(180),"
            strSql += vbCrLf + "  RESULT INT,"
            strSql += vbCrLf + "  COLHEAD VARCHAR(1),"
            strSql += vbCrLf + "  SNO INT IDENTITY)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " ALTER TABLE TEMP" & systemId & "CASHTRAN ADD COLHEAD VARCHAR(1)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CASHTRAN)>0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRAN SET COLHEAD = 'T' WHERE RESULT IN (0)"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRAN SET COLHEAD = 'S' WHERE RESULT IN (3)"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRAN SET COLHEAD = 'G' WHERE RESULT IN (5)"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRAN SET COLHEAD = 'G' WHERE RESULT IN (6)"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CASHTRAN)>0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "CASHTRANSACTION(BILLNO,BILLDATE,MTRANNAME,TRANNAME,"
            strSql += vbCrLf + "  RECEIPT,PAYMENT,PARTICULARS,REMARKS,SYSTEMID,ADDRESS,BATCHNO,USERNAME,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT TRANNO,TRANDATE,MTRANNAME,TRANNAME,"
            strSql += vbCrLf + "  RECEIPT,PAYMENT,PARTICULARS,REMARKS,SYSTEMID,ADDRESS,BATCHNO,USERNAME,RESULT,COLHEAD"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "CASHTRAN ORDER BY MTRANNAME,TRANNAME,RESULT,BATCHNO,TRANDATE,TRANNO"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CASHTRANSACTION)>0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRANSACTION SET RECEIPT = NULL WHERE RECEIPT = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "CASHTRANSACTION SET PAYMENT = NULL WHERE PAYMENT = 0"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  SELECT BILLNO,CONVERT(VARCHAR,BILLDATE,103) BILLDATE,MTRANNAME,TRANNAME,RECEIPT,PAYMENT,PARTICULARS,REMARKS,SYSTEMID,ADDRESS,BATCHNO,USERNAME,RESULT,COLHEAD,SNO,BILLDATE TRANDATE1 FROM TEMP" & systemId & "CASHTRANSACTION "
            If rbtSummary.Checked Then
                strSql += vbCrLf + "  WHERE RESULT > 2"
                strSql += vbCrLf + "  ORDER BY MTRANNAME,RESULT,TRANDATE1,BILLNO,TRANNAME"
            ElseIf rbtNormal.Checked Then
                strSql += vbCrLf + "  WHERE RESULT NOT IN (0,1,3)"
                strSql += vbCrLf + "  ORDER BY RESULT,TRANDATE1,BILLNO,TRANNAME"
            Else
                strSql += vbCrLf + "  ORDER BY MTRANNAME,RESULT,TRANDATE1,BILLNO,TRANNAME"
            End If
            dtCashTran = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCashTran)
            'Add Title of the Report into Label lblTitle
            Dim strTitle As String = Nothing
            strTitle = "CASHTRANSACTION REPORT"
            strTitle += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""

            If chkCmbCostcentre.Text <> "ALL" And chkCmbCostcentre.Text <> "" Then
                strTitle += " " & chkCmbCostcentre.Text & "  "
            End If

            If (cmbCashTransactionType.Text <> "ALL" And cmbCashTransactionType.Text <> "") Or (txtNodeId.Text <> "ALL" And txtNodeId.Text <> "") Then
                strTitle += " FOR "
            End If
            If cmbCashTransactionType.Text <> "ALL" And cmbCashTransactionType.Text <> "" Then
                strTitle += " " & cmbCashTransactionType.Text & " AND"
            End If
            If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
                strTitle += " " & txtNodeId.Text & ""
            End If
            If Strings.Right(strTitle, 3) = "AND" Then
                strTitle = Strings.Left(strTitle, strTitle.Length - 3)
            End If
            lblTitle.Text = strTitle
            gridView_OWN.DataSource = dtCashTran
            GridViewFormat()
            If rbtSummary.Checked Then
                For Each col As DataGridViewColumn In gridView_OWN.Columns
                    If col.Name = "TRANNAME" Then
                        col.Visible = True
                    ElseIf col.Name = "RECEIPT" Then
                        col.Visible = True
                    ElseIf col.Name = "PAYMENT" Then
                        col.Visible = True
                    Else
                        col.Visible = False
                    End If
                Next
            End If
            If rbtDetail.Checked Or rbtNormal.Checked Then
                For Each col As DataGridViewColumn In gridView_OWN.Columns
                    col.Visible = True
                Next
                gridView_OWN.Columns("COLHEAD").Visible = False
                gridView_OWN.Columns("SNO").Visible = False
                gridView_OWN.Columns("TRANDATE1").Visible = False
                gridView_OWN.Columns("RESULT").Visible = False
                gridView_OWN.Columns("MTRANNAME").Visible = False
            End If

            'AddHandler objMonthSummary.gridView.KeyDown, AddressOf gridCashTransactionKeyDown
            gridView_OWN.Focus()
        Catch ex As Exception

            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        btnView_Search.Enabled = True

    End Function

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .Cells("TRANNAME").Style.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportTotalStyle.ForeColor
                    Case "S"
                        .Cells("TRANNAME").Style.BackColor = reportSubTotalStyle1.BackColor
                        .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                    Case "T"
                        .Cells("TRANNAME").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView_OWN.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
                If gridView_OWN.Columns(cnt).Visible Then
                    wid += gridView_OWN.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(gridView_OWN.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
            Me.Size = New Size(wid, Me.Height)
        End If
        If FormReLocation Then SetCenterLocation(Me)
    End Sub

    'Function funcItemFilter() As String
    '    Dim strFilter As String = Nothing
    '    strFilter += " and O.TranDate between '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
    '    strFilter += " and IsNull(O.Cancel,'') <> 'Y'"
    '    If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
    '        strFilter += " and O.systemid ='" & Replace(txtNodeId.Text, "'", "''") & "'"
    '    End If
    '    If cmbCashTransactionType.Text <> "ALL" And cmbCashTransactionType.Text <> "" Then
    '        strFilter += " AND O.CTranCode =(select TranCode from " & cnAdminDb & "..CashTran where TranName='" & Replace(cmbCashTransactionType.Text, "'", "''") & "')"
    '    End If
    '    Return strFilter
    'End Function
    '==================================================================================================
    'QUERY FOR WITHOUT STORED PROCEDURE
    '=======================================
    'strItemFilter = funcItemFilter() + " "
    'Query for take the values from Outstanding table and insert into Temporary table
    'strsql = "If(select 1 from sysobjects where name='TEMP" & SystemId & "CASHTRAN')>0"
    'strsql += " drop table TEMP" & SystemId & "CASHTRAN"
    'strsql += " select "
    'strsql += " TranNo  TRANNO,"
    'strsql += "TRANDATE,"
    'strsql += "(select ISNULL(TranName,' ') from " & cnAdminDb & "..CashTran where TranCode=O.CTranCode)  TRANNAME,"
    'strsql += "(case when RecPay ='R' then ISNULL(Amount,0) else 0 end) RECEIPT,"
    'strsql += "(case when RecPay ='P' then ISNULL(Amount,0)  else 0 end) PAYMENT,"
    'strsql += "(case when (select PName from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo) <> ' ' then"
    'strsql += " (select PName from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo)"
    'strsql += " else "
    'strsql += " (select AcName from " & cnAdminDb & "..achead where AcCode ="
    'strsql += " (select AcCode from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo))"
    'strsql += " end)PARTICULARS,"
    'strsql += "(Remark1 + Remark2) REMARKS,"
    'strsql += " SystemId  SYSTEMID,"
    'strsql += "(case when (select PName from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo) <> ' ' then"
    'strsql += " (select (DoorNo+Address1+Address2+City) from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo)"
    'strsql += " else "
    'strsql += " (select (DoorNo+Address1+Address2+City) from " & cnAdminDb & "..achead where AcCode ="
    'strsql += " (select AcCode from " & cnStockDb & "..PersonalInfo where BatchNo=O.BatchNo))"
    'strsql += " end)ADDRESS"
    'strsql += ",BATCHNO"
    'strsql += ",1 RESULT"
    'strsql += " into TEMP" & SystemId & "CASHTRAN"
    'strsql += " from "
    'strsql += " " & cnAdminDb & "..OUTSTANDING as O"
    'strsql += " where TranType='T'"
    'strsql += strItemFilter

    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    'cmd.ExecuteNonQuery()
    ''Query for calculate the difference between Receipt and Payment summary amount
    'strsql = "IF(select count(*) from TEMP" & SystemId & "CASHTRAN)>0"
    'strsql += " BEGIN"
    'strsql += " if (select 1 from sysobjects where name='TEMP" & SystemId & "TOTALCASH')>0"
    'strsql += " drop table TEMP" & SystemId & "TOTALCASH"
    'strsql += " select"
    'strsql += " 'DIFFERENCE'TRANNAME"
    'strsql += ",0 RECEIPT,(SUM(RECEIPT)-SUM(PAYMENT)) PAYMENT"
    'strsql += ",3 RESULT"
    'strsql += " into TEMP" & SystemId & "TOTALCASH"
    'strsql += " FROM TEMP" & SystemId & "CASHTRAN"
    'strsql += " END"
    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    'cmd.ExecuteNonQuery()
    ''Query for calculate Sum Amount of Receipt amount and Payment amount
    'strsql = "IF(select count(*) from TEMP" & SystemId & "CASHTRAN)>0"
    'strsql += " BEGIN"
    'strsql += " insert into TEMP" & SystemId & "CASHTRAN(TRANNAME,RECEIPT,PAYMENT,RESULT)"
    'strsql += " select"
    'strsql += " 'TOTAL'TRANNAME,"
    'strsql += "sum(RECEIPT) RECEIPT,"
    'strsql += "sum(PAYMENT) PAYMENT,2 RESULT"
    'strsql += " from TEMP" & SystemId & "CASHTRAN"
    'strsql += " UNION ALL"
    'strsql += " select * from TEMP" & SystemId & "TOTALCASH"
    'strsql += " END"

    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    'cmd.ExecuteNonQuery()
    '
    Private Sub gridCashTransaction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        'AddHandler objMonthSummary.gridView.KeyDown, AddressOf gridCashTransactionKeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        ElseIf e.KeyCode = Keys.U Then
            If Not gridView_OWN.RowCount > 0 Then Exit Sub
            If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
            If gridView_OWN.CurrentRow.Cells("TRANNAME").Value.ToString = "" Then Exit Sub
            Dim objCashTranUpd As New CashTranUpdate()
            objCashTranUpd.Trantype = gridView_OWN.CurrentRow.Cells("TRANNAME").Value.ToString
            objCashTranUpd.Remark = gridView_OWN.CurrentRow.Cells("REMARKS").Value.ToString
            objCashTranUpd.batchNo = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
            objCashTranUpd.ShowDialog()
            btnView_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.M Then
            If gridView_OWN.Rows.Count > 0 Then
                If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "5" Or gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "6") Then Exit Sub
                If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "2") Then
                    funcMonthSummary(gridView_OWN.CurrentRow.Cells("TRANNAME").Value.ToString, True)
                    Exit Sub
                Else
                    funcMonthSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString, False)
                    Exit Sub
                End If
            End If
        ElseIf e.KeyCode = Keys.L Then
            If gridView_OWN.Rows.Count > 0 Then
                If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "3" Or gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "0") Then
                    funcLedgerSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString, False)
                End If
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.C Then
            If gridView_OWN.Rows.Count > 0 Then
                If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "3" Or gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "0") Then
                    funcContraSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString, False)
                End If
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridCashTransactionKeyDown2(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyChar = Chr(Keys.M) Then
        '    If gridView_OWN.Rows.Count > 0 Then
        '        If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "5" Or gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "6") Then Exit Sub
        '        If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "2") Then
        '            funcMonthSummary(gridView_OWN.CurrentRow.Cells("TRANNAME").Value.ToString, True)
        '            Exit Sub
        '        Else
        '            funcMonthSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString, False)
        '            Exit Sub
        '        End If
        '    End If
        'ElseIf e.KeyChar = Chr(Keys.L) Then
        '    If gridView_OWN.Rows.Count > 0 Then
        '        If (gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "3" Or gridView_OWN.CurrentRow.Cells("RESULT").Value.ToString = "0") Then
        '            funcLedgerSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString)
        '        End If
        '        Exit Sub
        '    End If
        'End If
        With objLedgerSummary.gridView
            If e.KeyCode = Keys.M Then
                If Not .Columns.Contains("TRANNAME") Then Exit Sub
                If .Rows.Count > 0 Then
                    If .CurrentRow.Cells("TRANNAME").Value.ToString = "" Then Exit Sub
                    funcMonthSummary(.CurrentRow.Cells("TRANNAME").Value.ToString, True)
                    Exit Sub
                End If
            End If
        End With
    End Sub

    Private Sub gridCashTransactionKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        With objMonthSummary.gridView
            If e.KeyCode = Keys.L Then
                If Not (.Columns.Contains("TRANNAME") Or .Columns.Contains("MTRANNAME")) Then Exit Sub
                If .Rows.Count > 0 Then
                    If (.Columns.Contains("TRANNAME")) Then
                        funcLedgerSummary(gridView_OWN.CurrentRow.Cells("TRANNAME").Value.ToString, True)
                    Else
                        funcLedgerSummary(gridView_OWN.CurrentRow.Cells("MTRANNAME").Value.ToString, False)
                    End If
                    Exit Sub
                End If
                End If
        End With

        'With objLedgerSummary.gridView
        '    'If e.KeyChar.ToString = "m" Then e.KeyChar = Keys.M
        '    If e.KeyCode = Keys.M Then
        '        If Not .Columns.Contains("TRANNAME") Then Exit Sub
        '        If .Rows.Count > 0 Then
        '            If .CurrentRow.Cells("TRANNAME").Value.ToString = "" Then Exit Sub
        '            funcMonthSummary(.CurrentRow.Cells("TRANNAME").Value.ToString, True)
        '            Exit Sub
        '        End If
        '    End If
        'End With
    End Sub

    Function funcMonthSummary(ByVal mTran As String, ByVal SubLedger As Boolean) As Integer
        Dim dtSum As DataTable

        strSql = vbCrLf + "   	SELECT CONVERT(VARCHAR,BILLYEAR) YEAR,BILLMNAME MONTH,RECEIPT,PAYMENT,BILLYEAR,BILLMONTH,'" & mTran & "' AS " & IIf(SubLedger, "MTRANNAME", "TRANNAME") & " FROM ("
        strSql += vbCrLf + "   	SELECT BILLYEAR,BILLMONTH,BILLMNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT FROM ("
        strSql += vbCrLf + "   	SELECT  RECEIPT,PAYMENT,YEAR(BILLDATE)BILLYEAR,MONTH(BILLDATE)BILLMONTH,DATENAME(MONTH,BILLDATE)BILLMNAME"
        If SubLedger Then
            strSql += vbCrLf + "   	FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(TRANNAME,'')='" & mTran & "' AND RESULT=2)X  "
        Else
            strSql += vbCrLf + "   	FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(MTRANNAME,'')='" & mTran & "' AND RESULT=2)X  "
        End If
        strSql += vbCrLf + "   	GROUP BY BILLYEAR,BILLMONTH,BILLMNAME"
        strSql += vbCrLf + "   	UNION ALL"
        strSql += vbCrLf + "   	SELECT 9999 BILLYEAR,99 BILLMONTH,'TOTAL' BILLMNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT FROM TEMP" & systemId & "CASHTRANSACTION "
        strSql += vbCrLf + "   	WHERE ISNULL(" & IIf(SubLedger, "TRANNAME", "MTRANNAME") & ",'')='" & mTran & "' AND RESULT=2   	   	"
        strSql += vbCrLf + "   	)X ORDER BY BILLYEAR,BILLMONTH"
        da = New OleDbDataAdapter(strSql, cn)
        dtSum = New DataTable
        da.Fill(dtSum)
        If Not dtSum.Rows.Count > 0 Then Exit Function
        objMonthSummary = New frmGridDispDia
        objMonthSummary.BaseName = "Cash Transaction Month Summary"
        objMonthSummary.Name = Me.Name
        objMonthSummary.WindowState = FormWindowState.Normal
        objMonthSummary.gridView.RowTemplate.Height = 21
        objMonthSummary.gridView.DataSource = dtSum
        With objMonthSummary.gridView.Columns(IIf(SubLedger, "MTRANNAME", "TRANNAME"))
            .VISIBLE = False
        End With
        With objMonthSummary.gridView.Columns("MONTH")
            .HeaderText = "MONTH"
            '.Width = 410
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With objMonthSummary.gridView.Columns("BILLYEAR")
            .Visible = False
        End With
        With objMonthSummary.gridView.Columns("BILLMONTH")
            .Visible = False
        End With
        With objMonthSummary.gridView.Columns("RECEIPT")
            '.Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objMonthSummary.gridView.Columns("PAYMENT")
            '.Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        'objMonthSummary.Size = New Size(778, 550)
        objMonthSummary.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objMonthSummary.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objMonthSummary.gridView.BackgroundColor = objMonthSummary.BackColor
        objMonthSummary.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
        objMonthSummary.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
        objMonthSummary.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
        objMonthSummary.Text = mTran & " [Month Summary]"
        objMonthSummary.lblTitle.Text = mTran & " [Month Summary]"
        objMonthSummary.lblTitle.Text = lblTitle.Text + Environment.NewLine + "[ " + mTran + " ]"
        objMonthSummary.pnlHeader.Height = objMonthSummary.lblTitle.Height + 20
        objMonthSummary.StartPosition = FormStartPosition.CenterScreen
        AddHandler objMonthSummary.gridView.KeyDown, AddressOf gridCashTransactionKeyDown
        objMonthSummary.Show()

        For Each dgvRow As DataGridViewRow In objMonthSummary.gridView.Rows
            If dgvRow.Cells("YEAR").Value.ToString.Contains(Chr(13)) Then
                dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                objMonthSummary.gridView.AutoResizeRow(dgvRow.Index, DataGridViewAutoSizeRowMode.AllCells)
            End If
            If Val(dgvRow.Cells("RECEIPT").Value.ToString) = 0 Then
                dgvRow.Cells("RECEIPT").Value = DBNull.Value
                dgvRow.Cells("RECEIPT").Style.BackColor = dgvRow.Cells("MONTH").Style.BackColor
            Else
                dgvRow.Cells("RECEIPT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("PAYMENT").Value.ToString) = 0 Then
                dgvRow.Cells("PAYMENT").Value = DBNull.Value
                'dgvRow.Cells("PAYMENT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("PAYMENT").Style.BackColor = Color.LavenderBlush
            End If
            Select Case dgvRow.Cells("MONTH").Value.ToString
                Case "TOTAL"
                    dgvRow.Cells("YEAR").Value = ""
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
            End Select
        Next
        'LedgerSummaryGridViewFormat(objMonthSummary.gridView)
    End Function

    Function funcLedgerSummary(ByVal mTran As String, ByVal SubLedger As Boolean, Optional ByVal billYear As String = "", Optional ByVal billMonth As String = "") As Integer
        Dim dtSum As DataTable
        strSql = vbCrLf + "   SELECT TRANNAME,RECEIPT,PAYMENT FROM ("
        strSql += vbCrLf + "   SELECT  TRANNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,0 AS RESULT"
        If SubLedger Then
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(TRANNAME,'')='" & mTran & "' AND RESULT=2"
        Else
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(MTRANNAME,'')='" & mTran & "' AND RESULT=2"
        End If
        strSql += vbCrLf + "   GROUP BY TRANNAME"
        strSql += vbCrLf + "   UNION ALL"
        strSql += vbCrLf + "   SELECT  'TOTAL' TRANNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,1 AS RESULT"
        If SubLedger Then
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(TRANNAME,'')='" & mTran & "' AND RESULT=2"
        Else
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(MTRANNAME,'')='" & mTran & "' AND RESULT=2"
        End If
        strSql += vbCrLf + "   )X ORDER BY RESULT,TRANNAME"
        da = New OleDbDataAdapter(strSql, cn)
        dtSum = New DataTable
        da.Fill(dtSum)
        If Not dtSum.Rows.Count > 0 Then Exit Function
        objLedgerSummary = New frmGridDispDia
        objLedgerSummary.BaseName = "Cash Transaction Ledger Summary"
        objLedgerSummary.Name = Me.Name
        objLedgerSummary.WindowState = FormWindowState.Normal
        objLedgerSummary.gridView.RowTemplate.Height = 21
        objLedgerSummary.gridView.DataSource = dtSum
        With objLedgerSummary.gridView.Columns("TRANNAME")
            .HeaderText = "TRANNAME"
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With objLedgerSummary.gridView.Columns("RECEIPT")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objLedgerSummary.gridView.Columns("PAYMENT")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        'objLedgerSummary.Size = New Size(778, 550)
        objLedgerSummary.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objLedgerSummary.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objLedgerSummary.gridView.BackgroundColor = objLedgerSummary.BackColor
        objLedgerSummary.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
        objLedgerSummary.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
        objLedgerSummary.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
        objLedgerSummary.Text = mTran & " [Ledger Summary]"
        objLedgerSummary.lblTitle.Text = mTran & " [Ledger Summary]"
        objLedgerSummary.lblTitle.Text = lblTitle.Text + Environment.NewLine + "[ " + mTran + " ]"
        objLedgerSummary.pnlHeader.Height = objLedgerSummary.lblTitle.Height + 20
        objLedgerSummary.StartPosition = FormStartPosition.CenterScreen
        AddHandler objLedgerSummary.gridView.KeyDown, AddressOf gridCashTransactionKeyDown2
        objLedgerSummary.Show()

        For Each dgvRow As DataGridViewRow In objLedgerSummary.gridView.Rows
            If Val(dgvRow.Cells("RECEIPT").Value.ToString) = 0 Then
                dgvRow.Cells("RECEIPT").Value = DBNull.Value
                dgvRow.Cells("RECEIPT").Style.BackColor = dgvRow.Cells("TRANNAME").Style.BackColor
            Else
                dgvRow.Cells("RECEIPT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("PAYMENT").Value.ToString) = 0 Then
                dgvRow.Cells("PAYMENT").Value = DBNull.Value
                'dgvRow.Cells("PAYMENT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("PAYMENT").Style.BackColor = Color.LavenderBlush
            End If
            Select Case dgvRow.Cells("TRANNAME").Value.ToString
                Case "TOTAL"
                    dgvRow.Cells("TRANNAME").Value = ""
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
            End Select
        Next
        'LedgerSummaryGridViewFormat(objLedgerSummary.gridView)
    End Function


    Function funcContraSummary(ByVal mTran As String, ByVal SubLedger As Boolean, Optional ByVal billYear As String = "", Optional ByVal billMonth As String = "") As Integer
        Dim dtSum As DataTable
        strSql = vbCrLf + "   SELECT PARTICULARS,RECEIPT,PAYMENT FROM ("
        strSql += vbCrLf + "   SELECT  PARTICULARS,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,0 AS RESULT"
        If SubLedger Then
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(PARTICULARS,'')='" & mTran & "' AND RESULT=2"
        Else
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(MTRANNAME,'')='" & mTran & "' AND RESULT=2"
        End If
        strSql += vbCrLf + "   GROUP BY PARTICULARS"
        strSql += vbCrLf + "   UNION ALL"
        strSql += vbCrLf + "   SELECT  'TOTAL' PARTICULARS,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT,1 AS RESULT"
        If SubLedger Then
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(PARTICULARS,'')='" & mTran & "' AND RESULT=2"
        Else
            strSql += vbCrLf + "   FROM TEMP" & systemId & "CASHTRANSACTION WHERE ISNULL(MTRANNAME,'')='" & mTran & "' AND RESULT=2"
        End If
        strSql += vbCrLf + "   )X ORDER BY RESULT,PARTICULARS"
        da = New OleDbDataAdapter(strSql, cn)
        dtSum = New DataTable
        da.Fill(dtSum)
        If Not dtSum.Rows.Count > 0 Then Exit Function
        objLedgerSummary = New frmGridDispDia
        objLedgerSummary.BaseName = "Cash Transaction Ledger Summary"
        objLedgerSummary.Name = Me.Name
        objLedgerSummary.WindowState = FormWindowState.Normal
        objLedgerSummary.gridView.RowTemplate.Height = 21
        objLedgerSummary.gridView.DataSource = dtSum
        With objLedgerSummary.gridView.Columns("PARTICULARS")
            .HeaderText = "PARTICULARS"
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With objLedgerSummary.gridView.Columns("RECEIPT")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objLedgerSummary.gridView.Columns("PAYMENT")
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        'objLedgerSummary.Size = New Size(778, 550)
        objLedgerSummary.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objLedgerSummary.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objLedgerSummary.gridView.BackgroundColor = objLedgerSummary.BackColor
        objLedgerSummary.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
        objLedgerSummary.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
        objLedgerSummary.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
        objLedgerSummary.Text = mTran & " [Contra Summary]"
        objLedgerSummary.lblTitle.Text = mTran & " [Contra Summary]"
        objLedgerSummary.lblTitle.Text = lblTitle.Text + Environment.NewLine + "[ " + mTran + " ]"
        objLedgerSummary.pnlHeader.Height = objLedgerSummary.lblTitle.Height + 20
        objLedgerSummary.StartPosition = FormStartPosition.CenterScreen
        AddHandler objLedgerSummary.gridView.KeyDown, AddressOf gridCashTransactionKeyDown2
        objLedgerSummary.Show()

        For Each dgvRow As DataGridViewRow In objLedgerSummary.gridView.Rows
            If Val(dgvRow.Cells("RECEIPT").Value.ToString) = 0 Then
                dgvRow.Cells("RECEIPT").Value = DBNull.Value
                dgvRow.Cells("RECEIPT").Style.BackColor = dgvRow.Cells("PARTICULARS").Style.BackColor
            Else
                dgvRow.Cells("RECEIPT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("PAYMENT").Value.ToString) = 0 Then
                dgvRow.Cells("PAYMENT").Value = DBNull.Value
                'dgvRow.Cells("PAYMENT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("PAYMENT").Style.BackColor = Color.LavenderBlush
            End If
            Select Case dgvRow.Cells("PARTICULARS").Value.ToString
                Case "TOTAL"
                    dgvRow.Cells("PARTICULARS").Value = ""
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
            End Select
        Next
        'LedgerSummaryGridViewFormat(objLedgerSummary.gridView)
    End Function


    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCashTransactionReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCashTransactionReport_Properties))
        cmbCashTransactionType.Text = obj.p_cmbCashTransactionType
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        txtNodeId.Text = obj.p_txtNodeId
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCashTransactionReport_Properties
        obj.p_cmbCashTransactionType = cmbCashTransactionType.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_txtNodeId = txtNodeId.Text
        SetSettingsObj(obj, Me.Name, GetType(frmCashTransactionReport_Properties))
    End Sub


    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView_OWN.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN)
        objSearch.ShowDialog()
    End Sub
End Class


Public Class frmCashTransactionReport_Properties

    Private cmbCashTransactionType As String = "ALL"
    Public Property p_cmbCashTransactionType() As String
        Get
            Return cmbCashTransactionType
        End Get
        Set(ByVal value As String)
            cmbCashTransactionType = value
        End Set
    End Property
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private txtNodeId As String = ""
    Public Property p_txtNodeId() As String
        Get
            Return txtNodeId
        End Get
        Set(ByVal value As String)
            txtNodeId = value
        End Set
    End Property

End Class