Imports System.Data.OleDb
Public Class frmAccOutstanding
    Dim strSql As String

    Public adjAmt As Double

    Private Sub frmAccOutstanding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Val(lblBalance.Text) > 0 Then
                If MsgBox("Amount not tallied,Sure Want to Close?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            ElseIf Val(lblBalance.Text) = 0 Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Public Sub StyleGridOutStDt()
        With gridOutSt
            .Columns("RUNNO").HeaderText = "INV.NO"
            .Columns("TRANTYPE").HeaderText = "TYPE"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT").DefaultCellStyle.Format = "0.00"
            .Columns("PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PAYMENT").DefaultCellStyle.Format = "0.00"
            .Columns("BALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALANCE").DefaultCellStyle.Format = "0.00"
            .Columns("ADJUST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ADJUST").DefaultCellStyle.Format = "0.00"
            gridOutSt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridOutSt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End With
    End Sub

    Public Sub AutoAdjust()
        Dim amt As Double = adjAmt
        lblBalance.Text = Format(0, "0.00")
        For Each ro As DataRow In CType(gridOutSt.DataSource, DataTable).Rows
            If amt = 0 Then
                ro!ADJUST = DBNull.Value    
            ElseIf Val(ro!BALANCE.ToString) <= amt Then
                ro!ADJUST = ro!BALANCE
                amt -= Val(ro!BALANCE.ToString)
            ElseIf Val(ro!BALANCE.ToString) > amt Then
                ro!ADJUST = amt
                amt = 0
            End If
        Next
    End Sub

    Public Sub LoadGridOutStDt(ByVal dt As DataTable)
        gridOutSt.DataSource = dt
        StyleGridOutStDt()
        gridOutSt.Select()
    End Sub

    Private Sub frmAccOutstanding_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'LoadGridOutStDt(dtGridView)
    End Sub

    Private Sub gridOutSt_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridOutSt.EditingControlShowing
        If Me.gridOutSt.CurrentCell.ColumnIndex = gridOutSt.Columns("ADJUST").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            '---add an event handler to the TextBox control---
            AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
            AddHandler tb.Leave, AddressOf TextBox_Leave
            AddHandler tb.TextChanged, AddressOf TextBox_TextChanged
        End If
    End Sub

    Private Sub TextBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim adjbal As Double = adjAmt - Val(CType(gridOutSt.DataSource, DataTable).Compute("SUM(ADJUST)", Nothing).ToString)
        If adjbal > 0 Then
            lblBalance.Text = Format(adjbal, "0.00")
        End If
    End Sub

    Private Sub TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim adjbal As Double = adjAmt - Val(CType(gridOutSt.DataSource, DataTable).Compute("SUM(ADJUST)", Nothing).ToString)
        If adjbal > 0 Then
            lblBalance.Text = Format(adjbal, "0.00")
        End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "." And CType(sender, TextBox).Text.Contains(".") Then
            e.Handled = True
            Return
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "0" To "9", ChrW(Keys.Back), ".", _
                        ChrW(Keys.Enter), ChrW(Keys.Escape)
                Dim adjSum As Double = adjAmt + Val(gridOutSt.CurrentRow.Cells("ADJUST").Value.ToString) _
                - Val(CType(gridOutSt.DataSource, DataTable).Compute("SUM(ADJUST)", "ADJUST IS NOT NULL").ToString)
                Dim adjbal As Double = adjAmt _
                - Val(CType(gridOutSt.DataSource, DataTable).Compute("SUM(ADJUST)", Nothing).ToString)
                If adjbal > 0 Then
                    lblBalance.Text = Format(adjbal, "0.00")
                End If
                If Val(CType(sender, TextBox).Text + e.KeyChar) > adjSum Then
                    e.Handled = True
                    Return
                End If
                If Val(gridOutSt.CurrentRow.Cells("BALANCE").Value.ToString) < Val(CType(sender, TextBox).Text + e.KeyChar) Then
                    e.Handled = True
                    Return
                End If
            Case Else
                e.Handled = True
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


    Private Sub gridOutSt_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOutSt.SelectionChanged
            If gridOutSt.CurrentCell Is Nothing Then Exit Sub
        Select Case gridOutSt.CurrentCell.ColumnIndex
            Case 0, 1, 2, 3, 4, 5
                gridOutSt.CurrentCell = gridOutSt.CurrentRow.Cells(6)
        End Select
    End Sub
End Class