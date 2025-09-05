Imports System.Data.OleDb
Public Class frmSalesAbsWithReturn
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
    Dim RPT_SALESABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SALESABS", "N") = "Y", True, False)

    Private Sub frmCategoryStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub NewReport()
        strSql = " EXEC " & cnAdminDb & "..RPT_SALESABS"
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@STRCOMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        Prop_Sets()
        dtGridView = New DataTable
        dsGridView = New DataSet
        gridView.DataSource = Nothing
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dtGridView = dsGridView.Tables(0)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            lblTitle.Visible = False
            dtpFrom.Focus()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        With gridView
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("BILLTYPE").Visible = False
            .Columns("TRANDATE1").Visible = False
            .Columns("KEYNO").Visible = False
        End With
        FormatGridColumns(gridView, False)
        FillGridGroupStyle_KeyNoWise(gridView, "TRANDATE")
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
        Dim title As String = Nothing
        title += " SALES ABSTRACT REPORT "
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
        lblTitle.Visible = True
        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Me.Cursor = Cursors.WaitCursor
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        Me.Refresh()
        'Dim selCatCode As String = Nothing
        'If cmbMetal.Text <> "" Then
        '    Dim sql As String
        '    If cmbMetal.Text = "ALL" Then
        '        sql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY"
        '    Else
        '        sql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN(" & GetQryString(cmbMetal.Text) & "))"
        '    End If
        '    Dim dtCat As New DataTable()
        '    da = New OleDbDataAdapter(sql, cn)
        '    da.Fill(dtCat)
        '    If dtCat.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtCat.Rows.Count - 1
        '            selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
        '        Next
        '        If selCatCode <> "" Then
        '            selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
        '        End If
        '    End If
        'End If
        If RPT_SALESABS Then
            NewReport()
            Exit Sub
        End If
        strSql = " EXEC " & cnStockDb & "..SP_RPT_ABSTRACT_WITHSR_NEW"
        strSql += vbCrLf + " @SYSTEMID = '" & IIf(systemId = "", userId, systemId) & "'"
        strSql += vbCrLf + " ,@WITHSR = 'Y'"
        strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@RPTTYPE = 'SA'"
        strSql += vbCrLf + " ,@METALNAME ='ALL'"
        strSql += vbCrLf + " ,@CATCODE = 'ALL'"
        strSql += vbCrLf + " ,@CATNAME = 'ALL'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@VA = 'Y'"
        strSql += vbCrLf + " ,@CHITDBID = '" & cnChitCompanyid & "'"
        strSql += vbCrLf + " ,@PURTYPE = ''"
        strSql += vbCrLf + " ,@WITSTN = 'N'"
        strSql += vbCrLf + " ,@PUREMC = 'N'"
        Prop_Sets()
        dtGridView = New DataTable
        dsGridView = New DataSet
        gridView.DataSource = Nothing
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dtGridView = dsGridView.Tables(0)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            lblTitle.Visible = False
            dtpFrom.Focus()
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        GridViewFormat()
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
        Dim title As String = Nothing
        title += " SALES ABSTRACT REPORT WITH PAYMENT DETAIL"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
        lblTitle.Visible = True

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).Visible = False
        Next
        GridViewFormat()
        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = GetServerDate()
        dtpFrom.Focus()
        lblTitle.Visible = False
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
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
        With gridView
            .Columns("PARTICULAR").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("SRAMOUNT").Visible = True
            .Columns("TAX").Visible = True
            .Columns("ADVANCEREC").Visible = True
            .Columns("DUERECEIPT").Visible = True
            .Columns("TOTAL").Visible = True
            .Columns("CASH").Visible = True
            .Columns("CREDIT").Visible = True
            .Columns("CREDITCARD").Visible = True
            .Columns("CHEQUE").Visible = True
            .Columns("ADVANCE").Visible = True
            .Columns("CHIT").Visible = True
            .Columns("GIFT").Visible = True
            .Columns("OG").Visible = True
            .Columns("OTHERS").Visible = True
            With .Columns("PARTICULAR")
                .HeaderText = "PARTICULAR"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTAL")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAX")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SRAMOUNT")
                .HeaderText = "SALESRETURN"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CASH")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CREDIT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CREDITCARD")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHEQUE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OTHERS")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADVANCE")
                .HeaderText = "ADVANCEADJ"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADVANCEREC")
                .HeaderText = "ADVANCEREC"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DUERECEIPT")
                .HeaderText = "DUERECEIPT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHIT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GIFT")
                .HeaderText = "GIFTVOUCHER"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OG")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If gridView.RowCount > 0 Then
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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        InitializeComponent()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
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
        Dim obj As New frmStockRegister_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        'obj.p_cmbMetal = cmbMetal.Text
        SetSettingsObj(obj, Me.Name, GetType(frmStockRegister_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmStockRegister_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmStockRegister_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        'cmbMetal.Text = obj.p_cmbMetal
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
End Class

Public Class frmSalesAbsWithReturn_Properties
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
    Private rbtNetWt As Boolean = True
    Public Property p_rbtNetWt() As Boolean
        Get
            Return rbtNetWt
        End Get
        Set(ByVal value As Boolean)
            rbtNetWt = value
        End Set
    End Property

    Private rbtGrsWt As Boolean = False
    Public Property p_rbtGrsWt() As Boolean
        Get
            Return rbtGrsWt
        End Get
        Set(ByVal value As Boolean)
            rbtGrsWt = value
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
