Imports System.Data.OleDb
Public Class frmOtherMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Menuid As Integer
    Dim OldName As String = ""
    Public Sub New(ByVal Mnuid As Integer, ByVal title As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = title
        Me.Menuid = Mnuid
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT ID,NAME"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS M WHERE M.ITEMID = I.ITEMID)AS ITEMNAME"
        strSql += " ,CASE WHEN ACTIVE = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE "
        strSql += " FROM " & cnAdminDb & "..OTHERMASTER AS I"
        strSql += " WHERE MISCID='" & Menuid & "'"
        funcOpenGrid(strSql, gridView)
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
        gridView.Columns("ID").Width = 80
        gridView.Columns("NAME").Width = 150
        gridView.Columns("ITEMNAME").Width = 150
        gridView.Columns("ACTIVE").Width = 80

    End Function
    Function funcLoadItemName() As Integer
        Dim dt As New DataTable
        dt.Clear()
        cmbItemName_Man.Items.Clear()
        strSql = " SELECT 'ALL' ITEMNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,2 RESULT FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ISNULL(ACTIVE,'') <> 'N'"
        strSql += " ORDER BY RESULT,ITEMNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer
        For cnt = 0 To dt.Rows.Count - 1
            cmbItemName_Man.Items.Add(dt.Rows(cnt).Item("ITEMNAME"))
        Next
        cmbItemName_Man.Text = dt.Rows(0).Item("ITEMNAME")
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        cmbActive.Text = "YES"
        OldName = ""
        funcCallGrid()
        funcLoadItemName()
        txtPatternId_Num_Man.Text = objGPack.GetMax("ID", "OTHERMASTER", cnAdminDb)
        cmbItemName_Man.Select()
    End Function
    Function funcSave() As Integer
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        strSql = " SELECT 1 FROM " & cnAdminDb & "..OTHERMASTER "
        strSql += " WHERE NAME = '" & txtPatternName__Man.Text & "'"
        If OldName <> "" Then strSql += " AND NAME<>'" & OldName & "'"
        If cmbItemName_Man.Text <> "ALL" Then strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        strSql += " AND MISCID = '" & Menuid & "'"
        If objGPack.DupChecker(txtPatternName__Man, strSql) Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim dt As New DataTable
        dt.Clear()
        Dim itemId As Integer = Nothing

        If cmbItemName_Man.Text <> "ALL" Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                itemId = dt.Rows(0).Item("ITEMID")
            End If
        Else
            itemId = 0
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..OTHERMASTER"
        strSql += " ("
        strSql += " ID,NAME,MISCID,ITEMID,USERID,UPDATED,UPTIME,ACTIVE"
        strSql += " )VALUES("
        strSql += " " & Val(txtPatternId_Num_Man.Text) & "" 'Id
        strSql += " ,'" & txtPatternName__Man.Text & "'" 'Name
        strSql += " ," & Menuid & "" 'MiscId
        strSql += " ," & itemId & "" 'ItemId
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += ")"
        Try
            If ExecQuery(SyncMode.Master, strSql, cn) = False Then Exit Function
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim dt As New DataTable
        dt.Clear()

        Dim itemId As Integer = Nothing
        If cmbItemName_Man.Text <> "ALL" Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
        Else
            itemId = 0
        End If

        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If
        strSql = " UPDATE " & cnAdminDb & "..OTHERMASTER SET"
        strSql += " ITEMID=" & itemId & ""
        strSql += " ,NAME='" & txtPatternName__Man.Text & "'"
        strSql += " ,MISCID=" & Menuid & ""
        strSql += " ,USERID=" & userId & ""
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " WHERE ID = '" & txtPatternId_Num_Man.Text & "'"
        Try
            If ExecQuery(SyncMode.Master, strSql, cn) = False Then Exit Function

            funcNew()
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
        strSql = " SELECT ID,NAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS M WHERE M.ITEMID = I.ITEMID)AS ITEMNAME"
        strSql += " ,CASE WHEN ACTIVE = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE "
        strSql += " FROM " & cnAdminDb & "..OTHERMASTER AS I"
        strSql += " WHERE ID = '" & tempSizeId & "' AND MISCID='" & Menuid & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = IIf(IsDBNull(.Item("ITEMNAME")), "ALL", .Item("ITEMNAME"))
            txtPatternId_Num_Man.Text = .Item("ID")
            txtPatternName__Man.Text = .Item("NAME")
            cmbActive.Text = .Item("ACTIVE")
            OldName = .Item("NAME")
        End With
        flagSave = True
    End Function

    Private Sub frmPattern_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmPattern_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = Color.Silver
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        txtPatternId_Num_Man.Enabled = False
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.View) Then Exit Sub
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
                cmbItemName_Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtSizeName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPatternName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..OTHERMASTER "
            strSql += " WHERE NAME = '" & txtPatternName__Man.Text & "'"
            If cmbItemName_Man.Text <> "ALL" Then strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            strSql += " AND ID <> '" & txtPatternId_Num_Man.Text & "'"
            strSql += " AND MISCID = '" & Menuid & "'"
            If objGPack.DupChecker(txtPatternName__Man, strSql) Then Exit Sub
        End If
    End Sub
End Class