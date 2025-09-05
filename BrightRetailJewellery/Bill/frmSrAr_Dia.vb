Public Class frmSrAr_Dia

    'Private Sub dtpSRBillDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpSRBillDate.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If txtSRBillNo.Focused Then Exit Sub
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    Private Sub frmSrAr_Dia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Escape Then
            dtpSRBillDate.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmSrAr_Dia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSRBillNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSrAr_Dia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpSRBillDate.Value = GetEntryDate(GetServerDate)
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpSRBillDate.Enabled = chkDate.Checked
    End Sub
End Class