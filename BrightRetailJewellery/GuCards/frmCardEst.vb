Imports System.Data.OleDb
Public Class frmCardEst

    Dim strSql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As DataTable
    Dim UpdateWt As Double
    Private Sub funcNew()
        txtWeight_Wet_Man.Text = ""
        txtSize_wet_man.Text = ""
        strSql = "  SELECT WT Weight,mm MilliMeter FROM " & cnAdminDb & "..GCARDWTVSMM"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        If dt.Columns.Count > 0 Then
            With gridView
                .Columns("WEIGHT").Width = 185
                .Columns("WEIGHT").SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns("MILLIMETER").Width = 185
                .Columns("MILLIMETER").SortMode = DataGridViewColumnSortMode.NotSortable

                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
            End With
        End If
        txtWeight_Wet_Man.Enabled = True
        btnSave.Text = "Save"
        txtWeight_Wet_Man.Focus()
    End Sub

    Private Sub frmCardEst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                SendKeys.Send("{TAB}")
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub


    Private Sub frmCardEst_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            funcNew()
            txtWeight_Wet_Man.Focus()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If txtWeight_Wet_Man.Text = "" Then
                MsgBox("Weight Should Not Empty...")
                txtWeight_Wet_Man.Focus()
                Exit Sub
            End If
            If txtSize_wet_man.Text = "" Then
                MsgBox("Size ShouldNot Empty...")
                txtSize_wet_man.Focus()
                Exit Sub
            End If
            If btnSave.Text = "Save" Then
                strSql = " SELECT COUNT(*) COUNTROW FROM " & cnAdminDb & "..GCARDWTVSMM WHERE ISNULL(WT,0) = '" & txtWeight_Wet_Man.Text & "'"
                If CInt(objGPack.GetSqlValue(strSql, "COUNTROW", "0")) > 0 Then
                    MsgBox("Weight Already Exists")
                    txtWeight_Wet_Man.Focus()
                    Exit Sub
                End If
                strSql = " INSERT INTO " & cnAdminDb & "..GCARDWTVSMM(WT,MM) VALUES"
                strSql += " ('" & txtWeight_Wet_Man.Text & "','" & txtSize_wet_man.Text & "')"
                ExecQuery(SyncMode.Stock, strSql, cn)
            Else
                strSql = " UPDATE " & cnAdminDb & "..GCARDWTVSMM SET mm ='" & txtSize_wet_man.Text & "' WHERE ISNULL(WT,0) ='" & txtWeight_Wet_Man.Text & "'"
                ExecQuery(SyncMode.Stock, strSql, cn)
            End If
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            If gridView.RowCount > 0 Then
                gridView.Focus()
            Else
                txtWeight_Wet_Man.Focus()
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.Click
        Try
            If gridView.RowCount > 0 Then
                gridView.Focus()
            Else
                txtWeight_Wet_Man.Focus()
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Try
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If gridView.RowCount > 0 Then
                    txtSize_wet_man.Text = gridView.CurrentRow.Cells("millimeter").Value.ToString()
                    txtWeight_Wet_Man.Text = gridView.CurrentRow.Cells("weight").Value.ToString()
                    txtWeight_Wet_Man.Enabled = False
                    btnSave.Text = "Update"
                End If
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub
End Class