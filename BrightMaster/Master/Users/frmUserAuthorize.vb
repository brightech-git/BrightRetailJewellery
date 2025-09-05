Imports System.Data.OleDb
Public Class frmUserAuthorize
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
        Dim DTOP As New DataTable
        strSql = "SELECT OPTIONID,OPTIONNAME FROM " & cnAdminDb & "..PRJPWDOPTION where active='Y'"
        da = New OleDbDataAdapter(strSql, cn)
        dtOP = New DataTable()
        da.Fill(dtOP)
        If dtOP.Rows.Count > 0 Then
            cmboptionname_OWN.DataSource = dtOP
            cmboptionname_OWN.DisplayMember = "OPTIONNAME"
            cmboptionname_OWN.ValueMember = "OPTIONID"
            cmboptionname_OWN.SelectedIndex = 0
            'cmboptionname_SelectedIndexChanged(Me, New EventArgs)
        End If

        cmbEmplyeeName_Man.Text = ""
        cmbEmplyeeName_Man.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = " SELECT VALUEID,"
        strSql += " AUSERID,"
        strSql += " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER AS E WHERE E.USERID = D.AUSERID)AS USERNAME,"
        strSql += " VALUENAME,VALUETYPE,VALUEVALUE "
        strSql += " FROM " & cnAdminDb & "..USERAUTHORIZE AS D"
        If cmbEmplyeeName_Man.Text <> "" Then strSql += " WHERE AUSERID IN (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbEmplyeeName_Man.Text & "')"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("VALUEID").Visible = False
            .Columns("AUSERID").Visible = False
            .Columns("USERNAME").Width = 200
            .Columns("VALUENAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VALUENAME").Width = 80
            .Columns("VALUEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VALUEVALUE").Width = 80
            .Columns("VALUETYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VALUETYPE").Width = 80

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
            strSql = " SELECT 1 FROM " & cnAdminDb & "..USERAUTHORIZE"
            strSql += " WHERE AUSERID = (SELECT top 1 USERID FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'') <> 'N' AND USERNAME = '" & cmbEmplyeeName_Man.Text & "')"
            strSql += " AND VALUEID = " & Val(cmboptionname_OWN.SelectedValue)

            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                MsgBox(E0002, MsgBoxStyle.Information)
                cmbValueType.Focus()
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
        strSql = " select Userid from " & cnAdminDb & "..UserMaster where UserName = '" & cmbEmplyeeName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "Empid")
        If ds.Tables("EmpId").Rows.Count > 0 Then
            EmpId = ds.Tables("Empid").Rows(0).Item("UserId")
        Else
            EmpId = Nothing
        End If
        strSql = " Insert into " & cnAdminDb & "..UserAuthorize"
        strSql += " ("
        strSql += " Auserid,VALUEID,VALUENAME,VALUETYPE,VALUEVALUE,"
        strSql += " USERID,Updated,UpTime)Values("
        strSql += " " & EmpId & "" 'EmpId
        strSql += " ," & cmboptionname_OWN.SelectedValue & "" 'DiscPer
        strSql += " ,'" & cmboptionname_OWN.Text & "'" 'DiscPer
        strSql += " ,'" & Mid(cmbValueType.Text, 1, 1) & "'" 'DiscPer
        strSql += " ," & Val(txtValue.Text) & "" 'Discamt
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
        strSql = " UPDATE " & cnAdminDb & "..USERAUTHORIZE SET"
        strSql += " ,VALUEVALUE='" & txtValue.Text & "'"
        strSql += " ,VALUETYPE='" & Mid(cmbValueType.Text, 1, 1) & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += " WHERE AUSERID = (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbEmplyeeName_Man.Text & "')"
        strSql += " AND VALUENAME= '" & cmboptionname_OWN.Text & "'"
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
        strSql += " (select USERNAME from " & cnAdminDb & "..UserMaster as e where e.AUserId = d.UserId)as UserName,"
        strSql += " Valuename,CASE WHEN Valuetype='P' then 'PERCENT' ELSE WHEN VALUETYPE = 'N' THEN 'NUMERIC' else 'TEXT' end as valuetype ,Valuevalue,Active"
        strSql += " from " & cnAdminDb & "..UserAuthorize as D"
        strSql += " Where AUserid = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbEmplyeeName_Man.Text = .Item("UserName").ToString
        
            cmboptionname_OWN.Text = .Item("Valuename").ToString
            txtValue.Text = .Item("Valuevalue").ToString
            cmbValueType.Text = .Item("valuetype").ToString


        End With
        flagSave = True
        

    End Function
    Function funcCmboLoad() As Integer
        strSql = " select UserName from " & cnAdminDb & "..usermaster where ISNULL(ACTIVE,'') <> 'N'  order by username"
        objGPack.FillCombo(strSql, cmbEmplyeeName_Man)

        
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
            End If
        ElseIf e.KeyCode = Keys.Escape Then

            cmboptionname_OWN.Focus()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..USERAUTHORIZE WHERE 1<>1"
        Dim delQry As String = Nothing
        delQry += " DELETE FROM " & cnAdminDb & "..USERAUTHORIZE"
        delQry += " WHERE AUSERID = " & gridView.Rows(gridView.CurrentRow.Index).Cells("AUSERID").Value.ToString
        delQry += " AND ISNULL(VALUEID,0) = " & Val(gridView.Rows(gridView.CurrentRow.Index).Cells("VALUEID").Value.ToString)
        DeleteItem(SyncMode.Master, chkQry, delQry)
        funcOpen()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)



    End Sub
End Class