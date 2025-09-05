Imports System.Data.OleDb
Public Class FrmStockSaleValueCheckRpt

    Dim dtSalesStock As New DataTable
    Dim dsSalesStock As New DataSet
    Dim strSql As String
    Dim cmd As OleDbCommand
    Private Sub FrmStockSaleValueCheckRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
        funcAddComboData()
    End Sub
    Private Sub FrmStockSaleValueCheckRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Try
            btnView_Search.Enabled = False
            dtSalesStock.Clear()
            dsSalesStock.Clear()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            Me.Refresh()
            Dim strHomeSales As String = Nothing
            If chkHomeSalesView.Checked = True Then
                strHomeSales = "Y"
            Else
                strHomeSales = "N"
            End If
            Dim ItemCtrId As String = ""
            Dim ItemctrName As String = GetChecked_CheckedList(chkcmbItemCtr)
            If ItemctrName <> "" Then
                strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & ItemctrName & ")"
                Dim dtTemp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                    ItemCtrId += "" & dtTemp.Rows(cnt).Item("ITEMCTRID").ToString & ""
                    If cnt <> dtTemp.Rows.Count - 1 Then
                        ItemCtrId += ","
                    End If
                Next
            End If
            ''cmd.CommandType = CommandType.StoredProcedure
            strSql = " EXECUTE " & cnStockDb & "..SP_RPT_STOCKSALEVALUECHECK"
            strSql += vbCrLf + " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@DATETO ='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COSTCENTRE='" & Replace(cmbCostCentre.Text, "'", "''''") & "'"
            strSql += vbCrLf + " ,@METALNAME='" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += vbCrLf + " ,@HOMESALES='" & strHomeSales & "'"
            strSql += vbCrLf + " ,@SYSTEMID='" & systemId & "'"
            strSql += vbCrLf + " ,@CNCOMPANYID='" & strCompanyId & "'"
            strSql += vbCrLf + " ,@TRANDB='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@TAGNO='" & txtTagNo.Text.Trim & "'"
            strSql += vbCrLf + " ,@ITEMCTRID='" & ItemCtrId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "STKSALVALCHK')"
            strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "STKSALVALCHK"
            strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "STKSALVALCHK("
            strSql += vbCrLf + " PARTICULAR VARCHAR(50),"
            strSql += vbCrLf + " BILLNO INT,"
            strSql += vbCrLf + " ITEMNAME VARCHAR(50),"
            strSql += vbCrLf + " TAGNO VARCHAR(20),"

            strSql += vbCrLf + " RATE NUMERIC(15,3),"
            strSql += vbCrLf + " BOARDRATE NUMERIC(15,3),"

            strSql += vbCrLf + " GRSWT NUMERIC(15,3),"
            strSql += vbCrLf + " NETWT NUMERIC(15,3),"

            strSql += vbCrLf + " TAGMINWASTAGE NUMERIC(15,3),"
            strSql += vbCrLf + " TAGMAXWASTAGE NUMERIC(15,3),"

            strSql += vbCrLf + " SALEWASTAGE NUMERIC(15,3),"

            strSql += vbCrLf + " TAGMINMC NUMERIC(15,3),"
            strSql += vbCrLf + " TAGMAXMC NUMERIC(15,3),"
            strSql += vbCrLf + " SALEMC NUMERIC(15,3),"

            strSql += vbCrLf + " MINVALUE NUMERIC(15,2),"
            strSql += vbCrLf + " MAXVALUE NUMERIC(15,2),"
            strSql += vbCrLf + " SALEVALUE NUMERIC(15,2),"
            strSql += vbCrLf + " DIFF NUMERIC(15,2),"
            strSql += vbCrLf + " DIFFPER NUMERIC(15,2),"
            strSql += vbCrLf + " DISCOUNT NUMERIC(15,2),"
            strSql += vbCrLf + " DISCOUNTDIFF NUMERIC(15,2),"
            strSql += vbCrLf + " SALESPERSON VARCHAR(50),"
            strSql += vbCrLf + " METALNAME VARCHAR(50),"
            strSql += vbCrLf + " COSTID VARCHAR(50),"
            strSql += vbCrLf + " COSTNAME VARCHAR(500),"
            strSql += vbCrLf + " ITEMCTRID VARCHAR(50),"
            strSql += vbCrLf + " ITEMCTRNAME VARCHAR(500),"
            strSql += vbCrLf + " STATUS VARCHAR(700),"
            strSql += vbCrLf + " RESULT INT,"
            strSql += vbCrLf + " COLHEAD VARCHAR(2),"
            strSql += vbCrLf + " PARTYSALES VARCHAR(1),"
            strSql += vbCrLf + " MULTIMETAL VARCHAR(1),"
            strSql += vbCrLf + " SNO INT IDENTITY)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " ALTER TABLE TEMP" & systemId & "FINALSTOCKSALEVALUECHECK ADD PARTICULAR VARCHAR(50)"
            strSql += " ALTER TABLE TEMP" & systemId & "FINALSTOCKSALEVALUECHECK ADD RESULT INT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If rdbBillNoWise.Checked = True Then
                strSql = " UPDATE TEMP" & systemId & "FINALSTOCKSALEVALUECHECK SET PARTICULAR = BILLNO "
            ElseIf rdbItemWise.Checked = True Then
                strSql = " UPDATE TEMP" & systemId & "FINALSTOCKSALEVALUECHECK SET PARTICULAR = ITEMNAME "
            End If
            strSql += " UPDATE TEMP" & systemId & "FINALSTOCKSALEVALUECHECK SET RESULT = 1 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "FINALSTOCKSALEVALUECHECK)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "STKSALVALCHK(PARTICULAR,BILLNO,ITEMNAME,TAGNO,MAXVALUE,"
            strSql += vbCrLf + " MINVALUE,SALEVALUE,DIFF,DIFFPER,DISCOUNT,DISCOUNTDIFF,SALESPERSON,METALNAME "
            strSql += vbCrLf + " ,COSTID,COSTNAME,ITEMCTRID,ITEMCTRNAME,STATUS"
            strSql += vbCrLf + " ,GRSWT,NETWT"
            strSql += vbCrLf + " ,TAGMINWASTAGE,TAGMAXWASTAGE,SALEWASTAGE,TAGMINMC,TAGMAXMC,SALEMC"
            strSql += vbCrLf + " ,RESULT,COLHEAD,PARTYSALES,MULTIMETAL,RATE,BOARDRATE)"
            strSql += vbCrLf + " SELECT PARTICULAR,BILLNO,ITEMNAME,TAGNO,MAXVALUE,"
            strSql += vbCrLf + " MINVALUE,SALEVALUE,DIFF,DIFFPER,DISCOUNT,DISCOUNTDIFF"
            strSql += vbCrLf + " ,SALESPERSON,METALNAME,COSTID,COSTNAME,ITEMCTRID,ITEMCTRNAME,STATUS"
            strSql += vbCrLf + " ,SALEGRSWT,SALENETWT"
            strSql += vbCrLf + " ,TAGMINWAST,TAGMAXWAST,SALEMAXWAST"
            strSql += vbCrLf + " ,TAGMINMC,TAGMAXMC,SALEMAXMC,1 RESULT,'D0' COLHEAD,PARTYSALES"
            strSql += vbCrLf + " ,MULTIMETAL,RATE,BOARDRATE"
            strSql += vbCrLf + " FROM TEMP" & systemId & "FINALSTOCKSALEVALUECHECK "
            If chkDifference.Checked = True Then
                If txtDifference.Text <> "" Then
                    strSql += vbCrLf + " WHERE DIFFPER >='" & Replace(txtDifference.Text, "'", "''''") & "'"
                End If
            End If
            If rdbBillNoWise.Checked = True Then
                strSql += vbCrLf + " ORDER BY METALNAME,BILLNO,ITEMNAME,TAGNO,RESULT"
            ElseIf rdbItemWise.Checked = True Then
                strSql += vbCrLf + " ORDER BY METALNAME,ITEMNAME,BILLNO,TAGNO,RESULT"
            End If
            strSql += vbCrLf + vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "STKSALVALCHK)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "STKSALVALCHK(PARTICULAR,METALNAME,"
            strSql += vbCrLf + " RESULT,DISCOUNTDIFF,DISCOUNT,MINVALUE,MAXVALUE,ITEMNAME,SALESPERSON,COLHEAD)"
            strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,-5 RESULT,0,0,0,0,'','','T1' COLHEAD "
            strSql += vbCrLf + " FROM TEMP" & systemId & "STKSALVALCHK WHERE RESULT=1"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If cmbGroupBy.Text = "NONE" Then
            ElseIf cmbGroupBy.Text = "ITEMCOUNTER" Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "STKSALVALCHK)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "STKSALVALCHK(PARTICULAR,ITEMCTRNAME,"
                strSql += vbCrLf + " RESULT,DISCOUNTDIFF,DISCOUNT,MINVALUE,MAXVALUE,ITEMNAME,SALESPERSON,ITEMCTRID,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,-4 RESULT,0 DISCOUNTDIFF"
                strSql += vbCrLf + " ,0 DISCOUNT,0 MINVALUE,0 MAXVALUE,'' ITEMNAME,'' SALESPERSON"
                strSql += vbCrLf + " ,ITEMCTRID,'T2' COLHEAD FROM TEMP" & systemId & "STKSALVALCHK WHERE RESULT=1"
                strSql += vbCrLf + " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "STKSALVALCHK)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "STKSALVALCHK(PARTICULAR,ITEMCTRNAME,"
                strSql += vbCrLf + " RESULT,DISCOUNTDIFF,DISCOUNT,MINVALUE,MAXVALUE "
                strSql += vbCrLf + " ,ITEMNAME,SALESPERSON,ITEMCTRID,COLHEAD "
                strSql += vbCrLf + " ,GRSWT"
                strSql += vbCrLf + " ,NETWT"
                strSql += vbCrLf + " ,SALEVALUE"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " SELECT ITEMCTRNAME + 'TOTAL ',ITEMCTRNAME + '-' +  LTRIM(ITEMCTRID),2 RESULT"
                strSql += vbCrLf + " ,SUM(DISCOUNTDIFF) DISCOUNTDIFF"
                strSql += vbCrLf + " ,SUM(DISCOUNT) DISCOUNT"
                strSql += vbCrLf + " ,SUM(MINVALUE) MINVALUE"
                strSql += vbCrLf + " ,SUM(MAXVALUE) MAXVALUE"
                strSql += vbCrLf + " ,'' ITEMNAME"
                strSql += vbCrLf + " ,'' SALESPERSON"
                strSql += vbCrLf + " ,ITEMCTRID,'S1' COLHEAD "
                strSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
                strSql += vbCrLf + " ,SUM(NETWT) NETWT"
                strSql += vbCrLf + " ,SUM(SALEVALUE) SALEVALUE"
                strSql += vbCrLf + " FROM TEMP" & systemId & "STKSALVALCHK WHERE RESULT=1"
                strSql += vbCrLf + " GROUP BY ITEMCTRNAME,ITEMCTRID"
                strSql += vbCrLf + " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = " UPDATE TEMP" + systemId + "STKSALVALCHK SET COLHEAD = 'T' WHERE RESULT = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET MAXVALUE = NULL WHERE MAXVALUE = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET MINVALUE = NULL WHERE MINVALUE = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET SALEVALUE = NULL WHERE SALEVALUE = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET DIFF = NULL WHERE DIFF = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET DIFFPER = NULL WHERE DIFFPER = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET DISCOUNT = NULL WHERE DISCOUNT = 0 "
            strSql += vbCrLf + " UPDATE TEMP" + systemId + "STKSALVALCHK SET DISCOUNTDIFF = NULL WHERE DISCOUNTDIFF= 0 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "SELECT * FROM TEMP" + systemId + "STKSALVALCHK "
            If rdbBillNoWise.Checked = True Then
                If cmbGroupBy.Text = "NONE" Then
                    strSql += vbCrLf + " ORDER BY METALNAME,BILLNO,ITEMNAME,TAGNO,RESULT"
                ElseIf cmbGroupBy.Text = "ITEMCOUNTER" Then
                    strSql += vbCrLf + " ORDER BY ITEMCTRNAME,RESULT,ITEMCTRID,METALNAME,BILLNO,ITEMNAME,TAGNO"
                Else
                    strSql += vbCrLf + " ORDER BY METALNAME,BILLNO,ITEMNAME,TAGNO,RESULT"
                End If
            ElseIf rdbItemWise.Checked = True Then
                If cmbGroupBy.Text = "NONE" Then
                    strSql += vbCrLf + " ORDER BY METALNAME,ITEMNAME,BILLNO,TAGNO,RESULT"
                ElseIf cmbGroupBy.Text = "ITEMCOUNTER" Then
                    strSql += vbCrLf + " ORDER BY ITEMCTRNAME,RESULT,ITEMCTRID,METALNAME,ITEMNAME,BILLNO,TAGNO"
                Else
                    strSql += vbCrLf + " ORDER BY METALNAME,ITEMNAME,BILLNO,TAGNO,RESULT"
                End If
            End If
            dtSalesStock = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSalesStock)
            If dtSalesStock.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records Not Found..", MsgBoxStyle.Information, "MESSAGE")
                Exit Sub
            End If
            gridView.DataSource = dtSalesStock
            tabView.Show()
            GridViewFormat()
            funcGridStyle()
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            Dim strTitle As String = Nothing
            strTitle = "STOCK-SALE VALUE CHECK REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strTitle += " FOR " & cmbCostCentre.Text & ""
            End If
            If cmbGroupBy.Text <> "" And cmbGroupBy.Text <> "ALL" And cmbGroupBy.Text <> "NONE" Then
                strTitle += " GROUP BY " & cmbGroupBy.Text & ""
            End If
            If ItemctrName <> "ALL" And ItemctrName <> "" And ItemctrName <> "'ALL'" Then
                strTitle += "" & ItemctrName
            End If
            strTitle += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")
            lblTitle.Text = strTitle
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub
    Private Sub LoadItemCounter()
        strSql = " SELECT 'ALL' ITEMCTRNAME,1 ORD UNION ALL SELECT ITEMCTRNAME,2 ORD FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ORD,ITEMCTRNAME "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCounter As New DataTable
        da.Fill(dtCounter)
        chkcmbItemCtr.Items.Clear()
        If dtCounter.Rows.Count > 0 Then
            For i As Integer = 0 To dtCounter.Rows.Count - 1
                chkcmbItemCtr.Items.Add(dtCounter.Rows(i)(0).ToString.Replace(",", ",,"), dtCounter.Rows(i)(0).ToString = "ALL")
            Next
        Else
            chkcmbItemCtr.Enabled = False
        End If
        chkcmbItemCtr.Text = "ALL"
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            Prop_Gets()
            txtTagNo.Clear()
            dtpFrom.Select()
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            lblTitle.Text = "TITLE"
            dtSalesStock.Clear()
            gridView.DataSource = Nothing
            cmbGroupBy.Items.Clear()
            cmbGroupBy.Items.Add("NONE")
            cmbGroupBy.Items.Add("ITEMCOUNTER")
            cmbGroupBy.Text = "NONE"
            LoadItemCounter()
            'chkDifference.Checked = True
            'txtDifference.Enabled = True
            'chkMinValue.Checked = False
            'chkHomeSalesView.Checked = False
            'rdbBillNoWise.Checked = True
            'rdbItemWise.Checked = False
            'cmbCostCentre.Text = "ALL"
            'cmbMetalName.Text = "ALL"
            'txtDifference.Text = 0
            Me.Refresh()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
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

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T1"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "T2"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "S1"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle2.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridStockSaleValueCheck_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
        ElseIf UCase(e.KeyCode) = Keys.X And tabMain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyCode) = Keys.P And tabMain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Function funcAddComboData() As Integer
        Try
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            strSql = "select COSTNAME from " & cnAdminDb & "..COSTCENTRE Order By COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False

            cmbMetalName.Items.Clear()
            cmbMetalName.Items.Add("ALL")
            strSql = "select METALNAME from " & cnAdminDb & "..METALMAST order by METALNAME"
            objGPack.FillCombo(strSql, cmbMetalName, False, False)
            cmbMetalName.Text = "ALL"
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        Try
            With gridView
                With .Columns("PARTICULAR")
                    .HeaderText = "METALNAME"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    If rdbBillNoWise.Checked = True Then
                        .Width = 100
                        .HeaderText = "BILLNO"
                    ElseIf rdbItemWise.Checked = True Then
                        .Width = 200
                        .HeaderText = "METALNAME"
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("METALNAME")
                    .Visible = False
                End With
                With .Columns("BILLNO")
                    If rdbBillNoWise.Checked = True Then
                        .Visible = False
                    Else
                        .Visible = True
                    End If
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("ITEMNAME")
                    If rdbItemWise.Checked = True Then
                        .Visible = False
                    Else
                        .Visible = True
                    End If
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("TAGNO")
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("MAXVALUE")
                    .Width = 80
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    If chkMinValue.Checked = True Then
                        .HeaderText = "MAX VALUE"
                    Else
                        .HeaderText = "TAG VALUE"
                    End If
                End With
                With .Columns("MINVALUE")
                    If chkMinValue.Checked = True Then
                        .Visible = True
                        .Width = 100
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .HeaderText = "MIN VALUE"
                    Else
                        .Visible = False
                    End If
                End With
                With .Columns("SALEVALUE")
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "SALE VALUE"
                End With
                With .Columns("DIFF")
                    .Width = 80
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("DIFFPER")
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "DIFF %"
                End With
                With .Columns("DISCOUNT")
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("DISCOUNTDIFF")
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "DISC DIFF"
                End With
                With .Columns("SALESPERSON")
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .HeaderText = "SALES PERSON"
                End With
                With .Columns("COSTID")
                    .Visible = False
                End With
                With .Columns("COSTNAME")
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .HeaderText = "COSTNAME"
                End With
                With .Columns("ITEMCTRNAME")
                    .Width = 200
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .HeaderText = "ITEMCTRNAME"
                End With
                With .Columns("ITEMCTRID")
                    .Visible = False
                End With
                With .Columns("GRSWT")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "GRS WT"
                End With
                With .Columns("NETWT")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "NET WT"
                End With
                With .Columns("TAGMINWASTAGE")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "TAG MIN WAST"
                    .Visible = False
                End With
                With .Columns("TAGMAXWASTAGE")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "TAG WAST"
                End With
                With .Columns("SALEWASTAGE")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "SALE WAST"
                End With
                With .Columns("TAGMINMC")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "TAG MIN MC"
                    .Visible = False
                End With
                With .Columns("TAGMAXMC")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "TAG MC"
                End With
                With .Columns("SALEMC")
                    .Width = 100
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "SALE MC"
                End With
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub chkDifference_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDifference.CheckedChanged
        If chkDifference.Checked = True Then
            txtDifference.Enabled = True
        Else
            txtDifference.Enabled = False
            txtDifference.Text = 0
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New FrmStockSaleValueCheckRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(FrmStockSaleValueCheckRpt_Properties))
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        cmbMetalName.Text = obj.p_cmbMetalName
        chkDifference.Checked = obj.p_chkDifference
        txtDifference.Text = obj.p_txtDifference
        chkMinValue.Checked = obj.p_chkMinValue
        chkHomeSalesView.Checked = obj.p_chkHomeSalesView
        rdbBillNoWise.Checked = obj.p_rdbBillNoWise
        rdbItemWise.Checked = obj.p_rdbItemWise
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New FrmStockSaleValueCheckRpt_Properties
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_cmbMetalName = cmbMetalName.Text
        obj.p_chkDifference = chkDifference.Checked
        obj.p_txtDifference = txtDifference.Text
        obj.p_chkMinValue = chkMinValue.Checked
        obj.p_chkHomeSalesView = chkHomeSalesView.Checked
        obj.p_rdbBillNoWise = rdbBillNoWise.Checked
        obj.p_rdbItemWise = rdbItemWise.Checked
        SetSettingsObj(obj, Me.Name, GetType(FrmStockSaleValueCheckRpt_Properties))
    End Sub
End Class

Public Class FrmStockSaleValueCheckRpt_Properties
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
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
    Private chkDifference As Boolean = True
    Public Property p_chkDifference() As Boolean
        Get
            Return chkDifference
        End Get
        Set(ByVal value As Boolean)
            chkDifference = value
        End Set
    End Property

    Private txtDifference As String = "0"
    Public Property p_txtDifference() As String
        Get
            Return txtDifference
        End Get
        Set(ByVal value As String)
            txtDifference = value
        End Set
    End Property
    Private chkMinValue As Boolean = True
    Public Property p_chkMinValue() As Boolean
        Get
            Return chkMinValue
        End Get
        Set(ByVal value As Boolean)
            chkMinValue = value
        End Set
    End Property

    Private chkHomeSalesView As Boolean = True
    Public Property p_chkHomeSalesView() As Boolean
        Get
            Return chkHomeSalesView
        End Get
        Set(ByVal value As Boolean)
            chkHomeSalesView = value
        End Set
    End Property
    Private rdbBillNoWise As Boolean = True
    Public Property p_rdbBillNoWise() As Boolean
        Get
            Return rdbBillNoWise
        End Get
        Set(ByVal value As Boolean)
            rdbBillNoWise = value
        End Set
    End Property
    Private rdbItemWise As Boolean = True
    Public Property p_rdbItemWise() As Boolean
        Get
            Return rdbItemWise
        End Get
        Set(ByVal value As Boolean)
            rdbItemWise = value
        End Set
    End Property
End Class