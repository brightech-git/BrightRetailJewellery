Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        Try
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "yyyy/MM/dd")
        Catch ex As Exception
        End Try
        Dim filePath As String = Application.StartupPath + "\ConInfo.ini"
        If IO.File.Exists(filepath) = False Then
            Dim objSign As New frmSign
            If objSign.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Application.Exit()
                Exit Sub
            End If
        End If
        Dim obj As New BrighttechRetailJewellery.DatabaseCreator
        obj.ShowDialog()
        Application.Exit()
    End Sub
End Class
