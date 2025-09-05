Imports System.Data.OleDb
Public Class frmMetalVscostcentre
#Region "VARIABLE"
    Dim strsql As String
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim dtItemName As New DataTable
#End Region
#Region "USER DEFINE FUNCTION "
    Function gridviewHeader() As Integer
        Dim dtHeader As New DataTable
        With dtHeader
            .Columns.Add("COSTID~COSTNAME", GetType(String))
            For i As Integer = 2 To gridView.Columns.Count - 1 Step 3
                Dim name As String = gridView.Columns(i).Name.ToString & "~" & gridView.Columns(i + 1).Name.ToString & "~" & gridView.Columns(i + 2).Name.ToString
                .Columns.Add(name, GetType(String))
            Next
            .Columns.Add("SCROLL", GetType(String))
        End With
        With gridHead
            .DataSource = Nothing
            .DataSource = dtHeader
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("COSTID~COSTNAME").HeaderText = "PARTICULAR"
            .Columns("COSTID~COSTNAME").Width = gridView.Columns(0).Width + gridView.Columns(1).Width
            For i As Integer = 2 To gridView.Columns.Count - 1 Step 3
                Dim name As String = gridView.Columns(i).Name.ToString & "~" & gridView.Columns(i + 1).Name.ToString & "~" & gridView.Columns(i + 2).Name.ToString
                .Columns(name).Width = gridView.Columns(i).Width + gridView.Columns(i + 1).Width + gridView.Columns(i + 2).Width
                If Not name.Contains("COSTID") Then
                    .Columns(name).HeaderText = Mid(name, name.LastIndexOf("_") + 2)
                Else
                    .Columns(name).HeaderText = "PARTICULAR"
                End If
            Next
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
            .Columns("SCROLL").HeaderText = ""
        End With
    End Function
    Function gridviewHeaderNew() As Integer
        Dim dtHeader As New DataTable
        With dtHeader
            .Columns.Add("COSTNAME", GetType(String))
            For i As Integer = 1 To gridView.Columns.Count - 1 Step 8
                Dim name As String = gridView.Columns(i).Name.ToString & "~" & gridView.Columns(i + 1).Name.ToString & "~" & gridView.Columns(i + 2).Name.ToString _
                   & "~" & gridView.Columns(i + 3).Name.ToString & "~" & gridView.Columns(i + 4).Name.ToString & "~" & gridView.Columns(i + 5).Name.ToString _
                   & "~" & gridView.Columns(i + 6).Name.ToString & "~" & gridView.Columns(i + 7).Name.ToString
                .Columns.Add(name, GetType(String))
            Next
            .Columns.Add("SCROLL", GetType(String))
        End With
        With gridHead
            .DataSource = Nothing
            .DataSource = dtHeader
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("COSTNAME").HeaderText = "PARTICULAR"
            .Columns("COSTNAME").Width = gridView.Columns(0).Width
            For i As Integer = 1 To gridView.Columns.Count - 1 Step 8
                Dim name As String = gridView.Columns(i).Name.ToString & "~" & gridView.Columns(i + 1).Name.ToString & "~" & gridView.Columns(i + 2).Name.ToString _
                   & "~" & gridView.Columns(i + 3).Name.ToString & "~" & gridView.Columns(i + 4).Name.ToString & "~" & gridView.Columns(i + 5).Name.ToString _
                   & "~" & gridView.Columns(i + 6).Name.ToString & "~" & gridView.Columns(i + 7).Name.ToString
                .Columns(name).Width = gridView.Columns(i).Width + gridView.Columns(i + 1).Width + gridView.Columns(i + 2).Width + gridView.Columns(i + 3).Width _
                     + gridView.Columns(i + 4).Width + gridView.Columns(i + 5).Width + gridView.Columns(i + 6).Width + gridView.Columns(i + 7).Width
                If Not name.Contains("COSTNAME") Then
                    If name.Contains("PS_") Then
                        .Columns(name).HeaderText = "PARTLY SALE"
                    ElseIf name.Contains("PU_") Then
                        .Columns(name).HeaderText = "PURCHASE"
                    ElseIf name.Contains("SR_") Then
                        .Columns(name).HeaderText = "SALES RETURN"
                    Else
                        .Columns(name).HeaderText = ""
                    End If
                Else
                    .Columns(name).HeaderText = "PARTICULAR"
                End If
            Next
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                If gridView.Columns(cnt).HeaderText.Contains("PS_") Then
                    gridView.Columns(cnt).HeaderText = gridView.Columns(cnt).HeaderText.Replace("PS_", "")
                ElseIf gridView.Columns(cnt).HeaderText.Contains("PU_") Then
                    gridView.Columns(cnt).HeaderText = gridView.Columns(cnt).HeaderText.Replace("PU_", "")
                ElseIf gridView.Columns(cnt).HeaderText.Contains("SR_") Then
                    gridView.Columns(cnt).HeaderText = gridView.Columns(cnt).HeaderText.Replace("SR_", "")
                End If
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
            .Columns("SCROLL").HeaderText = ""
        End With
    End Function
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnView_Search_Click(sender As Object, e As EventArgs) Handles btnView_Search.Click
        gridView.DataSource = Nothing
        gridHead.DataSource = Nothing
        Dim strCompanyid As String = ""
        Dim strmetalid As String = ""
        If CmbCompany.Text <> "ALL" And CmbCompany.Text.Trim <> "" Then
            strsql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany.Text & "'"
            strCompanyid = GetSqlValue(cn, strsql)
        End If
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
            strsql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'"
            strmetalid = GetSqlValue(cn, strsql)
        End If
        If rbtTag.Checked Then
            strsql = vbCrLf + "EXEC " & cnAdminDb & "..SP_RPT_METALVSCOSTCENTRE"
            strsql += vbCrLf + "@DBNAME = '" & cnStockDb & "' "
            strsql += vbCrLf + ",@ADMINDB = '" & cnAdminDb & "'"
            strsql += vbCrLf + ",@ASONDATE = '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + ",@FILTERSTRCOMPANYID = '" & strCompanyid & "'"
            strsql += vbCrLf + ",@FILTERCOSTNAME= '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strsql += vbCrLf + ",@FILTERMETALID= '" & strmetalid & "'"
            strsql += vbCrLf + ",@FILTERCATCODE = ''"
            strsql += vbCrLf + ",@FILTERITEMNAME= ''"
            strsql += vbCrLf + ",@NODEID=''"
            strsql += vbCrLf + ",@TAGANDNONTAG = ''"
            strsql += vbCrLf + ",@WITHCUMMULATIVE = '" & IIf(chkwithCummulative.Checked = True, "Y", "N") & "' "
            strsql += vbCrLf + ",@CARAT = 'N' "
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = "SELECT * FROM  TEMPTABLEDB..TEMPFINAL3 ORDER BY COSTID "
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridView, False, False, True, False)
                    For col As Integer = 0 To gridView.Columns.Count - 1
                        If gridView.Columns(col).ToString.Contains("PCS") Then
                            gridView.Columns(col).HeaderText = "PCS"
                            gridView.Columns(col).DefaultCellStyle.BackColor = Color.LightBlue
                        ElseIf gridView.Columns(col).ToString.Contains("GRSWT") Then
                            gridView.Columns(col).HeaderText = "GRSWT"
                            gridView.Columns(col).DefaultCellStyle.BackColor = Color.LightPink
                        ElseIf gridView.Columns(col).ToString.Contains("NETWT") Then
                            gridView.Columns(col).HeaderText = "NETWT"
                            gridView.Columns(col).DefaultCellStyle.BackColor = Color.Lavender
                        End If
                    Next
                    gridAutoSize(gridView)
                    gridviewHeader()
                    lblTitle.Text = "AS ON " & Format(dtpFrom.Value.Date, "dd-MM-yyyy") & " METAL & COSTWISE REPORT " & CmbCompany.Text & " " & chkCmbCostCentre.Text
                    dtpFrom.Focus()
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
                gridHead.DataSource = Nothing
                dtpFrom.Focus()
            End If
        ElseIf rbtPUSRPS.Checked Then
            strsql = vbCrLf + "EXEC " & cnAdminDb & "..SP_RPT_BRANCHPENDINGSTK"
            strsql += vbCrLf + "@TRANDB = '" & cnStockDb & "' "
            strsql += vbCrLf + ",@FROMDATE = '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + ",@TODATE = '" & Format(dtpTodate.Value.Date, "yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            strsql = vbCrLf + " SELECT COSTNAME"
            strsql += vbCrLf + " ,PS_GOLD_PCS,PS_GOLD_GRSWT,PS_SILVER_PCS,PS_SILVER_GRSWT,PS_PLATINUM_PCS,PS_PLATINUM_GRSWT,PS_DIAMOND_PCS,PS_DIAMOND_GRSWT"
            strsql += vbCrLf + " ,PU_GOLD_PCS,PU_GOLD_GRSWT,PU_SILVER_PCS,PU_SILVER_GRSWT,PU_PLATINUM_PCS,PU_PLATINUM_GRSWT,PU_DIAMOND_PCS,PU_DIAMOND_GRSWT"
            strsql += vbCrLf + " ,SR_GOLD_PCS,SR_GOLD_GRSWT,SR_SILVER_PCS,SR_SILVER_GRSWT,SR_PLATINUM_PCS,SR_PLATINUM_GRSWT,SR_DIAMOND_PCS,SR_DIAMOND_GRSWT"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP_PENDREP "
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridView, False, False, True, False)
                    gridAutoSize(gridView)
                    gridviewHeaderNew()
                    For COL As Integer = 0 To gridView.Columns.Count - 1
                        If gridView.Columns(COL).ToString.Contains("GOLD_") Then
                            gridView.Columns(COL).DefaultCellStyle.BackColor = Color.LightBlue
                            gridView.Columns(COL).HeaderText = gridView.Columns(COL).HeaderText.Replace("GOLD", "G")
                        ElseIf gridView.Columns(COL).ToString.Contains("SILVER") Then
                            gridView.Columns(COL).DefaultCellStyle.BackColor = Color.LightPink
                            gridView.Columns(COL).HeaderText = gridView.Columns(COL).HeaderText.Replace("SILVER", "S")
                        ElseIf gridView.Columns(COL).ToString.Contains("PLATINUM") Then
                            gridView.Columns(COL).DefaultCellStyle.BackColor = Color.LightSeaGreen
                            gridView.Columns(COL).HeaderText = gridView.Columns(COL).HeaderText.Replace("PLATINUM", "P")
                        ElseIf gridView.Columns(COL).ToString.Contains("DIAMOND") Then
                            gridView.Columns(COL).DefaultCellStyle.BackColor = Color.Lavender
                            gridView.Columns(COL).HeaderText = gridView.Columns(COL).HeaderText.Replace("DIAMOND", "D")
                        End If
                    Next
                    lblTitle.Text = "AS ON " & Format(dtpFrom.Value.Date, "dd-MM-yyyy") & " METAL & COSTWISE REPORT " & CmbCompany.Text & " " & chkCmbCostCentre.Text
                    dtpFrom.Focus()
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
                gridHead.DataSource = Nothing
                dtpFrom.Focus()
            End If
        End If
    End Sub

    Private Sub gridAutoSize(ByVal dv As DataGridView)
        With dv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTodate.Value = GetServerDate()
        CmbCompany.Items.Clear()
        CmbCompany.Items.Add("ALL")
        strsql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYID"
        objGPack.FillCombo(strsql, CmbCompany, False)
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strsql = "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  ORDER BY DISPLAYORDER "
        objGPack.FillCombo(strsql, cmbMetal, False)
        gridHead.DataSource = Nothing
        gridView.DataSource = Nothing
        rbtTag.Checked = True
        rbtTag.Focus()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExtToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "FORM EVENTS"
    Private Sub frmMetalVscostcentre_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmMetalVscostcentre_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_Scroll(sender As Object, e As ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub rbtTag_CheckedChanged(sender As Object, e As EventArgs) Handles rbtTag.CheckedChanged
        If rbtTag.Checked Then
            rbtPUSRPS.Checked = False
            lblToDate.Visible = False
            dtpTodate.Visible = False
            lblAsOn.Text = "AS ON"
            chkwithCummulative.Visible = True
            gridHead.DataSource = Nothing
            gridView.DataSource = Nothing
        End If
    End Sub

    Private Sub rbtPUSRPS_CheckedChanged(sender As Object, e As EventArgs) Handles rbtPUSRPS.CheckedChanged
        If rbtPUSRPS.Checked Then
            rbtTag.Checked = False
            lblToDate.Visible = True
            dtpTodate.Visible = True
            lblAsOn.Text = "From"
            chkwithCummulative.Visible = False
            gridHead.DataSource = Nothing
            gridView.DataSource = Nothing
        End If
    End Sub
#End Region

End Class