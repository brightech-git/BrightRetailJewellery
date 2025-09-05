Public Class BillKatchaFtDetail
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub txtSaTouch_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaTouch_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSaTouch_AMT.Text) = 0 Then
                MsgBox("Touch should not empty", MsgBoxStyle.Information)
                txtSaTouch_AMT.Focus()
            ElseIf Val(txtSaTouch_AMT.Text) > 112 Then
                MsgBox("Touch should not exceed the value 112", MsgBoxStyle.Information)
                txtSaTouch_AMT.Focus()
            Else
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub txtSaPureWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaPureWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSaPureWt_WET.Text) = 0 Then
                MsgBox("Weight should not empty", MsgBoxStyle.Information)
                txtSaPureWt_WET.Focus()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub BillKatchaFtDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class