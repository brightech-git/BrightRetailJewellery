Imports System.Data.OleDb
Public Class frmAlloyDetails
    Dim strSql As String

    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim dr As OleDbDataReader
    ''alloy
    Public dtGridAlloy As New DataTable
    Public dtGridAlloyTotal As New DataTable
    Public AlloyEditFlag As New Boolean
    Public AlloyDetails As Boolean = False

    Public AlloySno As String = ""
    Public AlloyTotWt As Double

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' ''ALLOY
        'strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST"
        'strSql += " WHERE ISNULL(METALALLOY,'') = 'A' ORDER BY METALNAME"
        'objGPack.FillCombo(strSql, cmbAlloy, "METALNAME", True)

        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridAlloyView.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridAlloyTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridAlloyTotal.DefaultCellStyle.BackColor = grpAlloy.BackgroundColor
        gridAlloyTotal.DefaultCellStyle.SelectionBackColor = grpAlloy.BackgroundColor

        'funcGridStyle(gridAlloyView)
        'gridAlloyView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        'gridAlloyView.ColumnHeadersVisible = False
        'gridAlloyView.BorderStyle = BorderStyle.None
        'Dim AlloyCol As New DataColumn
        'AlloyCol = New DataColumn
        'AlloyCol.ColumnName = "SNO"
        'AlloyCol.AutoIncrement = True
        'AlloyCol.AutoIncrementSeed = 1
        'AlloyCol.AutoIncrementStep = 1
        'dtGridAlloy.Columns.Add(AlloyCol)
        'strSql = " SELECT '' AS ALLOY,'' AS WEIGHT"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGridAlloy)
        'dtGridAlloy.Rows.Clear()
        'dtGridAlloy.AcceptChanges()
        'gridAlloyView.DataSource = dtGridAlloy
        'With gridAlloyView
        '    .Columns("SNO").Visible = False
        '    .Columns("ALLOY").Width = txtAlloy_MAN.Width + 1
        '    .Columns("WEIGHT").Width = txtAlloyWt_WET.Width
        '    .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'End With


        With dtGridAlloy.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("ALLOY", GetType(String))
            .Add("WEIGHT", GetType(Decimal))
        End With
        gridAlloyView.DataSource = dtGridAlloy
        FormatGridColumns(gridAlloyView)
        StyleGridAlloy(gridAlloyView)

        Dim dtGridAlloyTotal As New DataTable
        dtGridAlloyTotal = dtGridAlloy.Copy
        dtGridAlloyTotal.Rows.Clear()
        dtGridAlloyTotal.Rows.Add()
        dtGridAlloyTotal.Rows(0).Item("ALLOY") = "Total"
        With gridAlloyTotal
            .DataSource = dtGridAlloyTotal
            For Each col As DataGridViewColumn In gridAlloyView.Columns
                With gridAlloyTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        StyleGridAlloy(gridAlloyTotal)
  
    End Sub

    Public Sub StyleGridAlloy(ByVal gridAlloy As DataGridView)
        With gridAlloy
            .Columns("KEYNO").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("ALLOY").Width = txtAlloy_MAN.Width + 1
            .Columns("WEIGHT").Width = txtAlloyWt_WET.Width + 1
        End With
    End Sub

    Private Sub frmAlloyDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Math.Round(Val(gridAlloyTotal.Rows(0).Cells("WEIGHT").Value.ToString), 3) <> Val(AlloyTotWt) Then
                MsgBox("Alloy Weight ShouldBe Equal to total Alloy weight", MsgBoxStyle.Information)
                txtAlloyWt_WET.Select()
                txtAlloyWt_WET.SelectAll()
                Exit Sub
            End If
            dtGridAlloy.AcceptChanges()
            txtAlloy_MAN.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmAlloyDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridAlloyView.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridAlloyTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridAlloyTotal.DefaultCellStyle.BackColor = grpAlloy.BackgroundColor
        gridAlloyTotal.DefaultCellStyle.SelectionBackColor = grpAlloy.BackgroundColor
        StyleGridAlloy(gridAlloyView)
        StyleGridAlloy(gridAlloyTotal)
    End Sub

    Private Sub gridAlloyView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridAlloyView.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    e.Handled = True
        '    If gridAlloyView.RowCount > 0 Then
        '        gridAlloyView.CurrentCell = gridAlloyView.Rows(gridAlloyView.CurrentRow.Index).Cells("ALLOY")
        '        With gridAlloyView.Rows(gridAlloyView.CurrentRow.Index)
        '            AlloySno = .Cells("SNO").Value.ToString
        '            txtAlloy_MAN.Text = .Cells("ALLOY").Value.ToString
        '            txtAlloyWt_WET.Text = .Cells("WEIGHT").Value.ToString
        '            AlloyEditFlag = True
        '            txtAlloy_MAN.Select()
        '            txtAlloy_MAN.SelectAll()
        '        End With
        '    End If
        'End If

        If e.KeyCode = Keys.Enter Then
            If Not gridAlloyView.RowCount > 0 Then Exit Sub
            With gridAlloyView.Rows(gridAlloyView.CurrentRow.Index)
                txtAlloy_MAN.Text = .Cells("ALLOY").FormattedValue
                txtAlloyWt_WET.Text = .Cells("WEIGHT").FormattedValue
                txtAlloyRowIndex.Text = gridAlloyView.CurrentRow.Index
                txtAlloyWt_WET.Focus()
            End With
        End If
    End Sub

    Private Sub gridAlloyView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridAlloyView.UserDeletedRow
        dtGridAlloy.AcceptChanges()
        CalcAlloyTotalWt()
        If Not gridAlloyView.RowCount > 0 Then
            txtAlloy_MAN.Select()
        End If
    End Sub

    Private Sub txtAlloyWt_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlloyWt_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridAlloyView.RowCount > 0 Then gridAlloyView.Focus()
        End If
    End Sub

    Private Sub txtAlloyWt_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlloyWt_WET.LostFocus
        Try
            If txtAlloyWt_WET.Text <> "" Then
                txtAlloy_MAN.Select()
                txtAlloy_MAN.Focus()
            End If
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub
    Public Function CalcAlloyTotalWt()
        Dim wt As Double = Nothing
        For cnt As Integer = 0 To dtGridAlloy.Rows.Count - 1
            wt += Val(dtGridAlloy.Rows(cnt).Item("WEIGHT").ToString)
        Next

        If gridAlloyTotal.RowCount > 0 Then
            gridAlloyTotal.Rows(0).Cells("WEIGHT").Value = IIf(wt <> 0, Format(wt, "0.000"), DBNull.Value)
        End If
        'Return IIf(wt <> 0, Format(wt, "0.000"), "")
    End Function

    'Private Sub cmbAlloy_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        'If ("Alloy Name", cmbAlloy) Then
    '        '    Exit Sub
    '        'Else
    '        '    'strSql = "SELECT DEFAULTVALUE FROM " & IIf(BackEnd = "S", "" & cnAdminDb & "..", "") & "MISCCHARGES WHERE MISCNAME ='" & cmbMisc.Text & "'"
    '        '    'Dim MiscDefaultAmt As Double = Val(funcGetSqlValue(strSql, "DEFAULTVALUE", ""))
    '        '    'txtMiscAmount.Text = IIf(Val(MiscDefaultAmt) <> 0, Format(MiscDefaultAmt, "0.00"), "")
    '        'End If
    '    End If
    'End Sub

    Private Sub txtAlloyWt_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAlloyWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAlloy_MAN.Text = "" Then
                MsgBox(Me.GetNextControl(txtAlloy_MAN, False).Text + E0001, MsgBoxStyle.Information)
                txtAlloy_MAN.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & txtAlloy_MAN.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid ALLOY", MsgBoxStyle.Information)
                txtAlloy_MAN.Focus()
                Exit Sub
            End If
            If Not Val(txtAlloyWt_WET.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtAlloyWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtAlloyWt_WET.Select()
                Exit Sub
            End If

            If txtAlloyRowIndex.Text = "" Then
                If Math.Round(Val(gridAlloyTotal.Rows(0).Cells("WEIGHT").Value.ToString) + Val(txtAlloyWt_WET.Text), 3) > Val(AlloyTotWt) Then
                    MsgBox("Alloy Weight ShouldBe Equal to total Alloy weight", MsgBoxStyle.Information)
                    txtAlloyWt_WET.Select()
                    txtAlloyWt_WET.SelectAll()
                    Exit Sub
                End If
            Else
                If Math.Round(Val(gridAlloyTotal.Rows(0).Cells("WEIGHT").Value.ToString) - Val(gridAlloyView.Rows(Val(txtAlloyRowIndex.Text)).Cells("WEIGHT").Value.ToString) + Val(txtAlloyWt_WET.Text), 3) > Val(AlloyTotWt) Then
                    MsgBox("Alloy Weight ShouldBe Equal to total Alloy weight", MsgBoxStyle.Information)
                    txtAlloyWt_WET.Select()
                    txtAlloyWt_WET.SelectAll()
                    Exit Sub
                End If
            End If

            If txtAlloyRowIndex.Text <> "" Then
                dtGridAlloy.Rows(Val(txtAlloyRowIndex.Text)).Item("ALLOY") = txtAlloy_MAN.Text
                dtGridAlloy.Rows(Val(txtAlloyRowIndex.Text)).Item("WEIGHT") = IIf(Val(txtAlloyWt_WET.Text) <> 0, Val(txtAlloyWt_WET.Text), DBNull.Value)
                GoTo AFTERINSERT
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridAlloy.NewRow
            ro("ALLOY") = txtAlloy_MAN.Text
            ro("WEIGHT") = IIf(Val(txtAlloyWt_WET.Text) <> 0, Val(txtAlloyWt_WET.Text), DBNull.Value)
            dtGridAlloy.Rows.Add(ro)
AFTERINSERT:
            dtGridAlloy.AcceptChanges()
            CalcAlloyTotalWt()
            StyleGridAlloy(gridAlloyView)
            StyleGridAlloy(gridAlloyTotal)
            gridAlloyView.CurrentCell = gridAlloyView.Rows(gridAlloyView.RowCount - 1).Cells("ALLOY")

            objGPack.TextClear(grpAlloy)
            txtAlloy_MAN.Select()
        End If
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If Not Val(txtAlloyWt_WET.Text) > 0 Then
        '        MsgBox("Invalid Weight", MsgBoxStyle.Information)
        '        txtAlloyWt_WET.Select()
        '        txtAlloyWt_WET.SelectAll()
        '        Exit Sub
        '    End If

        '    If Math.Round(Val(txtAlloyTotalWt.Text) + Val(txtAlloyWt_WET.Text), 3) > Val(AlloyTotWt) Then
        '        MsgBox("Alloy Weight ShouldBe Equal to total Alloy weight", MsgBoxStyle.Information)
        '        txtAlloyWt_WET.Select()
        '        txtAlloyWt_WET.SelectAll()
        '        Exit Sub
        '    End If

        '    If AlloyEditFlag = False Then
        '        Dim rw As DataRow = Nothing
        '        rw = dtGridAlloy.NewRow
        '        rw!Alloy = txtAlloy_MAN.Text
        '        rw!Weight = Format(Val(txtAlloyWt_WET.Text), "0.000")
        '        dtGridAlloy.Rows.Add(rw)
        '        dtGridAlloy.AcceptChanges()
        '    Else
        '        For cnt As Integer = 0 To dtGridAlloy.Rows.Count - 1
        '            With dtGridAlloy.Rows(cnt)
        '                If .Item("SNO").ToString = AlloySno Then
        '                    .Item("ALLOY") = txtAlloy_MAN.Text
        '                    .Item("WEIGHT") = Format(Val(txtAlloyWt_WET.Text), "0.000")
        '                    dtGridAlloy.AcceptChanges()
        '                    Exit For
        '                End If
        '            End With
        '        Next
        '    End If
        '    txtAlloy_MAN.Text = ""
        '    txtAlloyWt_WET.Clear()
        '    AlloySno = ""
        '    txtAlloyTotalWt.Text = funcCalcAlloyTotalWt()
        '    AlloyEditFlag = False
        '    txtAlloy_MAN.Select()
        '    txtAlloy_MAN.Focus()
        'End If
    End Sub

    Private Sub txtAlloy_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlloy_MAN.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtAlloy_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlloy_MAN.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadAlloyName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridAlloyView.RowCount > 0 Then
                gridAlloyView.Focus()
            End If
        End If
    End Sub

    Private Sub txtAlloy_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAlloy_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAlloy_MAN.Text = "" Then
                LoadAlloyName()
            ElseIf txtAlloy_MAN.Text <> "" And objGPack.DupCheck("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & txtAlloy_MAN.Text & "'") = False Then
                LoadAlloyName()
            Else
                Me.SelectNextControl(txtAlloy_MAN, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub txtAlloy_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlloy_MAN.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub LoadAlloyName()
        strSql = " SELECT"
        strSql += " METALID ALLOYID, METALNAME ALLOYNAME"
        strSql += " FROM " & cnAdminDb & "..METALMAST"
        strSql += " WHERE ISNULL(TTYPE,'') = 'A' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find AlloyName", strSql, cn, 1, 1)
        If itemName <> "" Then
            txtAlloy_MAN.Text = itemName
        Else
            txtAlloy_MAN.Focus()
            txtAlloy_MAN.SelectAll()
        End If
    End Sub
End Class