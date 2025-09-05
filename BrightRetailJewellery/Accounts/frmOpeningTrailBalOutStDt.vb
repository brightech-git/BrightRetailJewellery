Imports System.Data.OleDb
Public Class frmOpeningTrailBalOutStDt
    Public dtGridView As New DataTable
    Public amt As Double
    Public type As EntryType
    Public UpdateFlag As Boolean = False
    Public COSTID As String
    Public _Accode As String
    Dim strSql As String
    Dim EditFlag As Boolean = False

    Enum EntryType
        Debit = 0
        Credit = 1
    End Enum
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal amount As Double, Optional ByVal Accode As String = "")

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        dtpTranDate.Value = GetEntryDate(GetServerDate)

        cmbMode.Items.Add("DUE")
        cmbMode.Items.Add("DUE REC")
        cmbMode.Items.Add("ADVANCE")
        cmbMode.Text = "DUE"
        type = EntryType.Debit
        amt = amount
        _Accode = Accode

        With dtGridView.Columns
            .Add("TRANNO", GetType(Integer))
            .Add("TRANDATE", GetType(Date))
            .Add("REFTYPE", GetType(String))
            .Add("REFNO", GetType(Integer))
            .Add("RUNNO", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("REMARK", GetType(String))
        End With
        gridView.DataSource = dtGridView
        gridView.ColumnHeadersVisible = False
        With gridView
            .Columns("TRANNO").Width = txtTranNo_NUM_MAN.Width + 1
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = dtpTranDate.Width + 1
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("REFTYPE").Width = cmbMode.Width + 1
            .Columns("REFNO").Width = txtRefNo_NUM_MAN.Width + 1
            .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RUNNO").Width = txtRunno.Width + 1
            .Columns("AMOUNT").Width = txtAmount_AMT_MAN.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Format = "##,##,##,###.00"
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REMARK").Width = txtRemark.Width
        End With
        If _Accode <> "" Then
            strSql = "SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE ACCODE='" & _Accode & "' AND FROMFLAG='O' "
            Dim dr As DataRow = GetSqlRow(strSql, cn, tran)
            If Not dr Is Nothing Then
                Dim ro As DataRow = dtGridView.NewRow
                ro.Item("TRANNO") = dr.Item("TRANNO").ToString
                ro.Item("TRANDATE") = dr.Item("TRANDATE").ToString
                If dr.Item("TRANTYPE").ToString = "A" Then
                    ro.Item("REFTYPE") = "ADVANCE"
                ElseIf dr.Item("TRANTYPE").ToString = "D" And dr.Item("RECPAY").ToString = "R" Then
                    ro.Item("REFTYPE") = "DUE REC"
                ElseIf dr.Item("TRANTYPE").ToString = "D" Then
                    ro.Item("REFTYPE") = "DUE"
                End If
                ro.Item("REFNO") = Mid(dr.Item("RUNNO").ToString, 9, 20)
                ro.Item("RUNNO") = Mid(dr.Item("RUNNO").ToString, 6, 20)
                ro.Item("AMOUNT") = Val(dr.Item("AMOUNT").ToString)
                ro.Item("REMARK") = dr.Item("REMARK1").ToString
                dtGridView.Rows.Add(ro)
            End If
        End If
        CalcBalance()
        txtTranNo_NUM_MAN.Focus()
        EditFlag = False
    End Sub



    Private Sub CalcBalance()
        dtGridView.AcceptChanges()
        Dim tot As Double = Nothing
        For Each ro As DataRow In dtGridView.Rows
            Select Case ro!REFTYPE.ToString
                Case "ADVANCE", "DUE REC"
                    If type = EntryType.Debit Then
                        tot -= Val(ro!AMOUNT.ToString)
                    Else
                        tot += Val(ro!AMOUNT.ToString)
                    End If
                Case "DUE"
                    If type = EntryType.Debit Then
                        tot += Val(ro!AMOUNT.ToString)
                    Else
                        tot -= Val(ro!AMOUNT.ToString)
                    End If
            End Select
        Next
        tot = amt - tot
        lblBalanceAmt.Text = IIf(tot = 0, "0.00", Format(tot, "0.00"))
    End Sub

    Private Sub frmOpeningTrailBalOutStDt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRemark.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Function checkRefno() As Boolean
        If txtRefNo_NUM_MAN.Text = "" Then
            MsgBox("RefNo should not empty", MsgBoxStyle.Information)
            txtRefNo_NUM_MAN.Focus()
            Return True
        ElseIf objGPack.GetSqlValue("SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & GetCostId(COSTID) & GetCompanyId(strCompanyId) & txtRunno.Text & "'").Length > 0 Then
            If EditFlag = False Then
                MsgBox("RefNo already exist", MsgBoxStyle.Information)
                txtRefNo_NUM_MAN.Focus()
                Return True
            End If
        End If
    End Function

    Private Sub genRunno()
        If txtRefNo_NUM_MAN.Text = "" Then
            txtRunno.Text = ""
            Exit Sub
        End If
        Select Case cmbMode.Text
            Case "DUE", "DUE REC"
                txtRunno.Text = "D"
            Case Else
                txtRunno.Text = "A"
        End Select
        txtRunno.Text += Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + txtRefNo_NUM_MAN.Text
    End Sub

    Private Sub frmOpeningTrailBalOutStDt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpTranDate.MinimumDate = (New DateTimePicker).MinDate
        dtpTranDate.MaximumDate = (New DateTimePicker).MaxDate
        If UpdateFlag = False Then
            If dtGridView.Columns.Contains("TRANNO") = False Then
                With dtGridView.Columns
                    .Add("TRANNO", GetType(Integer))
                    .Add("TRANDATE", GetType(Date))
                    .Add("REFTYPE", GetType(String))
                    .Add("REFNO", GetType(Integer))
                    .Add("RUNNO", GetType(String))
                    .Add("AMOUNT", GetType(Double))
                    .Add("REMARK", GetType(String))
                End With
                gridView.DataSource = dtGridView
                gridView.ColumnHeadersVisible = False
                With gridView
                    .Columns("TRANNO").Width = txtTranNo_NUM_MAN.Width + 1
                    .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDATE").Width = dtpTranDate.Width + 1
                    .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("REFTYPE").Width = cmbMode.Width + 1
                    .Columns("REFNO").Width = txtRefNo_NUM_MAN.Width + 1
                    .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("RUNNO").Width = txtRunno.Width + 1
                    .Columns("AMOUNT").Width = txtAmount_AMT_MAN.Width + 1
                    .Columns("AMOUNT").DefaultCellStyle.Format = "##,##,##,###.00"
                    .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("REMARK").Width = txtRemark.Width
                End With
            End If
            If type = EntryType.Debit Then
                cmbMode.Text = "DUE"
            Else
                cmbMode.Text = "ADVANCE"
            End If
        End If
        CalcBalance()
        txtTranNo_NUM_MAN.Focus()
    End Sub

    Private Sub txtRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If txtTranNo_NUM_MAN.Text = "" Then
                MsgBox("Tranno should not empty", MsgBoxStyle.Information)
                txtTranNo_NUM_MAN.Focus()
                Exit Sub
            End If
            If checkRefno() Then Exit Sub
            If Val(txtAmount_AMT_MAN.Text) = 0 Then
                MsgBox("Amount Should not empty", MsgBoxStyle.Information)
                txtAmount_AMT_MAN.Focus()
                Exit Sub
            End If
            If Val(txtAmount_AMT_MAN.Text) > amt Then
                MsgBox("Invalid Amount", MsgBoxStyle.Information)
                txtAmount_AMT_MAN.Focus()
                Exit Sub
            End If
            If EditFlag Then
                With gridView.Rows(0)
                    .Cells("TRANNO").Value = txtTranNo_NUM_MAN.Text
                    .Cells("TRANDATE").Value = dtpTranDate.Value.Date.ToString("yyyy-MM-dd")
                    .Cells("REFTYPE").Value = cmbMode.Text
                    .Cells("REFNO").Value = txtRefNo_NUM_MAN.Text
                    .Cells("RUNNO").Value = txtRunno.Text
                    .Cells("AMOUNT").Value = txtAmount_AMT_MAN.Text
                    .Cells("REMARK").Value = txtRemark.Text
                    dtGridView.AcceptChanges()
                End With
            Else
                Dim ro As DataRow = dtGridView.NewRow
                ro.Item("TRANNO") = txtTranNo_NUM_MAN.Text
                ro.Item("TRANDATE") = dtpTranDate.Value.Date.ToString("yyyy-MM-dd")
                ro.Item("REFTYPE") = cmbMode.Text
                ro.Item("REFNO") = txtRefNo_NUM_MAN.Text
                ro.Item("RUNNO") = txtRunno.Text
                ro.Item("AMOUNT") = txtAmount_AMT_MAN.Text
                ro.Item("REMARK") = txtRemark.Text
                dtGridView.Rows.Add(ro)
                dtGridView.AcceptChanges()
            End If
            ''CLEAR
            CalcBalance()
            objGPack.TextClear(Me)
            If type = EntryType.Debit Then
                cmbMode.Text = "DUE"
            Else
                cmbMode.Text = "ADVANCE"
            End If
            dtpTranDate.Value = GetEntryDate(GetServerDate)
            txtTranNo_NUM_MAN.Focus()
        End If
    End Sub

    Private Sub cmbMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMode.SelectedIndexChanged
        genRunno()
    End Sub

    Private Sub txtRefNo_NUM_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefNo_NUM_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            checkRefno()
        End If
    End Sub

    Private Sub txtRefNo_NUM_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefNo_NUM_MAN.TextChanged
        genRunno()
    End Sub

    Private Sub txtRunno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRunno.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtAmount_AMT_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount_AMT_MAN.GotFocus
        'txtAmount_AMT_MAN.Text = IIf(Val(lblBalanceAmt.Text) <> 0, Format(Val(lblBalanceAmt.Text), "0.00"), Nothing)
    End Sub

    Private Sub txtAmount_AMT_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount_AMT_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim _amt As Double = Val(dtGridView.Compute("SUM(AMOUNT)", Nothing).ToString)
            If Val(txtAmount_AMT_MAN.Text) - _amt > Val(lblBalanceAmt.Text) Then
                If Val(lblBalanceAmt.Text) = 0 Then
                    txtAmount_AMT_MAN.Clear()
                    Exit Sub
                End If
                MsgBox("Invalid Amount", MsgBoxStyle.Information)
                txtAmount_AMT_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Val(lblBalanceAmt.Text) <> 0 Then
            MsgBox("You must enter the detail abount balance Amount " & lblBalanceAmt.Text & "", MsgBoxStyle.Information)
            txtTranNo_NUM_MAN.Focus()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.Rows.Count > 0 Then
                e.Handled = True
                funcGetDetails()
                txtTranNo_NUM_MAN.Focus()
            End If
        End If
    End Sub
    Function funcGetDetails()
        With gridView.CurrentRow
            txtTranNo_NUM_MAN.Text = .Cells("TRANNO").Value.ToString
            dtpTranDate.Value = .Cells("TRANDATE").Value
            cmbMode.Text = .Cells("REFTYPE").Value.ToString
            txtRefNo_NUM_MAN.Text = .Cells("REFNO").Value.ToString
            txtRunno.Text = .Cells("RUNNO").Value.ToString
            txtAmount_AMT_MAN.Text = Val(.Cells("AMOUNT").Value.ToString)
            txtRemark.Text = .Cells("REMARK").Value.ToString
            EditFlag = True
        End With
    End Function

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
    End Sub

    Private Sub gridView_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridView.UserDeletingRow
        CalcBalance()
    End Sub

    Private Sub FocusToGrid(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    txtTranNo_NUM_MAN.KeyDown, txtRefNo_NUM_MAN.KeyDown, txtAmount_AMT_MAN.KeyDown, txtRemark.KeyDown
        If e.KeyCode = Keys.Down And gridView.RowCount > 0 Then
            gridView.Focus()
        ElseIf e.KeyCode = Keys.Escape And gridView.RowCount > 0 Then
            btnOk.Focus()
        ElseIf e.KeyCode = Keys.Escape And Not gridView.RowCount > 0 Then
            btnCancel.Focus()
        End If
    End Sub
End Class