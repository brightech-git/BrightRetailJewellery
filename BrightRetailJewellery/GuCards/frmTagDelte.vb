Imports System.Data.OleDb
Public Class frmTagDelte
    Dim da As OleDbDataAdapter
    Dim strsql As String
    Dim cmd As OleDbCommand

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE = 'Y' ORDER BY ITEMNAME "
        objGPack.FillCombo(strsql, cmbItem)
    End Sub

    Private Sub ProcNew()
        objGPack.TextClear(Me)
        cmbItem.Focus()
    End Sub

    Private Sub frmTagDelte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If txtTagNo.Text = "" Then
            MsgBox("TagNo Should Not Empty", MsgBoxStyle.Information)
            txtTagNo.Focus()
            Exit Sub
        End If
        tran = Nothing
        tran = cn.BeginTransaction
        Try
            strsql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            strsql += " WHERE TAGNO = '" & txtTagNo.Text & "'"
            strsql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            If objGPack.DupCheck(strsql, tran) = False Then
                MsgBox("Invalid Tag Details", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
            strsql = " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE"
            strsql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & txtTagNo.Text & "'"
            strsql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()

            strsql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
            strsql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & txtTagNo.Text & "'"
            strsql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()

            strsql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL"
            strsql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & txtTagNo.Text & "'"
            strsql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()

            strsql = " DELETE FROM " & cnAdminDb & "..ITEMTAG"
            strsql += " WHERE TAGNO = '" & txtTagNo.Text & "'"
            strsql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()

            tran.Commit()
            MsgBox("Successfully Deleted..")
            Me.Close()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran.Dispose()
            End If
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
End Class