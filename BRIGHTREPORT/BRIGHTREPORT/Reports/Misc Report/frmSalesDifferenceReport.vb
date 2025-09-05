Imports System.Data.OleDb
Imports System.Xml
Public Class frmSalesDifferenceReport
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Dim SelectedCompany As String

    Dim dtMetal As New DataTable
    Dim dtALLOY As New DataTable
    Dim dtCounter As New DataTable
    Dim dtItemType As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim HideSummary As Boolean = IIf(GetAdmindbSoftValue("HIDE-STOCKSUMMARY", "N") = "Y", True, False)


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        tabMain.SelectedTab = tabGen
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function

    'Function funcLoadItemName() As Integer
    '    strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
    '    strSql += " UNION ALL"
    '    strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
    '    strSql += " WHERE ACTIVE = 'Y'"
    '    If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
    '        strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
    '    End If
    '    strSql += " ORDER BY RESULT,ITEMNAME"
    '    dtItem = New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtItem)
    '    BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    'End Function

    'Function funcLoadMetal() As Integer
    '    cmbMetal.Items.Clear()
    '    cmbMetal.Items.Add("ALL")
    '    strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
    '    objGPack.FillCombo(strSql, cmbMetal, False, False)
    '    cmbMetal.Text = "ALL"
    'End Function

    Function funcGridViewStyle() As Integer
        With gridView
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("itemid").Visible = False
            .Columns("subitemid").Visible = False
            With .Columns("itemName")
                .HeaderText = "ITEM"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("subItemName")
                .Visible = False
                .HeaderText = "SUBITEM"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("designerName")
                .HeaderText = "DesignerName"
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CostName")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Counter")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function

    'Function funcEmptyItemFiltration() As String
    '    Dim str As String = Nothing
    '    str = " "
    '    If chkAll.Checked = False Then
    '        str = " Having not (sum(isnull(t.Pcs,0)) = 0 and sum(isnull(t.GrsWt,0)) = 0 and sum(isnull(t.NetWt,0))=0)"
    '    End If
    '    Return str
    'End Function

    Private Sub GridStyle()
        ''
        ' FillGridGroupStyle_KeyNoWise(gridView)

        'For Each dgvRow As DataGridViewRow In gridView.Rows
        '    If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
        '        dgvRow.Cells("PARTICULAR").Style.BackColor = Color.LightBlue
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    End If
        'Next
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("BILLNO").Width = 75
            .Columns("BILLNO1").Visible = False
            .Columns("BILLDATE").Width = 75
            .Columns("PRODUCTNAME").Width = 120
            If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Width = 100
            .Columns("TAGVALUE").Width = 100
            .Columns("SALVALUE").Width = 100
            .Columns("DIFFERENCE").Width = 100
            If .Columns.Contains("DIFFERENCEP") Then .Columns("DIFFERENCEP").Width = 100
            ''HEADER TEXT

            .Columns("BILLNO").HeaderText = "BILL NO"
            .Columns("BILLDATE").HeaderText = "BILL DATE"
            .Columns("PRODUCTNAME").HeaderText = "PRODUCT NAME"
            If .Columns.Contains("TAGNO") Then .Columns("TAGNO").HeaderText = "TAG NO"
            .Columns("TAGVALUE").HeaderText = "TAG VALUE"
            .Columns("SALVALUE").HeaderText = "SALE VALUE"
            .Columns("DIFFERENCE").HeaderText = "DIFFERENCE"
            If .Columns.Contains("DIFFERENCEP") Then .Columns("DIFFERENCEP").HeaderText = "DIFF %"

            If gridView.Columns.Contains("TAGWT") Then gridView.Columns("TAGWT").HeaderText = "TAG WT"
            If gridView.Columns.Contains("WASTAGE") Then gridView.Columns("WASTAGE").HeaderText = "TAG WAST"
            If gridView.Columns.Contains("SALWT") Then gridView.Columns("SALWT").HeaderText = "SALE WT"
            If gridView.Columns.Contains("SALWASTAGE") Then gridView.Columns("SALWASTAGE").HeaderText = "SALE WAST"

            If chkGrpCategory.Checked = False Then
                If gridView.Columns.Contains("TAGTOT") Then gridView.Columns("TAGTOT").Visible = False
                If gridView.Columns.Contains("TAGWT") Then gridView.Columns("TAGWT").Visible = False
                If gridView.Columns.Contains("WASTAGE") Then gridView.Columns("WASTAGE").Visible = False
                If gridView.Columns.Contains("SALWASTAGE") Then gridView.Columns("SALWASTAGE").Visible = False
            Else
                If gridView.Columns.Contains("ITEMID") Then gridView.Columns("ITEMID").Visible = False
                If gridView.Columns.Contains("TAGNO") Then gridView.Columns("TAGNO").Visible = False
                If gridView.Columns.Contains("BILLDATE") Then gridView.Columns("BILLDATE").Visible = False
                If rbtSummary.Checked Then
                    If gridView.Columns.Contains("BILLNO") Then gridView.Columns("BILLNO").Visible = False
                    If gridView.Columns.Contains("DISCPERGM") Then gridView.Columns("DISCPERGM").Visible = False
                    If gridView.Columns.Contains("DIFFPERGM") Then gridView.Columns("DIFFPERGM").Visible = False
                    If gridView.Columns.Contains("DIFFERENCEP") Then gridView.Columns("DIFFERENCEP").Visible = False
                Else
                    If gridView.Columns.Contains("DISCPERGM") Then gridView.Columns("DISCPERGM").Visible = False
                    If gridView.Columns.Contains("DIFFPERGM") Then gridView.Columns("DIFFPERGM").Visible = False
                    If gridView.Columns.Contains("DIFFERENCEP") Then gridView.Columns("DIFFERENCEP").Visible = False
                End If
            End If
            .Columns("PRODUCTNAME").HeaderText = "PARTICULARS"

            'FillGridGroupStyle_KeyNoWise(gridView, "METAL")
            .Columns("TAGVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("TAGVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFERENCE").DefaultCellStyle.Format = "0.00"
            .Columns("DIFFERENCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("DIFFERENCEP") Then .Columns("DIFFERENCEP").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("DIFFERENCEP") Then .Columns("DIFFERENCEP").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            ''VISIBLE
            'gridView.Columns("WEIGHT").Visible = chkwithweightdetails.Checked
            gridView.Columns("KEYNO").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("RESULT").Visible = False
            If gridView.Columns.Contains("METALID") Then gridView.Columns("METALID").Visible = False
            If gridView.Columns.Contains("CATCODE") Then gridView.Columns("CATCODE").Visible = False
            If gridView.Columns.Contains("TRANTYPE") Then gridView.Columns("TRANTYPE").Visible = False
            If gridView.Columns.Contains("CHIT") Then gridView.Columns("CHIT").Visible = False
        End With
    End Sub

    
    Private Sub Report()

        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        gridView.DataSource = Nothing
        strSql = " Exec " & cnStockDb & "..SP_RPT_SAL_DIFFERENCE "
        strSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METAL ='" & chkCmbMetal.Text & "' "
        strSql += vbCrLf + " ,@COSTCENTRE ='" & IIf(chkCmbCostCentre.Text <> "", chkCmbCostCentre.Text, "ALL") & "'"
        strSql += vbCrLf + " ,@BILLNO ='" & Val(txtBillno.Text) & "'"
        strSql += vbCrLf + " ,@BILLTOTAL ='" & IIf(ChkBillWise.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@BILLGRP ='" & IIf(ChkGrpBillno.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@CATGRP ='" & IIf(chkGrpCategory.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SUMMARY ='" & IIf(rbtSummary.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        If chkGrpCategory.Checked Then
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPSALDIFF ORDER BY METALID,CATCODE,RESULT,BILLNO1  "
        Else
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPSALDIFF  ORDER BY BILLNO1,RESULT  "
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        If dtSource.Rows.Count < 1 Then
            MessageBox.Show("Record Not Found")
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
        gridView.DataSource = Nothing
        gridView.DataSource = dtSource
        GridStyle()
        GridViewFormat()


        Dim title As String = Nothing
        title += " SALES DIFFERENCE"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")

        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
        gridView.Columns("DIFFERENCE").Visible = True
        Prop_Sets()
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Report()
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PRODUCTNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PRODUCTNAME").Style.ForeColor = reportHeadStyle.ForeColor
                        .Cells("PRODUCTNAME").Style.Font = reportHeadStyle.Font
                        .Cells("BILLNO1").Value = ""
                    Case "H"
                        .Cells("PRODUCTNAME").Style.BackColor = Color.Ivory
                        .Cells("PRODUCTNAME").Style.ForeColor = Color.BlueViolet
                        .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                        .Cells("BILLNO1").Value = ""
                        Dim TEST As String = .Cells("SALWT").Value.ToString

                        .Cells("SALWT").Style.BackColor = Color.Ivory
                        .Cells("SALWT").Style.ForeColor = Color.Black
                        .Cells("PRODUCTNAME").Style.BackColor = Color.White


                        .Cells("SALWT").Style.BackColor = Color.Ivory
                        .Cells("SALWT").Style.ForeColor = Color.Black
                        .Cells("PRODUCTNAME").Style.BackColor = Color.White

                        'If .Cells("SALWT").Value.ToString = " REPAIR GOLD " Then
                        '    .Cells("SALWT").Style.BackColor = Color.Ivory
                        '    .Cells("SALWT").Style.ForeColor = Color.Black
                        '    .Cells("PRODUCTNAME").Style.BackColor = Color.White
                        'End If
                        'If .Cells("SALWT").Value.ToString = " REPAIR SILVER " Then
                        '    .Cells("SALWT").Style.BackColor = Color.Ivory
                        '    .Cells("SALWT").Style.ForeColor = Color.Black
                        '    .Cells("PRODUCTNAME").Style.BackColor = Color.White
                        'End If

                    Case "G"
                        .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                        .Cells("BILLNO1").Value = ""
                    Case "K"
                        .DefaultCellStyle.BackColor = Color.Ivory
                        .DefaultCellStyle.ForeColor = Color.BlueViolet
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .Cells("BILLNO1").Value = ""
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .Cells("BILLNO1").Value = ""
                    Case "Z"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .Cells("BILLNO1").Value = ""
                End Select
            End With
        Next
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'funcLoadMetal()

        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
        ' chkDiamond.Checked = False
        ' chkStone.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbALLOY, dtALLOY, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        dtpFrom.Focus()
        dtpFrom.Select()
        Prop_Gets()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, )
        End If
    End Sub

    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE TTYPE='M'"
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        'strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        'strSql += " UNION ALL"
        'strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        'strSql += " WHERE TTYPE='A'"
        'strSql += " ORDER BY RESULT,METALNAME"
        'dtALLOY = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtALLOY)
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbALLOY, dtALLOY, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
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

    Private Sub chkCmbMetal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.TextChanged
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmALLOYREPORT_Properties
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        'GetChecked_CheckedList(chkCmbALLOY, obj.p_chkCmbALLOY)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtTranDate = rbtTranDate.Checked
        obj.p_rbtCategorywise = rbtCategorywise.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtIssue = rbtIssue.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmALLOYREPORT_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmALLOYREPORT_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmALLOYREPORT_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtTranDate.Checked = obj.p_rbtTranDate
        rbtCategorywise.Checked = obj.p_rbtCategorywise
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtIssue.Checked = obj.p_rbtIssue
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtTranDate.CheckedChanged
        'If rbtReceipt.Checked = True Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtCategorywise.CheckedChanged
        'If rbtIssue.Checked = False Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub ChkGrpBillno_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkGrpBillno.CheckedChanged
        If ChkGrpBillno.Checked = True Then
            ChkBillWise.Checked = False
            ChkBillWise.Enabled = False
            chkGrpCategory.Checked = False
            pnlCat.Enabled = False
        Else
            ChkBillWise.Enabled = True
        End If
    End Sub

    Private Sub ChkBillWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkBillWise.CheckedChanged
        If ChkBillWise.Checked Then
            chkGrpCategory.Checked = False
            ChkGrpBillno.Checked = False
            pnlCat.Enabled = False
        End If
    End Sub

    Private Sub chkGrpCategory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGrpCategory.CheckedChanged
        If chkGrpCategory.Checked Then
            ChkBillWise.Checked = False
            ChkGrpBillno.Checked = False
            pnlCat.Enabled = True
        Else
            pnlCat.Enabled = False
        End If
    End Sub
End Class

Public Class frmSalesDifferenceReport_Properties
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property

    Private chkCmbALLOY As New List(Of String)
    Public Property p_chkCmbALLOY() As List(Of String)
        Get
            Return chkCmbALLOY
        End Get
        Set(ByVal value As List(Of String))
            chkCmbALLOY = value
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

    Private rbtTranDate As Boolean = True
    Public Property p_rbtTranDate() As Boolean
        Get
            Return rbtTranDate
        End Get
        Set(ByVal value As Boolean)
            rbtTranDate = value
        End Set
    End Property

    Private rbtCategorywise As Boolean = True
    Public Property p_rbtCategorywise() As Boolean
        Get
            Return rbtCategorywise
        End Get
        Set(ByVal value As Boolean)
            rbtCategorywise = value
        End Set
    End Property
    Private chkCategory As Boolean = True
    Public Property p_chkCategory() As Boolean
        Get
            Return chkCategory
        End Get
        Set(ByVal value As Boolean)
            chkCategory = value
        End Set
    End Property
    Private chkDiscount As Boolean = True
    Public Property p_chkDiscount() As Boolean
        Get
            Return chkDiscount
        End Get
        Set(ByVal value As Boolean)
            chkDiscount = value
        End Set
    End Property

    Private rbtReceipt As Boolean = True
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property

    Private rbtIssue As Boolean = True
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
        End Set
    End Property

    Private chkGrpCategory As Boolean = True
    Public Property p_chkGrpCategory() As Boolean
        Get
            Return chkGrpCategory
        End Get
        Set(ByVal value As Boolean)
            chkGrpCategory = value
        End Set
    End Property

End Class