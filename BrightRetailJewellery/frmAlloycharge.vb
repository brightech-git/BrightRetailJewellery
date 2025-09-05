Public Class frmAlloycharge
    Public Grweight As Decimal
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmMeltPer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            If txtAlloy_AMT.Focused Then Me.Close() : Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPUMeltPer_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlloy_Wet.TextChanged
        txtAlloy_AMT.Text = Val(txtAlloy_Wet.Text) * Val(txtAlloyRate_AMT.Text)
    End Sub


    Private Sub txtAlloyRate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlloyRate_AMT.TextChanged
        txtAlloy_AMT.Text = Val(txtAlloy_Wet.Text) * Val(txtAlloyRate_AMT.Text)
    End Sub
End Class