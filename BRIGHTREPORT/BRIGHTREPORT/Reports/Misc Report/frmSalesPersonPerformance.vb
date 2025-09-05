Imports System.Data.OleDb
Public Class frmSalesPersonPerformance
    Dim objGridShower As frmGridDispDia
    Dim dtSalesPerson As New DataTable
    Dim dsSalesPerson As New DataSet
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
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

        strSql = " SELECT 'ALL' CASHNAME,'ALL' CASHID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT CASHNAME,CONVERT(VARCHAR,CASHID),2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER WHERE  ISNULL(ACTIVE,'N')='Y' "
        strSql += " ORDER BY RESULT,CASHNAME"
        dtCashCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCashCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCashCounter, dtCashCounter, "CASHNAME", , "ALL")

        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        funcAddSalesPerson()
        funcNew()
        dtpFrom.Select()
        btnNew_Click(Me, New EventArgs)
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
            If GetAdmindbSoftValue("RPT_SALESPERFORMANCE_AUTH", "") = "Y" Then
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, "frmSalesPersonPerformance", BrighttechPack.Methods.RightMode.View, False) Then
                    Dim objSecret As New frmAdminPassword()
                    If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                End If
            End If
            Dim selSalesPerson As String = Nothing
            Dim selCompany As String = Nothing
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
                    dtpFrom.Focus()
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
            Dim chkitemid As String = GetChecked_CheckedList(chkLstItem)



            If chkItemAll.Checked Then
                chkitemid = "ALL"
            Else
                Dim tsTr As String = ""
                For CNT As Integer = 0 To chkLstItem.CheckedItems.Count - 1
                    If chkLstItem.CheckedItems.Item(CNT).ToString = "ALL" Then
                        chkitemid = "ALL"
                        Exit For
                    End If
                    tsTr += objGPack.GetSqlValue("SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & chkLstItem.CheckedItems.Item(CNT).ToString & "'") + ","
                Next
                If tsTr <> "" Then
                    chkitemid = Mid(tsTr, 1, tsTr.Length - 1)
                End If
            End If



            If ChkDateWise.Checked Or chkMonthWise.Checked Then
                strSql = " EXEC " & cnStockDb & "..RPT_SALESPERSONPERFORMANCEDATEWISE"
            Else
                strSql = " EXEC " & cnStockDb & "..SP_RPT_SALESPERSONPERFORMANCENEW"
            End If
            strSql += vbCrLf + " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@DATETO= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            If rbtCounter.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'C'"
            ElseIf rbtMetal.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'M'"
            ElseIf rbtCategory.Checked Then
                strSql += vbCrLf + ",@GROUPBY = 'G'"
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
            If chkitemid = "" Then
                strSql += vbCrLf + ",@ITEMNAME = ''"
            Else
                If chkItemAll.Checked Then
                    strSql += vbCrLf + ",@ITEMNAME = ''"
                Else
                    strSql += vbCrLf + ",@ITEMNAME = '" & chkitemid & "'"
                End If
            End If
            strSql += vbCrLf + ",@WITHADV = '" + IIf(ChkWithAdv.Checked = True, "Y", "N") + "'"
            If Not (ChkDateWise.Checked Or chkMonthWise.Checked) Then
                strSql += vbCrLf + ",@WITHCHIT = '" + IIf(chkWithSavings.Checked = True, "Y", "N") + "'"
            End If
            If Val(txtTagNoDay_Num.Text) > 0 Then
                strSql += vbCrLf + ",@TAGDAY = '" & Val(txtTagNoDay_Num.Text) & "'"
            Else
                strSql += vbCrLf + ",@TAGDAY = ''"
            End If
            strSql += vbCrLf + ",@CASHCOUNTER='" & IIf(chkcmbCashCounter.Text <> "", GetQryString(chkcmbCashCounter.Text).Replace("'", ""), "ALL") & "'"
            If Not (ChkDateWise.Checked Or chkMonthWise.Checked) Then
                strSql += vbCrLf + ",@WITHCHITINC = '" + IIf(chkChitIncentive.Checked = True, "Y", "N") + "'"
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
            gridViewHead.Visible = True
            funcGridSalesPersonStyle()
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("GROUPCOL").Visible = False
            If withdate.Checked = True Then
                gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT").Visible = True
                gridView.Columns("OPENPCS").Visible = True
                gridView.Columns("OPENWEIGHT").Visible = True
                gridView.Columns("OPENSTNWT").Visible = True
                gridView.Columns("OPENDIAWT").Visible = True
                gridView.Columns("OPENAMOUNT").Visible = True
            Else
                gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT").Visible = False
                gridView.Columns("OPENPCS").Visible = False
                gridView.Columns("OPENWEIGHT").Visible = False
                gridView.Columns("OPENSTNWT").Visible = False
                gridView.Columns("OPENDIAWT").Visible = False
                gridView.Columns("OPENAMOUNT").Visible = False
            End If
            gridViewHead.Columns("SAVINGSPCS~SAVINGSWEIGHT~SAVINGSSTNWT~SAVINGSDIAWT~SAVINGSAMOUNT").Visible = chkWithSavings.Checked
            gridView.Columns("SAVINGSPCS").Visible = chkWithSavings.Checked
            gridView.Columns("SAVINGSWEIGHT").Visible = chkWithSavings.Checked
            gridView.Columns("SAVINGSSTNWT").Visible = chkWithSavings.Checked
            gridView.Columns("SAVINGSDIAWT").Visible = chkWithSavings.Checked
            gridView.Columns("SAVINGSAMOUNT").Visible = chkWithSavings.Checked
            If chkChitIncentive.Checked And rbtNone.Checked Then
                gridViewHead.Columns("CHIT_INC").Visible = chkChitIncentive.Checked
                gridView.Columns("CHIT_INC").Visible = chkChitIncentive.Checked
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Function funcGridHeaderNew() As Integer
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("SALESPERSON", GetType(String))
                .Columns.Add("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT", GetType(String))
                .Columns.Add("SALEPCS~SALEWEIGHT~SALESTNWT~SALEDIAWT~SALEAMOUNT", GetType(String))
                .Columns.Add("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNDIAWT~RETURNAMOUNT", GetType(String))
                .Columns.Add("SAVINGSPCS~SAVINGSWEIGHT~SAVINGSSTNWT~SAVINGSDIAWT~SAVINGSAMOUNT", GetType(String))
                .Columns.Add("PCS~WEIGHT~STNWT~DIAWT~AMOUNT", GetType(String))
                If chkChitIncentive.Checked And rbtNone.Checked Then .Columns.Add("CHIT_INC", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("SALESPERSON").Caption = ""
                .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT").Caption = "OPENING"
                .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEDIAWT~SALEAMOUNT").Caption = IIf(ChkOrderonly.Checked = True, "ORDER", "SALES")
                .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNDIAWT~RETURNAMOUNT").Caption = "RETURN"
                .Columns("SAVINGSPCS~SAVINGSWEIGHT~SAVINGSSTNWT~SAVINGSDIAWT~SAVINGSAMOUNT").Caption = "SAVINGS"
                .Columns("PCS~WEIGHT~STNWT~DIAWT~AMOUNT").Caption = "DIFFERENCE"
                If chkChitIncentive.Checked And rbtNone.Checked Then .Columns("CHIT_INC").Caption = "CHIT"
                .Columns("Scroll").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Private Sub frmSalesPersonPerformance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridSalesPersonPerform_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If ChkDateWise.Checked Then Exit Sub
        If gridViewHead.Columns.Count > 0 Then
            gridViewHead.Columns("SALESPERSON").Width = gridView.Columns("SALESPERSON").Width
            gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT").Width = gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENDIAWT").Width + gridView.Columns("OPENAMOUNT").Width
            gridViewHead.Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEDIAWT~SALEAMOUNT").Width = gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEDIAWT").Width + gridView.Columns("SALEAMOUNT").Width
            gridViewHead.Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNDIAWT~RETURNAMOUNT").Width = gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNDIAWT").Width + gridView.Columns("RETURNAMOUNT").Width
            gridViewHead.Columns("SAVINGSPCS~SAVINGSWEIGHT~SAVINGSSTNWT~SAVINGSDIAWT~SAVINGSAMOUNT").Width = gridView.Columns("SAVINGSWEIGHT").Width + gridView.Columns("SAVINGSSTNWT").Width + gridView.Columns("SAVINGSDIAWT").Width + gridView.Columns("SAVINGSAMOUNT").Width
            gridViewHead.Columns("PCS~WEIGHT~STNWT~DIAWT~AMOUNT").Width = gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("DIAWT").Width + gridView.Columns("AMOUNT").Width
            If chkChitIncentive.Checked And rbtNone.Checked Then gridViewHead.Columns("CHIT_INC").Width = gridView.Columns("CHIT_INC").Width
            gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End If
    End Sub

    Private Sub gridSalesPersonPerform_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
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

            With .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENDIAWT~OPENAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENDIAWT").Width + gridView.Columns("OPENAMOUNT").Width
                .HeaderText = "ACTUAL SALES"
            End With
            With .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEDIAWT~SALEAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEDIAWT").Width + gridView.Columns("SALEAMOUNT").Width
                .HeaderText = "SALES"
            End With
            With .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNDIAWT~RETURNAMOUNT")
                .HeaderText = "RETURN"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNDIAWT").Width + gridView.Columns("RETURNAMOUNT").Width
            End With
            With .Columns("SAVINGSPCS~SAVINGSWEIGHT~SAVINGSSTNWT~SAVINGSDIAWT~SAVINGSAMOUNT")
                .HeaderText = "SAVINGS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SAVINGSPCS").Width + gridView.Columns("SAVINGSWEIGHT").Width + gridView.Columns("SAVINGSSTNWT").Width + gridView.Columns("SAVINGSDIAWT").Width + gridView.Columns("SAVINGSAMOUNT").Width
            End With
            With .Columns("PCS~WEIGHT~STNWT~DIAWT~AMOUNT")
                .HeaderText = "ACTUAL SALES "
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("PCS").Width + gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("DIAWT").Width + gridView.Columns("AMOUNT").Width
            End With
            If chkChitIncentive.Checked And rbtNone.Checked Then
                With .Columns("CHIT_INC")
                    .HeaderText = "CHIT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Width = gridView.Columns("CHIT_INC").Width
                    gridView.Columns("CHIT_INC").HeaderText = "INCENTIVE"
                End With
            End If
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
            With .Columns("SAVINGSPCS")
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

            With .Columns("SALEWEIGHT")
                .HeaderText = "WEIGHT"
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
            If .Columns.Contains("SALEDIAWT") Then
                With .Columns("SALEDIAWT")
                    .HeaderText = "STNWT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                End With
            End If
            With .Columns("SALEAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNWEIGHT")
                .HeaderText = "WEIGHT"
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
            If .Columns.Contains("RETURNDIAWT") Then
                With .Columns("RETURNDIAWT")
                    .HeaderText = "STNWT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                End With
            End If
            With .Columns("RETURNAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SAVINGSWEIGHT")
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SAVINGSSTNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            If .Columns.Contains("SAVINGSDIAWT") Then
                With .Columns("SAVINGSDIAWT")
                    .HeaderText = "STNWT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                End With
            End If
            With .Columns("SAVINGSAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("WEIGHT")
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
            If .Columns.Contains("DIAWT") Then
                With .Columns("DIAWT")
                    .HeaderText = "STNWT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 100
                End With
            End If
            With .Columns("AMOUNT")
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
        funcGridHeaderNew()
    End Function

    Function funcNew() As Integer
        strSql = " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strSql += " ORDER BY RESULT,ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem, , chkItemAll.Checked)

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtNone.Checked = True
        If cmbSalesPerson.Items.Count > 0 Then
            cmbSalesPerson.Text = "ALL"
        End If
        chkcmbCashCounter.Text = "ALL"
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        txtTagNoDay_Num.Text = ""
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

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        End If
    End Sub

    Private Sub gridSalesPersonPerform_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
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
        obj.p_rbtCategory = rbtCategory.Checked
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
        rbtCategory.Checked = obj.p_rbtCategory
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

    Private Sub chkMonthWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtTagNoDay_Num.Text = ""
        txtTagNoDay_Num.ReadOnly = chkMonthWise.Checked
        If chkMonthWise.Checked Then
            ChkDateWise.Checked = False
        End If
    End Sub

    Private Sub ChkDateWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtTagNoDay_Num.Text = ""
        txtTagNoDay_Num.ReadOnly = ChkDateWise.Checked
        If ChkDateWise.Checked Then
            chkMonthWise.Checked = False
        End If
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemAll.Checked)
    End Sub

    Private Sub ChkDateWise_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkMonthWise.CheckedChanged, ChkDateWise.CheckedChanged
        chkWithSavings.Visible = Not (chkMonthWise.Checked Or ChkDateWise.Checked) : chkWithSavings.Checked = False
        rbtCounter.Enabled = Not (chkMonthWise.Checked Or ChkDateWise.Checked) : rbtCounter.Checked = False
        rbtMetal.Enabled = Not (chkMonthWise.Checked Or ChkDateWise.Checked) : rbtMetal.Checked = False
        rbtCategory.Enabled = Not (chkMonthWise.Checked Or ChkDateWise.Checked) : rbtCategory.Checked = False
        rbtNone.Checked = True
    End Sub
End Class


Public Class frmSalesPersonPerformance_Properties
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
