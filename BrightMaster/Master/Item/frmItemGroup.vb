Imports System.Data.OleDb
Public Class frmItemGroup
    Dim strSql As String
    Dim flagSave As Boolean
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Function funcAdd() As Integer

        strSql = " INSERT INTO " & cnAdminDb & "..ITEMGROUPMAST (GROUPID,GROUPNAME,DISPORDER,GROUPTYPE,ACTIVE) VALUES(" & txtGroupId.Text & ",'" & txtGroupName__Man.Text & "'," & Val(txtDispOrder.Text) & ",'" & Mid(cmbGroupType.Text, 1, 1) & "', '" & Mid(cmbActive.Text, 1, 1) & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnAdminDb & "..ITEMGROUPMAST SET GROUPNAME = '" & txtGroupName__Man.Text & "'"
        strSql += " ,DISPORDER = " & Val(txtDispOrder.Text)
        strSql += " ,GROUPTYPE= '" & Mid(cmbGroupType.Text, 1, 1) & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " WHERE GROUPID = '" & txtGroupId.Text & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcGetDetails(ByVal sgroupId As Integer) As Integer
        strSql = " select GROUPID,GROUPNAME,CASE WHEN GROUPTYPE = 'I' THEN 'ITEM' ELSE 'COUNTER' END AS GROUPTYPE,DISPORDER,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..ITEMGROUPMAST"
        strSql += " WHERE GROUPID = " & sgroupId & ""
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtGroupId.Text = dt.Rows(0).Item("GROUPID").ToString
            txtGroupName__Man.Text = dt.Rows(0).Item("GROUPNAME").ToString
            cmbGroupType.Text = dt.Rows(0).Item("GROUPTYPE").ToString
            txtDispOrder.Text = dt.Rows(0).Item("DISPORDER").ToString
            cmbActive.Text = dt.Rows(0).Item("ACTIVE").ToString
            flagSave = True
        End If
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbActive.Text = "YES"
        flagSave = False
        funcCallGrid()
        Dim dt As New DataTable
        txtGroupId.Text = Val(GetSqlValue(cn, "select isnull(max(groupid),0) from " & cnAdminDb & "..Itemgroupmast")) + 1
        txtDispOrder.Text = Val(txtGroupId.Text)
        txtGroupName__Man.Focus()
    End Function
    Function funcCallGrid() As Integer
        strSql = " SELECT GROUPID,GROUPNAME,DISPORDER,case ACTIVE WHEN 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE FROM " & cnAdminDb & "..ITEMGROUPMAST ORDER BY GROUPNAME"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("groupid").Visible = False
        gridView.Columns("GROUPNAME").MinimumWidth = 350
        gridView.Columns("GROUPNAME").HeaderText = "GROUPNAME"
    End Function

    Private Sub frmITEMGROUPMAST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGroupName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmITEMGROUPMAST_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbGroupType.Items.Add("ITEM GROUP")
        cmbGroupType.Items.Add("COUNTER GROUP")

        funcNew()
    End Sub

    Private Sub txtGroupName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGroupName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME = '" & txtGroupName__Man.Text & "' AND GROUPID <> '" & txtGroupId.Text & "'") Then
                Exit Sub
                Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        If gridView.RowCount > 0 Then
            gridView.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        Try
            If objGPack.DupChecker(txtGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME = '" & txtGroupName__Man.Text & "' AND GROUPID <> '" & txtGroupId.Text & "'") Then
                Exit Sub
            End If
            If flagSave = False Then
                funcAdd()
            Else
                funcUpdate()
            End If
            funcCallGrid()
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtGroupName__Man.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("GROUPID").Value.ToString))
                txtGroupName__Man.Focus()
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("GROUPID").Value.ToString
        chkQry += " SELECT TOP 1 GROUPID FROM " & cnAdminDb & "..ITEMMAST WHERE GROUPID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID = '" & delKey & "'")
        funcCallGrid()
    End Sub
End Class