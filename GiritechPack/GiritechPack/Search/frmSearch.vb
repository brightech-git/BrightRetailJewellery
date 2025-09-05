Imports System.Data.OleDb
Public Class frmSearch
    Public Enum GridStyle
        DefaultStyle = 0
        Style1 = 1
        Style2 = 2
    End Enum

    ''ligit green 192, 255, 185
    Dim da As OleDbDataAdapter
    Dim dtKey As New DataTable
    Dim col As DataColumn = Nothing
    Dim ro As DataRow = Nothing
    Dim strSql As String
    Dim oldIndex As Integer = -1
    Dim searchIndex As Integer = 0
    Dim returnIndex As Integer = 0
    Public ReturnValue As String = Nothing
    Public ReturnRow As DataRow = Nothing
    Dim dtGridView As New DataTable
    Dim Must As Boolean = False
    Dim _SortBaseonSrchKey As Boolean = False

    Public Sub New( _
    ByVal title As String _
    , ByVal dtGridData As DataTable _
    , ByVal defaultSearchIndex As Integer _
    , ByVal defaultReturnIndex As Integer _
    , Optional ByVal Style As GridStyle = GridStyle.DefaultStyle _
    , Optional ByVal defaultSearchValue As String = Nothing _
    , Optional ByVal searchKeyPnlVisible As Boolean = True _
    , Optional ByVal returnMust As Boolean = False _
    , Optional ByVal AutoFitColumns As Boolean = True _
    , Optional ByVal AutoFitRows As Boolean = False _
    , Optional ByVal Excel As Boolean = False _
    , Optional ByVal Sort As Boolean = True _
    )
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        dtGridView = dtGridData
        dtGridView.AcceptChanges()
        If AutoFitRows Then
            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        End If
        gridView.DataSource = dtGridView
        searchIndex = defaultSearchIndex
        returnIndex = defaultReturnIndex
        txtSearchString.Text = defaultSearchValue
        pnlSearch.Visible = searchKeyPnlVisible
        btnExport.Visible = Excel
        Must = returnMust
        _SortBaseonSrchKey = Sort
        If Must Then Me.ControlBox = False
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            If dgvCol.Name.Contains("_HIDE") Then dgvCol.Visible = False
        Next
        G_SearchDialogColAutoFit = AutoFitColumns
        InitializeForm()
        Me.Text = title
        Select Case Style
            Case GridStyle.Style1
                Me.BackColor = Color.LightSteelBlue
                gridView.BackgroundColor = Me.BackColor
                gridView.ColumnHeadersDefaultCellStyle.BackColor = Me.BackColor
                gridView.AlternatingRowsDefaultCellStyle.BackColor = Color.GhostWhite
                gridView.DefaultCellStyle.BackColor = Color.FromArgb(225, 237, 255)
            Case GridStyle.Style2
                Me.BackColor = Color.Silver
                gridView.BackgroundColor = Me.BackColor
                gridView.ColumnHeadersDefaultCellStyle.BackColor = Me.BackColor
                gridView.AlternatingRowsDefaultCellStyle.BackColor = Color.GhostWhite
                gridView.DefaultCellStyle.BackColor = Color.Gainsboro
                'gridView.DefaultCellStyle.BackColor = Color.FromArgb(225, 237, 255)
        End Select
        'Me.txtSearchString_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub InitializeForm()
        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = New Size(485, 371)
        Me.MaximumSize = New Size(1000, 371)
        Me.BackColor = Color.LightSteelBlue
        Me.BackColor = SystemColors.Control

        lblTitle.Visible = False

        ''Fill FindMode
        cmbFindMode.Items.Add("STARTS WITH")
        cmbFindMode.Items.Add("ENDS WITH")
        cmbFindMode.Items.Add("CONTAINS")
        cmbFindMode.Items.Add("DOES NOT CONTAINS")
        cmbFindMode.Items.Add("MUST EQUAL")
        cmbFindMode.SelectedIndex = 0

        gridView.EnableHeadersVisualStyles = False
        gridView.ColumnHeadersDefaultCellStyle.BackColor = Me.BackColor
        gridView.BackgroundColor = Me.BackColor
        'gridView.AlternatingRowsDefaultCellStyle.BackColor = Color.GhostWhite
        'gridView.DefaultCellStyle.BackColor = Color.FromArgb(225, 237, 255)
        gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        gridView.ColumnHeadersHeight = 30
        gridView.RowTemplate.Resizable = DataGridViewTriState.False
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font(gridView.Font.Name, gridView.Font.Size, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        dtKey.Columns.Add(New DataColumn("Name", GetType(String)))
        dtKey.Columns.Add(New DataColumn("Caption", GetType(String)))
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Name.Contains("~") Then Continue For
            Select Case UCase(gridView.Columns(cnt).ValueType.ToString)
                Case "SYSTEM.DATETIME"
                    gridView.Columns(cnt).DefaultCellStyle.Format = "dd/MM/yyyy"
                    gridView.Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Case "SYSTEM.STRING"
                    gridView.Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Case "SYSTEM.IMAGE"
                    Continue For
                Case Else
                    gridView.Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End Select
            ro = dtKey.NewRow
            ro!Name = gridView.Columns(cnt).Name
            ro!Caption = gridView.Columns(cnt).HeaderText
            cmbSearchKey.Items.Add(gridView.Columns(cnt).HeaderText)
            dtKey.Rows.Add(ro)
        Next
        cmbSearchKey.SelectedIndex = searchIndex
        cmbSearchKey_LostFocus(Me, New EventArgs)
    End Sub

    Private Sub frmSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Must = False Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        ElseIf e.KeyCode = Keys.F1 Then
            oldIndex = gridView.CurrentRow.Index
            FindNext()
        End If
    End Sub

    Private Sub frmSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        If G_SearchDialogColAutoFit Then
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Else
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If

        Dim totWid As Integer = 33
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Name.ToString.ToUpper.Contains("_HIDE") Then Continue For
            totWid += gridView.Columns(cnt).Width
        Next
        If totWid > Me.Width Then
            Me.Size = New Size(totWid, Me.Height)
        Else
            gridView.Columns(gridView.ColumnCount - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            gridView.Columns(gridView.ColumnCount - 1).Width += Me.Width - totWid
        End If
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point((Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2)
        If Must Then Me.ControlBox = False
        gridView.Select()
    End Sub

    Private Sub txtSearchString_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchString.GotFocus
        txtSearchString.SelectAll()
    End Sub

    Private Sub txtSearchString_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearchString.KeyDown
        If e.KeyCode = Keys.Down Then
            gridView.Select()
        End If
    End Sub

    Private Sub txtSearchString_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchString.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtSearchString_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchString.TextChanged
        If Not gridView.RowCount > 0 Then Exit Sub
        oldIndex = -1
        'If txtSearchString.Text <> "" Then

        'End If
        FindNext()
    End Sub

    'Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    '    If gridView.Columns.Contains("COLOR_HIDE") Then
    '        Dim clr As Color
    '        If gridView.Rows(e.RowIndex).Cells("COLOR_HIDE").Value.ToString <> "" Then
    '            clr = Color.FromName(gridView.Rows(e.RowIndex).Cells("COLOR_HIDE").Value.ToString)
    '            gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = clr
    '        Else
    '            clr = Color.White
    '            gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = clr
    '        End If
    '    End If
    'End Sub

    Private Sub gridView_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles gridView.ColumnHeaderMouseClick
        cmbSearchKey.Text = gridView.Columns(e.ColumnIndex).Name
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If Not gridView.RowCount > 0 Then Me.SelectNextControl(gridView, False, True, True, True)
        Select Case e.KeyChar
            Case Chr(Keys.Enter)
                Dim row As DataRow = dtGridView.NewRow
                For cnt As Integer = 0 To dtGridView.Columns.Count - 1
                    row(dtGridView.Columns(cnt).ColumnName) = gridView.Rows(gridView.CurrentRow.Index).Cells(dtGridView.Columns(cnt).ColumnName).Value
                Next
                ReturnRow = row
                ReturnValue = gridView.CurrentRow.Cells(returnIndex).Value.ToString
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Case Chr(Keys.Back)
                If txtSearchString.Text.Length > 0 Then _
                txtSearchString.Text = txtSearchString.Text.Remove _
                                    (txtSearchString.Text.Length - 1, 1)
            Case Else
                txtSearchString.Text += e.KeyChar
        End Select
    End Sub

    Private Sub FindNext()
        Dim pos As Integer
        Dim loopFlag As Boolean = False
        Dim startIndex As Integer = -1
        'If oldIndex = -1 Then oldIndex = gridView.CurrentRow.Index-1
        If oldIndex = -1 Then
            pos = 0
            oldIndex = 0
        Else
            pos = oldIndex + 1
            loopFlag = True
        End If
        If pos > gridView.RowCount - 1 Then pos = 0
        If Not cmbSearchKey.SelectedIndex >= 0 Then Exit Sub
        While 1 = 1
            If pos = oldIndex And loopFlag Then Exit Sub
            With gridView.Item(dtKey.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString, pos).Value

                Select Case UCase(cmbFindMode.Text)
                    Case "STARTS WITH"
                        If .ToString.StartsWith(txtSearchString.Text) Then
                            gridView.CurrentCell = gridView.Item(gridView.FirstDisplayedCell.ColumnIndex, pos)
                            Exit Sub
                        End If
                    Case "ENDS WITH"
                        If .ToString.EndsWith(txtSearchString.Text) Then
                            gridView.CurrentCell = gridView.Item(gridView.FirstDisplayedCell.ColumnIndex, pos)
                            Exit Sub
                        End If
                    Case "CONTAINS"
                        If .ToString.Contains(txtSearchString.Text) Then
                            gridView.CurrentCell = gridView.Item(gridView.FirstDisplayedCell.ColumnIndex, pos)
                            Exit Sub
                        End If
                    Case "DOES NOT CONTAINS"
                        If Not .ToString.Contains(txtSearchString.Text) Then
                            gridView.CurrentCell = gridView.Item(gridView.FirstDisplayedCell.ColumnIndex, pos)
                            Exit Sub
                        End If
                    Case "MUST EQUAL"
                        If .ToString = txtSearchString.Text Then
                            Dim dt As New DataTable()
                            dt = TryCast(gridView.DataSource, DataTable)
                            If dt IsNot Nothing Then
                                Dim dv As DataView
                                dv = dt.DefaultView
                                dv.RowFilter = ("" & dtKey.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString & "='" & Trim(txtSearchString.Text) & "'")
                                dt = dv.ToTable
                                gridView.DataSource = dt
                            End If

                            'gridView.CurrentCell = gridView.Item(gridView.FirstDisplayedCell.ColumnIndex, pos)
                            Exit Sub
                        End If
                End Select
            End With
            pos += 1
            If pos > gridView.RowCount - 1 Then pos = 0
            If loopFlag = False And pos = oldIndex Then Exit Sub
        End While
    End Sub

    Private Sub cmbSearchKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSearchKey.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSearchKey_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchKey.LostFocus
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If cmbSearchKey.Text <> "" And _SortBaseonSrchKey Then
            gridView.Sort(gridView.Columns(dtKey.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString), System.ComponentModel.ListSortDirection.Ascending)
        End If
        oldIndex = -1
        FindNext()
    End Sub

    Private Sub cmbFindMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbFindMode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSearchKey_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchKey.TextChanged
        If cmbSearchKey.Text <> "" And _SortBaseonSrchKey Then
            gridView.Sort(gridView.Columns(cmbSearchKey.Text), System.ComponentModel.ListSortDirection.Ascending)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "", gridView, BrightPosting.GExport.GExportType.Export, , , , , "dd/MM/yyyy")
        End If
    End Sub
End Class

