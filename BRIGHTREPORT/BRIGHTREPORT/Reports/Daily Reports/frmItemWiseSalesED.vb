Imports System.Data.OleDb
Public Class frmItemWiseSalesED
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGrid As New DataSet
    Dim dt As DataTable
    Dim SelectedCompanyId As String
    Dim dtCostCentre As New DataTable


    Public Sub LoadCompany(ByRef chkLstBox As CheckedListBox)
        chkLstBox.Items.Clear()
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,COMPANYNAME"
        Dim dtCompany As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        For Each ro As DataRow In dtCompany.Rows
            chkLstBox.Items.Add(ro("COMPANYNAME").ToString)
            If ro("COMPANYNAME").ToString = strCompanyName Then chkLstBox.SetItemChecked(chkLstBox.Items.Count - 1, True)
        Next
    End Sub

    Public Sub CheckedListCompany_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim chklst As CheckedListBox = CType(sender, CheckedListBox)
        If chklst.Items.Count > 0 Then
            If Not chklst.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chklst.Items.Count - 1
                    If chklst.Items.Item(cnt).ToString = strCompanyName Then
                        chklst.SetItemChecked(cnt, True)
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        ProcAddNodeId()
        funcLoadItemCtr()
        funcLoadSubItem()
    End Sub

    Function funcGridStyle() As Integer
        With gridView
            With .Columns("PARTICULAR")
                .Width = 160
                .HeaderText = "PARTICULAR"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAGNO")
                .HeaderText = "TAGNO"
                .Width = 75
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .HeaderText = "TRANDATE"
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .Visible = rbtTrading.Checked
            End With
            With .Columns("SUBITEMNAME")
                .HeaderText = "DESCRIPTION"
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PCS")
                .HeaderText = "PCS"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .HeaderText = "GRSWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("STNWT")
                .HeaderText = "DIAWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .HeaderText = "NETWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DIFFWT")
                .HeaderText = "DIFFWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RATE")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("FLAG")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DESIGNER")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("EMPNAME")
                .Width = 100
                .HeaderText = "EMP_NAME"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SYSTEMID")
                .HeaderText = "NODEID"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .Columns("keyno").Visible = False
        End With
    End Function
    Function funcLoadItemCtr()
        strSql = " SELECT 'ALL' ITEMCTRNAME,1 RESULT UNION ALL SELECT ITEMCTRNAME,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY ITEMCTRNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        chkCmbCounter.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCostCentre, "ITEMCTRNAME", , "ALL")
    End Function
    Function funcLoadSubItem() As Integer
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE ACTIVE = 'Y' AND ITEMID BETWEEN '" & Val(txtItemIdFrom_NUM.Text) & "' AND '" & Val(txtItemIdTo_NUM.Text) & "'"
        strSql += " ORDER BY SUBITEMNAME"
        chkLstSubItem.Items.Clear()
        chkLstSubItem.Items.Add("ALL", True)
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For cnt As Integer = 0 To dt.Rows.Count - 1
            chkLstSubItem.Items.Add(dt.Rows(cnt).Item("SUBITEMNAME"))
        Next
    End Function

    Function funcFiltraion() As String
        Dim Qry As String = Nothing : Dim tempchkitem As String = Nothing : Dim tmpcnt As Integer = 0
        Qry += " WHERE i.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"

        tempchkitem = "" : tmpcnt = 0
        If chkLstNodeId.Items.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                If chkLstNodeId.GetItemChecked(CNT) = True Then
                    tempchkitem = tempchkitem & " '" & chkLstNodeId.Items.Item(CNT) + "'"
                    tmpcnt += 1
                    If tmpcnt < chkLstNodeId.CheckedItems.Count Then tempchkitem += ","
                End If
            Next
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then Qry += " AND i.SYSTEMID IN (" & tempchkitem & ")"
        If Not (cmbMetal.Text = "" Or cmbMetal.Text = "ALL") Then
            Qry += " AND i.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "')))"
        End If
        If txtItemIdFrom_NUM.Text <> "" And txtItemIdTo_NUM.Text <> "" Then
            Qry += " AND i.ITEMID BETWEEN '" & Val(txtItemIdFrom_NUM.Text) & "' AND '" & Val(txtItemIdTo_NUM.Text) & "'"
        ElseIf txtItemIdFrom_NUM.Text <> "" And txtItemIdTo_NUM.Text = "" Then
            Qry += " AND i.ITEMID = '" & Val(txtItemIdFrom_NUM.Text) & "'"
        ElseIf txtItemIdFrom_NUM.Text = "" And txtItemIdTo_NUM.Text <> "" Then
            Qry += " AND i.ITEMID = '" & Val(txtItemIdTo_NUM.Text) & "'"
        End If
        If txtCtrIdFrom_NUM.Text <> "" And txtCtrTo_NUM.Text <> "" Then
            Qry += " AND i.ITEMCTRID BETWEEN '" & Val(txtCtrIdFrom_NUM.Text) & "' AND '" & Val(txtCtrTo_NUM.Text) & "'"
        ElseIf txtCtrIdFrom_NUM.Text <> "" And txtCtrTo_NUM.Text = "" Then
            Qry += " AND i.ITEMCTRID = '" & Val(txtCtrIdFrom_NUM.Text) & "'"
        ElseIf txtCtrIdFrom_NUM.Text = "" And txtCtrTo_NUM.Text <> "" Then
            Qry += " AND i.ITEMCTRID = '" & Val(txtCtrTo_NUM.Text) & "'"
        End If
        If chkLstSubItem.CheckedItems.Count > 0 And chkLstSubItem.GetItemChecked(0) <> True Then
            Dim subQry As String = "" '"("
            For CNT As Integer = 0 To chkLstSubItem.CheckedItems.Count - 1
                Dim dtC As New DataTable
                dtC.Clear()
                subQry += chkLstSubItem.CheckedItems.Item(CNT).ToString + ","
            Next
            subQry = subQry.Remove(subQry.Length - 1, 1)
            Qry += " and i.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMNAME WHERE SUBITEMNAME IN (" & subQry & ")"
        End If
        'If chkCmbItemType.Text <> "ALL" Or chkCmbItemType.Text <> "" Then
        '    Qry += " AND ITEMTYPE IN('" & GetSelecteditemtypeid(chkCmbItemType, True) & "'"
        'End If
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then
            Qry += " AND i.COSTID IN (" & GetSelectedCostId(chkcmbcostcentre, True) & ") "
        End If
        Qry += " AND i.COMPANYID IN (" & SelectedCompanyId & ") "
        Qry += " AND ISNULL(i.CANCEL,'') = '' "
        Qry += " AND I.TRANTYPE <> 'AI'"
        Return Qry
    End Function

    Private Sub frmItemWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabmain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) And tabmain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" And tabmain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmProductWise_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabmain.ItemSize = New Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        grp1.Location = New Point((ScreenWid - grp1.Width) / 2, ((ScreenHit - 128) - grp1.Height) / 2)
        Me.tabmain.SelectedTab = tabGen
        Try

            strSql = " SELECT 'ALL' METALNAME,1 RESULT UNION ALL SELECT METALNAME,2 RESULT FROM " & cnAdminDb & "..METALMAST"
            strSql += " ORDER BY METALNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbMetal, dtCostCentre, "METALNAME", , "ALL")

            LoadCompany(chkLstCompany)
            cmbOrderBy.Items.Clear()
            cmbOrderBy.Items.Add("ITEM")
            cmbOrderBy.Items.Add("ITEM,SUBITEM")
            cmbOrderBy.Items.Add("DESIGNER,ITEM")
            cmbDesigner.Items.Clear()
            cmbDesigner.Items.Add("ALL")
            strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
            objGPack.FillCombo(strSql, cmbDesigner, True, False)

            strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
            strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)

            BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            pnlHeading.Visible = False
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub NewReport()
        Dim SubItemId As String = ""
        Dim grand As String = ""
        Dim SelectedCounterId As String = ""
        Dim SelectedItemId As String = ""
        Dim MetalId As String = "ALL"
        Dim NodeId As String = ""
        Dim Stonesep As String = ""
        Dim StkType As String = ""
        Dim WithPurDetail As String = ""
        Dim DesignerId As String = "ALL"
        ResizeToolStripMenuItem.Checked = False
        If rbtManufacturing.Checked = True Then
            StkType = "M"
        ElseIf rbtTrading.Checked = True Then
            StkType = "T"
        ElseIf rbtStktypeAll.Checked = True Then
            StkType = "A"
        End If
        If chkPurdetail.Checked = True Then
            WithPurDetail = "Y"
        Else
            WithPurDetail = "N"
        End If
        Stonesep = "Y"
        If chkcmbMetal.Text <> "ALL" And chkcmbMetal.Text <> "" Then
            MetalId = GetChecked_CheckedList(chkcmbMetal, False)
            Dim METAL() As String
            METAL = MetalId.Split(",")
            MetalId = ""
            For jj As Integer = 0 To METAL.Length - 1
                MetalId += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & METAL(jj).ToString & "'") + ","
            Next
            MetalId = Mid(MetalId, 1, Len(MetalId) - 1)
        End If

        If chkDesigner.Checked = True Then
            If chkcmbDesginer.Text <> "ALL" And chkcmbDesginer.Text <> "" Then
                DesignerId = ""
                DesignerId = GetChecked_CheckedList(chkcmbDesginer, False)
                Dim DESIGNER() As String
                DESIGNER = DesignerId.Split(",")
                DesignerId = ""
                For jj As Integer = 0 To DESIGNER.Length - 1
                    DesignerId += objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & DESIGNER(jj).ToString & "'") + ","

                Next
                DesignerId = Mid(DesignerId, 1, Len(DesignerId) - 1)
            End If
        Else
            If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
                DesignerId = ""
                DesignerId += objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'") + ","
                DesignerId = Mid(DesignerId, 1, Len(DesignerId) - 1)
            End If

        End If


        For cnt As Integer = Val(txtItemIdFrom_NUM.Text) To Val(txtItemIdTo_NUM.Text)
            If cnt = 0 Then Continue For
            SelectedItemId += cnt.ToString & ","
        Next
        If SelectedItemId = "" Then
            SelectedItemId = "ALL"
        Else
            SelectedItemId = Mid(SelectedItemId, 1, SelectedItemId.Length - 1)
        End If

        If chkCounter.Checked = False Then
            For cnt As Integer = Val(txtCtrIdFrom_NUM.Text) To Val(txtCtrTo_NUM.Text)
                SelectedCounterId += cnt.ToString & ","
            Next
        End If

        If chkCounter.Checked Then
            If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
                SelectedCounterId = ""
                SelectedCounterId = GetChecked_CheckedList(chkCmbCounter, False)
                Dim Counter() As String
                Counter = SelectedCounterId.Split(",")
                SelectedCounterId = ""
                For jj As Integer = 0 To Counter.Length - 1
                    SelectedCounterId += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & Counter(jj).ToString & "'") + ","
                Next
                SelectedCounterId = Mid(SelectedCounterId, 1, Len(SelectedCounterId) - 1)
            Else
                SelectedCounterId = "ALL"
            End If
        End If

        If chkCounter.Checked = False Then
            If SelectedItemId = "" Then
                SelectedCounterId = "ALL"
            Else
                SelectedCounterId = Mid(SelectedCounterId, 1, SelectedCounterId.Length - 1)
            End If
        End If

        If chkLstSubItem.CheckedItems.Count > 0 And chkLstSubItem.GetItemChecked(0) = True Then
            SubItemId = "ALL"
        Else
            Dim tsTr As String = ""
            For CNT As Integer = 0 To chkLstSubItem.CheckedItems.Count - 1
                If chkLstSubItem.CheckedItems.Item(CNT).ToString = "ALL" Then
                    SubItemId = "ALL"
                    Exit For
                End If
                tsTr += objGPack.GetSqlValue("SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & chkLstSubItem.CheckedItems.Item(CNT).ToString & "'") + ","
            Next
            If tsTr <> "" Then
                SubItemId = Mid(tsTr, 1, tsTr.Length - 1)
            End If
        End If
        Dim SelectedSystemId As String = Nothing

        Dim retStr As String = ""
        If chkLstNodeId.CheckedItems.Count > 0 And chkLstNodeId.GetItemChecked(0) = True Then
            For cnt As Integer = 1 To chkLstNodeId.Items.Count - 1
                If chkLstNodeId.Items.Item(cnt).ToString <> "" Then
                    retStr += chkLstNodeId.Items.Item(cnt).ToString
                Else
                    retStr += "EMPTY"
                End If
                retStr += ","
            Next
        Else
            For cnt As Integer = 0 To chkLstNodeId.CheckedItems.Count - 1
                If chkLstNodeId.CheckedItems.Item(cnt).ToString <> "" Then
                    retStr += chkLstNodeId.CheckedItems.Item(cnt).ToString
                Else
                    retStr += "EMPTY"
                End If
                retStr += ","
            Next
        End If
        SelectedSystemId = Mid(retStr, 1, retStr.Length - 1)
        Dim costid As String = "ALL"
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then
            costid = GetSelectedCostId(chkcmbcostcentre, True)
        End If
        btnView_Search.Enabled = False

        grand = "Y"
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_ITEMWISESALES_EXCISE "
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@RECDATE = '" & dtpRec_OWN.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID = """ & costid & """"
        strSql += vbCrLf + " ,@METALID = '" & MetalId & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & SelectedItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & SubItemId & "'"
        strSql += vbCrLf + " ,@COUNTERID = '" & SelectedCounterId & "'"
        If cmbOrderBy.Text = "DESIGNER,ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'DI'"
        ElseIf cmbOrderBy.Text = "ITEM,SUBITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'ITS'"
        ElseIf cmbOrderBy.Text = "ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'I'"
        End If
        strSql += vbCrLf + " ,@ORDERBYNAME = '" & IIf(rbtOrderId.Checked, "N", "Y") & "'"
        strSql += vbCrLf + " ,@RPTTYPE = 'D'"
        strSql += vbCrLf + " ,@TRANTYPE = 'SA" & IIf(chkWithOrderRepair.Checked, ",OD,RD", "") & IIf(chkWithMiscIssue.Checked, ",MI", "") & "'"
        strSql += vbCrLf + " ,@EXCISETYPE = '" & StkType & "'"
        strSql += vbCrLf + " ,@WITHPURDET = '" & WithPurDetail & "'"
        strSql += vbCrLf + " ,@DESIGNER = '" & DesignerId & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & SelectedSystemId & "'"
        strSql += vbCrLf + " ,@DBNAME= '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@SYSID= '" & systemId & "'"
        strSql += vbCrLf + " ,@GRAND= '" & grand & "'"
        strSql += vbCrLf + " ,@BILLFROM= '" & Val(txtBillnoFrom_NUM.Text) & "'"
        strSql += vbCrLf + " ,@BILLTO= '" & Val(txtBillNoTo_NUM.Text) & "'"
        'strSql += vbCrLf + " ,@STNDIASEP= '" & Stonesep & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim DtGrid As New DataTable
        Dim dss As New DataSet
        da.Fill(dss)
        DtGrid = dss.Tables(0)
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Sub
        End If
        'If chkGrtotal.Checked Then
        '    strSql = " DELETE from TEMPTABLEDB..ITEMSALESRES where colhead='G'"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()
        'End If
        'If chkGrtotal.Checked Then
        '    DtGrid.RowFilter = "PARTICULAR <>'GRAND TOTAL'"
        'End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        gridView.DataSource = Nothing
        gridView.DataSource = DtGrid
        tabView.Show()
        With gridView
            .Columns("DIAWT").Visible = False
            .Columns("STN_GRM").Visible = True
            .Columns("STN_CARAT").Visible = True
            .Columns("DIA_GRM").Visible = True
            .Columns("DIA_CARAT").Visible = True

            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            If cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
                If rbtManufacturing.Checked = True Then
                    .Columns("PARTICULAR").Width = 150
                    .Columns("PCS").Width = 60
                    .Columns("GRSWT").Width = 100
                    .Columns("LESSWT").Width = 100
                    .Columns("NETWT").Width = 100
                    .Columns("WASTAGE").Width = 100
                    .Columns("MCHARGE").Width = 100
                    .Columns("DIAWT").Width = 100
                    .Columns("DIFFWT").Width = 100
                    .Columns("AMOUNT").Width = 100
                ElseIf rbtTrading.Checked = True Then
                    .Columns("TRANDATE").Visible = False
                    .Columns("TYPE").Visible = False
                    .Columns("SYSTEMID").Visible = False
                    .Columns("SALETYPE").Visible = False
                    .Columns("STYLENO").Visible = False
                    '.Columns("DESIGNER").Visible = False
                    .Columns("GROUP1").Visible = False
                    .Columns("GROUP2").Visible = False
                End If
            Else
                If cmbOrderBy.Text.ToUpper = "COUNTER" Or cmbOrderBy.Text.ToUpper = "ITEM" Or cmbOrderBy.Text.ToUpper = "ITEM,SUBITEM" Or cmbOrderBy.Text.ToUpper = "ITEMTYPE" Then
                    '.Columns("TYPE").Visible = False
                    .Columns("GROUP1").Visible = False
                    .Columns("GROUP2").Visible = False
                    '.Columns("DESIGNER").Visible = False
                    .Columns("GROUP2").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("KEYNO").Visible = False
                    '.Columns("TYPE").Visible = False
                    '.Columns("TYPE").Visible = False
                    .Columns("COUNTER").Visible = False
                    .Columns("ORDG1").Visible = False
                    .Columns("ORDG2").Visible = False
                End If
                For cnt As Integer = 0 To .ColumnCount - 1
                    '.Columns(cnt).Visible = False
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PARTICULAR").Width = 150
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Width = 70
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Width = 70
                If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Width = 70
                If .Columns.Contains("PCS") Then .Columns("PCS").Width = 60
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Width = 80
                If .Columns.Contains("LESSWT") Then .Columns("LESSWT").Width = 80
                If .Columns.Contains("NETWT") Then .Columns("NETWT").Width = 80
                If .Columns.Contains("DIAWT") Then .Columns("DIAWT").Width = 80
                If .Columns.Contains("DIFFWT") Then .Columns("DIFFWT").Width = 80
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").Width = 90
                If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").Width = 90
                If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").HeaderText = "PV"
                If .Columns.Contains("RATE") Then .Columns("RATE").Width = 70
                If .Columns.Contains("SALETYPE") Then .Columns("SALETYPE").Width = 100
                If .Columns.Contains("SYSTEMID") Then .Columns("SYSTEMID").Width = 60
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Width = 50
                If .Columns.Contains("EMPNAME") Then .Columns("EMPNAME").Width = 100
                If .Columns.Contains("PARTICULAR") Then .Columns("PARTICULAR").Visible = True
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = True
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = True
                If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Visible = True
                If .Columns.Contains("PCS") Then .Columns("PCS").Visible = True
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Visible = True
                If .Columns.Contains("LESSWT") Then .Columns("LESSWT").Visible = True
                If .Columns.Contains("NETWT") Then .Columns("NETWT").Visible = True
                If .Columns.Contains("DIAWT") Then .Columns("DIAWT").Visible = True
                If .Columns.Contains("DIFFWT") Then .Columns("DIFFWT").Visible = True
                If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").Visible = True
                If .Columns.Contains("MCHARGE") Then .Columns("MCHARGE").Visible = True
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").Visible = True
                If .Columns.Contains("EMPNAME") Then .Columns("EMPNAME").Visible = True
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Visible = rbtTrading.Checked
                If .Columns.Contains("STYLENO") Then .Columns("STYLENO").Visible = rbtTrading.Checked
                If .Columns.Contains("RATE") Then .Columns("RATE").Visible = rbtTrading.Checked
                If .Columns.Contains("SALETYPE") Then .Columns("SALETYPE").Visible = rbtTrading.Checked
                If .Columns.Contains("SYSTEMID") Then .Columns("SYSTEMID").Visible = rbtTrading.Checked
                If .Columns.Contains("ITEMTYPE") Then .Columns("ITEMTYPE").Visible = False
                If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.Format = "0.00"
            End If

        End With
        If gridView.Columns.Contains("PURVALUE") Then gridView.Columns("PURVALUE").Visible = True
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView, True)


        With gridView

            .Columns("DIAWT").Visible = False
            .Columns("STN_GRM").Visible = True
            .Columns("STN_CARAT").Visible = True

            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            '.Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            .Columns("DIAWT").DefaultCellStyle.Format = "0.000"
            '.Columns("DIFFWT").DefaultCellStyle.Format = "0.000"
            '.Columns("MCHARGE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("ORDG1") Then .Columns("ORDG1").Visible = False
            If .Columns.Contains("ORDG2") Then .Columns("ORDG2").Visible = False
            If .Columns.Contains("GROUP1") Then .Columns("GROUP1").Visible = False
            If .Columns.Contains("GROUP2") Then .Columns("GROUP2").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False

            If cmbOrderBy.Text = "ITEM" Then
                If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
                If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
            End If
        End With
        GridViewFormat()

        'FillGridGroupStyle_KeyNoWise(gridView)
        'funcGridStyle()
        btnView_Search.Enabled = True
        lblHeading.Text = "ITEM WISE EXCISE SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        If Val(txtItemIdFrom_NUM.Text) > 0 And Val(txtItemIdFrom_NUM.Text) = Val(txtItemIdTo_NUM.Text) Then
            Dim iName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemIdFrom_NUM.Text) & "")
            lblHeading.Text = iName & " SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        If cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
            lblHeading.Text = "COUNTER,ITEM,NODE-ID SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        If rbtTrading.Checked = True Then
            lblHeading.Text += vbCrLf & "" & "TRADING"
        ElseIf rbtManufacturing.Checked = True Then
            lblHeading.Text += vbCrLf & "" & "MANUFACTURING"
        ElseIf rbtStktypeAll.Checked = True Then
            lblHeading.Text += vbCrLf & "" & "ALL"
        End If
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then lblHeading.Text += vbCrLf & "" & chkcmbcostcentre.Text
        lblHeading.Font = New Font("VERDANA", 9, FontStyle.Bold)
        pnlHeading.Visible = True
        If DtGrid.Rows.Count > 0 Then tabmain.SelectedTab = tabView : gridView.Focus()
        gridView.Focus()
    End Sub


    Private Sub btnView_Search__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click

        NewReport()
        Prop_Sets()
        Exit Sub

    End Sub

    Private Sub txtItemIdFrom_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemIdFrom_NUM.Leave
        If txtItemIdTo_NUM.Text = "" Then
            txtItemIdTo_NUM.Text = txtItemIdFrom_NUM.Text
        End If
        funcLoadSubItem()
    End Sub

    Private Sub txtItemIdTo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemIdTo_NUM.Leave
        funcLoadSubItem()
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T1"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                        .Cells("PARTICULAR").Style.ForeColor = reportHeadStyle1.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                    Case "T2"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle2.BackColor
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "S1"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        If gridView.Columns.Contains("WASTPER") Then gridView.Columns("WASTPER").DefaultCellStyle.Format = "0.00"
    End Function
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtpFrom.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcLoadItemCtr()

        pnlHeading.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpRec_OWN.Value = "2016-02-28"
        gridView.DataSource = Nothing


        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        chkcmbcostcentre.Enabled = False
        'BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))


        Prop_Gets()
        dtpFrom.Select()
        txtBillnoFrom_NUM.Text = ""
        txtBillNoTo_NUM.Text = ""
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub ProcAddNodeId()
        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN  "
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE  "
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL", True)
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabmain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub txtItemIdFrom_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemIdFrom_NUM.KeyDown, txtItemIdTo_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT"
            strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
            strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
            strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
            strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
            strSql += " FROM " & cnAdminDb & "..ITEMMAST"
            strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
            CType(sender, TextBox).Text = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1)
        End If
    End Sub

    Public Function GetSelecteditemtypeid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 And chkLst.CheckedItems.Item(0).ToString.ToString <> "ALL" Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Private Sub cmbOrderBy_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrderBy.LostFocus

    End Sub

    Private Sub rbtSummary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtManufacturing.CheckedChanged

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
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseSalesED_properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        GetChecked_CheckedList(chkcmbMetal, obj.p_chkLstMetal)
        'obj.p_cmbMetal = cmbMetal.Text
        obj.p_txtItemIdFrom_NUM = txtItemIdFrom_NUM.Text
        obj.p_txtItemIdTo_NUM = txtItemIdTo_NUM.Text
        GetChecked_CheckedList(chkLstSubItem, obj.p_chkLstSubItem)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_txtCtrIdFrom_NUM = txtCtrIdFrom_NUM.Text
        obj.p_txtCtrTo_NUM = txtCtrTo_NUM.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_cmbOrderBy = cmbOrderBy.Text
        obj.p_rbtOrderId = rbtOrderId.Checked
        obj.p_rbtName = rbtName.Checked
        obj.p_chkWithOrderRepair = chkWithOrderRepair.Checked
        obj.p_chkWithMiscIssue = chkWithMiscIssue.Checked
        obj.p_rbtDetailed = rbtTrading.Checked
        obj.p_rbtSummary = rbtManufacturing.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseSalesED_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseSalesED_properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseSalesED_properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        SetChecked_CheckedList(chkcmbMetal, obj.p_chkLstMetal, "ALL")
        'cmbMetal.Text = obj.p_cmbMetal
        txtItemIdFrom_NUM.Text = obj.p_txtItemIdFrom_NUM
        txtItemIdTo_NUM.Text = obj.p_txtItemIdTo_NUM
        SetChecked_CheckedList(chkLstSubItem, obj.p_chkLstSubItem, "ALL")
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
        txtCtrIdFrom_NUM.Text = obj.p_txtCtrIdFrom_NUM
        txtCtrTo_NUM.Text = obj.p_txtCtrTo_NUM
        cmbDesigner.Text = obj.p_cmbDesigner
        cmbOrderBy.Text = obj.p_cmbOrderBy
        rbtOrderId.Checked = obj.p_rbtOrderId
        rbtName.Checked = obj.p_rbtName
        chkWithOrderRepair.Checked = obj.p_chkWithOrderRepair
        chkWithMiscIssue.Checked = obj.p_chkWithMiscIssue
        rbtTrading.Checked = obj.p_rbtDetailed
        rbtManufacturing.Checked = obj.p_rbtSummary
    End Sub

    Private Sub chkCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounter.CheckedChanged
        If chkCounter.Checked Then
            chkCmbCounter.Visible = True
            lblCtrTo.Visible = False
            txtCtrIdFrom_NUM.Visible = False
            txtCtrTo_NUM.Visible = False
        Else
            chkCmbCounter.Visible = False
            lblCtrTo.Visible = True
            txtCtrIdFrom_NUM.Visible = True
            txtCtrTo_NUM.Visible = True
        End If
    End Sub

    Private Sub chkDesigner_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesigner.CheckedChanged
        If chkDesigner.Checked = True Then
            LoadDesigner()
            chkcmbDesginer.Visible = True
            cmbDesigner.Visible = False
        Else
            chkcmbDesginer.Visible = False
            cmbDesigner.Visible = True
        End If
    End Sub

    Private Sub LoadDesigner()

        strSql = " SELECT 'ALL' DESIGNERNAME,1 RESULT UNION ALL SELECT DESIGNERNAME,2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT, DESIGNERNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbDesginer, dtCostCentre, "DESIGNERNAME", , "ALL")
    End Sub
End Class

Public Class frmItemWiseSalesED_properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
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
    Private txtItemIdFrom_NUM As String = ""
    Public Property p_txtItemIdFrom_NUM() As String
        Get
            Return txtItemIdFrom_NUM
        End Get
        Set(ByVal value As String)
            txtItemIdFrom_NUM = value
        End Set
    End Property
    Private chkGrtotal As Boolean = True
    Public Property p_chkGrtotal() As Boolean
        Get
            Return chkGrtotal
        End Get
        Set(ByVal value As Boolean)
            chkGrtotal = value
        End Set
    End Property
    Private txtItemIdTo_NUM As String = ""
    Public Property p_txtItemIdTo_NUM() As String
        Get
            Return txtItemIdTo_NUM
        End Get
        Set(ByVal value As String)
            txtItemIdTo_NUM = value
        End Set
    End Property
    Private chkLstSubItem As New List(Of String)
    Public Property p_chkLstSubItem() As List(Of String)
        Get
            Return chkLstSubItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstSubItem = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private txtCtrIdFrom_NUM As String = ""
    Public Property p_txtCtrIdFrom_NUM() As String
        Get
            Return txtCtrIdFrom_NUM
        End Get
        Set(ByVal value As String)
            txtCtrIdFrom_NUM = value
        End Set
    End Property
    Private txtCtrTo_NUM As String = ""
    Public Property p_txtCtrTo_NUM() As String
        Get
            Return txtCtrTo_NUM
        End Get
        Set(ByVal value As String)
            txtCtrTo_NUM = value
        End Set
    End Property
    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
        End Set
    End Property
    Private cmbOrderBy As String = "ITEM"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
    Private rbtOrderId As Boolean = True
    Public Property p_rbtOrderId() As Boolean
        Get
            Return rbtOrderId
        End Get
        Set(ByVal value As Boolean)
            rbtOrderId = value
        End Set
    End Property
    Private rbtName As Boolean = False
    Public Property p_rbtName() As Boolean
        Get
            Return rbtName
        End Get
        Set(ByVal value As Boolean)
            rbtName = value
        End Set
    End Property
    Private chkWithOrderRepair As Boolean = False
    Public Property p_chkWithOrderRepair() As Boolean
        Get
            Return chkWithOrderRepair
        End Get
        Set(ByVal value As Boolean)
            chkWithOrderRepair = value
        End Set
    End Property

    Private chkdiastnsep As Boolean = False
    Public Property p_chkdiastnsep() As Boolean
        Get
            Return chkdiastnsep
        End Get
        Set(ByVal value As Boolean)
            chkdiastnsep = value
        End Set
    End Property
    Private chkWithMiscIssue As Boolean = False
    Public Property p_chkWithMiscIssue() As Boolean
        Get
            Return chkWithMiscIssue
        End Get
        Set(ByVal value As Boolean)
            chkWithMiscIssue = value
        End Set
    End Property
    Private chkWithSubItem As Boolean = False
    Public Property p_chkWithSubItem() As Boolean
        Get
            Return chkWithSubItem
        End Get
        Set(ByVal value As Boolean)
            chkWithSubItem = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private chkWithTag As Boolean = False
    Public Property p_chkWithTag() As Boolean
        Get
            Return chkWithTag
        End Get
        Set(ByVal value As Boolean)
            chkWithTag = value
        End Set
    End Property
    Private chkWithTally As Boolean = False
    Public Property p_chkWithTally() As Boolean
        Get
            Return chkWithTally
        End Get
        Set(ByVal value As Boolean)
            chkWithTally = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class