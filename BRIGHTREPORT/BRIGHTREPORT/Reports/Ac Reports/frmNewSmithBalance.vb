Imports System.Data.OleDb
Public Class frmNewSmithBalance
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Sysid As String = ""

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        Prop_Sets()
        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        ReziseToolStripMenuItem.Checked = False
        Dim chkCostName As String = GetSelectedCostId(chkLstCostCentre, True)
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, True)
        Dim cmbAccName As String = ""
        If chkCostName = "''" Then chkCostName = ""
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If
        If chkCostName = "" Then chkCostName = "ALL"
        If chkLstCostCentre.CheckedItems.Count = chkLstCostCentre.Items.Count Then
            chkCostName = "ALL"
        End If

        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbSmith.Text & "'"
        cmbAccName = objGPack.GetSqlValue(strSql, , , )
        If cmbAccName = "" Then MsgBox("Smith Name Should not Empty", MsgBoxStyle.Information) : Exit Sub

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'XYTEMP" & Sysid & "SMITHBALDET')>0 DROP TABLE TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + "SELECT TRANNO,TRANDATE,REMARK,"
        strSql += vbCrLf + "CASE WHEN RGRSWT =0 THEN NULL ELSE RGRSWT END RGRSWT,"
        strSql += vbCrLf + "CASE WHEN RNETWT =0 THEN NULL ELSE RNETWT END RNETWT,"
        strSql += vbCrLf + "CASE WHEN RTOUCH =0 THEN NULL ELSE RTOUCH END RTOUCH,"
        strSql += vbCrLf + "CONVERT(NUMERIC(15,3),CASE WHEN RPUREWT=0 THEN NULL ELSE RPUREWT END) RPUREWT,"
        strSql += vbCrLf + "CONVERT(NUMERIC(15,2),CASE WHEN RAMOUNT=0 THEN NULL ELSE RAMOUNT END) RAMOUNT,"
        strSql += vbCrLf + "CASE WHEN IGRSWT =0 THEN NULL ELSE IGRSWT END IGRSWT,"
        strSql += vbCrLf + "CASE WHEN INETWT =0 THEN NULL ELSE INETWT END INETWT,"
        strSql += vbCrLf + "ITOUCH,"
        strSql += vbCrLf + "CONVERT(NUMERIC(15,3),CASE WHEN IPUREWT =0 THEN NULL ELSE IPUREWT END) IPUREWT,"
        strSql += vbCrLf + "CONVERT(NUMERIC(15,2),CASE WHEN IAMOUNT =0 THEN NULL ELSE IAMOUNT END) IAMOUNT,"
        strSql += vbCrLf + "RESULT,COLHEAD,BATCHNO,ACNAME,TFILTER"
        strSql += vbCrLf + " INTO TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET FROM  ( "
        strSql += vbCrLf + "/*OPENING WEIGHT*/"
        If cmbAccName <> "ALL" Then strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),'Party: '+(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN('" & cmbAccName & "'))) TRANNO,CONVERT(VARCHAR(12),NULL)TRANDATE"
        If cmbAccName = "ALL" Then strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),'Party: ALL')) TRANNO,CONVERT(VARCHAR(12),NULL)TRANDATE"
        strSql += vbCrLf + ",CONVERT(VARCHAR(50),'Opening Fine Wt:')[REMARK]"
        strSql += vbCrLf + ",NULL ITOUCH"
        strSql += vbCrLf + ",NULL AS IGRSWT"
        strSql += vbCrLf + ",NULL AS INETWT"
        strSql += vbCrLf + ",NULL AS IPUREWT"
        strSql += vbCrLf + ",NULL AS IAMOUNT"
        strSql += vbCrLf + ",NULL AS RTOUCH"
        strSql += vbCrLf + ",NULL AS RGRSWT"
        strSql += vbCrLf + ",NULL AS RNETWT"
        strSql += vbCrLf + ",SUM(RPUREWT)-SUM(IPUREWT) RPUREWT"
        strSql += vbCrLf + ",NULL AS RAMOUNT"
        strSql += vbCrLf + ",1 RESULT,'  'COLHEAD"
        strSql += vbCrLf + ",CONVERT(VARCHAR(15),NULL)AS BATCHNO"
        strSql += vbCrLf + ",CONVERT(VARCHAR(100),ACNAME)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + "FROM"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END IGRSWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END INETWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END IPUREWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'I' THEN AMOUNT ELSE 0 END IAMOUNT,0 ITOUCH"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END RGRSWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END RNETWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END RPUREWT"
        strSql += vbCrLf + ",CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE 0 END RAMOUNT,0 RTOUCH"
        strSql += vbCrLf + ",(SELECT ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE=O.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT O"
        strSql += vbCrLf + "WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID NOT IN('D','T')) "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND O.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND O.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT GRSWT IGRSWT,NETWT INETWT"
        strSql += vbCrLf + ",PUREWT AS IPUREWT,CONVERT(VARCHAR(50),TOUCH) ITOUCH,AMOUNT IAMOUNT,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RTOUCH,0 RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE AS I WHERE I.METALID NOT IN('D','T')"
        strSql += vbCrLf + "AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT STNWT IGRSWT,STNWT INETWT"
        strSql += vbCrLf + ",STNWT AS IPUREWT,CONVERT(VARCHAR(50),NULL) ITOUCH,0 IAMOUNT,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RTOUCH,0 RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSSTONE AS S "
        strSql += vbCrLf + "INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'')=''  "
        strSql += vbCrLf + "AND I.ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND I.COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 0 IGRSWT,0 INETWT,0 IPUREWT,0 TOUCH,0 IAMOUNT"
        strSql += vbCrLf + ",GRSWT RGRSWT,GRSWT RNETWT"
        strSql += vbCrLf + ",PUREWT AS RPUREWT,TOUCH RTOUCH,AMOUNT RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS R WHERE R.METALID NOT IN('D','T')"
        strSql += vbCrLf + "AND R.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND R.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND R.TRANTYPE NOT IN ('RPU')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 0 IGRSWT,0 INETWT,0 IPUREWT,0 TOUCH,0 IAMOUNT"
        strSql += vbCrLf + ",STNWT RGRSWT"
        strSql += vbCrLf + ",STNWT RNETWT"
        strSql += vbCrLf + ",STNWT AS RPUREWT,0 RTOUCH,0 RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPTSTONE AS S "
        strSql += vbCrLf + "INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'')=''  "
        strSql += vbCrLf + "AND I.ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND I.COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('RPU')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 0 IGRSWT"
        strSql += vbCrLf + ",0 INETWT,0 IPUREWT,0 ITOUCH,0 IAMOUNT"
        strSql += vbCrLf + ",0 RGRSWT "
        strSql += vbCrLf + ",0 RNETWT,0 RPUREWT,0 RTOUCH,0 RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=O.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT O"
        strSql += vbCrLf + "WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN('D','T')) "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND O.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND O.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 0 IGRSWT,0 INETWT,0 IPUREWT,CONVERT(VARCHAR(50),TOUCH) ITOUCH,AMOUNT IAMOUNT,0 RGRSWT,0 RNETWT,0 PUREWT,0 TOUCH,0 RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE AS I WHERE I.METALID IN('D','T')"
        strSql += vbCrLf + "AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT 0 IGRSWT,0 INETWT,0 IPUREWT,0 TOUCH,0 IAMOUNT"
        strSql += vbCrLf + ",0 RGRSWT"
        strSql += vbCrLf + ",0 RNETWT,0 AS RPUREWT,TOUCH RTOUCH,AMOUNT RAMOUNT"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS R WHERE R.METALID IN('D','T')"
        strSql += vbCrLf + "AND R.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND R.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND R.TRANTYPE NOT IN ('RPU')"
        strSql += vbCrLf + ")OPE"
        strSql += vbCrLf + "GROUP BY ACNAME"
        strSql += vbCrLf + "/*OPENING AMOUNT*/"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),'') TRANNO,CONVERT(VARCHAR(12),NULL)TRANDATE"
        strSql += vbCrLf + ",CONVERT(VARCHAR(50),'Opening Cash:')[REMARK]"
        strSql += vbCrLf + ",NULL ITOUCH"
        strSql += vbCrLf + ",NULL AS IGRSWT"
        strSql += vbCrLf + ",NULL AS INETWT"
        strSql += vbCrLf + ",NULL AS IPUREWT"
        strSql += vbCrLf + ",NULL AS IAMOUNT"
        strSql += vbCrLf + ",NULL AS RTOUCH"
        strSql += vbCrLf + ",NULL AS RGRSWT"
        strSql += vbCrLf + ",NULL AS RNETWT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),SUM(AMOUNT)) AS RPUREWT"
        strSql += vbCrLf + ",NULL AS RAMOUNT"
        strSql += vbCrLf + ",2 RESULT,'  'COLHEAD"
        strSql += vbCrLf + ",CONVERT(VARCHAR(15),NULL)AS BATCHNO"
        strSql += vbCrLf + ",CONVERT(VARCHAR(100),ACNAME)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + "FROM"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=O.ACCODE)ACNAME,CREDIT-DEBIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE O"
        strSql += vbCrLf + "WHERE O.ACCODE ='" & cmbAccName & "' "
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)ACNAME,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + "WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        strSql += vbCrLf + "GROUP BY BATCHNO,ACCODE"
        strSql += vbCrLf + ")OPE"
        strSql += vbCrLf + "GROUP BY ACNAME"
        strSql += vbCrLf + "/*TRANSACTION*/"
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),I.TRANNO)"
            strSql += vbCrLf + ",I.TRANDATE"
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN I.REFNO='' THEN I.TRANNO ELSE I.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END AS TRANDATE"
        End If
        strSql += vbCrLf + ",I.REMARK2 AS REMARK ,CONVERT(VARCHAR(50),TOUCH) ITOUCH ,SUM(ISNULL(GRSWT,0)) IGRSWT,"
        strSql += vbCrLf + "SUM(ISNULL(GRSWT,0)-ISNULL(LESSWT,0)) INETWT"
        strSql += vbCrLf + ",SUM(ISNULL(PUREWT,0)) IPUREWT"
        strSql += vbCrLf + ",SUM(CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END) AS IAMOUNT"
        strSql += vbCrLf + ",0 RTOUCH,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RAMOUNT"
        strSql += vbCrLf + ",4 RESULT,' 'COLHEAD,I.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME  ,'N' TFILTER"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..ISSUE AS I INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + "WHERE I.METALID NOT IN('D','T')"
        strSql += vbCrLf + "AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "GROUP BY I.CATCODE,I.TOUCH,I.ACCODE,I.TRANNO,I.BATCHNO,I.REMARK2"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + ",I.TRANDATE"
        Else
            strSql += vbCrLf + ",I.REFNO,CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END"
        End If
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),I.TRANNO)"
            strSql += vbCrLf + ",I.TRANDATE"
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN I.REFNO='' THEN I.TRANNO ELSE I.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END AS TRANDATE"
        End If
        strSql += vbCrLf + ",I.REMARK2 AS REMARK ,CONVERT(VARCHAR(50),TOUCH) ITOUCH "
        strSql += vbCrLf + ",STNWT IGRSWT,STNWT INETWT,STNWT IPUREWT,0 IAMOUNT"
        strSql += vbCrLf + ",0 RTOUCH,0 RGRSWT"
        strSql += vbCrLf + ",0 RNETWT"
        strSql += vbCrLf + ",0 AS RPUREWT,0 RAMOUNT,4 RESULT,' 'COLHEAD,I.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME,'N' TFILTER"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSSTONE AS S "
        strSql += vbCrLf + "INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'')=''  "
        strSql += vbCrLf + "AND I.ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND I.COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('RPU','IPU')"
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),R.TRANNO)"
            strSql += vbCrLf + ",R.TRANDATE "
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN R.REFNO ='' THEN R.TRANNO ELSE R.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN R.REFDATE IS NULL THEN R.TRANDATE ELSE R.REFDATE END"
        End If
        strSql += vbCrLf + ",R.REMARK1 AS REMARK"
        strSql += vbCrLf + ",CONVERT(VARCHAR(50),NULL) ITOUCH,0 IGRSWT,0 INETWT,0 IPUREWT,0 IAMOUNT,TOUCH RTOUCH"
        strSql += vbCrLf + ",SUM(ISNULL(GRSWT,0)) RGRSWT,"
        strSql += vbCrLf + "SUM(ISNULL(GRSWT,0)-ISNULL(LESSWT,0)) RNETWT"
        strSql += vbCrLf + ",SUM(ISNULL(PUREWT,0)) RPUREWT"
        strSql += vbCrLf + ",SUM(CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC')  THEN AMOUNT ELSE 0 END) AS RAMOUNT"
        strSql += vbCrLf + ",4 RESULT,' 'COLHEAD,R.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME  ,'N' TFILTER"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..RECEIPT AS R  WHERE R.METALID NOT IN('D','T')"
        strSql += vbCrLf + "AND R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND R.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND R.TRANTYPE NOT IN ('RPU','IPU')"
        strSql += vbCrLf + "GROUP BY R.CATCODE,R.TOUCH,R.ACCODE,R.TRANNO,R.BATCHNO,R.REMARK1"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + ",R.TRANDATE "
        Else
            strSql += vbCrLf + ",R.REFNO,CASE WHEN R.REFDATE IS NULL THEN R.TRANDATE ELSE R.REFDATE END"
        End If
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),I.TRANNO)"
            strSql += vbCrLf + ",I.TRANDATE"
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN I.REFNO ='' THEN I.TRANNO ELSE I.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END AS TRANDATE"
        End If
        strSql += vbCrLf + ",I.REMARK2 AS REMARK "
        strSql += vbCrLf + ",CONVERT(VARCHAR(50),NULL) ITOUCH ,0 IGRSWT,0 INETWT,0 IPUREWT,0 IAMOUNT"
        strSql += vbCrLf + ",0 RTOUCH,STNWT RGRSWT"
        strSql += vbCrLf + ",STNWT RNETWT"
        strSql += vbCrLf + ",STNWT AS RPUREWT,0 RAMOUNT,4 RESULT,' 'COLHEAD,I.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME,'N' TFILTER"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPTSTONE AS S "
        strSql += vbCrLf + "INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ISNULL(I.CANCEL,'')=''  "
        strSql += vbCrLf + "AND I.ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND I.COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('RPU','IPU')"
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),I.TRANNO) "
            strSql += vbCrLf + ",I.TRANDATE "
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN I.REFNO ='' THEN I.TRANNO ELSE I.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END AS TRANDATE"
        End If
        strSql += vbCrLf + ",I.REMARK2  AS REMARK,CONVERT(VARCHAR(50),TOUCH) ITOUCH,0 IGRSWT"
        strSql += vbCrLf + ",0 INETWT,0 IPUREWT"
        strSql += vbCrLf + ",SUM(CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC')  THEN AMOUNT ELSE 0 END) AS IAMOUNT"
        strSql += vbCrLf + ",0 RTOUCH,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RAMOUNT"
        strSql += vbCrLf + ",4 RESULT,' 'COLHEAD,I.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..ISSUE AS I INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE  "
        strSql += vbCrLf + "WHERE I.METALID IN('D','T')"
        strSql += vbCrLf + "AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND I.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND I.TRANTYPE NOT IN ('IPU','RPU','IIS')"
        strSql += vbCrLf + "GROUP BY I.CATCODE,I.TOUCH,I.ACCODE,I.TRANNO"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + ",I.TRANDATE"
        Else
            strSql += vbCrLf + ",I.REFNO,CASE WHEN I.REFDATE IS NULL THEN I.TRANDATE ELSE I.REFDATE END"
        End If
        strSql += vbCrLf + ",I.BATCHNO,I.REMARK2"
        strSql += vbCrLf + "UNION ALL"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),R.TRANNO)"
            strSql += vbCrLf + ",R.TRANDATE "
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN R.REFNO='' THEN R.TRANNO ELSE R.REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN R.REFDATE IS NULL THEN R.TRANDATE ELSE R.REFDATE END AS TRANDATE "
        End If
        strSql += vbCrLf + ",R.REMARK1 AS REMARK"
        strSql += vbCrLf + ",CONVERT(VARCHAR(50),TOUCH) ITOUCH,0 IGRSWT,0 INETWT,0 IPUREWT,0 IAMOUNT,0 RTOUCH"
        strSql += vbCrLf + ",0 RGRSWT,0 RNETWT,0 RPUREWT"
        strSql += vbCrLf + ",SUM(CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC')  THEN AMOUNT ELSE 0 END) AS RAMOUNT"
        strSql += vbCrLf + ",4 RESULT,' 'COLHEAD,R.BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..RECEIPT AS R  WHERE R.METALID IN('D','T')"
        strSql += vbCrLf + "AND R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND R.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "AND R.TRANTYPE NOT IN ('RPU')"
        strSql += vbCrLf + "GROUP BY R.CATCODE,R.TOUCH,R.ACCODE,R.TRANNO"
        If rbtntrandate.Checked Then
            strSql += vbCrLf + ",R.TRANDATE"
        Else
            strSql += vbCrLf + ",R.REFNO,CASE WHEN R.REFDATE IS NULL THEN R.TRANDATE ELSE R.REFDATE END"
        End If
        strSql += vbCrLf + ",R.BATCHNO,R.REMARK1"
        strSql += vbCrLf + "UNION ALL"

        If rbtntrandate.Checked Then
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),TRANNO)TRANNO"
            strSql += vbCrLf + ",TRANDATE"
        Else
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(50),(CASE WHEN REFNO ='' THEN TRANNO ELSE REFNO END)) AS TRANNO "
            strSql += vbCrLf + ",CASE WHEN REFDATE IS NULL THEN TRANDATE ELSE REFDATE END AS TRANDATE "
        End If
        strSql += vbCrLf + ",REMARK1 AS REMARK"
        strSql += vbCrLf + ",NULL ITOUCH,NULL IGRSWT,NULL INETWT,NULL IPUREWT"
        strSql += vbCrLf + ",CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS IAMOUNT"
        strSql += vbCrLf + ",NULL RTOUCH,NULL RGRSWT,NULL RNETWT,NULL RPUREWT"
        strSql += vbCrLf + ",CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS RAMOUNT"
        strSql += vbCrLf + ",4 RESULT,' 'COLHEAD,BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=T.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        strSql += vbCrLf + "AND BATCHNO NOT IN "
        strSql += vbCrLf + "(SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE"
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + "AND TRANTYPE NOT IN ('IAP','RAP')"
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        strSql += vbCrLf + "UNION"
        strSql += vbCrLf + "SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT"
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "AND ACCODE ='" & cmbAccName & "' "
        If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + "AND TRANTYPE NOT IN ('RPU')"
        strSql += vbCrLf + " AND COMPANYID IN(" & SelectedCompanyId & ")"
        If chkCostName <> "ALL" Then
            strSql += vbCrLf + "AND COSTID IN(" & chkCostName & ")  "
        End If
        strSql += vbCrLf + ")"
        strSql += vbCrLf + ")X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "SELECT COUNT(*) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=1"
        Dim PureWt As Integer = objGPack.GetSqlValue(strSql, , 0)
        strSql = "SELECT COUNT(*) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=2"
        Dim Amount As Integer = objGPack.GetSqlValue(strSql, , 0)
        strSql = "SELECT COUNT(*) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4"
        Dim Tran As Integer = objGPack.GetSqlValue(strSql, , 0)
        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET(REMARK,RPUREWT,RESULT,ACNAME,TFILTER,COLHEAD)"
        strSql += vbCrLf + "SELECT DISTINCT 'Closing Fine Wt:'"
        If PureWt > 0 And Tran > 0 Then
            strSql += vbCrLf + ",((SELECT ISNULL(RPUREWT,0) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=1 )+SUM(ISNULL(RPUREWT,0)))-SUM(ISNULL(IPUREWT,0))"
        ElseIf PureWt > 0 Then
            strSql += vbCrLf + ",(SELECT ISNULL(RPUREWT,0) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=1 )"
        Else
            strSql += vbCrLf + ",SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0))"
        End If
        strSql += vbCrLf + ",6,ACNAME ,'Y','G'"
        If PureWt > 0 Or Tran > 0 Then
            strSql += vbCrLf + "FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT NOT IN(1,2) GROUP BY ACNAME"
        Else
            strSql += vbCrLf + "FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET  GROUP BY ACNAME"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET(REMARK,RPUREWT,RESULT,ACNAME,TFILTER,COLHEAD)"
        strSql += vbCrLf + "SELECT DISTINCT 'Closing Cash:'"
        If Amount > 0 And Tran > 0 Then
            strSql += vbCrLf + ",((SELECT ISNULL(RPUREWT,0) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=2 )+SUM(ISNULL(RAMOUNT,0)))-SUM(ISNULL(IAMOUNT,0))"
        ElseIf Amount > 0 Then
            strSql += vbCrLf + ",(SELECT ISNULL(RPUREWT,0) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=2 )"
        Else
            strSql += vbCrLf + ",SUM(ISNULL(RAMOUNT,0))-SUM(ISNULL(IAMOUNT,0))"
        End If
        strSql += vbCrLf + ",7,ACNAME ,'Y','G'"
        If Amount > 0 Or Tran > 0 Then
            strSql += vbCrLf + "FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT NOT IN(1,6,2) GROUP BY ACNAME"
        Else
            strSql += vbCrLf + "FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET GROUP BY ACNAME"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim OpenWt As Integer
        strSql = " SELECT COUNT(*) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
        strSql += vbCrLf + " WHERE RESULT=1"
        OpenWt = objGPack.GetSqlValue(strSql, , 0)
        If OpenWt = 0 Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET(TRANNO,REMARK,RPUREWT,RESULT,ACNAME,TFILTER,COLHEAD)"
            strSql += vbCrLf + "SELECT 'Party:" & CmbSmith.Text & "','Opening Fine Wt:',0,1,'" & CmbSmith.Text & "','N','T' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        Dim OpenAmt As Integer
        strSql = " SELECT COUNT(*) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
        strSql += vbCrLf + " WHERE RESULT=2"
        OpenAmt = objGPack.GetSqlValue(strSql, , 0)
        If OpenAmt = 0 Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET(REMARK,RPUREWT,RESULT,ACNAME,TFILTER,COLHEAD)"
            strSql += vbCrLf + "SELECT 'Opening Cash:',0,2,'" & CmbSmith.Text & "','N','T' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If

        'strSql = " DELETE FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
        'strSql += vbCrLf + " WHERE RPUREWT=0 AND RESULT IN(6,7)"
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()
        strSql = " IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPSMITHBALDET')>0 DROP TABLE MASTER..TEMPSMITHBALDET"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " select * into  [MASTER].[DBO].[TEMPSMITHBALDET] FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT='4' ORDER BY TRANDATE,TRANNO"
        strSql += vbCrLf + ""
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM MASTER..TEMPSMITHBALDET"
        strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            strSql = " IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPSMITHBALDET')>0 DROP TABLE MASTER..TEMPSMITHBALDET"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " select * into  [MASTER].[DBO].[TEMPSMITHBALDET] FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT='2' ORDER BY TRANDATE,TRANNO"
            strSql += vbCrLf + ""
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " UPDATE [MASTER].[DBO].[TEMPSMITHBALDET] SET RPUREWT=0,IPUREWT=0,RAMOUNT=0,IAMOUNT=0 "
            strSql += vbCrLf + ""
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPSMITHBALITITLE')> 0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + "    UPDATE [MASTER].[DBO].[TEMPSMITHBALITITLE] SET CLOCS=OPNCS,CLOWT=OPNWT "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPSMITHBALITITLE')> 0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + "    UPDATE [MASTER].[DBO].[TEMPSMITHBALITITLE] SET OPNCS=0,OPNWT=0 "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            'MsgBox("Record not found", MsgBoxStyle.Information)
            'Exit Sub
        End If

        Dim tit As String = " SMITH BALANCE DETAILED REPORT"
        tit += " FROM  " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCostName <> "" Then
            Dim Cname As String = ""
            If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                    If chkLstCostCentre.GetItemChecked(CNT) = True Then
                        Cname += "" & chkLstCostCentre.Items(CNT) + ","
                    End If
                Next
                If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
            End If
            If Cname <> "" Then
                tit += vbCrLf & "COSTCENTRE :"
                tit = tit + Cname
            End If
        End If
        strSql = " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPSMITHBALITITLE')> 0"
        strSql += vbCrLf + " DROP TABLE MASTER..TEMPSMITHBALITITLE"
        strSql += vbCrLf + " CREATE TABLE MASTER..TEMPSMITHBALITITLE([OPNCS] [varchar](100),[OPNWT] [varchar](100),[CLOCS] [varchar](100),"
        strSql += vbCrLf + " [CLOWT] [varchar](100),[PARTY] VARCHAR(100) ,[TITLE] VARCHAR(1000),[RNTWTSUM] VARCHAR(100),[RFNWTSUM] VARCHAR(100),[RAMTSUM] VARCHAR(100)"
        strSql += vbCrLf + " ,[INTWTSUM] VARCHAR(100),[IFNWTSUM] VARCHAR(100),[IAMTSUM] VARCHAR(100) )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO MASTER..TEMPSMITHBALITITLE SELECT "
        strSql += vbCrLf + " ISNULL((SELECT CONVERT(VARCHAR(50),RPUREWT)"
        strSql += vbCrLf + "  FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=2),0) AS OPNCS,"
        strSql += vbCrLf + " ISNULL((SELECT CONVERT(VARCHAR(50),RPUREWT)"
        strSql += vbCrLf + "  AS OPNWT  FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=1),0) AS OPNWT,"
        strSql += vbCrLf + " ISNULL((SELECT CONVERT(VARCHAR(50),RPUREWT)"
        strSql += vbCrLf + "  AS CLOCS  FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=7),0) AS CLOCS,"
        strSql += vbCrLf + " ISNULL((SELECT CONVERT(VARCHAR(50),RPUREWT)"
        strSql += vbCrLf + "  AS CLOWT  FROM  TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=6),0) AS CLOWT,"
        strSql += vbCrLf + " (SELECT CONVERT(VARCHAR(100),(TRANNO))  FROM "
        strSql += vbCrLf + " TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=1) AS PARTY,"
        strSql += vbCrLf + " ISNULL((SELECT CONVERT(VARCHAR(1000),'" & tit & "') ),0) AS TITLE,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(RNETWT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS RNTWTSUM,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(RPUREWT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS RFNWTSUM,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(RAMOUNT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS RAMTSUM,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(INETWT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS INTWTSUM,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(IPUREWT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS IFNWTSUM,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(IAMOUNT) FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4),0) AS IAMTSUM"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET WHERE RESULT=4"
        Dim dtGrid1 As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid1)
        If Not dtGrid1.Rows.Count > 0 Then
            strSql = " UPDATE [MASTER].[DBO].[TEMPSMITHBALITITLE] SET CLOCS=OPNCS,CLOWT=OPNWT "
            strSql += vbCrLf + ""
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If

        Dim objReport As New BrighttechReport
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptNewSmithBalance, cnDataSource)
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()

        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'objGridShower.gridView.RowTemplate.Height = 21
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ''objGridShower.Size = New Size(778, 550)
        'objGridShower.Text = "SMITH BALANCE DETAILED"
        'Dim tit As String = " SMITH BALANCE DETAILED REPORT"
        'tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        'If chkCostName <> "" Then
        '    Dim Cname As String = ""
        '    If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
        '        For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
        '            If chkLstCostCentre.GetItemChecked(CNT) = True Then
        '                Cname += "" & chkLstCostCentre.Items(CNT) + ","
        '            End If
        '        Next
        '        If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        '    End If
        '    tit += vbCrLf & "COSTCENTRE :"
        '    tit = tit + Cname
        'End If
        ''tit += vbCrLf & "COSTCENTRE  :" & Replace(chkLstCostCentre.Text, "'", "")
        'objGridShower.lblTitle.Text = tit

        'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf objGridShower.gridView_ColumnWidthChanged
        'objGridShower.gridView.DataSource = dtGrid
        'objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'FormatGridColumns(objGridShower.gridView, False, False, , False)
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        'objGridShower.formuser = userId
        'objGridShower.gridViewHeader.Visible = True
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        'objGridShower.WindowState = FormWindowState.Maximized
        'objGridShower.Show()
        'For Each dgvR As DataGridViewRow In objGridShower.gridView.Rows
        '    If Val(dgvR.Cells("RESULT").Value.ToString) = 0 Or Val(dgvR.Cells("RESULT").Value.ToString) = 1 Or Val(dgvR.Cells("RESULT").Value.ToString) = 2 Or Val(dgvR.Cells("RESULT").Value.ToString) = 0 Or Val(dgvR.Cells("RESULT").Value.ToString) = 6 Or Val(dgvR.Cells("RESULT").Value.ToString) = 7 Then
        '        dgvR.DefaultCellStyle = reportSubTotalStyle
        '    End If
        'Next
        'If objGridShower.gridView.Columns.Contains("TRANDATE") Then
        '    objGridShower.gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        'End If
        'With objGridShower.gridView
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        '    .Invalidate()
        '    For Each dgvCol As DataGridViewColumn In .Columns
        '        dgvCol.Width = dgvCol.Width
        '    Next
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        'End With
        'SetGridHeadColWid(objGridShower.gridViewHeader)
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[TRANNO~TRANDATE~REMARK]"
        strSql += " ,''[RTOUCH~RGRSWT~RNETWT~RPUREWT~RAMOUNT]"
        strSql += " ,''[ITOUCH~IGRSWT~INETWT~IPUREWT~IAMOUNT]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        FormatGridColumns(gridviewHead, False, False, , False)
        gridviewHead.Columns("TRANNO~TRANDATE~REMARK").HeaderText = "PARTICULARS"
        gridviewHead.Columns("ITOUCH~IGRSWT~INETWT~IPUREWT~IAMOUNT").HeaderText = "ISSUE"
        gridviewHead.Columns("RTOUCH~RGRSWT~RNETWT~RPUREWT~RAMOUNT").HeaderText = "RECEIPT"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeadER As DataGridView)
        If Not objGridShower.gridView.ColumnCount > 0 Then Exit Sub
        If Not gridViewHeader.ColumnCount > 0 Then Exit Sub
        With gridViewHeader
            .Columns("TRANNO~TRANDATE~REMARK").Width = IIf(objGridShower.gridView.Columns("TRANNO").Visible, objGridShower.gridView.Columns("TRANNO").Width, 0) _
           + IIf(objGridShower.gridView.Columns("TRANDATE").Visible, objGridShower.gridView.Columns("TRANDATE").Width, 0) + IIf(objGridShower.gridView.Columns("REMARK").Visible, objGridShower.gridView.Columns("REMARK").Width, 0)
            .Columns("TRANNO~TRANDATE~REMARK").HeaderText = "PARTICULAR"

            .Columns("ITOUCH~IGRSWT~INETWT~IPUREWT~IAMOUNT").Width = IIf(objGridShower.gridView.Columns("ITOUCH").Visible, objGridShower.gridView.Columns("ITOUCH").Width, 0) _
            + IIf(objGridShower.gridView.Columns("IGRSWT").Visible, objGridShower.gridView.Columns("IGRSWT").Width, 0) + IIf(objGridShower.gridView.Columns("INETWT").Visible, objGridShower.gridView.Columns("INETWT").Width, 0) _
            + IIf(objGridShower.gridView.Columns("IPUREWT").Visible, objGridShower.gridView.Columns("IPUREWT").Width, 0) + IIf(objGridShower.gridView.Columns("IAMOUNT").Visible, objGridShower.gridView.Columns("IAMOUNT").Width, 0)
            .Columns("ITOUCH~IGRSWT~INETWT~IPUREWT~IAMOUNT").HeaderText = "ISSUE"

            .Columns("RTOUCH~RGRSWT~RNETWT~RPUREWT~RAMOUNT").Width = IIf(objGridShower.gridView.Columns("RTOUCH").Visible, objGridShower.gridView.Columns("RTOUCH").Width, 0) _
            + IIf(objGridShower.gridView.Columns("RGRSWT").Visible, objGridShower.gridView.Columns("RGRSWT").Width, 0) + IIf(objGridShower.gridView.Columns("RNETWT").Visible, objGridShower.gridView.Columns("RNETWT").Width, 0) _
            + IIf(objGridShower.gridView.Columns("RPUREWT").Visible, objGridShower.gridView.Columns("RPUREWT").Width, 0) + IIf(objGridShower.gridView.Columns("RAMOUNT").Visible, objGridShower.gridView.Columns("RAMOUNT").Width, 0)
            .Columns("RTOUCH~RGRSWT~RNETWT~RPUREWT~RAMOUNT").HeaderText = "RECEIPT"
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To objGridShower.gridView.ColumnCount - 1
                If objGridShower.gridView.Columns(cnt).Visible Then colWid += objGridShower.gridView.Columns(cnt).Width
            Next
            If colWid >= objGridShower.gridView.Width Then
                gridViewHeadER.Columns("SCROLL").Visible = CType(objGridShower.gridView.Controls(1), VScrollBar).Visible
                gridViewHeadER.Columns("SCROLL").Width = CType(objGridShower.gridView.Controls(1), VScrollBar).Width
                gridViewHeadER.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeadER.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim cnt As Integer
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt = 27 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("TRANNO").Width = 60
            .Columns("REMARK").Width = 200
            .Columns("RAMOUNT").Width = 90
            .Columns("IGRSWT").Width = 85
            .Columns("INETWT").Width = 85
            .Columns("IPUREWT").Width = 85
            .Columns("IAMOUNT").Width = 90
            .Columns("RGRSWT").Width = 85
            .Columns("RNETWT").Width = 85
            .Columns("RPUREWT").Width = 85
            .Columns("ITOUCH").Width = 85
            .Columns("RTOUCH").Width = 85


            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPUREWT").HeaderText = "FINEWT"
            .Columns("ITOUCH").HeaderText = "PURITY"
            .Columns("IAMOUNT").HeaderText = "AMOUNT"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "FINEWT"
            .Columns("RTOUCH").HeaderText = "PURITY"
            .Columns("RAMOUNT").HeaderText = "AMOUNT"

            .Columns("RGRSWT").Visible = False
            .Columns("IGRSWT").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TFILTER").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("ACNAME").Visible = False
            '.CellBorderStyle = DataGridViewCellBorderStyle.None
        End With
    End Sub
    Private Sub frmNewSmithBalanceDetailedReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And TabMain.SelectedTab.Name = TabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmNewSmithBalanceDetailedReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        LoadCompany(chkLstCompany)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
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

        strSql = " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        strSql += " WHERE ACTYPE IN ('G','D','I') AND ISNULL(ACTIVE,'Y') <> 'H'"
        strSql += " ORDER BY RESULT,ACNAME"

        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        BrighttechPack.GlobalMethods.FillCombo(CmbSmith, dtAcc, "ACNAME", )

        Me.TabMain.ItemSize = New Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        Me.TabMain.SelectedTab = TabGeneral
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " ORDER BY CATNAME"
            FillCheckedListBox(strSql, chkLstCategory)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
        CmbSmith.Text = ""
        Prop_Gets()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmNewSmithBalance_Properties
        obj.p_cmbSmith = CmbSmith.Text
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        SetSettingsObj(obj, Me.Name, GetType(frmNewSmithBalance_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmNewSmithBalance_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmNewSmithBalance_Properties))
        CmbSmith.Text = obj.p_cmbSmith
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        TabMain.SelectedTab = TabGeneral
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If GridView.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Export, gridViewHeader)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        If GridView.Rows.Count > 0 And TabMain.SelectedTab.Name = TabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Print, gridViewHeader)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView.KeyPress
        If UCase(e.KeyChar) = "X" Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub GridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles GridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader.HorizontalScrollingOffset = e.NewValue
                If gridViewHeader.Columns.Count > 0 Then
                    If gridViewHeader.Columns.Contains("SCROLL") Then
                        gridViewHeader.Columns("SCROLL").Visible = CType(GridView.Controls(1), VScrollBar).Visible
                        gridViewHeader.Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub gridViewHeader_ColumnHeadersHeightChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewHeader.ColumnHeadersHeightChanged
        If gridViewHeader.DataSource IsNot Nothing Then
            'gridViewHeader.Size = New Size(gridViewHeader.Size.Width, gridViewHeader.ColumnHeadersHeight - 4)
        End If
    End Sub

    Private Sub CmbSmith_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbSmith.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub PnlMark_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PnlMark.Paint

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkCompanySelectAll_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.TextChanged

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class

Public Class frmNewSmithBalance_Properties
    Private cmbSmith_MAN As String = ""
    Public Property p_cmbSmith_MAN() As String
        Get
            Return cmbSmith_MAN
        End Get
        Set(ByVal value As String)
            cmbSmith_MAN = value
        End Set
    End Property
    Private cmbSmith As String = "ALL"
    Public Property p_cmbSmith() As String
        Get
            Return cmbSmith
        End Get
        Set(ByVal value As String)
            cmbSmith = value
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
End Class