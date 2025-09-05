Imports System.IO
Imports System.Data.OleDb
Public Class frmTransferVoucherGeneration
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtCatMast As DataTable
    Public objSoftKeys As New SoftKeys
    'GST
    Dim SGST As Double = Nothing
    Dim CGST As Double = Nothing
    Dim IGST As Double = Nothing
    Dim gstAmt As Double = Nothing
    Dim fgstAmt As Double = Nothing
    Dim StateId As Integer
    Dim SGSTPER As Decimal
    Dim CGSTPER As Decimal
    Dim IGSTPER As Decimal
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE_INTTRF", "")
    Dim GstRecAcc() As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim AUTOINTERNAL_VOUCHER As String = GetAdmindbSoftValue("AUTOBOOK_VOUCHER", "N")
    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String
    Dim AUTOBOOKVALUEG_PER As Decimal
    Dim AUTOBOOKVALUES_PER As Decimal
    Dim AUTOBOOKVALUEP_PER As Decimal
    Dim AUTOBOOKVALUED_PER As Decimal
    Dim AUTOBOOKVALUET_PER As Decimal
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)
    Dim STKTRAN_ContraPost As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_ACCOUNT_POST", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim STKTRAN_REPAIR_SEPPOST As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_REPAIR_SEPPOST", "N") = "Y", True, False)
    Dim OR_REP_NewCatCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT", "00018") & "'", , "00018")
    Dim OR_REP_NewCatCode_S As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_S", "00014") & "'", , "00014")
    Dim OR_REP_NewCatCode_P As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_P", "") & "'", , "")

    Public Sub New()
        InitializeComponent()
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub
    Private Sub frmTransferVoucherGeneration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If AUTOBOOKTRAN = False Then
            MsgBox("Enable AUTOBOOKTRAN control in softcontrol", MsgBoxStyle.Information)
            Me.Close()
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

        strSql = " SELECT * FROM " & cnAdminDb & "..CATEGORY "
        Dim daCat As New OleDbDataAdapter()
        daCat = New OleDbDataAdapter(strSql, cn)
        dtCatMast = New DataTable
        daCat.Fill(dtCatMast)

        If AUTOBOOKTRAN = True Then
            If AUTOBOOKVALUEARRY.Length < 6 Then MsgBox("Please Reset the value <AUTOBOOKVALUE> ex(N,0,0,0,0,0,0)") : Me.Close()
            AUTOBOOKVALUEENABLE = AUTOBOOKVALUEARRY(0).ToString
            AUTOBOOKVALUEG_PER = Val(AUTOBOOKVALUEARRY(1).ToString)
            AUTOBOOKVALUES_PER = Val(AUTOBOOKVALUEARRY(2).ToString)
            AUTOBOOKVALUEP_PER = Val(AUTOBOOKVALUEARRY(3).ToString)
            AUTOBOOKVALUED_PER = Val(AUTOBOOKVALUEARRY(4).ToString)
            AUTOBOOKVALUET_PER = Val(AUTOBOOKVALUEARRY(5).ToString)
        End If

        btnNew_Click(sender, e)
    End Sub

    Private Sub frmTransferVoucherGeneration_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtTransferRefNo.Clear()
        dtpDate.Value = GetServerDate(Nothing)
        dtpDate.Select()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtTransferRefNo.Text.ToString.Trim = "" Then
            MsgBox("Transfer Refno Should Not Be Empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim issrec As String = ""
        strSql = "SELECT DISTINCT ISSREC FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "' "
        issrec = objGPack.GetSqlValue(strSql, "ISSREC", "")
        If issrec.ToString = "" Then
            MsgBox("Invalid Refno", MsgBoxStyle.Information)
            Exit Sub
        End If
        If issrec.ToString <> "I" And issrec.ToString <> "R" Then
            MsgBox("Invalid Voucher Type", MsgBoxStyle.Information)
            Exit Sub
        End If
        If issrec.ToString = "I" Then
            strSql = "SELECT count(*)CNT FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='IIN' AND ISNULL(CANCEL,'')<>'Y' AND REFNO='" & txtTransferRefNo.Text & "' "
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                MsgBox("Issue Voucher already Generated", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If issrec.ToString = "R" Then
            strSql = "SELECT count(*)CNT FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE='RIN' AND ISNULL(CANCEL,'')<>'Y' AND REFNO='" & txtTransferRefNo.Text & "' "
            If objGPack.GetSqlValue(strSql, "CNT", 0) > 0 Then
                MsgBox("RECEIPT Voucher already Generated", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        Try
            Dim NEWBILLNO As Integer
            Dim billcontrolid As String = ""
            Dim RefNo As String = ""
            Dim TransitToid As String = ""
            Dim Transitfromid As String = ""
            Dim TransistNo As Integer
            Dim drtrfref As DataRow = Nothing
            strSql = " SELECT TOP 1 TOID,FROMID FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE "
            strSql += vbCrLf + " TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "' "
            drtrfref = GetSqlRow(strSql, cn)
            If drtrfref Is Nothing Then
                MsgBox("Invalid Transfer Refno", MsgBoxStyle.Information)
                Exit Sub
            End If
            If Not drtrfref Is Nothing Then
                Transitfromid = drtrfref("FROMID").ToString
                TransitToid = drtrfref("TOID").ToString
            End If
            Dim Toaccountid As String = ""
            If AUTOBOOKTRAN Then Toaccountid = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & TransitToid.ToString & "'", "ACCODE", "")

            If Toaccountid.ToString = "" Then MsgBox("Update Account code in CostCentre", MsgBoxStyle.Information) : Exit Sub

            tran = Nothing
            tran = cn.BeginTransaction
            BillDate = dtpDate.Value ''GetEntryDate(GetServerDate(tran), tran)
            Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)

            ''tranno
            RefNo = txtTransferRefNo.Text
            If issrec = "I" Then
                strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()


                billcontrolid = "GEN-SM-INTISS"
                strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                If UCase(objGPack.GetSqlValue(strSql,,, tran)) <> "Y" Then
                    billcontrolid = "GEN-STKREFNO"
                End If

                strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
                If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "N", tran)) = "Y" Then
                    billcontrolid = "GEN-SM-ISS"
                End If
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                NEWBILLNO = Val(objGPack.GetSqlValue(strSql,,, tran)) + 1

GenerateNewBillNo:

                If AUTOBOOKTRAN Then
                    strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
                End If
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo GenerateNewBillNo
                End If

                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
                    GoTo GenerateNewBillNo
                End If


                Dim billcontrolidX As String = "GEN-TRANSISTNO"
                strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
                If UCase(BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran)) = "Y" Then
                    strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
                    TransistNo = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , , tran))
GenerateNewBillNoX:
                    strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND BAGNO = '" & TransistNo.ToString & "'"
                    If BrighttechPack.GlobalMethods.GetSqlValue(cn, strSql, , "-1", tran) <> "-1" Then
                        TransistNo = TransistNo + 1
                        GoTo GenerateNewBillNoX
                    End If
                    strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TransistNo.ToString & "'"
                    strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    If cmd.ExecuteNonQuery() = 0 Then
                        GoTo GenerateNewBillNoX
                    End If
                End If
                CreateInternalTransfernew(TransitToid.ToString, RefNo, BillDate, NEWBILLNO, TransistNo, Transitfromid, Toaccountid)
            Else
                strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                billcontrolid = "GEN-SM-INTREC"
                strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If UCase(objGPack.GetSqlValue(strSql,,, tran)) <> "Y" Then
                    billcontrolid = "GEN-STKREFNO"
                End If
                strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
                If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "N", tran)) = "Y" Then
                    billcontrolid = "GEN-SM-REC"
                End If
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNorec:
                'RefNo = cnCostId & NEWBILLNO.ToString
                strSql = "SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo GenerateNewBillNorec
                End If

                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GenerateNewBillNorec
                End If

                CreateInternalTransferReceipt(TransitToid, NEWBILLNO, BillDate, Toaccountid)
            End If
            tran.Commit()
            tran = Nothing
            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate
            btnNew_Click(Me, New EventArgs)
            MsgBox("Transfer " & IIf(issrec = "I", "Issue", "Receipt") & " Voucher Generated." & vbCrLf & " Reference no is: " & RefNo)

            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValuefromDt(dtSoftKeys, "PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format.ToString = "M1" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA4N("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M2" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M3" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M4" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    Dim write As IO.StreamWriter
                    Dim Type As String = "SMI"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":" & Type)
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":" & Type & ";" &
                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                        LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" &
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

    Private Sub CreateInternalTransfernew(ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long, ByVal Transistno As Integer, Optional ByVal Fromcostid As String = Nothing, Optional ByVal Tocostacid As String = "")
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

        Dim _contraaccode As String = ""
        If STKTRAN_ContraPost Then
            _contraaccode = GetSqlValue("SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & cnCostId.ToString & "'", cn, tran)
            If _contraaccode.ToString = "" Then
                _contraaccode = "STKTRAN"
            End If
        Else
            _contraaccode = "STKTRAN"
        End If

        strSql = vbCrLf + " SELECT AX.*" ',IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
        strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If

        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
        End If
        If STKTRAN_REPAIR_SEPPOST Then
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
        'strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,0 RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
        End If
        If STKTRAN_REPAIR_SEPPOST Then
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If

        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID,0 RATE"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
            strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
            strSql += vbCrLf + " )/100)))))) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,MT.CATCODE CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = MT.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = MT.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.SALEMODE,T.STKTYPE"
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID,0 RATE"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
            strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
            strSql += vbCrLf + " )/100)))))) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,MT.CATCODE CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = MT.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = MT.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.SALEMODE,T.STKTYPE"
        End If

        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " , 0 RATE"
            strSql += vbCrLf + " , SUM(TRFVALUE) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
            'strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
            strSql += vbCrLf + " ,0 RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            ''excess wt
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,0 LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " , 0 RATE"
            strSql += vbCrLf + " , 0 AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND ISNULL(T.OREXCESSWT,0)<>0 "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,0 LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " ,0 RATE, 0 AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " AND ISNULL(T.OREXCESSWT,0)<>0 "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
        End If

        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,0 AS ITEMID ,0 AS SUBITEMID"
        strSql += vbCrLf + " ,0 AS RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID ,CA.PURITYID,T.STKTYPE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0)AS ITEMID ,0 AS SUBITEMID"
        strSql += vbCrLf + " ,0 AS RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN ('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE"
        strSql += vbCrLf + " ) AX"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT IDENTITY(INT,1,1)AS KEYNO,CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT,SUM(PUREWT) AS PUREWT"
        strSql += vbCrLf + " ,ITEMID,'G' GRSNET,0 RATE, SUM(AMOUNT) AS AMOUNT,COSTID,COMPANYID,CATCODE,CATCODE OCATCODE,'X' FLAG"
        strSql += vbCrLf + " ,ACCODE,METALID,53 ORDSTATE_ID,STKTYPE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " GROUP BY CATCODE,ITEMID,COMPANYID,METALID,ACCODE,COSTID,STKTYPE"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_NEW'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT AXX.*"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM ("

        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(TG.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID) ELSE TIM.CATCODE END ELSE  TIM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE"
        End If
        strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,MT.CATCODE AS CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAGMETAL AS MT ON TG.SNO = MT.TAGSNO AND MT.SNO=ST.TAGMSNO "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,(SELECT TOP 1  CATCODE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) AS CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') NOT IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
        End If
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,CASE WHEN TIM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN TIM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ST.STONEUNIT AS STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')"
        strSql += vbCrLf + " ) AS AXX"

        strSql += vbCrLf + " UPDATE SV  SET SV.ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW AS T ON T.COMPANYID = SV.TCOMPANYID"
        strSql += vbCrLf + " AND T.CATCODE = SV.CATCODE "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,STNITEMID,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
        strSql += vbCrLf + " ,TCATCODE AS CATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO,TMETALID AS STONEMODE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,TCATCODE,TMETALID,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"

        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()



        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", cn, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", cn, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", cn, tran)
        Dim DtTaxtran As New DataTable
        DtTaxtran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "TAXTRAN", cn, tran)

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
        Dim RoGstTax As DataRow = Nothing

        For Each Ro As DataRow In DtTag.Rows
            GSTCALC(Val(Ro.Item("AMOUNT").ToString), Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                RoGstTax = DtTaxtran.NewRow
                RoGstTax.Item("SNO") = GetNewSnoNew(TranSnoType.TAXTRANCODE, cn, tran)  ''SNO
                RoGstTax.Item("ISSSNO") = Ro.Item("SNO")
                RoGstTax.Item("TRANNO") = Tranno
                RoGstTax.Item("TRANDATE") = BillDate
                RoGstTax.Item("TRANTYPE") = "IIN"
                RoGstTax.Item("BATCHNO") = Batchno
                RoGstTax.Item("TAXID") = "IG"
                RoGstTax.Item("AMOUNT") = Ro.Item("AMOUNT")
                If Val(Ro.Item("AMOUNT").ToString) = 0 Then
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

        'Batchno = GetNewBatchno(cnCostId, BillDate, tran)
        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate(tran)
            'Ro.Item("UPTIME") = Date.Now.ToLongTimeString
            Ro.Item("UPTIME") = GetServerTime(tran)
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = RefNo
            Ro.Item("BAGNO") = Transistno.ToString
            Ro.Item("REFDATE") = RefDate
            Ro.Item("REMARK1") = ""
            Ro.Item("REMARK2") = "Generated Internal Voucher"
            Ro.Item("STNAMT") = Val(DtIssStone.Compute("SUM(STNAMT)", "ISSSNO='" & Ro.Item("SNO").ToString & "'").ToString)
            If GST And StateId <> CompanyStateId Then
                GSTCALC(Val(Ro.Item("AMOUNT").ToString), Ro.Item("CATCODE").ToString)
            End If
            'If GST And StateId <> CompanyStateId Then GSTCALC(ISNULL(Val(Ro.Item("AMOUNT")), , Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                If Val(Ro.Item("AMOUNT").ToString) = 0 Then
                    IGST = 0
                End If
                Ro.Item("TAX") = IGST
            End If

            If AUTOBOOKVALUEENABLE = "Y" Then
                Dim mamount As Decimal = Val(Ro.Item("AMOUNT").ToString)
                If mamount <> 0 Then
                    If AUTOINTERNAL_VOUCHER = "Y" Then
                        Dim Roacct As DataRow = Nothing
                        If STKTRAN_ContraPost Then
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                                .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                If GST And StateId <> CompanyStateId And IGST > 0 Then
                                    .Item("AMOUNT") = Val(mamount + IGST)
                                Else
                                    .Item("AMOUNT") = mamount
                                End If
                                .Item("REMARK1") = ""
                                .Item("REMARK2") = "Generated Internal Voucher"
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN")
                                .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                .Item("REMARK1") = ""
                                .Item("REMARK2") = "Generated Internal Voucher"
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            'GST                        
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                Roacct = DtAcctran.NewRow
                                With Roacct
                                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                    .Item("CONTRA") = Ro.Item("ACCODE")
                                    .Item("PCS") = 0
                                    .Item("GRSWT") = 0
                                    .Item("NETWT") = 0
                                    .Item("AMOUNT") = IGST
                                    .Item("REMARK1") = ""
                                    .Item("REMARK2") = "Generated Internal Voucher"
                                End With
                                DtAcctran.Rows.Add(Roacct)
                                DtAcctran.AcceptChanges()
                            End If
                        Else
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                                .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                If GST And StateId <> CompanyStateId And IGST > 0 Then
                                    .Item("AMOUNT") = Val(mamount + IGST)
                                Else
                                    .Item("AMOUNT") = mamount
                                End If
                                .Item("REMARK1") = ""
                                .Item("REMARK2") = "Generated Internal Voucher"
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN"
                                .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                .Item("REMARK1") = ""
                                .Item("REMARK2") = "Generated Internal Voucher"
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            'GST                        
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                Roacct = DtAcctran.NewRow
                                With Roacct
                                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                    .Item("CONTRA") = Ro.Item("ACCODE")
                                    .Item("PCS") = 0
                                    .Item("GRSWT") = 0
                                    .Item("NETWT") = 0
                                    .Item("AMOUNT") = IGST
                                    .Item("REMARK1") = ""
                                    .Item("REMARK2") = "Generated Internal Voucher"
                                End With
                                DtAcctran.Rows.Add(Roacct)
                                DtAcctran.AcceptChanges()
                            End If
                        End If

                    End If
                End If
                'Ro.Item("AMOUNT") = mamount

            End If

        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("STONEMODE") = ""
        Next
        DtIssStone.AcceptChanges()

        InsertData(SyncMode.Transaction, DtIssue, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, cn, tran, cnCostId)
        If AUTOINTERNAL_VOUCHER = "Y" Then InsertData(SyncMode.Transaction, DtAcctran, cn, tran, cnCostId) : InsertData(SyncMode.Transaction, DtTaxtran, cn, tran, cnCostId)
    End Sub

    Private Function CreateInternalTransferReceipt(ByVal ToCostId As String, ByVal tranno As Long, Optional ByVal trandate As Date = Nothing, Optional ByVal tranAcode As String = Nothing)
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

        If BillDate = Nothing Then BillDate = GetServerDate()
        If trandate = Nothing Then trandate = BillDate

        Dim _contraaccode As String = ""
        If STKTRAN_ContraPost Then
            _contraaccode = GetSqlValue("SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & cnCostId.ToString & "'", cn, tran)
            If _contraaccode.ToString = "" Then
                _contraaccode = "STKTRAN"
            End If
        Else
            _contraaccode = "STKTRAN"
        End If

        If tranAcode Is Nothing Then tranAcode = ""
        If StateId = 0 Then StateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = (SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & ToCostId.ToString & "')", "", "", tran).ToString)

        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'RIN' AS TRANTYPE,YY.REFNO "
        strSql += vbCrLf + " ,SUM(YY.PCS)PCS,SUM(YY.GRSWT)GRSWT,SUM(YY.NETWT)NETWT,SUM(YY.LESSWT)LESSWT,SUM(PUREWT)PUREWT"
        strSql += vbCrLf + " ,yy.ITEMID,yy.SUBITEMID"
        strSql += vbCrLf + " ,'G' GRSNET,0 RATE,SUM(YY.AMOUNT) AMOUNT,'" & cnCostId & "' COSTID,YY.COMPANYID,YY.PURITY,YY.CATCODE CATCODE"
        strSql += vbCrLf + " ,YY.ACCODE ACCODE"
        strSql += vbCrLf + " ,YY.METALID,STKTYPE"
        ' strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS FROM( "

        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
        'strSql += vbCrLf + " ,'G' GRSNET,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE,SUM(TRFVALUE) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
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
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
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
        strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
        'strSql += vbCrLf + " ,'G' GRSNET,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE,ISNULL(SUM(TRFVALUE),0) AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
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
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
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
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.STKTYPE"
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.STKTYPE"
        End If


        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            ''OREXCESSWT
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            strSql += vbCrLf + " union all "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'RIN' AS TRANTYPE,T.TRANINVNO AS REFNO "
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
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY T.TRANINVNO,IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
        End If

        strSql += vbCrLf + " union all "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'RIN' AS TRANTYPE, T.REFNO AS REFNO"
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
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "')  "
        strSql += vbCrLf + "  GROUP BY T.REFNO,IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE "
        strSql += vbCrLf + " union all "
        strSql += vbCrLf + "SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'RIN' AS TRANTYPE, T.REFNO AS REFNO"
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
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN ('W','N') AND T.SNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
        strSql += vbCrLf + "  GROUP BY T.REFNO,IM.CATCODE,T.ITEMID,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE "

        strSql += vbCrLf + " ) YY GROUP BY YY.REFNO "
        strSql += vbCrLf + " ,YY.COSTID,YY.COMPANYID,YY.PURITY,YY.CATCODE,YY.ITEMID,YY.SUBITEMID"
        strSql += vbCrLf + " ,YY.ACCODE,YY.METALID,STKTYPE"

        cmd = New OleDbCommand(strSql, cn, tran)
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
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'RIN'TRANTYPE"
        strSql += vbCrLf + " ,SUM(STNPCS) STNPCS,SUM(STNWT)STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,SUM(STNAMT)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL(YY.STNITEMID,0) STNITEMID,0 STNSUBITEMID,YY.STONEUNIT AS STONEUNIT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,YY.COMPANYID"
        strSql += vbCrLf + " ,YY.TRANINVNO ,YY.CATCODE,YY.TCATCODE,YY.TCOMPANYID ,YY.TPURITY ,YY.TMETALID "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM (SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'RIN'TRANTYPE"
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
        strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'RIN'TRANTYPE"
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
            strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'RIN'TRANTYPE"
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
            strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') NOT IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
        End If

        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'RIN' TRANTYPE"
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
            strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If

        strSql += vbCrLf + " UNION ALL SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'RIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,0 AS STNSUBITEMID"
        strSql += vbCrLf + " ,ST.STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TG.REFNO TRANINVNO,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM " & cnStockDb & "..TRANSIT_AUDIT WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' and refno='" & txtTransferRefNo.Text & "' AND COSTID='" & cnCostId & "') "
        strSql += vbCrLf + " ) YY "
        strSql += vbCrLf + " GROUP BY YY.STNITEMID,YY.STNRATE,YY.STONEUNIT "
        strSql += vbCrLf + " ,YY.COMPANYID,YY.TRANINVNO ,YY.CATCODE ,YY.TCATCODE, YY.TCOMPANYID,YY.TPURITY ,YY.TMETALID "
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 :
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

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
        cmd = New OleDbCommand(strSql, cn, tran)
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
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 :
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        Dim DtReceipt As New DataTable
        Dim DtReceiptStone As New DataTable

        DtReceipt = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPT", cn, tran)
        DtReceiptStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "RECEIPTSTONE", cn, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", cn, tran)
        Dim DtTaxtran As New DataTable
        DtTaxtran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "TAXTRAN", cn, tran)
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
        If BillDate = Nothing Then BillDate = GetServerDate(tran)
        If trandate = Nothing Then trandate = BillDate
        ''Batchno = GetNewBatchnoNew(cnCostId, trandate, cn, tran)

        Dim RoGstTax As DataRow = Nothing
        For Each Ro As DataRow In DtTag.Rows
            GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                RoGstTax = DtTaxtran.NewRow
                RoGstTax.Item("SNO") = GetNewSnoNew(TranSnoType.TAXTRANCODE, cn, tran)  ''SNO
                RoGstTax.Item("ISSSNO") = Ro.Item("SNO")
                RoGstTax.Item("TRANNO") = tranno
                RoGstTax.Item("TRANDATE") = BillDate
                RoGstTax.Item("TRANTYPE") = "RIN"
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
            Ro.Item("UPDATED") = GetServerDate(tran)
            Ro.Item("UPTIME") = GetServerTime(tran)
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = txtTransferRefNo.Text
            Ro.Item("REFDATE") = dtpDate.Value.ToString("yyyy-MM-dd")
            Ro.Item("REMARK1") = ""
            Ro.Item("REMARK2") = "Generated Internal Voucher"
            Dim mxrate As Decimal = 0
            mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, tran))
            Ro.Item("BOARDRATE") = mxrate
            If GST And StateId <> CompanyStateId Then GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                If Ro.Item("AMOUNT") = "0" Then
                    IGST = 0
                End If
                Ro.Item("TAX") = IGST
            End If
            If AUTOBOOKVALUEENABLE = "Y" Then
                Dim mamount As Decimal = Val(Ro.Item("AMOUNT").ToString)
                mamount = CalcRoundoffAmt(mamount, "H")
                If mamount <> 0 Then
                    If AUTOINTERNAL_VOUCHER = "Y" Then
                        'GST
                        If GST And StateId <> CompanyStateId Then GSTCALC(Val(Ro.Item("AMOUNT")), Ro.Item("CATCODE").ToString)
                        Dim Roacct As DataRow = Nothing
                        Roacct = DtAcctran.NewRow
                        With Roacct
                            .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                            .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                            .Item("BATCHNO") = Batchno
                            .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                            .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TR"
                            .Item("CONTRA") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                            .Item("NETWT") = Ro.Item("NETWT")
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                .Item("AMOUNT") = Val(mamount + IGST)
                            Else
                                .Item("AMOUNT") = mamount
                            End If
                            .Item("REMARK2") = "Generated Internal Voucher"
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
                            .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                            .Item("BATCHNO") = Batchno
                            .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                            .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = Ro.Item("COSTID") : .Item("FROMFLAG") = "A"
                            .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN")
                            .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TR"
                            .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                            .Item("REMARK2") = "Generated Internal Voucher"
                        End With
                        DtAcctran.Rows.Add(Roacct)
                        'GST                        
                        If GST And StateId <> CompanyStateId And IGST > 0 Then
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, cn, tran)
                                .Item("BATCHNO") = Batchno
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TR"
                                .Item("CONTRA") = Ro.Item("ACCODE")
                                .Item("PCS") = 0
                                .Item("GRSWT") = 0
                                .Item("NETWT") = 0
                                .Item("AMOUNT") = IGST
                                .Item("REMARK2") = "Generated Internal Voucher"
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
        InsertData(SyncMode.Transaction, DtReceipt, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtReceiptStone, cn, tran, cnCostId)
        If AUTOINTERNAL_VOUCHER = "Y" Then
            InsertData(SyncMode.Transaction, DtAcctran, cn, tran, cnCostId)
            InsertData(SyncMode.Transaction, DtTaxtran, cn, tran, cnCostId)
        End If


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


End Class