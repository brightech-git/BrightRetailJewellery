Imports System.Data.OleDb
Public Class FindSubItem
    Dim cmd As OleDbCommand
    Dim strSql As String

    Dim dtSoftKeyss As DataTable

    Public Sub New(ByVal Start As Boolean)
        InitializeComponent()
    End Sub


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        cmbSubItem_Man.Items.Clear()
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItem_Man, True, True)
        cmbSubItem_Man.Focus()
    End Sub

    Private Sub cmbSubItem_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub FindSubItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub
End Class