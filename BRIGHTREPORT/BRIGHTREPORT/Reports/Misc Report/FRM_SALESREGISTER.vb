Imports System.Data.OleDb
Public Class FRM_SALESREGISTER
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub FRM_SALESREGISTER_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub FRM_SALESREGISTER_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY  WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtGrid As New DataTable
        Prop_Sets()
        strSql = ""
        strSql = vbCrLf + " IF OBJECT_ID('TEMPDB..#SA_REGISTER','U') IS NOT NULL DROP TABLE #SA_REGISTER"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "I.TRANDATE,I.TRANNO,CONVERT(VARCHAR(500),(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)))PARTICULAR,SUM(I.GRSWT)GRSWT,SUM(I.AMOUNT)AMOUNT"
        strSql += vbCrLf + ",X.SGST AS SGST,X.CGST AS CGST,X.IGST AS IGST,X.GST AS GST"
        strSql += vbCrLf + ",SUM(I.AMOUNT+I.TAX) AS NETAMOUNT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)CASH"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)CHEQUE"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[CR CARD]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SCHEME"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SCH_BONUS"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SCH_GIFT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SCH_PRIZE"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)DISCOUNT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)ADVANCE"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)DUE"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)OTHERS"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[SALES RETURN]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[GOLD PUR]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[SILVER PUR]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[PLATINUM PUR]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[DIAMOND PUR]"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)[STN PUR]"
        strSql += vbCrLf + ",DISPLAYORDER RESULT1	,2 RESULT2"
        strSql += vbCrLf + ",CA.CATNAME,I.BATCHNO,CA.METALID"
        strSql += vbCrLf + ",IDENTITY(INT,1,1)SNO"
        strSql += vbCrLf + "INTO #SA_REGISTER"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        strSql += vbCrLf + "INNER JOIN (SELECT  CATCODE,TRANNO, CGST, SGST, IGST,GST FROM(SELECT I.CATCODE,I.TRANNO, SUM(CASE WHEN TAXID = 'CG' THEN TAXAMOUNT END) AS CGST"
        strSql += vbCrLf + ",SUM(CASE WHEN TAXID = 'SG' THEN TAXAMOUNT END) AS SGST"
        strSql += vbCrLf + ",SUM(CASE WHEN TAXID = 'IG' THEN TAXAMOUNT END) AS IGST,SUM(TAXAMOUNT) AS GST FROM " & cnStockDb & "..TAXTRAN AS T, " & cnStockDb & "..ISSUE AS I WHERE I.BATCHNO = T.BATCHNO "
        strSql += vbCrLf + "AND T.ISSSNO = I.SNO AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + "AND I.TRANTYPE IN ('SA','OD','RD')"
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + "AND I.AMOUNT > 0 AND ISNULL(T.STUDDED,'') <> 'Y'"
        strSql += vbCrLf + "GROUP BY I.CATCODE,I.TRANNO)X)X ON X.CATCODE = I.CATCODE AND X.TRANNO= I.TRANNO"
        strSql += vbCrLf + "WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + "AND I.TRANTYPE IN ('SA','OD','RD')"
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + "AND I.AMOUNT > 0"
        strSql += vbCrLf + "GROUP BY I.TRANDATE,I.TRANNO,CA.CATNAME,I.BATCHNO,CA.METALID,X.SGST,X.CGST,X.IGST,X.GST,DISPLAYORDER"
        strSql += vbCrLf + "ORDER BY CA.CATNAME,I.TRANDATE,I.TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF OBJECT_ID('TEMPDB..#PU_REGISTER','U') IS NOT NULL DROP TABLE #PU_REGISTER"
        strSql += vbCrLf + " SELECT I.TRANDATE,I.BATCHNO,SUM(I.AMOUNT+I.TAX)AS AMOUNT,I.TRANTYPE,ME.METALNAME "
        strSql += vbCrLf + " INTO #PU_REGISTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = I.METALID"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND I.TRANTYPE IN ('SR','PU')"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " GROUP BY I.TRANDATE,I.BATCHNO,I.TRANTYPE,ME.METALNAME"

        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#ACC_REGISTER','U') IS NOT NULL DROP TABLE #ACC_REGISTER"
        strSql += vbCrLf + " SELECT I.TRANDATE,I.BATCHNO"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'DI' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'DI' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS DISCOUNT"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CA' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CA' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS CASH"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CC' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CC' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS [CR CARD]"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CH' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CH' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS CHEQUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'SS' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'SS' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS SCHEME"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CB' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CB' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS SCH_BONUS"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CG' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CG' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS SCH_GIFT"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CZ' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CZ' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS SCH_PRIZE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'AA' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'AA' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS ADVANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'DU' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'DU' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS DUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE NOT IN ('SS','CH','CC','CA','DI','AA','DU','CB','CG','CZ') AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE NOT IN ('SS','CH','CC','CA','DI','AA','DU','CB','CG','CZ') AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS OTHERS"
        strSql += vbCrLf + " INTO #ACC_REGISTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS I"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND I.PAYMODE IN ('AA','CA','CC','CH','DI','HC','RO','SS','DU','CB','CG','CZ')"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " GROUP BY I.TRANDATE,I.BATCHNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + " UPDATE SA SET "
        strSql += vbCrLf + " SA.CASH = ACC.CASH "
        strSql += vbCrLf + " ,SA.[CR CARD]= ACC.[CR CARD],SA.[DISCOUNT] = ACC.DISCOUNT  ,SA.CHEQUE = ACC.CHEQUE ,SA.SCHEME = ACC.SCHEME ,SA.SCH_BONUS = ACC.SCH_BONUS  "
        strSql += vbCrLf + " ,SA.SCH_GIFT = ACC.SCH_GIFT,SA.SCH_PRIZE = ACC.SCH_PRIZE,SA.ADVANCE = ACC.ADVANCE  ,SA.DUE = ACC.DUE ,SA.OTHERS = ACC.OTHERS    "
        strSql += vbCrLf + " FROM  #SA_REGISTER AS SA INNER JOIN #ACC_REGISTER AS ACC ON ACC.BATCHNO = SA.BATCHNO AND SA.TRANDATE = ACC.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + " UPDATE #SA_REGISTER SET "
        strSql += vbCrLf + " CASH = NULL	,[CR CARD]= NULL,[DISCOUNT] = NULL  ,CHEQUE = NULL ,SCHEME = NULL ,SCH_BONUS = NULL  "
        strSql += vbCrLf + " ,SCH_GIFT = NULL,SCH_PRIZE = NULL,ADVANCE = NULL  ,DUE = NULL ,OTHERS = NULL    "
        strSql += vbCrLf + " WHERE  METALID = 'S' AND  BATCHNO IN "
        strSql += vbCrLf + " (SELECT BATCHNO FROM #SA_REGISTER WHERE METALID = 'G')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + " UPDATE SA SET "
        strSql += vbCrLf + "  SA.[SALES RETURN]=	CASE WHEN PU.TRANTYPE = 'SR'THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " ,SA.[GOLD PUR]=		CASE WHEN PU.TRANTYPE = 'PU' AND PU.METALNAME = 'GOLD' THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " ,SA.[SILVER PUR]=	CASE WHEN PU.TRANTYPE = 'PU' AND PU.METALNAME = 'SILVER' THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " ,SA.[PLATINUM PUR]=	CASE WHEN PU.TRANTYPE = 'PU' AND PU.METALNAME = 'PLATINUM' THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " ,SA.[DIAMOND PUR]=	CASE WHEN PU.TRANTYPE = 'PU' AND PU.METALNAME = 'DIAMOND' THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " ,SA.[STN PUR]=		CASE WHEN PU.TRANTYPE = 'PU' AND PU.METALNAME = 'STONE' THEN PU.AMOUNT ELSE NULL END"
        strSql += vbCrLf + " FROM #SA_REGISTER AS SA INNER JOIN #PU_REGISTER AS PU ON PU.BATCHNO = SA.BATCHNO AND SA.TRANDATE = PU.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + " INSERT INTO #SA_REGISTER"
        strSql += vbCrLf + " SELECT 	NULL,NULL,CATNAME AS PARTICULAR"
        strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM([AMOUNT])	,SUM([SGST])	,SUM([CGST])"
        strSql += vbCrLf + " ,SUM([IGST])	,SUM([GST])		,SUM([NETAMOUNT]),SUM([CASH])"
        strSql += vbCrLf + " ,SUM([CHEQUE])	,SUM([CR CARD])	,SUM([SCHEME])	,SUM([SCH_BONUS])"
        strSql += vbCrLf + " ,SUM([SCH_GIFT]),SUM([SCH_PRIZE]),SUM([DISCOUNT]),SUM([ADVANCE])"
        strSql += vbCrLf + " ,SUM([DUE])		,SUM([OTHERS])	,SUM([SALES RETURN]),SUM([GOLD PUR])"
        strSql += vbCrLf + " ,SUM([SILVER PUR]),SUM([PLATINUM PUR]),SUM([DIAMOND PUR])"
        strSql += vbCrLf + " ,SUM([STN PUR])	,[RESULT1]	,3 AS [RESULT2]	,NULL,NULL,NULL"
        strSql += vbCrLf + " FROM #SA_REGISTER WHERE RESULT2 = 2"
        strSql += vbCrLf + " GROUP BY  RESULT1,CATNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = Nothing
        strSql += vbCrLf + " INSERT INTO #SA_REGISTER (PARTICULAR,RESULT1,RESULT2)"
        strSql += vbCrLf + " SELECT DISTINCT CATNAME,RESULT1,1 FROM #SA_REGISTER WHERE RESULT2 = 2"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        If chkGrandTotal.Checked Then
            strSql = Nothing
            strSql += vbCrLf + " INSERT INTO #SA_REGISTER"
            strSql += vbCrLf + " SELECT 	NULL,NULL,'GRAND TOTAL' PARTICULAR"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM([AMOUNT])	,SUM([SGST])	,SUM([CGST])"
            strSql += vbCrLf + " ,SUM([IGST])	,SUM([GST])		,SUM([NETAMOUNT]),SUM([CASH])"
            strSql += vbCrLf + " ,SUM([CHEQUE])	,SUM([CR CARD])	,SUM([SCHEME])	,SUM([SCH_BONUS])"
            strSql += vbCrLf + " ,SUM([SCH_GIFT]),SUM([SCH_PRIZE]),SUM([DISCOUNT]),SUM([ADVANCE])"
            strSql += vbCrLf + " ,SUM([DUE])		,SUM([OTHERS])	,SUM([SALES RETURN]),SUM([GOLD PUR])"
            strSql += vbCrLf + " ,SUM([SILVER PUR]),SUM([PLATINUM PUR]),SUM([DIAMOND PUR])"
            strSql += vbCrLf + " ,SUM([STN PUR])	,999 AS [RESULT1]	,999 AS [RESULT2]	,NULL,NULL,NULL"
            strSql += vbCrLf + " FROM #SA_REGISTER WHERE RESULT2 = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If


        If rbtSummary.Checked Then
            strSql = ""
            If rbtDateWise.Checked Then
                strSql += vbCrLf + " SELECT * FROM ("
                strSql += vbCrLf + " SELECT 	CASE WHEN RESULT2 = 3 THEN 'SUB TOTAL' ELSE CONVERT(VARCHAR(10),TRANDATE,103) END AS PARTICULAR"
                strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM([AMOUNT])[AMOUNT]	,SUM([SGST])[SGST]	,SUM([CGST])[CGST]"
                strSql += vbCrLf + " ,SUM([IGST])[IGST]	,SUM([GST])	[GST],SUM([NETAMOUNT])[NETAMOUNT],SUM([CASH])[CASH]"
                strSql += vbCrLf + " ,SUM([CHEQUE])[CHEQUE],SUM([CR CARD])[CR CARD],SUM([SCHEME])[SCHEME],SUM([SCH_BONUS])[SCH_BONUS]"
                strSql += vbCrLf + " ,SUM([SCH_GIFT])[SCH_GIFT],SUM([SCH_PRIZE])[SCH_PRIZE],SUM([DISCOUNT])[DISCOUNT],SUM([ADVANCE])[ADVANCE]"
                strSql += vbCrLf + " ,SUM([DUE])	[DUE]	,SUM([OTHERS])[OTHERS]	,SUM([SALES RETURN])[SALES RETURN],SUM([GOLD PUR])[GOLD PUR]"
                strSql += vbCrLf + " ,SUM([SILVER PUR])[SILVER PUR],SUM([PLATINUM PUR])[PLATINUM PUR],SUM([DIAMOND PUR])[DIAMOND PUR]"
                strSql += vbCrLf + " ,SUM([STN PUR])[STN PUR]"
                strSql += vbCrLf + " ,RESULT1,RESULT2"
                strSql += vbCrLf + " FROM #SA_REGISTER WHERE RESULT2 IN (2,3) "
                strSql += vbCrLf + " GROUP BY  RESULT1,TRANDATE ,RESULT2"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT PARTICULAR,GRSWT,AMOUNT,SGST,CGST,IGST,GST,NETAMOUNT"
                strSql += vbCrLf + " 	,CASH,CHEQUE,[CR CARD],SCHEME,[SCH_BONUS],[SCH_GIFT],[SCH_PRIZE]"
                strSql += vbCrLf + " 	,DISCOUNT,ADVANCE,DUE,OTHERS,[SALES RETURN],[GOLD PUR],[SILVER PUR]"
                strSql += vbCrLf + " 	,[PLATINUM PUR],[DIAMOND PUR],[STN PUR],RESULT1,RESULT2"
                strSql += vbCrLf + " FROM #SA_REGISTER WHERE RESULT2 IN (999) "
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT PARTICULAR,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL"
                strSql += vbCrLf + " ,NULL,NULL,NULL,NULL,NULL,NULL,NULL,RESULT1,1 FROM #SA_REGISTER WHERE RESULT2 = 1)X "
                strSql += vbCrLf + " ORDER BY RESULT1,RESULT2"
            Else
                strSql += vbCrLf + " SELECT PARTICULAR,GRSWT,AMOUNT,SGST,CGST,IGST,GST,NETAMOUNT"
                strSql += vbCrLf + " 	,CASH,CHEQUE,[CR CARD],SCHEME,[SCH_BONUS],[SCH_GIFT],[SCH_PRIZE]"
                strSql += vbCrLf + " 	,DISCOUNT,ADVANCE,DUE,OTHERS,[SALES RETURN],[GOLD PUR],[SILVER PUR]"
                strSql += vbCrLf + " 	,[PLATINUM PUR],[DIAMOND PUR],[STN PUR],RESULT1,RESULT2"
                strSql += vbCrLf + " FROM #SA_REGISTER WHERE RESULT2 IN (3,999) ORDER BY RESULT1,RESULT2  "
            End If
        Else
            strSql = " SELECT * FROM #SA_REGISTER ORDER BY RESULT1,RESULT2 "
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        objGridShower = New frmGridDispDia
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = dtGrid
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "SALES REGISTER"
        Dim tit As String = "SALES REGISTER (CATEGORY WISE)" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        'objGridShower.ShowDialog()
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
        objGridShower.gridView.Columns("RESULT1").Visible = False
        objGridShower.gridView.Columns("RESULT2").Visible = False
        If rbtDetailed.Checked Then
            objGridShower.gridView.Columns("CATNAME").Visible = False
            objGridShower.gridView.Columns("SNO").Visible = False
            objGridShower.gridView.Columns("METALID").Visible = False
            objGridShower.gridView.Columns("BATCHNO").Visible = False
        End If

        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        For i As Integer = 0 To objGridShower.gridView.RowCount - 1
            If Val(objGridShower.gridView.Rows(i).Cells("RESULT2").Value.ToString) = 1 Then
                objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LavenderBlush
                objGridShower.gridView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            ElseIf Val(objGridShower.gridView.Rows(i).Cells("RESULT2").Value.ToString) = 3 Then
                objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                objGridShower.gridView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            ElseIf Val(objGridShower.gridView.Rows(i).Cells("RESULT2").Value.ToString) = 999 Then
                objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LawnGreen
                objGridShower.gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                objGridShower.gridView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
            End If
        Next
        'objGridShower.AutoResize()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            dgv.Columns("PARTICULAR").Visible = True
            If rbtDetailed.Checked Then
                dgv.Columns("BATCHNO").Visible = False
            End If
            dgv.Columns("METALID").Visible = False
            dgv.Columns("SNO").Visible = False
            dgv.Columns("KEYNO").Visible = False
            dgv.Columns("COLHEAD").Visible = False

            dgv.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            If rbtSummary.Checked Then
                dgv.Columns("TRANDATE").Visible = False
            End If
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New FRM_SALESREGISTER_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(FRM_SALESREGISTER_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New FRM_SALESREGISTER_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_SALESREGISTER_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")

    End Sub

    Private Sub rbtSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummary.CheckedChanged
        grpSummary.Enabled = rbtSummary.Checked
    End Sub
End Class
Public Class FRM_SALESREGISTER_Properties

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
End Class