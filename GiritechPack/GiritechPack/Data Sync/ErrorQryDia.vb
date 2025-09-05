Public Class ErrorQryDia

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If GroupBox1.Visible Then
            GroupBox1.Visible = False
            Button1.Text = "Show Error Log"
        Else
            GroupBox1.Visible = True
            Button1.Text = "Hide Error Log"
        End If

    End Sub
End Class