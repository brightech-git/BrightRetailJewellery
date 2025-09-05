Public Class frmJNDorCR
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
            BILLTYPE = "B"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

  


    Private Sub rbtBoth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtBoth.Click
        
    End Sub

    'Private Sub rbtCredit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCredit.Click
    '    If rbtCredit.Checked = True And Not IsAdvance Then
    '        BILLTYPE = "C"
    '        Me.DialogResult = Windows.Forms.DialogResult.OK
    '        Me.Close()
    '    End If
    'End Sub

    'Private Sub rbtJnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtJnd.Click
    '    If rbtJnd.Checked = True Then
    '        BILLTYPE = "J"
    '        Me.DialogResult = Windows.Forms.DialogResult.OK
    '        Me.Close()
    '    End If
    'End Sub

    Private Sub rbtBoth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtBoth.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            BILLTYPE = "B"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub rbtCredit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtCredit.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            BILLTYPE = "C"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub rbtJnd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtJnd.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            BILLTYPE = "J"
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmJNDorCR_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsAdvance Then rbtCredit.Text = "Order Advance" : rbtJnd.Text = "Gen Advance"
        rbtCredit.Checked = True : rbtCredit.Focus()
    End Sub
End Class