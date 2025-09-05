Public Class frmFinalDiscEst
    Public mtaxvalue As Decimal = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmFinalDiscEst_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    End Sub

    Private Sub frmFinalDiscEst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtGrs_Amt.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmFinalDiscEst_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVat_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtGrs_Amt.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    'Private Sub txtSaDiscper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrs_Amt.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then txtVat_AMT.Focus()
    'End Sub


    Private Sub txtGrs_Amt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGrs_Amt.TextChanged

    End Sub

    Private Sub rbtVbcYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtVbcYes.CheckedChanged
        If rbtVbcYes.Checked = True Then txtVat_AMT.Text = 0 : txtNet_AMT.Text = txtGrs_Amt.Text Else txtVat_AMT.Text = mtaxvalue : txtNet_AMT.Text = txtGrs_Amt.Text + mtaxvalue
        'txtGrs_Amt.Focus()
    End Sub

    Private Sub rbtVbcYes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtVbcYes.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtGrs_Amt.Focus()
    End Sub

    Private Sub rbtVbcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtVbcNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtNet_AMT.Focus()
    End Sub

End Class