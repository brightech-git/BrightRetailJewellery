Public Class MoreOptions
    Private Sub MoreOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dudLimit.Items.Clear()
        For cnt As Integer = 1 To 100
            dudLimit.Items.Add(cnt.ToString)
        Next
    End Sub
End Class