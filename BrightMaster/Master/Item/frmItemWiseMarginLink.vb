Imports System.Data.OleDb
Public Class frmItemWiseMarginLink
    'calno 290713 Alter by vasanth On-29/07/13 
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Sno As Integer = Nothing
    Dim chkMetal As String = ""
    Dim chkItem As String = ""
    Dim chkMargin As String = ""
    Dim dtRow As New DataTable
    Dim dtGridView As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dViewRow As DataRow = Nothing
    Dim dMarginId As String = Nothing
    Dim dItemId As String = Nothing
    Private Sub frmItemWiseMarginLink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcLoadMetalName()
        funcLoadCostCentreName()
        funcLoadItemName()
        funcLoadMarginName()
    End Sub
    Function funcLoadItemName() As Integer
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkListItem)
    End Function
    Function funcLoadMetalName() As Integer
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkListMetal)
        'SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Function
    Function funcLoadMarginName() As Integer
        strSql = " SELECT DISTINCT MARGINNAME FROM " & cnAdminDb & "..VAMARGIN "
        strSql += " ORDER BY MARGINNAME"
        FillCheckedListBox(strSql, chkListMargin)
        'SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Function
    Function funcLoadCostCentreName() As Integer
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += " ORDER BY COSTNAME"
        FillCheckedListBox(strSql, chkListCostCentre)
        'SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Function
    Function funcCheckItemMargin()
        Dim chkItemName As String = GetChecked_CheckedList(chkListItem)
        dtRow.Clear()
        dMarginId = Nothing
        If chkListItem.CheckedItems.Count > 0 Then
            strSql = " SELECT MARGINIDs FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtRow)
            If dtRow.Rows.Count > 0 Then
                For m As Integer = 0 To dtRow.Rows.Count - 1
                    dMarginId += dtRow.Rows(m).Item("MARGINIDS").ToString + ","
                Next
                If dMarginId <> "" Then
                    dMarginId = Mid(dMarginId, 1, dMarginId.Length - 1)
                End If
                Dim orgMarginId As String() = dMarginId.Split(",")
                dMarginId = Nothing
                For i As Integer = 0 To orgMarginId.Length - 1
                    dMarginId += "'" & orgMarginId(i).ToString + "',"
                Next
                If dMarginId <> "" Then
                    dMarginId = Mid(dMarginId, 1, dMarginId.Length - 1)
                End If
                strSql = " SELECT DISTINCT MARGINNAME FROM " & cnAdminDb & "..VAMARGIN WHERE MARGINID IN (" & dMarginId & ") ORDER BY MARGINNAME"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable()
                dMarginId = ""
                dt.Clear()
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    For k As Integer = 0 To dt.Rows.Count - 1
                        For j As Integer = 0 To chkListMargin.Items.Count - 1
                            If dt.Rows(k).Item("MARGINNAME").ToString = chkListMargin.Items(j) Then
                                chkListMargin.SetItemCheckState(j, CheckState.Checked)
                            End If
                        Next
                    Next
                End If
            End If
        End If
    End Function
    Function funcCheckMetal()
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)
        dItemId = ""
        dMarginId = ""
        If chkListMetal.CheckedItems.Count > 0 Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & "))"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtmetal As New DataTable()
            da.Fill(dtmetal)
            If dtmetal.Rows.Count > 0 Then
                For i As Integer = 0 To dtmetal.Rows.Count - 1
                    dItemId += dtmetal.Rows(i).Item("ITEMID").ToString + ","
                Next
                If dItemId <> "" Then
                    dItemId = Mid(dItemId, 1, dItemId.Length - 1)
                End If

                strSql = " SELECT MARGINIDs FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN (" & dItemId & ") AND MARGINIDS<>''"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dtRW As New DataTable
                da.Fill(dtRW)
                For i As Integer = 0 To dtRW.Rows.Count - 1
                    dMarginId += dtRW.Rows(i).Item("MARGINIDS").ToString + ","
                    If dMarginId <> "" Then
                        dMarginId = Mid(dMarginId, 1, dMarginId.Length - 1)
                    End If
                    Dim orgMarginId As String() = dMarginId.Split(",")
                    dMarginId = Nothing
                    For j As Integer = 0 To orgMarginId.Length - 1
                        dMarginId += "'" & orgMarginId(j).ToString + "',"
                    Next
                    If dMarginId <> "" Then
                        dMarginId = Mid(dMarginId, 1, dMarginId.Length - 1)
                    End If
                    strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..VAMARGIN WHERE MARGINID IN (" & dMarginId & "))"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim dtCost As New DataTable()
                    dMarginId = ""
                    dtCost.Clear()
                    da.Fill(dtCost)
                    If dtCost.Rows.Count > 0 Then
                        For m As Integer = 0 To dtCost.Rows.Count - 1
                            For o As Integer = 0 To chkListCostCentre.Items.Count - 1
                                If dtCost.Rows(m).Item("COSTNAME").ToString = chkListCostCentre.Items(o) Then
                                    chkListCostCentre.SetItemCheckState(o, CheckState.Checked)
                                End If
                            Next
                        Next
                    End If

                    strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN (" & dItemId & ") AND MARGINIDS IN ('" & dtRW.Rows(i).Item("MARGINIDS").ToString & "') ORDER BY ITEMNAME"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim dts As New DataTable()
                    'dItemId = ""
                    dts.Clear()
                    da.Fill(dts)
                    If dts.Rows.Count > 0 Then
                        For m As Integer = 0 To dts.Rows.Count - 1
                            For o As Integer = 0 To chkListItem.Items.Count - 1
                                If dts.Rows(m).Item("ITEMNAME").ToString = chkListItem.Items(o) Then
                                    chkListItem.SetItemCheckState(o, CheckState.Checked)
                                End If
                            Next
                        Next
                    End If
                Next
            End If
        End If
    End Function

    Private Sub frmItemWiseMarginLink_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub bnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnNew.Click
        funcNew()
    End Sub

    Private Sub bnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnExit.Click
        funcExit()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        funcLoadItemName()
        funcLoadMetalName()
        funcLoadMarginName()
        'funcCallGrid()
        dtRow.Clear()
        chkItemAll.Checked = False
        chkMetalAll.Checked = False
        chkMarginAll.Checked = False
        chkCostCentreAll.Checked = False
        For i As Integer = 0 To chkListItem.Items.Count - 1
            chkListItem.SetItemCheckState(i, CheckState.Unchecked)
        Next
        For j As Integer = 0 To chkListMetal.Items.Count - 1
            chkListMetal.SetItemCheckState(j, CheckState.Unchecked)
        Next
        For k As Integer = 0 To chkListMargin.Items.Count - 1
            chkListMargin.SetItemCheckState(k, CheckState.Unchecked)
        Next
        For l As Integer = 0 To chkListCostCentre.Items.Count - 1
            chkListCostCentre.SetItemCheckState(l, CheckState.Unchecked)
        Next
    End Function

    Private Sub chkListMetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListMetal.Leave
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)
        Dim chkCostName As String = GetChecked_CheckedList(chkListCostCentre)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkListItem)

        strSql = " SELECT DISTINCT MARGINNAME FROM " & cnAdminDb & "..VAMARGIN "
        If chkMetalName <> "" Then strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        'If chkCostName <> "" Then strSql += " WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & ")) "
        strSql += " ORDER BY MARGINNAME"
        FillCheckedListBox(strSql, chkListMargin)
        'SetChecked_CheckedList(chkListSubItem, chksubItemAll.Checked)
        funcCheckMetal()
    End Sub
    Private Sub LoadGridView()
        dtGridView.Clear()
        dViewRow = dtGridView.NewRow()
        For k As Integer = 0 To chkListItem.CheckedItems.Count - 1
            If chkListMargin.Items.Count > 0 Then
                For n As Integer = 0 To chkListMargin.CheckedItems.Count - 1
                    dViewRow = dtGridView.NewRow()
                    dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(k).ToString
                    dViewRow("MARGINNAME") = chkListMargin.CheckedItems.Item(n).ToString
                    dtGridView.Rows.Add(dViewRow)
                Next
            Else
                dViewRow = dtGridView.NewRow()
                'dViewRow("METALNAME") = chkListMetal.CheckedItems.Item(j).ToString
                dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(k).ToString
                'dViewRow("SUBITEMNAME") = chkListSubItem.CheckedItems.Item(k).ToString
                dViewRow("MARGINNAME") = ""
                dtGridView.Rows.Add(dViewRow)
            End If
        Next
        ' Next
        'Next
        dtGridView.AcceptChanges()
        gridShow.DataSource = dtGridView
        'For cnt As Integer = 0 To gridView.ColumnCount - 1
        '    gridShow.Columns(cnt).HeaderText = UCase(gridShow.Columns(cnt).HeaderText)
        'Next
        'End If
    End Sub

    Private Sub bnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSave.Click
        funcSave()
    End Sub
    Function funcSave()
        Dim ItemId As Integer = Nothing
        Dim MarginId As String = Nothing
        Dim dtItem, dtMargin, DtCostCentre As New DataTable()
        dtItem.Clear()
        dtMargin.Clear()

        'LoadGridView()
        Dim chkMarginName As String = GetChecked_CheckedList(chkListMargin)
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkListItem)
        Dim chkCostname As String = GetChecked_CheckedList(chkListCostCentre)

        If chkMarginName = "" Then
            MsgBox("Invalid Margin...")
            Exit Function
        End If
        If chkItemName = "" Then
            MsgBox("Invalid Item...")
            Exit Function
        End If
        If chkCostname <> "" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostname & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DtCostCentre)
        End If
        If chkItemName <> "" Then
            strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName in (" & chkItemName & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then
                For i As Integer = 0 To dtItem.Rows.Count - 1
                    If chkMetalName <> "" Then
                        If chkMarginName <> "" Then
                            Dim sql As String = "SELECT DISTINCT MARGINID as  MARGINID FROM " & cnAdminDb & "..VAMARGIN WHERE MARGINNAME IN (" & chkMarginName & ") AND METALID IN ( SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & " ))"
                            da = New OleDbDataAdapter(sql, cn)
                            MarginId = ""
                            dtMargin.Clear()
                            da.Fill(dtMargin)
                            If dtMargin.Rows.Count > 0 Then
                                For j As Integer = 0 To dtMargin.Rows.Count - 1
                                    MarginId += dtMargin.Rows(j).Item("MARGINID").ToString + ","
                                Next
                                If MarginId <> "" Then
                                    MarginId = Mid(MarginId, 1, MarginId.Length - 1)
                                End If
                            End If

                            strSql = " Update " & cnAdminDb & "..ITEMMAST Set"
                            strSql += " MarginIds= '" & MarginId & "'"
                            strSql += " where ItemId = " & Val(dtItem.Rows(i).Item("ITEMID").ToString) & ""
                            Try
                                'calno 290713
                                If DtCostCentre.Rows.Count > 0 Then
                                    For k As Integer = 0 To DtCostCentre.Rows.Count - 1
                                        ExecQuery(SyncMode.Stock, strSql, cn, , DtCostCentre.Rows(k).Item("COSTID").ToString)
                                    Next
                                Else
                                    ExecQuery(SyncMode.Master, strSql, cn, )
                                End If
                            Catch ex As Exception
                                MsgBox(ex.Message)
                                MsgBox(ex.StackTrace)
                            End Try
                        End If
                    End If
                Next
                funcNew()
                MsgBox("Updated Successfully...")
            End If
        End If
    End Function
    Private Sub chkListItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListItem.Leave
        funcCheckItemMargin()
    End Sub

    Private Sub chkMetalAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalAll.CheckedChanged
        SetChecked_CheckedList(chkListMetal, chkMetalAll.Checked)
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemAll.CheckedChanged
        SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Sub

    Private Sub chkMarginAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMarginAll.CheckedChanged
        SetChecked_CheckedList(chkListMargin, chkMarginAll.Checked)
    End Sub

    Private Sub chkCostCentreAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCostCentreAll.CheckedChanged
        SetChecked_CheckedList(chkListCostCentre, chkCostCentreAll.Checked)
    End Sub

    Private Sub chkListCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListCostCentre.Leave
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)
        Dim chkCostName As String = GetChecked_CheckedList(chkListCostCentre)
        strSql = " SELECT DISTINCT MARGINNAME FROM " & cnAdminDb & "..VAMARGIN "
        If chkMetalName <> "" And chkCostName <> "" Then
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
            strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & ")) "
        ElseIf chkCostName <> "" And chkMetalName = "" Then
            strSql += " WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & ")) "
        ElseIf chkMetalName <> "" And chkCostName = "" Then
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        End If
        strSql += " ORDER BY MARGINNAME"
        FillCheckedListBox(strSql, chkListMargin)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
End Class