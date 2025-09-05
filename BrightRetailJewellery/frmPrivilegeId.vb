Public Class frmPrivilegeId
    Dim strSql As String
    Public Privilegetypeid As Integer = 0
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Privilegetypeid = 0
    End Sub


    Private Sub txtPrivilegeid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrivilegeid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPrivilegeid.Text.Trim <> "" Then
                If objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & txtPrivilegeid.Text.Trim & "'", , Nothing) = "" Then
                    MsgBox("Invalid PrivilegeId", MsgBoxStyle.Information)
                    txtPrivilegeid.Focus()
                Else
                    Privilegetypeid = Val(objGPack.GetSqlValue("SELECT PREVILEGETYPEID FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & txtPrivilegeid.Text.Trim & "'", , Nothing))
                    Me.Close()
                End If

            End If
        End If
    End Sub
    Private Sub frmPrivilegeId_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtPrivilegeid.CharacterCasing = CharacterCasing.Normal
        txtPrivilegeid.Text = ""
        txtPrivilegeid.Focus()
    End Sub

    Private Sub frmPrivilegeId_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            Me.Close()
        End If
    End Sub
End Class