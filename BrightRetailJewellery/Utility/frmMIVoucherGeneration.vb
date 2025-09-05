Imports System.IO
Imports System.Data.OleDb
Public Class frmMIVoucherGeneration
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim BillDate As Date
    Dim Batchno As String = ""


    Public Sub New()
        InitializeComponent()
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub
    Private Sub frmTransferVoucherGeneration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If funcCheckCostCentreStatusFalse() Then
            cmbCostCentreName.Enabled = False
            cmbCostCentreName.Enabled = False
        End If
        If cmbCostCentreName.Enabled = True Then
            cmbCostCentreName.Items.Clear()
            cmbCostCentreName.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentreName, False, False)
        End If
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        btnNew_Click(sender, e)
    End Sub

    Private Sub frmTransferVoucherGeneration_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        BillDate = GetEntryDate(GetServerDate)
        dtpFrom.Value = GetServerDate(Nothing)
        dtpTo.Value = GetServerDate(Nothing)
        cmbMetal.Text = "ALL"
        cmbCostCentreName.Text = "ALL"
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False
        strSql = vbCrLf + " SELECT C.METALID,"
        strSql += vbCrLf + " 'MISCELLANIOUS' AS TYPE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
        strSql += vbCrLf + " ,A.PCS,A.GRSWT,0 AS DUSTWT,NETWT,ISNULL(A.PURITY,100) TOUCH"
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(ISNULL(A.GRSWT,0)*(ISNULL(A.PURITY,100)/100),3)) PUREWT,NETWT AS CNETWT"
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
        strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,A.NETWT ONETWT"
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
        '
        strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
        strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
        '
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME,ISNULL(A.ACCODE,'')ACCODE "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
        strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'),'')"
        strSql += vbCrLf + " AND"
        strSql += vbCrLf + " (A.TRANTYPE = '' OR A.TRANTYPE = 'MI')"
        If cmbCostCentreName.Enabled Then
            If UCase(cmbCostCentreName.Text) <> "ALL" And cmbCostCentreName.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentreName.Text & "')"
        End If
        strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
        strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtTemp As New DataTable
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            Dim NEWBILLNO As Integer
            Dim billcontrolid As String = ""
            Dim _InsAccode As String = ""
            Dim _dtinsAccodes As New DataTable
            Dim _insAccodesCnt As Integer = 0
            _dtinsAccodes = dtTemp.DefaultView.ToTable(True, "ACCODE")
            _insAccodesCnt = _dtinsAccodes.Select("ACCODE<>''").Length
            If _insAccodesCnt = 0 Then MsgBox("MiscIssue account Code Is empty", MsgBoxStyle.Information) : btnSave.Enabled = True : Exit Sub
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                For Each drr As DataRow In _dtinsAccodes.Rows
                    If drr.Item("ACCODE").ToString = "" Then Continue For
                    _InsAccode = drr.Item("ACCODE").ToString
                    BillDate = GetEntryDate(GetServerDate(tran), tran)
                    Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)

                    strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                    strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()


                    billcontrolid = "GEN-SM-ISS"
                    strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                    If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                    NEWBILLNO = Val(objGPack.GetSqlValue(strSql,,, tran)) + 1
GenerateNewBillNo:

                    strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIS' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
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

                    Dim BagNo As String = Nothing
GENBAGNO:
                    BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("BAGNO", True, tran)

                    'BagNo = cnCostId & 
                    ''check
                    strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
                    strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                    strSql += vbCrLf + "UNION ALL SELECT 'CHECK' FROM " & cnStockDb & "..ISSUE"
                    strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                    If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                        GoTo GENBAGNO
                    End If

                    Dim _BATCHNO As String = ""
                    Dim _Sno As String = ""
                    Dim TDbame As String = ""
                    CreateMiVouchernew(NEWBILLNO, Batchno, BagNo, _InsAccode)
                    For Each row As DataRow In dtTemp.Rows
                        If _InsAccode.ToString <> row.Item("ACCODE").ToString Then Continue For
                        _BATCHNO = "'" & row.Item("BATCHNO").ToString & "'"
                        _Sno = "'" & row.Item("SNO").ToString & "'"
                        TDbame = IIf(row.Item("TDBNAME").ToString <> "", row.Item("TDBNAME").ToString, cnStockDb.ToString)
                        strSql = " UPDATE " & TDbame & "..RECEIPT"
                        strSql += vbCrLf + "  SET MELT_RETAG='M', BAGNO = '" & BagNo & "'"
                        strSql += vbCrLf + "  WHERE BATCHNO =" & _BATCHNO & " and sno = " & _Sno & " AND ACCODE ='" & _InsAccode & "'"
                        strSql += vbCrLf + " UPDATE " & TDbame & "..ISSUE"
                        strSql += vbCrLf + "  SET MELT_RETAG='M', BAGNO = '" & BagNo & "'"
                        strSql += vbCrLf + "  WHERE BATCHNO =" & _BATCHNO & " AND SNO = " & _Sno & " AND ACCODE ='" & _InsAccode & "'"
                        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        If CENTR_DB_BR Then
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Else
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        End If
                    Next
                Next
                tran.Commit()
                tran = Nothing
                MsgBox(" Generated Sucessfully...", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpFrom.Select()
        End If
        btnSave.Enabled = True
    End Sub

    Private Sub CreateMiVouchernew(ByVal tranno As Integer, ByVal batchno As String, ByVal bagno As String, ByVal _ACCODE As String)
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " SELECT METALID,TRANDATE,TRANNO,CATCODE,COSTID,ACCODE,COMPANYID"
        strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
        strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),0)RATE,CONVERT(DECIMAL(15,2),0) AMOUNT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT A.METALID,A.TRANDATE,A.TRANNO"

        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(A.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =A.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =A.ITEMTYPEID) ELSE A.CATCODE END ELSE  A.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,A.CATCODE"
        End If
        strSql += vbCrLf + " ,'IIS' AS TRANTYPE "
        strSql += vbCrLf + " ,A.PCS PCS,A.GRSWT GRSWT,0 AS DUSTWT,A.NETWT NETWT,A.LESSWT LESSWT,'" & cnCostId & "' COSTID,A.ACCODE,A.COMPANYID"
        strSql += vbCrLf + " FROM  " & cnStockDb & " ..ISSUE A"
        strSql += vbCrLf + " WHERE  TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND A.TRANTYPE <> '' AND ISNULL(A.CANCEL,'') = ''"
        strSql += vbCrLf + " AND(A.TRANTYPE = '' OR A.TRANTYPE = 'MI')"
        strSql += vbCrLf + " AND ISNULL(A.BAGNO,'') =''"
        strSql += vbCrLf + " AND ISNULL(A.MELT_RETAG,'') ='' AND A.ACCODE='" & _ACCODE.ToString & "'"
        If cmbCostCentreName.Enabled Then
            If UCase(cmbCostCentreName.Text) <> "ALL" And cmbCostCentreName.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentreName.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND A.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'),'')"
        If MetalBasedStone Then '' added for vbj on 04-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =A.SNO) "
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT A.METALID,B.TRANDATE,B.TRANNO,A.CATCODE CATCODE"
            strSql += vbCrLf + " ,'IIS' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,A.GRSWT GRSWT,0 AS DUSTWT,CASE WHEN ISNULL(A.NETWT,0)<>0 THEN A.NETWT ELSE A.GRSWT END NETWT,0 LESSWT "
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,B.ACCODE,A.COMPANYID"
            strSql += vbCrLf + " FROM  " & cnStockDb & " ..ISSMETAL A"
            strSql += vbCrLf + " INNER JOIN  " & cnStockDb & " ..ISSUE B ON B.SNO=A.ISSSNO "
            strSql += vbCrLf + " WHERE  B.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND A.TRANTYPE <> '' AND ISNULL(B.CANCEL,'') = ''"
            strSql += vbCrLf + " AND(A.TRANTYPE = '' OR A.TRANTYPE = 'MI')"
            strSql += vbCrLf + " AND ISNULL(B.BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(B.MELT_RETAG,'') ='' AND B.ACCODE='" & _ACCODE.ToString & "'"
            If cmbCostCentreName.Enabled Then
                If UCase(cmbCostCentreName.Text) <> "ALL" And cmbCostCentreName.Text <> "" Then strSql += vbCrLf + " AND B.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentreName.Text & "')"
            End If
            If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND A.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'),'')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =B.SNO) "
        End If
        strSql += vbCrLf + " ) AX"
        strSql += vbCrLf + " GROUP BY METALID,TRANDATE,CATCODE,COSTID,ACCODE,COMPANYID,TRANNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT IDENTITY(INT,1,1)AS KEYNO,CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIS' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
        strSql += vbCrLf + " ,0 ITEMID,'G' GRSNET,0 RATE, SUM(AMOUNT) AS AMOUNT,COSTID,COMPANYID,CATCODE,CATCODE OCATCODE,'X' FLAG"
        strSql += vbCrLf + " ,ACCODE,METALID,53 ORDSTATE_ID,'' STKTYPE,TRANDATE,TRANDATE REFDATE,TRANNO,TRANNO REFNO "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW FROM TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS"
        strSql += vbCrLf + " GROUP BY CATCODE,COMPANYID,METALID,ACCODE,COSTID,TRANDATE,TRANNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "MITRANTRANS_NEW'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO,'IIS'TRANTYPE,METALID,TRANDATE,TRANNO,TCATCODE,CATCODE,COSTID,STONEUNIT,ACCODE,COMPANYID "
        strSql += vbCrLf + " ,SUM(STNPCS) STNPCS,SUM(STNWT)STNWT,CONVERT(DECIMAL(15,2),0)STNRATE,CONVERT(DECIMAL(15,2),0) STNAMT "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT IM.METALID,A.TRANDATE,I.TRANNO,A.STNPCS STNPCS,A.STNWT STNWT,'" & cnCostId & "' COSTID,A.STONEUNIT,I.ACCODE "
        strSql += vbCrLf + " ,I.COMPANYID"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(I.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =I.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =I.ITEMTYPEID) ELSE I.CATCODE END ELSE I.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,I.CATCODE AS CATCODE"
        End If
        strSql += vbCrLf + " ,A.CATCODE AS TCATCODE"
        strSql += vbCrLf + " FROM  " & cnStockDb & " ..ISSSTONE A "
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & " ..ISSUE I ON  I.SNO=A.ISSSNO "
        strSql += vbCrLf + " INNER JOIN  " & cnAdminDb & " ..ITEMMAST IM ON A.STNITEMID= IM.ITEMID"
        strSql += vbCrLf + " WHERE  I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.TRANTYPE <> '' AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND(I.TRANTYPE = '' OR I.TRANTYPE = 'MI')"
        strSql += vbCrLf + " AND ISNULL(I.BAGNO,'') =''"
        strSql += vbCrLf + " AND ISNULL(I.MELT_RETAG,'') ='' AND I.ACCODE='" & _ACCODE.ToString & "'"
        If cmbCostCentreName.Enabled Then
            If UCase(cmbCostCentreName.Text) <> "ALL" And cmbCostCentreName.Text <> "" Then strSql += vbCrLf + " AND I.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentreName.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND IM.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'),'')"

        If MetalBasedStone Then '' added for vbj on 04-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =I.SNO) "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT IM.METALID,A.TRANDATE,I.TRANNO,A.STNPCS STNPCS,A.STNWT STNWT,'" & cnCostId & "' COSTID,A.STONEUNIT,I.ACCODE"
            strSql += vbCrLf + " ,I.COMPANYID"
            strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =I.SNO) AS CATCODE"
            strSql += vbCrLf + " ,A.CATCODE AS TCATCODE "
            strSql += vbCrLf + " FROM  " & cnStockDb & " ..ISSSTONE A "
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & " ..ISSUE I ON  I.SNO=A.ISSSNO "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = A.STNITEMID"
            strSql += vbCrLf + " WHERE  I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.TRANTYPE <> '' AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND(I.TRANTYPE = '' OR I.TRANTYPE = 'MI')"
            strSql += vbCrLf + " AND ISNULL(I.BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(I.MELT_RETAG,'') ='' AND I.ACCODE='" & _ACCODE.ToString & "'"
            If cmbCostCentreName.Enabled Then
                If UCase(cmbCostCentreName.Text) <> "ALL" And cmbCostCentreName.Text <> "" Then strSql += vbCrLf + " AND I.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentreName.Text & "')"
            End If
            If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + " AND IM.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'),'')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =I.SNO) "
        End If
        strSql += vbCrLf + " ) AS AXX"
        strSql += vbCrLf + " GROUP BY METALID,TRANDATE,TCATCODE,CATCODE,COSTID,STONEUNIT,ACCODE,COMPANYID,TRANNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " UPDATE SV  SET SV.ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW AS T ON T.COMPANYID = SV.COMPANYID"
        strSql += vbCrLf + " AND T.CATCODE = SV.CATCODE AND T.ACCODE=SV.ACCODE AND T.TRANDATE=SV.TRANDATE AND T.TRANNO=SV.TRANNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
        strSql += vbCrLf + " ,TCATCODE AS CATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO,METALID AS STONEMODE,TRANDATE,TRANNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,TCATCODE,METALID,COSTID,COMPANYID,STONEUNIT,TRANDATE,TRANNO"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE SET SNO = KEYNO"

        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "MITRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "MITRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

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

        'Batchno = GetNewBatchno(cnCostId, BillDate, tran)
        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = tranno
            If ChkDatePosting.Checked Then
                Ro.Item("TRANDATE") = BillDate
            End If
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate(tran)
            'Ro.Item("UPTIME") = Date.Now.ToLongTimeString
            Ro.Item("UPTIME") = GetServerTime(tran)
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = batchno
            Ro.Item("BAGNO") = bagno.ToString
            Ro.Item("REMARK1") = ""
            Ro.Item("REMARK2") = "Generated MI Voucher "
            Ro.Item("STNAMT") = 0
            Ro.Item("TAX") = 0
        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = tranno
            If ChkDatePosting.Checked Then
                Ro.Item("TRANDATE") = BillDate
            End If
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = batchno
            Ro.Item("STONEMODE") = ""
        Next
        DtIssStone.AcceptChanges()

        InsertData(SyncMode.Transaction, DtIssue, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, cn, tran, cnCostId)
    End Sub

End Class