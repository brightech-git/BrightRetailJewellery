Imports System.Data.OleDb
Imports System.Xml
Public Class frmCounterStockDiary
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
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
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
            .Columns("OGRSWT").Width = 80
            .Columns("ONETWT").Width = 80
            .Columns("OSTNWT").Width = 80
            .Columns("ODIAWT").Width = 80
            .Columns("OSALVALUE").Width = 100

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 80
            .Columns("RNETWT").Width = 80
            .Columns("RSTNWT").Width = 80
            .Columns("RDIAWT").Width = 80
            .Columns("RSALVALUE").Width = 100

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 800
            .Columns("INETWT").Width = 800
            .Columns("ISTNWT").Width = 800
            .Columns("IDIAWT").Width = 800
            .Columns("ISALVALUE").Width = 100

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 800
            .Columns("CNETWT").Width = 800
            .Columns("CSTNWT").Width = 800
            .Columns("CDIAWT").Width = 800
            .Columns("CSALVALUE").Width = 100
            ''HEADER TEXT
            .Columns("OGRSWT").HeaderText = "GRSWT"
            .Columns("ONETWT").HeaderText = "NETWT"
            .Columns("OSTNWT").HeaderText = "STNWT"
            .Columns("ODIAWT").HeaderText = "DIAWT"
            .Columns("OSALVALUE").HeaderText = "AMOUNT"


            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RSTNWT").HeaderText = "STNWT"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RSALVALUE").HeaderText = "AMOUNT"


            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("ISTNWT").HeaderText = "STNWT"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("ISALVALUE").HeaderText = "AMOUNT"

            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            .Columns("CSTNWT").HeaderText = "STNWT"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSALVALUE").HeaderText = "AMOUNT"


            ''BACKCOLOR

            .Columns("RPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RSALVALUE").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("ISTNWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("ISALVALUE").DefaultCellStyle.BackColor = Color.LavenderBlush



            .Columns("ITEMCTRNAME").Visible = False
            '.Columns("ITEMNAME").Visible = False
            '.Columns("SUBITEMNAME").Visible = False
            .Columns("CATNAME").Visible = False
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
            retStr = "ALL"
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
            retStr = "ALL"
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
            retStr = "ALL"
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

        strSql = " EXEC " & cnAdminDb & "..SP_RPT_COUNTERSTOCKDIARY"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METAL = '" & GetSelectedMetalid(cmbMetal, False) & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & GetSelectedCatCode(cmbCategory, False) & "'"
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItemName, False) & "'"
        strSql += vbCrLf + " ,@DESIGNERID = '" & GetSelectedDesignerid(cmbDesigner, False) & "'"
        If rbtNonTag.Checked Then
            strSql += vbCrLf + ",@STOCKTYPE = 'N'"
        ElseIf rbtTag.Checked Then
            strSql += vbCrLf + ",@STOCKTYPE = 'T'"
        Else
            strSql += vbCrLf + ",@STOCKTYPE = 'B'"
        End If
        strSql += vbCrLf + " ,@COSTIDS = '" & IIf(chkAllCostCentre.Checked = True, "ALL", GetSelectedCostId(chkLstCostCentre, False)) & "'"
        strSql += vbCrLf + " ,@ITEMTYPEIDS = '" & IIf(chkAllItemType.Checked = True, "ALL", GetSelecteditemtypeid(chkLstItemType, False)) & "'"
        strSql += vbCrLf + " ,@COUNTERID = '" & GetSelectedCounderid(chkLstCounter, False) & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & IIf(chkCompanySelectAll.Checked = True, "ALL", SelectedCompany) & "'"
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "STOCKREPORT  ORDER BY CATNAME,RESULT,ITEMNAME"
        Dim dt As New DataTable
        dt.Columns.Add("KEYNO", GetType(Integer))

        dt.Columns("KEYNO").AutoIncrement = True
        dt.Columns("KEYNO").AutoIncrementSeed = 0
        dt.Columns("KEYNO").AutoIncrementStep = 1
        'If Is_Oldformat = "Y" Then
        '    dt.Columns.Add("APCS", GetType(Integer))
        '    dt.Columns.Add("ANETWT", GetType(Integer))
        '    dt.Columns.Add("AGRSWT", GetType(Integer))
        'End If
        da = New OleDbDataAdapter(cmd)
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
        lblTitle.Text = "COUNTER WISE STOCK CHECK"
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
            lblTitle.Text += " FOR [" & cmbMetal.Text & "]"
        End If
        lblTitle.Text += " AS ON " & dtpAsOnDate.Value.ToString("dd-MM-yyyy")
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        lblTitle.Text += Cname
        GridViewHeaderStyle()
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
       
        GridViewHeaderStyle()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPCS~OGRSWT~ONETWT~OSTNWT~ODIAWT~OSALVALUE", GetType(String))
            .Columns.Add("RPCS~RGRSWT~RNETWT~RSTNWT~RDIAWT~RSALVALUE", GetType(String))
            .Columns.Add("IPCS~IGRSWT~INETWT~ISTNWT~IDIAWT~ISALVALUE", GetType(String))
            .Columns.Add("CPCS~CGRSWT~CNETWT~CSTNWT~CDIAWT~CSALVALUE", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPCS~OGRSWT~ONETWT~OSTNWT~ODIAWT~OSALVALUE").Caption = "OPENING"
            .Columns("RPCS~RGRSWT~RNETWT~RSTNWT~RDIAWT~RSALVALUE").Caption = "RECEIPT"
            .Columns("IPCS~IGRSWT~INETWT~ISTNWT~IDIAWT~ISALVALUE").Caption = "ISSUE"
            .Columns("CPCS~CGRSWT~CNETWT~CSTNWT~CDIAWT~CSALVALUE").Caption = "CLOSING"
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
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkAllCostCentre.Checked = True
        If Not chkLstCounter.CheckedItems.Count > 0 Then chkAllCounter.Checked = True
        If Not chkLstItemType.CheckedItems.Count > 0 Then chkAllItemType.Checked = True
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
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            '.Columns("OPCS~OGRSWT~ONETWT~OSTNWT~ODIAWT~OSALVALUE").Visible = gridView.Columns("OPCS").Visible
            .Columns("OPCS~OGRSWT~ONETWT~OSTNWT~ODIAWT~OSALVALUE").Width = _
            IIf(gridView.Columns("OPCS").Visible, gridView.Columns("OPCS").Width, 0) _
            + IIf(gridView.Columns("OGRSWT").Visible, gridView.Columns("OGRSWT").Width, 0) _
            + IIf(gridView.Columns("ONETWT").Visible, gridView.Columns("ONETWT").Width, 0) _
            + IIf(gridView.Columns("OSTNWT").Visible, gridView.Columns("OSTNWT").Width, 0) _
            + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0) _
            + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT~OSTNWT~ODIAWT~OSALVALUE").HeaderText = "OPENING"

            '.Columns("RPCS~RGRSWT~RNETWT~RSTNWT~RDIAWT~RSALVALUE").Visible = gridView.Columns("RPCS").Visible
            .Columns("RPCS~RGRSWT~RNETWT~RSTNWT~RDIAWT~RSALVALUE").Width = _
            +IIf(gridView.Columns("RPCS").Visible, gridView.Columns("RPCS").Width, 0) _
            + IIf(gridView.Columns("RGRSWT").Visible, gridView.Columns("RGRSWT").Width, 0) _
            + IIf(gridView.Columns("RNETWT").Visible, gridView.Columns("RNETWT").Width, 0) _
            + IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNWT").Width, 0) _
            + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0) _
            + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0)
            .Columns("RPCS~RGRSWT~RNETWT~RSTNWT~RDIAWT~RSALVALUE").HeaderText = "RECEIPT"

            '.Columns("IPCS~IGRSWT~INETWT~ISTNWT~IDIAWT~ISALVALUE").Visible = gridView.Columns("IPCS").Visible
            .Columns("IPCS~IGRSWT~INETWT~ISTNWT~IDIAWT~ISALVALUE").Width = _
            IIf(gridView.Columns("IPCS").Visible, gridView.Columns("IPCS").Width, 0) _
            + IIf(gridView.Columns("IGRSWT").Visible, gridView.Columns("IGRSWT").Width, 0) _
            + IIf(gridView.Columns("INETWT").Visible, gridView.Columns("INETWT").Width, 0) _
            + IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNWT").Width, 0) _
            + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0) _
            + IIf(gridView.Columns("ISALVALUE").Visible, gridView.Columns("ISALVALUE").Width, 0)
            .Columns("IPCS~IGRSWT~INETWT~ISTNWT~IDIAWT~ISALVALUE").HeaderText = "ISSUE"

            '.Columns("CPCS~CGRSWT~CNETWT~CSTNWT~CDIAWT~CSALVALUE").Visible = gridView.Columns("CPCS").Visible
            .Columns("CPCS~CGRSWT~CNETWT~CSTNWT~CDIAWT~CSALVALUE").Width = _
            IIf(gridView.Columns("CPCS").Visible, gridView.Columns("CPCS").Width, 0) _
            + IIf(gridView.Columns("CGRSWT").Visible, gridView.Columns("CGRSWT").Width, 0) _
            + IIf(gridView.Columns("CNETWT").Visible, gridView.Columns("CNETWT").Width, 0) _
            + IIf(gridView.Columns("CSTNWT").Visible, gridView.Columns("CSTNWT").Width, 0) _
            + IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            .Columns("CPCS~CGRSWT~CNETWT~CSTNWT~CDIAWT~CSALVALUE").HeaderText = "CLOSING"

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
      
        dtpAsOnDate.Value = GetServerDate()
        cmbMetal.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbMetal.Focus()
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
        Dim obj As New frmCounterStockDiary_Properties
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
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked

        SetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(frmCounterStockDiary_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCounterStockDiary_Properties
        GetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(frmCounterStockDiary_Properties))
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
        chkAllCostCentre.Checked = obj.p_chkAllCostCentre
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        rbtBoth.Checked = obj.p_rbtBoth
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
    End Sub
End Class


Public Class frmCounterStockDiary_Properties
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
    Private chkSalvalue As Boolean = False
    Public Property p_chkSalvalue() As Boolean
        Get
            Return chkSalvalue
        End Get
        Set(ByVal value As Boolean)
            chkSalvalue = value
        End Set
    End Property
End Class