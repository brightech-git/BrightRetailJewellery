Public Class frmGiriPwd
    Dim strSql As String
    Private Sub frmAdminPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "AUTHORIZE PASSWORD"
        txtPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim pwd As String = "giri@54321"
            If pwd = Nothing Then
                MsgBox("Authorised Password Invalid", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            If Trim(txtPassword.Text.ToString) <> pwd Then
                MsgBox("Authorised Password Invalid", MsgBoxStyle.Information)
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