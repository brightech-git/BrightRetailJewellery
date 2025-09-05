Imports System.Data.OleDb
Public Class frmSmithBalanceSummaryReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Obj_MeFilterValues As frmSmithBalanceSummaryReport_Properties
    Dim DtTrantype As DataTable
    Dim chkCategory As String
    Dim chkCostName As String
    Dim chkMetalName As String
    Dim Selectedcatid As String
    Dim Selectedmetalid As String
    Dim SelectedCostId As String
    Dim ActIVE As String = ""
    Dim trantype As String
    Dim chkwt As String = ""
    Dim selTran As String = ""
    Dim AcTypeFilteration As String = ""
    Dim dtCostCentre As New DataTable
    Dim dispflag As Boolean = True
    Dim PurewtPer As Decimal = GetAdmindbSoftValue("RPT_PUREWTPER", "0")
    Dim objGridDetailShower As frmSmithBalSummery_F1
    Dim RPT_SMITHABS_DAYS As Boolean = IIf(GetAdmindbSoftValue("RPT_SMITHABS_DAYS", "N") = "Y", True, False)
    Dim RPT_SMITHABS_CATGRP As Boolean = IIf(GetAdmindbSoftValue("RPT_SMITHABS_CATGRP", "N") = "Y", True, False)
    Dim RPT_SMITHABS_HIDENETWT As Boolean = IIf(GetAdmindbSoftValue("RPT_SMITHABS_HIDENETWT", "N") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")


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

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        If ChkGrossWt.Checked = False And ChkNetWt.Checked = False And ChkPureWt.Checked = False Then MsgBox("Select GrsWt/NetWt/PureWt", MsgBoxStyle.Information) : ChkGrossWt.Focus() : Exit Sub
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
        End If
        Dim dtGrid As New DataTable("SUMMARY")
        AcTypeFilteration = ""
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
        If chkCustomer.Checked Then
            AcTypeFilteration += "'C',"
        End If
        If AcTypeFilteration <> "" Then
            Dim _dtAcType As DataTable
            _dtAcType = New DataTable
            strSql = "SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE WHERE ACTYPE IN (" & Mid(AcTypeFilteration, 1, AcTypeFilteration.Length - 1) & ")"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(_dtAcType)
            AcTypeFilteration = ""
            For k As Integer = 0 To _dtAcType.Rows.Count - 1
                AcTypeFilteration += "'" + _dtAcType.Rows(k)("TYPEID").ToString + "',"
            Next
            If AcTypeFilteration <> "" Then
                AcTypeFilteration = Mid(AcTypeFilteration, 1, AcTypeFilteration.Length - 1)
            End If
        End If


        selTran = ""
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If

        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        trantype = GetTranType(selTran)
        If trantype = "" Then trantype = "''"
        Dim Apptrantype As String = ""
        Dim Apptrantypechar As String = ""

        ''If ChkApproval.Checked = False Then Apptrantype = "IAP,RAP,AI,AR"
        If ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP,AI,AR"
            Apptrantypechar = "N"
        ElseIf ChkApproval.Checked = False Then
            Apptrantype = "AI,AR"
            Apptrantypechar = "C"
        ElseIf chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP"
            Apptrantypechar = "B"
        End If

        chkCategory = GetChecked_CheckedList(chkLstCategory)
        chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        chkMetalName = GetChecked_CheckedList(chkLstMetal)
        dispflag = True
        Dim Accode As String
        If CmbAcname.Text.Trim <> "" Then
            Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text.Trim & "'", , "", )
        Else
            Accode = ""
        End If

        If chkWithAbs.Checked = True Or chkWithSpecicFormat.Checked = True Then
            If Apptrantype = "" Then Apptrantype = "''"
            Selectedcatid = GetSelectedcatId(chkLstCategory, False)
            Selectedmetalid = GetSelectedMETALId(chkLstMetal, False)
            SelectedCostId = GetSelectedcostId(chkLstCostCentre, False)
            Dim chknb As String = IIf(chkWithNillBalance.Checked = True, "Y", "N")
            chkwt = ""
            If ChkGrossWt.Checked Then chkwt = "G"
            If ChkNetWt.Checked Then chkwt = "N"
            If ChkPureWt.Checked Then chkwt = "P"
            If ChkPureWt.Checked And ChkNetWt.Checked And ChkGrossWt.Checked Then chkwt = "A"
            If chkWithAbs.Checked = False Then dtpTodate.Value = dtpAsOnDate.Value
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHABSTRACT"
            strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & SYSID & "ABSTRACT'"
            strSql += vbCrLf + ",@FROMDATE='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@TODATE='" & dtpTodate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
            strSql += vbCrLf + ",@METALID='" & Selectedmetalid & "'"
            strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
            strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
            If AcTypeFilteration = "" Then
                strSql += vbCrLf + ",@ACFILTER=" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & ""
            Else
                strSql += vbCrLf + ",@ACFILTER=""" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & """"
            End If
            strSql += vbCrLf + ",@TRANFILTER=" & Replace(trantype, "','", ",") & ""
            strSql += vbCrLf + ",@CATFILTER='" & Selectedcatid & "'"
            strSql += vbCrLf + ",@NILBALANCE='" & chknb & "'"
            strSql += vbCrLf + ",@LOCALOUT='" & LocalOutSt & "'"
            strSql += vbCrLf + ",@APPTRANFILTER='" & Apptrantypechar.ToString & "'" ''IIf(ChkApproval.Checked = False, "N", "")
            strSql += vbCrLf + ",@PUREWTPER=" & PurewtPer & ""
            strSql += vbCrLf + ",@ACCODE='" & Accode & "'"
            strSql += vbCrLf + ",@WITHWAST='" & IIf(ChkwithWast.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@GROUPBY='" & IIf(chkGroupByAcType.Checked, "Y", "N") & "'"
            da = New OleDbDataAdapter(strSql, cn)
            Dim ds1 As New DataSet()
            da.Fill(ds1)
            dtGrid = ds1.Tables(0)
            Dim dtCol As New DataColumn("KEYNO")
            dtCol.AutoIncrement = True
            dtCol.AutoIncrementSeed = 0
            dtCol.AutoIncrementStep = 1
            dtGrid.Columns.Add(dtCol)
            For cnt As Integer = 0 To dtGrid.Rows.Count - 1
                dtGrid.Rows(cnt).Item("KEYNO") = cnt
            Next
        Else
            Dim acfilter As String = ""
            acfilter = " WHERE ISNULL(LEDPRINT,'') <> 'N'"
            If ActIVE = "B" Then
                acfilter += " AND ISNULL(ACTIVE,'Y') <> 'H'"
            ElseIf ActIVE = "Y" Then
                acfilter += " AND ISNULL(ACTIVE,'')  ='Y'"
            Else
                acfilter += " AND ISNULL(ACTIVE,'') = 'N'"
            End If
            If AcTypeFilteration <> "" Then acfilter += " AND ACTYPE IN (" & AcTypeFilteration & ")"
            If LocalOutSt <> "" Then acfilter += " AND LOCALOUTST ='" & LocalOutSt & "'"
            trantype = Replace(trantype, "''", "")
            strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSID & "SMITHBAL') > 0 DROP TABLE TEMP" & SYSID & "SMITHBAL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "  SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT) PUREWT,ACNAME AS PARTICULAR,METALID,ACNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  CASE WHEN TRANTYPE = 'I' THEN (CASE WHEN I.STONEUNIT='C' THEN I.GRSWT/5 ELSE I.GRSWT END) ELSE -1*(CASE WHEN I.STONEUNIT='C' THEN I.GRSWT/5 ELSE I.GRSWT END) END AS GRSWT"
            strSql += vbCrLf + "  ,CASE WHEN TRANTYPE = 'I' THEN (CASE WHEN I.STONEUNIT='C' THEN I.NETWT/5 ELSE I.NETWT END) ELSE -1*(CASE WHEN I.STONEUNIT='C' THEN I.NETWT/5 ELSE I.NETWT END) END AS NETWT"
            If PurewtPer <> 0 Then
                strSql += vbCrLf + "  ,CASE WHEN TRANTYPE = 'I' THEN (((CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END)/" & PurewtPer & ")*100) ELSE ((-1*(CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END)/" & PurewtPer & ")*100) END AS PUREWT"
            Else
                strSql += vbCrLf + "  ,CASE WHEN TRANTYPE = 'I' THEN (CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END) ELSE -1*(CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END) END AS PUREWT"
            End If
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT I"
            strSql += vbCrLf + "  WHERE COMPANYID = '" & strCompanyId & "'"
            If chkCmbTranType.Text = "ALL" And ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then
                strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')<>'A' "
            ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
                strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='A' "
            End If
            If chkCmbTranType.Text = "INTERNAL TRANSFER" Then
                strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='I' "
            End If
            If Accode <> "" Then strSql += vbCrLf + " AND I.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + "  AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + "  UNION ALL"


            'ISSUE
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "   ((CASE WHEN I.METALID NOT IN ('D','T') THEN GRSWT"
            ''strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.GRSWT*5 ELSE I.GRSWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.GRSWT*5 ELSE I.GRSWT END)"
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.GRSWT "
            End If

            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.GRSWT/5 ELSE I.GRSWT END) END) "
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('T') THEN I.GRSWT END) "
            End If

            'If ChkwithWast.Checked Then strSql += vbCrLf + "   +CASE WHEN TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END"
            If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') AND P.METALTYPE='O' THEN ISNULL(I.WASTAGE,0) WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
            strSql += vbCrLf + "   ) GRSWT"
            strSql += vbCrLf + "   ,((CASE WHEN I.METALID NOT IN ('D','T') THEN NETWT"
            ''strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.NETWT*5 ELSE I.NETWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.NETWT*5 ELSE I.NETWT END)"
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.NETWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.NETWT/5 ELSE I.NETWT END) END) "
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('T') THEN I.NETWT END) "
            End If

            'If ChkwithWast.Checked Then strSql += vbCrLf + "   +CASE WHEN TRANTYPE NOT IN('MI','AI') THEN ISNULL(ALLOY,0)-ISNULL(WASTAGE,0) ELSE 0 END"
            If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') AND P.METALTYPE='O' THEN ISNULL(I.WASTAGE,0) WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
            strSql += vbCrLf + "   ) NETWT"

            'If PurewtPer <> 0 Then
            '    strSql += vbCrLf + "  ,((PUREWT/" & PurewtPer & ")*100) PUREWT"
            'Else
            '    strSql += vbCrLf + "  ,(PUREWT) PUREWT"
            'End If
            If PurewtPer <> 0 Then
                strSql += vbCrLf + "   ,((((CASE WHEN I.METALID NOT IN ('D','T') THEN PUREWT"
                ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                If chkDiaWtConvert.Checked Then
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                Else
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.PUREWT "
                End If
                If chkStnWtConv.Checked Then
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END) END)  "
                Else
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.PUREWT END)  "
                End If

                strSql += vbCrLf + "   )/" & PurewtPer & ")*100) PUREWT"
            Else
                strSql += vbCrLf + "   ,((CASE WHEN I.METALID NOT IN ('D','T') THEN PUREWT"
                ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                If chkDiaWtConvert.Checked Then
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                Else
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.PUREWT "
                End If
                If chkStnWtConv.Checked Then
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END) END))  "
                Else
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.PUREWT END))  "
                End If

                strSql += vbCrLf + "   PUREWT"
            End If
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=I.CATCODE"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITYMAST P ON P.PURITYID=C.PURITYID"
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If Accode <> "" Then strSql += vbCrLf + " AND I.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If trantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If chkSales.Checked = True Then
                If ChkWithDirectPur.Checked Then
                Else
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('IPU','RPU')"
                End If
            Else
                If ChkWithDirectPur.Checked Then
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('SA')"
                Else
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('IPU','RPU','SA')"
                End If
            End If
            strSql += vbCrLf + "  UNION ALL"

            'RECEIPT
            strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "  -1*(GRSWT+CASE WHEN TRANTYPE NOT IN('AR') THEN ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) ELSE 0 END) GRSWT"
            'strSql += vbCrLf + "  ,-1*(NETWT+CASE WHEN TRANTYPE NOT IN('AR') THEN ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) ELSE 0 END) NETWT"
            strSql += vbCrLf + "   -1*((CASE WHEN I.METALID NOT IN ('D','T') THEN GRSWT"
            ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.GRSWT*5 ELSE I.GRSWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.GRSWT*5 ELSE I.GRSWT END)"
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.GRSWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.GRSWT/5 ELSE I.GRSWT END) END)  "
            Else
                strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.GRSWT END)  "
            End If
            If ChkwithWast.Checked Then strSql += vbCrLf + "   +CASE WHEN I.TRANTYPE NOT IN('AR') THEN ISNULL(WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END"
            strSql += vbCrLf + "   ) GRSWT"
            strSql += vbCrLf + "   ,-1*((CASE WHEN I.METALID NOT IN ('D','T') THEN NETWT"
            ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.NETWT*5 ELSE I.NETWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.NETWT*5 ELSE I.NETWT END)"
            Else
                strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.NETWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.NETWT/5 ELSE I.NETWT END) END)  "
            Else
                strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.NETWT END)  "
            End If
            If RPT_SMITHABS_CATGRP Then
                If ChkwithWast.Checked Then strSql += vbCrLf + "   +CASE WHEN I.TRANTYPE NOT IN('AR') AND C.CATGROUP<>'L' THEN ISNULL(WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END"
            Else
                If ChkwithWast.Checked Then strSql += vbCrLf + "   +CASE WHEN I.TRANTYPE NOT IN('AR') THEN ISNULL(WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END"
            End If
            strSql += vbCrLf + "   ) NETWT"
            If PurewtPer <> 0 Then
                'strSql += vbCrLf + "  ,((-1*PUREWT/" & PurewtPer & ")*100) PUREWT"
                strSql += vbCrLf + "   ,((-1*((CASE WHEN I.METALID NOT IN ('D','T') THEN PUREWT"
                ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT/5 ELSE I.PUREWT END)"
                If chkDiaWtConvert.Checked Then
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                Else
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.PUREWT "
                End If
                If chkStnWtConv.Checked Then
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.PUREWT*5 ELSE I.PUREWT END) END)  "
                Else
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.PUREWT END)  "
                End If
                strSql += vbCrLf + "   )/" & PurewtPer & ")*100) PUREWT"
            Else
                'strSql += vbCrLf + "  ,-1*PUREWT PUREWT"
                strSql += vbCrLf + "   ,-1*((CASE WHEN I.METALID NOT IN ('D','T') THEN PUREWT"
                ''strSql += vbCrLf + "   WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                If chkDiaWtConvert.Checked Then
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN (CASE WHEN I.STONEUNIT<>'C' THEN I.PUREWT*5 ELSE I.PUREWT END)"
                Else
                    strSql += vbCrLf + "    WHEN I.METALID IN ('D') THEN I.PUREWT "
                End If
                If chkStnWtConv.Checked Then
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN (CASE WHEN I.STONEUNIT='C' THEN I.PUREWT/5 ELSE I.PUREWT END) END))  "
                Else
                    strSql += vbCrLf + "   WHEN I.METALID IN ('T') THEN I.PUREWT END))  "
                End If
                strSql += vbCrLf + "   PUREWT"
            End If
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=I.CATCODE"
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If Accode <> "" Then strSql += vbCrLf + " AND I.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If trantype <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            'strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('IPU','RPU')"
            If chkSales.Checked = True Then
                If ChkWithDirectPur.Checked Then
                Else
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('IPU','RPU')"
                End If
            Else
                If ChkWithDirectPur.Checked Then
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('SA')"
                Else
                    strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('IPU','RPU','SA')"
                End If
            End If

            ''New
            'ISSSTONE
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "  S.STNWT AS GRSWT"
            'strSql += vbCrLf + "  ,S.STNWT AS NETWT"
            'strSql += vbCrLf + "  ,S.STNWT AS PUREWT"
            strSql += vbCrLf + "   (CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If
            strSql += vbCrLf + "   AS GRSWT"

            strSql += vbCrLf + "   ,(CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If

            strSql += vbCrLf + "   AS NETWT"

            strSql += vbCrLf + "   ,(CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If
            strSql += vbCrLf + "   AS PUREWT"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS METALID"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            If chkCategory <> "" Then strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            If chkMetalName <> "" Then strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
            strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
            strSql += vbCrLf + "  AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
            If Accode <> "" Then strSql += vbCrLf + " AND I.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If trantype.Trim <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
            If chkSales.Checked = True Then
                If ChkWithDirectPur.Checked Then
                Else
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('IPU','RPU')"
                End If
            Else
                If ChkWithDirectPur.Checked Then
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('SA')"
                Else
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('IPU','RPU','SA')"
                End If
            End If

            strSql += vbCrLf + "  UNION ALL"
            'RECEIPTSTONE
            strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "   -1*S.STNWT AS GRSWT"
            'strSql += vbCrLf + "  ,-1*S.STNWT AS NETWT"
            'strSql += vbCrLf + "  ,-1*S.STNWT AS PUREWT"
            strSql += vbCrLf + "   -1* (CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If
            strSql += vbCrLf + "   AS GRSWT   "
            strSql += vbCrLf + "   ,-1* (CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If
            strSql += vbCrLf + "   AS NETWT   "
            strSql += vbCrLf + "   ,-1* (CASE WHEN IM.METALID NOT IN ('D','T') THEN S.STNWT "
            ''strSql += vbCrLf + "   WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            If chkDiaWtConvert.Checked Then
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN (CASE WHEN S.STONEUNIT<>'C' THEN S.STNWT*5 ELSE S.STNWT END)"
            Else
                strSql += vbCrLf + "    WHEN IM.METALID IN ('D') THEN S.STNWT "
            End If
            If chkStnWtConv.Checked Then
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN (CASE WHEN S.STONEUNIT='C' THEN S.STNWT/5 ELSE S.STNWT END) END)"
            Else
                strSql += vbCrLf + "   WHEN IM.METALID IN ('T') THEN S.STNWT END)"
            End If
            strSql += vbCrLf + "   AS PUREWT"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS METALID"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            If chkCategory <> "" Then strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            If chkMetalName <> "" Then strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
            strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
            strSql += vbCrLf + "  AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
            If Accode <> "" Then strSql += vbCrLf + " AND I.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND I.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If trantype.Trim <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
            'strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('IPU','RPU')"
            If chkSales.Checked = True Then
                If ChkWithDirectPur.Checked Then
                Else
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('IPU','RPU')"
                End If
            Else
                If ChkWithDirectPur.Checked Then
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('SA')"
                Else
                    strSql += vbCrLf + "  AND S.TRANTYPE NOT IN ('IPU','RPU','SA')"
                End If
            End If
            strSql += vbCrLf + "  )X"
            strSql += vbCrLf + "  GROUP BY ACNAME,METALID"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSID & "SMITHAMTBAL') > 0 DROP TABLE TEMP" & SYSID & "SMITHAMTBAL"
            strSql += vbCrLf + "  SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) AMOUNT"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHAMTBAL"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN T"
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
            If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            If ChkRelatedtran.Checked Then
                If chkSales.Checked = True Then
                    If ChkWithDirectPur.Checked Then
                    Else
                        strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE "
                        strSql += vbCrLf + "  WHERE TRANTYPE IN ('IPU'))"
                    End If
                Else
                    If ChkWithDirectPur.Checked Then
                        strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE "
                        strSql += vbCrLf + "  WHERE TRANTYPE IN ('SA'))"
                    Else
                        strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE "
                        strSql += vbCrLf + "  WHERE TRANTYPE IN ('IPU','SA'))"
                    End If
                End If
                ''strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE "
                ''strSql += vbCrLf + "  WHERE TRANTYPE IN ('IPU','SA'))"
            End If
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkMetalName <> "" And ChkRelatedtran.Checked Then
                strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE "
                strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "' "
                If chkSales.Checked = True Then
                    If ChkWithDirectPur.Checked Then
                    Else
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('IPU','RPU')"
                    End If
                Else
                    If ChkWithDirectPur.Checked Then
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('SA')"
                    Else
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('IPU','RPU','SA')"
                    End If
                End If
                If chkWithAppAmt.Checked = False Then
                    strSql += vbCrLf + " AND TRANTYPE <> 'IAP' "
                End If
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
                strSql += vbCrLf + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
                strSql += vbCrLf + " WHERE METALNAME IN (" & chkMetalName & ")))"
                strSql += vbCrLf + " UNION "
                strSql += vbCrLf + " SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT "
                strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND COMPANYID = '" & strCompanyId & "' "
                If chkSales.Checked = True Then
                    If ChkWithDirectPur.Checked Then
                    Else
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('IPU','RPU')"
                    End If
                Else
                    If ChkWithDirectPur.Checked Then
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('SA')"
                    Else
                        strSql += vbCrLf + "  AND TRANTYPE NOT IN ('IPU','RPU','SA')"
                    End If
                End If
                If chkWithAppAmt.Checked = False Then
                    strSql += vbCrLf + " AND TRANTYPE <> 'RAP' "
                End If
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
                strSql += vbCrLf + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
                strSql += vbCrLf + " WHERE METALNAME IN (" & chkMetalName & "))))"
            End If
            strSql += vbCrLf + "  GROUP BY ACCODE"
            If ChkRelatedtran.Checked = False Then
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT CONVERT(NUMERIC(15,2),DEBIT-CREDIT) AS AMOUNT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)ACNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE T"
                strSql += vbCrLf + "  WHERE COMPANYID = '" & strCompanyId & "'"
                If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
                If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If LocalOutSt <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST ='" & LocalOutSt & "')"
            End If
            If chkWithAppAmt.Checked Then
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT -1*SUM(AMOUNT) AMOUNT"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT T WHERE TRANTYPE='R' AND STOCKTYPE='S' "
                strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='A' "
                If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
                If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                strSql += vbCrLf + "  GROUP BY ACCODE"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT SUM(AMOUNT) AMOUNT"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT T WHERE TRANTYPE='I' AND STOCKTYPE='S' "
                strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='A' "
                If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
                If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                strSql += vbCrLf + "  GROUP BY ACCODE"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT -1*SUM(AMOUNT) AMOUNT"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT T"
                strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
                If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND TRANTYPE='RAP'"
                strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN)"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + "  GROUP BY ACCODE"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT SUM(AMOUNT) AMOUNT"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE T"
                strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                If Accode <> "" Then strSql += vbCrLf + " AND T.ACCODE IN('" & Accode & "')"
                If acfilter <> "" Then strSql += vbCrLf + " AND T.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD " & acfilter & " )"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND TRANTYPE='IAP'"
                strSql += vbCrLf + "  AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN)"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + "  GROUP BY ACCODE"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL(GRSWT,NETWT,PUREWT,PARTICULAR,METALID,ACNAME)"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "  DISTINCT NULL GRSWT,NULL NETWT,NULL PUREWT"
            strSql += vbCrLf + "  ,ACNAME AS PARTICULAR,'G' METALID,ACNAME"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHAMTBAL"
            strSql += vbCrLf + " WHERE ACNAME NOT IN (SELECT ACNAME FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL)"
            'If chkWithNillBalance.Checked = False Then strSql += vbCrLf + " AND GRSWT <> 0 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & SYSID & "SMITHBALSUMMARY')>0 DROP TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY"
            strSql += vbCrLf + "  SELECT DISTINCT PARTICULAR,ACNAME INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  DECLARE @METALNAME VARCHAR(20)"
            strSql += vbCrLf + "  DECLARE @METALID VARCHAR(1)"
            strSql += vbCrLf + "  DECLARE @STR VARCHAR(8000)"
            strSql += vbCrLf + "  DECLARE CUR CURSOR"
            strSql += vbCrLf + "  FOR SELECT METALID,METALNAME FROM " & cnAdminDb & "..METALMAST  "
            If chkMetalName <> "" Then strSql += vbCrLf + "  WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & "))"
            strSql += vbCrLf + "  ORDER BY DISPLAYORDER"
            strSql += vbCrLf + "  OPEN CUR"
            strSql += vbCrLf + "  WHILE 1=1"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @METALID,@METALNAME"
            strSql += vbCrLf + "  IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + "  SELECT @STR = 'ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD ['+@METALNAME+'_GRSWT] NUMERIC(15,3)'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  SELECT @STR = 'ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD ['+@METALNAME+'_NETWT] NUMERIC(15,3)'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  SELECT @STR = 'ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD ['+@METALNAME+'_PUREWT] NUMERIC(15,3)'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  SELECT @STR = 'UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY SET ['+@METALNAME+'_GRSWT] = (SELECT SUM(ISNULL(GRSWT,0)) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL WHERE METALID = '''+@METALID+''' AND PARTICULAR = T.PARTICULAR) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY AS T'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  SELECT @STR = 'UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY SET ['+@METALNAME+'_NETWT] = (SELECT SUM(ISNULL(NETWT,0)) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL WHERE METALID = '''+@METALID+''' AND PARTICULAR = T.PARTICULAR) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY AS T'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  SELECT @STR = 'UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY SET ['+@METALNAME+'_PUREWT] = (SELECT SUM(ISNULL(PUREWT,0)) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBAL WHERE METALID = '''+@METALID+''' AND PARTICULAR = T.PARTICULAR) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY AS T'"
            strSql += vbCrLf + "  EXEC(@STR)"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  CLOSE CUR"
            strSql += vbCrLf + "  DEALLOCATE CUR"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = ""
            If chkAmtbal.Checked Then strSql += " ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD AMOUNT NUMERIC(15,2)"
            strSql += " ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD COLHEAD VARCHAR(3)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If chkAmtbal.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY SET AMOUNT = A.AMOUNT" + vbCrLf
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY T" + vbCrLf
                strSql += vbCrLf + "  INNER JOIN" + vbCrLf
                strSql += vbCrLf + "  (" + vbCrLf
                strSql += vbCrLf + "  SELECT"
                strSql += vbCrLf + "  SUM(AMOUNT)AMOUNT,ACNAME"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHAMTBAL"
                strSql += vbCrLf + "  GROUP BY ACNAME"
                strSql += vbCrLf + "  )A" + vbCrLf
                strSql += vbCrLf + "  ON T.ACNAME = A.ACNAME" + vbCrLf
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            Dim Filt As String = ""
            If Not chkWithNillBalance.Checked Then
                Dim DtColInfo As New DataTable
                'strSql = " SELECT * FROM TEMP" & SYSID & "SMITHBALSUMMARY WHERE 1<>1"
                strSql = vbCrLf + "  SELECT NAME FROM TEMPTABLEDB..SYSCOLUMNS WHERE ID IN "
                strSql += vbCrLf + "  (SELECT ID FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE='U' AND NAME='TEMP" & SYSID & "SMITHBALSUMMARY')"
                strSql += vbCrLf + "  AND ( 1<>1 "
                If ChkGrossWt.Checked Then strSql += vbCrLf + "  OR NAME LIKE '%GRSWT' "
                If ChkNetWt.Checked Then strSql += vbCrLf + "  OR NAME LIKE '%NETWT' "
                If ChkPureWt.Checked Then strSql += vbCrLf + "  OR NAME LIKE '%PUREWT'"
                strSql += vbCrLf + "  )"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DtColInfo)
                'For Each Col As DataColumn In DtColInfo.Columns
                '    Select Case Col.ColumnName
                '        Case "PARTICULAR", "ACNAME", "COLHEAD"
                '        Case Else
                '            Filt += " ISNULL([" & Col.ColumnName & "],0) = 0 AND" & Environment.NewLine
                '    End Select
                'Next
                For i As Integer = 0 To DtColInfo.Rows.Count - 1
                    If i <> DtColInfo.Rows.Count - 1 Then
                        Filt += " ISNULL([" & DtColInfo.Rows(i)("NAME").ToString & "],0) = 0 AND "
                    Else
                        Filt += " ISNULL([" & DtColInfo.Rows(i)("NAME").ToString & "],0) = 0  "
                    End If
                Next
                If chkAmtbal.Checked Then Filt += " AND ISNULL(AMOUNT,0) = 0  "
                'If Filt <> "" Then
                '    Filt = Mid(Filt, 1, Filt.Length - 3)
                'End If
            End If

            If RPT_SMITHABS_DAYS And chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
                strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPDAYS" & SYSID & "','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPDAYS" & SYSID & ""
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT ACCODE,MAX(TRANDATE)TRANDATE"
                strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME"
                strSql += vbCrLf + " ,DATEDIFF(DD,MAX(TRANDATE),'" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')DAYS   "
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPDAYS" & SYSID & " FROM ("
                strSql += vbCrLf + " SELECT ACCODE,TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE LEN(TRANTYPE)=3"
                'strSql += vbCrLf + " UNION ALL"
                'strSql += vbCrLf + " SELECT ACCODE,TRANDATE FROM " & cnStockDb & "..ISSUE WHERE LEN(TRANTYPE)=3 "
                strSql += vbCrLf + " )X GROUP BY ACCODE"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY ADD DAYS INT"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = " UPDATE TT SET DAYS=T.DAYS FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY TT,TEMPDAYS" & SYSID & " T"
                strSql += vbCrLf + " WHERE TT.ACNAME=T.ACNAME"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            Prop_Sets()
            Obj_MeFilterValues = New frmSmithBalanceSummaryReport_Properties
            GetSettingsObj(Obj_MeFilterValues, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))

            Dim dtCol As New DataColumn("KEYNO")
            dtCol.AutoIncrement = True
            dtCol.AutoIncrementSeed = 0
            dtCol.AutoIncrementStep = 1
            dtGrid.Columns.Add(dtCol)
            If chkGroupByAcType.Checked Then
                strSql = "   ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY  ADD RESULT INT DEFAULT NULL,ACTYPE VARCHAR(200) DEFAULT NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = "   UPDATE T SET RESULT=1 FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY AS T"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = "   UPDATE T SET ACTYPE=B.TYPENAME FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY AS T"
                strSql += vbCrLf + "   INNER JOIN " & cnAdminDb & "..ACHEAD AS A ON A.ACNAME=T.ACNAME "
                strSql += vbCrLf + "   INNER JOIN " & cnAdminDb & "..ACCTYPE AS B ON B.TYPEID=A.ACTYPE"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = "   INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY (PARTICULAR,ACTYPE,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT DISTINCT ACTYPE,ACTYPE,0,'T' FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                Dim dtMetal As DataTable
                strSql = "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")"
                dtMetal = New DataTable
                cmd = New OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtMetal)
                Dim colStr As String = "" : Dim colStrSum As String = ""
                For k As Integer = 0 To dtMetal.Rows.Count - 1
                    colStr += ",[" + dtMetal.Rows(k)("METALNAME").ToString + "_GRSWT]"
                    colStr += ",[" + dtMetal.Rows(k)("METALNAME").ToString + "_NETWT]"
                    colStr += ",[" + dtMetal.Rows(k)("METALNAME").ToString + "_PUREWT]"
                    colStrSum += ",SUM([" + dtMetal.Rows(k)("METALNAME").ToString + "_GRSWT]" + ") [" + dtMetal.Rows(k)("METALNAME").ToString + "_GRSWT]"
                    colStrSum += ",SUM([" + dtMetal.Rows(k)("METALNAME").ToString + "_NETWT]" + ") [" + dtMetal.Rows(k)("METALNAME").ToString + "_NETWT]"
                    colStrSum += ",SUM([" + dtMetal.Rows(k)("METALNAME").ToString + "_PUREWT]" + ") [" + dtMetal.Rows(k)("METALNAME").ToString + "_PUREWT]"
                Next
                If chkAmtbal.Checked Then
                    colStr += ",AMOUNT"
                    colStrSum += ",SUM(AMOUNT)AMOUNT"
                End If
                strSql = "   INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY (PARTICULAR,ACTYPE,RESULT" & colStr & ",COLHEAD)"
                strSql += vbCrLf + "   SELECT DISTINCT ACTYPE,ACTYPE,2" & colStrSum & ",'S' FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY GROUP BY ACTYPE "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If


            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALSUMMARY "
            If Not chkWithNillBalance.Checked Then
                strSql += " WHERE 1=1"
                strSql += " AND NOT (" & Filt & ")"
            End If
            If chkGroupByAcType.Checked Then
                strSql += " ORDER BY ACTYPE,RESULT,ACNAME "
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            ''Adding Grand Total
            Dim Row As DataRow = dtGrid.NewRow
            Dim tot As Decimal
            For Each col As DataColumn In dtGrid.Columns
                Select Case col.ColumnName.ToUpper
                    Case "PARTICULAR"
                        Row.Item(col.ColumnName) = "GRAND TOTAL"
                    Case "COLHEAD"
                        Row.Item(col.ColumnName) = "G"
                    Case "ACNAME", "KEYNO", "DAYS", "RESULT", "ACTYPE"

                    Case "AMOUNT"
                        tot = Val(dtGrid.Compute("SUM(AMOUNT)", "ISNULL(COLHEAD,'')=''").ToString)
                        Row.Item(col.ColumnName) = IIf(tot <> 0, Format(tot, "0.00"), DBNull.Value)
                    Case Else
                        If col.ColumnName.Contains("GRSWT") Then
                            tot = Val(dtGrid.Compute("SUM([" & col.ColumnName & "])", "ISNULL(COLHEAD,'')=''").ToString)
                            Row.Item(col.ColumnName) = IIf(tot <> 0, Format(tot, "0.000"), DBNull.Value)
                        ElseIf col.ColumnName.Contains("NETWT") Then
                            tot = Val(dtGrid.Compute("SUM([" & col.ColumnName & "])", "ISNULL(COLHEAD,'')=''").ToString)
                            Row.Item(col.ColumnName) = IIf(tot <> 0, Format(tot, "0.000"), DBNull.Value)
                        ElseIf col.ColumnName.Contains("PUREWT") Then
                            tot = Val(dtGrid.Compute("SUM([" & col.ColumnName & "])", "ISNULL(COLHEAD,'')=''").ToString)
                            Row.Item(col.ColumnName) = IIf(tot <> 0, Format(tot, "0.000"), DBNull.Value)
                        Else
                            tot = Val(dtGrid.Compute("SUM([" & col.ColumnName & "])", "ISNULL(COLHEAD,'')=''").ToString)
                            Row.Item(col.ColumnName) = IIf(tot <> 0, Format(tot, "0.00"), DBNull.Value)
                        End If
                End Select
            Next
            dtGrid.Rows.Add(Row)
            dtGrid.AcceptChanges()
        End If

        Prop_Sets()
        Obj_MeFilterValues = New frmSmithBalanceSummaryReport_Properties
        GetSettingsObj(Obj_MeFilterValues, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.Text = "SMITH BALANCE SUMMARY"
        Dim tit As String
        If chkWithAbs.Checked = True Then
            tit += "SMITH ABSTRACT FROM " & dtpAsOnDate.Text & " TO " & dtpTodate.Text
        ElseIf chkWithAbs.Checked = False And chkWithSpecicFormat.Checked Then
            tit += " SMITH BALANCE SUMMARY AS ON " & dtpAsOnDate.Text
        Else
            tit += " SMITH BALANCE SUMMARY AS ON " & dtpAsOnDate.Text
        End If
        tit += " WITH"
        If ChkGrossWt.Checked Then
            tit += "  GROSS WEIGHT,"
        End If
        If ChkNetWt.Checked Then
            tit += "  NET WEIGHT,"
        End If
        If ChkPureWt.Checked Then
            tit += "  PURE WEIGHT,"
        End If
        tit = Mid(tit, 1, tit.Length - 1)
        If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.gridView.DataSource = dtGrid
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = True
        objGridShower.formuser = userId
        objGridShower.WindowState = FormWindowState.Maximized
        If chkWithAbs.Checked = False Then objGridShower.lblStatus.Text = "<Press [D] for Detail View>"
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator_F2(objGridShower.gridViewHeader, objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, , , False)
        objGridShower.Show()
        If objGridShower.gridView.Columns.Contains("ACTYPE") Then objGridShower.gridView.Columns("ACTYPE").Visible = False
        If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
        If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
        If objGridShower.gridView.Columns.Contains("LASTTRANDATE") And SPECIFICFORMAT <> "1" Then
            objGridShower.gridView.Columns("LASTTRANDATE").Visible = False
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.AutoResize()
    End Sub
    Private Sub GridViewHeaderCreator_F2(ByVal gridviewHead As DataGridView, ByVal gridview As DataGridView)
        Dim dtHead As New DataTable
        If chkWithAbs.Checked Or chkWithSpecicFormat.Checked Then
            strSql = "SELECT ''[ACNAME]"
            If ChkGrossWt.Checked Then
                strSql += " ,''[OPENING_GRSWT~ISSUE_GRSWT~RECEIPT_GRSWT~CLOSING_GRSWT]"
            End If
            If ChkNetWt.Checked Then
                strSql += " ,''[OPENING_NETWT~ISSUE_NETWT~RECEIPT_NETWT~CLOSING_NETWT]"
            End If
            If ChkPureWt.Checked Then
                strSql += " ,''[OPENING_PUREWT~ISSUE_PUREWT~RECEIPT_PUREWT~CLOSING_PUREWT]"
            End If
            If gridview.Columns.Contains("AMOUNT") Then
                strSql += " ,''[AMOUNT]"
            End If
            If gridview.Columns.Contains("DAYS") Then
                strSql += " ,''[DAYS]"
            End If
            If gridview.Columns.Contains("LASTTRANDATE") And SPECIFICFORMAT = "1" Then
                strSql += " ,''[LASTTRANDATE]"
            End If
            If gridview.Columns.Contains("TYPENAME") Then
                strSql += " ,''[TYPENAME]"
            End If
            strSql += " ,''SCROLL"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            With gridviewHead
                .DataSource = Nothing
                .DataSource = dtHead
                .Columns("ACNAME").HeaderText = ""
                If ChkGrossWt.Checked Then
                    .Columns("OPENING_GRSWT~ISSUE_GRSWT~RECEIPT_GRSWT~CLOSING_GRSWT").HeaderText = "GRSWT"
                End If
                If ChkNetWt.Checked Then
                    .Columns("OPENING_NETWT~ISSUE_NETWT~RECEIPT_NETWT~CLOSING_NETWT").HeaderText = "NETWT"
                End If
                If ChkPureWt.Checked Then
                    .Columns("OPENING_PUREWT~ISSUE_PUREWT~RECEIPT_PUREWT~CLOSING_PUREWT").HeaderText = "PUREWT"
                End If
                If gridview.Columns.Contains("AMOUNT") Then
                    .Columns("AMOUNT").HeaderText = "AMOUNT"
                End If
                If gridview.Columns.Contains("DAYS") Then
                    .Columns("DAYS").HeaderText = ""
                End If
                If gridview.Columns.Contains("LASTTRANDATE") And SPECIFICFORMAT = "1" Then
                    .Columns("LASTTRANDATE").HeaderText = ""
                End If
                If gridview.Columns.Contains("TYPENAME") Then
                    .Columns("TYPENAME").HeaderText = ""
                End If

                .Columns("SCROLL").HeaderText = ""
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .Columns("ACNAME").Width = gridview.Columns("ACNAME").Width
                If ChkGrossWt.Checked Then
                    .Columns("OPENING_GRSWT~ISSUE_GRSWT~RECEIPT_GRSWT~CLOSING_GRSWT").Width =
                    IIf(gridview.Columns("OPENING_GRSWT").Visible = True, gridview.Columns("OPENING_GRSWT").Width, 0) _
                    + IIf(gridview.Columns("RECEIPT_GRSWT").Visible = True, gridview.Columns("RECEIPT_GRSWT").Width, 0) _
                    + IIf(gridview.Columns("ISSUE_GRSWT").Visible = True, gridview.Columns("ISSUE_GRSWT").Width, 0) _
                    + IIf(gridview.Columns("CLOSING_GRSWT").Visible = True, gridview.Columns("CLOSING_GRSWT").Width, 0)
                End If
                If ChkNetWt.Checked Then
                    .Columns("OPENING_NETWT~ISSUE_NETWT~RECEIPT_NETWT~CLOSING_NETWT").Width =
                    IIf(gridview.Columns("OPENING_NETWT").Visible = True, gridview.Columns("OPENING_NETWT").Width, 0) _
                    + IIf(gridview.Columns("RECEIPT_NETWT").Visible = True, gridview.Columns("RECEIPT_NETWT").Width, 0) _
                    + IIf(gridview.Columns("ISSUE_NETWT").Visible = True, gridview.Columns("ISSUE_NETWT").Width, 0) _
                    + IIf(gridview.Columns("CLOSING_NETWT").Visible = True, gridview.Columns("CLOSING_NETWT").Width, 0)
                End If
                If ChkPureWt.Checked Then
                    .Columns("OPENING_PUREWT~ISSUE_PUREWT~RECEIPT_PUREWT~CLOSING_PUREWT").Width =
                    IIf(gridview.Columns("OPENING_PUREWT").Visible = True, gridview.Columns("OPENING_PUREWT").Width, 0) _
                    + IIf(gridview.Columns("RECEIPT_PUREWT").Visible = True, gridview.Columns("RECEIPT_PUREWT").Width, 0) _
                    + IIf(gridview.Columns("ISSUE_PUREWT").Visible = True, gridview.Columns("ISSUE_PUREWT").Width, 0) _
                    + IIf(gridview.Columns("CLOSING_PUREWT").Visible = True, gridview.Columns("CLOSING_PUREWT").Width, 0)
                End If
                If gridview.Columns.Contains("AMOUNT") Then
                    .Columns("AMOUNT").Width = gridview.Columns("AMOUNT").Width
                End If
                If gridview.Columns.Contains("DAYS") Then
                    .Columns("DAYS").Width = gridview.Columns("DAYS").Width
                End If
                If gridview.Columns.Contains("LASTTRANDATE") And SPECIFICFORMAT = "1" Then
                    .Columns("LASTTRANDATE").Width = gridview.Columns("LASTTRANDATE").Width
                    gridview.Columns("LASTTRANDATE").HeaderText = "LAST TRANDATE"
                End If
                If gridview.Columns.Contains("TYPENAME") Then
                    .Columns("TYPENAME").Width = gridview.Columns("TYPENAME").Width
                End If
                If chkWithSpecicFormat.Checked Then
                    'SetGridHeadColWidth_F1(gridviewHead)
                Else
                    SetGridHeadColWidth(gridviewHead)
                End If
            End With
        Else
            Dim ChkMetal As String = ""
            Dim ChkMetalStr() As String
            For i As Integer = 0 To chkLstMetal.CheckedItems.Count - 1
                ChkMetal += chkLstMetal.CheckedItems.Item(i).ToString + ","
            Next
            ChkMetal = Mid(ChkMetal, 1, Len(ChkMetal) - 1)
            ChkMetalStr = ChkMetal.Split(",")
            strSql = "SELECT ''[PARTICULAR]"
            For j As Integer = 0 To ChkMetalStr.Length - 1
                strSql += " ,''[" & ChkMetalStr(j) & "_GRSWT~" & ChkMetalStr(j) & "_NETWT~" & ChkMetalStr(j) & "_PUREWT]"
            Next
            If gridview.Columns.Contains("AMOUNT") Then
                strSql += " ,''[AMOUNT]"
            End If
            If gridview.Columns.Contains("DAYS") Then
                strSql += " ,''[DAYS]"
            End If
            If gridview.Columns.Contains("TYPENAME") Then
                strSql += " ,''[TYPENAME]"
            End If
            strSql += " ,''SCROLL"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            With gridviewHead
                .DataSource = Nothing
                .DataSource = dtHead
                .Columns("PARTICULAR").HeaderText = ""
                For j As Integer = 0 To ChkMetalStr.Length - 1
                    .Columns("" & ChkMetalStr(j) & "_GRSWT~" & ChkMetalStr(j) & "_NETWT~" & ChkMetalStr(j) & "_PUREWT").HeaderText = ChkMetalStr(j)
                Next
                If gridview.Columns.Contains("AMOUNT") Then
                    .Columns("AMOUNT").HeaderText = "AMOUNT"
                End If
                If gridview.Columns.Contains("DAYS") Then
                    .Columns("DAYS").HeaderText = ""
                End If
                If gridview.Columns.Contains("TYPENAME") Then
                    .Columns("TYPENAME").HeaderText = ""
                End If
                .Columns("SCROLL").HeaderText = ""
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
                For j As Integer = 0 To ChkMetalStr.Length - 1
                    'If ChkMetalStr(j) <> "DIAMOND" And ChkMetalStr(j) <> "STONE" Then
                    If gridview.Columns.Contains(ChkMetalStr(j)) Then
                        .Columns(ChkMetalStr(j) & "_GRSWT~" & ChkMetalStr(j) & "_NETWT~" & ChkMetalStr(j) & "_PUREWT").Width =
                    IIf(gridview.Columns(ChkMetalStr(j) & "_GRSWT").Visible = True, gridview.Columns(ChkMetalStr(j) & "_GRSWT").Width, 0) _
                    + IIf(gridview.Columns(ChkMetalStr(j) & "_NETWT").Visible = True, gridview.Columns(ChkMetalStr(j) & "_NETWT").Width, 0) _
                    + IIf(gridview.Columns(ChkMetalStr(j) & "_PUREWT").Visible = True, gridview.Columns(ChkMetalStr(j) & "_PUREWT").Width, 0)
                    End If
                    'End If
                Next
                If gridview.Columns.Contains("AMOUNT") Then
                    .Columns("AMOUNT").Width = gridview.Columns("AMOUNT").Width
                End If
                If gridview.Columns.Contains("DAYS") Then
                    .Columns("DAYS").Width = gridview.Columns("DAYS").Width
                End If
                If gridview.Columns.Contains("TYPENAME") Then
                    .Columns("TYPENAME").Width = gridview.Columns("TYPENAME").Width
                End If
                SetGridHeadColWidth(gridviewHead)
            End With
        End If
    End Sub
    Public Function GetSelectedMETALId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function
    Public Function GetSelectedcatId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function

    Public Function GetSelectedcostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function

    Private Sub DataGridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If CType(dgv.DataSource, DataTable).TableName = "DETAILED" Then
            If dgv.CurrentRow Is Nothing Then Exit Sub
            Dim rw As Integer = e.RowIndex
            objGridShower.lblStatus.Visible = False
            objGridShower.txtRemark1.Text = ""
            objGridShower.txtRemark2.Text = ""
            If dgv.Rows(rw).Cells("COLHEAD").Value.ToString.Trim <> "" Then Exit Sub
            objGridShower.txtRemark1.Text = dgv.Rows(rw).Cells("REMARK1").Value.ToString
            objGridShower.txtRemark2.Text = dgv.Rows(rw).Cells("REMARK2").Value.ToString

            objGridShower.txtRemark1.Visible = True
            objGridShower.txtRemark2.Visible = True
            objGridShower.lblRemark1.Visible = True
            objGridShower.lblRemark2.Visible = True
        End If

    End Sub
    Private Sub DataGridView_RowEnter_F1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If chkWithSpecicFormat.Checked Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            Dim rw As Integer = e.RowIndex
            If dgv.RowCount > 0 Then
                With dgv.Rows(rw)
                    If .Cells("COLHEAD").Value.ToString.Trim <> "" Then

                        objGridDetailShower.lblCatName.Text = ""
                        objGridDetailShower.lblOCatName.Text = ""
                        objGridDetailShower.lblItem.Text = ""
                        objGridDetailShower.lblTrantype.Text = ""
                        objGridDetailShower.lblRemark.Text = ""
                        objGridDetailShower.lblAlloy.Text = ""
                        objGridDetailShower.lblTouch.Text = ""
                        objGridDetailShower.lblBillno.Text = ""
                        objGridDetailShower.lblTranno.Text = ""

                        objGridDetailShower.lblGrswt.Text = ""
                        objGridDetailShower.lblStnwt.Text = ""
                        objGridDetailShower.lblNetwt.Text = ""
                        objGridDetailShower.lblPurewt.Text = ""
                        objGridDetailShower.lblWtMode.Text = ""
                        objGridDetailShower.lblPcs.Text = ""
                        objGridDetailShower.lblWastage.Text = ""
                        objGridDetailShower.lblPacketno.Text = ""
                        objGridDetailShower.lblsalesman.Text = ""

                        objGridDetailShower.lblApproval.Text = ""
                        objGridDetailShower.lblTranmode.Text = ""
                        objGridDetailShower.lblStonemode.Text = ""
                        objGridDetailShower.lblMetalamt.Text = ""
                        objGridDetailShower.lblMc.Text = ""
                        objGridDetailShower.lblStnamt.Text = ""
                        objGridDetailShower.lblOthCharge.Text = ""
                        objGridDetailShower.lblTax.Text = ""
                        objGridDetailShower.lblTotAmt.Text = ""


                    Else
                        objGridDetailShower.lblCatName.Text = .Cells("CATNAME").Value.ToString
                        objGridDetailShower.lblOCatName.Text = .Cells("OCATNAME").Value.ToString
                        objGridDetailShower.lblItem.Text = .Cells("ITEM").Value.ToString & IIf(.Cells("SUBITEM").Value.ToString <> "", " -" & .Cells("SUBITEM").Value.ToString, "")
                        objGridDetailShower.lblTrantype.Text = ""
                        objGridDetailShower.lblRemark.Text = .Cells("REMARK1").Value.ToString
                        objGridDetailShower.lblAlloy.Text = IIf(Val(.Cells("RALLOY").Value.ToString) <> 0, .Cells("RALLOY").Value.ToString, .Cells("IALLOY").Value.ToString)
                        objGridDetailShower.lblTouch.Text = .Cells("TOUCH").Value.ToString
                        objGridDetailShower.lblBillno.Text = .Cells("REFNO").Value.ToString
                        objGridDetailShower.lblTranno.Text = .Cells("TRANNO").Value.ToString

                        objGridDetailShower.lblGrswt.Text = .Cells("GRSWT").Value.ToString
                        objGridDetailShower.lblStnwt.Text = .Cells("STNWT").Value.ToString
                        objGridDetailShower.lblNetwt.Text = .Cells("NETWT").Value.ToString
                        objGridDetailShower.lblPurewt.Text = .Cells("PUREWT").Value.ToString
                        objGridDetailShower.lblWtMode.Text = ""
                        objGridDetailShower.lblPcs.Text = .Cells("PCS").Value.ToString
                        objGridDetailShower.lblWastage.Text = IIf(.Cells("RWASTAGE").Value.ToString <> "", .Cells("RWASTAGE").Value.ToString, .Cells("IWASTAGE").Value.ToString)
                        objGridDetailShower.lblPacketno.Text = ""
                        Dim msname As String
                        msname = IIf(.Cells("Empname").Value.ToString <> "", .Cells("Empname").Value.ToString.Trim, "")
                        If msname <> "" Then msname += IIf(.Cells("USERNAME").Value.ToString <> "", "/" & .Cells("USERNAME").Value.ToString.Trim, "") Else msname = IIf(.Cells("USERNAME").Value.ToString <> "", .Cells("USERNAME").Value.ToString.Trim, "")
                        objGridDetailShower.lblsalesman.Text = msname

                        objGridDetailShower.lblApproval.Text = ""
                        objGridDetailShower.lblTranmode.Text = ""
                        objGridDetailShower.lblStonemode.Text = ""
                        objGridDetailShower.lblMetalamt.Text = IIf((Val(.Cells("AMOUNT").Value.ToString) - Val(.Cells("TAX").Value.ToString) - Val(.Cells("STNAMT").Value.ToString) - Val(.Cells("MC").Value.ToString)) <> 0, Format(Val(.Cells("AMOUNT").Value.ToString) - Val(.Cells("TAX").Value.ToString) - Val(.Cells("STNAMT").Value.ToString) - Val(.Cells("MC").Value.ToString), "#0.00"), "")
                        objGridDetailShower.lblMc.Text = IIf(Val(.Cells("MC").Value.ToString) <> 0, .Cells("MC").Value.ToString, "")
                        objGridDetailShower.lblStnamt.Text = IIf(Val(.Cells("STNAMT").Value.ToString) <> 0, .Cells("STNAMT").Value.ToString, "")
                        objGridDetailShower.lblOthCharge.Text = IIf(Val(.Cells("MISCAMT").Value.ToString) <> 0, .Cells("MISCAMT").Value.ToString, "")
                        objGridDetailShower.lblTax.Text = IIf(Val(.Cells("TAX").Value.ToString) <> 0, .Cells("TAX").Value.ToString, "")
                        objGridDetailShower.lblTotAmt.Text = IIf(Val(.Cells("AMOUNT").Value.ToString) <> 0, .Cells("AMOUNT").Value.ToString, "")
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        If e.KeyCode = Keys.Escape Then
            objGridShower.Close()
            btnSearch_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.D And dispflag = True Then
            dispflag = False
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If chkWithSpecicFormat.Checked Then
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
                Dim Acname As String = dgv.CurrentRow.Cells("ACNAME").Value.ToString
                FuncSpecificFormat(Acname, dgv)
                objGridShower.Close()

                Exit Sub
            End If
            If CType(dgv.DataSource, DataTable).TableName = "SUMMARY" Then
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                Dim Acname As String = dgv.CurrentRow.Cells("ACNAME").Value.ToString
                Dim dt As DataTable = FillDetailView(Acname)
                If dt Is Nothing Then MsgBox("Record not found", MsgBoxStyle.Information) : Exit Sub
                If Not dt.Rows.Count > 0 Then MsgBox("Record not found", MsgBoxStyle.Information) : Exit Sub

                If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                f.Text = "SMITH BALANCE DETAILED"
                ''Dim tit As String = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
                Dim tit As String = ""
                If Not chkWithAbs.Checked Then
                    tit = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
                Else
                    tit = "SMITH BALANCE DETAILED FROM " & dtpAsOnDate.Text & " TO " & dtpTodate.Text
                End If
                tit += "BASED ON"
                If ChkGrossWt.Checked Then
                    tit += " GROSS WEIGHT,"
                End If
                If ChkNetWt.Checked Then
                    tit += " NET WEIGHT,"
                End If
                If ChkPureWt.Checked Then
                    tit += " PURE WEIGHT,"
                End If
                tit = Mid(tit, 1, Len(tit) - 1)
                tit += vbCrLf + "FOR " & Acname
                If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
                f.lblTitle.Text = tit
                f.dsGrid.Tables.Add(dt)
                f.FormReSize = False
                f.FormReLocation = True
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_DetailViewFormatting(f.gridView)
                'If rbtPureWeight.Checked = False Then
                AddHandler f.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
                f.gridViewHeader.Visible = True
                GridViewHeaderCreator(f.gridViewHeader)
                'End If
                For Each dgvRow As DataGridViewRow In dgv.Rows
                    If dgvRow.Cells("TFILTER").Value.ToString = "Y" Then
                        dgvRow.Cells("RPUREWT").Style.BackColor = Color.MistyRose
                        dgvRow.Cells("RPCS").Style.BackColor = Color.MistyRose
                        dgvRow.Cells("RGRSWT").Style.BackColor = Color.MistyRose
                        dgvRow.Cells("RWASTAGE").Style.BackColor = Color.MistyRose
                        dgvRow.Cells("RALLOY").Style.BackColor = Color.MistyRose
                        dgvRow.Cells("RNETWT").Style.BackColor = Color.MistyRose
                    End If
                Next

                AddHandler objGridShower.gridView.RowEnter, AddressOf DataGridView_RowEnter

                f.lblStatus.Text = "<Press [ESCAPE] for Summary View> , <Marked [RED] column has not considered to Totals>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
            Else
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & dgv.CurrentRow.Cells("ACNAME").Value.ToString & "'"
                Dim Acname As String = dgv.CurrentRow.Cells("ACNAME").Value.ToString
                Dim Accode As String = GetSqlValue(cn, strSql)
                AcTypeFilteration = ""
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
                For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
                    selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
                Next
                If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
                trantype = GetTranType(selTran)
                If trantype = "" Then trantype = "''"
                chkCategory = GetChecked_CheckedList(chkLstCategory)
                chkCostName = GetChecked_CheckedList(chkLstCostCentre)
                chkMetalName = GetChecked_CheckedList(chkLstMetal)
                If chkWithAbs.Checked = True Then
                    Selectedcatid = GetSelectedcatId(chkLstCategory, False)
                    Selectedmetalid = GetSelectedMETALId(chkLstMetal, False)
                    SelectedCostId = GetSelectedcostId(chkLstCostCentre, False)

                    chkwt = ""
                    If ChkGrossWt.Checked Then chkwt = "G"
                    If ChkNetWt.Checked Then chkwt = "N"
                    If ChkPureWt.Checked Then chkwt = "P"
                    strSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHABSTRACT_DETAIL"
                    strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
                    strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & SYSID & "SMITHABSTRACT'"
                    strSql += vbCrLf + ",@FROMDATE='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + ",@TODATE='" & dtpTodate.Value.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
                    strSql += vbCrLf + ",@METALID='" & Selectedmetalid & "'"
                    strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
                    strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
                    strSql += vbCrLf + ",@ACFILTER=""" & AcTypeFilteration & """"
                    strSql += vbCrLf + ",@TRANFILTER=" & Replace(trantype, "','", ",") & ""
                    strSql += vbCrLf + ",@CATFILTER='" & Selectedcatid & "'"
                    strSql += vbCrLf + ",@ACCODE='" & Accode & "'"
                    strSql += vbCrLf + ",@PUREWTPER='" & PurewtPer & "'"
                    strSql += vbCrLf + ",@WITHWAST='" & IIf(ChkwithWast.Checked, "Y", "N") & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim ds1 As New DataSet()
                    Dim dt As New DataTable("DETAILED")
                    da.Fill(ds1)
                    dt = ds1.Tables(0)
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                    f.Text = "SMITH BALANCE DETAILED"
                    ''Dim tit As String = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
                    Dim tit As String = ""
                    If Not chkWithAbs.Checked Then
                        tit = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
                    Else
                        tit = "SMITH BALANCE DETAILED FROM " & dtpAsOnDate.Text & " TO " & dtpTodate.Text
                    End If
                    tit += "BASED ON "
                    If ChkGrossWt.Checked Then
                        tit += "  GROSS WEIGHT,"
                    End If
                    If ChkNetWt.Checked Then
                        tit += "  NET WEIGHT,"
                    End If
                    If ChkPureWt.Checked Then
                        tit += "  PURE WEIGHT,"
                    End If
                    tit = Mid(tit, 1, tit.Length - 1)
                    tit += vbCrLf + "FOR " & Acname
                    f.lblTitle.Text = tit
                    Dim dt1 As New DataTable()
                    dt1 = dt.Copy()
                    f.dsGrid.Tables.Add(dt1)
                    f.FormReSize = False
                    f.WindowState = FormWindowState.Maximized
                    f.FormReLocation = True
                    f.gridView.DataSource = Nothing
                    f.gridView.DataSource = f.dsGrid.Tables(0)
                    f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                    ' DataGridView_DetailViewFormatting(f.gridView)
                    FormatGridColumns(dgv, False, False, , False)
                    'FillGridGroupStyle_KeyNoWise(dgv)
                    'If rbtPureWeight.Checked = False Then
                    'f.AutoResize()
                    AddHandler f.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
                    f.gridViewHeader.Visible = True
                    GridViewHeader(f.gridViewHeader, f.gridView)
                    'End If
                    Dim RGrsWt, RNetWt, RPureWt As Decimal
                    For i As Integer = 0 To f.gridView.Rows.Count - 1
                        Select Case f.gridView.Rows(i).Cells("COLHEAD").Value
                            Case "S"
                                f.gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                                f.gridView.Rows(i).Cells("PARTICULAR").Style.ForeColor = Color.Red
                                f.gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        End Select
                        With f.gridView.Rows(i)
                            Dim GrsWt, NetWt, PureWt As Decimal
                            GrsWt = 0
                            NetWt = 0
                            PureWt = 0
                            If Not i = f.gridView.Rows.Count - 1 Then
                                GrsWt = Val(.Cells("IGRWT").Value.ToString) - Val(.Cells("RGRWT").Value.ToString)
                                NetWt = Val(.Cells("INETWT").Value.ToString) - Val(.Cells("RNETWT").Value.ToString)
                                PureWt = Val(.Cells("IPUREWT").Value.ToString) - Val(.Cells("RPUREWT").Value.ToString)
                            End If
                            RGrsWt += GrsWt
                            RNetWt += NetWt
                            RPureWt += PureWt
                            'If .Cells("COLHEAD").Value.ToString = "S" Then
                            '    .Cells("IGRWT").Value = DBNull.Value
                            '    .Cells("INETWT").Value = DBNull.Value
                            '    .Cells("IPUREWT").Value = DBNull.Value
                            '    .Cells("RGRWT").Value = DBNull.Value
                            '    .Cells("RNETWT").Value = DBNull.Value
                            '    .Cells("RPUREWT").Value = DBNull.Value
                            'End If
                            .Cells("RRGRWT").Value = Format(RGrsWt, "#0.000")
                            .Cells("RRNETWT").Value = Format(RNetWt, "#0.000")
                            .Cells("RRPUREWT").Value = Format(RPureWt, "#0.000")
                        End With
                    Next
                    FillGridGroupStyle_KeyNoWise(dgv)
                    f.gridView.Columns("COLHEAD").Visible = False
                    f.gridView.Columns("RESULT").Visible = False

                    f.lblStatus.Text = "<Press [ESCAPE] for Summary View> , <Marked [RED] column has not considered to Totals>"
                    f.FormReSize = True
                    'f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                    f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                    f.gridView.Select()
                End If
            End If
        End If
    End Sub
    Private Sub DataGridView_KeyDown_F1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            objGridDetailShower.Close()
            btnSearch_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub FuncSpecificFormat(ByVal Acname As String, ByVal dgv As DataGridView)

        objGridDetailShower = New frmSmithBalSummery_F1
        objGridDetailShower.Name = Me.Name
        'objGridDetailShower = objGPack.GetParentControl(dgv)

        Dim dt As DataTable = FillDetailView_F1(Acname)
        If dt Is Nothing Then MsgBox("Record not found", MsgBoxStyle.Information) : Exit Sub
        If Not dt.Rows.Count > 0 Then MsgBox("Record not found", MsgBoxStyle.Information) : Exit Sub

        If objGridDetailShower.dsGrid.Tables.Contains(dt.TableName) Then objGridDetailShower.dsGrid.Tables.Remove(dt.TableName)
        objGridDetailShower.Text = "SMITH BALANCE DETAILED"
        ''Dim tit As String = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
        Dim tit As String = ""
        If Not chkWithAbs.Checked Then
            tit = "SMITH BALANCE DETAILED AS ON " & dtpAsOnDate.Text
        Else
            tit = "SMITH BALANCE DETAILED FROM " & dtpAsOnDate.Text & " TO " & dtpTodate.Text
        End If
        tit += "BASED ON "
        If ChkGrossWt.Checked Then
            tit += "  GROSS WEIGHT,"
        End If
        If ChkNetWt.Checked Then
            tit += "  NET WEIGHT,"
        End If
        If ChkPureWt.Checked Then
            tit += "  PURE WEIGHT,"
        End If
        tit = Mid(tit, 1, tit.Length - 1)
        tit += vbCrLf + "FOR " & Acname
        If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridDetailShower.lblTitle.Text = tit
        objGridDetailShower.dsGrid.Tables.Add(dt)
        objGridDetailShower.FormReSize = False
        objGridDetailShower.FormReLocation = True
        objGridDetailShower.gridView.DataSource = Nothing
        objGridDetailShower.gridView.DataSource = objGridDetailShower.dsGrid.Tables("DETAILED")
        objGridDetailShower.gridView.CurrentCell = objGridDetailShower.gridView.FirstDisplayedCell
        DataGridView_DetailViewFormatting_f1(objGridDetailShower.gridView)
        'If rbtPureWeight.Checked = False Then
        AddHandler objGridDetailShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        AddHandler objGridDetailShower.gridView.KeyDown, AddressOf DataGridView_KeyDown_F1
        objGridDetailShower.gridViewHeader.Visible = True
        GridViewHeaderCreator_F1(objGridDetailShower.gridViewHeader)
        'End If
        For Each dgvRow As DataGridViewRow In objGridDetailShower.gridView.Rows
            If dgvRow.Cells("TFILTER").Value.ToString = "Y" Then
                dgvRow.Cells("RECWT").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RPCS").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RGRSWT").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RWASTAGE").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RALLOY").Style.BackColor = Color.MistyRose
                dgvRow.Cells("RNETWT").Style.BackColor = Color.MistyRose
            End If
        Next

        AddHandler objGridDetailShower.gridView.RowEnter, AddressOf DataGridView_RowEnter_F1
        'f.lblStatus.Text = "<Press [ESCAPE] for Summary View> , <Marked [RED] column has not considered to Totals>"
        'objGridDetailShower.FormReSize = True
        'objGridDetailShower.gridView.Columns(0).Width = objGridDetailShower.gridView.Columns(0).Width + 1
        'objGridDetailShower.gridView.CurrentCell = objGridDetailShower.gridView.FirstDisplayedCell
        'objGridDetailShower.gridView.Select()
        objGridDetailShower.pnlFooter.Visible = True
        objGridDetailShower.WindowState = FormWindowState.Maximized
        objGridDetailShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridDetailShower.gridView)
    End Sub

    Private Function FillDetailView_F1(ByVal AcName As String) As DataTable
        Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim chkCategory As String = GetQryString(Obj_MeFilterValues.p_chkLstCategory)
        Dim chkCostName As String = GetQryString(Obj_MeFilterValues.p_chkLstCostCentre)
        If chkCostName = "" Then chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetQryString(Obj_MeFilterValues.p_chkLstMetal)
        Dim selTran As String = ""
        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        Dim trantype As String = GetTranType(selTran)
        Dim Apptrantype As String = ""
        ''If ChkApproval.Checked = False Then Apptrantype = "IAP,RAP,AI,AR"
        If ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP,AI,AR"
        ElseIf ChkApproval.Checked = False Then
            Apptrantype = "AI,AR"
        ElseIf chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP"
        End If
        If Apptrantype = "" Then Apptrantype = "''"
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET')>0 DROP TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(75),'OPENING..')PARTICULAR,CONVERT(VARCHAR(75),'')CATNAME,CONVERT(VARCHAR(75),'')OCATNAME,CONVERT(INT,NULL) TRANNO,CONVERT(VARCHAR(10),NULL)REFNO,CONVERT(VARCHAR(12),NULL)TDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),NULL)[ITEM],CONVERT(VARCHAR(50),NULL)[SUBITEM]"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(ISSWT)-SUM(RECWT) > 0 THEN SUM(ISSWT)-SUM(RECWT) ELSE 0 END) AS ISSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECWT)-SUM(ISSWT) > 0 THEN SUM(RECWT)-SUM(ISSWT) ELSE 0 END) AS RECWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN SUM(DEBIT)-SUM(CREDIT) > 0 THEN SUM(DEBIT)-SUM(CREDIT) ELSE 0 END)DEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN SUM(CREDIT)-SUM(DEBIT) > 0 THEN SUM(CREDIT)-SUM(DEBIT) ELSE 0 END)CREDIT"
        strSql += vbCrLf + " ,CONVERT(INT,CASE WHEN SUM(IPCS)-SUM(RPCS) > 0 THEN SUM(IPCS)-SUM(RPCS) ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IGRSWT)-SUM(RGRSWT) > 0 THEN SUM(IGRSWT)-SUM(RGRSWT) ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(INETWT)-SUM(RNETWT) > 0 THEN SUM(INETWT)-SUM(RNETWT) ELSE 0 END) AS INETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IPUREWT)-SUM(RPUREWT) > 0 THEN SUM(IPUREWT)-SUM(RPUREWT) ELSE 0 END) AS IPUREWT"
        strSql += vbCrLf + " ,CONVERT(INT,CASE WHEN SUM(RPCS)-SUM(IPCS) > 0 THEN SUM(RPCS)-SUM(IPCS) ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RGRSWT)-SUM(IGRSWT) > 0 THEN SUM(RGRSWT)-SUM(IGRSWT) ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RNETWT)-SUM(INETWT) > 0 THEN SUM(RNETWT)-SUM(INETWT) ELSE 0 END) AS RNETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RPUREWT)-SUM(IPUREWT) > 0 THEN SUM(RPUREWT)-SUM(IPUREWT) ELSE 0 END) AS RPUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0)IALLOY,CONVERT(NUMERIC(15,3),0)IWASTAGE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0)RALLOY,CONVERT(NUMERIC(15,3),0)RWASTAGE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)TOUCH"
        strSql += vbCrLf + " ,CONVERT(INT,SUM(IPCS)-SUM(RPCS))BALPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(IGRSWT)-SUM(RGRSWT))BALGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(INETWT)-SUM(RNETWT))BALNETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(IPUREWT)-SUM(RPUREWT))BALPUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)BALAMT"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,CONVERT(SMALLDATETIME,NULL)TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = OPE.METALID)AS METALNAME,CONVERT(VARCHAR(15),NULL)AS BATCHNO,'N' as TFILTER"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL) REMARK1,CONVERT(VARCHAR(200),NULL) REMARK2"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) PURITY,CONVERT(NUMERIC(15,2),0) RATE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)AMOUNT,CONVERT(NUMERIC(15,2),0)STNAMT,CONVERT(NUMERIC(15,2),0)MC,CONVERT(NUMERIC(15,2),0)MISCAMT,CONVERT(NUMERIC(15,2),0)TAX"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),NULL) EMPNAME,CONVERT(VARCHAR(50),NULL) USERNAME"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)STNWT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT "
        If ChkGrossWt.Checked Then
            strSql += vbCrLf + " CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS ISSWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RECWT"
        ElseIf ChkNetWt.Checked Then
            strSql += vbCrLf + " CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS ISSWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RECWT"
        Else
            If PurewtPer <> 0 Then
                strSql += vbCrLf + " CONVERT(NUMERIC(15,3),CASE WHEN TRANTYPE = 'I' THEN (PUREWT/" & PurewtPer & ")*100 ELSE 0 END) AS ISSWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN TRANTYPE = 'R' THEN (PUREWT/" & PurewtPer & ")*100 ELSE 0 END) AS RECWT"
            Else
                strSql += vbCrLf + " CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS ISSWT"
                strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RECWT"
            End If

        End If
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS IGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS INETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS IPUREWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RNETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RPUREWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN AMOUNT ELSE 0 END  DEBIT,CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE 0 END CREDIT,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = O.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT O"
        strSql += vbCrLf + " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then ''chkCmbTranType.Text = "ALL" And
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')<>'A' "
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='A' "
        End If
        If chkCmbTranType.Text = "INTERNAL TRANSFER" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='I' "
        End If
        If Not chkCmbTranType.Text.ToString.Contains("INTERNAL TRANSFER") And chkCmbTranType.Text <> "ALL" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')<>'I' "
        End If
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If chkWithAbs.Checked Or chkWithSpecicFormat.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            If ChkGrossWt.Checked Then
                strSql += vbCrLf + " I.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE NOT IN('AI','MI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & ""
                ''If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(I.WASTAGE,0)-ISNULL(I.ALLOY,0) ELSE 0 END "
                strSql += vbCrLf + " ISSWT,0 RECWT"
            ElseIf ChkNetWt.Checked Then
                strSql += vbCrLf + " I.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE NOT IN('AI','MI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & ""
                ''If ChkwithWast.Checked Then strSql += vbCrLf + " +CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(I.WASTAGE,0)-ISNULL(I.ALLOY,0) ELSE 0 END "
                strSql += vbCrLf + " ISSWT"
                strSql += vbCrLf + " ,0 RECWT"
            Else
                If PurewtPer <> 0 Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT"
                Else
                    strSql += vbCrLf + " I.PUREWT ISSWT,0 RECWT"
                End If
            End If
            strSql += vbCrLf + " ,I.PCS IPCS,I.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE NOT IN('AI','MI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & " IGRSWT"
            strSql += vbCrLf + " ,I.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE NOT IN('AI','MI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & " INETWT,I.PUREWT IPUREWT"
            strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT"
            strSql += vbCrLf + " ,0 CREDIT"
            strSql += vbCrLf + " ,I.METALID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
            strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('IPU','SA') "
            strSql += vbCrLf + " AND I.TRANDATE < '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If trantype <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            If ChkGrossWt.Checked Then
                strSql += vbCrLf + " 0 ISSUE"
                strSql += vbCrLf + " ,R.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0) ELSE 0 END", "0") & ""
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.ALLOY,0) ELSE 0 END " '' ISNULL(R.WASTAGE,0)+
                strSql += vbCrLf + " RECEIPT"
            ElseIf ChkNetWt.Checked Then
                strSql += vbCrLf + " 0 ISSUE"
                strSql += vbCrLf + " ,R.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0) ELSE 0 END", "0") & ""
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.ALLOY,0) ELSE 0 END " ''ISNULL(R.WASTAGE,0)+
                strSql += vbCrLf + " RECEIPT"
            Else
                If PurewtPer <> 0 Then
                    strSql += vbCrLf + " 0 ISSUE,CONVERT(NUMERIC(15,3),(R.PUREWT/" & PurewtPer & ")*100) RECEIPT"

                Else
                    strSql += vbCrLf + " 0 ISSUE,R.PUREWT RECEIPT"
                End If
            End If
            strSql += vbCrLf + " ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT"
            strSql += vbCrLf + " ,R.PCS RPCS,R.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END", "0") & " RGRSWT"
            strSql += vbCrLf + " ,R.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END", "0") & " RNETWT,R.PUREWT RPUREWT"
            strSql += vbCrLf + " ,0 DEBIT"
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT"
            strSql += vbCrLf + " ,R.METALID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=R.EMPID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=R.USERID"
            strSql += vbCrLf + " WHERE R.TRANTYPE NOT IN ('RPU') "
            strSql += vbCrLf + " AND R.TRANDATE < '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(R.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND R.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If trantype <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + " AND R.COMPANYID = '" & strCompanyId & "'"
            If chkWithAbs.Checked Or chkWithSpecicFormat.Checked Then
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT CONVERT(NUMERIC(15,3),S.STNWT) ISSWT "
                strSql += vbCrLf + " ,0 RECWT ,S.STNPCS IPCS,CONVERT(NUMERIC(15,3),S.STNWT) IGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) INETWT,CONVERT(NUMERIC(15,3),S.STNWT) IPUREWT"
                strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT ,CASE WHEN S.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT"
                strSql += vbCrLf + " ,0 CREDIT ,CA.METALID"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID=S.STNSUBITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
                strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CAT ON CAT.CATCODE = I.OCATCODE"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
                strSql += vbCrLf + "  WHERE S.TRANTYPE NOT IN ('IPU','SA') "
                strSql += vbCrLf + " AND I.TRANDATE < '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
                strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
                If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
                If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
                strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT  0 ISSUE ,CONVERT(NUMERIC(15,3),S.STNWT) RECEIPT ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT"
                strSql += vbCrLf + " ,S.STNPCS RPCS,CONVERT(NUMERIC(15,3),S.STNWT) RGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) RNETWT,CONVERT(NUMERIC(15,3),S.STNWT) RPUREWT "
                strSql += vbCrLf + " ,0 DEBIT ,CASE WHEN S.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT "
                strSql += vbCrLf + " ,CA.METALID"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID=S.STNSUBITEMID"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
                strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CAT ON CAT.CATCODE = I.OCATCODE"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
                strSql += vbCrLf + "  WHERE S.TRANTYPE NOT IN ('RPU') "
                strSql += vbCrLf + "  AND I.TRANDATE < '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
                strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
                If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
                If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
                strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
            End If
        End If
        strSql += vbCrLf + " )OPE"
        strSql += vbCrLf + " GROUP BY METALID"
        strSql += vbCrLf + " UNION ALL"
        '/** TRANSACTION **/
        strSql += vbCrLf + " /** TRANSACTION **/"
        strSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS PARTICULAR"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE)AS OCATNAME"
        strSql += vbCrLf + " ,I.TRANNO,I.REFNO,CONVERT(VARCHAR,I.TRANDATE,103)TDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID AND ITEMID=I.ITEMID) SUBITEM"
        If ChkGrossWt.Checked Then
            strSql += vbCrLf + " ,I.GRSWT"
            ''If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " ISSWT,0 RECWT"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        ElseIf ChkNetWt.Checked Then
            strSql += vbCrLf + " ,I.NETWT"
            ''If ChkwithWast.Checked Then strSql += vbCrLf + " +CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " ISSWT"
            'strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
            'strSql += vbCrLf + " ,0 RECWT"
            'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        Else
            If PurewtPer <> 0 Then
                'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT"
                'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            Else
                'strSql += vbCrLf + " ,I.PUREWT ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                strSql += vbCrLf + " ,I.PUREWT ISSWT,0 RECWT"
                strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,I.PCS IPCS,I.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & " IGRSWT"
        strSql += vbCrLf + " ,I.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END ", "0") & " INETWT,I.PUREWT IPUREWT"
        strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN I.ALLOY ELSE 0 END IALLOY,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.WASTAGE ELSE 0 END IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE,I.TOUCH,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,I.TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METALNAME,BATCHNO,(case when i.TRANTYPE IN ('IPU','RPU')  then 'Y' else 'N' end) as TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " ,PURITY,I.RATE"
        'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.AMOUNT ELSE 0 END AMOUNT"
        'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN STNAMT ELSE 0 END STNAMT"
        'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN MCHARGE ELSE 0 END MC"
        'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
        'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(TAX,0) ELSE 0 END TAX"

        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN I.AMOUNT ELSE 0 END AMOUNT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN STNAMT ELSE 0 END STNAMT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN MCHARGE ELSE 0 END MC"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(TAX,0) ELSE 0 END TAX"

        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO= I.SNO AND BATCHNO=I.BATCHNO)STNWT "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
        strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('IPU','SA') "
        If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
            strSql += vbCrLf + " AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"

        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('MI')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + " SELECT PARTICULAR,CATNAME,OCATNAME,TRANNO,REFNO,TDATE,ITEM,SUBITEM "
            strSql += vbCrLf + " ,SUM(ISSWT)ISSWT,SUM(RECWT)RECWT,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT "
            strSql += vbCrLf + " ,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(IPUREWT)IPUREWT "
            strSql += vbCrLf + " ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(RPUREWT)RPUREWT "
            strSql += vbCrLf + " ,SUM(IALLOY)IALLOY,SUM(IWASTAGE)IWASTAGE,SUM(RALLOY)RALLOY,SUM(RWASTAGE)RWASTAGE "
            strSql += vbCrLf + " ,NULL TOUCH,SUM(BALPCS)BALPCS,SUM(BALGRSWT)BALGRSWT,SUM(BALNETWT)BALNETWT,SUM(BALPUREWT)BALPUREWT,SUM(BALAMT)BALAMT "
            strSql += vbCrLf + " ,2 RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,TFILTER,REMARK1,REMARK2,PURITY "
            strSql += vbCrLf + " ,AVG(RATE)RATE,SUM(AMOUNT)AMOUNT,SUM(STNAMT)STNAMT,SUM(MC)MC,SUM(MISCAMT)MISCAMT,SUM(TAX)TAX "
            strSql += vbCrLf + " ,EMPNAME,USERNAME,SUM(STNWT)STNWT "
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS PARTICULAR"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE)AS OCATNAME"
            strSql += vbCrLf + " ,I.TRANNO,I.REFNO,CONVERT(VARCHAR,I.TRANDATE,103)TDATE"
            strSql += vbCrLf + " ,NULL ITEM"
            strSql += vbCrLf + " ,NULL SUBITEM"
            If ChkGrossWt.Checked Then
                strSql += vbCrLf + " ,I.GRSWT"
                ''If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(I.WASTAGE,0)-ISNULL(I.ALLOY,0) ELSE 0 END "
                strSql += vbCrLf + " ISSWT,0 RECWT"
                'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            ElseIf ChkNetWt.Checked Then
                strSql += vbCrLf + " ,I.NETWT"
                ''If ChkwithWast.Checked Then strSql += vbCrLf + " +CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(I.WASTAGE,0)-ISNULL(I.ALLOY,0) ELSE 0 END "
                strSql += vbCrLf + " ISSWT"
                'strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                'strSql += vbCrLf + " ,0 RECWT"
                'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            Else
                If PurewtPer <> 0 Then
                    'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                    'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT"
                    'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
                Else
                    'strSql += vbCrLf + " ,I.PUREWT ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
                    strSql += vbCrLf + " ,I.PUREWT ISSWT,0 RECWT"
                    strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
                End If
            End If
            strSql += vbCrLf + " ,I.PCS IPCS,I.GRSWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END", "0") & " IGRSWT"
            strSql += vbCrLf + " ,I.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN ISNULL(I.WASTAGE,0)+ISNULL(I.ALLOY,0) ELSE 0 END ", "0") & " INETWT,I.PUREWT IPUREWT"
            strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE Not IN('MI','AI') THEN I.ALLOY ELSE 0 END IALLOY,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.WASTAGE ELSE 0 END IWASTAGE"
            strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE,I.TOUCH,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
            strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,I.TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METALNAME,BATCHNO,(case when i.TRANTYPE IN ('IPU','RPU')  then 'Y' else 'N' end) as TFILTER"
            strSql += vbCrLf + " ,REMARK1,REMARK2"
            strSql += vbCrLf + " ,PURITY,I.RATE"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.AMOUNT ELSE 0 END AMOUNT"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN STNAMT ELSE 0 END STNAMT"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN MCHARGE ELSE 0 END MC"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
            'strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(TAX,0) ELSE 0 END TAX"

            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN I.AMOUNT ELSE 0 END AMOUNT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN STNAMT ELSE 0 END STNAMT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN MCHARGE ELSE 0 END MC"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE IN('AI','IAP') THEN ISNULL(TAX,0) ELSE 0 END TAX"

            strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO= I.SNO AND BATCHNO=I.BATCHNO)STNWT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
            strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('IPU','SA') "
            If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
                strSql += vbCrLf + " AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If trantype <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND I.TRANTYPE IN ('MI')"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ) Y GROUP BY PARTICULAR,CATNAME,OCATNAME,TRANNO,REFNO,TDATE,ITEM,SUBITEM "
            strSql += vbCrLf + " ,COLHEAD,TRANDATE,METALNAME,BATCHNO,TFILTER,REMARK1,REMARK2,PURITY,EMPNAME,USERNAME"
        End If

        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS PARTICULAR"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.OCATCODE)AS OCATNAME"
        strSql += vbCrLf + " ,R.TRANNO,R.REFNO,CONVERT(VARCHAR,R.TRANDATE,103)TDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID) ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID AND ITEMID=R.ITEMID) SUBITEM"
        If ChkGrossWt.Checked Then
            strSql += vbCrLf + " ,0 ISSUE"
            strSql += vbCrLf + " ,R.GRSWT"
            ''If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.ALLOY,0) ELSE 0 END " ''ISNULL(R.WASTAGE,0)+
            strSql += vbCrLf + " RECEIPT"
            'strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR') THEN AMOUNT ELSE 0 END CREDIT"
            strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT"
            'strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE IN('RAP') THEN AMOUNT ELSE 0 END CREDIT"
            'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        ElseIf ChkNetWt.Checked Then
            strSql += vbCrLf + " ,0 ISSUE"
            strSql += vbCrLf + " ,R.NETWT"
            ''If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.ALLOY,0) ELSE 0 END " ''ISNULL(R.WASTAGE,0)+
            strSql += vbCrLf + " RECEIPT"
            'strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR') THEN AMOUNT ELSE 0 END CREDIT"
            strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT"
            'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        Else
            If PurewtPer <> 0 Then
                strSql += vbCrLf + " ,0 ISSUE,CONVERT(NUMERIC(15,3),(R.PUREWT/" & PurewtPer & ")*100) RECEIPT"
                'strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR') THEN AMOUNT ELSE 0 END CREDIT"
                strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT"
                'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            Else
                strSql += vbCrLf + " ,0 ISSUE,R.PUREWT RECEIPT"
                'strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR') THEN AMOUNT ELSE 0 END CREDIT"
                strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN AMOUNT ELSE 0 END CREDIT"
                'strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
            End If
        End If
        strSql += vbCrLf + " ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT"
        strSql += vbCrLf + " ,R.PCS RPCS,R.GRSWT + " & IIf(ChkwithWast.Checked, "CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END", "0") & " RGRSWT"
        strSql += vbCrLf + " ,R.NETWT + " & IIf(ChkwithWast.Checked, " CASE WHEN R.TRANTYPE NOT IN('AR','MI') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END", "0") & " RNETWT,R.PUREWT RPUREWT"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE Not IN('AR') THEN R.ALLOY ELSE 0 END RALLOY"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN R.WASTAGE ELSE 0 END RWASTAGE,R.TOUCH,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,R.TRANDATE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  "
        strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METALNAME"
        strSql += vbCrLf + " ,BATCHNO,(CASE WHEN R.TRANTYPE IN ('IPU','RPU')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " ,PURITY,R.RATE"
        'strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN R.AMOUNT ELSE 0 END AMOUNT"
        'strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN STNAMT ELSE 0 END STNAMT"
        'strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN MCHARGE ELSE 0 END MC"
        'strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
        'strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(TAX,0) ELSE 0 END TAX"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('AR','RAP') THEN R.AMOUNT ELSE 0 END AMOUNT"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN STNAMT ELSE 0 END STNAMT"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN MCHARGE ELSE 0 END MC"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(MISCAMT,0) ELSE 0 END MISCAMT"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('RAP','AR') THEN ISNULL(TAX,0) ELSE 0 END TAX"

        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO= R.SNO AND BATCHNO=R.BATCHNO)STNWT "
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=R.EMPID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=R.USERID"
        strSql += vbCrLf + " WHERE R.TRANTYPE NOT IN ('RPU') "
        If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
            strSql += vbCrLf + " AND R.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " AND R.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(R.CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND R.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE IN (" & trantype & ")"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + " AND R.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,CA.CATNAME AS CATNAME,CAT.CATNAME AS OCATNAME,S.TRANNO,I.REFNO,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,IM.ITEMNAME [ITEM],SM.SUBITEMNAME AS [SUBITEM]"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) ISSUE,0 RECEIPT,0 DEBIT,0 CREDIT"
        strSql += vbCrLf + " ,S.STNPCS PCS,CONVERT(NUMERIC(15,3),S.STNWT) GRSWT,CONVERT(NUMERIC(15,3),S.STNWT) NETWT,CONVERT(NUMERIC(15,3),S.STNWT) PUREWT"
        strSql += vbCrLf + " ,0 PCS,0 GRSWT,0 NETWT,0 PUREWT"
        strSql += vbCrLf + " ,0 IALLOY,0 IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE ,0 TOUCH ,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,S.TRANDATE,ME.METALNAME AS METALNAME,S.BATCHNO,(CASE WHEN S.TRANTYPE IN ('IPU','RPU')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " ,I.PURITY,I.RATE"
        strSql += vbCrLf + " ,0 AMOUNT,0 STNAMT,0 MC,0 MISCAMT,0 TAX"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
        strSql += vbCrLf + " ,S.STNWT STNWT "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID=S.STNSUBITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CAT ON CAT.CATCODE = I.OCATCODE"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
        strSql += vbCrLf + "  WHERE S.TRANTYPE NOT IN ('IPU','SA') "
        If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
            strSql += vbCrLf + " AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"

        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('MI')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + " SELECT PARTICULAR,CATNAME,OCATNAME,TRANNO,REFNO,TDATE,ITEM,SUBITEM "
            strSql += vbCrLf + " ,SUM(ISSWT)ISSWT,SUM(RECWT)RECWT,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT "
            strSql += vbCrLf + " ,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(IPUREWT)IPUREWT "
            strSql += vbCrLf + " ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(RPUREWT)RPUREWT "
            strSql += vbCrLf + " ,SUM(IALLOY)IALLOY,SUM(IWASTAGE)IWASTAGE,SUM(RALLOY)RALLOY,SUM(RWASTAGE)RWASTAGE "
            strSql += vbCrLf + " ,NULL TOUCH,SUM(BALPCS)BALPCS,SUM(BALGRSWT)BALGRSWT,SUM(BALNETWT)BALNETWT,SUM(BALPUREWT)BALPUREWT,SUM(BALAMT)BALAMT "
            strSql += vbCrLf + " ,2 RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,TFILTER,REMARK1,REMARK2,PURITY "
            strSql += vbCrLf + " ,AVG(RATE)RATE,SUM(AMOUNT)AMOUNT,SUM(STNAMT)STNAMT,SUM(MC)MC,SUM(MISCAMT)MISCAMT,SUM(TAX)TAX "
            strSql += vbCrLf + " ,EMPNAME,USERNAME,SUM(STNWT)STNWT "
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,CA.CATNAME AS CATNAME,CAT.CATNAME AS OCATNAME,S.TRANNO,I.REFNO,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,NULL [ITEM],NULL AS [SUBITEM]"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) ISSWT,0 RECWT,0 DEBIT,0 CREDIT"
            strSql += vbCrLf + " ,S.STNPCS IPCS,CONVERT(NUMERIC(15,3),S.STNWT) IGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) INETWT,CONVERT(NUMERIC(15,3),S.STNWT) IPUREWT"
            strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT"
            strSql += vbCrLf + " ,0 IALLOY,0 IWASTAGE"
            strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE ,0 TOUCH ,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
            strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,S.TRANDATE,ME.METALNAME AS METALNAME,S.BATCHNO,(CASE WHEN S.TRANTYPE IN ('IPU','RPU')  THEN 'Y' ELSE 'N' END) AS TFILTER"
            strSql += vbCrLf + " ,REMARK1,REMARK2"
            strSql += vbCrLf + " ,I.PURITY,I.RATE"
            strSql += vbCrLf + " ,0 AMOUNT,0 STNAMT,0 MC,0 MISCAMT,0 TAX"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
            strSql += vbCrLf + " ,S.STNWT STNWT "
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID=S.STNSUBITEMID"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
            strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CAT ON CAT.CATCODE = I.OCATCODE"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
            strSql += vbCrLf + "  WHERE S.TRANTYPE NOT IN ('IPU','SA') "
            If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
                strSql += vbCrLf + " AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
            End If
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
            strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  AND I.TRANTYPE IN ('MI')"
            strSql += vbCrLf + " ) Y GROUP BY PARTICULAR,CATNAME,OCATNAME,TRANNO,REFNO,TDATE,ITEM,SUBITEM "
            strSql += vbCrLf + " ,COLHEAD,TRANDATE,METALNAME,BATCHNO,TFILTER,REMARK1,REMARK2,PURITY,EMPNAME,USERNAME"
        End If

        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME AS PARTICULAR,CA.CATNAME AS CATNAME,CAT.CATNAME AS OCATNAME,S.TRANNO,I.REFNO,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,IM.ITEMNAME [ITEM],SM.SUBITEMNAME AS [SUBITEM]"
        strSql += vbCrLf + " ,0 ISSUE,CONVERT(NUMERIC(15,3),S.STNWT) RECEIPT,0 DEBIT,0 CREDIT"
        strSql += vbCrLf + " ,0 PCS,0 GRSWT,0 NETWT,0 PUREWT"
        strSql += vbCrLf + " ,S.STNPCS PCS,CONVERT(NUMERIC(15,3),S.STNWT) GRSWT,CONVERT(NUMERIC(15,3),S.STNWT) NETWT,CONVERT(NUMERIC(15,3),S.STNWT) PUREWT"
        strSql += vbCrLf + " ,0 IALLOY,0 IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE ,0 TOUCH ,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,S.TRANDATE,ME.METALNAME AS METALNAME,S.BATCHNO,(case when s.TRANTYPE IN ('IPU','RPU')  then 'Y' else 'N' end) as TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " ,I.PURITY,I.RATE"
        strSql += vbCrLf + " ,0 AMOUNT,0 STNAMT,0 MC,0 MISCAMT,0 TAX"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),E.EMPNAME) EMPNAME,CONVERT(VARCHAR(50),U.USERNAME) USERNAME"
        strSql += vbCrLf + " ,S.STNWT STNWT "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID=S.STNSUBITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CAT ON CAT.CATCODE = I.OCATCODE"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
        strSql += vbCrLf + "  WHERE S.TRANTYPE NOT IN ('RPU') "
        If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
            strSql += vbCrLf + "  AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND S.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  ORDER BY TRANDATE,TRANNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET ADD SNO INT IDENTITY(1,1)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        'strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT')>0 DROP TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT"
        'strSql += vbCrLf + " SELECT BATCHNO,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END DEBIT"
        'strSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE 0 END CREDIT"
        'strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT"
        'strSql += vbCrLf + " FROM"
        'strSql += vbCrLf + " ("
        'strSql += vbCrLf + " SELECT BATCHNO,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN"
        'strSql += vbCrLf + " WHERE TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        'If ChkRelatedtran.Checked Then strSql += vbCrLf + " AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET)"
        'strSql += vbCrLf + " GROUP BY BATCHNO"
        'strSql += vbCrLf + " )X"
        'strSql += vbCrLf + " "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT '','OPENING AMOUNT..',DEBIT,CREDIT,1,'',''"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN -1*SUM(AMOUNT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "     WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        'If chkWithAbs.Checked Then
        '    strSql += vbCrLf + " 	UNION ALL"
        '    strSql += vbCrLf + " 	SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT FROM " & cnStockDb & "..ACCTRAN"
        '    strSql += vbCrLf + "     WHERE TRANDATE < '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        '    strSql += vbCrLf + "     AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        '    If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        '    strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        'End If
        strSql += vbCrLf + " 	)AMT"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " )X WHERE NOT(DEBIT = 0 AND CREDIT = 0)"
        strSql += vbCrLf + " "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(PARTICULAR,TRANNO,TDATE,ISSWT,RECWT,RPCS,DEBIT,CREDIT,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT DISTINCT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
        strSql += vbCrLf + " ,T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)TDATE"
        strSql += vbCrLf + " ,0 AS DEBITWT,0 AS CREDITWT,0 AS PCS"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN T.AMOUNT ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN T.AMOUNT ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,1 RESULT,T.TRANDATE"
        strSql += vbCrLf + " ,SM.METALNAME,T.BATCHNO,''COLHEAD,'Y' as TFILTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SM ON T.BATCHNO=SM.BATCHNO"
        If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
            strSql += vbCrLf + "  WHERE T.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  WHERE T.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND T.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
        'strSql += vbCrLf + " AND BATCHNO  IN (SELECT ISNULL(BATCHNO,'') FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET "
        'If chkMetalName <> "" Then strSql += vbCrLf + "   WHERE METALNAME IN (" & chkMetalName & " ))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkRelatedtran.Checked = False Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(PARTICULAR,TRANNO,TDATE,ISSWT,RECWT,RPCS,DEBIT,CREDIT,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER)"
            strSql += vbCrLf + " SELECT DISTINCT "
            strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
            strSql += vbCrLf + " ,T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)TDATE"
            strSql += vbCrLf + " ,0 AS DEBITWT,0 AS CREDITWT,0 AS PCS"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN T.AMOUNT ELSE 0 END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN T.AMOUNT ELSE 0 END AS CREDIT"
            strSql += vbCrLf + " ,1 RESULT,T.TRANDATE"
            strSql += vbCrLf + " ,'' METALNAME,T.BATCHNO,''COLHEAD,'N' as TFILTER"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
            If chkWithAbs.Checked = False And chkWithSpecicFormat.Checked = False Then
                strSql += vbCrLf + " WHERE T.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND  '" & Obj_MeFilterValues.p_dtpToDate.Date.ToString("yyyy-MM-dd") & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND T.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT ISNULL(BATCHNO,'') FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET "
            If chkMetalName <> "" Then strSql += vbCrLf + "   WHERE METALNAME IN (" & chkMetalName & " )"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,0 RESULT,'T'COLHEAD,'N' FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,ISSWT,RECWT,DEBIT,CREDIT,RESULT,COLHEAD"
        strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IPUREWT,RPCS,RGRSWT,RNETWT,RPUREWT,IALLOY,IWASTAGE,RALLOY,RWASTAGE,TFILTER,BALPCS,BALGRSWT,BALNETWT"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT METALNAME,'SUB TOTAL',SUM(ISNULL((case when tfilter='Y' then 0 else ISSWT end),0))"
        strSql += vbCrLf + " ,SUM(ISNULL((case when tfilter='Y' then 0 else RECWT end),0))"
        'strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else ISNULL(DEBIT,0) end)"
        'strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else ISNULL(CREDIT,0) end)"
        strSql += vbCrLf + " ,SUM(case when tfilter<>'Y' then 0 else ISNULL(DEBIT,0) end)"
        strSql += vbCrLf + " ,SUM(case when tfilter<>'Y' then 0 else ISNULL(CREDIT,0) end)"
        strSql += vbCrLf + " ,3 RESULT,'S'COLHEAD"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IPCS end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IGRSWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else INETWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IPUREWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RPCS end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RGRSWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RNETWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RPUREWT end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IALLOY end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IWASTAGE end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RALLOY end)"
        strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RWASTAGE end)"
        strSql += vbCrLf + " ,'N'"
        strSql += vbCrLf + " ,Sum(case when tfilter = 'Y' then 0 else BALPCS end)"
        strSql += vbCrLf + " ,Sum(case when tfilter = 'Y' then  0 else BALGRSWT end)"
        strSql += vbCrLf + " ,Sum(case when tfilter = 'Y' then  0 else BALNETWT end)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET GROUP BY METALNAME"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,ISSWT,RECWT,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT 'zzzz' METALNAME,' '"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISSWT)-SUM(RECWT) > 0 THEN SUM(ISSWT)-SUM(RECWT) ELSE 0 END ISSWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(RECWT)-SUM(ISSWT) > 0 THEN SUM(RECWT)-SUM(ISSWT) ELSE 0 END RECWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT)-SUM(CREDIT) > 0 THEN SUM(DEBIT)-SUM(CREDIT) ELSE 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)-SUM(DEBIT) > 0 THEN SUM(CREDIT)-SUM(DEBIT) ELSE 0 END CREDIT"
        strSql += vbCrLf + " ,5 RESULT,'G'COLHEAD,'N'"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET WHERE RESULT = 3"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtsmithbal As New DataTable
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET ORDER BY METALNAME,RESULT,TRANDATE,SNO,TRANNO "
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtsmithbal)
        Dim BALPCS As Integer = 0
        Dim BALGRSWT As Decimal = 0
        Dim BALNETWT As Decimal = 0
        Dim BALPUREWT As Decimal = 0
        Dim BALAMT As Decimal = 0

        For I As Integer = 0 To dtsmithbal.Rows.Count - 1
            If dtsmithbal.Rows(I).Item("RESULT").ToString = 0 Then
                BALPCS = 0
                BALGRSWT = 0
                BALNETWT = 0
                BALPUREWT = 0
                If ChkRelatedtran.Checked And chkAmtbal.Checked Then BALAMT = 0
            End If
            If dtsmithbal.Rows(I).Item("COLHEAD").ToString <> "T" Then
                If dtsmithbal.Rows(I).Item("COLHEAD").ToString = "S" Or dtsmithbal.Rows(I).Item("COLHEAD").ToString = "G" Then
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS= (" & BALPCS & "),"
                    strSql += vbCrLf + " BALGRSWT= (" & BALGRSWT & "),"
                    strSql += vbCrLf + " BALNETWT = (" & BALNETWT & "),"
                    strSql += vbCrLf + " BALPUREWT = (" & BALPUREWT & ")"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & ""
                    strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT=(" & BALAMT & ")"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & " AND  TFILTER <> 'Y' "
                    'If ChkRelatedtran.Checked And chkAmtbal.Checked Then strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    dtsmithbal.Rows(I).Item("BALPCS") = BALPCS
                    dtsmithbal.Rows(I).Item("BALGRSWT") = BALGRSWT
                    dtsmithbal.Rows(I).Item("BALNETWT") = BALNETWT
                    dtsmithbal.Rows(I).Item("BALPUREWT") = BALPUREWT
                    dtsmithbal.Rows(I).Item("BALAMT") = BALAMT
                Else
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS= (" & BALPCS & "+ ISNULL(IPCS,0) - ISNULL(RPCS,0)),"
                    strSql += vbCrLf + " BALGRSWT= (" & BALGRSWT & "+ ISNULL(IGRSWT,0) - ISNULL(RGRSWT,0) ),"
                    strSql += vbCrLf + " BALNETWT = (" & BALNETWT & "+ ISNULL(INETWT, 0) - ISNULL(RNETWT, 0)),"
                    strSql += vbCrLf + " BALPUREWT = (" & BALPUREWT & "+ ISNULL(IPUREWT, 0) - ISNULL(RPUREWT, 0))"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & ""
                    strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT = (" & BALAMT & "+ ISNULL(DEBIT, 0)  - ISNULL(CREDIT, 0))"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & " AND  TFILTER <> 'Y' "
                    'If ChkRelatedtran.Checked And chkAmtbal.Checked Then strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    dtsmithbal.Rows(I).Item("BALPCS") = BALPCS + Val(dtsmithbal.Rows(I).Item("IPCS").ToString) - Val(dtsmithbal.Rows(I).Item("RPCS").ToString)
                    dtsmithbal.Rows(I).Item("BALGRSWT") = BALGRSWT + Val(dtsmithbal.Rows(I).Item("IGRSWT").ToString) - Val(dtsmithbal.Rows(I).Item("RGRSWT").ToString)
                    dtsmithbal.Rows(I).Item("BALNETWT") = BALNETWT + Val(dtsmithbal.Rows(I).Item("INETWT").ToString) - Val(dtsmithbal.Rows(I).Item("RNETWT").ToString)
                    dtsmithbal.Rows(I).Item("BALPUREWT") = BALPUREWT + Val(dtsmithbal.Rows(I).Item("IPUREWT").ToString) - Val(dtsmithbal.Rows(I).Item("RPUREWT").ToString)
                    dtsmithbal.Rows(I).Item("BALAMT") = BALAMT + Val(dtsmithbal.Rows(I).Item("DEBIT").ToString) - Val(dtsmithbal.Rows(I).Item("CREDIT").ToString)
                End If

            End If
            If dtsmithbal.Rows(I).Item("COLHEAD").ToString.Trim = "" Then
                BALPCS = Val(dtsmithbal.Rows(I).Item("BALPCS").ToString)
                BALGRSWT = Val(dtsmithbal.Rows(I).Item("BALGRSWT").ToString)
                BALNETWT = Val(dtsmithbal.Rows(I).Item("BALNETWT").ToString)
                BALPUREWT = Val(dtsmithbal.Rows(I).Item("BALPUREWT").ToString)
                BALAMT = IIf(dtsmithbal.Rows(I).Item("TFILTER").ToString <> "Y", Val(dtsmithbal.Rows(I).Item("BALAMT").ToString), BALAMT)
            End If
        Next

        strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET ISSWT = NULL WHERE ISSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RECWT = NULL WHERE RECWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET INETWT = NULL WHERE INETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IPUREWT = NULL WHERE IPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RNETWT = NULL WHERE RNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RPUREWT = NULL WHERE RPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IALLOY = NULL WHERE IALLOY = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IWASTAGE = NULL WHERE IWASTAGE = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RALLOY = NULL WHERE RALLOY = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RWASTAGE = NULL WHERE RWASTAGE = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET TOUCH = NULL WHERE TOUCH = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS = NULL WHERE BALPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALGRSWT = NULL WHERE BALGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALNETWT = NULL WHERE BALNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT= NULL WHERE BALAMT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET DEBIT = NULL WHERE DEBIT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET CREDIT = NULL WHERE CREDIT = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET1)>0"

        strSql = vbCrLf + " SELECT PARTICULAR,ITEM,SUBITEM,TDATE,TRANNO,PURITY,RATE"
        strSql += vbCrLf + " ,IPCS,ISSWT,IALLOY,IWASTAGE"
        strSql += vbCrLf + " ,IGRSWT,INETWT,IPUREWT"
        'strSql += vbCrLf + " ,RPCS,RECWT,RALLOY,RWASTAGE"
        'strSql += vbCrLf + " ,RGRSWT,RNETWT,RPUREWT"
        If ChkwithWast.Checked Then
            If ChkGrossWt.Checked Then
                strSql += vbCrLf + " ,RPCS,RECWT,RALLOY,RWASTAGE"
                strSql += vbCrLf + " ,RGRSWT,RNETWT,RPUREWT"
            ElseIf ChkNetWt.Checked Then
                strSql += vbCrLf + " ,RPCS, RECWT,RALLOY,RWASTAGE"
                strSql += vbCrLf + " ,RGRSWT,RNETWT,RPUREWT"
            Else
                strSql += vbCrLf + " ,RPCS,RECWT,RALLOY,RWASTAGE"
                strSql += vbCrLf + " ,RGRSWT,RNETWT,RPUREWT"
            End If
        Else
            strSql += vbCrLf + " ,RPCS,RECWT,RALLOY,RWASTAGE"
            strSql += vbCrLf + " ,RGRSWT,RNETWT,RPUREWT"
        End If
        strSql += vbCrLf + " ,DEBIT,CREDIT"
        strSql += vbCrLf + " ,BALPCS,BALGRSWT,BALNETWT,BALPUREWT,BALAMT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IPCS,0) <> 0 THEN IPCS ELSE RPCS END AS PCS"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IGRSWT,0) <> 0 THEN IGRSWT ELSE RGRSWT END AS GRSWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(INETWT,0) <> 0 THEN INETWT ELSE RNETWT END AS NETWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IPUREWT,0) <> 0 THEN IPUREWT ELSE RPUREWT END AS PUREWT"
        strSql += vbCrLf + " ,TOUCH"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,SNO,TFILTER"
        strSql += vbCrLf + " ,CATNAME,OCATNAME,REFNO"
        strSql += vbCrLf + " ,AMOUNT,STNAMT,MC,MISCAMT,TAX,EMPNAME,USERNAME,STNWT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        strSql += vbCrLf + " ORDER BY METALNAME,RESULT,TRANDATE,TRANNO"
        If chkWithItemDet.Checked Then strSql += vbCrLf + " ,ITEM,SUBITEM"


        Dim dtGrid As New DataTable("DETAILED")
        Dim dtCol As New DataColumn("KEYNO")
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function
    Private Sub GridViewHeaderCreator_F1(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        If chkWithItemDet.Checked Then
            strSql = "SELECT ''[PARTICULAR~ITEM~SUBITEM~TDATE~TRANNO]"
            strSql += " ,''[PURITY]"
            strSql += " ,''[RATE]"
            strSql += " ,''[IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE~ISSWT~RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE~RECWT]"
            strSql += " ,''[DEBIT~CREDIT]"
            strSql += " ,''[BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT]"
            strSql += " ,''[GRSWT~NETWT~PUREWT~TOUCH~REMARK1~REMARK2]"
            strSql += " ,''[CATNAME~OCATNAME~USERNAME]"
            strSql += " ,''SCROLL"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            gridviewHead.DataSource = dtHead
            gridviewHead.Columns("PARTICULAR~ITEM~SUBITEM~TDATE~TRANNO").HeaderText = ""
            gridviewHead.Columns("PURITY").HeaderText = "PURITY"
            gridviewHead.Columns("RATE").HeaderText = "RATE"
            gridviewHead.Columns("IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE~ISSWT~RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE~RECWT").HeaderText = "WEIGHT"
            gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "AMOUNT"
            gridviewHead.Columns("BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT").HeaderText = "BALANCE"
            gridviewHead.Columns("GRSWT~NETWT~PUREWT~TOUCH~REMARK1~REMARK2").HeaderText = ""
            gridviewHead.Columns("CATNAME~OCATNAME~USERNAME").HeaderText = ""
            gridviewHead.Columns("SCROLL").HeaderText = ""
            gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            SetGridHeadColWidth_F1(gridviewHead)
        Else
            strSql = "SELECT ''[PARTICULAR~TDATE~TRANNO]"
            strSql += " ,''[PURITY]"
            strSql += " ,''[RATE]"
            strSql += " ,''[IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE~ISSWT~RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE~RECWT]"
            strSql += " ,''[DEBIT~CREDIT]"
            strSql += " ,''[BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT]"
            strSql += " ,''[GRSWT~NETWT~PUREWT~TOUCH~ITEM~SUBITEM~REMARK1~REMARK2]"
            strSql += " ,''[CATNAME~OCATNAME~USERNAME]"
            strSql += " ,''SCROLL"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            gridviewHead.DataSource = dtHead
            gridviewHead.Columns("PARTICULAR~TDATE~TRANNO").HeaderText = ""
            gridviewHead.Columns("PURITY").HeaderText = "PURITY"
            gridviewHead.Columns("RATE").HeaderText = "RATE"
            gridviewHead.Columns("IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE~ISSWT~RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE~RECWT").HeaderText = "WEIGHT"
            gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "AMOUNT"
            gridviewHead.Columns("BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT").HeaderText = "BALANCE"
            gridviewHead.Columns("GRSWT~NETWT~PUREWT~TOUCH~ITEM~SUBITEM~REMARK1~REMARK2").HeaderText = ""
            gridviewHead.Columns("CATNAME~OCATNAME~USERNAME").HeaderText = ""
            gridviewHead.Columns("SCROLL").HeaderText = ""
            gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            SetGridHeadColWidth_F1(gridviewHead)
        End If
    End Sub
    Private Sub DataGridView_DetailViewFormatting_f1(ByVal dgv As DataGridView)
        With dgv
            If .Columns.Contains("TFILTER") Then .Columns("TFILTER").Visible = False
            If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").Visible = False
            If .Columns.Contains("METALNAME") Then .Columns("METALNAME").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
            If .Columns.Contains("PCS") Then .Columns("PCS").Visible = False
            If .Columns.Contains("IPCS") Then .Columns("IPCS").Visible = False
            If .Columns.Contains("RPCS") Then .Columns("RPCS").Visible = False
            If .Columns.Contains("GRSWT") Then .Columns("GRSWT").Visible = False 'Not rbtGrossWeight.Checked
            If .Columns.Contains("NETWT") Then .Columns("NETWT").Visible = False 'Not rbtNetWeight.Checked
            If .Columns.Contains("PUREWT") Then .Columns("PUREWT").Visible = False 'Not rbtPureWeight.Checked
            If .Columns.Contains("IGRSWT") Then .Columns("IGRSWT").Visible = ChkGrossWt.Checked
            If .Columns.Contains("INETWT") Then .Columns("INETWT").Visible = ChkNetWt.Checked
            If .Columns.Contains("IPUREWT") Then .Columns("IPUREWT").Visible = ChkPureWt.Checked
            If .Columns.Contains("RGRSWT") Then .Columns("RGRSWT").Visible = ChkGrossWt.Checked
            If .Columns.Contains("RNETWT") Then .Columns("RNETWT").Visible = ChkNetWt.Checked
            If .Columns.Contains("RPUREWT") Then .Columns("RPUREWT").Visible = ChkPureWt.Checked
            'If .Columns.Contains("PARTICULAR") Then .Columns("PARTICULAR").Visible = False
            If .Columns.Contains("BALPCS") Then .Columns("BALPCS").Visible = False
            If .Columns.Contains("STNWT") Then .Columns("STNWT").Visible = False

            If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False
            If .Columns.Contains("OCATNAME") Then .Columns("OCATNAME").Visible = False
            If .Columns.Contains("REFNO") Then .Columns("REFNO").Visible = False
            If .Columns.Contains("EMPNAME") Then .Columns("EMPNAME").Visible = False
            If .Columns.Contains("USERNAME") Then .Columns("USERNAME").Visible = True

            If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").Visible = False
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").Visible = False
            If .Columns.Contains("MC") Then .Columns("MC").Visible = False
            If .Columns.Contains("MISCAMT") Then .Columns("MISCAMT").Visible = False
            If .Columns.Contains("TAX") Then .Columns("TAX").Visible = False

            If .Columns.Contains("ISSWT") Then .Columns("ISSWT").Visible = IIf(ChkwithWast.Checked, True, False) 'false
            If .Columns.Contains("RECWT") Then .Columns("RECWT").Visible = IIf(ChkwithWast.Checked, True, False) 'false

            .Columns("BALGRSWT").Visible = ChkGrossWt.Checked
            .Columns("BALNETWT").Visible = ChkNetWt.Checked
            .Columns("BALPUREWT").Visible = ChkPureWt.Checked

            .Columns("IALLOY").Visible = IIf(ChkwithWast.Checked, True, False) 'false 'Not rbtPureWeight.Checked
            .Columns("IWASTAGE").Visible = IIf(ChkwithWast.Checked, True, False) 'false 'Not rbtPureWeight.Checked
            .Columns("RALLOY").Visible = IIf(ChkwithWast.Checked, True, False) 'false 'Not rbtPureWeight.Checked
            .Columns("RWASTAGE").Visible = IIf(ChkwithWast.Checked, True, False) 'false 'Not rbtPureWeight.Checked

            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "ISSUE " '"GRSWT"
            .Columns("INETWT").HeaderText = "ISSUE " '"NETWT"
            .Columns("IPUREWT").HeaderText = "ISSUE " '"PUREWT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "RECEIPT" '"GRSWT"
            .Columns("RNETWT").HeaderText = "RECEIPT" '"NETWT"
            .Columns("RPUREWT").HeaderText = "RECEIPT" '"PUREWT"

            .Columns("IALLOY").HeaderText = "ALLOY"
            .Columns("RALLOY").HeaderText = "ALLOY"
            .Columns("IWASTAGE").HeaderText = "WASTAGE"
            .Columns("RWASTAGE").HeaderText = "WASTAGE"
            .Columns("BALPCS").HeaderText = "PCS"
            .Columns("BALGRSWT").HeaderText = "GRSWT"
            .Columns("BALNETWT").HeaderText = "NETWT"
            .Columns("BALPUREWT").HeaderText = "PUREWT"
            .Columns("BALAMT").HeaderText = "AMOUNT"


            .Columns("PARTICULAR").Width = 200
            .Columns("TRANNO").Width = 60
            .Columns("TDATE").Width = 80
            .Columns("ITEM").Width = 100
            .Columns("SUBITEM").Width = 100
            .Columns("ITEM").Visible = chkWithItemDet.Checked
            .Columns("SUBITEM").Visible = chkWithItemDet.Checked
            .Columns("ISSWT").Width = 90
            .Columns("RECWT").Width = 90
            .Columns("DEBIT").Width = 90
            .Columns("CREDIT").Width = 90
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("PUREWT").Width = 90
            .Columns("IPCS").Width = 40
            .Columns("IGRSWT").Width = 80
            .Columns("INETWT").Width = 80
            .Columns("RPCS").Width = 40
            .Columns("RGRSWT").Width = 80
            .Columns("RNETWT").Width = 80
            .Columns("BALPCS").Width = 40
            .Columns("BALGRSWT").Width = 80
            .Columns("BALNETWT").Width = 80
            .Columns("IALLOY").Width = 80
            .Columns("IWASTAGE").Width = 80
            .Columns("RALLOY").Width = 80
            .Columns("RWASTAGE").Width = 80
            .Columns("TOUCH").Width = 70

            .Columns("TDATE").HeaderText = "TRANDATE"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub GridViewHeader(ByVal gridviewHead As DataGridView, ByVal gridview As DataGridView)
        If gridview.RowCount > 0 Then
            gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridview.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridview.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR~TRANNO~TRANDATE~ITEM]"
        strSql += " ,''[IGRWT~INETWT~IPUREWT~ITOUCH]"
        strSql += " ,''[RGRWT~RNETWT~RPUREWT~RTOUCH]"
        strSql += " ,''[RRGRWT~RRNETWT~RRPUREWT]"
        strSql += " ,''[NARRATION]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~TRANNO~TRANDATE~ITEM").HeaderText = ""
        gridviewHead.Columns("RGRWT~RNETWT~RPUREWT~RTOUCH").HeaderText = "RECEIPT"
        gridviewHead.Columns("IGRWT~INETWT~IPUREWT~ITOUCH").HeaderText = "ISSUE"
        gridviewHead.Columns("RRGRWT~RRNETWT~RRPUREWT").HeaderText = "RUNNING BALANCE"
        gridviewHead.Columns("NARRATION").HeaderText = ""
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridview
            gridviewHead.Columns("PARTICULAR~TRANNO~TRANDATE~ITEM").Width = .Columns("PARTICULAR").Width + .Columns("TRANNO").Width + .Columns("TRANDATE").Width + .Columns("ITEM").Width
            gridviewHead.Columns("IGRWT~INETWT~IPUREWT~ITOUCH").Width = .Columns("IGRWT").Width + .Columns("INETWT").Width + .Columns("IPUREWT").Width + .Columns("ITOUCH").Width
            gridviewHead.Columns("RGRWT~RNETWT~RPUREWT~RTOUCH").Width = .Columns("RGRWT").Width + .Columns("RNETWT").Width + .Columns("RPUREWT").Width + .Columns("RTOUCH").Width
            gridviewHead.Columns("RRGRWT~RRNETWT~RRPUREWT").Width = .Columns("RRGRWT").Width + .Columns("RRNETWT").Width + .Columns("RRPUREWT").Width
            .Columns("RGRWT").HeaderText = "GRSWT"
            .Columns("IGRWT").HeaderText = "GRSWT"
            .Columns("RRGRWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("RRNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "PUREWT"
            .Columns("IPUREWT").HeaderText = "PUREWT"
            .Columns("RRPUREWT").HeaderText = "PUREWT"
            .Columns("RTOUCH").HeaderText = "TOUCH"
            .Columns("ITOUCH").HeaderText = "TOUCH"
        End With
    End Sub
    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            'For Each dgvCol As DataGridViewColumn In dgv.Columns
            '    dgvCol.Visible = False
            'Next
            '.Columns("PARTICULAR").Visible = True
            '.Columns("TRANNO").Visible = True
            '.Columns("TDATE").Visible = True
            '.Columns("DESCRIPTION").Visible = True
            '.Columns("ISSWT").Visible = True
            '.Columns("RECWT").Visible = True
            '.Columns("DEBIT").Visible = True
            '.Columns("CREDIT").Visible = True
            .Columns("TFILTER").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("METALNAME").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("PCS").Visible = False
            .Columns("GRSWT").Visible = False 'Not rbtGrossWeight.Checked
            .Columns("NETWT").Visible = False 'Not rbtNetWeight.Checked
            .Columns("PUREWT").Visible = False 'Not rbtPureWeight.Checked
            .Columns("IGRSWT").Visible = ChkGrossWt.Checked
            .Columns("INETWT").Visible = ChkNetWt.Checked
            .Columns("IPUREWT").Visible = ChkPureWt.Checked
            .Columns("RGRSWT").Visible = ChkGrossWt.Checked
            .Columns("RNETWT").Visible = ChkNetWt.Checked
            .Columns("RPUREWT").Visible = ChkPureWt.Checked
            .Columns("BALGRSWT").Visible = ChkGrossWt.Checked
            .Columns("BALNETWT").Visible = ChkNetWt.Checked
            .Columns("BALPUREWT").Visible = ChkPureWt.Checked

            .Columns("IALLOY").Visible = Not ChkPureWt.Checked
            .Columns("IWASTAGE").Visible = Not ChkPureWt.Checked
            .Columns("RALLOY").Visible = Not ChkPureWt.Checked
            .Columns("RWASTAGE").Visible = Not ChkPureWt.Checked

            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPUREWT").HeaderText = "PUREWT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "PUREWT"
            .Columns("IALLOY").HeaderText = "ALLOY"
            .Columns("RALLOY").HeaderText = "ALLOY"
            .Columns("IWASTAGE").HeaderText = "WASTAGE"
            .Columns("RWASTAGE").HeaderText = "WASTAGE"
            .Columns("BALPCS").HeaderText = "PCS"
            .Columns("BALGRSWT").HeaderText = "GRSWT"
            .Columns("BALNETWT").HeaderText = "NETWT"
            .Columns("BALPUREWT").HeaderText = "PUREWT"

            .Columns("PARTICULAR").Width = 200
            .Columns("TRANNO").Width = 60
            .Columns("TDATE").Width = 80
            .Columns("DESCRIPTION").Width = 120
            '.Columns("ISSWT").Width = 90
            '.Columns("RECWT").Width = 90
            .Columns("DEBIT").Width = 90
            .Columns("CREDIT").Width = 90
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("PUREWT").Width = 90
            .Columns("IPCS").Width = 40
            .Columns("IGRSWT").Width = 80
            .Columns("INETWT").Width = 80
            .Columns("IPUREWT").Width = 80
            .Columns("RPCS").Width = 40
            .Columns("RGRSWT").Width = 80
            .Columns("RNETWT").Width = 80
            .Columns("RPUREWT").Width = 80
            .Columns("BALPCS").Width = 40
            .Columns("BALGRSWT").Width = 80
            .Columns("BALNETWT").Width = 80
            .Columns("BALPUREWT").Width = 80
            .Columns("IALLOY").Width = 80
            .Columns("IWASTAGE").Width = 80
            .Columns("RALLOY").Width = 80
            .Columns("RWASTAGE").Width = 80
            .Columns("TOUCH").Width = 70

            .Columns("TDATE").HeaderText = "TRANDATE"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
            If .Columns.Contains("BALGRSWT") Then .Columns("BALGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("BALNETWT") Then .Columns("BALNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("BALPUREWT") Then .Columns("BALPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        If chkWithSpecicFormat.Checked Then
            SetGridHeadColWidth_F1(CType(sender, DataGridView))
        Else
            SetGridHeadColWidth(CType(sender, DataGridView))
        End If

    End Sub

    Public Sub SetGridHeadColWidth_F1(ByVal gridViewHeader As DataGridView)
        Dim f1 As frmSmithBalSummery_F1
        f1 = objGPack.GetParentControl(gridViewHeader)
        'If Not f1.gridViewHeader.Visible Then Exit Sub
        If f1.gridViewHeader Is Nothing Then Exit Sub
        If Not f1.gridView.ColumnCount > 0 Then Exit Sub
        If Not f1.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f1.gridViewHeader
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Width = SetGroupHeadColWid(.Columns(cnt), f1.gridView)
            Next
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f1.gridView.ColumnCount - 1
                If f1.gridView.Columns(cnt).Visible Then colWid += f1.gridView.Columns(cnt).Width
            Next
            If colWid >= f1.gridView.Width Then
                f1.gridViewHeader.Columns("SCROLL").Visible = CType(f1.gridView.Controls(1), VScrollBar).Visible
                f1.gridViewHeader.Columns("SCROLL").Width = CType(f1.gridView.Controls(1), VScrollBar).Width
                f1.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f1.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR~TRANNO~TDATE]"
        strSql += " ,''[IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE]"
        strSql += " ,''[RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE]"
        strSql += " ,''[DEBIT~CREDIT]"
        strSql += " ,''[BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT]"
        strSql += " ,''[TOUCH~RATE~DESCRIPTION~REMARK1~REMARK2]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = Nothing
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~TRANNO~TDATE").HeaderText = ""
        gridviewHead.Columns("IPCS~IGRSWT~INETWT~IPUREWT~IALLOY~IWASTAGE").HeaderText = "ISSUE"
        gridviewHead.Columns("RPCS~RGRSWT~RNETWT~RPUREWT~RALLOY~RWASTAGE").HeaderText = "RECEIPT"
        gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "AMOUNT"
        gridviewHead.Columns("BALPCS~BALGRSWT~BALNETWT~BALPUREWT~BALAMT").HeaderText = "BALANCE"
        gridviewHead.Columns("TOUCH~RATE~DESCRIPTION~REMARK1~REMARK2").HeaderText = ""
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub

    Private Function FillDetailView(ByVal AcName As String) As DataTable
        Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim chkCategory As String = GetQryString(Obj_MeFilterValues.p_chkLstCategory)
        Dim chkCostName As String = GetQryString(Obj_MeFilterValues.p_chkLstCostCentre)
        If chkCostName = "" Then chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetQryString(Obj_MeFilterValues.p_chkLstMetal)
        Dim selTran As String = ""
        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        Dim trantype As String = GetTranType(selTran)
        Dim Apptrantype As String = ""
        ''If ChkApproval.Checked = False Then Apptrantype = "IAP,RAP,AI,AR"
        If ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP,AI,AR"
        ElseIf ChkApproval.Checked = False Then
            Apptrantype = "AI,AR"
        ElseIf chkMIMRApproval.Checked = False Then
            Apptrantype = "IAP,RAP"
        End If
        If Apptrantype = "" Then Apptrantype = "''"
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET')>0 DROP TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(75),'OPENING..')PARTICULAR,CONVERT(INT,NULL) TRANNO,CONVERT(VARCHAR(12),NULL)TDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),NULL)[DESCRIPTION]"
        'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(ISSWT)-SUM(RECWT) > 0 THEN SUM(ISSWT)-SUM(RECWT) ELSE 0 END) AS ISSWT"
        'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECWT)-SUM(ISSWT) > 0 THEN SUM(RECWT)-SUM(ISSWT) ELSE 0 END) AS RECWT"

        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(ISSGRSWT)-SUM(RECGRSWT) > 0 THEN SUM(ISSGRSWT)-SUM(RECGRSWT) ELSE 0 END) AS ISSGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(ISSNETWT)-SUM(RECNETWT) > 0 THEN SUM(ISSNETWT)-SUM(RECNETWT) ELSE 0 END) AS ISSNETWT"

        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(ISSPUREWT)-SUM(RECPUREWT) > 0 THEN SUM(ISSPUREWT)-SUM(RECPUREWT) ELSE 0 END) AS ISSPUREWT"

        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECGRSWT)-SUM(ISSGRSWT) > 0 THEN SUM(RECGRSWT)-SUM(ISSGRSWT) ELSE 0 END) AS RECGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECNETWT)-SUM(ISSNETWT) > 0 THEN SUM(RECNETWT)-SUM(ISSNETWT) ELSE 0 END) AS RECNETWT"

        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RECPUREWT)-SUM(ISSPUREWT) > 0 THEN SUM(RECPUREWT)-SUM(ISSPUREWT) ELSE 0 END) AS RECPUREWT"


        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)DEBIT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)CREDIT"
        strSql += vbCrLf + " ,CONVERT(INT,CASE WHEN SUM(IPCS)-SUM(RPCS) > 0 THEN SUM(IPCS)-SUM(RPCS) ELSE 0 END) AS IPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IGRSWT)-SUM(RGRSWT) > 0 THEN SUM(IGRSWT)-SUM(RGRSWT) ELSE 0 END) AS IGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(INETWT)-SUM(RNETWT) > 0 THEN SUM(INETWT)-SUM(RNETWT) ELSE 0 END) AS INETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(IPUREWT)-SUM(RPUREWT) > 0 THEN SUM(IPUREWT)-SUM(RPUREWT) ELSE 0 END) AS IPUREWT"
        strSql += vbCrLf + " ,CONVERT(INT,CASE WHEN SUM(RPCS)-SUM(IPCS) > 0 THEN SUM(RPCS)-SUM(IPCS) ELSE 0 END) AS RPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RGRSWT)-SUM(IGRSWT) > 0 THEN SUM(RGRSWT)-SUM(IGRSWT) ELSE 0 END) AS RGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RNETWT)-SUM(INETWT) > 0 THEN SUM(RNETWT)-SUM(INETWT) ELSE 0 END) AS RNETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN SUM(RPUREWT)-SUM(IPUREWT) > 0 THEN SUM(RPUREWT)-SUM(IPUREWT) ELSE 0 END) AS RPUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0)IALLOY,CONVERT(NUMERIC(15,3),0)IWASTAGE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0)RALLOY,CONVERT(NUMERIC(15,3),0)RWASTAGE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)TOUCH"
        strSql += vbCrLf + " ,CONVERT(INT,SUM(IPCS)-SUM(RPCS))BALPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(IGRSWT)-SUM(RGRSWT))BALGRSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(INETWT)-SUM(RNETWT))BALNETWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(IPUREWT)-SUM(RPUREWT))BALPUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)BALAMT"
        strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,CONVERT(SMALLDATETIME,NULL)TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = OPE.METALID)AS METALNAME,CONVERT(VARCHAR(15),NULL)AS BATCHNO,'N' as TFILTER"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL) REMARK1,CONVERT(VARCHAR(200),NULL) REMARK2,CONVERT(NUMERIC(15,2),NULL)AS RATE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT "
        'If ChkGrossWt.Checked Then
        strSql += vbCrLf + " CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS ISSGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RECGRSWT"
        'End If
        'If ChkNetWt.Checked Then
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS ISSNETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RECNETWT"
        'End If
        'If ChkPureWt.Checked Then
        If PurewtPer <> 0 Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN TRANTYPE = 'I' THEN (PUREWT/" & PurewtPer & ")*100 ELSE 0 END) AS ISSPUREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),CASE WHEN TRANTYPE = 'R' THEN (PUREWT/" & PurewtPer & ")*100 ELSE 0 END) AS RECPUREWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS ISSPUREWT"
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RECPUREWT"
        End If
        'End If
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PCS ELSE 0 END AS IPCS"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN GRSWT ELSE 0 END AS IGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN NETWT ELSE 0 END AS INETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE 0 END AS IPUREWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PCS ELSE 0 END AS RPCS"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE 0 END AS RGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN NETWT ELSE 0 END AS RNETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'R' THEN PUREWT ELSE 0 END AS RPUREWT"
        strSql += vbCrLf + " ,0 DEBIT,0 CREDIT,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = O.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT O"
        strSql += vbCrLf + " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCmbTranType.Text = "ALL" And ChkApproval.Checked = False And chkMIMRApproval.Checked = False Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')<>'A' "
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='A' "
        End If
        If chkCmbTranType.Text = "INTERNAL TRANSFER" Then
            strSql += vbCrLf + "  AND ISNULL(APPROVAL,'')='I' "
        End If
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " )OPE"
        strSql += vbCrLf + " GROUP BY METALID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " /** TRANSACTION **/"
        strSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS PARTICULAR"
        strSql += vbCrLf + " ,I.TRANNO,CONVERT(VARCHAR,I.TRANDATE,103)TDATE"
        strSql += vbCrLf + " ,CASE WHEN I.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        'If ChkGrossWt.Checked Then
        strSql += vbCrLf + " ,I.GRSWT"
        If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') AND P.METALTYPE='O' THEN ISNULL(I.WASTAGE,0) WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
        strSql += vbCrLf + " ISSGRSWT"
        'End If
        '''strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
        'If ChkNetWt.Checked Then
        strSql += vbCrLf + " ,I.NETWT"
        If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') AND P.METALTYPE='O' THEN ISNULL(I.WASTAGE,0) WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
        strSql += vbCrLf + " ISSNETWT"
        'End If
        '''strSql += vbCrLf + " ,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
        '''If ChkPureWt.Checked Then
        ' ''If PurewtPer <> 0 Then
        ' ''    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
        ' ''Else
        ' ''    strSql += vbCrLf + " ,I.PUREWT ISSWT,0 RECWT,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"
        ' ''End If
        '''End If

        If PurewtPer <> 0 Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(I.PUREWT/" & PurewtPer & ")*100) ISSPUREWT"
        Else
            strSql += vbCrLf + " ,I.PUREWT ISSPUREWT"
        End If
        strSql += vbCrLf + " ,0 RECGRSWT,0 RECNETWT,0 RECPUREWT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN AMOUNT ELSE 0 END DEBIT,0 CREDIT"

        strSql += vbCrLf + " ,I.PCS IPCS"
        strSql += vbCrLf + " ,I.GRSWT "
        'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
        strSql += vbCrLf + " IGRSWT"
        strSql += vbCrLf + " ,I.NETWT "
        'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN ISNULL(I.ALLOY,0)-ISNULL(I.WASTAGE,0) ELSE 0 END "
        strSql += vbCrLf + " INETWT"
        strSql += vbCrLf + " ,I.PUREWT IPUREWT"
        strSql += vbCrLf + " ,0 RPCS,0 RGRSWT,0 RNETWT,0 RPUREWT"
        strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.ALLOY ELSE 0 END IALLOY,CASE WHEN I.TRANTYPE NOT IN('MI','AI') THEN I.WASTAGE ELSE 0 END IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE,I.TOUCH,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,I.TRANDATE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  "
        strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS METALNAME"
        strSql += vbCrLf + " ,BATCHNO"
        If chkWithAppAmt.Checked Then
            strSql += vbCrLf + " ,(CASE WHEN I.TRANTYPE IN ('IPU','RPU','RAP','IAP')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        Else
            strSql += vbCrLf + " ,(CASE WHEN I.TRANTYPE IN ('IPU','RPU')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        End If
        strSql += vbCrLf + " ,REMARK1,REMARK2,CONVERT(NUMERIC(15,2),CASE WHEN I.TRANTYPE='IRC' THEN I.RATE ELSE NULL END)AS RATE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITYMAST AS P ON C.PURITYID = P.PURITYID"
        If chkSales.Checked = True Then
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + " WHERE I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('IPU') AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            End If
        Else
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('SA') AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " WHERE I.TRANTYPE NOT IN ('IPU','SA') AND I.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            End If

        End If

        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE IN (" & trantype & ")"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL"


        strSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS PARTICULAR"
        strSql += vbCrLf + " ,R.TRANNO,CONVERT(VARCHAR,R.TRANDATE,103)TDATE"
        strSql += vbCrLf + " ,CASE WHEN R.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID) END AS [DESCRIPTION]"
        ' ''If ChkGrossWt.Checked Then
        ' ''    strSql += vbCrLf + " ,0 ISSUE,"
        ' ''    strSql += vbCrLf + " R.GRSWT"
        ' ''    If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
        ' ''    strSql += vbCrLf + " RECEIPT"
        ' ''End If
        ' ''strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR','RRE') THEN AMOUNT ELSE 0 END CREDIT"
        ' ''If ChkNetWt.Checked Then
        ' ''    strSql += vbCrLf + " ,0 ISSUE"
        ' ''    strSql += vbCrLf + " ,R.NETWT"
        ' ''    If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
        ' ''    strSql += vbCrLf + " RECEIPT"
        ' ''End If
        ' ''strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR','RRE') THEN AMOUNT ELSE 0 END CREDIT"
        ' ''If ChkPureWt.Checked Then
        ' ''    If PurewtPer <> 0 Then
        ' ''        strSql += vbCrLf + " ,0 ISSUE,CONVERT(NUMERIC(15,3),(R.PUREWT/" & PurewtPer & ")*100) RECEIPT,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR','RRE') THEN AMOUNT ELSE 0 END CREDIT"
        ' ''    Else
        ' ''        strSql += vbCrLf + " ,0 ISSUE,R.PUREWT RECEIPT,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR','RRE') THEN AMOUNT ELSE 0 END CREDIT"
        ' ''    End If
        ' ''End If




        strSql += vbCrLf + " ,0 ISSGRSWT"
        strSql += vbCrLf + " ,0 ISSNETWT"
        strSql += vbCrLf + " ,0 ISSPUREWT"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,R.GRSWT"
            If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR')  THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " RECGRSWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR')  THEN  R.GRSWT"
            If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR')  THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END   "
            strSql += vbCrLf + " ELSE 0 END RECGRSWT"
        End If
        If chkSales.Checked Then
            strSql += vbCrLf + " ,R.NETWT"
            If RPT_SMITHABS_CATGRP Then
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') AND C.CATGROUP<>'L' THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            Else
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            End If
            strSql += vbCrLf + " RECNETWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR')  THEN R.NETWT"
            If RPT_SMITHABS_CATGRP Then
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') AND C.CATGROUP<>'L' THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            Else
                If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            End If
            strSql += vbCrLf + " ELSE 0 END RECNETWT"
        End If

        If chkSales.Checked Then
            If PurewtPer <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(R.PUREWT/" & PurewtPer & ")*100) RECPUREWT"
            Else
                strSql += vbCrLf + " ,R.PUREWT RECPUREWT"
            End If
        Else
            If PurewtPer <> 0 Then
                strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR')  THEN CONVERT(NUMERIC(15,3),(R.PUREWT/" & PurewtPer & ")*100) ELSE 0 END RECPUREWT"
            Else
                strSql += vbCrLf + " , CASE WHEN R.TRANTYPE NOT IN('SR')  THEN R.PUREWT ELSE 0 END RECPUREWT"
            End If
        End If


        strSql += vbCrLf + " ,0 DEBIT,CASE WHEN R.TRANTYPE NOT IN('AR','RRE') THEN AMOUNT ELSE 0 END CREDIT"


        strSql += vbCrLf + " ,0 IPCS,0 IGRSWT,0 INETWT,0 IPUREWT"
        strSql += vbCrLf + " ,R.PCS RPCS"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,R.GRSWT "
            'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " RGRSWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR') THEN R.GRSWT "
            'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " ELSE 0 END RGRSWT"
        End If
        If chkSales.Checked Then
            strSql += vbCrLf + " ,R.NETWT "
            'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " RNETWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR') THEN R.NETWT "
            'If ChkwithWast.Checked Then strSql += vbCrLf + " + CASE WHEN R.TRANTYPE NOT IN('AR') THEN ISNULL(R.WASTAGE,0)+ISNULL(R.ALLOY,0) ELSE 0 END "
            strSql += vbCrLf + " ELSE 0 END RNETWT"
        End If
        If chkSales.Checked Then
            strSql += vbCrLf + " ,R.PUREWT RPUREWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('SR') THEN R.PUREWT ELSE 0 END RPUREWT"
        End If

        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN R.ALLOY ELSE 0 END RALLOY"
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE NOT IN('AR') THEN R.WASTAGE ELSE 0 END RWASTAGE"
        strSql += vbCrLf + " ,R.TOUCH,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,R.TRANDATE"
        strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID =  "
        strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METALNAME"
        strSql += vbCrLf + " ,BATCHNO"
        If chkWithAppAmt.Checked Then
            strSql += vbCrLf + " ,(CASE WHEN R.TRANTYPE IN ('IPU','RPU','RAP','IAP')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        Else
            strSql += vbCrLf + " ,(CASE WHEN R.TRANTYPE IN ('IPU','RPU')  THEN 'Y' ELSE 'N' END) AS TFILTER"
        End If
        strSql += vbCrLf + " ,CASE WHEN R.TRANTYPE IN('SR')  THEN '(GrsWt:'+ CONVERT(VARCHAR,R.GRSWT) + ',NetWt:'+ CONVERT(VARCHAR,R.NETWT) + ') ' + REMARK1 ELSE REMARK1 END REMARK1"
        strSql += vbCrLf + " ,REMARK2,CONVERT(NUMERIC(15,2),CASE WHEN R.TRANTYPE='RRC' THEN R.RATE ELSE NULL END)AS RATE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = R.CATCODE"
        If chkSales.Checked = True Then
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + " WHERE R.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " WHERE R.TRANTYPE NOT IN ('RPU') AND R.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            End If
        Else
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + " WHERE R.TRANTYPE NOT IN ('SR') AND R.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " WHERE R.TRANTYPE NOT IN ('RPU','SR') AND R.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            End If

        End If

        strSql += vbCrLf + " AND ISNULL(R.CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND R.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE IN (" & trantype & ")"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND R.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + " AND R.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME,S.TRANNO,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,IM.ITEMNAME [DESCRIPTION]"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) ISSGRSWT,CONVERT(NUMERIC(15,3),S.STNWT) ISSNETWT,CONVERT(NUMERIC(15,3),S.STNWT) ISSPUREWT"
        strSql += vbCrLf + " ,0 RECGRSWT,0 RECNETWT,0 RECPUREWT"
        strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        strSql += vbCrLf + " ,S.STNPCS PCS,CONVERT(NUMERIC(15,3),S.STNWT) GRSWT,CONVERT(NUMERIC(15,3),S.STNWT) NETWT,CONVERT(NUMERIC(15,3),S.STNWT) PUREWT"
        strSql += vbCrLf + " ,0 PCS,0 GRSWT,0 NETWT,0 PUREWT"
        strSql += vbCrLf + " ,0 IALLOY,0 IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE ,0 TOUCH ,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,S.TRANDATE,ME.METALNAME AS METALNAME,S.BATCHNO,(case when s.TRANTYPE IN ('IPU','RPU')  then 'Y' else 'N' end) as TFILTER"
        strSql += vbCrLf + " ,REMARK1,REMARK2,CONVERT(NUMERIC(15,2),NULL)AS RATE"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS I ON S.ISSSNO = I.SNO "
        If chkSales.Checked = True Then
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('IPU') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If

        Else
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('SA') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('IPU','SA') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If

        End If
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CA.CATNAME,S.TRANNO,CONVERT(VARCHAR,S.TRANDATE,103)TDATE,IM.ITEMNAME [DESCRIPTION]"
        strSql += vbCrLf + " ,0 ISSUE,0 ISSUE,0 ISSUE"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) RECEIPT,CONVERT(NUMERIC(15,3),S.STNWT) RECEIPT,CONVERT(NUMERIC(15,3),S.STNWT) RECEIPT"
        Else
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END RECEIPT"
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END RECEIPT"
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END RECEIPT"
        End If
        strSql += vbCrLf + " ,0 DEBIT,0 CREDIT,0 PCS,0 GRSWT,0 NETWT,0 PUREWT"
        strSql += vbCrLf + " ,S.STNPCS PCS"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),S.STNWT) GRSWT,CONVERT(NUMERIC(15,3),S.STNWT) NETWT,CONVERT(NUMERIC(15,3),S.STNWT) PUREWT"
        Else
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END GRSWT "
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END NETWT "
            strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE NOT IN ('SR') THEN CONVERT(NUMERIC(15,3),S.STNWT) ELSE 0 END PUREWT "
        End If
        strSql += vbCrLf + " ,0 IALLOY,0 IWASTAGE"
        strSql += vbCrLf + " ,0 RALLOY,0 RWASTAGE ,0 TOUCH ,0 BALPCS,0 BALGRSWT,0 BALNETWT,0 BALPUREWT,0 BALAMT"
        strSql += vbCrLf + " ,2 RESULT,' 'COLHEAD,S.TRANDATE,ME.METALNAME AS METALNAME,S.BATCHNO,(case when S.TRANTYPE IN ('IPU','RPU')  then 'Y' else 'N' end) as TFILTER"
        strSql += vbCrLf + " ,CASE WHEN S.TRANTYPE IN('SR')  THEN '(GrsWt:'+ CONVERT(VARCHAR,S.STNWT) + ',NetWt:'+ CONVERT(VARCHAR,S.STNWT) + ') ' + REMARK1 ELSE REMARK1 END REMARK1"
        strSql += vbCrLf + " ,REMARK2,CONVERT(NUMERIC(15,2),NULL)AS RATE"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS S"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetalName <> "", " AND ME.METALNAME IN (" & chkMetalName & ")", "")
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS I ON S.ISSSNO = I.SNO "
        If chkSales.Checked Then
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('RPU') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If
        Else
            If ChkWithDirectPur.Checked Then
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('SR') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + "  WHERE I.TRANTYPE NOT IN ('RPU','SR') AND I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If

        End If

        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If trantype <> "" Then strSql += vbCrLf + "  AND S.TRANTYPE IN (" & trantype & ")"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Apptrantype.Trim <> "" Then strSql += vbCrLf + "  AND I.TRANTYPE NOT IN ('" & Apptrantype.Replace(",", "','") & "')"
        strSql += vbCrLf + "  AND S.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + "  ORDER BY TRANDATE,TRANNO"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET ADD SNO INT IDENTITY(1,1)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        'strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS= ISNULL(IPCS,0) - ISNULL(RPCS,0),"
        'strSql += vbCrLf + " BALGRSWT= ISNULL(IGRSWT,0) - ISNULL(RGRSWT,0) ,"
        'strSql += vbCrLf + " BALNETWT = ISNULL(INETWT, 0) - ISNULL(RNETWT, 0)"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()




        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT')>0 DROP TABLE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT"
        strSql += vbCrLf + " SELECT BATCHNO,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE 0 END CREDIT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT BATCHNO,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END)AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + " WHERE TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        strSql += vbCrLf + " AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET)"
        strSql += vbCrLf + " GROUP BY BATCHNO"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " "
        'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET DEBIT = AM.DEBIT,CREDIT = AM.CREDIT"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET AS DE,TEMPTABLEDB..TEMP" & SYSID & "SMITHBALAMT AS AM"
        'strSql += vbCrLf + " WHERE DE.BATCHNO = AM.BATCHNO"
        'strSql += vbCrLf + " AND DE.SNO = (SELECT TOP 1 SNO FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET WHERE BATCHNO = AM.BATCHNO ORDER BY SNO)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT '','OPENING AMOUNT..',DEBIT,CREDIT,1,'',''"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN -1*SUM(AMOUNT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "     WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        If chkWithAppAmt.Checked Then
            strSql += vbCrLf + " 	UNION ALL"
            strSql += vbCrLf + " 	SELECT SUM(CASE WHEN TRANTYPE='I' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT FROM " & cnStockDb & "..OPENWEIGHT"
            strSql += vbCrLf + "    WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            strSql += vbCrLf + "    AND ISNULL(APPROVAL,'')='A'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + "         AND COMPANYID = '" & strCompanyId & "'"
        End If
        strSql += vbCrLf + " 	)AMT"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " )X WHERE NOT(DEBIT = 0 AND CREDIT = 0)"
        strSql += vbCrLf + " " + vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        'strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(PARTICULAR,TRANNO,TDATE,ISSWT,RECWT,DEBIT,CREDIT,RPCS,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER)"
        'strSql += vbCrLf + " SELECT "
        'strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
        'strSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN NETWT  ELSE 0 END AS DEBITwt"
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN NETWT  ELSE 0 END AS CREDITwt"
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS DEBIT"
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS CREDIT"
        'strSql += vbCrLf + " ,pcs"
        'strSql += vbCrLf + " ,1 RESULT"
        'strSql += vbCrLf + " ,TRANDATE"
        'strSql += vbCrLf + " ,''METALNAME,BATCHNO,''COLHEAD,'Y' as TFILTER"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        'strSql += vbCrLf + " 	WHERE TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " 	AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + "     AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'strSql += vbCrLf + "     AND COMPANYID = '" & strCompanyId & "'"
        'strSql += vbCrLf + "     AND BATCHNO  IN (SELECT ISNULL(BATCHNO,'') FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET "
        'If chkMetalName <> "" Then strSql += vbCrLf + "   WHERE METALNAME IN (" & chkMetalName & " ))"

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(PARTICULAR,TRANNO,TDATE/*,ISSWT,RECWT*/,RPCS,DEBIT,CREDIT,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
        strSql += vbCrLf + " ,T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)TDATE"
        ' For nathella as per magesh sir instruction
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN NETWT  ELSE 0 END AS DEBITWT"
        'strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN NETWT  ELSE 0 END AS CREDITWT"
        'strSql += vbCrLf + " ,PCS"

        'strSql += vbCrLf + " ,0 AS DEBITWT"
        'strSql += vbCrLf + " ,0 AS CREDITWT"
        strSql += vbCrLf + " ,0 AS PCS"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,1 RESULT,T.TRANDATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 METALNAME FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET) ,T.BATCHNO,''COLHEAD,'Y' as TFILTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE T.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND T.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
        If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET)"
        'strSql += vbCrLf + "     AND BATCHNO  IN (SELECT ISNULL(BATCHNO,'') FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET "
        'If chkMetalName <> "" Then strSql += vbCrLf + "   WHERE METALNAME IN (" & chkMetalName & " ))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If ChkRelatedtran.Checked = False Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(PARTICULAR,TRANNO,TDATE/*,ISSWT,RECWT,RPCS*/,DEBIT,CREDIT,RESULT,TRANDATE,METALNAME,BATCHNO,COLHEAD,TFILTER,REMARK1,REMARK2)"
            strSql += vbCrLf + " SELECT DISTINCT "
            strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)AS PARTICULAR"
            strSql += vbCrLf + " ,T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)TDATE"
            strSql += vbCrLf + " /*,0 AS DEBITWT,0 AS CREDITWT,0 AS PCS*/"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN T.AMOUNT ELSE 0 END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN T.AMOUNT ELSE 0 END AS CREDIT"
            strSql += vbCrLf + " ,1 RESULT,T.TRANDATE"
            strSql += vbCrLf + " ,'' METALNAME,T.BATCHNO,''COLHEAD,'Y' as TFILTER"
            strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO)REMARK1"
            strSql += vbCrLf + " ,(SELECT TOP 1 REMARK2 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO)REMARK2"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
            strSql += vbCrLf + " WHERE T.TRANDATE <= '" & Obj_MeFilterValues.p_dtpAsOnDate.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND T.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & AcName & "')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT ISNULL(BATCHNO,'') FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET "
            If chkMetalName <> "" Then strSql += vbCrLf + "   WHERE METALNAME IN (" & chkMetalName & " )"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,0 RESULT,'T'COLHEAD,'N' FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"

        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,ISSGRSWT,ISSNETWT,ISSPUREWT,RECGRSWT,RECNETWT,RECPUREWT,DEBIT,CREDIT,RESULT,COLHEAD"
        strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IPUREWT,RPCS,RGRSWT,RNETWT,RPUREWT,IALLOY,IWASTAGE,RALLOY,RWASTAGE,TFILTER,BALPCS,BALGRSWT,BALNETWT,BALPUREWT"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT METALNAME,'SUB TOTAL'"
        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE ISSGRSWT END),0))"
        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE ISSNETWT END),0))"
        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE ISSPUREWT END),0))"

        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE RECGRSWT END),0))"
        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE RECNETWT END),0))"
        strSql += vbCrLf + " ,SUM(ISNULL((CASE WHEN TFILTER='Y' THEN 0 ELSE RECPUREWT END),0))"

        strSql += vbCrLf + " ,SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),3 RESULT,'S'COLHEAD"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE IPCS END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE IGRSWT END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE INETWT END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE IPUREWT END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RPCS END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RGRSWT END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RNETWT END),SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RPUREWT END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE IALLOY END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE IWASTAGE END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RALLOY END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER='Y' THEN 0 ELSE RWASTAGE END)"
        strSql += vbCrLf + " ,'N',SUM(CASE WHEN TFILTER = 'Y' THEN 0 ELSE BALPCS END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER = 'Y' THEN  0 ELSE BALGRSWT END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER = 'Y' THEN  0 ELSE BALNETWT END)"
        strSql += vbCrLf + " ,SUM(CASE WHEN TFILTER = 'Y' THEN  0 ELSE BALPUREWT END)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET GROUP BY METALNAME"
        'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,ISSWT,RECWT,DEBIT,CREDIT,RESULT,COLHEAD"
        'strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IPUREWT,RPCS,RGRSWT,RNETWT,RPUREWT,IALLOY,IWASTAGE,RALLOY,RWASTAGE,TFILTER,BALPCS,BALGRSWT,BALNETWT"
        'strSql += vbCrLf + " )"
        'strSql += vbCrLf + " SELECT 'ZZZZZ','GRAND TOTAL',SUM(ISNULL((case when tfilter='Y' then 0 else ISSWT end),0)),SUM(ISNULL((case when tfilter='Y' then 0 else RECWT end),0)),SUM(ISNULL(DEBIT,0)),SUM(ISNULL(CREDIT,0)),4 RESULT,'G'COLHEAD"
        'strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IPCS end),SUM(case when tfilter='Y' then 0 else IGRSWT end),SUM(case when tfilter='Y' then 0 else INETWT end),SUM(case when tfilter='Y' then 0 else IPUREWT end)"
        'strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else RPCS end),SUM(RGRSWT),SUM(RNETWT),SUM(RPUREWT)"
        'strSql += vbCrLf + " ,SUM(case when tfilter='Y' then 0 else IALLOY end),SUM(case when tfilter='Y' then 0 else IWASTAGE end),SUM(case when tfilter='Y' then 0 else RALLOY end),SUM(case when tfilter='Y' then 0 else RWASTAGE end),'N',Sum(case when tfilter='Y' then 0 else BALPCS end),Sum(case when tfilter='Y' then 0 else BALGRSWT end),Sum(case when tfilter='Y' then 0 else BALNETWT end)"
        'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET WHERE RESULT = 3"


        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET(METALNAME,PARTICULAR,ISSGRSWT,ISSNETWT,ISSPUREWT,RECGRSWT,RECNETWT,RECPUREWT,DEBIT,CREDIT,RESULT,COLHEAD,TFILTER)"
        strSql += vbCrLf + " SELECT METALNAME,' '"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISSGRSWT)-SUM(RECGRSWT) > 0 THEN SUM(ISSGRSWT)-SUM(RECGRSWT) ELSE 0 END ISSGRSWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISSNETWT)-SUM(RECNETWT) > 0 THEN SUM(ISSNETWT)-SUM(RECNETWT) ELSE 0 END ISSNETWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(ISSPUREWT)-SUM(RECPUREWT) > 0 THEN SUM(ISSPUREWT)-SUM(RECPUREWT) ELSE 0 END ISSPUREWT"


        strSql += vbCrLf + " ,CASE WHEN SUM(RECGRSWT)-SUM(ISSGRSWT) > 0 THEN SUM(RECGRSWT)-SUM(ISSGRSWT) ELSE 0 END RECGRSWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(RECNETWT)-SUM(ISSNETWT) > 0 THEN SUM(RECNETWT)-SUM(ISSNETWT) ELSE 0 END RECNETWT"
        strSql += vbCrLf + " ,CASE WHEN SUM(RECPUREWT)-SUM(ISSPUREWT) > 0 THEN SUM(RECPUREWT)-SUM(ISSPUREWT) ELSE 0 END RECPUREWT"

        strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT)-SUM(CREDIT) > 0 THEN SUM(DEBIT)-SUM(CREDIT) ELSE 0 END DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)-SUM(DEBIT) > 0 THEN SUM(CREDIT)-SUM(DEBIT) ELSE 0 END CREDIT"
        strSql += vbCrLf + " ,5 RESULT,'G'COLHEAD,'N'"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET WHERE RESULT = 3 GROUP BY METALNAME"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtsmithbal As New DataTable
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET ORDER BY METALNAME,RESULT,TRANDATE,TRANNO "
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtsmithbal)
        Dim BALPCS As Integer = 0
        Dim BALGRSWT As Decimal = 0
        Dim BALNETWT As Decimal = 0
        Dim BALPUREWT As Decimal = 0
        Dim BALAMT As Decimal = 0
        For I As Integer = 0 To dtsmithbal.Rows.Count - 1
            If dtsmithbal.Rows(I).Item("RESULT").ToString = 0 Then
                BALPCS = 0
                BALGRSWT = 0
                BALNETWT = 0
                BALPUREWT = 0
                If ChkRelatedtran.Checked And chkAmtbal.Checked Then BALAMT = 0
            End If
            If dtsmithbal.Rows(I).Item("COLHEAD").ToString <> "T" Then
                If dtsmithbal.Rows(I).Item("COLHEAD").ToString = "S" Or dtsmithbal.Rows(I).Item("COLHEAD").ToString = "G" Then
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS= (" & BALPCS & "),"
                    strSql += vbCrLf + " BALGRSWT= (" & BALGRSWT & "),"
                    strSql += vbCrLf + " BALNETWT = (" & BALNETWT & "),"
                    strSql += vbCrLf + " BALPUREWT = (" & BALPUREWT & ")"
                    strSql += vbCrLf + " WHERE TFILTER <> 'Y' AND SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & ""
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT=(" & BALAMT & ")"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & "" ' AND  TFILTER <> 'N' "
                    'If ChkRelatedtran.Checked And chkAmtbal.Checked Then strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    dtsmithbal.Rows(I).Item("BALPCS") = BALPCS
                    dtsmithbal.Rows(I).Item("BALGRSWT") = BALGRSWT
                    dtsmithbal.Rows(I).Item("BALNETWT") = BALNETWT
                    dtsmithbal.Rows(I).Item("BALPUREWT") = BALPUREWT
                    dtsmithbal.Rows(I).Item("BALAMT") = BALAMT
                Else
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS= (" & BALPCS & "+ ISNULL(IPCS,0) - ISNULL(RPCS,0)),"
                    strSql += vbCrLf + " BALGRSWT= (" & BALGRSWT & "+ ISNULL(ISSGRSWT,0) - ISNULL(RECGRSWT,0) ),"
                    strSql += vbCrLf + " BALNETWT = (" & BALNETWT & "+ ISNULL(ISSNETWT, 0) - ISNULL(RECNETWT, 0)),"
                    strSql += vbCrLf + " BALPUREWT = (" & BALPUREWT & "+ ISNULL(ISSPUREWT, 0) - ISNULL(RECPUREWT, 0))"
                    strSql += vbCrLf + " WHERE TFILTER <> 'Y' AND SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & ""
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT = (" & BALAMT & "+ ISNULL(DEBIT, 0)  - ISNULL(CREDIT, 0))"
                    strSql += vbCrLf + " WHERE SNO=" & dtsmithbal.Rows(I).Item("SNO").ToString & " AND  TFILTER <> 'N' "
                    'If ChkRelatedtran.Checked And chkAmtbal.Checked Then strSql += vbCrLf + " AND  TFILTER <> 'Y' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    dtsmithbal.Rows(I).Item("BALPCS") = BALPCS + Val(dtsmithbal.Rows(I).Item("IPCS").ToString) - Val(dtsmithbal.Rows(I).Item("RPCS").ToString)
                    dtsmithbal.Rows(I).Item("BALGRSWT") = BALGRSWT + Val(dtsmithbal.Rows(I).Item("ISSGRSWT").ToString) - Val(dtsmithbal.Rows(I).Item("RECGRSWT").ToString)
                    dtsmithbal.Rows(I).Item("BALNETWT") = BALNETWT + Val(dtsmithbal.Rows(I).Item("ISSNETWT").ToString) - Val(dtsmithbal.Rows(I).Item("RECNETWT").ToString)
                    dtsmithbal.Rows(I).Item("BALPUREWT") = BALPUREWT + Val(dtsmithbal.Rows(I).Item("IPUREWT").ToString) - Val(dtsmithbal.Rows(I).Item("RPUREWT").ToString)
                    dtsmithbal.Rows(I).Item("BALAMT") = BALAMT + Val(dtsmithbal.Rows(I).Item("DEBIT").ToString) - Val(dtsmithbal.Rows(I).Item("CREDIT").ToString)
                End If

            End If
            If dtsmithbal.Rows(I).Item("COLHEAD").ToString.Trim = "" Then
                BALPCS = Val(dtsmithbal.Rows(I).Item("BALPCS").ToString)
                BALGRSWT = Val(dtsmithbal.Rows(I).Item("BALGRSWT").ToString)
                BALNETWT = Val(dtsmithbal.Rows(I).Item("BALNETWT").ToString)
                BALPUREWT = Val(dtsmithbal.Rows(I).Item("BALPUREWT").ToString)
                BALAMT = IIf(dtsmithbal.Rows(I).Item("TFILTER").ToString <> "N", Val(dtsmithbal.Rows(I).Item("BALAMT").ToString), BALAMT)
            End If
        Next

        strSql = " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET ISSGRSWT = NULL WHERE ISSGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RECPUREWT = NULL WHERE RECPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RECNETWT = NULL WHERE RECNETWT = 0"

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET ISSPUREWT = NULL WHERE ISSPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET ISSNETWT = NULL WHERE ISSNETWT = 0"

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RECGRSWT = NULL WHERE RECGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IPCS = NULL WHERE IPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IGRSWT = NULL WHERE IGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET INETWT = NULL WHERE INETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IPUREWT = NULL WHERE IPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RPCS = NULL WHERE RPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RGRSWT = NULL WHERE RGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RNETWT = NULL WHERE RNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RPUREWT = NULL WHERE RPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IALLOY = NULL WHERE IALLOY = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET IWASTAGE = NULL WHERE IWASTAGE = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RALLOY = NULL WHERE RALLOY = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET RWASTAGE = NULL WHERE RWASTAGE = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET TOUCH = NULL WHERE TOUCH = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET DEBIT = NULL WHERE DEBIT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET CREDIT = NULL WHERE CREDIT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPCS = NULL WHERE BALPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALGRSWT = NULL WHERE BALGRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALNETWT = NULL WHERE BALNETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALPUREWT = NULL WHERE BALPUREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET SET BALAMT = NULL WHERE BALAMT= 0"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET1)>0"

        strSql = vbCrLf + " SELECT PARTICULAR,TRANNO,TDATE"
        strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IPUREWT"
        strSql += vbCrLf + " ,IALLOY,IWASTAGE"
        'strSql += vbCrLf + " ,ISSWT"
        strSql += vbCrLf + " ,RPCS,RGRSWT,RNETWT,RPUREWT"
        strSql += vbCrLf + " ,RALLOY,RWASTAGE"
        'strSql += vbCrLf + " ,RECWT"
        strSql += vbCrLf + " ,DEBIT,CREDIT"
        'strSql += vbCrLf + " ,BALPCS,CONVERT(VARCHAR,BALGRSWT)BALGRSWT,CONVERT(VARCHAR,BALNETWT)BALNETWT,CONVERT(VARCHAR,BALPUREWT)BALPUREWT"
        strSql += vbCrLf + " ,BALPCS,CASE WHEN ISNULL(CONVERT(VARCHAR,BALGRSWT),'')='' AND RESULT=5 THEN '-NIL-' ELSE CONVERT(VARCHAR,BALGRSWT) END BALGRSWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(CONVERT(VARCHAR,BALNETWT),'')='' AND RESULT=5 THEN '-NIL-' ELSE CONVERT(VARCHAR,BALNETWT) END BALNETWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(CONVERT(VARCHAR,BALPUREWT),'')='' AND RESULT=5 THEN '-NIL-' ELSE CONVERT(VARCHAR,BALPUREWT) END BALPUREWT"
        strSql += vbCrLf + " ,BALAMT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IPCS,0) <> 0 THEN IPCS ELSE RPCS END AS PCS"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IGRSWT,0) <> 0 THEN IGRSWT ELSE RGRSWT END AS GRSWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(INETWT,0) <> 0 THEN INETWT ELSE RNETWT END AS NETWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(IPUREWT,0) <> 0 THEN IPUREWT ELSE RPUREWT END AS PUREWT"
        strSql += vbCrLf + " ,TOUCH,RATE"
        strSql += vbCrLf + " ,DESCRIPTION,REMARK1,REMARK2"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE,METALNAME,BATCHNO,SNO,TFILTER"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        'strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSID & "SMITHBALDET"
        strSql += vbCrLf + " ORDER BY METALNAME,RESULT,TRANDATE,TRANNO"


        Dim dtGrid As New DataTable("DETAILED")
        Dim dtCol As New DataColumn("KEYNO")
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt As Integer = 1 To dgv.ColumnCount - 1
                .Columns(cnt).Width = 100
                If chkWithAbs.Checked Or chkWithSpecicFormat.Checked Then
                    If .Columns(cnt).HeaderText.Contains("_GRSWT") Then
                        If ChkGrossWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                    If .Columns(cnt).HeaderText.Contains("_NETWT") Then
                        If ChkNetWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                    If .Columns(cnt).HeaderText.Contains("_PUREWT") Then
                        If ChkPureWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                    If .Columns(cnt).HeaderText.Contains("OPENING_") Then
                        .Columns(cnt).HeaderText = "OPENING"
                    End If
                    If .Columns(cnt).HeaderText.Contains("ISSUE_") Then
                        .Columns(cnt).HeaderText = "ISSUE"
                    End If
                    If .Columns(cnt).HeaderText.Contains("RECEIPT_") Then
                        .Columns(cnt).HeaderText = "RECEIPT"
                    End If
                    If .Columns(cnt).HeaderText.Contains("CLOSING_") Then
                        .Columns(cnt).HeaderText = "CLOSING"
                    End If
                Else
                    If .Columns(cnt).HeaderText.Contains("_GRSWT") Then
                        .Columns(cnt).HeaderText = "GRSWT"
                        If ChkGrossWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                    If .Columns(cnt).HeaderText.Contains("_NETWT") Then
                        .Columns(cnt).HeaderText = "NETWT"
                        If ChkNetWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                    If .Columns(cnt).HeaderText.Contains("_PUREWT") Then
                        .Columns(cnt).HeaderText = "PUREWT"
                        If ChkPureWt.Checked = False Then .Columns(cnt).Visible = False
                    End If
                End If
            Next
            If chkWithAbs.Checked = True Or chkWithSpecicFormat.Checked Then
                .Columns("ACNAME").HeaderText = "SMITH NAME"
                .Columns("ACNAME").Width = 250
                If .Columns.Contains("COLHEAD") = True Then .Columns("COLHEAD").Visible = False
            Else
                .Columns("PARTICULAR").Width = 250
                .Columns("KEYNO").Visible = False
                .Columns("ACNAME").Visible = False
                If .Columns.Contains("COLHEAD") = True Then .Columns("COLHEAD").Visible = False
            End If
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub frmSmithBalanceSummaryReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmSmithBalanceSummaryReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER"
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
        cmbtrantypeload()
        'FillAcname()
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
        If chkCustomer.Checked = True Then
            strSql += " UNION ALL"
            strSql += " SELECT 'URD ISSUE',12 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT 'URD ISSUE RETURN',13 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT 'URD PURCHASE',14 RESULT"
        End If
        strSql += " ORDER BY result"
        DtTrantype = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTrantype)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbTranType, DtTrantype, "TTYPE", , "ALL")
    End Function

    Private Function FillAcname()


        Dim Actype As String = ""
        If chkDealer.Checked = False And chkSmith.Checked = False And chkInternal.Checked = False And chkOthers.Checked = False And chkCustomer.Checked = False Then
            Actype = "'D','G','I','O','C'"
        Else
            If chkDealer.Checked Then
                Actype += "'D',"
            End If
            If chkSmith.Checked Then
                Actype += "'G',"
            End If
            If chkInternal.Checked Then
                Actype += "'I',"
            End If
            If chkOthers.Checked Then
                Actype += "'O',"
            End If
            If chkCustomer.Checked Then
                Actype += "'C',"
            End If
            If Actype <> "" Then
                Actype = Mid(Actype, 1, Actype.Length - 1)
            End If
        End If


        strSql = " SELECT 'ALL' ACNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE"
        'strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        If ActIVE = "B" Then
            strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        ElseIf ActIVE = "Y" Then
            strSql += "  ISNULL(ACTIVE,'') = 'Y' "
        Else
            strSql += "  ISNULL(ACTIVE,'') ='N' "
        End If
        strSql += "  AND ACTYPE IN (" & Actype & ")"
        strSql += " ORDER BY RESULT,ACNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtAcname)
        CmbAcname.Items.Clear()
        BrighttechPack.FillCombo(CmbAcname, DtAcname, "ACNAME", True)
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

    Private Sub chkLstMetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.Leave
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE ISNULL(LEDGERPRINT,'') <> 'N' AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " ORDER BY CATNAME"
            FillCheckedListBox(strSql, chkLstCategory)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsOnDate.Value = GetServerDate()
        'cmbTranType.Text = "ALL"
        cmbactive.Items.Clear()
        cmbactive.Items.Add("BOTH VIEW")
        cmbactive.Items.Add("ACTIVE ONLY")
        cmbactive.Items.Add("IN ACTIVE ONLY")
        cmbactive.SelectedIndex = 0
        FillAcname()
        Prop_Gets()
        If RPT_SMITHABS_HIDENETWT Then
            ChkNetWt.Visible = False
            ChkNetWt.Checked = False
        End If
        chkWithAbs.Select()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSmithBalanceSummaryReport_Properties
        obj.p_dtpAsOnDate = dtpAsOnDate.Value.Date
        obj.p_dtpToDate = dtpTodate.Value.Date
        GetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype)
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkDealer = chkDealer.Checked
        obj.p_chkSmith = chkSmith.Checked
        obj.p_chkOthers = chkOthers.Checked
        obj.p_chkCustomer = chkCustomer.Checked
        obj.p_rbtGrossWeight = ChkGrossWt.Checked
        obj.p_rbtNetWeight = ChkNetWt.Checked
        obj.p_rbtPureWeight = ChkPureWt.Checked
        obj.p_chkWithNillBalance = chkWithNillBalance.Checked
        obj.p_chkStnWtConv = chkStnWtConv.Checked
        obj.p_chkDiaWtConvert = chkDiaWtConvert.Checked
        obj.p_chkAmtBal = chkAmtbal.Checked
        obj.p_chkRelatedTran = ChkRelatedtran.Checked
        obj.p_chkSpecificFormat = chkWithSpecicFormat.Checked
        obj.p_chkLocal = rbtLocal.Checked
        obj.p_chkout = rbtOutstation.Checked
        obj.p_chkBoth = rbtBothMU.Checked
        obj.p_chkWithWast = ChkwithWast.Checked
        obj.p_chkWithAppBal = chkWithAppAmt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSmithBalanceSummaryReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))
        SetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype, "ALL")
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkDealer.Checked = obj.p_chkDealer
        chkSmith.Checked = obj.p_chkSmith
        chkOthers.Checked = obj.p_chkOthers
        chkCustomer.Checked = obj.p_chkCustomer
        ChkGrossWt.Checked = obj.p_rbtGrossWeight
        ChkNetWt.Checked = obj.p_rbtNetWeight
        ChkPureWt.Checked = obj.p_rbtPureWeight
        chkWithNillBalance.Checked = obj.p_chkWithNillBalance
        chkAmtbal.Checked = obj.p_chkAmtBal
        ChkRelatedtran.Checked = obj.p_chkRelatedTran
        chkWithSpecicFormat.Checked = obj.p_chkSpecificFormat
        ChkwithWast.Checked = obj.p_chkWithWast
        chkStnWtConv.Checked = obj.p_chkStnWtConv
        chkDiaWtConvert.Checked = obj.p_chkDiaWtConvert
        rbtLocal.Checked = obj.p_chkLocal
        rbtOutstation.Checked = obj.p_chkout
        rbtBothMU.Checked = obj.p_chkBoth
        chkWithAppAmt.Checked = obj.p_chkWithAppBal
    End Sub
    Private Sub chkWithAbs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWithAbs.CheckedChanged
        If chkWithAbs.Checked = True Then
            lblFrom.Text = "To date"
            chkWithAbs.Text = "From Date"
            dtpTodate.Visible = True
            lblFrom.Visible = True
            chkAmtbal.Checked = False
            chkAmtbal.Visible = False
            ChkWithDirectPur.Visible = False
            ChkWithDirectPur.Enabled = False
            ChkWithDirectPur.Checked = False
        Else
            lblFrom.Visible = False
            chkWithAbs.Text = "As On Date"
            dtpTodate.Visible = False
            If chkWithSpecicFormat.Checked Then
                chkAmtbal.Checked = False
                chkAmtbal.Visible = False
                ChkWithDirectPur.Visible = False
                ChkWithDirectPur.Enabled = False
                ChkWithDirectPur.Checked = False
            Else
                chkAmtbal.Checked = False
                chkAmtbal.Visible = True
                ChkWithDirectPur.Visible = True
                ChkWithDirectPur.Enabled = True
                ChkWithDirectPur.Checked = False
            End If
        End If
    End Sub

    Private Sub chkCmbTranType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbTranType.LostFocus
        If chkCmbTranType.Text = "ALL" Then
            ChkApproval.Enabled = True
            chkMIMRApproval.Enabled = True
            ChkApproval.Checked = True
            chkMIMRApproval.Checked = True
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
            chkMIMRApproval.Enabled = False
            chkMIMRApproval.Checked = True
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE, APPROVAL RECEIPT" Then
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
            chkMIMRApproval.Enabled = False
            chkMIMRApproval.Checked = True
        Else
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
            chkMIMRApproval.Enabled = False
            chkMIMRApproval.Checked = False
        End If
    End Sub

    Private Sub chkDealer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDealer.CheckedChanged
        FillAcname()
    End Sub
    Private Sub chkInternal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInternal.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkOthers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOthers.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSmith.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCustomer.CheckedChanged
        FillAcname()
        cmbtrantypeload()
    End Sub

    Private Sub chkAmtbal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAmtbal.CheckedChanged
        If chkAmtbal.Checked Then
            ChkRelatedtran.Visible = True
        Else
            ChkRelatedtran.Visible = False
        End If
    End Sub

    Private Sub cmbactive_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbactive.SelectedIndexChanged

        If cmbactive.Text = "BOTH VIEW" Then
            ActIVE = "B"
        ElseIf cmbactive.Text = "ACTIVE ONLY" Then
            ActIVE = "Y"
        Else
            ActIVE = "N"
        End If
        CmbAcname.Items.Clear()
        FillAcname()
    End Sub

    Private Sub chkWithSpecicFormat_CheckedChanged(sender As Object, e As EventArgs) Handles chkWithSpecicFormat.CheckedChanged
        If chkWithSpecicFormat.Checked Then
            ChkWithDirectPur.Visible = False
            ChkWithDirectPur.Enabled = False
            ChkWithDirectPur.Checked = False
        Else
            ChkWithDirectPur.Visible = True
            ChkWithDirectPur.Enabled = True
            ChkWithDirectPur.Checked = False
        End If
    End Sub
End Class

Public Class frmSmithBalanceSummaryReport_Properties
    Private dtpAsOnDate As Date = GetServerDate()
    Public Property p_dtpAsOnDate() As Date
        Get
            Return dtpAsOnDate
        End Get
        Set(ByVal value As Date)
            dtpAsOnDate = value
        End Set
    End Property
    Private dtpToDate As Date = GetServerDate()
    Public Property p_dtpToDate() As Date
        Get
            Return dtpToDate
        End Get
        Set(ByVal value As Date)
            dtpToDate = value
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
    Private chkWithNillBalance As Boolean = True
    Public Property p_chkWithNillBalance() As Boolean
        Get
            Return chkWithNillBalance
        End Get
        Set(ByVal value As Boolean)
            chkWithNillBalance = value
        End Set
    End Property
    Private chkAmtBal As Boolean = True
    Public Property p_chkAmtBal() As Boolean
        Get
            Return chkAmtBal
        End Get
        Set(ByVal value As Boolean)
            chkAmtBal = value
        End Set
    End Property
    Private chkRelatedTran As Boolean = True
    Public Property p_chkRelatedTran() As Boolean
        Get
            Return chkRelatedTran
        End Get
        Set(ByVal value As Boolean)
            chkRelatedTran = value
        End Set
    End Property
    Private chkSpecificFormat As Boolean = False
    Public Property p_chkSpecificFormat() As Boolean
        Get
            Return chkSpecificFormat
        End Get
        Set(ByVal value As Boolean)
            chkSpecificFormat = value
        End Set
    End Property
    Private chkLocal As Boolean = True
    Public Property p_chkLocal() As Boolean
        Get
            Return chkLocal
        End Get
        Set(ByVal value As Boolean)
            chkLocal = value
        End Set
    End Property
    Private chkout As Boolean = True
    Public Property p_chkout() As Boolean
        Get
            Return chkout
        End Get
        Set(ByVal value As Boolean)
            chkout = value
        End Set
    End Property
    Private chkBoth As Boolean = False
    Public Property p_chkBoth() As Boolean
        Get
            Return chkBoth
        End Get
        Set(ByVal value As Boolean)
            chkBoth = value
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
    Private chkStnWtConv As Boolean = True
    Public Property p_chkStnWtConv() As Boolean
        Get
            Return chkStnWtConv
        End Get
        Set(ByVal value As Boolean)
            chkStnWtConv = value
        End Set
    End Property

    Private chkDiaWtConvert As Boolean = True
    Public Property p_chkDiaWtConvert() As Boolean
        Get
            Return chkDiaWtConvert
        End Get
        Set(ByVal value As Boolean)
            chkDiaWtConvert = value
        End Set
    End Property

    Private chkWithAppBal As Boolean = False
    Public Property p_chkWithAppBal() As Boolean
        Get
            Return chkWithAppBal
        End Get
        Set(ByVal value As Boolean)
            chkWithAppBal = value
        End Set
    End Property
End Class