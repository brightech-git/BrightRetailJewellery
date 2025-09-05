Imports System.Data.OleDb
Public Class DayBookWithJournalfrm
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim DAYBOOKDRCR As Boolean = IIf(GetAdmindbSoftValue("DAYBOOKDRCR", "Y").ToString() = "Y", True, False)

    Private Sub DayBookWithJournalfrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DayBookWithJournalfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
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
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        chkLstUserWise.Enabled = True
        chkLstUserWise.Items.Clear()
        strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER ORDER BY USERNAME"
        Dim dtUsr As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtUsr)
        For i As Integer = 0 To dtUsr.Rows.Count - 1
            chkLstUserWise.Items.Add(dtUsr.Rows(i).Item("USERNAME").ToString, False)
        Next
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

        If Not chkLstUserWise.CheckedItems.Count > 0 Then
            chkUserWiseSelectAll.Checked = True
        End If

        Dim chkUserName As String
        Dim userIds As String = ""
        If chkUserWiseSelectAll.Checked = True Then
            chkUserName = "ALL"
            userIds = "ALL"
        Else
            chkUserName = GetChecked_CheckedList(chkLstUserWise)

            If chkUserName <> "" Then
                strSql = " SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME IN (" & chkUserName & ")"
                Dim dtUserTemp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtUserTemp)
                For cnt As Integer = 0 To dtUserTemp.Rows.Count - 1
                    userIds += "" & dtUserTemp.Rows(cnt).Item("USERID").ToString & ""
                    If cnt <> dtUserTemp.Rows.Count - 1 Then
                        userIds += ","
                    End If
                Next
            End If
        End If

        strSql = " EXEC " & cnAdminDb & "..RPT_DAYBOOKWITHJOURNAL "
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@TEMPTABLE='TEMP" & systemId & "DAYBOOK'"
        strSql += vbCrLf + " , @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " , @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " , @OPENING = '" & IIf(chkOpening.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " , @COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " , @COSTID = '" & costId & "'"
        strSql += vbCrLf + " , @DETAILED = 'N'"
        strSql += vbCrLf + " , @USERID = '" & userIds & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        dt = dss.Tables(0)
        'cmd.ExecuteNonQuery()
        If Me.dtpFrom.Value = dtpTo.Value Then dt.Columns.Remove("TRANDATE")
        Dim dtgrid As New DataTable
        dtgrid = dt.Copy

        dtgrid.Columns.Add("KEYNO", GetType(Integer))
        dtgrid.Columns("KEYNO").AutoIncrement = True
        dtgrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtgrid.Columns("KEYNO").AutoIncrementStep = 1

        If Not dtgrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim mjdr As Decimal = 0
        Dim mjcr As Decimal = 0, mdr As Decimal = 0, mcr As Decimal = 0
        mjdr = dtgrid.Compute("SUM(jdebit)", Nothing)
        mjcr = dtgrid.Compute("SUM(jcredit)", Nothing)
        mdr = dtgrid.Compute("SUM(debit)", Nothing)
        mcr = dtgrid.Compute("SUM(credit)", Nothing)
        Dim dr As DataRow = dtgrid.NewRow
        dr!PAYMODE = "Z1"
        dr!PARTICULARS = "TOTAL "

        dr!jDEBIT = mjdr
        dr!jCREDIT = mjcr
        dr!DEBIT = mdr
        dr!CREDIT = mcr
        dtgrid.Rows.Add(dr)

        dr = dtgrid.NewRow
        dr!PAYMODE = ""
        dr!PARTICULARS = "CLOSING BALANCE"
        dr!colhead = "S1"
        'dr!jDEBIT = IIf(mjdr > mjcr, mjdr - mjcr, 0)
        'dr!jCREDIT = IIf(mjcr > mjdr, mjcr - mjdr, 0)
        dr!DEBIT = IIf(mdr > mcr, mdr - mcr, 0)
        dr!CREDIT = IIf(mcr > mdr, mcr - mdr, 0)
        dtgrid.Rows.Add(dr)
        dtgrid.Columns("KEYNO").SetOrdinal(dtgrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia(True)
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DAYBOOK"
        Dim tit As String = " DAYBOOK " & IIf(dtpFrom.Value = dtpTo.Value, "FOR " + dtpFrom.Text, "FROM " + dtpFrom.Text + " TO " + dtpTo.Text)
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtgrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridView.Columns("PAYMODE").Visible = False
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        If dt.Columns.Contains("DATE") Then
            objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("DATE")))
        End If
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)
            If .Columns.Contains("DATE") Then .Columns("DATE").Width = 90
            .Columns("TRANNO").Width = 80
            .Columns("PAYHEAD").Width = 150
            .Columns("PARTICULARS").Width = 250
            .Columns("PARTICULARS").Width = 200
            .Columns("DEBIT").Width = 100
            .Columns("CREDIT").Width = 100
            .Columns("JDEBIT").Width = 90
            .Columns("JCREDIT").Width = 90

            .Columns("JDEBIT").HeaderText = "DEBIT"
            .Columns("JCREDIT").HeaderText = "CREDIT"
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("ORDERNO") Then .Columns("ORDERNO").Visible = False
            If .Columns.Contains("DATE") Then .Columns("DATE").HeaderText = "TRANDATE"

            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            
            '.Columns("DATE").Visible = False
            '.Columns("PAYMODE").Visible = False

            For CNT As Integer = 10 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            Exit Sub
            '.Columns("CHQCARDNO").Visible = chkChequeNo.Checked
            '.Columns("CHQDATE").Visible = chkChequeDate.Checked
            '.Columns("REFNO").Visible = chkRefNo.Checked
            '.Columns("REFDATE").Visible = chkRefDate.Checked

            '.Columns("CHQCARDNO").Width = 80
            '.Columns("CHQDATE").Width = 80
            '.Columns("REFNO").Width = 80
            '.Columns("REFDATE").Width = 80

            '.Columns("CHQCARDNO").Visible = IIf(chkMore.Checked = True, chkChequeNo.Checked, chkMore.Checked)
            '.Columns("CHQDATE").Visible = IIf(chkMore.Checked = True, chkChequeDate.Checked, chkMore.Checked)
            '.Columns("REFNO").Visible = IIf(chkMore.Checked = True, chkRefNo.Checked, chkMore.Checked)
            '.Columns("REFDATE").Visible = IIf(chkMore.Checked = True, chkRefDate.Checked, chkMore.Checked)
        End With
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.Leave
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New DayBookWithJournalfrm_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkOpening = chkOpening.Checked
        obj.p_chkPageBreak = chkPageBreak.Checked
        obj.p_chkDetailed = chkDetailed.Checked
        obj.p_rbtAcName = rbtAcName.Checked
        obj.p_rbtTranNo = rbtTranNo.Checked
        obj.p_chkMore = chkMore.Checked
        obj.p_chkChequeNo = chkChequeNo.Checked
        obj.p_chkChequeDate = chkChequeDate.Checked
        obj.p_chkRefNo = chkRefNo.Checked
        obj.p_chkRefDate = chkRefDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(DayBookWithJournalfrm_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New DayBookWithJournalfrm_Properties
        GetSettingsObj(obj, Me.Name, GetType(DayBookWithJournalfrm_Properties))
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkOpening.Checked = obj.p_chkOpening
        chkPageBreak.Checked = obj.p_chkPageBreak
        chkDetailed.Checked = obj.p_chkDetailed
        rbtAcName.Checked = obj.p_rbtAcName
        rbtTranNo.Checked = obj.p_rbtTranNo
        chkMore.Checked = obj.p_chkMore
        chkChequeNo.Checked = obj.p_chkChequeNo
        chkChequeDate.Checked = obj.p_chkChequeDate
        chkRefNo.Checked = obj.p_chkRefNo
        chkRefDate.Checked = obj.p_chkRefDate
        chkMore_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub chkMore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMore.CheckedChanged
        grpMore.Enabled = chkMore.Checked
    End Sub

    Private Sub chkUserWiseSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUserWiseSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstUserWise, chkUserWiseSelectAll.Checked)
    End Sub
End Class

Public Class DayBookWithJournalfrm_Properties
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

    Private chkMore As Boolean = False
    Public Property p_chkMore() As Boolean
        Get
            Return chkMore
        End Get
        Set(ByVal value As Boolean)
            chkMore = value
        End Set
    End Property

    Private chkChequeNo As Boolean = False
    Public Property p_chkChequeNo() As Boolean
        Get
            Return chkChequeNo
        End Get
        Set(ByVal value As Boolean)
            chkChequeNo = value
        End Set
    End Property

    Private chkChequeDate As Boolean = False
    Public Property p_chkChequeDate() As Boolean
        Get
            Return chkChequeDate
        End Get
        Set(ByVal value As Boolean)
            chkChequeDate = value
        End Set
    End Property

    Private chkRefNo As Boolean = False
    Public Property p_chkRefNo() As Boolean
        Get
            Return chkRefNo
        End Get
        Set(ByVal value As Boolean)
            chkRefNo = value
        End Set
    End Property

    Private chkRefDate As Boolean = False
    Public Property p_chkRefDate() As Boolean
        Get
            Return chkRefDate
        End Get
        Set(ByVal value As Boolean)
            chkRefDate = value
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