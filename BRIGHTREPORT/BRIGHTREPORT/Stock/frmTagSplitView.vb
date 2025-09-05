Imports System.Data.OleDb
Public Class frmTagSplitView
    Dim dt As New DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        gridheader.DataSource = Nothing
        lblTitle.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmTagSplitView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridCancelReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
        End If
    End Sub
    Private Sub frmTagSplitView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.Value = GetServerDate(tran)
        dtpTo.Value = GetServerDate(tran)
        dtpFrom.Focus()
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridheader)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridheader)
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnView_Search.Enabled = False
        gridView.DataSource = Nothing
        strSql = "  SELECT CONVERT(VARCHAR(12),ISSDATE,103)REFDATE,"
        strSql += vbCrLf + "(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=X.USERID )USERNAME,"
        strSql += vbCrLf + "OLDTAGNO,SUM(OLDPCS)OLDPCS,SUM(OLDGRSWT)OLDGRSWT,SUM(OLDNETWT)OLDNETWT,SUM(OLDDIAWT)OLDDIAWT,SUM(OLDSTNWT)OLDSTNWT,"
        strSql += vbCrLf + "TAGNO,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAWT)DIAWT,SUM(STNWT)STNWT"
        strSql += vbCrLf + "FROM ("
        strSql += vbCrLf + "SELECT OLDTAGNO,ISSDATE,USERID, SUM(PCS)OLDPCS,SUM(GRSWT) OLDGRSWT,SUM(NETWT)OLDNETWT, "
        strSql += vbCrLf + "(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = I.SNO "
        strSql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )AS OLDDIAWT,"
        strSql += vbCrLf + "(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = I.SNO "
        strSql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('S','P')) )AS OLDSTNWT"
        strSql += vbCrLf + ",NULL TAGNO,NULL PCS,NULL GRSWT,NULL NETWT,NULL AS DIAWT,NULL AS STNWT"
        strSql += vbCrLf + "FROM " & cnAdminDb & "..ITEMTAG I"
        strSql += vbCrLf + "WHERE NARRATION ='SPLITTED'"
        strSql += vbCrLf + "GROUP BY OLDTAGNO,ISSDATE,USERID,SNO"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT  OLDTAGNO,ISSDATE,USERID,NULL AS OLDPCS,NULL  AS OLDGRSWT,NULL AS OLDNETWT,NULL AS OLDDIAWT,NULL AS OLDSTNWT "
        strSql += vbCrLf + ",TAGNO,PCS,GRSWT,NETWT,"
        strSql += vbCrLf + "(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = I.SNO "
        strSql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )AS DIAWT,"
        strSql += vbCrLf + "(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = I.SNO "
        strSql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('S','P')) )AS STNWT"
        strSql += vbCrLf + "FROM " & cnAdminDb & "..ITEMTAG I "
        strSql += vbCrLf + "WHERE NARRATION ='SPLITTED'"
        strSql += vbCrLf + ")X"
        strSql += vbCrLf + " WHERE 1=1"
        If txttagno_OWN.Text <> "" Then
            strSql += vbCrLf + " AND OLDTAGNO='" & txttagno_OWN.Text & "' "
        End If
        strSql += vbCrLf + " AND ISSDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " GROUP BY OLDTAGNO,TAGNO,ISSDATE,USERID"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            gridformat()
            AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
            Dim TIT As String
            TIT = " TAG SPLIT VIEW ON " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO " & dtpTo.Value.ToString("dd-MM-yyyy") & ""
            If txttagno_OWN.Text <> "" Then
                TIT = TIT + "  TAGNO AT " & txttagno_OWN.Text & ""
            End If
            lblTitle.Text = TIT
            lblTitle.Visible = True
            gridView.Focus()
        Else
            gridView.DataSource = Nothing
            MsgBox("No Information Found.", MsgBoxStyle.Information)
            dtpFrom.Focus()
            lblTitle.Visible = False
        End If
        btnView_Search.Enabled = True
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Function gridformat()
        GridViewHeaderStyle()

        For I As Integer = dt.Rows.Count - 1 To 1 Step -1
            With dt
                ' If IsDBNull(.Rows(I).Item("OLDTAGNO")) = IsDBNull(.Rows(I - 1).Item("OLDTAGNO")) And .Rows(I).Item("REFDATE").ToString = .Rows(I - 1).Item("REFDATE").ToString Then
                '.Rows(I).Item("USERNAME") = ""
                'End If
                If .Rows(I).Item("OLDTAGNO").ToString = .Rows(I - 1).Item("OLDTAGNO").ToString Then
                    .Rows(I).Item("OLDTAGNO") = ""
                    .Rows(I).Item("USERNAME") = ""
                    .Rows(I).Item("REFDATE") = ""
                End If
            End With
        Next
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        gridView.Columns("OLDTAGNO").HeaderText = "TAGNO"
        gridView.Columns("REFDATE").HeaderText = "DATE"
        gridView.Columns("USERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        gridView.Columns("OLDPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDPCS").HeaderText = "PCS"
        gridView.Columns("OLDGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDGRSWT").HeaderText = "GRSWT"
        gridView.Columns("OLDNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDNETWT").HeaderText = "NETWT"
        gridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDDIAWT").HeaderText = "DIAWT"
        gridView.Columns("OLDSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("OLDSTNWT").HeaderText = "STNWT"
        gridView.Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        funcColWidth()
        FormatGridColumns(gridView, False, False, False, False)
        FormatGridColumns(gridheader, False, False, False, False)
        'For Each dv As DataGridViewRow In gridView.Rows
        '    With dv
        '        Select Case .Cells("OLDTAGNO").Value.ToString
        '            Case ""
        '                .DefaultCellStyle.BackColor = Color.LightYellow
        '            Case Is <> ""
        '                .DefaultCellStyle.BackColor = Color.Lavender
        '        End Select
        '    End With
        'Next
    End Function

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            AutoResizeToolStripMenuItem.Checked = True
            If AutoResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            AutoResizeToolStripMenuItem.Checked = False
        End If
        GridViewHeaderStyle()
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("REFDATE~USERNAME", GetType(String))
            .Columns.Add("OLDTAGNO~OLDPCS~OLDGRSWT~OLDNETWT~OLDDIAWT~OLDSTNWT", GetType(String))
            .Columns.Add("TAGNO~PCS~GRSWT~NETWT~DIAWT~STNWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))

            .Columns("REFDATE~USERNAME").Caption = "PARTICULARS"
            .Columns("OLDTAGNO~OLDPCS~OLDGRSWT~OLDNETWT~OLDDIAWT~OLDSTNWT").Caption = "OLD TAG DATAILS"
            .Columns("TAGNO~PCS~GRSWT~NETWT~DIAWT~STNWT").Caption = "NEW TAG DETAILS"
            .Columns("SCROLL").Caption = ""
        End With
        With gridheader
            .DataSource = dtMergeHeader
            .Columns("REFDATE~USERNAME").HeaderText = "PARTICULARS"
            .Columns("OLDTAGNO~OLDPCS~OLDGRSWT~OLDNETWT~OLDDIAWT~OLDSTNWT").HeaderText = "OLD TAG DETAILS"
            .Columns("TAGNO~PCS~GRSWT~NETWT~DIAWT~STNWT").HeaderText = "NEW TAG DETAILS"
            .Columns("SCROLL").HeaderText = ""
            '.Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Function funcColWidth() As Integer
        With gridheader
            .Columns("REFDATE~USERNAME").Width = _
            IIf(gridView.Columns("REFDATE").Visible, gridView.Columns("REFDATE").Width, 0) _
            + IIf(gridView.Columns("USERNAME").Visible, gridView.Columns("USERNAME").Width, 0)
            .Columns("REFDATE~USERNAME").HeaderText = "PARTICULARS"

            .Columns("OLDTAGNO~OLDPCS~OLDGRSWT~OLDNETWT~OLDDIAWT~OLDSTNWT").Width = _
            IIf(gridView.Columns("OLDTAGNO").Visible, gridView.Columns("OLDTAGNO").Width, 0) _
            + IIf(gridView.Columns("OLDPCS").Visible, gridView.Columns("OLDPCS").Width, 0) _
            + IIf(gridView.Columns("OLDGRSWT").Visible, gridView.Columns("OLDGRSWT").Width, 0) _
            + IIf(gridView.Columns("OLDNETWT").Visible, gridView.Columns("OLDNETWT").Width, 0) _
            + IIf(gridView.Columns("OLDDIAWT").Visible, gridView.Columns("OLDDIAWT").Width, 0) _
            + IIf(gridView.Columns("OLDSTNWT").Visible, gridView.Columns("OLDSTNWT").Width, 0)
            .Columns("OLDTAGNO~OLDPCS~OLDGRSWT~OLDNETWT~OLDDIAWT~OLDSTNWT").HeaderText = "OLD TAG DETAILS"

            .Columns("TAGNO~PCS~GRSWT~NETWT~DIAWT~STNWT").Width = _
            IIf(gridView.Columns("TAGNO").Visible, gridView.Columns("TAGNO").Width, 0) _
          + IIf(gridView.Columns("PCS").Visible, gridView.Columns("PCS").Width, 0) _
          + IIf(gridView.Columns("GRSWT").Visible, gridView.Columns("GRSWT").Width, 0) _
          + IIf(gridView.Columns("NETWT").Visible, gridView.Columns("NETWT").Width, 0) _
          + IIf(gridView.Columns("DIAWT").Visible, gridView.Columns("DIAWT").Width, 0) _
          + IIf(gridView.Columns("STNWT").Visible, gridView.Columns("STNWT").Width, 0)
            .Columns("TAGNO~PCS~GRSWT~NETWT~DIAWT~STNWT").HeaderText = "NEW TAG DETAILS"

        End With
    End Function

    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridheader.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub

    Private Sub gridheader_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridheader.CellContentClick

    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class
