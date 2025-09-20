Imports System.Data.OleDb
Public Class frmSmithBalanceDetailedReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim dtissue As New DataTable
    Dim dtreceipt As New DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Sysid As String = ""
    Dim SmithFormat As Integer = GetAdmindbSoftValue("RPT_SMITHBALDET_FORMAT", 1)
    Dim DtTrantype As DataTable

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ''If objGPack.Validator_Check(Me) Then Exit Sub
        Prop_Sets()
        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        ReziseToolStripMenuItem.Checked = False
        If ChkFormat.Checked Then funcSpecificFormat() : Exit Sub
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, True)
        Dim cmbAccName As String = Nothing

        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If
        Dim AcTypeFilteration As String = ""
        If chkDealer.Checked Then
            AcTypeFilteration += "'D',"
        End If
        If chkSmith.Checked Then
            AcTypeFilteration += "'G',"
        End If
        If chkInternal.Checked Then
            AcTypeFilteration += "'I',"
        End If
        If chkOthers.Checked Then
            AcTypeFilteration += "'O',"
        End If
        If AcTypeFilteration <> "" Then
            AcTypeFilteration = Mid(AcTypeFilteration, 1, AcTypeFilteration.Length - 1)
        End If
        If (cmbSmith_MAN.Text = "ALL" And chkMultiSelect.Checked) Or (CmbSmith.Text = "ALL" And chkMultiSelect.Checked = False) Then
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD"
            strSql += " WHERE ISNULL(LEDPRINT,'') <> 'N' AND ISNULL(ACTIVE,'Y') <> 'H' AND ACTYPE IN ('G','D','I')"
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
        ElseIf cmbSmith_MAN.Text <> "" And chkMultiSelect.Checked Then
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
        ElseIf CmbSmith.Text <> "" And chkMultiSelect.Checked = False Then
            cmbAccName = "'" & CmbSmith.Text & "'"
        End If

        Dim trantype As String = ""
        Dim selTran As String = ""
        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        trantype = GetTranType(selTran)
        Dim WitApproval As String = ""
        If chkCmbTranType.Text = "ALL" Then
            WitApproval = "IAP','RAP"
        End If

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & Sysid & "SMITHBALDET')>0 DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(75),'OPENING..')PARTICULAR,CONVERT(INT,NULL) TRANNO,CONVERT(VARCHAR(12),NULL) TRANTYPE,CONVERT(VARCHAR(12),NULL)TDATE,CONVERT(VARCHAR(12),NULL)BILLDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),NULL)[DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,0),CASE WHEN SUM(IPCS)-SUM(RPCS) > 0 THEN SUM(IPCS)-SUM(RPCS) ELSE 0 END) AS IPCS "
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IGRSWT)-SUM(RGRSWT) > 0 THEN SUM(IGRSWT)-SUM(RGRSWT) ELSE 0 END) AS IGRSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(INETWT)-SUM(RNETWT) > 0 THEN SUM(INETWT)-SUM(RNETWT) ELSE 0 END) AS INETWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IPUREWT)-SUM(RPUREWT) > 0 THEN SUM(IPUREWT)-SUM(RPUREWT) ELSE 0 END) AS IPUREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN SUM(ITOUCH)-SUM(RTOUCH) > 0 THEN SUM(ITOUCH)-SUM(RTOUCH) ELSE 0 END) AS ITOUCH"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,0),CASE WHEN SUM(RPCS)-SUM(IPCS) > 0 THEN SUM(RPCS)-SUM(IPCS) ELSE 0 END) AS RPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RGRSWT)-SUM(IGRSWT) > 0 THEN SUM(RGRSWT)-SUM(IGRSWT) ELSE 0 END) AS RGRSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RNETWT)-SUM(INETWT) > 0 THEN SUM(RNETWT)-SUM(INETWT) ELSE 0 END) AS RNETWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RPUREWT)-SUM(IPUREWT) > 0 THEN SUM(RPUREWT)-SUM(IPUREWT) ELSE 0 END) AS RPUREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN SUM(RTOUCH)-SUM(ITOUCH) > 0 THEN SUM(RTOUCH)-SUM(ITOUCH) ELSE 0 END) AS RTOUCH"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,0),CASE WHEN SUM(IPCS)-SUM(RPCS) > 0 THEN SUM(IPCS)-SUM(RPCS) ELSE 0 END) AS IPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,0),CASE WHEN SUM(RPCS)-SUM(IPCS) > 0 THEN SUM(RPCS)-SUM(IPCS) ELSE 0 END) AS RPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN SUM(ISSUE)-SUM(RECEIPT) > 0 THEN SUM(ISSUE)-SUM(RECEIPT) ELSE 0 END) AS ISSUE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECEIPT)-SUM(ISSUE) > 0 THEN SUM(RECEIPT)-SUM(ISSUE) ELSE 0 END) AS RECEIPT"
        End If
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)DEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)CREDIT"
        strSql += vbCrLf + " ,2 RESULT,'  'COLHEAD,CONVERT(SMALLDATETIME,NULL)TRANDATE,CONVERT(SMALLDATETIME,NULL)BDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = OPE.METALID)AS METALNAME,CONVERT(VARCHAR(15),NULL)AS BATCHNO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),ACNAME)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),REMARK1)REMARK1 "
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),REMARK2)REMARK2"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT "
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS IGRSWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS INETWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS IPUREWT "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN 0 ELSE 0 END AS ITOUCH "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RGRSWT "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RNETWT "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RPUREWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN 0 ELSE 0 END AS RTOUCH"
        Else
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + "  CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS ISSUE"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RECEIPT"
            ElseIf rbtNetWeight.Checked Then
                strSql += vbCrLf + "  CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS ISSUE"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RECEIPT"
            Else
                strSql += vbCrLf + "  CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS ISSUE"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RECEIPT"
            End If
        End If
        Dim acfilter As String = ""

        acfilter = " WHERE ISNULL(LEDPRINT,'') <> 'N'"
        acfilter += " AND ISNULL(ACTIVE,'Y') <> 'H'"
        acfilter += " AND ACNAME IN (" & cmbAccName & ")"
        If LocalOutSt <> "" Then acfilter += " AND LOCALOUTST ='" & LocalOutSt & "'"
        If AcTypeFilteration <> "" Then acfilter += " AND ACTYPE IN (" & AcTypeFilteration & ")"
        strSql += vbCrLf + " ,0 DEBIT,0 CREDIT,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = O.CATCODE)AS METALID"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=O.ACCODE)ACNAME"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT O"
        strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If (ChkApproval.Checked = False And chkCmbTranType.Text = "ALL") Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'')<>'A'"
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'')='A'"
        End If
        If chkCmbTranType.Text = "INTERNAL TRANSFER" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='I' "
        End If
        If chkCmbTranType.Text <> "ALL" Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'')=''"
        End If
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND COMPANYID in(" & SelectedCompanyId & ")"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " PCS IPCS"
            If ChkWithWast.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END IGRSWT"
                strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END INETWT"
            Else
                strSql += vbCrLf + " ,GRSWT IGRSWT"
                strSql += vbCrLf + " ,NETWT INETWT"
            End If
            strSql += vbCrLf + " ,PUREWT AS IPUREWT,0 ITOUCH,0 RPCS,0 RGRSWT,0 RNETWT,0 PUREWT,0 TOUCH,0 DEBIT,0 CREDIT"
        Else
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " PCS IPCS,0 RPCS"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
                Else
                    strSql += vbCrLf + ",GRSWT  ISSUE"
                End If
                strSql += vbCrLf + ",0 RECEIPT,0 DEBIT,0 CREDIT"
            ElseIf rbtNetWeight.Checked Then
                strSql += vbCrLf + " PCS IPCS,0 RPCS"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
                Else
                    strSql += vbCrLf + ",NETWT AS ISSUE"
                End If
                strSql += vbCrLf + ",0 RECEIPT,0 DEBIT,0 CREDIT"
            Else
                strSql += vbCrLf + " PCS IPCS,0 RPCS ,PUREWT ISSUE,0 RECEIPT,0 DEBIT,0 CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=I.CATCODE"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITYMAST P ON P.PURITYID=C.PURITYID"
        strSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND COMPANYID in(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " 0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 TOUCH,PCS RPCS"
            If ChkWithWast.Checked Then
                strSql += vbCrLf + ",CASE WHEN TRANTYPE IN('RAP') THEN GRSWT ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RGRSWT"
                strSql += vbCrLf + ",CASE WHEN TRANTYPE IN('RAP') THEN NETWT ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RNETWT"
            Else
                strSql += vbCrLf + ",GRSWT RGRSWT"
                strSql += vbCrLf + ",NETWT RNETWT"
            End If
            strSql += vbCrLf + ",PUREWT AS RPUREWT,0 RTOUCH,0 DEBIT,0 CREDIT"
        Else
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " 0 IPCS,PCS RPCS,0 ISSUE"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + ",CASE WHEN TRANTYPE IN('RAP') THEN GRSWT ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
                Else
                    strSql += vbCrLf + ",GRSWT RECEIPT"
                End If
                strSql += vbCrLf + ",0 DEBIT,0 CREDIT"
            ElseIf rbtNetWeight.Checked Then
                strSql += vbCrLf + " 0 IPCS,PCS RPCS,0 ISSUE"
                strSql += vbCrLf + ",CASE WHEN TRANTYPE IN('RAP') THEN NETWT ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
                strSql += vbCrLf + ",0 DEBIT,0 CREDIT"
            Else
                strSql += vbCrLf + " 0 IPCS,PCS RPCS,0 ISSUE,PUREWT RECEIPT,0 DEBIT,0 CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS METALID"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND R.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND R.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " S.STNPCS AS IPCS,CONVERT(NUMERIC(15,3),S.STNWT) AS IGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) AS INETWT,0 IPUREWT,0 ITOUCH,0 RPCS"
            strSql += vbCrLf + " ,0 RGRSWT,0 RNETWT,0 PUREWT,0 RTOUCH,0 AS DEBIT,0 AS CREDIT"
        Else
            strSql += vbCrLf + " S.STNPCS AS IPCS,0 RPCS,S.STNWT AS ISSUE,0 RECEIPT,0 AS DEBIT,0 AS CREDIT"
        End If
        strSql += vbCrLf + " ,ME.METALID"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND S.COMPANYID in(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        If chkRunningBal.Checked Then
            strSql += vbCrLf + "  0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 ITOUCH,S.STNPCS AS RPCS"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) AS RGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) RNETWT,0 RPUREWT,0 RTOUCH,0 AS DEBIT,0 AS CREDIT"
        Else
            strSql += vbCrLf + " 0 IPCS,S.STNPCS AS RPCS,0 ISSUE,S.STNWT AS RECEIPT,0 AS DEBIT,0 AS CREDIT"
        End If
        strSql += vbCrLf + " ,ME.METALID"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND S.COMPANYID IN(" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + " )OPE"
        strSql += vbCrLf + " GROUP BY METALID,ACNAME,REMARK1,REMARK2"
        strSql += vbCrLf + " UNION ALL"

        '020825
        'strSql += vbCrLf + " /** TRANSACTION **/"
        'strSql += vbCrLf + " SELECT isnull((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE),'')+case WHEN I.tranflag in('WC','AC') AND I.TRANTYPE = 'IRC' THEN 'RATE CUT @'+CONVERT(VARCHAR(10),I.RATE)+'/-' END   AS PARTICULAR"
        'strSql += vbCrLf + " ,I.TRANNO,CASE WHEN I.TRANTYPE = 'MI' THEN 'MISS' ELSE 'ISS' END TRANTYPE,CONVERT(VARCHAR,I.TRANDATE,103)TDATE ,CASE WHEN ISNULL(I.REFDATE,'')='' THEN CONVERT(VARCHAR,I.TRANDATE,103) ELSE CONVERT(VARCHAR,I.REFDATE,103) END BILLDATE"
        'strSql += vbCrLf + " ,CASE WHEN I.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
        'strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + ",I.PCS IPCS"
        '    If ChkWithWast.Checked Then
        '        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN I.GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE I.GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END IGRSWT"
        '        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN I.NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE I.NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END INETWT"
        '    Else
        '        strSql += vbCrLf + ",I.GRSWT IGRSWT"
        '        strSql += vbCrLf + ",I.NETWT INETWT"
        '    End If
        '    strSql += vbCrLf + ",CASE WHEN ISNULL(A.ACCODE,'') = '' THEN I.PUREWT ELSE 0 END IPUREWT,CONVERT(NUMERIC(12,2),TOUCH) ITOUCH,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RTOUCH"
        '    strSql += vbCrLf + ",CASE WHEN ISNULL(A.ACCODE,'') = '' THEN (CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN I.AMOUNT ELSE 0 END) ELSE 0 END AS DEBIT,0 CREDIT"
        'Else
        '    If rbtGrossWeight.Checked Then
        '        strSql += vbCrLf + " ,I.PCS IPCS,0 RPCS"
        '        If ChkWithWast.Checked Then
        '            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN I.GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE I.GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
        '        Else
        '            strSql += vbCrLf + ",I.GRSWT ISSUE"
        '        End If
        '        strSql += vbCrLf + " ,0 RECEIPT"
        '        strSql += vbCrLf + ",CASE WHEN ISNULL(A.ACCODE,'') = '' THEN (CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN I.AMOUNT ELSE 0 END) ELSE 0 END AS DEBIT,0 CREDIT"
        '    ElseIf rbtNetWeight.Checked Then
        '        strSql += vbCrLf + " ,I.PCS IPCS,0 RPCS"
        '        If ChkWithWast.Checked Then
        '            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN I.NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE I.NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
        '        Else
        '            strSql += vbCrLf + " ,I.NETWT ISSUE"
        '        End If
        '        strSql += vbCrLf + " ,0 RECEIPT,CASE WHEN ISNULL(A.ACCODE,'') = '' THEN (CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN I.AMOUNT ELSE 0 END) ELSE 0 END AS DEBIT,0 CREDIT"
        '    Else
        '        strSql += vbCrLf + " ,I.PCS IPCS,0 RPCS,CASE WHEN ISNULL(A.ACCODE,'') = '' THEN I.PUREWT ELSE 0 END ISSUE,0 RECEIPT,CASE WHEN ISNULL(A.ACCODE,'') = '' THEN (CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN I.AMOUNT ELSE 0 END) ELSE 0 END AS DEBIT,0 CREDIT"
        '    End If
        'End If
        'strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,I.TRANDATE,I.REFDATE AS BDATE"
        'strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME"
        'strSql += vbCrLf + " ,I.BATCHNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        'strSql += vbCrLf + " ,I.REMARK1,I.REMARK2"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITYMAST P ON P.PURITYID=C.PURITYID"
        'strSql += vbCrLf + " Left JOIN " & cnStockDb & "..ACCTRAN A ON I.BATCHNO = A.BATCHNO And A.ACCODE = 'CASH'"
        'strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        'If trantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & trantype & ")"
        'If ChkApproval.Checked = False Then
        '    strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        'End If
        'strSql += vbCrLf + " AND I.COMPANYID IN(  " & SelectedCompanyId & "  )"
        'strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " SELECT ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE),'')++case WHEN TRANFLAG IN('AC','WC') AND R.TRANTYPE = 'RRC' THEN 'RATE CUT @'+CONVERT(VARCHAR(10),RATE)+'/-' END  AS PARTICULAR"
        'strSql += vbCrLf + " ,TRANNO,'REC' TRANTYPE,CONVERT(VARCHAR,TRANDATE,103)TDATE,CONVERT(VARCHAR,REFDATE,103)BILLDATE"
        'strSql += vbCrLf + " ,CASE WHEN R.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID)"
        'strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID) END AS [DESCRIPTION]"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + ",0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 ITOUCH,PCS RPCS"
        '    If ChkWithWast.Checked Then
        '        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('RAP') THEN GRSWT "
        '        strSql += vbCrLf + " ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RGRSWT"
        '        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('RAP') THEN NETWT "
        '        strSql += vbCrLf + " ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RNETWT"
        '    Else
        '        strSql += vbCrLf + ",GRSWT RGRSWT"
        '        strSql += vbCrLf + ",NETWT RNETWT"
        '    End If
        '    strSql += vbCrLf + ",PUREWT RPUREWT"
        '    strSql += vbCrLf + " ,CONVERT(NUMERIC(12,2),TOUCH) RTOUCH,0 DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') "
        '    strSql += vbCrLf + " THEN AMOUNT ELSE 0 END AS CREDIT"
        'Else
        '    If rbtGrossWeight.Checked Then
        '        strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE"
        '        If ChkWithWast.Checked Then
        '            strSql += vbCrLf + ",CASE WHEN R.TRANTYPE IN('RAP') THEN GRSWT ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
        '        Else
        '            strSql += vbCrLf + ",GRSWT RECEIPT"
        '        End If
        '        strSql += vbCrLf + ",0DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS  CREDIT"
        '    ElseIf rbtNetWeight.Checked Then
        '        strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE"
        '        If ChkWithWast.Checked Then
        '            strSql += vbCrLf + ",CASE WHEN R.TRANTYPE IN('RAP') THEN NETWT ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
        '        Else
        '            strSql += vbCrLf + ",NETWT RECEIPT"
        '        End If
        '        strSql += vbCrLf + ",0 DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS  CREDIT"
        '    Else
        '        strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE,PUREWT RECEIPT,0 DEBIT"
        '        strSql += vbCrLf + ",CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS CREDIT"
        '    End If
        'End If
        'strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,TRANDATE,REFDATE AS BDATE"
        ''strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METALNAME"
        'strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METALNAME"
        'strSql += vbCrLf + " ,BATCHNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME ,'N' TFILTER"
        'strSql += vbCrLf + " ,REMARK1,REMARK2"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND R.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        'If trantype <> "" Then strSql += vbCrLf + " AND R.TRANTYPE IN (" & trantype & ")"
        'If ChkApproval.Checked = False Then
        '    strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('" & WitApproval & "')"
        'End If
        'strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        'strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,S.TRANNO,'ISS' TRANTYPE,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,CONVERT(VARCHAR,I.REFDATE,103)BILLDATE,IM.ITEMNAME [DESCRIPTION]"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,S.STNPCS AS IPCS,CONVERT(NUMERIC(15,3),S.STNWT) IGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) INETWT,0 IPUREWT,0 ITOUCH,0 AS RPCS,0 RGRSWT,0 RNETWT,0 PUREWT,0 RTOUCH,0 DEBIT,0 CREDIT"
        'Else
        '    strSql += vbCrLf + " ,S.STNPCS AS IPCS,0 AS RPCS,S.STNWT ISSUE,0 RECEIPT,0 DEBIT,0 CREDIT"
        'End If
        'strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,S.TRANDATE,I.REFDATE AS BDATE,ME.METALNAME AS METALNAME,S.BATCHNO"
        'strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        'strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        'strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        'strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        'strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        'If ChkApproval.Checked = False Then
        '    strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        'End If
        'strSql += vbCrLf + " AND S.COMPANYID IN(  " & SelectedCompanyId & "  )"
        'strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,S.TRANNO,'REC' TRANTYPE,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,CONVERT(VARCHAR,I.REFDATE,103)BILLDATE,IM.ITEMNAME [DESCRIPTION]"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 ITOUCH,S.STNPCS AS RPCS,CONVERT(NUMERIC(15,3),S.STNWT) RGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) RNETWT,0 PUREWT,0 RTOUCH,0 DEBIT,0 CREDIT"
        'Else
        '    strSql += vbCrLf + " ,0 IPCS,S.STNPCS AS RPCS,0 ISSUE,S.STNWT RECEIPT,0 DEBIT,0 CREDIT"
        'End If
        'strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,S.TRANDATE,I.REFDATE AS BDATE,ME.METALNAME AS METALNAME,S.BATCHNO"
        'strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        'strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        'strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        'strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        'strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        'strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        'If ChkApproval.Checked = False Then
        '    strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('" & WitApproval & "')"
        'End If
        'If LocalOutSt <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST ='" & LocalOutSt & "')"
        'strSql += vbCrLf + " AND S.COMPANYID IN(  " & SelectedCompanyId & "  )"
        'strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"

        strSql += vbCrLf + " /** TRANSACTION **/"
        strSql += vbCrLf + " SELECT isnull((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE),'')+case WHEN tranflag in('WC','AC') AND I.TRANTYPE = 'IRC' THEN 'RATE CUT @'+CONVERT(VARCHAR(10),RATE)+'/-' END   AS PARTICULAR"
        strSql += vbCrLf + " ,TRANNO,CASE WHEN I.TRANTYPE = 'MI' THEN 'MISS' ELSE 'ISS' END TRANTYPE,CONVERT(VARCHAR,TRANDATE,103)TDATE ,CASE WHEN ISNULL(REFDATE,'')='' THEN CONVERT(VARCHAR,TRANDATE,103) ELSE CONVERT(VARCHAR,REFDATE,103) END BILLDATE"
        strSql += vbCrLf + " ,CASE WHEN I.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + ",PCS IPCS"
            If ChkWithWast.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END IGRSWT"
                strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END INETWT"
            Else
                strSql += vbCrLf + ",GRSWT IGRSWT"
                strSql += vbCrLf + ",NETWT INETWT"
            End If
            strSql += vbCrLf + ",PUREWT IPUREWT,CONVERT(NUMERIC(12,2),TOUCH) ITOUCH,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT,0 RTOUCH"
            strSql += vbCrLf + ",CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS DEBIT,0 CREDIT"
        Else
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " ,PCS IPCS,0 RPCS"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN GRSWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE GRSWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
                Else
                    strSql += vbCrLf + ",GRSWT ISSUE"
                End If
                strSql += vbCrLf + " ,0 RECEIPT"
                strSql += vbCrLf + ",CASE WHEN ISNULL(TRANFLAG,'')NOT IN('AC','WC') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            ElseIf rbtNetWeight.Checked Then
                strSql += vbCrLf + " ,PCS IPCS,0 RPCS"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','IAP') AND P.METALTYPE='O' THEN NETWT+ISNULL(I.ALLOY,0)+ISNULL(WASTAGE,0) ELSE NETWT+ISNULL(WASTAGE,0)-ISNULL(I.ALLOY,0) END ISSUE"
                Else
                    strSql += vbCrLf + " ,NETWT ISSUE"
                End If
                strSql += vbCrLf + " ,0 RECEIPT,CASE WHEN ISNULL(I.TRANFLAG,'')NOT IN('AC','WC') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            Else
                strSql += vbCrLf + " ,PCS IPCS,0 RPCS,PUREWT ISSUE,0 RECEIPT,CASE WHEN ISNULL(I.TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,TRANDATE,REFDATE AS BDATE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME"
        strSql += vbCrLf + " ,BATCHNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITYMAST P ON P.PURITYID=C.PURITYID"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE),'')++case WHEN TRANFLAG IN('AC','WC') AND R.TRANTYPE = 'RRC' THEN 'RATE CUT @'+CONVERT(VARCHAR(10),RATE)+'/-' END  AS PARTICULAR"
        strSql += vbCrLf + " ,TRANNO,'REC' TRANTYPE,CONVERT(VARCHAR,TRANDATE,103)TDATE,CONVERT(VARCHAR,REFDATE,103)BILLDATE"
        strSql += vbCrLf + " ,CASE WHEN R.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID) END AS [DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + ",0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 ITOUCH,PCS RPCS"
            If ChkWithWast.Checked Then
                strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('RAP') THEN GRSWT "
                strSql += vbCrLf + " ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RGRSWT"
                strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('RAP') THEN NETWT "
                strSql += vbCrLf + " ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RNETWT"
            Else
                strSql += vbCrLf + ",GRSWT RGRSWT"
                strSql += vbCrLf + ",NETWT RNETWT"
            End If
            strSql += vbCrLf + ",PUREWT RPUREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(12,2),TOUCH) RTOUCH,0 DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') "
            strSql += vbCrLf + " THEN AMOUNT ELSE 0 END AS CREDIT"
        Else
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + ",CASE WHEN R.TRANTYPE IN('RAP') THEN GRSWT ELSE GRSWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
                Else
                    strSql += vbCrLf + ",GRSWT RECEIPT"
                End If
                strSql += vbCrLf + ",0DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS  CREDIT"
            ElseIf rbtNetWeight.Checked Then
                strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE"
                If ChkWithWast.Checked Then
                    strSql += vbCrLf + ",CASE WHEN R.TRANTYPE IN('RAP') THEN NETWT ELSE NETWT+ISNULL(WASTAGE,0)+ISNULL(R.ALLOY,0) END RECEIPT"
                Else
                    strSql += vbCrLf + ",NETWT RECEIPT"
                End If
                strSql += vbCrLf + ",0 DEBIT,CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS  CREDIT"
            Else
                strSql += vbCrLf + " ,0 IPCS,PCS RPCS,0 ISSUE,PUREWT RECEIPT,0 DEBIT"
                strSql += vbCrLf + ",CASE WHEN ISNULL(TRANFLAG,'') NOT IN('AC','WC') THEN AMOUNT ELSE 0 END AS CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,TRANDATE,REFDATE AS BDATE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METALNAME"
        strSql += vbCrLf + " ,BATCHNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=R.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND R.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND R.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + " AND R.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,S.TRANNO,'ISS' TRANTYPE,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,CONVERT(VARCHAR,I.REFDATE,103)BILLDATE,IM.ITEMNAME [DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,S.STNPCS AS IPCS,CONVERT(NUMERIC(15,3),S.STNWT) IGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) INETWT,0 IPUREWT,0 ITOUCH,0 AS RPCS,0 RGRSWT,0 RNETWT,0 PUREWT,0 RTOUCH,0 DEBIT,0 CREDIT"
        Else
            strSql += vbCrLf + " ,S.STNPCS AS IPCS,0 AS RPCS,S.STNWT ISSUE,0 RECEIPT,0 DEBIT,0 CREDIT"
        End If
        strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,S.TRANDATE,I.REFDATE AS BDATE,ME.METALNAME AS METALNAME,S.BATCHNO"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND I.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + " AND S.COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,S.TRANNO,'REC' TRANTYPE,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,CONVERT(VARCHAR,I.REFDATE,103)BILLDATE,IM.ITEMNAME [DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT,0 ITOUCH,S.STNPCS AS RPCS,CONVERT(NUMERIC(15,3),S.STNWT) RGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) RNETWT,0 PUREWT,0 RTOUCH,0 DEBIT,0 CREDIT"
        Else
            strSql += vbCrLf + " ,0 IPCS,S.STNPCS AS RPCS,0 ISSUE,S.STNWT RECEIPT,0 DEBIT,0 CREDIT"
        End If
        strSql += vbCrLf + " ,3 RESULT,' 'COLHEAD,S.TRANDATE,I.REFDATE AS BDATE,ME.METALNAME AS METALNAME,S.BATCHNO"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)ACNAME ,'N' TFILTER"
        strSql += vbCrLf + " ,''REMARK1,''REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        If LocalOutSt <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST ='" & LocalOutSt & "')"
        strSql += vbCrLf + " AND S.COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND S.TRANTYPE NOT IN ('IPU','RPU','AI','AR')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If ChktransactionOnly.Checked = True And chkRunningBal.Checked = True Then
            strSql = " DELETE FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE PARTICULAR='OPENING..' AND RESULT=2"
            strSql += vbCrLf + " AND ACNAME NOT IN(SELECT DISTINCT ACNAME FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT IN(1,3,4))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If


        strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET ADD SNO INT IDENTITY(1,1)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT')>0 DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT"
        strSql += vbCrLf + " SELECT ACCODE,BATCHNO,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE 0 END CREDIT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT ACCODE,BATCHNO,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'strSql += vbCrLf + " AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET)"
        If LocalOutSt <> "" Then strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST ='" & LocalOutSt & "')"
        strSql += vbCrLf + " GROUP BY BATCHNO,ACCODE"
        strSql += vbCrLf + " )X"
        'strSql += vbCrLf + " "
        'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET DEBIT = AM.DEBIT,CREDIT = AM.CREDIT"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET AS DE,TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT AS AM"
        'strSql += vbCrLf + " WHERE DE.BATCHNO = AM.BATCHNO"
        'strSql += vbCrLf + " AND DE.SNO = (SELECT TOP 1 SNO FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE BATCHNO = AM.BATCHNO ORDER BY SNO)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'If ChktransactionOnly.Checked = False Then
        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT ACNAME,'','OPENING AMOUNT..',DEBIT,CREDIT,1,'','Y'TFILTER"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT ACNAME,CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN -1*SUM(AMOUNT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=O.ACCODE)ACNAME,DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE O"
        strSql += vbCrLf + " where O.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        If ChktransactionOnly.Checked Then
            strSql += vbCrLf + "     AND ACCODE IN(SELECT DISTINCT ACCODE FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT)"
        End If
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)ACNAME,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + " 	WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND A.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If ChktransactionOnly.Checked Then
            strSql += vbCrLf + "     AND ACCODE IN(SELECT DISTINCT ACCODE FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALAMT)"
        End If
        'strSql += vbCrLf + "     AND BATCHNO IN "
        'strSql += vbCrLf + "         ("
        'strSql += vbCrLf + "         SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE"
        'strSql += vbCrLf + "         WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + "         AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        'If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN (" & trantype & ")"
        'If ChkApproval.Checked = False Then
        '    strSql += vbCrLf + " AND TRANTYPE NOT IN ('" & WitApproval & "')"
        'End If
        'strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        'strSql += vbCrLf + "         UNION"
        'strSql += vbCrLf + "         SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT"
        'strSql += vbCrLf + "         WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + "         AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        'If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN (" & trantype & ")"
        'strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        'strSql += vbCrLf + "          )"
        strSql += vbCrLf + " 	GROUP BY BATCHNO,ACCODE"
        strSql += vbCrLf + " 	)AMT "
        strSql += vbCrLf + " GROUP BY ACNAME "
        strSql += vbCrLf + " )X WHERE NOT(DEBIT = 0 AND CREDIT = 0)"
        strSql += vbCrLf + " "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        'End If
        If ChkWithDirectPur.Checked Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,TRANNO,TRANTYPE,TDATE,TRANDATE,PARTICULAR,IPCS,RPCS"
            If chkRunningBal.Checked Then
                strSql += vbCrLf + " ,IGRSWT,INETWT,RGRSWT,RNETWT"
            End If
            strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER,REMARK1,REMARK2)" + vbCrLf
            strSql += vbCrLf + " SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=T.ACCODE)ACNAME,'',TRANNO ,'ISS' TRANTYPE,CONVERT(VARCHAR,TRANDATE,103)AS TDATE,TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA) AS PARTICULAR"
            strSql += vbCrLf + " ,PCS AS IPCS,0 RPCS"
            If chkRunningBal.Checked Then
                strSql += vbCrLf + " ,GRSWT IGRSWT,NETWT INETWT,0 RGRSWT,0 RNETWT"
            End If
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT"
            strSql += vbCrLf + " ,2 RESULT,''AS COLHEAD,'Y'TFILTER"
            strSql += vbCrLf + " ,REMARK1,REMARK2"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS T"
            strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
            strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND T.COMPANYID IN(  " & SelectedCompanyId & "  )"
            strSql += vbCrLf + " AND T.BATCHNO IN"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'IPU'"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=T.ACCODE)ACNAME,'',TRANNO ,'REC' TRANTYPE,CONVERT(VARCHAR,TRANDATE,103)AS TDATE,TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA) AS PARTICULAR"
            strSql += vbCrLf + " ,0 IPCS,PCS RPCS"
            If chkRunningBal.Checked Then
                strSql += vbCrLf + " ,0 IGRSWT,0 INETWT,GRSWT RGRSWT,NETWT RNETWT "
            End If
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT"
            strSql += vbCrLf + " ,2 RESULT,''AS COLHEAD,'Y'TFILTER"
            strSql += vbCrLf + " ,REMARK1,REMARK2"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS T"
            strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
            strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND T.COMPANYID IN(  " & SelectedCompanyId & "  )"
            strSql += vbCrLf + " AND T.BATCHNO IN"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'RPU'"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If


        strSql = " alter table TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET alter column REMARK1 varchar(100)"
        strSql += " alter table TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET alter column REMARK2 varchar(100)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(PARTICULAR,ACNAME,TRANNO,TDATE,IPCS,RPCS"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,IPUREWT,RPUREWT"
        Else
            strSql += vbCrLf + " ,ISSUE,RECEIPT"
        End If
        strSql += vbCrLf + ",DEBIT,CREDIT,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER,REMARK1,REMARK2)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
        'strSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE,0,0,0,0"
        strSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE,0,0"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' AND TRANFLAG <> 'AC' THEN PUREWT ELSE 0 END AS IPUREWT"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' AND TRANFLAG <> 'AC' THEN PUREWT ELSE 0 END AS RPUREWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' AND TRANFLAG <> 'AC' THEN PUREWT ELSE 0 END AS ISSUE"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' AND TRANFLAG <> 'AC' THEN PUREWT ELSE 0 END AS RECEIPT"
        End If
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,2 RESULT,TRANDATE,''METALNAME,BATCHNO,''COLHEAD,'N'TFILTER"
        strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=T.BATCHNO AND REMARK1<>'')REMARK1"
        strSql += vbCrLf + " ,(SELECT TOP 1 REMARK2 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=T.BATCHNO AND REMARK2<>'')REMARK2"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + "     AND COMPANYID IN(  " & SelectedCompanyId & "  )"

        '230825
        strSql += vbCrLf + "     AND BATCHNO NOT IN "
        strSql += vbCrLf + "     (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE"
        strSql += vbCrLf + " 	 WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "     AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN (" & trantype & ")"
        If ChkApproval.Checked = False Then
            strSql += vbCrLf + " AND TRANTYPE NOT IN ('" & WitApproval & "')"
        End If
        strSql += vbCrLf + "         AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + "         UNION"
        strSql += vbCrLf + "         SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT"
        strSql += vbCrLf + " 	    WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "         AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN (" & trantype & ")"
        strSql += vbCrLf + " AND COMPANYID IN(  " & SelectedCompanyId & "  )"
        strSql += vbCrLf + "          )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkTranNowise.Checked = True Then

            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(PARTICULAR,ACNAME,TRANNO,TDATE,BILLDATE,TRANTYPE,IPCS,RPCS"
            If chkRunningBal.Checked Then
                strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT,ITOUCH,RGRSWT,RNETWT,RPUREWT,RTOUCH"
            Else
                strSql += vbCrLf + ",ISSUE,RECEIPT"
            End If
            strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,TRANDATE,BDATE,METALNAME,BATCHNO,COLHEAD,TFILTER)"
            strSql += vbCrLf + " SELECT PARTICULAR,ACNAME,TRANNO,TDATE,BILLDATE,TRANTYPE,SUM(ISNULL(IPCS,0)),SUM(ISNULL(RPCS,0))"
            If chkRunningBal.Checked Then
                strSql += vbCrLf + " ,SUM(ISNULL(IGRSWT,0)),SUM(ISNULL(INETWT,0)),SUM(ISNULL(IPUREWT,0)),SUM(ISNULL(ITOUCH,0)),SUM(ISNULL(RGRSWT,0)),SUM(ISNULL(RNETWT,0)),SUM(ISNULL(RPUREWT,0)),SUM(ISNULL(RTOUCH,0))"
            Else
                strSql += vbCrLf + " ,SUM(ISNULL(ISSUE,0)),SUM(ISNULL(RECEIPT,0))"
            End If
            strSql += vbCrLf + " ,SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),4 RESULT,TRANDATE,BDATE,METALNAME,BATCHNO,COLHEAD,TFILTER "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT=3 GROUP BY PARTICULAR,ACNAME,TRANNO,TDATE,BILLDATE,TRANTYPE,TRANDATE,BDATE,METALNAME,BATCHNO,COLHEAD,TFILTER"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " DELETE FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT=3"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

        End If
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT DISTINCT ACNAME,'' AS METALNAME,ACNAME,0 RESULT,'T'COLHEAD,'N' TFILTER FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(PARTICULAR,METALNAME,ACNAME,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,ACNAME,1 RESULT,'T1'COLHEAD,'N' TFILTER FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE ISNULL(METALNAME,'')<>'' GROUP BY ACNAME,METALNAME "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,IPCS,RPCS"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT,RGRSWT,RNETWT,RPUREWT"
        Else
            strSql += vbCrLf + ",ISSUE,RECEIPT"
        End If
        strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT ACNAME,METALNAME,'SUB TOTAL',SUM(ISNULL(IPCS,0)),SUM(ISNULL(RPCS,0))"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,SUM(ISNULL(IGRSWT,0)),SUM(ISNULL(INETWT,0)),SUM(ISNULL(IPUREWT,0)),SUM(ISNULL(RGRSWT,0)),SUM(ISNULL(RNETWT,0)),SUM(ISNULL(RPUREWT,0))"
        Else
            strSql += vbCrLf + " ,SUM(ISNULL(ISSUE,0)),SUM(ISNULL(RECEIPT,0))"
        End If
        strSql += vbCrLf + " ,SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),5 RESULT,'S'COLHEAD,'N'TFILTER"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET  WHERE ISNULL(METALNAME,'')<>''"
        strSql += vbCrLf + " GROUP BY ACNAME,METALNAME"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ACNAME,METALNAME,'SUB TOTAL',SUM(ISNULL(IPCS,0)),SUM(ISNULL(RPCS,0))"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,SUM(ISNULL(IGRSWT,0)),SUM(ISNULL(INETWT,0)),SUM(ISNULL(IPUREWT,0)),SUM(ISNULL(RGRSWT,0)),SUM(ISNULL(RNETWT,0)),SUM(ISNULL(RPUREWT,0))"
        Else
            strSql += vbCrLf + " ,SUM(ISNULL(ISSUE,0)),SUM(ISNULL(RECEIPT,0))"
        End If
        strSql += vbCrLf + " ,SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),5 RESULT,'S'COLHEAD,'Y'TFILTER"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET  WHERE ISNULL(METALNAME,'')=''"
        strSql += vbCrLf + " GROUP BY ACNAME,METALNAME HAVING (SUM(ISNULL(DEBIT,0))<>0 OR SUM(ISNULL(CREDIT,0))<>0)"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,DESCRIPTION,IPCS,RPCS"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT,RGRSWT,RNETWT,RPUREWT"
        Else
            strSql += vbCrLf + " ,ISSUE,RECEIPT"
        End If
        strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT ACNAME,METALNAME,' ','BALANCE 'DESCRIPTION"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) > 0 THEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) WHEN SUM(ISNULL(IPCS,0))=SUM(ISNULL(RPCS,0)) THEN 0 END IPCS"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) > 0 THEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) END RPCS"
        If chkRunningBal.Checked Then
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) > 0 THEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) WHEN SUM(ISNULL(IGRSWT,0))=SUM(ISNULL(RGRSWT,0)) THEN 0 END IGRSWT"
            Else
                strSql += vbCrLf + " ,NULL IGRSWT"
            End If
            If rbtNetWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) > 0 THEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) WHEN SUM(ISNULL(INETWT,0))=SUM(ISNULL(RNETWT,0)) THEN 0 END INETWT"
            Else
                strSql += vbCrLf + " ,NULL INETWT"
            End If
            If rbtPureWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) > 0 THEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) WHEN SUM(ISNULL(IPUREWT,0))=SUM(ISNULL(RPUREWT,0)) THEN 0 END IPUREWT"
            Else
                strSql += vbCrLf + " ,NULL IPUREWT"
            End If
            If rbtGrossWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0)) > 0 THEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0))  END RGRSWT"
            Else
                strSql += vbCrLf + " ,NULL RGRSWT"
            End If
            If rbtNetWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) > 0 THEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) END RNETWT"
            Else
                strSql += vbCrLf + " ,NULL RNETWT"
            End If
            If rbtPureWeight.Checked Then
                strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) > 0 THEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) END RPUREWT"
            Else
                strSql += vbCrLf + " ,NULL RPUREWT"
            End If
        Else
            strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) WHEN SUM(ISNULL(ISSUE,0))=SUM(ISNULL(RECEIPT,0)) THEN 0 END ISSUE"
            strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) > 0 THEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) END RECEIPT"
        End If
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) > 0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) WHEN SUM(ISNULL(DEBIT,0))=SUM(ISNULL(CREDIT,0)) THEN 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) > 0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) END CREDIT"
        strSql += vbCrLf + " ,6 RESULT,'G'COLHEAD,'N'TFILTER"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT = 5 AND TFILTER='N' AND ISNULL(METALNAME,'')<>''"
        strSql += vbCrLf + " GROUP BY ACNAME,METALNAME"
        'If ChktransactionOnly.Checked = False Then
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ACNAME,'ZZZZZZZZZZZZZ',' ','BALANCE 'DESCRIPTION"
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) > 0 THEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) WHEN SUM(ISNULL(IPCS,0))=SUM(ISNULL(RPCS,0)) THEN 0 END IPCS"
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) > 0 THEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) END RPCS"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) > 0 THEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) WHEN SUM(ISNULL(IGRSWT,0))=SUM(ISNULL(RGRSWT,0)) THEN 0 END IGRSWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) > 0 THEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) WHEN SUM(ISNULL(INETWT,0))=SUM(ISNULL(RNETWT,0)) THEN 0 END INETWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) > 0 THEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) WHEN SUM(ISNULL(IPUREWT,0))=SUM(ISNULL(RPUREWT,0)) THEN 0 END IPUREWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0)) > 0 THEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0))  END RGRSWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) > 0 THEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) END RNETWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) > 0 THEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) END RPUREWT"
        'Else
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) WHEN SUM(ISNULL(ISSUE,0))=SUM(ISNULL(RECEIPT,0)) THEN 0 END ISSUE"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) > 0 THEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) END RECEIPT"
        'End If
        strSql += vbCrLf + " ,NULL IPCS"
        strSql += vbCrLf + " ,NULL RPCS"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " ,NULL IGRSWT"
            strSql += vbCrLf + " ,NULL INETWT"
            strSql += vbCrLf + " ,NULL IPUREWT"
            strSql += vbCrLf + " ,NULL RGRSWT"
            strSql += vbCrLf + " ,NULL RNETWT"
            strSql += vbCrLf + " ,NULL RPUREWT"
        Else
            strSql += vbCrLf + " ,NULL ISSUE"
            strSql += vbCrLf + " ,NULL RECEIPT"
        End If
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) > 0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) WHEN SUM(ISNULL(DEBIT,0))=SUM(ISNULL(CREDIT,0)) THEN 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) > 0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) END CREDIT"
        strSql += vbCrLf + " ,7 RESULT,'G'COLHEAD,'Y'TFILTER"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT = 5 --AND TFILTER='Y' AND ISNULL(METALNAME,'')=''"
        strSql += vbCrLf + " GROUP BY ACNAME--,METALNAME"
        'End If
        'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,IPCS,RPCS"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT,RGRSWT,RNETWT,RPUREWT"
        'Else
        '    strSql += vbCrLf + " ,ISSUE,RECEIPT"
        'End If
        'strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        'strSql += vbCrLf + " SELECT 'ZZZZZZZZ','','GRAND TOTAL',SUM(ISNULL(IPCS,0)),SUM(ISNULL(RPCS,0))"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,SUM(ISNULL(IGRSWT,0)),SUM(ISNULL(INETWT,0)),SUM(ISNULL(IPUREWT,0)),SUM(ISNULL(RGRSWT,0)),SUM(ISNULL(RNETWT,0)),SUM(ISNULL(RPUREWT,0))"
        'Else
        '    strSql += vbCrLf + " ,SUM(ISNULL(ISSUE,0)),SUM(ISNULL(RECEIPT,0))"
        'End If
        'strSql += vbCrLf + ",SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),7 RESULT,'G'COLHEAD,'N'TFILTER"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT = 5 AND TFILTER='N'"

        'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET(ACNAME,METALNAME,PARTICULAR,DESCRIPTION,IPCS,RPCS"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT,RGRSWT,RNETWT,RPUREWT"
        'Else
        '    strSql += vbCrLf + " ,ISSUE,RECEIPT"
        'End If
        'strSql += vbCrLf + " ,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        'strSql += vbCrLf + " SELECT 'ZZZZZZZZZ','',' ','BALANCE 'DESCRIPTION"
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) > 0 THEN SUM(ISNULL(IPCS,0))-SUM(ISNULL(RPCS,0)) WHEN SUM(ISNULL(IPCS,0))=SUM(ISNULL(RPCS,0)) THEN 0 END IPCS"
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) > 0 THEN SUM(ISNULL(RPCS,0))-SUM(ISNULL(IPCS,0)) END RPCS"
        'If chkRunningBal.Checked Then
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) > 0 THEN SUM(ISNULL(IGRSWT,0))-SUM(ISNULL(RGRSWT,0)) WHEN SUM(ISNULL(IGRSWT,0))=SUM(ISNULL(RGRSWT,0)) THEN 0 END IGRSWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) > 0 THEN SUM(ISNULL(INETWT,0))-SUM(ISNULL(RNETWT,0)) WHEN SUM(ISNULL(INETWT,0))=SUM(ISNULL(RNETWT,0)) THEN 0 END INETWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) > 0 THEN SUM(ISNULL(IPUREWT,0))-SUM(ISNULL(RPUREWT,0)) WHEN SUM(ISNULL(IPUREWT,0))=SUM(ISNULL(RPUREWT,0)) THEN 0 END IPUREWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0)) > 0 THEN SUM(ISNULL(RGRSWT,0))-SUM(ISNULL(IGRSWT,0))  END RGRSWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) > 0 THEN SUM(ISNULL(RNETWT,0))-SUM(ISNULL(INETWT,0)) END RNETWT"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) > 0 THEN SUM(ISNULL(RPUREWT,0))-SUM(ISNULL(IPUREWT,0)) END RPUREWT"
        'Else
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) > 0 THEN SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) WHEN SUM(ISNULL(ISSUE,0))=SUM(ISNULL(RECEIPT,0)) THEN 0 END ISSUE"
        '    strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) > 0 THEN SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)) END RECEIPT"
        'End If
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) > 0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) WHEN SUM(ISNULL(DEBIT,0))=SUM(ISNULL(CREDIT,0)) THEN 0 END DEBIT"
        'strSql += vbCrLf + " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) > 0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) END CREDIT"
        'strSql += vbCrLf + " ,8 RESULT,'G'COLHEAD,'N'TFILTER"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET WHERE RESULT = 7 AND TFILTER='N'"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RPCS = NULL WHERE RPCS = 0"
        If chkRunningBal.Checked Then
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET IGRSWT = NULL WHERE IGRSWT = 0  AND RESULT<>6"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET INETWT = NULL WHERE INETWT = 0   AND RESULT<>6 "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET IPUREWT = NULL WHERE IPUREWT = 0   AND RESULT<>6"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET ITOUCH = NULL WHERE ITOUCH = 0   "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RGRSWT = NULL WHERE RGRSWT = 0   AND RESULT<>6"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RNETWT = NULL WHERE RNETWT = 0   AND RESULT<>6"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RPUREWT = NULL WHERE RPUREWT = 0   AND RESULT<>6"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RTOUCH = NULL WHERE RTOUCH = 0   "
        Else
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET ISSUE = NULL WHERE ISSUE = 0 "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET RECEIPT = NULL WHERE RECEIPT = 0 "
        End If
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET DEBIT = NULL WHERE DEBIT = 0 AND RESULT<>7"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET SET CREDIT = NULL WHERE CREDIT = 0 AND RESULT<>7"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkRunningBal.Checked Then
            strSql = " ;with CTE as("
            strSql += vbCrLf + " Select * From TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
            strSql += vbCrLf + " Where RESULT IN (2,3)"
            strSql += vbCrLf + " ),"
            strSql += vbCrLf + " CTE1 as("
            strSql += vbCrLf + " Select ACNAME, METALNAME, TRANDATE,TRANNO,SUM(ISNULL(RPUREWT, 0) - ISNULL(IPUREWT, 0)) OVER (PARTITION BY ACNAME, METALNAME ORDER BY TRANDATE,TRANNO) As RUNWT,"
            strSql += vbCrLf + " SUM(ISNULL(DEBIT, 0) - ISNULL(CREDIT, 0)) OVER (PARTITION BY ACNAME, METALNAME ORDER BY TRANDATE,TRANNO) As RUNBAL"
            strSql += vbCrLf + " From TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
            strSql += vbCrLf + " Where RESULT IN (2,3)"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " ,CTE2 as("
            strSql += vbCrLf + " Select DISTINCT PARTICULAR,cte.TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,INETWT,IPUREWT,ITOUCH,RPCS,RGRSWT,RNETWT,RPUREWT,RTOUCH,DEBIT,CREDIT,CTE1.RUNWT,CTE1.RUNBAL,RESULT,COLHEAD,cte.TRANDATE,BDATE,cte.METALNAME,"
            strSql += vbCrLf + " BATCHNO, cte.ACNAME, TFILTER, REMARK1, REMARK2, SNO from CTE"
            strSql += vbCrLf + " inner Join CTE1 on CTE.ACNAME = CTE1.ACNAME And CTE.METALNAME = CTE1.METALNAME And CTE.TRANDATE = CTE1.TRANDATE And CTE.TRANNO = CTE1.TRANNO"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " select DISTINCT PARTICULAR,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,INETWT,IPUREWT,ITOUCH,RPCS,RGRSWT,RNETWT,RPUREWT,RTOUCH,DEBIT,CREDIT,0 RUNWT,0 RUNBAL,RESULT,COLHEAD,TRANDATE,BDATE,"
            strSql += vbCrLf + " METALNAME,BATCHNO,ACNAME,TFILTER,REMARK1,REMARK2,SNO from TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
            strSql += vbCrLf + " WHERE 1=1 AND RESULT NOT IN (3)"

            '230825
            strSql += vbCrLf + " AND ISNULL(BATCHNO,'') NOT IN(Select DISTINCT BATCHNO from CTE inner Join CTE1 on CTE.ACNAME = CTE1.ACNAME And CTE.METALNAME = CTE1.METALNAME And CTE.TRANDATE = CTE1.TRANDATE And CTE.TRANNO = CTE1.TRANNO WHERE ISNULL(BATCHNO,'')<>''))"

            strSql += vbCrLf + " ,CTE3 as(select ROW_NUMBER()over(partition by batchno order by RUNBAL desc)Cnt,* from CTE2 where ISNULL(BATCHNO,'') <> '')"
            strSql += vbCrLf + " select PARTICULAR,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,INETWT,ITOUCH,IPUREWT,RPCS,RGRSWT,RNETWT,RTOUCH,RPUREWT,DEBIT,CREDIT,RUNWT,RUNBAL,RESULT,COLHEAD,TRANDATE,BDATE,METALNAME,"
            strSql += vbCrLf + " BATCHNO,ACNAME, TFILTER, REMARK1, REMARK2, SNO from CTE3 --where Cnt = 1"
            strSql += vbCrLf + " union"
            strSql += vbCrLf + " select PARTICULAR,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,INETWT,ITOUCH,IPUREWT,RPCS,RGRSWT,RNETWT,RTOUCH,RPUREWT,DEBIT,CREDIT,RUNWT,RUNBAL,RESULT,COLHEAD,TRANDATE,BDATE,METALNAME,"
            strSql += vbCrLf + " BATCHNO,ACNAME, TFILTER, REMARK1, REMARK2, SNO from CTE2 where isnull(BATCHNO,'') not in (select BATCHNO from CTE3 where Cnt = 2)"
        Else
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDET"
        End If
        strSql += vbCrLf + " ORDER BY ACNAME,METALNAME,RESULT"
        If chkTranno.Checked Then
            strSql += vbCrLf + " ,BDATE,TRANDATE,TRANNO"
            'strSql += vbCrLf + " ,BDATE,TRANNO"
        Else
            strSql += vbCrLf + " ,TRANDATE,TRANNO"
            'strSql += vbCrLf + " ,TRANNO"
        End If

        Dim dtGrid As New DataTable
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
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 7, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "SMITH BALANCE DETAILED"
        Dim tit As String = " SMITH BALANCE DETAILED REPORT" + vbCrLf
        tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkRunningBal.Checked = False Then
            tit += " BASED ON "
            If rbtGrossWeight.Checked Then
                tit += "GROSS WEIGHT"
            ElseIf rbtNetWeight.Checked Then
                tit += "NET WEIGHT"
            Else
                tit += "PURE WEIGHT"
            End If
        End If
        If chkCostName <> "" Then tit += vbCrLf & "COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        If chkRunningBal.Checked = False Then
            objGridShower.gridView.Columns("IPCS").HeaderText = "ISSUE"
            objGridShower.gridView.Columns("RPCS").HeaderText = "RECEIPT"
        End If
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.formuser = userId
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        With objGridShower.gridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            If dgvRow.Cells("TFILTER").Value.ToString = "Y" Then
                dgvRow.Cells("IPCS").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RPCS").Style.BackColor = Color.MistyRose
                If chkRunningBal.Checked Then
                    dgvRow.Cells("IGRSWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("INETWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("RGRSWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("RNETWT").Style.BackColor = Color.MistyRose
                End If
            End If
        Next
        objGridShower.gridView.Columns("PARTICULAR").Frozen = True
    End Sub
    Private Sub funcSpecificFormat()
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory, False)
        Dim chkCostName As String = GetSelectedCostId(chkLstCostCentre, False)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal, False)
        Dim SelectedCompanyId As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim cmbAccName As String = ""
        If chkCostName = "''" Then chkCostName = ""
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If
        Dim AcTypeFilteration As String = ""
        If chkDealer.Checked Then
            AcTypeFilteration += "'D',"
        End If
        If chkSmith.Checked Then
            AcTypeFilteration += "'G',"
        End If
        If chkInternal.Checked Then
            AcTypeFilteration += "'I',"
        End If
        If chkOthers.Checked Then
            AcTypeFilteration += "'O',"
        End If
        If AcTypeFilteration <> "" Then
            AcTypeFilteration = Mid(AcTypeFilteration, 1, AcTypeFilteration.Length - 1)
        End If
        If cmbSmith_MAN.Text <> "" And cmbSmith_MAN.Text <> "ALL" And chkMultiSelect.Checked Then
            Dim sql As String = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN (" & GetQryString(cmbSmith_MAN.Text) & ")"
            Dim dtAcname As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtAcname)
            If dtAcname.Rows.Count > 0 Then
                For i As Integer = 0 To dtAcname.Rows.Count - 1
                    cmbAccName += dtAcname.Rows(i).Item("ACCODE").ToString
                    cmbAccName += ","
                Next
                If cmbAccName <> "" Then
                    cmbAccName = Mid(cmbAccName, 1, cmbAccName.Length - 1)
                End If
            End If
        End If
        If CmbSmith.Text <> "" And CmbSmith.Text <> "ALL" And chkMultiSelect.Checked = False Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbSmith.Text & "'"
            cmbAccName = objGPack.GetSqlValue(strSql, , , )
        End If
        If cmbAccName.Trim = "" Then cmbAccName = "ALL"

        Dim trantype As String = ""
        Dim selTran As String = ""
        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        trantype = GetTranType(selTran)
        If trantype = "" Then trantype = "''"
        Dim WitApproval As String = ""
        If chkCmbTranType.Text = "ALL" Then
            WitApproval = "IAP','RAP"
        ElseIf chkCmbTranType.Text.Contains("APPROVAL") Then
            WitApproval = "IAP','RAP"
        End If
        If cmbSmith_MAN.Text = "" Then cmbSmith_MAN.Text = "ALL"
        If CmbSmith.Text = "" Then CmbSmith.Text = "ALL"
        If chkMetalName = "" Then chkMetalName = "ALL"
        If SmithFormat = 1 Then
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHBAL_DETNEW_FORMAT1"
        Else
            If ChkWithWast.Checked Then
                strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT='Y' WHERE CTLID='RPT_SMITHBAL_WASTLESS_GRSWT'"
            Else
                strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT='N' WHERE CTLID='RPT_SMITHBAL_WASTLESS_GRSWT'"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHBAL_DETNEW"
        End If
        strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@FROMDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        'strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COMPANYID='" & SelectedCompanyId & "'"
        strSql += vbCrLf + ",@COSTIDS='" & IIf(chkCostName = "", "ALL", chkCostName) & "'"
        strSql += vbCrLf + ",@ACCODE='" & cmbAccName & "'"
        strSql += vbCrLf + ",@METAL='" & chkMetalName & "'"
        strSql += vbCrLf + ",@TEMPTABLE='TEMP" & Sysid & "SMITHBALDET'"
        If rbtPureWeight.Checked Then
            strSql += vbCrLf + ",@RUNBAL='P'"
        ElseIf rbtNetWeight.Checked Then
            strSql += vbCrLf + ",@RUNBAL='N'"
        Else
            strSql += vbCrLf + ",@RUNBAL='G'"
        End If
        strSql += vbCrLf + ",@TRANSUM='" & IIf(chkTranNowise.Checked, "Y", "N") & "'"
        strSql += vbCrLf + ",@WITHAPP='" & IIf(ChkApproval.Checked, "Y", "N") & "'"
        strSql += vbCrLf + ",@WITHPU='" & IIf(ChkWithDirectPur.Checked, "Y", "N") & "'"
        If SmithFormat <> 1 Then
            strSql += vbCrLf + ",@TRANFILTER=" & Replace(trantype, "','", ",") & ""
            strSql += vbCrLf + ",@WITHAMTBAL='" & IIf(chkWithAmountBalance.Checked, "Y", "N") & "'"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " SELECT "
        'strSql += vbCrLf + " PARTICULAR,CATNAME,OCATNAME,ITEM,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,ISTNWT,ISTNWTC,ITOUCH,INETWT,IWAST,ISSWT,IPUREWT,RPCS,RGRSWT"
        'strSql += vbCrLf + " ,RSTNWT,RSTNWTC,RTOUCH,RNETWT,RWAST,RECWT,RPUREWT,RUNTOTALG,RUNTOTALN,RUNTOTAL,STNGRM,STNCRT,DEBIT,CREDIT,RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,ACNAME,TFILTER,USERNAME"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
        'strSql += vbCrLf + " ORDER BY ACNAME,RESULT,TRANDATE,TRANNO"

        If SmithFormat = 1 Then
            strSql = " SELECT "
            strSql += vbCrLf + " PARTICULAR,CATNAME,OCATNAME,ITEM,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,ISTNWT"
            strSql += vbCrLf + " ,ITOUCH,INETWT,IWASTPER,IWAST,IMC,IALLMC,ISSWT,IPUREWT,RPCS,RGRSWT"
            strSql += vbCrLf + " ,RSTNWT,RTOUCH,RNETWT,RWASTPER,RWAST,RMC,RALLMC,RECWT,RPUREWT,DEBIT,CREDIT,RUNTOTAL,RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,ACNAME,TFILTER,USERNAME"
            strSql += vbCrLf + " FROM TEMPTABLEDB..XYTEMP" & Sysid & "SMITHBALDET"
            strSql += vbCrLf + " ORDER BY ACNAME,RESULT,TRANDATE,TRANNO"
        Else
            strSql = " SELECT "
            strSql += vbCrLf + " PARTICULAR,CATNAME,OCATNAME,ITEM,TRANNO,TRANTYPE,TDATE,BILLDATE,DESCRIPTION,IPCS,IGRSWT,ISTNWT"
            strSql += vbCrLf + " ,ISTNWTC,ITOUCH,INETWT,IWAST,ISSWT,IPUREWT,RPCS,RGRSWT"
            strSql += vbCrLf + " ,RSTNWT,RSTNWTC,RTOUCH,RNETWT,RWAST,RECWT,RPUREWT,RUNTOTALG,RUNTOTALN,RUNTOTAL,STNGRM,STNCRT"
            strSql += vbCrLf + " ,DEBIT,CREDIT"
            strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,ACNAME,TFILTER,USERNAME"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "SMITHBALDETXY"
            strSql += vbCrLf + " ORDER BY ACNAME,RESULT,TRANDATE,TRANTYPE,TRANNO"
        End If

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        TabMain.SelectedTab = TabView
        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'objGridShower.gridView.RowTemplate.Height = 21
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ''objGridShower.Size = New Size(778, 550)
        'objGridShower.Text = "SMITH BALANCE DETAILED"
        Dim tit As String = " SMITH BALANCE DETAILED REPORT"
        tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkRunningBal.Checked = False Then
            tit += " BASED ON "
            If rbtGrossWeight.Checked Then
                tit += "GROSS WEIGHT"
            ElseIf rbtNetWeight.Checked Then
                tit += "NET WEIGHT"
            Else
                tit += "PURE WEIGHT"
            End If
        End If
        If chkCostName <> "" Then tit += "COSTCENTRE :" & Replace(GetChecked_CheckedList(chkLstCostCentre), "'", "")
        lblTitle.Text = tit
        AddHandler GridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        'objGridShower.StartPosition = FormStartPosition.CenterScreen
        'objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        GridView.DataSource = Nothing
        GridView.DataSource = dtGrid 'objGridShower.dsGrid.Tables(0)
        'objGridShower.FormReSize = True
        'objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = False
        gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting1(GridView)
        FormatGridColumns(GridView, False, False, , False)
        'objGridShower.formuser = userId
        'objGridShower.Show()
        'objGridShower.WindowState = FormWindowState.Maximized

        'objGridShower.FormReSize = True
        'objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator1(gridViewHeader)
        FillGridGroupStyle_KeyNoWise(GridView)
        For Each dgvRow As DataGridViewRow In GridView.Rows
            If SmithFormat = 1 Then
                If dgvRow.Cells("TFILTER").Value.ToString = "Y" Then
                    dgvRow.Cells("IPCS").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("RPCS").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("IGRSWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("INETWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("RGRSWT").Style.BackColor = Color.MistyRose
                    dgvRow.Cells("RNETWT").Style.BackColor = Color.MistyRose
                End If
            Else
                If dgvRow.Cells("TFILTER").Value.ToString = "Y" Then
                    dgvRow.Cells("IPCS").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RPCS").Style.ForeColor = Color.Blue
                    dgvRow.Cells("IGRSWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("INETWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RGRSWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RNETWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RSTNWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("ISTNWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RSTNWTC").Style.ForeColor = Color.Blue
                    dgvRow.Cells("ISTNWTC").Style.ForeColor = Color.Blue
                    dgvRow.Cells("ISSWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RECWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("IPUREWT").Style.ForeColor = Color.Blue
                    dgvRow.Cells("RPUREWT").Style.ForeColor = Color.Blue
                End If
                dgvRow.Cells("RUNTOTALG").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RUNTOTALN").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RUNTOTAL").Style.BackColor = Color.MistyRose
                dgvRow.Cells("STNGRM").Style.BackColor = Color.MistyRose
                dgvRow.Cells("STNCRT").Style.BackColor = Color.MistyRose
            End If
        Next
        If chkTranNowise.Checked Then
            Label7.Visible = False
            lblItem.Visible = False
        Else
            Label7.Visible = True
            lblItem.Visible = True
        End If
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        If ChkFormat.Checked Then
            SetGridHeadColWid1(CType(sender, DataGridView))
        Else
            SetGridHeadColWid(CType(sender, DataGridView))
        End If
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION]"
        If chkRunningBal.Checked Then
            strSql += " ,''[IPCS~IGRSWT~INETWT~IPUREWT~ITOUCH]"
            strSql += " ,''[RPCS~RGRSWT~RNETWT~RPUREWT~RTOUCH]"
            strSql += " ,''[DEBIT~CREDIT]"
            strSql += " ,''[RUNWT~RUNBAL]"
            strSql += " ,''[REMARK1~REMARK2]"
            strSql += " ,''SCROLL"
        Else
            strSql += " ,''[IPCS~RPCS]"
            strSql += " ,''[ISSUE~RECEIPT]"
            strSql += " ,''[DEBIT~CREDIT]"
            strSql += " ,''[REMARK1~REMARK2]"
            strSql += " ,''SCROLL"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION").HeaderText = ""
        If chkRunningBal.Checked Then
            gridviewHead.Columns("IPCS~IGRSWT~INETWT~IPUREWT~ITOUCH").HeaderText = "ISSUE"
            gridviewHead.Columns("RPCS~RGRSWT~RNETWT~RPUREWT~RTOUCH").HeaderText = "RECEIPT"
            gridviewHead.Columns("REMARK1~REMARK2").HeaderText = "NARRATION"
        Else
            gridviewHead.Columns("IPCS~RPCS").HeaderText = "PCS"
            gridviewHead.Columns("ISSUE~RECEIPT").HeaderText = "WEIGHT"
            gridviewHead.Columns("REMARK1~REMARK2").HeaderText = "NARRATION"
        End If
        gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "AMOUNT"
        If chkRunningBal.Checked Then
            gridviewHead.Columns("RUNWT~RUNBAL").HeaderText = "RUNNING"
        End If
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
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
            .Columns("PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION").Width = f.gridView.Columns("PARTICULAR").Width + _
            f.gridView.Columns("TRANNO").Width + _
            f.gridView.Columns("TRANTYPE").Width + _
            f.gridView.Columns("TDATE").Width + _
            f.gridView.Columns("BILLDATE").Width + _
            f.gridView.Columns("DESCRIPTION").Width
            If chkRunningBal.Checked Then
                .Columns("IPCS~IGRSWT~INETWT~IPUREWT~ITOUCH").Width = f.gridView.Columns("IPCS").Width + f.gridView.Columns("IGRSWT").Width + f.gridView.Columns("INETWT").Width + f.gridView.Columns("IPUREWT").Width + f.gridView.Columns("ITOUCH").Width
                .Columns("RPCS~RGRSWT~RNETWT~RPUREWT~RTOUCH").Width = f.gridView.Columns("RPCS").Width + f.gridView.Columns("RGRSWT").Width + f.gridView.Columns("RNETWT").Width + f.gridView.Columns("RPUREWT").Width + f.gridView.Columns("RTOUCH").Width
            Else
                .Columns("IPCS~RPCS").Width = f.gridView.Columns("IPCS").Width + f.gridView.Columns("RPCS").Width
                If .Columns.Contains("ISSUE") And .Columns.Contains("RECEIPT") Then
                    .Columns("ISSUE~RECEIPT").Width = f.gridView.Columns("ISSUE").Width + f.gridView.Columns("RECEIPT").Width
                End If
            End If
            .Columns("REMARK1~REMARK2").Width = f.gridView.Columns("REMARK1").Width + f.gridView.Columns("REMARK2").Width
            .Columns("DEBIT~CREDIT").Width = f.gridView.Columns("DEBIT").Width + f.gridView.Columns("CREDIT").Width
            If chkRunningBal.Checked Then
                .Columns("RUNWT~RUNBAL").Width = f.gridView.Columns("RUNWT").Width + f.gridView.Columns("RUNBAL").Width
            End If
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
    Private Sub GridViewHeaderCreator1(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        If SmithFormat = 1 Then
            strSql = "SELECT ''[PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION]"
            strSql += " ,''[ITOUCH~IPCS~IGRSWT~ISTNWT~INETWT~IWAST~ISSWT~IPUREWT],''[RTOUCH~RPCS~RGRSWT~RSTNWT~RNETWT~RWAST~RECWT~RPUREWT],''[DEBIT~CREDIT],''[RUNTOTAL],''USERNAME,''SCROLL"
        Else
            strSql = "SELECT ''[PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION]"
            strSql += " ,''[ITOUCH~IPCS~IGRSWT~ISTNWT~ISTNWTC~INETWT~IWAST~ISSWT~IPUREWT],''[RTOUCH~RPCS~RGRSWT~RSTNWT~RSTNWTC~RNETWT~RWAST~RECWT~RPUREWT]"
            strSql += " ,''[RUNTOTALG~RUNTOTALN~RUNTOTAL~STNGRM~STNCRT~DEBIT~CREDIT]"
            strSql += " ,''USERNAME,''SCROLL"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        FormatGridColumns(gridviewHead, False, False, , False)
        gridviewHead.Columns("PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION").HeaderText = ""
        If SmithFormat = 1 Then
            gridviewHead.Columns("ITOUCH~IPCS~IGRSWT~ISTNWT~INETWT~IWAST~ISSWT~IPUREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("RTOUCH~RPCS~RGRSWT~RSTNWT~RNETWT~RWAST~RECWT~RPUREWT").HeaderText = "RECEIPT"
            gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "AMOUNT"
            gridviewHead.Columns("RUNTOTAL").HeaderText = "PUREWT"
            gridviewHead.Columns("USERNAME").HeaderText = "USERNAME"
        Else
            gridviewHead.Columns("ITOUCH~IPCS~IGRSWT~ISTNWT~ISTNWTC~INETWT~IWAST~ISSWT~IPUREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("RTOUCH~RPCS~RGRSWT~RSTNWT~RSTNWTC~RNETWT~RWAST~RECWT~RPUREWT").HeaderText = "RECEIPT"
            gridviewHead.Columns("RUNTOTALG~RUNTOTALN~RUNTOTAL~STNGRM~STNCRT~DEBIT~CREDIT").HeaderText = "BALANCE"
        End If
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid1(gridviewHead)
    End Sub

    Private Sub SetGridHeadColWid1(ByVal gridViewHead As DataGridView)
        'Dim f As frmGridDispDia
        'f = objGPack.GetParentControl(gridViewHeader)
        'If Not f.gridViewHeader.Visible Then Exit Sub
        'If f.gridViewHeader Is Nothing Then Exit Sub
        If Not GridView.ColumnCount > 0 Then Exit Sub
        If Not gridViewHeader.ColumnCount > 0 Then Exit Sub
        With gridViewHeader
            .Columns("PARTICULAR~TRANNO~TRANTYPE~TDATE~BILLDATE~DESCRIPTION").Width = GridView.Columns("PARTICULAR").Width + _
            GridView.Columns("TRANNO").Width + _
            GridView.Columns("TRANTYPE").Width + _
            GridView.Columns("TDATE").Width + _
            GridView.Columns("BILLDATE").Width + _
            IIf(GridView.Columns("DESCRIPTION").Visible, GridView.Columns("DESCRIPTION").Width, 0)
            If SmithFormat = 1 Then
                .Columns("ITOUCH~IPCS~IGRSWT~ISTNWT~INETWT~IWAST~ISSWT~IPUREWT").Width = IIf(GridView.Columns("ITOUCH").Visible, GridView.Columns("ITOUCH").Width, 0) _
            + IIf(GridView.Columns("IPCS").Visible, GridView.Columns("IPCS").Width, 0) + IIf(GridView.Columns("IGRSWT").Visible, GridView.Columns("IGRSWT").Width, 0) + IIf(GridView.Columns("INETWT").Visible, GridView.Columns("INETWT").Width, 0) _
            + IIf(GridView.Columns("ISSWT").Visible, GridView.Columns("ISSWT").Width, 0) _
            + IIf(GridView.Columns("IPUREWT").Visible, GridView.Columns("IPUREWT").Width, 0) + IIf(GridView.Columns("IWAST").Visible, GridView.Columns("IWAST").Width, 0) + IIf(GridView.Columns("ISTNWT").Visible, GridView.Columns("ISTNWT").Width, 0)

                .Columns("RTOUCH~RPCS~RGRSWT~RSTNWT~RNETWT~RWAST~RECWT~RPUREWT").Width = IIf(GridView.Columns("RTOUCH").Visible, GridView.Columns("RTOUCH").Width, 0) _
                + IIf(GridView.Columns("RPCS").Visible, GridView.Columns("RPCS").Width, 0) + IIf(GridView.Columns("RGRSWT").Visible, GridView.Columns("RGRSWT").Width, 0) + IIf(GridView.Columns("RNETWT").Visible, GridView.Columns("RNETWT").Width, 0) _
                + IIf(GridView.Columns("RECWT").Visible, GridView.Columns("RECWT").Width, 0) _
                + IIf(GridView.Columns("RPUREWT").Visible, GridView.Columns("RPUREWT").Width, 0) + IIf(GridView.Columns("RWAST").Visible, GridView.Columns("RWAST").Width, 0) + IIf(GridView.Columns("RSTNWT").Visible, GridView.Columns("RSTNWT").Width, 0)
                .Columns("DEBIT~CREDIT").Width = GridView.Columns("DEBIT").Width + GridView.Columns("CREDIT").Width
                .Columns("RUNTOTAL").Width = GridView.Columns("RUNTOTAL").Width
                .Columns("USERNAME").Width = GridView.Columns("USERNAME").Width
                .Columns("USERNAME").Visible = True
                .Columns("RUNTOTAL").Visible = True
            Else
                .Columns("ITOUCH~IPCS~IGRSWT~ISTNWT~ISTNWTC~INETWT~IWAST~ISSWT~IPUREWT").Width = IIf(GridView.Columns("ITOUCH").Visible, GridView.Columns("ITOUCH").Width, 0) _
                            + IIf(GridView.Columns("IPCS").Visible, GridView.Columns("IPCS").Width, 0) + IIf(GridView.Columns("IGRSWT").Visible, GridView.Columns("IGRSWT").Width, 0) + IIf(GridView.Columns("INETWT").Visible, GridView.Columns("INETWT").Width, 0) _
                            + IIf(GridView.Columns("ISSWT").Visible, GridView.Columns("ISSWT").Width, 0) _
                            + IIf(GridView.Columns("IPUREWT").Visible, GridView.Columns("IPUREWT").Width, 0) + IIf(GridView.Columns("IWAST").Visible, GridView.Columns("IWAST").Width, 0) + IIf(GridView.Columns("ISTNWT").Visible, GridView.Columns("ISTNWT").Width, 0) _
                            + IIf(GridView.Columns("ISTNWTC").Visible, GridView.Columns("ISTNWTC").Width, 0)

                .Columns("RTOUCH~RPCS~RGRSWT~RSTNWT~RSTNWTC~RNETWT~RWAST~RECWT~RPUREWT").Width = IIf(GridView.Columns("RTOUCH").Visible, GridView.Columns("RTOUCH").Width, 0) _
                + IIf(GridView.Columns("RPCS").Visible, GridView.Columns("RPCS").Width, 0) + IIf(GridView.Columns("RGRSWT").Visible, GridView.Columns("RGRSWT").Width, 0) + IIf(GridView.Columns("RNETWT").Visible, GridView.Columns("RNETWT").Width, 0) _
                + IIf(GridView.Columns("RECWT").Visible, GridView.Columns("RECWT").Width, 0) _
                + IIf(GridView.Columns("RPUREWT").Visible, GridView.Columns("RPUREWT").Width, 0) + IIf(GridView.Columns("RWAST").Visible, GridView.Columns("RWAST").Width, 0) + IIf(GridView.Columns("RSTNWT").Visible, GridView.Columns("RSTNWT").Width, 0) _
                + IIf(GridView.Columns("RSTNWTC").Visible, GridView.Columns("RSTNWTC").Width, 0)
                .Columns("RUNTOTALG~RUNTOTALN~RUNTOTAL~STNGRM~STNCRT~DEBIT~CREDIT").Width = GridView.Columns("DEBIT").Width + GridView.Columns("CREDIT").Width + GridView.Columns("STNGRM").Width + GridView.Columns("STNCRT").Width + GridView.Columns("RUNTOTALG").Width + GridView.Columns("RUNTOTALN").Width + GridView.Columns("RUNTOTAL").Width
                .Columns("USERNAME").Width = GridView.Columns("USERNAME").Width
                .Columns("USERNAME").Visible = True
            End If
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To GridView.ColumnCount - 1
                If GridView.Columns(cnt).Visible Then colWid += GridView.Columns(cnt).Width
            Next
            If colWid >= GridView.Width Then
                gridViewHeader.Columns("SCROLL").Visible = CType(GridView.Controls(1), VScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
                gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting1(ByVal dgv As DataGridView)
        Dim cnt As Integer
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            If SmithFormat = 1 Then
                For cnt = 35 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
            Else
                For cnt = 27 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
            End If
            

            .Columns("PARTICULAR").Width = 200
            .Columns("TRANNO").Width = 60
            .Columns("TDATE").Width = 80
            .Columns("TDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("BILLDATE").Width = 80
            .Columns("DESCRIPTION").Width = 120
            .Columns("DESCRIPTION").Visible = False
            .Columns("IPCS").Width = 60
            .Columns("RPCS").Width = 60

            .Columns("IGRSWT").Width = 85
            .Columns("INETWT").Width = 85
            .Columns("IPUREWT").Width = 85
            .Columns("IWAST").Width = 90
            .Columns("RGRSWT").Width = 85
            .Columns("RNETWT").Width = 85
            .Columns("RPUREWT").Width = 85

            .Columns("ITOUCH").Width = 85
            .Columns("RTOUCH").Width = 85

            .Columns("DEBIT").Width = 100
            .Columns("CREDIT").Width = 100
            .Columns("RUNTOTAL").Width = 100

            .Columns("TDATE").HeaderText = "TRANDATE"

            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPUREWT").HeaderText = "PUREWT"
            .Columns("ISSWT").HeaderText = "ISSWT"
            .Columns("ITOUCH").HeaderText = "TOUCH"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "PUREWT"
            .Columns("RECWT").HeaderText = "RECWT"
            .Columns("RTOUCH").HeaderText = "TOUCH"
            .Columns("RWAST").HeaderText = "WAST"
            .Columns("IWAST").HeaderText = "ALLOY/WAST"
            If SmithFormat = 1 Then
                .Columns("RSTNWT").HeaderText = "STNWT"
                .Columns("ISTNWT").HeaderText = "STNWT"
                .Columns("RUNTOTAL").HeaderText = "BALANCE"
            Else
                .Columns("RSTNWT").HeaderText = "STONE_GRM"
                .Columns("ISTNWT").HeaderText = "STONE_GRM"
                .Columns("RSTNWTC").HeaderText = "STONE_CRT"
                .Columns("ISTNWTC").HeaderText = "STONE_CRT"
                .Columns("STNGRM").HeaderText = "STN_GRM"
                .Columns("STNCRT").HeaderText = "STN_CRT"
                .Columns("RUNTOTALG").HeaderText = "GRSWT"
                .Columns("RUNTOTALN").HeaderText = "NETWT"
                .Columns("RUNTOTALG").Visible = True
                .Columns("RUNTOTALN").Visible = True
                .Columns("STNGRM").Visible = True
                .Columns("STNCRT").Visible = True
                .Columns("DEBIT").Visible = True
                .Columns("CREDIT").Visible = True
            End If
            If SmithFormat = 1 Then
                .Columns("IMC").Visible = False
                .Columns("RMC").Visible = False
                .Columns("IWASTPER").Visible = False
                .Columns("RWASTPER").Visible = False
                .Columns("IALLMC").Visible = False
                .Columns("RALLMC").Visible = False
            End If
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("RUNTOTAL").HeaderText = "PUREWT"
            .Columns("USERNAME").HeaderText = "USERNAME"
            .Columns("RPCS").Visible = False
            '.Columns("RGRSWT").Visible = rbtGrossWeight.Checked
            '.Columns("RNETWT").Visible = rbtNetWeight.Checked
            '.Columns("RPUREWT").Visible = rbtPureWeight.Checked
            '.Columns("RTOUCH").Visible = False
            .Columns("IPCS").Visible = False
            '.Columns("IGRSWT").Visible = rbtGrossWeight.Checked
            '.Columns("INETWT").Visible = rbtNetWeight.Checked
            '.Columns("IPUREWT").Visible = rbtPureWeight.Checked
            .Columns("ITOUCH").Visible = True
            .Columns("RUNTOTAL").Visible = True 'chkRunningBal.Checked
            .Columns("USERNAME").Visible = True
            .Columns("ITEM").Visible = False
            .Columns("CREDIT").Visible = chkWithAmountBalance.Checked
            .Columns("DEBIT").Visible = chkWithAmountBalance.Checked
            ' .Columns("SUBITEM").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("OCATNAME").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim cnt As Integer
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            If chkRunningBal.Checked Then
                For cnt = 20 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
            Else
                For cnt = 12 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
            End If

            .Columns("PARTICULAR").Width = 200
            .Columns("TRANNO").Width = 60
            .Columns("TDATE").Width = 80
            .Columns("BILLDATE").Width = 80
            .Columns("DESCRIPTION").Width = 120
            .Columns("IPCS").Width = 60
            .Columns("RPCS").Width = 60
            If .Columns.Contains("RPCS") Then .Columns("RPCS").HeaderText = "PCS"
            If .Columns.Contains("IPCS") Then .Columns("IPCS").HeaderText = "PCS"
            If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "NO"
            If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").HeaderText = "TYPE"
            'If .Columns.Contains("DESCRIPTION") Then .Columns("DESCRIPTION").HeaderText = "DESC"
            If .Columns.Contains("DEBIT") Then .Columns("DEBIT").HeaderText = "DR"
            If .Columns.Contains("CREDIT") Then .Columns("CREDIT").HeaderText = "CR"
            If .Columns.Contains("REMARK1") Then .Columns("REMARK1").Visible = True
            If .Columns.Contains("REMARK2") Then .Columns("REMARK2").Visible = True
            If chkRunningBal.Checked Then
                .Columns("IGRSWT").Width = 85
                .Columns("INETWT").Width = 85
                .Columns("IPUREWT").Width = 85
                .Columns("RGRSWT").Width = 85
                .Columns("RNETWT").Width = 85
                .Columns("RPUREWT").Width = 85
            Else
                If .Columns.Contains("ISSUE") Then .Columns("ISSUE").Width = 100
                If .Columns.Contains("RECEIPT") Then .Columns("RECEIPT").Width = 100
            End If
            .Columns("DEBIT").Width = 100
            .Columns("CREDIT").Width = 100

            If chkRunningBal.Checked Then
                .Columns("RUNWT").Width = 85
                .Columns("RUNBAL").Width = 100
            End If

            .Columns("TDATE").HeaderText = "TRANDATE"
            If chkRunningBal.Checked Then
                .Columns("IGRSWT").HeaderText = "GRSWT"
                .Columns("INETWT").HeaderText = "NETWT"
                .Columns("IPUREWT").HeaderText = "PUREWT"
                .Columns("ITOUCH").HeaderText = "TOUCH"
                .Columns("RGRSWT").HeaderText = "GRSWT"
                .Columns("RNETWT").HeaderText = "NETWT"
                .Columns("RPUREWT").HeaderText = "PUREWT"
                .Columns("RTOUCH").HeaderText = "TOUCH"
                .Columns("RUNWT").HeaderText = "GRSWT"
                .Columns("RUNBAL").HeaderText = "AMOUNT"
            End If

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub frmSmithBalanceDetailedReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And TabMain.SelectedTab.Name = TabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmSmithBalanceDetailedReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        LoadCompany(chkLstCompany)
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
        strSql += " WHERE ACTYPE IN ('G','D','I') AND ISNULL(ACTIVE,'Y') <> 'H'"
        strSql += " ORDER BY RESULT,ACNAME"

        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        BrighttechPack.GlobalMethods.FillCombo(cmbSmith_MAN, dtAcc, "ACNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(CmbSmith, dtAcc, "ACNAME", )

        cmbtrantypeload()
        Me.TabMain.ItemSize = New Size(1, 1)
        Me.TabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        Me.TabMain.SelectedTab = TabGeneral
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Function cmbtrantypeload()
        strSql = " SELECT 'ALL' TTYPE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'ISSUE',2 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'RECEIPT',3 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'INTERNAL TRANSFER',4 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'APPROVAL ISSUE',5 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'APPROVAL RECEIPT',6 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'OTHER ISSUE',7 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'OTHER RECEIPT',8 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE RETURN',9 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE',10 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'MISC ISSUE',11 RESULT"
        strSql += " ORDER BY result"
        DtTrantype = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTrantype)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbTranType, DtTrantype, "TTYPE", , "ALL")
    End Function

    Private Function GetTranType(ByVal selTran As String)
        Dim trantype As String = ""
        If selTran <> "ALL" And selTran <> "" Then
            selTran = "," & selTran
            If selTran.Contains(",ISSUE") Then
                trantype += "'IIS',"
            End If
            If selTran.Contains(",APPROVAL ISSUE") Then
                trantype += "'IAP',"
            End If
            If selTran.Contains(",OTHER ISSUE") Then
                trantype += "'IOT',"
            End If
            If selTran.Contains(",PURCHASE RETURN") Then
                trantype += "'IPU',"
            End If
            If selTran.Contains(",INTERNAL TRANSFER") Then
                trantype += "'IIN',"
                trantype += "'RIN',"
            End If
            If selTran.Contains(",RECEIPT") Then
                trantype += "'RRE',"
            End If
            If selTran.Contains(",APPROVAL RECEIPT") Then
                trantype += "'RAP',"
            End If
            If selTran.Contains(",OTHER RECEIPT") Then
                trantype += "'ROT',"
            End If
            If selTran.Contains(",MISC ISSUE") Then
                trantype += "'MI',"
            End If
            If selTran.Contains(",PURCHASE") Then
                trantype += "'RPU',"
            End If
            If selTran.Contains(",URD PURCHASE") Then
                trantype += "'PU',"
            End If
            If selTran.Contains(",URD ISSUE") Then
                trantype += "'SA',"
            End If
            If selTran.Contains(",URD ISSUE RETURN") Then
                trantype += "'SR',"
            End If
        End If
        If trantype <> "" Then
            trantype = Mid(trantype, 1, trantype.Length - 1)
        End If
        Return trantype
    End Function

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
        CmbSmith.Text = ""
        Prop_Gets()
        chkMultiSelect_CheckStateChanged(Me, New EventArgs)
        'cmbTranType.Select()
        chkCompanySelectAll.Select()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSmithBalanceDetailedReport_Properties
        GetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype)
        obj.p_cmbSmith_MAN = cmbSmith_MAN.Text
        obj.p_cmbSmith = CmbSmith.Text
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_rbtGrossWeight = rbtGrossWeight.Checked
        obj.p_rbtNetWeight = rbtNetWeight.Checked
        obj.p_rbtPureWeight = rbtPureWeight.Checked
        obj.p_chkTranNowise = chkTranNowise.Checked
        obj.p_chkTranno = chkTranno.Checked
        obj.p_chkApp = ChkApproval.Checked
        obj.p_chkRunningBal = chkRunningBal.Checked
        obj.p_chWithDirPur = ChkWithDirectPur.Checked
        obj.p_chkMultiSelect = chkMultiSelect.Checked
        obj.p_chkSmith = chkSmith.Checked
        obj.p_chkOthers = chkOthers.Checked
        obj.p_chkInternal = chkInternal.Checked
        obj.p_chkDealer = chkDealer.Checked
        obj.p_chkWithWast = ChkWithWast.Checked
        obj.p_chkSpecific = ChkFormat.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceDetailedReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSmithBalanceDetailedReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceDetailedReport_Properties))
        SetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype, "ALL")
        cmbSmith_MAN.Text = obj.p_cmbSmith_MAN
        CmbSmith.Text = obj.p_cmbSmith
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        rbtGrossWeight.Checked = obj.p_rbtGrossWeight
        rbtNetWeight.Checked = obj.p_rbtNetWeight
        rbtPureWeight.Checked = obj.p_rbtPureWeight
        chkTranNowise.Checked = obj.p_chkTranNowise
        chkTranno.Checked = obj.p_chkTranno
        ChkApproval.Checked = obj.p_chkApp
        chkRunningBal.Checked = obj.p_chkRunningBal
        ChkWithDirectPur.Checked = obj.p_chWithDirPur
        chkMultiSelect.Checked = obj.p_chkMultiSelect
        chkSmith.Checked = obj.p_chkSmith
        chkOthers.Checked = obj.p_chkOthers
        chkInternal.Checked = obj.p_chkInternal
        chkDealer.Checked = obj.p_chkDealer
        ChkWithWast.Checked = obj.p_chkWithWast
        ChkFormat.Checked = obj.p_chkSpecific
    End Sub
    Private Sub chkRunningBal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRunningBal.CheckedChanged
        rbtGrossWeight.Enabled = chkRunningBal.Checked
        rbtNetWeight.Enabled = chkRunningBal.Checked
        rbtPureWeight.Enabled = chkRunningBal.Checked
        ChktransactionOnly.Enabled = chkRunningBal.Checked
        If chkRunningBal.Checked = False Then ChktransactionOnly.Checked = False
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
        ElseIf UCase(e.KeyChar) = "S" Then
            LoadStudedDet()
        End If
    End Sub
    Private Sub LoadStudedDet()
        If GridView.CurrentRow Is Nothing Then Exit Sub
        If Val(GridView.CurrentRow.Cells("TRANNO").Value.ToString) = 0 Then Exit Sub
        If GridView.CurrentRow.Cells("TDATE").Value.ToString = "" Then Exit Sub
        If GridView.CurrentRow.Cells("TRANTYPE").Value.ToString = "RECEIPT" Then
            strSql = " SELECT IM.ITEMNAME ITEM,SM.SUBITEMNAME SUBITEM,STNPCS AS PCS,STNWT AS WEIGHT"
            strSql += vbCrLf + " ,CASE WHEN I.STONEUNIT='C' THEN 'CARAT' ELSE 'GRAM' END UNIT  "
            strSql += vbCrLf + " ,CASE WHEN I.CALCMODE='P' THEN 'PCS' ELSE 'WEIGHT' END CALCMODE"
            strSql += vbCrLf + " ,STNRATE AS RATE,STNAMT AS AMOUNT  FROM " & cnStockDb & "..RECEIPTSTONE I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.STNITEMID=IM.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON I.STNSUBITEMID=SM.SUBITEMID  AND I.STNITEMID= SM.ITEMID "
            strSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=I.BATCHNO AND TRANDATE ='" & Format(GridView.CurrentRow.Cells("TDATE").Value, "yyyy-MM-dd") & "' AND TRANNO=" & Val(GridView.CurrentRow.Cells("TRANNO").Value.ToString) & ")"
        Else
            strSql = " SELECT IM.ITEMNAME ITEM,SM.SUBITEMNAME SUBITEM,STNPCS AS PCS,STNWT AS WEIGHT"
            strSql += vbCrLf + " ,CASE WHEN I.STONEUNIT='C' THEN 'CARAT' ELSE 'GRAM' END UNIT  "
            strSql += vbCrLf + " ,CASE WHEN I.CALCMODE='P' THEN 'PCS' ELSE 'WEIGHT' END CALCMODE"
            strSql += vbCrLf + " ,STNRATE AS RATE,STNAMT AS AMOUNT  FROM " & cnStockDb & "..ISSSTONE I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.STNITEMID=IM.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON I.STNSUBITEMID=SM.SUBITEMID  AND I.STNITEMID= SM.ITEMID "
            strSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=I.BATCHNO AND TRANDATE ='" & Format(GridView.CurrentRow.Cells("TDATE").Value, "yyyy-MM-dd") & "' AND TRANNO=" & Val(GridView.CurrentRow.Cells("TRANNO").Value.ToString) & ")"
        End If
        Dim dtStud As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStud)
        If dtStud.Rows.Count > 0 Then
            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Text = "SMITH BALANCE DETAILED"
            Dim tit As String = " SMITH BALANCE DETAILED REPORT"
            objGridShower.lblTitle.Text = tit
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtStud)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.Visible = False
            objGridShower.formuser = userId
            objGridShower.ShowDialog()
        End If
    End Sub
    Function funcSummaryDetails(ByVal rwIndex As Integer) As Integer
        ClearSummaryDetails()
        If GridView.Columns.Contains("TYPE") Then
            With GridView.Rows(rwIndex)
                If .Cells("TYPE").Value.ToString = "R" Then
                    Dim Amt As Decimal = Val(.Cells("AMT").Value.ToString) - (Val(.Cells("STNAMT").Value.ToString) + Val(.Cells("RALLMC").Value.ToString) + Val(.Cells("OTHAMT").Value.ToString))
                    SetSummaryDetails(0, "GRS WT", IIf((Val(.Cells("RGRSWT").Value.ToString)) <> 0, .Cells("RGRSWT").Value.ToString, ""), _
                            "METAL AMT", IIf((Val(Amt)) <> 0, Amt, ""), _
                            "CATNAME", IIf(.Cells("CATNAME").Value.ToString <> "", .Cells("CATNAME").Value.ToString, "---"), _
                            "TOUCH", IIf((Val(.Cells("RTOUCH").Value.ToString)) <> 0, .Cells("RTOUCH").Value.ToString, ""))
                    SetSummaryDetails(1, "STN WT", IIf((Val(.Cells("RSTNWT").Value.ToString)) <> 0, .Cells("RSTNWT").Value.ToString, ""), _
                            "STN AMT", IIf((Val(.Cells("STNAMT").Value.ToString)) <> 0, .Cells("STNAMT").Value.ToString, ""), _
                            "ITEM NAME", IIf(.Cells("ITEM").Value.ToString <> "", .Cells("ITEM").Value.ToString, ""), _
                            "WAST %", IIf((Val(.Cells("RWASTPER").Value.ToString)) <> 0, .Cells("RWASTPER").Value.ToString, ""))
                    SetSummaryDetails(2, "NET WT", IIf((Val(.Cells("RNETWT").Value.ToString)) <> 0, .Cells("RNETWT").Value.ToString, ""), _
                            "MC AMT", IIf((Val(.Cells("RALLMC").Value.ToString)) <> 0, .Cells("RALLMC").Value.ToString, ""), _
                            "SUB ITEM", IIf(.Cells("SUBITEM").Value.ToString <> "", .Cells("SUBITEM").Value.ToString, ""), _
                            "MC %", IIf((Val(.Cells("RMC").Value.ToString)) <> 0, .Cells("RMC").Value.ToString, ""))
                    SetSummaryDetails(3, "WAST/ALLOY", IIf((Val(.Cells("RWAST").Value.ToString)) <> 0, .Cells("RWAST").Value.ToString, ""), _
                            "OTHER AMT", IIf((Val(.Cells("OTHAMT").Value.ToString)) <> 0, .Cells("OTHAMT").Value.ToString, ""), _
                            "REMARK1", IIf(.Cells("REMARK1").Value.ToString <> "", .Cells("REMARK1").Value.ToString, "---"), _
                            "RATE", IIf((Val(.Cells("RATE").Value.ToString)) <> 0, .Cells("RATE").Value.ToString, ""))
                    SetSummaryDetails(4, "TOUCH", IIf((Val(.Cells("RTOUCH").Value.ToString)) <> 0, .Cells("RTOUCH").Value.ToString, ""), _
                            "VAT AMT", IIf((Val(.Cells("VATAMT").Value.ToString)) <> 0, .Cells("VATAMT").Value.ToString, ""), _
                            "REMARK2", IIf(.Cells("REMARK2").Value.ToString <> "", .Cells("REMARK2").Value.ToString, "---"), _
                            "TDS", IIf((Val(.Cells("TDS").Value.ToString)) <> 0, .Cells("TDS").Value.ToString, ""))
                    SetSummaryDetails(5, "PURE WT", IIf((Val(.Cells("RPUREWT").Value.ToString)) <> 0, .Cells("RPUREWT").Value.ToString, ""), _
                            "TOTAL AMT", "", _
                            "USERNAME", IIf(.Cells("USERNAME").Value.ToString <> "", .Cells("USERNAME").Value.ToString, "---"), _
                            "CALCTYPE", IIf(.Cells("CALCTYPE").Value.ToString <> "", .Cells("CALCTYPE").Value.ToString, ""))
                Else
                    Dim Amt As Decimal = Val(.Cells("AMT").Value.ToString) - (Val(.Cells("STNAMT").Value.ToString) + Val(.Cells("IALLMC").Value.ToString) + Val(.Cells("OTHAMT").Value.ToString))
                    SetSummaryDetails(0, "GRS WT", IIf((Val(.Cells("IGRSWT").Value.ToString)) <> 0, .Cells("IGRSWT").Value.ToString, ""), _
                            "METAL AMT", IIf((Val(Amt)) <> 0, Amt, ""), _
                            "CATNAME", IIf(.Cells("CATNAME").Value.ToString <> "", .Cells("CATNAME").Value.ToString, "---"), _
                            "TOUCH", IIf((Val(.Cells("ITOUCH").Value.ToString)) <> 0, .Cells("ITOUCH").Value.ToString, ""))
                    SetSummaryDetails(1, "STN WT", IIf((Val(.Cells("ISTNWT").Value.ToString)) <> 0, .Cells("ISTNWT").Value.ToString, ""), _
                            "STN AMT", IIf((Val(.Cells("STNAMT").Value.ToString)) <> 0, .Cells("STNAMT").Value.ToString, ""), _
                            "ITEM NAME", IIf(.Cells("ITEM").Value.ToString <> "", .Cells("ITEM").Value.ToString, ""), _
                            "WAST %", IIf((Val(.Cells("IWASTPER").Value.ToString)) <> 0, .Cells("IWASTPER").Value.ToString, ""))
                    SetSummaryDetails(2, "NET WT", IIf((Val(.Cells("INETWT").Value.ToString)) <> 0, .Cells("INETWT").Value.ToString, ""), _
                            "MC AMT", IIf((Val(.Cells("IALLMC").Value.ToString)) <> 0, .Cells("IALLMC").Value.ToString, ""), _
                            "SUB ITEM", IIf(.Cells("SUBITEM").Value.ToString <> "", .Cells("SUBITEM").Value.ToString, ""), _
                            "MC %", IIf((Val(.Cells("RMC").Value.ToString)) <> 0, .Cells("IMC").Value.ToString, ""))
                    SetSummaryDetails(3, "WAST/ALLOY", IIf((Val(.Cells("IWAST").Value.ToString)) <> 0, .Cells("IWAST").Value.ToString, ""), _
                            "OTHER AMT", IIf((Val(.Cells("OTHAMT").Value.ToString)) <> 0, .Cells("OTHAMT").Value.ToString, ""), _
                            "REMARK1", IIf(.Cells("REMARK1").Value.ToString <> "", .Cells("REMARK1").Value.ToString, "---"), _
                            "RATE", IIf((Val(.Cells("RATE").Value.ToString)) <> 0, .Cells("RATE").Value.ToString, ""))
                    SetSummaryDetails(4, "TOUCH", IIf((Val(.Cells("ITOUCH").Value.ToString)) <> 0, .Cells("ITOUCH").Value.ToString, ""), _
                            "VAT AMT", IIf((Val(.Cells("VATAMT").Value.ToString)) <> 0, .Cells("VATAMT").Value.ToString, ""), _
                            "REMARK2", IIf(.Cells("REMARK2").Value.ToString <> "", .Cells("REMARK2").Value.ToString, "---"), _
                            "TDS", IIf((Val(.Cells("TDS").Value.ToString)) <> 0, .Cells("TDS").Value.ToString, ""))
                    SetSummaryDetails(5, "PURE WT", IIf((Val(.Cells("IPUREWT").Value.ToString)) <> 0, .Cells("IPUREWT").Value.ToString, ""), _
                            "TOTAL AMT", "", _
                            "USERNAME", IIf(.Cells("USERNAME").Value.ToString <> "", .Cells("USERNAME").Value.ToString, "---"), _
                            "CALCTYPE", IIf(.Cells("CALCTYPE").Value.ToString <> "", .Cells("CALCTYPE").Value.ToString, ""))
                End If
            End With
        Else
            With GridView.Rows(rwIndex)
                SetSummaryDetails(0, "GRS WT", IIf((Val(.Cells("IGRSWT").Value.ToString)) <> 0, .Cells("IGRSWT").Value.ToString, ""), _
                "CAT NAME", IIf(IsDBNull(.Cells("CATNAME").Value.ToString), "", Mid(.Cells("CATNAME").Value.ToString, 1, 20)) _
                 , "GRS WT", IIf((Val(.Cells("RGRSWT").Value.ToString)) <> 0, .Cells("RGRSWT").Value.ToString, "") _
                 , "CAT NAME", IIf(IsDBNull(.Cells("CATNAME").Value.ToString), "", Mid(.Cells("CATNAME").Value.ToString, 1, 20)))

                SetSummaryDetails(1, "STN WT", IIf((Val(.Cells("ISTNWT").Value.ToString)) <> 0, .Cells("ISTNWT").Value.ToString, ""), _
                "ACNAME", IIf(.Cells("ACNAME").Value.ToString <> "", .Cells("ACNAME").Value.ToString, "") _
                , "STN WT", IIf((Val(.Cells("RSTNWT").Value.ToString)) <> 0, .Cells("RSTNWT").Value.ToString, "") _
                , "USERNAME", IIf(.Cells("USERNAME").Value.ToString <> "", .Cells("USERNAME").Value.ToString, ""))

                SetSummaryDetails(2, "NET WT", IIf((Val(.Cells("INETWT").Value.ToString)) <> 0, .Cells("INETWT").Value.ToString, ""), _
                "ITEM NAME", IIf(IsDBNull(.Cells("ITEM").Value.ToString), "", Mid(.Cells("ITEM").Value.ToString, 1, 20)) _
                 , "NET WT", IIf((Val(.Cells("RNETWT").Value.ToString)) <> 0, .Cells("RNETWT").Value.ToString, "") _
                 , "ITEM NAME", IIf(IsDBNull(.Cells("ITEM").Value.ToString), "", Mid(.Cells("ITEM").Value.ToString, 1, 20)))

                'Format(RGrsWt, "#0.000")
                If SmithFormat = 1 Then
                    SetSummaryDetails(3, "WASTPER", IIf((Val(.Cells("IWASTPER").Value.ToString)) <> 0, .Cells("IWASTPER").Value.ToString & "%", ""),
                    "WAST/ALLOY", IIf((Val(.Cells("IWAST").Value.ToString)) <> 0, .Cells("IWAST").Value.ToString, "") _
                    , "WASTPER", IIf((Val(.Cells("RWASTPER").Value.ToString)) <> 0, .Cells("RWASTPER").Value.ToString & "%", "") _
                    , "WAST/ALLOY", IIf((Val(.Cells("RWAST").Value.ToString)) <> 0, .Cells("RWAST").Value.ToString, ""))
                Else
                    SetSummaryDetails(3, "WASTPER", "", _
                    "WAST/ALLOY", IIf((Val(.Cells("IWAST").Value.ToString)) <> 0, .Cells("IWAST").Value.ToString, "") _
                    , "WASTPER", "" _
                    , "WAST/ALLOY", IIf((Val(.Cells("RWAST").Value.ToString)) <> 0, .Cells("RWAST").Value.ToString, ""))
                End If
                SetSummaryDetails(4, "PURE WT", IIf((Val(.Cells("IPUREWT").Value.ToString)) <> 0, .Cells("IPUREWT").Value.ToString, ""), _
                "OLD CAT NAME", IIf(IsDBNull(.Cells("OCATNAME").Value.ToString), "", Mid(.Cells("OCATNAME").Value.ToString, 1, 20)) _
                 , "PURE WT", IIf((Val(.Cells("RPUREWT").Value.ToString)) <> 0, .Cells("RPUREWT").Value.ToString, "") _
                 , "OLD CAT NAME", IIf(IsDBNull(.Cells("OCATNAME").Value.ToString), "", Mid(.Cells("OCATNAME").Value.ToString, 1, 20)))
                If SmithFormat = 1 Then
                    SetSummaryDetails(5, "MC/GRM ", IIf((Val(.Cells("IMC").Value.ToString)) <> 0, .Cells("IMC").Value.ToString, ""), _
                    "MC", IIf((Val(.Cells("IALLMC").Value.ToString)) <> 0, .Cells("IALLMC").Value.ToString, "") _
                     , "MC/GRM ", IIf((Val(.Cells("RMC").Value.ToString)) <> 0, .Cells("RMC").Value.ToString, "") _
                     , "MC", IIf((Val(.Cells("RALLMC").Value.ToString)) <> 0, .Cells("RALLMC").Value.ToString, ""))
                Else
                    SetSummaryDetails(5, " MC/GRM", "", _
                    "MC", "" _
                     , "MC/GRM", "" _
                     , "MC", "")
                End If
            End With
        End If
        gridview_issue.Refresh()
    End Function
    Private Sub ClearSummaryDetails()
        For Each ro As DataRow In dtissue.Rows
            ro("COL2") = ""
            ro("COL4") = ""
            ro("COL6") = ""
            ro("COL8") = ""
        Next
    End Sub
    Private Sub SetSummaryDetails(ByVal rwIndex As Integer, ByVal col1 As String, ByVal col2 As String, _
    ByVal col3 As String, ByVal col4 As String, ByVal col5 As String, ByVal col6 As String, ByVal col7 As String, ByVal col8 As String)
        If Not rwIndex <= dtissue.Rows.Count - 1 Then Exit Sub
        With dtissue.Rows(rwIndex)
            .Item("COL1") = col1
            .Item("COL2") = col2
            .Item("COL3") = col3
            .Item("COL4") = col4
            .Item("COL5") = col5
            .Item("COL6") = col6
            .Item("COL7") = col7
            .Item("COL8") = col8
        End With
    End Sub
    Private Sub GridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridView.RowEnter
        funcSummaryDetails(e.RowIndex)
        'Dim rw As Integer = e.RowIndex
        'If GridView.RowCount > 0 Then
        '    With GridView.Rows(rw)
        '        If Val(.Cells("IGRSWT").Value.ToString) > 0 Then
        '            Label11.Text = Val(.Cells("IGRSWT").Value.ToString)
        '        ElseIf Val(.Cells("RGRSWT").Value.ToString) > 0 Then
        '            Label11.Text = Val(.Cells("RGRSWT").Value.ToString)
        '        Else
        '            Label11.Text = ""
        '        End If
        '        If Val(.Cells("IPUREWT").Value.ToString) > 0 Then
        '            Label14.Text = Val(.Cells("IPUREWT").Value.ToString)
        '        ElseIf Val(.Cells("RPUREWT").Value.ToString) > 0 Then
        '            Label14.Text = Val(.Cells("RPUREWT").Value.ToString)
        '        Else
        '            Label14.Text = ""
        '        End If
        '        If Val(.Cells("INETWT").Value.ToString) > 0 Then
        '            Label16.Text = Val(.Cells("INETWT").Value.ToString)
        '        ElseIf Val(.Cells("RNETWT").Value.ToString) > 0 Then
        '            Label16.Text = Val(.Cells("RNETWT").Value.ToString)
        '        Else
        '            Label16.Text = ""
        '        End If
        '        lblCatName.Text = IIf(IsDBNull(.Cells("CATNAME").Value.ToString), "", Mid(.Cells("CATNAME").Value.ToString, 1, 20))
        '        lblOCatName.Text = IIf(IsDBNull(.Cells("OCATNAME").Value.ToString), "", Mid(.Cells("OCATNAME").Value.ToString, 1, 20))
        '        lblItem.Text = IIf(IsDBNull(.Cells("ITEM").Value.ToString), "", Mid(.Cells("ITEM").Value.ToString, 1, 20))
        '    End With
        'End If
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

    Private Sub ReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReziseToolStripMenuItem.Click
        If GridView.RowCount > 0 Then
            If ReziseToolStripMenuItem.Checked Then
                ReziseToolStripMenuItem.Checked = False
            Else
                ReziseToolStripMenuItem.Checked = True
            End If
            If ReziseToolStripMenuItem.Checked Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridViewHeader_ColumnHeadersHeightChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewHeader.ColumnHeadersHeightChanged
        If gridViewHeader.DataSource IsNot Nothing Then
            'gridViewHeader.Size = New Size(gridViewHeader.Size.Width, gridViewHeader.ColumnHeadersHeight - 4)
        End If
    End Sub

    Private Sub chkMultiSelect_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMultiSelect.CheckStateChanged
        If chkMultiSelect.Checked Then
            cmbSmith_MAN.Visible = True
            CmbSmith.Visible = False
        Else
            cmbSmith_MAN.Visible = False
            CmbSmith.Visible = True
        End If
    End Sub

    Private Sub CmbSmith_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbSmith.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function funcSummaryGridStyle() As Integer
        With gridview_issue
            With .Columns("COL1")
                .Width = 100
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
                .HeaderText = ""
            End With
            With .Columns("COL2")
                .HeaderText = ""
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("COL3")
                .HeaderText = ""
                .Width = 100
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With
            With .Columns("COL4")
                .HeaderText = ""
                .Width = 135
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("COL5")
                .HeaderText = ""
                .Width = 80
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With
            With .Columns("COL6")
                .HeaderText = ""
                .Width = 175
            End With
            With .Columns("COL7")
                .HeaderText = ""
                .Width = 80
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With
            With .Columns("COL8")
                .HeaderText = ""
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
    End Function

    Private Sub ChkFormat_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkFormat.CheckedChanged
        If ChkFormat.Checked Then
            chkRunningBal.Enabled = False
            ChkWithDirectPur.Visible = True
            ChkWithDirectPur.Enabled = True
            chkWithAmountBalance.Visible = True
        Else
            chkRunningBal.Enabled = True
            ChkWithDirectPur.Visible = False
            ChkWithDirectPur.Checked = False
            chkWithAmountBalance.Visible = False
            chkWithAmountBalance.Checked = False
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT ' 'COL1,' 'COL2,' 'COL3,' 'COL4,' 'COL5,' 'COL6,' 'COL7,' 'COL8  where  1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtissue)
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        dtissue.Rows.Add()
        gridview_issue.DataSource = dtissue
        gridview_issue.BackgroundColor = Me.BackColor
        funcSummaryGridStyle()
    End Sub

    Private Sub gridview_issue_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridview_issue.CellFormatting
        e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub chkCmbTranType_LostFocus(sender As Object, e As EventArgs) Handles chkCmbTranType.LostFocus
        If chkCmbTranType.Text = "ALL" Then
            ChkApproval.Enabled = True
            ChkApproval.Checked = True
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            ChkApproval.Enabled = False
            ChkApproval.Checked = True
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE, APPROVAL RECEIPT" Then
            ChkApproval.Enabled = False
            ChkApproval.Checked = True
        Else
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
        End If
    End Sub
End Class

Public Class frmSmithBalanceDetailedReport_Properties
    Private chkTranNowise As Boolean = False
    Public Property p_chkTranNowise() As Boolean
        Get
            Return chkTranNowise
        End Get
        Set(ByVal value As Boolean)
            chkTranNowise = value
        End Set
    End Property

    Private chkCmbTrantype As New List(Of String)
    Public Property p_chkCmbTrantype() As List(Of String)
        Get
            Return chkCmbTrantype
        End Get
        Set(ByVal value As List(Of String))
            chkCmbTrantype = value
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
    Private chkTranno As Boolean = False
    Public Property p_chkTranno() As Boolean
        Get
            Return chkTranno
        End Get
        Set(ByVal value As Boolean)
            chkTranno = value
        End Set
    End Property
    Private chWithDirPur As Boolean = False
    Public Property p_chWithDirPur() As Boolean
        Get
            Return chWithDirPur
        End Get
        Set(ByVal value As Boolean)
            chWithDirPur = value
        End Set
    End Property
    Private chkRunningBal As Boolean = False
    Public Property p_chkRunningBal() As Boolean
        Get
            Return chkRunningBal
        End Get
        Set(ByVal value As Boolean)
            chkRunningBal = value
        End Set
    End Property
    Private chkApp As Boolean = False
    Public Property p_chkApp() As Boolean
        Get
            Return chkApp
        End Get
        Set(ByVal value As Boolean)
            chkApp = value
        End Set
    End Property
    Private chkMultiSelect As Boolean = False
    Public Property p_chkMultiSelect() As Boolean
        Get
            Return chkMultiSelect
        End Get
        Set(ByVal value As Boolean)
            chkMultiSelect = value
        End Set
    End Property
    Private chkDealer As Boolean = True
    Public Property p_chkDealer() As Boolean
        Get
            Return chkDealer
        End Get
        Set(ByVal value As Boolean)
            chkDealer = value
        End Set
    End Property
    Private chkSmith As Boolean = True
    Public Property p_chkSmith() As Boolean
        Get
            Return chkSmith
        End Get
        Set(ByVal value As Boolean)
            chkSmith = value
        End Set
    End Property
    Private chkOthers As Boolean = False
    Public Property p_chkOthers() As Boolean
        Get
            Return chkOthers
        End Get
        Set(ByVal value As Boolean)
            chkOthers = value
        End Set
    End Property
    Private chkCustomer As Boolean = False
    Public Property p_chkCustomer() As Boolean
        Get
            Return chkCustomer
        End Get
        Set(ByVal value As Boolean)
            chkCustomer = value
        End Set
    End Property
    Private chkInternal As Boolean = False
    Public Property p_chkInternal() As Boolean
        Get
            Return chkInternal
        End Get
        Set(ByVal value As Boolean)
            chkInternal = value
        End Set
    End Property
    Private chkWithWast As Boolean = True
    Public Property p_chkWithWast() As Boolean
        Get
            Return chkWithWast
        End Get
        Set(ByVal value As Boolean)
            chkWithWast = value
        End Set
    End Property
    Private chkSpecific As Boolean = False
    Public Property p_chkSpecific() As Boolean
        Get
            Return chkSpecific
        End Get
        Set(ByVal value As Boolean)
            chkSpecific = value
        End Set
    End Property
End Class