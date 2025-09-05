Public Class frmAdminPassword
    Dim strSql As String
    Dim pwdMobileno As String = ""
    Dim pwdType As String = ""
    Dim OPTIONNAME As String = ""
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        'OPTIONNAME = _OPTIONNAME.ToString
        '' add any initialization after the initializecomponent() call.
        'Dim drsms As DataRow = Nothing
        'Dim mobile As String() = Nothing
        'Dim Optionid As String = ""
        'strSql = "SELECT OPTIONID,MOBILENO FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='" + OPTIONNAME + "' AND ACTIVE = 'Y' "
        'drsms = GetSqlRow(strSql, cn)
        'Dim Msg As String = "Pls Share the OTP for " + _OPTIONNAME
        'If Not drsms Is Nothing Then
        '    Optionid = drsms.Item("OPTIONID").ToString
        '    funcGenPwd1(Optionid, drsms("MOBILENO").ToString, Msg.Trim, True)
        'End If
    End Sub

    Private Sub frmAdminPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If cnUserName <> "" Then Me.Text = cnUserName.ToString.ToUpper & " PASSWORD"
        txtPassword.CharacterCasing = CharacterCasing.Normal
        'If pwdType = "S" Then
        '    If pwdMobileno = "" Then
        '        MsgBox("Mobile No not set for " + OPTIONNAME, MsgBoxStyle.Information)
        '    End If
        'End If
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