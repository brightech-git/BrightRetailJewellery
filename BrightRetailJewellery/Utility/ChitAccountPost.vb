Imports System.Data.OleDb
Public Class ChitAccountPost
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim objGridShower As New frmGridDispDia
    Private Sub ChitAccountPost_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ChitAccountPost_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If cnChitTrandb = "" Then
            MsgBox("Scheme database not found", MsgBoxStyle.Information)
            Me.Close()
            Exit Sub
        End If
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        Try
            If dtpFrom.Value < cnTranFromDate Or dtpTo.Value > cnTranToDate Then
                MsgBox("Date between " & cnTranFromDate & " and " & cnTranToDate & " only Allowed", MsgBoxStyle.Information)
                dtpFrom.Focus()
                Exit Sub
            End If
            strSql = " EXEC " & cnAdminDb & "..SP_ACCODE_CHK @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            Dim dtAccode As New DataTable
            Dim dss As New DataSet
            da.Fill(dtAccode)
            'dtAccode = dss.Tables(0)
            'dss.Tables.Clear()
            If dtAccode.Rows.Count > 0 Then
                objGridShower = New frmGridDispDia
                objGridShower.Name = Me.Name
                objGridShower.gridView.RowTemplate.Height = 21
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                objGridShower.Text = "SCHEME ACCOUNT DETAIL"
                Dim tit As String = "SCHEME ACCOUNT DETAIL FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                objGridShower.lblTitle.Text = tit
                objGridShower.StartPosition = FormStartPosition.CenterScreen
                objGridShower.dsGrid.DataSetName = objGridShower.Name
                objGridShower.dsGrid.Tables.Add(dtAccode)
                objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                objGridShower.FormReSize = True
                objGridShower.FormReLocation = False
                objGridShower.pnlFooter.Visible = False
                objGridShower.gridView.Columns("DATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                objGridShower.gridView.Columns("NAME").Width = 200
                objGridShower.Show()
                Exit Sub
            End If

            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " EXEC " & cnStockDb & "..SP_CHIT_ACC_POST @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            MsgBox("SCHEME Posting Successfully Completed", MsgBoxStyle.Information)
            btnExit_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class