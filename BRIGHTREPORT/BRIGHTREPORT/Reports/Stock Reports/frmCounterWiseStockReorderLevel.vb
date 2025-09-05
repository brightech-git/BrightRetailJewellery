Imports System.Data.OleDb
Public Class frmCounterWiseStockReorderLevel
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strFilter As String = Nothing

    Dim dtCostCentre As New DataTable
    Dim dtCounter As New DataTable
    Dim dtCompany As New DataTable


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        grpControls.BringToFront()

        ''ITEM COUNTER
        StrSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        StrSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")

        ''COSTCENTRE
        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmReorderStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        chkCmbCounter.Select()
        btnNew_Click(Me, e)
    End Sub

    Private Sub NewReport()
        Dim selItemctrId As String = ""
        Dim selCostId As String = ""
        Dim rType As String = ""
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If rbtExcess.Checked Then
            rType = "E"
        ElseIf rbtShort.Checked Then
            rType = "S"
        Else
            rType = "B"
        End If

        Dim dt As New DataTable
        Dim strCostId As String = ""
        If chkCmbCostCentre.GetItemChecked(0) = False Then
            StrSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text, ",") & ")"
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                strCostId += dt.Rows(cnt).Item("COSTID").ToString
                If cnt <> dt.Rows.Count - 1 Then
                    strCostId += ","
                End If
            Next
        Else
            strCostId = "ALL"
        End If

        Dim strCompId As String = ""
        If chkCmbCompany.GetItemChecked(0) = False Then
            StrSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text, ",") & ")"
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                strCompId += dt.Rows(cnt).Item("COMPANYID").ToString
                If cnt <> dt.Rows.Count - 1 Then
                    strCompId += ","
                End If
            Next
        Else
            'strCompId = "ALL"
            StrSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                strCompId += dt.Rows(cnt).Item("COMPANYID").ToString
                If cnt <> dt.Rows.Count - 1 Then
                    strCompId += ","
                End If
            Next
        End If

        Dim strCounterId As String = ""
        If chkCmbCounter.GetItemChecked(0) = False Then
            StrSql = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkCmbCounter.Text, ",") & ")"
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            For cnt As Integer = 0 To dt.Rows.Count - 1
                strCounterId += dt.Rows(cnt).Item("ITEMCTRID").ToString
                If cnt <> dt.Rows.Count - 1 Then
                    strCounterId += ","
                End If
            Next
        Else
            strCounterId = "ALL"
        End If

        StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_ITEMCOUNTER_STKREORDER "
        StrSql += vbCrLf + "  @COSTCENTRE = '" & strCostId & "'"
        StrSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@ITEMCTRNAME = '" & strCounterId & "'"
        StrSql += vbCrLf + " ,@COMPANY = '" & strCompId & "'"
        StrSql += vbCrLf + " ,@RTYPE = '" & rType & "'"
        StrSql += vbCrLf + " ,@WITHDIFFVALUE = '" & IIf(chkWithNillDifference.Checked, "Y", "N") & "'"
        Dim DtGrid As New DataTable
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "COUNTER WISE STOCK REORDER LEVEL"
        Dim tit As String = "COUNTER WISE STOCK REORDER LEVEL REPORT" + vbCrLf
        tit += " AS ON " & dtpAsOnDate.Text & ""
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PARTICULAR").Width = 300
            .Columns("STKPCS").Width = 60
            .Columns("STKWT").Width = 80
            .Columns("REPCS").Width = 60
            .Columns("REWT").Width = 80
            .Columns("DIFFPCS").Width = 60
            .Columns("DIFFWT").Width = 80

            .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("PARTICULAR").Visible = True
            .Columns("STKPCS").Visible = True
            .Columns("STKWT").Visible = True
            .Columns("REPCS").Visible = True
            .Columns("REWT").Visible = True
            .Columns("DIFFPCS").Visible = True
            .Columns("DIFFWT").Visible = True

            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub btnView_SearchClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            NewReport()
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub frmReorderStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

 

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

  

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Prop_Sets()
        NewReport()
    End Sub

    
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        dtpAsOnDate.Value = GetServerDate()
        ' rbtBoth.Checked = True
        Prop_Gets()

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmCounterWiseStockReorderLevel_Properties
        GetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter)
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtShort = rbtShort.Checked
        obj.p_rbtExcess = rbtExcess.Checked
        obj.p_chkWithNillDifference = chkWithNillDifference.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCounterWiseStockReorderLevel_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCounterWiseStockReorderLevel_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCounterWiseStockReorderLevel_Properties))
        SetChecked_CheckedList(chkCmbCounter, obj.p_chkCmbCounter, "ALL")
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtBoth.Checked = obj.p_rbtBoth
        rbtShort.Checked = obj.p_rbtShort
        rbtExcess.Checked = obj.p_rbtExcess
        chkWithNillDifference.Checked = obj.p_chkWithNillDifference
    End Sub

    Private Sub rbtBoth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtBoth.CheckedChanged
        If rbtBoth.Checked Then
            chkWithNillDifference.Enabled = True
        Else
            chkWithNillDifference.Enabled = False
            chkWithNillDifference.Checked = True
        End If
    End Sub
End Class

Public Class frmCounterWiseStockReorderLevel_Properties
    Private chkCmbCounter As New List(Of String)
    Public Property p_chkCmbCounter() As List(Of String)
        Get
            Return chkCmbCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCounter = value
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

    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
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

    Private rbtShort As Boolean = False
    Public Property p_rbtShort() As Boolean
        Get
            Return rbtShort
        End Get
        Set(ByVal value As Boolean)
            rbtShort = value
        End Set
    End Property

    Private rbtExcess As Boolean = False
    Public Property p_rbtExcess() As Boolean
        Get
            Return rbtExcess
        End Get
        Set(ByVal value As Boolean)
            rbtExcess = value
        End Set
    End Property
    Private chkWithNillDifference As Boolean = True
    Public Property p_chkWithNillDifference() As Boolean
        Get
            Return chkWithNillDifference
        End Get
        Set(ByVal value As Boolean)
            chkWithNillDifference = value
        End Set
    End Property
End Class
