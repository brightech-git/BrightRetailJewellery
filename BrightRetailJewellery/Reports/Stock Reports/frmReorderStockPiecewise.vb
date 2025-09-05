Imports System.Data.OleDb

Public Class frmReorderStockPiecewise
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim blnPcs As Boolean
    Public dsGrid As New DataSet
    Dim Days As String
    Dim HDays As String
    Dim SHOWFLAG As Boolean = False
    Dim objfinsubitem As New FindSubItem

    Private Sub frmPiecewiseReorderStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.O Then
            Call GridOrderbook()
        ElseIf e.KeyCode = Keys.Escape Then
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
        'objGPack.FillCombo(strSql, cmbSubItemName, True, True)

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)


        strSql = vbCrLf + " SELECT 'ALL' SGROUPNAME,'ALL' SGROUPID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SGROUPNAME ,CONVERT(VARCHAR,SGROUPID),2 RESULT FROM " & cnAdminDb & "..SUBITEMGROUP WHERE ACTIVE='Y' ORDER BY RESULT,SGROUPNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtSubGrp As New DataTable
        da.Fill(dtSubGrp)
        cmbSubItemGrp.Items.Clear()
        For Each Row As DataRow In dtSubGrp.Rows
            cmbSubItemGrp.Items.Add(Row.Item("SGROUPNAME").ToString)
        Next

        'strSql = vbCrLf + " SELECT 'ALL' SMITHNAME,'ALL' ACCODE,1 RESULT"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " SELECT ACNAME  AS SMITHNAME ,CONVERT(VARCHAR,S.ACCODE),2 RESULT FROM " & cnAdminDb & "..SMITHSUBITEMDETAIL S"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ACHEAD A ON A.ACCODE=S.ACCODE"
        ''strSql += vbCrLf + " ORDER BY RESULT,ACNAME"
        'da = New OleDbDataAdapter(strSql, cn)
        'Dim dtSubSmith As New DataTable
        'da.Fill(dtSubSmith)
        'cmbSubItemSmith.Items.Clear()
        'For Each Row As DataRow In dtSubSmith.Rows
        '    cmbSubItemSmith.Items.Add(Row.Item("SMITHNAME").ToString)
        'Next

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmPiecewiseReorderStock_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'Call GridView_KeyPress(sender, e)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        Tabmain.SelectedTab = tabgeneral
        dtpAsOnDate.Value = GetServerDate()
        dtpAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub chkItemSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'GrpSearch.Visible = False
        DataGridLoad()
    End Sub
    Private Sub DataGridLoad()
        Dim dtItem As New DataTable
        Dim dts As New DataTable
        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        Dim chkSubItem As String = GetChecked_CheckedList(chkLstSubitem)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        Dim IsLessMinStk As String = Nothing
        Dim IsNilstk As String = Nothing
        Dim IsStkWithoutSales As String = Nothing
        Dim IsSubItemGrp As String = Nothing
        Dim SubItemGrp As String = Nothing
        Dim IsSubItemSmith As String = Nothing
        Dim Accode As String = Nothing
        Dim nilpcs As Integer
        Dim itemname As String = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If chkItemSelectAll.Checked = True Or chkItemName = "" Then
            selItemId = "ALL"
        End If
        If chkItemSelectAll.Checked = False And chkItemName <> "" Then
            strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    selItemId += dt.Rows(i).Item("ITEMID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selItemId <> "" Then
                    selItemId = Mid(selItemId, 1, selItemId.Length - 1)
                End If
            End If
        End If
        If chkSubitemAll.Checked = True Or chkSubItem = "" Then
            selSubItemId = "ALL"
        End If
        If chkSubitemAll.Checked = False And chkSubItem <> "" Then
            strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & chkSubItem & ")"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtSub As New DataTable
            da.Fill(dtSub)
            If dtSub.Rows.Count > 0 Then
                For i As Integer = 0 To dtSub.Rows.Count - 1
                    selSubItemId += dtSub.Rows(i).Item("SUBITEMID").ToString + ","
                Next
                If selSubItemId <> "" Then
                    selSubItemId = Mid(selSubItemId, 1, selSubItemId.Length - 1)
                End If
            End If
        End If

        Dim asDate As Date
        Dim noDays As Integer

        asDate = dtpAsOnDate.Value.Date
        If txtNoofDays.Text <> "" Then
            noDays = txtNoofDays.Text
        Else
            noDays = 0
        End If
        Days = asDate.AddDays(-noDays)
        HDays = asDate.AddDays(-noDays).ToString("dd/MM/yyyy")
        If rdbLessMinPcsStk.Checked Then
            IsLessMinStk = "Y"
        Else
            IsLessMinStk = "N"
        End If
        If rbtnilStockSales.Checked Then
            IsNilstk = "Y"
        Else
            IsNilstk = "N"
        End If
        If rdbStockWithoutSale.Checked Then
            IsStkWithoutSales = "Y"
        Else
            IsStkWithoutSales = "N"
        End If
        If cmbSubItemGrp.Text <> "" Then
            If cmbSubItemGrp.Text <> "ALL" Then
                IsSubItemGrp = "Y"
                SubItemGrp = cmbSubItemGrp.Text
                rdbAll.Checked = True
            Else
                IsSubItemGrp = "N"
            End If
        Else
            IsSubItemGrp = "N"
        End If
            'If rdbAll.Checked Then
            '    IsSubItemGrp = "N"
            'End If
            'If cmbSubItemSmith.Text <> "" Then
            '    IsSubItemSmith = "Y"
            '    strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbSubItemSmith.Text & "'"
            '    da = New OleDbDataAdapter(strSql, cn)
            '    Dim dtSmith As New DataTable
            '    da.Fill(dtSmith)
            '    If dtSmith.Rows.Count > 0 Then Accode = dtSmith.Rows(0).Item("ACCODE").ToString
            'Else
            '    IsSubItemSmith = "N"
            'End If
        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_REORDERSTKPIECE"
        strSql += vbCrLf + " @ITEMID = '" & selItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        strSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,@CNADMINDB='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@CNSTOCKDB='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@NOD='" & Days & "'"
        strSql += vbCrLf + " ,@ISLESSMINPCSSTK='" & IsLessMinStk & "'"
        strSql += vbCrLf + " ,@ISNILSTKPCS='" & IsNilstk & "'"
        strSql += vbCrLf + " ,@ISSTKWITHOUTSALES='" & IsStkWithoutSales & "'"
        strSql += vbCrLf + " ,@ISSUBGROUPWISE='" & IsSubItemGrp & "'"
        strSql += vbCrLf + " ,@SUBITEMGROUP='" & SubItemGrp & "'"
        strSql += vbCrLf + " ,@LOTCHECK='" & IIf(chkItemLot.Checked = True, "Y", "N") & "'"
            'strSql += vbCrLf + " ,@ISSUBSMITHWISE='" & IsSubItemSmith & "'"
            'strSql += vbCrLf + " ,@ACCODE='" & Accode & "'"

            Dim DtGrid As New DataTable("SUMMARY")
            DtGrid.Columns.Add("KEYNO", GetType(Integer))
            DtGrid.Columns("KEYNO").AutoIncrement = True
            DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            DtGrid.Columns("KEYNO").AutoIncrementStep = 1
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            da.Fill(DtGrid)
        DtGrid.Columns("SGROUPNAME").SetOrdinal(3)
        'DtGrid.Columns("DIFFPCS2").SetOrdinal(11)
            If Not DtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            Else
                If rbtnilStockSales.Checked Then
                    For i As Integer = 0 To DtGrid.Rows.Count - 1
                        nilpcs = Val(DtGrid.Rows(i).Item("STKPCS").ToString)
                        If nilpcs = 0 Then

                        End If
                    Next
                End If
            End If
            'Dim dtdistinct As New DataTable
            'dtdistinct = DtGrid.DefaultView.ToTable(True, "SUBITEMNAME")
            'cmbSubItemName.Items.Clear()
            'For Each Row As DataRow In dtdistinct.Rows
            '    cmbSubItemName.Items.Add(Row.Item("SUBITEMNAME").ToString)
            'Next


            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            GridView.RowTemplate.Height = 21
            GridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            GridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim tit As String = ""
            objGridShower.Text = "PIECE WISE STOCK REORDER"
            tit = "PIECE WISE STOCK REORDER" + vbCrLf
            tit += " AS ON " & dtpAsOnDate.Text & ""
            lbltitle.Text = tit
            GridView.DataSource = DtGrid
            DataGridView_SummaryFormatting(GridView)
            FormatGridColumns(GridView, False, False, , False)
            Prop_Sets()
            Tabmain.SelectedTab = tabview
            GridViewFormat()
            GridView.Focus()
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("ITEMNAME").Width = 200
            .Columns("SUBITEMNAME").Width = 300
            .Columns("SGROUPNAME").Width = 100
            .Columns("DESIGNER").Width = 100
            .Columns("STKPCS").Width = 100
            .Columns("ORDPCS").Width = 100
            .Columns("LOTPCS").Width = 100
            .Columns("MINPCS").Width = 80
            .Columns("MAXPCS").Width = 80
            .Columns("DIFFPCS").Width = 80
            .Columns("DIFFPCS2").Width = 80
            .Columns("DIFFPCS1").Width = 80

            .Columns("NDAYSTOCK").Width = 150
            .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ORDPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LOTPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MINPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS2").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITEMNAME").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("SUBITEMNAME").Visible = True
            .Columns("SGROUPNAME").Visible = True
            .Columns("STKPCS").Visible = True
            .Columns("ORDPCS").Visible = True
            .Columns("LOTPCS").Visible = True
            .Columns("MINPCS").Visible = True
            .Columns("MAXPCS").Visible = True
            .Columns("DIFFPCS").Visible = True
            .Columns("DIFFPCS2").Visible = True
            .Columns("DIFFPCS1").Visible = True
            .Columns("DESIGNER").Visible = True
            .Columns("NDAYSTOCK").Visible = True
            .Columns("SGROUPNAME").HeaderText = "SUBITEM GROUP"
            .Columns("STKPCS").HeaderText = "STOCK"
            .Columns("ORDPCS").HeaderText = "SM.ORDER"
            .Columns("LOTPCS").HeaderText = "LOT PCS"
            .Columns("MINPCS").HeaderText = "MIN"
            .Columns("MAXPCS").HeaderText = "MAX"
            .Columns("DIFFPCS").HeaderText = "MAX-STK"
            .Columns("DIFFPCS2").HeaderText = "MIN-STK"
            .Columns("DIFFPCS1").HeaderText = "REORD PCS"
            If txtNoofDays.Text <> "" Then
                .Columns("NDAYSTOCK").HeaderText = "<" + txtNoofDays.Text + " (" + HDays + ")"
            ElseIf txtNoofDays.Text = "" Then
                .Columns("NDAYSTOCK").HeaderText = "AsOnDate Stock(" + System.DateTime.Now.Date.ToString("dd/MM/yyyy") + ")"
            End If
        End With
    End Sub
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

    Private Sub GridOrderbook()
        With objGridShower
            If Not .gridView.RowCount > 0 Then Exit Sub
            If .gridView.CurrentRow Is Nothing Then Exit Sub
            If .gridView.CurrentRow.Cells("ITEMID").Value.ToString = "" Then Exit Sub
            If .gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then Exit Sub
            Dim objTagViewer As New frmTagImageViewer( _
             .gridView.CurrentRow.Cells("TAGNO").Value.ToString, _
             Val(.gridView.CurrentRow.Cells("ITEMID").Value.ToString), _
             BrighttechPack.Methods.GetRights(_DtUserRights, frmTagCheck.Name, BrighttechPack.Methods.RightMode.Authorize, False))

            Dim objAutoorder As New AutoOrder
            objAutoorder.BillCostId = cnCostId
            objAutoorder.ShowDialog()
        End With
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
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
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
    Private Sub EditToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem1.Click
        Dim pcs As Integer
        If GridView.Focused Then
            Dim objauto As New AutoOrder
            If GridView.CurrentRow.Cells("RESULT").Value.ToString = "1" Then
                objauto.cmbItemName_Man.Text = GridView.CurrentRow.Cells("ITEM").Value.ToString
                objauto.cmbSubItemName_Own.Text = GridView.CurrentRow.Cells("SUBITEM").Value.ToString
                pcs = Val(GridView.CurrentRow.Cells("DIFFPCS1").Value.ToString)
                objauto.txtPiece_NUM.Text = pcs
                If objauto.txtPiece_NUM.Text < 0 Then
                    objauto.txtPiece_NUM.ReadOnly = True
                Else
                    objauto.txtPiece_NUM.ReadOnly = False
                End If
                objauto.ShowDialog()
                btnSearch_Click(Me, New EventArgs)
                For i As Integer = 0 To GridView.Rows.Count - 1
                    If GridView.Rows(i).Cells("ITEMNAME").Value.ToString = objauto.cmbItemName_Man.Text And GridView.Rows(i).Cells("SUBITEMNAME").Value.ToString = objauto.cmbSubItemName_Own.Text Then
                        GridView.Focus()
                        SHOWFLAG = True
                        GridView.Rows(i).Selected = True
                        GridView.Rows(i).Cells(2).Selected = True
                        GridView.FirstDisplayedScrollingRowIndex = GridView.Rows(i).Index
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub UpdateMasterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateMasterToolStripMenuItem.Click
        Dim pcs As Integer
        If GridView.Focused Then
            If GridView.CurrentRow.Cells("RESULT").Value.ToString = "1" Then
                Dim itemid As Integer
                Dim subitemid As Integer

                Dim itemname As String
                Dim subitemname As String

                'Dim costid As String
                itemid = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMNAME='" & GridView.CurrentRow.Cells("ITEM").Value.ToString & "'")
                subitemid = GetSqlValue(cn, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME='" & GridView.CurrentRow.Cells("SUBITEM").Value.ToString & "'")
                'costid = GetSqlValue(cn, "SELECT COSTID " & cnAdminDb & "..COSTCENTRE FROM WHERE COSTNAME='" & GridView.CurrentRow.Cells("SUMITEM").Value.ToString & "'")
                strSql = "SELECT SNO FROM  " & cnAdminDb & "..STKREORDER  WHERE ITEMID=" & itemid & " "
                strSql += " AND SUBITEMID=" & subitemid & " "
                strSql += " AND MINPIECE='" & GridView.CurrentRow.Cells("MINPCS").Value.ToString & "' AND PIECE='" & GridView.CurrentRow.Cells("MAXPCS").Value.ToString & "'"
                Dim sno As String = ""
                sno = GetSqlValue(cn, strSql)
                Dim objupdate As New frmPieceWiseStockReOrder(True, sno)
                strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID IN (SELECT METALID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & GridView.CurrentRow.Cells("ITEM").Value.ToString & "') "
                objupdate.cmbMetal_Man.Text = GetSqlValue(cn, strSql)
                strSql = " Select ItemName from " & cnAdminDb & "..ItemMast"
                strSql += " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & objupdate.cmbMetal_Man.Text & "')"
                strSql += " Order by ItemName"
                objGPack.FillCombo(strSql, objupdate.cmbItem_Man)
                objupdate.cmbItem_Man.Text = GridView.CurrentRow.Cells("ITEM").Value.ToString
                itemname = GridView.CurrentRow.Cells("ITEM").Value.ToString
                objupdate.cmbSubItem_Man.Text = ""
                strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where "
                strSql += " ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & objupdate.cmbItem_Man.Text & "' and SubItem = 'Y')"
                strSql += " Order by SubItemName"
                objGPack.FillCombo(strSql, objupdate.cmbSubItem_Man, , False)
                objupdate.cmbSubItem_Man.Text = GridView.CurrentRow.Cells("SUBITEM").Value.ToString
                subitemname = GridView.CurrentRow.Cells("SUBITEM").Value.ToString
                objupdate.cmbDesignerName.Text = GridView.CurrentRow.Cells("DESIGNER").Value.ToString
                objupdate.txtMaxPiece.Text = GridView.CurrentRow.Cells("MAXPCS").Value.ToString
                objupdate.txtMinPcs.Text = GridView.CurrentRow.Cells("MINPCS").Value.ToString
                objupdate.txtMinPcs.Select()
                objupdate.ShowDialog()
                btnSearch_Click(Me, New EventArgs)
                For i As Integer = 0 To GridView.Rows.Count - 1
                    If GridView.Rows(i).Cells("ITEMNAME").Value.ToString = itemname And GridView.Rows(i).Cells("SUBITEMNAME").Value.ToString = subitemname Then
                        GridView.Focus()
                        SHOWFLAG = True
                        GridView.Rows(i).Selected = True
                        GridView.Rows(i).Cells(2).Selected = True
                        GridView.FirstDisplayedScrollingRowIndex = GridView.Rows(i).Index
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
    'Private Sub btnViewSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    For i As Integer = 0 To GridView.Rows.Count - 1
    '        If GridView.Rows(i).Cells("SUBITEMNAME").Value.ToString = cmbSubItemName.Text Then
    '            GridView.Focus()
    '            GridView.Rows(i).Selected = True
    '            GridView.FirstDisplayedScrollingRowIndex = GridView.Rows(i).Index
    '            Exit For
    '        End If
    '    Next
    'End Sub

    Private Sub escapeToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles escapeToolStripMenuItem2.Click
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
        If GridView.Rows.Count > 0 Then
            If GridView.Rows(GridView.CurrentCell.RowIndex).Cells("ITEMNAME").Value.ToString <> "" Then
                strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & GridView.Rows(GridView.CurrentCell.RowIndex).Cells("ITEMNAME").Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dtItem As New DataTable
                da.Fill(dtItem)
                If dtItem.Rows.Count > 0 Then
                    selItemId += dtItem.Rows(0).Item("ITEMID").ToString
                End If
                strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME ='" & GridView.Rows(GridView.CurrentCell.RowIndex).Cells("SUBITEMNAME").Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dtSItem As New DataTable
                da.Fill(dtSItem)
                If dtSItem.Rows.Count > 0 Then
                    selSubItemId += dtSItem.Rows(0).Item("SUBITEMID").ToString
                End If
            Else
                Exit Sub
            End If
        End If

        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_REORDERSALESPIECE"
        strSql += vbCrLf + "  @ITEMID = '" & selItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & cnTranFromDate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,@CNADMINDB='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@CNSTOCKDB='" & cnStockDb & "'"

        Dim DtSales As New DataTable("SUMMARY")
        DtSales.Columns.Add("KEYNO", GetType(Integer))
        DtSales.Columns("KEYNO").AutoIncrement = True
        DtSales.Columns("KEYNO").AutoIncrementSeed = 0
        DtSales.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        DtSales.Clear()
        gridViewDetail.DataSource = Nothing
        da.Fill(DtSales)
        If Not DtSales.Rows.Count > 0 Then
            gridViewDetail.Visible = True
            gridViewHeader.Visible = True
            If GridView.Rows(GridView.CurrentCell.RowIndex).Cells("COLHEAD").Value <> "S" And GridView.Rows(GridView.CurrentCell.RowIndex).Cells("COLHEAD").Value <> "T" Then
                DtSales.Rows.Add()
                DtSales.Rows(0).Item("ITEMNAME") = GridView.Rows(GridView.CurrentCell.RowIndex).Cells("ITEM").Value
                DtSales.Rows(0).Item("SUBITEMNAME") = GridView.Rows(GridView.CurrentCell.RowIndex).Cells("SUBITEM").Value
                DtSales.Rows(0).Item("APRIL") = 0
                DtSales.Rows(0).Item("MAY") = 0
                DtSales.Rows(0).Item("JUNE") = 0
                DtSales.Rows(0).Item("JULY") = 0
                DtSales.Rows(0).Item("AUG") = 0
                DtSales.Rows(0).Item("SEP") = 0
                DtSales.Rows(0).Item("OCT") = 0
                DtSales.Rows(0).Item("NOV") = 0
                DtSales.Rows(0).Item("DEC") = 0
                DtSales.Rows(0).Item("JAN") = 0
                DtSales.Rows(0).Item("FEB") = 0
                DtSales.Rows(0).Item("MARCH") = 0
            End If
        Else
            gridViewDetail.Visible = True
            gridViewHeader.Visible = True
        End If
        gridViewDetail.DataSource = DtSales
        gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridViewHeader.Visible = True
        gridViewDetail.Columns("KEYNO").Visible = False
        gridViewDetail.Columns("ITEMNAME").Width = 150
        gridViewDetail.Columns("SUBITEMNAME").Width = 150
        gridViewDetail.Columns("APRIL").Width = 60
        gridViewDetail.Columns("MAY").Width = 60
        gridViewDetail.Columns("JUNE").Width = 60
        gridViewDetail.Columns("JULY").Width = 60
        gridViewDetail.Columns("AUG").Width = 60
        gridViewDetail.Columns("SEP").Width = 60
        gridViewDetail.Columns("OCT").Width = 60
        gridViewDetail.Columns("NOV").Width = 60
        gridViewDetail.Columns("DEC").Width = 60
        gridViewDetail.Columns("JAN").Width = 60
        gridViewDetail.Columns("FEB").Width = 60
        gridViewDetail.Columns("MARCH").Width = 60
        gridViewDetail.Columns("APRIL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("MAY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("JUNE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("JULY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("AUG").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("SEP").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("OCT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("NOV").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("DEC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("JAN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("FEB").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridViewDetail.Columns("MARCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Private Sub GridHead()
        With gridViewDetail
            gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0

            TEMPCOLWIDTH = .Columns("ITEMNAME").Width
            TEMPCOLWIDTH = .Columns("SUBITEMNAME").Width
            gridViewHeader.Columns("ITEMNAME").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("SUBITEMNAME").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("ITEMNAME").HeaderText = "ITEMNAME"
            TEMPCOLWIDTH = 0
            gridViewHeader.Columns("SUBITEMNAME").HeaderText = "SUBITEMNAME"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("APPCS").Width + .Columns("APWT").Width
            gridViewHeader.Columns("APPCS~APWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("APPCS~APWT").HeaderText = "APRIL"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("MAPCS").Width + .Columns("MAWT").Width
            gridViewHeader.Columns("MAPCS~MAWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("MAPCS~MAWT").HeaderText = "MAY"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("JUNPCS").Width + .Columns("JUNWT").Width
            gridViewHeader.Columns("JUNPCS~JUNWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("JUNPCS~JUNWT").HeaderText = "JUNE"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("JULPCS").Width + .Columns("JULWT").Width
            gridViewHeader.Columns("JULPCS~JULWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("JULPCS~JULWT").HeaderText = "JULY"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("AUPCS").Width + .Columns("AUWT").Width
            gridViewHeader.Columns("AUPCS~AUWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("AUPCS~AUWT").HeaderText = "AUGUST"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("SEPCS").Width + .Columns("SEWT").Width
            gridViewHeader.Columns("SEPCS~SEWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("SEPCS~SEWT").HeaderText = "SEPTEMBER"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("OCPCS").Width + .Columns("OCWT").Width
            gridViewHeader.Columns("OCPCS~OCWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("OCPCS~OCWT").HeaderText = "OCTOBER"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("NOPCS").Width + .Columns("NOWT").Width
            gridViewHeader.Columns("NOPCS~NOWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("NOPCS~NOWT").HeaderText = "NOVEMBER"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("DEPCS").Width + .Columns("DEWT").Width
            gridViewHeader.Columns("DEPCS~DEWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("DEPCS~DEWT").HeaderText = "DECEMBER"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("JPCS").Width + .Columns("JWT").Width
            gridViewHeader.Columns("JPCS~JWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("JPCS~JWT").HeaderText = "JANUARY"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("FPCS").Width + .Columns("FWT").Width
            gridViewHeader.Columns("FPCS~FWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("FPCS~FWT").HeaderText = "FEBRUARY"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("MPCS").Width + .Columns("MWT").Width
            gridViewHeader.Columns("MPCS~MWT").Width = TEMPCOLWIDTH
            gridViewHeader.Columns("MPCS~MWT").HeaderText = "MARCH"


            Dim colWid As Integer = 0
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Weight"
            Next
        End With
    End Sub

    Private Sub SaveToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem3.Click
        Dim pcs As Integer
        If GridView.Focused Then
            Dim objSGrp As New frmSubItemGrp
            If GridView.CurrentRow.Cells("RESULT").Value.ToString = "1" Then
                objSGrp.txtItem.Text = GridView.CurrentRow.Cells("ITEM").Value.ToString
                objSGrp.txtSubItem.Text = GridView.CurrentRow.Cells("SUBITEM").Value.ToString
                objSGrp.cmbSGrpName.Text = GridView.CurrentRow.Cells("SGROUPNAME").Value.ToString
                'If objauto.txtPiece_NUM.Text < 0 Then
                '    objauto.txtPiece_NUM.ReadOnly = True
                'Else
                '    objauto.txtPiece_NUM.ReadOnly = False
                'End If
                objSGrp.ShowDialog()
                btnSearch_Click(Me, New EventArgs)
                For i As Integer = 0 To GridView.Rows.Count - 1
                    If GridView.Rows(i).Cells("ITEMNAME").Value.ToString = objSGrp.txtItem.Text And GridView.Rows(i).Cells("SUBITEMNAME").Value.ToString = objSGrp.txtSubItem.Text Then
                        GridView.Focus()
                        SHOWFLAG = True
                        GridView.Rows(i).Selected = True
                        GridView.Rows(i).Cells(2).Selected = True
                        GridView.FirstDisplayedScrollingRowIndex = GridView.Rows(i).Index
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub


    Private Sub SearchSubItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchSubItemToolStripMenuItem.Click
        'GrpSearch.Visible = True
        'cmbSubItemName.Focus()
FindSubItem:
        objfinsubitem.ShowDialog()
        If objfinsubitem.DialogResult = Windows.Forms.DialogResult.OK Then
            For i As Integer = 0 To GridView.Rows.Count - 1
                If GridView.Rows(i).Cells("SUBITEMNAME").Value.ToString = objfinsubitem.cmbSubItem_Man.Text Then
                    GridView.Focus()
                    GridView.Rows(i).Selected = True
                    GridView.Rows(i).Cells(2).Selected = True
                    GridView.FirstDisplayedScrollingRowIndex = GridView.Rows(i).Index
                    Exit Sub
                End If
            Next
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            GoTo FindSubItem
        End If
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