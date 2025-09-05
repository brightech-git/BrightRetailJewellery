Imports System.Data.OleDb
Public Class frmTrackingCustomer
#Region "VARIABLE"
    Dim strsql As String
    Dim cmd As OleDbCommand
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim systemId As String = Environment.MachineName.Replace("-", "")
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtdbName As New DataTable
    Dim dtgetTable As DataTable
#End Region

#Region "BUTTON EVENTS"

    Private Function MoreDetail() As String
        Dim Qry As String = ""
        If chkMore.Checked = True Then
            If chkAddress.Checked = True Then Qry += vbCrLf + ",P.ADDRESS1,P.ADDRESS2,P.ADDRESS3 "
            If chkArea.Checked = True Then Qry += vbCrLf + ",P.AREA "
            If chkCity.Checked = True Then Qry += vbCrLf + ",P.CITY"
            If chkState.Checked = True Then Qry += vbCrLf + ",P.STATE"
            If chkPincode.Checked = True Then Qry += vbCrLf + " ,P.PINCODE"
            If chkCountry.Checked = True Then Qry += vbCrLf + " ,P.COUNTRY"
            If chkPhoneRes.Checked = True Then Qry += vbCrLf + ",P.PHONERES"
        End If
        Return Qry
    End Function

    Private Sub dropTable()
        strsql = " IF OBJECT_ID('TEMPTABLEDB..TEMPISSUEBATCHNO" & systemId & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUEBATCHNO" & systemId & " "
        strsql += " IF OBJECT_ID('TEMPTABLEDB..TEMPCUSTOMERINFO" & systemId & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCUSTOMERINFO" & systemId & " "
        strsql += " IF OBJECT_ID('TEMPTABLEDB..TEMPTRACKINGCUSTOMER" & systemId & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTRACKINGCUSTOMER" & systemId & " "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub View1()
        gridView.DataSource = Nothing
        gridDetail.DataSource = Nothing
        dropTable()
        btnView_Search.Enabled = False
        Dim chkCompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True, ",")
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True, ",")

        strsql = vbCrLf + " SELECT DISTINCT BATCHNO "
        strsql += vbCrLf + " INTO TEMPTABLEDB..TEMPISSUEBATCHNO" & systemId & " "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I  "
        strsql += vbCrLf + " WHERE "
        ' strsql += vbCrLf + " I.TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        If rbtLostCustomer.Checked = True Then
            strsql += vbCrLf + " I.TRANDATE < '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'  "
        ElseIf rbtNewCustomer.Checked = True Then
            strsql += vbCrLf + " I.TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        ElseIf rbtOldCustomer.Checked = True Then
            strsql += vbCrLf + " I.TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        End If
        strsql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strsql += vbCrLf + " AND I.TRANTYPE IN('SA','RD','OD')"
        If chkCompanyId <> "" And chkCompanyId <> "ALL" Then
            strsql += vbCrLf + " AND COMPANYID IN (" & chkCompanyId & ")"
        End If
        If chkCostId <> "" And chkCostId <> "ALL" Then
            strsql += vbCrLf + " AND COSTID IN (" & chkCostId & ")"
        End If
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " SELECT DISTINCT PSNO  FROM " & cnAdminDb & "..CUSTOMERINFO AS C WHERE BATCHNO IN " 'BATCHNO
        strsql += vbCrLf + " (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMPISSUEBATCHNO" & systemId & ") "
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)

        Dim PathDefalut As String = Application.StartupPath '"E:" 
        Dim objWriter As New System.IO.StreamWriter(PathDefalut & "\txt.txt")
        objWriter.Close()

        If System.IO.File.Exists(PathDefalut & "\txt.txt") = True Then
            System.IO.File.Create(PathDefalut & "\txt.txt").Dispose()
        Else
            System.IO.File.Create(PathDefalut & "\txt.txt").Close()
        End If
        objWriter = New System.IO.StreamWriter(PathDefalut & "\txt.txt")

        Dim dt1 As New DataTable

        If dt.Rows.Count > 0 Then
            For j As Integer = 0 To dt.Rows.Count - 1
                Dim CurrentBatchno As String = ""
                Dim PreviousBatchno As String = ""
                Dim PreviousCount As Integer = 0
                strsql = " SELECT DISTINCT I.BATCHNO from " & cnAdminDb & "..CUSTOMERINFO AS C "
                strsql += " ," & cnStockDb & "..ISSUE AS I where C.BATCHNO = I.BATCHNO "
                strsql += " AND PSNO = '" & dt.Rows(j).Item("PSNO").ToString & "' "
                If chkCostId <> "" And chkCostId <> "ALL" Then
                    strsql += vbCrLf + " AND COSTID IN (" & chkCostId & ")"
                End If
                dt1 = GetSqlTable(strsql, cn)
                If dt1.Rows.Count > 0 Then
                    PreviousCount = Val(dt1.Compute("COUNT(BATCHNO)", "").ToString)
                    If PreviousCount > 1 Then
                        PreviousBatchno = dt1.Rows(PreviousCount - 2).Item("BATCHNO").ToString
                    Else
                        PreviousBatchno = dt1.Rows(PreviousCount - 1).Item("BATCHNO").ToString
                    End If
                    CurrentBatchno = dt1.Rows(PreviousCount - 1).Item("BATCHNO").ToString
                End If
                objWriter.WriteLine(dt.Rows(j).Item("PSNO").ToString & "," & CurrentBatchno & "," & PreviousBatchno & "," & PreviousCount & "," & "R", True)
            Next
        End If
        objWriter.Close()


        strsql = " CREATE TABLE TEMPTABLEDB..TEMPCUSTOMERINFO" & systemId & "(PSNO VARCHAR(15),BATCHNO VARCHAR(15)"
        strsql += " , PREBATCHNO VARCHAR(15), PRECOUNT INT, TYPE VARCHAR(1))"
        cmd = New OleDbCommand(strsql, cn, tran)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " BULK "
        strsql += vbCrLf + "INSERT TEMPTABLEDB..TEMPCUSTOMERINFO" & systemId & ""
        strsql += vbCrLf + "FROM '" & PathDefalut & "\txt.txt" & "'"
        strsql += vbCrLf + "WITH"
        strsql += vbCrLf + "("
        strsql += vbCrLf + "FIELDTERMINATOR = ',',"
        strsql += vbCrLf + "ROWTERMINATOR = '\n'"
        strsql += vbCrLf + ")"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT (SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.PREBATCHNO) LASTTRANDATE "
        strsql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO) CURRENTTRANDATE"
        strsql += vbCrLf + " , BATCHNO"
        strsql += vbCrLf + " , PREBATCHNO"
        strsql += vbCrLf + " , PRECOUNT "
        If rbtNewCustomer.Checked = True Then strsql += vbCrLf + " ,'N' COLHEAD"
        If rbtOldCustomer.Checked = True Then strsql += vbCrLf + " ,'O' COLHEAD"
        If rbtLostCustomer.Checked = True Then strsql += vbCrLf + " ,'L' COLHEAD"
        strsql += vbCrLf + " ,P.PNAME,P.MOBILE"
        strsql += vbCrLf + " " & MoreDetail() & " "
        strsql += vbCrLf + " INTO TEMPTABLEDB..TEMPTRACKINGCUSTOMER" & systemId & " "
        strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO" & systemId & " AS I "
        strsql += vbCrLf + " ," & cnAdminDb & "..PERSONALINFO AS P "
        strsql += vbCrLf + " WHERE P.SNO = I.PSNO "
        If txtSearchBy.Text.Trim <> "" And cmbSearchBy.Text.Trim <> "NOOFVISIT" Then
            strsql += vbCrLf + " AND P." & cmbSearchBy.Text & " Like '%" & txtSearchBy.Text & "%'"
        End If
        If rbtNewCustomer.Checked = True Then
            strsql += vbCrLf + " AND I.PRECOUNT = 1 "
        ElseIf rbtOldCustomer.Checked = True Then
            If txtSearchBy.Text.Trim <> "" And cmbSearchBy.Text.Trim = "NOOFVISIT" Then
                strsql += vbCrLf + " AND I.PRECOUNT > " & Val(txtSearchBy.Text) & ""
            Else
                strsql += vbCrLf + " AND I.PRECOUNT >1 "
            End If
        ElseIf rbtLostCustomer.Checked = True Then
        End If
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " SELECT * FROM TEMPTABLEDB..TEMPTRACKINGCUSTOMER" & systemId & ""
        Dim dtResult As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtResult)
        If dtResult.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dtResult
                Dim strCount As String = ""
                If rbtNewCustomer.Checked = True Then
                    strCount = "NEW CUSTOMER"
                ElseIf rbtOldCustomer.Checked = True Then
                    strCount = "OLD CUSTOMER"
                Else
                    strCount = "LOST CUSTOMER"
                End If
                lblcount1.Text = strCount & " - " & Val(dtResult.Compute("COUNT(COLHEAD)", "").ToString)
                FormatGridColumns(gridView, False, True, True, True)
                .Columns("COLHEAD").Visible = False
                .Columns("PREBATCHNO").Visible = False
                .Columns("BATCHNO").Visible = False
                .Columns("PRECOUNT").Visible = False
                'AutoResizeToolStripMenuItem_Click(Me, New System.EventArgs)
            End With
            dropTable()
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            dropTable()
            lblcount1.Text = "..."
            gridView.DataSource = Nothing
            gridDetail.DataSource = Nothing
            dtpFrom.Focus()
            Exit Sub
        End If

        btnView_Search.Enabled = True

        If System.IO.File.Exists(PathDefalut & "\txt.txt") = True Then
            System.IO.File.Create(PathDefalut & "\txt.txt").Dispose()
        Else
            System.IO.File.Create(PathDefalut & "\txt.txt").Close()
        End If

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        cmbSearchBy.Items.Clear()
        cmbSearchBy.Items.Add("NONE")
        cmbSearchBy.Items.Add("MOBILE")
        cmbSearchBy.Items.Add("PNAME")
        cmbSearchBy.Items.Add("AREA")
        cmbSearchBy.Items.Add("CITY")
        cmbSearchBy.Items.Add("STATE")
        cmbSearchBy.Items.Add("PINCODE")
        cmbSearchBy.Items.Add("PHONERES")
        cmbSearchBy.Items.Add("NOOFVISIT")
        cmbSearchBy.Text = "MOBILE"
        strsql = " SELECT COMPANYNAME,COMPANYID FROM ( "
        strsql += " SELECT COMPANYNAME,COMPANYID,1 RESULT FROM " & cnAdminDb & "..COMPANY "
        strsql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strsql += " UNION ALL"
        strsql += " SELECT 'ALL' COMPANYNAME, '0' COMPANYID, 0 RESULT "
        strsql += " )X "
        strsql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        strsql = " SELECT COSTNAME,COSTID FROM ("
        strsql += " SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE "
        strsql += " UNION ALL"
        strsql += " SELECT 'ALL' COSTNAME, 'ALL' COSTID, 0 RESULT"
        strsql += " )X "
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostId)
        If chkCmbCostCentre.Text = "" Then
            chkCmbCostCentre.Text = "ALL"
        End If
        dropTable()

        lblcount1.Text = "..."

        strsql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME  IN (SELECT NAME FROM MASTER..SYSDATABASES)"
        dtdbName = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtdbName)

        check()
        gridDetail.DataSource = Nothing


        btnView_Search.Enabled = True
        dtpFrom.Focus()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "TRACKING CUSTOMER", gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "TRACKING CUSTOMER", gridView, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "FORM LOAD"
    Private Sub frmTrackingCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmTrackingCustomer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub detail2()

    End Sub

    Private Sub Detail1(ByVal mobile As String, ByVal name As String)
        strsql = vbCrLf + " SELECT CONVERT(varchar(15),I.TRANDATE,103) TRANDATE,TRANNO,SUM(PCS) PCS"
        strsql += vbCrLf + " , SUM(GRSWT) GRSWT,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX"
        strsql += vbCrLf + " ,I.COSTID"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strsql += vbCrLf + " ," & cnAdminDb & "..CUSTOMERINFO AS C"
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
        strsql += vbCrLf + " WHERE I.BATCHNO = C.BATCHNO"
        strsql += vbCrLf + " And ISNULL(I.CANCEL,'') = ''"
        strsql += vbCrLf + " AND I.TRANTYPE IN('SA','RD','OD')"
        strsql += vbCrLf + " AND P.MOBILE = '" & mobile & "'"
        strsql += vbCrLf + " AND P.PNAME = '" & name & "'"
        strsql += vbCrLf + " GROUP BY I.TRANDATE, TRANNO,I.COSTID"
        dt = New DataTable
        dt.Columns.Add("SNO", GetType(Integer))
        dt.Columns("SNO").AutoIncrement = True
        dt.Columns("SNO").AutoIncrementStep = 1
        dt.Columns("SNO").AutoIncrementSeed = 1
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridDetail
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridView, False, False, True, True)
                .Columns("AMOUNT").DefaultCellStyle.BackColor = Color.Lavender
                .Columns("TAX").DefaultCellStyle.BackColor = Color.LavenderBlush
            End With
        Else
            gridDetail.DataSource = Nothing
        End If
    End Sub

    Function GetTable() As DataTable
        Dim table As New DataTable
        table.Columns.Add("BILLNO", GetType(Integer))
        table.Columns.Add("BILLDATE", GetType(DateTime))
        table.Columns.Add("COSTID", GetType(String))
        table.Columns.Add("CANCEL", GetType(String))
        table.Columns.Add("TYPE", GetType(String))
        table.Columns.Add("TRANTYPE", GetType(String))

        Return table
    End Function

    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        If gridView.Rows.Count > 0 Then
            If e.KeyCode = Keys.D Then
                With gridView
                    dtgetTable = New DataTable
                    dtgetTable = GetTable()
                    gridDetail.DataSource = Nothing
                    If rbtCustomerWise.Checked = True Or rbtLiveCustomer.Checked = True Then
                        If rbtCustomerWise.Checked = True Then
                            'CUSTOMERWISE
                        ElseIf rbtLiveCustomer.Checked = True Then
                            dtdbName = New DataTable
                            strsql = " SELECT NAME DBNAME FROM MASTER.SYS.DATABASES WHERE NAME IN (SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER "
                            If rbtNewCustomer.Checked = True Or rbtOldCustomer.Checked = True Then
                                strsql += " where dbname in ('" & cnStockDb & "')"
                            Else
                                strsql += " where dbname not in ('" & cnStockDb & "')"
                            End If
                            strsql += " )"
                            da = New OleDbDataAdapter(strsql, cn)
                            da.Fill(dtdbName)
                        End If

                        strsql = "SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO ='" & .CurrentRow.Cells("PSNO").Value.ToString & "'"
                        da = New OleDbDataAdapter(strsql, cn)
                        dt = New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            For i As Integer = 0 To dt.Rows.Count - 1
                                For j As Integer = 0 To dtdbName.Rows.Count - 1
                                    Dim dt1 As New DataTable
                                    strsql = " SELECT DISTINCT TRANDATE,TRANNO,ISNULL(COSTID,'') COSTID,ISNULL(CANCEL,'') CANCEL,'ISSUE' [TYPE],TRANTYPE "
                                    strsql += vbCrLf + " FROM " & dtdbName.Rows(j).Item("DBNAME").ToString & "..ISSUE "
                                    strsql += vbCrLf + " WHERE BATCHNO = '" & dt.Rows(i).Item("BATCHNO").ToString & "'"
                                    strsql += vbCrLf + " UNION ALL "
                                    strsql += vbCrLf + " SELECT DISTINCT TRANDATE,TRANNO,ISNULL(COSTID,'')COSTID,ISNULL(CANCEL,'') CANCEL,'RECEIPT' [TYPE],TRANTYPE "
                                    strsql += vbCrLf + " FROM " & dtdbName.Rows(j).Item("DBNAME").ToString & "..RECEIPT "
                                    strsql += vbCrLf + " WHERE BATCHNO = '" & dt.Rows(i).Item("BATCHNO").ToString & "'"

                                    da = New OleDbDataAdapter(strsql, cn)
                                    da.Fill(dt1)
                                    If dt1.Rows.Count > 0 Then
                                        Dim dr As DataRow = Nothing
                                        dr = dtgetTable.NewRow
                                        dr!BillNo = dt1.Rows(0).Item("TRANNO").ToString
                                        dr!BILLDATE = dt1.Rows(0).Item("TRANDATE").ToString
                                        dr!COSTID = dt1.Rows(0).Item("COSTID").ToString
                                        dr!CANCEL = dt1.Rows(0).Item("CANCEL").ToString
                                        dr!TYPE = dt1.Rows(0).Item("TYPE").ToString
                                        dr!TRANTYPE = dt1.Rows(0).Item("TRANTYPE").ToString
                                        dtgetTable.Rows.Add(dr)
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                        If dtgetTable.Rows.Count > 0 Then
                            With gridDetail
                                .DataSource = Nothing
                                .DataSource = dtgetTable
                                FormatGridColumns(gridView, False, False, True, True)
                            End With
                        End If
                    Else
                        Detail1(.CurrentRow.Cells("MOBILE").Value.ToString, .CurrentRow.Cells("PNAME").Value.ToString)
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
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

    Private Sub cmbSearchBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearchBy.SelectedIndexChanged
        If cmbSearchBy.Text = "NONE" Then
            txtSearchBy.Visible = False
            txtSearchBy.Text = ""
        Else
            txtSearchBy.Visible = True
            txtSearchBy.Text = ""
        End If
    End Sub

    Private Sub chkboxvalid(ByVal bool As Boolean)
        chkAddress.Checked = bool
        chkArea.Checked = bool
        chkCity.Checked = bool
        chkState.Checked = bool
        chkPincode.Checked = bool
        chkCountry.Checked = bool
        chkPhoneRes.Checked = bool
    End Sub

    Private Sub chkMore_CheckedChanged(sender As Object, e As EventArgs) Handles chkMore.CheckedChanged
        If chkMore.Checked = True Then
            grpDetail.Visible = True
            chkboxvalid(True)
        Else
            grpDetail.Visible = False
            chkboxvalid(False)
        End If
    End Sub

    Private Sub wasteQuery()
        'strsql = vbCrLf + " SELECT DISTINCT COUNT(P.MOBILE) [PURCHASECOUNT]"
        '' strsql += vbCrLf + ", 'BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'' TRANDATE"
        'If rbtNewCustomer.Checked = True Then strsql += vbCrLf + " ,'N' COLHEAD"
        'If rbtOldCustomer.Checked = True Then strsql += vbCrLf + " ,'O' COLHEAD"
        'If rbtLostCustomer.Checked = True Then strsql += vbCrLf + " ,'L' COLHEAD"
        'strsql += vbCrLf + " ,P.PNAME,P.MOBILE"
        'strsql += vbCrLf + " " & MoreDetail() & " "
        'strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUEBATCHNO" & systemId & " AS I "
        'strsql += vbCrLf + " ," & cnAdminDb & "..CUSTOMERINFO AS C"
        'strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
        'strsql += vbCrLf + " WHERE I.BATCHNO = C.BATCHNO"
        'If txtSearchBy.Text.Trim <> "" And cmbSearchBy.Text.Trim <> "NOOFVISIT" Then
        '    strsql += vbCrLf + " And P." & cmbSearchBy.Text & " Like '%" & txtSearchBy.Text & "%'"
        'End If
        'strsql += vbCrLf + " GROUP BY P.PNAME,P.MOBILE"
        'strsql += vbCrLf + " " & MoreDetail() & " "
        'If rbtNewCustomer.Checked = True Then
        '    strsql += vbCrLf + " HAVING COUNT(P.MOBILE) = 1 "
        'ElseIf rbtOldCustomer.Checked = True Then
        '    strsql += vbCrLf + " HAVING  "
        '    If txtSearchBy.Text.Trim <> "" And cmbSearchBy.Text.Trim = "NOOFVISIT" Then
        '        strsql += vbCrLf + " COUNT(P.MOBILE) > " & Val(txtSearchBy.Text) & ""
        '    Else
        '        strsql += vbCrLf + " COUNT(P.MOBILE) >1"
        '    End If
        'ElseIf rbtLostCustomer.Checked = True Then
        '    strsql += vbCrLf + " "
        'End If
    End Sub

    Private Sub check()
        rbtNewCustomer.Checked = True
        If rbtCustomerWise.Checked = True Or rbtLiveCustomer.Checked = True Then
            Label1.Text = "DbType"
            dtpFrom.Enabled = False
            Label3.Enabled = False
            dtpTo.Enabled = False
        Else
            Label1.Text = "From"
            dtpFrom.Enabled = True
            Label3.Enabled = True
            dtpTo.Enabled = True
            dtpFrom.Focus()
        End If
        If rbtLiveCustomer.Checked = True Then
            rbtLostCustomer.Visible = True
        Else
            rbtLostCustomer.Visible = False
        End If
        check2()
    End Sub

    Private Sub check2()
        rbtLiveCustomer.Text = "Live Db Customer"
        If rbtLiveCustomer.Checked = True Then
            If rbtNewCustomer.Checked = True Or rbtOldCustomer.Checked = True Then
                rbtLiveCustomer.Text = "Live Db Customer"
            Else
                rbtLiveCustomer.Text = "Old Db Customer"
            End If
        End If
    End Sub
    Private Sub chkDbname_CheckedChanged(sender As Object, e As EventArgs)
        check()
    End Sub

    Private Function VIEW2fILTER() As String
        Dim Qry As String = ""
        If chkMore.Checked = True Then
            If chkAddress.Checked = True Then
                Qry += vbCrLf + ",P.DOORNO , P.ADDRESS1, P.ADDRESS2, P.ADDRESS3"
            End If

            If chkArea.Checked = True Then
                Qry += vbCrLf + " ,P.AREA "
            End If
            If chkCity.Checked = True Then
                Qry += vbCrLf + " ,P.CITY "
            End If
            If chkState.Checked = True Then
                Qry += vbCrLf + " , P.STATE"
            End If
            If chkPincode.Checked = True Then
                Qry += vbCrLf + " , P.PINCODE"
            End If
            If chkCountry.Checked = True Then
                Qry += vbCrLf + " , P.COUNTRY"
            End If
            If chkPhoneRes.Checked = True Then
                Qry += vbCrLf + " , P.PHONERES, P.MOBILE"
            End If
        End If
        Return Qry
    End Function

    Private Sub View2()
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True, ",")
        gridView.DataSource = Nothing
        strsql = vbCrLf + " SELECT P.SNO,C.PSNO, P.PNAME"
        strsql += vbCrLf + VIEW2fILTER()
        strsql += vbCrLf + ",COUNT(PSNO) NOOFCOUNT FROM " & cnAdminDb & "..PERSONALINFO AS P"
        strsql += vbCrLf + "," & cnAdminDb & "..CUSTOMERINFO AS C"
        strsql += vbCrLf + " WHERE P.SNO = C.PSNO "
        strsql += vbCrLf + " AND ISNULL(P.ACCODE,'') = ''"
        If chkCmbCostCentre.Text <> "ALL" Then
            strsql += vbCrLf + " AND ISNULL(C.COSTID,'') IN (" & chkCostId & ")"
        End If
        If txtSearchBy.Text.Trim <> "" Then
            strsql += vbCrLf + " And P." & cmbSearchBy.Text & " LIKE '%" & txtSearchBy.Text.Trim & "%' "
        End If
        strsql += vbCrLf + "GROUP BY P.SNO, C.PSNO,P.PNAME"
        strsql += vbCrLf + VIEW2fILTER()
        If rbtNewCustomer.Checked = True Then
            ' strsql += vbCrLf + "HAVING COUNT(PSNO) = 1"
            strsql += vbCrLf + " HAVING (SELECT COUNT(MOBILE) FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE = P.MOBILE) = 1"
        Else
            ' strsql += vbCrLf + "HAVING COUNT(PSNO) >1 "
            strsql += vbCrLf + " HAVING (SELECT COUNT(MOBILE) FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE = P.MOBILE) > 1"
        End If
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                .Columns("SNO").Visible = False
                .Columns("PSNO").Visible = False
                FormatGridColumns(gridView, False, True, True, True)
                'AutoResizeToolStripMenuItem_Click(Me, New System.EventArgs)
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            chkCmbCompany.Focus()
        End If
    End Sub


    Private Sub View3()
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True, ",")

        strsql = "IF OBJECT_ID('TEMPTABLEDB..TEMPBATCHNO','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPBATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = "CREATE TABLE TEMPTABLEDB..TEMPBATCHNO( BATCHNO VARCHAR(15))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " SELECT NAME FROM MASTER.SYS.DATABASES WHERE NAME IN (SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER "
        If rbtNewCustomer.Checked = True Or rbtOldCustomer.Checked = True Then
            strsql += " where dbname in ('" & cnStockDb & "')"
        Else
            strsql += " where dbname not in ('" & cnStockDb & "')"
        End If
        strsql += " )"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                strsql = "INSERT INTO TEMPTABLEDB..TEMPBATCHNO(BATCHNO)"
                strsql += " SELECT DISTINCT BATCHNO FROM " & dt.Rows(i).Item("name").ToString & "..ISSUE WHERE TRANTYPE IN ('SA','RD','OD') AND ISNULL(CANCEL,'')=''"
                If chkCostId <> "ALL" Then strsql += " AND COSTID IN ('" & chkCostId & "')"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            Next
        End If

        gridView.DataSource = Nothing
        strsql = vbCrLf + " SELECT P.SNO,C.PSNO, P.PNAME"
        strsql += vbCrLf + VIEW2fILTER()
        strsql += vbCrLf + ",COUNT(PSNO) NOOFCOUNT FROM " & cnAdminDb & "..PERSONALINFO AS P"
        strsql += vbCrLf + "," & cnAdminDb & "..CUSTOMERINFO AS C"
        strsql += vbCrLf + " WHERE P.SNO = C.PSNO "
        strsql += vbCrLf + " AND ISNULL(P.ACCODE,'') = ''"
        If chkCmbCostCentre.Text <> "ALL" Then
            strsql += vbCrLf + " AND ISNULL(C.COSTID,'') IN (" & chkCostId & ")"
        End If
        If txtSearchBy.Text.Trim <> "" Then
            strsql += vbCrLf + " And P." & cmbSearchBy.Text & " LIKE '%" & txtSearchBy.Text.Trim & "%' "
        End If
        strsql += vbCrLf + " AND BATCHNO in (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMPBATCHNO)"
        strsql += vbCrLf + "GROUP BY P.SNO, C.PSNO,P.PNAME"
        strsql += vbCrLf + VIEW2fILTER()
        If rbtNewCustomer.Checked = True Then
            strsql += vbCrLf + " HAVING (SELECT COUNT(MOBILE) FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE = P.MOBILE) = 1"
        Else
            strsql += vbCrLf + " HAVING (SELECT COUNT(MOBILE) FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE = P.MOBILE) > 1"
        End If
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                .Columns("SNO").Visible = False
                .Columns("PSNO").Visible = False
                FormatGridColumns(gridView, False, True, True, True)
                'AutoResizeToolStripMenuItem_Click(Me, New System.EventArgs)
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            chkCmbCompany.Focus()
        End If
    End Sub

    Private Sub btnView_Search_Click(sender As Object, e As EventArgs) Handles btnView_Search.Click
        If rbtNewCustomer.Checked = False And rbtOldCustomer.Checked = False And rbtLostCustomer.Checked = False Then
            MsgBox("select any one ", MsgBoxStyle.Information)
            rbtNewCustomer.Focus()
            Exit Sub
        End If
        If rbtCustomerWise.Checked = True Then
            View2()
        ElseIf rbtDateWise.Checked = True Then
            View1()
        ElseIf rbtLiveCustomer.Checked = True Then
            View3()
        End If
    End Sub

    Private Sub btnTemplate_Click(sender As Object, e As EventArgs) Handles btnTemplate.Click
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        'Dim oRng As Excel.Range
        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet
        oSheet.Range("A1").Value = "MOBILE"
        oSheet.Range("A1").ColumnWidth = 25
        oSheet.Range("A1:C1:H1").Font.Bold = True
        oSheet.Range("A1:C1:H1").Font.Name = "VERDANA"
        oSheet.Range("A1:C1:H1").Font.Size = 8
        oSheet.Range("A1:C1:H1").HorizontalAlignment = Excel.Constants.xlCenter
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        Str += "|(*.csv) | *.csv"
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExcel(path)
            End If
        End If
    End Sub

    Function GetTable1() As DataTable
        Dim table As New DataTable
        ' Create four typed columns in the DataTable.
        table.Columns.Add("MOBILE", GetType(String))
        table.Columns.Add("STATUS", GetType(String))
        Return table
    End Function

    Private Sub LoadFromExcel(ByVal _path As String)
        Dim dtTran As DataTable
        dtTran = GetTable1()
        Try
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & _path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=2;""")
            strsql = "SELECT * FROM [Mobile$]"
            da = New OleDbDataAdapter(strsql, MyConnection)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dr As DataRow = Nothing
                    strsql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE = '" & dt.Rows(i).Item("MOBILE").ToString & "'"
                    strsql += " OR PHONERES = '" & dt.Rows(i).Item("MOBILE").ToString & "'"
                    Dim chk As Integer = Val(GetSqlValue(cn, strsql).ToString)
                    dr = dtTran.NewRow
                    dr!MOBILE = dt.Rows(i).Item("MOBILE").ToString
                    If chk = 0 Then
                        dr!STATUS = "NOT AVAILABLE"
                    Else
                        dr!STATUS = "AVAILABLE"
                    End If
                    dtTran.Rows.Add(dr)
                Next
            End If
            If dtTran.Rows.Count > 0 Then
                gridView.DataSource = Nothing
                gridView.DataSource = dtTran
            End If
        Catch ex As Exception
            If ex.ToString.Contains("External table is not in the expected format") Then
                dtTran.Rows.Clear()
                dtTran.AcceptChanges()
                MsgBox("Please Open Excel Sheet ", MsgBoxStyle.Information)
                Exit Sub
            ElseIf ex.ToString.Contains("'Mobile' is not a valid name") Then
                MsgBox("Excel Sheet Name : Mobile Enter Correctly", MsgBoxStyle.Information)
                Exit Sub
            Else
                dtTran.Rows.Clear()
                dtTran.AcceptChanges()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                Exit Sub
            End If
        End Try
    End Sub

    Private Sub rbtFormat3_CheckedChanged(sender As Object, e As EventArgs) Handles rbtLiveCustomer.CheckedChanged
        check()
    End Sub

    Private Sub rbtFormat2_CheckedChanged(sender As Object, e As EventArgs) Handles rbtCustomerWise.CheckedChanged
        check()
    End Sub

    Private Sub rbtFormat1_CheckedChanged(sender As Object, e As EventArgs) Handles rbtDateWise.CheckedChanged
        check()
    End Sub

    Private Sub rbtLostCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles rbtLostCustomer.CheckedChanged
        check2()
    End Sub

    Private Sub rbtNewCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles rbtNewCustomer.CheckedChanged
        check2()
    End Sub

    Private Sub rbtOldCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles rbtOldCustomer.CheckedChanged
        check2()
    End Sub
#End Region
End Class