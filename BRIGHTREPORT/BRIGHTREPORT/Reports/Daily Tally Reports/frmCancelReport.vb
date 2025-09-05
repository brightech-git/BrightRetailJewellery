Imports System.Data.OleDb
Public Class frmCancelReport
    Dim dtCancelRpt As New DataTable
    Dim strItemFilter As String = Nothing
    Dim strSql As String
    Dim cmd As OleDbCommand

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNewHeader()
        'If cmbCostCentre.Items.Count > 0 Then
        '    cmbCostCentre.SelectedIndex = 0
        'End If
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' rbtBillType.Checked = True
        Prop_Gets()
        ChkBillSummary.Checked = False
        dtpFrom.Focus()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmCancelReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

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
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridCancelReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
        End If
    End Sub

    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function

    Private Sub frmCancelReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.Value = GetServerDate(tran)
        dtpTo.Value = GetServerDate(tran)
        LoadCompany(chkLstCompany)
        loadcmbbilltype()
        dtpFrom.Focus()
        funcAddCostCentre()
        funcNewHeader()
        ChkBillSummary.Checked = False
        rbtBillType.Checked = True
        btnNew_Click(Me, New EventArgs)
    End Sub

    Function loadcmbbilltype() As Integer
        cmbBillType.Items.Clear()
        cmbBillType.Items.Add("ALL")
        cmbBillType.Items.Add("SALES")
        cmbBillType.Items.Add("PURCHASE")
        cmbBillType.Items.Add("SALES RETURN")
        cmbBillType.Items.Add("APPROVAL ISSUE")
        cmbBillType.Items.Add("APPROVAL RECEIPT")
        cmbBillType.Items.Add("MISC ISSUE")
        cmbBillType.Items.Add("OEDER DELIVERY")
        cmbBillType.Items.Add("REPAIR DELIVERY")
        cmbBillType.Items.Add("GIFT VOUCHER")
        cmbBillType.Items.Add("ISSUE")
        cmbBillType.Items.Add("ISSUE INTERNAL TRANSFER")
        cmbBillType.Items.Add("ISSUE PURCHASE RETURN")
        cmbBillType.Items.Add("ISSUE APPROVAL ISSUE")
        cmbBillType.Items.Add("ISSUE OTHER ISSUE")
        cmbBillType.Items.Add("ISSUE PACKING")
        cmbBillType.Items.Add("RECEIPT")
        cmbBillType.Items.Add("RECEIPT PURCHASE")
        cmbBillType.Items.Add("RECEIPT INTERNAL TRANSFER")
        cmbBillType.Items.Add("RECEIPT APPROVAL RECEIPT")
        cmbBillType.Items.Add("RECEIPT OTHER RECEIPT")
        cmbBillType.Items.Add("RECEIPT PACKING")
        cmbBillType.Text = "ALL"
    End Function

    Function funcNewHeader() As Integer
        Try
            lblTitle.Text = "TITLE"
            dtCancelRpt.Clear()
            gridView.DataSource = Nothing
            strSql = "select ''BATCHNO,''BILLTYPE,''BILLNO,''BILLDATE,''USERNAME,''UPDATED,''UPTIME,"
            strSql += "''PCS,''GRSWT,''NETWT,''AMOUNT,''TAX,''SC,''ADSC,''TOTAL,''DUMMYDATE WHERE 1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCancelRpt)
            gridView.DataSource = dtCancelRpt
            funcGridStyle()
            lblTitle.Height = gridView.ColumnHeadersHeight
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        btnView_Search.Enabled = False
        Try
            If RbdBilling.Checked Then



                dtCancelRpt.Clear()
                lblTitle.Text = "TITLE"
                Me.Refresh()
                '=======================================================================================================================================
                'QUERY FOR WITHOUT STORED PROCEDURE
                '==================================
                'strItemFilter = funcFilteration() + " "
                ''Query for take the values from Issue table
                'strsql = "select"
                'strsql += " BatchNo as BATCHNO,TranType as BILLTYPE,TranNo as BILLNO,CONVERT(VARCHAR,TranDate,103) as BILLDATE,"
                'strsql += "USERNAME,CONVERT(VARCHAR,UPDATED,103) UPDATED,CONVERT(VARCHAR,UPTIME,108) UPTIME,"
                'strsql += "PCS,GRSWT,NETWT,AMOUNT,TAX,SC,ADSC,(AMOUNT+TAX+SC+ADSC) as TOTAL,DUMMYDATE"
                'strsql += " from"
                'strsql += "("
                'strsql += "select"
                'strsql += " BatchNO,TranNo,TranDate,TranType,"
                'strsql += "(select EmpName from " & cnAdminDb & "..EmpMaster where EmpId =TI.UserId) as UserName,"
                'strsql += "UpDated,UpTime,"
                'strsql += "PCS,GRSWT,NETWT,AMOUNT,TAX,SC,ADSC,TRANDATE AS DUMMYDATE"
                'strsql += " from " & cnStockDb & "..Issue as TI"
                'strsql += strItemFilter
                'strsql += ")I"
                ''Union All between Issue and Receipt table 
                'strsql += " Union All"
                ''Query for take the values from Receipt table
                'strsql += " select "
                'strsql += " BatchNo as BATCHNO,TranType as BILLTYPE,TranNo as BILLNO,CONVERT(VARCHAR,TranDate,103) as BILLDATE,"
                'strsql += " USERNAME,CONVERT(VARCHAR,UPDATED,103) UPDATED,CONVERT(VARCHAR,UPTIME,108) UPTIME,"
                'strsql += " PCS,GRSWT,NETWT,AMOUNT,TAX,SC,ADSC,(AMOUNT+TAX+SC+ADSC) as TOTAL,DUMMYDATE"
                'strsql += " from"
                'strsql += " ("
                'strsql += "select "
                'strsql += " BatchNo,TranNo,TranDate,TranType,"
                'strsql += "(select EmpName from " & cnAdminDb & "..EmpMaster where EmpId =TR.UserId) as UserName,"
                'strsql += "UPDATED,UPTIME,"
                'strsql += "PCS,GRSWT,NETWT,AMOUNT,TAX,SC,ADSC,TRANDATE AS DUMMYDATE"
                'strsql += " from " & cnStockDb & "..Receipt as TR"
                'strsql += strItemFilter
                'strsql += " )R"
                '=======================================================================================================================
                strSql = " EXEC " & cnStockDb & "..SP_RPT_CANCEL"
                strSql += " @DATEFROM = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,@DATETO = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,@COSTNAME ='" & Replace(cmbCostCentre.Text, "'", "''''") & "'"
                strSql += " ,@SystemID='" & systemId & "',@cnAdminDB='" & cnAdminDb & "',@cnStockDB='" & cnStockDb & "'"
                strSql += " ,@CNCOMPANYID='" & strCompanyId & "'"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT"
                If rbtBillDate.Checked = True Then
                    strSql += " Order By DUMMYDATE,BATCHNO,BILLNO"
                ElseIf rbtBillType.Checked = True Then
                    strSql += " Order By BILLTYPE,BATCHNO,DUMMYDATE,BILLNO"
                ElseIf rbtBillNo.Checked = True Then
                    strSql += " Order By BILLNO,BATCHNO,DUMMYDATE"
                End If
                dtCancelRpt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCancelRpt)
                If dtCancelRpt.Rows.Count < 1 Then
                    MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    btnView_Search.Enabled = True
                    btnView_Search.Focus()
                    Exit Sub
                End If

                strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CANCELREP')"
                strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CANCELREP"
                strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "CANCELREP("
                strSql += " BATCHNO VARCHAR(20),"
                strSql += " BILLTYPE VARCHAR(50),"
                strSql += " BILLNO INT,"
                strSql += " BILLDATE VARCHAR(12),"
                strSql += " USERNAME VARCHAR(50),"
                strSql += " UPDATED VARCHAR(20),"
                strSql += " UPTIME VARCHAR(12),"
                strSql += " PCS INT,"
                strSql += " GRSWT NUMERIC(15,3),"
                strSql += " NETWT NUMERIC(15,3),"
                strSql += " AMOUNT NUMERIC(15,2),"
                strSql += " TAX NUMERIC(15,2),"
                strSql += " SC NUMERIC(15,2),"
                strSql += " ADSC NUMERIC(15,2),"
                strSql += " TOTAL NUMERIC(15,2),"
                strSql += " REMARK1 VARCHAR(50),"
                strSql += " REMARK2 VARCHAR(50),"
                strSql += " DUMMYDATE SMALLDATETIME,"
                strSql += " COLHEAD VARCHAR(1),"
                strSql += " RESULT INT,"
                strSql += " SNO INT IDENTITY)"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT ADD COLHEAD VARCHAR(1)"
                strSql += " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT ADD RESULT INT"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT SET RESULT = 1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT)>0 "
                strSql += " BEGIN"
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT(PCS,GRSWT,NETWT,AMOUNT,TAX,SC,ADSC,TOTAL,COLHEAD,RESULT)"
                strSql += " SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,"
                strSql += " SUM(TAX)TAX,SUM(SC)SC,SUM(ADSC)ADSC,SUM(TOTAL)TOTAL,'G'COLHEAD,2"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT"
                strSql += " END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT)>0 "
                strSql += " BEGIN"
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CANCELREP(BATCHNO,BILLTYPE,BILLNO,BILLDATE,"
                strSql += " USERNAME,UPDATED,UPTIME, PCS, GRSWT, NETWT, AMOUNT, TAX, SC, ADSC, TOTAL, DUMMYDATE,REMARK1,REMARK2,COLHEAD)"
                strSql += " SELECT BATCHNO,BILLTYPE,BILLNO,BILLDATE,"
                strSql += " USERNAME,UPDATED,UPTIME, PCS, GRSWT, NETWT, AMOUNT, TAX ,SC, ADSC, TOTAL, DUMMYDATE,REMARK1,REMARK2,COLHEAD"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREPORT "
                If rbtBillDate.Checked = True Then
                    strSql += " ORDER BY RESULT,DUMMYDATE,BATCHNO,BILLNO"
                ElseIf rbtBillType.Checked = True Then
                    strSql += " ORDER BY RESULT,BILLTYPE,BATCHNO,DUMMYDATE,BILLNO"
                ElseIf rbtBillNo.Checked = True Then
                    strSql += " ORDER BY RESULT,BILLNO,BATCHNO,DUMMYDATE"
                End If
                strSql += " END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET PCS=NULL WHERE PCS = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET GRSWT=NULL WHERE GRSWT = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET NETWT=NULL WHERE NETWT = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET AMOUNT=NULL WHERE AMOUNT = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET TAX =NULL WHERE TAX = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET SC =NULL WHERE SC = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET ADSC =NULL WHERE ADSC = 0 "
                strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "CANCELREP SET TOTAL =NULL WHERE TOTAL = 0 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                If ChkBillSummary.Checked Then
                    strSql = " SELECT T.BILLTYPE,T.BILLNO,T.BILLDATE "
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(SC)SC,SUM(ADSC)ADSC,SUM(TOTAL)TOTAL"
                    strSql += vbCrLf + " ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,P.AREA,P.CITY,P.[STATE],P.MOBILE,P.PAN,P.GSTNO GST"
                    strSql += vbCrLf + " ,'' COLHEAD,0 RESULT,T.BATCHNO,T.DUMMYDATE"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREP T"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO =T.BATCHNO "
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO  "
                    strSql += vbCrLf + " WHERE ISNULL(COLHEAD,'') ='' "
                    If cmbBillType.Text <> "ALL" And cmbBillType.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.BILLTYPE,'') ='" & cmbBillType.Text & "' "
                    strSql += vbCrLf + " GROUP BY T.BILLTYPE,T.BILLDATE,T.BATCHNO,T.BILLNO,T.DUMMYDATE,P.PNAME,P.DOORNO"
                    strSql += vbCrLf + " ,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,P.AREA,P.CITY,P.[STATE],P.MOBILE,P.PAN,P.GSTNO "
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT NULL BILLTYPE,NULL BILLNO,NULL BILLDATE"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(SC)SC,SUM(ADSC)ADSC,SUM(TOTAL)TOTAL"
                    strSql += vbCrLf + " ,NULL PNAME,NULL DOORNO,NULL ADDRESS1,NULL ADDRESS2,NULL ADDRESS3,NULL AREA,NULL CITY,NULL [STATE],NULL MOBILE,NULL PAN,NULL  GST"
                    strSql += vbCrLf + " ,'G' COLHEAD,1 RESULT,NULL BATCHNO,NULL DUMMYDATE"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREP T WHERE ISNULL(COLHEAD,'') =''"
                    If cmbBillType.Text <> "ALL" And cmbBillType.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.BILLTYPE,'') ='" & cmbBillType.Text & "' "
                    strSql += vbCrLf + " ORDER BY RESULT,DUMMYDATE,BATCHNO,BILLTYPE DESC"
                Else
                    strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "CANCELREP ORDER BY SNO "
                End If

                dtCancelRpt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCancelRpt)

                'Add Title of Report into label lbltitle
                Dim strTitle As String = Nothing
                strTitle = "CANCEL REPORT"
                strTitle += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
                If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                    strTitle += "(" & cmbCostCentre.Text & ")"
                End If
                lblTitle.Text = strTitle
                gridView.DataSource = dtCancelRpt
                GridViewFormat()
                If gridView.Columns.Contains("SNO") Then gridView.Columns("SNO").Visible = False
                gridView.Columns("COLHEAD").Visible = False
                gridView.Columns("RESULT").Visible = False
                If gridView.Columns.Contains("DUMMYDATE") Then gridView.Columns("DUMMYDATE").Visible = False
                gridView.Focus()

            ElseIf RdbAccounts.Checked = True Then
                strSql = "SELECT CAPTION BILLTYPE,T.TRANNO,CONVERT(VARCHAR(15),C.TRANDATE,103)TRANDATE,SUM(T.AMOUNT)AMOUNT,USERNAME,"
                strSql += vbCrLf + " CONVERT(VARCHAR(25),C.UPDATED,103)UPDATED,SUBSTRING(CONVERT(VARCHAR,C.UPTIME,108),0,6)UPTIME"
                strSql += vbCrLf + "  ,T.REMARK1,T.REMARK2"
                strSql += vbCrLf + "FROM " & cnStockDb & "..CANCELLEDTRAN  C"
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ACCTRAN T ON T.BATCHNO=C.BATCHNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACCENTRYMASTER A ON A.PAYMODE=T.PAYMODE "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=T.USERID"
                strSql += vbCrLf + " WHERE T.TRANMODE='C'"
                strSql += vbCrLf + " AND C.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " GROUP BY CAPTION,T.TRANNO,C.TRANDATE,USERNAME,C.UPDATED,C.UPTIME,T.REMARK1,T.REMARK2  "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()
                dtCancelRpt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCancelRpt)
                Dim strTitle As String = Nothing
                strTitle = "CANCEL REPORT FOR ACCOUNTS"
                strTitle += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
                If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                    strTitle += "(" & cmbCostCentre.Text & ")"
                End If
                lblTitle.Text = strTitle
                gridView.DataSource = dtCancelRpt
                With gridView
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End With
                'With gridView
                '    With .Columns("EMPNAME")
                '        .HeaderText = "EMPLOYEE NAME"
                '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                '    End With

                'End With
                ' GridViewFormat()
                'gridView.Columns("SNO").Visible = False
                'gridView.Columns("COLHEAD").Visible = False
                'gridView.Columns("RESULT").Visible = False
                gridView.Focus()
            End If
        Catch ex As Exception

            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub

    Function funcGridStyle() As Integer
        With gridView
            .Columns("BATCHNO").Visible = False
            .Columns("DUMMYDATE").Visible = False
            With .Columns("BILLTYPE")
                .Width = 80
                .HeaderText = "BILL TYPE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
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
            With .Columns("USERNAME")
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("UPDATED")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("UPTIME")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PCS")
                .Width = 40
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAX")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SC")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADSC")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTAL")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Function funcFilteration() As String
        Dim strFilter As String = Nothing
        strFilter += " where TranDate between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strFilter += " and Cancel ='y'"
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strFilter += " and costid =(select costid from " & cnAdminDb & "..costcentre where costname ='" & Replace(cmbCostCentre.Text, "'", "''") & "')"
        End If
        Return strFilter
    End Function

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmCancelReport_Properties
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_rbtBillType = rbtBillType.Checked
        obj.p_rbtBillNo = rbtBillNo.Checked
        obj.p_rbtBillDate = rbtBillDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCancelReport_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmCancelReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCancelReport_Properties))
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        rbtBillType.Checked = obj.p_rbtBillType
        rbtBillNo.Checked = obj.p_rbtBillNo
        rbtBillDate.Checked = obj.p_rbtBillDate
    End Sub

    Private Sub ChkBillSummary_CheckedChanged(sender As Object, e As EventArgs) Handles ChkBillSummary.CheckedChanged
        If ChkBillSummary.Checked Then
            GrpType.Enabled = False
            grpOrderBy.Enabled = False
            RbdBilling.Checked = True
            rbtBillDate.Checked = True
            cmbBillType.Visible = True
            lblBillType.Visible = True
        Else
            GrpType.Enabled = True
            grpOrderBy.Enabled = True
            RbdBilling.Checked = True
            rbtBillDate.Checked = True
            cmbBillType.Visible = False
            lblBillType.Visible = False
        End If
    End Sub
End Class

Public Class frmCancelReport_Properties
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
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
    Private rbtBillType As Boolean = True
    Public Property p_rbtBillType() As Boolean
        Get
            Return rbtBillType
        End Get
        Set(ByVal value As Boolean)
            rbtBillType = value
        End Set
    End Property
    Private rbtBillNo As Boolean = True
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private rbtBillDate As Boolean = True
    Public Property p_rbtBillDate() As Boolean
        Get
            Return rbtBillDate
        End Get
        Set(ByVal value As Boolean)
            rbtBillDate = value
        End Set
    End Property
End Class