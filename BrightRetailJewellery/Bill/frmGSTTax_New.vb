Public Class frmGSTTax_New
    Dim strsql As String
    Public DtTdsCategory As New DataTable
    Dim Da As OleDb.OleDbDataAdapter
    Public straccode As Integer
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        strsql = "SELECT TDSCATNAME  AS TDSCAPTION,* FROM " & cnAdminDb & "..TDSCATEGORY ORDER BY DISPLAYORDER,TDSCATNAME"
        Da = New OleDb.OleDbDataAdapter(strsql, cn)
        Da.Fill(DtTdsCategory)
        cmbTdsCategory_OWN.DataSource = DtTdsCategory
        cmbTdsCategory_OWN.DisplayMember = "TDSCAPTION"
        cmbTdsCategory_OWN.ValueMember = "TDSCATID"
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
    End Sub

    Private Sub txtIgst_per_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIgst_per_AMT.GotFocus
        If Val(txtSgst_per_AMT.Text) > 0 Or Val(txtCgst_per_AMT.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbTdsCategory_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTdsCategory_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            strsql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & cmbTdsCategory_OWN.SelectedValue & "'"

            straccode = cmbTdsCategory_OWN.SelectedValue
        End If

    End Sub

    Private Sub cmbTdsCategory_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTdsCategory_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If EditFlag Then Exit Sub
            strsql = " SELECT TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & cmbTdsCategory_OWN.SelectedValue & "'"
            Dim tdsPer As Decimal = Val(objGPack.GetSqlValue(strsql))
            txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), "")


        End If
    End Sub

    Private Sub txtTdsPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTdsPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtTdsAmt_AMT.Focus()

    End Sub

    Private Sub cmbTdsCategory_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTdsCategory_OWN.SelectedIndexChanged

    End Sub
End Class