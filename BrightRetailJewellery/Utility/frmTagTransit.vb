Imports System.Data.OleDb
Public Class frmTagTransit
    Dim strSql As String
    'Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim tagSnoStr As String = ""
    Dim nonTagSnoStr As String = ""
    Dim ToCostId As String = ""
    Dim SyncStock As String = GetAdmindbSoftValue("SYNC-STOCK", "N")
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP", "")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim Isbulkupdate As Boolean = IIf(GetAdmindbSoftValue("BULKTAGTRANSFER", "N") = "Y", True, False)
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOINTERNAL_VOUCHER As String = GetAdmindbSoftValue("AUTOBOOK_VOUCHER", "N")
    Dim CENTR_DB_GLB As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim ReplRecdate As Boolean = False 'IIf(GetAdmindbSoftValue("STKTRAN_REC_DATE", "N") = "Y", True, False)
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String = AUTOBOOKVALUEARRY(0).ToString
    Dim AUTOBOOKVALUEG_PER As Decimal = 0
    Dim AUTOBOOKVALUES_PER As Decimal = 0
    Dim AUTOBOOKVALUEP_PER As Decimal = 0
    Dim AUTOBOOKVALUED_PER As Decimal = 0
    Dim AUTOBOOKVALUET_PER As Decimal = 0
    Dim BillDate As Date
    Dim XAccode As String
    Dim TranNo As Integer
    Dim Batchno As String
    Dim XCnAdmin As OleDbConnection = Nothing
    Public Xtran As OleDbTransaction = Nothing
    Private XSyncdb As String = Replace(cnAdminDb, "ADMINDB", "UTILDB")
    Dim bulkchecked As Boolean = False
    Public objSoftKeys As New SoftKeys
    Dim TAG_DOWNONLYBYSCAN As Boolean = IIf(GetAdmindbSoftValue("TAG_DOWNONLYBYSCAN", "N") = "Y", True, False)
    Dim Authorize As Boolean = False
    Dim GEN_SKUFILE As Boolean = IIf(GetAdmindbSoftValue("GEN_SKUFILE", "N").ToUpper = "Y", True, False)
    Dim SKUFILEPATH As String = GetAdmindbSoftValue("SKUFILEPATH", "")
    Dim REQ_FRANCHISEE As Boolean = IIf(GetAdmindbSoftValue("REQ_FRANCHISEE", "N") = "Y", True, False)
    Dim _isFrachisee As Boolean = False
    Dim LOT_TRANSFER_COSTID As String = GetAdmindbSoftValue("LOT_TRANSFER_COSTID", "")
    Dim AUTOBOOKENTRY As Boolean = True
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
    Dim TagTransitValid As Boolean = IIf(GetAdmindbSoftValue("TAGTRANSIT_VALIDATION", "Y") = "Y", True, False)
    ''GST
    Dim dtCatMast As DataTable
    Dim SGST As Double = Nothing
    Dim CGST As Double = Nothing
    Dim IGST As Double = Nothing
    Dim gstAmt As Double = Nothing
    Dim fgstAmt As Double = Nothing
    Dim StateId As Integer
    Dim SGSTPER As Decimal
    Dim CGSTPER As Decimal
    Dim IGSTPER As Decimal
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE_INTTRF_REC", "")
    Dim GstRecAcc() As String
    Dim SCode As String

    Dim CCode As String
    Dim ICode As String
    Dim _txtkeydownhandled As Boolean = False
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)
    Dim STKTRAN_ContraPost As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_ACCOUNT_POST", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim STKTRAN_REPAIR_SEPPOST As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_REPAIR_SEPPOST", "N") = "Y", True, False)
    Dim OR_REP_NewCatCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT", "00018") & "'", , "00018")
    Dim OR_REP_NewCatCode_S As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_S", "00014") & "'", , "00014")
    Dim OR_REP_NewCatCode_P As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_P", "") & "'", , "")

    Private Function CreateInternalTransferForNonTag(ByVal ToCostId As String, ByVal Tranno As Long, Optional ByVal trandate As Date = Nothing) As Boolean
        'If TransSnos = "" Then Exit Sub
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & "  AS TRANTYPE, T.REFNO AS REFNO"
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS ITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS SUBITEMID"
        strSql += vbCrLf + " ,'G' GRSNET,'" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
        strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID"
        strSql += vbCrLf + " ,53 ORDSTATE_ID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        'strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
        strSql += vbCrLf + " WHERE T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='N' and MOVETYPE = 'I' "
        If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID,T.REFNO"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'RECEIPTCODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPT'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS'"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),STNAMT)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNSUBITEMID"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = TG.ITEMID"
        'strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & TransSnos & ")"
        strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='N' and MOVETYPE = 'I' "
        If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " )"

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS AS T ON T.CATCODE = SV.TCATCODE AND T.COMPANYID = SV.TCOMPANYID "
        strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
        '        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
        strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,TMETALID,COSTID,COMPANYID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,STNSUBITEMID,TMETALID,COSTID,COMPANYID"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'RECEIPTSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPTSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim DtReceipt As New DataTable
        Dim DtReceiptStone As New DataTable
        DtReceipt = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPT", XCnAdmin, tran)
        DtReceiptStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPTSTONE", XCnAdmin, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", XCnAdmin, tran)

        Dim RoIns As DataRow = Nothing
        For Each Ro As DataRow In DtTag.Rows
            RoIns = DtReceipt.NewRow
            For Each Col As DataColumn In DtTag.Columns
                If DtReceipt.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtReceipt.Rows.Add(RoIns)
        Next
        For Each Ro As DataRow In DtTagStone.Rows
            RoIns = DtReceiptStone.NewRow
            For Each Col As DataColumn In DtTagStone.Columns
                If DtReceiptStone.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtReceiptStone.Rows.Add(RoIns)
        Next

        'strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
        'strSql += " DROP TABLE TEMP" & systemId & "BILLNO"
        'cmd = New OleDbCommand(strSql, cn, tran)
        'cmd.ExecuteNonQuery()
        'TranNo = GetBillNoValue("GEN-SM-ISS", tran)

        If BillDate = Nothing Then BillDate = GetServerDate()
        If trandate = Nothing Then trandate = BillDate
        Batchno = GetNewBatchnoNew(cnCostId, BillDate, XCnAdmin, tran)
        For Each Ro As DataRow In DtReceipt.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = trandate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate()
            ' Ro.Item("UPTIME") = GetServerTime(tran)
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            If AUTOBOOKVALUEENABLE = "Y" Then
                Dim mxrate As Decimal = 0
                mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, ))
                If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                If Ro.Item("METALID").ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
                If Ro.Item("METALID").ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
                'If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                Dim mamount As Decimal = mxrate * Val(Ro.Item("NETWT").ToString)
                Dim Stnamount As Decimal = Val(DtReceiptStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE NOT IN('D')").ToString)
                Dim Diaamount As Decimal = Val(DtReceiptStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE = 'D'").ToString)
                If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
                If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
                mamount += Stnamount + Diaamount
                'Dim Stnamount As Decimal = Val(DtReceiptStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "'").ToString)
                'mamount += Stnamount
                mamount = CalcRoundoffAmt(mamount, objSoftKeys.RoundOff_Gross)
                If mamount <> 0 Then
                    If AUTOINTERNAL_VOUCHER = "Y" Then
                        Dim Roacct As DataRow = Nothing
                        Roacct = DtAcctran.NewRow
                        With Roacct
                            .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                            .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                            .Item("BATCHNO") = Batchno
                            .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                            .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TR"
                            .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                        End With
                        DtAcctran.Rows.Add(Roacct)
                        Roacct = DtAcctran.NewRow
                        With Roacct
                            .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                            .Item("BATCHNO") = Batchno
                            .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                            .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                            .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN" : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TR"
                            .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                        End With
                        DtAcctran.Rows.Add(Roacct)
                    End If
                End If
                Ro.Item("AMOUNT") = mamount
            End If

        Next
        DtReceipt.AcceptChanges()
        For Each Ro As DataRow In DtReceiptStone.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = trandate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("STONEMODE") = ""
        Next
        DtReceiptStone.AcceptChanges()

        If InsertData(SyncMode.Transaction, DtReceipt, XCnAdmin, tran, cnCostId) = False Then Return False
        If InsertData(SyncMode.Transaction, DtReceiptStone, XCnAdmin, tran, cnCostId) = False Then Return False
        If AUTOINTERNAL_VOUCHER = "Y" Then If InsertData(SyncMode.Transaction, DtAcctran, XCnAdmin, tran, cnCostId) = False Then Return False
        Return True
    End Function

    Private Function CreateInternalTransferReceipt(ByVal ToCostId As String, ByVal tranno As Long, Optional ByVal trandate As Date = Nothing, Optional ByVal tranAcode As String = Nothing)
        'If trandate = Nothing Then trandate = BillDate
        ''alter in transfer voucher generation in utility also
        Try

            If BillDate = Nothing Then BillDate = GetServerDate()
            If trandate = Nothing Then trandate = BillDate

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            ': cmd.CommandTimeout = 1000 : 
            cmd.ExecuteNonQuery()

            Dim _contraaccode As String = ""
            If STKTRAN_ContraPost Then
                _contraaccode = GetSqlValue("SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & cnCostId.ToString & "'", XCnAdmin, tran)
                If _contraaccode.ToString = "" Then
                    _contraaccode = "STKTRAN"
                End If
            Else
                _contraaccode = "STKTRAN"
            End If

            If tranAcode Is Nothing Then tranAcode = ""
            If StateId = 0 Then StateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = (SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & ToCostId.ToString & "')").ToString)

            strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,YY.REFNO " ''  'RIN' AS TRANTYPE
            strSql += vbCrLf + " ,SUM(YY.PCS)PCS,SUM(YY.GRSWT)GRSWT,SUM(YY.NETWT)NETWT,SUM(YY.LESSWT)LESSWT,SUM(PUREWT)PUREWT"
            strSql += vbCrLf + " ,yy.ITEMID,yy.SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET,0 RATE,SUM(YY.AMOUNT) AMOUNT,'" & cnCostId & "' COSTID,YY.COMPANYID,YY.PURITY,YY.CATCODE CATCODE"
            strSql += vbCrLf + " ,YY.ACCODE ACCODE"
            strSql += vbCrLf + " ,YY.METALID,STKTYPE"
            ' strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS FROM ( "

            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET,0 RATE,ISNULL(SUM(TRFVALUE),0) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            If tranAcode.ToString <> "" Then
                strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
            Else
                strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
            End If
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " )"
            If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            End If
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET, 0 RATE,ISNULL(SUM(TRFVALUE),0) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            If tranAcode.ToString <> "" Then
                strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
            Else
                strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
            End If

            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " )"
            If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            End If
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If

            If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
                strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET,0 RATE"
                ''strSql += vbCrLf + " ,ISNULL(SUM(MT.AMOUNT),0) AMOUNT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
                strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
                strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
                strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
                strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
                strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
                strSql += vbCrLf + " )/100)))))) AS AMOUNT"
                strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
                strSql += vbCrLf + " ,MT.CATCODE CATCODE"
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If
                strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG T ON T.SNO = MT.TAGSNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
                If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                End If
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.STKTYPE"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
                strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET,0 RATE"
                ''strSql += vbCrLf + " ,ISNULL(SUM(MT.AMOUNT),0) AMOUNT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
                strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
                strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
                strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
                strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
                strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
                strSql += vbCrLf + " )/100)))))) AS AMOUNT"
                strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
                strSql += vbCrLf + " ,MT.CATCODE CATCODE"
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If
                strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG T ON T.SNO = MT.TAGSNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
                If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                End If
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.STKTYPE"
            End If

            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " union all "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
                strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET,0 RATE,ISNULL(SUM(TRFVALUE),0) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
                strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
                strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If
                strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,T.ITEMTYPEID"
                End If
                strSql += vbCrLf + " union all "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
                strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET, 0 RATE,ISNULL(SUM(TRFVALUE),0) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
                strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
                strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If

                strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,T.ITEMTYPEID"
                End If
                ''OREXCESSWT
                strSql += vbCrLf + " union all "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,0 LESSWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
                strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET,0 RATE,0 AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                    strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
                Else
                    strSql += vbCrLf + " ,IM.CATCODE"
                End If
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If
                strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,T.ITEMTYPEID"
                End If
                strSql += vbCrLf + " union all "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE,T.TRANINVNO AS REFNO "
                strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,SUM(0)LESSWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
                strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
                strSql += vbCrLf + " ,'G' GRSNET, 0 RATE,0 AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                    strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
                Else
                    strSql += vbCrLf + " ,IM.CATCODE"
                End If
                If tranAcode.ToString <> "" Then
                    strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
                Else
                    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
                End If

                strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' AND STOCKMODE = 'T' AND MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
                strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,T.ITEMTYPEID"
                End If
            End If

            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE, T.REFNO AS REFNO"
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 AS SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET,0 as RATE,ISNULL(SUM(TRFVALUE),0) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
            If tranAcode.ToString <> "" Then
                strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
            Else
                strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
            End If
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            'strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='N' and MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ) GROUP BY T.REFNO,IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE "
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + "SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AS TRANTYPE, T.REFNO AS REFNO"
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
            strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 AS SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET,0 as RATE,ISNULL(SUM(TRFVALUE),0) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
            If tranAcode.ToString <> "" Then
                strSql += vbCrLf + " ,'" & tranAcode & "' AS ACCODE"
            Else
                strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
            End If
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN ('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='N' and MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ) GROUP BY T.REFNO,IM.CATCODE,T.ITEMID,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE "

            strSql += vbCrLf + " ) YY GROUP BY YY.REFNO "
            strSql += vbCrLf + " ,YY.COSTID,YY.COMPANYID,YY.PURITY,YY.CATCODE,YY.ITEMID,YY.SUBITEMID"
            strSql += vbCrLf + " ,YY.ACCODE,YY.METALID,STKTYPE"

            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " , TRANTYPE,YY.REFNO "
            strSql += vbCrLf + " ,SUM(YY.PCS)PCS,SUM(YY.GRSWT)GRSWT,SUM(YY.NETWT)NETWT,SUM(YY.LESSWT)LESSWT,SUM(PUREWT)PUREWT"
            strSql += vbCrLf + " ,yy.ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " ,'G' GRSNET,0 RATE,SUM(YY.AMOUNT) AMOUNT,'" & cnCostId & "' COSTID,YY.COMPANYID,'X'FLAG,YY.CATCODE CATCODE,YY.CATCODE OCATCODE"
            strSql += vbCrLf + " ,YY.ACCODE ACCODE"
            strSql += vbCrLf + " ,YY.METALID,53 ORDSTATE_ID,STKTYPE"
            strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS YY "
            strSql += vbCrLf + " GROUP BY SNO,TRANTYPE, YY.REFNO ,YY.COSTID,YY.COMPANYID,YY.CATCODE,YY.ITEMID"
            strSql += vbCrLf + " ,YY.ACCODE,YY.METALID,STKTYPE"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
            strSql += vbCrLf + " ,SUM(STNPCS) STNPCS,SUM(STNWT)STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,SUM(STNAMT)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(YY.STNITEMID,0) STNITEMID,0 STNSUBITEMID,YY.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,YY.COMPANYID"
            strSql += vbCrLf + " ,YY.TRANINVNO ,YY.CATCODE,YY.TCATCODE,YY.TCOMPANYID ,YY.TPURITY ,YY.TMETALID "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW FROM ( "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,0 STNSUBITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,TG.TRANINVNO TRANINVNO"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(TG.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID) ELSE TIM.CATCODE END ELSE  TIM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE"
            End If
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,TIM.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='T' and MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " )"
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
            If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
                strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
                strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,0 STNSUBITEMID,ST.STONEUNIT AS STONEUNIT"
                strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
                strSql += vbCrLf + " ,TG.TRANINVNO TRANINVNO"
                strSql += vbCrLf + " ,MT.CATCODE AS CATCODE"
                strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,TIM.METALID AS TMETALID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAGMETAL AS MT ON TG.SNO = MT.TAGSNO AND MT.SNO=ST.TAGMSNO "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='T' and MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
                strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
                If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
                End If
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
                strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
                strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,0 STNSUBITEMID,ST.STONEUNIT AS STONEUNIT"
                strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
                strSql += vbCrLf + " ,TG.TRANINVNO TRANINVNO"
                strSql += vbCrLf + " ,(SELECT TOP 1  CATCODE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) AS CATCODE"
                strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,TIM.METALID AS TMETALID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='T' and MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
                strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') NOT IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
                If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
                End If
            End If

            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
                strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
                strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
                strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,0 STNSUBITEMID,ST.STONEUNIT AS STONEUNIT"
                strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
                strSql += vbCrLf + " ,TG.TRANINVNO TRANINVNO"
                strSql += vbCrLf + " ,CASE WHEN TIM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN TIM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
                strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
                strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,TIM.METALID AS TMETALID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='T' and MOVETYPE = 'I' "
                If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If

            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ," & IIf(_isFrachisee, "'RPU'", "'RIN'") & " TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
            strSql += vbCrLf + " ,0 AS STNSUBITEMID"
            strSql += vbCrLf + " ,ST.STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,TG.REFNO TRANINVNO,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
            strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & XSyncdb & "..trans_status where FROMID = '" & ToCostId & "' AND STATUS = 'T' and STOCKMODE='N' and MOVETYPE = 'I' "
            If trandate <> Nothing Then strSql += vbCrLf + " AND TRANDATE = '" & trandate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " ) YY "
            strSql += vbCrLf + " GROUP BY YY.STNITEMID,YY.STNRATE,YY.STONEUNIT "
            strSql += vbCrLf + " ,YY.COMPANYID,YY.TRANINVNO ,YY.CATCODE ,YY.TCATCODE, YY.TCOMPANYID,YY.TPURITY ,YY.TMETALID "
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            ': cmd.CommandTimeout = 1000 :
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            tran = Nothing
            tran = XCnAdmin.BeginTransaction

            strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW SET SNO = KEYNO"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
            strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
            strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@CTLID = 'RECEIPTCODE'"
            strSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPT'"
            strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
            strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_NEW'"
            strSql += vbCrLf + " "
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
            strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW AS T ON T.CATCODE = SV.CATCODE AND T.COMPANYID = SV.TCOMPANYID AND T.REFNO = SV.TRANINVNO"
            ''strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
            strSql += vbCrLf + " ,STNITEMID,0 STNSUBITEMID,TCATCODE AS CATCODE,TMETALID AS STONEMODE,COSTID,COMPANYID,STONEUNIT"
            strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
            strSql += vbCrLf + " GROUP BY ISSSNO,TRANINVNO,TRANTYPE,STNITEMID,TCATCODE,TMETALID,COSTID,COMPANYID,STONEUNIT"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
            strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
            strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@CTLID = 'RECEIPTSTONECODE'"
            strSql += vbCrLf + " ,@CHECK_TABLENAME = 'RECEIPTSTONE'"
            strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
            strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            ': cmd.CommandTimeout = 1000 :
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()



            Dim DtTag As New DataTable
            strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(DtTag)

            Dim DtTagStone As New DataTable
            strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(DtTagStone)

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.ExecuteNonQuery()

            Dim DtReceipt As New DataTable
            Dim DtReceiptStone As New DataTable

            DtReceipt = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPT", XCnAdmin, tran)
            DtReceiptStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPTSTONE", XCnAdmin, tran)
            Dim DtAcctran As New DataTable
            DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", XCnAdmin, tran)
            Dim DtTaxtran As New DataTable
            DtTaxtran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "TAXTRAN", XCnAdmin, tran)
            Dim RoIns As DataRow = Nothing
            For Each Ro As DataRow In DtTag.Rows
                RoIns = DtReceipt.NewRow
                For Each Col As DataColumn In DtTag.Columns
                    If DtReceipt.Columns.Contains(Col.ColumnName) = False Then Continue For
                    RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
                Next
                DtReceipt.Rows.Add(RoIns)
            Next
            For Each Ro As DataRow In DtTagStone.Rows
                RoIns = DtReceiptStone.NewRow
                For Each Col As DataColumn In DtTagStone.Columns
                    If DtReceiptStone.Columns.Contains(Col.ColumnName) = False Then Continue For
                    RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
                Next
                DtReceiptStone.Rows.Add(RoIns)
            Next

            Batchno = GetNewBatchnoNew(cnCostId, trandate, XCnAdmin, tran)

            Dim RoGstTax As DataRow = Nothing
            For Each Ro As DataRow In DtTag.Rows
                GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
                If GST And StateId <> CompanyStateId And IGST > 0 Then
                    RoGstTax = DtTaxtran.NewRow
                    RoGstTax.Item("SNO") = GetNewSnoNew(TranSnoType.TAXTRANCODE, XCnAdmin, tran)  ''SNO
                    RoGstTax.Item("ISSSNO") = Ro.Item("SNO")
                    RoGstTax.Item("TRANNO") = tranno
                    RoGstTax.Item("TRANDATE") = BillDate
                    RoGstTax.Item("TRANTYPE") = IIf(_isFrachisee, "RPU", "RIN")
                    RoGstTax.Item("BATCHNO") = Batchno
                    RoGstTax.Item("TAXID") = "IG"
                    RoGstTax.Item("AMOUNT") = Ro.Item("AMOUNT")
                    If Ro.Item("AMOUNT") = "0" Then
                        IGST = 0
                    End If
                    RoGstTax.Item("TAXAMOUNT") = IGST
                    RoGstTax.Item("TAXPER") = IGSTPER
                    RoGstTax.Item("TAXTYPE") = DBNull.Value
                    RoGstTax.Item("TSNO") = "3"
                    RoGstTax.Item("COSTID") = Ro.Item("COSTID")
                    RoGstTax.Item("COMPANYID") = strCompanyId
                    DtTaxtran.Rows.Add(RoGstTax)
                End If
            Next

            For Each Ro As DataRow In DtReceipt.Rows
                Ro.Item("TRANNO") = tranno
                Ro.Item("TRANDATE") = trandate
                Ro.Item("USERID") = userId
                Ro.Item("UPDATED") = GetServerDate()
                'Ro.Item("UPTIME") = GetServerTime(tran)
                Ro.Item("APPVER") = VERSION
                Ro.Item("BATCHNO") = Batchno
                Dim mxrate As Decimal = 0
                mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, ))
                Ro.Item("BOARDRATE") = mxrate
                If GST And StateId <> CompanyStateId Then GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
                If GST And StateId <> CompanyStateId And IGST > 0 Then
                    If Ro.Item("AMOUNT") = "0" Then
                        IGST = 0
                    End If
                    Ro.Item("TAX") = IGST
                End If
                If AUTOBOOKVALUEENABLE = "Y" Or _isFrachisee Then
                    Dim mamount As Decimal = Val(Ro.Item("AMOUNT").ToString)
                    mamount = CalcRoundoffAmt(mamount, "H")
                    If mamount <> 0 Then
                        If AUTOINTERNAL_VOUCHER = "Y" Or _isFrachisee Then
                            'GST
                            If GST And StateId <> CompanyStateId Then GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
                            Dim Roacct As DataRow = Nothing
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                                .Item("BATCHNO") = Batchno
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TR"
                                .Item("CONTRA") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                                .Item("NETWT") = Ro.Item("NETWT")
                                If GST And StateId <> CompanyStateId And IGST > 0 Then
                                    .Item("AMOUNT") = Val(mamount + IGST)
                                Else
                                    .Item("AMOUNT") = mamount
                                End If

                            End With
                            DtAcctran.Rows.Add(Roacct)
                            Roacct = DtAcctran.NewRow
                            'With Roacct
                            '    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                            '    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                            '    .Item("BATCHNO") = Batchno
                            '    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                            '    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TR"
                            '    .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                            'End With
                            'DtAcctran.Rows.Add(Roacct)
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("BATCHNO") = Batchno
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN")
                                .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TR"
                                .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            'GST                        
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                Roacct = DtAcctran.NewRow
                                With Roacct
                                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                    .Item("BATCHNO") = Batchno
                                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TR"
                                    .Item("CONTRA") = Ro.Item("ACCODE")
                                    .Item("PCS") = 0
                                    .Item("GRSWT") = 0
                                    .Item("NETWT") = 0
                                    .Item("AMOUNT") = IGST
                                End With
                                DtAcctran.Rows.Add(Roacct)
                                DtAcctran.AcceptChanges()
                            End If
                        End If
                    End If
                    ' Ro.Item("AMOUNT") = mamount
                End If
            Next
            DtReceipt.AcceptChanges()
            For Each Ro As DataRow In DtReceiptStone.Rows
                Ro.Item("TRANNO") = tranno
                Ro.Item("TRANDATE") = trandate
                Ro.Item("APPVER") = VERSION
                Ro.Item("BATCHNO") = Batchno

                Ro.Item("STONEMODE") = ""
            Next
            DtReceiptStone.AcceptChanges()
            InsertData(SyncMode.Transaction, DtReceipt, XCnAdmin, tran, cnCostId)
            InsertData(SyncMode.Transaction, DtReceiptStone, XCnAdmin, tran, cnCostId)
            If AUTOINTERNAL_VOUCHER = "Y" Or _isFrachisee Then
                InsertData(SyncMode.Transaction, DtAcctran, XCnAdmin, tran, cnCostId)
                InsertData(SyncMode.Transaction, DtTaxtran, XCnAdmin, tran, cnCostId)
            End If
            strSql = " delete " & XSyncdb & "..TRANS_STATUS  WHERE STATUS = 'T' AND FROMID = '" & ToCostId & "' AND MOVETYPE = 'I'"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing

            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValuefromDt(dtSoftKeys, "PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format.ToString = "M1" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA4N("POS", Batchno.ToString, trandate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M2" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB5("POS", Batchno.ToString, trandate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M3" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA5("POS", Batchno.ToString, trandate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M4" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", Batchno.ToString, trandate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", Batchno.ToString, trandate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    ''Dim prnmemsuffix As String = ""
                    ''If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    Dim write As IO.StreamWriter
                    Dim Type As String = "SMR"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":" & Type)
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & Batchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & trandate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":" & Type & ";" &
                        LSet("BATCHNO", 15) & ":" & Batchno & ";" &
                        LSet("TRANDATE", 15) & ":" & trandate.ToString("yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If

        Catch ex As Exception
            If tran.Connection IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message)

        End Try
    End Function


    Private Function GSTCALC(ByVal grsamt As Double, ByVal catcode As String)
        ' speed access if and endif only
        If grsamt = 0 Then Exit Function
        grsamt = Math.Round(grsamt, 2)
        Dim dr() As DataRow = dtCatMast.Select("CATCODE='" & catcode & "'", Nothing)
        If dr.Length > 0 Then
            SGSTPER = Val(dr(0).Item("S_SGSTTAX").ToString)
            CGSTPER = Val(dr(0).Item("S_CGSTTAX").ToString)
            IGSTPER = Val(dr(0).Item("S_IGSTTAX").ToString)
        End If
        SGST = 0 : CGST = 0 : IGST = 0
        If SGSTPER > 0 Or CGSTPER > 0 Or IGSTPER > 0 Then
            gstAmt = grsamt
            If StateId = CompanyStateId Then
                'SGST = Math.Round(Val(gstAmt) * SGSTPER / 100, 2)
                'SGST = CalcRoundoffAmt(SGST, objSoftKeys.RoundOff_Vat)
                'CGST = Math.Round(Val(gstAmt) * CGSTPER / 100, 2)
                'CGST = CalcRoundoffAmt(CGST, objSoftKeys.RoundOff_Vat)
            Else
                IGST = Math.Round(Val(gstAmt) * IGSTPER / 100, 2)
                IGST = CalcRoundoffAmt(IGST, objSoftKeys.RoundOff_Vat)
            End If
            fgstAmt = Val(gstAmt)
        Else
            grsamt = grsamt
            SGST = 0
            CGST = 0
            IGST = 0
            fgstAmt = Val(gstAmt)
        End If
    End Function

    Private Sub StockInsert()
        Dim drStk() As DataRow = dtGrid.Select("CHECK = TRUE AND (RESULT = 1 OR RESULT = 2)")
        If Not drStk.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim TAGNOS As String = ""
        Dim obj As TrasitIssRec
        BillDate = GetEntryDate(GetServerDate)
        If BillDate < cnTranFromDate Or BillDate > cnTranToDate Then
            MsgBox("Transaction date not valid within finanical period" & vbCrLf & vbCrLf & "Unable to download In Transit data", MsgBoxStyle.Critical)
            Exit Sub
        End If
        Try
            Dim dtcostsno As New DataTable
            dtcostsno.Columns.Add("Costid", GetType(String))
            dtcostsno.Columns.Add("Sno", GetType(String))
            ToCostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'", , , tran)
            For cnt As Integer = 0 To drStk.Length - 1
                strSql = "SELECT ISNULL(STOCKTYPE,'')STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & drStk(cnt).Item("ITEMID").ToString & "'"
                If objGPack.GetSqlValue(strSql,, "").ToString = "T" Then
                    strSql = "SELECT ISNULL(TOFLAG,'')TOFLAG,BATCHNO,GRSWT,NETWT,ISNULL(CONVERT(VARCHAR(12),ISSDATE,103),'')ISSDATE,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & drStk(cnt).Item("SNO").ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim dtChk As New DataTable
                    da.Fill(dtChk)
                    If dtChk.Rows.Count > 0 And TagTransitValid = True Then
                        'If dtChk.Rows(0)("BATCHNO").ToString <> "" And dtChk.Rows(0)("ISSDATE").ToString <> "" Then
                        '    MsgBox("Tag Data Mismatched.")
                        '    Exit Sub
                        'End If

                        If dtChk.Rows(0)("ISSDATE").ToString = "" And (dtChk.Rows(0)("TOFLAG").ToString = "SA" Or dtChk.Rows(0)("TOFLAG").ToString = "MI") Then
                            MsgBox("Tag Data Mismatched." & vbCrLf + "Tagno - " + drStk(cnt).Item("TAGNO").ToString)
                            Exit Sub
                        End If

                        If dtChk.Rows(0)("TAGNO").ToString <> drStk(cnt).Item("TAGNO").ToString Then
                            MsgBox("TAGNO Mismatched." & vbCrLf + "Tagno - " + drStk(cnt).Item("TAGNO").ToString)
                            Exit Sub
                        End If

                        If dtChk.Rows(0)("ISSDATE").ToString = "" Then
                            MsgBox("Tag Issue Date Mismatched." & vbCrLf + "Tagno - " + drStk(cnt).Item("TAGNO").ToString)
                            Exit Sub
                        End If

                        'If drStk(cnt).Item("METALID").ToString <> "D" And drStk(cnt).Item("METALID").ToString <> "T" Then
                        '    If Val(dtChk.Rows(0)("GRSWT").ToString) <> Val(drStk(cnt).Item("GRSWT").ToString) Then
                        '        MsgBox("Tag Weight Mismatched.")
                        '        Exit Sub
                        '    End If
                        '    If Val(dtChk.Rows(0)("NETWT").ToString) <> Val(drStk(cnt).Item("NETWT").ToString) Then
                        '        MsgBox("Tag Weight Mismatched.")
                        '        Exit Sub
                        '    End If
                        'ElseIf drStk(cnt).Item("METALID").ToString = "D" Then
                        '    If Val(dtChk.Rows(0)("NETWT").ToString) <> Val(drStk(cnt).Item("DIAWT").ToString) Then
                        '        MsgBox("Tag Weight Mismatched.")
                        '        Exit Sub
                        '    End If
                        'ElseIf drStk(cnt).Item("METALID").ToString = "T" Then
                        '    If Val(dtChk.Rows(0)("NETWT").ToString) <> Val(drStk(cnt).Item("STNWT").ToString) Then
                        '        MsgBox("Tag Weight Mismatched.")
                        '        Exit Sub
                        '    End If
                        'End If

                    End If
                End If
                Dim drr As DataRow
                If AUTOBOOKTRAN And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & drStk(cnt).Item("COSTID").ToString & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
                If _isFrachisee And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & drStk(cnt).Item("COSTID").ToString & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
                drr = dtcostsno.NewRow
                drr("COSTID") = drStk(cnt).Item("COSTID").ToString
                drr("SNO") = drStk(cnt).Item("SNO").ToString
                dtcostsno.Rows.Add(drr)
                TAGNOS += "'" & drStk(cnt).Item("TAGNO").ToString & "',"
            Next
            Dim mstockmode As String
            For cnt As Integer = 0 To drStk.Length - 1
                Try
                    tran = Nothing
                    tran = XCnAdmin.BeginTransaction
                    'If ReplRecdate Then BillDate = drStk(cnt).Item("RECDATE")
                    obj = New TrasitIssRec(drStk(cnt).Item("COSTID").ToString, cnCostId, "R", BillDate, drStk(cnt).Item("SNO").ToString, XCnAdmin, tran, XSyncdb)
                    If drStk(cnt).Item("RESULT").ToString = "1" Then
                        If Not obj.InsertTagReceipt() Then GoTo ContFor
                        strSql = " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
                        strSql += " (COSTID,REFNO,FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
                        strSql += " SELECT '" & cnCostId & "','" & drStk(cnt).Item("REFNO").ToString & "','" & drStk(cnt).Item("COSTID").ToString & "','" & cnCostId & "','" & BillDate & "','" & drStk(cnt).Item("SNO").ToString & "','R','T'"
                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                        cmd.ExecuteNonQuery()
                        mstockmode = "T"
                    Else
                        obj.InsertNonTagReceipt()
                        mstockmode = "N"
                    End If
                    ''If Not CENTR_DB_GLB Then
                    'strSql = "INSERT INTO " & XSyncdb & "..TRANS_STATUS (FROMID,TAGSNO,STATUS,TRANNO,TRANDATE,STOCKMODE,MOVETYPE)"
                    'strSql += " SELECT '" & drStk(cnt).Item("COSTID").ToString & "','" & drStk(cnt).Item("SNO").ToString & "','T',0,'" & BillDate & "','" & mstockmode & "','I'"
                    'cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                    'cmd.ExecuteNonQuery()
                    ''End If
ContFor:
                    tran.Commit()
                    tran = Nothing
                Catch exxx As Exception
                    If tran.Connection IsNot Nothing Then tran.Rollback()
                    MsgBox(exxx.Message + vbCrLf + exxx.StackTrace)
                    Exit Sub
                End Try
            Next
            Dim NEWBILLNO As Integer = 0
            XAccode = ""
            If AUTOBOOKTRAN And AUTOBOOKENTRY Then
                If CmbAcname.Text <> "" Then XAccode = objGPack.GetSqlValue("select Accode from " & cnAdminDb & "..achead where Acname ='" & CmbAcname.Text & "'").ToString
                strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, XCnAdmin)
                cmd.ExecuteNonQuery()
                Dim billcontrolid As String = "GEN-SM-INTREC"
                strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
                If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
                    billcontrolid = "GEN-STKREFNO"
                End If
                strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
                If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "N")) = "Y" Then
                    billcontrolid = "GEN-SM-REC"
                End If

                If _isFrachisee Then
                    billcontrolid = "GEN-SM-REC"
                End If

                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
                NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , Nothing)) + 1
GenerateNewBillNo:
                strSql = "SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = " & IIf(_isFrachisee, "'RPU'", "'RIN'") & "  AND TRANNO = " & NEWBILLNO
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
                If objGPack.GetSqlValue(strSql, , "-1", Nothing) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo GenerateNewBillNo
                End If
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') = '" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, Nothing)
                If cmd.ExecuteNonQuery() = 0 Then
                    If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
                    GoTo GenerateNewBillNo
                End If


                Dim dtcr As New DataTable
                dtcr = dtcostsno.DefaultView.ToTable(True, "COSTID")
                For iirow As Integer = 0 To dtcr.Rows.Count - 1
                    Dim mCostid As String = dtcr.Rows(iirow).Item("costid").ToString
                    Dim costchk As String = "COSTID = '" & mCostid & "'"
                    Try
                        CreateInternalTransferReceipt(mCostid, NEWBILLNO, , XAccode)
                    Catch exx As Exception
                        MsgBox(exx.Message)
                        Exit Sub
                    End Try
                Next
            Else
                strSql = " DELETE " & XSyncdb & "..TRANS_STATUS WHERE STATUS = 'T'  AND MOVETYPE = 'I'"
                cmd = New OleDbCommand(strSql, XCnAdmin)
                cmd.ExecuteNonQuery()
            End If
            GenerateSkuFile(XCnAdmin, tran, "", TAGNOS)
            dtcostsno.Clear()
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Sub

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Call StockInsert()
        Exit Sub
    End Sub
    Private Sub InsertIntoNonTag()
        If Not nonTagSnoStr.Length > 0 Then Exit Sub
        strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAG"
        strSql += " WHERE SNO IN (" & nonTagSnoStr & ")"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * "
        strSql += " FROM " & cnAdminDb & "..TITEMNONTAG"
        strSql += " WHERE SNO IN (" & nonTagSnoStr & ")"
        Dim dtHost As DataTable
        dtHost = New DataTable("CITEMNONTAG")
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtHost)
        For Each row As DataRow In dtHost.Rows
            'row.Item("COSTID") = ToCostId
            'ExecQuery(SyncMode.Stock, InsertQry(row, cnStockDb), cn, tran, ToCostId, , , , False)
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAG"
            strSql += " WHERE SNO = '" & row.Item("SNO").ToString & "'"
            ExecQuery(SyncMode.Stock, strSql, XCnAdmin, tran, ToCostId, , , , False)
        Next

        strSql = " SELECT * "
        strSql += " FROM " & cnAdminDb & "..TITEMNONTAG"
        strSql += " WHERE SNO IN (" & nonTagSnoStr & ")"
        Dim dtTemp As DataTable
        dtTemp = New DataTable("ITEMNONTAG")
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTemp)
        For Each row As DataRow In dtTemp.Rows
            ExecQuery(SyncMode.Stock, InsertQry(row, cnStockDb), XCnAdmin, tran, ToCostId)
        Next

        strSql = " DELETE FROM " & cnAdminDb & "..TITEMNONTAG WHERE SNO IN (" & nonTagSnoStr & ")"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selMetalId As String = Nothing
        Dim selCostid As String = Nothing
        gridView_OWN.DataSource = Nothing
        If cmbCostCentre_MAN.Text = "" Then MsgBox("Please Proper From COST CENTRE", MsgBoxStyle.Critical) : Exit Sub

        If REQ_FRANCHISEE Then
            strSql = "SELECT COSTTYPE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'"
            If objGPack.GetSqlValue(strSql, "COSTTYPE", "B").ToString = "F" Then
                _isFrachisee = True
            End If
        End If
        AUTOBOOKENTRY = True
        If LOT_TRANSFER_COSTID <> "" Then
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & LOT_TRANSFER_COSTID & "'"
            If cmbCostCentre_MAN.Text = objGPack.GetSqlValue(strSql, "COSTNAME", "").ToString() Then
                AUTOBOOKENTRY = False
            End If
        End If
        'dtGrid.Rows.Clear()
        Dim STONEDIAMETAL As String = "'D','T'"
        Dim sql1 As String = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostCentre_MAN.Text & "'"
        selCostid = objGPack.GetSqlValue(sql1)
        If cmbMetal.Text = "ALL" Then
            selMetalId = "ALL"
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, XCnAdmin)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For i As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += dtMetal.Rows(i).Item("METALID").ToString + ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If
        chkCounter.Enabled = False
        If chkCounter.Checked Then
            If selCostid Is Nothing Then selCostid = "ALL"
            strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_TAGTRANSIT"
            strSql += vbCrLf + " @METALID = '" & selMetalId & "'"
            strSql += vbCrLf + " ,@CNADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@CNSTOCKDB='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@CNCOSTID='" & selCostid & "'"
            strSql += vbCrLf + " ,@SEARCHKEY='" & cmbSearchKey.Text & "'"
            strSql += vbCrLf + " ,@SEARCHTEXT='" & txtSearch.Text & "'"

        ElseIf chkCounter.Checked = False Then
            strSql = vbCrLf + " SELECT DISTINCT SNO,RECDATE,TRANSFERDATE,TRANINVNO AS REFNO"
            strSql += vbCrLf + " ,IT.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SIT.SUBITEMNAME aS SUBITEM,TAGNO"
            strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            strSql += vbCrLf + " ,SALVALUE,RATE,1 RESULT,T.COSTID,T.ITEMID,T.TAGKEY,T.TAGVAL,'T' STOCKMODE "
            strSql += vbCrLf + " ,CTR.ITEMCTRNAME COUNTER,C.COSTNAME AS COSTCENTRE "
            strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE,IT.METALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TITEMTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON SIT.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID = T.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID = T.COSTID "
            strSql += vbCrLf + " WHERE 1=1"
            strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"
            If txttranno.Text <> "" Then strSql += vbCrLf + " AND TRANINVNO= '" & txttranno.Text & "'"
            If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            If selCostid <> "" Then strSql += vbCrLf + " and T.COSTID ='" & selCostid & "'"
            If strBCostid <> Nothing Then strSql += vbCrLf + " and T.TCOSTID ='" & strBCostid & "'"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            End If
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT DISTINCT SNO,RECDATE,TRANSFERDATE ,TRANINVNO AS REFNO"
            strSql += vbCrLf + " ,IT.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SIT.SUBITEMNAME aS SUBITEM,TAGNO"
            strSql += vbCrLf + " ,0 PCS,0 GRSWT,0 NETWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
            strSql += vbCrLf + " ,SALVALUE,RATE,1 RESULT,T.COSTID,T.ITEMID,T.TAGKEY,T.TAGVAL,'T' STOCKMODE "
            strSql += vbCrLf + " ,CTR.ITEMCTRNAME COUNTER,C.COSTNAME AS COSTCENTRE "
            strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE,IT.METALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TITEMTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON SIT.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID = T.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID = T.COSTID "
            strSql += vbCrLf + " WHERE 1=1"
            strSql += vbCrLf + " AND IT.METALID IN(" & STONEDIAMETAL & ")"
            If txttranno.Text <> "" Then strSql += vbCrLf + " AND TRANINVNO= '" & txttranno.Text & "'"
            If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            If selCostid <> "" Then strSql += vbCrLf + " and T.COSTID ='" & selCostid & "'"
            If strBCostid <> Nothing Then strSql += vbCrLf + " and T.TCOSTID ='" & strBCostid & "'"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            End If
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT DISTINCT SNO,RECDATE,RECDATE AS TRANSFERDATE,REFNO AS REFNO"
            strSql += vbCrLf + " ,IT.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SIT.SUBITEMNAME aS SUBITEM,NULL TAGNO"
            strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
            strSql += vbCrLf + " ,NULL SALVALUE,NULL RATE,2 RESULT,T.COSTID,T.ITEMID,NULL TAGKEY,NULL TAGVAL,'N' STOCKMODE"
            strSql += vbCrLf + " ,CTR.ITEMCTRNAME COUNTER,C.COSTNAME AS COSTCENTRE "
            strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE,IT.METALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TITEMNONTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON SIT.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID = T.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID = T.COSTID "
            strSql += vbCrLf + " where 1=1 "
            strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"
            If selCostid <> "" Then strSql += vbCrLf + " and T.COSTID ='" & selCostid & "'"
            If strBCostid <> Nothing Then strSql += vbCrLf + " and T.TCOSTID ='" & strBCostid & "'"
            If txttranno.Text <> "" Then strSql += vbCrLf + " AND REFNO= '" & txttranno.Text & "'"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            End If
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT DISTINCT SNO,RECDATE,RECDATE AS TRANSFERDATE,REFNO AS REFNO"
            strSql += vbCrLf + " ,IT.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SIT.SUBITEMNAME aS SUBITEM,NULL TAGNO"
            strSql += vbCrLf + " ,0 PCS,0 GRSWT,0 NETWT"
            strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
            strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
            strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
            strSql += vbCrLf + " ,NULL SALVALUE,NULL RATE,2 RESULT,T.COSTID,T.ITEMID,NULL TAGKEY,NULL TAGVAL,'N' STOCKMODE"
            strSql += vbCrLf + " ,CTR.ITEMCTRNAME COUNTER,C.COSTNAME AS COSTCENTRE "
            strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE,IT.METALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TITEMNONTAG T "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON SIT.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID = T.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID = T.COSTID "
            strSql += vbCrLf + " where 1=1 "
            strSql += vbCrLf + " AND IT.METALID IN(" & STONEDIAMETAL & ")"
            If selCostid <> "" Then strSql += vbCrLf + " and T.COSTID ='" & selCostid & "'"
            If strBCostid <> Nothing Then strSql += vbCrLf + " and T.TCOSTID ='" & strBCostid & "'"
            If txttranno.Text <> "" Then strSql += vbCrLf + " AND REFNO= '" & txttranno.Text & "'"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            End If
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            strSql += vbCrLf + " ORDER BY RESULT"
        End If

        'dtGrid = New DataTable
        Dim dtFill As New DataTable
        Dim dtCol As DataColumn
        dtCol = New DataColumn("CHECK", GetType(Boolean))
        If chkAll.Checked Then dtCol.DefaultValue = True Else dtCol.DefaultValue = False
        dtFill.Columns.Add(dtCol)
        dtCol = New DataColumn("KEYNO", GetType(Integer))
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtFill.Columns.Add(dtCol)
        Me.Refresh()
        cmd = New OleDbCommand(strSql, XCnAdmin) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtFill)
        If dtFill.Rows.Count > 0 Then
            If dtGrid.Rows.Count > 0 Then
                For k As Integer = 0 To dtGrid.Rows.Count - 1
                    If dtGrid.Rows(k).Item("RESULT").ToString = "3" Or dtGrid.Rows(k).Item("RESULT").ToString = "4" Then
                        dtGrid.Rows(k).Delete()
                    End If
                Next
                dtGrid.AcceptChanges()


                'If Ro.Item("STOCKMODE").ToString <> "N" Then
                '    If Isgridexist(Ro.Item("SNO").ToString, Ro.Item("STOCKMODE").ToString) = False Then
                '        dtGridView.ImportRow(Ro)
                '    End If
                'Else
                '    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & Ro.Item("SNO").ToString & "' AND STOCKMODE = '" & Ro.Item("STOCKMODE").ToString & "'")
                '    If RowCol.Length = 0 Then
                '        dtGridView.ImportRow(Ro)
                '    End If

                'End If



                For k As Integer = 0 To dtFill.Rows.Count - 1
                    If Isgridexist_new(dtFill.Rows(k).Item("SNO").ToString, dtFill.Rows(k).Item("STOCKMODE").ToString, dtGrid) = True Then
                        dtFill.Rows(k).Delete()
                    End If
                Next
                dtFill.AcceptChanges()
                dtGrid.Merge(dtFill)
                dtGrid.AcceptChanges()
            Else
                dtGrid.Merge(dtFill)
            End If
        End If
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnSearch.Focus()
            Exit Sub
        End If
        cmbCostCentre_MAN.Enabled = False
        If Val(dtGrid.Compute("COUNT(ITEM)", "CHECK = 'TRUE'").ToString) > 0 Then
            btnTransfer.Enabled = True
        Else
            btnTransfer.Enabled = False
        End If

        If chkCounter.Checked = True Then
            Dim DTMETALDET As New DataTable
            If cmbMetal.Text = "ALL" Then
                strSql = " SELECT (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  WHERE METALNAME=T.TMETAL ) ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(PSTNPCS)PSTNPCS,SUM(PSTNWT)PSTNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPDB..TEMPTAGTRANSIT T WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY TMETAL"

                da = New OleDbDataAdapter(strSql, XCnAdmin)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtGrid.Merge(DTMETALDET)
                End If
            ElseIf cmbMetal.Text <> "" Then
                strSql = " SELECT (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST  WHERE METALNAME=T.TMETAL ) ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(PSTNPCS)PSTNPCS,SUM(PSTNWT)PSTNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPDB..TEMPTAGTRANSIT T WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY TMETAL"

                da = New OleDbDataAdapter(strSql, XCnAdmin)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtGrid.Merge(DTMETALDET)
                End If
            End If
        End If

        Dim Ro As DataRow = Nothing
        If chkCounter.Checked = False Then
            Ro = dtGrid.NewRow
            Ro.Item("ITEM") = "TOTAL"
            Ro.Item("TAGNO") = Val(dtGrid.Compute("COUNT(TAGNO)", "RESULT IN (1,2)").ToString)
            Ro.Item("PCS") = Val(dtGrid.Compute("SUM(PCS)", "RESULT IN (1,2)").ToString)
            Ro.Item("GRSWT") = Val(dtGrid.Compute("SUM(GRSWT)", "RESULT IN (1,2)").ToString)
            Ro.Item("NETWT") = Val(dtGrid.Compute("SUM(NETWT)", "RESULT IN (1,2)").ToString)
            Ro.Item("STNPCS") = Val(dtGrid.Compute("SUM(STNPCS)", "RESULT IN (1,2)").ToString)
            Ro.Item("STNWT") = Val(dtGrid.Compute("SUM(STNWT)", "RESULT IN (1,2)").ToString)
            Ro.Item("PSTNPCS") = Val(dtGrid.Compute("SUM(PSTNPCS)", "RESULT IN (1,2)").ToString)
            Ro.Item("PSTNWT") = Val(dtGrid.Compute("SUM(PSTNWT)", "RESULT IN (1,2)").ToString)
            Ro.Item("DIAPCS") = Val(dtGrid.Compute("SUM(DIAPCS)", "RESULT IN (1,2)").ToString)
            Ro.Item("DIAWT") = Val(dtGrid.Compute("SUM(DIAWT)", "RESULT IN (1,2)").ToString)

            If chkCounter.Checked Then
                Ro.Item("STNPCS") = Val(dtGrid.Compute("SUM(STNPCS)", "RESULT IN (5)").ToString)
                Ro.Item("STNWT") = Val(dtGrid.Compute("SUM(STNWT)", "RESULT IN (5)").ToString)
            End If
            Ro.Item("SALVALUE") = Val(dtGrid.Compute("SUM(SALVALUE)", "RESULT IN (1,2)").ToString)
            Ro.Item("RESULT") = 3
            dtGrid.Rows.Add(Ro)
        End If

        Ro = dtGrid.NewRow
        Ro.Item("ITEM") = "SELECTED"
        Ro.Item("TAGNO") = Val(dtGrid.Compute("COUNT(TAGNO)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        Ro.Item("PCS") = Val(dtGrid.Compute("SUM(PCS)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        Ro.Item("GRSWT") = Val(dtGrid.Compute("SUM(GRSWT)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        Ro.Item("NETWT") = Val(dtGrid.Compute("SUM(NETWT)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        If chkCounter.Checked Then
            Ro.Item("STNPCS") = Val(dtGrid.Compute("SUM(STNPCS)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
            Ro.Item("STNWT") = Val(dtGrid.Compute("SUM(STNWT)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
            Ro.Item("DIAPCS") = Val(dtGrid.Compute("SUM(DIAPCS)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
            Ro.Item("DIAWT") = Val(dtGrid.Compute("SUM(DIAWT)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        End If
        Ro.Item("SALVALUE") = Val(dtGrid.Compute("SUM(SALVALUE)", "CHECK = 'TRUE' AND RESULT IN (1,2)").ToString)
        Ro.Item("RESULT") = 4
        dtGrid.Rows.Add(Ro)
        dtGrid.AcceptChanges()

        gridView_OWN.DataSource = dtGrid
        FormatGridColumns(gridView_OWN, False, , False, False)
        For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
            gridView_OWN.Columns(cnt).ReadOnly = True
        Next
        gridView_OWN.Columns("CHECK").ReadOnly = False
        gridView_OWN.Columns("RESULT").Visible = False
        gridView_OWN.Columns("COSTID").Visible = False
        gridView_OWN.Columns("KEYNO").Visible = False
        gridView_OWN.Columns("ITEMID").Visible = False
        gridView_OWN.Columns("TAGKEY").Visible = False
        gridView_OWN.Columns("TAGVAL").Visible = False
        If SPECIFICFORMAT.ToString <> "1" Then
            gridView_OWN.Columns("SALVALUE").Visible = False
            If gridView_OWN.Columns.Contains("RATE") Then gridView_OWN.Columns("RATE").Visible = False
        End If
        gridView_OWN.Columns("SNO").Visible = False
        gridView_OWN.Columns("STKTYPE").HeaderText = "STOCKTYPE"
        If gridView_OWN.Columns.Contains("STOCKMODE") Then gridView_OWN.Columns("STOCKMODE").Visible = False
        If chkCounter.Checked = True Then
            gridView_OWN.Columns("COLHEAD").Visible = False
            gridView_OWN.Columns("TITEM").Visible = False
            gridView_OWN.Columns("TCOUNTER").Visible = False
            gridView_OWN.Columns("TMETAL").Visible = False
            gridView_OWN.Columns("SNO").Visible = False
        End If
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.BackColor = Color.Aqua
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.SelectionBackColor = Color.Aqua
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.SelectionForeColor = Color.Red
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.ForeColor = Color.Red
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        If chkCounter.Checked = False Then
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle.BackColor = Color.Aqua
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle.SelectionBackColor = Color.Aqua
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle.SelectionForeColor = Color.Red
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle.ForeColor = Color.Red
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        End If
        If chkCounter.Checked Then GridViewFormat()
        gridView_OWN.Focus()
        LoadTransactionType()
        Prop_Sets()
    End Sub

    Private Sub LoadTransactionType()
        If dtGrid.Rows.Count > 0 Then
            CmbStockType.Items.Clear()
            Dim ExistingTranType As New List(Of String)
            Dim Row() As DataRow = dtGrid.Select("STKTYPE <> ''")
            For Each Ro As DataRow In Row
                ExistingTranType.Add(Ro.Item("STKTYPE").ToString)
            Next
            If ExistingTranType.Count > 0 Then
                CmbStockType.Items.Add(ExistingTranType(0))
                CmbStockType.SelectedIndex = 0
            Else
                CmbStockType.Items.Add("ALL")
                CmbStockType.Items.Add("TRADING")
                CmbStockType.Items.Add("EXEMPTED")
                CmbStockType.Items.Add("MANUFACTURING")
                CmbStockType.SelectedIndex = 0
            End If
        Else
            CmbStockType.Items.Clear()
            CmbStockType.Items.Add("ALL")
            CmbStockType.Items.Add("TRADING")
            CmbStockType.Items.Add("EXEMPTED")
            CmbStockType.Items.Add("MANUFACTURING")
            CmbStockType.SelectedIndex = 0
        End If
    End Sub
    Private Function Isgridexist(ByVal sno As String, ByVal dtgrid As DataTable) As Boolean
        For ii As Integer = 0 To dtgrid.Rows.Count - 1
            If dtgrid.Rows(ii).Item("SNO").ToString <> "" Then
                If dtgrid.Rows(ii).Item("SNO").ToString = sno Then
                    Return True
                    Exit Function
                End If
            End If
        Next
        Return False
    End Function

    Private Function Isgridexist_new(ByVal sno As String, ByVal stockmode As String, ByVal dtgrid As DataTable) As Boolean
        For ii As Integer = 0 To dtgrid.Rows.Count - 1
            If dtgrid.Rows(ii).Item("SNO").ToString <> "" Then
                If dtgrid.Rows(ii).Item("SNO").ToString = sno And dtgrid.Rows(ii).Item("STOCKMODE").ToString = stockmode Then
                    Return True
                    Exit Function
                End If
            End If
        Next
        Return False
    End Function

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.MistyRose
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "N"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "M"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtGrid.Rows.Clear()
        dtGrid.AcceptChanges()
        dtGrid = New DataTable
        cmbCostCentre_MAN.Enabled = True
        chkAll.Checked = False
        strSql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='TITEMTAG')"
        strSql += vbCrLf + " AND NAME IN(SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='TITEMNONTAG'))"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        txtSearch.Text = ""
        chkCounter.Enabled = True
        Me.Refresh()
        chkCounter.Focus()
        LoadTransactionType()
        CmbStockType.Text = "ALL"
        CmbStockType.Enabled = True
        _isFrachisee = False
        AUTOBOOKENTRY = True
        Prop_Gets()
    End Sub

    Private Sub frmTagTransit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Focused And txttranno.Text = "" Then Exit Sub
            'If txtItemId.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagTransit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If XSyncdb = "" Then
        'XSyncdb = cnAdminDb

        strSql = " SELECT * FROM " & cnAdminDb & "..CATEGORY "
        Dim daCat As New OleDbDataAdapter()
        daCat = New OleDbDataAdapter(strSql, cn)
        dtCatMast = New DataTable
        daCat.Fill(dtCatMast)

        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If TAG_DOWNONLYBYSCAN Then btnSearch.Enabled = False ''And Authorize = False
        If XSyncdb <> "" Then

            If UCase(ConInfo.lDbLoginType = "W") Then
                XCnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & XSyncdb & ";Data Source=" & ConInfo.lServerName & "")
            Else
                Dim passWord As String = ConInfo.lDbPwd
                If passWord <> "" Then passWord = BrighttechPack.Methods.Decrypt(passWord)
                XCnAdmin = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & XSyncdb & ";Data Source=" & ConInfo.lServerName & ";user id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "sa") & ";password=" & passWord & ";")
            End If
            'XCnAdmin = cn
            XCnAdmin.Open()
        Else
            XCnAdmin = cn
        End If
        Dim mtrandate As Date = GetEntryDate(GetServerDate)
        If mtrandate < cnTranFromDate Or mtrandate > cnTranToDate Then
            MsgBox("Transaction date not within finanical period" & vbCrLf & vbCrLf & "Unable to download In Transit data", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If GstRecCode.Contains(",") Then
            GstRecAcc = GstRecCode.Split(",")
            If GstRecAcc.Length <> 3 Then
                MsgBox("GST Account not set Properly Control Id[GSTACCODE_INTTRF].", MsgBoxStyle.Information)
            Else
                SCode = GstRecAcc(0).ToString
                CCode = GstRecAcc(1).ToString
                ICode = GstRecAcc(2).ToString
            End If
        End If

        If AUTOBOOKTRAN = True Then
            'If AUTOBOOKVALUEARRY.Length < 6 Then MsgBox("Please Reset the value <AUTOBOOKVALUE> ex(N,0,0,0,0,0,0)") : Me.Close()
            'AUTOBOOKVALUEENABLE = AUTOBOOKVALUEARRY(0).ToString
            'AUTOBOOKVALUEG_PER = Val(AUTOBOOKVALUEARRY(1).ToString)
            'AUTOBOOKVALUES_PER = Val(AUTOBOOKVALUEARRY(2).ToString)
            'AUTOBOOKVALUEP_PER = Val(AUTOBOOKVALUEARRY(3).ToString)
            'AUTOBOOKVALUED_PER = Val(AUTOBOOKVALUEARRY(4).ToString)
            'AUTOBOOKVALUET_PER = Val(AUTOBOOKVALUEARRY(5).ToString)
            strSql = "SELECT count(*),FROMID,TRANDATE FROM " & XSyncdb & "..trans_status where STATUS = 'T' and MOVETYPE = 'I' GROUP BY FROMID,TRANDATE"

            da = New OleDbDataAdapter(strSql, XCnAdmin)
            Dim dtPendRIN As New DataTable()
            da.Fill(dtPendRIN)
            If Not (dtPendRIN Is Nothing) And dtPendRIN.Rows.Count > 0 Then
                MsgBox("Internal transfer receipt are pending.." & vbCrLf & "Now on create the voucher", MsgBoxStyle.OkOnly)
                strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                Dim billcontrolid As String = "GEN-SM-INTREC"
                strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If UCase(objGPack.GetSqlValue(strSql)) <> "Y" Then
                    billcontrolid = "GEN-STKREFNO"
                End If
                strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
                If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "N")) = "Y" Then
                    billcontrolid = "GEN-SM-REC"
                End If

                If _isFrachisee Then
                    billcontrolid = "GEN-SM-REC"
                End If

                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                Dim NEWBILLNO As Long = Val(objGPack.GetSqlValue(strSql, , , Nothing)) + 1
GenerateNewBillNo:
                'RefNo = cnCostId & NEWBILLNO.ToString
                strSql = "SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = " & IIf(_isFrachisee, "'RPU'", "'RIN'") & " AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
                If objGPack.GetSqlValue(strSql, , "-1", Nothing) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo GenerateNewBillNo
                End If

                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, XCnAdmin, Nothing)
                For ir As Integer = 0 To dtPendRIN.Rows.Count - 1
                    Try
                        Dim Trandate As Date
                        If IsDBNull(dtPendRIN.Rows(ir).Item("TRANDATE")) Then Trandate = Nothing Else Trandate = dtPendRIN.Rows(ir).Item("TRANDATE")
                        CreateInternalTransferReceipt(dtPendRIN.Rows(ir).Item("FROMID").ToString, NEWBILLNO, Trandate) '= False Then Throw New Exception("Roll Back")
                    Catch exxx As Exception
                        If tran.Connection IsNot Nothing Then tran.Rollback()
                        MsgBox(exxx.Message + vbCrLf + exxx.StackTrace)
                        Exit Sub
                    End Try
                Next
            End If
        End If
        btnNew_Click(Me, New EventArgs)
        '' LOAD METAL
        strSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strSql, XCnAdmin)
        Dim dtMetal As New DataTable()
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")

        'strSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        'strSql += vbCrLf + " UNION ALL"
        strSql = vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)

        '        BrighttechPack.GlobalMethods.FillCombo(cmbCostCentre, dtCost, "COSTNAME", , )



        'For Each Row As DataRow In dtMetal.Rows
        '    cmbMetal.Items.Add(Row.Item("METALNAME").ToString)
        'Next
    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView_OWN.CellBeginEdit
        If gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "3" Or gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "4" Or gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "6" Or gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "7" Or gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "8" Or gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "0" Then
            e.Cancel = True
        End If
    End Sub

    'Private CellValChange As Boolean = False
    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellValueChanged
        Dim blnCheck As Boolean = False
        If Not e.RowIndex > -1 Then Exit Sub
        gridView_OWN.Update()
        If Not bulkchecked And e.ColumnIndex = 0 Then computeTotal()
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView_OWN.EditingControlShowing
        gridView_OWN.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Function textBoxInputValidation(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
        If UCase(e.KeyCode) = Keys.A _
            Or UCase(e.KeyCode) = Keys.B _
            Or UCase(e.KeyCode) = Keys.C _
            Or UCase(e.KeyCode) = Keys.D _
            Or UCase(e.KeyCode) = Keys.E _
            Or UCase(e.KeyCode) = Keys.F _
            Or UCase(e.KeyCode) = Keys.G _
            Or UCase(e.KeyCode) = Keys.H _
            Or UCase(e.KeyCode) = Keys.I _
            Or UCase(e.KeyCode) = Keys.J _
            Or UCase(e.KeyCode) = Keys.K _
            Or UCase(e.KeyCode) = Keys.L _
            Or UCase(e.KeyCode) = Keys.M _
            Or UCase(e.KeyCode) = Keys.N _
            Or UCase(e.KeyCode) = Keys.O _
            Or UCase(e.KeyCode) = Keys.P _
            Or UCase(e.KeyCode) = Keys.Q _
            Or UCase(e.KeyCode) = Keys.R _
            Or UCase(e.KeyCode) = Keys.S _
            Or UCase(e.KeyCode) = Keys.T _
            Or UCase(e.KeyCode) = Keys.U _
            Or UCase(e.KeyCode) = Keys.V _
            Or UCase(e.KeyCode) = Keys.W _
            Or UCase(e.KeyCode) = Keys.X _
            Or UCase(e.KeyCode) = Keys.Y _
            Or UCase(e.KeyCode) = Keys.Z _
            Or UCase(e.KeyCode) = Keys.NumPad0 _
            Or UCase(e.KeyCode) = Keys.NumPad1 _
            Or UCase(e.KeyCode) = Keys.NumPad2 _
            Or UCase(e.KeyCode) = Keys.NumPad3 _
            Or UCase(e.KeyCode) = Keys.NumPad4 _
            Or UCase(e.KeyCode) = Keys.NumPad5 _
            Or UCase(e.KeyCode) = Keys.NumPad6 _
            Or UCase(e.KeyCode) = Keys.NumPad7 _
            Or UCase(e.KeyCode) = Keys.NumPad8 _
            Or UCase(e.KeyCode) = Keys.NumPad9 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function textBoxInputValidation_keypress(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If UCase(e.KeyChar) = Chr(Keys.A) _
            Or UCase(e.KeyChar) = Chr(Keys.B) _
            Or UCase(e.KeyChar) = Chr(Keys.C) _
            Or UCase(e.KeyChar) = Chr(Keys.D) _
            Or UCase(e.KeyChar) = Chr(Keys.E) _
            Or UCase(e.KeyChar) = Chr(Keys.F) _
            Or UCase(e.KeyChar) = Chr(Keys.G) _
            Or UCase(e.KeyChar) = Chr(Keys.H) _
            Or UCase(e.KeyChar) = Chr(Keys.I) _
            Or UCase(e.KeyChar) = Chr(Keys.J) _
            Or UCase(e.KeyChar) = Chr(Keys.K) _
            Or UCase(e.KeyChar) = Chr(Keys.L) _
            Or UCase(e.KeyChar) = Chr(Keys.M) _
            Or UCase(e.KeyChar) = Chr(Keys.N) _
            Or UCase(e.KeyChar) = Chr(Keys.O) _
            Or UCase(e.KeyChar) = Chr(Keys.P) _
            Or UCase(e.KeyChar) = Chr(Keys.Q) _
            Or UCase(e.KeyChar) = Chr(Keys.R) _
            Or UCase(e.KeyChar) = Chr(Keys.S) _
            Or UCase(e.KeyChar) = Chr(Keys.T) _
            Or UCase(e.KeyChar) = Chr(Keys.U) _
            Or UCase(e.KeyChar) = Chr(Keys.V) _
            Or UCase(e.KeyChar) = Chr(Keys.W) _
            Or UCase(e.KeyChar) = Chr(Keys.X) _
            Or UCase(e.KeyChar) = Chr(Keys.Y) _
            Or UCase(e.KeyChar) = Chr(Keys.Z) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad0) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad1) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad2) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad3) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad4) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad5) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad6) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad7) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad8) _
            Or UCase(e.KeyChar) = Chr(Keys.NumPad9) _
            Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub txtTagNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTagNo.KeyDown
        _txtkeydownhandled = False
        If textBoxInputValidation(e) = True And TAG_DOWNONLYBYSCAN = True Then
            e.Handled = True
            _txtkeydownhandled = True
        End If
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If (textBoxInputValidation_keypress(e) = True Or _txtkeydownhandled) And TAG_DOWNONLYBYSCAN = True Then
            e.Handled = True
        End If

        If e.KeyChar = Chr(Keys.Enter) Then
            Dim ITEMID As Integer = Nothing
            Dim TAGNO As String = Nothing
            Dim sp() As String = txtTagNo.Text.Split(PRODTAGSEP)
            If TAG_DOWNONLYBYSCAN Then
                If PRODTAGSEP <> "" And txtTagNo.Text <> "" And txtTagNo.Text.Contains(PRODTAGSEP) Then
                    sp = txtTagNo.Text.Split(PRODTAGSEP)
                    ITEMID = Val(Trim(sp(0).ToString))
                    TAGNO = Trim(sp(1).ToString) ''Val(Trim(sp(1)))
                    txtTagNo.Text = TAGNO
                    btnSearch_Click(Me, New EventArgs)
                ElseIf PRODTAGSEP <> "" And txtTagNo.Text <> "" And txtTagNo.Text.Contains(PRODTAGSEP) = False Then
                    strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..TITEMTAG WHERE TAGKEY = '" & txtTagNo.Text & "'"
                    Dim dtItemDet As New DataTable
                    da = New OleDbDataAdapter(strSql, XCnAdmin)
                    da.Fill(dtItemDet)
                    If dtItemDet.Rows.Count > 0 Then
                        ITEMID = dtItemDet.Rows(0).Item("ITEMID").ToString
                        TAGNO = dtItemDet.Rows(0).Item("TAGNO").ToString
                        txtTagNo.Text = TAGNO
                        btnSearch_Click(Me, New EventArgs)
                    Else
                        MsgBox("TagNo not found", MsgBoxStyle.Information)
                        txtTagNo.Clear()
                        txtTagNo.Select()
                        Exit Sub
                    End If
                End If
                If TAGNO <> "" And ITEMID <> 0 Then
                    Dim rows() As DataRow = Nothing
                    If dtGrid.Rows.Count = 0 Then Exit Sub
                    rows = dtGrid.Select("TAGNO = '" & TAGNO & "' AND ITEMID = " & ITEMID & "")
                    If rows IsNot Nothing Then
                        If rows.Length > 0 Then
                            gridView_OWN.Rows(gridView_OWN.Rows.Count - 3).Cells("CHECK").Value = True
                            gridView_OWN.CurrentCell = gridView_OWN.Rows(rows(0).Item("KEYNO")).Cells("CHECK")
                            txtTagNo.Clear()
                        End If
                    End If
                Else
                    MsgBox("TagNo not found", MsgBoxStyle.Information)
                    txtTagNo.Clear()
                    txtTagNo.Select()
                End If
            Else
                If PRODTAGSEP <> "" And txtTagNo.Text <> "" And txtTagNo.Text.Contains(PRODTAGSEP) Then
                    sp = txtTagNo.Text.Split(PRODTAGSEP)
                    ITEMID = Val(Trim(sp(0)))
                End If
                If txtTagNo.Text.StartsWith("#") Then txtTagNo.Text = txtTagNo.Text.Remove(0, 1)
CheckItem:
                If txtTagNo.Text = "" Then

                    Exit Sub
                ElseIf txtTagNo.Text <> "" And ITEMID = 0 Then
                    strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..TITEMTAG WHERE TAGVAL = " & Val(txtTagNo.Text) & ""
                    Dim dtItemDet As New DataTable
                    da = New OleDbDataAdapter(strSql, XCnAdmin)
                    da.Fill(dtItemDet)
                    If dtItemDet.Rows.Count > 0 Then
                        ITEMID = dtItemDet.Rows(0).Item("ITEMID").ToString
                        TAGNO = dtItemDet.Rows(0).Item("TAGNO").ToString
                        GoTo CheckItem
                    Else
                        MsgBox("TagNo not found", MsgBoxStyle.Information)
                        txtTagNo.Clear()
                        txtTagNo.Select()
                        Exit Sub
                    End If
                End If
                If sp.Length > 1 And PRODTAGSEP <> "" And ITEMID <> 0 Then
                    TAGNO = Trim(sp(1))
                End If
                If TAGNO <> "" And ITEMID <> 0 Then
                    Dim rows() As DataRow = Nothing
                    If dtGrid.Rows.Count = 0 Then Exit Sub
                    rows = dtGrid.Select("TAGNO = '" & TAGNO & "' AND ITEMID = " & ITEMID & "")
                    If rows IsNot Nothing Then
                        If rows.Length > 0 Then
                            gridView_OWN.Rows(rows(0).Item("KEYNO")).Cells("CHECK").Value = True
                            gridView_OWN.CurrentCell = gridView_OWN.Rows(rows(0).Item("KEYNO")).Cells("CHECK")
                            txtTagNo.Clear()
                        End If
                    End If
                Else
                    MsgBox("TagNo not found", MsgBoxStyle.Information)
                    txtTagNo.Clear()
                    txtTagNo.Select()
                End If
            End If
        End If
    End Sub

    Private Sub gridView_OWN_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellClick
        bulkchecked = False
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmTagTransit_Properties
        obj.p_cmbMetal = cmbMetal.Text
        SetSettingsObj(obj, Me.Name, GetType(frmTagTransit_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagTransit_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagTransit_Properties))
        cmbMetal.Text = obj.p_cmbMetal
    End Sub

    Private Sub chkCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounter.CheckedChanged
        If chkCounter.Checked Then
            cmbMetal.Enabled = True
            dtGrid.Rows.Clear()
        Else
            cmbMetal.Enabled = False
            dtGrid.Rows.Clear()
        End If
    End Sub

    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        bulkchecked = True
        For ii As Integer = 0 To Me.gridView_OWN.RowCount - 1
            gridView_OWN.Rows(ii).Cells("CHECK").Value = chkAll.Checked
        Next
        gridView_OWN.Update()
        Call computeTotal()
    End Sub
    Private Sub computeTotal()
        If gridView_OWN.Rows.Count = 0 Then Exit Sub
        Dim dtselect As New DataTable
        gridView_OWN.Update()
        dtselect = gridView_OWN.DataSource
        dtselect.AcceptChanges()
        If dtselect.Rows.Count = 0 Then Exit Sub
        Dim cnt As Integer = Val(dtselect.Compute("COUNT(CHECK)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        Dim pcs As Decimal = Val(dtselect.Compute("SUM(PCS)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        Dim grsWt As Decimal = Val(dtselect.Compute("SUM(GRSWT)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        Dim netWT As Decimal = Val(dtselect.Compute("SUM(NETWT)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        Dim salValue As Decimal = Val(dtselect.Compute("SUM(SALVALUE)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("SUBITEM").Value = "TAG COUNT : " & cnt
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("SUBITEM").Style.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("GRSWT").Value = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("NETWT").Value = IIf(netWT <> 0, Format(netWT, "0.000"), DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("SALVALUE").Value = IIf(salValue <> 0, Format(salValue, "0.000"), DBNull.Value)
        Dim Stnpcs As Decimal = Val(dtselect.Compute("SUM(STNPCS)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        Dim StnWt As Decimal = Val(dtselect.Compute("SUM(STNWT)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("STNPCS").Value = IIf(Stnpcs <> 0, Stnpcs, DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("STNWT").Value = IIf(StnWt <> 0, Format(StnWt, "0.000"), DBNull.Value)

        Stnpcs = Val(dtselect.Compute("SUM(PSTNPCS)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        StnWt = Val(dtselect.Compute("SUM(PSTNWT)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("PSTNPCS").Value = IIf(Stnpcs <> 0, Stnpcs, DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("PSTNWT").Value = IIf(StnWt <> 0, Format(StnWt, "0.000"), DBNull.Value)

        Stnpcs = Val(dtselect.Compute("SUM(DIAPCS)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        StnWt = Val(dtselect.Compute("SUM(DIAWT)", "CHECK=TRUE AND (RESULT=1 OR RESULT = 2)").ToString)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("DIAPCS").Value = IIf(Stnpcs <> 0, Stnpcs, DBNull.Value)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("DIAWT").Value = IIf(StnWt <> 0, Format(StnWt, "0.000"), DBNull.Value)



        gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.BackColor = Color.Aqua
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.SelectionBackColor = Color.Aqua
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.SelectionForeColor = Color.Red
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.ForeColor = Color.Red
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        If cnt > 0 Then btnTransfer.Enabled = True Else btnTransfer.Enabled = False

    End Sub

    Private Sub gridView_OWN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.Click

    End Sub

    'Private Sub gridView_OWN_CellStateChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellStateChangedEventArgs) Handles gridView_OWN.CellStateChanged
    '    If e.Cell.ColumnIndex = 0 Then
    '        computeTotal()
    '    End If
    'End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transit", gridView_OWN, BrightPosting.GExport.GExportType.Export)
    End Sub



    Private Sub cmbCostCentre_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmbCostCentre_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.SelectedIndexChanged
        If cmbCostCentre_MAN.Text <> "" Then
            CmbAcname.Items.Clear()
            Dim acname, Aacname As String
            acname = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =(SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "')")
            If acname <> "" Then CmbAcname.Items.Add(acname)
            Aacname = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =(SELECT ISNULL(AACCODE,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "')")
            If Aacname <> "" Then CmbAcname.Items.Add(Aacname) : CmbAcname.Enabled = True Else CmbAcname.Enabled = False
            If CmbAcname.Items.Count > 0 Then CmbAcname.SelectedIndex = 0
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txttranno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txttranno.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txttranno.Text <> "" Then
                strSql = "SELECT CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += " FROM " & cnAdminDb & "..TITEMTAG WHERE TRANINVNO='" & txttranno.Text & "'"
                CmbStockType.Text = objGPack.GetSqlValue(strSql, "STKTYPE", "TRADING").ToString
                CmbStockType.Enabled = False
            End If
        End If
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbSearchKey.Text = "HM_BILLNO" And txtSearch.Text <> "" Then
                strSql = "SELECT CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE HM_BILLNO='" & txtSearch.Text & "'"
                CmbStockType.Text = objGPack.GetSqlValue(strSql, "STKTYPE", "TRADING").ToString
                CmbStockType.Enabled = False
            End If
        End If
    End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        gridView_OWN.Select()
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN, 1)
        objSearch.ShowDialog()
    End Sub

End Class
Public Class frmTagTransit_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
End Class