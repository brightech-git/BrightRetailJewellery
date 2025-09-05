Imports System.Data.OleDb
Public Class FrmPcsUpdate
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtPCS_NUM.Text = ""
    End Sub
    Private Sub txtPCS_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPCS_NUM.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPCS_NUM.Text <> "" Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                txtPCS_NUM.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub FrmPcsUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub
End Class