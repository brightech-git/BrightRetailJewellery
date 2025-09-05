Imports System.Data.OleDb
Public Class EstMargin
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Public MarginId As Integer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub EstMargin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub EstMargin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not cmbVaMargin_OWN.Items.Count > 0 Then
            Me.Close()
        End If
    End Sub

    Private Sub cmbVaMargin_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        cmbVaMargin_OWN.DroppedDown = True
    End Sub

    Private Sub cmbVaMargin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbVaMargin_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " SELECT TOP 1 GETPWD FROM " & cnAdminDb & "..VAMARGIN WHERE MARGINNAME = '" & cmbVaMargin_OWN.Text & "'"
            If objGPack.GetSqlValue(StrSql) = "Y" Then
                StrSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..DISCAUTHORIZE HAVING COUNT(*) > 0"
                If Val(objGPack.GetSqlValue(StrSql, , "0")) = 0 Then
                    MsgBox("Authorize info not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Dim objDisc As New frmDiscAuthorize
                If objDisc.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class