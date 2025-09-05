Imports System.Data.OleDb
Public Class frm4CReport
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim headerBgColor As New System.Drawing.Color
    Dim dtCompany As New DataTable
    Dim dtMetal As New DataTable
    Dim RowFillState As Boolean = False
    Dim Studded As Boolean = False
    Dim Type As String

    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frm4CReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagedItemsStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
            'ElseIf e.KeyChar = Chr(Keys.Enter) Then
        End If
    End Sub

    Private Sub frmTagedItemsStockView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Me.WindowState = FormWindowState.Maximized
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)

        pnlTotalGridView.Dock = DockStyle.Fill
        headerBgColor = System.Drawing.SystemColors.ControlLight

        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCenter.Enabled = True
        Else
            cmbCostCenter.Enabled = False
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click, btnExit.Click
        funcExit()
    End Sub

    Private Sub gridTotalView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.X) Or AscW(e.KeyChar) = 120 Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)

        ''ITEMNAME
        CmbItem.Items.Clear()
        CmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN('D','T') ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, CmbItem, False)
        CmbItem.Text = "ALL"

        ''SUBITEM
        cmbsubitem.Items.Clear()
        'cmbsubitem.Items.Add("ALL")
        'strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE METALID IN('D','T') ORDER BY ITEMNAME"
        'objGPack.FillCombo(strSql, cmbsubitem, False)
        cmbsubitem.Text = "ALL"


        ''STNCUT
        CmbCut.Items.Clear()
        CmbCut.Items.Add("ALL")
        strSql = "SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT ORDER BY CUTID"
        objGPack.FillCombo(strSql, CmbCut, False)
        CmbCut.Text = "ALL"

        ''STNCLARITY
        CmbClarity.Items.Clear()
        CmbClarity.Items.Add("ALL")
        strSql = "SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY ORDER BY CLARITYID"
        objGPack.FillCombo(strSql, CmbClarity, False)
        CmbClarity.Text = "ALL"

        ''STNCOLOR
        CmbColor.Items.Clear()
        CmbColor.Items.Add("ALL")
        strSql = "SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR ORDER BY COLORID"
        objGPack.FillCombo(strSql, CmbColor, False)
        CmbColor.Text = "ALL"

        ''STNSHAPE
        CmbShape.Items.Clear()
        CmbShape.Items.Add("ALL")
        strSql = "SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE ORDER BY SHAPEID"
        objGPack.FillCombo(strSql, CmbShape, False)
        CmbShape.Text = "ALL"

        ''STNSETTYPE
        CmbSettype.Items.Clear()
        CmbSettype.Items.Add("ALL")
        strSql = "SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE ORDER BY SETTYPEID"
        objGPack.FillCombo(strSql, CmbSettype, False)
        CmbSettype.Text = "ALL"

        ''CostCenter
        cmbCostCenter.Items.Clear()
        If cmbCostCenter.Enabled = True Then
            cmbCostCenter.Items.Add("ALL")
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCenter, False)
            cmbCostCenter.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCenter.Enabled = False
        End If
        Prop_Gets()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub btnView_Search_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        AutoResizeToolStripMenuItem.Checked = False
        gridView.DataSource = Nothing
        tabView.Show()
        Dim counter As String = GetID("COSTID", "COSTCENTRE", cnAdminDb, " COSTNAME", cmbCostCenter.Text)
        Dim itemid As String = GetID("ITEMID", "ITEMMAST", cnAdminDb, " ITEMNAME", CmbItem.Text)
        Dim subitemid As String = GetID("SUBITEMID", "SUBITEMMAST", cnAdminDb, " SUBITEMNAME", cmbsubitem.Text)
        Dim shape As String = GetID("SHAPEID", "STNSHAPE", cnAdminDb, " SHAPENAME", CmbShape.Text)
        Dim color As String = GetID("COLORID", "STNCOLOR", cnAdminDb, " COLORNAME", CmbColor.Text)
        Dim clarity As String = GetID("CLARITYID", "STNCLARITY", cnAdminDb, " CLARITYNAME", CmbClarity.Text)
        Dim cut As String = GetID("CUTID", "STNCUT", cnAdminDb, " CUTNAME", CmbCut.Text)
        Dim dtSource2 As New DataTable
        Dim ds As DataSet = New DataSet
        strSql = " EXEC " & cnAdminDb & "..RPT_PROC_FOURCSTOCKDETAILS"
        strSql += vbCrLf + " @COSTID = '" & counter & "'"
        strSql += vbCrLf + " ,@ITEMID = '" & itemid & "'"
        strSql += vbCrLf + " ,@SUBITEMID = '" & subitemid & "'"
        strSql += vbCrLf + " ,@SHAPE = '" & shape & "'"
        strSql += vbCrLf + " ,@COLOR = '" & color & "'"
        strSql += vbCrLf + " ,@CUT = '" & cut & "'"
        strSql += vbCrLf + " ,@CLARITY = '" & clarity & "'"
        strSql += vbCrLf + " ,@CARATFROM = '" & Val(txtCaratFrom.Text) & "'"
        strSql += vbCrLf + " ,@CARATTO = '" & Val(txtCaratTo.Text) & "'"
        strSql += vbCrLf + " ,@PRICEFROM = '" & Val(txtPriceFrom.Text) & "'"
        strSql += vbCrLf + " ,@PRICETO = '" & Val(txtPriceTo.Text) & "'"
        strSql += vbCrLf + " ,@RPTTYPE = '" & Type & "'"
        strSql += vbCrLf + " ,@SYSID = '" & systemId & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = "  SELECT PARTICULAR,ITEM,SUBITEM,STNITEM,STNSUBITEM,TAGNO,STNRATE,STNAMT ,STNWT ,CUT,COLOR,CLARITY,SHAPE,CERTIFICATENO,PCTFILE ,RESULT  FROM TEMPTABLEDB..TEMP" & systemId & "FOURCSTONE ORDER BY ITEM,RESULT  "
        dtSource2 = New DataTable
        dtSource2.Columns.Add("KEYNO", GetType(Integer))
        dtSource2.Columns("KEYNO").AutoIncrement = True
        dtSource2.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource2.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource2)
        If dtSource2.Rows.Count < -1 Then
            MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
            Exit Sub
        End If
        gridView.DataSource = dtSource2
        With gridView
            With .Columns("ITEM")
                .HeaderText = "ITEM"
                .Width = 150
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("PARTICULAR")
                .HeaderText = "ITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("SUBITEM")
                .HeaderText = "SUBITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("STNITEM")
                .HeaderText = "STNITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("STNSUBITEM")
                .HeaderText = "STNSUBITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("TAGNO")
                .HeaderText = "TAGNO"
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("STNRATE")
                .HeaderText = "STNRATE"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNAMT")
                .HeaderText = "STNAMT"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNWT")
                .HeaderText = "STNWT"
                .Width = 150
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("CUT")
                .HeaderText = "CUT"
                .Width = 150
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With

            With .Columns("COLOR")
                .HeaderText = "COLOR"
                .Width = 150
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("CLARITY")
                .HeaderText = "CLARITY"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With

            With .Columns("SHAPE")
                .HeaderText = "SHAPE"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With

            With .Columns("PCTFILE")
                .HeaderText = "PCTFILE"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("CERTIFICATENO")
                .HeaderText = "CERTIFICATENO"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RESULT")
                .HeaderText = "RESULT"
                .Width = 150
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("KEYNO")
                .HeaderText = "KEYNO"
                .Width = 150
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                'Select Case .Cells("PARTICULAR").Value.ToString
                '    Case "TOTAL"
                '        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                '        .DefaultCellStyle = reportSubTotalStyle2
                'End Select
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle = reportColumnHeadStyle
                    Case "3"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle = reportSubTotalStyle
                    Case "4"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle = reportSubTotalStyle1
                End Select

            End With
        Next
        Prop_Sets()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub rbtStock_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtStock.CheckedChanged
        If rbtStock.Checked = True Then
            Type = "STOCK"
        Else
            Type = "SALES"
        End If
    End Sub
    Public Function GetID(ByVal field As String, ByVal table As String, ByVal admindb As String, ByVal col As String, ByVal field2 As String) As String
        Dim retStr As String = ""
        retStr += objGPack.GetSqlValue(" SELECT " & field & " FROM " & admindb & ".." & table & " where " & col & " ='" & field2 & "' ")
        If retStr <> "" Then
            retStr = retStr
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Private Sub txtCaratFrom_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCaratFrom.KeyDown
       
    End Sub

    Private Sub txtCaratTo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCaratTo.KeyDown
        
    End Sub

    Private Sub txtPriceFrom_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPriceFrom.KeyDown


    End Sub

    Private Sub txtPriceTo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPriceTo.KeyDown
       
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frm4CReport_properties
        obj.p_rbtStock = rbtStock.Checked
        obj.p_rbtSales = rbtSales.Checked
        obj.p_txtCaratFrom = txtCaratFrom.Text
        obj.p_txtCaratTo = txtCaratTo.Text
        obj.p_txtPriceFrom = txtPriceFrom.Text
        obj.p_txtPriceTo = txtPriceTo.Text
        obj.p_CmbClarity = CmbClarity.Text
        obj.p_CmbColor = CmbColor.Text
        obj.p_CmbCut = CmbCut.Text
        obj.p_CmbSettype = CmbSettype.Text
        obj.p_CmbShape = CmbShape.Text
        SetSettingsObj(obj, Me.Name, GetType(frm4CReport_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frm4CReport_properties
        GetSettingsObj(obj, Me.Name, GetType(frm4CReport_properties))
        rbtStock.Checked = obj.p_rbtStock
        rbtSales.Checked = obj.p_rbtSales
        txtCaratFrom.Text = obj.p_txtCaratFrom
        txtCaratTo.Text = obj.p_txtCaratTo
        txtPriceFrom.Text = obj.p_txtPriceFrom
        txtPriceTo.Text = obj.p_txtPriceTo
        CmbClarity.Text = obj.p_CmbClarity
        CmbColor.Text = obj.p_CmbColor
        CmbCut.Text = obj.p_CmbCut
        CmbSettype.Text = obj.p_CmbSettype
        CmbShape.Text = obj.p_CmbShape

    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
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

    Private Sub cmbsubitem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbsubitem.GotFocus
        'If CmbItem.Text <> "ALL" Then
        '    cmbsubitem.Items.Clear()
        '    Dim ITEMID As Integer = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE  ITEMNAME='" & CmbItem.Text & "'")
        '    strSql = "select SUBITEMNAME AS NAME from " & cnAdminDb & "..SUBITEMMAST where  ITEMID=" & ITEMID & ""
        '    objGPack.FillCombo(strSql, cmbsubitem, False)
        '    cmbsubitem.Enabled = True
        'Else
        '    cmbsubitem.Enabled = False
        'End If
    End Sub

    Private Sub CmbItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbItem.LostFocus

        If CmbItem.Text <> "ALL" Then
            cmbsubitem.Items.Clear()
            Dim ITEMID As Integer = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE  ITEMNAME='" & CmbItem.Text & "'")
            strSql = "select SUBITEMNAME AS NAME from " & cnAdminDb & "..SUBITEMMAST where  ITEMID=" & ITEMID & ""
            objGPack.FillCombo(strSql, cmbsubitem, False)
            cmbsubitem.Enabled = True
            cmbsubitem.Focus()
        Else
            cmbsubitem.Text = "ALL"
            cmbsubitem.Enabled = False
        End If
    End Sub
End Class
Public Class frm4CReport_properties
    Private rbtStock As Boolean = True
    Public Property p_rbtStock() As Boolean
        Get
            Return rbtStock
        End Get
        Set(ByVal value As Boolean)
            rbtStock = value
        End Set
    End Property
    Private rbtSales As Boolean = True
    Public Property p_rbtSales() As Boolean
        Get
            Return rbtSales
        End Get
        Set(ByVal value As Boolean)
            rbtSales = value
        End Set
    End Property
    Private txtCaratFrom As String
    Public Property p_txtCaratFrom() As String
        Get
            Return txtCaratFrom
        End Get
        Set(ByVal value As String)
            txtCaratFrom = value
        End Set
    End Property
    Private txtCaratTo As String
    Public Property p_txtCaratTo() As String
        Get
            Return txtCaratTo
        End Get
        Set(ByVal value As String)
            txtCaratTo = value
        End Set
    End Property
    Private txtPriceFrom As String
    Public Property p_txtPriceFrom() As String
        Get
            Return txtPriceFrom
        End Get
        Set(ByVal value As String)
            txtPriceFrom = value
        End Set
    End Property
    Private txtPriceTo As String
    Public Property p_txtPriceTo() As String
        Get
            Return txtPriceTo
        End Get
        Set(ByVal value As String)
            txtPriceTo = value
        End Set
    End Property
    Private CmbCut As String
    Public Property p_CmbCut() As String
        Get
            Return CmbCut
        End Get
        Set(ByVal value As String)
            CmbCut = value
        End Set
    End Property
    Private CmbClarity As String
    Public Property p_CmbClarity() As String
        Get
            Return CmbClarity
        End Get
        Set(ByVal value As String)
            CmbClarity = value
        End Set
    End Property
    Private CmbColor As String
    Public Property p_CmbColor() As String
        Get
            Return CmbColor
        End Get
        Set(ByVal value As String)
            CmbColor = value
        End Set
    End Property
    Private CmbShape As String
    Public Property p_CmbShape() As String
        Get
            Return CmbShape
        End Get
        Set(ByVal value As String)
            CmbShape = value
        End Set
    End Property
    Private CmbSettype As String
    Public Property p_CmbSettype() As String
        Get
            Return CmbSettype
        End Get
        Set(ByVal value As String)
            CmbSettype = value
        End Set
    End Property
End Class