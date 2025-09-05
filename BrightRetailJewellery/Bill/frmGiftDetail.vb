Imports System.Data.OleDb
Public Class frmGiftDetail
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Public Sub New()
        InitializeComponent()
        txtGiftAmt.Focus()
    End Sub
    Private Sub frmGiftDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub frmGiftDetail_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbGvName.Items.Clear()
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE IN ('G') ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbGvName, False, False)
        cmbGvName.Text = ""
        'cmbGvName.Focus()
    End Sub

    Private Sub BtnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOk.Click
        If cmbGvName.Text <> "" Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class