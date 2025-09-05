Imports System.Data.OleDb
Public Class frmNonTagCostcentreChange
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim RefNo As String

    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If cmbOldCostCentre_MAN.Text = "" Then
            MsgBox("Invalid Costcentre", MsgBoxStyle.Information)
            cmbOldCostCentre_MAN.Select()
            Exit Sub
        End If
        If cmbNewCostCenter_MAN.Text = "" Then
            MsgBox("Invalid CostCentre", MsgBoxStyle.Information)
            cmbNewCostCenter_MAN.Select()
            Exit Sub
        End If
        Dim frmCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbOldCostCentre_MAN.Text & "'", , , tran)
        Dim ToCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbNewCostCenter_MAN.Text & "'", , , tran)
        If Not ValidationPcs() Then Exit Sub
        If Not ValidationGrswt() Then Exit Sub
        If Not ValidationNetWt() Then Exit Sub
        BillDate = GetEntryDate(dtpTranDate.Value)
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            Dim NEWBILLNO As Integer
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran))
GenRefNo:
            RefNo = cnCostId & NEWBILLNO.ToString 'GetBillNoValue("GEN-STKREFNO", tran)
            strSql = "SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMTAG WHERE REFNO = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..CITEMTAG WHERE REFNO = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE REFNO = '" & RefNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenRefNo
            End If
            'If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            '    GoTo GenRefNo
            'End If
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
            strSql += " WHERE CTLID ='GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            'strSql += " AND CONVERT(INT,CTLTEXT) = '" & NEWBILLNO & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GenRefNo
            End If
            InsertIntoNonTag(frmCostId, ToCostId)
            tran.Commit()
            tran = Nothing
            MsgBox("Transfered Completed..")
            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate
            btnNew_Click(Me, New EventArgs)
            If AUTOBOOKTRAN Then
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim prnmemsuffix As String = ""
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    Dim write As IO.StreamWriter
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":SMI")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":SMI" & ";" & _
                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" & _
                        LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" & _
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub CreateInternalTransferForNonTag(ByVal TransSnos As String, ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date)
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS ITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS SUBITEMID"
        strSql += vbCrLf + " ,'G' GRSNET,'" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
        strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID"
        strSql += vbCrLf + " ,53 ORDSTATE_ID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO MASTER..TEMP_" & systemId & "TRANTRANS FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID"
        strSql += vbCrLf + " UPDATE MASTER..TEMP_" & systemId & "TRANTRANS SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'MASTER'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS'"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNSUBITEMID"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
        strSql += vbCrLf + " INTO MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = TG.ITEMID"
        strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & TransSnos & ")"

        strSql += vbCrLf + " UPDATE MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN MASTER..TEMP_" & systemId & "TRANTRANS AS T ON T.CATCODE = SV.TCATCODE AND T.COMPANYID = SV.TCOMPANYID "
        strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,CONVERT(NUMERIC(15,2),0) AS STNAMT"
        strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,COSTID,COMPANYID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " INTO MASTER..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,STNSUBITEMID,COSTID,COMPANYID"
        strSql += vbCrLf + " UPDATE MASTER..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'MASTER'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMP_" & systemId & "TRANTRANS"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", cn, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", cn, tran)

        Dim RoIns As DataRow = Nothing
        For Each Ro As DataRow In DtTag.Rows
            RoIns = DtIssue.NewRow
            For Each Col As DataColumn In DtTag.Columns
                If DtIssue.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssue.Rows.Add(RoIns)
        Next
        For Each Ro As DataRow In DtTagStone.Rows
            RoIns = DtIssStone.NewRow
            For Each Col As DataColumn In DtTagStone.Columns
                If DtIssStone.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssStone.Rows.Add(RoIns)
        Next
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
        strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        TranNo = GetBillNoValue("GEN-SM-ISS", tran)
        Batchno = GetNewBatchno(cnCostId, BillDate, tran)
        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = TranNo
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate(tran)
            Ro.Item("UPTIME") = Now
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = RefNo
            Ro.Item("REFDATE") = RefDate
        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = TranNo
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
        Next
        DtIssStone.AcceptChanges()

        InsertData(SyncMode.Transaction, DtIssue, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, cn, tran, cnCostId)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbItemType_MAN.Enabled = True
        dtpTranDate.Value = GetEntryDate(GetServerDate)
        cmbOldCostCentre_MAN.Text = cnCostName
        dtpTranDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub grpFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtOldPcs.GotFocus, txtOldGrsWt.GotFocus, txtOldNetWt.GotFocus _
    , txtNewPcs.GotFocus, txtNewGrsWt.GotFocus, txtNewNetWt.GotFocus
        dtpTranDate.Select()
    End Sub

    Private Sub GetStock(ByVal cmbCost As ComboBox)
        If cmbCost.Name = cmbOldCostCentre_MAN.Name Then objGPack.TextClear(grpOld) Else objGPack.TextClear(grpNew)
        strSql = " SELECT "
        strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG"
        strSql += " WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "),0)"
        strSql += " AND ITEMCTRID = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'),0)"
        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCost.Text & "'),'')"
        strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then Exit Sub
        With dt.Rows(0)
            If cmbCost.Name = cmbOldCostCentre_MAN.Name Then
                txtOldPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtOldGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtOldNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            Else
                txtNewPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtNewGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtNewNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            End If
        End With
    End Sub


    Private Sub GetStockpkt(ByVal cmbCost As ComboBox)
        If cmbCost.Name = cmbOldCostCentre_MAN.Name Then objGPack.TextClear(grpOld) Else objGPack.TextClear(grpNew)
        strSql = " SELECT T.ITEMID,B.ITEMNAME,T.SUBITEMID,C.SUBITEMNAME,T.ITEMCTRID,D.ITEMCTRNAME,T.COSTID,E.COSTNAME"
        strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG T "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST A ON A.ITEMID = T.ITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST B ON B.SUBITEMID = T.SUBITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER C ON C.ITEMCTRID = T.ITEMCTRID"
        strSql += " LEFT JOIN " & cnAdminDb & "..COSTCENTRE D ON C.COSTID= T.COSTID"
        strSql += " WHERE T.PACKETNO = " & Val(txtPktno.Text)


        'strSql += " WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        'strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "),0)"
        'strSql += " AND ITEMCTRID = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'),0)"
        'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCost.Text & "'),'')"

        strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += " AND ISNULL(T.CANCEL,'') <> 'Y'"
        strSql += " GROUP BY T.ITEMID,B.ITEMNAME,T.SUBITEMID,C.SUBITEMNAME,T.ITEMCTRID,D.ITEMCTRNAME,T.COSTID,E.COSTNAME"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then Exit Sub
        With dt.Rows(0)
            If cmbCost.Name = cmbOldCostCentre_MAN.Name Then
                txtOldPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtOldGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtOldNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            Else
                txtNewPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtNewGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtNewNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            End If
        End With
    End Sub



    Private Sub LoadCostDetail(ByVal tCost As ComboBox)
        GetStock(tCost)
    End Sub

    Private Sub frmNonTagCostcentreChange_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtItemId_NUM.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Function ValidationPcs() As Boolean
        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        If txtSubItemName.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
        End If
        Dim CalType As String = objGPack.GetSqlValue(strSql)
        If CalType <> "R" Then
            strSql = " SELECT ISNULL(ALLOWZEROPCS,'') ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
            If objGPack.GetSqlValue(strSql) = "Y" Then
                Return True
            End If
        End If
        If Val(txtPcs_NUM.Text) = 0 Then
            MsgBox("Pieces should not empty", MsgBoxStyle.Information)
            Return False
            'ElseIf Val(txtPcs_NUM.Text) > Val(txtOldPcs.Text) Then
            '    MsgBox("Pcs should not exceed the available stock", MsgBoxStyle.Information)
            '    Return False
        End If
        Return True
    End Function

    Private Function ValidationGrswt() As Boolean
        If txtSubItemName.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "'"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql, , "W") <> "R" Then
            If Val(txtGrsWt_WET.Text) = 0 Then
                MsgBox("GrsWt should not empty", MsgBoxStyle.Information)
                Return False
                'ElseIf Val(txtGrsWt_WET.Text) > Val(txtOldGrsWt.Text) Then
                '    MsgBox("GrsWt should not exceed the available stock", MsgBoxStyle.Information)
                '    Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidationNetWt() As Boolean
        If txtSubItemName.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "'"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql, , "W") <> "R" Then
            If Val(txtNetWt_WET.Text) = 0 Then
                MsgBox("NetWt should not empty", MsgBoxStyle.Information)
                Return False
                'ElseIf Val(txtNetWt_WET.Text) > Val(txtOldNetWt.Text) Then
                '    MsgBox("Netwt should not exceed the available stock", MsgBoxStyle.Information)
                '    Return False
            End If
        End If
        Return True
    End Function

    Private Sub txtPcs_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPcs_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationPcs() Then txtPcs_NUM.Select()
        End If
    End Sub

    Private Sub txtGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationGrswt() Then txtGrsWt_WET.Select()
        End If
    End Sub

    Private Sub txtNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationNetWt() Then txtNetWt_WET.Select()
        End If
    End Sub

    Private Sub InsertIntoNonTag(ByVal tCostId As String, ByVal TransCostId As String)
        Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
        strSql += " PCS,GRSWT,LESSWT,NETWT,"
        strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
        strSql += " PACKETNO,DREFNO,ITEMCTRID,"
        strSql += " ORDREPNO,ORSNO,NARRATION,"
        strSql += " RATE,COSTID,"
        strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
        strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,REFNO,REFDATE)VALUES("
        strSql += " '" & Sno & "'" 'SNO
        strSql += " ," & Val(txtItemId_NUM.Text) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "", , , tran)) & "" 'SubItemId
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & Val(txtPcs_NUM.Text) & "" 'Pcs
        strSql += " ," & Val(txtGrsWt_WET.Text) & "" 'GrsWt
        strSql += " ," & Val(txtGrsWt_WET.Text) - Val(txtNetWt_WET.Text) & "" 'LessWt
        strSql += " ," & Val(txtNetWt_WET.Text) & "" 'NetWt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'I'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & tCostId & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'Updated
        strSql += " ,'" & GetServerTime(tran) & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & TransCostId & "'" 'TCOSTID
        strSql += " ,'" & RefNo & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, tCostId)

        If AUTOBOOKTRAN Then
            CreateInternalTransferForNonTag("'" & Sno & "'", TransCostId, RefNo, BillDate)
        End If

        strSql = " INSERT INTO " & cnAdminDb & "..TITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
        strSql += " PCS,GRSWT,LESSWT,NETWT,"
        strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
        strSql += " PACKETNO,DREFNO,ITEMCTRID,"
        strSql += " ORDREPNO,ORSNO,NARRATION,"
        strSql += " RATE,COSTID,"
        strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
        strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,REFNO,REFDATE)VALUES("
        strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
        strSql += " ," & Val(txtItemId_NUM.Text) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "", , , tran)) & "" 'SubItemId
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & Val(txtPcs_NUM.Text) & "" 'Pcs
        strSql += " ," & Val(txtGrsWt_WET.Text) & "" 'GrsWt
        strSql += " ," & Val(txtGrsWt_WET.Text) - Val(txtNetWt_WET.Text) & "" 'LessWt
        strSql += " ," & Val(txtNetWt_WET.Text) & "" 'NetWt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'R'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & tCostId & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'Updated
        strSql += " ,'" & GetServerTime(tran) & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & TransCostId & "'" 'TCOSTID
        strSql += " ,'" & RefNo & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " )"
        Exec(strSql.Replace("'", "''"), cn, TransCostId, Nothing, tran)
    End Sub

    Private Sub LoadItem()
        strSql = " SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE IN('N','P') AND ACTIVE = 'Y' "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        Dim itemId As Integer = Nothing
        itemId = Val(BrighttechPack.SearchDialog.Show("Select ItemName", strSql, cn, 1))
        If itemId > 0 Then
            txtItemId_NUM.Text = itemId
            LoadItemDetail()
        End If
    End Sub

    Private Sub LoadItemDetail()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        strSql += GetItemQryFilteration("S")
        Dim itemName As String = Nothing
        itemName = objGPack.GetSqlValue(strSql)
        If itemName <> "" Then
            If objGPack.GetSqlValue("SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & "").ToString = "Y" Then
                cmbItemType_MAN.Enabled = True
            Else
                cmbItemType_MAN.Enabled = False
            End If
            txtItemName.Text = itemName
            Dim DefItem As String = txtSubItemName.Text
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, Val(txtItemId_NUM.Text))
            txtSubItemName.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtItemId_NUM_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtItemId_NUM_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtItemId_NUM_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & "").Length > 0 Then
                LoadItemDetail()
            Else
                LoadItem()
            End If
        End If
    End Sub

    Private Sub txtItemId_NUM_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItem()
        End If
    End Sub

    Private Sub txtSubItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub frmNonTagCostcentreChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN)
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME "
        objGPack.FillCombo(strSql, cmbItemType_MAN)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = "SELECT MAIN FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & cnCostId & "'"
            Dim ThisCO As Boolean = IIf(objGPack.GetSqlValue(strSql).ToString = "Y", True, False)
            Dim MainCostId As String = GetAdmindbSoftValue("SYNC-TO", "")
            cmbOldCostCentre_MAN.Enabled = True
            cmbNewCostCenter_MAN.Enabled = True
            If ThisCO Then
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            Else
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "' ORDER BY COSTNAME"
            End If
            objGPack.FillCombo(strSql, cmbOldCostCentre_MAN)
            If ThisCO Then
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME <> '" & cnCostName & "' ORDER BY COSTNAME"
            Else
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & MainCostId & "' ORDER BY COSTNAME"
            End If
            objGPack.FillCombo(strSql, cmbNewCostCenter_MAN)
            cmbOldCostCentre_MAN.Text = cnCostName
        Else
            cmbOldCostCentre_MAN.Enabled = False
            cmbNewCostCenter_MAN.Enabled = False
        End If

        If GetAdmindbSoftValue("ITEMCOUNTER") = "Y" Then
            cmbItemCounter_MAN.Enabled = True
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbItemCounter_MAN)
        Else
            cmbItemCounter_MAN.Enabled = False
        End If
    End Sub

    Private Sub cmbNewCostCenter_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbNewCostCenter_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            GetStock(cmbNewCostCenter_MAN)
        End If
    End Sub

    Private Sub cmbOldCostCentre_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOldCostCentre_MAN.GotFocus
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbOldCostCentre_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbOldCostCentre_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            GetStock(cmbOldCostCentre_MAN)
        End If
    End Sub

    Private Sub cmbOldCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOldCostCentre_MAN.LostFocus
        GetStock(cmbOldCostCentre_MAN)
    End Sub

    Private Sub txtItemId_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.TextChanged

    End Sub
End Class