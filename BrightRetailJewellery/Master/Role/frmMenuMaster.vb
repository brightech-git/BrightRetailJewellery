Imports System.Data.OleDb
Public Class frmMenuMaster
    Private WithEvents _tStrip As New ToolStripMenuItem
    Dim strsql As String
    Dim _cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Private Sub frmMenuMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmMenuMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbbxShortKey.Items.Clear()
        cmbbxShortKey.Items.Add("Yes")
        cmbbxShortKey.Items.Add("No")
        cmbbxShortKey.Text = "Yes"

        txtMenuText_MAN.CharacterCasing = CharacterCasing.Normal
        mnu.Items.Clear()

        For Each mnuStrip As ToolStripMenuItem In Main.MenuStrip1.Items
            If Not mnuStrip.Visible Then Continue For
            Dim mStrip As New ToolStripMenuItem
            mStrip.Name = mnuStrip.Name
            mStrip.Text = mnuStrip.Text
            loadToolStrip(mStrip, mnuStrip)
            mnu.Items.Add(mStrip)
        Next
        cmbFrmType.Items.Add("AKSHAYAMASTERDLL")
        cmbFrmType.Items.Add("AKSHAYARETAILEXE")
        cmbFrmType.Items.Add("AKSHAYAREPORTDLL")

    End Sub

    Private Sub loadToolStrip(ByRef sStrip As ToolStripMenuItem, ByVal dStrip As ToolStripMenuItem)
        For Each item As ToolStripMenuItem In dStrip.DropDownItems
            _tStrip = New ToolStripMenuItem(item.Text, Nothing, AddressOf mnu_Click)
            _tStrip.Name = item.Name
            _tStrip.Text = item.Text
            _tStrip.AccessibleDescription = item.AccessibleDescription
            _tStrip.AccessibleName = item.AccessibleName
            sStrip.DropDownItems.Add(_tStrip)
            loadToolStrip(_tStrip, item)
        Next
    End Sub

    Private Sub mnu_Click(ByVal sender As Object, ByVal e As EventArgs)
        txtMenuId.Text = CType(sender, ToolStripMenuItem).Name
        Dim text As String() = CType(sender, ToolStripMenuItem).Text.Split("(")
        Dim FrmType As String = ""
        If CType(sender, ToolStripMenuItem).AccessibleName <> Nothing Then
            FrmType = CType(sender, ToolStripMenuItem).AccessibleName.ToString
        Else
            FrmType = ""
        End If
        txtMenuText_MAN.Text = text(0)
        If objGPack.GetSqlValue("SELECT ACCESSID FROM " & cnAdminDb & "..PRJMENUS WHERE ACCESSID NOT LIKE '%OWN%' AND MENUID = '" & txtMenuId.Text & "'").Length > 0 Then
            If CType(sender, ToolStripMenuItem).HasDropDownItems = False Then
                txtShortcutKey.Enabled = True
                cmbbxShortKey.Enabled = True
            Else
                cmbbxShortKey.Text = "No"
                txtShortcutKey.Enabled = False
                cmbbxShortKey.Enabled = False
                txtShortcutKey.Clear()
            End If
        Else
            cmbbxShortKey.Text = "No"
            txtShortcutKey.Enabled = False
            cmbbxShortKey.Enabled = False
            txtShortcutKey.Clear()
        End If

        If text.Length > 1 Then txtShortcutKey.Text = text(1)
        If FrmType = "M" Then
            cmbFrmType.Text = "AKSHAYAMASTERDLL"
        ElseIf FrmType = "R" Then
            cmbFrmType.Text = "AKSHAYAREPORTDLL"
        Else
            cmbFrmType.Text = "AKSHAYARETAILEXE"
        End If
        txtMenuId.Focus()
        If cmbFrmType.Text <> "" Then cmbFrmType.Enabled = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        'Validation
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtMenuId.Text = "" Then
            MsgBox("MenuId should not empty. Please select menuitem", MsgBoxStyle.Information)
            Exit Sub
        End If

        'Updation
        strsql = " UPDATE " & cnAdminDb & "..MENUMASTER SET"
        strsql += " MENUTEXT = '" & txtMenuText_MAN.Text & "'"
        strsql += " ,ACCESSKEY = '" & txtShortcutKey.Text & "'"
        strsql += " ,USERID = " & userId & ""
        strsql += " ,UPDATED = '" & GetEntryDate(GetServerDate) & "'"
        strsql += " ,UPTIME = '" & GetServerTime() & "'"
        strsql += " , SHORTCUT = '" & Mid(cmbbxShortKey.Text, 1, 1) & "'"
        If cmbFrmType.Text = "AKSHAYAMASTERDLL" Then
            strsql += " ,REF_DLL = 'AkshayaMaster'"
        ElseIf cmbFrmType.Text = "AKSHAYAREPORTDLL" Then
            strsql += " ,REF_DLL = 'AKSHAYAREPORT'"
        Else
            strsql += " ,REF_DLL = NULL"
        End If
        strsql += " WHERE MENUID = '" & txtMenuId.Text & "'"
        ExecQuery(SyncMode.Master, strsql, cn)
        MsgBox("Updated Successfully", MsgBoxStyle.Information)
        txtMenuId.Clear()
        txtMenuText_MAN.Clear()
        txtShortcutKey.Clear()
        cmbFrmType.Text = ""
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub txtMenuId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMenuId.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
End Class