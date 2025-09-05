Imports System.Data.OleDb
Public Class frmSalesPersonPerformanceNew

    Dim objGridShower As frmGridDispDia
    Dim dtSalesPerson As New DataTable
    Dim dsSalesPerson As New DataSet
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim flagORMAST As Boolean = IIf(GetAdmindbSoftValue("RPT_P_ORMAST", "Y") = "Y", True, False)
    Private Sub frmSalesPersonPerformance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        'LOAD COMPANY
        strSql = vbCrLf + " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCompany As New DataTable()
        da.Fill(dtCompany)
        cmbCompany.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        funcAddSalesPerson()
        funcAddItemcounter()
        funcNew()
        dtpFrom.Select()
        btnNew_Click(Me, New EventArgs)
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.Region = New Region(New RectangleF(tabReport.Left, tabReport.Top, tabReport.Width, tabReport.Height))
    End Sub

    Function funcwithdate()
        If withdate.Checked = True Then
            btnView_Search.Enabled = False
            dtSalesPerson.Clear()
            dsSalesPerson.Clear()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Me.Refresh()
            Dim day As Integer = dtpFrom.Value.Day
            Dim date1 As DateTime

            Try
                If (Val(txtwithdate.Text.ToString) > day) Then
                    date1 = New Date(dtpFrom.Value.Date.AddMonths(-1).Year, dtpFrom.Value.AddMonths(-1).Date.Month, Val(txtwithdate.Text.ToString))
                Else
                    date1 = New Date(dtpFrom.Value.Date.Year, dtpFrom.Value.Date.Month, Val(txtwithdate.Text.ToString))
                End If

                strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESPERSONPERFORMANCENEW"
                strSql += " @DATEFROM ='" & DateTime.Parse(date1.ToString("yyyy-MM-dd")) & "'"
                strSql += ",@DATETO= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
                If rbtCounter.Checked Then
                    strSql += ",@GROUPBY = 'C'"
                ElseIf rbtMetal.Checked Then
                    strSql += ",@GROUPBY = 'M'"
                Else
                    strSql += ",@GROUPBY = ''"
                End If
                strSql += ",@METALNAME = '" & cmbMetal.Text & "'"
                strSql += ",@EMPNAME='" & Replace(cmbSalesPerson.Text, "'", "''''") & "'"
                strSql += ",@CNCOMPANYID='" & strCompanyId & "'"
                strSql += ",@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                dtSalesPerson = New DataTable
                dtSalesPerson.Columns.Add("KEYNO", GetType(Integer))
                dtSalesPerson.Columns("KEYNO").AutoIncrement = True
                dtSalesPerson.Columns("KEYNO").AutoIncrementSeed = 0
                dtSalesPerson.Columns("KEYNO").AutoIncrementStep = 1
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtSalesPerson)
                If dtSalesPerson.Rows.Count < 1 Then
                    btnView_Search.Enabled = True
                    MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                    Exit Function
                End If
                dtSalesPerson.Columns("KEYNO").SetOrdinal(dtSalesPerson.Columns.Count - 1)
                gridView.DataSource = dtSalesPerson
                FillGridGroupStyle_KeyNoWise(gridView, "SALESPERSON")
                funcGridSalesPersonStyle()
                gridView.Columns("RESULT").Visible = False
                gridView.Columns("COLHEAD").Visible = False
                gridView.Columns("SNO").Visible = False
                gridView.Columns("GROUPCOL").Visible = False

                Dim strTitle As String = Nothing
                strTitle = "SALESPERSON PERFORMANCE REPORT FROM '" & date1 & "' TO '" & dtpFrom.Value.Date.ToString("yyyy/MM/dd") & "'"
                If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
                    strTitle += " FOR " & cmbSalesPerson.Text & ""
                End If
                lblTitle.Text = strTitle

                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                With gridViewHead
                    If colWid >= gridView.Width Then
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    Else
                        .Columns("SCROLL").Visible = False
                    End If
                End With
                '   FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "Sno")
                gridView.Focus()
            Catch ex As Exception
                btnView_Search.Enabled = True
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End Try
            Prop_Sets()
            btnView_Search.Enabled = True
            Exit Function
        Else
            Exit Function
        End If
    End Function
    Private Function GetSqlValue(ByVal qry As String, Optional ByVal defValue As String = Nothing, Optional ByVal ttran As OleDbTransaction = Nothing) As String
        cmd = New OleDbCommand(qry, cn)
        If Not ttran Is Nothing Then cmd.Transaction = ttran
        da = New OleDbDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then Return dt.Rows(0).Item(0).ToString()
        Return defValue
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            If rbtGRSWT.Checked = False And rbtNetWT.Checked = False And rbtBothWT.Checked = False Then
                rbtNetWT.Checked = True
            End If
            Dim selSalesPerson As String = Nothing
            Dim selCompany As String = Nothing
            Dim selItemCtrId As String = "0"
            Dim selCreditView As String = ""
            ResizeToolStripMenuItem.Checked = False
            dtSalesPerson.Clear()
            dsSalesPerson.Clear()
            lblTitle.Text = " "
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Me.Refresh()
            If ChkDateWise.Checked Then
                strSql = "SELECT @@MICROSOFTVERSION/POWER(2,24)"
                Dim Version As Integer = GetSqlValue(strSql, 8)
                If Version <= 8 Then
                    MsgBox("Report Not Available for this Version", MsgBoxStyle.Information)
                    btnNew.Focus()
                    Exit Sub
                End If
                If dtpFrom.Value.Month <> dtpTo.Value.Month Then
                    MsgBox("Report Available for One Month Only,Check From & To Date", MsgBoxStyle.Information)
                    btnNew.Focus()
                    Exit Sub
                End If
            End If
            btnView_Search.Enabled = False
            Dim day As Integer = dtpFrom.Value.Day
            Dim date1 As DateTime
            If withdate.Checked = True Then
                If (Val(txtwithdate.Text.ToString) >= day) Then
                    If dtpFrom.Value.Date.AddDays(-1 * dtpFrom.Value.Date.Day).Day < Val(txtwithdate.Text.ToString) Then
                        date1 = dtpFrom.Value.Date.AddDays(-1 * dtpFrom.Value.Date.Day)
                    Else
                        date1 = New Date(dtpFrom.Value.Date.AddMonths(-1).Year, dtpFrom.Value.AddMonths(-1).Date.Month, Val(txtwithdate.Text.ToString))
                    End If
                Else
                    date1 = New Date(dtpFrom.Value.Date.Year, dtpFrom.Value.Date.Month, Val(txtwithdate.Text.ToString))
                End If
            End If
            If cmbSalesPerson.Text = "ALL" Then
                selSalesPerson = "ALL"
            ElseIf cmbSalesPerson.Text <> "" Then
                Dim sql As String = "SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN (" & GetQryString(cmbSalesPerson.Text) & ")"
                Dim dtItem As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtItem)
                If dtItem.Rows.Count > 0 Then
                    'selItemId = "'"
                    For i As Integer = 0 To dtItem.Rows.Count - 1
                        selSalesPerson += dtItem.Rows(i).Item("EMPNAME").ToString + ","
                    Next
                    If selSalesPerson <> "" Then
                        selSalesPerson = Mid(selSalesPerson, 1, selSalesPerson.Length - 1)
                    End If
                    'selItemId += "'"
                End If
            End If
            If cmbCompany.Text = "ALL" Then
                'selCompany = "ALL"
                Dim sql As String = "SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
                Dim dtCompany As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCompany)
                If dtCompany.Rows.Count > 0 Then
                    'selItemId = "'"
                    For i As Integer = 0 To dtCompany.Rows.Count - 1
                        selCompany += dtCompany.Rows(i).Item("COMPANYNAME").ToString + ","
                    Next
                    If selCompany <> "" Then
                        selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                    End If
                    'selItemId += "'"
                End If
            ElseIf cmbCompany.Text <> "" Then
                Dim sql As String = "SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(cmbCompany.Text) & ")"
                Dim dtCompany As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCompany)
                If dtCompany.Rows.Count > 0 Then
                    'selItemId = "'"
                    For i As Integer = 0 To dtCompany.Rows.Count - 1
                        selCompany += dtCompany.Rows(i).Item("COMPANYNAME").ToString + ","
                    Next
                    If selCompany <> "" Then
                        selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                    End If
                    'selItemId += "'"
                End If
            End If

            If cmbItemcounter.Text = "ALL" Then
                selItemCtrId = "0"
            Else
                selItemCtrId = ""
                Dim sql As String = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbItemcounter.Text) & ")"
                Dim dtItemCtrId As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtItemCtrId)
                If dtItemCtrId.Rows.Count > 0 Then
                    For i As Integer = 0 To dtItemCtrId.Rows.Count - 1
                        selItemCtrId += dtItemCtrId.Rows(i).Item("ITEMCTRID").ToString + ","
                    Next
                    If selItemCtrId <> "" Then
                        selItemCtrId = Mid(selItemCtrId, 1, selItemCtrId.Length - 1)
                    End If
                End If
            End If

            If rbtAll.Checked = True Then
                selCreditView = ""
            ElseIf rbtNoCreditView.Checked = True Then
                selCreditView = "NO"
            ElseIf rbtOnlyCredit.Checked = True Then
                selCreditView = "YES"
            End If

            strSql = "IF OBJECT_ID('TEMPTABLEDB..TMP1')IS  NOT NULL DROP TABLE TEMPTABLEDB..TMP1 "
            strSql += "IF OBJECT_ID('TEMPTABLEDB..TMP2')IS  NOT NULL DROP TABLE TEMPTABLEDB..TMP2"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            If ChkDateWise.Checked Or chkMonthWise.Checked Then
                strSql = " EXEC " & cnStockDb & "..RPT_SALESPERSONPERFORMANCEDATEWISE"
            Else
                strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESPERSONPERFORMANCENEW_INC"
            End If
            strSql += vbCrLf + " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@DATETO= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            If rbtCounter.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'C'"
            ElseIf rbtMetal.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'M'"
            ElseIf rdbCategory.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'G'"
            ElseIf rbtTagType.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'T'"
            Else
                strSql += vbCrLf + ",@GROUPBY = ''"
            End If
            strSql += vbCrLf + ",@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + ",@EMPNAME='" & selSalesPerson & "'" '& Replace(cmbSalesPerson.Text, "'", "''''") & "'"
            strSql += vbCrLf + ",@CNCOMPANY='" & selCompany & "'"
            strSql += vbCrLf + ",@COSTNAME = '" & IIf(chkCmbCostCentre.Text <> "", GetQryString(chkCmbCostCentre.Text).Replace("'", ""), "ALL") & "'"
            If withdate.Checked = True Then
                strSql += vbCrLf + ",@WITHOPENING = 'Y'"
                strSql += vbCrLf + ",@OPENINGFDATE = '" & date1.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + ",@OPENINGTDATE = '" & dtpFrom.Value.AddDays(-1).Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + ",@WITHOPENING = 'N'"
                strSql += vbCrLf + ",@OPENINGFDATE = ''"
                strSql += vbCrLf + ",@OPENINGTDATE = ''"
            End If
            strSql += vbCrLf + ",@GRSNET = '" + IIf(rbtGRSWT.Checked = True, "G", "N") + "'"
            strSql += vbCrLf + ",@ORDERONLY = '" + IIf(ChkOrderonly.Checked = True, "Y", "N") + "'"
            strSql += vbCrLf + ",@ORMAST = '" + IIf(flagORMAST, "Y", "N") + "'"
            strSql += vbCrLf + ",@CNADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + ",@CNSTOCKDB='" & cnStockDb & "'"
            If ChkDateWise.Checked Then
                strSql += ",@SYSTEMID='" & systemId & "'"
                strSql += ",@MONTHWISE='N'"
            ElseIf chkMonthWise.Checked Then
                strSql += ",@SYSTEMID=''"
                strSql += ",@MONTHWISE='Y'"
            End If
            strSql += vbCrLf + ",@WITHORDER = '" + IIf(ChkWithOrder.Checked = True, "Y", "N") + "'"
            strSql += vbCrLf + ",@WITHREPAIR = '" + IIf(ChkWithRepair.Checked = True, "Y", "N") + "'"
            If ChkDateWise.Checked Or chkMonthWise.Checked Then
            Else
                strSql += vbCrLf + ",@ITEMCTRID = '" + selItemCtrId + "'"
                strSql += vbCrLf + ",@CREDITVIEW = '" + selCreditView + "'"
            End If
            dtSalesPerson = New DataTable
            dtSalesPerson.Columns.Add("KEYNO", GetType(Integer))
            dtSalesPerson.Columns("KEYNO").AutoIncrement = True
            dtSalesPerson.Columns("KEYNO").AutoIncrementSeed = 0
            dtSalesPerson.Columns("KEYNO").AutoIncrementStep = 1
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtSalesPerson)
            If dtSalesPerson.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records Not Found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If
            dtSalesPerson.Columns("KEYNO").SetOrdinal(dtSalesPerson.Columns.Count - 1)
            gridView.DataSource = dtSalesPerson
            FillGridGroupStyle_KeyNoWise(gridView, "SALESPERSON")
            If ChkDateWise.Checked Then funcDateWise() : Exit Sub
            If chkMonthWise.Checked Then funcMonthWise() : Exit Sub
            funcGridSalesPersonStyle()
            If gridView.Columns.Contains("RESULT") Then
                gridView.Columns("RESULT").Visible = False
            End If

            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("GROUPCOL").Visible = False
            If gridView.Columns.Contains("COUNTERID") Then
                gridView.Columns("COUNTERID").Visible = False
            End If
            If withdate.Checked = True Then
                If rbtGRSWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENGRSWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = True
                ElseIf rbtNetWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = True
                ElseIf rbtBothWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENGRSWEIGHT~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = True
                End If
                gridView.Columns("OPENPCS").Visible = True
                gridView.Columns("OPENWEIGHT").Visible = True
                gridView.Columns("OPENSTNWT").Visible = True
                gridView.Columns("OPENAMOUNT").Visible = True
            Else
                If rbtGRSWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENGRSWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = False
                ElseIf rbtNetWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = False
                ElseIf rbtBothWT.Checked = True Then
                    gridViewHead.Columns("OPENPCS~OPENGRSWEIGHT~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = False
                End If
                gridView.Columns("OPENPCS").Visible = False
                gridView.Columns("OPENGRSWEIGHT").Visible = False
                gridView.Columns("OPENNETWEIGHT").Visible = False
                gridView.Columns("OPENSTNWT").Visible = False
                gridView.Columns("OPENAMOUNT").Visible = False
            End If
            Dim strTitle As String = Nothing
            strTitle = "SALESPERSON PERFORMANCE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            strTitle += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
            If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
                strTitle += " FOR " & cmbSalesPerson.Text & ""
            End If
            lblTitle.Text = strTitle

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            With gridViewHead
                If colWid >= gridView.Width Then
                    .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                    .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                Else
                    .Columns("SCROLL").Visible = False
                End If
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            funcGridHeaderStyle()
            gridView.Focus()
            BtnBack_Click(Me, New EventArgs)
        Catch ex As Exception
            btnView_Search.Enabled = True
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Prop_Sets()
        btnView_Search.Enabled = True
    End Sub
    Private Sub funcMonthWise()
        Dim strTitle As String = Nothing
        strTitle = "SALESPERSON PERFORMANCE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        strTitle += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
            strTitle += " FOR " & cmbSalesPerson.Text & ""
        End If
        lblTitle.Text = strTitle
        With gridView
            .Columns("GROUPCOL").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(gridView, False, False, False, False)
            For Each gv As DataGridViewRow In .Rows
                With gv
                    Select Case Val(.Cells("RESULT").Value.ToString)
                        Case 0
                            .Cells("SALESPERSON").Style = reportHeadStyle
                        Case 2
                            .DefaultCellStyle = reportSubTotalStyle
                        Case 3
                            .DefaultCellStyle = reportTotalStyle
                    End Select
                End With
            Next
        End With
        For Each gc As DataGridViewColumn In gridView.Columns
            If Val(gc.HeaderText) > 0 Then
                Dim strDate As DateTime
                strDate = New Date(2005, Mid(gc.HeaderText, 5, 2), 1)
                gc.HeaderText = strDate.ToString("MMM") & " - " & Mid(gc.HeaderText, 1, 4)
            End If
        Next

        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        gridViewHead.Visible = False
        gridView.Focus()
        Prop_Sets()
        btnView_Search.Enabled = True
    End Sub
    Private Sub funcDateWise()
        Dim strTitle As String = Nothing
        strTitle = "SALESPERSON PERFORMANCE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        strTitle += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
            strTitle += " FOR " & cmbSalesPerson.Text & ""
        End If
        lblTitle.Text = strTitle
        With gridView
            .Columns("GROUPCOL").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(gridView, False, False, False, False)
            For Each gv As DataGridViewRow In .Rows
                With gv
                    Select Case Val(.Cells("RESULT").Value.ToString)
                        Case 0
                            .Cells("SALESPERSON").Style = reportHeadStyle
                        Case 2
                            .DefaultCellStyle = reportSubTotalStyle
                        Case 3
                            .DefaultCellStyle = reportTotalStyle
                    End Select
                End With
            Next
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        gridViewHead.Visible = False
        gridView.Focus()
        Prop_Sets()
        btnView_Search.Enabled = True
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        Prop_Gets()
        funcNew()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And TabControl1.SelectedIndex = 0 Then
            BrightPosting.GExport.Post(Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        ElseIf grdView_Det.Rows.Count > 0 And TabControl1.SelectedIndex = 1 Then
            BrightPosting.GExport.Post(Name, strCompanyName, lblTitle_Det.Text, grdView_Det, BrightPosting.GExport.GExportType.Export)
        End If
        Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And TabControl1.SelectedIndex = 0 Then
            BrightPosting.GExport.Post(Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        ElseIf grdView_Det.Rows.Count > 0 And TabControl1.SelectedIndex = 1 Then
            BrightPosting.GExport.Post(Name, strCompanyName, lblTitle_Det.Text, grdView_Det, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub BtnExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Close()
    End Sub

    Public Function FuncGridHeaderNew() As Integer
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("SALESPERSON", GetType(String))
                If rbtGRSWT.Checked = True Then
                    .Columns.Add("OPENPCS~OPENGRSWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                    .Columns.Add("SALEPCS~SALEGRSWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                    .Columns.Add("RETURNPCS~RETURNGRSWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                    .Columns.Add("PCS~GRSWEIGHT~STNWT~AMOUNT~INCAMOUNT", GetType(String))
                ElseIf rbtNetWT.Checked = True Then
                    .Columns.Add("OPENPCS~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                    .Columns.Add("SALEPCS~SALENETWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                    .Columns.Add("RETURNPCS~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                    .Columns.Add("PCS~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT", GetType(String))
                ElseIf rbtBothWT.Checked = True Then
                    .Columns.Add("OPENPCS~OPENGRSWEIGHT~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                    .Columns.Add("SALEPCS~SALEGRSWEIGHT~SALENETWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                    .Columns.Add("RETURNPCS~RETURNGRSWEIGHT~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                    .Columns.Add("PCS~GRSWEIGHT~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT", GetType(String))
                End If
                'If rbtCounter.Checked = True Then
                '    .Columns.Add("PCS~WEIGHT~STNWT~AMOUNT~INCAMOUNT", GetType(String))
                'Else
                '    .Columns.Add("PCS~WEIGHT~STNWT~AMOUNT", GetType(String))
                'End If
                .Columns.Add("SCROLL", GetType(String))
                .Columns("SALESPERSON").Caption = ""
                If rbtGRSWT.Checked = True Then
                    .Columns("OPENPCS~OPENGRSWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                    .Columns("SALEPCS~SALEGRSWEIGHT~SALESTNWT~SALEAMOUNT").Caption = IIf(ChkOrderonly.Checked = True, "ORDER", "SALES")
                    .Columns("RETURNPCS~RETURNGRSWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                    .Columns("PCS~GRSWEIGHT~STNWT~AMOUNT~INCAMOUNT").Caption = "DIFFERENCE"
                ElseIf rbtNetWT.Checked = True Then
                    .Columns("OPENPCS~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                    .Columns("SALEPCS~SALENETWEIGHT~SALESTNWT~SALEAMOUNT").Caption = IIf(ChkOrderonly.Checked = True, "ORDER", "SALES")
                    .Columns("RETURNPCS~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                    .Columns("PCS~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT").Caption = "DIFFERENCE"
                ElseIf rbtBothWT.Checked = True Then
                    .Columns("OPENPCS~OPENGRSWEIGHT~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                    .Columns("SALEPCS~SALEGRSWEIGHT~SALENETWEIGHT~SALESTNWT~SALEAMOUNT").Caption = IIf(ChkOrderonly.Checked = True, "ORDER", "SALES")
                    .Columns("RETURNPCS~RETURNGRSWEIGHT~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                    .Columns("PCS~GRSWEIGHT~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT").Caption = "DIFFERENCE"
                End If
                'If rbtCounter.Checked = True Then
                '    .Columns("PCS~WEIGHT~STNWT~AMOUNT~INCAMOUNT").Caption = "DIFFERENCE"
                'Else
                '    .Columns("PCS~WEIGHT~STNWT~AMOUNT").Caption = "DIFFERENCE"
                'End If
                .Columns("Scroll").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Private Sub frmSalesPersonPerformance_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar.ToString.ToUpper = "S" Then
            If TabControl1.SelectedIndex = 0 Then
                If gridView.CurrentRow.Index >= 0 And gridView.CurrentRow.Cells("COLHEAD").Value.ToString.Trim = "" Then
                    If chkHorizontal.Checked = True Then
                        DetailHorizonal(True)
                    Else
                        Detail(True)
                    End If
                End If
            End If
        ElseIf e.KeyChar.ToString.ToUpper = "D" Then
            If TabControl1.SelectedIndex = 0 Then
                If gridView.CurrentRow.Index >= 0 And gridView.CurrentRow.Cells("COLHEAD").Value.ToString.Trim = "" Then
                    If chkHorizontal.Checked = True Then
                        DetailHorizonal(False)
                    Else
                        Detail(False)
                    End If
                End If
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            BtnBack_Click(sender, e)
        End If
    End Sub
    Private Sub DetailHorizonal(ByVal singleUserNameFilter As Boolean)
        If singleUserNameFilter = True Then
            lblTitle_Det.Text = gridView.CurrentRow.Cells("SALESPERSON").Value.ToString
        Else
            lblTitle_Det.Text = ""
        End If

        strSql = "IF OBJECT_ID('TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]','U') "
        strSql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = ""
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " SALESPERSON"
        strSql += vbCrLf + " ,(SELECT TOP 1 ALIASNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = A.SALESPERSON) SALEPERSONALIASNAME"
        If chkWithDate.Checked = True Then
            strSql += vbCrLf + " ,TRANNO"
            strSql += vbCrLf + " ,TRANDATE"
            If rbtOnlyCredit.Checked = True Then
                strSql += vbCrLf + " ,(SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN ( "
                strSql += vbCrLf + " SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANNO=A.TRANNO AND TRANDATE=A.TRANDATE AND TRANTYPE='SA' "
                strSql += vbCrLf + " GROUP BY BATCHNO))) PNAME"
            End If
        End If
        strSql += vbCrLf + " ,ITEMID "
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.ITEMID)ITEMNAME"
        strSql += vbCrLf + " ,SUM(SALEPCS) SAPCS"
        strSql += vbCrLf + " ,SUM(SALEGRSWEIGHT) SAGRSWT"
        strSql += vbCrLf + " ,SUM(SALENETWEIGHT) SANETWT"
        strSql += vbCrLf + " ,SUM(SALESTNWT) SASTNWT"
        strSql += vbCrLf + " ,MAX(SALESRATE) SARATE"
        strSql += vbCrLf + " ,SUM(SALESWASTPER) SAWASTPER"
        strSql += vbCrLf + " ,SUM(SALESWASTAGE) SAWASTAGE"
        strSql += vbCrLf + " ,SUM(SALESMCHARGE) SAMCHARGE"
        strSql += vbCrLf + " ,SUM(SALEAMOUNT) SAAMOUNT"
        strSql += vbCrLf + " ,SUM(RETURNPCS) SRPCS"
        strSql += vbCrLf + " ,SUM(RETURNGRSWEIGHT) SRGRSWT"
        strSql += vbCrLf + " ,SUM(RETURNNETWEIGHT) SRNETWT"
        strSql += vbCrLf + " ,SUM(RETURNSTNWT) SRSTNWT"
        strSql += vbCrLf + " ,SUM(RETURNAMOUNT) SRAMOUNT"
        strSql += vbCrLf + " ,SUM(PCS) PCS"
        strSql += vbCrLf + " ,SUM(GRSWEIGHT) GRSWT"
        strSql += vbCrLf + " ,SUM(NETWEIGHT) NETWT"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,'D' COLHEAD"
        strSql += vbCrLf + " INTO TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 AS A WHERE RESULT =1"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " AND SALESPERSON='" & lblTitle_Det.Text.Trim & "'"
        End If
        strSql += vbCrLf + " GROUP BY A.SALESPERSON"
        strSql += vbCrLf + " ,A.ITEMID"
        If chkWithDate.Checked = True Then
            strSql += vbCrLf + " ,TRANNO"
            strSql += vbCrLf + " ,TRANDATE"
        End If
        strSql += vbCrLf + " ORDER BY SALESPERSON,ITEMNAME"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        strSql += vbCrLf + "(SALESPERSON,ITEMNAME,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT SALESPERSON,SALESPERSON,0 RESULT,'T' COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        strSql += vbCrLf + " (SALESPERSON "
        strSql += vbCrLf + " ,ITEMNAME,RESULT,COLHEAD "
        strSql += vbCrLf + " ,SAPCS,SAGRSWT,SANETWT,SASTNWT,SARATE,SAWASTPER,SAWASTAGE,SAMCHARGE,SAAMOUNT "
        strSql += vbCrLf + " ,SRPCS,SRGRSWT,SRNETWT,SRSTNWT,SRAMOUNT"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,STNWT,AMOUNT "
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT SALESPERSON,SALESPERSON "
        strSql += vbCrLf + " ,9 RESULT,'G' COLHEAD"
        strSql += vbCrLf + " ,SUM(SAPCS)SAPCS"
        strSql += vbCrLf + " ,SUM(SAGRSWT)SAGRSWT"
        strSql += vbCrLf + " ,SUM(SANETWT)SANETWT"
        strSql += vbCrLf + " ,SUM(SASTNWT)SASTNWT"
        strSql += vbCrLf + " ,MAX(SARATE) SARATE"
        strSql += vbCrLf + " ,SUM(SAWASTPER) SAWASTPER"
        strSql += vbCrLf + " ,SUM(SAWASTAGE) SAWASTAGE"
        strSql += vbCrLf + " ,SUM(SAMCHARGE) SAMCHARGE"
        strSql += vbCrLf + " ,SUM(SAAMOUNT)SAAMOUNT"
        strSql += vbCrLf + " ,SUM(SRPCS)SRPCS"
        strSql += vbCrLf + " ,SUM(SRGRSWT)SRGRSWT"
        strSql += vbCrLf + " ,SUM(SRNETWT)SRNETWT"
        strSql += vbCrLf + " ,SUM(SRSTNWT)SRSTNWT"
        strSql += vbCrLf + " ,SUM(SRAMOUNT)SRAMOUNT"
        strSql += vbCrLf + " ,SUM(PCS)PCS"
        strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
        strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        strSql += vbCrLf + " ,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        strSql += vbCrLf + " GROUP BY SALESPERSON"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..[TEMPSALEPERSONPERFORMANCE" & systemId & "]"
        strSql += vbCrLf + " ORDER BY SALESPERSON,RESULT"
        grdView_Det.DataSource = Nothing
        grdView_Det.DataSource = GetSqlTable(strSql, cn)
        FillGridGroupStyle(grdView_Det)
        With grdView_Det
            With .Columns("SALESPERSON")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Visible = True
            End With
            With .Columns("ITEMID")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("ITEMNAME")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            If .Columns.Contains("TRANNO") Then
                With .Columns("TRANNO")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            End If
            If .Columns.Contains("TRANDATE") Then
                With .Columns("TRANDATE")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            End If
            With .Columns("SAPCS")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SAGRSWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SANETWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SASTNWT")
                .HeaderText = "SADIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SARATE")
                .DefaultCellStyle.Format = "0.00"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SAWASTPER")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SAWASTAGE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SAMCHARGE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SAAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With


            With .Columns("SRPCS")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SRGRSWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SRNETWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SRSTNWT")
                .HeaderText = "SRDIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SRAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With


            With .Columns("PCS")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NETWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            If rbtGRSWT.Checked = True Then
                .Columns("SANETWT").Visible = False
                .Columns("SRNETWT").Visible = False
                .Columns("NETWT").Visible = False
            ElseIf rbtNetWT.Checked = True Then
                .Columns("SAGRSWT").Visible = False
                .Columns("SRGRSWT").Visible = False
                .Columns("GRSWT").Visible = False
            ElseIf rbtBothWT.Checked = True Then
            End If
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Focus()
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            If singleUserNameFilter = True Then
            Else
                If .Columns.Contains("TRANNO") = True Then
                    .Columns("TRANNO").Visible = True
                End If
                If .Columns.Contains("TRANDATE") = True Then
                    .Columns("TRANDATE").Visible = True
                End If
            End If
        End With
        TabControl1.SelectedIndex = 1
    End Sub

    Private Sub Detail(ByVal singleUserNameFilter As Boolean)
        If singleUserNameFilter = True Then
            lblTitle_Det.Text = gridView.CurrentRow.Cells("SALESPERSON").Value.ToString
        Else
            lblTitle_Det.Text = ""
        End If
        strSql = ""
        strSql += vbCrLf + " DECLARE @SALESPERSON AS VARCHAR(100) = '" + gridView.CurrentRow.Cells("SALESPERSON").Value.ToString + "'"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT DISTINCT 'SALES' PARTICULAR "
        strSql += vbCrLf + " ,SALESPERSON"
        strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,NULL) TRANDATE "
        strSql += vbCrLf + " ,NULL TRANNO"
        strSql += vbCrLf + " ,NULL PCS "
        strSql += vbCrLf + " ,NULL GRSWEIGHT"
        strSql += vbCrLf + " ,NULL NETWEIGHT"
        strSql += vbCrLf + " ,NULL STNWT"
        strSql += vbCrLf + " ,NULL AMOUNT"
        strSql += vbCrLf + " ,1 RESULT "
        strSql += vbCrLf + " ,1 RESULT1 "
        strSql += vbCrLf + " ,'T' COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T "
        strSql += vbCrLf + " WHERE 1=1  "
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " AND SALESPERSON Like @SALESPERSON"
        End If
        strSql += vbCrLf + " AND SALEAMOUNT > 0  "
        strSql += vbCrLf + " And ISNULL(TRANNO,0) > 0"
        strSql += vbCrLf + " UNION"
        strSql += vbCrLf + " SELECT (SELECT ITEMNAME FROM " + cnAdminDb + "..ITEMMAST WHERE ITEMID = T.ITEMID)PARTICULAR"
        strSql += vbCrLf + " ,SALESPERSON"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,TRANDATE) TRANDATE "
            strSql += vbCrLf + " ,TRANNO"
        Else
            strSql += vbCrLf + " ,NULL TRANDATE "
            strSql += vbCrLf + " ,0 TRANNO"
        End If
        strSql += vbCrLf + " ,SUM(SALEPCS) SALEPCS"
        strSql += vbCrLf + " ,SUM(SALEGRSWEIGHT) SALEGRSWEIGHT"
        strSql += vbCrLf + " ,SUM(SALENETWEIGHT) SALENETWEIGHT"
        strSql += vbCrLf + " ,SUM(SALESTNWT) SALESTNWT"
        strSql += vbCrLf + " ,SUM(SALEAMOUNT) SALEAMOUNT"
        strSql += vbCrLf + " ,1 RESULT "
        strSql += vbCrLf + " ,2 RESULT1 "
        strSql += vbCrLf + " ,COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T "
        strSql += vbCrLf + " WHERE 1=1 "
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0 "
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " And SALESPERSON Like @SALESPERSON"
        End If
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " GROUP BY T.ITEMID,TRANDATE,TRANNO,SALESPERSON,COLHEAD"
        Else
            strSql += vbCrLf + " GROUP BY T.ITEMID,SALESPERSON,COLHEAD"
        End If
        strSql += vbCrLf + " HAVING SUM(SALEAMOUNT) > 0"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'SALES TOTAL' "
        strSql += vbCrLf + " ,SALESPERSON"
        strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,NULL) "
        strSql += vbCrLf + " ,NULL TRANNO"
        strSql += vbCrLf + " ,SUM(SALEPCS) "
        strSql += vbCrLf + " ,SUM(SALEGRSWEIGHT) "
        strSql += vbCrLf + " ,SUM(SALENETWEIGHT) "
        strSql += vbCrLf + " ,SUM(SALESTNWT) "
        strSql += vbCrLf + " ,SUM(SALEAMOUNT)"
        strSql += vbCrLf + " ,1 RESULT "
        strSql += vbCrLf + " ,99 RESULT1 "
        strSql += vbCrLf + " ,'S'COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T WHERE 1=1 "
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " AND SALESPERSON Like @SALESPERSON"
        End If
        strSql += vbCrLf + " GROUP BY SALESPERSON"
        strSql += vbCrLf + " HAVING SUM(SALEAMOUNT) > 0"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'RETURN' "
        strSql += vbCrLf + " ,SALESPERSON"
        strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,NULL) "
        strSql += vbCrLf + " ,NULL TRANNO"
        strSql += vbCrLf + " ,NULL PCS "
        strSql += vbCrLf + " ,NULL GRSWEIGHT "
        strSql += vbCrLf + " ,NULL NETWEIGHT "
        strSql += vbCrLf + " ,NULL STNWT "
        strSql += vbCrLf + " ,NULL AMOUNT"
        strSql += vbCrLf + " ,2 RESULT "
        strSql += vbCrLf + " ,1 RESULT1 "
        strSql += vbCrLf + "  ,'T' COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T "
        strSql += vbCrLf + " WHERE 1=1"
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " And SALESPERSON Like @SALESPERSON"
        End If
        strSql += vbCrLf + " And RETURNAMOUNT > 0"
        strSql += vbCrLf + " UNION"
        strSql += vbCrLf + " SELECT (SELECT ITEMNAME FROM " + cnAdminDb + "..ITEMMAST WHERE ITEMID = T.ITEMID)PARTICULAR"
        strSql += vbCrLf + " ,SALESPERSON"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,TRANDATE) TRANDATE "
            strSql += vbCrLf + " ,TRANNO"
        Else
            strSql += vbCrLf + " ,NULL TRANDATE "
            strSql += vbCrLf + " ,0 TRANNO"
        End If
        strSql += vbCrLf + " ,SUM(RETURNPCS) RETURNPCS"
        strSql += vbCrLf + " ,SUM(RETURNGRSWEIGHT) RETURNGRSWEIGHT"
        strSql += vbCrLf + " ,SUM(RETURNNETWEIGHT) RETURNNETWEIGHT"
        strSql += vbCrLf + " ,SUM(RETURNSTNWT) RETURNSTNWT"
        strSql += vbCrLf + " ,SUM(RETURNAMOUNT) RETURNAMOUNT"
        strSql += vbCrLf + " ,2 RESULT "
        strSql += vbCrLf + " ,2 RESULT1 "
        strSql += vbCrLf + "  ,COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T "
        strSql += vbCrLf + " WHERE 1=1"
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " And SALESPERSON Like @SALESPERSON"
        End If
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " GROUP BY T.ITEMID,TRANDATE,TRANNO,SALESPERSON,COLHEAD"
        Else
            strSql += vbCrLf + " GROUP BY T.ITEMID,SALESPERSON,COLHEAD"
        End If
        strSql += vbCrLf + " HAVING SUM(RETURNAMOUNT) > 0"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'RETURN TOTAL' "
        strSql += vbCrLf + " ,SALESPERSON"
        strSql += vbCrLf + " ,CONVERT(SMALLDATETIME,NULL) "
        strSql += vbCrLf + " ,NULL TRANNO"
        strSql += vbCrLf + " ,SUM(RETURNPCS) "
        strSql += vbCrLf + " ,SUM(RETURNGRSWEIGHT) "
        strSql += vbCrLf + " ,SUM(RETURNNETWEIGHT) "
        strSql += vbCrLf + " ,SUM(RETURNSTNWT) "
        strSql += vbCrLf + " ,SUM(RETURNAMOUNT)"
        strSql += vbCrLf + " ,2 RESULT "
        strSql += vbCrLf + " ,99 RESULT1 "
        strSql += vbCrLf + "  ,'S'COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T WHERE  1=1"
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " AND SALESPERSON Like @SALESPERSON"
        End If
        strSql += vbCrLf + " GROUP BY SALESPERSON"
        strSql += vbCrLf + " HAVING SUM(RETURNAMOUNT) > 0"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'ZZZZZZ GRAND TOTAL' "
        strSql += vbCrLf + " ,'ZZZZZZ' SALESPERSON"
        strSql += vbCrLf + " ,NULL "
        strSql += vbCrLf + " ,NULL TRANNO"
        strSql += vbCrLf + " ,SUM(PCS) "
        strSql += vbCrLf + " ,SUM(GRSWEIGHT) "
        strSql += vbCrLf + " ,SUM(NETWEIGHT) "
        strSql += vbCrLf + " ,SUM(STNWT) "
        strSql += vbCrLf + " ,SUM(AMOUNT)"
        strSql += vbCrLf + " ,9 RESULT "
        strSql += vbCrLf + " ,99 RESULT1 "
        strSql += vbCrLf + "  ,'G'COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TMP1 T  "
        strSql += vbCrLf + " WHERE 1=1"
        strSql += vbCrLf + " AND ISNULL(TRANNO,0) > 0"
        If singleUserNameFilter = True Then
            strSql += vbCrLf + " And SALESPERSON Like @SALESPERSON"
        End If
        strSql += vbCrLf + " ORDER BY SALESPERSON,RESULT,RESULT1"
        grdView_Det.DataSource = Nothing
        grdView_Det.DataSource = GetSqlTable(strSql, cn)
        FillGridGroupStyle(grdView_Det)
        With grdView_Det
            With .Columns("PARTICULAR")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("TRANNO")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TRANDATE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PCS")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWEIGHT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NETWEIGHT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            If rbtGRSWT.Checked = True Then
                .Columns("NETWEIGHT").Visible = False
            ElseIf rbtNetWT.Checked = True Then
                .Columns("GRSWEIGHT").Visible = False
            ElseIf rbtBothWT.Checked = True Then
            End If
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Columns("RESULT").Visible = False
            .Columns("RESULT1").Visible = False
            .Columns("COLHEAD").Visible = False
            .Focus()
            If singleUserNameFilter = True Then
            Else
                .Columns("TRANNO").Visible = False
                .Columns("TRANDATE").Visible = False
            End If
        End With
        TabControl1.SelectedIndex = 1
    End Sub
    Private Sub gridSalesPersonPerform_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        If ChkDateWise.Checked Then Exit Sub
        If gridViewHead.Columns.Count > 0 Then
            gridViewHead.Columns("SALESPERSON").Width = gridView.Columns("SALESPERSON").Width
            gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Width = gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
            gridViewHead.Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Width = gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
            gridViewHead.Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Width = gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
            gridViewHead.Columns("PCS~WEIGHT~STNWT~AMOUNT~INCAMOUNT").Width = gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("INCAMOUNT").Width

            gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End If
    End Sub

    Private Sub gridSalesPersonPerform_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Function funcGridHeaderStyle() As Integer
        With gridViewHead
            If chkMonthWise.Checked = False Then .Columns("SCROLL").HeaderText = ""
            With .Columns("SALESPERSON")
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALESPERSON").Width
                .HeaderText = " "
            End With
            If rbtGRSWT.Checked = True Then
                With .Columns("OPENPCS~OPENGRSWEIGHT~OPENSTNWT~OPENAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENGRSWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
                    .HeaderText = "OPENING"
                End With
                With .Columns("SALEPCS~SALEGRSWEIGHT~SALESTNWT~SALEAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALEGRSWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
                    .HeaderText = "SALES"
                End With
                With .Columns("RETURNPCS~RETURNGRSWEIGHT~RETURNSTNWT~RETURNAMOUNT")
                    .HeaderText = "RETURN"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNGRSWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
                End With
                With .Columns("PCS~GRSWEIGHT~STNWT~AMOUNT~INCAMOUNT")
                    .HeaderText = "DIFFERENCE"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("PCS").Width + gridView.Columns("GRSWEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("INCAMOUNT").Width
                End With
            ElseIf rbtNetWT.Checked = True Then
                With .Columns("OPENPCS~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENNETWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
                    .HeaderText = "OPENING"
                End With
                With .Columns("SALEPCS~SALENETWEIGHT~SALESTNWT~SALEAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALENETWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
                    .HeaderText = "SALES"
                End With
                With .Columns("RETURNPCS~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT")
                    .HeaderText = "RETURN"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNNETWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
                End With
                With .Columns("PCS~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT")
                    .HeaderText = "DIFFERENCE"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("PCS").Width + gridView.Columns("NETWEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("INCAMOUNT").Width
                End With
            ElseIf rbtBothWT.Checked = True Then
                With .Columns("OPENPCS~OPENGRSWEIGHT~OPENNETWEIGHT~OPENSTNWT~OPENAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENGRSWEIGHT").Width + gridView.Columns("OPENNETWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
                    .HeaderText = "OPENING"
                End With
                With .Columns("SALEPCS~SALEGRSWEIGHT~SALENETWEIGHT~SALESTNWT~SALEAMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALEGRSWEIGHT").Width + gridView.Columns("SALENETWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
                    .HeaderText = "SALES"
                End With
                With .Columns("RETURNPCS~RETURNGRSWEIGHT~RETURNNETWEIGHT~RETURNSTNWT~RETURNAMOUNT")
                    .HeaderText = "RETURN"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNGRSWEIGHT").Width + gridView.Columns("RETURNNETWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
                End With
                With .Columns("PCS~GRSWEIGHT~NETWEIGHT~STNWT~AMOUNT~INCAMOUNT")
                    .HeaderText = "DIFFERENCE"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("PCS").Width + gridView.Columns("GRSWEIGHT").Width + gridView.Columns("NETWEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("INCAMOUNT").Width
                End With
            End If
            'If rbtCounter.Checked = True Then
            '    With .Columns("PCS~WEIGHT~STNWT~AMOUNT~INCAMOUNT")
            '        .HeaderText = "DIFFERENCE"
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '        .Width = gridView.Columns("PCS").Width + gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width + gridView.Columns("INCAMOUNT").Width
            '    End With
            'Else
            '    With .Columns("PCS~WEIGHT~STNWT~AMOUNT")
            '        .HeaderText = "DIFFERENCE"
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '        .Width = gridView.Columns("PCS").Width + gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width
            '    End With
            'End If
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Function funcGridSalesPersonStyle() As Integer
        With gridView
            With .Columns("SALESPERSON")
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = 200
            End With
            With .Columns("PCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNPCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALEPCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALEGRSWEIGHT")
                .HeaderText = "GWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALENETWEIGHT")
                .HeaderText = "NWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALESTNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALEAMOUNT")
                .HeaderText = "AMOUNT."
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNGRSWEIGHT")
                .HeaderText = "GWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNNETWEIGHT")
                .HeaderText = "NWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNSTNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("GRSWEIGHT")
                .HeaderText = "GWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("NETWEIGHT")
                .HeaderText = "NWEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("STNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("AMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("INCAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("GROUPCOL")
                .Visible = False
            End With
            .Columns("KEYNO").Visible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        If rbtGRSWT.Checked = True Then
            gridView.Columns("SALENETWEIGHT").Visible = False
            gridView.Columns("RETURNNETWEIGHT").Visible = False
            gridView.Columns("NETWEIGHT").Visible = False
        ElseIf rbtNetWT.Checked = True Then
            gridView.Columns("SALEGRSWEIGHT").Visible = False
            gridView.Columns("RETURNGRSWEIGHT").Visible = False
            gridView.Columns("GRSWEIGHT").Visible = False
        ElseIf rbtBothWT.Checked = True Then

        End If
        FuncGridHeaderNew()
    End Function

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtNone.Checked = True
        If cmbSalesPerson.Items.Count > 0 Then
            cmbSalesPerson.Text = "ALL"
        End If
        If cmbItemcounter.Items.Count > 0 Then
            cmbItemcounter.Text = "ALL"
        End If
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        dtpFrom.Select()
    End Function

    Function funcAddSalesPerson() As Integer
        Try
            strSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT DISTINCT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
            strSql += " WHERE ACTIVE = 'Y'"
            strSql += " ORDER BY RESULT,EMPNAME"
            Dim dtSales As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSales)
            BrighttechPack.GlobalMethods.FillCombo(cmbSalesPerson, dtSales, "EMPNAME", , "ALL")
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Function funcAddItemcounter() As Integer
        Try
            strSql = " SELECT 'ALL' ITEMCTRNAME,'0' ITEMCTRID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ITEMCTRNAME,ITEMCTRID,2 RESULT "
            strSql += " FROM " & cnAdminDb & "..ITEMCOUNTER"
            strSql += " ORDER BY RESULT,ITEMCTRNAME"
            Dim dtItemCounter As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemCounter)
            BrighttechPack.GlobalMethods.FillCombo(cmbItemcounter, dtItemCounter, "ITEMCTRNAME", , "ALL")
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        End If
    End Sub

    Private Sub gridSalesPersonPerform_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        If ChkDateWise.Checked Then Exit Sub
        If chkMonthWise.Checked Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridViewHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSalesPersonPerformance_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbSalesPerson = cmbSalesPerson.Text
        obj.p_rbtNone = rbtNone.Checked
        obj.p_rbtMetal = rbtMetal.Checked
        obj.p_rbtCounter = rbtCounter.Checked
        obj.p_rbtCategory = rdbCategory.Checked
        obj.p_rbtGRSWT = rbtGRSWT.Checked
        obj.p_rbtNETWT = rbtNetWT.Checked
        obj.p_chkDateWise = ChkDateWise.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSalesPersonPerformance_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesPersonPerformance_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesPersonPerformance_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbSalesPerson.Text = obj.p_cmbSalesPerson
        rbtNone.Checked = obj.p_rbtNone
        rbtMetal.Checked = obj.p_rbtMetal
        rbtCounter.Checked = obj.p_rbtCounter
        rdbCategory.Checked = obj.p_rbtCategory
        rbtGRSWT.Checked = obj.p_rbtGRSWT
        rbtNetWT.Checked = obj.p_rbtNETWT
        ChkDateWise.Checked = obj.p_chkDateWise
    End Sub

    Private Sub withdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles withdate.CheckedChanged
        If withdate.Checked = True Then
            txtwithdate.Visible = True
        Else
            txtwithdate.Visible = False
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
            If ChkDateWise.Checked = False And chkMonthWise.Checked = False Then funcGridHeaderStyle()
        End If
    End Sub

    Private Sub chkMonthWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMonthWise.CheckedChanged
        If chkMonthWise.Checked Then
            ChkDateWise.Checked = False

        End If
    End Sub

    Private Sub ChkDateWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkDateWise.CheckedChanged
        If ChkDateWise.Checked Then
            chkMonthWise.Checked = False

        End If
    End Sub

    Private Sub rbtGRSWT_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtGRSWT.CheckedChanged

    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        grdView_Det.DataSource = Nothing
        lblTitle_Det.Text = ""
        TabControl1.SelectTab(tabReport)
    End Sub

    Private Sub chkHorizontal_CheckedChanged(sender As Object, e As EventArgs) Handles chkHorizontal.CheckedChanged
        If chkHorizontal.Checked = True Then
            chkWithDate.Visible = True
        Else
            chkWithDate.Visible = False
        End If
    End Sub
End Class


Public Class frmSalesPersonPerformanceNew_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbSalesPerson As String = "ALL"
    Public Property p_cmbSalesPerson() As String
        Get
            Return cmbSalesPerson
        End Get
        Set(ByVal value As String)
            cmbSalesPerson = value
        End Set
    End Property
    Private rbtNone As Boolean = True
    Public Property p_rbtNone() As Boolean
        Get
            Return rbtNone
        End Get
        Set(ByVal value As Boolean)
            rbtNone = value
        End Set
    End Property
    Private rbtMetal As Boolean = True
    Public Property p_rbtMetal() As Boolean
        Get
            Return rbtMetal
        End Get
        Set(ByVal value As Boolean)
            rbtMetal = value
        End Set
    End Property
    Private rbtCounter As Boolean = True
    Public Property p_rbtCounter() As Boolean
        Get
            Return rbtCounter
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
        End Set
    End Property
    Private rbtCategory As Boolean = True
    Public Property p_rbtCategory() As Boolean
        Get
            Return rbtCategory
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
        End Set
    End Property
    Private rbtGRSWT As Boolean = True
    Public Property p_rbtGRSWT() As Boolean
        Get
            Return rbtGRSWT
        End Get
        Set(ByVal value As Boolean)
            rbtGRSWT = value
        End Set
    End Property
    Private rbtNETWT As Boolean = True
    Public Property p_rbtNETWT() As Boolean
        Get
            Return rbtNETWT
        End Get
        Set(ByVal value As Boolean)
            rbtNETWT = value
        End Set
    End Property
    Private chkDateWise As Boolean = False
    Public Property p_chkDateWise() As Boolean
        Get
            Return chkDateWise
        End Get
        Set(ByVal value As Boolean)
            chkDateWise = value
        End Set
    End Property
End Class

'=======================================================================================================================================
'QUERY FOR WITHOUT STORED PROCEDURE
'==================================
'strsql = "select"
'strsql += " ISNULL(SALESPERSON,'.') SALESPERSON"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN GRSWT ELSE 0 END),0) SALEWEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN STNWT ELSE 0 END),0) SALESTNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN AMOUNT ELSE 0 END),0) SALEAMOUNT"

'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN GRSWT ELSE 0 END),0) RETURNWEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN STNWT ELSE 0 END),0) RETURNSTNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN AMOUNT ELSE 0 END),0) RETURNAMOUNT"

'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*GRSWT ELSE GRSWT END),0) WEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*STNWT ELSE STNWT END),0) STNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*AMOUNT ELSE AMOUNT END),0) AMOUNT"

'strsql += ",ISNULL(COUNTERNAME,'') COUNTERNAME"
'strsql += " FROM"
'strsql += " ("
'strsql += " select"
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.EMPID) SALESPERSON"
'strsql += ",ISNULL(GRSWT,0) GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
'strsql += " WHERE ISSSNO=I.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0) AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=I.ITEMCTRID) COUNTERNAME"
'strsql += ",'I' SEP"
'strsql += " from " & cnStockDb & "..ISSUE AS I"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbSalesPerson.Text, "'", "''") & "')"
'End If
'strsql += " UNION ALL"
'strsql += " select "
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=R.EMPID) SALESPERSON"
'strsql += ",ISNULL(GRSWT,0)GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE"
'strsql += " WHERE ISSSNO=R.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0)AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=R.ITEMCTRID) COUNTERNAME"
'strsql += ",'R' SEP"
'strsql += " from " & cnStockDb & "..RECEIPT AS R"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbSalesPerson.Text, "'", "''") & "')"
'End If
'strsql += " )X GROUP BY SALESPERSON,COUNTERNAME"
'=======================================================================================================================================
