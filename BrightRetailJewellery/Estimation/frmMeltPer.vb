Public Class frmMeltPer
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
            If txtPUMelt_WET.Focused Then Me.Close() : Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub txtPUMeltPer_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUMeltPer_Per.TextChanged
        txtPUMelt_WET.Text = Grweight * (Val(txtPUMeltPer_Per.Text) / 100)
    End Sub

    Private Sub frmMeltPer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'txtPUMeltPer_Per.Focus()
        Me.SelectNextControl(txtPUMeltPer_Per, False, True, True, True)
    End Sub
End Class