Imports System.Data.OleDb
Public Class frmPurchaseVATRpt
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String

    Private Sub frmPurchaseVATRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        LoadCompany(chkLstCompany)
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
        funcAddCmbItem()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Try
            'Me.Cursor = Cursors.WaitCursor
            btnView_Search.Enabled = False
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            Dim dtPurchaseVAT As New DataTable
            Dim dsPurchaseVAT As New DataSet
            Me.Refresh()

            Dim LOCOUT As String = Nothing
            Dim BOTH As String = Nothing

            If rbtLocal.Checked = True Then
                LOCOUT = "L"
            ElseIf rbtOutStation.Checked = True Then
                LOCOUT = "O"
            ElseIf rbtBoth.Checked = True Then
                LOCOUT = "B"
            End If

            Dim strChkCat As String = Nothing
            If chkCategoryWise.Checked = False Then
                strChkCat = "N"
            Else
                strChkCat = "Y"
            End If
            Dim GroupBy As String = Nothing
            If rbtSummaryWise.Checked = True Then
                GroupBy = "S"
            ElseIf rbtDateWise.Checked = True Then
                GroupBy = "D"
            ElseIf rbtMonth.Checked = True Then
                GroupBy = "M"
            ElseIf rbtBillNoWise.Checked = True Then
                GroupBy = "B"
            End If
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)

            ''cmd.CommandType = CommandType.StoredProcedure
            strSql = " EXEC " & cnStockDb & "..SP_RPT_PURCHASEVAT"
            strSql += " @DATEFROM = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,@DATETO = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,@LOCOUT ='" & LOCOUT & "'"
            strSql += " ,@ACNAME = '" & Replace(cmbPartyName.Text, "'", "''''") & "'"
            strSql += " ,@CATNAME = '" & Replace(cmbCategory.Text, "'", "''''") & "'"
            strSql += " ,@METALNAME = '" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += " ,@ANAMENALL ='" & Replace(cmbPartyName.Text, "'", "''''") & "'"
            strSql += ",@CNAMEALL='" & Replace(cmbCategory.Text, "'", "''''") & "'"
            strSql += " ,@METNAMEALL='" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += " ,@chkCat='" & strChkCat & "'"
            strSql += " ,@GroupBy='" & GroupBy & "'"
            strSql += "  ,@SystemID='" & systemId & "',@cnAdminDB='" & cnAdminDb & "',@cnStockDB='" & cnStockDb & "'"
            strSql += " ,@COMPANYID='" & SelectedCompany & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "PURVATREP')"
            strSql += " DROP TABLE TEMP" & systemId & "PURVATREP"
            strSql += " CREATE TABLE TEMP" & systemId & "PURVATREP("
            strSql += " PARTICULAR VARCHAR(50),"
            strSql += " PARTYNAME VARCHAR(50),"
            strSql += " VATNO VARCHAR(30),"
            strSql += " PCS INT,"
            strSql += " GRSWT NUMERIC(15,3),"
            strSql += " NETWT NUMERIC(15,3),"
            strSql += " AMOUNT NUMERIC(15,2),"
            strSql += " TAX NUMERIC(15,2),"
            strSql += " TOTAL NUMERIC(15,2),"
            strSql += " RESULT VARCHAR(1),"
            strSql += " CATEGORYNAME VARCHAR(50),"
            strSql += " CATCODE VARCHAR(30),"
            strSql += " TRANMONTHNO INT,"
            strSql += " TRANMONTH VARCHAR(3),"
            strSql += " TRANDATE SMALLDATETIME,"
            strSql += " BILLDATE VARCHAR(12),"
            strSql += " BILLNO VARCHAR(30),"
            strSql += " TRANNO INT,"
            strSql += " METALNAME VARCHAR(30),"
            strSql += " METALID VARCHAR(1),"
            strSql += " COLHEAD VARCHAR(1),"
            strSql += " SNO INT IDENTITY)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "ALTER TABLE TEMP" & systemId & "PURCHASEVATRES2 ADD PARTICULAR VARCHAR(150) "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If chkCategoryWise.Checked Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PURCHASEVATRES2)>0"
                strSql += " BEGIN "
                strSql += " INSERT INTO TEMP" & systemId & "PURCHASEVATRES2(PARTICULAR,CATEGORYNAME,"
                strSql += " CATCODE,RESULT,PARTYNAME,VATNO)"
                strSql += " SELECT DISTINCT CATEGORYNAME, CATEGORYNAME, CATCODE,0 RESULT,'','' "
                strSql += " FROM TEMP" & systemId & "PURCHASEVATRES2 WHERE RESULT = 1"
                strSql += " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PURCHASEVATRES2)>0"
            strSql += " BEGIN "
            strSql += " INSERT INTO TEMP" & systemId & "PURVATREP(PARTICULAR,PARTYNAME,VATNO,PCS,GRSWT,"
            strSql += " NETWT,AMOUNT,TAX,TOTAL,RESULT"
            If chkCategoryWise.Checked Then
                strSql += " ,CATEGORYNAME,CATCODE"
            Else
                strSql += " ,METALNAME,METALID"
            End If
            If rbtSummaryWise.Checked Then
                strSql += ")"
            ElseIf rbtMonth.Checked Then
                strSql += " ,TRANMONTHNO,TRANMONTH)"
            ElseIf rbtDateWise.Checked Then
                strSql += " ,BILLDATE,TRANDATE)"
            ElseIf rbtBillNoWise.Checked Then
                strSql += " ,BILLNO,TRANNO)"
            End If
            strSql += " SELECT PARTICULAR,PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,  "
            strSql += " TOTAL, RESULT"
            If chkCategoryWise.Checked Then
                strSql += " ,CATEGORYNAME,CATCODE"
            Else
                strSql += " ,METALNAME,METALID"
            End If
            If rbtMonth.Checked Then
                strSql += " ,TRANMONTHNO,TRANMONTH"
            ElseIf rbtDateWise.Checked Then
                strSql += " ,BILLDATE,TRANDATE"
            ElseIf rbtBillNoWise.Checked Then
                strSql += " ,BILLNO,TRANNO"
            End If
            strSql += " FROM TEMP" & systemId & "PURCHASEVATRES2 "
            If chkCategoryWise.Checked Then
                strSql += " ORDER BY CATCODE,RESULT"
            Else
                strSql += " ORDER BY METALID,RESULT"
            End If
            strSql += " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If chkCategoryWise.Checked Then
                strSql = " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = CATEGORYNAME WHERE RESULT = 2"
                If rbtSummaryWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = CATEGORYNAME WHERE RESULT = 1"
                ElseIf rbtMonth.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = TRANMONTH WHERE RESULT = 1"
                ElseIf rbtDateWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = BILLDATE WHERE RESULT = 1"
                ElseIf rbtBillNoWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = BILLNO WHERE RESULT = 1"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = METALNAME WHERE RESULT = 2"
                If rbtSummaryWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = METALNAME WHERE RESULT = 1"
                ElseIf rbtMonth.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = TRANMONTH WHERE RESULT = 1"
                ElseIf rbtDateWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = BILLDATE WHERE RESULT = 1"
                ElseIf rbtBillNoWise.Checked Then
                    strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PARTICULAR = BILLNO WHERE RESULT = 1"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            strSql = " UPDATE TEMP" & systemId & "PURVATREP SET COLHEAD = 'T' WHERE RESULT = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET COLHEAD = 'S' WHERE RESULT = 2"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET PCS = NULL WHERE PCS = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET GRSWT = NULL WHERE GRSWT = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET NETWT = NULL WHERE NETWT = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET AMOUNT = NULL WHERE AMOUNT = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET TAX = NULL WHERE TAX = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET TOTAL = NULL WHERE TOTAL = 0"
            strSql += " UPDATE TEMP" & systemId & "PURVATREP SET VATNO = NULL WHERE VATNO = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = "select * from TEMP" & systemId & "PURVATREP ORDER BY SNO"
            dtPurchaseVAT = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtPurchaseVAT)

            If dtPurchaseVAT.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If
            With gridView
                .DataSource = Nothing
                .DataSource = dtPurchaseVAT
                tabView.Show()
                GridViewFormat()
                funcGridStyle()
                .Columns("TRANMONTH").Visible = False
                .Columns("TRANMONTHNO").Visible = False
                .Columns("TRANDATE").Visible = False
                .Columns("BILLDATE").Visible = False
                .Columns("TRANNO").Visible = False
                .Columns("BILLNO").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                .Columns("CATEGORYNAME").Visible = False
                .Columns("CATCODE").Visible = False
                .Columns("METALNAME").Visible = False
                .Columns("METALID").Visible = False
            End With
            Dim strTitle As String = Nothing
            strTitle = "PURCHASEVAT REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If cmbPartyName.Text <> "ALL" And cmbPartyName.Text <> "" Then
                strTitle += " FOR " & cmbPartyName.Text & ""
            End If
            lblTitle.Text = strTitle
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Arrow
            btnView_Search.Enabled = True
        End Try
        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            Prop_Gets()
            dtpFrom.Select()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            rbtSummaryWise.Checked = True
            rbtBoth.Checked = True
            chkCategoryWise.Checked = True
            If cmbCategory.Items.Count > 0 Then
                cmbCategory.SelectedIndex = 0
            End If
            If cmbMetalName.Items.Count > 0 Then
                cmbMetalName.SelectedIndex = 0
            End If
            If cmbPartyName.Items.Count > 0 Then
                cmbPartyName.SelectedIndex = 0
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And Me.tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And Me.tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmPurchaseVATRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridPurchaseVAT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.X Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.P Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Function funcGridStyle() As Integer
        Try
            With gridView
                With .Columns("PARTICULAR")
                    If chkCategoryWise.Checked = True Then
                        If rbtSummaryWise.Checked Then
                            .HeaderText = "CATEGORY"
                        ElseIf rbtMonth.Checked Then
                            .HeaderText = "MONTH"
                        ElseIf rbtDateWise.Checked Then
                            .HeaderText = "DATE"
                        ElseIf rbtBillNoWise.Checked Then
                            .HeaderText = "BILLNO"
                        End If
                    Else
                        If rbtSummaryWise.Checked Then
                            .HeaderText = "METALNAME"
                        ElseIf rbtMonth.Checked Then
                            .HeaderText = "MONTH"
                        ElseIf rbtDateWise.Checked Then
                            .HeaderText = "DATE"
                        ElseIf rbtBillNoWise.Checked Then
                            .HeaderText = "BILLNO"
                        End If
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("PARTYNAME")
                    If rbtSummaryWise.Checked = True Then
                        .Visible = True
                    Else
                        .Visible = False
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("VATNO")
                    If rbtSummaryWise.Checked = True Then
                        .Visible = True
                    Else
                        .Visible = False
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("PCS")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 40
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("GRSWT")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("NETWT")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("AMOUNT")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("TAX")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("TOTAL")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                .Columns("RESULT").Visible = False
                If rbtMonth.Checked = True Then
                    .Columns("TRANMONTHNO").Visible = False
                    .Columns("TRANMONTH").Visible = False
                ElseIf rbtDateWise.Checked = True Then
                    .Columns("BILLDATE").Visible = False
                    .Columns("TRANDATE").Visible = False
                ElseIf rbtBillNoWise.Checked = True Then
                    .Columns("BILLNO").Visible = False
                    .Columns("TRANNO").Visible = False
                ElseIf rbtSummaryWise.Checked = True Then
                    .Columns("PARTYNAME").Visible = False
                ElseIf rbtSummaryWise.Checked = False Then
                    .Columns("PARTYNAME").Visible = True
                End If
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcAddCmbItem() As Integer
        Try
            strSql = "select distinct ACNAME from " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (5,6) ORDER BY ACNAME"
            cmbPartyName.Items.Clear()
            cmbPartyName.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbPartyName, False, False)
            cmbPartyName.SelectedIndex = 0
            strSql = "select distincT CATNAME from " & cnAdminDb & "..CATEGORY order by CATNAME"
            cmbCategory.Items.Clear()
            cmbCategory.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCategory, False, False)
            cmbCategory.SelectedIndex = 0
            strSql = "SELECT DISTINCT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
            cmbMetalName.Items.Clear()
            cmbMetalName.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbMetalName, False, False)
            cmbMetalName.SelectedIndex = 0
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    'Function funcAddData() As Integer
    '    Try
    '        strsql = " if(select 1 from sysobjects where name='TEMPPURCHASEVAT')>0"
    '        strsql += " drop table TEMPPURCHASEVAT"
    '        strsql += " select"
    '        strsql += " TRANNO,TRANDATE,ITEMID,CATCODE,PCS,GRSWT,NETWT,LESSWT,AMOUNT,TAX,METALID,ACCODE"
    '        strsql += " into TEMPPURCHASEVAT"
    '        strsql += " from "
    '        strsql += " " & cnStockDb & "..RECEIPT "
    '        strsql += " WHERE "
    '        strsql += " TRANDATE BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
    '        strsql += " AND TRANTYPE='RPU'"
    '        If rbtBoth.Checked = False Then
    '            strsql += " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST"
    '            If rbtLocal.Checked = True Then
    '                strsql += "='L')"
    '            ElseIf rbtOutStanding.Checked = True Then
    '                strsql += " <> 'L')"
    '            End If
    '        End If
    '        If cmbPartyName.Text <> "ALL" And cmbPartyName.Text <> "" Then
    '            strsql += " AND ACCODE=(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbPartyName.Text & "')"
    '        End If
    '        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
    '            strsql += " AND CATCODE=(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCategory.Text & "')"
    '        End If
    '        If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
    '            strsql += " AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName.Text & "')"
    '        End If
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.ExecuteNonQuery()

    '        strsql = " if(select 1 from sysobjects where name='TEMPPURCHASEVATRES1')>0"
    '        strsql += " drop table TEMPPURCHASEVATRES1"
    '        strsql += " SELECT "
    '        strsql += " ISNULL((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE),'') PARTYNAME"
    '        strsql += ",ISNULL((SELECT CASE WHEN LOCALTAXNO<>0 THEN LOCALTAXNO ELSE CENTRALTAXNO END FROM " & cnAdminDb & "..ACHEAD"
    '        strsql += "  WHERE ACCODE=X.ACCODE),0) VATNO"
    '        strsql += ",SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
    '        strsql += ",SUM(AMOUNT) AMOUNT,SUM(TAX) TAX,SUM(TOTAL) TOTAL,'1'RESULT"
    '        If chkCategoryWise.Checked = False Then
    '            If rbtSummaryWise.Checked = True Then
    '                ' --TRANNO WITH METALNAME
    '                strsql += ",METALNAME,METALID"
    '            ElseIf rbtMonth.Checked = True Then
    '                ' -- --TRANDATE WITH METALNAME
    '                strsql += ",TRANMONTH,METALNAME,METALID,TRANMONTHNO"
    '            ElseIf rbtDateWise.Checked = True Then
    '                ' -- --TRANDATE WITH METALNAME
    '                strsql += ",CONVERT(VARCHAR,TRANDATE,103) BILLDATE,METALNAME,METALID"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                ' -- --TRANNO WITH METALNAME
    '                strsql += ",CONVERT(VARCHAR,TRANNO) TRANNO,METALNAME,METALID"
    '            End If
    '        Else
    '            If rbtSummaryWise.Checked = True Then
    '                ' -- --TRANNO WITH CATEGORY
    '                strsql += ",CATEGORYNAME,CATCODE"
    '            ElseIf rbtMonth.Checked = True Then
    '                ' -- --TRANDATE WITH CATEGORY
    '                strsql += ",TRANMONTH,CATEGORYNAME,TRANMONTHNO,CATCODE"
    '            ElseIf rbtDateWise.Checked = True Then
    '                '-- --TRANDATE WITH CATEGORY"
    '                strsql += ",CONVERT(VARCHAR,TRANDATE,103) BILLDATE,CATEGORYNAME,CATCODE"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                ' -- --TRANNO WITH CATEGORY
    '                strsql += ",CONVERT(VARCHAR,TRANNO) TRANNO,CATEGORYNAME,CATCODE"
    '            End If
    '        End If
    '        strsql += " INTO TEMPPURCHASEVATRES1"
    '        strsql += " FROM"
    '        strsql += " ("
    '        strsql += " SELECT "
    '        strsql += " ACCODE,TRANNO,TRANDATE"
    '        strsql += " ,DATENAME(MONTH,TRANDATE)TRANMONTH"
    '        strsql += " ,DATEPART(MONTH,TRANDATE)TRANMONTHNO"
    '        strsql += " ,PCS,GRSWT,NETWT,AMOUNT,TAX"
    '        strsql += " ,(AMOUNT+TAX) TOTAL"
    '        strsql += " ,ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=P.CATCODE),'') CATEGORYNAME"
    '        strsql += " ,ISNULL((SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=P.METALID),'') METALNAME"
    '        strsql += " ,CATCODE,METALID"
    '        strsql += " FROM TEMPPURCHASEVAT AS P"
    '        strsql += " )X "
    '        If chkCategoryWise.Checked = False Then
    '            If rbtSummaryWise.Checked = True Then
    '                ' --SUMMARY WITH METALNAME"
    '                strsql += " GROUP BY ACCODE,METALNAME,METALID"
    '                strsql += " ORDER BY METALNAME,METALID,PARTYNAME"
    '            ElseIf rbtMonth.Checked = True Then
    '                ' -- --TRANMONTH WISE WITH METALNAME"
    '                strsql += " GROUP BY TRANMONTH,TRANMONTHNO,METALNAME,METALID,ACCODE"
    '                strsql += " ORDER BY METALNAME,METALID,TRANMONTHNO,PARTYNAME"
    '            ElseIf rbtDateWise.Checked = True Then
    '                ' -- --TRANDATE WISE WITH METALNAME"
    '                strsql += " GROUP BY TRANDATE,METALNAME,METALID,ACCODE"
    '                strsql += " ORDER BY METALNAME,METALID,TRANDATE,PARTYNAME"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                ' -- --TRANDATE WISE WITH METALNAME"
    '                strsql += " GROUP BY TRANNO,METALNAME,METALID,ACCODE"
    '                strsql += " ORDER BY METALNAME,METALID,TRANNO,PARTYNAME"
    '            End If
    '        Else
    '            If rbtSummaryWise.Checked = True Then
    '                ' -- --SUMMARY WITH CATEGORY"
    '                strsql += " GROUP BY ACCODE,CATEGORYNAME,CATCODE"
    '                strsql += " ORDER BY CATEGORYNAME,CATCODE,PARTYNAME"
    '            ElseIf rbtMonth.Checked = True Then
    '                ' -- --TRANDATEWISE WITH CATEGORYWISE"
    '                strsql += " GROUP BY TRANMONTH,TRANMONTHNO,CATEGORYNAME,CATCODE,ACCODE"
    '                strsql += " ORDER BY CATEGORYNAME,CATCODE,TRANMONTHNO,PARTYNAME"
    '            ElseIf rbtDateWise.Checked = True Then
    '                '-- --TRANDATEWISE WITH CATEGORYWISE"
    '                strsql += " GROUP BY TRANDATE,CATEGORYNAME,CATCODE,ACCODE"
    '                strsql += " ORDER BY CATEGORYNAME,CATCODE,TRANDATE,PARTYNAME"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                ' -- --TRANDATEWISE WITH CATEGORYWISE"
    '                strsql += " GROUP BY TRANNO,CATEGORYNAME,CATCODE,ACCODE"
    '                strsql += " ORDER BY CATEGORYNAME,CATCODE,TRANNO,PARTYNAME"
    '            End If
    '        End If
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.ExecuteNonQuery()
    '    Catch ex As Exception
    '        
    '        MsgBox(ex.Message)
    '        MsgBox(ex.StackTrace)
    '    End Try
    'End Function
    'Function funcAddSubTotal() As Integer
    '    Try
    '        strsql = "if (select count(*) from TEMPPURCHASEVATRES1)>0"
    '        strsql += " BEGIN "
    '        If chkCategoryWise.Checked = True Then
    '            If rbtSummaryWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,CATEGORYNAME,CATCODE FROM("
    '            ElseIf rbtDateWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,BILLDATE,CATEGORYNAME,CATCODE FROM("
    '            ElseIf rbtMonth.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,TRANMONTH,CATEGORYNAME,TRANMONTHNO,CATCODE FROM("
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,TRANNO,CATEGORYNAME,CATCODE FROM("
    '            End If
    '        Else
    '            If rbtSummaryWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,METALNAME,METALID FROM("
    '            ElseIf rbtDateWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,BILLDATE,METALNAME,METALID FROM("
    '            ElseIf rbtMonth.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,TRANMONTH,METALNAME,TRANMONTHNO,METALID FROM("
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                strsql += "select PARTYNAME,VATNO,PCS,GRSWT,NETWT,AMOUNT,TAX,TOTAL,RESULT,TRANNO,METALNAME,METALID FROM("
    '            End If
    '        End If
    '        strsql += " SELECT * FROM TEMPPURCHASEVATRES1"
    '        strsql += " UNION ALL"
    '        If chkCategoryWise.Checked = True Then
    '            strsql += " SELECT ''PARTYNAME,''VATNO,SUM(PCS) PCS"
    '            strsql += ",SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX,SUM(TOTAL) TOTAL,'2'RESULT"
    '            If rbtSummaryWise.Checked = True Then
    '                strsql += ",'SUB TOTAL'CATEGORYNAME,CATCODE"
    '            ElseIf rbtDateWise.Checked = True Then
    '                strsql += ",''BILLDATE,'SUB TOTAL'CATEGORYNAME,CATCODE"
    '            ElseIf rbtMonth.Checked = True Then
    '                strsql += ",''TRANMONTH,'SUB TOTAL'CATEGORYNAME,''TRANMONTHNO,CATCODE"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                strsql += ",''TRANNO,'SUB TOTAL'CATEGORYNAME,CATCODE"
    '            End If
    '            strsql += " FROM TEMPPURCHASEVATRES1 "
    '            strsql += " GROUP BY CATCODE"
    '            'strsql += " SELECT "
    '            'strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            'strsql += ",SUM(TOTAL) TOTAL,'2' RESULT"
    '            'strsql += " FROM TEMPPURCHASEVATRES1"
    '            'strsql += " GROUP BY CATEGORYNAME"
    '            'strsql += " SELECT "
    '            'strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            'strsql += ",SUM(TOTAL) TOTAL,'2' RESULT"
    '            'strsql += " FROM TEMPPURCHASEVATRES1"
    '            'strsql += " GROUP BY CATEGORYNAME"
    '            'strsql += " SELECT "
    '            'strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            'strsql += ",SUM(TOTAL) TOTAL,'2' RESULT"
    '            'strsql += " FROM TEMPPURCHASEVATRES1"
    '            'strsql += " GROUP BY CATEGORYNAME"
    '        Else
    '            strsql += " SELECT ''PARTYNAME,''VATNO,SUM(PCS) PCS"
    '            strsql += ",SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX,SUM(TOTAL) TOTAL,'2'RESULT"
    '            If rbtSummaryWise.Checked = True Then
    '                strsql += ",'SUB TOTAL'METALNAME,METALID"
    '            ElseIf rbtDateWise.Checked = True Then
    '                strsql += ",''BILLDATE,'SUB TOTAL'METALNAME,METALID"
    '            ElseIf rbtMonth.Checked = True Then
    '                strsql += ",''TRANMONTH,'SUB TOTAL'METALNAME,METALID,''TRANMONTHNO"
    '            ElseIf rbtBillNoWise.Checked = True Then
    '                strsql += ",''TRANNO,'SUB TOTAL'METALNAME,METALID"
    '            End If
    '            strsql += " FROM TEMPPURCHASEVATRES1 "
    '            strsql += " GROUP BY METALNAME,METALID"
    '            'If rbtSummaryWise.Checked = True Then
    '            '    strsql += " SELECT ''PARTYNAME,''VATNO,SUM(PCS) PCS"
    '            '    strsql += ",SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX,SUM(TOTAL) TOTAL,'2'RESULT,METALNAME"
    '            '    strsql += "  FROM TEMPPURCHASEVATRES1 "
    '            '    strsql += " GROUP BY METALNAME"
    '            'ElseIf rbtDateWise.Checked = True Then
    '            '    strsql += " SELECT "
    '            '    strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            '    strsql += ",SUM(TOTAL) TOTAL,'2' RESULT,''BILLDATE,METALNAME"
    '            '    strsql += " FROM TEMPPURCHASEVATRES1"
    '            '    strsql += " GROUP BY METALNAME"
    '            'ElseIf rbtMonth.Checked = True Then
    '            '    strsql += " SELECT "
    '            '    strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            '    strsql += ",SUM(TOTAL) TOTAL,'2' RESULT,''TRANMONTH,METALNAME,''TRANMONTHNO"
    '            '    strsql += " FROM TEMPPURCHASEVATRES1"
    '            '    strsql += " GROUP BY METALNAME"
    '            'ElseIf rbtBillNoWise.Checked = True Then
    '            '    strsql += " SELECT "
    '            '    strsql += " ''PARTYNAME,''VATNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(AMOUNT) AMOUNT,SUM(TAX) TAX"
    '            '    strsql += ",SUM(TOTAL) TOTAL,'2' RESULT,''TRANNO,METALNAME"
    '            '    strsql += " FROM TEMPPURCHASEVATRES1"
    '            '    strsql += " GROUP BY METALNAME"
    '            'End If
    '        End If
    '        strsql += " )X"
    '        If chkCategoryWise.Checked = True Then
    '            strsql += " ORDER BY CATCODE,RESULT"
    '        Else
    '            strsql += " ORDER BY METALID,RESULT"
    '        End If
    '        If rbtSummaryWise.Checked = True Then
    '            strsql += ",PARTYNAME"
    '        ElseIf rbtDateWise.Checked = True Then
    '            strsql += ",BILLDATE"
    '        ElseIf rbtMonth.Checked = True Then
    '            strsql += ",TRANMONTHNO"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += ",TRANNO"
    '        End If
    '        strsql += " END"
    '    Catch ex As Exception
    '        
    '        MsgBox(ex.Message)
    '        MsgBox(ex.StackTrace)
    '    End Try
    'End Function

    'da = New OleDbDataAdapter(cmd)
    'cmd.Connection = cn
    'cmd = New OleDbCommand("PURCHASEVATMAIN", cn)
    ''cmd.CommandText = "PURCHASEVATMAIN"
    'cmd.CommandType = CommandType.StoredProcedure
    'cmd.Parameters.Add(New OleDbParameter("@DATEFROM", dtpDateFrom.Value.Date.ToString("yyyy-MM-dd")))
    'cmd.Parameters.Add(New OleDbParameter("@DATETO", dtpDateTo.Value.Date.ToString("yyyy-MM-dd")))
    'cmd.Parameters.Add(New OleDbParameter("@LOCOUT", LOCOUT))
    'cmd.Parameters.Add(New OleDbParameter("@ACNAME", cmbPartyName.Text))
    'cmd.Parameters.Add(New OleDbParameter("@CATNAME", cmbCategory.Text))
    'cmd.Parameters.Add(New OleDbParameter("@METALNAME", cmbMetalName.Text))
    ''cmd.Parameters.Add(New OleDbParameter("@BOTH", BOTH))
    'cmd.Parameters.Add(New OleDbParameter("@ANAMENALL", cmbPartyName.Text))
    'cmd.Parameters.Add(New OleDbParameter("@CNAMEALL", cmbCategory.Text))
    'cmd.Parameters.Add(New OleDbParameter("@METNAMEALL", cmbMetalName.Text))
    'cmd.Parameters.Add(New OleDbParameter("@chkCat", strChkCat))
    'cmd.Parameters.Add(New OleDbParameter("@sum", strSum))
    'cmd.Parameters.Add(New OleDbParameter("@month", strMonth))
    'cmd.Parameters.Add(New OleDbParameter("@date", strDate))
    'cmd.Parameters.Add(New OleDbParameter("@billno", strBillno))
    'cmd.ExecuteNonQuery()
    'cmd = New OleDbCommand("PURCHASEVATMAIN1", cn)
    'cmd.CommandType = CommandType.StoredProcedure
    'da = New OleDbDataAdapter(cmd)

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
        Dim obj As New frmPurchaseVATRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPurchaseVATRpt_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        cmbPartyName.Text = obj.p_cmbPartyName
        cmbCategory.Text = obj.p_cmbCategory
        cmbMetalName.Text = obj.p_cmbMetalName
        rbtSummaryWise.Checked = obj.p_rbtSummaryWise
        rbtMonth.Checked = obj.p_rbtMonth
        rbtDateWise.Checked = obj.p_rbtDateWise
        rbtBillNoWise.Checked = obj.p_rbtBillNoWise
        rbtLocal.Checked = obj.p_rbtLocal
        rbtOutStation.Checked = obj.p_rbtOutStation
        rbtBoth.Checked = obj.p_rbtBoth
        chkCategoryWise.Checked = obj.p_chkCategoryWise

    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPurchaseVATRpt_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_cmbPartyName = cmbPartyName.Text
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_cmbMetalName = cmbMetalName.Text
        obj.p_rbtSummaryWise = rbtSummaryWise.Checked
        obj.p_rbtMonth = rbtMonth.Checked
        obj.p_rbtDateWise = rbtDateWise.Checked
        obj.p_rbtBillNoWise = rbtBillNoWise.Checked
        obj.p_rbtLocal = rbtLocal.Checked
        obj.p_rbtOutStation = rbtOutStation.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_chkCategoryWise = chkCategoryWise.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPurchaseVATRpt_Properties))
    End Sub
End Class

Public Class frmPurchaseVATRpt_Properties
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
    Private cmbPartyName As String = "ALL"
    Public Property p_cmbPartyName() As String
        Get
            Return cmbPartyName
        End Get
        Set(ByVal value As String)
            cmbPartyName = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private cmbMetalName As String = "ALL"
    Public Property p_cmbMetalName() As String
        Get
            Return cmbMetalName
        End Get
        Set(ByVal value As String)
            cmbMetalName = value
        End Set
    End Property
    Private rbtSummaryWise As Boolean = True
    Public Property p_rbtSummaryWise() As Boolean
        Get
            Return rbtSummaryWise
        End Get
        Set(ByVal value As Boolean)
            rbtSummaryWise = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDateWise As Boolean = False
    Public Property p_rbtDateWise() As Boolean
        Get
            Return rbtDateWise
        End Get
        Set(ByVal value As Boolean)
            rbtDateWise = value
        End Set
    End Property
    Private rbtBillNoWise As Boolean = False
    Public Property p_rbtBillNoWise() As Boolean
        Get
            Return rbtBillNoWise
        End Get
        Set(ByVal value As Boolean)
            rbtBillNoWise = value
        End Set
    End Property
    Private rbtLocal As Boolean = True
    Public Property p_rbtLocal() As Boolean
        Get
            Return rbtLocal
        End Get
        Set(ByVal value As Boolean)
            rbtLocal = value
        End Set
    End Property
    Private rbtOutStation As Boolean = False
    Public Property p_rbtOutStation() As Boolean
        Get
            Return rbtOutStation
        End Get
        Set(ByVal value As Boolean)
            rbtOutStation = value
        End Set
    End Property

    Private rbtBoth As Boolean = False
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private chkCategoryWise As Boolean = False
    Public Property p_chkCategoryWise() As Boolean
        Get
            Return chkCategoryWise
        End Get
        Set(ByVal value As Boolean)
            chkCategoryWise = value
        End Set
    End Property
End Class