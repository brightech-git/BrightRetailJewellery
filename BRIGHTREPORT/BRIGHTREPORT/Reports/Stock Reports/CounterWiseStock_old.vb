Imports System.Data.OleDb
Imports System.Xml
Public Class CounterWiseStock_old
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Private CheckOnly As Boolean = False

    Dim celWasEndEdit As DataGridViewCell = Nothing

    Public Sub New(ByVal CheckOnly As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        Me.CheckOnly = CheckOnly
        tabMain.SelectedTab = tabGen
    End Sub

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

    Function funcLoadCostCentre() As Integer
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Function funcLoadItemType() As Integer
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        chkLstItemType.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            chkLstItemType.Items.Add(dt.Rows(i).Item("NAME").ToString)
        Next
    End Function

    Function funcLoadCounter() As Integer
        strSql = " select itemCtrName from " & cnAdminDb & "..itemCounter WHERE ISNULL(ACTIVE,'')<>'N' order by ItemCtrName"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        chkLstCounter.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            chkLstCounter.Items.Add(dt.Rows(i).Item("ITEMCTRNAME").ToString)
        Next
    End Function

    Function funcLoadDesigner() As Integer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"
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
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        strSql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        If cmbMetal.Text <> "ALL" Then
            strSql += " AND  METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "')"
        End If
        strSql += "  ORDER BY ITEMID"
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Text = "ALL"
    End Function

    Function funcLoadMetal() As Integer
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        cmbMetal.Text = "ALL"
    End Function
    Private Sub GridStyle()
        ''
        FillGridGroupStyle_KeyNoWise(gridView)
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("PARTICULAR").Width = 200
            .Columns("OPCS").Width = 70
            .Columns("OGRSWT").Width = 100
            .Columns("ONETWT").Width = 100
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                .Columns("APPOPCS").Width = 70
                .Columns("APPOGRSWT").Width = 100
                .Columns("APPONETWT").Width = 100
            End If

            .Columns("TRPCS").Width = 70
            .Columns("TRGRSWT").Width = 100
            .Columns("TRNETWT").Width = 100

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                .Columns("APPRPCS").Width = 70
                .Columns("APPRGRSWT").Width = 100
                .Columns("APPRNETWT").Width = 100
            End If
            .Columns("TIPCS").Width = 70
            .Columns("TIGRSWT").Width = 100
            .Columns("TINETWT").Width = 100

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                .Columns("APPIPCS").Width = 70
                .Columns("APPIGRSWT").Width = 100
                .Columns("APPINETWT").Width = 100
            End If
            .Columns("MIPCS").Width = 70
            .Columns("MIGRSWT").Width = 100
            .Columns("MINETWT").Width = 100

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                .Columns("APPCPCS").Width = 70
                .Columns("APPCGRSWT").Width = 100
                .Columns("APPCNETWT").Width = 100
            End If
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "O.PCS" Else .Columns("OPCS").HeaderText = "TAG"
            .Columns("OGRSWT").HeaderText = "O.GRSWT"
            .Columns("ONETWT").HeaderText = "O.NETWT"

            If Not chkOnlyTag.Checked Then .Columns("TRPCS").HeaderText = "T.PCS" Else .Columns("TRPCS").HeaderText = "TAG"
            .Columns("TRGRSWT").HeaderText = "T.GRSWT"
            .Columns("TRNETWT").HeaderText = "T.NETWT"

            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "R.PCS" Else .Columns("RPCS").HeaderText = "TAG"
            .Columns("RGRSWT").HeaderText = "R.GRSWT"
            .Columns("RNETWT").HeaderText = "R.NETWT"

            If Not chkOnlyTag.Checked Then .Columns("TIPCS").HeaderText = "T.PCS" Else .Columns("TIPCS").HeaderText = "TAG"
            .Columns("TIGRSWT").HeaderText = "T.GRSWT"
            .Columns("TINETWT").HeaderText = "T.NETWT"

            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "I.PCS" Else .Columns("IPCS").HeaderText = "TAG"
            .Columns("IGRSWT").HeaderText = "I.GRSWT"
            .Columns("INETWT").HeaderText = "I.NETWT"

            If Not chkOnlyTag.Checked Then .Columns("MIPCS").HeaderText = "M.PCS" Else .Columns("MIPCS").HeaderText = "TAG"
            .Columns("MIGRSWT").HeaderText = "M.GRSWT"
            .Columns("MINETWT").HeaderText = "M.NETWT"


            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "C.PCS" Else .Columns("CPCS").HeaderText = "TAG"
            .Columns("CGRSWT").HeaderText = "C.GRSWT"
            .Columns("CNETWT").HeaderText = "C.NETWT"

            ''BACKCOLOR
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("TIPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TINETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MIPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MINETWT").DefaultCellStyle.BackColor = Color.AliceBlue


            ''VISIBLE
            If CheckOnly Then
                .Columns("CHPCS").Visible = True
                .Columns("CHGRSWT").Visible = chkGrsWt.Checked
                .Columns("CHNETWT").Visible = chkNetWt.Checked
                .Columns("CHPCS").ReadOnly = False
                .Columns("CHGRSWT").ReadOnly = False
                .Columns("CHNETWT").ReadOnly = False
                .Columns("CHPCS").HeaderText = "PCS"
                .Columns("CHGRSWT").HeaderText = "GRSWT"
                .Columns("CHNETWT").HeaderText = "NETWT"
            End If
            .Columns("OPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("IPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
            .Columns("RPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
            .Columns("CPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            .Columns("OGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("TRGRSWT").Visible = chkGrsWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("RGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TIGRSWT").Visible = chkGrsWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("IGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MIGRSWT").Visible = chkGrsWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("CGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            .Columns("ONETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("TRNETWT").Visible = chkNetWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TINETWT").Visible = chkNetWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MINETWT").Visible = chkNetWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("CNETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            .Columns("TRPCS").Visible = chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TIPCS").Visible = chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MIPCS").Visible = chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                .Columns("APPOPCS").Visible = True
                .Columns("APPOGRSWT").Visible = True
                .Columns("APPONETWT").Visible = True
                .Columns("APPRPCS").Visible = True
                .Columns("APPRGRSWT").Visible = True
                .Columns("APPRNETWT").Visible = True
                .Columns("APPIPCS").Visible = True
                .Columns("APPIGRSWT").Visible = True
                .Columns("APPINETWT").Visible = True
                .Columns("APPCPCS").Visible = True
                .Columns("APPCGRSWT").Visible = True
                .Columns("APPCNETWT").Visible = True
            Else
                If GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then
                    .Columns("APPOPCS").Visible = False
                    .Columns("APPOGRSWT").Visible = False
                    .Columns("APPONETWT").Visible = False
                    .Columns("APPRPCS").Visible = False
                    .Columns("APPRGRSWT").Visible = False
                    .Columns("APPRNETWT").Visible = False
                    .Columns("APPIPCS").Visible = False
                    .Columns("APPIGRSWT").Visible = False
                    .Columns("APPINETWT").Visible = False
                    .Columns("APPCPCS").Visible = False
                    .Columns("APPCGRSWT").Visible = False
                    .Columns("APPCNETWT").Visible = False
                End If

            End If
            '.Columns("TRGRSWT").Visible = chkSeperateTransferCol.Checked
            '.Columns("TIGRSWT").Visible = chkSeperateTransferCol.Checked
            '.Columns("MIGRSWT").Visible = chkSeperateTransferCol.Checked

            '.Columns("TRNETWT").Visible = chkSeperateTransferCol.Checked
            '.Columns("TINETWT").Visible = chkSeperateTransferCol.Checked
            '.Columns("MINETWT").Visible = chkSeperateTransferCol.Checked

            .Columns("ITEMCTRNAME").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False

        End With
    End Sub
    Public Function GetSelectedMetalid(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT Metalid FROM " & cnAdminDb & "..MetalMast WHERE MetalName= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelecteditemids(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Public Function GetSelectedDesignerid(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
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

    Public Function GetSelecteditemtypeid(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
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

    Public Function GetSelectedCostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function

    Public Function GetSelectedCounderid(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
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
    Private Sub Report()
        Dim RecDate As String = Nothing
        gridView.DataSource = Nothing
        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        Dim SelectedCompany As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim SelectedCounter As String = GetChecked_CheckedList(chkLstCounter, False)
        Dim SelectedItemType As String = GetChecked_CheckedList(chkLstItemType, False)
        Dim SelectedCostCentre As String = GetChecked_CheckedList(chkLstCostCentre, False)
        Dim TranCol As String = "P"
        If chkGrsWt.Checked Then TranCol += "G"
        If chkNetWt.Checked Then TranCol += "N"
        If GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "Y" Then

            strSql = " EXEC " & cnStockDb & "..SP_RPT_COUNTERWISESTOCK_OLD"
            strSql += vbCrLf + " @ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METAL = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@CATNAME = '" & cmbCategory.Text & "'"
            strSql += vbCrLf + " ,@ITEMNAME = '" & cmbItemName.Text & "'"
            strSql += vbCrLf + " ,@DESIGNER = '" & cmbDesigner.Text & "'"
            strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSummary.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
            If rbtNonTag.Checked Then
                strSql += vbCrLf + ",@STOCKTYPE = 'N'"
            ElseIf rbtTag.Checked Then
                strSql += vbCrLf + ",@STOCKTYPE = 'T'"
            Else
                strSql += vbCrLf + ",@STOCKTYPE = 'B'"
            End If
            strSql += vbCrLf + " ,@COSTNAME = '" & SelectedCostCentre & "'"
            strSql += vbCrLf + " ,@ITEMTYPE = '" & SelectedItemType & "'"
            strSql += vbCrLf + " ,@COUNTER = '" & SelectedCounter & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
            strSql += vbCrLf + " ,@ORDER = '" & IIf(rbtOrder.Checked, "O", "") & "'"
            If chkWithApproval.Checked = True And chkOnlyApproval.Checked = False Then
                strSql += vbCrLf + " ,@APPROVAL = 'A'"
            ElseIf chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
                strSql += vbCrLf + " ,@APPROVAL = ''"
            Else
                strSql += vbCrLf + " ,@APPROVAL = 'O'"
            End If
            strSql += vbCrLf + " ,@TRANSFER_DETAIL = '" & IIf(chkSeperateTransferCol.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@TRANSACTIONONLY = '" & IIf(chkTransactionOnly.Checked, TranCol, "") & "'"
            strSql += vbCrLf + " ,@ORDERBYID = '" & IIf(chkOrderbyId.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPTABLEDB..TEMPSTOCKREPORT ORDER BY ITEMCTRNAME,RESULT,ITEMNAME"
        Else
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_COUNTERWISESTOCK"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METAL = '" & GetSelectedMetalid(cmbMetal, False) & "'"
            strSql += vbCrLf + " ,@CATCODE = '" & GetSelectedCatCode(cmbCategory, False) & "'"
            strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItemName, False) & "'"
            strSql += vbCrLf + " ,@DESIGNERID = '" & GetSelectedDesignerid(cmbDesigner, False) & "'"
            strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSummary.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
            If rbtNonTag.Checked Then
                strSql += vbCrLf + ",@STOCKTYPE = 'N'"
            ElseIf rbtTag.Checked Then
                strSql += vbCrLf + ",@STOCKTYPE = 'T'"
            Else
                strSql += vbCrLf + ",@STOCKTYPE = 'B'"
            End If
            strSql += vbCrLf + " ,@COSTIDS = '" & IIf(chkAllCostCentre.Checked = True, "ALL", GetSelectedCostId(chkLstCostCentre, False)) & "'"
            strSql += vbCrLf + " ,@ITEMTYPEIDS = '" & IIf(chkAllItemType.Checked = True, "ALL", GetSelecteditemtypeid(chkLstItemType, False)) & "'"
            strSql += vbCrLf + " ,@COUNTERID = '" & IIf(chkAllCounter.Checked = True, "ALL", GetSelectedCounderid(chkLstCounter, False)) & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & IIf(chkCompanySelectAll.Checked = True, "ALL", SelectedCompany) & "'"
            strSql += vbCrLf + " ,@ORDER = '" & IIf(rbtOrder.Checked, "O", "") & "'"
            If chkWithApproval.Checked = True And chkOnlyApproval.Checked = False Then
                strSql += vbCrLf + " ,@APPROVAL = 'A'"
            ElseIf chkWithApproval.Checked = False And chkOnlyApproval.Checked = False Then
                strSql += vbCrLf + " ,@APPROVAL = ''"
            Else
                strSql += vbCrLf + " ,@APPROVAL = 'O'"
            End If
            strSql += vbCrLf + " ,@TRANSFER_DETAIL = '" & IIf(chkSeperateTransferCol.Checked, "Y", "N") & "'"
            strSql += vbCrLf + " ,@TRANSACTIONONLY = '" & IIf(chkTransactionOnly.Checked, TranCol, "") & "'"
            strSql += vbCrLf + " ,@ORDERBYID = '" & IIf(chkOrderbyId.Checked, "Y", "N") & "'"
            ' Dim GUID As New System.Guid
            ' LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
            Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "STOCKREPORT ORDER BY ITEMCTRNAME,RESULT,ITEMNAME"
        End If


        Dim dt As New DataTable
        dt.Columns.Add("KEYNO", GetType(Integer))
        dt.Columns("KEYNO").AutoIncrement = True
        dt.Columns("KEYNO").AutoIncrementSeed = 0
        dt.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Prop_Sets()
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpAsOnDate.Focus()
            Exit Sub
        End If
        dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        If CheckOnly Then
            gridViewHead.Visible = False
            dt.Columns.Add("CHPCS", GetType(Integer))
            dt.Columns.Add("CHGRSWT", GetType(Decimal))
            dt.Columns.Add("CHNETWT", GetType(Decimal))
            dt.Columns.Add("STATUS", GetType(String))
        End If
        tabView.Show()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dt
        'FOR PENDING TRANSFER
        Dim psds As New DataSet
        Dim psdt As New DataTable
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_PENDINGSTK"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(cmbMetal, False) & "'"
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItemName, False) & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & IIf(chkCompanySelectAll.Checked = True, "ALL", SelectedCompany) & "'"
        strSql += vbCrLf + " ,@COSTIDS = '" & IIf(chkAllCostCentre.Checked = True, "ALL", GetSelectedCostId(chkLstCostCentre, False)) & "'"
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
                    gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("PCS")
                    gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("GRSWT")
                    gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("NETWT")
                End With
            Next
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "TOTAL"
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = psdt.Compute("SUM(PCS)", "")
            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = psdt.Compute("SUM(GRSWT)", "")
            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = psdt.Compute("SUM(NETWT)", "")
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

        End If
        lblTitle.Text = "COUNTER WISE STOCK CHECK"
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
            lblTitle.Text += " FOR [" & cmbMetal.Text & "]"
        End If
        lblTitle.Text += " AS ON " & dtpAsOnDate.Value.ToString("dd-MM-yyyy")
        GridViewHeaderStyle()
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        GridViewHeaderStyle()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strapproval As String = ""
        If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then strapproval = "~APPOPCS~APPOGRSWT~APPONETWT~OTAGPCS"
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPCS~OGRSWT~ONETWT~OTAGPCS" & strapproval, GetType(String))
            .Columns.Add("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS" & strapproval, GetType(String))
            .Columns.Add("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS" & strapproval, GetType(String))
            .Columns.Add("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval, GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS" & strapproval).Caption = "OPENING"
            .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS" & strapproval).Caption = "RECEIPT"
            .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS" & strapproval).Caption = "ISSUE"
            .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Caption = "CLOSING"
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
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
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If chkOnlyTag.Checked = True And rbtTag.Checked = False Then
            MsgBox("Filtering allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkAllCostCentre.Checked = True
        End If
        If Not chkLstCounter.CheckedItems.Count > 0 Then
            chkAllCounter.Checked = True
        End If
        If Not chkLstItemType.CheckedItems.Count > 0 Then
            chkAllItemType.Checked = True
        End If
        Me.Refresh()
        Report()
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit
        If gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "T" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        celWasEndEdit = gridView(e.ColumnIndex, e.RowIndex)
        Dim pcs As Integer = 0
        Dim grsWt As Decimal = 0
        Dim netWt As Decimal = 0
        With gridView.Rows(e.RowIndex)
            pcs = Val(.Cells("CPCS").Value.ToString) - Val(.Cells("CHPCS").Value.ToString)
            If chkGrsWt.Checked Then
                grsWt = Val(.Cells("CGRSWT").Value.ToString) - Val(.Cells("CHGRSWT").Value.ToString)
            End If
            If chkNetWt.Checked Then
                netWt = Val(.Cells("CNETWT").Value.ToString) - Val(.Cells("CHNETWT").Value.ToString)
            End If
            If pcs = 0 And grsWt = 0 And netWt = 0 Then
                .Cells("STATUS").Value = "Ok"
            Else
                .Cells("STATUS").Value = "DIFFER"
            End If
        End With

    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub

    Function funcColWidth() As Integer
        With gridViewHead
            Dim strapproval As String = ""
            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then strapproval = "~APPOPCS~APPOGRSWT~APPONETWT~OTAGPCS"
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width

            If chkWithApproval.Checked = True And GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "N" Then

                .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS" & strapproval).Visible = gridView.Columns("OPCS").Visible
                .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS" & strapproval).Width = _
                IIf(gridView.Columns("OPCS").Visible, gridView.Columns("OPCS").Width, 0) _
                + IIf(gridView.Columns("OGRSWT").Visible, gridView.Columns("OGRSWT").Width, 0) _
                + IIf(gridView.Columns("ONETWT").Visible, gridView.Columns("ONETWT").Width, 0) _
                + IIf(gridView.Columns("APPOPCS").Visible, gridView.Columns("APPOPCS").Width, 0) _
                + IIf(gridView.Columns("APPOGRSWT").Visible, gridView.Columns("APPOGRSWT").Width, 0) _
                + IIf(gridView.Columns("APPONETWT").Visible, gridView.Columns("APPONETWT").Width, 0)
                .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS" & strapproval).HeaderText = "OPENING"

                .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS" & strapproval).Visible = gridView.Columns("RPCS").Visible
                .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS" & strapproval).Width = _
                IIf(gridView.Columns("TRPCS").Visible, gridView.Columns("TRPCS").Width, 0) _
                + IIf(gridView.Columns("TRGRSWT").Visible, gridView.Columns("TRGRSWT").Width, 0) _
                + IIf(gridView.Columns("TRNETWT").Visible, gridView.Columns("TRNETWT").Width, 0) _
                + IIf(gridView.Columns("RPCS").Visible, gridView.Columns("RPCS").Width, 0) _
                + IIf(gridView.Columns("RGRSWT").Visible, gridView.Columns("RGRSWT").Width, 0) _
                + IIf(gridView.Columns("RNETWT").Visible, gridView.Columns("RNETWT").Width, 0) _
                + IIf(gridView.Columns("APPRPCS").Visible, gridView.Columns("RPCS").Width, 0) _
                + IIf(gridView.Columns("APPRGRSWT").Visible, gridView.Columns("RGRSWT").Width, 0) _
                + IIf(gridView.Columns("APPRNETWT").Visible, gridView.Columns("RNETWT").Width, 0)
                .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS" & strapproval).HeaderText = "RECEIPT"

                .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS" & strapproval).Visible = gridView.Columns("IPCS").Visible
                .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS" & strapproval).Width = _
                IIf(gridView.Columns("TIPCS").Visible, gridView.Columns("TIPCS").Width, 0) _
                + gridView.Columns("IPCS").Width _
                + IIf(gridView.Columns("MIPCS").Visible, gridView.Columns("MIPCS").Width, 0) _
                + IIf(gridView.Columns("TIGRSWT").Visible, gridView.Columns("TIGRSWT").Width, 0) _
                + IIf(gridView.Columns("IGRSWT").Visible, gridView.Columns("IGRSWT").Width, 0) _
                + IIf(gridView.Columns("MIGRSWT").Visible, gridView.Columns("MIGRSWT").Width, 0) _
                + IIf(gridView.Columns("TINETWT").Visible, gridView.Columns("TINETWT").Width, 0) _
                + IIf(gridView.Columns("INETWT").Visible, gridView.Columns("INETWT").Width, 0) _
                + IIf(gridView.Columns("MINETWT").Visible, gridView.Columns("MINETWT").Width, 0) _
                + IIf(gridView.Columns("APPIPCS").Visible, gridView.Columns("APPIPCS").Width, 0) _
                + IIf(gridView.Columns("APPIGRSWT").Visible, gridView.Columns("APPIGRSWT").Width, 0) _
                + IIf(gridView.Columns("APPINETWT").Visible, gridView.Columns("APPINETWT").Width, 0)
                .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS" & strapproval).HeaderText = "ISSUE"

                .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Visible = gridView.Columns("CPCS").Visible
                .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width = gridView.Columns("CPCS").Width
                If chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width += gridView.Columns("CGRSWT").Width
                If chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width += gridView.Columns("CNETWT").Width
                If chkWithApproval.Checked = True Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width += gridView.Columns("APPCPCS").Width
                If chkWithApproval.Checked = True Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width += gridView.Columns("APPCGRSWT").Width
                If chkWithApproval.Checked = True Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).Width += gridView.Columns("APPCNETWT").Width
                'If chkOnlyTag.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width = gridView.Columns("CTAGPCS").Width
                .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS" & strapproval).HeaderText = "CLOSING"

            Else
                .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS").Width = gridView.Columns("OPCS").Width
                If chkGrsWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS").Width += gridView.Columns("OGRSWT").Width
                If chkNetWt.Checked Then .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS").Width += gridView.Columns("ONETWT").Width
                'If chkOnlyTag.Checked Then .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS").Width = gridView.Columns("OTAGPCS").Width
                .Columns("OPCS~OGRSWT~ONETWT~OTAGPCS").HeaderText = "OPENING"

                .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS").Width = IIf(gridView.Columns("TRPCS").Visible, gridView.Columns("TRPCS").Width, 0) + gridView.Columns("RPCS").Width
                If chkGrsWt.Checked Then .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS").Width += gridView.Columns("TRGRSWT").Width + gridView.Columns("RGRSWT").Width
                If chkNetWt.Checked Then .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS").Width += gridView.Columns("TRNETWT").Width + gridView.Columns("RNETWT").Width
                'If chkOnlyTag.Checked Then .Columns("RPCS~RGRSWT~RNETWT~RTAGPCS").Width = gridView.Columns("RTAGPCS").Width
                .Columns("TRPCS~TRGRSWT~TRNETWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RTAGPCS").HeaderText = "RECEIPT"

                .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS").Width = IIf(gridView.Columns("TIPCS").Visible, gridView.Columns("TIPCS").Width, 0) + gridView.Columns("IPCS").Width + gridView.Columns("MIPCS").Width
                If chkGrsWt.Checked Then .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS").Width += gridView.Columns("TIGRSWT").Width + gridView.Columns("IGRSWT").Width + gridView.Columns("MIGRSWT").Width
                If chkNetWt.Checked Then .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS").Width += gridView.Columns("TINETWT").Width + gridView.Columns("INETWT").Width + gridView.Columns("MINETWT").Width
                'If chkOnlyTag.Checked Then .Columns("IPCS~IGRSWT~INETWT~ITAGPCS").Width = gridView.Columns("ITAGPCS").Width
                .Columns("TIPCS~TIGRSWT~TINETWT~TITAGPCS~IPCS~IGRSWT~INETWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS").HeaderText = "ISSUE"

                .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width = gridView.Columns("CPCS").Width
                If chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width += gridView.Columns("CGRSWT").Width
                If chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width += gridView.Columns("CNETWT").Width
                'If chkOnlyTag.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width = gridView.Columns("CTAGPCS").Width
                .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").HeaderText = "CLOSING"

            End If
            
            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub
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

    Private Sub chkLstCounter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCounter.GotFocus
        If chkLstCounter.Items.Count > 0 Then
            chkLstCounter.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCounter.LostFocus
        chkLstCounter.ClearSelected()
    End Sub

    Private Sub chkLstItemType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItemType.GotFocus
        If chkLstItemType.Items.Count > 0 Then
            chkLstItemType.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstItemType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItemType.LostFocus
        chkLstItemType.ClearSelected()
    End Sub

    Private Sub chkLstCostCentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.GotFocus
        If chkLstCostCentre.Items.Count > 0 Then
            chkLstCostCentre.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.LostFocus
        chkLstCostCentre.ClearSelected()
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

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        funcLoadCategory()
        funcLoadItemName()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcLoadMetal()
        funcLoadCategory()
        funcLoadItemName()
        funcLoadDesigner()
        funcLoadCounter()
        funcLoadItemType()
        chkLstCostCentre.Items.Clear()
        If chkLstCostCentre.Enabled = True Then
            funcLoadCostCentre()
        End If
        Prop_Gets()
        If CheckOnly Then
            chkSeperateTransferCol.Checked = False
            chkSeperateTransferCol.Visible = False
            rbtWithBoth.Visible = False
            rbtWithClosing.Visible = False
            rbtWithOpening.Visible = False
        End If

        'rbtDetailed.Checked = True
        'chkAllCostCentre.Checked = False
        'chkAllItemType.Checked = False
        'chkAllCounter.Checked = False
        'chkSeperateTransferCol.Checked = True
        'chkTransactionOnly.Checked = False

        'gridView.DataSource = Nothing
        dtpAsOnDate.Value = GetServerDate()
        cmbMetal.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        LoadCompany(chkLstCompany)
        transitCheck()
        btnNew_Click(Me, New EventArgs)
        If GetAdmindbSoftValue("CTRSTK_REP_OLD", "N") = "Y" Then
            chkWithApproval.Checked = False
        Else
            chkWithApproval.Checked = True
        End If
    End Sub
    Private Function transitCheck()
        If objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMTAG", , 0, ) > 0 Or objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMNONTAG", , 0, ) > 0 Then
            MsgBox("Stock available in In-Transit " & vbCrLf & vbCrLf & "Please download it", MsgBoxStyle.Information)
        End If
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbMetal.Focus()
    End Sub

    Private Sub chkWithApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWithApproval.Click
        If chkOnlyApproval.Checked = True Then chkOnlyApproval.Checked = False
    End Sub

    Private Sub chkOnlyApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyApproval.Click
        If chkWithApproval.Checked = True Then chkWithApproval.Checked = False
    End Sub

    Private Sub chkOnlyTag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyTag.Click
        If chkOnlyTag.Checked = True Then
            chkNetWt.Checked = False
            chkGrsWt.Checked = False
            chkGrsWt.Enabled = False
            chkNetWt.Enabled = False
        Else
            chkGrsWt.Enabled = True
            chkNetWt.Enabled = True
        End If
    End Sub

    Private Sub chkAllCostCentre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllCostCentre.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkAllCostCentre.Checked)
    End Sub

    Private Sub chkAllCounter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllCounter.CheckedChanged
        SetChecked_CheckedList(chkLstCounter, chkAllCounter.Checked)
    End Sub

    Private Sub chkAllItemType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllItemType.CheckedChanged
        SetChecked_CheckedList(chkLstItemType, chkAllItemType.Checked)
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        ' if selection is changed after Cell Editing
        If CheckOnly Then
            If celWasEndEdit IsNot Nothing AndAlso _
               gridView.CurrentCell IsNot Nothing Then
                ' if we are currently in the next line of last edit cell
                If (gridView.CurrentCell.RowIndex = celWasEndEdit.RowIndex + 1 AndAlso _
                   gridView.CurrentCell.ColumnIndex = celWasEndEdit.ColumnIndex) Then
                    Dim iColNew As Integer
                    Dim iRowNew As Integer
                    ' if we at the last column
                    If celWasEndEdit.ColumnIndex >= gridView.ColumnCount - 1 Then
                        iColNew = 0                         ' move to first column
                        iRowNew = gridView.CurrentCell.RowIndex   ' and move to next row
                    Else ' else it means we are NOT at the last column
                        ' move to next column
                        iColNew = celWasEndEdit.ColumnIndex + 1
                        ' but row should remain same
                        iRowNew = celWasEndEdit.RowIndex
                    End If
                    celWasEndEdit = gridView(iColNew, iRowNew)   ' ok set the current column
                    If Not celWasEndEdit.Visible Or celWasEndEdit.ReadOnly Then
                        For rIndex As Integer = iRowNew To gridView.RowCount - 1
                            'If gridView.Rows(rIndex).Cells("COLHEAD").Value.ToString <> "" Then Continue For
                            For cIndex As Integer = iColNew To gridView.Columns.Count - 1
                                celWasEndEdit = gridView.Rows(rIndex).Cells(cIndex)
                                If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
                            Next
                            If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
                            iColNew = 0
                        Next
                    End If
                    gridView.CurrentCell = celWasEndEdit
                End If
            End If
            celWasEndEdit = Nothing                      ' reset the cell end edit
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
        Dim obj As New CounterWiseStock_old_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkAllCounter = chkAllCounter.Checked
        GetChecked_CheckedList(chkLstCounter, obj.p_chkLstCounter)
        obj.p_chkAllItemType = chkAllItemType.Checked
        GetChecked_CheckedList(chkLstItemType, obj.p_chkLstItemType)
        obj.p_chkAllCostCentre = chkAllCostCentre.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        obj.p_rbtWithBoth = rbtWithBoth.Checked
        obj.p_rbtWithOpening = rbtWithOpening.Checked
        obj.p_rbtWithClosing = rbtWithClosing.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkWithSubItem = chkWithSubItem.Checked
        obj.p_chkSeperateTransferCol = chkSeperateTransferCol.Checked
        obj.p_chkWithApproval = chkWithApproval.Checked
        obj.p_chkOnlyApproval = chkOnlyApproval.Checked
        obj.p_chkTransactionOnly = chkTransactionOnly.Checked
        obj.p_chkOrderbyId = chkOrderbyId.Checked
        SetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(CounterWiseStock_old_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New CounterWiseStock_old_Properties
        GetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(CounterWiseStock_old_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbCategory.Text = obj.p_cmbCategory
        cmbItemName.Text = obj.p_cmbItemName
        cmbDesigner.Text = obj.p_cmbDesigner
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkAllCounter.Checked = obj.p_chkAllCounter
        SetChecked_CheckedList(chkLstCounter, obj.p_chkLstCounter, Nothing)
        chkAllItemType.Checked = obj.p_chkAllItemType
        SetChecked_CheckedList(chkLstItemType, obj.p_chkLstItemType, Nothing)
        'chkAllCostCentre.Checked = obj.p_chkAllCostCentre
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        rbtBoth.Checked = obj.p_rbtBoth
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
        rbtWithBoth.Checked = obj.p_rbtWithBoth
        rbtWithOpening.Checked = obj.p_rbtWithOpening
        rbtWithClosing.Checked = obj.p_rbtWithClosing
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkWithSubItem.Checked = obj.p_chkWithSubItem
        chkSeperateTransferCol.Checked = obj.p_chkSeperateTransferCol
        chkWithApproval.Checked = obj.p_chkWithApproval
        chkOnlyApproval.Checked = obj.p_chkOnlyApproval
        chkTransactionOnly.Checked = obj.p_chkTransactionOnly
        chkOrderbyId.Checked = obj.p_chkOrderbyId
    End Sub
End Class


Public Class CounterWiseStock_old_Properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property

    Private chkAllCounter As Boolean = False
    Public Property p_chkAllCounter() As Boolean
        Get
            Return chkAllCounter
        End Get
        Set(ByVal value As Boolean)
            chkAllCounter = value
        End Set
    End Property

    Private chkAllItemType As Boolean = False
    Public Property p_chkAllItemType() As Boolean
        Get
            Return chkAllItemType
        End Get
        Set(ByVal value As Boolean)
            chkAllItemType = value
        End Set
    End Property

    Private chkAllCostCentre As Boolean = False
    Public Property p_chkAllCostCentre() As Boolean
        Get
            Return chkAllCostCentre
        End Get
        Set(ByVal value As Boolean)
            chkAllCostCentre = value
        End Set
    End Property

    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            If Not chkLstCompany.Count > 0 Then
                chkLstCompany.Add(strCompanyName)
            End If
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property

    Private chkLstCounter As New List(Of String)
    Public Property p_chkLstCounter() As List(Of String)
        Get
            Return chkLstCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCounter = value
        End Set
    End Property

    Private chkLstItemType As New List(Of String)
    Public Property p_chkLstItemType() As List(Of String)
        Get
            Return chkLstItemType
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemType = value
        End Set
    End Property


    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
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

    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
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

    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
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

    Private chkWithSubItem As Boolean = False
    Public Property p_chkWithSubItem() As Boolean
        Get
            Return chkWithSubItem
        End Get
        Set(ByVal value As Boolean)
            chkWithSubItem = value
        End Set
    End Property

    Private chkSeperateTransferCol As Boolean = True
    Public Property p_chkSeperateTransferCol() As Boolean
        Get
            Return chkSeperateTransferCol
        End Get
        Set(ByVal value As Boolean)
            chkSeperateTransferCol = value
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

    Private chkOnlyApproval As Boolean = False
    Public Property p_chkOnlyApproval() As Boolean
        Get
            Return chkOnlyApproval
        End Get
        Set(ByVal value As Boolean)
            chkOnlyApproval = value
        End Set
    End Property

    Private chkTransactionOnly As Boolean = False
    Public Property p_chkTransactionOnly() As Boolean
        Get
            Return chkTransactionOnly
        End Get
        Set(ByVal value As Boolean)
            chkTransactionOnly = value
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

    Private rbtWithBoth As Boolean = True
    Public Property p_rbtWithBoth() As Boolean
        Get
            Return rbtWithBoth
        End Get
        Set(ByVal value As Boolean)
            rbtWithBoth = value
        End Set
    End Property
    Private rbtWithOpening As Boolean = False
    Public Property p_rbtWithOpening() As Boolean
        Get
            Return rbtWithOpening
        End Get
        Set(ByVal value As Boolean)
            rbtWithOpening = value
        End Set
    End Property
    Private rbtWithClosing As Boolean = False
    Public Property p_rbtWithClosing() As Boolean
        Get
            Return rbtWithClosing
        End Get
        Set(ByVal value As Boolean)
            rbtWithClosing = value
        End Set
    End Property
    Private chkOrderbyId As Boolean = False
    Public Property p_chkOrderbyId() As Boolean
        Get
            Return chkOrderbyId
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyId = value
        End Set
    End Property
End Class