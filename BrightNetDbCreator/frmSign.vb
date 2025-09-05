Imports System.IO
Public Class frmSign


    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtCompid.Text = "" Then txtCompid.Focus() : Exit Sub
        If txtCompName.Text = "" Then txtCompName.Focus() : Exit Sub
        If txtDbserver.Text = "" Then txtDbserver.Focus() : Exit Sub
        Dim Line1 As String = txtCompid.Text
        Dim Line2 As String = txtCompName.Text
        Dim Line3 As String = txtDbserver.Text
        Dim Line4 As String = txtDbPath.Text
        Dim Line5 As String = IIf(txtSqlPwd.Text.Trim <> "", BrighttechPack.Methods.Encrypt(txtSqlPwd.Text.Trim), "")

        Dim Line6 As String = IIf(txtSqlPwd.Text.Trim <> "", txtSqluser.Text, Mid(cmbLogintype.Text, 1, 1))
        Dim Line7 As Integer = Val(txtCurrency_NUM.Text)
        Dim filePath As String = Application.StartupPath + "\ConInfo.ini"

        Dim fil As New FileStream(filePath, FileMode.CreateNew, FileAccess.Write)
        Dim write As New StreamWriter(fil, System.Text.Encoding.Default)

        'Dim write As IO.StreamWriter
        'write = IO.File.CreateText(filePath)
        write.WriteLine(LSet("Company Id         :", 20) & Line1)
        write.WriteLine(LSet("Company Name       :", 20) & Line2)
        write.WriteLine(LSet("Database Server    :", 20) & Line3)
        write.WriteLine(LSet("Database Path      :", 20) & Line4)
        write.WriteLine(LSet("Password           :", 20) & Line5)
        write.WriteLine(LSet("DB Login Type      :", 20) & Line6)
        If Line7 <> 2 Then
            write.WriteLine(LSet("Currency Decimal   :", 20) & Line7)
        End If
        write.Flush()
        write.Close()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmSign_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCompid.Focused And Len(txtCompid.Text) <> 3 Then Exit Sub
            If txtCompName.Focused And Len(txtCompName.Text) = 0 Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtSqlPwd.CharacterCasing = CharacterCasing.Normal
        txtCompid.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtCurrency_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCurrency_NUM.KeyPress
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back),
                ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                sender.Focus()
        End Select
    End Sub
End Class