Public Class StuddedDeDuctPer
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub StuddedDeDuctPer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtStuddedPer_PER.Select()
            If Val(txtStuddedPer_PER.Text) > 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK Else Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub txtStuddedPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStuddedPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStuddedPer_PER.Select()
            If Val(txtStuddedPer_PER.Text) > 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK Else Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If Val(txtStuddedPer_PER.Text) > 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK Else Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

End Class