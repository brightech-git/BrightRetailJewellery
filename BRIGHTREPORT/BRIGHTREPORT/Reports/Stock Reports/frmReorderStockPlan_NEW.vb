Imports System.Data.OleDb
Public Class frmReorderStockPlan_NEW
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
        StrSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
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
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
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
        If CmbItemName.Text <> "ALL" And Trim(CmbItemName.Text) <> "" Then
            strFilter += " and r.itemId = (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & CmbItemName.Text & "')"
        End If
        If cmbSubItemName.Text <> "ALL" And Trim(cmbSubItemName.Text) <> "" Then
            strFilter += " and r.subItemId = (Select subItemId from " & cnAdminDb & "..subitemMast where subitemName = '" & cmbSubItemName.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & CmbItemName.Text & "'))"
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
        StrSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_REORDERSTOCKPLAN_NEW"
        StrSql += vbCrLf + " @ITEMID = '" & selItemId & "'"
        StrSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        StrSql += vbCrLf + " ,@COSTID = '" & selCostId & "'"
        StrSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        StrSql += vbCrLf + " ,@TRANTYPE = '" & IIf(ChkIssueOnly.Checked, "I", "") & "'"
        'StrSql += vbCrLf + " ,@RTYPE = '" & rType & "'"
        Dim DtGrid As New DataTable
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("DESIGNERNAME~SEAL~ITEMNAME~SUBITEMNAME", GetType(String))
            .Columns.Add("RANGE", GetType(String))
            .Columns.Add("STKPCS~STKWT~STRPCS~STRWT~STIPCS~STIWT~TOTPCS~TOTWT", GetType(String))
            .Columns.Add("MINPCS", GetType(String))
            .Columns.Add("ORDPLACE", GetType(String))
            '.Columns.Add("REORDQTY", GetType(String))
            .Columns("DESIGNERNAME~SEAL~ITEMNAME~SUBITEMNAME").Caption = "DESCRIPTION"
            .Columns("RANGE").Caption = "WEIGHT RANGE"
            .Columns("STKPCS~STKWT~STRPCS~STRWT~STIPCS~STIWT~TOTPCS~TOTWT").Caption = "AVAILABLE STOCK"
            .Columns("MINPCS").Caption = "MINIMUM QTY"
            .Columns("ORDPLACE").Caption = "ORDER PLACED"
            '.Columns("REORDQTY").Caption = "REORDER QTY"
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
            Dim TEMPCOLWIDTH As Integer = 0


            TEMPCOLWIDTH += .Columns("DESIGNERNAME").Width + .Columns("SEAL").Width + .Columns("SUBITEMNAME").Width
            objGridShower.gridViewHeader.Columns("DESIGNERNAME~SEAL~ITEMNAME~SUBITEMNAME").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("DESIGNERNAME~SEAL~ITEMNAME~SUBITEMNAME").HeaderText = "DESCRIPTION"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("RANGE").Width
            objGridShower.gridViewHeader.Columns("RANGE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("RANGE").HeaderText = "WEIGHT RANGE"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("STKPCS").Width + .Columns("STKWT").Width + .Columns("STRPCS").Width + .Columns("STRWT").Width + .Columns("STIPCS").Width + .Columns("STIWT").Width + .Columns("TOTPCS").Width + .Columns("TOTWT").Width
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT~STRPCS~STRWT~STIPCS~STIWT~TOTPCS~TOTWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT~STRPCS~STRWT~STIPCS~STIWT~TOTPCS~TOTWT").HeaderText = "AVAILABLE STOCK"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("MINPCS").Width
            objGridShower.gridViewHeader.Columns("MINPCS").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("MINPCS").HeaderText = "MINIMUM QTY"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("ORDPLACE").Width
            objGridShower.gridViewHeader.Columns("ORDPLACE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("ORDPLACE").HeaderText = "ORDER PLACED"
            'TEMPCOLWIDTH = 0
            'TEMPCOLWIDTH += .Columns("REORDQTY").Width
            'objGridShower.gridViewHeader.Columns("REORDQTY").Width = TEMPCOLWIDTH
            'objGridShower.gridViewHeader.Columns("REORDQTY").HeaderText = "REORDER QTY"

            'Dim colWid As Integer = 0
            'For cnt As Integer = 0 To .ColumnCount - 1
            '    If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
            '    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
            '    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Weight"
            'Next

        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("DESIGNERNAME").Width = 120
            .Columns("SEAL").Width = 100
            .Columns("SUBITEMNAME").Width = 150
            .Columns("RANGE").Width = 150
            .Columns("STKPCS").Width = 60
            .Columns("STKWT").Width = 80
            .Columns("STRPCS").Width = 60
            .Columns("STRWT").Width = 80
            .Columns("STIPCS").Width = 60
            .Columns("STIWT").Width = 80
            .Columns("TOTPCS").Width = 60
            .Columns("TOTWT").Width = 80
            .Columns("MINPCS").Width = 80
            .Columns("ORDPLACE").Width = 80
            .Columns("REORDQTY").Width = 80

            .Columns("RANGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STRPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STRWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STIPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STIWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DESIGNERNAME").Visible = True
            .Columns("SEAL").Visible = True
            .Columns("SUBITEMNAME").Visible = True
            .Columns("RANGE").Visible = True
            .Columns("STKPCS").Visible = True
            .Columns("STKWT").Visible = True
            .Columns("STRPCS").Visible = True
            .Columns("STRWT").Visible = True
            .Columns("STIPCS").Visible = True
            .Columns("STIWT").Visible = True
            .Columns("TOTPCS").Visible = True
            .Columns("TOTWT").Visible = True
            .Columns("MINPCS").Visible = True
            .Columns("ORDPLACE").Visible = True
            .Columns("REORDQTY").Visible = False

            .Columns("SEAL").HeaderText = "DESIGNER CODE"
            .Columns("STKPCS").HeaderText = "OP_PCS"
            .Columns("STKWT").HeaderText = "OP_WT"
            .Columns("STRPCS").HeaderText = "REC_PCS"
            .Columns("STRWT").HeaderText = "REC_WT"
            .Columns("STIPCS").HeaderText = "ISS_PCS"
            .Columns("STIWT").HeaderText = "ISS_WT"
            .Columns("TOTPCS").HeaderText = "CL_PCS"
            .Columns("TOTWT").HeaderText = "CL_WT"
            .Columns("ORDPLACE").HeaderText = "PCS"
            .Columns("REORDQTY").HeaderText = "REORD_QTY"

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
            CmbItemName.Focus()
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
        dtpAsOnDate.Value = GetServerDate()
        Prop_Gets()
    End Sub

    Private Sub pnlView_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlView.Paint

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpAsOnDate.Focus()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmReorderStockPlan_NEW_Properties
        obj.p_cmbItemName = CmbItemName.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        SetSettingsObj(obj, Me.Name, GetType(frmReorderStockPlan_NEW_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmReorderStockPlan_NEW_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmReorderStockPlan_NEW_Properties))
        CmbItemName.Text = obj.p_cmbItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        'cmbCostCentre.Text = obj.p_cmbCostCentre

    End Sub

    Private Sub cmbMetal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.TextChanged
        funcLoadItemName()
    End Sub
    Function funcLoadItemName() As Integer
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            StrSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & "))"
        End If
        StrSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
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

Public Class frmReorderStockPlan_NEW_Properties
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