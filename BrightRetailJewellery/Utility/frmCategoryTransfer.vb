Imports System.Data.OleDb
Public Class frmCategoryTransfer
    'CALNO 101212 ,VASANTH, CLIENT-AKSHAYA GOLD
#Region "Variable Declaration"
    Dim strSql As String
    Dim ds As New DataSet
    Dim dtUpdatable As New DataTable
    Dim dtCompany As New DataTable, dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim frmcatcode As String = Nothing
    Dim tocatcode As String = Nothing
    Dim selectedcompid As String = Nothing
    Dim selectedcostid As String = Nothing
    Dim mtempacctran As String = "TEMP" & systemId & "ACCTRAN"
#End Region

#Region "User Defined Function"

    Private Sub Funcnew()
        frmcatcode = Nothing
        tocatcode = Nothing
        selectedcompid = Nothing
        selectedcostid = Nothing
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        txtTranno.Text =""
        btnTransfer.Enabled = True
        chkCmbCompany.Items.Clear()
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            chkCmbCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
        Else
            chkCmbCostCentre.Items.Clear()
            chkCmbCostCentre.Enabled = False
        End If

        strSql = "SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY ORDER BY RESULT,CATNAME"
        objGPack.FillCombo(strSql, cmbCatNamefrom, True, True)
        objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        chkCmbCompany.Select()
        chkCmbCompany.Focus()
    End Sub

    Private Sub Filteration()
        Dim Dreader As OleDb.OleDbDataReader
        If cmbCatNamefrom.Enabled = True And cmbCategoryTo.Enabled = True Then
            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCatNamefrom.Text.ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            Dreader = cmd.ExecuteReader()
            frmcatcode = IIf(Dreader.Read = True, Dreader.Item("CATCODE"), "")

            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCategoryTo.Text.ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            Dreader = cmd.ExecuteReader()
            tocatcode = IIf(Dreader.Read = True, Dreader.Item("CATCODE"), "")
        Else
            frmcatcode = Nothing
            tocatcode = Nothing
        End If
        If chkCmbCompany.Text <> "ALL" Then selectedcompid = GetSelectedCompanyId(chkCmbCompany, True) Else selectedcompid = Nothing
        If chkCmbCostCentre.Text <> "ALL" Then selectedcostid = IIf(chkCmbCostCentre.Text.Trim <> "", GetSelectedCostId(chkCmbCostCentre, True), Nothing) Else selectedcostid = Nothing
    End Sub

    Private Sub Transfer()

        If MessageBox.Show("Kindly Backup Current Database" + vbCrLf + "Sure you want to proceed?", "Category Transfer Backup Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        btnTransfer.Enabled = False
        Dim objSoftKeys As New SoftKeys
        Try
            Filteration()
            tran = Nothing
            tran = cn.BeginTransaction
            If rbtAcupdate.Checked = True Then
                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'SAF_OLDACCTRAN') > 0 "
                strSql += " DROP TABLE SAF_OLDACCTRAN "
                strSql += " SELECT * INTO SAF_OLDACCTRAN FROM " & cnStockDb & "..ACCTRAN AS T "
                strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += " AND ISNULL(CANCEL,'') = ''"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(COMPANYID,'') IN (" & selectedcompid & ") AND FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(COSTID,'') IN(" & selectedcostid & ")"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN "
                strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += " AND ISNULL(CANCEL,'') = ''"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(COMPANYID,'') IN (" & selectedcompid & ") AND FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                ' ''UPDATE ISSUE
                'strSql = " UPDATE " & cnStockDb & "..ISSUE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..ISSUE AS I," & cnAdminDb & "..ITEMMAST AS M"
                'strSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.ITEMID = M.ITEMID"
                'If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                'If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
                'If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                ' ''UPDATE ISSSTONE
                'strSql = " UPDATE " & cnStockDb & "..ISSSTONE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..ISSSTONE AS I," & cnAdminDb & "..ITEMMAST AS M"
                'strSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.STNITEMID = M.ITEMID"
                'If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                'If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
                'If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                ' ''UPDATE RECEIPT
                'strSql = " UPDATE " & cnStockDb & "..RECEIPT SET CATCODE = M.CATCODE FROM " & cnStockDb & "..RECEIPT AS R," & cnAdminDb & "..ITEMMAST AS M"
                'strSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND R.ITEMID = M.ITEMID"
                'If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(R.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                'If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(R.COMPANYID,'') IN (" & selectedcompid & ")  "
                'If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(R.COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                ' ''UPDATE RECEIPTSTONE
                'strSql = " UPDATE " & cnStockDb & "..RECEIPTSTONE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..RECEIPTSTONE AS R," & cnAdminDb & "..ITEMMAST AS M"
                'strSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND R.STNITEMID = M.ITEMID"
                'If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(R.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                'If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(R.COMPANYID,'') IN (" & selectedcompid & ")  "
                'If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(R.COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            Else


                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'SAF_OLDACCTRAN') > 0 "
                strSql += vbCrLf + " DROP TABLE SAF_OLDACCTRAN "
                strSql += vbCrLf + " SELECT * INTO SAF_OLDACCTRAN FROM( "
                strSql += vbCrLf + " SELECT * FROM " & cnStockDb & "..ACCTRAN AS T "
                strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND T.PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
                strSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE CATCODE='" & frmcatcode & "')"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(T.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'') IN (" & selectedcompid & ") AND T.FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') IN(" & selectedcostid & ")"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT * FROM " & cnStockDb & "..ACCTRAN AS T "
                strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND T.PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
                strSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE CATCODE='" & frmcatcode & "')"
                If txtTranno.Text.Trim <> "" Then strSql += " and ISNULL(T.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'') IN (" & selectedcompid & ") AND T.FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') IN(" & selectedcostid & ")"
                strSql += vbCrLf + ")X"

                cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN "
                strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE CATCODE='" & frmcatcode & "')"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + " AND ISNULL(COMPANYID,'') IN (" & selectedcompid & ") AND FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN "
                strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE CATCODE='" & frmcatcode & "')"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + " AND ISNULL(COMPANYID,'') IN (" & selectedcompid & ") AND FROMFLAG = 'P'"
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') IN(" & selectedcostid & ")"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                ''UPDATE ISSUE
                strSql = " UPDATE " & cnStockDb & "..ISSUE SET CATCODE = '" & tocatcode & "' FROM " & cnStockDb & "..ISSUE AS I"
                strSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
                strSql += " AND I.CATCODE='" & frmcatcode & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                ''UPDATE ISSSTONE
                strSql = " UPDATE " & cnStockDb & "..ISSSTONE SET CATCODE = '" & tocatcode & "' FROM " & cnStockDb & "..ISSSTONE AS I," & cnAdminDb & "..ITEMMAST AS M"
                strSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.STNITEMID = M.ITEMID"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
                strSql += " AND I.CATCODE='" & frmcatcode & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                ''UPDATE RECEIPT
                strSql = " UPDATE " & cnStockDb & "..RECEIPT SET CATCODE = '" & tocatcode & "' FROM " & cnStockDb & "..RECEIPT AS R "
                strSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(R.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(R.COMPANYID,'') IN (" & selectedcompid & ")  "
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(R.COSTID,'') IN(" & selectedcostid & ")"
                strSql += " AND R.CATCODE='" & frmcatcode & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                ''UPDATE RECEIPTSTONE
                strSql = " UPDATE " & cnStockDb & "..RECEIPTSTONE SET CATCODE = '" & tocatcode & "' FROM " & cnStockDb & "..RECEIPTSTONE AS R "
                strSql += " WHERE R.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(R.TRANNO,0)=" & Val(txtTranno.Text.Trim)
                If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += " AND ISNULL(R.COMPANYID,'') IN (" & selectedcompid & ")  "
                If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += " AND ISNULL(R.COSTID,'') IN(" & selectedcostid & ")"
                strSql += " AND R.CATCODE='" & frmcatcode & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If


            strSql = vbCrLf + "  DECLARE @SEPACCPOST_ITEM VARCHAR(10)"
            strSql += vbCrLf + "  SELECT @SEPACCPOST_ITEM = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SEPACCPOST_ITEM'"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUE"
            'strSql += vbCrLf + "  SELECT I.TRANNO,I.TRANDATE,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT"
            strSql += vbCrLf + "  SELECT I.TRANNO,I.TRANDATE,I.PCS"
            strSql += vbCrLf + "  ,CASE WHEN I.TRANTYPE ='OD' THEN 0 ELSE I.GRSWT END GRSWT "
            strSql += vbCrLf + "  ,CASE WHEN I.TRANTYPE ='OD' THEN 0 ELSE I.NETWT END NETWT "
            strSql += vbCrLf + "  ,CASE WHEN I.TRANTYPE ='OD' THEN 0 ELSE I.AMOUNT END AMOUNT "
            strSql += vbCrLf + "  ,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,CONVERT(VARCHAR(15),I.SNO)SNO,I.MCHARGE,I.METALID,I.TAX,I.WASTAGE,I.RATE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(1),'C')TRANMODE,CONVERT(VARCHAR(2),'SA')AS PAYMODE,CONVERT(VARCHAR(7),C.SALESID)AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID,CONVERT(VARCHAR(1),'I')TBL"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPISSUE"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(I.CANCEL,'') = ''"
            If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
            If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
            strSql += vbCrLf + "  AND I.TRANTYPE IN ('SA','OD','RD')"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            If rbtCatUpdate.Checked Then strSql += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND CATCODE  = '" & tocatcode & "')"
            'strSql += vbCrLf + "  AND I.AMOUNT <> 0"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT I.TRANNO,I.TRANDATE,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT"
            strSql += vbCrLf + "  ,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,CONVERT(VARCHAR(15),I.SNO)SNO,I.MCHARGE,I.METALID,I.TAX,I.WASTAGE,I.RATE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(1),'D')TRANMODE,CONVERT(VARCHAR(2),I.TRANTYPE)AS PAYMODE,CONVERT(VARCHAR(7),CASE WHEN I.TRANTYPE = 'SR' THEN C.SRETURNID ELSE C.PURCHASEID END)AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID,CONVERT(VARCHAR(1),'R')TBL"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(I.CANCEL,'') = ''"
            If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")  "
            If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
            strSql += vbCrLf + "  AND I.TRANTYPE IN ('SR','PU')"
            If rbtCatUpdate.Checked Then strSql += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND CATCODE  = '" & tocatcode & "')"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(I.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            'strSql += vbCrLf + "  AND I.AMOUNT <> 0"

            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPISSUE SET MCHARGE = NULL,WASTAGE = NULL" '',SNO = NULL"
            strSql += vbCrLf + "  WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'  AND TRANTYPE IN ('OD','RD') "
            If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + "  AND ISNULL(COMPANYID,'') IN (" & selectedcompid & ")"
            If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + "  AND ISNULL(COSTID,'') IN(" & selectedcostid & ")"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPDIASTONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPDIASTONE"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMPDIASTONE(DIASTONE VARCHAR(1))"
            strSql += vbCrLf + "  IF ISNULL(CHARINDEX('D',@SEPACCPOST_ITEM),0) <> 0 INSERT INTO TEMPTABLEDB..TEMPDIASTONE(DIASTONE)SELECT 'D'"
            strSql += vbCrLf + "  IF ISNULL(CHARINDEX('S',@SEPACCPOST_ITEM),0) <> 0 INSERT INTO TEMPTABLEDB..TEMPDIASTONE(DIASTONE)SELECT 'S'"
            strSql += vbCrLf + "  IF ISNULL(CHARINDEX('P',@SEPACCPOST_ITEM),0) <> 0 INSERT INTO TEMPTABLEDB..TEMPDIASTONE(DIASTONE)SELECT 'P'"

            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPDIASTONE) > 0"
            strSql += vbCrLf + "  	BEGIN /** GETTING DSP INFO **/"
            strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID,TAX)"
            strSql += vbCrLf + "  	SELECT I.TRANNO,I.TRANDATE,0 STNPCS,0 STNWT,0 STNWT,-1*S.STNAMT,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,I.TRANMODE,I.PAYMODE,CASE WHEN I.PAYMODE = 'SA' THEN C.SALESID WHEN I.PAYMODE = 'SR' THEN C.SRETURNID ELSE C.PURCHASEID END AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID," + IIf(objSoftKeys.SEPSTUDTAXPOST, "-1*S.TAX", "0")
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..ISSSTONE AS S"
            strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPISSUE AS I ON I.SNO = S.ISSSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE IN (SELECT DIASTONE FROM TEMPTABLEDB..TEMPDIASTONE)"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + "    WHERE ISNULL(I.SNO,'') <> '' AND I.TBL = 'I'"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(S.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            strSql += vbCrLf + "  	UNION ALL"
            strSql += vbCrLf + "  	SELECT I.TRANNO,I.TRANDATE,S.STNPCS,S.STNWT,S.STNWT,S.STNAMT,I.BATCHNO,I.COMPANYID,I.COSTID,S.CATCODE,I.TRANMODE,I.PAYMODE,CASE WHEN I.PAYMODE = 'SA' THEN C.SALESID WHEN I.PAYMODE = 'SR' THEN C.SRETURNID ELSE C.PURCHASEID END AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID," + IIf(objSoftKeys.SEPSTUDTAXPOST, "S.TAX", "0")
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..ISSSTONE AS S"
            strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPISSUE AS I ON I.SNO = S.ISSSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE IN (SELECT DIASTONE FROM TEMPTABLEDB..TEMPDIASTONE)"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = S.CATCODE"
            strSql += vbCrLf + "    WHERE ISNULL(I.SNO,'') <> '' AND I.TBL = 'I'"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(S.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            strSql += vbCrLf + "  	END /** GETTING DSP INFO **/"

            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPDIASTONE) > 0"
            strSql += vbCrLf + "  	BEGIN /** GETTING DSP INFO **/"
            strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID,TAX)"
            strSql += vbCrLf + "  	SELECT I.TRANNO,I.TRANDATE,0 STNPCS,0 STNWT,0 STNWT,-1*S.STNAMT,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,I.TRANMODE,I.PAYMODE,CASE WHEN I.PAYMODE = 'SA' THEN C.SALESID WHEN I.PAYMODE = 'SR' THEN C.SRETURNID ELSE C.PURCHASEID END AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID," + IIf(objSoftKeys.SEPSTUDTAXPOST, "-1*S.TAX", "0")
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..RECEIPTSTONE AS S"
            strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPISSUE AS I ON I.SNO = S.ISSSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE IN (SELECT DIASTONE FROM TEMPTABLEDB..TEMPDIASTONE)"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = S.CATCODE"
            strSql += vbCrLf + "    WHERE ISNULL(I.SNO,'') <> '' AND I.TBL = 'R'"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(S.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            strSql += vbCrLf + "  	UNION ALL"
            strSql += vbCrLf + "  	SELECT I.TRANNO,I.TRANDATE,S.STNPCS,S.STNWT,S.STNWT,S.STNAMT,I.BATCHNO,I.COMPANYID,I.COSTID,S.CATCODE,I.TRANMODE,I.PAYMODE,CASE WHEN I.PAYMODE = 'SA' THEN C.SALESID WHEN I.PAYMODE = 'SR' THEN C.SRETURNID ELSE C.PURCHASEID END AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID," + IIf(objSoftKeys.SEPSTUDTAXPOST, "S.TAX", "0")
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..RECEIPTSTONE AS S"
            strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPISSUE AS I ON I.SNO = S.ISSSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE IN (SELECT DIASTONE FROM TEMPTABLEDB..TEMPDIASTONE)"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + "    WHERE ISNULL(I.SNO,'') <> '' AND I.TBL = 'R'"
            If txtTranno.Text.Trim <> "" Then strSql += " AND ISNULL(S.TRANNO,0)='" & Val(txtTranno.Text.Trim) & "'"
            strSql += vbCrLf + "  	END /** GETTING DSP INFO **/"

            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID)"
            strSql += vbCrLf + "  SELECT I.TRANNO,I.TRANDATE,I.TAX AS AMOUNT,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,CASE WHEN PAYMODE = 'SR' THEN 'D' ELSE 'C' END,CASE WHEN PAYMODE = 'SA' THEN 'SV' WHEN PAYMODE = 'SR' THEN 'RV' ELSE 'PV' END"
            strSql += vbCrLf + "  ,CASE WHEN I.PAYMODE = 'PU' THEN C.PTAXID ELSE C.STAXID END "
            strSql += vbCrLf + "  ,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE AS I"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"

            strSql += vbCrLf + " delete FROM TEMPTABLEDB..TEMPISSUE WHERE AMOUNT = 0"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF ISNULL(CHARINDEX('M',@SEPACCPOST_ITEM),0) <> 0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID)"
            strSql += vbCrLf + "  SELECT TRANNO,TRANDATE,-1*MCHARGE AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE WHERE ISNULL(MCHARGE,0) <> 0"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT TRANNO,TRANDATE,MCHARGE AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,METALID+'MAK'+CASE WHEN TRANMODE = 'C' THEN 'R' ELSE 'P' END,USERID,UPDATED,UPTIME,SYSTEMID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE WHERE ISNULL(MCHARGE,0) <> 0"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF ISNULL(CHARINDEX('W',@SEPACCPOST_ITEM),0) <> 0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID)"
            strSql += vbCrLf + "  SELECT TRANNO,TRANDATE,-1*(WASTAGE*RATE) AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE WHERE ISNULL(WASTAGE,0) <> 0"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT TRANNO,TRANDATE,(WASTAGE*RATE) AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,TRANMODE,PAYMODE,METALID+'WAST'+CASE WHEN TRANMODE = 'C' THEN 'R' ELSE 'P' END,USERID,UPDATED,UPTIME,SYSTEMID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE WHERE ISNULL(WASTAGE,0) <> 0"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  "

            strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMPISSUE(TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,BATCHNO,COMPANYID,COSTID,CATCODE,SNO,MCHARGE"
            strSql += vbCrLf + "   ,METALID,TAX,WASTAGE,RATE,TRANMODE,PAYMODE,ACCODE,USERID,UPDATED,UPTIME,SYSTEMID,TBL)"
            strSql += vbCrLf + "  SELECT I.TRANNO,I.TRANDATE,0,0,0"
            strSql += vbCrLf + "   ,I.MCHARGE AMOUNT "
            strSql += vbCrLf + "   ,I.BATCHNO,I.COMPANYID,I.COSTID,I.CATCODE,CONVERT(VARCHAR(15),NULL)SNO,0,I.METALID,0,0,0"
            strSql += vbCrLf + "   ,CONVERT(VARCHAR(1),'C')TRANMODE,CONVERT(VARCHAR(2),'SA')AS PAYMODE,C.METALID+'MAKR' AS ACCODE,I.USERID,I.UPDATED,I.UPTIME,I.SYSTEMID,CONVERT(VARCHAR(1),'I')TBL"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = I.CATCODE"
            strSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(I.CANCEL,'') = ''"
            If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")"
            If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
            strSql += vbCrLf + "  AND I.TRANTYPE IN ('OD')"
            strSql += vbCrLf + "  AND I.MCHARGE > 0"

            strSql += vbCrLf + "  IF OBJECT_ID('MASTER..TEMP" & systemId & "ISSREC') IS NOT NULL DROP TABLE TEMP" & systemId & "ISSREC"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  CONVERT(VARCHAR(15),NULL)SNO"
            strSql += vbCrLf + "  ,I.TRANNO,I.TRANDATE,I.TRANMODE,I.ACCODE,SUM(I.PCS)PCS,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.AMOUNT)AMOUNT"
            strSql += vbCrLf + "  ,I.PAYMODE,I.BATCHNO,I.COMPANYID,I.COSTID"
            'CALNO 101212
            strSql += vbCrLf + "  ,I.USERID,I.UPDATED,MAX(I.UPTIME) UPTIME,I.SYSTEMID"
            strSql += vbCrLf + "  ,IDENTITY(INT,1,1) AS KEYNO"
            strSql += vbCrLf + "  INTO TEMP" & systemId & "ISSREC"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPISSUE AS I"
            strSql += vbCrLf + "  WHERE 1=1 "

            If selectedcompid <> Nothing And selectedcompid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COMPANYID,'') IN (" & selectedcompid & ")"
            If selectedcostid <> Nothing And selectedcostid <> "''" Then strSql += vbCrLf + "  AND ISNULL(I.COSTID,'') IN(" & selectedcostid & ")"
            If tocatcode <> Nothing Then strSql += " AND ISNULL(I.CATCODE,'')='" & tocatcode & "'"

            strSql += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE FROMFLAG = 'P')"
            strSql += vbCrLf + "  OR BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE)"
            strSql += vbCrLf + "  OR BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT)"
            strSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.TRANMODE,I.PAYMODE,I.BATCHNO,I.COMPANYID,I.COSTID,I.ACCODE"
            strSql += vbCrLf + "  ,I.USERID,I.UPDATED,I.SYSTEMID"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = " UPDATE TEMP" & systemId & "ISSREC SET SNO = KEYNO"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
            strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
            strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@CTLID = 'ACCTRANCODE'"
            strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ACCTRAN'"
            strSql += vbCrLf + " ,@UPD_DBNAME = 'MASTER'"
            strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP" & systemId & "ISSREC'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & mtempacctran & "')> 0"
            strSql += " DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " SELECT  SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE,AMOUNT,BALANCE,PCS,GRSWT,NETWT,PUREWT,REFNO,REFDATE,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE,BRSFLAG, RELIASEDATE, FROMFLAG, REMARK1, REMARK2, CONTRA, BATCHNO, USERID, UPDATED, UPTIME, SYSTEMID, CANCEL, CASHID, COSTID, SCOSTID,  APPVER, COMPANYID, TRANSFERED, WT_ENTORDER,RATE,TRANFLAG,PCODE,DISC_EMPID,CASHPOINTID,TDSCATID,TDSPER,TDSAMOUNT,FLAG,ENTREFNO,ESTBATCHNO"
            strSql += " INTO " & cnStockDb & ".." & mtempacctran & " FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()


            strSql = " INSERT INTO " & cnStockDb & ".." & mtempacctran & ""
            strSql += " ("
            strSql += "  SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,AMOUNT"
            strSql += " ,PCS,GRSWT,NETWT,REFNO,REFDATE"
            strSql += " ,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE"
            strSql += " ,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
            strSql += " ,CONTRA,BATCHNO,USERID,UPDATED,UPTIME"
            strSql += " ,SYSTEMID,CANCEL,CASHID,COSTID,COMPANYID,APPVER"
            strSql += " )"
            strSql += " SELECT "
            strSql += "  SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,AMOUNT"
            strSql += " ,ISNULL(PCS,0),ISNULL(GRSWT,0),ISNULL(NETWT,0),'' REFNO,NULL REFDATE"
            strSql += " ,PAYMODE,'' CHQCARDNO,0 CARDID,'' CHQCARDREF,NULL CHQDATE"
            strSql += " ,'' BRSFLAG,NULL RELIASEDATE,'P' FROMFLAG"
            strSql += " ,(SELECT TOP 1 REMARK1 FROM SAF_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANNO = T.TRANNO AND COMPANYID = T.COMPANYID AND COSTID = T.COSTID)COSTID"
            strSql += " ,(SELECT TOP 1 REMARK2 FROM SAF_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANNO = T.TRANNO AND COMPANYID = T.COMPANYID AND COSTID = T.COSTID)COSTID"
            strSql += " ,''CONTRA,BATCHNO"
            strSql += " ,USERID,UPDATED,UPTIME"
            strSql += " ,SYSTEMID,'' CANCEL"
            strSql += " ,(SELECT TOP 1 CASHID FROM SAF_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANNO = T.TRANNO AND COMPANYID = T.COMPANYID AND COSTID = T.COSTID)CASHID"
            strSql += " ,COSTID,COMPANYID,'" & VERSION & "'"
            strSql += " FROM TEMP" & systemId & "ISSREC as T"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " INSERT INTO " & cnStockDb & ".." & mtempacctran & "(SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE,AMOUNT,BALANCE,PCS,GRSWT,NETWT,PUREWT,REFNO,REFDATE,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE,BRSFLAG, RELIASEDATE, FROMFLAG, REMARK1, REMARK2, CONTRA, BATCHNO, USERID, UPDATED, UPTIME, SYSTEMID, CANCEL, CASHID, COSTID, SCOSTID,  APPVER, COMPANYID, TRANSFERED, WT_ENTORDER,RATE,TRANFLAG,PCODE,DISC_EMPID,CASHPOINTID,TDSCATID,TDSPER,TDSAMOUNT,FLAG,ENTREFNO,ESTBATCHNO)"
            strSql += " SELECT SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE,AMOUNT,BALANCE,PCS,GRSWT,NETWT,PUREWT,REFNO,REFDATE,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE,BRSFLAG, RELIASEDATE, FROMFLAG, REMARK1, REMARK2, CONTRA, BATCHNO, USERID, UPDATED, UPTIME, SYSTEMID, CANCEL, CASHID, COSTID, SCOSTID,  APPVER, COMPANYID, TRANSFERED, WT_ENTORDER,RATE,TRANFLAG,PCODE,DISC_EMPID,CASHPOINTID,TDSCATID,TDSPER,TDSAMOUNT,FLAG,ENTREFNO,ESTBATCHNO"
            strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & ".." & mtempacctran & ")"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = "  UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> '' AND ACCODE <> T.ACCODE)"
            strSql += "  FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ISNULL(CONTRA,'') = ''"
            strSql += "  UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> '' AND ACCODE <> T.ACCODE)"
            strSql += "  FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " DELETE FROM " & cnStockDb & ".." & mtempacctran & " WHERE SNO IN("
            strSql += " SELECT SNO FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & ".." & mtempacctran & "))"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()


            strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = '" & mtempacctran & "',@MASK_TABLENAME = 'ACCTRAN'"
            Dim DtTempQry As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(DtTempQry)
            For Each ro As DataRow In DtTempQry.Rows
                strSql = ro.Item(0).ToString
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                strSql = " "
            Next
            strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & mtempacctran & "')> 0"
            strSql += " DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()


            tran.Commit()
            tran = Nothing
            MsgBox("Transfer Completed..")
            btnTransfer.Enabled = True
            btnExit.Select()
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            btnTransfer.Enabled = True
        End Try
    End Sub

#End Region

#Region "Events"

    Private Sub frmCategoryTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCategoryTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Funcnew()
    End Sub


    Private Sub btnTransfer_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Transfer()
        Exit Sub
        Try
            btnTransfer.Enabled = False

            'strSql = " EXECUTE " & cnStockDb & "..SP_ReceiptVsAcctran @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " ,@TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " ,@CATCODE = ''"
            'strSql += " ,@CNCOMPANYID = '" & strCompanyId & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.CommandTimeout = 2000
            'cmd.ExecuteNonQuery()
            Dim objCheck As New frmDataChecking(frmDataChecking.Type.Issue)
            objCheck.CheckIssue(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), "", strCompanyId)
            objCheck.CheckReceipt(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), "", strCompanyId)
            'strSql = " EXECUTE " & cnStockDb & "..SP_IssueVsAcctran @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " ,@TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            'strSql += " ,@CATCODE = ''"
            'strSql += " ,@CNCOMPANYID = '" & strCompanyId & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.CommandTimeout = 2000
            'cmd.ExecuteNonQuery()
            tran = cn.BeginTransaction

            ' ''UPDATE ISSUE
            'strSql = " UPDATE " & cnStockDb & "..ISSUE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..ISSUE AS I," & cnAdminDb & "..ITEMMAST AS M"
            'strSql += " WHERE I.ITEMID = M.ITEMID"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            ' ''UPDATE ISSSTONE
            'strSql = " UPDATE " & cnStockDb & "..ISSSTONE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..ISSSTONE AS I," & cnAdminDb & "..ITEMMAST AS M"
            'strSql += " WHERE I.STNITEMID = M.ITEMID"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            ' ''UPDATE RECEIPT
            'strSql = " UPDATE " & cnStockDb & "..RECEIPT SET CATCODE = M.CATCODE FROM " & cnStockDb & "..RECEIPT AS R," & cnAdminDb & "..ITEMMAST AS M"
            'strSql += " WHERE R.ITEMID = M.ITEMID"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            ' ''UPDATE RECEIPTSTONE
            'strSql = " UPDATE " & cnStockDb & "..RECEIPTSTONE SET CATCODE = M.CATCODE FROM " & cnStockDb & "..RECEIPTSTONE AS R," & cnAdminDb & "..ITEMMAST AS M"
            'strSql += " WHERE R.STNITEMID = M.ITEMID"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCATTRANS')>0 /**/"
            strSql += " DROP TABLE TEMPCATTRANS /**/"
            strSql += " SELECT * INTO TEMPCATTRANS FROM TEMP_ISSVSTRAN /**/"
            strSql += " UNION ALL /**/"
            strSql += " SELECT * FROM TEMP_RECVSTRAN /**/"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_OLDACCTRAN') > 0 /**/"
            strSql += " DROP TABLE TEMP_OLDACCTRAN /**/"
            strSql += " SELECT * INTO TEMP_OLDACCTRAN FROM " & cnStockDb & "..ACCTRAN AS T /**/"
            strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') /**/"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "' /**/"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN "
            strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " AND PAYMODE IN ('SA','SV','PU','PV','SR','RV') /**/"
            strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "' /**/"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_NEWACCTRAN') > 0 /**/"
            'strSql += " DROP TABLE TEMP_NEWACCTRAN /**/"
            'strSql += " SELECT N.TRANNO,N.TRANDATE,N.TRANMODE,N.ACCODE,N.AMOUNT,N.PCS,N.GRSWT,N.NETWT /**/"
            'strSql += " ,O.REFNO,O.REFDATE,N.PAYMODE,O.CHQCARDNO,O.CARDID,O.CHQCARDREF /**/"
            'strSql += " ,O.CHQDATE,O.BRSFLAG,O.RELIASEDATE,O.FROMFLAG,O.REMARK1,O.REMARK2,O.CONTRA,O.BATCHNO,O.USERID /**/"
            'strSql += " ,CONVERT(SMALLDATETIME,CONVERT(VARCHAR,GETDATE(),101))UPDATED,CONVERT(SMALLDATETIME,CONVERT(VARCHAR,GETDATE(),108))UPTIME /**/"
            'strSql += " ,O.SYSTEMID,O.CANCEL,O.CASHID,O.COSTID,O.VATEXM,O.APPVER,O.COMPANYID /**/"
            'strSql += " INTO TEMP_NEWACCTRAN FROM /**/"
            'strSql += " ( /**/"
            'strSql += " SELECT TRANNO,TRANDATE,CASE WHEN PAYMODE = 'SA' THEN 'C' ELSE 'D' END TRANMODE /**/"
            'strSql += " ,ISNULL((SELECT CASE PAYMODE WHEN 'SA' THEN SALESID WHEN 'SR' THEN SRETURNID ELSE PURCHASEID END FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE = T.CATCODE),T.CATCODE)AS ACCODE /**/"
            'strSql += " ,IAMOUNT AMOUNT,IPCS PCS,IGRSWT GRSWT,INETWT NETWT,PAYMODE,BATCHNO /**/"
            'strSql += " FROM TEMPCATTRANS AS T /**/"
            'strSql += " UNION ALL /**/"
            'strSql += " SELECT TRANNO,TRANDATE,CASE WHEN PAYMODE = 'SA' THEN 'C' ELSE 'D' END TRANMODE /**/"
            'strSql += " ,ISNULL((SELECT CASE WHEN PAYMODE IN ('SA','SR') THEN STAXID ELSE PTAXID END FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE = T.CATCODE),T.CATCODE)AS ACCODE /**/"
            'strSql += " ,ITAX AMOUNT,IPCS PCS,IGRSWT GRSWT,INETWT NETWT /**/"
            'strSql += " ,CASE PAYMODE WHEN 'SA' THEN 'SV' WHEN 'SR' THEN 'RV' ELSE 'PV' END PAYMODE,BATCHNO /**/"
            'strSql += " FROM TEMPCATTRANS AS T /**/"
            'strSql += " )N JOIN TEMP_OLDACCTRAN O /**/"
            'strSql += " ON N.TRANDATE = O.TRANDATE AND N.BATCHNO = O.BATCHNO AND N.TRANNO = O.TRANNO AND O.PAYMODE = N.PAYMODE /**/"
            'strSql += " ORDER BY N.BATCHNO,N.TRANNO,N.TRANDATE /**/"

            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_NEWACCTRAN') > 0 /**/"
            strSql += " DROP TABLE TEMP_NEWACCTRAN /**/"
            strSql += " SELECT T.TRANNO,T.TRANDATE,T.TRANMODE,T.ACCODE,T.AMOUNT,T.PCS,T.GRSWT,T.NETWT /**/"
            strSql += " ,(SELECT TOP 1 REFNO FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO = T.BATCHNO AND PAYMODE = T.PAYMODE )AS REFNO"
            strSql += " ,(SELECT TOP 1 REFDATE FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO = T.BATCHNO AND PAYMODE = T.PAYMODE) REFDATE"
            strSql += " ,T.PAYMODE"
            strSql += " ,(SELECT TOP 1 CHQCARDNO FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO= T.BATCHNO AND PAYMODE = T.PAYMODE)CHQCARDNO"
            strSql += " ,(SELECT TOP 1 CARDID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO= T.BATCHNO AND PAYMODE = T.PAYMODE)CARDID"
            strSql += " ,(SELECT TOP 1 CHQCARDREF FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)CHQCARDREF"
            strSql += " ,(SELECT TOP 1 CHQDATE FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)CHQDATE"
            strSql += " ,CONVERT(VARCHAR(1),'')BRSFLAG"
            strSql += " ,NULL RELIASEDATE"
            strSql += " ,(SELECT TOP 1 FROMFLAG FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)FROMFLAG"
            strSql += " ,(SELECT TOP 1 REMARK1 FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)REMARK1"
            strSql += " ,(SELECT TOP 1 REMARK2 FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)REMARK2"
            strSql += " ,(SELECT TOP 1 CONTRA FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)CONTRA"
            strSql += " ,T.BATCHNO"
            strSql += " ,(SELECT TOP 1 USERID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO= T.BATCHNO AND PAYMODE = T.PAYMODE)USERID"
            strSql += " ,CONVERT(SMALLDATETIME,CONVERT(VARCHAR,GETDATE(),101))UPDATED,CONVERT(SMALLDATETIME,CONVERT(VARCHAR,GETDATE(),108))UPTIME /**/"
            strSql += " ,(SELECT TOP 1 SYSTEMID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)SYSTEMID"
            strSql += " ,CONVERT(VARCHAR(1),'')CANCEL"
            strSql += " ,(SELECT TOP 1 CASHID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)CASHID"
            strSql += " ,(SELECT TOP 1 COSTID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)COSTID"

            strSql += " ,(SELECT TOP 1 APPVER FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)APPVER"
            strSql += " ,(SELECT TOP 1 COMPANYID FROM TEMP_OLDACCTRAN WHERE TRANDATE = T.TRANDATE AND TRANNO = T.TRANNO AND BATCHNO =T.BATCHNO AND PAYMODE = T.PAYMODE)COMPANYID"
            strSql += " INTO TEMP_NEWACCTRAN"
            strSql += " FROM"
            strSql += " ( /**/"
            strSql += " SELECT TRANNO,TRANDATE,CASE WHEN PAYMODE = 'SA' THEN 'C' ELSE 'D' END TRANMODE /**/"
            strSql += " ,ISNULL((SELECT CASE PAYMODE WHEN 'SA' THEN SALESID WHEN 'SR' THEN SRETURNID ELSE PURCHASEID END FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE),T.CATCODE)AS ACCODE /**/"
            strSql += " ,IAMOUNT AMOUNT,IPCS PCS,IGRSWT GRSWT,INETWT NETWT,PAYMODE,BATCHNO /**/"
            strSql += " FROM TEMPCATTRANS AS T /**/"
            strSql += " UNION ALL /**/"
            strSql += " SELECT TRANNO,TRANDATE,CASE WHEN PAYMODE = 'SA' THEN 'C' ELSE 'D' END TRANMODE /**/"
            strSql += " ,ISNULL((SELECT CASE WHEN PAYMODE IN ('SA','SR') THEN STAXID ELSE PTAXID END FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE),T.CATCODE)AS ACCODE /**/"
            strSql += " ,ITAX AMOUNT,IPCS PCS,IGRSWT GRSWT,INETWT NETWT /**/"
            strSql += " ,CASE PAYMODE WHEN 'SA' THEN 'SV' WHEN 'SR' THEN 'RV' ELSE 'PV' END PAYMODE,BATCHNO /**/"
            strSql += " FROM TEMPCATTRANS AS T /**/"
            strSql += " )T "
            strSql += " ORDER BY T.BATCHNO,T.TRANNO,T.TRANDATE /**/"

            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMP_NEWACCTRAN SET CONTRA = ISNULL((SELECT TOP 1 ACCODE FROM TEMP_NEWACCTRAN WHERE TRANMODE = 'D' AND BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE),ACCODE) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE TRANMODE = 'C' /**/"
            strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET CONTRA = ISNULL((SELECT TOP 1 ACCODE FROM TEMP_NEWACCTRAN WHERE TRANMODE = 'C' AND BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE),ACCODE) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE TRANMODE = 'D' /**/"
            strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            strSql += " CASHID = ISNULL((SELECT TOP 1 CASHID FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(CASHID,'') <> ''),T.CASHID) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(CASHID,'') = '' /**/"
            strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            strSql += " COSTID = ISNULL((SELECT TOP 1 COSTID FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(COSTID,'') <> ''),T.COSTID) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(COSTID,'') = '' /**/"
            strSql += " "
            'strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            'strSql += " VATEXM = ISNULL((SELECT TOP 1 VATEXM FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(VATEXM,'') <> ''),T.VATEXM) /**/"
            'strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(VATEXM,'') = '' /**/"
            'strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            strSql += " COMPANYID = ISNULL((SELECT TOP 1 COMPANYID FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(COMPANYID,'') <> ''),T.COMPANYID) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(COMPANYID,'') = '' /**/"
            strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            strSql += " APPVER = ISNULL((SELECT TOP 1 APPVER FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(APPVER,'') <> ''),T.APPVER) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(APPVER,'') = '' /**/"
            strSql += " "
            strSql += " UPDATE TEMP_NEWACCTRAN SET  /**/"
            strSql += " SYSTEMID = ISNULL((SELECT TOP 1 SYSTEMID FROM TEMP_NEWACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND ISNULL(SYSTEMID,'') <> ''),T.SYSTEMID) /**/"
            strSql += " FROM TEMP_NEWACCTRAN AS T WHERE ISNULL(SYSTEMID,'') = '' /**/"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMP_NEWACCTRAN /**/"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 2000
            cmd.ExecuteNonQuery()

            'Dim tranSno As Integer = GetNewSno(TranSnoType.ACCTRANCODE, tran)
            strSql = " ALTER TABLE TEMP_NEWACCTRAN ADD KEYSNO INT IDENTITY(1,1)"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " ALTER TABLE TEMP_NEWACCTRAN ADD SNO VARCHAR(15)"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = " UPDATE TEMP_NEWACCTRAN SET SNO = KEYSNO"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
            strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
            strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@CTLID = 'ACCTRANCODE'"
            strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ACCTRAN'"
            strSql += vbCrLf + " ,@UPD_DBNAME = 'MASTER'"
            strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_NEWACCTRAN'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


            strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
            strSql += " ("
            strSql += "  SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,AMOUNT"
            strSql += " ,PCS,GRSWT,NETWT,REFNO,REFDATE"
            strSql += " ,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE"
            strSql += " ,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
            strSql += " ,CONTRA,BATCHNO,USERID,UPDATED,UPTIME"
            strSql += " ,SYSTEMID,CANCEL,CASHID,COSTID,COMPANYID"
            strSql += " )"
            strSql += " SELECT "
            strSql += "  SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,AMOUNT"
            strSql += " ,PCS,GRSWT,NETWT,REFNO,REFDATE"
            strSql += " ,PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE"
            strSql += " ,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
            strSql += " ,CONTRA,BATCHNO," & userId & "USERID,'" & Today.Date & "' UPDATED,'" & Date.Now.ToLongTimeString & "' UPTIME"
            strSql += " ,'" & systemId & "' SYSTEMID,'' CANCEL,CASHID,COSTID,COMPANYID"
            strSql += " FROM TEMP_NEWACCTRAN"
            strSql += " WHERE AMOUNT <> 0"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            'strSql = " UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT = STR((SELECT ISNULL(MAX(SNO),0) FROM " & cnStockDb & "..ACCTRAN)) "
            'strSql += " WHERE CTLID = 'ACCTRANCODE' AND CTLMODULE = 'X'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()
            tran.Commit()
            MsgBox("Transferd..")
        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
        Finally
            btnTransfer.Enabled = True
        End Try
    End Sub

    Private Sub NewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem1.Click
        btnnew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        Funcnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbCatNamefrom_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCatNamefrom.SelectedValueChanged
        If cmbCatNamefrom.Text <> "ALL" Then
            strSql = " SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME<>'" & cmbCatNamefrom.Text.ToString & "' ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        Else
            strSql = "SELECT 'ALL' CATNAME,0 RESULT UNION ALL SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        End If
    End Sub
#End Region

    Private Sub rbtAcupdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtAcupdate.CheckedChanged
        cmbCatNamefrom.Enabled = Not rbtAcupdate.Checked
        cmbCategoryTo.Enabled = Not rbtAcupdate.Checked
    End Sub

    Private Sub rbtCatUpdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtCatUpdate.CheckedChanged
        cmbCatNamefrom.Enabled = rbtCatUpdate.Checked
        cmbCategoryTo.Enabled = rbtCatUpdate.Checked
    End Sub
End Class