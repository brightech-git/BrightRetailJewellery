Public Class frmDefEmpId
    Dim EMPID_SUPPRESS As Boolean = False
    Dim PreviousKeyPressTime As DateTime = Nothing

    Private Sub txtDefEmpId_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDefEmpId_NUM.KeyDown

    End Sub

    Private Sub txtDefEmpId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDefEmpId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtDefEmpId_NUM.Text.ToString = "" Or Val(txtDefEmpId_NUM.Text.ToString) <= 0 Then
                txtDefEmpId_NUM.Focus()
            Else
                Me.Close()
            End If
        Else
            If EMPID_SUPPRESS Then
                If PreviousKeyPressTime = Nothing Then
                    PreviousKeyPressTime = DateTime.Now
                End If
                Dim startTime As DateTime = Now
                Dim runLength As Global.System.TimeSpan = startTime.Subtract(CType(PreviousKeyPressTime, DateTime))
                Dim millisecs As Integer = runLength.Milliseconds
                Dim secs As Integer = runLength.Seconds
                Dim TotalMiliSecs As Integer = ((secs * 1000) + millisecs)
                If TotalMiliSecs > 50 Then
                    txtDefEmpId_NUM.Text = ""
                    e.KeyChar = ""
                End If
                PreviousKeyPressTime = DateTime.Now
            End If
        End If
    End Sub
    Public Sub New(ByVal Sup As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EMPID_SUPPRESS = Sup
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


End Class