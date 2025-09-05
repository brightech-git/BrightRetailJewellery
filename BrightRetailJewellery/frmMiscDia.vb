Imports System.Data.OleDb
Public Class frmMiscDia
    Public dtGridMisc As New DataTable
    Dim strSql As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ''Miscellaneous Group
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridMisc.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMiscTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMiscTotal.DefaultCellStyle.BackColor = grpMisc.BackgroundColor
        gridMiscTotal.DefaultCellStyle.SelectionBackColor = grpMisc.BackgroundColor


        With dtGridMisc.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("DISCOUNT", GetType(Double))
        End With
        gridMisc.DataSource = dtGridMisc
        FormatGridColumns(gridMisc)
        StyleGridMisc(gridMisc)

        Dim dtGridMiscTotal As New DataTable
        dtGridMiscTotal = dtGridMisc.Copy
        dtGridMiscTotal.Rows.Clear()
        dtGridMiscTotal.Rows.Add()
        dtGridMiscTotal.Rows(0).Item("MISC") = "Total"
        With gridMiscTotal
            .DataSource = dtGridMiscTotal
            For Each col As DataGridViewColumn In gridMisc.Columns
                With gridMiscTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        StyleGridMisc(gridMiscTotal)
    End Sub
    Public Sub StyleGridMisc(ByVal gridMisc As DataGridView)
        With gridMisc
            .Columns("KEYNO").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("MISC").Width = txtMiscMisc.Width + 1
            .Columns("AMOUNT").Width = txtMiscAmount_AMT.Width + 1
            .Columns("DISCOUNT").Visible = False
        End With
    End Sub
    Private Sub txtMiscAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMisc.RowCount > 0 Then gridMisc.Focus()
        End If
    End Sub

    Private Sub txtMiscAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then
                MsgBox(Me.GetNextControl(txtMiscMisc, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscMisc.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                txtMiscMisc.Focus()
                Exit Sub
            End If
            If Not Val(txtMiscAmount_AMT.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtMiscAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscAmount_AMT.Select()
                Exit Sub
            End If
            If txtMiscRowIndex.Text <> "" Then
                dtGridMisc.Rows(Val(txtMiscRowIndex.Text)).Item("MISC") = txtMiscMisc.Text
                dtGridMisc.Rows(Val(txtMiscRowIndex.Text)).Item("AMOUNT") = IIf(Val(txtMiscAmount_AMT.Text) <> 0, Val(txtMiscAmount_AMT.Text), DBNull.Value)
                GoTo AFTERINSERT
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridMisc.NewRow
            ro("MISC") = txtMiscMisc.Text
            ro("AMOUNT") = IIf(Val(txtMiscAmount_AMT.Text) <> 0, Val(txtMiscAmount_AMT.Text), DBNull.Value)
            dtGridMisc.Rows.Add(ro)
AFTERINSERT:
            dtGridMisc.AcceptChanges()
            CalcMiscTotalAmount()
            StyleGridMisc(gridMisc)
            StyleGridMisc(gridMiscTotal)
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.RowCount - 1).Cells("MISC")

            objGPack.TextClear(grpMisc)
            txtMiscMisc.Select()
        End If
    End Sub
    Public Sub CalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            miscTot += Val(gridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        If gridMiscTotal.RowCount > 0 Then
            gridMiscTotal.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        End If
    End Sub

    Private Sub frmMiscDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridMisc.AcceptChanges()
            txtMiscMisc.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtMiscMisc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMiscMisc.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtMiscMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscMisc.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadMiscName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMisc.RowCount > 0 Then gridMisc.Focus()
        End If
    End Sub

    Private Sub txtMiscMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then
                LoadMiscName()
            ElseIf txtMiscMisc.Text <> "" And objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'") = False Then
                LoadMiscName()
            Else
                LoadMiscDetails()
            End If
        End If
    End Sub

    Private Sub txtMiscMisc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMiscMisc.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub LoadMiscName()
        strSql = " SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE = 'Y'"
        Dim Name As String = BrighttechPack.SearchDialog.Show("Find MiscName", strSql, cn, , , , txtMiscMisc.Text)
        If Name <> "" Then
            txtMiscMisc.Text = Name
            LoadMiscDetails()
        Else
            txtMiscMisc.Focus()
            txtMiscMisc.SelectAll()
        End If
    End Sub
    Private Sub LoadMiscDetails()
        If txtMiscMisc.Text <> "" Then
            strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'"
            Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
            txtMiscAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
            Me.SelectNextControl(txtMiscMisc, True, True, True, True)
        End If
    End Sub

    Private Sub gridMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridMisc.RowCount > 0 Then Exit Sub
            With gridMisc.Rows(gridMisc.CurrentRow.Index)
                txtMiscMisc.Text = .Cells("MISC").FormattedValue
                txtMiscAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                txtMiscRowIndex.Text = gridMisc.CurrentRow.Index
                txtMiscAmount_AMT.Focus()
            End With
        End If
    End Sub

    Private Sub gridMisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMisc.UserDeletedRow
        dtGridMisc.AcceptChanges()
        CalcMiscTotalAmount()
        If Not gridMisc.RowCount > 0 Then txtMiscMisc.Focus()

    End Sub

    Private Sub frmMiscDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridMisc.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMiscTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMiscTotal.DefaultCellStyle.BackColor = grpMisc.BackgroundColor
        gridMiscTotal.DefaultCellStyle.SelectionBackColor = grpMisc.BackgroundColor
    End Sub
End Class