Imports System.Data.OleDb
Public Class frmOthers
    Dim tran As OleDbTransaction
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public DefTranName As String
    Public DefAcname As String
    Public DefTranCode As String
    Public DefAcCode As String
    Public TranType As String
    Public Amount As String
    Public DefBatchno As String
    Public BillCostId As String = Nothing
    Private Sub frmOthers_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmOthers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtRemarks_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemarks.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If MsgBox("Do you want to Update?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Dim TAccode As String = ""
                Dim TTrancode As String = ""
                TAccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text.ToString.Trim & "'")
                TTrancode = objGPack.GetSqlValue("SELECT PROID FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME='" & cmbTranType.Text.ToString.Trim & "'")
                Try
                    tran = cn.BeginTransaction
                    strSql = "UPDATE " & cnAdminDb & "..OUTSTANDING SET CTRANCODE='" & TTrancode & "',REMARK1='" & txtRemarks.Text.ToString.Trim & "' WHERE BATCHNO='" & DefBatchno & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET ACCODE='" & TAccode & "',REMARK1='" & txtRemarks.Text.ToString.Trim & "' WHERE BATCHNO = '" & DefBatchno & "' AND ACCODE='" & DefAcCode & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA='" & TAccode & "',REMARK1='" & txtRemarks.Text.ToString.Trim & "' WHERE BATCHNO = '" & DefBatchno & "' AND CONTRA='" & DefAcCode & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    tran.Commit()
                    tran = Nothing
                    MsgBox(" Successfully Updated...")
                    Me.Close()
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox("Error   :" + ex.Message + vbCrLf + ex.StackTrace)
                End Try

            Else
                txtPaytype.Select()
            End If
        End If
    End Sub
End Class