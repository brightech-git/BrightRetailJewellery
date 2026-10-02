Imports System.Data.OleDb
Imports System.Xml
Imports System.IO
Imports com.ms.win32
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports System.Windows.Controls.Primitives
Public Class frmCustomerTransactionReport
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Dim SelectedCompany As String

    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable
    Dim dtItemType As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim HideSummary As Boolean = IIf(GetAdmindbSoftValue("HIDE-STOCKSUMMARY", "N") = "Y", True, False)
    Dim NormalMode As Boolean = IIf(GetAdmindbSoftValue("ITEMSTKRPT", "Y") = "Y", True, False)
    Dim spbaserpt As Boolean = IIf(GetAdmindbSoftValue("SP_ITEMSTKRPT", "Y") = "Y", True, False)
    Dim StoneRound As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-DIA", 2))
    Dim SelectionFormatNew As Boolean = IIf(GetAdmindbSoftValue("ITEMWISESTKFORMAT", "N") = "Y", True, False)
    Dim dtGrid As New DataTable()
    Dim DiaRnd As Integer = 3
    Dim StoneDetail As Boolean = False
    Dim itemid As String = ""
    Dim subitemid As String = ""
    Dim costids As String = ""
    Dim dtrange As DataTable
    Dim IS40COLCLSSTKPRINT As Boolean = IIf(GetAdmindbSoftValue("40COLCLSSTKPRINT", "N") = "Y", True, False)
    Dim dtSource As DataTable
    Dim hoServerId As String
    Dim hoPassword As String
    Dim hoComIpd As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        tabMain.SelectedTab = tabGen
        optAsOn.Checked = True : optBetween.Checked = False
        LoadMetal()
        LoadCostcentre()
        LoadSearchKey()
        txtSearch.Clear()
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function
    Public Function GetSelecteditemtypeid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedDesignerid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Function GetSelectedCounderid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Public Function GetSelectedMetalid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT Metalid FROM " & cnAdminDb & "..MetalMast WHERE MetalName= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCatCode(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Public Function GetSelectedRange(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += chkLst.CheckedItems.Item(cnt).ToString
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Public Function GetSelectedItemType(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += Mid(chkLst.CheckedItems.Item(cnt).ToString, 1, 1)
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
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
    Private Sub LoadMetal()
        strSql = $"select 'ALL' METALID,'ALL' METALNAME, 0 DISPLAYORDER"
        strSql += vbCrLf + $"UNION"
        strSql += vbCrLf + $"select METALID,METALNAME,DISPLAYORDER from {cnAdminDb}..METALMAST where ACTIVE = 'Y' order by DISPLAYORDER"
        cmd = New OleDb.OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        Dim dtMetal As New DataTable
        da.Fill(dtMetal)
        cmbMetal.DataSource = Nothing
        cmbMetal.DataSource = dtMetal
        cmbMetal.DisplayMember = "METALNAME"
        cmbMetal.ValueMember = "METALID"
    End Sub
    Private Sub LoadCostcentre()
        strSql = $"select 'ALL' COSTID,'ALL' COSTNAME"
        strSql += vbCrLf + $"UNION"
        strSql += vbCrLf + $"select COSTID,COSTNAME from {cnAdminDb}..COSTCENTRE where active = 'Y'"
        cmd = New OleDb.OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        Dim dtMetal As New DataTable
        da.Fill(dtMetal)
        cmbCostcentre.DataSource = Nothing
        cmbCostcentre.DataSource = dtMetal
        cmbCostcentre.DisplayMember = "COSTNAME"
        cmbCostcentre.ValueMember = "COSTID"
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridviewDetail.Visible = False
        ' CARD / CREDIT / JND / ADVANCE are numeric filters - reject text before building the EXEC
        If ",CARD,CREDIT,JND,ADVANCE,".Contains("," & cmbSearchKey.Text & ",") AndAlso txtSearch.Text.Trim <> "" AndAlso Not IsNumeric(txtSearch.Text.Trim) Then
            MsgBox(cmbSearchKey.Text & " search needs a numeric amount...", MsgBoxStyle.Information)
            txtSearch.Focus()
            Exit Sub
        End If
        If optAsOn.Checked Then
            strSql = $"EXEC {cnStockDb}..SP_RPT_BILLWISETRANSACTION_SEARCHASON"
            strSql += vbCrLf + $"@ASONDATE           = '{dtpFrom.Value.ToString("yyyy-MM-dd")}'"
        Else
            strSql = $"EXEC {cnStockDb}..SP_RPT_BILLWISETRANSACTION_SEARCH"
            strSql += vbCrLf + $"@DATE           = '{dtpFrom.Value.ToString("yyyy-MM-dd")}'"
            strSql += vbCrLf + $",@TODATE        = '{dtpTo.Value.ToString("yyyy-MM-dd")}'"
        End If
        strSql += vbCrLf + $",@COSTCENTRE    = '{IIf(cmbCostcentre.Text <> "ALL", cmbCostcentre.SelectedValue, "ALL")}'"
        strSql += vbCrLf + $",@NODEID        = ''"
        strSql += vbCrLf + $",@SystemId      = ''"
        strSql += vbCrLf + $",@COMPANYID     = '{strCompanyId}'"
        strSql += vbCrLf + $",@WITHORD       = 'N'"
        strSql += vbCrLf + $",@WITHCANBILL   = 'N'"
        strSql += vbCrLf + $",@ADMINDB       = '{cnAdminDb}'"
        strSql += vbCrLf + $",@METAL         = '{IIf(cmbMetal.Text <> "ALL", cmbMetal.SelectedValue, "")}'"
        strSql += vbCrLf + $",@CASHID        = ''"
        strSql += vbCrLf + $",@BILLNO        = ''"
        strSql += vbCrLf + $",@WITHAPPROVAL  = 'N'"
        strSql += vbCrLf + $"---- search parameters ----"
        strSql += vbCrLf + $",@CUSTOMER      = '{IIf(cmbSearchKey.Text = "CUSTOMER", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@PHONENO       = '{IIf(cmbSearchKey.Text = "MOBILENO", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@PAN           = '{IIf(cmbSearchKey.Text = "PAN", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@GSTNO         = '{IIf(cmbSearchKey.Text = "GSTNO", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@ADDRESS       = '{IIf(cmbSearchKey.Text = "ADDRESS", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@SEARCHBILLNO  = '{IIf(cmbSearchKey.Text = "BILLNO", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@FROMTRANDATE  = NULL"
        strSql += vbCrLf + $",@TOTRANDATE    = NULL"
        strSql += vbCrLf + $",@ITEMNAME  = '{IIf(cmbSearchKey.Text = "ITEMNAME", txtSearch.Text.Trim, "")}'"
        strSql += vbCrLf + $",@CARD          = {SearchAmount("CARD")}"
        strSql += vbCrLf + $",@CREDIT        = {SearchAmount("CREDIT")}"
        strSql += vbCrLf + $",@JND           = {SearchAmount("JND")}"
        strSql += vbCrLf + $",@ADVANCE       = {SearchAmount("ADVANCE")}"

        cmd = New OleDb.OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 180
        da = New OleDbDataAdapter(cmd)
        dtSource = New DataTable
        da.Fill(dtSource)
        If dtSource.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtSource
            FormatNumericColumns(gridView)
            tabMain.SelectedTab = tabView

            Dim tit As String
            tit = " CUSTOMER TRANSACTION REPORT" + vbCrLf
            If optAsOn.Checked Then
                tit += " AS ON DATE : " & dtpFrom.Text
            Else
                tit += " DATE FROM : " & dtpFrom.Text + " TO " + dtpTo.Text
            End If
            lblTitle.Text = tit.ToString + vbCrLf + "METAL : " + cmbMetal.Text + vbCrLf + "COSTCENTRE : " + cmbCostcentre.Text
        Else
            gridView.DataSource = Nothing
            tabMain.SelectedTab = tabGen
            MsgBox("Records not found...", MsgBoxStyle.Information)
        End If
    End Sub
    Function LoadSearchKey()
        cmbSearchKey.Items.Clear()
        cmbSearchKey.Items.Add("CUSTOMER")
        cmbSearchKey.Items.Add("MOBILENO")
        cmbSearchKey.Items.Add("PAN")
        cmbSearchKey.Items.Add("GSTNO")
        cmbSearchKey.Items.Add("ADDRESS")
        cmbSearchKey.Items.Add("BILLNO")
        cmbSearchKey.Items.Add("ITEMNAME")
        cmbSearchKey.Items.Add("CARD")
        cmbSearchKey.Items.Add("CREDIT")
        cmbSearchKey.Items.Add("JND")
        cmbSearchKey.Items.Add("ADVANCE")
        cmbSearchKey.Text = "MOBILENO"
    End Function

    ' Unlike the text filters, an unselected amount filter must be NULL and not an
    ' empty string - an empty string reaches the NUMERIC parameter as 0 and would
    ' wrongly restrict the result to zero-amount rows.
    Private Function SearchAmount(ByVal key As String) As String
        If cmbSearchKey.Text <> key Then Return "NULL"
        Dim s As String = txtSearch.Text.Trim
        If s = "" OrElse Not IsNumeric(s) Then Return "NULL"
        Return CDec(s).ToString("0.00", Globalization.CultureInfo.InvariantCulture)
    End Function

    ' The procedure returns every weight / rate / amount column as a numeric type,
    ' so align on the bound column type rather than on a hard-coded name list - that
    ' stays correct if the procedure's column list changes. Scale comes from the
    ' column itself, so only the columns whose stored scale differs from what the
    ' report should show need an explicit format.
    Private Sub FormatNumericColumns(ByVal grid As DataGridView)
        For Each col As DataGridViewColumn In grid.Columns
            If col.ValueType Is Nothing Then Continue For
            Select Case Type.GetTypeCode(col.ValueType)
                Case TypeCode.Decimal, TypeCode.Double, TypeCode.Single, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.Byte, TypeCode.SByte, TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            End Select
        Next

        ' RATE is NUMERIC(15,4) in this database, so it renders as .0000 - show 2 decimals.
        If grid.Columns.Contains("IRATE") Then grid.Columns("IRATE").DefaultCellStyle.Format = "0.00"
    End Sub

    ' frmPurchaseOrderDetail is shared with the purchase order and stock reports, so the total
    ' row colouring is attached to that one instance from the caller instead of being built into
    ' the shared form. CellFormatting re-evaluates on every paint, so the colours stay on the
    ' correct rows even if a column gets sorted.
    Private ReadOnly _billTotalBack As Color = Color.FromArgb(222, 235, 247)
    Private ReadOnly _grandTotalBack As Color = Color.FromArgb(255, 226, 150)
    Private _totalRowFont As Font

    Private Sub DetailGrid_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs)
        Dim grid As DataGridView = DirectCast(sender, DataGridView)
        If e.RowIndex < 0 OrElse Not grid.Columns.Contains("ITEMNAME") Then Exit Sub

        Dim label As Object = grid.Rows(e.RowIndex).Cells("ITEMNAME").Value
        If label Is Nothing OrElse IsDBNull(label) Then Exit Sub

        Dim back As Color
        Select Case label.ToString()
            Case "BILL TOTAL"
                back = _billTotalBack
            Case "GRAND TOTAL"
                back = _grandTotalBack
            Case Else
                Exit Sub
        End Select

        If _totalRowFont Is Nothing Then _totalRowFont = New Font(grid.Font, FontStyle.Bold)
        e.CellStyle.BackColor = back
        e.CellStyle.SelectionBackColor = back
        e.CellStyle.ForeColor = Color.Black
        e.CellStyle.SelectionForeColor = Color.Black
        e.CellStyle.Font = _totalRowFont
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
        optAsOn.Checked = True : optBetween.Checked = False
        LoadMetal()
        LoadCostcentre()
        LoadSearchKey()
        txtSearch.Clear()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If StoneDetail = True Then
            If gridviewDetail.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridviewDetail, BrightPosting.GExport.GExportType.Export, gridViewHead)
            End If
        Else
            If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
            End If
        End If

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If IS40COLCLSSTKPRINT Then
            If MsgBox("Do you want to print on 60 Col. Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then IS40COLCLSSTKPRINT = False
        End If
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
    End Sub
    Private Sub optAsOn_CheckedChanged(sender As Object, e As EventArgs) Handles optAsOn.CheckedChanged
        If optAsOn.Checked Then
            lblFrom.Text = "AS ON : "
            lblTo.Visible = False : dtpTo.Visible = False
        Else
            lblFrom.Text = "FROM : "
            lblTo.Visible = True : dtpTo.Visible = True
        End If
    End Sub

    Private Sub optBetween_CheckedChanged(sender As Object, e As EventArgs) Handles optBetween.CheckedChanged
        If optBetween.Checked Then
            lblFrom.Text = "FROM : "
            lblTo.Visible = True : dtpTo.Visible = True
        Else
            lblFrom.Text = "AS ON : "
            lblTo.Visible = False : dtpTo.Visible = False
        End If
    End Sub

    Private Sub gridView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "D" Then
            Dim batchNo As String = gridView.Item("BATCHNO", gridView.CurrentRow.Index).Value.ToString
            Dim trandate As Date = gridView.Item("TRANDATE", gridView.CurrentRow.Index).Value.ToString
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim memfile As String = "\BillPrint.mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":POS")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & batchNo)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & trandate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                                LSet("TYPE", 15) & ":POS;" &
                                LSet("BATCHNO", 15) & ":" & batchNo & ";" &
                                LSet("TRANDATE", 15) & ":" & trandate.ToString("yyyy-MM-dd") & ";" &
                                LSet("DUPLICATE", 15) & ":Y")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        ElseIf UCase(e.KeyChar) = "B" Then
            Dim phoneNo As String = gridView.Item("PHONENO", gridView.CurrentRow.Index).Value.ToString
            If phoneNo.Trim = "" Then
                MessageBox.Show("The selected row doesn't contains phone number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim dv As New DataView(dtSource)
            dv.RowFilter = "PHONENO = '" & phoneNo.Replace("'", "''") & "'"
            Dim dtFilteredDate As DataTable = dv.ToTable(False, "TRANDATE", "BILLNO", "PHONENO", "RUNNO", "IITEMNAME", "IGRSWT", "INETWT", "RAMOUNT", "IAMOUNT", "CASH", "CARD", "ADVANCE", "CHITCARD", "CREDIT", "JND", "TOTAL", "CUSTOMER")

            Dim colSno As New DataColumn("SNO", GetType(Integer))
            dtFilteredDate.Columns.Add(colSno)
            colSno.SetOrdinal(0)

            For i As Integer = 0 To dtFilteredDate.Rows.Count - 1
                dtFilteredDate.Rows(i)("SNO") = i + 1
            Next

            dtFilteredDate.Columns("TRANDATE").ColumnName = "BILLDATE"
            dtFilteredDate.Columns("PHONENO").ColumnName = "MOBILENO"
            dtFilteredDate.Columns("IITEMNAME").ColumnName = "ITEMNAME"
            dtFilteredDate.Columns("IGRSWT").ColumnName = "GRSWT"
            dtFilteredDate.Columns("INETWT").ColumnName = "NETWT"
            dtFilteredDate.Columns("RAMOUNT").ColumnName = "RETURN AMOUNT"
            dtFilteredDate.Columns("IAMOUNT").ColumnName = "AMOUNT"
            dtFilteredDate.Columns("CHITCARD").ColumnName = "CHIT"

            Dim totalCols() As String = {"GRSWT", "NETWT", "RETURN AMOUNT", "AMOUNT", "CASH", "CARD", "ADVANCE", "CHIT", "CREDIT", "JND", "TOTAL"}

            ' Rebuild the table with a BILL TOTAL row after each bill, followed by one GRAND TOTAL row.
            Dim dtGrouped As DataTable = dtFilteredDate.Clone()
            Dim billNosSeen As New List(Of String)
            For Each r As DataRow In dtFilteredDate.Rows
                Dim billNo As String = If(IsDBNull(r("BILLNO")), "", r("BILLNO").ToString)
                If Not billNosSeen.Contains(billNo) Then billNosSeen.Add(billNo)
            Next

            For Each billNo As String In billNosSeen
                Dim billFilter As String = "BILLNO = '" & billNo.Replace("'", "''") & "'"
                For Each r As DataRow In dtFilteredDate.Select(billFilter)
                    dtGrouped.ImportRow(r)
                Next

                Dim billTotalRow As DataRow = dtGrouped.NewRow()
                billTotalRow("BILLNO") = billNo
                billTotalRow("ITEMNAME") = "BILL TOTAL"
                For Each colName As String In totalCols
                    If dtFilteredDate.Columns.Contains(colName) Then
                        Dim result As Object = dtFilteredDate.Compute("Sum([" & colName & "])", billFilter)
                        billTotalRow(colName) = If(IsDBNull(result), DBNull.Value, result)
                    End If
                Next
                dtGrouped.Rows.Add(billTotalRow)
            Next

            Dim totalRow As DataRow = dtGrouped.NewRow()
            totalRow("ITEMNAME") = "GRAND TOTAL"
            For Each colName As String In totalCols
                If dtFilteredDate.Columns.Contains(colName) Then
                    Dim result As Object = dtFilteredDate.Compute("Sum([" & colName & "])", "")
                    totalRow(colName) = If(IsDBNull(result), DBNull.Value, result)
                End If
            Next
            dtGrouped.Rows.Add(totalRow)

            Dim ofrmPurchaseOrderDetail As New frmPurchaseOrderDetail(dtGrouped)
            ofrmPurchaseOrderDetail.Text = ""
            ofrmPurchaseOrderDetail.lblHead.Text = "CUSTOMER TRANSACTION DETAIL" + vbCrLf + $"FOR THE MOBILE - {phoneNo}"
            AddHandler ofrmPurchaseOrderDetail.DgView.CellFormatting, AddressOf DetailGrid_CellFormatting
            If ofrmPurchaseOrderDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Else
                Exit Sub
            End If
        End If
    End Sub
End Class
