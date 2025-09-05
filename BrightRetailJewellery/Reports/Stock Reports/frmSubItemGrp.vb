Imports System.Data.OleDb
Public Class frmSubItemGrp
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim blnPcs As Boolean
    Public dsGrid As New DataSet
    Private Sub frmSubItemGrp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = "SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtSubGrp As New DataTable
        da.Fill(dtSubGrp)
        'cmbSGrpName.Items.Clear()
        For Each Row As DataRow In dtSubGrp.Rows
            cmbSGrpName.Items.Add(Row.Item("SGROUPNAME").ToString)
        Next
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim sGrpID As Integer
        strSql = "SELECT SGROUPID FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPNAME='" & cmbSGrpName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtSGrpId As New DataTable
        da.Fill(dtSGrpId)
        If dtSGrpId.Rows.Count > 0 Then
            sGrpID = dtSGrpId.Rows(0).Item("SGROUPID").ToString
        End If
        strSql = " UPDATE " & cnAdminDb & "..SUBITEMMAST SET SGROUPID = " & sGrpID & ""
        strSql += " WHERE SUBITEMNAME = '" & txtSubItem.Text & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
        Me.Close()
    End Sub

    Private Sub frmSubItemGrp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'Call GridView_KeyPress(sender, e)
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class