Imports System.Data.OleDb
Public Class frmDiscountRateMaster
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim RATECARD_MANUAL As String = GetAdmindbSoftValue("RATECARD_MANUAL", "N")
    Dim RATEPURITY_SEP As Boolean = IIf(GetAdmindbSoftValue("RATEPURITY_SEP", "N") = "Y", True, False)
    Dim SYNC_RATE As Boolean = IIf(GetAdmindbSoftValue("SYNC_RATE", "N") = "Y", True, False)
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Public Rateviewonly As Boolean = False
    Dim SMS_RATE_UPDATE As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_RATE_UPDATE' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim dtCostCentre As New DataTable
    Dim TAG_DUMP As String = GetAdmindbSoftValue("TAG_DUMP", "")
    Dim Authorize As Boolean = False
    Dim _Edit As Boolean = False
    Dim RateCalc_Shortname As Boolean = IIf(GetAdmindbSoftValue("RATECALC_SHORTNAME", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        funcLoadMetalName()
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal Edit As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dtpDate.Value = GetServerDate(tran)
        btnUpdate.Visible = False
        lblBullGold.Visible = RATEPURITY_SEP : txtBullGRate_AMT.Visible = RATEPURITY_SEP
        lblBullSil.Visible = RATEPURITY_SEP : txtBullSRate_AMT.Visible = RATEPURITY_SEP
        lblBullPlat.Visible = RATEPURITY_SEP : txtBullPRate_AMT.Visible = RATEPURITY_SEP
    End Sub

    Function funcNew() As Integer
        dtpDate.Value = GetServerDate(tran)
        dtpDate.Focus()
        txtDisrate.Focus()
        funcLoadMetalName()
    End Function
    Function funcExit() As Integer
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Function
    Private Sub frmDiscountRateMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub frmDiscountRateMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Exit Sub
        End If
    End Sub

    Private Sub frmDiscountRateMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If RATEPURITY_SEP = True Then
            Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
            _Edit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        Else
            Authorize = True
            '_Edit = True
            _Edit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        End If

        dtpDate.Value = GetEntryDate(GetServerDate)
        lblBullGold.Visible = RATEPURITY_SEP And _Edit : txtBullGRate_AMT.Visible = RATEPURITY_SEP And _Edit
        lblBullSil.Visible = RATEPURITY_SEP And _Edit : txtBullSRate_AMT.Visible = RATEPURITY_SEP And _Edit
        lblBullPlat.Visible = RATEPURITY_SEP And _Edit : txtBullPRate_AMT.Visible = RATEPURITY_SEP And _Edit
        If RATEPURITY_SEP = False Then
            Authorize = False
            Me.Width = 510
            Me.Height = 405
        Else
            Me.Width = 685
            Me.Height = 530 '405
        End If
        If RATEPURITY_SEP = True And Authorize = False Then
        End If

        txtDisrate.Focus()
    End Sub
    Function funcLoadMetalName()
        Dim dt As New DataTable
        strSql = " select METALID,METALNAME from " & cnAdminDb & "..MetalMast "
        strSql += " where isnull(active,'') <> 'N' order by displayorder,MetalName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        cmbMetalName_Man.DataSource = Nothing
        cmbMetalName_Man.DataSource = dt
        cmbMetalName_Man.SelectedIndex = 0
        cmbMetalName_Man.DisplayMember = "METALNAME"
        cmbMetalName_Man.ValueMember = "METALID"
        Return 0
    End Function

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Val(txtDisrate.Text) = 0 Then
            MessageBox.Show("Enter the Discount rate.")
            txtDisrate.Focus()
            Exit Sub
        End If
        strSql = $"UPDATE {cnAdminDb}..DISCMASTER SET BOARDRATE = {Val(txtDisrate.Text)} where METAL = '{cmbMetalName_Man.SelectedValue}'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        MessageBox.Show("Discount rate updated successfully.")
        txtDisrate.Text = ""
        funcLoadMetalName()
        cmbMetalName_Man.Focus()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnUpdate_Click(Me, New EventArgs)
    End Sub
    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDisrate.KeyPress

        If e.KeyChar = "." And CType(sender, TextBox).Text.Contains(".") Then
            e.Handled = True
            Return
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back), ".",
            ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                CType(sender, TextBox).Focus()
                Return
        End Select
        If CType(sender, TextBox).Text.Contains(".") Then
            Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
            Dim sp() As String = CType(sender, TextBox).Text.Split(".")
            Dim curPos As Integer = CType(sender, TextBox).SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 1 Then
                        e.Handled = True
                        Return
                    End If
                End If
            End If
        End If
    End Sub
End Class