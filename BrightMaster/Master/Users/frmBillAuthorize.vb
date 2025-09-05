Imports System.Data.OleDb
Public Class frmBillAuthorize
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim DUELIMIT_USER_AUTH As String = GetAdmindbSoftValue("DUELIMIT_USER_AUTH", "N")


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        funcOpen()
        funcCmboLoad()
        CmbUserName_MAN.Text = ""
        CmbUserName_MAN.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = " SELECT "
        strSql += " USERID,"
        strSql += " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER AS E WHERE E.USERID = D.USERID)AS USERNAME,"
        strSql += " FROMDAYS,TODAYS"
        strSql += " FROM " & cnAdminDb & "..BILLAUTHORIZE AS D"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("USERID").Visible = False
            .Columns("USERNAME").Width = 200
            .Columns("FROMDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("FROMDAYS").Width = 80
            .Columns("TODAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TODAYS").Width = 80
        End With
        gridView.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..BILLAUTHORIZE"
            strSql += " WHERE USERID = (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & CmbUserName_MAN.Text & "')"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                MsgBox(E0002, MsgBoxStyle.Information)
                CmbUserName_MAN.Focus()
                Exit Function
            End If
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim Userid As Integer = Nothing
        Dim MetalId As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & CmbUserName_MAN.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "USERId")
        If ds.Tables("USERId").Rows.Count > 0 Then
            Userid = ds.Tables("USERId").Rows(0).Item("USERId")
        Else
            Userid = Nothing
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..BILLAUTHORIZE"
        strSql += " ("
        strSql += " USERID,FROMDAYS,TODAYS)"
        strSql += " VALUES("
        strSql += " " & Userid & "" 'USERID
        strSql += " ," & Val(txtFromDays_NUM.Text) & "" 'FROMDAYS
        strSql += " ," & Val(txtToDays_NUM.Text) & "" 'TODAYS
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnAdminDb & "..BILLAUTHORIZE SET"
        strSql += " FROMDAYS=" & Val(txtFromDays_NUM.Text) & ""
        strSql += " ,TODAYS=" & Val(txtToDays_NUM.Text) & ""
        strSql += " WHERE USERID = (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & CmbUserName_MAN.Text & "')"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " SELECT "
        strSql += " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER AS E WHERE E.USERID = D.USERID)AS USERNAME,"
        strSql += " FROMDAYS,TODAYS"
        strSql += " FROM " & cnAdminDb & "..BILLAUTHORIZE AS D"
        strSql += " WHERE USERID = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            CmbUserName_MAN.Text = .Item("USERNAME").ToString
            txtFromDays_NUM.Text = .Item("FROMDAYS").ToString
                txtToDays_NUM.Text = .Item("TODAYS").ToString
        End With
        flagSave = True
    End Function
    Function funcCmboLoad() As Integer
        strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'Y') <> 'N' ORDER BY USERNAME"
        objGPack.FillCombo(strSql, CmbUserName_MAN)
    End Function

    Private Sub frmDiscAuthorize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDiscAuthorize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NwewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NwewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                CmbUserName_MAN.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            CmbUserName_MAN.Focus()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..BILLAUTHORIZE WHERE 1<>1"
        Dim delQry As String = Nothing
        delQry += " DELETE FROM " & cnAdminDb & "..BILLAUTHORIZE"
        delQry += " WHERE USERID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("USERID").Value.ToString & "'"
        DeleteItem(SyncMode.Master, chkQry, delQry)
        funcOpen()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
End Class