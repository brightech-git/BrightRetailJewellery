Imports System.Data.OleDb
Public Class frmReceiptPaymentDetailss
    Dim objGridShower As frmGridDispDia
    Dim objMiscRemark As frmMiscRemark
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        objGridShower = New frmGridDispDia
        objMiscRemark = New frmMiscRemark
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub CashAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub
    Private Sub AddRow(ByRef dt As DataTable, ByVal valu As String)
        Dim ro As DataRow = Nothing
        ro = dt.NewRow
        ro!title = valu
        dt.Rows.Add(ro)
    End Sub
    Private Sub funcLoadPaymodeName()
        Dim dt As New Data.DataTable
        dt.Clear()
        ChkLstPaymodetype.Enabled = True
        ChkLstPaymodetype.Items.Clear()
        dt.Columns.Add("TITLE", GetType(String))
        AddRow(dt, "CREDIT SALES")
        AddRow(dt, "OTHER RECEIPTS")
        AddRow(dt, "FURTHER ADVANCE")
        AddRow(dt, "ORDER ADVANCE")
        AddRow(dt, "REPAIR ADVANCE")
        AddRow(dt, "CUSTOMER ADVANCE")
        AddRow(dt, "PURCHASE\SALES RETURN")
        AddRow(dt, "OTHER PAYMENT")
        AddRow(dt, "ORDER REPAY")
        AddRow(dt, "ADVANCE REPAY")
        AddRow(dt, "ORDER DELIVERY")
        AddRow(dt, "REPAIR DELIVERY")
        AddRow(dt, "CREDIT ADJ")
        AddRow(dt, "GIFT VOUCHER PAYMENT")
        AddRow(dt, "GIFT VOUCHER RECEIPT")

        'strSql = " Select DISTINCT "
        'strSql += " CASE WHEN RECPAY = 'R' THEN "
        'strSql += " CASE WHEN TRANTYPE = 'D' THEN 'CREDIT SALES'"
        'strSql += " WHEN TRANTYPE = 'T' THEN 'OTHER RECEIPTS' "
        'strSql += " WHEN TRANTYPE = 'A' AND SUBSTRING(RUNNO,6,1) = 'O' AND ISNULL(FLAG,'') = 'F' THEN 'FURTHER ADVANCE' "
        'strSql += " WHEN TRANTYPE = 'A' AND SUBSTRING(RUNNO,6,1) = 'O' AND ISNULL(FLAG,'') = '' THEN 'ORDER ADVANCE' "
        'strSql += " WHEN TRANTYPE = 'A' THEN 'CUSTOMER ADVANCE' ELSE TRANTYPE END "
        'strSql += " WHEN RECPAY = 'P' THEN "
        'strSql += " CASE WHEN TRANTYPE = 'D' THEN 'PURCHASE\SALES RETURN' "
        'strSql += " WHEN TRANTYPE = 'T' THEN 'OTHER PAYMENT' "
        'strSql += " WHEN TRANTYPE = 'A' AND SUBSTRING(RUNNO,6,1) = 'O' AND ISNULL(FLAG,'') = 'F' THEN 'ORDER REPAY' "
        'strSql += " WHEN TRANTYPE = 'A' THEN 'ADVANCE REPAY' ELSE TRANTYPE END "
        'strSql += " ELSE RECPAY END AS TITLE FROM " & cnAdminDb & "..OUTSTANDING "
        'da = New OleDb.OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)

        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                ChkLstPaymodetype.Items.Add(dt.Rows(cnt).Item("TITLE").ToString)
            Next
        End If
        'For CNT As Integer = 0 To ChkLstPaymodetype.Items.Count - 1
        '    ChkLstPaymodetype.SetItemChecked(CNT, chkAllGroup.Checked)
        'Next

    End Sub

    Private Sub CashAbstract_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcLoadPaymodeName()
        LoadCompany(chkLstCompany)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtCostCentre As New DataTable
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                'chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, "ALL", False))
                For i As Integer = 0 To dtCostCentre.Rows.Count - 1
                    If cnCostName = dtCostCentre.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            Else
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim NodeId As String = ""

        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If

        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)

        If ChkLstPaymodetype.Items.Count > 0 Then
            If Not ChkLstPaymodetype.CheckedItems.Count > 0 Then
                chkPayModeSelectAll.Checked = True
            End If
        End If
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)

        Dim chkPayModeName As String = GetChecked_CheckedList(ChkLstPaymodetype, False)

        strSql = " EXEC " & cnStockDb & "..SP_RPT_REPPAYSUMMARY"
        strSql += vbCrLf + " @DateFrom = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DateTo = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TYPEID = '" & chkPayModeName & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
        strSql += vbCrLf + " ,@NodeId = '" & txtSystemId.Text & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT RUNNO AS PARTICULAR,TRANNO,TRANDATE,CONVERT(NUMERIC(15,2),AMOUNT)AMOUNT,CONVERT(NUMERIC(15,3),WEIGHT)WEIGHT,"
        strSql += " CONVERT(NUMERIC(15,2),CASH)CASH,CONVERT(NUMERIC(15,2),CCARD)CCARD,CONVERT(NUMERIC(15,2),CHEQUE)CHEQUE,CONVERT(NUMERIC(15,2),PURCHASE)PURCHASE,"
        strSql += " CONVERT(NUMERIC(15,2),SRETURN)SRETURN,CONVERT(NUMERIC(15,2),ADVADJ)ADVADJ,CONVERT(NUMERIC(15,2),SCHEME)SCHEME,CONVERT(NUMERIC(15,2),DISCOUNT)DISCOUNT,"
        strSql += " CUSTNAME,MOBILE,DOORNO,(ADDRESS1+ADDRESS2+ADDRESS3) AS ADDRESS,AREA,CITY,STATE,COUNTRY,EMPNAME,REMARK"
        strSql += " ,COLHEAD,BATCHNO FROM TEMPTABLEDB..TEMP" & systemId & "FINAL "
        strSql += " ORDER BY TITLE,RESULT,TRANDATE,TRANNO"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
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
        objGridShower.Text = "PAYMENT AND RECEIPT DETAILS"
        Dim tit As String = "PAYMENT AND RECEIPT DETAILS FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        objGridShower.Show()
        If objGridShower.gridView.Columns.Contains("REMARK") Then objGridShower.gridView.Columns("REMARK").Visible = True
        If objGridShower.gridView.Columns.Contains("BATCHNO") Then objGridShower.gridView.Columns("BATCHNO").Visible = False
        If objGridShower.gridView.Columns.Contains("COLHEAD") Then objGridShower.gridView.Columns("COLHEAD").Visible = False
        If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
        If objGridShower.gridView.Columns.Contains("AMOUNT") Then objGridShower.gridView.Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("WEIGHT") Then objGridShower.gridView.Columns("WEIGHT").DefaultCellStyle.Format = "0.000"
        If objGridShower.gridView.Columns.Contains("DISCOUNT") Then objGridShower.gridView.Columns("DISCOUNT").DefaultCellStyle.Format = "0.00"

        If objGridShower.gridView.Columns.Contains("CASH") Then objGridShower.gridView.Columns("CASH").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("CCARD") Then objGridShower.gridView.Columns("CCARD").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("CHEQUE") Then objGridShower.gridView.Columns("CHEQUE").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("PURCHASE") Then objGridShower.gridView.Columns("PURCHASE").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("SRETURN") Then objGridShower.gridView.Columns("SRETURN").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("ADVADJ") Then objGridShower.gridView.Columns("ADVADJ").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("SCHEME") Then objGridShower.gridView.Columns("SCHEME").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("SCHEME") Then objGridShower.gridView.Columns("SCHEME").HeaderText = "CHIT CARD"
        If Not chkWithPaymode.Checked Then
            If objGridShower.gridView.Columns.Contains("CASH") Then objGridShower.gridView.Columns("CASH").Visible = False
            If objGridShower.gridView.Columns.Contains("CCARD") Then objGridShower.gridView.Columns("CCARD").Visible = False
            If objGridShower.gridView.Columns.Contains("CHEQUE") Then objGridShower.gridView.Columns("CHEQUE").Visible = False
            If objGridShower.gridView.Columns.Contains("PURCHASE") Then objGridShower.gridView.Columns("PURCHASE").Visible = False
            If objGridShower.gridView.Columns.Contains("SRETURN") Then objGridShower.gridView.Columns("SRETURN").Visible = False
            If objGridShower.gridView.Columns.Contains("ADVADJ") Then objGridShower.gridView.Columns("ADVADJ").Visible = False
            If objGridShower.gridView.Columns.Contains("SCHEME") Then objGridShower.gridView.Columns("SCHEME").Visible = False
        End If

        AddHandler objGridShower.gridView.KeyPress, AddressOf gridkeypress_KeyPress
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Prop_Sets()
    End Sub

    Private Sub gridkeypress_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "R" Then
            If objGridShower.gridView.Item("BatchNo", objGridShower.gridView.CurrentRow.Index).Value.ToString <> "" Then
                Dim Defremark As String = objGridShower.gridView.Item("REMARK", objGridShower.gridView.CurrentRow.Index).Value.ToString
                If Defremark.ToString <> "" Then objMiscRemark.cmbRemark1_OWN.Text = Defremark
                objMiscRemark.BackColor = Me.BackColor
                objMiscRemark.StartPosition = FormStartPosition.CenterScreen
                If objMiscRemark.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    If objMiscRemark.cmbRemark1_OWN.Text.ToString <> "" Then
                        strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET "
                        strSql += " REMARK1='" & objMiscRemark.cmbRemark1_OWN.Text & "' "
                        strSql += " ,REMARK2='' WHERE "
                        strSql += " BatchNo='" & objGridShower.gridView.Item("BatchNo", objGridShower.gridView.CurrentRow.Index).Value.ToString & "'"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                        objGridShower.gridView.Item("REMARK", objGridShower.gridView.CurrentRow.Index).Value = objMiscRemark.cmbRemark1_OWN.Text
                        objMiscRemark.cmbRemark1_OWN.Text = ""
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PARTICULAR").Width = 160
            .Columns("TRANNO").Width = 63
            .Columns("TRANDATE").Width = 75
            .Columns("AMOUNT").Width = 100
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("WEIGHT").Width = 80
            If chkDis.Checked = True Then
                .Columns("DISCOUNT").Visible = True
                .Columns("DISCOUNT").Width = 80
                .Columns("DISCOUNT").DefaultCellStyle.Format = "0.00"
            Else
                .Columns("DISCOUNT").Visible = False
            End If
            .Columns("CUSTNAME").Width = 150
            .Columns("ADDRESS").Width = 150
            .Columns("MOBILE").Width = 100
            .Columns("EMPNAME").Width = 125
            .Columns("REMARK").Width = 150
            ''For cnt As Integer = 15 To dgv.ColumnCount - 1
            ''    .Columns(cnt).Visible = False
            ''Next

            FormatGridColumns(dgv, False, , , False)
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkPayModeSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPayModeSelectAll.CheckedChanged
        SetChecked_CheckedList(ChkLstPaymodetype, chkPayModeSelectAll.Checked)
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.Leave
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkLstPaymodetype_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkLstPaymodetype.Leave
        If ChkLstPaymodetype.Items.Count > 0 Then
            If Not ChkLstPaymodetype.CheckedItems.Count > 0 Then
                chkPayModeSelectAll.Checked = True
            End If
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub


    Private Sub Prop_Gets()
        Dim obj As New frmReceiptPaymentDetailss_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmReceiptPaymentDetailss_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkPayModeSelectAll.Checked = obj.p_chkPayModeSelectAll
        SetChecked_CheckedList(ChkLstPaymodetype, obj.p_ChkLstPaymodetype, Nothing)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        txtSystemId.Text = obj.p_txtSystemId
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmReceiptPaymentDetailss_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkPayModeSelectAll = chkPayModeSelectAll.Checked
        GetChecked_CheckedList(ChkLstPaymodetype, obj.p_ChkLstPaymodetype)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_txtSystemId = txtSystemId.Text
        SetSettingsObj(obj, Me.Name, GetType(frmReceiptPaymentDetailss_Properties))
    End Sub

    'Private Sub chkDis_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDis.CheckedChanged
    '    If chkDis.Checked = True Then
    '        dis = 1
    '    Else
    '        dis = 0
    '    End If
    'End Sub
End Class


Public Class frmReceiptPaymentDetailss_Properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkPayModeSelectAll As Boolean = False
    Public Property p_chkPayModeSelectAll() As Boolean
        Get
            Return chkPayModeSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkPayModeSelectAll = value
        End Set
    End Property
    Private ChkLstPaymodetype As New List(Of String)
    Public Property p_ChkLstPaymodetype() As List(Of String)
        Get
            Return ChkLstPaymodetype
        End Get
        Set(ByVal value As List(Of String))
            ChkLstPaymodetype = value
        End Set
    End Property
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
    Private txtSystemId As String = ""
    Public Property p_txtSystemId() As String
        Get
            Return txtSystemId
        End Get
        Set(ByVal value As String)
            txtSystemId = value
        End Set
    End Property
End Class