Public Class FRM_ESTIMATENO
    Dim StrSql As String
    Public BillDate As Date
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub txtEstNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEstNo_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = " SELECT TRANNO "
            StrSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = E.PSNO)AS NAME"
            StrSql += " ,TRANDATE"
            StrSql += " FROM " & cnStockDb & "..ESTRECEIPT AS E WHERE TRANDATE = '" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'PU' "
            StrSql += " AND ISNULL(BATCHNO,'') = ''"
            StrSql += " ORDER BY TRANNO"
            txtEstNo_NUM.Text = BrighttechPack.SearchDialog.Show("Search Purchase Estimation No", StrSql, cn)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Hide()
        ElseIf e.KeyCode = Keys.Enter Then
            If txtEstNo_NUM.Text <> "" Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub txtEstNo_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEstNo_NUM.KeyPress
    End Sub
End Class