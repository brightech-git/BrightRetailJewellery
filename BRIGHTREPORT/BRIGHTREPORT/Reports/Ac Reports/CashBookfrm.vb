Imports System.Data.OleDb
Public Class CashBookfrm
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia

    Private Sub CashBookfrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CashBookfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'rbtAcName.Checked = True
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim costId As String = ""
        If chkCostName <> "" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & ")"
            Dim dtTemp As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                costId += "" & dtTemp.Rows(cnt).Item("COSTID").ToString & ""
                If cnt <> dtTemp.Rows.Count - 1 Then
                    costId += ","
                End If
            Next
        End If

        strSql = " EXEC " & cnStockDb & "..SP_CASHBOOK @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " , @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " , @OPENING = '" & IIf(chkOpening.Checked, "Y", "N") & "'"
        strSql += " , @PAGEBREAK = '" & IIf(chkPageBreak.Checked, "Y", "N") & "'"
        strSql += " , @COMPANYID = '" & strCompanyId & "'"
        strSql += " , @COSTID = '" & costId & "'"
        strSql += " , @DETAILED = '" & IIf(chkDetailed.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If rbtAcName.Checked Then
            strSql = " SELECT * FROM MASTER..TEMPDAYBOOK ORDER BY TRANDATE,RESULT,PARTICULAR"
        Else
            strSql = " SELECT * FROM MASTER..TEMPDAYBOOK ORDER BY TRANDATE,RESULT,TRANNO,PARTICULAR"
        End If
        Prop_Sets()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DAYBOOK"
        Dim tit As String = " DAYBOOK FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("TDATE")))
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)
            .Columns("TDATE").Width = 90
            .Columns("TRANNO").Width = 80
            .Columns("PARTICULAR").Width = 350
            .Columns("DEBIT").Width = 150
            .Columns("CREDIT").Width = 150

            .Columns("TDATE").HeaderText = "TRANDATE"

            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For CNT As Integer = 5 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
        End With
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.Leave
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New CashBookfrm_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkOpening = chkOpening.Checked
        obj.p_chkPageBreak = chkPageBreak.Checked
        obj.p_chkDetailed = chkDetailed.Checked
        obj.p_rbtAcName = rbtAcName.Checked
        obj.p_rbtTranNo = rbtTranNo.Checked
        SetSettingsObj(obj, Me.Name, GetType(CashBookfrm_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New CashBookfrm_Properties
        GetSettingsObj(obj, Me.Name, GetType(CashBookfrm_Properties))
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkOpening.Checked = obj.p_chkOpening
        chkPageBreak.Checked = obj.p_chkPageBreak
        chkDetailed.Checked = obj.p_chkDetailed
        rbtAcName.Checked = obj.p_rbtAcName
        rbtTranNo.Checked = obj.p_rbtTranNo
    End Sub
End Class

Public Class CashBookfrm_Properties
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
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
    Private chkOpening As Boolean = False
    Public Property p_chkOpening() As Boolean
        Get
            Return chkOpening
        End Get
        Set(ByVal value As Boolean)
            chkOpening = value
        End Set
    End Property
    Private chkPageBreak As Boolean = False
    Public Property p_chkPageBreak() As Boolean
        Get
            Return chkPageBreak
        End Get
        Set(ByVal value As Boolean)
            chkPageBreak = value
        End Set
    End Property
    Private chkDetailed As Boolean = False
    Public Property p_chkDetailed() As Boolean
        Get
            Return chkDetailed
        End Get
        Set(ByVal value As Boolean)
            chkDetailed = value
        End Set
    End Property
    Private rbtAcName As Boolean = True
    Public Property p_rbtAcName() As Boolean
        Get
            Return rbtAcName
        End Get
        Set(ByVal value As Boolean)
            rbtAcName = value
        End Set
    End Property
    Private rbtTranNo As Boolean = False
    Public Property p_rbtTranNo() As Boolean
        Get
            Return rbtTranNo
        End Get
        Set(ByVal value As Boolean)
            rbtTranNo = value
        End Set
    End Property
End Class