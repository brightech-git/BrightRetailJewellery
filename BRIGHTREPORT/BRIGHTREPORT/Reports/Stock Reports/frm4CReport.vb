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

    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frmTagedItemsStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
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
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, DataGridView1, GiriPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, GiriPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub btnView_Search_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridView.DataSource = Nothing
        tabView.Show()
        Dim dtSource2 As New DataTable
        Dim ds As DataSet = New DataSet
        strSql = " EXEC " & cnAdminDb & "..RPT_PROC_FOURCSTOCKDETAILS"
        strSql += vbCrLf + " @COSTID = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @ITEMID = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @SHAPE = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @COLOR = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @CUT = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @CLARITY = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @CARATFROM = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @CARATTO = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @PRICEFROM = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @PRICETO = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @RPTTYPE = '" & CmbItem.SelectedItem.ToString & "'"
        strSql += vbCrLf + " @SYSTEMID = '" & CmbItem.SelectedItem.ToString & "'"
        dtSource2.Columns.Add("KEYNO", GetType(Integer))
        dtSource2.Columns("KEYNO").AutoIncrement = True
        dtSource2.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource2.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource2)
        gridView.DataSource = dtSource2

        With gridView
            With .Columns("ITEMID")
                .HeaderText = "ITEMID"
                .Width = 50
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("TAGNO")
                .HeaderText = "TAGNO"
                .Width = 100
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("GRSWT")
                .HeaderText = "GRSWT"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("SALVALUE")
                .HeaderText = "SALVALUE"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("CUTID")
                .HeaderText = "CUTID"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("COLORID")
                .HeaderText = "COLORID"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("CLARITYID")
                .HeaderText = "CLARITYID"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("SHAPEID")
                .HeaderText = "SHAPEID"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("SETTYPEID")
                .HeaderText = "SETTYPEID"
                .Width = 150
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("RESULT")
                .HeaderText = "RESULT"
                .Width = 150
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("COLHEAD")

                .HeaderText = "COLHEAD"
                .Width = 150
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub
End Class