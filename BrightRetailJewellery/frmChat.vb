Imports System.Data.OleDb
Public Class frmChat
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        For cnt As Integer = 0 To chkLstUsers.CheckedItems.Count - 1
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'GIRI_MESSAGE')>0"
            strSql += " BEGIN"
            strSql += " INSERT INTO GIRI_MESSAGE(MSG,SYSTEMID)VALUES('" & txtMsg.Text & "','" & chkLstUsers.CheckedItems.Item(cnt).ToString & "')"
            strSql += " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Next
        txtMsg.Clear()
        chkLstUsers.Focus()
        Me.Close()
    End Sub

    Private Sub frmChat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT DISTINCT SYSTEMID FROM GIRI_USERS"
        'strSql += " WHERE SYSTEMID <> '" & systemId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtUsers As New DataTable
        da.Fill(dtUsers)
        chkLstUsers.Items.Clear()
        For Each ro As DataRow In dtUsers.Rows
            chkLstUsers.Items.Add(ro!SYSTEMID.ToString)
        Next
    End Sub

    Private Sub chkLstUsers_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstUsers.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkLstUsers.CheckedItems.Count > 0 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMsg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMsg.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMsg.Text <> "" Then SendKeys.Send("{TAB}")
        End If
    End Sub
End Class