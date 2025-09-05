Public Class frmAcAddInfo
    Dim Strsql As String
    Public editflag As Boolean = False
    Public Dob As String
    Public C1Dob As String
    Public C2Dob As String
    Public C3Dob As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal edit As Boolean)
        editflag = edit
    End Sub
    Private Sub frmAcAddInfo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmAcAddInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.BackColor = frmBackColor
        objGPack.Validator_Object(Me)
        dtpDOB.MaximumDate = New Date(9998, 12, 31)
        dtpDOB.MinimumDate = New Date(1753, 1, 1)
        dtpC1DOB.MaximumDate = New Date(9998, 12, 31)
        dtpC1DOB.MinimumDate = New Date(1753, 1, 1)
        dtpC2DOB.MaximumDate = New Date(9998, 12, 31)
        dtpC2DOB.MinimumDate = New Date(1753, 1, 1)
        dtpC3DOB.MaximumDate = New Date(9998, 12, 31)
        dtpC3DOB.MinimumDate = New Date(1753, 1, 1)
        If editflag = False Then
            dtpDOB.Value = GetServerDate(tran)
            dtpC1DOB.Value = GetServerDate(tran)
            dtpC2DOB.Value = GetServerDate(tran)
            dtpC3DOB.Value = GetServerDate(tran)
            Strsql = "select Empname from " & cnAdminDb & "..EMPMASTER order by Empname"
            objGPack.FillCombo(Strsql, CmbEnteredBy, False)
            objGPack.FillCombo(Strsql, CmbIssuedBy, False)
            CmbEnteredBy.SelectedIndex = 0
            CmbIssuedBy.SelectedIndex = 0
        Else
            dtpDOB.Value = Dob
            dtpC1DOB.Value = C1Dob
            dtpC2DOB.Value = C2Dob
            dtpC3DOB.Value = C3Dob
        End If
        txtGuardian.Focus()
    End Sub

    Private Sub Chkdb1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chkdb1.CheckedChanged
        If Chkdb1.Checked Then
            dtpDOB.Enabled = True
        Else
            dtpDOB.Enabled = False
        End If
    End Sub

    Private Sub Chkdb2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chkdb2.CheckedChanged
        If Chkdb2.Checked Then
            dtpC1DOB.Enabled = True
        Else
            dtpC1DOB.Enabled = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            dtpC2DOB.Enabled = True
        Else
            dtpC2DOB.Enabled = False
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            dtpC3DOB.Enabled = True
        Else
            dtpC3DOB.Enabled = False
        End If
    End Sub
End Class