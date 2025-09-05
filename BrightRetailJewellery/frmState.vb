Public Class frmState

    Private Sub frmState_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub rbtOwn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOwn.GotFocus
        rbtOwn.BackColor = Color.LightGreen
    End Sub

    Private Sub rbtOwn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOwn.LostFocus
        rbtOwn.BackColor = Color.Lavender
    End Sub

    Private Sub rbtOther_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOther.GotFocus
        rbtOther.BackColor = Color.LightGreen
    End Sub

    Private Sub rbtOther_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOther.LostFocus
        rbtOther.BackColor = Color.Lavender
    End Sub
End Class