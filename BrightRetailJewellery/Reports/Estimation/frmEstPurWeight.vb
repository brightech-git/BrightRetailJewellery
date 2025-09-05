Imports System.Data.OleDb
Public Class frmEstPurWeight
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
            strSql += "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METAL"
            strSql += " ,TRANNO,TRANDATE,PCS,GRSWT"
            strSql += " ,TRANDATE TTRANDATE"
            strSql += " FROM " & cnStockDb & "..ESTWEIGHT AS I"
            strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND COMPANYID = '" & strCompanyId & "'"
            If cmbMetal.Text <> "ALL" Then
                strSql += " AND ISNULL(METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME ='" & cmbMetal.Text & "')"
            End If
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

            lstColumn.Add("METAL")
            sumColumn.Add("PCS")
            sumColumn.Add("GRSWT")

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
            gridView.Columns("TTRANDATE").Visible = False
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
            Dim EstWtTranno As Integer = Val(gridView.CurrentRow.Cells("TRANNO").Value.ToString)
            Dim EstWtBillDate As DateTime = gridView.CurrentRow.Cells("TTRANDATE").Value.ToString
            PRINT40COLSUMMARY(EstWtTranno, EstWtBillDate)
        End If
    End Sub

    Private Sub PRINT40COLSUMMARY(ByVal billno As Integer, ByVal billdate As DateTime)

        Dim dtestwt As New DataTable
        strSql = " SELECT * FROM " & cnStockDb & "..ESTWEIGHT WHERE "
        strSql += " TRANDATE='" & billdate.ToString("yyyy-MM-dd") & "'"
        strSql += " AND TRANNO='" & billno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtestwt)

        If dtestwt.Rows.Count > 0 Then
            Dim estMetalName As String = GetSqlValue(cn, "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID='" & dtestwt.Rows(0).Item("METALID").ToString & "'")
            If IO.File.Exists(Application.StartupPath & "\ESTWTPRINT.TXT") Then
                IO.File.Delete(Application.StartupPath & "\ESTWTPRINT.TXT")
            End If
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\ESTWTPRINT.TXT")
            Dim lineprn As String = Space(50)
            write.WriteLine("")
            write.WriteLine("")
            write.WriteLine(Space(10) & "ESTIMATION WEIGHT SLIP")
            write.WriteLine("")
            write.WriteLine("------------------------------------------")
            write.WriteLine("EST DATE :" + billdate.ToString("dd-MM-yyyy") + RSet("EST NO :" & billno.ToString, 20))
            write.WriteLine("------------------------------------------")
            Dim SName As String = "METAL  : " & estMetalName.ToString
            Dim PCS As String = "PCS    : " & IIf(Val(dtestwt.Rows(0).Item("PCS").ToString) <> 0, Val(dtestwt.Rows(0).Item("PCS").ToString), "")
            Dim Grswt As String = "GRSWT  : " & dtestwt.Rows(0).Item("GRSWT").ToString & " gms"
            SName = LSet(SName, 30)
            Grswt = LSet(Grswt, 30)
            write.WriteLine(Space(5) & SName)
            write.WriteLine(Space(5) & PCS)
            write.WriteLine(Space(5) & Grswt)
            write.WriteLine("------------------------------------------")

            For i As Integer = 0 To 6
                write.WriteLine()
            Next
            write.WriteLine("i")
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\ESTWTPRINT.BAT") Then
                IO.File.Delete(Application.StartupPath & "\ESTWTPRINT.BAT")
            End If
            Dim writebat As IO.StreamWriter

            Dim PrnName As String = ""
            Dim CondId As String = ""
            Try
                CondId = "'ESTWTPRINT" & Environ("NODE-ID").ToString & "'"
            Catch ex As Exception
                MsgBox("Set Node-Id", MsgBoxStyle.Information)
                Exit Sub
            End Try
            writebat = IO.File.CreateText(Application.StartupPath & "\ESTWTPRINT.BAT")
            strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
            Dim dt As New DataTable
            dt = GetSqlTable(strSql, cn)
            If dt.Rows.Count <> 0 Then
                PrnName = dt.Rows(0).Item("CTLTEXT").ToString
            Else
                'Dim settings As Printing.PrinterSettings = New Printing.PrinterSettings()
                'PrnName = settings.PrinterName
                Dim compname As String = Environment.MachineName
                PrnName = "\\" & compname & "\EST"
            End If
            writebat.WriteLine("TYPE " & Application.StartupPath & "\ESTWTPRINT.TXT>" & PrnName)
            writebat.Flush()
            writebat.Close()
            Shell(Application.StartupPath & "\ESTWTPRINT.BAT")
        End If
    End Sub
    Function funcTitle() As Integer
        lblTitle.Text = ""
        lblTitle.Text += "PURCHASE WEIGHT ESTIMATION DETAILS"
        lblTitle.Text += " FROM " + dtpFrom.Value.ToString("dd/MM/yyyy") + " TO " + dtpTo.Value.ToString("dd/MM/yyyy")
        lblTitle.Refresh()
    End Function

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
End Class