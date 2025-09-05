Imports System.Data.OleDb
Public Class frm4cgroup
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Groupid As Integer = 0
    Dim CutId As Integer = 0
    Dim Colorid As Integer = 0
    Dim ClarityId As Integer = 0
    Dim ShapeId As Integer = 0
    Dim Sizeid As Integer = 0
    Dim Typeid As Integer = 0
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        gridView.DataSource = Nothing
        strSql = " SELECT GROUPID, GROUPNAME,"
        strSql += vbCrLf + " ISNULL((SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT AS S WHERE S.CUTID = CR.CUTID),'')AS CUTNAME,"
        strSql += vbCrLf + " ISNULL((SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR AS S WHERE S.COLORID = CR.COLORID),'')AS COLORNAME,"
        strSql += vbCrLf + " ISNULL((SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY AS S WHERE S.CLARITYID = CR.CLARITYID),'')AS CLARITYNAME,"
        strSql += vbCrLf + " ISNULL((SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE AS SH WHERE SH.SHAPEID = CR.SHAPEID),'')AS SHAPENAME,"
        strSql += vbCrLf + " ISNULL((SELECT SIZENAME FROM " & cnAdminDb & "..STNSIZE AS SH WHERE SH.STNSIZEID = CR.SIZEID),'')AS SIZENAME,"
        strSql += vbCrLf + " ISNULL((SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE AS SH WHERE SH.SETTYPEID = CR.SETTYPEID),'')AS TYPENAME,"
        strSql += vbCrLf + " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE "
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..STONEGROUP AS CR"
        strSql += vbCrLf + " ORDER BY "
        strSql += vbCrLf + " GROUPNAME"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                .Columns("GROUPID").Visible = False
                FormatGridColumns(gridView, False, True, True, False)
            End With
        End If
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        funcCallGrid()
        txtGroupId.Text = getgroupid(Nothing)
        txtGroupName.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If txtGroupName.Text.Trim = "" Then
            MsgBox("GroupName Should Not Empty", MsgBoxStyle.Information)
            txtGroupName.Focus()
            txtGroupName.SelectAll()
            Exit Function
        End If
        Dim cnt As Integer = 0
        If flagSave = False Then
            getvalueId()
            strSql = " SELECT COUNT(*) CNT FROM " & cnAdminDb & "..STONEGROUP WHERE  "
            strSql += vbCrLf + " CUTID = " & CutId & ""
            strSql += vbCrLf + " AND COLORID = " & Colorid & ""
            strSql += vbCrLf + " AND CLARITYID = " & ClarityId & ""
            strSql += vbCrLf + " AND SHAPEID = " & ShapeId & ""
            strSql += vbCrLf + " AND SIZEID = " & Sizeid & ""
            strSql += vbCrLf + " AND SETTYPEID = " & Typeid & ""
            cnt = GetSqlValue(cn, strSql, Nothing)
            If cnt > 0 Then
                MsgBox("Already this GroupType Available ", MsgBoxStyle.Information)
                txtGroupName.Focus()
                txtGroupName.SelectAll()
                Exit Function
            End If
            cnt = 0
            strSql = " select COUNT(*) CNT from " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & txtGroupName.Text.Trim & "' "
            cnt = GetSqlValue(cn, strSql, Nothing)
            If cnt > 0 Then
                MsgBox("Already Existing Groupname ", MsgBoxStyle.Information)
                txtGroupName.Focus()
                txtGroupName.SelectAll()
                Exit Function
            End If
            funcAdd()
            Exit Function
        Else
            funcUpdate()
            Exit Function
        End If
    End Function

    Private Sub getvalueId()
        CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'", "CUTID", 0)
        Colorid = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & cmbColor.Text & "'", "COLORID", 0)
        ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'", "CLARITYID", 0)
        ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'", "SHAPEID", 0)
        Sizeid = objGPack.GetSqlValue("SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & cmbStnSize.Text & "'", "STNSIZEID", 0)
        Typeid = objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & cmbSettingType.Text & "'", "SETTYPEID", 0)
    End Sub

    Function funcAdd() As Integer

        getvalueId()

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            Me.Cursor = Cursors.WaitCursor
            strSql = " INSERT INTO " & cnAdminDb & "..STONEGROUP"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " GROUPID,GROUPNAME"
            strSql += vbCrLf + " ,CUTID,COLORID"
            strSql += vbCrLf + " ,CLARITYID,SHAPEID"
            strSql += vbCrLf + " ,SIZEID,SETTYPEID"
            strSql += vbCrLf + " ,USERID"
            strSql += vbCrLf + " ,UPDATED,UPTIME"
            strSql += vbCrLf + " ,SYSTEMID,ACTIVE"
            strSql += vbCrLf + " ,DISPLAYORDER"
            strSql += vbCrLf + " ) VALUES ( "
            strSql += vbCrLf + " " & getgroupid(tran) & "" 'GROUPID
            strSql += vbCrLf + " ,'" & txtGroupName.Text.Trim & "'" 'GROUPNAME
            strSql += vbCrLf + " , " & CutId & "" ' CUTID
            strSql += vbCrLf + " , " & Colorid & "" 'COLORID
            strSql += vbCrLf + " , " & ClarityId & "" 'CLARITYID
            strSql += vbCrLf + " , " & ShapeId & "" 'SHAPEID
            strSql += vbCrLf + " , " & Sizeid & "" 'SIZEID
            strSql += vbCrLf + " , " & Typeid & "" 'TYPEID
            strSql += vbCrLf + " , " & userId & "" 'USERID
            strSql += vbCrLf + " , '" & Today.Date.ToString("yyyy-MM-dd") & "' " 'Updated
            strSql += vbCrLf + " , '" & Date.Now.ToLongTimeString & "' " 'Uptime
            strSql += vbCrLf + " , '" & Environment.MachineName & "' "
            strSql += vbCrLf + " , '" & IIf(cmbActive.Text = "YES", "Y", "N") & "' "
            strSql += vbCrLf + " , 0"
            strSql += " ) "
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            tran = Nothing
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function
    Function funcUpdate() As Integer

        Try
            getvalueId()
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " UPDATE " & cnAdminDb & "..STONEGROUP SET "
            strSql += vbCrLf + " UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " ,UPTIME='" & Date.Now.ToLongTimeString & "' "
            strSql += vbCrLf + " ,GROUPNAME = '" & txtGroupName.Text.Trim & "'"
            strSql += vbCrLf + " ,CUTID = '" & CutId & "' " 'CUTID
            strSql += vbCrLf + " ,COLORID = '" & Colorid & "' " 'COLORID
            strSql += vbCrLf + " ,CLARITYID = '" & ClarityId & "' " 'CLARITYID
            strSql += vbCrLf + " ,SHAPEID = '" & ShapeId & "' " 'SHAPEID
            strSql += vbCrLf + " ,SIZEID = '" & Sizeid & "' " 'SHAPEID
            strSql += vbCrLf + " ,SETTYPEID = '" & Typeid & "' " 'TYPEID
            strSql += vbCrLf + " ,USERID = '" & userId & "' " 'SHAPEID
            strSql += vbCrLf + " ,SYSTEMID = '" & Environment.MachineName & "' "
            strSql += vbCrLf + " ,ACTIVE = '" & IIf(cmbActive.Text = "YES", "Y", "N") & "' "
            strSql += vbCrLf + " WHERE GROUPID = '" & txtGroupId.Text & "' "
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            tran = Nothing
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " SELECT GROUPID,GROUPNAME"
        strSql += vbCrLf + " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = CR.CUTID)AS CUTNAME"
        strSql += vbCrLf + " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = CR.COLORID)AS COLORNAME"
        strSql += vbCrLf + " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = CR.CLARITYID)AS CLARITYNAME"
        strSql += vbCrLf + " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = CR.SHAPEID)AS SHAPENAME"
        strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..STNSIZE WHERE STNSIZEID = CR.SIZEID)AS SIZENAME"
        strSql += vbCrLf + " ,(SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = CR.SETTYPEID)AS TYPENAME"
        strSql += vbCrLf + " ,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE  "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..STONEGROUP AS CR"
        strSql += vbCrLf + " WHERE GROUPID = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtGroupId.Text = .Item("GROUPID").ToString
            txtGroupName.Text = .Item("GROUPNAME").ToString
            CmbCut.Text = .Item("CUTNAME").ToString
            cmbColor.Text = .Item("COLORNAME").ToString
            CmbClarity.Text = .Item("CLARITYNAME").ToString
            cmbShape.Text = .Item("SHAPENAME").ToString
            cmbStnSize.Text = .Item("SIZENAME").ToString
            cmbSettingType.Text = .Item("TYPENAME").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            flagSave = True
            txtGroupName.Focus()
            txtGroupName.SelectAll()
        End With
    End Function

    Private Sub frmRapaport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Function GetSqlValue(ByVal Cn As OleDb.OleDbConnection, ByVal Qry As String, ByVal tran As OleDbTransaction) As Object
        Dim Obj As Object = Nothing
        Dim Da As OleDb.OleDbDataAdapter
        Dim DtTemp As New DataTable
        cmd = New OleDbCommand(Qry, Cn, tran)
        Da = New OleDb.OleDbDataAdapter(cmd)
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Obj = DtTemp.Rows(0).Item(0)
        End If
        Return Obj
    End Function

    Private Function getgroupid(ByVal _tran As OleDbTransaction) As Integer
        strSql = " SELECT ISNULL(MAX(GROUPID),0) + 1 [COUNT] FROM " & cnAdminDb & "..STONEGROUP"
        Groupid = Val(GetSqlValue(cn, strSql, _tran).ToString)
        Return Groupid
    End Function
    Private Sub frmRapaport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()

        strSql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strSql, CmbCut)

        strSql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
        objGPack.FillCombo(strSql, cmbColor)

        strSql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strSql, CmbClarity)


        strSql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strSql, cmbShape)

        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..STNSIZE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SIZENAME"
        objGPack.FillCombo(strSql, cmbStnSize)

        strSql = " SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SETTYPENAME"
        objGPack.FillCombo(strSql, cmbSettingType)

        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"

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

    Private Sub cmbItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If MsgBox("Do You Want Delete", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.No Then
            Exit Sub
        End If
        With gridView
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                strSql = " DELETE " & cnAdminDb & "..STONEGROUP  WHERE GROUPID = '" & .CurrentRow.Cells("GROUPID").Value & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                tran.Commit()
                tran = Nothing
            Catch ex As Exception
                If Not tran Is Nothing Then
                    tran.Rollback()
                    tran = Nothing
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                Else
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                End If
            End Try
        End With
        funcCallGrid()
        funcNew()
    End Sub
End Class
