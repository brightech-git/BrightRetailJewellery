Imports System.Data.OleDb
Public Class frmApprovalSummary
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As New frmGridDispDia
    Dim dtCostCentre As New DataTable
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False
        objGridShower = New frmGridDispDia
        'lblTitle.Visible = False
        'objGridShower.gridView.DataSource = Nothing
        Me.Refresh()
        Try
            'strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemid & "APPROVALSUM')>0 DROP TABLE TEMP" & systemid & "APPROVALSUM"
            'strSql += " SELECT PARTICULAR"
            'strSql += " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
            'strSql += " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END)AS OGRSWT"
            'strSql += " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
            'strSql += " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END)AS IGRSWT"
            'strSql += " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
            'strSql += " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END)AS RGRSWT"
            'strSql += " ,SUM(CASE WHEN SEP = 'REC' THEN -1*PCS ELSE PCS END) AS CPCS"
            'strSql += " ,SUM(CASE WHEN SEP = 'REC' THEN -1*GRSWT ELSE GRSWT END)AS CGRSWT"
            'strSql += " ,ITEM,1 RESULT,' 'COLHEAD"
            'If rbtCounter.Checked Then
            '    strSql += " ,COUNTER"
            'ElseIf rbtEmployee.Checked Then
            '    strSql += " ,EMPLOYEE"
            'End If
            'strSql += " INTO TEMP" & systemId & "APPROVALSUM"
            'strSql += " FROM"
            'strSql += " ("
            'strSql += " SELECT 'OPE' SEP"
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS PARTICULAR"
            'strSql += " ,PCS,GRSWT "
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & CNADMINDB & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER"
            'strSql += " ,(SELECT EMPNAME FROM " & CNADMINDB & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPLOYEE"
            'strSql += " FROM " & CNSTOCKDB & "..ISSUE I"
            'strSql += " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND TRANTYPE = 'AI' AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "'"
            'If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & CNADMINDB & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            'strSql += " UNION ALL"
            'strSql += " SELECT 'OPE' SEP"
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS PARTICULAR "
            'strSql += " ,-1*PCS PCS,-1*GRSWT GRSWT"
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & CNADMINDB & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER"
            'strSql += " ,(SELECT EMPNAME FROM " & CNADMINDB & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPLOYEE"
            'strSql += " FROM " & CNSTOCKDB & "..RECEIPT I"
            'strSql += " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND TRANTYPE = 'AR' AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "'"
            'If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & CNADMINDB & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            'strSql += " UNION ALL"
            'strSql += " SELECT 'ISS' SEP"
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS PARTICULAR"
            'strSql += " ,PCS,GRSWT "
            'strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & CNADMINDB & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER"
            'strSql += " ,(SELECT EMPNAME FROM " & CNADMINDB & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPLOYEE"
            'strSql += " FROM " & cnStockDb & "..ISSUE I"
            'strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND TRANTYPE = 'AI' AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "'"
            'If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            'strSql += " UNION ALL"
            'strSql += " SELECT 'REC' SEP"
            'strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS PARTICULAR"
            'strSql += " ,PCS,GRSWT "
            'strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER"
            'strSql += " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPLOYEE"
            'strSql += " FROM " & cnStockDb & "..RECEIPT I"
            'strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += " AND TRANTYPE = 'AR' AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "'"
            'If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            'strSql += " )X GROUP BY ITEM,PARTICULAR"
            'If rbtCounter.Checked Then
            '    strSql += " ,COUNTER"
            'ElseIf rbtEmployee.Checked Then
            '    strSql += " ,EMPLOYEE"
            'End If
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            'If rbtCounter.Checked Or rbtEmployee.Checked Then
            '    Dim grpName As String = ""
            '    If rbtCounter.Checked Then grpName = "COUNTER" Else grpName = "EMPLOYEE"
            '    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "APPROVALSUM)>0"
            '    strSql += " BEGIN"
            '    strSql += " INSERT INTO TEMP" & systemId & "APPROVALSUM(" & grpName & ",PARTICULAR,RESULT,COLHEAD)"
            '    strSql += " SELECT DISTINCT " & grpName & "," & grpName & ",0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "APPROVALSUM"
            '    strSql += " INSERT INTO TEMP" & systemId & "APPROVALSUM(" & grpName & ",PARTICULAR,OPCS,OGRSWT,IPCS,IGRSWT,RPCS,RGRSWT,CPCS,CGRSWT,RESULT,COLHEAD)"
            '    strSql += " SELECT " & grpName & ",'SUB TOTAL',SUM(ISNULL(OPCS,0)),SUM(ISNULL(OGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(IPCS,0)),SUM(ISNULL(IGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(RPCS,0)),SUM(ISNULL(RGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(CPCS,0)),SUM(ISNULL(CGRSWT,0))"
            '    strSql += " ,2 RESULT,'S'COLHEAD FROM TEMP" & systemId & "APPROVALSUM GROUP BY " & grpName & ""
            '    strSql += " INSERT INTO TEMP" & systemId & "APPROVALSUM(" & grpName & ",PARTICULAR,OPCS,OGRSWT,IPCS,IGRSWT,RPCS,RGRSWT,CPCS,CGRSWT,RESULT,COLHEAD)"
            '    strSql += " SELECT 'ZZZZZ'" & grpName & ",'GRAND TOTAL',SUM(ISNULL(OPCS,0)),SUM(ISNULL(OGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(IPCS,0)),SUM(ISNULL(IGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(RPCS,0)),SUM(ISNULL(RGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(CPCS,0)),SUM(ISNULL(CGRSWT,0))"
            '    strSql += " ,3 RESULT,'G'COLHEAD FROM TEMP" & systemId & "APPROVALSUM WHERE RESULT = 2"
            '    strSql += " END"
            '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '    cmd.ExecuteNonQuery()
            'Else
            '    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "APPROVALSUM)>0"
            '    strSql += " BEGIN"
            '    strSql += " INSERT INTO TEMP" & systemId & "APPROVALSUM(ITEM,PARTICULAR,OPCS,OGRSWT,IPCS,IGRSWT,RPCS,RGRSWT,CPCS,CGRSWT,RESULT,COLHEAD)"
            '    strSql += " SELECT 'ZZZZZ'ITEM,'GRAND TOTAL',SUM(ISNULL(OPCS,0)),SUM(ISNULL(OGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(IPCS,0)),SUM(ISNULL(IGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(RPCS,0)),SUM(ISNULL(RGRSWT,0))"
            '    strSql += " ,SUM(ISNULL(CPCS,0)),SUM(ISNULL(CGRSWT,0))"
            '    strSql += " ,3 RESULT,'G'COLHEAD FROM TEMP" & systemId & "APPROVALSUM"
            '    strSql += " END"
            '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '    cmd.ExecuteNonQuery()
            'End If

            'strSql = " DELETE FROM TEMP" & systemId & "APPROVALSUM"
            'strSql += " WHERE RESULT = 1"
            'strSql += " AND ISNULL(OPCS,0) = 0 AND ISNULL(OGRSWT,0) = 0"
            'strSql += " AND ISNULL(IPCS,0) = 0 AND ISNULL(IGRSWT,0) = 0"
            'strSql += " AND ISNULL(RPCS,0) = 0 AND ISNULL(RGRSWT,0) = 0"
            'strSql += " AND ISNULL(CPCS,0) = 0 AND ISNULL(CGRSWT,0) = 0"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()


            'strSql = " UPDATE TEMP" & systemId & "APPROVALSUM SET OPCS = NULL WHERE ISNULL(OPCS,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET OGRSWT = NULL WHERE ISNULL(OGRSWT,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET IPCS = NULL WHERE ISNULL(IPCS,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET IGRSWT = NULL WHERE ISNULL(IGRSWT,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET RPCS = NULL WHERE ISNULL(RPCS,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET RGRSWT = NULL WHERE ISNULL(RGRSWT,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET CPCS = NULL WHERE ISNULL(CPCS,0) = 0"
            'strSql += " UPDATE TEMP" & systemId & "APPROVALSUM SET CGRSWT = NULL WHERE ISNULL(CGRSWT,0) = 0"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()


            'strSql = " SELECT * FROM TEMP" & systemId & "APPROVALSUM"
            'strSql += " ORDER BY "
            'If rbtItem.Checked Then
            '    strSql += " ITEM"
            'ElseIf rbtCounter.Checked Then
            '    strSql += " COUNTER"
            'ElseIf rbtEmployee.Checked Then
            '    strSql += " EMPLOYEE"
            'End If
            'strSql += " ,RESULT"



            strSql = " EXEC " & cnStockDb & "..SP_RPT_APPROVALSUMMARY"
            'strSql += vbCrLf + " @FROMDATE = '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
            'strSql += vbCrLf + " ,@TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
            strSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
            If rbtItem.Checked Then
                strSql += vbCrLf + " ,@GROUPBY = 'I'"
            ElseIf rbtCounter.Checked Then
                strSql += vbCrLf + " ,@GROUPBY = 'C'"
            ElseIf rbtCustomer.Checked Then
                strSql += vbCrLf + " ,@GROUPBY = 'P'"
            ElseIf rbtEmployee.Checked Then
                strSql += vbCrLf + " ,@GROUPBY = 'E'"
            End If
            If chkDetailedView.Checked And Not rbtItem.Checked Then strSql += vbCrLf + " ,@DETAILED = 'Y'" Else strSql += vbCrLf + " ,@DETAILED = 'N'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            If cmbProduct.Text = "" Then
                strSql += vbCrLf + " ,@PRODUCT = 'ALL'"
            Else
                strSql += vbCrLf + " ,@PRODUCT = '" & cmbProduct.Text & "'"
            End If
            If cmbSubProduct.Text = "" Then
                strSql += vbCrLf + " ,@SUBPRODUCT = 'ALL'"
            Else
                strSql += vbCrLf + " ,@SUBPRODUCT = '" & cmbSubProduct.Text & "'"
            End If
            If cmbPurity.Text = "" Then
                strSql += vbCrLf + " ,@PURITY = 'ALL'"
            Else
                strSql += vbCrLf + " ,@PURITY = '" & cmbPurity.Text & "'"
            End If
            'If cmbSize.Text = "" Then
            '    strSql += vbCrLf + " ,@SIZE = 'ALL'"
            'Else
            '    strSql += vbCrLf + " ,@SIZE = '" & cmbSize.Text & "'"
            'End If
            If txtPartyname.Text = "" Then
                strSql += vbCrLf + " ,@SUPPLIER = 'ALL'"
            Else
                strSql += vbCrLf + " ,@SUPPLIER = '" & txtPartyname.Text & "'"
            End If
            strSql += vbCrLf + " ,@COSTID = '" & GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False) & "'"
            strSql += vbCrLf + " ,@PNAME = '" & txtPartyname.Text & "'"
            strSql += vbCrLf + " ,@WITHRUNNO = '" & IIf(chkWithRunNo.Checked, "Y", "N") & "'"
            'strSql += vbCrLf + " ,@WITHDIA = '" & IIf(chkWithDia.Checked, "Y", "N") & "'"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            If Not dtGrid.Rows.Count > 0 Then
                btnSearch.Enabled = True
                MsgBox("Record not found", MsgBoxStyle.Information)
                btnSearch.Select()
                Exit Sub
            End If
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            Dim tit As String = "APPROVAL SUMMARY "
            If chkAsOnDate.Checked Then
                tit += " AS ON " & dtpFrom.Text
            Else
                tit += dtpFrom.Text & " TO " & dtpTo.Text & " GROUP BY"
            End If

            If rbtItem.Checked Then
                tit += " ITEM WISE "
            ElseIf rbtCounter.Checked Then
                tit += " COUNTER WISE"
            ElseIf rbtCustomer.Checked Then
                tit += " CUSTOMER WISE"
            Else
                tit += " EMPLOYEE WISE"
            End If
            tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
            objGridShower.lblTitle.Text = tit
            objGridShower.lblTitle.Visible = True
            objGridShower.gridView.DataSource = dtGrid
            With objGridShower.gridView
                For cnt As Integer = 0 To objGridShower.gridView.ColumnCount - 1
                    objGridShower.gridView.Columns(cnt).Visible = False
                Next
                .Columns("PARTICULAR").Width = 250
                .Columns("OPCS").Width = 70
                .Columns("OGRSWT").Width = 90
                .Columns("ODIAPCS").Width = 70
                .Columns("ODIAWT").Width = 90
                .Columns("OAMOUNT").Width = 90
                .Columns("IPCS").Width = 70
                .Columns("IGRSWT").Width = 90
                .Columns("IDIAPCS").Width = 70
                .Columns("IDIAWT").Width = 90
                .Columns("IAMOUNT").Width = 90
                .Columns("RPCS").Width = 70
                .Columns("RGRSWT").Width = 90
                .Columns("RDIAPCS").Width = 70
                .Columns("RDIAWT").Width = 90
                .Columns("RAMOUNT").Width = 90
                .Columns("CPCS").Width = 70
                .Columns("CGRSWT").Width = 90
                .Columns("CDIAPCS").Width = 70
                .Columns("CDIAWT").Width = 90
                .Columns("CAMOUNT").Width = 90

                .Columns("OPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("OGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ODIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ODIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("OAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IDIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("IAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("RPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RDIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("CPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CDIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CDIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("OPCS").HeaderText = "PCS"
                .Columns("OGRSWT").HeaderText = "GRSWT"
                .Columns("ODIAPCS").HeaderText = "DIAPCS"
                .Columns("ODIAWT").HeaderText = "DIAWT"
                .Columns("OAMOUNT").HeaderText = "AMOUNT"

                .Columns("IPCS").HeaderText = "PCS"
                .Columns("IGRSWT").HeaderText = "GRSWT"
                .Columns("IDIAPCS").HeaderText = "DIAPCS"
                .Columns("IDIAWT").HeaderText = "DIAWT"
                .Columns("IAMOUNT").HeaderText = "AMOUNT"

                .Columns("RPCS").HeaderText = "PCS"
                .Columns("RGRSWT").HeaderText = "GRSWT"
                .Columns("RDIAPCS").HeaderText = "DIAPCS"
                .Columns("RDIAWT").HeaderText = "DIAWT"
                .Columns("RAMOUNT").HeaderText = "AMOUNT"

                .Columns("CPCS").HeaderText = "PCS"
                .Columns("CGRSWT").HeaderText = "GRSWT"
                .Columns("CDIAPCS").HeaderText = "DIAPCS"
                .Columns("CDIAWT").HeaderText = "DIAWT"
                .Columns("CAMOUNT").HeaderText = "AMOUNT"

                .Columns("PARTICULAR").Visible = True
                .Columns("OPCS").Visible = True
                .Columns("OGRSWT").Visible = True
                .Columns("ODIAPCS").Visible = chkWithDia.Checked
                .Columns("ODIAWT").Visible = chkWithDia.Checked
                .Columns("ODIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("OAMOUNT").Visible = chkwithval.Checked

                .Columns("IPCS").Visible = True
                .Columns("IGRSWT").Visible = True
                .Columns("IDIAPCS").Visible = chkWithDia.Checked
                .Columns("IDIAWT").Visible = chkWithDia.Checked
                .Columns("IDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("IAMOUNT").Visible = chkwithval.Checked

                .Columns("RPCS").Visible = True
                .Columns("RGRSWT").Visible = True
                .Columns("RDIAPCS").Visible = chkWithDia.Checked
                .Columns("RDIAWT").Visible = chkWithDia.Checked
                .Columns("RDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("RAMOUNT").Visible = chkwithval.Checked

                .Columns("CPCS").Visible = True
                .Columns("CGRSWT").Visible = True
                .Columns("CDIAPCS").Visible = chkWithDia.Checked
                .Columns("CDIAWT").Visible = chkWithDia.Checked
                .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("CAMOUNT").Visible = chkwithval.Checked
            End With
            FormatGridColumns(objGridShower.gridView, False, False, , False)

            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
            'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)


            Dim dtHead As New DataTable
            If chkWithDia.Checked Then
                strSql = "SELECT ''[PARTICULAR],''[OPCS~OGRSWT~ODIAPCS~ODIAWT~OAMOUNT],''[IPCS~IGRSWT~IDIAPCS~IDIAWT~IAMOUNT],''[RPCS~RGRSWT~RDIAPCS~RDIAWT~RAMOUNT],''[CPCS~CGRSWT~CDIAPCS~CDIAWT~CAMOUNT],''SCROLL"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtHead)
                objGridShower.gridViewHeader.Visible = True
                objGridShower.gridViewHeader.DataSource = dtHead
                objGridShower.gridViewHeader.Columns("PARTICULAR").HeaderText = ""
                objGridShower.gridViewHeader.Columns("OPCS~OGRSWT~ODIAPCS~ODIAWT~OAMOUNT").HeaderText = "OPENING"
                objGridShower.gridViewHeader.Columns("IPCS~IGRSWT~IDIAPCS~IDIAWT~IAMOUNT").HeaderText = "ISSUE"
                objGridShower.gridViewHeader.Columns("RPCS~RGRSWT~RDIAPCS~RDIAWT~RAMOUNT").HeaderText = "RECEIPT"
                objGridShower.gridViewHeader.Columns("CPCS~CGRSWT~CDIAPCS~CDIAWT~CAMOUNT").HeaderText = "CLOSING"
                objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
                objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                SetGridHeadColWid(objGridShower.gridViewHeader)
            Else
                strSql = "SELECT ''[PARTICULAR],''[OPCS~OGRSWT~OAMOUNT],''[IPCS~IGRSWT~IAMOUNT],''[RPCS~RGRSWT~RAMOUNT],''[CPCS~CGRSWT~CAMOUNT],''SCROLL"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtHead)
                objGridShower.gridViewHeader.Visible = True
                objGridShower.gridViewHeader.DataSource = dtHead
                objGridShower.gridViewHeader.Columns("PARTICULAR").HeaderText = ""
                objGridShower.gridViewHeader.Columns("OPCS~OGRSWT~OAMOUNT").HeaderText = "OPENING"
                objGridShower.gridViewHeader.Columns("IPCS~IGRSWT~IAMOUNT").HeaderText = "ISSUE"
                objGridShower.gridViewHeader.Columns("RPCS~RGRSWT~RAMOUNT").HeaderText = "RECEIPT"
                objGridShower.gridViewHeader.Columns("CPCS~CGRSWT~CAMOUNT").HeaderText = "CLOSING"
                objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
                objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                SetGridHeadColWid(objGridShower.gridViewHeader)
            End If

            objGridShower.Name = Me.Name
            objGridShower.Show()
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))
            Call SetGridHeadColWid(objGridShower.gridViewHeader)

        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnSearch.Enabled = True
            If objGridShower.gridView.RowCount > 0 Then objGridShower.gridView.Select() Else btnSearch.Select()
        End Try
        Prop_Sets()
    End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        If Not objGridShower.gridViewHeader.Visible Then Exit Sub
        If objGridShower.gridViewHeader Is Nothing Then Exit Sub
        If Not objGridShower.gridView.ColumnCount > 0 Then Exit Sub
        If Not objGridShower.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With objGridShower.gridViewHeader
            If chkWithDia.Checked Then
                .Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width
                .Columns("OPCS~OGRSWT~ODIAPCS~ODIAWT~OAMOUNT").Width = objGridShower.gridView.Columns("OPCS").Width + objGridShower.gridView.Columns("OGRSWT").Width + objGridShower.gridView.Columns("ODIAPCS").Width + objGridShower.gridView.Columns("ODIAWT").Width + objGridShower.gridView.Columns("OAMOUNT").Width
                .Columns("IPCS~IGRSWT~IDIAPCS~IDIAWT~IAMOUNT").Width = objGridShower.gridView.Columns("IPCS").Width + objGridShower.gridView.Columns("IGRSWT").Width + objGridShower.gridView.Columns("IDIAPCS").Width + objGridShower.gridView.Columns("IDIAWT").Width + objGridShower.gridView.Columns("IAMOUNT").Width
                .Columns("RPCS~RGRSWT~RDIAPCS~RDIAWT~RAMOUNT").Width = objGridShower.gridView.Columns("RPCS").Width + objGridShower.gridView.Columns("RGRSWT").Width + objGridShower.gridView.Columns("RDIAPCS").Width + objGridShower.gridView.Columns("RDIAWT").Width + objGridShower.gridView.Columns("RAMOUNT").Width
                .Columns("CPCS~CGRSWT~CDIAPCS~CDIAWT~CAMOUNT").Width = objGridShower.gridView.Columns("CPCS").Width + objGridShower.gridView.Columns("CGRSWT").Width + objGridShower.gridView.Columns("CDIAPCS").Width + objGridShower.gridView.Columns("CDIAWT").Width + objGridShower.gridView.Columns("CAMOUNT").Width
            Else
                .Columns("PARTICULAR").Width = objGridShower.gridView.Columns("PARTICULAR").Width
                .Columns("OPCS~OGRSWT~OAMOUNT").Width = objGridShower.gridView.Columns("OPCS").Width + objGridShower.gridView.Columns("OGRSWT").Width + objGridShower.gridView.Columns("OAMOUNT").Width
                .Columns("IPCS~IGRSWT~IAMOUNT").Width = objGridShower.gridView.Columns("IPCS").Width + objGridShower.gridView.Columns("IGRSWT").Width + objGridShower.gridView.Columns("IAMOUNT").Width
                .Columns("RPCS~RGRSWT~RAMOUNT").Width = objGridShower.gridView.Columns("RPCS").Width + objGridShower.gridView.Columns("RGRSWT").Width + objGridShower.gridView.Columns("RAMOUNT").Width
                .Columns("CPCS~CGRSWT~CAMOUNT").Width = objGridShower.gridView.Columns("CPCS").Width + objGridShower.gridView.Columns("CGRSWT").Width + objGridShower.gridView.Columns("CAMOUNT").Width
            End If
            
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To objGridShower.gridView.ColumnCount - 1
                If objGridShower.gridView.Columns(cnt).Visible Then colWid += objGridShower.gridView.Columns(cnt).Width
            Next
            If colWid >= objGridShower.gridView.Width Then
                objGridShower.gridViewHeader.Columns("SCROLL").Visible = CType(objGridShower.gridView.Controls(1), VScrollBar).Visible
                objGridShower.gridViewHeader.Columns("SCROLL").Width = CType(objGridShower.gridView.Controls(1), VScrollBar).Width
                objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                objGridShower.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGridShower.gridView.DataSource = Nothing
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' cmbMetal.Text = "ALL"
        'lblTitle.Text = ""
        'lblTitle.Visible = False
        'rbtItem.Checked = True
        'chkAsOnDate.Checked = True
        'chkDetailedView.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub frmApprovalSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmApprovalSummary_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objGridShower.gridView.RowTemplate.Height = 21
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  Order by ITEMNAME "
        objGPack.FillCombo(strSql, cmbProduct, False, False)

        strSql = " SELECT PURITYNAME FROM " & cnAdminDb & "..PURITYMAST "
        strSql += " ORDER BY PURITYNAME "
        objGPack.FillCombo(strSql, cmbPurity, False, False)


        'strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        'strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE GRPLEDGER NOT IN ('T','P'))"
        'strSql += " ORDER BY ACNAME"
        strSql = " SELECT DISTINCT PNAME AS NAME FROM " & cnAdminDb & "..PERSONALINFO "        strSql += " UNION ALL"        strSql += " SELECT DISTINCT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD"
        strSql += " ORDER BY NAME"
        objGPack.FillCombo(strSql, txtPartyname, False, False)


        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER  Order by DESIGNERNAME "
        objGPack.FillCombo(strSql, cmbSupplier, False, False)

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate

        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                objGridShower.gridViewHeader.HorizontalScrollingOffset = e.NewValue
                If objGridShower.gridViewHeader.Columns.Count > 0 Then
                    If objGridShower.gridViewHeader.Columns.Contains("SCROLL") Then
                        objGridShower.gridViewHeader.Columns("SCROLL").Visible = CType(objGridShower.gridView.Controls(1), VScrollBar).Visible
                        objGridShower.gridViewHeader.Columns("SCROLL").Width = CType(objGridShower.gridView.Controls(1), VScrollBar).Width
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(objGridShower.gridViewHeader)
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, objGridShower.lblTitle.Text, objGridShower.gridView, BrightPosting.GExport.GExportType.Print, objGridShower.gridViewHeader)
    End Sub
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, objGridShower.lblTitle.Text, objGridShower.gridView, BrightPosting.GExport.GExportType.Export, objGridShower.gridViewHeader)
    End Sub
    Private Sub rbtItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        chkDetailedView.Visible = Not rbtItem.Checked
    End Sub
    Private Sub Load_SubItem()
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN "
        strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST where ItemName='" & cmbProduct.Text & "') Order by SUBITEMNAME "
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        cmbSubProduct.Items.Clear()
        For Each ro As DataRow In dt.Rows
            cmbSubProduct.Items.Add(ro.Item("SUBITEMNAME").ToString)
        Next
    End Sub
    Private Sub Load_SIZENAME()
        'strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE "
        'If cmbProduct.Text <> "" Then
        '    strSql += " WHERE ITEMID IN "
        '    strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST where ItemName='" & cmbProduct.Text & "')  "
        'End If
        'strSql += " ORDER BY SIZENAME "
        'Dim dt As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'cmbSize.Items.Clear()
        'For Each ro As DataRow In dt.Rows
        '    cmbSize.Items.Add(ro.Item("SIZENAME").ToString)
        'Next
    End Sub
    Private Sub cmbSubProduct_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubProduct.GotFocus
        Call Load_SubItem()
    End Sub
    Private Sub cmbSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Call Load_SIZENAME()
    End Sub
    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As OnDate"
        Else
            chkAsOnDate.Text = "Date From"
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmApprovalSummary_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbProduct = cmbProduct.Text
        obj.p_cmbSubProduct = cmbSubProduct.Text
        obj.p_cmbPurity = cmbPurity.Text
        obj.p_cmbSupplier = cmbSupplier.Text
        obj.p_rbtItem = rbtItem.Checked
        obj.p_rbtCounter = rbtCounter.Checked
        obj.p_rbtEmployee = rbtEmployee.Checked
        obj.p_rbtCustomer = rbtCustomer.Checked
        obj.p_chkDetailedView = chkDetailedView.Checked
        obj.p_txtPartyname = txtPartyname.Text
        obj.p_chkWithRunNo = chkWithRunNo.Checked
        obj.p_chkWithDia = chkWithDia.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmApprovalSummary_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmApprovalSummary_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmApprovalSummary_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbProduct.Text = obj.p_cmbProduct
        cmbSubProduct.Text = obj.p_cmbSubProduct
        cmbPurity.Text = obj.p_cmbPurity
        cmbSupplier.Text = obj.p_cmbSupplier
        rbtItem.Checked = obj.p_rbtItem
        rbtCounter.Checked = obj.p_rbtCounter
        rbtEmployee.Checked = obj.p_rbtEmployee
        rbtCustomer.Checked = obj.p_rbtCustomer
        chkDetailedView.Checked = obj.p_chkDetailedView
        txtPartyname.Text = obj.p_txtPartyname
        chkWithRunNo.Checked = obj.p_chkWithRunNo
        chkWithDia.Checked = obj.p_chkWithDia

    End Sub

    Private Sub txtPartyname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPartyname.SelectedIndexChanged

    End Sub
End Class

Public Class frmApprovalSummary_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
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
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbProduct As String = ""
    Public Property p_cmbProduct() As String
        Get
            Return cmbProduct
        End Get
        Set(ByVal value As String)
            cmbProduct = value
        End Set
    End Property
    Private cmbSubProduct As String = ""
    Public Property p_cmbSubProduct() As String
        Get
            Return cmbSubProduct
        End Get
        Set(ByVal value As String)
            cmbSubProduct = value
        End Set
    End Property
    Private cmbPurity As String = ""
    Public Property p_cmbPurity() As String
        Get
            Return cmbPurity
        End Get
        Set(ByVal value As String)
            cmbPurity = value
        End Set
    End Property
    Private cmbSupplier As String = ""
    Public Property p_cmbSupplier() As String
        Get
            Return cmbSupplier
        End Get
        Set(ByVal value As String)
            cmbSupplier = value
        End Set
    End Property
    Private rbtItem As Boolean = True
    Public Property p_rbtItem() As Boolean
        Get
            Return rbtItem
        End Get
        Set(ByVal value As Boolean)
            rbtItem = value
        End Set
    End Property
    Private rbtCounter As Boolean = False
    Public Property p_rbtCounter() As Boolean
        Get
            Return rbtCounter
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
        End Set
    End Property
    Private rbtEmployee As Boolean = False
    Public Property p_rbtEmployee() As Boolean
        Get
            Return rbtEmployee
        End Get
        Set(ByVal value As Boolean)
            rbtEmployee = value
        End Set
    End Property
    Private rbtCustomer As Boolean = False
    Public Property p_rbtCustomer() As Boolean
        Get
            Return rbtCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtCustomer = value
        End Set
    End Property
    Private chkDetailedView As Boolean = False
    Public Property p_chkDetailedView() As Boolean
        Get
            Return chkDetailedView
        End Get
        Set(ByVal value As Boolean)
            chkDetailedView = value
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
    Private chkWithRunNo As Boolean = False
    Public Property p_chkWithRunNo() As Boolean
        Get
            Return chkWithRunNo
        End Get
        Set(ByVal value As Boolean)
            chkWithRunNo = value
        End Set
    End Property
    Private chkWithDia As Boolean = False
    Public Property p_chkWithDia() As Boolean
        Get
            Return chkWithDia
        End Get
        Set(ByVal value As Boolean)
            chkWithDia = value
        End Set
    End Property
End Class