Imports System.Data.OleDb
Public Class CatalogCart
    Public dtGrid As New DataTable
    Public Sub New(ByVal DtGrid As DataTable)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.dtGrid = DtGrid
    End Sub

    Private Sub CatalogCart_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Ignore
            Me.Close()
            Exit Sub
        End If
        If e.Control Then
            If e.KeyCode = Keys.D Then
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                If MessageBox.Show("Do you want to delete all selected catalog items", "Delete Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    Dim StrSql As String
                    Dim Cmd As OleDbCommand
                    StrSql = " DELETE FROM " & cnStockDb & "..STYLECARD"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.ExecuteNonQuery()
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub CatalogCart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With DataGridView1
            dtGrid.Columns("SAMPLE").SetOrdinal(0)
            .RowTemplate.Height = 100
            .DataSource = dtGrid
            .Columns("STYLENO").Width = 120
            .Columns("CARTNAME").Width = 100
            .Columns("DESCRIPTION").Width = 150
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 70
            .Columns("SAMPLE").Width = 100
            .Columns("PCTFILE_HIDE").Visible = False
            .Columns("SID_HIDE").Visible = False
            .Columns("CATSNO_HIDE").Visible = False
            BrighttechPack.GlobalMethods.FormatGridColumns(DataGridView1, True, True, True)
        End With
    End Sub

    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If DataGridView1.CurrentRow Is Nothing Then Exit Sub
            DataGridView1.CurrentCell = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("SAMPLE")
        End If
    End Sub

    Private Sub DataGridView1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DataGridView1.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If DataGridView1.CurrentRow Is Nothing Then Exit Sub
            DataGridView1.CurrentCell = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells("SAMPLE")
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub DataGridView1_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.UserDeletedRow

    End Sub

    Private Sub DataGridView1_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles DataGridView1.UserDeletingRow
        If Not e.Row.Index >= 0 Then Exit Sub
        Dim StrSql As String
        Dim Cmd As OleDbCommand
        StrSql = " DELETE FROM " & cnStockDb & "..STYLECARD WHERE SID = " & Val(e.Row.Cells("SID_HIDE").Value.ToString) & ""
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        dtGrid.AcceptChanges()
    End Sub
End Class