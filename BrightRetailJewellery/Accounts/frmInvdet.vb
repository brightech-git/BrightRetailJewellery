Imports System.Data.OleDb
Public Class frmInvdet
    Public DtTdsCategory As New DataTable
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String
    Public oneTimeClear As Boolean = False
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        txtInvno.Focus()
    End Sub
    Private Sub frmAccTds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter And dtpInvDate.Focused Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub txtTdsPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtInvno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dtpInvDate.Focus()
        End If
    End Sub
    Private Sub frmAccTds_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If oneTimeClear = False Then
            txtInvno.Text = ""
        End If
        txtInvno.Select()
        dtpInvDate.Value = Today.Date
        'cmbTdsCategory_OWN.Select()
    End Sub
    Private Sub dtpInvDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpInvDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub dtpInvDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpInvDate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class