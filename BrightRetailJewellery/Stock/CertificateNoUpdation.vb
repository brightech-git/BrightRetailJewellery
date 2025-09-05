Imports System.Data.OleDb
Public Class CertificateNoUpdation
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim hasEdit As Boolean = False
    Private Sub CertificateNoUpdation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
        cmbUpdationMode.Enabled = True
        Me.Refresh()
        chkDate.Focus()
    End Sub

    Private Sub CertificateNoUpdation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hasEdit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)

        StrSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(StrSql, cmbDesigner)
        StrSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(StrSql, cmbNewCounter)
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate

        cmbUpdationMode.Items.Clear()
        cmbUpdationMode.Items.Add("CERTIFICATENO")
        cmbUpdationMode.Items.Add("STYLENO")
        cmbUpdationMode.Items.Add("ITEMTYPENAME")
        cmbUpdationMode.Items.Add("NARRATION")
        cmbUpdationMode.SelectedIndex = 0

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        StrSql = vbCrLf + " SELECT T.RECDATE,T.ITEMID,IM.ITEMNAME,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"

        If cmbUpdationMode.Text = "CERTIFICATENO" Then
            StrSql += vbCrLf + " ,T.CERTIFICATENO"
        ElseIf cmbUpdationMode.Text = "STYLENO" Then
            StrSql += vbCrLf + " ,T.STYLENO CERTIFICATENO"
        ElseIf cmbUpdationMode.Text = "ITEMTYPENAME" Then
            StrSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) CERTIFICATENO"
        ElseIf cmbUpdationMode.Text = "NARRATION" Then
            StrSql += vbCrLf + " ,T.NARRATION CERTIFICATENO"
        End If

        StrSql += vbCrLf + " ,CO.ITEMCTRNAME COUNTER,DE.DESIGNERNAME,T.SNO,T.COSTID,T.TAGVAL,T.ITEMTYPEID"
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
        StrSql += vbCrLf + " ORDER BY T.TAGVAL"
        dtGrid = New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.CommandTimeout = 1000
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        cmbUpdationMode.Enabled = IIf(hasEdit = False, True, False)

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

            .Columns("TAGNO").Width = 60
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 70
            .Columns("NETWT").Width = 70
            .Columns("DIAPCS").Width = 50
            .Columns("DIAWT").Width = 70
            .Columns("COUNTER").Width = 90
            .Columns("DESIGNERNAME").Width = 120
            .Columns("CERTIFICATENO").Width = 150

            .Columns("CERTIFICATENO").HeaderText = cmbUpdationMode.Text

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
            .Columns("CERTIFICATENO").Visible = True
            .Columns("CERTIFICATENO").ReadOnly = Not hasEdit
            .Columns("TAGVAL").Visible = False

            .Columns("ITEMTYPEID").Visible = False


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
                gridView.CurrentCell = gridView.CurrentRow.Cells("CERTIFICATENO")
            End If
        End With
    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpFrom.Enabled = chkDate.Checked
        dtpTo.Enabled = chkDate.Checked
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not hasEdit Then Exit Sub

        If cmbUpdationMode.Text = "CERTIFICATENO" Then
            UpdateColumn(e.RowIndex, "CERTIFICATENO", "CERTIFICATENO")
        ElseIf cmbUpdationMode.Text = "STYLENO" Then
            UpdateColumn(e.RowIndex, "STYLENO", "STYLENO")
        ElseIf cmbUpdationMode.Text = "ITEMTYPENAME" Then
            'StrSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) CERTIFICATENO"
            UpdateColumn(e.RowIndex, "ITEMTYPEID", "ITEMTYPENAME")
        ElseIf cmbUpdationMode.Text = "NARRATION" Then
            UpdateColumn(e.RowIndex, "NARRATION", "NARRATION")
        End If


        '        StrSql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE CERTIFICATENO = '" & Trim(gridView.Rows(e.RowIndex).Cells("CERTIFICATENO").Value.ToString) & "'"
        '        If Trim(gridView.Rows(e.RowIndex).Cells("CERTIFICATENO").Value.ToString) <> "" Then
        '            If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
        '                Select Case MessageBox.Show("Already exists this certificateno" + vbCrLf + "Do you want to accept then press Yes", "Certificate Duplicate Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        '                    Case Windows.Forms.DialogResult.Yes
        '                        GoTo updateinfo
        '                    Case Else
        '                        gridView.CurrentRow.Cells("CERTIFICATENO").Value = DBNull.Value
        '                        Exit Sub
        '                End Select
        '            End If
        '        End If
        'updateinfo:
        '        Try
        '            tran = Nothing
        '            tran = cn.BeginTransaction

        '            StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET"
        '            StrSql += " CERTIFICATENO = '" & Trim(gridView.Rows(e.RowIndex).Cells("CERTIFICATENO").Value.ToString) & "'"
        '            StrSql += " WHERE SNO = '" & gridView.Rows(e.RowIndex).Cells("SNO").Value.ToString & "'"
        '            ExecQuery(SyncMode.Stock, StrSql, cn, tran, gridView.Rows(e.RowIndex).Cells("COSTID").Value.ToString)
        '            tran.Commit()
        '            tran = Nothing
        '        Catch ex As Exception
        '            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        '        End Try
    End Sub

    Private Sub UpdateColumn(ByVal rwIndex As Integer, ByVal UpdateCol As String, ByVal MsgCol As String)
        Dim gridVal As String = ""
        If cmbUpdationMode.Text = "ITEMTYPENAME" Then
            StrSql = "SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE ISNULL(NAME,'') = '" & Trim(gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value.ToString) & "'"
            gridVal = objGPack.GetSqlValue(StrSql, "ITEMTYPEID", "-1")
            If gridVal = "-1" Then
                MsgBox("Invalid ItemTypeId")
                'gridView.Rows(rwIndex).Cells("CERTIFICATENO").Selected = True
                gridView.CurrentCell = gridView.Rows(rwIndex).Cells("CERTIFICATENO")
                'gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value = dtGrid.Rows(rwIndex).Item("CERTIFICATENO").
                Exit Sub
            End If
        Else
            gridVal = Trim(gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value.ToString)
        End If

        'StrSql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & UpdateCol & " = '" & Trim(gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value.ToString) & "'"
        StrSql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE " & UpdateCol & " = '" & gridVal & "'"
        If Trim(gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value.ToString) <> "" Then
            If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
                Select Case MessageBox.Show("Already exists this " + MsgCol + vbCrLf + "Do you want to accept then press Yes", MsgCol + " Duplicate Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    Case Windows.Forms.DialogResult.Yes
                        GoTo updateinfo
                    Case Else
                        gridView.Rows(rwIndex).Cells(UpdateCol).Value = DBNull.Value
                        Exit Sub
                End Select
            End If
        End If

updateinfo:
        Try
            tran = Nothing
            tran = cn.BeginTransaction

            StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
            'StrSql += UpdateCol & "  = '" & Trim(gridView.Rows(rwIndex).Cells("CERTIFICATENO").Value.ToString) & "'"
            StrSql += UpdateCol & "= '" & gridVal & "'"
            StrSql += " WHERE SNO = '" & gridView.Rows(rwIndex).Cells("SNO").Value.ToString & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, gridView.Rows(rwIndex).Cells("COSTID").Value.ToString)
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
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
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("CERTIFICATENO")
            End If
        End If
    End Sub
    Private Sub txtFindTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFindTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If dtGrid Is Nothing Then Exit Sub
            If Not gridView.Rows.Count > 0 Then Exit Sub
            If txtFindTag.Text = "" Then Exit Sub
            Dim Row() As DataRow = dtGrid.Select("TAGNO = '" & txtFindTag.Text & "'")
            If Row.Length > 1 Then
                If Val(txtItemId_NUM.Text) = 0 Then
                    MsgBox("This tag no exist in multiple items" + vbCrLf + "Please give itemid id in filteration", MsgBoxStyle.Information)
                    txtItemId_NUM.Select()
                    Exit Sub
                End If
            End If
            If Row.Length > 0 Then
                gridView.CurrentCell = gridView.Rows(Val(Row(0).Item("KEYNO".ToString))).Cells("CERTIFICATENO")
                txtFindTag.Clear()
                Exit Sub
            End If
        End If
    End Sub
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