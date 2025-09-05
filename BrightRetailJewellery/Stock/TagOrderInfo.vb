Public Class TagOrderInfo
    Dim StrSql As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub txtOrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtEmpNo_NUM.Focus()
        End If
    End Sub

    Private Sub txtEmpNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmpNo_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtOrderNo.Text = "" Then
                MsgBox("Order No should not empty", MsgBoxStyle.Information)
                txtOrderNo.Focus()
                Exit Sub
            End If
            If txtEmpNo_NUM.Text = "" Then
                LoadEmpId(txtEmpNo_NUM)
                Exit Sub
            ElseIf Not Val(objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpNo_NUM.Text) & "")) > 0 Then
                LoadEmpId(txtEmpNo_NUM)
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub LoadEmpId(ByVal txtEmpBox As TextBox)
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim empId As Integer = Val(BrighttechPack.SearchDialog.Show("Select EmpName", strSql, cn, 1))
        If empId > 0 Then
            txtEmpBox.Text = empId
            txtEmpBox.SelectAll()
        End If
    End Sub

    Private Sub txtOrderNo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrderNo.Leave
        If Trim(txtOrderNo.Text) = "" Then
            MessageBox.Show("OrderNo Shouldn't Empty.")
            txtOrderNo.Focus()
        End If
    End Sub
End Class