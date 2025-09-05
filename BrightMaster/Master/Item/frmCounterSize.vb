Imports System.Data.OleDb
Public Class frmCounterSize
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim ITEMSIZE_GRPBASE As Boolean = IIf(GetAdmindbSoftValue("ITEMSIZE_GRPBASE", "N") = "Y", True, False)
    Dim TAGNO_SIZEBASED As Boolean = IIf(GetAdmindbSoftValue("TAGNO_SIZEBASED", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " select "
        strSql += " SizeId,SizeName,"
        If ITEMSIZE_GRPBASE Then
            strSql += " (select top 1 GroupName from " & cnAdminDb & "..ItemGroupmast where Groupid=(select top 1 ItemGroup from " & cnAdminDb & "..ItemMast as m where m.ItemId = i.ItemId))as ItemName,"
        Else
            strSql += " (select ItemName from " & cnAdminDb & "..ItemMast as m where m.ItemId = i.ItemId)as ItemName,"
        End If
        strSql += " ReOrdPcs,ReOrdWt"
        If TAGNO_SIZEBASED Then
            strSql += " ,ShortName,TagNo"
        End If
        strSql += " from " & cnAdminDb & "..ItemSize as i"
        funcOpenGrid(strSql, gridView)
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function
    Function funcLoadItemName() As Integer
        Dim dt As New DataTable
        dt.Clear()
        cmbItemName_Man.Items.Clear()
        If ITEMSIZE_GRPBASE Then
            strSql = " Select GroupName as ItemName from " & cnAdminDb & "..ItemGroupmast "
            strSql += " Order by GroupName"
        Else
            strSql = " Select ItemName from " & cnAdminDb & "..ItemMast "
            strSql += " Where SizeStock = 'Y'"
            strSql += " Order by ItemName"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer
        For cnt = 0 To dt.Rows.Count - 1
            cmbItemName_Man.Items.Add(dt.Rows(cnt).Item("ItemName"))
        Next
        cmbItemName_Man.Text = dt.Rows(0).Item("ItemName")
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        funcCallGrid()
        funcLoadItemName()
        txtSizeId_Num_Man.Text = objGPack.GetMax("SizeId", "ItemSize", cnAdminDb)
        If ITEMSIZE_GRPBASE Then lblItem.Text = "Item Group"
        cmbItemName_Man.Select()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim itemId As Integer
        Dim SizeId As Integer = Val(txtSizeId_Num_Man.Text)
        Dim dt As New DataTable
        If ITEMSIZE_GRPBASE Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMGROUP"
            strSql += " IN (SELECT GROUPID FROM  " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME='" & cmbItemName_Man.Text & "')"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(cnt)
                        itemId = Val(.Item("ITEMID").ToString)
                        strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMSIZE WHERE "
                        strSql += " ITEMID = " & itemId
                        strSql += " AND SIZENAME = '" & txtSizeName__Man.Text & "'"
                        If flagSave = True Then
                            strSql += " AND SIZEID <> '" & Val(txtSizeId_Num_Man.Text) & "'"
                        Else
                            strSql += " AND SIZEID <> '" & SizeId & "'"
                        End If
                        If objGPack.DupChecker(txtSizeName__Man, strSql) Then Continue For
                        If flagSave = False Then
                            funcAdd(itemId, SizeId)
                        ElseIf flagSave = True Then
                            funcUpdate(itemId)
                        End If
                        SizeId += 1
                    End With
                Next
            End If
        Else
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMSIZE WHERE "
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            strSql += " AND SIZENAME = '" & txtSizeName__Man.Text & "'"
            strSql += " AND SIZEID <> '" & txtSizeId_Num_Man.Text & "'"
            If objGPack.DupChecker(txtSizeName__Man, strSql) Then Exit Function
            strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                itemId = dt.Rows(0).Item("ItemId")
            End If
            If flagSave = False Then
                funcAdd(itemId, SizeId)
            ElseIf flagSave = True Then
                funcUpdate(itemId)
            End If
        End If
        funcNew()
    End Function
    Function funcAdd(ByVal Itemid As Integer, ByVal SizeId As Integer) As Integer

        strSql = " insert into " & cnAdminDb & "..ItemSize"
        strSql += " ("
        strSql += " ItemId,SizeId,SizeName,ReOrdPcs,"
        strSql += " ReOrdWt,UserId,Updated,Uptime,TagNo,ShortName"
        strSql += " )Values("
        strSql += " " & Itemid & "" 'ItemId
        strSql += " ," & Val(SizeId) & "" 'SizeId
        strSql += " ,'" & txtSizeName__Man.Text & "'" 'SizeName
        strSql += " ," & Val(txtReorderPieces_Num.Text) & "" 'ReOrdPcs
        strSql += " ," & Val(txtReorderWt_Wet.Text) & "" 'ReOrdWt
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        'If TAGNO_SIZEBASED Then
        strSql += " ,'" & txtTagNo.Text & "'" 'TagNo
        strSql += " ,'" & txtShortName.Text & "'" 'ShortName
        'End If
        strSql += ")"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate(ByVal Itemid As Integer) As Integer
        strSql = " Update " & cnAdminDb & "..ItemSize Set"
        strSql += " ItemId=" & Itemid & ""
        strSql += " ,SizeName='" & txtSizeName__Man.Text & "'"
        strSql += " ,ReOrdPcs=" & Val(txtReorderPieces_Num.Text) & ""
        strSql += " ,ReOrdWt=" & Val(txtReorderWt_Wet.Text) & ""
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        If TAGNO_SIZEBASED Then
            strSql += " ,TagNo='" & txtTagNo.Text & "'"
            strSql += " ,ShortName='" & txtShortName.Text & "'"
        End If
        strSql += " Where SizeId = '" & txtSizeId_Num_Man.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGetDetails(ByVal tempSizeId As Integer) As Integer
        strSql = " SELECT "
        strSql += " SIZEID,SIZENAME,"
        If ITEMSIZE_GRPBASE Then
            strSql += " (SELECT TOP 1 GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID=(SELECT TOP 1 ITEMGROUP FROM " & cnAdminDb & "..ITEMMAST AS M WHERE M.ITEMID = I.ITEMID))AS ITEMNAME,"
        Else
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS M WHERE M.ITEMID = I.ITEMID)AS ITEMNAME,"
        End If
        strSql += " REORDPCS,REORDWT"
        strSql += " ,SHORTNAME,TAGNO"
        strSql += " FROM " & cnAdminDb & "..ITEMSIZE AS I"
        strSql += " WHERE SIZEID = '" & tempSizeId & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ITEMNAME")
            txtSizeId_Num_Man.Text = .Item("SIZEID")
            txtSizeName__Man.Text = .Item("SIZENAME")
            txtReorderPieces_Num.Text = .Item("REORDPCS")
            txtReorderWt_Wet.Text = .Item("REORDWT")    
            txtShortName.Text = .Item("SHORTNAME").ToString
            txtTagNo.Text = .Item("TAGNO").ToString
        End With
        flagSave = True
    End Function

    Private Sub frmCounterSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmCounterSize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtSizeId_Num_Man.Enabled = False
        If TAGNO_SIZEBASED Then pnlTag.Visible = True
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
                txtSizeName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtSizeName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSizeName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMSIZE WHERE "
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            strSql += " AND SIZENAME = '" & txtSizeName__Man.Text & "'"
            strSql += " AND SIZEID <> '" & txtSizeId_Num_Man.Text & "'"
            If objGPack.DupChecker(txtSizeName__Man, strSql) Then Exit Sub
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SIZEID").Value.ToString

        chkQry = " SELECT TOP 1 SIZEID FROM " & cnAdminDb & "..ITEMTAG WHERE SIZEID = '" & delKey & "' "
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 SIZEID FROM " & cnAdminDb & "..ORMAST WHERE SIZEID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = '" & delKey & "'")
        funcCallGrid()
    End Sub
 
End Class