Public Class frmSetItemWastMc

    Public EscapePress As Boolean = False

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmSetItemWastMc_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.OK
            EscapePress = True
            Me.Close()
        End If
    End Sub

    Private Sub frmSetItemWastMc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If (txtSetMc_AMT.Focused = True) Or (txtSetMcPerGrm_AMT.Focused = True And txtSetMc_AMT.Enabled = False) Then
                DialogResult = Windows.Forms.DialogResult.OK
                EscapePress = False
                Me.Close()
            End If

            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSetItemWastMc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtSetWastage_WET_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSetWastage_WET.Enter
        If Val(txtSetWastagePer_Per.Text) <> 0 Then
            txtSetWastage_WET.Enabled = False
        Else
            txtSetWastage_WET.Enabled = True
        End If
    End Sub

    Private Sub txtSetMc_AMT_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSetMc_AMT.Enter
        If Val(txtSetMcPerGrm_AMT.Text) <> 0 Then
            txtSetMc_AMT.Enabled = False
        Else
            txtSetMc_AMT.Enabled = True
        End If
    End Sub

    Private Sub txtSetWastagePer_Per_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSetWastagePer_Per.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtSetWastagePer_Per.Text) <> 0 Then
                txtSetWastage_WET.Enabled = False
                txtSetWastage_WET.Text = ""
            Else
                txtSetWastage_WET.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtSetMcPerGrm_AMT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSetMcPerGrm_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtSetMcPerGrm_AMT.Text) <> 0 Then
                txtSetMc_AMT.Enabled = False
                txtSetMc_AMT.Text = ""
            Else
                txtSetMc_AMT.Enabled = True
            End If
        End If
    End Sub
End Class