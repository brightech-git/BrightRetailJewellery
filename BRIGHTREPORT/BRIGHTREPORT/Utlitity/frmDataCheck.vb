Imports System.Data.OleDb
Public Class frmDataCheck
    Dim Strsql As String
    Dim Cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btncheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncheck.Click
        If CmbYear.Text = "" Then MsgBox("Please Select Database", MsgBoxStyle.Information) : Exit Sub
        gridView.DataSource = Nothing
        gridView.Refresh()
        Strsql = "EXEC " & cnAdminDb & "..SP_CHECKDATACONSISTENT"
        Strsql += " @DBNAME='" & CmbYear.Text & "'"
        Strsql += " ,@CHKCOSTID='" & IIf(ChkCostid.Checked, "Y", "N") & "'"
        Cmd = New OleDbCommand(Strsql, cn)
        Cmd.ExecuteNonQuery()
        Strsql = "SELECT TABLENAME,FIELD AS COLUMNNAME,STATUS"
        Strsql += " FROM TEMPTABLEDB..CHKFIELD ORDER BY TABLENAME,FIELD "
        da = New OleDbDataAdapter(Strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                .Columns("TABLENAME").Width = 150
            End With
        End If
    End Sub

    Private Sub frmDataCheck_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDataCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Strsql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME IN(SELECT NAME FROM MASTER..SYSDATABASES)"
        objGPack.FillCombo(Strsql, CmbYear, True)
        CmbYear.Text = cnStockDb
    End Sub
End Class