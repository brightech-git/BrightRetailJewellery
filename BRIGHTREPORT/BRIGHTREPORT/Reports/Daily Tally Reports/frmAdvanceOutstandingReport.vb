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
    Dim dtaccnames As New DataTable
    Dim dtacnames As New DataTable
    Dim objSearch As Object = Nothing
    Dim SMS_DUE_REMAINDER As String
    Dim Detailed As Boolean = False
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        '  ProcAddNodeId()
    End Sub

    Private Sub txtAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcName.GotFocus
        'txtAcName.Text = cmbAcName.Text
        Dgvsearch.Location = New Point(txtAcName.Location.X, txtAcName.Location.Y + txtAcName.Height)
        Dgvsearch.Size = New Size(txtAcName.Size.Width, 150)
        Dgvsearch.Columns(0).Width = txtAcName.Size.Width
        Dgvsearch.BringToFront()
    End Sub
    Private Sub txtAcname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAcName.KeyDown
        If e.KeyCode = Keys.Down Then
            If Dgvsearch.Visible Then
                If Dgvsearch.RowCount > 0 Then
                    Dgvsearch.CurrentCell = Dgvsearch.Rows(0).Cells(Dgvsearch.FirstDisplayedCell.ColumnIndex)
                    Dgvsearch.Select()
                End If
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            Exit Sub
        End If
    End Sub

    Private Sub txtAcname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcName.TextChanged
        If txtAcName.Focused = False Then Exit Sub
        If txtAcName.Text = "" Then
            Dgvsearch.Visible = False
            Exit Sub
        Else
            Dgvsearch.Visible = True
        End If
        Dim sw As String = txtAcName.Text
        Dim RowFilterStr As String
        RowFilterStr = "ACNAME LIKE '%" & sw & "%'"
        dtaccnames.DefaultView.RowFilter = RowFilterStr
    End Sub

    
    Private Sub frmItemWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) And tabmain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" And tabmain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Escape) And Detailed = False Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Public Function GetSelectedAcgrpcode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
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
        If chkAcname.Checked Then
            If chkcmbAcGrp.Text <> "" Or chkcmbAcGrp.Text <> "ALL" Then
                acGrpcode = GetSelectedAcgrpcode(chkcmbAcGrp, False) 'objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME= '" & cmbAcGrp.Text & "'")
            End If
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
        If gridView.Columns.Contains("MOBILE") = True Then gridView.Columns("MOBILE").Visible = False

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
    Public Function Getaccode(ByVal Source As String, Optional ByVal sep As String = ",", Optional ByVal WithQuotes As Boolean = False) As String
        Dim adt As New DataTable()
        Dim ret As String = ""
        Dim sp(50) As String
        If Not Source = "ALL" Then
            If WithQuotes Then
                strSql = "SELECT ACCODE from " & cnAdminDb & "..ACHEAD WHERE ACNAME IN('" & Source & "')"
            Else
                strSql = "SELECT ACCODE from " & cnAdminDb & "..ACHEAD WHERE ACNAME IN(" & Source & ")"
            End If
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Prop_Sets()
        Dim acstr As String = IIf(chkAcname.Checked, GetQryString(chkCmbAcName.Text), txtAcName.Text)
        Dim accodestr As String
        If chkAcname.Checked Then
            accodestr = Getaccode(acstr)
        Else
            accodestr = Getaccode(acstr, ",", True)
        End If
        If chkCredit.Checked And chkAdvance.Checked = False And chkOrder.Checked = False _
                                And chkJandD.Checked = False And chkPurReturn.Checked = False _
                                And chkOther.Checked = False Then
            If SMS_DUE_REMAINDER <> "" Then btnSendSms.Enabled = True
        End If
        Detailed = False
        NewReport("N", accodestr)
        'objSearch = New frmGridSearch(gridView)
        Exit Sub
    End Sub

    Private Sub cmbAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        txtAcName.Visible = True
        txtAcName.BringToFront()
        txtAcName.Select()
    End Sub

    Private Sub cmbAcName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dgvsearch.Visible = False
        txtAcName.Visible = False
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Detailed Then
                Dim acstr As String = IIf(chkAcname.Checked, GetQryString(chkCmbAcName.Text), txtAcName.Text)
                Dim accodestr As String
                If chkAcname.Checked Then
                    accodestr = Getaccode(acstr)
                Else
                    accodestr = Getaccode(acstr, ",", True)
                End If
                NewReport("N", accodestr)
                Detailed = False
                btnSendSms.Enabled = True
            Else
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyCode = Keys.D Then
            NewReport("Y", gridView.CurrentRow.Cells("ACCODE").Value.ToString())
            Detailed = True
            btnSendSms.Enabled = False
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        pnlHeading.Visible = False
        funcAddCostCentre()
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        chkAcname.Checked = True
        Prop_Gets()
        btnView_Search.Enabled = True
        btnSendSms.Enabled = False
        dtpFrom.Select()
        Detailed = False
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabmain.SelectedTab = tabGen
        btnSendSms.Enabled = False
        dtpFrom.Focus()
    End Sub
    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'dtpTo.MinimumDate = dtpFrom.Value
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
        Dim obj As New frmAdvanceOutstandingReport_properties
        obj.p_ChkAcName = chkAcname.Checked
        obj.p_rbtAdvance = chkAdvance.Checked
        obj.p_rbtCredit = chkCredit.Checked
        obj.p_rbtOrder = chkOrder.Checked
        obj.p_rbtOther = chkOther.Checked
        obj.p_rbtOrder = chkOrder.Checked
        obj.p_rbtChkJNd = chkJandD.Checked
        obj.p_rbtChkCrPurch = chkPurReturn.Checked
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        GetChecked_CheckedList(chkCmbAcName, obj.p_chkcmbAcName)
        GetChecked_CheckedList(chkcmbAcGrp, obj.p_chkcmbAcGroup)
        obj.p_txtAccname = txtAcName.Text
        obj.p_cmbAcType = cmbAccountType.Text
        obj.p_cmbCostCentre = cmbCostCentre.Text
        SetSettingsObj(obj, Me.Name, GetType(frmAdvanceOutstandingReport_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmAdvanceOutstandingReport_properties
        GetSettingsObj(obj, Me.Name, GetType(frmAdvanceOutstandingReport_properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkAcname.Checked = obj.p_ChkAcName
        chkAdvance.Checked = obj.p_rbtAdvance
        chkCredit.Checked = obj.p_rbtCredit
        chkOrder.Checked = obj.p_rbtOrder
        chkOther.Checked = obj.p_rbtOther
        chkOrder.Checked = obj.p_rbtOrder
        chkJandD.Checked = obj.p_rbtChkJNd
        chkPurReturn.Checked = obj.p_rbtChkCrPurch
        cmbAccountType.Text = obj.p_cmbAcType
        cmbCostCentre.Text = obj.p_cmbCostCentre
        SetChecked_CheckedList(chkcmbAcGrp, obj.p_chkcmbAcGroup, "ALL")
        SetChecked_CheckedList(chkCmbAcName, obj.p_chkcmbAcName, "ALL")
        If chkAcname.Checked = False Then
            txtAcName.Text = obj.p_txtAccname
        End If
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
        BrighttechPack.GlobalMethods.FillCombo(chkcmbAcGrp, dtAcGrp, "ACGRPNAME", , "ALL")

        cmbAccountType.Items.Add("SMITH")
        cmbAccountType.Items.Add("CUSTOMER")
        cmbAccountType.Items.Add("DEALER")
        cmbAccountType.Items.Add("OTHERS")
        cmbAccountType.Items.Add("INTERNAL")
        cmbAccountType.Text = "SMITH"

        chkAcname.Checked = True
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strSql += " ORDER BY RESULT,ACNAME"
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")


        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y') <> 'H'"
        strSql += " ORDER BY ACNAME"
        dtaccnames = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtaccnames)
        Dgvsearch.DataSource = dtaccnames
        Dgvsearch.ColumnHeadersVisible = False
        Dgvsearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Dgvsearch.Visible = False
        btnNew_Click(Me, New EventArgs)
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        SMS_DUE_REMAINDER = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_DUE_REMAINDER' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
        btnSendSms.Enabled = False
    End Sub

    Private Sub GridView2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView2.KeyDown
        If e.KeyCode = Keys.Escape Then
            GridView2.Visible = False
            gridView.Visible = True
            gridView.Focus()
        End If
    End Sub
    Private Sub chkcmbAcGrp_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbAcGrp.Leave
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
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y')<>'N'"
        If chkcmbAcGrp.Text <> "ALL" And chkcmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " AND ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkcmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf chkcmbAcGrp.Text <> "ALL" And chkcmbAcGrp.Text <> "" Then
            strSql += " AND ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkcmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And chkcmbAcGrp.Text = "ALL" Then
            strSql += " AND ACTYPE ='" & acType & "'"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        Dim dtacname As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtacname, "ACNAME", , "ALL")
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y') <> 'N'"
        If chkcmbAcGrp.Text <> "ALL" And chkcmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " AND ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkcmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf chkcmbAcGrp.Text <> "ALL" And chkcmbAcGrp.Text <> "" Then
            strSql += " AND ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkcmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And chkcmbAcGrp.Text = "ALL" Then
            strSql += " AND ACTYPE ='" & acType & "'"
        End If
        strSql += " ORDER BY ACNAME"
        dtaccnames = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtaccnames)
        Dgvsearch.DataSource = Nothing
        Dgvsearch.DataSource = dtaccnames
    End Sub

    Private Sub DgvSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgvsearch.KeyDown
        If e.KeyCode = Keys.Up Then
            If Dgvsearch.CurrentRow Is Nothing Then Exit Sub
            If Dgvsearch.CurrentRow.Index = 0 Then
                txtAcName.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If Dgvsearch.CurrentRow Is Nothing Then Exit Sub
            e.Handled = True
            txtAcName.Text = Dgvsearch.CurrentRow.Cells("ACNAME").Value.ToString
            Dgvsearch.Visible = False
            txtAcName.Select()
        End If
    End Sub
    Private Sub DgvSearch_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles Dgvsearch.CellPainting
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtAcName.Text
            If Not String.IsNullOrEmpty(sw) Then
                'highlight search word

                Dim val As String = DirectCast(e.FormattedValue, String)

                Dim sindx As Integer = val.ToLower.IndexOf(sw.ToLower)
                If sindx >= 0 Then
                    'search word found

                    Dim sf As Font = New Font(e.CellStyle.Font, FontStyle.Bold)
                    Dim sbr As SolidBrush = New SolidBrush(Color.Red)

                    Dim br As SolidBrush
                    If e.State = DataGridViewElementStates.Selected Then
                        br = New SolidBrush(e.CellStyle.SelectionForeColor)
                    Else
                        br = New SolidBrush(e.CellStyle.ForeColor)
                    End If

                    Dim sBefore As String = val.Substring(0, sindx)
                    Dim sBeforeSize As SizeF = e.Graphics.MeasureString(sBefore, e.CellStyle.Font, e.CellBounds.Size)
                    Dim sWord As String = val.Substring(sindx, sw.Length)
                    Dim sWordSize As SizeF = e.Graphics.MeasureString(sWord, sf, e.CellBounds.Size)
                    Dim sAfter As String = val.Substring(sindx + sw.Length, val.Length - (sindx + sw.Length))

                    e.Graphics.DrawString(sBefore, e.CellStyle.Font, br, e.CellBounds)
                    e.Graphics.DrawString(sWord, sf, sbr, e.CellBounds.X + sBeforeSize.Width, e.CellBounds.Location.Y)
                    e.Graphics.DrawString(sAfter, e.CellStyle.Font, br, e.CellBounds.X + sBeforeSize.Width + sWordSize.Width, e.CellBounds.Location.Y)
                Else
                    'paint as usual
                    e.PaintContent(e.CellBounds)
                End If
            Else
                'paint as usual
                e.PaintContent(e.CellBounds)
            End If
        End If
    End Sub
    'Private Sub FindSearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindSearchToolStripMenuItem.Click
    '    If Not Dgvsearch.RowCount > 0 Then
    '        Exit Sub
    '    End If
    '    If Not Dgvsearch.RowCount > 0 Then Exit Sub
    '    objSearch.Show()
    'End Sub
    Private Sub chkAcname_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAcname.CheckedChanged
        chkCmbAcName.Visible = chkAcname.Checked
        txtAcName.Visible = Not chkAcname.Checked
    End Sub

    Private Sub btnSendSms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendSms.Click
        If gridView.RowCount = 0 Then Exit Sub
        Dim Send As Boolean = False
        Dim dt As New DataTable
        dt = gridView.DataSource
        Dim dtAccode As DataTable = dt.DefaultView.ToTable(True, "ACCODE")
        For Each rowOut As DataRow In dtAccode.Rows
            Dim Mobile As String = ""
            Dim Name As String = ""
            Dim TempMsg As String = ""
            Dim Amount As Decimal
            Dim TranNo As String = ""
            Dim rw() As DataRow = dt.Select("ACCODE='" & rowOut.Item("ACCODE") & "'", Nothing)
            If rw.Length > 0 Then
                strSql = vbCrLf + " IF OBJECT_ID('" & cnAdminDb & "..TEMP" & systemId & "ADVCR') IS NOT NULL "
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..TEMP" & systemId & "ADVCR WHERE ACCODE='" & rowOut.Item("ACCODE").ToString & "' ORDER BY TRANDATE DESC,TRANNO DESC"
                strSql += vbCrLf + " END"
                TranNo = Val(objGPack.GetSqlValue(strSql, , "").ToString)
                Mobile = rw(0).Item("MOBILE").ToString
                Name = rw(0).Item("PARTICULARS").ToString
                Amount = Val(dt.Compute("SUM(BALANCE)", "ACCODE='" & rowOut.Item("ACCODE") & "'").ToString)
                If Amount = 0 Then Continue For
                TempMsg = SMS_DUE_REMAINDER
                TempMsg = Replace(SMS_DUE_REMAINDER, vbCrLf, "")
                TempMsg = Replace(TempMsg, "<NAME>", Name)
                TempMsg = Replace(TempMsg, "<AMOUNT>", Amount.ToString)
                TempMsg = Replace(TempMsg, "<TRANNO>", "[Ref. " & TranNo.ToString & "]")
                If SmsSend(TempMsg, Mobile) = True Then
                    Send = True
                Else
                    Exit For
                End If
            End If
        Next
        If Send Then MsgBox("Sms Generated.", MsgBoxStyle.Information)
    End Sub
    Public Function SmsSend(ByVal Msg As String, ByVal Mobile As String) As Boolean
        If Msg <> "" And Mobile.Length = 10 Then
            strSql = "SELECT COUNT(*)CNT FROM SYSDATABASES WHERE NAME='AKSHAYASMSDB'"
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                strSql = "INSERT INTO AKSHAYASMSDB..SMSDATA(MOBILENO,MESSAGES,STATUS,EXPIRYDATE,UPDATED)"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & Mobile.Trim & "','" & Msg & "','N'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ) "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                Return True
            Else
                MsgBox("AkhayaSmsDb Not Found", MsgBoxStyle.Information)
                Return False
            End If
        Else
            Return True
        End If
    End Function
End Class
Public Class frmAdvanceOutstandingReport_properties
    Private ChkAcName As Boolean = False
    Public Property p_ChkAcName() As Boolean
        Get
            Return ChkAcName
        End Get
        Set(ByVal value As Boolean)
            ChkAcName = value
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
    Private chkcmbAcName As New List(Of String)
    Public Property p_chkcmbAcName() As List(Of String)
        Get
            Return chkcmbAcName
        End Get
        Set(ByVal value As List(Of String))
            chkcmbAcName = value
        End Set
    End Property
    Private chkcmbAcGroup As New List(Of String)
    Public Property p_chkcmbAcGroup() As List(Of String)
        Get
            Return chkcmbAcGroup
        End Get
        Set(ByVal value As List(Of String))
            chkcmbAcGroup = value
        End Set
    End Property
    Private txtAccname As String = Nothing
    Public Property p_txtAccname() As String
        Get
            Return txtAccname
        End Get
        Set(ByVal value As String)
            txtAccname = value
        End Set
    End Property
    Private cmbAcType As String = Nothing
    Public Property p_cmbAcType() As String
        Get
            Return cmbAcType
        End Get
        Set(ByVal value As String)
            cmbAcType = value
        End Set
    End Property
    Private cmbCostCentre As String = Nothing
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
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
    Private rbtOther As Boolean = True
    Public Property p_rbtOther() As Boolean
        Get
            Return rbtOther
        End Get
        Set(ByVal value As Boolean)
            rbtOther = value
        End Set
    End Property
    Private rbtOrder As Boolean = True
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property
    Private rbtChkJNd As Boolean = True
    Public Property p_rbtChkJNd() As Boolean
        Get
            Return rbtChkJNd
        End Get
        Set(ByVal value As Boolean)
            rbtChkJNd = value
        End Set
    End Property
    Private rbtChkCrPurch As Boolean = True
    Public Property p_rbtChkCrPurch() As Boolean
        Get
            Return rbtChkCrPurch
        End Get
        Set(ByVal value As Boolean)
            rbtChkCrPurch = value
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