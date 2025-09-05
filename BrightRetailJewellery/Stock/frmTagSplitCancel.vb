Imports System.Data.OleDb
Public Class frmTagSplitCancel
    Dim dtTagDet As New DataTable
    Dim strSql As String
    Dim TblName As String = "ITEMTAG"

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
        dtTagDet.Rows.Clear()
        dtTagDet.AcceptChanges()
        txtTagNo.Select()
        gridView.DataSource = Nothing
        gridView.Refresh()
        ShowTagDetail(False)
    End Sub

    Private Sub frmTagCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlTagDetail.Size = New Size(356, 150)

        gridView.BackgroundColor = Color.White
        dtTagDet = New DataTable
        dtTagDet.Columns.Add("ITEM", GetType(String))
        dtTagDet.Columns.Add("TAGNO", GetType(String))
        dtTagDet.Columns.Add("OLDTAGNO", GetType(String))
        dtTagDet.Columns.Add("ITEMID", GetType(String))
        gridView.DataSource = dtTagDet
        gridView.Columns("ITEM").MinimumWidth = 180
        gridView.Columns("ITEMID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        fNew()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim OldTagCnt As Integer
            If txtTagNo.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If

            strSql = "SELECT OLDTAGNO FROM " & cnAdminDb & ".." & TblName & " "
            strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            Dim OldTag As String = ""
            OldTag = objGPack.GetSqlValue(strSql, "OLDTAGNO", "")
            If OldTag = "" Then MsgBox("TagNo Not found", MsgBoxStyle.Information) : txtTagNo.Focus() : Exit Sub


            strSql = "SELECT REFDATE FROM " & cnAdminDb & ".." & TblName & " "
            strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            Dim RefDate As Date
            RefDate = objGPack.GetSqlValue(strSql, "REFDATE", "")
            If RefDate <> GetEntryDate(GetServerDate, tran).ToString Then MsgBox("Previous Date Cancel not Allowed", MsgBoxStyle.Information) : txtTagNo.Focus() : Exit Sub

            strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAG WHERE OLDTAGNO='" & OldTag & "'"
            OldTagCnt = objGPack.GetSqlValue(strSql, "CNT", 0)

            dtTagDet.Rows.Clear()
            strSql = " SELECT "
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += " ,TAGNO,OLDTAGNO,ITEMID FROM " & cnAdminDb & ".." & TblName & " AS T"
            strSql += " WHERE TAGNO = '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            strSql += " UNION ALL"
            strSql += " SELECT "
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += " ,TAGNO,OLDTAGNO,ITEMID FROM " & cnAdminDb & ".." & TblName & " AS T"
            strSql += " WHERE TAGNO <> '" & txtTagNo.Text & "' AND ISNULL(ISSDATE,'')='' "
            strSql += " AND OLDTAGNO = '" & OldTag & "'"
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cnCostId <> "" Then strSql += " AND COSTID = '" & cnCostId & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtTagDet.Rows.Count > 0 Then
                If dtTagDet.Rows.Count = OldTagCnt Then
                    With gridView
                        .DataSource = dtTagDet
                        .Columns("ITEMID").Visible = False
                        .Columns("ITEM").Width = 180
                        .Refresh()
                    End With
                    btnCancel.Enabled = True
                    ShowTagDetail(True)
                    btnCancel.Focus()
                Else
                    MsgBox("Splitted Tag Count not Matched", MsgBoxStyle.Information)
                    gridView.DataSource = Nothing
                    gridView.Refresh()
                    btnCancel.Enabled = False
                    ShowTagDetail(False)
                    txtTagNo.Focus()
                    Exit Sub
                End If
            Else
                gridView.DataSource = Nothing
                gridView.Refresh()
                btnCancel.Enabled = False
                ShowTagDetail(False)
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
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If gridView.RowCount <= 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Try
            tran = Nothing
            tran = cn.BeginTransaction

            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN"
            strSql += " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG  WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            strSql += " AND OLDTAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND NARRATION IN ('SPLITTED','TAG SPLITTED'))"
            strSql += " AND ISSDATE IS NULL"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO IN"
            strSql += " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG  WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            strSql += " AND OLDTAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND NARRATION IN ('SPLITTED','TAG SPLITTED'))"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO IN"
            strSql += " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG  WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            strSql += " AND OLDTAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND NARRATION IN ('SPLITTED','TAG SPLITTED'))"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAG "
            strSql += " WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(OLDTAGNO,'')<>''"
            strSql += " AND OLDTAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND NARRATION IN ('SPLITTED','TAG SPLITTED')"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE=NULL,TOFLAG=NULL"
            strSql += " WHERE TAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND ISSDATE IS NOT NULL AND TOFLAG='MI'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET ISSDATE=NULL"
            strSql += " WHERE TAGNO='" & gridView.Rows(0).Cells("OLDTAGNO").Value.ToString & "'"
            strSql += " AND ISSDATE IS NOT NULL "
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            tran.Commit()
            tran = Nothing
            MsgBox("Cancelled Successfully", MsgBoxStyle.Information)
            fNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class