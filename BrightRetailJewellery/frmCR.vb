Public Class frmCR
    Public BILLTYPE As String = "B"
    Public IsAdvance As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)

    End Sub

    Private Sub GlobalVatPer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            BILLTYPE = "A"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub rbtCredit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtAdvance.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            BILLTYPE = "A"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub rbtJnd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtJnd.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            BILLTYPE = "C"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmJNDorCR_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rbtAdvance.Checked = True : rbtAdvance.Focus()
    End Sub
End Class