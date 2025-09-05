Imports System.Data.OleDb

Public Class frmProcessTypeSelection
    Dim strSql As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        LoadProcess(cmbProcessType)
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub frmProcessTypeSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter And cmbProcessType.Focused = True Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub frmProcessTypeSelection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbProcessType.Select()
    End Sub
    Private Sub LoadProcess(ByVal Cmb As ComboBox)
        strSql = vbCrLf + " SELECT ORDSTATE_NAME,ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS ORDER BY ORDSTATE_NAME"
        Dim dtMetal As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "ORDSTATE_NAME"
        Cmb.ValueMember = "ORDSTATE_ID"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub
End Class