Imports System.Data.OleDb
Public Class frmGiftTransfer
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGridView As DataTable
    Dim defalutDestination As String
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim IS_IMAGE_TRF As Boolean = IIf(GetAdmindbSoftValue("STKTRANWITHIMAGE", "N") = "Y", True, False)
    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String

    Private Sub frmGiftTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not txtLotNo.Focused Then
                    SendKeys.Send("{TAB}")  
                Exit Sub
            End If
            If Not chkCheckByScan.Checked Then
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            If txtTagNo.Text = "" Then Exit Sub
            If txtLotNo.Text = "" Then Exit Sub
            If chkCheckByScan.Checked Then
                btnSearch_Click(Me, New EventArgs)
            End If
        End If
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If txtTagNo.Focused Then Exit Sub
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub GridStyle()

    End Sub

    Private Sub frmGiftTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtMetal As New DataTable
        Dim dtCounter As New DataTable
        defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        dtGridView = New DataTable
        dtGridView.Columns.Add("SNO", GetType(String))
        dtGridView.Columns.Add("TRANDATE", GetType(String))
        dtGridView.Columns.Add("RUNNO", GetType(String))
        dtGridView.Columns.Add("CARDID", GetType(Integer))
        dtGridView.Columns.Add("QTY", GetType(Integer))
        dtGridView.Columns.Add("AMOUNT", GetType(Double))
        dtGridView.Columns.Add("COSTID", GetType(String))
        dtGridView.Columns.Add("TCOSTID", GetType(String))
        dtGridView.Columns.Add("DUEDATE", GetType(String))
        dtGridView.Columns.Add("BATCHNO", GetType(String))

        dtGridView.Columns.Add("COLHEAD", GetType(String))
        'dtGridView.Columns.Add("ROID", GetType(Integer))
        'dtGridView.Columns("ROID").AutoIncrement = True
        'dtGridView.Columns("ROID").AutoIncrementSeed = 1
        'dtGridView.Columns("ROID").AutoIncrementStep = 1
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            '.Columns("ROID").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        cmbCostCentre_MAN.Items.Clear()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "' "
        strSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
        strSql += " ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)

        '' LOAD METAL
        strSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        'For Each Row As DataRow In dtMetal.Rows
        '    cmbMetal.Items.Add(Row.Item("METALNAME").ToString)
        'Next

        strSql = vbCrLf + " SELECT 'ALL' COUNTERNAME,'ALL' COUNTERID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMCTRNAME as COUNTERNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY RESULT,COUNTERNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        cmbCounter_OWN.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCounter_OWN, dtCounter, "COUNTERNAME", , "ALL")
        'For Each Row As DataRow In dtCounter.Rows
        '    cmbCounter_OWN.Items.Add(Row.Item("COUNTERNAME").ToString)
        'Next
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtGridView.Rows.Clear()
        cmbCounter_OWN.Text = ""
        'chkCheckByScan.Checked = False
        chkRecDate.Checked = True
        chkRecDate.Checked = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtGridView.Rows.Clear()
        cmbCostCentre_MAN.Enabled = True

        'strSql = " SELECT SNO"
        'strSql += vbCrLf + " ,TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
        'strSql += vbCrLf + " WHERE 1=0"
        'Dim dtTemp As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtTemp)
        dtGridView.Clear()
        AddGrandTotal()
        cmbCostCentre_MAN.Focus()
        Prop_Gets()
    End Sub

    Private Sub AddGrandTotal()
        For Each _dr As DataRow In dtGridView.Select("TRANDATE='GRAND TOTAL'")
            dtGridView.Rows.Remove(_dr)
            dtGridView.AcceptChanges()
        Next
        Dim Ro As DataRow = Nothing
        Dim QTY As Integer = Val(dtGridView.Compute("SUM(QTY)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim AMOUNT As Decimal = Val(dtGridView.Compute("SUM(AMOUNT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        'If dtGridView.Rows.Count > 0 Then
        '    Ro = dtGridView.Select("COLHEAD = 'G'")(0)
        'Else
        '    Ro = dtGridView.NewRow
        'End If
        Ro = dtGridView.NewRow
        Ro.Item("TRANDATE") = "GRAND TOTAL"
        Ro.Item("QTY") = IIf(QTY <> 0, QTY, DBNull.Value)
        Ro.Item("AMOUNT") = IIf(AMOUNT <> 0, Format(AMOUNT, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then
            dtGridView.Rows.Add(Ro)
        End If
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        If gridView.Rows.Count > 0 Then gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not chkCheckByScan.Checked Then
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            If txtTagNo.Text = "" Then Exit Sub
            If chkCheckByScan.Checked Then
                btnSearch_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub chkRecDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRecDate.CheckedChanged
        dtpDate.Enabled = chkRecDate.Checked
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        AddGrandTotal()
    End Sub

    Private Sub CreateInsertGenrator()
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'InsertGenerator')>0 DROP PROC InsertGenerator"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "  create PROC InsertGenerator    "
        strSql += "  (@tableName varchar(100)) as    "
        strSql += "  DECLARE @query nvarchar(4000) -- provide for the whole query,     "
        strSql += "                                -- you may increase the size    "
        strSql += "  DECLARE cursCol CURSOR FAST_FORWARD FOR    "
        strSql += "  select b.name, c.name    "
        strSql += "  from sysobjects a, syscolumns b, systypes c    "
        strSql += "  where a.id = b.id and b.xtype = c.xtype and    "
        strSql += "    a.name = @tableName    "
        strSql += "       "
        strSql += "  OPEN cursCol    "
        strSql += "  DECLARE @string nvarchar(3000) --for storing the first half     "
        strSql += "                                 --of INSERT statement    "
        strSql += "  DECLARE @stringData nvarchar(3000) --for storing the data     "
        strSql += "                                     --(VALUES) related statement    "
        strSql += "  DECLARE @dataType nvarchar(1000) --data types returned     "
        strSql += "                                   --for respective columns    "
        strSql += "  SET @string='INSERT '+@tableName+'('    "
        strSql += "  SET @stringData=''    "
        strSql += "       "
        strSql += "  DECLARE @colName nvarchar(50)    "
        strSql += "       "
        strSql += "  FETCH NEXT FROM cursCol INTO @colName,@dataType    "
        strSql += "       "
        strSql += "  IF @@fetch_status<>0    "
        strSql += "      begin    "
        strSql += "      print 'Table '+@tableName+' not found, processing skipped.'    "
        strSql += "      close curscol    "
        strSql += "      deallocate curscol    "
        strSql += "      return    "
        strSql += "  END    "
        strSql += "  WHILE @@FETCH_STATUS=0    "
        strSql += "  BEGIN    "
        strSql += "  IF @dataType in ('varchar','char','nchar','nvarchar')    "
        strSql += "   BEGIN    "
        strSql += "       SET @stringData=@stringData+'''''''''+    "
        strSql += "               isnull('+@colName+','''')+'''''',''+'    "
        strSql += "   END    "
        strSql += "  ELSE    "
        strSql += "   if @dataType in ('text','ntext') --if the datatype  --is text or something else     "
        strSql += "    BEGIN    "
        strSql += "        SET @stringData=@stringData+'''''''''+    "
        strSql += "              isnull(cast('+@colName+' as varchar(2000)),'''')+'''''',''+'    "
        strSql += "    END    "
        strSql += "   ELSE    "
        strSql += "    IF @dataType = 'money' --because money doesn't get converted  --from varchar implicitly    "
        strSql += "     BEGIN    "
        strSql += "         SET @stringData=@stringData+'''convert(money,''''''+    "
        strSql += "             isnull(cast('+@colName+' as varchar(200)),''0.0000'')+''''''),''+'    "
        strSql += "     END    "
        strSql += "    ELSE     "
        strSql += "     IF @dataType='datetime'    "
        strSql += "      BEGIN    "
        strSql += "          SET @stringData=@stringData+'''convert(datetime,''''''+    "
        strSql += "              isnull(cast('+@colName+' as varchar(200)),''0'')+''''''),''+'    "
        strSql += "      END    "
        strSql += "     ELSE     "
        strSql += "      IF @dataType='image'     "
        strSql += "       BEGIN    "
        strSql += "           SET @stringData=@stringData+'''''''''+    "
        strSql += "              isnull(cast(convert(varbinary,'+@colName+')     "
        strSql += "               as varchar(6)),''0'')+'''''',''+'    "
        strSql += "       END    "
        strSql += "      ELSE --presuming the data type is int,bit,numeric,decimal     "
        strSql += "       BEGIN    "
        strSql += "           SET @stringData=@stringData+'''''''''+    "
        strSql += "                 isnull(cast('+@colName+' as varchar(200)),''0'')+'''''',''+'    "
        strSql += "       END    "
        strSql += "       "
        strSql += "  SET @string=@string+@colName+','    "
        strSql += "       "
        strSql += "  FETCH NEXT FROM cursCol INTO @colName,@dataType    "
        strSql += "  END     "
        strSql += "       "
        strSql += "  SET @query ='SELECT '''+substring(@string,0,len(@string)) + ')     "
        strSql += "      VALUES(''+ ' + substring(@stringData,0,len(@stringData)-2)+'''+'')''     "
        strSql += "      FROM '+@tableName    "
        strSql += "  exec sp_executesql @query --load and run the built query    "
        strSql += "  CLOSE cursCol    "
        strSql += "  DEALLOCATE cursCol    "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub CreateInternalTransfer(ByVal TransSnos As String, ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date)
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE MASTER..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS ITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS SUBITEMID"
        strSql += vbCrLf + " ,'G' GRSNET,'" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,T.PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
        strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID"
        strSql += vbCrLf + " ,53 ORDSTATE_ID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO MASTER..TEMP_" & systemId & "TRANTRANS FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID"
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
        strSql += vbCrLf + " ,TIM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,TIM.METALID AS TMETALID"
        strSql += vbCrLf + " INTO MASTER..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
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
            Ro.Item("UPTIME") = GetServerTime(tran)
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

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If objGPack.Validator_Check(Me) Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim toCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
        If toCostId = "" Then
            MsgBox("Cost centre should not empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MessageBox.Show("Sure, You want to transfer the loaded Gifts?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Dim obj As TrasitIssRec
        Dim RefNo As String
        Dim TransSno As String = ""
        BillDate = GetEntryDate(GetServerDate)
        Try
            '            tran = Nothing
            '            tran = cn.BeginTransaction
            '            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            '            strSql += " DROP TABLE TEMP" & systemId & "BILLNO"
            '            cmd = New OleDbCommand(strSql, cn, tran)
            '            cmd.ExecuteNonQuery()

            '            Dim NEWBILLNO As Integer
            '            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            '            NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran))

            'GenerateNewBillNo:
            '            RefNo = cnCostId & NEWBILLNO.ToString
            '            strSql = "SELECT TOP 1 SNO FROM " & cnStockDb & "..GVTRAN WHERE CONVERT(VARCHAR,RUNNO) = '" & RefNo & "'"
            '            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            '                NEWBILLNO = NEWBILLNO + 1
            '                GoTo GenerateNewBillNo
            '            End If
            '            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NEWBILLNO + 1).ToString & "'"
            '            strSql += " WHERE CTLID ='GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            '            'strSql += " AND CONVERT(INT,CTLTEXT) = '" & NEWBILLNO & "'"
            '            cmd = New OleDbCommand(strSql, cn, tran)
            '            If cmd.ExecuteNonQuery() = 0 Then
            '                GoTo GenerateNewBillNo
            '            End If

            '            For Each ro As DataRow In dtGridView.Rows
            '                obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, cn, tran, RefNo)
            '                If Not obj.InsertTagGift(True) Then
            '                    Continue For
            '                End If
            '                TransSno += "'" & ro.Item("SNO").ToString & "',"
            '            Next
            '            tran.Commit()
            '            tran = Nothing

            
            strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            Dim NEWBILLNO As Integer
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            NEWBILLNO = Val(objGPack.GetSqlValue(strSql))

GenerateNewBillNo:
            RefNo = cnCostId & NEWBILLNO.ToString
            strSql = "SELECT TOP 1 SNO FROM " & cnStockDb & "..GVTRAN WHERE CONVERT(VARCHAR,RUNNO) = '" & RefNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1") <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenerateNewBillNo
            End If
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (NEWBILLNO + 1).ToString & "'"
            strSql += " WHERE CTLID ='GEN-STKREFNO' AND COMPANYID = '" & strCompanyId & "'"
            'strSql += " AND CONVERT(INT,CTLTEXT) = '" & NEWBILLNO & "'"
            cmd = New OleDbCommand(strSql, cn)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GenerateNewBillNo
            End If

            For Each ro As DataRow In dtGridView.Rows
                obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, cn, Nothing, RefNo)
                If Not obj.InsertTagGift(True) Then
                    Continue For
                End If
                TransSno += "'" & ro.Item("SNO").ToString & "',"
                strSql = "UPDATE " & cnStockDb & "..GVTRAN SET TOFLAG = 'TR'"
                strSql += " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            Next

            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate
            btnNew_Click(Me, New EventArgs)
            MsgBox("Data Transfered,Please do the Syncronization")
            
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


    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Insert Then

        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer", gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer", gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
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

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selMetalId As String = Nothing
        Dim selCounterId As String = Nothing
        If objGPack.Validator_Check(Me) Then Exit Sub
        Dim _RunNo As String = txtLotNo.Text.ToString
        _RunNo = _RunNo.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
        _RunNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & _RunNo

        strSql = " SELECT SNO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
        strSql += vbCrLf + " WHERE 1=1"
        If dtpDate.Enabled Then strSql += vbcrlf + "  AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        If txtTagNo.Text <> "" Then
            strSql += vbCrLf + " AND RUNNO = '" & txtTagNo.Text & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(TOFLAG,'')=''"
        'strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
        strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbGiftsCostName_MAN.Text & "')"
        If txtLotNo.Text.ToString <> "" Then strSql += vbCrLf + " AND RUNNO='" & _RunNo & "'"
        If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And txtTagNo.Text = "" And Val(txtLotNo.Text) = 0 Then
            If MessageBox.Show("Do you want to load all GIFT items?", "Gift Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If

        Dim dtTemp As New DataTable
        'dtGridView.Clear()
        gridView.DataSource = Nothing
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)

        If Not dtTemp.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            txtTagNo.Focus()
            If chkCheckByScan.Checked Then
                txtTagNo.Focus()
            End If
            If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
            Exit Sub
        ElseIf dtTemp.Rows.Count > 0 Then
            txtTagNo.Focus()
        End If
        For Each ro As DataRow In dtTemp.Rows
            Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
            If RowCol.Length = 0 Then
                dtGridView.ImportRow(ro)
            End If
        Next
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        AddGrandTotal()
        cmbCostCentre_MAN.Enabled = False
        If chkCheckByScan.Checked Then
            txtTagNo.Clear()
            If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
        End If
    End Sub

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus
        cmbGiftsCostName_MAN.Items.Clear()
        cmbGiftsCostName_MAN.Items.Add(cmbCostCentre_MAN.Text)
        cmbGiftsCostName_MAN.Items.Add(cnCostName)
        cmbGiftsCostName_MAN.Text = cmbCostCentre_MAN.Text
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
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
    Private Sub Prop_Sets()
        Dim obj As New frmGiftTransfer_Properties
        obj.p_cmbCounter = cmbCounter_OWN.Text
        SetSettingsObj(obj, Me.Name, GetType(frmGiftTransfer_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGiftTransfer_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGiftTransfer_Properties))
        cmbCounter_OWN.Text = obj.p_cmbCounter
    End Sub

    Private Sub txtLotNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim selMetalId As String = Nothing
            Dim selCounterId As String = Nothing
            If objGPack.Validator_Check(Me) Then Exit Sub
            Dim _RunNo As String = txtLotNo.Text.ToString
            _RunNo = _RunNo.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
            _RunNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & _RunNo

            strSql = " SELECT SNO"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,RUNNO,CARDID,QTY,AMOUNT,BATCHNO,DUEDAYS,COSTID,TCOSTID,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1) AS RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..GVTRAN T"
            strSql += vbCrLf + " WHERE 1=1"
            If dtpDate.Enabled Then strSql += vbCrLf + "  AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            If txtTagNo.Text <> "" Then
                strSql += vbCrLf + " AND RUNNO = '" & txtTagNo.Text & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(TOFLAG,'')=''"
            'strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbGiftsCostName_MAN.Text & "')"
            If txtLotNo.Text.ToString <> "" Then strSql += vbCrLf + " AND RUNNO='" & _RunNo & "'"
            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And txtTagNo.Text = "" And Val(txtLotNo.Text) = 0 Then
                If MessageBox.Show("Do you want to load all GIFT items?", "Gift Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If

            Dim dtTemp As New DataTable
            'dtGridView.Clear()
            gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)

            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                txtTagNo.Focus()
                If chkCheckByScan.Checked Then
                    txtTagNo.Focus()
                End If
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                Exit Sub
            ElseIf dtTemp.Rows.Count > 0 Then
                txtTagNo.Focus()
            End If
            For Each ro As DataRow In dtTemp.Rows
                Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
                If RowCol.Length = 0 Then
                    dtGridView.ImportRow(ro)
                End If
            Next
            dtGridView.AcceptChanges()
            gridView.DataSource = dtGridView
            With gridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            AddGrandTotal()
            cmbCostCentre_MAN.Enabled = False
            If chkCheckByScan.Checked Then
                txtTagNo.Clear()
                txtLotNo.Clear()
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
            End If
        End If
    End Sub
 
    Private Sub btnMiscIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMiscIssue.Click
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each ro As DataRow In dtGridView.Rows
            strSql = "UPDATE " & cnStockDb & "..GVTRAN SET TOFLAG = 'MI'"
            strSql += " WHERE SNO='" & ro.Item("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        Next
        btnNew_Click(Me, New EventArgs)
        MsgBox("Misc Issue Updated...", MsgBoxStyle.Information)
    End Sub
End Class
Public Class frmGiftTransfer_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCounter As String = "ALL"
    Public Property p_cmbCounter() As String
        Get
            Return cmbCounter
        End Get
        Set(ByVal value As String)
            cmbCounter = value
        End Set
    End Property
End Class
