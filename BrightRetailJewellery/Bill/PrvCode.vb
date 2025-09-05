Public Class PrvCode
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub GlobalVatPer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtPrivilegeId.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtSaVatPer_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPrivilegeId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtPrivilegeId.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub txtSaVatPer_PER_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrivilegeId.TextChanged

    End Sub
End Class