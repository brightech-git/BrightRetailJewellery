Imports System.Data.OleDb
Public Class frmItemUpdation
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim dt As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dtitem, dtsubitem, dtsize As New DataTable
    Dim CTITEM_UPDATE As String = GetAdmindbSoftValue("CTR_ITEM_CHANGE", "N")
    Dim CITEMTAG_UPDATE As String = GetAdmindbSoftValue("CITEMTAG_ITEM_CHANGE", "N")
    Dim TAGNOIU As String = GetAdmindbSoftValue("TAGNOFROM", "U")
    Dim tempsubitemname As String = ""
    Dim tempsizename As String = ""

    Private Sub frmItemUpdation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        DataGridView1.Visible = False
        DataGridView1.DataSource = Nothing
        txtTagNo.Text = ""
        txtTagNo.Focus()
        cmbItem.Items.Clear()
        CmbSubItem.Items.Clear()
        cmbSize.Items.Clear()
        tempsubitemname = ""
        tempsizename = ""
        dt.Clear()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem, , False)
        cmbItem.Text = ""
    End Function


    Private Sub frmItemUpdation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs)
        dt.Columns.Add("ITEMID")
        dt.Columns.Add("TAGNO")
        dt.Columns.Add("ITEMNAME")
        dt.Columns.Add("SUBITEMNAME")
        dt.Columns.Add("SIZENAME")
        dt.Columns.Add("PCS")
        dt.Columns.Add("GRSWT")
        dt.Columns.Add("NETWT")
    End Sub


    Private Sub txtTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL"
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
            If Not ro Is Nothing Then
                txtTagNo.Text = ro!TAGNO.ToString
                txtItemId_NUM.Text = ro!ITEMID.ToString
            End If
            txtTagNo.SelectAll()
        ElseIf (e.KeyCode = Keys.Enter) Then
            If txtTagNo.Text <> "" Then

                Dim barcode2d() As String = txtTagNo.Text.Split(GetAdmindbSoftValue("BARCODE2DSEP", ""))
                If barcode2d.Length = 2 Then
                    txtTagNo.Text = Trim(barcode2d(1))
                End If

                strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "' "
                If Val(GetSqlValue(cn, strSql)) <> 1 Then
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGKEY='" & txtTagNo.Text & "' "
                    If Val(GetSqlValue(cn, strSql)) = 1 Then
                        strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGKEY='" & txtTagNo.Text & "' "
                        txtTagNo.Text = GetSqlValue(cn, strSql).ToString
                    End If
                End If

                strSql = "SELECT ISNULL(CONVERT(VARCHAR(20),ISSDATE,111),'') ISSDATE FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "' "
                Dim StrDate As String = GetSqlValue(cn, strSql)
                If StrDate <> "" Then
                    MessageBox.Show("Tag Already Issue Cannot Edit This Tagno ", "Brighttech", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
                Dim dtfound As New DataTable
                If CITEMTAG_UPDATE <> "Y" Then
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "' "
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in History table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                End If
                If CTITEM_UPDATE <> "Y" Then
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CTRANSFER T WHERE TAGNO='" & txtTagNo.Text & "'   AND ISSDATE IS NULL"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in Transfer History table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                End If
                strSql = "SELECT 1 FROM " & cnAdminDb & "..PITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox("Tagno found in Pending Transit table" & vbCrLf & "Can not change the item")
                    txtTagNo.SelectAll() : txtTagNo.Focus()
                    Exit Sub
                End If

                strSql = " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST S WHERE S.ITEMID=T.ITEMID) AS ITEMNAME "
                strSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'  AND ISSDATE IS NULL"
                da = New OleDbDataAdapter(strSql, cn)
                dtitem = New DataTable
                da.Fill(dtitem)
                If dtitem.Rows.Count > 0 Then
                    If dtitem.Rows(0).Item("ITEMNAME").ToString <> "" Then
                        'cmbItem.Text = dtitem.Rows(0).Item("ItemName")
                        txtOldItemName.Text = dtitem.Rows(0).Item("ItemName").ToString
                    End If
                End If
                Dim Itemid As Integer = Nothing
                Itemid = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTagNo.Text & "'  AND ISSDATE IS NULL")
                txtItemId_NUM.Text = Itemid

                If DataGridView1.Rows.Count > 0 Then
                    For cnt As Integer = 0 To DataGridView1.Rows.Count - 1
                        If txtItemId_NUM.Text = DataGridView1.Rows(cnt).Cells("ITEMID").Value.ToString And txtTagNo.Text = DataGridView1.Rows(cnt).Cells("TAGNO").Value.ToString Then
                            MsgBox("Tagno Already Added", MsgBoxStyle.Information)
                            txtTagNo.Text = ""
                            txtOldItemName.Text = ""
                            txtOldSubItemName.Text = ""
                            txtTagNo.Focus()
                            Exit Sub
                        End If
                    Next
                End If

                tempsubitemname = ""
                If CmbSubItem.Text <> "" Then
                    tempsubitemname = CmbSubItem.Text
                Else
                    tempsubitemname = ""
                    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
                    strSql += " WHERE ITEMID='" & Itemid & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
                    objGPack.FillCombo(strSql, CmbSubItem, , False)
                End If



                strSql = "SELECT (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST S WHERE S.SUBITEMID=T.SUBITEMID) AS SUBITEM FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'  AND ISSDATE IS NULL"
                da = New OleDbDataAdapter(strSql, cn)
                dtsubitem = New DataTable
                da.Fill(dtsubitem)
                If dtsubitem.Rows.Count > 0 Then
                    If dtsubitem.Rows(0).Item("SUBITEM").ToString <> "" Then
                        'CmbSubItem.Text = dtsubitem.Rows(0).Item("SubItem")
                        txtOldSubItemName.Text = dtsubitem.Rows(0).Item("SubItem").ToString
                        'CmbSubItem.Items.Add(CMBSBITM)
                    End If
                End If

                tempsizename = ""
                strSql = "SELECT (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE S WHERE S.SIZEID=T.SIZEID) AS SIZENAME FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "' AND ISSDATE IS NULL"
                da = New OleDbDataAdapter(strSql, cn)
                dtsize = New DataTable
                da.Fill(dtsize)
                If dtsize.Rows.Count > 0 Then
                    If dtsize.Rows(0).Item("SIZENAME").ToString <> "" Then
                        cmbSize.Enabled = True
                        cmbSize.Text = dtsize.Rows(0).Item("SIZENAME")
                        tempsizename = dtsize.Rows(0).Item("SIZENAME").ToString
                    End If
                End If
            Else
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG  WHERE ISSDATE IS NULL"
                Dim ro As DataRow = Nothing
                ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
                If Not ro Is Nothing Then
                    txtTagNo.Text = ro!TAGNO.ToString
                    txtItemId_NUM.Text = ro!ITEMID.ToString
                End If
                txtTagNo.SelectAll()
            End If
        End If
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        funcUpdate()
    End Sub
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim subItemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim saveFlag As Boolean = False

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1("TAGNO", i).Value.ToString <> "" Then
                Dim dtfound As New DataTable

                If CITEMTAG_UPDATE <> "Y" Then
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CITEMTAG T WHERE TAGNO='" & DataGridView1("TAGNO", i).Value.ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox(DataGridView1("TAGNO", i).Value.ToString & " Tagno found in History table" & vbCrLf & "Can not change the item")
                        Continue For
                    End If
                End If

                If CTITEM_UPDATE <> "Y" Then

                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CTRANSFER T WHERE TAGNO='" & DataGridView1("TAGNO", i).Value.ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox(DataGridView1("TAGNO", i).Value.ToString & " Tagno found in Transfer History table" & vbCrLf & "Can not change the item")
                        Continue For
                    End If
                End If
                Dim IssUpdate As Boolean = False
                Dim RecUpdate As Boolean = False

                strSql = "SELECT 1 FROM " & cnStockDb & "..ISSUE T "
                strSql += " WHERE TAGNO='" & DataGridView1("TAGNO", i).Value.ToString & "' "
                strSql += " AND ITEMID='" & DataGridView1("ITEMID", i).Value.ToString & "' "
                strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    IssUpdate = True
                End If
                strSql = "SELECT 1 FROM " & cnStockDb & "..RECEIPT T "
                strSql += " WHERE TAGNO='" & DataGridView1("TAGNO", i).Value.ToString & "' "
                strSql += " AND ITEMID='" & DataGridView1("ITEMID", i).Value.ToString & "' "
                strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    RecUpdate = True
                End If

                strSql = "SELECT 1 FROM " & cnAdminDb & "..PITEMTAG T WHERE TAGNO='" & DataGridView1("TAGNO", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox(DataGridView1("TAGNO", i).Value.ToString & " Tagno found in Pending Transit table" & vbCrLf & "Can not change the item")
                    txtTagNo.SelectAll() : txtTagNo.Focus()
                    Exit Function
                End If

                strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & DataGridView1("ITEMNAME", i).Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtitem = New DataTable
                da.Fill(dtitem)
                If dtitem.Rows.Count > 0 Then
                    ItemId = dtitem.Rows(0).Item("ItemId")
                End If

                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & DataGridView1("SUBITEMNAME", i).Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtsubitem = New DataTable
                da.Fill(dtsubitem)
                If dtsubitem.Rows.Count > 0 Then
                    subItemId = dtsubitem.Rows(0).Item("SubItemId")
                End If

                strSql = " Select SizeId from " & cnAdminDb & "..ItemSize where SizeName = '" & DataGridView1("SIZENAME", i).Value & "'  AND ITEMID = " & ItemId & ""
                da = New OleDbDataAdapter(strSql, cn)
                dtsize = New DataTable
                da.Fill(dtsize)
                If dtsize.Rows.Count > 0 Then
                    sizeId = dtsize.Rows(0).Item("SizeId")
                End If

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " ITEMID=" & ItemId & ","
                strSql += " SUBITEMID=" & subItemId & ","
                strSql += " SIZEID=" & sizeId & ","
                strSql += " TAGKEY='" & ItemId & DataGridView1("TAGNO", i).Value & "'"
                strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"
                strSql += " AND ITEMID = '" & DataGridView1("ITEMID", i).Value & "'"

                strSql += " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET "
                strSql += " ITEMID=" & ItemId & ""
                strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"

                strSql += " UPDATE " & cnAdminDb & "..CITEMTAG SET "
                strSql += " ITEMID=" & ItemId & ""
                strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"

                ''CTRANSFER
                If CTITEM_UPDATE = "Y" Then
                    If TAGNOIU <> "U" Then Exit For
                    strSql += " UPDATE " & cnAdminDb & "..CTRANSFER SET "
                    strSql += " ITEMID=" & ItemId & ""
                    strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"
                End If
                If IssUpdate Then
                    strSql += " UPDATE " & cnStockDb & "..ISSUE SET "
                    strSql += " ITEMID=" & ItemId & ","
                    strSql += " SUBITEMID=" & subItemId & ""
                    strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"
                    strSql += " AND ITEMID = '" & DataGridView1("ITEMID", i).Value & "'"
                End If
                If RecUpdate Then
                    strSql += " UPDATE " & cnStockDb & "..RECEIPT SET "
                    strSql += " ITEMID=" & ItemId & ","
                    strSql += " SUBITEMID=" & subItemId & ""
                    strSql += " WHERE TAGNO = '" & DataGridView1("TAGNO", i).Value & "'"
                    strSql += " AND ITEMID = '" & DataGridView1("ITEMID", i).Value & "'"
                End If

                Try
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Nothing)
                    'ExecQuery(SyncMode.Master, strSql, cn)
                    saveFlag = True
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                    Exit For
                End Try
            End If
        Next
        If saveFlag = True Then MsgBox("Updated Successfully...") : btnSave.Enabled = False
        funcNew()
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Function funcCalGrid() As Integer
        dtGridView.Clear()
        dRow = dt.NewRow()
        If txtTagNo.Text <> "" And cmbItem.Text <> "" Then
            If DataGridView1.Rows.Count > 0 Then
                'For j As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1("TAGNO", DataGridView1.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId_NUM.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("ITEMNAME") = cmbItem.Text
                    dRow("SUBITEMNAME") = CmbSubItem.Text
                    dRow("SIZENAME") = cmbSize.Text

                    'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                    strSql = " Select PCS,GRSWT,NETWT"
                    strSql += " from " & cnAdminDb & "..ITEMTAG T"
                    strSql += " Where TagNo='" + txtTagNo.Text + "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtGridView)
                    If dtGridView.Rows.Count > 0 Then
                        dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                        dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                        dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                    End If
                    dt.Rows.Add(dRow)
                    dt.AcceptChanges()
                    DataGridView1.Visible = True
                    DataGridView1.DataSource = dt
                    'DataGridView1.Columns("SNO").Visible = False
                    'DataGridView1.Columns("ItemId").Visible = False
                    DataGridView1.Columns("TAGNO").Width = 63
                    DataGridView1.Columns("ITEMNAME").Width = 63
                    DataGridView1.Columns("SUBITEMNAME").MinimumWidth = 230
                    DataGridView1.Columns("SIZENAME").MinimumWidth = 50
                    'DataGridView1.Columns("PCS").Width = 35

                    For cnt As Integer = 0 To DataGridView1.ColumnCount - 1
                        DataGridView1.Columns(cnt).HeaderText = UCase(DataGridView1.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                    txtTagNo.Text = ""
                    txtOldItemName.Text = ""
                    txtOldSubItemName.Text = ""
                    txtTagNo.Focus()
                    Exit Function

                ElseIf DataGridView1("TAGNO", DataGridView1.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId_NUM.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("ITEMNAME") = cmbItem.Text
                    dRow("SUBITEMNAME") = CmbSubItem.Text
                    dRow("SIZENAME") = cmbSize.Text

                    'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                    strSql = " Select PCS,GRSWT,NETWT"
                    strSql += " from " & cnAdminDb & "..ITEMTAG T"
                    strSql += " Where TagNo='" + txtTagNo.Text + "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtGridView)
                    If dtGridView.Rows.Count > 0 Then
                        dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                        dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                        dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                    End If
                    dt.Rows.Add(dRow)
                    dt.AcceptChanges()
                    DataGridView1.Visible = True
                    DataGridView1.DataSource = dt
                    'DataGridView1.Columns("SNO").Visible = False
                    'DataGridView1.Columns("ItemId").Visible = False
                    DataGridView1.Columns("TAGNO").Width = 63
                    DataGridView1.Columns("ITEMNAME").Width = 63
                    DataGridView1.Columns("SUBITEMNAME").MinimumWidth = 230
                    DataGridView1.Columns("SIZENAME").MinimumWidth = 50
                    'DataGridView1.Columns("PCS").Width = 35

                    For cnt As Integer = 0 To DataGridView1.ColumnCount - 1
                        DataGridView1.Columns(cnt).HeaderText = UCase(DataGridView1.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                    txtTagNo.Text = ""
                    txtOldItemName.Text = ""
                    txtOldSubItemName.Text = ""
                    txtTagNo.Focus()
                    Exit Function
                End If
                'Next
            Else
                dRow("ITEMID") = txtItemId_NUM.Text
                dRow("TAGNO") = txtTagNo.Text
                dRow("ITEMNAME") = cmbItem.Text
                dRow("SUBITEMNAME") = CmbSubItem.Text
                dRow("SIZENAME") = cmbSize.Text

                'If txtItemId_NUM.Text <> "" And txtTagNo.Text <> "" Then
                strSql = " Select PCS,GRSWT,NETWT"
                strSql += " from " & cnAdminDb & "..ITEMTAG T"
                strSql += " Where TagNo='" + txtTagNo.Text + "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridView)
                If dtGridView.Rows.Count > 0 Then
                    dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                    dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                    dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
                End If
                dt.Rows.Add(dRow)
                dt.AcceptChanges()
                DataGridView1.Visible = True
                DataGridView1.DataSource = dt
                'DataGridView1.Columns("SNO").Visible = False
                'DataGridView1.Columns("ItemId").Visible = False
                DataGridView1.Columns("TAGNO").Width = 63
                DataGridView1.Columns("ITEMNAME").Width = 200
                DataGridView1.Columns("SUBITEMNAME").MinimumWidth = 200
                DataGridView1.Columns("SIZENAME").MinimumWidth = 50
                'DataGridView1.Columns("PCS").Width = 35

                For cnt As Integer = 0 To DataGridView1.ColumnCount - 1
                    DataGridView1.Columns(cnt).HeaderText = UCase(DataGridView1.Columns(cnt).HeaderText)
                Next
                btnSave.Enabled = True
                txtTagNo.Text = ""
                txtOldItemName.Text = ""
                txtOldSubItemName.Text = ""
                txtTagNo.Focus()
                Exit Function
            End If
            DataGridView1.Focus()
            'Else
            '    MsgBox("Invalid Entry...")
        End If
    End Function

    Private Sub cmbSize_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSize.Leave
        funcCalGrid()
    End Sub


    Private Sub cmbItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.LostFocus
        If cmbItem.Text <> "" Then
            Dim Itemid As Integer = Nothing
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtitem = New DataTable
            da.Fill(dtitem)
            If dtitem.Rows.Count > 0 Then
                Itemid = dtitem.Rows(0).Item("ItemId")
            End If

            If Itemid <> 0 Then
                tempsubitemname = ""
                If CmbSubItem.Text <> "" Then
                    tempsubitemname = CmbSubItem.Text
                Else
                    tempsubitemname = ""
                End If
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
                strSql += " WHERE ITEMID='" & Itemid & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
                objGPack.FillCombo(strSql, CmbSubItem, , False)
            End If

            strSql = " SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST"
            strSql += " WHERE ITEMID='" & Itemid & "' AND SUBITEMNAME='" & tempsubitemname.ToString & "' AND ACTIVE = 'Y'"
            If GetSqlValue(cn, strSql) <> 1 Then
                CmbSubItem.Text = ""
            Else
                CmbSubItem.Text = tempsubitemname.ToString
            End If

            strSql = "SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID='" & Itemid & "'"
            objGPack.FillCombo(strSql, cmbSize, , False)
            If tempsizename.ToString <> "" Then
                cmbSize.Text = tempsizename.ToString
            End If
        End If



    End Sub

    Private Sub CmbSubItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbSubItem.Leave
        If CmbSubItem.Text = "" Then
            strSql = " SELECT isnull(subitem,'N') subitem FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'"
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then
                MsgBox("Sub item can't be blank", MsgBoxStyle.Critical)
                CmbSubItem.Focus()
                Exit Sub
            End If
        End If

    End Sub


End Class