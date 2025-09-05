Imports System.Data.OleDb
Public Class frmRoleWiseMargin
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Sno As Integer = Nothing
    Dim chkMetal As String = ""
    Dim chkRole As String = ""
    Dim chkMargin As String = ""
    Dim dtRow As New DataTable
    Dim dtGridView As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dViewRow As DataRow = Nothing
    Dim dMarginId As String = Nothing

    Private Sub frmRoleMargin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcLoadMetalName()
        funcLoadRoleName()
        funcLoadMarginName()
    End Sub
    Function funcLoadRoleName() As Integer
        strSql = " SELECT ROLENAME FROM " & cnAdminDb & "..ROLEMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ROLENAME"
        FillCheckedListBox(strSql, chkListRole)
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
    Function funcCheckRoleMargin()
        Dim chkRoleName As String = GetChecked_CheckedList(chkListRole)
        If chkListRole.CheckedItems.Count > 0 Then
            strSql = " SELECT MARGINIDs FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME IN (" & chkRoleName & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtRow)
            If dtRow.Rows.Count > 0 Then
                dMarginId += dtRow.Rows(0).Item("MARGINIDS").ToString
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

    Private Sub frmRoleMargin_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
        funcLoadRoleName()
        funcLoadMetalName()
        funcLoadMarginName()
        'funcCallGrid()
        dtRow.Clear()
        For i As Integer = 0 To chkListRole.Items.Count - 1
            chkListRole.SetItemCheckState(i, CheckState.Unchecked)
        Next
        For j As Integer = 0 To chkListMetal.Items.Count - 1
            chkListMetal.SetItemCheckState(j, CheckState.Unchecked)
        Next
        For k As Integer = 0 To chkListMargin.Items.Count - 1
            chkListMargin.SetItemCheckState(k, CheckState.Unchecked)
        Next
    End Function

    Private Sub chkListMetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListMetal.Leave
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)

        strSql = " SELECT DISTINCT MARGINNAME FROM " & cnAdminDb & "..VAMARGIN "
        If chkMetalName <> "" Then strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        strSql += " ORDER BY MARGINNAME"
        FillCheckedListBox(strSql, chkListMargin)
    End Sub

    Private Sub bnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSave.Click, FindToolStripMenuItem.Click
        Dim RoleId As Integer = Nothing
        Dim MarginId As String = Nothing
        Dim dtRole, dtMargin As New DataTable()
        dtRole.Clear()
        dtMargin.Clear()

        'LoadGridView()
        Dim chkMarginName As String = GetChecked_CheckedList(chkListMargin)
        Dim chkMetalName As String = GetChecked_CheckedList(chkListMetal)
        Dim chkRoleName As String = GetChecked_CheckedList(chkListRole)

        strSql = " Select RoleId from " & cnAdminDb & "..RoleMaster where RoleName in (" & chkRoleName & ")"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtRole)
        If dtRole.Rows.Count > 0 Then
            For i As Integer = 0 To dtRole.Rows.Count - 1
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

                strSql = " Update " & cnAdminDb & "..ROLEMASTER Set"
                strSql += " MarginIds= '" & MarginId & "'"
                strSql += " where RoleId = " & Val(dtRole.Rows(i).Item("ROLEID").ToString) & ""
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

    Private Sub chkListRole_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListRole.Leave
        funcCheckRoleMargin()
    End Sub
End Class