Imports System.Data.OleDb
Public Class frmOtherMasterEntry
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim OldMiscname As String = ""
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT MISCID,MISCNAME,CASE WHEN ACTIVE = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE  FROM " & cnAdminDb & "..OTHERMASTERENTRY"
        funcOpenGrid(strSql, gridView)
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbActive.Text = "YES"
        OldMiscname = ""
        flagSave = False
        funcCallGrid()
        txtmiscId_Man.Text = objGPack.GetMax("MISCID", "OTHERMASTERENTRY", cnAdminDb)
    End Function
    Function funcSave() As Integer
        If txtMIscName_MAN.Text.Trim = "" Then MsgBox("Menu name cannot empty.", MsgBoxStyle.Information) : txtMIscName_MAN.Focus() : Exit Function
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        strSql = " SELECT 1 FROM " & cnAdminDb & "..OTHERMASTERENTRY "
        strSql += " WHERE MISCNAME = '" & txtMIscName_MAN.Text & "'"
        If OldMiscname <> "" Then strSql += " AND MISCNAME<>'" & OldMiscname & "'"
        If objGPack.DupChecker(txtMIscName_MAN, strSql) Then Exit Function

        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim dt As New DataTable
        dt.Clear()

        strSql = " INSERT INTO " & cnAdminDb & "..OTHERMASTERENTRY"
        strSql += " ("
        strSql += " MISCID,MISCNAME,USERID,UPDATED,UPTIME,ACTIVE"
        strSql += " )VALUES("
        strSql += " " & Val(txtmiscId_Man.Text) & "" 'MISCID
        strSql += " ,'" & txtMIscName_MAN.Text & "'" 'MISCNAME
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += ")"
        Try
            If ExecQuery(SyncMode.Master, strSql, cn) = False Then Exit Function
            funcNew()
            Dim str As String = Nothing
            str = "Saved Successfully.."
            str += vbCrLf + "You must restart your application for some of the change"
            str += vbCrLf + "made by menu creation to take effect."
            str += vbCrLf + "Do you want to restart your application now?"
            If MessageBox.Show(str, "Restart Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Application.Restart()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " UPDATE " & cnAdminDb & "..OTHERMASTERENTRY SET"
        strSql += " MISCNAME='" & txtMIscName_MAN.Text & "'"
        strSql += " ,USERID=" & userId & ""
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " WHERE MISCID = '" & txtmiscId_Man.Text & "'"
        Try
            If ExecQuery(SyncMode.Master, strSql, cn) = False Then Exit Function
            funcNew()
            Dim str As String = Nothing
            str = "Updated Successfully.."
            str += vbCrLf + "You must restart your application for some of the change"
            str += vbCrLf + "made by menu creation to take effect."
            str += vbCrLf + "Do you want to restart your application now?"
            If MessageBox.Show(str, "Restart Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Application.Restart()
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            If cn.State = ConnectionState.Open Then

            End If
        End Try
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGetDetails(ByVal tempSizeId As Integer) As Integer
        strSql = " SELECT MISCID,MISCNAME,CASE WHEN ACTIVE = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE "
        strSql += " FROM " & cnAdminDb & "..OTHERMASTERENTRY AS I"
        strSql += " WHERE MISCID = '" & tempSizeId & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtmiscId_Man.Text = .Item("MISCID")
            txtMIscName_MAN.Text = .Item("MISCNAME")
            OldMiscname = .Item("MISCNAME")
            cmbActive.Text = .Item("ACTIVE")
        End With
        flagSave = True
    End Function

    Private Sub frmMiscEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmMiscEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        objGPack.Validator_Object(Me)
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        txtMIscName_MAN.CharacterCasing = CharacterCasing.Normal
        txtmiscId_Man.Enabled = False
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
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


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
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
                txtMIscName_MAN.Select()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtMIscName_MAN.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtSizeName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMIscName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMIscName_MAN.Text.Trim = "" Then MsgBox("Menu name cannot empty.", MsgBoxStyle.Information) : txtMIscName_MAN.Focus() : Exit Sub
            strSql = " SELECT 1 FROM " & cnAdminDb & "..OTHERMASTERENTRY "
            strSql += " WHERE MISCNAME = '" & txtMIscName_MAN.Text & "'"
            strSql += " AND MISCID <> '" & txtmiscId_Man.Text & "'"
            If objGPack.DupChecker(txtMIscName_MAN, strSql) Then Exit Sub
        End If
    End Sub
End Class