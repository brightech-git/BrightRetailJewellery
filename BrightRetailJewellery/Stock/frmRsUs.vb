
Public Class frmRsUs
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub frmRsUs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            txtUSDollar_Amt.Focus()
            Me.Close()
        End If
    End Sub

    Private Sub frmRsUs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtIndRs_Amt.Focused Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                txtUSDollar_Amt.Focus()
                Me.Close()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRsUs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtUSDollar_Amt.Focus()
    End Sub
End Class