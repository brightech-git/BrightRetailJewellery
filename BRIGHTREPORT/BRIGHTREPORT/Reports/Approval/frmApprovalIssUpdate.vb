Imports System.Data.OleDb
Public Class frmApprovalIssUpdate
    '250213 VASANTHAN For WHITEFIRE
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dtItem As New DataTable
    Dim dtCostCentre As New DataTable
    Dim flagHighlight As Boolean = IIf(GetAdmindbSoftValue("RPT_COLOR", "N") = "Y", True, False)
    Dim CMBGridFlag As Boolean = False
    Dim BILLDATE As DateTime = Nothing
    Dim escape As Boolean = False
    Public Sub New() '
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtGridView
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("TRANDATE", GetType(Date))
            .Columns.Add("NAME", GetType(String))
            .Columns.Add("NEW_PARTY", GetType(String))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("DIAPCS", GetType(Integer))
            .Columns.Add("DIAWT", GetType(Decimal))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("SALESMAN", GetType(String))
            .Columns.Add("NARRATION", GetType(String))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("PRUNNO", GetType(String))
            .Columns.Add("PNAME", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("PSALESMAN", GetType(String))
            .Columns.Add("RESULT", GetType(String))
            .Columns.Add("DUEDATE", GetType(String))
            .Columns.Add("ISSNO", GetType(String))
            .Columns.Add("BATCHNO", GetType(String))
        End With
        gridView.DataSource = dtGridView
        FormatGridColumns(gridView)
        gridView.ColumnHeadersVisible = True
        With gridView
            .Columns("RUNNO").Width = 80
            .Columns("RUNNO").HeaderText = "APPROVAL NO"
            .Columns("TRANDATE").Width = 80
            .Columns("TAGNO").Width = 70
            .Columns("ITEMNAME").Width = 130
            .Columns("PARTICULAR").Width = 130
            .Columns("PARTICULAR").HeaderText = "SUBITEM NAME"
            .Columns("PCS").Width = 50
            .Columns("GRSWT").Width = 60
            .Columns("NETWT").Width = 60
            .Columns("DIAPCS").Width = 50
            .Columns("DIAPCS").HeaderText = "DIA PCS"
            .Columns("DIAWT").Width = 60
            .Columns("DIAWT").HeaderText = "DIA WT"
            .Columns("STYLENO").Width = 80
            .Columns("NAME").Width = 150
            .Columns("SALESMAN").Width = 160
            .Columns("NARRATION").Width = 150
            .Columns("REMARK").Width = 150
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("NEW_PARTY").Width = 150
            .Columns("ISSNO").Width = 80
        End With
        BILLDATE = GetServerDate()

        strSql = " SELECT /*ITEMID,*/ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S'"
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItem, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmApprovalIssUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        'Me.WindowState = FormWindowState.Maximized
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate

        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetal, False, False)

        strSql = " SELECT DISTINCT NAME FROM( "
        strSql += " SELECT DISTINCT PNAME  AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += " )X "
        strSql += " ORDER BY NAME"
        'strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE GRPLEDGER NOT IN ('T','P'))"
        'strSql += " ORDER BY PNAME"
        objGPack.FillCombo(strSql, txtPartyname, False, False)
        objGPack.FillCombo(strSql, cmbUptPartyname, False, False)

        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen

        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("APPROVAL NO")
        cmbOrderBy.Items.Add("PARTY NAME")
        cmbOrderBy.Items.Add("ITEM")
        cmbOrderBy.Items.Add("SALESMAN")
        cmbOrderBy.SelectedIndex = 0

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'cmbItem.Text = "ALL"
        'cmbMetal.Text = "ALL"
        dtGridView.Rows.Clear()
        'rbtPending.Checked = True
        pnlTitle.Visible = False
        lblTitle.Text = ""
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        rbtPending_CheckedChanged(Me, New EventArgs)
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            btnView_Search.Enabled = False
            cmbUptPartyname.Visible = False

            dtGridView.Rows.Clear()
            pnlTitle.Visible = False
            lblTitle.Text = ""
            If dtGridView.Columns.Contains("SNO") Then dtGridView.Columns.Remove("SNO")
            Dim idenCol As New DataColumn("SNO", GetType(Integer))
            idenCol.AutoIncrement = True
            idenCol.AutoIncrementSeed = 1
            idenCol.AutoIncrementStep = 1
            dtGridView.Columns.Add(idenCol)

            Dim ItemId As String = ""
            If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem.Text & "'"
                ItemId = objGPack.GetSqlValue(strSql, "ITEMID", "-1")
            End If
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPAPPROVAL')>0 DROP TABLE TEMPTABLEDB..TEMPAPPROVAL"
            strSql += vbCrLf + " SELECT SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE,TAGNO,ITEMNAME,PARTICULAR"
            strSql += vbCrLf + " ,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END AS PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END AS NETWT"
            strSql += vbCrLf + " ,CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END AS DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END AS DIAWT"
            strSql += vbCrLf + " ,STYLENO,Name,SALESMAN,REMARK1 NARRATION,REMARK,CONVERT(VARCHAR(50),PRUNNO)PRUNNO"
            strSql += vbCrLf + " ,PNAME,ITEM,PSALESMAN,CONVERT (VARCHAR(200), '')AS IMAGEPATH,CONVERT(VARBINARY(8000),'') AS TMPIMG,1 RESULT,CONVERT(VARCHAR(4),NULL)COLHEAD,DUEDATE,ISSNO,BATCHNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPAPPROVAL"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " RUNNO"
            strSql += vbCrLf + " ,TRANDATE,TAGNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS PARTICULAR "
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 STYLENO FROM " & cnStockDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS STYLENO"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO"
            strSql += vbCrLf + " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS NAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS SALESMAN"
            strSql += vbCrLf + " ,REMARK1"
            strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) REMARK"
            strSql += vbCrLf + " ,RUNNO as PRUNNO"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO"
            strSql += vbCrLf + " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS PNAME"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS PSALESMAN"
            strSql += vbCrLf + " ,(SELECT TOP 1 ISNULL(CONVERT(VARCHAR(12),DUEDATE,103),'') FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) DUEDATE,I.SNO AS ISSNO,BATCHNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND COMPANYID = '" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANTYPE = 'AI'"
            strSql += vbCrLf + " AND REFDATE IS NULL"
            'If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += " AND ITEMID = " & Val(dtItem.Rows(cmbItem.SelectedIndex).Item("ITEMID").ToString) & ""
            If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMID = " & ItemId & ""
            If txtApprovalNo.Text <> "" Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtApprovalNo.Text & "'"
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID  = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " and COSTID in"
                strSql += vbCrLf + "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            'strSql += " SUBSTRING(RUNNO,6,20)RUNNO"
            strSql += vbCrLf + " RUNNO"
            strSql += vbCrLf + " ,TRANDATE,TAGNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS PARTICULAR "
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) AS DIAPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 STYLENO FROM " & cnStockDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS STYLENO"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS NAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS SALESMAN"
            strSql += vbCrLf + " ,REMARK1"
            strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) REMARK"
            strSql += vbCrLf + " ,RUNNO as PRUNNO"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS PNAME"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)as ITEM"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS PSALESMAN"
            strSql += vbCrLf + " ,(SELECT TOP 1 ISNULL(CONVERT(VARCHAR(12),DUEDATE,103),'') FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) DUEDATE,I.SNO AS ISSNO,BATCHNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..APPISSUE AS I"
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND COMPANYID = '" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANTYPE = 'AI'"
            If rbtPending.Checked Then strSql += " AND REFDATE IS NULL"
            'If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += " AND ITEMID = " & Val(dtItem.Rows(cmbItem.SelectedIndex).Item("ITEMID").ToString) & ""
            If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMID = " & ItemId & ""
            If txtApprovalNo.Text <> "" Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtApprovalNo.Text & "'"
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID  = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " and COSTID in"
                strSql += vbCrLf + "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If

            strSql += vbCrLf + " )X"
            If txtPartyname.Text <> "" Then strSql += vbCrLf + " WHERE NAME LIKE '" & txtPartyname.Text & "%'"
            If cmbOrderBy.Text.ToUpper = "PARTY NAME" Then
                strSql += vbCrLf + " ORDER BY NAME,TRANDATE,CONVERT(INT,SUBSTRING(RUNNO,LEN(RUNNO) - PATINDEX('%[A-Z,a-z]%',REVERSE(RUNNO)) + 2 ,20))"
            Else
                strSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(RUNNO,LEN(RUNNO) - PATINDEX('%[A-Z,a-z]%',REVERSE(RUNNO)) + 2 ,20)),TRANDATE,NAME"
            End If
            'strSql += " ORDER BY NAME,CONVERT(INT,SUBSTRING(RUNNO,6,20)),TRANDATE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPAPPROVAL)>0"
            'strSql += vbCrLf + " BEGIN"
            'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPAPPROVAL("
            'If cmbOrderBy.Text = "PARTY NAME" Then
            '    strSql += vbCrLf + " PNAME, PARTICULAR"
            'ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
            '    strSql += vbCrLf + " PRUNNO, RUNNO"
            'ElseIf cmbOrderBy.Text = "ITEM" Then
            '    strSql += vbCrLf + " ITEM, ITEMNAME"
            'ElseIf cmbOrderBy.Text = "SALESMAN" Then
            '    strSql += vbCrLf + " PSALESMAN, PARTICULAR"
            'End If
            'strSql += vbCrLf + " ,RESULT,COLHEAD) "
            'strSql += vbCrLf + " SELECT DISTINCT "
            'If cmbOrderBy.Text = "PARTY NAME" Then
            '    strSql += vbCrLf + " PNAME,PNAME"
            'ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
            '    strSql += vbCrLf + " PRUNNO, PRUNNO"
            'ElseIf cmbOrderBy.Text = "ITEM" Then
            '    strSql += vbCrLf + " ITEM, ITEM"
            'ElseIf cmbOrderBy.Text = "SALESMAN" Then
            '    strSql += vbCrLf + " PSALESMAN,PSALESMAN"
            'End If
            'strSql += vbCrLf + " ,0 RESULT,'T' COLHEAD"
            'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPAPPROVAL"
           
            'strSql += " INSERT INTO"
            'strSql += " TEMPTABLEDB..TEMPAPPROVAL("
            'If cmbOrderBy.Text = "PARTY NAME" Then
            '    strSql += " PNAME, PARTICULAR"
            'ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
            '    strSql += " PRUNNO, RUNNO"
            'ElseIf cmbOrderBy.Text = "ITEM" Then
            '    strSql += " ITEM, ITEMNAME"
            'ElseIf cmbOrderBy.Text = "SALESMAN" Then
            '    strSql += " PSALESMAN, PARTICULAR"
            'End If
            'strSql += " , RESULT, COLHEAD,"
            'strSql += " PCS,GRSWT,NETWT,DIAPCS,DIAWT) "
            'strSql += " Select DISTINCT 'ZZZZ','GRAND TOTAL',3 RESULT,'G' COLHEAD, SUM(PCS),"
            'strSql += " SUM(GRSWT),SUM(NETWT),SUM(DIAPCS), SUM(DIAWT)"
            'strSql += " FROM TEMPTABLEDB..TEMPAPPROVAL"
            'strSql += " where Result = 1"
            'strSql += " End"
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            strSql = "SELECT *"
            If rbtPending.Checked And flagHighlight Then
                strSql += " ,DATEDIFF(DD,TRANDATE,'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "')DIFF"
            End If
            strSql += " FROM TEMPTABLEDB..TEMPAPPROVAL ORDER BY "
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN"
            End If
            strSql += " ,RESULT"
            'dtGridView.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)

            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If

            lblTitle.Text = "APPROVAL ISSUE REPORT ASON " + dtpFrom.Text

            If cmbMetal.Text.ToUpper = "ALL" Then
                lblTitle.Text += " FOR ALL METAL"
            Else
                lblTitle.Text += " FOR " & cmbMetal.Text
            End If
            lblTitle.BackColor = gridView.ColumnHeadersDefaultCellStyle.BackColor
            pnlTitle.Visible = True
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("PNAME").Visible = False
            gridView.Columns("PRUNNO").Visible = False
            gridView.Columns("ITEM").Visible = False
            gridView.Columns("PSALESMAN").Visible = False
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("IMAGEPATH").Visible = False
            gridView.Columns("TMPIMG").Visible = False

            gridView.Columns("DUEDATE").Visible = False
            gridView.Columns("NARRATION").Visible = False
            gridView.Columns("REMARK").Visible = False
            gridView.Columns("SALESMAN").Visible = False
            gridView.Columns("STYLENO").Visible = False
            gridView.Columns("ISSNO").Visible = False
            gridView.Columns("BATCHNO").Visible = False
            For cnt As Integer = 0 To gridView.Columns.Count - 1
                gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            gridView.Columns("NAME").HeaderText = "OLD_PARTY"
            gridView.Columns("NEW_PARTY").ReadOnly = False
            tabView.Show()
            GridViewFormat()
            If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Select()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnView_Search.Enabled = True
        End Try
        Prop_Sets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmApprovalIssUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbUptPartyname.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            If escape = True Then Exit Sub
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub


    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "T"
                        .Cells("RUNNO").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
                If rbtPending.Checked And flagHighlight Then
                    Select Case IIf(IsDBNull(.Cells("DIFF").Value), 0, .Cells("DIFF").Value)
                        Case 1 To 7
                            .DefaultCellStyle.BackColor = Color.LightGreen
                        Case 8 To 15
                            .DefaultCellStyle.BackColor = Color.LightBlue
                        Case Is >= 16
                            .DefaultCellStyle.BackColor = Color.LightPink
                    End Select
                End If
            End With
        Next
        gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
    End Function

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
        lblStatus.Text = "Press U to update New Party"
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.U Then
            escape = True
            If gridView.CurrentRow.Cells("RESULT").Value.ToString <> "1" Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("NEW_PARTY")
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) And tabMain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) And tabMain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.U) Then
            If gridView.CurrentRow.Cells("RESULT").Value.ToString <> "1" Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("NEW_PARTY")
            Dim pt As Point = gridView.Location
            cmbUptPartyname.Visible = True
            pt = pt + gridView.GetCellDisplayRectangle(gridView.Columns("NEW_PARTY").Index, gridView.CurrentRow.Index, False).Location
            cmbUptPartyname.Location = pt
            cmbUptPartyname.Width = gridView.Columns("NEW_PARTY").Width
            CMBGridFlag = True
            cmbUptPartyname.SelectedIndex = 0
            cmbUptPartyname.Focus()
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmApprovalIssUpdate_Properties
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbItem = cmbItem.Text
        obj.p_txtPartyname = txtPartyname.Text
        obj.p_txtApprovalNo = txtApprovalNo.Text
        obj.p_rbtPending = rbtPending.Checked
        obj.p_cmbOrderBy = cmbOrderBy.Text
        SetSettingsObj(obj, Me.Name, GetType(frmApprovalIssUpdate_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmApprovalIssUpdate_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmApprovalIssUpdate_Properties))
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbItem.Text = obj.p_cmbItem
        cmbOrderBy.Text = obj.p_cmbOrderBy
        txtPartyname.Text = obj.p_txtPartyname
        txtApprovalNo.Text = obj.p_txtApprovalNo
        rbtPending.Checked = obj.p_rbtPending
    End Sub

    Private Sub rbtPending_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPending.CheckedChanged
        lblDateTo.Visible = IIf(rbtPending.Checked, False, True)
        dtpTo.Visible = IIf(rbtPending.Checked, False, True)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        lblDateTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As OnDate"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub

    Private Sub cmbUptPartyname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbUptPartyname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim pt As Point = gridView.Location
            gridView.CurrentRow.Cells("NEW_PARTY").Value = cmbUptPartyname.Text
            If gridView.CurrentRow.Index = gridView.Rows.Count - 1 Then cmbUptPartyname.Visible = False : btnUpdate.Focus() : Exit Sub
            If gridView.Rows(gridView.CurrentRow.Index + 1).Cells("RESULT").Value.ToString <> "1" Then cmbUptPartyname.Visible = False : btnUpdate.Focus() : Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells("NEW_PARTY")
            cmbUptPartyname.Visible = True
            pt = pt + gridView.GetCellDisplayRectangle(gridView.Columns("NEW_PARTY").Index, gridView.CurrentRow.Index, False).Location
            cmbUptPartyname.Location = pt
            CMBGridFlag = True
            cmbUptPartyname.SelectedIndex = 0
            cmbUptPartyname.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            escape = True
            cmbUptPartyname.Visible = False : btnUpdate.Focus()
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If dtGridView.Rows.Count > 0 Then
            Try
                tran = Nothing
                tran = cn.BeginTransaction()
                strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                Dim dtparty As DataTable
                Dim dtrunno As DataTable
                Dim newtranno As Boolean = True
                Dim TRANNO As Integer
                Dim BATCHNO As String
                Dim PSNO As String = ""

                dtrunno = dtGridView.DefaultView.ToTable(True, "PRUNNO")
                For K As Integer = 0 To dtrunno.Rows.Count - 1
                    newtranno = True
                    For i As Integer = 0 To dtGridView.Rows.Count - 1
                        If dtGridView.Rows(i).Item("NEW_PARTY").ToString.Trim = "" Then Continue For
                        If dtGridView.Rows(i).Item("PRUNNO").ToString <> dtrunno.Rows(K).Item("PRUNNO").ToString Then Continue For
                        If newtranno = True Then
                            PSNO = objGPack.GetSqlValue("SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = '" & dtGridView.Rows(i).Item("BATCHNO").ToString & "'", , "", tran)
                            TRANNO = GetBillNoValue("GEN-APPRECBILLNO", tran)
                            BATCHNO = GetNewBatchno(cnCostId, BILLDATE, tran)
                            newtranno = False
                        End If
                        insertintoissrec(dtGridView.Rows(i).Item("ISSNO").ToString, BILLDATE, "AR", TRANNO, BATCHNO, dtGridView.Rows(i).Item("PRUNNO").ToString, PSNO)
                    Next
                Next
                dtparty = dtGridView.DefaultView.ToTable(True, "NEW_PARTY")
                For J As Integer = 0 To dtparty.Rows.Count - 1
                    If dtparty.Rows(J).Item("NEW_PARTY").ToString = "" Then Continue For
                    Dim RUNNO As String = ""
                    newtranno = True
                    For i As Integer = 0 To dtGridView.Rows.Count - 1
                        If dtGridView.Rows(i).Item("NEW_PARTY").ToString.Trim = "" Then Continue For
                        If dtGridView.Rows(i).Item("NEW_PARTY").ToString <> dtparty.Rows(J).Item("NEW_PARTY").ToString Then Continue For
                        If newtranno = True Then
                            PSNO = objGPack.GetSqlValue("SELECT TOP 1 SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME = '" & dtGridView.Rows(i).Item("NEW_PARTY").ToString & "'", , "", tran)
                            TRANNO = GetBillNoValue("GEN-APPISSBILLNO", tran)
                            BATCHNO = GetNewBatchno(cnCostId, BILLDATE, tran)
                            RUNNO = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "P" + Mid(Format(cnTranToDate, "dd/MM/yyyy").ToString, 9, 2) & TRANNO.ToString
                            newtranno = False
                        End If
                        insertintoissrec(dtGridView.Rows(i).Item("ISSNO").ToString, BILLDATE, "AI", TRANNO, BATCHNO, RUNNO, PSNO)
                    Next
                Next
                tran.Commit()
                tran = Nothing
                btnBack_Click(Me, New System.EventArgs)
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try

        End If
    End Sub

    Private Sub insertintoissrec(ByVal sno As String, ByVal trandate As DateTime, ByVal trantype As String _
    , ByVal tranno As Integer, ByVal BatchNo As String, ByVal runno As String, ByVal PSNO As String)
        Dim dtiss As New DataTable
        strSql = "SELECT * FROM " & cnStockDb & "..ISSUE WHERE SNO='" & sno & "'"
        If trantype = "AR" Then strSql += " AND RUNNO='" & runno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtiss)
        If Not dtiss.Rows.Count > 0 Then
            strSql = "SELECT * FROM " & cnStockDb & "..APPISSUE WHERE SNO='" & sno & "'"
            If trantype = "AR" Then strSql += " AND RUNNO='" & runno & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtiss)
            If Not dtiss.Rows.Count > 0 Then Exit Sub
        End If

        Dim issSno As String = ""
        If trantype = "AR" Then
            issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
        Else
            issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
        End If

        If trantype = "AR" Then
            strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
        Else
            strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
        End If
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT"
        strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS,REFNO,REFDATE,COSTID"
        strSql += " ,COMPANYID,FLAG,EMPID,TAGPCS,TAGGRSWT,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
        strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
        strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT"
        strSql += " ,RUNNO,CASHID,VATEXM,TAX,SC,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
        strSql += " ,TOUCH,ESTSNO,FIN_DISCOUNT,RATEID,SETGRPID,TRANFLAG"
        strSql += ",ORDSTATE_ID )"
        strSql += " VALUES("
        strSql += " '" & issSno & "'" ''SNO
        strSql += " ," & tranno & "" 'TRANNO
        strSql += " ,'" & trandate & "'" 'TRANDATE 'BillDate
        strSql += " ,'" & trantype & "'" 'TRANTYPE
        strSql += " ," & Val(dtiss.Rows(0).Item("PCS").ToString) & "" 'PCS
        strSql += " ," & Val(dtiss.Rows(0).Item("GRSWT").ToString) & "" 'GRSWT
        strSql += " ," & Val(dtiss.Rows(0).Item("NETWT").ToString) & "" 'NETWT
        strSql += " ," & Math.Abs(Val(dtiss.Rows(0).Item("GRSWT").ToString) - Val(dtiss.Rows(0).Item("NETWT").ToString)) & "" 'LESSWT
        strSql += " ," & Val(dtiss.Rows(0).Item("PUREWT").ToString) & "" 'PUREWT '
        strSql += " ,'" & dtiss.Rows(0).Item("TAGNO").ToString & "'" 'TAGNO
        strSql += " ," & Val(dtiss.Rows(0).Item("ITEMID").ToString) & "" 'ITEMID
        strSql += " ," & Val(dtiss.Rows(0).Item("SUBITEMID").ToString) & "" 'SUBITEMID
        strSql += " ," & Val(dtiss.Rows(0).Item("WASTPER").ToString) & "" 'WASTPER
        strSql += " ," & Val(dtiss.Rows(0).Item("WASTAGE").ToString) & "" 'WASTAGE
        strSql += " ," & Val(dtiss.Rows(0).Item("MCGRM").ToString) & "" 'MCGRM
        strSql += " ," & Val(dtiss.Rows(0).Item("MCHARGE").ToString) & "" 'MCHARGE
        strSql += " ," & Val(dtiss.Rows(0).Item("AMOUNT").ToString) & "" 'AMOUNT
        strSql += " ," & Val(dtiss.Rows(0).Item("RATE").ToString) & "" 'RATE
        strSql += " ," & Val(dtiss.Rows(0).Item("BOARDRATE").ToString) & "" 'BOARDRATE
        strSql += " ,'" & dtiss.Rows(0).Item("SALEMODE").ToString & "'" 'SALEMODE
        strSql += " ,'" & dtiss.Rows(0).Item("GRSNET").ToString & "'" 'GRSNET
        strSql += " ,'" & dtiss.Rows(0).Item("TRANSTATUS").ToString & "'" 'TRANSTATUS ''
        If trantype = "AR" Then
            strSql += " ,'" & dtiss.Rows(0).Item("TRANNO").ToString & "'" 'REFNO ''
        Else
            strSql += " ,''" 'REFNO ''
        End If

        If trantype = "AR" Then
            strSql += " ,'" & Format(dtiss.Rows(0).Item("TRANDATE"), "yyyy-MM-dd") & "'" 'REFDATE NULL
        Else
            strSql += " ,NULL" 'REFDATE NULL
        End If
        strSql += " ,'" & dtiss.Rows(0).Item("COSTID").ToString & "'" 'COSTID 
        strSql += " ,'" & dtiss.Rows(0).Item("COMPANYID").ToString & "'" 'COMPANYID
        strSql += " ,'" & dtiss.Rows(0).Item("FLAG").ToString & "'" 'FLAG 
        strSql += " ," & Val(dtiss.Rows(0).Item("EMPID").ToString) & "" 'EMPID
        strSql += " ," & Val(dtiss.Rows(0).Item("TAGPCS").ToString) & "" 'TAGPCS
        strSql += " ," & Val(dtiss.Rows(0).Item("TAGGRSWT").ToString) & "" 'TAGGRSWT
        strSql += " ," & Val(dtiss.Rows(0).Item("TAGNETWT").ToString) & "" 'TAGNETWT
        strSql += " ," & Val(dtiss.Rows(0).Item("TAGRATEID").ToString) & "" 'TAGRATEID
        strSql += " ," & Val(dtiss.Rows(0).Item("TAGSVALUE").ToString) & "" 'TAGSVALUE
        strSql += " ,'" & dtiss.Rows(0).Item("TAGDESIGNER").ToString & "'" 'TAGDESIGNER
        strSql += " ," & Val(dtiss.Rows(0).Item("ITEMCTRID").ToString) & "" 'ITEMCTRID
        strSql += " ," & Val(dtiss.Rows(0).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
        strSql += " ," & Val(dtiss.Rows(0).Item("PURITY").ToString) & "" 'PURITY
        strSql += " ,'" & dtiss.Rows(0).Item("TABLECODE").ToString & "'" 'TABLECODE
        strSql += " ,''" 'INCENTIVE
        strSql += " ,''" 'WEIGHTUNIT
        strSql += " ,'" & dtiss.Rows(0).Item("CATCODE").ToString & "'" 'CATCODE
        strSql += " ,''" 'OCATCODE
        If trantype = "AR" Then
            strSql += " ,'" & dtiss.Rows(0).Item("ACCODE").ToString & "'" 'ACCODE
        Else
            strSql += " ,''" 'ACCODE
        End If

        strSql += " ,0" 'ALLOY
        strSql += " ,'" & BatchNo & "'" 'BATCHNO
        strSql += " ,'" & dtiss.Rows(0).Item("REMARK1").ToString & "'" 'REMARK1
        strSql += " ,'" & dtiss.Rows(0).Item("REMARK2").ToString & "'" 'REMARK2
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Val(dtiss.Rows(0).Item("DISCOUNT").ToString) & "" 'DISCOUNT
        strSql += " ,'" & runno & "'" 'RUNNO
        strSql += " ,'" & dtiss.Rows(0).Item("CASHID").ToString & "'" 'CASHID
        strSql += " ,'" & dtiss.Rows(0).Item("VATEXM").ToString & "'" 'VATEXM
        strSql += " ," & Val(dtiss.Rows(0).Item("TAX").ToString) & "" 'TAX
        strSql += " ," & Val(dtiss.Rows(0).Item("SC").ToString) & "" 'SC
        strSql += " ," & Val(dtiss.Rows(0).Item("STNAMT").ToString) & "" 'STONEAMT
        strSql += " ," & Val(dtiss.Rows(0).Item("MISCAMT").ToString) & "" 'MISCAMT
        strSql += " ,'" & dtiss.Rows(0).Item("METALID").ToString & "'" 'METALID
        strSql += " ,'" & dtiss.Rows(0).Item("STONEUNIT").ToString & "'" 'STONEUNIT
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ," & Val(dtiss.Rows(0).Item("TOUCH").ToString) & "" 'TOUCH '
        strSql += " ,'" & Val(dtiss.Rows(0).Item("ESTSNO").ToString) & "'" 'ESTNO'
        strSql += " ," & Val(dtiss.Rows(0).Item("FIN_DISCOUNT").ToString) & "" 'DISCOUNT
        strSql += " ," & Val(dtiss.Rows(0).Item("RATEID").ToString) & "" 'RATEID
        strSql += " ,'" & dtiss.Rows(0).Item("SETGRPID").ToString & "'" 'SETGRPID
        strSql += " ,'" & dtiss.Rows(0).Item("TRANFLAG").ToString & "'" 'TRANFLAG
        strSql += "," & Val(dtiss.Rows(0).Item("ORDSTATE_ID").ToString)
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

        strSql = " IF NOT (SELECT COUNT(*) FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = '" & BatchNo & "')>0"
        strSql += vbCrLf + " BEGIN"
        strSql += " INSERT INTO " & cnStockDb & "..CUSTOMERINFO"
        strSql += " (BATCHNO,PSNO,REMARK1,COSTID,PAN,DUEDATE)VALUES"
        strSql += " ('" & BatchNo & "','" & psno & "','','" & cnCostId & "','',NULL)"
        strSql += vbCrLf + " END"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        If trantype = "AR" Then
            strSql = "UPDATE " & cnStockDb & "..ISSUE SET REFNO='" & tranno.ToString & "',REFDATE='" & Format(trandate, "yyyy-MM-dd") & "' WHERE SNO='" & sno & "' AND RUNNO='" & runno & "'"
            strSql += " UPDATE " & cnStockDb & "..APPISSUE SET REFNO='" & tranno.ToString & "',REFDATE='" & Format(trandate, "yyyy-MM-dd") & "' WHERE SNO='" & sno & "' AND RUNNO='" & runno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End If

    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
End Class

Public Class frmApprovalIssUpdate_Properties
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbOrderBy As String = "APPROVAL NO"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
    Private cmbItem As String = "ALL"
    Public Property p_cmbItem() As String
        Get
            Return cmbItem
        End Get
        Set(ByVal value As String)
            cmbItem = value
        End Set
    End Property
    Private txtPartyname As String = ""
    Public Property p_txtPartyname() As String
        Get
            Return txtPartyname
        End Get
        Set(ByVal value As String)
            txtPartyname = value
        End Set
    End Property
    Private txtApprovalNo As String = ""
    Public Property p_txtApprovalNo() As String
        Get
            Return txtApprovalNo
        End Get
        Set(ByVal value As String)
            txtApprovalNo = value
        End Set
    End Property
    Private rbtPending As Boolean = True
    Public Property p_rbtPending() As Boolean
        Get
            Return rbtPending
        End Get
        Set(ByVal value As Boolean)
            rbtPending = value
        End Set
    End Property
End Class