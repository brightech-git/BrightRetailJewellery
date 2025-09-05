Imports System.Data.OleDb
Imports System.Xml
Public Class frmItemWiseStock_NEW
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


    Function funcLoadCategory() As Integer
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " select CatName from " & cnAdminDb & "..Category "
        If chkCmbMetal.Text <> "ALL" Then
            strSql += " where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname = '" & chkCmbMetal.Text & "')"
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
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function

    'Function funcLoadMetal() As Integer
    '    cmbMetal.Items.Clear()
    '    cmbMetal.Items.Add("ALL")
    '    strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
    '    objGPack.FillCombo(strSql, cmbMetal, False, False)
    '    cmbMetal.Text = "ALL"
    'End Function

    Function funcGridViewStyle() As Integer
        With gridView
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("itemid").Visible = False
            .Columns("subitemid").Visible = False
            With .Columns("itemName")
                .HeaderText = "ITEM"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("subItemName")
                .Visible = False
                .HeaderText = "SUBITEM"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("designerName")
                .HeaderText = "DesignerName"
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CostName")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Counter")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("oPcs")
                If chkOnlyTag.Checked Then
                    .Visible = False
                Else
                    .Visible = True
                End If
                .HeaderText = "PCS"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("oTagPcs")
                If chkOnlyTag.Checked Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "TAG"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("oGrsWt")
                If chkGrsWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "GRSWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("oNetWt")
                If chkNetWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "NETWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("rPcs")
                If chkOnlyTag.Checked Then
                    .Visible = False
                Else
                    .Visible = True
                End If
                .HeaderText = "PCS"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("rTagPcs")
                If chkOnlyTag.Checked Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "TAG"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("rGrsWt")
                If chkGrsWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "GRSWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("rNetWt")
                If chkNetWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "NETWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("iPcs")
                If chkOnlyTag.Checked Then
                    .Visible = False
                Else
                    .Visible = True
                End If
                .HeaderText = "PCS"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("iTagPcs")
                If chkOnlyTag.Checked Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "TAG"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("iGrsWt")
                If chkGrsWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "GRSWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("iNetWt")
                If chkNetWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "NETWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("cPcs")
                If chkOnlyTag.Checked Then
                    .Visible = False
                Else
                    .Visible = True
                End If
                .HeaderText = "PCS"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("cTagPcs")
                If chkOnlyTag.Checked Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "TAG"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("cGrsWt")
                If chkGrsWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "GRSWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("cNetWt")
                If chkNetWt.Checked = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "NETWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Result")
                .HeaderText = "Result"
                .Width = 20
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Stone")
                .HeaderText = "Stone"
                .Width = 20
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Title")
                .HeaderText = "Title"
                .Width = 20
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TempVal")
                .HeaderText = "TempVal"
                .Width = 20
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Function funcEmptyItemFiltration() As String
        Dim str As String = Nothing
        str = " "
        If chkAll.Checked = False Then
            str = " Having not (sum(isnull(t.Pcs,0)) = 0 and sum(isnull(t.GrsWt,0)) = 0 and sum(isnull(t.NetWt,0))=0)"
        End If
        Return str
    End Function

    Function funcEmptyItemFiltration_NONTAG() As String
        Dim str As String = Nothing
        str = " "
        If chkAll.Checked = False Then
            ''str = " Having not (sum(isnull(t.Pcs,0)) = 0 and sum(isnull(t.GrsWt,0)) = 0 and sum(isnull(t.NetWt,0))=0)"
            str = " Having not (sum(isnull((CASE WHEN T.RECISS = 'R' THEN t.Pcs ELSE -T.PCS END),0)) = 0 and sum(isnull((CASE WHEN T.RECISS = 'R' THEN t.GrsWt ELSE -t.GrsWt END),0)) = 0 and sum(isnull((CASE WHEN T.RECISS = 'R' THEN t.NetWt ELSE -t.NetWt END),0))=0) "
        End If
        Return str
    End Function


    Function funcItemFiltration() As String
        Dim str As String = Nothing
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If cmbCategory.Text <> "ALL" Then
            str += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..Category where CatName = '" & cmbCategory.Text & "'))"
        End If
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            str += " AND t.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        str += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y' AND ISNULL(STOCKREPORT,'') = 'Y')"
        Return str
    End Function

    Function funcTagFiltration() As String
        Dim str As String = ""
        If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
            str += " and t.DesignerId IN (select DesignerId from " & cnAdminDb & "..Designer where DesignerName IN (" & GetQryString(chkCmbDesigner.Text) & "))"
        End If
        If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
            str += "and t.itemctrid in (select ItemCtrId from " & cnAdminDb & "..itemCounter where ItemCtrName in (" & GetQryString(chkCmbCounter.Text) & "))"
        End If
        If chkCmbItemType.Text <> "ALL" And chkCmbItemType.Text <> "" Then
            str += " and t.itemTypeId in "
            str += "(select itemTypeId from " & cnAdminDb & "..itemType where Name in (" & GetQryString(chkCmbItemType.Text) & "))"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            str += " and t.COSTID in"
            str += "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If rbtOrder.Checked = True Then
            str += " and t.OrdRepNo <> ''"
        ElseIf rbtRegular.Checked = True Then
            str += " and t.OrdRepNo = ''"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        Return str
    End Function

    Function funcApproval() As String
        Dim str As String = ""
        If chkOnlyApproval.Checked = False And chkWithApproval.Checked = False And chkSeperateColumnApproval.Checked = False Then
            str += " AND ISNULL(APPROVAL,'') <> 'A' "
        End If
        'If chkOnlyApproval.Checked Then
        '    str += " AND ISNULL(APPROVAL,'') = 'Z' "
        'ElseIf chkOnlyApproval.Checked = False And chkWithApproval.Checked = False And chkSeperateColumnApproval.Checked = False Then
        '    str += " AND ISNULL(APPROVAL,'') <> 'A' "
        'Else
        '    'str += " AND ISNULL(APPROVAL,'') <> 'A' "
        'End If
        'If chkSeperateColumnApproval.Checked Then
        'ElseIf chkWithApproval.Checked Then
        'ElseIf chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
        '    str += " AND ISNULL(APPROVAL,'') <> 'A' "
        'ElseIf chkOnlyApproval.Checked Then
        '    str += " AND ISNULL(APPROVAL,'') = 'A' "
        'End If
        Return str
    End Function





    Private Sub GridStyle()
        ''
        ' FillGridGroupStyle_KeyNoWise(gridView)

        'For Each dgvRow As DataGridViewRow In gridView.Rows
        '    If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
        '        dgvRow.Cells("PARTICULAR").Style.BackColor = Color.LightBlue
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    End If
        'Next
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("PARTICULAR").Width = 200
            .Columns("PARTICULAR").Visible = True
            .Columns("COUNTER").Visible = False
            .Columns("DESIGNER").Visible = False
            .Columns("COSTCENTRE").Visible = False
            .Columns("TITEM").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("STYLENO").Visible = chkStyleNo.Checked
            .Columns("STYLENO").Width = 80
            .Columns("STONE").Visible = False

            .Columns("OPCS").Width = 70
            .Columns("OGRSWT").Width = 100
            .Columns("ONETWT").Width = 100
            .Columns("ODIAPCS").Width = 70
            .Columns("ODIAWT").Width = 80
            .Columns("OSTNPCS").Width = 70
            .Columns("OSTNWT").Width = 80
            .Columns("OVALUE").Width = 100
            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            .Columns("RDIAPCS").Width = 70
            .Columns("RDIAWT").Width = 80
            .Columns("RSTNPCS").Width = 70
            .Columns("RSTNWT").Width = 80
            .Columns("RVALUE").Width = 100
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            .Columns("IDIAPCS").Width = 70
            .Columns("IDIAWT").Width = 80
            .Columns("ISTNPCS").Width = 70
            .Columns("ISTNWT").Width = 80
            .Columns("IVALUE").Width = 100
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS").Width = 70
                .Columns("ARGRSWT").Width = 100
                .Columns("ARNETWT").Width = 100
                .Columns("ARDIAPCS").Width = 70
                .Columns("ARDIAWT").Width = 80
                .Columns("ARSTNPCS").Width = 70
                .Columns("ARSTNWT").Width = 80
                .Columns("ARVALUE").Width = 100
                .Columns("ARDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

                .Columns("AIPCS").Width = 70
                .Columns("AIGRSWT").Width = 100
                .Columns("AINETWT").Width = 100
                .Columns("AIDIAPCS").Width = 70
                .Columns("AIDIAWT").Width = 80
                .Columns("AISTNPCS").Width = 70
                .Columns("AISTNWT").Width = 80
                .Columns("AIVALUE").Width = 100
                .Columns("AIDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            .Columns("CDIAPCS").Width = 70
            .Columns("CDIAWT").Width = 80
            .Columns("CSTNPCS").Width = 70
            .Columns("CSTNWT").Width = 80
            .Columns("CVALUE").Width = 100
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "PCS" Else .Columns("OPCS").HeaderText = "TAG"
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ONETWT").HeaderText = "NETWT"
            .Columns("ODIAPCS").HeaderText = "DIAPCS"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSTNPCS").HeaderText = "STNPCS"
            .Columns("OSTNWT").HeaderText = "STNWT"
            .Columns("OVALUE").HeaderText = "VALUE"

            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "PCS" Else .Columns("RPCS").HeaderText = "TAG"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNWT").HeaderText = "STNWT"
            .Columns("RVALUE").HeaderText = "VALUE"

            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "PCS" Else .Columns("IPCS").HeaderText = "TAG"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNWT").HeaderText = "STNWT"
            .Columns("IVALUE").HeaderText = "VALUE"

            If chkSeperateColumnApproval.Checked Then
                If Not chkOnlyTag.Checked Then .Columns("ARPCS").HeaderText = "PCS" Else .Columns("ARPCS").HeaderText = "TAG"
                .Columns("ARGRSWT").HeaderText = "GRSWT"
                .Columns("ARNETWT").HeaderText = "NETWT"
                .Columns("ARDIAPCS").HeaderText = "DIAPCS"
                .Columns("ARDIAWT").HeaderText = "DIAWT"
                .Columns("ARSTNPCS").HeaderText = "STNPCS"
                .Columns("ARSTNWT").HeaderText = "STNWT"
                .Columns("ARVALUE").HeaderText = "VALUE"

                If Not chkOnlyTag.Checked Then .Columns("AIPCS").HeaderText = "PCS" Else .Columns("AIPCS").HeaderText = "TAG"
                .Columns("AIGRSWT").HeaderText = "GRSWT"
                .Columns("AINETWT").HeaderText = "NETWT"
                .Columns("AIDIAPCS").HeaderText = "DIAPCS"
                .Columns("AIDIAWT").HeaderText = "DIAWT"
                .Columns("AISTNPCS").HeaderText = "STNPCS"
                .Columns("AISTNWT").HeaderText = "STNWT"
                .Columns("AIVALUE").HeaderText = "VALUE"
            End If

            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "PCS" Else .Columns("CPCS").HeaderText = "TAG"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNWT").HeaderText = "STNWT"
            .Columns("CVALUE").HeaderText = "VALUE"

            ''BACKCOLOR
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            ''VISIBLE
            .Columns("OGRSWT").Visible = chkGrsWt.Checked
            .Columns("RGRSWT").Visible = chkGrsWt.Checked
            .Columns("IGRSWT").Visible = chkGrsWt.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARGRSWT").Visible = chkGrsWt.Checked
                .Columns("AIGRSWT").Visible = chkGrsWt.Checked
            End If
            .Columns("CGRSWT").Visible = chkGrsWt.Checked

            .Columns("ODIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("RDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("IDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
                .Columns("AIDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            End If
            .Columns("CDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked

            .Columns("ODIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("RDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("IDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
                .Columns("AIDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            End If
            .Columns("CDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked

            .Columns("OSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("OSTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARSTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("ONETWT").Visible = chkNetWt.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARNETWT").Visible = chkNetWt.Checked
                .Columns("AINETWT").Visible = chkNetWt.Checked
            End If
            .Columns("CNETWT").Visible = chkNetWt.Checked
            .Columns("OVALUE").Visible = chkWithValue.Checked
            .Columns("RVALUE").Visible = chkWithValue.Checked
            .Columns("IVALUE").Visible = chkWithValue.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARVALUE").Visible = chkWithValue.Checked
                .Columns("AIVALUE").Visible = chkWithValue.Checked
            End If
            .Columns("CVALUE").Visible = chkWithValue.Checked

            .Columns("RATE").Visible = chkWithRate.Checked
        End With
    End Sub

    'Private Sub Report()
    '    Dim RecDate As String = Nothing
    '    gridViewHead.DataSource = Nothing
    '    gridView.DataSource = Nothing
    '    If chkAsOnDate.Checked Then
    '        dtpTo.Value = dtpFrom.Value
    '    End If
    '    RecDate = "RECDATE"
    '    strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSTEMID & "ITEMSTK')>0 DROP TABLE TEMP" & SYSTEMID & "ITEMSTK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    cmd.CommandTimeout = 100

    '    cmd.ExecuteNonQuery()
    '    If rbtTag.Checked Or rbtBoth.Checked Then
    '        strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '        strSql += " DECLARE @TODATE SMALLDATETIME"
    '        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
    '        strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
    '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
    '        strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
    '        strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        strSql += vbCrLf + " UNION ALL"
    '        strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
    '        strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        strSql += vbCrLf + " UNION ALL"
    '        strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
    '        strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        If chkWithCumulative.Checked Then
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
    '            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
    '            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
    '            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
    '            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '        End If
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '        End If
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If
    '    If rbtNonTag.Checked Or rbtBoth.Checked Then
    '        strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '        strSql += " DECLARE @TODATE SMALLDATETIME"
    '        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
    '        strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO"
    '        strSql += " ,RECISS"
    '        strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
    '        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE < @ASONDATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        strSql += vbCrLf + "  UNION ALL"
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
    '        Else
    '            strSql += vbCrLf + "  SELECT 'ISS'SEP"
    '        End If
    '        strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        If chkWithApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
    '        ElseIf chkOnlyApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
    '        End If

    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        strSql += vbCrLf + "  UNION ALL"
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
    '        Else
    '            strSql += vbCrLf + "  SELECT 'REC'SEP"
    '        End If
    '        strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        If chkWithApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
    '        ElseIf chkOnlyApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
    '        End If
    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
    '            Dim diaStone As String = ""
    '            If chkDiamond.Checked Then diaStone += "'D',"
    '            If chkStone.Checked Then diaStone += "'S',"
    '            diaStone = Mid(diaStone, 1, diaStone.Length - 1)
    '            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
    '            strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            strSql += vbCrLf + "  ,STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAWT,NULL STNWT"
    '            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
    '            strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '        End If
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If

    '    If chkOnlyTag.Checked Then
    '        strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
    '        strSql += vbCrLf + " ,ITEMID"
    '        strSql += vbCrLf + " ,SUBITEMID"
    '        strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAWT,NULL STNWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
    '        strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
    '        strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
    '        strSql += vbCrLf + " ,ITEMCTRID"
    '        strSql += vbCrLf + " ,DESIGNERID"
    '        strSql += vbCrLf + " ,COSTID"
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    Else
    '        If rbtTag.Checked Or rbtBoth.Checked Then
    '            strSql = " SELECT *"
    '            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
    '            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '            cmd.CommandTimeout = 100
    '            cmd.ExecuteNonQuery()
    '        Else 'nontag
    '            strSql = " SELECT *"
    '            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
    '            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '            cmd.CommandTimeout = 100
    '            cmd.ExecuteNonQuery()
    '        End If
    '    End If

    '    strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
    '    strSql += " SELECT "
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     (SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
    '    Else
    '        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
    '    End If
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.TITEMID)) + CONVERT(VARCHAR,X.TITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
    '    Else
    '        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
    '    End If
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.SUBITEMID)) + CONVERT(VARCHAR,X.SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
    '    Else
    '        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
    '    End If
    '    strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
    '    strSql += " ("
    '    If rbtTag.Checked Or rbtBoth.Checked Then
    '        strSql += vbCrLf + " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,0 STONE,DIAWT,STNWT,STYLENO,RATE FROM TEMP" & systemId & "ITEMSTOCK"
    '        If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
    '            Dim diaStone As String = ""
    '            If chkDiamond.Checked Then diaStone += "'D',"
    '            If chkStone.Checked Then diaStone += "'S',"
    '            diaStone = Mid(diaStone, 1, diaStone.Length - 1)
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
    '            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
    '            strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '            'strSql += vbCrLf + " UNION ALL"
    '            'strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            'strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
    '            'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T "
    '            'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
    '            'strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '        End If
    '        If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
    '    End If
    '    If rbtNonTag.Checked Or rbtBoth.Checked Then
    '        strSql += vbCrLf + "  SELECT "
    '        strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT,VALUE,STONE"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNWT ELSE -1*STNWT END) AS STNWT,STYLENO,RATE"
    '        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
    '        strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
    '    End If
    '    strSql += " )X"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()


    '    strSql = ""
    '    If Not chkWithRate.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET RATE = NULL"
    '    If Not chkWithValue.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET VALUE = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET COSTID = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET DESIGNERID = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEMCTRID = NULL"
    '    If Not chkStyleNo.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET STYLENO = NULL"
    '    'If Not ChkLstGroupBy.CheckedItems.Contains("ITEM") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEM = NULL"
    '    If Not chkWithSubItem.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET SUBITEM = NULL"
    '    If strSql <> "" Then
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If


    '    strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '    strSql += " DECLARE @TODATE SMALLDATETIME"
    '    strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '    strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '    strSql += vbCrLf + " SELECT ITEM,SUBITEM,TITEM"
    '    strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
    '    strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
    '    strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
    '    strSql += vbCrLf + " ,STYLENO"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNWT ELSE 0 END) AS OSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS RSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS ISTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNWT ELSE 0 END) AS ARSTNWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNWT ELSE 0 END) AS AISTNWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
    '    End If
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNWT ELSE DIAWT END) AS CSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
    '    strSql += vbCrLf + " ,RATE"
    '    strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE "
    '    If chkWithSubItem.Checked Then
    '        strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
    '    Else
    '        strSql += vbCrLf + " ,ITEM AS PARTICULAR"
    '    End If
    '    strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
    '    strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
    '    strSql += vbCrLf + " GROUP BY  TITEM,ITEM,SUBITEM,STONE,RATE"
    '    strSql += vbCrLf + " ,ITEMCTRID"
    '    strSql += vbCrLf + " ,DESIGNERID"
    '    strSql += vbCrLf + " ,COSTID"
    '    strSql += vbCrLf + " ,SUBITEM,STYLENO"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()

    '    'Dim GroupColumn As String = Nothing
    '    'If cmbGroupBy.Text <> "ITEM WISE" Or chkWithSubItem.Checked Then
    '    '    Select Case cmbGroupBy.Text
    '    '        Case "COUNTER WISE"
    '    '            GroupColumn = "COUNTER"
    '    '        Case "DESIGNER WISE"
    '    '            GroupColumn = "DESIGNER"
    '    '        Case "COSTCENTRE WISE"
    '    '            GroupColumn = "COSTNAME"
    '    '        Case "ITEM WISE"
    '    '            GroupColumn = "TEMPITEM"
    '    '    End Select
    '    '    strSql = " /*INSERTING GROUP TITLE*/"
    '    '    strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbcrlf + " SELECT DISTINCT " & GroupColumn & "," & GroupColumn & ",0 RESULT,'T'COLHEAD,3 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1"

    '    '    strSql += vbcrlf + " /*INSERTIN GROUP SUBTOTAL*/"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT " & GroupColumn & "," & IIf(rbtSummary.Checked, GroupColumn, "'SUBTOTAL'") & " AS ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'S'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbcrlf + " GROUP BY " & GroupColumn & ""
    '    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()

    '    '    strSql = " /*INSERTIN Grand */"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'Else
    '    '    GroupColumn = "TEMPITEM"
    '    '    strSql = " /*INSERTIN Grand */"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'End If




    '    strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNWT = NULL WHERE OSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNWT = NULL WHERE RSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNWT = NULL WHERE ISTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNWT = NULL WHERE ARSTNWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNWT = NULL WHERE AISTNWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
    '    End If
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNWT = NULL WHERE CSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
    '    'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
    '    If chkWithSubItem.Checked Then
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
    '    End If
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()


    '    strSql = " SELECT PARTICULAR"
    '    'If chkWithSubItem.Checked Then
    '    '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
    '    'Else
    '    '    strSql += vbCrLf + " ITEM AS PARTICULAR"
    '    'End If
    '    strSql += vbCrLf + " ,OPCS,OGRSWT,ONETWT,ODIAWT,OSTNWT,OVALUE"
    '    strSql += vbCrLf + " ,RPCS,RGRSWT,RNETWT,RDIAWT,RSTNWT,RVALUE"
    '    strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IDIAWT,ISTNWT,IVALUE"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAWT,ARSTNWT,ARVALUE"
    '        strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAWT,AISTNWT,AIVALUE"
    '    End If
    '    strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAWT,CSTNWT,CVALUE"
    '    strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
    '    strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
    '    'strSql += vbcrlf + "  WHERE NOT(" & GroupColumn & " <> 'ZZZZZZ' AND RESULT <> 0 AND ISNULL(OPCS,0) = 0 AND ISNULL(RPCS,0) = 0 AND ISNULL(IPCS,0) = 0 AND ISNULL(CPCS,0) = 0 "
    '    'If chkGrsWt.Checked Then
    '    '    strSql += vbcrlf + "  AND ISNULL(OGRSWT,0) = 0 AND ISNULL(RGRSWT,0) = 0 AND ISNULL(IGRSWT,0) = 0 AND ISNULL(CGRSWT,0) = 0 "
    '    'End If
    '    'If chkNetWt.Checked Then
    '    '    strSql += vbcrlf + "  AND ISNULL(ONETWT,0) = 0 AND ISNULL(RNETWT,0) = 0 AND ISNULL(INETWT,0) = 0 AND ISNULL(CNETWT,0) = 0 "
    '    'End If
    '    'strSql += vbcrlf + " )"
    '    'If rbtSummary.Checked Then
    '    '    strSql += vbcrlf + " AND RESULT NOT IN (0,1)"
    '    'End If
    '    'If cmbGroupBy.Text = "COUNTER WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY COUNTER,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "DESIGNER WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY DESIGNER,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "COSTCENTRE WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY COSTNAME,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "ITEM WISE" And chkWithSubItem.Checked = True Then
    '    '    strSql += vbCrLf + " ORDER BY TEMPITEM,RESULT,STONE,PARTICULAR"
    '    'Else
    '    '    strSql += vbCrLf + " ORDER BY RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'End If

    '    'strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW"
    '    tabView.Show()
    '    Dim dtSource As New DataTable
    '    dtSource.Columns.Add("KEYNO", GetType(Integer))
    '    dtSource.Columns("KEYNO").AutoIncrement = True
    '    dtSource.Columns("KEYNO").AutoIncrementSeed = 0
    '    dtSource.Columns("KEYNO").AutoIncrementStep = 1
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtSource)
    '    Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
    '    For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
    '        ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
    '    Next
    '    If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
    '    ObjGrouper.pColumns_Sum.Add("OPCS")
    '    ObjGrouper.pColumns_Sum.Add("OGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("ONETWT")
    '    ObjGrouper.pColumns_Sum.Add("ODIAWT")
    '    ObjGrouper.pColumns_Sum.Add("OSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("OVALUE")

    '    ObjGrouper.pColumns_Sum.Add("RPCS")
    '    ObjGrouper.pColumns_Sum.Add("RGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("RNETWT")
    '    ObjGrouper.pColumns_Sum.Add("RDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("RSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("RVALUE")
    '    ObjGrouper.pColumns_Sum.Add("IPCS")
    '    ObjGrouper.pColumns_Sum.Add("IGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("INETWT")
    '    ObjGrouper.pColumns_Sum.Add("IDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("ISTNWT")
    '    ObjGrouper.pColumns_Sum.Add("IVALUE")

    '    If chkSeperateColumnApproval.Checked Then
    '        ObjGrouper.pColumns_Sum.Add("ARPCS")
    '        ObjGrouper.pColumns_Sum.Add("ARGRSWT")
    '        ObjGrouper.pColumns_Sum.Add("ARNETWT")
    '        ObjGrouper.pColumns_Sum.Add("ARDIAWT")
    '        ObjGrouper.pColumns_Sum.Add("ARSTNWT")
    '        ObjGrouper.pColumns_Sum.Add("ARVALUE")
    '        ObjGrouper.pColumns_Sum.Add("AIPCS")
    '        ObjGrouper.pColumns_Sum.Add("AIGRSWT")
    '        ObjGrouper.pColumns_Sum.Add("AINETWT")
    '        ObjGrouper.pColumns_Sum.Add("AIDIAWT")
    '        ObjGrouper.pColumns_Sum.Add("AISTNWT")
    '        ObjGrouper.pColumns_Sum.Add("AIVALUE")
    '    End If

    '    ObjGrouper.pColumns_Sum.Add("CPCS")
    '    ObjGrouper.pColumns_Sum.Add("CGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("CNETWT")
    '    ObjGrouper.pColumns_Sum.Add("CDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("CSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("CVALUE")
    '    ObjGrouper.pColName_Particular = "PARTICULAR"
    '    ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
    '    ObjGrouper.pColumns_Sort = "TITEM,STONE"
    '    ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
    '    ObjGrouper.GroupDgv()
    '    If HideSummary = False Then
    '        Dim ind As Integer = gridView.RowCount - 1
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        ''RECEIPT
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        ''ISSUE
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("INETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        If chkSeperateColumnApproval.Checked Then
    '            ''APP RECEIPT
    '            CType(gridView.DataSource, DataTable).Rows.Add()
    '            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
    '            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

    '            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '            ''APP ISSUE
    '            CType(gridView.DataSource, DataTable).Rows.Add()
    '            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
    '            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle

    '        End If
    '        ''CLOSING
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '    End If
    '    'If HideSummary = False Then
    '    '    strSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ',''ITEM,3 RESULT,' ',3 STONE "
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','OPENING'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,4 RESULT,'G',4 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','RECEIPT'ITEM,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 RESULT,'G',5 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','ISSUE'ITEM,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,6 RESULT,'G',6 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','CLOSING'ITEM,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,7 RESULT,'G',7 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'End If

    '    'Dim dt As New DataTable
    '    'dt.Columns.Add("KEYNO", GetType(Integer))
    '    'dt.Columns("KEYNO").AutoIncrement = True
    '    'dt.Columns("KEYNO").AutoIncrementSeed = 0
    '    'dt.Columns("KEYNO").AutoIncrementStep = 1
    '    'da = New OleDbDataAdapter(strSql, cn)
    '    'da.Fill(dt)
    '    'If Not dt.Rows.Count > 0 Then
    '    '    MsgBox("Record not found", MsgBoxStyle.Information)
    '    '    dtpAsOnDate.Focus()
    '    '    Exit Sub
    '    'End If
    '    'dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
    '    'tabView.Show()
    '    'gridView.DataSource = dt
    '    lblTitle.Text = ""
    '    If rbtTag.Checked Then lblTitle.Text += " TAGGED"
    '    If rbtNonTag.Checked Then lblTitle.Text += " NON TAGGED"
    '    lblTitle.Text += " ITEM WISE STOCK REPORT"
    '    If chkAsOnDate.Checked Then
    '        lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
    '    Else
    '        lblTitle.Text += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
    '    End If
    '    If chkCmbMetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkCmbMetal.Text
    '    If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
    '    lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
    '    GridStyle()
    '    gridView.Columns("COLHEAD").Visible = False
    '    GridViewHeaderStyle()
    '    tabMain.SelectedTab = tabView
    'End Sub


    Private Sub AppStock(ByVal flag As String)
        ''Flag "WITHOUT-WITH-APP
        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,SNO,STYLENO,RATE"
        strSql += vbCrLf + " ,DIAPCS,DIAWT,STNPCS,STNWT,CHKAPP"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT"
        If flag = "WITHOUT" Then
            strSql += vbCrLf + " 'OPE' SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*PCS ELSE PCS END AS PCS"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*GRSWT ELSE GRSWT END AS GRSWT"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*NETWT ELSE NETWT END AS NETWT"
            strSql += vbCrLf + " ,VALUE,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*DIAPCS ELSE DIAPCS END AS DIAPCS"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*DIAWT ELSE DIAWT END AS DIAWT"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*STNPCS ELSE STNPCS END AS STNPCS"
            strSql += vbCrLf + " ,-1*CASE WHEN SEP = 'REC' THEN -1*STNWT ELSE STNWT END AS STNWT"
            strSql += vbCrLf + " ,CHKAPP"
        Else
            strSql += vbCrLf + " SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,VALUE,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,DIAPCS,DIAWT,STNPCS,STNWT,CHKAPP"
        End If
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT 'OPE'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,CASE WHEN AP.TRANTYPE = 'AI' THEN T.PCS ELSE -1*T.PCS END AS PCS"
        strSql += vbCrLf + " ,CASE WHEN AP.TRANTYPE = 'AI' THEN T.GRSWT ELSE -1*T.GRSWT END AS GRSWT"
        strSql += vbCrLf + " ,CASE WHEN AP.TRANTYPE = 'AI' THEN T.NETWT ELSE -1*T.NETWT END AS NETWT"
        strSql += vbCrLf + " ,CASE WHEN AP.TRANTYPE = 'AI' THEN T.SALVALUE ELSE -1*T.SALVALUE END AS VALUE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN AP.TRANTYPE = 'AI' THEN 1 ELSE -1 END * (SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN AP.TRANTYPE = 'AI' THEN 1 ELSE -1 END *(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        End If
        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN AP.TRANTYPE = 'AI' THEN 1 ELSE -1 END *(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),CASE WHEN AP.TRANTYPE = 'AI' THEN 1 ELSE -1 END *(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        End If
        strSql += vbCrLf + " ,'A' CHKAPP"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,'AI' AS TRANTYPE FROM " & cnStockDb & "..APPISSUE WHERE TRANDATE < @ASONDATE AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'AI'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,'AI' AS TRANTYPE FROM " & cnStockDb & "..ISSUE WHERE TRANDATE < @ASONDATE AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'AI'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,'AR' AS TRANTYPE FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE < @ASONDATE AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'AR'"
        strSql += vbCrLf + " )AP ON AP.ITEMID = T.ITEMID AND AP.TAGNO = T.TAGNO"
        strSql += vbCrLf + " WHERE 1=1"
        strSql += itemCondStr
        strSql += tagCondStr
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '" & IIf(chkSeperateColumnApproval.Checked, "APPISS", "ISS") & "'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        End If
        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        End If
        strSql += vbCrLf + " ,'A' CHKAPP"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
        strSql += vbCrLf + " WHERE 1=1"
        strSql += itemCondStr
        strSql += tagCondStr
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '" & IIf(chkSeperateColumnApproval.Checked, "APPISS", "ISS") & "'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        End If
        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        End If
        strSql += vbCrLf + " ,'A' CHKAPP"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
        strSql += vbCrLf + " WHERE 1=1"
        strSql += itemCondStr
        strSql += tagCondStr
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '" & IIf(chkSeperateColumnApproval.Checked, "APPREC", "REC") & "'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        End If
        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        End If
        strSql += vbCrLf + " ,'A' CHKAPP"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
        strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
        strSql += vbCrLf + " WHERE 1=1"
        strSql += itemCondStr
        strSql += tagCondStr
        strSql += vbCrLf + " )XY"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    End Sub
    '/* ORIGINAL */
    Private Sub Report()
        Dim RecDate As String = Nothing
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        If chkAsOnDate.Checked Then
            dtpTo.Value = dtpFrom.Value
        End If

        RecDate = "T.RECDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTK')>0 DROP TABLE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP VARCHAR(10),COSTID VARCHAR(2)"
        strSql += vbCrLf + " ,DESIGNERID INT,ITEMCTRID INT,ITEMID INT,SUBITEMID INT,PCS INT,GRSWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,NETWT NUMERIC(15,3),VALUE NUMERIC(15,2),SNO VARCHAR(15),STYLENO VARCHAR(20),RATE NUMERIC(15,2)"
        strSql += vbCrLf + " ,DIAPCS INT,DIAWT NUMERIC(15,2),STNPCS INT,STNWT NUMERIC(15,4),CHKAPP VARCHAR(20)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If chkOnlyApproval.Checked Then
            AppStock("")
        End If
        If chkWithApproval.Checked = False And chkOnlyApproval.Checked = False And chkSeperateColumnApproval.Checked = False Then
            AppStock("WITHOUT")
        End If

        If (rbtTag.Checked Or rbtBoth.Checked) And chkOnlyApproval.Checked = False Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"

            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,DIAPCS,DIAWT,STNPCS,STNWT,CHKAPP"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " ,CONVERT(VARCHAR(20),'') CHKAPP"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " ,'' CHKAPP"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " ,'' CHKAPP"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " ,'' CHKAPP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
                strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
                ''strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " ,'' CHKAPP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
                strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " ,'' CHKAPP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
                strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
            strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO"
            strSql += " ,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            If chkOnlyApproval.Checked Then
                strSql += vbCrLf + " ,'A' CHKAPP"
            Else
                strSql += vbCrLf + " ,CONVERT(NVARCHAR(2),' ') CHKAPP"
            End If
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE < @ASONDATE"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'ISS'SEP"
            End If
            strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            If chkOnlyApproval.Checked Then
                strSql += vbCrLf + " ,'A' CHKAPP"
            Else
                strSql += vbCrLf + " ,CONVERT(NVARCHAR(2),' ') CHKAPP"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkWithApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            ElseIf chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            End If

            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'REC'SEP"
            End If
            strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            If chkOnlyApproval.Checked Then
                strSql += vbCrLf + " ,'A' CHKAPP"
            Else
                strSql += vbCrLf + " ,CONVERT(NVARCHAR(2),' ') CHKAPP"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkWithApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            ElseIf chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            End If
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
                strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT"
                If chkOnlyApproval.Checked Then
                    strSql += vbCrLf + " ,'A' CHKAPP"
                Else
                    strSql += vbCrLf + " ,CONVERT(NVARCHAR(2),' ') CHKAPP"
                End If
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE,'' CHKAPP"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     (SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.TITEMID)) + CONVERT(VARCHAR,X.TITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.SUBITEMID)) + CONVERT(VARCHAR,X.SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        End If
        strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
        strSql += " ("
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,0 STONE,DIAPCS,DIAWT,STNPCS,STNWT,STYLENO,RATE,CHKAPP "
            strSql += vbCrLf + " FROM TEMP" & systemId & "ITEMSTOCK"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,S.STYLENO,S.RATE,CHKAPP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                'strSql += vbCrLf + " UNION ALL"
                'strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                'strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T "
                'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                'strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNWT ELSE -1*STNWT END) AS STNWT,STYLENO,RATE,CHKAPP"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE,CHKAPP"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = ""
        If Not chkWithRate.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET RATE = NULL"
        If Not chkWithValue.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET VALUE = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET COSTID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET DESIGNERID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEMCTRID = NULL"
        If Not chkStyleNo.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET STYLENO = NULL"
        'If Not ChkLstGroupBy.CheckedItems.Contains("ITEM") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEM = NULL"
        If Not chkWithSubItem.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET SUBITEM = NULL"
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        If chkWithApproval.Checked = False And chkOnlyApproval.Checked = False And chkSeperateColumnApproval.Checked = False Then
            strSql += vbCrLf + " ,NULL AS OPCS"
            strSql += vbCrLf + " ,NULL AS OGRSWT"
            strSql += vbCrLf + " ,NULL AS ONETWT"
            strSql += vbCrLf + " ,NULL AS ODIAPCS"
            strSql += vbCrLf + " ,NULL AS ODIAWT"
            strSql += vbCrLf + " ,NULL AS OSTNPCS"
            strSql += vbCrLf + " ,NULL AS OSTNWT"
            strSql += vbCrLf + " ,NULL AS OVALUE"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNWT ELSE 0 END) AS OSTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS RSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS ISTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNWT ELSE 0 END) AS ARSTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNWT ELSE 0 END) AS AISTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
        End If
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNWT ELSE STNWT END) AS CSTNWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN PCS ELSE -1*PCS END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * PCS ELSE PCS END ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN GRSWT ELSE -1*GRSWT END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * GRSWT ELSE GRSWT END ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN GRSWT ELSE -1*NETWT END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * NETWT ELSE NETWT END ELSE NETWT END) AS CNETWT"


        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN DIAPCS ELSE -1*DIAPCS END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * DIAPCS ELSE DIAPCS END ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN DIAWT ELSE -1*DIAWT END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * DIAWT ELSE DIAWT END ELSE DIAWT END) AS CDIAWT"

        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN STNPCS ELSE -1*STNPCS END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * STNPCS ELSE STNPCS END ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN STNWT ELSE -1*STNWT END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * STNWT ELSE STNWT END ELSE STNWT END) AS CSTNWT"

        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN CASE WHEN CHKAPP = 'A' THEN VALUE ELSE -1*VALUE END WHEN SEP IN ('REC','APPREC') THEN CASE WHEN CHKAPP = 'A' THEN -1 * VALUE ELSE VALUE END ELSE VALUE END) AS CVALUE"

        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()

        'Dim GroupColumn As String = Nothing
        'If cmbGroupBy.Text <> "ITEM WISE" Or chkWithSubItem.Checked Then
        '    Select Case cmbGroupBy.Text
        '        Case "COUNTER WISE"
        '            GroupColumn = "COUNTER"
        '        Case "DESIGNER WISE"
        '            GroupColumn = "DESIGNER"
        '        Case "COSTCENTRE WISE"
        '            GroupColumn = "COSTNAME"
        '        Case "ITEM WISE"
        '            GroupColumn = "TEMPITEM"
        '    End Select
        '    strSql = " /*INSERTING GROUP TITLE*/"
        '    strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbcrlf + " SELECT DISTINCT " & GroupColumn & "," & GroupColumn & ",0 RESULT,'T'COLHEAD,3 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1"

        '    strSql += vbcrlf + " /*INSERTIN GROUP SUBTOTAL*/"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT " & GroupColumn & "," & IIf(rbtSummary.Checked, GroupColumn, "'SUBTOTAL'") & " AS ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'S'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbcrlf + " GROUP BY " & GroupColumn & ""
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 100
        'cmd.ExecuteNonQuery()

        If chkLoseStSepCol.Checked = True And rbtDiaStnByColumn.Checked Then
            strSql = vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,ODIAPCS = ISNULL(ODIAPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,ODIAWT = ISNULL(ODIAWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RDIAPCS = ISNULL(RDIAPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RDIAWT = ISNULL(RDIAWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,IDIAPCS = ISNULL(IDIAPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,IDIAWT = ISNULL(IDIAWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CDIAPCS = ISNULL(CDIAPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CDIAWT = ISNULL(CDIAWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            strSql += vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,OSTNPCS = ISNULL(OSTNPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,OSTNWT = ISNULL(OSTNWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RSTNPCS = ISNULL(RSTNPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RSTNWT = ISNULL(RSTNWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,ISTNPCS = ISNULL(ISTNPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,ISTNWT = ISNULL(ISTNWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CSTNPCS = ISNULL(CSTNPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CSTNWT = ISNULL(CSTNWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNWT = NULL WHERE OSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNWT = NULL WHERE RSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNWT = NULL WHERE ISTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNWT = NULL WHERE ARSTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNWT = NULL WHERE AISTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNWT = NULL WHERE CSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNWT,AIVALUE"
            'strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAPCS,ARDIAWT,ARSTNPCS,ARSTNWT,ARVALUE"
            'strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAPCS,AIDIAWT,AISTNPCS,AISTNWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNWT,CVALUE"
        'strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
        'strSql += vbcrlf + "  WHERE NOT(" & GroupColumn & " <> 'ZZZZZZ' AND RESULT <> 0 AND ISNULL(OPCS,0) = 0 AND ISNULL(RPCS,0) = 0 AND ISNULL(IPCS,0) = 0 AND ISNULL(CPCS,0) = 0 "
        'If chkGrsWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(OGRSWT,0) = 0 AND ISNULL(RGRSWT,0) = 0 AND ISNULL(IGRSWT,0) = 0 AND ISNULL(CGRSWT,0) = 0 "
        'End If
        'If chkNetWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(ONETWT,0) = 0 AND ISNULL(RNETWT,0) = 0 AND ISNULL(INETWT,0) = 0 AND ISNULL(CNETWT,0) = 0 "
        'End If
        'strSql += vbcrlf + " )"
        'If rbtSummary.Checked Then
        '    strSql += vbcrlf + " AND RESULT NOT IN (0,1)"
        'End If
        'If cmbGroupBy.Text = "COUNTER WISE" Then
        '    strSql += vbcrlf + " ORDER BY COUNTER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "DESIGNER WISE" Then
        '    strSql += vbcrlf + " ORDER BY DESIGNER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "COSTCENTRE WISE" Then
        '    strSql += vbcrlf + " ORDER BY COSTNAME,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "ITEM WISE" And chkWithSubItem.Checked = True Then
        '    strSql += vbCrLf + " ORDER BY TEMPITEM,RESULT,STONE,PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ORDER BY RESULT,TEMPITEM,STONE,PARTICULAR"
        'End If

        'strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW"
        tabView.Show()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        If HideSummary = False Then
            Dim ind As Integer = gridView.RowCount - 1
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("OSTNWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("RSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ISTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ARSTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''APP ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("AISTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            End If
            ''CLOSING
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("CSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If
        'If HideSummary = False Then
        '    strSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ',''ITEM,3 RESULT,' ',3 STONE "
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','OPENING'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,4 RESULT,'G',4 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','RECEIPT'ITEM,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 RESULT,'G',5 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','ISSUE'ITEM,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,6 RESULT,'G',6 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','CLOSING'ITEM,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,7 RESULT,'G',7 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If

        'Dim dt As New DataTable
        'dt.Columns.Add("KEYNO", GetType(Integer))
        'dt.Columns("KEYNO").AutoIncrement = True
        'dt.Columns("KEYNO").AutoIncrementSeed = 0
        'dt.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If Not dt.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    dtpAsOnDate.Focus()
        '    Exit Sub
        'End If
        'dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        'tabView.Show()
        'gridView.DataSource = dt
        lblTitle.Text = ""
        If rbtTag.Checked Then lblTitle.Text += " TAGGED"
        If rbtNonTag.Checked Then lblTitle.Text += " NON TAGGED"
        lblTitle.Text += " ITEM WISE STOCK REPORT"
        If chkAsOnDate.Checked Then
            lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
        Else
            lblTitle.Text += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        If chkCmbMetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkCmbMetal.Text
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle()
        tabMain.SelectedTab = tabView
    End Sub


    Private Sub Report_WithOutApp()


        Dim RecDate As String = Nothing
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        If chkAsOnDate.Checked Then
            dtpTo.Value = dtpFrom.Value
        End If

        Report_WithOutAppSub()

        RecDate = "T.RECDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTK')>0 DROP TABLE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"


            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPOPE AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"

            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPOPE AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPOPE AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
                strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
                strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
                strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval()
            End If
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
                strSql += vbCrLf + " WHERE 1=1"
                strSql += itemCondStr
                strSql += tagCondStr
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
            strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO"
            strSql += " ,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE < @ASONDATE"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'ISS'SEP"
            End If
            strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkWithApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            ElseIf chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            End If

            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'REC'SEP"
            End If
            strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkWithApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            ElseIf chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            End If
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
                strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     (SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.TITEMID)) + CONVERT(VARCHAR,X.TITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.SUBITEMID)) + CONVERT(VARCHAR,X.SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        End If
        strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
        strSql += " ("
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,0 STONE,DIAPCS,DIAWT,STNPCS,STNWT,STYLENO,RATE FROM TEMP" & systemId & "ITEMSTOCK"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                'strSql += vbCrLf + " UNION ALL"
                'strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                'strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T "
                'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                'strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNWT ELSE -1*STNWT END) AS STNWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = ""
        If Not chkWithRate.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET RATE = NULL"
        If Not chkWithValue.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET VALUE = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET COSTID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET DESIGNERID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEMCTRID = NULL"
        If Not chkStyleNo.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET STYLENO = NULL"
        'If Not ChkLstGroupBy.CheckedItems.Contains("ITEM") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEM = NULL"
        If Not chkWithSubItem.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET SUBITEM = NULL"
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNWT ELSE 0 END) AS OSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS RSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS ISTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNWT ELSE 0 END) AS ARSTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNWT ELSE 0 END) AS AISTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNWT ELSE STNWT END) AS CSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()

        'Dim GroupColumn As String = Nothing
        'If cmbGroupBy.Text <> "ITEM WISE" Or chkWithSubItem.Checked Then
        '    Select Case cmbGroupBy.Text
        '        Case "COUNTER WISE"
        '            GroupColumn = "COUNTER"
        '        Case "DESIGNER WISE"
        '            GroupColumn = "DESIGNER"
        '        Case "COSTCENTRE WISE"
        '            GroupColumn = "COSTNAME"
        '        Case "ITEM WISE"
        '            GroupColumn = "TEMPITEM"
        '    End Select
        '    strSql = " /*INSERTING GROUP TITLE*/"
        '    strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbcrlf + " SELECT DISTINCT " & GroupColumn & "," & GroupColumn & ",0 RESULT,'T'COLHEAD,3 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1"

        '    strSql += vbcrlf + " /*INSERTIN GROUP SUBTOTAL*/"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT " & GroupColumn & "," & IIf(rbtSummary.Checked, GroupColumn, "'SUBTOTAL'") & " AS ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'S'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbcrlf + " GROUP BY " & GroupColumn & ""
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 100
        'cmd.ExecuteNonQuery()

        If chkLoseStSepCol.Checked = True And rbtDiaStnByColumn.Checked Then
            strSql = vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,ODIAPCS = ISNULL(ODIAPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,ODIAWT = ISNULL(ODIAWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RDIAPCS = ISNULL(RDIAPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RDIAWT = ISNULL(RDIAWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,IDIAPCS = ISNULL(IDIAPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,IDIAWT = ISNULL(IDIAWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CDIAPCS = ISNULL(CDIAPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CDIAWT = ISNULL(CDIAWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            strSql += vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,OSTNPCS = ISNULL(OSTNPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,OSTNWT = ISNULL(OSTNWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RSTNPCS = ISNULL(RSTNPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RSTNWT = ISNULL(RSTNWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,ISTNPCS = ISNULL(ISTNPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,ISTNWT = ISNULL(ISTNWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CSTNPCS = ISNULL(CSTNPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CSTNWT = ISNULL(CSTNWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNWT = NULL WHERE OSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNWT = NULL WHERE RSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNWT = NULL WHERE ISTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNWT = NULL WHERE ARSTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNWT = NULL WHERE AISTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNWT = NULL WHERE CSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNWT,AIVALUE"
            'strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAPCS,ARDIAWT,ARSTNPCS,ARSTNWT,ARVALUE"
            'strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAPCS,AIDIAWT,AISTNPCS,AISTNWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNWT,CVALUE"
        'strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
        'strSql += vbcrlf + "  WHERE NOT(" & GroupColumn & " <> 'ZZZZZZ' AND RESULT <> 0 AND ISNULL(OPCS,0) = 0 AND ISNULL(RPCS,0) = 0 AND ISNULL(IPCS,0) = 0 AND ISNULL(CPCS,0) = 0 "
        'If chkGrsWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(OGRSWT,0) = 0 AND ISNULL(RGRSWT,0) = 0 AND ISNULL(IGRSWT,0) = 0 AND ISNULL(CGRSWT,0) = 0 "
        'End If
        'If chkNetWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(ONETWT,0) = 0 AND ISNULL(RNETWT,0) = 0 AND ISNULL(INETWT,0) = 0 AND ISNULL(CNETWT,0) = 0 "
        'End If
        'strSql += vbcrlf + " )"
        'If rbtSummary.Checked Then
        '    strSql += vbcrlf + " AND RESULT NOT IN (0,1)"
        'End If
        'If cmbGroupBy.Text = "COUNTER WISE" Then
        '    strSql += vbcrlf + " ORDER BY COUNTER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "DESIGNER WISE" Then
        '    strSql += vbcrlf + " ORDER BY DESIGNER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "COSTCENTRE WISE" Then
        '    strSql += vbcrlf + " ORDER BY COSTNAME,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "ITEM WISE" And chkWithSubItem.Checked = True Then
        '    strSql += vbCrLf + " ORDER BY TEMPITEM,RESULT,STONE,PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ORDER BY RESULT,TEMPITEM,STONE,PARTICULAR"
        'End If

        'strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW"
        tabView.Show()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        If HideSummary = False Then
            Dim ind As Integer = gridView.RowCount - 1
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("OSTNWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("RSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ISTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ARSTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''APP ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("AISTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            End If
            ''CLOSING
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("CSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If
        'If HideSummary = False Then
        '    strSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ',''ITEM,3 RESULT,' ',3 STONE "
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','OPENING'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,4 RESULT,'G',4 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','RECEIPT'ITEM,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 RESULT,'G',5 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','ISSUE'ITEM,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,6 RESULT,'G',6 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','CLOSING'ITEM,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,7 RESULT,'G',7 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If

        'Dim dt As New DataTable
        'dt.Columns.Add("KEYNO", GetType(Integer))
        'dt.Columns("KEYNO").AutoIncrement = True
        'dt.Columns("KEYNO").AutoIncrementSeed = 0
        'dt.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If Not dt.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    dtpAsOnDate.Focus()
        '    Exit Sub
        'End If
        'dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        'tabView.Show()
        'gridView.DataSource = dt
        lblTitle.Text = ""
        If rbtTag.Checked Then lblTitle.Text += " TAGGED"
        If rbtNonTag.Checked Then lblTitle.Text += " NON TAGGED"
        lblTitle.Text += " ITEM WISE STOCK REPORT"
        If chkAsOnDate.Checked Then
            lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
        Else
            lblTitle.Text += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        If chkCmbMetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkCmbMetal.Text
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle()
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub Report_WithOutAppSub()
        Dim RecDate As String = Nothing
        RecDate = "T.RECDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APP')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APP"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APPOPE')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APPOPE"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APPTRAN')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APPTRAN"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"

            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
            strSql += vbCrLf + " SELECT 'OPE' SEP,TAGNO,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN PCS ELSE -1 * PCS END) PCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN GRSWT ELSE -1 * GRSWT END) GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN NETWT ELSE -1 * NETWT END) NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN VALUE ELSE -1 * VALUE END) VALUE"
            strSql += vbCrLf + " ,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAPCS ELSE -1 * DIAPCS END) DIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAWT ELSE -1 * DIAWT END) DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNPCS ELSE -1 * STNPCS END) STNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNWT ELSE -1 * STNWT END) STNWT"
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " FROM (/* A STARTS */"
            strSql += vbCrLf + " SELECT 'I' ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..ISSUE AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND I.TRANDATE < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R'ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND I.TRANDATE < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += vbCrLf + " )A GROUP BY COSTID,DESIGNERID,TAGNO,ITEMCTRID,ITEMID,SUBITEMID,SNO,STYLENO,RATE"

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..ISSUE AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            strSql += vbCrLf + " AND I.TRANDATE <= @TODATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            strSql += itemCondStr
            strSql += tagCondStr

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            strSql += vbCrLf + " AND I.TRANDATE <= @TODATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            strSql += itemCondStr
            strSql += tagCondStr
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT ITEMID,TAGNO "
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APPOPE FROM TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " WHERE SEP IN ('OPE')"
            strSql += vbCrLf + " GROUP BY ITEMID,TAGNO  "
            strSql += vbCrLf + " HAVING "
            strSql += vbCrLf + " SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE -1 * PCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE -1 * GRSWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE -1 * NETWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE -1 * VALUE END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE -1 * DIAPCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE -1 * STNWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE -1 * STNPCS END) <> 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT ITEMID,TAGNO "
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APPTRAN FROM TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " WHERE SEP NOT IN ('OPE')"
            strSql += vbCrLf + " GROUP BY ITEMID,TAGNO  "
            strSql += vbCrLf + " HAVING "
            strSql += vbCrLf + " SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE -1 * PCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE -1 * GRSWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE -1 * NETWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE -1 * VALUE END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE -1 * DIAPCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE -1 * STNWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE -1 * STNPCS END) <> 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()

        End If
    End Sub

    Private Sub Report_AppOnly()
        Dim RecDate As String = Nothing
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        If chkAsOnDate.Checked Then
            dtpTo.Value = dtpFrom.Value
        End If
        RecDate = "I.TRANDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTK')>0 DROP TABLE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"

            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
            strSql += vbCrLf + " SELECT 'OPE' SEP,TAGNO,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN PCS ELSE -1 * PCS END) PCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN GRSWT ELSE -1 * GRSWT END) GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN NETWT ELSE -1 * NETWT END) NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN VALUE ELSE -1 * VALUE END) VALUE"
            strSql += vbCrLf + " ,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAPCS ELSE -1 * DIAPCS END) DIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAWT ELSE -1 * DIAWT END) DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNPCS ELSE -1 * STNPCS END) STNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNWT ELSE -1 * STNWT END) STNWT"
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM (/* A STARTS */"
            strSql += vbCrLf + " SELECT 'I' ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..ISSUE AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            'strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R'ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            'strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " )A GROUP BY COSTID,DESIGNERID,TAGNO,ITEMCTRID,ITEMID,SUBITEMID,SNO,STYLENO,RATE"



            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..ISSUE AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            'If chkWithCumulative.Checked Then
            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '    End If
            '    strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
            '    strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            '    strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
            '    'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
            '    strSql += itemCondStr
            '    strSql += tagCondStr
            '    strSql += funcApproval()
            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '    End If
            '    strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
            '    strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            '    'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
            '    strSql += itemCondStr
            '    strSql += tagCondStr
            '    strSql += funcApproval()
            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '    End If
            '    strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
            '    strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            '    'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
            '    strSql += itemCondStr
            '    strSql += tagCondStr
            '    strSql += funcApproval()
            'End If
            '    If chkSeperateColumnApproval.Checked Then
            '        strSql += vbCrLf + " UNION ALL"
            '        strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
            '        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '        End If
            '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            '        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
            '        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
            '        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            '        strSql += vbCrLf + " WHERE 1=1"
            '        strSql += itemCondStr
            '        strSql += tagCondStr
            '        strSql += vbCrLf + " UNION ALL"
            '        strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
            '        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '        End If
            '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            '        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
            '        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
            '        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            '        strSql += vbCrLf + " WHERE 1=1"
            '        strSql += itemCondStr
            '        strSql += tagCondStr
            '        strSql += vbCrLf + " UNION ALL"
            '        strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
            '        strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
            '        End If
            '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            '        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
            '        strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
            '        strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            '        strSql += vbCrLf + " WHERE 1=1"
            '        strSql += itemCondStr
            '        strSql += tagCondStr
            '    End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        'If rbtNonTag.Checked Or rbtBoth.Checked Then
        '    strSql = " DECLARE @ASONDATE SMALLDATETIME"
        '    strSql += " DECLARE @TODATE SMALLDATETIME"
        '    strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        '    strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        '    strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
        '    strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO"
        '    strSql += " ,RECISS"
        '    strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
        '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        '    End If
        '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        '    End If
        '    strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
        '    strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE < @ASONDATE"
        '    strSql += itemCondStr
        '    strSql += tagCondStr
        '    strSql += funcApproval()
        '    If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
        '    strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        '    strSql += vbCrLf + "  UNION ALL"
        '    If chkSeperateColumnApproval.Checked Then
        '        strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
        '    Else
        '        strSql += vbCrLf + "  SELECT 'ISS'SEP"
        '    End If
        '    strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
        '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        '    End If
        '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        '    End If
        '    strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
        '    strSql += itemCondStr
        '    strSql += tagCondStr
        '    If chkWithApproval.Checked Then
        '        strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
        '    ElseIf chkOnlyApproval.Checked Then
        '        strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
        '    End If

        '    If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
        '    strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        '    strSql += vbCrLf + "  UNION ALL"
        '    If chkSeperateColumnApproval.Checked Then
        '        strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
        '    Else
        '        strSql += vbCrLf + "  SELECT 'REC'SEP"
        '    End If
        '    strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
        '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        '    End If
        '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        '    Else
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNPCS"
        '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
        '    End If
        '    strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
        '    strSql += itemCondStr
        '    strSql += tagCondStr
        '    If chkWithApproval.Checked Then
        '        strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
        '    ElseIf chkOnlyApproval.Checked Then
        '        strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
        '    End If
        '    If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
        '    strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        '    If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
        '        Dim diaStone As String = ""
        '        If chkDiamond.Checked Then diaStone += "'D',"
        '        If chkStone.Checked Then diaStone += "'S',"
        '        diaStone = Mid(diaStone, 1, diaStone.Length - 1)
        '        strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
        '        strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
        '        strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT"
        '        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
        '        strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
        '    End If
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 100
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     (SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.TITEMID)) + CONVERT(VARCHAR,X.TITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
        End If
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.SUBITEMID)) + CONVERT(VARCHAR,X.SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        End If
        strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
        strSql += " ("
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,0 STONE,DIAPCS,DIAWT,STNPCS,STNWT,STYLENO,RATE FROM TEMP" & systemId & "ITEMSTOCK"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                'strSql += vbCrLf + " UNION ALL"
                'strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                'strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T "
                'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                'strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNWT ELSE -1*STNWT END) AS STNWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = ""
        If Not chkWithRate.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET RATE = NULL"
        If Not chkWithValue.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET VALUE = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET COSTID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET DESIGNERID = NULL"
        If Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEMCTRID = NULL"
        If Not chkStyleNo.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET STYLENO = NULL"
        'If Not ChkLstGroupBy.CheckedItems.Contains("ITEM") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEM = NULL"
        If Not chkWithSubItem.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET SUBITEM = NULL"
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNWT ELSE 0 END) AS OSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS RSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS ISTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNWT ELSE 0 END) AS ARSTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNWT ELSE 0 END) AS AISTNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
        End If
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNWT ELSE STNWT END) AS CSTNWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*PCS ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*STNWT ELSE STNWT END) AS CSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()

        'Dim GroupColumn As String = Nothing
        'If cmbGroupBy.Text <> "ITEM WISE" Or chkWithSubItem.Checked Then
        '    Select Case cmbGroupBy.Text
        '        Case "COUNTER WISE"
        '            GroupColumn = "COUNTER"
        '        Case "DESIGNER WISE"
        '            GroupColumn = "DESIGNER"
        '        Case "COSTCENTRE WISE"
        '            GroupColumn = "COSTNAME"
        '        Case "ITEM WISE"
        '            GroupColumn = "TEMPITEM"
        '    End Select
        '    strSql = " /*INSERTING GROUP TITLE*/"
        '    strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbcrlf + " SELECT DISTINCT " & GroupColumn & "," & GroupColumn & ",0 RESULT,'T'COLHEAD,3 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1"

        '    strSql += vbcrlf + " /*INSERTIN GROUP SUBTOTAL*/"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT " & GroupColumn & "," & IIf(rbtSummary.Checked, GroupColumn, "'SUBTOTAL'") & " AS ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'S'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbcrlf + " GROUP BY " & GroupColumn & ""
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 100
        'cmd.ExecuteNonQuery()

        If chkLoseStSepCol.Checked = True And rbtDiaStnByColumn.Checked Then
            strSql = vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,ODIAPCS = ISNULL(ODIAPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,ODIAWT = ISNULL(ODIAWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RDIAPCS = ISNULL(RDIAPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RDIAWT = ISNULL(RDIAWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,IDIAPCS = ISNULL(IDIAPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,IDIAWT = ISNULL(IDIAWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CDIAPCS = ISNULL(CDIAPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CDIAWT = ISNULL(CDIAWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'D' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            strSql += vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = 0,OSTNPCS = ISNULL(OSTNPCS,0) + ISNULL(OPCS,0),OGRSWT = 0,OSTNWT = ISNULL(OSTNWT,0) + ISNULL(OGRSWT,0),ONETWT = 0"
            strSql += vbCrLf + "						 ,RPCS = 0,RSTNPCS = ISNULL(RSTNPCS,0) + ISNULL(RPCS,0),RGRSWT = 0,RSTNWT = ISNULL(RSTNWT,0) + ISNULL(RGRSWT,0),RNETWT = 0"
            strSql += vbCrLf + "						 ,IPCS = 0,ISTNPCS = ISNULL(ISTNPCS,0) + ISNULL(IPCS,0),IGRSWT = 0,ISTNWT = ISNULL(ISTNWT,0) + ISNULL(IGRSWT,0),INETWT = 0"
            strSql += vbCrLf + "						 ,CPCS = 0,CSTNPCS = ISNULL(CSTNPCS,0) + ISNULL(CPCS,0),CGRSWT = 0,CSTNWT = ISNULL(CSTNWT,0) + ISNULL(CGRSWT,0),CNETWT = 0							"
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "WHERE LTRIM(SUBSTRING(ITEM,8,LEN(ITEM))) IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            Else
                strSql += vbCrLf + "WHERE ITEM IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') = 'T' AND ISNULL(STUDDED,0) = 'L')	"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 100
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNWT = NULL WHERE OSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNWT = NULL WHERE RSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNWT = NULL WHERE ISTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNWT = NULL WHERE ARSTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNWT = NULL WHERE AISTNWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNWT = NULL WHERE CSTNWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 100
        cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNWT,AIVALUE"
            'strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAPCS,ARDIAWT,ARSTNPCS,ARSTNWT,ARVALUE"
            'strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAPCS,AIDIAWT,AISTNPCS,AISTNWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNWT,CVALUE"
        'strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
        'strSql += vbcrlf + "  WHERE NOT(" & GroupColumn & " <> 'ZZZZZZ' AND RESULT <> 0 AND ISNULL(OPCS,0) = 0 AND ISNULL(RPCS,0) = 0 AND ISNULL(IPCS,0) = 0 AND ISNULL(CPCS,0) = 0 "
        'If chkGrsWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(OGRSWT,0) = 0 AND ISNULL(RGRSWT,0) = 0 AND ISNULL(IGRSWT,0) = 0 AND ISNULL(CGRSWT,0) = 0 "
        'End If
        'If chkNetWt.Checked Then
        '    strSql += vbcrlf + "  AND ISNULL(ONETWT,0) = 0 AND ISNULL(RNETWT,0) = 0 AND ISNULL(INETWT,0) = 0 AND ISNULL(CNETWT,0) = 0 "
        'End If
        'strSql += vbcrlf + " )"
        'If rbtSummary.Checked Then
        '    strSql += vbcrlf + " AND RESULT NOT IN (0,1)"
        'End If
        'If cmbGroupBy.Text = "COUNTER WISE" Then
        '    strSql += vbcrlf + " ORDER BY COUNTER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "DESIGNER WISE" Then
        '    strSql += vbcrlf + " ORDER BY DESIGNER,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "COSTCENTRE WISE" Then
        '    strSql += vbcrlf + " ORDER BY COSTNAME,RESULT,TEMPITEM,STONE,PARTICULAR"
        'ElseIf cmbGroupBy.Text = "ITEM WISE" And chkWithSubItem.Checked = True Then
        '    strSql += vbCrLf + " ORDER BY TEMPITEM,RESULT,STONE,PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ORDER BY RESULT,TEMPITEM,STONE,PARTICULAR"
        'End If

        'strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW"
        tabView.Show()
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        If HideSummary = False Then
            Dim ind As Integer = gridView.RowCount - 1
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("OSTNWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("RSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ISTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ARSTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''APP ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("AISTNWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            End If
            ''CLOSING
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("CSTNWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If
        'If HideSummary = False Then
        '    strSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ',''ITEM,3 RESULT,' ',3 STONE "
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RESULT,COLHEAD,STONE)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','OPENING'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,4 RESULT,'G',4 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','RECEIPT'ITEM,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 RESULT,'G',5 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','ISSUE'ITEM,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,6 RESULT,'G',6 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    strSql += vbCrLf + " UNION ALL"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','CLOSING'ITEM,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,7 RESULT,'G',7 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.CommandTimeout = 100
        '    cmd.ExecuteNonQuery()
        'End If

        'Dim dt As New DataTable
        'dt.Columns.Add("KEYNO", GetType(Integer))
        'dt.Columns("KEYNO").AutoIncrement = True
        'dt.Columns("KEYNO").AutoIncrementSeed = 0
        'dt.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If Not dt.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    dtpAsOnDate.Focus()
        '    Exit Sub
        'End If
        'dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        'tabView.Show()
        'gridView.DataSource = dt
        lblTitle.Text = ""
        If rbtTag.Checked Then lblTitle.Text += " TAGGED"
        If rbtNonTag.Checked Then lblTitle.Text += " NON TAGGED"
        lblTitle.Text += " ITEM WISE STOCK REPORT"
        If chkAsOnDate.Checked Then
            lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
        Else
            lblTitle.Text += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        If chkCmbMetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkCmbMetal.Text
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle()
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub GridViewHeaderStyle()

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE", GetType(String))
            .Columns.Add("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE", GetType(String))
            .Columns.Add("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE", GetType(String))
            If chkSeperateColumnApproval.Checked Then
                .Columns.Add("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE", GetType(String))
                .Columns.Add("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE", GetType(String))
            End If
            .Columns.Add("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE", GetType(String))
            .Columns.Add("RATE~STYLENO", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Caption = "OPENING"
            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Caption = "RECEIPT"
            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Caption = "ISSUE"
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Caption = "APP REC"
                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Caption = "APP ISS"
            End If
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Caption = "CLOSING"
            .Columns("RATE~STYLENO").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkOnlyApproval.Checked = True And rbtTag.Checked = False Then
            MsgBox("Approval is allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If

        If chkOnlyTag.Checked = True And rbtTag.Checked = False Then
            MsgBox("Filtering allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If
        itemCondStr = funcItemFiltration() + " "
        tagCondStr = funcTagFiltration() + " "
        emptyCondStr = funcEmptyItemFiltration() + ""
        emptyCondStr_NONTAG = funcEmptyItemFiltration_NONTAG() + ""
        Prop_Sets()
        ''NEW MODIFICATION WITH COUNTER CHANGE
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Report()
        'If NormalMode = False Then
        '    If chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
        '        Report_WithOutApp()
        '    ElseIf chkOnlyApproval.Checked = True Then
        '        Report_AppOnly()
        '    Else
        '        Report()
        '    End If
        'Else
        '    Report()
        'End If

    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub

    Function funcColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width = gridView.Columns("OPCS").Width
            If chkGrsWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width += gridView.Columns("OGRSWT").Width
            If chkNetWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width += gridView.Columns("ONETWT").Width
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("OSTNWT").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNWT").Visible, gridView.Columns("OSTNWT").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE").HeaderText = "OPENING"

            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width = gridView.Columns("RPCS").Width
            If chkGrsWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width += gridView.Columns("RGRSWT").Width
            If chkNetWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width += gridView.Columns("RNETWT").Width
            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNWT").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE").HeaderText = "RECEIPT"

            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width = gridView.Columns("IPCS").Width
            If chkGrsWt.Checked Then .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width += gridView.Columns("IGRSWT").Width
            If chkNetWt.Checked Then .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width += gridView.Columns("INETWT").Width
            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNWT").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE").HeaderText = "ISSUE"

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width = gridView.Columns("ARPCS").Width
                If chkGrsWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARGRSWT").Width
                If chkNetWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARNETWT").Width
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAPCS").Width, 0) + IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAWT").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARSTNWT").Visible, gridView.Columns("ARSTNPCS").Width, 0) + IIf(gridView.Columns("ARSTNWT").Visible, gridView.Columns("ARSTNWT").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARVALUE").Visible, gridView.Columns("ARVALUE").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE").HeaderText = "APP REC"

                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width = gridView.Columns("AIPCS").Width
                If chkGrsWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AIGRSWT").Width
                If chkNetWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AINETWT").Width
                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0) + IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AISTNWT").Visible, gridView.Columns("AISTNPCS").Width, 0) + IIf(gridView.Columns("AISTNWT").Visible, gridView.Columns("AISTNWT").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIVALUE").Visible, gridView.Columns("AIVALUE").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE").HeaderText = "APP ISS"
            End If

            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width = gridView.Columns("CPCS").Width
            If chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width += gridView.Columns("CGRSWT").Width
            If chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width += gridView.Columns("CNETWT").Width
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAPCS").Width, 0) + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CSTNWT").Visible, gridView.Columns("CSTNPCS").Width, 0) + IIf(gridView.Columns("CSTNWT").Visible, gridView.Columns("CSTNWT").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CVALUE").Visible, gridView.Columns("CVALUE").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CVALUE").HeaderText = "CLOSING"

            .Columns("RATE~STYLENO").Width = IIf(gridView.Columns("RATE").Visible, gridView.Columns("RATE").Width, 0) _
            + IIf(gridView.Columns("STYLENO").Visible, gridView.Columns("STYLENO").Width, 0)
            .Columns("RATE~STYLENO").Visible = gridView.Columns("RATE").Visible Or gridView.Columns("STYLENO").Visible
            .Columns("RATE~STYLENO").HeaderText = ""

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
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
        'funcLoadMetal()
        funcLoadCategory()
        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
        ' chkDiamond.Checked = False
        ' chkStone.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        ' chkAsOnDate.Checked = True
        funcLoadItemName()
        ' chkWithApproval.Checked = False
        chkCmbMetal.Select()
        Prop_Gets()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
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
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        strSql = " SELECT 'ALL' NAME,'ALL' ITEMTYPEID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT NAME,CONVERT(vARCHAR,ITEMTYPEID),2 RESULT FROM " & cnAdminDb & "..ITEMTYPE"
        strSql += " ORDER BY RESULT,NAME"
        dtItemType = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemType)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")

        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
    End Sub

    Private Sub chkOnlyTag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyTag.CheckedChanged
        If chkOnlyTag.Checked = True Then
            chkNetWt.Checked = False
            chkGrsWt.Checked = False
            chkGrsWt.Enabled = False
            chkNetWt.Enabled = False
            chkDiamond.Checked = False
            chkStone.Checked = False
            chkDiamond.Enabled = False
            chkStone.Checked = False
        Else
            chkGrsWt.Enabled = True
            chkNetWt.Enabled = True
            chkDiamond.Enabled = True
            chkStone.Checked = True
        End If
    End Sub

    Private Sub chkDiamond_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDiamond.CheckedChanged
        If chkDiamond.Checked Or chkStone.Checked Then
            pnlDisStnResult.Enabled = True
        Else
            pnlDisStnResult.Enabled = False
        End If
    End Sub

    Private Sub chkStone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkStone.CheckedChanged
        If chkDiamond.Checked Or chkStone.Checked Then
            pnlDisStnResult.Enabled = True
        Else
            pnlDisStnResult.Enabled = False
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

    Private Sub chkCmbMetal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.TextChanged
        funcLoadItemName()
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As On Date"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub

    Private Sub chkWithApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWithApproval.Click
        If chkOnlyApproval.Checked = True Then chkOnlyApproval.Checked = False
        If chkSeperateColumnApproval.Checked = True Then chkSeperateColumnApproval.Checked = False
    End Sub

    Private Sub chkOnlyApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyApproval.Click
        If chkWithApproval.Checked = True Then chkWithApproval.Checked = False
        If chkSeperateColumnApproval.Checked = True Then chkSeperateColumnApproval.Checked = False
    End Sub

    Private Sub chkSeperateColumnApproval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeperateColumnApproval.CheckedChanged
        If chkSeperateColumnApproval.Checked Then
            rbtAll.Checked = True
            rbtRegular.Enabled = False
            rbtOrder.Enabled = False
        Else
            rbtRegular.Enabled = True
            rbtOrder.Enabled = True
        End If
    End Sub

    Private Sub chkSeperateColumnApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeperateColumnApproval.Click
        If chkWithApproval.Checked = True Then chkWithApproval.Checked = False
        If chkOnlyApproval.Checked = True Then chkOnlyApproval.Checked = False
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseStock_NEW_Properties
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        obj.p_cmbCategory = cmbCategory.Text
        GetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem)
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter)
        GetChecked_CheckedList(chkCmbItemType, obj.p_chkCmbItemType)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        obj.p_chkOnlyTag = chkOnlyTag.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkWithValue = chkWithValue.Checked
        obj.p_chkWithRate = chkWithRate.Checked
        obj.p_chkStyleNo = chkStyleNo.Checked
        obj.p_chkAll = chkAll.Checked
        obj.p_chkWithNegativeStock = chkWithNegativeStock.Checked
        obj.p_chkWithSubItem = chkWithSubItem.Checked
        obj.p_chkDiamond = chkDiamond.Checked
        obj.p_chkStone = chkStone.Checked
        obj.p_rbtDiaStnByRow = rbtDiaStnByRow.Checked
        obj.p_rbtDiaStnByColumn = rbtDiaStnByColumn.Checked
        obj.p_chkWithApproval = chkWithApproval.Checked
        obj.p_chkOnlyApproval = chkOnlyApproval.Checked
        obj.p_chkSeperateColumnApproval = chkSeperateColumnApproval.Checked
        obj.p_chkOrderbyItemId = chkOrderbyItemId.Checked
        obj.p_rbtAll = rbtAll.Checked
        obj.p_rbtRegular = rbtRegular.Checked
        obj.p_rbtOrder = rbtOrder.Checked
        obj.p_chkWithCumulative = chkWithCumulative.Checked
        obj.p_chkLoseStSepCol = chkLoseStSepCol.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseStock_NEW_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseStock_NEW_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseStock_NEW_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        cmbCategory.Text = obj.p_cmbCategory
        SetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem, "ALL")
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        SetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter, "ALL")
        SetChecked_CheckedList(chkCmbItemType, obj.p_chkCmbItemType, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy, Nothing)
        rbtBoth.Checked = obj.p_rbtBoth
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
        chkOnlyTag.Checked = obj.p_chkOnlyTag
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkWithValue.Checked = obj.p_chkWithValue
        chkWithRate.Checked = obj.p_chkWithRate
        chkStyleNo.Checked = obj.p_chkStyleNo
        chkAll.Checked = obj.p_chkAll
        chkWithNegativeStock.Checked = obj.p_chkWithNegativeStock
        chkWithSubItem.Checked = obj.p_chkWithSubItem
        chkDiamond.Checked = obj.p_chkDiamond
        chkStone.Checked = obj.p_chkStone
        rbtDiaStnByRow.Checked = obj.p_rbtDiaStnByRow
        rbtDiaStnByColumn.Checked = obj.p_rbtDiaStnByColumn
        chkWithApproval.Checked = obj.p_chkWithApproval
        chkOnlyApproval.Checked = obj.p_chkOnlyApproval
        chkSeperateColumnApproval.Checked = obj.p_chkSeperateColumnApproval
        chkOrderbyItemId.Checked = obj.p_chkOrderbyItemId
        rbtAll.Checked = obj.p_rbtAll
        rbtRegular.Checked = obj.p_rbtRegular
        rbtOrder.Checked = obj.p_rbtOrder
        chkWithCumulative.Checked = obj.p_chkWithCumulative
        chkLoseStSepCol.Checked = obj.p_chkLoseStSepCol
    End Sub
End Class

Public Class frmItemWiseStock_NEW_Properties
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property

    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property

    Private chkCmbItem As New List(Of String)
    Public Property p_chkCmbItem() As List(Of String)
        Get
            Return chkCmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItem = value
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

    Private chkCmbDesigner As New List(Of String)
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
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

    Private chkCmbCounter As New List(Of String)
    Public Property p_chkCmbCounter() As List(Of String)
        Get
            Return chkCmbCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCounter = value
        End Set
    End Property

    Private chkCmbItemType As New List(Of String)
    Public Property p_chkCmbItemType() As List(Of String)
        Get
            Return chkCmbItemType
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItemType = value
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

    Private ChkLstGroupBy As New List(Of String)
    Public Property p_ChkLstGroupBy() As List(Of String)
        Get
            Return ChkLstGroupBy
        End Get
        Set(ByVal value As List(Of String))
            ChkLstGroupBy = value
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

    Private chkOnlyTag As Boolean = False
    Public Property p_chkOnlyTag() As Boolean
        Get
            Return chkOnlyTag
        End Get
        Set(ByVal value As Boolean)
            chkOnlyTag = value
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

    Private chkWithValue As Boolean = False
    Public Property p_chkWithValue() As Boolean
        Get
            Return chkWithValue
        End Get
        Set(ByVal value As Boolean)
            chkWithValue = value
        End Set
    End Property

    Private chkWithRate As Boolean = False
    Public Property p_chkWithRate() As Boolean
        Get
            Return chkWithRate
        End Get
        Set(ByVal value As Boolean)
            chkWithRate = value
        End Set
    End Property

    Private chkStyleNo As Boolean = False
    Public Property p_chkStyleNo() As Boolean
        Get
            Return chkStyleNo
        End Get
        Set(ByVal value As Boolean)
            chkStyleNo = value
        End Set
    End Property

    Private chkAll As Boolean = False
    Public Property p_chkAll() As Boolean
        Get
            Return chkAll
        End Get
        Set(ByVal value As Boolean)
            chkAll = value
        End Set
    End Property

    Private chkWithNegativeStock As Boolean = True
    Public Property p_chkWithNegativeStock() As Boolean
        Get
            Return chkWithNegativeStock
        End Get
        Set(ByVal value As Boolean)
            chkWithNegativeStock = value
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

    Private chkDiamond As Boolean = False
    Public Property p_chkDiamond() As Boolean
        Get
            Return chkDiamond
        End Get
        Set(ByVal value As Boolean)
            chkDiamond = value
        End Set
    End Property

    Private chkStone As Boolean = False
    Public Property p_chkStone() As Boolean
        Get
            Return chkStone
        End Get
        Set(ByVal value As Boolean)
            chkStone = value
        End Set
    End Property
    Private rbtDiaStnByRow As Boolean = True
    Public Property p_rbtDiaStnByRow() As Boolean
        Get
            Return rbtDiaStnByRow
        End Get
        Set(ByVal value As Boolean)
            rbtDiaStnByRow = value
        End Set
    End Property

    Private rbtDiaStnByColumn As Boolean = False
    Public Property p_rbtDiaStnByColumn() As Boolean
        Get
            Return rbtDiaStnByColumn
        End Get
        Set(ByVal value As Boolean)
            rbtDiaStnByColumn = value
        End Set
    End Property

    Private chkWithApproval As Boolean = False
    Public Property p_chkWithApproval() As Boolean
        Get
            Return chkWithApproval
        End Get
        Set(ByVal value As Boolean)
            chkWithApproval = value
        End Set
    End Property

    Private chkOnlyApproval As Boolean = False
    Public Property p_chkOnlyApproval() As Boolean
        Get
            Return chkOnlyApproval
        End Get
        Set(ByVal value As Boolean)
            chkOnlyApproval = value
        End Set
    End Property

    Private chkSeperateColumnApproval As Boolean = False
    Public Property p_chkSeperateColumnApproval() As Boolean
        Get
            Return chkSeperateColumnApproval
        End Get
        Set(ByVal value As Boolean)
            chkSeperateColumnApproval = value
        End Set
    End Property

    Private chkOrderbyItemId As Boolean = True
    Public Property p_chkOrderbyItemId() As Boolean
        Get
            Return chkOrderbyItemId
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyItemId = value
        End Set
    End Property

    Private rbtAll As Boolean = True
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property

    Private rbtRegular As Boolean = False
    Public Property p_rbtRegular() As Boolean
        Get
            Return rbtRegular
        End Get
        Set(ByVal value As Boolean)
            rbtRegular = value
        End Set
    End Property

    Private rbtOrder As Boolean = False
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property
    Private chkWithCumulative As Boolean = False
    Public Property p_chkWithCumulative() As Boolean
        Get
            Return chkWithCumulative
        End Get
        Set(ByVal value As Boolean)
            chkWithCumulative = value
        End Set
    End Property

    Private chkLoseStSepCol As Boolean = False
    Public Property p_chkLoseStSepCol() As Boolean
        Get
            Return chkLoseStSepCol
        End Get
        Set(ByVal value As Boolean)
            chkLoseStSepCol = value
        End Set
    End Property

End Class