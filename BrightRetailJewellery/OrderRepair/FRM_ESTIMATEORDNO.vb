Public Class FRM_ESTIMATEORDNO
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
            StrSql = " SELECT DISTINCT ORNO "
            'StrSql += " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = E.PSNO)AS NAME"
            StrSql += " ,ORDATE"
            StrSql += " FROM " & cnStockDb & "..ESTORMAST AS E WHERE ORTYPE = 'O' "
            StrSql += " AND ISNULL(ODBATCHNO,'') = ''"
            StrSql += " ORDER BY ORNO"
            txtEstNo_NUM.Text = BrighttechPack.SearchDialog.Show("Search order Estimation No", StrSql, cn)
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

    Private Sub grpOrderSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpOrderSearch.Load

    End Sub

    Private Sub FRM_ESTIMATEORDNO_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class