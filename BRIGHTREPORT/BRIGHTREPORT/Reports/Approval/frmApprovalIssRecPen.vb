Imports System.Data.OleDb
Public Class frmApprovalIssRecPen
    '250213 VASANTHAN For WHITEFIRE
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dtItem As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtCompanyName As New DataTable
    Dim dtNodeId As New DataTable
    Dim flagHighlight As Boolean = IIf(GetAdmindbSoftValue("RPT_COLOR", "N") = "Y", True, False)
    Public Sub New() '
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtGridView
            .Columns.Add("SERNO", GetType(Integer))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("TRANDATE", GetType(Date))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("DIAPCS", GetType(Integer))
            .Columns.Add("DIAWT", GetType(Decimal))
            .Columns.Add("DIAAMT", GetType(Decimal))
            .Columns.Add("STNPCS", GetType(Integer))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("STNAMT", GetType(Decimal))
            .Columns.Add("MC", GetType(Decimal))
            .Columns.Add("OTHERAMT", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(String))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("NAME", GetType(String))
            .Columns.Add("SALESMAN", GetType(String))
            .Columns.Add("NARRATION", GetType(String))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("PRUNNO", GetType(String))
            .Columns.Add("PNAME", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("PSALESMAN", GetType(String))
            .Columns.Add("RESULT", GetType(String))
            .Columns.Add("DUEDATE", GetType(String))
        End With
        gridView.DataSource = dtGridView
        FormatGridColumns(gridView)
        gridView.ColumnHeadersVisible = True
        With gridView
            .Columns("SERNO").Width = 50
            .Columns("SERNO").HeaderText = "S.NO"
            .Columns("RUNNO").Width = 80
            .Columns("RUNNO").HeaderText = "APPROVAL NO"
            .Columns("TRANDATE").Width = 80
            .Columns("TAGNO").Width = 70
            .Columns("ITEMNAME").Width = 160
            .Columns("PARTICULAR").Width = 160
            .Columns("PARTICULAR").HeaderText = "SUBITEM NAME"
            .Columns("PCS").Width = 50
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("DIAPCS").Width = 50
            .Columns("DIAPCS").HeaderText = "DIA PCS"
            .Columns("DIAWT").Width = 80
            .Columns("DIAWT").HeaderText = "DIA WT"
            .Columns("DIAAMT").Width = 80
            .Columns("DIAAMT").HeaderText = "DIA AMT"
            .Columns("STYLENO").Width = 80
            .Columns("NAME").Width = 220
            .Columns("SALESMAN").Width = 160
            .Columns("NARRATION").Width = 200
            .Columns("REMARK").Width = 200
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("MC").DefaultCellStyle.Format = "0"
            .Columns("OTHERAMT").DefaultCellStyle.Format = "0.00"
            .Columns("DIAAMT").DefaultCellStyle.Format = "0"
            .Columns("STNAMT").DefaultCellStyle.Format = "0"
        End With

        strSql = " SELECT /*ITEMID,*/ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S'"
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItem, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmApprovalIssRecPen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'LOAD COSTNAME
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        'Me.WindowState = FormWindowState.Maximized
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        'LOAD METAL
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        'LOAD COMPANY       
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPNAYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompanyName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , "ALL")

        'load Item counter
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        cmbItemCounter_OWN.Items.Clear()
        cmbItemCounter_OWN.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemCounter_OWN, False, False)
        cmbItemCounter_OWN.Text = "ALL"

        'LOAD AC AND PER
        strSql = " SELECT DISTINCT NAME FROM( "
        strSql += " SELECT DISTINCT PNAME  AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += " UNION ALL"
        strSql += " SELECT DISTINCT ACNAME  AS NAME FROM " & cnAdminDb & "..ACHEAD"
        strSql += " )X "
        strSql += " ORDER BY NAME"
        'strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE GRPLEDGER NOT IN ('T','P'))"
        'strSql += " ORDER BY PNAME"
        objGPack.FillCombo(strSql, txtPartyname, False, False)

        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen

        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("APPROVAL NO")
        cmbOrderBy.Items.Add("PARTY NAME")
        cmbOrderBy.Items.Add("ITEM")
        cmbOrderBy.Items.Add("ITEMCOUNTER")
        cmbOrderBy.Items.Add("SALESMAN")
        cmbOrderBy.Items.Add("TRANDATE")
        cmbOrderBy.Items.Add("DESIGNER")
        cmbOrderBy.SelectedIndex = 0
        chkAsOnDate.Checked = True

        Try
            strSql = " SELECT DISTINCT SYSTEMID,RESULT FROM (SELECT 'ALL' SYSTEMID,1 RESULT UNION "
            strSql += " SELECT DISTINCT SYSTEMID,2 RESULT FROM " & cnStockDb & "..ISSUE UNION "
            strSql += " SELECT DISTINCT SYSTEMID,2 RESULT FROM " & cnStockDb & "..RECEIPT)X ORDER BY RESULT,SYSTEMID"
            dtNodeId = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtNodeId)
            If dtNodeId.Rows.Count > 0 Then
                BrighttechPack.GlobalMethods.FillCombo(chkCmbNodeId, dtNodeId, "SYSTEMID", , "ALL")
            End If

        Catch ex As Exception

        End Try

        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"

        btnNew_Click(Me, New EventArgs)
        If chkCmbNodeId.Items.Count > 0 Then
            chkCmbNodeId.Text = "ALL"
        End If
        cmbDesigner.Text = "ALL"

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
        txtItemId_NUM.Text = ""
        lblTitle.Text = ""
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCompany, dtCompanyName, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        rbtPending_CheckedChanged(Me, New EventArgs)
        Prop_Gets()
        lblDateTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        cmbItemCounter_OWN.Text = "ALL"
        dtpFrom.Select()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            btnView_Search.Enabled = False
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
            Dim ItemctrId As String = ""
            If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem.Text & "'"
                ItemId = objGPack.GetSqlValue(strSql, "ITEMID", "-1")
            End If
            If cmbItemCounter_OWN.Text <> "" And cmbItemCounter_OWN.Text <> "ALL" Then
                strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ITEMCTRNAME,'') = '" & cmbItemCounter_OWN.Text & "'"
                ItemctrId = objGPack.GetSqlValue(strSql, "ITEMCTRID", "-1")
            End If
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPAPPROVAL')>0 DROP TABLE TEMPTABLEDB..TEMPAPPROVAL"
            strSql += vbCrLf + " SELECT "
            If cmbOrderBy.Text.ToUpper = "PARTY NAME" Then
                strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY NAME,TRANDATE,CONVERT(INT,SUBSTRING(RUNNO,LEN(RUNNO) - PATINDEX('%[A-Z,a-z]%',REVERSE(RUNNO)) + 2 ,20))) SERNO"
            Else
                strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY CONVERT(INT,SUBSTRING(RUNNO,LEN(RUNNO) - PATINDEX('%[A-Z,a-z]%',REVERSE(RUNNO)) + 2 ,20)),TRANDATE,NAME) SERNO"
            End If
            strSql += vbCrLf + " ,SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE,TAGNO,ITEMID,ITEMNAME,PARTICULAR"
            strSql += vbCrLf + " ,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END AS PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END AS NETWT"
            strSql += vbCrLf + " ,CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END AS DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END AS DIAWT"
            strSql += vbCrLf + " ,CASE WHEN DIAAMT <> 0 THEN DIAAMT ELSE NULL END AS DIAAMT"
            strSql += vbCrLf + " ,CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END AS STNPCS"
            strSql += vbCrLf + " ,CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END AS STNWT"
            strSql += vbCrLf + " ,CASE WHEN STNAMT <> 0 THEN STNAMT ELSE NULL END AS STNAMT"
            strSql += vbCrLf + " ,CASE WHEN MCHARGE <> 0 THEN CONVERT(NUMERIC(15),MCHARGE) ELSE NULL END AS MC"
            strSql += vbCrLf + " ,CASE WHEN OTHERAMT <> 0 THEN OTHERAMT ELSE NULL END AS OTHERAMT"
            strSql += vbCrLf + " ,CASE WHEN AMOUNT<>0 THEN AMOUNT ELSE NULL END AS AMOUNT"
            strSql += vbCrLf + " ,STYLENO,TAGTYPE,DESNAME,CONVERT(VARCHAR,RECDATE,105)RECDATE,NAME,SALESMAN,REMARK1 NARRATION,REMARK,CONVERT(VARCHAR(50),PRUNNO)PRUNNO"
            strSql += vbCrLf + " ,PNAME,ITEM,PSALESMAN,PICPATH ,PICNAME,CONVERT (VARCHAR(200), '')AS IMAGEPATH,CONVERT(VARBINARY(8000),'') AS TMPIMG "
            strSql += vbCrLf + ",1 RESULT,CONVERT(VARCHAR(4),NULL)COLHEAD,DUEDATE,ITEMCOUNTER"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPAPPROVAL"
            strSql += vbCrLf + " FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " RUNNO"
            strSql += vbCrLf + " ,TRANDATE,TAGNO,I.ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS PARTICULAR "
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
            If rbtIssue.Checked Or rbtPending.Checked Then

                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " And COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAAMT"

                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNAMT"

                strSql += vbCrLf + " ,I.MCHARGE,I.OTHERAMT"
                strSql += vbCrLf + " ,(AMOUNT+TAX)AMOUNT"
            Else
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO"
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAAMT"


                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO"
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNAMT"
                strSql += vbCrLf + " ,I.MCHARGE,I.OTHERAMT"
                strSql += vbCrLf + " ,(AMOUNT+TAX)AMOUNT"
            End If
            strSql += vbCrLf + " ,(SELECT TOP 1 STYLENO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS STYLENO"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID )AS TAGTYPE"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME  FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERID in (SELECT DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = I.TAGNO and ITEMID = I.ITEMID))AS DESNAME"
            strSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS RECDATE"

            strSql += vbCrLf + " ,(SELECT PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICNAME"
            strSql += vbCrLf + " ,(SELECT PCTPATH FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICPATH"

            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO"
            strSql += vbCrLf + " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS NAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS SALESMAN"
            strSql += vbCrLf + " ,REMARK1"
            strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) REMARK"
            strSql += vbCrLf + " ,RUNNO as PRUNNO"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO"
            strSql += vbCrLf + " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS PNAME"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
            strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS PSALESMAN"
            strSql += vbCrLf + " ,(SELECT TOP 1 ISNULL(CONVERT(VARCHAR(12),DUEDATE,103),'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) DUEDATE"
            strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER E"
            strSql += vbCrLf + " WHERE E.ITEMCTRID =I.ITEMCTRID )AS ITEMCOUNTER"
            If rbtIssue.Checked Or rbtPending.Checked Then
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            Else
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
            End If
            If rbtPending.Checked = True Then
                If chkAsOnDate.Checked Then
                    strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
                End If
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
            Else
                strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
            End If
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If rbtIssue.Checked Or rbtPending.Checked Then
                strSql += vbCrLf + " AND TRANTYPE = 'AI'"
            Else
                strSql += vbCrLf + " AND TRANTYPE = 'AR'"
            End If
            If rbtPending.Checked Then strSql += vbCrLf + " AND REFDATE IS NULL"
            'If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += " AND ITEMID = " & Val(dtItem.Rows(cmbItem.SelectedIndex).Item("ITEMID").ToString) & ""
            If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMID = " & ItemId & ""
            If cmbItemCounter_OWN.Text <> "" And cmbItemCounter_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMCTRID = " & ItemctrId & ""
            If txtApprovalNo.Text <> "" Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtApprovalNo.Text & "'"
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID  = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " and COSTID in"
                strSql += vbCrLf + "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            If chkCmbNodeId.Text <> "ALL" Then
                strSql += vbCrLf + " AND SYSTEMID IN (" & GetQryString(chkCmbNodeId.Text) & ")"
            End If
            If cmbDesigner.Text <> "" And cmbDesigner.Text <> "ALL" Then
                strSql += vbCrLf + " AND EXISTS(SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = I.TAGNO AND ITEMID = I.ITEMID "
                strSql += vbCrLf + " AND DESIGNERID IN(SELECT DESIGNERID  FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERNAME='" & cmbDesigner.Text & "')) "
            End If
            If rbtIssue.Checked Or rbtPending.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT "
                'strSql += " SUBSTRING(RUNNO,6,20)RUNNO"
                strSql += vbCrLf + " RUNNO"
                strSql += vbCrLf + " ,TRANDATE,TAGNO,I.ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEMNAME"
                strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
                strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS PARTICULAR "
                strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) AS DIAPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)) AS DIAAMT"


                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO "
                If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                    strSql += vbCrLf + " AND COMPANYID IN"
                    strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                End If
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0) AS STNPCS"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..APPISSSTONE WHERE ISSSNO = I.SNO AND COMPANYID = '" & strCompanyId & "' AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE != 'D')),0)) AS STNAMT"

                strSql += vbCrLf + " ,I.MCHARGE,I.OTHERAMT"
                strSql += vbCrLf + " ,(AMOUNT+TAX)AMOUNT"
                strSql += vbCrLf + " ,(SELECT TOP 1 STYLENO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS STYLENO"
                strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID )AS TAGTYPE"
                strSql += vbCrLf + " ,(SELECT DESIGNERNAME  FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERID in (SELECT DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = I.TAGNO and ITEMID = I.ITEMID))AS DESNAME"
                strSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS RECDATE"

                strSql += vbCrLf + " ,(SELECT PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICNAME"
                strSql += vbCrLf + " ,(SELECT PCTPATH FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICPATH"

                strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS NAME"
                strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
                strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS SALESMAN"
                strSql += vbCrLf + " ,REMARK1"
                strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) REMARK"
                strSql += vbCrLf + " ,RUNNO as PRUNNO"
                strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO))AS PNAME"
                strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)as ITEM"
                strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER E"
                strSql += vbCrLf + " WHERE E.EMPID =I.EMPID )AS PSALESMAN"
                strSql += vbCrLf + " ,(SELECT TOP 1 ISNULL(CONVERT(VARCHAR(12),DUEDATE,103),'') FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) DUEDATE"
                strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER E"
                strSql += vbCrLf + " WHERE E.ITEMCTRID =I.ITEMCTRID )AS ITEMCOUNTER"
                strSql += vbCrLf + " FROM " & cnStockDb & "..APPISSUE AS I"
                If rbtPending.Checked = True Then
                    If chkAsOnDate.Checked Then
                        strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
                    End If
                    If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                        strSql += vbCrLf + " AND COMPANYID IN"
                        strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                    End If
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
                    If ChkCmbCompany.Text <> "ALL" And ChkCmbCompany.Text <> "" Then
                        strSql += vbCrLf + " AND COMPANYID IN"
                        strSql += vbCrLf + "(SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkCmbCompany.Text) & "))"
                    End If
                End If

                strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
                strSql += vbCrLf + " AND TRANTYPE = 'AI'"
                If rbtPending.Checked Then strSql += " AND REFDATE IS NULL"
                'If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += " AND ITEMID = " & Val(dtItem.Rows(cmbItem.SelectedIndex).Item("ITEMID").ToString) & ""
                If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMID = " & ItemId & ""
                If cmbItemCounter_OWN.Text <> "" And cmbItemCounter_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND ITEMCTRID = " & ItemctrId & ""
                If txtApprovalNo.Text <> "" Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtApprovalNo.Text & "'"
                If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID  = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
                If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                    strSql += vbCrLf + " and COSTID in"
                    strSql += vbCrLf + "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
                End If
                If chkCmbNodeId.Text <> "ALL" Then
                    strSql += vbCrLf + " AND SYSTEMID IN (" & GetQryString(chkCmbNodeId.Text) & ")"
                End If
                If cmbDesigner.Text <> "" And cmbDesigner.Text <> "ALL" Then
                    strSql += vbCrLf + " AND EXISTS(SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = I.TAGNO AND ITEMID = I.ITEMID "
                    strSql += vbCrLf + " AND DESIGNERID IN(SELECT DESIGNERID  FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERNAME='" & cmbDesigner.Text & "')) "
                End If
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

            strSql = " UPDATE  TEMPTABLEDB..TEMPAPPROVAL SET DIAWT=GRSWT,DIAPCS=PCS,GRSWT=NULL,PCS=NULL,NETWT=NULL,AMOUNT=NULL WHERE "
            strSql += vbCrLf + " ITEMNAME IN(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPAPPROVAL)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPAPPROVAL("
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += vbCrLf + " PNAME, PARTICULAR"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += vbCrLf + " PRUNNO, RUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += vbCrLf + " ITEM, ITEMNAME"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += vbCrLf + " PSALESMAN, PARTICULAR"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += vbCrLf + " TRANDATE"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER, PARTICULAR"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME, PARTICULAR"
            End If
            strSql += vbCrLf + " ,RESULT,COLHEAD) "
            strSql += vbCrLf + " SELECT DISTINCT "
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += vbCrLf + " PNAME,PNAME"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += vbCrLf + " PRUNNO, PRUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += vbCrLf + " ITEM, ITEM"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += vbCrLf + " PSALESMAN,PSALESMAN"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += vbCrLf + " TRANDATE "
                'strSql += vbCrLf + " TRANDATE, REPLACE(CONVERT(VARCHAR,TRANDATE,105),'-','/')"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER,ITEMCOUNTER"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME, DESNAME"
            End If
            strSql += vbCrLf + " ,0 RESULT,'T' COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPAPPROVAL"
            '250213

            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPAPPROVAL("
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME,PARTICULAR"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO, RUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM, ITEMNAME"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN,PARTICULAR"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += vbCrLf + " TRANDATE, PARTICULAR"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER,PARTICULAR"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME, PARTICULAR"
            End If
            strSql += " ,RESULT,COLHEAD,PCS,GRSWT,NETWT,DIAPCS,DIAWT,DIAAMT,STNPCS,STNWT,STNAMT,MC,OTHERAMT,AMOUNT)"
            strSql += " Select distinct "
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += vbCrLf + " TRANDATE"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME"
            End If
            strSql += " ,'SUB TOTAL',2 RESULT,'S' COLHEAD,SUM(PCS),SUM(GRSWT),SUM(NETWT), SUM(DIAPCS) "
            strSql += " ,SUM(DIAWT),SUM(DIAAMT), SUM(STNPCS),SUM(STNWT),SUM(STNAMT),SUM(MC),SUM(OTHERAMT),sum(AMOUNT)"
            strSql += " from TEMPTABLEDB..TEMPAPPROVAL"
            strSql += "  Group by "
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += " TRANDATE"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += " ITEMCOUNTER"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += " DESNAME "
            End If
            '250213:

            strSql += vbCrLf + " INSERT INTO"
            strSql += " TEMPTABLEDB..TEMPAPPROVAL("
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME, PARTICULAR"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO, RUNNO"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM, ITEMNAME"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN, PARTICULAR"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += " TRANDATE, PARTICULAR"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER,PARTICULAR"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME, PARTICULAR"
            End If
            strSql += " , RESULT, COLHEAD,"
            strSql += " PCS,GRSWT,NETWT,DIAPCS,DIAWT,DIAAMT,STNPCS,STNWT,STNAMT,MC,OTHERAMT,AMOUNT) "
            strSql += " Select DISTINCT " + IIf(cmbOrderBy.Text <> "TRANDATE", "'ZZZZ'", "NULL") + ",'GRAND TOTAL',3 RESULT,'G' COLHEAD, SUM(PCS),"
            strSql += " SUM(GRSWT),SUM(NETWT),SUM(DIAPCS), SUM(DIAWT), SUM(DIAAMT),SUM(STNPCS),SUM(STNWT),SUM(STNAMT), SUM(MC), SUM(OTHERAMT),SUM(AMOUNT)"
            strSql += " FROM TEMPTABLEDB..TEMPAPPROVAL"
            strSql += " where Result = 1"
            strSql += " End"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "SELECT *"
            If rbtPending.Checked = True And flagHighlight = True Then
                strSql += " ,DATEDIFF(DD,TRANDATE,'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "')DIFF"
            ElseIf rbtPending.Checked = True And flagHighlight = False Then
                strSql += " ,DATEDIFF(DD,TRANDATE,'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "')AGE"
            End If
            strSql += " FROM TEMPTABLEDB..TEMPAPPROVAL ORDER BY "
            If cmbOrderBy.Text = "PARTY NAME" Then
                strSql += " PNAME,"
            ElseIf cmbOrderBy.Text = "APPROVAL NO" Then
                strSql += " PRUNNO,"
            ElseIf cmbOrderBy.Text = "ITEM" Then
                strSql += " ITEM,"
            ElseIf cmbOrderBy.Text = "SALESMAN" Then
                strSql += " PSALESMAN,"
            ElseIf cmbOrderBy.Text = "TRANDATE" Then
                strSql += "ISNULL(TRANDATE,'2050-12-30') ,"
            ElseIf cmbOrderBy.Text = "ITEMCOUNTER" Then
                strSql += vbCrLf + " ITEMCOUNTER,"
            ElseIf cmbOrderBy.Text = "DESIGNER" Then
                strSql += vbCrLf + " DESNAME,"
            End If
            strSql += " RESULT"
            'dtGridView.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)

            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim tit As String = ""
            If rbtIssue.Checked = True Then
                tit = "ISSUE"
            ElseIf rbtReceipt.Checked = True Then
                tit = "RECEIPT"
            ElseIf rbtPending.Checked = True Then
                tit = "PENDING"
            End If
            If ChkImage.Checked = False Then
                If rbtPending.Checked = True Then
                    lblTitle.Text = "APPROVAL " + tit + " REPORT ASON " + dtpFrom.Text
                Else
                    lblTitle.Text = "APPROVAL " + tit + " REPORT FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                End If

                If cmbMetal.Text.ToUpper = "ALL" Then
                    lblTitle.Text += " FOR ALL METAL"
                Else
                    lblTitle.Text += " FOR " & cmbMetal.Text
                End If
                lblTitle.Text += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
                lblTitle.BackColor = gridView.ColumnHeadersDefaultCellStyle.BackColor
                pnlTitle.Visible = True
                gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridView.Columns("COLHEAD").Visible = False
                gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                gridView.Columns("PNAME").Visible = False
                gridView.Columns("PRUNNO").Visible = False
                gridView.Columns("ITEM").Visible = False
                gridView.Columns("PSALESMAN").Visible = False
                gridView.Columns("RESULT").Visible = False
                gridView.Columns("SNO").Visible = False
                gridView.Columns("IMAGEPATH").Visible = False
                gridView.Columns("TMPIMG").Visible = False
                If gridView.Columns.Contains("OTHERAMT") Then gridView.Columns("OTHERAMT").HeaderText = "OTH AMT"

                If rbtIssue.Checked Or rbtPending.Checked Then
                    gridView.Columns("DUEDATE").Visible = True
                Else
                    gridView.Columns("DUEDATE").Visible = False
                End If
                For cnt As Integer = 0 To gridView.Columns.Count - 1
                    gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

                tabView.Show()
                tabMain.SelectedTab = tabView
                GridViewFormat()
                If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
                gridView.Select()
            Else
                strSql = " ALTER TABLE TEMPTABLEDB..TEMPAPPROVAL ALTER COLUMN TMPIMG VARBINARY(MAX)"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
                If Not defaultPic.EndsWith("\") Then defaultPic = defaultPic + "\"
                strSql = " DECLARE @DEFPATH VARCHAR(200)"
                strSql += vbCrLf + " SELECT @DEFPATH='" & defaultPic & "' "
                strSql += vbCrLf + " UPDATE E SET IMAGEPATH=(@defpath + I.PCTFILE)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPAPPROVAL E LEFT JOIN " & cnAdminDb & "..ITEMTAG I ON E.TAGNO = I.TAGNO "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                Dim strSq As String
                Dim dtImage As New DataTable
                strSq = " SELECT PRUNNO,TAGNO,IMAGEPATH FROM TEMPTABLEDB..TEMPAPPROVAL"
                da = New OleDbDataAdapter(strSq, cn)
                da.Fill(dtImage)
                If dtImage.Rows.Count > 0 Then
                    For Each ro As DataRow In dtImage.Rows
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = ro!IMAGEPATH.ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            Dim bmp As New Bitmap(Finfo.FullName)
                            Dim width As Integer = bmp.Width
                            Dim height As Integer = bmp.Height
                            Dim resizeimg As Boolean = False
                            If width > 3000 Then
                                width = 3000
                                resizeimg = True
                            End If
                            If height > 2400 Then
                                height = 2400
                                resizeimg = True
                            End If
                            bmp.Dispose()
                            resizeimg = False
                            If resizeimg = True Then
                                Dim fileName = Finfo.FullName
                                Dim CropRect As New Rectangle(0, 0, width, height)
                                Dim OrignalImage = Image.FromFile(fileName)
                                Dim CropImage = New Bitmap(CropRect.Width, CropRect.Height)
                                Using grp = Graphics.FromImage(CropImage)
                                    grp.DrawImage(OrignalImage, New Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel)
                                    OrignalImage.Dispose()
                                    CropImage.Save(fileName)
                                End Using
                            End If
                            If IO.Directory.Exists(Finfo.Directory.FullName) Then
                                Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                                Dim reader As New IO.BinaryReader(fileStr)
                                Dim reslt As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                fileStr.Read(reslt, 0, reslt.Length)
                                fileStr.Close()
                                strSql = " UPDATE TEMPTABLEDB..TEMPAPPROVAL SET TMPIMG = ? WHERE PRUNNO = '" & ro!PRUNNO.ToString & "' AND TAGNO = '" & ro!TAGNO.ToString & "'"
                                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                                cmd.Parameters.AddWithValue("@image", reslt)
                                cmd.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If
                Dim objReport As New BrighttechReport
                Dim objRptViewer As New frmReportViewer
                objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptApproval, cnDataSource)
                objRptViewer.WindowState = FormWindowState.Minimized
                BrighttechPack.LanguageChange.Set_Language_Form(objRptViewer, LangId)
                objGPack.Validator_Object(objRptViewer)

                objRptViewer.Size = New Size(1032, 745)
                objRptViewer.MaximumSize = New Size(1032, 745)
                objRptViewer.StartPosition = FormStartPosition.Manual
                objRptViewer.Location = New Point((ScreenWid - objRptViewer.Width) / 2, ((ScreenHit - 25) - objRptViewer.Height) / 2)
                objRptViewer.Show()
                objRptViewer.CrystalReportViewer1.Select()

            End If

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

    Private Sub frmApprovalIssRecPen_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

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
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) And tabMain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) And tabMain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmApprovalIssRecPen_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbItem = cmbItem.Text
        obj.p_txtPartyname = txtPartyname.Text
        obj.p_txtApprovalNo = txtApprovalNo.Text
        obj.p_rbtPending = rbtPending.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_cmbOrderBy = cmbOrderBy.Text

        SetSettingsObj(obj, Me.Name, GetType(frmApprovalIssRecPen_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmApprovalIssRecPen_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmApprovalIssRecPen_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbItem.Text = obj.p_cmbItem
        cmbOrderBy.Text = obj.p_cmbOrderBy
        txtPartyname.Text = obj.p_txtPartyname
        txtApprovalNo.Text = obj.p_txtApprovalNo
        rbtPending.Checked = obj.p_rbtPending
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtIssue.Checked = obj.p_rbtIssue
    End Sub

    Private Sub rbtPending_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPending.CheckedChanged
        lblDateTo.Visible = IIf(rbtPending.Checked, False, True)
        dtpTo.Visible = IIf(rbtPending.Checked, False, True)
    End Sub
    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        chkAsOnDate.Checked = Not rbtIssue.Checked And Not rbtReceipt.Checked
        lblDateTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As OnDate"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        lblDateTo.Visible = IIf(rbtReceipt.Checked, False, True)
        dtpTo.Visible = IIf(rbtReceipt.Checked, False, True)
        chkAsOnDate.Checked = IIf(rbtReceipt.Checked, False, True)
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        lblDateTo.Visible = IIf(rbtIssue.Checked, False, True)
        dtpTo.Visible = IIf(rbtIssue.Checked And chkAsOnDate.Checked, False, True)
        chkAsOnDate.Checked = IIf(rbtIssue.Checked, False, True)
    End Sub

    Private Sub txtItemId_NUM_TextChanged(sender As Object, e As EventArgs) Handles txtItemId_NUM.TextChanged
        If txtItemId_NUM.Text <> "" Then
            strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text)
            cmbItem.Text = GetSqlValue(cn, strSql)
        End If
    End Sub
    Private Sub txtItemId_NUM_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,ITEMNAME,"
            strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strSql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM, "
            strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strSql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            strSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE"
            strSql += " FROM " & cnAdminDb & "..ITEMMAST"
            strSql += " WHERE ACTIVE = 'Y'"
            If txtItemId_NUM.Text <> "" Then
                strSql += " AND ITEMID like '%" & txtItemId_NUM.Text & "%'"
            End If
            strSql += " AND STUDDED <> 'S'"
            strSql += GetItemQryFilteration("S")
            strSql += " ORDER BY ITEMNAME"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            txtItemId_NUM.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strSql, cn, 1)
        End If
    End Sub

End Class

Public Class frmApprovalIssRecPen_Properties
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
    Private rbtReceipt As Boolean = False
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private rbtIssue As Boolean = False
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
        End Set
    End Property
    Private chkCmbNodeId As New List(Of String)
    Public Property p_chkCmbNodeId() As List(Of String)
        Get
            Return chkCmbNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkCmbNodeId = value
        End Set
    End Property
End Class