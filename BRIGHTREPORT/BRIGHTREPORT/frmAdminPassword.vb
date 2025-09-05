Public Class frmAdminPassword
    Dim strSql As String
    Private Sub frmAdminPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If cnUserName <> "" Then Me.Text = cnUserName.ToString.ToUpper & " PASSWORD"
        txtPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim pwd As String = objGPack.GetSqlValue("SELECT AUTHPWD FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cnUserName & "' AND AUTHPWD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'", , Nothing)
            If pwd = Nothing Then
                MsgBox("Authorised Password Invalid", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            If BrighttechPack.Methods.Encrypt(txtPassword.Text) <> pwd Then
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

    Private Sub lblChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged

    End Sub
End Class