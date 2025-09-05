Imports System.Data.OleDb
Public Class frmTagedITemCostCenterWise
    Dim strSql As String
    Dim dtCompany As New DataTable
    Dim dtOptions As New DataTable
    Dim dtAcName As New DataTable
    Dim dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim DS As DataSet
    Dim DT As DataTable
    Dim da As OleDbDataAdapter
    Dim dtCounter As New DataTable
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridViewHeader.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        FuncCostCenterLoad()

        txtlotno.Text = ""
        dtpFrom.Select()
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
    Private Sub FuncCostCenterLoad()

        cmbMetal_Man.Items.Clear()
        cmbMetal_Man.Items.Add("ALL")
        cmbMetal_Man.Text = "ALL"
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal_Man, False, False)
        cmbMetal_Man.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
        lblTitle.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub frmMeltingDetailReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FuncCostCenterLoad()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'AutoResizeToolStripMenuItem.Checked = False
        Dim metal As String
        Dim SelectedCostId As String = GetSelectedCostId(chkCmbCostCentre, False)
        If SelectedCostId = "" Then SelectedCostId = "ALL"
        If cmbMetal_Man.Text <> "ALL" Then
            metal = GetSqlValue(cn, "select METALID  from " & cnAdminDb & " ..METALMAST  WHERE METALNAME='" & cmbMetal_Man.Text & "'  ")
        Else
            metal = "ALL"
        End If

        strSql = "EXEC " & cnAdminDb & "..SP_COSTCENTREWISERECEIPTVIEW"
        strSql += vbCrLf + " @AdminDb='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@FRMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COSTCENTER='" & SelectedCostId & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@METALID = '" & metal & "'"
        strSql += vbCrLf + " ,@LOTNO = '" & Val(txtlotno.Text.ToString) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        DS = New DataSet()
        DT = New DataTable()
        da.Fill(DS)
        DT = DS.Tables(0)
        If DT.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = DT
            strSql = "(SELECT COSTNAME ,COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN "
            strSql += vbCrLf + "(SELECT DISTINCT TCOSTID  FROM TEMPTABLEDB..TEMPCOSTCENTREWISERECEIPT))"
            da = New OleDbDataAdapter(strSql, cn)
            dtCounter = New DataTable
            da.Fill(dtCounter)
            GRIDFORMATSTYLE()

            lblTitle.Visible = True
            lblTitle.Text = "COSTCENTER WISE TAGED ITEM FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += "  AT " & chkCmbCostCentre.Text & ""

        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            gridViewHeader.DataSource = Nothing
            dtpFrom.Focus()
        End If
    End Sub

    Private Sub frmMeltingDetailReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function
    Public Function GetSelectedAccCode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function
    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHeader.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub GRIDFORMATSTYLE()
        With gridView
            .Columns("RESULT").Visible = False
            For j As Integer = 0 To .ColumnCount - 1
                If .Columns(j).Name.Contains("_PCS") Then .Columns(j).HeaderText = "PCS"
                If .Columns(j).Name.Contains("_GRSWT") Then .Columns(j).HeaderText = "GRSWT"
                If .Columns(j).Name.Contains("_NETWT") Then .Columns(j).HeaderText = "NETWT"
                If .Columns(j).Name.Contains("TOTPCS") Then .Columns(j).HeaderText = "PCS"
                If .Columns(j).Name.Contains("TOTGRSWT") Then .Columns(j).HeaderText = "GRSWT"
                If .Columns(j).Name.Contains("TOTNETWT") Then .Columns(j).HeaderText = "NETWT"
                .Columns(j).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With

        Dim dtHeader As New DataTable
        With dtHeader
            .Columns.Add("DEALER~ITEMNAME~RESULT", GetType(String))
            For j As Integer = 0 To dtCounter.Rows.Count - 1
                Dim Counter As String = dtCounter.Rows(j).Item("COSTID").ToString
                .Columns.Add(Counter & "_PCS~" & Counter & "_GRSWT~" & Counter & "_NETWT", GetType(String))
            Next
            .Columns.Add("TOTPCS~TOTGRSWT~TOTNETWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
        End With
        With gridViewHeader
            .DataSource = Nothing
            .DataSource = dtHeader
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DEALER~ITEMNAME~RESULT").HeaderText = "PARTICULARS"

            .Columns("DEALER~ITEMNAME~RESULT").Width = _
            IIf(gridView.Columns("DEALER").Visible, gridView.Columns("DEALER").Width, 0) _
            + IIf(gridView.Columns("ITEMNAME").Visible, gridView.Columns("ITEMNAME").Width, 0) _
            + IIf(gridView.Columns("RESULT").Visible, gridView.Columns("RESULT").Width, 0)
            .Columns("DEALER~ITEMNAME~RESULT").HeaderText = "PARTICULARS"

            For j As Integer = 0 To dtCounter.Rows.Count - 1
                Dim Counter As String = dtCounter.Rows(j).Item("COSTID").ToString
                Dim countername As String = dtCounter.Rows(j).Item("COSTNAME").ToString
                'If .Columns.Contains(Counter & "_PCS") Then  Else Continue For
                .Columns(Counter & "_PCS~" & Counter & "_GRSWT~" & Counter & "_NETWT").HeaderText = countername
                .Columns(Counter & "_PCS~" & Counter & "_GRSWT~" & Counter & "_NETWT").Width = gridView.Columns(Counter & "_PCS").Width + gridView.Columns(Counter & "_GRSWT").Width + gridView.Columns(Counter & "_NETWT").Width
            Next
            .Columns("TOTPCS~TOTGRSWT~TOTNETWT").Width = _
                     IIf(gridView.Columns("TOTPCS").Visible, gridView.Columns("TOTPCS").Width, 0) _
                     + IIf(gridView.Columns("TOTGRSWT").Visible, gridView.Columns("TOTGRSWT").Width, 0) + IIf(gridView.Columns("TOTNETWT").Visible, gridView.Columns("TOTNETWT").Width, 0)
            .Columns("TOTPCS~TOTGRSWT~TOTNETWT").HeaderText = "TOTAL"

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
        With gridView
            .Columns("DEALER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With
        For Each gv As DataGridViewRow In gridView.Rows
            With gv
                Select Case .Cells("ITEMNAME").Value.ToString
                    Case "TOTAL"
                        .DefaultCellStyle.Format = Format("TOTAL", "#0.00")
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.BackColor = Color.LemonChiffon
                End Select
            End With
        Next
    End Sub
End Class