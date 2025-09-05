Imports System.Data.OleDb
Public Class frmSubLedger
    Dim strSql As String
    Public Totalamt As Double
    Dim ro As New DataTable
    Dim DtAcname As New DataTable
    Public Entrydate As Date
    Public Entrytype As String
    Public Bcostid As String = ""
    Public Ischkbudget As Boolean = False
    Dim lblbud As String = ""
    Dim chkval As Decimal
    Dim budval As String
    Public IsSubLedger As Boolean = False
    Dim dtsubledger As New DataTable
    Dim dtEditAmt As New DataTable

    Private Sub frmSubLedger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            gridView_OWN.Update()
            If gridView_OWN.RowCount > 1 Then
                For Each ro As DataGridViewRow In gridView_OWN.Rows
                    If IIf(IsDBNull(ro.Cells("ACNAME").Value.ToString), "", ro.Cells("ACNAME").Value.ToString) = "" Or Val(ro.Cells("AMOUNT").Value.ToString) = 0 Then
                        gridView_OWN.Rows.Remove(gridView_OWN.Rows(ro.Index))
                    End If
                Next
            End If
            If Val(CType(gridView_OWN.DataSource, DataTable).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) <> Totalamt Then MsgBox("TOTAL AMOUNT NOT TALLY", MsgBoxStyle.Critical) : e.Handled = True : Exit Sub
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'gridView_OWN.Focus()
    End Sub
    Public Sub New(ByVal dt1 As DataTable)
        InitializeComponent()
       

        'DgvSearch.DataSource = Nothing
        'DgvSearch.DataSource = DtAcname
        'DgvSearch.Columns("AMOUNT").Visible = False
        'DgvSearch.ColumnHeadersVisible = False
        'DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        'gridView_OWN.Select()
        'DgvSearch.Visible = False
    End Sub

    Private Sub StyleGrid(ByVal gridView As DataGridView)
        With gridView
            .Columns("ACNAME").Width = cmbAcname.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").Width = txtAmount_AMT.Width + 1
            gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            'gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End With
    End Sub
    Private Sub CalcSubledgerTotal()
        Dim Amt As Double = Nothing
        For i As Integer = 0 To gridView_OWN.RowCount - 1
            With gridView_OWN.Rows(i)
                '.DefaultCellStyle.BackColor = SystemColors.HighlightText 
                Amt += Val(.Cells("AMOUNT").Value.ToString)
            End With
        Next
        With GridSubledgerTotal.Rows(0)
            .Cells("ACNAME").Value = "TOTAL"
            .Cells("AMOUNT").Value = IIf(Amt <> 0, Amt, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End With
    End Sub

    Private Sub frmSubLedger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CmbAcname.Focused Then Exit Sub
            If txtAmount_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSubLedger_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtsubledger = New DataTable(DtAcname.TableName)
        With dtsubledger
            .Columns.Add("ACNAME", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
        End With
        dtsubledger.AcceptChanges()
        gridView_OWN.DataSource = dtsubledger
        gridView_OWN.ColumnHeadersVisible = False
        'FormatGridColumns(gridView_OWN)
        StyleGrid(gridView_OWN)

        Dim dtGridsubTotal As New DataTable
        dtGridsubTotal = dtsubledger.Copy
        dtGridsubTotal.Rows.Clear()
        dtGridsubTotal.Rows.Add()
        GridSubledgerTotal.ColumnHeadersVisible = False
        GridSubledgerTotal.DataSource = dtGridsubTotal
        For Each col As DataGridViewColumn In gridView_OWN.Columns
            With GridSubledgerTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        CalcSubledgerTotal()
        StyleGrid(GridSubledgerTotal)

        If DtAcname.Rows.Count > 0 Then
            IsSubLedger = True
            'cmbAcname.Items.Clear()
            cmbAcname.DataSource = DtAcname
            cmbAcname.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbAcname.AutoCompleteSource = AutoCompleteSource.ListItems
            cmbAcname.DisplayMember = "ACNAME"
            'For i As Integer = 0 To DtAcname.Rows.Count - 1
            'CmbAcname.Items.Add(DtAcname.Rows(i).Item(0).ToString)
            'Next
        End If
        If Not dtEditAmt Is Nothing Then
            If dtEditAmt.Rows.Count > 0 Then
                For i As Integer = 0 To dtEditAmt.Rows.Count - 1
                    If Val(dtEditAmt.Rows(i).Item("AMOUNT").ToString) <> 0 Then
                        Dim ro As DataRow = Nothing
                        ro = dtsubledger.NewRow
                        ro("ACNAME") = dtEditAmt.Rows(i).Item("ACNAME").ToString
                        ro("AMOUNT") = Val(dtEditAmt.Rows(i).Item("AMOUNT").ToString)
                        dtsubledger.Rows.Add(ro)
                        dtsubledger.AcceptChanges()
                    End If
                Next
            End If
        End If

        txtRowIndex.Text = ""
        cmbAcname.Focus()
    End Sub
    'Public Sub StylegridView_OWNDt(ByVal gridview As DataGridView)
    '    With gridview

    '        .Columns("ACNAME").HeaderText = "ACNAME"
    '        .Columns("ACNAME").Width = CmbAcname.Width
    '        .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '        .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
    '        .Columns("AMOUNT").Width = txtAmount_AMT.Width
    '        gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    '        gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    '    End With
    'End Sub

    Public Sub LoadGridOutStDt(ByVal dt As DataTable, Optional ByVal dtedit As DataTable = Nothing)
        DtAcname = New DataTable
        DtAcname = dt
        dtEditAmt = New DataTable
        If Not dtedit Is Nothing Then dtedit.AcceptChanges()
        dtEditAmt = dtedit
        If DtAcname.Rows.Count > 0 Then
            'CmbAcname.Items.Clear()
            cmbAcname.DataSource = DtAcname
            'cmbAcname.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbAcname.AutoCompleteSource = AutoCompleteSource.ListItems
            cmbAcname.DisplayMember = "ACNAME"
            'For i As Integer = 0 To DtAcname.Rows.Count - 1
            'cmbAcname.Items.Add(DtAcname.Rows(i).Item(0).ToString)
            'Next

        End If
    End Sub

    Private Sub gridView_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells(gridView_OWN.CurrentCell.ColumnIndex)
        'End If
    End Sub


    Private Sub txtGrid_OWN_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.IsInputKey = True
        ElseIf e.KeyCode = Keys.Down Then
            e.IsInputKey = True
        End If
    End Sub


    'Private Sub KeyEnter(ByVal e As KeyPressEventArgs)
    '    With gridView_OWN
    '        'If .CurrentCell Is Nothing Then Exit Sub
    '        Dim budgetvalue As Decimal = 0
    '        Select Case .Columns(.CurrentCell.ColumnIndex).Name
    '            Case "ACNAME"
    '                For i As Integer = 0 To DtAcname.Rows.Count - 1
    '                    If txtGrid_OWN.Text.Trim.ToUpper = DtAcname.Rows(i).Item("ACNAME").ToString.Trim.ToUpper Then
    '                        gridView_OWN.CurrentCell.Value = txtGrid_OWN.Text
    '                        If Ischkbudget Then
    '                            budval = 0
    '                            Dim mPartycode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & txtGrid_OWN.Text.ToString() & "'")
    '                            budval = Budgetcheck(mPartycode, Entrydate, Entrytype, True, Bcostid)
    '                            If budval <> "" And budval <> "0" Then budgetvalue = Val(budval.Substring(0, budval.Length - 3).ToString) Else budgetvalue = 0
    '                            If budgetvalue <> 0 And budval.Contains("Dr") Then lblbud = "Budget :" + budval.ToString : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) Else lblbud = ""
    '                            If budgetvalue <> 0 And budval.Contains("Cr") Then lblbud = "Budget :" + budval.ToString : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) Else lblbud = ""
    '                        End If
    '                        .CurrentCell = .CurrentRow.Cells("AMOUNT")
    '                        txtGrid_OWN.SelectAll()
    '                        Exit Sub
    '                    End If
    '                Next
    '                txtGrid_OWN.SelectAll()

    '            Case "AMOUNT"
    '                If budval <> "" And budval <> "0" Then budgetvalue = Val(budval.Substring(0, budval.Length - 3).ToString) Else budgetvalue = 0

    '                If budgetvalue <> 0 Then
    '                    If (Val(txtGrid_OWN.Text.ToString) > chkval) And Val(txtGrid_OWN.Text.ToString) <> 0 Then
    '                        MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
    '                        If Not OTPCHECK("BUDGETVALUE", cnCostId, userId) Then Exit Sub
    '                        'Exit Sub
    '                    End If
    '                End If

    '                gridView_OWN.CurrentCell.Value = Val(txtGrid_OWN.Text)
    '                If .CurrentCell.RowIndex <> .RowCount - 1 Then
    '                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("ACNAME")
    '                    budval = 0
    '                    txtGrid_OWN.Focus()
    '                    txtGrid_OWN.SelectAll()
    '                    Exit Sub
    '                End If

    '        End Select
    '    End With
    'End Sub



    Private Sub txtAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtAmount_AMT.Text) = 0 Then
                MsgBox("Amount coult not zero", MsgBoxStyle.Information)
                txtAmount_AMT.Focus()
                Exit Sub
            End If
            Dim budgetvalue As Decimal = 0

            dtsubledger.AcceptChanges()

            Dim rwIndex As Integer = -1
            For Each ro As DataRow In dtsubledger.Rows
                rwIndex += 1
                If ro("ACNAME").ToString <> "" Then
                    If txtRowIndex.Text <> "" And rwIndex = Val(txtRowIndex.Text) Then
                        Continue For
                    End If
                    If ro("ACNAME").ToString = CmbAcname.Text Then
                        MsgBox(" This Account Already Loaded in Grid", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
            Next
            If Ischkbudget Then
                budval = 0
                Dim mPartycode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text.Trim & "'")
                budval = Budgetcheck(mPartycode, Entrydate, Entrytype, True, Bcostid)
                If budval <> "" And budval <> "0" Then budgetvalue = Val(budval.Substring(0, budval.Length - 3).ToString) Else budgetvalue = 0
                If budgetvalue <> 0 And budval.Contains("Dr") Then lblbud = "Budget :" + budval.ToString : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) Else lblbud = ""
                If budgetvalue <> 0 And budval.Contains("Cr") Then lblbud = "Budget :" + budval.ToString : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) Else lblbud = ""
            End If

            If budval <> "" And budval <> "0" Then budgetvalue = Val(budval.Substring(0, budval.Length - 3).ToString) Else budgetvalue = 0

            If budgetvalue <> 0 Then
                If (Val(txtAmount_AMT.Text) > chkval) And Val(txtAmount_AMT.Text) <> 0 Then
                    MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
                    If Not OTPCHECK("BUDGETVALUE", cnCostId, userId) Then Exit Sub
                    'Exit Sub
                End If
            End If


            Dim index As Integer = Nothing

            If txtRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtsubledger.NewRow
                ro("ACNAME") = cmbAcname.Text
                ro("AMOUNT") = Val(txtAmount_AMT.Text)
                dtsubledger.Rows.Add(ro)
                dtsubledger.AcceptChanges()
                gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("ACNAME")
                'Exit For
            Else
                'update
                With dtsubledger.Rows(Val(txtRowIndex.Text))
                    .Item("ACNAME") = cmbAcname.Text
                    .Item("AMOUNT") = Val(txtAmount_AMT.Text)
                    index = Val(txtRowIndex.Text)
                    txtRowIndex.Text = ""
                End With
            End If
            CalcSubledgerTotal()
            txtAmount_AMT.Clear()
            CmbAcname.Text = ""
            cmbAcname.Focus()
            If Val(CType(gridView_OWN.DataSource, DataTable).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) = Totalamt Then
                If MsgBox("TOTAL AMOUNT TALLYED Do you want close ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    frmSubLedger_KeyDown(Me, New KeyEventArgs(Keys.Escape))
                End If
            End If

        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If Totalamt = Val(dtsubledger.Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) Then
                frmSubLedger_KeyDown(Me, New KeyEventArgs(Keys.Escape))
            Else
                MsgBox("Amount not tally...")
                cmbAcname.Select()
            End If
        End If
    End Sub

    Private Sub cmbAcname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAcname.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbAcname.Text.Trim = "" Then
                MsgBox("Acount name cannot empty", MsgBoxStyle.Information)
                cmbAcname.Focus()
                Exit Sub
            End If
            txtAmount_AMT.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            frmSubLedger_KeyDown(Me, New KeyEventArgs(Keys.Escape))
        End If
    End Sub
    'Private Sub CmbAcname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbAcname.KeyDown
    '    If e.KeyCode = Keys.Down Then
    '        If gridView_OWN.Rows.Count = 0 Then Exit Sub
    '        gridView_OWN.Focus()
    '    End If
    'End Sub
    Private Sub cmbAcname_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbAcname.Text.Trim = "" Then
                MsgBox("Acount name cannot empty", MsgBoxStyle.Information)
                cmbAcname.Focus()
                Exit Sub
            End If
            txtAmount_AMT.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            frmSubLedger_KeyDown(Me, New KeyEventArgs(Keys.Escape))
        End If
    End Sub

    Private Sub gridView_OWN_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView_OWN.Rows.Count = 0 Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ACNAME")
            gridView_OWN.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
            gridView_OWN.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
            Dim rwIndex As Integer = gridView_OWN.CurrentRow.Index
            With gridView_OWN.Rows(rwIndex)
                txtAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                cmbAcname.Text = .Cells("ACNAME").FormattedValue.ToString
                CalcSubledgerTotal()
                txtRowIndex.Text = rwIndex
                cmbAcname.Focus()
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            frmSubLedger_KeyDown(Me, New KeyEventArgs(Keys.Escape))
        End If
    End Sub

    Private Sub gridView_OWN_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView_OWN.UserDeletedRow
        'dtEditAmt = CType(gridView_OWN.DataSource, DataTable).Copy
    End Sub

    Private Sub gridView_OWN_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridView_OWN.UserDeletingRow
        CalcSubledgerTotal()
        If gridView_OWN.Rows.Count = 0 Then cmbAcname.Focus()
        dtEditAmt.Rows(gridView_OWN.CurrentRow.Index).Delete()
        dtEditAmt.AcceptChanges()
    End Sub
End Class