Imports System.Data.OleDb
Public Class frmEstPurchase
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim cMenu As New ContextMenuStrip
    Dim lstColumn As New List(Of String)
    Dim sumColumn As New List(Of String)

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        rbtBoth.Checked = True
        gridView.ContextMenuStrip = Nothing
        dtpFrom.Select()
    End Sub

    Private Sub dgv_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles gridView.ColumnHeaderMouseClick
        Select Case gridView.Columns(e.ColumnIndex).ValueType.FullName
            Case GetType(Int16).FullName, GetType(Int32).FullName, GetType(Int64).FullName, GetType(Integer).FullName, GetType(Decimal).FullName, GetType(Double).FullName
                If sumColumn.Contains(gridView.Columns(e.ColumnIndex).Name) Then
                    sumColumn.Remove(gridView.Columns(e.ColumnIndex).Name)
                Else
                    sumColumn.Add(gridView.Columns(e.ColumnIndex).Name)
                End If
                dgvTStripMenu(Me, New EventArgs)
        End Select
    End Sub

    Private Sub dgvTStripMenu(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cnt As Integer = 0
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                cnt += 1
            End If
        Next
        If cnt > 2 Then
            For Each mm As ToolStripMenuItem In cMenu.Items
                mm.CheckState = CheckState.Unchecked
            Next
            CType(sender, ToolStripMenuItem).CheckState = CheckState.Checked
        End If
        Dim lstColumn As New List(Of String)
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                lstColumn.Add(mm.Text)
            End If
        Next
        gridView.DataSource = Nothing
        Dim dt As New DataTable
        dt = dtGridView.Copy
        If cnt = 0 Then
            gridView.DataSource = dt
            BrighttechPack.GridViewGrouper.StyleGridColumns(gridView, Nothing)
        Else
            gridView.DataSource = dt
            BrighttechPack.GridViewGrouper.GridView_Grouper(gridView, Nothing, lstColumn, sumColumn)
        End If
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnView_Search.Enabled = False
        gridView.DataSource = Nothing
        Try
            dtGridView.Rows.Clear()
            strSql = " SELECT "
            strSql += "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METAL"
            strSql += " ,TRANNO,EMPID,PURITY,PCS,GRSWT,WASTAGE,NETWT,RATE,AMOUNT,BOARDRATE"
            strSql += " ,CASE WHEN FLAG = 'W' THEN 'OWN' ELSE 'OTHER' END AS OWN,TRANDATE,ESTBATCHNO,TRANDATE TTRANDATE,COSTID"
            strSql += " FROM " & cnStockDb & "..ESTRECEIPT AS I"
            strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND COMPANYID = '" & strCompanyId & "'"
            If rbtBilled.Checked Then
                strSql += " AND ISNULL(BATCHNO,'') <> ''"
            ElseIf rbtPending.Checked Then
                strSql += " AND ISNULL(BATCHNO,'') = ''"
            End If
            If cmbMetal.Text <> "ALL" Then
                strSql += " AND ISNULL(METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME ='" & cmbMetal.Text & "')"
            End If
            strSql += " AND TRANTYPE = 'PU'"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            If txtEstNo_NUM.Text <> "" Then
                strSql += " AND ISNULL(TRANNO,'') = '" & Val(txtEstNo_NUM.Text) & "'"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                btnView_Search.Enabled = True
                dtpFrom.Focus()
                Exit Sub
            End If
            gridView.ContextMenuStrip = Nothing
            gridView.DataSource = dtGridView
            gridView.Columns("ESTBATCHNO").Visible = False
            gridView.Columns("TTRANDATE").Visible = False
            gridView.Columns("COSTID").Visible = False

            lstColumn.Add("METAL")
            lstColumn.Add("OWN")
            sumColumn.Add("PCS")
            sumColumn.Add("GRSWT")
            sumColumn.Add("WASTAGE")
            sumColumn.Add("NETWT")
            sumColumn.Add("AMOUNT")

            cMenu.Items.Clear()
            For Each col As DataGridViewColumn In gridView.Columns
                If col.Name = "PARCOL" Then Continue For
                col.SortMode = DataGridViewColumnSortMode.NotSortable
                Dim menu As New ToolStripMenuItem
                menu.Name = col.Name
                menu.Text = col.HeaderText
                menu.CheckOnClick = True
                If lstColumn.Contains(col.HeaderText) Then menu.Checked = True
                AddHandler menu.Click, AddressOf dgvTStripMenu
                cMenu.Items.Add(menu)
            Next
            gridView.ContextMenuStrip = cMenu
            dgvTStripMenu(Me, New EventArgs)
            funcTitle()
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub NEWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NEWToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub EXITToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXITToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub frmEstPurchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmEstPurchase_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_ColumnNameChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnNameChanged

    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
            If gridView.CurrentRow Is Nothing Then Exit Sub
            If gridView.CurrentRow.Cells("TRANNO").Value.ToString = "" Then Exit Sub
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":EST")
                write.WriteLine(LSet("ESTNO", 15) & ":S.0;P." & gridView.CurrentRow.Cells("TRANNO").Value.ToString & "")
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(gridView.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":EST" & ";" & _
                    LSet("ESTNO", 15) & ":S.0;P." & gridView.CurrentRow.Cells("TRANNO").Value.ToString & "" & ";" & _
                    LSet("TRANDATE", 15) & ":" & Format(gridView.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":Y")
                End If

            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        ElseIf UCase(e.KeyChar) = "E" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            Dim objEst As New frmEstimation1
            objEst.Hide()
            objEst.BillDate = GetEntryDate(GetServerDate)
            objEst.lblUserName.Text = cnUserName
            objEst.lblSystemId.Text = systemId
            objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
            objEst.Set916Rate(objEst.BillDate)
            objEst.WindowState = FormWindowState.Minimized
            BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
            objGPack.Validator_Object(objEst)

            objEst.Size = New Size(1032, 745)
            objEst.MaximumSize = New Size(1032, 745)
            objEst.StartPosition = FormStartPosition.Manual
            objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

            objEst.KeyPreview = True
            objEst.MaximizeBox = False
            objEst.StartPosition = FormStartPosition.CenterScreen
            objEst.EditTranNo = gridView.CurrentRow.Cells("TRANNO").Value.ToString
            objEst.EditBatchno = gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString
            objEst.EditTranDate = CType(gridView.CurrentRow.Cells("TTRANDATE").Value.ToString, Date)
            objEst.EditCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
            objEst.btnNew.Enabled = False
            objEst.Show()
            objEst.WindowState = FormWindowState.Normal
        ElseIf UCase(e.KeyChar) = "C" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Red Then
                MessageBox.Show("Estimation No is already canceled...")
            Else
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If

                If MessageBox.Show("Do you want to cancel", "CANCEL", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                    strSql = vbCrLf + " UPDATE " & cnStockDb & "..ESTISSUE SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..ESTRECEIPT SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    If cnCostId <> gridView.CurrentRow.Cells("COSTID").Value.ToString Then
                        Exec(strSql.Replace("'", "''"), cn, gridView.CurrentRow.Cells("COSTID").Value.ToString, Nothing, tran)
                    End If
                    gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Red
                    gridView.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Red
                    MessageBox.Show("Successfully Canceled...")
                End If
            End If
        End If
    End Sub

    Function funcTitle() As Integer
        lblTitle.Text = ""
        lblTitle.Text += "PURCHASE ESTIMATION DETAILS"
        lblTitle.Text += " FROM " + dtpFrom.Value.ToString("dd/MM/yyyy") + " TO " + dtpTo.Value.ToString("dd/MM/yyyy")
        If rbtBilled.Checked Then
            lblTitle.Text += "(ONLY BILLED)"
        ElseIf rbtPending.Checked Then
            lblTitle.Text += "(ONLY PENDING)"
        End If
        lblTitle.Refresh()
    End Function

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
End Class