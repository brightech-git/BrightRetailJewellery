Imports System.Data.OleDb
Public Class frmSubItemSmithLink
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As Integer = Nothing
    Dim chkMetal As String = ""
    Dim chkItem As String = ""
    Dim chkMargin As String = ""
    Dim dtRow As New DataTable
    Dim dtGridView As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dViewRow As DataRow = Nothing
    Dim dAccode As String = Nothing

    Private Sub frmSubItemSmithLink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtGridView.Columns.Add("ITEMNAME")
        dtGridView.Columns.Add("SUBITEMNAME")
        dtGridView.Columns.Add("ACNAME")

        funcLoadItemName()
        funcLoadSubItemName()
        funcLoadAccountName()

        SearchSmith(cmbSmith_OWN)
        SearchItem(cmbItem_OWN)
    End Sub

    Function funcLoadItemName() As Integer
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkListItem)
        SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Function
    Function funcLoadSubItemName() As Integer
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY SUBITEMNAME"
        FillCheckedListBox(strSql, chkListSubItem)
        SetChecked_CheckedList(chkListSubItem, chkSubItemAll.Checked)
    End Function
    Function funcLoadAccountName() As Integer
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ACNAME"
        FillCheckedListBox(strSql, chkListAccode)
        SetChecked_CheckedList(chkListAccode, chkSmithAll.Checked)
    End Function
    Function funcCheckSmith()
        Dim chkSubItemName As String = GetChecked_CheckedList(chkListSubItem)
        If chkListSubItem.CheckedItems.Count > 0 Then
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..SMITHSUBITEMDETAIL WHERE SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & chkSubItemName & "))"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtRow)
            If dtRow.Rows.Count > 0 Then
                dAccode += dtRow.Rows(0).Item("ACCODE").ToString
                Dim orgAccode As String() = dAccode.Split(",")
                dAccode = Nothing
                For i As Integer = 0 To orgAccode.Length - 1
                    dAccode += "'" & orgAccode(i).ToString + "',"
                Next
                If dAccode <> "" Then
                    dAccode = Mid(dAccode, 1, dAccode.Length - 1)
                End If
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (" & dAccode & ") ORDER BY ACNAME"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable()
                dAccode = ""
                dt.Clear()
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    For k As Integer = 0 To dt.Rows.Count - 1
                        For j As Integer = 0 To chkListAccode.Items.Count - 1
                            If dt.Rows(k).Item("ACNAME").ToString = chkListAccode.Items(j) Then
                                chkListAccode.SetItemCheckState(j, CheckState.Checked)
                            End If
                        Next
                    Next
                End If
            End If
        End If
    End Function

    Private Sub frmSubItemSmithLink_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub bnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub bnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnExit.Click, ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        funcCallGrid()
        funcLoadItemName()
        funcLoadSubItemName()
        funcLoadAccountName()
        'funcCallGrid()
        dtRow.Clear()
        For i As Integer = 0 To chkListItem.Items.Count - 1
            chkListItem.SetItemCheckState(i, CheckState.Unchecked)
        Next
        For j As Integer = 0 To chkListSubItem.Items.Count - 1
            chkListSubItem.SetItemCheckState(j, CheckState.Unchecked)
        Next
        For k As Integer = 0 To chkListAccode.Items.Count - 1
            chkListAccode.SetItemCheckState(k, CheckState.Unchecked)
        Next
    End Function

    Private Sub chkListItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListItem.Leave
        Dim chkItemName As String = GetChecked_CheckedList(chkListItem)
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkItemName <> "" Then strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")) "
        strSql += " ORDER BY SUBITEMNAME"
        FillCheckedListBox(strSql, chkListSubItem)
        SetChecked_CheckedList(chkListSubItem, chksubItemAll.Checked)
    End Sub
    Private Sub LoadGridView()
        dtGridView.Clear()
        dViewRow = dtGridView.NewRow()
        For j As Integer = 0 To chkListSubItem.CheckedItems.Count - 1
            If chkListAccode.Items.Count > 0 Then
                For n As Integer = 0 To chkListAccode.CheckedItems.Count - 1
                    dViewRow = dtGridView.NewRow()
                    dViewRow("SUBITEMNAME") = chkListSubItem.CheckedItems(j).ToString
                    dViewRow("ACNAME") = chkListAccode.CheckedItems(n).ToString
                    dtGridView.Rows.Add(dViewRow)
                Next
            Else
                dViewRow = dtGridView.NewRow()
                dViewRow("SUBITEMNAME") = chkListSubItem.CheckedItems(j).ToString
                dViewRow("ACNAME") = ""
                dtGridView.Rows.Add(dViewRow)
            End If
        Next
        dtGridView.AcceptChanges()
        gridShow.DataSource = dtGridView
    End Sub
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function

        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Private Sub funcAdd()
        Dim SubItemId As Integer = Nothing
        Dim Accode As String = Nothing
        Dim dtSubItem, dtAccode As New DataTable()
        dtSubItem.Clear()
        dtAccode.Clear()

        Dim chkAccName As String = GetChecked_CheckedList(chkListAccode)
        Dim chkItemName As String = GetChecked_CheckedList(chkListItem)
        Dim chkSubItemName As String = GetChecked_CheckedList(chkListSubItem)

        If gridShow.Rows.Count > 0 Then
            For i As Integer = 0 To gridShow.Rows.Count - 1

                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName= '" & gridShow("SUBITEMNAME", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtSubItem)
                If dtSubItem.Rows.Count > 0 Then
                    SubItemId = dtSubItem.Rows(i).Item("SUBITEMID").ToString
                Else
                    SubItemId = 0
                End If

                Dim sql As String = "SELECT ACCODE as  ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME= '" & gridShow("ACNAME", i).Value.ToString & "'"
                da = New OleDbDataAdapter(sql, cn)
                Accode = ""
                'dtAccode.Clear()
                da.Fill(dtAccode)
                If dtAccode.Rows.Count > 0 Then
                    Accode = dtAccode.Rows(i).Item("ACCODE").ToString
                End If

                strSql = " INSERT INTO " & cnAdminDb & "..SMITHSUBITEMDETAIL "
                strSql += " (SUBITEMID,ACCODE)"
                strSql += " VALUES("
                strSql += "" & SubItemId & "" 'SubItemId
                strSql += " ,'" & Accode & "')"

                Try
                    ExecQuery(SyncMode.Master, strSql, cn)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            Next
            funcNew()
            MsgBox("Updated Successfully...")
        End If
    End Sub
    Private Sub funcUpdate()
        Dim SubItemId As Integer = Nothing
        Dim Accode As String = Nothing
        Dim dtSubItem, dtAccode As New DataTable()
        dtSubItem.Clear()
        dtAccode.Clear()
        ''Find ItemId

        For i As Integer = 0 To gridShow.Rows.Count - 1

            ''Find SubItemId

            strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName ='" & gridShow("SUBITEMNAME", i).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSubItem)
            If dtSubItem.Rows.Count > 0 Then
                SubItemId = dtSubItem.Rows(i).Item("SubItemid").ToString
            Else
                SubItemId = 0
            End If

            ''Find ACCODE
            strSql = " Select Accode from " & cnAdminDb & "..Achead where AcName ='" & gridShow("ACNAME", i).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAccode)
            If dtAccode.Rows.Count > 0 Then
                Accode = dtAccode.Rows(i).Item("Accode")
            Else
                Accode = ""
            End If

            strSql = " Update " & cnAdminDb & "..SMITHSUBITEMDETAIL Set"
            strSql += " Accode='" & Accode & "'"
            strSql += " where SNO = " & Sno & ""
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End Try
        Next
        funcNew()
    End Sub

    Private Sub bnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSave.Click, SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub chkListSubItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListSubItem.Leave
        funcCheckSmith()
    End Sub

    Private Sub chkListSubItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListSubItem.SelectedIndexChanged

    End Sub

    Private Sub bnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tbMain.SelectedIndex = 1
        tbView.Focus()
        funcCallGrid()
        gridView.Focus()
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS I WHERE I.SUBITEMID=RM.SUBITEMID)AS SUBITEMNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD AS C WHERE C.ACCODE = RM.ACCODE)AS ACNAME"
        strSql += " FROM " & cnAdminDb & "..SMITHSUBITEMDETAIL AS RM"
        funcOpenGrid(strSql, gridView)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("SUBITEMNAME").Width = 250
        gridView.Columns("ACNAME").Width = 150
    End Function

    Private Sub chkListAccode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListAccode.Leave
        LoadGridView()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tbMain.SelectedTab = tbGen
            End If
        End If
    End Sub
    Function funcGetDetails(ByVal temp As Integer) As Integer
        Dim chkSubItem As String = Nothing
        Dim chkAcName As String = Nothing

            strSql = " Select"
        strSql += " isnull((select SubItemName from " & cnAdminDb & "..SubItemMast as s where s.SubItemId=RM.SubItemId),'')as SubItemName,"
        strSql += " isnull((select AcName from " & cnAdminDb & "..AChead as c where c.Accode = RM.Accode),'')as AcName"
        strSql += " from " & cnAdminDb & "..SMITHSUBITEMDETAIL as RM"
        strSql += " Where SNO = " & temp & ""
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        flagSave = True
        flagUpdate = True
        With dt.Rows(0)
            pnlItem.Enabled = True
            pnlSubItem.Enabled = True
            funcLoadItemName()
            funcLoadSubItemName()
            funcLoadAccountName()
            chkSubItem = dt.Rows(0).Item("SUBITEMNAME").ToString
            chkAcName = dt.Rows(0).Item("ACNAME").ToString
            For cnt As Integer = 0 To chkListSubItem.Items.Count - 1
                If chkListSubItem.Items(cnt).ToString = chkSubItem Then
                    chkListSubItem.SetItemChecked(cnt, True)
                    Exit For
                End If
            Next
            For j As Integer = 0 To chkListAccode.Items.Count - 1
                If chkListAccode.Items(j).ToString = chkAcName Then
                    chkListAccode.SetItemChecked(j, True)
                    Exit For
                End If
            Next
        End With
        Sno = temp
    End Function

    Private Sub frmSubItemSmithLink_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.Escape) Then
            tbMain.SelectedIndex = 0
            tbGen.Focus()
        End If
        If e.KeyCode = (Keys.F9) Then
            cmbItem_OWN.Focus()
            cmbSubItem_OWN.Items.Add("ALL")
        End If
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemAll.CheckedChanged
        SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Sub

    Private Sub chkSubItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSubItemAll.CheckedChanged
        SetChecked_CheckedList(chkListSubItem, chkSubItemAll.Checked)
    End Sub

    Private Sub chkSmithAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSmithAll.CheckedChanged
        SetChecked_CheckedList(chkListAccode, chkSmithAll.Checked)
    End Sub

    Private Sub btnSearch_Own_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch_Own.Click
        strSql = " SELECT "
        strSql += " '" & cmbItem_OWN.Text & "' AS ITEMNAME, "
        strSql += " SUBITEMNAME,"
        strSql += " ACNAME AS ACNAME"
        strSql += " FROM " & cnAdminDb & "..SMITHSUBITEMDETAIL AS SR"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S on S.SUBITEMID=SR.SUBITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ACHEAD C on C.ACCODE=SR.ACCODE"
        If UCase(cmbSubItem_OWN.Text) = "ALL" Then
            If cmbItem_OWN.Text <> "" Or cmbSmith_OWN.Text <> "" Then
                strSql += " where S.ItemId=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItem_OWN.Text & "')"
                If cmbSmith_OWN.Text <> "" Then
                    strSql += " and C.ACNAME='" & cmbSmith_OWN.Text & "'"
                End If
                strSql += " Order By S.SubItemName"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("ACNAME").Width = 300
                End With
                gridView.Select()
            End If
            'ElseIf cmbCostSearch.Text <> "" Or (cmbItemSearch.Text <> "" And cmbSubItemSearch.Text <> "") Then
        Else
            If cmbItem_OWN.Text <> "" Or cmbSubItem_OWN.Text <> "" Then
                strSql += " Where S.SubItemName='" & cmbSubItem_OWN.Text & "'"
                If cmbSmith_OWN.Text <> "" Then
                    strSql += " and C.ACNAME='" & cmbSmith_OWN.Text & "'"
                End If
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("ACNAME").Width = 300
                End With
                gridView.Select()
            End If
        End If
    End Sub
    Private Function SearchSmith(ByVal combo As ComboBox) As Integer
        strSql = " Select AcName from " & cnAdminDb & "..AcHeaD order by AcName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        cmbSubItem_Own.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast order by ItemName"
        objGPack.FillCombo(strSql, combo, , False)
        cmbSubItem_Own.Items.Add("ALL")
    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox) As Integer
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItem_Own.Text & "') order by SubItemName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function

    Private Sub cmbItem_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.SelectedIndexChanged
        SearchSubItem(cmbSubItem_OWN)
        cmbSubItem_OWN.Items.Add("ALL")
    End Sub
End Class