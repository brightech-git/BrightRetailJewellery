Imports System.Data.OleDb
Public Class frmBillMultiDiscount
    Public dtGridDisc As New DataTable
    Dim ObjdiscCode As New DiscCode
    Dim strSql As String
    Public TotalDisc As Double = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridDisc.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridDiscTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' Add any initialization after the InitializeComponent() call.
        ''MULTI DISCOUNT
        strSql = " SELECT DISCNAME FROM " & cnAdminDb & "..MULTIDISCOUNT ORDER BY DISCNAME"
        objGPack.FillCombo(strSql, cmbDisc)
        With dtGridDisc
            .Columns.Add("DISCNAME", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
        End With
        gridDisc.DataSource = dtGridDisc
        FormatGridColumns(gridDisc)
        StyleGridCheque(gridDisc)
        Dim dtGridDiscTotal As New DataTable
        dtGridDiscTotal = dtGridDisc.Copy
        gridDiscTotal.DataSource = dtGridDiscTotal
        dtGridDiscTotal.Rows.Clear()
        dtGridDiscTotal.Rows.Add()
        dtGridDiscTotal.Rows(0).Item("DISCNAME") = "Total"
        With gridDiscTotal
            .DataSource = dtGridDiscTotal
            For Each col As DataGridViewColumn In gridDisc.Columns
                With gridDiscTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridDiscTotal)
        StyleGridCheque(gridDiscTotal)
    End Sub

    Private Sub StyleGridCheque(ByVal grid As DataGridView)
        gridDiscTotal.DefaultCellStyle.SelectionBackColor = grpMultiDiscount.BackgroundColor
        With grid
            .Columns("DISCNAME").Width = cmbDisc.Width + 1
            .Columns("AMOUNT").Width = txtAmount.Width
        End With
    End Sub

    Public Sub FormatGridColumns(ByVal grid As DataGridView)
        With grid
            .ColumnHeadersVisible = False
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.000"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                    .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
        End With
    End Sub

    Private Sub frmBillMultiDiscount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then


            If TotalDisc > 0 Then
                Dim amt As Double = Nothing
                For Each ro As DataRow In dtGridDisc.Rows
                    amt += Val(ro!AMOUNT.ToString)
                Next
                If amt <> TotalDisc Then
                    MsgBox("Discount Mismatched", MsgBoxStyle.Information)
                    txtAmount.Text = "0"
                    cmbDisc.Focus()
                    Exit Sub
                End If
            End If


            dtGridDisc.AcceptChanges()
            cmbDisc.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmBillMultiDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAmount.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub CalcGridDiscTotal()
        dtGridDisc.AcceptChanges()
        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridDisc.Rows
            amt += Val(ro!AMOUNT.ToString)
        Next
        If TotalDisc > 0 Then
            Me.Text = "Multi Discount " & TotalDisc.ToString & "  Balance " & Math.Round(TotalDisc - amt, 2)
        End If
        gridDiscTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub

    Private Sub txtAmount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount.GotFocus
        
    End Sub

    Private Sub txtAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtAmount.Text) = 0 Then Exit Sub
            If cmbDisc.Items.Contains(cmbDisc.Text) = False Then
                MsgBox("Invalid DiscName", MsgBoxStyle.Information)
                cmbDisc.Focus()
                Exit Sub
            End If
            If TotalDisc > 0 Then
                Dim amt As Double = Nothing
                For Each ro As DataRow In dtGridDisc.Rows
                    amt += Val(ro!AMOUNT.ToString)
                Next
                amt += Val(txtAmount.Text)
                If amt > TotalDisc Then
                    MsgBox("Discount Exceeded", MsgBoxStyle.Information)
                    txtAmount.Text = "0"
                    cmbDisc.Focus()
                    Exit Sub
                End If
            End If
            If txtDiscRowIndex.Text = "" Then
                Dim ro As DataRow = dtGridDisc.NewRow
                ro("DISCNAME") = cmbDisc.Text
                ro("AMOUNT") = Val(txtAmount.Text)
                dtGridDisc.Rows.Add(ro)
                dtGridDisc.AcceptChanges()
            Else
                gridDisc.Rows(Val(txtDiscRowIndex.Text)).Cells("DISCNAME").Value = cmbDisc.Text
                gridDisc.Rows(Val(txtDiscRowIndex.Text)).Cells("AMOUNT").Value = Val(txtAmount.Text)
            End If

            CalcGridDiscTotal()
            ''CLEAR
            objGPack.TextClear(Me)
            cmbDisc.Focus()
        End If
    End Sub

    Private Sub gridDisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridDisc.UserDeletedRow
        dtGridDisc.AcceptChanges()
        CalcGridDiscTotal()
    End Sub

    Private Sub frmBillMultiDiscount_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CalcGridDiscTotal()
        cmbDisc.Focus()
    End Sub

    Private Sub cmbDisc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDisc.Leave
        If cmbDisc.Text <> "" Then
            strSql = "select DISCAUTHCODE,DISCAUTHAMT FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & cmbDisc.Text & "'"
            Dim DRow As DataRow
            DRow = GetSqlRow(strSql, cn)
            If Not DRow Is Nothing Then
                If DRow!DISCAUTHCODE.ToString <> "" Then
                    ObjdiscCode = New DiscCode
                    ObjdiscCode.txtDiscCode.Text = ""
                    If ObjdiscCode.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        If DRow!discauthcode.ToString = ObjdiscCode.txtDiscCode.Text Then txtAmount.Text = Val(DRow!discauthamt.ToString) : Exit Sub Else cmbDisc.Focus()
                    End If

                End If
            End If
        End If
    End Sub

    Private Sub cmbDisc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDisc.SelectedIndexChanged

    End Sub
End Class