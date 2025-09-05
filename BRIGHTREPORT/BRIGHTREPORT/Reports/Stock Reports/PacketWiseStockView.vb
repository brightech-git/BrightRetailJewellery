Imports System.Data.OleDb
Public Class PacketWiseStockView
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim dtItemName As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtCostCentre As New DataTable

    Private Sub PacketWiseStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub PacketWiseStockView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y' AND STOCKTYPE = 'P'"
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItemName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , "ALL")
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(VARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo_OWN.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo_OWN.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom_OWN.Value = GetServerDate()
        dtpTo_OWN.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtItemName, "ITEMNAME", , "ALL")
        Prop_Gets()
        chkAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom_OWN.Value) Then dtpFrom_OWN.Focus() : Exit Sub
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
        Dim chkItemId As String = GetQryStringForSp(chkCmbItemName.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME")
        Dim chkDesignerId As String = GetQryStringForSp(chkCmbDesigner.Text, cnAdminDb & "..DESIGNER", "DESIGNERID", "DESIGNERNAME")
        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_PACKETSTOCK"
        strSql += vbCrLf + " @FRMDATE = '" & dtpFrom_OWN.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom_OWN.Value.ToString("yyyy-MM-dd"), dtpTo_OWN.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubitem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@GROUPBYITEM = '" & IIf(chkGroupbyItem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@GROUPBYDESIGNER = '" & IIf(chkGroupByDesigner.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & chkItemId & "'"
        strSql += vbCrLf + " ,@COSTID = '" & chkCostId & "'"
        strSql += vbCrLf + " ,@DESIGNERID = '" & chkDesignerId & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,@WITHVALUE = '" & IIf(chkWithValue.Checked, "Y", "N") & "'"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Prop_Sets()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "PACKET WISE STOCK BALANCE"
        Dim tit As String = "PACKET WISE STOCK BALANCE REPORT" + vbCrLf
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom_OWN.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom_OWN.Text + " TO " + dtpTo_OWN.Text
        End If
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
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

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR~SUBITEMNAME]"
        strSql += " ,''[OPCS~OGRSWT~ODIAPCS~ODIAWT]"
        strSql += " ,''[RPCS~RGRSWT~RDIAPCS~RDIAWT]"
        strSql += " ,''[IPCS~IGRSWT~IDIAPCS~IDIAWT]"
        strSql += " ,''[CPCS~CGRSWT~CDIAPCS~CDIAWT]"
        strSql += " ,''[RATE],''[PURRATE],''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        Dim colName As String = "Apr-Val"
        Dim s() As String = colName.Split("-")

        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~SUBITEMNAME").HeaderText = ""
        gridviewHead.Columns("OPCS~OGRSWT~ODIAPCS~ODIAWT").HeaderText = "OPEING"
        gridviewHead.Columns("RPCS~RGRSWT~RDIAPCS~RDIAWT").HeaderText = "RECEIPT"
        gridviewHead.Columns("IPCS~IGRSWT~IDIAPCS~IDIAWT").HeaderText = "ISSUE"
        gridviewHead.Columns("CPCS~CGRSWT~CDIAPCS~CDIAWT").HeaderText = "CLOSING"
        gridviewHead.Columns("RATE").HeaderText = ""
        gridviewHead.Columns("PURRATE").HeaderText = ""
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub



    'Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
    '    Dim f As frmGridDispDia
    '    f = objGPack.GetParentControl(gridViewHeader)
    '    If Not f.gridViewHeader.Visible Then Exit Sub
    '    If f.gridViewHeader Is Nothing Then Exit Sub
    '    If Not f.gridView.ColumnCount > 0 Then Exit Sub
    '    If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
    '    With f.gridViewHeader
    '        .Columns("PARTICULAR~SUBITEMNAME").Width = f.gridView.Columns("PARTICULAR").Width + _
    '        IIf(f.gridView.Columns("SUBITEMNAME").Visible, f.gridView.Columns("SUBITEMNAME").Width, 0)
    '        .Columns("OPCS~OGRSWT").Width = f.gridView.Columns("OPCS").Width + f.gridView.Columns("OGRSWT").Width
    '        .Columns("RPCS~RGRSWT").Width = f.gridView.Columns("RPCS").Width + f.gridView.Columns("RGRSWT").Width
    '        .Columns("IPCS~IGRSWT").Width = f.gridView.Columns("IPCS").Width + f.gridView.Columns("IGRSWT").Width
    '        .Columns("CPCS~CGRSWT").Width = f.gridView.Columns("CPCS").Width + f.gridView.Columns("CGRSWT").Width
    '        .Columns("RATE").Width = f.gridView.Columns("RATE").Width
    '        .Columns("RATE").Visible = f.gridView.Columns("RATE").Visible
    '        .Columns("PURRATE").Width = f.gridView.Columns("PURRATE").Width
    '        .Columns("PURRATE").Visible = f.gridView.Columns("PURRATE").Visible
    '        Dim colWid As Integer = 0
    '        For cnt As Integer = 0 To f.gridView.ColumnCount - 1
    '            If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
    '        Next
    '        If colWid >= f.gridView.Width Then
    '            f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
    '            f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
    '            f.gridViewHeader.Columns("SCROLL").HeaderText = ""
    '        Else
    '            f.gridViewHeader.Columns("SCROLL").Visible = False
    '        End If
    '    End With
    'End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Width = IIf(chkGroupbyItem.Checked, 200, 100)
            .Columns("SUBITEMNAME").Width = 150
            .Columns("OPCS").Width = 50
            .Columns("OGRSWT").Width = 70
            .Columns("ODIAPCS").Width = 40
            .Columns("ODIAWT").Width = 60
            .Columns("RPCS").Width = 50
            .Columns("RGRSWT").Width = 70
            .Columns("RDIAPCS").Width = 40
            .Columns("RDIAWT").Width = 60
            .Columns("IPCS").Width = 50
            .Columns("IGRSWT").Width = 70
            .Columns("IDIAPCS").Width = 40
            .Columns("IDIAWT").Width = 60
            .Columns("CPCS").Width = 50
            .Columns("CGRSWT").Width = 70
            .Columns("CDIAPCS").Width = 40
            .Columns("CDIAWT").Width = 60

            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("PARTICULAR").Visible = True
            .Columns("SUBITEMNAME").Visible = chkWithSubitem.Checked
            .Columns("OPCS").Visible = True
            .Columns("OGRSWT").Visible = True
            .Columns("ODIAPCS").Visible = True
            .Columns("ODIAWT").Visible = True
            .Columns("RPCS").Visible = True
            .Columns("RGRSWT").Visible = True
            .Columns("RDIAPCS").Visible = True
            .Columns("RDIAWT").Visible = True
            .Columns("IPCS").Visible = True
            .Columns("IGRSWT").Visible = True
            .Columns("IDIAPCS").Visible = True
            .Columns("IDIAWT").Visible = True
            .Columns("CPCS").Visible = True
            .Columns("CGRSWT").Visible = True
            .Columns("CDIAPCS").Visible = True
            .Columns("CDIAWT").Visible = True
            .Columns("RATE").Visible = chkWithValue.Checked
            .Columns("PURRATE").Visible = chkWithValue.Checked
            .Columns("PARTICULAR").HeaderText = IIf(chkGroupbyItem.Checked, "PARTICULAR", "PACKETNO")
            .Columns("OPCS").HeaderText = "PCS"
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ODIAPCS").HeaderText = "DPCS"
            .Columns("ODIAWT").HeaderText = "DWT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RDIAPCS").HeaderText = "DPCS"
            .Columns("RDIAWT").HeaderText = "DWT"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("IDIAPCS").HeaderText = "DPCS"
            .Columns("IDIAWT").HeaderText = "DWT"
            .Columns("CPCS").HeaderText = "PCS"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CDIAPCS").HeaderText = "DPCS"
            .Columns("CDIAWT").HeaderText = "DWT"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New PacketWiseStockView_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        GetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName)
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_chkGroupByDesigner = chkGroupByDesigner.Checked
        obj.p_chkGroupbyItem = chkGroupbyItem.Checked
        obj.p_chkWithSubitem = chkWithSubitem.Checked
        obj.p_chkWithValue = chkWithValue.Checked
        SetSettingsObj(obj, Me.Name, GetType(PacketWiseStockView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New PacketWiseStockView_Properties
        GetSettingsObj(obj, Me.Name, GetType(PacketWiseStockView_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        SetChecked_CheckedList(chkCmbItemName, obj.p_chkCmbItemName, "ALL")
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        chkGroupByDesigner.Checked = obj.p_chkGroupByDesigner
        chkGroupbyItem.Checked = obj.p_chkGroupbyItem
        chkWithSubitem.Checked = obj.p_chkWithSubitem
        chkWithValue.Checked = obj.p_chkWithValue
    End Sub
End Class

Public Class PacketWiseStockView_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
        'New list(of string) {"a", "b", "c", "d"}
    End Property

    Private chkCmbItemName As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbItemName() As List(Of String)
        Get
            Return chkCmbItemName
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItemName = value
        End Set
    End Property
    Private chkCmbDesigner As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String) '= New List(Of String)(New String() {"ALL"})
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkGroupByDesigner As Boolean = False
    Public Property p_chkGroupByDesigner() As Boolean
        Get
            Return chkGroupByDesigner
        End Get
        Set(ByVal value As Boolean)
            chkGroupByDesigner = value
        End Set
    End Property
    Private chkGroupbyItem As Boolean = False
    Public Property p_chkGroupbyItem() As Boolean
        Get
            Return chkGroupbyItem
        End Get
        Set(ByVal value As Boolean)
            chkGroupbyItem = value
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
    Private chkWithValue As Boolean = False
    Public Property p_chkWithValue() As Boolean
        Get
            Return chkWithValue
        End Get
        Set(ByVal value As Boolean)
            chkWithValue = value
        End Set
    End Property
End Class