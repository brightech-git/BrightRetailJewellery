Imports System.Data.OleDb
Imports System.Xml
Public Class frmDesignerWiseStock
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
    Dim dttranyear As New DataTable
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
            .Columns("PARTICULAR").Width = 200
            ''HEADER TEXT
            .Columns("OPPCS").HeaderText = "PCS"
            .Columns("OPGRS").HeaderText = "GRS WT"
            .Columns("OPNET").HeaderText = "NET WT"
            .Columns("OPDIAPCS").HeaderText = "DIA PCS"
            .Columns("OPDIAWT").HeaderText = "DIA WT"
            .Columns("OPSTNPCS").HeaderText = "STN PCS"
            .Columns("OPSTNWT").HeaderText = "STN WT"
            .Columns("REPCS").HeaderText = "PCS"
            .Columns("REGRS").HeaderText = "GRS WT"
            .Columns("RENET").HeaderText = "NET WT"
            .Columns("REDIAPCS").HeaderText = "DIA PCS"
            .Columns("REDIAWT").HeaderText = "DIA WT"
            .Columns("RESTNPCS").HeaderText = "STN PCS"
            .Columns("RESTNWT").HeaderText = "STN WT"
            .Columns("ISPCS").HeaderText = "PCS"
            .Columns("ISGRS").HeaderText = "GRS WT"
            .Columns("ISNET").HeaderText = "NET WT"
            .Columns("ISDIAPCS").HeaderText = "DIA PCS"
            .Columns("ISDIAWT").HeaderText = "DIA WT"
            .Columns("ISSTNPCS").HeaderText = "STN PCS"
            .Columns("ISSTNWT").HeaderText = "STN WT"
            .Columns("CLPCS").HeaderText = "PCS"
            .Columns("CLGRS").HeaderText = "GRS WT"
            .Columns("CLNET").HeaderText = "NET WT"
            .Columns("CLDIAPCS").HeaderText = "DIA PCS"
            .Columns("CLDIAWT").HeaderText = "DIA WT"
            .Columns("CLSTNPCS").HeaderText = "STN PCS"
            .Columns("CLSTNWT").HeaderText = "STN WT"
            .Columns("OPPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPGRS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPNET").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("ISPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISGRS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISNET").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPGRS").Visible = chkGrsWt.Checked
            .Columns("OPNET").Visible = chkNetWt.Checked
            .Columns("OPDIAPCS").Visible = chkDiamond.Checked
            .Columns("OPDIAWT").Visible = chkDiamond.Checked
            .Columns("OPSTNPCS").Visible = chkStone.Checked
            .Columns("OPSTNWT").Visible = chkStone.Checked
            .Columns("ISGRS").Visible = chkGrsWt.Checked
            .Columns("ISNET").Visible = chkNetWt.Checked
            .Columns("ISDIAPCS").Visible = chkDiamond.Checked
            .Columns("ISDIAWT").Visible = chkDiamond.Checked
            .Columns("ISSTNPCS").Visible = chkStone.Checked
            .Columns("ISSTNWT").Visible = chkStone.Checked
            .Columns("REGRS").Visible = chkGrsWt.Checked
            .Columns("RENET").Visible = chkNetWt.Checked
            .Columns("REDIAPCS").Visible = chkDiamond.Checked
            .Columns("REDIAWT").Visible = chkDiamond.Checked
            .Columns("RESTNPCS").Visible = chkStone.Checked
            .Columns("RESTNWT").Visible = chkStone.Checked
            .Columns("CLGRS").Visible = chkGrsWt.Checked
            .Columns("CLNET").Visible = chkNetWt.Checked
            .Columns("CLDIAPCS").Visible = chkDiamond.Checked
            .Columns("CLDIAWT").Visible = chkDiamond.Checked
            .Columns("CLSTNPCS").Visible = chkStone.Checked
            .Columns("CLSTNWT").Visible = chkStone.Checked


            ''VISIBLE
            'gridView.Columns("WEIGHT").Visible = chkwithweightdetails.Checked
            gridView.Columns("KEYNO").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("ITEM").Visible = False
            gridView.Columns("SNAME").Visible = False
            gridView.Columns("PARTICULAR").Visible = True
        End With
    End Sub

    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Private Sub Report()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkCmbCompany.Text = "" Then MsgBox("Company Should not Empty...", MsgBoxStyle.Information) : chkCmbCompany.Focus() : Exit Sub
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        Dim otherdb As String = Nothing
        If chkcmbFinancialYear.Text = "ALL" Then
            For Each ro As DataRow In _DtTransactionYear.Rows
                If cnStockDb <> ro.Item("DBNAME").ToString Then
                    otherdb = otherdb + ro.Item("DBNAME").ToString + ","
                End If
            Next
        Else
            Dim TEMP() As String = chkcmbFinancialYear.Text.Split(",")
            For i As Integer = 0 To TEMP.GetUpperBound(0)
                For Each ro As DataRow In _DtTransactionYear.Rows
                    If TEMP(i) = ro.Item("TRANYEAR").ToString And cnStockDb <> ro.Item("DBNAME").ToString Then
                        otherdb = otherdb + ro.Item("DBNAME").ToString + ","
                    End If
                Next
            Next
        End If
        strSql = " Exec " & cnAdminDb & "..SP_DESIGNERWISESTOCK "
        strSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @METAL ='" & chkCmbMetal.Text & "', "
        strSql += vbCrLf + " @DESIGNER='" & chkCmbDesigner.Text & "', "
        strSql += vbCrLf + " @COSTCENTRE ='" & chkCmbCostCentre.Text & "',"
        strSql += vbCrLf + " @STOCKDB ='" & cnStockDb & "',"
        strSql += vbCrLf + " @OTHERDB ='" & otherdb & "',"
        strSql += vbCrLf + " @WITHSUB ='" & IIf(chkWithSubItem.Checked = True, "Y", "N") & "',"
        strSql += vbCrLf + " @COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        'strSql += vbCrLf + " @AMETAL ='" & chkCmbALLOY.Text & "', "
        'strSql += vbCrLf + " @TRANTYPE ='" & IIf(rbtReceipt.Checked = True, "R", "I") & "'"
        '        tabView.Show()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        strSql = "SELECT * FROM " & cnAdminDb & "..TEMPDESIGNERSTOCK ORDER BY ITEM,SNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtSource)
        If Not dtSource.Rows.Count > 0 Then
            MessageBox.Show("Record Not Found")
            Exit Sub
        End If

        'gridView.DataSource = dtSource
        tabView.Show()
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        'If rbtCategorywise.Checked = True Then ObjGrouper.pColumns_Group.Add("CATNAME")
        ObjGrouper.pColumns_Group.Add("DESIGNER")
        If chkWithSubItem.Checked = True Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPPCS")
        ObjGrouper.pColumns_Sum.Add("OPGRS")
        ObjGrouper.pColumns_Sum.Add("OPNET")
        ObjGrouper.pColumns_Sum.Add("OPDIAPCS")
        ObjGrouper.pColumns_Sum.Add("OPDIAWT")
        ObjGrouper.pColumns_Sum.Add("OPSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OPSTNWT")
        ObjGrouper.pColumns_Sum.Add("REPCS")
        ObjGrouper.pColumns_Sum.Add("REGRS")
        ObjGrouper.pColumns_Sum.Add("RENET")
        ObjGrouper.pColumns_Sum.Add("REDIAPCS")
        ObjGrouper.pColumns_Sum.Add("REDIAWT")
        ObjGrouper.pColumns_Sum.Add("RESTNPCS")
        ObjGrouper.pColumns_Sum.Add("RESTNWT")
        ObjGrouper.pColumns_Sum.Add("ISPCS")
        ObjGrouper.pColumns_Sum.Add("ISGRS")
        ObjGrouper.pColumns_Sum.Add("ISNET")
        ObjGrouper.pColumns_Sum.Add("ISDIAPCS")
        ObjGrouper.pColumns_Sum.Add("ISDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISSTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISSTNWT")
        ObjGrouper.pColumns_Sum.Add("CLPCS")
        ObjGrouper.pColumns_Sum.Add("CLGRS")
        ObjGrouper.pColumns_Sum.Add("CLNET")
        ObjGrouper.pColumns_Sum.Add("CLDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CLDIAWT")
        ObjGrouper.pColumns_Sum.Add("CLSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CLSTNWT")
        'If rbtCategorywise.Checked = True Then
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "ITEM"
        'End If
        ObjGrouper.GroupDgv()
        GridStyle()
        GridViewHeaderStyle()
        tabMain.SelectedTab = tabView
        Dim title As String = Nothing
        title += " DESIGNER WISE RECEIPT/ISSUE"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
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
                        .Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
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
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkcmbFinancialYear, dttranyear, "TRANYEAR", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")

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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))

   
        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

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
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' TRANYEAR,null enddate,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " SUBSTRING(CONVERT(VARCHAR,STARTDATE,102),1,4)+'-'"
        strSql += " +SUBSTRING(CONVERT(VARCHAR,ENDDATE,102),3,2)+ ' ('+DBNAME+')' AS TRANYEAR,enddate,2 RESULT"
        strSql += " FROM " & cnAdminDb & "..DBMASTER"
        strSql += " ORDER BY RESULT,enddate"
        dttranyear = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dttranyear)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dttranyear, "tranyear", , "ALL")

        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
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
        Dim obj As New frmDesignerWiseStock_Properties
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDESIGNER)
        GetChecked_CheckedList(chkcmbFinancialYear, obj.p_chkCmbTRANYEAR)
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkWithSubItem = chkWithSubItem.Checked
        obj.p_chkDiamond = chkDiamond.Checked
        obj.p_chkStone = chkStone.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmDesignerWiseStock_Properties))

    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmDesignerWiseStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmDesignerWiseStock_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDESIGNER, "ALL")
        SetChecked_CheckedList(chkcmbFinancialYear, obj.p_chkCmbTRANYEAR, "ALL")
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkWithSubItem.Checked = obj.p_chkWithSubItem
        chkDiamond.Checked = obj.p_chkDiamond
        chkStone.Checked = obj.p_chkStone

    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If rbtReceipt.Checked = True Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If rbtIssue.Checked = False Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub tabGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGen.Click

    End Sub

    Private Sub GridViewHeaderStyle()

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT", GetType(String))
            .Columns.Add("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT", GetType(String))
            .Columns.Add("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT", GetType(String))
            .Columns.Add("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Caption = "OPENING"
            .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Caption = "RECEIPT"
            .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Caption = "ISSUE"
            .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub
    Function funcColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Width = gridView.Columns("OPPCS").Width
            If chkGrsWt.Checked Then .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Width += gridView.Columns("OPGRS").Width
            If chkNetWt.Checked Then .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Width += gridView.Columns("OPNET").Width
            .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Width += IIf(gridView.Columns("OPDIAWT").Visible, gridView.Columns("OPDIAPCS").Width, 0) + IIf(gridView.Columns("OPDIAWT").Visible, gridView.Columns("OPDIAWT").Width, 0)
            .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").Width += IIf(gridView.Columns("OPSTNWT").Visible, gridView.Columns("OPSTNPCS").Width, 0) + IIf(gridView.Columns("OPSTNWT").Visible, gridView.Columns("OPSTNWT").Width, 0)
            .Columns("OPPCS~OPGRS~OPNET~OPDIAPCS~OPDIAWT~OPSTNPCS~OPSTNWT").HeaderText = "OPENING"

            .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Width = gridView.Columns("REPCS").Width
            If chkGrsWt.Checked Then .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Width += gridView.Columns("REGRS").Width
            If chkNetWt.Checked Then .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Width += gridView.Columns("RENET").Width
            .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Width += IIf(gridView.Columns("REDIAWT").Visible, gridView.Columns("REDIAPCS").Width, 0) + IIf(gridView.Columns("REDIAWT").Visible, gridView.Columns("REDIAWT").Width, 0)
            .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").Width += IIf(gridView.Columns("RESTNWT").Visible, gridView.Columns("RESTNPCS").Width, 0) + IIf(gridView.Columns("RESTNWT").Visible, gridView.Columns("RESTNWT").Width, 0)
            .Columns("REPCS~REGRS~RENET~REDIAPCS~REDIAWT~RESTNPCS~RESTNWT").HeaderText = "RECEIPT"

            .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Width = gridView.Columns("ISPCS").Width
            If chkGrsWt.Checked Then .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Width += gridView.Columns("ISGRS").Width
            If chkNetWt.Checked Then .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Width += gridView.Columns("ISNET").Width
            .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Width += IIf(gridView.Columns("ISDIAWT").Visible, gridView.Columns("ISDIAPCS").Width, 0) + IIf(gridView.Columns("ISDIAWT").Visible, gridView.Columns("ISDIAWT").Width, 0)
            .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").Width += IIf(gridView.Columns("ISSTNWT").Visible, gridView.Columns("ISSTNPCS").Width, 0) + IIf(gridView.Columns("ISSTNWT").Visible, gridView.Columns("ISSTNWT").Width, 0)
            .Columns("ISPCS~ISGRS~ISNET~ISDIAPCS~ISDIAWT~ISSTNPCS~ISSTNWT").HeaderText = "ISSUE"

            .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Width = gridView.Columns("CLPCS").Width
            If chkGrsWt.Checked Then .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Width += gridView.Columns("CLGRS").Width
            If chkNetWt.Checked Then .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Width += gridView.Columns("CLNET").Width
            .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Width += IIf(gridView.Columns("CLDIAWT").Visible, gridView.Columns("CLDIAPCS").Width, 0) + IIf(gridView.Columns("CLDIAWT").Visible, gridView.Columns("CLDIAWT").Width, 0)
            .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").Width += IIf(gridView.Columns("CLSTNWT").Visible, gridView.Columns("CLSTNPCS").Width, 0) + IIf(gridView.Columns("CLSTNWT").Visible, gridView.Columns("CLSTNWT").Width, 0)
            .Columns("CLPCS~CLGRS~CLNET~CLDIAPCS~CLDIAWT~CLSTNPCS~CLSTNWT").HeaderText = "CLOSING"

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function

    Private Sub gridView_ColumnWidthChanged_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub

    Private Sub gridView_KeyPress_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class

Public Class frmDesignerWiseStock_Properties
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property

    Private chkCmbDESIGNER As New List(Of String)
    Public Property p_chkCmbDESIGNER() As List(Of String)
        Get
            Return chkCmbDESIGNER
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDESIGNER = value
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

    Private chkCmbTRANYEAR As New List(Of String)
    Public Property p_chkCmbTRANYEAR() As List(Of String)
        Get
            Return chkCmbTRANYEAR
        End Get
        Set(ByVal value As List(Of String))
            chkCmbTRANYEAR = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property

    Private chkNetWt As Boolean = False
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property

    Private chkWithSubItem As Boolean = False
    Public Property p_chkWithSubItem() As Boolean
        Get
            Return chkWithSubItem
        End Get
        Set(ByVal value As Boolean)
            chkWithSubItem = value
        End Set
    End Property

    Private chkDiamond As Boolean = False
    Public Property p_chkDiamond() As Boolean
        Get
            Return chkDiamond
        End Get
        Set(ByVal value As Boolean)
            chkDiamond = value
        End Set
    End Property

    Private chkStone As Boolean = False
    Public Property p_chkStone() As Boolean
        Get
            Return chkStone
        End Get
        Set(ByVal value As Boolean)
            chkStone = value
        End Set
    End Property

End Class