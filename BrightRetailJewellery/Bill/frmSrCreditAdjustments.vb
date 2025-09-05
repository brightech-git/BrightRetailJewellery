Imports System.Data.OleDb
Public Class frmSrCreditAdjustments
    Dim strSql As String
    Public dtGridSrCrAdj As New DataTable
    Public Sub New(ByVal start As Boolean)
        InitializeComponent()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridSrCr.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridSrCrTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' Add any initialization after the InitializeComponent() call.
        With dtGridSrCrAdj
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("ACCODE", GetType(String))
        End With
        gridSrCr.DataSource = dtGridSrCrAdj
        FormatGridColumns(gridSrCr)
        StyleGridSrCr(gridSrCr)
        Dim dtGridSrCrTotal As New DataTable
        dtGridSrCrTotal = dtGridSrCrAdj.Copy
        gridSrCrTotal.DataSource = dtGridSrCrTotal
        dtGridSrCrTotal.Rows.Clear()
        dtGridSrCrTotal.Rows.Add()
        dtGridSrCrTotal.Rows(0).Item("RUNNO") = "Total"
        With gridSrCrTotal
            .DataSource = dtGridSrCrTotal
            For Each col As DataGridViewColumn In gridSrCr.Columns
                With gridSrCrTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridSrCrTotal)
        StyleGridSrCr(gridSrCrTotal)
    End Sub
    Public Sub StyleGridSrCr(ByVal grid As DataGridView)
        gridSrCr.DefaultCellStyle.SelectionBackColor = grpSrCr.BackgroundColor
        With grid
            .Columns("RUNNO").Width = txtRefNo.Width + 1
            .Columns("AMOUNT").Width = txtAmount_AMT.Width + 1
            .Columns("ACCODE").Visible = False
        End With
    End Sub

    Private Sub frmSrCreditAdjustments_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridSrCrAdj.AcceptChanges()
            txtRefNo.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmSrCreditAdjustments_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAmount_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtRefNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefNo.GotFocus
        txtAmount_AMT.Focus()
    End Sub

    Private Sub txtKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtAmount_AMT.KeyDown, txtRefNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridSrCr.RowCount > 0 Then
                gridSrCr.Focus()
            End If
        End If
    End Sub

    Private Sub gridSrCr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridSrCr.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridSrCr.RowCount > 0 Then Exit Sub
            gridSrCr.CurrentCell = gridSrCr.CurrentRow.Cells("RUNNO")
            txtSrCrRowIndex.Text = gridSrCr.CurrentRow.Index
            txtRefNo.Text = gridSrCr.CurrentRow.Cells("RUNNO").FormattedValue
            txtAmount_AMT.Text = gridSrCr.CurrentRow.Cells("AMOUNT").FormattedValue
            txtAmount_AMT.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            If Not gridSrCr.RowCount > 0 Then Exit Sub
            If gridSrCr.CurrentRow.Index = 0 Then txtRefNo.Focus()
        End If
    End Sub


    Public Sub CalcGridSrCrTotal()
        dtGridSrCrAdj.AcceptChanges()
        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridSrCrAdj.Rows
            amt += Val(ro!AMOUNT.ToString)
        Next
        If gridSrCrTotal.Rows.Count > 0 Then
            gridSrCrTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
        End If
    End Sub

    Private Sub txtAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSrCrRowIndex.Text = "" Then
                txtAmount_AMT.Clear()
                Exit Sub
            End If
            If Val(txtAmount_AMT.Text) > Val(gridSrCr.Rows(Val(txtSrCrRowIndex.Text)).Cells("AMOUNT").Value.ToString) Then
                MsgBox("Invalid Amount", MsgBoxStyle.Information)
                txtAmount_AMT.Focus()
                Exit Sub
            End If
            With gridSrCr.Rows(Val(txtSrCrRowIndex.Text))
                .Cells("AMOUNT").Value = IIf(Val(txtAmount_AMT.Text) <> 0, Val(txtAmount_AMT.Text), DBNull.Value)
            End With
            CalcGridSrCrTotal()
            objGPack.TextClear(grpSrCr)
            txtAmount_AMT.Select()
        End If
    End Sub

    Private Sub gridSrCr_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridSrCr.UserDeletedRow
        CalcGridSrCrTotal()
    End Sub
End Class