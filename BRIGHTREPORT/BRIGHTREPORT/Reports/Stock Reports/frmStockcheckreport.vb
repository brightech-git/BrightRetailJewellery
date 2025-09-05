Imports System.Data.OleDb
Public Class frmStockcheckreport
    Dim strSql As String
    Dim frmtype As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtcost, dtvcost, dtpre, dtchk, dtsentby, dtrecby, dtsentto, dtdocdes, dtvsentby As DataTable
    Dim serverdate As Date

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Function funcnew()
        gridviewR.DataSource = Nothing
        dtpRfrmdate.Value = Date.Now
        dtpRTodate.Value = Date.Now
        txt_grid.Visible = False
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,0 RESULT UNION ALL "
        strSql += " SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN('" & cnCostId & "')ORDER BY RESULT,COSTNAME"
        dtcost = GetSqlTable(strSql, cn)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtcost, "COSTNAME", , "ALL")
        strSql = " SELECT 'ALL' METALNAME,0 RESULT UNION ALL "
        strSql += " SELECT METALNAME,1 RESULT FROM " & cnAdminDb & "..METALMAST ORDER BY RESULT,METALNAME"
        dtcost = New DataTable
        dtcost = GetSqlTable(strSql, cn)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbMetal, dtcost, "METALNAME", , "ALL")
        dtpRfrmdate.Focus()
    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRexit.Click, ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click, NewToolStripMenuItem1.Click
        funcnew()
    End Sub

    Private Sub Funcview()
        Dim costid As String = "ALL"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            costid = GetSelectedCostId(chkCmbCostCentre, True)
        End If
        Dim TEMPTABLE As String = "TEMPTABLEDB..TEMP" & systemId & "STOCKCHECK"
        strSql = vbCrLf + "     IF OBJECT_ID('" & TEMPTABLE & "')IS NOT NULL DROP TABLE " & TEMPTABLE

        strSql += vbCrLf + "  SELECT CONVERT(VARCHAR(50),SNO)SNO,COSTNAME,USERNAME,CONVERT(VARCHAR,REPORTDATE,105)REPORTDATE"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,CHKFROMDATE,105)CHKFROMDATE"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,CHKTODATE,105)CHKTODATE"
        strSql += vbCrLf + "  ,ITEMNAME,STKPCS,STKWT,CHKPCS,CHKWT,SUM(STKPCS-CHKPCS)DIFFPCS,SUM(STKWT-CHKWT)DIFFWT,REMARK"
        strSql += vbCrLf + "  ,2 RESULT INTO " & TEMPTABLE & " FROM " & cnAdminDb & "..STOCK_CHKREPORT S"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID=S.COSTID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=S.GENERATEDBY"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID=S.ITEMID"
        strSql += vbCrLf + "  WHERE REPORTDATE BETWEEN '" & dtpRfrmdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpRTodate.Value.ToString("yyyy-MM-dd") & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND S.COSTID IN(" & costid & ") "
        If ChkCmbMetal.Text <> "ALL" And ChkCmbMetal.Text <> "" Then
            strSql += vbCrLf + " AND S.ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID "
            strSql += vbCrLf + " IN(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(ChkCmbMetal.Text) & ")))"
        End If
        strSql += vbCrLf + " GROUP BY SNO,COSTNAME,USERNAME,REPORTDATE,CHKFROMDATE,CHKTODATE,ITEMNAME,STKPCS,STKWT,CHKPCS,CHKWT,REMARK "
        If rbtDiffonly.Checked Then strSql += vbCrLf + " HAVING SUM(STKPCS-CHKPCS)<> 0 "
        If rbttallyonly.Checked Then strSql += vbCrLf + " HAVING SUM(STKPCS-CHKPCS)= 0 "
        strSql += vbCrLf + " ORDER BY REPORTDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        strSql = "IF (SELECT COUNT(*) FROM " & TEMPTABLE & " WHERE RESULT=2)>0 BEGIN INSERT INTO  " & TEMPTABLE & "(COSTNAME,STKPCS,STKWT,CHKPCS,CHKWT,DIFFPCS,DIFFWT,RESULT)"
        strSql += vbCrLf + " SELECT 'TOTAL',SUM(STKPCS),SUM(STKWT),SUM(CHKPCS),SUM(CHKWT),SUM(DIFFPCS),SUM(DIFFWT),3 FROM " & TEMPTABLE & " WHERE RESULT=2 END "
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM " & TEMPTABLE & " ORDER BY RESULT"
        Dim dtgrid As New DataTable
        dtgrid = GetSqlTable(strSql, cn)

        strSql = vbCrLf + "IF OBJECT_ID('" & TEMPTABLE & "')IS NOT NULL DROP TABLE " & TEMPTABLE
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

        If dtgrid.Rows.Count > 0 Then
            With gridviewR
                .DataSource = dtgrid
                .Columns("SNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("REMARK").Width = 200
                .Columns("USERNAME").Width = 150
                .Columns("ITEMNAME").Width = 200

                .Columns("STKPCS").Width = 60
                .Columns("STKWT").Width = 100
                .Columns("CHKPCS").Width = 60
                .Columns("CHKWT").Width = 100
                .Columns("DIFFPCS").Width = 60
                .Columns("DIFFWT").Width = 100

                .Columns("REPORTDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("CHKFROMDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("CHKTODATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CHKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CHKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIFFWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                For i As Integer = 0 To gridviewR.RowCount - 1
                    If Val(gridviewR.Rows(i).Cells("RESULT").Value.ToString) = 3 Then
                        gridviewR.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                        gridviewR.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                        gridviewR.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                Next
            End With
        Else
            gridviewR.DataSource = Nothing
            MsgBox("No Record found.", MsgBoxStyle.Information)
            dtpRfrmdate.Focus()
        End If
    End Sub

    
    Private Sub frmSyncCostcentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And txt_grid.Focused = False And gridviewR.Focused = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSyncCostcentre_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcnew()
    End Sub
    Private Sub closeit()
        Me.Close()
    End Sub

    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRview.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        AutoReziseToolStripMenuItem.Checked = False
        Funcview()
    End Sub

    Private Sub gridview_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridviewR.CellEnter
        If gridviewR.Columns(gridviewR.CurrentCell.ColumnIndex).Name = "REMARK" Then
            If gridviewR.CurrentCell.Value Is Nothing Or IsDBNull(gridviewR.CurrentCell.Value) Then : txt_grid.Text = String.Empty
            Else : txt_grid.Text = gridviewR.CurrentCell.Value : End If
            Dim CurrentCellx As Integer = gridviewR.GetCellDisplayRectangle(gridviewR.CurrentCell.ColumnIndex, gridviewR.CurrentRow.Index, False).Left + gridviewR.Left
            Dim CurrentCelly As Integer = gridviewR.GetCellDisplayRectangle(gridviewR.CurrentCell.ColumnIndex, gridviewR.CurrentRow.Index, False).Top + gridviewR.Top
            txt_grid.Location = New Point(CurrentCellx, CurrentCelly)
            txt_grid.Size = New Size(gridviewR.CurrentCell.Size.Width - 1, gridviewR.CurrentCell.Size.Height - 2)
            txt_grid.Show()
            txt_grid.BringToFront()
            txt_grid.Select()
            txt_grid.Focus()
        Else
            txt_grid.Visible = False
        End If
    End Sub

    Private Sub cmbgrid_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_grid.KeyDown
        If e.KeyCode = Keys.Return Then
            e.Handled = True
            If txt_grid.Focused = False Then Exit Sub
            If gridviewR.Columns(gridviewR.CurrentCell.ColumnIndex).Name = "REMARK" Then
                If txt_grid.Text <> String.Empty Then
                    strSql = "UPDATE T SET T.REMARK='" & txt_grid.Text & "'"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..STOCK_CHKREPORT T WHERE SNO='" & gridviewR.CurrentRow.Cells("SNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
                    strSql = "SELECT REMARK FROM " & cnAdminDb & "..STOCK_CHKREPORT T WHERE SNO='" & gridviewR.CurrentRow.Cells("SNO").Value.ToString & "'"
                    Dim dt As New DataTable : dt = GetSqlTable(strSql, cn)
                    If dt.Rows.Count > 0 Then
                        gridviewR.CurrentRow.Cells("REMARK").Value = dt.Rows(0).Item("REMARK").ToString
                    End If
skip:
                    If gridviewR.CurrentRow.Index < gridviewR.RowCount - 1 Then
                        gridviewR.CurrentCell = gridviewR.Item("REMARK", gridviewR.CurrentRow.Index + 1)
                        gridviewR.Item("REMARK", gridviewR.CurrentRow.Index).Selected = True
                    Else
                        txt_grid.Visible = False
                    End If
                Else
                    If gridviewR.CurrentRow.Index < gridviewR.RowCount - 1 Then
                        gridviewR.CurrentCell = gridviewR.Item("REMARK", gridviewR.CurrentRow.Index + 1)
                        gridviewR.Item("REMARK", gridviewR.CurrentRow.Index).Selected = True
                    Else
                        txt_grid.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridview_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        txt_grid.Visible = False
    End Sub
    Private Sub btnRexport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRexport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridviewR.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "Document Maintanence", gridviewR, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridviewR.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "Document Maintanence", gridviewR, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub gridviewR_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridviewR.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            btnRexport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If AutoReziseToolStripMenuItem.Checked = True Then
            AutoReziseToolStripMenuItem.Checked = False
        Else
            AutoReziseToolStripMenuItem.Checked = True
        End If
        If gridviewR.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridviewR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridviewR.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridviewR.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridviewR.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class

