Imports System.Data.OleDb
Public Class frmPMSubMast
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
        Dim mpmpgrpid As Integer = Val(objGPack.GetSqlValue("select PMID from " & cnAdminDb & "..pmmast where pmname = '" & cmbPmGroup.Text & "'").ToString)
        strSql = " INSERT INTO " & cnAdminDb & "..PMSUBMAST (PMSUBID,PMID,PMSUBNAME,ACTIVE) VALUES(" & txtGroupId.Text & "," & mpmpgrpid & ",'" & txtGroupName__Man.Text & "','" & Mid(cmbActive.Text, 1, 1) & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnAdminDb & "..PMSUBMAST SET PMSUBNAME = '" & txtGroupName__Man.Text & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " WHERE PMSUBID = '" & txtGroupId.Text & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcGetDetails(ByVal sgroupId As Integer) As Integer
        strSql = " select A.PMID,G.PMNAME,A.PMSUBID,A.PMSUBNAME,CASE WHEN A.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..PMSUBMAST A INNER JOIN " & cnAdminDb & "..PMMAST G ON A.PMID = G.PMID"
        strSql += " WHERE PMSUBID = " & sgroupId & ""
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtGroupId.Text = dt.Rows(0).Item("PMID").ToString
            txtGroupName__Man.Text = dt.Rows(0).Item("PMNAME").ToString
            cmbPmGroup.Text = dt.Rows(0).Item("PMGROUPNAME").ToString
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
        txtGroupId.Text = Val(GetSqlValue(cn, "select isnull(max(Pmsubid),0) from " & cnAdminDb & "..PMSUBMAST")) + 1

        txtGroupName__Man.Focus()
    End Function
    Function funcCallGrid() As Integer
        strSql = " SELECT A.PMID,G.PMNAME,A.PMSUBID,A.PMSUBNAME,case A.ACTIVE WHEN 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE FROM " & cnAdminDb & "..PMSUBMAST A INNER JOIN " & cnAdminDb & "..PMMAST G ON A.PMID = G.PMID ORDER BY PMNAME,PMSUBNAME"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("PMid").Visible = False
        gridView.Columns("PMNAME").MinimumWidth = 250
        gridView.Columns("PMNAME").HeaderText = "ITEM NAME"
        gridView.Columns("PMSUBNAME").MinimumWidth = 300
        gridView.Columns("PMSUBNAME").HeaderText = "MATERIAL SUB NAME"
    End Function

    Private Sub frmPMGROUP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGroupName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmPMGROUP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        strSql = "SELECT PMNAME FROM " & cnAdminDb & "..PMMAST WHERE ISNULL(ACTIVE,'Y') <> 'N'"
        objGPack.FillCombo(strSql, cmbPmGroup, False)
        funcNew()
    End Sub

    Private Sub txtGroupName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGroupName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..PMSUBMAST  WHERE PMSUBNAME = '" & txtGroupName__Man.Text & "' AND PMSUBID <> '" & txtGroupId.Text & "'") Then
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
            If objGPack.DupChecker(txtGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..PMSUBMAST WHERE PMSUBNAME = '" & txtGroupName__Man.Text & "' AND PMID <> '" & txtGroupId.Text & "'") Then
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
                funcGetDetails(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("PMGROUPID").Value.ToString))
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
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("PMID").Value.ToString
        'chkQry += " SELECT TOP 1 PMID FROM " & cnAdminDb & "..PMSUBMAST WHERE PMID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..PMSUBMAST WHERE PMID = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub cmbGroupType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class