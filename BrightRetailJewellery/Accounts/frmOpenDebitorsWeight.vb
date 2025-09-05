Public Class frmOpenDebitorsWeight
    Dim strSql As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub frmOpenDebitorsWeight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub frmOpenDebitorsWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCategory.Focused Then Exit Sub
            If txtRate_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOpenDebitorsWeight_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub LoadCategory()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        txtCategory.Text = BrighttechPack.SearchDialog.Show("Search Category", strSql, cn, , , , , , , True)
        LoadCategoryDetail()
    End Sub

    Private Sub LoadCategoryDetail()
        Dim purity As Double = Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtCategory.Text & "')"))
        txtPurity_PER.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
        If txtPurity_PER.Text <> "" Then
            txtRate_AMT.Focus()
        Else
            txtPurity_PER.Focus()
        End If
    End Sub

    Private Sub txtCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCategory.Text = "" Then
                LoadCategory()
            ElseIf Not objGPack.GetSqlValue("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtCategory.Text & "'").Length > 0 Then
                LoadCategory()
            Else
                LoadCategoryDetail()
            End If
        End If
    End Sub

    Private Sub txtRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class