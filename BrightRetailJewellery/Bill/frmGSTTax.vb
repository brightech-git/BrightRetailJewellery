Imports System.Data.OleDb
Public Class frmGSTTax
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim gstType As String
    Dim _accStateId As Integer
    Dim _CompStateId As Integer
    Dim _gstId As Integer
    Public Sub New(ByVal taxType As String, ByVal stateId As Integer, ByVal CompstateId As Integer)
        Me.gstType = taxType
        Me._accStateId = stateId
        Me._CompStateId = CompstateId
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmGSTTax_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            btnOk_Click(Me, e)
        End If
    End Sub

    Private Sub frmGSTTax_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtSgst_per_AMT.Focus()
        LoadGstCategory(CmbGstCategory)
    End Sub

    Private Sub txtIgst_per_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIgst_per_AMT.GotFocus
        If Val(txtSgst_per_AMT.Text) > 0 Or Val(txtCgst_per_AMT.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LoadGstCategory(ByVal Cmb As ComboBox)
        StrSql = " SELECT GSTCATNAME,GSTPER FROM " & cnAdminDb & "..GSTCATEGORY WHERE RD='" & gstType & "'"
        StrSql += " ORDER BY GSTCATID"
        Dim dtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "GSTCATNAME"
        Cmb.ValueMember = "GSTPER"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
        StrSql = "SELECT TOP 1 GSTCATNAME FROM " & cnAdminDb & "..GSTCATEGORY WHERE RD='" & gstType & "'"
        Cmb.Text = objGPack.GetSqlValue(StrSql)
    End Sub

    Private Sub CmbGstCategory_Leave(sender As Object, e As System.EventArgs) Handles CmbGstCategory.Leave
        If _accStateId = _CompStateId Then
            txtSgst_per_AMT.Text = Format(Val(CmbGstCategory.SelectedValue / 2), "0.00")
            txtCgst_per_AMT.Text = Format(Val(CmbGstCategory.SelectedValue / 2), "0.00")
        Else
            txtIgst_per_AMT.Text = Format(Val(CmbGstCategory.SelectedValue), "0.00")
        End If
    End Sub
End Class