Imports System.Data.OleDb
Public Class ExeExtender
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim _HandleFlag As Boolean = False
    Dim DtPrjMenus As New DataTable
    Dim dt As New DataTable

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        StrSql = " SELECT * FROM " & GlobalVariables.cnAdminDb & "..ACCENTRYMASTER ORDER BY DISPLAYORDER,CAPTION"
        Dim dtAccMenus As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtAccMenus)
        For Each ro As DataRow In dtAccMenus.Rows
            Dim tStrip As New ToolStripMenuItem
            tStrip.Name = "TSTRIPACC" & ro!SNO.ToString
            tStrip.Text = ro!CAPTION.ToString
            tStrip.AccessibleDescription = ro!SNO.ToString & "frmAccountsEnt~OWN"
            tStrip.Size = New System.Drawing.Size(140, 22)
            tStrip.ShortcutKeyDisplayString = "AGA-9-" & ro!SNO.ToString
            Main.tStripAccountsEnt.DropDownItems.Add(tStrip)
        Next

        If Val(GetSqlValue(cn, "SELECT 1 FROM " & GlobalVariables.cnAdminDb & "..SYSOBJECTS WHERE XTYPE ='U' AND NAME='OTHERMASTERENTRY'")) > 0 Then
            StrSql = " SELECT * FROM " & GlobalVariables.cnAdminDb & "..OTHERMASTERENTRY ORDER BY MISCID"
            Dim dtOthMasterMenus As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtOthMasterMenus)
            For Each ro As DataRow In dtOthMasterMenus.Rows
                Dim tStrip As New ToolStripMenuItem
                tStrip.Name = "TSTRIPOTHMASTER" & ro!MISCID.ToString
                tStrip.Text = ro!MISCNAME.ToString
                tStrip.AccessibleDescription = ro!MISCID.ToString & "frmOtherMaster"
                tStrip.Size = New System.Drawing.Size(140, 22)
                tStrip.ShortcutKeyDisplayString = "AGM-2-31-2-" & ro!MISCID.ToString
                Main.tStripOtherMaster.DropDownItems.Add(tStrip)
            Next
        End If


        ' Add any initialization after the InitializeComponent() call.
        StrSql = " SELECT * FROM " & GlobalVariables.cnAdminDb & "..PRJMENUS"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtPrjMenus)
        _HandleFlag = True
        LoadMdiMenusNames()
        tView.CollapseAll()
        _HandleFlag = False
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        _HandleFlag = True
        Try
            tran = cn.BeginTransaction
            StrSql = " DELETE " & cnAdminDb & "..PRJMENUS"
            Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
            CheckParent(tView.Nodes)
            For Each n As TreeNode In GetCheck(tView.Nodes)
                If n.Checked = False Then Continue For
                Dim objNodeInfo As New NodeInfo
                If TypeOf n.Tag Is NodeInfo Then
                    objNodeInfo = n.Tag
                    StrSql = " INSERT INTO " & cnAdminDb & "..PRJMENUS(MENUID,MENUTEXT,CHILD,ACCESSID,MODULEID)"
                    StrSql += " VALUES"
                    StrSql += " ("
                    StrSql += " '" & n.Name & "','" & objNodeInfo.pMnuText & "'"
                    StrSql += " ,'" & IIf(objNodeInfo.pChild, "Y", "") & "'"
                    StrSql += " ,'" & objNodeInfo.pAcessId & "'"
                    StrSql += " ,'" & objNodeInfo.pModuleId & "'"
                    StrSql += " )"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                    StrSql = " UPDATE " & cnAdminDb & "..MENUMASTER SET ACCESSKEY1='" & objNodeInfo.pModuleId & "'"
                    StrSql += "  WHERE MENUID='" & n.Name & "'"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
            Next
            StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='REP_NEWSALESCOMM'"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            dt = New DataTable
            Da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("CTLTEXT").ToString = "Y" Then
                    StrSql = "UPDATE " & cnAdminDb & "..PRJMENUS SET ACCESSID='frmSalesCommission~RPT' WHERE MENUID='tStripSalesPersonCommision'"
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    Cmd.ExecuteNonQuery()
                End If
            End If
            SetCheck(tView.Nodes, False)
            tView.CollapseAll()
            tran.Commit()
            tran = Nothing
            btnExit_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            _HandleFlag = False
        End Try
    End Sub

    Private Sub LoadMdiMenusNames()
        tView.Nodes.Clear()
        Dim parentNode As TreeNode
        Try
            For Each mnu As ToolStripMenuItem In Main.MenuStrip1.Items
                Dim str As String = Nothing
                str = mnu.Text
                str = str.Replace("&", "")
                parentNode = tView.Nodes.Add(mnu.Name, mnu.Text.Replace("&", ""))

                Dim Ro() As DataRow = DtPrjMenus.Select("MENUID = '" & mnu.Name & "'")
                If Ro.Length > 0 Then
                    parentNode.Checked = True
                Else
                    parentNode.Checked = False
                End If

                Dim ObjNodeInfo As New NodeInfo
                ObjNodeInfo.pMnuText = mnu.Text.Replace("&", "")
                ObjNodeInfo.pAcessId = mnu.AccessibleDescription
                ObjNodeInfo.pModuleId = mnu.ShortcutKeyDisplayString
                ObjNodeInfo.pChild = IIf(mnu.DropDownItems.Count > 0, False, True)
                parentNode.Tag = ObjNodeInfo
                If mnu.DropDownItems.Count > 0 Then
                    funcGetToolStripMenuItem(mnu, parentNode)
                End If
            Next
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + " StackTrack :" + ex.StackTrace)
        End Try
    End Sub

    Private Function funcGetToolStripMenuItem(ByVal tStrip As ToolStripMenuItem, ByVal parentNode As TreeNode) As Integer
        Dim childNode As TreeNode
        If tStrip.DropDownItems.Count > 0 Then
            For Each mnu As ToolStripMenuItem In tStrip.DropDownItems
                childNode = parentNode.Nodes.Add(mnu.Name, mnu.Text.Replace("&", ""))
                Dim Ro() As DataRow = DtPrjMenus.Select("MENUID = '" & mnu.Name & "'")
                If Ro.Length > 0 Then
                    childNode.Checked = True
                Else
                    childNode.Checked = False
                End If
                Dim ObjNodeInfo As New NodeInfo
                ObjNodeInfo.pMnuText = mnu.Text.Replace("&", "")
                ObjNodeInfo.pAcessId = mnu.AccessibleDescription
                ObjNodeInfo.pModuleId = mnu.ShortcutKeyDisplayString
                ObjNodeInfo.pChild = IIf(mnu.DropDownItems.Count > 0, False, True)
                childNode.Tag = ObjNodeInfo
                If mnu.DropDownItems.Count > 0 Then
                    funcGetToolStripMenuItem(mnu, childNode)
                End If
            Next
        End If
    End Function

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

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub tView_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tView.AfterCheck
        If _HandleFlag = False Then
            nodeCheck(e.Node, e.Node.Checked)
        End If
    End Sub
    Private Sub ExpandAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandAllToolStripMenuItem.Click
        '_HandleFlag = True
        'CheckParent(tView.Nodes)
        '_HandleFlag = False
        tView.ExpandAll()
    End Sub

    Private Sub CollapseAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseAllToolStripMenuItem.Click
        '_HandleFlag = True
        'CheckParent(tView.Nodes)
        '_HandleFlag = False
        tView.CollapseAll()
    End Sub
End Class

Public Class NodeInfo
    Private MnuText As String = ""
    Public Property pMnuText() As String
        Get
            Return MnuText
        End Get
        Set(ByVal value As String)
            MnuText = value
        End Set
    End Property
    Private AccessId As String = ""
    Public Property pAcessId() As String
        Get
            Return AccessId
        End Get
        Set(ByVal value As String)
            AccessId = value
        End Set
    End Property
    Private ModuleId As String = ""
    Public Property pModuleId() As String
        Get
            Return ModuleId
        End Get
        Set(ByVal value As String)
            ModuleId = value
        End Set
    End Property
    Private Child As Boolean = False
    Public Property pChild() As Boolean
        Get
            Return Child
        End Get
        Set(ByVal value As Boolean)
            Child = value
        End Set
    End Property
End Class
