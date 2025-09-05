Imports System.Data.OleDb
Public Class frmUserCash
    Dim strSql As String
    Dim flagSave As Boolean
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCounter As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Function funcGetDetails(ByVal SNO As Integer) As Integer
        strSql = " SELECT SNO,U.USERNAME AS USERNAME,C.CASHNAME AS CASHCOUNTER,CASE WHEN UC.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " ,ISNULL(ITEMCTRID,'') AS ITEMCTRIDS"
        strSql += " FROM " & cnAdminDb & "..USERCASH UC"
        strSql += " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=UC.USERID"
        strSql += " LEFT JOIN " & cnAdminDb & "..CASHCOUNTER C ON C.CASHID=UC.CASHID"
        strSql += " WHERE SNO = " & SNO & ""
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtsno.Text = dt.Rows(0).Item("SNO").ToString
            cmbUser_MAN.Text = dt.Rows(0).Item("USERNAME").ToString
            CmbCash_Man.Text = dt.Rows(0).Item("CASHCOUNTER").ToString
            cmbActive.Text = dt.Rows(0).Item("ACTIVE").ToString
            Dim selCounters As New List(Of String)
            For Each s As String In dt.Rows(0).Item("ITEMCTRIDS").ToString.Split(",")
                selCounters.Add(s.Replace("'", ""))
            Next

            If dt.Rows(0).Item("ITEMCTRIDS").ToString = "" Then
                chkCmbItemCounter.SetItemChecked(0, True)
            Else
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    If selCounters.Contains(dtCounter.Rows(cnt).Item("ITEMCTRID").ToString) Then
                        chkCmbItemCounter.SetItemChecked(cnt, True)
                    Else
                        chkCmbItemCounter.SetItemChecked(cnt, False)
                    End If
                Next
            End If
            flagSave = True
        End If
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbActive.Text = "YES"
        flagSave = False
        strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'')<>'N' AND USERID<>999"
        objGPack.FillCombo(strSql, cmbUser_MAN, True, False)
        strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE ISNULL(ACTIVE,'')<>'N'"
        objGPack.FillCombo(strSql, CmbCash_Man, True, False)
        dtCounter = New DataTable
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' AS ITEMCTRID,1 RESULT "
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        funcCallGrid()
        Dim dt As New DataTable
        cmbUser_MAN.Focus()
    End Function
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,U.USERNAME AS USERNAME,C.CASHNAME AS CASHCOUNTER,CASE WHEN UC.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..USERCASH UC"
        strSql += " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=UC.USERID"
        strSql += " LEFT JOIN " & cnAdminDb & "..CASHCOUNTER C ON C.CASHID=UC.CASHID"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("SNO").Visible = False
        gridView.Columns("USERNAME").MinimumWidth = 200
        gridView.Columns("CASHCOUNTER").MinimumWidth = 200
    End Function

    Private Function GetSelectedCounters() As String
        Dim selectedItemCounters As String = ""
        If chkCmbItemCounter.Text <> "ALL" And chkCmbItemCounter.Text <> "" Then
            For cnt As Integer = 0 To chkCmbItemCounter.Items.Count - 1
                If chkCmbItemCounter.GetItemChecked(cnt) = False Then Continue For
                selectedItemCounters += "''" & dtCounter.Rows(cnt).Item("ITEMCTRID").ToString & "'',"
            Next
            If selectedItemCounters <> "" Then
                selectedItemCounters = Mid(selectedItemCounters, 1, selectedItemCounters.Length - 1)
            End If
        End If
        Return selectedItemCounters
    End Function

    Private Sub frmUserCash_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmUserCash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        funcNew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        If gridView.RowCount > 0 Then
            gridView.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        Try
            strSql = " SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME ='" & cmbUser_MAN.Text & "'"
            Dim USERID As Integer = Val(objGPack.GetSqlValue(strSql, , "", ))
            strSql = " SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME='" & CmbCash_Man.Text & "'"
            Dim CASHID As String = objGPack.GetSqlValue(strSql, , "", )

            If flagSave = False Then
                strSql = "SELECT 1 FROM " & cnAdminDb & "..USERCASH WHERE USERID= '" & USERID & "' AND CASHID = '" & CASHID & "'"
                If Val(objGPack.GetSqlValue(strSql, , "0", )) = 1 Then
                    MsgBox("This group already exists", MsgBoxStyle.Information)
                    Exit Sub
                End If
                funcAdd(USERID, CASHID)
            Else
                funcUpdate(USERID, CASHID)
            End If
            funcCallGrid()
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Function funcAdd(ByVal USERID As Integer, ByVal CASHID As String) As Integer
        Dim selectedItemCounters As String = GetSelectedCounters()
        strSql = " INSERT INTO " & cnAdminDb & "..USERCASH (USERID,CASHID,ACTIVE,ITEMCTRID) VALUES(" & USERID & ",'" & CASHID & "','" & Mid(cmbActive.Text, 1, 1) & "','" & selectedItemCounters & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcUpdate(ByVal USERID As Integer, ByVal CASHID As String) As Integer
        Dim selectedItemCounters As String = GetSelectedCounters()
        strSql = " UPDATE " & cnAdminDb & "..USERCASH SET USERID = " & USERID & ""
        strSql += " ,CASHID = '" & CASHID & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,ITEMCTRID = '" & selectedItemCounters & "'"
        strSql += " WHERE SNO = '" & Val(txtsno.Text) & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString))
                cmbUser_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

End Class