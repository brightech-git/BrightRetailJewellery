
Public Class frmOR_ReceiptDia
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub frmRepairReceiptDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Val(txtPcs_NUM.Text) = 0 Then
            MsgBox("Pcs should not empty", MsgBoxStyle.Information)
            txtPcs_NUM.Select()
            Exit Sub
        End If
        If Val(txtGrsWt_WET.Text) = 0 Then
            MsgBox("Grs weight should not empty", MsgBoxStyle.Information)
            txtGrsWt_WET.Select()
            Exit Sub
        End If
        If Val(txtNetWt_WET.Text) = 0 Then
            MsgBox("Net weight should not empty", MsgBoxStyle.Information)
            txtNetWt_WET.Select()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub frmOR_ReceiptDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'objGPack.TextClear(Me)
        txtDustWt_WET.Select()
    End Sub
    Private Sub txtGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrsWt_WET.TextChanged
        Dim grsWt As Double = Val(txtGrsWt_WET.Text)
        txtNetWt_WET.Text = IIf(grsWt <> 0, Format(grsWt, "0.000"), "")
    End Sub
    Private Sub txtNetWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_WET.LostFocus
        If Val(txtNetWt_WET.Text) > Val(txtGrsWt_WET.Text) Then
            Dim grsWt As Double = Val(txtGrsWt_WET.Text)
            txtNetWt_WET.Text = IIf(grsWt <> 0, Format(grsWt, "0.000"), "")
            txtNetWt_WET.Select()
            MsgBox("Net weight should not exeed Grs weight", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub txtNetWt_WET_TextChanged(sender As Object, e As EventArgs) Handles txtNetWt_WET.TextChanged
        Dim ExcessWt As Double = Val(txtNetWt_WET.Text) - (Val(txtRnetWt_WET.Text) - Val(txtDustWt_WET.Text))
        If ExcessWt > 0 Then
            txtExcessWt_WET.Text = IIf(ExcessWt <> 0, Format(ExcessWt, "0.000"), "")
        Else
            txtExcessWt_WET.Text = IIf(ExcessWt <> 0, Format(ExcessWt, "0.000"), "")
        End If

    End Sub
End Class