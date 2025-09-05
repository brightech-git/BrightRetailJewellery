Imports System.Data.OleDb
Public Class frmReorderStockPlan
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strFilter As String = Nothing

    Dim dtItem As New DataTable
    Dim dtSubItem As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtSize As New DataTable

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        pnlView.Dock = DockStyle.Fill
        pnlView.Visible = False
        grpControls.BringToFront()

        ''COSTCENTRE
        StrSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL'COSTID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & CNADMINDB & "..COSTCENTRE"
        StrSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCostCentre)
        cmbCostCentre.Items.Clear()
        For Each Row As DataRow In dtCostCentre.Rows
            cmbCostCentre.Items.Add(Row.Item("COSTNAME").ToString)
        Next
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        Else
            cmbCostCentre.Enabled = False
        End If

        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmReorderStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.WindowState = FormWindowState.Maximized
        StrSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        StrSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        Dim dtMetal As New DataTable()
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")
        btnNew_Click(Me, e)
    End Sub

    Function funcFiltration() As String
        strFilter = "where 1=1 "
        If cmbItemName.Text <> "ALL" And Trim(cmbItemName.Text) <> "" Then
            strFilter += " and r.itemId = (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItemName.Text & "')"
        End If
        If cmbSubItemName.Text <> "ALL" And Trim(cmbSubItemName.Text) <> "" Then
            strFilter += " and r.subItemId = (Select subItemId from " & cnAdminDb & "..subitemMast where subitemName = '" & cmbSubItemName.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'))"
        End If
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text <> "ALL" And Trim(cmbCostCentre.Text) = "" Then
                strFilter += " and r.COSTID = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "')"
            End If
        End If
        Return strFilter
    End Function

    Private Sub NewReport()
        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        Dim selCostId As String = Nothing
        Dim selSizeid As String = Nothing
        Dim rType As String = Nothing
        Dim selMetalId As String = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If CmbItemName.Text = "ALL" Then
            selItemId = "ALL"
        ElseIf CmbItemName.Text <> "" Then
            Dim sql As String = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(CmbItemName.Text) & ")"
            Dim dtItem As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtItem.Rows.Count - 1
                    selItemId += dtItem.Rows(i).Item("ITEMID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selItemId <> "" Then
                    selItemId = Mid(selItemId, 1, selItemId.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        If cmbSubItemName.SelectedIndex >= 0 Then
            selSubItemId = dtSubItem.Rows(cmbSubItemName.SelectedIndex).Item("SUBITEMID").ToString
        Else
            selSubItemId = "ALL"
        End If
        If cmbCostCentre.SelectedIndex >= 0 Then
            selCostId = dtCostCentre.Rows(cmbCostCentre.SelectedIndex).Item("COSTID").ToString
        Else
            selCostId = "ALL"
        End If
        rType = "B"
        StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_STOCKREORDERNEW_R"
        StrSql += vbCrLf + " @ITEMID = '" & selItemId & "'"
        StrSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        StrSql += vbCrLf + " ,@COSTID = '" & selCostId & "'"
        StrSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        'StrSql += vbCrLf + " ,@RTYPE = '" & rType & "'"
        Dim DtGrid As New DataTable
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("DESIGNERNAME~ITEMNAME~SUBITEMNAME", GetType(String))
            .Columns.Add("RANGE", GetType(String))
            .Columns.Add("STKPCS~STKWT", GetType(String))
            .Columns.Add("REORDPCS~REORDWT", GetType(String))
            ' .Columns.Add("STRPCS~STRWT", GetType(String))
            .Columns.Add("TOTPCS~TOTWT", GetType(String))
            '.Columns.Add("LEADTIME", GetType(String))
            .Columns.Add("MINPCS", GetType(String))
            '.Columns.Add("DIFFPCS~DIFFWT", GetType(String))
            .Columns("DESIGNERNAME~ITEMNAME~SUBITEMNAME").Caption = "DESCRIPTION"
            .Columns("RANGE").Caption = "WEIGHT RANGE"
            .Columns("STKPCS~STKWT").Caption = "AVAILABLE STOCK"
            .Columns("REORDPCS~REORDWT").Caption = "SHOWROOM"
            '.Columns("STRPCS~STRWT").Caption = "STRONG ROOM"
            .Columns("TOTPCS~TOTWT").Caption = "TOTAL"
            '.Columns("LEADTIME").Caption = "LEADTIME"
            .Columns("MINPCS").Caption = "REORDER LEVEL"
            '.Columns("DIFFPCS~DIFFWT").Caption = "T P O"
        End With


        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "STOCK REORDER"
        Dim tit As String = "REORDER LEVEL STOCK REPORT" + vbCrLf
        tit += " AS ON " & dtpAsOnDate.Text & ""
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(1)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("SUBITEMNAME")))
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        objGridShower.gridViewHeader.Visible = True
        GridHead()
    End Sub

    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            'objGridShower.gridViewHeader.Columns("COL1").Width = .Columns("COL1").Width
            'objGridShower.gridViewHeader.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0


            TEMPCOLWIDTH += .Columns("DESIGNERNAME").Width + .Columns("SUBITEMNAME").Width
            objGridShower.gridViewHeader.Columns("DESIGNERNAME~ITEMNAME~SUBITEMNAME").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("DESIGNERNAME~ITEMNAME~SUBITEMNAME").HeaderText = "DESCRIPTION"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("RANGE").Width
            objGridShower.gridViewHeader.Columns("RANGE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("RANGE").HeaderText = "WEIGHT RANGE"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("STKPCS").Width + .Columns("STKWT").Width
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT").HeaderText = "AVAILABLE STOCK"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("REORDPCS").Width + .Columns("REORDWT").Width
            objGridShower.gridViewHeader.Columns("REORDPCS~REORDWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("REORDPCS~REORDWT").HeaderText = "SHOWROOM"
            'TEMPCOLWIDTH = 0
            'TEMPCOLWIDTH += .Columns("STRPCS").Width + .Columns("STRWT").Width
            'objGridShower.gridViewHeader.Columns("STRPCS~STRWT").Width = TEMPCOLWIDTH
            'objGridShower.gridViewHeader.Columns("STRPCS~STRWT").HeaderText = "STRONG ROOM"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("TOTPCS").Width + .Columns("TOTWT").Width
            objGridShower.gridViewHeader.Columns("TOTPCS~TOTWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("TOTPCS~TOTWT").HeaderText = "TOTAL"
            'TEMPCOLWIDTH = 0
            'TEMPCOLWIDTH += .Columns("LEADTIME").Width
            'objGridShower.gridViewHeader.Columns("LEADTIME").Width = TEMPCOLWIDTH
            'objGridShower.gridViewHeader.Columns("LEADTIME").HeaderText = "LEADTIME"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("MINPCS").Width
            objGridShower.gridViewHeader.Columns("MINPCS").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("MINPCS").HeaderText = "REORDER LEVEL"
            'TEMPCOLWIDTH = 0
            'TEMPCOLWIDTH += .Columns("DIFFPCS").Width + .Columns("DIFFWT").Width
            'objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT").Width = TEMPCOLWIDTH
            'objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT").HeaderText = "TO BE REQ."


            Dim colWid As Integer = 0
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Weight"
            Next
            
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("DESIGNERNAME").Width = 200
            .Columns("SUBITEMNAME").Width = 300
            .Columns("RANGE").Width = 200
            .Columns("STKPCS").Width = 100
            .Columns("STKWT").Width = 100
            .Columns("REORDPCS").Width = 100 ' = .Columns("REORDPCS").HeaderText = "PCS"
            .Columns("REORDWT").Width = 100 '= .Columns("REORDWT").HeaderText = "WT"
            '.Columns("DIFFPCS").Width = 100
            .Columns("MINPCS").Width = 100
            .Columns("STRPCS").Width = 100
            .Columns("STRWT").Width = 100
            .Columns("TOTWT").Width = 100
            .Columns("TOTPCS").Width = 100
            .Columns("LEADTIME").Width = 100
            '.Columns("DIFFWT").Width = 100
            .Columns("RANGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REORDPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REORDWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("DIFFWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DESIGNERNAME").Visible = True
            .Columns("SUBITEMNAME").Visible = True
            .Columns("RANGE").Visible = True
            .Columns("STKPCS").Visible = True
            .Columns("STKWT").Visible = True
            .Columns("REORDPCS").Visible = True
            .Columns("REORDWT").Visible = True
            .Columns("STRPCS").Visible = False
            .Columns("STRWT").Visible = False
            .Columns("TOTPCS").Visible = True
            .Columns("TOTWT").Visible = True
            .Columns("LEADTIME").Visible = False
            .Columns("DIFFPCS").Visible = False
            .Columns("MINPCS").Visible = True
            .Columns("DIFFWT").Visible = False

            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub btnView_SearchClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        NewReport()
        Exit Sub
    End Sub

    Private Sub frmReorderStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub


    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            pnlView.Visible = False
            grpControls.Visible = True
            grpControls.BringToFront()
            cmbItemName.Focus()
        End If
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbItemName.SelectedIndexChanged
        'StrSql = " select subItemName from " & cnAdminDb & "..subItemMast where itemId = "
        'StrSql += " (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItemName.Text & "')"
        'StrSql += " order by SubItemName"
        'cmbSubItemName.Items.Add("ALL")
        'objGPack.FillCombo(StrSql, cmbSubItemName, False)
        'cmbSubItemName.Text = "ALL"
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
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

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If (cmbCostCentre.Enabled = True) Then cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        ' cmbSize.Enabled = False
        ' cmbItemName.Text = "ALL"
        ' cmbSubItemName.Text = "ALL"
        ' cmbSize.Text = "ALL"
        dtpAsOnDate.Value = GetServerDate()
        'rbtStock.Checked = True
        ' rbtBoth.Checked = True
        Prop_Gets()
    End Sub

    Private Sub pnlView_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlView.Paint

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpAsOnDate.Focus()
    End Sub

    


    Private Sub Prop_Sets()
        Dim obj As New frmReorderStockPLAN_Properties
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        SetSettingsObj(obj, Me.Name, GetType(frmReorderStockPLAN_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmReorderStockPLAN_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmReorderStockPLAN_Properties))
        cmbItemName.Text = obj.p_cmbItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        'cmbCostCentre.Text = obj.p_cmbCostCentre

    End Sub

    Private Sub cmbMetal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.TextChanged
        funcLoadItemName()
    End Sub
    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            StrSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(CmbItemName, dtItem, "ITEMNAME", , "ALL")
    End Function

    Private Sub CmbItemName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbItemName.TextChanged
        cmbSubItemName.Items.Clear()
        StrSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME,'ALL'SUBITEMID,1 RESULT,0 DISPLAYORDER"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT,DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST"
        StrSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & CmbItemName.Text & "')"
        If _SubItemOrderByName Then
            StrSql += vbCrLf + " ORDER BY RESULT,SUBITEMNAME"
        Else
            StrSql += vbCrLf + " ORDER BY RESULT,DISPLAYORDER,SUBITEMNAME"
        End If

        da = New OleDbDataAdapter(StrSql, cn)
        dtSubItem = New DataTable
        da.Fill(dtSubItem)
        For Each ro As DataRow In dtSubItem.Rows
            cmbSubItemName.Items.Add(ro.Item("SUBITEMNAME").ToString)
        Next
    End Sub
End Class

Public Class frmReorderStockPLAN_Properties
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbSubItemName As String = "ALL"
    Public Property p_cmbSubItemName() As String
        Get
            Return cmbSubItemName
        End Get
        Set(ByVal value As String)
            cmbSubItemName = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private chkSize As Boolean = False
    Public Property p_chkSize() As Boolean
        Get
            Return chkSize
        End Get
        Set(ByVal value As Boolean)
            chkSize = value
        End Set
    End Property
    Private cmbSize As String = "ALL"
    Public Property p_cmbSize() As String
        Get
            Return cmbSize
        End Get
        Set(ByVal value As String)
            cmbSize = value
        End Set
    End Property
    Private rbtStock As Boolean = True
    Public Property p_rbtStock() As Boolean
        Get
            Return rbtStock
        End Get
        Set(ByVal value As Boolean)
            rbtStock = value
        End Set
    End Property
    Private rbtSales As Boolean = False
    Public Property p_rbtSales() As Boolean
        Get
            Return rbtSales
        End Get
        Set(ByVal value As Boolean)
            rbtSales = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtShort As Boolean = False
    Public Property p_rbtShort() As Boolean
        Get
            Return rbtShort
        End Get
        Set(ByVal value As Boolean)
            rbtShort = value
        End Set
    End Property
    Private rbtExcess As Boolean = False
    Public Property p_rbtExcess() As Boolean
        Get
            Return rbtExcess
        End Get
        Set(ByVal value As Boolean)
            rbtExcess = value
        End Set
    End Property
    Private chkDetailedOthers As Boolean = False
    Public Property p_chkDetailedOthers() As Boolean
        Get
            Return chkDetailedOthers
        End Get
        Set(ByVal value As Boolean)
            chkDetailedOthers = value
        End Set
    End Property
End Class