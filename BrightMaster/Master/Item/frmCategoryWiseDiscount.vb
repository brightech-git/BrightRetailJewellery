Imports System.Data.OleDb
Public Class frmCategoryWiseDiscount
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Private Sub frmCategoryWiseDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCategoryWiseDiscount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StrSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE 1=1 ORDER BY CATNAME"
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbItem, False, False)
        cmbItem.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        StrSql = vbCrLf + " SELECT * FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT METALNAME AS PARTICULAR,METALNAME,'' AS CATCODE,NULL AS DISPER,'T' AS COLHEAD FROM " & cnAdminDb & "..METALMAST "
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT ' ' + CATNAME AS PARTICULAR,"
        StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = C.METALID)CATNAME,CATCODE,CASE WHEN DISPER <> 0 THEN DISPER ELSE NULL END DISPER,"
        StrSql += vbCrLf + " ' ' AS COLHEAD FROM " & cnAdminDb & "..CATEGORY C "
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " ORDER BY METALNAME,COLHEAD DESC"
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
            .Columns("METALNAME").Visible = False
            .Columns("PARTICULAR").Width = 300
            .Columns("CATCODE").Visible = False
            .Columns("DISPER").HeaderText = "DISCOUNT %"
            .Columns("DISPER").Width = 100
            .Columns("DISPER").ReadOnly = False
            .Columns("DISPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DISPER").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        StrSql = " UPDATE " & cnAdminDb & "..CATEGORY SET DISPER = " & Val(gridView.CurrentRow.Cells("DISPER").Value.ToString) & ""
        StrSql += " WHERE CATCODE = " & Val(gridView.CurrentRow.Cells("CATCODE").Value.ToString) & ""
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, StrSql, cn, tran)
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISPER").Index And Not e.Control Is Nothing Then
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
        gridView.DataSource = Nothing
        cmbItem.Select()
    End Sub
End Class