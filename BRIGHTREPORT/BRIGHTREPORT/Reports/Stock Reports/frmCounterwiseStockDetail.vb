Imports System.Data.OleDb
Imports System.Xml
Public Class frmCounterwiseStockDetail
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
    Dim objGridShower As frmGridDispDia

    Dim celWasEndEdit As DataGridViewCell = Nothing
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        'tabMain.SelectedTab = tabGen
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcLoadCounter() As Integer
        cmbCounter.Items.Clear()
        strSql = " select itemCtrName from " & cnAdminDb & "..itemCounter order by ItemCtrName"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        For Each ro As DataRow In dt.Rows
            cmbCounter.Items.Add(ro.Item("itemCtrName").ToString)
        Next
    End Function
    Function funcLoadDesigner() As Integer
        cmbDesig.Items.Clear()
        cmbDesig.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesig, False)
        cmbDesig.Text = "ALL"
    End Function
    Function funcLoadCategory() As Integer
        cmbCat.Items.Clear()
        cmbCat.Items.Add("ALL")
        strSql = " select CatName from " & cnAdminDb & "..Category "
        If cmbMetalName.Text <> "ALL" Then
            strSql += " where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname = '" & cmbMetalName.Text & "')"
        End If
        strSql += "  order by CatName"
        objGPack.FillCombo(strSql, cmbCat, False)
        cmbCat.Text = "ALL"
    End Function
    Function funcLoadItemName() As Integer
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        strSql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        If cmbMetalName.Text <> "ALL" Then
            strSql += " AND  METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "')"
        End If
        strSql += "  ORDER BY ITEMID"
        objGPack.FillCombo(strSql, cmbItem, False)
        cmbItem.Text = "ALL"
    End Function
    Function funcLoadMetal() As Integer
        cmbMetalName.Items.Clear()
        cmbMetalName.Items.Add("ALL")
        strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        objGPack.FillCombo(strSql, cmbMetalName, False, False)
        cmbMetalName.Text = "ALL"
    End Function
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

    Public Function GetSelectedCounderid() As String
        Dim retStr As String = ""
        retStr += "'"
        retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & cmbCounter.Text & "'")
        retStr += "'"
        Return retStr
    End Function
    Private Sub Report()
        Dim RecDate As String = Nothing
        'gridView.DataSource = Nothing
        If chkAsOnDate.Checked Then
            If Not CheckBckDays(userId, Me.Name, GetServerDate()) Then dtpAsOnDate.Focus() : Exit Sub
        Else
            If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        End If
        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        Dim SelectedCompany As String = GetSelectedCompanyId(chkListCmpny, False)
        Dim TranCol As String = "P"

        If cmbCounter.Text = "" Then
            MsgBox("Select Counter...")
            Exit Sub
        End If
        strSql = " EXEC " & cnAdminDb & "..SP_RPT_COUNTERWISEDETAIL"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        If chkAsOnDate.Checked Then
            strSql += vbCrLf + " ,@FRMDATE = '" & GetServerDate() & "'"
            strSql += vbCrLf + " ,@ASONDATE = '" & GetServerDate() & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ASONDATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(cmbMetalName, False) & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & GetSelectedCatCode(cmbCat, False) & "'"
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItem, False) & "'"
        strSql += vbCrLf + " ,@DESIGNERID = '" & GetSelectedDesignerid(cmbDesig, False) & "'"
        strSql += vbCrLf + " ,@COUNTERID = " & GetSelectedCounderid() & ""
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"

        Dim DtGrid As New DataTable("SUMMARY")
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)
        Prop_Sets()
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dim tit As String = ""
        tit = "COUNTER WISE STOCK DETAIL REPORT"
        tit += " AS ON " & dtpTo.Text & ""
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        CType(objGridShower.gridView.DataSource, DataTable).Rows.Add()
        CType(objGridShower.gridView.DataSource, DataTable).Rows.Add()
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COUNTERNAME").Value = "OPENING"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COLHEAD").Value = "OP"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

        CType(objGridShower.gridView.DataSource, DataTable).Rows.Add()
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COUNTERNAME").Value = "FILLED ITEMS"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COLHEAD").Value = "FI"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

        CType(objGridShower.gridView.DataSource, DataTable).Rows.Add()
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COUNTERNAME").Value = "SOLD ITEMS"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COLHEAD").Value = "SI"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

        CType(objGridShower.gridView.DataSource, DataTable).Rows.Add()
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COUNTERNAME").Value = "CLOSING"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).Cells("COLHEAD").Value = "CL"
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

        Dim Opcs, Ogrswt, Onetwt, Fpcs, Fgrswt, Fnetwt, Spcs, Sgrswt, Snetwt, Cpcs, Cgrswt, Cnetwt As Decimal
        For ind As Integer = 0 To objGridShower.gridView.RowCount - 1
            If objGridShower.gridView("SEP", ind).Value.ToString <> "" Then
                If objGridShower.gridView("SEP", ind).Value = "[A]OPENING" And objGridShower.gridView("COUNTERNAME", ind).Value.ToString = "TOTAL" Then
                    If objGridShower.gridView("PCS", ind).Value.ToString <> "" Then Opcs = objGridShower.gridView("PCS", ind).Value
                    If objGridShower.gridView("GRSWT", ind).Value.ToString <> "" Then Ogrswt = objGridShower.gridView("GRSWT", ind).Value
                    If objGridShower.gridView("NETWT", ind).Value.ToString <> "" Then Onetwt = objGridShower.gridView("NETWT", ind).Value
                End If
            End If
            If objGridShower.gridView("SEP", ind).Value.ToString <> "" Then
                If objGridShower.gridView("SEP", ind).Value = "[F]FILLED ITEMS" And objGridShower.gridView("COUNTERNAME", ind).Value.ToString = "TOTAL" Then
                    If objGridShower.gridView("PCS", ind).Value.ToString <> "" Then Fpcs = objGridShower.gridView("PCS", ind).Value
                    If objGridShower.gridView("GRSWT", ind).Value.ToString <> "" Then Fgrswt = objGridShower.gridView("GRSWT", ind).Value
                    If objGridShower.gridView("NETWT", ind).Value.ToString <> "" Then Fnetwt = objGridShower.gridView("NETWT", ind).Value
                End If
            End If
            If objGridShower.gridView("SEP", ind).Value.ToString <> "" Then
                If objGridShower.gridView("SEP", ind).Value = "[S]SOLD ITEMS" And objGridShower.gridView("COUNTERNAME", ind).Value.ToString = "TOTAL" Then
                    If objGridShower.gridView("PCS", ind).Value.ToString <> "" Then Spcs = objGridShower.gridView("PCS", ind).Value
                    If objGridShower.gridView("GRSWT", ind).Value.ToString <> "" Then Sgrswt = objGridShower.gridView("GRSWT", ind).Value
                    If objGridShower.gridView("NETWT", ind).Value.ToString <> "" Then Snetwt = objGridShower.gridView("NETWT", ind).Value
                End If
            End If
            If objGridShower.gridView("SEP", ind).Value.ToString <> "" Then
                If objGridShower.gridView("SEP", ind).Value = "CLOSING" And objGridShower.gridView("COLHEAD", ind).Value = "C" Then
                    If objGridShower.gridView("PCS", ind).Value.ToString <> "" Then Cpcs = objGridShower.gridView("PCS", ind).Value
                    If objGridShower.gridView("GRSWT", ind).Value.ToString <> "" Then Cgrswt = objGridShower.gridView("GRSWT", ind).Value
                    If objGridShower.gridView("NETWT", ind).Value.ToString <> "" Then Cnetwt = objGridShower.gridView("NETWT", ind).Value
                End If
            End If
            If objGridShower.gridView("COUNTERNAME", ind).Value.ToString <> "" Then
                If objGridShower.gridView("COUNTERNAME", ind).Value = "OPENING" And objGridShower.gridView("COLHEAD", ind).Value.ToString = "OP" Then
                    objGridShower.gridView("PCS", ind).Value = Opcs
                    objGridShower.gridView("GRSWT", ind).Value = Ogrswt
                    objGridShower.gridView("NETWT", ind).Value = Onetwt
                End If
            End If
            If objGridShower.gridView("COUNTERNAME", ind).Value.ToString <> "" Then
                If objGridShower.gridView("COUNTERNAME", ind).Value = "FILLED ITEMS" And objGridShower.gridView("COLHEAD", ind).Value.ToString = "FI" Then
                    objGridShower.gridView("PCS", ind).Value = Fpcs
                    objGridShower.gridView("GRSWT", ind).Value = Fgrswt
                    objGridShower.gridView("NETWT", ind).Value = Fnetwt
                End If
            End If
            If objGridShower.gridView("COUNTERNAME", ind).Value.ToString <> "" Then
                If objGridShower.gridView("COUNTERNAME", ind).Value = "SOLD ITEMS" And objGridShower.gridView("COLHEAD", ind).Value.ToString = "SI" Then
                    objGridShower.gridView("PCS", ind).Value = Spcs
                    objGridShower.gridView("GRSWT", ind).Value = Sgrswt
                    objGridShower.gridView("NETWT", ind).Value = Snetwt
                End If
            End If
            If objGridShower.gridView("COUNTERNAME", ind).Value.ToString <> "" Then
                If objGridShower.gridView("COUNTERNAME", ind).Value = "CLOSING" And objGridShower.gridView("COLHEAD", ind).Value.ToString = "CL" Then
                    objGridShower.gridView("PCS", ind).Value = Cpcs
                    objGridShower.gridView("GRSWT", ind).Value = Cgrswt
                    objGridShower.gridView("NETWT", ind).Value = Cnetwt
                End If
            End If
        Next
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = False
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridViewHeader.Visible = False
        GridViewFormat()
        objGridShower.gridView.Columns("SEP").Visible = False
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            'For Each dgvCol As DataGridViewColumn In dgv.Columns
            '    dgvCol.Visible = False
            '    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            'Next
            .Columns("COUNTERNAME").Width = 350
            .Columns("PCS").Width = 150
            .Columns("GRSWT").Width = 150
            .Columns("NETWT").Width = 150
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COUNTERNAME").Visible = True
            .Columns("TAGNO").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("SEP").Visible = False
            .Columns("ITEMCTRID").Visible = False
            .Columns("CCOUNTER").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False

            .Columns("COUNTERNAME").HeaderText = "COUNTER"
        End With
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "OP"
                        .DefaultCellStyle.BackColor = Color.LightPink
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "FI"
                        .DefaultCellStyle.BackColor = Color.LightPink
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "SI"
                        .DefaultCellStyle.BackColor = Color.LightPink
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "CL"
                        .DefaultCellStyle.BackColor = Color.LightPink
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcLoadMetal()
        funcLoadCategory()
        funcLoadItemName()
        funcLoadDesigner()
        funcLoadCounter()
        Prop_Gets()
        dtpFrom.Value = GetServerDate()
        cmbMetalName.Select()
    End Sub

    Private Sub butnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butnNew.Click
        funcLoadMetal()
        funcLoadCategory()
        funcLoadItemName()
        funcLoadDesigner()
        funcLoadCounter()
        Prop_Gets()
        dtpFrom.Value = GetServerDate()
        cmbMetalName.Select()
    End Sub

    Private Sub butnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butnExit.Click
        funcExit()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCounterwiseStockDetail_Properties
        obj.p_cmbMetal = cmbMetalName.Text
        obj.p_cmbCategory = cmbCat.Text
        obj.p_cmbItemName = cmbItem.Text
        obj.p_cmbDesigner = cmbDesig.Text
        obj.p_chkCompanySelectAll = chkCompanyAll.Checked
        GetChecked_CheckedList(chkListCmpny, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(frmCounterwiseStockDetail_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCounterwiseStockDetail_Properties
        GetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(frmCounterwiseStockDetail_Properties))
        cmbMetalName.Text = obj.p_cmbMetal
        cmbCat.Text = obj.p_cmbCategory
        cmbItem.Text = obj.p_cmbItemName
        cmbDesig.Text = obj.p_cmbDesigner
        chkCompanyAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkListCmpny, obj.p_chkLstCompany, strCompanyName)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Report()
    End Sub

    Private Sub frmCounterwiseStockDetail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCompany(chkListCmpny)
        butnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmCounterwiseStockDetail_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As On Date"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub
End Class
Public Class frmCounterwiseStockDetail_Properties
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
End Class