Public Class frmAuthPassword
    Dim strSql As String
    Dim UserName As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(ByVal User As String)
        InitializeComponent()
        UserName = User
    End Sub
    Private Sub frmAdminPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If UserName <> "" Then Me.Text = UserName.ToString.ToUpper & " PASSWORD"
        txtPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim pwd As String = objGPack.GetSqlValue("SELECT AUTHPWD FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & UserName & "' AND AUTHPWD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'", , Nothing)
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
End Class