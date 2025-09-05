Imports System.Data.OleDb
Public Class frmCashBook
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim DAYBOOKDRCR As Boolean = IIf(GetAdmindbSoftValue("DAYBOOKDRCR", "Y").ToString() = "Y", True, False)

    Private Sub frmCashBook_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCashBook_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        'Load Costcentre CheckedListBox
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
        'Load Company CheckedListBox
        ChkLstCompany.Items.Clear()
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        FillCheckedListBox(strSql, ChkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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
        Else
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE"
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

        If Not ChkLstCompany.CheckedItems.Count > 0 Then
            ChkCompany.Checked = True
        End If
        Dim chkComp As String = GetChecked_CheckedList(ChkLstCompany)
        Dim companyId As String = ""
        If chkComp <> "" Then
            strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & chkComp & ")"
            Dim dtTemp1 As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp1)
            For cnt As Integer = 0 To dtTemp1.Rows.Count - 1
                companyId += "" & dtTemp1.Rows(cnt).Item("COMPANYID").ToString & ""
                If cnt <> dtTemp1.Rows.Count - 1 Then
                    companyId += ","
                End If
            Next
        Else
            strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY "
            Dim dtTemp1 As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp1)
            For cnt As Integer = 0 To dtTemp1.Rows.Count - 1
                companyId += "" & dtTemp1.Rows(cnt).Item("COMPANYID").ToString & ""
                If cnt <> dtTemp1.Rows.Count - 1 Then
                    companyId += ","
                End If
            Next
        End If

        strSql = " EXEC " & cnAdminDb & "..SP_RPT_CASHBOOK "
        strSql += vbCrLf + " @DBNAME ='" & cnStockDb & "' "
        strSql += vbCrLf + " , @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " , @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " , @OPENING = '" & IIf(chkOpening.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " , @PAGEBREAK = '" & IIf(chkPageBreak.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " , @COMPANYID = '" & companyId & "'"
        strSql += vbCrLf + " , @COSTID = '" & costId & "'"
        strSql += vbCrLf + " , @DETAILED = '" & IIf(chkDetailed.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " , @SUMMARY = '" & IIf(ChkSummary.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " , @ORDERBY = '" & IIf(rbtGeneral.Checked, "G", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        If rbtAcName.Checked Then
            strSql = " SELECT TDATE,TRANNO ,PARTICULAR"
            If chkWeightDetail.Checked = True Then
                strSql += " ,GRSWT"
            End If
            If DAYBOOKDRCR = True Then
                strSql += " ,DEBIT,CREDIT"
            Else
                strSql += " ,DEBIT CREDIT,CREDIT DEBIT"
            End If
            strSql += " ,CHQCARDNO,CONVERT(VARCHAR,CHQDATE,103) CHQDATE,REFNO,CONVERT(VARCHAR,REFDATE,103) REFDATE,COLHEAD,RESULT,ACCODE,PAGEBREAK,TRANDATE,SNO,BATCHNO,ENTRYORDER FROM TEMPTABLEDB..TEMPCASHBOOK ORDER BY TRANDATE,TDATE DESC,RESULT,PARTICULAR"
        ElseIf rbtTranNo.Checked Then
            strSql = " SELECT TDATE,TRANNO,PARTICULAR"
            If chkWeightDetail.Checked = True Then
                strSql += " ,GRSWT"
            End If
            If DAYBOOKDRCR = True Then
                strSql += " ,DEBIT,CREDIT"
            Else
                strSql += " ,DEBIT CREDIT,CREDIT DEBIT"
            End If
            strSql += " ,CHQCARDNO,CONVERT(VARCHAR,CHQDATE,103) CHQDATE,REFNO,CONVERT(VARCHAR,REFDATE,103) REFDATE,COLHEAD,RESULT,ACCODE,PAGEBREAK,TRANDATE,SNO,BATCHNO,ENTRYORDER FROM TEMPTABLEDB..TEMPCASHBOOK ORDER BY TRANDATE,TDATE DESC,RESULT,TRANNO,PARTICULAR"
        Else
            strSql = " SELECT TDATE,TRANNO,PARTICULAR"
            If chkWeightDetail.Checked = True Then
                strSql += " ,GRSWT"
            End If
            If DAYBOOKDRCR = True Then
                
                strSql += " ,DEBIT,CREDIT"
            Else
                strSql += " ,DEBIT CREDIT,CREDIT DEBIT"
            End If
            strSql += " ,CHQCARDNO,CONVERT(VARCHAR,CHQDATE,103) CHQDATE,REFNO,CONVERT(VARCHAR,REFDATE,103) REFDATE,COLHEAD,RESULT,ACCODE,PAGEBREAK,TRANDATE,SNO,BATCHNO,ENTRYORDER FROM TEMPTABLEDB..TEMPCASHBOOK ORDER BY TRANDATE,TDATE DESC,RESULT,TRANNO,CREDIT"
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
        objGridShower.Text = "CASHBOOK"
        Dim tit As String = " CASHBOOK FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCostName <> "" Then tit += " COSTCENTRE : " & Replace(chkCostName, "'", "")
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
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCASHBOOK')>0 DROP TABLE TEMPTABLEDB..TEMPCASHBOOK "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
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

            For CNT As Integer = 6 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next

            .Columns("CHQCARDNO").Visible = chkChequeNo.Checked
            .Columns("CHQDATE").Visible = chkChequeDate.Checked
            .Columns("REFNO").Visible = chkRefNo.Checked
            .Columns("REFDATE").Visible = chkRefDate.Checked

            .Columns("CHQCARDNO").Width = 80
            .Columns("CHQDATE").Width = 80
            .Columns("REFNO").Width = 80
            .Columns("REFDATE").Width = 80

            .Columns("CHQCARDNO").Visible = IIf(chkMore.Checked = True, chkChequeNo.Checked, chkMore.Checked)
            .Columns("CHQDATE").Visible = IIf(chkMore.Checked = True, chkChequeDate.Checked, chkMore.Checked)
            .Columns("REFNO").Visible = IIf(chkMore.Checked = True, chkRefNo.Checked, chkMore.Checked)
            .Columns("REFDATE").Visible = IIf(chkMore.Checked = True, chkRefDate.Checked, chkMore.Checked)
        End With
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCashBook_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkOpening = chkOpening.Checked
        obj.p_chkPageBreak = chkPageBreak.Checked
        obj.p_chkDetailed = chkDetailed.Checked
        obj.p_rbtAcName = rbtAcName.Checked
        obj.p_rbtTranNo = rbtTranNo.Checked
        obj.p_rbtGeneral = rbtGeneral.Checked
        obj.p_chkMore = chkMore.Checked
        obj.p_chkChequeNo = chkChequeNo.Checked
        obj.p_chkChequeDate = chkChequeDate.Checked
        obj.p_chkRefNo = chkRefNo.Checked
        obj.p_chkRefDate = chkRefDate.Checked
        obj.p_chkSummary = ChkSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCashBook_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCashBook_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCashBook_Properties))
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkOpening.Checked = obj.p_chkOpening
        chkPageBreak.Checked = obj.p_chkPageBreak
        chkDetailed.Checked = obj.p_chkDetailed
        rbtAcName.Checked = obj.p_rbtAcName
        rbtTranNo.Checked = obj.p_rbtTranNo
        rbtGeneral.Checked = obj.p_rbtGeneral
        chkMore.Checked = obj.p_chkMore
        chkChequeNo.Checked = obj.p_chkChequeNo
        chkChequeDate.Checked = obj.p_chkChequeDate
        chkRefNo.Checked = obj.p_chkRefNo
        chkRefDate.Checked = obj.p_chkRefDate
        ChkSummary.Checked = obj.p_chkSummary
        chkMore_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub chkMore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMore.CheckedChanged
        grpMore.Enabled = chkMore.Checked
        If chkMore.Checked = True Then ChkSummary.Checked = False
    End Sub

    Private Sub ChkSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSummary.CheckedChanged
        If ChkSummary.Enabled = True Then
            chkMore.Checked = False
            chkDetailed.Checked = False
        End If
    End Sub

    Private Sub chkDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDetailed.CheckedChanged
        If chkDetailed.Checked = True Then ChkSummary.Checked = False
    End Sub
End Class

Public Class frmCashBook_Properties
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

    Private chkSummary As Boolean = False
    Public Property p_chkSummary() As Boolean
        Get
            Return chkSummary
        End Get
        Set(ByVal value As Boolean)
            chkSummary = value
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
    Private rbtGeneral As Boolean = False
    Public Property p_rbtGeneral() As Boolean
        Get
            Return rbtGeneral
        End Get
        Set(ByVal value As Boolean)
            rbtGeneral = value
        End Set
    End Property
End Class