Public Class frmGiriPass
    Dim strSql As String
    Public pwd As String = "Gsexpress@90"
    Private Sub frmAdminPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "PassWord"
        txtPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If pwd = Nothing Then
                MsgBox("INVALID ACCESS CODE", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            If Trim(txtPassword.Text.ToString) <> pwd Then
                MsgBox("INVALID ACCESS CODE", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class