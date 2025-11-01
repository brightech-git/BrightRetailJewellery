Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Web.UI.WebControls
Imports System.Windows.Controls.Primitives
Imports System.Xml
Imports com.ms.win32
Imports java.lang
Public Class frmReorderOrderReport
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
        If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)

        strSql = $";with cte as("
        strSql += vbCrLf + $" select (ITEMMAST.ITEMNAME + ' - ' + isnull(ITEMSIZE.SIZENAME,'') + ' - ' + STKREORDER.RANGECAPTION) PARTICULAR,STKREORDER.MINPIECE REORDERPCS,sum(ITEMTAG.PCS) CLSPCS,STKREORDER.ITEMID,STKREORDER.SIZEID,STKREORDER.RANGECAPTION from {cnAdminDb}..ITEMTAG"
        strSql += vbCrLf + $" left join {cnAdminDb}..STKREORDER on ITEMTAG.ITEMID = STKREORDER.ITEMID and ITEMTAG.SIZEID = STKREORDER.SIZEID"
        strSql += vbCrLf + $" left join {cnAdminDb}..ITEMMAST on STKREORDER.ITEMID = ITEMMAST.ITEMID"
        strSql += vbCrLf + $" left join {cnAdminDb}..ITEMSIZE on STKREORDER.ITEMID = ITEMSIZE.ITEMID and STKREORDER.SIZEID = ITEMSIZE.SIZEID"
        strSql += vbCrLf + $" where 1=1"
        If itemid <> "ALL" Then strSql += vbCrLf + $" and STKREORDER.ITEMID in ({itemid})"
        strSql += vbCrLf + $" and ISSDATE is null"
        strSql += vbCrLf + $" and (ITEMTAG.GRSWT between STKREORDER.FROMWEIGHT and STKREORDER.TOWEIGHT)"
        strSql += vbCrLf + $" group by ITEMMAST.ITEMNAME,ITEMSIZE.SIZENAME,STKREORDER.RANGECAPTION,STKREORDER.MINPIECE,STKREORDER.ITEMID,STKREORDER.SIZEID,STKREORDER.RANGECAPTION"
        strSql += vbCrLf + $" )"
        strSql += vbCrLf + $" select PARTICULAR,REORDERPCS,CLSPCS,(case when (REORDERPCS-CLSPCS) > 0 then (REORDERPCS-CLSPCS) else 0 end) SHORTAGE,"
        strSql += vbCrLf + $" (case when (REORDERPCS-CLSPCS) < 0 then (REORDERPCS-CLSPCS)*-1 else 0 end) EXCESS,ITEMID,SIZEID,RANGECAPTION from cte"
        strSql += vbCrLf + $" order by PARTICULAR"
        cmd = New OleDb.OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)

        dtSource = New DataTable
        da.Fill(dtSource)
        If dtSource.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtSource
            tabMain.SelectedTab = tabView
            gridView.Columns("ITEMID").Visible = False
            gridView.Columns("SIZEID").Visible = False
            gridView.Columns("RANGECAPTION").Visible = False

            Dim tit As String
            tit = " REORDER STOCK REPORT " + vbCrLf
            If chkCmbMetal.Text.ToString <> "ALL" And chkCmbMetal.Text.ToString <> "" Then
                tit += " FOR METAL " & chkCmbMetal.Text.ToString & ""
            End If
            lblTitle.Text = tit.ToString
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
        gridView.DataSource = Nothing
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        funcLoadItemName()
        If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        strSql = " SELECT 'ALL' Caption,0 RESULT UNION ALL "
        strSql += "SELECT DISTINCT CAPTION,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        chkCmbMetal.Select()
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
    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.D Then
                Dim itemId As Integer = gridView.Item("ITEMID", gridView.CurrentRow.Index).Value
                Dim sizeId As Integer = gridView.Item("SIZEID", gridView.CurrentRow.Index).Value
                Dim rangeCaption As String = gridView.Item("RANGECAPTION", gridView.CurrentRow.Index).Value.ToString
                Dim particular As String = gridView.Item("PARTICULAR", gridView.CurrentRow.Index).Value.ToString

                strSql = $" select '{particular} - ' + SUBITEMNAME PARTICULAR,count(tagno) as TagCount,sum(ITEMTAG.PCS)PCS,sum(ITEMTAG.GRSWT) GRSWT,sum(ITEMTAG.NETWT) NETWT from {cnAdminDb}..ITEMTAG"
                strSql += vbCrLf + $" left join {cnAdminDb}..SUBITEMMAST on ITEMTAG.SUBITEMID =SUBITEMMAST.SUBITEMID"
                strSql += vbCrLf + $" join {cnAdminDb}..STKREORDER on ITEMTAG.ITEMID = STKREORDER.ITEMID and ITEMTAG.SIZEID = STKREORDER.SIZEID"
                strSql += vbCrLf + $" where 1=1"
                strSql += vbCrLf + $" and ISSDATE is null"
                strSql += vbCrLf + $" and ITEMTAG.ITEMID = {itemId}"
                strSql += vbCrLf + $" and ITEMTAG.SIZEID = {sizeId}"
                strSql += vbCrLf + $" and STKREORDER.RANGECAPTION = '{rangeCaption}'"
                strSql += vbCrLf + $" and (ITEMTAG.GRSWT between STKREORDER.FROMWEIGHT and STKREORDER.TOWEIGHT) group by SUBITEMNAME"
                strSql += vbCrLf + $" UNION"
                strSql += vbCrLf + $" select '    TOTAL' PARTICULAR,count(tagno) as TagCount,sum(ITEMTAG.PCS)PCS,sum(GRSWT) GRSWT,sum(NETWT) NETWT from {cnAdminDb}..ITEMTAG"
                strSql += vbCrLf + $" join {cnAdminDb}..STKREORDER on ITEMTAG.ITEMID = STKREORDER.ITEMID and ITEMTAG.SIZEID = STKREORDER.SIZEID"
                strSql += vbCrLf + $" where 1=1"
                strSql += vbCrLf + $" and ISSDATE is null"
                strSql += vbCrLf + $" and ITEMTAG.ITEMID = {itemId}"
                strSql += vbCrLf + $" and ITEMTAG.SIZEID = {sizeId}"
                strSql += vbCrLf + $" and STKREORDER.RANGECAPTION = '{rangeCaption}'"
                strSql += vbCrLf + $" and (ITEMTAG.GRSWT between STKREORDER.FROMWEIGHT and STKREORDER.TOWEIGHT)"
                strSql += vbCrLf + $" order by PARTICULAR desc"
                Dim dt As New DataTable
                cmd = New OleDb.OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Dim ofrmPurchaseOrderDetail As New frmPurchaseOrderDetail(dt)
                    ofrmPurchaseOrderDetail.Text = ""
                    ofrmPurchaseOrderDetail.lblHead.Text = "REORDER STOCK TAG DETAILS" + vbCrLf + $"FOR THE PARTICULAR {particular}"
                    If ofrmPurchaseOrderDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Else
                        Exit Sub
                    End If
                Else
                    MessageBox.Show("No Details found")
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


End Class
