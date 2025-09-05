Imports System.Data.OleDb
Public Class frmMeltingIssue
    Dim dtMelting As New DataTable
    Dim strSql As String
    Dim dtPendingBagNo As New DataTable
    Dim dtPendingBagNoCat As New DataTable
    Dim cmd As OleDbCommand
    Dim tranNo As Integer = Nothing
    Dim batchNo As String = Nothing
    Dim cancelFlag As Boolean = False
    Dim ManualBag As Boolean = False
    Dim bagno As String = Nothing
    Dim IssOnly As Boolean = IIf(GetAdmindbSoftValue("MELTING_ISSONLY", "N", ) = "Y", True, False)
    Dim MRMI_MANUALNO As Boolean = IIf(GetAdmindbSoftValue("MRMI_MANUALNO", "Y") = "Y", True, False)
    Dim objManualBill As frmManualBillNoGen
    Dim flagupdate As Boolean = False
    Dim flagupdateBatchno As String = ""
    Dim flagupdateTranno As String = ""
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        dtMelting = New DataTable
        dtMelting.Columns.Add("BAGNO", GetType(String))
        dtMelting.Columns.Add("CATEGORY", GetType(String))
        dtMelting.Columns.Add("PURITY", GetType(Decimal))
        dtMelting.Columns.Add("RATE", GetType(Decimal))
        dtMelting.Columns.Add("PCS", GetType(Integer))
        dtMelting.Columns.Add("WEIGHT", GetType(Decimal))
        dtMelting.Columns.Add("NETWT", GetType(Decimal))
        dtMelting.Columns.Add("LESSWT", GetType(Decimal))
        dtMelting.Columns.Add("PUREWT", GetType(Decimal))
        dtMelting.Columns.Add("DIFF_WT", GetType(Decimal))
        dtMelting.Columns.Add("VALUE", GetType(Decimal))
        dtMelting.Columns.Add("GRPBAG", GetType(String))
        dtMelting.Columns.Add("REMARK1", GetType(String))
        dtMelting.Columns.Add("REMARK2", GetType(String))
        dtMelting.AcceptChanges()
        GridView.DataSource = dtMelting
        GridView.Columns("CATEGORY").MinimumWidth = 250
        GridView.Columns("PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("DIFF_WT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView.Columns("WEIGHT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("NETWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("LESSWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("PUREWT").DefaultCellStyle.Format = "0.000"
        GridView.Columns("VALUE").DefaultCellStyle.Format = "0.00"
        GridView.Columns("DIFF_WT").DefaultCellStyle.Format = "0.00"
        GridView.Columns("GRPbag").Visible = False
    End Sub

    Public Sub calcDiffWeight()
        If Val(txtDiffWeight_WET.Text) = 0 Then
            'txtDiffWeight_WET.Text = 0
        End If
        If Val(txtWeight_WET.Text) > 0 Then
            txtIssueWeight_WET.Text = Format(Val(txtWeight_WET.Text) - Val(txtDiffWeight_WET.Text), "0.000")
        End If
        If Val(txtLessStnWt_WET.Text) > 0 Then
            txtIssueLessStnWt_WET.Text = Format(Val(txtLessStnWt_WET.Text) - Val(txtDiffWeight_WET.Text), "0.000")
        End If
        If Val(txtNetWt_WET.Text) > 0 Then
            txtIssueNetWt_WET.Text = Format(Val(txtNetWt_WET.Text) - Val(txtDiffWeight_WET.Text), "0.000")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        btnSave.Enabled = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtpBillDate.Value = GetEntryDate(GetServerDate)
        CmbTrantype.Items.Clear()
        CmbTrantype.Items.Add("")
        CmbTrantype.Items.Add("MISC ISSUE")
        CmbTrantype.Items.Add("PARTLY SALE")
        CmbTrantype.Items.Add("PURCHASE")
        CmbTrantype.Items.Add("SALES RETURN")
        CmbTrantype.Text = ""
        chkPurchase.Checked = False
        dtpBillDate.Enabled = False
        ''CmbTrantype.Enabled = False
        txtBagNo.Clear()
        txtGrpbag.Clear()
        objGPack.TextClear(Me)
        CmbCompany_MAN.Text = "ALL"
        loadGrid()
        tabMain.SelectedTab = tabGeneral
        flagupdate = False
        flagupdateBatchno = ""
        flagupdateTranno = ""
        lblTranNo.Text = "..."
        dtpDate.Enabled = True
        dtpDate.Select()
    End Sub

    Private Function loadGrid()
        cancelFlag = False
        ManualBag = False
        'objGPack.TextClear(Me)
        txtRate_AMT.Text = Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY =100 AND METALID = 'G' AND METALTYPE = 'M' ORDER BY PURITY DESC)"))), "0.00")
        cmbSmith_OWN.Text = ""
        Dim StrTrantype As String = ""
        If CmbTrantype.Text <> "" Then
            If CmbTrantype.Text = "MISC ISSUE" Then
                StrTrantype = "MI"
            ElseIf CmbTrantype.Text = "PARTLY SALE" Then
                StrTrantype = "SA"
            ElseIf CmbTrantype.Text = "PURCHASE" Then
                StrTrantype = "PU"
            ElseIf CmbTrantype.Text = "SALES RETURN" Then
                StrTrantype = "SR"
            End If
        End If
        Dim CostCentre As Boolean = False
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..COSTCENTRE  "
        If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then CostCentre = True
        Dim ISPREVYRDB As Boolean = False
        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & DateAdd(DateInterval.Day, -1, cnTranFromDate) & "' BETWEEN STARTDATE AND ENDDATE"
        Dim PREVYRDB As String = objGPack.GetSqlValue(strSql)
        strSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & PREVYRDB & "'"
        If objGPack.GetSqlValue(strSql, , "-1") <> "-1" Then ISPREVYRDB = True Else PREVYRDB = ""
        Dim CURRDB As String
        CURRDB = cnStockDb

        If IssOnly = True Then
            strSql = vbCrLf + " SELECT BAGNO,METAL,CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(RATE))RATE "
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND BAGNO NOT IN (SELECT BAGNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> '' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S' "
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then
                strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "'))"
            Else
                strSql += vbCrLf + " )"
            End If
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "')"
            strSql += vbCrLf + " )X"
            If chkPurchase.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M' "
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            ElseIf StrTrantype <> "" Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE <='" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M'"
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE <='" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            End If
            strSql += vbCrLf + " GROUP BY BAGNO,CATEGORY,METAL"
        Else

            strSql = ""
            strSql = vbCrLf + " SELECT BAGNO,METAL,CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT,GRPBAG"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(RATE))RATE "
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
NEXTT:
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..RECEIPT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '0'"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,0 LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..OPENWEIGHT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '0'"
            strSql += vbCrLf + " AND ISNULL(STOCKTYPE,'') = 'C'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE='SA' THEN TAGGRSWT-GRSWT ELSE GRSWT END GRSWT,CASE WHEN TRANTYPE='SA' THEN TAGNETWT-NETWT ELSE NETWT END NETWT,LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND TRANTYPE ='SA' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'MI'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'I' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            'strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'IIS'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"

            If ISPREVYRDB Then
                CURRDB = PREVYRDB
                strSql += vbCrLf + " UNION ALL"
                ISPREVYRDB = False
                GoTo NEXTT

            End If
            strSql += vbCrLf + " )X"
            If chkPurchase.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & CURRDB & "..RECEIPT WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M' "
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & CURRDB & "..ISSUE WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            End If
            strSql += vbCrLf + " GROUP BY BAGNO,CATEGORY,METAL,GRPBAG"
            strSql += vbCrLf + " having SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 /*or SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) > 0*/ "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        dtPendingBagNoCat = New DataTable
        da.Fill(dtPendingBagNoCat)
        dtPendingBagNoCat.AcceptChanges()
        CURRDB = cnStockDb
        If PREVYRDB <> "" Then ISPREVYRDB = True
        If IssOnly = True Then
            strSql = vbCrLf + " SELECT BAGNO,METAL,'' CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,'' aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND BAGNO NOT IN (SELECT BAGNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> '' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S' "
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then
                strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "'))"
            Else
                strSql += vbCrLf + " )"
            End If
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "')"
            strSql += vbCrLf + " )X"
            If chkPurchase.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M'"
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            End If
            strSql += vbCrLf + " GROUP BY BAGNO,METAL"
        Else
            strSql = vbCrLf + " SELECT BAGNO,METAL,'' CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
NEXTT1:
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,'' aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & CURRDB & "..RECEIPT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '0'"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,'' aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,0 LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & CURRDB & "..OPENWEIGHT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '0'"
            strSql += vbCrLf + " AND ISNULL(STOCKTYPE,'') ='C'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,'' aS CATEGORY"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE='SA' THEN TAGGRSWT-GRSWT ELSE GRSWT END GRSWT,CASE WHEN TRANTYPE='SA' THEN TAGNETWT-NETWT ELSE NETWT END NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND TRANTYPE ='SA' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'MI'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'I' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,'' aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            'strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'IIS'"
            If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            If ISPREVYRDB Then
                CURRDB = PREVYRDB
                strSql += vbCrLf + " UNION ALL"
                ISPREVYRDB = False
                GoTo NEXTT1
            End If
            strSql += vbCrLf + " )X"
            If chkPurchase.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M'"
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE ='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
                If StrTrantype <> "" Then
                    strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            ElseIf StrTrantype <> "" Then
                strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE <='" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M'"
                strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE <='" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            End If
            strSql += vbCrLf + " GROUP BY BAGNO,METAL"
            strSql += vbCrLf + " having SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 /*or SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) > 0*/ "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        dtPendingBagNo = New DataTable
        da.Fill(dtPendingBagNo)
        dtPendingBagNo.AcceptChanges()


        'gridViewPendingBagNo_OWN.DefaultCellStyle.BackColor = Color.White
        'gridViewPendingBagNo_OWN.DefaultCellStyle.SelectionBackColor = Color.White
        'gridViewPendingBagNo_OWN.DefaultCellStyle.SelectionForeColor = Color.Black
        'gridViewPendingBagNo_OWN.DefaultCellStyle.ForeColor = Color.Black
        gridViewPendingBagNo.BorderStyle = BorderStyle.Fixed3D
        gridViewPendingBagNo.BackgroundColor = Color.White
        gridViewPendingBagNo.Visible = False
        gridViewPendingBagNo.DataSource = dtPendingBagNo.Copy 'Select("GRSWT <> 0 OR NETWT <> 0 ", Nothing)
        gridViewPendingBagNo.GridColor = Color.White
        With gridViewPendingBagNo
            .Columns("BAGNO").Width = 75
            .Columns("METAL").Width = 50
            .Columns("CATEGORY").Width = 100
            .Columns("CATEGORY").Visible = False
            .Columns("GRSWT").Width = 80
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 80
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").Width = 80
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 80
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        dtMelting.Rows.Clear()
        dtMelting.AcceptChanges()
    End Function
    Private Sub frmMeltingIssue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBAck_Click(Me, New EventArgs)
                Exit Sub
            End If
            If gridViewPendingBagNo.Focused Then
                If btnSave.Enabled Then
                    btnSave.Focus()
                Else
                    If MessageBox.Show("Do you want enter manual Bag weight?", "Bag No Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        txtBagNo.Text = GetBagno()
                        CmbCategory_OWN.Focus()
                        ManualBag = True
                        Exit Sub
                    End If
                    dtpDate.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub frmMeltingIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridViewPendingBagNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmMeltingIssue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D')"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbSmith_OWN, , False)
        Loadcategory()
        ''Load Company Name       
        strSql = " SELECT 'ALL' COMPANYNAME "
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, CmbCompany_MAN, , True)
        CmbCompany_MAN.Text = strCompanyName
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Function Loadcategory(Optional ByVal metalid As String = "", Optional ByVal Catnames As String = "")
        strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY C"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID=M.METALID "
        If metalid <> "" Then strSql += vbCrLf + " WHERE C.METALID='" & metalid & "'"
        If Catnames <> "" Then strSql += vbCrLf + " AND CATNAME IN (" & Catnames & ")"
        strSql += vbCrLf + "  ORDER BY C.CATNAME"
        objGPack.FillCombo(strSql, CmbCategory_OWN, True, False)
    End Function

    Private Function GetBagno(Optional ByVal UPDATE As Boolean = False) As String
        Dim BagNo As String = Nothing
GENBAGNO:
        'BagNo = cnCostId & GetTranDbSoftControlValue("BAGNO", UPDATE, tran)
        BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("BAGNO", UPDATE, tran)
        ''check
        strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
        strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
        If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
            GoTo GENBAGNO
        End If
        Return BagNo
    End Function

    Private Sub gridViewPendingBagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewPendingBagNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridViewPendingBagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewPendingBagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridViewPendingBagNo.RowCount > 0 Then
                gridViewPendingBagNo.CurrentCell = gridViewPendingBagNo.Rows(gridViewPendingBagNo.CurrentRow.Index).Cells(0)
                txtBagNo.Text = gridViewPendingBagNo.CurrentRow.Cells("BAGNO").Value.ToString
                'txtCategory.Text = gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString
                Dim METAL As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & gridViewPendingBagNo.CurrentRow.Cells("METAL").Value.ToString & "'", , "", )
                Dim dtbagcat() As DataRow = dtPendingBagNoCat.Select("BAGNO='" & txtBagNo.Text & "'")
                Dim catnames As String = ""
                Dim firstname As String
                For Each xrow As DataRow In dtbagcat
                    If firstname = "" Then firstname = xrow!category.ToString
                    If Not catnames.Contains(xrow!category.ToString) Then catnames += "'" & xrow!category.ToString & "',"
                Next
                catnames = Mid(catnames, 1, Len(catnames) - 1)
                Loadcategory(METAL, catnames)
                CmbCategory_OWN.Text = firstname 'gridViewPendingBagNo.CurrentRow.Cells("CATEGORY").Value.ToString
                txtPurity_PER.Text = Format(Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & firstname & "')")), "0.00")
                Dim rate As Double = Val(gridViewPendingBagNo.CurrentRow.Cells("AMOUNT").Value.ToString) / Val(gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString)
                txtRate_AMT.Text = Format(rate, "0.00")
                txtWeight_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString
                txtLessStnWt_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("LESSWT").Value.ToString
                txtNetWt_WET.Text = gridViewPendingBagNo.CurrentRow.Cells("NETWT").Value.ToString
                'txtGrpbag.Text = gridViewPendingBagNo.CurrentRow.Cells("GRPBAG").Value.ToString
                'txtRate_AMT.Enabled = True
                'txtRate_AMT.ReadOnly = False
                Me.SelectNextControl(txtBagNo, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub gridViewPendingBagNo_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewPendingBagNo.LostFocus
        gridViewPendingBagNo.Visible = False
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_AMT.GotFocus
        'If txtRate_AMT.Text.Trim <> "" Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ''validation
        If objGPack.Validator_Check(Me) Then Exit Sub
        'If CmbCompany_MAN.Text = "" Then
        '    MsgBox("Company Name should not Empty", MsgBoxStyle.Information)
        '    CmbCompany_MAN.Select()
        '    Exit Sub
        'End If
        Dim app_Achead As String = objGPack.GetSqlValue("SELECT ISNULL(ACTIVE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , Nothing)
        If app_Achead = "" Then
            MsgBox("Partycode not approve", MsgBoxStyle.Information)
            cmbSmith_OWN.Focus()
            Exit Sub
        End If

        If txtBagNo.Text = "" Then
            MsgBox("Bag No should not Empty", MsgBoxStyle.Information)
            txtBagNo.Select()
            Exit Sub
        End If
        If Val(txtRate_AMT.Text) = 0 Then
            MsgBox("Bulliion Rate Should not Empty", MsgBoxStyle.Information)
            txtRate_AMT.Focus()
            Exit Sub
        End If
        If Val(txtWeight_WET.Text) = 0 Then
            MsgBox("Weight should not Empty", MsgBoxStyle.Information)
            txtWeight_WET.Focus()
            Exit Sub
        End If
        Dim ro As DataRow = dtMelting.NewRow
        ro("BAGNO") = txtBagNo.Text
        ro("CATEGORY") = CmbCategory_OWN.Text
        ro("PURITY") = Val(txtPurity_PER.Text)
        ro("RATE") = Val(txtRate_AMT.Text)
        ro("PCS") = Val(txtPcs_NUM.Text)
        'ro("WEIGHT") = Val(txtWeight_WET.Text)
        'ro("NETWT") = Val(txtNetWt_WET.Text)
        'ro("LESSWT") = Val(txtLessStnWt_WET.Text)
        'ro("PUREWT") = Val(txtNetWt_WET.Text) * (Val(txtPurity_PER.Text) / 100)
        'ro("VALUE") = Val(txtNetWt_WET.Text) * Val(txtRate_AMT.Text)
        ro("WEIGHT") = Val(txtIssueWeight_WET.Text)
        ro("NETWT") = Val(txtIssueNetWt_WET.Text)
        ro("LESSWT") = Val(txtIssueLessStnWt_WET.Text)
        ro("PUREWT") = Val(txtIssueNetWt_WET.Text) * (Val(txtPurity_PER.Text) / 100)
        ro("VALUE") = Val(txtIssueNetWt_WET.Text) * Val(txtRate_AMT.Text)
        ro("GRPBAG") = txtGrpbag.Text
        ro("REMARK1") = txtRemark1.Text.Trim
        ro("REMARK2") = txtRemark2.Text.Trim
        ro("DIFF_WT") = Val(txtDiffWeight_WET.Text)
        dtMelting.Rows.Add(ro)
        dtMelting.AcceptChanges()
        CmbCategory_OWN.Text = ""
        txtPurity_PER.Clear()
        txtRate_AMT.Clear()
        txtPcs_NUM.Clear()
        txtWeight_WET.Clear()
        txtNetWt_WET.Clear()
        txtLessStnWt_WET.Clear()
        txtDiffWeight_WET.Clear()
        txtIssueWeight_WET.Clear()
        txtIssueLessStnWt_WET.Clear()
        txtIssueNetWt_WET.Clear()
        txtRemark1.Clear()
        txtRemark2.Clear()
        If ManualBag = False Then txtBagNo.Clear()
        btnSave.Enabled = True
        RefreshPendingBagNoView()
        If gridViewPendingBagNo.RowCount > 0 Then
            'txtBagNo.Select()
            cmbSmith_OWN.Focus()
        ElseIf ManualBag = True Then
            CmbCategory_OWN.Focus()
        Else
            btnSave.Select()
        End If
    End Sub
    Private Sub RefreshPendingBagNoView()
        Dim bagNo As String = ""

        Dim dt As New DataTable
        dt = dtPendingBagNo.Copy
        For Each Row As DataRow In dtMelting.Rows
            bagNo += "'" & Row("BAGNO").ToString & "',"
        Next
        If bagNo.Length > 0 Then
            bagNo = Mid(bagNo, 1, bagNo.Length - 1)
            dt.DefaultView.RowFilter = "BAGNO NOT IN (" + bagNo + ") "
        End If
        dt.AcceptChanges()
        gridViewPendingBagNo.DataSource = dt
    End Sub
    Private Sub GridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles GridView.UserDeletedRow
        dtMelting.AcceptChanges()
        RefreshPendingBagNoView()
        If Not GridView.RowCount > 0 Then
            btnSave.Enabled = False
            dtpDate.Select()
        End If
    End Sub

    Private Sub dtpDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If GridView.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbSmith_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSmith_OWN.GotFocus
        If GridView.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not btnSave.Enabled Then Exit Sub
        Try

            If MRMI_MANUALNO Then
                objManualBill = New frmManualBillNoGen
                objManualBill.ShowDialog()
                If Val(objManualBill.txtBillNo_NUM.Text.ToString) = 0 Then MsgBox("Tran No not valid...", MsgBoxStyle.Information) : Exit Sub
            End If

            tran = Nothing
            tran = cn.BeginTransaction()
GenBillNo:
            If flagupdate = True And flagupdateBatchno <> "" And flagupdateTranno <> "" Then

                strSql = vbCrLf + " DELETE FROM " & cnStockDb & "..ISSUE WHERE "
                strSql += vbCrLf + " TRANDATE = '" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = 'IIS' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '' "
                strSql += vbCrLf + " AND BATCHNO = '" & flagupdateBatchno & "'"
                strSql += vbCrLf + " AND TRANNO = '" & flagupdateTranno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPT WHERE "
                strSql += vbCrLf + " TRANDATE = '" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND TRANTYPE = 'RRE' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '' "
                strSql += vbCrLf + " AND BATCHNO = '" & flagupdateBatchno & "'"
                strSql += vbCrLf + " AND TRANNO = '" & flagupdateTranno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnStockDb & "..MELTINGDETAIL "
                strSql += " WHERE BATCHNO = '" & flagupdateBatchno & "' "
                strSql += " AND TRANNO = '" & flagupdateTranno & "'"
                strSql += " AND RECISS = 'I' AND TRANTYPE = 'MI'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnStockDb & "..MELTINGDETAIL "
                strSql += " WHERE BATCHNO = '" & flagupdateBatchno & "' "
                strSql += " AND TRANNO = '" & flagupdateTranno & "'"
                strSql += " AND RECISS = 'R' AND TRANTYPE = 'MR'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                tranNo = flagupdateTranno
                batchNo = flagupdateBatchno
            Else
                tranNo = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'", , , tran))
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo + 1 & "' WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'"
                strSql += " and CONVERT(INT,CTLTEXT) = " & tranNo & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                If Not cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If
                If MRMI_MANUALNO Then
                    If Val(objManualBill.txtBillNo_NUM.Text.ToString) <> 0 Then
                        tranNo = Val(objManualBill.txtBillNo_NUM.Text.ToString)
                    End If
                Else
                    tranNo += 1
                End If
                batchNo = GetNewBatchno(cnCostId, dtpDate.Value.ToString("yyyy-MM-dd"), tran)
            End If
            If ManualBag = True Then
                Dim BAGNO As String = GetBagno(True)
            End If
            For cnt As Integer = 0 To GridView.RowCount - 1
                InsertSADetails(cnt)
            Next
            If flagupdate = True And flagupdateBatchno <> "" And flagupdateTranno <> "" Then
                Dim cntMelting As String = ""
                Dim cntMeltingIssue As String = ""
                strSql = "SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE BATCHNO='" & batchNo & "'"
                cntMelting = objGPack.GetSqlValue(strSql,,, tran).Trim
                strSql = "SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & batchNo & "'"
                cntMeltingIssue = objGPack.GetSqlValue(strSql,,, tran).Trim
                If cntMelting <> cntMeltingIssue Then
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox("Meltingdetail vs Issue Record Mismatch", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            tran.Commit()
            tran = Nothing
            MsgBox(tranNo & " Generated..")
            Dim pBatchno As String = batchNo
            Dim pBillDate As Date = dtpDate.Value.Date.ToString("yyyy-MM-dd")
            btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":SMI")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":SMI;" &
                    LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                    LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";" &
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Public Sub InsertSADetails(ByVal index As Integer)
        With GridView.Rows(index)
            Dim insertDiffWeight As Boolean = True
            'Dim issSno As String = GetNewSno(TranSnoType.ISSUECODE, tran)
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATEGORY").Value.ToString & "'", , , tran)
            Dim Metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATEGORY").Value.ToString & "'", , , tran)
            Dim metal As String = objGPack.GetSqlValue("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID='" & Metalid & "'", , "", tran)
            Dim mAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSmith_OWN.Text & "'", , , tran)
            Dim DiffWt_Accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = 'DIFFWT'", , , tran)
            Dim wast As Double = Nothing
            Dim wastPer As Double = Nothing
            Dim alloy As Double = Nothing
            Dim type As String = objGPack.GetSqlValue("SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATEGORY").Value.ToString & "')", , , tran)
            Dim drIssDet() As DataRow
            drIssDet = dtPendingBagNoCat.Select("BAGNO='" & .Cells("BAGNO").Value.ToString & "' AND METAL='" & metal & "'")
            Dim MTotWeight As Decimal = Val(.Cells("WEIGHT").Value.ToString)
            Dim MTotNetwt As Decimal = Val(.Cells("NETWT").Value.ToString)
            Dim MTotValue As Double = Val(.Cells("VALUE").Value.ToString)
            For k As Integer = 0 To drIssDet.Length - 1
                If MTotWeight <= 0 Then Exit For
                Dim McatCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & drIssDet(k).Item("CATEGORY").ToString & "'", , , tran)
                Dim MMetalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & drIssDet(k).Item("CATEGORY").ToString & "'", , , tran)
                Dim MWeight As Decimal = 0
                Dim MNetwt As Decimal = 0
                Dim MPurewt As Decimal = 0
                Dim MValue As Double = 0
                If MTotWeight > Val(drIssDet(k).Item("GRSWT").ToString) Then
                    MWeight = Val(drIssDet(k).Item("GRSWT").ToString)
                    MNetwt = Val(drIssDet(k).Item("NETWT").ToString)
                    MValue = Val(drIssDet(k).Item("NETWT").ToString) * Val(.Cells("RATE").Value.ToString)
                Else
                    MWeight = MTotWeight
                    MNetwt = MTotNetwt
                    MValue = MTotValue
                End If
                If MNetwt > 0 And Val(GridView.Rows(index).Cells("PURITY").Value.ToString & "") > 0 Then
                    MPurewt = MNetwt * Val(GridView.Rows(index).Cells("PURITY").Value.ToString & "") / 100
                End If
                strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
                strSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
                strSql += " )"
                strSql += " VALUES("
                strSql += " '" & GetNewSno(TranSnoType.ISSUECODE, tran) & "'" ''SNO
                strSql += " ," & tranNo & "" 'TRANNO
                strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ,'IIS'" 'TRANTYPE
                strSql += " ,0" 'PCS
                strSql += " ," & MWeight & "" 'GRSWT
                strSql += " ," & MNetwt & "" 'NETWT
                strSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                strSql += " ," & MPurewt & "" 'PUREWT '0
                strSql += " ,''" 'TAGNO
                strSql += " ,0" 'ITEMID
                strSql += " ,0" 'SUBITEMID
                strSql += " ," & wastPer & "" 'WASTPER
                strSql += " ," & wast & "" 'WASTAGE
                strSql += " ,0" 'MCGRM
                strSql += " ,0" 'MCHARGE
                strSql += " ,0" ' & MValue & "" 'AMOUNT
                strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
                strSql += " ,''" 'SALEMODE
                strSql += " ,'N'" 'GRSNET
                strSql += " ,''" 'TRANSTATUS ''
                strSql += " ,''" 'REFNO ''
                strSql += " ,NULL" 'REFDATE NULL
                strSql += " ,'" & cnCostId & "'" 'COSTID 
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                Select Case type
                    Case "O"
                        strSql += " ,'O'" 'FLAG 
                    Case "M"
                        strSql += " ,'M'" 'FLAG 
                    Case Else
                        strSql += " ,'T'" 'FLAG 
                End Select
                strSql += " ,0" 'EMPID
                strSql += " ,0" 'TAGGRSWT
                strSql += " ,0" 'TAGNETWT
                strSql += " ,0" 'TAGRATEID
                strSql += " ,0" 'TAGSVALUE
                strSql += " ,''" 'TAGDESIGNER  
                strSql += " ,0" 'ITEMCTRID
                strSql += " ,0" 'ITEMTYPEID
                strSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                strSql += " ,''" 'TABLECODE
                strSql += " ,''" 'INCENTIVE
                strSql += " ,''" 'WEIGHTUNIT
                strSql += " ,'" & McatCode & "'" 'CATCODE
                strSql += " ,''" 'OCATCODE
                strSql += " ,'" & mAccode & "'" 'ACCODE
                strSql += " ," & alloy & "" 'ALLOY
                strSql += " ,'" & batchNo & "'" 'BATCHNO
                strSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                strSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                strSql += " ,'" & userId & "'" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,0" 'DISCOUNT
                strSql += " ,''" 'RUNNO
                strSql += " ,''" 'CASHID
                strSql += " ,0" 'TAX
                strSql += " ,0" 'STONEAMT
                strSql += " ,0" 'MISCAMT
                strSql += " ,'" & MMetalid & "'" 'METALID
                strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                MTotWeight = MTotWeight - MWeight
                MTotNetwt = MTotNetwt - MNetwt
                'MTotValue = MTotValue - MValue
                If Val(.Cells("DIFF_WT").Value.ToString) > 0 And insertDiffWeight = True Then
                    insertDiffWeight = False
                    strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                    strSql += " ("
                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                    strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                    strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                    strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                    strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                    strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                    strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                    strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                    strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                    strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                    strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                    strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
                    strSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
                    strSql += " )"
                    strSql += " VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ISSUECODE, tran) & "'" ''SNO
                    strSql += " ," & tranNo & "" 'TRANNO
                    strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    strSql += " ,'IIS'" 'TRANTYPE
                    strSql += " ,0" 'PCS
                    strSql += " ," & Val(.Cells("DIFF_WT").Value.ToString) & "" 'GRSWT
                    strSql += " ," & Val(.Cells("DIFF_WT").Value.ToString) & "" 'NETWT
                    strSql += " ,0" 'LESSWT
                    strSql += " ,0" 'PUREWT '0
                    strSql += " ,''" 'TAGNO
                    strSql += " ,0" 'ITEMID
                    strSql += " ,0" 'SUBITEMID
                    strSql += " ," & wastPer & "" 'WASTPER
                    strSql += " ," & wast & "" 'WASTAGE
                    strSql += " ,0" 'MCGRM
                    strSql += " ,0" 'MCHARGE
                    strSql += " ,0" ' & MValue & "" 'AMOUNT
                    strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                    strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
                    strSql += " ,''" 'SALEMODE
                    strSql += " ,'N'" 'GRSNET
                    strSql += " ,''" 'TRANSTATUS ''
                    strSql += " ,''" 'REFNO ''
                    strSql += " ,NULL" 'REFDATE NULL
                    strSql += " ,'" & cnCostId & "'" 'COSTID 
                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    Select Case type
                        Case "O"
                            strSql += " ,'O'" 'FLAG 
                        Case "M"
                            strSql += " ,'M'" 'FLAG 
                        Case Else
                            strSql += " ,'T'" 'FLAG 
                    End Select
                    strSql += " ,0" 'EMPID
                    strSql += " ,0" 'TAGGRSWT
                    strSql += " ,0" 'TAGNETWT
                    strSql += " ,0" 'TAGRATEID
                    strSql += " ,0" 'TAGSVALUE
                    strSql += " ,''" 'TAGDESIGNER  
                    strSql += " ,0" 'ITEMCTRID
                    strSql += " ,0" 'ITEMTYPEID
                    strSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                    strSql += " ,''" 'TABLECODE
                    strSql += " ,''" 'INCENTIVE
                    strSql += " ,''" 'WEIGHTUNIT
                    strSql += " ,'" & McatCode & "'" 'CATCODE
                    strSql += " ,''" 'OCATCODE
                    strSql += " ,'" & DiffWt_Accode & "'" 'ACCODE
                    strSql += " ," & alloy & "" 'ALLOY
                    strSql += " ,'" & batchNo & "'" 'BATCHNO
                    strSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                    strSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                    strSql += " ,'" & userId & "'" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,0" 'DISCOUNT
                    strSql += " ,''" 'RUNNO
                    strSql += " ,''" 'CASHID
                    strSql += " ,0" 'TAX
                    strSql += " ,0" 'STONEAMT
                    strSql += " ,0" 'MISCAMT
                    strSql += " ,'" & MMetalid & "'" 'METALID
                    strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
                If Val(.Cells("DIFF_WT").Value.ToString) < 0 And insertDiffWeight = True Then
                    insertDiffWeight = False
                    strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                    strSql += " ("
                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                    strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                    strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                    strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                    strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                    strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                    strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                    strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                    strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                    strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                    strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                    strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
                    strSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
                    strSql += " )"
                    strSql += " VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ISSUECODE, tran) & "'" ''SNO
                    strSql += " ," & tranNo & "" 'TRANNO
                    strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    strSql += " ,'RRE'" 'TRANTYPE
                    strSql += " ,0" 'PCS
                    strSql += " ," & -1 * Val(.Cells("DIFF_WT").Value.ToString) & "" 'GRSWT
                    strSql += " ," & -1 * Val(.Cells("DIFF_WT").Value.ToString) & "" 'NETWT
                    strSql += " ,0" 'LESSWT
                    strSql += " ,0" 'PUREWT '0
                    strSql += " ,''" 'TAGNO
                    strSql += " ,0" 'ITEMID
                    strSql += " ,0" 'SUBITEMID
                    strSql += " ," & wastPer & "" 'WASTPER
                    strSql += " ," & wast & "" 'WASTAGE
                    strSql += " ,0" 'MCGRM
                    strSql += " ,0" 'MCHARGE
                    strSql += " ,0" ' & MValue & "" 'AMOUNT
                    strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                    strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
                    strSql += " ,''" 'SALEMODE
                    strSql += " ,'N'" 'GRSNET
                    strSql += " ,''" 'TRANSTATUS ''
                    strSql += " ,''" 'REFNO ''
                    strSql += " ,NULL" 'REFDATE NULL
                    strSql += " ,'" & cnCostId & "'" 'COSTID 
                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    Select Case type
                        Case "O"
                            strSql += " ,'O'" 'FLAG 
                        Case "M"
                            strSql += " ,'M'" 'FLAG 
                        Case Else
                            strSql += " ,'T'" 'FLAG 
                    End Select
                    strSql += " ,0" 'EMPID
                    strSql += " ,0" 'TAGGRSWT
                    strSql += " ,0" 'TAGNETWT
                    strSql += " ,0" 'TAGRATEID
                    strSql += " ,0" 'TAGSVALUE
                    strSql += " ,''" 'TAGDESIGNER  
                    strSql += " ,0" 'ITEMCTRID
                    strSql += " ,0" 'ITEMTYPEID
                    strSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                    strSql += " ,''" 'TABLECODE
                    strSql += " ,''" 'INCENTIVE
                    strSql += " ,''" 'WEIGHTUNIT
                    strSql += " ,'" & McatCode & "'" 'CATCODE
                    strSql += " ,''" 'OCATCODE
                    strSql += " ,'" & DiffWt_Accode & "'" 'ACCODE
                    strSql += " ," & alloy & "" 'ALLOY
                    strSql += " ,'" & batchNo & "'" 'BATCHNO
                    strSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                    strSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                    strSql += " ,'" & userId & "'" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,0" 'DISCOUNT
                    strSql += " ,''" 'RUNNO
                    strSql += " ,''" 'CASHID
                    strSql += " ,0" 'TAX
                    strSql += " ,0" 'STONEAMT
                    strSql += " ,0" 'MISCAMT
                    strSql += " ,'" & MMetalid & "'" 'METALID
                    strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
                strSql = ""
            Next
            strSql = ""
            strSql = " INSERT INTO " & cnStockDb & "..MELTINGDETAIL"
            strSql += " (TRANNO,TRANDATE,RECISS,TRANTYPE,ACCODE,CATCODE,METALID,PCS,GRSWT,NETWT,RATE,AMOUNT"
            strSql += " ,BAGNO,BATCHNO,APPVER,COMPANYID,USERID,UPDATED"
            strSql += " ,UPTIME,CANCEL,DIFF_WT,BAGWT)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " " & tranNo & "" 'TRANNO
            strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'trandate
            strSql += " ,'I','MI'" 'RECISS,TRANTYPE
            strSql += " ,'" & mAccode & "'" 'accode
            strSql += " ,'" & catCode & "'" 'CATCODE.
            strSql += " ,'" & Metalid & "'" 'METAL.
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS    
            strSql += " ," & Val(.Cells("WEIGHT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += " ," & Val(.Cells("VALUE").Value.ToString) & "" 'AMOUNT
            strSql += " ,'" & .Cells("BAGNO").Value.ToString & "'" 'BAGNO
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,''" 'CANCEL
            strSql += " ," & Val(.Cells("DIFF_WT").Value.ToString) & "" 'AMOUNT
            strSql += " ," & Val(.Cells("WEIGHT").Value.ToString) & "" 'BAGWT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End With
    End Sub

    Private Sub CallGrid()
        strSql = " SELECT BAGNO,TRANDATE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATEGORY"
        strSql += " , GRSWT,NETWT,LESSWT,RATE,AMOUNT VALUE,BATCHNO"
        strSql += " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += " WHERE ISNULL(BAGNO,'') <> '' AND TRANTYPE = 'IIS' AND ISNULL(CANCEL,'') = ''"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridViewOpen.DataSource = dt
        If gridViewOpen.RowCount > 0 Then
            With gridViewOpen
                .Columns("bATCHNO").Visible = False
                .Columns("BAGNO").Width = 100
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("CATEGORY").Width = 250
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not btnOpen.Enabled Then Exit Sub
        CallGrid()
        If gridViewOpen.RowCount > 0 Then
            tabMain.SelectedTab = tabView
            gridViewOpen.Select()
        Else
            MsgBox("There is no record", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnBAck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBAck.Click
        If tabMain.SelectedTab.Name = tabView.Name Then
            If cancelFlag Then
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If
            tabMain.SelectedTab = tabGeneral
            dtpDate.Select()
        End If
    End Sub

    Private Sub btnSave_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.EnabledChanged
        btnOpen.Enabled = Not btnSave.Enabled
    End Sub

    Private Sub gridViewOpen_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewOpen.KeyPress
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
        If UCase(e.KeyChar) = "C" Then
            strSql = "SELECT 'CHECK' FROM " & cnStockDb & "..MELTINGDETAIL "
            strSql += " WHERE ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
            strSql += " AND RECISS = 'R' AND ISNULL(CANCEL,'') = '' AND TRANTYPE='MR'"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Melting Receipt made against this bagno." & vbCrLf & "Cannot cancel this entry.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MessageBox.Show("Do you want to cancel this entry", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    strSql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL = 'Y' "
                    strSql += " WHERE TRANDATE = '" & gridViewOpen.CurrentRow.Cells("TRANDATE").Value & "' "
                    strSql += " AND ISNULL(BATCHNO,'') = '" & gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " AND ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                    strSql = " UPDATE " & cnStockDb & "..MELTINGDETAIL SET CANCEL = 'Y' "
                    strSql += " where  ISNULL(BATCHNO,'') = '" & gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " and ISNULL(BAGNO,'') = '" & gridViewOpen.CurrentRow.Cells("BAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    tran.Commit()
                    tran = Nothing
                    cancelFlag = True
                    MsgBox("Successfully Canceled.", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs)
                    btnOpen_Click(Me, New EventArgs)
                Catch ex As Exception
                    If tran IsNot Nothing Then tran.Rollback()
                    MsgBox("Message: " + ex.Message + vbCrLf + ex.StackTrace)
                End Try
            End If
        End If
    End Sub

    Private Sub txtWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_WET.TextChanged
        If Not gridViewPendingBagNo.RowCount > 0 Then Exit Sub
        If ManualBag = True Then
            Dim netwt As Double
            netwt = Val(txtWeight_WET.Text) - Val(txtLessStnWt_WET.Text)
            txtNetWt_WET.Text = Format(netwt, "0.000")
        Else
            If Val(txtWeight_WET.Text) > Val(gridViewPendingBagNo.CurrentRow.Cells("GRSWT").Value.ToString) Then Exit Sub
        End If
    End Sub

    Private Sub txtLessStnWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessStnWt_WET.TextChanged
        If ManualBag = True Then
            Dim netwt As Double
            netwt = Val(txtWeight_WET.Text) - Val(txtLessStnWt_WET.Text)
            txtNetWt_WET.Text = Format(netwt, "0.000")
        Else
            Dim netwt As Double
            netwt = Val(txtWeight_WET.Text) - Val(txtLessStnWt_WET.Text)
            txtNetWt_WET.Text = Format(netwt, "0.000")
        End If
    End Sub
    Private Sub txtNetWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_WET.GotFocus
        If ManualBag = True Then
            Dim netwt As Double
            netwt = Val(txtWeight_WET.Text) - Val(txtLessStnWt_WET.Text)
            txtNetWt_WET.Text = Format(netwt, "0.000")
        End If
    End Sub
    Private Sub cmbSmith_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSmith_OWN.Leave
        If flagupdate = True Then
            Exit Sub
        End If
        If gridViewPendingBagNo.Rows.Count > 0 Then
            gridViewPendingBagNo.Visible = True
            gridViewPendingBagNo.Select()
        Else
            If MessageBox.Show("There is no bag(s) to be Issue. " & vbCrLf & "Do you want enter manual weight?", "Bagno Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                txtBagNo.Text = GetBagno()
                ManualBag = True
                Exit Sub
            End If
            dtpDate.Focus()
        End If
    End Sub
    Private Sub CmbCompany_MAN_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbCompany_MAN.SelectedValueChanged
        loadGrid()
    End Sub

    Private Function filterQry(ByVal _date As String, ByVal _batchno As String) As String
        Dim Qry As String = ""
        Qry = ""
        Qry += vbCrLf + " I.TRANDATE = '" & _date & "' AND I.TRANTYPE = 'IIS' "
        Qry += vbCrLf + " AND ISNULL(I.CANCEL,'') = '' AND ISNULL(I.BAGNO,'') <> ''"
        Qry += vbCrLf + " AND I.BATCHNO = '" & _batchno & "'"
        Return Qry
    End Function
    Private Sub gridViewOpen_KeyDown(sender As Object, e As KeyEventArgs) Handles gridViewOpen.KeyDown
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            strSql = vbCrLf + " SELECT I.TRANDATE,I.TRANNO,I.BATCHNO"
            strSql += vbCrLf + " ,(SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = I.COMPANYID) COMPANYNAME"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) SMITHNAME"
            strSql += vbCrLf + " ,I.BAGNO"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) CATEGORYNAME"
            strSql += vbCrLf + " ,PURITY"
            strSql += vbCrLf + " ,I.RATE,I.BOARDRATE"
            strSql += vbCrLf + " ,SUM(M.PCS) PCS,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT)LESSWT"
            strSql += vbCrLf + " ,I.REMARK1,I.REMARK2, COUNT(*) CNT "
            strSql += vbCrLf + " ,SUM(ISNULL(M.DIFF_WT,0)) DIFF_WT"
            strSql += vbCrLf + " ,(SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = I.COMPANYID) COMPANYNAME"
            strSql += vbCrLf + " ,I.METALID"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=I.METALID) METALNAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  AS I"
            strSql += vbCrLf + " ," & cnStockDb & "..MELTINGDETAIL AS M "
            strSql += vbCrLf + " WHERE I.BATCHNO = M.BATCHNO AND "
            strSql += vbCrLf + " " & filterQry(Format(gridViewOpen.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), gridViewOpen.CurrentRow.Cells("BATCHNO").Value) & ""
            strSql += vbCrLf + " GROUP BY I.TRANDATE,I.TRANNO "
            strSql += vbCrLf + " ,I.BATCHNO,I.COMPANYID,I.ACCODE "
            strSql += vbCrLf + " ,I.BAGNO,I.CATCODE,PURITY,I.RATE,I.BOARDRATE,M.PCS,I.GRSWT,I.NETWT,I.LESSWT "
            strSql += vbCrLf + " ,I.REMARK1,I.REMARK2,I.COMPANYID,ISNULL(M.DIFF_WT,0),I.METALID "
            strSql += vbCrLf + " ORDER BY I.GRSWT DESC,I.NETWT DESC,I.LESSWT DESC"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'Val(dt.Rows(0).Item("CNT").ToString)
                If Val(dt.Compute("SUM(CNT)", "").ToString) = 1 Then
                    CmbCompany_MAN.Text = dt.Rows(0).Item("COMPANYNAME").ToString
                    dtpDate.Value = dt.Rows(0).Item("TRANDATE")
                    lblTranNo.Text = dt.Rows(0).Item("TRANNO").ToString
                    CmbCompany_MAN.Text = dt.Rows(0).Item("COMPANYNAME").ToString
                    cmbSmith_OWN.Text = dt.Rows(0).Item("SMITHNAME").ToString
                    txtBagNo.Text = dt.Rows(0).Item("BAGNO").ToString
                    CmbCategory_OWN.Text = dt.Rows(0).Item("CATEGORYNAME").ToString
                    txtPurity_PER.Text = dt.Rows(0).Item("PURITY").ToString
                    txtRate_AMT.Text = dt.Rows(0).Item("RATE").ToString
                    txtPcs_NUM.Text = Val(dt.Compute("SUM(PCS)", "").ToString) 'dt.Rows(0).Item("PCS").ToString
                    'txtWeight_WET.Text = dt.Rows(0).Item("GRSWT").ToString
                    'txtLessStnWt_WET.Text = dt.Rows(0).Item("LESSWT").ToString
                    'txtNetWt_WET.Text = dt.Rows(0).Item("NETWT").ToString
                    txtWeight_WET.Text = Val(dt.Compute("SUM(GRSWT)", "").ToString) + Val(dt.Compute("SUM(DIFF_WT)", "").ToString) 'Val(dt.Rows(0).Item("GRSWT").ToString) + Val(dt.Rows(0).Item("DIFF_WT").ToString)
                    If Val(dt.Compute("SUM(LESSWT)", "").ToString) > 0 Then
                        txtLessStnWt_WET.Text = Val(dt.Compute("SUM(LESSWT)", "").ToString)  'Val(dt.Rows(0).Item("LESSWT").ToString)
                    Else
                        txtLessStnWt_WET.Text = Val(dt.Compute("SUM(LESSWT)", "").ToString) + Val(dt.Compute("SUM(DIFF_WT)", "").ToString)
                    End If
                    txtNetWt_WET.Text = Val(dt.Compute("SUM(NETWT)", "").ToString) + Val(dt.Compute("SUM(DIFF_WT)", "").ToString) 'Val(dt.Rows(0).Item("NETWT").ToString)
                    txtDiffWeight_WET.Text = Val(dt.Compute("SUM(DIFF_WT)", "").ToString) 'Val(dt.Rows(0).Item("DIFF_WT").ToString)
                    txtIssueWeight_WET.Text = Val(dt.Compute("SUM(GRSWT)", "").ToString) 'Val(dt.Rows(0).Item("GRSWT").ToString)
                    txtIssueLessStnWt_WET.Text = Val(dt.Compute("SUM(LESSWT)", "").ToString) 'Val(dt.Rows(0).Item("LESSWT").ToString)
                    txtIssueNetWt_WET.Text = Val(dt.Compute("SUM(NETWT)", "").ToString) 'Val(dt.Rows(0).Item("NETWT").ToString)
                    txtRemark1.Text = dt.Rows(0).Item("REMARK1").ToString.Trim
                    txtRemark2.Text = dt.Rows(0).Item("REMARK2").ToString.Trim
                    tabMain.SelectedTab = tabGeneral
                    flagupdateBatchno = gridViewOpen.CurrentRow.Cells("BATCHNO").Value.ToString
                    flagupdateTranno = dt.Rows(0).Item("TRANNO").ToString.Trim

                    If dtPendingBagNoCat.Rows.Count = 0 Then
                        dtPendingBagNoCat.Clear()
                        Dim dr As DataRow = Nothing
                        dr = dtPendingBagNoCat.NewRow
                        dr!BAGNO = dt.Rows(0).Item("BAGNO").ToString.Trim
                        dr!METAL = dt.Rows(0).Item("METALNAME").ToString.Trim
                        dr!CATEGORY = dt.Rows(0).Item("CATEGORYNAME").ToString.Trim
                        dr!GRSWT = Val(dt.Compute("SUM(GRSWT)", "").ToString)
                        dr!NETWT = Val(dt.Compute("SUM(NETWT)", "").ToString)
                        dr!LESSWT = Val(dt.Compute("SUM(LESSWT)", "").ToString)
                        dtPendingBagNoCat.Rows.Add(dr)
                    End If

                    dtpDate.Enabled = False
                    flagupdate = True
                Else
                    MsgBox("more than one cann't edit ", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewOpen.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MELTING ISSUE", gridViewOpen, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridViewOpen.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MELTING ISSUE", gridViewOpen, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub txtDiffWeight_WET_TextChanged(sender As Object, e As EventArgs) Handles txtDiffWeight_WET.TextChanged
        calcDiffWeight()
    End Sub
    Private Sub txtDiffWeight_WET_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDiffWeight_WET.KeyDown
        If e.KeyCode = Keys.Enter Then
            calcDiffWeight()
        End If
    End Sub
    Private Sub txtDiffWeight_WET_Leave(sender As Object, e As EventArgs) Handles txtDiffWeight_WET.Leave
        calcDiffWeight()
    End Sub

    Private Sub chkPurchase_CheckedChanged(sender As Object, e As EventArgs) Handles chkPurchase.CheckedChanged
        dtpBillDate.Enabled = chkPurchase.Checked
        ''CmbTrantype.Enabled = chkPurchase.Checked
    End Sub

    Private Sub CmbCompany_MAN_LostFocus(sender As Object, e As EventArgs) Handles CmbCompany_MAN.LostFocus
        If chkPurchase.Checked Or CmbTrantype.Text <> "" Then
            loadGrid()
        End If
    End Sub

    Private Sub CmbTrantype_LostFocus(sender As Object, e As EventArgs) Handles CmbTrantype.LostFocus
        If chkPurchase.Checked Or CmbTrantype.Text <> "" Then
            loadGrid()
        End If
    End Sub

    Private Sub txtNetWt_WET_TextChanged(sender As Object, e As EventArgs) Handles txtNetWt_WET.TextChanged
        calcDiffWeight()
    End Sub
End Class