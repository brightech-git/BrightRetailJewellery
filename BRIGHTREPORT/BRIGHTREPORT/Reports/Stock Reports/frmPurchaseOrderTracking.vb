Imports System.Data.OleDb
Imports System.Xml
Imports System.IO
Imports com.ms.win32
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports System.Windows.Controls.Primitives
Public Class frmPurchaseOrderTracking
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
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridviewDetail.Visible = False
        strSql = $"	with cte1 as(	"
        strSql += vbCrLf + $"	select PONUMBER,sum(PO_PIECES)PO_PIECES from {cnAdminDb}..PURCHASEORDER a	"
        strSql += vbCrLf + $"	where 1=1	"
        strSql += vbCrLf + $"	and TRANDATE between '{dtpFrom.Value}' and '{dtpTo.Value}'	"
        strSql += vbCrLf + $"	group by PONUMBER	"
        strSql += vbCrLf + $"	),	"
        strSql += vbCrLf + $"	cte2 as(	"
        strSql += vbCrLf + $"	select PONUMBER,sno,sum(a.pcs)LOTPCS from {cnAdminDb}..ITEMLOT a	"
        strSql += vbCrLf + $"	where ISNULL(ponumber,'') <> ''	"
        strSql += vbCrLf + $"	group by PONUMBER,sno	"
        strSql += vbCrLf + $"	),	"
        strSql += vbCrLf + $"	cte3 as(	"
        strSql += vbCrLf + $"	select lotsno,sum(pcs)TAGPCS from {cnAdminDb}..itemtag group by LOTSNO 	"
        strSql += vbCrLf + $"	),	"
        strSql += vbCrLf + $"	cte4 as(	"
        strSql += vbCrLf + $"	select a.PONUMBER,sum(a.LOTPCS)LOTPCS,sum(b.TAGPCS)TAGPCS from cte2 a	"
        strSql += vbCrLf + $"	join cte3 b on a.SNO = b.LOTSNO 	"
        strSql += vbCrLf + $"	group by  a.PONUMBER	"
        strSql += vbCrLf + $"	)	"
        strSql += vbCrLf + $"	select a.PONUMBER,isnull(a.PO_PIECES,0)PO_PIECES,isnull(b.LOTPCS,0)LOTPCS,isnull(b.TAGPCS,0)TAGPCS,	"
        strSql += vbCrLf + $"	(isnull(b.LOTPCS,0)-isnull(b.TAGPCS,0))PENDINGLOTPCS,	"
        strSql += vbCrLf + $"	(isnull(a.PO_PIECES,0)-isnull(b.LOTPCS,0))PENDINGPOPCS from cte1 a	"
        strSql += vbCrLf + $"	left join cte4 b on a.ponumber = b.ponumber	"

        cmd = New OleDb.OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)

        dtSource = New DataTable
        da.Fill(dtSource)
        If dtSource.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtSource
            tabMain.SelectedTab = tabView

            Dim tit As String
            tit = " PURCHASE ORDER TRACKING" + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
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
        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
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

    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.D Then
                Dim _poNo As String = gridView.Item("PONUMBER", gridView.CurrentRow.Index).Value.ToString
                Dim dt As New DataTable

                strSql = $"	select c.ITEMNAME,d.SUBITEMNAME,b.LOTNO,TAGNO,a.GRSWT from {cnAdminDb}..ITEMTAG a	"
                strSql += vbCrLf + $"	join {cnAdminDb}..ITEMLOT b on b.SNO = a.LOTSNO 	"
                strSql += vbCrLf + $"	join {cnAdminDb}..ITEMMAST c on c.ITEMID = a.ITEMID	"
                strSql += vbCrLf + $"	join {cnAdminDb}..SUBITEMMAST d on d.SUBITEMID = a.SUBITEMID 	"
                strSql += vbCrLf + $"	where b.PONUMBER = '{_poNo}'	"

                cmd = New OleDb.OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Dim ofrmPurchaseOrderDetail As New frmPurchaseOrderDetail(dt)
                    ofrmPurchaseOrderDetail.lblHead.Text = $"PURCHASE ORDER TAG DETAILS { vbCrLf } PONUMBER : {_poNo}"
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
