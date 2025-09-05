Imports System.Data.OleDb
Imports System.IO

Public Class frmCategoryWisePurchase
    Dim objGridShower As frmGridDispDia

    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim dtCostCentre As New DataTable
    Dim dtEMP As New DataTable
    Dim dtCategory As New DataTable

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
        strSql += " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Focus()
    End Function


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim SelectedCostid As String = ""
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCostid = GetSelectedCostId(chkCmbCostCentre, False)
        Else
            SelectedCostid = "ALL"
        End If

        Dim selCatCode As String = Nothing
        If chkcmbCategory.Text = "ALL" Then
            selCatCode = "ALL"
        Else
            Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & ")"
            Dim dtCat1 As New DataTable
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCat1)
            If dtCat1.Rows.Count > 0 Then
                For i As Integer = 0 To dtCat1.Rows.Count - 1
                    selCatCode += dtCat1.Rows(i).Item("CATCODE").ToString + ","
                Next
                If selCatCode <> "" Then
                    selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                End If
            End If
        End If
        Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", False)
        Dim pureexch As String
        If rbtPurchase.Checked Then
            pureexch = "P"
        ElseIf rbtExchange.Checked Then
            pureexch = "E"
        Else
            pureexch = "ALL"
        End If
        Dim MAKE As String
        If rbtOwn.Checked Then
            MAKE = "W"
        ElseIf rbtOthers.Checked Then
            MAKE = "O"
        Else
            MAKE = "ALL"
        End If

        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_CATWISEPURCHASE"
        strSql += vbCrLf + " @TEMPTABLE ='TEMP" & systemId & "CATPURCHASE'"
        strSql += vbCrLf + " ,@DBNAME ='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYIDS ='" & strCompanyId & "'"
        strSql += vbCrLf + " ,@COSTIDS ='" & SelectedCostid & "'"
        strSql += vbCrLf + " ,@CATCODE ='" & selCatCode & "'"
        strSql += vbCrLf + " ,@EMPCODE ='" & chkempid & "'"
        strSql += vbCrLf + " ,@PUREXCH ='" & pureexch & "'"
        strSql += vbCrLf + " ,@MAKE ='" & MAKE & "'"

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
        strSql = "SELECT DISTINCT CATNAME FROM TEMPTABLEDB..TEMP" & systemId & "CATPURCHASE1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dtcat As New DataTable
        da.Fill(dtcat)
        'dtcat = dtGrid.DefaultView.ToTable(True, "CATNAME")

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns("PARTICULAR").Caption = "Bill No"
            Dim tempcolname As String = ""
            If dtcat.Rows.Count > 0 Then
                For k As Integer = 0 To dtcat.Rows.Count - 1
                    'tempcolname = dtcat.Rows(k).Item(0).ToString + " GRSWT"
                    'tempcolname = tempcolname + "~" + dtcat.Rows(k).Item(0).ToString + " WASTAGE"
                    .Columns.Add(dtcat.Rows(k).Item(0).ToString, GetType(String))
                    .Columns(dtcat.Rows(k).Item(0).ToString).Caption = dtcat.Rows(k).Item(0).ToString
                    tempcolname = ""
                Next
            End If
            .Columns.Add("TOTAL", GetType(String))
            .Columns("TOTAL").Caption = "TOTAL"
            .Columns.Add("SCROLL", GetType(String))
            .Columns("SCROLL").Caption = ""
        End With

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "CATEGORY WISE PURCHASE REPORT"
        Dim TITLE As String
        TITLE += " CATEGORY WISE PURCHASE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = TITLE

        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)

        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(1)

        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView, dtcat)
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        GridHead(dtcat)
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView, ByVal dttable As DataTable)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next

            .Columns("TRANNO").Visible = False
            .Columns("COLHEAD").Visible = False
            '.Columns("TOTALGRSWT").Visible = False
            '.Columns("TOTALWASTAGE").Visible = False
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            If dttable.Rows.Count > 0 Then
                For i As Integer = 0 To dttable.Rows.Count - 1
                    .Columns(dttable.Rows(i).Item(0).ToString & " GRSWT").HeaderText = "GRSWT"
                    .Columns(dttable.Rows(i).Item(0).ToString & " WASTAGE").HeaderText = "WASTAGE"
                    .Columns(dttable.Rows(i).Item(0).ToString & " GRSWT").Width = 90
                    .Columns(dttable.Rows(i).Item(0).ToString & " WASTAGE").Width = 90
                Next
            End If
            .Columns("TOTALGRSWT").HeaderText = "GRSWT"
            .Columns("TOTALWASTAGE").HeaderText = "WASTAGE"
            .Columns("TOTALGRSWT").Width = 90
            .Columns("TOTALWASTAGE").Width = 90
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
    Private Sub frmCategoryWisePurchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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

        strSql = " SELECT 'ALL' CATNAME,0 DISPLAYORDER "
        strSql += " UNION ALL"
        strSql += " SELECT CATNAME,2 DISPLAYORDER  FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE CATMODE IN ('B','P')"
        strSql += " ORDER BY DISPLAYORDER,CATNAME"
        dtCategory = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCategory)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtCategory, "CATNAME", , "ALL")

        strSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        strSql += " ORDER BY RESULT,EMPNAME"
        dtEMP = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtEMP)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")


        btnNew_Click(Me, New EventArgs)
    End Sub

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
        Dim obj As New frmCategoryWisePurchase_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmCategoryWisePurchase_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCategoryWisePurchase_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCategoryWisePurchase_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
    End Sub

    Private Sub dtpTo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.Leave
        strSql = " SELECT 'ALL' CATNAME,0 DISPLAYORDER "
        strSql += " UNION ALL"
        strSql += " SELECT CATNAME,DISPLAYORDER  FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE CATMODE IN ('B','P') AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE='PU' AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' )"
        strSql += " ORDER BY DISPLAYORDER,CATNAME"
        dtCategory = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCategory)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtCategory, "CATNAME", , "ALL")
    End Sub
End Class

Public Class frmCategoryWisePurchase_Properties
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
End Class