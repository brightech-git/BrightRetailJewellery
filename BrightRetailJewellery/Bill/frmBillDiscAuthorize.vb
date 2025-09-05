Imports System.Data.OleDb

Public Class frmBillDiscAuthorize
    Dim strSql As String
    Dim DiscAmt As Double = Nothing
    Dim TotAmt As Double = Nothing
    Public Ischeck As Boolean = True
    Dim DispDisc As Boolean = True
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.DiscAmt = 0
        Me.TotAmt = 0

    End Sub

    Public Sub New(ByVal totAmt As Double, ByVal discAmt As Double, Optional ByVal Disp As Boolean = False)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        Me.DiscAmt = discAmt
        Me.TotAmt = totAmt
        Me.DispDisc = Disp
        Dim discPer As Double = Math.Round((discAmt * 100) / totAmt, 2)
        'Me.Text = Me.Text & " Upto " & discPer & "%"
        If Not Ischeck Then txtPassword.Visible = False
    End Sub

    Private Sub LoadEmpId()
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim ro As DataRow = BrighttechPack.SearchDialog.Show_R("Search EmpName", strSql, cn)
        If Not ro Is Nothing Then
            txtEmpId_NUM.Text = ro!EMPID
            txtEmpName.Text = ro!EMPNAME
            If Ischeck Then txtPassword.Focus() Else btnOk.Focus()
        End If
    End Sub

    Private Sub txtEmpId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmpId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadEmpId()
        End If
    End Sub

    Private Sub txtEmpId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmpId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEmpId_NUM.Text = "" Then
                LoadEmpId()
            Else
                txtEmpName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_NUM.Text) & "")
                If txtEmpName.Text <> "" Then
                    If Ischeck Then txtPassword.Focus() Else btnOk.Focus()
                Else
                    LoadEmpId()
                End If
            End If
        End If
    End Sub

    Private Sub frmBillDiscAuthorize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub frmBillDiscAuthorize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtPassword.CharacterCasing = CharacterCasing.Normal
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        txtPassword.CharacterCasing = CharacterCasing.Normal
        If txtEmpId_NUM.Text = "" Then
            MsgBox(Me.GetNextControl(txtEmpId_NUM, False).Text + E0001, MsgBoxStyle.Information)
            txtEmpId_NUM.Select()
        Else
            txtEmpName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_NUM.Text) & "")
            If txtEmpName.Text <> "" Then
                If Ischeck Then txtPassword.Focus()
            Else
                MsgBox(E0004 + Me.GetNextControl(txtEmpId_NUM, False).Text, MsgBoxStyle.Information)
                txtEmpId_NUM.Focus()
            End If
        End If
        If txtPassword.Text = "" And Ischeck Then
            MsgBox(Me.GetNextControl(txtPassword, False).Text + E0001, MsgBoxStyle.Information)
            txtPassword.Focus()
            Exit Sub
        End If
        If Not Ischeck Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
            Exit Sub
        End If
        strSql = "SELECT PASSWORD,DISCPER,DISCAMT FROM " & cnAdminDb & "..DISCAUTHORIZE WHERE EMPID = " & Val(txtEmpId_NUM.Text) & ""
        strSql += " AND PASSWORD = '" & objGPack.Encrypt(txtPassword.Text) & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If CheckAuthrorize(Val(dt.Rows(0).Item("DISCPER").ToString), Val(dt.Rows(0).Item("DISCAMT").ToString)) = False Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                txtEmpId_NUM.Focus()
                txtEmpId_NUM.SelectAll()
            End If
        Else
            MsgBox("Invalid Authorize Detail", MsgBoxStyle.Information)
            txtEmpId_NUM.Focus()
            txtEmpId_NUM.SelectAll()
        End If
        'If objGPack.Encrypt(txtPassword.Text) = objGPack.GetSqlValue(strSql) Then
        '    Me.DialogResult = Windows.Forms.DialogResult.OK
        'Else
        '    MsgBox("Invalid Authorize Detail", MsgBoxStyle.Information)
        '    txtEmpId_NUM.Focus()
        '    txtEmpId_NUM.SelectAll()
        'End If
    End Sub

    Private Sub txtEmpName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPassword.Text = "" Then
                MsgBox(Me.GetNextControl(txtPassword, False).Text + E0001, MsgBoxStyle.Information)
                txtPassword.Focus()
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Function CheckAuthrorize(ByVal alDiscPer As Double, ByVal alDiscAmt As Double) As Boolean
        Dim discPer As Double = Math.Round((DiscAmt * 100) / TotAmt, 2)
        If alDiscPer <> 0 And alDiscAmt <> 0 Then
            If DiscAmt > alDiscAmt Then
                If DispDisc Then
                    MsgBox("This User Authorize " & alDiscAmt & " Rs Only")
                Else
                    MsgBox("Discount Limit Exceeds.", MsgBoxStyle.Information)
                End If
                Return True
            End If
            If discPer > alDiscPer Then
                If DispDisc Then
                    MsgBox("This User Authorize " & Math.Round(TotAmt * (alDiscPer / 100), 2) & " Rs Only")
                Else
                    MsgBox("Discount Limit Exceeds.", MsgBoxStyle.Information)
                End If
                Return True
            End If
        ElseIf alDiscPer = 0 And alDiscAmt <> 0 Then
            If DiscAmt > alDiscAmt Then
                If DispDisc Then
                    MsgBox("This User Authorize " & alDiscAmt & " Rs Only")
                Else
                    MsgBox("Discount Limit Exceeds.", MsgBoxStyle.Information)
                End If
                Return True
            End If
        ElseIf alDiscPer <> 0 And alDiscAmt = 0 Then
            If discPer > alDiscPer Then
                If DispDisc Then
                    MsgBox("This User Authorize " & Math.Round(TotAmt * (alDiscPer / 100), 2) & " Rs Only")
                Else
                    MsgBox("Discount Limit Exceeds.", MsgBoxStyle.Information)
                End If
                Return True
            End If
        End If
    End Function
End Class