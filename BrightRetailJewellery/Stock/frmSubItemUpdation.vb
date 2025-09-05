Imports System.Data.OleDb
Public Class frmSubItemUpdation
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim dt As New DataTable
    Dim dRow As DataRow = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub frmSubItemUpdation_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSubItemUpdation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        dt.Columns.Add("ITEMID")
        dt.Columns.Add("TAGNO")
        dt.Columns.Add("SUBITEMNAME")
        dt.Columns.Add("SIZENAME")
        dt.Columns.Add("PCS")
        dt.Columns.Add("GRSWT")
        dt.Columns.Add("NETWT")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Temp1()
        Dim dt As New DataTable
        cmbSubItem.Items.Clear()
        strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid= '" & txtItemId_NUM.Text & "' order by SubitemName"
        objGPack.FillCombo(strSql, cmbSubItem, False, False)
        Dim dt1 As New DataTable
        cmbSize.Items.Clear()
        strSql = "Select SizeName from " & cnAdminDb & "..ItemSize Where itemId='" & txtItemId_NUM.Text & "' order by SizeName"
        objGPack.FillCombo(strSql, cmbSize, False, False)
    End Sub

    Private Sub txtItemId_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,ITEMNAME,STOCKTYPE,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'Y') = 'Y'"
            'strSql += GetItemQryFilteration("S")
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search ItemId", strSql, cn)
            If Not ro Is Nothing Then
                txtItemId_NUM.Text = Val(ro!ITEMID)
            End If
            txtItemId_NUM.SelectAll()
            Temp1()
        End If
        If e.KeyCode = Keys.Enter Then
            If txtItemId_NUM.Text.Trim = "" Then
                MsgBox("Itemid cannot Empty.", MsgBoxStyle.Information)
                txtItemId_NUM.Focus()
                Exit Sub
            End If
            Dim a() As String = txtItemId_NUM.Text.Split("-")
            If a.Length = 2 Then
                txtItemId_NUM.Text = a(0).ToString
                txtTagNo.Text = a(1).ToString
            End If
            Temp1()
        End If
    End Sub

    Private Sub txtTagNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        Dim dt, dt1 As New DataTable
        dt.Clear()
        dt1.Clear()
        If txtItemId_NUM.Text.Trim = "" Then
            MsgBox("Itemid cannot Empty.", MsgBoxStyle.Information)
            txtItemId_NUM.Focus()
            Exit Sub
        End If
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            If txtItemId_NUM.Text.Trim <> "" Then strSql += " WHERE ITEMID='" & txtItemId_NUM.Text & "'"
            'strSql += GetItemQryFilteration("S")
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
            If Not ro Is Nothing Then
                txtTagNo.Text = ro!TAGNO.ToString
                txtItemId_NUM.Text = ro!ITEMID.ToString
            End If
            txtTagNo.SelectAll()
        End If
        If e.KeyCode = Keys.Enter Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE TAGNO='" & txtTagNo.Text & "'"
            If Val(objGPack.GetSqlValue(strSql)) <> Val(txtItemId_NUM.Text) Then
                MsgBox("Invalid Itemid for this tagno", MsgBoxStyle.Information)
                txtItemId_NUM.Focus()
                Exit Sub
            End If
        End If
        'If txtItemId_NUM.Text.Trim = "" Then
        '    strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG"
        '    If txtItemId_NUM.Text <> "" Then strSql += " WHERE ITEMID='" & txtItemId_NUM.Text & "'"
        '    Dim ro As DataRow = Nothing
        '    ro = GetSqlRow(strSql, cn)
        '    If Not ro Is Nothing Then
        '        txtItemId_NUM.Text = ro!ITEMID.ToString
        '    End If
        'End If

        If txtItemId_NUM.Text.Trim <> "" Then
            cmbSubItem.Items.Clear()
            strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid IN (Select itemid from " & cnAdminDb & "..ITEMTAG T where TagNo='" & txtTagNo.Text & "') order by SubitemName"
            objGPack.FillCombo(strSql, cmbSubItem, False, False)
            'cmbSubItem.Text = "[NONE]"
            cmbSize.Items.Clear()
            strSql = "Select SizeName from " & cnAdminDb & "..ItemSize where itemid IN (Select itemid from " & cnAdminDb & "..ITEMTAG T where TagNo='" & txtTagNo.Text & "') order by SizeName"
            objGPack.FillCombo(strSql, cmbSize, False, False)
        End If
        strSql = "Select (Select SubItemName from " & cnAdminDb & "..SubItemMast S where S.SubItemId=T.SubItemId) as SubItem from " & cnAdminDb & "..ITEMTAG T where TagNo='" & txtTagNo.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("SUBITEM").ToString <> "" Then cmbSubItem.Text = dt.Rows(0).Item("SUBITEM")
        End If

        strSql = "Select (Select SizeName from " & cnAdminDb & "..ItemSize S where S.SizeId=T.SizeId AND S.ITEMID=T.ITEMID) as SizeName from " & cnAdminDb & "..ITEMTAG T where TagNo='" & txtTagNo.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            If dt1.Rows(0).Item("SizeName").ToString <> "" Then
                cmbSize.Enabled = True
                cmbSize.Text = dt1.Rows(0).Item("SizeName")
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcUpdate()
    End Sub
    Function funcUpdate() As Integer
        Dim subItemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim dt2, dt1 As New DataTable
        dt1.Clear()
        dt2.Clear()
        Dim saveFlag As Boolean = False

        For i As Integer = 0 To gridView.Rows.Count - 1
            If gridView("TAGNO", i).Value.ToString <> "" Then
                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & gridView("SUBITEMNAME", i).Value & "'"
                strSql += " AND ITEMID ='" & gridView("ITEMid", i).Value & " '"
                da = New OleDbDataAdapter(strSql, cn)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    subItemId = dt1.Rows(0).Item("SubItemId")
                End If

                strSql = " Select SizeId from " & cnAdminDb & "..ItemSize where SizeName = '" & gridView("SIZENAME", i).Value & "'"
                strSql += " AND ITEMID ='" & gridView("ITEMid", i).Value & " '"
                da = New OleDbDataAdapter(strSql, cn)
                dt2 = New DataTable
                da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    sizeId = dt2.Rows(0).Item("SizeId")
                End If

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " SUBITEMID=" & subItemId & ","
                strSql += " SIZEID=" & sizeId & ""
                strSql += " WHERE TAGNO = '" & gridView("TAGNO", i).Value & "'"
                strSql += " AND ITEMID ='" & gridView("ITEMID", i).Value & " '"
                Try
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Nothing)
                    'ExecQuery(SyncMode.Master, strSql, cn)

                    strSql = " UPDATE T SET T.MAXWASTPER=W.MAXWASTPER,T.MAXWAST=W.MAXWAST,T.MAXMCGRM=W.MAXMCGRM,T.MAXMC= W.MAXMC FROM " & cnAdminDb & "..ITEMTAG T"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..WMCTABLE W ON T.ITEMID =W.ITEMID "
                    strSql += vbCrLf + " WHERE T.TAGNO='" & gridView("TAGNO", i).Value & "' AND T.ITEMID='" & gridView("ITEMid", i).Value & "'"
                    strSql += vbCrLf + " AND T.SUBITEMID=" & subItemId & " AND W.SUBITEMID IN(" & subItemId & ",0) "
                    strSql += vbCrLf + " AND T.GRSWT BETWEEN W.FROMWEIGHT AND W.TOWEIGHT  "
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Nothing)

                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            End If
        Next
        saveFlag = True
        funcNew()

        If saveFlag = True Then MsgBox("Updated Successfully...") : btnSave.Enabled = False
    End Function
    Function funcCalGrid() As Integer
        dtGridView.Clear()
        dRow = dt.NewRow()
        If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" And cmbSubItem.Text <> "" Then
            If gridView.Rows.Count > 0 Then
                'For j As Integer = 0 To gridView.Rows.Count - 1
                If gridView("ITEMID", gridView.CurrentRow.Index).Value <> txtItemId_NUM.Text And gridView("TAGNO", gridView.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId_NUM.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("SUBITEMNAME") = cmbSubItem.Text
                    dRow("SIZENAME") = cmbSize.Text

                    'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                    strSql = " Select PCS,GRSWT,NETWT"
                    strSql += " from " & cnAdminDb & "..ITEMTAG T"
                    strSql += " Where ItemId=" + txtItemId_NUM.Text + " and TagNo='" + txtTagNo.Text + "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtGridView)
                    If dtGridView.Rows.Count > 0 Then
                        dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                        dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                        dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                    End If
                    dt.Rows.Add(dRow)
                    dt.AcceptChanges()
                    gridView.Visible = True
                    gridView.DataSource = dt
                    'gridView.Columns("SNO").Visible = False
                    'gridView.Columns("ItemId").Visible = False
                    gridView.Columns("ITEMID").Width = 63
                    gridView.Columns("TAGNO").Width = 63
                    gridView.Columns("SUBITEMNAME").MinimumWidth = 230
                    gridView.Columns("SIZENAME").MinimumWidth = 50
                    'gridView.Columns("PCS").Width = 35

                    For cnt As Integer = 0 To gridView.ColumnCount - 1
                        gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                ElseIf gridView("ITEMID", gridView.CurrentRow.Index).Value <> txtItemId_NUM.Text Or gridView("TAGNO", gridView.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId_NUM.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("SUBITEMNAME") = cmbSubItem.Text
                    dRow("SIZENAME") = cmbSize.Text

                    'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                    strSql = " Select PCS,GRSWT,NETWT"
                    strSql += " from " & cnAdminDb & "..ITEMTAG T"
                    strSql += " Where ItemId=" + txtItemId_NUM.Text + " and TagNo='" + txtTagNo.Text + "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtGridView)
                    If dtGridView.Rows.Count > 0 Then
                        dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                        dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                        dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                    End If
                    dt.Rows.Add(dRow)
                    dt.AcceptChanges()
                    gridView.Visible = True
                    gridView.DataSource = dt
                    'gridView.Columns("SNO").Visible = False
                    'gridView.Columns("ItemId").Visible = False
                    gridView.Columns("ITEMID").Width = 63
                    gridView.Columns("TAGNO").Width = 63
                    gridView.Columns("SUBITEMNAME").MinimumWidth = 230
                    gridView.Columns("SIZENAME").MinimumWidth = 50
                    'gridView.Columns("PCS").Width = 35

                    For cnt As Integer = 0 To gridView.ColumnCount - 1
                        gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                End If
                'Next
            Else
                dRow("ITEMID") = txtItemId_NUM.Text
                dRow("TAGNO") = txtTagNo.Text
                dRow("SUBITEMNAME") = cmbSubItem.Text
                dRow("SIZENAME") = cmbSize.Text

                'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                strSql = " Select PCS,GRSWT,NETWT"
                strSql += " from " & cnAdminDb & "..ITEMTAG T"
                strSql += " Where ItemId=" + txtItemId_NUM.Text + " and TagNo='" + txtTagNo.Text + "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridView)
                If dtGridView.Rows.Count > 0 Then
                    dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                    dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                    dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                End If
                dt.Rows.Add(dRow)
                dt.AcceptChanges()
                gridView.Visible = True
                gridView.DataSource = dt
                'gridView.Columns("SNO").Visible = False
                'gridView.Columns("ItemId").Visible = False
                gridView.Columns("ITEMID").Width = 63
                gridView.Columns("TAGNO").Width = 63
                gridView.Columns("SUBITEMNAME").MinimumWidth = 230
                gridView.Columns("SIZENAME").MinimumWidth = 50
                'gridView.Columns("PCS").Width = 35

                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
                Next
                btnSave.Enabled = True
            End If
        Else
            MsgBox("Invalid Entry...")
        End If
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        gridView.Visible = False
        txtItemId_NUM.Text = ""
        txtTagNo.Text = ""
        txtItemId_NUM.Focus()
        cmbSubItem.Items.Clear()
        cmbSubItem.Text = ""
        cmbSize.Text = ""
        dt.Clear()
        btnSave.Enabled = False
    End Function

    Private Sub frmSubItemUpdation_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F1) Then funcUpdate()
        If e.KeyCode = (Keys.F3) Then funcNew()
        If e.KeyCode = (Keys.F12) Then Me.Close()
        If e.KeyCode = Keys.Escape And btnSave.Enabled = True Then
            btnSave.Focus()
        End If
    End Sub
  
    Private Sub cmbSubItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSubItem.SelectedIndexChanged
        'Dim dt As New DataTable
        'cmbSubItem.Items.Clear()
        'strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid= '" & txtItemId_NUM.Text & "' order by SubitemName"
        'objGPack.FillCombo(strSql, cmbSubItem, False, False)
        'cmbSubItem.Text = "[NONE]"
    End Sub

    Private Sub cmbSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSize.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcCalGrid()
            txtItemId_NUM.Text = ""
            txtTagNo.Text = ""
            txtItemId_NUM.Focus()
            cmbSubItem.Items.Clear()
            cmbSubItem.Text = ""
            cmbSize.Text = ""
            'Me.SelectNextControl(txtItemId_NUM, False, False, False, False)
            'txtItemId_NUM.Focus()
            Label1.Focus()
        End If
    End Sub
    Private Sub cmbSize_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSize.Leave
        'funcCalGrid()
    End Sub
End Class