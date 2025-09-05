Imports System.Data.OleDb
Public Class ItemWiseStockFlow
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim dtItemName As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtcounter As New DataTable

    Private Sub ItemWiseStockFlow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LoadItem()
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "')"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItemName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , "ALL")
    End Sub

    Private Sub ItemWiseStockFlow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)

        cmbMetal.Items.Clear()
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        cmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtcounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        BrighttechPack.GlobalMethods.FillCombo(CHKCOUNTER, dtcounter, "ITEMCTRNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkAsOnDate.Checked = True
        'cmbMetal.Text = "ALL"
        LoadItem()
        'rbtOrderItemId.Checked = True
        ' chkTag.Checked = True
        'chkNonTag.Checked = True
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        chkAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If chkTag.Checked = False And chkNonTag.Checked = False Then
            chkTag.Checked = True
            chkNonTag.Checked = True
        End If
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
        Dim chkItemId As String = GetQryStringForSp(chkCmbItemName.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME")
        Dim chkCompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")
        Dim chkCounterId As String = GetQryStringForSp(CHKCOUNTER.Text, cnAdminDb & "..ITEMCOUNTER ", "ITEMCTRID", "ITEMCTRNAME")
        Dim metalId As String = "ALL"
        Dim stockType As String = ""
        If chkTag.Checked And chkNonTag.Checked Then
            stockType = "B"
        ElseIf chkTag.Checked Then
            stockType = "T"
        ElseIf chkNonTag.Checked Then
            stockType = "N"
        End If
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            metalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'", , "ALL")
        End If
        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_STOCKFLOW"
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@ITEMID_ORDER = '" & IIf(rbtOrderItemId.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & chkItemId & "'"
        strSql += vbCrLf + " ,@COSTID = '" & chkCostId & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & chkCompanyId & "'"
        strSql += vbCrLf + " ,@METALID = '" & metalId & "'"
        strSql += vbCrLf + " ,@STOCKTYPE = '" & stockType & "'"
        strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubitem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@COUNTER   ='" & IIf(chkAllCounter.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@COUNTERID  ='" & chkCounterId & "'"
        strSql += vbCrLf + " ,@BEADS ='" & IIf(chkWithBeads.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHCUMSTK='" & IIf(ChkwithCumStk.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHAPP='" & IIf(ChkWithApproval.Checked, "Y", "N") & "'"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ITEM WISE STOCK FLOW"
        Dim tit As String = "ITEM WISE STOCK FLOW REPORT"
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += vbCrLf
        tit += " METAL : " & cmbMetal.Text
        If chkCmbItemName.CheckedItems.Count > 1 Then
            tit += " ITEM : MULTIPLE SELECTION"
        Else
            tit += " ITEM : " & chkCmbItemName.Text & ""
        End If
        tit += vbCrLf & " COSTCENTRE : " & IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 5)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Show()
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        objGridShower.gridView.Columns(0).Frozen = True
        Prop_Sets()
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR]"
        strSql += " ,''[OPCS~OGRSWT]"
        strSql += " ,''[RPCS~RGRSWT]"
        strSql += " ,''[RPCS_PA~RGRSWT_PA]"
        strSql += " ,''[RPCS_AP~RGRSWT_AP]"
        strSql += " ,''[RPCS_TR~RGRSWT_TR]"
        strSql += " ,''[RPCS_RE~RGRSWT_RE]"
        strSql += " ,''[RPCS_PU~RGRSWT_PU]"
        strSql += " ,''[IPCS~IGRSWT]"
        strSql += " ,''[IPCS_MI~IGRSWT_MI]"
        strSql += " ,''[IPCS_PA~IGRSWT_PA]"
        strSql += " ,''[IPCS_AP~IGRSWT_AP]"
        strSql += " ,''[IPCS_TR~IGRSWT_TR]"
        strSql += " ,''[IPCS_NT~IGRSWT_NT]"
        strSql += " ,''[IPCS_NTTT~IGRSWT_NTTT]"
        strSql += " ,''[CPCS~CGRSWT]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("OPCS~OGRSWT").HeaderText = "OPEING"
        gridviewHead.Columns("RPCS~RGRSWT").HeaderText = "ACTUAL REC"
        gridviewHead.Columns("RPCS_PA~RGRSWT_PA").HeaderText = "PARTLY REC"
        gridviewHead.Columns("RPCS_AP~RGRSWT_AP").HeaderText = "APPROVAL REC"
        gridviewHead.Columns("RPCS_TR~RGRSWT_TR").HeaderText = "TRANSFER REC"
        gridviewHead.Columns("RPCS_RE~RGRSWT_RE").HeaderText = "RETURN REC"
        gridviewHead.Columns("RPCS_PU~RGRSWT_PU").HeaderText = "PURCHASE REC"
        gridviewHead.Columns("IPCS~IGRSWT").HeaderText = "ACTUAL ISS"
        gridviewHead.Columns("IPCS_MI~IGRSWT_MI").HeaderText = "MISC ISS"
        gridviewHead.Columns("IPCS_PA~IGRSWT_PA").HeaderText = "PARTLY DIFF"
        gridviewHead.Columns("IPCS_AP~IGRSWT_AP").HeaderText = "APPROVAL ISS"
        gridviewHead.Columns("IPCS_TR~IGRSWT_TR").HeaderText = "TRANSFER ISS"
        gridviewHead.Columns("IPCS_NT~IGRSWT_NT").HeaderText = "NT ISS"
        gridviewHead.Columns("IPCS_NTTT~IGRSWT_NTTT").HeaderText = "NT-TG ISS"

        gridviewHead.Columns("CPCS~CGRSWT").HeaderText = "CLOSING"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            .Columns("ITEM").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("METAL").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("ICOUNTERNAME").Visible = False
            .Columns("OPCS").HeaderText = "PCS"
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RPCS_PA").HeaderText = "PCS"
            .Columns("RGRSWT_PA").HeaderText = "GRSWT"
            .Columns("RPCS_AP").HeaderText = "PCS"
            .Columns("RGRSWT_AP").HeaderText = "GRSWT"
            .Columns("RPCS_TR").HeaderText = "PCS"
            .Columns("RGRSWT_TR").HeaderText = "GRSWT"
            .Columns("RPCS_RE").HeaderText = "PCS"
            .Columns("RGRSWT_RE").HeaderText = "GRSWT"
            .Columns("RPCS_PU").HeaderText = "PCS"
            .Columns("RGRSWT_PU").HeaderText = "GRSWT"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("IPCS_MI").HeaderText = "PCS"
            .Columns("IGRSWT_MI").HeaderText = "GRSWT"
            .Columns("IPCS_PA").HeaderText = "PCS"
            .Columns("IGRSWT_PA").HeaderText = "GRSWT"
            .Columns("IPCS_AP").HeaderText = "PCS"
            .Columns("IGRSWT_AP").HeaderText = "GRSWT"
            .Columns("IPCS_TR").HeaderText = "PCS"
            .Columns("IGRSWT_TR").HeaderText = "GRSWT"
            .Columns("IPCS_NT").HeaderText = "PCS"
            .Columns("IGRSWT_NT").HeaderText = "GRSWT"
            .Columns("IPCS_NTTT").HeaderText = "PCS"
            .Columns("IGRSWT_NTTT").HeaderText = "GRSWT"
            .Columns("CPCS").HeaderText = "PCS"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New ItemWiseStockFlow_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_cmbMetal = cmbMetal.Text
        GetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName)
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtOrderItemId = rbtOrderItemId.Checked()
        obj.p_rbtOrderItemName = rbtOrderItemName.Checked
        obj.p_chkWithSubitem = chkWithSubitem.Checked
        obj.p_chkTag = chkTag.Checked
        obj.p_chkWithBeads = chkWithBeads.Checked
        obj.p_chkNonTag = chkNonTag.Checked
        obj.p_chkWithApproval = ChkWithApproval.Checked
        obj.p_chkWithCumStk = ChkwithCumStk.Checked
        SetSettingsObj(obj, Me.Name, GetType(ItemWiseStockFlow_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New ItemWiseStockFlow_Properties
        GetSettingsObj(obj, Me.Name, GetType(ItemWiseStockFlow_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        cmbMetal.Text = obj.p_cmbMetal
        SetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName, "ALL")
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
        rbtOrderItemId.Checked = obj.p_rbtOrderItemId
        rbtOrderItemName.Checked = obj.p_rbtOrderItemName
        chkWithSubitem.Checked = obj.p_chkWithSubitem
        chkTag.Checked = obj.p_chkTag
        chkNonTag.Checked = obj.p_chkNonTag
        chkWithBeads.Checked = obj.p_chkWithBeads
        ChkWithApproval.Checked = obj.p_chkWithApproval
        ChkwithCumStk.Checked = obj.p_chkWithCumStk
    End Sub

    Private Sub chkNonTag_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNonTag.CheckedChanged

    End Sub

    Private Sub chkTag_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTag.CheckedChanged

    End Sub

    Private Sub chkAllCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllCounter.CheckedChanged
        If chkAllCounter.Checked = True Then
            CHKCOUNTER.Visible = True
        Else
            CHKCOUNTER.Visible = False
        End If

    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click

    End Sub
End Class


Public Class ItemWiseStockFlow_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
    End Property

    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private chkCmbItemName As New List(Of String)
    Public Property p_chkCmbItemName() As List(Of String)
        Get
            Return chkCmbItemName
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItemName = value
        End Set
    End Property
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property

    Private rbtOrderItemId As Boolean = True
    Public Property p_rbtOrderItemId() As Boolean
        Get
            Return rbtOrderItemId
        End Get
        Set(ByVal value As Boolean)
            rbtOrderItemId = value
        End Set
    End Property
    Private rbtOrderItemName As Boolean = False
    Public Property p_rbtOrderItemName() As Boolean
        Get
            Return rbtOrderItemName
        End Get
        Set(ByVal value As Boolean)
            rbtOrderItemName = value
        End Set
    End Property
    Private chkWithSubitem As Boolean = False
    Public Property p_chkWithSubitem() As Boolean
        Get
            Return chkWithSubitem
        End Get
        Set(ByVal value As Boolean)
            chkWithSubitem = value
        End Set
    End Property
    Private chkTag As Boolean = True
    Public Property p_chkTag() As Boolean
        Get
            Return chkTag
        End Get
        Set(ByVal value As Boolean)
            chkTag = value
        End Set
    End Property
    Private chkNonTag As Boolean = True
    Public Property p_chkNonTag() As Boolean
        Get
            Return chkNonTag
        End Get
        Set(ByVal value As Boolean)
            chkNonTag = value
        End Set
    End Property
    Private chkWithBeads As Boolean = True
    Public Property p_chkWithBeads() As Boolean
        Get
            Return chkWithBeads
        End Get
        Set(ByVal value As Boolean)
            chkWithBeads = value
        End Set
    End Property
    Private chkWithApproval As Boolean = True
    Public Property p_chkWithApproval() As Boolean
        Get
            Return chkWithApproval
        End Get
        Set(ByVal value As Boolean)
            chkWithApproval = value
        End Set
    End Property
    Private chkWithCumStk As Boolean = True
    Public Property p_chkWithCumStk() As Boolean
        Get
            Return chkWithCumStk
        End Get
        Set(ByVal value As Boolean)
            chkWithCumStk = value
        End Set
    End Property
End Class