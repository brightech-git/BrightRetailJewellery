Public Class frmAdvFixWtper
    Public ADVFIXWTPERSTR As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        
    End Sub

    Private Sub cmbWtper_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbWtper.KeyDown
        If e.KeyCode = Keys.Enter Then SendKeys.Send("{TAB}") : Exit Sub
    End Sub

    
    
    


    
    
    Private Sub cmbWtper_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWtper.SelectedIndexChanged
        If Val(cmbWtper.Text) > 100 Then
            txtFixWt.Text = Math.Round((Val(cmbWtper.Text) / 100) * (Val(txtAmount.Text) / Val(txtRate.Text)), 3)
        Else

            txtFixWt.Text = Math.Round((Val(txtAmount.Text) / (Val(cmbWtper.Text) / 100)) / Val(txtRate.Text), 3)
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmAdvFixWtper_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        
    End Sub

    Private Sub frmAdvFixWtper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}") : Exit Sub
        If e.KeyChar = Chr(Keys.Escape) Then SendKeys.Send("{TAB}") : Exit Sub
    End Sub

    Private Sub frmAdvFixWtper_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim advwtarr() As String = Split(ADVFIXWTPERSTR, ",")
        For ii As Integer = 0 To advwtarr.Length - 1
            cmbWtper.Items.Add(advwtarr(ii))
        Next ii
        cmbWtper.SelectedIndex = 0
        cmbWtper.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class