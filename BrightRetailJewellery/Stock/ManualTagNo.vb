Public Class ManualTagNo
    Public ReturnValue As String

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ManualTagNo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtTagNo.Focus()
    End Sub

    Private Sub ManualTagNo_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If IIf(IsDBNull(txtTagNo.Text), "", txtTagNo.Text) = "" Then
                MsgBox("Tag No should not empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If IIf(IsDBNull(txtTagNo.Text), "", txtTagNo.Text) = "" Then
            MsgBox("Tag No should not empty", MsgBoxStyle.Information)
            txtTagNo.Focus()
            Exit Sub
        End If
        If Val(GetSqlValue(cn, "SELECT COUNT(*)CNT FROM " + cnAdminDb + "..ITEMTAG WHERE TAGNO = '" + txtTagNo.Text + "'")) > 0 Then
            MsgBox("Tag No Already Exist", MsgBoxStyle.Information)
            txtTagNo.Focus()
            Exit Sub
        End If

        ReturnValue = txtTagNo.Text
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class