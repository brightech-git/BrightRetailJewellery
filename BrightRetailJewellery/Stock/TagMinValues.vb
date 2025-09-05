Public Class TagMinValues
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub
    Private Sub TagMinValues_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub TagMinValues_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMinMkCharge_Amt.Focused Then
                Me.Close()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub TagMinValues_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtMinWastage_Per.Select()
    End Sub

    Private Sub txtMinWastage_Per_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinWastage_Per.Leave

    End Sub
End Class