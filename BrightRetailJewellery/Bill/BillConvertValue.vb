Public Class BillConvertValue
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub
    Private Sub BillConvertValue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub txtConvertValue_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtConvertValue_AMT.KeyPress _
    , txtConvertRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub BillConvertValue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtConvertValue_AMT.Select()
    End Sub

    Private Sub txtConvertWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtConvertWeight_WET.KeyPress
        e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.Close()
        End If
    End Sub

    Private Sub ConvetWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    txtConvertRate_AMT.TextChanged, _
    txtConvertWeight_WET.TextChanged
        CalcConvertWeight()
    End Sub
    Private Sub CalcConvertWeight()
        Dim wt As Double = Nothing
        If Val(txtConvertRate_AMT.Text) > 0 Then
            wt = Val(txtConvertValue_AMT.Text) / Val(txtConvertRate_AMT.Text)
        End If
        txtConvertWeight_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub
End Class