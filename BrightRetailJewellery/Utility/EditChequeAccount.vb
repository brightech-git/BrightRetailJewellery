Imports System.Data.OleDb
Public Class EditChequeAccount
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter

    Private Sub EditChequeAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If cmbDefaultBank.Focused Then
                gridView_OWN.Focus()
            End If
        End If
    End Sub

    Private Sub EditChequeAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            If cmbDefaultBank.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub EditChequeAccount_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''CHEQUE
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE (ACGRPCODE = 2 OR ACTYPE='B') "
        strSql += GetAcNameQryFilteration()
        'strSql += " AND LEFT(ACCODE,2) <> 'CC' "
        strSql += " ORDER BY ACNAME "
        objGPack.FillCombo(strSql, cmbDefaultBank)
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        gridView_OWN.DataSource = Nothing
        cmbDefaultBank.Visible = False
        lblStatus.Visible = False
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView_OWN.DataSource = Nothing
        strSql = " SELECT TRANNO,TRANDATE"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS DEFAULTBANK"
        strSql += vbCrLf + " ,AMOUNT,CHQCARDNO CHQNO,CHQCARDREF BANKNAME,CHQDATE,SNO,COSTID FROM " & cnStockDb & "..ACCTRAN AS T "
        strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtCard.Checked = True Then
            strSql += " AND PAYMODE = 'CC'"
        Else
            strSql += " AND PAYMODE = 'CH'"
        End If

        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If
        gridView_OWN.DataSource = dtGrid
        FormatGridColumns(gridView_OWN, False)
        With gridView_OWN
            .Columns("TRANNO").Width = 70
            .Columns("TRANDATE").Width = 80
            .Columns("DEFAULTBANK").Width = 300
            .Columns("AMOUNT").Width = 100
            .Columns("CHQNO").Width = 110
            .Columns("BANKNAME").Width = 150
            .Columns("CHQDATE").Width = 80
            If rbtCard.Checked = True Then
                .Columns("CHQNO").HeaderText = "CARDNO"
                .Columns("CHQDATE").HeaderText = "CARDDATE"
            End If
            .Columns("SNO").Visible = False
        End With
        gridView_OWN.Focus()
        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("DEFAULTBANK")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridView_OWN.Rows.Count > 0 Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("DEFAULTBANK")
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridView_OWN.RowCount > 0 Then Exit Sub
            If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("DEFAULTBANK")
            Dim pt As Point = gridView_OWN.Location
            cmbDefaultBank.Visible = True
            pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("DEFAULTBANK").Index, gridView_OWN.CurrentRow.Index, False).Location
            cmbDefaultBank.Size = gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("DEFAULTBANK").Index, gridView_OWN.CurrentRow.Index, False).Size
            cmbDefaultBank.Text = gridView_OWN.CurrentRow.Cells("DEFAULTBANK").Value.ToString
            cmbDefaultBank.Location = pt
            cmbDefaultBank.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.SelectionChanged
        'gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("DEFAULTBANK")
    End Sub

    Private Sub cmbDefaultBank_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDefaultBank.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Try
                strSql = "SELECT 'N' AS RELIASEDATE FROM " & cnStockDb & "..ACCTRAN "
                strSql += " WHERE SNO = '" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                strSql += " AND RELIASEDATE IS NULL "
                If objGPack.GetSqlValue(strSql, "RELIASEDATE", "") <> "N" Then MsgBox("Already Reliased", MsgBoxStyle.Information) : Exit Sub
                tran = Nothing
                tran = cn.BeginTransaction
                Dim Accode As String = ""
                strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbDefaultBank.Text & "'"
                Accode = objGPack.GetSqlValue(strSql, "ACCODE", "", tran)
                strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET ACCODE = '" & Accode & "'"
                strSql += " WHERE SNO = '" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString)
                Dim ChitDb As String = ""
                strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='CHITDBPREFIX'"
                ChitDb = objGPack.GetSqlValue(strSql, "CTLTEXT", "", tran)
                If ChitDb.Length >= 3 Then
                    Dim Entrefno As String = ""
                    strSql = "SELECT ENTREFNO FROM " & cnStockDb & "..ACCTRAN WHERE SNO='" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                    Entrefno = objGPack.GetSqlValue(strSql, "ENTREFNO", "", tran)
                    If Entrefno <> "" Then
                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = '" & Accode & "'"
                        strSql += " WHERE ENTREFNO='" & Entrefno & "' AND SNO <> '" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString)

                        strSql = " UPDATE " & Mid(ChitDb, 1, 3) & "SH0708..SCHEMECOLLECT SET ACCODE = '" & Accode & "'"
                        strSql += " WHERE ENTREFNO='" & Entrefno & "'"
                        If rbtCard.Checked = True Then
                            strSql += " AND MODEPAY='R'"
                        Else
                            strSql += " AND MODEPAY='D'"
                        End If
                        ExecQuery(SyncMode.Master, strSql, cn, tran)
                    End If
                End If
                cmbDefaultBank.Visible = False
                tran.Commit()
                gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("DEFAULTBANK").Value = cmbDefaultBank.Text
                tran = Nothing
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
            gridView_OWN.Focus()
        End If
    End Sub

    Private Sub cmbDefaultBank_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDefaultBank.LostFocus
        cmbDefaultBank.Visible = False
    End Sub
    Private Sub FindSearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindSearchToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN, gridView_OWN.Columns("CHQNO").Index)
        objSearch.ShowDialog()
    End Sub

    Private Sub rbtCard_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtCard.CheckedChanged
        If rbtCard.Checked Then
            ''CARD
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN(SELECT ACCODE FROM "
            strSql += "" & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R')"
            strSql += GetAcNameQryFilteration()
            'strSql += " AND LEFT(ACCODE,2) <> 'CC' "
            strSql += " ORDER BY ACNAME "
            objGPack.FillCombo(strSql, cmbDefaultBank, True)
        Else
            ''CHEQUE
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = 2"
            strSql += GetAcNameQryFilteration()
            'strSql += " AND LEFT(ACCODE,2) <> 'CC' "
            strSql += " ORDER BY ACNAME "
            objGPack.FillCombo(strSql, cmbDefaultBank, True)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "EDIT CHEQUE ACCOUNT", gridView_OWN, BrightPosting.GExport.GExportType.Export)
    End Sub
End Class