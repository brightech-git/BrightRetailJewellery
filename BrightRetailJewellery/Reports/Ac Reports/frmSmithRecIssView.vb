Imports System.Data.OleDb
Public Class frmSmithRecIssView
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtCostCentre As New DataTable
    Dim MRCANCEL_TAG As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MRCANCEL_TAG'", "CTLTEXT", "N")
    Dim MREDIT_WITHLOT As Boolean = IIf(GetAdmindbSoftValue("MREDIT_WITHLOT", "N") = "Y", True, False)
    Dim STKTRAN_PRINTNEW As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_PRINTNEW", "N") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
    Dim MIMR_RATEFIXED As Boolean = IIf(GetAdmindbSoftValue("MIMR_RATEFIXED", "N") = "Y", True, False)
    Dim MIMR_MARKED As Boolean = IIf(GetAdmindbSoftValue("MIMR_CHECKINVOICE", "N") = "Y", True, False)

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ChkApproval.Enabled = True
        If rbtReceipt.Checked = False Then rbtIssue.Checked = True : rbtIssue.Select()
        chkcmbTranType.Text = "ALL"
        chkcmbProcessType.Text = "ALL"
        Chktran.Enabled = True
    End Sub

    Private Sub LoadProcess(ByVal Cmb As ComboBox)
        strSql = vbCrLf + " SELECT 'ALL' ORDSTATE_NAME,0 ORDSTATE_ID UNION ALL SELECT ORDSTATE_NAME,ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS ORDER BY ORDSTATE_NAME"
        Dim dtMetal As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "ORDSTATE_NAME"
        Cmb.ValueMember = "ORDSTATE_ID"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmSmithRecIssView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmSmithRecIssView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        strSql = " SELECT LTRIM(ACNAME) FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(GRPLEDGER,'') NOT IN ('T','P'))"
        strSql += " ORDER BY LTRIM(ACNAME)"
        FillCheckedListBox(strSql, chkLstSmith)

        strSql = " SELECT 'ALL' ACNAME ,0 RESULT UNION ALL"
        strSql += " SELECT LTRIM(ACNAME),1 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(GRPLEDGER,'') NOT IN ('T','P'))"
        strSql += " ORDER BY RESULT,ACNAME"
        objGPack.FillCombo(strSql, CmbSmithname, True, False)
        chkRateCut.Enabled = True
        If MIMR_MARKED Then
            PnlCheckType.Visible = True
        Else
            PnlCheckType.Visible = False
            rbtIssRecBoth.Checked = True
        End If

        chkcmbProcessType.Items.Clear()
        strSql = vbCrLf + " SELECT 'ALL' ORDSTATE_NAME,0 RESULT UNION ALL SELECT ORDSTATE_NAME,1 RESULT FROM " & cnAdminDb & "..ORDERSTATUS ORDER BY RESULT,ORDSTATE_NAME"
        Dim dt1 As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            For i As Integer = 0 To dt1.Rows.Count - 1
                chkcmbProcessType.Items.Add(dt1.Rows(i).Item("ORDSTATE_NAME").ToString, False)
            Next
        End If
        chkcmbProcessType.Text = "ALL"

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

    Private Sub chkSmithSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSmith.CheckedChanged
        SetChecked_CheckedList(chkLstSmith, chkSmith.Checked)
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

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtIssue.Checked Then
            chkcmbTranType.Items.Clear()
            chkcmbTranType.Items.Add("ALL")
            chkcmbTranType.Items.Add("ISSUE")
            chkcmbTranType.Items.Add("MELTING ISSUE")
            chkcmbTranType.Items.Add("APPROVAL ISSUE")
            chkcmbTranType.Items.Add("OTHER ISSUE")
            chkcmbTranType.Items.Add("PURCHASE RETURN")
            chkcmbTranType.Items.Add("INTERNAL TRANSFER")
            chkcmbTranType.Items.Add("DELIVERY NOTE")
            chkcmbTranType.Text = "ALL"
            Chktran.Enabled = True
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked Then
            chkcmbTranType.Items.Clear()
            chkcmbTranType.Items.Add("ALL")
            chkcmbTranType.Items.Add("RECEIPT")
            chkcmbTranType.Items.Add("MELTING RECEIPT")
            chkcmbTranType.Items.Add("APPROVAL RECEIPT")
            chkcmbTranType.Items.Add("OTHER RECEIPT")
            chkcmbTranType.Items.Add("PURCHASE")
            chkcmbTranType.Items.Add("INTERNAL TRANSFER")
            chkcmbTranType.Items.Add("RECEIPT NOTE")
            chkcmbTranType.Text = "ALL"
            Chktran.Enabled = True
        End If
    End Sub


    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkTranType As String = GetChecked_CheckedList(chkcmbTranType, False)
        Dim chkSmithName As String
        If ChkMultiSmith.Checked Then
            chkSmithName = GetChecked_CheckedList(chkLstSmith)
        Else
            chkSmithName = IIf(CmbSmithname.Text.Trim <> "ALL" And CmbSmithname.Text.Trim <> "", "'" & CmbSmithname.Text & "'", "")
        End If
        Dim trantype As String = ""
        Dim Flag As String = ""
        Dim Apptrantype As String = ""

        If chkcmbTranType.Text <> "ALL" Then
            If rbtIssue.Checked Then
                If chkcmbTranType.Items.Count > 0 Then
                    For cnt As Integer = 0 To chkcmbTranType.CheckedItems.Count - 1
                        trantype = trantype + ",'I" & Mid(chkcmbTranType.CheckedItems.Item(cnt).ToString, 1, 2) & "'"
                        If chkcmbTranType.Text.ToUpper = "MELTING ISSUE" Then
                            trantype = ",'IIS'"
                        End If
                    Next
                    If chkcmbTranType.Text.ToUpper = "ISSUE" Then
                        Flag = "AND ISNULL(BAGNO,'') = ''"
                    ElseIf chkcmbTranType.Text.ToUpper = "MELTING ISSUE" Then
                        Flag = "AND ISNULL(BAGNO,'') <> ''"
                    Else
                        Flag = ""
                    End If
                End If
                'trantype = "I" & Mid(chkcmbTranType.Text, 1, 2)
            Else
                'trantype = "R" & Mid(chkcmbTranType.Text, 1, 2)
                If chkcmbTranType.Items.Count > 0 Then
                    For cnt As Integer = 0 To chkcmbTranType.CheckedItems.Count - 1
                        trantype = trantype + ",'R" & Mid(chkcmbTranType.CheckedItems.Item(cnt).ToString, 1, 2) & "'"
                        If chkcmbTranType.Text.ToUpper = "MELTING RECEIPT" Then
                            trantype = ",'RRE'"
                        End If
                    Next
                    If chkcmbTranType.Text.ToUpper = "RECEIPT" Then
                        'Flag = "AND ISNULL(MELT_RETAG,'') <> 'M'"
                        Flag = "AND ISNULL(BAGNO,'') = ''"
                    ElseIf chkcmbTranType.Text.ToUpper = "MELTING RECEIPT" Then
                        'Flag = "AND ISNULL(MELT_RETAG,'') = 'M'"
                        Flag = "AND ISNULL(BAGNO,'') <> ''"
                    Else
                        Flag = ""
                    End If
                End If
            End If
            trantype = Mid(trantype, 2, Len(trantype))
        Else
            If ChkApproval.Checked = False Then Apptrantype = "'IAP','RAP'"
        End If

        Dim AppProcessType As String = ""
        If chkcmbProcessType.Items.Count > 0 Then
            For cnt As Integer = 0 To chkcmbProcessType.CheckedItems.Count - 1
                AppProcessType = AppProcessType + ",'" & chkcmbProcessType.CheckedItems.Item(cnt).ToString & "'"
            Next
            AppProcessType = Mid(AppProcessType, 2, Len(AppProcessType))
        End If

        If chkRateCut.Checked = True Then trantype = "'IRC','RRC'"

        If rbtBoth.Checked Then funcReceiptIssue() : Exit Sub
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SMITHISSREC')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
        strSql += vbCrLf + "   SELECT  TRANNO,TDATE,BILLNO,BILLDATE,ACNAME,GSTNO,TRANTYPE,"
        If Chktran.Checked = False Then strSql += vbCrLf + "    DESCRIPTION,SUBITEM, "
        If ChkGrpCategory.Checked = False Then strSql += vbCrLf + "SEIVE,STNGRP,"
        strSql += vbCrLf + "  CATNAME,ISNULL(TAGNO,'')TAGNO ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT )NETWT,SUM(LESSWT)LESSWT,"
        strSql += vbCrLf + "  SUM(PUREWT)PUREWT,SUM(STNWT)STNWT,SUM(PREWT)PREWT,SUM(DIAWT)DIAWT"
        strSql += vbCrLf + "  ,SUM(DIAPCS)DIAPCS,STONEUNIT,SUM(ALLOY)ALLOY,CONVERT(NUMERIC(15,2),AVG(TOUCH))TOUCH"
        strSql += vbCrLf + "  ,SUM(WASTAGE)WASTAGE,SUM(MC)MC,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),SUM(TDS))TDS"
        strSql += vbCrLf + "  ,SUM(IGST)IGST"
        strSql += vbCrLf + "  ,SUM(CGST)CGST"
        strSql += vbCrLf + "  ,SUM(SGST)SGST"
        strSql += vbCrLf + "  ,SUM(TCS)TCS"
        strSql += vbCrLf + "  ,SUM(VAT)VAT"
        strSql += vbCrLf + "  ,TRANDATE, RESULT, COLHEAD,BATCHNO ,COSTID, Type, CONVERT(VARCHAR(6),CANCEL)CANCEL, TRANTYPE1, REFNO, REFDATE, CATCODE"
        strSql += vbCrLf + " ,SUM(X.AMOUNT-X.TDS+X.VAT+X.TCS) TOTAL"
        If Chktran.Checked = False Then strSql += vbCrLf + ",REMARK1 ,REMARK2,SNO "
        strSql += vbCrLf + "  ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=X.USERID) USERNAME" & IIf(MIMR_RATEFIXED, ",RATEFIXED [RATE FIX]", "")
        strSql += vbCrLf + "  ,ROW_NUMBER() OVER(PARTITION BY BATCHNO ORDER BY TRANDATE,TRANNO,TRANTYPE) UPDSNO "
        strSql += vbCrLf + IIf(MIMR_MARKED And rbtDetailed.Checked, ",ISNULL(SETTLEMENT,'') MARKED,SETTLEMENTDATE MARKEDDATE", "")
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
        strSql += vbCrLf + "  FROM ( "
        If rbtIssue.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  I.TRANNO,I.TRANDATE TDATE"
            strSql += vbCrLf + "  ,CASE WHEN I.TRANTYPE ='MI' THEN I.TRFNO ELSE I.REFNO END BILLNO,I.REFDATE BILLDATE"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
            strSql += vbCrLf + "  ,(SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) GSTNO"
            strSql += vbCrLf + "  ,CASE I.TRANTYPE WHEN 'IIS' THEN CASE FLAG WHEN 'M' THEN 'MELTING ISSUE' ELSE'ISSUE'END WHEN 'IAP' THEN 'APPROVAL ISSUE' WHEN 'IOT' THEN 'OTHER ISSUE' WHEN 'IPU' THEN 'PURCHASE RETURN' WHEN 'IIN' THEN 'INTERNAL TRANSFER'"
            strSql += vbCrLf + "   WHEN 'MI' THEN 'MISC ISSUE' WHEN 'IDN' THEN 'DELIVERY NOTE' ELSE TRANTYPE END AS TRANTYPE"
            'strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)"
            'strSql += vbCrLf + "  ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS [DESCRIPTION]"
            strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
            strSql += vbCrLf + "  ,CASE WHEN SUBITEMID = 0 THEN '' ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS [SUBITEM],SEIVE"
            strSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID) STNGRP"
            strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME"
            'strSql += vbCrLf + "  ,CONVERT(INT,CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END)PCS,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
            strSql += vbCrLf + "  ,CONVERT(INT,CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END)PCS,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END)GRSWT"
            'Convert(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN NETWT END)NETWT"
            strSql += vbCrLf + " , CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PUREWT END)PUREWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS STNWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) AS PREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS PREWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END,0) ) AS DIAWT "
            strSql += vbCrLf + " ,CONVERT(INT,ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END,0) ) AS DIAPCS "

            strSql += vbCrLf + " ,ISNULL(CASE WHEN (SELECT TOP 1  STONEUNIT FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))<>'' THEN"
            strSql += vbCrLf + "(SELECT TOP 1 STONEUNIT FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))"
            strSql += vbCrLf + "Else"
            strSql += vbCrLf + "(SELECT TOP 1  STONEUNIT FROM  " & cnStockDb & "..ISSUE WHERE  sno=i.sno And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T'))) END,'')"
            strSql += vbCrLf + "STONEUNIT"

            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,2),TOUCH)TOUCH,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN MCHARGE ELSE 0 END)MC,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN AMOUNT ELSE 0 END)AMOUNT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TDS)TDS"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='SG')SGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='CG')CGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='IG')IGST"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN TAX ELSE 0 END)VAT"
            strSql += vbCrLf + " ,(SELECT ISNULL(SUM(AMOUNT),0) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=I.BATCHNO And ACCODE='TCS') AS TCS"
            strSql += vbCrLf + " ,REMARK1,REMARK2,ISNULL(I.TAGNO,'')TAGNO"
            strSql += vbCrLf + " ,TRANDATE,1 RESULT,' ' COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'I')TYPE,CANCEL,TRANTYPE AS TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO,I.CATCODE"
            ' strSql += vbCrLf + "  INTO TEMP" & systemId & "SMITHISSREC"
            If MIMR_RATEFIXED Then strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.RATEFIXED,'')='Y' THEN 'Y' ELSE 'N' END RATEFIXED"
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",I.SETTLEMENT,I.SETTLEMENTDATE", "")
            strSql += vbCrLf + "  ,USERID FROM " & cnStockDb & "..ISSUE AS I"
        End If
        If rbtReceipt.Checked Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
            strSql += vbCrLf + "  ,REFNO BILLNO,REFDATE BILLDATE"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
            strSql += vbCrLf + "  ,(SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) GSTNO"
            strSql += vbCrLf + "  ,CASE TRANTYPE  WHEN 'RRE' THEN CASE FLAG WHEN 'M' THEN 'MELTING RECEIPT' ELSE'RECEIPT'END  WHEN 'RAP' THEN 'APPROVAL RECEIPT' WHEN 'ROT' THEN 'OTHER RECEIPT' WHEN 'RPU' THEN 'PURCHASE' WHEN 'RIN' THEN 'INTERNAL TRANSFER' "
            strSql += vbCrLf + "   WHEN 'RDN' THEN 'RECEIPT NOTE' ELSE TRANTYPE END AS TRANTYPE"
            'strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) WHEN SUBITEMID = 0 THEN (sELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)"
            'strSql += vbCrLf + "  ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS [DESCRIPTION]"
            strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
            strSql += vbCrLf + "  ,CASE WHEN SUBITEMID = 0 THEN '' ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS [SUBITEM],SEIVE"
            strSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID) STNGRP"
            strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME"
            strSql += vbCrLf + "  ,CONVERT(INT,CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END)PCS,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END)GRSWT"
            'Convert(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN NETWT END)NETWT"
            strSql += vbCrLf + " , CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PUREWT END)PUREWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(Select SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS STNWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) AS PREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS PREWT"
            'strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END,0) ) AS DIAWT "
            strSql += vbCrLf + " ,CONVERT(INT,ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END,0) ) AS DIAPCS "

            strSql += vbCrLf + " ,ISNULL(CASE WHEN (SELECT TOP 1 STONEUNIT FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))<>'' THEN"
            strSql += vbCrLf + "(SELECT TOP 1 STONEUNIT FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))"
            strSql += vbCrLf + "Else"
            strSql += vbCrLf + "(SELECT TOP 1  STONEUNIT FROM  " & cnStockDb & "..RECEIPT WHERE  sno=i.sno And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T'))) END,'')"
            strSql += vbCrLf + "STONEUNIT"

            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,2),TOUCH)TOUCH,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TDS)TDS"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='SG')SGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='CG')CGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='IG')IGST"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TAX)VAT"
            strSql += vbCrLf + " ,(SELECT ISNULL(SUM(AMOUNT),0) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=I.BATCHNO And ACCODE='TCS') AS TCS"
            strSql += vbCrLf + " ,REMARK1,REMARK2,ISNULL(I.TAGNO,'')TAGNO"
            strSql += vbCrLf + "  ,TRANDATE,1 RESULT,' 'COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'R')TYPE,CANCEL,TRANTYPE TRANTYPE1,REFNO,REFDATE,"
            strSql += vbCrLf + "  CONVERT(VARCHAR(20),SNO) SNO,"
            strSql += vbCrLf + "  I.CATCODE"
            If MIMR_RATEFIXED Then strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.RATEFIXED,'')='Y' THEN 'Y' ELSE 'N' END RATEFIXED"
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",SETTLEMENT,SETTLEMENTDATE", "")
            strSql += vbCrLf + "  ,USERID FROM " & cnStockDb & "..RECEIPT AS I"
        End If
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCancel.Checked = False Then strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND (LEN(TRANTYPE) > 2 OR (TRANTYPE = 'MI' AND (ISNULL(ACCODE,'')<> '' OR ISNULL(TRFNO,'')<> '')))"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If chkSmithName <> "" Then
            strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LTRIM(ACNAME) IN (" & chkSmithName & ")"
            If rbtSmith.Checked Then
                strSql += " AND ACTYPE = 'G')"
            ElseIf rbtDealer.Checked Then
                strSql += " AND ACTYPE = 'D' )"
            ElseIf rbtOthers.Checked Then
                strSql += " AND ACTYPE NOT IN ('G','D'))"
            Else
                strSql += ")"
            End If
        End If
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ") " + Flag
        If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
        If AppProcessType <> "ALL" And AppProcessType <> "" And Not AppProcessType.Contains("ALL") Then
            strSql += vbCrLf + " AND I.ORDSTATE_ID IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME IN (" & AppProcessType.ToString & "))"
        End If
        If rbtIssue.Checked And SPECIFICFORMAT.ToString = "1" Then
            strSql += vbCrLf + " AND TRANTYPE NOT IN('MI')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANNO,TDATE,BILLNO,BILLDATE,ACNAME,GSTNO,TRANTYPE,[DESCRIPTION],SUBITEM,SEIVE,STNGRP,CATNAME "
            strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT,SUM(PUREWT)PUREWT,SUM(STNWT)STNWT,SUM(PREWT)PREWT "
            strSql += vbCrLf + " ,SUM(DIAWT)DIAWT,SUM(DIAPCS)DIAPCS,STONEUNIT,SUM(ALLOY)ALLOY,TOUCH,SUM(WASTAGE)WASTAGE,SUM(MC)MC,SUM(AMOUNT)AMOUNT "
            strSql += vbCrLf + " ,SUM(TDS)TDS,SUM(SGST)SGST,SUM(CGST)CGST,SUM(IGST)IGST,SUM(TCS)TCS,SUM(VAT)VAT"
            strSql += vbCrLf + " ,REMARK1,REMARK2,TAGNO,TRANDATE,RESULT,COLHEAD,BATCHNO,COSTID,[TYPE],CANCEL,TRANTYPE1,REFNO,REFDATE,'' SNO,CATCODE,USERID"
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",SETTLEMENT,SETTLEMENTDATE", "")
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  I.TRANNO,I.TRANDATE TDATE"
            strSql += vbCrLf + "  ,CASE WHEN I.TRANTYPE ='MI' THEN I.TRFNO ELSE I.REFNO END BILLNO,I.REFDATE BILLDATE"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
            strSql += vbCrLf + "  ,(SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) GSTNO"
            strSql += vbCrLf + "  ,'MISC ISSUE' AS TRANTYPE"
            strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS [DESCRIPTION]"
            strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)  AS [SUBITEM],SEIVE"
            strSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID) STNGRP"
            strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME"
            strSql += vbCrLf + "  ,CONVERT(INT,CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END)PCS,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END)GRSWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN NETWT END)NETWT"
            strSql += vbCrLf + " , CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),CASE WHEN ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PUREWT END)PUREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')),0)) "
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " AS PREWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + CONVERT(NUMERIC(15,3),ISNULL((SELECT SUM(STNWT) FROM (SELECT CASE WHEN ISNULL(STNITEMID,0) = 0 THEN SUM(STNWT) ELSE 0 END STNWT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND CATCODE IN (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') GROUP BY STNITEMID)X),0))"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN GRSWT END,0) ) AS DIAWT "
            strSql += vbCrLf + " ,CONVERT(INT,ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
            strSql += vbCrLf + " + "
            strSql += vbCrLf + " ISNULL(CASE WHEN ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') THEN PCS END,0) ) AS DIAPCS "

            strSql += vbCrLf + " ,ISNULL(CASE WHEN (SELECT TOP 1  STONEUNIT FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))<>'' THEN"
            strSql += vbCrLf + "(SELECT TOP 1 STONEUNIT FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T')))"
            strSql += vbCrLf + "Else"
            strSql += vbCrLf + "(SELECT TOP 1  STONEUNIT FROM  " & cnStockDb & "..ISSUE WHERE  sno=i.sno And CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('D','T'))) END,'')"
            strSql += vbCrLf + "STONEUNIT"

            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,2),TOUCH)TOUCH,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN MCHARGE ELSE 0 END)MC,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN AMOUNT ELSE 0 END)AMOUNT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TDS)TDS"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='SG')SGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='CG')CGST"
            strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='IG')IGST"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN TAX ELSE 0 END)VAT,0 AS TCS"
            strSql += vbCrLf + " ,REMARK1,REMARK2,ISNULL(I.TAGNO,'')TAGNO"
            strSql += vbCrLf + " ,TRANDATE,1 RESULT,' ' COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'I')TYPE,CANCEL,TRANTYPE AS TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO,I.CATCODE"
            If MIMR_RATEFIXED Then strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.RATEFIXED,'')='Y' THEN 'Y' ELSE 'N' END RATEFIXED"
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",I.SETTLEMENT,I.SETTLEMENTDATE", "")
            strSql += vbCrLf + "  ,USERID FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If chkCancel.Checked = False Then strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  AND (LEN(TRANTYPE) > 2 OR (TRANTYPE = 'MI' AND (ISNULL(ACCODE,'')<> '' OR ISNULL(TRFNO,'')<> '')))"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If chkSmithName <> "" Then
                strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LTRIM(ACNAME) IN (" & chkSmithName & ")"
                If rbtSmith.Checked Then
                    strSql += " AND ACTYPE = 'G')"
                ElseIf rbtDealer.Checked Then
                    strSql += " AND ACTYPE = 'D' )"
                ElseIf rbtOthers.Checked Then
                    strSql += " AND ACTYPE NOT IN ('G','D'))"
                Else
                    strSql += ")"
                End If
            End If
            If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ") " + Flag
            If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
            If AppProcessType <> "ALL" And AppProcessType <> "" And Not AppProcessType.Contains("ALL") Then
                strSql += vbCrLf + " AND I.ORDSTATE_ID IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME IN (" & AppProcessType.ToString & "))"
            End If
            strSql += vbCrLf + " AND TRANTYPE IN('MI')"
            strSql += vbCrLf + " )Y GROUP BY TRANNO,TDATE,BILLNO,BILLDATE,ACNAME,GSTNO,TRANTYPE,[DESCRIPTION],SUBITEM,SEIVE,STNGRP,CATNAME,TOUCH "
            strSql += vbCrLf + " ,REMARK1,REMARK2,TRANDATE,RESULT,COLHEAD,BATCHNO,COSTID,[TYPE],CANCEL,TRANTYPE1,REFNO,REFDATE,STONEUNIT,CATCODE,USERID,TAGNO"
            If MIMR_RATEFIXED Then strSql += vbCrLf + " ,RATEFIXED"
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",SETTLEMENT,SETTLEMENTDATE", "")
        End If
        strSql += vbCrLf + " )X  GROUP BY TRANNO,TDATE ,BILLNO ,BILLDATE,ACNAME,GSTNO,TRANTYPE,X.AMOUNT,X.TDS,X.VAT,X.TCS,TAGNO,"
        If Chktran.Checked = False Then strSql += vbCrLf + "DESCRIPTION,SUBITEM,"
        If ChkGrpCategory.Checked = False Then strSql += vbCrLf + "SEIVE,STNGRP,"
        strSql += vbCrLf + "CATNAME ,TRANDATE ,RESULT ,COLHEAD ,BATCHNO,COSTID ,TYPE ,CANCEL ,TRANTYPE1,REFNO,REFDATE,CATCODE,STONEUNIT "
        If Chktran.Checked = False Then strSql += vbCrLf + ",REMARK1 ,REMARK2,SNO "
        strSql += vbCrLf + ",USERID"
        If MIMR_RATEFIXED Then strSql += vbCrLf + " ,RATEFIXED"
        strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",SETTLEMENT,SETTLEMENTDATE", "")
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()




        strSql = vbCrLf + " UPDATE A  SET TRANTYPE = 'MELTING ISSUE' FROM  TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC AS A, "
        strSql += vbCrLf + " " & cnStockDb & "..MELTINGDETAIL AS B "
        strSql += vbCrLf + " WHERE A.BATCHNO = B.BATCHNO"
        strSql += vbCrLf + " And A.TRANTYPE1 = 'IIS' AND B.TRANTYPE = 'MI'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " update A  set TRANTYPE = 'MELTING RECEIPT' from  TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC as a, "
        strSql += vbCrLf + " " & cnStockDb & "..MELTINGDETAIL as b "
        strSql += vbCrLf + " where a.BATCHNO = b.BATCHNO"
        strSql += vbCrLf + " AND A.TRANTYPE1 = 'RRE' AND B.TRANTYPE = 'MR'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''''''''FOR TCS SINGLE ROW UPDATE [19-04-2021]''''''''
        strSql = vbCrLf + " UPDATE A  SET TCS=0 FROM  TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC AS A "
        strSql += vbCrLf + " WHERE A.UPDSNO <> 1 AND ISNULL(A.TCS,0)<>0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE A  SET TOTAL=(A.AMOUNT-A.TDS+A.VAT+A.TCS) FROM  TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC AS A "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC DROP COLUMN UPDSNO "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If Chktran.Checked Then
            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SMITHISSREC_RES')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC_RES"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC_RES FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SMITHISSREC')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "   SELECT  TRANNO,TDATE,BILLNO,BILLDATE,ACNAME,GSTNO,TRANTYPE,"
            If ChkGrpCategory.Checked = False Then strSql += vbCrLf + "SEIVE,STNGRP,"
            strSql += vbCrLf + "  CATNAME ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT )NETWT,SUM(LESSWT)LESSWT,"
            strSql += vbCrLf + "  SUM(PUREWT)PUREWT,SUM(STNWT)STNWT,SUM(PREWT)PREWT,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + "  ,SUM(DIAPCS)DIAPCS,STONEUNIT,SUM(ALLOY)ALLOY,CONVERT(NUMERIC(15,2),AVG(TOUCH))TOUCH"
            strSql += vbCrLf + "  ,SUM(WASTAGE)WASTAGE,SUM(MC)MC,SUM(AMOUNT)AMOUNT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),SUM(TDS))TDS"
            strSql += vbCrLf + "  ,SUM(IGST)IGST"
            strSql += vbCrLf + "  ,SUM(CGST)CGST"
            strSql += vbCrLf + "  ,SUM(SGST)SGST"
            strSql += vbCrLf + "  ,SUM(TCS)TCS"
            strSql += vbCrLf + "  ,SUM(VAT)VAT"
            strSql += vbCrLf + "  ,TRANDATE, RESULT, COLHEAD,BATCHNO ,COSTID, Type, CONVERT(VARCHAR(6),CANCEL)CANCEL, TRANTYPE1, REFNO, REFDATE, CATCODE"
            strSql += vbCrLf + "  ,SUM(X.AMOUNT-X.TDS+X.VAT+X.TCS) TOTAL"
            strSql += vbCrLf + "  , USERNAME" & IIf(MIMR_RATEFIXED, ",[RATE FIX]", "")
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
            strSql += vbCrLf + "  FROM ( "
            strSql += vbCrLf + "  SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC_RES"
            strSql += vbCrLf + "  ) X"
            strSql += vbCrLf + " GROUP BY TRANNO,TDATE ,BILLNO ,BILLDATE,ACNAME,GSTNO,TRANTYPE,"
            If ChkGrpCategory.Checked = False Then strSql += vbCrLf + "SEIVE,STNGRP,"
            strSql += vbCrLf + "CATNAME ,TRANDATE ,RESULT ,COLHEAD ,BATCHNO,COSTID ,TYPE ,CANCEL ,TRANTYPE1,REFNO,REFDATE,CATCODE,STONEUNIT "
            strSql += vbCrLf + ",USERNAME" & IIf(MIMR_RATEFIXED, ",[RATE FIX]", "")
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        ''''''''END FOR TCS SINGLE ROW UPDATE [19-04-2021]''''''''

        strSql = vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
        strSql += vbCrLf + " ,STNWT=T.GRSWT,DIAWT=NULL,DIAPCS=NULL,PREWT=NULL FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC T "
        strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T' AND DIASTONE='S'))"
        'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
        'strSql += vbCrLf + " ,STNWT=NULL,DIAWT=T.GRSWT,DIAPCS=NULL,PREWT=NULL FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC T "
        'strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D')"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
        strSql += vbCrLf + " ,STNWT=NULL,DIAWT=NULL,DIAPCS=NULL,PREWT=T.GRSWT FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC T "
        strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T' AND DIASTONE='P'))"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC SET CANCEL='Cancel' WHERE CANCEL='Y'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC)>0 "
        strSql += vbCrLf + "  BEGIN"
        strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC(ACNAME,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS"
        If Not Chktran.Checked Then
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",MARKED,MARKEDDATE", "")
            strSql += vbCrLf & ",TAGNO"
        End If
        strSql += vbCrLf + "  ,STONEUNIT,PREWT,ALLOY,TOUCH,WASTAGE,MC,AMOUNT,VAT,TDS,IGST,CGST,SGST,TCS,TOTAL,RESULT,COLHEAD" & IIf(MIMR_RATEFIXED, ",[RATE FIX]", "") & ")"
        strSql += vbCrLf + "  SELECT 'GRAND TOTAL'TDATE,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(LESSWT),SUM(PUREWT),SUM(STNWT),SUM(DIAWT),SUM(DIAPCS)"
        If Not Chktran.Checked Then
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",'' MARKED,NULL MARKEDDATE", "")
            strSql += vbCrLf & ",'' TAGNO"
        End If
        strSql += vbCrLf + "  ,''STONEUNIT,SUM(PREWT),SUM(ALLOY),AVG(TOUCH),SUM(WASTAGE),SUM(MC),SUM(AMOUNT) "
        strSql += vbCrLf + "  ,SUM(VAT),SUM(TDS),SUM(IGST),SUM(CGST),SUM(SGST),SUM(TCS),SUM(TOTAL)"
        strSql += vbCrLf + "  ,3 RESULT,'G'COLHEAD" & IIf(MIMR_RATEFIXED, ",''", "")
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
        strSql += vbCrLf + "  WHERE ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkGrpCategory.Checked Then
            strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC)>0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC(ACNAME,CATNAME,PCS,GRSWT,NETWT"
            strSql += vbCrLf + "  ,LESSWT,PUREWT,DIAWT,DIAPCS,ALLOY,TOUCH,WASTAGE,MC,AMOUNT,VAT,RESULT,COLHEAD,STONEUNIT" & IIf(Not Chktran.Checked, ",TAGNO", "") & IIf(MIMR_RATEFIXED, ",[RATE FIX]", "")
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",MARKED,MARKEDDATE", "")
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  SELECT DISTINCT CATNAME,CATNAME,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL"
            strSql += vbCrLf + "  ,0 RESULT,'T'COLHEAD, '' STONEUNIT " & IIf(Not Chktran.Checked, ",'' TAGNO", "") & IIf(MIMR_RATEFIXED, ",''", "")
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",'' MARKED,NULL MARKEDDATE", "")
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC(ACNAME,CATNAME,PCS,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += vbCrLf + "  ,STNWT,DIAWT,DIAPCS,PREWT,ALLOY,TOUCH,WASTAGE,MC,AMOUNT,VAT,TDS,IGST,CGST,SGST,TCS,RESULT,COLHEAD,STONEUNIT" & IIf(Not Chktran.Checked, ",TAGNO", "") & IIf(MIMR_RATEFIXED, ",[RATE FIX]", "")
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",MARKED,MARKEDDATE", "")
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  SELECT DISTINCT CATNAME + ' SUBTOTAL',CATNAME,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(LESSWT),SUM(PUREWT),SUM(STNWT)"
            strSql += vbCrLf + "  ,SUM(DIAWT),SUM(DIAPCS),SUM(PREWT),SUM(ALLOY),AVG(TOUCH),SUM(WASTAGE),SUM(MC),SUM(AMOUNT)"
            strSql += vbCrLf + "  ,SUM(VAT)"
            strSql += vbCrLf + "  ,SUM(TDS),SUM(IGST),SUM(CGST),SUM(SGST),SUM(TCS)"
            strSql += vbCrLf + "  ,2 RESULT,'S'COLHEAD,'' STONEUNIT " & IIf(Not Chktran.Checked, ",'' TAGNO", "") & IIf(MIMR_RATEFIXED, ",''", "")
            strSql += vbCrLf & IIf(MIMR_MARKED And rbtDetailed.Checked, ",'' MARKED,NULL MARKEDDATE", "")
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC WHERE RESULT=1 "
            strSql += vbCrLf + "  GROUP BY CATNAME"
            strSql += vbCrLf + "  END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If rbtsummary.Checked = True Then
            strSql = " SELECT ACNAME"
            If ChkGrpCategory.Checked Then strSql += vbCrLf + " ,CATNAME"
            strSql += vbCrLf + "  ,SUM(PCS) PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,SUM(LESSWT)LESSWT,SUM(STNWT)STNWT"
            strSql += vbCrLf + "  ,SUM(DIAWT)DIAWT,SUM(DIAPCS)DIAPCS,SUM(PREWT)PREWT,SUM(ALLOY)ALLOY,AVG(TOUCH)TOUCH"
            strSql += vbCrLf + "  ,SUM(WASTAGE) WASTAGE,SUM(MC) MC,SUM(AMOUNT) AMOUNT,RESULT,COLHEAD "
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC "
            strSql += vbCrLf + "  GROUP BY ACNAME"
            If ChkGrpCategory.Checked Then strSql += vbCrLf + " ,CATNAME"
            strSql += vbCrLf + " ,RESULT,COLHEAD"
            strSql += vbCrLf + "  ORDER BY "
            If ChkGrpCategory.Checked Then strSql += vbCrLf + "  CATNAME DESC,"
            strSql += vbCrLf + "  RESULT"
        Else
            strSql = "  SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "SMITHISSREC "
            If MIMR_MARKED And rbtIssRecMarked.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(MARKED,'')='Y' "
            ElseIf MIMR_MARKED And rbtIssRecUnMarked.Checked Then
                strSql += vbCrLf + " WHERE ISNULL(MARKED,'')<>'Y' "
            End If
            strSql += vbCrLf + "  ORDER BY "
            If ChkGrpCategory.Checked Then strSql += vbCrLf + "  CATNAME DESC,"
            strSql += vbCrLf + "  RESULT"
            If rbtTranNo.Checked Then
                strSql += vbCrLf + " ,TRANNO"
            Else
                strSql += vbCrLf + " ,TRANDATE,TRANNO"
            End If
        End If

        Dim dtGrid As New DataTable
        ''If rbtDetailed.Checked And MIMR_MARKED And rbtBoth.Checked = False Then
        ''    dtGrid.Columns.Add("CHECK", GetType(Boolean))
        ''End If
        dtGrid.Columns.Add("KEYNO", GetType(String))
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

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "SMITH ISSUE RECEIPT VIEW"
        Dim tit As String = "SMITH "
        If rbtIssue.Checked Then tit += "ISSUE" Else tit += "RECEIPT"
        tit += " VIEW FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkTranType <> "" And chkTranType <> "ALL" Then tit += vbNewLine & " TYPE :" & Replace(chkTranType, "'", "")
        If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        If rbtDetailed.Checked Then
            If MIMR_MARKED And rbtIssRecBoth.Checked Then
                objGridShower.lblStatus.Text = "<Press [C] for Cancel> <Press [D] for Duplicate Print> <Press [E] for Edit> <Press [U] for Edit PCS Only> <Press [M] for Mark/Unmark Invoice>"
            Else
                objGridShower.lblStatus.Text = "<Press [C] for Cancel> <Press [D] for Duplicate Print> <Press [E] for Edit> <Press [U] for Edit PCS Only>"
            End If
        Else
            objGridShower.lblStatus.Text = ""
        End If
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, True, , False)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.gridView.Columns("TOUCH").DefaultCellStyle.Format = "0.00"
        If objGridShower.gridView.Columns.Contains("WASTAGE") Then
            objGridShower.gridView.Columns("WASTAGE").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("MC") Then
            objGridShower.gridView.Columns("MC").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("AMOUNT") Then
            objGridShower.gridView.Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("TDS") Then
            objGridShower.gridView.Columns("TDS").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("IGST") Then
            objGridShower.gridView.Columns("IGST").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("CGST") Then
            objGridShower.gridView.Columns("CGST").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("SGST") Then
            objGridShower.gridView.Columns("SGST").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("TCS") Then
            objGridShower.gridView.Columns("TCS").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("GST") Then
            objGridShower.gridView.Columns("GST").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("VAT") Then
            objGridShower.gridView.Columns("VAT").DefaultCellStyle.Format = "0.00"
        End If
        If objGridShower.gridView.Columns.Contains("TOTAL") Then
            objGridShower.gridView.Columns("TOTAL").DefaultCellStyle.Format = "0.00"
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        If objGridShower.gridView.Columns.Contains("MARKED") And rbtIssRecBoth.Checked Then
            For cnt As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView.Rows(cnt).Cells("MARKED").Value.ToString = "Y" Then
                    objGridShower.gridView.Rows(cnt).DefaultCellStyle.BackColor = Color.Lavender
                End If
            Next
        End If
    End Sub
    Private Sub funcReceiptIssue()
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkSmithName As String = GetChecked_CheckedList(chkLstSmith)
        Dim trantype As String = ""
        Dim Apptrantype As String = ""
        Dim temptableRec As String = "TEMPTABLEDB..TEMP" & systemId & "SMITHREC"
        Dim temptableIss As String = "TEMPTABLEDB..TEMP" & systemId & "SMITHISS"
        If chkcmbTranType.Text <> "ALL" Then
            If rbtIssue.Checked Then
                If chkcmbTranType.Items.Count > 0 Then
                    For cnt As Integer = 0 To chkcmbTranType.CheckedItems.Count - 1
                        trantype = trantype + ",'I" & Mid(chkcmbTranType.CheckedItems.Item(cnt).ToString, 1, 2) & "'"
                    Next
                End If
                'trantype = "I" & Mid(chkcmbTranType.Text, 1, 2)
            Else
                'trantype = "R" & Mid(chkcmbTranType.Text, 1, 2)
                If chkcmbTranType.Items.Count > 0 Then
                    For cnt As Integer = 0 To chkcmbTranType.CheckedItems.Count - 1
                        trantype = trantype + ",'R" & Mid(chkcmbTranType.CheckedItems.Item(cnt).ToString, 1, 2) & "'"
                    Next
                End If
            End If
            trantype = Mid(trantype, 2, Len(trantype))
        Else
            If ChkApproval.Checked = False Then Apptrantype = "'IAP','RAP'"
        End If

        Dim AppProcessType As String = ""
        If chkcmbProcessType.Items.Count > 0 Then
            For cnt As Integer = 0 To chkcmbProcessType.CheckedItems.Count - 1
                AppProcessType = AppProcessType + ",'" & chkcmbProcessType.CheckedItems.Item(cnt).ToString & "'"
            Next
            AppProcessType = Mid(AppProcessType, 2, Len(AppProcessType))
        End If

        If chkRateCut.Checked Then trantype = "'IRC','RRC'"

        strSql = vbCrLf + "  IF OBJECT_ID('" & temptableRec & "','U')IS NOT NULL DROP TABLE " & temptableRec & ""
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  ' '+(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) PARTICULARS"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=I.METALID)METAL"
        strSql += vbCrLf + "  ,(SELECT CASE WHEN CATGROUP='B' THEN 'BAR' WHEN CATGROUP='L' THEN 'OLD' WHEN CATGROUP='R' THEN 'REPAIR'"
        strSql += vbCrLf + "  WHEN CATGROUP='O' THEN 'ORDER' WHEN CATGROUP='P' THEN 'ORNAMENTS' ELSE 'PARTYMETALS' END"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE =I.CATCODE)CATEGORY"
        strSql += vbCrLf + "  ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
        strSql += vbCrLf + "  ,REFNO BILLNO,REFDATE BILLDATE"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
        strSql += vbCrLf + "  ,(SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) GSTNO"
        strSql += vbCrLf + "  ,CASE TRANTYPE  WHEN 'RRE' THEN 'RECEIPT' WHEN 'RAP' THEN 'APPROVAL RECEIPT' WHEN 'ROT' THEN 'OTHER RECEIPT' WHEN 'RPU' THEN 'PURCHASE' WHEN 'RIN' THEN 'INTERNAL TRANSFER' "
        strSql += vbCrLf + "   WHEN 'RDN' THEN 'RECEIPT NOTE' ELSE TRANTYPE END AS TRANTYPE"
        strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        strSql += vbCrLf + "  ,CASE WHEN SUBITEMID = 0 THEN '' ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS SUBITEM,SEIVE"
        strSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID) STNGRP"
        strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,CONVERT(INT,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TAX )VAT"
        strSql += vbCrLf + " ,REMARK1,REMARK2"
        strSql += vbCrLf + "  ,TRANDATE,4 RESULT,'  'COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'R')TYPE,CANCEL,TRANTYPE TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO"
        strSql += vbCrLf + "  ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=I.USERID)USERNAME"
        strSql += vbCrLf + "  INTO " & temptableRec & ""
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCancel.Checked = False Then strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND LEN(TRANTYPE) > 2"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If chkSmithName <> "" Then strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LTRIM(ACNAME) IN (" & chkSmithName & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
        If AppProcessType <> "ALL" And AppProcessType <> "" And Not AppProcessType.Contains("ALL") Then
            strSql += vbCrLf + " AND I.ORDSTATE_ID IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME IN (" & AppProcessType.ToString & "))"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM " & temptableRec & ")>0"
        strSql += vbCrLf + " BEGIN"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(DESCRIPTION,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT '          RECEIPT',0,'H'"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,1,'T' FROM " & temptableRec & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ' ' + CATEGORY,CATEGORY,METAL,2,'T1' FROM " & temptableRec & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL',CATEGORY,METAL,5,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' GROUP BY CATEGORY,METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,'ZZZZZ',6,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' GROUP BY METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'GRANDTOTAL','ZZZZZ','ZZZZZ',7,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET PCS=NULL WHERE ISNULL(PCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET GRSWT=NULL WHERE ISNULL(GRSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET NETWT=NULL WHERE ISNULL(NETWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET LESSWT=NULL WHERE ISNULL(LESSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET PUREWT=NULL WHERE ISNULL(PUREWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET STNWT=NULL WHERE ISNULL(STNWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET DIAWT=NULL WHERE ISNULL(DIAWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET DIAPCS=NULL WHERE ISNULL(DIAPCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET ALLOY=NULL WHERE ISNULL(ALLOY,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET WASTAGE=NULL WHERE ISNULL(WASTAGE,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET MC=NULL WHERE ISNULL(MC,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET AMOUNT=NULL WHERE ISNULL(AMOUNT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET VAT=NULL WHERE ISNULL(VAT,0)=0 "
        strSql += vbCrLf + "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  IF OBJECT_ID('" & temptableIss & "','U')IS NOT NULL DROP TABLE " & temptableIss & ""
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  ' '+(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) PARTICULARS"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=I.METALID)METAL"
        strSql += vbCrLf + "  ,(SELECT CASE WHEN CATGROUP='B' THEN 'BAR' WHEN CATGROUP='L' THEN 'OLD' WHEN CATGROUP='R' THEN 'REPAIR'"
        strSql += vbCrLf + "  WHEN CATGROUP='O' THEN 'ORDER' WHEN CATGROUP='P' THEN 'ORNAMENTS' ELSE 'PARTYMETALS' END"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE =I.CATCODE)CATEGORY"
        strSql += vbCrLf + "  ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
        strSql += vbCrLf + "  ,REFNO BILLNO"
        strSql += vbCrLf + "  ,REFDATE BILLDATE"
        'strSql += vbCrLf + "  ,CONVERT(VARCHAR,REFDATE,103)BILLDATE"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
        strSql += vbCrLf + "  ,(SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) GSTNO"
        strSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'IIS' THEN 'ISSUE' WHEN 'IAP' THEN 'APPROVAL ISSUE' WHEN 'IOT' THEN 'OTHER ISSUE' WHEN 'IPU' THEN 'PURCHASE RETURN' WHEN 'IIN' THEN 'INTERNAL TRANSFER'"
        strSql += vbCrLf + "   ELSE TRANTYPE END AS TRANTYPE"
        strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        strSql += vbCrLf + "  ,CASE WHEN SUBITEMID = 0 THEN '' ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) END AS SUBITEM,SEIVE"
        strSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID) STNGRP"
        strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,CONVERT(INT,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT,CONVERT(NUMERIC(15,2),TAX )VAT,REMARK1,REMARK2"
        strSql += vbCrLf + "  ,TRANDATE,14 RESULT,'  ' COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'I')TYPE,CANCEL,TRANTYPE AS TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO"
        strSql += vbCrLf + "  ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=I.USERID)USERNAME"
        strSql += vbCrLf + "  INTO " & temptableIss & ""
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCancel.Checked = False Then strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND LEN(TRANTYPE) > 2"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If chkSmithName <> "" Then strSql += vbCrLf + " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LTRIM(ACNAME) IN (" & chkSmithName & "))"
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
        If AppProcessType <> "ALL" And AppProcessType <> "" And Not AppProcessType.Contains("ALL") Then
            strSql += vbCrLf + " AND I.ORDSTATE_ID IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME IN (" & AppProcessType.ToString & "))"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM " & temptableIss & ")>0"
        strSql += vbCrLf + " BEGIN"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(DESCRIPTION,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT '          ISSUE',10,'H'"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,11,'T' FROM " & temptableIss & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ' ' + CATEGORY,CATEGORY,METAL,12,'T1' FROM " & temptableIss & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL',CATEGORY,METAL,15,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' GROUP BY CATEGORY,METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,'ZZZZZ',16,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' GROUP BY METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'GRANDTOTAL','ZZZZZ','ZZZZZ',17,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET PCS=NULL WHERE ISNULL(PCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET GRSWT=NULL WHERE ISNULL(GRSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET NETWT=NULL WHERE ISNULL(NETWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET LESSWT=NULL WHERE ISNULL(LESSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET PUREWT=NULL WHERE ISNULL(PUREWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET STNWT=NULL WHERE ISNULL(STNWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET DIAWT=NULL WHERE ISNULL(DIAWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET DIAPCS=NULL WHERE ISNULL(DIAPCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET ALLOY=NULL WHERE ISNULL(ALLOY,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET WASTAGE=NULL WHERE ISNULL(WASTAGE,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET MC=NULL WHERE ISNULL(MC,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET AMOUNT=NULL WHERE ISNULL(AMOUNT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET VAT=NULL WHERE ISNULL(VAT,0)=0 "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT PARTICULARS,TRANNO,BILLDATE,ACNAME,GSTNO,DESCRIPTION,SUBITEM,PCS"
        strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE,USERNAME"
        strSql += vbCrLf + "  FROM " & temptableRec & " ORDER BY METAL,CATEGORY,RESULT,TRANDATE"
        Dim dtGridRec As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridRec)
        strSql = vbCrLf + "  SELECT PARTICULARS,TRANNO,BILLDATE,ACNAME,GSTNO,DESCRIPTION,SUBITEM,PCS"
        strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,DIAPCS,ALLOY,WASTAGE,MC,AMOUNT,VAT"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE,USERNAME"
        strSql += vbCrLf + "  FROM " & temptableIss & " ORDER BY METAL,CATEGORY,RESULT,TRANDATE"
        Dim dtGridIss As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridIss)
        dtGridRec.Merge(dtGridIss)

        If Not dtGridRec.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.Text = "SMITH ISSUE RECEIPT VIEW"
        Dim tit As String = "SMITH RECEIPT AND ISSUE"
        tit += " VIEW FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGridRec)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.lblStatus.Text = ""
        With objGridShower.gridView
            .Columns("TRANDATE").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("PARTICULARS").Frozen = True
        End With
        FormatGridColumns(objGridShower.gridView, False, True, , False)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.AutoResize()
        Dim ind As Integer = 0
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString = "H" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.ForeColor = Color.Red
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T1" Then
                dgvRow.Cells(ind).Style.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T2" Then
                dgvRow.Cells(ind).Style.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "T3" Then
                dgvRow.Cells(ind).Style.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S1" Then
                dgvRow.DefaultCellStyle.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S2" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "G" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim GstFlag As Boolean = funcGstView(dtpFrom.Value)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .Columns("ACNAME").Width = 150
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("PUREWT").Width = 80
            .Columns("ALLOY").Width = 80
            .Columns("WASTAGE").Width = 80
            .Columns("MC").Width = 80
            .Columns("AMOUNT").Width = 100
            If rbtDetailed.Checked Then
                .Columns("TRANNO").Width = 60
                .Columns("TDATE").Width = 80
                .Columns("BILLNO").Width = 60
                .Columns("BILLDATE").Width = 80
                .Columns("TRANTYPE").Width = 100
                If Chktran.Checked = False Then
                    .Columns("DESCRIPTION").Width = 150
                End If
                .Columns("STNWT").Width = 80
                .Columns("DIAWT").Width = 80
                .Columns("DIAPCS").Width = 80
                .Columns("PREWT").Width = 80
                .Columns("VAT").Width = 100
                If GstFlag Then
                    .Columns("IGST").Visible = True
                    .Columns("CGST").Visible = True
                    .Columns("SGST").Visible = True
                    .Columns("VAT").HeaderText = "GST"
                Else
                    .Columns("VAT").HeaderText = "VAT"
                    .Columns("IGST").Visible = False
                    .Columns("CGST").Visible = False
                    .Columns("SGST").Visible = False
                End If
                .Columns("TDATE").HeaderText = "TRANDATE"
                If Chktran.Checked = False Then
                    .Columns("DESCRIPTION").HeaderText = "ITEMNAME"
                    If .Columns.Contains("TOTAL") Then .Columns("TOTAL").Width = 120
                    .Columns("REMARK1").Width = 120
                    .Columns("REMARK2").Width = 120
                End If
            ElseIf rbtsummary.Checked Then
                .Columns("ACNAME").HeaderText = "PARTICULARS"
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
            End If
            If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False

            For cnt As Integer = 27 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("TOTAL") Then .Columns("TOTAL").Visible = True
            If .Columns.Contains("REMARK1") Then .Columns("REMARK1").Visible = True
            If .Columns.Contains("REMARK2") Then .Columns("REMARK2").Visible = True
            If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").Visible = True
            If GstFlag Then
                If .Columns.Contains("IGST") Then .Columns("IGST").Visible = True
                If .Columns.Contains("CGST") Then .Columns("CGST").Visible = True
                If .Columns.Contains("SGST") Then .Columns("SGST").Visible = True
                If .Columns.Contains("TCS") Then .Columns("TCS").Visible = True
            End If
            If .Columns.Contains("VAT") Then .Columns("VAT").Visible = True
            If .Columns.Contains("MARKED") Then .Columns("MARKED").Visible = True
            If .Columns.Contains("MARKEDDATE") Then .Columns("MARKEDDATE").Visible = True
            If chkCancel.Checked Then
                If .Columns.Contains("CANCEL") Then .Columns("CANCEL").Visible = True
                If .Columns.Contains("CANCEL") Then .Columns("CANCEL").HeaderText = "STATUS"
            End If
            If .Columns.Contains("USERNAME") Then .Columns("USERNAME").Visible = True
            If .Columns.Contains("RATE FIX") And MIMR_RATEFIXED Then .Columns("RATE FIX").Visible = True
            If .Columns.Contains("TDS") Then .Columns("TDS").Visible = True
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub
    Private Function chkbackdateedit(ByVal ledgerdate As Date) As Boolean

        Dim serverDate As Date = GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
        If RestrictDays.Contains(",") = False Then
            If Not (ledgerdate >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                Return False
                Exit Function
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)

                    Return False
                    Exit Function
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                    Return False
                    Exit Function
                End If
            End If
        End If
        Return True
    End Function

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Chktran.Checked = False Then
            If UCase(e.KeyChar) = "C" Then
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
                If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Cancel.", MsgBoxStyle.Information) : Exit Sub
                Dim dgv As DataGridView = CType(sender, DataGridView)
                Dim dt As New DataTable
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                If MRCANCEL_TAG = "N" Then
                    ''Checking for Tag Issued or Not,if Tag issue from the Lot not able to Cancel
                    strSql = "SELECT CPCS,CGRSWT FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN "
                    strSql += vbCrLf + " (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO IN"
                    strSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "') "
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'')='')"
                    da = New OleDbDataAdapter(strSql, cn)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim CPcs As Integer = 0
                        Dim CGrsWt As Decimal = 0
                        CPcs = dt.Compute("SUM(CPCS)", Nothing)
                        CGrsWt = dt.Compute("SUM(CGRSWT)", Nothing)
                        If CGrsWt <> 0 Or CPcs <> 0 Then
                            MsgBox("Tag Generated for this Entry." & vbCrLf & "Not able to Cancel.", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    ElseIf MREDIT_WITHLOT Then
                        strSql = "UPDATE " & cnAdminDb & "..ITEMLOT SET CANCEL = 'Y' WHERE SNO IN "
                        strSql += vbCrLf + " (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO IN"
                        strSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "') "
                        strSql += vbCrLf + " AND ISNULL(CANCEL,'')='')"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran)
                    End If
                End If

                ''Checking for Purchase generated against this Entry ,if Entry available not able to Cancel
                strSql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='IAP' AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " AND REFNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "')"
                da = New OleDbDataAdapter(strSql, cn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If Val(dt.Rows(0).Item("CNT").ToString) > 0 Then
                        MsgBox("Purchase Entry Generated against this Entry" & vbCrLf & "Not able to Cancel.", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
                If dgv.CurrentRow.Cells("TRANTYPE").Value.ToString = "MI" Then MsgBox("Can't able to cancel POS Entry " & vbCrLf & " Use Bill view Option", MsgBoxStyle.Information) : Exit Sub
                If MessageBox.Show("Do you want to Cancel?", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
                Dim toCostId As String = dgv.CurrentRow.Cells("COSTID").Value.ToString
                Try
                    If dgv.Item("CANCEL", dgv.CurrentRow.Index).Value.ToString = "CANCEL" Then
                        MsgBox("ALREADY CANCELLED", MsgBoxStyle.Information)
                        dgv.Focus()
                        Exit Sub
                    End If
                    Dim objSecret As New frmAdminPassword()
                    If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                    Dim costId As String = dgv.Item("costid", dgv.CurrentRow.Index).Value.ToString
                    Dim objRemark As New frmBillRemark
                    objRemark.Text = "Cancel Remark"
                    If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                    'If MessageBox.Show("Do you want to cancel", "CANCEL", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.OK Then
                    '    Exit Sub
                    'End If
                    tran = Nothing
                    tran = cn.BeginTransaction
                    'strSql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL = 'Y' WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " UPDATE " & cnStockDb & "..RECEIPT SET CANCEL = 'Y' WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " UPDATE " & cnStockDb & "..ACCTRAN SET CANCEL = 'Y' WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'strSql += " UPDATE " & cnAdminDb & "..OUTSTANDING SET CANCEL = 'Y' WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, toCostId)

                    strSql = " INSERT INTO " & cnStockDb & "..CANCELLEDTRAN"
                    strSql += vbCrLf + "  (TRANDATE,BATCHNO,UPDATED,UPTIME,FLAG,REMARK1,REMARK2,USERID)"
                    strSql += vbCrLf + "  VALUES"
                    strSql += vbCrLf + "  ("
                    strSql += vbCrLf + "  '" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
                    strSql += vbCrLf + "  ,'" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    strSql += vbCrLf + "  ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                    strSql += vbCrLf + "  ,'" & GetServerTime(tran) & "'"
                    strSql += vbCrLf + "  ,'C'" 'FLAG CANCEL OR DELETE
                    strSql += vbCrLf + "  ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
                    strSql += vbCrLf + "  ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK2
                    strSql += vbCrLf + "  ," & userId & ""
                    strSql += vbCrLf + "  )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                    strSql = vbCrLf + " DELETE FROM " & cnStockDb & "..JOBNODETAILS WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                    If dgv.CurrentRow.Cells("TYPE").Value.ToString = "I" Then
                        strSql = "SELECT TRANTYPE,FLAG,BAGNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                        Dim dtIsschk As New DataTable
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtIsschk)
                        If dtIsschk.Rows(0).Item(1) = "X" And dtIsschk.Rows(0).Item(0) = "IIN" And dtIsschk.Rows(0).Item("BAGNO").ToString <> "" Then
                            strSql = vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET BAGNO='',MELT_RETAG='' WHERE BAGNO = '" & dtIsschk.Rows(0).Item("BAGNO").ToString & "'"
                            strSql += vbCrLf + " AND BATCHNO <> '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                        End If
                    End If

                    tran.Commit()
                    tran = Nothing
                    objGridShower.Close()
                    MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
                    btnSearch.Focus()
                    btnSearch_Click(Me, New EventArgs)
                    'dgv.Focus()
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                End Try
            ElseIf UCase(e.KeyChar) = "D" Then
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
                Dim dgv As DataGridView = CType(sender, DataGridView)
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                If dgv.CurrentRow.Cells("TRANTYPE").Value.ToString = "MI" Then MsgBox("Can't able to duplicate POS Entry " & vbCrLf & " Use Bill view Option", MsgBoxStyle.Information) : Exit Sub
                Dim toCostId As String = dgv.CurrentRow.Cells("COSTID").Value.ToString
                If GetAdmindbSoftValue("PRN_STKTRANSFER", "N") = "Y" Then
                    If dgv.CurrentRow.Cells("TRANTYPE1").Value.ToString = "IIN" And
                    dgv.CurrentRow.Cells("REFNO").Value.ToString <> "" Then
                        Dim RefNo As String = dgv.CurrentRow.Cells("REFNO").Value.ToString
                        'Dim RefDate As Date = dgv.CurrentRow.Cells("REFDATE").Value
                        Dim RefDate As Date = IIf(IsDBNull(dgv.CurrentRow.Cells("REFDATE").Value), dgv.CurrentRow.Cells("TRANDATE").Value, dgv.CurrentRow.Cells("REFDATE").Value)
                        If STKTRAN_PRINTNEW Then
                            PrintStockTransferNew(RefNo, RefDate, dgv.CurrentRow.Cells("TRANTYPE1").Value.ToString)
                        Else
                            If GetAdmindbSoftValue("PRN_STKTRANSFER_DETSUMM", "N") = "Y" Then
                                PrintStockTransfer_DETSUM(RefNo, RefDate, dgv.CurrentRow.Cells("TRANTYPE1").Value.ToString)
                            Else
                                PrintStockTransfer(RefNo, RefDate, dgv.CurrentRow.Cells("TRANTYPE1").Value.ToString)
                            End If
                        End If
                    End If
                End If

                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
                Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
                If GST And BillPrint_Format = "M1" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("POS", dgv.CurrentRow.Cells("BATCHNO").Value.ToString, Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M2" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB5("POS", dgv.CurrentRow.Cells("BATCHNO").Value.ToString, Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M3" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA5("POS", dgv.CurrentRow.Cells("BATCHNO").Value.ToString, Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M4" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", dgv.CurrentRow.Cells("BATCHNO").Value.ToString, Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y")
                    Me.Refresh()
                ElseIf GST And BillPrintExe = False Then
                    Dim billDoc As New frmBillPrintDoc("POS", dgv.CurrentRow.Cells("BATCHNO").Value.ToString, Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y")
                    Me.Refresh()
                Else
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                        Dim write As IO.StreamWriter
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        If dgv.CurrentRow.Cells("TYPE").Value.ToString = "I" Then
                            write.WriteLine(LSet("TYPE", 15) & ":SMI")
                        Else
                            write.WriteLine(LSet("TYPE", 15) & ":SMR")
                        End If
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString)
                        write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"))
                        write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                            LSet("TYPE", 15) & ":" & IIf(dgv.CurrentRow.Cells("TYPE").Value.ToString = "I", "SMI", "SMR") & ";" &
                            LSet("BATCHNO", 15) & ":" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & ";" &
                            LSet("TRANDATE", 15) & ":" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & ";" &
                            LSet("DUPLICATE", 15) & ":Y")
                        End If
                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                    End If
                End If



            ElseIf UCase(e.KeyChar) = "E" Then
                Dim dgv As DataGridView = CType(sender, DataGridView)
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Edit.", MsgBoxStyle.Information) : Exit Sub
                If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
                If chkbackdateedit(dgv.CurrentRow.Cells("TRANDATE").Value) = False Then Exit Sub
                If dgv.CurrentRow.Cells("TRANTYPE").Value.ToString = "MI" Then MsgBox("Can't able to edit POS Entry " & vbCrLf & " Use Bill view Option", MsgBoxStyle.Information) : Exit Sub
                If dgv.CurrentRow.Cells("TRANTYPE").Value.ToString = "MISC ISSUE" Then MsgBox("Can't able to edit POS Entry " & vbCrLf & " Use Bill view Option", MsgBoxStyle.Information) : Exit Sub
                If objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..MELTINGDETAIL WHERE BATCHNO ='" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "' AND ISNULL(CANCEL,'')=''", "", "", tran) Then MsgBox("This Melting Record unable to edit") : Exit Sub
                ''Checking for Tag Issued or Not,if Tag issue from the Lot not able to Edit
                strSql = "SELECT CPCS,CGRSWT FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN "
                strSql += vbCrLf + " (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO IN"
                strSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "') "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'')='')"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Dim CPcs As Integer = 0
                    Dim CGrsWt As Decimal = 0
                    CPcs = dt.Compute("SUM(CPCS)", Nothing)
                    CGrsWt = dt.Compute("SUM(CGRSWT)", Nothing)
                    If CGrsWt <> 0 Or CPcs <> 0 Then
                        'MsgBox("Tag Generated for this Entry." & vbCrLf & "Not able to Edit.", MsgBoxStyle.Information)
                        'Exit Sub
                        If MessageBox.Show("Tag Generated,Continue Edit ?.", "Edit Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                End If
                Dim obj As MaterialIssRecTran
                If dgv.CurrentRow.Cells("TYPE").Value.ToString = "I" Then
                    strSql = "SELECT TRANTYPE,FLAG FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    Dim dtIsschk As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtIsschk)
                    If dtIsschk.Rows(0).Item(1) = "X" And dtIsschk.Rows(0).Item(0) = "IIN" Then
                        MsgBox("This Internal Transfer unable to edit")
                        Exit Sub
                    End If
                    obj = New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Issue)
                    obj.lblTitle.Text = "MATERIAL ISSUE"
                Else
                    strSql = "SELECT TRANTYPE,FLAG FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    Dim dtIsschk As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtIsschk)
                    If dtIsschk.Rows(0).Item(1) = "X" And dtIsschk.Rows(0).Item(0) = "RIN" Then
                        MsgBox("This Internal Transfer unable to edit")
                        Exit Sub
                    End If
                    obj = New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Receipt)
                    obj.lblTitle.Text = "MATERIAL RECEIPT"
                End If
                obj.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
                obj.MaximizeBox = False
                obj.EditBatchno = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
                If obj.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    obj.btnView.Enabled = False
                    objGPack.GetParentControl(dgv).Close()
                End If
            ElseIf UCase(e.KeyChar) = "U" Then
                Dim dgv As DataGridView = CType(sender, DataGridView)
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("TYPE").Value.ToString = "I" Then Exit Sub
                If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                If dgv.CurrentRow.Cells("SNO").Value.ToString = "" Then Exit Sub
                If dgv.CurrentRow.Cells("TRANTYPE").Value.ToString = "MI" Then MsgBox("Can't able to update POS Entry " & vbCrLf & " Use Bill view Option", MsgBoxStyle.Information) : Exit Sub
                Dim SNO As String = dgv.CurrentRow.Cells("SNO").Value.ToString
                Dim BATCHNO As String = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
                Dim Tranno As String = dgv.CurrentRow.Cells("TRANNO").Value.ToString
                Dim costId As String = dgv.Item("costid", dgv.CurrentRow.Index).Value.ToString

                Dim mredit As Boolean = IIf(GetAdmindbSoftValue("MR_EDIT_ALLOW", "N") = "Y", True, False)
                Dim LotAutoPost As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
                Dim objPcs As New FrmPcsUpdate
                If mredit = True Then
                    'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                    objPcs.txtPCS_NUM.Text = dgv.CurrentRow.Cells("PCS").Value.ToString
                    objPcs.txtPCS_NUM.Select()
                    If objPcs.ShowDialog = Windows.Forms.DialogResult.OK Then
                        strSql = "UPDATE " & cnStockDb & "..RECEIPT SET PCS=" & Val(objPcs.txtPCS_NUM.Text) & " WHERE SNO='" & SNO & "' AND BATCHNO='" & BATCHNO & "' AND TRANNO=" & Tranno & " AND COSTID='" & costId & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                        If LotAutoPost = True Then
                            strSql = "UPDATE " & cnStockDb & "..LOTISSUE SET PCS=" & Val(objPcs.txtPCS_NUM.Text) & " WHERE RECSNO='" & SNO & "' AND TRANNO=" & Tranno & " "
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            objGridShower.Close()
                            dgv.Focus()
                            btnSearch_Click(Me, New EventArgs)
                        End If
                    ElseIf objPcs.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                        Exit Sub
                    End If
                Else
                    If userId = 999 Then
                        objPcs.txtPCS_NUM.Text = dgv.CurrentRow.Cells("PCS").Value.ToString
                        objPcs.txtPCS_NUM.Select()
                        If objPcs.ShowDialog = Windows.Forms.DialogResult.OK Then
                            strSql = "UPDATE " & cnStockDb & "..RECEIPT SET PCS=" & Val(objPcs.txtPCS_NUM.Text) & " WHERE SNO='" & SNO & "' AND BATCHNO='" & BATCHNO & "' AND TRANNO=" & Tranno & " AND COSTID='" & costId & "'"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            If LotAutoPost = True Then
                                strSql = "UPDATE " & cnStockDb & "..LOTISSUE SET PCS=" & Val(objPcs.txtPCS_NUM.Text) & " WHERE RECSNO='" & SNO & "' AND TRANNO=" & Tranno & " "
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                                objGridShower.Close()
                                dgv.Focus()
                                btnSearch_Click(Me, New EventArgs)
                            End If
                        ElseIf objPcs.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                            Exit Sub
                        End If
                    End If
                End If
            ElseIf UCase(e.KeyChar) = "M" Then
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize) Then Exit Sub
                If MIMR_MARKED And rbtDetailed.Checked And rbtBoth.Checked = False And rbtIssRecBoth.Checked Then
                    Dim dgv As DataGridView = CType(sender, DataGridView)
                    Dim SNO As String = dgv.CurrentRow.Cells("SNO").Value.ToString
                    Dim BATCHNO As String = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
                    Dim Tranno As String = dgv.CurrentRow.Cells("TRANNO").Value.ToString
                    Dim costId As String = dgv.Item("costid", dgv.CurrentRow.Index).Value.ToString
                    Dim _trandate As String = Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd").ToString
                    Dim _marked As String = IIf(dgv.Item("MARKED", dgv.CurrentRow.Index).Value.ToString = "", "Y", "")
                    Dim _markeddate As DateTime = Nothing
                    If dgv.Item("MARKED", dgv.CurrentRow.Index).Value.ToString = "" Then
                        _markeddate = GetSqlValue(cn, "SELECT GETDATE()")
                    End If

                    If SNO = "" Then Exit Sub
                    If BATCHNO = "" Then Exit Sub
                    If Tranno = "" Then Exit Sub
                    If _trandate = "" Then Exit Sub
                    If _marked <> "Y" Then
                        If MessageBox.Show("Do you want to Unmark Invoice?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                            Exit Sub
                        End If
                    End If
                    If rbtIssue.Checked Then
                        strSql = " UPDATE " & cnStockDb & "..ISSUE SET SETTLEMENT='" & _marked.ToString & "'"
                        If dgv.Item("MARKED", dgv.CurrentRow.Index).Value.ToString = "" Then
                            strSql += " ,SETTLEMENTDATE='" & Format(_markeddate, "yyyy-MM-dd HH:mm:ss").ToString & "'  "
                        Else
                            strSql += " ,SETTLEMENTDATE=NULL  "
                        End If
                        strSql += " WHERE TRANDATE='" & _trandate.ToString & "' AND BATCHNO='" & BATCHNO & "' AND TRANNO=" & Tranno & " AND COSTID='" & costId & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                    ElseIf rbtReceipt.Checked Then
                        strSql = "UPDATE " & cnStockDb & "..RECEIPT SET SETTLEMENT='" & _marked.ToString & "' "
                        If dgv.Item("MARKED", dgv.CurrentRow.Index).Value.ToString = "" Then
                            strSql += " ,SETTLEMENTDATE='" & Format(_markeddate, "yyyy-MM-dd HH:mm:ss").ToString & "'  "
                        Else
                            strSql += " ,SETTLEMENTDATE=NULL  "
                        End If
                        strSql += " WHERE TRANDATE='" & _trandate.ToString & "' AND BATCHNO='" & BATCHNO & "' AND TRANNO=" & Tranno & " AND COSTID='" & costId & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                    End If
                    If _marked <> "Y" Then
                        MsgBox("UnMarked Successfully", MsgBoxStyle.Information)
                    Else
                        MsgBox("Marked Successfully", MsgBoxStyle.Information)
                    End If

                    For cntt As Integer = 0 To dgv.Rows.Count - 1
                        If dgv.Rows(cntt).Cells("BATCHNO").Value.ToString = BATCHNO.ToString Then
                            dgv.Rows(cntt).Cells("MARKED").Value = _marked.ToString
                            If _marked <> "Y" Then
                                dgv.Rows(cntt).Cells("MARKEDDATE").Value = DBNull.Value
                                dgv.Rows(cntt).DefaultCellStyle.BackColor = Color.White
                            Else
                                dgv.Rows(cntt).Cells("MARKEDDATE").Value = _markeddate
                                dgv.Rows(cntt).DefaultCellStyle.BackColor = Color.Lavender
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSmithRecIssView_Properties
        obj.p_chkcmbTranType = chkcmbTranType.Text
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkSmith = chkSmith.Checked
        GetChecked_CheckedList(chkLstSmith, obj.p_chkLstSmith)
        SetSettingsObj(obj, Me.Name, GetType(frmSmithRecIssView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSmithRecIssView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithRecIssView_Properties))
        chkcmbTranType.Text = obj.p_chkcmbTranType
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkSmith.Checked = obj.p_chkSmith
        SetChecked_CheckedList(chkLstSmith, obj.p_chkLstSmith, Nothing)
    End Sub

    Private Sub chkcmbTranType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbTranType.LostFocus
        If chkcmbTranType.Text = "ALL" Then
            ChkApproval.Enabled = True
            ChkApproval.Checked = True
        Else
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
        End If
    End Sub

    Private Sub ChkMultiSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMultiSmith.CheckedChanged
        If ChkMultiSmith.Checked Then
            CmbSmithname.Visible = False
            chkLstSmith.Visible = True
            Panel1.Location = New System.Drawing.Point(12, 409)
        Else
            CmbSmithname.Visible = True
            chkLstSmith.Visible = False
            Panel1.Location = New System.Drawing.Point(12, 390)
        End If
    End Sub

    Private Sub rbtBoth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtBoth.CheckedChanged
        Chktran.Enabled = False
        If MIMR_MARKED Then
            PnlCheckType.Visible = Not rbtBoth.Checked
            If rbtBoth.Checked Then
                rbtIssRecBoth.Checked = True
            End If
        Else
            PnlCheckType.Visible = False
            rbtIssRecBoth.Checked = True
        End If

    End Sub

    Private Sub rbtAcAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtAcAll.CheckedChanged, rbtSmith.CheckedChanged, rbtDealer.CheckedChanged, rbtOthers.CheckedChanged
        strSql = vbCrLf + " SELECT LTRIM(ACNAME) FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(GRPLEDGER,'') NOT IN ('T','P'))"
        If rbtSmith.Checked Then
            strSql += vbCrLf + " AND ACTYPE IN ('G')"
        ElseIf rbtDealer.Checked Then
            strSql += vbCrLf + " AND ACTYPE IN ('D')"
        ElseIf rbtOthers.Checked Then
            strSql += vbCrLf + " AND ACTYPE NOT IN ('D','G')"
        End If
        strSql += vbCrLf + " ORDER BY LTRIM(ACNAME)"
        chkLstSmith.Items.Clear()
        FillCheckedListBox(strSql, chkLstSmith)

        strSql = " SELECT 'ALL' ACNAME ,0 RESULT UNION ALL"
        strSql += vbCrLf + " SELECT LTRIM(ACNAME),1 RESULT FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(GRPLEDGER,'') NOT IN ('T','P'))"
        If rbtSmith.Checked Then
            strSql += vbCrLf + " AND ACTYPE IN ('G')"
        ElseIf rbtDealer.Checked Then
            strSql += vbCrLf + " AND ACTYPE IN ('D')"
        ElseIf rbtOthers.Checked Then
            strSql += vbCrLf + " AND ACTYPE NOT IN ('D','G')"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        CmbSmithname.Items.Clear()
        objGPack.FillCombo(strSql, CmbSmithname, True, False)
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        pnlOrder.Enabled = rbtDetailed.Checked
        If MIMR_MARKED Then
            PnlCheckType.Visible = rbtDetailed.Checked
            If rbtDetailed.Checked = False Then
                rbtIssRecBoth.Checked = True
            End If
        Else
            PnlCheckType.Visible = False
            rbtIssRecBoth.Checked = True
        End If
    End Sub

    Private Sub rbtsummary_CheckedChanged(sender As Object, e As EventArgs) Handles rbtsummary.CheckedChanged
        If MIMR_MARKED Then
            If rbtsummary.Checked = True Then
                PnlCheckType.Visible = False
                rbtIssRecBoth.Checked = True
            Else
                PnlCheckType.Visible = True
                rbtIssRecBoth.Checked = True
            End If
        Else
            PnlCheckType.Visible = False
            rbtIssRecBoth.Checked = True
        End If
    End Sub

End Class

Public Class frmSmithRecIssView_Properties
    Private rbtIssue As Boolean = False
    Private rbtReceipt As Boolean = False
    Private chkcmbTranType As String = ""
    Public Property p_chkcmbTranType() As String
        Get
            Return chkcmbTranType
        End Get
        Set(ByVal value As String)
            chkcmbTranType = value
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

    Private chkSmith As Boolean = False
    Public Property p_chkSmith() As Boolean
        Get
            Return chkSmith
        End Get
        Set(ByVal value As Boolean)
            chkSmith = value
        End Set
    End Property

    Private chkLstSmith As New List(Of String)
    Public Property p_chkLstSmith() As List(Of String)
        Get
            Return chkLstSmith
        End Get
        Set(ByVal value As List(Of String))
            chkLstSmith = value
        End Set
    End Property

End Class