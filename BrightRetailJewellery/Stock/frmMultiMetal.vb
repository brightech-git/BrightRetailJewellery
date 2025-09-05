Imports System.Data.OleDb
Public Class frmMultiMetal
    Dim strSql As String
    Dim dtMultiMetal As New DataTable

    Dim WithEvents lstSearch As New ListBox
    Dim searchSender As Control = Nothing
    Dim searchFlag As Boolean = False


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        txtGrid.Visible = False
        gridMultiMetal.RowTemplate.Height = 21
        gridMultiMetal.RowTemplate.Resizable = DataGridViewTriState.False
        gridMultiMetal.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridMultiMetal.DefaultCellStyle.SelectionBackColor = gridMultiMetal.DefaultCellStyle.BackColor
        gridMultiMetal.DefaultCellStyle.SelectionForeColor = gridMultiMetal.DefaultCellStyle.ForeColor
        gridMultiMetal.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ' Add any initialization after the InitializeComponent() call.
        With dtMultiMetal
            .Columns.Add("CATEGORY", GetType(String))
            .Columns.Add("WEIGHT", GetType(Decimal))
            .Columns.Add("WASTPER", GetType(Decimal))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("MCPERGRM", GetType(Double))
            .Columns.Add("MC", GetType(Double))
            .Columns.Add("VALUE", GetType(Double))
        End With
    End Sub

    Private Sub GridStyleMultiMetal()
        With gridMultiMetal
            .Columns("CATEGORY").Width = 250
            .Columns("WEIGHT").Width = 80
            .Columns("WEIGHT").DefaultCellStyle.Format = "0.000"
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("WASTPER").Width = 80
            .Columns("WASTPER").DefaultCellStyle.Format = "0.00"
            .Columns("WASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTPER").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("WASTAGE").Width = 80
            .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("MCPERGRM").Width = 80
            .Columns("MCPERGRM").DefaultCellStyle.Format = "0.00"
            .Columns("MCPERGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MCPERGRM").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("MC").Width = 80
            .Columns("MC").DefaultCellStyle.Format = "0.00"
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("VALUE").Width = 80
            .Columns("VALUE").DefaultCellStyle.Format = "0.00"
            .Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VALUE").SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub frmMultiMetal_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If gridMultiMetal.RowCount > 0 Then
            gridMultiMetal.CurrentCell = gridMultiMetal.Rows(gridMultiMetal.RowCount - 1).Cells(0)
            gridMultiMetal.Focus()
        End If
    End Sub

    Private Sub frmMultiMetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If lstSearch.Visible And (Not searchSender Is Nothing) Then
                searchSender.Select()
                lstSearch.Visible = False
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub frmMultiMetal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not dtMultiMetal.Rows.Count > 0 Then
            dtMultiMetal.Rows.Add()
        End If
        gridMultiMetal.DataSource = dtMultiMetal
        GridStyleMultiMetal()
    End Sub

    Private Sub gridMultiMetal_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridMultiMetal.CellEnter
        Dim pt As Point = gridMultiMetal.Location
        lstSearch.Visible = False
        Select Case gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name
            Case "CATEGORY"
                txtGrid.TextAlign = HorizontalAlignment.Left
            Case "WEIGHT"
                txtGrid.MaxLength = 9
                txtGrid.TextAlign = HorizontalAlignment.Right
            Case "WASTPER"
                txtGrid.MaxLength = 6
                txtGrid.TextAlign = HorizontalAlignment.Right
            Case "WASTAGE"
                txtGrid.MaxLength = 7
                txtGrid.TextAlign = HorizontalAlignment.Right
            Case "MCPERGRM"
                txtGrid.MaxLength = 7
                txtGrid.TextAlign = HorizontalAlignment.Right
            Case "MC"
                txtGrid.MaxLength = 9
                txtGrid.TextAlign = HorizontalAlignment.Right
            Case "VALUE"
                txtGrid.MaxLength = 9
                txtGrid.TextAlign = HorizontalAlignment.Right
        End Select
        txtGrid.Size = New Size(gridMultiMetal.Columns(e.ColumnIndex).Width, txtGrid.Height)
        txtGrid.Location = pt + gridMultiMetal.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
        txtGrid.Text = gridMultiMetal.CurrentCell.FormattedValue
        txtGrid.Visible = True
        txtGrid.Focus()
        txtGrid.SelectAll()
    End Sub

    Private Sub gridMultiMetal_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridMultiMetal.CellLeave
        Select Case gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name
            Case "CATEGORY"
                gridMultiMetal.CurrentCell.Value = txtGrid.Text
            Case "WEIGHT"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
            Case "WASTPER"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
            Case "WASTAGE"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
            Case "MCPERGRM"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
            Case "MC"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
            Case "VALUE"
                gridMultiMetal.CurrentCell.Value = IIf(Val(txtGrid.Text) <> 0, Val(txtGrid.Text), DBNull.Value)
        End Select
        txtGrid.Clear()
        txtGrid.Visible = False
    End Sub

    'Private Sub gridMultiMetal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMultiMetal.GotFocus
    '    gridMultiMetal.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight
    '    gridMultiMetal.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
    'End Sub

    'Private Sub gridMultiMetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMultiMetal.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        e.Handled = True
    '    End If
    'End Sub

    'Private Sub gridMultiMetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMultiMetal.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If gridMultiMetal.RowCount > 0 Then
    '            gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells(gridMultiMetal.CurrentCell.ColumnIndex)
    '        End If
    '    End If
    'End Sub

    'Private Sub gridMultiMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMultiMetal.LostFocus
    '    gridMultiMetal.DefaultCellStyle.SelectionBackColor = gridMultiMetal.DefaultCellStyle.BackColor
    '    gridMultiMetal.DefaultCellStyle.SelectionForeColor = gridMultiMetal.DefaultCellStyle.ForeColor
    'End Sub

    Private Sub txtGrid_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.GotFocus
        txtGrid.BackColor = focusColor
        txtGrid.SelectAll()
    End Sub

    Private Sub txtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyDown
        e.Handled = True
        searchFlag = False

        If e.KeyCode = Keys.Left Then
            If gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name <> "CATEGORY" Then
                gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells(gridMultiMetal.CurrentCell.ColumnIndex - 1)
            End If
        ElseIf e.KeyCode = Keys.Right Then
            If gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name <> "VALUE" Then
                If gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex + 1).Visible Then
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells(gridMultiMetal.CurrentCell.ColumnIndex + 1)
                End If
            End If
        ElseIf e.KeyCode = Keys.Down Then
            If lstSearch.Visible Then
                lstSearch.Select()
            ElseIf gridMultiMetal.CurrentRow.Index <> gridMultiMetal.RowCount - 1 Then
                gridMultiMetal.CurrentCell = gridMultiMetal.Rows(gridMultiMetal.CurrentRow.Index + 1).Cells(gridMultiMetal.CurrentCell.ColumnIndex)
            End If
        ElseIf e.KeyCode = Keys.Up Then
            If gridMultiMetal.CurrentRow.Index <> 0 Then
                gridMultiMetal.CurrentCell = gridMultiMetal.Rows(gridMultiMetal.CurrentRow.Index - 1).Cells(gridMultiMetal.CurrentCell.ColumnIndex)
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            DeleteRow()
        ElseIf e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            lstSearch.Visible = False
        ElseIf e.KeyCode = Keys.Insert Then
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
            showSearch(txtGrid, strSql)
        End If
    End Sub

    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function

    Private Sub ShowSearch(ByVal sender As Control, ByVal str As String)
        lstSearch.Items.Clear()
        Dim dtSearch As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSearch)
        For Each ro As DataRow In dtSearch.Rows
            lstSearch.Items.Add(ro(0))
        Next
        searchSender = sender
        Dim pt As New Point(GetControlLocation(sender, New Point))
        Me.Controls.Add(lstSearch)
        lstSearch.Location = New Point(pt.X, pt.Y + sender.Height)

        lstSearch.Size = New Size(sender.Width, 120)
        lstSearch.BringToFront()
        txtGrid.BringToFront()

        If lstSearch.Items.Count > 0 Then
            lstSearch.Visible = True
        Else
            lstSearch.Visible = False
        End If
    End Sub

    Private Sub DeleteRow()
        If gridMultiMetal.RowCount > 0 Then
            If gridMultiMetal.CurrentRow.Index = gridMultiMetal.RowCount - 1 Then
                txtGrid.Clear()
                For Each dgvCol As DataGridViewColumn In gridMultiMetal.Columns
                    gridMultiMetal.CurrentRow.Cells(dgvCol.Name).Value = DBNull.Value
                Next
                gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells(0)
            Else
                dtMultiMetal.AcceptChanges()
                dtMultiMetal.Rows.Remove(dtMultiMetal.Rows(gridMultiMetal.CurrentRow.Index))
                dtMultiMetal.AcceptChanges()
                gridMultiMetal.CurrentCell = gridMultiMetal.Rows(gridMultiMetal.RowCount - 1).Cells(0)
                gridMultiMetal.Focus()
            End If
        End If
    End Sub

    Private Function ValidationCategory(ByVal catName As String) As Boolean
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
        strSql += " AND CATNAME = '" & catName & "'"
        If objGPack.DupCheck(strSql) = False Then
            MsgBox("")
            Return True
        End If
    End Function


    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Select Case gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name
                Case "CATEGORY"
                    If ValidationCategory(txtGrid.Text) = False Then
                        gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("WEIGHT")
                    End If
                Case "WEIGHT"
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("WASTPER")
                Case "WASTPER"
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("WASTAGE")
                Case "WASTAGE"
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("MCPERGRM")
                Case "MCPERGRM"
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("MC")
                Case "MC"
                    gridMultiMetal.CurrentCell = gridMultiMetal.CurrentRow.Cells("VALUE")
                Case "VALUE"
                    If gridMultiMetal.CurrentRow.Index = gridMultiMetal.RowCount - 1 Then
                        ''NEW ROW
                        dtMultiMetal.Rows.Add()
                    End If
                    gridMultiMetal.CurrentCell = gridMultiMetal.Rows(gridMultiMetal.CurrentRow.Index + 1).Cells("CATEGORY")
            End Select
        Else
            searchFlag = True
            Select Case gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name
                Case "WEIGHT"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Weight)
                Case "WASTPER"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Percentage)
                Case "WASTAGE"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Weight)
                Case "MCPERGRM"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Amount)
                Case "MC"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Amount)
                Case "VALUE"
                    textBoxKeyPressValidator(txtGrid, e, Datatype.Amount)
            End Select
        End If
    End Sub

    Private Sub txtGrid_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.LostFocus
        txtGrid.BackColor = lostFocusColor
    End Sub

    Private Sub ShowSearch()
        Select Case gridMultiMetal.Columns(gridMultiMetal.CurrentCell.ColumnIndex).Name
            Case "CATEGORY"
                strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
                strSql += " AND CATNAME LIKE '" & txtGrid.Text & "%'"
                ShowSearch(txtGrid, strSql)
            Case Else
                lstSearch.Visible = False
                Exit Sub
        End Select
    End Sub

    Private Sub txtGrid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.TextChanged
        If txtGrid.Text <> "" Then
            If searchFlag Then
                ShowSearch()
            End If
        Else
            lstSearch.Visible = False
        End If
    End Sub

    Private Sub lstSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSearch.GotFocus
        lstSearch.SelectedIndex = 0
    End Sub

    Private Sub lstSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            searchSender.Text = lstSearch.Text
            If Not searchSender Is Nothing Then
                searchSender.Select()
            End If
            lstSearch.Visible = False
        End If
    End Sub
End Class