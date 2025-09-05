Imports System.Data.OleDb
Public Class frmItemWiseSales
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGrid As New DataSet
    Dim dt As DataTable
    Dim SelectedCompanyId As String
    Dim dtCostCentre As New DataTable
    Dim RPT_HIDE_GRANDTOTAL As Boolean = IIf(GetAdmindbSoftValue("RPT_HIDE_GRANDTOTAL", "Y") = "Y", True, False)
    Dim RPT_ITEMSALES_WITHVAT As Boolean = IIf(GetAdmindbSoftValue("RPT_ITEMSALES_WITHVAT", "N") = "Y", True, False)
    Dim ITEMSALES_StnWtcol As Boolean = IIf(GetAdmindbSoftValue("RPT_ITEMWISESALE_STNWT", "N") = "Y", True, False)
    Dim RPT_ITEMWISESALE_DESIGN As Boolean = IIf(GetAdmindbSoftValue("RPT_ITEMWISESALE_DESIGN", "N") = "Y", True, False)
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        ProcAddNodeId()
        funcLoadItemCtr()
        funcLoadItemName()
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
                .Visible = rbtDetailed.Checked
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

    Function funcLoadItemName()
        strSql = " SELECT 'ALL' ITEMNAME,1 RESULT UNION ALL SELECT ITEMNAME,2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        chkCmbItemName.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemName, dtCostCentre, "ITEMNAME", , "ALL")
    End Function

    Function funcLoadSubItem() As Integer
        If chkItem.Checked And chkCmbItemName.Text <> "" And chkCmbItemName.Text <> "ALL" Then
            Dim SelectedItemId As String
            SelectedItemId = ""
            SelectedItemId = GetChecked_CheckedList(chkCmbItemName, False)
            Dim item() As String
            item = SelectedItemId.Split(",")
            SelectedItemId = ""
            For jj As Integer = 0 To item.Length - 1
                SelectedItemId += objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & item(jj).ToString & "'") + ","
            Next
            SelectedItemId = Mid(SelectedItemId, 1, Len(SelectedItemId) - 1)
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE ACTIVE = 'Y' AND ITEMID IN (" & SelectedItemId.ToString & ")"
            strSql += " ORDER BY SUBITEMNAME"
        Else
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE ACTIVE = 'Y' AND ITEMID BETWEEN '" & Val(txtItemIdFrom_NUM.Text) & "' AND '" & Val(txtItemIdTo_NUM.Text) & "'"
            strSql += " ORDER BY SUBITEMNAME"
        End If
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

        strSql = " SELECT 'ALL' METALNAME,1 RESULT UNION ALL SELECT METALNAME,2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += " ORDER BY METALNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbMetal, dtCostCentre, "METALNAME", , "ALL")

        LoadCompany(chkLstCompany)
        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("COUNTER")
        cmbOrderBy.Items.Add("COUNTER,ITEM,NODE-ID")
        cmbOrderBy.Items.Add("ITEM")
        cmbOrderBy.Items.Add("ITEM,SUBITEM")
        cmbOrderBy.Items.Add("METAL,ITEM")
        cmbOrderBy.Items.Add("ITEM,COUNTER,ITEMTYPE")
        cmbOrderBy.Items.Add("DESIGNER")
        cmbOrderBy.Items.Add("DESIGNER,ITEM")
        cmbOrderBy.Items.Add("ITEMTYPE")
        cmbOrderBy.Items.Add("ITEMGROUP")
        cmbOrderBy.Items.Add("ITEMGROUP,EMPLOYEE")
        cmbOrderBy.Items.Add("COUNTERGROUP")
        cmbOrderBy.Text = "COUNTER"
        cmbOrderBy.Items.Add("COUNTER,DESIGNER,ITEM")
        cmbOrderBy.Items.Add("COUNTER,ITEM")
        cmbOrderBy.Items.Add("COUNTER,DESIGNER")
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False, False)
        cmbEmployee.Items.Clear()
        cmbEmployee.Items.Add("ALL")
        strSql = " SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER ORDER BY EMPNAME"
        objGPack.FillCombo(strSql, cmbEmployee, False, False)
        cmbEmployee.Text = "ALL"

        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        If rbtDetailed.Checked = True Then
            chkOnlymin.Visible = True
        Else
            chkOnlymin.Visible = False
        End If
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        ChkWithTag.Checked = False
        pnlHeading.Visible = False

        ''Hallmark
        cmbHallmarkFilter.Items.Clear()
        cmbHallmarkFilter.Items.Add("BOTH")
        cmbHallmarkFilter.Items.Add("WITH HALLMARK")
        cmbHallmarkFilter.Items.Add("WITHOUT HALLMARK")
        cmbHallmarkFilter.Text = "BOTH"

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewReport()
        Dim SubItemId As String = ""
        Dim grand As String = ""
        Dim SelectedCounterId As String = ""
        Dim SelectedItemId As String = ""
        Dim MetalId As String = "ALL"
        Dim NodeId As String = ""
        Dim Stonesep As String = ""
        Dim DesignerId As String = "ALL"
        Dim Employeeid As String = "ALL"
        ResizeToolStripMenuItem.Checked = False
        'If cmbMetal.Text = "ALL" Or cmbMetal.Text = "" Then
        '    MetalId = "ALL"
        'Else
        '    MetalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'")
        'End If
        If chkdiastnsep.Checked Then
            Stonesep = "Y"
        Else
            Stonesep = "N"
        End If

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
        If chkEmployee.Checked = True Then
            If chkcmbEmployee.Text <> "ALL" And chkcmbEmployee.Text <> "" Then
                Employeeid = ""
                Employeeid = GetChecked_CheckedList(chkcmbEmployee, False)
                Dim Employee() As String
                Employee = Employeeid.Split(",")
                Employeeid = ""
                For jj As Integer = 0 To Employee.Length - 1
                    Employeeid += objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & Employee(jj).ToString & "'") + ","

                Next
                Employeeid = Mid(Employeeid, 1, Len(Employeeid) - 1)
            End If
        Else
            If cmbEmployee.Text <> "ALL" And cmbEmployee.Text <> "" Then
                Employeeid = ""
                Employeeid += objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & cmbEmployee.Text & "'") + ","
                Employeeid = Mid(Employeeid, 1, Len(Employeeid) - 1)
            End If

        End If

        If chkItem.Checked Then
            If chkCmbItemName.Text <> "ALL" And chkCmbItemName.Text <> "" Then
                SelectedItemId = ""
                SelectedItemId = GetChecked_CheckedList(chkCmbItemName, False)
                Dim item() As String
                item = SelectedItemId.Split(",")
                SelectedItemId = ""
                For jj As Integer = 0 To item.Length - 1
                    SelectedItemId += objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & item(jj).ToString & "'") + ","
                Next
                SelectedItemId = Mid(SelectedItemId, 1, Len(SelectedItemId) - 1)
            Else
                SelectedItemId = "ALL"
            End If
        Else
            For cnt As Integer = Val(txtItemIdFrom_NUM.Text) To Val(txtItemIdTo_NUM.Text)
                If cnt = 0 Then Continue For
                SelectedItemId += cnt.ToString & ","
            Next
            If SelectedItemId = "" Then
                SelectedItemId = "ALL"
            Else
                SelectedItemId = Mid(SelectedItemId, 1, SelectedItemId.Length - 1)
            End If
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
        If chkLstNodeId.CheckedItems.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) = True Then
                For cnt As Integer = 1 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.Items.Item(cnt).ToString <> "" Then
                        retStr += chkLstNodeId.Items.Item(cnt).ToString
                    Else
                        retStr += "EMPTY"
                    End If
                    retStr += ","
                Next
            End If
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
        If retStr <> "" Then SelectedSystemId = Mid(retStr, 1, retStr.Length - 1)
        Dim costid As String = "ALL"
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then
            costid = GetSelectedCostId(chkcmbcostcentre, True)
        End If
        btnView_Search.Enabled = False

        If Not chkGrtotal.Checked Then
            grand = "N"
        Else
            grand = "Y"
        End If

        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_ITEMWISESALES"
        strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID = """ & costid & """"
        strSql += vbCrLf + " ,@METALID = '" & MetalId & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & SelectedItemId & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & SubItemId & "'"
        strSql += vbCrLf + " ,@COUNTERID = '" & SelectedCounterId & "'"
        If cmbOrderBy.Text = "DESIGNER,ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'DI'"
        ElseIf cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CSI'"
        ElseIf cmbOrderBy.Text = "ITEM,COUNTER,ITEMTYPE" Then
            strSql += vbCrLf + " ,@GROUPBY = 'ICI'"
        ElseIf cmbOrderBy.Text = "ITEM,SUBITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'ITS'"
        ElseIf cmbOrderBy.Text = "METAL,ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'MI'"
        ElseIf cmbOrderBy.Text = "ITEMTYPE" Then
            strSql += vbCrLf + " ,@GROUPBY = 'ITYPE'"
        ElseIf cmbOrderBy.Text = "ITEMGROUP" Then
            strSql += vbCrLf + " ,@GROUPBY = 'IGRP'"
        ElseIf cmbOrderBy.Text = "COUNTERGROUP" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CGRP'"
        ElseIf cmbOrderBy.Text = "COUNTER,DESIGNER,ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CDI'"
        ElseIf cmbOrderBy.Text = "COUNTER,ITEM" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CI'"
        ElseIf cmbOrderBy.Text = "COUNTER,DESIGNER" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CTS'"
        ElseIf cmbOrderBy.Text = "ITEMGROUP,EMPLOYEE" Then
            strSql += vbCrLf + " ,@GROUPBY = 'IGRE'"
        ElseIf cmbOrderBy.Text = "COUNTER,EMPLOYEE" Then
            strSql += vbCrLf + " ,@GROUPBY = 'CE'"
        ElseIf cmbOrderBy.Text = "COUNTER" Then
            strSql += vbCrLf + " ,@GROUPBY = 'C'"
        Else
            strSql += vbCrLf + " ,@GROUPBY = '" & Mid(cmbOrderBy.Text, 1, 1) & "'"
        End If
        strSql += vbCrLf + " ,@ORDERBYNAME = '" & IIf(rbtOrderId.Checked, "N", "Y") & "'"
        strSql += vbCrLf + " ,@RPTTYPE = '" & IIf(rbtSummary.Checked, "S", "D") & "'"
        strSql += vbCrLf + " ,@WITHSUBITEMGROUP = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@TRANTYPE = 'SA" & IIf(chkWithOrderRepair.Checked, ",OD,RD", "") & IIf(chkWithMiscIssue.Checked, ",MI", "") & "'"
        strSql += vbCrLf + " ,@DESIGNER = '" & DesignerId & "'"
        If chkOnlymin.Checked = True Then
            strSql += vbCrLf + " ,@SPECIFIC = 'Y'"
        Else
            strSql += vbCrLf + " ,@SPECIFIC = 'N'"
        End If
        'If chkDesigner.Checked = True Then
        '    strSql += vbCrLf + " ,@DESIGNER = '" & DesignerId & "'"
        'Else
        '    strSql += vbCrLf + " ,@DESIGNER = '" & cmbDesigner.Text & "'"
        'End If


        strSql += vbCrLf + " ,@ISTAG = '" & IIf(ChkWithTag.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & SelectedSystemId & "'"
        strSql += vbCrLf + " ,@DBNAME= '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@WITHTALLY= '" & IIf(ChKWithTally.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SYSID= '" & systemId & "'"
        strSql += vbCrLf + " ,@GRAND= '" & grand & "'"
        strSql += vbCrLf + " ,@BILLFROM= '" & Val(txtBillnoFrom_NUM.Text) & "'"
        strSql += vbCrLf + " ,@BILLTO= '" & Val(txtBillNoTo_NUM.Text) & "'"
        strSql += vbCrLf + " ,@WITHSR= '" & IIf(ChkLessSr.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHTAX= '" & IIf(RPT_ITEMSALES_WITHVAT, "Y", "N") & "'"
        strSql += vbCrLf + " ,@STNWTCOL= '" & IIf(ITEMSALES_StnWtcol, "Y", "N") & "'"
        strSql += vbCrLf + " ,@EMPLOYEEID= '" & Employeeid & "'"
        strSql += vbCrLf + " ,@WITHHALLMARK= '" & IIf(cmbHallmarkFilter.Text = "WITH HALLMARK", "WTH", IIf(cmbHallmarkFilter.Text = "WITHOUT HALLMARK", "WOH", "ALL")) & "'"
        strSql += vbCrLf + " ,@WITHOUTRDADJ= '" & IIf(chkwithoutrdadj.Checked, "Y", "N") & "'"
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='B'"
        ElseIf rbtTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='T'"
        ElseIf rbtNonTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='N'"
        End If
        If chkChit.Checked Then strSql += vbCrLf + " ,@CHITCOMPANYID = '" & SelectedCompanyId & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim DtGrid As New DataTable
        Dim dss As New DataSet
        da.Fill(dss)

        If dss.Tables(0).Rows.Count <= 0 Then
            MsgBox("No Record Found", MsgBoxStyle.Information)
            Exit Sub
        End If
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

            If chkdiastnsep.Checked = True Then
                .Columns("DIAWT").Visible = False
                .Columns("STN_PCS").Visible = True
                .Columns("STN_GRM").Visible = True
                .Columns("STN_CARAT").Visible = True
                .Columns("DIA_PCS").Visible = True
                .Columns("DIA_GRM").Visible = True
                .Columns("DIA_CARAT").Visible = True
            Else
                .Columns("STN_PCS").Visible = False
                .Columns("STN_GRM").Visible = False
                .Columns("STN_CARAT").Visible = False
                .Columns("DIA_PCS").Visible = False
                .Columns("DIA_GRM").Visible = False
                .Columns("DIA_CARAT").Visible = False
                .Columns("DIAWT").Visible = True
            End If

            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            If cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
                If rbtSummary.Checked = True Then
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
                ElseIf rbtDetailed.Checked = True Then
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
                    .Columns("TYPE").Visible = False
                    .Columns("GROUP1").Visible = False
                    .Columns("GROUP2").Visible = False
                    '.Columns("DESIGNER").Visible = False
                    .Columns("GROUP2").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("KEYNO").Visible = False
                    .Columns("TYPE").Visible = False
                    .Columns("TYPE").Visible = False
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
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
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
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = rbtDetailed.Checked
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = rbtDetailed.Checked
                If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Visible = rbtDetailed.Checked
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
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Visible = rbtDetailed.Checked
                If .Columns.Contains("STYLENO") Then .Columns("STYLENO").Visible = rbtDetailed.Checked
                If .Columns.Contains("RATE") Then .Columns("RATE").Visible = rbtDetailed.Checked
                If .Columns.Contains("SALETYPE") Then .Columns("SALETYPE").Visible = rbtDetailed.Checked
                If .Columns.Contains("SYSTEMID") Then .Columns("SYSTEMID").Visible = rbtDetailed.Checked
                If .Columns.Contains("ITEMTYPE") Then .Columns("ITEMTYPE").Visible = False
                If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.Format = "0.00"
            End If

        End With
        If gridView.Columns.Contains("PURVALUE") Then gridView.Columns("PURVALUE").Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView, True)
        With gridView
            If chkdiastnsep.Checked = True Then
                .Columns("DIAWT").Visible = False
                .Columns("STN_GRM").Visible = True
                .Columns("STN_CARAT").Visible = True
            Else
                .Columns("STN_GRM").Visible = False
                .Columns("STN_CARAT").Visible = False
                .Columns("DIAWT").Visible = True
            End If

            If .Columns.Contains("WASTAGE") Then .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("WASTAGE") Then .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("DIAWT") Then .Columns("DIAWT").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("DIFFWT") Then .Columns("DIFFWT").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("MCHARGE") Then .Columns("MCHARGE").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("ORDG1") Then .Columns("ORDG1").Visible = False
            If .Columns.Contains("ORDG2") Then .Columns("ORDG2").Visible = False
            If .Columns.Contains("GROUP1") Then .Columns("GROUP1").Visible = False
            If .Columns.Contains("GROUP2") Then .Columns("GROUP2").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("TAGS") Then .Columns("TAGS").Visible = ChkWithTag.Checked

            'If rbtDetailed.Checked Then
            '    If cmbOrderBy.Text = "ITEM" Then
            '        If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            '        If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            '    ElseIf cmbOrderBy.Text = "COUNTER" Then
            '        If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = IIf(cmbOrderBy.Text = "COUNTER", True, False)
            '    ElseIf cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
            '        If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            '        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = False
            '    ElseIf cmbOrderBy.Text = "ITEM,COUNTER,ITEMTYPE" Then
            '        If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            '        If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = False
            '        If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = False
            '    ElseIf cmbOrderBy.Text = "DESIGNER" Then
            '        If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            '        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
            '    ElseIf cmbOrderBy.Text = "ITEMGROUP" Then
            '        If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            '        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
            '        If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            '    End If
            'Else
            '    If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
            '    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            '    If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            'End If
            If rbtDetailed.Checked Then
                If cmbOrderBy.Text = "ITEM" Then
                    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                    If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTER" Then
                    If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = IIf(cmbOrderBy.Text = "COUNTER", True, False)
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
                    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                    If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "ITEM,COUNTER,ITEMTYPE" Then
                    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                    If .Columns.Contains("TRANNO") Then .Columns("TRANNO").Visible = False
                    If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "DESIGNER" Then
                    If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "ITEMGROUP" Then
                    If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False

                ElseIf cmbOrderBy.Text = "DESIGNER,ITEM" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "ITEMTYPE" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "ITEMGROUP,EMPLOYEE" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "ITEMTYPE" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTERGROUP" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTER,DESIGNER,ITEM" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTER,ITEM" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "COUNTER,DESIGNER" Then
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                ElseIf cmbOrderBy.Text = "METAL,ITEM" Then
                    If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                    'If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
                    If RPT_ITEMWISESALE_DESIGN = True Then
                        If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                    End If
                End If

            Else
                If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            End If
        End With

        If gridView.Columns.Contains("HM_BILLNO") Then gridView.Columns("HM_BILLNO").HeaderText = "HALLMARK"

        With gridView
            If chkOnlymin.Checked = True Then
                If .Columns.Contains("RECDATE") Then .Columns("RECDATE").Visible = False
                If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                If .Columns.Contains("TAGGRSWT") Then .Columns("TAGGRSWT").Visible = False
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Visible = False
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Visible = False
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Visible = False
                If .Columns.Contains("LESSWT") Then .Columns("LESSWT").Visible = False
                If .Columns.Contains("DIAWT") Then .Columns("DIAWT").Visible = False
                If .Columns.Contains("STNAMT") Then .Columns("STNAMT").Visible = False
                If .Columns.Contains("DIFFWT") Then .Columns("DIFFWT").Visible = False
                If .Columns.Contains("PURVALUE") Then .Columns("PURVALUE").Visible = False
                If .Columns.Contains("STYLENO") Then .Columns("STYLENO").Visible = False
                If .Columns.Contains("RATE") Then .Columns("RATE").Visible = False
                If .Columns.Contains("SYSTEMID") Then .Columns("SYSTEMID").Visible = False
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Visible = False
                If .Columns.Contains("TIME") Then .Columns("TIME").Visible = False
            End If
        End With
        GridViewFormat()

        'FillGridGroupStyle_KeyNoWise(gridView)
        'funcGridStyle()
        btnView_Search.Enabled = True
        lblHeading.Text = "ITEM WISE SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        If Val(txtItemIdFrom_NUM.Text) > 0 And Val(txtItemIdFrom_NUM.Text) = Val(txtItemIdTo_NUM.Text) Then
            Dim iName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemIdFrom_NUM.Text) & "")
            lblHeading.Text = iName & " SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        If cmbOrderBy.Text = "COUNTER,ITEM,NODE-ID" Then
            lblHeading.Text = "COUNTER,ITEM,NODE-ID SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then lblHeading.Text += vbCrLf & "" & chkcmbcostcentre.Text
        lblHeading.Font = New Font("VERDANA", 9, FontStyle.Bold)
        pnlHeading.Visible = True
        If DtGrid.Rows.Count > 0 Then tabmain.SelectedTab = tabView : gridView.Focus()
        If gridView.Columns.Contains("TRANDATE") Then gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Focus()
    End Sub


    Private Sub btnView_Search__Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        NewReport()
        Prop_Sets()
        Exit Sub
        '--ITEM
        lblHeading.Text = ""
        btnView_Search.Enabled = False
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        Dim strFilter As String = funcFiltraion()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMWISESALE')>0"
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "ITEMWISESALE "
        strSql += vbCrLf + "  CREATE TABLE TEMP" & systemId & "ITEMWISESALE ("
        strSql += vbCrLf + "  ITEMID VARCHAR(6), "
        strSql += vbCrLf + "  ITEMNAME VARCHAR(50),"
        strSql += vbCrLf + "  ITEMCTRID VARCHAR(6),"
        strSql += vbCrLf + "  COUNTER VARCHAR(50),"
        strSql += vbCrLf + "  TRANNO INT,"
        strSql += vbCrLf + "  TRANDATE SMALLDATETIME,"
        strSql += vbCrLf + "  TAGNO VARCHAR(12),"
        strSql += vbCrLf + "  PCS  VARCHAR(10),"
        strSql += vbCrLf + "  GRSWT VARCHAR(20),"
        strSql += vbCrLf + "  STNWT VARCHAR(20),"
        strSql += vbCrLf + "  NETWT  VARCHAR(20),"
        strSql += vbCrLf + "  DIFFWT  VARCHAR(20),"
        strSql += vbCrLf + "  RATE VARCHAR(20),"
        strSql += vbCrLf + "  AMOUNT  VARCHAR(20),"
        strSql += vbCrLf + "  SUBITEMNAME  VARCHAR(50),"
        strSql += vbCrLf + "  FLAG VARCHAR(20),"
        strSql += vbCrLf + "  DESIGNER  VARCHAR(30),"
        strSql += vbCrLf + "  EMPNAME  VARCHAR(30),"
        strSql += vbCrLf + "  SYSTEMID  VARCHAR(3),"
        strSql += vbCrLf + "  COLHEAD  VARCHAR(1),"
        strSql += vbCrLf + "  SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PROD')>0"
        'strsql += vbcrlf + "  DROP TABLE TEMP" & systemId & "PROD"
        'strsql += vbcrlf + "  SELECT"
        'strsql += vbcrlf + "  I.ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)"
        'strsql += vbcrlf + "  + '   ['+CONVERT(VARCHAR,ITEMID)+']' AS ITEMNAME"
        'strsql += vbcrlf + "  ,ITEMCTRID,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER "
        'strsql += vbcrlf + "  WHERE ITEMCTRID = I.ITEMCTRID)+'   ['+CONVERT(VARCHAR,ITEMCTRID)+']' AS COUNTER"
        'strsql += vbcrlf + "  ,CONVERT(VARCHAR,TRANNO)TRANNO,TAGNO"
        'strsql += vbcrlf + "  ,PCS,GRSWT,ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE "
        'strsql += vbcrlf + "  WHERE ISSSNO = I.SNO AND COMPANYID IN (" & SelectedCompanyId & ")),0)AS STNWT"
        'strsql += vbcrlf + "  ,NETWT,CASE WHEN FLAG = '' THEN TAGGRSWT-GRSWT ELSE 0 END AS DIFFWT"
        'strsql += vbcrlf + "  ,RATE"
        'strsql += vbcrlf + "  ,AMOUNT"
        'strsql += vbcrlf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        'strsql += vbcrlf + "  WHERE SUBITEMID = I.SUBITEMID)AS SUBITEMNAME"
        'strsql += vbcrlf + "  ,CASE WHEN FLAG = 'C' THEN 'COUNTER' WHEN FLAG = 'B' THEN 'BACK OFFICE' ELSE '' END AS FLAG"
        'strsql += vbcrlf + "  ,ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER "
        'strsql += vbcrlf + "  WHERE DESIGNERID = I.TAGDESIGNER),'')AS DESIGNER"
        'strsql += vbcrlf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        'strsql += vbcrlf + "  WHERE EMPID = I.EMPID)AS EMPNAME,SYSTEMID"
        'strsql += vbcrlf + "  ,'1'RESULT, ' 'COLHEAD"
        'strsql += vbcrlf + "  INTO TEMP" & systemId & "PROD"
        'strsql += vbcrlf + "  FROM " & cnStockDb & "..ISSUE AS I"
        'strsql += vbcrlf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS P ON I.ITEMID = P.ITEMID"
        'strSql += strFilter
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PROD')>0"
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "PROD"
        strSql += vbCrLf + "  SELECT"
        strSql += vbCrLf + "  I.ITEMID,P.ITEMNAME"
        strSql += vbCrLf + "  + '   ['+CONVERT(VARCHAR,I.ITEMID)+']' AS ITEMNAME"
        strSql += vbCrLf + "  ,ITEMCTRID,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += vbCrLf + "  WHERE ITEMCTRID = I.ITEMCTRID)+'   ['+CONVERT(VARCHAR,ITEMCTRID)+']' AS COUNTER"
        strSql += vbCrLf + "  ,TRANNO,TAGNO"
        strSql += vbCrLf + "  ,PCS,I.GRSWT,ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE "
        strSql += vbCrLf + "  WHERE ISSSNO = I.SNO AND COMPANYID IN (" & SelectedCompanyId & ")),0)AS STNWT"
        strSql += vbCrLf + "  ,NETWT,CASE WHEN FLAG = '' THEN TAGGRSWT-I.GRSWT ELSE 0 END AS DIFFWT"
        'strsql += vbcrlf + "  ,CASE WHEN P.CALTYPE IN ('R','F') THEN RATE ELSE NULL END AS RATE"
        strSql += vbCrLf + "  ,RATE"
        strSql += vbCrLf + "  ,AMOUNT"
        strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += vbCrLf + "  WHERE SUBITEMID = I.SUBITEMID AND ITEMID = I.ITEMID)AS SUBITEMNAME"
        strSql += vbCrLf + "  ,CASE WHEN FLAG = 'C' THEN 'COUNTER' WHEN FLAG = 'B' THEN 'BACK OFFICE' ELSE '' END AS FLAG"
        strSql += vbCrLf + "  ,ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER "
        strSql += vbCrLf + "  WHERE DESIGNERID = I.TAGDESIGNER),'')AS DESIGNER"
        strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += vbCrLf + "  WHERE EMPID = I.EMPID)AS EMPNAME,SYSTEMID"
        strSql += vbCrLf + "  ,'1'RESULT, ' 'COLHEAD,P.CALTYPE,I.TRANDATE"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "PROD"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS P ON I.ITEMID = P.ITEMID"
        strSql += strFilter
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        '--SUBTOTAL ITEM
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SUBTOTPROD')> 0"
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "SUBTOTPROD"
        strSql += vbCrLf + "  SELECT"
        If cmbOrderBy.Text = "ITEM" Then
            strSql += vbCrLf + "  ITEMID," & IIf(rbtDetailed.Checked, "'SUB TOTAL'", "ITEMNAME") & " ITEMNAME"
            strSql += vbCrLf + "  ,' 'ITEMCTRID,' 'COUNTER"
        Else 'If cmbOrderBy.Text = "COUNTER" Then
            strSql += vbCrLf + "  ' 'ITEMID,' 'ITEMNAME"
            strSql += vbCrLf + "  ,ITEMCTRID," & IIf(rbtDetailed.Checked, "'SUB TOTAL'", "COUNTER") & " COUNTER"
        End If
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)TRANNO,' 'TAGNO"
        strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(STNWT)STNWT"
        strSql += vbCrLf + "  ,SUM(NETWT)NETWT,SUM(DIFFWT)DIFFWT,NULL RATE,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + "  ,' 'SUBITEMNAME"
        strSql += vbCrLf + "  ,' 'FLAG"
        strSql += vbCrLf + "  ,' 'DESIGNER"
        strSql += vbCrLf + "  ,' 'EMPNAME,' 'SYSTEMID"
        strSql += vbCrLf + "  ,'2'RESULT, 'S' COLHEAD"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "SUBTOTPROD"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  TEMP" & systemId & "PROD"

        If cmbOrderBy.Text = "ITEM" Then
            strSql += vbCrLf + "  GROUP BY ITEMID,ITEMNAME"
        Else 'cmbOrderBy.Text = "COUNTER" Then
            strSql += vbCrLf + "  GROUP BY ITEMCTRID,COUNTER"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        '--GRANDTOTAL ITEM
        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PROD)>0"
        strSql += vbCrLf + "  BEGIN "
        strSql += vbCrLf + "  IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GRANDTOTPROD')> 0"
        strSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "GRANDTOTPROD"
        strSql += vbCrLf + "  SELECT "
        If cmbOrderBy.Text = "ITEM" Then
            strSql += vbCrLf + "  999999 ITEMID, ' 'ITEMCTRID, 'GRAND TOTAL'ITEMNAME, ' 'COUNTER"
        Else ' If cmbOrderBy.Text = "COUNTER" Then
            strSql += vbCrLf + "  ' 'ITEMID, 999999 ITEMCTRID, ' 'ITEMNAME,'GRAND TOTAL'COUNTER"
        End If
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)TRANNO,' 'TAGNO"
        strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(STNWT)STNWT"
        strSql += vbCrLf + "  ,SUM(NETWT)NETWT,SUM(DIFFWT)DIFFWT,NULL RATE,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + "  ,' 'SUBITEMNAME"
        strSql += vbCrLf + "  ,' 'FLAG"
        strSql += vbCrLf + "  ,' 'DESIGNER"
        strSql += vbCrLf + "  ,' 'EMPNAME,' 'SYSTEMID"
        strSql += vbCrLf + "  ,'3'RESULT, 'G' COLHEAD"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "GRANDTOTPROD"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  TEMP" & systemId & "PROD"
        strSql += vbCrLf + "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''SUBTOTAL
        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "SUBTOTPROD)>0"
        strSql += vbCrLf + "  BEGIN "
        strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "PROD( "
        strSql += vbCrLf + "  ITEMID, ITEMNAME, ITEMCTRID, COUNTER, TRANNO, TAGNO, PCS, GRSWT, STNWT, NETWT, "
        strSql += vbCrLf + "  DIFFWT, Rate, AMOUNT, SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId, RESULT, COLHEAD)"
        strSql += vbCrLf + "  SELECT ITEMID,ITEMNAME,ITEMCTRID,COUNTER,TRANNO,TAGNO,PCS,GRSWT,STNWT,NETWT,"
        strSql += vbCrLf + "  DIFFWT, Rate, AMOUNT, SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId, RESULT, COLHEAD"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "SUBTOTPROD "
        strSql += vbCrLf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''GRANDTOTAL
        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PROD)>0"
        strSql += vbCrLf + "  BEGIN "
        strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "GRANDTOTPROD)>0"
        strSql += vbCrLf + "  BEGIN "
        strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "PROD( "
        strSql += vbCrLf + "  ITEMID, ITEMNAME, ITEMCTRID, COUNTER, TRANNO, TAGNO, PCS, GRSWT, STNWT, NETWT, "
        strSql += vbCrLf + "  DIFFWT, Rate, AMOUNT, SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId, RESULT, COLHEAD)"
        strSql += vbCrLf + "  SELECT ITEMID,ITEMNAME,ITEMCTRID,COUNTER,TRANNO,TAGNO,PCS,GRSWT,STNWT,NETWT,"
        strSql += vbCrLf + "  DIFFWT, Rate, AMOUNT, SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId, RESULT, COLHEAD"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "GRANDTOTPROD "
        strSql += vbCrLf + "  END "
        strSql += vbCrLf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtDetailed.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PROD)>0"
            strSql += vbCrLf + "  BEGIN "
            If cmbOrderBy.Text = "ITEM" Then
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "PROD(ITEMID,ITEMNAME,RESULT,COLHEAD,STNWT,FLAG,DESIGNER)"
                strSql += vbCrLf + "  SELECT DISTINCT ITEMID,ITEMNAME, 0 RESULT, 'T' COLHEAD,0,' ',' ' FROM "
                strSql += vbCrLf + "  TEMP" & systemId & "PROD WHERE ITEMNAME <> 'SUB TOTAL' AND ITEMNAME <> 'GRAND TOTAL'"
            Else 'If cmbOrderBy.Text = "COUNTER" Then
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "PROD(ITEMCTRID,COUNTER,RESULT,COLHEAD,"
                strSql += vbCrLf + "  STNWT,FLAG,DESIGNER) "
                strSql += vbCrLf + "  SELECT DISTINCT ITEMCTRID,COUNTER, 0 RESULT, 'T' COLHEAD,0,' ',' ' FROM "
                strSql += vbCrLf + "  TEMP" & systemId & "PROD WHERE COUNTER <> 'SUB TOTAL' AND COUNTER <> 'GRAND TOTAL'"
            End If
            strSql += vbCrLf + "  END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If rbtSummary.Checked Then
            strSql = " DELETE FROM TEMP" & systemId & "PROD"
            strSql += vbCrLf + "  WHERE RESULT = 1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " UPDATE TEMP" & systemId & "PROD SET COLHEAD = '' WHERE ISNULL(COLHEAD,'') = 'S'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "PROD)>0"
        strSql += vbCrLf + "  BEGIN "
        strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMWISESALE( "
        strSql += vbCrLf + "  ITEMID, ITEMNAME, ITEMCTRID, COUNTER, TRANNO, TRANDATE,TAGNO, PCS,GRSWT,STNWT,NETWT,"
        strSql += vbCrLf + "  DIFFWT, Rate, AMOUNT, SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId,  COLHEAD)"
        strSql += vbCrLf + "  SELECT ITEMID,ITEMNAME,ITEMCTRID,COUNTER,TRANNO,TRANDATE,TAGNO, "
        strSql += vbCrLf + "  (CASE WHEN PCS =0 THEN NULL ELSE PCS END) PCS,"
        strSql += vbCrLf + "  (CASE WHEN GRSWT =0 THEN NULL ELSE GRSWT END) GRSWT,"
        strSql += vbCrLf + "  (CASE WHEN STNWT =0 THEN NULL ELSE STNWT END) STNWT,"
        strSql += vbCrLf + "  (CASE WHEN NETWT =0 THEN NULL ELSE NETWT END) NETWT,"
        strSql += vbCrLf + "  (CASE WHEN DIFFWT =0 THEN NULL ELSE DIFFWT END) DIFFWT,"
        strSql += vbCrLf + "  (CASE WHEN RATE =0 THEN NULL ELSE RATE END) RATE,"
        strSql += vbCrLf + "  (CASE WHEN AMOUNT =0 THEN NULL ELSE AMOUNT END) AMOUNT,"
        strSql += vbCrLf + "  SUBITEMNAME, FLAG, DESIGNER, EMPNAME, systemId,  COLHEAD"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "PROD "
        If rbtDetailed.Checked Then
            If cmbOrderBy.Text = "ITEM" Then
                strSql += vbCrLf + "  ORDER BY itemid,RESULT,TRANNO"
            ElseIf cmbOrderBy.Text = "COUNTER" Then
                strSql += vbCrLf + "  ORDER BY itemctrid,RESULT,TRANNO"
            End If
        Else
            strSql += vbCrLf + "  ORDER BY RESULT"
        End If
        strSql += vbCrLf + "  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtDetailed.Checked Then
            If cmbOrderBy.Text = "ITEM" Then
                strSql = " SELECT CASE WHEN ISNULL(COLHEAD,'') IN('T','S','G')THEN ISNULL(ITEMNAME,'') ELSE CONVERT(vARCHAR,TRANNO) END PARTICULAR,TRANDATE,"
            Else 'If cmbOrderBy.Text = "COUNTER" Then
                strSql = " SELECT CASE WHEN ISNULL(COLHEAD,'') IN('T','S','G')THEN ISNULL(COUNTER,'') ELSE CONVERT(vARCHAR,TRANNO) END PARTICULAR,TRANDATE,"
            End If
        Else
            If cmbOrderBy.Text = "ITEM" Then
                strSql = " SELECT ISNULL(ITEMNAME,'') PARTICULAR,NULL TRANDATE,"
            Else 'If cmbOrderBy.Text = "COUNTER" Then
                strSql = " SELECT ISNULL(COUNTER,'') PARTICULAR,NULL TRANDATE,"
            End If
        End If
        strSql += vbCrLf + "  DESIGNER,SUBITEMNAME,TAGNO,PCS, GRSWT,STNWT, NETWT, DIFFWT,RATE, AMOUNT,"
        strSql += vbCrLf + "  FLAG,"
        strSql += vbCrLf + "  EMPNAME, SYSTEMID,COLHEAD"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMWISESALE "
        strSql += " ORDER BY SNO"
        'If cmbOrderBy.Text = "ITEM" Then
        '    strSql += vbCrLf + "  ORDER BY ITEMNAME,TRANNO"
        'Else
        '    strSql += vbCrLf + "  ORDER BY COUNTER,TRANNO"
        'End If


        Dim dt As New DataTable
        dt.Columns.Add("KEYNO", GetType(Integer))
        dt.Columns("KEYNO").AutoIncrement = True
        dt.Columns("KEYNO").AutoIncrementSeed = 0
        dt.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            btnView_Search.Enabled = True
            MsgBox("RECORD NOT FOUND", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        gridView.DataSource = dt
        tabView.Show()
        'GridViewFormat()
        FillGridGroupStyle_KeyNoWise(gridView)
        funcGridStyle()
        gridView.Columns("COLHEAD").Visible = False
        btnView_Search.Enabled = True
        lblHeading.Text = "ITEM WISE SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        lblHeading.Text += vbCrLf & "" & chkcmbcostcentre.Text
        If Val(txtItemIdFrom_NUM.Text) > 0 And Val(txtItemIdFrom_NUM.Text) = Val(txtItemIdTo_NUM.Text) Then
            Dim iName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemIdFrom_NUM.Text) & "")
            lblHeading.Text = iName & " SALES REPORT-DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        lblHeading.Font = New Font("VERDANA", 9, FontStyle.Bold)
        pnlHeading.Visible = True
        If dt.Rows.Count > 0 Then tabmain.SelectedTab = tabView : gridView.Focus()
        gridView.Focus()
        Prop_Sets()
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
                        .DefaultCellStyle.BackColor = Color.LavenderBlush
                    Case "S1"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                        .DefaultCellStyle.BackColor = Color.LavenderBlush
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        dgvRow.Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
                        .DefaultCellStyle.BackColor = Color.Lavender
                    Case "C"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = Color.Yellow
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
        funcLoadItemName()
        funcLoadSubItem()
        objGPack.TextClear(Me)
        pnlHeading.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True

        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))


        Prop_Gets()
        dtpFrom.Select()
        txtBillnoFrom_NUM.Text = ""
        txtBillNoTo_NUM.Text = ""
        If RPT_HIDE_GRANDTOTAL Then chkGrtotal.Checked = False : chkGrtotal.Enabled = False Else chkGrtotal.Enabled = True
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHeading.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
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
        If cmbOrderBy.Text.ToUpper = "COUNTER" Or rbtSummary.Checked Then
            chkWithSubItem.Checked = False
            chkWithSubItem.Enabled = False
        Else
            chkWithSubItem.Enabled = True
        End If
    End Sub

    Private Sub rbtSummary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSummary.CheckedChanged
        If cmbOrderBy.Text.ToUpper = "COUNTER" Or rbtSummary.Checked Then
            chkWithSubItem.Checked = False
            chkWithSubItem.Enabled = False
            chkOnlymin.Visible = False
            chkOnlymin.Checked = False
        Else
            chkWithSubItem.Enabled = True
            chkOnlymin.Visible = False
            chkOnlymin.Checked = False
        End If
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
        Dim obj As New frmItemWiseSales_properties
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
        obj.p_cmbEmployee = cmbEmployee.Text
        obj.p_cmbOrderBy = cmbOrderBy.Text
        obj.p_rbtOrderId = rbtOrderId.Checked
        obj.p_rbtName = rbtName.Checked
        obj.p_chkWithOrderRepair = chkWithOrderRepair.Checked
        obj.p_chkWithMiscIssue = chkWithMiscIssue.Checked
        obj.p_chkdiastnsep = chkdiastnsep.Checked
        obj.p_chkWithSubItem = chkWithSubItem.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_chkWithTag = ChkWithTag.Checked
        obj.p_chkWithTally = ChKWithTally.Checked
        obj.p_chkGrtotal = chkGrtotal.Checked
        obj.p_chkSR = ChkLessSr.Checked
        obj.p_cmbHallmarkfilter = cmbHallmarkFilter.Text
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseSales_properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
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
        cmbEmployee.Text = obj.p_cmbEmployee
        cmbOrderBy.Text = obj.p_cmbOrderBy
        rbtOrderId.Checked = obj.p_rbtOrderId
        rbtName.Checked = obj.p_rbtName
        chkWithOrderRepair.Checked = obj.p_chkWithOrderRepair
        chkWithMiscIssue.Checked = obj.p_chkWithMiscIssue
        chkdiastnsep.Checked = obj.p_chkdiastnsep
        chkWithSubItem.Checked = obj.p_chkWithSubItem
        chkGrtotal.Checked = obj.p_chkGrtotal
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        ChkWithTag.Checked = obj.p_chkWithTag
        ChKWithTally.Checked = obj.p_chkWithTally
        ChkLessSr.Checked = obj.p_chkSR
        cmbHallmarkFilter.Text = obj.p_cmbHallmarkfilter
        rbtBoth.Checked = obj.p_rbtBoth
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
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


    Private Sub chkItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkItem.CheckedChanged
        If chkItem.Checked Then
            chkItem.Text = "ItemName"
            chkCmbItemName.Visible = True
            lblItemidTo.Visible = False
            txtItemIdFrom_NUM.Visible = False
            txtItemIdTo_NUM.Visible = False
            txtItemIdFrom_NUM.Text = ""
            txtItemIdTo_NUM.Text = ""
        Else
            chkItem.Text = "Item Id From"
            chkCmbItemName.Visible = False
            lblItemidTo.Visible = True
            txtItemIdFrom_NUM.Visible = True
            txtItemIdTo_NUM.Visible = True
            txtItemIdFrom_NUM.Text = ""
            txtItemIdTo_NUM.Text = ""
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
    Private Sub LoadEmployee()

        strSql = " SELECT 'ALL' EMPNAME,1 RESULT UNION ALL SELECT EMPNAME,2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        strSql += " ORDER BY RESULT, EMPNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbEmployee, dtCostCentre, "EMPNAME", , "ALL")
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        chkOnlymin.Visible = True
    End Sub

    Private Sub chkEmployee_CheckedChanged(sender As Object, e As EventArgs) Handles chkEmployee.CheckedChanged
        If chkEmployee.Checked = True Then
            LoadEmployee()
            chkcmbEmployee.Visible = True
            cmbEmployee.Visible = False
        Else
            chkcmbEmployee.Visible = False
            cmbEmployee.Visible = True
        End If
    End Sub

    Private Sub chkCmbItemName_Leave(sender As Object, e As EventArgs) Handles chkCmbItemName.Leave
        funcLoadSubItem()
    End Sub

    Private Sub chkCmbItemName_SelectedValueChanged(sender As Object, e As EventArgs) Handles chkCmbItemName.SelectedValueChanged
        funcLoadSubItem()
    End Sub
End Class

Public Class frmItemWiseSales_properties
    Private chkSR As Boolean = False
    Public Property p_chkSR() As Boolean
        Get
            Return chkSR
        End Get
        Set(ByVal value As Boolean)
            chkSR = value
        End Set
    End Property

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
    Private cmbEmployee As String = "ALL"
    Public Property p_cmbEmployee() As String
        Get
            Return cmbEmployee
        End Get
        Set(ByVal value As String)
            cmbEmployee = value
        End Set
    End Property
    Private cmbOrderBy As String = "COUNTER"
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
    Private cmbHallmarkfilter As String = "BOTH"
    Public Property p_cmbHallmarkfilter() As String
        Get
            Return cmbHallmarkfilter
        End Get
        Set(ByVal value As String)
            cmbHallmarkfilter = value
        End Set
    End Property
    Private chkwithoutrdadj As Boolean = False
    Public Property p_chkwithoutrdadj() As Boolean
        Get
            Return chkwithoutrdadj
        End Get
        Set(ByVal value As Boolean)
            chkwithoutrdadj = value
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
End Class
