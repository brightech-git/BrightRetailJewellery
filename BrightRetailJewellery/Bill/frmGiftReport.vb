Imports System.Data.OleDb
Public Class frmGiftReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridview.DataSource = Nothing
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub frmGiftReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub frmGiftReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            Dim dtCostCentre As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New Gift_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSectAll.Checked
        ' GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(Gift_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New Gift_Properties
        GetSettingsObj(obj, Me.Name, GetType(Gift_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        ' chkCostCentreSectAll.Checked = obj.p_chkCostCentreSelectAll
        '  SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim SelectedCosts As String = ""
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCosts = GetSelectedCostId(chkCmbCostCentre, False)
        End If

        strSql = " SELECT G.CARDID,G.BATCHNO,C.NAME AS PARTICULAR ,TRANDATE AS BILLDATE,RUNNO REFNO,QTY,AMOUNT,DUEDAYS FROM " & cnStockDb & "..GVTRAN G"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD C ON G.CARDID=C.CARDCODE AND C.CARDTYPE='G'"
        strSql += vbCrLf + " WHERE 1=1 "
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            strSql += vbCrLf + " AND COSTID IN('" & SelectedCosts.Replace(",", "','") & "')"
        End If
        If SelectedCompanyId <> "" Then
            strSql += vbCrLf + " AND COMPANYID IN('" & SelectedCompanyId.Replace(",", "','") & "')"
        End If
        strSql += vbCrLf + " ORDER BY TRANDATE,REFNO,AMOUNT "
        da = New OleDbDataAdapter(strSql, cn)
        Dim ds As New DataSet
        Dim dtGrid As New DataTable
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        gridview.DataSource = Nothing
        gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridview.Invalidate()
        For Each gvCol As DataGridViewColumn In gridview.Columns
            gvCol.Width = gvCol.Width
        Next
        gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        If Not dtGrid.Rows.Count > 1 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridview.DataSource = dtGrid
        DataGridView_SummaryFormatting(gridview)
        Prop_Sets()
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("CARDID").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("PARTICULAR").Width = 170
            .Columns("BILLDATE").Width = 100
            .Columns("BILLDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("REFNO").Width = 80
            .Columns("QTY").Width = 60
            .Columns("AMOUNT").Width = 80
            .Columns("DUEDAYS").Width = 80
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub gridview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If e.KeyChar = Chr(Keys.D) Then
            If Not gridview.Rows.Count > 0 Then Exit Sub
            Dim oldItem As Integer = Nothing
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\Gvcodeprint.mem")
            'For Each ro As DataRow In dtTagPrint.Rows
            '    write.WriteLine(LSet("CARD", 7) & ":" & ro!CARDID.ToString)
            '    write.WriteLine(LSet("BATCHNO", 7) & ":" & ro!BATCHNO.ToString)
            'Next
            With gridview.CurrentRow
                write.WriteLine(LSet("CARD", 7) & ":" & .Cells("CARDID").Value.ToString)
                write.WriteLine(LSet("BATCHNO", 7) & ":" & .Cells("BATCHNO").Value.ToString)
            End With
            write.Flush()
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\GvPrint.exe") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\GvPrint.exe")
            Else
                MsgBox("GvPrint.exe not found", MsgBoxStyle.Information)
            End If
        End If
    End Sub
End Class
Public Class Gift_Properties
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
End Class