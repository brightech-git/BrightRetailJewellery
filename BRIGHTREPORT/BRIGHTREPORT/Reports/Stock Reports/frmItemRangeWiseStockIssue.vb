Imports System.Data.OleDb
Imports System.Xml
Imports System.IO
Imports com.ms.win32
Imports System.Data.SqlClient
Public Class frmItemRangeWiseStockIssue
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
    Dim dtGrid As New DataTable()
    Dim DiaRnd As Integer = 3
    Dim StoneDetail As Boolean = False
    Dim itemid As String = ""
    Dim subitemid As String = ""
    Dim costids As String = ""
    Dim dtrange As DataTable
    Dim IS40COLCLSSTKPRINT As Boolean = IIf(GetAdmindbSoftValue("40COLCLSSTKPRINT", "N") = "Y", True, False)
    Dim dtDetail As DataTable

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
        If ChkItemMode.Text <> "ALL" And ChkItemMode.Text <> "" Then
            strSql += " AND CALTYPE='" & Mid(ChkItemMode.Text, 1, 1) & "'"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
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
    Public Function GetSelectedRange(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += chkLst.CheckedItems.Item(cnt).ToString
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
    Private Sub SpReport()
        ResizeToolStripMenuItem.Checked = False
        Try
            Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..RPT_ITEMWISESTOCK_RANGE_ISSUE"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            strSql += vbCrLf + " ,@CATCODE ='" & GetSelectedCatCode(cmbCategory, False) & "'"
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
            strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
            If rbtBoth.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='B'"
            ElseIf rbtTag.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='T'"
            ElseIf rbtNonTag.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='N'"
            End If
            strSql += vbCrLf + " ,@WITHSUBITEM = 'Y'"
            strSql += vbCrLf + " ,@WITHDIA='N'"
            strSql += vbCrLf + " ,@WITHSTONE='N'"
            If chkcmbrange.Text.ToString.Trim = "ALL" Or chkcmbrange.Text.ToString.Trim = "" Then
                strSql += vbCrLf + " ,@RANGE='ALL'"
            Else
                strSql += vbCrLf + " ,@RANGE='" & GetSelectedRange(chkcmbrange, False) & "'"
            End If
            strSql += vbCrLf + " ,@WITHRANGEOUT='" & IIf(chkRangeOut.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@RPTBASED='" & IIf(rbtItem.Checked, "I", "S") & "'"
            strSql += vbCrLf + " ,@TRANONLY='" & IIf(ChkTransPrint.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@ISSUEONLY='" & IIf(ChkIssueOnly.Checked, "Y", "N") & "'"
            cmd = New OleDb.OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 100000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " EXEC " & cnAdminDb & "..RPT_ITEMWISESTOCK_RANGE_ISSUE_DETAIL"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            strSql += vbCrLf + " ,@CATCODE ='" & GetSelectedCatCode(cmbCategory, False) & "'"
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
            strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "De'"
            If rbtBoth.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='B'"
            ElseIf rbtTag.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='T'"
            ElseIf rbtNonTag.Checked = True Then
                strSql += vbCrLf + " ,@TAGTYPE='N'"
            End If
            strSql += vbCrLf + " ,@WITHSUBITEM = 'Y'"
            strSql += vbCrLf + " ,@WITHDIA='N'"
            strSql += vbCrLf + " ,@WITHSTONE='N'"
            If chkcmbrange.Text.ToString.Trim = "ALL" Or chkcmbrange.Text.ToString.Trim = "" Then
                strSql += vbCrLf + " ,@RANGE='ALL'"
            Else
                strSql += vbCrLf + " ,@RANGE='" & GetSelectedRange(chkcmbrange, False) & "'"
            End If
            strSql += vbCrLf + " ,@WITHRANGEOUT='" & IIf(chkRangeOut.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@RPTBASED='" & IIf(rbtItem.Checked, "I", "S") & "'"
            strSql += vbCrLf + " ,@TRANONLY='" & IIf(ChkTransPrint.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@ISSUEONLY='" & IIf(ChkIssueOnly.Checked, "Y", "N") & "'"
            cmd = New OleDb.OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 100000
            cmd.ExecuteNonQuery()

            If chkZero.Checked Then
                strSql = "DELETE from TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN where ISNULL(cpcs,0) <> 0"
                strSql += vbCrLf + " DELETE from TEMPTABLEDB..TEMP" & SYSTEMID & "DeITEMSTKFIN where ISNULL(cpcs,0) <> 0"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()
            End If

            If ChkIssueOnly.Checked Then
                If rbtSubItem.Checked Then
                    strSql = ";WITH CTE AS"
                    strSql += vbCrLf + "("
                    strSql += vbCrLf + " SELECT ITEM,SUBITEM,COSTCENTRE,METAL,COUNTER,SUM(CONVERT(INT,CTAGS)) CTAGS,SUM(CONVERT(INT,CPCS)) CPCS,SUM(CGRSWT) CGRSWT,SUM(CNETWT) CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS T "
                    strSql += vbCrLf + " WHERE 1=1 "
                    strSql += vbCrLf + " AND RESULT = 5"
                    strSql += vbCrLf + " And ISNULL(IPCS,'')<>''"
                    strSql += vbCrLf + " GROUP BY ITEM,SUBITEM,COSTCENTRE,METAL,COUNTER"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " UPDATE A SET A.CTAGS = B.CTAGS, A.CPCS = B.CPCS, A.CGRSWT = B.CGRSWT, A.CNETWT = B.CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS A"
                    strSql += vbCrLf + " INNER JOIN CTE AS B ON A.ITEM = B.ITEM AND A.SUBITEM = B.SUBITEM AND A.COSTCENTRE = B.COSTCENTRE AND A.METAL = B.METAL AND A.COUNTER = B.COUNTER"
                    strSql += vbCrLf + " WHERE A.RESULT = 6"
                    cmd = New OleDb.OleDbCommand(strSql, cn)
                    cmd.CommandTimeout = 100000
                    cmd.ExecuteNonQuery()
                End If

                strSql = ";WITH CTE AS"
                strSql += vbCrLf + "("
                strSql += vbCrLf + " SELECT ITEM,COSTCENTRE,METAL,COUNTER,SUM(CONVERT(INT,CTAGS)) CTAGS,SUM(CONVERT(INT,CPCS)) CPCS,SUM(CGRSWT) CGRSWT,SUM(CNETWT) CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS T "
                strSql += vbCrLf + " WHERE 1=1 "
                If rbtSubItem.Checked Then
                    strSql += vbCrLf + " AND RESULT = 6"
                ElseIf rbtItem.Checked Then
                    strSql += vbCrLf + " AND RESULT = 5"
                End If
                strSql += vbCrLf + " And ISNULL(IPCS,'')<>''"
                strSql += vbCrLf + " GROUP BY ITEM,COSTCENTRE,METAL,COUNTER"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " UPDATE A SET A.CTAGS = B.CTAGS, A.CPCS = B.CPCS, A.CGRSWT = B.CGRSWT, A.CNETWT = B.CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS A"
                strSql += vbCrLf + " INNER JOIN CTE AS B ON A.ITEM = B.ITEM AND A.COSTCENTRE = B.COSTCENTRE AND A.METAL = B.METAL AND A.COUNTER = B.COUNTER"
                strSql += vbCrLf + " WHERE A.RESULT = 7"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()

                strSql = ";WITH CTE AS"
                strSql += vbCrLf + "("
                strSql += vbCrLf + " SELECT COSTCENTRE,METAL,COUNTER,SUM(CONVERT(INT,CTAGS)) CTAGS,SUM(CONVERT(INT,CPCS)) CPCS,SUM(CGRSWT) CGRSWT,SUM(CNETWT) CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS T "
                strSql += vbCrLf + " WHERE 1=1 "
                strSql += vbCrLf + " AND RESULT = 7"
                strSql += vbCrLf + " And ISNULL(IPCS,'')<>''"
                strSql += vbCrLf + " GROUP BY COSTCENTRE,METAL,COUNTER"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " UPDATE A SET A.CTAGS = B.CTAGS, A.CPCS = B.CPCS, A.CGRSWT = B.CGRSWT, A.CNETWT = B.CNETWT FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN AS A"
                strSql += vbCrLf + " INNER JOIN CTE AS B ON A.COSTCENTRE = B.COSTCENTRE AND A.METAL = B.METAL AND A.COUNTER = B.COUNTER"
                strSql += vbCrLf + " WHERE A.RESULT = 8"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()
            End If

            Dim dtSource As New DataTable
            strSql = "select x.*,isnull(y.ponumber,'')PONUMBER,NULLIF(y.PO_PIECES, 0) PO_PIECES from("
            If ChkIssueOnly.Checked Then
                'strSql += vbCrLf + " Select * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN As T WHERE 1=1"
                'strSql += vbCrLf + " And RESULT <= 3"
                strSql += vbCrLf + "Select T.* FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN As T"
                strSql += vbCrLf + "inner join (Select distinct Item,counter FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN As T WHERE 1=1 And RESULT = 5 and IPCS > 0) P on t.item = p.item  and t.counter = p.counter"
                strSql += vbCrLf + "WHERE 1=1"
                strSql += vbCrLf + "And RESULT <= 3"
                strSql += vbCrLf + " UNION"
            End If
            strSql += vbCrLf + " Select * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN As T WHERE 1=1"
            If ChkIssueOnly.Checked Then strSql += vbCrLf + "And ISNULL(IPCS,'')<>''"
            If ChkIssueOnly.Checked Then
                strSql += vbCrLf + " UNION"
                strSql += vbCrLf + " SELECT 'GRAND TOTAL' AS PARTICULAR,'ZZZZZ' RANGE, 0 RANGEWT,"
                strSql += vbCrLf + " SUM(CONVERT(INT,OTAGS)) OTAGS,SUM(CONVERT(INT,OPCS)) OPCS,SUM(OGRSWT) OGRSWT,SUM(ONETWT) ONETWT           "
                strSql += vbCrLf + ",SUM(CONVERT(INT,RTAGS)) RTAGS,SUM(CONVERT(INT,RPCS)) RPCS,SUM(RGRSWT) RGRSWT,SUM(RNETWT) RNETWT                "
                strSql += vbCrLf + ",SUM(CONVERT(INT,ARRTAGS)) ARRTAGS,SUM(CONVERT(INT,ARRPCS)) ARRPCS,SUM(ARRGRSWT) ARRGRSWT,SUM(ARRNETWT) ARRNETWT                 "
                strSql += vbCrLf + ",SUM(CONVERT(INT,CRRTAGS)) CRRTAGS,SUM(CONVERT(INT, CRRPCS)) CRRPCS,SUM(CRRGRSWT) CRRGRSWT,SUM(CRRNETWT) CRRNETWT                    "
                strSql += vbCrLf + ",SUM(CONVERT(INT,ITAGS)) ITAGS,SUM(CONVERT(INT,IPCS)) IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT) INETWT                "
                strSql += vbCrLf + ",SUM(CONVERT(INT,AIITAGS)) AIITAGS,SUM(CONVERT(INT,AIIPCS)) AIIPCS,SUM(AIIGRSWT)AIIGRSWT,SUM(AIINETWT)AIINETWT                "
                strSql += vbCrLf + ",SUM(CONVERT(INT,CIITAGS)) CIITAGS,SUM(CONVERT(INT,CIIPCS)) CIIPCS,SUM(CIIGRSWT)CIIGRSWT,SUM(CIINETWT)CIINETWT               "
                strSql += vbCrLf + ",SUM(CONVERT(INT,MITAGS)) MITAGS,SUM(CONVERT(INT,MIPCS)) MIPCS,SUM(MIGRSWT)MIGRSWT,SUM(MINETWT)MINETWT           "
                strSql += vbCrLf + ",SUM(CONVERT(INT,CTAGS)) CTAGS,SUM(CONVERT(INT,CPCS)) CPCS,SUM(CGRSWT) CGRSWT,SUM(CNETWT) CNETWT                "
                strSql += vbCrLf + ",'ZZZZZ' ITEM, 'ZZZZZ' SUBITEM,'ZZZZZ' COSTCENTRE,'ZZZZZ' METAL,'ZZZZZ' COUNTER,11 AS RESULT                 "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSTEMID & "ITEMSTKFIN As T WHERE 1=1 "
                strSql += vbCrLf + " And ISNULL(IPCS,'')<>'' "
                strSql += vbCrLf + " AND RESULT = 5 "
            End If
            strSql += vbCrLf + " )as x"
            strSql += vbCrLf + $" left join {cnAdminDb}..PURCHASEORDER y on x.PARTICULAR = y.PARTICULAR and y.POFROMDATE = '{dtpFrom.Value}' and y.POTODATE = '{dtpTo.Value}'"
            strSql += vbCrLf + " ORDER BY COSTCENTRE,METAL,COUNTER"
            If rbtSubItem.Checked Then
                strSql += vbCrLf + " ,ITEM,SUBITEM,RESULT"
            ElseIf rbtItem.Checked Then
                strSql += vbCrLf + " ,ITEM,RESULT"
            End If
            strSql += vbCrLf + " ,RANGEWT"
            cmd = New OleDb.OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtSource)
            If dtSource.Rows.Count > 0 Then
                strSql = " SELECT DISTINCT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "DeITEMSTKFIN AS T "
                cmd = New OleDb.OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                dtDetail = New DataTable
                da.Fill(dtDetail)

                gridView.DataSource = dtSource
                gridView.Columns("PARTICULAR").Frozen = True
                tabMain.SelectedTab = tabView
                gridStyle()
                'EditGridColumn()
                btnPost.Enabled = True
            Else
                btnPost.Enabled = False
                gridView.DataSource = Nothing
                tabMain.SelectedTab = tabGen
                MsgBox("Records not found...", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            MsgBox("Predefined conditions/dbs are un matched") : Exit Sub
        End Try
    End Sub
    Private Sub EditGridColumn_old()
        gridView.ReadOnly = False
        Dim newColumn As New DataGridViewTextBoxColumn()
        newColumn.Name = "PO_PIECES"
        newColumn.HeaderText = "PO_PIECES"
        newColumn.ReadOnly = False
        gridView.Columns.Add(newColumn)

        newColumn = New DataGridViewTextBoxColumn()
        newColumn.Name = "REMARKS"
        newColumn.HeaderText = "REMARKS"
        newColumn.ReadOnly = False
        gridView.Columns.Add(newColumn)

        gridView.Columns("PO_PIECES").ReadOnly = False
        gridView.Columns("REMARKS").ReadOnly = False

        gridView.Columns(gridView.Columns.Count - 1).ReadOnly = False
        gridView.Columns(gridView.Columns.Count - 2).ReadOnly = False

        For Each row As DataGridViewRow In gridView.Rows
            row.Cells(gridView.Columns.Count - 1).Value = ""
            row.Cells(gridView.Columns.Count - 2).Value = ""
        Next
    End Sub
    Private Sub EditGridColumn()
        ' Set the entire grid to be editable first
        gridView.ReadOnly = False

        ' Add the PO_PIECES column
        Dim newColumn As New DataGridViewTextBoxColumn()
        newColumn.Name = "PO_PIECES"
        newColumn.HeaderText = "PO_PIECES"
        gridView.Columns.Add(newColumn)

        ' Add the REMARKS column
        'newColumn = New DataGridViewTextBoxColumn()
        'newColumn.Name = "REMARKS"
        'newColumn.HeaderText = "REMARKS"
        'gridView.Columns.Add(newColumn)

        ' Explicitly set these columns to be editable
        gridView.Columns("PO_PIECES").ReadOnly = False
        'gridView.Columns("REMARKS").ReadOnly = False

        ' Clear the contents of the new columns for all rows
        For Each row As DataGridViewRow In gridView.Rows
            row.Cells("PO_PIECES").Value = row.Cells("POPIECES").Value
            'row.Cells("REMARKS").Value = String.Empty
        Next
    End Sub
    Private Sub Generateselectedragetable(Optional ByVal sysId As String = "")
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysId & "CHECKEDITEM') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysId & "CHECKEDITEM"
        strSql += " SELECT DISTINCT CAPTION,1 RESULT INTO TEMPTABLEDB..TEMP" & sysId & "CHECKEDITEM FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then
            strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        End If
        strSql += vbCrLf + " ORDER BY RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()


        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysId & "SELECTRANGE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysId & "SELECTRANGE"
        strSql += " SELECT CAPTION INTO TEMPTABLEDB..TEMP" & sysId & "SELECTRANGE FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1 "
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        Dim res As String
        If Trim(chkcmbrange.Text.ToString) = "ALL" Or Trim(chkcmbrange.Text.ToString) = "" Then
            For i As Integer = 1 To dtrange.Rows.Count - 1
                res = dtrange.Rows(i).Item("Caption").ToString
                If res <> "ALL" Then
                    InsertRange(dtrange.Rows(i).Item("CAPTION").ToString, sysId)
                End If
            Next
        Else
            For i As Integer = 0 To chkcmbrange.CheckedItems.Count - 1
                InsertRange(chkcmbrange.CheckedItems.Item(i).ToString, sysId)
            Next
        End If
    End Sub
    Private Sub InsertRange(ByVal Cap As String, Optional ByVal sysId As String = "")
        strSql = "INSERT INTO TEMPTABLEDB..TEMP" & sysId & "SELECTRANGE"
        strSql += " SELECT '" & Cap & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
    End Sub
    Private Sub GridViewHeaderStyleDetail()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strAPPROVAL As String = ""
        With dtMergeHeader
            .Columns.Add("PARTICULAR~RANGE", GetType(String))
            If ChkTransPrint.Checked Then
                .Columns.Add("RPCS~RTAGS~RGRSWT~RNETWT" & strAPPROVAL, GetType(String))
            ElseIf ChkIssueOnly.Checked Then
                .Columns.Add("IPCS~ITAGS~IGRSWT~INETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("CPCS~CTAGS~CGRSWT~CNETWT" & strAPPROVAL, GetType(String))
            End If

            If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                .Columns.Add("RPCS~RTAGS~RGRSWT~RNETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("OPCS~OTAGS~OGRSWT~ONETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("IPCS~ITAGS~IGRSWT~INETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("MIPCS~MITAGS~MIGRSWT~MINETWT" & strAPPROVAL, GetType(String))
                .Columns.Add("CPCS~CTAGS~CGRSWT~CNETWT" & strAPPROVAL, GetType(String))
            End If
            .Columns.Add("PONUMBER" & strAPPROVAL, GetType(String))
            .Columns.Add("PO_PIECES" & strAPPROVAL, GetType(String))

            '.Columns.Add("REMARKS" & strAPPROVAL, GetType(String))

            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR~RANGE").Caption = ""
            If ChkTransPrint.Checked Then
                .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strAPPROVAL).Caption = "RECEIPT"
            ElseIf ChkIssueOnly.Checked Then
                .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strAPPROVAL).Caption = "ISSUE"
                .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strAPPROVAL).Caption = "CLOSING"
            End If

            If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strAPPROVAL).Caption = "RECEIPT"
                .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strAPPROVAL).Caption = "OPENING"
                .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strAPPROVAL).Caption = "APPRECEIPT"
                .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strAPPROVAL).Caption = "CTRANRECEIPT"
                .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strAPPROVAL).Caption = "ISSUE"
                .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strAPPROVAL).Caption = "APPISSUE"
                .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strAPPROVAL).Caption = "CTRANISSUE"
                .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strAPPROVAL).Caption = "MISCISSUE"
                .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strAPPROVAL).Caption = "CLOSING"
            End If
            .Columns("PONUMBER").Caption = "PONUMBER"
            .Columns("PO_PIECES").Caption = "PO_PIECES"
            '.Columns("REMARKS").Caption = "REMARKS"
            .Columns("SCROLL").Caption = ""
        End With

        With gridViewHead
            .DataSource = Nothing
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR~RANGE").HeaderText = ""
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

    Function funcColWidthDetail() As Integer
        With gridViewHead
            If gridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR~RANGE").Width = gridView.Columns("PARTICULAR").Width
            '.Columns("PARTICULAR~RANGE").Width += gridView.Columns("RANGE").Width
            Dim strSepApp As String = ""
            If rbtDispPcs.Checked Then
                If ChkTransPrint.Checked Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RPCS").Width
                ElseIf ChkIssueOnly.Checked Then
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IPCS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CPCS").Width
                End If
                If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RPCS").Width
                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width = gridView.Columns("OPCS").Width
                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width = gridView.Columns("ARRPCS").Width
                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width = gridView.Columns("CRRPCS").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IPCS").Width
                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width = gridView.Columns("AIIPCS").Width
                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width = gridView.Columns("CIIPCS").Width
                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width = gridView.Columns("MIPCS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CPCS").Width
                End If
            ElseIf rbtDispWt.Checked Then
                If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RGRSWT").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RNETWT").Width

                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width = gridView.Columns("OGRSWT").Width
                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width += gridView.Columns("ONETWT").Width

                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width = gridView.Columns("ARRGRSWT").Width
                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width += gridView.Columns("ARRNETWT").Width

                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width = gridView.Columns("CRRGRSWT").Width
                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width += gridView.Columns("CRRNETWT").Width

                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IGRSWT").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("INETWT").Width

                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width = gridView.Columns("AIIGRSWT").Width
                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width += gridView.Columns("AIINETWT").Width

                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width = gridView.Columns("CIIGRSWT").Width
                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width += gridView.Columns("CIINETWT").Width

                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width = gridView.Columns("MIGRSWT").Width
                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width += gridView.Columns("MINETWT").Width

                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CGRSWT").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CNETWT").Width
                End If
                If ChkTransPrint.Checked Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RGRSWT").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RNETWT").Width
                ElseIf ChkIssueOnly.Checked Then
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IGRSWT").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("INETWT").Width

                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CGRSWT").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CNETWT").Width
                End If
            Else
                If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RPCS").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RTAGS").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RGRSWT").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RNETWT").Width

                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width = gridView.Columns("OPCS").Width
                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width += gridView.Columns("OTAGS").Width
                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width += gridView.Columns("OGRSWT").Width
                    .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).Width += gridView.Columns("ONETWT").Width

                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width = gridView.Columns("ARRPCS").Width
                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width += gridView.Columns("ARRTAGS").Width
                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width += gridView.Columns("ARRGRSWT").Width
                    .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).Width += gridView.Columns("ARRNETWT").Width

                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width = gridView.Columns("CRRPCS").Width
                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width += gridView.Columns("CRRTAGS").Width
                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width += gridView.Columns("CRRGRSWT").Width
                    .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).Width += gridView.Columns("CRRNETWT").Width

                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IPCS").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("ITAGS").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("IGRSWT").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("INETWT").Width

                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width = gridView.Columns("AIIPCS").Width
                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width += gridView.Columns("AIITAGS").Width
                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width += gridView.Columns("AIIGRSWT").Width
                    .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).Width += gridView.Columns("AIINETWT").Width

                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width = gridView.Columns("CIIPCS").Width
                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width += gridView.Columns("CIITAGS").Width
                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width += gridView.Columns("CIIGRSWT").Width
                    .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).Width += gridView.Columns("CIINETWT").Width

                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width = gridView.Columns("MIPCS").Width
                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width += gridView.Columns("MITAGS").Width
                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width += gridView.Columns("MIGRSWT").Width
                    .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).Width += gridView.Columns("MINETWT").Width

                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CPCS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CTAGS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CGRSWT").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CNETWT").Width
                End If
                If ChkTransPrint.Checked Then
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width = gridView.Columns("RPCS").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RTAGS").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RGRSWT").Width
                    .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).Width += gridView.Columns("RNETWT").Width
                ElseIf ChkIssueOnly.Checked Then
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width = gridView.Columns("IPCS").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("ITAGS").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("IGRSWT").Width
                    .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).Width += gridView.Columns("INETWT").Width

                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width = gridView.Columns("CPCS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CTAGS").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CGRSWT").Width
                    .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).Width += gridView.Columns("CNETWT").Width
                End If
            End If
            If ChkTransPrint.Checked = False AndAlso ChkIssueOnly.Checked = False Then
                .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).HeaderText = "RECEIPT"
                .Columns("OPCS~OTAGS~OGRSWT~ONETWT" & strSepApp).HeaderText = "OPENING"
                .Columns("ARRPCS~ARRTAGS~ARRGRSWT~ARRNETWT" & strSepApp).HeaderText = "APPRECEIPT"
                .Columns("CRRPCS~CRRTAGS~CRRGRSWT~CRRNETWT" & strSepApp).HeaderText = "CTRANRECEIPT"
                .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).HeaderText = "ISSUE"
                .Columns("AIIPCS~AIITAGS~AIIGRSWT~AIINETWT" & strSepApp).HeaderText = "APPISSUE"
                .Columns("CIIPCS~CIITAGS~CIIGRSWT~CIINETWT" & strSepApp).HeaderText = "CTRANISSUE"
                .Columns("MIPCS~MITAGS~MIGRSWT~MINETWT" & strSepApp).HeaderText = "MISCISSUE"
                .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).HeaderText = "CLOSING"
            End If
            If ChkTransPrint.Checked Then
                .Columns("RPCS~RTAGS~RGRSWT~RNETWT" & strSepApp).HeaderText = "RECEIPT"
            ElseIf ChkIssueOnly.Checked Then
                .Columns("IPCS~ITAGS~IGRSWT~INETWT" & strSepApp).HeaderText = "ISSUE"
                .Columns("CPCS~CTAGS~CGRSWT~CNETWT" & strSepApp).HeaderText = "CLOSING"
            End If

            .Columns("PONUMBER" & strSepApp).Width = gridView.Columns("PONUMBER").Width
            .Columns("PONUMBER" & strSepApp).HeaderText = "PONUMBER"

            .Columns("PO_PIECES" & strSepApp).Width = gridView.Columns("PO_PIECES").Width
            .Columns("PO_PIECES" & strSepApp).HeaderText = "PO_PIECES"

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function  '01
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridviewDetail.Visible = False
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If spbaserpt = True Then
            SpReport()
            Exit Sub
        End If
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
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
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        ' chkAsOnDate.Checked = True
        funcLoadItemName()
        ' chkWithApproval.Checked = False
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        strSql = " SELECT 'ALL' Caption,0 RESULT UNION ALL "
        strSql += "SELECT DISTINCT CAPTION,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange, "Caption", , "ALL")
        chkCmbMetal.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
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
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If IS40COLCLSSTKPRINT Then
            If MsgBox("Do you want to print on 60 Col. Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then IS40COLCLSSTKPRINT = False
        End If
        If IS40COLCLSSTKPRINT Then
            If rbtDispPcs.Checked Then
                PRINT40COLSUMMARY_PCS()
            Else
                Call PRINT40COLSUMMARY()
            End If
        Else
            If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
            End If
        End If
    End Sub
    Private Sub PRINT40COLSUMMARY_PCS()
        If MsgBox("Do you want to Summary Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
                IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
            End If
            Dim write As StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
            Dim lineprn As String = Space(60)
            write.WriteLine(Chr(27) + "M")
            write.WriteLine("------------------------------------------------")
            write.WriteLine("DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
            write.WriteLine("------------------------------------------------")
            Dim Desc As String = ""
            Dim oPcs As String = "Opn"
            ' Dim rPcs As String = "Rec"
            'Dim sPcs As String = "Sal"
            'Dim mPcs As String = "Misc"
            Dim cPcs As String = "Cls"
            Desc = LSet(Desc, 15)
            oPcs = RSet(oPcs, 5)
            'rPcs = RSet(rPcs, 5)
            'sPcs = RSet(sPcs, 5)
            'mPcs = RSet(mPcs, 5)
            cPcs = RSet(cPcs, 5)
            write.WriteLine(Desc & Space(1) & oPcs & Space(1) & Space(1) & cPcs)
            Desc = "Item"
            oPcs = ""
            oPcs = "Qty"
            Desc = LSet(Desc, 15)
            oPcs = RSet(oPcs, 5)
            'rPcs = RSet(oPcs, 5)
            'sPcs = RSet(oPcs, 5)
            'mPcs = RSet(oPcs, 5)
            cPcs = RSet(oPcs, 5)
            write.WriteLine(Desc & Space(1) & oPcs & Space(1) & Space(1) & cPcs)
            write.WriteLine("-------------------------------------------------")
            For i As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(i)
                    Dim prndesc As String = Mid(.Cells("PARTICULAR").Value.ToString, 1, 15)
                    If prndesc.Contains("TOTAL") And Not prndesc.Contains("COUNTER TOTAL") Then write.WriteLine("------------------------------------------------")
                    'Dim prnOpcs As String = Val(.Cells("OPCS").Value.ToString)
                    ''Dim prnRpcs As String = Val(.Cells("RPCS").Value.ToString)
                    ''Dim prnSpcs As String = Val(.Cells("IPCS").Value.ToString)
                    ''Dim prnMpcs As String = Val(.Cells("MIPCS").Value.ToString)
                    'Dim prnCpcs As String = Val(.Cells("CPCS").Value.ToString)
                    Dim prnOpcs As String = .Cells("OPCS").Value.ToString
                    Dim prnCpcs As String = .Cells("CPCS").Value.ToString
                    prndesc = LSet(prndesc, 15)
                    prnOpcs = RSet(prnOpcs, 5)
                    'prnRpcs = RSet(prnRpcs, 5)
                    'prnSpcs = RSet(prnSpcs, 5)
                    'prnMpcs = RSet(prnMpcs, 5)
                    prnCpcs = RSet(prnCpcs, 5)
                    'If rbtDispPcs.Checked Then
                    write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnCpcs)
                    ' End If

                    'If prndesc.Contains("TOTAL") Then write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnRpcs & Space(1) & prnSpcs & Space(1) & prnMpcs & Space(1) & prnCpcs)
                    'If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")

                    ' If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
                    ' If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
                    'If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnRpcs & Space(1) & prnSpcs & Space(1) & prnMpcs & Space(1) & prnCpcs)
                    If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine("------------------------------------------------")
                    If prndesc.Contains("TOTAL") And Not prndesc.Contains("COUNTER TOTAL") Then write.WriteLine("------------------------------------------------")
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
        Else


            If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
                IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
            End If
            Dim write As StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
            Dim lineprn As String = Space(60)
            write.WriteLine(Chr(27) + "M")
            write.WriteLine("------------------------------------------------")
            write.WriteLine("DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
            write.WriteLine("------------------------------------------------")
            Dim Desc As String = ""
            Dim oPcs As String = "Opn"
            Dim rPcs As String = "Rec"
            Dim sPcs As String = "Sal"
            Dim mPcs As String = "Misc"
            Dim cPcs As String = "Cls"
            Desc = LSet(Desc, 15)
            oPcs = RSet(oPcs, 5)
            rPcs = RSet(rPcs, 5)
            sPcs = RSet(sPcs, 5)
            mPcs = RSet(mPcs, 5)
            cPcs = RSet(cPcs, 5)
            write.WriteLine(Desc & Space(1) & oPcs & Space(1) & rPcs & Space(1) & sPcs & Space(1) & mPcs & Space(1) & cPcs)
            Desc = "Item"
            oPcs = ""
            oPcs = "Qty"
            Desc = LSet(Desc, 15)
            oPcs = RSet(oPcs, 5)
            rPcs = RSet(oPcs, 5)
            sPcs = RSet(oPcs, 5)
            mPcs = RSet(oPcs, 5)
            cPcs = RSet(oPcs, 5)
            write.WriteLine(Desc & Space(1) & oPcs & Space(1) & rPcs & Space(1) & sPcs & Space(1) & mPcs & Space(1) & cPcs)
            write.WriteLine("-------------------------------------------------")
            For i As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(i)
                    Dim prndesc As String = Mid(.Cells("PARTICULAR").Value.ToString, 1, 15)
                    Dim prnOpcs As String = Val(.Cells("OPCS").Value.ToString)
                    Dim prnRpcs As String = Val(.Cells("RPCS").Value.ToString)
                    Dim prnSpcs As String = Val(.Cells("IPCS").Value.ToString)
                    Dim prnMpcs As String = Val(.Cells("MIPCS").Value.ToString)
                    Dim prnCpcs As String = Val(.Cells("CPCS").Value.ToString)
                    prndesc = LSet(prndesc, 15)
                    prnOpcs = RSet(prnOpcs, 5)
                    prnRpcs = RSet(prnRpcs, 5)
                    prnSpcs = RSet(prnSpcs, 5)
                    prnMpcs = RSet(prnMpcs, 5)
                    prnCpcs = RSet(prnCpcs, 5)
                    'If rbtDispPcs.Checked Then
                    write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnRpcs & Space(1) & prnSpcs & Space(1) & prnMpcs & Space(1) & prnCpcs)
                    ' End If

                    'If prndesc.Contains("TOTAL") Then write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnRpcs & Space(1) & prnSpcs & Space(1) & prnMpcs & Space(1) & prnCpcs)
                    'If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")

                    ' If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
                    ' If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
                    'If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine(prndesc & Space(1) & prnOpcs & Space(1) & prnRpcs & Space(1) & prnSpcs & Space(1) & prnMpcs & Space(1) & prnCpcs)
                    If prndesc.Contains("COUNTER TOTAL") Then write.WriteLine("------------------------------------------------")
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
        End If
    End Sub
    Private Sub PRINT40COLSUMMARY()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim write As StreamWriter
        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        Dim lineprn As String = Space(60)
        write.WriteLine("-----------------------------------------------------")
        write.WriteLine("RANGE WISE " & IIf(ChkTransPrint.Checked, " PLUS REPORT", " STOCK") & " DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
        write.WriteLine("-----------------------------------------------------")
        Dim Desc As String = "DESCRIPTION"
        Dim Pcs As String = "PCS"
        Dim Grswt As String = "GRS WT"
        Desc = LSet(Desc, 29)
        Pcs = RSet(Pcs, 5)
        Grswt = RSet(Grswt, 9)
        If rbtDispPcs.Checked Then
            write.WriteLine(Desc & Space(1) & Pcs)
        ElseIf rbtDispWt.Checked Then
            write.WriteLine(Desc & Space(1) & Grswt)
        Else
            write.WriteLine(Desc & Space(1) & Pcs & Space(1) & Grswt)
        End If
        write.WriteLine("-----------------------------------------------------")
        For i As Integer = 0 To gridView.Rows.Count - 1
            With gridView.Rows(i)
                Dim prndesc As String = Mid(.Cells("PARTICULAR").Value.ToString, 1, 32)
                Dim prnpcs As String = IIf(ChkTransPrint.Checked, .Cells("RPCS").Value.ToString, .Cells("CPCS").Value.ToString)
                Dim prngwt As String = IIf(ChkTransPrint.Checked, .Cells("RGRSWT").Value.ToString, .Cells("CGRSWT").Value.ToString)
                prndesc = LSet(prndesc, 29)
                prnpcs = RSet(prnpcs, 5)
                prngwt = RSet(prngwt, 9)
                If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
                If rbtDispPcs.Checked Then
                    write.WriteLine(prndesc & Space(1) & prnpcs)
                ElseIf rbtDispWt.Checked Then
                    write.WriteLine(prndesc & Space(1) & prngwt)
                Else
                    write.WriteLine(prndesc & Space(1) & prnpcs & Space(1) & prngwt)
                End If
                If prndesc.Contains("TOTAL") Then write.WriteLine(Space(5) & "-----------------------------------------------------")
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

        ChkItemMode.Items.Add("ALL")
        ChkItemMode.Items.Add("WEIGHT")
        ChkItemMode.Items.Add("RATE")
        'ChkItemMode.Items.Add("BOTH")
        ChkItemMode.Items.Add("FIXED")
        ChkItemMode.Items.Add("METAL RATE")
        ChkItemMode.Text = "ALL"
        'pnlGroupFilter.Location = New POINT((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Function gridStyle()
        If gridView.Columns.Contains("COSTCENTRE") Then gridView.Columns("COSTCENTRE").Visible = False
        If gridView.Columns.Contains("METAL") Then gridView.Columns("METAL").Visible = False
        If gridView.Columns.Contains("COUNTER") Then gridView.Columns("COUNTER").Visible = False
        If gridView.Columns.Contains("RESULT") Then gridView.Columns("RESULT").Visible = False
        'If gridView.Columns.Contains("POPIECES") Then gridView.Columns("POPIECES").Visible = False
        If gridView.Columns.Contains("ITEM") Then gridView.Columns("ITEM").Visible = False
        If gridView.Columns.Contains("SUBITEM") Then gridView.Columns("SUBITEM").Visible = False
        If gridView.Columns.Contains("RANGE") Then gridView.Columns("RANGE").Visible = False
        If gridView.Columns.Contains("RANGEWT") Then gridView.Columns("RANGEWT").Visible = False
        If rbtDispPcs.Checked Then
            gridView.Columns("OTAGS").Visible = False
            gridView.Columns("OGRSWT").Visible = False
            gridView.Columns("ONETWT").Visible = False
            gridView.Columns("RTAGS").Visible = False
            gridView.Columns("RGRSWT").Visible = False
            gridView.Columns("RNETWT").Visible = False
            gridView.Columns("ARRTAGS").Visible = False
            gridView.Columns("ARRGRSWT").Visible = False
            gridView.Columns("ARRNETWT").Visible = False
            gridView.Columns("CRRTAGS").Visible = False
            gridView.Columns("CRRGRSWT").Visible = False
            gridView.Columns("CRRNETWT").Visible = False
            gridView.Columns("ITAGS").Visible = False
            gridView.Columns("IGRSWT").Visible = False
            gridView.Columns("INETWT").Visible = False
            gridView.Columns("AIITAGS").Visible = False
            gridView.Columns("AIIGRSWT").Visible = False
            gridView.Columns("AIINETWT").Visible = False
            gridView.Columns("CIITAGS").Visible = False
            gridView.Columns("CIIGRSWT").Visible = False
            gridView.Columns("CIINETWT").Visible = False
            gridView.Columns("MITAGS").Visible = False
            gridView.Columns("MIGRSWT").Visible = False
            gridView.Columns("MINETWT").Visible = False
            gridView.Columns("CTAGS").Visible = False
            gridView.Columns("CGRSWT").Visible = False
            gridView.Columns("CNETWT").Visible = False

            gridView.Columns("OPCS").Visible = True
            gridView.Columns("RPCS").Visible = True
            gridView.Columns("ARRPCS").Visible = True
            gridView.Columns("CRRPCS").Visible = True
            gridView.Columns("IPCS").Visible = True
            gridView.Columns("AIIPCS").Visible = True
            gridView.Columns("CIIPCS").Visible = True
            gridView.Columns("MIPCS").Visible = True
            gridView.Columns("CPCS").Visible = True

            gridView.Columns("PARTICULAR").Width = 250
            gridView.Columns("OPCS").Width = 120
            gridView.Columns("RPCS").Width = 120
            gridView.Columns("ARRPCS").Width = 120
            gridView.Columns("CRRPCS").Width = 120
            gridView.Columns("IPCS").Width = 120
            gridView.Columns("AIIPCS").Width = 120
            gridView.Columns("CIIPCS").Width = 120
            gridView.Columns("MIPCS").Width = 120
            gridView.Columns("CPCS").Width = 120
        ElseIf rbtDispWt.Checked Then
            gridView.Columns("OTAGS").Visible = False
            gridView.Columns("OPCS").Visible = False
            gridView.Columns("RTAGS").Visible = False
            gridView.Columns("RPCS").Visible = False
            gridView.Columns("ARRTAGS").Visible = False
            gridView.Columns("ARRPCS").Visible = False
            gridView.Columns("CRRTAGS").Visible = False
            gridView.Columns("CRRPCS").Visible = False
            gridView.Columns("ITAGS").Visible = False
            gridView.Columns("IPCS").Visible = False
            gridView.Columns("AIITAGS").Visible = False
            gridView.Columns("AIIPCS").Visible = False
            gridView.Columns("CIITAGS").Visible = False
            gridView.Columns("CIIPCS").Visible = False
            gridView.Columns("MITAGS").Visible = False
            gridView.Columns("MIPCS").Visible = False
            gridView.Columns("CTAGS").Visible = False
            gridView.Columns("CPCS").Visible = False
            gridView.Columns("OGRSWT").Visible = True
            gridView.Columns("ONETWT").Visible = True
            gridView.Columns("RGRSWT").Visible = True
            gridView.Columns("RNETWT").Visible = True
            gridView.Columns("ARRGRSWT").Visible = True
            gridView.Columns("ARRNETWT").Visible = True
            gridView.Columns("CRRGRSWT").Visible = True
            gridView.Columns("CRRNETWT").Visible = True
            gridView.Columns("IGRSWT").Visible = True
            gridView.Columns("INETWT").Visible = True
            gridView.Columns("AIIGRSWT").Visible = True
            gridView.Columns("AIINETWT").Visible = True
            gridView.Columns("CIIGRSWT").Visible = True
            gridView.Columns("CIINETWT").Visible = True
            gridView.Columns("MIGRSWT").Visible = True
            gridView.Columns("MINETWT").Visible = True
            gridView.Columns("CGRSWT").Visible = True
            gridView.Columns("CNETWT").Visible = True
        Else
            gridView.Columns("OTAGS").Visible = True
            gridView.Columns("RTAGS").Visible = True
            gridView.Columns("ITAGS").Visible = True
            gridView.Columns("CTAGS").Visible = True
            gridView.Columns("OPCS").Visible = True
            gridView.Columns("RPCS").Visible = True
            gridView.Columns("IPCS").Visible = True
            gridView.Columns("CPCS").Visible = True
            gridView.Columns("OGRSWT").Visible = True
            gridView.Columns("ONETWT").Visible = True
            gridView.Columns("RGRSWT").Visible = True
            gridView.Columns("RNETWT").Visible = True
            gridView.Columns("IGRSWT").Visible = True
            gridView.Columns("INETWT").Visible = True
            gridView.Columns("CGRSWT").Visible = True
            gridView.Columns("CNETWT").Visible = True
            gridView.Columns("ARRTAGS").Visible = True
            gridView.Columns("ARRPCS").Visible = True
            gridView.Columns("ARRGRSWT").Visible = True
            gridView.Columns("ARRNETWT").Visible = True
            gridView.Columns("CRRTAGS").Visible = True
            gridView.Columns("CRRPCS").Visible = True
            gridView.Columns("CRRGRSWT").Visible = True
            gridView.Columns("CRRNETWT").Visible = True
            gridView.Columns("AIITAGS").Visible = True
            gridView.Columns("AIIPCS").Visible = True
            gridView.Columns("AIIGRSWT").Visible = True
            gridView.Columns("AIINETWT").Visible = True
            gridView.Columns("CIITAGS").Visible = True
            gridView.Columns("CIIPCS").Visible = True
            gridView.Columns("CIIGRSWT").Visible = True
            gridView.Columns("CIINETWT").Visible = True
            gridView.Columns("MITAGS").Visible = True
            gridView.Columns("MIPCS").Visible = True
            gridView.Columns("MIGRSWT").Visible = True
            gridView.Columns("MINETWT").Visible = True
        End If
        gridView.Columns("OTAGS").HeaderText = "TAGS"
        gridView.Columns("OPCS").HeaderText = "PCS"
        gridView.Columns("OGRSWT").HeaderText = "GRSWT"
        gridView.Columns("ONETWT").HeaderText = "NETWT"

        gridView.Columns("RTAGS").HeaderText = "TAGS"
        gridView.Columns("RPCS").HeaderText = "PCS"
        gridView.Columns("RGRSWT").HeaderText = "GRSWT"
        gridView.Columns("RNETWT").HeaderText = "NETWT"

        gridView.Columns("ARRTAGS").HeaderText = "TAGS"
        gridView.Columns("ARRPCS").HeaderText = "PCS"
        gridView.Columns("ARRGRSWT").HeaderText = "GRSWT"
        gridView.Columns("ARRNETWT").HeaderText = "NETWT"

        gridView.Columns("CRRTAGS").HeaderText = "TAGS"
        gridView.Columns("CRRPCS").HeaderText = "PCS"
        gridView.Columns("CRRGRSWT").HeaderText = "GRSWT"
        gridView.Columns("CRRNETWT").HeaderText = "NETWT"

        gridView.Columns("ITAGS").HeaderText = "TAGS"
        gridView.Columns("IPCS").HeaderText = "PCS"
        gridView.Columns("IGRSWT").HeaderText = "GRSWT"
        gridView.Columns("INETWT").HeaderText = "NETWT"

        gridView.Columns("AIITAGS").HeaderText = "TAGS"
        gridView.Columns("AIIPCS").HeaderText = "PCS"
        gridView.Columns("AIIGRSWT").HeaderText = "GRSWT"
        gridView.Columns("AIINETWT").HeaderText = "NETWT"

        gridView.Columns("CIITAGS").HeaderText = "TAGS"
        gridView.Columns("CIIPCS").HeaderText = "PCS"
        gridView.Columns("CIIGRSWT").HeaderText = "GRSWT"
        gridView.Columns("CIINETWT").HeaderText = "NETWT"

        gridView.Columns("MITAGS").HeaderText = "TAGS"
        gridView.Columns("MIPCS").HeaderText = "PCS"
        gridView.Columns("MIGRSWT").HeaderText = "GRSWT"
        gridView.Columns("MINETWT").HeaderText = "NETWT"

        gridView.Columns("CTAGS").HeaderText = "TAGS"
        gridView.Columns("CPCS").HeaderText = "PCS"
        gridView.Columns("CGRSWT").HeaderText = "GRSWT"
        gridView.Columns("CNETWT").HeaderText = "NETWT"

        If ChkTransPrint.Checked Then
            For I As Integer = 0 To gridView.ColumnCount - 1
                gridView.Columns(I).Visible = False
            Next
            gridView.Columns("PARTICULAR").Visible = True
            gridView.Columns("RTAGS").Visible = True
            If rbtDispPcs.Checked Then
                gridView.Columns("RPCS").Visible = True
            ElseIf rbtDispWt.Checked Then
                gridView.Columns("RGRSWT").Visible = True
                gridView.Columns("RNETWT").Visible = True
            Else
                gridView.Columns("RPCS").Visible = True
                gridView.Columns("RGRSWT").Visible = True
                gridView.Columns("RNETWT").Visible = True

            End If
        End If
        If ChkIssueOnly.Checked Then
            For I As Integer = 0 To gridView.ColumnCount - 1
                gridView.Columns(I).Visible = False
            Next
            gridView.Columns("PARTICULAR").Visible = True
            gridView.Columns("ITAGS").Visible = True
            gridView.Columns("CTAGS").Visible = True
            If rbtDispPcs.Checked Then
                gridView.Columns("IPCS").Visible = True
                gridView.Columns("CPCS").Visible = True
            ElseIf rbtDispWt.Checked Then
                gridView.Columns("IGRSWT").Visible = True
                gridView.Columns("INETWT").Visible = True

                gridView.Columns("CGRSWT").Visible = True
                gridView.Columns("CNETWT").Visible = True
            Else
                gridView.Columns("IPCS").Visible = True
                gridView.Columns("IGRSWT").Visible = True
                gridView.Columns("INETWT").Visible = True

                gridView.Columns("CPCS").Visible = True
                gridView.Columns("CGRSWT").Visible = True
                gridView.Columns("CNETWT").Visible = True
            End If
        End If

        gridView.Columns("PONUMBER").Visible = True
        gridView.Columns("PO_PIECES").Visible = True

        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "1"
                        .Cells("PARTICULAR").Style.BackColor = Color.PaleGreen
                        .DefaultCellStyle.ForeColor = Color.DarkGreen
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "2"
                        .Cells("PARTICULAR").Style.BackColor = Color.Lavender
                        .DefaultCellStyle.ForeColor = Color.DarkViolet
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "3"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "4"
                        '.Cells("PARTICULAR").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.ForeColor = Color.DarkBlue
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "6"
                        '.DefaultCellStyle.BackColor = Color.LightBlue
                        .DefaultCellStyle.ForeColor = Color.DarkBlue
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "7"
                        .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "8"
                        .DefaultCellStyle.BackColor = Color.Lavender
                        .DefaultCellStyle.ForeColor = Color.DarkViolet
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "9"
                        .DefaultCellStyle.BackColor = Color.PaleGreen
                        .DefaultCellStyle.ForeColor = Color.DarkGreen
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "10"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "11"
                        .DefaultCellStyle.BackColor = Color.DarkGreen
                        .DefaultCellStyle.ForeColor = Color.LightGreen
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                End Select
                Select Case Mid(.Cells("RANGE").Value.ToString, 1, 6)
                    Case "ZZZZZ "
                        .Cells("RANGE").Value = Replace(.Cells("RANGE").Value.ToString, "ZZZZZ ", "")
                End Select
                Select Case .Cells("PARTICULAR").Value.ToString
                    Case "ZZZZZ "
                        .Cells("PARTICULAR").Value = Replace(.Cells("PARTICULAR").Value.ToString, "ZZZZZ ", "")
                End Select
            End With
        Next
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            If dgvCol.Name.ToString = "PARTICULAR" Then
                dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Else
                dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        Next
        GridViewHeaderStyleDetail()
        Dim tit As String
        tit = " ITEM RANGE WISE STOCK" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            tit += " FOR METAL " & chkCmbMetal.Text & ""
        End If
        If chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "" Then
            tit += "(" & chkCmbCounter.Text & ")"
        End If
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            tit += "(" & chkCmbItem.Text & ")"
        End If
        If chkCmbItemType.Text <> "ALL" And chkCmbItemType.Text <> "" Then
            tit += vbCrLf & "FOR " & chkCmbItemType.Text
        End If
        lblTitle.Text = tit.ToString
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
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
            GridViewHeaderStyleDetail()
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub ChkTransPrint_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTransPrint.CheckedChanged
        If ChkTransPrint.Checked = True Then ChkIssueOnly.Checked = False
    End Sub

    Private Sub ChkIssueOnly_CheckedChanged(sender As Object, e As EventArgs) Handles ChkIssueOnly.CheckedChanged
        If ChkIssueOnly.Checked = True Then ChkTransPrint.Checked = False
    End Sub

    Private Sub gridView_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit
        If e.ColumnIndex <> gridView.Columns.Count - 1 Then
            e.Cancel = True
        End If
    End Sub
    Function funcLoadRange() As Boolean
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkCmbItem.Text.ToString) = "ALL" Or Trim(chkCmbItem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkCmbItem, True)
        strSql = " SELECT 'ALL' Caption,0 RESULT UNION ALL "
        strSql += "SELECT DISTINCT CAPTION,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange, "Caption", , "ALL")
    End Function

    Private Sub chkCmbMetal_Validated(sender As Object, e As EventArgs) Handles chkCmbMetal.Validated
        funcLoadItemName()
    End Sub

    Private Sub chkCmbItem_Validated(sender As Object, e As EventArgs) Handles chkCmbItem.Validated
        funcLoadRange()
    End Sub

    Private Sub btnPost_Click(sender As Object, e As EventArgs) Handles btnPost.Click
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("PARTICULAR")
            dt.Columns.Add("ITEM")
            dt.Columns.Add("PO_PIECES")
            dt.Columns.Add("RESULT")

            For Each row As DataGridViewRow In gridView.Rows
                If Not row.IsNewRow Then
                    If row.Cells("RESULT").Value = 5 And Not IsDBNull(row.Cells("PO_PIECES").Value) Then
                        Dim dataRow As DataRow = dt.NewRow()
                        dataRow("PARTICULAR") = row.Cells("PARTICULAR").Value
                        dataRow("ITEM") = row.Cells("ITEM").Value
                        dataRow("PO_PIECES") = row.Cells("PO_PIECES").Value
                        dataRow("RESULT") = row.Cells("RESULT").Value
                        dt.Rows.Add(dataRow)
                    End If
                End If
            Next

            If dt.Rows.Count = 0 Then Throw New Exception("Records not found.")

            UpdatePurchaseOrderData(userId, dt)
            BtnGenerate.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Sub UpdatePurchaseOrderData(userId As Integer, purchaseOrderData As DataTable)
        ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
        Using connection As New SqlConnection("Data Source=" & ConInfo.lServerName & ";Initial Catalog=" & cnAdminDb & ";User ID=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";Password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & "")
            Using command As New SqlCommand("UpdatePurchaseOrder", connection)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddWithValue("@USERID", userId)
                command.Parameters.AddWithValue("@POFromDate", dtpFrom.Value)
                command.Parameters.AddWithValue("@POToDate", dtpTo.Value)

                Dim tvpParameter As New SqlParameter("@PurchaseOrderData", SqlDbType.Structured)
                tvpParameter.Value = purchaseOrderData
                tvpParameter.TypeName = "PurchaseOrderTableType"
                command.Parameters.Add(tvpParameter)
                connection.Open()
                command.ExecuteNonQuery()
                MessageBox.Show("Purchase Order posted successfully!")
            End Using
        End Using
    End Sub

    Private Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles BtnGenerate.Click
        Try
            strSql = $"select distinct ITEMID from {cnAdminDb}.. PURCHASEORDER where ISNULL(po_pieces,0)<>0 and POFROMDATE = '{dtpFrom.Value}' and POTODATE = '{dtpTo.Value}' and isnull(PONUMBER,'') = ''"
            Dim distinctDt As DataTable = GetSqlTable(strSql, cn)
            If distinctDt.Rows.Count = 0 Then Throw New Exception("No records found, to generate Purchase order.")
            For Each row As DataRow In distinctDt.Rows
                strSql = "select ISNULL(ctltext,0)+1 ctltext from " & cnStockDb & "..BILLCONTROL where CTLID = 'MET-O-PUR' and COMPANYID = '" & strCompanyId & "'"
                Dim poNum As Integer = objGPack.GetSqlValue(strSql, , , tran)
                Dim ActPoNum As String = $"{cnCostId}-{row("ITEMID").ToString()}-({dtpFrom.Value.ToString("ddMM")}/{dtpTo.Value.ToString("ddMM")}/{dtpTo.Value.ToString("yy")})-{poNum.ToString("0000")}"
                strSql = $"update {cnAdminDb}.. PURCHASEORDER set PONUMBER = '{ActPoNum}',PODATE = '{Now.Date}' where itemid = {row("ITEMID").ToString()} and ISNULL(po_pieces,0)<>0 and POFROMDATE = '{dtpFrom.Value}' and POTODATE = '{dtpTo.Value}' and isnull(PONUMBER,'') = ''"
                strSql += vbCrLf + $"update {cnStockDb}..BILLCONTROL set ctltext = '{poNum}' where CTLID = 'MET-O-PUR' and COMPANYID = '{strCompanyId}'"
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 100000
                cmd.ExecuteNonQuery()
            Next
            MessageBox.Show("Purchase Order generated successfully!")
            BtnGenerate.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub gridView_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "PO_PIECES" Then
            Dim editingControl As TextBox = TryCast(e.Control, TextBox)
            If editingControl IsNot Nothing Then
                AddHandler editingControl.KeyPress, AddressOf NumericOnly_KeyPress
            End If
        End If
    End Sub
    Private Sub NumericOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.D Then
                Dim _particular As String = gridView.Item("PARTICULAR", gridView.CurrentRow.Index).Value.ToString
                Dim _tags As String = gridView.Item("IPCS", gridView.CurrentRow.Index).Value.ToString
                If Not String.IsNullOrEmpty(_tags) Then
                    Dim dt As DataTable = dtDetail.Select("PARTICULAR = '" & _particular & "'").CopyToDataTable
                    Dim ofrmPurchaseOrderDetail As New frmPurchaseOrderDetail(dt, _particular)
                    If ofrmPurchaseOrderDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Else
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub chkCmbItem_Validated_1(sender As Object, e As EventArgs)
        funcLoadRange()
    End Sub
End Class
