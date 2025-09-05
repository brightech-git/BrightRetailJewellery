Imports System.Data.OleDb
Public Class frmAccTds
    Public Amount As Double = Nothing
    Public DtTdsCategory As New DataTable
    Public Accode As String
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String
    Public EditFlag As Boolean = False
    Public SRVTCODE As String = Nothing
    'Private ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N")
    Private ROUNDOFF_ACC_TDS As String = GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.
        StrSql = "SELECT TDSCATNAME + ' [' + CONVERT(VARCHAR,TDSPER) + '%]' AS TDSCAPTION,* FROM " & cnAdminDb & "..TDSCATEGORY ORDER BY DISPLAYORDER,TDSCATNAME"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtTdsCategory)
        cmbTdsCategory_OWN.DataSource = DtTdsCategory
        cmbTdsCategory_OWN.DisplayMember = "TDSCAPTION"
        cmbTdsCategory_OWN.ValueMember = "TDSCATID"
    End Sub
    Private Sub frmAccTds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtTdsPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTdsPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtTdsAmt_AMT.Focus()

    End Sub

    Private Sub txtServTax_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtServTax_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then cmbTdsCategory_OWN.Focus()

    End Sub

    Private Sub CalcNetAmount(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtActualAmount_AMT.TextChanged _
    , txtTdsAmt_AMT.TextChanged, txtServTax_Amt.TextChanged
        Amount = Val(txtActualAmount_AMT.Text) '- Val(txtServTax_Amt.Text)
        txtGrsAmt.Text = IIf(Amount <> 0, Format(Amount, "0.00"), Nothing)
        If Val(txtServTax_PER.Text) <> 0 Or Val(txtServTax_Amt.Text) <> 0 Then CalcSvTAmt()
        If Val(txtTdsPer_PER.Text) <> 0 Then CalcTdsAmt()
        Dim NetAmt As Decimal = Nothing
        NetAmt = Val(txtGrsAmt.Text) - Val(txtTdsAmt_AMT.Text)
        

        txtNetAmount_AMT.Text = IIf(NetAmt <> 0, Format(NetAmt, "0.00"), Nothing)
    End Sub

    Private Sub CalcTdsAmt()
        Dim tdsAmt As Double = Val(txtActualAmount_AMT.Text) * (Val(txtTdsPer_PER.Text) / 100)
        tdsAmt = RoundOffPisa(tdsAmt)
        txtTdsAmt_AMT.Text = IIf(tdsAmt <> 0, Format(tdsAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtTdsPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTdsPer_PER.TextChanged
        CalcTdsAmt()
    End Sub

    Private Sub KeyEnter(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles _
    txtActualAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtTdsAmt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTdsAmt_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = "SELECT PAN FROM " & cnAdminDb & "..ACHEAD WHERE  Accode='" & Accode & "'"
            If objGPack.GetSqlValue(StrSql).ToString = "" Then MsgBox("Pan No Empty. Please update in Account Master", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            If Val(txtTdsPer_PER.Text) > 0 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub cmbTdsCategory_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTdsCategory_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmAccTds_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtActualAmount_AMT.Select()
        'cmbTdsCategory_OWN.Select()
    End Sub

    Private Sub cmbTdsCategory_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTdsCategory_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If EditFlag Then Exit Sub
            StrSql = " SELECT TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & cmbTdsCategory_OWN.SelectedValue & "'"
            Dim tdsPer As Decimal = Val(objGPack.GetSqlValue(StrSql))
            txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), "")
        End If
    End Sub

    Private Sub txtNetAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetAmount_AMT.GotFocus
        txtTdsAmt_AMT.Select()
    End Sub
    Private Function RoundOffPisa(ByVal value As Decimal) As Decimal
        Select Case ROUNDOFF_ACC_TDS
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

    Private Sub txtServTax_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtServTax_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If SRVTCODE Is Nothing Then MsgBox("Please check the Control <SRVT_COMPONENTS>" & vbCrLf & "Service tax Account Code", MsgBoxStyle.OkOnly)
            txtServTax_Amt.Focus()
        End If
    End Sub

    Private Sub txtServTax_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtServTax_PER.TextChanged
        CalcSvTAmt()
    End Sub
    Private Sub CalcSvTAmt()
        If Val(txtServTax_PER.Text) <> 0 Then
            Dim minclper As Double = Val(txtServTax_PER.Text) '+ 100
            'Dim tdsAmt As Double = Val(txtActualAmount_AMT.Text) - (Val(txtActualAmount_AMT.Text) / minclper) * 100
            Dim tdsAmt As Double = Val(txtActualAmount_AMT.Text) * (minclper / 100)
            tdsAmt = RoundOffPisa(tdsAmt)
            txtServTax_Amt.Text = IIf(tdsAmt <> 0, Format(tdsAmt, "0.00"), Nothing)
        End If
        Dim grsamt As Double = Val(txtActualAmount_AMT.Text) + Val(txtServTax_Amt.Text)
        txtGrsAmt.Text = Format(grsamt, "0.00")
    End Sub

    Private Sub txtServTax_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtServTax_Amt.TextChanged

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub grpWastageMc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpWastageMc.Load

    End Sub
End Class