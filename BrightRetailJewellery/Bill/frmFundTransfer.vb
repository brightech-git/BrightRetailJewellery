Public Class frmFundTransfer
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        rbtRTGS.Checked = True
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ''Validation
        If txtAcNo_MAN.Text = "" Then
            MsgBox("A/c No should not empty", MsgBoxStyle.Information)
            txtAcNo_MAN.Focus()
            Exit Sub
        End If
        If txtAcName_MAN.Text = "" Then
            MsgBox("A/c Name should not empty", MsgBoxStyle.Information)
            txtAcName_MAN.Focus()
            Exit Sub
        End If
        If txtAcAddress_MAN.Text = "" Then
            MsgBox("Address Name should not empty", MsgBoxStyle.Information)
            txtAcAddress_MAN.Focus()
            Exit Sub
        End If
        If rbtRTGS.Checked Then
            If txtIFSCCode_MAN.Text = "" Then
                MsgBox("IFSC Code should not empty", MsgBoxStyle.Information)
                txtIFSCCode_MAN.Focus()
                Exit Sub
            End If
            If txtBankName_MAN.Text = "" Then
                MsgBox("Bank Name should not empty", MsgBoxStyle.Information)
                txtBankName_MAN.Focus()
                Exit Sub
            End If
            If txtBankBranch_MAN.Text = "" Then
                MsgBox("Branch name should not empty", MsgBoxStyle.Information)
                txtBankBranch_MAN.Focus()
                Exit Sub
            End If
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub frmFundTransfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnOk.Focus()
        End If
    End Sub

    Private Sub frmFundTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAcAddress_MAN.Focused Then Exit Sub
            If txtBankAddress.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub rbtRTGS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtRTGS.CheckedChanged
        If Not rbtRTGS.Checked Then
            objGPack.TextClear(grpBank)
        End If
    End Sub

    Private Sub ctrl_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    txtAcAddress_MAN.KeyDown, txtBankAddress.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(CType(sender, Control), True, True, True, True)
            End If
        End If

    End Sub

    Private Sub skip_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtBankAddress.GotFocus, txtBankBranch_MAN.GotFocus, txtBankName_MAN.GotFocus, txtIFSCCode_MAN.GotFocus
        If rbtCheque.Checked Then Me.SelectNextControl(CType(sender, Control), True, True, True, True)
    End Sub
End Class