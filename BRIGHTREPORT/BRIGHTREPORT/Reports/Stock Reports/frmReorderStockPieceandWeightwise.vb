Imports System.Data.OleDb
Public Class frmReorderStockPieceandWeightwise
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim blnPcs As Boolean
    Public dsGrid As New DataSet
    Dim Days As String
    Dim HDays As String
    Dim SHOWFLAG As Boolean = False
    'Dim objfinsubitem As New FindSubItem
    Private Sub frmPiecewiseReorderStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmPiecewiseReorderStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Tabmain.ItemSize = New System.Drawing.Size(1, 1)
        Me.Tabmain.Region = New Region(New RectangleF(Me.tabgeneral.Left, Me.tabgeneral.Top, Me.tabgeneral.Width, Me.tabgeneral.Height))
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER"
        FillCheckedListBox(strSql, chkLstMetal)

        chkLstSubitem.Items.Clear()
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST ORDER BY SUBITEMNAME"
        FillCheckedListBox(strSql, chkLstSubitem)

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmPiecewiseReorderStock_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        Tabmain.SelectedTab = tabgeneral
        dtpAsOnDate.Value = GetServerDate()
        dtpAsOnDate.Select()

        Dim dtcost As New DataTable
        cmbCostCenter.Text = "ALL"
        strSql = vbCrLf + " SELECT '' COSTID,'ALL' COSTNAME "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        da = New OleDbDataAdapter(strSql, cn)
        dtcost = New DataTable
        da.Fill(dtcost)
        If dtcost.Rows.Count > 0 Then
            cmbCostCenter.DataSource = Nothing
            cmbCostCenter.DataSource = dtcost
            cmbCostCenter.DisplayMember = "COSTNAME"
            cmbCostCenter.ValueMember = "COSTID"
        End If

        Prop_Gets()
        cmbxGrpby.Items.Clear()
        cmbxGrpby.Items.Add("ITEM")
        cmbxGrpby.Items.Add("SUBITEM")
        cmbxGrpby.Items.Add("DESIGNER")
        cmbxGrpby.Text = "ITEM"
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub chkItemSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        GridView.DataSource = Nothing
        DataGridLoad()
    End Sub
    Private Sub DataGridLoad()
        Dim dtItem As New DataTable
        Dim dts As New DataTable

        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkSubItem As String = GetChecked_CheckedList(chkLstSubitem)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)

        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        Dim selDesignerId As String = ""
        Dim selItemcounterId As String = ""
        Dim filterby As String = ""

        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If chkItemName = "" Then
            MsgBox("Choose atleast one item", MsgBoxStyle.Information)
            chkLstItem.Focus()
            Exit Sub
        End If
        'ITEMNAME
        selItemId = ""
        strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                selItemId += dt.Rows(i).Item("ITEMID").ToString + ","
            Next
            If selItemId <> "" Then
                selItemId = Mid(selItemId, 1, selItemId.Length - 1)
            End If
        End If
        'SUBITEMNAME
        selSubItemId = ""
        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (" & selItemId & ") "
        If chkSubItem <> "" Then
            strSql += " AND SUBITEMNAME IN (" & chkSubItem & ")"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtSub As New DataTable
        da.Fill(dtSub)
        If dtSub.Rows.Count > 0 Then
            For i As Integer = 0 To dtSub.Rows.Count - 1
                selSubItemId += dtSub.Rows(i).Item("SUBITEMID").ToString + ","
            Next
            selSubItemId = selSubItemId.Trim(",")
        End If
        'DESINGER

        strSql = "Select DESIGNERID FROM " & cnAdminDb & "..DESIGNER "
        If chkDesigner <> "" Then
            strSql += "   WHERE DESIGNERNAME In (" & chkDesigner & ")"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtdesign As New DataTable
        da.Fill(dtdesign)
        If dtdesign.Rows.Count > 0 Then
            For i As Integer = 0 To dtdesign.Rows.Count - 1
                selDesignerId += dtdesign.Rows(i).Item("DESIGNERID").ToString + ","
            Next
            selDesignerId = selDesignerId.Trim(",")
        End If

        'ITEMCOUNTER
        strSql = "Select ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER  "
        If chkItemCounter <> "" Then
            strSql += " WHERE ITEMCTRNAME In (" & chkItemCounter & ")"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtitemcounter As New DataTable
        da.Fill(dtitemcounter)
        If dtitemcounter.Rows.Count > 0 Then
            For i As Integer = 0 To dtitemcounter.Rows.Count - 1
                selItemcounterId += dtitemcounter.Rows(i).Item("ITEMCTRID").ToString + ","
            Next
            selItemcounterId = selItemcounterId.Trim(",")
        End If

        If rbtItemTag.Checked = True Then
            filterby = "I"
        End If
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_REORDERPIECESANDWEIGHTWISESTOCK"
        strSql += vbCrLf + " @TEMPTABLEDB = 'TEMPTABLEDB..[TEMPWEIGHTPIECESWISESTOCK" & Environment.MachineName & "]'"
        strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@ASDATE = '" & Format(dtpAsOnDate.Value.Date, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYIDS = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & selItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        strSql += vbCrLf + " ,@FILTERBY = '" & filterby & "' "
        strSql += vbCrLf + " ,@GROUPBY = '" & Mid(cmbxGrpby.Text, 1, 1) & "' "
        strSql += vbCrLf + " ,@COSTID = '" & cmbCostCenter.SelectedValue.ToString & "' "
        strSql += vbCrLf + " ,@DESIGNERID = '" & selDesignerId & "' "
        strSql += vbCrLf + " ,@ITEMCTRID = '" & selItemcounterId & "' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim DtGrid As New DataTable("SUMMARY")
        strSql = " SELECT * FROM TEMPTABLEDB..TEMPREORPCSWGTFINAL "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        Else
        End If

        GridView.RowTemplate.Height = 21
        GridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        GridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dim tit As String = ""
        tit = " PIECE AND WEIGHT MIN AND MAX STOCK REORDER" + vbCrLf
        tit += " AS ON " & dtpAsOnDate.Text & ""
        lbltitle.Text = tit
        GridView.DataSource = DtGrid
        GridView.Columns("RESULT").Visible = False
        GridView.Columns("COLHEAD").Visible = False
        GridView.Columns("ITEMNAME").Visible = False
        GridView.Columns("FROMWEIGHT").Visible = False
        GridView.Columns("TOWEIGHT").Visible = False
        If cmbxGrpby.Text = "ITEM" Then
        ElseIf cmbxGrpby.Text = "SUBITEM" Then
            GridView.Columns("SUBITEMNAME").Visible = False
        ElseIf cmbxGrpby.Text = "DESIGNER" Then
            GridView.Columns("SUBITEMNAME").Visible = False
            GridView.Columns("DESIGNERNAME").Visible = False
        End If
        GridView.Columns("PARTICULAR").Width = 150

        GridView.Columns("RANGE").Width = 140
        GridView.Columns("RANGECAPTION").HeaderText = "CAPTION"

        GridView.Columns("MINPIECE").HeaderText = "PCS"
        GridView.Columns("MINPIECE").Width = 90
        GridView.Columns("MINWEIGHT").HeaderText = "WT"
        GridView.Columns("MINWEIGHT").Width = 120

        GridView.Columns("MAXPIECE").HeaderText = "PCS"
        GridView.Columns("MAXPIECE").Width = 90
        GridView.Columns("MAXWEIGHT").HeaderText = "WT"
        GridView.Columns("MAXWEIGHT").Width = 120

        GridView.Columns("STOCKPCS").HeaderText = "PCS"
        GridView.Columns("STOCKPCS").Width = 90
        GridView.Columns("STOCKWEIGHT").HeaderText = "WT"
        GridView.Columns("STOCKWEIGHT").Width = 120

        GridView.Columns("MINBALPCS").HeaderText = "MINBALPCS"
        GridView.Columns("MINBALPCS").Width = 90
        GridView.Columns("MAXBALPCS").HeaderText = "MAXBALPCS"
        GridView.Columns("MAXBALPCS").Width = 90

        GridView.Columns("MINBALWEIGHT").HeaderText = "MINBALWT"
        GridView.Columns("MINBALWEIGHT").Width = 120
        GridView.Columns("MAXBALWEIGHT").HeaderText = "MAXBALWT"
        GridView.Columns("MAXBALWEIGHT").Width = 120
        FormatGridColumns(GridView, False, False, , False)
        Prop_Sets()
        Tabmain.SelectedTab = tabview
        GridViewFormat()
        gridviewHeader()

        GridView.Focus()
    End Sub

    Function gridviewHeader()
        Dim dtHeader As New DataTable
        With dtHeader
            .Columns.Add("RANGE~PARTICULAR~RANGECAPTION", GetType(String))
            .Columns.Add("MINPIECE~MINWEIGHT", GetType(String))
            .Columns.Add("MAXPIECE~MAXWEIGHT", GetType(String))
            .Columns.Add("STOCKPCS~STOCKWEIGHT", GetType(String))
            .Columns.Add("MINBALPCS~MAXBALPCS~MINBALWEIGHT~MAXBALWEIGHT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtHeader
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("RANGE~PARTICULAR~RANGECAPTION").Width = GridView.Columns("PARTICULAR").Width + GridView.Columns("RANGE").Width + GridView.Columns("RANGECAPTION").Width
            .Columns("RANGE~PARTICULAR~RANGECAPTION").HeaderText = ""

            .Columns("MINPIECE~MINWEIGHT").Width = GridView.Columns("MINPIECE").Width + GridView.Columns("MINWEIGHT").Width
            .Columns("MINPIECE~MINWEIGHT").HeaderText = "MIN"

            .Columns("MAXPIECE~MAXWEIGHT").Width = GridView.Columns("MAXPIECE").Width + GridView.Columns("MAXWEIGHT").Width
            .Columns("MAXPIECE~MAXWEIGHT").HeaderText = "MAX"

            .Columns("STOCKPCS~STOCKWEIGHT").Width = GridView.Columns("STOCKPCS").Width + GridView.Columns("STOCKWEIGHT").Width
            .Columns("STOCKPCS~STOCKWEIGHT").HeaderText = "STOCK"

            .Columns("MINBALPCS~MAXBALPCS~MINBALWEIGHT~MAXBALWEIGHT").Width = GridView.Columns("MINBALPCS").Width + GridView.Columns("MAXBALPCS").Width + GridView.Columns("MINBALWEIGHT").Width + GridView.Columns("MAXBALWEIGHT").Width
            .Columns("MINBALPCS~MAXBALPCS~MINBALWEIGHT~MAXBALWEIGHT").HeaderText = "REORDER STOCK"


            Dim colWid As Integer = 0
            For cnt As Integer = 0 To GridView.ColumnCount - 1
                If GridView.Columns(cnt).Visible Then colWid += GridView.Columns(cnt).Width
            Next
            If colWid >= GridView.Width Then
                .Columns("SCROLL").Visible = CType(GridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
            .Columns("SCROLL").HeaderText = ""
        End With
    End Function

    Function GridViewFormat() As Integer
        For j As Integer = 0 To GridView.Rows.Count - 1
            With GridView.Rows(j)
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.MistyRose
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "U"
                        .DefaultCellStyle.BackColor = Color.Lavender
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub chkLstItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.GotFocus
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub
    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
        End If
    End Sub



    Private Sub Prop_Sets()
        Dim obj As New frmPiecewiseReorderStock_Properties
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        obj.p_chkSubItemSelectAll = chkSubitemAll.Checked
        GetChecked_CheckedList(chkLstSubitem, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkItemSelectAll = chkItemSelectAll.Checked
        GetChecked_CheckedList(chkLstItem, obj.p_chkLstItem)
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPiecewiseReorderStock_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmPiecewiseReorderStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPiecewiseReorderStock_Properties))
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        chkSubitemAll.Checked = obj.p_chkSubItemSelectAll
        'SetChecked_CheckedList(chkLstSubitem, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkItemSelectAll.Checked = obj.p_chkItemSelectAll
        SetChecked_CheckedList(chkLstItem, obj.p_chkLstItem, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
    End Sub

    Private Sub chkSubitemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSubitemAll.CheckedChanged
        SetChecked_CheckedList(chkLstSubitem, chkSubitemAll.Checked)
    End Sub


    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 And Tabmain.SelectedTab.Name = tabview.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 And Tabmain.SelectedTab.Name = tabview.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle.Text, GridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub escapeToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnBack_Click(Me, New EventArgs)
    End Sub

    Private Sub lbltitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbltitle.Click

    End Sub

    Private Sub GridView_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView.SelectionChanged
        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        If SHOWFLAG = True Then
            SHOWFLAG = False
            Exit Sub
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If GridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            SetGridHeadColWidth(gridViewHead)
        End If
    End Sub

    Public Sub SetGridHeadColWidth(ByVal gridViewHeader As DataGridView)
        If Not gridViewHeader.Visible Then Exit Sub
        If gridViewHeader Is Nothing Then Exit Sub
        If Not GridView.ColumnCount > 0 Then Exit Sub
        If Not gridViewHeader.ColumnCount > 0 Then Exit Sub
        With gridViewHeader
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Width = SetGroupHeadColWid(.Columns(cnt), GridView)
            Next
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To GridView.ColumnCount - 1
                If GridView.Columns(cnt).Visible Then colWid += GridView.Columns(cnt).Width
            Next
            If colWid >= GridView.Width Then
                gridViewHeader.Columns("SCROLL").Visible = CType(GridView.Controls(1), VScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
                gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub chkLstItem_Leave(sender As Object, e As EventArgs) Handles chkLstItem.Leave
        Dim selItemId As String = ""
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        If chkItemName = "" Then Exit Sub
        'ITEMNAME
        selItemId = ""
        strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                selItemId += dt.Rows(i).Item("ITEMID").ToString + ","
            Next
            If selItemId <> "" Then
                selItemId = Mid(selItemId, 1, selItemId.Length - 1)
            End If
        End If
        chkLstSubitem.Items.Clear()
        If selItemId = "" Then
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST ORDER BY SUBITEMNAME"
        Else
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (" & selItemId & ") ORDER BY SUBITEMNAME"
        End If
        FillCheckedListBox(strSql, chkLstSubitem)
    End Sub

    Private Sub GridView_Scroll(sender As Object, e As ScrollEventArgs) Handles GridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(GridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
Public Class frmPiecewiseReorderStock_Properties
    Private chkDesignerSelectAll As Boolean = False
    Public Property p_chkDesignerSelectAll() As Boolean
        Get
            Return chkDesignerSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkDesignerSelectAll = value
        End Set
    End Property
    Private chkLstDesigner As New List(Of String)
    Public Property p_chkLstDesigner() As List(Of String)
        Get
            Return chkLstDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkLstDesigner = value
        End Set
    End Property
    Private chkSubItemSelectAll As Boolean = False
    Public Property p_chkSubItemSelectAll() As Boolean
        Get
            Return chkSubItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkSubItemSelectAll = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property

    Private chkItemSelectAll As Boolean = False
    Public Property p_chkItemSelectAll() As Boolean
        Get
            Return chkItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemSelectAll = value
        End Set
    End Property
    Private chkLstItem As New List(Of String)
    Public Property p_chkLstItem() As List(Of String)
        Get
            Return chkLstItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstItem = value
        End Set
    End Property
    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
End Class