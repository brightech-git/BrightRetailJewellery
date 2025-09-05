Imports System.Data.OleDb
Public Class FRM_ACC_OUTSTANDING
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtAcName As New DataTable
    Dim dtAcGroup As New DataTable
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub FRM_ACC_OUTSTANDING_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub FRM_ACC_OUTSTANDING_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N')"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        ''ACGROUP
        strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,'1' RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACGRPNAME,CONVERT(vARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
        strSql += " ORDER BY RESULT,ACGRPNAME"
        dtAcGroup = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcGroup)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcGroup, dtAcGroup, "ACGRPNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcName.Enter
        ''ACNAME
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD where isnull(active,'Y') <> 'H'"
        If chkCmbAcGroup.Text <> "ALL" Then
            strSql += " AND ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN (" & GetQryString(chkCmbAcGroup.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        dtAcName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcName)
        BrighttechPack.GlobalMethods.FillCombo(cmbAcName, dtAcName, "ACNAME", True)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '  If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Prop_Sets()
        If Not CheckBckDays(userId, Me.Name, dtpDate.Value) Then dtpDate.Focus() : Exit Sub
        strSql = vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ACC_OUTSTANDING','U') IS NOT NULL DROP TABLE TEMP" & systemId & "ACC_OUTSTANDING"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " 	ACCODE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE) ACNAME"
        strSql += vbCrLf + " 	,DEBIT,CREDIT,BALANCE,TRANDATE,TRANNO"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "ACC_OUTSTANDING"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " 	SELECT "
        strSql += vbCrLf + " 		ACCODE"
        strSql += vbCrLf + " 		,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT END) DEBIT"
        strSql += vbCrLf + " 		,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT END) CREDIT	"
        strSql += vbCrLf + " 		,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1 * AMOUNT END) BALANCE"
        strSql += vbCrLf + " 		,MAX(TRANDATE) TRANDATE"
        strSql += vbCrLf + " 		,MAX(TRANNO) TRANNO"
        strSql += vbCrLf + " 	FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strSql += vbCrLf + " 	WHERE TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND O.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND O.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbAcGroup.Text <> "ALL" And chkCmbAcGroup.Text <> "" Then
            strSql += vbCrLf + " AND O.ACCODE IN"
            strSql += vbCrLf + "(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y') <> 'H' AND ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkCmbAcGroup.Text) & ")))"
        End If
        If cmbAcName.Text <> "ALL" And cmbAcName.Text <> "" Then
            strSql += vbCrLf + " 	AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & ")"
        End If
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'') != 'Y'"
        strSql += vbCrLf + " 	GROUP BY ACCODE"
        strSql += vbCrLf + " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        strSql = vbCrLf + " SELECT ACCODE,ACNAME PARTICULAR,DEBIT,CREDIT,BALANCE,TRANDATE,TRANNO,1 RESULT,'  ' COLHEAD FROM TEMP" & systemId & "ACC_OUTSTANDING"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '' ACCODE,'GRAND TOTAL' PARTICULAR,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT,SUM(BALANCE) BALANCE,NULL TRANDATE,NULL TRANNO,2 RESULT,'G' COLHEAD FROM TEMP" & systemId & "ACC_OUTSTANDING"
        strSql += vbCrLf + " ORDER BY RESULT,ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.DataSource = dtGrid
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ACCOUNTS OUTSTANDING"
        Dim tit As String = "ACCOUNTS OUTSTANDING" + vbCrLf
        tit += " ASON " & dtpDate.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULAR")
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("ACCODE").Width = 100
            .Columns("PARTICULAR").Width = 250
            .Columns("PARTICULAR").HeaderText = "ACNAME"
            .Columns("DEBIT").Width = 120
            .Columns("CREDIT").Width = 120
            .Columns("BALANCE").Width = 120
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = 100
            .Columns("TRANNO").Width = 100
            .Columns("TRANDATE").HeaderText = "LAST DATE"
            .Columns("TRANNO").HeaderText = "LAST NO"
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Prop_Gets()
        dtpDate.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New FRM_ACC_OUTSTANDING_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup)
        obj.p_cmbAcName = cmbAcName.Text
        SetSettingsObj(obj, Me.Name, GetType(FRM_ACC_OUTSTANDING_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New FRM_ACC_OUTSTANDING_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_ACC_OUTSTANDING_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup, "ALL")
        cmbAcName.Text = obj.p_cmbAcName
    End Sub
End Class

Public Class FRM_ACC_OUTSTANDING_Properties

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

    Private chkCmbAcGroup As New List(Of String)
    Public Property p_chkCmbAcGroup() As List(Of String)
        Get
            Return chkCmbAcGroup
        End Get
        Set(ByVal value As List(Of String))
            chkCmbAcGroup = value
        End Set
    End Property

    Private cmbAcName As String

    Public Property p_cmbAcName() As String
        Get
            Return cmbAcName
        End Get
        Set(ByVal value As String)
            cmbAcName = value
        End Set
    End Property

End Class