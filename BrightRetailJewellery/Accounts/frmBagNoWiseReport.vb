Imports System.Data.OleDb
Public Class frmBagNoWiseReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable

    Private Sub frmBagNoWiseSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBagNoWiseSummary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT CATNAME FROM ("
        strSql += " SELECT 'ALL' CATNAME,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += " SELECT CATNAME,2 RESULT FROM " & cnAdminDb & "..CATEGORY)X ORDER BY RESULT,CATNAME"
        objGPack.FillCombo(strSql, cmbCatName, False, False)
        cmbCatName.Text = "ALL"
        CmbTrantype.Items.Clear()
        CmbTrantype.Items.Add("ALL")
        CmbTrantype.Items.Add("MISC ISSUE")
        CmbTrantype.Items.Add("PARTLY SALE")
        CmbTrantype.Items.Add("PURCHASE")
        CmbTrantype.Items.Add("SALES RETURN")
        CmbTrantype.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim StrTrantype As String = "ALL"
        If CmbTrantype.Text <> "ALL" Then
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

        If ChkOnlyPendingBagno.Checked Then
            loadGrid()
            Exit Sub
        Else
            strSql = " SELECT "
            strSql += vbCrLf + " TRANDATE,BAGNO"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
            strSql += vbCrLf + " ,SUM(ISNULL(GRSWT,0))GRSWT,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = R.COSTID)AS COSTNAME,'PURCHASE' TYPE,'SUMMARY' TABLENAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If StrTrantype <> "ALL" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'PU'"
            Else
                strSql += vbCrLf + " AND TRANTYPE = 'PU'"
            End If
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " GROUP BY TRANDATE,BAGNO,CATCODE,COSTID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " TRANDATE,BAGNO"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
            strSql += vbCrLf + " ,SUM(ISNULL(GRSWT,0))GRSWT,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = R.COSTID)AS COSTNAME,'SALES RETURN' TYPE,'SUMMARY' TABLENAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If StrTrantype <> "ALL" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'SR'"
            Else
                strSql += vbCrLf + " AND TRANTYPE = 'SR'"
            End If
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " GROUP BY TRANDATE,BAGNO,CATCODE,COSTID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " TRANDATE,BAGNO"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
            strSql += vbCrLf + " ,SUM(ISNULL(GRSWT,0))GRSWT,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = R.COSTID)AS COSTNAME,'MISC ISSUE' TYPE,'SUMMARY' TABLENAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If StrTrantype <> "ALL" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'MI'"
            Else
                strSql += vbCrLf + " AND TRANTYPE = 'MI'"
            End If
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " GROUP BY TRANDATE,BAGNO,CATCODE,COSTID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " TRANDATE,BAGNO"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
            strSql += vbCrLf + " ,SUM(ISNULL(TAGGRSWT-GRSWT,0))GRSWT,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = R.COSTID)AS COSTNAME,'PARTLY SALE' TYPE,'SUMMARY' TABLENAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If StrTrantype <> "ALL" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'SA'"
            Else
                strSql += vbCrLf + " AND TRANTYPE = 'SA'"
            End If
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " GROUP BY TRANDATE,BAGNO,CATCODE,COSTID"
        End If
        Dim dtGrid As New DataTable("SUMMARY")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Dim dr As DataRow
        dr = dtGrid.NewRow
        dr("CATEGORY") = "TOTAL"
        dr("GRSWT") = dtGrid.Compute("SUM(GRSWT)", "")
        dr("AMOUNT") = dtGrid.Compute("SUM(AMOUNT)", "")
        dtGrid.Rows.Add(dr)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text
        Dim tit As String = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text + vbCrLf
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.lblStatus.Text = "<Press [Alt+D] for Detail View>"
        DataGridView_SummaryFormatting(objGridShower.gridView)
    End Sub

    Private Function loadGrid()
        Dim dtgrid As New DataTable
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
        Dim IssOnly As Boolean = IIf(GetAdmindbSoftValue("MELTING_ISSONLY", "N", ) = "Y", True, False)
        If IssOnly = True Then
            strSql = vbCrLf + " SELECT TRANDATE,BAGNO,METAL,CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE ='MI' THEN 'MISC ISSUE' WHEN TRANTYPE ='SA' THEN 'PARTLY SALE' WHEN TRANTYPE ='SR' THEN 'SALES RETURN' "
            strSql += vbCrLf + " WHEN TRANTYPE ='PU' THEN 'PURCHASE' ELSE '' END TYPE"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO,TRANDATE,TRANTYPE"
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " AND BAGNO NOT IN (SELECT BAGNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> '' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S' "
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " )"
            'strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "')"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            If StrTrantype <> "" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M' "
            strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            If StrTrantype <> "" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            strSql += vbCrLf + " GROUP BY BAGNO,CATEGORY,METAL,TRANDATE,TRANTYPE"
        Else
            strSql = ""
            strSql = vbCrLf + " SELECT TRANDATE,BAGNO,METAL,CATEGORY,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE ='MI' THEN 'MISC ISSUE' WHEN TRANTYPE ='SA' THEN 'PARTLY SALE' WHEN TRANTYPE ='SR' THEN 'SALES RETURN' "
            strSql += vbCrLf + " WHEN TRANTYPE ='PU' THEN 'PURCHASE' ELSE '' END TYPE"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
NEXTT:
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO,TRANDATE,TRANTYPE"
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
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM ("
            strSql += vbCrLf + " (SELECT TOP 1 TRANDATE FROM " & CURRDB & "..ISSUE WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M' UNION "
            strSql += vbCrLf + " SELECT TOP 1 TRANDATE FROM " & CURRDB & "..RECEIPT WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M') "
            strSql += vbCrLf + " )X) TRANDATE "
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANTYPE FROM ("
            strSql += vbCrLf + " (SELECT TOP 1 TRANTYPE FROM " & CURRDB & "..ISSUE WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M' UNION "
            strSql += vbCrLf + " SELECT TOP 1 TRANTYPE FROM " & CURRDB & "..RECEIPT WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M') "
            strSql += vbCrLf + " )X) TRANTYPE "
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (sELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,0 LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..OPENWEIGHT R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <> '0'"
            strSql += vbCrLf + " AND ISNULL(STOCKTYPE,'') = 'C'"
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO,TRANDATE,TRANTYPE"
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
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'R' SEP,BAGNO,TRANDATE,TRANTYPE"
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
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'I' SEP,BAGNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM ("
            strSql += vbCrLf + " (SELECT TOP 1 TRANDATE FROM " & CURRDB & "..ISSUE WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M' UNION "
            strSql += vbCrLf + " SELECT TOP 1 TRANDATE FROM " & CURRDB & "..RECEIPT WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M') "
            strSql += vbCrLf + " )X) TRANDATE "
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANTYPE FROM ("
            strSql += vbCrLf + " (SELECT TOP 1 TRANTYPE FROM " & CURRDB & "..ISSUE WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M' UNION "
            strSql += vbCrLf + " SELECT TOP 1 TRANTYPE FROM " & CURRDB & "..RECEIPT WHERE BAGNO =R.BAGNO AND MELT_RETAG ='M') "
            strSql += vbCrLf + " )X) TRANTYPE "
            strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))aS METAL"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)aS CATEGORY"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT,BAGNO+CATCODE AS GRPBAG"
            strSql += vbCrLf + " ,RATE "
            strSql += vbCrLf + " FROM " & CURRDB & "..ISSUE R"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') <> ''"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CostCentre = False Then strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'IIS'"
            strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            If cmbCatName.Text <> "" And cmbCatName.Text <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & cmbCatName.Text & "' )"
            End If
            If ISPREVYRDB Then
                CURRDB = PREVYRDB
                strSql += vbCrLf + " UNION ALL"
                ISPREVYRDB = False
                GoTo NEXTT
            End If
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') IN(SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            If StrTrantype <> "" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M' "
            strSql += vbCrLf + " UNION SELECT DISTINCT BAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            If StrTrantype <> "" Then
                strSql += vbCrLf + " AND TRANTYPE = '" & StrTrantype.ToString & "' "
            End If
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND COSTID IN"
                strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' AND MELT_RETAG ='M') "
            strSql += vbCrLf + " GROUP BY BAGNO,CATEGORY,METAL,TRANDATE,TRANTYPE"
            strSql += vbCrLf + " having SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0  "
            strSql += vbCrLf + " ORDER BY TRANDATE,CATEGORY,BAGNO"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        dtgrid = New DataTable
        da.Fill(dtgrid)
        dtgrid.AcceptChanges()
        If Not dtgrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "BAGNO WISE PENDING REPORT FOR " + cmbMetal.Text
        Dim tit As String = "BAGNO WISE PENDING REPORT FOR " + cmbMetal.Text + vbCrLf
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtgrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.lblStatus.Text = ""
        DataGridView_SummaryFormattingPend(objGridShower.gridView)
    End Function

    Private Sub DataGridView_SummaryFormattingPend(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BAGNO").Width = 120
            .Columns("CATEGORY").Width = 350
            .Columns("GRSWT").Width = 120
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 120
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 120
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

#Region "GridDiaShower Events"

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If ChkOnlyPendingBagno.Checked Then Exit Sub
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "DETAILED" Then
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.FormReSize = False
                f.ResizeToolStripMenuItem.Checked = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                DataGridView_SummaryFormatting(f.gridView)
                Dim tit As String = "BAGNO WISE SUMMARY FOR " + cmbMetal.Text + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press [Alt+D] for Detail View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            End If
        ElseIf e.Alt And UCase(e.KeyCode) = UCase(Keys.D) Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow.Cells("BAGNO").Value.ToString = "" Then Exit Sub
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                Dim bagNo As String = dgv.CurrentRow.Cells("BAGNO").Value.ToString
                Dim strTrantypeName As String = dgv.CurrentRow.Cells("TYPE").Value.ToString
                Dim strCatName As String = dgv.CurrentRow.Cells("CATEGORY").Value.ToString
                Dim strTrantype As String = ""
                Dim strTblname As String = ""
                If strTrantypeName.ToString = "MISC ISSUE" Then
                    strTrantype = "MI"
                    strTblname = "ISSUE"
                ElseIf strTrantypeName.ToString = "PARTLY SALE" Then
                    strTrantype = "SA"
                    strTblname = "ISSUE"
                ElseIf strTrantypeName.ToString = "PURCHASE" Then
                    strTrantype = "PU"
                    strTblname = "RECEIPT"
                ElseIf strTrantypeName.ToString = "SALES RETURN" Then
                    strTrantype = "SR"
                    strTblname = "RECEIPT"
                End If

                Dim dt As DataTable = DetailView(bagNo, strTrantype, strTblname, strCatName)
                Dim dr As DataRow
                dr = dt.NewRow
                dr("CATEGORY") = "TOTAL"
                dr("GRSWT") = dt.Compute("SUM(GRSWT)", "")
                dr("NETWT") = dt.Compute("SUM(NETWT)", "")
                dr("AMOUNT") = dt.Compute("SUM(AMOUNT)", "")
                dt.Rows.Add(dr)
                dt.AcceptChanges()
                If Not dt.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                f.dsGrid.Tables.Add(dt)
                f.FormReSize = False
                f.ResizeToolStripMenuItem.Checked = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
                DataGridView_DetailViewFormatting(f.gridView)
                Dim tit As String = "DETAIL VIEW FOR BAGNO : " + bagNo + vbCrLf
                tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                f.lblTitle.Text = tit
                f.lblStatus.Text = "<Press ESC for Summary View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
            End If
        End If
    End Sub

    Private Function DetailView(ByVal bagNo As String, ByVal trantype As String, ByVal tblname As String, ByVal Catname As String) As DataTable
        strSql = " SELECT TRANNO,TRANDATE,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATEGORY"
        strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += " ,BATCHNO,'DETAILED' TABLENAME,BAGNO"
        strSql += " FROM " & cnStockDb & ".." & tblname & " AS R"
        strSql += " WHERE ISNULL(BAGNO,'') = '" & bagNo & "'"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += " AND TRANTYPE = '" & trantype & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If Catname <> "" Then
            strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & Catname.ToString & "')"
        End If
        strSql += " GROUP BY TRANNO,TRANDATE,BATCHNO,BAGNO,CATCODE"
        Dim dtGrid As New DataTable("DETAILED")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function

    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BATCHNO").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("BAGNO").Visible = False
            .Columns("TRANNO").Width = 80
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("AMOUNT").Width = 100
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("TABLENAME").Visible = False
            .Columns("BAGNO").Width = 120
            .Columns("CATEGORY").Width = 350
            .Columns("GRSWT").Width = 120
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 120
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMetal.SelectedIndexChanged
        If cmbMetal.Text <> "ALL" Then
            strSql = " SELECT CATNAME FROM ("
            strSql += " SELECT 'ALL' CATNAME,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += " SELECT CATNAME,2 RESULT FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID =(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "' )"
            strSql += " )X ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCatName, False, False)
            cmbCatName.Text = "ALL"
        Else
            strSql = " SELECT CATNAME FROM ("
            strSql += " SELECT 'ALL' CATNAME,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += " SELECT CATNAME,2 RESULT FROM " & cnAdminDb & "..CATEGORY)X ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCatName, False, False)
            cmbCatName.Text = "ALL"
        End If
    End Sub
#End Region
End Class