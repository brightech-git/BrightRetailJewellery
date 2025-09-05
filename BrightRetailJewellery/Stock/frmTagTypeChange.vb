Imports System.Data.OleDb
Public Class frmTagTypeChange
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim dt As New DataTable
    Dim dRow As DataRow = Nothing
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
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
        dt.Columns.Add("TYPE")
        dt.Columns.Add("PCS")
        dt.Columns.Add("GRSWT")
        dt.Columns.Add("NETWT")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub txtItemId_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,ITEMNAME,STOCKTYPE,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'Y') = 'Y'"
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search ItemId", strSql, cn)
            If Not ro Is Nothing Then
                txtItemId.Text = Val(ro!ITEMID)
            End If
            txtItemId.SelectAll()
        End If
        If e.KeyCode = Keys.Enter Then
            If txtItemId.Text.Trim = "" Then
                MsgBox("Itemid cannot Empty.", MsgBoxStyle.Information)
                txtItemId.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtTagNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        Dim dt, dt1 As New DataTable
        dt.Clear()
        dt1.Clear()
        If txtItemId.Text.Trim = "" Then
            MsgBox("Itemid cannot Empty.", MsgBoxStyle.Information)
            txtItemId.Focus()
            Exit Sub
        End If
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            If txtItemId.Text.Trim <> "" Then strSql += " WHERE ITEMID='" & txtItemId.Text & "'"
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search TAGNO", strSql, cn)
            If Not ro Is Nothing Then
                txtTagNo.Text = ro!TAGNO.ToString
                txtItemId.Text = ro!ITEMID.ToString
            End If
            txtTagNo.SelectAll()
        End If
        If e.KeyCode = Keys.Enter Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE TAGNO='" & txtTagNo.Text & "'"
            If Val(txtItemId.Text) <> 0 Then
                strSql += " AND ITEMID=" & Val(txtItemId.Text)
            End If
            If Val(objGPack.GetSqlValue(strSql)) <> Val(txtItemId.Text) Then
                MsgBox("Invalid Itemid for this tagno", MsgBoxStyle.Information)
                txtItemId.Focus()
                Exit Sub
            End If
        End If
        'If txtItemId_NUM.Text.Trim <> "" Then
        '    cmbSubItem.Items.Clear()
        '    strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE "
        '    objGPack.FillCombo(strSql, cmbSubItem, False, False)
        'End If
        'strSql = "SELECT (SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE S WHERE S.ITEMTYPEID=T.ITEMTYPEID) AS TYPE FROM " & cnAdminDb & "..ITEMTAG T WHERE TAGNO='" & txtTagNo.Text & "'"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If dt.Rows(0).Item("TYPE").ToString <> "" Then cmbSubItem.Text = dt.Rows(0).Item("TYPE")
        'End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
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
                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & gridView("TYPE", i).Value & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dt1 = New DataTable
                da.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    subItemId = dt1.Rows(0).Item("ITEMTYPEID")
                End If

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " ITEMTYPEID=" & subItemId & ""
                strSql += " WHERE TAGNO = '" & gridView("TAGNO", i).Value & "'"
                strSql += " AND ITEMID ='" & gridView("ITEMID", i).Value & "'"
                Try
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
        If txtItemId.Text <> "" And txtTagNo.Text <> "" And cmbSubItem.Text <> "" Then
            If gridView.Rows.Count > 0 Then
                If gridView("ITEMID", gridView.CurrentRow.Index).Value <> txtItemId.Text And gridView("TAGNO", gridView.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("TYPE") = cmbSubItem.Text
                    strSql = " SELECT PCS,GRSWT,NETWT"
                    strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
                    strSql += " WHERE ITEMID=" + txtItemId.Text + " AND TAGNO='" + txtTagNo.Text + "'"
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
                    gridView.Columns("ITEMID").Width = 63
                    gridView.Columns("TAGNO").Width = 63
                    gridView.Columns("TYPE").MinimumWidth = 230
                    For cnt As Integer = 0 To gridView.ColumnCount - 1
                        gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                ElseIf gridView("ITEMID", gridView.CurrentRow.Index).Value <> txtItemId.Text Or gridView("TAGNO", gridView.CurrentRow.Index).Value <> txtTagNo.Text Then
                    dRow("ITEMID") = txtItemId.Text
                    dRow("TAGNO") = txtTagNo.Text
                    dRow("TYPE") = cmbSubItem.Text

                    strSql = " SELECT PCS,GRSWT,NETWT"
                    strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
                    strSql += " WHERE ITEMID=" + txtItemId.Text + " AND TAGNO='" + txtTagNo.Text + "'"
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
                    gridView.Columns("ITEMID").Width = 63
                    gridView.Columns("TAGNO").Width = 63
                    gridView.Columns("TYPE").MinimumWidth = 230

                    For cnt As Integer = 0 To gridView.ColumnCount - 1
                        gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
                    Next
                    btnSave.Enabled = True
                End If
                'Next
            Else
                dRow("ITEMID") = txtItemId.Text
                dRow("TAGNO") = txtTagNo.Text
                dRow("TYPE") = cmbSubItem.Text

                strSql = " SELECT PCS,GRSWT,NETWT"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += " WHERE ITEMID=" + txtItemId.Text + " AND TAGNO='" + txtTagNo.Text + "'"
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
                gridView.Columns("ITEMID").Width = 63
                gridView.Columns("TAGNO").Width = 63
                gridView.Columns("TYPE").MinimumWidth = 230
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
        txtItemId.Text = ""
        txtTagNo.Text = ""
        txtItemId.Focus()
        cmbSubItem.Items.Clear()
        cmbSubItem.Text = ""
        btnSave.Enabled = False
        gridView.DataSource = Nothing
        gridView.Refresh()
        dt.Clear()
        cmbSubItem.Items.Clear()
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE "
        objGPack.FillCombo(strSql, cmbSubItem, False, False)
    End Function

    Private Sub frmSubItemUpdation_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F1) Then funcUpdate()
        If e.KeyCode = (Keys.F3) Then funcNew()
        If e.KeyCode = (Keys.F12) Then Me.Close()
        If e.KeyCode = Keys.Escape And btnSave.Enabled = True Then
            btnSave.Focus()
        End If
    End Sub

    Private Function Barcode2ddetails(ByVal barcode2dstring As String)
        Dim barcode2darray1() As String = barcode2dstring.Split(BARCODE2DSEP)
        txtItemId.Text = barcode2darray1(0).ToString
        txtTagNo.Text = barcode2darray1(2).ToString
        If txtTagNo.Text <> "" Then txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Function

    Private Sub txtItemId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim barcode2d() As String = txtItemId.Text.Split(BARCODE2DSEP)
            If barcode2d.Length > 2 Then Call Barcode2ddetails(txtItemId.Text) : Exit Sub

            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            Dim ScanStr As String = txtItemId.Text
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = Trim(sp(0))
            End If
            If txtItemId.Text.StartsWith("#") Then txtItemId.Text = txtItemId.Text.Remove(0, 1)
CheckItem:
            If txtItemId.Text = "" Then
                MsgBox("Item Id should not empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If PRODTAGSEP <> "" Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr).Replace(PRODTAGSEP, "") & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf IsNumeric(ScanStr) = True And ScanStr.Contains(PRODTAGSEP) = False Then
                Exit Sub
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'") = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                    Exit Sub
                    GoTo CheckItem
                End If
            End If
LoadItemInfo:
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" Then
                txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            End If
        End If
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        funcCalGrid()
        txtItemId.Text = ""
        txtTagNo.Text = ""
        txtItemId.Focus()
        Label1.Focus()
    End Sub
End Class