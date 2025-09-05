
Public Class frmManualBillNoGen

    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtBillNo_NUM.Text = "" Then
                MsgBox("BillNo Should Not Empty", MsgBoxStyle.Information)
                txtBillNo_NUM.Focus()
            ElseIf Not Val(txtBillNo_NUM.Text) > 0 Then
                MsgBox("Invalid BillNo", MsgBoxStyle.Information)
                txtBillNo_NUM.Focus()
                txtBillNo_NUM.SelectAll()
            Else
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        End If
    End Sub

    Private Sub frmManualBillNoGen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub frmManualBillNoGen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtBillNo_NUM.BackColor = focusColor
        txtBillNo_NUM.Clear()
    End Sub
End Class