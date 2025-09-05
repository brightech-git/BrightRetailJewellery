Imports System.Data.OleDb
Imports System.IO

Public Class frmSalesPurAbs
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

        Dim Type As String = "A"
        Dim BillType As String = "A"
        If rbtAll.Checked Then Type = "A"
        If rbtVat.Checked Then Type = "Y"
        If rbtWoVat.Checked Then Type = "N"

        If rbtTAll.Checked Then BillType = "A"
        If rbtSales.Checked Then BillType = "S"
        If rbtSR.Checked Then BillType = "R"
        If rbtPurchase.Checked Then BillType = "P"
        Dim GstFlag As Boolean = False
        GstFlag = funcGstView(dtpFrom.Value)
        strSql = " EXEC " & cnAdminDb & "..RPT_SALESREPORT"
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@TAX='" & Type & "'"
        strSql += vbCrLf + " ,@BILLTYPE='" & BillType & "'"
        strSql += vbCrLf + " ,@GST='" & IIf(GstFlag, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPSASRPU1 ORDER BY CATNAME,RESULT "
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
        FormatGridColumns(gridView, False, False, True, False)

        Dim dt As New DataTable
        dt = gridView.DataSource
        dt.AcceptChanges()
        Dim ros() As DataRow = Nothing


        ros = dt.Select("COLHEAD = 'G'")
        'For cnt As Integer = 0 To ros.Length - 1
        '    gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
        'Next
        'With gridViewHead
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        '    .Invalidate()
        'End With
        With gridView
            .Columns("CATNAME").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TRANDATE1").Visible = False
            .Columns("TRANNO1").Visible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        funcHeaderNew()
        FormatGridColumns(gridView, False, False)
        FillGridGroupStyle_KeyNoWise(gridView, "TRANDATE")
        Dim TITLE As String = ""


        With gridView
            .Columns("G_GRSWT").HeaderText = "GRSWT"
            .Columns("G_LESSWT").HeaderText = "LESSWT"
            .Columns("G_NETWT").HeaderText = "NETWT"

            .Columns("S_GRSWT").HeaderText = "GRSWT"
            .Columns("S_LESSWT").HeaderText = "LESSWT"
            .Columns("S_NETWT").HeaderText = "NETWT"

            .Columns("P_GRSWT").HeaderText = "GRSWT"
            .Columns("P_LESSWT").HeaderText = "LESSWT"
            .Columns("P_NETWT").HeaderText = "NETWT"

            .Columns("SRWT").HeaderText = "GRSWT"
            .Columns("SRLESSWT").HeaderText = "LESSWT"
            .Columns("SRNETWT").HeaderText = "NETWT"

            .Columns("OGWT").HeaderText = "GRSWT"
            .Columns("OGLESSWT").HeaderText = "LESSWT"
            .Columns("OGNETWT").HeaderText = "NETWT"
        End With
        TITLE += " SALES REPORT "
        TITLE += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        TITLE += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()



        'Dim dtGridView As New DataTable
        'dsGridView = New DataSet
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.CommandTimeout = 1000
        'da = New OleDbDataAdapter(cmd)
        'da.Fill(dsGridView)
        'dtGridView = dsGridView.Tables(0)

        'If Not dtGridView.Rows.Count > 0 Then
        '    MsgBox("Record Not Found", MsgBoxStyle.Information)
        '    lblTitle.Text = ""
        '    Exit Sub
        'End If

        ''With gridView
        ''    .Columns("COSTID").Visible = False
        ''End With

        'pnlHeading.Visible = True
        'gridView.DefaultCellStyle = Nothing
        'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        'gridView.DataSource = dtGridView
        'funcHeaderNew()
        'FormatGridColumns(gridView, False, False)
        'FillGridGroupStyle_KeyNoWise(gridView, "TRANDATE")
        'GridViewFormat()
        'Dim title As String = Nothing
        'title += " SALES REPORT "
        'title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        'title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        'lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'lblTitle.Text = title
    End Sub
    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()

        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("PARTICULARS~TRANDATE~TYPE~TRANNO~TITLE~INITIAL~NAME~MOBILE~HSN", GetType(String))
                .Columns.Add("G_GRSWT~G_LESSWT~G_NETWT", GetType(String))
                .Columns.Add("S_GRSWT~S_LESSWT~S_NETWT", GetType(String))
                .Columns.Add("P_GRSWT~P_LESSWT~P_NETWT", GetType(String))
                .Columns.Add("STNAMT~SALESAMOUNT", GetType(String))
                .Columns.Add("IGST~CGST~SGST~VAT", GetType(String))
                .Columns.Add("DISCOUNT~SALESNETAMT", GetType(String))
                .Columns.Add("OGTYPE~OGNO~OGWT~OGLESSWT~OGNETWT~OGAMT~OGDISC", GetType(String))
                .Columns.Add("SRTYPE~SRNO~SRWT~SRLESSWT~SRNETWT~SRAMT~SRIGST~SRCGST~SRSGST~SRVAT~SRDISC~SRNETAMT", GetType(String))
                .Columns.Add("ADVANCE~ADVANCEADJ", GetType(String))
                .Columns.Add("NETAMT~CASH~CARD~CHIT~CHEQUE~BALANCE", GetType(String))
                .Columns.Add("SCROLL", GetType(String))

                .Columns("PARTICULARS~TRANDATE~TYPE~TRANNO~TITLE~INITIAL~NAME~MOBILE~HSN").Caption = "PARTICULAR"
                .Columns("G_GRSWT~G_LESSWT~G_NETWT").Caption = "GOLDWT"
                .Columns("S_GRSWT~S_LESSWT~S_NETWT").Caption = "SILVER WEIGHT"
                .Columns("P_GRSWT~P_LESSWT~P_NETWT").Caption = "PLATINUM WEIGHT"
                .Columns("STNAMT~SALESAMOUNT").Caption = ""
                .Columns("IGST~CGST~SGST~VAT").Caption = "GST"
                .Columns("DISCOUNT~SALESNETAMT").Caption = "SALE AMOUNT"
                .Columns("OGTYPE~OGNO~OGWT~OGLESSWT~OGNETWT~OGAMT~OGDISC").Caption = "PURCHASE"
                .Columns("SRTYPE~SRNO~SRWT~SRLESSWT~SRNETWT~SRAMT~SRIGST~SRCGST~SRSGST~SRVAT~SRDISC~SRNETAMT").Caption = "SALES RETURN"
                .Columns("ADVANCE~ADVANCEADJ").Caption = "ADVANCE"
                .Columns("NETAMT~CASH~CARD~CHIT~CHEQUE~BALANCE").Caption = "PAYMODE"

            End With
            With gridViewHead
                .DataSource = dtMergeHeader

                .Columns("PARTICULARS~TRANDATE~TYPE~TRANNO~TITLE~INITIAL~NAME~MOBILE~HSN").HeaderText = "PARTICULAR"
                .Columns("G_GRSWT~G_LESSWT~G_NETWT").HeaderText = "GOLDWT"
                .Columns("S_GRSWT~S_LESSWT~S_NETWT").HeaderText = "SILVER WEIGHT"
                .Columns("P_GRSWT~P_LESSWT~P_NETWT").HeaderText = "PLATINUM WEIGHT"
                .Columns("STNAMT~SALESAMOUNT").HeaderText = ""
                .Columns("IGST~CGST~SGST~VAT").HeaderText = "GST"
                .Columns("DISCOUNT~SALESNETAMT").HeaderText = "SALE AMOUNT"
                .Columns("OGTYPE~OGNO~OGWT~OGLESSWT~OGNETWT~OGAMT~OGDISC").HeaderText = "PURCHASE"
                .Columns("SRTYPE~SRNO~SRWT~SRLESSWT~SRNETWT~SRAMT~SRIGST~SRCGST~SRSGST~SRVAT~SRDISC~SRNETAMT").HeaderText = "SALES RETURN"
                .Columns("ADVANCE~ADVANCEADJ").HeaderText = "ADVANCE"
                .Columns("NETAMT~CASH~CARD~CHIT~CHEQUE~BALANCE").HeaderText = "PAYMODE"
                gridViewHead.Refresh()
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                funcColWidth()
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

    Function funcColWidth() As Integer
        With gridViewHead

            Dim ColWidth As Integer
            Dim ColWidth1 As Integer

            .Columns("PARTICULARS~TRANDATE~TYPE~TRANNO~TITLE~INITIAL~NAME~MOBILE~HSN").Width = gridView.Columns("PARTICULARS").Width _
                + gridView.Columns("TRANDATE").Width _
                + gridView.Columns("TYPE").Width + gridView.Columns("TRANNO").Width + gridView.Columns("NAME").Width + gridView.Columns("MOBILE").Width _
                + gridView.Columns("HSN").Width


            .Columns("G_GRSWT~G_LESSWT~G_NETWT").Width = gridView.Columns("G_GRSWT").Width _
                + gridView.Columns("G_LESSWT").Width + gridView.Columns("G_NETWT").Width

            .Columns("S_GRSWT~S_LESSWT~S_NETWT").Width = gridView.Columns("S_GRSWT").Width _
                + gridView.Columns("S_LESSWT").Width + gridView.Columns("S_NETWT").Width

            .Columns("P_GRSWT~P_LESSWT~P_NETWT").Width = gridView.Columns("P_GRSWT").Width _
                + gridView.Columns("P_LESSWT").Width + gridView.Columns("P_NETWT").Width

            .Columns("STNAMT~SALESAMOUNT").Width = gridView.Columns("STNAMT").Width + gridView.Columns("SALESAMOUNT").Width

            If .Columns.Contains("IGST~CGST~SGST~VAT") Then
                .Columns("IGST~CGST~SGST~VAT").Width = gridView.Columns("IGST").Width _
                    + gridView.Columns("CGST").Width + gridView.Columns("SGST").Width + gridView.Columns("VAT").Width
            End If

            .Columns("DISCOUNT~SALESNETAMT").Width = gridView.Columns("DISCOUNT").Width + gridView.Columns("SALESNETAMT").Width

            .Columns("OGTYPE~OGNO~OGWT~OGLESSWT~OGNETWT~OGAMT~OGDISC").Width = gridView.Columns("OGTYPE").Width _
                + gridView.Columns("OGNO").Width + gridView.Columns("OGWT").Width + gridView.Columns("OGLESSWT").Width _
            + gridView.Columns("OGNETWT").Width + gridView.Columns("OGAMT").Width + gridView.Columns("OGDISC").Width

            .Columns("SRTYPE~SRNO~SRWT~SRLESSWT~SRNETWT~SRAMT~SRIGST~SRCGST~SRSGST~SRVAT~SRDISC~SRNETAMT").Width = gridView.Columns("SRTYPE").Width _
                + gridView.Columns("SRNO").Width + gridView.Columns("SRWT").Width + gridView.Columns("SRLESSWT").Width _
            + gridView.Columns("SRNETWT").Width + gridView.Columns("SRAMT").Width + gridView.Columns("SRIGST").Width _
               + gridView.Columns("SRCGST").Width + gridView.Columns("SRSGST").Width + gridView.Columns("SRVAT").Width _
            + gridView.Columns("SRDISC").Width + gridView.Columns("SRNETAMT").Width

            .Columns("ADVANCE~ADVANCEADJ").Width = gridView.Columns("ADVANCE").Width + gridView.Columns("ADVANCEADJ").Width

            .Columns("NETAMT~CASH~CARD~CHIT~CHEQUE~BALANCE").Width = gridView.Columns("NETAMT").Width _
                + gridView.Columns("CASH").Width + gridView.Columns("CARD").Width + gridView.Columns("CHIT").Width _
                + gridView.Columns("CHEQUE").Width + gridView.Columns("BALANCE").Width

            '.Columns("TRANDATE1~TRANNO1~COLHEAD~RESULT").Width = gridView.Columns("TRANDATE1").Width _
            '    + gridView.Columns("TRANNO1").Width + gridView.Columns("COLHEAD").Width + gridView.Columns("RESULT").Width _
            '    + gridView.Columns("KEYNO").Width

            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            'With .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB")
            '    If ColWidth = 0 Then
            '        .HeaderText = ""
            '        .Width = 0
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '    End If
            'End With
        End With
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        PurchaseAbs()
    End Sub

    Private Sub frmPurchaseAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
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

    Function GridViewFormat() As Integer
        With gridView
            If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("TRANDATE1") Then .Columns("TRANDATE1").Visible = False
            If .Columns.Contains("TRANNO1") Then .Columns("TRANNO1").Visible = False
            If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TRANDATE1").Visible = False
            .Columns("TRANNO1").Visible = False

            .Columns("OGTYPE").HeaderText = "TYPE"
            .Columns("SRTYPE").HeaderText = "TYPE"
            If funcGstView(dtpFrom.Value) = True Then
                .Columns("SRVAT").HeaderText = "SRGST"
                .Columns("VAT").HeaderText = "GST"
            Else
                If .Columns.Contains("IGST") Then .Columns("IGST").Visible = False
                If .Columns.Contains("CGST") Then .Columns("CGST").Visible = False
                If .Columns.Contains("SGST") Then .Columns("SGST").Visible = False
                If .Columns.Contains("SRIGST") Then .Columns("SRIGST").Visible = False
                If .Columns.Contains("SRCGST") Then .Columns("SRCGST").Visible = False
                If .Columns.Contains("SRSGST") Then .Columns("SRSGST").Visible = False
                If .Columns.Contains("HSN") Then .Columns("HSN").Visible = False
            End If
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Function

    Private Sub Prop_Sets()
        Dim obj As New frmSalesPurAbs_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_rbtAll = rbtAll.Checked
        obj.p_rbtVat = rbtVat.Checked
        obj.p_rbtWVat = rbtWoVat.Checked
        obj.p_rbtTAll = rbtTAll.Checked
        obj.p_rbtSales = rbtSales.Checked
        obj.p_rbtSalesReturn = rbtSR.Checked
        obj.p_rbtPU = rbtPurchase.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSalesPurAbs_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesPurAbs_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesPurAbs_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        rbtAll.Checked = obj.p_rbtAll
        rbtVat.Checked = obj.p_rbtVat
        rbtWoVat.Checked = obj.p_rbtWVat
        rbtTAll.Checked = obj.p_rbtTAll
        rbtSales.Checked = obj.p_rbtSales
        rbtSR.Checked = obj.p_rbtSalesReturn
        rbtPurchase.Checked = obj.p_rbtPU
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
        funcHeaderNew()
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

Public Class frmSalesPurAbs_Properties
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