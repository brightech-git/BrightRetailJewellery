Imports System.Data.OleDb
Public Class frmTransferSumCostCentreWise
    Dim objGridShower As frmGridDispDia
    Dim objGridShower1 As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim f_costname As String
    Dim t_costname As String

    Private Sub frmTransferSumCostCentreWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTransferSumCostCentreWise_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        cmbItemName.Items.Clear()
        cmbtoCostCenter.Text = ""

        txtCCostid.Text = cnHOCostId
        txtCorpCCName.Text = GetSqlValue(cn, "SELECT TOP 1  COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE     COSTID = '" & cnHOCostId & "' ")

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstttCostCentre.Enabled = True
            chkLstttCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE  ACTIVE = 'Y' AND  COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(MAIN,'') <>'Y' ) ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstttCostCentre)
        Else
            chkLstttCostCentre.Enabled = False
            chkLstttCostCentre.Items.Clear()
        End If

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
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
        chkAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtCCostid.Text = "" Or txtCorpCCName.Text = "" Then Exit Sub
        Dim chkTCostId As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbtoCostCenter.Text & "'")
        Dim chkFCostId As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbFromCostCenter.Text & "'")
        Dim chktoCostName As String = GetSelectedCostIddouble(chkLstttCostCentre, True)
        Dim dtGrid As New DataTable
        If chktoCostName <> "" Then
            strSql = " EXEC " & cnAdminDb & "..PROC_TRANSFERSUMMARYCOSTCENTREWISE"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@MCOSTNAME = '" & txtCorpCCName.Text & "'"
            strSql += vbCrLf + " ,@MCOSTID = '" & txtCCostid.Text & "'"
            strSql += vbCrLf + " ,@CCOSTID = '" & chktoCostName & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            MsgBox("Check At Least One Cost Name  ", MsgBoxStyle.Information)
            chkLstttCostCentre.Focus()
            Exit Sub
        End If

        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON_SUM ORDER BY GROUP1,GROUP2,RESULT"
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
        objGridShower.Text = "TRANSFER  ISSUE SUMMARY COSTCENTRE WISE "


        Dim tit As String = ""
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            tit += cmbItemName.Text + " - "
        End If
        tit += "TRANSFER  ISSUE SUMMARY COSTCENTRE WISE " + IIf(rbtIssue.Checked, "ISSUE", "RECEIPT")


        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Show()
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyCode = Keys.D Then
        '    If objGridShower.gridView.Rows.Count > 0 Then
        '        Dim dtGrid1 As New DataTable
        '        Dim tagno As String = objGridShower.gridView.CurrentRow.Cells("TAGNO").Value.ToString
        '        Dim ItemName As String = objGridShower.gridView.CurrentRow.Cells("ITEMNAME").Value.ToString
        '        If tagno.ToString = "" Then Exit Sub
        '        strSql = vbCrLf + " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) STNITEMNAME"
        '        strSql += vbCrLf + " ,STNPCS"
        '        strSql += vbCrLf + " ,STNWT"
        '        strSql += vbCrLf + " ,STNRATE"
        '        strSql += vbCrLf + " ,STNAMT"
        '        strSql += vbCrLf + " ,DESCRIP"
        '        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagno & "'"
        '        strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ItemName & "'))"
        '        dtGrid1.Columns.Add("KEYNO", GetType(Integer))
        '        dtGrid1.Columns("KEYNO").AutoIncrement = True
        '        dtGrid1.Columns("KEYNO").AutoIncrementSeed = 0
        '        dtGrid1.Columns("KEYNO").AutoIncrementStep = 1
        '        da = New OleDbDataAdapter(strSql, cn)
        '        da.Fill(dtGrid1)
        '        dtGrid1.Columns("KEYNO").SetOrdinal(dtGrid1.Columns.Count - 1)
        '        If Not dtGrid1.Rows.Count > 0 Then
        '            MsgBox("Record not found", MsgBoxStyle.Information)
        '            Exit Sub
        '        End If
        '        objGridShower1 = New frmGridDispDia
        '        objGridShower1.Name = Me.Name
        '        objGridShower1.gridView.RowTemplate.Height = 21
        '        objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        '        objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        '        objGridShower1.Text = "STONE / DIAMOND DETAILS"
        '        Dim tit As String = ""
        '        tit = " STONE / DIAMOND DETAILS : " & ItemName & " / TAGNO : " & tagno
        '        objGridShower1.lblTitle.Text = tit
        '        objGridShower1.StartPosition = FormStartPosition.CenterScreen
        '        objGridShower1.dsGrid.DataSetName = objGridShower1.Name
        '        objGridShower1.dsGrid.Tables.Add(dtGrid1)
        '        objGridShower1.gridView.DataSource = objGridShower1.dsGrid.Tables(0)
        '        objGridShower1.FormReSize = False
        '        objGridShower1.FormReLocation = False
        '        objGridShower1.pnlFooter.Visible = False
        '        objGridShower1.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '        'DataGridView_SummaryFormatting(objGridShower1.gridView)
        '        FormatGridColumns(objGridShower1.gridView, False, False, , False)
        '        objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        '        objGridShower1.Show()
        '        objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        '        objGridShower1.FormReSize = True
        '        objGridShower1.FormReLocation = True
        '    End If
        'End If
    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).Visible = True
            Next
            For CNT As Integer = 20 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("COUNTERNAME").Visible = False
            .Columns("GROUP1").Visible = False
            .Columns("GROUP2").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmTransferSumCostCentreWise_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstttCostCentre, obj.p_chkCostCentreSelectAll)
        SetSettingsObj(obj, Me.Name, GetType(frmTransferSumCostCentreWise_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTransferSumCostCentreWise_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTransferSumCostCentreWise_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstttCostCentre, obj.p_chkLstttCostCentre, cnCostName)
    End Sub

    Private Sub chktCostCentreSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chktCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstttCostCentre, chktCostCentreSelectAll.Checked)
    End Sub
End Class

Public Class frmTransferSumCostCentreWise_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
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
    Private chkFCostCentreSelectAll As Boolean = False
    Public Property p_chkFCostCentreSelectAll() As Boolean
        Get
            Return chkFCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkFCostCentreSelectAll = value
        End Set
    End Property

    Private chkLstttCostCentre As New List(Of String)
    Public Property p_chkLstttCostCentre() As List(Of String)
        Get
            Return chkLstttCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstttCostCentre = value
        End Set
    End Property








End Class






