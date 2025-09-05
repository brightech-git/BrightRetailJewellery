Imports System.Data.OleDb
Imports System.IO
Public Class ReSendMaster
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Syncdb As String = cnAdminDb
    
    Private Sub ReSendMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub ReSendMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SenderId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        StrSql = " SELECT 'ALL' COSTID "
        StrSql += " UNION ALL "
        StrSql += " SELECT COSTID "
        StrSql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
        StrSql += " WHERE COSTID <> '" & SenderId & "' AND ISNULL(ACTIVE,'')<>'N'"
        objGPack.FillCombo(StrSql, cmbCostId)
        If cmbCostId.Items.Count > 0 Then
            cmbCostId.SelectedIndex = 0
        End If

        cmbStatus.Items.Add("M")
        cmbStatus.Items.Add("N")
        cmbStatus.Items.Add("Y")
        cmbStatus.SelectedIndex = 0
        cmbCostId.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        StrSql = " SELECT FROMID,TOID,UID,UPDFILE,STATUS FROM " & Syncdb & "..SENDSYNC WHERE 1=1"
        If cmbStatus.Text <> "" Or cmbStatus.Text <> "ALL" Then
            StrSql += " AND STATUS='" & cmbStatus.Text & "'"
        End If

        If cmbCostId.Text <> "ALL" Then
            StrSql += " AND TOID='" & cmbCostId.Text & "' "
        Else
            StrSql += " AND TOID IN (SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If txtSearchText.Text <> "" Then
            StrSql += " AND SQLTEXT LIKE '%" & txtSearchText.Text.Trim & "%'"
        End If
        Dim dtsync As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsync)
        If dtsync.Rows.Count > 0 Then
            gridView_OWN.DataSource = dtsync
        Else
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        With gridView_OWN
            .Columns("FROMID").Width = 70
            .Columns("TOID").Width = 70
            .Columns("UID").Width = 80
            .Columns("UPDFILE").Width = 170
            .Columns("STATUS").Width = 80
            .Columns("FROMID").ReadOnly = True
            .Columns("TOID").ReadOnly = True
            .Columns("UID").ReadOnly = True
            .Columns("UPDFILE").ReadOnly = True
            .Columns("STATUS").ReadOnly = False
        End With
        gridView_OWN.Focus()
        gridView_OWN.Rows(0).Cells("STATUS").Selected = True
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If gridView_OWN.Rows.Count > 0 Then
            For k As Integer = 0 To gridView_OWN.Rows.Count - 1
                If gridView_OWN.Rows(k).Cells("STATUS").Value.ToString <> "" Then
                    StrSql = " UPDATE " & Syncdb & "..SENDSYNC SET STATUS='" & gridView_OWN.Rows(k).Cells("STATUS").Value.ToString & "'"
                    StrSql += " WHERE UID='" & gridView_OWN.Rows(k).Cells("UID").Value.ToString & "' AND UPDFILE='" & gridView_OWN.Rows(k).Cells("UPDFILE").Value.ToString & "'"
                    Cmd = New OleDbCommand(StrSql, cn)
                    'Cmd.CommandTimeout = 500
                    Cmd.ExecuteNonQuery()
                End If
            Next
            gridView_OWN.DataSource = Nothing
            cmbCostId.SelectedIndex = 0
            cmbStatus.SelectedIndex = 0
            txtSearchText.Text = ""
            cmbCostId.Focus()
        End If
    End Sub

    Private Sub BtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellEndEdit
        If gridView_OWN.RowCount > 0 Then
            If Not gridView_OWN.CurrentCell.RowIndex > -1 Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS")
            If gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value.ToString.Trim.ToUpper = "M" Or gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value.ToString.Trim.ToUpper = "Y" Or gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value.ToString.Trim.ToUpper = "N" Then
                btnUpdate.Enabled = True
                gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value = gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value.ToString.ToUpper
                Dim rwIndex As Integer
                If gridView_OWN.CurrentRow.Index = gridView_OWN.Rows.Count - 1 Then
                    rwIndex = gridView_OWN.CurrentRow.Index
                    gridView_OWN.CurrentCell = gridView_OWN.Rows(rwIndex).Cells("STATUS")
                    btnUpdate.Focus()
                Else
                    rwIndex = gridView_OWN.CurrentRow.Index + 1
                    gridView_OWN.CurrentCell = gridView_OWN.Rows(rwIndex).Cells("STATUS")
                End If

                Exit Sub
            Else
                'MsgBox("Enter status is wrong", MsgBoxStyle.Information)
                gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells("STATUS").Value = ""
                Dim rwIndex As Integer
                rwIndex = gridView_OWN.CurrentRow.Index
                gridView_OWN.CurrentCell = gridView_OWN.Rows(rwIndex).Cells("STATUS")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If gridView_OWN.RowCount > 0 Then
            If e.KeyChar = Chr(Keys.Enter) Then
                Dim rwIndex As Integer
                If gridView_OWN.RowCount = 1 Then
                    rwIndex = gridView_OWN.CurrentRow.Index
                Else
                    rwIndex = gridView_OWN.CurrentRow.Index - 1
                End If
                gridView_OWN.CurrentCell = gridView_OWN.Rows(rwIndex).Cells("STATUS")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                btnUpdate.Focus()
            End If
        End If
    End Sub
 
End Class