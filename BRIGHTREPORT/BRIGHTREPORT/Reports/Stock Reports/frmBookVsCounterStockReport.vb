Imports System.Data.OleDb
Public Class frmBookVsCounterStockReport
    Dim strSql As String
    Dim dtCompany As New DataTable
    Dim dtOptions As New DataTable
    Dim dtmetal As New DataTable
    Dim dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim ds As DataSet
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridViewHeader.DataSource = Nothing
        gridView.DataSource = Nothing
        gridView.Refresh()
        lblTitle.Visible = False
        funcNew()
        chkCmbCompany.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHeader)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHeader)
    End Sub
    Private Sub funcNew()

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = "  SELECT 'ALL' METALNAME ,NULL AS  DISPLAYORDER, 1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME ,DISPLAYORDER,2 RESULT FROM " & cnAdminDb & ".. METALMAST  WHERE ACTIVE='Y' ORDER BY RESULT , DISPLAYORDER"
        dtmetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtmetal, "METALNAME", , "ALL")
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        lblTitle.Visible = False
        chkCmbCompany.Select()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Dim SelectedCostId As String = GetSelectedCostId(chkCmbCostCentre, False)
        Dim selectedMetal As String = GetSelectedMetalid(chkCmbMetal, False)
        AutoResizeToolStripMenuItem.Checked = False
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_BOOKVSCOUNTERSTK"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@ASONDATE='" & cnTranFromDate & "'"
        strSql += vbCrLf + ",@COMPANYIDS='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTIDS='" & SelectedCostId & "'"
        strSql += vbCrLf + ",@METALIDS='" & selectedMetal & "'"
        strSql += vbCrLf + ",@SYSID = '" & systemId & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        ds = New DataSet()
        dt = New DataTable
        da.Fill(DS)
        dt = ds.Tables(0)
        If dt.Rows.Count <= 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            gridViewHeader.DataSource = Nothing
            chkCmbCompany.Select()
        Else
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            GridViewFormat()
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            lblTitle.Visible = True
            lblTitle.Text = " BOOK VS COUNTER STOCK "
            gridView.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End If
    End Sub
    Private Sub GridViewFormat()
        GridViewHeaderStyle()
        gridView.Columns("BREC").HeaderText = "REC"
        gridView.Columns("BISS").HeaderText = "ISS"
        gridView.Columns("BINTISS").HeaderText = "INTISS"
        gridView.Columns("BBALANCE").HeaderText = "BALANCE"
        gridView.Columns("CREC").HeaderText = "REC"
        gridView.Columns("CISS").HeaderText = "ISS"
        gridView.Columns("CINTISS").HeaderText = "INTISS"
        gridView.Columns("CBALANCE").HeaderText = "BALANCE"
        gridView.Columns("BHS").HeaderText = "HOME SALE"
        gridView.Columns("BPS").HeaderText = "PART SALE"
        gridView.Columns("BSR").HeaderText = "RETURN"
        funcColWidth()
        FormatGridColumns(gridView, False, True, False, False)
    End Sub
    Private Sub AutoResizeToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
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
        End If

        GridViewHeaderStyle()
    End Sub

    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHeader.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("DATE", GetType(String))
            .Columns.Add("CREC~CISS~CINTISS~CBALANCE", GetType(String))
            .Columns.Add("BREC~BSR~BISS~BPS~BHS~BINTISS~SALES~BBALANCE", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("DATE").Caption = "PARTICULARS"
            .Columns("CREC~CISS~CINTISS~CBALANCE").Caption = "COUNTER STOCK"
            .Columns("BREC~BSR~BISS~BPS~BHS~BINTISS~SALES~BBALANCE").Caption = "BOOK STOCK"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHeader
            .DataSource = dtMergeHeader
            .Columns("DATE").HeaderText = ""
            .Columns("CREC~CISS~CINTISS~CBALANCE").HeaderText = "COUNTER STOCK"
            .Columns("BREC~BSR~BISS~BPS~BHS~BINTISS~SALES~BBALANCE").HeaderText = "BOOK STOCK"
            .Columns("SCROLL").HeaderText = ""
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
        With gridViewHeader
            .Columns("DATE").Width = _
            IIf(gridView.Columns("DATE").Visible, gridView.Columns("DATE").Width, 0)
            .Columns("DATE").HeaderText = ""

            .Columns("CREC~CISS~CINTISS~CBALANCE").Width = _
            IIf(gridView.Columns("CREC").Visible, gridView.Columns("CREC").Width, 0) _
            + IIf(gridView.Columns("CISS").Visible, gridView.Columns("CISS").Width, 0) _
            + IIf(gridView.Columns("CINTISS").Visible, gridView.Columns("CINTISS").Width, 0) _
            + IIf(gridView.Columns("CBALANCE").Visible, gridView.Columns("CBALANCE").Width, 0)
            .Columns("CREC~CISS~CINTISS~CBALANCE").HeaderText = "COUNTER STOCK"

            .Columns("BREC~BSR~BISS~BPS~BHS~BINTISS~SALES~BBALANCE").Width = _
            IIf(gridView.Columns("BREC").Visible, gridView.Columns("BREC").Width, 0) _
            + IIf(gridView.Columns("BSR").Visible, gridView.Columns("BSR").Width, 0) _
            + IIf(gridView.Columns("BISS").Visible, gridView.Columns("BISS").Width, 0) _
            + IIf(gridView.Columns("BPS").Visible, gridView.Columns("BPS").Width, 0) _
            + IIf(gridView.Columns("BHS").Visible, gridView.Columns("BHS").Width, 0) _
            + IIf(gridView.Columns("BINTISS").Visible, gridView.Columns("BINTISS").Width, 0) _
            + IIf(gridView.Columns("SALES").Visible, gridView.Columns("SALES").Width, 0) _
            + IIf(gridView.Columns("BBALANCE").Visible, gridView.Columns("BBALANCE").Width, 0)
            .Columns("BREC~BSR~BISS~BPS~BHS~BINTISS~SALES~BBALANCE").HeaderText = "BOOK STOCK"
        End With
    End Function

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmBookVsCounterStockReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBookVsCounterStockReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
    End Sub
End Class