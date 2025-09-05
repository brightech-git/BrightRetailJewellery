Imports System.Data.OleDb
Public Class frmClosingStock
    Dim strSql As String = Nothing
    Dim dtTemp As New DataTable
    Dim ftrIssue As String
    Dim ftrReceipt As String
    Dim ftrStoneStr As String
    Dim dsGridView As New DataSet
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtGridView As New DataTable

    Private Sub frmCategoryStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Me.Cursor = Cursors.WaitCursor
        gridView.DataSource = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        lblTitle.Text = ""
        Me.Refresh()
        pnlHeading.Visible = False
        Dim selCatCode As String = Nothing
        If cmbCatName.Text = "ALL" Then
            selCatCode = "ALL"
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(cmbCatName.Text) & ")"
            Dim dtCat As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCat)
            If dtCat.Rows.Count > 0 Then
                For i As Integer = 0 To dtCat.Rows.Count - 1
                    selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
                Next
                If selCatCode <> "" Then
                    selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                End If
            End If
        End If
        Dim CompId As String = "ALL"
        If CmbCompany.Text = "ALL" Then
            CompId = "ALL"
        Else
            CompId = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME='" & CmbCompany.Text & "'", "COMPANYID")
        End If
        If RbtLifo.Checked Then
            strSql = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
        Else
            strSql = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
        End If
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@FRMDATE = '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
        strSql += vbCrLf + " ,@CATNAME = '" & cmbCatName.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & CompId & "'"
        If rbtSummary.Checked Then
            strSql += vbCrLf + " ,@RPTTYPE = 'S'"
        ElseIf rbtDateWise.Checked Then
            strSql += vbCrLf + " ,@RPTTYPE = 'D'"
        ElseIf rbtTranNoWise.Checked Then
            strSql += vbCrLf + " ,@RPTTYPE = 'T'"
        Else
            strSql += vbCrLf + " ,@RPTTYPE = 'M'"
        End If
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        Prop_Sets()
        dtGridView = New DataTable
        dsGridView = New DataSet
        gridView.DataSource = Nothing
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dtGridView = dsGridView.Tables(0)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found")
            lblTitle.Text = ""
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        GridViewFormat()
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPENING", GetType(String))
            .Columns.Add("RECEIPT", GetType(String))
            .Columns.Add("ISSUE", GetType(String))
            .Columns.Add("CLOSING", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = dtMergeHeader
        funcGridHeaderStyle()
        pnlHeading.Visible = True
        Dim title As String = Nothing
        If rbtSummary.Checked = True Then
            title += " SUMMARY WISE"
        ElseIf rbtMonthWise.Checked = True Then
            title += " MONTH WISE"
        ElseIf rbtDateWise.Checked = True Then
            title += " DATE WISE"
        Else
            title += " TRANNO WISE"
        End If
        title += " STOCK CLOSING REPORT "
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridHead
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = GetServerDate()
        dtpFrom.Focus()
        lblTitle.Text = ""
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .Cells("PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULARS").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
        With gridView
            .Columns("METALNAME").Visible = False
            .Columns("CATEGORY").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            'If .Columns.Contains("TRANNO") Then
            '    .Columns("TRANNO").Frozen = True
            'Else
            '    .Columns("PARTICULARS").Frozen = True
            'End If
            If ChkPcs.Checked = False Then
                .Columns("OPCS").Visible = False
                .Columns("RPCS").Visible = False
                .Columns("IPCS").Visible = False
                .Columns("CPCS").Visible = False
            End If
            If ChkGrsWt.Checked = False Then
                .Columns("OGRSWT").Visible = False
                .Columns("RGRSWT").Visible = False
                .Columns("IGRSWT").Visible = False
                .Columns("CGRSWT").Visible = False
            End If
            If .Columns.Contains("MM") Then .Columns("MM").Visible = False
            If .Columns.Contains("MID") Then .Columns("MID").Visible = False
            If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = False
            With .Columns("PARTICULARS")
                .HeaderText = "PARTICULAR"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OPCS")
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RPCS")
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IPCS")
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CPCS")
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OGRSWT")
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RGRSWT")
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGRSWT")
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGRSWT")
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ONETWT")
                .Visible = False
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RNETWT")
                .Visible = False
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("INETWT")
                .Visible = False
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CNETWT")
                .Visible = False
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OAMOUNT")
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RAMOUNT")
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IAMOUNT")
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CAMOUNT")
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If .Columns.Contains("RATE") Then
                With .Columns("RATE")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Function funcGridHeaderStyle() As Integer
        With gridHead
            With .Columns("PARTICULAR")
                If gridView.Columns.Contains("TRANNO") Then
                    .Width = gridView.Columns("PARTICULARS").Width + gridView.Columns("TRANNO").Width
                Else
                    .Width = gridView.Columns("PARTICULARS").Width
                End If
                .HeaderText = ""
            End With
            With .Columns("OPENING")
                .Width = IIf(ChkPcs.Checked, gridView.Columns("OPCS").Width, 0) + IIf(ChkGrsWt.Checked, gridView.Columns("OGRSWT").Width, 0) + gridView.Columns("OAMOUNT").Width
            End With
            With .Columns("RECEIPT")
                .Width = IIf(ChkPcs.Checked, gridView.Columns("RPCS").Width, 0) + IIf(ChkGrsWt.Checked, gridView.Columns("RGRSWT").Width, 0) + gridView.Columns("RAMOUNT").Width
            End With
            With .Columns("ISSUE")
                .Width = IIf(ChkPcs.Checked, gridView.Columns("IPCS").Width, 0) + IIf(ChkGrsWt.Checked, gridView.Columns("IGRSWT").Width, 0) + gridView.Columns("IAMOUNT").Width
            End With
            With .Columns("CLOSING")
                If gridView.Columns.Contains("RATE") Then
                    .Width = IIf(ChkPcs.Checked, gridView.Columns("CPCS").Width, 0) + IIf(ChkGrsWt.Checked, gridView.Columns("CGRSWT").Width, 0) + gridView.Columns("CAMOUNT").Width + gridView.Columns("RATE").Width
                Else
                    .Width = IIf(ChkPcs.Checked, gridView.Columns("CPCS").Width, 0) + IIf(ChkGrsWt.Checked, gridView.Columns("CGRSWT").Width, 0) + gridView.Columns("CAMOUNT").Width
                End If
            End With
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If gridView.RowCount > 0 Then
            If e.KeyChar = Chr(Keys.Enter) Then
                Dim rwIndex As Integer
                If gridView.RowCount = 1 Then
                    rwIndex = gridView.CurrentRow.Index
                Else
                    rwIndex = gridView.CurrentRow.Index - 1
                End If
                gridView.CurrentCell = gridView.Rows(rwIndex).Cells("OPENING")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                
            End If
            If UCase(e.KeyChar) = "X" Then
                btnExcel_Click(Me, New EventArgs)
            End If
            If UCase(e.KeyChar) = "P" Then
                btnPrint_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pnlHeading.Size = New System.Drawing.Size(100, 21)

        cmbCatName.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbCatName, False, False)

        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)

        cmbMetalType.Items.Add("ALL")
        cmbMetalType.Items.Add("ORNAMENT")
        cmbMetalType.Items.Add("METAL")
        cmbMetalType.Items.Add("STONE")

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        objGPack.FillCombo(strSql, CmbCompany, True)
        CmbCompany.Text = "ALL"
        'dtCompany = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtCompany)
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, e)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmClosingStock_Properties
        'GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbMetalType = cmbMetalType.Text
        obj.p_cmbCatName = cmbCatName.Text
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtMonthWise = rbtMonthWise.Checked
        obj.p_rbtDateWise = rbtDateWise.Checked
        obj.p_rbtTranNoWise = rbtTranNoWise.Checked
        obj.p_rbtWeighAvg = RbtWeigAvg.Checked
        obj.p_rbtLifo = RbtLifo.Checked
        obj.p_chkWithGrsWt = ChkGrsWt.Checked
        obj.p_chkWithPcs = ChkPcs.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmClosingStock_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmClosingStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmClosingStock_Properties))
        'SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbMetalType.Text = obj.p_cmbMetalType
        cmbCatName.Text = obj.p_cmbCatName
        rbtSummary.Checked = obj.p_rbtSummary
        rbtMonthWise.Checked = obj.p_rbtMonthWise
        rbtDateWise.Checked = obj.p_rbtDateWise
        rbtTranNoWise.Checked = obj.p_rbtTranNoWise
        RbtWeigAvg.Checked = obj.p_rbtWeighAvg
        RbtLifo.Checked = obj.p_rbtLifo
        ChkGrsWt.Checked = obj.p_chkWithGrsWt
        ChkPcs.Checked = obj.p_chkWithPcs
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
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

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridHead Is Nothing Then Exit Sub
        If Not gridHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridHead.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class

Public Class frmClosingStock_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property

    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property

    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property

    Private cmbMetalType As String = "ALL"
    Public Property p_cmbMetalType() As String
        Get
            Return cmbMetalType
        End Get
        Set(ByVal value As String)
            cmbMetalType = value
        End Set
    End Property
    Private rbtWeigh As Boolean = True
    Public Property p_rbtWeighAvg() As Boolean
        Get
            Return rbtWeigh
        End Get
        Set(ByVal value As Boolean)
            rbtWeigh = value
        End Set
    End Property

    Private rbtLIFO As Boolean = False
    Public Property p_rbtLifo() As Boolean
        Get
            Return rbtLIFO
        End Get
        Set(ByVal value As Boolean)
            rbtLIFO = value
        End Set
    End Property

    Private cmbCatName As String = "ALL"
    Public Property p_cmbCatName() As String
        Get
            Return cmbCatName
        End Get
        Set(ByVal value As String)
            cmbCatName = value
        End Set
    End Property

    Private rbtGeneral As Boolean = True
    Public Property p_rbtGeneral() As Boolean
        Get
            Return rbtGeneral
        End Get
        Set(ByVal value As Boolean)
            rbtGeneral = value
        End Set
    End Property

    Private rbtGram As Boolean = False
    Public Property p_rbtGram() As Boolean
        Get
            Return rbtGram
        End Get
        Set(ByVal value As Boolean)
            rbtGram = value
        End Set
    End Property

    Private rbtCarat As Boolean = False
    Public Property p_rbtCarat() As Boolean
        Get
            Return rbtCarat
        End Get
        Set(ByVal value As Boolean)
            rbtCarat = value
        End Set
    End Property

    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property

    Private rbtMonthWise As Boolean = False
    Public Property p_rbtMonthWise() As Boolean
        Get
            Return rbtMonthWise
        End Get
        Set(ByVal value As Boolean)
            rbtMonthWise = value
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

    Private rbtTranNoWise As Boolean = False
    Public Property p_rbtTranNoWise() As Boolean
        Get
            Return rbtTranNoWise
        End Get
        Set(ByVal value As Boolean)
            rbtTranNoWise = value
        End Set
    End Property

    Private chkPcs As Boolean = False
    Public Property p_chkWithPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property

    Private chkGrsWt As Boolean = False
    Public Property p_chkWithGrsWt() As Boolean
        Get
            Return chkGrsWt

        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
End Class
