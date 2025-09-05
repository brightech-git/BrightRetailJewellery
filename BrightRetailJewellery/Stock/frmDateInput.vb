Imports System.Data.OleDb
Public Class frmDateInput
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.


    End Sub
    Private Sub frmBviewadv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Or e.KeyCode = Keys.Enter Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub





    Private Sub frmdateinput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.dtpBillDatef.Value = Today.Date
        Me.dtpBillDatef.Select()
    End Sub



End Class