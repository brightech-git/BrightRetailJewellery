Imports System.Data.OleDb
Public Class ItemWiseSales_Review
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable


    Private Sub PacketWiseStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub PacketWiseStockView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "Tran Date"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        ' chkAsOnDate.Checked = True
        Prop_Gets()
        chkAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub



    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")

        strSql = " EXEC " & cnStockDb & "..SP_RPT_ITEMWISESALES_REVIEW"
        strSql += " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " ,@COMPANYID = '" & chkCompanyId & "'"
        strSql += " ,@COSTID = '" & chkCostId & "'"
        strSql += " ,@DBNAME = '" & cnAdminDb & "'"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ITEM WISE SALES REVIEW"
        Dim tit As String = "ITEM WISE SALES REVIEW REPORT" + vbCrLf
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit = tit + IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
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
        Prop_Sets()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PURITY").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("SNO").Visible = False
            .Columns("SLNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("MARGIN_PER").HeaderText = "MARGIN %"
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New ItemWiseSales_Review_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(ItemWiseSales_Review_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New ItemWiseSales_Review_Properties
        GetSettingsObj(obj, Me.Name, GetType(ItemWiseSales_Review_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub
End Class

Public Class ItemWiseSales_Review_Properties
    Private chkAsOnDate As Boolean = False
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
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
End Class