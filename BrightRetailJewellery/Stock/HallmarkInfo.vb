Imports System.Data.OleDb
Public Class HallmarkInfo
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim hasEdit As Boolean = False
    Dim HallmarkMinLen As Integer = Val(GetAdmindbSoftValue("HALLMARK_MINLEN", 0,, True))
    Private Sub HallmarkInfo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFindTag.Focused Then Exit Sub
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        gridView.DataSource = Nothing
        chkDate.Checked = True
        Me.Refresh()
        chkDate.Focus()
    End Sub

    Private Sub HallmarkInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hasEdit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        StrSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(StrSql, cmbDesigner)
        StrSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(StrSql, cmbNewCounter)

        cmbCostCentre_Man.Items.Clear()
        cmbCostCentre_Man.Items.Add("ALL")
        StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'')='Y' "
        StrSql += " ORDER BY COSTNAME"
        objGPack.FillCombo(StrSql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = "ALL"

        ''CostCentre Checking..
        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", "Y", tran)) = "N" Then
            cmbCostCentre_Man.Items.Clear()
            cmbCostCentre_Man.Text = ""
            cmbCostCentre_Man.Enabled = False
        End If
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
        cmbCostCentre_Man.Text = "ALL"
        chkDate.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        StrSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
        Dim SqlVersion As String = GetSqlValue(cn, StrSql)

        StrSql = vbCrLf + " SELECT T.RECDATE,T.ITEMID,IM.ITEMNAME,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        ''StrSql += vbCrLf + " ,T.HM_BILLNO"
        If Val(SqlVersion) > 8 Then
            StrSql += vbCrLf + " ,(STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnAdminDb & "..ITEMTAGHALLMARK H WHERE (T.SNO =H.TAGSNO) FOR XML PATH ('')), 1, 1, '')) HM_BILLNO"
            StrSql += vbCrLf + " ,T.HM_CENTER"
        Else
            StrSql += vbCrLf + " ,T.HM_BILLNO,T.HM_CENTER"
        End If
        StrSql += vbCrLf + " ,CO.ITEMCTRNAME COUNTER,DE.DESIGNERNAME,T.SNO,T.COSTID,T.TAGVAL "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = T.DESIGNERID"
        StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = T.ITEMCTRID"
        StrSql += vbCrLf + " WHERE "
        If chkDate.Checked Then
            StrSql += vbCrLf + " T.RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Else
            StrSql += vbCrLf + " 1=1"
        End If
        If txtItemId_NUM.Text <> "" Then StrSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId_NUM.Text) & ""
        If txtTagNo.Text <> "" Then StrSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
        If txtLotNo_NUM.Text <> "" Then StrSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
        StrSql += vbCrLf + " AND T.ISSDATE IS NULL"
        If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then StrSql += vbCrLf + " AND T.DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "')"
        If cmbNewCounter.Text <> "ALL" And cmbNewCounter.Text <> "" Then StrSql += vbCrLf + " AND T.ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME  = '" & cmbNewCounter.Text & "')"
        If txtHmBillNo.Text <> "" Then
            StrSql += vbCrLf + " AND T.HM_BILLNO = '" & txtHmBillNo.Text & "'"
        End If
        If txtHmBillCenter.Text <> "" Then
            StrSql += vbCrLf + " AND T.HM_CENTER = '" & txtHmBillCenter.Text & "'"
        End If
        If cmbCostCentre_Man.Text <> "ALL" And cmbCostCentre_Man.Text <> "" Then StrSql += vbCrLf + " AND T.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "')"
        StrSql += vbCrLf + " ORDER BY T.TAGVAL"
        dtGrid = New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        With gridView
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .DataSource = dtGrid
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).ReadOnly = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).Visible = False
            Next
            .Columns("RECDATE").Width = 80
            .Columns("ITEMID").Width = 40
            .Columns("ITEMNAME").Width = 140
            .Columns("TAGNO").Width = 100
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 70
            .Columns("NETWT").Width = 70
            .Columns("DIAPCS").Width = 70
            .Columns("DIAWT").Width = 70
            .Columns("COUNTER").Width = 90
            .Columns("DESIGNERNAME").Width = 120
            .Columns("HM_BILLNO").Width = 200
            .Columns("HM_CENTER").Width = 100

            .Columns("RECDATE").Visible = True
            .Columns("ITEMID").Visible = True
            .Columns("ITEMNAME").Visible = True
            .Columns("TAGNO").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("DIAPCS").Visible = True
            .Columns("DIAWT").Visible = True
            .Columns("COUNTER").Visible = True
            .Columns("DESIGNERNAME").Visible = True
            .Columns("HM_BILLNO").Width = 200
            .Columns("HM_CENTER").Width = 100
            .Columns("HM_BILLNO").Visible = True
            .Columns("HM_CENTER").Visible = True
            .Columns("HM_CENTER").ReadOnly = Not hasEdit
            .Columns("HM_BILLNO").ReadOnly = Not hasEdit
            .Columns("HM_CENTER").ReadOnly = True
            .Columns("HM_BILLNO").ReadOnly = True
            .Columns("TAGVAL").Visible = False

            .Columns("ITEMID").HeaderText = "ID"
            .Columns("DESIGNERNAME").HeaderText = "DESIGNER"
            .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("ITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Select()
            If gridView.CurrentRow IsNot Nothing Then
                gridView.CurrentCell = gridView.CurrentRow.Cells("HM_BILLNO")
            End If
        End With
    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpFrom.Enabled = chkDate.Checked
        dtpTo.Enabled = chkDate.Checked
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        'newly added
        gridView.Columns("HM_CENTER").ReadOnly = True
        gridView.Columns("HM_BILLNO").ReadOnly = True
        ''updateinfo:
        ''        If Not hasEdit Then Exit Sub

        ''        If Val(gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value.ToString.Length) > 0 Then
        ''            If HallmarkMinLen > 0 Then
        ''                If Val(HallmarkMinLen) > Val(gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value.ToString.Length) Then
        ''                    MsgBox("HallmarkNo Length Less Than Minimum Length", MsgBoxStyle.Information)
        ''                    gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value = ""
        ''                    txtFindTag.Select()
        ''                    Exit Sub
        ''                End If
        ''            End If
        ''            Dim Htagsno As String = ""
        ''            Htagsno = GetSqlValue(cn, "SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & Trim(gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value.ToString) & "'")
        ''            If Val(Htagsno.Length) > 0 Then
        ''                Dim Htagrow As DataRow
        ''                Htagrow = Nothing
        ''                StrSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
        ''                StrSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
        ''                StrSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
        ''                StrSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
        ''                StrSql += " SNO='" & Htagsno.ToString & "'"
        ''                Htagrow = GetSqlRow(StrSql, cn, Nothing)
        ''                If Not Htagrow Is Nothing Then
        ''                    MsgBox("HallmarkNo Already Exist" _
        ''                        & IIf(Htagrow!Costname.ToString <> "", vbCrLf & " Branch : " & Htagrow!Costname.ToString, "") _
        ''                        & vbCrLf & " Itemname : " & Htagrow!ITEMNAME.ToString & vbCrLf & " Recdate : " & Htagrow!RECDATE.ToString _
        ''                        & vbCrLf & " Itemid : " & Htagrow!ITEMID.ToString & vbCrLf & " Tagno : " & Htagrow!TAGNO.ToString _
        ''                        , MsgBoxStyle.Information)
        ''                    gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value = ""
        ''                    txtFindTag.Select()
        ''                    Exit Sub
        ''                End If
        ''            End If
        ''        End If
        ''        Try
        ''            tran = Nothing
        ''            tran = cn.BeginTransaction
        ''            StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET"
        ''            StrSql += " HM_BILLNO = '" & Trim(gridView.Rows(e.RowIndex).Cells("HM_BILLNO").Value.ToString) & "'"
        ''            StrSql += " ,HM_CENTER = '" & Trim(gridView.Rows(e.RowIndex).Cells("HM_CENTER").Value.ToString) & "'"
        ''            StrSql += " WHERE SNO = '" & gridView.Rows(e.RowIndex).Cells("SNO").Value.ToString & "'"
        ''            ExecQuery(SyncMode.Stock, StrSql, cn, tran, gridView.Rows(e.RowIndex).Cells("COSTID").Value.ToString)
        ''            tran.Commit()
        ''            tran = Nothing
        ''        Catch ex As Exception
        ''            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        ''        End Try
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.CurrentRow Is Nothing Then
                gridView.Select()
                Exit Sub
            End If
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("HM_BILLNO")
            End If
        ElseIf e.KeyChar = "M" Or e.KeyChar = "m" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            Dim obj As New frmItemTagMiscCharge(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString)
            obj.ShowDialog()
        ElseIf e.KeyChar = "H" Or e.KeyChar = "h" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            Dim _temptagno As String = ""
            Dim obj As New frmItemHallmarkDetalisUpdate(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString)
            obj.ShowDialog()
            obj.lblTagWt.Focus()
            _temptagno = gridView.Rows(gridView.CurrentRow.Index).Cells("TAGNO").Value.ToString
            btnSearch_Click(Me, New System.EventArgs)
            txtFindTag.Text = _temptagno.ToString
            tagfind()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtFindTag.Select()
        End If
    End Sub
    Private Sub txtFindTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''If dtGrid Is Nothing Then Exit Sub
            ''If Not gridView.Rows.Count > 0 Then Exit Sub
            ''If txtFindTag.Text = "" Then Exit Sub
            ''Dim Row() As DataRow = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
            ''If Row.Length > 1 Then
            ''    If Val(txtItemId_NUM.Text) = 0 Then
            ''        MsgBox("This tag no exist in multiple items" + vbCrLf + "Please give itemid id in filteration", MsgBoxStyle.Information)
            ''        txtItemId_NUM.Select()
            ''        Exit Sub
            ''    End If
            ''End If
            ''If Row.Length > 0 Then
            ''    'txtFindTag.Clear()
            ''    gridView.CurrentCell = gridView.Rows(Val(Row(0).Item("KEYNO".ToString))).Cells("HM_BILLNO")
            ''    gridView.Select()
            ''    Exit Sub
            ''End If
            tagfind()
        End If
    End Sub

    Private Function tagfind() As Integer
        If dtGrid Is Nothing Then Exit Function
        If Not gridView.Rows.Count > 0 Then Exit Function
        If txtFindTag.Text = "" Then Exit Function
        Dim Row() As DataRow = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
        If Row.Length > 1 Then
            If Val(txtItemId_NUM.Text) = 0 Then
                MsgBox("This tag no exist in multiple items" + vbCrLf + "Please give itemid id in filteration", MsgBoxStyle.Information)
                txtItemId_NUM.Select()
                Exit Function
            End If
        End If
        If Row.Length > 0 Then
            'txtFindTag.Clear()
            gridView.CurrentCell = gridView.Rows(Val(Row(0).Item("KEYNO".ToString))).Cells("HM_BILLNO")
            gridView.Select()
            Exit Function
        End If
    End Function
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Me.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Me.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class