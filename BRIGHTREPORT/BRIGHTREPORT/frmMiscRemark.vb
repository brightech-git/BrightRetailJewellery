Imports System.Data.OleDb
Public Class frmMiscRemark
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmMiscRemark_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub
    Private Sub frmMiscRemark_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If cmbRemark1_OWN.Text.ToString <> "" Then Exit Sub
        objGPack.TextClear(Me)
        cmbRemark1_OWN.Items.Clear()
        cmbRemark1_OWN.Select() : cmbRemark1_OWN.Focus()
    End Sub

    Private Sub txtRemark1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbRemark1_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.SelectNextControl(cmbRemark1_OWN, True, True, True, True)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class