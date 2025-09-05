Imports System.Data.OleDb
Imports System.Collections
Imports System.Text.RegularExpressions

Public Class frmAlertTran
    Dim _Cmd As OleDbCommand
    Dim strSql As String
    Dim _Da As OleDbDataAdapter
    Dim _HandleFlag As Boolean = False
    Dim _DtUserRightsFrms As New DataTable

#Region "Load TreeView"
    Private Function LoadTablesNames()
        tView.Nodes.Clear()
        Dim parentNode As TreeNode
        Dim childNode As TreeNode
        Dim dttables As New DataTable
        strSql = "select NAME from " & cnAdminDb & "..sysobjects where xtype ='U' and name not like 'Temp%' ORDER BY NAME "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dttables)
        If dttables.Rows.Count > 0 Then
            parentNode = tView.Nodes.Add("MASTER", "MASTER")
            For ij As Integer = 0 To 3
                If ij = 0 Then childNode = parentNode.Nodes.Add("!@#$%_Add", "Add")
                If ij = 1 Then childNode = parentNode.Nodes.Add("!@#$%_Edit", "Edit")
                If ij = 2 Then childNode = parentNode.Nodes.Add("!@#$%_Del", "Del")
                If ij = 3 Then childNode = parentNode.Nodes.Add("!@#$%_Cancel", "Cancel")
                ' childNode = parentNode.Nodes.Add(dttables.Rows(i'i).Item("Name"), dttables.Rows(ii).Item("Name"))
                For ii As Integer = 0 To dttables.Rows.Count - 1
                    childNode.Nodes.Add(dttables.Rows(ii).Item("Name"), dttables.Rows(ii).Item("Name"))
                Next

            Next ij
        End If
        dttables.Clear()
        strSql = "select NAME from " & cnStockDb & "..sysobjects where xtype ='U' and name not like 'Temp%' ORDER BY NAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dttables)
        If dttables.Rows.Count > 0 Then
            parentNode = tView.Nodes.Add("TRANSACTION", "TRANSACTION")
            For ij As Integer = 0 To 3
                If ij = 0 Then childNode = parentNode.Nodes.Add("!@#$%_Add", "Add")
                If ij = 1 Then childNode = parentNode.Nodes.Add("!@#$%_Edit", "Edit")
                If ij = 2 Then childNode = parentNode.Nodes.Add("!@#$%_Del", "Del")
                If ij = 3 Then childNode = parentNode.Nodes.Add("!@#$%_Cancel", "Cancel")
                ' childNode = parentNode.Nodes.Add(dttables.Rows(i'i).Item("Name"), dttables.Rows(ii).Item("Name"))
                For ii As Integer = 0 To dttables.Rows.Count - 1
                    childNode.Nodes.Add(dttables.Rows(ii).Item("Name"), dttables.Rows(ii).Item("Name"))
                Next
            Next ij
        End If
    End Function


    Private Function funcGetToolStripMenuItem(ByVal tStrip As ToolStripMenuItem, ByVal parentNode As TreeNode) As Integer
        Dim childNode As TreeNode
        If tStrip.DropDownItems.Count > 0 Then
            For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
                childNode = parentNode.Nodes.Add(mnu.Name, mnu.Text.Replace("&", ""))
                If mnu.DropDownItems.Count > 0 Then
                    funcGetToolStripMenuItem(mnu, childNode)
                Else
                    If mnu.AccessibleDescription Is Nothing Then
                        childNode.Nodes.Add("!@#$%_Add", "Add")
                        childNode.Nodes.Add("!@#$%_Edit", "Edit")
                        'If mnu.AccessibleDescription.ToString.Contains("~PRINT") Then
                        ' childNode.Nodes.Add("!@#$%_Print", "Duplicate")
                        ' End If
                        childNode.Nodes.Add("!@#$%_Del", "Del")
                    Else
                        childNode.Nodes.Add("!@#$%_Add", "Add")
                        childNode.Nodes.Add("!@#$%_Edit", "Edit")
                        childNode.Nodes.Add("!@#$%_Del", "Del")
                        If mnu.AccessibleDescription.ToString.Contains("~CANCEL") Then
                            childNode.Nodes.Add("!@#$%_Cancel", "Cancel")
                        End If
                        If mnu.AccessibleDescription.ToString.Contains("~PRINT") Then
                            childNode.Nodes.Add("!@#$%_Print", "Duplicate")
                        End If
                    End If
                End If
            Next
        End If
    End Function
    Private Sub LoadMdiMenusNames()
        tView.Nodes.Clear()
        Dim parentNode As TreeNode
        Try
            For Each mnu As ToolStripMenuItem In Main.MenuStrip1.Items
                If Not mnu.Visible Then Continue For
                Dim str As String = Nothing
                str = mnu.Text
                str = str.Replace("&", "")
                parentNode = tView.Nodes.Add(mnu.Name, mnu.Text.Replace("&", ""))
                If mnu.DropDownItems.Count > 0 Then
                    funcGetToolStripMenuItem(mnu, parentNode)
                End If
            Next
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + " StackTrack :" + ex.StackTrace)
        End Try
    End Sub
#End Region

    Private Function nodeCheck(ByVal node As TreeNode, ByVal state As Boolean) As Integer
        For Each n As TreeNode In node.Nodes
            n.Checked = state
        Next
    End Function
    Private Function GetCheck(ByVal node As TreeNodeCollection) As List(Of TreeNode)
        Dim lN As New List(Of TreeNode)
        For Each n As TreeNode In node
            If n.Checked Then lN.Add(n)
            lN.AddRange(GetCheck(n.Nodes))
        Next
        Return lN
    End Function


    Private Function SetCheck(ByVal node As TreeNodeCollection, ByVal State As Boolean) As Integer
        For Each n As TreeNode In node
            n.Checked = State
            SetCheck(n.Nodes, State)
        Next
    End Function

    Private Sub CheckParent1(ByVal node As TreeNode)
        If Not node.Parent Is Nothing Then
            node.Checked = True
            CheckParent1(node.Parent)
        Else
            node.Checked = True
        End If
    End Sub

    Private Sub CheckParent(ByVal node As TreeNodeCollection)
        For Each n As TreeNode In node
            If n.Checked Then CheckParent1(n)
            CheckParent(n.Nodes)
        Next
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        strSql = " SELECT GROUPNAME FROM " & cnAdminDb & "..ALERTGROUP WHERE ACTIVE = 'Y' ORDER BY GROUPNAME"
        objGPack.FillCombo(strSql, cmbRoleName_MAN, , False)
        cmbRoleName_MAN.Text = ""
        SetCheck(tView.Nodes, False)
        tView.CollapseAll()
        cmbRoleName_MAN.Select()
    End Sub

    Private Sub frmALERTTRAN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ALERTBASE'", , "M", tran).ToUpper = "M" Then
            LoadMdiMenusNames()
        Else
            LoadTablesNames()
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub tView_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tView.AfterCheck
        If _HandleFlag = False Then
            nodeCheck(e.Node, e.Node.Checked)
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        _HandleFlag = True
        Try
            tran = cn.BeginTransaction
            Dim UPDA As Boolean = False
            Dim TNAME As String
            Dim TNAME1 As String
            Dim treeName As String
            Dim k As Integer
            Dim qry As String
            Dim childName As New ArrayList()
            Dim _RoleId As Integer = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..ALERTGROUP WHERE GROUPNAME = '" & cmbRoleName_MAN.Text & "'", , , tran)
            strSql = " DELETE " & cnAdminDb & "..ALERTTRAN"
            strSql += " WHERE GROUPID = " & _RoleId & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            treeName = tView.Nodes.Item(0).Text

            If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ALERTBASE'", , "M", tran).ToUpper = "M" Then
                CheckParent(tView.Nodes)
                For Each n As TreeNode In GetCheck(tView.Nodes)
                    'If n.Checked = False Then
                    Dim treview As String = tView.Nodes.Item(0).Text
                    If TNAME <> treview Then
                        Select Case UCase(n.Name)
                            Case "!@#$%_ADD"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _ADD = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_EDIT"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _EDIT = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_VIEW"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _VIEW = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_DEL"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DELETE = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_PRINT"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DUPL = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_CANCEL"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _CANCEL = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case Else
                                strSql = " INSERT INTO " & cnAdminDb & "..ALERTTRAN(GROUPID,MENUID,USERID,UPDATED,UPTIME)"
                                strSql += " VALUES("
                                strSql += " " & _RoleId & ""
                                strSql += " ,'" & n.Name & "'"
                                strSql += " ," & userId & ""
                                strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                                strSql += " ,'" & GetServerTime(tran) & "'"
                                strSql += " )"
                        End Select
                    Else
                        Select Case UCase(n.Name)
                            Case "!@#$%_ADD"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _ADD = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_EDIT"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _EDIT = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_VIEW"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _VIEW = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_DEL"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DELETE = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_PRINT"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DUPL = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                            Case "!@#$%_CANCEL"
                                strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _CANCEL = 'Y' WHERE MENUID = '" & n.Parent.Name & "'"
                        End Select
                    End If
                    ExecQuery(SyncMode.Master, strSql, cn, tran)
                Next
            Else

                CheckParent(tView.Nodes)
                For Each n As TreeNode In GetCheck(tView.Nodes)
                    Dim mname As String = ""
                    If n.Parent Is Nothing Then GoTo nenxt
                    'mname = n.Name
                    'Else
                    mname = n.Parent.Name
                    Dim int As Integer
                    'childName.Clear()
                    For int = 0 To n.Nodes.Count - 1
                        If n.Nodes.Item(int).Checked = True Then
                            childName.Add(n.Nodes.Item(int).Text.ToString)
                        End If
                    Next
                    Dim strSql = "Select TableName from  " & cnAdminDb & "..ALERTTRAN where TableName='" & treeName & "'"
                    _Cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(_Cmd)
                    Dim dt As New DataTable
                    da.Fill(dt)
                    Dim i As Integer
                    Dim j As Integer
                    If dt.Rows.Count > 0 Then
                        For i = 0 To dt.Rows.Count - 1
                            TNAME = dt.Rows(i).Item("TableName").ToString
                        Next
                    End If

                    'Next
                    If TNAME = treeName Then
                        'For k = 0 To n.Nodes.Count - 1
                        For k = 0 To childName.Count - 1
                            'If n.Nodes.Item().Checked = True Then
                            qry = "Select TableName from  " & cnAdminDb & "..ALERTTRAN where TableName='" & childName(k) & "'"
                            Dim cmd As OleDbCommand
                            Dim dts As New DataTable
                            cmd = New OleDbCommand(qry, cn, tran)
                            Dim dap As OleDbDataAdapter
                            dap = New OleDbDataAdapter(cmd)
                            dap.Fill(dts)
                            If dts.Rows.Count > 0 Then
                                TNAME1 = dts.Rows(0).Item("TableName").ToString
                            End If

                            If TNAME1 <> childName(k) Then
                                'If n.Nodes.Item(k).Checked = True Then
                                strSql = " INSERT INTO " & cnAdminDb & "..ALERTTRAN(GROUPID,_ADD,TABLENAME,USERID,UPDATED,UPTIME)"
                                strSql += " VALUES("
                                strSql += " " & _RoleId & ""
                                strSql += ",'Y'"
                                strSql += " ,'" & childName(k) & "'"
                                strSql += " ," & userId & ""
                                strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                                strSql += " ,'" & GetServerTime(tran) & "'"
                                strSql += " )"
                                ExecQuery(SyncMode.Master, strSql, cn, tran)
                                'End If
                            Else
                            'Dim nCount As Integer
                                For Each child As TreeNode In GetCheck(tView.Nodes)
                                    'child.Checked = True
                                    'If child.Nodes.Count > 0 Then CheckChildNodes(child, checked)
                                    Dim c As Integer
                                    For c = 0 To child.Nodes.Count - 1
                                        Select Case UCase(mname)
                                            Case "!@#$%_ADD"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql = " UPDATE " & cnAdminDb & "..ALERTTRAN SET _ADD = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case "!@#$%_EDIT"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql += " UPDATE " & cnAdminDb & "..ALERTTRAN SET _EDIT = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case "!@#$%_VIEW"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql += " UPDATE " & cnAdminDb & "..ALERTTRAN SET _VIEW = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case "!@#$%_DEL"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql += " UPDATE " & cnAdminDb & "..ALERTTRAN SET _DELETE = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case "!@#$%_PRINT"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql += " UPDATE " & cnAdminDb & "..ALERTTRAN SET _DUPL = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case "!@#$%_CANCEL"
                                                If child.Name = mname Then
                                                    If child.Nodes.Item(c).Checked = True Then
                                                        strSql += " UPDATE " & cnAdminDb & "..ALERTTRAN SET _CANCEL = 'Y' WHERE TABLENAME = '" & child.Nodes.Item(c).Text & "'"
                                                        UPDA = True
                                                    End If
                                                End If
                                            Case Else
                                        End Select
                                    Next
                                Next
                                If UPDA Then ExecQuery(SyncMode.Master, strSql, cn, tran)
                            End If
                Next
                    Else
                        strSql = " INSERT INTO " & cnAdminDb & "..ALERTTRAN(GROUPID,TABLENAME,USERID,UPDATED,UPTIME)"
                        strSql += " VALUES("
                        strSql += " " & _RoleId & ""
                        strSql += " ,'" & treeName & "'"
                        strSql += " ," & userId & ""
                        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                        strSql += " ,'" & GetServerTime(tran) & "'"
                        strSql += " )"
                        ExecQuery(SyncMode.Master, strSql, cn, tran)
                    End If
                    'If UPDA Then ExecQuery(SyncMode.Master, strSql, cn, tran)
nenxt:
                Next
            End If
            SetCheck(tView.Nodes, False)
            tView.CollapseAll()
            tran.Commit()
            tran = Nothing
            MsgBox("Saved..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            _HandleFlag = False
        End Try

        'If UCase(n.Name) <> "!@#$%_ADD" And UCase(n.Name) <> "!@#$%_EDIT" And UCase(n.Name) <> "!@#$%_DEL" And UCase(n.Name) <> "!@#$%_VIEW" And UCase(n.Name) <> "!@#$%_CANCEL" And UCase(n.Name) <> "!@#$%_PRINT" Then
        '    strSql = " INSERT INTO " & cnAdminDb & "..ALERTTRAN(GROUPID,TABLENAME,USERID,UPDATED,UPTIME)"
        '    strSql += " VALUES("
        '    strSql += " " & _RoleId & ""
        '    strSql += " ,'" & n.Name & "'"
        '    strSql += " ," & userId & ""
        '    strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
        '    strSql += " ,'" & GetServerTime(tran) & "'"
        '    strSql += " )"
        '    ExecQuery(SyncMode.Master, strSql, cn, tran)
        'End If
        ' If UNAME <> n.Text Then
        'If UCase(mname) <> "!@#$%_ADD" And UCase(mname) <> "!@#$%_EDIT" And UCase(mname) <> "!@#$%_DEL" And UCase(mname) <> "!@#$%_VIEW" And UCase(mname) <> "!@#$%_CANCEL" And UCase(mname) <> "!@#$%_PRINT" Then
        '    strSql = " INSERT INTO " & cnAdminDb & "..ALERTTRAN(GROUPID,TABLENAME,USERID,UPDATED,UPTIME)"
        '    strSql += " VALUES("
        '    strSql += " " & _RoleId & ""
        '    strSql += " ,'" & mname & "'"
        '    strSql += " ," & userId & ""
        '    strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
        '    strSql += " ,'" & GetServerTime(tran) & "'"
        '    strSql += " )"
        '    ExecQuery(SyncMode.Master, strSql, cn, tran)
        'End If
        'Select Case UCase(mname)
        '    Case "!@#$%_ADD"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _ADD = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case "!@#$%_EDIT"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _EDIT = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case "!@#$%_VIEW"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _VIEW = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case "!@#$%_DEL"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DELETE = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case "!@#$%_PRINT"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DUPL = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case "!@#$%_CANCEL"
        '        strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _CANCEL = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '        UPDA = True
        '    Case Else

        'End Select

        'Dim treview As String = tView.Nodes.Item(0).Text
        'End If

        'Else
        '    Select Case UCase(mname)
        '        Case "!@#$%_ADD"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _ADD = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case "!@#$%_EDIT"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _EDIT = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case "!@#$%_VIEW"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _VIEW = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case "!@#$%_DEL"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DELETE = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case "!@#$%_PRINT"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _DUPL = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case "!@#$%_CANCEL"
        '            strSql = "UPDATE " & cnAdminDb & "..ALERTTRAN SET _CANCEL = 'Y' WHERE TABLENAME = '" & n.Name & "'"
        '            UPDA = True
        '        Case Else
        '    End Select                        
        'End If
    End Sub

    Private Sub ExpandAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandAllToolStripMenuItem.Click
        _HandleFlag = True
        CheckParent(tView.Nodes)
        _HandleFlag = False
        tView.ExpandAll()
    End Sub

    Private Sub CollapseAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseAllToolStripMenuItem.Click
        _HandleFlag = True
        CheckParent(tView.Nodes)
        _HandleFlag = False
        tView.CollapseAll()
    End Sub

    Private Sub SetToUserRightsItem(ByVal node As TreeNodeCollection)
        Dim mchkField As String
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ALERTBASE'", , "M", tran).ToUpper = "M" Then
            mchkField = "MENUID"
            For Each n As TreeNode In node
                Dim _Row() As DataRow = Nothing
                Select Case UCase(n.Name)
                    Case "!@#$%_ADD"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & n.Parent.Name & "' AND _ADD = 'Y'")
                    Case "!@#$%_EDIT"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & n.Parent.Name & "' AND _EDIT = 'Y'")
                    Case "!@#$%_DEL"
                        _Row = _DtUserRightsFrms.Select(mchkField & "= '" & n.Parent.Name & "' AND _DELETE = 'Y'")
                    Case "!@#$%_PRINT"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & n.Parent.Name & "' AND _DUPL = 'Y'")
                    Case "!@#$%_CANCEL"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & n.Parent.Name & "' AND _CANCEL = 'Y'")
                    Case Else
                        _Row = _DtUserRightsFrms.Select(mchkField & "= '" & n.Name & "'")
                End Select
                If _Row.Length > 0 Then
                    n.Checked = True
                    'n.Expand()
                End If
                SetToUserRightsItem(n.Nodes)
            Next
        Else
            mchkField = "TABLENAME"
            Dim mname As String, mpname As String

            For Each n As TreeNode In node

                If (n.Name = "MASTER" Or n.Name = "TRANSACTION") And n.Parent Is Nothing Then
                    mpname = n.Name
                Else
                    mpname = n.Parent.Name
                End If
                mname = UCase(n.Name)
                Dim _Row() As DataRow = Nothing
                Select Case UCase(mpname)
                    Case "!@#$%_ADD"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mname & "' AND _ADD = 'Y'")
                    Case "!@#$%_EDIT"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mname & "' AND _EDIT = 'Y'")
                    Case "!@#$%_DEL"
                        _Row = _DtUserRightsFrms.Select(mchkField & "= '" & mname & "' AND _DELETE = 'Y'")
                    Case "!@#$%_PRINT"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mname & "' AND _DUPL = 'Y'")
                    Case "!@#$%_CANCEL"
                        _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mname & "' AND _CANCEL = 'Y'")
                    Case Else
                        Select Case UCase(mname)
                            Case "!@#$%_ADD"
                                _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mpname & "' AND _ADD = 'Y'")
                            Case "!@#$%_EDIT"
                                _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mpname & "' AND _EDIT = 'Y'")
                            Case "!@#$%_DEL"
                                _Row = _DtUserRightsFrms.Select(mchkField & "= '" & mpname & "' AND _DELETE = 'Y'")
                            Case "!@#$%_PRINT"
                                _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mpname & "' AND _DUPL = 'Y'")
                            Case "!@#$%_CANCEL"
                                _Row = _DtUserRightsFrms.Select(mchkField & " = '" & mpname & "' AND _CANCEL = 'Y'")
                            Case Else
                                _Row = _DtUserRightsFrms.Select(mchkField & "= '" & mpname & "'")
                        End Select
                End Select
                        If _Row.Length > 0 Then
                            n.Checked = True
                            'n.Expand()
                        End If

                        SetToUserRightsItem(n.Nodes)
            Next
        End If
        
    End Sub


    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If cmbRoleName_MAN.Text = "" Then
            MsgBox("RoleName should not Empty", MsgBoxStyle.Information)
            cmbRoleName_MAN.Focus()
            Exit Sub
        End If
        If Not cmbRoleName_MAN.Items.Contains(cmbRoleName_MAN.Text) Then
            MsgBox("Invalid RoleName", MsgBoxStyle.Information)
            cmbRoleName_MAN.Focus()
            Exit Sub
        End If
        If cmbRoleName_MAN.Text <> "" Then
            _DtUserRightsFrms = New DataTable
            SetCheck(tView.Nodes, False)
            tView.CollapseAll()
            _HandleFlag = True
            strSql = " SELECT * FROM " & cnAdminDb & "..ALERTTRAN WHERE GROUPID = "
            strSql += " (SELECT GROUPID FROM " & cnAdminDb & "..ALERTGROUP WHERE GROUPNAME = '" & cmbRoleName_MAN.Text & "')"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(_DtUserRightsFrms)
            If _DtUserRightsFrms.Rows.Count > 0 Then
                SetToUserRightsItem(tView.Nodes)
            End If
            _HandleFlag = False
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbRoleName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbRoleName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbRoleName_MAN.Text = "" Then
                MsgBox("GroupRoleName should not Empty", MsgBoxStyle.Information)
                cmbRoleName_MAN.Focus()
                Exit Sub
            End If
            If Not cmbRoleName_MAN.Items.Contains(cmbRoleName_MAN.Text) Then
                MsgBox("Invalid RoleName", MsgBoxStyle.Information)
                cmbRoleName_MAN.Focus()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub




End Class