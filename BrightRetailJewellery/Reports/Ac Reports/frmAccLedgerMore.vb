Imports System.Data.OleDb
Public Class frmAccLedgerMore
    Dim strSql As String
    Dim dt As DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            lstContra.Items.Add(ro!ACNAME.ToString)
        Next

        cmbFindAmount.Items.Add("")
        cmbFindAmount.Items.Add(">")
        cmbFindAmount.Items.Add(">=")
        cmbFindAmount.Items.Add("<")
        cmbFindAmount.Items.Add("<=")
        cmbFindAmount.Items.Add("<>")
        cmbFindAmount.Items.Add("=")
        cmbFindAmount.Text = ""


        cmbVoucherType.Items.Add("")
        strSql = " SELECT CAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE ACTIVE = 'Y' ORDER BY CAPTION"
        objGPack.FillCombo(strSql, cmbVoucherType, False, False)
        'cmbVoucherType.Items.Add("SALES")
        'cmbVoucherType.Items.Add("PURCHASE")
        'cmbVoucherType.Items.Add("SALES RETURN")
        'cmbVoucherType.Items.Add("RECEIPT")
        'cmbVoucherType.Items.Add("PAYMENT")
        'cmbVoucherType.Items.Add("JOURNAL")
        'cmbVoucherType.Items.Add("DEBIT NOTE")
        'cmbVoucherType.Items.Add("CREDIT NOTE")
        'cmbVoucherType.Text = ""
    End Sub
    Private Sub cmbFindAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
        If cmbFindAmount.Text = "" Then
            txtFindAmount_AMT.Clear()
            txtFindAmount_AMT.Enabled = False
        Else
            txtFindAmount_AMT.Enabled = True
        End If
    End Sub

    Private Sub frmAccLedgerMore_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmAccLedgerMore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFindNarration.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtFindNarration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindNarration.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmAccLedgerMore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lstContra.Focus()
    End Sub

    Private Sub txtFindAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFindAmount_AMT.GotFocus
        If cmbFindAmount.Text = "" Then
            txtFindAmount_AMT.Clear()
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkContraAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkContraAll.CheckedChanged
        SetChecked_CheckedList(lstContra, chkContraAll.Checked)
    End Sub
End Class