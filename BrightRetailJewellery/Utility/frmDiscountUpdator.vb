Imports System.Data.OleDb
Public Class frmDiscountUpdator
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub frmDiscountUpdator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmDiscountUpdatorr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDiscountUpdator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        ' tabMode.ItemSize = New System.Drawing.Size(1, 1)
        '  tabMode.SendToBack()
        ' Me.tabMode.Region = New Region(New RectangleF(Me.tabWastage.Left, Me.tabWastage.Top, Me.tabWastage.Width, Me.tabWastage.Height))

        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = ''"
        strSql += GetItemQryFilteration("S")
        strSql += " AND ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbItem, False, False)
        cmbItem.Text = "ALL"

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub GenWeightGrid()

        strSql = " SELECT SNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += " ,TAGNO,PCS,GRSWT,LESSWT,NETWT,MAXWASTPER,MAXWAST,MAXMC,MAXMCGRM "
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += " WHERE ISSDATE IS NULL"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "'"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        End If
        If txtWeightFrom.Text <> "" And txtWeightTo.Text <> "" Then
            strSql += " AND GRSWT BETWEEN '" & txtWeightFrom.Text & "'  AND  '" & txtWeightTo.Text & "'"
        End If
        If strBCostid <> Nothing Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
        strSql += " ORDER BY ITEM,TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
        End With
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing

        GenWeightGrid()

        'If UCase(cmbCalcType.Text) = "WEIGHT" Or cmbCalcType.Text = "METAL RATE" Or cmbCalcType.Text = "FIXED" Then
        '    GenWeightGrid()
        'ElseIf UCase(cmbCalcType.Text) = "RATE" Then
        '    GenRateGrid()
        'End If
        If Not gridView.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        cmbItem.Text = "ALL"
        cmbSubItem_MAN.Text = "ALL"
        cmbItem.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CheckList(ByVal chkLst As CheckedListBox, ByVal state As Boolean)
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            chkLst.SetItemChecked(cnt, state)
        Next
    End Sub


    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbItem.Select()
    End Sub

    Private Function GetCheckedItemStr(ByVal chkLst As CheckedListBox) As String
        Dim ret As String = Nothing
        If Not chkLst.Items.Count > 0 Then Return ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            ret += "'" & chkLst.CheckedItems.Item(cnt).ToString & "',"
        Next
        If ret <> Nothing Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem_MAN.Items.Clear()
        cmbSubItem_MAN.Items.Add("ALL")
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            strSql += " ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbSubItem_MAN, True, True)
        End If
        cmbSubItem_MAN.Text = "ALL"
    End Sub


    'Private Sub cmbSubItemOld_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    cmbSubItemNew_MAN.Items.Clear()
    '    cmbSubItem_MAN.Text = ""
    '    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S"
    '    strSql += " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = S.ITEMID AND CALTYPE = S.CALTYPE AND SUBITEMNAME = '" & cmbSubItemOld_MAN.Text & "')"
    '    objGPack.FillCombo(strSql, cmbSubItemNew_MAN)
    'End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dr() As DataRow
        dr = CType(gridView.DataSource, DataTable).Select("CHECK = TRUE")
        If Not dr.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For Each ro As DataRow In dr
                strSql = GenUpdateQry(ro)
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Function GenUpdateQry(ByVal ro As DataRow) As String
        Dim qry As String = Nothing

        qry += " UPDATE " & cnAdminDb & "..ITEMTAG SET"
        If (txtwast.Text <> "" Or txtwastper.Text <> "") And (txtMc.Text <> "" Or txtMcGrm.Text <> "") Then
            If txtwast.Text <> "" Or txtwastper.Text <> "" Then
                qry += " MAXWASTPER = " & Val(txtwastper.Text.ToString)
                qry += " ,MAXWAST = " & Val(txtwast.Text.ToString)
            End If

            If txtMc.Text <> "" Or txtMcGrm.Text <> "" Then
                qry += " ,MAXMCGRM = " & Val(txtMcGrm.Text.ToString)
                qry += " ,MAXMC = " & Val(txtMc.Text.ToString)
            End If

        Else

            If txtwast.Text <> "" Or txtwastper.Text <> "" Then
                qry += " MAXWASTPER = " & Val(txtwastper.Text.ToString)
                qry += " ,MAXWAST = " & Val(txtwast.Text.ToString)
            ElseIf txtMc.Text <> "" Or txtMcGrm.Text <> "" Then
                qry += " MAXMCGRM = " & Val(txtMcGrm.Text.ToString)
                qry += " ,MAXMC = " & Val(txtMc.Text.ToString)
            End If
        End If

        qry += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"

        Return qry
    End Function
End Class