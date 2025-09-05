Imports System.Data.OleDb
Imports System.IO
Imports System.Xml

Public Class frmPurchaseReport
    'CALID-591: CLIENT = ALL: CORRECTION = GRAND TOTAL OPTION SHOULD BE ADD : ALTER BY SATHYA
    'calno 281112: CLIENT=PRINCE: CORRECTION =ESTIMATIONNO NEEDED: ALTER BY VASANTHAN
    'calno 040314: CLIENT=NATHELLA: CORRECTION =Partly sales included in this report
    Dim dsPurchase As New DataSet
    Dim dtPurchase As New DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String
    Dim dtMetal As DataTable
    Dim dtCashCounter As New DataTable
    Dim Authorize As Boolean = False
    Dim Save As Boolean = False
    Dim MELPUR As Double = Val(GetAdmindbSoftValue("MELT_PURITY", 2))
    Dim MELPUTGROUPBY As Boolean = IIf(GetAdmindbSoftValue("MELT_PURITY_GROUPBY", "N") = "Y", True, False)
    Dim IS40COLCLSSTKPRINT As Boolean = IIf(GetAdmindbSoftValue("40COLCLSSTKPRINT", "N") = "Y", True, False)
    Dim SqlVersion As String = ""

    Private Sub frmPurchaseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        funcAddMetalName()
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
        cmbCostCentre.Items.Clear()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, False)
        Else
            cmbCostCentre.Enabled = False
        End If
        LoadCompany(chkLstCompany)
        ProcAddNodeId()
        ProcAddCashCounter()
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If Authorize = False Then
            chkLstCompany.Enabled = False
            chkCompanySelectAll.Enabled = False
            cmbCostCentre.Enabled = False
            chkCmbMetal.Enabled = False
            Panel2.Enabled = False
            Panel3.Enabled = False
            Panel4.Enabled = False
            pnlMake.Enabled = False
            PnlMark.Enabled = False
            chkcmbCash.Enabled = False
            chkcmbCategory.Enabled = False
            chkNodeSelectAll.Enabled = False
            chkPureWtBased.Enabled = False
            chkLstNodeId.Enabled = False
            ChkLstGroupBy.Enabled = False : ChkPurchaseMelting.Enabled = False
            ChkBillSum.Enabled = False : chkGrtotal.Enabled = False
            rbtSummary.Enabled = False : chkSubitem.Enabled = False
            rbtDetailed.Enabled = False : chkItem.Enabled = False
            btnSave_OWN.Enabled = False
        End If

        SqlVersion = ""
        strSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
        SqlVersion = GetSqlValue(cn, strSql)

        btnnew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnnew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewReport()
        If ChkPurchase.Checked = False And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False And chkOrdAdv.Checked = False Then
            MsgBox("Select any one option", MsgBoxStyle.Information)
            ChkPurchase.Focus()
            Exit Sub
        End If
        Dim tmpcnt As Integer = 0
        Dim Rtrantype As String = ""
        If ChkPurchase.Checked Then Rtrantype += "'PU',"
        If ChkSalesReturn.Checked Then Rtrantype += "'SR',"
        If chkOrdAdv.Checked Then Rtrantype += "'AD',"
        If Rtrantype.Trim <> "" Then Rtrantype = Mid(Rtrantype, 1, Len(Rtrantype) - 1)

        'If chkLstCashCounter.CheckedItems.Count = 0 Then chkCashCounterSelectAll.Checked = True
        If chkLstCompany.CheckedItems.Count = 0 Then chkCompanySelectAll.Checked = True
        If chkLstCompany.CheckedItems.Count = chkLstCompany.Items.Count Then
            SelectedCompany = "ALL"
        Else
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        End If
        Dim SelectedCashCounter As String = "ALL"
        ''If chkLstCashCounter.CheckedItems.Count = chkLstCashCounter.Items.Count Then
        ''    SelectedCashCounter = "ALL"
        ''Else
        ''    SelectedCashCounter = GetChecked_CheckedListid(chkLstCashCounter, "CASHID", "CASHNAME", cnAdminDb & "..CASHCOUNTER", True)
        ''End If
        Dim tempchkitem As String = ""
        If chkLstNodeId.Items.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                    If chkLstNodeId.GetItemChecked(CNT) = True Then
                        tempchkitem = tempchkitem & " '" & chkLstNodeId.Items.Item(CNT) & "'" & ", "
                    End If
                Next
                If Len(tempchkitem) > 1 Then tempchkitem = Mid(Trim(tempchkitem), 1, Len(Trim(tempchkitem)) - 1)
            End If
        Else
            tempchkitem = ""
        End If

        strSql = vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPPUSR_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPUSR_VIEW "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPPUSR_VIEW_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPUSR_VIEW_NEW "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  SELECT "
        If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then strSql += vbCrLf + "  CONVERT(INT,ISNULL(R.MARK,0)) MARK,"
        strSql += vbCrLf + " CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(400),NULL)AS PARTICULAR"
        If chkSubitem.Checked = False Then
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME ELSE '' END AS SUBITEM "
        End If
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),CASE WHEN ISNULL(IM.SHORTNAME,'') <> '' THEN IM.SHORTNAME ELSE '' END) AS SNAME  "
        strSql += vbCrLf + " ,R.TRANNO,R.TRANDATE,CSH.CASHNAME AS CASHCOUNTER"
        If chkItem.Checked Then
            'strSql += vbCrLf + " ,('['+REPLICATE(' ',4-LEN(IM.ITEMID)) + CONVERT(VARCHAR,IM.ITEMID) + '] ' + IM.ITEMNAME) AS DESCRIPT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(IM.ITEMID,0)<>0 THEN ('['+REPLICATE(' ',4-LEN(IM.ITEMID)) + CONVERT(VARCHAR,IM.ITEMID) + '] ' + IM.ITEMNAME) ELSE ISNULL(R.REMARK1,'') END AS DESCRIPT"
        ElseIf chkItem.Checked = False Then
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME WHEN ISNULL(IM.ITEMNAME,'') <> '' THEN IM.ITEMNAME ELSE C.CATNAME END+CASE WHEN ISNULL(TAGNO,'')<>'' THEN '(TAGNO:'+TAGNO+')' END AS DESCRIPT      "
        End If

        strSql += vbCrLf + "  ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = R.ITEMTYPEID)AS ITEMTYPE"
        strSql += vbCrLf + "  ,R.PCS,R.GRSWT "
        strSql += vbCrLf + "  ,ISNULL((SELECT SUM(CASE WHEN STONEUNIT='C' THEN CONVERT(NUMERIC(15,3),STNWT*0.2) ELSE STNWT END) FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO),0) + ISNULL(R.LESSWT,0)LESSWT,R.DUSTWT"
        strSql += vbCrLf + "  ,CASE WHEN WASTPER<>0 THEN WASTPER ELSE ROUND((R.WASTAGE/CASE WHEN R.GRSWT=0 THEN 1 ELSE R.GRSWT END)*100,2) END WASTPER,R.WASTAGE"
        If chkPureWtBased.Checked Then
            strSql += vbCrLf + "  ,ISNULL(R.GRSWT,0) - (ISNULL((SELECT SUM(STNWT) FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO),0) + ISNULL(R.DUSTWT,0)) NETWT"
        Else
            strSql += vbCrLf + "  ,R.NETWT"
        End If
        If chkPureWtBased.Checked Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),R.PURITY)PURITY"
            strSql += vbCrLf + "  ,R.PUREWT AS PUREWT"
        Else
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),R.PURITY)PURITY"
            strSql += vbCrLf + " ,NULL AS PUREWT"
        End If
        strSql += vbCrLf + "  ,R.RATE "
        strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S'))STNWT"
        strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM  " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT"
        If chkNodewiseSummary.Checked = True Then
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0.00)MELTPER,  CONVERT(NUMERIC(15,3),0.00)MELTWT "
        Else
            If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)MELTPER,  CONVERT(NUMERIC(15,3),NULL)MELTWT "
            End If
        End If
        strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=R.BATCHNO AND ISSSNO=R.SNO AND TRANTYPE='SR' AND TAXID='IG' AND ISNULL(STUDDED,'N')<>'Y')AS IGST"
        strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=R.BATCHNO AND ISSSNO=R.SNO AND TRANTYPE='SR' AND TAXID='CG' AND ISNULL(STUDDED,'N')<>'Y')AS CGST"
        strSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=R.BATCHNO AND ISSSNO=R.SNO AND TRANTYPE='SR' AND TAXID='SG' AND ISNULL(STUDDED,'N')<>'Y')AS SGST"
        strSql += vbCrLf + "  ,(R.AMOUNT + R.TAX) AS AMOUNT,"
        If chkNodewiseSummary.Checked = True Then
            strSql += vbCrLf + "  NULL TABLECODE,NULL NODE,NULL REFNO,NULL AS REFDATE"
            strSql += vbCrLf + "  ,NULL EMPNAME      "
            strSql += vbCrLf + "  ,NULL AS METAL,NULL AS CATEGORY"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN R.TRANTYPE = 'PU' THEN 'PURCHASE' WHEN R.TRANTYPE = 'AD' THEN 'ORDER ADVANCE' ELSE 'SALES RETURN' END) AS TRANTYPE      "
            strSql += vbCrLf + "  ,NULL AS SNO,NULL AS EMPID ,NULL ESTIMATENO"
        Else
            strSql += vbCrLf + "  R.TABLECODE,R.SYSTEMID AS NODE,R.REFNO,CONVERT(VARCHAR,R.REFDATE,103)AS REFDATE"
            strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=R.EMPID)EMPNAME      "
            strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METAL,C.CATNAME AS CATEGORY"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN R.TRANTYPE = 'PU' THEN 'PURCHASE' WHEN R.TRANTYPE = 'AD' THEN 'ORDER ADVANCE' ELSE 'SALES RETURN' END) AS TRANTYPE      "
            'strSql += vbCrLf + "  ,SNO"
            'calno 281112
            'strSql += vbCrLf + "  ,R.SNO AS SNO,R.EMPID AS EMPID ,(SELECT top 1 TRANNO FROM " & cnStockDb & "..ESTRECEIPT WHERE BATCHNO=R.BATCHNO )AS ESTIMATENO"
            strSql += vbCrLf + "  ,R.SNO AS SNO,R.EMPID AS EMPID ,CONVERT(INT,R.ESTSNO) ESTIMATENO"
        End If
        strSql += vbCrLf + " ,CONVERT(VARCHAR(10),(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=R.BATCHNO AND TRANTYPE IN('SA','OD','RD'))) SREFNO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(10),(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=R.BATCHNO AND ACCODE <> 'ADVORD' AND PAYMODE IN('AR'))) AREFNO"
        'strSql += vbCrLf + "  ,CONVERT(VARCHAR(300),NULL)GROUP1,CONVERT(VARCHAR(300),NULL)GROUP2,CONVERT(VARCHAR(300),NULL)GROUP3      "
        'strSql += vbCrLf + "  ,CONVERT(INT,1)ORDG1,CONVERT(INT,1)ORDG2,CONVERT(INT,1)ORDG3"
        strSql += vbCrLf + "  ,R.BOARDRATE,CONVERT(NUMERIC(15,2),NULL)TOUCH,CONVERT(VARCHAR(3),NULL)COLHEAD "
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(10),CASE WHEN R.MAKE='W' THEN 'OWN' WHEN R.MAKE='O' THEN 'OTHER' ELSE 'NONE' END) MAKEWISE,TRFNO,(CASE WHEN R.TRANTYPE = 'PU' THEN 1 WHEN R.TRANTYPE = 'AD' THEN 3 ELSE 2 END) SRESULT"
        If rbtDetailed.Checked And ChkBillSum.Checked = False And chkNodewiseSummary.Checked = False Then
            If Val(SqlVersion) > 8 Then
                strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnStockDb & "..RECEIPTHALLMARK H WHERE (R.SNO = H.ISSSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
            Else
                strSql += vbCrLf + " ,SELECT TOP 1 H.[HM_BILLNO] FROM " & cnStockDb & "..RECEIPTHALLMARK H WHERE (R.SNO = H.ISSSNO) HALLMARKNO"
            End If
        End If
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPPUSR_VIEW "
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS R "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = R.CATCODE "
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CASHCOUNTER AS CSH ON CSH.CASHID = R.CASHID "
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = R.ITEMID "
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = R.ITEMID AND SM.SUBITEMID =R.SUBITEMID "
        'strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ESTRECEIPT AS EST ON EST.BATCHNO=R.BATCHNO "
        strSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(R.CANCEL,'') = '' "
        If tempchkitem <> "" And tempchkitem <> "ALL" Then
            strSql += vbCrLf + " AND R.SYSTEMID IN (" & tempchkitem & ")"
        End If
        If Rtrantype.Trim <> "" Then
            strSql += vbCrLf + " AND R.TRANTYPE IN(" & Rtrantype & ")"
        Else
            strSql += vbCrLf + " AND R.TRANTYPE IN('')"
        End If
        If rbtOWN.Checked Then
            strSql += vbCrLf + "  AND R.MAKE= 'W'"
        ElseIf rbtOther.Checked Then
            strSql += vbCrLf + "  AND R.MAKE= 'O'"
        End If
        If chkCmbMetal.Text <> "" And chkCmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        If chkcmbCategory.Text <> "" And chkcmbCategory.Text <> "ALL" Then strSql += vbCrLf + "  AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & "))"
        If chkcmbCash.Text <> "" And chkcmbCash.Text <> "ALL" Then strSql += vbCrLf + "  AND R.CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & GetQryString(chkcmbCash.Text) & "))"
        If chkCmbItem.Text <> "" And chkCmbItem.Text <> "ALL" Then strSql += vbCrLf + "  AND R.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then strSql += vbCrLf + "  AND ISNULL(R.COSTID,'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then
            If rbtMarked.Checked Then
                strSql += vbCrLf + "  AND ISNULL(R.MARK,0) = 1"
            ElseIf rbtUnmarked.Checked Then
                strSql += vbCrLf + "  AND ISNULL(R.MARK,0) <> 1"
            End If
        End If
        If rbtPurchaseOnly.Checked Then
            strSql += vbCrLf + "  AND BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(R.CANCEL,'') = '' )"
        ElseIf rbtSalesPurchase.Checked Then
            strSql += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(R.CANCEL,'') = '' )"
        End If
        'strSql += vbCrLf + "  AND R.GRSWT <> 0  "
        strSql += vbCrLf + "  AND R.COMPANYID IN (" & GetSelectedCompanyId(chkLstCompany, True) & ")"
        If SelectedCashCounter <> "ALL" And SelectedCashCounter <> "" Then
            strSql += vbCrLf + "  AND R.CASHID IN(" & SelectedCashCounter & ")"
        End If
        'For Partly Sales
        If ChkPartlySales.Checked Then
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then strSql += vbCrLf + "  0 MARK,"
            strSql += vbCrLf + " CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(400),NULL)AS PARTICULAR"
            If chkSubitem.Checked = False Then
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME ELSE '' END AS SUBITEM "
            End If
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(IM.SHORTNAME,'') <> '' THEN IM.SHORTNAME ELSE '' END AS SNAME  "
            strSql += vbCrLf + "  ,I.TRANNO,I.TRANDATE,CSH.CASHNAME AS CASHCOUNTER"
            If chkItem.Checked Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(IM.ITEMID,0)<>0 THEN ('['+REPLICATE(' ',4-LEN(IM.ITEMID)) + CONVERT(VARCHAR,IM.ITEMID) + '] ' + IM.ITEMNAME) ELSE ISNULL(I.REMARK1,'') END AS DESCRIPT"
            ElseIf chkItem.Checked = False Then
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME WHEN ISNULL(IM.ITEMNAME,'') <> '' THEN IM.ITEMNAME ELSE C.CATNAME END AS DESCRIPT      "
            End If

            strSql += vbCrLf + "  ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID)AS ITEMTYPE"
            strSql += vbCrLf + "  ,(I.TAGPCS-I.PCS) AS PCS,(I.TAGGRSWT-I.GRSWT) AS GRSWT "
            strSql += vbCrLf + "  ,ISNULL(I.LESSWT,0)LESSWT,0 AS DUSTWT"
            'strSql += vbCrLf + "  ,CASE WHEN WASTPER<>0 THEN WASTPER ELSE ROUND((I.WASTAGE/CASE WHEN I.GRSWT=0 THEN 1 ELSE I.GRSWT END)*100,2) END WASTPER,I.WASTAGE"
            strSql += vbCrLf + "  ,0 AS WASTPER,0 AS WASTAGE"
            If chkPureWtBased.Checked Then
                strSql += vbCrLf + "  ,(I.TAGGRSWT-I.GRSWT) - (ISNULL((SELECT SUM(TAGSTNWT-STNWT) FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO),0) ) NETWT"
            Else
                strSql += vbCrLf + "  ,(I.TAGNETWT-I.NETWT) AS NETWT"
            End If
            If chkPureWtBased.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),I.PURITY)PURITY"
                strSql += vbCrLf + "  ,(I.TAGNETWT-I.NETWT) AS PUREWT"
            Else
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),I.PURITY)PURITY"
                strSql += vbCrLf + " ,NULL AS PUREWT"
            End If
            strSql += vbCrLf + "  ,I.RATE "
            strSql += vbCrLf + "  ,(SELECT SUM(TAGSTNWT-STNWT) FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S'))STNWT"
            strSql += vbCrLf + "  ,(SELECT SUM(TAGSTNWT-STNWT) FROM  " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT"
            If chkNodewiseSummary.Checked = True Then
                strSql += vbCrLf + "  ,0 AS MELTPER"
                strSql += vbCrLf + "  ,0 AS MELTWT"
            End If
            strSql += vbCrLf + "  ,0 AS IGST"
            strSql += vbCrLf + "  ,0 AS CGST"
            strSql += vbCrLf + "  ,0 AS SGST"
            If chkNodewiseSummary.Checked = True Then
                strSql += vbCrLf + "  ,0 AMOUNT,NULL TABLECODE,NULL NODE,NULL REFNO,CONVERT(VARCHAR,NULL)AS REFDATE"
                strSql += vbCrLf + "  ,NULL EMPNAME"
                strSql += vbCrLf + "  ,NULL AS METAL,NULL AS CATEGORY"
                strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),'PARTLY SALES') AS TRANTYPE"
                strSql += vbCrLf + "  ,NULL AS SNO,NULL EMPID ,NULL ESTIMATENO"
            Else
                strSql += vbCrLf + "  ,0 AMOUNT,I.TABLECODE,I.SYSTEMID AS NODE,NULL REFNO,CONVERT(VARCHAR,NULL)AS REFDATE"
                strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.EMPID)EMPNAME"
                strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METAL,C.CATNAME AS CATEGORY"
                strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),'PARTLY SALES') AS TRANTYPE"
                strSql += vbCrLf + "  ,I.SNO AS SNO,I.EMPID AS EMPID ,CONVERT(INT,I.ESTSNO) ESTIMATENO"
            End If

            strSql += vbCrLf + " ,'' SREFNO,'' AREFNO"
            strSql += vbCrLf + "  ,I.BOARDRATE,CONVERT(NUMERIC(15,2),NULL)TOUCH,CONVERT(VARCHAR(3),NULL)COLHEAD "
            strSql += vbCrLf + "  ,NULL MAKEWISE,TRFNO,5 SRESULT"
            If rbtDetailed.Checked And ChkBillSum.Checked = False And chkNodewiseSummary.Checked = False Then
                If Val(SqlVersion) > 8 Then
                    strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnStockDb & "..ISSHALLMARK H WHERE (I.SNO = H.ISSSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
                Else
                    strSql += vbCrLf + " ,SELECT TOP 1 H.[HM_BILLNO] FROM " & cnStockDb & "..ISSHALLMARK H WHERE (I.SNO = H.ISSSNO) HALLMARKNO"
                End If
            End If
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CASHCOUNTER AS CSH ON CSH.CASHID = I.CASHID "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID =I.SUBITEMID "
            strSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' "
            strSql += vbCrLf + "  AND ((I.TAGGRSWT-I.GRSWT)<>0 OR (I.TAGNETWT-I.NETWT)<>0 OR (I.TAGPCS-I.PCS)<>0)"
            If tempchkitem <> "" And tempchkitem <> "ALL" Then
                strSql += vbCrLf + " AND I.SYSTEMID IN (" & tempchkitem & ")"
            End If
            strSql += vbCrLf + " AND I.TRANTYPE IN('SA') AND I.TAGNO<>''"
            If chkCmbMetal.Text <> "" And chkCmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & ")))"
            If chkcmbCategory.Text <> "" And chkcmbCategory.Text <> "ALL" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & "))"
            If chkcmbCash.Text <> "" And chkcmbCash.Text <> "ALL" Then strSql += vbCrLf + "  AND I.CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN (" & GetQryString(chkcmbCash.Text) & "))"
            If chkCmbItem.Text <> "" And chkCmbItem.Text <> "ALL" Then strSql += vbCrLf + "  AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then strSql += vbCrLf + "  AND ISNULL(I.COSTID,'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
            strSql += vbCrLf + "  AND I.COMPANYID IN (" & GetSelectedCompanyId(chkLstCompany, True) & ")"
            If SelectedCashCounter <> "ALL" And SelectedCashCounter <> "" Then
                strSql += vbCrLf + "  AND I.CASHID IN(" & SelectedCashCounter & ")"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        If chkNodewiseSummary.Checked = True Then
        Else
            strSql = "  UPDATE temptabledb..tempPUSR_VIEW SET TOUCH= ((ISNULL(AMOUNT,0)/ISNULL(GRSWT,0))/((BOARDRATE / 93) * 100))*100 WHERE ISNULL(GRSWT,0) <> 0 AND ISNULL(BOARDRATE,0) <> 0"
            strSql += " AND TRANTYPE <>'PARTLY SALES'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        If chkNodewiseSummary.Checked = True Then
        Else


            If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then
                strSql = "  select * from temptabledb..tempPUSR_VIEW ORDER BY TRANNO "
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable
                dt = New DataTable
                da.Fill(dt)
                For i As Integer = 0 To dt.Rows.Count - 1
                    If MELPUR = 0.0 Then MELPUR = dt.Rows(i).Item("PURITY").ToString()
                    If dt.Rows(i).Item("RATE") <> 0 Then 'And dt.Rows(i).Item("PURITY") <> 0 
                        If dt.Rows(i).Item("PURITY") <> 100 Then
                            Dim t1 As Decimal = dt.Rows(i).Item("AMOUNT") / dt.Rows(i).Item("GRSWT")
                            Dim t2 As Decimal = t1 / (dt.Rows(i).Item("RATE") / MELPUR)
                            strSql = " UPDATE temptabledb..tempPUSR_VIEW SET MELTPER='" & Math.Round(t2, 2) & "'"
                            strSql += " WHERE tranno='" & dt.Rows(i).Item("TRANNO") & "'  "
                            strSql += " and  GRSWT='" & dt.Rows(i).Item("GRSWT") & "' and "
                            strSql += "  ISNULL(" & dt.Rows(i).Item("GRSWT") & ",0) <> 0"
                            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                        ElseIf dt.Rows(i).Item("PURITY") = 100 Then
                            Dim t1 As Decimal = dt.Rows(i).Item("AMOUNT") / dt.Rows(i).Item("GRSWT")
                            Dim t2 As Decimal = (dt.Rows(i).Item("RATE") / 100)
                            Dim t9 As Decimal = Math.Round(t1, 2) / Math.Round(t2, 2)

                            strSql = " UPDATE temptabledb..tempPUSR_VIEW SET MELTPER='" & t9 & "'"
                            strSql += " WHERE tranno='" & dt.Rows(i).Item("TRANNO") & "'  "
                            strSql += " and  GRSWT='" & dt.Rows(i).Item("GRSWT") & "' and "
                            strSql += "  ISNULL(" & dt.Rows(i).Item("GRSWT") & ",0) <> 0"
                            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                        End If
                        'ElseIf dt.Rows(i).Item("RATE") <> 0 And dt.Rows(i).Item("PURITY") = 0 Then
                        '    Dim t1 As Decimal = dt.Rows(i).Item("AMOUNT") / dt.Rows(i).Item("GRSWT")
                        '    Dim t2 As Decimal = t1 / dt.Rows(i).Item("RATE")
                        '    strSql = " UPDATE temptabledb..tempPUSR_VIEW SET MELTPER='" & Math.Round(t2, 2) & "'"
                        '    strSql += " WHERE tranno='" & dt.Rows(i).Item("TRANNO") & "'  "
                        '    strSql += " and  GRSWT='" & dt.Rows(i).Item("GRSWT") & "' and "
                        '    strSql += "  ISNULL(" & dt.Rows(i).Item("GRSWT") & ",0) <> 0"
                        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                    ElseIf dt.Rows(i).Item("RATE") = 0 And dt.Rows(i).Item("PURITY") <> 0 Then
                        Dim t1 As Decimal = dt.Rows(i).Item("AMOUNT") / dt.Rows(i).Item("GRSWT")
                        Dim t2 As Decimal = t1 / MELPUR
                        strSql = " UPDATE temptabledb..tempPUSR_VIEW SET MELTPER='" & Math.Round(t2, 2) & "'"
                        strSql += " WHERE tranno='" & dt.Rows(i).Item("TRANNO") & "'  "
                        strSql += " and  GRSWT='" & dt.Rows(i).Item("GRSWT") & "' and "
                        strSql += "  ISNULL(" & dt.Rows(i).Item("GRSWT") & ",0) <> 0"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                    End If
                Next
                strSql = "  select * from temptabledb..tempPUSR_VIEW ORDER BY TRANNO "
                da = New OleDbDataAdapter(strSql, cn)
                dt = New DataTable
                dt = New DataTable
                da.Fill(dt)
                For j As Integer = 0 To dt.Rows.Count - 1
                    Dim t4 As Decimal = dt.Rows(j).Item("GRSWT")
                    Dim t5 As Decimal = Val(dt.Rows(j).Item("MELTPER").ToString)
                    Dim t6 As Decimal = (t4 * t5) / 100

                    strSql = " UPDATE temptabledb..tempPUSR_VIEW  SET MELTWT=" & t6 & ""
                    strSql += " WHERE tranno='" & dt.Rows(j).Item("TRANNO") & "'  "
                    strSql += " and  GRSWT='" & dt.Rows(j).Item("GRSWT") & "' and "
                    strSql += "  ISNULL(" & dt.Rows(j).Item("GRSWT") & ",0) <> 0"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                Next


                'strSql = "			    DECLARE @PURITY NUMERIC(15,3)"
                'strSql += vbCrLf + "         DECLARE @RATE NUMERIC(15,2) "
                'strSql += vbCrLf + "         DECLARE @AMOUNT NUMERIC(15,3)          "
                'strSql += vbCrLf + "         DECLARE @GRSWT NUMERIC(15,3) "
                'strSql += vbCrLf + "         DECLARE I_CUR CURSOR FOR SELECT AMOUNT,GRSWT,RATE,PURITY FROM temptabledb..tempPUSR_VIEW ORDER BY TRANDATE,TRANNO"
                'strSql += vbCrLf + "         OPEN I_CUR"
                'strSql += vbCrLf + "         WHILE 1=1"
                'strSql += vbCrLf + "         BEGIN"
                'strSql += vbCrLf + "         	FETCH NEXT FROM I_CUR INTO @PURITY,@RATE,@AMOUNT,@GRSWT"
                'strSql += vbCrLf + "         	IF @@FETCH_STATUS = -1 BREAK"
                'strSql += vbCrLf + "         	 WHILE 1 = 1"
                'strSql += vbCrLf + "         	BEGIN"
                'strSql += vbCrLf + "         		IF @RATE <> 0 AND @PURITY<>0"
                'strSql += vbCrLf + "         		BEGIN "
                'strSql += vbCrLf + "         			IF @PURITY<>100 "
                'strSql += vbCrLf + "         				BEGIN "
                'strSql += vbCrLf + "         				UPDATE TT  SET MELTPER=((ISNULL(@AMOUNT,0)/ISNULL(@GRSWT,0))/(@RATE/93.5)) FROM temptabledb..tempPUSR_VIEW TT"
                'strSql += vbCrLf + "         				WHERE ISNULL(@GRSWT,0) <> 0"
                'strSql += vbCrLf + "         				END"
                'strSql += vbCrLf + "         			ELSE  "
                'strSql += vbCrLf + "         				BEGIN              				"
                'strSql += vbCrLf + "         				UPDATE TT  SET MELTPER=((ISNULL(@AMOUNT,0)/ISNULL(@GRSWT,0))/((@RATE /93.5/100)/100) )FROM temptabledb..tempPUSR_VIEW TT"
                'strSql += vbCrLf + "         				WHERE ISNULL(@GRSWT,0) <> 0"
                'strSql += vbCrLf + "         				END             		             		   "
                'strSql += vbCrLf + "         		END"
                'strSql += vbCrLf + "         		IF @RATE <> 0 AND @PURITY=0"
                'strSql += vbCrLf + "         		BEGIN          			"
                'strSql += vbCrLf + "	                UPDATE TT  SET MELTPER=((ISNULL(@AMOUNT,0)/ISNULL(@GRSWT,0))/(@RATE)) FROM temptabledb..tempPUSR_VIEW TT"
                'strSql += vbCrLf + "         			WHERE ISNULL(@GRSWT,0) <> 0"
                'strSql += vbCrLf + "         		END"
                'strSql += vbCrLf + "         		IF @RATE = 0 AND @PURITY<>0"
                'strSql += vbCrLf + "         		BEGIN          			"
                'strSql += vbCrLf + "	                UPDATE TT  SET MELTPER=((ISNULL(@AMOUNT,0)/ISNULL(@GRSWT,0))/93.5) FROM temptabledb..tempPUSR_VIEW TT"
                'strSql += vbCrLf + "         			WHERE ISNULL(@GRSWT,0) <> 0"
                'strSql += vbCrLf + "         		END"
                'strSql += vbCrLf + "         	END        "
                'strSql += vbCrLf + "          END"
                'strSql += vbCrLf + "         CLOSE I_CUR"
                'strSql += vbCrLf + "         DEALLOCATE I_CUR"
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            End If
        End If
        'strSql = "  UPDATE temptabledb..tempPUSR_VIEW SET TOUCH= ((ISNULL(AMOUNT,0)/ISNULL(GRSWT,0))/((BOARDRATE / 93) * 100))*100 WHERE ISNULL(GRSWT,0) <> 0 AND ISNULL(BOARDRATE,0) <> 0 and "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        If rbtDetailed.Checked And (ChkBillSum.Checked Or chkNodewiseSummary.Checked = True) Then
            strSql = " SELECT "
            If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then
                strSql += vbCrLf + " CONVERT(BIT,0) MARK,"
            End If
            strSql += vbCrLf + " CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(400),NULL)AS PARTICULAR"
            strSql += vbCrLf + " ,TRANNO,TRANDATE,CASHCOUNTER"
            strSql += vbCrLf + " ,NULL SUBITEM,NULL DESCRIPT,NULL ITEMTYPE"
            strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(LESSWT)LESSWT"
            strSql += vbCrLf + " ,SUM(DUSTWT)DUSTWT,NULL WASTPER,SUM(WASTAGE)WASTAGE"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT,NULL PURITY,SUM(PUREWT)PUREWT"
            strSql += vbCrLf + " ,NULL RATE,SUM(STNWT)STNWT,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + " ,SUM(IGST)IGST,SUM(CGST)CGST,SUM(SGST)SGST"
            strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT"
            strSql += vbCrLf + " ,NULL TABLECODE,NULL NODE,NULL REFNO,NULL REFDATE,EMPNAME,METAL,CATEGORY,TRANTYPE,NULL SNO"
            strSql += vbCrLf + " ,EMPID,ESTIMATENO,SREFNO,AREFNO,NULL BOARDRATE,NULL TOUCH,CONVERT(VARCHAR(2),NULL) COLHEAD"
            'strSql += vbCrLf + " ,MAKEWISE"
            strSql += vbCrLf + " ,NULL MAKEWISE,SRESULT" 'ON 14-09-2018
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPPUSR_VIEW_NEW"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPPUSR_VIEW"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,CASHCOUNTER,EMPNAME,METAL,CATEGORY,TRANTYPE"
            strSql += vbCrLf + " ,EMPID,ESTIMATENO,SREFNO,AREFNO,SRESULT" 'MAKEWISE
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        strSql = " UPDATE TEMPTABLEDB..TEMPPUSR_VIEW SET"
        strSql += vbCrLf + " PCS = CASE WHEN PCS = 0 THEN NULL ELSE PCS END"
        strSql += vbCrLf + " ,GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
        strSql += vbCrLf + " ,LESSWT = CASE WHEN LESSWT = 0 THEN NULL ELSE LESSWT END"
        strSql += vbCrLf + " ,DUSTWT = CASE WHEN DUSTWT = 0 THEN NULL ELSE DUSTWT END"
        strSql += vbCrLf + " ,WASTPER = CASE WHEN WASTPER = 0 THEN NULL ELSE WASTPER END"
        strSql += vbCrLf + " ,WASTAGE = CASE WHEN WASTAGE = 0 THEN NULL ELSE WASTAGE END"
        strSql += vbCrLf + " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
        strSql += vbCrLf + " ,PUREWT = CASE WHEN PUREWT = 0 THEN NULL ELSE PUREWT END"
        strSql += vbCrLf + " ,RATE = CASE WHEN RATE = 0 THEN NULL ELSE RATE END"
        strSql += vbCrLf + " ,STNWT = CASE WHEN STNWT = 0 THEN NULL ELSE STNWT END"
        strSql += vbCrLf + " ,DIAWT = CASE WHEN DIAWT = 0 THEN NULL ELSE DIAWT END"
        strSql += vbCrLf + " ,AMOUNT = CASE WHEN AMOUNT = 0 THEN NULL ELSE AMOUNT END"
        strSql += vbCrLf + " ,IGST = CASE WHEN IGST = 0 THEN NULL ELSE IGST END"
        strSql += vbCrLf + " ,CGST = CASE WHEN CGST = 0 THEN NULL ELSE CGST END"
        strSql += vbCrLf + " ,SGST = CASE WHEN SGST = 0 THEN NULL ELSE SGST END"
        strSql += vbCrLf + " ,PURITY = CASE WHEN PURITY = 0 THEN NULL ELSE PURITY END"
        strSql += vbCrLf + " ,REFNO = CASE WHEN REFNO = 0 THEN NULL ELSE REFNO END"
        strSql += vbCrLf + " ,SREFNO = CASE WHEN SREFNO = 0 THEN NULL ELSE SREFNO END"
        strSql += vbCrLf + " ,AREFNO = CASE WHEN AREFNO = 0 THEN NULL ELSE AREFNO END"
        strSql += vbCrLf + " ,ESTIMATENO = CASE WHEN ESTIMATENO = 0 THEN NULL ELSE ESTIMATENO END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


        'If chkTotalSummay.Checked Then
        '    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPPUSR_VIEW("
        '    strSql += vbCrLf + " SUBITEM,SNAME,METAL,CATEGORY,TRANTYPE,GRSWT,NETWT,AMOUNT,SNO)"
        '    strSql += vbCrLf + " SELECT METAL,TRANTYPE,'SUMMARY' METAL,METAL CATEGORY,'SUMMARY' TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,20000 + ROW_NUMBER() OVER (ORDER BY TRANTYPE,METAL)"
        '    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPPUSR_VIEW GROUP BY TRANTYPE,METAL"
        '    strSql += vbCrLf + " HAVING SUM(GRSWT)<>0"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        'End If


        If MELPUTGROUPBY = True Then
            strSql = " UPDATE TEMPTABLEDB..TEMPPUSR_VIEW SET TRANTYPE ='GOLD WITH DIAMOND' WHERE ISNULL(DIAWT,0) <>'0.0'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " update TEMPTABLEDB..TEMPPUSR_VIEW SET TRANTYPE=ITEMTYPE  where PURITY ='100.00'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        If rbtDetailed.Checked And (ChkBillSum.Checked Or chkNodewiseSummary.Checked = True) Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPPUSR_VIEW_NEW ORDER BY SRESULT,TRANDATE,TRANNO"
        Else
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPPUSR_VIEW ORDER BY SRESULT,TRANDATE,TRANNO"
        End If

        Dim DtGrid As New DataTable
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtGrid)

        If Not DtGrid.Rows.Count > 0 Then
            Dim drr As DataRow
            drr = DtGrid.NewRow
            drr.Item("PARTICULAR") = "NO TRANSACTION AVAILABLE"
            DtGrid.Rows.Add(drr)
            drr.AcceptChanges()
            gridView.DataSource = Nothing
            With gridView
                .DataSource = DtGrid
                .Columns("PUREWT").Visible = chkPureWtBased.Checked
                .Columns("SLNO").Visible = rbtDetailed.Checked
                .Columns("CATEGORY").Visible = False
                .Columns("DESCRIPT").Visible = True
                .Columns("SRESULT").Visible = True
                .Columns("METAL").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TRANTYPE").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("SNO").Visible = False
                .Columns("BOARDRATE").Visible = False
                .Columns("EMPID").Visible = False
                .Columns("PARTICULAR").Width = 200
            End With
            tabView.Show()
            GoTo FINALVIEW
            '  MsgBox("Record not found", MsgBoxStyle.Information)
            'Exit Sub
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        gridView.DataSource = Nothing
        tabView.Show()
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridView, DtGrid)
        ObjGrouper.pColumns_Group.Add("TRANTYPE")
        ObjGrouper.pIssSort = False
        For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
        Next
        ObjGrouper.pColumns_Sum.Add("PCS")
        ObjGrouper.pColumns_Sum.Add("GRSWT")
        ObjGrouper.pColumns_Sum.Add("LESSWT")
        ObjGrouper.pColumns_Sum.Add("DUSTWT")
        ObjGrouper.pColumns_Sum.Add("WASTAGE")
        ObjGrouper.pColumns_Sum.Add("NETWT")
        ObjGrouper.pColumns_Sum.Add("PUREWT")
        ObjGrouper.pColumns_Sum.Add("STNWT")
        ObjGrouper.pColumns_Sum.Add("DIAWT")
        ObjGrouper.pColumns_Sum.Add("AMOUNT")
        ObjGrouper.pColumns_Sum.Add("IGST")
        ObjGrouper.pColumns_Sum.Add("CGST")
        ObjGrouper.pColumns_Sum.Add("SGST")
        If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False Then
            If ChkBillSum.Checked = False And chkNodewiseSummary.Checked = False Then ObjGrouper.pColumns_Sum.Add("MELTWT")

            'Dim T3 As Decimal = (ObjGrouper.pColumns_Sum.Add("MELTWT") / ObjGrouper.pColumns_Sum.Add("GRSWT"))
            'ObjGrouper.pColumns_Sum.Add((ObjGrouper.pColumns_Sum.Add("MELTWT") / ObjGrouper.pColumns_Sum.Add("GRSWT")) * 100)
        End If
        ObjGrouper.pColumns_Sum.Add("BOARDRATE")

        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pIdentityColName = "SLNO"
        If ChkLstGroupBy.GetItemChecked(1) Then
            ''CATEGORY 
            ObjGrouper.pColName_ReplaceWithParticular = "TRANDATE"
        Else
            ObjGrouper.pColName_ReplaceWithParticular = "DESCRIPT"
        End If
        'calno 281112
        If rbtEstimation.Checked = True Then
            ObjGrouper.pColumns_Sort = "ESTIMATENO"
        ElseIf rbtEmpid.Checked = True Then
            ObjGrouper.pColumns_Sort = "EMPNAME"
        Else
            ObjGrouper.pColumns_Sort = "TRANDATE,TRANNO"
        End If

        ObjGrouper.GroupDgv() '''''''''GroupSummery

        Dim brdrate As Double = 0
        Dim touchval As Double = 0
        For i As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S1" Or gridView.Rows(i).Cells("COLHEAD").Value.ToString = "G" Or gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S" Then
                brdrate = (Val(gridView.Rows(i).Cells("BOARDRATE").Value.ToString) / (Val(DtGrid.Rows.Count)))
                If brdrate <= 0 Then Continue For
                touchval = ((Val(gridView.Rows(i).Cells("AMOUNT").Value.ToString) / (IIf(Val(gridView.Rows(i).Cells("GRSWT").Value.ToString) = 0, 1, Val(gridView.Rows(i).Cells("GRSWT").Value.ToString)))) / (brdrate)) * 100
                gridView.Rows(i).Cells("TOUCH").Value = touchval.ToString("0.00")
            End If
        Next
        If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False And ChkBillSum.Checked = False And chkNodewiseSummary.Checked = False Then
            Dim mettol As Double = 0
            For i As Integer = 0 To gridView.Rows.Count - 1
                If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S1" Then
                    mettol = (Val(gridView.Rows(i).Cells("MELTWT").Value.ToString) / gridView.Rows(i).Cells("GRSWT").Value.ToString) * 100
                    gridView.Rows(i).Cells("MELTPER").Value = mettol.ToString("0.00")
                End If
                If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S2" Then
                    mettol = (Val(gridView.Rows(i).Cells("MELTWT").Value.ToString) / gridView.Rows(i).Cells("GRSWT").Value.ToString) * 100
                    gridView.Rows(i).Cells("MELTPER").Value = mettol.ToString("0.00")
                End If
                If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "S" Then
                    mettol = (Val(gridView.Rows(i).Cells("MELTWT").Value.ToString) / gridView.Rows(i).Cells("GRSWT").Value.ToString) * 100
                    gridView.Rows(i).Cells("MELTPER").Value = mettol.ToString("0.00")
                End If
                If gridView.Rows(i).Cells("COLHEAD").Value.ToString = "G" Then
                    mettol = (Val(gridView.Rows(i).Cells("MELTWT").Value.ToString) / gridView.Rows(i).Cells("GRSWT").Value.ToString) * 100
                    gridView.Rows(i).Cells("MELTPER").Value = mettol.ToString("0.00")
                End If
            Next
        End If

        Dim dv As DataView
        dv = CType(gridView.DataSource, DataTable).DefaultView
        If rbtSummary.Checked Then
            Dim filt As String = "NOT (COLHEAD = '' OR COLHEAD IS NULL"
            If ChkLstGroupBy.CheckedItems.Count = 0 Then
                filt += " OR COLHEAD = 'T'"
            ElseIf ChkLstGroupBy.CheckedItems.Count = 1 Then
                filt += " OR COLHEAD = 'T1'"
            ElseIf ChkLstGroupBy.CheckedItems.Count = 2 Then
                filt += " OR COLHEAD = 'T2'"
            ElseIf ChkLstGroupBy.CheckedItems.Count = 3 Then
                filt += " OR COLHEAD = 'T3'"
            End If
            filt += ")"
            dv.RowFilter = filt
            'FillGridGroupStyle(gridView, "PARTICULAR")
        End If
        '591
        If Not chkGrtotal.Checked Then
            If chkTotalSummay.Checked = False Then
                dv.RowFilter = "PARTICULAR <>'GRAND TOTAL'"
            End If
        End If




        '591
        If ChkPurchaseMelting.Checked = True Then
            Dim ind As Integer = gridView.RowCount - 1
            CType(gridView.DataSource, DataTable).Rows.Add()
            CType(gridView.DataSource, DataTable).Rows.Add()

            Dim value As Double = Nothing

            value = (Val(gridView.Rows(ind).Cells("GRSWT").Value.ToString) - Val(gridView.Rows(ind).Cells("WASTAGE").Value.ToString)) * (92 / 100)
            value = (value / Val(gridView.Rows(ind).Cells("GRSWT").Value.ToString)) * 100

            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "MELTING RESULT"
            gridView.Rows(gridView.RowCount - 1).Cells("PURITY").Value = Math.Round(value, 2)
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        End If

        'gridView.DataSource = DtGrid
        For Each gv As DataGridViewRow In gridView.Rows
            If gv.Cells("COLHEAD").Value.ToString = "S1" Or gv.Cells("COLHEAD").Value.ToString = "S2" Or gv.Cells("COLHEAD").Value.ToString = "S3" Then
                gv.Cells("TOUCH").Value = DBNull.Value
                gv.Cells("COLHEAD").Value = "G"
            End If
        Next

        If chkTotalSummay.Checked Then
            strSql = vbCrLf + "  SELECT TRANTYPE PARTICULAR,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT FROM TEMPTABLEDB..TEMPPUSR_VIEW AS T GROUP BY TRANTYPE,SRESULT HAVING SUM(GRSWT)<>0  ORDER BY SRESULT "
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtsum As New DataTable
            da.Fill(dtsum)
            For Each dr As DataRow In dtsum.Rows
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = dr("PARTICULAR").ToString
                gridView.Rows(gridView.RowCount - 1).Cells("GRSWT").Value = dr("GRSWT")
                gridView.Rows(gridView.RowCount - 1).Cells("NETWT").Value = dr("NETWT")
                gridView.Rows(gridView.RowCount - 1).Cells("AMOUNT").Value = dr("AMOUNT")
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "S3"
            Next
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " LEFT(ISNULL(METAL,' '),1)+'-'+ISNULL(CATEGORY,' ')+''+(CASE WHEN TRANTYPE='PURCHASE' THEN ''WHEN TRANTYPE='PARTLY SALES' THEN '-PS'WHEN TRANTYPE='SALES RETURN' THEN '-SR' ELSE '' END)PARTICULAR,"
            strSql += vbCrLf + " METAL,TRANTYPE,'SUMMARY' METAL,CATEGORY CATEGORY,'SUMMARY' TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,20000 + ROW_NUMBER() OVER (ORDER BY TRANTYPE,METAL)SNO"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPPUSR_VIEW AS T "
            strSql += vbCrLf + " WHERE TRANTYPE<>'PARTLY SALES'"
            strSql += vbCrLf + " GROUP BY TRANTYPE,METAL,CATEGORY"
            strSql += vbCrLf + " HAVING SUM(GRSWT)<>0 "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " LEFT(ISNULL(METAL,' '),1)+'-PARTLY SALES'PARTICULAR,"
            strSql += vbCrLf + " METAL,TRANTYPE,'SUMMARY' METAL,METAL CATEGORY,'SUMMARY' TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,20000 + ROW_NUMBER() OVER (ORDER BY TRANTYPE,METAL)SNO"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPPUSR_VIEW AS T "
            strSql += vbCrLf + " WHERE TRANTYPE='PARTLY SALES'"
            strSql += vbCrLf + " GROUP BY TRANTYPE,METAL"
            strSql += vbCrLf + " HAVING SUM(GRSWT)<>0 "
            strSql += vbCrLf + " ORDER BY T.METAL,CATEGORY"
            da = New OleDbDataAdapter(strSql, cn)
            dtsum = New DataTable
            da.Fill(dtsum)
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "SUMMARY"
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
            For Each dr As DataRow In dtsum.Rows
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = dr("PARTICULAR").ToString
                gridView.Rows(gridView.RowCount - 1).Cells("GRSWT").Value = dr("GRSWT")
                gridView.Rows(gridView.RowCount - 1).Cells("NETWT").Value = dr("NETWT")
                gridView.Rows(gridView.RowCount - 1).Cells("AMOUNT").Value = dr("AMOUNT")
            Next
            CType(gridView.DataSource, DataTable).Rows.Add()
            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "TOTAL"
            gridView.Rows(gridView.RowCount - 1).Cells("GRSWT").Value = Val(dtsum.Compute("SUM(GRSWT)", Nothing).ToString)
            gridView.Rows(gridView.RowCount - 1).Cells("NETWT").Value = Val(dtsum.Compute("SUM(NETWT)", Nothing).ToString)
            gridView.Rows(gridView.RowCount - 1).Cells("AMOUNT").Value = Val(dtsum.Compute("SUM(AMOUNT)", Nothing).ToString)
            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
        End If

        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(cnt).Visible = False
            Next
            If ChkLstGroupBy.GetItemChecked(1) Then
                ''CATEGORY 
                .Columns("TRANDATE").Visible = False
                ObjGrouper.pColName_ReplaceWithParticular = "TRANDATE"
            End If
            .Columns("PUREWT").Visible = chkPureWtBased.Checked
            .Columns("SLNO").Visible = rbtDetailed.Checked
            .Columns("ESTIMATENO").HeaderText = "ESTNO"
            .Columns("CATEGORY").Visible = False
            .Columns("DESCRIPT").Visible = True
            .Columns("METAL").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("BOARDRATE").Visible = False
            .Columns("EMPID").Visible = False
            .Columns("SRESULT").Visible = False
            .Columns("DUSTWT").HeaderText = "DSTWT"
            .Columns("WASTPER").HeaderText = "WAST%"
            .Columns("WASTAGE").HeaderText = "WAST"
            .Columns("PURITY").HeaderText = "PURE"



            .Columns("SLNO").Width = 40
            If ChkPurchase.Checked And ChkPartlySales.Checked = False And ChkSalesReturn.Checked = False Then .Columns("MARK").Width = 40
            .Columns("PARTICULAR").Width = 170
            If chkItem.Checked Then
                .Columns("PARTICULAR").HeaderText = "ITEM NAME"
            ElseIf chkItem.Checked = False Then
                .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            End If
            '.Columns("TRANNO").Width = 60
            '.Columns("TRANDATE").Width = 80
            '.Columns("ITEMTYPE").Width = 60
            '.Columns("PURITY").Width = 60
            '.Columns("PCS").Width = 50
            '.Columns("GRSWT").Width = 80
            '.Columns("LESSWT").Width = 80
            '.Columns("WASTAGE").Width = 80
            '.Columns("WASTPER").Width = 70
            '.Columns("NETWT").Width = 80
            '.Columns("PUREWT").Width = 80
            '.Columns("RATE").Width = 80
            '.Columns("AMOUNT").Width = 100
            '.Columns("REFNO").Width = 60
            '.Columns("REFDATE").Width = 80
            '.Columns("EMPNAME").Width = 110
            .Columns("IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DUSTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AREFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("TOUCH") Then .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("SREFNO") Then .Columns("SREFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("ESTIMATENO") Then .Columns("ESTIMATENO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("EMPID") Then .Columns("EMPID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If ChkPurchase.Checked And ChkSalesReturn.Checked = False And ChkPartlySales.Checked = False And ChkBillSum.Checked = False And chkNodewiseSummary.Checked = False Then
                .Columns("MELTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MELTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MELTPER").HeaderText = "MELT%"
                .Columns("MELTWT").HeaderText = "METWT"
                .Columns("MELTPER").DefaultCellStyle.Format = "0.00"
            End If
            .Columns("SREFNO").HeaderText = "SALES NO"
            .Columns("AREFNO").HeaderText = "ADV NO"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
            .Columns("DUSTWT").DefaultCellStyle.Format = "0.000"
            .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            .Columns("WASTPER").DefaultCellStyle.Format = "0.00"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("PUREWT").DefaultCellStyle.Format = "0.000"
            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"

            .Columns("REFNO").Visible = ChkSalesReturn.Checked
            .Columns("REFDATE").Visible = ChkSalesReturn.Checked
            .Columns("IGST").Visible = ChkWithGST.Checked
            .Columns("CGST").Visible = ChkWithGST.Checked
            .Columns("SGST").Visible = ChkWithGST.Checked
        End With
        'FillGridGroupStyle_KeyNoWise(gridView)

FINALVIEW:
        Dim strTitle As String = Nothing
        If ChkPurchase.Checked = True Then
            strTitle += "PURCHASE,"
        End If
        If ChkSalesReturn.Checked = True Then
            strTitle += "SALES RETURN,"
        End If
        If ChkPartlySales.Checked = True Then
            strTitle += "PARTLY SALES,"
        End If
        If chkOrdAdv.Checked = True Then
            strTitle += "ORDER ADVANCE,"
        End If
        strTitle = Mid(strTitle, 1, Len(strTitle) - 1)
        strTitle += vbCrLf + " REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""

        If rbtPurchaseOnly.Checked Then
            strTitle += vbCrLf + "(CASH PURCHASE)"
        ElseIf rbtSalesPurchase.Checked Then
            strTitle += vbCrLf + "(EXCHANGE)"
        End If
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strTitle += " FOR " & chkCmbMetal.Text & ""
        End If
        If rbtOther.Checked = True Then
            strTitle += " FOR OTHER MAKE"
        End If
        If rbtOWN.Checked = True Then
            strTitle += " FOR OWN MAKE"
        End If
        strTitle += IIf(cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "", " :" & cmbCostCentre.Text, "")

        Dim Nodename As String = ""
        If chkLstNodeId.Items.Count > 0 And chkLstNodeId.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstNodeId.Items.Count - 1
                If chkLstNodeId.GetItemChecked(CNT) = True Then
                    Nodename += "" & chkLstNodeId.Items(CNT) + ","
                End If
            Next
            If Nodename <> "" Then Nodename = " :" + Mid(Nodename, 1, Len(Nodename) - 1)
        End If
        'strTitle = strTitle + vbCrLf + IIf(Nodename <> "", " FOR SYSTEMID" & Nodename, Nodename)
        lblTitle.Text = strTitle
        ResizeToolStripMenuItem_Click(Me, New EventArgs)
        tabMain.SelectedTab = tabView
        FillGridGroupStyle(gridView, "PARTICULAR")
        gridView.Focus()
    End Sub
    Public Sub FillGridGroupStyle(ByVal gridview As DataGridView, Optional ByVal ParticularColName As String = "")
        Dim ind As Integer = 0
        If ParticularColName <> "" Then
            If gridview.Columns.Contains(ParticularColName) Then
                ind = gridview.Columns(ParticularColName).Index
            End If
        End If
        For Each dgvRow As DataGridViewRow In gridview.Rows
            If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
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
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S1" Then
                dgvRow.DefaultCellStyle.BackColor = Color.Beige
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S2" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGreen
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.MistyRose
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "G" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            End If
        Next
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        ResizeToolStripMenuItem.Checked = True
        NewReport()
        If Authorize Then Prop_Sets()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If IS40COLCLSSTKPRINT Then
            If MsgBox("Do you want to print on 60 Col. Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then IS40COLCLSSTKPRINT = False
        End If

        If IS40COLCLSSTKPRINT Then
            PRINT40COLSUMMARY()
        Else
            If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub

    Private Sub PRINT40COLSUMMARY()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim write As StreamWriter
        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        Dim lineprn As String = Space(60)
        write.WriteLine("")
        write.WriteLine("")
        write.WriteLine("")
        write.WriteLine("------------------------------------------------")
        write.WriteLine("PURCHASE REPORT DATE FROM " + Mid(dtpFrom.Text, 1, 6) + Mid(dtpFrom.Text, 9, 10) + " TO " + Mid(dtpTo.Text, 1, 6) + Mid(dtpTo.Text, 9, 10))
        write.WriteLine("------------------------------------------------")
        Dim sno As String = "SNO"
        Dim Bno As String = "B.NO"
        Dim SName As String = "SNAME "
        Dim Grswt As String = "GRS WT"
        Dim Lesswt As String = "LESS "
        Dim Purity As String = "%    "
        Dim Avg As String = "Avg  "
        sno = RSet(sno, 3)
        Bno = RSet(Bno, 5)
        SName = LSet(SName, 5)
        Grswt = RSet(Grswt, 9)
        Lesswt = RSet(Lesswt, 6)
        Purity = RSet(Purity, 7)
        Avg = RSet(Avg, 6)
        write.WriteLine(sno & Space(1) & Bno & Space(1) & SName & Space(1) & Grswt & Space(1) & Lesswt & Space(1) & Purity & Space(1) & Avg)
        write.WriteLine("------------------------------------------------")
        For i As Integer = 0 To gridView.Rows.Count - 1
            With gridView.Rows(i)
                If .Cells("COLHEAD").Value.ToString = "G" Or .Cells("COLHEAD").Value.ToString = "T" Then Continue For
                Dim prndesc As String = .Cells("PARTICULAR").Value.ToString
                Dim prnsno As String = .Cells("SLNO").Value.ToString
                Dim prnBno As String = .Cells("TRANNO").Value.ToString
                Dim prnSName As String = .Cells("SNAME").Value.ToString
                Dim prnGrswt As String = .Cells("GRSWT").Value.ToString
                Dim prnLesswt As String = .Cells("LESSWT").Value.ToString
                Dim prnPurity As String = .Cells("PURITY").Value.ToString
                Dim prnAvg As String = "0000.00"

                If prndesc.Contains("TOTAL") Then
                    prnSName = "TOTAL"
                    prnAvg = Format(Math.Round(Val(.Cells("AMOUNT").Value.ToString) / Val(.Cells("GRSWT").Value.ToString), 2), "0.00")
                Else
                    prnAvg = Format(Math.Round(Val(.Cells("RATE").Value.ToString) * Val(.Cells("PURITY").Value.ToString) / 100, 2), "0.00")
                End If

                prnsno = RSet(prnsno, 3)
                prnBno = RSet(prnBno, 5)
                prnSName = LSet(prnSName, 5)
                prnGrswt = RSet(prnGrswt, 9)
                prnLesswt = RSet(prnLesswt, 6)
                prnPurity = RSet(prnPurity, 7)
                prnAvg = RSet(prnAvg, 7)

                If prndesc.Contains("TOTAL") Then write.WriteLine("------------------------------------------------")
                write.WriteLine(prnsno & Space(1) & prnBno & Space(1) & prnSName & Space(1) & prnGrswt & Space(1) & prnLesswt & Space(1) & prnPurity & Space(1) & prnAvg)
                If prndesc.Contains("TOTAL") Then write.WriteLine("------------------------------------------------")
            End With
        Next
        For i As Integer = 0 To 4
            write.WriteLine()
        Next
        write.Close()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.BAT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.BAT")
        End If
        Dim writebat As StreamWriter

        Dim PrnName As String = ""
        Dim CondId As String = ""
        Try
            CondId = "'AGOLDREPORT40COLUMN" & Environ("NODE-ID").ToString & "'"
        Catch ex As Exception
            MsgBox("Set Node-Id", MsgBoxStyle.Information)
            Exit Sub
        End Try
        writebat = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.BAT")
        strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
        Dim dt As New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count <> 0 Then
            PrnName = dt.Rows(0).Item("CTLTEXT").ToString
        Else
            PrnName = "PRN"
        End If
        writebat.WriteLine("TYPE " & Application.StartupPath & "\SUMMARYPRINT.TXT>" & PrnName)
        writebat.Flush()
        writebat.Close()
        Shell(Application.StartupPath & "\SUMMARYPRINT.BAT")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            dtPurchase.Clear()
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            funcAddItemName()
            'txtNodeID.Text = ""
            ' rbtBoth.Checked = True
            'rbtTypeWise.Checked = True
            rbtMake.Checked = True
            pnlMake.Enabled = False
            gridView.DataSource = Nothing
            lblTitle.Text = "TITLE"
            If cmbCostCentre.Enabled Then
                cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            Else
                cmbCostCentre.Text = ""
            End If
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
            Prop_Gets()
            rbtDetailed_CheckedChanged(Me, New EventArgs)
            dtpFrom.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub frmPurchaseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Function funcAddMetalName() As Integer
        Try
            strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
            strSql += " ORDER BY RESULT,METALNAME"
            dtMetal = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMetal)
            chkCmbMetal.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")

            strSql = " SELECT 'ALL' CASHNAME,'ALL' CASHID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CASHNAME,CASHID,2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER "
            strSql += " ORDER BY RESULT,CASHNAME"
            Dim dtCach As DataTable = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCach)
            chkcmbCash.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCash, dtCach, "CASHNAME", , "ALL")

            strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY "
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            strSql += " ORDER BY RESULT,CATNAME"
            Dim dtCategory As DataTable = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCategory)
            chkcmbCategory.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtCategory, "CATNAME", , "ALL")
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcAddItemName() As Integer
        Try

            strSql = " SELECT 'ALL' ITEMNAME,0 ITEMID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ITEMNAME,ITEMID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            If chkcmbCategory.Text <> "ALL" And chkcmbCategory.Text <> "" Then
                strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & "))"
            End If
            strSql += " ORDER BY RESULT,ITEMNAME"
            Dim dtITEM As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtITEM)
            chkCmbItem.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtITEM, "ITEMNAME", , "ALL")

            strSql = " SELECT 'ALL' CASHNAME,'ALL' CASHID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CASHNAME,CASHID,2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER "
            strSql += " ORDER BY RESULT,CASHNAME"
            Dim dtCach As DataTable = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCach)
            chkcmbCash.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCash, dtCach, "CASHNAME", , "ALL")

            strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY "
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            strSql += " ORDER BY RESULT,CATNAME"
            Dim dtCategory As DataTable = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCategory)
            chkcmbCategory.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCategory, dtCategory, "CATNAME", , "ALL")
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        Try
            With gridView
                .Columns("TRANTYPE").Visible = False
                If Not rbtBillNo.Checked = True Then
                    With .Columns("PARTICULAR")
                        If rbtTypeWise.Checked = True Then
                            .HeaderText = "TYPE"
                        ElseIf rbtBillDateWise.Checked = True Then
                            .HeaderText = "BILLDATE"
                        End If
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        ' .Width = 120
                    End With
                ElseIf rbtBillNo.Checked = True Then
                    .Columns("PARTICULAR").HeaderText = "BILLNO"
                End If
                With .Columns("BILLNO")
                    If rbtBillDateWise.Checked = True Or rbtBillNo.Checked = True Then
                        .Visible = False
                    Else
                        .Visible = True
                    End If
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    '.Width = 80
                End With
                With .Columns("PURITY")
                    ' .Width = 70
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With



                With .Columns("BILLDATE")
                    If rbtTypeWise.Checked = True Or rbtBillDateWise.Checked = True Then
                        .Visible = False
                    Else
                        .Visible = True
                    End If
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    ' .Width = 80
                End With
                With .Columns("ITEMNAME")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    ' .Width = 100
                    .HeaderText = "PARTICULAR"
                End With
                With .Columns("PCS")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    '.Width = 40
                End With
                With .Columns("GRSWT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Width = 80
                End With
                With .Columns("STONEWT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .HeaderText = "STONEWT/ DUSTWT"
                    ' .Width = 80
                End With
                With .Columns("WASTAGE")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Width = 80
                End With
                With .Columns("NETWT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Width = 80
                End With
                With .Columns("RATE")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    '  .Width = 80
                End With
                With .Columns("AMOUNT")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Width = 100
                End With
                With .Columns("EMPNAME")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ' .Width = 100
                End With
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub



    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            ResizeToolStripMenuItem.Checked = True
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPurchaseReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPurchaseReport_Properties), IIf(Authorize = False, True, False))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        'SetChecked_CheckedList(chkcmbCash, obj.p_chkCmb, "ALL")
        'SetChecked_CheckedList(chkcmbCategory, obj.p_chkCmbMetal, "ALL")


        chkNodeSelectAll.Checked = obj.p_chkLstNodeIdSelectAll
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, Nothing)
        chkPureWtBased.Checked = obj.p_chkPureWtBased
        ChkPurchase.Checked = obj.p_chkPurchase
        ChkSalesReturn.Checked = obj.p_chkSalesReturn
        ChkPartlySales.Checked = obj.p_ChkPartlySales
        chkGroupByItemType.Checked = obj.p_chkGroupByItemType
        SetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy, "")
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        rbtBothMU.Checked = obj.p_rbtBothMU
        rbtMarked.Checked = obj.p_rbtMarked
        rbtUnmarked.Checked = obj.p_rbtUnmarked
        chkItem.Checked = obj.p_chkItemName
        chkGrtotal.Checked = obj.p_chkGrtotal
        chkOrdAdv.Checked = obj.p_chkOrdAdv
        ChkPurchaseMelting.Checked = obj.p_chkMelting
        chkSubitem.Checked = obj.p_chkSubItemName
        rbtDefault.Checked = obj.p_rbtDefault
        rbtEstimation.Checked = obj.p_rbtEstimation
        rbtEmpid.Checked = obj.p_rbtEmp
        ChkBillSum.Checked = obj.p_chkBillSum
        chkTotalSummay.Checked = obj.p_chkTotalSummay
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPurchaseReport_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        'obj.p_chkCashCounterSelectAll = chkCashCounterSelectAll.Checked
        'GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        obj.p_chkLstNodeIdSelectAll = chkNodeSelectAll.Checked
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_chkPureWtBased = chkPureWtBased.Checked
        obj.p_chkPurchase = ChkPurchase.Checked
        obj.p_chkSalesReturn = ChkSalesReturn.Checked
        obj.p_ChkPartlySales = ChkPartlySales.Checked
        obj.p_chkGroupByItemType = chkGroupByItemType.Checked
        GetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtBothMU = rbtBothMU.Checked
        obj.p_rbtMarked = rbtMarked.Checked
        obj.p_rbtUnmarked = rbtUnmarked.Checked
        obj.p_chkItemName = chkItem.Checked
        obj.p_chkGrtotal = chkGrtotal.Checked
        obj.p_chkOrdAdv = chkOrdAdv.Checked
        obj.p_chkMelting = ChkPurchaseMelting.Checked
        obj.p_chkSubItemName = chkSubitem.Checked
        obj.p_rbtDefault = rbtDefault.Checked
        obj.p_rbtEstimation = rbtEstimation.Checked
        obj.p_rbtEmp = rbtEmpid.Checked
        obj.p_chkBillSum = ChkBillSum.Checked
        obj.p_chkTotalSummay = chkTotalSummay.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPurchaseReport_Properties), Save)
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick
        If ChkPurchase.Checked = True And ChkPartlySales.Checked = False And ChkSalesReturn.Checked = False Then
            If e.ColumnIndex = 0 Then
                If gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString() = DBNull.Value.ToString() Or gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString() = "" Then
                    gridView.Rows(e.RowIndex).Cells("MARK").Value = Not gridView.Rows(e.RowIndex).Cells("MARK").Value
                    strSql = " UPDATE " & cnStockDb & "..RECEIPT SET MARK=" & IIf(gridView.Rows(e.RowIndex).Cells("MARK").Value = True, "1", "0") & " WHERE SNO='" & gridView.Rows(e.RowIndex).Cells("SNO").Value.ToString() & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If
    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit
        If Not (gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString() = DBNull.Value.ToString() Or gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString() = "") Or e.ColumnIndex > 0 Then
            e.Cancel = True
        End If
    End Sub


    Private Sub ChkPurchase_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPurchase.CheckedChanged
        If Authorize = False Then Exit Sub
        If ChkPurchase.Checked = True Then
            PnlMark.Enabled = True
            pnlMake.Enabled = True
        Else
            PnlMark.Enabled = False
            pnlMake.Enabled = False
        End If
    End Sub

    Private Sub chkItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItem.CheckedChanged
        If Authorize Then chkSubitem.Enabled = chkItem.Checked
    End Sub

    Private Sub ProcAddCashCounter()
        'chkLstCashCounter.Items.Clear()
        'strSql = "SELECT CASHID, CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        'da = New OleDbDataAdapter(strSql, cn)
        'dtCashCounter = New DataTable
        'da.Fill(dtCashCounter)
        'If dtCashCounter.Rows.Count > 0 Then
        '    For cnt As Integer = 0 To dtCashCounter.Rows.Count - 1
        '        chkLstCashCounter.Items.Add(dtCashCounter.Rows(cnt).Item(1).ToString)
        '    Next
        'End If
    End Sub
    Private Sub ProcAddNodeId()
        Dim dt As DataTable
        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN  "
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE  "
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL")
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Enabled = True
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub
    'Private Sub chkCashCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SetChecked_CheckedList(chkLstCashCounter, chkCashCounterSelectAll.Checked)
    'End Sub

    Private Sub chkNodeSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNodeSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstNodeId, chkNodeSelectAll.Checked)
    End Sub
    Private Sub rbtSummary_CheckedChanged(sender As Object, e As EventArgs) Handles rbtSummary.CheckedChanged
        chkNodewiseSummary.Checked = False
    End Sub
    Private Sub rbtDetailed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        ChkBillSum.Visible = rbtDetailed.Checked
        chkNodewiseSummary.Visible = rbtDetailed.Checked
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub


    Private Sub lblMetalName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMetalName.Click

    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub
    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
    Private Sub cmbCostCentre_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCostCentre.SelectedIndexChanged

    End Sub
    Private Sub ChkLstGroupBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkLstGroupBy.SelectedIndexChanged

    End Sub
    Private Sub chkPureWtBased_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPureWtBased.CheckedChanged

    End Sub
    Private Sub chkCmbMetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbMetal.SelectedIndexChanged
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            funcAddItemName()
        End If
    End Sub
    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint

    End Sub
    Private Sub pnlMake_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlMake.Paint

    End Sub
    Private Sub Panel4_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel4.Paint

    End Sub
    Private Sub chkLstCashCounter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub chkLstNodeId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstNodeId.SelectedIndexChanged

    End Sub

    Private Sub chkCmbMetal_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkCmbMetal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcAddItemName()
        End If
    End Sub

    Private Sub chkcmbCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbCategory.SelectedIndexChanged
        strSql = " SELECT 'ALL' ITEMNAME,0 ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,ITEMID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        If chkcmbCategory.Text <> "ALL" And chkcmbCategory.Text <> "" Then
            strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        Dim dtITEM As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtITEM)
        chkCmbItem.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtITEM, "ITEMNAME", , "ALL")
    End Sub

    Private Sub chkcmbCategory_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcmbCategory.KeyDown
        If e.KeyCode = Keys.Enter Then
            chkcmbCategory_SelectedIndexChanged(Me, New EventArgs)
        End If
    End Sub

    Private Sub ChkBillSum_CheckedChanged(sender As Object, e As EventArgs) Handles ChkBillSum.CheckedChanged
        If chkNodewiseSummary.Checked = True Then
            ChkBillSum.Checked = False
        End If
    End Sub

    Private Sub chkNodewiseSummary_CheckedChanged(sender As Object, e As EventArgs) Handles chkNodewiseSummary.CheckedChanged
        If ChkBillSum.Checked = True Then
            chkNodewiseSummary.Checked = False
        End If
    End Sub


End Class

Public Class frmPurchaseReport_Properties

    Private chkTotalSummay As Boolean = False
    Public Property p_chkTotalSummay() As Boolean
        Get
            Return chkTotalSummay
        End Get
        Set(ByVal value As Boolean)
            chkTotalSummay = value
        End Set
    End Property

    Private chkLstNodeIdSelectAll As Boolean = False
    Public Property p_chkLstNodeIdSelectAll() As Boolean
        Get
            Return chkLstNodeIdSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkLstNodeIdSelectAll = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private chkCashCounterSelectAll As Boolean = False
    Public Property p_chkCashCounterSelectAll() As Boolean
        Get
            Return chkCashCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCashCounterSelectAll = value
        End Set
    End Property
    Private chkLstCashCounter As New List(Of String)
    Public Property p_chkLstCashCounter() As List(Of String)
        Get
            Return chkLstCashCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCashCounter = value
        End Set
    End Property
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property

    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private chkPureWtBased As Boolean = False
    Public Property p_chkPureWtBased() As Boolean
        Get
            Return chkPureWtBased
        End Get
        Set(ByVal value As Boolean)
            chkPureWtBased = value
        End Set
    End Property
    Private chknode As Boolean = False
    Public Property p_chknode() As Boolean
        Get
            Return chknode
        End Get
        Set(ByVal value As Boolean)
            chknode = value
        End Set
    End Property

    Private chkPurchase As Boolean = False
    Public Property p_chkPurchase() As Boolean
        Get
            Return chkPurchase
        End Get
        Set(ByVal value As Boolean)
            chkPurchase = value
        End Set
    End Property

    Private ChkSalesReturn As Boolean = False
    Public Property p_chkSalesReturn() As Boolean
        Get
            Return ChkSalesReturn
        End Get
        Set(ByVal value As Boolean)
            ChkSalesReturn = value
        End Set
    End Property

    Private ChkPartlySales As Boolean = False
    Public Property p_ChkPartlySales() As Boolean
        Get
            Return ChkPartlySales
        End Get
        Set(ByVal value As Boolean)
            ChkPartlySales = value
        End Set
    End Property

    Private chkGroupByItemType As Boolean = False
    Public Property p_chkGroupByItemType() As Boolean
        Get
            Return chkGroupByItemType
        End Get
        Set(ByVal value As Boolean)
            chkGroupByItemType = value
        End Set
    End Property
    Private ChkLstGroupBy As New List(Of String)
    Public Property p_ChkLstGroupBy() As List(Of String)
        Get
            Return ChkLstGroupBy
        End Get
        Set(ByVal value As List(Of String))
            ChkLstGroupBy = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtBothMU As Boolean = False
    Public Property p_rbtBothMU() As Boolean
        Get
            Return rbtBothMU
        End Get
        Set(ByVal value As Boolean)
            rbtBothMU = value
        End Set
    End Property
    Private rbtMarked As Boolean = False
    Public Property p_rbtMarked() As Boolean
        Get
            Return rbtMarked
        End Get
        Set(ByVal value As Boolean)
            rbtMarked = value
        End Set
    End Property
    Private rbtUnmarked As Boolean = False
    Public Property p_rbtUnmarked() As Boolean
        Get
            Return rbtUnmarked
        End Get
        Set(ByVal value As Boolean)
            rbtUnmarked = value
        End Set
    End Property
    Private chkItemName As Boolean = False
    Public Property p_chkItemName() As Boolean
        Get
            Return chkItemName
        End Get
        Set(ByVal value As Boolean)
            chkItemName = value
        End Set
    End Property
    Private chkGrtotal As Boolean = False
    Public Property p_chkGrtotal() As Boolean
        Get
            Return chkGrtotal
        End Get
        Set(ByVal value As Boolean)
            chkGrtotal = value
        End Set
    End Property
    Private chkOrdAdv As Boolean = False
    Public Property p_chkOrdAdv() As Boolean
        Get
            Return chkOrdAdv
        End Get
        Set(ByVal value As Boolean)
            chkOrdAdv = value
        End Set
    End Property
    Private chkSubItemName As Boolean = False
    Public Property p_chkSubItemName() As Boolean
        Get
            Return chkSubItemName
        End Get
        Set(ByVal value As Boolean)
            chkSubItemName = value
        End Set
    End Property
    Private chkMelting As Boolean = False
    Public Property p_chkMelting() As Boolean
        Get
            Return chkMelting
        End Get
        Set(ByVal value As Boolean)
            chkMelting = value
        End Set
    End Property
    Private rbtDefault As Boolean = False
    Public Property p_rbtDefault() As Boolean
        Get
            Return rbtDefault
        End Get
        Set(ByVal value As Boolean)
            rbtDefault = value
        End Set
    End Property
    Private rbtEstimation As Boolean = False
    Public Property p_rbtEstimation() As Boolean
        Get
            Return rbtEstimation
        End Get
        Set(ByVal value As Boolean)
            rbtEstimation = value
        End Set
    End Property
    Private rbtEmp As Boolean = False
    Public Property p_rbtEmp() As Boolean
        Get
            Return rbtEmp
        End Get
        Set(ByVal value As Boolean)
            rbtEmp = value
        End Set
    End Property
    Private chkBillSum As Boolean = False
    Public Property p_chkBillSum() As Boolean
        Get
            Return chkBillSum
        End Get
        Set(ByVal value As Boolean)
            chkBillSum = value
        End Set
    End Property
End Class