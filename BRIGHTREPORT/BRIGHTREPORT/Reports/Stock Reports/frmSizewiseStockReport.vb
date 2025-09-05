Imports System.Data.OleDb
Public Class frmSizewiseStockReport
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtItem As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtSize As New DataTable
    Dim dtSubItem As New DataTable
    Dim dtCompany As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtItemType As New DataTable
    Dim dtMetal As New DataTable
    Dim objGridShower As frmGridDispDia
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        dtpFrom.Focus()

        ''COSTCENTRE
        StrSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL'COSTID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & CNADMINDB & "..COSTCENTRE"
        StrSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCostCentre)
        cmbCostCentre.Items.Clear()
        For Each Row As DataRow In dtCostCentre.Rows
            cmbCostCentre.Items.Add(Row.Item("COSTNAME").ToString)
        Next
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False

        '' LOAD METAL
        StrSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        StrSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtItem)
        cmbMetal.Items.Clear()
        For Each Row As DataRow In dtItem.Rows
            cmbMetal.Items.Add(Row.Item("METALNAME").ToString)
        Next

        StrSql = vbCrLf + " SELECT 'ALL' SIZENAME,'ALL'SIZEID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT SIZENAME,CONVERT(VARCHAR,SIZEID),2 RESULT FROM " & cnAdminDb & "..ITEMSIZE"
        StrSql += vbCrLf + " ORDER BY RESULT,SIZENAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtSize)
        cmbSize.Items.Clear()
        For Each Row As DataRow In dtSize.Rows
            cmbSize.Items.Add(Row.Item("SIZENAME").ToString)
        Next
        btnNew_Click(Me, New EventArgs)
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcLoadCategory() As Integer
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " select CatName from " & cnAdminDb & "..Category "
        If cmbMetal.Text <> "ALL" Then
            strSql += " where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname = '" & cmbMetal.Text & "')"
        End If
        strSql += "  order by CatName"
        objGPack.FillCombo(strSql, cmbCategory, False)
        cmbCategory.Text = "ALL"
    End Function
    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function
    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As On Date"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub

    Private Sub chkCmbItem_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbItem.TextChanged
        cmbSubItemName.Items.Clear()
        strSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME,'ALL'SUBITEMID,1 RESULT,0 DISPLAYORDER"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT,DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & chkCmbItem.Text & "')"
        If _SubItemOrderByName Then
            strSql += vbCrLf + " ORDER BY RESULT,SUBITEMNAME"
        Else
            strSql += vbCrLf + " ORDER BY RESULT,DISPLAYORDER,SUBITEMNAME"
        End If

        da = New OleDbDataAdapter(strSql, cn)
        dtSubItem = New DataTable
        da.Fill(dtSubItem)
        For Each ro As DataRow In dtSubItem.Rows
            cmbSubItemName.Items.Add(ro.Item("SUBITEMNAME").ToString)
        Next
    End Sub

    Private Sub cmbMetal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcLoadItemName()
    End Sub

    Private Sub frmSizewiseStockReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        
        strSql = " SELECT 'ALL' NAME,'ALL' ITEMTYPEID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT NAME,CONVERT(vARCHAR,ITEMTYPEID),2 RESULT FROM " & cnAdminDb & "..ITEMTYPE"
        strSql += " ORDER BY RESULT,NAME"
        dtItemType = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemType)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")
       
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        btnNew_Click(Me, e)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcLoadCategory()
        dtpFrom.Value = GetServerDate()
        'objGridShower.gridView.DataSource = Nothing
        ' chkDiamond.Checked = False
        ' chkStone.Checked = False
        'BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")
        'BrighttechPack.GlobalMethods.FillCombo(cmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        ' chkAsOnDate.Checked = True
        funcLoadItemName()
        ' chkWithApproval.Checked = False
        cmbMetal.Select()
        Prop_Gets()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmSizeWiseStockReport_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbItemName = chkCmbItem.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_cmbSize = cmbSize.Text
        obj.p_chkCmbItemType = chkCmbItemType.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSizeWiseStockReport_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmSizeWiseStockReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSizeWiseStockReport_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        chkCmbItem.Text = obj.p_cmbItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        cmbSize.Text = obj.p_cmbSize
        chkCmbDesigner.Text = obj.p_chkCmbDesigner
        chkCmbItemType.Text = obj.p_chkCmbItemType
        chkCmbCompany.Text = obj.p_chkCmbCompany
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        Dim selCostId As String = Nothing
        Dim selSizeid As String = Nothing
        Dim selMETALID As String = Nothing
        Dim SelectedCompany As String
        Dim rType As String = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkCmbItem.Text = "ALL" Then
            selItemId = "ALL"
        ElseIf chkCmbItem.Text <> "" Then
            Dim sql As String = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & ")"
            Dim dtItem As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtItem.Rows.Count - 1
                    selItemId += dtItem.Rows(i).Item("ITEMID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selItemId <> "" Then
                    selItemId = Mid(selItemId, 1, selItemId.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        If cmbSubItemName.SelectedIndex >= 0 Then
            selSubItemId = dtSubItem.Rows(cmbSubItemName.SelectedIndex).Item("SUBITEMID").ToString
        Else
            selSubItemId = "ALL"
        End If
        If cmbCostCentre.SelectedIndex >= 0 Then
            selCostId = dtCostCentre.Rows(cmbCostCentre.SelectedIndex).Item("COSTID").ToString
        Else
            selCostId = "ALL"
        End If

        If chkCmbCompany.Text = "ALL" Then
            SelectedCompany = "ALL"
        Else
            SelectedCompany = GetSelectedCompanyId(chkCmbCompany, False)
        End If


        'If cmbMetal.SelectedIndex >= 0 Then
        '    selMETALID = dtMetal.Rows(cmbMetal.SelectedIndex).Item("METALID").ToString
        'Else
        '    SELMETALID = "ALL"
        'End If
        If cmbSize.SelectedIndex >= 0 Then
            selSizeid = dtSize.Rows(cmbSize.SelectedIndex).Item("SIZEID").ToString
        Else
            selSizeid = "ALL"
        End If

        If cmbMetal.Text = "ALL" Then
            selMETALID = "ALL"
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtTMETAL As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtTMETAL)
            If dtTMETAL.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtTMETAL.Rows.Count - 1
                    selMETALID += dtTMETAL.Rows(i).Item("METALID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selMETALID <> "" Then
                    selMETALID = Mid(selMETALID, 1, selMETALID.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_SIZEWISESTOCK"

        strSql += vbCrLf + " @ITEMID = '" & selItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & selSubItemId & "'"
        strSql += vbCrLf + " ,@COSTID = '" & selCostId & "'"
        strSql += vbCrLf + " ,@SIZEID='" & selSizeid & "'"
        strSql += vbCrLf + " ,@ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        strSql += vbCrLf + " ,@cnAdminDB='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@cnStockDB='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@WithApproval= '" & IIf(chkWithApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@METALID= '" & selMETALID & "'"

        Dim DtGrid As New DataTable("SUMMARY")
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)

        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtMergeHeader As New DataTable()

        dtMergeHeader = New DataTable("~MERGEHEADER")
        With dtMergeHeader

            .Columns.Add("SIZENAME", GetType(String))
            .Columns.Add("PCS~GRSWT~RATE", GetType(String))
            .Columns.Add("IPCS~IGRSWT~IRATE", GetType(String))
            .Columns.Add("RPCS~RGRSWT~RRATE", GetType(String))
            .Columns.Add("DIFFPCS~DIFFWT~DIFFRT", GetType(String))
            .Columns("SIZENAME").Caption = "PRODUCT"
            .Columns("PCS~GRSWT~RATE").Caption = "OPENING"
            .Columns("IPCS~IGRSWT~IRATE").Caption = "ISSUE"
            .Columns("RPCS~RGRSWT~RRATE").Caption = "RECEIPT"
            .Columns("DIFFPCS~DIFFWT~DIFFRT").Caption = "CLOSING"

        End With

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        Dim tit As String = ""
        objGridShower.Text = "SIZEWISE STOCK REPORT"
        tit = "SIZE WISE STOCK REPORT" + vbCrLf
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text
        Else
            tit += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        objGridShower.lblTitle.Text = tit & IIf(cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL", " :" & cmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit & IIf(cmbMetal.Text <> "" And cmbMetal.Text <> "ALL", " :" & cmbMetal.Text, "")
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(1)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = False

        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("SIZENAME")))
        objGridShower.gridViewHeader.Visible = True
        GridHead()
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .Columns("SIZENAME").Width = 240
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 100
            .Columns("RATE").Width = 100
            .Columns("IPCS").Width = 60
            .Columns("IGRSWT").Width = 100
            .Columns("IRATE").Width = 100
            .Columns("RPCS").Width = 60
            .Columns("RGRSWT").Width = 100
            .Columns("RRATE").Width = 100
            .Columns("DIFFPCS").Width = 100
            .Columns("DIFFWT").Width = 100
            .Columns("DIFFRT").Width = 100
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFRT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("SIZENAME").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("RATE").Visible = True
            .Columns("IPCS").Visible = True
            .Columns("IGRSWT").Visible = True
            .Columns("IRATE").Visible = True
            .Columns("RPCS").Visible = True
            .Columns("RGRSWT").Visible = True
            .Columns("RRATE").Visible = True
            .Columns("DIFFPCS").Visible = True
            .Columns("DIFFWT").Visible = True
            .Columns("DIFFRT").Visible = True
            .Columns("SIZENAME").HeaderText = "SUBPRODUCT"
            .Columns("RATE").HeaderText = "Rate"
            .Columns("IRATE").HeaderText = "Rate"
            .Columns("RRATE").HeaderText = "Rate"
            .Columns("DIFFRT").HeaderText = "Rate"
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            'objGridShower.gridViewHeader.Columns("COL1").Width = .Columns("COL1").Width
            'objGridShower.gridViewHeader.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0

            TEMPCOLWIDTH += .Columns("SIZENAME").Width
            objGridShower.gridViewHeader.Columns("SIZENAME").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("SIZENAME").HeaderText = "PRODUCT"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("PCS").Width + .Columns("GRSWT").Width + .Columns("RATE").Width
            objGridShower.gridViewHeader.Columns("PCS~GRSWT~RATE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("PCS~GRSWT~RATE").HeaderText = "OPENING"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("IPCS").Width + .Columns("IGRSWT").Width + .Columns("IRATE").Width
            objGridShower.gridViewHeader.Columns("IPCS~IGRSWT~IRATE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("IPCS~IGRSWT~IRATE").HeaderText = "ISSUE"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("RPCS").Width + .Columns("RGRSWT").Width + .Columns("RRATE").Width
            objGridShower.gridViewHeader.Columns("RPCS~RGRSWT~RRATE").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("RPCS~RGRSWT~RRATE").HeaderText = "RECEIPT"
            TEMPCOLWIDTH = 0

            TEMPCOLWIDTH += .Columns("DIFFPCS").Width + .Columns("DIFFWT").Width + .Columns("DIFFRT").Width
            objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT~DIFFRT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT~DIFFRT").HeaderText = "CLOSING"

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "Pieces"
                If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Gross Wt"
                'If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "Rate" Then .Columns(cnt).HeaderText = "Rate"
            Next
        End With
    End Sub

    Private Sub frmSizewiseStockReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class
Public Class frmSizeWiseStockReport_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbSubItemName As String = "ALL"
    Public Property p_cmbSubItemName() As String
        Get
            Return cmbSubItemName
        End Get
        Set(ByVal value As String)
            cmbSubItemName = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private chkSize As Boolean = False
    Public Property p_chkSize() As Boolean
        Get
            Return chkSize
        End Get
        Set(ByVal value As Boolean)
            chkSize = value
        End Set
    End Property
    Private cmbSize As String = "ALL"
    Public Property p_cmbSize() As String
        Get
            Return cmbSize
        End Get
        Set(ByVal value As String)
            cmbSize = value
        End Set
    End Property
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
    End Property
    Private chkCmbDesigner As String = "ALL"
    Public Property p_chkCmbDesigner() As String
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As String)
            chkCmbDesigner = value
        End Set
    End Property

    Private chkCmbCompany As String = "ALL"
    Public Property p_chkCmbCompany() As String
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As String)
            chkCmbCompany = value
        End Set
    End Property

    Private chkCmbItemType As String = "ALL"
    Public Property p_chkCmbItemType() As String
        Get
            Return chkCmbItemType
        End Get
        Set(ByVal value As String)
            chkCmbItemType = value
        End Set
    End Property

    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property

    Private rbtTag As Boolean = False
    Public Property p_rbtTag() As Boolean
        Get
            Return rbtTag
        End Get
        Set(ByVal value As Boolean)
            rbtTag = value
        End Set
    End Property

    Private rbtNonTag As Boolean = False
    Public Property p_rbtNonTag() As Boolean
        Get
            Return rbtNonTag
        End Get
        Set(ByVal value As Boolean)
            rbtNonTag = value
        End Set
    End Property

    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property

    Private chkNetWt As Boolean = False
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property

    Private chkExtraWt As Boolean = False
    Public Property p_chkExtraWt() As Boolean
        Get
            Return chkExtraWt
        End Get
        Set(ByVal value As Boolean)
            chkExtraWt = value
        End Set
    End Property
End Class