Imports System.Data.OleDb
Public Class frmErrorupd
    Dim uid As String
    Dim strsql As String
    Dim cmd As New OleDbCommand

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal uids As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        uid = uids
        strsql = "SELECT SQLTEXT FROM RECEIVESYNC WHERE UID='" & uid.Trim & "'"
        txtQry.Text = GetSqlValue(_Cn1, strsql).ToString
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtQry.Text <> "" Then
            strsql = " UPDATE RECEIVESYNC SET SQLTEXT='" & txtQry.Text.Replace("'", "''") & "' WHERE UID='" & uid & "'"
            cmd = New OleDbCommand(strsql, _Cn1)
            cmd.ExecuteNonQuery()
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class