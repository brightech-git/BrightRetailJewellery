Public Class PropertyDia
    Public Sub New(ByVal obj As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PropertyGrid1.SelectedObject = obj
        PropertyGrid1.Refresh()
    End Sub

    Private Sub PropertyDia_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        PropertyGrid1.Refresh()
    End Sub

    Private Sub PropertyDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            PropertyGrid1.Refresh()
            Me.Close()
        End If
    End Sub
End Class