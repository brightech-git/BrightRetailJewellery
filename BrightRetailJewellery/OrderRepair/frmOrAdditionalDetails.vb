Imports System.Data.OleDb
Public Class frmOrAdditionalDetails
#Region "VARIABLE"
    Public dtOrAdditionalDetails As New DataTable
    Dim strsql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Public DtView As New DataTable
    Dim Typeid As Integer
    Dim UpdateFlag As Boolean = False
    Dim ValueId As Integer
#End Region
#Region "CONSTRUCTOR "
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.BackColor = frmBackColor
        objGPack.TextClear(Me)
        txtAdRowIndex.Text = ""
        gridView.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With DtView.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TYPENAME", GetType(String))
            .Add("VALUENAME", GetType(String))
        End With

        With gridView
            .DataSource = DtView
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
        End With

        If gridView.RowCount = 0 Then
            If gridView.RowCount > 0 Then
                gridView.Rows(0).Cells("TYPENAME").Value = DBNull.Value
                gridView.Rows(0).Cells("VALUENAME").Value = DBNull.Value
            End If
            Exit Sub
        End If
    End Sub
#End Region

#Region "FORM LOAD"
    Private Sub frmOrAdditionalDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If dtOrAdditionalDetails.Rows.Count > 0 Then
            LoadType()
            cmbValue.Items.Clear()
            strsql = " SELECT VALUENAME FROM " & cnAdminDb & "..ORADVALUEMAST WHERE ISNULL(ACTIVE,'Y') <> 'N'  ORDER BY DISPLAYORDER, VALUENAME"
            objGPack.FillCombo(strsql, cmbValue, True, True)
            If cmbValue.Items.Count > 0 Then
                cmbValue.SelectedIndex = 0
            End If
            DtView = dtOrAdditionalDetails.Copy
            gridView.DataSource = DtView
            gridView.Columns("TYPENAME").Width = 225
            gridView.Columns("VALUENAME").Width = 230
            If gridView.Columns.Contains("KEYNO") Then gridView.Columns("KEYNO").Visible = False
            If gridView.Columns.Contains("ORSNO") Then
                gridView.Columns("ORSNO").Visible = False
                cmbType.Enabled = False
                cmbValue.Enabled = False
            End If
        Else
            Btn_New()
        End If
    End Sub

    Private Sub frmOrAdditionalDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then 'Or cmbType.Focused = True
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            DtView.AcceptChanges()
            dtOrAdditionalDetails = DtView.Copy
            dtOrAdditionalDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub
#End Region

#Region "user define function "
    Function Btn_New()

        gridView.DataSource = DtView
        gridView.Columns("TYPENAME").Width = 225
        gridView.Columns("VALUENAME").Width = 230
        If gridView.Columns.Contains("KEYNO") Then gridView.Columns("KEYNO").Visible = False
        If gridView.Columns.Contains("ORSNO") Then
            gridView.Columns("ORSNO").Visible = False
            cmbType.Enabled = False
            cmbValue.Enabled = False
        End If
        LoadType()
        cmbValue.Items.Clear()
        strsql = " SELECT VALUENAME FROM " & cnAdminDb & "..ORADVALUEMAST WHERE ISNULL(ACTIVE,'Y') <> 'N'  ORDER BY DISPLAYORDER, VALUENAME"
        objGPack.FillCombo(strsql, cmbValue, True, True)
        If cmbValue.Items.Count > 0 Then
            cmbValue.SelectedIndex = 0
        End If
    End Function

    Private Sub cmbType_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbType.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadValue()
        Else
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function LoadType()
        cmbType.Items.Clear()
        strsql = " "
        strsql = " SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE ISNULL(ACTIVE,'Y') <> 'N'  "
        strsql += " AND TYPEID IN ( SELECT TYPEID FROM " & cnAdminDb & "..ORADVALUEMAST WHERE ISNULL(ACTIVE,'') = 'Y') "
        If gridView.Rows.Count > 0 Then
            strsql += " AND TYPENAME NOT IN ("
            For cnt As Integer = 0 To gridView.Rows.Count - 1
                strsql += "'" & gridView.Rows(cnt).Cells("TYPENAME").Value.ToString & "',"
            Next
            strsql += "'')"
        End If
        strsql += " ORDER BY DISPLAYORDER, TYPENAME"
        objGPack.FillCombo(strsql, cmbType, True, True)
        If cmbType.Items.Count > 0 Then
            cmbType.SelectedIndex = 0
        End If
    End Function
    Private Sub cmbValue_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbValue.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim RO As DataRow = Nothing
            If txtAdRowIndex.Text = "" Then
                RO = DtView.NewRow
                If UpdateFlag = True Then
                    For Each dr As DataRow In DtView.Rows
                        If dr.Item("TYPENAME").ToString = cmbType.Text Then
                            DtView.Rows.Remove(dr)
                            UpdateFlag = False
                            Exit For
                        Else
                            Continue For
                        End If
                    Next
                End If
                DuplicateChecker(DtView, cmbType.Text, True)
                If cmbValue.Text <> "" And cmbType.Text <> "" Then
                    RO("TYPENAME") = cmbType.Text
                    RO("VALUENAME") = cmbValue.Text
                    DtView.Rows.Add(RO)
                End If

            Else
                With gridView.Rows(Val(txtAdRowIndex.Text) - 1)
                    'If cmbValue.Text <> "" And cmbType.Text <> "" Then
                    '    .Cells("TYPENAME").Value = cmbType.Text
                    '    .Cells("VALUENAME").Value = cmbValue.Text
                    'End If
                    If UpdateFlag = True Then
                        For Each dr As DataRow In DtView.Rows
                            If dr.Item("TYPENAME").ToString = cmbType.Text Then
                                DtView.Rows.Remove(dr)
                                UpdateFlag = False
                                Exit For
                            Else
                                Continue For
                            End If
                        Next
                    End If
                    DuplicateChecker(DtView, cmbType.Text, True)
                    If cmbValue.Text <> "" And cmbType.Text <> "" Then
                        DtView.Rows.Add(DBNull.Value, cmbType.Text, cmbValue.Text)
                    End If
                End With
            End If
            DtView.AcceptChanges()
            gridView.DataSource = DtView
            gridView.Columns("TYPENAME").Width = 225
            gridView.Columns("VALUENAME").Width = 225
            If gridView.Columns.Contains("KEYNO") Then gridView.Columns("KEYNO").Visible = False
            If gridView.Columns.Contains("ORSNO") Then gridView.Columns("ORSNO").Visible = False
            cmbType.Items.Remove(cmbType.Text)
            If cmbType.Items.Count > 0 Then
                cmbType.SelectedIndex = 0
            End If
        End If
    End Sub
    Private Sub DuplicateChecker(ByVal dtGrid As DataTable, ByVal TypeName As String, ByVal AutoRemove As Boolean)
        If dtGrid.Rows.Count > 0 Then
            With dtGrid
                For i As Integer = 0 To dtGrid.Rows.Count - 1
                    If .Rows(i).Item("TYPENAME").ToString = TypeName Then
                        DtView.Rows.Remove(dtGrid.Rows(i))
                        Exit For
                    End If
                Next
            End With
        End If
    End Sub
    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                cmbType.Items.Add(gridView.Item(1, gridView.CurrentRow.Index).Value.ToString)
                cmbType.Text = gridView.Item(1, gridView.CurrentRow.Index).Value.ToString
                LoadValue()
                cmbValue.Text = gridView.Item(2, gridView.CurrentRow.Index).Value.ToString
                txtAdRowIndex.Text = gridView.CurrentRow.Index
                UpdateFlag = True
                cmbType.Focus()
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            With gridView.CurrentRow
                For i As Integer = 0 To DtView.Rows.Count - 1
                    If .Cells("TYPENAME").Value.ToString = DtView.Rows(i).Item("TYPENAME") And .Cells("VALUENAME").Value.ToString = DtView.Rows(i).Item("VALUENAME") Then
                        DtView.Rows.Remove(DtView.Rows(i))
                        Exit For
                    End If
                Next
            End With
            DtView.AcceptChanges()
            gridView.DataSource = DtView
            gridView.Columns("TYPENAME").Width = 225
            gridView.Columns("VALUENAME").Width = 225
            gridView.Columns("KEYNO").Visible = False
            If cmbType.Items.Count > 0 Then
                cmbType.SelectedIndex = 0
            End If
            LoadType()
        ElseIf e.KeyCode = Keys.Escape Then
            cmbType.Focus()
        End If
    End Sub

    'Function funcGetDetails(ByVal TEMPVALUE As Integer) As Integer
    '    strsql = " SELECT "
    '    strsql += " (SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = I.TYPEID ) AS TYPENAME"
    '    strsql += " VALUENAME "
    '    strsql += " FROM " & cnAdminDb & "..ORADVALUEMAST AS I"
    '    strsql += " WHERE VALUENAME = '" & TEMPVALUE & "' ORDER BY DISPLAYORDER,VALUENAME"
    '    Dim dt As New DataTable
    '    dt.Clear()
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dt)
    '    If Not dt.Rows.Count > 0 Then
    '        Return 0
    '    End If
    '    With dt.Rows(0)
    '        cmbType.Text = .Item("TYPENAME")
    '        cmbValue.Text = .Item("VALUENAME")
    '    End With
    'End Function

    Function LoadValue()
        cmbValue.Items.Clear()
        Typeid = GetSqlValue(cn, "SELECT TYPEID FROM " & cnAdminDb & "..ORADMAST WHERE TYPENAME = '" & cmbType.Text & "'")
        strsql = "SELECT DISTINCT VALUENAME FROM " & cnAdminDb & "..ORADVALUEMAST WHERE TYPEID = " & Typeid & "ORDER BY VALUENAME"
        objGPack.FillCombo(strsql, cmbValue, True, True)
        If cmbValue.Items.Count > 0 Then
            cmbValue.SelectedIndex = 0
        End If
    End Function
    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        LoadValue()
    End Sub

    Private Sub cmbType_GotFocus(sender As Object, e As EventArgs) Handles cmbType.GotFocus
        LoadValue()
    End Sub

    Private Sub cmbValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbValue.KeyPress
        If Char.IsLetter(e.KeyChar) Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
        Me.SelectNextControl(cmbType, True, True, True, True)
    End Sub

    Private Sub gridView_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        DtView.AcceptChanges()
        For cnt As Integer = 0 To gridView.RowCount - 1
            gridView.Rows(cnt).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            gridView.AutoResizeRow(0)
        Next
    End Sub

    Private Sub gridView_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles gridView.UserDeletingRow
        Dim CurrentRow As Integer = Val(gridView.CurrentRow.Index)
        Dim RowId As Integer = Val(gridView.Rows(CurrentRow).Cells("ROWINDEX").Value.ToString)
        If DtView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                If gridView.Rows(i).Cells("ROWINDEX").Value.ToString = RowId Then
                    DtView.Rows.RemoveAt(i) 'currentrow-i
                    DtView.AcceptChanges()
                End If
            Next
            'In future indexout of memory error occur why
        End If
    End Sub


#End Region
End Class