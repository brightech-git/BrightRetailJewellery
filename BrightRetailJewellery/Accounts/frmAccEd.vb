Imports System.Data.OleDb
Public Class frmAccEd
    Public Amount As Double = Nothing
    Public DtTdsCategory As New DataTable
    Public Accode As String
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String
    Public EditFlag As Boolean = False
    Private ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.
        
        
    End Sub
    Private Sub frmAcced_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtEdPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEdPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtEdAmt_AMT.Focus()
        End If
    End Sub

    Private Sub CalcNetAmount(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtActualAmount_AMT.TextChanged _
    , txtEdAmt_AMT.TextChanged, txtEcAmt_AMT.TextChanged, txtHEAmt_AMT.TextChanged
        Amount = Val(txtActualAmount_AMT.Text)
        CalcEdAmt()
        Dim NetAmt As Decimal = Nothing
        NetAmt = Val(txtEdAmt_AMT.Text) + Val(txtEcAmt_AMT.Text) + Val(txtHEAmt_AMT.Text)
        txtNetAmount_AMT.Text = IIf(NetAmt <> 0, Format(NetAmt, "0.00"), Nothing)
    End Sub

    Private Sub CalcEdAmt()
        Dim tdsAmt As Double = Amount * (Val(txtEdPer_PER.Text) / 100)
        tdsAmt = RoundOffPisa(tdsAmt)
        txtEdAmt_AMT.Text = IIf(tdsAmt <> 0, Format(tdsAmt, "0.00"), Nothing)
        Dim ecsAmt As Double = tdsAmt * (Val(txtEcPer_PER.Text) / 100)
        ecsAmt = RoundOffPisa(ecsAmt)
        txtEcAmt_AMT.Text = IIf(ecsAmt <> 0, Format(ecsAmt, "0.00"), Nothing)
        Dim hesAmt As Double = tdsAmt * (Val(txtHePer_PER.Text) / 100)
        hesAmt = RoundOffPisa(hesAmt)
        txtHEAmt_AMT.Text = IIf(hesAmt <> 0, Format(hesAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtEdPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEdPer_PER.TextChanged
        CalcEdAmt()
    End Sub
    Private Sub txtEcPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEcPer_PER.TextChanged
        CalcEdAmt()
    End Sub
    Private Sub txthEPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHePer_PER.TextChanged
        CalcEdAmt()
    End Sub

    Private Sub KeyEnter(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles _
    txtActualAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtEdAmt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEdAmt_AMT.KeyPress, txtEcAmt_AMT.KeyPress, txtHEAmt_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            If Val(txtEdPer_PER.Text) > 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    

    Private Sub frmAccTds_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtActualAmount_AMT.Select()

    End Sub

    
    Private Sub txtNetAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        txtEdAmt_AMT.Select()
    End Sub
    Private Function RoundOffPisa(ByVal value As Decimal) As Decimal
        Select Case ROUNDOFF_ACC
            Case "L"
                Return Math.Floor(value)
            Case "F"
                If Math.Abs(value - Math.Floor(value)) >= 0.5 Then
                    Return Math.Ceiling(value)
                Else
                    Return Math.Floor(value)
                End If
            Case "H"
                Return Math.Ceiling(value)
            Case Else
                Return value
        End Select
        Return value
    End Function

    Private Sub txtNetAmount_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNetAmount_AMT.TextChanged

    End Sub
End Class