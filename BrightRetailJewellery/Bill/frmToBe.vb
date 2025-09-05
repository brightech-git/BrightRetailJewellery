Public Class frmToBe
    Public dtGridToBe As New DataTable
    Public TobeRate As Decimal
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        gridToBe.Columns.Clear()
        With dtGridToBe.Columns
            .Add("ITEM", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("VALUE", GetType(Double))
            .Add("REMARK", GetType(String))
            .Add("KEYNO", GetType(Integer))
            .Add("ISSSNO", GetType(String))
        End With
        gridToBe.DataSource = dtGridToBe
        StyleGridToBe()
        dtGridToBe.AcceptChanges()

        AddHandler txtDescription.KeyDown, AddressOf KeyDownToGrid_KeyDown
        AddHandler txtPcs_NUM.KeyDown, AddressOf KeyDownToGrid_KeyDown
        AddHandler txtGrsWt_WET.KeyDown, AddressOf KeyDownToGrid_KeyDown
        AddHandler txtNetWt_WET.KeyDown, AddressOf KeyDownToGrid_KeyDown
        AddHandler txtValue_AMT.KeyDown, AddressOf KeyDownToGrid_KeyDown
        AddHandler txtRemark.KeyDown, AddressOf KeyDownToGrid_KeyDown
    End Sub

    Private Sub StyleGridToBe()
        With gridToBe
            .Columns("ITEM").Width = txtDescription.Width + 1
            .Columns("PCS").Width = txtPcs_NUM.Width + 1
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = txtGrsWt_WET.Width + 1
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").Width = txtNetWt_WET.Width + 1
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("VALUE").Width = txtValue_AMT.Width + 1
            .Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VALUE").DefaultCellStyle.Format = "0.00"
            .Columns("REMARK").Width = txtRemark.Width
            .Columns("KEYNO").Visible = False
            .Columns("ISSSNO").Visible = False

        End With
    End Sub

    Private Sub frmToBe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmToBe_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
                If txtNetWt_WET.Focused And TobeRate <> 0 Then txtValue_AMT.Text = Val(txtNetWt_WET.Text) * TobeRate : txtValue_AMT.ReadOnly = True
                If txtRemark.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub AddToBe(ByVal Keyno As Integer, ByVal itemname As String, ByVal pcs As Integer, ByVal grsWt As Decimal, ByVal netWt As Decimal, ByVal value As Double, ByVal remark As String)
        Dim ro As DataRow = Nothing
        ro = dtGridToBe.NewRow
        ro!KEYNO = Keyno
        ro!ITEM = itemname
        ro!PCS = IIf(pcs > 0, pcs, DBNull.Value)
        ro!GRSWT = IIf(grsWt > 0, grsWt, DBNull.Value)
        ro!NETWT = IIf(netWt > 0, netWt, DBNull.Value)
        ro!VALUE = IIf(value > 0, value, DBNull.Value)
        ro!REMARK = remark
        dtGridToBe.Rows.Add(ro)
        dtGridToBe.AcceptChanges()
    End Sub

    Public Sub RemoveToBe(ByVal Keyno As Integer)
        For Each ro As DataRow In dtGridToBe.Rows
            If ro!KEYNO.ToString = Keyno.ToString Then
                ro.Delete()
                Exit For
            End If
        Next
        dtGridToBe.AcceptChanges()
    End Sub

    Private Sub txtRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If Not Val(txtValue_AMT.Text) > 0 Then
                MsgBox("Value Should not Empty", MsgBoxStyle.Information)
                txtValue_AMT.Focus()
                Exit Sub
            End If
            If txtRemark.Text = "" Then
                MsgBox("Remark Should not Empty", MsgBoxStyle.Information)
                txtRemark.Focus()
                Exit Sub
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridToBe.NewRow
            ro!ITEM = txtDescription.Text
            ro!PCS = IIf(Val(txtPcs_NUM.Text) > 0, Val(txtPcs_NUM.Text), DBNull.Value)
            ro!GRSWT = IIf(Val(txtGrsWt_WET.Text) > 0, Val(txtGrsWt_WET.Text), DBNull.Value)
            ro!NETWT = IIf(Val(txtNetWt_WET.Text) > 0, Val(txtNetWt_WET.Text), DBNull.Value)
            ro!VALUE = IIf(Val(txtValue_AMT.Text) > 0, Val(txtValue_AMT.Text), DBNull.Value)
            ro!REMARK = txtRemark.Text
            dtGridToBe.Rows.Add(ro)
            objGPack.TextClear(Me)
            txtDescription.Focus()
        End If
    End Sub

    Private Sub KeyDownToGrid_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            If gridToBe.RowCount > 0 Then
                gridToBe.Select()
            End If
        End If
    End Sub

    Private Sub gridToBe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridToBe.KeyDown
        If e.KeyCode = Keys.Up Then
            If gridToBe.RowCount > 0 Then
                If gridToBe.CurrentRow.Index = 0 Then txtDescription.Focus()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridToBe.RowCount > 0 Then
                gridToBe.CurrentCell = gridToBe.CurrentRow.Cells("ITEM")
            End If
        End If
    End Sub

    Private Sub gridToBe_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridToBe.UserDeletedRow
        dtGridToBe.AcceptChanges()
        If Not gridToBe.RowCount > 0 Then
            txtDescription.Focus()
        End If
    End Sub

    Private Sub txtDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDescription.TextChanged

    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click

    End Sub

    Private Sub gridToBe_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridToBe.CellContentClick

    End Sub
End Class