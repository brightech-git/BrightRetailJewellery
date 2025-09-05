Imports System.Data.OleDb
Imports System.Runtime.Serialization.Formatters.Soap
Public Class AdvanceClosedAging
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        'chkCompanySelectAll.Checked = False
        'chkCostCentreSelectAll.Checked = False
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

    Private Sub CashAbstract_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        LoadCompany(chkLstCompany)

        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If
        Dim lctot As Integer = 0
        Dim schchk As Integer = 365
        cmd = New OleDbCommand("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdvanceClosedAginginput]') AND type in (N'U')) DROP TABLE [dbo].[AdvanceClosedAginginput]", cn)
        cmd.ExecuteNonQuery()

        cmd = New OleDbCommand(" CREATE TABLE AdvanceClosedAginginput (Fromvalue numeric,Tovalue numeric)", cn)
        cmd.ExecuteNonQuery()

        lctot = IIf(Val(txtr1ta.Text) < 1, schchk, Val(txtr1ta.Text))
        If Val(txtr1fa.Text) > 0 Then
            cmd = New OleDbCommand("insert into AdvanceClosedAginginput (Fromvalue ,Tovalue) values (" & Val(txtr1fa.Text) & "," & lctot & ")", cn)
            cmd.ExecuteNonQuery()
        End If

        lctot = IIf(Val(txtr2ta.Text) < 1, schchk, Val(txtr2ta.Text))
        If Val(txtr2fa.Text) > 0 Then
            cmd = New OleDbCommand("insert into AdvanceClosedAginginput (Fromvalue ,Tovalue) values (" & Val(txtr2fa.Text) & "," & lctot & ")", cn)
            cmd.ExecuteNonQuery()
        End If

        lctot = IIf(Val(txtr3ta.Text) < 1, schchk, Val(txtr3ta.Text))
        If Val(txtr3fa.Text) > 0 Then
            cmd = New OleDbCommand("insert into AdvanceClosedAginginput (Fromvalue ,Tovalue) values (" & Val(txtr3fa.Text) & "," & lctot & ")", cn)
            cmd.ExecuteNonQuery()
        End If

        lctot = IIf(Val(txtr4ta.Text) < 1, schchk, Val(txtr4ta.Text))
        If Val(txtr4fa.Text) > 0 Then
            cmd = New OleDbCommand("insert into AdvanceClosedAginginput (Fromvalue ,Tovalue) values (" & Val(txtr4fa.Text) & "," & lctot & ")", cn)
            cmd.ExecuteNonQuery()
        End If

        lctot = IIf(Val(txtr5ta.Text) < 1, schchk, Val(txtr5ta.Text))
        If Val(txtr5fa.Text) > 0 Then
            cmd = New OleDbCommand("insert into AdvanceClosedAginginput (Fromvalue ,Tovalue) values (" & Val(txtr5fa.Text) & "," & lctot & ")", cn)
            cmd.ExecuteNonQuery()
        End If


        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)
        strSql = " EXEC " & cnStockDb & "..SP_ADVANCECLOSEDAGING"
        strSql += vbCrLf + "  @COMPANYID = '" & IIf(SelectedCompanyId = "", "ALL", SelectedCompanyId) & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & IIf(chkCostName = "", "ALL", chkCostName) & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        dtGrid.Columns.Add("SNO", GetType(String))
        dtGrid.Columns("SNO").AutoIncrement = True
        dtGrid.Columns("SNO").AutoIncrementSeed = 1
        dtGrid.Columns("SNO").AutoIncrementStep = 1

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("SNO").SetOrdinal(0)
        If Not dtGrid.Rows.Count > 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ADVANCE CLOSED AGING"
        Dim tit As String = "ADVANCE CLOSED AGING FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        Prop_Sets()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            '            .Columns("PARTICULAR").Width = 300
            '           .Columns("PCS").Width = 70
            '          .Columns("GRSWT").Width = 80
            '         .Columns("NETWT").Width = 80
            '        .Columns("AMOUNT").Width = 100
            '       .Columns("SALRATE").Width = 100
            '      .Columns("DIFFRATE").Width = 100
            '     For cnt As Integer = 7 To dgv.ColumnCount - 1
            '.Columns(cnt).Visible = False
            'Next
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub txtr2fa_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtr2fa.GotFocus
        txtr2fa.Text = IIf(Val(txtr1ta.Text) > 0, Val(txtr1ta.Text) + 1, "")
    End Sub

    Private Sub txtr2fa_TextChangDed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtr2fa.TextChanged

    End Sub

    Private Sub txtr3fa_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtr3fa.GotFocus
        txtr3fa.Text = IIf(Val(txtr2ta.Text) > 0, Val(txtr2ta.Text) + 1, "")
    End Sub

    Private Sub txtr3fa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtr3fa.TextChanged

    End Sub

    Private Sub txtr4fa_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtr4fa.GotFocus
        txtr4fa.Text = IIf(Val(txtr3ta.Text) > 0, Val(txtr3ta.Text) + 1, "")
    End Sub

    Private Sub txtr4fa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtr4fa.TextChanged

    End Sub

    Private Sub txtr5fa_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtr5fa.GotFocus
        txtr5fa.Text = IIf(Val(txtr4ta.Text) > 0, Val(txtr4ta.Text) + 1, "")
    End Sub

    Private Sub txtr5fa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtr5fa.TextChanged

    End Sub

    Private Sub Prop_Sets()
        Dim obj As New AdvanceClosedAging_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_txtr1fa = txtr1fa.Text
        obj.p_txtr1ta = txtr1ta.Text
        obj.p_txtr2fa = txtr2fa.Text
        obj.p_txtr2ta = txtr2ta.Text
        obj.p_txtr3fa = txtr3fa.Text
        obj.p_txtr3ta = txtr3ta.Text
        obj.p_txtr4fa = txtr4fa.Text
        obj.p_txtr4ta = txtr4ta.Text
        obj.p_txtr5fa = txtr5fa.Text
        obj.p_txtr5ta = txtr5ta.Text
        SetSettingsObj(obj, Me.Name, GetType(AdvanceClosedAging_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New AdvanceClosedAging_Properties
        GetSettingsObj(obj, Me.Name, GetType(AdvanceClosedAging_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        txtr1fa.Text = obj.p_txtr1fa
        txtr1ta.Text = obj.p_txtr1ta
        txtr2fa.Text = obj.p_txtr2fa
        txtr2ta.Text = obj.p_txtr2ta
        txtr3fa.Text = obj.p_txtr3fa
        txtr3ta.Text = obj.p_txtr3ta
        txtr4fa.Text = obj.p_txtr4fa
        txtr4ta.Text = obj.p_txtr4ta
        txtr5fa.Text = obj.p_txtr5fa
        txtr5ta.Text = obj.p_txtr5ta

    End Sub
End Class

Public Class AdvanceClosedAging_Properties
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
    Private txtr1fa As String = ""
    Public Property p_txtr1fa() As String
        Get
            Return txtr1fa
        End Get
        Set(ByVal value As String)
            txtr1fa = value
        End Set
    End Property
    Private txtr1ta As String = ""
    Public Property p_txtr1ta() As String
        Get
            Return txtr1ta
        End Get
        Set(ByVal value As String)
            txtr1ta = value
        End Set
    End Property
    Private txtr2fa As String = ""
    Public Property p_txtr2fa() As String
        Get
            Return txtr2fa
        End Get
        Set(ByVal value As String)
            txtr2fa = value
        End Set
    End Property
    Private txtr2ta As String = ""
    Public Property p_txtr2ta() As String
        Get
            Return txtr2ta
        End Get
        Set(ByVal value As String)
            txtr2ta = value
        End Set
    End Property
    Private txtr3fa As String = ""
    Public Property p_txtr3fa() As String
        Get
            Return txtr3fa
        End Get
        Set(ByVal value As String)
            txtr3fa = value
        End Set
    End Property
    Private txtr3ta As String = ""
    Public Property p_txtr3ta() As String
        Get
            Return txtr3ta
        End Get
        Set(ByVal value As String)
            txtr3ta = value
        End Set
    End Property
    Private txtr4fa As String = ""
    Public Property p_txtr4fa() As String
        Get
            Return txtr4fa
        End Get
        Set(ByVal value As String)
            txtr4fa = value
        End Set
    End Property
    Private txtr4ta As String = ""
    Public Property p_txtr4ta() As String
        Get
            Return txtr4ta
        End Get
        Set(ByVal value As String)
            txtr4ta = value
        End Set
    End Property
    Private txtr5fa As String = ""
    Public Property p_txtr5fa() As String
        Get
            Return txtr5fa
        End Get
        Set(ByVal value As String)
            txtr5fa = value
        End Set
    End Property
    Private txtr5ta As String = ""
    Public Property p_txtr5ta() As String
        Get
            Return txtr5ta
        End Get
        Set(ByVal value As String)
            txtr5ta = value
        End Set
    End Property
End Class