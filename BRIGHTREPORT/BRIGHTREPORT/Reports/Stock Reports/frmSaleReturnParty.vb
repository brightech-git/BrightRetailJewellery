Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Linq
Imports java.lang
Public Class frmSaleReturnParty
    '01 SHERIFF - 24-10-12
    '250213 VASANTHAN For WHITEFIRE
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
        'HoServerDetails()
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text.ToString <> "ALL" And chkCmbMetal.Text.ToString <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text.ToString) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
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
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridviewDetail.Visible = False
        'chkSelectAll.Checked = False
        'If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        'If optDetail.Checked Then
        '    strSql = $"select cast(0 as bit) [MARK], cast(a.ITEMID as nvarchar(100))+ '-' + b.ITEMNAME ITEM,a.PARTICULAR,a.PO_PIECES, a.POFROMDATE POSTINGFROM, a.POTODATE POSTINGTO,a.PONUMBER, a.PODATE PODATE, a.TRANDATE [TRANS DATE], a.RANGECAPTION, a.SIZEID from {cnAdminDb}.. PURCHASEORDER a"
        '    strSql += $" join {cnAdminDb}..itemmast b on a.itemid = b.ITEMID"
        '    strSql += " where ISNULL(po_pieces,0)<>0"
        '    If chkCmbMetal.Text.ToString <> "ALL" And chkCmbMetal.Text.ToString <> "" Then
        '        strSql += " AND b.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text.ToString) & "))"
        '    End If
        '    If itemid <> "ALL" Then strSql += $" And a.itemid in ({itemid})"
        '    'strSql += $" And a.podate between '{dtpFrom.Value}' and '{dtpTo.Value}'"
        '    If optBetween.Checked Then
        '        strSql += vbCrLf + $"	and a.podate between '{dtpFrom.Value}' and '{dtpTo.Value}'	"
        '    ElseIf optAsOn.Checked Then
        '        strSql += vbCrLf + $"	and a.podate <= '{dtpFrom.Value}'	"
        '    End If
        '    If Not chkTrans.Checked Then strSql += " and isnull(a.TRANFLAG,0) = 0"
        '    strSql += " order by ponumber"
        '    chkSelectAll.Visible = True : btnMerge.Visible = True
        'Else
        '    strSql = $"select cast(0 as bit) [MARK], cast(a.ITEMID as nvarchar(100))+ '-' + b.ITEMNAME ITEM, SUM(a.PO_PIECES) PO_PIECES,a.PONUMBER, a.PODATE PODATE, a.TRANDATE [TRANS DATE] from {cnAdminDb}.. PURCHASEORDER a"
        '    strSql += $" join {cnAdminDb}..itemmast b on a.itemid = b.ITEMID"
        '    strSql += " where ISNULL(po_pieces,0)<>0"
        '    If chkCmbMetal.Text.ToString <> "ALL" And chkCmbMetal.Text.ToString <> "" Then
        '        strSql += " AND b.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text.ToString) & "))"
        '    End If
        '    If itemid <> "ALL" Then strSql += $" And a.itemid in ({itemid})"
        '    'strSql += $" And a.podate between '{dtpFrom.Value}' and '{dtpTo.Value}'"
        '    If optBetween.Checked Then
        '        strSql += vbCrLf + $"	and a.podate between '{dtpFrom.Value}' and '{dtpTo.Value}'	"
        '    ElseIf optAsOn.Checked Then
        '        strSql += vbCrLf + $"	and a.podate <= '{dtpFrom.Value}'	"
        '    End If
        '    If Not chkTrans.Checked Then strSql += " and isnull(a.TRANFLAG,0) = 0"
        '    strSql += " group by a.ITEMID,ponumber,b.ITEMNAME,a.PODATE,a.TRANDATE"
        '    chkSelectAll.Visible = False : btnMerge.Visible = False
        'End If

        strSql = $";with cte as ("
        strSql += vbCrLf + $"Select TRFNO,CATCODE,COSTID,sum(PCS) SRPCS,sum(GRSWT) SRGRSWT from {cnStockDb}..RECEIPT where TRANTYPE='SR' and TRFNO is not null"
        strSql += vbCrLf + $"and trandate between '{dtpFrom.Text}' and '{dtpTo.Text}'	"
        strSql += vbCrLf + "group by TRFNO, CATCODE, COSTID, TRFNO"
        strSql += vbCrLf + ")"
        strSql += vbCrLf + ",cte2 as("
        strSql += vbCrLf + $"Select cte.*,RECEIPT.COSTID RCOSTID,RECEIPT.PCS RPCS,RECEIPT.GRSWT RGRSWT,RECEIPT.SNO RECSNO  from {cnStockDb}..receipt"
        strSql += vbCrLf + "Join cte on cte.TRFNO = RECEIPT.TRFNO And cte.CATCODE = RECEIPT.CATCODE "
        strSql += vbCrLf + "where TRANTYPE ='RIN'"
        strSql += vbCrLf + ")"
        strSql += vbCrLf + ",cte3 as("
        strSql += vbCrLf + "Select cte2.*,LOTISSUE.PCS LIPCS,LOTISSUE.GRSWT LIGRSWT,LOTISSUE.LOTSNO from cte2"
        strSql += vbCrLf + $"Left Join {cnStockDb}..LOTISSUE on cte2.RECSNO = LOTISSUE.RECSNO"
        strSql += vbCrLf + ")"
        strSql += vbCrLf + ",cte4 as("
        strSql += vbCrLf + "Select cte3.*,ITEMLOT.PCS LPCS,ITEMLOT.GRSWT LGRSWT from cte3"
        strSql += vbCrLf + $"Left Join {cnAdminDb}..ITEMLOT on ITEMLOT.SNO = cte3.LOTSNO"
        strSql += vbCrLf + ")"
        strSql += vbCrLf + "Select cte4.*,DESIGNER.DESIGNERID,DESIGNER.DESIGNERNAME,ITEMTAG.TPCS,ITEMTAG.TGRSWT from cte4"
        strSql += vbCrLf + $"Left Join(select LOTSNO, DESIGNERID, sum(PCS)TPCS, sum(GRSWT)TGRSWT from {cnAdminDb}..ITEMTAG group by LOTSNO, DESIGNERID) ITEMTAG on ITEMTAG.LOTSNO = cte4.LOTSNO"
        strSql += vbCrLf + $"inner Join {cnAdminDb}..DESIGNER on ITEMTAG.DESIGNERID = DESIGNER.DESIGNERID"

        cmd = New OleDb.OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)

        dtSource = New DataTable
        da.Fill(dtSource)
        If dtSource.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtSource
            tabMain.SelectedTab = tabView

            Dim tit As String
            tit = " Sales Return Partywise " + vbCrLf
            tit += " DATE FROM : " & dtpFrom.Text + " TO " + dtpTo.Text
            'If optAsOn.Checked Then
            '    tit += " AS ON DATE : " & dtpFrom.Text
            'Else
            '    tit += " DATE FROM : " & dtpFrom.Text + " TO " + dtpTo.Text
            'End If
            'If chkCmbMetal.Text.ToString <> "ALL" And chkCmbMetal.Text.ToString <> "" Then
            '    tit += " FOR METAL " & chkCmbMetal.Text.ToString & ""
            'End If
            lblTitle.Text = tit.ToString
            chkSelectAll.Enabled = True : btnMerge.Enabled = True
        Else
            gridView.DataSource = Nothing
            tabMain.SelectedTab = tabGen
            MsgBox("Records not found...", MsgBoxStyle.Information)
        End If
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
        dtpTo.Value = GetServerDate()
        'gridView.DataSource = Nothing
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        'funcLoadItemName()
        'If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        'strSql = " SELECT 'ALL' Caption,0 RESULT UNION ALL "
        'strSql += "SELECT DISTINCT CAPTION,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        'If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        'strSql += vbCrLf + " ORDER BY RESULT"
        'dtrange = New DataTable
        'dtrange = GetSqlTable(strSql, cn)
        'If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        'chkCmbMetal.Select()
        'chkSelectAll.Checked = False : chkSelectAll.Enabled = False
        'btnMerge.Enabled = False
        'optDetail.Checked = True : optSummary.Checked = False
        'HoServerDetails()
        'chkTrans.Checked = False
        'optAsOn.Checked = True : optBetween.Checked = False
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
    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
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
    Private Sub gridView_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit
        If e.ColumnIndex <> gridView.Columns.Count - 1 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub chkCmbMetal_Validated(sender As Object, e As EventArgs) Handles chkCmbMetal.Validated
        funcLoadItemName()
    End Sub
    Private Sub gridView_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles gridView.CellMouseClick
        'If gridView.Columns(e.ColumnIndex).Name = "MARK" Then
        '    If optSummary.Checked Then
        '        If gridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False Then
        '            gridView.Rows(e.RowIndex).Cells("MARK").Value = "True"
        '        Else
        '            gridView.Rows(e.RowIndex).Cells("MARK").Value = "False"
        '        End If
        '    Else
        '        Dim poNumber As String = gridView.Rows(e.RowIndex).Cells("PONUMBER").Value
        '        If Not IsDBNull(gridView.Rows(e.RowIndex).Cells("TRANS DATE").Value) Then Exit Sub
        '        If gridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False Then
        '            For Each row As DataGridViewRow In gridView.Rows
        '                If row.Cells("PONUMBER").Value.ToString() = poNumber Then
        '                    row.Cells("MARK").Value = "True"
        '                End If
        '            Next
        '        Else
        '            For Each row As DataGridViewRow In gridView.Rows
        '                If row.Cells("PONUMBER").Value.ToString() = poNumber Then
        '                    row.Cells("MARK").Value = "False"
        '                End If
        '            Next
        '        End If
        '    End If
        'End If
    End Sub
    Private Sub btnMerge_Click(sender As Object, e As EventArgs) Handles btnMerge.Click
        Try
            Dim filteredTable As DataTable = dtSource.Clone()
            For Each dr As DataRow In dtSource.Rows
                If dr("MARK").ToString = "True" Then
                    Dim parts() As String = dr("PONUMBER").ToString.Split("-"c)
                    Dim lastPart As String = parts(parts.Length - 1)
                    If lastPart.StartsWith("M", StringComparison.OrdinalIgnoreCase) Then Continue For
                    filteredTable.ImportRow(dr)
                End If
            Next

            If filteredTable.AsDataView.ToTable(True, "ITEM").Rows.Count > 1 Then
                MessageBox.Show("Different items cannot be merged.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If filteredTable.Rows.Count <= 0 Then
                Throw New Exception("Invalid data to merge.")
            End If

            ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
            Using connection As New SqlConnection("Data Source=" & ConInfo.lServerName & ";Initial Catalog=" & cnAdminDb & ";User ID=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";Password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & "")
                Using command As New SqlCommand("MergePurchaseOrder", connection)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.AddWithValue("@USERID", userId)

                    Dim tvpParameter As New SqlParameter("@MergePurchaseOrderData", SqlDbType.Structured)
                    tvpParameter.Value = filteredTable
                    tvpParameter.TypeName = "MergePurchaseOrderTableType"
                    command.Parameters.Add(tvpParameter)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Dim distinctDt As DataTable = filteredTable.AsDataView.ToTable(True, "ITEM", "PONUMBER")
            Dim grouped = distinctDt.AsEnumerable() _
                .GroupBy(Function(row) row.Field(Of String)("ITEM")) _
                .Select(Function(g) New With {
                    .Item = g.Key.ToString().Split("-"c)(0),
                    .PONumbers = String.Join(",", g.Select(Function(r) "'" & r.Field(Of String)("PONUMBER") & "'"))
                })
            For Each itemGroup As Object In grouped
                strSql = $"select cast(min(pofromdate) as date)frm, cast(max(potodate) as date) [to] from {cnAdminDb}..PURCHASEORDER where ITEMID = {itemGroup.Item} and PONUMBER IN ({itemGroup.PONumbers})"
                Dim dtMinMax As New DataTable
                cmd = New OleDb.OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtMinMax)
                Dim frmDt As DateTime = CType(dtMinMax.Rows(0)(0), DateTime)
                Dim toDt As DateTime = CType(dtMinMax.Rows(0)(1), DateTime)

                strSql = "select ISNULL(ctltext,0)+1 ctltext from " & cnStockDb & "..BILLCONTROL where CTLID = 'MET-O-PUR' and COMPANYID = '" & strCompanyId & "'"
                Dim poNum As Integer = objGPack.GetSqlValue(strSql, , , tran)
                Dim ActPoNum As String = $"{cnCostId}-{itemGroup.Item}-({frmDt.ToString("ddMM")}/{toDt.ToString("ddMM")}/{toDt.ToString("yy")})-M{poNum.ToString("0000")}"
                strSql = $"update {cnAdminDb}.. PURCHASEORDER set PONUMBER = '{ActPoNum}',PODATE = '{Now.Date}', POFROMDATE = '{dtMinMax.Rows(0)(0)}', POTODATE = '{dtMinMax.Rows(0)(1)}' where itemid = {itemGroup.Item} and PONUMBER IN ({itemGroup.PONumbers})"
                strSql += vbCrLf + $"update {cnStockDb}..BILLCONTROL set ctltext = '{poNum}' where CTLID = 'MET-O-PUR' and COMPANYID = '{strCompanyId}'"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()
            Next
            MessageBox.Show("Purchase Order merged successfully!")
            btnView_Search_Click(sender, e)
            chkSelectAll.Enabled = False : btnMerge.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked Then
            For Each row As DataGridViewRow In gridView.Rows
                row.Cells("MARK").Value = "True"
            Next
        Else
            For Each row As DataGridViewRow In gridView.Rows
                row.Cells("MARK").Value = "False"
            Next
        End If
    End Sub
    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        'Try
        '    If e.KeyCode = Keys.P Then
        '        Dim filteredTable As DataTable = dtSource.Clone()
        '        For Each dr As DataRow In dtSource.Rows
        '            If dr("MARK").ToString = "True" Then
        '                filteredTable.ImportRow(dr)
        '            End If
        '        Next
        '        If filteredTable.Rows.Count <= 0 Then
        '            Throw New Exception("Mark and then print.")
        '        End If
        '        Dim distinctDt As DataTable = filteredTable.DefaultView.ToTable(True, "PONUMBER")

        '        Dim _type As String = "PO"
        '        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
        '            strSql = $"select isnull(MAX(SNO),0)+1 from {cnAdminDb}..POPRINT"
        '            cmd = New OleDbCommand(strSql, cn)
        '            Dim sno As Integer = cmd.ExecuteScalar().ToString()
        '            For Each dr As DataRow In distinctDt.Rows
        '                strSql = $"INSERT INTO {cnAdminDb}..POPRINT(SNO,PONUMBER,PRINTDATETIME)"
        '                strSql += vbCrLf + $"VALUES({sno},'{dr("PONUMBER").ToString}','{Now}')"
        '                cmd = New OleDb.OleDbCommand(strSql, cn)
        '                cmd.CommandTimeout = 100000
        '                cmd.ExecuteNonQuery()
        '            Next
        '            Dim write As IO.StreamWriter
        '            write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
        '            write.WriteLine(LSet("TYPE", 15) & ":" & _type)
        '            write.WriteLine(LSet("BATCHNO", 15) & ":" & sno)
        '            write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
        '            write.Flush()
        '            write.Close()
        '            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
        '        Else
        '            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
        '        End If
        '    ElseIf e.KeyCode = Keys.D Then
        '        If optSummary.Checked Then
        '            Dim _poNo As String = gridView.Item("PONUMBER", gridView.CurrentRow.Index).Value.ToString
        '            Dim _item As String = gridView.Item("ITEM", gridView.CurrentRow.Index).Value.ToString
        '            Dim _totPieces As Integer = gridView.Item("PO_PIECES", gridView.CurrentRow.Index).Value
        '            Dim ofrmPurchaseOrderDetail As New frmPurchaseOrderDetail(chkCmbMetal.Text, itemid, dtpFrom.Value, dtpTo.Value, _poNo, _item,
        '                                                                      _totPieces, optAsOn.Checked)
        '            If ofrmPurchaseOrderDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '            Else
        '                Exit Sub
        '            End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try
    End Sub
    Private Sub HoServerDetails()
        Try
            strSql = "select FTPID,[PASSWORD],COMPID from " & cnAdminDb & "..SYNCCOSTCENTRE where MAIN = 'Y'"
            Dim dt As New DataTable
            cmd = New OleDb.OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                btnTransfer.Visible = True
                hoServerId = dt.Rows(0)("FTPID").ToString
                hoPassword = dt.Rows(0)("PASSWORD").ToString
                hoComIpd = dt.Rows(0)("COMPID").ToString
            Else
                btnTransfer.Visible = False
            End If
        Catch ex As Exception
            btnTransfer.Visible = False
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        Try
            Dim filteredTable As DataTable = dtSource.Clone()
            For Each dr As DataRow In dtSource.Rows
                If dr("MARK").ToString = "True" Then
                    filteredTable.ImportRow(dr)
                End If
            Next

            If filteredTable.Rows.Count <= 0 Then
                Throw New Exception("Invalid data to transfer.")
            End If

            Dim distinctDt As DataTable = filteredTable.DefaultView.ToTable(True, "PONUMBER")
            For Each dr As DataRow In distinctDt.Rows
                strSql = $"select ITEMID,PARTICULAR,PO_PIECES,POFROMDATE,POTODATE,USERID,PONUMBER,PODATE,SUBITEMID,RANGECAPTION,SIZEID from {cnAdminDb}..PURCHASEORDER where PONUMBER = '{dr("PONUMBER")}'"
                Dim dtData As New DataTable
                cmd = New OleDb.OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtData)

                Using connection As New SqlConnection("Data Source=" & hoServerId & ";Initial Catalog=" & hoComIpd & "ADMINDB;User ID=SA;Password=" & BrighttechPack.Decrypt(hoPassword) & "")
                    Using command As New SqlCommand("TransferPurchaseOrder", connection)
                        command.CommandType = CommandType.StoredProcedure
                        command.Parameters.AddWithValue("@TranFlag", 1)
                        command.Parameters.AddWithValue("@Trandate", Now.Date)

                        Dim tvpParameter As New SqlParameter("@PurchaseOrderTransferData", SqlDbType.Structured)
                        tvpParameter.Value = dtData
                        tvpParameter.TypeName = "PurchaseOrderTransferTableType"
                        command.Parameters.Add(tvpParameter)
                        connection.Open()
                        command.ExecuteNonQuery()
                    End Using
                End Using

                strSql = $"update {cnAdminDb}.. PURCHASEORDER set TRANFLAG = 1,TRANDATE = '{Now.Date}' where PONUMBER = '{dr("PONUMBER")}'"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()
            Next

            MessageBox.Show("Purchase Order transfered successfully!")
            btnView_Search_Click(sender, e)
            chkSelectAll.Enabled = False : btnMerge.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub optAsOn_CheckedChanged(sender As Object, e As EventArgs) Handles optAsOn.CheckedChanged
        'If optAsOn.Checked Then
        '    lblFrom.Text = "AS ON : "
        '    lblTo.Visible = False : dtpTo.Visible = False
        'Else
        '    lblFrom.Text = "FROM : "
        '    lblTo.Visible = True : dtpTo.Visible = True
        'End If
    End Sub
    Private Sub optBetween_CheckedChanged(sender As Object, e As EventArgs) Handles optBetween.CheckedChanged
        'If optBetween.Checked Then
        '    lblFrom.Text = "FROM : "
        '    lblTo.Visible = True : dtpTo.Visible = True
        'Else
        '    lblFrom.Text = "AS ON : "
        '    lblTo.Visible = False : dtpTo.Visible = False
        'End If
    End Sub
End Class
