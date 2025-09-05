Public Class frmB4Discount
    Public Iwt As Decimal
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmB4Discount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtSADiscount_AMT.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtSADiscount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSADiscount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtSADiscount_AMT.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtSaDiscper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaDiscper.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtSADiscount_AMT.Focus()
    End Sub

    Private Sub txtSaDiscper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaDiscper.TextChanged
        txtSADiscount_AMT.Text = Val(Grsamt.Text) * (Val(txtSaDiscper.Text) / 100)
    End Sub

    Private Sub txtDiscgm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiscgm.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtSADiscount_AMT.Focus()
    End Sub

    Private Sub txtDiscgm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscgm.TextChanged
        txtSADiscount_AMT.Text = Iwt * Val(txtDiscgm.Text)
    End Sub
End Class