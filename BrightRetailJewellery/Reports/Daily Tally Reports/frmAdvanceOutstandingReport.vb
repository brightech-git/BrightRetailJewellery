Imports System.Data.OleDb

Public Class frmAdvanceOutstandingReport
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGrid As New DataSet
    Dim dt As DataTable
    Dim SelectedCompanyId As String
    Dim SelectCostcenter As String
    Dim CCENTRE As String = Nothing
    Dim acType As String = Nothing


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        '  ProcAddNodeId()

    End Sub


    Private Sub frmItemWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
            'ElseIf e.KeyChar = Chr(Keys.Escape) And tabmain.SelectedTab.Name = tabView.Name Then
            '    btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) And tabmain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" And tabmain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Public Function GetSelectedAcgrpcode(ByVal chkLst As GiritechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Private Sub NewReport(ByVal Detailed As String, Optional ByVal ACCODE As String = "")
        Dim SelectedCounterId As String = ""
        Dim NodeId As String = ""
        Dim AdvCr As String
        Dim temptable As String

        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        If Not chkAdvance.Checked And _
        Not chkCredit.Checked And _
        Not chkOrder.Checked And _
        Not chkOther.Checked And _
        Not chkJandD.Checked And _
        Not chkPurReturn.Checked Then
            chkAdvance.Checked = True
            chkCredit.Checked = True
            chkOrder.Checked = True
            chkOther.Checked = True
            chkJandD.Checked = True
            chkPurReturn.Checked = True
        End If
        Me.Refresh()
        Dim Display As String
        Dim OrderBy As String
        Dim Type As String = ""
        If chkAdvance.Checked Then Type += "A,"
        If chkCredit.Checked Or chkPurReturn.Checked Then Type += "D,"
        If chkOrder.Checked Then Type += "O,"
        If chkOther.Checked Then Type += "T,"
        If chkJandD.Checked Then Type += "J,"

        Dim acGrpcode As String
        If cmbAcGrp.Text <> "" Or cmbAcGrp.Text <> "ALL" Then
            acGrpcode = GetSelectedAcgrpcode(cmbAcGrp, False) 'objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME= '" & cmbAcGrp.Text & "'")
        End If
        If cmbAccountType.Text <> "" Then
            If cmbAccountType.Text = "SMITH" Then
                acType = "G"
            ElseIf cmbAccountType.Text = "DEALER" Then
                acType = "D"
            ElseIf cmbAccountType.Text = "CUSTOMER" Then
                acType = "C"
            ElseIf cmbAccountType.Text = "INTERNAL" Then
                acType = "I"
            ElseIf cmbAccountType.Text = "OTHERS" Then
                acType = "O"
            End If
        End If

        Dim TEMPTYPE As String = ""
        If Type <> "" Then TEMPTYPE = Type.Substring(0, Type.Length - 1)
        Type = TEMPTYPE
        btnView_Search.Enabled = False
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        temptable = "TEMP" & systemId & "ADVCR"

        strSql = " EXEC " & cnAdminDb & "..SP_RPT_CRADV"
        strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTABLE='" & temptable & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID='" & cmbCostCentre.Text & "'"
        strSql += vbCrLf + " ,@DETAILED='" & Detailed & "'"
        strSql += vbCrLf + " ,@ADVCR='" & Type & "'"
        strSql += vbCrLf + " ,@ACCODE='" & ACCODE & "'"
        strSql += vbCrLf + " ,@ACTYPE = '" & acType & "'"
        strSql += vbCrLf + " ,@ACGROUP = '" & IIf(acGrpcode Is Nothing, "", acGrpcode) & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim DtGrid As New DataTable
        DtGrid = dss.Tables(0)
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Sub
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        If Detailed = "N" Then
            gridView.DataSource = Nothing
            gridView.DataSource = DtGrid
        Else
            GridView2.DataSource = Nothing
            GridView2.DataSource = DtGrid
        End If

        gridView.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("BALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("KEYNO").Visible = False
        gridView.Columns("COLHEAD").Visible = False
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " SELECT * FROM MASTER.." & temptable & "CRADV"

        If Detailed = "N" Then
            gridView.Columns("OPENING").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("PARTICULARS").Width = 250
            gridView.Columns("DEBIT").Width = 150
            gridView.Columns("CREDIT").Width = 150
            gridView.Columns("BALANCE").Width = 150
            gridView.Columns("ACCODE").Visible = False
            gridView.Visible = True
            GridView2.Visible = False
            tabView.Show()
            FillGridGroupStyle_KeyNoWise(gridView)
            If DtGrid.Rows.Count > 0 Then tabmain.SelectedTab = tabView : gridView.Focus()
            lblHeading.Text = ""
            If chkAdvance.Checked Then lblHeading.Text = "ADVANCE/OUTSTANDING,"
            If chkCredit.Checked Then lblHeading.Text += " CREDIT,"
            If chkOrder.Checked Then lblHeading.Text += " ORDER,"
            If chkOther.Checked Then lblHeading.Text += " OTHER,"
            If chkJandD.Checked Then lblHeading.Text += " JND,"
            lblHeading.Text += " REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            lblHeading.Text += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")

        ElseIf Detailed = "Y" Then
            GridView2.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView2.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView2.Columns("PARTICULARS").HeaderText = "REMARKS"
            GridView2.Columns("DEBIT").Width = 139
            GridView2.Columns("CREDIT").Width = 139
            GridView2.Columns("PARTICULARS").Width = 310
            GridView2.Columns("TRANDATE").Width = 157
            GridView2.Columns("TRANNO").Width = 100
            GridView2.Columns("TRANTYPE").Width = 150
            GridView2.Columns("KEYNO").Visible = False
            GridView2.Columns("COLHEAD").Visible = False
            gridView.Visible = False
            GridView2.Focus()
            GridView2.Visible = True
            tabView.Show()
            ' FillGridGroupStyle_KeyNoWise(GridView2)
            GridViewFormat(GridView2)
            If GridView2.Rows.Count > 0 Then GridView2.Focus()
            lblHeading.Text = ""
            strSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & ACCODE & "'"
            lblHeading.Text = globalMethods.GetSqlValue(cn, strSql)
            lblHeading.Text += " REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            lblHeading.Text += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")

        End If
        btnView_Search.Enabled = True
        lblHeading.Font = New Font("VERDANA", 9, FontStyle.Bold)
        pnlHeading.Visible = True


    End Sub
    Function GridViewFormat(ByVal grdvw As DataGridView) As Integer
        For Each dgvRow As DataGridViewRow In grdvw.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "TS"
                        .Cells("TRANDATE").Style.BackColor = Color.LightGreen
                        .Cells("TRANDATE").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S1"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S2"
                        .Cells("TRANDATE").Style.BackColor = Color.LightGreen
                        .Cells("TRANDATE").Style.ForeColor = Color.Red
                        .Cells("DEBIT").Style.BackColor = Color.Red
                        .Cells("CREDIT").Style.BackColor = Color.Red
                        .Cells("DEBIT").Style.ForeColor = Color.White
                        .Cells("CREDIT").Style.ForeColor = Color.White
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next
    End Function
    Public Function Getaccode(ByVal Source As String, Optional ByVal sep As String = ",") As String
        Dim adt As New DataTable()
        Dim ret As String = ""
        Dim sp(50) As String
        If Not Source = "ALL" Then
            strSql = "SELECT ACCODE from " & cnAdminDb & "..ACHEAD WHERE ACNAME IN(" & Source & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(adt)
            If adt.Rows.Count > 0 Then
                For i As Integer = 0 To adt.Rows.Count - 1
                    sp(i) = adt.Rows(i).Item("ACCODE").ToString()
                Next

            End If

            For Each s As String In sp
                If Not s = Nothing Then
                    ret += "" & Trim(s) & ","
                Else
                    Exit For
                End If
            Next
            If ret <> "" Then
                ret = Mid(ret, 1, ret.Length - 1)
            End If
        End If

        Return ret
    End Function
    Private Sub btnView_Search__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim acstr As String = GetQryString(chkCmbAcName.Text)
        Dim accodestr As String = Getaccode(acstr)
        NewReport("N", accodestr)
        Exit Sub
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.D Then
            NewReport("Y", gridView.CurrentRow.Cells("ACCODE").Value.ToString())
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        pnlHeading.Visible = False
        funcAddCostCentre()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        Prop_Gets()
        btnView_Search.Enabled = True
        dtpFrom.Select()
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
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, GiriPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, GiriPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub



    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabmain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
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
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseSales_properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)

        'obj.p_rbtDetailed = rbtDetailed.Checked
        'obj.p_rbtSummary = rbtSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseSales_properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)

        'rbtDetailed.Checked = obj.p_rbtDetailed
        'rbtSummary.Checked = obj.p_rbtSummary
    End Sub

    Private Sub frmAdvanceOutstandingReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tabmain.ItemSize = New Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        grp1.Location = New Point((ScreenWid - grp1.Width) / 2, ((ScreenHit - 128) - grp1.Height) / 2)
        Me.tabmain.SelectedTab = tabGen

        LoadCompany(chkLstCompany)
        strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACGRPNAME,CONVERT(vARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
        strSql += " ORDER BY RESULT,ACGRPNAME"
        Dim dtAcGrp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcGrp)
        GiritechPack.GlobalMethods.FillCombo(cmbAcGrp, dtAcGrp, "ACGRPNAME", , "ALL")

        cmbAccountType.Items.Add("SMITH")
        cmbAccountType.Items.Add("CUSTOMER")
        cmbAccountType.Items.Add("DEALER")
        cmbAccountType.Items.Add("OTHERS")
        cmbAccountType.Items.Add("INTERNAL")
        cmbAccountType.Text = "SMITH"

        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strSql += " ORDER BY RESULT,ACNAME"
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        GiritechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")

        pnlHeading.Visible = False
        btnNew_Click(Me, New EventArgs)
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
    End Sub

    Private Sub GridView2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView2.KeyDown
        If e.KeyCode = Keys.Escape Then
            GridView2.Visible = False
            gridView.Visible = True
            gridView.Focus()
        End If
    End Sub

    Private Sub chkCmbAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbAcName.Enter
        If cmbAccountType.Text <> "" Then
            If cmbAccountType.Text = "SMITH" Then
                acType = "G"
            ElseIf cmbAccountType.Text = "DEALER" Then
                acType = "D"
            ElseIf cmbAccountType.Text = "CUSTOMER" Then
                acType = "C"
            ElseIf cmbAccountType.Text = "INTERNAL" Then
                acType = "I"
            ElseIf cmbAccountType.Text = "OTHERS" Then
                acType = "O"
            End If
        End If

        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And cmbAcGrp.Text = "ALL" Then
            strSql += " WHERE ACTYPE ='" & acType & "'"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        'objGPack.FillCombo(strSql, cmbAcName_Own, , False)
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        GiritechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")

    End Sub

    Private Sub cmbAccountType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAccountType.Leave
        If cmbAccountType.Text <> "" Then
            If cmbAccountType.Text = "SMITH" Then
                acType = "G"
            ElseIf cmbAccountType.Text = "DEALER" Then
                acType = "D"
            ElseIf cmbAccountType.Text = "CUSTOMER" Then
                acType = "C"
            ElseIf cmbAccountType.Text = "INTERNAL" Then
                acType = "I"
            ElseIf cmbAccountType.Text = "OTHERS" Then
                acType = "O"
            End If
        End If

        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And cmbAcGrp.Text = "ALL" Then
            strSql += " WHERE ACTYPE ='" & acType & "'"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        'objGPack.FillCombo(strSql, cmbAcName_Own, , False)
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        GiritechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")

    End Sub

    Private Sub cmbAcGrp_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGrp.Leave
        If cmbAccountType.Text <> "" Then
            If cmbAccountType.Text = "SMITH" Then
                acType = "G"
            ElseIf cmbAccountType.Text = "DEALER" Then
                acType = "D"
            ElseIf cmbAccountType.Text = "CUSTOMER" Then
                acType = "C"
            ElseIf cmbAccountType.Text = "INTERNAL" Then
                acType = "I"
            ElseIf cmbAccountType.Text = "OTHERS" Then
                acType = "O"
            End If
        End If

        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And cmbAcGrp.Text = "ALL" Then
            strSql += " WHERE ACTYPE ='" & acType & "'"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        'objGPack.FillCombo(strSql, cmbAcName_Own, , False)
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        GiritechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")

    End Sub

End Class


Public Class frmAdvanceOutstandingReport_properties
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
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property

    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property


    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtAdvance As Boolean = True
    Public Property p_rbtAdvance() As Boolean
        Get
            Return rbtAdvance
        End Get
        Set(ByVal value As Boolean)
            rbtAdvance = value
        End Set
    End Property

    Private rbtCredit As Boolean = False
    Public Property p_rbtCredit() As Boolean
        Get
            Return rbtCredit
        End Get
        Set(ByVal value As Boolean)
            rbtCredit = value
        End Set
    End Property


End Class