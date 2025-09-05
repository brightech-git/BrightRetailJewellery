Imports System.Data.OleDb
Public Class FRM_USRATEUPDATE_BULK
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim dt As New DataTable

    Private Sub FRM_USRATEUPDATE_BULK_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            tabMain.SelectedTab = tabGeneral
            txtItemCode_NUM.Select()
        End If
    End Sub
    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT DISTINCT"
            strSql += vbcrlf + " ITEMID, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS T "
            itemId = BrighttechPack.SearchDialog.Show("Find ItemId", StrSql, cn)
            strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & itemId & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = itemId
                txtItemName.Text = dt.Rows(0).Item("itemName")
            End If
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            'StrSql += " AND (METALID IN ('T','D') AND STUDDED = 'L')"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemName.Text = dt.Rows(0).Item("itemName")
            Else
                txtItemName.Clear()
                txtItemCode_NUM.Text = ""
            End If

        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If ChkOldUsRate.Checked = False And chkOldRup.Checked = False Then
            MsgBox("Check any one Combination", MsgBoxStyle.Information)
            ChkOldUsRate.Focus()
            Exit Sub
        End If
        If ChkOldUsRate.Checked = True Then
            If txtNewUSRate_AMT.Text = String.Empty Then
                MsgBox("UsRate should Not Rate Empty", MsgBoxStyle.Information)
                txtNewUSRate_AMT.Focus()
                Exit Sub
            End If
        End If
        If chkOldRup.Checked = True Then
            If txtNewRupees_AMT.Text = String.Empty Then
                MsgBox("Rupess should Not Rate Empty", MsgBoxStyle.Information)
                txtNewRupees_AMT.Focus()
                Exit Sub
            End If
        End If
        Me.Cursor = Cursors.WaitCursor
        StrSql = String.Empty
        StrSql = "SELECT IM.ITEMID,IM.ITEMNAME,SI.SUBITEMNAME,SI.SUBITEMID,IT.TAGNO,IT.SNO,"
        If RdEffectTag.Checked Then
            StrSql += vbCrLf + " IT.USRATE AS OLDUSRATE," & IIf(Val(txtNewUSRate_AMT.Text) > 0, Val(txtNewUSRate_AMT.Text), " IT.USRATE ") & " AS NEWUSRATE,"
        ElseIf RdEffectTagStn.Checked Then
            StrSql += vbCrLf + " IT.USRATE AS OLDUSRATE," & IIf(Val(txtNewUSRate_AMT.Text) > 0, Val(txtNewUSRate_AMT.Text), " IT.USRATE ") & " AS NEWUSRATE,"
        End If
        StrSql += vbCrLf + " IT.INDRS AS OLDINDRUPESS," & IIf(Val(txtNewRupees_AMT.Text) > 0, Val(txtNewRupees_AMT.Text), " IT.INDRS ") & " AS NEWINDRUPESS "
        If RdEffectTag.Checked Then
            StrSql += vbCrLf + " FROM " & cnAdminDb & " ..ITEMTAG IT"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID =IT.ITEMID "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID =IT.SUBITEMID "
        ElseIf RdEffectTagStn.Checked Then
            StrSql += vbCrLf + " FROM " & cnAdminDb & " ..ITEMTAGSTONE IT"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID =IT.STNITEMID "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID =IT.STNSUBITEMID "
        End If
        StrSql += vbCrLf + " WHERE ISSDATE IS NULL "
        If ChkOldUsRate.Checked Then
            StrSql += vbCrLf + " AND ISNULL(USRATE,0) = " & Val(txtOldUSRate_AMT.Text)
        End If
        If chkOldRup.Checked Then
            StrSql += vbCrLf + " AND ISNULL(INDRS,0) = " & Val(txtOldRupees_AMT.Text)
        End If
        If txtItemCode_NUM.Text <> String.Empty Then
            StrSql += vbCrLf + " AND IT.ITEMID =" & txtItemCode_NUM.Text
        End If
        If txtTagNo.Text <> String.Empty Then
            StrSql += vbCrLf + " AND IT.TAGNO ='" & txtTagNo.Text & "'"
        End If
        StrSql += vbCrLf + " AND ISNULL(IT.COMPANYID,'')='" & strCompanyId & "'"
        Da = New OleDbDataAdapter
        dt = New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = False
        dt.Columns.Add(dtCol)
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then
            GridView_OWN.DataSource = Nothing
            GridView_OWN.DataSource = dt
            GridView_OWN.RowHeadersVisible = False
            FormatGridColumns(GridView_OWN, False, False, , False)
            GridView_OWN.Columns("CHECK").ReadOnly = False
            GridView_OWN.Columns("SNO").Visible = False
            GridView_OWN.Columns("ITEMID").Visible = False
            GridView_OWN.Columns("SUBITEMID").Visible = False
            'GridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            GridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            GridView_OWN.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information, "Brighttech Gold")
            GridView_OWN.DataSource = Nothing
            Me.Cursor = Cursors.Arrow
            Exit Sub
        End If
        Me.Cursor = Cursors.Arrow
        If Not GridView_OWN.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        GridView_OWN.Select()
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dr() As DataRow
        dr = CType(GridView_OWN.DataSource, DataTable).Select("CHECK = TRUE")
        If Not dr.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            Dim objSecret As New frmAdminPassword()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            Dim qry As String = Nothing
            For Each ro As DataRow In dr
                If RdEffectTag.Checked Then
                    qry = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
                    qry += vbCrLf + " USRATE=" & Val(ro.Item("NEWUSRATE").ToString) & ""
                    qry += vbCrLf + " ,RATE=CASE WHEN " & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & " > 0 THEN " & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & " ELSE RATE END"
                    qry += vbCrLf + " ,INDRS=" & Val(ro.Item("NEWINDRUPESS").ToString) & ""
                    qry += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                    qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
                    qry += vbCrLf + " AND ITEMID='" & ro.Item("ITEMID").ToString & "'"
                    Cmd = New OleDbCommand(qry, cn, tran)
                    Cmd.ExecuteNonQuery()
                ElseIf RdEffectTagStn.Checked Then
                    qry = "UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET "
                    qry += vbCrLf + " USRATE=" & Val(ro.Item("NEWUSRATE").ToString) & ""
                    qry += vbCrLf + " ,STNRATE=CASE WHEN " & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & " > 0 THEN " & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & " ELSE STNRATE END"
                    qry += vbCrLf + " ,INDRS=" & Val(ro.Item("NEWINDRUPESS").ToString) & ""
                    qry += vbCrLf + " ,STNAMT = CASE WHEN " & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & " > 0 THEN CASE WHEN CALCMODE = 'P' THEN STNPCS ELSE STNWT END * (" & Val(ro.Item("OLDINDRUPESS").ToString) & " * " & Val(ro.Item("NEWUSRATE").ToString) & ") ELSE STNAMT END"
                    qry += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                    qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
                    qry += vbCrLf + " AND STNITEMID='" & ro.Item("ITEMID").ToString & "'"
                    Cmd = New OleDbCommand(qry, cn, tran)
                    Cmd.ExecuteNonQuery()
                End If
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
            tabMain.SelectedTab = tabGeneral
            txtItemCode_NUM.Select()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Function SelectAll(ByVal Obj As Object)
        If GridView_OWN.Rows.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            For i As Integer = 0 To GridView_OWN.Rows.Count - 1
                GridView_OWN.Rows(i).Cells("CHECK").Value = Obj.checked
            Next
            Me.Cursor = Cursors.Arrow
        End If
    End Function
    Private Sub ChkAll_OWN_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll_OWN.CheckedChanged
        SelectAll(sender)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtItemCode_NUM.Select()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtItemCode_NUM.Clear()
        txtItemName.Clear()
        txtTagNo.Clear()
        txtNewRupees_AMT.Clear()
        txtNewUSRate_AMT.Clear()
        txtOldRupees_AMT.Clear()
        txtOldUSRate_AMT.Clear()
        RdEffectTag.Checked = True
        GridView_OWN.DataSource = Nothing
        txtItemCode_NUM.Focus()
    End Sub

    Private Sub FRM_USRATEUPDATE_BULK_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub chkOldRup_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOldRup.CheckStateChanged
        If chkOldRup.Checked = False Then
            txtNewRupees_AMT.Clear()
            txtOldRupees_AMT.Clear()
            txtOldRupees_AMT.Enabled = False
            txtNewRupees_AMT.Enabled = False
        Else
            txtOldRupees_AMT.Enabled = True
            txtNewRupees_AMT.Enabled = True
        End If
    End Sub

    Private Sub ChkOldUsRate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkOldUsRate.CheckStateChanged
        If ChkOldUsRate.Checked = False Then
            txtNewUSRate_AMT.Clear()
            txtOldUSRate_AMT.Clear()
            txtOldUSRate_AMT.Enabled = False
            txtNewUSRate_AMT.Enabled = False
            chkOldRup.Focus()
        Else
            txtOldUSRate_AMT.Enabled = True
            txtNewUSRate_AMT.Enabled = True
        End If
    End Sub

    Private Sub txtTagNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.Leave
        If txtItemCode_NUM.Text <> "" Or txtTagNo.Text <> "" Then
            StrSql = String.Empty
            StrSql = "SELECT IM.ITEMID,IM.ITEMNAME,SI.SUBITEMNAME,SI.SUBITEMID,IT.TAGNO,IT.SNO,"
            StrSql += vbCrLf + " IT.USRATE AS OLDUSRATE,"
            StrSql += vbCrLf + " IT.INDRS AS OLDINDRUPESS"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG IT"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID =IT.ITEMID "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID =IT.SUBITEMID "
            StrSql += vbCrLf + " WHERE ISSDATE IS NULL "
            If txtItemCode_NUM.Text <> String.Empty Then
                StrSql += vbCrLf + " AND IT.ITEMID =" & txtItemCode_NUM.Text
            End If
            If txtTagNo.Text <> String.Empty Then
                StrSql += vbCrLf + " AND IT.TAGNO ='" & txtTagNo.Text & "'"
            End If
            StrSql += vbCrLf + " AND ISNULL(IT.COMPANYID,'')='" & strCompanyId & "'"
            Da = New OleDbDataAdapter
            dt = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            dt.AcceptChanges()
            If dt.Rows.Count > 0 Then
                GridView1.DataSource = Nothing
                GridView1.DataSource = dt
                FormatGridColumns(GridView1, False, False, , False)
                GridView1.Columns("SNO").Visible = False
                GridView1.Columns("ITEMID").Visible = False
                GridView1.Columns("SUBITEMID").Visible = False
                GridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                GridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            Else
                MsgBox("No Record Found", MsgBoxStyle.Information, "Brighttech Gold")
                GridView1.DataSource = Nothing
                Exit Sub
            End If
        End If
    End Sub
End Class