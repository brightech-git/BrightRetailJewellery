Imports System.Data.OleDb
Public Class frmbillgift
    Public dtGridgift As New DataTable
    Public Sub New()
        Dim strSql As String = Nothing
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'objGPack.Validator_Object(Me)
        gridToBe1.Columns.Clear()
        With dtGridgift.Columns
            .Add("ITEM", GetType(Integer))
            .Add("DESCRIPTION", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("REMARK", GetType(String))
            .Add("KEYNO", GetType(Integer))
            .Add("ISSSNO", GetType(String))
        End With
        gridToBe1.DataSource = dtGridgift
        StyleGridToBe()
        dtGridgift.AcceptChanges()
    End Sub

    Private Sub StyleGridToBe()
        With gridToBe1
            .Columns("ITEM").Width = txtiname.Width + 1
            .Columns("DESCRIPTION").Width = txtidesc.Width + 1
            .Columns("PCS").Width = txtipce_NUM.Width + 1
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REMARK").Width = txtmemo.Width
            .Columns("KEYNO").Visible = False
            .Columns("ISSSNO").Visible = False
        End With
    End Sub

    Private Sub frmbillgift_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmbillgift_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtmemo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub AddToBe(ByVal Keyno As Integer, ByVal itemname As String, ByVal pcs As Integer, ByVal grsWt As Decimal, ByVal netWt As Decimal, ByVal value As Double, ByVal remark As String)
        Dim ro As DataRow = Nothing
        ro = dtGridgift.NewRow
        ro!KEYNO = Keyno
        ro!ITEM = itemname
        ro!DESCRIPTION = itemname
        ro!PCS = IIf(pcs > 0, pcs, DBNull.Value)
        ro!REMARK = remark
        dtGridgift.Rows.Add(ro)
        dtGridgift.AcceptChanges()
    End Sub

    Public Sub RemoveToBe(ByVal Keyno As Integer)
        For Each ro As DataRow In dtGridgift.Rows
            If ro!KEYNO.ToString = Keyno.ToString Then
                ro.Delete()
                Exit For
            End If
        Next
        dtGridgift.AcceptChanges()
    End Sub

    Private Sub txtRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    End Sub

    Private Sub KeyDownToGrid_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            If gridToBe1.RowCount > 0 Then
                gridToBe1.Select()
            End If
        End If
    End Sub

    Private Sub gridToBe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridToBe1.KeyDown
            If e.KeyCode = Keys.Up Then
            If gridToBe1.RowCount > 0 Then
                If gridToBe1.CurrentRow.Index = 0 Then txtmemo.Focus()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridToBe1.RowCount > 0 Then
                gridToBe1.CurrentCell = gridToBe1.CurrentRow.Cells("ITEM")
            End If
        End If
    End Sub

    Private Sub gridToBe_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridToBe1.UserDeletedRow
        dtGridgift.AcceptChanges()
        If Not gridToBe1.RowCount > 0 Then
            txtiname.Focus()
        End If
    End Sub
    Private Sub LoaditmId(ByVal txtEmpBox As TextBox)
        Dim STRSQL As String
        STRSQL = " SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        STRSQL += " WHERE ACTIVE = 'Y'"
        Dim ITMId As Integer = Val(BrighttechPack.SearchDialog.Show("Select ITEMNAME", STRSQL, cn, 1))
        If ITMId > 0 Then
            txtiname.Text = ITMId
            txtidesc.Text = (objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE itemid = '" & ITMId & "'", , ) & "'")
            txtiname.SelectAll()
        End If
    End Sub

    Private Sub txtiname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtiname.GotFocus
        txtiname.BackColor = Color.PaleGreen
    End Sub

    Private Sub txtiname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtiname.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridToBe1.RowCount > 0 Then
                gridToBe1.Focus()
            End If
        End If
    End Sub

    Private Sub LoadItemName()
        Dim STRSQL As String
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        STRSQL += " WHERE ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", STRSQL, cn, 1, 1, , txtiname.Text)
        If itemName <> "" Then
            txtiname.Text = Val(objGPack.GetSqlValue("SELECT itemid FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME  = '" & itemName & "'", , ) & "'")
        Else
            txtiname.Focus()
            txtiname.SelectAll()
        End If
    End Sub

    Private Sub txtiname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtiname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtiname.Text = "" Then
                LoaditmId(txtiname)
                Exit Sub
            ElseIf Val(txtiname.Text) < 1 Then
                LoadItemName()
                Exit Sub
            ElseIf Not Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & txtiname.Text & "")) > 0 Then
                LoaditmId(txtiname)
                Exit Sub
            ElseIf Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & txtiname.Text & "")) > 0 Then
                txtidesc.Text = (objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE itemid = '" & txtiname.Text & "'", , ) & "'")
            End If
        End If
    End Sub

    Private Sub txtmemo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmemo.GotFocus
        txtmemo.BackColor = Color.PaleGreen
    End Sub


    Private Sub txtmemo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtmemo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If Val(txtipce_NUM.Text) < 1 Then
                txtipce_NUM.Focus()
                Exit Sub
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridgift.NewRow
            ro!ITEM = txtiname.Text
            ro!DESCRIPTION = txtidesc.Text
            ro!PCS = IIf(Val(txtipce_NUM.Text) > 0, Val(txtipce_NUM.Text), DBNull.Value)
            ro!REMARK = txtmemo.Text
            dtGridgift.Rows.Add(ro)
            objGPack.TextClear(Me)
            txtiname.Focus()
        End If
    End Sub

    Private Sub txtiname_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtiname.LostFocus
        txtiname.BackColor = Color.White
    End Sub

    Private Sub txtipce_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtipce_NUM.GotFocus
        txtipce_NUM.BackColor = Color.PaleGreen
    End Sub


    Private Sub txtipce_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtipce_NUM.LostFocus
        txtipce_NUM.BackColor = Color.White
    End Sub

    Private Sub txtmemo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmemo.LostFocus
        txtmemo.BackColor = Color.White
    End Sub

    Private Sub txtiname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtiname.TextChanged

    End Sub

    Private Sub txtipce_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtipce_NUM.TextChanged

    End Sub
End Class