Public Class frmUserPassword
    Dim strSql As String
    Dim userpwdid As Integer
    Dim pwd As String

    Private Sub frmUserPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If cnUserName <> "" Then Me.Text = cnUserName.ToString.ToUpper & " PASSWORD"
        pwd = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID = " & userpwdid & " and ISNULL(PWDSTATUS,'') <> 'C'", , Nothing)
        txtPassword.CharacterCasing = CharacterCasing.Normal
        lblUsrPwd.Text = BrighttechPack.Methods.Decrypt(pwd)
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If userpwdid = Nothing Then
                MsgBox("Authorised Password Is Empty", MsgBoxStyle.Information)
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
            strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='C',PWDCLSDATE= '" + GetServerDate() + "',PWDCLSTIME='" & GetServerTime() + "'"
            strSql += " WHERE PWDID=" & userpwdid
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            'Update  a set  FROM " & cnAdminDb & "..PWDMASTER a WHERE PWDID = " & userpwdId  
            Me.Close()
        End If
    End Sub

    Private Sub lblChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged

    End Sub



    Public Sub New(ByVal pwdid As String)
        userpwdid = pwdid
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class