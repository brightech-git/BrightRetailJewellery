Imports System.Data.OleDb
Public Class frmGSTGSTTDS
    Dim strsql As String
    Dim DtTdsCategory As New DataTable
    Dim dtGSTCategory As New DataTable
    Dim Da As OleDbDataAdapter
    Public InterState As Boolean = False
    Public Sub New(ByVal _interstate As Boolean, ByVal ItemId As Integer, ByVal accode As String, ByVal trantype As String, ByVal Tcsper As Double)
        InitializeComponent()
        'strsql = "SELECT TDSCATNAME  AS TDSCAPTION,* FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER,TDSCATNAME"
        'Da = New OleDbDataAdapter(strsql, cn)
        'Da.Fill(DtTdsCategory)
        'If DtTdsCategory.Rows.Count > 0 Then
        '    cmbTdsCategory_OWN.DataSource = Nothing
        '    cmbTdsCategory_OWN.DataSource = DtTdsCategory
        '    cmbTdsCategory_OWN.DisplayMember = "TDSCAPTION"
        '    cmbTdsCategory_OWN.ValueMember = "TDSCATID"
        'End If
        'strsql = ""
        'strsql += vbCrLf + " SELECT CATCODE,SALESTAX"
        'strsql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY "
        'strsql += vbCrLf + " WHERE CATCODE IN "
        'strsql += vbCrLf + "(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & ItemId & " )"
        'Da = New OleDbDataAdapter(strsql, cn)
        'Da.Fill(dtGSTCategory)
        'If dtGSTCategory.Rows.Count > 0 Then
        '    cmbGstPer.DataSource = Nothing
        '    cmbGstPer.DataSource = dtGSTCategory
        '    cmbGstPer.DisplayMember = "SALESTAX"
        '    cmbGstPer.ValueMember = "CATCODE"
        'End If
        If accode = "" Then
        Else
            strsql = " select COUNT(*) CNT from " & cnAdminDb & "..ACHEAD where ACCODE = '" & accode & "' "
            If Val(GetSqlValue(cn, strsql).ToString) > 0 Then
                Dim drG As DataRow = Nothing
                strsql = " select isnull(TDSPER,0)TDSPER,COUNT(*) CNT from " & cnAdminDb & "..ACHEAD "
                strsql += " where TDSPER >= 0 And isnull(TDSFLAG,'')='Y' and ACCODE = '" & accode & "'"
                strsql += " GROUP BY TDSPER"
                drG = GetSqlRow(strsql, cn)
                If Not drG Is Nothing Then
                    If Val(drG.Item("CNT").ToString) > 0 Then
                        txtTdsPer_PER.Text = Format(Val(drG.Item("TDSPER").ToString), "0.00")
                    End If
                End If
                If trantype = "PURCHASE" Or trantype = "PURCHASE RETURN" Then
                    txtTdsPer_PER.Text = 0
                End If
            Else
                txtTdsPer_PER.Text = 0
            End If
        End If
        InterState = _interstate
        tdsval(Val(txtTdsPer_PER.Text))
        taxcollectresourceval(Tcsper)
        'GstVal()
    End Sub

    Private Sub frmGSTTax_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            'If Val(txtCgst_per_AMT.Text) > 0 And Val(txtSgst_per_AMT.Text) And Val(txtIgst_per_AMT.Text) > 0 Then
            '    MsgBox("Invalid Gst Per")
            '    Exit Sub
            'End If
            btnOk.Focus()
        End If
    End Sub

    Private Sub txtTdsPer_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTdsPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtTdsAmt_AMT.Focus()
    End Sub

    Public Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub tdsval(ByVal tdsPer As Decimal)
        'txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), "")
        txtTdsAmt_AMT.Text = Format(Math.Ceiling((Val(txtNetAmount_AMT.Text) * Val(txtTdsPer_PER.Text)) / 100), "0.00")
    End Sub

    Public Sub taxcollectresourceval(ByVal tcsPer As Decimal)
        ' txtTCSAmt_AMT.Text = Format(Math.Ceiling(((Val(txtNetAmount_AMT.Text) + Val(txtGstAmount.Text)) * Val(txtTCSPer_PER.Text)) / 100), "0.00")
        'txtTCSAmt_AMT.Text = Format(Math.Ceiling(((Val(txtNetAmount_AMT.Text) + Val(txtGstAmount.Text)) * Val(tcsPer)) / 100), "0.00")
        txtTCSAmt_AMT.Text = Format(Math.Round(((Val(txtNetAmount_AMT.Text) + Val(txtGstAmount.Text)) * Val(tcsPer)) / 100, 2), "0.00")
    End Sub
    Public Sub txtTdsPer_PER_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTdsPer_PER.TextChanged
        tdsval(Val(txtTdsPer_PER.Text))
    End Sub

    Private Sub frmGSTGSTTDS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tdsval(Val(txtTdsPer_PER.Text))
        taxcollectresourceval(Val(txtTCSPer_PER.Text))
    End Sub

    Private Sub txtTCSPer_PER_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTCSPer_PER.KeyDown
        If e.KeyCode = Keys.Enter Then
            taxcollectresourceval(Val(txtTCSPer_PER.Text))
        End If
    End Sub
    Private Sub txtTCSPer_PER_TextChanged(sender As Object, e As EventArgs) Handles txtTCSPer_PER.TextChanged
        taxcollectresourceval(Val(txtTCSPer_PER.Text))
    End Sub
End Class