Imports System.Data.OleDb
Public Class frmAccgroupschedule
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Finalamtlockbase As String

    Private Sub ItemwiseDiscountLock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ItemwiseDiscountLock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbCostCentre_MAN.Enabled = True
        StrSql = " SELECT 'ALL' AS ACGRPNAME UNION ALL "
        StrSql += " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        objGPack.FillCombo(StrSql, cmbCostCentre_MAN, , False)
        cmbCostCentre_MAN.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
       
        Dim updatetype As String
        StrSql = vbCrLf + "  SELECT AG.ACGRPNAME,AC.ACCODE,AC.ACNAME  ,AC.ACGRPCODE,AC.DISPORDER FROM " & cnAdminDb & ".. ACHEAD  AC"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & ".. ACGROUP AG ON  AG.ACGRPCODE=AC.ACGRPCODE"
        If cmbCostCentre_MAN.Text <> "ALL" Or cmbCostCentre_MAN.Text <> "" Then
            StrSql += vbCrLf + " WHERE ACGRPNAME='" & cmbCostCentre_MAN.Text & "'"
        End If
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
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView, True, False, True)
        FillGridGroupStyle_KeyNoWise(gridView)
        gridView.Select()
        With gridView
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .Columns("KEYNO").Visible = False
            .Columns("ACCODE").Visible = True
            .Columns("ACNAME").Width = 300
            .Columns("ACGRPNAME").Width = 300
            .Columns("ACGRPNAME").Visible = True
            .Columns("ACGRPCODE").Visible = False
            .Columns("DISPORDER").Visible = True
        End With

        With gridView
            .Columns("ACCODE").HeaderText = "ACCODE"
            .Columns("ACNAME").HeaderText = "A/C NAME"
            .Columns("ACNAME").HeaderText = "GROUP NAME"
            .Columns("DISPORDER").HeaderText = "ORDER"
        End With
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        Try
            Dim mCostid As String = ""
            If cmbCostCentre_MAN.Enabled Then
                StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & cmbCostCentre_MAN.Text & "'"
                mCostid = objGPack.GetSqlValue(StrSql)
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            StrSql = "update " & cnAdminDb & "..ACHEAD set DISPORDER='" & Val(gridView.CurrentRow.Cells("disporder").Value.ToString) & "' where ACCODE='" & gridView.CurrentRow.Cells("ACCODE").Value.ToString & "'"
            ExecQuery(SyncMode.Master, StrSql, cn, tran)
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISPORDER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "DISPORDER"
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
        gridView.DataSource = Nothing
        cmbCostCentre_MAN.Focus()
    End Sub
End Class