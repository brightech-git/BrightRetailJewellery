Imports System.Data.OleDb
Public Class frmTagCheck
    Dim dtTagDet As New DataTable
    Dim dtItemDet As New DataTable
    Dim strSql As String
    Dim TblName As String = "ITEMTAG"
    Dim TAGSEARCH As Boolean = IIf(GetAdmindbSoftValue("TAGSEARCH", "N").ToUpper.ToString = "Y", True, False)

    Private Sub ShowTagDetail(ByVal status As Boolean)
        If status Then
            Me.Size = New Size(362, 220)
            pnlTagDetail.Visible = status
        Else
            Me.Size = New Size(362, 78)
            pnlTagDetail.Visible = status
        End If
    End Sub


    Private Sub fNew()
        txtTagNo.Clear()
        txtHuid.Clear()
        dtTagDet.Rows.Clear()
        dtTagDet.AcceptChanges()
        txtTagNo.Select()
    End Sub

    Private Sub frmTagCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlTagDetail.Size = New Size(356, 150)

        gridView.BackgroundColor = Color.White
        dtTagDet = New DataTable
        dtTagDet.Columns.Add("ITEM", GetType(String))
        dtTagDet.Columns.Add("TAGNO", GetType(String))
        dtTagDet.Columns.Add("ITEMID", GetType(String))
        gridView.DataSource = dtTagDet
        gridView.Columns("ITEM").MinimumWidth = 180
        gridView.Columns("ITEMID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        fNew()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
            dtTagDet.Rows.Clear()
            gridView.DataSource = dtTagDet
            Dim strNarration As String = txtTagNo.Text.ToString
            dtItemDet = New DataTable

            Dim itemId As String = ""
            Dim spChar As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            If spChar <> "" Then
                If txtTagNo.Text.Contains(spChar) Then
                    Dim sp() As String = txtTagNo.Text.Split(spChar)
                    itemId = Trim(sp(0))
                    If sp.Length >= 2 Then
                        txtTagNo.Text = Trim(sp(1))
                    End If
                End If
            End If
            Dim LoopCnt As Integer = 0
            TblName = "ITEMTAG"
TagReCheck:
            Dim SearchCond As String = " WHERE TAGNO LIKE '%" & txtTagNo.Text & "%'"
            If TAGSEARCH = True Then
                SearchCond = " WHERE TAGNO ='" & txtTagNo.Text & "'"
            End If
            dtTagDet.Rows.Clear()
            strSql = " SELECT DISTINCT "
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += " ,TAGNO,ITEMID FROM " & cnAdminDb & ".." & TblName & " AS T"
            strSql += SearchCond
            If itemId <> "" Then
                strSql += " AND ITEMID = " & Val(itemId) & ""
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtTagDet.Rows.Count = 0 Then
                dtTagDet.Rows.Clear()
                If TAGSEARCH = True Then
                Else
                    SearchCond = " WHERE NARRATION LIKE '%" & strNarration.ToString & "%'"
                End If
                strSql = " SELECT DISTINCT "
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                strSql += " ,TAGNO,ITEMID FROM " & cnAdminDb & ".." & TblName & " AS T"
                strSql += SearchCond
                If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTagDet)
            End If

            If dtTagDet.Rows.Count > 0 Then
                If dtTagDet.Rows.Count = 1 Then
                    Dim objTagViewer As New frmTagImageViewer(
                     gridView.Rows(gridView.CurrentRow.Index).Cells("TAGNO").Value.ToString,
                     gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMID").Value.ToString,
                     BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False), IIf(TblName = "ITEMTAG", True, False))
                    objTagViewer.ShowDialog()
                    Exit Sub
                End If
                gridView.Focus()
            Else
                If LoopCnt > 5 Then
                    MsgBox("TagNo doesn't Exist..!", MsgBoxStyle.Information)
                    txtTagNo.Select()
                    Exit Sub
                End If
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtTagNo.Text) & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    itemId = Val(dtItemDet.Rows(0).Item("ITEMID").ToString)
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    LoopCnt += 1
                    GoTo TagReCheck
                ElseIf TblName = "ITEMTAG" Then
                    TblName = "CITEMTAG"
                    LoopCnt += 1
                    GoTo TagReCheck
                End If
            End If
            If (dtTagDet.Rows.Count = 0 And dtItemDet.Rows.Count = 0) Then
                MsgBox("TagNo doesn't Exist..!", MsgBoxStyle.Information)
                txtTagNo.Select()
            End If
        End If
    End Sub

    Private Sub txtHuid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHuid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtHuid.Text = "" Then
                MsgBox("HallmarkNo should not empty", MsgBoxStyle.Information)
                txtHuid.Focus()
                Exit Sub
            End If
            Dim _HMTAGSNO As String = ""
            Dim LoopCnt As Integer = 0
            TblName = "ITEMTAG"
TagReCheck1:
            strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE HM_BILLNO = '" & Trim(txtHuid.Text) & "' "
            If Val(GetSqlValue(cn, strSql).ToString) > 0 Then
                dtTagDet.Rows.Clear()
                strSql = " SELECT DISTINCT "
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                strSql += " ,TAGNO,ITEMID FROM " & cnAdminDb & ".." & TblName & " AS T"
                strSql += " WHERE SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & ".." & TblName & "HALLMARK WHERE HM_BILLNO = '" & Trim(txtHuid.Text) & "')"
                If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTagDet)
                If dtTagDet.Rows.Count > 0 Then
                    If dtTagDet.Rows.Count = 1 Then
                        Dim objTagViewer As New frmTagImageViewer(
                             gridView.Rows(gridView.CurrentRow.Index).Cells("TAGNO").Value.ToString,
                             gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMID").Value.ToString,
                             BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False), IIf(TblName = "ITEMTAG", True, False))
                        objTagViewer.ShowDialog()
                        Exit Sub
                    End If
                    gridView.Focus()
                Else
                    If LoopCnt > 0 Then
                        MsgBox("HallmarkNo doesn't Exist..!", MsgBoxStyle.Information)
                        txtTagNo.Select()
                        Exit Sub
                    End If
                    TblName = "CITEMTAG"
                    LoopCnt += 1
                    GoTo TagReCheck1
                End If
            Else
                MsgBox("Invalid HallmarkNo", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        If Not gridView.RowCount > 0 Then
            txtTagNo.Focus()
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentCell.RowIndex).Cells("ITEM")
            Else
                txtTagNo.Focus()
                Exit Sub
            End If
            Dim objTagViewer As New frmTagImageViewer(
            gridView.Rows(gridView.CurrentRow.Index).Cells("TAGNO").Value.ToString,
            gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMID").Value.ToString,
            BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False))
            objTagViewer.ShowDialog()
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class