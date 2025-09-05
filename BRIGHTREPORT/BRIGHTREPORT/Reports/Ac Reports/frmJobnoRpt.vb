Imports System.Data.OleDb
Imports System.IO

Public Class frmJobnoRpt
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        Prop_Gets()
        dtpFrom.Focus()
    End Function

    Private Sub PurchaseAbs()

        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        gridView.Refresh()
        gridViewHead.Refresh()
        strSql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & CmbPCategory.Text & "'"
        Dim Catcode As String = objGPack.GetSqlValue(strSql, "CATCODE", "").ToString
        strSql = " EXEC " & cnAdminDb & "..RPT_JOBNOWISE"
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@PURCATCODE='" & Catcode & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPRECIPT1 ORDER BY RESULT,TRANDATE"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        With gridView
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
        Dim TITLE As String = ""
        For i As Integer = 0 To gridView.ColumnCount - 1
            With gridView
                If .Columns(i).HeaderText.Contains("AA_") Then .Columns(i).HeaderText = .Columns(i).HeaderText.Replace("AA_", "")
                If .Columns(i).HeaderText.Contains("BB_") Then .Columns(i).HeaderText = .Columns(i).HeaderText.Replace("BB_", "")
                If .Columns(i).HeaderText.Contains("CC_") Then .Columns(i).HeaderText = .Columns(i).HeaderText.Replace("CC_", "")
            End With
        Next
        'FormatGridColumns(gridView, False, False)
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView, "JOBNO")
        With gridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        funcHeaderNew()
        TITLE += " JOBNO WISE REPORT "
        TITLE += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        TITLE += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()
    End Sub
    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Dim ColName As String = ""
        Dim ColName1 As String = ""
        Dim ColWidth As Integer = 0
        Dim ColWidth1 As Integer = 0
        Try
            strSql = "SELECT DISTINCT TRANTYPE,CATNAME FROM TEMPTABLEDB..TEMPRECIPT WHERE TRANTYPE IN('AA','BB','CC') "
            strSql += "ORDER BY TRANTYPE,CATNAME"
            Dim dthead As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dthead)

            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("TRANDATE~JOBNO~CATNAME", GetType(String))
                For i As Integer = 0 To dthead.Rows.Count - 1
                    If dthead.Rows(i).Item("TRANTYPE") = "CC" Then Continue For
                    ColName = ColName & dthead.Rows(i).Item("TRANTYPE").ToString & "_" & dthead.Rows(i).Item("CATNAME").ToString & "~"
                    ColWidth += gridView.Columns(dthead.Rows(i).Item("TRANTYPE").ToString & "_" & dthead.Rows(i).Item("CATNAME").ToString).Width
                Next
                If ColName <> "" Then
                    ColName = Mid(ColName, 1, Len(ColName) - 1)
                    .Columns.Add(ColName, GetType(String))
                End If
                .Columns.Add("RECDATE~CATNAME", GetType(String))
                For i As Integer = 0 To dthead.Rows.Count - 1
                    If dthead.Rows(i).Item("TRANTYPE") = "CC" Then
                        ColName1 = ColName1 & dthead.Rows(i).Item("TRANTYPE").ToString & "_" & dthead.Rows(i).Item("CATNAME").ToString & "~"
                        ColWidth1 += gridView.Columns(dthead.Rows(i).Item("TRANTYPE").ToString & "_" & dthead.Rows(i).Item("CATNAME").ToString).Width
                    End If
                Next
                If ColName1 <> "" Then
                    ColName1 = Mid(ColName1, 1, Len(ColName1) - 1)
                    .Columns.Add(ColName1, GetType(String))
                End If
                .Columns.Add("SCROLL", GetType(String))
            End With
            With gridViewHead
                .DataSource = dtMergeHeader
                .Columns("TRANDATE~JOBNO~CATNAME").HeaderText = ""
                If ColName <> "" Then
                    .Columns(ColName).HeaderText = "PURCHASE-GS11-24KT"
                End If
                .Columns("RECDATE~CATNAME").HeaderText = ""
                If ColName1 <> "" Then
                    .Columns(ColName1).HeaderText = "MANUFACTURING-GS11-24KT"
                End If
                gridViewHead.Refresh()
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                .Columns("TRANDATE~JOBNO~CATNAME").Width = gridView.Columns("TRANDATE").Width + gridView.Columns("JOBNO").Width
                If ColName <> "" Then
                    .Columns(ColName).Width = ColWidth
                End If
                .Columns("RECDATE~CATNAME").Width = gridView.Columns("RECDATE").Width
                If ColName1 <> "" Then
                    .Columns(ColName1).Width = ColWidth1 + gridView.Columns("COPPER").Width
                End If
                With .Columns("SCROLL")
                    .HeaderText = ""
                    .Width = 0
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                gridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                .Columns("SCROLL").Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        PurchaseAbs()
        Prop_Sets()
    End Sub

    Private Sub frmPurchaseAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Function FillAcname()
        strSql = " SELECT 'ALL' ACNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE"
        strSql += "  ISNULL(ACTIVE,'Y') <>'N' "
        strSql += "  AND ACTYPE IN ('D','G','I','O')"
        strSql += " ORDER BY RESULT,ACNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtAcname)
        CmbAcname.Items.Clear()
        BrighttechPack.FillCombo(CmbAcname, DtAcname, "ACNAME", True)
    End Function
    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        FillAcname()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += "  WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='M')"
        strSql += " ORDER BY CATNAME"
        Dim dtCat As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCat)
        CmbPCategory.Items.Clear()
        BrighttechPack.FillCombo(CmbPCategory, dtCat, "CATNAME", True)

        btnNew_Click(Me, New EventArgs)
        Prop_Gets()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead, , , , , True)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmJobnoRpt_Properties
        obj.p_catCode = CmbPCategory.Text
        SetSettingsObj(obj, Me.Name, GetType(frmJobnoRpt_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmJobnoRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmJobnoRpt_Properties))
        CmbPCategory.Text = obj.p_catCode
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
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
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridViewHead.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridViewHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class

Public Class frmJobnoRpt_Properties
    Private catCode As String
    Public Property p_catCode() As String
        Get
            Return catCode
        End Get
        Set(ByVal value As String)
            catCode = value
        End Set
    End Property

    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
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
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
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
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
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
    Private rbtAll As Boolean = True
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property
    Private rbtVat As Boolean = False
    Public Property p_rbtVat() As Boolean
        Get
            Return rbtVat
        End Get
        Set(ByVal value As Boolean)
            rbtVat = value
        End Set
    End Property
    Private rbtWVat As Boolean = False
    Public Property p_rbtWVat() As Boolean
        Get
            Return rbtWVat
        End Get
        Set(ByVal value As Boolean)
            rbtWVat = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
    Private rbtTAll As Boolean = True
    Public Property p_rbtTAll() As Boolean
        Get
            Return rbtTAll
        End Get
        Set(ByVal value As Boolean)
            rbtTAll = value
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
    Private rbtSalesReturn As Boolean = False
    Public Property p_rbtSalesReturn() As Boolean
        Get
            Return rbtSalesReturn
        End Get
        Set(ByVal value As Boolean)
            rbtSalesReturn = value
        End Set
    End Property
    Private rbtPU As Boolean = False
    Public Property p_rbtPU() As Boolean
        Get
            Return rbtPU
        End Get
        Set(ByVal value As Boolean)
            rbtPU = value
        End Set
    End Property
End Class