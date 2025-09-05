Imports System.Data.OleDb
Public Class frmTagnoUpdation
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim dtGrid As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dtitem, dtsubitem, dtsize As New DataTable
        Private Sub frmItemUpdation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        dgvTagnoUpd.Visible = False
        dgvTagnoUpd.DataSource = Nothing
        txtTagNo.Text = ""
        txtTagNo.Focus()
        cmbItem.Items.Clear()
        CmbSubItem.Items.Clear()
        txtNewTagno.Text = ""
        dtGrid.Clear()
        'strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        'strSql += " WHERE ACTIVE = 'Y' ORDER BY ITEMNAME"
        'objGPack.FillCombo(strSql, cmbItem, , False)
        cmbItem.Text = ""
    End Function


    Private Sub frmItemUpdation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs)
        'dt.Columns.Add("ITEMID")
        dtGrid.Columns.Add("TAGNO")
        dtGrid.Columns.Add("ITEMNAME")
        dtGrid.Columns.Add("SUBITEMNAME")
        dtGrid.Columns.Add("NEWTAGNO")
        dtGrid.Columns.Add("PCS")
        dtGrid.Columns.Add("GRSWT")
        dtGrid.Columns.Add("NETWT")
    End Sub


    Private Sub txtTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        Try
            If e.KeyCode = Keys.Insert Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                Dim ro As DataRow = Nothing
                ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
                If Not ro Is Nothing Then
                    txtTagNo.Text = ro!TAGNO.ToString
                End If
                txtTagNo.SelectAll()
            ElseIf (e.KeyCode = Keys.Enter) Then
                If txtTagNo.Text <> "" Then
                    If dgvTagnoUpd.Rows.Count > 0 Then
                        dtGridView = TryCast(dgvTagnoUpd.DataSource, DataTable)
                        For Each ro As DataRow In dtGridView.Rows
                            If ro("TAGNO").ToString <> "" Then
                                If ro("TAGNO").ToString = txtTagNo.Text Then
                                    MsgBox("This Tagno Already Loaded in Grid", MsgBoxStyle.Information)
                                    'Continue For
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                    Dim dtfound As New DataTable
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If Not dtfound.Rows.Count > 0 Then
                        MsgBox("Invalid Tagno")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in History table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                    strSql = "SELECT 1 FROM " & cnAdminDb & "..CTRANSFER T WHERE TAGNO='" & txtTagNo.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in Transfer History table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                    strSql = "SELECT 1 FROM " & cnStockDb & "..ISSUE T WHERE TAGNO='" & txtTagNo.Text & "' AND ISNULL(CANCEL,'') <> 'Y'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in Issue table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
                    End If
                    strSql = "SELECT 1 FROM " & cnStockDb & "..RECEIPT T WHERE TAGNO='" & txtTagNo.Text & "' AND ISNULL(CANCEL,'') <> 'Y'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtfound = New DataTable
                    da.Fill(dtfound)
                    If dtfound.Rows.Count > 0 Then
                        MsgBox("Tagno found in Receipt table" & vbCrLf & "Can not change the item")
                        txtTagNo.SelectAll() : txtTagNo.Focus()
                        Exit Sub
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
                    Dim CMBSBITM As String
                    strSql = "SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST S WHERE S.ITEMID=T.ITEMID) AS ITEMNAME FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtitem = New DataTable
                    da.Fill(dtitem)
                    If dtitem.Rows.Count > 0 Then
                        If dtitem.Rows(0).Item("ITEMNAME").ToString <> "" Then
                            objGPack.FillCombo(strSql, cmbItem, , False)
                            cmbItem.Text = dtitem.Rows(0).Item("ITEMNAME")
                        End If
                    End If
                    Dim Itemid As Integer = Nothing
                    Itemid = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTagNo.Text & "'")

                    'strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
                    'strSql += " WHERE ITEMID='" & Itemid & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
                    'objGPack.FillCombo(strSql, CmbSubItem, , False)
                    'CmbSubItem.Text = ""
                    strSql = "SELECT (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST S WHERE S.SUBITEMID=T.SUBITEMID) AS SUBITEM FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtsubitem = New DataTable
                    da.Fill(dtsubitem)
                    If dtsubitem.Rows.Count > 0 Then
                        If dtsubitem.Rows(0).Item("SUBITEM").ToString <> "" Then
                            objGPack.FillCombo(strSql, CmbSubItem, , False)
                            CmbSubItem.Text = dtsubitem.Rows(0).Item("SubItem")
                        Else
                            CmbSubItem.Items.Clear()
                            CmbSubItem.Text = ""
                        End If
                    Else
                        CmbSubItem.Items.Clear()
                        CmbSubItem.Text = ""
                    End If
                    'txtNewTagno.SelectAll() : txtNewTagno.Focus()

                    'strSql = "SELECT (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE S WHERE S.SIZEID=T.SIZEID) AS SIZENAME FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
                    'da = New OleDbDataAdapter(strSql, cn)
                    'dtsize = New DataTable
                    'da.Fill(dtsize)
                    'If dtsize.Rows.Count > 0 Then
                    '    If dtsize.Rows(0).Item("SIZENAME").ToString <> "" Then
                    '        cmbSize.Enabled = True
                    '        cmbSize.Text = dtsize.Rows(0).Item("SIZENAME")
                    '    End If
                    'End If
                Else
                    strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                    Dim ro As DataRow = Nothing
                    ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
                    If Not ro Is Nothing Then
                        txtTagNo.Text = ro!TAGNO.ToString
                    End If
                    txtTagNo.SelectAll()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)

        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        funcUpdate()
    End Sub
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim subItemId As Integer = Nothing
        Dim Newtagno As String
        Dim saveFlag As Boolean = False

        For i As Integer = 0 To dgvTagnoUpd.Rows.Count - 1
            If dgvTagnoUpd("NEWTAGNO", i).Value.ToString <> "" Then
                Dim dtfound As New DataTable
                strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & dgvTagnoUpd("NEWTAGNO", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox(dgvTagnoUpd("NEWTAGNO", i).Value.ToString & " Tagno found in Itemtag table" & vbCrLf & "Can not change the tagno")

                    Continue For
                End If
                strSql = "SELECT 1 FROM " & cnAdminDb & "..CITEMTAG T WHERE TAGNO='" & dgvTagnoUpd("NEWTAGNO", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox(dgvTagnoUpd("NEWTAGNO", i).Value.ToString & " Tagno found in History table" & vbCrLf & "Can not change the tagno")

                    Continue For
                End If
                strSql = "SELECT 1 FROM " & cnAdminDb & "..CTRANSFER T WHERE TAGNO='" & dgvTagnoUpd("NEWTAGNO", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox(dgvTagnoUpd("NEWTAGNO", i).Value.ToString & " Tagno found in Transfer History table" & vbCrLf & "Can not change the tagno")
                    Continue For
                End If

                strSql = "SELECT 1 FROM " & cnAdminDb & "..PITEMTAG T WHERE TAGNO='" & dgvTagnoUpd("NEWTAGNO", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtfound = New DataTable
                da.Fill(dtfound)
                If dtfound.Rows.Count > 0 Then
                    MsgBox(dgvTagnoUpd("NEWTAGNO", i).Value.ToString & " Tagno found in Pending Transit table" & vbCrLf & "Can not change the item")
                    txtNewTagno.SelectAll() : txtNewTagno.Focus()
                    Exit Function
                End If

                strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & dgvTagnoUpd("ITEMNAME", i).Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtitem = New DataTable
                da.Fill(dtitem)
                If dtitem.Rows.Count > 0 Then
                    ItemId = dtitem.Rows(0).Item("ItemId")
                End If

                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & dgvTagnoUpd("SUBITEMNAME", i).Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtsubitem = New DataTable
                da.Fill(dtsubitem)
                If dtsubitem.Rows.Count > 0 Then
                    subItemId = dtsubitem.Rows(0).Item("SubItemId")
                End If

                Newtagno = dgvTagnoUpd("NEWTAGNO", i).Value.ToString

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " ITEMID=" & ItemId & ","
                strSql += " SUBITEMID=" & subItemId & ","
                strSql += " TAGNO='" & Newtagno & "',"
                strSql += " TAGKEY='" & ItemId & dgvTagnoUpd("NEWTAGNO", i).Value & "'"
                strSql += " WHERE TAGNO = '" & dgvTagnoUpd("TAGNO", i).Value & "'"
                strSql += " AND ITEMID = '" & ItemId & "'"
                strSql += " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET "
                strSql += " ITEMID=" & ItemId & ","
                strSql += " TAGNO='" & Newtagno & "'"
                strSql += " WHERE TAGNO = '" & dgvTagnoUpd("TAGNO", i).Value & "'"
                strSql += " AND STNITEMID = '" & ItemId & "'"
                Try
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, Nothing)
                    'ExecQuery(SyncMode.Master, strSql, cn)
                    funcNew()
                    saveFlag = True
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            End If
        Next
        If saveFlag = True Then MsgBox("Updated Successfully...") : btnSave.Enabled = False
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Function funcCalGrid() As Integer
        dtGridView.Clear()
        dRow = dtGrid.NewRow()
        If txtTagNo.Text <> "" And cmbItem.Text <> "" And txtNewTagno.Text <> "" Then
            dRow("TAGNO") = txtTagNo.Text
            dRow("ITEMNAME") = cmbItem.Text
            dRow("SUBITEMNAME") = CmbSubItem.Text
            dRow("NEWTAGNO") = txtNewTagno.Text

            strSql = " Select PCS,GRSWT,NETWT"
            strSql += " from " & cnAdminDb & "..ITEMTAG T"
            strSql += " Where TagNo='" + txtTagNo.Text + "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtGridView = Nothing
            dtGridView = New DataTable
            da.Fill(dtGridView)
            If dtGridView.Rows.Count > 0 Then
                dRow("PCS") = dtGridView.Rows(0).Item("PCS").ToString
                dRow("GRSWT") = dtGridView.Rows(0).Item("GRSWT").ToString
                dRow("NETWT") = dtGridView.Rows(0).Item("NETWT").ToString
            End If
            dtGrid.Rows.Add(dRow)
            dtGrid.AcceptChanges()
            dgvTagnoUpd.Visible = True
            dgvTagnoUpd.DataSource = dtGrid
            dgvTagnoUpd.Columns("TAGNO").Width = 63
            dgvTagnoUpd.Columns("ITEMNAME").Width = 200
            dgvTagnoUpd.Columns("SUBITEMNAME").MinimumWidth = 200
            dgvTagnoUpd.Columns("NEWTAGNO").MinimumWidth = 50

            For cnt As Integer = 0 To dgvTagnoUpd.ColumnCount - 1
                dgvTagnoUpd.Columns(cnt).HeaderText = UCase(dgvTagnoUpd.Columns(cnt).HeaderText)
            Next
            btnSave.Enabled = True
            txtTagNo.SelectAll()

        Else
            MsgBox("Invalid Entry...")
        End If
    End Function

    Private Sub cmbSize_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
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

            'If Itemid <> 0 Then
            '    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
            '    strSql += " WHERE ITEMID='" & Itemid & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
            '    objGPack.FillCombo(strSql, CmbSubItem, , False)
            'End If

            'CmbSubItem.Text = ""
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

    Private Sub CmbSubItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSubItem.SelectedIndexChanged

    End Sub

    Private Sub txtNewTagno_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNewTagno.Leave
        funcCalGrid()
        End Sub

    Private Sub txtNewTagno_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewTagno.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            funcCalValidationNewtagno()
        End If
    End Sub
    Function funcCalValidationNewtagno() As Integer
        If txtNewTagno.Text <> "" Then
            If dgvTagnoUpd.Rows.Count > 0 Then
                dtGridView = TryCast(dgvTagnoUpd.DataSource, DataTable)
                For Each ro As DataRow In dtGridView.Rows
                    If ro("NEWTAGNO").ToString <> "" Then
                        If ro("NEWTAGNO").ToString = txtNewTagno.Text Then
                                MsgBox("This Tagno Already Loaded in Grid", MsgBoxStyle.Information)
                            'Continue For
                            Exit Function
                        End If
                    End If
                Next
            End If
            Dim dtfound As New DataTable
            strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtNewTagno.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Itemtag table" & vbCrLf & "Can not change the tagno")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If

            strSql = "SELECT 1 FROM " & cnAdminDb & "..CITEMTAG T WHERE TAGNO='" & txtNewTagno.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in History table" & vbCrLf & "Can not change the tago")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If
            strSql = "SELECT 1 FROM " & cnAdminDb & "..CTRANSFER T WHERE TAGNO='" & txtNewTagno.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Transfer History table" & vbCrLf & "Can not change the tagno")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If
            strSql = "SELECT 1 FROM " & cnStockDb & "..ISSUE T WHERE TAGNO='" & txtNewTagno.Text & "' AND ISNULL(CANCEL,'') <> 'Y'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Issue table" & vbCrLf & "Can not change the tagno")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If
            strSql = "SELECT 1 FROM " & cnStockDb & "..RECEIPT T WHERE TAGNO='" & txtNewTagno.Text & "' AND ISNULL(CANCEL,'') <> 'Y'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Receipt table" & vbCrLf & "Can not change the tagno")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If
            strSql = "SELECT 1 FROM " & cnAdminDb & "..PITEMTAG T WHERE TAGNO='" & txtNewTagno.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtfound = New DataTable
            da.Fill(dtfound)
            If dtfound.Rows.Count > 0 Then
                MsgBox("Tagno found in Pending Transit table" & vbCrLf & "Can not change the tagno")
                txtNewTagno.SelectAll() : txtNewTagno.Focus()
                Exit Function
            End If
            'funcCalGrid()
        End If
    End Function
End Class