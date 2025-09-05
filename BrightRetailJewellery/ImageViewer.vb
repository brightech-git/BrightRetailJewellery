
Public Class ImageViewer

    Private Sub StrechToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StrechToolStripMenuItem.Click
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub ZoomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomToolStripMenuItem.Click
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

    Private Sub AutoSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoSizeToolStripMenuItem.Click
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize
    End Sub

    Private Sub CentreImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CentreImageToolStripMenuItem.Click
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
    End Sub
End Class