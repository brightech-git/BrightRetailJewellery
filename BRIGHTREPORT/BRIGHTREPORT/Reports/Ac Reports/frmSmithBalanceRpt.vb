Imports System.Data.OleDb
Public Class frmSmithBalanceRpt
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim ds As New DataSet
    Dim dt As New DataTable

    Private Sub frmSmithBalanceRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcnew()
    End Sub
    Private Sub funcnew()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        LoadCompany(chkLstCompany)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub
    Public Function GetSelectedMETALId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
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
    Public Function GetSelectedcostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
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
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim Selectedmetalid As String = GetSelectedMETALId(chkLstMetal, False)
        Dim SelectedCostId As String = GetSelectedcostId(chkLstCostCentre, False)
        Dim chkwt As String
        If chkGrstWt.Checked = True Then chkwt = "G"
        If chknetwt.Checked = True Then chkwt = chkwt + "N"
        If Me.chkpurwt.Checked = True Then chkwt = chkwt + "P"
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHABSTRACT"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & systemId & "SMITHABSTRACT'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
        strSql += vbCrLf + ",@METALID='" & Selectedmetalid & "'"
        strSql += vbCrLf + ",@COMPANYID='" & SelectedCompanyId & "'"
        strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
         
        da = New OleDbDataAdapter(strSql, cn)
        ds = New DataSet()
        dt = New DataTable()
        da.Fill(ds)
        dt = ds.Tables(0)
        If dt.Rows.Count > 0 Then
            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Size = Me.Size
            objGridShower.Text = "SMITH ABSTRACT REPORT"
            Dim tit As String = "SMITH ABSTRACT REPORT "
            tit += dtpFrom.Text + " TO " + dtpTo.Text
            objGridShower.lblTitle.Text = tit
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf objGridShower.gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name

            objGridShower.dsGrid = ds.Copy()
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            'DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)
            GridViewHeaderStyle()
            objGridShower.formuser = userId
            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.gridViewHeader.Visible = True
            'GridViewHeaderCreator(objGridShower.gridViewHeader)
            ' FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmSmithBalanceRpt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        objGridShower.gridView.Columns("ACNAME").Width = 250

        With objGridShower.gridView

            .Columns("ACNAME").HeaderText = "NAME"
            If .Columns.Contains("OPGRWT") Then objGridShower.gridView.Columns("OPGRWT").HeaderText = "GRS WT"
            If .Columns.Contains("OPNETWT") Then objGridShower.gridView.Columns("OPNETWT").HeaderText = "NET WT"
            If .Columns.Contains("OPPUREWT") Then objGridShower.gridView.Columns("OPPUREWT").HeaderText = "PURE WT"

            If .Columns.Contains("RGRWT") Then objGridShower.gridView.Columns("RGRWT").HeaderText = "GRS WT"
            If .Columns.Contains("RNETWT") Then objGridShower.gridView.Columns("RNETWT").HeaderText = "NET WT"
            If .Columns.Contains("RPUREWT") Then objGridShower.gridView.Columns("RPUREWT").HeaderText = "PURE WT"

            If .Columns.Contains("IGRWT") Then objGridShower.gridView.Columns("IGRWT").HeaderText = "GRS WT"
            If .Columns.Contains("INETWT") Then objGridShower.gridView.Columns("INETWT").HeaderText = "NET WT"
            If .Columns.Contains("IPUREWT") Then objGridShower.gridView.Columns("IPUREWT").HeaderText = "PURE WT"

            If .Columns.Contains("CLGRWT") Then objGridShower.gridView.Columns("CLGRWT").HeaderText = "GRS WT"
            If .Columns.Contains("CLNETWT") Then objGridShower.gridView.Columns("CLNETWT").HeaderText = "NET WT"
            If .Columns.Contains("CLPUREWT") Then objGridShower.gridView.Columns("CLPUREWT").HeaderText = "PURE WT"

        End With


       
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPGRWT~OPNETWT~OPPUREWT", GetType(String))
            .Columns.Add("RGRWT~RNETWT~RPUREWT", GetType(String))
            .Columns.Add("IGRWT~INETWT~IPUREWT", GetType(String))
            .Columns.Add("CLGRWT~CLNETWT~CLPUREWT", GetType(String))
        End With
        With objGridShower.gridViewHeader
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = "SMITH/DEALER"
            .Columns("PARTICULAR").Width = objGridShower.gridView.Columns("ACNAME").Width
            .Columns("OPGRWT~OPNETWT~OPPUREWT").HeaderText = "OPENING "
            .Columns("RGRWT~RNETWT~RPUREWT").HeaderText = "RECEIPT"
            .Columns("IGRWT~INETWT~IPUREWT").HeaderText = "ISSUE"
            .Columns("CLGRWT~CLNETWT~CLPUREWT").HeaderText = "CLOSING"
            Dim mcolwidth As Integer = 0
            If objGridShower.gridView.Columns.Contains("OPGRWT") Then mcolwidth = objGridShower.gridView.Columns("OPGRWT").Width
            If objGridShower.gridView.Columns.Contains("OPNETWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("OPNETWT").Width
            If objGridShower.gridView.Columns.Contains("OPPUREWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("OPPUREWT").Width
            .Columns("OPGRWT~OPNETWT~OPPUREWT").Width = mcolwidth

            mcolwidth = 0
            If objGridShower.gridView.Columns.Contains("RGRWT") Then mcolwidth = objGridShower.gridView.Columns("RGRWT").Width
            If objGridShower.gridView.Columns.Contains("RNETWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("RNETWT").Width
            If objGridShower.gridView.Columns.Contains("RPUREWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("RPUREWT").Width
            .Columns("RGRWT~RNETWT~RPUREWT").Width = mcolwidth
            mcolwidth = 0
            If objGridShower.gridView.Columns.Contains("IGRWT") Then mcolwidth = objGridShower.gridView.Columns("IGRWT").Width
            If objGridShower.gridView.Columns.Contains("INETWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("INETWT").Width
            If objGridShower.gridView.Columns.Contains("IPUREWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("IPUREWT").Width
            .Columns("IGRWT~INETWT~IPUREWT").Width = mcolwidth
            mcolwidth = 0
            If objGridShower.gridView.Columns.Contains("CLGRWT") Then mcolwidth = objGridShower.gridView.Columns("CLGRWT").Width
            If objGridShower.gridView.Columns.Contains("CLNETWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("CLNETWT").Width
            If objGridShower.gridView.Columns.Contains("CLPUREWT") Then mcolwidth = mcolwidth + objGridShower.gridView.Columns("CLPUREWT").Width
            .Columns("CLGRWT~CLNETWT~CLPUREWT").Width = mcolwidth
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


            objGridShower.gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To objGridShower.gridView.ColumnCount - 1
                If objGridShower.gridView.Columns(cnt).Visible Then colWid += objGridShower.gridView.Columns(cnt).Width
            Next

        End With

    End Sub

    Function funcColWidth() As Integer
        With objGridShower.gridViewHeader
            If .DataSource Is Nothing Then Exit Function
            ' .Columns("SCROLL").HeaderText = ""
            ' .Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width

            '.Columns("SCROLL").Visible = CType(objGridShower.gridView.Controls(0), HScrollBar).Visible
            '.Columns("SCROLL").Width = CType(objGridShower.gridView.Controls(1), VScrollBar).Width
        End With
    End Function
End Class