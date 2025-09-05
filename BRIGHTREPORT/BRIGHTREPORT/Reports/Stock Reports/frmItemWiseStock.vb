Imports System.Data.OleDb
Imports System.Xml
Imports System.IO
Public Class frmItemWiseStock
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
    Dim DISABLE_ECISEDUTY As Boolean = IIf(GetAdmindbSoftValue("DISABLE_EXCISEDUTY", "N") = "Y", True, False)
    Dim dtGrid As New DataTable()
    Dim DiaRnd As Integer = 3
    Dim StoneDetail As Boolean = False
    Dim Authorize As Boolean = False
    Dim Save As Boolean = False
    Dim dtgroupid As New DataTable
    Dim dtparty As New DataTable
    Dim PartyCondStr As String = Nothing
    Dim Sepaccodeview As Boolean = IIf(GetAdmindbSoftValue("RPTITEMSTK_ACCODE", "N") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Dim grpby As String = ""
    Dim StkType As String = ""
    Dim HMDetail As Boolean = False

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
    Function funcLoaddesigner() As Integer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME from " & cnAdminDb & "..DESIGNER "
        strSql += "  ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"
    End Function
    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        If ChkItemMode.Text <> "ALL" And ChkItemMode.Text <> "" Then
            strSql += " AND CALTYPE='" & Mid(ChkItemMode.Text, 1, 1) & "'"
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
                .Width = 150
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

            If chkExtraWt.Checked Then
                With .Columns("OEXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If

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
            If chkExtraWt.Checked Then
                With .Columns("REXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
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
            If chkExtraWt.Checked Then
                With .Columns("IEXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
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
            If chkExtraWt.Checked Then
                With .Columns("CEXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
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
    Function funcPartyFiltration() As String
        Dim str As String = Nothing
        Dim tempaccode As String
        str = ""
        If Sepaccodeview = True Then
            tempaccode = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='RPTITEMSTK_ACC_BASED'")
            If tempaccode <> "" Then
                If ChkSepaccode.Checked Then
                    str += " AND I.ACCODE IN(" & GetQryString(tempaccode, ",") & ")"
                Else
                    str += " AND I.ACCODE NOT IN( " & GetQryString(tempaccode, ",") & ")"
                End If
            End If
        End If
        Return str
    End Function


    Function funcItemFiltration() As String
        Dim str As String = Nothing
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where METALID in (select Metalid from " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If cmbCategory.Text <> "ALL" Then
            str += " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where CatCode = (select CatCode from " & cnAdminDb & "..Category where CatName = '" & cmbCategory.Text & "'))"
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
    Public Function GetSelectedCoundergrpid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
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
    Public Function GetSelectedsubitemgrpid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT SGROUPID FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
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

    Function funcApproval_MODE1() As String
        Dim str As String = ""
        If chkSeperateColumnApproval.Checked Then
        ElseIf chkWithApproval.Checked Then
        ElseIf chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
            str += " AND ISNULL(APPROVAL,'') <> 'A' "
        ElseIf chkOnlyApproval.Checked Then
            str += " AND ISNULL(APPROVAL,'') = 'A' "
        End If
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
            .Columns("GROUPNAME").Visible = False
            .Columns("PARTICULAR").Width = 150
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
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").Width = 100
            .Columns("ODIAPCS").Width = 70
            .Columns("ODIAWT").Width = 80
            .Columns("OSTNPCS").Width = 70
            .Columns("OSTNCRWT").Width = 80
            .Columns("OSTNGRWT").Width = 80
            .Columns("OVALUE").Width = 100
            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("REXTRAWT").Width = 100
            .Columns("RDIAPCS").Width = 70
            .Columns("RDIAWT").Width = 80
            .Columns("RSTNPCS").Width = 70
            .Columns("RSTNCRWT").Width = 80
            .Columns("RSTNGRWT").Width = 80
            .Columns("RVALUE").Width = 100
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").Width = 100
            .Columns("IDIAPCS").Width = 70
            .Columns("IDIAWT").Width = 80
            .Columns("ISTNPCS").Width = 70
            .Columns("ISTNCRWT").Width = 80
            .Columns("ISTNGRWT").Width = 80
            .Columns("IVALUE").Width = 100
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS").Width = 70
                .Columns("ARGRSWT").Width = 100
                .Columns("ARNETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("AREXTRAWT").Width = 100
                .Columns("ARDIAPCS").Width = 70
                .Columns("ARDIAWT").Width = 80
                .Columns("ARSTNPCS").Width = 70
                .Columns("ARSTNCRWT").Width = 80
                .Columns("ARSTNGRWT").Width = 80
                .Columns("ARVALUE").Width = 100
                .Columns("ARDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

                .Columns("AIPCS").Width = 70
                .Columns("AIGRSWT").Width = 100
                .Columns("AINETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("AIEXTRAWT").Width = 100
                .Columns("AIDIAPCS").Width = 70
                .Columns("AIDIAWT").Width = 80
                .Columns("AISTNPCS").Width = 70
                .Columns("AISTNCRWT").Width = 80
                .Columns("AISTNGRWT").Width = 80
                .Columns("AIVALUE").Width = 100
                .Columns("AIDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").Width = 100
            .Columns("CDIAPCS").Width = 70
            .Columns("CDIAWT").Width = 80
            .Columns("CSTNPCS").Width = 70
            .Columns("CSTNCRWT").Width = 80
            .Columns("CSTNGRWT").Width = 80
            .Columns("CVALUE").Width = 100
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "PCS" Else .Columns("OPCS").HeaderText = "TAG"
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ONETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("ODIAPCS").HeaderText = "DIAPCS"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSTNPCS").HeaderText = "STNPCS"
            .Columns("OSTNCRWT").HeaderText = "CARAT"
            .Columns("OSTNGRWT").HeaderText = "GRAM"
            .Columns("OVALUE").HeaderText = "VALUE"
            .Columns("OSALVALUE").HeaderText = "SALVALUE"


            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "PCS" Else .Columns("RPCS").HeaderText = "TAG"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("REXTRAWT").HeaderText = "EXTRAWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNCRWT").HeaderText = "CARAT"
            .Columns("RSTNGRWT").HeaderText = "GRAM"
            .Columns("RVALUE").HeaderText = "VALUE"
            .Columns("RSALVALUE").HeaderText = "SALVALUE"

            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "PCS" Else .Columns("IPCS").HeaderText = "TAG"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNCRWT").HeaderText = "CARAT"
            .Columns("ISTNGRWT").HeaderText = "GRAM"
            .Columns("IVALUE").HeaderText = "VALUE"
            .Columns("ISALVALUE").HeaderText = "SALVALUE"
            If chkSeperateColumnApproval.Checked Then
                If Not chkOnlyTag.Checked Then .Columns("ARPCS").HeaderText = "PCS" Else .Columns("ARPCS").HeaderText = "TAG"
                .Columns("ARGRSWT").HeaderText = "GRSWT"
                .Columns("ARNETWT").HeaderText = "NETWT"
                If chkExtraWt.Checked Then .Columns("AREXTRAWT").HeaderText = "EXTRAWT"
                .Columns("ARDIAPCS").HeaderText = "DIAPCS"
                .Columns("ARDIAWT").HeaderText = "DIAWT"
                .Columns("ARSTNPCS").HeaderText = "STNPCS"
                .Columns("ARSTNCRWT").HeaderText = "CARAT"
                .Columns("ARSTNGRWT").HeaderText = "GRAM"
                .Columns("ARVALUE").HeaderText = "VALUE"
                .Columns("ARSALVALUE").HeaderText = "SALVALUE"
                If Not chkOnlyTag.Checked Then .Columns("AIPCS").HeaderText = "PCS" Else .Columns("AIPCS").HeaderText = "TAG"
                .Columns("AIGRSWT").HeaderText = "GRSWT"
                .Columns("AINETWT").HeaderText = "NETWT"
                If chkExtraWt.Checked Then .Columns("AIEXTRAWT").HeaderText = "EXTRAWT"
                .Columns("AIDIAPCS").HeaderText = "DIAPCS"
                .Columns("AIDIAWT").HeaderText = "DIAWT"
                .Columns("AISTNPCS").HeaderText = "STNPCS"
                .Columns("AISTNCRWT").HeaderText = "CARAT"
                .Columns("AISTNGRWT").HeaderText = "GRAM"
                .Columns("AIVALUE").HeaderText = "VALUE"
                .Columns("AISALVALUE").HeaderText = "SALVALUE"
            End If

            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "PCS" Else .Columns("CPCS").HeaderText = "TAG"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNCRWT").HeaderText = "CARAT"
            .Columns("CSTNGRWT").HeaderText = "GRAM"
            .Columns("CVALUE").HeaderText = "VALUE"
            .Columns("CSALVALUE").HeaderText = "SALVALUE"
            ''BACKCOLOR
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSALVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISALVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
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

            .Columns("OSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("OSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("ARSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("CSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("ONETWT").Visible = chkNetWt.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARNETWT").Visible = chkNetWt.Checked
                .Columns("AINETWT").Visible = chkNetWt.Checked
            End If
            .Columns("CNETWT").Visible = chkNetWt.Checked
            .Columns("OVALUE").Visible = chkStoneVal.Checked
            .Columns("RVALUE").Visible = chkStoneVal.Checked
            .Columns("IVALUE").Visible = chkStoneVal.Checked
            .Columns("OSALVALUE").Visible = chkWithValue.Checked
            .Columns("RSALVALUE").Visible = chkWithValue.Checked
            .Columns("ISALVALUE").Visible = chkWithValue.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARVALUE").Visible = chkStoneVal.Checked
                .Columns("AIVALUE").Visible = chkStoneVal.Checked
                .Columns("ARSALVALUE").Visible = chkWithValue.Checked
                .Columns("AISALVALUE").Visible = chkWithValue.Checked
            End If
            .Columns("CVALUE").Visible = chkStoneVal.Checked
            .Columns("CSALVALUE").Visible = chkWithValue.Checked
            .Columns("RATE").Visible = chkWithRate.Checked
        End With
    End Sub
    Private Sub GridStyle1()
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
            If ChkTagWt.Checked = False And ChkCoverWt.Checked = False Then
                If .Columns.Contains("OTOTWT") Then .Columns("OTOTWT").Visible = False
                If .Columns.Contains("RTOTWT") Then .Columns("RTOTWT").Visible = False
                If .Columns.Contains("ITOTWT") Then .Columns("ITOTWT").Visible = False
                If .Columns.Contains("CTOTWT") Then .Columns("CTOTWT").Visible = False
                If .Columns.Contains("OBOXWT") Then .Columns("OBOXWT").Visible = False
                If .Columns.Contains("RBOXWT") Then .Columns("RBOXWT").Visible = False
                If .Columns.Contains("IBOXWT") Then .Columns("IBOXWT").Visible = False
                If .Columns.Contains("CBOXWT") Then .Columns("CBOXWT").Visible = False
            End If
            .Columns("KEYNO").Visible = False
            If .Columns.Contains("GROUPNAME") Then .Columns("GROUPNAME").Visible = False
            .Columns("PARTICULAR").Width = 150
            .Columns("PARTICULAR").Visible = True
            If .Columns.Contains("COUNTER") Then .Columns("COUNTER").Visible = False
            If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
            If .Columns.Contains("COSTCENTRE") Then .Columns("COSTCENTRE").Visible = False
            If .Columns.Contains("TITEM") Then .Columns("TITEM").Visible = False
            If .Columns.Contains("ITEM") Then .Columns("ITEM").Visible = False
            If .Columns.Contains("SUBITEM") Then .Columns("SUBITEM").Visible = False
            If .Columns.Contains("STYLENO") Then .Columns("STYLENO").Visible = chkStyleNo.Checked
            If .Columns.Contains("STYLENO") Then .Columns("STYLENO").Width = 80
            If .Columns.Contains("STONE") Then .Columns("STONE").Visible = False
            If .Columns.Contains("CGROUP") Then .Columns("CGROUP").Visible = False
            If .Columns.Contains("TCGROUP") Then .Columns("TCGROUP").Visible = False
            If .Columns.Contains("SGROUP") Then .Columns("SGROUP").Visible = False
            If ChkWithTag.Checked Then .Columns("OTAGS").Width = 70
            ' If ChkWithTag.Checked = False And chkSummary.Checked = False Then .Columns("OTAGS").Visible = False
            If ChkWithTag.Checked = False Then
                If .Columns.Contains("OTAGS") Then .Columns("OTAGS").Visible = False
            End If
            '.Columns("OTAGS").Visible = False
            .Columns("OPCS").Width = 70
            .Columns("OGRSWT").Width = 100
            .Columns("ONETWT").Width = 100
            If chkExtraWt.Checked And .Columns.Contains("OEXTRAWT") Then .Columns("OEXTRAWT").Width = 100
            .Columns("ODIAPCS").Width = 70
            .Columns("ODIAWT").Width = 80
            .Columns("OSTNPCS").Width = 70
            .Columns("OSTNCRWT").Width = 80
            .Columns("OSTNGRWT").Width = 80
            .Columns("OVALUE").Width = 100
            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If .Columns.Contains("APPOTAGS") Then .Columns("APPOTAGS").Visible = False
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then .Columns("APPOTAGS").Width = 70
                If ChkWithTag.Checked = False Then .Columns("APPOTAGS").Visible = False Else .Columns("APPOTAGS").Visible = True
                .Columns("APPOPCS").Width = 70
                .Columns("APPOGRSWT").Width = 100
                .Columns("APPONETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("APPOEXTRAWT").Width = 100
                .Columns("APPODIAPCS").Width = 70
                .Columns("APPODIAWT").Width = 80
                .Columns("APPOSTNPCS").Width = 70
                .Columns("APPOSTNCRWT").Width = 80
                .Columns("APPOSTNGRWT").Width = 80
                .Columns("APPOVALUE").Width = 100
                .Columns("APPODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            If ChkWithTag.Checked Then .Columns("RTAGS").Width = 70
            If ChkWithTag.Checked = False Then .Columns("RTAGS").Visible = False
            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("REXTRAWT").Width = 100
            .Columns("RDIAPCS").Width = 70
            .Columns("RDIAWT").Width = 80
            .Columns("RSTNPCS").Width = 70
            .Columns("RSTNCRWT").Width = 80
            .Columns("RSTNGRWT").Width = 80
            .Columns("RVALUE").Width = 100
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If .Columns.Contains("ARRTAGS") Then .Columns("ARRTAGS").Visible = False
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then .Columns("ARRTAGS").Width = 70
                If ChkWithTag.Checked = False Then .Columns("ARRTAGS").Visible = False Else .Columns("ARRTAGS").Visible = True
                .Columns("ARRPCS").Width = 70
                .Columns("ARRGRSWT").Width = 100
                .Columns("ARRNETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("ARREXTRAWT").Width = 100
                .Columns("ARRDIAPCS").Width = 70
                .Columns("ARRDIAWT").Width = 80
                .Columns("ARRSTNPCS").Width = 70
                .Columns("ARRSTNCRWT").Width = 80
                .Columns("ARRSTNGRWT").Width = 80
                .Columns("ARRVALUE").Width = 100
                .Columns("ARRDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            If ChkWithTag.Checked Then .Columns("ITAGS").Width = 70
            If ChkWithTag.Checked = False Then .Columns("ITAGS").Visible = False
            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").Width = 100
            .Columns("IDIAPCS").Width = 70
            .Columns("IDIAWT").Width = 80
            .Columns("ISTNPCS").Width = 70
            .Columns("ISTNCRWT").Width = 80
            .Columns("ISTNGRWT").Width = 80
            .Columns("IVALUE").Width = 100
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If .Columns.Contains("AIITAGS") Then .Columns("AIITAGS").Visible = False

            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then .Columns("AIITAGS").Width = 70
                If ChkWithTag.Checked = False Then .Columns("AIITAGS").Visible = False Else .Columns("AIITAGS").Visible = True
                .Columns("AIIPCS").Width = 70
                .Columns("AIIGRSWT").Width = 100
                .Columns("AIINETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("AIIEXTRAWT").Width = 100
                .Columns("AIIDIAPCS").Width = 70
                .Columns("AIIDIAWT").Width = 80
                .Columns("AIISTNPCS").Width = 70
                .Columns("AIISTNCRWT").Width = 80
                .Columns("AIISTNGRWT").Width = 80
                .Columns("AIIVALUE").Width = 100
                .Columns("AIIDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            If ChkWithTag.Checked Then .Columns("CTAGS").Width = 70
            If ChkWithTag.Checked = False Then .Columns("CTAGS").Visible = False
            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").Width = 100
            .Columns("CDIAPCS").Width = 70
            .Columns("CDIAWT").Width = 80
            .Columns("CSTNPCS").Width = 70
            .Columns("CSTNCRWT").Width = 80
            .Columns("CSTNGRWT").Width = 80
            .Columns("CVALUE").Width = 100
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If .Columns.Contains("APPCTAGS") Then .Columns("APPCTAGS").Visible = False
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then .Columns("APPCTAGS").Width = 70
                If ChkWithTag.Checked = False Then .Columns("APPCTAGS").Visible = False Else .Columns("APPCTAGS").Visible = True
                .Columns("APPCPCS").Width = 70
                .Columns("APPCGRSWT").Width = 100
                .Columns("APPCNETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("APPCEXTRAWT").Width = 100
                If ChkWithTag.Checked Then .Columns("APPCDIAPCS").Width = 70
                .Columns("APPCDIAWT").Width = 80
                .Columns("APPCSTNPCS").Width = 70
                .Columns("APPCSTNCRWT").Width = 80
                .Columns("APPCSTNGRWT").Width = 80
                .Columns("APPCVALUE").Width = 100
                .Columns("APPCDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            If chkBoffStockOnly.Checked = False Then
                'FOR PENDING TRANSFER
                If ChkWithTag.Checked Then
                    .Columns("PTAGS").Width = 70
                Else
                    .Columns("PTAGS").Visible = False
                End If
                .Columns("PPCS").Width = 70
                .Columns("PGRSWT").Width = 100
                .Columns("PNETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("PEXTRAWT").Width = 100
                .Columns("PDIAPCS").Width = 70
                .Columns("PDIAWT").Width = 80
                .Columns("PSTNPCS").Width = 70
                .Columns("PSTNCRWT").Width = 80
                .Columns("PSTNGRWT").Width = 80
                .Columns("PVALUE").Width = 100
                .Columns("PDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If

            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "PCS" Else .Columns("OPCS").HeaderText = "TAG"
            If ChkWithTag.Checked Then
                .Columns("OTAGS").HeaderText = "NO.TAG"
            End If
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ONETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").HeaderText = "EXTRAWT"
            If ChkTagWt.Checked Then .Columns("OTAGWT").HeaderText = "TAGWT"
            If ChkCoverWt.Checked Then .Columns("OCOVERWT").HeaderText = "COVERWT"
            If .Columns.Contains("OTOTWT") Then .Columns("OTOTWT").HeaderText = "TOTALWT"
            If .Columns.Contains("RTOTWT") Then .Columns("RTOTWT").HeaderText = "TOTALWT"
            If .Columns.Contains("ITOTWT") Then .Columns("ITOTWT").HeaderText = "TOTALWT"
            If .Columns.Contains("CTOTWT") Then .Columns("CTOTWT").HeaderText = "TOTALWT"
            If .Columns.Contains("OBOXWT") Then .Columns("OBOXWT").HeaderText = "BOXWT"
            If .Columns.Contains("RBOXWT") Then .Columns("RBOXWT").HeaderText = "BOXWT"
            If .Columns.Contains("IBOXWT") Then .Columns("IBOXWT").HeaderText = "BOXWT"
            If .Columns.Contains("CBOXWT") Then .Columns("CBOXWT").HeaderText = "BOXWT"
            .Columns("ODIAPCS").HeaderText = "DIAPCS"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSTNPCS").HeaderText = "STNPCS"
            .Columns("OSTNCRWT").HeaderText = "CARAT"
            .Columns("OSTNGRWT").HeaderText = "GRAM"
            .Columns("OVALUE").HeaderText = "VALUE"
            .Columns("OSALVALUE").HeaderText = "SALVALUE"


            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "PCS" Else .Columns("RPCS").HeaderText = "TAG"
            If ChkWithTag.Checked Then .Columns("RTAGS").HeaderText = "NO.TAG"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("REXTRAWT").HeaderText = "EXTRAWT"
            If ChkTagWt.Checked Then .Columns("RTAGWT").HeaderText = "TAGWT"
            If ChkCoverWt.Checked Then .Columns("RCOVERWT").HeaderText = "COVERWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNCRWT").HeaderText = "CARAT"
            .Columns("RSTNGRWT").HeaderText = "GRAM"
            .Columns("RVALUE").HeaderText = "VALUE"
            .Columns("RSALVALUE").HeaderText = "SALVALUE"

            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "PCS" Else .Columns("IPCS").HeaderText = "TAG"
            If ChkWithTag.Checked Then .Columns("ITAGS").HeaderText = "NO.TAG"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").HeaderText = "EXTRAWT"
            If ChkTagWt.Checked Then .Columns("ITAGWT").HeaderText = "TAGWT"
            If ChkCoverWt.Checked Then .Columns("ICOVERWT").HeaderText = "COVERWT"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNCRWT").HeaderText = "CARAT"
            .Columns("ISTNGRWT").HeaderText = "GRAM"
            .Columns("IVALUE").HeaderText = "VALUE"
            .Columns("ISALVALUE").HeaderText = "SALVALUE"


            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "PCS" Else .Columns("CPCS").HeaderText = "TAG"
            If ChkWithTag.Checked Then .Columns("CTAGS").HeaderText = "NO.TAG"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").HeaderText = "EXTRAWT"
            If ChkTagWt.Checked Then .Columns("CTAGWT").HeaderText = "TAGWT"
            If ChkCoverWt.Checked Then .Columns("CCOVERWT").HeaderText = "COVERWT"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNCRWT").HeaderText = "CARAT"
            .Columns("CSTNGRWT").HeaderText = "GRAM"
            .Columns("CVALUE").HeaderText = "VALUE"
            .Columns("CSALVALUE").HeaderText = "SALVALUE"

            'FOR PENDING TRANSFER
            If Not chkOnlyTag.Checked Then .Columns("PPCS").HeaderText = "PCS" Else .Columns("PPCS").HeaderText = "TAG"
            .Columns("PGRSWT").HeaderText = "GRSWT"
            .Columns("PNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("PEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("PDIAPCS").HeaderText = "DIAPCS"
            .Columns("PDIAWT").HeaderText = "DIAWT"
            .Columns("PSTNPCS").HeaderText = "STNPCS"
            .Columns("PSTNCRWT").HeaderText = "CARAT"
            .Columns("PSTNGRWT").HeaderText = "GRAM"
            .Columns("PVALUE").HeaderText = "VALUE"
            .Columns("PSALVALUE").HeaderText = "SALVALUE"

            ''BACKCOLOR
            If ChkWithTag.Checked Then .Columns("OTAGS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSALVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            If ChkWithTag.Checked Then .Columns("ITAGS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISALVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            If ChkWithTag.Checked Then .Columns("PTAGS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PGRSWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PNETWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            If chkExtraWt.Checked Then .Columns("PEXTRAWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PDIAPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PDIAWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PSTNPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PSTNCRWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PSTNGRWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PVALUE").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("PSALVALUE").DefaultCellStyle.BackColor = Color.LavenderBlush


            ''VISIBLE

            .Columns("OGRSWT").Visible = chkGrsWt.Checked
            .Columns("RGRSWT").Visible = chkGrsWt.Checked
            .Columns("IGRSWT").Visible = chkGrsWt.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRGRSWT").Visible = chkGrsWt.Checked
                .Columns("AIIGRSWT").Visible = chkGrsWt.Checked
            End If
            .Columns("CGRSWT").Visible = chkGrsWt.Checked

            .Columns("ODIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("RDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("IDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
                .Columns("AIIDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            End If
            .Columns("CDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked

            .Columns("ODIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("RDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            .Columns("IDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
                .Columns("AIIDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked
            End If
            .Columns("CDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked

            .Columns("OSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AIISTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("OSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("OSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("ARRSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AIISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AIISTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("CSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("ONETWT").Visible = chkNetWt.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked
            'If chkSeperateColumnApproval.Checked Then
            '    .Columns("ARRNETWT").Visible = chkNetWt.Checked
            '    .Columns("AIINETWT").Visible = chkNetWt.Checked
            'End If
            .Columns("CNETWT").Visible = chkNetWt.Checked
            .Columns("OVALUE").Visible = chkStoneVal.Checked
            .Columns("RVALUE").Visible = chkStoneVal.Checked
            .Columns("IVALUE").Visible = chkStoneVal.Checked
            .Columns("OSALVALUE").Visible = chkWithValue.Checked
            .Columns("RSALVALUE").Visible = chkWithValue.Checked
            .Columns("ISALVALUE").Visible = chkWithValue.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRVALUE").Visible = chkStoneVal.Checked
                .Columns("AIIVALUE").Visible = chkStoneVal.Checked
                .Columns("ARRSALVALUE").Visible = chkWithValue.Checked
                .Columns("AIISALVALUE").Visible = chkWithValue.Checked
            End If
            .Columns("CVALUE").Visible = chkStoneVal.Checked
            .Columns("CSALVALUE").Visible = chkWithValue.Checked
            If chkSummary.Checked = False Then .Columns("RATE").Visible = chkWithRate.Checked

            If ChkWithTag.Checked Then .Columns("PTAGS").Visible = ChkPendingStk.Checked
            .Columns("PPCS").Visible = ChkPendingStk.Checked
            .Columns("PGRSWT").Visible = chkGrsWt.Checked And ChkPendingStk.Checked
            .Columns("PDIAPCS").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked And ChkPendingStk.Checked
            .Columns("PDIAWT").Visible = rbtDiaStnByColumn.Checked And chkDiamond.Checked And ChkPendingStk.Checked
            .Columns("PSTNPCS").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked And ChkPendingStk.Checked
            .Columns("PSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked And ChkPendingStk.Checked
            .Columns("PSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked And ChkPendingStk.Checked
            .Columns("PNETWT").Visible = chkNetWt.Checked And ChkPendingStk.Checked
            .Columns("PVALUE").Visible = chkStoneVal.Checked And ChkPendingStk.Checked
            .Columns("PSALVALUE").Visible = chkWithValue.Checked And ChkPendingStk.Checked
        End With
    End Sub
    Private Sub GridStyleDetail()
        FormatGridColumns(gridviewDetail, False, , , False)
        gridviewDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridviewDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewDetail.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridviewDetail
            .Columns("KEYNO").Visible = False
            .Columns("PARTICULAR").Width = 250
            .Columns("PARTICULAR").Visible = True
            .Columns("TITEM").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("STONE").Visible = False
            .Columns("ODIAPCS").Width = 90
            .Columns("ODIAWT").Width = 90
            .Columns("OSTNPCS").Width = 90
            .Columns("OSTNWT").Width = 90
            .Columns("OVALUE").Width = 90
            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If chkSeperateColumnApproval.Checked Then
                .Columns("APPODIAPCS").Width = 90
                .Columns("APPODIAWT").Width = 90
                .Columns("APPOSTNPCS").Width = 90
                .Columns("APPOSTNWT").Width = 90
                .Columns("APPOVALUE").Width = 90
                .Columns("APPODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            .Columns("RDIAPCS").Width = 90
            .Columns("RDIAWT").Width = 90
            .Columns("RSTNPCS").Width = 90
            .Columns("RSTNWT").Width = 90
            .Columns("RVALUE").Width = 90
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRDIAPCS").Width = 90
                .Columns("ARRDIAWT").Width = 90
                .Columns("ARRSTNPCS").Width = 90
                .Columns("ARRSTNWT").Width = 90
                .Columns("ARRVALUE").Width = 90
                .Columns("ARRDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            .Columns("IDIAPCS").Width = 90
            .Columns("IDIAWT").Width = 90
            .Columns("ISTNPCS").Width = 90
            .Columns("ISTNWT").Width = 90
            .Columns("IVALUE").Width = 90
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            If chkSeperateColumnApproval.Checked Then
                .Columns("AIIDIAPCS").Width = 90
                .Columns("AIIDIAWT").Width = 90
                .Columns("AIISTNPCS").Width = 90
                .Columns("AIISTNWT").Width = 90
                .Columns("AIIVALUE").Width = 90
                .Columns("AIIDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            .Columns("CDIAPCS").Width = 90
            .Columns("CDIAWT").Width = 90
            .Columns("CSTNPCS").Width = 90
            .Columns("CSTNWT").Width = 90
            .Columns("CVALUE").Width = 90
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If chkSeperateColumnApproval.Checked Then
                .Columns("APPCDIAPCS").Width = 90
                .Columns("APPCDIAWT").Width = 90
                .Columns("APPCSTNPCS").Width = 90
                .Columns("APPCSTNWT").Width = 90
                .Columns("APPCVALUE").Width = 90
                .Columns("APPCDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("ODIAPCS").HeaderText = "PCS" Else .Columns("ODIAPCS").HeaderText = "TAG"
            .Columns("ODIAPCS").HeaderText = "DIAPCS"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSTNPCS").HeaderText = "STNPCS"
            .Columns("OSTNWT").HeaderText = "STNWT"
            .Columns("OVALUE").HeaderText = "VALUE"
            If Not chkOnlyTag.Checked Then .Columns("RDIAPCS").HeaderText = "PCS" Else .Columns("RDIAPCS").HeaderText = "TAG"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNWT").HeaderText = "STNWT"
            .Columns("RVALUE").HeaderText = "VALUE"
            If Not chkOnlyTag.Checked Then .Columns("IDIAPCS").HeaderText = "PCS" Else .Columns("IDIAPCS").HeaderText = "TAG"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNWT").HeaderText = "STNWT"
            .Columns("IVALUE").HeaderText = "VALUE"
            If Not chkOnlyTag.Checked Then .Columns("CDIAPCS").HeaderText = "PCS" Else .Columns("CDIAPCS").HeaderText = "TAG"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNWT").HeaderText = "STNWT"
            .Columns("CVALUE").HeaderText = "VALUE"
            ''BACKCOLOR
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").Visible = True
            .Columns("RDIAPCS").Visible = True
            .Columns("IDIAPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRDIAPCS").Visible = True
                .Columns("AIIDIAPCS").Visible = True
            End If
            .Columns("CDIAPCS").Visible = True
            .Columns("ODIAWT").Visible = True
            .Columns("RDIAWT").Visible = True
            .Columns("IDIAWT").Visible = True
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRDIAWT").Visible = True
                .Columns("AIIDIAWT").Visible = True
            End If
            .Columns("CDIAWT").Visible = True
            .Columns("OSTNPCS").Visible = True
            .Columns("RSTNPCS").Visible = True
            .Columns("ISTNPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRSTNPCS").Visible = True
                .Columns("AIISTNPCS").Visible = True
            End If
            .Columns("CSTNPCS").Visible = True

            .Columns("OSTNWT").Visible = True
            .Columns("RSTNWT").Visible = True
            .Columns("ISTNWT").Visible = True
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRSTNWT").Visible = True
                .Columns("AIISTNWT").Visible = True
            End If
            .Columns("CSTNWT").Visible = True
            .Columns("OVALUE").Visible = chkStoneVal.Checked
            .Columns("RVALUE").Visible = chkStoneVal.Checked
            .Columns("IVALUE").Visible = chkStoneVal.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARRVALUE").Visible = chkStoneVal.Checked
                .Columns("AIIVALUE").Visible = chkStoneVal.Checked
            End If
            .Columns("CVALUE").Visible = chkStoneVal.Checked
            .Columns("STNRATE").Visible = chkWithRate.Checked
        End With
    End Sub
    Private Sub GridStyle(ByVal rptflag As Boolean)
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
            .Columns("GROUPNAME").Visible = False
            .Columns("PARTICULAR").Width = 150
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
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").Width = 100
            .Columns("ODIAPCS").Width = 70
            .Columns("ODIAWT").Width = 80
            .Columns("OSTNPCS").Width = 70
            .Columns("OSTNCRWT").Width = 80
            .Columns("OSTNGRWT").Width = 80
            .Columns("OVALUE").Width = 100
            .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("REXTRAWT").Width = 100
            .Columns("RDIAPCS").Width = 70
            .Columns("RDIAWT").Width = 80
            .Columns("RSTNPCS").Width = 70
            .Columns("RSTNCRWT").Width = 80
            .Columns("RSTNGRWT").Width = 80
            .Columns("RVALUE").Width = 100
            .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").Width = 100
            .Columns("IDIAPCS").Width = 70
            .Columns("IDIAWT").Width = 80
            .Columns("ISTNPCS").Width = 70
            .Columns("ISTNCRWT").Width = 80
            .Columns("ISTNGRWT").Width = 80
            .Columns("IVALUE").Width = 100
            .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS").Width = 70
                .Columns("ARGRSWT").Width = 100
                .Columns("ARNETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("AREXTRAWT").Width = 100
                .Columns("ARDIAPCS").Width = 70
                .Columns("ARDIAWT").Width = 80
                .Columns("ARSTNPCS").Width = 70
                .Columns("ARSTNCRWT").Width = 80
                .Columns("ARSTNGRWT").Width = 80
                .Columns("ARVALUE").Width = 100
                .Columns("ARDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

                .Columns("AIPCS").Width = 70
                .Columns("AIGRSWT").Width = 100
                .Columns("AINETWT").Width = 100
                If chkExtraWt.Checked Then .Columns("AIEXTRAWT").Width = 100
                .Columns("AIDIAPCS").Width = 70
                .Columns("AIDIAWT").Width = 80
                .Columns("AISTNPCS").Width = 70
                .Columns("AISTNCRWT").Width = 80
                .Columns("AISTNGRWT").Width = 80
                .Columns("AIVALUE").Width = 100
                .Columns("AIDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            End If

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").Width = 100
            .Columns("CDIAPCS").Width = 70
            .Columns("CDIAWT").Width = 80
            .Columns("CSTNPCS").Width = 70
            .Columns("CSTNCRWT").Width = 80
            .Columns("CSTNGRWT").Width = 80
            .Columns("CVALUE").Width = 100
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "PCS" Else .Columns("OPCS").HeaderText = "TAG"
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ONETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("ODIAPCS").HeaderText = "DIAPCS"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSTNPCS").HeaderText = "STNPCS"
            .Columns("OSTNCRWT").HeaderText = "CARAT"
            .Columns("OSTNGRWT").HeaderText = "GRAM"
            .Columns("OVALUE").HeaderText = "VALUE"


            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "PCS" Else .Columns("RPCS").HeaderText = "TAG"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("REXTRAWT").HeaderText = "EXTRAWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNCRWT").HeaderText = "CARAT"
            .Columns("RSTNGRWT").HeaderText = "GRAM"
            .Columns("RVALUE").HeaderText = "VALUE"


            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "PCS" Else .Columns("IPCS").HeaderText = "TAG"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNCRWT").HeaderText = "CARAT"
            .Columns("ISTNGRWT").HeaderText = "GRAM"
            .Columns("IVALUE").HeaderText = "VALUE"

            If chkSeperateColumnApproval.Checked Then
                If Not chkOnlyTag.Checked Then .Columns("ARPCS").HeaderText = "PCS" Else .Columns("ARPCS").HeaderText = "TAG"
                .Columns("ARGRSWT").HeaderText = "GRSWT"
                .Columns("ARNETWT").HeaderText = "NETWT"
                If chkExtraWt.Checked Then .Columns("AREXTRAWT").HeaderText = "EXTRAWT"
                .Columns("ARDIAPCS").HeaderText = "DIAPCS"
                .Columns("ARDIAWT").HeaderText = "DIAWT"
                .Columns("ARSTNPCS").HeaderText = "STNPCS"
                .Columns("ARSTNCRWT").HeaderText = "CARAT"
                .Columns("ARSTNGRWT").HeaderText = "GRAM"
                .Columns("ARVALUE").HeaderText = "VALUE"

                If Not chkOnlyTag.Checked Then .Columns("AIPCS").HeaderText = "PCS" Else .Columns("AIPCS").HeaderText = "TAG"
                .Columns("AIGRSWT").HeaderText = "GRSWT"
                .Columns("AINETWT").HeaderText = "NETWT"
                If chkExtraWt.Checked Then .Columns("AIEXTRAWT").HeaderText = "EXTRAWT"
                .Columns("AIDIAPCS").HeaderText = "DIAPCS"
                .Columns("AIDIAWT").HeaderText = "DIAWT"
                .Columns("AISTNPCS").HeaderText = "STNPCS"
                .Columns("AISTNCRWT").HeaderText = "CARAT"
                .Columns("AISTNGRWT").HeaderText = "GRAM"
                .Columns("AIVALUE").HeaderText = "VALUE"

            End If

            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "PCS" Else .Columns("CPCS").HeaderText = "TAG"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            If chkExtraWt.Checked Then .Columns("CEXTRAWT").HeaderText = "EXTRAWT"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNCRWT").HeaderText = "CARAT"
            .Columns("CSTNGRWT").HeaderText = "GRAM"
            .Columns("CVALUE").HeaderText = "VALUE"

            ''BACKCOLOR
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("OEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OVALUE").DefaultCellStyle.BackColor = Color.AliceBlue


            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            If chkExtraWt.Checked Then .Columns("IEXTRAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNCRWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNGRWT").DefaultCellStyle.BackColor = Color.AliceBlue
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

            .Columns("OSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("OSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("RSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("ISTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("ARSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
                .Columns("AISTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            End If
            .Columns("CSTNCRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked
            .Columns("CSTNGRWT").Visible = rbtDiaStnByColumn.Checked And chkStone.Checked

            .Columns("ONETWT").Visible = chkNetWt.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARNETWT").Visible = chkNetWt.Checked
                .Columns("AINETWT").Visible = chkNetWt.Checked
            End If
            .Columns("CNETWT").Visible = chkNetWt.Checked
            .Columns("OVALUE").Visible = chkStoneVal.Checked
            .Columns("RVALUE").Visible = chkStoneVal.Checked
            .Columns("IVALUE").Visible = chkStoneVal.Checked

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARVALUE").Visible = chkStoneVal.Checked
                .Columns("AIVALUE").Visible = chkStoneVal.Checked

            End If
            .Columns("CVALUE").Visible = chkStoneVal.Checked

            .Columns("RATE").Visible = chkWithRate.Checked
        End With
    End Sub
    Private Sub SpReport()
        Dim dtx As New DataTable
        Dim ObjGrouper As BrighttechPack.DataGridViewGrouper
        If chkBoffStockOnly.Checked Then
            tabView.Show()
            dtx.Columns.Add("KEYNO", GetType(Integer))
            dtx.Columns("KEYNO").AutoIncrement = True
            dtx.Columns("KEYNO").AutoIncrementSeed = 0
            dtx.Columns("KEYNO").AutoIncrementStep = 1
            dtx.Columns.Add("PARTICULAR")
            dtx.Columns.Add("COUNTER")
            If ChkWithTag.Checked Then dtx.Columns.Add("OTAGS")
            dtx.Columns.Add("OPCS")
            dtx.Columns.Add("OGRSWT")
            dtx.Columns.Add("ONETWT")
            If chkExtraWt.Checked Then dtx.Columns.Add("OEXTRAWT")
            dtx.Columns.Add("ODIAPCS")
            dtx.Columns.Add("ODIAWT")
            dtx.Columns.Add("OSTNPCS")
            ''
            dtx.Columns.Add("OSTNCRWT")
            dtx.Columns.Add("OSTNGRWT")
            dtx.Columns.Add("OVALUE")
            dtx.Columns.Add("OSALVALUE")

            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then dtx.Columns.Add("APPOTAGS")
                dtx.Columns.Add("APPOPCS")
                dtx.Columns.Add("APPOGRSWT")
                dtx.Columns.Add("APPONETWT")
                If chkExtraWt.Checked Then dtx.Columns.Add("APPOEXTRAWT")
                dtx.Columns.Add("APPODIAPCS")
                dtx.Columns.Add("APPODIAWT")
                dtx.Columns.Add("APPOSTNPCS")
                ''
                dtx.Columns.Add("APPOSTNCRWT")
                dtx.Columns.Add("APPOSTNGRWT")
                dtx.Columns.Add("APPOVALUE")
                dtx.Columns.Add("APPOSALVALUE")
            End If
            If ChkWithTag.Checked Then dtx.Columns.Add("RTAGS")
            dtx.Columns.Add("RPCS")
            dtx.Columns.Add("RGRSWT")
            dtx.Columns.Add("RNETWT")
            If chkExtraWt.Checked Then dtx.Columns.Add("REXTRAWT")
            dtx.Columns.Add("RDIAPCS")
            dtx.Columns.Add("RDIAWT")
            dtx.Columns.Add("RSTNPCS")
            ''
            dtx.Columns.Add("RSTNCRWT")
            dtx.Columns.Add("RSTNGRWT")
            dtx.Columns.Add("RVALUE")
            dtx.Columns.Add("RSALVALUE")
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then dtx.Columns.Add("ARRTAGS")
                dtx.Columns.Add("ARRPCS")
                dtx.Columns.Add("ARRGRSWT")
                dtx.Columns.Add("ARRNETWT")
                If chkExtraWt.Checked Then dtx.Columns.Add("ARREXTRAWT")
                dtx.Columns.Add("ARRDIAPCS")
                dtx.Columns.Add("ARRDIAWT")
                dtx.Columns.Add("ARRSTNPCS")
                ''
                dtx.Columns.Add("ARRSTNCRWT")
                dtx.Columns.Add("ARRSTNGRWT")
                dtx.Columns.Add("ARRVALUE")
                dtx.Columns.Add("ARRSALVALUE")
            End If
            If ChkWithTag.Checked Then dtx.Columns.Add("ITAGS")
            dtx.Columns.Add("IPCS")
            dtx.Columns.Add("IGRSWT")
            dtx.Columns.Add("INETWT")
            If chkExtraWt.Checked Then dtx.Columns.Add("IEXTRAWT")
            dtx.Columns.Add("IDIAPCS")
            dtx.Columns.Add("IDIAWT")
            dtx.Columns.Add("ISTNPCS")
            ''
            dtx.Columns.Add("ISTNCRWT")
            dtx.Columns.Add("ISTNGRWT")
            dtx.Columns.Add("IVALUE")
            dtx.Columns.Add("ISALVALUE")
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then dtx.Columns.Add("AIITAGS")
                dtx.Columns.Add("AIIPCS")
                dtx.Columns.Add("AIIGRSWT")
                dtx.Columns.Add("AIINETWT")
                If chkExtraWt.Checked Then dtx.Columns.Add("AIIEXTRAWT")
                dtx.Columns.Add("AIIDIAPCS")
                dtx.Columns.Add("AIIDIAWT")
                dtx.Columns.Add("AIISTNPCS")
                ''
                dtx.Columns.Add("AIISTNCRWT")
                dtx.Columns.Add("AIISTNGRWT")
                dtx.Columns.Add("AIIVALUE")
                dtx.Columns.Add("AIISALVALUE")
            End If
            If ChkWithTag.Checked Then dtx.Columns.Add("CTAGS")
            dtx.Columns.Add("CPCS")
            dtx.Columns.Add("CGRSWT")
            dtx.Columns.Add("CNETWT")
            If chkExtraWt.Checked Then dtx.Columns.Add("CEXTRAWT")
            dtx.Columns.Add("CDIAPCS")
            dtx.Columns.Add("CDIAWT")
            dtx.Columns.Add("CSTNPCS")
            ''
            dtx.Columns.Add("CSTNCRWT")
            dtx.Columns.Add("CSTNGRWT")
            dtx.Columns.Add("CVALUE")
            dtx.Columns.Add("CSALVALUE")
            If chkSeperateColumnApproval.Checked Then
                If ChkWithTag.Checked Then dtx.Columns.Add("APPCTAGS")
                dtx.Columns.Add("APPCPCS")
                dtx.Columns.Add("APPCGRSWT")
                dtx.Columns.Add("APPCNETWT")
                If chkExtraWt.Checked Then dtx.Columns.Add("APPCEXTRAWT")
                dtx.Columns.Add("APPCDIAPCS")
                dtx.Columns.Add("APPCDIAWT")
                dtx.Columns.Add("APPCSTNPCS")
                ''
                dtx.Columns.Add("APPCSTNCRWT")
                dtx.Columns.Add("APPCSTNGRWT")
                dtx.Columns.Add("APPCVALUE")
                dtx.Columns.Add("APPCSALVALUE")
            End If
            If ChkWithTag.Checked Then dtx.Columns.Add("PTAGS")
            dtx.Columns.Add("PPCS")
            dtx.Columns.Add("PGRSWT")
            dtx.Columns.Add("PNETWT")
            If chkExtraWt.Checked Then dtx.Columns.Add("PEXTRAWT")
            dtx.Columns.Add("PDIAPCS")
            dtx.Columns.Add("PDIAWT")
            dtx.Columns.Add("PSTNPCS")
            ''
            dtx.Columns.Add("PSTNCRWT")
            dtx.Columns.Add("PSTNGRWT")
            dtx.Columns.Add("PVALUE")
            dtx.Columns.Add("PSALVALUE")

            dtx.Columns.Add("RATE")
            dtx.Columns.Add("STYLENO")
            dtx.Columns.Add("GROUPNAME")
            ObjGrouper = New BrighttechPack.DataGridViewGrouper(gridView, dtx)
            ObjGrouper.GroupDgv()
            GoTo bkoffstk
        End If
        ResizeToolStripMenuItem.Checked = False
        StkType = ""
        If chkCmbStktype.Text <> "ALL" And chkCmbStktype.Text <> "" Then
            If chkCmbStktype.Text.Contains("MANUFACTURING") Then
                StkType += "M,"
            End If
            If chkCmbStktype.Text.Contains("EXEMPTED") Then
                StkType += "E,"
            End If
            If chkCmbStktype.Text.Contains("TRADING") Then
                StkType += "T,,"
            End If
            If StkType <> "" Then
                StkType = Mid(StkType, 1, StkType.Length - 1)
            End If
        End If
        'strSql = " EXEC " & cnAdminDb & "..SP_RPT_ITEMWISESTOCK"
        strSql = " EXEC " & cnAdminDb & "..RPT_ITEMWISESTOCK"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
        strSql += vbCrLf + " ,@CATCODE ='" & GetSelectedCatCode(cmbCategory, False) & "'"
        If ChkItemMode.Text = "ALL" Then
            strSql += vbCrLf + " ,@ITEMTYPE = ''"
        Else
            strSql += vbCrLf + " ,@ITEMTYPE = '" & GetSelectedItemType(ChkItemMode, False) & "'"
        End If
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
        strSql += vbCrLf + " ,@ITEMTYPEIDS = '" & GetSelecteditemtypeid(chkCmbItemType, False) & "'"
        If chkDesigner.Checked Then
            strSql += vbCrLf + " ,@DESIGNERIDS = '" & GetSelectedDesignerid(chkCmbDesigner, False) & "'"
        Else
            Dim desigr As String = Nothing
            If cmbDesigner.Text = "ALL" Then
                strSql += vbCrLf + " ,@DESIGNERIDS = ''"
            Else
                desigr = GetSqlValue(cn, "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME ='" & cmbDesigner.Text & "'")
                strSql += vbCrLf + " ,@DESIGNERIDS ='" & desigr & "' "
            End If
        End If
        strSql += vbCrLf + " ,@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
        strSql += vbCrLf + " ,@COUNTERIDS = '" & GetSelectedCounderid(chkCmbCounter, False) & "'"
        strSql += vbCrLf + " ,@COSTIDS = '" & GetSelectedCostId(chkCmbCostCentre, False) & "'"

        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='B'"
        ElseIf rbtTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='T'"
        ElseIf rbtNonTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='N'"
        End If
        strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHTR = '" & IIf(Chkwithtr.Checked, "Y", "N") & "'"

        If chkDiamond.Checked And (rbtDiaStnByColumn.Checked Or rbtDiaStnByRow.Checked) Then
            strSql += vbCrLf + " ,@WITHDIA='Y'"
        Else
            strSql += vbCrLf + " ,@WITHDIA='N'"
        End If
        If chkStone.Checked And (rbtDiaStnByColumn.Checked Or rbtDiaStnByRow.Checked) Then
            strSql += vbCrLf + " ,@WITHSTONE='Y'"
        Else
            strSql += vbCrLf + " ,@WITHSTONE='N'"
        End If
        ' If rbtDiaStnByRow.Checked And (chkDiamond.Checked Or chkStone.Checked Or chkmuiltmetal.Checked) Then
        If rbtDiaStnByRow.Checked And (chkDiamond.Checked Or chkStone.Checked Or chkmultimetal.Checked) Then
            strSql += vbCrLf + " ,@DIASTNBYROW = 'Y'"
        Else
            strSql += vbCrLf + " ,@DIASTNBYROW = 'N'"
        End If
        strSql += vbCrLf + " ,@WITHAPP = '" & IIf(chkWithApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SHORTNAME = '" & IIf(chkShortname.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ORDERBY = '" & IIf(chkOrderbyItemId.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@HIDEBACKOFF = '" & IIf(_HideBackOffice = True, "Y", "N") & "'"
        Dim Tagorderreg As String = ""
        If rbtOrder.Checked Then Tagorderreg = "O"
        If rbtRegular.Checked Then Tagorderreg = "G"
        Dim groupby As String = ""

        If ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then
            groupby = groupby + ",CU"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then
            groupby = groupby + ",DE"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then
            groupby = groupby + ",CO"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then
            groupby = groupby + ",IT"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("COUNTERGROUP") Then
            groupby = groupby + ",CG"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("METALNAME") Then
            groupby = groupby + ",ME"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("HALLMARK") Then
            groupby = groupby + ",HM"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("SUBITEMGROUP") Then
            groupby = groupby + ",SG"
        End If
        If Not ChkLstGroupBy.CheckedItems.Contains("METALNAME") And Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") And Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") And Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") And Not ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") And Not ChkLstGroupBy.CheckedItems.Contains("COUNTERGROUP") And Not ChkLstGroupBy.CheckedItems.Contains("HALLMARK") And Not ChkLstGroupBy.CheckedItems.Contains("SUBITEMGROUP") Then
            groupby = ",N"
        End If
        strSql += vbCrLf + " ,@GROUPBY='" & Mid(groupby, 2, Len(groupby)) & "'"
        strSql += vbCrLf + " ,@SEPAPP = '" & IIf(chkSeperateColumnApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHCUMSTK = '" & IIf(chkWithCumulative.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHRATE = '" & IIf(chkWithRate.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ISORDREG = '" & Tagorderreg & "'"
        If chkSummary.Checked = True Then
            strSql += vbCrLf + " ,@SUMMARY = 'Y'"
        Else
            strSql += vbCrLf + " ,@SUMMARY = ''"
        End If
        If chkLoseStSepCol.Checked And (chkDiamond.Checked = False Or chkStone.Checked = False) Then
            strSql += vbCrLf + " ,@STNINGRSWT='Y'"
        Else
            strSql += vbCrLf + " ,@STNINGRSWT='N'"
        End If
        strSql += vbCrLf + " ,@COUNTERGROUP = '" & GetSelectedCoundergrpid(chkcountergroup, False) & "'"
        strSql += vbCrLf + " ,@ONLYAPP ='" & IIf(chkOnlyApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@STKTYPE = '" & StkType & "'"
        strSql += vbCrLf + " ,@ZEROSTK ='" & IIf(chkAll.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@PENDINGSTK ='" & IIf(ChkPendingStk.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@MINUSSTK ='" & IIf(chkWithNegativeStock.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ORDERBYIDNAME ='" & IIf(rbtItemId.Checked, "I", "N") & "'"
        strSql += vbCrLf + " ,@SPECIFICFORMAT ='" & SPECIFICFORMAT.ToString & "'"
        strSql += vbCrLf + " ,@MUILTCHK ='" & IIf(chkmultimetal.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SUBITEMGROUP = '" & GetSelectedsubitemgrpid(cmbSubItemGroup, False) & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        tabView.Show()
        Dim dss As New DataSet

        da.Fill(dss)
        Dim dt As New DataTable
        If dss.Tables.Contains("Table1") Then
            dt = dss.Tables(1)
        Else
            dt = dss.Tables(0)
        End If

        Dim dtSource As New DataTable
        dtSource = dt.Copy
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtSource)
        grpby = groupby
        ObjGrouper = New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
            'ObjGrouper.pColumns_Group.Add("DISPORDER")
        Next
        If chkWithGroupitem.Checked And chkSummary.Checked = False Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked And chkSummary.Checked = False Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OTAGS")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("OEXTRAWT")
        If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("OTAGWT")
        If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("OCOVERWT")
        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(groupby, 2, Len(groupby)) = "CU" Then
                ObjGrouper.pColumns_Sum.Add("OBOXWT")
                ObjGrouper.pColumns_Sum.Add("OTOTWT")
            End If
        End If
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ''
        ObjGrouper.pColumns_Sum.Add("OSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("OSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")
        ObjGrouper.pColumns_Sum.Add("OSALVALUE")
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("APPOTAGS")
            ObjGrouper.pColumns_Sum.Add("APPOPCS")
            ObjGrouper.pColumns_Sum.Add("APPOGRSWT")
            ObjGrouper.pColumns_Sum.Add("APPONETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPOEXTRAWT")
            If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPOTAGWT")
            If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPOCOVERWT")
            ObjGrouper.pColumns_Sum.Add("APPODIAPCS")
            ObjGrouper.pColumns_Sum.Add("APPODIAWT")
            ObjGrouper.pColumns_Sum.Add("APPOSTNPCS")
            ''
            ObjGrouper.pColumns_Sum.Add("APPOSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("APPOSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("APPOVALUE")
            ObjGrouper.pColumns_Sum.Add("APPOSALVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("RTAGS")
        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("REXTRAWT")
        If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("RTAGWT")
        If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("RCOVERWT")
        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(groupby, 2, Len(groupby)) = "CU" Then
                ObjGrouper.pColumns_Sum.Add("RBOXWT")
                ObjGrouper.pColumns_Sum.Add("RTOTWT")
            End If
        End If
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ''
        ObjGrouper.pColumns_Sum.Add("RSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("RSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("RSALVALUE")
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARRTAGS")
            ObjGrouper.pColumns_Sum.Add("ARRPCS")
            ObjGrouper.pColumns_Sum.Add("ARRGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARRNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("ARREXTRAWT")
            If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("ARRTAGWT")
            If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("ARRCOVERWT")
            ObjGrouper.pColumns_Sum.Add("ARRDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARRDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARRSTNPCS")
            ''
            ObjGrouper.pColumns_Sum.Add("ARRSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("ARRSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("ARRVALUE")
            ObjGrouper.pColumns_Sum.Add("ARRSALVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("ITAGS")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("IEXTRAWT")
        If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("ITAGWT")
        If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("ICOVERWT")
        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(groupby, 2, Len(groupby)) = "CU" Then
                ObjGrouper.pColumns_Sum.Add("IBOXWT")
                ObjGrouper.pColumns_Sum.Add("ITOTWT")
            End If
        End If
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ''
        ObjGrouper.pColumns_Sum.Add("ISTNCRWT")
        ObjGrouper.pColumns_Sum.Add("ISTNGRWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")
        ObjGrouper.pColumns_Sum.Add("ISALVALUE")
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("AIITAGS")
            ObjGrouper.pColumns_Sum.Add("AIIPCS")
            ObjGrouper.pColumns_Sum.Add("AIIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AIINETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIIEXTRAWT")
            If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIITAGWT")
            If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIICOVERWT")
            ObjGrouper.pColumns_Sum.Add("AIIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AIISTNPCS")
            ''
            ObjGrouper.pColumns_Sum.Add("AIISTNCRWT")
            ObjGrouper.pColumns_Sum.Add("AIISTNGRWT")
            ObjGrouper.pColumns_Sum.Add("AIIVALUE")
            ObjGrouper.pColumns_Sum.Add("AIISALVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("CTAGS")
        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("CEXTRAWT")
        If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("CTAGWT")
        If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("CCOVERWT")
        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(groupby, 2, Len(groupby)) = "CU" Then
                ObjGrouper.pColumns_Sum.Add("CBOXWT")
                ObjGrouper.pColumns_Sum.Add("CTOTWT")
            End If
        End If
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ''
        ObjGrouper.pColumns_Sum.Add("CSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("CSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColumns_Sum.Add("CSALVALUE")
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("APPCTAGS")
            ObjGrouper.pColumns_Sum.Add("APPCPCS")
            ObjGrouper.pColumns_Sum.Add("APPCGRSWT")
            ObjGrouper.pColumns_Sum.Add("APPCNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPCEXTRAWT")
            If ChkTagWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPCTAGWT")
            If ChkCoverWt.Checked Then ObjGrouper.pColumns_Sum.Add("APPCCOVERWT")
            ObjGrouper.pColumns_Sum.Add("APPCDIAPCS")
            ObjGrouper.pColumns_Sum.Add("APPCDIAWT")
            ObjGrouper.pColumns_Sum.Add("APPCSTNPCS")
            ''
            ObjGrouper.pColumns_Sum.Add("APPCSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("APPCSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("APPCVALUE")
            ObjGrouper.pColumns_Sum.Add("APPCSALVALUE")
        End If
        'FOR PENDING TRANSFER
        If ChkPendingStk.Checked Then
            ObjGrouper.pColumns_Sum.Add("PTAGS")
            ObjGrouper.pColumns_Sum.Add("PPCS")
            ObjGrouper.pColumns_Sum.Add("PGRSWT")
            ObjGrouper.pColumns_Sum.Add("PNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("PEXTRAWT")
            ObjGrouper.pColumns_Sum.Add("PDIAPCS")
            ObjGrouper.pColumns_Sum.Add("PDIAWT")
            ObjGrouper.pColumns_Sum.Add("PSTNPCS")
            ''
            ObjGrouper.pColumns_Sum.Add("PSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("PSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("PVALUE")
            ObjGrouper.pColumns_Sum.Add("PSALVALUE")
        End If
        ObjGrouper.pColName_Particular = "PARTICULAR"

        'ObjGrouper.pColName_Counter = "Counter"

        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        If chkSummary.Checked = False Then
            If rbtItemName.Checked = False Then
                ObjGrouper.pColumns_Sort = "TITEM,STONE"
            End If
            If chkmultimetal.Checked = True And rbtDiaStnByRow.Checked = True Then
                ObjGrouper.pColumns_Sum_FilterString = "STONE NOT IN ('1')"
            Else
                ObjGrouper.pColumns_Sum_FilterString = "STONE NOT IN ('1')"
            End If
        End If
        If chkSummary.Checked = True And chkDiamond.Checked = True _
            And chkStone.Checked = True And groupby = ",CU" And rbtDiaStnByRow.Checked = True Then
            'ObjGrouper.pColumns_Sum_FilterString = "OTAGS IS NOT NULL "
            ObjGrouper.pColumns_Sum_FilterString = "PARTICULAR NOT IN ('     STUD.STONE','     STUD.DIAMOND') "
        End If
        If rbtItemId.Checked Then
            If dt.Columns.Contains("ITEMID") Then
                ObjGrouper.pColumns_Sort = "ITEMID,ITEM"
                ''Else
                ''    ObjGrouper.pColumns_Sort = "ITEM"
            ElseIf dt.Columns.Contains("ITEM") Then
                ObjGrouper.pColumns_Sort = "ITEM"
            Else
                ObjGrouper.pColumns_Sort = ""
            End If
        Else
            'ObjGrouper.pColumns_Sort = "ITEM"
        End If
        'ObjGrouper.pColumns_Sort = "DISPORDER,COUNTER"
        If chkSummary.Checked Then ObjGrouper.Summary_View = True
        ObjGrouper.pIssSort = False
        ObjGrouper.GroupDgv()

        Dim ind As Integer = gridView.RowCount - 1
        CType(gridView.DataSource, DataTable).Rows.Add()

        If rbtDiaStnByRow.Checked = True And chkSummary.Checked = False Then
            strSql = " SELECT * FROM (SELECT (SELECT TOP 1 CATNAME FROM " & cnAdminDb & " .. CATEGORY C WHERE C.CATCODE = T.CATCODE)  PARTICULAR"
            If ChkWithTag.Checked Then strSql += vbCrLf + ",SUM(OTAGS) OTAGS "
            strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(OTAGWT)OTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OCOVERWT"
            strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"

            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGS) APPOTAGS"
                strSql += vbCrLf + " ,SUM(APPOPCS) APPOPCS,SUM(APPOGRSWT)APPOGRSWT,SUM(APPONETWT) APPONETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPOEXTRAWT)APPOEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGWT)APPOTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPOCOVERWT)APPOCOVERWT"
                strSql += vbCrLf + " ,SUM(APPODIAPCS) APPODIAPCS,SUM(APPODIAWT) APPODIAWT,SUM(APPOSTNPCS) APPOSTNPCS,SUM(APPOSTNCRWT) APPOSTNCRWT,SUM(APPOSTNGRWT) APPOSTNGRWT,SUM(APPOVALUE) APPOVALUE,SUM(APPOSALVALUE) APPOSALVALUE"

            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(RTAGS) RTAGS"
            strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(RTAGWT)RTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(RCOVERWT)RCOVERWT"
            strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGS) ARRTAGS"
                strSql += vbCrLf + " ,SUM(ARRPCS) ARRPCS,SUM(ARRGRSWT)ARRGRSWT,SUM(ARRNETWT)ARRNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(ARREXTRAWT)ARREXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGWT)ARRTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ARRCOVERWT)ARRCOVERWT"
                strSql += vbCrLf + " ,SUM(ARRDIAPCS) ARRDIAPCS,SUM(ARRDIAWT)ARRDIAWT,SUM(ARRSTNPCS) ARRSTNPCS,SUM(ARRSTNCRWT)ARRSTNCRWT,SUM(ARRSTNGRWT)ARRSTNGRWT,SUM(ARRVALUE)ARRVALUE,SUM(ARRSALVALUE)ARRSALVALUE"
            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ITAGS) ITAGS"
            strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ITAGWT)ITAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ICOVERWT)ICOVERWT"
            strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"

            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(AIITAGS) AIITAGS"
                strSql += vbCrLf + " ,SUM(AIIPCS) AIIPCS,SUM(AIIGRSWT)AIIGRSWT,SUM(AIINETWT)AIINETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIIEXTRAWT)AIIEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(AIITAGWT)AIITAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(AIICOVERWT)AIICOVERWT"
                strSql += vbCrLf + " ,SUM(AIIDIAPCS) AIIDIAPCS,SUM(AIIDIAWT)AIIDIAWT,SUM(AIISTNPCS) AIISTNPCS,SUM(AIISTNCRWT)AIISTNCRWT,SUM(AIISTNGRWT)AIISTNGRWT,SUM(AIIVALUE)AIIVALUE,SUM(AIISALVALUE)AIISALVALUE"
            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(CTAGS) CTAGS"
            strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(CTAGWT)CTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(CCOVERWT)CCOVERWT"
            strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGS) APPCTAGS"
                strSql += vbCrLf + " ,SUM(APPCPCS) APPCPCS,SUM(APPCGRSWT)APPCGRSWT,SUM(APPCNETWT)APPCNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPCEXTRAWT)APPCEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGWT)APPCTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPCCOVERWT)APPCCOVERWT"
                strSql += vbCrLf + " ,SUM(APPCDIAPCS) APPCDIAPCS,SUM(APPCDIAWT)APPCDIAWT,SUM(APPCSTNPCS) APPCSTNPCS,SUM(APPCSTNCRWT)APPCSTNCRWT,SUM(APPCSTNGRWT)APPCSTNGRWT,SUM(APPCVALUE)APPCVALUE,SUM(APPCSALVALUE)APPCSALVALUE"
            End If
            'FOR PENDING TRANSFER
            If ChkPendingStk.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(PTAGS) PTAGS"
                strSql += vbCrLf + " ,SUM(PPCS) PPCS,SUM(PGRSWT)PGRSWT,SUM(PNETWT) PNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(PEXTRAWT)PEXTRAWT"
                strSql += vbCrLf + " ,SUM(PDIAPCS) PDIAPCS,SUM(PDIAWT)PDIAWT,SUM(PSTNPCS) PSTNPCS,SUM(PSTNCRWT) PSTNCRWT,SUM(PSTNGRWT) PSTNGRWT,SUM(PVALUE) PVALUE,SUM(PSALVALUE) PSALVALUE"
            End If
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTK T WHERE ISNULL(CATCODE,'')<>'' AND ISNULL(STONE,0) NOT IN ( 2,3) "
            strSql += vbCrLf + "  GROUP BY CATCODE  "
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + " SELECT  'Z REPAIR EXCESS WT  [' + (SELECT TOP 1 CATNAME FROM " & cnAdminDb & " .. CATEGORY C WHERE C.CATCODE = T.CATCODE) + ']'  PARTICULAR"
        Else
            strSql = " SELECT  'Z REPAIR EXCESS WT  [' + (SELECT TOP 1 CATNAME FROM " & cnAdminDb & " .. CATEGORY C WHERE C.CATCODE = T.CATCODE) + ']'  PARTICULAR"
        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + ",SUM(OTAGS) OTAGS "
        strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(OTAGWT)OTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OCOVERWT"
        strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"

        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGS) APPOTAGS"
            strSql += vbCrLf + " ,SUM(APPOPCS) APPOPCS,SUM(APPOGRSWT)APPOGRSWT,SUM(APPONETWT) APPONETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPOEXTRAWT)APPOEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGWT)APPOTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPOCOVERWT)APPOCOVERWT"
            strSql += vbCrLf + " ,SUM(APPODIAPCS) APPODIAPCS,SUM(APPODIAWT) APPODIAWT,SUM(APPOSTNPCS) APPOSTNPCS,SUM(APPOSTNCRWT) APPOSTNCRWT,SUM(APPOSTNGRWT) APPOSTNGRWT,SUM(APPOVALUE) APPOVALUE,SUM(APPOSALVALUE) APPOSALVALUE"

        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(RTAGS) RTAGS"
        strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(RTAGWT)RTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(RCOVERWT)RCOVERWT"
        strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGS) ARRTAGS"
            strSql += vbCrLf + " ,SUM(ARRPCS) ARRPCS,SUM(ARRGRSWT)ARRGRSWT,SUM(ARRNETWT)ARRNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(ARREXTRAWT)ARREXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGWT)ARRTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ARRCOVERWT)ARRCOVERWT"
            strSql += vbCrLf + " ,SUM(ARRDIAPCS) ARRDIAPCS,SUM(ARRDIAWT)ARRDIAWT,SUM(ARRSTNPCS) ARRSTNPCS,SUM(ARRSTNCRWT)ARRSTNCRWT,SUM(ARRSTNGRWT)ARRSTNGRWT,SUM(ARRVALUE)ARRVALUE,SUM(ARRSALVALUE)ARRSALVALUE"
        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ITAGS) ITAGS"
        strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ITAGWT)ITAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ICOVERWT)ICOVERWT"
        strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"

        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(AIITAGS) AIITAGS"
            strSql += vbCrLf + " ,SUM(AIIPCS) AIIPCS,SUM(AIIGRSWT)AIIGRSWT,SUM(AIINETWT)AIINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIIEXTRAWT)AIIEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(AIITAGWT)AIITAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(AIICOVERWT)AIICOVERWT"
            strSql += vbCrLf + " ,SUM(AIIDIAPCS) AIIDIAPCS,SUM(AIIDIAWT)AIIDIAWT,SUM(AIISTNPCS) AIISTNPCS,SUM(AIISTNCRWT)AIISTNCRWT,SUM(AIISTNGRWT)AIISTNGRWT,SUM(AIIVALUE)AIIVALUE,SUM(AIISALVALUE)AIISALVALUE"
        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(CTAGS) CTAGS"
        strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(CTAGWT)CTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(CCOVERWT)CCOVERWT"
        strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGS) APPCTAGS"
            strSql += vbCrLf + " ,SUM(APPCPCS) APPCPCS,SUM(APPCGRSWT)APPCGRSWT,SUM(APPCNETWT)APPCNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPCEXTRAWT)APPCEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGWT)APPCTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPCCOVERWT)APPCCOVERWT"
            strSql += vbCrLf + " ,SUM(APPCDIAPCS) APPCDIAPCS,SUM(APPCDIAWT)APPCDIAWT,SUM(APPCSTNPCS) APPCSTNPCS,SUM(APPCSTNCRWT)APPCSTNCRWT,SUM(APPCSTNGRWT)APPCSTNGRWT,SUM(APPCVALUE)APPCVALUE,SUM(APPCSALVALUE)APPCSALVALUE"
        End If
        'FOR PENDING TRANSFER
        If ChkPendingStk.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(PTAGS) PTAGS"
            strSql += vbCrLf + " ,SUM(PPCS) PPCS,SUM(PGRSWT)PGRSWT,SUM(PNETWT) PNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(PEXTRAWT)PEXTRAWT"
            strSql += vbCrLf + " ,SUM(PDIAPCS) PDIAPCS,SUM(PDIAWT)PDIAWT,SUM(PSTNPCS) PSTNPCS,SUM(PSTNCRWT) PSTNCRWT,SUM(PSTNGRWT) PSTNGRWT,SUM(PVALUE) PVALUE,SUM(PSALVALUE) PSALVALUE"
        End If
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTK T WHERE  T.STONE =2  AND ISNULL(T.CATCODE,'')<>'' GROUP BY T.CATCODE  "

        strSql += vbCrLf + "UNION ALL"

        strSql += vbCrLf + " SELECT  'X REPAIR WEIGHT  [' + (SELECT TOP 1 CATNAME FROM " & cnAdminDb & " .. CATEGORY C WHERE C.CATCODE = T.CATCODE) + ']'  PARTICULAR"
        If ChkWithTag.Checked Then strSql += vbCrLf + ",SUM(OTAGS) OTAGS "
        strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(OTAGWT)OTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OCOVERWT"
        strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"

        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGS) APPOTAGS"
            strSql += vbCrLf + " ,SUM(APPOPCS) APPOPCS,SUM(APPOGRSWT)APPOGRSWT,SUM(APPONETWT) APPONETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPOEXTRAWT)APPOEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGWT)APPOTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPOCOVERWT)APPOCOVERWT"
            strSql += vbCrLf + " ,SUM(APPODIAPCS) APPODIAPCS,SUM(APPODIAWT) APPODIAWT,SUM(APPOSTNPCS) APPOSTNPCS,SUM(APPOSTNCRWT) APPOSTNCRWT,SUM(APPOSTNGRWT) APPOSTNGRWT,SUM(APPOVALUE) APPOVALUE,SUM(APPOSALVALUE) APPOSALVALUE"

        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(RTAGS) RTAGS"
        strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(RTAGWT)RTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(RCOVERWT)RCOVERWT"
        strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGS) ARRTAGS"
            strSql += vbCrLf + " ,SUM(ARRPCS) ARRPCS,SUM(ARRGRSWT)ARRGRSWT,SUM(ARRNETWT)ARRNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(ARREXTRAWT)ARREXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGWT)ARRTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ARRCOVERWT)ARRCOVERWT"
            strSql += vbCrLf + " ,SUM(ARRDIAPCS) ARRDIAPCS,SUM(ARRDIAWT)ARRDIAWT,SUM(ARRSTNPCS) ARRSTNPCS,SUM(ARRSTNCRWT)ARRSTNCRWT,SUM(ARRSTNGRWT)ARRSTNGRWT,SUM(ARRVALUE)ARRVALUE,SUM(ARRSALVALUE)ARRSALVALUE"
        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ITAGS) ITAGS"
        strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ITAGWT)ITAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ICOVERWT)ICOVERWT"
        strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"

        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(AIITAGS) AIITAGS"
            strSql += vbCrLf + " ,SUM(AIIPCS) AIIPCS,SUM(AIIGRSWT)AIIGRSWT,SUM(AIINETWT)AIINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIIEXTRAWT)AIIEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(AIITAGWT)AIITAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(AIICOVERWT)AIICOVERWT"
            strSql += vbCrLf + " ,SUM(AIIDIAPCS) AIIDIAPCS,SUM(AIIDIAWT)AIIDIAWT,SUM(AIISTNPCS) AIISTNPCS,SUM(AIISTNCRWT)AIISTNCRWT,SUM(AIISTNGRWT)AIISTNGRWT,SUM(AIIVALUE)AIIVALUE,SUM(AIISALVALUE)AIISALVALUE"
        End If
        If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(CTAGS) CTAGS"
        strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
        If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(CTAGWT)CTAGWT"
        If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(CCOVERWT)CCOVERWT"
        strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGS) APPCTAGS"
            strSql += vbCrLf + " ,SUM(APPCPCS) APPCPCS,SUM(APPCGRSWT)APPCGRSWT,SUM(APPCNETWT)APPCNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPCEXTRAWT)APPCEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGWT)APPCTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPCCOVERWT)APPCCOVERWT"
            strSql += vbCrLf + " ,SUM(APPCDIAPCS) APPCDIAPCS,SUM(APPCDIAWT)APPCDIAWT,SUM(APPCSTNPCS) APPCSTNPCS,SUM(APPCSTNCRWT)APPCSTNCRWT,SUM(APPCSTNGRWT)APPCSTNGRWT,SUM(APPCVALUE)APPCVALUE,SUM(APPCSALVALUE)APPCSALVALUE"
        End If
        'FOR PENDING TRANSFER
        If ChkPendingStk.Checked Then
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(PTAGS) PTAGS"
            strSql += vbCrLf + " ,SUM(PPCS) PPCS,SUM(PGRSWT)PGRSWT,SUM(PNETWT) PNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(PEXTRAWT)PEXTRAWT"
            strSql += vbCrLf + " ,SUM(PDIAPCS) PDIAPCS,SUM(PDIAWT)PDIAWT,SUM(PSTNPCS) PSTNPCS,SUM(PSTNCRWT) PSTNCRWT,SUM(PSTNGRWT) PSTNGRWT,SUM(PVALUE) PVALUE,SUM(PSALVALUE) PSALVALUE"
        End If
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTK T WHERE  T.STONE =3  AND ISNULL(T.CATCODE,'')<>'' GROUP BY T.CATCODE  "
        If rbtDiaStnByRow.Checked = True And chkSummary.Checked = False Then
            strSql += vbCrLf + "  ) X  ORDER BY PARTICULAR "
        End If
        Dim DTSTUD As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DTSTUD)
        If DTSTUD.Rows.Count > 0 Then
            For cnt As Integer = 0 To DTSTUD.Rows.Count - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                With DTSTUD.Rows(cnt)
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = .Item("PARTICULAR")
                    If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = .Item("OTAGS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("OPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("OGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("ONETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = .Item("OEXTRAWT")
                    If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = .Item("OTAGWT")
                    If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = .Item("OCOVERWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = .Item("ODIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = .Item("ODIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = .Item("OSTNPCS")
                    ''
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("OSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = .Item("OSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = .Item("OVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = .Item("OSALVALUE")
                    If chkAsOnDate.Checked Then
                        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOTAGS").Value = .Item("APPOTAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOPCS").Value = .Item("APPOPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOGRSWT").Value = .Item("APPOGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPONETWT").Value = .Item("APPONETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOEXTRAWT").Value = .Item("APPOEXTRAWT")
                            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOTAGWT").Value = .Item("APPOTAGWT")
                            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOCOVERWT").Value = .Item("APPOCOVERWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPODIAPCS").Value = .Item("APPODIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPODIAWT").Value = .Item("APPODIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNPCS").Value = .Item("APPOSTNPCS")
                            ''
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNCRWT").Value = .Item("APPOSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNGRWT").Value = .Item("APPOSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOVALUE").Value = .Item("APPOVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSALVALUE").Value = .Item("APPOSALVALUE")
                        End If
                    End If
                    If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RTAGS").Value = .Item("RTAGS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RPCS").Value = .Item("RPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RGRSWT").Value = .Item("RGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RNETWT").Value = .Item("RNETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("REXTRAWT").Value = .Item("REXTRAWT")
                    If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RTAGWT").Value = .Item("RTAGWT")
                    If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RCOVERWT").Value = .Item("RCOVERWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RDIAPCS").Value = .Item("RDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RDIAWT").Value = .Item("RDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNPCS").Value = .Item("RSTNPCS")
                    ''
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNCRWT").Value = .Item("RSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNGRWT").Value = .Item("RSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RVALUE").Value = .Item("RVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSALVALUE").Value = .Item("RSALVALUE")
                    If chkSeperateColumnApproval.Checked Then
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRTAGS").Value = .Item("ARRTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRPCS").Value = .Item("ARRPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRGRSWT").Value = .Item("ARRGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRNETWT").Value = .Item("ARRNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARREXTRAWT").Value = .Item("ARREXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRTAGWT").Value = .Item("ARRTAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRCOVERWT").Value = .Item("ARRCOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRDIAPCS").Value = .Item("ARRDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRDIAWT").Value = .Item("ARRDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNPCS").Value = .Item("ARRSTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNCRWT").Value = .Item("ARRSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNGRWT").Value = .Item("ARRSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRVALUE").Value = .Item("ARRVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARRSALVALUE").Value = .Item("ARRSALVALUE")
                    End If
                    If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ITAGS").Value = .Item("ITAGS")
                    gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = .Item("IPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = .Item("IGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = .Item("INETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("IEXTRAWT").Value = .Item("IEXTRAWT")
                    If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ITAGWT").Value = .Item("ITAGWT")
                    If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ICOVERWT").Value = .Item("ICOVERWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("IDIAPCS").Value = .Item("IDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("IDIAWT").Value = .Item("IDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNPCS").Value = .Item("ISTNPCS")

                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNCRWT").Value = .Item("ISTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNGRWT").Value = .Item("ISTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("IVALUE").Value = .Item("IVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISALVALUE").Value = .Item("ISALVALUE")
                    If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIITAGS").Value = .Item("AIITAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIIPCS").Value = .Item("AIIPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIIGRSWT").Value = .Item("AIIGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIINETWT").Value = .Item("AIINETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIIEXTRAWT").Value = .Item("AIIEXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIITAGWT").Value = .Item("AIITAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIICOVERWT").Value = .Item("AIICOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIIDIAPCS").Value = .Item("AIIDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIIDIAWT").Value = .Item("AIIDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIISTNPCS").Value = .Item("AIISTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("AIISTNCRWT").Value = .Item("AIISTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIISTNGRWT").Value = .Item("AIISTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIIVALUE").Value = .Item("AIIVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIISALVALUE").Value = .Item("AIISALVALUE")
                    End If
                    If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CTAGS").Value = .Item("CTAGS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CPCS").Value = .Item("CPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CGRSWT").Value = .Item("CGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CNETWT").Value = .Item("CNETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CEXTRAWT").Value = .Item("CEXTRAWT")
                    If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CTAGWT").Value = .Item("CTAGWT")
                    If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CCOVERWT").Value = .Item("CCOVERWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CDIAPCS").Value = .Item("CDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CDIAWT").Value = .Item("CDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNPCS").Value = .Item("CSTNPCS")

                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNCRWT").Value = .Item("CSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNGRWT").Value = .Item("CSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CVALUE").Value = .Item("CVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSALVALUE").Value = .Item("CSALVALUE")
                    If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCTAGS").Value = .Item("APPCTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCPCS").Value = .Item("APPCPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCGRSWT").Value = .Item("APPCGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCNETWT").Value = .Item("APPCNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCEXTRAWT").Value = .Item("APPCEXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCTAGWT").Value = .Item("APPCTAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCCOVERWT").Value = .Item("APPCCOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCDIAPCS").Value = .Item("APPCDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCDIAWT").Value = .Item("APPCDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNPCS").Value = .Item("APPCSTNPCS")
                        ''
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNCRWT").Value = .Item("APPCSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNGRWT").Value = .Item("APPCSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCVALUE").Value = .Item("APPCVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("APPCSALVALUE").Value = .Item("APPCSALVALUE")
                    End If
                    'FOR PENDING TRANSFER
                    If ChkPendingStk.Checked Then
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("PTAGS").Value = .Item("PTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("PPCS").Value = .Item("PPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("PGRSWT").Value = .Item("PGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("PNETWT").Value = .Item("PNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("PEXTRAWT").Value = .Item("PEXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("PDIAPCS").Value = .Item("PDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("PDIAWT").Value = .Item("PDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("PSTNPCS").Value = .Item("PSTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("PSTNCRWT").Value = .Item("PSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("PSTNGRWT").Value = .Item("PSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("PVALUE").Value = .Item("PVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("PSALVALUE").Value = .Item("PSALVALUE")
                    End If
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                End With
            Next
        End If

        If chkCmbMetal.Text = "ALL" Then
            CType(gridView.DataSource, DataTable).Rows.Add()

            strSql = " SELECT (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  WHERE METALID=T.METALID ) PARTICULAR"
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(OTAGS) OTAGS"
            strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(OTAGWT)OTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(OCOVERWT)OCOVERWT"
            strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGS) APPOTAGS"
                strSql += vbCrLf + " ,SUM(APPOPCS) APPOPCS,SUM(APPOGRSWT)APPOGRSWT,SUM(APPONETWT) APPONETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPOEXTRAWT)APPOEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPOTAGWT)APPOTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPOCOVERWT)APPOCOVERWT"
                strSql += vbCrLf + " ,SUM(APPODIAPCS) APPODIAPCS"
                strSql += vbCrLf + " ,SUM(APPODIAWT) APPODIAWT,SUM(APPOSTNPCS) APPOSTNPCS,SUM(APPOSTNCRWT) APPOSTNCRWT,SUM(APPOSTNGRWT) APPOSTNGRWT,SUM(APPOVALUE) APPOVALUE,SUM(APPOSALVALUE) APPOSALVALUE"
            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(RTAGS) RTAGS"
            strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(RTAGWT)RTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(RCOVERWT)RCOVERWT"
            strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGS) ARRTAGS"
                strSql += vbCrLf + " ,SUM(ARRPCS) ARRPCS,SUM(ARRGRSWT)ARRGRSWT,SUM(ARRNETWT)ARRNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(ARREXTRAWT)ARREXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ARRTAGWT)ARRTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ARRCOVERWT)ARRCOVERWT"
                strSql += vbCrLf + " ,SUM(ARRDIAPCS) ARRDIAPCS,SUM(ARRDIAWT)ARRDIAWT,SUM(ARRSTNPCS) ARRSTNPCS,SUM(ARRSTNCRWT)ARRSTNCRWT,SUM(ARRSTNGRWT)ARRSTNGRWT,SUM(ARRVALUE)ARRVALUE,SUM(ARRSALVALUE)ARRSALVALUE"
            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(ITAGS) ITAGS"
            strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(ITAGWT)ITAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(ICOVERWT)ICOVERWT"
            strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(AIITAGS) AIITAGS"
                strSql += vbCrLf + " ,SUM(AIIPCS) AIIPCS,SUM(AIIGRSWT)AIIGRSWT,SUM(AIINETWT)AIINETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIIEXTRAWT)AIIEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(AIITAGWT)AIITAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(AIICOVERWT)AIICOVERWT"
                strSql += vbCrLf + " ,SUM(AIIDIAPCS) AIIDIAPCS,SUM(AIIDIAWT)AIIDIAWT,SUM(AIISTNPCS) AIISTNPCS,SUM(AIISTNCRWT)AIISTNCRWT,SUM(AIISTNGRWT)AIISTNGRWT,SUM(AIIVALUE)AIIVALUE,SUM(AIISALVALUE)AIISALVALUE"
            End If
            If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(CTAGS) CTAGS"
            strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
            If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(CTAGWT)CTAGWT"
            If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(CCOVERWT)CCOVERWT"
            strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
            If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGS) APPCTAGS"
                strSql += vbCrLf + " ,SUM(APPCPCS) APPCPCS,SUM(APPCGRSWT)APPCGRSWT,SUM(APPCNETWT)APPCNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(APPCEXTRAWT)APPCEXTRAWT"
                If ChkTagWt.Checked Then strSql += vbCrLf + " ,SUM(APPCTAGWT)APPCTAGWT"
                If ChkCoverWt.Checked Then strSql += vbCrLf + " ,SUM(APPCCOVERWT)APPCCOVERWT"
                strSql += vbCrLf + " ,SUM(APPCDIAPCS) APPCDIAPCS,SUM(APPCDIAWT)APPCDIAWT,SUM(APPCSTNPCS) APPCSTNPCS,SUM(APPCSTNCRWT)APPCSTNCRWT,SUM(APPCSTNGRWT)APPCSTNGRWT,SUM(APPCVALUE)APPCVALUE,SUM(APPCSALVALUE)APPCSALVALUE"
            End If
            'FOR PENDING TRANSFER
            If ChkPendingStk.Checked Then
                If ChkWithTag.Checked Then strSql += vbCrLf + " ,SUM(PTAGS) PTAGS"
                strSql += vbCrLf + " ,SUM(PPCS) PPCS,SUM(PGRSWT)PGRSWT,SUM(PNETWT) PNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(PEXTRAWT)PEXTRAWT"
                strSql += vbCrLf + " ,SUM(PDIAPCS) PDIAPCS,SUM(PDIAWT)PDIAWT,SUM(PSTNPCS) PSTNPCS,SUM(PSTNCRWT) PSTNCRWT,SUM(PSTNGRWT) PSTNGRWT,SUM(PVALUE) PVALUE,SUM(PSALVALUE) PSALVALUE"
            End If
            If chkmultimetal.Checked = True And rbtDiaStnByRow.Checked = True Then
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTK T WHERE STONE=0 AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST)"
            Else
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTK T WHERE STONE=0 AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST)"
            End If

            strSql += vbCrLf + "  GROUP BY METALID"
            Dim DTMETALDET As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DTMETALDET)
            If DTMETALDET.Rows.Count > 0 Then
                For cnt As Integer = 0 To DTMETALDET.Rows.Count - 1
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    With DTMETALDET.Rows(cnt)
                        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = .Item("PARTICULAR")
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = .Item("OTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("OPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("OGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("ONETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = .Item("OEXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = .Item("OTAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = .Item("OCOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = .Item("ODIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = .Item("ODIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = .Item("OSTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("OSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = .Item("OSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = .Item("OVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = .Item("OSALVALUE")
                        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOTAGS").Value = .Item("APPOTAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOPCS").Value = .Item("APPOPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOGRSWT").Value = .Item("APPOGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPONETWT").Value = .Item("APPONETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOEXTRAWT").Value = .Item("APPOEXTRAWT")
                            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOTAGWT").Value = .Item("APPOTAGWT")
                            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPOCOVERWT").Value = .Item("APPOCOVERWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPODIAPCS").Value = .Item("APPODIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPODIAWT").Value = .Item("APPODIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNPCS").Value = .Item("APPOSTNPCS")

                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNCRWT").Value = .Item("APPOSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSTNGRWT").Value = .Item("APPOSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOVALUE").Value = .Item("APPOVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPOSALVALUE").Value = .Item("APPOSALVALUE")
                        End If
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RTAGS").Value = .Item("RTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RPCS").Value = .Item("RPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RGRSWT").Value = .Item("RGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RNETWT").Value = .Item("RNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("REXTRAWT").Value = .Item("REXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RTAGWT").Value = .Item("RTAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("RCOVERWT").Value = .Item("RCOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RDIAPCS").Value = .Item("RDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RDIAWT").Value = .Item("RDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNPCS").Value = .Item("RSTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNCRWT").Value = .Item("RSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNGRWT").Value = .Item("RSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RVALUE").Value = .Item("RVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSALVALUE").Value = .Item("RSALVALUE")
                        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRTAGS").Value = .Item("ARRTAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRPCS").Value = .Item("ARRPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRGRSWT").Value = .Item("ARRGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRNETWT").Value = .Item("ARRNETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARREXTRAWT").Value = .Item("ARREXTRAWT")
                            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRTAGWT").Value = .Item("ARRTAGWT")
                            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ARRCOVERWT").Value = .Item("ARRCOVERWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRDIAPCS").Value = .Item("ARRDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRDIAWT").Value = .Item("ARRDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNPCS").Value = .Item("ARRSTNPCS")

                            gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNCRWT").Value = .Item("ARRSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRSTNGRWT").Value = .Item("ARRSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRVALUE").Value = .Item("ARRVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARRSALVALUE").Value = .Item("ARRSALVALUE")
                        End If
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ITAGS").Value = .Item("ITAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = .Item("IPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = .Item("IGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = .Item("INETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("IEXTRAWT").Value = .Item("IEXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ITAGWT").Value = .Item("ITAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("ICOVERWT").Value = .Item("ICOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("IDIAPCS").Value = .Item("IDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IDIAWT").Value = .Item("IDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNPCS").Value = .Item("ISTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNCRWT").Value = .Item("ISTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNGRWT").Value = .Item("ISTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("IVALUE").Value = .Item("IVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISALVALUE").Value = .Item("ISALVALUE")
                        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIITAGS").Value = .Item("AIITAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIIPCS").Value = .Item("AIIPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIIGRSWT").Value = .Item("AIIGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIINETWT").Value = .Item("AIINETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIIEXTRAWT").Value = .Item("AIIEXTRAWT")
                            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIITAGWT").Value = .Item("AIITAGWT")
                            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIICOVERWT").Value = .Item("AIICOVERWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIIDIAPCS").Value = .Item("AIIDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIIDIAWT").Value = .Item("AIIDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIISTNPCS").Value = .Item("AIISTNPCS")

                            gridView.Rows(gridView.RowCount - 1).Cells("AIISTNCRWT").Value = .Item("AIISTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIISTNGRWT").Value = .Item("AIISTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIIVALUE").Value = .Item("AIIVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIISALVALUE").Value = .Item("AIISALVALUE")
                        End If
                        If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CTAGS").Value = .Item("CTAGS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CPCS").Value = .Item("CPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CGRSWT").Value = .Item("CGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CNETWT").Value = .Item("CNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CEXTRAWT").Value = .Item("CEXTRAWT")
                        If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CTAGWT").Value = .Item("CTAGWT")
                        If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CCOVERWT").Value = .Item("CCOVERWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CDIAPCS").Value = .Item("CDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CDIAWT").Value = .Item("CDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNPCS").Value = .Item("CSTNPCS")

                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNCRWT").Value = .Item("CSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNGRWT").Value = .Item("CSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CVALUE").Value = .Item("CVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSALVALUE").Value = .Item("CSALVALUE")
                        If chkSeperateColumnApproval.Checked Or chkOnlyApproval.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCTAGS").Value = .Item("APPCTAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCPCS").Value = .Item("APPCPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCGRSWT").Value = .Item("APPCGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCNETWT").Value = .Item("APPCNETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCEXTRAWT").Value = .Item("APPCEXTRAWT")
                            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCTAGWT").Value = .Item("APPCTAGWT")
                            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("APPCCOVERWT").Value = .Item("APPCCOVERWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCDIAPCS").Value = .Item("APPCDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCDIAWT").Value = .Item("APPCDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNPCS").Value = .Item("APPCSTNPCS")

                            gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNCRWT").Value = .Item("APPCSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCSTNGRWT").Value = .Item("APPCSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCVALUE").Value = .Item("APPCVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("APPCSALVALUE").Value = .Item("APPCSALVALUE")
                        End If
                        'FOR PENDING TRANSFER
                        If ChkPendingStk.Checked Then
                            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("PTAGS").Value = .Item("PTAGS")
                            gridView.Rows(gridView.RowCount - 1).Cells("PPCS").Value = .Item("PPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("PGRSWT").Value = .Item("PGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("PNETWT").Value = .Item("PNETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("PEXTRAWT").Value = .Item("PEXTRAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("PDIAPCS").Value = .Item("PDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("PDIAWT").Value = .Item("PDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("PSTNPCS").Value = .Item("PSTNPCS")

                            gridView.Rows(gridView.RowCount - 1).Cells("PSTNCRWT").Value = .Item("PSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("PSTNGRWT").Value = .Item("PSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("PVALUE").Value = .Item("PVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("PSALVALUE").Value = .Item("PSALVALUE")
                        End If
                        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    End With
                Next
            End If
        End If

        If gridView.Columns.Contains("ITEMTYPE") Then gridView.Columns("ITEMTYPE").Visible = False


        If HideSummary = False Then

            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("OTAGS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("OTAGWT").Value
            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("OCOVERWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("OSALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP OPENING
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP-OPENING"

                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("APPOTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("APPOPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("APPOGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("APPONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("APPOEXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("APPOTAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("APPOCOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("APPODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("APPODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("APPOSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("APPOSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("APPOSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("APPOVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("APPOSALVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
            If chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("APPOTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("APPOPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("APPOGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("APPONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("APPOEXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("APPOTAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("APPOCOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("APPODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("APPODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("APPOSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("APPOSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("APPOSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("APPOVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("APPOSALVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("RTAGS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("RTAGWT").Value
            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("RCOVERWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("RSALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APPROVAL RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP-RECEIPT"
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("ARRTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARRPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARRGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARRNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("ARREXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("ARRTAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("ARRCOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARRDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARRDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARRSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARRSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARRSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARRVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("ARRSALVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
            If chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("ARRTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARRPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARRGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARRNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("ARREXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("ARRTAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("ARRCOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARRDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARRDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARRSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARRSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARRSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARRVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("ARRSALVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("ITAGS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("ITAGWT").Value
            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("ICOVERWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("ISALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APPROVAL ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP-ISSUE"
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("AIITAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AIINETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIIEXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("AIITAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("AIICOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AIISTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AIISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AIISTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("AIISALVALUE").Value
            End If
            If chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("AIITAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AIINETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIIEXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("AIITAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("AIICOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AIISTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AIISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AIISTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("AIISALVALUE").Value
            End If
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            ''CLOSING
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("CTAGS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
            If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("CTAGWT").Value
            If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("CCOVERWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("CSALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP-CLOSING"
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("APPCTAGS").Value
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("APPCTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("APPCPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("APPCGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("APPCNETWT").Value
                'If chkExtraWt.Checked And chkSummary.Checked = False Then gridView.Rows(gridView.RowCount - 1).Cells("OCEXTRAWT").Value = gridView.Rows(ind).Cells("APPCEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("APPCDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("APPCDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("APPCSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("APPCSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("APPCSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("APPCVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("APPCSALVALUE").Value
            End If
            If chkOnlyApproval.Checked Then
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("APPCTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("APPCPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("APPCGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("APPCNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("APPCEXTRAWT").Value
                If ChkTagWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGWT").Value = gridView.Rows(ind).Cells("APPCTAGWT").Value
                If ChkCoverWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OCOVERWT").Value = gridView.Rows(ind).Cells("APPCCOVERWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("APPCDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("APPCDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("APPCSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("APPCSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("APPCSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("APPCVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("APPCSALVALUE").Value
            End If
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            'FOR PENDING TRANSFER
            If ChkPendingStk.Checked Then
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "PENDING TRANSFER"
                If ChkWithTag.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OTAGS").Value = gridView.Rows(ind).Cells("PTAGS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("PPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("PGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("PNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("PEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("PDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("PDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("PSTNPCS").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("PSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("PSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("PVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("PSALVALUE").Value
            End If
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If
bkoffstk:
        If chkBoffStock.Checked Then
            Dim bcds As New DataSet
            Dim bcdt As New DataTable
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_BACKOFFSTK"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
            strSql += vbCrLf + " ,@COSTIDS = '" & GetSelectedCostId(chkCmbCostCentre, False) & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(bcds)
            bcdt = bcds.Tables(0)
            If bcdt.Rows.Count <> 0 Then
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "BACK OFFICE - STOCK"
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
            For ii As Integer = 0 To bcdt.Rows.Count - 1
                CType(gridView.DataSource, DataTable).Rows.Add()

                With bcdt.Rows(ii)
                    Dim mitemname As String = .Item("ITEMNAME").ToString
                    If mitemname = "" Then mitemname = .Item("CATNAME").ToString Else mitemname = "**" & mitemname
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = mitemname
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("PCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("GRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("NETWT")

                End With
            Next
        End If
        'FOR PENDING TRANSFER
        If ChkPendingStk.Checked Then
            Dim psds As New DataSet
            Dim psdt As New DataTable
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_PENDINGSTK"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
            strSql += vbCrLf + " ,@COSTIDS = '" & GetSelectedCostId(chkCmbCostCentre, False) & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(psds)
            psdt = psds.Tables(0)
            If psdt.Rows.Count > 0 Then
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "PENDING TRANSFER"
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

                For ii As Integer = 0 To psdt.Rows.Count - 1
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    With psdt.Rows(ii)
                        Dim mitemname As String = .Item("ITEMNAME").ToString
                        If mitemname = "" Then mitemname = .Item("CATNAME").ToString Else mitemname = "**" & mitemname
                        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = mitemname
                        gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = .Item("PCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = .Item("GRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = .Item("NETWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = .Item("DIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = .Item("DIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = .Item("STNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("STNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("STNGRWT")
                    End With
                Next

                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "TOTAL"
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = psdt.Compute("SUM(PCS)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = psdt.Compute("SUM(GRSWT)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = psdt.Compute("SUM(NETWT)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = psdt.Compute("SUM(DIAPCS)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = psdt.Compute("SUM(DIAWT)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = psdt.Compute("SUM(STNPCS)", "")

                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = psdt.Compute("SUM(STNCRWT)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = psdt.Compute("SUM(STNGRWT)", "")
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
        End If
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
        If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "

        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        lblTitle.Text += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        If StkType <> "" Then lblTitle.Text += " " & chkCmbStktype.Text
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridViewHeaderStyle1()
        GridStyle1()
        gridView.Columns("COLHEAD").Visible = False
        If gridView.Columns.Contains("DISPORDER") Then gridView.Columns("DISPORDER").Visible = False
        If gridView.Columns.Contains("ITEMID") Then gridView.Columns("ITEMID").Visible = False
        If chkSeperateColumnApproval.Checked Then
            If ChkWithTag.Checked Then gridView.Columns("ARRTAGS").Visible = True
            gridView.Columns("ARRPCS").Visible = True
            gridView.Columns("ARRGRSWT").Visible = True
            gridView.Columns("ARRNETWT").Visible = True
            gridView.Columns("APPODIAPCS").Visible = False
            gridView.Columns("APPODIAWT").Visible = False
            gridView.Columns("APPOSTNPCS").Visible = False
            gridView.Columns("APPOSTNCRWT").Visible = False
            gridView.Columns("APPOSTNGRWT").Visible = False
            gridView.Columns("APPOVALUE").Visible = False
            gridView.Columns("APPOSALVALUE").Visible = False
            gridView.Columns("ARRDIAPCS").Visible = False
            gridView.Columns("ARRDIAWT").Visible = False
            gridView.Columns("ARRSTNPCS").Visible = False
            gridView.Columns("ARRVALUE").Visible = False
            gridView.Columns("ARRSALVALUE").Visible = False
            gridView.Columns("AIIDIAPCS").Visible = False
            gridView.Columns("AIIDIAWT").Visible = False
            gridView.Columns("AIISTNPCS").Visible = False
            gridView.Columns("AIIVALUE").Visible = False
            gridView.Columns("AIISALVALUE").Visible = False

            gridView.Columns("APPCDIAPCS").Visible = False
            gridView.Columns("APPCDIAWT").Visible = False
            gridView.Columns("APPCSTNPCS").Visible = False
            gridView.Columns("APPCSTNCRWT").Visible = False
            gridView.Columns("APPCSTNGRWT").Visible = False
            gridView.Columns("APPCVALUE").Visible = False
            gridView.Columns("APPCSALVALUE").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("OEXTRAWT") Then gridView.Columns("OEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("REXTRAWT") Then gridView.Columns("REXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("IEXTRAWT") Then gridView.Columns("IEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("CEXTRAWT") Then gridView.Columns("CEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("PEXTRAWT") Then gridView.Columns("PEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("APPOEXTRAWT") Then gridView.Columns("APPOEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("ARREXTRAWT") Then gridView.Columns("ARREXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("AIIEXTRAWT") Then gridView.Columns("AIIEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then If gridView.Columns.Contains("APPCEXTRAWT") Then gridView.Columns("APPCEXTRAWT").Visible = False

            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("OTAGWT") Then gridView.Columns("OTAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("RTAGWT") Then gridView.Columns("RTAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("ITAGWT") Then gridView.Columns("ITAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("CTAGWT") Then gridView.Columns("CTAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("APPOTAGWT") Then gridView.Columns("APPOTAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("ARRTAGWT") Then gridView.Columns("ARRTAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("AIITAGWT") Then gridView.Columns("AIITAGWT").Visible = False
            If ChkTagWt.Checked = False Then If gridView.Columns.Contains("APPCTAGWT") Then gridView.Columns("APPCTAGWT").Visible = False

            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("OCOVERWT") Then gridView.Columns("OCOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("RCOVERWT") Then gridView.Columns("RCOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("ICOVERWT") Then gridView.Columns("ICOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("CCOVERWT") Then gridView.Columns("CCOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("APPOCOVERWT") Then gridView.Columns("APPOCOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("ARRCOVERWT") Then gridView.Columns("ARRCOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("AIICOVERWT") Then gridView.Columns("AIICOVERWT").Visible = False
            If ChkCoverWt.Checked = False Then If gridView.Columns.Contains("APPCCOVERWT") Then gridView.Columns("APPCCOVERWT").Visible = False

            If chkGrsWt.Checked = False Then gridView.Columns("APPOGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("APPONETWT").Visible = False
            If chkGrsWt.Checked = False Then gridView.Columns("ARRGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("ARRNETWT").Visible = False
            If chkGrsWt.Checked = False Then gridView.Columns("AIIGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("AIINETWT").Visible = False
            If chkGrsWt.Checked = False Then gridView.Columns("APPCGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("APPCNETWT").Visible = False
            If ChkWithTag.Checked Then gridView.Columns("APPOTAGS").HeaderText = "APP-TAGS"

            gridView.Columns("APPOPCS").HeaderText = "APP-PCS"
            gridView.Columns("APPOGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("APPONETWT").HeaderText = "APP-NETWT"
            gridView.Columns("AIITAGS").HeaderText = "APP-TAGS"
            gridView.Columns("AIIPCS").HeaderText = "APP-PCS"
            gridView.Columns("AIIGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("AIINETWT").HeaderText = "APP-NETWT"
            gridView.Columns("ARRTAGS").HeaderText = "APP-TAGS"
            gridView.Columns("ARRPCS").HeaderText = "APP-PCS"
            gridView.Columns("ARRGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("ARRNETWT").HeaderText = "APP-NETWT"

            If ChkWithTag.Checked Then gridView.Columns("APPCTAGS").HeaderText = "APP-TAGS"
            gridView.Columns("APPCPCS").HeaderText = "APP-PCS"
            gridView.Columns("APPCGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("APPCNETWT").HeaderText = "APP-NETWT"

        ElseIf chkOnlyApproval.Checked = True Then
            If chkSeperateColumnApproval.Checked And ChkWithTag.Checked = True Then gridView.Columns("ARRTAGS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridView.Columns("ARRPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridView.Columns("ARRGRSWT").Visible = True
            If chkSeperateColumnApproval.Checked Then gridView.Columns("ARRNETWT").Visible = True

            'OPENING
            If ChkWithTag.Checked = False Then gridView.Columns("OTAGS").Visible = False
            gridView.Columns("OPCS").Visible = False
            gridView.Columns("OGRSWT").Visible = False
            gridView.Columns("ONETWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("OEXTRAWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("OTAGWT") Then gridView.Columns("OTAGWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("OCOVERWT") Then gridView.Columns("OCOVERWT").Visible = False
            gridView.Columns("ODIAPCS").Visible = False
            gridView.Columns("ODIAWT").Visible = False
            gridView.Columns("OSTNPCS").Visible = False
            gridView.Columns("OSTNCRWT").Visible = False
            gridView.Columns("OSTNGRWT").Visible = False
            gridView.Columns("OVALUE").Visible = False
            gridView.Columns("OSALVALUE").Visible = False
            'RECEIPT
            If ChkWithTag.Checked = False Then gridView.Columns("RTAGS").Visible = False
            gridView.Columns("RPCS").Visible = False
            gridView.Columns("RGRSWT").Visible = False
            gridView.Columns("RNETWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("REXTRAWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("RTAGWT") Then gridView.Columns("RTAGWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("RCOVERWT") Then gridView.Columns("RCOVERWT").Visible = False
            gridView.Columns("RDIAPCS").Visible = False
            gridView.Columns("RDIAWT").Visible = False
            gridView.Columns("RSTNPCS").Visible = False
            gridView.Columns("RSTNCRWT").Visible = False
            gridView.Columns("RSTNGRWT").Visible = False
            gridView.Columns("RVALUE").Visible = False
            gridView.Columns("RSALVALUE").Visible = False
            'ISSUE
            If ChkWithTag.Checked = False Then gridView.Columns("ITAGS").Visible = False
            gridView.Columns("IPCS").Visible = False
            gridView.Columns("IGRSWT").Visible = False
            gridView.Columns("INETWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("IEXTRAWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("ITAGWT") Then gridView.Columns("ITAGWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("ICOVERWT") Then gridView.Columns("ICOVERWT").Visible = False
            gridView.Columns("IDIAPCS").Visible = False
            gridView.Columns("IDIAWT").Visible = False
            gridView.Columns("ISTNPCS").Visible = False
            gridView.Columns("ISTNCRWT").Visible = False
            gridView.Columns("ISTNGRWT").Visible = False
            gridView.Columns("IVALUE").Visible = False
            gridView.Columns("ISALVALUE").Visible = False
            'CLOSING
            If ChkWithTag.Checked = False Then gridView.Columns("CTAGS").Visible = False
            gridView.Columns("CPCS").Visible = False
            gridView.Columns("CGRSWT").Visible = False
            gridView.Columns("CNETWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("CEXTRAWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("CTAGWT") Then gridView.Columns("CTAGWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("CCOVERWT") Then gridView.Columns("CCOVERWT").Visible = False
            gridView.Columns("CDIAPCS").Visible = False
            gridView.Columns("CDIAWT").Visible = False
            gridView.Columns("CSTNPCS").Visible = False
            gridView.Columns("CSTNCRWT").Visible = False
            gridView.Columns("CSTNGRWT").Visible = False
            gridView.Columns("CVALUE").Visible = False
            gridView.Columns("CSALVALUE").Visible = False

            'PENDING
            If ChkWithTag.Checked = False Then gridView.Columns("PTAGS").Visible = False
            gridView.Columns("PPCS").Visible = False
            gridView.Columns("PGRSWT").Visible = False
            gridView.Columns("PNETWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("PEXTRAWT").Visible = False
            gridView.Columns("PDIAPCS").Visible = False
            gridView.Columns("PDIAWT").Visible = False
            gridView.Columns("PSTNPCS").Visible = False
            gridView.Columns("PSTNCRWT").Visible = False
            gridView.Columns("PSTNGRWT").Visible = False
            gridView.Columns("PVALUE").Visible = False
            gridView.Columns("PSALVALUE").Visible = False

            If chkGrsWt.Checked = False Then gridView.Columns("APPOGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("APPONETWT").Visible = False
            If chkGrsWt.Checked = False Then gridView.Columns("AIIGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("AIINETWT").Visible = False
            If chkGrsWt.Checked = False Then gridView.Columns("APPCGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("APPCNETWT").Visible = False
            'gridView.Columns("ARRTAGS").Visible = True
            'gridView.Columns("ARRPCS").Visible = True
            If chkGrsWt.Checked = False Then gridView.Columns("ARRGRSWT").Visible = False
            If chkNetWt.Checked = False Then gridView.Columns("ARRNETWT").Visible = False

            gridView.Columns("APPODIAPCS").Visible = False
            gridView.Columns("APPODIAWT").Visible = False
            gridView.Columns("APPOSTNPCS").Visible = False
            gridView.Columns("APPOSTNCRWT").Visible = False
            gridView.Columns("APPOSTNGRWT").Visible = False
            gridView.Columns("APPOVALUE").Visible = False
            gridView.Columns("APPOSALVALUE").Visible = False
            gridView.Columns("ARRDIAPCS").Visible = False
            gridView.Columns("ARRDIAWT").Visible = False
            gridView.Columns("ARRSTNPCS").Visible = False
            gridView.Columns("ARRSTNCRWT").Visible = False
            gridView.Columns("ARRSTNGRWT").Visible = False
            gridView.Columns("ARRVALUE").Visible = False
            gridView.Columns("ARRSALVALUE").Visible = False
            gridView.Columns("AIIDIAPCS").Visible = False
            gridView.Columns("AIIDIAWT").Visible = False
            gridView.Columns("AIISTNPCS").Visible = False
            gridView.Columns("AIISTNCRWT").Visible = False
            gridView.Columns("AIISTNGRWT").Visible = False
            gridView.Columns("AIIVALUE").Visible = False
            gridView.Columns("AIISALVALUE").Visible = False

            gridView.Columns("APPCDIAPCS").Visible = False
            gridView.Columns("APPCDIAWT").Visible = False
            gridView.Columns("APPCSTNPCS").Visible = False
            gridView.Columns("APPCSTNCRWT").Visible = False
            gridView.Columns("APPCSTNGRWT").Visible = False
            gridView.Columns("APPCVALUE").Visible = False
            gridView.Columns("APPCSALVALUE").Visible = False

            If chkExtraWt.Checked = False Then gridView.Columns("OEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("REXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("IEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("CEXTRAWT").Visible = False
            If chkExtraWt.Checked = False Then gridView.Columns("PEXTRAWT").Visible = False
            If gridView.Columns.Contains("APPOEXTRAWT") Then gridView.Columns("APPOEXTRAWT").Visible = False
            If gridView.Columns.Contains("ARREXTRAWT") Then gridView.Columns("ARREXTRAWT").Visible = False
            If gridView.Columns.Contains("AIIEXTRAWT") Then gridView.Columns("AIIEXTRAWT").Visible = False
            If gridView.Columns.Contains("APPCEXTRAWT") Then gridView.Columns("APPCEXTRAWT").Visible = False

            If ChkTagWt.Checked = False And gridView.Columns.Contains("OTAGWT") Then gridView.Columns("OTAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("RTAGWT") Then gridView.Columns("RTAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("ITAGWT") Then gridView.Columns("ITAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("CTAGWT") Then gridView.Columns("CTAGWT").Visible = False
            If gridView.Columns.Contains("APPOTAGWT") Then gridView.Columns("APPOTAGWT").Visible = False
            If gridView.Columns.Contains("ARRTAGWT") Then gridView.Columns("ARRTAGWT").Visible = False
            If gridView.Columns.Contains("AIITAGWT") Then gridView.Columns("AIITAGWT").Visible = False
            If gridView.Columns.Contains("APPCTAGWT") Then gridView.Columns("APPCTAGWT").Visible = False

            If ChkCoverWt.Checked = False And gridView.Columns.Contains("OCOVERWT") Then gridView.Columns("OCOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("RCOVERWT") Then gridView.Columns("RCOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("ICOVERWT") Then gridView.Columns("ICOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("CCOVERWT") Then gridView.Columns("CCOVERWT").Visible = False
            If gridView.Columns.Contains("APPOCOVERWT") Then gridView.Columns("APPOCOVERWT").Visible = False
            If gridView.Columns.Contains("ARRCOVERWT") Then gridView.Columns("ARRCOVERWT").Visible = False
            If gridView.Columns.Contains("AIICOVERWT") Then gridView.Columns("AIICOVERWT").Visible = False
            If gridView.Columns.Contains("APPCCOVERWT") Then gridView.Columns("APPCCOVERWT").Visible = False

            If ChkWithTag.Checked Then gridView.Columns("APPOTAGS").HeaderText = "APP-TAGS"
            gridView.Columns("APPOPCS").HeaderText = "APP-PCS"
            gridView.Columns("APPOGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("APPONETWT").HeaderText = "APP-NETWT"
            gridView.Columns("AIITAGS").HeaderText = "APP-TAGS"
            gridView.Columns("AIIPCS").HeaderText = "APP-PCS"
            gridView.Columns("AIIGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("AIINETWT").HeaderText = "APP-NETWT"
            gridView.Columns("ARRTAGS").HeaderText = "APP-TAGS"
            gridView.Columns("ARRPCS").HeaderText = "APP-PCS"
            gridView.Columns("ARRGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("ARRNETWT").HeaderText = "APP-NETWT"
            gridView.Columns("APPCTAGS").HeaderText = "APP-TAGS"
            gridView.Columns("APPCPCS").HeaderText = "APP-PCS"
            gridView.Columns("APPCGRSWT").HeaderText = "APP-GRSWT"
            gridView.Columns("APPCNETWT").HeaderText = "APP-NETWT"

            If ChkWithTag.Checked Then gridView.Columns("APPOTAGS").Width = gridView.Columns("OTAGS").Width
            gridView.Columns("APPOPCS").Width = gridView.Columns("OPCS").Width
            gridView.Columns("APPOGRSWT").Width = gridView.Columns("OGRSWT").Width
            gridView.Columns("APPONETWT").Width = gridView.Columns("ONETWT").Width
            If ChkWithTag.Checked Then gridView.Columns("ARRTAGS").Width = gridView.Columns("RTAGS").Width
            gridView.Columns("ARRPCS").Width = gridView.Columns("RPCS").Width
            gridView.Columns("ARRGRSWT").Width = gridView.Columns("RGRSWT").Width
            gridView.Columns("ARRNETWT").Width = gridView.Columns("RNETWT").Width
            If ChkWithTag.Checked Then gridView.Columns("AIITAGS").Width = gridView.Columns("ITAGS").Width
            gridView.Columns("AIIPCS").Width = gridView.Columns("IPCS").Width
            gridView.Columns("AIIGRSWT").Width = gridView.Columns("IGRSWT").Width
            gridView.Columns("AIINETWT").Width = gridView.Columns("INETWT").Width
            If ChkWithTag.Checked Then gridView.Columns("APPCTAGS").Width = gridView.Columns("CTAGS").Width
            gridView.Columns("APPCPCS").Width = gridView.Columns("CPCS").Width
            gridView.Columns("APPCGRSWT").Width = gridView.Columns("CGRSWT").Width
            gridView.Columns("APPCNETWT").Width = gridView.Columns("CNETWT").Width
        Else
            If chkExtraWt.Checked = False And gridView.Columns.Contains("OEXTRAWT") Then gridView.Columns("OEXTRAWT").Visible = False
            If chkExtraWt.Checked = False And gridView.Columns.Contains("REXTRAWT") Then gridView.Columns("REXTRAWT").Visible = False
            If chkExtraWt.Checked = False And gridView.Columns.Contains("IEXTRAWT") Then gridView.Columns("IEXTRAWT").Visible = False
            If chkExtraWt.Checked = False And gridView.Columns.Contains("CEXTRAWT") Then gridView.Columns("CEXTRAWT").Visible = False
            If chkExtraWt.Checked = False And gridView.Columns.Contains("PEXTRAWT") Then gridView.Columns("PEXTRAWT").Visible = False

            If ChkTagWt.Checked = False And gridView.Columns.Contains("OTAGWT") Then gridView.Columns("OTAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("RTAGWT") Then gridView.Columns("RTAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("ITAGWT") Then gridView.Columns("ITAGWT").Visible = False
            If ChkTagWt.Checked = False And gridView.Columns.Contains("CTAGWT") Then gridView.Columns("CTAGWT").Visible = False

            If ChkCoverWt.Checked = False And gridView.Columns.Contains("OCOVERWT") Then gridView.Columns("OCOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("RCOVERWT") Then gridView.Columns("RCOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("ICOVERWT") Then gridView.Columns("ICOVERWT").Visible = False
            If ChkCoverWt.Checked = False And gridView.Columns.Contains("CCOVERWT") Then gridView.Columns("CCOVERWT").Visible = False

            If gridView.Columns.Contains("APPOTAGS") Then gridView.Columns("APPOTAGS").Visible = False
            If gridView.Columns.Contains("APPOPCS") Then gridView.Columns("APPOPCS").Visible = False
            If gridView.Columns.Contains("APPOGRSWT") Then gridView.Columns("APPOGRSWT").Visible = False
            If gridView.Columns.Contains("APPONETWT") Then gridView.Columns("APPONETWT").Visible = False
            If gridView.Columns.Contains("ARRTAGS") Then gridView.Columns("ARRTAGS").Visible = False
            If gridView.Columns.Contains("ARRPCS") Then gridView.Columns("ARRPCS").Visible = False
            If gridView.Columns.Contains("ARRGRSWT") Then gridView.Columns("ARRGRSWT").Visible = False
            If gridView.Columns.Contains("ARRNETWT") Then gridView.Columns("ARRNETWT").Visible = False
            If gridView.Columns.Contains("AIITAGS") Then gridView.Columns("AIITAGS").Visible = False
            If gridView.Columns.Contains("AIIPCS") Then gridView.Columns("AIIPCS").Visible = False
            If gridView.Columns.Contains("AIIGRSWT") Then gridView.Columns("AIIGRSWT").Visible = False
            If gridView.Columns.Contains("AIINETWT") Then gridView.Columns("AIINETWT").Visible = False
            If gridView.Columns.Contains("APPCTAGS") Then gridView.Columns("APPCTAGS").Visible = False
            If gridView.Columns.Contains("APPCPCS") Then gridView.Columns("APPCPCS").Visible = False
            If gridView.Columns.Contains("APPCGRSWT") Then gridView.Columns("APPCGRSWT").Visible = False
            If gridView.Columns.Contains("APPCNETWT") Then gridView.Columns("APPCNETWT").Visible = False

            If gridView.Columns.Contains("APPODIAPCS") Then gridView.Columns("APPODIAPCS").Visible = False
            If gridView.Columns.Contains("APPODIAWT") Then gridView.Columns("APPODIAWT").Visible = False
            If gridView.Columns.Contains("APPOSTNPCS") Then gridView.Columns("APPOSTNPCS").Visible = False
            If gridView.Columns.Contains("APPOSTNCRWT") Then gridView.Columns("APPOSTNCRWT").Visible = False
            If gridView.Columns.Contains("APPOSTNGRWT") Then gridView.Columns("APPOSTNGRWT").Visible = False
            If gridView.Columns.Contains("APPOVALUE") Then gridView.Columns("APPOVALUE").Visible = False
            If gridView.Columns.Contains("APPOSALVALUE") Then gridView.Columns("APPOSALVALUE").Visible = False
            If gridView.Columns.Contains("ARRDIAPCS") Then gridView.Columns("ARRDIAPCS").Visible = False
            If gridView.Columns.Contains("ARRDIAWT") Then gridView.Columns("ARRDIAWT").Visible = False
            If gridView.Columns.Contains("ARRSTNPCS") Then gridView.Columns("ARRSTNPCS").Visible = False
            If gridView.Columns.Contains("ARRSTNCRWT") Then gridView.Columns("ARRSTNCRWT").Visible = False
            If gridView.Columns.Contains("ARRSTNGRWT") Then gridView.Columns("ARRSTNGRWT").Visible = False
            If gridView.Columns.Contains("ARRVALUE") Then gridView.Columns("ARRVALUE").Visible = False
            If gridView.Columns.Contains("ARRSALVALUE") Then gridView.Columns("ARRSALVALUE").Visible = False
            If gridView.Columns.Contains("AIIDIAPCS") Then gridView.Columns("AIIDIAPCS").Visible = False
            If gridView.Columns.Contains("AIIDIAWT") Then gridView.Columns("AIIDIAWT").Visible = False
            If gridView.Columns.Contains("AIISTNPCS") Then gridView.Columns("AIISTNPCS").Visible = False
            If gridView.Columns.Contains("AIISTNCRWT") Then gridView.Columns("AIISTNCRWT").Visible = False
            If gridView.Columns.Contains("AIISTNGRWT") Then gridView.Columns("AIISTNGRWT").Visible = False
            If gridView.Columns.Contains("AIISTNPCS") Then gridView.Columns("AIISTNPCS").Visible = False
            If gridView.Columns.Contains("AIISTNWT") Then gridView.Columns("AIISTNWT").Visible = False
            If gridView.Columns.Contains("ARRSTNPCS") Then gridView.Columns("ARRSTNPCS").Visible = False
            If gridView.Columns.Contains("ARRSTNCRWT") Then gridView.Columns("ARRSTNCRWT").Visible = False
            If gridView.Columns.Contains("ARRSTNGRWT") Then gridView.Columns("ARRSTNGRWT").Visible = False

            If gridView.Columns.Contains("AIIVALUE") Then gridView.Columns("AIIVALUE").Visible = False
            If gridView.Columns.Contains("AIISALVALUE") Then gridView.Columns("AIISALVALUE").Visible = False

            If gridView.Columns.Contains("APPCDIAPCS") Then gridView.Columns("APPCDIAPCS").Visible = False
            If gridView.Columns.Contains("APPCDIAWT") Then gridView.Columns("APPCDIAWT").Visible = False
            If gridView.Columns.Contains("APPCSTNPCS") Then gridView.Columns("APPCSTNPCS").Visible = False
            If gridView.Columns.Contains("APPCSTNCRWT") Then gridView.Columns("APPCSTNCRWT").Visible = False
            If gridView.Columns.Contains("APPCSTNGRWT") Then gridView.Columns("APPCSTNGRWT").Visible = False
            If gridView.Columns.Contains("APPCVALUE") Then gridView.Columns("APPCVALUE").Visible = False
            If gridView.Columns.Contains("APPCSALVALUE") Then gridView.Columns("APPCSALVALUE").Visible = False


            If chkExtraWt.Checked = False Then gridView.Columns("CEXTRAWT").Visible = False

            If gridView.Columns.Contains("APPOEXTRAWT") Then gridView.Columns("APPOEXTRAWT").Visible = False
            If gridView.Columns.Contains("ARREXTRAWT") Then gridView.Columns("ARREXTRAWT").Visible = False
            If gridView.Columns.Contains("AIIEXTRAWT") Then gridView.Columns("AIIEXTRAWT").Visible = False
            If gridView.Columns.Contains("APPCEXTRAWT") Then gridView.Columns("APPCEXTRAWT").Visible = False

            If ChkTagWt.Checked = False And gridView.Columns.Contains("CTAGWT") Then gridView.Columns("CTAGWT").Visible = False

            If gridView.Columns.Contains("APPOTAGWT") Then gridView.Columns("APPOTAGWT").Visible = False
            If gridView.Columns.Contains("ARRTAGWT") Then gridView.Columns("ARRTAGWT").Visible = False
            If gridView.Columns.Contains("AIITAGWT") Then gridView.Columns("AIITAGWT").Visible = False
            If gridView.Columns.Contains("APPCTAGWT") Then gridView.Columns("APPCTAGWT").Visible = False

            If ChkCoverWt.Checked = False And gridView.Columns.Contains("CCOVERWT") Then gridView.Columns("CCOVERWT").Visible = False

            If gridView.Columns.Contains("APPOCOVERWT") Then gridView.Columns("APPOCOVERWT").Visible = False
            If gridView.Columns.Contains("ARRCOVERWT") Then gridView.Columns("ARRCOVERWT").Visible = False
            If gridView.Columns.Contains("AIICOVERWT") Then gridView.Columns("AIICOVERWT").Visible = False
            If gridView.Columns.Contains("APPCCOVERWT") Then gridView.Columns("APPCCOVERWT").Visible = False
        End If

        'If gridView.Columns.Contains("PDIAPCS") Then gridView.Columns("PDIAPCS").Visible = False
        'If gridView.Columns.Contains("PDIAWT") Then gridView.Columns("PDIAWT").Visible = False
        'If gridView.Columns.Contains("PSTNPCS") Then gridView.Columns("PSTNPCS").Visible = False
        'If gridView.Columns.Contains("PSTNWT") Then gridView.Columns("PSTNWT").Visible = False
        If gridView.Columns.Contains("PVALUE") Then gridView.Columns("PVALUE").Visible = ChkPendingStk.Checked
        If gridView.Columns.Contains("PSALVALUE") Then gridView.Columns("PSALVALUE").Visible = ChkPendingStk.Checked
        If gridView.Columns.Contains("PEXTRAWT") Then gridView.Columns("PEXTRAWT").Visible = ChkPendingStk.Checked And chkExtraWt.Checked
        If gridView.Columns.Contains("PARTICULAR") Then gridView.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        If gridView.Columns.Contains("CGROUP") Then gridView.Columns("CGROUP").Visible = False
        If gridView.Columns.Contains("TCGROUP") Then gridView.Columns("TCGROUP").Visible = False
        If gridView.Columns.Contains("CATCODE") Then gridView.Columns("CATCODE").Visible = False
        If gridView.Columns.Contains("SGROUP") Then gridView.Columns("SGROUP").Visible = False
        GridViewHeaderStyle1()
        gridView.Columns(0).Frozen = True
        tabMain.SelectedTab = tabView
    End Sub
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
        If rbtTag.Checked Or rbtBoth.Checked Then

            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"


            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            'strSql += vbCrLf + " ,SALVALUE AS VALUE"
            strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval_MODE1()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            'strSql += vbCrLf + " ,SALVALUE AS VALUE"
            strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
            strSql += vbCrLf + " ,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval_MODE1()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            'strSql += vbCrLf + " ,SALVALUE AS VALUE"
            strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
            strSql += vbCrLf + ",SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval_MODE1()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                ' strSql += vbCrLf + " ,SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + ",CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
                strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval_MODE1()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                'strSql += vbCrLf + ",SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + ",SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
                strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval_MODE1()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMTYPEID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                'strSql += vbCrLf + ",SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + ",SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
                strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += funcApproval_MODE1()
            End If
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                'strSql += vbCrLf + ",T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO  AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                ' strSql += vbCrLf + " ,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                ' strSql += vbCrLf + " ,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO) AS VALUE,SALVALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
            strSql += vbCrLf + "  SELECT 'OPE'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)VALUE,"
            strSql += vbCrLf + "CASE WHEN IT.CALTYPE='W' AND RECISS='R' THEN (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0))"
            strSql += vbCrLf + "WHEN IT.CALTYPE='R' AND RECISS='R' THEN (ISNULL(T.RATE,0)*ISNULL(T.PCS,0)) ELSE (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0)) END SALVALUE,SNO"
            strSql += " ,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID WHERE RECDATE < @ASONDATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval_MODE1()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "AND ISNULL(CANCEL,'') = '' AND T.RECISS='R'"

            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 'OPE'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)VALUE,"
            strSql += vbCrLf + "CASE WHEN IT.CALTYPE='W' THEN (ISNULL(T.RATE,0)*(ISNULL(T.NETWT,0)*(-1)))"
            strSql += vbCrLf + "WHEN IT.CALTYPE='R'  THEN (ISNULL(T.RATE,0)*(ISNULL(T.PCS,0)*(-1))) ELSE (ISNULL(T.RATE,0)*(ISNULL(T.NETWT,0)*(-1))) END SALVALUE,SNO"
            strSql += " ,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID WHERE RECDATE < @ASONDATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += funcApproval_MODE1()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = '' AND T.RECISS='I'"

            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'ISS'SEP"
            End If
            strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)VALUE,"
            strSql += vbCrLf + "CASE WHEN IT.CALTYPE='W' THEN (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0))"
            strSql += vbCrLf + "WHEN IT.CALTYPE='R' THEN (ISNULL(T.RATE,0)*ISNULL(T.PCS,0)) ELSE (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0)) END SALVALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            Else
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            End If
            'If chkWithApproval.Checked Then
            '    strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            'ElseIf chkOnlyApproval.Checked Then
            '    strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            'End If

            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'REC'SEP"
            End If
            strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMTYPEID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)VALUE,"
            strSql += vbCrLf + "CASE WHEN IT.CALTYPE='W' THEN (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0))"
            strSql += vbCrLf + "WHEN IT.CALTYPE='R' THEN (ISNULL(T.RATE,0)*ISNULL(T.PCS,0)) ELSE (ISNULL(T.RATE,0)*ISNULL(T.NETWT,0)) END SALVALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID  WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += itemCondStr
            strSql += tagCondStr
            If chkOnlyApproval.Checked Then
                strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            Else
                strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            End If
            'If chkWithApproval.Checked Then
            '    strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
            'ElseIf chkOnlyApproval.Checked Then
            '    strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
            'End If
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
                strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMTYPEID,S.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,NULL EXTRAWT"
                strSql += vbCrLf + ",T.STNAMT VALUE,"
                strSql += vbCrLf + "CASE WHEN IT.CALTYPE='W' THEN (ISNULL(S.RATE,0)*ISNULL(S.NETWT,0))"
                strSql += vbCrLf + "WHEN IT.CALTYPE='R' THEN (ISNULL(S.RATE,0)*ISNULL(S.PCS,0)) ELSE (ISNULL(S.RATE,0)*ISNULL(S.NETWT,0)) END SALVALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID"
                strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,0 EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) VALUE,CONVERT(NUMERIC(15,2),NULL)SALVALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
            If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        strSql += vbCrLf + "(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID=X.ITEMGROUP)AS GROUPNAME"
        If chkShortname.Checked = True Then
            strSql += vbCrLf + "     ,(SELECT  substring(ITEMNAME,1,15) FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT SUBSTRING(ITEMNAME,1,15) FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
            strSql += vbCrLf + "     ,(SELECT SUBSTRING(SUBITEMNAME,1,15) FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
        Else
            If chkOrderbyItemId.Checked Then
                strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
            Else
                strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
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
        End If
        strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
        strSql += " ("
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + " SELECT T.SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
            strSql += vbCrLf + " ,T.VALUE,T.SALVALUE,0 STONE,T.DIAPCS,T.DIAWT,T.STNPCS,T.STNCRWT,T.STNGRWT,T.STYLENO,T.RATE FROM TEMP" & systemId & "ITEMSTOCK T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,IT.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,S.EXTRAWT"
                If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
                strSql += vbCrLf + " ,S.VALUE,S.SALVALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON S.ITEMID=IT.ITEMID"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
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
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMGROUP,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN EXTRAWT ELSE -1*EXTRAWT END) EXTRAWT"
            If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
            strSql += vbCrLf + "  ,VALUE,SALVALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNCRWT ELSE -1*STNCRWT END) AS STNCRWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNGRWT ELSE -1*STNGRWT END) AS STNGRWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,ITEMGROUP,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE,SALVALUE"
            If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT GROUPNAME,ITEM,SUBITEM,TITEM"
        If chkShortname.Checked = True Then
            strSql += vbCrLf + " ,'.'+(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        End If
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = X.ITEMTYPEID)AS ITEMTYPE "
        strSql += vbCrLf + " ,STYLENO"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN EXTRAWT ELSE 0 END) AS OEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNCRWT ELSE 0 END) AS OSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNGRWT ELSE 0 END) AS OSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN SALVALUE ELSE 0 END) AS OSALVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE 0 END) AS REXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNCRWT ELSE 0 END) AS RSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNGRWT ELSE 0 END) AS RSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN SALVALUE ELSE 0 END) AS RSALVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN EXTRAWT ELSE 0 END) AS IEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNCRWT ELSE 0 END) AS ISTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNGRWT ELSE 0 END) AS ISTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN SALVALUE ELSE 0 END) AS ISALVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN EXTRAWT ELSE 0 END) AS AREXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNCRWT ELSE 0 END) AS ARSTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNGRWT ELSE 0 END) AS ARSTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN SALVALUE ELSE 0 END) AS ARSALVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN EXTRAWT ELSE 0 END) AS AIEXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNCRWT ELSE 0 END) AS AISTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNGRWT ELSE 0 END) AS AISTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN SALVALUE ELSE 0 END) AS AISALVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*EXTRAWT ELSE EXTRAWT END) AS CEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNCRWT ELSE STNCRWT END) AS CSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNGRWT ELSE STNGRWT END) AS CSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*SALVALUE ELSE SALVALUE END) AS CSALVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE,CASE WHEN STONE=1 THEN (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=X.ITEMID) ELSE '' END DIASTONE"
        strSql += vbCrLf + " ,(SELECT METALID from " & cnAdminDb & "..ITEMMAST WHERE ITEMID=X.ITEMID) METALID,ITEMID"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  GROUPNAME,TITEM,ITEM,SUBITEM,STONE,RATE,ITEMID"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO,ITEMID"
        If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPEID "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
        '    
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
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
            cmd.ExecuteNonQuery()
        End If
        strSql = " ALTER TABLE TEMP" & systemId & "ITEMSTK   ALTER COLUMN PARTICULAR VARCHAR(100)  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OEXTRAWT = NULL WHERE OEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNCRWT = NULL WHERE OSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNGRWT = NULL WHERE OSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSALVALUE = NULL WHERE OSALVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET REXTRAWT = NULL WHERE REXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNCRWT = NULL WHERE RSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNGRWT = NULL WHERE RSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSALVALUE = NULL WHERE RSALVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IEXTRAWT = NULL WHERE IEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNCRWT = NULL WHERE ISTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNGRWT = NULL WHERE ISTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISALVALUE = NULL WHERE ISALVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AREXTRAWT = NULL WHERE AREXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNCRWT = NULL WHERE ARSTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNGRWT = NULL WHERE ARSTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSALVALUE = NULL WHERE ARSALVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIEXTRAWT = NULL WHERE AIEXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNCRWT = NULL WHERE AISTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNGRWT = NULL WHERE AISTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISALVALUE = NULL WHERE AISALVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CEXTRAWT = NULL WHERE CEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNCRWT = NULL WHERE CSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNGRWT = NULL WHERE CSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSALVALUE = NULL WHERE CSALVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        If chkShortname.Checked = False Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        End If

        If chkWithSubItem.Checked Then
            If chkShortname.Checked = False Then
                strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " DELETE FROM TEMP" & systemId & "ITEMSTK WHERE "
        strSql += vbCrLf + "  OPCS IS NULL"
        strSql += vbCrLf + "  AND OGRSWT IS NULL"
        strSql += vbCrLf + "  AND ONETWT IS NULL"
        strSql += vbCrLf + "  AND RPCS IS NULL"
        strSql += vbCrLf + "  AND RGRSWT IS NULL"
        strSql += vbCrLf + "  AND RNETWT IS NULL"
        strSql += vbCrLf + "  AND IPCS IS NULL"
        strSql += vbCrLf + "  AND IGRSWT IS NULL"
        strSql += vbCrLf + "  AND INETWT IS NULL"
        strSql += vbCrLf + "  AND CPCS IS NULL"
        strSql += vbCrLf + "  AND CGRSWT IS NULL"
        strSql += vbCrLf + "  AND CNETWT IS NULL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,ODIAPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RDIAPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,IDIAPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CDIAPCS=L.CPCS"
        strSql += vbCrLf + " ,ODIAWT = L.OGRSWT,RDIAWT=L.RGRSWT,IDIAWT=L.IGRSWT,CDIAWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,OSTNPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RSTNPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,ISTNPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CSTNPCS=L.CPCS"
        strSql += vbCrLf + " ,OSTNCRWT=NULL,OSTNGRWT = L.OGRSWT,RSTNCRWT=NULL,RSTNGRWT=L.RGRSWT,ISTNCRWT=NULL,ISTNGRWT=L.IGRSWT,CSTNCRWT=NULL,CSTNGRWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()




        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,OEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNCRWT,OSTNGRWT,OVALUE,CONVERT(DECIMAL(15,2),OSALVALUE)OSALVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,REXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNCRWT,RSTNGRWT,RVALUE,CONVERT(DECIMAL(15,2),RSALVALUE)RSALVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,IEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNCRWT,ISTNGRWT,IVALUE,CONVERT(DECIMAL(15,2),ISALVALUE)ISALVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AREXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNCRWT,ARSTNGRWT,ARVALUE,CONVERT(DECIMAL(15,2),ARSALVALUE)ARSALVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AIEXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNCRWT,AISTNGRWT,AIVALUE,CONVERT(DECIMAL(15,2),AISALVALUE)AISALVALUE"

        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,CEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNCRWT,CSTNGRWT,CVALUE,CONVERT(DECIMAL(15,2),CSALVALUE)CSALVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,GROUPNAME,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
        If ChkLstGroupBy.CheckedItems.Contains("ITEMTYPE") Then strSql += vbCrLf + " ,ITEMTYPE "
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
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
        If chkWithGroupitem.Checked Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("OEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("OSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")
        ObjGrouper.pColumns_Sum.Add("OSALVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("REXTRAWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("RSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("RSALVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("IEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNCRWT")
        ObjGrouper.pColumns_Sum.Add("ISTNGRWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")
        ObjGrouper.pColumns_Sum.Add("ISALVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AREXTRAWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("ARSALVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIEXTRAWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNCRWT")
            ObjGrouper.pColumns_Sum.Add("AISTNGRWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
            ObjGrouper.pColumns_Sum.Add("AISALVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("CEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("CSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColumns_Sum.Add("CSALVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()

        Dim ind As Integer = gridView.RowCount - 1
        CType(gridView.DataSource, DataTable).Rows.Add()

        strSql = " SELECT CASE WHEN diastone='D' then 'STUD. DIAMOND' WHEN diastone='S' then 'STUD. STONE' ELSE 'STUD. PRECIOUS' END PARTICULAR"
        strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
        strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"
        strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
        strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
        strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
        strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(ARPCS) ARPCS,SUM(ARGRSWT)ARGRSWT,SUM(ARNETWT)ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AREXTRAWT)AREXTRAWT"
            strSql += vbCrLf + " ,SUM(ARDIAPCS) ARDIAPCS,SUM(ARDIAWT)ARDIAWT,SUM(ARSTNPCS) ARSTNPCS,SUM(ARSTNCRWT)ARSTNCRWT,SUM(ARSTNGRWT)ARSTNGRWT,SUM(ARVALUE)ARVALUE,SUM(ARSALVALUE)ARSALVALUE"
            strSql += vbCrLf + " ,SUM(AIPCS) AIPCS,SUM(AIGRSWT)AIGRSWT,SUM(AINETWT)AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIEXTRAWT)AIEXTRAWT"
            strSql += vbCrLf + " ,SUM(AIDIAPCS) AIDIAPCS,SUM(AIDIAWT)AIDIAWT,SUM(AISTNPCS) AISTNPCS,SUM(AISTNCRWT)AISTNCRWT,SUM(AISTNGRWT)AISTNGRWT,SUM(AIVALUE)AIVALUE,SUM(AISALVALUE)AISALVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
        strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK WHERE STONE=1"
        strSql += vbCrLf + "  GROUP BY DIASTONE"
        Dim DTSTUD As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DTSTUD)
        If DTSTUD.Rows.Count > 0 Then
            For cnt As Integer = 0 To DTSTUD.Rows.Count - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                With DTSTUD.Rows(cnt)
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = .Item("PARTICULAR")
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("OPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("OGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("ONETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = .Item("OEXTRAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = .Item("ODIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = .Item("ODIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = .Item("OSTNPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("OSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = .Item("OSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = .Item("OVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = .Item("OSALVALUE")

                    gridView.Rows(gridView.RowCount - 1).Cells("RPCS").Value = .Item("RPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RGRSWT").Value = .Item("RGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RNETWT").Value = .Item("RNETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("REXTRAWT").Value = .Item("REXTRAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RDIAPCS").Value = .Item("RDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RDIAWT").Value = .Item("RDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNPCS").Value = .Item("RSTNPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNCRWT").Value = .Item("RSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSTNGRWT").Value = .Item("RSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("RVALUE").Value = .Item("RVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("RSALVALUE").Value = .Item("RSALVALUE")

                    gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = .Item("IPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = .Item("IGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = .Item("INETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("IEXTRAWT").Value = .Item("IEXTRAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("IDIAPCS").Value = .Item("IDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("IDIAWT").Value = .Item("IDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNPCS").Value = .Item("ISTNPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNCRWT").Value = .Item("ISTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISTNGRWT").Value = .Item("ISTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("IVALUE").Value = .Item("IVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("ISALVALUE").Value = .Item("ISALVALUE")
                    If chkSeperateColumnApproval.Checked Then
                        gridView.Rows(gridView.RowCount - 1).Cells("ARPCS").Value = .Item("ARPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARGRSWT").Value = .Item("ARGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARNETWT").Value = .Item("ARNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AREXTRAWT").Value = .Item("AREXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARDIAPCS").Value = .Item("ARDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARDIAWT").Value = .Item("ARDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARSTNPCS").Value = .Item("ARSTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARSTNCRWT").Value = .Item("ARSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARSTNGRWT").Value = .Item("ARSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARVALUE").Value = .Item("ARVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("ARSALVALUE").Value = .Item("ARSALVALUE")

                        gridView.Rows(gridView.RowCount - 1).Cells("AIPCS").Value = .Item("AIPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIGRSWT").Value = .Item("AIGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AINETWT").Value = .Item("AINETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIEXTRAWT").Value = .Item("AIEXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIDIAPCS").Value = .Item("AIDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIDIAWT").Value = .Item("AIDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AISTNPCS").Value = .Item("AISTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("AISTNCRWT").Value = .Item("AISTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AISTNGRWT").Value = .Item("AISTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("AIVALUE").Value = .Item("AIVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("AISALVALUE").Value = .Item("AISALVALUE")
                    End If
                    gridView.Rows(gridView.RowCount - 1).Cells("CPCS").Value = .Item("CPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CGRSWT").Value = .Item("CGRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CNETWT").Value = .Item("CNETWT")
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CEXTRAWT").Value = .Item("CEXTRAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CDIAPCS").Value = .Item("CDIAPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CDIAWT").Value = .Item("CDIAWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNPCS").Value = .Item("CSTNPCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNCRWT").Value = .Item("CSTNCRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSTNGRWT").Value = .Item("CSTNGRWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("CVALUE").Value = .Item("CVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("CSALVALUE").Value = .Item("CSALVALUE")
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                End With
            Next
        End If

        If chkCmbMetal.Text = "ALL" Then
            CType(gridView.DataSource, DataTable).Rows.Add()
            strSql = " SELECT (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  WHERE METALID=T.METALID ) PARTICULAR"
            strSql += vbCrLf + " ,SUM(OPCS) OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT) ONETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(OEXTRAWT)OEXTRAWT"
            strSql += vbCrLf + " ,SUM(ODIAPCS) ODIAPCS,SUM(ODIAWT) ODIAWT,SUM(OSTNPCS) OSTNPCS,SUM(OSTNCRWT) OSTNCRWT,SUM(OSTNGRWT) OSTNGRWT,SUM(OVALUE) OVALUE,SUM(OSALVALUE) OSALVALUE"
            strSql += vbCrLf + " ,SUM(RPCS) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(REXTRAWT)REXTRAWT"
            strSql += vbCrLf + " ,SUM(RDIAPCS) RDIAPCS,SUM(RDIAWT) RDIAWT,SUM(RSTNPCS) RSTNPCS,SUM(RSTNCRWT) RSTNCRWT,SUM(RSTNGRWT) RSTNGRWT,SUM(RVALUE) RVALUE,SUM(RSALVALUE) RSALVALUE"
            strSql += vbCrLf + " ,SUM(IPCS) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(IEXTRAWT)IEXTRAWT"
            strSql += vbCrLf + " ,SUM(IDIAPCS) IDIAPCS,SUM(IDIAWT)IDIAWT,SUM(ISTNPCS) ISTNPCS,SUM(ISTNCRWT) ISTNCRWT,SUM(ISTNGRWT) ISTNGRWT,SUM(IVALUE) IVALUE,SUM(ISALVALUE) ISALVALUE"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + " ,SUM(ARPCS) ARPCS,SUM(ARGRSWT)ARGRSWT,SUM(ARNETWT)ARNETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AREXTRAWT)AREXTRAWT"
                strSql += vbCrLf + " ,SUM(ARDIAPCS) ARDIAPCS,SUM(ARDIAWT)ARDIAWT,SUM(ARSTNPCS) ARSTNPCS,SUM(ARSTNCRWT)ARSTNCRWT,SUM(ARSTNGRWT)ARSTNGRWT,SUM(ARVALUE)ARVALUE,SUM(ARSALVALUE)ARSALVALUE"
                strSql += vbCrLf + " ,SUM(AIPCS) AIPCS,SUM(AIGRSWT)AIGRSWT,SUM(AINETWT)AINETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(AIEXTRAWT)AIEXTRAWT"
                strSql += vbCrLf + " ,SUM(AIDIAPCS) AIDIAPCS,SUM(AIDIAWT)AIDIAWT,SUM(AISTNPCS) AISTNPCS,SUM(AISTNWT)AISTNWT,SUM(AIVALUE)AIVALUE,SUM(AISALVALUE)AISALVALUE"
            End If
            strSql += vbCrLf + " ,SUM(CPCS) CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CEXTRAWT)CEXTRAWT"
            strSql += vbCrLf + " ,SUM(CDIAPCS) CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS) CSTNPCS,SUM(CSTNCRWT)CSTNCRWT,SUM(CSTNGRWT)CSTNGRWT,SUM(CVALUE)CVALUE,SUM(CSALVALUE)CSALVALUE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK T WHERE STONE=0 AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST)"
            strSql += vbCrLf + "  GROUP BY METALID"
            Dim DTMETALDET As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DTMETALDET)
            If DTMETALDET.Rows.Count > 0 Then
                For cnt As Integer = 0 To DTMETALDET.Rows.Count - 1
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    With DTMETALDET.Rows(cnt)
                        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = .Item("PARTICULAR")
                        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("OPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("OGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("ONETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = .Item("OEXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = .Item("ODIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = .Item("ODIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = .Item("OSTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = .Item("OSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = .Item("OSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = .Item("OVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = .Item("OSALVALUE")

                        gridView.Rows(gridView.RowCount - 1).Cells("RPCS").Value = .Item("RPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RGRSWT").Value = .Item("RGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RNETWT").Value = .Item("RNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("REXTRAWT").Value = .Item("REXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RDIAPCS").Value = .Item("RDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RDIAWT").Value = .Item("RDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNPCS").Value = .Item("RSTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNCRWT").Value = .Item("RSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSTNGRWT").Value = .Item("RSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("RVALUE").Value = .Item("RVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("RSALVALUE").Value = .Item("RSALVALUE")

                        gridView.Rows(gridView.RowCount - 1).Cells("IPCS").Value = .Item("IPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IGRSWT").Value = .Item("IGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("INETWT").Value = .Item("INETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("IEXTRAWT").Value = .Item("IEXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("IDIAPCS").Value = .Item("IDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("IDIAWT").Value = .Item("IDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNPCS").Value = .Item("ISTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNCRWT").Value = .Item("ISTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISTNGRWT").Value = .Item("ISTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("IVALUE").Value = .Item("IVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("ISALVALUE").Value = .Item("ISALVALUE")
                        If chkSeperateColumnApproval.Checked Then
                            gridView.Rows(gridView.RowCount - 1).Cells("ARPCS").Value = .Item("ARPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARGRSWT").Value = .Item("ARGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARNETWT").Value = .Item("ARNETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AREXTRAWT").Value = .Item("AREXTRAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARDIAPCS").Value = .Item("ARDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARDIAWT").Value = .Item("ARDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARSTNPCS").Value = .Item("ARSTNPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARSTNCRWT").Value = .Item("ARSTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARSTNGRWT").Value = .Item("ARSTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARVALUE").Value = .Item("ARVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("ARSALVALUE").Value = .Item("ARSALVALUE")

                            gridView.Rows(gridView.RowCount - 1).Cells("AIPCS").Value = .Item("AIPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIGRSWT").Value = .Item("AIGRSWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AINETWT").Value = .Item("AINETWT")
                            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("AIEXTRAWT").Value = .Item("AIEXTRAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIDIAPCS").Value = .Item("AIDIAPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIDIAWT").Value = .Item("AIDIAWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AISTNPCS").Value = .Item("AISTNPCS")
                            gridView.Rows(gridView.RowCount - 1).Cells("AISTNCRWT").Value = .Item("AISTNCRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AISTNGRWT").Value = .Item("AISTNGRWT")
                            gridView.Rows(gridView.RowCount - 1).Cells("AIVALUE").Value = .Item("AIVALUE")
                            gridView.Rows(gridView.RowCount - 1).Cells("AISALVALUE").Value = .Item("AISALVALUE")
                        End If
                        gridView.Rows(gridView.RowCount - 1).Cells("CPCS").Value = .Item("CPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CGRSWT").Value = .Item("CGRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CNETWT").Value = .Item("CNETWT")
                        If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("CEXTRAWT").Value = .Item("CEXTRAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CDIAPCS").Value = .Item("CDIAPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CDIAWT").Value = .Item("CDIAWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNPCS").Value = .Item("CSTNPCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNCRWT").Value = .Item("CSTNCRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSTNGRWT").Value = .Item("CSTNGRWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("CVALUE").Value = .Item("CVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("CSALVALUE").Value = .Item("CSALVALUE")
                        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    End With
                Next
            End If
        End If

        If HideSummary = False Then
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("OSALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("RSALVALUE").Value

            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("ISALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("ARSALVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''APP ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("AISALVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            End If
            ''CLOSING
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSALVALUE").Value = gridView.Rows(ind).Cells("CSALVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If
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
        If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle()
        'GridViewHeaderStyle1()
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub Report_MODE2()
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
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"


            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            strSql += vbCrLf + " ,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += PartyCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += PartyCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
            strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += PartyCondStr
            'strSql += funcApproval()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                strSql += vbCrLf + " ,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
                strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += PartyCondStr
                'strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                strSql += vbCrLf + ",SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"

                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
                strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += PartyCondStr
                'strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,EXTRAWT"
                strSql += vbCrLf + ",SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
                strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += PartyCondStr
                'strSql += funcApproval()
            End If
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                strSql += vbCrLf + ",T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += PartyCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                strSql += vbCrLf + " ,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN (SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..ISSUE UNION ALL SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..APPISSUE) AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += PartyCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
                strSql += vbCrLf + " ,T.SALVALUE AS VALUE"
                strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
                strSql += vbCrLf + " WHERE 1=1"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += PartyCondStr
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
            strSql += vbCrLf + "   SELECT 'OPE'SEP,COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)VALUE,SNO"
            strSql += " ,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE < @ASONDATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'ISS'SEP"
            End If
            strSql += vbCrLf + "  ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15," & StoneRound & "),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
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
            strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15," & StoneRound & "),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
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
                strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,NULL EXTRAWT"
                strSql += vbCrLf + ",T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,0 EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,NULL STYLENO,CONVERT(NUMERIC(15," & StoneRound & "),NULL)RATE"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        strSql += vbCrLf + "(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID=X.ITEMGROUP)AS GROUPNAME"
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
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
            strSql += vbCrLf + " SELECT SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.VALUE,0 STONE,T.DIAPCS,T.DIAWT,T.STNPCS,T.STNCRWT,t.STNGRWT,T.STYLENO,T.RATE FROM TEMP" & systemId & "ITEMSTOCK T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,IT.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,S.EXTRAWT"
                strSql += vbCrLf + " ,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON S.ITEMID=IT.ITEMID"
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
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMGROUP,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN EXTRAWT ELSE -1*EXTRAWT END) EXTRAWT"
            strSql += vbCrLf + "  ,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNCRWT ELSE -1*STNCRWT END) AS STNCRWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNGRWT ELSE -1*STNGRWT END) AS STNGRWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE,ITEMGROUP"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT GROUPNAME,ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN EXTRAWT ELSE 0 END) AS OEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNCRWT ELSE 0 END) AS OSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNGRWT ELSE 0 END) AS OSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE 0 END) AS REXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNCRWT ELSE 0 END) AS RSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNGRWT ELSE 0 END) AS RSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN EXTRAWT ELSE 0 END) AS IEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNCRWT ELSE 0 END) AS ISTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNGRWT ELSE 0 END) AS ISTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN EXTRAWT ELSE 0 END) AS AREXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNCRWT ELSE 0 END) AS ARSTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNGRWT ELSE 0 END) AS ARSTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN EXTRAWT ELSE 0 END) AS AIEXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNCRWT ELSE 0 END) AS AISTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNGRWT ELSE 0 END) AS AISTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*EXTRAWT ELSE EXTRAWT END) AS CEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNCRWT ELSE STNCRWT END) AS CSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNGRWT ELSE STNGRWT END) AS CSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE,ITEMID "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  GROUPNAME,TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO,ITEMID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
        '    
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
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
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OEXTRAWT = NULL WHERE OEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNCRWT = NULL WHERE OSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNGRWT = NULL WHERE OSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET REXTRAWT = NULL WHERE REXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNCRWT = NULL WHERE RSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNGRWT = NULL WHERE RSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IEXTRAWT = NULL WHERE IEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNCRWT = NULL WHERE ISTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNGRWT = NULL WHERE ISTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AREXTRAWT = NULL WHERE AREXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNCRWT = NULL WHERE ARSTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNGRWT = NULL WHERE ARSTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIEXTRAWT = NULL WHERE AIEXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNCRWT = NULL WHERE AISTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNGRWT = NULL WHERE AISTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CEXTRAWT = NULL WHERE CEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNCRWT = NULL WHERE CSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNGRWT = NULL WHERE CSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,ODIAPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RDIAPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,IDIAPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CDIAPCS=L.CPCS"
        strSql += vbCrLf + " ,ODIAWT = L.OGRSWT,RDIAWT=L.RGRSWT,IDIAWT=L.IGRSWT,CDIAWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,OSTNPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RSTNPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,ISTNPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CSTNPCS=L.CPCS"
        strSql += vbCrLf + " ,OSTNCRWT = L.OGRSWT,RSTNGRWT=L.RGRSWT,ISTNCRWT=L.IGRSWT,CSTNGRWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,OEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNCRWT,OSTNGRWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,REXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNCRWT,RSTNGRWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,IEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNCRWT,ISTNGRWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AREXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNCRWT,ARSTNGRWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AIEXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNCRWT,AISTNGRWT,AIVALUE"
            'strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAPCS,ARDIAWT,ARSTNPCS,ARSTNWT,ARVALUE"
            'strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAPCS,AIDIAWT,AISTNPCS,AISTNWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,CEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNCRWT,CSTNGRWT,CVALUE"
        'strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,GROUPNAME,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
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
        'dtGrid.AcceptChanges()

        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        If chkWithGroupitem.Checked Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("OEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("OSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("REXTRAWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("RSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("IEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNCRWT")
        ObjGrouper.pColumns_Sum.Add("ISTNGRWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AREXTRAWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIEXTRAWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNCRWT")
            ObjGrouper.pColumns_Sum.Add("AISTNGRWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("CEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("CSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        Dim ind As Integer
        If HideSummary = False Then
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
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
                If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                GridStyle(True)
                gridView.Columns("COLHEAD").Visible = False
                GridViewHeaderStyle(True)
                tabMain.SelectedTab = tabView
            ElseIf chkOnlyApproval.Checked = True Then
                ind = gridView.RowCount - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
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
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    ''APP ISSUE
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                End If
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

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
                If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                GridStyle(True)
                gridView.Columns("COLHEAD").Visible = False
                GridViewHeaderStyle(True)
                tabMain.SelectedTab = tabView
            ElseIf chkWithApproval.Checked = True Then
                ind = gridView.RowCount - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
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
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    ''APP ISSUE
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                End If
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

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
                If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                GridStyle(True)
                gridView.Columns("COLHEAD").Visible = False
                GridViewHeaderStyle(True)
                tabMain.SelectedTab = tabView
            End If
        End If
        If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APPROVAL ONLY"
            dtGrid = gridView.DataSource
            Report_AppOnly()
        End If
        'End If

        'dtGrid.Rows.Add()
        'dtGrid = gridView.DataSource
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
        '    
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
    End Sub
    Private Sub Report_WithOutApp()
        Dim RecDate As String = Nothing
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        If chkAsOnDate.Checked Then
            dtpTo.Value = dtpFrom.Value
        End If

        Report_WithOutAppSub()
        Dim mapp As Boolean = True
        RecDate = "T.RECDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTK')>0 DROP TABLE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
        strSql += vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,EXTRAWT"
            End If
            strSql += vbCrLf + ",SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"

            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()

            strSql += vbCrLf + " UNION ALL"
            'strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            'If chkExtraWt.Checked Then
            '    strSql += vbCrLf + " ,EXTRAWT"
            'End If
            'strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            'If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            'Else
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            'End If
            'If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            'Else
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
            'End If
            'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            'strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            ''strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPOPE AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM TEMP" & systemId & "ITEMSTOCK1APPTRAN AS I WHERE I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO)"
            'strSql += vbCrLf + " AND TAGNO IN(SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN @ASONDATE AND @TODATE AND TRANTYPE<>'MI')"
            ''strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            'strSql += itemCondStr
            'strSql += tagCondStr
            ''strSql += funcApproval()
            'strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,EXTRAWT"
            End If
            strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
            '''''strSql += vbCrLf + " AND TAGNO IN(SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN @ASONDATE AND @TODATE AND TRANTYPE='MI')" ''''05-05-2021
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,EXTRAWT"
            End If
            strSql += vbCrLf + ",SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
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
                strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,EXTRAWT"
                End If
                strSql += vbCrLf + " ,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
                strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,EXTRAWT"
                End If
                strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
                strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += funcApproval()
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,EXTRAWT"
                End If
                strSql += vbCrLf + " ,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
                strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
                'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO AND RECDATE > T.RECDATE)"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += funcApproval()
            End If

            If chkSeperateColumnApproval.Checked Or mapp Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,T.EXTRAWT"
                End If
                strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,T.EXTRAWT"
                End If
                strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
                strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
                strSql += vbCrLf + " WHERE 1=1"
                strSql += itemCondStr
                strSql += tagCondStr
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,T.EXTRAWT"
                End If
                strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
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
            cmd.ExecuteNonQuery()
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
            strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,EXTRAWT"
            End If
            strSql += " ,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS"
            strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE < @ASONDATE"
            strSql += itemCondStr
            strSql += tagCondStr
            'strSql += funcApproval()
            If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            If chkSeperateColumnApproval.Checked Then
                strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
            Else
                strSql += vbCrLf + "  SELECT 'ISS'SEP"
            End If
            strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
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
            strSql += vbCrLf + ",T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWGRT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
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
                strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,NULL EXTRAWT"
                End If
                strSql += vbCrLf + "  ,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,0 EXTRAWT"
            End If
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        strSql += vbCrLf + "(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID=X.ITEMGROUP)AS GROUPNAME"
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
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
            strSql += vbCrLf + " SELECT T.SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + " ,T.VALUE,0 STONE,T.DIAPCS,T.DIAWT,T.STNPCS,T.STNCRWT,T.STNGRWT,T.STYLENO,T.RATE FROM TEMP" & systemId & "ITEMSTOCK T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,IT.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then
                    strSql += vbCrLf + " ,S.EXTRAWT"
                End If
                strSql += vbCrLf + " ,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON S.ITEMID=IT.ITEMID"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
            End If
            If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMGROUP,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN EXTRAWT ELSE -1*EXTRAWT END) AS EXTRAWT"
            End If
            strSql += vbCrLf + " ,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNCRWT ELSE -1*STNCRWT END) AS STNCRWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNGRWT ELSE -1*STNGRWT END) AS STNGRWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,ITEMGROUP,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT GROUPNAME,ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        '250213
        'If rbtTag.Checked Then
        '    strSql += vbCrLf + " ,NULL AS OPCS"
        '    strSql += vbCrLf + " ,NULL AS OGRSWT"
        '    strSql += vbCrLf + " ,NULL AS ONETWT"
        '    If chkExtraWt.Checked Then strSql += vbCrLf + " ,NULL AS OEXTRAWT"
        '    strSql += vbCrLf + " ,NULL AS ODIAPCS"
        '    strSql += vbCrLf + " ,NULL AS ODIAWT"
        '    strSql += vbCrLf + " ,NULL AS OSTNPCS"
        '    strSql += vbCrLf + " ,NULL AS OSTNWT"
        '    strSql += vbCrLf + " ,NULL AS OVALUE"
        'Else
        '250213
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN EXTRAWT ELSE 0 END) OEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNCRWT ELSE 0 END) AS OSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNGRWT ELSE 0 END) AS OSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        '250213
        'End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE 0 END) REXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNCRWT ELSE 0 END) AS RSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNGRWT ELSE 0 END) AS RSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN EXTRAWT ELSE 0 END) IEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNCRWT ELSE 0 END) AS ISTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNGRWT ELSE 0 END) AS ISTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN EXTRAWT ELSE 0 END) AREXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNCRWT ELSE 0 END) AS ARSTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNGRWT ELSE 0 END) AS ARSTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN EXTRAWT ELSE 0 END) AIEXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNCRWT ELSE 0 END) AS AISTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNGRWT ELSE 0 END) AS AISTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
        End If
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*EXTRAWT ELSE EXTRAWT END) AS CEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNCRWT ELSE STNCRWT END) AS CSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNGRWT ELSE STNGRWT END) AS CSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE,ITEMID "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"

        strSql += vbCrLf + " GROUP BY  GROUPNAME,TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO,ITEMID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If mapp Then
            'strSql = vbCrLf + "UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = CPCS-RPCS,CGRSWT = CGRSWT-RGRSWT,CNETWT = CNETWT-RGRSWT"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()
        End If
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
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OEXTRAWT = NULL WHERE OEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNCRWT = NULL WHERE OSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNGRWT = NULL WHERE OSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET REXTRAWT = NULL WHERE REXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNCRWT = NULL WHERE RSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNGRWT = NULL WHERE RSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IEXTRAWT = NULL WHERE IEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNCRWT = NULL WHERE ISTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNGRWT = NULL WHERE ISTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AREXTRAWT = NULL WHERE AREXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNCRWT = NULL WHERE ARSTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNGRWT = NULL WHERE ARSTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIEXTRAWT = NULL WHERE AIEXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNCRWT = NULL WHERE AISTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNGRWT = NULL WHERE AISTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CEXTRAWT = NULL WHERE CEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNCRWT = NULL WHERE CSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNGRWT = NULL WHERE CSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,ODIAPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RDIAPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,IDIAPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CDIAPCS=L.CPCS"
        strSql += vbCrLf + " ,ODIAWT = L.OGRSWT,RDIAWT=L.RGRSWT,IDIAWT=L.IGRSWT,CDIAWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,OSTNPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RSTNPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,ISTNPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CSTNPCS=L.CPCS"
        strSql += vbCrLf + " ,OSTNCRWT = L.OGRSWT,RSTNGRWT=L.RGRSWT,ISTNCRWT=L.IGRSWT,CSTNGRWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,OEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNCRWT,OSTNGRWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,REXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNCRWT,RSTNGRWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,IEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNCRWT,ISTNGRWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AREXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNCRWT,ARSTNGRWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AIEXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNCRWT,AISTNGRWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,CEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNCRWT,CSTNGRWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,GROUPNAME,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
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
        If chkWithGroupitem.Checked Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("OEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("OSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("REXTRAWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("RSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("IEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNCRWT")
        ObjGrouper.pColumns_Sum.Add("ISTNGRWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")

        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AREXTRAWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIEXTRAWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNCRWT")
            ObjGrouper.pColumns_Sum.Add("AISTNGRWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If

        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("CEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("CSTNGRWT")
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

            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''RECEIPT
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            ''ISSUE
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
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
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''APP ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
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
            If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
            gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
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
        '    
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
        If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridStyle(True)
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle(True)
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub Report_WithOutAppSub()
        Dim RecDate As String = Nothing
        RecDate = "I.TRANDATE"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APP')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APP"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APPOPE')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APPOPE"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1APPTRAN')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1APPTRAN"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN EXTRAWT ELSE -1 * EXTRAWT END) EXTRAWT"
            End If
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN VALUE ELSE -1 * VALUE END) VALUE"
            strSql += vbCrLf + " ,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAPCS ELSE -1 * DIAPCS END) DIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAWT ELSE -1 * DIAWT END) DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNPCS ELSE -1 * STNPCS END) STNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNWT ELSE -1 * STNWT END) STNWT"
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " FROM (/* A STARTS */"
            strSql += vbCrLf + " SELECT 'I' ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + ",T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
            End If

            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN (SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..ISSUE UNION ALL SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..APPISSUE) AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE "
            'strSql += vbCrLf + " AND I.TRANDATE < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R'ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + ",T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE "
            'strSql += vbCrLf + " AND I.TRANDATE < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += vbCrLf + " )A GROUP BY COSTID,DESIGNERID,TAGNO,ITEMCTRID,ITEMID,SUBITEMID,SNO,STYLENO,RATE"

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN (SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..ISSUE UNION ALL SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE FROM " & cnStockDb & "..APPISSUE) AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            'strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            'strSql += vbCrLf + " AND I.TRANDATE <= @TODATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            strSql += itemCondStr
            strSql += tagCondStr

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,T.EXTRAWT"
            End If
            strSql += vbCrLf + ",T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            'strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            'strSql += vbCrLf + " AND I.TRANDATE <= @TODATE"
            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            strSql += itemCondStr
            strSql += tagCondStr


            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT ITEMID,TAGNO "
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APPOPE FROM TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " WHERE SEP IN ('OPE')"
            strSql += vbCrLf + " GROUP BY ITEMID,TAGNO  "
            strSql += vbCrLf + " HAVING "
            strSql += vbCrLf + " SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE -1 * PCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE -1 * GRSWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE -1 * NETWT END) <> 0"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE -1 * EXTRAWT END) <> 0"
            End If
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE -1 * VALUE END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE -1 * DIAPCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE -1 * STNWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE -1 * STNPCS END) <> 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT ITEMID,TAGNO "
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1APPTRAN FROM TEMP" & systemId & "ITEMSTOCK1APP"
            strSql += vbCrLf + " WHERE SEP NOT IN ('OPE')"
            strSql += vbCrLf + " GROUP BY ITEMID,TAGNO  "
            strSql += vbCrLf + " HAVING "
            strSql += vbCrLf + " SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE -1 * PCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE -1 * GRSWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE -1 * NETWT END) <> 0"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE -1 * EXTRAWT END) <> 0"
            End If
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE -1 * VALUE END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE -1 * DIAPCS END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE -1 * STNWT END) <> 0"
            strSql += vbCrLf + " OR SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE -1 * STNPCS END) <> 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

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

        cmd.ExecuteNonQuery()
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql = " DECLARE @ASONDATE SMALLDATETIME"
            strSql += " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"

            strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
            strSql += vbCrLf + " SELECT 'OPE' SEP,TAGNO,COSTID,DESIGNERID,ITEMCTRID,ITEMGROUP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN PCS ELSE -1 * PCS END) PCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN GRSWT ELSE -1 * GRSWT END) GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN NETWT ELSE -1 * NETWT END) NETWT"
            If chkExtraWt.Checked Then
                strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN EXTRAWT ELSE -1 * EXTRAWT END) EXTRAWT"
            End If
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN VALUE ELSE -1 * VALUE END) VALUE"
            strSql += vbCrLf + " ,SNO,STYLENO,RATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAPCS ELSE -1 * DIAPCS END) DIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN DIAWT ELSE -1 * DIAWT END) DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNPCS ELSE -1 * STNPCS END) STNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNCRWT ELSE -1 * STNCRWT END) STNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN ISSREC = 'I' THEN STNGRWT ELSE -1 * STNGRWT END) STNGRWT"
            strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " FROM (/* A STARTS */"
            strSql += vbCrLf + " SELECT 'I' ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G'  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN (SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE,ACCODE FROM " & cnStockDb & "..ISSUE UNION ALL SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE,ACCODE FROM " & cnStockDb & "..APPISSUE) AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON T.ITEMID=IT.ITEMID WHERE " & RecDate & " < @ASONDATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            'strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += PartyCondStr
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
                If chkOnlyApproval.Checked = True Then
                    strSql += vbCrLf + " AND ISNULL(T.APPROVAL,'') = 'A'"
                End If
            End If
            'strSql += funcApproval()
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R'ISSREC,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.SALVALUE AS VALUE,CONVERT(VARCHAR(15),T.SNO)SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO  AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON T.ITEMID=IT.ITEMID "
            strSql += vbCrLf + "   WHERE " & RecDate & " < @ASONDATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            'strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += PartyCondStr
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
                If chkOnlyApproval.Checked = True Then
                    strSql += vbCrLf + " AND ISNULL(T.APPROVAL,'') = 'A'"
                End If
            End If
            'strSql += funcApproval()
            strSql += vbCrLf + " )A GROUP BY COSTID,DESIGNERID,TAGNO,ITEMCTRID,ITEMGROUP,ITEMID,SUBITEMID,SNO,STYLENO,RATE"

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ISS'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN (SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE,ACCODE FROM " & cnStockDb & "..ISSUE UNION ALL SELECT ITEMID,TAGNO,TRANDATE,CANCEL,TRANTYPE,ACCODE FROM " & cnStockDb & "..APPISSUE) AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON T.ITEMID=IT.ITEMID WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += PartyCondStr
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
                If chkOnlyApproval.Checked = True Then
                    strSql += vbCrLf + " AND ISNULL(T.APPROVAL,'') = 'A'"
                End If
            End If
            'strSql += funcApproval()

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'REC'SEP,T.TAGNO,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.SALVALUE AS VALUE,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE "
            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            End If
            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON T.ITEMID = I.ITEMID AND T.TAGNO = I.TAGNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON T.ITEMID=IT.ITEMID WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
            If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
            'strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE SNO = T.SNO)"
            strSql += itemCondStr
            strSql += tagCondStr
            strSql += PartyCondStr
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
                If chkOnlyApproval.Checked = True Then
                    strSql += vbCrLf + " AND ISNULL(T.APPROVAL,'') = 'A'"
                End If
            End If
            'strSql += funcApproval()
            'If chkWithCumulative.Checked Then
            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
            '    If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '    End If
            '    If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '    Else
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
            '        End If
            '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            '        Else
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
            '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNWT"
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
            cmd.ExecuteNonQuery()
        End If
        '-uncommented
        If chkOnlyApproval.Checked And chkWithApproval.Checked Then
            If rbtNonTag.Checked Or rbtBoth.Checked Then
                strSql = " DECLARE @ASONDATE SMALLDATETIME"
                strSql += " DECLARE @TODATE SMALLDATETIME"
                strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
                strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,CONVERT(NUMERIC(15,2),NULL)EXTRAWT,T.SNO"
                strSql += " ,T.RECISS"
                strSql += " ,0 STONE,CONVERT(VARCHAR(20),T.PACKETNO)STYLENO,T.RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON T.ITEMID=IT.ITEMID WHERE RECDATE < @ASONDATE"
                strSql += itemCondStr
                strSql += tagCondStr
                'strSql += funcApproval()
                If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
                strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
                If chkOnlyApproval.Checked Then
                    strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
                End If
                strSql += vbCrLf + "  UNION ALL"
                If chkSeperateColumnApproval.Checked Then
                    strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
                Else
                    strSql += vbCrLf + "  SELECT 'ISS'SEP"
                End If
                strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,CONVERT(NUMERIC(15,2),NULL)EXTRAWT,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
                strSql += itemCondStr
                strSql += tagCondStr
                'If chkWithApproval.Checked Then
                '    strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
                If chkOnlyApproval.Checked Then
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
                strSql += vbCrLf + " ,T.COSTID,T.DESIGNERID,T.ITEMCTRID,I.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE ,CONVERT(NUMERIC(15,2),NULL)EXTRAWT ,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
                If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)DIAWT"
                End If
                If chkStone.Checked And rbtDiaStnByColumn.Checked Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='C' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STONEUNIT='G' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNGRWT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNPCS"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNCRWT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & StoneRound & "),NULL)STNGRWT"
                End If
                strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG T LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON T.ITEMID=I.ITEMID WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
                strSql += itemCondStr
                strSql += tagCondStr
                'If chkWithApproval.Checked Then
                '    strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
                If chkOnlyApproval.Checked Then
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
                    strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                    strSql += vbCrLf + "  ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,0 EXTRAWT,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNWT"
                    strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
                    strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

                cmd.ExecuteNonQuery()
            End If
        End If
        '-uncommended
        If chkOnlyTag.Checked Then
            strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,SUBITEMID"
            strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,0 EXTRAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
            strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,DESIGNERID"
            strSql += vbCrLf + " ,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()
        Else
            If rbtTag.Checked Or rbtBoth.Checked Then
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

                cmd.ExecuteNonQuery()
            Else 'nontag
                strSql = " SELECT *"
                strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
        strSql += " SELECT "
        strSql += vbCrLf + "(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID=X.ITEMGROUP)AS GROUPNAME"
        If chkOrderbyItemId.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
        Else
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
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
            strSql += vbCrLf + " SELECT T.SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,IT.ITEMGROUP,T.ITEMID TITEMID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,T.EXTRAWT"
            strSql += vbCrLf + " ,T.VALUE,0 STONE,T.DIAPCS,T.DIAWT,T.STNPCS,T.STNCRWT,T.STNGRWT,T.STYLENO,T.RATE FROM TEMP" & systemId & "ITEMSTOCK T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON T.ITEMID=IT.ITEMID"
            If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
                Dim diaStone As String = ""
                If chkDiamond.Checked Then diaStone += "'D',"
                If chkStone.Checked Then diaStone += "'S',"
                diaStone = Mid(diaStone, 1, diaStone.Length - 1)
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,IT.ITEMGROUP,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
                strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT"
                If chkExtraWt.Checked Then strSql += vbCrLf + " ,NULL EXTRAWT"
                strSql += vbCrLf + " ,S.VALUE,1 STONE,NULL DIAPCS,NULL DIAWT,NULL STNPCS,NULL STNCRWT,NULL STNGRWT,S.STYLENO,S.RATE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON S.ITEMID=IT.ITEMID"
                strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
                If Chkwithtr.Checked = False Then strSql += vbCrLf + " And ISNULL(T.TRANSFERED,'') <> 'Y'"
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
            strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMGROUP,ITEMID TITEMID,ITEMID,SUBITEMID"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN EXTRAWT ELSE -1*EXTRAWT END) AS EXTRAWT"
            strSql += vbCrLf + "  ,VALUE,STONE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAPCS ELSE -1*DIAPCS END) AS DIAPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNPCS ELSE -1*STNPCS END) AS STNPCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNCRWT ELSE -1*STNCRWT END) AS STNCRWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNGRWT ELSE -1*STNGRWT END) AS STNGRWT,STYLENO,RATE"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
            strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,ITEMGROUP,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
        End If
        strSql += " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

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

            cmd.ExecuteNonQuery()
        End If


        strSql = " DECLARE @ASONDATE SMALLDATETIME"
        strSql += " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT GROUPNAME,ITEM,SUBITEM,TITEM"
        strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
        strSql += vbCrLf + " ,STYLENO"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN EXTRAWT ELSE 0 END) AS OEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAPCS ELSE 0 END) AS ODIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNPCS ELSE 0 END) AS OSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNCRWT ELSE 0 END) AS OSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNGRWT ELSE 0 END) AS OSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN EXTRAWT ELSE 0 END) AS REXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS RDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS RSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNCRWT ELSE 0 END) AS RSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNGRWT ELSE 0 END) AS RSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN EXTRAWT ELSE 0 END) AS IEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS IDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS ISTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNCRWT ELSE 0 END) AS ISTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNGRWT ELSE 0 END) AS ISTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN EXTRAWT ELSE 0 END) AS AREXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAPCS ELSE 0 END) AS ARDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNPCS ELSE 0 END) AS ARSTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNCRWT ELSE 0 END) AS ARSTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNGRWT ELSE 0 END) AS ARSTNGRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN EXTRAWT ELSE 0 END) AS AIEXTRAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAPCS ELSE 0 END) AS AIDIAPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNPCS ELSE 0 END) AS AISTNPCS"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNCRWT ELSE 0 END) AS AISTNCRWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNGRWT ELSE 0 END) AS AISTNGRWT"
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
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*EXTRAWT ELSE EXTRAWT END) AS CEXTRAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*DIAPCS ELSE DIAPCS END) AS CDIAPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*STNPCS ELSE STNPCS END) AS CSTNPCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*STNCRWT ELSE STNCRWT END) AS CSTNCRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*STNGRWT ELSE STNGRWT END) AS CSTNGRWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('REC','APPREC') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
        strSql += vbCrLf + " ,RATE"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE,ITEMID "
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        Else
            strSql += vbCrLf + " ,ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
        strSql += vbCrLf + " GROUP BY  GROUPNAME,TITEM,ITEM,SUBITEM,STONE,RATE"
        strSql += vbCrLf + " ,ITEMCTRID"
        strSql += vbCrLf + " ,DESIGNERID"
        strSql += vbCrLf + " ,COSTID"
        strSql += vbCrLf + " ,SUBITEM,STYLENO,ITEMID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

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
        '    
        '    cmd.ExecuteNonQuery()

        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'Else
        '    GroupColumn = "TEMPITEM"
        '    strSql = " /*INSERTIN Grand */"
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
        '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    
        '    cmd.ExecuteNonQuery()
        'End If
        'strSql = vbCrLf + " update TEMP123ITEMSTK set ITEM = '' where ITEM = '[   30] IDOL'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
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

            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OEXTRAWT = NULL WHERE OEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAPCS = NULL WHERE ODIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNPCS = NULL WHERE OSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNCRWT = NULL WHERE OSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNGRWT = NULL WHERE OSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET REXTRAWT = NULL WHERE REXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAPCS = NULL WHERE RDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNPCS = NULL WHERE RSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNCRWT = NULL WHERE RSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNGRWT = NULL WHERE RSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IEXTRAWT = NULL WHERE IEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAPCS = NULL WHERE IDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNPCS = NULL WHERE ISTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNCRWT = NULL WHERE ISTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNGRWT = NULL WHERE ISTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AREXTRAWT = NULL WHERE AREXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAPCS = NULL WHERE ARDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNPCS = NULL WHERE ARSTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNCRWT = NULL WHERE ARSTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNGRWT = NULL WHERE ARSTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
            If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIEXTRAWT = NULL WHERE AIEXTRAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAPCS = NULL WHERE AIDIAPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNPCS = NULL WHERE AISTNPCS = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNCRWT = NULL WHERE AISTNCRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNGRWT = NULL WHERE AISTNGRWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
        If chkExtraWt.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CEXTRAWT = NULL WHERE CEXTRAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAPCS = NULL WHERE CDIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNPCS = NULL WHERE CSTNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNCRWT = NULL WHERE CSTNCRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNGRWT = NULL WHERE CSTNGRWT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
        If chkWithSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,ODIAPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RDIAPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,IDIAPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CDIAPCS=L.CPCS"
        strSql += vbCrLf + " ,ODIAWT = L.OGRSWT,RDIAWT=L.RGRSWT,IDIAWT=L.IGRSWT,CDIAWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ITEMSTK"
        strSql += vbCrLf + " SET OPCS = null,OGRSWT = null,ONETWT = null,OSTNPCS = L.OPCS,RPCS=null,RGRSWT=null,RNETWT=null,RSTNPCS=L.RPCS,IPCS=null,IGRSWT=null,INETWT=null,ISTNPCS=L.IPCS"
        strSql += vbCrLf + " ,CPCS=null,CGRSWT=null,CNETWT=null,CSTNPCS=L.CPCS"
        strSql += vbCrLf + " ,OSTNCRWT=NULL,OSTNGRWT = L.OGRSWT,RSTNCRWT=NULL,RSTNGRWT=L.RGRSWT,ISTNCRWT=NULL,ISTNGRWT=L.IGRSWT,CSTNCRWT=NULL,CSTNGRWT=L.CGRSWT"
        strSql += " FROM TEMP" & systemId & "ITEMSTK AS L"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()


        strSql = " SELECT PARTICULAR"
        'If chkWithSubItem.Checked Then
        '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ITEM AS PARTICULAR"
        'End If
        strSql += vbCrLf + " ,CONVERT(INT,OPCS) OPCS,OGRSWT,ONETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,OEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,ODIAPCS) ODIAPCS,ODIAWT,CONVERT(INT,OSTNPCS) OSTNPCS,OSTNCRWT,OSTNGRWT,OVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,RPCS) RPCS,RGRSWT,RNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,REXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,RDIAPCS) RDIAPCS,RDIAWT,CONVERT(INT,RSTNPCS) RSTNPCS,RSTNCRWT,RSTNGRWT,RVALUE"
        strSql += vbCrLf + " ,CONVERT(INT,IPCS) IPCS,IGRSWT,INETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,IEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,IDIAPCS) IDIAPCS,IDIAWT,CONVERT(INT,ISTNPCS) ISTNPCS,ISTNCRWT,ISTNGRWT,IVALUE"
        If chkSeperateColumnApproval.Checked Then
            strSql += vbCrLf + " ,CONVERT(INT,ARPCS) ARPCS,ARGRSWT,ARNETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AREXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,ARDIAPCS) ARDIAPCS,ARDIAWT,CONVERT(INT,ARSTNPCS) ARSTNPCS,ARSTNCRWT,ARSTNGRWT,ARVALUE"
            strSql += vbCrLf + " ,CONVERT(INT,AIPCS) AIPCS,AIGRSWT,AINETWT"
            If chkExtraWt.Checked Then strSql += vbCrLf + " ,AIEXTRAWT"
            strSql += vbCrLf + " ,CONVERT(INT,AIDIAPCS) AIDIAPCS,AIDIAWT,CONVERT(INT,AISTNPCS) AISTNPCS,AISTNCRWT,AISTNGRWT,AIVALUE"
            'strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAPCS,ARDIAWT,ARSTNPCS,ARSTNWT,ARVALUE"
            'strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAPCS,AIDIAWT,AISTNPCS,AISTNWT,AIVALUE"
        End If
        strSql += vbCrLf + " ,CONVERT(INT,CPCS) CPCS,CGRSWT,CNETWT"
        If chkExtraWt.Checked Then strSql += vbCrLf + " ,CEXTRAWT"
        strSql += vbCrLf + " ,CONVERT(INT,CDIAPCS) CDIAPCS,CDIAWT,CONVERT(INT,CSTNPCS) CSTNPCS,CSTNCRWT,CSTNGRWT,CVALUE"
        'strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,CVALUE"
        strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,GROUPNAME,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
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
        If chkWithGroupitem.Checked Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("OPCS")
        ObjGrouper.pColumns_Sum.Add("OGRSWT")
        ObjGrouper.pColumns_Sum.Add("ONETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("OEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("OSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")

        ObjGrouper.pColumns_Sum.Add("RPCS")
        ObjGrouper.pColumns_Sum.Add("RGRSWT")
        ObjGrouper.pColumns_Sum.Add("RNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("REXTRAWT")
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("RSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        ObjGrouper.pColumns_Sum.Add("IPCS")
        ObjGrouper.pColumns_Sum.Add("IGRSWT")
        ObjGrouper.pColumns_Sum.Add("INETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("IEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNCRWT")
        ObjGrouper.pColumns_Sum.Add("ISTNGRWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")
        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARPCS")
            ObjGrouper.pColumns_Sum.Add("ARGRSWT")
            ObjGrouper.pColumns_Sum.Add("ARNETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AREXTRAWT")
            ObjGrouper.pColumns_Sum.Add("ARDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARSTNCRWT")
            ObjGrouper.pColumns_Sum.Add("ARSTNGRWT")
            ObjGrouper.pColumns_Sum.Add("ARVALUE")
            ObjGrouper.pColumns_Sum.Add("AIPCS")
            ObjGrouper.pColumns_Sum.Add("AIGRSWT")
            ObjGrouper.pColumns_Sum.Add("AINETWT")
            If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("AIEXTRAWT")
            ObjGrouper.pColumns_Sum.Add("AIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AISTNCRWT")
            ObjGrouper.pColumns_Sum.Add("AISTNGRWT")
            ObjGrouper.pColumns_Sum.Add("AIVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("CPCS")
        ObjGrouper.pColumns_Sum.Add("CGRSWT")
        ObjGrouper.pColumns_Sum.Add("CNETWT")
        If chkExtraWt.Checked Then ObjGrouper.pColumns_Sum.Add("CEXTRAWT")
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNCRWT")
        ObjGrouper.pColumns_Sum.Add("CSTNGRWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "TITEM,STONE"
        ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        Dim ind As Integer
        If HideSummary = False Then
            If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
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
                If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                If Sepaccodeview = True Then
                    If ChkSepaccode.Checked Then
                        Dim tempaccode As String = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='RPTITEMSTK_ACC_BASED'")
                        If tempaccode <> "" Then
                            Dim Str As New DataTable
                            Str = GetSqlTable("SELECT INITIAL FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN(" & GetQryString(tempaccode, ",") & ")", cn)
                            If Str.Rows.Count > 0 Then
                                lblTitle.Text += "["
                                For I As Integer = 0 To Str.Rows.Count - 1
                                    lblTitle.Text += Str.Rows(I).Item("INITIAL").ToString
                                    If I <> Str.Rows.Count - 1 Then
                                        lblTitle.Text += ","
                                    End If
                                Next
                                lblTitle.Text += "]"
                            End If
                        End If
                    End If
                End If
                lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                GridStyle(True)
                gridView.Columns("COLHEAD").Visible = False
                GridViewHeaderStyle(True)
                tabMain.SelectedTab = tabView
            ElseIf chkOnlyApproval.Checked = True Then
                ind = gridView.RowCount - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
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
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    ''APP ISSUE
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                End If
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
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
                If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                If Sepaccodeview = True Then
                    If ChkSepaccode.Checked Then
                        Dim tempaccode As String = GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='RPTITEMSTK_ACC_BASED'")
                        If tempaccode <> "" Then
                            Dim Str As New DataTable
                            Str = GetSqlTable("SELECT INITIAL FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN(" & GetQryString(tempaccode, ",") & ")", cn)
                            If Str.Rows.Count > 0 Then
                                lblTitle.Text += "["
                                For I As Integer = 0 To Str.Rows.Count - 1
                                    lblTitle.Text += Str.Rows(I).Item("INITIAL").ToString
                                    If I <> Str.Rows.Count - 1 Then
                                        lblTitle.Text += ","
                                    End If
                                Next
                                lblTitle.Text += "]"
                            End If
                        End If
                    End If
                End If
                lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                GridStyle(True)
                gridView.Columns("COLHEAD").Visible = False
                GridViewHeaderStyle(True)
                tabMain.SelectedTab = tabView

            ElseIf chkWithApproval.Checked Then
                ind = gridView.RowCount - 1
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("OEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ODIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("OSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("OSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("OSTNGRWT").Value

                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''RECEIPT
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("REXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("RDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("RSTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("RSTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("RSTNGRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                ''ISSUE
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
                If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("IEXTRAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("IDIAPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ISTNPCS").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ISTNCRWT").Value
                gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ISTNGRWT").Value
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
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AREXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("ARDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("ARSTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("ARSTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("ARSTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
                    ''APP ISSUE
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("AIEXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("AIDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("AISTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("AISTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("AISTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

                    CType(gridView.DataSource, DataTable).Rows.Add()
                    gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
                    If chkExtraWt.Checked Then gridView.Rows(gridView.RowCount - 1).Cells("OEXTRAWT").Value = gridView.Rows(ind).Cells("CEXTRAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAPCS").Value = gridView.Rows(ind).Cells("CDIAPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNPCS").Value = gridView.Rows(ind).Cells("CSTNPCS").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNCRWT").Value = gridView.Rows(ind).Cells("CSTNCRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OSTNGRWT").Value = gridView.Rows(ind).Cells("CSTNGRWT").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
                    gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                    gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

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
                    If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
                    If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
                    If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
                    lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    GridStyle(True)
                    gridView.Columns("COLHEAD").Visible = False
                    GridViewHeaderStyle(True)
                    tabMain.SelectedTab = tabView

                End If
            End If
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
        '    
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

        Dim dt As New DataTable()
        dt = gridView.DataSource
        If chkWithApproval.Checked = True And chkOnlyApproval.Checked = True Then
            dtGrid.Rows.Add()
            dtGrid.Merge(dt)
            gridView.DataSource = dtGrid
            For i As Integer = 0 To gridView.Rows.Count - 1
                If gridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "" Then
                    'If gridView.Rows(i).Cells("PARTICULAR").Value = "OPENING" Then gridView.Rows(i).DefaultCellStyle = GlobalVariables.reportTotalStyle
                    If gridView.Rows(i).Cells("PARTICULAR").Value = "GRAND TOTAL" Then gridView.Rows(i).DefaultCellStyle = Globalvariables.reportTotalStyle
                    'If gridView.Rows(i).Cells("PARTICULAR").Value = "RECEIPT" Then gridView.Rows(i).DefaultCellStyle = GlobalVariables.reportTotalStyle
                    'If gridView.Rows(i).Cells("PARTICULAR").Value = "ISSUE" Then gridView.Rows(i).DefaultCellStyle = GlobalVariables.reportTotalStyle
                    'If gridView.Rows(i).Cells("PARTICULAR").Value = "CLOSING" Then gridView.Rows(i).DefaultCellStyle = GlobalVariables.reportTotalStyle
                    If gridView.Rows(i).Cells("PARTICULAR").Value = "APPROVAL ONLY" Then gridView.Rows(i).DefaultCellStyle.BackColor = Color.Red : gridView.Rows(i).DefaultCellStyle.ForeColor = Color.White : gridView.Rows(i).DefaultCellStyle.Font = lblTitle.Font
                End If
            Next
        End If
    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strOExtraWt As String = ""
        Dim strRExtraWt As String = ""
        Dim strIExtraWt As String = ""
        Dim strARExtraWt As String = ""
        Dim strAIExtraWt As String = ""
        Dim strCExtraWt As String = ""
        If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
        If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
        If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
        If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
        If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
        If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"

        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE", GetType(String))
            .Columns.Add("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE", GetType(String))
            .Columns.Add("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE", GetType(String))
            If chkSeperateColumnApproval.Checked Then
                .Columns.Add("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE", GetType(String))
                .Columns.Add("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE", GetType(String))
            End If
            .Columns.Add("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE", GetType(String))
            .Columns.Add("RATE~STYLENO", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Caption = "OPENING"
            .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Caption = "RECEIPT"
            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Caption = "ISSUE"
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Caption = "APP REC"
                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Caption = "APP ISS"
            End If
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Caption = "CLOSING"
            .Columns("RATE~STYLENO").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            ' funcColWidth()
            funcColWidth2()
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

    Private Sub GridViewHeaderStyle1()

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strOExtraWt As String = ""
        Dim strRExtraWt As String = ""
        Dim strIExtraWt As String = ""
        Dim strOTagWt As String = ""
        Dim strRTagWt As String = ""
        Dim strITagWt As String = ""
        Dim strOCoverWt As String = ""
        Dim strRCoverWt As String = ""
        Dim strICoverWt As String = ""
        Dim strARExtraWt As String = ""
        Dim strAIExtraWt As String = ""
        Dim strARTagWt As String = ""
        Dim strAITagWt As String = ""
        Dim strARCoverWt As String = ""
        Dim strAICoverWt As String = ""
        Dim strCExtraWt As String = ""
        Dim strCTagWt As String = ""
        Dim strCCoverWt As String = ""
        Dim strPExtraWt As String = ""
        Dim strOAPPROVAL As String = ""
        Dim strRAPPROVAL As String = ""
        Dim strIAPPROVAL As String = ""
        Dim strCAPPROVAL As String = ""
        If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
        If ChkTagWt.Checked Then strOTagWt = "~OTAGWT"
        If ChkCoverWt.Checked Then strOCoverWt = "~OCOVERWT"
        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(grpby, 2, Len(grpby)) = "CU" Then
                strOCoverWt += "~OBOXWT~OTOTWT"
            End If
        End If
        If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strOAPPROVAL = "~APPOTAGS~APPOPCS~APPOGRSWT~APPONETWT"
        If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strRAPPROVAL = "~ARRTAGS~ARRPCS~ARRGRSWT~ARRNETWT"
        If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strIAPPROVAL = "~AIITAGS~AIIPCS~AIIGRSWT~AIINETWT"
        If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strCAPPROVAL = "~APPCTAGS~APPCPCS~APPCGRSWT~APPCNETWT"
        ''
        If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strOAPPROVAL = "~APPOTAGS~APPOPCS~APPOGRSWT~APPONETWT"
        If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strRAPPROVAL = "~ARRTAGS~ARRPCS~ARRGRSWT~ARRNETWT"
        If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strIAPPROVAL = "~AIITAGS~AIIPCS~AIIGRSWT~AIINETWT"
        If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strCAPPROVAL = "~APPCTAGS~APPCPCS~APPCGRSWT~APPCNETWT"

        If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
        If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
        If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
        If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
        If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"
        If chkExtraWt.Checked Then strPExtraWt = "~PEXTRAWT"

        If ChkTagWt.Checked Then strRTagWt = "~RTAGWT"
        If ChkTagWt.Checked Then strITagWt = "~ITAGWT"
        If ChkTagWt.Checked Then strARTagWt = "~ARTAGWT"
        If ChkTagWt.Checked Then strAITagWt = "~AITAGWT"
        If ChkTagWt.Checked Then strCTagWt = "~CTAGWT"

        If ChkCoverWt.Checked Then strRCoverWt = "~RCOVERWT"
        If ChkCoverWt.Checked Then strICoverWt = "~ICOVERWT"
        If ChkCoverWt.Checked Then strARCoverWt = "~ARCOVERWT"
        If ChkCoverWt.Checked Then strAICoverWt = "~AICOVERWT"
        If ChkCoverWt.Checked Then strCCoverWt = "~CCOVERWT"

        If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
            If Mid(grpby, 2, Len(grpby)) = "CU" Then
                strRCoverWt += "~RBOXWT~RTOTWT"
                strICoverWt += "~IBOXWT~ITOTWT"
                strCCoverWt += "~CBOXWT~CTOTWT"
            End If
        End If
        If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strOAPPROVAL += "~APPOEXTRAWT"
        If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strRAPPROVAL += "~ARREXTRAWT"
        If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strIAPPROVAL += "~AIIEXTRAWT"
        If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strCAPPROVAL += "~APPCEXTRAWT"


        If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strOAPPROVAL += "~APPOTAGWT"
        If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strRAPPROVAL += "~ARRTAGWT"
        If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strIAPPROVAL += "~AIITAGWT"
        If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strCAPPROVAL += "~APPCTAGWT"

        If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strOAPPROVAL += "~APPOCOVERWT"
        If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strRAPPROVAL += "~ARRCOVERWT"
        If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strIAPPROVAL += "~AIICOVERWT"
        If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strCAPPROVAL += "~APPCCOVERWT"

        If chkExtraWt.Checked And chkOnlyApproval.Checked Then strOAPPROVAL += "~APPOEXTRAWT"
        If chkExtraWt.Checked And chkOnlyApproval.Checked Then strRAPPROVAL += "~ARREXTRAWT"
        If chkExtraWt.Checked And chkOnlyApproval.Checked Then strIAPPROVAL += "~AIIEXTRAWT"
        If chkExtraWt.Checked And chkOnlyApproval.Checked Then strCAPPROVAL += "~APPCEXTRAWT"

        If ChkTagWt.Checked And chkOnlyApproval.Checked Then strOAPPROVAL += "~APPOTAGWT"
        If ChkTagWt.Checked And chkOnlyApproval.Checked Then strRAPPROVAL += "~ARRTAGWT"
        If ChkTagWt.Checked And chkOnlyApproval.Checked Then strIAPPROVAL += "~AIITAGWT"
        If ChkTagWt.Checked And chkOnlyApproval.Checked Then strCAPPROVAL += "~APPCTAGWT"


        If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strOAPPROVAL += "~APPOCOVERWT"
        If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strRAPPROVAL += "~ARRCOVERWT"
        If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strIAPPROVAL += "~AIICOVERWT"
        If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strCAPPROVAL += "~APPCCOVERWT"

        With dtMergeHeader

            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOAPPROVAL, GetType(String))
            .Columns.Add("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRAPPROVAL, GetType(String))
            .Columns.Add("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strIAPPROVAL, GetType(String))
            .Columns.Add("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCAPPROVAL, GetType(String))
            .Columns.Add("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE", GetType(String))
            .Columns.Add("RATE~STYLENO", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""

            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOAPPROVAL).Caption = "OPENING"
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRAPPROVAL).Caption = "RECEIPT"
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strIAPPROVAL).Caption = "ISSUE"
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCAPPROVAL).Caption = "CLOSING"
            .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Caption = "PENDING"
            .Columns("RATE~STYLENO").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth1()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Visible = ChkPendingStk.Checked
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If

        End With
    End Sub
    '01
    Private Sub GridViewHeaderStyleDetail()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strAPPROVAL As String = ""
        If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strAPPROVAL = "~APPDIAPCS~APPDIAWT~APPSTNPCS~APPSTNWT"
        With dtMergeHeader

            .Columns.Add("PARTICULAR", GetType(String))

            .Columns.Add("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strAPPROVAL, GetType(String))

            'If chkSeperateColumnApproval.Checked Then
            '    .Columns.Add("ARRDIAPCS~ARRDIAWT~ARRSTNPCS~ARRSTNWT~ARRVALUE", GetType(String))
            '    .Columns.Add("AIIDIAPCS~AIIDIAWT~AIISTNPCS~AIISTNWT~AIIVALUE", GetType(String))
            'End If
            .Columns.Add("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strAPPROVAL, GetType(String))
            .Columns.Add("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strAPPROVAL, GetType(String))

            .Columns.Add("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strAPPROVAL, GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""

            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strAPPROVAL).Caption = "OPENING"

            'If chkSeperateColumnApproval.Checked Then
            '    .Columns("ARRDIAPCS~ARRDIAWT~ARRSTNPCS~ARRSTNWT~ARRVALUE").Caption = "APP REC"
            '    .Columns("AIIDIAPCS~AIIDIAWT~AIISTNPCS~AIISTNWT~AIIVALUE").Caption = "APP ISS"
            'End If
            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strAPPROVAL).Caption = "RECEIPT"
            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strAPPROVAL).Caption = "ISSUE"

            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strAPPROVAL).Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""

        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidthDetail()
            gridviewDetail.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridviewDetail.ColumnCount - 1
                If gridviewDetail.Columns(cnt).Visible Then colWid += gridviewDetail.Columns(cnt).Width
            Next
            If colWid >= gridviewDetail.Width Then
                .Columns("SCROLL").Visible = CType(gridviewDetail.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridviewDetail.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub '01
    Private Sub GridViewHeaderStyle(ByVal rpflag As Boolean)

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strOExtraWt As String = ""
        Dim strRExtraWt As String = ""
        Dim strIExtraWt As String = ""
        Dim strARExtraWt As String = ""
        Dim strAIExtraWt As String = ""
        Dim strCExtraWt As String = ""
        If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
        If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
        If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
        If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
        If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
        If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"

        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE", GetType(String))
            .Columns.Add("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE", GetType(String))
            .Columns.Add("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE", GetType(String))
            If chkSeperateColumnApproval.Checked Then
                .Columns.Add("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE", GetType(String))
                .Columns.Add("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE", GetType(String))
            End If
            .Columns.Add("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE", GetType(String))
            .Columns.Add("RATE~STYLENO", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Caption = "OPENING"
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Caption = "RECEIPT"
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Caption = "ISSUE"
            If chkSeperateColumnApproval.Checked Then
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Caption = "APP REC"
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Caption = "APP ISS"
            End If
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Caption = "CLOSING"
            .Columns("RATE~STYLENO").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth(True)
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
        If ChkLstGroupBy.CheckedItems.Count > 1 Then
            MsgBox("Please Select One Item in Group by ")
            Exit Sub
        End If
        '01
        StoneDetail = False
        HMDetail = False
        gridviewDetail.Visible = False
        gridView.DataSource = Nothing
        '01
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If spbaserpt = True Then
            SpReport()
            Prop_Sets()
            Exit Sub
        End If
        If chkOnlyApproval.Checked = True And chkWithApproval.Checked = True Then
        ElseIf chkOnlyApproval.Checked = True And rbtTag.Checked = False Then
            MsgBox("Approval is allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub

        ElseIf chkOnlyTag.Checked = True And rbtTag.Checked = False Then
            MsgBox("Filtering allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If

        itemCondStr = funcItemFiltration() + " "
        tagCondStr = funcTagFiltration() + " "
        emptyCondStr = funcEmptyItemFiltration() + ""
        emptyCondStr_NONTAG = funcEmptyItemFiltration_NONTAG() + ""
        PartyCondStr = funcPartyFiltration() + " "
        Prop_Sets()
        ''NEW MODIFICATION WITH COUNTER CHANGE


        If NormalMode = False Then
            If chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
                Report_WithOutApp()
                ' ElseIf chkOnlyApproval.Checked = True Then
            ElseIf chkWithApproval.Checked = True Then
                Report_MODE2()
            ElseIf chkOnlyApproval.Checked = True Then
                Report_AppOnly()
            End If
        Else
            Report()
        End If
        If chkWithApproval.Checked = True And chkOnlyApproval.Checked = True Then
            Report_MODE2()
        End If
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
                        .Cells("ITEMNAME").Style.Font = New Font("VERDANA", 7, FontStyle.Bold) 'reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold) 'reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold) 'reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    '01
    Function GridViewFormatDetail() As Integer
        For Each dgvRow As DataGridViewRow In gridviewDetail.Rows
            With dgvRow
                If .Cells("ODIAPCS").Value.ToString() <> "" Then If .Cells("ODIAPCS").Value = "0" Then .Cells("ODIAPCS").Value = DBNull.Value
                If .Cells("ODIAWT").Value.ToString() <> "" Then If .Cells("ODIAWT").Value = "0.000" Then .Cells("ODIAWT").Value = DBNull.Value
                If .Cells("OSTNPCS").Value.ToString() <> "" Then If .Cells("OSTNPCS").Value = "0" Then .Cells("OSTNPCS").Value = DBNull.Value
                If .Cells("OSTNWT").Value.ToString() <> "" Then If .Cells("OSTNWT").Value = "0.000" Then .Cells("OSTNWT").Value = DBNull.Value
                If chkSeperateColumnApproval.Checked Then
                    If .Cells("APPODIAPCS").Value.ToString() <> "" Then If .Cells("APPODIAPCS").Value = "0" Then .Cells("APPODIAPCS").Value = DBNull.Value
                    If .Cells("APPODIAWT").Value.ToString() <> "" Then If .Cells("APPODIAWT").Value = "0.000" Then .Cells("APPODIAWT").Value = DBNull.Value
                    If .Cells("APPOSTNPCS").Value.ToString() <> "" Then If .Cells("APPOSTNPCS").Value = "0" Then .Cells("APPOSTNPCS").Value = DBNull.Value
                    If .Cells("APPOSTNWT").Value.ToString() <> "" Then If .Cells("APPOSTNWT").Value = "0.000" Then .Cells("APPOSTNWT").Value = DBNull.Value
                End If
                If .Cells("RDIAPCS").Value.ToString() <> "" Then If .Cells("RDIAPCS").Value = "0" Then .Cells("RDIAPCS").Value = DBNull.Value
                If .Cells("RDIAWT").Value.ToString() <> "" Then If .Cells("RDIAWT").Value = "0.000" Then .Cells("RDIAWT").Value = DBNull.Value
                If .Cells("RSTNPCS").Value.ToString() <> "" Then If .Cells("RSTNPCS").Value = "0" Then .Cells("RSTNPCS").Value = DBNull.Value
                If .Cells("RSTNWT").Value.ToString() <> "" Then If .Cells("RSTNWT").Value = "0.000" Then .Cells("RSTNWT").Value = DBNull.Value
                If chkSeperateColumnApproval.Checked Then
                    If .Cells("ARRDIAPCS").Value.ToString() <> "" Then If .Cells("ARRDIAPCS").Value = "0" Then .Cells("ARRDIAPCS").Value = DBNull.Value
                    If .Cells("ARRDIAWT").Value.ToString() <> "" Then If .Cells("ARRDIAWT").Value = "0.000" Then .Cells("ARRDIAWT").Value = DBNull.Value
                    If .Cells("ARRSTNPCS").Value.ToString() <> "" Then If .Cells("ARRSTNPCS").Value = "0" Then .Cells("ARRSTNPCS").Value = DBNull.Value
                    If .Cells("ARRSTNWT").Value.ToString() <> "" Then If .Cells("ARRSTNWT").Value = "0.000" Then .Cells("ARRSTNWT").Value = DBNull.Value
                End If
                If .Cells("IDIAPCS").Value.ToString() <> "" Then If .Cells("IDIAPCS").Value = "0" Then .Cells("IDIAPCS").Value = DBNull.Value
                If .Cells("IDIAWT").Value.ToString() <> "" Then If .Cells("IDIAWT").Value = "0.000" Then .Cells("IDIAWT").Value = DBNull.Value
                If .Cells("ISTNPCS").Value.ToString() <> "" Then If .Cells("ISTNPCS").Value = "0" Then .Cells("ISTNPCS").Value = DBNull.Value
                If .Cells("ISTNWT").Value.ToString() <> "" Then If .Cells("ISTNWT").Value = "0.000" Then .Cells("ISTNWT").Value = DBNull.Value
                If chkSeperateColumnApproval.Checked Then
                    If .Cells("AIIDIAPCS").Value.ToString() <> "" Then If .Cells("AIIDIAPCS").Value = "0" Then .Cells("AIIDIAPCS").Value = DBNull.Value
                    If .Cells("AIIDIAWT").Value.ToString() <> "" Then If .Cells("AIIDIAWT").Value = "0.000" Then .Cells("AIIDIAWT").Value = DBNull.Value
                    If .Cells("AIISTNPCS").Value.ToString() <> "" Then If .Cells("AIISTNPCS").Value = "0" Then .Cells("AIISTNPCS").Value = DBNull.Value
                    If .Cells("AIISTNWT").Value.ToString() <> "" Then If .Cells("AIISTNWT").Value = "0.000" Then .Cells("AIISTNWT").Value = DBNull.Value
                End If
                If .Cells("CDIAPCS").Value.ToString() <> "" Then If .Cells("CDIAPCS").Value = "0" Then .Cells("CDIAPCS").Value = DBNull.Value
                If .Cells("CDIAWT").Value.ToString() <> "" Then If .Cells("CDIAWT").Value = "0.000" Then .Cells("CDIAWT").Value = DBNull.Value
                If .Cells("CSTNPCS").Value.ToString() <> "" Then If .Cells("CSTNPCS").Value = "0" Then .Cells("CSTNPCS").Value = DBNull.Value
                If .Cells("CSTNWT").Value.ToString() <> "" Then If .Cells("CSTNWT").Value = "0.000" Then .Cells("CSTNWT").Value = DBNull.Value
                If chkSeperateColumnApproval.Checked Then
                    If .Cells("APPCDIAPCS").Value.ToString() <> "" Then If .Cells("APPCDIAPCS").Value = "0" Then .Cells("APPCDIAPCS").Value = DBNull.Value
                    If .Cells("APPCDIAWT").Value.ToString() <> "" Then If .Cells("APPCDIAWT").Value = "0.000" Then .Cells("APPCDIAWT").Value = DBNull.Value
                    If .Cells("APPCSTNPCS").Value.ToString() <> "" Then If .Cells("APPCSTNPCS").Value = "0" Then .Cells("APPCSTNPCS").Value = DBNull.Value
                    If .Cells("APPCSTNWT").Value.ToString() <> "" Then If .Cells("APPCSTNWT").Value = "0.000" Then .Cells("APPCSTNWT").Value = DBNull.Value
                End If
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = New Font("VERDANA", 8, FontStyle.Bold) 'reportHeadStyle.Font
                    Case "S"
                        .Cells("ITEM").Style.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold) 'reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold) 'reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function '01
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            If spbaserpt = True Then
                funcColWidth1()
            Else
                If NormalMode = False Then
                    If chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
                        funcColWidth()
                    ElseIf chkWithApproval.Checked = True Then
                        funcColWidth()
                    ElseIf chkOnlyApproval.Checked = True Then
                        funcColWidth()
                    End If
                Else
                    funcColWidth2()
                End If
                'funcColWidth()
                'funcColWidth1()
            End If
        End If
    End Sub

    Function funcColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            Dim strOExtraWt As String = ""
            Dim strRExtraWt As String = ""
            Dim strIExtraWt As String = ""
            Dim strARExtraWt As String = ""
            Dim strAIExtraWt As String = ""
            Dim strCExtraWt As String = ""
            If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
            If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
            If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
            If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
            If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
            If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"


            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width = gridView.Columns("OPCS").Width
            If chkGrsWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("OGRSWT").Width
            If chkNetWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("ONETWT").Width
            If chkExtraWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("OEXTRAWT").Width
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNCRWT").Visible, gridView.Columns("OSTNCRWT").Width, 0) + IIf(gridView.Columns("OSTNGRWT").Visible, gridView.Columns("OSTNGRWT").Width, 0)
            '.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0) + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").HeaderText = "OPENING"

            ''.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width = gridView.Columns("OPCS").Width
            ''If chkGrsWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("OGRSWT").Width
            ''If chkNetWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("ONETWT").Width
            ''If chkExtraWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("OEXTRAWT").Width
            ''.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            ''.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNWCRT").Visible, gridView.Columns("OSTNCRWT").Width, 0) + IIf(gridView.Columns("OSTNGRWT").Visible, gridView.Columns("OSTNGRWT").Width, 0)
            ' ''.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0) + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0)
            ''.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").HeaderText = "OPENING"




            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width = gridView.Columns("RPCS").Width
            If chkGrsWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("RGRSWT").Width
            If chkNetWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("RNETWT").Width
            If chkExtraWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("REXTRAWT").Width
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RSTNPCS").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNCRWT").Visible, gridView.Columns("RSTNCRWT").Width, 0) + IIf(gridView.Columns("RSTNGRWT").Visible, gridView.Columns("RSTNGRWT").Width, 0)
            '.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0) + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").HeaderText = "RECEIPT"

            ''.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width = gridView.Columns("RPCS").Width
            ''If chkGrsWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("RGRSWT").Width
            ''If chkNetWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("RNETWT").Width
            ''If chkExtraWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("REXTRAWT").Width
            ''.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            ''.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNWT").Width, 0)
            ' ''.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0) + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0)
            ''.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").HeaderText = "RECEIPT"


            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width = gridView.Columns("IPCS").Width
            If chkGrsWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("IGRSWT").Width
            If chkNetWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("INETWT").Width
            If chkExtraWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("IEXTRAWT").Width
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("ISTNPCS").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNCRWT").Visible, gridView.Columns("ISTNCRWT").Width, 0) + IIf(gridView.Columns("ISTNGRWT").Visible, gridView.Columns("ISTNGRWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").HeaderText = "ISSUE"


            ''.Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width = gridView.Columns("IPCS").Width
            ''If chkGrsWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("IGRSWT").Width
            ''If chkNetWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("INETWT").Width
            ''If chkExtraWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("IEXTRAWT").Width
            ''.Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            ''.Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNWT").Width, 0)
            ''.Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0)
            ''.Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~IVALUE~ISALVALUE").HeaderText = "ISSUE"


            If chkSeperateColumnApproval.Checked Then
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width = gridView.Columns("ARPCS").Width
                If chkGrsWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARGRSWT").Width
                If chkNetWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARNETWT").Width
                If chkExtraWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("AREXTRAWT").Width
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAPCS").Width, 0) + IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAWT").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARSTNPCS").Visible, gridView.Columns("ARSTNPCS").Width, 0) + IIf(gridView.Columns("ARSTNCRWT").Visible, gridView.Columns("ARSTNCRWT").Width, 0) + IIf(gridView.Columns("ARSTNGRWT").Visible, gridView.Columns("ARSTNGRWT").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARVALUE").Visible, gridView.Columns("ARVALUE").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").HeaderText = "APP REC"

                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width = gridView.Columns("AIPCS").Width
                If chkGrsWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AIGRSWT").Width
                If chkNetWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AINETWT").Width
                If chkExtraWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AIEXTRAWT").Width
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0) + IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AISTNPCS").Visible, gridView.Columns("AISTNPCS").Width, 0) + IIf(gridView.Columns("AISTNCRWT").Visible, gridView.Columns("AISTNCRWT").Width, 0) + IIf(gridView.Columns("AISTNCRWT").Visible, gridView.Columns("AISTNCRWT").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIVALUE").Visible, gridView.Columns("AIVALUE").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").HeaderText = "APP ISS"


                ''.Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width = gridView.Columns("ARPCS").Width
                ''If chkGrsWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("ARGRSWT").Width
                ''If chkNetWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("ARNETWT").Width
                ''If chkExtraWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("AREXTRAWT").Width
                ''.Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAPCS").Width, 0) + IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAWT").Width, 0)
                ''.Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARSTNWT").Visible, gridView.Columns("ARSTNPCS").Width, 0) + IIf(gridView.Columns("ARSTNWT").Visible, gridView.Columns("ARSTNWT").Width, 0)
                ''.Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARVALUE").Visible, gridView.Columns("ARVALUE").Width, 0)
                ''.Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNWT~ARTAGPCS~ARVALUE~ARSALVALUE").HeaderText = "APP REC"

                ''.Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width = gridView.Columns("AIPCS").Width
                ''If chkGrsWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AIGRSWT").Width
                ''If chkNetWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AINETWT").Width
                ''If chkExtraWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AIEXTRAWT").Width
                ''.Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0) + IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0)
                ''.Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AISTNWT").Visible, gridView.Columns("AISTNPCS").Width, 0) + IIf(gridView.Columns("AISTNWT").Visible, gridView.Columns("AISTNWT").Width, 0)
                ''.Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AIVALUE").Visible, gridView.Columns("AIVALUE").Width, 0)
                ''.Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNWT~AITAGPCS~AIVALUE~AISALVALUE").HeaderText = "APP ISS"
            End If

            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width = gridView.Columns("CPCS").Width
            If chkGrsWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CGRSWT").Width
            If chkNetWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CNETWT").Width
            If chkExtraWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CEXTRAWT").Width
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAPCS").Width, 0) + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CSTNPCS").Visible, gridView.Columns("CSTNPCS").Width, 0) + IIf(gridView.Columns("CSTNCRWT").Visible, gridView.Columns("CSTNCRWT").Width, 0) + IIf(gridView.Columns("CSTNGRWT").Visible, gridView.Columns("CSTNGRWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CVALUE").Visible, gridView.Columns("CVALUE").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").HeaderText = "CLOSING"

            .Columns("RATE~STYLENO").Width = IIf(gridView.Columns("RATE").Visible, gridView.Columns("RATE").Width, 0) _
            + IIf(gridView.Columns("STYLENO").Visible, gridView.Columns("STYLENO").Width, 0)
            .Columns("RATE~STYLENO").Visible = gridView.Columns("RATE").Visible Or gridView.Columns("STYLENO").Visible
            .Columns("RATE~STYLENO").HeaderText = ""

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function
    Function funcColWidth2() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            Dim strOExtraWt As String = ""
            Dim strRExtraWt As String = ""
            Dim strIExtraWt As String = ""
            Dim strARExtraWt As String = ""
            Dim strAIExtraWt As String = ""
            Dim strCExtraWt As String = ""
            If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
            If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
            If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
            If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
            If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
            If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"


            .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width = gridView.Columns("OPCS").Width
            If chkGrsWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("OGRSWT").Width
            If chkNetWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("ONETWT").Width
            If chkExtraWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += gridView.Columns("OEXTRAWT").Width
            .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNCRWT").Visible, gridView.Columns("OSTNCRWT").Width, 0) + IIf(gridView.Columns("OSTNGRWT").Visible, gridView.Columns("OSTNGRWT").Width, 0)
            '.Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OVALUE~OSALVALUE").Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0) + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE").HeaderText = "OPENING"




            .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width = gridView.Columns("RPCS").Width
            If chkGrsWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("RGRSWT").Width
            If chkNetWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("RNETWT").Width
            If chkExtraWt.Checked Then .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width += gridView.Columns("REXTRAWT").Width
            .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RSTNPCS").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNCRWT").Visible, gridView.Columns("RSTNCRWT").Width, 0) + IIf(gridView.Columns("RSTNGRWT").Visible, gridView.Columns("RSTNGRWT").Width, 0)
            '.Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RVALUE~RSALVALUE").Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0) + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE").HeaderText = "RECEIPT"

            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width = gridView.Columns("IPCS").Width
            If chkGrsWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("IGRSWT").Width
            If chkNetWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("INETWT").Width
            If chkExtraWt.Checked Then .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += gridView.Columns("IEXTRAWT").Width
            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("ISTNPCS").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNCRWT").Visible, gridView.Columns("ISTNCRWT").Width, 0) + IIf(gridView.Columns("ISTNGRWT").Visible, gridView.Columns("ISTNGRWT").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE").HeaderText = "ISSUE"

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width = gridView.Columns("ARPCS").Width
                If chkGrsWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("ARGRSWT").Width
                If chkNetWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("ARNETWT").Width
                If chkExtraWt.Checked Then .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += gridView.Columns("AREXTRAWT").Width
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAPCS").Width, 0) + IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAWT").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARSTNPCS").Visible, gridView.Columns("ARSTNPCS").Width, 0) + IIf(gridView.Columns("ARSTNCRWT").Visible, gridView.Columns("ARSTNCRWT").Width, 0) + IIf(gridView.Columns("ARSTNGRWT").Visible, gridView.Columns("ARSTNGRWT").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").Width += IIf(gridView.Columns("ARVALUE").Visible, gridView.Columns("ARVALUE").Width, 0)
                .Columns("ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE~ARSALVALUE").HeaderText = "APP REC"

                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width = gridView.Columns("AIPCS").Width
                If chkGrsWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AIGRSWT").Width
                If chkNetWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AINETWT").Width
                If chkExtraWt.Checked Then .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += gridView.Columns("AIEXTRAWT").Width
                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0) + IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AISTNPCS").Visible, gridView.Columns("AISTNPCS").Width, 0) + IIf(gridView.Columns("AISTNCRWT").Visible, gridView.Columns("AISTNCRWT").Width, 0) + IIf(gridView.Columns("AISTNCRWT").Visible, gridView.Columns("AISTNCRWT").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").Width += IIf(gridView.Columns("AIVALUE").Visible, gridView.Columns("AIVALUE").Width, 0)
                .Columns("AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE~AISALVALUE").HeaderText = "APP ISS"
            End If
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width = gridView.Columns("CPCS").Width
            If chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += gridView.Columns("CGRSWT").Width
            If chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += gridView.Columns("CNETWT").Width
            If chkExtraWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += gridView.Columns("CEXTRAWT").Width
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAPCS").Width, 0) + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += IIf(gridView.Columns("CSTNPCS").Visible, gridView.Columns("CSTNPCS").Width, 0) + IIf(gridView.Columns("CSTNCRWT").Visible, gridView.Columns("CSTNCRWT").Width, 0) + IIf(gridView.Columns("CSTNGRWT").Visible, gridView.Columns("CSTNGRWT").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").Width += IIf(gridView.Columns("CVALUE").Visible, gridView.Columns("CVALUE").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE").HeaderText = "CLOSING"

            .Columns("RATE~STYLENO").Width = IIf(gridView.Columns("RATE").Visible, gridView.Columns("RATE").Width, 0) _
            + IIf(gridView.Columns("STYLENO").Visible, gridView.Columns("STYLENO").Width, 0)
            .Columns("RATE~STYLENO").Visible = gridView.Columns("RATE").Visible Or gridView.Columns("STYLENO").Visible
            .Columns("RATE~STYLENO").HeaderText = ""

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function
    '01
    Function funcColWidthDetail() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridviewDetail.Columns("PARTICULAR").Width
            Dim strSepApp As String = ""
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strSepApp = "~APPDIAPCS~APPDIAWT~APPSTNPCS~APPSTNCRWT~APPSTNGRWT"
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strSepApp = "~APPDIAPCS~APPDIAWT~APPSTNPCS~APPSTNWT"

            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width = gridviewDetail.Columns("ODIAPCS").Width
            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("ODIAWT").Width
            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("OSTNPCS").Width
            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("OSTNWT").Width
            'If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
            '    .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPODIAPCS").Width
            '    .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPODIAWT").Width
            '    .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPOSTNPCS").Width
            '    .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPOSTNWT").Width
            'End If

            .Columns("ODIAPCS~ODIAWT~OSTNPCS~OSTNWT" & strSepApp).HeaderText = "OPENING"

            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width = gridviewDetail.Columns("RDIAPCS").Width
            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("RDIAWT").Width
            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("RSTNPCS").Width
            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("RSTNWT").Width

            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("ARRDIAPCS").Width
                .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("ARRDIAWT").Width
                .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("ARRSTNPCS").Width
                .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).Width += gridviewDetail.Columns("ARRSTNWT").Width
            End If

            .Columns("RDIAPCS~RDIAWT~RSTNPCS~RSTNWT" & strSepApp).HeaderText = "RECEIPT"

            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width = gridviewDetail.Columns("IDIAPCS").Width
            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("IDIAWT").Width
            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("ISTNPCS").Width
            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("ISTNWT").Width

            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("AIIDIAPCS").Width
                .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("AIIDIAWT").Width
                .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("AIISTNPCS").Width
                .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).Width += gridviewDetail.Columns("AIISTNWT").Width
            End If
            .Columns("IDIAPCS~IDIAWT~ISTNPCS~ISTNWT" & strSepApp).HeaderText = "ISSUE"


            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width = gridviewDetail.Columns("CDIAPCS").Width
            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("CDIAWT").Width
            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("CSTNPCS").Width
            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("CSTNWT").Width
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPCDIAPCS").Width
                .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPCDIAWT").Width
                .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPCSTNPCS").Width
                .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).Width += gridviewDetail.Columns("APPCSTNWT").Width
            End If


            .Columns("CDIAPCS~CDIAWT~CSTNPCS~CSTNWT" & strSepApp).HeaderText = "CLOSING"

            .Columns("SCROLL").Visible = CType(gridviewDetail.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridviewDetail.Controls(1), VScrollBar).Width
        End With
    End Function
    '01 
    Function funcColWidth1() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            Dim strOExtraWt As String = ""
            Dim strRExtraWt As String = ""
            Dim strIExtraWt As String = ""
            Dim strARExtraWt As String = ""
            Dim strAIExtraWt As String = ""
            Dim strCExtraWt As String = ""
            Dim strPExtraWt As String = ""

            Dim strOTagWt As String = ""
            Dim strRTagWt As String = ""
            Dim strITagWt As String = ""
            Dim strARTagWt As String = ""
            Dim strAITagWt As String = ""
            Dim strCTagWt As String = ""

            Dim strOCoverWt As String = ""
            Dim strRCoverWt As String = ""
            Dim strICoverWt As String = ""
            Dim strARCoverWt As String = ""
            Dim strAICoverWt As String = ""
            Dim strCCoverWt As String = ""

            If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
            If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
            If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
            'If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
            'If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
            If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"
            If chkExtraWt.Checked Then strPExtraWt = "~PEXTRAWT"

            If ChkTagWt.Checked Then strOTagWt = "~OTAGWT"
            If ChkTagWt.Checked Then strRTagWt = "~RTAGWT"
            If ChkTagWt.Checked Then strITagWt = "~ITAGWT"
            If ChkTagWt.Checked Then strCTagWt = "~CTAGWT"
            If ChkCoverWt.Checked Then strOCoverWt = "~OCOVERWT"
            If ChkCoverWt.Checked Then strRCoverWt = "~RCOVERWT"
            If ChkCoverWt.Checked Then strICoverWt = "~ICOVERWT"
            If ChkCoverWt.Checked Then strCCoverWt = "~CCOVERWT"
            If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
                If Mid(grpby, 2, Len(grpby)) = "CU" Then
                    strOCoverWt += "~OBOXWT~OTOTWT"
                    strRCoverWt += "~RBOXWT~RTOTWT"
                    strICoverWt += "~IBOXWT~ITOTWT"
                    strCCoverWt += "~CBOXWT~CTOTWT"
                End If
            End If
            Dim strOSepApp As String = ""
            Dim strRSepApp As String = ""
            Dim strISepApp As String = ""
            Dim strCSepApp As String = ""

            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strOSepApp = "~APPOTAGS~APPOPCS~APPOGRSWT~APPONETWT"
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strRSepApp = "~ARRTAGS~ARRPCS~ARRGRSWT~ARRNETWT"
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strISepApp = "~AIITAGS~AIIPCS~AIIGRSWT~AIINETWT"
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then strCSepApp = "~APPCTAGS~APPCPCS~APPCGRSWT~APPCNETWT"

            If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strOSepApp += "~APPOEXTRAWT"
            If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strRSepApp += "~ARREXTRAWT"
            If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strISepApp += "~AIIEXTRAWT"
            If chkExtraWt.Checked And chkSeperateColumnApproval.Checked Then strCSepApp += "~APPCEXTRAWT"

            If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strOSepApp += "~APPOTAGWT"
            If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strRSepApp += "~ARRTAGWT"
            If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strISepApp += "~AIITAGWT"
            If ChkTagWt.Checked And chkSeperateColumnApproval.Checked Then strCSepApp += "~APPCTAGWT"

            If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strOSepApp += "~APPOCOVERWT"
            If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strRSepApp += "~ARRCOVERWT"
            If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strISepApp += "~AIICOVERWT"
            If ChkCoverWt.Checked And chkSeperateColumnApproval.Checked Then strCSepApp += "~APPCCOVERWT"

            If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strOSepApp = "~APPOTAGS~APPOPCS~APPOGRSWT~APPONETWT"
            If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strRSepApp = "~ARRTAGS~ARRPCS~ARRGRSWT~ARRNETWT"
            If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strISepApp = "~AIITAGS~AIIPCS~AIIGRSWT~AIINETWT"
            If chkSeperateColumnApproval.Checked = False And chkOnlyApproval.Checked = True Then strCSepApp = "~APPCTAGS~APPCPCS~APPCGRSWT~APPCNETWT"

            If chkExtraWt.Checked And chkOnlyApproval.Checked Then strOSepApp += "~APPOEXTRAWT"
            If chkExtraWt.Checked And chkOnlyApproval.Checked Then strRSepApp += "~ARREXTRAWT"
            If chkExtraWt.Checked And chkOnlyApproval.Checked Then strISepApp += "~AIIEXTRAWT"
            If chkExtraWt.Checked And chkOnlyApproval.Checked Then strCSepApp += "~APPCEXTRAWT"

            If ChkTagWt.Checked And chkOnlyApproval.Checked Then strOSepApp += "~APPOTAGWT"
            If ChkTagWt.Checked And chkOnlyApproval.Checked Then strRSepApp += "~ARRTAGWT"
            If ChkTagWt.Checked And chkOnlyApproval.Checked Then strISepApp += "~AIITAGWT"
            If ChkTagWt.Checked And chkOnlyApproval.Checked Then strCSepApp += "~APPCTAGWT"

            If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strOSepApp += "~APPOCOVERWT"
            If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strRSepApp += "~ARRCOVERWT"
            If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strISepApp += "~AIICOVERWT"
            If ChkCoverWt.Checked And chkOnlyApproval.Checked Then strCSepApp += "~APPCCOVERWT"

            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width = gridView.Columns("OPCS").Width
            If ChkWithTag.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("OTAGS").Width
            If chkGrsWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("OGRSWT").Width
            If chkNetWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("ONETWT").Width
            If chkExtraWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("OEXTRAWT").Width
            If ChkTagWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("OTAGWT").Width
            If ChkCoverWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("OCOVERWT").Width
            If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
                If Mid(grpby, 2, Len(grpby)) = "CU" Then
                    .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += IIf(gridView.Columns("OBOXWT").Visible, gridView.Columns("OBOXWT").Width, 0) _
                    + IIf(gridView.Columns("OTOTWT").Visible, gridView.Columns("OTOTWT").Width, 0)
                End If
            End If
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += IIf(gridView.Columns("ODIAPCS").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNCRWT").Visible, gridView.Columns("OSTNCRWT").Width, 0) + IIf(gridView.Columns("OSTNGRWT").Visible, gridView.Columns("OSTNGRWT").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0) + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0)
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOPCS").Width
                If ChkWithTag.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOTAGS").Width
                If chkGrsWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOGRSWT").Width
                If chkNetWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPONETWT").Width
                If chkExtraWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOEXTRAWT").Width
                If ChkTagWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOTAGWT").Width
                If ChkCoverWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).Width += gridView.Columns("APPOCOVERWT").Width
            End If
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & strOTagWt & strOCoverWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE~OSALVALUE" & strOSepApp).HeaderText = "OPENING"

            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width = gridView.Columns("RPCS").Width
            If ChkWithTag.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("RTAGS").Width
            If chkGrsWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("RGRSWT").Width
            If chkNetWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("RNETWT").Width
            If chkExtraWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("REXTRAWT").Width
            If ChkTagWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("RTAGWT").Width
            If ChkCoverWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("RCOVERWT").Width
            If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
                If Mid(grpby, 2, Len(grpby)) = "CU" Then
                    .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += +IIf(gridView.Columns("RBOXWT").Visible, gridView.Columns("RBOXWT").Width, 0) _
                    + IIf(gridView.Columns("RTOTWT").Visible, gridView.Columns("RTOTWT").Width, 0)
                End If
            End If
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += IIf(gridView.Columns("RDIAPCS").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += IIf(gridView.Columns("RSTNPCS").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNCRWT").Visible, gridView.Columns("RSTNCRWT").Width, 0) + IIf(gridView.Columns("RSTNGRWT").Visible, gridView.Columns("RSTNGRWT").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0) + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0)

            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRPCS").Width
                If ChkWithTag.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRTAGS").Width
                If chkGrsWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRGRSWT").Width
                If chkNetWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRNETWT").Width
                If chkExtraWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARREXTRAWT").Width
                If ChkTagWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRTAGWT").Width
                If ChkCoverWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).Width += gridView.Columns("ARRCOVERWT").Width
            End If

            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & strRTagWt & strRCoverWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE~RSALVALUE" & strRSepApp).HeaderText = "RECEIPT"

            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width = gridView.Columns("IPCS").Width
            If ChkWithTag.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("ITAGS").Width
            If chkGrsWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("IGRSWT").Width
            If chkNetWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("INETWT").Width
            If chkExtraWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("IEXTRAWT").Width
            If ChkTagWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("ITAGWT").Width
            If ChkCoverWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("ICOVERWT").Width
            If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
                If Mid(grpby, 2, Len(grpby)) = "CU" Then
                    .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += IIf(gridView.Columns("IBOXWT").Visible, gridView.Columns("IBOXWT").Width, 0) _
                    + IIf(gridView.Columns("ITOTWT").Visible, gridView.Columns("ITOTWT").Width, 0)
                End If
            End If
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += IIf(gridView.Columns("IDIAPCS").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += IIf(gridView.Columns("ISTNPCS").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNCRWT").Visible, gridView.Columns("ISTNCRWT").Width, 0) + IIf(gridView.Columns("ISTNGRWT").Visible, gridView.Columns("ISTNGRWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0) + IIf(gridView.Columns("ISALVALUE").Visible, gridView.Columns("ISALVALUE").Width, 0)

            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIIPCS").Width
                If ChkWithTag.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIITAGS").Width
                If chkGrsWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIIGRSWT").Width
                If chkNetWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIINETWT").Width
                If chkExtraWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIIEXTRAWT").Width
                If ChkTagWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIITAGWT").Width
                If ChkCoverWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).Width += gridView.Columns("AIICOVERWT").Width
            End If
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & strITagWt & strICoverWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE~ISALVALUE" & strISepApp).HeaderText = "ISSUE"


            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width = gridView.Columns("CPCS").Width
            If ChkWithTag.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CTAGS").Width
            If chkGrsWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CGRSWT").Width
            If chkNetWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CNETWT").Width
            If chkExtraWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CEXTRAWT").Width
            If ChkTagWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CTAGWT").Width
            If ChkCoverWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("CCOVERWT").Width
            If chkSummary.Checked And (ChkCoverWt.Checked Or ChkTagWt.Checked) Then
                If Mid(grpby, 2, Len(grpby)) = "CU" Then
                    .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += IIf(gridView.Columns("CBOXWT").Visible, gridView.Columns("CBOXWT").Width, 0) _
                    + IIf(gridView.Columns("CTOTWT").Visible, gridView.Columns("CTOTWT").Width, 0)
                End If
            End If
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += IIf(gridView.Columns("CDIAPCS").Visible, gridView.Columns("CDIAPCS").Width, 0) + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += IIf(gridView.Columns("CSTNPCS").Visible, gridView.Columns("CSTNPCS").Width, 0) + IIf(gridView.Columns("CSTNCRWT").Visible, gridView.Columns("CSTNCRWT").Width, 0) + IIf(gridView.Columns("CSTNGRWT").Visible, gridView.Columns("CSTNGRWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += IIf(gridView.Columns("CVALUE").Visible, gridView.Columns("CVALUE").Width, 0) + IIf(gridView.Columns("CSALVALUE").Visible, gridView.Columns("CSALVALUE").Width, 0)
            If chkSeperateColumnApproval.Checked And chkOnlyApproval.Checked = False Then
                .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCPCS").Width
                If ChkWithTag.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCTAGS").Width
                If chkGrsWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCGRSWT").Width
                If chkNetWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCNETWT").Width
                If chkExtraWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCEXTRAWT").Width
                If ChkTagWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCTAGWT").Width
                If ChkCoverWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).Width += gridView.Columns("APPCCOVERWT").Width
            End If


            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & strCTagWt & strCCoverWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE~CSALVALUE" & strCSepApp).HeaderText = "CLOSING"
            'FOR PENDING TRANSFER
            If ChkPendingStk.Checked Then
                .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width = IIf(gridView.Columns.Contains("PPCS"), gridView.Columns("PPCS").Width, 0)
                If ChkWithTag.Checked Then .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += gridView.Columns("PTAGS").Width
                If chkGrsWt.Checked Then .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += gridView.Columns("PGRSWT").Width
                If chkNetWt.Checked Then .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += gridView.Columns("PNETWT").Width
                If chkExtraWt.Checked Then .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += gridView.Columns("PEXTRAWT").Width
                .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += IIf(gridView.Columns("PDIAPCS").Visible, gridView.Columns("PDIAPCS").Width, 0) + IIf(gridView.Columns("PDIAWT").Visible, gridView.Columns("PDIAWT").Width, 0)
                .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += IIf(gridView.Columns("PSTNPCS").Visible, gridView.Columns("PSTNPCS").Width, 0) + IIf(gridView.Columns("PSTNCRWT").Visible, gridView.Columns("PSTNCRWT").Width, 0) + IIf(gridView.Columns("PSTNGRWT").Visible, gridView.Columns("PSTNGRWT").Width, 0)
                .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").Width += IIf(gridView.Columns("PVALUE").Visible, gridView.Columns("PVALUE").Width, 0) + IIf(gridView.Columns("PSALVALUE").Visible, gridView.Columns("PSALVALUE").Width, 0)
                .Columns("PTAGS~PPCS~PGRSWT~PNETWT" & strPExtraWt & "~PDIAPCS~PDIAWT~PSTNPCS~PSTNCRWT~PSTNGRWT~PTAGPCS~PVALUE~PSALVALUE").HeaderText = "PENDING"
            End If
            If chkSummary.Checked = False Then
                .Columns("RATE~STYLENO").Visible = gridView.Columns("RATE").Visible Or gridView.Columns("STYLENO").Visible
                .Columns("RATE~STYLENO").Width = IIf(gridView.Columns("RATE").Visible, gridView.Columns("RATE").Width, 0) _
                + IIf(gridView.Columns("STYLENO").Visible, gridView.Columns("STYLENO").Width, 0)
                .Columns("RATE~STYLENO").HeaderText = ""
                .Columns("RATE~STYLENO").HeaderText = ""
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("RATE~STYLENO").Visible = False
            End If
        End With
    End Function
    Function funcColWidth(ByVal rpflag As Boolean) As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            Dim strOExtraWt As String = ""
            Dim strRExtraWt As String = ""
            Dim strIExtraWt As String = ""
            Dim strARExtraWt As String = ""
            Dim strAIExtraWt As String = ""
            Dim strCExtraWt As String = ""
            If chkExtraWt.Checked Then strOExtraWt = "~OEXTRAWT"
            If chkExtraWt.Checked Then strRExtraWt = "~REXTRAWT"
            If chkExtraWt.Checked Then strIExtraWt = "~IEXTRAWT"
            If chkExtraWt.Checked Then strARExtraWt = "~AREXTRAWT"
            If chkExtraWt.Checked Then strAIExtraWt = "~AIEXTRAWT"
            If chkExtraWt.Checked Then strCExtraWt = "~CEXTRAWT"

            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width = gridView.Columns("OPCS").Width
            If chkGrsWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("OGRSWT").Width
            If chkNetWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("ONETWT").Width
            If chkExtraWt.Checked Then .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += gridView.Columns("OEXTRAWT").Width
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAPCS").Width, 0) + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) + IIf(gridView.Columns("OSTNCRWT").Visible, gridView.Columns("OSTNCRWT").Width, 0) + +IIf(gridView.Columns("OSTNGRWT").Visible, gridView.Columns("OSTNGRWT").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").Width += IIf(gridView.Columns("OVALUE").Visible, gridView.Columns("OVALUE").Width, 0)
            .Columns("OTAGS~OPCS~OGRSWT~ONETWT" & strOExtraWt & "~ODIAPCS~ODIAWT~OSTNPCS~OSTNCRWT~OSTNGRWT~OTAGPCS~OVALUE").HeaderText = "OPENING"

            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width = gridView.Columns("RPCS").Width
            If chkGrsWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("RGRSWT").Width
            If chkNetWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("RNETWT").Width
            If chkExtraWt.Checked Then .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += gridView.Columns("REXTRAWT").Width
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAPCS").Width, 0) + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RSTNPCS").Visible, gridView.Columns("RSTNPCS").Width, 0) + IIf(gridView.Columns("RSTNCRWT").Visible, gridView.Columns("RSTNCRWT").Width, 0) + IIf(gridView.Columns("RSTNGRWT").Visible, gridView.Columns("RSTNGRWT").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").Width += IIf(gridView.Columns("RVALUE").Visible, gridView.Columns("RVALUE").Width, 0)
            .Columns("RTAGS~RPCS~RGRSWT~RNETWT" & strRExtraWt & "~RDIAPCS~RDIAWT~RSTNPCS~RSTNCRWT~RSTNGRWT~RTAGPCS~RVALUE").HeaderText = "RECEIPT"

            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width = gridView.Columns("IPCS").Width
            If chkGrsWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("IGRSWT").Width
            If chkNetWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("INETWT").Width
            If chkExtraWt.Checked Then .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += gridView.Columns("IEXTRAWT").Width
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAPCS").Width, 0) + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("ISTNPCS").Visible, gridView.Columns("ISTNPCS").Width, 0) + IIf(gridView.Columns("ISTNCRWT").Visible, gridView.Columns("ISTNCRWT").Width, 0) + IIf(gridView.Columns("ISTNGRWT").Visible, gridView.Columns("ISTNGRWT").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").Width += IIf(gridView.Columns("IVALUE").Visible, gridView.Columns("IVALUE").Width, 0)
            .Columns("ITAGS~IPCS~IGRSWT~INETWT" & strIExtraWt & "~IDIAPCS~IDIAWT~ISTNPCS~ISTNCRWT~ISTNGRWT~ITAGPCS~IVALUE").HeaderText = "ISSUE"

            If chkSeperateColumnApproval.Checked Then
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width = gridView.Columns("ARPCS").Width
                If chkGrsWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARGRSWT").Width
                If chkNetWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("ARNETWT").Width
                If chkExtraWt.Checked Then .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += gridView.Columns("AREXTRAWT").Width
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAPCS").Width, 0) + IIf(gridView.Columns("ARDIAWT").Visible, gridView.Columns("ARDIAWT").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARSTNPCS").Visible, gridView.Columns("ARSTNPCS").Width, 0) + IIf(gridView.Columns("ARSTNCRWT").Visible, gridView.Columns("ARSTNCRWT").Width, 0) + IIf(gridView.Columns("ARSTNGRWT").Visible, gridView.Columns("ARSTNGRWT").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").Width += IIf(gridView.Columns("ARVALUE").Visible, gridView.Columns("ARVALUE").Width, 0)
                .Columns("ARTAGS~ARPCS~ARGRSWT~ARNETWT" & strARExtraWt & "~ARDIAPCS~ARDIAWT~ARSTNPCS~ARSTNCRWT~ARSTNGRWT~ARTAGPCS~ARVALUE").HeaderText = "APP REC"

                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width = gridView.Columns("AIPCS").Width
                If chkGrsWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AIGRSWT").Width
                If chkNetWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AINETWT").Width
                If chkExtraWt.Checked Then .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += gridView.Columns("AIEXTRAWT").Width
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0) + IIf(gridView.Columns("AIDIAWT").Visible, gridView.Columns("AIDIAWT").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AISTNPCS").Visible, gridView.Columns("AISTNPCS").Width, 0) + IIf(gridView.Columns("AISTNCRWT").Visible, gridView.Columns("AISTNCRWT").Width, 0) + IIf(gridView.Columns("AISTNGRWT").Visible, gridView.Columns("AISTNGRWT").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").Width += IIf(gridView.Columns("AIVALUE").Visible, gridView.Columns("AIVALUE").Width, 0)
                .Columns("AITAGS~AIPCS~AIGRSWT~AINETWT" & strAIExtraWt & "~AIDIAPCS~AIDIAWT~AISTNPCS~AISTNCRWT~AISTNGRWT~AITAGPCS~AIVALUE").HeaderText = "APP ISS"
            End If

            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width = gridView.Columns("CPCS").Width
            If chkGrsWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CGRSWT").Width
            If chkNetWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CNETWT").Width
            If chkExtraWt.Checked Then .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += gridView.Columns("CEXTRAWT").Width
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAPCS").Width, 0) + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CSTNPCS").Visible, gridView.Columns("CSTNPCS").Width, 0) + IIf(gridView.Columns("CSTNCRWT").Visible, gridView.Columns("CSTNCRWT").Width, 0) + IIf(gridView.Columns("CSTNGRWT").Visible, gridView.Columns("CSTNGRWT").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").Width += IIf(gridView.Columns("CVALUE").Visible, gridView.Columns("CVALUE").Width, 0)
            .Columns("CTAGS~CPCS~CGRSWT~CNETWT" & strCExtraWt & "~CDIAPCS~CDIAWT~CSTNPCS~CSTNCRWT~CSTNGRWT~CTAGPCS~CVALUE").HeaderText = "CLOSING"

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
        funcLoaddesigner()
        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
        ' chkDiamond.Checked = False
        ' chkStone.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemType, dtItemType, "NAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        strSql = " SELECT 'ALL' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'MANUFACTURING' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'TRADING' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'EXEMPTED' TRANS "
        Dim dtTran As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTran)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbStktype, dtTran, "TRANS", , "ALL")
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        cmbSubItemGroup.Items.Clear()
        strSql = vbCrLf + " SELECT DISTINCT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE ACTIVE='Y'"
        strSql += vbCrLf + " ORDER BY SGROUPNAME"
        Dim dtsubitemgrp As DataTable
        dtsubitemgrp = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsubitemgrp)
        cmbSubItemGroup.Items.Add("ALL", True)
        If dtsubitemgrp.Rows.Count > 0 Then
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemGroup, dtsubitemgrp, "SGROUPNAME", False, "ALL")
        End If
        cmbSubItemGroup.Text = "ALL"
        ' chkAsOnDate.Checked = True
        funcLoadItemName()
        ' chkWithApproval.Checked = False
        chkCmbMetal.Select()
        If Sepaccodeview = True Then
            ChkSepaccode.Visible = True
        Else
            ChkSepaccode.Visible = False
        End If
        If SelectionFormatNew = False Then Prop_Gets()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Dim IS40COLCLSSTKPRINT As Boolean = IIf(GetAdmindbSoftValue("40COLCLSSTKPRINT", "N") = "Y", True, False)
        If IS40COLCLSSTKPRINT Then
            If MsgBox("Do you want to print on 40 Col. Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then IS40COLCLSSTKPRINT = False
        End If
        If IS40COLCLSSTKPRINT Then
            Call PRINT40COLSUMMARY()
        Else
            If StoneDetail = True Then
                If gridviewDetail.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                    BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridviewDetail, BrightPosting.GExport.GExportType.Print, gridViewHead)
                End If
            Else
                If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                    BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
                End If
            End If
        End If
    End Sub
    Private Sub PRINT40COLSUMMARY()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim write As StreamWriter
        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        Dim lineprn As String = Space(40)
        write.WriteLine("--------------------------------------")
        write.WriteLine("ITEMWISE CLOSING STOCK")
        write.WriteLine("--------------------------------------")
        write.WriteLine("DESCRIPTION".PadRight(17, "") & Space(1) & "PCS".PadRight(5, "") & Space(1) & IIf(chkGrsWt.Checked, "GRS WT", "NET WT").PadRight(8, "") & Space(1) & "RATE".PadRight(7, ""))
        write.WriteLine("--------------------------------------")
        For i As Integer = 0 To gridView.Rows.Count - 1
            With gridView.Rows(i)
                Dim prndesc As String = Mid(.Cells("PARTICULAR").Value.ToString, 1, 17)
                Dim prnpcs As String = .Cells("CPcs").Value.ToString
                Dim prngwt As String = IIf(chkGrsWt.Checked, .Cells("CGRSWT").Value.ToString, .Cells("CNETWT").Value.ToString)
                Dim prnrate As String
                If Not IsDBNull(.Cells("Rate").Value.ToString) Then prnrate = IIf(Val(.Cells("RATE").Value.ToString) <> 0, Format(Val(.Cells("RATE").Value.ToString), "0.00"), "")

                prndesc = LSet(prndesc, 14)
                prnpcs = RSet(prnpcs, 5)
                prngwt = RSet(prngwt, 9)
                prnrate = RSet(prnrate, 9)
                If prndesc.Contains("TOTAL") Then write.WriteLine("--------------------------------------")
                write.WriteLine(prndesc & Space(1) & prnpcs & Space(1) & prngwt & Space(1) & prnrate)
                If prndesc.Contains("TOTAL") Then write.WriteLine("--------------------------------------")
                'write.WriteLine(prndesc.PadRight(17, "") & Space(1) & prnpcs.PadLeft(5, "") & Space(1) & prngwt.PadLeft(8, "") & Space(1) & prnrate.PadLeft(7, ""))
            End With
        Next
        For i As Integer = 0 To 4
            write.WriteLine()
        Next
        write.Close()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.BAT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.BAT")
        End If
        Dim writebat As StreamWriter

        Dim PrnName As String = ""
        Dim CondId As String = ""
        Try
            CondId = "'AGOLDREPORT40COLUMN" & Environ("NODE-ID").ToString & "'"
        Catch ex As Exception
            MsgBox("Set Node-Id", MsgBoxStyle.Information)
            Exit Sub
        End Try
        writebat = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.BAT")
        strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
        Dim dt As New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count <> 0 Then
            PrnName = dt.Rows(0).Item("CTLTEXT").ToString
        Else
            PrnName = "PRN"
        End If
        writebat.WriteLine("TYPE " & Application.StartupPath & "\SUMMARYPRINT.TXT>" & PrnName)
        writebat.Flush()
        writebat.Close()
        Shell(Application.StartupPath & "\SUMMARYPRINT.BAT")
    End Sub


    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DISABLE_ECISEDUTY Then
            Label18.Enabled = False
            chkCmbStktype.Enabled = False
        End If
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
        strSql = " SELECT 'ALL' GROUPNAME,'ALL' GROUPID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT GROUPNAME,CONVERT(vARCHAR,GROUPID),2 RESULT FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPTYPE='C'"
        strSql += " ORDER BY RESULT,GROUPNAME"
        dtgroupid = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtgroupid)
        BrighttechPack.GlobalMethods.FillCombo(chkcountergroup, dtgroupid, "GROUPNAME", , "ALL")
        ChkItemMode.Items.Add("ALL")
        ChkItemMode.Items.Add("WEIGHT")
        ChkItemMode.Items.Add("RATE")
        'ChkItemMode.Items.Add("BOTH")
        ChkItemMode.Items.Add("FIXED")
        ChkItemMode.Items.Add("METAL RATE")
        ChkItemMode.Text = "ALL"
        transitCheck()
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMMAST WHERE TAGWT='Y'"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) > 0 Then
            ChkTagWt.Visible = True
        Else
            ChkTagWt.Visible = False
        End If
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMMAST WHERE COVERWT='Y'"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) > 0 Then
            ChkCoverWt.Visible = True
        Else
            ChkCoverWt.Visible = False
        End If
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        btnNew_Click(Me, New EventArgs)
        If _HideBackOffice Then
            chkBoffStock.Visible = False
            chkBoffStockOnly.Visible = False
            chkBoffStock.Checked = False
            chkBoffStockOnly.Checked = False
        End If
        If Authorize = False Then
            PnlField1.Enabled = False
            PnlField2.Enabled = False
            PnlField3.Enabled = False
            PnlField4.Enabled = False
            chkWithNegativeStock.Enabled = False
            chkStoneVal.Enabled = False
            btnSave_OWN.Enabled = False

        End If
        rbtItemId.Checked = False
    End Sub
    Private Function transitCheck()
        If objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMTAG WHERE ISNULL(COSTID,'')='" & cnCostId & "'", , 0, ) > 0 Or objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMNONTAG WHERE ISNULL(COSTID,'')='" & cnCostId & "'", , 0, ) > 0 Then
            MsgBox("Stock available in In-Transit " & vbCrLf & vbCrLf & "Please download it", MsgBoxStyle.Information)
        End If
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
    End Sub

    Private Sub chkOnlyTag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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

    Private Sub chkDiamond_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkDiamond.Checked Or chkStone.Checked Then
            pnlDisStnResult.Enabled = True
            chkLoseStSepCol.Enabled = False
            chkLoseStSepCol.Checked = False
        Else
            pnlDisStnResult.Enabled = False
            chkLoseStSepCol.Enabled = True
        End If
    End Sub

    Private Sub chkStone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkDiamond.Checked Or chkStone.Checked Then
            pnlDisStnResult.Enabled = True
            chkLoseStSepCol.Enabled = False
            chkLoseStSepCol.Checked = False
        Else
            pnlDisStnResult.Enabled = False
            chkLoseStSepCol.Enabled = True
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

    Private Sub chkWithApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkOnlyApproval.Checked = True Then chkOnlyApproval.Checked = False
    End Sub

    Private Sub chkOnlyApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkWithApproval.Checked = True Then chkWithApproval.Checked = False
        If chkSeperateColumnApproval.Checked = True Then chkSeperateColumnApproval.Checked = False
    End Sub

    Private Sub chkSeperateColumnApproval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkSeperateColumnApproval.Checked Then
            rbtAll.Checked = True
            rbtRegular.Enabled = False
            rbtOrder.Enabled = False
        Else
            rbtRegular.Enabled = True
            rbtOrder.Enabled = True
        End If
    End Sub

    Private Sub chkSeperateColumnApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkOnlyApproval.Checked = True Then chkOnlyApproval.Checked = False
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseStock_Properties
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
        obj.p_chkExtraWt = chkExtraWt.Checked
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
        obj.p_chkShortname = chkShortname.Checked
        obj.p_chkWithTag = ChkWithTag.Checked
        obj.p_chkWithPendingTr = ChkPendingStk.Checked
        obj.p_ChkTagWt = ChkTagWt.Checked
        obj.p_ChkCoverWt = ChkCoverWt.Checked
        obj.p_rbtItemId = rbtItemId.Checked
        obj.p_rbtItemName = rbtItemName.Checked
        obj.p_ChkMuiltmetal = chkmultimetal.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseStock_Properties), Save)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseStock_Properties), IIf(Authorize = False, True, False))
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
        chkExtraWt.Checked = obj.p_chkExtraWt
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
        chkShortname.Checked = obj.p_chkShortname
        ChkWithTag.Checked = obj.p_chkWithTag
        ChkPendingStk.Checked = obj.p_chkWithPendingTr
        ChkTagWt.Checked = obj.p_ChkTagWt
        ChkCoverWt.Checked = obj.p_ChkCoverWt
        rbtItemId.Checked = obj.p_rbtItemId
        rbtItemName.Checked = obj.p_rbtItemName
        chkmultimetal.Checked = obj.p_ChkMuiltmetal
    End Sub

    Private Sub chkOnlyApproval_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkOnlyApproval.Checked = True Then chkSeperateColumnApproval.Checked = True
    End Sub

    Private Sub ChkItemMode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkItemMode.TextChanged
        funcLoadItemName()
    End Sub
    '01
    Private Sub StoneDetails()
        StoneDetail = True
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_ITEMWISESTONESTOCK"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
        If ChkItemMode.Text = "ALL" Then
            strSql += vbCrLf + " ,@ITEMTYPE = ''"
        Else
            strSql += vbCrLf + " ,@ITEMTYPE = '" & GetSelectedItemType(ChkItemMode, False) & "'"
        End If
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
        strSql += vbCrLf + " ,@ITEMTYPEIDS = '" & GetSelecteditemtypeid(chkCmbItemType, False) & "'"
        strSql += vbCrLf + " ,@DESIGNERIDS = '" & GetSelectedDesignerid(chkCmbDesigner, False) & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
        strSql += vbCrLf + " ,@COUNTERIDS = '" & GetSelectedCounderid(chkCmbCounter, False) & "'"
        strSql += vbCrLf + " ,@COSTIDS = '" & GetSelectedCostId(chkCmbCostCentre, False) & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='B'"
        ElseIf rbtTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='T'"
        ElseIf rbtNonTag.Checked = True Then
            strSql += vbCrLf + " ,@TAGTYPE='N'"
        End If
        strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHTR = '" & IIf(Chkwithtr.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHDIA='Y'"
        strSql += vbCrLf + " ,@WITHSTONE='Y'"
        strSql += vbCrLf + " ,@DIASTNBYROW = '" & IIf(rbtDiaStnByRow.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHAPP = '" & IIf(chkWithApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHSEPAPP = '" & IIf(chkSeperateColumnApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SHORTNAME = '" & IIf(chkShortname.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ORDERBY = '" & IIf(chkOrderbyItemId.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@HIDEBACKOFF = '" & IIf(_HideBackOffice = True, "Y", "N") & "'"
        If ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then
            strSql += vbCrLf + " ,@GROUPBYCOUNTER='Y'"
        Else
            strSql += vbCrLf + " ,@GROUPBYCOUNTER='N'"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then
            strSql += vbCrLf + " ,@GROUPBYDESIGNER='Y'"
        Else
            strSql += vbCrLf + " ,@GROUPBYDESIGNER='N'"
        End If
        If ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then
            strSql += vbCrLf + " ,@GROUPBYCOST='Y'"
        Else
            strSql += vbCrLf + " ,@GROUPBYCOST='N'"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)


        tabView.Show()
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        If dss.Tables.Contains("Table1") Then
            dt = dss.Tables(1)
        Else
            dt = dss.Tables(0)
        End If
        Dim dtSource As New DataTable
        dtSource = dt.Copy
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtSource)

        gridviewDetail.Visible = True
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridviewDetail, dtSource)
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        If chkWithGroupitem.Checked Then ObjGrouper.pColumns_Group.Add("GROUPNAME")
        If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
        ObjGrouper.pColumns_Sum.Add("ODIAPCS")
        ObjGrouper.pColumns_Sum.Add("ODIAWT")
        ObjGrouper.pColumns_Sum.Add("OSTNPCS")
        ObjGrouper.pColumns_Sum.Add("OSTNWT")
        ObjGrouper.pColumns_Sum.Add("OVALUE")
        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("APPODIAPCS")
            ObjGrouper.pColumns_Sum.Add("APPODIAWT")
            ObjGrouper.pColumns_Sum.Add("APPOSTNPCS")
            ObjGrouper.pColumns_Sum.Add("APPOSTNWT")
            ObjGrouper.pColumns_Sum.Add("APPOVALUE")
            'ObjGrouper.pColumns_Sum.Add("APPOSALVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("RDIAPCS")
        ObjGrouper.pColumns_Sum.Add("RDIAWT")
        ObjGrouper.pColumns_Sum.Add("RSTNPCS")
        ObjGrouper.pColumns_Sum.Add("RSTNWT")
        ObjGrouper.pColumns_Sum.Add("RVALUE")
        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("ARRDIAPCS")
            ObjGrouper.pColumns_Sum.Add("ARRDIAWT")
            ObjGrouper.pColumns_Sum.Add("ARRSTNPCS")
            ObjGrouper.pColumns_Sum.Add("ARRSTNWT")
            ObjGrouper.pColumns_Sum.Add("ARRVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("IDIAPCS")
        ObjGrouper.pColumns_Sum.Add("IDIAWT")
        ObjGrouper.pColumns_Sum.Add("ISTNPCS")
        ObjGrouper.pColumns_Sum.Add("ISTNWT")
        ObjGrouper.pColumns_Sum.Add("IVALUE")
        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("AIIDIAPCS")
            ObjGrouper.pColumns_Sum.Add("AIIDIAWT")
            ObjGrouper.pColumns_Sum.Add("AIISTNPCS")
            ObjGrouper.pColumns_Sum.Add("AIISTNWT")
            ObjGrouper.pColumns_Sum.Add("AIIVALUE")
        End If
        ObjGrouper.pColumns_Sum.Add("CDIAPCS")
        ObjGrouper.pColumns_Sum.Add("CDIAWT")
        ObjGrouper.pColumns_Sum.Add("CSTNPCS")
        ObjGrouper.pColumns_Sum.Add("CSTNWT")
        ObjGrouper.pColumns_Sum.Add("CVALUE")
        If chkSeperateColumnApproval.Checked Then
            ObjGrouper.pColumns_Sum.Add("APPCDIAPCS")
            ObjGrouper.pColumns_Sum.Add("APPCDIAWT")
            ObjGrouper.pColumns_Sum.Add("APPCSTNPCS")
            ObjGrouper.pColumns_Sum.Add("APPCSTNWT")
            ObjGrouper.pColumns_Sum.Add("APPCVALUE")
        End If
        gridviewDetail.DataSource = Nothing
        gridviewDetail.DataSource = dtSource
        Dim ind As Integer = gridviewDetail.RowCount - 1
        CType(gridviewDetail.DataSource, DataTable).Rows.Add()

        If HideSummary = False Then
            CType(gridviewDetail.DataSource, DataTable).Rows.Add()
            CType(gridviewDetail.DataSource, DataTable).Rows.Add()
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("ODIAPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("ODIAWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("OSTNPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("OSTNWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("OVALUE").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APP OPENING
                CType(gridviewDetail.DataSource, DataTable).Rows.Add()
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "APP-OPENING"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("APPODIAPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("APPODIAWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("APPOSTNPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("APPOSTNWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("APPOVALUE").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If

            ''RECEIPT
            CType(gridviewDetail.DataSource, DataTable).Rows.Add()
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("RDIAPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("RDIAWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("RSTNPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("RSTNWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("RVALUE").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APPROVAL RECEIPT
                CType(gridviewDetail.DataSource, DataTable).Rows.Add()
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "APP-RECEIPT"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("ARRDIAPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("ARRDIAWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("ARRSTNPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("ARRSTNWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("ARRVALUE").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If

            ''ISSUE
            CType(gridviewDetail.DataSource, DataTable).Rows.Add()
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("IDIAPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("IDIAWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("ISTNPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("ISTNWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("IVALUE").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            If chkSeperateColumnApproval.Checked Then
                ''APPROVAL ISSUE
                CType(gridviewDetail.DataSource, DataTable).Rows.Add()
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "APP-ISSUE"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("AIIDIAPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("AIIDIAWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("AIISTNPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("AIISTNWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("AIIVALUE").Value
            End If
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

            ''CLOSING
            CType(gridviewDetail.DataSource, DataTable).Rows.Add()
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("CDIAPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("CDIAWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("CSTNPCS").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("CSTNWT").Value
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("CVALUE").Value
            If chkSeperateColumnApproval.Checked Then
                CType(gridviewDetail.DataSource, DataTable).Rows.Add()
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("PARTICULAR").Value = "APP-CLOSING"
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAPCS").Value = gridviewDetail.Rows(ind).Cells("APPCDIAPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("ODIAWT").Value = gridviewDetail.Rows(ind).Cells("APPCDIAWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNPCS").Value = gridviewDetail.Rows(ind).Cells("APPCSTNPCS").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OSTNWT").Value = gridviewDetail.Rows(ind).Cells("APPCSTNWT").Value
                gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("OVALUE").Value = gridviewDetail.Rows(ind).Cells("APPCVALUE").Value
            End If
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridviewDetail.Rows(gridviewDetail.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If

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
        If cmbCategory.Text <> "ALL" Then lblTitle.Text += " [" & cmbCategory.Text & "] "
        If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
        If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridViewHeaderStyleDetail()
        GridStyleDetail()
        gridviewDetail.Columns("COLHEAD").Visible = False
        gridviewDetail.Columns("RESULT").Visible = False

        If chkSeperateColumnApproval.Checked Then
            gridviewDetail.Columns("APPODIAPCS").Visible = True
            gridviewDetail.Columns("APPODIAWT").Visible = True
            gridviewDetail.Columns("APPOSTNPCS").Visible = True
            gridviewDetail.Columns("APPOSTNWT").Visible = True
            gridviewDetail.Columns("APPODIAPCS").HeaderText = "APP-DIAPCS"
            gridviewDetail.Columns("APPODIAWT").HeaderText = "APP-CRT"
            gridviewDetail.Columns("APPOSTNPCS").HeaderText = "APP-STNPCS"
            gridviewDetail.Columns("APPOSTNWT").HeaderText = "APP-GRM"
            gridviewDetail.Columns("APPOVALUE").Visible = False
            gridviewDetail.Columns("ARRDIAPCS").Visible = True
            gridviewDetail.Columns("ARRDIAWT").Visible = True
            gridviewDetail.Columns("ARRSTNPCS").Visible = True
            gridviewDetail.Columns("ARRSTNWT").Visible = True
            gridviewDetail.Columns("ARRDIAPCS").HeaderText = "APP-DIAPCS"
            gridviewDetail.Columns("ARRDIAWT").HeaderText = "APP-CRT"
            gridviewDetail.Columns("ARRSTNPCS").HeaderText = "APP-STNPCS"
            gridviewDetail.Columns("ARRSTNWT").HeaderText = "APP-GRM"
            gridviewDetail.Columns("AIIDIAPCS").Visible = True
            gridviewDetail.Columns("AIIDIAWT").Visible = True
            gridviewDetail.Columns("AIISTNPCS").Visible = True
            gridviewDetail.Columns("AIISTNWT").Visible = True
            gridviewDetail.Columns("AIIDIAPCS").HeaderText = "APP-DIAPCS"
            gridviewDetail.Columns("AIIDIAWT").HeaderText = "APP-CRT"
            gridviewDetail.Columns("AIISTNPCS").HeaderText = "APP-STNPCS"
            gridviewDetail.Columns("AIISTNWT").HeaderText = "APP-GRM"
            gridviewDetail.Columns("APPCDIAPCS").Visible = True
            gridviewDetail.Columns("APPCDIAWT").Visible = True
            gridviewDetail.Columns("APPCSTNPCS").Visible = True
            gridviewDetail.Columns("APPCSTNWT").Visible = True
            gridviewDetail.Columns("APPCVALUE").Visible = False
            gridviewDetail.Columns("APPCDIAPCS").HeaderText = "APP-DIAPCS"
            gridviewDetail.Columns("APPCDIAWT").HeaderText = "APP-CRT"
            gridviewDetail.Columns("APPCSTNPCS").HeaderText = "APP-STNPCS"
            gridviewDetail.Columns("APPCSTNWT").HeaderText = "APP-GRM"
        End If
        If chkOnlyApproval.Checked = True Then
            If gridviewDetail.Columns.Contains("APPODIAPCS") Then gridviewDetail.Columns("APPODIAPCS").Visible = True
            If gridviewDetail.Columns.Contains("APPODIAWT") Then gridviewDetail.Columns("APPODIAWT").Visible = True
            If gridviewDetail.Columns.Contains("APPOSTNPCS") Then gridviewDetail.Columns("APPOSTNPCS").Visible = True
            If gridviewDetail.Columns.Contains("APPOSTNWT") Then gridviewDetail.Columns("APPOSTNWT").Visible = True
            If gridviewDetail.Columns.Contains("ARRDIAPCS") Then gridviewDetail.Columns("ARRDIAPCS").Visible = True
            If gridviewDetail.Columns.Contains("ARRDIAWT") Then gridviewDetail.Columns("ARRDIAWT").Visible = True
            If gridviewDetail.Columns.Contains("ARRSTNPCS") Then gridviewDetail.Columns("ARRSTNPCS").Visible = True
            If gridviewDetail.Columns.Contains("ARRSTNWT") Then gridviewDetail.Columns("ARRSTNWT").Visible = True
            If gridviewDetail.Columns.Contains("AIIDIAPCS") Then gridviewDetail.Columns("AIIDIAPCS").Visible = True
            If gridviewDetail.Columns.Contains("AIIDIAWT") Then gridviewDetail.Columns("AIIDIAWT").Visible = True
            If gridviewDetail.Columns.Contains("AIISTNPCS") Then gridviewDetail.Columns("AIISTNPCS").Visible = True
            If gridviewDetail.Columns.Contains("AIISTNWT") Then gridviewDetail.Columns("AIISTNWT").Visible = True
            'OPENING
            If gridviewDetail.Columns.Contains("ODIAPCS") Then gridviewDetail.Columns("ODIAPCS").Visible = False
            If gridviewDetail.Columns.Contains("ODIAWT") Then gridviewDetail.Columns("ODIAWT").Visible = False
            If gridviewDetail.Columns.Contains("OSTNPCS") Then gridviewDetail.Columns("OSTNPCS").Visible = False
            If gridviewDetail.Columns.Contains("OSTNWT") Then gridviewDetail.Columns("OSTNWT").Visible = False
            'gridViewDetail.Columns("OVALUE").Visible = True
            'RECEIPT
            If gridviewDetail.Columns.Contains("RDIAPCS") Then gridviewDetail.Columns("RDIAPCS").Visible = False
            If gridviewDetail.Columns.Contains("RDIAWT") Then gridviewDetail.Columns("RDIAWT").Visible = False
            If gridviewDetail.Columns.Contains("RSTNPCS") Then gridviewDetail.Columns("RSTNPCS").Visible = False
            If gridviewDetail.Columns.Contains("RSTNWT") Then gridviewDetail.Columns("RSTNWT").Visible = False
            'gridViewDetail.Columns("RVALUE").Visible = True
            'ISSUE
            If gridviewDetail.Columns.Contains("IDIAPCS") Then gridviewDetail.Columns("IDIAPCS").Visible = False
            If gridviewDetail.Columns.Contains("IDIAWT") Then gridviewDetail.Columns("IDIAWT").Visible = False
            If gridviewDetail.Columns.Contains("ISTNPCS") Then gridviewDetail.Columns("ISTNPCS").Visible = False
            If gridviewDetail.Columns.Contains("ISTNWT") Then gridviewDetail.Columns("ISTNWT").Visible = False
            'gridViewDetail.Columns("IVALUE").Visible = True
            'CLOSING
            If gridviewDetail.Columns.Contains("CDIAPCS") Then gridviewDetail.Columns("CDIAPCS").Visible = False
            If gridviewDetail.Columns.Contains("CDIAWT") Then gridviewDetail.Columns("CDIAWT").Visible = False
            If gridviewDetail.Columns.Contains("CSTNPCS") Then gridviewDetail.Columns("CSTNPCS").Visible = False
            gridviewDetail.Columns("CSTNWT").Visible = False
            ' gridViewDetail.Columns("CVALUE").Visible = True

            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("APPODIAPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("APPODIAWT").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("APPOSTNPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("APPOSTNWT").Visible = True
            'If chkSeperateColumnApproval.Checked Then gridViewDetail.Columns("APPOVALUE").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("ARRDIAPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("ARRDIAWT").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("ARRSTNPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("ARRSTNWT").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("AIIDIAPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("AIIDIAWT").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("AIISTNPCS").Visible = True
            If chkSeperateColumnApproval.Checked Then gridviewDetail.Columns("AIISTNWT").Visible = True
        End If
        ' gridviewDetail.DataSource = Nothing
        For i As Integer = 0 To gridviewDetail.Columns.Count - 1
            Select Case gridviewDetail.Columns(i).Name
                Case "ODIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "O-CRTPCS"
                Case "ODIAWT"
                    gridviewDetail.Columns(i).HeaderText = "O-CRT"
                Case "OSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "O-GRMPCS"
                Case "OSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "O-GRM"
                Case "RDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "R-CRTPCS"
                Case "RDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "R-CRT"
                Case "RSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "R-GRMPCS"
                Case "RSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "R-GRM"
                Case "IDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "I-CRTPCS"
                Case "IDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "I-CRT"
                Case "ISTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "I-GRMPCS"
                Case "ISTNWT"
                    gridviewDetail.Columns(i).HeaderText = "I-GRM"
                Case "CDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "C-CRTPCS"
                Case "CDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "C-CRT"
                Case "CSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "C-GRMPCS"
                Case "CSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "C-GRM"
                Case "APPDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRTPCS"
                Case "APPDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRT"
                Case "APPSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRMPCS"
                Case "APPSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRM"
                Case "ARRDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRTPCS"
                Case "ARRDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRT"
                Case "ARRSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRMPCS"
                Case "ARRSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRM"
                Case "AIIDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRTPCS"
                Case "AIIDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRT"
                Case "AIISTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRMPCS"
                Case "AIISTNWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRM"
                Case "APPCDIAPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRTPCS"
                Case "APPCDIAWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-CRT"
                Case "APPCSTNPCS"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRMPCS"
                Case "APPCSTNWT"
                    gridviewDetail.Columns(i).HeaderText = "APP-GRM"
            End Select
        Next
        GridViewHeaderStyleDetail()
        tabMain.SelectedTab = tabView
        GridViewFormatDetail()
    End Sub  '01
    '01

    Private Sub HallmarkDetails()

        MsgBox("Only Date,Costid Filteration Available", MsgBoxStyle.Information)
        HMDetail = True
        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTKHMSUMM')>0"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM"
        strSql += vbCrLf + " DECLARE @FRMDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + " SET @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SET @TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " SET @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SET @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " SELECT CASE WHEN [TYPE]='WH' THEN 'WITH HALLMARK' ELSE 'WITHOUT HALLMARK' END HMTYPE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=Z.METALID) METAL"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=Z.COSTID) COSTCENTRE"
        strSql += vbCrLf + " ,SUM(ISNULL(OPN_GRSWT,0)) OPN_GRSWT,SUM(ISNULL(OPN_NETWT,0)) AS OPN_NETWT"
        strSql += vbCrLf + " ,SUM(ISNULL(REC_GRSWT,0)) REC_GRSWT,SUM(ISNULL(REC_NETWT,0)) AS REC_NETWT"
        strSql += vbCrLf + " ,SUM(ISNULL(ROT_GRSWT,0)) ROT_GRSWT,SUM(ISNULL(ROT_NETWT,0)) AS ROT_NETWT"
        strSql += vbCrLf + " ,SUM(ISNULL(SAL_GRSWT,0)) SAL_GRSWT,SUM(ISNULL(SAL_NETWT,0)) AS SAL_NETWT"
        strSql += vbCrLf + " ,SUM(ISNULL(OTH_GRSWT,0)) IOT_GRSWT,SUM(ISNULL(OTH_NETWT,0)) AS IOT_NETWT"
        strSql += vbCrLf + " ,SUM((ISNULL(OPN_GRSWT,0)+ISNULL(REC_GRSWT,0)+ISNULL(ROT_GRSWT,0))-(ISNULL(SAL_GRSWT,0)+ISNULL(OTH_GRSWT,0))) CLS_GRSWT"
        strSql += vbCrLf + " ,SUM((ISNULL(OPN_NETWT,0)+ISNULL(REC_NETWT,0)+ISNULL(ROT_NETWT,0))-(ISNULL(SAL_NETWT,0)+ISNULL(OTH_NETWT,0))) AS CLS_NETWT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM "
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        If rbtBoth.Checked = True Or rbtTag.Checked = True Then
            strSql += vbCrLf + " -- OPENING WH"
            strSql += vbCrLf + " SELECT 'WH' AS TYPE,B.METALID,A.COSTID,TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT) OPN_GRSWT,SUM(NETWT) OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE < @FRMDATE AND (ISSDATE >= @FRMDATE OR ISSDATE IS NULL)"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,A.COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'WH' AS TYPE,B.METALID,A.COSTID,TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT) OPN_GRSWT,SUM(NETWT) OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE < @FRMDATE AND (ISSDATE >= @FRMDATE OR ISSDATE IS NULL)"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,A.COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- OPENING NH"
            strSql += vbCrLf + " SELECT 'NH' AS TYPE,B.METALID,A.COSTID,TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT) OPN_GRSWT,SUM(NETWT) OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE < @FRMDATE AND (ISSDATE >= @FRMDATE OR ISSDATE IS NULL)"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,A.COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' AS TYPE,B.METALID,A.COSTID,TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT) OPN_GRSWT,SUM(NETWT) OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE < @FRMDATE AND (ISSDATE >= @FRMDATE OR ISSDATE IS NULL)"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,A.COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- RECEIPT WH REC"
            strSql += vbCrLf + " SELECT 'WH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) REC_GRSWT,SUM(NETWT) REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') <> 'TR'"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'WH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) REC_GRSWT,SUM(NETWT) REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID "
            strSql += vbCrLf + " AND RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') <> 'TR'"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- RECEIPT NH REC"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) REC_GRSWT,SUM(NETWT) REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') <> 'TR'"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) REC_GRSWT,SUM(NETWT) REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') <> 'TR'"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- RECEIPT WH TR"
            strSql += vbCrLf + " SELECT 'WH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) ROT_GRSWT,SUM(NETWT) ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') = 'TR'"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'WH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) ROT_GRSWT,SUM(NETWT) ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID "
            strSql += vbCrLf + " AND RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') = 'TR'"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- RECEIPT NH OTH"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) ROT_GRSWT,SUM(NETWT) ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') = 'TR'"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) ROT_GRSWT,SUM(NETWT) ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " RECDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'') = 'TR'"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " --SALE WH"
            strSql += vbCrLf + " SELECT 'WH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) SAL_GRSWT,SUM(NETWT) SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG = 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'WH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) SAL_GRSWT,SUM(NETWT) SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG = 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " --SALE WH"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) SAL_GRSWT,SUM(NETWT) SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG = 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) SAL_GRSWT,SUM(NETWT) SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG = 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- TRANSFER / OTHERISSUE WH"
            strSql += vbCrLf + " SELECT 'WH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) OTH_GRSWT,SUM(NETWT) OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG <> 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'WH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) OTH_GRSWT,SUM(NETWT) OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG <> 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " -- TRANSFER / OTHERISSUE WH"
            strSql += vbCrLf + " SELECT 'NH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) OTH_GRSWT,SUM(NETWT) OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG <> 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE, B.METALID,COSTID,TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) OTH_GRSWT,SUM(NETWT) OTH_NETWT"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " " & cnAdminDb & "..CITEMTAG A," & cnAdminDb & "..ITEMMAST B"
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND "
            strSql += vbCrLf + " TOFLAG <> 'SA' AND "
            strSql += vbCrLf + " RECDATE <= @TODATE AND ISSDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK)"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID,TOFLAG "
        End If
        If rbtBoth.Checked = True Or rbtNonTag.Checked = True Then
            If rbtBoth.Checked = True Then strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,'' TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT) OPN_GRSWT,SUM(NETWT) OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG A, " & cnAdminDb & "..ITEMMAST B "
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " AND RECDATE < @FRMDATE AND RECISS = 'R'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID  "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,'' TOFLAG,"
            strSql += vbCrLf + " SUM(A.GRSWT)*-1 OPN_GRSWT,SUM(NETWT)*-1 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG A, " & cnAdminDb & "..ITEMMAST B "
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " AND RECDATE < @FRMDATE AND RECISS = 'I'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID  "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,'' TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) REC_GRSWT,SUM(NETWT) REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " 0 SAL_GRSWT,0 SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG A, " & cnAdminDb & "..ITEMMAST B "
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " AND RECDATE BETWEEN @TODATE AND @TODATE AND RECISS = 'R'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID  "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'NH' TYPE,B.METALID,COSTID,'' TOFLAG,"
            strSql += vbCrLf + " 0 OPN_GRSWT,0 OPN_NETWT,"
            strSql += vbCrLf + " 0 REC_GRSWT,0 REC_NETWT,"
            strSql += vbCrLf + " 0 ROT_GRSWT,0 ROT_NETWT,"
            strSql += vbCrLf + " SUM(A.GRSWT) SAL_GRSWT,SUM(NETWT) SAL_NETWT,"
            strSql += vbCrLf + " 0 OTH_GRSWT,0 OTH_NETWT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG A, " & cnAdminDb & "..ITEMMAST B "
            strSql += vbCrLf + " WHERE A.ITEMID = B.ITEMID AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " AND RECDATE BETWEEN @TODATE AND @TODATE AND RECISS = 'I'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then strSql += vbCrLf + " AND B.METALID IN ( '" & GetSelectedMetalid(chkCmbMetal, False).ToString.Replace(",", "','") & "')"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND A.COSTID IN ( '" & GetSelectedCostId(chkCmbCostCentre, False).ToString.Replace(",", "','") & "')"
            strSql += vbCrLf + " GROUP BY METALID,COSTID  "
        End If
        strSql += vbCrLf + " ) Z"
        strSql += vbCrLf + " GROUP BY TYPE,METALID,COSTID"
        strSql += vbCrLf + " ORDER BY TYPE DESC,METALID,COSTID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM"
        strSql = "SELECT * FROM ("
        strSql += vbCrLf + " SELECT DISTINCT HMTYPE PARTICULAR, NULL OPN_GRSWT, NULL OPN_NETWT "
        strSql += vbCrLf + " ,NULL REC_GRSWT,NULL REC_NETWT,NULL ROT_GRSWT,NULL ROT_NETWT "
        strSql += vbCrLf + " ,NULL SAL_GRSWT,NULL SAL_NETWT,NULL IOT_GRSWT,NULL IOT_NETWT "
        strSql += vbCrLf + " ,NULL CLS_GRSWT,NULL CLS_NETWT,0 RESULT,HMTYPE,'' COSTCENTRE "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT DISTINCT COSTCENTRE PARTICULAR "
        strSql += vbCrLf + " ,SUM(OPN_GRSWT)OPN_GRSWT,SUM(OPN_NETWT)OPN_NETWT "
        strSql += vbCrLf + " ,SUM(REC_GRSWT)REC_GRSWT,SUM(REC_NETWT)REC_NETWT,SUM(ROT_GRSWT)ROT_GRSWT,SUM(ROT_NETWT)ROT_NETWT "
        strSql += vbCrLf + " ,SUM(SAL_GRSWT)SAL_GRSWT,SUM(SAL_NETWT)SAL_NETWT,SUM(IOT_GRSWT)IOT_GRSWT,SUM(IOT_NETWT)IOT_NETWT "
        strSql += vbCrLf + " ,SUM(CLS_GRSWT)CLS_GRSWT,SUM(CLS_NETWT)CLS_NETWT"
        strSql += vbCrLf + " ,1 RESULT,HMTYPE,COSTCENTRE "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM GROUP BY HMTYPE,COSTCENTRE "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT LTRIM(HMTYPE) + ' TOTAL' PARTICULAR "
        strSql += vbCrLf + " ,SUM(OPN_GRSWT)OPN_GRSWT,SUM(OPN_NETWT)OPN_NETWT "
        strSql += vbCrLf + " ,SUM(REC_GRSWT)REC_GRSWT,SUM(REC_NETWT)REC_NETWT,SUM(ROT_GRSWT)ROT_GRSWT,SUM(ROT_NETWT)ROT_NETWT "
        strSql += vbCrLf + " ,SUM(SAL_GRSWT)SAL_GRSWT,SUM(SAL_NETWT)SAL_NETWT,SUM(IOT_GRSWT)IOT_GRSWT,SUM(IOT_NETWT)IOT_NETWT "
        strSql += vbCrLf + " ,SUM(CLS_GRSWT)CLS_GRSWT,SUM(CLS_NETWT)CLS_NETWT"
        strSql += vbCrLf + " ,2 RESULT,HMTYPE,'ZZZZZZZZZZ' COSTCENTRE "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM GROUP BY HMTYPE "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT 'GRAND TOTAL' PARTICULAR "
        strSql += vbCrLf + " ,SUM(OPN_GRSWT)OPN_GRSWT,SUM(OPN_NETWT)OPN_NETWT "
        strSql += vbCrLf + " ,SUM(REC_GRSWT)REC_GRSWT,SUM(REC_NETWT)REC_NETWT,SUM(ROT_GRSWT)ROT_GRSWT,SUM(ROT_NETWT)ROT_NETWT "
        strSql += vbCrLf + " ,SUM(SAL_GRSWT)SAL_GRSWT,SUM(SAL_NETWT)SAL_NETWT,SUM(IOT_GRSWT)IOT_GRSWT,SUM(IOT_NETWT)IOT_NETWT "
        strSql += vbCrLf + " ,SUM(CLS_GRSWT)CLS_GRSWT,SUM(CLS_NETWT)CLS_NETWT"
        strSql += vbCrLf + " ,3 RESULT,'ZZZZZZZZZZ' HMTYPE,'ZZZZZZZZZZ' COSTCENTRE "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTKHMSUMM "
        strSql += vbCrLf + " )X ORDER BY HMTYPE,COSTCENTRE,RESULT "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)

        tabView.Show()

        Dim dt As New DataTable
        da.Fill(dt)

        Dim dtSource As New DataTable
        dtSource = dt.Copy
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtSource)

        gridviewDetail.Visible = True


        gridviewDetail.DataSource = Nothing
        gridviewDetail.DataSource = dtSource
        HMGridStyle()
        HMGridViewHeaderStyle()

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
        If chkCmbCostCentre.Text <> "ALL" Then lblTitle.Text += " [" & chkCmbCostCentre.Text & "] "
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        tabMain.SelectedTab = tabView
        HMGridStyle()
        HMGridViewHeaderStyle()
    End Sub

    Private Sub HMGridStyle()
        FormatGridColumns(gridviewDetail, False, , , False)
        gridviewDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridviewDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewDetail.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridviewDetail

            .Columns("KEYNO").Visible = False
            If .Columns.Contains("HMTYPE") Then .Columns("HMTYPE").Visible = False
            If .Columns.Contains("COSTCENTRE") Then .Columns("COSTCENTRE").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            .Columns("PARTICULAR").Width = 250
            .Columns("PARTICULAR").Visible = True


            .Columns("OPN_GRSWT").Width = 125
            .Columns("OPN_GRSWT").Visible = True
            .Columns("OPN_NETWT").Width = 125
            .Columns("OPN_NETWT").Visible = chkNetWt.Checked

            .Columns("REC_GRSWT").Width = 125
            .Columns("REC_GRSWT").Visible = True
            .Columns("REC_NETWT").Width = 125
            .Columns("REC_NETWT").Visible = chkNetWt.Checked
            .Columns("ROT_GRSWT").Width = 125
            .Columns("ROT_GRSWT").Visible = True
            .Columns("ROT_NETWT").Width = 125
            .Columns("ROT_NETWT").Visible = chkNetWt.Checked

            .Columns("SAL_GRSWT").Width = 125
            .Columns("SAL_GRSWT").Visible = True
            .Columns("SAL_NETWT").Width = 125
            .Columns("SAL_NETWT").Visible = chkNetWt.Checked
            .Columns("IOT_GRSWT").Width = 125
            .Columns("IOT_GRSWT").Visible = True
            .Columns("IOT_NETWT").Width = 125
            .Columns("IOT_NETWT").Visible = chkNetWt.Checked

            .Columns("CLS_GRSWT").Width = 125
            .Columns("CLS_GRSWT").Visible = True
            .Columns("CLS_NETWT").Width = 125
            .Columns("CLS_NETWT").Visible = chkNetWt.Checked

            .Columns("OPN_GRSWT").HeaderText = "GRSWT"
            .Columns("OPN_NETWT").HeaderText = "NETWT"

            .Columns("REC_GRSWT").HeaderText = "GRSWT"
            .Columns("REC_NETWT").HeaderText = "NETWT"
            .Columns("ROT_GRSWT").HeaderText = "OTH GRSWT"
            .Columns("ROT_NETWT").HeaderText = "OTH NETWT"

            .Columns("SAL_GRSWT").HeaderText = "SALES GRSWT"
            .Columns("SAL_NETWT").HeaderText = "SALES NETWT"
            .Columns("IOT_GRSWT").HeaderText = "OTH GRSWT"
            .Columns("IOT_NETWT").HeaderText = "OTH NETWT"

            .Columns("CLS_GRSWT").HeaderText = "GRSWT"
            .Columns("CLS_NETWT").HeaderText = "NETWT"

        End With
    End Sub
    Private Sub HMGridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")

        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPN_GRSWT" & IIf(chkNetWt.Checked, "~OPN_NETWT", ""), GetType(String))
            .Columns.Add("REC_GRSWT" & IIf(chkNetWt.Checked, "~REC_NETWT", "") & "~ROT_GRSWT" & IIf(chkNetWt.Checked, "~ROT_NETWT", ""), GetType(String))
            .Columns.Add("SAL_GRSWT" & IIf(chkNetWt.Checked, "~SAL_NETWT", "") & "~IOT_GRSWT" & IIf(chkNetWt.Checked, "~IOT_NETWT", ""), GetType(String))
            .Columns.Add("CLS_GRSWT" & IIf(chkNetWt.Checked, "~CLS_NETWT", ""), GetType(String))
            .Columns.Add("SCROLL", GetType(String))

            .Columns("PARTICULAR").Caption = ""
            .Columns("OPN_GRSWT" & IIf(chkNetWt.Checked, "~OPN_NETWT", "")).Caption = "OPENING"
            .Columns("REC_GRSWT" & IIf(chkNetWt.Checked, "~REC_NETWT", "") & "~ROT_GRSWT" & IIf(chkNetWt.Checked, "~ROT_NETWT", "")).Caption = "RECEIPT"
            .Columns("SAL_GRSWT" & IIf(chkNetWt.Checked, "~SAL_NETWT", "") & "~IOT_GRSWT" & IIf(chkNetWt.Checked, "~IOT_NETWT", "")).Caption = "ISSUE"
            .Columns("CLS_GRSWT" & IIf(chkNetWt.Checked, "~CLS_NETWT", "")).Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            HMfuncColWidth()
            gridviewDetail.Focus()
            Dim colWid As Integer = 0
            If gridviewDetail.Columns.Contains("RESULT") Then
                For cnt As Integer = 0 To gridviewDetail.Rows.Count - 1
                    If Val(gridviewDetail.Rows(cnt).Cells("RESULT").Value.ToString) = 0 Then
                        gridviewDetail.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                    ElseIf Val(gridviewDetail.Rows(cnt).Cells("RESULT").Value.ToString) = 2 Then
                        gridviewDetail.Rows(cnt).DefaultCellStyle.BackColor = Color.Lavender
                    ElseIf Val(gridviewDetail.Rows(cnt).Cells("RESULT").Value.ToString) = 3 Then
                        gridviewDetail.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                        gridviewDetail.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                Next
            End If

            For cnt As Integer = 0 To gridviewDetail.ColumnCount - 1
                If gridviewDetail.Columns(cnt).Visible Then colWid += gridviewDetail.Columns(cnt).Width
            Next

            If colWid >= gridviewDetail.Width Then
                .Columns("SCROLL").Visible = CType(gridviewDetail.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridviewDetail.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub

    Function HMfuncColWidth() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridviewDetail.Columns("PARTICULAR").Width
            .Columns("PARTICULAR").HeaderText = "PARTICULARS"

            .Columns("OPN_GRSWT" & IIf(chkNetWt.Checked, "~OPN_NETWT", "")).Width = gridviewDetail.Columns("OPN_GRSWT").Width + IIf(chkNetWt.Checked, gridviewDetail.Columns("OPN_NETWT").Width, 0)
            .Columns("OPN_GRSWT" & IIf(chkNetWt.Checked, "~OPN_NETWT", "")).HeaderText = "OPENING"

            .Columns("REC_GRSWT" & IIf(chkNetWt.Checked, "~REC_NETWT", "") & "~ROT_GRSWT" & IIf(chkNetWt.Checked, "~ROT_NETWT", "")).Width = gridviewDetail.Columns("REC_GRSWT").Width _
                + IIf(chkNetWt.Checked, gridviewDetail.Columns("REC_NETWT").Width, 0) + gridviewDetail.Columns("ROT_GRSWT").Width + IIf(chkNetWt.Checked, gridviewDetail.Columns("ROT_NETWT").Width, 0)
            .Columns("REC_GRSWT" & IIf(chkNetWt.Checked, "~REC_NETWT", "") & "~ROT_GRSWT" & IIf(chkNetWt.Checked, "~ROT_NETWT", "")).HeaderText = "RECEIPT"

            .Columns("SAL_GRSWT" & IIf(chkNetWt.Checked, "~SAL_NETWT", "") & "~IOT_GRSWT" & IIf(chkNetWt.Checked, "~IOT_NETWT", "")).Width = gridviewDetail.Columns("SAL_GRSWT").Width _
                + IIf(chkNetWt.Checked, gridviewDetail.Columns("SAL_NETWT").Width, 0) + gridviewDetail.Columns("IOT_GRSWT").Width + IIf(chkNetWt.Checked, gridviewDetail.Columns("IOT_NETWT").Width, 0)
            .Columns("SAL_GRSWT" & IIf(chkNetWt.Checked, "~SAL_NETWT", "") & "~IOT_GRSWT" & IIf(chkNetWt.Checked, "~IOT_NETWT", "")).HeaderText = "ISSUE"

            .Columns("CLS_GRSWT" & IIf(chkNetWt.Checked, "~CLS_NETWT", "")).Width = gridviewDetail.Columns("CLS_GRSWT").Width + IIf(chkNetWt.Checked, gridviewDetail.Columns("CLS_NETWT").Width, 0)
            .Columns("CLS_GRSWT" & IIf(chkNetWt.Checked, "~CLS_NETWT", "")).HeaderText = "CLOSING"

            .Columns("SCROLL").Visible = CType(gridviewDetail.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridviewDetail.Controls(1), VScrollBar).Width
        End With
    End Function

    Private Sub gridviewDetail_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridviewDetail.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            If HMDetail = True Then
                HMGridViewHeaderStyle()
                Exit Sub
            Else
                Exit Sub
            End If
            funcColWidthDetail()
        End If
    End Sub  '01
    '01
    Private Sub gridviewDetail_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridviewDetail.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridviewDetail.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridviewDetail.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub '01
    '01
    Private Sub gridviewDetail_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridviewDetail.DataError

    End Sub '01
    ' 01
    Private Sub ResizeDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeDetailTooStripMenuItem.Click
        If gridviewDetail.RowCount > 0 Then
            If ResizeDetailTooStripMenuItem.Checked Then
                gridviewDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridviewDetail.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridviewDetail.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridviewDetail.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub '01
    '01
    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.T Then
            If chkCmbMetal.Text = "ALL" Or chkCmbMetal.Text = "" Then
                StoneDetails()
            Else
                MsgBox("Select All Metal...", MsgBoxStyle.Information)
            End If
        ElseIf e.KeyCode = Keys.H Then
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" And Not chkCmbMetal.Text.Contains(",") Then
                HallmarkDetails()
            Else
                MsgBox("Select One Metal...", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub chkSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If chkSummary.Checked = True Then rbtDiaStnByColumn.Checked = True
        If chkSummary.Checked = True Then chkOnlyApproval.Checked = False
    End Sub

    Private Sub rbtDiaStnByRow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rbtDiaStnByRow.Checked = True Then chkSummary.Checked = False
    End Sub

    Private Sub chkOnlyApproval_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If chkOnlyApproval.Checked = True Then chkSummary.Checked = False
    End Sub

    Private Sub btnSave_OWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub

    Public Sub FillCombo(ByVal Combo As ComboBox, ByVal DtSource As DataTable, ByVal FillFieldName As String, Optional ByVal Clear As Boolean = True)
        If Clear Then Combo.Items.Clear()
        For Each Row As DataRow In DtSource.Rows
            Combo.Items.Add(Row.Item(FillFieldName).ToString)
        Next
    End Sub

    Private Sub chkDesigner_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesigner.CheckedChanged
        If chkDesigner.Checked Then
            cmbDesigner.Visible = False
            chkCmbDesigner.Visible = True
        Else
            chkCmbDesigner.Visible = False
            cmbDesigner.Visible = True
        End If
    End Sub
    Private Sub chkCmbCounter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbCounter.GotFocus

    End Sub

    Private Sub chkcountergroup_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcountergroup.KeyDown
        If e.KeyCode = Keys.Enter Then
            If chkcountergroup.Text <> "ALL" And chkcountergroup.Text <> "" Then
                chkCmbCounter.Items.Clear()
                strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
                strSql += " UNION ALL"
                strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
                strSql += " WHERE CTRGROUP IN( SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID IN(" & GetSelectedCoundergrpid(chkcountergroup, True) & "))"
                strSql += " ORDER BY RESULT,ITEMCTRNAME"
                dtCounter = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCounter)
                BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
            End If
        End If
    End Sub

    Private Sub chkmuiltmetal_CheckedChanged(sender As Object, e As EventArgs) Handles chkmultimetal.CheckedChanged
        If chkmultimetal.Checked Then
            rbtItemId.Checked = False
            rbtItemName.Checked = False
        End If
    End Sub

    Private Sub chkCmbMetal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkCmbMetal.SelectedIndexChanged

    End Sub
End Class

Public Class frmItemWiseStock_Properties
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

    Private chkShortname As Boolean = False
    Public Property p_chkShortname() As Boolean
        Get
            Return chkShortname
        End Get
        Set(ByVal value As Boolean)
            chkShortname = value
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
    Private ChkCoverWt As Boolean = False
    Private chkmuiltmetal As Boolean = False
    Public Property p_ChkMuiltmetal() As Boolean
        Get
            Return chkmuiltmetal
        End Get
        Set(ByVal value As Boolean)
            chkmuiltmetal = value
        End Set
    End Property
    Public Property p_ChkCoverWt() As Boolean
        Get
            Return ChkCoverWt
        End Get
        Set(ByVal value As Boolean)
            ChkCoverWt = value
        End Set
    End Property
    Private ChkTagWt As Boolean = False
    Public Property p_ChkTagWt() As Boolean
        Get
            Return ChkTagWt
        End Get
        Set(ByVal value As Boolean)
            ChkTagWt = value
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
    Private chkWithTag As Boolean = False
    Public Property p_chkWithTag() As Boolean
        Get
            Return chkWithTag
        End Get
        Set(ByVal value As Boolean)
            chkWithTag = value
        End Set
    End Property
    Private chkWithPendingTr As Boolean = False
    Public Property p_chkWithPendingTr() As Boolean
        Get
            Return chkWithPendingTr
        End Get
        Set(ByVal value As Boolean)
            chkWithPendingTr = value
        End Set
    End Property

    Private rbtItemId As Boolean = False
    Public Property p_rbtItemId() As Boolean
        Get
            Return rbtItemId
        End Get
        Set(ByVal value As Boolean)
            rbtItemId = value
        End Set
    End Property

    Private rbtItemName As Boolean = False
    Public Property p_rbtItemName() As Boolean
        Get
            Return rbtItemName
        End Get
        Set(ByVal value As Boolean)
            rbtItemName = value
        End Set
    End Property
End Class