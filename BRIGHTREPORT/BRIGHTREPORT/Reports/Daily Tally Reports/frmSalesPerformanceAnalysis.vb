Imports System.Data.OleDb
Imports System.IO

Public Class frmSalesPerformanceAnalysis
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim dtCostCentre As New DataTable
    Dim dtEMP As New DataTable
    Dim dtCategory As New DataTable
    Dim dtCompany As New DataTable


    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Focus()
    End Function


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_SALESPERFORMANCE_ANALYSIS"
        strSql += vbCrLf + " @TEMPTABLE ='TEMP" & systemId & "DISCSALE'"
        strSql += vbCrLf + " ,@DBNAME ='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYIDS ='" & GetSelectedComId(chkCmbCompany, False) & "'"
        strSql += vbCrLf + " ,@COSTIDS ='" & GetSelectedCostId(chkCmbCostCentre, False) & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtGrid As New DataTable
        If dss.Tables.Contains("Table") Then
            dtGrid = dss.Tables(0)
        End If
        dss.Tables.Clear()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DISCOUNT WISE SALES REPORT"
        Dim TITLE As String
        TITLE += " DISCOUNT WISE SALES REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = TITLE

        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.dsGrid.Tables.Add(dtMergeHeader)

        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(1)

        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False

        'GridHead(dtcat)
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("TRANNO").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("PCS").Visible = False
            .Columns("NETWT").Visible = False
            .Columns("COLHEAD").Visible = False

            '.Columns("PARTICULAR").HeaderText = "PARTICULAR"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With

    End Sub

    Private Sub GridHead(ByVal dtTable As DataTable)
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0

            TEMPCOLWIDTH += .Columns("PARTICULAR").Width
            objGridShower.gridViewHeader.Columns("PARTICULAR").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("PARTICULAR").HeaderText = "Bill No"
            TEMPCOLWIDTH = 0
            If dtTable.Rows.Count > 0 Then
                For j As Integer = 0 To dtTable.Rows.Count - 1
                    TEMPCOLWIDTH += .Columns(dtTable.Rows(j).Item(0).ToString & " GRSWT").Width + .Columns(dtTable.Rows(j).Item(0).ToString & " WASTAGE").Width
                    objGridShower.gridViewHeader.Columns(dtTable.Rows(j).Item(0).ToString).Width = TEMPCOLWIDTH
                    objGridShower.gridViewHeader.Columns(dtTable.Rows(j).Item(0).ToString).HeaderText = dtTable.Rows(j).Item(0).ToString
                    TEMPCOLWIDTH = 0
                Next
            End If
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("TOTALGRSWT").Width + .Columns("TOTALWASTAGE").Width
            objGridShower.gridViewHeader.Columns("TOTAL").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("TOTAL").HeaderText = "TOTAL"

            objGridShower.gridViewHeader.Columns("SCROLL").Visible = False

        End With
    End Sub
    Private Sub frmDiscountWiseSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        btnNew_Click(Me, New EventArgs)
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

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmDiscountWiseSales_Properties
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmDiscountWiseSales_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmDiscountWiseSales_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmDiscountWiseSales_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
    End Sub
End Class

Public Class frmDiscountWiseSales_Properties
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
End Class