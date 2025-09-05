Imports System.Data.OleDb

Public Class OrderTagtoRegular
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        chkSelectAll.Checked = False
        txtTagNo.Clear()
        cmbItem.Text = "ALL"
        chkDate.Checked = False
        chkDate.Select()
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpFrom.Enabled = chkDate.Checked
        dtpTo.Enabled = chkDate.Checked
    End Sub

    Private Sub OrderTagtoRegular_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub OrderTagtoRegular_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE = 'Y' AND STOCKTYPE = 'T' "
        StrSql += GetItemQryFilteration("S")
        StrSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbItem, False, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        Try
            StrSql = vbCrLf + " SELECT T.ORDREPNO ORDERNO,T.RECDATE"
            StrSql += vbCrLf + " ,I.ITEMNAME ITEM"
            StrSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
            StrSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            StrSql += vbCrLf + " ,T.ORSNO,T.SNO,T.ITEMCTRID,T.COSTID"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            StrSql += vbCrLf + " WHERE ISNULL(T.ORDREPNO,'') <> '' AND ISNULL(T.ORSNO,'') <> ''"
            StrSql += vbCrLf + " AND T.ISSDATE IS NULL AND ISNULL(APPROVAL,'') = ''"
            If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If chkDate.Checked Then StrSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then StrSql += vbCrLf + " AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            If txtTagNo.Text <> "" Then StrSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("CHECK", GetType(Boolean))
            dtGrid.Columns("CHECK").DefaultValue = CType(chkSelectAll.Checked, Boolean)
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                chkDate.Select()
                Exit Sub
            End If
            gridView.DataSource = dtGrid
            With gridView
                For Each DgvCol As DataGridViewColumn In gridView.Columns
                    DgvCol.ReadOnly = True
                Next
                BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
                gridView.Columns("CHECK").Width = 40
                gridView.Columns("ORDERNO").Width = 70
                gridView.Columns("RECDATE").Width = 80
                gridView.Columns("ITEM").Width = 200
                gridView.Columns("TAGNO").Width = 80
                gridView.Columns("PCS").Width = 60
                gridView.Columns("GRSWT").Width = 80
                gridView.Columns("NETWT").Width = 80
                gridView.Columns("COUNTER").Width = 130

                gridView.Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                gridView.Columns("CHECK").ReadOnly = False
                gridView.Columns("CHECK").HeaderText = ""
                gridView.Columns("SNO").Visible = False
                gridView.Columns("ORSNO").Visible = False
                gridView.Columns("ITEMCTRID").Visible = False
                gridView.Columns("COSTID").Visible = False
                gridView.Select()
            End With
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If gridView.DataSource Is Nothing Then Exit Sub
        Dim dtSource As New DataTable
        dtSource = CType(gridView.DataSource, DataTable).Copy
        If Not dtSource.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim roSelected() As DataRow = dtSource.Select("CHECK = TRUE")
        If Not roSelected.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            If gridView.RowCount > 0 Then gridView.Select() Else btnSearch.Focus()
            Exit Sub
        End If
        Dim NewCounterId As Integer = Nothing
        If GetAdmindbSoftValue("ITEMCOUNTER", "N") = "Y" Then
            Select Case MessageBox.Show("Do you want to change counter for selected items", "Counter Change Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                Case Windows.Forms.DialogResult.Yes
                    NewCounterId = Val(BrighttechPack.SearchDialog.Show("Search Item Counter", "SELECT ITEMCTRID ID,ITEMCTRNAME COUNTER FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY COUNTER", cn, 1, 0))
                    If NewCounterId = 0 Then
                        Exit Sub
                    End If
                Case Windows.Forms.DialogResult.No
                    Exit Select
                Case Else
                    Exit Sub
            End Select
        End If
        Dim RowOrderToRegular As DataRow = Nothing
        Dim dtOrderToRegular As New DataTable
        dtOrderToRegular = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "OrderToRegular", cn)
        dtOrderToRegular.TableName = "OrderToRegular"
        Dim dtReceiptDetail As New DataTable
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For Each Row As DataRow In roSelected
                StrSql = " SELECT IR.* FROM " & cnadmindb & "..ORIRDETAIL AS IR"
                StrSql += " INNER JOIN " & cnAdminDb & "..ORMAST AS O ON O.SNO = IR.ORSNO AND O.SNO = '" & Row.Item("ORSNO").ToString & "'"
                StrSql += " WHERE IR.ORSTATUS = 'R'"
                dtReceiptDetail = New DataTable("OrderToRegular")
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtReceiptDetail)
                'If Not dtReceiptDetail.Rows.Count > 0 Then
                '    Throw New Exception("Receipt Detail Not Found" + vbCrLf + "ORDER NO : " & Row.Item("ORDERNO").ToString)
                'End If
                If dtReceiptDetail.Rows.Count > 0 Then
                    RowOrderToRegular = dtOrderToRegular.NewRow
                    For Each dtCol As DataColumn In dtOrderToRegular.Columns
                        RowOrderToRegular(dtCol.ColumnName) = dtReceiptDetail.Rows(0).Item(dtCol.ColumnName)
                    Next
                    RowOrderToRegular.Item("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
                    RowOrderToRegular.Item("USERID") = userId
                    RowOrderToRegular.Item("UPTIME") = Date.Now.ToLongTimeString
                    StrSql = InsertQry(RowOrderToRegular, cnStockDb)
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
                End If

                StrSql = vbCrLf + " DECLARE @ORSNO VARCHAR(15)"
                StrSql += vbCrLf + " SELECT @ORSNO = '" & Row.Item("ORSNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO = '',ORSNO = '' WHERE ORSNO = @ORSNO"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ESTISSUE SET ORDERNO = '',ORSNO = '' WHERE ORSNO = @ORSNO"
                StrSql += vbCrLf + " DELETE FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSNO = @ORSNO AND ORSTATUS = 'R'"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))

                If NewCounterId <> 0 Then
                    StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & Row.Item("SNO").ToString & "')>0"
                    StrSql += "     BEGIN"
                    StrSql += "     UPDATE " & cnAdminDb & "..CTRANSFER SET ISSDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                    StrSql += "     WHERE TAGSNO = '" & Row.Item("SNO").ToString & "'"
                    StrSql += "     AND ISSDATE IS NULL"
                    StrSql += "     END"
                    StrSql += " ELSE"
                    StrSql += "     BEGIN"
                    StrSql += "     INSERT INTO " & cnAdminDb & "..CTRANSFER"
                    StrSql += "     (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                    StrSql += "     ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID)"
                    StrSql += "     SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                    StrSql += "     ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & GetEntryDate(GetServerDate(tran), tran) & "' AS ISSDATE"
                    StrSql += "     ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,1,COSTID"
                    StrSql += "     FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & Row.Item("SNO").ToString & "'"
                    StrSql += "     END"
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))

                    StrSql = vbCrLf + " DECLARE @ENTORDER INT"
                    StrSql += vbCrLf + " SELECT @ENTORDER = ISNULL(MAX(ENTORDER),0)+1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & Row.Item("SNO").ToString & "'"
                    StrSql += " INSERT INTO " & cnAdminDb & "..CTRANSFER"
                    StrSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,TAGVAL,USERID"
                    StrSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID)"
                    StrSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                    StrSql += " ,ITEMID,TAGNO," & NewCounterId & ",'" & GetEntryDate(GetServerDate(tran), tran) & "' AS RECDATE"
                    StrSql += " ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "' UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,@ENTORDER,COSTID"
                    StrSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & Row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))

                    StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ITEMCTRID = " & NewCounterId & ""
                    StrSql += " ,TRANSFERDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                    StrSql += " WHERE SNO = '" & Row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
                End If
            Next
            tran.Commit()
            MsgBox("Successfully Transfered", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub
End Class