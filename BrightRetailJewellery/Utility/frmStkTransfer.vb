Imports System.Data.OleDb
Public Class frmStkTransfer
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGridView As DataTable
    Dim defalutDestination As String
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim IS_IMAGE_TRF As Boolean = IIf(GetAdmindbSoftValue("STKTRANWITHIMAGE", "N") = "Y", True, False)
    Dim Isbulkupdate As Boolean = IIf(GetAdmindbSoftValue("BULKTAGTRANSFER", "N") = "Y", True, False)
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String
    Dim AUTOBOOKVALUEG_PER As Decimal
    Dim AUTOBOOKVALUES_PER As Decimal
    Dim AUTOBOOKVALUEP_PER As Decimal
    Dim AUTOBOOKVALUED_PER As Decimal
    Dim AUTOBOOKVALUET_PER As Decimal

    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String

    Private Sub frmTagTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            If txtNetWt_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub GridStyle()

    End Sub

    Private Sub frmTagTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If AUTOBOOKTRAN = True Then
            If AUTOBOOKVALUEARRY.Length < 6 Then MsgBox("Please Reset the value <AUTOBOOKVALUE> ex(N,0,0,0,0,0,0)") : Me.Close()
            AUTOBOOKVALUEENABLE = AUTOBOOKVALUEARRY(0).ToString
            AUTOBOOKVALUEG_PER = Val(AUTOBOOKVALUEARRY(1).ToString)
            AUTOBOOKVALUES_PER = Val(AUTOBOOKVALUEARRY(2).ToString)
            AUTOBOOKVALUEP_PER = Val(AUTOBOOKVALUEARRY(3).ToString)
            AUTOBOOKVALUED_PER = Val(AUTOBOOKVALUEARRY(4).ToString)
            AUTOBOOKVALUET_PER = Val(AUTOBOOKVALUEARRY(5).ToString)
        End If
        Dim dtMetal As New DataTable
        Dim dtCounter As New DataTable
        defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        dtGridView = New DataTable
        dtGridView.Columns.Add("SNO", GetType(String))
        dtGridView.Columns.Add("COUNTER", GetType(String))
        dtGridView.Columns.Add("ITEM", GetType(String))
        dtGridView.Columns.Add("SUBITEM", GetType(String))
        dtGridView.Columns.Add("TAGNO", GetType(String))
        dtGridView.Columns.Add("PCS", GetType(Integer))
        dtGridView.Columns.Add("GRSWT", GetType(Decimal))
        dtGridView.Columns.Add("LESSWT", GetType(Decimal))
        dtGridView.Columns.Add("NETWT", GetType(Decimal))
        dtGridView.Columns.Add("STNPCS", GetType(Integer))
        dtGridView.Columns.Add("STNWT", GetType(Decimal))
        dtGridView.Columns.Add("DIAPCS", GetType(Integer))
        dtGridView.Columns.Add("DIAWT", GetType(Decimal))
        dtGridView.Columns.Add("SALVALUE", GetType(Double))
        dtGridView.Columns.Add("COLHEAD", GetType(String))
        dtGridView.Columns.Add("PCTFILE", GetType(String))
        dtGridView.Columns.Add("STOCKMODE", GetType(String))
        'dtGridView.Columns("ROID").AutoIncrement = True
        'dtGridView.Columns("ROID").AutoIncrementSeed = 1
        'dtGridView.Columns("ROID").AutoIncrementStep = 1
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("STNWT").DefaultCellStyle.Format = "0.000"
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("STOCKMODE").Visible = False
            '.Columns("ROID").Visible = False
            .Columns("PCTFILE").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        cmbCostCentre_MAN.Items.Clear()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "' ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)

        '' LOAD METAL
        strSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")
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

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_OWN)

        'For Each Row As DataRow In dtCounter.Rows
        '    cmbCounter_OWN.Items.Add(Row.Item("COUNTERNAME").ToString)
        'Next
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtGridView.Rows.Clear()
        cmbCounter_OWN.Text = ""
        strSql = vbCrLf + " SELECT NAME FROM " & cnStockDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnStockDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        chkCheckByScan.Checked = False
        chkRecDate.Checked = True
        chkRecDate.Checked = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtGridView.Rows.Clear()
        dtGridView.Clear()
        grpNonTag.Visible = False
        cmbCostCentre_MAN.Enabled = True
        AddGrandTotal()
        cmbCostCentre_MAN.Focus()
        Prop_Gets()
    End Sub

    Private Sub GetStock(ByVal cmbCost As ComboBox, Optional ByVal msubitemid As Integer = Nothing)
        objGPack.TextClear(grpNonTag)
        strSql = " SELECT "
        strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG"
        strSql += " WHERE ITEMID = " & Val(txtItemId.Text)
        If msubitemid <> Nothing Then strSql += "  AND SUBITEMID = " & msubitemid
        'strSql += " AND ITEMCTRID = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'),0)"
        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'),'')"
        strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then Exit Sub
        With dt.Rows(0)
            txtOldPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
            txtOldGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
            txtOldNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            txtPcs_NUM.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
            txtGrsWt_WET.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
            txtNetWt_WET.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
        End With
    End Sub

    Private Sub AddGrandTotal()

        Dim Ro As DataRow = Nothing
        Dim pcs As Integer = Val(dtGridView.Compute("SUM(PCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim grsWt As Decimal = Val(dtGridView.Compute("SUM(GRSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL or COLHEAD IS NULL").ToString)
        Dim lessWT As Decimal = Val(dtGridView.Compute("SUM(LESSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim netWt As Decimal = Val(dtGridView.Compute("SUM(NETWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        Dim stnpcs As Decimal = Val(dtGridView.Compute("SUM(STNPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim stnWt As Decimal = Val(dtGridView.Compute("SUM(STNWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim diaPcs As Decimal = Val(dtGridView.Compute("SUM(DIAPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim diaWt As Decimal = Val(dtGridView.Compute("SUM(DIAWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim salValue As Decimal = Val(dtGridView.Compute("SUM(SALVALUE)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        If dtGridView.Rows.Count > 0 Then
            Ro = dtGridView.Select("COLHEAD = 'G'")(0)
        Else
            Ro = dtGridView.NewRow
        End If
        'Ro = dtGridView.NewRow
        Ro.Item("ITEM") = "GRAND TOTAL"
        Ro.Item("PCS") = IIf(pcs <> 0, pcs, DBNull.Value)
        Ro.Item("GRSWT") = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
        Ro.Item("STNPCS") = IIf(stnpcs <> 0, stnpcs, DBNull.Value)
        Ro.Item("STNWT") = IIf(stnWt <> 0, Format(stnWt, "0.000"), DBNull.Value)
        Ro.Item("DIAPCS") = IIf(diaPcs <> 0, diaPcs, DBNull.Value)
        Ro.Item("DIAWT") = IIf(diaWt <> 0, Format(diaWt, "0.000"), DBNull.Value)
        Ro.Item("LESSWT") = IIf(lessWT <> 0, Format(lessWT, "0.000"), DBNull.Value)
        Ro.Item("NETWT") = IIf(netWt <> 0, Format(netWt, "0.000"), DBNull.Value)
        Ro.Item("SALVALUE") = IIf(salValue <> 0, Format(salValue, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then

        Else
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
            '    If dtpDate.Enabled = False Then
            '        MsgBox("Please select tag receipt date", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            '    If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            '        Exit Sub
            '    End If

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
    'ByVal TransSnos As String, ByVal NTransSnos As String,
    Private Sub CreateInternalTransfer(ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long, ByVal Transistno As Integer)
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT AX.*,IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS "
        strSql += vbCrLf + " FROM (SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(C.ITEMID,0) ITEMID,ISNULL(C.SUBITEMID,0) SUBITEMID"

        strSql += vbCrLf + " ,'G' GRSNET,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, '" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,T.PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
        strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID"
        strSql += vbCrLf + " ,53 ORDSTATE_ID FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        'strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
        strSql += " WHERE T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,C.ITEMID,C.SUBITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS ITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS SUBITEMID"
        strSql += vbCrLf + " ,'G' GRSNET,0 AS RATE,'" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
        strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID"
        strSql += vbCrLf + " ,53 ORDSTATE_ID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += " WHERE T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U')"
        'strSql += vbCrLf + " WHERE T.SNO IN (" & NTransSnos & ")"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,CA.ITEMID,T.COMPANYID,IM.METALID,CA.PURITYID) AX"

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS'"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT AXX.*"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM (SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ISNULL(ST.STNSUBITEMID,0) STNSUBITEMID,ST.STONEUNIT AS STONEUNIT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        'strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & TransSnos & ")"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNSUBITEMID"
        strSql += vbCrLf + " ,ST.STONEUNIT AS STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        'strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & NTransSnos & ")"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U')"
        strSql += vbCrLf + " ) AS AXX"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS AS T ON T.COMPANYID = SV.TCOMPANYID"
        strSql += vbCrLf + " AND T.CATCODE = SV.CATCODE "
        'strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,CONVERT(NUMERIC(15,2),0) AS STNAMT"
        strSql += vbCrLf + " ,TCATCODE AS CATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,TCATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
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
        cmd = New OleDbCommand(strSql, cn, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", cn, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", cn, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", cn, tran)

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


        Batchno = GetNewBatchno(cnCostId, BillDate, tran)
        Dim mamount As Decimal = 0
        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate(tran)
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = RefNo
            Ro.Item("REFDATE") = RefDate
            Ro.Item("BAGNO") = Transistno.ToString

            Dim mxrate As Decimal = 0
            mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, tran))
            If Val(Ro.Item("NETWT").ToString) = 0 Then mxrate = Val(Ro.Item("rate").ToString)
            If Ro.Item("METALID").ToString = "G" And AUTOBOOKVALUEG_PER <> 0 Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
            If Ro.Item("METALID").ToString = "S" And AUTOBOOKVALUES_PER <> 0 Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
            If Ro.Item("METALID").ToString = "P" And AUTOBOOKVALUEP_PER <> 0 Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
            If Ro.Item("METALID").ToString = "D" Then mxrate = Val(Ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
            If Ro.Item("METALID").ToString = "T" Then mxrate = Val(Ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
            If Val(Ro.Item("NETWT").ToString) = 0 Then mamount += mxrate Else mamount += mxrate * Val(Ro.Item("NETWT").ToString)
        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Dim Stnamount As Decimal = Val(DtIssStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE NOT IN('D')").ToString)
            Dim Diaamount As Decimal = Val(DtIssStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE = 'D'").ToString)
            If AUTOBOOKVALUET_PER <> 0 And Ro.Item("STONEMODE") = "T" Then mamount += Val(Ro.Item("Stnamt").ToString) * (AUTOBOOKVALUET_PER / 100)
            If AUTOBOOKVALUED_PER <> 0 And Ro.Item("STONEMODE") = "T" Then mamount += Val(Ro.Item("Stnamt").ToString) * (AUTOBOOKVALUED_PER / 100)



        Next
        DtIssStone.AcceptChanges()

        If mamount <> 0 Then
            Dim Roacct As DataRow = Nothing
            Roacct = DtAcctran.NewRow
            With Roacct
                .Item("SNO") = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                '.Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                '.Item("TRANNO") = Tranno : .Item("TRANDATE") = BillDate : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                '.Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
            End With
            DtAcctran.Rows.Add(Roacct)
            DtAcctran.AcceptChanges()
            Roacct = DtAcctran.NewRow
            With Roacct
                .Item("SNO") = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                '.Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                '.Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN" : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                '.Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
            End With
            DtAcctran.Rows.Add(Roacct)
            DtAcctran.AcceptChanges()
        End If




        InsertData(SyncMode.Transaction, DtIssue, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, cn, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtAcctran, cn, tran, cnCostId)
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
        If AUTOBOOKTRAN And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & toCostId & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
        If MessageBox.Show("Sure, You want to transfer the loaded Tags?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Dim frmCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")
        Dim syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then syncdb = uprefix + usuffix
        End If

        Dim obj As TrasitIssRec
        Dim RefNo As String
        Dim TransSno As String = ""
        Dim NTransSno As String = ""
        BillDate = GetEntryDate(GetServerDate)
        Try

            strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            Dim billcontrolid As String = "GEN-SM-INTISS"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
                billcontrolid = "GEN-STKREFNO"
            End If

            Dim NEWBILLNO As Integer
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , Nothing))
GenerateNewBillNo:
            RefNo = cnCostId & NEWBILLNO.ToString
            strSql = "SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..CITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            If AUTOBOOKTRAN Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND TRANNO = " & NEWBILLNO
            End If
            If objGPack.GetSqlValue(strSql, , "-1", Nothing) <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenerateNewBillNo
            End If

            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn, Nothing)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GenerateNewBillNo
            End If
            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TRANS_STATUS')> 0"
            strSql += " DROP TABLE TRANS_STATUS "
            strSql += " CREATE TABLE TRANS_STATUS(STOCKMODE VARCHAR(1),TAGSNO VARCHAR(20),STATUS VARCHAR(1)) "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()



            Isbulkupdate = False

            For Each ro As DataRow In dtGridView.Rows
                tran = Nothing
                tran = cn.BeginTransaction
                If ro.Item("STOCKMODE").ToString = "N" Then
                    Dim xitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
                    Dim xsubitemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
                    Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                    NTransSno += "'" & Sno.ToString & "',"
                    InsertIntoNonTag(frmCostId, toCostId, Sno, xitemid, xsubitemid, 0, Val(ro.Item("PCS").ToString), Val(ro.Item("GRSWT").ToString), Val(ro.Item("NETWT").ToString), RefNo, NEWBILLNO)
                    strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS) SELECT 'N','" & Sno.ToString & "','U'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    If ro.Item("SNO").ToString = "" Then GoTo nextfor
                    obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, cn, tran, RefNo)
                    If Not obj.InsertTagIssue(True) Then GoTo nextfor

                    TransSno += "'" & ro.Item("SNO").ToString & "',"
                    strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS) SELECT 'T','" & ro.Item("Sno").ToString & "','U'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    If IS_IMAGE_TRF = True Then
                        If ro.Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
                            If IO.File.Exists(defalutDestination & ro.Item("PCTFILE").ToString) Then
                                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                                strSql += " )"
                                strSql += " VALUES"
                                strSql += " ('" & cnCostId & "','" & toCostId & "',?,?,'PICPATH')"
                                cmd = New OleDbCommand(strSql, cn, tran)
                                Dim fileStr As New IO.FileStream(defalutDestination & ro.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                                Dim reader As New IO.BinaryReader(fileStr)
                                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                fileStr.Read(result, 0, result.Length)
                                fileStr.Close()
                                cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                                Dim fInfo As New IO.FileInfo(defalutDestination & ro.Item("PCTFILE").ToString)
                                cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                                cmd.ExecuteNonQuery()
                            End If
                        End If
                    End If

                    strSql = " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
                    strSql += " (FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
                    strSql += " SELECT '" & cnCostId & "','" & toCostId & "','" & BillDate.ToString("yyyy-MM-dd") & "','" & ro.Item("SNO").ToString & "','I','T'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " EXEC " & cnAdminDb & "..SP_CCTRANSFER "
                    strSql += vbCrLf + " @TAGSNO = '" & ro.Item("SNO").ToString & "',@COSTID = '" & cnCostId & "',@ISSDATE = '" & BillDate.ToString("yyyy-MM-dd") & "',@ISSTIME=NULL,@VERSION='" & GlobalVariables.VERSION & "',@USERID = " & GlobalVariables.userId & ",@TREFNO='" & RefNo & "',@FLAG='I'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
Nextfor:
                tran.Commit()
                tran = Nothing
            Next

            If TransSno <> "" Then TransSno = Mid(TransSno, 1, TransSno.Length - 1)
            If NTransSno <> "" Then NTransSno = Mid(NTransSno, 1, NTransSno.Length - 1)
            Dim TransistNo As Integer
            billcontrolid = "GEN-TRANSISTNO"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                TransistNo = Val(objGPack.GetSqlValue(strSql, , , tran))
GenerateNewTrnsNo:
                strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND BAGNO = '" & TransistNo.ToString & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    TransistNo = TransistNo + 1
                    GoTo GenerateNewTrnsNo
                End If
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TransistNo.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GenerateNewTrnsNo
                End If

            End If

            tran = Nothing
            tran = cn.BeginTransaction

            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U')"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U')"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            If AUTOBOOKTRAN Then
                CreateInternalTransfer(toCostId, RefNo, BillDate, NEWBILLNO, TransistNo)
            End If
            tran.Commit()
            tran = Nothing

            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate
            btnNew_Click(Me, New EventArgs)
            MsgBox("Data Transfered,Please do the Syncronization")
            If GetAdmindbSoftValue("PRN_STKTRANSFER", "N") = "Y" Then
                ''PrintStockTransfer(RefNo, pBilldate, "IIN")
                If GetAdmindbSoftValue("PRN_STKTRANSFER_DETSUMM", "N") = "Y" Then
                    PrintStockTransfer_DETSUM(RefNo, pBilldate, "IIN")
                Else
                    PrintStockTransfer(RefNo, pBilldate, "IIN")
                End If
            End If
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

    Private Sub InsertIntoNonTag(ByVal tCostId As String, ByVal TransCostId As String, _
ByVal Sno As String, ByVal itemid As Integer, ByVal subitemid As Integer, ByVal designid As Integer, ByVal pcs As Integer, ByVal grswt As Decimal, ByVal netwt As Decimal, ByVal refno As String, ByVal Tranno As Long)

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
        strSql += " ," & itemid
        strSql += " ," & subitemid
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & pcs
        strSql += " ," & grswt
        strSql += " ," & grswt - netwt
        strSql += " ," & netwt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'I'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ,0" '& Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & tCostId & "'" 'COSTID
        strSql += " ,''"
        strSql += " ,0"
        strSql += " ,0"
        '& Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
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
        strSql += " ,'" & refno & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        '        ExecQuery(SyncMode.Transaction, strSql, cn, tran, tCostId)

        'If AUTOBOOKTRAN Then
        ' CreateInternalTransferForNonTag("'" & Sno & "'", tCostId, refno, BillDate, Tranno)
        ' End If

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
        strSql += " ," & itemid
        strSql += " ," & subitemid
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & pcs
        strSql += " ," & grswt
        strSql += " ," & grswt - netwt
        strSql += " ," & netwt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'R'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ,0" '& Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & TransCostId & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & designid '& Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ,0" ' & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'Updated
        strSql += " ,'" & GetServerTime(tran) & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & tCostId & "'" 'TCOSTID
        strSql += " ,'" & refno & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " )"
        Exec(strSql.Replace("'", "''"), cn, TransCostId, Nothing, tran)
    End Sub


    Private Sub CreateInternalTransferForNonTag(ByVal TransSnos As String, ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long)
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
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
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS'"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNSUBITEMID"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = TG.ITEMID"
        strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & TransSnos & ")"

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS AS T ON T.CATCODE = SV.TCATCODE AND T.COMPANYID = SV.TCOMPANYID "
        strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,CONVERT(NUMERIC(15,2),0) AS STNAMT"
        strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,COSTID,COMPANYID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,STNSUBITEMID,COSTID,COMPANYID"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
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

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", cn, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", cn, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", cn, tran)

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
        'TranNo = GetBillNoValue("GEN-SM-ISS", tran)
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


            Dim mxrate As Decimal = 0
            mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, tran))
            If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
            If Ro.Item("METALID").ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
            If Ro.Item("METALID").ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
            'If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
            Dim mamount As Decimal = mxrate * Val(Ro.Item("GRSWT").ToString)
            If mamount <> 0 Then
                Dim Roacct As DataRow = Nothing
                Roacct = DtAcctran.NewRow
                With Roacct
                    .Item("SNO") = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                    .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                End With
                DtAcctran.Rows.Add(Roacct)
                DtAcctran.AcceptChanges()
                Roacct = DtAcctran.NewRow
                With Roacct
                    .Item("SNO") = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate(tran) : .Item("APPVER") = VERSION
                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN" : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                    .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                End With
                DtAcctran.Rows.Add(Roacct)
                DtAcctran.AcceptChanges()
            End If

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
        InsertData(SyncMode.Transaction, DtAcctran, cn, tran, cnCostId)
    End Sub

    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesItemName()
        End If
    End Sub

    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dritem As DataRow
            If Not chkCheckByScan.Checked Then
                If Val(txtItemId.Text) <> 0 Then
                    dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
                    If dritem(0).ToString = "N" Then
                        grpNonTag.Visible = True
                        If dritem(1).ToString = "Y" Then txtSubItemName_OWN.Focus() Else GetStock(cmbTagsCostName_MAN) : cmbDesigner_OWN.Focus()
                        Exit Sub
                    Else
                        grpNonTag.Visible = False
                    End If
                End If
                SendKeys.Send("{TAB}")
                Exit Sub
            End If

            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = Trim(sp(0))
            End If

            dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
            If dritem(0).ToString = "N" Then
                grpNonTag.Visible = True
                If dritem(1).ToString = "Y" Then txtSubItemName_OWN.Focus() Else GetStock(cmbTagsCostName_MAN) : cmbDesigner_OWN.Focus()
                Exit Sub
            End If

            If txtItemId.Text.StartsWith("#") Then txtItemId.Text = txtItemId.Text.Remove(0, 1)
CheckItem:
            If txtItemId.Text = "" Then
                LoadSalesItemName()
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'") = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo CheckItem
                Else
                    LoadSalesItemName()
                End If
            Else
                LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" Then
                txtTagNo.Focus()
                txtTagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
            End If
        End If
    End Sub
    Private Sub LoadSalesItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'" '' AND STOCKTYPE = 'T'"

        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtItemId.Text))
        If itemId > 0 Then

            txtItemId.Text = itemId
            If objGPack.GetSqlValue("select STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & itemId) = "N" Then Exit Sub
            LoadSalesItemNameDetail()
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
    End Sub

    Private Sub LoadSalesItemNameDetail()
        If txtItemId.Text = "" Then
            Exit Sub
        Else
            Me.SelectNextControl(txtItemId, True, True, True, True)
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

        If cmbMetal.Text = "ALL" Then
            strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST"
            strSql += " ORDER BY METALNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    selMetalId += "'" & dt.Rows(i).Item("METALID").ToString & "',"
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For j As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += "'"
                    selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
                    selMetalId += "'"
                    selMetalId += ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If

        If cmbCounter_OWN.Text = "ALL" Then
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER"
            strSql += " ORDER BY ITEMCTRNAME"
            Dim dt1 As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                For k As Integer = 0 To dt1.Rows.Count - 1
                    selCounterId += "'" & dt1.Rows(k).Item("ITEMCTRID").ToString & "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        ElseIf cmbCounter_OWN.Text <> "" Then
            Dim sql As String = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbCounter_OWN.Text) & ")"
            Dim dtCounter As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCounter)
            If dtCounter.Rows.Count > 0 Then
                For m As Integer = 0 To dtCounter.Rows.Count - 1
                    selCounterId += "'" & dtCounter.Rows(m).Item("ITEMCTRID").ToString + "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        End If


        If chkCounter.Checked Then
            If gridView.Rows.Count > 1 Then
                If grpNonTag.Visible = True Then
                    strSql = vbCrLf + " insert into TEMPTABLEDB..TEMPTAGTRANSFER1"
                    strSql += vbCrLf + " SELECT '' SNO,(select itemname from " & cnAdminDb & "..ITEMMAST IT where IT.ITEMID = " & txtItemId.Text & ") ITEM"
                    strSql += vbCrLf + " ,'" & txtSubItemName_OWN.Text & "' SUBITEM"
                    strSql += vbCrLf + " ,'NONTAG' TAGNO,CONVERT(INT," & txtPcs_NUM.Text & ") PCS,CONVERT(DECIMAL," & txtGrsWt_WET.Text & ") GRSWT,0 LESSWT,CONVERT(DECIMAL," & txtNetWt_WET.Text & ") NETWT"
                    strSql += vbCrLf + " ,0 AS STNPCS"
                    strSql += vbCrLf + " ,0 AS STNWT"
                    strSql += vbCrLf + " ,0 AS DIAPCS"
                    strSql += vbCrLf + " ,0 AS DIAWT"
                    strSql += vbCrLf + " ,0 SALVALUE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE"
                    strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,''TCOUNTER "
                    strSql += vbCrLf + ",(SELECT METALNAME  FROM " & cnAdminDb & "..METALMAST M WHERE METALID IN(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text) & ")) TMETAL,'N' STOCKMODE"
                    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGTRANSFER1"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                Else

                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER1 "
                    strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM,SUB.SUBITEMNAME SUBITEM"
                    strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                    strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                    strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                    strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    strSql += vbCrLf + " ,SALVALUE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                    strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER "
                    strSql += vbCrLf + ",M.METALNAME  TMETAL,'T' STOCKMODE"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                    strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                    strSql += vbCrLf + " AND SNO NOT IN (SELECT SNO FROM TEMPTABLEDB..TEMPTAGTRANSFER1)"
                    'strSql += vbCrLf + " AND (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) IN (" & selMetalId & ")"
                    strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                    strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                    strSql += vbCrLf + " AND APPROVAL = ''"
                    If Val(txtLotNo_NUM.Text) <> 0 Then
                        strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                    End If
                    If chkCounter.Checked = False Then
                        If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                            strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                        End If
                    End If
                    If dtpDate.Enabled Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                    If txtTagNo.Text <> "" Then
                        strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                    End If
                    If Val(txtItemId.Text) > 0 Then
                        strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                    End If
                    If Val(txtEstNo.Text) <> 0 Then
                        strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                    End If
                    strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                    strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                    If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                        strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                    End If
                End If
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

            Else

                strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER1') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER1 "
                If grpNonTag.Visible Then
                    strSql += vbCrLf + " SELECT '' SNO,(select itemname from " & cnAdminDb & "..ITEMMAST IT where IT.ITEMID = " & txtItemId.Text & ") ITEM"
                    strSql += vbCrLf + " , CONVERT(VARCHAR(30),'" & txtSubItemName_OWN.Text & "') SUBITEM"
                    strSql += vbCrLf + " ,'NONTAG' TAGNO,CONVERT(INT," & txtPcs_NUM.Text & ") PCS,CONVERT(DECIMAL," & txtGrsWt_WET.Text & ") GRSWT,0 LESSWT,CONVERT(DECIMAL," & txtNetWt_WET.Text & ") NETWT"
                    strSql += vbCrLf + " ,0 AS STNPCS"
                    strSql += vbCrLf + " ,0 AS STNWT"
                    strSql += vbCrLf + " ,0 AS DIAPCS"
                    strSql += vbCrLf + " ,0 AS DIAWT"
                    strSql += vbCrLf + " ,0 SALVALUE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE"
                    strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,CONVERT(VARCHAR(30),(select itemname from " & cnAdminDb & "..ITEMMAST IT where IT.ITEMID = " & txtItemId.Text & "))  TITEM,CONVERT(VARCHAR(30),'')TCOUNTER "
                    strSql += vbCrLf + ",CONVERT(VARCHAR(30),(SELECT METALNAME  FROM " & cnAdminDb & "..METALMAST M WHERE METALID IN(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text) & "))) TMETAL,'N' STOCKMODE"
                    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGTRANSFER1"

                Else
                    strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM,SUB.SUBITEMNAME SUBITEM"
                    'strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                    ' strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
                    strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                    strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                    strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                    strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                    strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                    strSql += vbCrLf + " ,SALVALUE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                    strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER "
                    'strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS TITEM "
                    'strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS TCOUNTER"
                    'strSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)) TMETAL"
                    strSql += vbCrLf + ",M.METALNAME  TMETAL,'T' STOCKMODE"
                    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGTRANSFER1"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                    strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                    'strSql += vbCrLf + " AND (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) IN (" & selMetalId & ")"
                    strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                    'strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                    strSql += vbCrLf + " AND APPROVAL = ''"
                    If Val(txtLotNo_NUM.Text) <> 0 Then
                        strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                    End If
                    If chkCounter.Checked = False Then
                        If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                            strSql += vbCrLf & " AND T.ITEMCTRID IN (" & selCounterId & ")"
                            'strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                        End If
                    End If
                    If dtpDate.Enabled Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                    If txtTagNo.Text <> "" Then
                        strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                    End If
                    If Val(txtItemId.Text) > 0 Then
                        strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                    End If
                    If Val(txtEstNo.Text) <> 0 Then
                        strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                    End If
                    strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                    strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                    If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                        strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                    End If
                End If
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER"
            strSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPTAGTRANSFER FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"

            'Dim dtTempx As New DataTable
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtTempx)
            'For Each ro As DataRow In dtGridView.Select("Sno<>''")
            '    dtTempx.ImportRow(ro)
            'Next


            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPTAGTRANSFER)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,PCTFILE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER ,TMETAL,TCOUNTER,'' SUBITEM,'' TAGNO,0 RESULT,'T' COLHEAD,'' SNO,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,'' PCTFILE  FROM TEMPTABLEDB..TEMPTAGTRANSFER "

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,PCTFILE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TMETAL,'' SUBITEM,''TAGNO,6 RESULT,'N' COLHEAD,'' SNO ,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,'' PCTFILE  FROM TEMPTABLEDB..TEMPTAGTRANSFER"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,TITEM,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,SNO,STOCKMODE,PCTFILE)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TITEM,'ITEM SUB TOTAL','' SUBITEM,''TAGNO,2 RESULT,'S' COLHEAD,"
            strSql += vbCrLf + " SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),'' SNO,'' STOCKMODE,''PCTFILE"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER GROUP BY TCOUNTER,TMETAL,TITEM"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,SNO,TITEM,STOCKMODE,PCTFILE)  "
            strSql += vbCrLf + " SELECT DISTINCT 'ZZZZZZZ','ZZZZZZZZZZZZ','GRAND TOTAL','' SUBITEM,'' TAGNO,3 RESULT,'G' COLHEAD"
            strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),'' SNO,'GRAND TOTAL','' STOCKMODE,'' PCTFILE"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE RESULT IN (1) "
            strSql += vbCrLf + " End"

            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " DELETE FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE TITEM IS NULL AND COLHEAD='S'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"

            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            Dim dtTemp As New DataTable
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            'dtTemp = dtTempx.Copy
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                txtTagNo.Focus()
                If chkCheckByScan.Checked Then
                    txtTagNo.Focus()
                End If
                Exit Sub
            ElseIf dtTemp.Rows.Count > 0 Then
                If grpNonTag.Visible Then txtItemId.Focus() Else txtTagNo.Focus()
            End If

            Dim DTMETALDET As New DataTable
            If cmbMetal.Text = "ALL" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY M.METALNAME ,TMETAL"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            ElseIf cmbMetal.Text <> "" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY TMETAL,m.METALNAME"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            End If

            'For Each ro As DataRow In dtTemp.Rows
            '    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
            '    If RowCol.Length = 0 Then
            '        dtGridView.ImportRow(ro)
            '    End If
            'Next

            dtGridView.AcceptChanges()
            gridView.DataSource = dtTemp
            dtGridView = dtTemp.Copy
            With gridView

                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("TITEM").Visible = False
                .Columns("TCOUNTER").Visible = False
                .Columns("TMETAL").Visible = False
                .Columns("ITEM").Width = 150
                .Columns("SUBITEM").Width = 150
                'gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            'AddGrandTotal()
            GridViewFormat()
            grpNonTag.Visible = False
            cmbCostCentre_MAN.Enabled = False
            If chkCheckByScan.Checked Then

                txtTagNo.Clear()
                txtItemId.Select()
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Clear() : txtEstNo.Select()
            End If
            Prop_Sets()
        ElseIf chkCounter.Checked = False Then
            If grpNonTag.Visible Then
                strSql = vbCrLf + " SELECT '' SNO,(select itemname from " & cnAdminDb & "..ITEMMAST IT where IT.ITEMID = " & txtItemId.Text & ") ITEM"
                strSql += vbCrLf + " , '" & txtSubItemName_OWN.Text & "' SUBITEM"
                strSql += vbCrLf + " ,'NONTAG' TAGNO,CONVERT(INT," & txtPcs_NUM.Text & ") PCS,CONVERT(DECIMAL," & txtGrsWt_WET.Text & ") GRSWT,0 LESSWT,CONVERT(DECIMAL," & txtNetWt_WET.Text & ") NETWT"
                strSql += vbCrLf + " ,0 AS STNPCS"
                strSql += vbCrLf + " ,0 AS STNWT"
                strSql += vbCrLf + " ,0 AS DIAPCS"
                strSql += vbCrLf + " ,0 AS DIAWT"
                strSql += vbCrLf + " ,0 SALVALUE"
                strSql += vbCrLf + ", '' AS COUNTER"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE,'N' AS STOCKMODE"

            Else
                strSql = " SELECT SNO,IT.ITEMNAME ITEM,SUB.SUBITEMNAME SUBITEM"
                strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += " ,SALVALUE"
                strSql += " ,CTR.ITEMCTRNAME AS COUNTER"
                'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
                strSql += " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE,'T' AS STOCKMODE"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += " WHERE ISSDATE IS NULL"
                strSql += " AND APPROVAL = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                    strSql += " AND T.ITEMCTRID IN (" & selCounterId & ")"
                    '(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                End If
                If dtpDate.Enabled Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                If txtTagNo.Text <> "" Then
                    strSql += " AND TAGNO = '" & txtTagNo.Text & "'"
                End If
                If Val(txtItemId.Text) > 0 Then
                    strSql += " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                End If
                If Val(txtEstNo.Text) <> 0 Then
                    strSql += vbCrLf + " AND TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                End If
                strSql += " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                strSql += " AND COSTID = '" & cnCostId & "'"
                If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then
                    strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                End If
            End If
            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            Dim dtTemp As New DataTable
            ' dtGridView.Clear()
            ' gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                grpNonTag.Visible = False
                txtTagNo.Focus()
                If chkCheckByScan.Checked Then
                    txtTagNo.Focus()
                End If
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                Exit Sub
            ElseIf dtTemp.Rows.Count > 0 Then
                If grpNonTag.Visible = True Then txtItemId.Focus() Else txtTagNo.Focus()
            End If
            For Each ro As DataRow In dtTemp.Rows
                If ro.Item("TAGNO").ToString = "NONTAG" Then
                    dtGridView.ImportRow(ro)
                Else
                    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
                    If RowCol.Length = 0 Then
                        dtGridView.ImportRow(ro)
                    End If

                End If

            Next
            dtGridView.AcceptChanges()
            gridView.DataSource = dtTemp
            With gridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False

            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            AddGrandTotal()
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            cmbCostCentre_MAN.Enabled = False
            grpNonTag.Visible = False
            If chkCheckByScan.Checked Then
                txtTagNo.Clear()
                txtItemId.Select()
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
            End If
        End If
    End Sub

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus
        cmbTagsCostName_MAN.Items.Clear()
        cmbTagsCostName_MAN.Items.Add(cmbCostCentre_MAN.Text)
        cmbTagsCostName_MAN.Items.Add(cnCostName)
        cmbTagsCostName_MAN.Text = cmbCostCentre_MAN.Text
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
        Dim obj As New frmTagTransfer_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCounter = cmbCounter_OWN.Text
        SetSettingsObj(obj, Me.Name, GetType(frmTagTransfer_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagTransfer_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagTransfer_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbCounter_OWN.Text = obj.p_cmbCounter
    End Sub

    Private Sub chkCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounter.CheckedChanged
        If chkCounter.Checked Then
            cmbMetal.Enabled = True
        Else
            cmbMetal.Enabled = False
        End If
    End Sub

    Private Sub txtEstNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEstNo.TextChanged

    End Sub

    Private Sub ChkwithImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtItemId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId.TextChanged

    End Sub

    Private Sub txtSubItemName_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSubItemName_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSubItemName_OWN.Text = "" Then Loadsubitemnames()
            txtSubItemName_OWN.Tag = Val(txtSubItemName_OWN.Text)
        End If
    End Sub

    Private Sub txtSubItemName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubItemName_OWN.Leave



        GetStock(cmbTagsCostName_MAN, txtSubItemName_OWN.Tag)
        If txtSubItemName_OWN.Tag <> 0 Then
            txtSubItemName_OWN.Text = objGPack.GetSqlValue("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & txtSubItemName_OWN.Tag & " AND ITEMID = " & Val(txtItemId.Text))
        End If
    End Sub

    Private Sub txtNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then btnSearch.Focus() : Exit Sub

    End Sub
    Private Sub Loadsubitemnames()
        strSql = " SELECT"
        strSql += " SUBITEMID AS SUBITEMID,SUBITEMNAME AS SUBITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        strSql += " FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += " WHERE ITEMID = " & txtItemId.Text & " AND ACTIVE = 'Y'" '' AND STOCKTYPE = 'T'"
        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find Sub Item Name", strSql, cn, 1, , , txtSubItemName_OWN.Text))
        If itemId > 0 Then
            txtSubItemName_OWN.Text = itemId
        End If
    End Sub

    Private Sub txtGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrsWt_WET.TextChanged
        txtNetWt_WET.Text = txtGrsWt_WET.Text
    End Sub
End Class

Public Class frmTagTransfer_Properties
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
