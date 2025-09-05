Imports System.Data.OleDb
Imports System.Xml
Public Class frmwastagereport
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

    
    Private Sub GridStyle()
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("TRANNO").Width = 75
            .Columns("TRANDATE").Width = 75
            .Columns("METAL").Width = 100
            .Columns("PARTY").Width = 400
            .Columns("WASTAGE").Width = 150
            .Columns("WEIGHT").Width = 150
            ''HEADER TEXT

            .Columns("TRANNO").HeaderText = "TRAN NO"
            .Columns("TRANDATE").HeaderText = "TRAN DATE"
            .Columns("METAL").HeaderText = "METAL"
            .Columns("PARTY").HeaderText = "PARTY"
            .Columns("WASTAGE").HeaderText = "WASTAGE"
            .Columns("WEIGHT").HeaderText = "WEIGHT"

            FillGridGroupStyle_KeyNoWise(gridView, "METAL")

            
            ''VISIBLE
            gridView.Columns("WEIGHT").Visible = chkwithweightdetails.Checked
            gridView.Columns("KEYNO").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("RESULT").Visible = False
        End With
    End Sub

    Private Sub Report()

        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        gridView.DataSource = Nothing

        Dim mtype As String
        If rbtBoth.Checked = True Then
            mtype = "B"
        ElseIf rbtOrnament.Checked = True Then
            mtype = "O"
        Else
            mtype = "M"
        End If
        strSql = " Exec " & cnStockDb & "..Sp_RPT_WASTAGEDETAILS "
        strSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @METAL ='" & chkCmbMetal.Text & "', "
        strSql += vbCrLf + " @TRANTYPE ='" & IIf(rbtReceipt.Checked = True, "R", "I") & "',"
        strSql += vbCrLf + " @MTYPE ='" & mtype & "',"
        strSql += vbCrLf + " @COSTCENTRE ='" & chkCmbCostCentre.Text & "',"
        strSql += vbCrLf + " @DPUR ='" & IIf(chkDirectpurchase.Checked = True, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        tabView.Show()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        strSql = "SELECT * FROM " + cnStockDb + "..TEMPWASTAGEDETS"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        If dtSource.Rows.Count <= 1 Then
            MessageBox.Show("Record Not Found")
            Exit Sub
        End If
        gridView.DataSource = dtSource
        GridStyle()
        tabMain.SelectedTab = tabView
        Dim title As String = Nothing
        If rbtReceipt.Checked = True Then
            title += " RECEIPT WISE"
        Else
            title += " ISSUE WISE"
        End If
        title += " WASTAGE REPORT BASED ON"
        If rbtOrnament.Checked = True Then
            title += " ORNAMENT"
        ElseIf rbtMetal.Checked = True Then
            title += " METAL"
        Else
            title += " ORNAMENT AND METAL"
        End If
        title += " ("
        If chkDirectpurchase.Checked = True Then
            If rbtReceipt.Checked = True Then
                title += " DIRECT PURCHASE"
            Else
                title += " DIRECT SALES"
            End If
        End If
        If chkwithweightdetails.Checked = True Then
            title += " WEIGHT DETAILS"
        End If
        title += ")"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title.Replace(" ()", "")
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
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        Prop_Gets()
        dtpFrom.Focus()
        dtpFrom.Select()
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
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & CNADMINDB & "..METALMAST "
        strSql += " WHERE TTYPE='M'"
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & CNADMINDB & "..COSTCENTRE"
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
        Dim obj As New frmWastageReport_Properties
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_rbtOrnament = rbtOrnament.Checked
        obj.p_rbtMetal = rbtMetal.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_chkDirectpurchase = chkDirectpurchase.Checked
        obj.p_chkwithweightdetails = chkwithweightdetails.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmWastageReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmWastageReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmWastageReport_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtIssue.Checked = obj.p_rbtIssue
        rbtOrnament.Checked = obj.p_rbtOrnament
        rbtMetal.Checked = obj.p_rbtMetal
        rbtBoth.Checked = obj.p_rbtBoth
        chkDirectpurchase.Checked = obj.p_chkDirectpurchase
        chkwithweightdetails.Checked = obj.p_chkwithweightdetails
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        'If rbtReceipt.Checked = True Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        'If rbtIssue.Checked = False Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub
End Class



Public Class frmWastageReport_Properties
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
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

    Private rbtOrnament As Boolean = True
    Public Property p_rbtOrnament() As Boolean
        Get
            Return rbtOrnament
        End Get
        Set(ByVal value As Boolean)
            rbtOrnament = value
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

    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
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

    Private chkDirectpurchase As Boolean = True
    Public Property p_chkDirectpurchase() As Boolean
        Get
            Return chkDirectpurchase
        End Get
        Set(ByVal value As Boolean)
            chkDirectpurchase = value
        End Set
    End Property

    Private chkwithweightdetails As Boolean = True
    Public Property p_chkwithweightdetails() As Boolean
        Get
            Return chkwithweightdetails
        End Get
        Set(ByVal value As Boolean)
            chkwithweightdetails = value
        End Set
    End Property

End Class