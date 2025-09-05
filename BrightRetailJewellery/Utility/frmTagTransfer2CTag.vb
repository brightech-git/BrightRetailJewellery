Imports System.data.oledb
Public Class frmTagTransfer2CTag
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Try
            Dim FILTERSTR As String = ""

            If MessageBox.Show("Sure, Do you want to remove this info from taginfo?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                FILTERSTR = "ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "'"
                If txtTagNo.Text <> "" Then FILTERSTR += " AND TAGNO = '" & txtTagNo.Text & "'"

                tran = cn.BeginTransaction

                strSql = "INSERT INTO " & cnAdminDb & "..CITEMTAG"
                strSql += "("
                strSql += GetColumnNames(cnAdminDb, "CITEMTAG", tran)
                strSql += ")"
                strSql += " SELECT "
                strSql += GetColumnNames(cnAdminDb, "CITEMTAG", tran)
                strSql += " FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE " & FILTERSTR
                'ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery = 0 Then
                    tran.Rollback()
                    MsgBox("There is no records", MsgBoxStyle.Information)
                    Exit Sub
                End If


                strSql = "INSERT INTO " & cnAdminDb & "..CITEMTAGSTONE"
                strSql += "("
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGSTONE", tran)
                strSql += ")"
                strSql += " SELECT "
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGSTONE", tran)
                strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                ' ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO " & cnAdminDb & "..CITEMTAGMETAL"
                strSql += "("
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGMETAL", tran)
                strSql += ")"
                strSql += " SELECT "
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGMETAL", tran)
                strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO " & cnAdminDb & "..CITEMTAGMISCCHAR"
                strSql += "("
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGMISCCHAR", tran)
                strSql += ")"
                strSql += " SELECT "
                strSql += GetColumnNames(cnAdminDb, "CITEMTAGMISCCHAR", tran)
                strSql += " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                'strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
                strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & FILTERSTR & ")"
                'strSql += " WHERE ISNULL(TAGSNO,0) IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "')"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAG"
                'strSql += " WHERE ISSDATE <= '" & dtpAsOn.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " WHERE " & FILTERSTR
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                tran.Commit()
                MsgBox("Successfully Transfered", MsgBoxStyle.Information)
            End If

        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            dtpAsOn.Value = GetEntryDate(GetServerDate)
            txtTagNo.Focus()
            'dtpAsOn.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmTagTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmTagTransfer2CTag_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class