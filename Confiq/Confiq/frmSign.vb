Imports System.IO
Public Class frmSign
    
    Public Shared Function Encrypt(ByVal Pwd As String) As String
        Dim strEncryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) + (cnt * 2) + 14)
                strEncryptPwd = strEncryptPwd & Chr(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strEncryptPwd

    End Function
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtCompid.Text = "" Then txtCompid.Focus() : Exit Sub
        If txtDbserver.Text = "" Then txtDbserver.Focus() : Exit Sub
        Dim Line1 As String = txtCompid.Text
        Dim Line2 As String = txtDbserver.Text
        Dim Line3 As String = txtDbPath.Text
        Dim Line4 As String = IIf(txtSqlPwd.Text.Trim <> "", Encrypt(txtSqlPwd.Text.Trim), "")
        Dim Line5 As String = IIf(txtSqlPwd.Text.Trim <> "", txtSqluser.Text, Mid(cmbLogintype.Text, 1, 1))
        Dim envName As String
        Dim envValue As String

        envName = "PRJCTRL"
        envValue = Line1 & ";" & Line2 & ";" & Line3 & ";" & Line4 & ";" & Line5
        ' Determine whether the environment variable exists.
        If Environment.GetEnvironmentVariable(envName) Is Nothing Then
            ' If it doesn't exist, create it.
            Environment.SetEnvironmentVariable(envName, envValue, EnvironmentVariableTarget.Machine)
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmSign_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCompid.Focused And Len(txtCompid.Text) <> 3 Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSqlPwd.CharacterCasing = CharacterCasing.Normal
        txtCompid.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class