Imports System.Data.OleDb
Public Class frmNarration
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim narrationId As Integer = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " Select "
        strSql += " NarrID,Narration,"
        strSql += " case when ModuleId = 'S' then 'STOCK'"
        strSql += " when ModuleId = 'P' then 'POINT OF SALES' else 'FINANCE' end ModuleId"
        strSql += " from " & cnAdminDb & "..Narration"
        funcOpenGrid(strSql, gridView)
        gridView.Columns("NARRID").Visible = False
        gridView.Columns("NARRATION").MinimumWidth = 300
        If gridView.RowCount > 0 Then
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
            Next
        End If
    End Function
    Function funcNew() As Integer
        cmbModuleId.Text = "FINANCE"
        objGPack.TextClear(Me)
        narrationId = Nothing
        flagSave = False
        funcCallGrid()
        txtNarrationName__Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtNarrationName__Man, "SELECT 1 FROM " & cnAdminDb & "..NARRATION WHERE NARRATION = '" & txtNarrationName__Man.Text & "' AND NARRID <> '" & narrationId & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        strSql = " Insert into " & cnAdminDb & "..Narration"
        strSql += " ("
        strSql += " Narration,ModuleId,"
        strSql += " UserId,Updated,Uptime"
        strSql += " )values("
        strSql += " '" & txtNarrationName__Man.Text & "'" 'Narration
        strSql += " ,'" & Mid(cmbModuleId.Text, 1, 1) & "'" 'ModuleId
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As OleDbException
            If cn.State = ConnectionState.Open Then

            End If
            If ex.ErrorCode = 2627 Then
                MsgBox("Narration Name Already Exist", MsgBoxStyle.Information)
                txtNarrationName__Man.Focus()
            Else
                If cn.State = ConnectionState.Open Then

                End If
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            If cn.State = ConnectionState.Open Then

            End If
        End Try
    End Function
    Function funcUpdate() As Integer
        strSql = " Update " & cnAdminDb & "..Narration Set"
        strSql += " Narration='" & txtNarrationName__Man.Text & "'"
        strSql += " ,ModuleId='" & Mid(cmbModuleId.Text, 1, 1) & "'"
        strSql += " ,UserId='" & userId & "'"
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " where NarrID = '" & narrationId & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As OleDbException
            If cn.State = ConnectionState.Open Then

            End If
            If ex.ErrorCode = 2627 Then
                MsgBox("Narration Name Already Exist", MsgBoxStyle.Information)
                txtNarrationName__Man.Focus()
            Else
                If cn.State = ConnectionState.Open Then

                End If
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If

            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            If cn.State = ConnectionState.Open Then

            End If
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " Select "
        strSql += " Narration,"
        strSql += " case when ModuleId = 'S' then 'STOCK'"
        strSql += " when ModuleId = 'P' then 'POINT OF SALES' else 'FINANCE' end ModuleId"
        strSql += " from " & cnAdminDb & "..Narration"
        strSql += " Where NarrId = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtNarrationName__Man.Text = .Item("Narration").ToString
            cmbModuleId.Text = .Item("ModuleId").ToString
        End With

        flagSave = True
        narrationId = temp
    End Function

    Private Sub frmNarration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtNarrationName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmNarration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbModuleId.Items.Add("FINANCE")
        cmbModuleId.Items.Add("STOCK")
        cmbModuleId.Items.Add("POINT OF SALES")
        cmbModuleId.Text = "FINANCE"
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
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

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.Rows.Count > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
            End If
            txtNarrationName__Man.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            txtNarrationName__Man.Focus()
        End If
    End Sub

    Private Sub txtNarrationName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNarrationName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtNarrationName__Man, "SELECT 1 FROM " & cnAdminDb & "..NARRATION WHERE NARRATION = '" & txtNarrationName__Man.Text & "' AND NARRID <> '" & narrationId & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        strSql = " DELETE FROM " & cnAdminDb & "..NARRATION WHERE NARRID = '" & gridView.CurrentRow.Cells("NARRID").Value.ToString & "'"
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                funcCallGrid()
            End If
        End If
    End Sub
End Class