Imports System.Data.OleDb
Public Class EstRateChange
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub BillOrderSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub txtRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtRate.Text) > 0 Then
                StrSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE='" & GetServerDate() & "' AND SRATE='" & Val(txtRate.Text) & "'"
                If Val(objGPack.GetSqlValue(StrSql)) = 0 Then
                    If MessageBox.Show("Rate not found in RateMaster,Do you want to Proceed.?", "Rate Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    End If
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub
End Class