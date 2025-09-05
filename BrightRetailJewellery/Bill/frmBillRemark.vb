Imports System.Data.OleDb
Public Class frmBillRemark
    Public MiscIssue As Boolean = False
    Public MiscReasonid As Integer = 0
    Public Misreason As Boolean = False
    Dim objAutoCompleteStr As New AutoCompleteStringCollection
    Dim StrSql As String
    Dim Da As OleDbDataAdapter


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmBillRemark_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub txtRemark2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbRemark2_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbReason.Visible = True Then MiscReasonid = Val(cmbReason.SelectedValue.ToString)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub


    Private Sub frmBillRemark_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MiscIssue Then
            objGPack.TextClear(Me)
            cmbRemark1_OWN.Items.Clear()
            cmbRemark2_OWN.Items.Clear()
            StrSql = " SELECT PRONAME FROM " & cnAdminDb & "..PROCESSTYPE "
            StrSql += " WHERE PROTYPE IN ('O') AND PROMODULE = 'P'"
            Dim dt As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            For Each Row As DataRow In dt.Rows
                cmbRemark1_OWN.Items.Add(Row.Item(0).ToString)
                cmbRemark2_OWN.Items.Add(Row.Item(0).ToString)
                objAutoCompleteStr.Add(Row.Item(0).ToString)
            Next
            cmbRemark1_OWN.AutoCompleteCustomSource = objAutoCompleteStr
            cmbRemark1_OWN.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbRemark1_OWN.AutoCompleteSource = AutoCompleteSource.CustomSource

            cmbRemark2_OWN.AutoCompleteCustomSource = objAutoCompleteStr
            cmbRemark2_OWN.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbRemark2_OWN.AutoCompleteSource = AutoCompleteSource.CustomSource

            StrSql = vbCrLf + " SELECT ORDSTATE_NAME,ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS  ORDER BY ORDSTATE_NAME" 'WHERE SMITH ='Y'
            Dim dtMetal As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtMetal)
            cmbReason.DataSource = dtMetal
            cmbReason.DisplayMember = "ORDSTATE_NAME"
            cmbReason.ValueMember = "ORDSTATE_ID"
            cmbReason.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbReason.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
        If Misreason Then cmbReason.Focus() : cmbReason.Select() Else cmbRemark1_OWN.Select() : cmbRemark1_OWN.Focus()
    End Sub

    Private Sub txtRemark1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbRemark1_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.SelectNextControl(cmbRemark1_OWN, True, True, True, True)
        End If
    End Sub

    Private Sub cmbReason_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbReason.KeyDown
        If e.KeyCode = Keys.Enter Then cmbRemark1_OWN.Focus()
    End Sub
End Class