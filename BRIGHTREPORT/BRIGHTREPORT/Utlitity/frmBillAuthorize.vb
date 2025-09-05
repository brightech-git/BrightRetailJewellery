Imports System.Data.OleDb
Imports System.IO
Imports System.Globalization
Public Class frmbillauthorize
    Dim strSql As String
    Dim dt As DataTable
    Dim da As New OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim sysid As String
    Dim GridRowIndex As Integer = Nothing
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        funcview()
    End Sub

    Private Sub Bill_Authorize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridview_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            gridview_OWN.Focus()
            gridview_OWN.CurrentCell = gridview_OWN.Rows(GridRowIndex).Cells("btnAuth")
            GridRowIndex = Nothing
        End If
    End Sub
    Function funcview()
        strSql = Nothing
        strSql = "SELECT PWDID,PWDDATE AS REQUESTDATE"
        If cmbtype.Text = "ADVANCE" Then
            strSql += " ,REFNO AS ADVANCENO  "
        Else
            strSql += " ,GROUPCODE,REGNO"
        End If
        strSql += " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=P.CRUSERID)REQUESTBY"
        strSql += " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=P.USERID)AUTHORIZEBY"
        strSql += " ,DAYS"
        strSql += " FROM " & cnAdminDb & "..PWDAUTHORIZE P WHERE  1=1 "
        If rbtAuth.Checked Then
            strSql += " AND ISNULL(AUTHORIZE,'N')='Y'"
        ElseIf rbtNotAuth.Checked Then
            strSql += " AND ISNULL(AUTHORIZE,'N')='N'"
        End If
        If cmbtype.Text <> "ALL" Then
            strSql += " AND PWDTYPE='" & Mid(cmbtype.Text, 1, 1) & "'"
        End If
        strSql += " ORDER BY PWDID DESC"
        gridview_OWN.DataSource = Nothing
        gridview_OWN.Columns.Clear()
        Me.Refresh()
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        Else
            gridview_OWN.DataSource = dtGridView
        End If
        Dim dgvBtnAuthorize As New DataGridViewButtonColumn
        dgvBtnAuthorize.Name = "btnAuth"
        dgvBtnAuthorize.Width = gridview_OWN.RowTemplate.Height
        dgvBtnAuthorize.Text = "Authorize"
        dgvBtnAuthorize.HeaderText = "Authorize"
        dgvBtnAuthorize.Width = 90
        'dgvBtnAuthorize.
        dgvBtnAuthorize.UseColumnTextForButtonValue = True
        If cmbtype.Text = "ADVANCE" Then
            gridview_OWN.Columns.Insert(6, dgvBtnAuthorize)
        Else
            gridview_OWN.Columns.Insert(7, dgvBtnAuthorize)
        End If
        BrighttechPack.FormatGridColumns(gridview_OWN)
        gridview_OWN.RowTemplate.Height = 30
        gridview_OWN.Select()
        If gridview_OWN.RowCount > 0 Then
            gridview_OWN.CurrentCell = gridview_OWN.Rows(0).Cells("btnAuth")
        End If
        With gridview_OWN
            .Columns("REQUESTDATE").HeaderText = "REQUEST DATE"
            .Columns("REQUESTDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("REQUESTBY").HeaderText = "REQUEST BY"
            .Columns("AUTHORIZEBY").HeaderText = "AUTHORIZE BY"
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            For I As Integer = 0 To .Rows.Count - 1
                If .Rows(I).Cells("AUTHORIZEBY").Value.ToString <> "" Then
                    .Rows(I).DefaultCellStyle.ForeColor = Color.Green
                Else
                    .Rows(I).DefaultCellStyle.ForeColor = Color.Red
                End If
            Next
        End With
    End Function

    Private Sub Bill_Authorize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FUNCLOAD()
    End Sub
    Function FUNCLOAD()
        cmbtype.Items.Clear()
        cmbtype.Items.Add("SCHEME")
        cmbtype.Items.Add("ADVANCE")
        cmbtype.Items.Add("PAYMENT")
        cmbtype.SelectedIndex = 0
        gridview_OWN.DataSource = Nothing
    End Function
    Private Sub gridview_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridview_OWN.CellClick
        If UCase(gridview_OWN.Columns(e.ColumnIndex).Name) = "BTNAUTH" Then
            If gridview_OWN.Rows(e.RowIndex).Cells("AUTHORIZEBY").Value.ToString <> "" Then MsgBox("Already Authorized...", MsgBoxStyle.Information) : Exit Sub
            GridRowIndex = gridview_OWN.CurrentRow.Index
            Dim flag As Boolean = False
            Dim DiffDays As Integer = Val(gridview_OWN.Rows(e.RowIndex).Cells("DAYS").Value.ToString)
            strSql = "SELECT USERID FROM " & cnAdminDb & "..BILLAUTHORIZE WHERE " & DiffDays & " BETWEEN FROMDAYS AND TODAYS"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If Val(dt.Rows(i).Item("USERID").ToString) = userId Then
                        flag = True
                        Exit For
                    End If
                Next
            Else
                MsgBox("Not Valid user to Authorize...", MsgBoxStyle.Information)
                Exit Sub
            End If
            If flag = False Then MsgBox("Not Valid user to Authorize...", MsgBoxStyle.Information) : Exit Sub
            strSql = "UPDATE " & cnAdminDb & "..PWDAUTHORIZE SET"
            strSql += " AUTHORIZE='Y'"
            strSql += " ,USERID=" & userId
            strSql += " WHERE PWDID= '" & Val(gridview_OWN.Rows(e.RowIndex).Cells("PWDID").Value.ToString) & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Authorized Successfully...", MsgBoxStyle.Information)
            funcview()
        End If
    End Sub
End Class
