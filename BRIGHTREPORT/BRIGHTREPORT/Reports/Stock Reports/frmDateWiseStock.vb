Imports System.Data.OleDb
Public Class frmDateWiseStock
#Region "Variable Decalarations"
    Dim strsql As String = Nothing
    Dim dtcompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtMetal As New DataTable
    Dim dtcategory As New DataTable
#End Region

    Private Sub frmDateWiseStock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
        If e.KeyCode = Keys.X Then
            btnExport_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.P Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmDateWiseStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtcompany, "COMPANYNAME", , "ALL")
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strsql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strsql += vbCrLf + " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbMetalType, dtMetal, "METALNAME", , "ALL")
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    'Private Sub chkcmbMetalType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbMetalType.Leave
    '    Dim Qry As String
    '    Dim Metal As String = funcFiltMetalType()
    '    Qry = "SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
    '    Qry += vbCrLf + " UNION ALL"
    '    Qry += vbCrLf + " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE 1=1"
    '    If Metal <> "" Then Qry += vbCrLf + Metal
    '    Qry += vbCrLf + " ORDER BY RESULT,CATNAME"
    '    dtcategory = New DataTable
    '    da = New OleDbDataAdapter(Qry, cn)
    '    da.Fill(dtcategory)
    '    BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtcategory, "CATNAME", , "ALL")
    'End Sub
    Function funcFiltMetalType()
        Dim str As String = ""
        If chkcmbMetalType.Text <> "ALL" And chkcmbMetalType.Text <> "" Then
            str += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkcmbMetalType.Text) & "))"
        End If
        Return str
    End Function

    Private Sub chkcmbMetalType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbMetalType.TextChanged
        Dim Qry As String
        Dim Metal As String = funcFiltMetalType()
        Qry = "SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
        Qry += vbCrLf + " UNION ALL"
        Qry += vbCrLf + " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE 1=1 "
        Qry += vbCrLf + Metal
        Qry += vbCrLf + " ORDER BY RESULT,CATNAME"
        dtcategory = New DataTable
        da = New OleDbDataAdapter(Qry, cn)
        da.Fill(dtcategory)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtcategory, "CATNAME", , "ALL")
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkCmbCompany.Text = "" Then
            MsgBox("Select the Company", MsgBoxStyle.Information)
            chkCmbCompany.Focus()
            Exit Sub
        ElseIf chkcmbMetalType.Text = "" Then
            MsgBox("Select the Metal Type", MsgBoxStyle.Information)
            chkcmbMetalType.Focus()
            Exit Sub
        ElseIf chkcmbCategory.Text = "" Then
            MsgBox("Select the Category", MsgBoxStyle.Information)
            chkcmbCategory.Focus()
            Exit Sub
        ElseIf chkCmbCostCentre.Text = "" Then
            MsgBox("Select the CostCentre", MsgBoxStyle.Information)
            chkCmbCostCentre.Focus()
            Exit Sub
        End If
        Report()
    End Sub
    Private Sub Report()
        Try

            If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
            gridView.DataSource = Nothing
            Dim StrCompany As String = GetQryStringForSp(chkCmbCompany.Text, "" & cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")
            Dim StrMetal As String = GetQryStringForSp(chkcmbMetalType.Text, "" & cnAdminDb & "..METALMAST", "METALID", "METALNAME")
            Dim StrCat As String = GetQryStringForSp(chkcmbCategory.Text, "" & cnAdminDb & "..CATEGORY", "CATCODE", "CATNAME")
            Dim StrCost As String = GetQryStringForSp(chkCmbCostCentre.Text, "" & cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
            Dim dt As New DataTable
            strsql = " EXEC " & cnStockDb & "..SP_RPT_ISSUERECEIPT"
            strsql += vbCrLf + " @FROMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
            strsql += vbCrLf + " @TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
            strsql += vbCrLf + " @COMPANYID='" & StrCompany & "',"
            strsql += vbCrLf + " @METALID='" & StrMetal & "',"
            strsql += vbCrLf + " @COSTID='" & StrCost & "' ,"
            strsql += vbCrLf + " @catid='" & StrCat & "'"
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dt)
            If dt.Rows.Count = 0 Then MsgBox("No Record found", MsgBoxStyle.Information) : gridView.DataSource = Nothing : Exit Sub
            Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dt)
            ObjGrouper.pColumns_Group.Add("METAL")
            ObjGrouper.pColumns_Group.Add("CATEGORY")
            ObjGrouper.pColumns_Sum.Add("RGRSWT")
            ObjGrouper.pColumns_Sum.Add("RPURWT")
            ObjGrouper.pColumns_Sum.Add("RNETWT")
            ObjGrouper.pColumns_Sum.Add("IGRSWT")
            ObjGrouper.pColumns_Sum.Add("IPURWT")
            ObjGrouper.pColumns_Sum.Add("INETWT")
            ObjGrouper.pColumns_Sum.Add("CGRSWT")
            ObjGrouper.pColumns_Sum.Add("CPURWT")
            ObjGrouper.pColumns_Sum.Add("CNETWT")
            ObjGrouper.pColName_Particular = "PARTYNAME"
            ObjGrouper.pColName_ReplaceWithParticular = "PARTYNAME"
            'ObjGrouper.pColName_Particular = "TOUCH"
            ObjGrouper.pColumns_Sort = "PARTYNAME"

            ObjGrouper.GroupDgv()

           
            lblTitle.Text = "Opening Issue and Receipt Details"
            lblTitle.Text += " From " + dtpFrom.Value.ToString("dd/MM/yyyy") + " To " + dtpTo.Value.ToString("dd/MM/yyyy")
            lblTitle.Text += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
            funcGridViewStyle()
            funcGridHeaderText()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            With gridViewHead
                If colWid >= gridView.Width Then
                    .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                    .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                Else
                    .Columns("SCROLL").Visible = False
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Function funcGridViewStyle()
        With gridView

            With .Columns("PARTYNAME")
                .Visible = True
                .HeaderText = " "
                .Width = 250
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("RGRSWT")
                .HeaderText = "Grs Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("IGRSWT")
                .HeaderText = "Grs Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("CGRSWT")
                .HeaderText = "Grs Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("RPURWT")
                .HeaderText = "Pure Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("IPURWT")
                .HeaderText = "Pure Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("CPURWT")
                .HeaderText = "Pure Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("RNETWT")
                .HeaderText = "Net Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("INETWT")
                .HeaderText = "Net Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("CNETWT")
                .HeaderText = "Net Weight"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            If Not chkWithGrsWt.Checked Then
                .Columns("RGRSWT").Visible = False
                .Columns("IGRSWT").Visible = False
                .Columns("CGRSWT").Visible = False
            End If
            If Not chkWithPurWt.Checked Then
                .Columns("RPURWT").Visible = False
                .Columns("IPURWT").Visible = False
                .Columns("CPURWT").Visible = False
            End If
            If Not chkWithNetWt.Checked Then
                .Columns("RNETWT").Visible = False
                .Columns("INETWT").Visible = False
                .Columns("CNETWT").Visible = False
            End If
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function
    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        BrighttechPack.GlobalMethods.FillCombo(chkcmbMetalType, dtMetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtcompany, "COMPANYNAME", , "ALL")
        lblTitle.Text = ""
        dtpFrom.Focus()

    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub
    Function funcGridHeaderText()
        Dim dtheader As New DataTable
        With dtheader
            .Columns.Add("PARTYNAME", Type.GetType("System.String"))
            .Columns.Add("TOUCH", Type.GetType("System.String"))
            .Columns.Add("RGRSWT~RPURWT~RNETWT", Type.GetType("System.String"))
            .Columns.Add("IGRSWT~IPURWT~INETWT", Type.GetType("System.String"))
            .Columns.Add("CGRSWT~CPURWT~CNETWT", Type.GetType("System.String"))
            .Columns.Add("SCROLL", Type.GetType("System.String"))

            .Columns("PARTYNAME").Caption = ""
            .Columns("TOUCH").Caption = ""
            .Columns("RGRSWT~RPURWT~RNETWT").Caption = "Receipt"
            .Columns("IGRSWT~IPURWT~INETWT").Caption = "Issue"
            .Columns("CGRSWT~CPURWT~CNETWT").Caption = "Closing"
            .Columns("SCROLL").Caption = ""
        End With
        gridViewHead.DataSource = dtheader
        HeadGridStyle()
    End Function
    Function HeadGridStyle()
        With gridViewHead
            .Columns("SCROLL").HeaderText = ""
            With .Columns("PARTYNAME")
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("PARTYNAME").Width
                .HeaderText = "Party Name"
            End With
            With .Columns("RGRSWT~RPURWT~RNETWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("RGRSWT").Width + gridView.Columns("RPURWT").Width + gridView.Columns("RNETWT").Width
                .HeaderText = "Receipt"
            End With
            With .Columns("IGRSWT~IPURWT~INETWT")
                .HeaderText = "Issue"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("IGRSWT").Width + gridView.Columns("IPURWT").Width + gridView.Columns("INETWT").Width
            End With
            With .Columns("CGRSWT~CPURWT~CNETWT")
                .HeaderText = "Closing"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("CGRSWT").Width + gridView.Columns("CPURWT").Width + gridView.Columns("CNETWT").Width
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.Columns.Count > 0 Then
            gridViewHead.Columns("PARTYNAME").Width = gridView.Columns("PARTYNAME").Width
            gridViewHead.Columns("RGRSWT~RPURWT~RNETWT").Width = gridView.Columns("RGRSWT").Width + gridView.Columns("RPURWT").Width + gridView.Columns("RNETWT").Width
            gridViewHead.Columns("IGRSWT~IPURWT~INETWT").Width = gridView.Columns("IGRSWT").Width + gridView.Columns("IPURWT").Width + gridView.Columns("INETWT").Width
            gridViewHead.Columns("CGRSWT~CPURWT~CNETWT").Width = gridView.Columns("CGRSWT").Width + gridView.Columns("CPURWT").Width + gridView.Columns("CNETWT").Width
            gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End If
    End Sub
    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridViewHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class