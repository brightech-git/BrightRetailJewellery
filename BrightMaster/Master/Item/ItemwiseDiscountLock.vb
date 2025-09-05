Imports System.Data.OleDb
Public Class ItemwiseDiscountLock
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Private Sub ItemwiseDiscountLock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ItemwiseDiscountLock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE = 'Y' ORDER BY ITEMNAME"
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbItem, False, False)
        cmbItem.Text = "ALL"

        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        cmbSubItem.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        cmbSubItem.Text = "ALL"
        StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        objGPack.FillCombo(StrSql, cmbSubItem, False, False)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        StrSql = vbCrLf + "  SELECT * FROM "
        StrSql += vbCrLf + "  ("
        StrSql += vbCrLf + "  SELECT ITEMNAME AS PARTICULAR,ITEMNAME,CASE WHEN DISCVAPER <> 0 THEN DISCVAPER ELSE NULL END DISCVAPER,CASE WHEN DISCPERGM <> 0 THEN DISCPERGM ELSE NULL END DISCPERGM,CASE WHEN DISCFIXPER <> 0 THEN DISCFIXPER ELSE NULL END DISCFIXPER,'T'COLHEAD,SUBITEM,ITEMID,CONVERT(INT,NULL)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            StrSql += vbCrLf + " WHERE  ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT '   ' + SUBITEMNAME,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID)AS ITEMNAME,CASE WHEN DISCVAPER <> 0 THEN DISCVAPER ELSE NULL END DISCVAPER,CASE WHEN DISCPERGM <> 0 THEN DISCPERGM ELSE NULL END DISCPERGM,CASE WHEN DISCFIXPER <> 0 THEN DISCFIXPER ELSE NULL END DISCFIXPER,'' COLHEAD,'N' AS SUBITEM,ITEMID,SUBITEMID"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE 1=1"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            StrSql += vbCrLf + " AND  ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
            StrSql += vbCrLf + " AND  SUBITEMNAME = '" & cmbSubItem.Text & "'"
        End If
        StrSql += vbCrLf + "  )X"
        StrSql += vbCrLf + "  ORDER BY ITEMNAME,COLHEAD DESC"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        DA = New OleDbDataAdapter(StrSql, cn)
        DA.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView, True, )
        FillGridGroupStyle_KeyNoWise(gridView)
        gridView.Select()
        With gridView
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("PARTICULAR").Width = 300
            .Columns("DISCVAPER").HeaderText = "DISC. VA %"
            .Columns("DISCPERGM").HeaderText = "DISC./GM."
            .Columns("DISCFIXPER").HeaderText = "DISC.ON MRP%"
            .Columns("DISCVAPER").Width = 80
            .Columns("DISCPERGM").Width = 120
            .Columns("DISCFIXPER").Width = 120
            .Columns("DISCVAPER").ReadOnly = False
            .Columns("DISCPERGM").ReadOnly = False
            .Columns("DISCFIXPER").ReadOnly = False
            .Columns("DISCVAPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DISCPERGM").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DISCFIXPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DISCVAPER").DefaultCellStyle.Format = "0.00"
            .Columns("DISCPERGM").DefaultCellStyle.Format = "0.00"
            .Columns("DISCFIXPER").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "DISCVAPER" Then
            gridView.CurrentRow.Cells("DISCPERGM").Value = DBNull.Value
        ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "DISCPERGM" Then
            gridView.CurrentRow.Cells("DISCVAPER").Value = DBNull.Value
        ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "DISCFIXPER" Then
            gridView.CurrentRow.Cells("DISCFIXPER").Value = DBNull.Value
        End If


        If gridView.CurrentRow.Cells("COLHEAD").Value.ToString = "T" And gridView.CurrentRow.Cells("SUBITEM").Value.ToString <> "Y" Then
            ''main item
            StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET DISCVAPER = " & Val(gridView.CurrentRow.Cells("DISCVAPER").Value.ToString) & ""
            StrSql += " ,DISCPERGM = " & Val(gridView.CurrentRow.Cells("DISCPERGM").Value.ToString) & ""
            StrSql += " ,DISCFIXPER= " & Val(gridView.CurrentRow.Cells("DISCFIXPER").Value.ToString) & ""
            StrSql += " WHERE ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
        ElseIf gridView.CurrentRow.Cells("COLHEAD").Value.ToString = "T" And gridView.CurrentRow.Cells("SUBITEM").Value.ToString = "Y" Then
            ''IT has subitem
            StrSql = " UPDATE " & cnAdminDb & "..SUBITEMMAST SET DISCVAPER = " & Val(gridView.CurrentRow.Cells("DISCVAPER").Value.ToString) & ""
            StrSql += " ,DISCPERGM = " & Val(gridView.CurrentRow.Cells("DISCPERGM").Value.ToString) & ""
            StrSql += " ,DISCFIXPER= " & Val(gridView.CurrentRow.Cells("DISCFIXPER").Value.ToString) & ""
            StrSql += " WHERE ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
            StrSql += " UPDATE " & cnAdminDb & "..ITEMMAST SET DISCVAPER = " & Val(gridView.CurrentRow.Cells("DISCVAPER").Value.ToString) & ""
            StrSql += " ,DISCPERGM = " & Val(gridView.CurrentRow.Cells("DISCPERGM").Value.ToString) & ""
            StrSql += " ,DISCFIXPER= " & Val(gridView.CurrentRow.Cells("DISCFIXPER").Value.ToString) & ""
            StrSql += " WHERE ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
        Else
            StrSql = " UPDATE " & cnAdminDb & "..SUBITEMMAST SET DISCVAPER = " & Val(gridView.CurrentRow.Cells("DISCVAPER").Value.ToString) & ""
            StrSql += " ,DISCPERGM = " & Val(gridView.CurrentRow.Cells("DISCPERGM").Value.ToString) & ""
            StrSql += " ,DISCFIXPER= " & Val(gridView.CurrentRow.Cells("DISCFIXPER").Value.ToString) & ""
            StrSql += " WHERE ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
            StrSql += " AND SUBITEMID = " & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ""
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, StrSql, cn, tran)
            If gridView.CurrentRow.Cells("SUBITEM").Value.ToString = "Y" Then
                For cnt As Integer = e.RowIndex + 1 To gridView.RowCount - 1
                    If gridView.Rows(cnt).Cells("ITEMNAME").Value.ToString <> gridView.CurrentRow.Cells("ITEMNAME").Value.ToString Then Exit For
                    gridView.Rows(cnt).Cells("DISCVAPER").Value = gridView.CurrentRow.Cells("DISCVAPER").Value
                    gridView.Rows(cnt).Cells("DISCPERGM").Value = gridView.CurrentRow.Cells("DISCPERGM").Value
                    gridView.Rows(cnt).Cells("DISCFIXPER").Value = gridView.CurrentRow.Cells("DISCFIXPER").Value
                Next
                'gridView.CurrentCell.Value = DBNull.Value
            End If
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISCPERGM").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "AMOUNT"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISCVAPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISCFIXPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent

        End If
    End Sub
    Private Sub TextKeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If CType(sender, TextBox).Tag = "AMOUNT" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Amount)
        Else
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Percentage)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbItem.Text = "ALL"
        cmbSubItem.Text = "ALL"
        gridView.DataSource = Nothing
        cmbItem.Select()
    End Sub
End Class