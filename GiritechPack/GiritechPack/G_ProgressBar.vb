Public Class G_ProgressBar
    Private frm As Form
    Private pbar As ProgressBar
    Private statusLable As Control
    Public Sub New(ByVal frm As Form, ByVal pbar As ProgressBar, ByVal statusLable As Control)
        Me.frm = frm
        Me.pbar = pbar
        Me.statusLable = statusLable
    End Sub

    Public Sub ProgressBarShow(Optional ByVal progressStyle As ProgressBarStyle = ProgressBarStyle.Blocks)
        statusLable.Text = ""
        pbar.Style = progressStyle
        pbar.Value = 0
        pbar.Maximum = 100
        pbar.Step = 5
        pbar.Visible = True
        frm.Refresh()
    End Sub

    Public Sub ProgressBarStep(Optional ByVal statusComment As String = Nothing, Optional ByVal stepValue As Integer = 5)
        If pbar.Value >= pbar.Maximum - stepValue Then
            pbar.Value = 0
        Else
            pbar.Value = pbar.Value + stepValue
        End If
        statusLable.Text = statusComment + IIf(statusComment <> Nothing, "....     ", "")
        frm.Refresh()
    End Sub

    Public Sub ProgressBarHide()
        pbar.Value = pbar.Maximum
        frm.Refresh()
        pbar.Visible = False
        statusLable.Text = ""
        frm.Refresh()
    End Sub

End Class
