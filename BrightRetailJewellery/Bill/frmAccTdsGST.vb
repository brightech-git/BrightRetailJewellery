Imports System.Data.OleDb
Public Class frmAccTdsGST
    Public Amount As Double = Nothing
    Public DtTdsCategory As New DataTable
    Public Accode As String
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String
    Public StrGstCrAcname As String
    Public StrGstDrAcname As String
    Dim StrGsttDSAcname As String = ""
    Public EditFlag As Boolean = False
    'Private ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N")
    Private ROUNDOFF_ACC_TDS As String = GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N")
    Public ScFlag As Boolean

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.

        StrSql = " SELECT TDSCAPTION,TDSCATID FROM "
        StrSql += vbCrLf + " ( "
        StrSql += vbCrLf + " SELECT '' TDSCAPTION, 0 TDSCATID,0 DISPLAYORDER"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT TDSCATNAME AS TDSCAPTION,TDSCATID,DISPLAYORDER "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..TDSCATEGORY "
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " ORDER BY DISPLAYORDER,TDSCAPTION"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtTdsCategory)

        cmbTdsCategory_OWN.DataSource = DtTdsCategory
        cmbTdsCategory_OWN.DisplayMember = "TDSCAPTION"
        cmbTdsCategory_OWN.ValueMember = "TDSCATID"
    End Sub

    Public Sub GetTdsDefregistervalue()
        StrGsttDSAcname = ""
        Dim TdsFlag As Boolean = False
        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & StrGstCrAcname & "'").ToUpper = "Y" Then
            StrGsttDSAcname = StrGstCrAcname
            TdsFlag = True
        End If
        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & StrGstDrAcname & "'").ToUpper = "Y" Then
            StrGsttDSAcname = StrGstDrAcname
            TdsFlag = True
        End If
        If TdsFlag = True Then
            StrSql = "SELECT TOP 1 TDSCATNAME AS TDSCAPTION FROM " & cnAdminDb & "..TDSCATEGORY "
            StrSql += vbCrLf + " WHERE TDSCATID IN (SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & StrGsttDSAcname & "')"
            StrSql += vbCrLf + " ORDER BY DISPLAYORDER,TDSCATNAME "
            cmbTdsCategory_OWN.Text = objGPack.GetSqlValue(StrSql, "TDSCAPTION", "", )

            StrSql = "SELECT TDSPER FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & StrGsttDSAcname & "'"
            Dim tdsPer As Decimal = Val(objGPack.GetSqlValue(StrSql))
            txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), "")
        End If
    End Sub

    Private Sub frmAccTds_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cmbTdsCategory_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTdsCategory_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbTdsCategory_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTdsCategory_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If EditFlag Then Exit Sub
            'If txtTdsPer_PER.Text <> "" Then Exit Sub
            StrSql = " SELECT TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & cmbTdsCategory_OWN.SelectedValue & "'"
            Dim tdsPer As Decimal = Val(objGPack.GetSqlValue(StrSql))
            txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), "")
        End If
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
    Private Sub txtTdsPer_PER_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTdsPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class