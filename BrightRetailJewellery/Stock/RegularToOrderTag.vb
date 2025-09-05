Imports System.Data.OleDb

Public Class RegularToOrderTag
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim dtorder As New DataTable
    Dim curtagno As String = ""
    Dim curpcs As Double
    Dim curgrswt As Double
    Dim curnetwt As Double
    Dim curdesignerid As String
    Dim curitemid As String
    Dim dtupdate As New DataTable
    Dim XToCostid As String = ""
    Dim XBranchtag As Boolean = False

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub initializevariable()
        gridView.Enabled = True
        gridorderview.Enabled = True
        curtagno = "" : curpcs = 0 : curgrswt = 0 : curnetwt = 0 : curdesignerid = "" : curitemid = ""
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        gridorderview.DataSource = Nothing
        initializevariable()
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtupdate = New DataTable
        txtTagNo.Clear()
        cmbItem.Text = ""
        chkDate.Checked = False
        'Label4.Visible = False
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
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE = 'Y' AND STOCKTYPE = 'T' "
        StrSql += GetItemQryFilteration("S")
        StrSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbItem, False, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        gridorderview.DataSource = Nothing
        Me.Refresh()
        If cmbItem.Text = "" Then MsgBox("ItemName Should Not Empty.") : cmbItem.Focus() : Exit Sub
        Try
            StrSql = vbCrLf + " SELECT T.RECDATE"
            StrSql += vbCrLf + " ,I.ITEMNAME ITEM,T.ITEMID"
            StrSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
            StrSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            StrSql += vbCrLf + " ,T.SNO,T.ITEMCTRID,T.COSTID,T.DESIGNERID"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            StrSql += vbCrLf + " WHERE ISNULL(T.ORDREPNO,'') = '' AND ISNULL(T.ORSNO,'') = ''"
            StrSql += vbCrLf + " AND T.ISSDATE IS NULL AND ISNULL(APPROVAL,'') = ''"
            If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If chkDate.Checked Then StrSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then StrSql += vbCrLf + " AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            If txtTagNo.Text <> "" Then StrSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("CHECK", GetType(Boolean))
            dtGrid.Columns("CHECK").DefaultValue = False
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
                gridView.Columns("ITEMCTRID").Visible = False
                gridView.Columns("COSTID").Visible = False
                gridView.Columns("DESIGNERID").Visible = False
                gridView.Columns("ITEMID").Visible = False
                For i As Integer = 1 To gridView.Columns.Count - 1
                    .Columns(i).ReadOnly = True
                Next
                gridView.Select()
            End With
            StrSql = " SELECT CONVERT(VARCHAR(20),'')TAGNO,O.SNO,O.ORNO,O.ORDATE,I.ITEMNAME,O.PCS,O.GRSWT,O.NETWT,O.DUEDATE,CONVERT(VARCHAR(3),'')DESIGNERID,O.ITEMID FROM " & cnAdminDb & "..ORMAST O"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID=O.ITEMID"
            StrSql += vbCrLf + " WHERE 1=1"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then StrSql += vbCrLf + " AND O.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            StrSql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & cnCostId & "'"
            StrSql += vbCrLf + " AND ISNULL(O.ODBATCHNO,'')='' "
            StrSql += vbCrLf + " AND O.SNO NOT IN(SELECT ISNULL(ORSNO,'') FROM " & cnAdminDb & "..ITEMTAG WHERE 1=1"
            StrSql += vbCrLf + " AND ISSDATE IS NULL AND ISNULL(APPROVAL,'') = ''"
            If Not cnCentStock Then StrSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If chkDate.Checked Then StrSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then StrSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            If txtTagNo.Text <> "" Then StrSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "')"
            StrSql += vbCrLf + " AND ISNULL(O.CANCEL,'') <> 'Y'"
            StrSql += vbCrLf + " AND ISNULL(O.ORDCANCEL,'') <> 'Y'"
            StrSql += vbCrLf + " ORDER BY O.ORDATE,O.DUEDATE"
            dtorder = New DataTable
            dtorder.Columns.Add("CHECK", GetType(Boolean))
            dtorder.Columns("CHECK").DefaultValue = False
            dtorder = GetSqlTable(StrSql, cn)
            If dtorder.Rows.Count > 0 Then
                gridorderview.DataSource = dtorder
                gridorderview.Columns("SNO").Visible = False
                gridorderview.Columns("DESIGNERID").Visible = False
                gridorderview.Columns("ITEMID").Visible = False
            Else
                gridorderview.DataSource = Nothing
                MsgBox("No Order Detail Found.")
            End If
            initializevariable()
            
            dtupdate = New DataTable
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        XToCostid = GetAdmindbSoftValue("SYNC-TO", "")
        XBranchtag = IIf(GetAdmindbSoftValue("BRANCHTAG", "") = "Y", True, False)
        If gridorderview.DataSource Is Nothing Then Exit Sub
        dtupdate = New DataTable
        dtupdate = CType(gridorderview.DataSource, DataTable).Copy
        Dim dv As New DataView
        dv = dtupdate.DefaultView
        dv.RowFilter = ("TAGNO<>''")
        dtupdate = dv.ToTable()
        If Not dtupdate.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For i As Integer = 0 To dtupdate.Rows.Count - 1
                With dtupdate.Rows(i)
                    StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO = '" & .Item("ORNO").ToString & "',ORSNO = '" & .Item("SNO").ToString & "' WHERE TAGNO='" & .Item("TAGNO").ToString & "'"
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
                    InsertOrDetail(i)
                End With
            Next
            tran.Commit()
            MsgBox("Successfully Transfered", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub InsertOrDetail(ByVal cnt As Integer)
        With dtupdate.Rows(cnt)
            Dim ordCompanyId As String = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & .Item("SNO").ToString & "'", , , tran)
            Dim batchno As String = GetNewBatchno(cnCostId, GetEntryDate(System.DateTime.Now.ToString("yyyy-MM-dd"), tran), tran)
            Dim Ord_TranNo As String
            StrSql = " SELECT 1 CNT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & .Item("ORNO").ToString & "' AND ISNULL(ORSNO,'') = '" & .Item("SNO").ToString & "' AND ISNULL(ORSTATUS,'') = 'I' AND ISNULL(CANCEL,'') = ''"
            Dim dtChk As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtChk)
            If dtChk.Rows.Count = 0 Then
                Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)
                StrSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
                StrSql += " ("
                StrSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                StrSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                StrSql += " )"
                StrSql += " VALUES"
                StrSql += " ("
                StrSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                StrSql += " ,'" & .Item("SNO").ToString & "'" 'ORSNO
                StrSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                StrSql += " ,'" & GetEntryDate(System.DateTime.Now.ToString("yyyy-MM-dd"), tran) & "'" 'TRANDATE
                StrSql += " ," & .Item("DESIGNERID").ToString & "" 'DESIGNERID
                StrSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
                StrSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
                StrSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                StrSql += " ,'" & .Item("TAGNO").ToString & "'" 'TAGNO
                StrSql += " ,'I'" 'ORSTATUS
                StrSql += " ,''" 'CANCEL
                StrSql += " ,'" & cnCostId & "'" 'COSTID
                StrSql += " ,'Regular To Order Tag'" 'DESCRIPT
                StrSql += " ,'" & .Item("ORNO").ToString & "'" 'ORNO
                StrSql += " ,'" & batchno & "'" 'BATCHNO
                StrSql += " ," & userId & "" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & ordCompanyId & "'" 'COMPANYID
                StrSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                StrSql += " ,2" 'ORDSTATE_ID
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Item("itemid").ToString) & "", , , tran) & "'" 'CATCODE
                StrSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, XToCostid, , , , , True)
                Else
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
            Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)
            StrSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
            StrSql += " ("
            StrSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
            StrSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
            StrSql += " )"
            StrSql += " VALUES"
            StrSql += " ("
            StrSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            StrSql += " ,'" & .Item("SNO").ToString & "'" 'ORSNO
            StrSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
            StrSql += " ,'" & GetEntryDate(System.DateTime.Now.ToString("yyyy-MM-dd"), tran) & "'" 'TRANDATE
            StrSql += " ," & .Item("DESIGNERID").ToString & "" 'DESIGNERID
            StrSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
            StrSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
            StrSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
            StrSql += " ,'" & .Item("TAGNO").ToString & "'" 'TAGNO
            StrSql += " ,'R'" 'ORSTATUS
            StrSql += " ,''" 'CANCEL
            StrSql += " ,'" & cnCostId & "'" 'COSTID
            StrSql += " ,'Regular To Order Tag'" 'DESCRIPT
            StrSql += " ,'" & .Item("ORNO").ToString & "'" 'ORNO
            StrSql += " ,'" & batchno & "'" 'BATCHNO
            StrSql += " ," & userId & "" 'USERID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,'" & ordCompanyId & "'" 'COMPANYID
            StrSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
            StrSql += " ,4" 'ORDSTATE_ID
            StrSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & "", , , tran) & "'" 'CATCODE
            StrSql += " )"
            If XToCostid <> "" And XBranchtag Then
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, XToCostid, , , , , True)
            Else
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
            End If
            'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , , , False)
            StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()
        End With
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If gridView.RowCount <= 0 Then Exit Sub
        If e.KeyCode = Keys.Space Then
            If gridView.CurrentRow.Cells("CHECK").Value = False Then
                gridView.CurrentRow.Cells("CHECK").Value = True
                Me.gridView.Refresh()
                curtagno = gridView.CurrentRow.Cells("TAGNO").Value.ToString
                curitemid = gridView.CurrentRow.Cells("ITEMID").Value.ToString
                curpcs = gridView.CurrentRow.Cells("PCS").Value.ToString
                curgrswt = gridView.CurrentRow.Cells("GRSWT").Value.ToString
                curnetwt = gridView.CurrentRow.Cells("NETWT").Value.ToString
                curdesignerid = gridView.CurrentRow.Cells("DESIGNERID").Value.ToString
                gridView.Enabled = False
                gridorderview.Enabled = True
                gridorderview.Focus()
            Else
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnTransfer.Focus()
        End If

    End Sub
    Private Sub gridView_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridView.Enter
        If gridView.RowCount > 0 Then
            Label4.Visible = True
        Else
            Label4.Visible = False
        End If
    End Sub

    Private Sub gridorderview_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridorderview.Enter
        If gridorderview.RowCount > 0 Then
            Label4.Visible = True
        Else
            Label4.Visible = False
        End If
    End Sub

    Private Sub gridView_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.Leave
        'Label4.Visible = False
    End Sub

    Private Sub gridorderview_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridorderview.Leave
        'Label4.Visible = False
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick
        If gridView.RowCount <= 0 Then Exit Sub

    End Sub

    Private Sub gridorderview_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridorderview.KeyDown
        If gridorderview.RowCount <= 0 Then Exit Sub
        If e.KeyCode = Keys.Space Then
            If curtagno <> "" And gridorderview.CurrentRow.Cells("TAGNO").Value.ToString = "" Then
                Dim dtsource As New DataTable()
                dtsource = CType(gridorderview.DataSource, DataTable)
                Dim roSelected() As DataRow = dtSource.Select("TAGNO='" & curtagno & "'")
                If roSelected.Length > 0 Then
                    MsgBox("Selected Tagno already alloted ", MsgBoxStyle.Information)
                    gridView.Enabled = True
                    gridorderview.Enabled = False
                    gridView.Focus()
                    gridView.CurrentCell = gridView.CurrentRow.Cells(1)
                    Exit Sub
                End If
                gridorderview.CurrentRow.Cells("TAGNO").Value = curtagno
                gridorderview.CurrentRow.Cells("ITEMID").Value = curitemid
                gridorderview.CurrentRow.Cells("PCS").Value = curpcs
                gridorderview.CurrentRow.Cells("GRSWT").Value = curgrswt
                gridorderview.CurrentRow.Cells("NETWT").Value = curnetwt
                gridorderview.CurrentRow.Cells("DESIGNERID").Value = curdesignerid
                gridView.Enabled = True
                gridorderview.Enabled = False
                gridView.Focus()
                gridView.CurrentCell = gridView.CurrentRow.Cells(1)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnTransfer.Focus()
        End If
    End Sub

    Private Sub txtitemid_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtitemid.Leave
        If Trim(txtitemid.Text) <> "" Then
            Dim itemname As String = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtitemid.Text & "'")
            If itemname <> "" Then
                cmbItem.Text = itemname
                cmbItem.Enabled = False
                txtTagNo.Focus()
            Else
                cmbItem.Text = ""
                cmbItem.Enabled = True
                cmbItem.Focus()
            End If
        End If
    End Sub
End Class