Public Class TagMaxMinValues
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub
    Private Sub TagMaxMinValues_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub TagMaxMinValues_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMinMkCharge_Amt.Focused Then
                Me.Close()
            ElseIf txtMinMkCharge_Amt.Enabled = False And txtMinMcPerGram_Amt.Focused Then
                Me.Close()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub TagMaxMinValues_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' txtMinWastage_Per.Focus()
    End Sub
End Class