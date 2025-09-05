Imports System.Data.OleDb
Public Class frmBillWiseTransaction
    Dim strItemFilter As String = Nothing
    Dim dtCheck As New DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String
    Dim BILLTRAN_MARGIN As Boolean = IIf(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='BILLTRAN_MARGIN'") = "Y", True, False)
    Dim BILLTRAN_CATTYPE As Boolean = IIf(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RPT_BILLWISETRAN_ITEM'") = "I", True, False)
    Dim MINUSDISCOUNT_BILLTRAN As Boolean = IIf(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='MINUSDISCOUNT_BILLTRAN'") = "Y", True, False)
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpDate.Value = GetServerDate()
        txtNodeId.Text = ""
        If BILLTRAN_MARGIN Then
            ChkBillTotal.Visible = False
            ChkSummaryView.Visible = False
            ChkBillTotal.Checked = False
            ChkSummaryView.Checked = False
        End If
        gridBillWiseTransaction.DataSource = Nothing
        gridHeader.DataSource = Nothing
        Prop_Gets()
        ChkSummaryView_CheckedChanged(Me, New EventArgs)
        dtpDate.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridBillWiseTransaction.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridBillWiseTransaction, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridBillWiseTransaction.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridBillWiseTransaction, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpDate.Value) Then dtpDate.Focus() : Exit Sub
        Try
            Dim dtFinal As New DataTable
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            btnView_Search.Enabled = False
            gridHeader.DataSource = Nothing
            lblTitle.Visible = False
            gridBillWiseTransaction.DataSource = Nothing
            ReziseToolStripMenuItem.Checked = False
            Me.Refresh()
            Dim Metalid As String = ""
            Dim Cashid As String = ""
            strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'"
            Metalid = GetSqlValue(cn, strSql)
            strSql = " SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'"
            Cashid = GetSqlValue(cn, strSql)
            'Call function for datas add into grid GridBillWiseTransaction
            'cmd = New OleDbCommand
            'cmd.CommandType = CommandType.StoredProcedure
            strSql = " EXEC " & cnStockDb & "..SP_RPT_BILLWISETRANSACTION"
            strSql += vbcrlf + "  @DATE = '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbcrlf + "  ,@TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbcrlf + "  ,@COSTCENTRE='" & Replace(cmbCostCentre.Text, "'", "''''") & "'"
            strSql += vbcrlf + "  ,@NODEID='" & Replace(txtNodeId.Text, "'", "''''") & "'"
            strSql += vbcrlf + "  ,@SystemId='" & systemId & "'"
            strSql += vbcrlf + "  ,@COMPANYID='" & SelectedCompany & "'"
            strSql += vbcrlf + "  ,@WITHORD='" & IIf(ChkOrderRecipt.Checked, "Y", "N") & "'"
            strSql += vbcrlf + "  ,@WITHCANBILL='" & IIf(chkCanelBill.Checked, "Y", "N") & "'"
            strSql += vbCrLf + "  ,@ADMINDB='" & cnAdminDb & "'"
            strSql += vbCrLf + "  ,@METAL = '" & Metalid & "' "
            strSql += vbCrLf + "  ,@CASHID = '" & Cashid & "' "
            strSql += vbCrLf + "  ,@BILLNO = '' "
            strSql += vbCrLf + "  ,@WITHAPPROVAL='" & IIf(chkWithAppIssRec.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ' -----FINAL QUERY FOR ITEMDETAIL
            If chkItemDetail.Checked = True Then
                strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPFINAL2') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPFINAL2 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                'strSql = "IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "FINAL )>0"
                'strSql += vbCrLf + "  BEGIN"
                strSql = "SELECT "
                strSql += vbCrLf + "  BILLNO,TRANDATE"
                If ChkWithHSN.Checked Then
                    strSql += vbCrLf + "  ,HSN"
                End If
                strSql += vbCrLf + "  ,TRANTYPE"
                If ChkWithRunno.Checked Then
                    strSql += vbCrLf + "  ,RUNNO"
                End If
                strSql += vbCrLf + "  ,CATEGORY,IITEMNAME"
                If chkWithSubItem.Checked Then
                    strSql += vbCrLf + "  ,SUBITEMNAME"
                End If
                strSql += vbCrLf + "  ,ITAGNO,IITEMCTRNAME"
                strSql += vbCrLf + "  ,CASE WHEN IPCS <> 0 THEN IPCS ELSE NULL END IPCS"
                strSql += vbCrLf + "  ,CASE WHEN IGRSWT <> 0 THEN IGRSWT ELSE NULL END IGRSWT"
                strSql += vbCrLf + "  ,CASE WHEN ILESSWT <> 0 THEN ILESSWT ELSE NULL END ILESSWT"
                strSql += vbCrLf + "  ,CASE WHEN INETWT <> 0 THEN INETWT ELSE NULL END INETWT"
                strSql += vbCrLf + "  ,CASE WHEN IADVWT <> 0 THEN IADVWT ELSE NULL END IADVWT"
                strSql += vbCrLf + "  ,CASE WHEN IBALWT <> 0 THEN IBALWT ELSE NULL END IBALWT"

                strSql += vbCrLf + "  ,CASE WHEN ISTNWT <> 0 THEN ISTNWT ELSE NULL END ISTNWT"
                strSql += vbCrLf + "  ,CASE WHEN IDIAWT <> 0 THEN IDIAWT ELSE NULL END IDIAWT"

                strSql += vbCrLf + "  ,CASE WHEN IWASTPER <> 0 THEN IWASTPER ELSE NULL END IWASTPER"
                strSql += vbCrLf + "  ,CASE WHEN IWASTAGE <> 0 THEN IWASTAGE ELSE NULL END IWASTAGE"
                strSql += vbCrLf + "  ,CASE WHEN IMCHARGE <> 0 THEN IMCHARGE ELSE NULL END IMCHARGE"
                strSql += vbCrLf + "  ,CASE WHEN IRATE <> 0 THEN IRATE ELSE NULL END IRATE"
                strSql += vbCrLf + "  ,CASE WHEN IPURITY <> 0 THEN IPURITY ELSE NULL END IPURITY"

                strSql += vbCrLf + "  ,CASE WHEN ISTNAMT <> 0 THEN ISTNAMT ELSE NULL END ISTNAMT"
                strSql += vbCrLf + "  ,CASE WHEN IDIAAMT <> 0 THEN IDIAAMT ELSE NULL END IDIAAMT"

                strSql += vbCrLf + "  ,CASE WHEN IMISCAMT <> 0 THEN IMISCAMT ELSE NULL END IMISCAMT"
                If ChkWithTax.Checked Then
                    strSql += vbCrLf + "  ,CASE WHEN SGST <> 0 THEN SGST ELSE NULL END SGST"
                    strSql += vbCrLf + "  ,CASE WHEN CGST <> 0 THEN CGST ELSE NULL END CGST"
                    strSql += vbCrLf + "  ,CASE WHEN IGST <> 0 THEN IGST ELSE NULL END IGST"
                    strSql += vbCrLf + "  ,CASE WHEN GST <> 0 THEN GST ELSE NULL END GST"
                End If
                strSql += vbCrLf + "  ,CASE WHEN IAMOUNT <> 0 THEN IAMOUNT ELSE NULL END IAMOUNT"
                If BILLTRAN_MARGIN Then
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(CASE WHEN IGRSWT <> 0 THEN ((IWASTAGE + IMCHARGE/( CASE WHEN IRATE=0 THEN 1 ELSE IRATE END ) ) - ((ISNULL(DISCOUNT,0)+ ISNULL(DISCOUNT1,0)) / ( CASE WHEN IRATE=0 THEN 1 ELSE IRATE END ))) * 100 / INETWT END))ACTUAL_MARGIN"
                End If
                strSql += vbCrLf + "  ,RITEMNAME"
                strSql += vbCrLf + "  ,CASE WHEN RPCS <> 0 THEN RPCS ELSE NULL END RPCS"
                strSql += vbCrLf + "  ,CASE WHEN RGRSWT <> 0 THEN RGRSWT ELSE NULL END RGRSWT"
                strSql += vbCrLf + "  ,CASE WHEN RLESSWT <> 0 THEN RLESSWT ELSE NULL END RLESSWT"
                strSql += vbCrLf + "  ,CASE WHEN RNETWT <> 0 THEN RNETWT ELSE NULL END RNETWT"

                strSql += vbCrLf + "  ,CASE WHEN RSTNWT <> 0 THEN RSTNWT ELSE NULL END RSTNWT"
                strSql += vbCrLf + "  ,CASE WHEN RDIAWT <> 0 THEN RDIAWT ELSE NULL END RDIAWT"

                strSql += vbCrLf + "  ,CASE WHEN RPURITY <> 0 THEN RPURITY ELSE NULL END RPURITY"
                strSql += vbCrLf + "  ,CASE WHEN RWASTAGE <> 0 THEN RWASTAGE ELSE NULL END RWASTAGE"
                strSql += vbCrLf + "  ,CASE WHEN RMCHARGE <> 0 THEN RMCHARGE ELSE NULL END RMCHARGE"

                strSql += vbCrLf + "  ,CASE WHEN RSTNAMT <> 0 THEN RSTNAMT ELSE NULL END RSTNAMT"
                strSql += vbCrLf + "  ,CASE WHEN RDIAAMT <> 0 THEN RDIAAMT ELSE NULL END RDIAAMT"

                strSql += vbCrLf + "  ,CASE WHEN RAMOUNT <> 0 THEN RAMOUNT ELSE NULL END RAMOUNT"
                strSql += vbCrLf + " ,CASE WHEN RSGST <> 0 THEN RSGST ELSE NULL END RSGST"
                strSql += vbCrLf + " ,CASE WHEN RCGST <> 0 THEN RCGST ELSE NULL END RCGST"
                strSql += vbCrLf + " ,CASE WHEN RIGST <> 0 THEN RIGST ELSE NULL END RIGST"
                strSql += vbCrLf + " ,CASE WHEN RNETAMOUNT <> 0 THEN RNETAMOUNT ELSE NULL END RNETAMOUNT"
                strSql += vbCrLf + "  ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END BALANCE"
                strSql += vbCrLf + "  ,CASE WHEN CASH <> 0 THEN CASH ELSE NULL END CASH"
                strSql += vbCrLf + "  ,CASE WHEN CARD <> 0 THEN CARD ELSE NULL END CARD"
                strSql += vbCrLf + "  ,CASE WHEN CHEQUE <> 0 THEN CHEQUE ELSE NULL END CHEQUE"
                strSql += vbCrLf + " ,CASE WHEN ROUNDOFF <> 0 THEN ROUNDOFF ELSE NULL END ROUNDOFF"
                strSql += vbCrLf + " ,CASE WHEN ADVANCE <> 0 THEN ADVANCE ELSE NULL END ADVANCE"
                strSql += vbCrLf + " ,CASE WHEN HANDLOG <> 0 THEN HANDLOG ELSE NULL END HANDLOG"
                strSql += vbCrLf + " ,CASE WHEN DISCOUNT <> 0 THEN DISCOUNT ELSE NULL END DISCOUNT"
                strSql += vbCrLf + " ,CASE WHEN DISCOUNT1 <> 0 THEN DISCOUNT1 ELSE NULL END DISCOUNT1"
                strSql += vbCrLf + " ,CASE WHEN CHITCARD <> 0 THEN CHITCARD ELSE NULL END CHITCARD"
                strSql += vbCrLf + " ,CASE WHEN GIFTVOUCHER <> 0 THEN GIFTVOUCHER ELSE NULL END GIFTVOUCHER"
                strSql += vbCrLf + " ,CASE WHEN CREDIT <> 0 THEN CREDIT ELSE NULL END CREDIT"
                strSql += vbCrLf + " ,CASE WHEN JND <> 0 THEN JND ELSE NULL END JND"
                strSql += vbCrLf + " ,CASE WHEN TOTAL <> 0 THEN TOTAL ELSE NULL END TOTAL"
                strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO,PAN,GSTNO,EMPNAME,USERNAME,NARRATION,STATUS,BATCHNO,TAGTYPE "
                strSql += vbCrLf + " ,RESULT,SEP,KEYNO  "
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPFINAL2 FROM TEMPTABLEDB..TEMP" & systemId & "FINAL "
                If ChkBillTotal.Checked = False Then strSql += vbCrLf + "  WHERE BILLNO <> 'SUB TOTAL'"
                strSql += vbCrLf + "  ORDER BY SEP,BATCHNO,KEYNO"
                'strSql += vbCrLf + "  END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If MINUSDISCOUNT_BILLTRAN Then
                    'COMMENT ON 04-08-2018
                    'strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPFINALX') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPFINALX"
                    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    'cmd.ExecuteNonQuery()
                    'strSql = "  SELECT * INTO TEMPTABLEDB..TEMPFINALX FROM TEMPTABLEDB..TEMPFINAL2 WHERE BILLNO='SUB TOTAL'"
                    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    'cmd.ExecuteNonQuery()
                    'strSql = "  DECLARE @BATCHNO AS VARCHAR(15)"
                    'strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT BATCHNO FROM TEMPTABLEDB..TEMPFINALX ORDER BY BATCHNO "
                    'strSql += vbCrLf + "  OPEN CUR"
                    'strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @BATCHNO"
                    'strSql += vbCrLf + "  WHILE @@FETCH_STATUS=0"
                    'strSql += vbCrLf + "  BEGIN "
                    'strSql += vbCrLf + "  PRINT @BATCHNO"
                    'strSql += vbCrLf + "  DECLARE @KEYNO AS INT"
                    'strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPFINAL2 SET CASH=NULL,CARD=NULL,CHEQUE=NULL,ROUNDOFF=NULL,ADVANCE=NULL,HANDLOG=NULL,DISCOUNT=NULL"
                    'strSql += vbCrLf + "  ,DISCOUNT1=NULL,CHITCARD=NULL,GIFTVOUCHER=NULL,CREDIT=NULL,JND=NULL WHERE BATCHNO=@BATCHNO"
                    'strSql += vbCrLf + "  SELECT @KEYNO = (SELECT TOP 1 KEYNO FROM TEMPTABLEDB..TEMPFINAL2 AS T WHERE BATCHNO=@BATCHNO ORDER BY SEP,BATCHNO,T.KEYNO)"
                    'strSql += vbCrLf + "  PRINT @KEYNO"
                    'strSql += vbCrLf + "  UPDATE A "
                    'strSql += vbCrLf + "  SET A.CASH=B.CASH,A.CARD=B.CARD,A.CHEQUE=B.CHEQUE,A.ROUNDOFF=B.ROUNDOFF,A.ADVANCE=B.ADVANCE"
                    'strSql += vbCrLf + "  ,A.HANDLOG=B.HANDLOG,A.DISCOUNT=B.DISCOUNT,A.DISCOUNT1=B.DISCOUNT1,A.CHITCARD=B.CHITCARD"
                    'strSql += vbCrLf + "  ,A.GIFTVOUCHER=B.GIFTVOUCHER,A.CREDIT=B.CREDIT,A.JND=B.JND"
                    'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPFINAL2 AS A,TEMPTABLEDB..TEMPFINALX AS B "
                    'strSql += vbCrLf + "  WHERE A.BATCHNO=B.BATCHNO And A.BATCHNO=@BATCHNO  And B.BATCHNO=@BATCHNO "
                    'strSql += vbCrLf + "  AND A.KEYNO =@KEYNO"
                    'strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @BATCHNO"
                    'strSql += vbCrLf + "  END"
                    'strSql += vbCrLf + "  CLOSE CUR"
                    'strSql += vbCrLf + "  DEALLOCATE CUR"
                    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    'cmd.ExecuteNonQuery()
                End If
                If BILLTRAN_MARGIN Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPFINAL2(CATEGORY,IITEMNAME,SEP,RESULT)"
                    strSql += vbCrLf + "  SELECT DISTINCT CATEGORY,CATEGORY,SEP,0 FROM TEMPTABLEDB..TEMPFINAL2 WHERE  SEP=1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPFINAL2(CATEGORY,IITEMNAME,IPCS,IGRSWT,ILESSWT,INETWT,IADVWT,IBALWT,IWASTPER,IWASTAGE,IMCHARGE,IRATE,ISTNAMT"
                    strSql += vbCrLf + "  ,IMISCAMT,IAMOUNT,ACTUAL_MARGIN,RPCS,RGRSWT,RLESSWT,RNETWT,RWASTAGE,RMCHARGE,RSTNAMT,RAMOUNT,SEP,BATCHNO,RESULT)"
                    strSql += vbCrLf + "  SELECT DISTINCT CATEGORY,CATEGORY,SUM(IPCS),SUM(IGRSWT),SUM(ILESSWT),SUM(INETWT),SUM(IADVWT),SUM(IBALWT),SUM(IWASTPER),SUM(IWASTAGE),"
                    strSql += vbCrLf + "  SUM(IMCHARGE),SUM(IRATE),SUM(ISTNAMT),SUM(IMISCAMT),SUM(IAMOUNT),AVG(ACTUAL_MARGIN),SUM(RPCS),SUM(RGRSWT),SUM(RLESSWT),"
                    strSql += vbCrLf + "  SUM(RNETWT),SUM(RWASTAGE),SUM(RMCHARGE),SUM(RSTNAMT),SUM(RAMOUNT)"
                    strSql += vbCrLf + "  ,SEP,'ZZZZZZZZ' BATCHNO,9 FROM TEMPTABLEDB..TEMPFINAL2 WHERE  SEP=1 GROUP BY CATEGORY,SEP"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    strSql = " SELECT * FROM TEMPTABLEDB..TEMPFINAL2 ORDER BY SEP,CATEGORY,BATCHNO,KEYNO "
                Else
                    strSql = " SELECT * FROM TEMPTABLEDB..TEMPFINAL2 ORDER BY SEP,BATCHNO,KEYNO "
                End If
            Else
                ' -----FINAL QUERY FOR NOITEMDETAIL
                If ChkSummaryView.Checked = False Then
                    strSql = "IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "FINAL )>0"
                    strSql += vbCrLf + "  BEGIN"
                    strSql += vbCrLf + "  SELECT  BILLNO"
                    strSql += vbCrLf + "  ,TRANDATE,TRANTYPE"
                    If ChkWithCatName.Checked Then
                        strSql += vbCrLf + "  ,CATEGORY "
                    End If
                    If ChkWithHSN.Checked Then
                        strSql += vbCrLf + "  ,HSN"
                    End If
                    If ChkWithRunno.Checked Then
                        strSql += vbCrLf + "  ,RUNNO"
                    End If
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IPCS) <> 0 THEN SUM(IPCS) ELSE NULL END IPCS"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IGRSWT) <> 0 THEN SUM(IGRSWT) ELSE NULL END IGRSWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(ILESSWT) <> 0 THEN SUM(INETWT) ELSE NULL END ILESSWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(INETWT) <> 0 THEN SUM(INETWT) ELSE NULL END INETWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IADVWT) <> 0 THEN SUM(IADVWT) ELSE NULL END IADVWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IBALWT) <> 0 THEN SUM(IBALWT) ELSE NULL END IBALWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(ISTNWT) <> 0 THEN SUM(ISTNWT) ELSE NULL END ISTNWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IDIAWT) <> 0 THEN SUM(IDIAWT) ELSE NULL END IDIAWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IWASTPER) <> 0 THEN SUM(IWASTPER) ELSE NULL END IWASTPER"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IWASTAGE) <> 0 THEN SUM(IWASTAGE) ELSE NULL END IWASTAGE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IMCHARGE) <> 0 THEN SUM(IMCHARGE) ELSE NULL END IMCHARGE"
                    strSql += vbCrLf + "  ,CASE WHEN IRATE <> 0 THEN IRATE ELSE NULL END IRATE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(ISTNAMT) <> 0 THEN SUM(ISTNAMT) ELSE NULL END ISTNAMT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IDIAAMT) <> 0 THEN SUM(IDIAAMT) ELSE NULL END IDIAAMT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IMISCAMT) <> 0 THEN SUM(IMISCAMT) ELSE NULL END IMISCAMT"
                    If ChkWithTax.Checked Then
                        strSql += vbCrLf + " ,CASE WHEN SUM(SGST) <> 0 THEN SUM(SGST) ELSE NULL END SGST"
                        strSql += vbCrLf + " ,CASE WHEN SUM(CGST) <> 0 THEN SUM(CGST) ELSE NULL END CGST"
                        strSql += vbCrLf + " ,CASE WHEN SUM(IGST) <> 0 THEN SUM(IGST) ELSE NULL END IGST"
                        strSql += vbCrLf + " ,CASE WHEN SUM(GST) <> 0 THEN SUM(GST) ELSE NULL END GST"
                    End If
                    strSql += vbCrLf + " ,CASE WHEN SUM(IAMOUNT) <> 0 THEN SUM(IAMOUNT) ELSE NULL END IAMOUNT"
                    If BILLTRAN_MARGIN Then
                        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(CASE WHEN SUM(IGRSWT) <> 0 THEN ((SUM(IWASTAGE) + SUM(IMCHARGE)/( CASE WHEN SUM(IRATE)=0 THEN 1 ELSE SUM(IRATE) END ) ) - ((ISNULL(SUM(DISCOUNT),0)+ ISNULL(SUM(DISCOUNT1),0)) / ( CASE WHEN SUM(IRATE)=0 THEN 1 ELSE SUM(IRATE) END ))) * 100 / SUM(INETWT) END))ACTUAL_MARGIN"
                    End If
                    strSql += vbCrLf + " ,CASE WHEN SUM(RPCS) <> 0 THEN SUM(RPCS) ELSE NULL END RPCS"
                    strSql += vbCrLf + " ,CASE WHEN SUM(RGRSWT) <> 0 THEN SUM(RGRSWT) ELSE NULL END RGRSWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RNETWT) <> 0 THEN SUM(RNETWT) ELSE NULL END RNETWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RLESSWT) <> 0 THEN SUM(RLESSWT) ELSE NULL END RLESSWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSTNWT) <> 0 THEN SUM(RSTNWT) ELSE NULL END RSTNWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RDIAWT) <> 0 THEN SUM(RDIAWT) ELSE NULL END RDIAWT"
                    If ChkWithCatName.Checked Then
                        strSql += vbCrLf + "  ,CASE WHEN SUM(RPURITY) <> 0 THEN SUM(RPURITY) ELSE NULL END RPURITY"
                    End If

                    strSql += vbCrLf + "  ,CASE WHEN SUM(RWASTAGE) <> 0 THEN SUM(RWASTAGE) ELSE NULL END RWASTAGE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RMCHARGE) <> 0 THEN SUM(RMCHARGE) ELSE NULL END RMCHARGE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSTNAMT) <> 0 THEN SUM(RSTNAMT) ELSE NULL END RSTNAMT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RDIAAMT) <> 0 THEN SUM(RDIAAMT) ELSE NULL END RDIAAMT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(RAMOUNT) <> 0 THEN SUM(RAMOUNT) ELSE NULL END RAMOUNT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSGST) <> 0 THEN SUM(RSGST) ELSE NULL END RSGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RCGST) <> 0 THEN SUM(RCGST) ELSE NULL END RCGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RIGST) <> 0 THEN SUM(RIGST) ELSE NULL END RIGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RNETAMOUNT) <> 0 THEN SUM(RNETAMOUNT) ELSE NULL END RNETAMOUNT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(BALANCE) <> 0 THEN SUM(BALANCE) ELSE NULL END BALANCE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CASH) <> 0 THEN SUM(CASH) ELSE NULL END CASH"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CARD) <> 0 THEN SUM(CARD) ELSE NULL END CARD"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CHEQUE) <> 0 THEN SUM(CHEQUE) ELSE NULL END CHEQUE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(ROUNDOFF) <> 0 THEN SUM(ROUNDOFF) ELSE NULL END ROUNDOFF"
                    strSql += vbCrLf + " ,CASE WHEN SUM(ADVANCE) <> 0 THEN SUM(ADVANCE) ELSE NULL END ADVANCE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(HANDLOG) <> 0 THEN SUM(HANDLOG) ELSE NULL END HANDLOG"
                    strSql += vbCrLf + " ,CASE WHEN SUM(DISCOUNT) <> 0 THEN SUM(DISCOUNT) ELSE NULL END DISCOUNT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(DISCOUNT1) <> 0 THEN SUM(DISCOUNT1) ELSE NULL END DISCOUNT1"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CHITCARD) <> 0 THEN SUM(CHITCARD) ELSE NULL END CHITCARD"
                    strSql += vbCrLf + " ,CASE WHEN SUM(GIFTVOUCHER) <> 0 THEN SUM(GIFTVOUCHER) ELSE NULL END GIFTVOUCHER"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT) <> 0 THEN SUM(CREDIT) ELSE NULL END CREDIT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(JND) <> 0 THEN SUM(JND) ELSE NULL END JND"
                    strSql += vbCrLf + " ,CASE WHEN SUM(TOTAL) <> 0 THEN SUM(TOTAL) ELSE NULL END TOTAL"
                    strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO,EMPNAME,USERNAME,NARRATION,STATUS,BATCHNO,TAGTYPE"
                    strSql += vbCrLf + " ,SEP,KEYNO,RESULT"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "FINAL "
                    If ChkBillTotal.Checked = False Then strSql += vbCrLf + "  WHERE BILLNO <> 'SUB TOTAL'"
                    strSql += vbCrLf + "  GROUP BY BATCHNO,BILLNO,TRANDATE,IRATE,CUSTOMER,ADDRESS,PHONENO,SEP,KEYNO,RESULT,GSTNO,EMPNAME,USERNAME,NARRATION,STATUS,TAGTYPE,TRANTYPE "
                    If ChkWithCatName.Checked Then
                        strSql += vbCrLf + "  ,CATEGORY,RPURITY "
                    End If
                    If ChkWithHSN.Checked Then
                        strSql += vbCrLf + "  ,HSN"
                    End If
                    If ChkWithRunno.Checked Then
                        strSql += vbCrLf + "  ,RUNNO"
                    End If
                    strSql += vbCrLf + "  ORDER BY SEP,BATCHNO,BILLNO"
                    strSql += vbCrLf + "  END"
                Else
                    strSql = "IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "FINAL )>0"
                    strSql += vbCrLf + "  BEGIN"
                    strSql += vbCrLf + "  SELECT  BILLNO,trandate,TRANTYPE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IPCS) <> 0 THEN SUM(IPCS) ELSE NULL END IPCS"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IGRSWT) <> 0 THEN SUM(IGRSWT) ELSE NULL END IGRSWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(ILESSWT) <> 0 THEN SUM(INETWT) ELSE NULL END ILESSWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(INETWT) <> 0 THEN SUM(INETWT) ELSE NULL END INETWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IADVWT) <> 0 THEN SUM(IADVWT) ELSE NULL END IADVWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IBALWT) <> 0 THEN SUM(IBALWT) ELSE NULL END IBALWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(ISTNWT) <> 0 THEN SUM(ISTNWT) ELSE NULL END ISTNWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IDIAWT) <> 0 THEN SUM(IDIAWT) ELSE NULL END IDIAWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IWASTPER) <> 0 THEN SUM(IWASTPER) ELSE NULL END IWASTPER"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IWASTAGE) <> 0 THEN SUM(IWASTAGE) ELSE NULL END IWASTAGE"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IMCHARGE) <> 0 THEN SUM(IMCHARGE) ELSE NULL END IMCHARGE"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN IRATE <> 0 THEN IRATE ELSE NULL END IRATE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(ISTNAMT) <> 0 THEN SUM(ISTNAMT) ELSE NULL END ISTNAMT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(IDIAAMT) <> 0 THEN SUM(IDIAAMT) ELSE NULL END IDIAAMT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(IMISCAMT) <> 0 THEN SUM(IMISCAMT) ELSE NULL END IMISCAMT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(IAMOUNT) <> 0 THEN SUM(IAMOUNT) ELSE NULL END IAMOUNT"
                    If BILLTRAN_MARGIN Then
                        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(CASE WHEN SUM(IGRSWT) <> 0 THEN ((SUM(IWASTAGE) + SUM(IMCHARGE)/( CASE WHEN SUM(IRATE)=0 THEN 1 ELSE SUM(IRATE) END ) ) - ((ISNULL(SUM(DISCOUNT),0)+ ISNULL(SUM(DISCOUNT1),0)) / ( CASE WHEN SUM(IRATE)=0 THEN 1 ELSE SUM(IRATE) END ))) * 100 / SUM(INETWT) END))ACTUAL_MARGIN"
                    End If
                    strSql += vbCrLf + " ,CASE WHEN SUM(RPCS) <> 0 THEN SUM(RPCS) ELSE NULL END RPCS"
                    strSql += vbCrLf + " ,CASE WHEN SUM(RGRSWT) <> 0 THEN SUM(RGRSWT) ELSE NULL END RGRSWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(RNETWT) <> 0 THEN SUM(RNETWT) ELSE NULL END RNETWT"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(RLESSWT) <> 0 THEN SUM(RNETWT) ELSE NULL END RLESSWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSTNWT) <> 0 THEN SUM(RSTNWT) ELSE NULL END RSTNWT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RDIAWT) <> 0 THEN SUM(RDIAWT) ELSE NULL END RDIAWT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(RPURITY) <> 0 THEN SUM(RPURITY) ELSE NULL END RPURITY"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(RWASTAGE) <> 0 THEN SUM(RWASTAGE) ELSE NULL END RWASTAGE"
                    If ChkSummaryView.Checked = False Then strSql += vbCrLf + "  ,CASE WHEN SUM(RMCHARGE) <> 0 THEN SUM(RMCHARGE) ELSE NULL END RMCHARGE"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSTNAMT) <> 0 THEN SUM(RSTNAMT) ELSE NULL END RSTNAMT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RDIAAMT) <> 0 THEN SUM(RDIAAMT) ELSE NULL END RDIAAMT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(RAMOUNT) <> 0 THEN SUM(RAMOUNT) ELSE NULL END RAMOUNT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RSGST) <> 0 THEN SUM(RSGST) ELSE NULL END RSGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RCGST) <> 0 THEN SUM(RCGST) ELSE NULL END RCGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RIGST) <> 0 THEN SUM(RIGST) ELSE NULL END RIGST"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(RNETAMOUNT) <> 0 THEN SUM(RNETAMOUNT) ELSE NULL END RNETAMOUNT"
                    strSql += vbCrLf + "  ,CASE WHEN SUM(BALANCE) <> 0 THEN SUM(BALANCE) ELSE NULL END BALANCE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CASH) <> 0 THEN SUM(CASH) ELSE NULL END CASH"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CARD) <> 0 THEN SUM(CARD) ELSE NULL END CARD"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CHEQUE) <> 0 THEN SUM(CHEQUE) ELSE NULL END CHEQUE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(ROUNDOFF) <> 0 THEN SUM(ROUNDOFF) ELSE NULL END ROUNDOFF"
                    strSql += vbCrLf + " ,CASE WHEN SUM(ADVANCE) <> 0 THEN SUM(ADVANCE) ELSE NULL END ADVANCE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(HANDLOG) <> 0 THEN SUM(HANDLOG) ELSE NULL END HANDLOG"
                    strSql += vbCrLf + " ,CASE WHEN SUM(DISCOUNT) <> 0 THEN SUM(DISCOUNT) ELSE NULL END DISCOUNT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(DISCOUNT1) <> 0 THEN SUM(DISCOUNT1) ELSE NULL END DISCOUNT1"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CHITCARD) <> 0 THEN SUM(CHITCARD) ELSE NULL END CHITCARD"
                    strSql += vbCrLf + " ,CASE WHEN SUM(GIFTVOUCHER) <> 0 THEN SUM(GIFTVOUCHER) ELSE NULL END GIFTVOUCHER"
                    strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT) <> 0 THEN SUM(CREDIT) ELSE NULL END CREDIT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(JND) <> 0 THEN SUM(JND) ELSE NULL END JND"
                    strSql += vbCrLf + " ,CASE WHEN SUM(TOTAL) <> 0 THEN SUM(TOTAL) ELSE NULL END TOTAL"
                    strSql += vbCrLf + " ,CUSTOMER,ADDRESS,PHONENO,EMPNAME,USERNAME,NARRATION,STATUS,BATCHNO,TAGTYPE"
                    strSql += vbCrLf + " ,SEP"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "FINAL "
                    If ChkBillTotal.Checked = False Then strSql += vbCrLf + "  WHERE BILLNO <> 'SUB TOTAL'"
                    strSql += vbCrLf + "  GROUP BY BATCHNO,BILLNO,TRANDATE,IRATE,CUSTOMER,ADDRESS,PHONENO,SEP,GSTNO,EMPNAME,USERNAME,NARRATION,STATUS,TAGTYPE,TRANTYPE "
                    strSql += vbCrLf + "  ORDER BY SEP,BATCHNO,BILLNO"
                    strSql += vbCrLf + "  END"
                End If
            End If
            '
            If ChkSummaryView.Checked = True Then
                dtFinal.Columns.Add("KEYNO", GetType(Integer))
                dtFinal.Columns("KEYNO").AutoIncrement = True
                dtFinal.Columns("KEYNO").AutoIncrementSeed = 0
                dtFinal.Columns("KEYNO").AutoIncrementStep = 1
                dtFinal.Columns.Add("RESULT", GetType(Integer))
                dtFinal.Columns("RESULT").AutoIncrement = True
                dtFinal.Columns("RESULT").AutoIncrementSeed = 0
                dtFinal.Columns("RESULT").AutoIncrementStep = 1
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtFinal)
            If dtFinal.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If

            If chkSpecificSummary.Checked = True Then
                If dtFinal.Rows.Count > 0 Then
                    strSql = ""
                    strSql += vbCrLf + " EXEC " & cnAdminDb & "..[RPT_SALEBILLWISETRANSACTIONSUMMARYVIEW] "
                    strSql += vbCrLf + " @ADMINDB='" & cnAdminDb & "'"
                    strSql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
                    strSql += vbCrLf + " ,@FDATE='" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " ,@TDATE='" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
                    If cmbCostCentre.Text = "ALL" Or cmbCostCentre.Text = "" Then
                        strSql += vbCrLf + " ,@COSTNAME=''"
                    Else
                        strSql += vbCrLf + " ,@COSTNAME='" & cmbCostCentre.Text & "'"
                    End If
                    Dim ds As New DataSet
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(ds)
                    For i As Integer = 0 To ds.Tables.Count - 1
                        Dim dt As New DataTable
                        dt = ds.Tables(i)
                        Dim dr As DataRow = Nothing
                        If dt.Rows.Count > 0 Then
                            For j As Integer = 0 To dt.Rows.Count - 1
                                If i = 0 Then
                                    dr = Nothing
                                    If j = 0 Then
                                        dr = dtFinal.NewRow
                                        dr!BILLNO = "DETAIL"
                                        dr!RESULT = 999
                                        dtFinal.Rows.Add(dr)

                                        dr = dtFinal.NewRow
                                        dr!BILLNO = "PARTICULAR"
                                        dr!RESULT = 999
                                        dtFinal.Rows.Add(dr)
                                    End If
                                    dr = dtFinal.NewRow
                                    dr!BILLNO = dt.Rows(j).Item("PARTICULARNAME").ToString
                                    dr!CASH = dt.Rows(j).Item("RECEIPT").ToString
                                    dr!CARD = dt.Rows(j).Item("PAYMENT").ToString
                                    dr!CHEQUE = dt.Rows(j).Item("TOTAL").ToString
                                    dtFinal.Rows.Add(dr)
                                End If
                                If i = 1 Then
                                    If j = 0 Then
                                        dr = dtFinal.NewRow
                                        dr!BILLNO = ""
                                        dr!RESULT = 999
                                        dtFinal.Rows.Add(dr)
                                    End If
                                    dr = Nothing
                                    dr = dtFinal.NewRow
                                    dr!BILLNO = dt.Rows(j).Item("PARTICULAR").ToString
                                    dr!CASH = dt.Rows(j).Item("FROMBILLNO").ToString
                                    dr!CARD = dt.Rows(j).Item("TOBILLNO").ToString
                                    dr!CHEQUE = DBNull.Value
                                    dtFinal.Rows.Add(dr)
                                End If
                            Next
                        End If
                    Next
                End If
            End If
            gridBillWiseTransaction.DataSource = dtFinal
            funcGridStyle()
            'Add Title of the Report in label
            If gridBillWiseTransaction.Rows.Count > 0 Then
                gridBillWiseTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridBillWiseTransaction.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridBillWiseTransaction.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridBillWiseTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

                lblTitle.Visible = True
                Dim strTitle As String = Nothing
                strTitle = "BILLWISE TRANSACTION REPORT"
                strTitle += " FROM " & dtpDate.Text & " TO " & dtpTo.Text
                If (cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "") Or (txtNodeId.Text <> "ALL" And txtNodeId.Text <> "") Then
                    strTitle += "("
                End If


                If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                    strTitle += "" & cmbCostCentre.Text & ","
                End If
                If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
                    strTitle += "" & txtNodeId.Text & ""
                End If
                If Strings.Right(strTitle, 1) = "," Then
                    strTitle = Strings.Left(strTitle, strTitle.Length - 1)
                End If
                If (cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "") Or (txtNodeId.Text <> "ALL" And txtNodeId.Text <> "") Then
                    strTitle += ")"
                End If
                lblTitle.Text = strTitle
                lblTitle.Height = gridBillWiseTransaction.ColumnHeadersHeight
                gridBillWiseTransaction.Columns("BILLNO").Frozen = True
                gridHeader.Columns("BILLNO").Frozen = True
                gridBillWiseTransaction.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub

    Private Sub frmBillWiseTransaction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmBillWiseTransaction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkItemDetail.Checked = True
        funcAddCostCentre()
        funcAddCashCounter()
        funcAddMetal()
        LoadCompany(chkLstCompany)
        dtpDate.Value = GetServerDate(tran)
        lblTitle.Visible = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Function funcAddCostCentre() As Integer
        strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
        cmbCostCentre.Items.Clear()
        cmbCostCentre.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCostCentre, False, False)
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
    End Function

    Function funcAddCashCounter() As Integer
        strSql = "SELECT DISTINCT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        cmbCashCounter.Items.Clear()
        cmbCashCounter.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCashCounter, False, False)
        cmbCashCounter.Text = "ALL"
    End Function

    Function funcAddMetal() As Integer
        strSql = "select METALNAME from " & cnAdminDb & "..METALMAST order by DISPLAYORDER"
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        cmbMetal.Text = "ALL"
    End Function


    Public Function funcGridStyle() As Integer
        With gridBillWiseTransaction
            .Columns("BATCHNO").Visible = False
            .Columns("SEP").Visible = False
            If chkTagType.Checked Then
                .Columns("TAGTYPE").Visible = True
                .Columns("RSGST").Visible = True
                .Columns("RCGST").Visible = True
                .Columns("RIGST").Visible = True
                .Columns("RAMOUNT").Visible = True
                .Columns("RNETAMOUNT").Visible = True
            Else
                .Columns("TAGTYPE").Visible = False
                .Columns("RSGST").Visible = False
                .Columns("RCGST").Visible = False
                .Columns("RIGST").Visible = False
                .Columns("RAMOUNT").Visible = False
                .Columns("RNETAMOUNT").Visible = True
            End If
            If ChkWithCatName.Checked = True Then
                If .Columns.Contains("IPURITY") Then
                    With .Columns("IPURITY")
                        .Width = 70
                        .HeaderText = "PURITY"
                        .DefaultCellStyle.Format = "0.00"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                    End With
                End If
                If .Columns.Contains("RPURITY") Then
                    With .Columns("RPURITY")
                        .Width = 70
                        .HeaderText = "PURITY"
                        .DefaultCellStyle.Format = "0.00"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                    End With
                End If
                If .Columns.Contains("RPURITY") Then .Columns("RPURITY").Visible = True
                If .Columns.Contains("CATEGORY") Then .Columns("CATEGORY").Visible = True
                If .Columns.Contains("IPURITY") Then .Columns("IPURITY").Visible = True
                If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = True
            Else
                If .Columns.Contains("RPURITY") Then .Columns("RPURITY").Visible = False
                If .Columns.Contains("IPURITY") Then .Columns("IPURITY").Visible = False
                If .Columns.Contains("CATEGORY") Then .Columns("CATEGORY").Visible = False
                If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            End If
            If ChkSummaryView.Checked = True Then
                If rbtnetwght.Checked = True Then
                    .Columns("IGRSWT").Visible = False
                    If .Columns.Contains("ILESSWT") Then .Columns("ILESSWT").Visible = False
                    .Columns("RGRSWT").Visible = False
                    If .Columns.Contains("RLESSWT") Then .Columns("RLESSWT").Visible = False
                End If
            Else
                If rbtGrswght.Checked = True Then
                    .Columns("INETWT").Visible = False
                    If .Columns.Contains("ILESSWT") Then .Columns("ILESSWT").Visible = False
                    .Columns("RNETWT").Visible = False
                    If .Columns.Contains("RLESSWT") Then .Columns("RLESSWT").Visible = False
                ElseIf rbtnetwght.Checked = True Then
                    .Columns("IGRSWT").Visible = False
                    If .Columns.Contains("ILESSWT") Then .Columns("ILESSWT").Visible = False
                    .Columns("RGRSWT").Visible = False
                    If .Columns.Contains("RLESSWT") Then .Columns("RLESSWT").Visible = False
                End If
            End If
            If BILLTRAN_CATTYPE Then
                If .Columns.Contains("CATEGORY") Then
                    With .Columns("CATEGORY")
                        .Width = 80
                        .HeaderText = "ITEM"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
            End If
            'If chkItemDetail.Checked = True Then
            With .Columns("BILLNO")
                .Width = 80
                .HeaderText = "BILLNO"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .Width = 80
                .HeaderText = "BILLDATE"
                .DefaultCellStyle.Format = "dd-MM-yyyy"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If chkItemDetail.Checked = True Then
                If ChkSummaryView.Checked = False Then
                    With .Columns("IITEMNAME")
                        .Width = 150
                        .HeaderText = "ITEMNAME"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                    End With
                    With .Columns("ITAGNO")
                        .Width = 70
                        .HeaderText = "TAGNO"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                    End With
                    With .Columns("IITEMCTRNAME")
                        .Width = 100
                        .HeaderText = "COUNTERNAME"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                    End With
                    With .Columns("RITEMNAME")
                        .Width = 150
                        .HeaderText = "DESCRIPTION"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
            End If

            With .Columns("TAGTYPE")
                .Width = 100
                .HeaderText = "TAGTYPE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("IPCS")
                .Width = 50
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("IGRSWT")
                .Width = 70
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            If ChkSummaryView.Checked = False Then
                With .Columns("INETWT")
                    .Width = 70
                    .HeaderText = "NETWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("ILESSWT")
                    .Width = 70
                    .HeaderText = "LESSWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("IADVWT")
                    .Width = 70
                    .HeaderText = "ADVWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                    .Visible = ChkOrdAdvWt.Checked
                End With
                With .Columns("IBALWT")
                    .Width = 70
                    .HeaderText = "BALWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("ISTNWT")
                    .Width = 70
                    .HeaderText = "STNWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("IDIAWT")
                    .Width = 70
                    .HeaderText = "DIAWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                    '.Visible = ChkOrdAdvWt.Checked
                End With
                With .Columns("IWASTPER")
                    .Width = 70
                    .HeaderText = "WASTPER"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("IWASTAGE")
                    .Width = 70
                    .HeaderText = "WASTAGE"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With

                With .Columns("IMCHARGE")
                    .Width = 70
                    .HeaderText = "MCHARGE"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With

                With .Columns("IRATE")
                    .Width = 70
                    .HeaderText = "RATE"
                    .DefaultCellStyle.Format = "0.00"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With


                'With .Columns("IPURITY")
                '    .Width = 70
                '    .HeaderText = "RATE"
                '    .DefaultCellStyle.Format = "0.00"
                '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                '    .SortMode = DataGridViewColumnSortMode.NotSortable
                '    .DefaultCellStyle.BackColor = Color.AliceBlue
                'End With



                With .Columns("ISTNAMT")
                    .Width = 80
                    .HeaderText = "STNAMT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("IDIAAMT")
                    .Width = 80
                    .HeaderText = "DIAAMT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With

                With .Columns("IMISCAMT")
                    .Width = 80
                    .HeaderText = "MISCAMT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
            End If
            With .Columns("IAMOUNT")
                .Width = 100
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With


            If .Columns.Contains("GST") Then
                With .Columns("GST")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("SGST")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("CGST")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                With .Columns("IGST")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
            End If
            If .Columns.Contains("HSN") Then
                With .Columns("HSN")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
            End If
            'If .Columns.Contains("CATEGORY") Then .Columns("CATEGORY").Visible = False
            If BILLTRAN_MARGIN Then
                With .Columns("ACTUAL_MARGIN")
                    .Width = 100
                    .HeaderText = "MARGIN"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                End With
                .Columns("BALANCE").Visible = False
                .Columns("CASH").Visible = False
                .Columns("CARD").Visible = False
                .Columns("CHEQUE").Visible = False
                .Columns("ROUNDOFF").Visible = False
                .Columns("ADVANCE").Visible = False
                .Columns("HANDLOG").Visible = False
                .Columns("DISCOUNT").Visible = False
                .Columns("DISCOUNT1").Visible = False
                .Columns("CHITCARD").Visible = False
                .Columns("GIFTVOUCHER").Visible = False
                .Columns("CREDIT").Visible = False
                .Columns("JND").Visible = False
                .Columns("TOTAL").Visible = False
                .Columns("CUSTOMER").Visible = False
                .Columns("ADDRESS").Visible = False
                .Columns("PHONENO").Visible = False
                .Columns("EMPNAME").Visible = False
                .Columns("USERNAME").Visible = False
                .Columns("NARRATION").Visible = False
                .Columns("STATUS").Visible = False
                .Columns("BATCHNO").Visible = False
            End If

            With .Columns("RPCS")
                .Width = 50
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RGRSWT")
                .Width = 70
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If ChkSummaryView.Checked = False Then
                With .Columns("RNETWT")
                    .Width = 70
                    .HeaderText = "NETWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RLESSWT")
                    .Width = 70
                    .HeaderText = "LESSWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RSTNWT")
                    .Width = 70
                    .HeaderText = "STNWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RDIAWT")
                    .Width = 70
                    .HeaderText = "DIAWT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RSTNAMT")
                    .Width = 70
                    .HeaderText = "STNAMT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RDIAAMT")
                    .Width = 70
                    .HeaderText = "DIAAMT"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("RAMOUNT")
                .Width = 100
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RSGST")
                .Width = 100
                .HeaderText = "SGST"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RCGST")
                .Width = 100
                .HeaderText = "CGST"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RIGST")
                .Width = 100
                .HeaderText = "IGST"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RNETAMOUNT")
                .Width = 100
                .HeaderText = "PUR/SR AMOUNT" 'NETAMOUNT
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If ChkSummaryView.Checked = False Then
                With .Columns("RMCHARGE")
                    .Width = 70
                    .HeaderText = "MCHARGE"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("RWASTAGE")
                    .Width = 70
                    .HeaderText = "WASTAGE"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If

            .Columns("keyno").Visible = False
            .Columns("RESULT").Visible = False
            'Else
            '    With .Columns("BILLNO")
            '        .Width = 120
            '        .HeaderText = "BILLNO"
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '    End With
            '    With .Columns("IAMOUNT")
            '        .Width = 100
            '        .HeaderText = "AMOUNT"
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '        .DefaultCellStyle.BackColor = Color.AliceBlue
            '    End With
            '    With .Columns("RAMOUNT")
            '        .Width = 100
            '        .HeaderText = "AMOUNT"
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '    End With
            'End If
            'gridflag.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            With .Columns("BALANCE")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("CASH")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CARD")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHEQUE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ROUNDOFF")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADVANCE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("HANDLOG")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DISCOUNT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DISCOUNT1")
                .HeaderText = "DISC"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHITCARD")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CREDIT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTAL")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("CUSTOMER")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ADDRESS")
                .Width = 180
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PHONENO")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("EMPNAME")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("USERNAME")
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NARRATION")
                .Width = 180
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("STATUS")
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With


        gridBillWiseTransaction.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        For rowCount As Integer = 0 To gridBillWiseTransaction.RowCount - 1
            ' If chkItemDetail.Checked = True Then
            If gridBillWiseTransaction.Rows(rowCount).Cells("RESULT").Value.ToString = "3" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BALANCE").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).Cells("TOTAL").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("RESULT").Value.ToString = "0" And BILLTRAN_MARGIN Then
                If gridBillWiseTransaction.Columns.Contains("IITEMNAME") Then
                    gridBillWiseTransaction.Rows(rowCount).Cells("IITEMNAME").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("RESULT").Value.ToString = "9" And BILLTRAN_MARGIN Then
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.BackColor = Color.MistyRose
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("RESULT").Value.ToString = "9" _
                And gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "CHIT COLLECTION TOTAL" Then
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.BackColor = Color.MistyRose
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "SUB TOTAL" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BALANCE").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).Cells("TOTAL").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            End If
            ' End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "RECEIPT" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Style.BackColor = Color.LightBlue
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "PAYMENT" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Style.BackColor = Color.LightBlue
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "APPROVAL" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Style.BackColor = Color.LightBlue
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "ORDER RECEIPT" Then
                gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Style.BackColor = Color.LightBlue
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "TOTAL" Or gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "TAG TOTAL" Or gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "HOME SALES TOTAL" Or gridBillWiseTransaction.Rows(rowCount).Cells("BILLNO").Value.ToString = "NON TAG TOTAL" Then
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
            If gridBillWiseTransaction.Rows(rowCount).Cells("RESULT").Value.ToString = "999" Then
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                gridBillWiseTransaction.Rows(rowCount).DefaultCellStyle.BackColor = Color.LightBlue
            End If
        Next

        funcGridHeaderNew()
    End Function

    Public Function funcGridHeaderNew() As Integer
        Dim dtheader As New DataTable

        dtheader.Clear()
        Try

            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("BILLNO", GetType(String))
                .Columns.Add("TRANDATE", GetType(String))
                .Columns.Add("TRANTYPE", GetType(String))
                If ChkWithCatName.Checked = True Then
                    .Columns.Add("CATEGORY", GetType(String))
                End If
                .Columns.Add("IITEMNAME~RUNNO~HSN~ITAGNO~IITEMCTRNAME~TAGTYPE~IPCS~IGRSWT~ILESSWT~INETWT~IADVWT~IBALWT~ISTNWT~IDIAWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~IPURITY~ISTNAMT~IDIAAMT~IMISCAMT~SGST~CGST~IGST~GST~IAMOUNT" & IIf(BILLTRAN_MARGIN, "~ACTUAL_MARGIN", ""), GetType(String))
                .Columns.Add("RITEMNAME~RPCS~RGRSWT~RLESSWT~RNETWT~RSTNWT~RDIAWT~RPURITY~RWASTAGE~RMCHARGE~RSTNAMT~RDIAAMT~RAMOUNT~RSGST~RCGST~RIGST~RNETAMOUNT", GetType(String))
                .Columns.Add("BALANCE", GetType(String))
                .Columns.Add("CASH~CARD~CHEQUE~ROUNDOFF~ADVANCE~HANDLOG~DISCOUNT~CHITCARD~GIFTVOUCHER~CREDIT~JND~TOTAL", GetType(String))
                .Columns.Add("CUSTOMER~ADDRESS~PHONENO~PAN~GSTNO~EMPNAME~USERNAME~NARRATION~STATUS", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("IITEMNAME~RUNNO~HSN~ITAGNO~IITEMCTRNAME~TAGTYPE~IPCS~IGRSWT~ILESSWT~INETWT~IADVWT~IBALWT~ISTNWT~IDIAWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~IPURITY~ISTNAMT~IDIAAMT~IMISCAMT~SGST~CGST~IGST~GST~IAMOUNT" & IIf(BILLTRAN_MARGIN, "~ACTUAL_MARGIN", "")).Caption = "SALES"
                .Columns("RITEMNAME~RPCS~RGRSWT~RLESSWT~RNETWT~RSTNWT~RDIAWT~RPURITY~RWASTAGE~RMCHARGE~RSTNAMT~RDIAAMT~RAMOUNT~RSGST~RCGST~RIGST~RNETAMOUNT").Caption = "PURCHASE/RETURN"
                .Columns("BALANCE").Caption = "BALANCE"
                .Columns("CASH~CARD~CHEQUE~ROUNDOFF~ADVANCE~HANDLOG~DISCOUNT~CHITCARD~GIFTVOUCHER~CREDIT~JND~TOTAL").Caption = "PAYMENT"
                .Columns("CUSTOMER~ADDRESS~PHONENO~PAN~GSTNO~EMPNAME~USERNAME~NARRATION~STATUS").Caption = ""
                '.Columns("NARRATION", GetType(String))
                .Columns("SCROLL").Caption = ""
            End With
            gridHeader.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridHeaderStyle() As Integer
        With gridHeader
            With .Columns("BILLNO")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = gridBillWiseTransaction.Columns("BILLNO").Width
                .HeaderText = " "
            End With
            With .Columns("TRANDATE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = gridBillWiseTransaction.Columns("TRANDATE").Width
                .HeaderText = " "
            End With
            If gridBillWiseTransaction.Columns.Contains("TRANTYPE") Then
                With .Columns("TRANTYPE")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .Width = gridBillWiseTransaction.Columns("TRANTYPE").Width
                    .HeaderText = " "
                End With
            End If

            If ChkWithCatName.Checked = True Then
                With .Columns("CATEGORY")
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .Width = gridBillWiseTransaction.Columns("CATEGORY").Width
                    .HeaderText = " "
                End With
            End If

            If chkItemDetail.Checked = True Then
                With .Columns("IITEMNAME~RUNNO~HSN~ITAGNO~IITEMCTRNAME~TAGTYPE~IPCS~IGRSWT~ILESSWT~INETWT~IADVWT~IBALWT~ISTNWT~IDIAWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~IPURITY~ISTNAMT~IDIAAMT~IMISCAMT~SGST~CGST~IGST~GST~IAMOUNT" & IIf(BILLTRAN_MARGIN, "~ACTUAL_MARGIN", ""))
                    .Width = gridBillWiseTransaction.Columns("IITEMNAME").Width + gridBillWiseTransaction.Columns("ITAGNO").Width + gridBillWiseTransaction.Columns("IITEMCTRNAME").Width + gridBillWiseTransaction.Columns("TAGTYPE").Width + gridBillWiseTransaction.Columns("IPCS").Width
                    .Width += IIf(gridBillWiseTransaction.Columns("IGRSWT").Visible = True, gridBillWiseTransaction.Columns("IGRSWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("ILESSWT").Visible = True, gridBillWiseTransaction.Columns("INETWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("INETWT").Visible = True, gridBillWiseTransaction.Columns("INETWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("IADVWT").Visible = True, gridBillWiseTransaction.Columns("IADVWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("IBALWT").Visible = True, gridBillWiseTransaction.Columns("IBALWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("ISTNWT").Visible = True, gridBillWiseTransaction.Columns("ISTNWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("IDIAWT").Visible = True, gridBillWiseTransaction.Columns("IDIAWT").Width, 0)
                    .Width += gridBillWiseTransaction.Columns("IMCHARGE").Width + gridBillWiseTransaction.Columns("IWASTPER").Width
                    .Width += gridBillWiseTransaction.Columns("IWASTAGE").Width + gridBillWiseTransaction.Columns("IRATE").Width
                    .Width += gridBillWiseTransaction.Columns("ISTNAMT").Width + gridBillWiseTransaction.Columns("IDIAAMT").Width
                    .Width += gridBillWiseTransaction.Columns("IMISCAMT").Width + gridBillWiseTransaction.Columns("IAMOUNT").Width
                    .WIDTH += IIf(gridBillWiseTransaction.Columns("IPURITY").Visible = True, gridBillWiseTransaction.Columns("IPURITY").Width, 0)
                    If ChkWithTax.Checked Then
                        .Width += gridBillWiseTransaction.Columns("SGST").Width
                        .Width += gridBillWiseTransaction.Columns("CGST").Width
                        .Width += gridBillWiseTransaction.Columns("IGST").Width
                        .Width += gridBillWiseTransaction.Columns("GST").Width
                    End If
                    If ChkWithHSN.Checked Then
                        .Width += gridBillWiseTransaction.Columns("HSN").Width
                    End If
                    If ChkWithRunno.Checked Then
                        .Width += gridBillWiseTransaction.Columns("RUNNO").Width
                    End If
                    If BILLTRAN_MARGIN Then
                        .Width += gridBillWiseTransaction.Columns("ACTUAL_MARGIN").Width
                    End If

                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .HeaderText = "SALES"
                End With
                With .Columns("RITEMNAME~RPCS~RGRSWT~RLESSWT~RNETWT~RSTNWT~RDIAWT~RPURITY~RWASTAGE~RMCHARGE~RSTNAMT~RDIAAMT~RAMOUNT~RSGST~RCGST~RIGST~RNETAMOUNT")
                    .Width = gridBillWiseTransaction.Columns("RITEMNAME").Width + gridBillWiseTransaction.Columns("RPCS").Width
                    .Width += IIf(gridBillWiseTransaction.Columns("RGRSWT").Visible = True, gridBillWiseTransaction.Columns("RGRSWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("RLESSWT").Visible = True, gridBillWiseTransaction.Columns("RLESSWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("RNETWT").Visible = True, gridBillWiseTransaction.Columns("RNETWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("RSTNWT").Visible = True, gridBillWiseTransaction.Columns("RSTNWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("RDIAWT").Visible = True, gridBillWiseTransaction.Columns("RDIAWT").Width, 0)
                    .Width += IIf(gridBillWiseTransaction.Columns("RPURITY").Visible = True, gridBillWiseTransaction.Columns("RPURITY").Width, 0)
                    .Width += gridBillWiseTransaction.Columns("RWASTAGE").Width + gridBillWiseTransaction.Columns("RMCHARGE").Width
                    .Width += gridBillWiseTransaction.Columns("RSTNAMT").Width
                    .Width += gridBillWiseTransaction.Columns("RDIAAMT").Width
                    .Width += gridBillWiseTransaction.Columns("RAMOUNT").Width
                    .Width += gridBillWiseTransaction.Columns("RSGST").Width
                    .Width += gridBillWiseTransaction.Columns("RCGST").Width
                    .Width += gridBillWiseTransaction.Columns("RIGST").Width
                    .Width += gridBillWiseTransaction.Columns("RNETAMOUNT").Width
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .HeaderText = "PURCHASE/RETURN"
                End With
            Else
                If ChkSummaryView.Checked = False Then
                    With .Columns("IITEMNAME~RUNNO~HSN~ITAGNO~IITEMCTRNAME~TAGTYPE~IPCS~IGRSWT~ILESSWT~INETWT~IADVWT~IBALWT~ISTNWT~IDIAWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~IPURITY~ISTNAMT~IDIAAMT~IMISCAMT~SGST~CGST~IGST~GST~IAMOUNT" & IIf(BILLTRAN_MARGIN, "~ACTUAL_MARGIN", ""))
                        .Width = gridBillWiseTransaction.Columns("IPCS").Width
                        .Width += IIf(gridBillWiseTransaction.Columns("IGRSWT").Visible = True, gridBillWiseTransaction.Columns("IGRSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("ILESSWT").Visible = True, gridBillWiseTransaction.Columns("ILESSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("INETWT").Visible = True, gridBillWiseTransaction.Columns("INETWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("IADVWT").Visible = True, gridBillWiseTransaction.Columns("IADVWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("IBALWT").Visible = True, gridBillWiseTransaction.Columns("IBALWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("ISTNWT").Visible = True, gridBillWiseTransaction.Columns("ISTNWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("IDIAWT").Visible = True, gridBillWiseTransaction.Columns("IDIAWT").Width, 0)
                        .Width += gridBillWiseTransaction.Columns("IMCHARGE").Width
                        .Width += gridBillWiseTransaction.Columns("IWASTPER").Width
                        .Width += gridBillWiseTransaction.Columns("IWASTAGE").Width
                        .Width += gridBillWiseTransaction.Columns("IRATE").Width
                        .Width += gridBillWiseTransaction.Columns("ISTNAMT").Width
                        .Width += gridBillWiseTransaction.Columns("IDIAAMT").Width
                        .Width += gridBillWiseTransaction.Columns("IMISCAMT").Width
                        .Width += gridBillWiseTransaction.Columns("IAMOUNT").Width
                        If ChkWithTax.Checked Then
                            .Width += gridBillWiseTransaction.Columns("SGST").Width
                            .Width += gridBillWiseTransaction.Columns("CGST").Width
                            .Width += gridBillWiseTransaction.Columns("IGST").Width
                            .Width += gridBillWiseTransaction.Columns("GST").Width
                        End If
                        If ChkWithHSN.Checked Then
                            .Width += gridBillWiseTransaction.Columns("HSN").Width
                        End If
                        If ChkWithRunno.Checked Then
                            .Width += gridBillWiseTransaction.Columns("RUNNO").Width
                        End If
                        If BILLTRAN_MARGIN Then
                            .Width += gridBillWiseTransaction.Columns("ACTUAL_MARGIN").Width
                        End If
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .HeaderText = "SALES"
                    End With
                    With .Columns("RITEMNAME~RPCS~RGRSWT~RLESSWT~RNETWT~RSTNWT~RDIAWT~RPURITY~RWASTAGE~RMCHARGE~RSTNAMT~RDIAAMT~RAMOUNT~RSGST~RCGST~RIGST~RNETAMOUNT")
                        .Width = gridBillWiseTransaction.Columns("RPCS").Width
                        .Width += IIf(gridBillWiseTransaction.Columns("RGRSWT").Visible = True, gridBillWiseTransaction.Columns("RGRSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RLESSWT").Visible = True, gridBillWiseTransaction.Columns("RLESSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RNETWT").Visible = True, gridBillWiseTransaction.Columns("RNETWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RSTNWT").Visible = True, gridBillWiseTransaction.Columns("RSTNWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RDIAWT").Visible = True, gridBillWiseTransaction.Columns("RDIAWT").Width, 0)
                        If gridBillWiseTransaction.Columns.Contains("RPURITY") Then
                            .Width += IIf(gridBillWiseTransaction.Columns("RPURITY").Visible = True, gridBillWiseTransaction.Columns("RPURITY").Width, 0)
                        End If
                        .Width += gridBillWiseTransaction.Columns("RWASTAGE").Width
                        .Width += gridBillWiseTransaction.Columns("RMCHARGE").Width
                        .Width += gridBillWiseTransaction.Columns("RSTNAMT").Width
                        .Width += gridBillWiseTransaction.Columns("RDIAAMT").Width
                        .Width += gridBillWiseTransaction.Columns("RAMOUNT").Width
                        .Width += gridBillWiseTransaction.Columns("RSGST").Width
                        .Width += gridBillWiseTransaction.Columns("RCGST").Width
                        .Width += gridBillWiseTransaction.Columns("RIGST").Width
                        .Width += gridBillWiseTransaction.Columns("RNETAMOUNT").Width
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .HeaderText = "PURCHASE/RETURN"
                    End With
                Else
                    With .Columns("IITEMNAME~RUNNO~HSN~ITAGNO~IITEMCTRNAME~TAGTYPE~IPCS~IGRSWT~ILESSWT~INETWT~IADVWT~IBALWT~ISTNWT~IDIAWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~IPURITY~ISTNAMT~IDIAAMT~IMISCAMT~SGST~CGST~IGST~GST~IAMOUNT" & IIf(BILLTRAN_MARGIN, "~ACTUAL_MARGIN", ""))
                        .Width = gridBillWiseTransaction.Columns("IPCS").Width
                        .Width += IIf(gridBillWiseTransaction.Columns("IGRSWT").Visible = True, gridBillWiseTransaction.Columns("IGRSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("ISTNWT").Visible = True, gridBillWiseTransaction.Columns("ISTNWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("IDIAWT").Visible = True, gridBillWiseTransaction.Columns("IDIAWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("ISTNAMT").Visible = True, gridBillWiseTransaction.Columns("ISTNAMT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("IDIAAMT").Visible = True, gridBillWiseTransaction.Columns("IDIAAMT").Width, 0)
                        .Width += gridBillWiseTransaction.Columns("IAMOUNT").Width
                        If ChkWithTax.Checked Then
                            .Width += gridBillWiseTransaction.Columns("SGST").Width
                            .Width += gridBillWiseTransaction.Columns("CGST").Width
                            .Width += gridBillWiseTransaction.Columns("IGST").Width
                            .Width += gridBillWiseTransaction.Columns("GST").Width
                        End If
                        If ChkWithHSN.Checked Then
                            .Width += gridBillWiseTransaction.Columns("HSN").Width
                        End If
                        If ChkWithRunno.Checked Then
                            .Width += gridBillWiseTransaction.Columns("RUNNO").Width
                        End If
                        If BILLTRAN_MARGIN Then
                            .Width += gridBillWiseTransaction.Columns("ACTUAL_MARGIN").Width
                        End If
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .HeaderText = "SALES"
                    End With
                    With .Columns("RITEMNAME~RPCS~RGRSWT~RLESSWT~RNETWT~RSTNWT~RDIAWT~RPURITY~RWASTAGE~RMCHARGE~RSTNAMT~RDIAAMT~RAMOUNT~RSGST~RCGST~RIGST~RNETAMOUNT")
                        .Width = gridBillWiseTransaction.Columns("RPCS").Width
                        .Width += IIf(gridBillWiseTransaction.Columns("RGRSWT").Visible = True, gridBillWiseTransaction.Columns("RGRSWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RSTNWT").Visible = True, gridBillWiseTransaction.Columns("RSTNWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RDIAWT").Visible = True, gridBillWiseTransaction.Columns("RDIAWT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RSTNAMT").Visible = True, gridBillWiseTransaction.Columns("RSTNAMT").Width, 0)
                        .Width += IIf(gridBillWiseTransaction.Columns("RDIAAMT").Visible = True, gridBillWiseTransaction.Columns("RDIAAMT").Width, 0)
                        .Width += gridBillWiseTransaction.Columns("RAMOUNT").Width
                        .Width += gridBillWiseTransaction.Columns("RSGST").Width
                        .Width += gridBillWiseTransaction.Columns("RCGST").Width
                        .Width += gridBillWiseTransaction.Columns("RIGST").Width
                        .Width += gridBillWiseTransaction.Columns("RNETAMOUNT").Width
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                        .HeaderText = "PURCHASE/RETURN"
                    End With
                End If

            End If
            With .Columns("BALANCE")
                .Width = gridBillWiseTransaction.Columns("BALANCE").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "BALANCE"
            End With
            With .Columns("CASH~CARD~CHEQUE~ROUNDOFF~ADVANCE~HANDLOG~DISCOUNT~CHITCARD~GIFTVOUCHER~CREDIT~JND~TOTAL")
                .Width = gridBillWiseTransaction.Columns("CASH").Width + gridBillWiseTransaction.Columns("CARD").Width + gridBillWiseTransaction.Columns("CHEQUE").Width + gridBillWiseTransaction.Columns("ROUNDOFF").Width + gridBillWiseTransaction.Columns("ADVANCE").Width + gridBillWiseTransaction.Columns("HANDLOG").Width + gridBillWiseTransaction.Columns("DISCOUNT").Width + gridBillWiseTransaction.Columns("CHITCARD").Width + gridBillWiseTransaction.Columns("GIFTVOUCHER").Width + gridBillWiseTransaction.Columns("CREDIT").Width + gridBillWiseTransaction.Columns("JND").Width + gridBillWiseTransaction.Columns("TOTAL").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "PAYMENT"
            End With
            With .Columns("CUSTOMER~ADDRESS~PHONENO~PAN~GSTNO~EMPNAME~USERNAME~NARRATION~STATUS")
                .Width = gridBillWiseTransaction.Columns("CUSTOMER").Width
                .Width += gridBillWiseTransaction.Columns("ADDRESS").Width
                .Width += gridBillWiseTransaction.Columns("PHONENO").Width
                .Width += gridBillWiseTransaction.Columns("NARRATION").Width
                .Width += gridBillWiseTransaction.Columns("STATUS").Width
                If gridBillWiseTransaction.Columns.Contains("PAN") Then
                    .Width += gridBillWiseTransaction.Columns("PAN").Width
                End If
                If gridBillWiseTransaction.Columns.Contains("GSTNO") Then
                    .Width += gridBillWiseTransaction.Columns("GSTNO").Width
                End If
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            If BILLTRAN_MARGIN Then
                .Columns("BALANCE").Visible = False
                .Columns("CASH~CARD~CHEQUE~ROUNDOFF~ADVANCE~HANDLOG~DISCOUNT~CHITCARD~GIFTVOUCHER~CREDIT~JND~TOTAL").Visible = False
                .Columns("CUSTOMER~ADDRESS~PHONENO~GSTNO~EMPNAME~USERNAME~NARRATION~STATUS").Visible = False
            End If
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("verdAna", 8, FontStyle.Bold)
            .ColumnHeadersHeight = gridBillWiseTransaction.ColumnHeadersHeight
        End With
    End Function

    Private Sub gridBillWiseTransaction_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridBillWiseTransaction.ColumnWidthChanged
        If gridHeader.ColumnCount > 0 Then
            funcGridHeaderStyle()
            'gridHeader.Columns("BILLNO").Width = gridBillWiseTransaction.Columns("BILLNO").Width
            'gridHeader.Columns("TRANDATE").Width = gridBillWiseTransaction.Columns("TRANDATE").Width
            'With gridHeader
            '    If chkItemDetail.Checked = True Then
            '        With .Columns("IITEMNAME~ITAGNO~IITEMCTRNAME~IPCS~IGRSWT~INETWT~IADVWT~IBALWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~ISTNAMT~IMISCAMT~IAMOUNT")
            '            .Width = gridBillWiseTransaction.Columns("IITEMNAME").Width + gridBillWiseTransaction.Columns("ITAGNO").Width + gridBillWiseTransaction.Columns("IITEMCTRNAME").Width + gridBillWiseTransaction.Columns("IPCS").Width
            '            .Width += IIf(gridBillWiseTransaction.Columns("IGRSWT").Visible = True, gridBillWiseTransaction.Columns("IGRSWT").Width, 0)
            '            .Width += IIf(gridBillWiseTransaction.Columns("INETWT").Visible = True, gridBillWiseTransaction.Columns("INETWT").Width, 0)
            '            .Width += IIf(gridBillWiseTransaction.Columns("IADVWT").Visible = True, gridBillWiseTransaction.Columns("IADVWT").Width, 0)
            '            .Width += IIf(gridBillWiseTransaction.Columns("IBALWT").Visible = True, gridBillWiseTransaction.Columns("IBALWT").Width, 0)
            '            .Width += gridBillWiseTransaction.Columns("IMCHARGE").Width + gridBillWiseTransaction.Columns("IWASTPER").Width + gridBillWiseTransaction.Columns("IWASTAGE").Width + gridBillWiseTransaction.Columns("IRATE").Width + gridBillWiseTransaction.Columns("ISTNAMT").Width + gridBillWiseTransaction.Columns("IMISCAMT").Width + gridBillWiseTransaction.Columns("IAMOUNT").Width
            '        End With
            '        .Columns("RITEMNAME~RPCS~RGRSWT~RNETWT~RWASTAGE~RMCHARGE~RSTNAMT~RAMOUNT").Width = gridBillWiseTransaction.Columns("RITEMNAME").Width + gridBillWiseTransaction.Columns("RPCS").Width + gridBillWiseTransaction.Columns("RGRSWT").Width + gridBillWiseTransaction.Columns("RAMOUNT").Width
            '    Else
            '        .Columns("IITEMNAME~ITAGNO~IPCS~IGRSWT~INETWT~IWASTPER~IWASTAGE~IMCHARGE~IRATE~ISTNAMT~IMISCAMT~IAMOUNT").Width = gridBillWiseTransaction.Columns("IAMOUNT").Width
            '        .Columns("RITEMNAME~RPCS~RGRSWT~RNETWT~RWASTAGE~RMCHARGE~RSTNAMT~RAMOUNT").Width = gridBillWiseTransaction.Columns("RAMOUNT").Width
            '    End If
            '    .Columns("BALANCE").Width = gridBillWiseTransaction.Columns("BALANCE").Width
            '    .Columns("CASH~CARD~CHEQUE~ROUNDOFF~ADVANCE~HANDLOG~DISCOUNT~CHITCARD~GIFTVOUCHER~CREDIT~JND~TOTAL").Width = gridBillWiseTransaction.Columns("CASH").Width + gridBillWiseTransaction.Columns("CARD").Width + gridBillWiseTransaction.Columns("CHEQUE").Width + gridBillWiseTransaction.Columns("ROUNDOFF").Width + gridBillWiseTransaction.Columns("ADVANCE").Width + gridBillWiseTransaction.Columns("HANDLOG").Width + gridBillWiseTransaction.Columns("DISCOUNT").Width + gridBillWiseTransaction.Columns("CHITCARD").Width + gridBillWiseTransaction.Columns("GIFTVOUCHER").Width + gridBillWiseTransaction.Columns("CREDIT").Width + gridBillWiseTransaction.Columns("JND").Width + gridBillWiseTransaction.Columns("TOTAL").Width
            '    .Columns("SCROLL").Visible = CType(gridBillWiseTransaction.Controls(0), HScrollBar).Visible
            '    .Columns("SCROLL").Width = CType(gridBillWiseTransaction.Controls(1), VScrollBar).Width
            'End With
        End If
    End Sub

    Private Sub gridBillWiseTransaction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridBillWiseTransaction.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Private Sub gridBillWiseTransaction_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridBillWiseTransaction.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHeader.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub
    Function funcFilter() As String
        Dim strFilter As String = Nothing
        strFilter = " AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strFilter += " AND COSTID =(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & Replace(cmbCostCentre.Text, "'", "''") & "')"
        End If
        If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
            strFilter += " AND SYSTEMID ='" & Replace(txtNodeId.Text, "'", "''") & "'"
        End If
        Return strFilter
    End Function

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmBillWiseTransaction_Properties
        obj.p_txtNodeId = txtNodeId.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkItemDetail = chkItemDetail.Checked
        obj.p_chkWithCancel = chkCanelBill.Checked
        obj.P_ChkSummaryView = ChkSummaryView.Checked
        obj.P_ChkWithHsn = ChkWithHSN.Checked
        obj.P_ChkWithRunNo = ChkWithRunno.Checked
        obj.P_ChkWithTax = ChkWithTax.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmBillWiseTransaction_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmBillWiseTransaction_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmBillWiseTransaction_Properties))
        txtNodeId.Text = obj.p_txtNodeId
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, Nothing)
        chkItemDetail.Checked = obj.p_chkItemDetail
        chkCanelBill.Checked = obj.p_chkWithCancel
        ChkSummaryView.Checked = obj.P_ChkSummaryView
        ChkWithHSN.Checked = obj.P_ChkWithHsn
        ChkWithRunno.Checked = obj.P_ChkWithRunNo
        ChkWithTax.Checked = obj.P_ChkWithTax
        rbtGrswght.Checked = True
    End Sub

    Private Sub ReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReziseToolStripMenuItem.Click
        If gridBillWiseTransaction.RowCount > 0 Then
            If ReziseToolStripMenuItem.Checked Then
                gridBillWiseTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridBillWiseTransaction.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridBillWiseTransaction.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridBillWiseTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridBillWiseTransaction.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridBillWiseTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub chkItemDetail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemDetail.CheckedChanged
        If chkItemDetail.Checked = True Then
            ChkSummaryView.Checked = False
            If BILLTRAN_MARGIN Then
                ChkBillTotal.Checked = False
            End If
        End If
    End Sub

    Private Sub ChkSummaryView_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSummaryView.CheckedChanged
        If ChkSummaryView.Checked = True Then
            chkItemDetail.Checked = False
            ChkWithRunno.Visible = False
            ChkWithHSN.Visible = False
            ChkWithTax.Visible = False
            ChkWithRunno.Checked = False
            ChkWithHSN.Checked = False
            ChkWithTax.Checked = False
            ChkWithCatName.Checked = False
            ChkWithCatName.Visible = False
        Else
            ChkWithRunno.Visible = True
            ChkWithHSN.Visible = True
            ChkWithCatName.Visible = True
            ChkWithTax.Visible = True
        End If
    End Sub

    Private Sub ChkBillTotal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkBillTotal.CheckedChanged
        If ChkBillTotal.Checked Then
            If BILLTRAN_MARGIN Then
                ChkBillTotal.Checked = False
            End If
        End If
    End Sub

End Class

Public Class frmBillWiseTransaction_Properties
    Private txtNodeId As String = ""
    Public Property p_txtNodeId() As String
        Get
            Return txtNodeId
        End Get
        Set(ByVal value As String)
            txtNodeId = value
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
    Private chkItemDetail As Boolean = True
    Public Property p_chkItemDetail() As Boolean
        Get
            Return chkItemDetail
        End Get
        Set(ByVal value As Boolean)
            chkItemDetail = value
        End Set
    End Property
    Private chkWithCancel As Boolean = False
    Public Property p_chkWithCancel() As Boolean
        Get
            Return chkWithCancel
        End Get
        Set(ByVal value As Boolean)
            chkWithCancel = value
        End Set
    End Property
    Private ChkSummaryView As Boolean = False
    Public Property P_ChkSummaryView() As Boolean
        Get
            Return ChkSummaryView
        End Get
        Set(ByVal value As Boolean)
            ChkSummaryView = value
        End Set
    End Property
    Private ChkWithTax As Boolean = False
    Public Property P_ChkWithTax() As Boolean
        Get
            Return ChkWithTax
        End Get
        Set(ByVal value As Boolean)
            ChkWithTax = value
        End Set
    End Property
    Private ChkWithHsn As Boolean = False
    Public Property P_ChkWithHsn() As Boolean
        Get
            Return ChkWithHsn
        End Get
        Set(ByVal value As Boolean)
            ChkWithHsn = value
        End Set
    End Property
    Private ChkWithRunNo As Boolean = False
    Public Property P_ChkWithRunNo() As Boolean
        Get
            Return ChkWithRunNo
        End Get
        Set(ByVal value As Boolean)
            ChkWithRunNo = value
        End Set
    End Property
End Class