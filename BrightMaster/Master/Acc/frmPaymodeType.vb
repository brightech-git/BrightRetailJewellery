Imports System.Data.OleDb
Public Class frmPaymodeType
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim flagUpdateId As String = Nothing
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub frmPaymodeType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub frmPaymodeType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LoadGrid()
        strSql = "SELECT MODEID,MODENAME FROM " & cnAdminDb & "..PAYMENTMODE ORDER BY MODEID"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid As New DataTable
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        'gridView.Columns("TYPEID").Visible = False
        gridView.Columns("MODEID").HeaderText = "PAYMENTMODE ID"
        gridView.Columns("MODENAME").HeaderText = "PAYMENTMODE NAME"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODENAME = '" & txtPaymodeTypeName_MAN.Text & "' AND MODEID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtPaymodeTypeName_MAN.Focus()
            Exit Sub
        End If
        If flagUpdateId <> Nothing Then 'UPDATE
            strSql = "UPDATE " & cnAdminDb & "..PAYMENTMODE"
            strSql += " SET MODENAME = '" & txtPaymodeTypeName_MAN.Text & "'"
            strSql += " WHERE MODEID = '" & flagUpdateId & "'"
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Successfully Updated")
            btnNew_Click(Me, New EventArgs)
            Exit Sub
        End If
        Dim cnt As Integer = objGPack.GetSqlValue("SELECT COUNT(*)+1 FROM " & cnAdminDb & "..PAYMENTMODE")
        Dim discId As String = Nothing
genDis:

        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODEID = '" & discId & "'").Length > 0 Then
            cnt += 1
            GoTo genDis
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..PAYMENTMODE"
        strSql += " (MODENAME)VALUES"
        strSql += " ('" & txtPaymodeTypeName_MAN.Text & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
        MsgBox("Successfully Inserted")
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        LoadGrid()
        If gridView.Rows.Count > 0 Then
            gridView.Focus()
        End If
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtPaymodeTypeName_MAN.Clear()
        flagUpdateId = Nothing
        LoadGrid()
        txtPaymodeTypeName_MAN.Focus()
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub
    Private Sub gridView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
                txtPaymodeTypeName_MAN.Text = gridView.CurrentRow.Cells("MODENAME").Value
                flagUpdateId = gridView.CurrentRow.Cells("MODEID").Value
                txtPaymodeTypeName_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtPaymodeTypeName_MAN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPaymodeTypeName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPaymodeTypeName_MAN.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODENAME = '" & txtPaymodeTypeName_MAN.Text & "' AND MODEID <> '" & flagUpdateId & "'").Length > 0 Then
                MsgBox("Already exist", MsgBoxStyle.Information)
                txtPaymodeTypeName_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtPaymodeTypeName_MAN_LostFocus(sender As Object, e As EventArgs) Handles txtPaymodeTypeName_MAN.LostFocus
        If txtPaymodeTypeName_MAN.Text = "" Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODENAME = '" & txtPaymodeTypeName_MAN.Text & "' AND MODEID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtPaymodeTypeName_MAN.Focus()
        End If
    End Sub
End Class