Imports System.Data.OleDb
Public Class frmDiscAuthorize
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
        cmbMetalName_Man.Text = ""
        cmbEmplyeeName_Man.Text = ""
        If DUELIMIT_USER_AUTH = "Y" Then txtCre_AMT.Enabled = True : txtCre_Per.Enabled = True
        pnlControls.Enabled = True
        cmbEmplyeeName_Man.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = " SELECT "
        strSql += " EMPID,"
        strSql += " (SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER AS E WHERE E.EMPID = D.EMPID)AS EMPNAME,"
        strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M WHERE M.METALID=D.METALID)AS METALNAME,"
        strSql += " DISCPER,DISCAMT,DUEPER,DUEAMT"
        strSql += " FROM " & cnAdminDb & "..DISCAUTHORIZE AS D"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("EMPID").Visible = False
            .Columns("EMPNAME").Width = 200
            .Columns("METALNAME").Width = 150
            .Columns("DISCPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCPER").Width = 80
            .Columns("DISCAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCAMT").Width = 80
            If DUELIMIT_USER_AUTH = "Y" Then
                .Columns("DUEPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DUEPER").Width = 80
                .Columns("DUEAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DUEAMT").Width = 80
            Else
                .Columns("DUEPER").Visible = False
                .Columns("DUEAMT").Visible = False
            End If
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
            strSql = " SELECT 1 FROM " & cnAdminDb & "..DISCAUTHORIZE"
            strSql += " WHERE EMPID = (SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & cmbEmplyeeName_Man.Text & "')"
            strSql += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                MsgBox(E0002, MsgBoxStyle.Information)
                cmbMetalName_Man.Focus()
                Exit Function
            End If
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim EmpId As Integer = Nothing
        Dim MetalId As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select EmpId from " & cnAdminDb & "..EmpMaster where EmpName = '" & cmbEmplyeeName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "Empid")
        If ds.Tables("EmpId").Rows.Count > 0 Then
            EmpId = ds.Tables("Empid").Rows(0).Item("EmpId")
        Else
            EmpId = Nothing
        End If
        strSql = " Select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("MetalId").Rows.Count > 0 Then
            MetalId = ds.Tables("MetalId").Rows(0).Item("MetalId")
        Else
            MetalId = Nothing
        End If
        strSql = " Insert into " & cnAdminDb & "..DiscAuthorize"
        strSql += " ("
        strSql += " EmpId,MetalId,Password,"
        strSql += " DiscPer,Discamt,DuePer,Dueamt,UserId,"
        strSql += " Updated,UpTime)Values("
        strSql += " " & EmpId & "" 'EmpId
        strSql += " ,'" & MetalId & "'" 'MetalId
        strSql += " ,'" & objGPack.Encrypt(txtPassword__Man.Text) & "'" 'Password
        strSql += " ," & Val(txtDisc_Per.Text) & "" 'DiscPer
        strSql += " ," & Val(txtDisc_Amt.Text) & "" 'Discamt
        strSql += " ," & Val(txtCre_Per.Text) & "" 'DiscPer
        strSql += " ," & Val(txtCre_AMT.Text) & "" 'Discamt
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UpTime
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
        strSql = " UPDATE " & cnAdminDb & "..DISCAUTHORIZE SET"
        strSql += " DISCPER=" & Val(txtDisc_Per.Text) & ""
        strSql += " ,DISCAMT=" & Val(txtDisc_Amt.Text) & ""
        strSql += " ,DUEPER=" & Val(txtCre_Per.Text) & ""
        strSql += " ,DUEAMT=" & Val(txtCre_AMT.Text) & ""
        strSql += " ,USERID=" & userId & ""
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        ' strSql += " ,PASSWORD = '" & objGPack.Encrypt(txtPassword__Man.Text) & "'"
        strSql += " WHERE EMPID = (SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & cmbEmplyeeName_Man.Text & "')"
        strSql += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " Select "
        strSql += " (select EmpName from " & cnAdminDb & "..EmpMaster as e where e.EmpId = d.EmpId)as EmpName,"
        strSql += " (select MetalName from " & cnAdminDb & "..MetalMast as m where m.MetalId=d.MetalId)as MetalName,"
        strSql += " DiscPer,Discamt,DuePer,Dueamt,password"
        strSql += " from " & cnAdminDb & "..DiscAuthorize as D"
        strSql += " Where EmpId = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbEmplyeeName_Man.Text = .Item("EmpName").ToString
            cmbMetalName_Man.Text = .Item("MetalName").ToString
            txtPassword__Man.Text = .Item("Password").ToString
            txtDisc_Per.Text = .Item("DiscPer").ToString
            txtDisc_Amt.Text = .Item("Discamt").ToString
            txtCre_Per.Text = .Item("DuePer").ToString
            txtCre_AMT.Text = .Item("Dueamt").ToString
        End With
        flagSave = True
        pnlControls.Enabled = False
        If userId = "999" Then
            txtPassword__Man.Enabled = True
        Else
            txtPassword__Man.Enabled = False
        End If

    End Function
    Function funcCmboLoad() As Integer
        strSql = " select EmpName from " & cnAdminDb & "..EmpMaster WHERE DISCAUTHORIZE = 'Y' order by EmpName"
        objGPack.FillCombo(strSql, cmbEmplyeeName_Man)

        strSql = " select MetalName from " & cnAdminDb & "..MetalMast order by displayorder"
        objGPack.FillCombo(strSql, cmbMetalName_Man)
    End Function

    Private Sub frmDiscAuthorize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPassword__Man.Focused = False Then SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDiscAuthorize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtPassword__Man.CharacterCasing = CharacterCasing.Normal
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
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
                If pnlControls.Enabled = True Then
                    cmbEmplyeeName_Man.Focus()
                Else
                    txtPassword__Man.Focus()
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If pnlControls.Enabled = True Then
                cmbEmplyeeName_Man.Focus()
            Else
                txtDisc_Per.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..DISCAUTHORIZE WHERE 1<>1"
        Dim delQry As String = Nothing
        delQry += " DELETE FROM " & cnAdminDb & "..DISCAUTHORIZE"
        delQry += " WHERE EMPID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("EMPID").Value.ToString & "'"
        delQry += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("METALNAME").Value.ToString & "')"
        DeleteItem(SyncMode.Master, chkQry, delQry)
        funcOpen()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub
End Class