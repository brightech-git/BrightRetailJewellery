Imports System.Data.OleDb
Public Class frmStuddedAmtRateUpdate
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
    Dim TranCompanyid As String = Nothing
    Dim CompanyState As String = Nothing
    Dim CustomerState As String = Nothing
    Dim GSTFLAG As Boolean = IIf(GetAdmindbSoftValue("GST", "Y") = "Y", True, False)
    Dim GSTDATE As Date = GetAdmindbSoftValue("GSTDATE")
    Dim StuddedCatCodes As String
    Dim MetalIds As String = Nothing
    Dim dtTemp As New DataTable
#End Region

#Region "User Defined Function"

    Private Sub Funcnew()
        frmcatcode = Nothing
        tocatcode = Nothing
        selectedcompid = Nothing
        selectedcostid = Nothing
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        txtTranno.Text = ""
        btnUpdate.Enabled = True
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
        txtMcPerGrm.Text = "750"
        StuddedCatCodes = GetAdmindbSoftValue("UPDT_STUDDED_CATCODE")
        chkCmbCompany.Select()
        chkCmbCompany.Focus()
    End Sub
#End Region

#Region "Events"

    Private Sub frmStuddedAmtRateUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmStuddedAmtRateUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Funcnew()
    End Sub


    Private Sub btnTransfer_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize) Then Exit Sub
        Try
            Dim SelectedCompanyName As String
            Dim SelectedCostname As String
            btnUpdate.Enabled = False
            StuddedCatCodes = Replace(StuddedCatCodes, ",", "','")
            SelectedCompanyName = Replace(chkCmbCompany.Text, ",", "','")
            SelectedCostname = Replace(chkCmbCostCentre.Text, ",", "','")

            strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSUE"
            strSql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSSTONE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSSTONE"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPISSDIA','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSDIA"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCOMPANY','U') > 0 DROP TABLE TEMPTABLEDB..TEMPCOMPANY"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCOSTCENTRE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPCOSTCENTRE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT COMPANYID INTO TEMPTABLEDB..TEMPCOMPANY FROM " & cnAdminDb & "..COMPANY "
            If selectedcompid <> "" And selectedcompid <> "ALL" Then strSql += vbCrLf + " Then WHERE CompanyName IN ('" & SelectedCompanyName & "')"
            strSql += vbCrLf + " SELECT COSTID INTO TEMPTABLEDB..TEMPCOSTCENTRE FROM " & cnAdminDb & "..COSTCENTRE "
            If selectedcompid <> "" And selectedcompid <> "ALL" Then strSql += vbCrLf + " WHERE COSTCE IN ('" & SelectedCompanyName & "')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            If StuddedCatCodes = "" Then MsgBox("Check SoftControl UPDT_STUDDED_CATCODE") : Exit Sub
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = vbCrLf + "/*STEP1*/"
            strSql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSUE"
            strSql += vbCrLf + "SELECT SNO,TRANNO,TRANDATE, TRANTYPE,PCS,GRSWT,NETWT,LESSWT,TAGNO,ITEMID,SUBITEMID"
            strSql += vbCrLf + ",WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT,RATE,BOARDRATE ,SALEMODE ,GRSNET, CATCODE ,BATCHNO,TAX,STNAMT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),CASE WHEN GRSNET='G' THEN GRSWT ELSE NETWT END*" & Val(txtMcPerGrm.Text) & " ) NEW_MCHARGE"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),CASE WHEN GRSNET='G' THEN GRSWT ELSE NETWT END*RATE) METALRATE"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL)STNAMOUNT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL)DIAAMOUNT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)DIARATE"
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPISSUE "
            strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "AND TRANTYPE = 'SA'"
            strSql += vbCrLf + "AND ISNULL(TAGNO,'')<>''"
            strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "AND COMPANYID IN (SELECT COMPANYID FROM TEMPTABLEDB..TEMPCOMPANY)"
            strSql += vbCrLf + "AND COSTID IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)"
            strSql += vbCrLf + "AND ITEMID IN "
            strSql += vbCrLf + "(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN ('" & StuddedCatCodes & "'))"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP2*/"
            strSql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSSTONE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSSTONE"
            strSql += vbCrLf + "SELECT "
            strSql += vbCrLf + " SNO"
            strSql += vbCrLf + ",ISSSNO,TRANNO, TRANDATE, TRANTYPE,STNPCS, STNWT,STNRATE,STNAMT, STNITEMID,CATCODE "
            strSql += vbCrLf + ",STONEUNIT "
            strSql += vbCrLf + ",(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO = I.ISSSNO) ITEMID"
            strSql += vbCrLf + ",(SELECT TAGNO  FROM " & cnStockDb & "..ISSUE WHERE SNO = I.ISSSNO) TAGNO"
            strSql += vbCrLf + ",(SELECT ISNULL(PURAMT,0) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE ITEMID=(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)"
            strSql += vbCrLf + "  AND TAGNO=(SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO) AND STNITEMID=I.STNITEMID AND STNWT=I.STNWT"
            strSql += vbCrLf + "  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T')"
            strSql += vbCrLf + ")PUR_STNAMT"
            strSql += vbCrLf + ",(SELECT ISNULL(PURRATE,0) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE ITEMID=(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)"
            strSql += vbCrLf + "  AND TAGNO=(SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO) AND STNITEMID=I.STNITEMID AND STNWT=I.STNWT"
            strSql += vbCrLf + "  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T')"
            strSql += vbCrLf + ")PUR_STNRATE"
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPISSSTONE "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMPISSUE)"
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T')"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " /*STEP3*/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPISSDIA','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSDIA"
            strSql += vbCrLf + "SELECT "
            strSql += vbCrLf + " SNO"
            strSql += vbCrLf + ",ISSSNO,TRANNO, TRANDATE, TRANTYPE,STNPCS, STNWT,STNRATE,STNAMT, STNITEMID,CATCODE "
            strSql += vbCrLf + ",(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO = I.ISSSNO) ITEMID"
            strSql += vbCrLf + ",(SELECT TAGNO  FROM " & cnStockDb & "..ISSUE WHERE SNO = I.ISSSNO) TAGNO"
            strSql += vbCrLf + ",STONEUNIT "
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL)DIAAMOUNT"
            strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL)DIARATE"
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPISSDIA "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMPISSUE)"
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D')"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " /*STEP4*/"
            strSql += vbCrLf + " IF (SELECT COUNT(*)FROM TEMPTABLEDB..TEMPISSSTONE ) > 0  "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + "	 DECLARE @STNAMOUNT AS NUMERIC(15,3)"
            strSql += vbCrLf + "	 DECLARE @ISSSNO AS VARCHAR(20)"
            strSql += vbCrLf + "	 DECLARE CUR CURSOR FOR SELECT ISSSNO ,SUM(ISNULL(PUR_STNAMT,0)) STNAMOUNT FROM TEMPTABLEDB..TEMPISSSTONE  GROUP BY ISSSNO "
            strSql += vbCrLf + "	 OPEN CUR"
            strSql += vbCrLf + "		 FETCH NEXT FROM CUR INTO @ISSSNO,@STNAMOUNT "
            strSql += vbCrLf + "		 WHILE @@FETCH_STATUS <> -1"
            strSql += vbCrLf + "		 BEGIN"
            strSql += vbCrLf + "			IF ISNULL(@STNAMOUNT,0) > 0"
            strSql += vbCrLf + "			BEGIN"
            strSql += vbCrLf + "				UPDATE TEMPTABLEDB..TEMPISSUE SET STNAMOUNT = @STNAMOUNT WHERE SNO = @ISSSNO "
            strSql += vbCrLf + "			END"
            strSql += vbCrLf + "			FETCH NEXT FROM CUR INTO @ISSSNO,@STNAMOUNT 	"
            strSql += vbCrLf + "		 END"
            strSql += vbCrLf + "	 CLOSE CUR"
            strSql += vbCrLf + "	 DEALLOCATE CUR"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "/*STEP5*/"
            strSql += vbCrLf + "UPDATE  TEMPTABLEDB..TEMPISSUE  SET DIAAMOUNT = ISNULL(AMOUNT,0)-(ISNULL(METALRATE,0)+ISNULL(NEW_MCHARGE,0)+ISNULL(STNAMOUNT,0) )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP6*/"
            strSql += vbCrLf + " IF (SELECT COUNT(*)FROM TEMPTABLEDB..TEMPISSDIA ) > 0  "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + "	 DECLARE @DIAWT AS NUMERIC(15,3)"
            strSql += vbCrLf + "	 DECLARE @ISSSNO AS VARCHAR(20)"
            strSql += vbCrLf + "	 DECLARE CUR CURSOR FOR SELECT ISSSNO ,SUM(ISNULL(STNWT,0)) DIAWT FROM TEMPTABLEDB..TEMPISSDIA GROUP BY ISSSNO "
            strSql += vbCrLf + "	 OPEN CUR"
            strSql += vbCrLf + "		 FETCH NEXT FROM CUR INTO @ISSSNO,@DIAWT "
            strSql += vbCrLf + "		 WHILE @@FETCH_STATUS <> -1"
            strSql += vbCrLf + "		 BEGIN"
            strSql += vbCrLf + "			IF ISNULL(@DIAWT,0) > 0 "
            strSql += vbCrLf + "			BEGIN"
            strSql += vbCrLf + "				UPDATE TEMPTABLEDB..TEMPISSUE SET DIARATE  = ISNULL(DIAAMOUNT,0)/CASE WHEN @DIAWT > 0 THEN @DIAWT ELSE 1 END  WHERE SNO = @ISSSNO "
            strSql += vbCrLf + "			END"
            strSql += vbCrLf + "			FETCH NEXT FROM CUR INTO @ISSSNO,@DIAWT 	"
            strSql += vbCrLf + "		 END"
            strSql += vbCrLf + "	 CLOSE CUR"
            strSql += vbCrLf + "	 DEALLOCATE CUR"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " /*STEP7*/"
            strSql += vbCrLf + " IF (SELECT COUNT(*)FROM TEMPTABLEDB..TEMPISSDIA  ) > 0  "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + "	 DECLARE @DIARATE AS NUMERIC(15,3)"
            strSql += vbCrLf + "	 DECLARE @SNO AS VARCHAR(20)"
            strSql += vbCrLf + "	 DECLARE CUR CURSOR FOR SELECT SNO,DIARATE  FROM TEMPTABLEDB..TEMPISSUE  "
            strSql += vbCrLf + "	 OPEN CUR"
            strSql += vbCrLf + "		 FETCH NEXT FROM CUR INTO @SNO,@DIARATE"
            strSql += vbCrLf + "		 WHILE @@FETCH_STATUS <> -1"
            strSql += vbCrLf + "		 BEGIN"
            strSql += vbCrLf + "			IF ISNULL(@DIARATE,0) > 0 "
            strSql += vbCrLf + "			BEGIN"
            strSql += vbCrLf + "				UPDATE TEMPTABLEDB..TEMPISSDIA SET DIAAMOUNT  = STNWT*ISNULL(@DIARATE,0),DIARATE = @DIARATE   WHERE ISSSNO  = @SNO "
            strSql += vbCrLf + "			END"
            strSql += vbCrLf + "			FETCH NEXT FROM CUR INTO @SNO,@DIARATE"
            strSql += vbCrLf + "		 END"
            strSql += vbCrLf + "	 CLOSE CUR"
            strSql += vbCrLf + "	 DEALLOCATE CUR"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP8*/"
            strSql += vbCrLf + "UPDATE I SET I.MCHARGE = T.NEW_MCHARGE  FROM " & cnStockDb & "..ISSUE AS I, TEMPTABLEDB..TEMPISSUE AS T WHERE I.SNO = T.SNO "
            strSql += vbCrLf + "UPDATE I SET I.MCGRM = '" & Val(txtMcPerGrm.Text) & "'  FROM " & cnStockDb & "..ISSUE AS I, TEMPTABLEDB..TEMPISSUE AS T WHERE I.SNO = T.SNO "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP9*/"
            strSql += vbCrLf + "UPDATE I SET I.STNAMT = T.DIAAMOUNT  FROM " & cnStockDb & "..ISSSTONE AS I, TEMPTABLEDB..TEMPISSDIA AS T WHERE I.SNO = T.SNO "
            strSql += vbCrLf + "UPDATE I SET I.STNRATE = T.DIARATE  FROM " & cnStockDb & "..ISSSTONE AS I, TEMPTABLEDB..TEMPISSDIA AS T WHERE I.SNO = T.SNO "
            strSql += vbCrLf + "UPDATE I SET I.STNAMT = T.PUR_STNAMT  FROM " & cnStockDb & "..ISSSTONE AS I, TEMPTABLEDB..TEMPISSSTONE AS T WHERE I.SNO = T.SNO "
            strSql += vbCrLf + "UPDATE I SET I.STNRATE = T.PUR_STNRATE  FROM " & cnStockDb & "..ISSSTONE AS I, TEMPTABLEDB..TEMPISSSTONE AS T WHERE I.SNO = T.SNO "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP10*/"
            strSql += vbCrLf + "UPDATE I SET I.STNAMT = T.STNAMT  FROM " & cnStockDb & "..ISSUE AS I, TEMPTABLEDB..TEMPISSUE AS T WHERE I.SNO = T.SNO "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "/*STEP11*/"
            strSql = vbCrLf + "UPDATE I SET I.STNAMT = (SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO) FROM " & cnStockDb & "..ISSUE I WHERE SNO  IN (SELECT DISTINCT SNO FROM TEMPTABLEDB..TEMPISSUE)"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSUE"
            strSql += vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPISSSTONE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSSTONE"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPISSDIA','U') > 0 DROP TABLE TEMPTABLEDB..TEMPISSDIA"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCOMPANY','U') > 0 DROP TABLE TEMPTABLEDB..TEMPCOMPANY"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCOSTCENTRE','U') > 0 DROP TABLE TEMPTABLEDB..TEMPCOSTCENTRE"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            MsgBox("Updated", MsgBoxStyle.OkOnly)
            Funcnew()
        Catch ex As Exception
            tran.Rollback()
            tran = Nothing
            MsgBox(ex.Message)
            Funcnew()
        Finally
            btnUpdate.Enabled = True
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
#End Region

End Class
