Imports System.Data.OleDb
Public Class frmAdjCrCard
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Public dtGridCreditCard As New DataTable

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmSyncMast_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        gridView.RowTemplate.Height = 21
        gridView.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridViewTotal.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        strSql = " SELECT NAME,CONVERT(NUMERIC(15,2),NULL)AS AMOUNT FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R'"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If gridView.Rows.Count > 0 Then gridView.Focus() : gridView.CurrentCell = gridView.Rows(0).Cells("AMOUNT") : Exit Sub
        gridView.DataSource = dtGrid
        gridView.Columns("NAME").Width = 350
        gridView.Columns("NAME").ReadOnly = True
        gridView.Columns("AMOUNT").Width = 118
        gridView.Columns("AMOUNT").ReadOnly = True
        If gridView.RowCount > 0 Then
            gridView.CurrentCell = gridView.Rows(0).Cells("AMOUNT")
        End If
    End Sub

    Private Sub funcTotal()
        gridViewTotal.DefaultCellStyle.SelectionBackColor = gridView.BackgroundColor
        dtGridCreditCard.AcceptChanges()
        Dim amt As Double = Nothing
        Dim cramt As Double = Nothing
        For cnt As Integer = 0 To gridView.RowCount - 1
            amt += Val(gridView.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        gridViewTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub
    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        funcTotal()
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "AMOUNT"
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("AMOUNT")
                Dim pt As Point = gridView.Location
                txtGrid.Visible = True
                pt = pt + gridView.GetCellDisplayRectangle(gridView.Columns("AMOUNT").Index, gridView.CurrentRow.Index, False).Location
                txtGrid.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("AMOUNT").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = gridView.Columns("AMOUNT").Width
                txtGrid.Focus()
                funcTotal()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "AMOUNT"
                gridView.CurrentCell.Value = IIf(Val(txtGrid.Text) = 0, DBNull.Value, Val(txtGrid.Text))
        End Select
        txtGrid.Clear()
        txtGrid.Visible = False
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        funcTotal()
    End Sub

    'Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
    '    'If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("AMOUNT").Index And Not e.Control Is Nothing Then
    '    '    Dim tb As TextBox = CType(e.Control, TextBox)
    '    '    '---add an event handler to the TextBox control---
    '    '    AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
    '    'End If
    '    If Not e.Control Is Nothing Then
    '        Dim tb As TextBox = CType(e.Control, TextBox)
    '        If gridView.Columns(gridView.CurrentCell.ColumnIndex).HeaderText.Contains("PER") Then
    '            tb.Tag = "PER"
    '        Else
    '            tb.Tag = "AMOUNT"
    '        End If
    '        AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
    '    End If
    'End Sub

    'Private Sub TextKeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)
    '    If CType(sender, TextBox).Tag = "AMOUNT" Then
    '        textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Amount)
    '    ElseIf CType(sender, TextBox).Tag = "PER" Then
    '        textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Percentage)
    '    Else
    '        textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Weight)
    '    End If
    'End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        With dtGridCreditCard
            .Columns.Add("CARDTYPE", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
        End With
        gridView.DataSource = dtGridCreditCard
        FormatGridColumns(gridView)
        StyleGridCreditCard(gridView)
        Dim dtGridCreditCardTotal As New DataTable
        dtGridCreditCardTotal = dtGridCreditCard.Copy
        dtGridCreditCardTotal.Rows.Clear()
        dtGridCreditCardTotal.Rows.Add()
        dtGridCreditCardTotal.Rows(0).Item("CARDTYPE") = "Total"
        With gridViewTotal
            .DataSource = dtGridCreditCardTotal
            For Each col As DataGridViewColumn In gridViewTotal.Columns
                With gridView.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridViewTotal)
        StyleGridCreditCard(gridViewTotal)
    End Sub
    Private Sub StyleGridCreditCard(ByVal grid As DataGridView)
        If grid.DataSource Is Nothing Then Exit Sub
        gridView.DefaultCellStyle.SelectionBackColor = gridView.BackgroundColor
        With grid
            .Columns("CARDTYPE").Width = 350
            .Columns("AMOUNT").Width = 118
            .Columns("CARDTYPE").DefaultCellStyle.SelectionBackColor = SystemColors.Window
            .Columns("CARDTYPE").DefaultCellStyle.SelectionForeColor = SystemColors.Window
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        gridView.Focus()
        If gridView.RowCount > 0 Then
            gridView.CurrentCell = gridView.Rows(0).Cells("AMOUNT")
        End If
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnOk.Focus()
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
        End If
    End Sub
    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With gridView
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "AMOUNT"
                        If Val(txtGrid.Text) > 0 Then
                            .CurrentCell.Value = IIf(Val(txtGrid.Text) = 0, DBNull.Value, Val(txtGrid.Text))
                        End If
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            .CurrentCell.Value = IIf(Val(txtGrid.Text) = 0, DBNull.Value, Val(txtGrid.Text))
                            btnOk.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("AMOUNT")
                        End If
                End Select
            End With
        End If
    End Sub

    Private Sub txtGrid_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.Leave
        With gridView
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "AMOUNT"
                    If Val(txtGrid.Text) > 0 Then
                        .CurrentCell.Value = IIf(Val(txtGrid.Text) = 0, DBNull.Value, Val(txtGrid.Text))
                    End If
            End Select
        End With
    End Sub
End Class