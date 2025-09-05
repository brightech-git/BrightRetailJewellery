Public Class frm4C
    Dim strsql As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'objGPack.Validator_Object(Me)
        strsql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strsql, CmbCut, True, False)
        strsql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
        objGPack.FillCombo(strsql, CmbColor, True, False)
        strsql = " SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SETTYPENAME"
        objGPack.FillCombo(strsql, cmbSetType, True, False)
        strsql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strsql, CmbClarity, True, False)
        strsql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strsql, cmbShape, True, False)
    End Sub
    Private Sub frmDiamondDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
    Private Sub frmDiamondDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.Close()
    End Sub
End Class