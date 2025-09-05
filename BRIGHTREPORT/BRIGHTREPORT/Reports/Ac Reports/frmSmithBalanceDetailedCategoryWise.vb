Imports System.Data.OleDb
Public Class frmSmithBalanceDetailedCategoryWise
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim chkMetal As String = ""
    Dim chkCategory As String = ""

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        GridReport()
        Dim dtGrid As New DataTable
    End Sub
    Private Sub GridReport()
        Dim cmbAccName As String = Nothing
        chkMetal = GetChecked_CheckedList(chkLstMetal)
        chkCategory = GetChecked_CheckedList(chkLstCategory)
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If

        If cmbSmith_MAN.Text = "ALL" Then
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD"
            strSql += " WHERE ISNULL(LEDPRINT,'') <> 'N' AND ACTYPE IN ('G','D','I')"
            strSql += " ORDER BY ACNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    cmbAccName += "'"
                    cmbAccName += dt.Rows(i).Item("ACNAME").ToString
                    cmbAccName += "'"
                    cmbAccName += ","
                Next
                If cmbAccName <> "" Then
                    cmbAccName = Mid(cmbAccName, 1, cmbAccName.Length - 1)
                End If
            End If
        ElseIf cmbSmith_MAN.Text <> "" Then
            Dim sql As String = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN (" & GetQryString(cmbSmith_MAN.Text) & ")"
            Dim dtAcname As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtAcname)
            If dtAcname.Rows.Count > 0 Then
                For i As Integer = 0 To dtAcname.Rows.Count - 1
                    cmbAccName += "'"
                    cmbAccName += dtAcname.Rows(i).Item("ACNAME").ToString
                    cmbAccName += "'"
                    cmbAccName += ","
                Next
                If cmbAccName <> "" Then
                    cmbAccName = Mid(cmbAccName, 1, cmbAccName.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        Dim WitApproval As String = ""
        If cmbTranType.Text = "ALL" Then
            WitApproval = "IAP','RAP"
        End If

        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ORDREP_DEL' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "ORDREP_DEL"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK_1' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "REC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim acfilter As String = ""
        acfilter = " WHERE ISNULL(LEDPRINT,'') <> 'N'"
        'If AcTypeFilteration <> "" Then acfilter += " AND ACTYPE IN (" & AcTypeFilteration & ")"
        If LocalOutSt <> "" Then acfilter += " AND LOCALOUTST ='" & LocalOutSt & "'"

        strSql = " CREATE VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT I.TRANDATE"
        If chkCatName.Checked Then strSql += vbCrLf + ",CATCODE"
        strSql += vbCrLf + ",A.ACNAME as ACCODE"
        strSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT)PUREWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
        strSql += vbCrLf + " WHERE I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('IPU','RPU')"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
            'Else
            'strSql += vbCrLf + " AND I.TRANTYPE IN ('" & WitApproval & "')"
        End If

        'If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCatName.Checked Then
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(LEDGERPRINT,'') <> 'N' AND CATNAME IN (" & chkCategory & "))"
        Else
            acfilter += " AND ACNAME IN (" & cmbAccName & ")"
        End If
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"

        strSql += vbCrLf + " group by I.TRANDATE"
        If chkCatName.Checked Then strSql += vbCrLf + " ,CATCODE"
        strSql += vbCrLf + " ,i.ACCODE,ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "REC"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT I.TRANDATE"
        If chkCatName.Checked Then strSql += vbCrLf + ",CATCODE"
        strSql += vbCrLf + ",A.ACNAME as ACCODE"
        strSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT)PUREWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " left join " & cnAdminDb & "..Achead as A on i.accode = a.accode"
        strSql += vbCrLf + " WHERE I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('IPU','RPU')"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
            'Else
            'strSql += vbCrLf + " AND I.TRANTYPE IN ('" & WitApproval & "')"
        End If
        If chkCatName.Checked Then
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        Else
            acfilter += " AND ACNAME IN (" & cmbAccName & ")"
        End If
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " group by I.TRANDATE"
        If chkCatName.Checked Then strSql += vbCrLf + ",CATCODE"
        strSql += vbCrLf + ",i.ACCODE,ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT BATCHNO INTO TEMP" & systemId & "ORDREP_DEL "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANTYPE IN ('OD') AND ISNULL(CANCEL,'') = ''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK"
        strSql += " ("
        strSql += " PARTICULAR VARCHAR(500)"
        strSql += vbCrLf + ",TTRANDATE VARCHAR(10)"
        strSql += vbCrLf + " ,[RECEIPT GRSWT] NUMERIC(15,3),[RECEIPT NETWT] NUMERIC(15,3)"
        strSql += vbCrLf + " ,[RECEIPT PUREWT] NUMERIC(15,3)"
        strSql += vbCrLf + " ,[ISSUE GRSWT] NUMERIC(15,3),[ISSUE NETWT] NUMERIC(15,3)"
        strSql += vbCrLf + " ,[ISSUE PUREWT] NUMERIC(15,3)"
        strSql += vbCrLf + " ,[CLOSING GRSWT] NUMERIC(15,3),[CLOSING NETWT] NUMERIC(15,3)"
        strSql += vbCrLf + " ,[CLOSING PUREWT] NUMERIC(15,3)"
        If chkCatName.Checked Then strSql += vbCrLf + " ,CATNAME VARCHAR(100)"
        strSql += vbCrLf + ",METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,RESULT INT,COLHEAD VARCHAR(3)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP VARCHAR(10),"
        strSql += vbCrLf + " GRSWT NUMERIC(15,3),"
        strSql += vbCrLf + " NETWT NUMERIC(15,3),"
        strSql += vbCrLf + " PUREWT NUMERIC(15,3)"
        If chkCatName.Checked Then strSql += vbCrLf + " ,CATNAME VARCHAR(100)"
        strSql += vbCrLf + ",METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + "  	SELECT "
        strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
        strSql += vbCrLf + "    ,SUM(GRSWT) AS GRSWT"
        strSql += vbCrLf + "    ,SUM(NETWT) AS NETWT"
        strSql += vbCrLf + "    ,SUM(PUREWT) AS PUREWT"
        If chkCatName.Checked Then
            strSql += vbCrLf + "    ,CA.CATNAME"
            strSql += vbCrLf + " ,ME.METALNAME AS METAL"
            strSql += vbCrLf + "    ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS,NULL TRANDATE"
            strSql += vbCrLf + ",NULL AS ACNAME"
        Else
            strSql += vbCrLf + " ,NULL AS METAL"
            strSql += vbCrLf + "    ,NULL AS GS,NULL TRANDATE"
            strSql += vbCrLf + ",NULL AS ACNAME"
        End If
        strSql += vbCrLf + "  	FROM " & cnStockDb & "..OPENWEIGHT I "
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
        strSql += vbCrLf + "  	WHERE I.STOCKTYPE = 'C'"
        If (ChkApproval.Checked = False And cmbTranType.Text = "ALL") Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'')<>'A'"
        ElseIf cmbTranType.Text = "APPROVAL ISSUE" Or cmbTranType.Text = "APPROVAL RECEIPT" Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'')='A'"
        End If
        strSql += vbCrLf + "    AND I.COMPANYID = '" & strCompanyId & "'"
        If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
        If chkCatName.Checked Then
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            'Else
            '    strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN (" & cmbAccName & "))"
        End If
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        strSql += vbCrLf + "    GROUP BY I.TRANTYPE"
        If chkCatName.Checked Then
            strSql += vbCrLf + ",CA.CATNAME"
            strSql += vbCrLf + ",CA.GS11,CA.GS12,ME.METALNAME"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        'InsGsIssRecPrint("O", "I", False, "INSERTING OPENING FROM ISSUE WITHOUT ORDREP INFO")
        'InsGsIssRecPrint("O", "R", False, "INSERTING OPENING FROM RECEIPT WITHOUT ORDREP INFO")
        InsGsIssRecPrint("O", "I", True, "INSERTING OPENING ORDREP INFO FROM ISSUE")
        InsGsIssRecPrint("O", "R", True, "INSERTING OPENING ORDREP INFO FROM RECEIPT")
        InsGsStonePrint("O", "I", "INSERTING OPENING FROM ISSUESTONE")
        InsGsStonePrint("O", "R", "INSERTING OPENING FROM RECEIPTSTONE")
        'InsGsIssRecPrint("T", "I", False, "INSERTING TRANSACTION FROM ISSUE WITHOUT ORDREP INFO")
        'InsGsIssRecPrint("T", "R", False, "INSERTING TRANSACTION FROM RECEIPT WITHOUT ORDREP INFO")
        InsGsIssRecPrint("T", "I", True, "INSERTING TRANSACTION ORDREP INFO FROM ISSUE")
        InsGsIssRecPrint("T", "R", True, "INSERTING TRANSACTION ORDREP INFO FROM RECEIPT")
        InsGsStonePrint("T", "I", "INSERTING TRANSACTION FROM ISSUESTONE")
        InsGsStonePrint("T", "R", "INSERTING TRANSACTION FROM RECEIPTSTONE")

        If chkDetailed.Checked = True Then
            strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
            strSql += " (PARTICULAR "
            strSql += vbCrLf + " ,TTRANDATE"
            strSql += vbCrLf + " ,[RECEIPT GRSWT]"
            strSql += vbCrLf + " ,[RECEIPT NETWT]"
            strSql += vbCrLf + " ,[RECEIPT PUREWT]"
            strSql += vbCrLf + " ,[ISSUE GRSWT]"
            strSql += vbCrLf + " ,[ISSUE NETWT]"
            strSql += vbCrLf + " ,[ISSUE PUREWT]"
            strSql += vbCrLf + " ,[CLOSING GRSWT]"
            strSql += vbCrLf + " ,[CLOSING NETWT]"
            strSql += vbCrLf + " ,[CLOSING PUREWT]"
            If chkCatName.Checked Then strSql += vbCrLf + " ,CATNAME"
            strSql += vbCrLf + ",METAL,GS,TRANDATE,ACNAME,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT * FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'REC' THEN PUREWT ELSE 0 END) AS [RECEIPT PUREWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN PUREWT ELSE 0 END) AS [ISSUE PUREWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"
            strSql += vbCrLf + " ,(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE PUREWT END) AS [CLOSING PUREWT]"
            If chkCatName.Checked Then strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " , 1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " WHERE NOT ([RECEIPT GRSWT] = 0 AND [RECEIPT NETWT] = 0 AND [RECEIPT PUREWT] = 0  AND [ISSUE GRSWT] = 0 AND [ISSUE NETWT] = 0 AND [ISSUE PUREWT] = 0) AND TRANDATE IS NOT NULL "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT * FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PUREWT ELSE 0 END) AS [RECEIPT PUREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PUREWT ELSE 0 END) AS [ISSUE PUREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE PUREWT END) AS [CLOSING PUREWT]"
            If chkCatName.Checked Then strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " , 1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANDATE,ACNAME,GS"
            If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",GS"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " WHERE NOT ([RECEIPT GRSWT] = 0 AND [RECEIPT NETWT] = 0 AND [RECEIPT PUREWT] = 0  AND [ISSUE GRSWT] = 0 AND [ISSUE NETWT] = 0 AND [ISSUE PUREWT] = 0) AND TRANDATE IS NULL"
            strSql += vbCrLf + "  ORDER BY GS"
            If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",TRANDATE"
            strSql += vbCrLf + " ,[RECEIPT GRSWT] DESC"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else
            strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
            strSql += " (PARTICULAR "
            strSql += vbCrLf + " ,TTRANDATE"
            strSql += vbCrLf + " ,[RECEIPT GRSWT]"
            strSql += vbCrLf + " ,[RECEIPT NETWT]"
            strSql += vbCrLf + " ,[RECEIPT PUREWT]"
            strSql += vbCrLf + " ,[ISSUE GRSWT]"
            strSql += vbCrLf + " ,[ISSUE NETWT]"
            strSql += vbCrLf + " ,[ISSUE PUREWT]"
            strSql += vbCrLf + " ,[CLOSING GRSWT]"
            strSql += vbCrLf + " ,[CLOSING NETWT]"
            strSql += vbCrLf + " ,[CLOSING PUREWT]"
            If chkCatName.Checked Then strSql += vbCrLf + " ,CATNAME"
            strSql += vbCrLf + ",METAL,GS,TRANDATE,ACNAME,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT * FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS [RECEIPT GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS [RECEIPT NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PUREWT ELSE 0 END) AS [RECEIPT PUREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS [ISSUE GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS [ISSUE NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PUREWT ELSE 0 END) AS [ISSUE PUREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*GRSWT ELSE GRSWT END) AS [CLOSING GRSWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE NETWT END) AS [CLOSING NETWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*NETWT ELSE PUREWT END) AS [CLOSING PUREWT]"
            If chkCatName.Checked Then strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " , 1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANDATE,ACNAME,GS"
            If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",GS"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " WHERE NOT ([RECEIPT GRSWT] = 0 AND [RECEIPT NETWT] = 0 AND [RECEIPT PUREWT] = 0  AND [ISSUE GRSWT] = 0 AND [ISSUE NETWT] = 0 AND [ISSUE PUREWT] = 0) "
            strSql += vbCrLf + "  ORDER BY GS"
            If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",TRANDATE"
            strSql += vbCrLf + " ,[RECEIPT GRSWT] DESC"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If



        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
        strSql += vbCrLf + " BEGIN"
        If chkCatName.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 "
            strSql += vbCrLf + " AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
        End If
        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS"
        If chkCatName.Checked Then
            strSql += vbCrLf + ",CATNAME"
            'Else
            '    strSql += vbCrLf + ",ACNAME"
        End If
        strSql += vbCrLf + ",TRANDATE,PARTICULAR"
        'strSql += vbCrLf + " ,[RECEIPT GRSWT]"
        'strSql += vbCrLf + " ,[RECEIPT NETWT]"
        'strSql += vbCrLf + " ,[RECEIPT PUREWT]"
        'strSql += vbCrLf + " ,[ISSUE GRSWT]"
        'strSql += vbCrLf + " ,[ISSUE NETWT]"
        'strSql += vbCrLf + " ,[ISSUE PUREWT]"
        strSql += vbCrLf + " ,RESULT,COLHEAD)"
        strSql += vbCrLf + "   SELECT METAL,GS"
        If chkCatName.Checked Then
            strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",'" & _MaxDate.ToString("yyyy-MM-dd") & "','   '+'CLOSING'"
        Else
            strSql += vbCrLf + ",TRANDATE,'   '+'CLOSING'"
        End If
        'strSql += vbCrLf + " ,SUM(ISNULL([CLOSING GRSWT],0))"
        ' strSql += vbCrLf + " ,SUM(ISNULL([CLOSING NETWT],0))"
        'strSql += vbCrLf + " ,SUM(ISNULL([CLOSING PUREWT],0))"
        'strSql += vbCrLf + " ,SUM([ISSUE GRSWT])"
        'strSql += vbCrLf + " ,SUM([ISSUE NETWT])"
        'strSql += vbCrLf + " ,SUM([ISSUE PUREWT])"
        strSql += vbCrLf + " ,4,'S2'COLHEAD"
        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  GROUP BY METAL,GS"
        If chkCatName.Checked Then
            strSql += vbCrLf + ",CATNAME"
        Else
            strSql += vbCrLf + ",TRANDATE"
        End If
        strSql += vbCrLf + ",RESULT"
        strSql += vbCrLf + "  "
        If chkCatName.Checked = False Then
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS"
            strSql += vbCrLf + ",TRANDATE,PARTICULAR"
            strSql += vbCrLf + " ,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT METAL,GS"
            strSql += vbCrLf + ",TRANDATE,'   '+'OPENING'"
            strSql += vbCrLf + " ,0,'S0'COLHEAD"
            strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  GROUP BY METAL,GS"
            strSql += vbCrLf + ",TRANDATE"
            strSql += vbCrLf + ",RESULT"
            strSql += vbCrLf + "  "
        End If
        If chkCatName.Checked Then
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS"
            If chkCatName.Checked Then
                strSql += vbCrLf + ",CATNAME"
            End If
            strSql += vbCrLf + ",PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT DISTINCT METAL,GS"
            If chkCatName.Checked Then
                strSql += vbCrLf + ",CATNAME,CATNAME"
            Else
                strSql += vbCrLf + ",ACNAME"
            End If
            strSql += vbCrLf + ",0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
        End If
        If chkCatName.Checked Then
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS"
            If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
            strSql += vbCrLf + ",PARTICULAR,TRANDATE"
            strSql += vbCrLf + " ,[RECEIPT GRSWT]"
            strSql += vbCrLf + " ,[RECEIPT NETWT]"
            strSql += vbCrLf + " ,[RECEIPT PUREWT]"
            strSql += vbCrLf + " ,[ISSUE GRSWT]"
            strSql += vbCrLf + " ,[ISSUE NETWT]"
            strSql += vbCrLf + " ,[ISSUE PUREWT]"
            strSql += vbCrLf + " ,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT METAL,GS"
            If chkCatName.Checked Then
                strSql += vbCrLf + ",CATNAME,' '+CATNAME+' TOT'"
            ElseIf chkCatName.Checked = False Then
                strSql += vbCrLf + ",'' ACNAME"
            End If
            strSql += vbCrLf + ",'" & _MaxDate.ToString("yyyy-MM-dd") & "'" 'ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
            strSql += vbCrLf + " ,SUM([RECEIPT GRSWT])"
            strSql += vbCrLf + " ,SUM([RECEIPT NETWT])"
            strSql += vbCrLf + " ,SUM([RECEIPT PUREWT])"
            strSql += vbCrLf + " ,SUM([ISSUE GRSWT])"
            strSql += vbCrLf + " ,SUM([ISSUE NETWT])"
            strSql += vbCrLf + " ,SUM([ISSUE PUREWT])"
            strSql += vbCrLf + " ,3,'S1'COLHEAD"
            strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  GROUP BY METAL,GS"
            If chkCatName.Checked Then
                strSql += vbCrLf + ",CATNAME"
            Else
                strSql += vbCrLf + ",ACNAME"
            End If
        End If
        strSql += vbCrLf + "   END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT GRSWT] = NULL WHERE [RECEIPT GRSWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT NETWT] = NULL WHERE [RECEIPT NETWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PUREWT] = NULL WHERE [RECEIPT PUREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE GRSWT] = NULL WHERE [ISSUE GRSWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE NETWT] = NULL WHERE [ISSUE NETWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PUREWT] = NULL WHERE [ISSUE PUREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING GRSWT] = NULL WHERE [CLOSING GRSWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING NETWT] = NULL WHERE [CLOSING NETWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PUREWT] = NULL WHERE [CLOSING PUREWT] = 0"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Prop_Sets()

        Dim dtGrid As New DataTable("SUMMARY")
        dtGrid.Columns.Add("TABLENAME", GetType(String))
        dtGrid.Columns("TABLENAME").DefaultValue = "SUMMARY"
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        strSql = " SELECT * "
        strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK "
        strSql += vbCrLf + "  ORDER BY METAL,GS"
        If chkCatName.Checked Then strSql += vbCrLf + ",CATNAME"
        strSql += vbCrLf + ",TRANDATE,RESULT"

        da = New OleDbDataAdapter(strSql, cn)
        dtGrid.Rows.Clear()

        da.Fill(dtGrid)


        Dim CheckCol As String = "GS"
        Dim Checker As String = "##gs2932"
        Dim RunGWt As Decimal = 0
        Dim RunNWt As Decimal = 0
        Dim RunPWt As Decimal = 0
        Dim TotRunGWt As Decimal = 0
        Dim TotRunNWt As Decimal = 0
        Dim TotRunPWt As Integer = 0
        Dim checkCat As String = "CAT"
        Dim checkItem As String = "ITEM"
        Dim checkerCat As String = "##cat"

        For Each Ro As DataRow In dtGrid.Rows
            'If chkCategorywise.Checked = False Then
            If Ro.Item("PARTICULAR").ToString = "   OPENING.." Then
                RunGWt = 0 : RunNWt = 0 : RunPWt = 0
            End If
            If Ro.Item(CheckCol).ToString <> Checker Then
                Checker = Ro.Item(CheckCol).ToString
                RunGWt = 0 : RunNWt = 0 : RunPWt = 0
            End If
            If Ro.Item("COLHEAD").ToString <> "" Then Continue For
            RunGWt += Val(Ro.Item("RECEIPT GRSWT").ToString) - Val(Ro.Item("ISSUE GRSWT").ToString)
            RunNWt += Val(Ro.Item("RECEIPT NETWT").ToString) - Val(Ro.Item("ISSUE NETWT").ToString)
            RunPWt += Val(Ro.Item("RECEIPT PUREWT").ToString) - Val(Ro.Item("ISSUE PUREWT").ToString)

            Ro.Item("CLOSING GRSWT") = IIf(RunGWt <> 0, Format(RunGWt, "0.000"), DBNull.Value)
            Ro.Item("CLOSING NETWT") = IIf(RunNWt <> 0, Format(RunNWt, "0.000"), DBNull.Value)
            Ro.Item("CLOSING PUREWT") = IIf(RunPWt <> 0, Format(RunPWt, "0.000"), DBNull.Value)
        Next

        dtGrid.AcceptChanges()
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        dtGrid.Columns("TABLENAME").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("TTRANDATE").SetOrdinal(0)
        dtGrid.Columns("PARTICULAR").SetOrdinal(1)

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "SMITH BALANCE DETAILED"
        Dim tit As String = "SMITH BALANCE DETAILED REPORT"
        tit += " DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        'If chkCostName <> "" Then tit += vbCrLf & "COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        DataGridView_Trandate(objGridShower.gridView)
        objGridShower.Show()
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        dtpFrom.Select()

        For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
            Dim cloGrswt, cloNetwt, cloPuWt As Decimal
            If chkCatName.Checked Then
                If objGridShower.gridView.Rows(i).Cells("COLHEAD").Value.ToString <> "" Then
                    If objGridShower.gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                        If objGridShower.gridView.Rows(i - 1).Cells("CLOSING GRSWT").Value.ToString <> "" Then
                            cloGrswt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING GRSWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING GRSWT").Value = cloGrswt
                            If cloGrswt > 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("RECEIPT GRSWT").Value = cloGrswt
                                End If
                            ElseIf cloGrswt < 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("ISSUE GRSWT").Value = cloGrswt
                                End If
                            End If
                        End If
                        If objGridShower.gridView.Rows(i - 1).Cells("CLOSING NETWT").Value.ToString <> "" Then
                            cloNetwt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING NETWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING NETWT").Value = cloNetwt
                            If cloNetwt > 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("RECEIPT NETWT").Value = cloNetwt
                                End If
                            ElseIf cloNetwt < 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("ISSUE NETWT").Value = cloNetwt
                                End If
                            End If
                        End If
                        If objGridShower.gridView.Rows(i - 1).Cells("CLOSING PUREWT").Value.ToString <> "" Then
                            cloPuWt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING PUREWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING PUREWT").Value = cloPuWt
                            If cloPuWt > 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("RECEIPT PUREWT").Value = cloPuWt
                                End If
                            ElseIf cloPuWt < 0 Then
                                If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                    objGridShower.gridView.Rows(i + 1).Cells("ISSUE PUREWT").Value = cloPuWt
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                If objGridShower.gridView.Rows(i).Cells("RESULT").Value.ToString <> "" Then
                    If objGridShower.gridView.Rows(i).Cells("RESULT").Value = "1" Then
                        If objGridShower.gridView.Rows(i).Cells("CLOSING GRSWT").Value.ToString <> "" Then
                            cloGrswt = objGridShower.gridView.Rows(i).Cells("CLOSING GRSWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING GRSWT").Value = cloGrswt
                            If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "   OPENING.." Then
                                If cloGrswt > 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("RECEIPT GRSWT").Value = cloGrswt
                                        End If
                                    End If
                                ElseIf cloGrswt < 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("ISSUE GRSWT").Value = cloGrswt
                                            'objGridShower.gridView.Rows(i + 2).Cells("ISSUE GRSWT").Value = cloGrswt
                                        End If
                                    End If
                                End If
                            Else
                                objGridShower.gridView.Rows(i - 1).Cells("PARTICULAR").Value = ""
                                objGridShower.gridView.Rows(i + 1).Cells("PARTICULAR").Value = ""
                                'objGridShower.gridView.Rows(i + 2).Cells("PARTICULAR").Value = ""
                            End If
                        End If
                        If objGridShower.gridView.Rows(i).Cells("CLOSING NETWT").Value.ToString <> "" Then
                            cloNetwt = objGridShower.gridView.Rows(i).Cells("CLOSING NETWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING NETWT").Value = cloNetwt
                            If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "   OPENING.." Then
                                If cloNetwt > 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("RECEIPT NETWT").Value = cloNetwt
                                            'objGridShower.gridView.Rows(i + 2).Cells("RECEIPT NETWT").Value = cloNetwt
                                        End If
                                    End If
                                ElseIf cloNetwt < 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("ISSUE NETWT").Value = cloNetwt
                                            'objGridShower.gridView.Rows(i + 2).Cells("ISSUE NETWT").Value = cloNetwt
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If objGridShower.gridView.Rows(i).Cells("CLOSING PUREWT").Value.ToString <> "" Then
                            cloPuWt = objGridShower.gridView.Rows(i).Cells("CLOSING PUREWT").Value
                            objGridShower.gridView.Rows(i).Cells("CLOSING PUREWT").Value = cloPuWt
                            If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "   OPENING.." Then
                                If cloPuWt > 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("RECEIPT PUREWT").Value = cloPuWt
                                            'objGridShower.gridView.Rows(i + 2).Cells("RECEIPT PUREWT").Value = cloPuWt
                                        End If
                                    End If
                                ElseIf cloPuWt < 0 Then
                                    If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value.ToString <> "" Then
                                        If objGridShower.gridView.Rows(i + 1).Cells("COLHEAD").Value = "S2" Then
                                            objGridShower.gridView.Rows(i + 1).Cells("ISSUE PUREWT").Value = cloPuWt
                                            'objGridShower.gridView.Rows(i + 2).Cells("ISSUE PUREWT").Value = cloPuWt
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    ElseIf objGridShower.gridView.Rows(i).Cells("RESULT").Value = "0" Then
                        If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value = "   OPENING" Then
                            If objGridShower.gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S0" Then
                                If cloGrswt > 0 Then
                                    objGridShower.gridView.Rows(i).Cells("RECEIPT GRSWT").Value = cloGrswt
                                ElseIf cloGrswt < 0 Then
                                    objGridShower.gridView.Rows(i).Cells("ISSUE GRSWT").Value = cloGrswt
                                End If
                                If cloNetwt > 0 Then
                                    objGridShower.gridView.Rows(i).Cells("RECEIPT NETWT").Value = cloNetwt
                                ElseIf cloNetwt < 0 Then
                                    objGridShower.gridView.Rows(i).Cells("ISSUE NETWT").Value = cloNetwt
                                End If
                                If cloPuWt > 0 Then
                                    objGridShower.gridView.Rows(i).Cells("RECEIPT PUREWT").Value = cloPuWt
                                ElseIf cloPuWt < 0 Then
                                    objGridShower.gridView.Rows(i).Cells("ISSUE PUREWT").Value = cloPuWt
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub InsGsIssRecPrint(ByVal Tran As String, ByVal RecIss As String, ByVal OrderInfo As Boolean, Optional ByVal InsComments As String = "")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CA.CATNAME"
                strSql += vbCrLf + ",ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS"
            Else
                strSql += vbCrLf + ",null METAL"
                strSql += vbCrLf + " ,null GS"
            End If
            strSql += vbCrLf + " ,I.TRANDATE,I.ACCODE AS ACNAME"
            strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            If chkCatName.Checked Then
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            End If
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & "','U') IS NOT NULL DROP TABLE #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CA.CATNAME"
                strSql += vbCrLf + ",ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS"
            End If
            strSql += vbCrLf + " INTO #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            If chkCatName.Checked Then
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            End If
            strSql += vbCrLf + " WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,SUM(GRSWT) GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT) NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CATNAME"
                strSql += vbCrLf + ",METAL"
                strSql += vbCrLf + " ,GS"
            Else
                strSql += vbCrLf + ",null METAL"
                strSql += vbCrLf + " ,null GS"
            End If
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME"
            strSql += vbCrLf + " FROM #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & " AS I"
            If chkCatName.Checked Then
                strSql += vbCrLf + " GROUP BY "
                strSql += vbCrLf + " CATNAME,GS,METAL"
                'Else
                'strSql += vbCrLf + " GS,METAL"
            End If

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub InsGsStonePrint(ByVal Tran As String, ByVal RecIss As String, Optional ByVal InsComments As String = "")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CA.CATNAME AS CATNAME"
                strSql += vbCrLf + ",ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS"
            Else
                strSql += vbCrLf + ",null METAL"
                strSql += vbCrLf + " ,null GS"
            End If
            strSql += vbCrLf + " ,S.TRANDATE,I.ACCODE AS ACNAME"
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            If chkCatName.Checked Then
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            End If
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU')"
            'If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & "') IS NOT NULL DROP TABLE #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " 'OP'TRANTYPE"
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CA.CATNAME AS CATNAME"
                strSql += vbCrLf + ",ME.METALNAME AS METAL"
                strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN 'GS11' WHEN CA.GS12 = 'Y' THEN 'GS12' ELSE 'OTHER' END AS GS"
            End If
            strSql += vbCrLf + " INTO #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            If chkCatName.Checked Then
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            End If
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " WHERE S.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU')"
            'If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            If chkCatName.Checked Then
                strSql += vbCrLf + " ,CATNAME"
                strSql += vbCrLf + ",METAL"
                strSql += vbCrLf + " ,GS"
            Else
                strSql += vbCrLf + ",null METAL"
                strSql += vbCrLf + " ,null GS"
            End If
            strSql += vbCrLf + " ,NULL TRANDATE,CONVERT(VARCHAR(100),NULL) AS ACNAME"
            strSql += vbCrLf + " FROM #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            If chkCatName.Checked Then
                strSql += vbCrLf + " GROUP BY "
                strSql += vbCrLf + "CATNAME,METAL,GS"
                'Else
                '    strSql += vbCrLf + "METAL,GS"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If

    End Sub
    Private Sub DataGridView_Trandate(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 1 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("TTRANDATE").Visible = True
            .Columns("PARTICULAR").Visible = True
            .Columns("RECEIPT GRSWT").Visible = True
            .Columns("RECEIPT NETWT").Visible = True
            .Columns("RECEIPT PUREWT").Visible = True
            .Columns("ISSUE GRSWT").Visible = True
            .Columns("ISSUE NETWT").Visible = True
            .Columns("ISSUE PUREWT").Visible = True
            .Columns("CLOSING GRSWT").Visible = True
            .Columns("CLOSING NETWT").Visible = True
            .Columns("CLOSING PUREWT").Visible = True

            .Columns("TTRANDATE").Width = 80
            .Columns("PARTICULAR").Width = 340
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("RECEIPT GRSWT").Width = 100
            .Columns("RECEIPT NETWT").Width = 100
            .Columns("RECEIPT PUREWT").Width = 100
            .Columns("ISSUE GRSWT").Width = 100
            .Columns("ISSUE NETWT").Width = 100
            .Columns("ISSUE PUREWT").Width = 100
            .Columns("CLOSING GRSWT").Width = 100
            .Columns("CLOSING NETWT").Width = 100
            .Columns("CLOSING PUREWT").Width = 100

            .Columns("RECEIPT GRSWT").HeaderText = "REC GRSWT"
            .Columns("RECEIPT NETWT").HeaderText = "REC NETWT"
            .Columns("RECEIPT PUREWT").HeaderText = "REC PUREWT"
            .Columns("ISSUE GRSWT").HeaderText = "ISS GRSWT"
            .Columns("ISSUE NETWT").HeaderText = "ISS NETWT"
            .Columns("ISSUE PUREWT").HeaderText = "ISS PUREWT"
            .Columns("CLOSING GRSWT").HeaderText = "GRSWT"
            .Columns("CLOSING NETWT").HeaderText = "NETWT"
            .Columns("CLOSING PUREWT").HeaderText = "PUREWT"

            .Columns("RECEIPT GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[TTRANDATE],''[PARTICULAR]"
        strSql += " ,''[RECEIPT],''[ISSUE],''RUNBAL,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("TTRANDATE").HeaderText = ""
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("RECEIPT").HeaderText = "RECEIPTS"
        gridviewHead.Columns("ISSUE").HeaderText = "ISSUES"
        gridviewHead.Columns("RUNBAL").HeaderText = "RUNNING BALANCE"
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        SetGridHeadColWid(gridviewHead)
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("TTRANDATE").Width = f.gridView.Columns("TTRANDATE").Width
            .Columns("PARTICULAR").Width = f.gridView.Columns("PARTICULAR").Width
            .Columns("RECEIPT").Width = f.gridView.Columns("RECEIPT GRSWT").Width + f.gridView.Columns("RECEIPT NETWT").Width + f.gridView.Columns("RECEIPT PUREWT").Width
            .Columns("ISSUE").Width = f.gridView.Columns("ISSUE GRSWT").Width + f.gridView.Columns("ISSUE NETWT").Width + f.gridView.Columns("ISSUE PUREWT").Width
            .Columns("RUNBAL").Width = f.gridView.Columns("CLOSING GRSWT").Width + f.gridView.Columns("CLOSING NETWT").Width + f.gridView.Columns("CLOSING PUREWT").Width

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim cnt As Integer
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt = 12 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next


            .Columns("PARTICULAR").Width = 320
            .Columns("TRANNO").Width = 60
            .Columns("IPCS").Width = 60
            .Columns("RPCS").Width = 60
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            .Columns("IPUREWT").Width = 100
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            .Columns("RPUREWT").Width = 100
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPUREWT").HeaderText = "PUREWT"
            .Columns("ITOUCH").HeaderText = "TOUCH"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "PUREWT"
            .Columns("RTOUCH").HeaderText = "TOUCH"
            .Columns("IPCS").Visible = False
            .Columns("RPCS").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub frmSmithBalanceDetailedCategoryWise_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSmithBalanceDetailedCategoryWise_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If

        strSql = " SELECT 'ALL' ACNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        strSql += " WHERE ACTYPE IN ('G','D','I')"
        strSql += " ORDER BY RESULT,ACNAME"

        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        BrighttechPack.GlobalMethods.FillCombo(cmbSmith_MAN, dtAcc, "ACNAME", , "ALL")

        cmbTranType.Items.Clear()
        cmbTranType.Items.Add("ALL")
        cmbTranType.Items.Add("ISSUE")
        cmbTranType.Items.Add("APPROVAL ISSUE")
        cmbTranType.Items.Add("OTHER ISSUE")
        cmbTranType.Items.Add("PURCHASE")
        cmbTranType.Items.Add("RECEIPT")
        cmbTranType.Items.Add("APPROVAL RECEIPT")
        cmbTranType.Items.Add("OTHER RECEIPT")
        cmbTranType.Items.Add("PURCHASE RETURN")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub
    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strsql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strsql += " ORDER BY CATNAME"
            FillCheckedListBox(strsql, chkLstCategory)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbSmith_MAN.Text = ""
        'cmbTranType.Text = "ALL"
        Prop_Gets()
        dtpFrom.Select()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmSmithBalanceDetailedCategoryWise_Properties
        obj.p_cmbTranType = cmbTranType.Text
        obj.p_cmbSmith_MAN = cmbSmith_MAN.Text
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        SetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceDetailedCategoryWise_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSmithBalanceDetailedCategoryWise_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceDetailedCategoryWise_Properties))
        cmbTranType.Text = obj.p_cmbTranType
        cmbSmith_MAN.Text = obj.p_cmbSmith_MAN
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
    End Sub

    Private Sub pnlContainer_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlContainer.Paint

    End Sub

    Private Sub cmbTranType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTranType.SelectedIndexChanged
        If cmbTranType.Text = "ALL" Then
            ChkApproval.Enabled = True
            ChkApproval.Checked = True
        Else
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
        End If
    End Sub
End Class
Public Class frmSmithBalanceDetailedCategoryWise_Properties
    Private cmbTranType As String = "ALL"
    Public Property p_cmbTranType() As String
        Get
            Return cmbTranType
        End Get
        Set(ByVal value As String)
            cmbTranType = value
        End Set
    End Property

    Private cmbSmith_MAN As String = ""
    Public Property p_cmbSmith_MAN() As String
        Get
            Return cmbSmith_MAN
        End Get
        Set(ByVal value As String)
            cmbSmith_MAN = value
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
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property
    Private chkCategorySelectAll As Boolean = False
    Public Property p_chkCategorySelectAll() As Boolean
        Get
            Return chkCategorySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCategorySelectAll = value
        End Set
    End Property
    Private chkLstCategory As New List(Of String)
    Public Property p_chkLstCategory() As List(Of String)
        Get
            Return chkLstCategory
        End Get
        Set(ByVal value As List(Of String))
            chkLstCategory = value
        End Set
    End Property
    Private rbtGrossWeight As Boolean = False
    Public Property p_rbtGrossWeight() As Boolean
        Get
            Return rbtGrossWeight
        End Get
        Set(ByVal value As Boolean)
            rbtGrossWeight = value
        End Set
    End Property
    Private rbtNetWeight As Boolean = False
    Public Property p_rbtNetWeight() As Boolean
        Get
            Return rbtNetWeight
        End Get
        Set(ByVal value As Boolean)
            rbtNetWeight = value
        End Set
    End Property
    Private rbtPureWeight As Boolean = True
    Public Property p_rbtPureWeight() As Boolean
        Get
            Return rbtPureWeight
        End Get
        Set(ByVal value As Boolean)
            rbtPureWeight = value
        End Set
    End Property
End Class