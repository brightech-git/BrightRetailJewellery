Public Class frmAdminNewPassword
    Private Sub txtNewPwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewPwd.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtRetypePwd.Focus()
        End If
    End Sub

    Private Sub txtNewPwd_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewPwd.Leave
        If txtNewPwd.Text = "" Then
            MsgBox("Password should not empty", MsgBoxStyle.Information)
            txtNewPwd.Focus()
        End If
    End Sub

    Private Sub txtRetypePwd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRetypePwd.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRetypePwd.Text = "" Then
                MsgBox("Password should not empty", MsgBoxStyle.Information)
                txtRetypePwd.Focus()
                Exit Sub
            End If
            If txtNewPwd.Text <> txtRetypePwd.Text Then
                MsgBox("Password mismatch", MsgBoxStyle.Information)
                txtRetypePwd.Clear()
                txtNewPwd.Clear()
                txtNewPwd.Focus()
                Exit Sub
            End If
            If txtAuthPassword.Visible = False Then
                If e.KeyChar = Chr(Keys.Enter) Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            Else
                txtAuthPassword.Focus()
            End If

        End If
    End Sub

    Private Sub frmAdminNewPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub frmAdminNewPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtNewPwd.CharacterCasing = CharacterCasing.Normal
        txtRetypePwd.CharacterCasing = CharacterCasing.Normal
        txtAuthPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub txtAuthPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAuthPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAuthPassword.Text = "" Then
                MsgBox("Authorised Password should not empty", MsgBoxStyle.Information)
                txtAuthPassword.Focus()
                Exit Sub
            End If
            'If txtNewPwd.Text <> txtRetypePwd.Text Then
            '    MsgBox("Password mismatch", MsgBoxStyle.Information)
            '    txtRetypePwd.Clear()
            '    txtNewPwd.Clear()
            '    txtNewPwd.Focus()
            '    Exit Sub
            'End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class



