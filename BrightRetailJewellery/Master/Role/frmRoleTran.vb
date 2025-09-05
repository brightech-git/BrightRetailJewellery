Imports System.Data.OleDb
Public Class frmRoleTran
    Dim _Cmd As OleDbCommand
    Dim strSql As String
    Dim _Da As OleDbDataAdapter
    Dim _HandleFlag As Boolean = False
    Dim _DtUserRightsFrms As New DataTable

#Region "Load TreeView"
    Private Function funcGetToolStripMenuItem(ByVal tStrip As ToolStripMenuItem, ByVal parentNode As TreeNode) As Integer
        Dim childNode As TreeNode
        If tStrip.DropDownItems.Count > 0 Then
            For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
                If funcChkPrjMenus(mnu.Name) = False Then Continue For
                childNode = parentNode.Nodes.Add(mnu.Name, mnu.Text.Replace("&", ""))
                If mnu.DropDownItems.Count > 0 Then
                    funcGetToolStripMenuItem(mnu, childNode)
                Else
                    If mnu.AccessibleDescription Is Nothing Then
                        childNode.Nodes.Add("!@#$%_Add", "Add")
                        childNode.Nodes.Add("!@#$%_Edit", "Edit")
                        childNode.Nodes.Add("!@#$%_View", "View")
                        childNode.Nodes.Add("!@#$%_Del", "Del")
                    Else
                        childNode.Nodes.Add("!@#$%_Add", "Add")
                        childNode.Nodes.Add("!@#$%_Edit", "Edit")
                        childNode.Nodes.Add("!@#$%_View", "View")
                        childNode.Nodes.Add("!@#$%_Del", "Del")
                        If mnu.AccessibleDescription.ToString.Contains("~CANCEL") Then
                            childNode.Nodes.Add("!@#$%_Cancel", "Cancel")
                        End If
                        If mnu.AccessibleDescription.ToString.Contains("~RPT") Then
                            childNode.Nodes.Add("!@#$%_Excel", "Export")
                            childNode.Nodes.Add("!@#$%_Print", "Print")
                        End If
                        If mnu.AccessibleDescription.ToString.Contains("~AUT") Then
                            childNode.Nodes.Add("!@#$%_Authorize", "Authorize")
                        End If
                        If mnu.AccessibleDescription.ToString.Contains("~MISCISSUE") Then
                            childNode.Nodes.Add("!@#$%_MiscIssue", "Misc Issue")
                        End If
                        If mnu.AccessibleDescription.ToString.Contains("~APPISSUE") Then
                            childNode.Nodes.Add("!@#$%_AppIssue", "Approval Issue")
                        End If
                        'If mnu.AccessibleDescription.ToString.Contains("~GV") Then
                        '    childNode.Nodes.Add("!@#$%_GiftVoucher", "Gift Voucher")
                        'End If
                        If mnu.AccessibleDescription.ToString.Contains("~SPR") Then
                            childNode.Nodes.Add("!@#$%_Sale", "Sales")
                            childNode.Nodes.Add("!@#$%_Purchase", "Purchase")
                            childNode.Nodes.Add("!@#$%_Return", "Return")
                            childNode.Nodes.Add("!@#$%_QSale", "Quik Sales")
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
    Private Function funcChkPrjMenus(ByVal MenuId As String) As Boolean
        strSql = "SELECT 1 AS AVAIL FROM " & cnAdminDb & "..PRJMENUS WHERE MENUID='" & MenuId & "'"
        If Val(objGPack.GetSqlValue(strSql, "AVAIL", 0).ToString) = 0 Then Return False
        Return True
    End Function
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
        strSql = " SELECT ROLENAME FROM " & cnadmindb & "..ROLEMASTER WHERE ACTIVE = 'Y' ORDER BY ROLENAME"
        objGPack.FillCombo(strSql, cmbRoleName_MAN, , False)
        cmbRoleName_MAN.Text = ""
        SetCheck(tView.Nodes, False)
        tView.CollapseAll()
        cmbRoleName_MAN.Select()
    End Sub

    Private Sub frmRoleTran_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadMdiMenusNames()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub tView_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tView.AfterCheck
        If _HandleFlag = False Then
            nodeCheck(e.Node, e.Node.Checked)
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If _SyncTo <> "" Then MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information) : Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        _HandleFlag = True
        Try
            
            Dim _RoleId As Integer = objGPack.GetSqlValue("SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRoleName_MAN.Text & "'", , , tran)


            Dim dtTemp As New DataTable
            strSql = " IF OBJECT_ID('" & cnAdminDb & "..INS_ROLETRAN', 'U') IS NOT NULL DROP TABLE " & cnAdminDb & "..INS_ROLETRAN"
            strSql += vbCrLf + " SELECT * INTO " & cnAdminDb & "..INS_ROLETRAN  FROM " & cnAdminDb & "..ROLETRAN WHERE 1=2"
            _Cmd = New OleDbCommand(strSql, cn)
            _Cmd.ExecuteNonQuery()
            CheckParent(tView.Nodes)
            For Each n As TreeNode In GetCheck(tView.Nodes)
                Select Case UCase(n.Name)
                    Case "!@#$%_ADD"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _ADD = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_EDIT"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _EDIT = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_VIEW"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _VIEW = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_DEL"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _DEL = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_EXCEL"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _EXCEL = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_PRINT"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _PRINT = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_AUTHORIZE"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _AUTHORIZE = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_MISCISSUE"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _MISCISSUE = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                        'Case "!@#$%_GIFTVOUCHER"
                        '    strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _GIFTVOUCHER = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_SALE"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _SALE= 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_PURCHASE"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _PURCHASE = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_RETURN"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _RETURN = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_QSALE"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _QSALE = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case "!@#$%_CANCEL"
                        strSql = "UPDATE " & cnAdminDb & "..INS_ROLETRAN SET _CANCEL = 'Y' WHERE MENUID = '" & n.Parent.Name & "' AND ROLEID = " & _RoleId
                    Case Else
                        Dim ViewDays As Integer = 0
                        ViewDays = Val(GetSqlValue(cn, "SELECT ISNULL(VIEWDAYS,0)VIEWDAYS FROM " & cnAdminDb & "..ROLETRAN WHERE MENUID = '" & n.Name & "' AND ROLEID = " & _RoleId))
                        strSql = " INSERT INTO " & cnAdminDb & "..INS_ROLETRAN(ROLEID,MENUID,USERID,UPDATED,UPTIME,VIEWDAYS)"
                        strSql += " VALUES("
                        strSql += " " & _RoleId & ""
                        strSql += " ,'" & n.Name & "'"
                        strSql += " ," & userId & ""
                        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                        strSql += " ,'" & GetServerTime(tran) & "'"
                        strSql += " ," & ViewDays
                        strSql += " )"
                End Select
                _Cmd = New OleDbCommand(strSql, cn)
                _Cmd.ExecuteNonQuery()
                'ExecQuery(SyncMode.Master, strSql, cn, tran)
            Next
            Dim ColList As String
            ColList = GetColumnNames(cnAdminDb, "ROLETRAN", )

            strSql = " EXEC " & cnAdminDb & "..INSERTQRYGENERATOR_TABLE "
            strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_ROLETRAN',@MASK_TABLENAME = 'ROLETRAN'"
            _Cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(_Cmd)
            dtTemp = New DataTable
            da.Fill(dtTemp)
            tran = cn.BeginTransaction
            strSql = " DELETE " & cnAdminDb & "..ROLETRAN"
            strSql += " WHERE ROLEID = " & _RoleId & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            strSql = ""
            For Each ro As DataRow In dtTemp.Rows
                strSql = ro.Item(0).ToString
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            Next
            strSql = " DROP TABLE " & cnAdminDb & "..INS_ROLETRAN"
            _Cmd = New OleDbCommand(strSql, cn, tran)
            _Cmd.ExecuteNonQuery()



            

            tran.Commit()
            tran = Nothing
            SetCheck(tView.Nodes, False)
            tView.CollapseAll()
            MsgBox("Saved..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            _HandleFlag = False
        End Try
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
        For Each n As TreeNode In node
            Dim _Row() As DataRow = Nothing
            Select Case UCase(n.Name)
                Case "!@#$%_ADD"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _ADD = 'Y'")
                Case "!@#$%_EDIT"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _EDIT = 'Y'")
                Case "!@#$%_VIEW"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _VIEW = 'Y'")
                Case "!@#$%_DEL"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _DEL = 'Y'")
                Case "!@#$%_EXCEL"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _EXCEL = 'Y'")
                Case "!@#$%_PRINT"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _PRINT = 'Y'")
                Case "!@#$%_AUTHORIZE"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _AUTHORIZE = 'Y'")
                Case "!@#$%_MISCISSUE"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _MISCISSUE = 'Y'")
                    'Case "!@#$%_GIFTVOUCHER"
                    '    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _GIFTVOUCHER = 'Y'")
                Case "!@#$%_SALE"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _SALE = 'Y'")
                Case "!@#$%_PURCHASE"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _PURCHASE = 'Y'")
                Case "!@#$%_RETURN"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _RETURN = 'Y'")
                Case "!@#$%_QSALE"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _QSALE = 'Y'")

                Case "!@#$%_CANCEL"
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Parent.Name & "' AND _CANCEL = 'Y'")
                Case Else
                    _Row = _DtUserRightsFrms.Select("MENUID = '" & n.Name & "'")
            End Select
            If _Row.Length > 0 Then
                n.Checked = True
                'n.Expand()
            End If
            SetToUserRightsItem(n.Nodes)
        Next
    End Sub


    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
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
            strSql = " SELECT * FROM " & cnAdminDb & "..ROLETRAN WHERE ROLEID = "
            strSql += " (SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRoleName_MAN.Text & "')"
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
                MsgBox("RoleName should not Empty", MsgBoxStyle.Information)
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