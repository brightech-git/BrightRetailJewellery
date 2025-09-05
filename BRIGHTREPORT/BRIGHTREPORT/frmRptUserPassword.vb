Imports System.Data.OleDb
Public Class frmRptUserPassword
    Dim strSql As String
    Public userpwdid As Integer
    Dim OptId As Integer
    Dim pwd As String
    Dim Final_disc_val As String
    Dim MsgStr As String
    Dim MobileNo As String = ""
    Dim showPassword As Boolean = True
    Dim PWDCLOSETIME As Integer = Val(GetAdmindbSoftValue("PWDCLOSETIME", "").ToString)
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing

    Private Sub frmRptUserPassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim TEMPPWD As String = ""
        TEMPPWD = objGPack.GetSqlValue("SELECT ISNULL(PASSWORD,'')PASSWORD FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID = " & userpwdid & " AND ISNULL(PWDSTATUS,'')  NOT IN('C','E') ", , Nothing)
        If TEMPPWD <> "" And TEMPPWD IsNot Nothing Then
            pwd = TEMPPWD
            txtPassword.CharacterCasing = CharacterCasing.Normal
            lblUsrPwd.Text = BrighttechPack.Methods.Decrypt(pwd)
        End If

        If showPassword = False Then
            lblUsrPwd.Visible = False
            Me.Text = "One Time Password"
        End If
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
                txtPassword.Clear()
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
            'Me.Close()
        End If
    End Sub

    Public Sub New(ByVal pwdid As String, ByVal OptionId As Integer, Optional ByVal ShowPwd As Boolean = True, Optional ByVal Mobile As String = Nothing, Optional ByVal Msg As String = Nothing, Optional ByVal Fin_disc As String = Nothing)
        userpwdid = pwdid
        showPassword = ShowPwd
        MobileNo = Mobile
        OptId = OptionId
        MsgStr = Msg
        Final_disc_val = Fin_disc
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class