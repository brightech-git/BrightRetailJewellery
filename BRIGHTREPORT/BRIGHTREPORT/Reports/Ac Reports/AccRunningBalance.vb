Imports System.Data.OleDb
Public Class AccRunningBalance
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim StrSql As String
    Dim objGridShower As frmGridDispDia
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, ExitToolStripMenuItem.Click
        Try
            LoadCostName(chkLstCostCentre, False)
            ''LOAD ACNAME
            StrSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
            objGPack.FillCombo(StrSql, cmbAcName_MAN, , False)
            'cmbAcName_MAN.Text = ""
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            Prop_Gets()
            dtpFrom.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If cmbAcName_MAN.Text = "" Or cmbAcName_MAN.Items.Contains(cmbAcName_MAN.Text) = False Then
            cmbAcName_MAN.Focus()
            Exit Sub
        End If
        Dim SelectedCompany As String
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)

        StrSql = "  EXEC " & cnStockDb & "..SP_RPT_ACCRUNNINGBAL"
        StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@ACNAME = '" & cmbAcName_MAN.Text & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        StrSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        Dim dtGrid As New DataTable

        Prop_Sets()
        StrSql = " SELECT * FROM MASTER..TEMPACHEADRUNNINGBAL ORDER BY TRANDATE"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ACCOUNT HEAD RUNNING BALANCE"
        Dim tit As String = " " & cmbAcName_MAN.Text & " RUNNING BALANCE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCostName <> "" Then tit += vbCrLf & " COST CENTRE : " & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.Columns("OPENING").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        objGridShower.gridView.Columns("CLOSING").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            FormatGridColumns(dgv, False, False, , False)
            .Columns("TRANDATE").Width = 100
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("OPENING").Width = 120
            .Columns("DEBIT").Width = 120
            .Columns("CREDIT").Width = 120
            .Columns("CLOSING").Width = 120
            .Columns("OPENING").DefaultCellStyle.Format = "0.00"
            .Columns("DEBIT").DefaultCellStyle.Format = "0.00"
            .Columns("CREDIT").DefaultCellStyle.Format = "0.00"
            .Columns("CLOSING").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub AccRunningBalance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AccRunningBalance_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub

    Private Sub AccRunningBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkSelectAllCostCentre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAllCostCentre.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkSelectAllCostCentre.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New AccRunningBalance_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_cmbAcName_MAN = cmbAcName_MAN.Text
        'obj.p_chkSelectAllCostCentre = chkSelectAllCostCentre.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(AccRunningBalance_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New AccRunningBalance_Properties
        GetSettingsObj(obj, Me.Name, GetType(AccRunningBalance_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        cmbAcName_MAN.Text = obj.p_cmbAcName_MAN
        'chkSelectAllCostCentre.Checked = obj.p_chkSelectAllCostCentre
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
    End Sub
End Class

Public Class AccRunningBalance_Properties
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

    Private cmbAcName_MAN As String = Nothing
    Public Property p_cmbAcName_MAN() As String
        Get
            Return cmbAcName_MAN
        End Get
        Set(ByVal value As String)
            cmbAcName_MAN = value
        End Set
    End Property

    Private chkSelectAllCostCentre As Boolean = False
    Public Property p_chkSelectAllCostCentre() As Boolean
        Get
            Return chkSelectAllCostCentre
        End Get
        Set(ByVal value As Boolean)
            chkSelectAllCostCentre = value
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

End Class