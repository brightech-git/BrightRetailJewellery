Imports System.Data.OleDb
Public Class TablewiseMarginLock
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Private Sub ItemwiseDiscountLock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ItemwiseDiscountLock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StrSql = " SELECT TABLECODE FROM " & cnAdminDb & "..wmctable WHERE ISNULL(TABLECODE,'')<> '' ORDER BY TABLECODE"
        cmbtable.Items.Clear()
        cmbtable.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbtable, False, False)
        cmbtable.Text = "ALL"

        btnNew_Click(Me, New EventArgs)
    End Sub

    'Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbtable.SelectedIndexChanged
    '    cmbSubItem.Items.Clear()
    '    cmbSubItem.Items.Add("ALL")
    '    cmbSubItem.Text = "ALL"
    '    StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
    '    If cmbtable.Text <> "ALL" And cmbtable.Text <> "" Then
    '        StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbtable.Text & "')"
    '    End If
    '    objGPack.FillCombo(StrSql, cmbSubItem, False, False)
    'End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        StrSql = vbCrLf + "  SELECT * FROM "
        StrSql += vbCrLf + "  ("
        StrSql += vbCrLf + "  SELECT distinct TABLECODE AS PARTICULAR,TABLECODE,CASE WHEN MARGINPER <> 0 THEN MARGINPER ELSE NULL END MARGINPER,CASE WHEN MARGINVALUE <> 0 THEN MARGINVALUE ELSE NULL END MARGINVALUE,'T'COLHEAD,VID,'' COSTID FROM " & cnAdminDb & "..WMCTABLE WHERE ISNULL(TABLECODE,'')<>''"
        If cmbtable.Text <> "ALL" And cmbtable.Text <> "" Then
            StrSql += vbCrLf + " AND  TABLECODE= (SELECT top 1 TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE= '" & cmbtable.Text & "')"
        End If
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT '   ' + TABLECODE,(CONVERT(VARCHAR(10),FROMWEIGHT)+'-'+CONVERT(VARCHAR(10),TOWEIGHT))AS RANGE,CASE WHEN MARGINPER <> 0 THEN MARGINPER ELSE NULL END MARGINPER,CASE WHEN MARGINVALUE <> 0 THEN MARGINVALUE ELSE NULL END MARGINVALUE,'' COLHEAD,VID,COSTID"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..WMCTABLE AS S WHERE ISNULL(TABLECODE,'')<>''"
        If cmbtable.Text <> "ALL" And cmbtable.Text <> "" Then
            StrSql += vbCrLf + " AND  TABLECODE= (SELECT top 1 TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE= '" & cmbtable.Text & "')"
        End If
        StrSql += vbCrLf + "  )X"
        StrSql += vbCrLf + "  ORDER BY VID,COLHEAD DESC"
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
            .Columns("VID").Visible = False
            '.Columns("SUBITEMID").Visible = False
            .Columns("PARTICULAR").Width = 300
            .Columns("MARGINPER").HeaderText = "MARGIN %"
            .Columns("MARGINVALUE").HeaderText = "MARGIN VAL"
            .Columns("MARGINPER").Width = 80
            .Columns("MARGINVALUE").Width = 120
            .Columns("MARGINPER").ReadOnly = False
            .Columns("MARGINVALUE").ReadOnly = False
            .Columns("MARGINPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("MARGINVALUE").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("MARGINPER").DefaultCellStyle.Format = "0.00"
            .Columns("MARGINVALUE").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "MARGINPER" Then
            gridView.CurrentRow.Cells("MARGINVALUE").Value = DBNull.Value
        ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "MARGINVALUE" Then
            gridView.CurrentRow.Cells("MARGINPER").Value = DBNull.Value
        End If


        If gridView.CurrentRow.Cells("COLHEAD").Value.ToString = "T" Then
            ''main item
            StrSql = " UPDATE " & cnAdminDb & "..WMCTABLE SET MARGINPER = " & Val(gridView.CurrentRow.Cells("MARGINPER").Value.ToString) & ""
            StrSql += " ,MARGINVALUE = " & Val(gridView.CurrentRow.Cells("MARGINVALUE").Value.ToString) & ""

            StrSql += " WHERE TABLECODE= '" & Trim(gridView.CurrentRow.Cells("PARTICULAR").Value.ToString) & "'"
        ElseIf gridView.CurrentRow.Cells("COLHEAD").Value.ToString = "" Then

            StrSql = " UPDATE " & cnAdminDb & "..WMCTABLE SET MARGINPER = " & Val(gridView.CurrentRow.Cells("MARGINPER").Value.ToString) & ""
            StrSql += " ,MARGINVALUE = " & Val(gridView.CurrentRow.Cells("MARGINVALUE").Value.ToString) & ""
            StrSql += " WHERE VID= " & Val(gridView.CurrentRow.Cells("VID").Value.ToString) & ""

        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, StrSql, cn, tran)
            If gridView.CurrentRow.Cells("COLHEAD").Value.ToString = "T" Then
                For cnt As Integer = e.RowIndex + 1 To gridView.RowCount - 1
                    If gridView.Rows(cnt).Cells("PARTICULAR").Value.ToString.Trim <> gridView.CurrentRow.Cells("PARTICULAR").Value.ToString.Trim Then Exit For
                    gridView.Rows(cnt).Cells("MARGINPER").Value = gridView.CurrentRow.Cells("MARGINPER").Value
                    gridView.Rows(cnt).Cells("MARGINVALUE").Value = gridView.CurrentRow.Cells("MARGINVALUE").Value
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
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MARGINVALUE").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "AMOUNT"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MARGINPER").Index And Not e.Control Is Nothing Then
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
        cmbtable.Text = "ALL"

        gridView.DataSource = Nothing
        cmbtable.Select()
    End Sub
End Class