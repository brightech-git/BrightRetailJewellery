Imports System.Data.OleDb
Public Class frmOtherbranchadvanceadjustment
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim SelectedCompany As String
    Dim flagHighlight As Boolean = IIf(GetAdmindbSoftValue("RPT_COLOR", "N") = "Y", True, False)
    Dim ChitDbId As String = GetAdmindbSoftValue("CHITDBPREFIX", "")

    Private Sub frmCustomerOutstanding_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
        'grpOutStanding.Visible = True
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpFrom.Focus()
        dtpFrom.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Report()
        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP_OUTSTANDING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUTSTANDING          "
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP_CROUTSTANDING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_CROUTSTANDING          "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        Me.Refresh()
        Dim Display As String
        Dim summ As String
        Dim OrderBy As String
        Dim Type As String = ""
        Dim actype As String = "N"
        Display = "A"
        summ = "N"
        If chkadvance.Checked = True Then
            actype = "A"
        End If
        If chkScheme.Checked = True Then
            actype = "C"
        End If
        If chkadvance.Checked = True And chkScheme.Checked = True Then
            actype = "B"
        End If

        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_OTHERBRANCHADVANCEADJUST"
        strSql += vbCrLf + " @DBNAME= '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & cmbCostCentre.Text & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & cmbcompany.Text & "'"
        strSql += vbCrLf + " ,@ACFILTER = '" & actype & "'"
        strSql += vbCrLf + " ,@CHITDBID = '" & ChitDbId & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,NAME,CTRANNO"
        gridView.DataSource = Nothing
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Dim da1 As New OleDbDataAdapter
        da1 = New OleDbDataAdapter(strSql, cn)
        da1.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        dtGrid.Columns("TRANNO").SetOrdinal(2)
        dtGrid.Columns("TRANDATE").SetOrdinal(3)
        dtGrid.Columns("DEBIT").SetOrdinal(4)
        dtGrid.Columns("CREDIT").SetOrdinal(5)
        dtGrid.Columns("BALANCE").SetOrdinal(6)
        dtGrid.Columns("NAME").SetOrdinal(7)
        dtGrid.Columns("CTRANNO").SetOrdinal(9)
        gridView.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise(gridView)
        gridformat()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        If gridView.Columns.Contains("SALESPERSON") = True Then gridView.Columns("SALESPERSON").Visible = False
        If gridView.Columns.Contains("COSTNAME") = True Then gridView.Columns("COSTNAME").Visible = True
        If gridView.Columns.Contains("COSTID") = True Then gridView.Columns("COSTID").Visible = False
        If gridView.Columns.Contains("TRANDATE") = True Then gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("LASTTRANDATE") = True Then gridView.Columns("LASTTRANDATE").Visible = False
        If gridView.Columns.Contains("CTRANDATE") = True Then gridView.Columns("CTRANDATE").Visible = False
        If gridView.Columns.Contains("CTRANNO") = True Then gridView.Columns("CTRANNO").Visible = False
        If gridView.Columns.Contains("BALANCE") = True Then gridView.Columns("BALANCE").Visible = False
        If gridView.Columns.Contains("TRANTYPE") = True Then gridView.Columns("TRANTYPE").Visible = False
        If gridView.Columns.Contains("RUNNO") = True Then gridView.Columns("RUNNO").Visible = False
        If gridView.Columns.Contains("RESULT") = True Then gridView.Columns("RESULT").Visible = False
        If gridView.Columns.Contains("COLHEAD") = True Then gridView.Columns("COLHEAD").Visible = False
        If gridView.Columns.Contains("KEYNO") = True Then gridView.Columns("KEYNO").Visible = False
        If gridView.Columns.Contains("KNO") = True Then gridView.Columns("KNO").Visible = False
        If gridView.Columns.Contains("AREAORD") = True Then gridView.Columns("AREAORD").Visible = False
        If gridView.Columns.Contains("AREA") = True Then gridView.Columns("AREA").Visible = False
        If gridView.Columns.Contains("MOBILE") = True Then gridView.Columns("MOBILE").Visible = False
        If gridView.Columns.Contains("AMOUNT") = True Then gridView.Columns("AMOUNT").Visible = False
        If gridView.Columns.Contains("REMARK") = True Then gridView.Columns("REMARK").Visible = False
        If gridView.Columns.Contains("SALESPERSON") = True Then gridView.Columns("SALESPERSON").Visible = False
        If gridView.Columns.Contains("BATCHNO") = True Then gridView.Columns("BATCHNO").Visible = False
        If gridView.Columns.Contains("COMPANYID") = True Then gridView.Columns("COMPANYID").Visible = False
        If gridView.Rows.Count > 0 Then
            lblTitle.Visible = True
            Dim strTitle As String = Nothing
            strTitle = " OTHER BRANCH ADVANCE \ CHIT ADJUSTMENT DETAIL REPORT"
            strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            strTitle += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")
            lblTitle.Text = strTitle
            lblTitle.Height = gridView.ColumnHeadersHeight
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        End If
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub
    Private Sub gridformat()

        With gridView
            If .Columns.Contains("PARTICULAR") Then
                With .Columns("PARTICULAR")
                    .Width = 300
                End With
            End If
            With .Columns("TRANNO")
                .Width = 150
            End With
            With .Columns("TRANDATE")
                .Width = 180
            End With
            With .Columns("DEBIT")
                .Width = 130
            End With
            With .Columns("CREDIT")
                .Width = 130
            End With
            With .Columns("NAME")
                .Width = 180
            End With
            With .Columns("COSTNAME")
                .Width = 180
            End With
            With .Columns("FROMCOSTID")
                .Width = 180
            End With

        End With
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                If gridView.Columns.Contains("COLHEAD") = True Then
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "G1"
                            .Cells("PARTICULAR").Style.ForeColor = Color.Red
                            .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            .DefaultCellStyle.BackColor = Color.Wheat
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End Select
                End If
            End With
        Next
    End Sub


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkadvance.Checked = False And chkScheme.Checked = False Then MsgBox("Select any one Option.", MsgBoxStyle.Information) : Exit Sub
        Prop_Sets()
        Report()
        Prop_Gets()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        funcAddcompany()
        funcAddCostCentre()
        Prop_Gets()
        dtpFrom.Focus()
    End Sub
    Function funcAddcompany() As Integer
        strSql += " SELECT COMPANYNAME,CONVERT(vARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        strSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        objGPack.FillCombo(strSql, cmbcompany, False, False)
        cmbcompany.SelectedIndex = 0
    End Function

    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
            cmbCostCentre.Items.Clear()
            'cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        grbRunNoFocus.Visible = False
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub



    Private Sub frmCustomerOutstanding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridCustomerOutstanding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
        If rbtDetailWise.Checked = True Then
            If e.KeyCode = Keys.D Then
                Try
                    Dim dtRunNo As New DataTable
                    dtRunNo.Clear()
                    gridRunNoFocus.DataSource = Nothing
                    strSql = "select TRANNO,TRANDATE,NAME,DEBIT,CREDIT from TEMP" & systemId & "FINAL where RUNNO='" & gridView.CurrentRow.Cells("RUNNO").Value & "' ORDER BY TRANNO"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtRunNo)
                    gridRunNoFocus.DataSource = dtRunNo
                    grbRunNoFocus.Text = gridView.CurrentRow.Cells(0).Value & " " & "DETAILS"
                    grbRunNoFocus.Visible = True
                    With gridRunNoFocus
                        With .Columns("TRANNO")
                            .Width = 70
                            .HeaderText = "BILLNO"
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("TRANDATE")
                            .Width = 80
                            .HeaderText = "BILLDATE"
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("NAME")
                            .Width = 200
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("CREDIT")
                            .Width = 80
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                        With .Columns("DEBIT")
                            .Width = 80
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                        End With
                    End With
                Catch ex As Exception

                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            ElseIf e.KeyCode = Keys.E Then
                'Dim objSecret As New frmAdminPassword()
                'If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                '    gridView.Focus()
                '    Exit Sub
                'End If
                'Me.GroupBox1.Visible = True
                'txtOldRunNo.Text = gridView.CurrentRow.Cells("RUNNO").Value
                'txtNewRunNo.Text = ""
                'txtNewRunNo.Focus()
            End If
        End If
        gridRunNoFocus.Focus()
    End Sub

    Private Sub gridRunNoFocus_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridRunNoFocus.KeyDown
        If e.KeyCode = Keys.Escape Then
            grbRunNoFocus.Visible = False
            gridView.Visible = True
            gridView.Focus()
        End If
    End Sub

    Private Sub gridRunNoFocus_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridRunNoFocus.LostFocus
        grbRunNoFocus.Visible = False
        gridView.Visible = True
        gridView.Focus()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then

        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()



        ' Add any initialization after the InitializeComponent() call.
        'AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
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
    Private Sub Prop_Gets()
        Dim obj As New frmOtherbranchadvanceadjustment_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOtherbranchadvanceadjustment_Properties))
        chkadvance.Checked = obj.p_chkAdvance
        chkScheme.Checked = obj.p_chkScheme
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmOtherbranchadvanceadjustment_Properties
        obj.p_chkAdvance = chkadvance.Checked
        obj.p_chkScheme = chkScheme.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmOtherbranchadvanceadjustment_Properties))
    End Sub

End Class


Public Class frmOtherbranchadvanceadjustment_Properties

    Private chkAdvance As Boolean = False
    Public Property p_chkAdvance() As Boolean
        Get
            Return chkAdvance
        End Get
        Set(ByVal value As Boolean)
            chkAdvance = value
        End Set
    End Property
    Private chkScheme As Boolean = False
    Public Property p_chkScheme() As Boolean
        Get
            Return chkScheme
        End Get
        Set(ByVal value As Boolean)
            chkScheme = value
        End Set
    End Property
End Class