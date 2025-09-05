Imports System.Data.OleDb
Imports System.IO
Public Class Maintenance
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand

    Private Sub Maintanance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub Maintanance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SenderId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        StrSql = " SELECT 'ALL' COSTID "
        StrSql += " UNION ALL "
        StrSql += " SELECT COSTID "
        StrSql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
        objGPack.FillCombo(StrSql, cmbCostId)
        If cmbCostId.Items.Count > 0 Then
            cmbCostId.SelectedIndex = 0
        End If

        cmbStatus.Items.Add("M")
        cmbStatus.Items.Add("Y")
        cmbStatus.SelectedIndex = 0

        dtpFrom.Value = GetServerDate()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If MessageBox.Show("Do you want Remove the Data " + vbCrLf + "Click Ok to continue", "Remove Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then

            Dim syncdb As String = cnAdminDb
            Dim uprefix As String = Mid(cnAdminDb, 1, 3)
            If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
                Dim usuffix As String = "UTILDB"
                If DbChecker(uprefix + usuffix, True, tran) <> 0 Then syncdb = uprefix + usuffix
            End If

            Dim uid As String = Nothing
            Dim result As Boolean = False
            If ChkSend.Checked Then
                StrSql = " SELECT MAX(UID) FROM " & syncdb & "..SENDSYNC WHERE STATUS='" & cmbStatus.Text & "'"
                StrSql += " AND UPDFILE LIKE '%" & Format(dtpFrom.Value, "dd-MM-yy").Replace("-", "") & "%'"
                If cmbCostId.Text <> "ALL" Then
                    StrSql += " AND TOID='" & cmbCostId.Text & "'"
                End If
                uid = IIf(IsDBNull(GetSqlValue(cn, StrSql)), "", GetSqlValue(cn, StrSql))
                If uid = Nothing Or uid = "" Then
                    'MsgBox("Record not found", MsgBoxStyle.Information)
                    result = False
                    GoTo RECEIVE
                    dtpFrom.Focus()
                    Exit Sub
                End If
                StrSql = " DELETE FROM " & syncdb & "..SENDSYNC WHERE STATUS='" & cmbStatus.Text & "'"
                StrSql += " AND UID <='" & uid & "'"
                If cmbCostId.Text <> "ALL" Then
                    StrSql += " AND TOID='" & cmbCostId.Text & "'"
                End If
                Cmd = New OleDbCommand(StrSql, cn)
                'Cmd.CommandTimeout = 500
                Cmd.ExecuteNonQuery()
                result = True
            End If
RECEIVE:
            If ChkReceive.Checked Then
                StrSql = " SELECT MAX(UID) FROM " & syncdb & "..RECEIVESYNC WHERE STATUS='" & cmbStatus.Text & "'"
                StrSql += " AND UPDFILE LIKE '%" & Format(dtpFrom.Value, "dd-MM-yy").Replace("-", "") & "%'"
                If cmbCostId.Text <> "ALL" Then
                    StrSql += " AND FROMID='" & cmbCostId.Text & "'"
                End If
                uid = IIf(IsDBNull(GetSqlValue(cn, StrSql)), "", GetSqlValue(cn, StrSql))
                If uid = Nothing Or uid = "" Then
                    result = True
                End If
                StrSql = " DELETE FROM " & syncdb & "..RECEIVESYNC WHERE STATUS='" & cmbStatus.Text & "'"
                StrSql += " AND UID <='" & uid & "'"
                If cmbCostId.Text <> "ALL" Then
                    StrSql += " AND FROMID='" & cmbCostId.Text & "'"
                End If
                Cmd = New OleDbCommand(StrSql, cn)
                'Cmd.CommandTimeout = 500
                Cmd.ExecuteNonQuery()
                result = True
            End If

            MsgBox("Data Remove Successfully Completed", MsgBoxStyle.Information)
            cmbCostId.SelectedIndex = 0
            cmbStatus.SelectedIndex = 0
            dtpFrom.Value = GetServerDate()
            btnCancel.Focus()
        End If
    End Sub
End Class