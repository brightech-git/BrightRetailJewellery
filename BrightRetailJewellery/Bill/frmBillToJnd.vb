Imports System.Data.OleDb
Public Class frmBillToJnd
    Public Enum BillViewType
        BillView = 0
        DulicateBillPrint = 1
    End Enum
    Dim objRemark As New frmBillRemark
    Dim objAdvscr As New frmBviewadv
    Dim dtOtherDetails As New DataTable
    Dim tran As OleDbTransaction
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dtHead As New DataTable
    Public type As BillViewType = BillViewType.BillView
    Dim Advsearch1 As String = "", Advsearch2 As String = ""
    Private PicPath As String = GetAdmindbSoftValue("PICPATH")
    Dim Iscurrdate As Boolean = IIf(GetAdmindbSoftValue("VIEW_CURDATE") = "Y", True, False)
    Dim musrid As Integer = 0
    Dim Authorize As Boolean = False


#Region "Approval Material"
    Private Sub ApprovalMaterial(ByVal batchno As String, ByVal costId As String)
        'MsgBox(PicPath)
        strSql = vbCrLf + "DECLARE @DEFPATH VARCHAR(100)"
        strSql += vbCrLf + " SET @DEFPATH ='" + PicPath + "';"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET_ISSUE') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET_ISSUE"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " ISS.SNO"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL)TEMP_ITEM"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)SLNO"
        strSql += vbCrLf + " ,TG.STYLENO"
        strSql += vbCrLf + " ,IM.ITEMNAME AS ITEM"
        strSql += vbCrLf + " --,SM.SUBITEMNAME AS SUBITEM"
        strSql += vbCrLf + " ,IT.NAME KT"
        strSql += vbCrLf + " ,ISS.GRSWT,ISS.NETWT"
        strSql += vbCrLf + " ,ISS.RATE "
        strSql += vbCrLf + " ,ISS.AMOUNT - ISNULL(MCHARGE,0)"
        strSql += vbCrLf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = ISS.SNO),0) AS COST"
        strSql += vbCrLf + " ,CONVERT(NVARCHAR(50),NULL) DETAILS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) DWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) CSWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RDYWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) SRATE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) SCOST"
        strSql += vbCrLf + " ,ISS.MCHARGE AS LABOUR"
        strSql += vbCrLf + " ,ISS.AMOUNT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(TG.PCTFILE,'')<>'' THEN @DEFPATH + TG.PCTFILE ELSE '' END PCTFILE "
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET_ISSUE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS ISS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.ITEMID AND SM.SUBITEMID = ISS.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.ITEMID = ISS.ITEMID AND TG.TAGNO = ISS.TAGNO AND TG.BATCHNO = ISS.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID= TG.ITEMTYPEID "
        strSql += vbCrLf + " WHERE ISS.BATCHNO = '" & batchno & "' AND ISNULL(ISS.COSTID,'') = '" & costId & "'"
        strSql += vbCrLf + " ORDER BY ITEM"

        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET_ISSUE SET TEMP_ITEM = ITEM"
        strSql += vbCrLf + "-- SELECT * FROM MASTER..TEMP_WSHEET_ISSUE "


        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET_STUD') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(INT,0) SLNO"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(SM.SHORTNAME,'') <> '' THEN SM.SHORTNAME"
        strSql += vbCrLf + "      WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME"
        strSql += vbCrLf + "      WHEN ISNULL(IM.SHORTNAME,'') <> '' THEN IM.SHORTNAME"
        strSql += vbCrLf + "      ELSE IM.ITEMNAME END AS SHAPE"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'D' THEN ISS.STNWT ELSE NULL END AS DWT"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'S' THEN ISS.STNWT ELSE NULL END AS CSWT"
        strSql += vbCrLf + " ,ISS.STNRATE SRATE"
        strSql += vbCrLf + " ,ISS.STNAMT SCOST"
        strSql += vbCrLf + " ,ISS.ISSSNO AS SNO"
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS ISS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.STNITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.STNITEMID AND SM.SUBITEMID = ISS.STNSUBITEMID"
        strSql += vbCrLf + " WHERE ISS.ISSSNO IN (SELECT SNO FROM MASTER..TEMP_WSHEET_ISSUE)"
        strSql += vbCrLf + " ORDER BY SNO"

        strSql += vbCrLf + "  DECLARE @USLNO INT"
        strSql += vbCrLf + "  DECLARE @USNO VARCHAR(15)"
        strSql += vbCrLf + "  DECLARE @OSNO VARCHAR(15)"
        strSql += vbCrLf + "  SET @USLNO = 0"
        strSql += vbCrLf + "  SET @OSNO = ''"
        strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT SNO FROM MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + "  OPEN CUR"
        strSql += vbCrLf + "  WHILE 1=1"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	FETCH NEXT FROM CUR INTO @USNO"
        strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  	IF @OSNO <> @USNO"
        strSql += vbCrLf + "  		BEGIN"
        strSql += vbCrLf + "  		SELECT @USLNO = SLNO FROM MASTER..TEMP_WSHEET_ISSUE WHERE SNO = @USNO"
        strSql += vbCrLf + "  		SET @OSNO = @USNO"
        strSql += vbCrLf + "  		END"
        strSql += vbCrLf + "  	ELSE"
        strSql += vbCrLf + "  		SET @USLNO = @USLNO + 1"
        strSql += vbCrLf + "  	PRINT @USLNO"
        strSql += vbCrLf + "  	UPDATE MASTER..TEMP_WSHEET_STUD SET SLNO = @USLNO WHERE CURRENT OF CUR		"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  CLOSE CUR"
        strSql += vbCrLf + "  DEALLOCATE CUR"
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " SELECT DS.SNO AS DSNO, ISS.SNO,ISS.RESULT,ISS.COLHEAD,ISS.TEMP_ITEM,ISS.SLNO,ISS.STYLENO,"
        strSql += vbCrLf + "ISS.ITEM,ISS.KT,ISS.GRSWT,ISS.NETWT,ISS.RATE,ISS.COST,"
        strSql += vbCrLf + "DS.SHAPE DETAILS,DS.DWT,DS.CSWT,ISS.RDYWT,DS.SRATE,DS.SCOST,ISS.LABOUR,ISS.AMOUNT,ISS.PCTFILE"
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET_ISSUE AS ISS"
        strSql += vbCrLf + " FULL JOIN MASTER..TEMP_WSHEET_STUD AS DS ON DS.SLNO = ISS.SLNO AND DS.SNO = ISS.SNO "

        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET SET RESULT = 1 ,SNO = DSNO WHERE SNO IS NULL"
        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET SET TEMP_ITEM = (SELECT TEMP_ITEM FROM MASTER..TEMP_WSHEET WHERE SNO = O.SNO AND ISNULL(TEMP_ITEM,'') <> '')"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET AS O"
        strSql += vbCrLf + " WHERE ISNULL(O.TEMP_ITEM,'') = ''"

        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..TEMP_WSHEET)>0"
        strSql += vbCrLf + " BEGIN /** INSERTING SUB & GRAND TOT **/"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " (RESULT,COLHEAD,TEMP_ITEM,STYLENO"
        strSql += vbCrLf + " ,GRSWT,NETWT,COST,LABOUR,AMOUNT,"
        strSql += vbCrLf + " DWT,CSWT,SRATE,SCOST)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 2 RESULT,'S'COLHEAD,TEMP_ITEM,'SUB AMOUNT'"
        strSql += vbCrLf + " ,SUM(GRSWT),SUM(NETWT),SUM(COST),SUM(LABOUR),SUM(AMOUNT)"
        strSql += vbCrLf + " ,SUM(DWT),SUM(CSWT),SUM(SRATE),SUM(SCOST)"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET "
        strSql += vbCrLf + " WHERE RESULT = 1"
        strSql += vbCrLf + " GROUP BY TEMP_ITEM"

        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " (RESULT,COLHEAD,TEMP_ITEM,STYLENO"
        strSql += vbCrLf + " ,GRSWT,NETWT,COST,LABOUR,AMOUNT"
        strSql += vbCrLf + " ,DWT,CSWT,SRATE,SCOST)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 3 RESULT,'G'COLHEAD,'ZZZZZZZ'TEMP_ITEM,'GRAND AMOUNT'"
        strSql += vbCrLf + " ,SUM(GRSWT),SUM(NETWT),SUM(COST),SUM(LABOUR),SUM(AMOUNT)"
        strSql += vbCrLf + " ,SUM(DWT),SUM(CSWT),SUM(SRATE),SUM(SCOST)"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET "
        strSql += vbCrLf + " WHERE RESULT = 1"
        strSql += vbCrLf + " END"


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE MASTER..TEMP_WSHEET SET"
        strSql += vbCrLf + " GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
        strSql += vbCrLf + " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
        strSql += vbCrLf + " ,RATE = CASE WHEN RATE = 0 THEN NULL ELSE RATE END"
        strSql += vbCrLf + " ,COST = CASE WHEN COST = 0 THEN NULL ELSE COST END"
        strSql += vbCrLf + " ,DWT = CASE WHEN DWT = 0 THEN NULL ELSE DWT END"
        strSql += vbCrLf + " ,CSWT = CASE WHEN CSWT = 0 THEN NULL ELSE CSWT END"
        strSql += vbCrLf + " ,RDYWT = CASE WHEN RDYWT = 0 THEN NULL ELSE RDYWT END"
        strSql += vbCrLf + " ,SRATE = CASE WHEN SRATE = 0 THEN NULL ELSE SRATE END"
        strSql += vbCrLf + " ,SCOST = CASE WHEN SCOST = 0 THEN NULL ELSE SCOST END"
        strSql += vbCrLf + " ,LABOUR = CASE WHEN LABOUR = 0 THEN NULL ELSE LABOUR END"
        strSql += vbCrLf + " ,AMOUNT = CASE WHEN AMOUNT = 0 THEN NULL ELSE AMOUNT END"


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "SELECT *,CONVERT(image,null) TAGIMAGE FROM MASTER..TEMP_WSHEET ORDER BY TEMP_ITEM,RESULT,SNO,SLNO DESC"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        'With dtGrid
        '    .Columns("GRSWT").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("NETWT").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("RATE").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("COST").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("LABOUR").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("SRATE").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("SCOST").SetOrdinal(dtGrid.Columns.Count - 1)
        '    .Columns("AMOUNT").SetOrdinal(dtGrid.Columns.Count - 1)
        'End With


        Dim refreshVar As Integer = 0
        Dim fileDestPath As String = Nothing
        Dim picBox As New PictureBox
        picBox.Size = New Size(100, 100)
        picBox.SizeMode = PictureBoxSizeMode.CenterImage


        For i As Integer = 0 To dtGrid.Rows.Count - 1
            refreshVar += 1
            If dtGrid.Rows(i).Item("SLNO").ToString() <> DBNull.Value.ToString() Then
                If dtGrid.Rows(i).Item("PCTFILE").ToString = "" Then
                    picBox.Image = My.Resources.EmptyImage
                    Dim ms As New IO.MemoryStream
                    picBox.Image.Save(ms, Imaging.ImageFormat.Jpeg)
                    Dim bytes() As Byte = ms.GetBuffer()
                    dtGrid.Rows(i).Item("TAGIMAGE") = bytes
                Else
                    fileDestPath = dtGrid.Rows(i).Item("PCTFILE").ToString
                    If IO.File.Exists(fileDestPath) Then
                        AutoImageSizer(fileDestPath, picBox, PictureBoxSizeMode.CenterImage)
                        Dim ms As New IO.MemoryStream
                        picBox.Image.Save(ms, Imaging.ImageFormat.Jpeg)
                        Dim bytes() As Byte = ms.GetBuffer()
                        dtGrid.Rows(i).Item("TAGIMAGE") = bytes
                    Else
                        picBox.Image = My.Resources.EmptyImage
                        Dim ms As New IO.MemoryStream
                        picBox.Image.Save(ms, Imaging.ImageFormat.Jpeg)
                        Dim bytes() As Byte = ms.GetBuffer()
                        dtGrid.Rows(i).Item("TAGIMAGE") = bytes
                    End If
                End If
                If (refreshVar) = 200 Then
                    ProgressBarStep(refreshVar.ToString + " Tag's Loaded..")
                    refreshVar = 1
                End If
            End If
        Next
        '------------------------
        Dim objGridShower As New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "APPROVAL MEMO"
        Dim dtTit As New DataTable
        Dim tit As String = "APPROVAL MEMO"
        Dim Name As String = ""
        Dim GoldRate As String = ""
        tit += vbCrLf + " REF NO : " & objGPack.GetSqlValue("SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "' AND COSTID = '" & costId & "'") + " DATE : " & dtpBillDate.Text + vbCrLf
        'strSql = vbCrLf + " SELECT DISTINCT RATE FROM " & cnStockDb & "..ISSUE AS ISS"
        'strSql += vbCrLf + " WHERE ISS.BATCHNO = '" & batchno & "' AND ISNULL(ISS.COSTID,'') = '" & costId & "'"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtTit)
        'tit += " Gold Pure  : " & Format(Val(GetRate_Purity(dtpBillDate.Value, "01")), "0.00")
        'tit += " ,Gold Charg : "
        'If dtTit.Rows.Count > 0 Then
        '    tit += dtTit.Rows(0).Item(0).ToString + ","
        'End If
        'tit = Mid(tit, 1, tit.Length - 1) + vbCrLf
        strSql = " SELECT CASE WHEN ISNULL(PREVID,'') <> '' THEN 'PREVILEGEID : ' + PREVID ELSE NAME END"
        strSql += vbCrLf + " FROM"
        strSql += " ("
        strSql += " SELECT 'PARTY : ' + PNAME + CASE WHEN ISNULL(ACCODE,'') <> '' THEN '['+ACCODE+']' ELSE '' END AS NAME "
        strSql += vbCrLf + " ,(SELECT PREVILEGEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = P.ACCODE)AS PREVID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS P"
        strSql += " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchno & "')"
        strSql += " )X"
        Name += objGPack.GetSqlValue(strSql) + vbCrLf
        tit += Name
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        '-------------------
        'Dim DgvImageColumn = New DataGridViewImageColumn
        'DgvImageColumn.Name = "TAGIMAGE"
        'DgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch
        'objGridShower.gridView.RowTemplate.Height = 120
        'objGridShower.gridView.Columns.Add(DgvImageColumn)
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        objGridShower.gridView.AllowUserToOrderColumns = True
        For Each col As DataGridViewColumn In objGridShower.gridView.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        '-------------------
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.gridViewHeader.Visible = True
        ApprovalGridViewHeaderCreator(objGridShower.gridViewHeader)
        ApprovalGridFormat(objGridShower.gridView)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub ApprovalGridFormat(ByVal Dgv As DataGridView)
        Dgv.Columns("DSNO").Visible = False
        Dgv.Columns("SNO").Visible = False
        Dgv.Columns("RESULT").Visible = False
        Dgv.Columns("COLHEAD").Visible = False
        Dgv.Columns("TEMP_ITEM").Visible = False
        Dgv.Columns("KEYNO").Visible = False
        Dgv.Columns("PCTFILE").Visible = False
        Dgv.Columns("SRATE").HeaderText = "RATE"
        Dgv.Columns("SCOST").HeaderText = "COST"
        BrighttechPack.FormatGridColumns(Dgv)
    End Sub

    Private Sub ApprovalGridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[SLNO],''[STYLENO],''[ITEM],''[KT],''[GRSWT~NETWT],''[RATE~COST]"
        strSql += " ,''[DETAILS~DWT~CSWT~RDYWT~SRATE~SCOST]"
        strSql += " ,''[LABOUR]"
        strSql += " ,''[AMOUNT]"
        strSql += " ,''[TAGIMAGE]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("GRSWT~NETWT").HeaderText = "WEIGHT"
        gridviewHead.Columns("RATE~COST").HeaderText = "MATERIAL"
        gridviewHead.Columns("DETAILS~DWT~CSWT~RDYWT~SRATE~SCOST").HeaderText = "SUB - ITEMS"
        gridviewHead.Columns("TAGIMAGE").HeaderText = "DESIGN"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub
#End Region

#Region "Packing Material"
    Private Sub PackingMaterial(ByVal batchno As String, ByVal costId As String)
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET_ISSUE') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET_ISSUE"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " ISS.SNO"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL)TEMP_ITEM"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)SLNO"
        strSql += vbCrLf + " ,TG.STYLENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(50),TG.TAGNO)TAGNO"
        strSql += vbCrLf + " ,TG.CERTIFICATENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)BRANCH"
        'strSql += vbCrLf + " ,SUBSTRING(CASE WHEN ISNULL(SM.SHORTNAME,'') <> '' THEN SM.SHORTNAME"
        'strSql += vbCrLf + "      WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME"
        'strSql += vbCrLf + "      WHEN ISNULL(IM.SHORTNAME,'') <> '' THEN IM.SHORTNAME"
        'strSql += vbCrLf + "      ELSE IM.ITEMNAME END,1,3) AS ITEM"
        strSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
        strSql += vbCrLf + " ,ISS.PCS,ISS.GRSWT,ISS.NETWT,ISS.WASTAGE"
        strSql += vbCrLf + " ,ISS.AMOUNT - ISNULL(MCHARGE,0)"
        strSql += vbCrLf + "  - ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = ISS.SNO),0) AS GVALUE"
        strSql += vbCrLf + " ,ISS.MCHARGE AS LABOUR"
        strSql += vbCrLf + " ,ISS.AMOUNT AS TOTAL"
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET_ISSUE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS ISS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.ITEMID AND SM.SUBITEMID = ISS.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.ITEMID = ISS.ITEMID AND TG.TAGNO = ISS.TAGNO AND TG.BATCHNO = ISS.BATCHNO"
        strSql += vbCrLf + " WHERE ISS.BATCHNO = '" & batchno & "' AND ISNULL(ISS.COSTID,'') = '" & costId & "'"
        'strSql += vbCrLf + " WHERE ISS.TRANDATE = '2011-01-19' AND ISS.TRANNO = 805 "
        strSql += vbCrLf + " ORDER BY ITEM"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET_ISSUE SET TEMP_ITEM = ITEM"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET_STUD') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(INT,0) SLNO"
        strSql += vbCrLf + " ,SUBSTRING(CASE WHEN ISNULL(SM.SHORTNAME,'') <> '' THEN SM.SHORTNAME"
        strSql += vbCrLf + "      WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN SM.SUBITEMNAME"
        strSql += vbCrLf + "      WHEN ISNULL(IM.SHORTNAME,'') <> '' THEN IM.SHORTNAME"
        strSql += vbCrLf + "      ELSE IM.ITEMNAME END,1,3) AS SHAPE"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'D' THEN ISS.STNPCS ELSE NULL END AS DPCS"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'D' THEN ISS.STNWT ELSE NULL END AS DWT"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'D' THEN ISS.STNRATE ELSE NULL END AS DRATE"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'D' THEN ISS.STNAMT ELSE NULL END AS DVALUE"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'S' THEN ISS.STNPCS ELSE NULL END AS SPCS"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'S' THEN ISS.STNWT ELSE NULL END AS SWT"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'S' THEN ISS.STNRATE ELSE NULL END AS SRATE"
        strSql += vbCrLf + " ,CASE WHEN IM.DIASTONE = 'S' THEN ISS.STNAMT ELSE NULL END AS SVALUE"
        strSql += vbCrLf + " ,ISS.ISSSNO AS SNO"
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS ISS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.STNITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.STNITEMID AND SM.SUBITEMID = ISS.STNSUBITEMID"
        strSql += vbCrLf + " WHERE ISS.ISSSNO IN (SELECT SNO FROM MASTER..TEMP_WSHEET_ISSUE)"
        strSql += vbCrLf + " ORDER BY SNO"

        strSql += vbCrLf + "  DECLARE @USLNO INT"
        strSql += vbCrLf + "  DECLARE @USNO VARCHAR(15)"
        strSql += vbCrLf + "  DECLARE @OSNO VARCHAR(15)"
        strSql += vbCrLf + "  SET @USLNO = 0"
        strSql += vbCrLf + "  SET @OSNO = ''"
        strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT SNO FROM MASTER..TEMP_WSHEET_STUD"
        strSql += vbCrLf + "  OPEN CUR"
        strSql += vbCrLf + "  WHILE 1=1"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	FETCH NEXT FROM CUR INTO @USNO"
        strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  	IF @OSNO <> @USNO"
        strSql += vbCrLf + "  		BEGIN"
        strSql += vbCrLf + "  		SELECT @USLNO = SLNO FROM MASTER..TEMP_WSHEET_ISSUE WHERE SNO = @USNO"
        strSql += vbCrLf + "  		SET @OSNO = @USNO"
        strSql += vbCrLf + "  		END"
        strSql += vbCrLf + "  	ELSE"
        strSql += vbCrLf + "  		SET @USLNO = @USLNO + 1"
        strSql += vbCrLf + "  	print @uslno"
        strSql += vbCrLf + "  	UPDATE MASTER..TEMP_WSHEET_STUD SET SLNO = @USLNO WHERE CURRENT OF CUR		"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  CLOSE CUR"
        strSql += vbCrLf + "  DEALLOCATE CUR"


        strSql += vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WSHEET') IS NOT NULL DROP TABLE MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " SELECT DS.SNO AS DSNO,ISS.*"
        strSql += vbCrLf + " ,DS.SHAPE,DS.DPCS,DS.DWT,DS.DRATE,DS.DVALUE"
        strSql += vbCrLf + " ,DS.SPCS,DS.SWT,DS.SRATE,DS.SVALUE"
        strSql += vbCrLf + " INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET_ISSUE AS ISS"
        strSql += vbCrLf + " FULL JOIN MASTER..TEMP_WSHEET_STUD AS DS ON DS.SLNO = ISS.SLNO AND DS.SNO = ISS.SNO "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET SET RESULT = 1 ,SNO = DSNO WHERE SNO IS NULL"
        strSql += vbCrLf + " UPDATE MASTER..TEMP_WSHEET SET TEMP_ITEM = (SELECT TEMP_ITEM FROM MASTER..TEMP_WSHEET WHERE SNO = O.SNO AND ISNULL(TEMP_ITEM,'') <> '')"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET AS O"
        strSql += vbCrLf + " WHERE ISNULL(O.TEMP_ITEM,'') = ''"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..TEMP_WSHEET)>0"
        strSql += vbCrLf + " BEGIN /** INSERTING SUB & GRAND TOT **/"
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " (RESULT,COLHEAD,TEMP_ITEM,TAGNO"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,WASTAGE,GVALUE,LABOUR,TOTAL"
        strSql += vbCrLf + " ,DPCS,DWT,DRATE,DVALUE"
        strSql += vbCrLf + " ,SPCS,SWT,SRATE,SVALUE)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 2 RESULT,'S'COLHEAD,TEMP_ITEM,'SUB TOTAL'"
        strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(WASTAGE),SUM(GVALUE),SUM(LABOUR),SUM(TOTAL)"
        strSql += vbCrLf + " ,SUM(DPCS),SUM(DWT),SUM(DRATE),SUM(DVALUE)"
        strSql += vbCrLf + " ,SUM(SPCS),SUM(SWT),SUM(SRATE),SUM(SVALUE)"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET "
        strSql += vbCrLf + " WHERE RESULT = 1"
        strSql += vbCrLf + " GROUP BY TEMP_ITEM"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " INSERT INTO MASTER..TEMP_WSHEET"
        strSql += vbCrLf + " (RESULT,COLHEAD,TEMP_ITEM,TAGNO"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,WASTAGE,GVALUE,LABOUR,TOTAL"
        strSql += vbCrLf + " ,DPCS,DWT,DRATE,DVALUE"
        strSql += vbCrLf + " ,SPCS,SWT,SRATE,SVALUE)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 3 RESULT,'G'COLHEAD,'ZZZZZZZ'TEMP_ITEM,'GRAND TOTAL'"
        strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(WASTAGE),SUM(GVALUE),SUM(LABOUR),SUM(TOTAL)"
        strSql += vbCrLf + " ,SUM(DPCS),SUM(DWT),SUM(DRATE),SUM(DVALUE)"
        strSql += vbCrLf + " ,SUM(SPCS),SUM(SWT),SUM(SRATE),SUM(SVALUE)"
        strSql += vbCrLf + " FROM MASTER..TEMP_WSHEET "
        strSql += vbCrLf + " WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE MASTER..TEMP_WSHEET SET"
        strSql += vbCrLf + " PCS = CASE WHEN PCS = 0 THEN NULL ELSE PCS END"
        strSql += vbCrLf + " ,GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
        strSql += vbCrLf + " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
        strSql += vbCrLf + " ,WASTAGE = CASE WHEN WASTAGE = 0 THEN NULL ELSE WASTAGE END"
        strSql += vbCrLf + " ,GVALUE = CASE WHEN GVALUE  = 0 THEN NULL ELSE GVALUE END"
        strSql += vbCrLf + " ,LABOUR = CASE WHEN LABOUR  = 0 THEN NULL ELSE LABOUR END"
        strSql += vbCrLf + " ,TOTAL = CASE WHEN TOTAL  = 0 THEN NULL ELSE TOTAL END"
        strSql += vbCrLf + " ,DPCS = CASE WHEN DPCS  = 0 THEN NULL ELSE DPCS END"
        strSql += vbCrLf + " ,DWT = CASE WHEN DWT  = 0 THEN NULL ELSE DWT END"
        strSql += vbCrLf + " ,DRATE = CASE WHEN DRATE  = 0 THEN NULL ELSE DRATE END"
        strSql += vbCrLf + " ,DVALUE = CASE WHEN DVALUE  = 0 THEN NULL ELSE DVALUE END"
        strSql += vbCrLf + " ,SPCS = CASE WHEN SPCS  = 0 THEN NULL ELSE SPCS END"
        strSql += vbCrLf + " ,SWT = CASE WHEN SWT  = 0 THEN NULL ELSE SWT END"
        strSql += vbCrLf + " ,SRATE = CASE WHEN SRATE  = 0 THEN NULL ELSE SRATE END"
        strSql += vbCrLf + " ,SVALUE = CASE WHEN SVALUE  = 0 THEN NULL ELSE SVALUE END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "SELECT * FROM MASTER..TEMP_WSHEET ORDER BY TEMP_ITEM,RESULT,SNO,SLNO DESC"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        With dtGrid
            .Columns("GRSWT").SetOrdinal(dtGrid.Columns.Count - 1)
            .Columns("NETWT").SetOrdinal(dtGrid.Columns.Count - 1)
            .Columns("WASTAGE").SetOrdinal(dtGrid.Columns.Count - 1)
            .Columns("GVALUE").SetOrdinal(dtGrid.Columns.Count - 1)
            .Columns("LABOUR").SetOrdinal(dtGrid.Columns.Count - 1)
            .Columns("TOTAL").SetOrdinal(dtGrid.Columns.Count - 1)
        End With
        Dim objGridShower As New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "PACKING MATERIAL"
        Dim dtTit As New DataTable
        Dim tit As String = "PACKING MATERIAL"
        Dim Name As String = ""
        Dim GoldRate As String = ""
        tit += " DATE : " & dtpBillDate.Text & " BILLNO : " & objGPack.GetSqlValue("SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "' AND COSTID = '" & costId & "'") + vbCrLf
        strSql = vbCrLf + " SELECT DISTINCT RATE FROM " & cnStockDb & "..ISSUE AS ISS"
        strSql += vbCrLf + " WHERE ISS.BATCHNO = '" & batchno & "' AND ISNULL(ISS.COSTID,'') = '" & costId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTit)
        tit += " Gold Pure  : " & Format(Val(GetRate_Purity(dtpBillDate.Value, "01")), "0.00")
        tit += " ,Gold Charg : "
        If dtTit.Rows.Count > 0 Then
            tit += dtTit.Rows(0).Item(0).ToString + ","
        End If
        tit = Mid(tit, 1, tit.Length - 1) + vbCrLf
        strSql = " SELECT CASE WHEN ISNULL(PREVID,'') <> '' THEN 'PREVILEGEID : ' + PREVID ELSE NAME END"
        strSql += vbCrLf + " FROM"
        strSql += " ("
        strSql += " SELECT 'PARTY : ' + PNAME + CASE WHEN ISNULL(ACCODE,'') <> '' THEN '['+ACCODE+']' ELSE '' END AS NAME "
        strSql += vbCrLf + " ,(SELECT PREVILEGEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = P.ACCODE)AS PREVID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS P"
        strSql += " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchno & "')"
        strSql += " )X"
        Name += objGPack.GetSqlValue(strSql) + vbCrLf
        tit += Name
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        PackingGridFormat(objGridShower.gridView)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[SLNO],''[STYLENO],''[TAGNO],''[CERTIFICATENO],''[BRANCH],''[ITEM~SUBITEM~PCS]"
        strSql += " ,''[SHAPE~DPCS~DWT~DRATE~DVALUE]"
        strSql += " ,''[SPCS~SWT~SRATE~SVALUE]"
        strSql += " ,''[GRSWT~NETWT~WASTAGE~GVALUE]"
        strSql += " ,''[LABOUR]"
        strSql += " ,''[TOTAL]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("ITEM~SUBITEM~PCS").HeaderText = "ITEM"
        gridviewHead.Columns("SHAPE~DPCS~DWT~DRATE~DVALUE").HeaderText = "DIAMOND"
        gridviewHead.Columns("SPCS~SWT~SRATE~SVALUE").HeaderText = "STONE"
        gridviewHead.Columns("GRSWT~NETWT~WASTAGE~GVALUE").HeaderText = "GOLD"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub



    Private Sub PackingGridFormat(ByVal Dgv As DataGridView)
        Dgv.Columns("DSNO").Visible = False
        Dgv.Columns("SNO").Visible = False
        Dgv.Columns("RESULT").Visible = False
        Dgv.Columns("COLHEAD").Visible = False
        Dgv.Columns("TEMP_ITEM").Visible = False
        Dgv.Columns("KEYNO").Visible = False
        Dgv.Columns("DPCS").HeaderText = "PCS"
        Dgv.Columns("DWT").HeaderText = "WT"
        Dgv.Columns("DRATE").HeaderText = "RATE"
        Dgv.Columns("DVALUE").HeaderText = "VALUE"
        Dgv.Columns("SPCS").HeaderText = "PCS"
        Dgv.Columns("SWT").HeaderText = "WT"
        Dgv.Columns("SRATE").HeaderText = "RATE"
        Dgv.Columns("SVALUE").HeaderText = "VALUE"
        BrighttechPack.FormatGridColumns(Dgv)
    End Sub

#End Region



    Private Sub frmBillToJnt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dtpBillDate.Enabled = True Then dtpBillDate.Focus()
        ElseIf e.KeyCode = Keys.S Then
            If e.Control Then
                ''STUDDED DETAIL
                If Not gridFullView.Rows.Count > 0 Then Exit Sub
                If gridFullView.CurrentRow Is Nothing Then Exit Sub
                Dim objStone As New frmStoneDia
                strSql = " SELECT 0 KEYNO,TRANTYPE"
                strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
                strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
                strSql += " ,STNPCS PCS,STNWT WEIGHT,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT,''METALID,STNPCS TAGSTNPCS,STNWT TAGSTNWT"
                objStone.CalcStoneWtAmount()
                Select Case gridFullView.CurrentRow.Cells("TRANTYPE").Value.ToString
                    Case "SA", "MI", "OD", "RD", "AI"
                        strSql += " FROM " & cnStockDb & "..ISSSTONE"
                    Case "SR", "AR", "PU"
                        strSql += " FROM " & cnStockDb & "..RECEIPTSTONE"
                    Case Else
                        Exit Sub
                End Select
                strSql += " WHERE ISSSNO = '" & gridFullView.CurrentRow.Cells("SNO").Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(objStone.dtGridStone)
                If objStone.dtGridStone.Rows.Count > 0 Then
                    objStone.CalcStoneWtAmount()
                    objStone.StyleGridStone(objStone.gridStone)
                    objStone.StartPosition = FormStartPosition.CenterScreen
                    objStone.ShowDialog()
                End If
            End If

        End If
    End Sub

    Function StyleGridFullView() As Integer
        With gridFullView
            .Columns("sno").Visible = False
            With .Columns("ITRANNO")
                .HeaderText = "BILLNO"
                .Width = 65
                .Visible = True
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("ITEMID")
                .HeaderText = "ID"
                .Width = 50
                .Visible = True
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("ITAGNO")
                .HeaderText = "TAGNO"
                .Width = 70
                .Visible = True
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("IPCS")
                .HeaderText = "PCS"
                .Width = 55
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
            End With
            With .Columns("IGRSWT")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("INETWT")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .DefaultCellStyle.Format = "#,##0.000"
            End With

            With .Columns("IAMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("RTRANNO")
                .HeaderText = "BILLNO"
                .Width = 65
                .Visible = True
                .DefaultCellStyle.BackColor = Color.LemonChiffon
            End With



            .Columns("RTAGNO").Visible = False
            .Columns("RPCS").Visible = False
            With .Columns("RGRSWT")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.LemonChiffon
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("RNETWT")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.LemonChiffon
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("RAMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.LemonChiffon
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            For cnt As Integer = 14 To .ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Function

    Function funcGridHeadStyle() As Integer
        dtHead.Clear()
        gridHead.DataSource = Nothing
        strSql = " SELECT ''ISSUE,''RECEIPT WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridHead.DataSource = dtHead
        With gridHead
            With .Columns("ISSUE")
                .HeaderText = "SALES/RECEIPTS"
                .Width = gridFullView.Columns("ITRANNO").Width + gridFullView.Columns("ITEMID").Width + gridFullView.Columns("ITAGNO").Width + gridFullView.Columns("IPCS").Width + gridFullView.Columns("IGRSWT").Width + gridFullView.Columns("INETWT").Width + gridFullView.Columns("IAMOUNT").Width
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RECEIPT")
                .HeaderText = "PURCHASE/PAYMENTS"
                .Width = gridFullView.Columns("RTRANNO").Width + gridFullView.Columns("RGRSWT").Width + gridFullView.Columns("RNETWT").Width + gridFullView.Columns("RAMOUNT").Width
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function

    Function StyleGridTot() As Integer
        With gridTot
            With .Columns("iDes")
                .Width = 185
                .Visible = True
            End With
            With .Columns("rDes")
                .Width = 65
                .Visible = True
            End With
            With .Columns("iPcs")
                .HeaderText = "PCS"
                .Width = 55
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iGrsWt")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iNETwt")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iAmount")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            '''''''''''''''
            With .Columns("rGrsWt")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("rNETwt")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("rAmount")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
        End With
    End Function


    Function funcSetOtherDetailsStyle() As Integer
        With gridMiscDetails
            With .Columns("Col1")
                .Width = 100
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col2")
                .Width = 125
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col3")
                .Width = 100
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
            End With
            With .Columns("Col4")
                .Width = 125
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col5")
                .Width = 100
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col6")
                .Width = 100
                .Resizable = DataGridViewTriState.False
            End With
        End With
    End Function

    Function funcClearOtherDetails() As Integer
        strSql = " Select 'PRODUCT'Col1,''Col2,'COUNTER'Col3,''Col4,'CASH'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'SUB PRODUCT'Col1,''Col2,'CATEGORY'Col3,''Col4,'CHEQUE'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'TAG WEIGHT'Col1,''Col2,'PUR.WASTAGE'Col3,''Col4,'CARD'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strSql += vbCrLf + "  Select 'DIA WEIGHT'Col1,''Col2,'PUR.TAX'Col3,''Col4,'SCHEME'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'MK CHARGE'Col1,''Col2,'PUR.SC'Col3,''Col4,'CREDIT'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'WASTAGE'Col1,''Col2,'PUR.ASC'Col3,''Col4,'TOBE AMT'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'RATE'Col1,''Col2,'TOT.PUR.TAX'Col3,''Col4,'ADVANCE'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'RATE ID'Col1,''Col2,'TOT.PUR.AMT'Col3,''Col4,'DISCOUNT'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'STONE AMT'Col1,''Col2,'SALES-PUR'Col3,''Col4,'HAND CHARGE'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'OTHER CHAR'Col1,''Col2,'BILL STATUS'Col3,''Col4,'TOTAL'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'TAX'Col1,''Col2,'ORDER/REPAIR'Col3,''Col4,'UPDATED'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'SC'Col1,''Col2,'REFERENCE'Col3,''Col4,'NARATION'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'ASC'Col1,''Col2,'OPERATOR'Col3,''Col4,'BILL-REMARK'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'TOT TAX'Col1,''Col2,'SALES PERSON'Col3,''Col4,'CASH COUNTER'Col5,''Col6"
        strsql += vbcrlf + "  UNION ALL"
        strsql += vbcrlf + "  Select 'SALE AMT'Col1,''Col2,'DISC AUTORIZE'Col3,''Col4,'BATCHNO'Col5,''Col6"
        dtOtherDetails.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOtherDetails)
        gridMiscDetails.DataSource = dtOtherDetails
        funcSetOtherDetailsStyle()
    End Function

    Private Sub frmBillToJnt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBillToJnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)


            ''cn = New OleDbConnection("Provider=SQLOLEDB.1;Persist Security Info=False;User ID=SA;Initial Catalog=MASTER;Data Source=ADMIN")
            If cmbCostCentre.Enabled = True Then
                strSql = " select CostName from " & cnAdminDb & "..CostCentre order by CostName"
                cmbCostCentre.Items.Clear()
                cmbCostCentre.Items.Add("ALL")
                objGPack.FillCombo(strSql, cmbCostCentre, False)
                cmbCostCentre.Text = "ALL"
            End If

            If cmbCashCounter.Enabled = True Then
                strSql = " select CASHNAME from " & cnAdminDb & "..CASHCOUNTER order by CASHNAME"
                cmbCashCounter.Items.Clear()
                cmbCashCounter.Items.Add("ALL")
                objGPack.FillCombo(strSql, cmbCashCounter, False)
                cmbCashCounter.Text = "ALL"
            End If

            strSql = "select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'COSTCENTRE' and ctlText = 'Y'"
            Dim dtCostCentreCheck As New DataTable
            dtCostCentreCheck.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentreCheck)
            If dtCostCentreCheck.Rows.Count > 0 Then
                cmbCostCentre.Enabled = True
            Else
                cmbCostCentre.Text = "ALL"
                cmbCostCentre.Enabled = False
            End If

            strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
            cmbMetalName.Items.Clear()
            cmbMetalName.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbMetalName, False)
            cmbMetalName.Text = "ALL"

            strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(COSTID,'') = '" & cnCostId & "' AND ISNULL(ACTIVE,'') = 'Y' "
            cmbUserName.Items.Clear()
            cmbUserName.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbUserName, False)
            cmbUserName.Text = "ALL"

            ''OtherTable
            funcClearOtherDetails()

            'pnlGrid.Dock = DockStyle.Fill
            'pnlGrid.Visible = False
            'btnView.Focus()
            dtpBillDate.Value = GetEntryDate(GetServerDate)


            txtAddress_OWN.BackColor = Me.BackColor
            gridMiscDetails.BackgroundColor = Me.BackColor
            gridFullView.RowTemplate.Height = 21

            If Not PicPath.EndsWith("\") And PicPath <> "" Then PicPath += "\"
            If Iscurrdate And Not Authorize Then dtpBillDate.Enabled = False Else dtpBillDate.Enabled = True
            If dtpBillDate.Enabled Then dtpBillDate.Focus() Else txtSystemId.Focus()
            If Authorize Then Me.BtnAdvanced.Visible = True Else BtnAdvanced.Visible = False
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub CreateIssRecView()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISSRECVIEW') > 0 DROP TABLE TEMP" & systemId & "ISSRECVIEW"
        strsql += vbcrlf + "  CREATE TABLE TEMP" & systemId & "ISSRECVIEW"
        strsql += vbcrlf + "  ("
        strsql += vbcrlf + "  SNO VARCHAR(15)"
        strsql += vbcrlf + "  ,ITRANNO VARCHAR(15)"
        strsql += vbcrlf + "  ,ITEMID INT"
        strsql += vbcrlf + "  ,ITAGNO VARCHAR(12)"
        strsql += vbcrlf + "  ,IPCS INT"
        strsql += vbcrlf + "  ,IGRSWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,INETWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,IAMOUNT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,RTRANNO VARCHAR(15)"
        strsql += vbcrlf + "  ,RTAGNO VARCHAR(12)"
        strsql += vbcrlf + "  ,RPCS INT"
        strsql += vbcrlf + "  ,RGRSWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,RNETWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,RAMOUNT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,ITEM VARCHAR(30)"
        strsql += vbcrlf + "  ,SUBITEM VARCHAR(50)"
        strsql += vbcrlf + "  ,DIAWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,TAGGRSWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,TAGNETWT NUMERIC(12,3)"
        strsql += vbcrlf + "  ,MCHARGE NUMERIC(15,2)"
        strsql += vbcrlf + "  ,WASTAGE NUMERIC(12,3)"
        strsql += vbcrlf + "  ,RATE NUMERIC(15,2)"
        strsql += vbcrlf + "  ,STNPCS INT"
        strsql += vbcrlf + "  ,STNWT NUMERIC(13,2)"
        strsql += vbcrlf + "  ,TAGSTNPCS INT"
        strsql += vbcrlf + "  ,TAGSTNWT NUMERIC(13,2)"
        strsql += vbcrlf + "  ,STNAMT NUMERIC(15,2)"
        strSql += vbCrLf + "  ,MISCAMT NUMERIC(15,2)"
        strSql += vbCrLf + "  ,FINDISCOUNT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,COUNTER VARCHAR(30)"
        strsql += vbcrlf + "  ,CATEGORY VARCHAR(50)"
        strsql += vbcrlf + "  ,TRANTYPE VARCHAR(4)"
        strSql += vbCrLf + "  ,BATCHNO VARCHAR(15)"
        strsql += vbcrlf + "  ,EMPNAME VARCHAR(25)"
        strsql += vbcrlf + "  ,USERNAME VARCHAR(25)"
        strsql += vbcrlf + "  ,REFERENCE VARCHAR(35)"
        strsql += vbcrlf + "  ,REMARK1 VARCHAR (50)"
        strsql += vbcrlf + "  ,REMARK2 VARCHAR (50)"
        strsql += vbcrlf + "  ,UPDATED VARCHAR(12)"
        strsql += vbcrlf + "  ,UPTIME VARCHAR(12)"
        strsql += vbcrlf + "  ,CANCEL VARCHAR(10)"
        strsql += vbcrlf + "  ,TRANMODE VARCHAR(25)"
        strsql += vbcrlf + "  ,RESULT INT"
        strsql += vbcrlf + "  ,PARTLY VARCHAR(1)"
        strsql += vbcrlf + "  ,COSTID VARCHAR(2)"
        strsql += vbcrlf + "  ,SALES NUMERIC(15,2)"
        strsql += vbcrlf + "  ,SALESVAT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,SRETURN NUMERIC(15,2)"
        strsql += vbcrlf + "  ,RETURNVAT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,PURCHASE NUMERIC(15,2)"
        strsql += vbcrlf + "  ,PURCHASEVAT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CASH NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CHEQUE NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CARD NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CHITCARD NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CREDIT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,ADVANCE NUMERIC(15,2)"
        strsql += vbcrlf + "  ,DISCOUNT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,HANDLING NUMERIC(15,2)"
        strsql += vbcrlf + "  ,GIFT NUMERIC(15,2)"
        strsql += vbcrlf + "  ,CASHCOUNTER VARCHAR(25)"
        strSql += vbCrLf + "  ,FROMFLAG VARCHAR(1)"
        strSql += vbCrLf + "  ,ORDREPNO VARCHAR(15)"
        strSql += vbCrLf + "  ,PICPATH VARCHAR(300)"
        strSql += vbCrLf + "  )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Function ChitPayment() As String
        Dim qry As String = ""
        Qry += vbCrLf + "  SELECT SNO,NULL ITRANNO,NULL ITEMID" + vbCrLf
        Qry += vbCrLf + "  ,NULL ITAGNO,NULL IPCS,NULL IGRSWT,NULL INETWT,NULL IAMOUNT" + vbCrLf
        qry += vbCrLf + "  ,CONVERT(VARCHAR(20),TRANNO) RTRANNO,NULL RTAGNO,NULL RPCS,NULL RGRSWT,NULL RNETWT"
        qry += vbCrLf + "  ,(SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE IN ('HB','HZ','HG','HD','HP')) RAMOUNT" + vbCrLf
        Qry += vbCrLf + "  ,NULL AS ITEM,NULL AS SUBITEM" + vbCrLf
        qry += vbCrLf + "  ,NULL TAGGRSWT,NULL TAGNETWT,NULL MCHARGE,NULL WASTAGE,NULL RATE,NULL DIAWT,NULL STNPCS,NULL STNWT,NULL TAGSTNPCS,NULL TAGSTNWT,NULL STNAMT,NULL MISCAMT,'0.00' FINDISCOUNT" + vbCrLf
        Qry += vbCrLf + "  ,NULL COUNTER,NULL AS CATEGORY" + vbCrLf
        Qry += vbCrLf + "  ,'P' + CASE WHEN PAYMODE = 'OR' THEN 'F' ELSE '' END + PAYMODE AS TRANTYPE" + vbCrLf
        Qry += vbCrLf + "  ,BATCHNO,NULL AS EMPNAME" + vbCrLf
        Qry += vbCrLf + "  ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME" + vbCrLf
        Qry += vbCrLf + "  ,REFNO + CASE WHEN REFNO <> '' THEN ';' ELSE '' END + CONVERT(VARCHAR,REFDATE,103)REFERENCE" + vbCrLf
        Qry += vbCrLf + "  ,REMARK1" + vbCrLf
        Qry += vbCrLf + "  ,REMARK2" + vbCrLf
        qry += vbCrLf + "  ,UPDATED,UPTIME,CANCEL,COSTID,CONVERT(VARCHAR,NULL)PICPATH,0 AS JND" + vbCrLf
        Qry += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN I" + vbCrLf
        qry += vbCrLf + "  WHERE  TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND PAYMODE IN ('HP'" + vbCrLf
        qry += vbCrLf + "  ) "
        If txtSystemId.Text <> "" Then qry += vbCrLf + "  AND SYSTEMID = '" & txtSystemId.Text & "'" + vbCrLf
        If txtBillNo.Text <> "" Then qry += vbCrLf + "  AND CONVERT(VARCHAR,TRANNO) = '" & txtBillNo.Text & "'" + vbCrLf
        If cmbCostCentre.Enabled And cmbCostCentre.Text <> "ALL" Then qry += vbCrLf + "  AND COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") & "'" + vbCrLf
        If cmbCashCounter.Enabled And cmbCashCounter.Text <> "ALL" Then strSql += vbCrLf + "  AND CASHID = '" & objGPack.GetSqlValue("SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'") & "'"
        If txtCustName.Text <> "" Then
            qry += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO IN (SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME LIKE '" & txtCustName.Text & "%'))"
        End If
        If cmbUserName.Enabled And cmbUserName.Text <> "ALL" Then strSql += vbCrLf + "  AND USERID = " & musrid
        qry += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        qry += vbCrLf + "  AND TRANMODE = 'D'"
        qry += " UNION ALL"
        Return qry
    End Function

    Private Function OrderQry() As String
        Dim qry As String = Nothing
        qry += " SELECT SNO,CONVERT(VARCHAR(20),TRANNO) ITRANNO,ITEMID" + vbCrLf
        qry += " ,TAGNO ITAGNO,PCS IPCS,GRSWT IGRSWT,NETWT INETWT,AMOUNT IAMOUNT" + vbCrLf
        qry += " ,NULL RTRANNO,NULL RTAGNO,NULL RPCS,NULL RGRSWT,NULL RNETWT,NULL RAMOUNT" + vbCrLf
        qry += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM" + vbCrLf
        qry += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)AS SUBITEM" + vbCrLf
        qry += " ,TAGGRSWT,TAGNETWT,MCHARGE,WASTAGE,RATE"
        qry += " ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        qry += " ,(SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNPCS"
        qry += " ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNWT"
        qry += " ,(SELECT SUM(ISNULL(TAGSTNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNPCS"
        qry += " ,(SELECT SUM(ISNULL(TAGSTNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNWT"
        qry += " ,STNAMT,MISCAMT,DISCOUNT FINDISCOUNT " + vbCrLf
        qry += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER" + vbCrLf
        qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATEGORY" + vbCrLf
        qry += " ,CONVERT(VARCHAR(3),'OD')TRANTYPE" + vbCrLf
        qry += " ,BATCHNO" + vbCrLf
        qry += " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPNAME" + vbCrLf
        qry += " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME" + vbCrLf
        qry += " ,REFNO + CASE WHEN REFNO <> '' THEN ';' ELSE '' END + CONVERT(VARCHAR,REFDATE,103)REFERENCE" + vbCrLf
        qry += " ,REMARK1" + vbCrLf
        qry += " ,REMARK2" + vbCrLf
        qry += " ,UPDATED,UPTIME,CANCEL,COSTID" + vbCrLf
        qry += "  ,(SELECT PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICPATH" + vbCrLf
        qry += "  ,(SELECT 1 FROM " & cnAdminDb & "..ITEMDETAIL WHERE ISSSNO = I.SNO)AS JND"
        qry += " FROM " & cnStockDb & "..ISSUE I WHERE 1=1 " + vbCrLf
        If Advsearch1 = "" Then
            qry += "  AND  TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
        Else
            qry += Advsearch1
        End If
        If Advsearch2 <> "" Then qry += Advsearch2

        'qry += " WHERE TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "'"

        qry += " AND BATCHNO IN (SELECT DISTINCT ODBATCHNO FROM " & cnadmindb & "..ORMAST WHERE ISNULL(ODBATCHNO,'') <> '' AND ORTYPE = 'O')"
        If txtSystemId.Text <> "" Then qry += " AND SYSTEMID = '" & txtSystemId.Text & "'" + vbCrLf
        If txtBillNo.Text <> "" Then qry += " AND CONVERT(VARCHAR,TRANNO) = '" & txtBillNo.Text & "'" + vbCrLf
        If txtItemId.Text <> "" Then qry += " AND ITEMID = " & Val(txtItemId.Text) & "" + vbCrLf
        If cmbMetalName.Text <> "" And cmbMetalName.Text <> "ALL" Then qry += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'))" + vbCrLf
        If cmbCostCentre.Enabled And cmbCostCentre.Text <> "ALL" Then qry += " AND COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") & "'" + vbCrLf
        If cmbCashCounter.Enabled And cmbCashCounter.Text <> "ALL" Then qry += "  AND CASHID = '" & objGPack.GetSqlValue("SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'") & "'"
        If txtCustName.Text <> "" Then
            qry += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO IN (SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME LIKE '" & txtCustName.Text & "%'))"
        End If
        If cmbUserName.Enabled And cmbUserName.Text <> "ALL" Then qry += vbCrLf + "  AND USERID = " & musrid
        qry += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        qry += " UNION ALL"
        Return qry
    End Function

    Private Function RepairQry() As String
        Dim qry As String = Nothing
        qry += " SELECT SNO,CONVERT(VARCHAR(20),TRANNO) ITRANNO,ITEMID" + vbCrLf
        qry += " ,TAGNO ITAGNO,PCS IPCS,GRSWT IGRSWT,NETWT INETWT,AMOUNT IAMOUNT" + vbCrLf
        qry += " ,NULL RTRANNO,NULL RTAGNO,NULL RPCS,NULL RGRSWT,NULL RNETWT,NULL RAMOUNT" + vbCrLf
        qry += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM" + vbCrLf
        qry += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)AS SUBITEM" + vbCrLf
        qry += " ,TAGGRSWT,TAGNETWT,MCHARGE,WASTAGE,RATE"
        qry += " ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        qry += " ,(SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNPCS"
        qry += " ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNWT"
        qry += " ,(SELECT SUM(ISNULL(TAGSTNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNPCS"
        qry += " ,(SELECT SUM(ISNULL(TAGSTNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNWT"
        qry += " ,STNAMT,MISCAMT,DISCOUNT FINDISCOUNT " + vbCrLf
        qry += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER" + vbCrLf
        qry += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATEGORY" + vbCrLf
        qry += " ,CONVERT(VARCHAR(3),'RD')TRANTYPE" + vbCrLf
        qry += " ,BATCHNO" + vbCrLf
        qry += " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPNAME" + vbCrLf
        qry += " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME" + vbCrLf
        qry += " ,REFNO + CASE WHEN REFNO <> '' THEN ';' ELSE '' END + CONVERT(VARCHAR,REFDATE,103)REFERENCE" + vbCrLf
        qry += " ,REMARK1" + vbCrLf
        qry += " ,REMARK2" + vbCrLf
        qry += " ,UPDATED,UPTIME,CANCEL,COSTID" + vbCrLf
        qry += vbCrLf + "  ,(SELECT PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICPATH" + vbCrLf
        qry += vbCrLf + "  ,(SELECT 1 FROM " & cnAdminDb & "..ITEMDETAIL WHERE ISSSNO = I.SNO)AS JND"
        qry += " FROM " & cnStockDb & "..ISSUE I WHERE 1=1" + vbCrLf
        If Advsearch1 = "" Then
            qry += "  AND  TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
        Else
            qry += Advsearch1
        End If
        If Advsearch2 <> "" Then qry += Advsearch2

        '        qry += " WHERE TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "'"
        qry += " AND BATCHNO IN (SELECT DISTINCT ODBATCHNO FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ODBATCHNO,'') <> '' AND ORTYPE = 'R')"
        If txtSystemId.Text <> "" Then qry += " AND SYSTEMID = '" & txtSystemId.Text & "'" + vbCrLf
        If txtBillNo.Text <> "" Then qry += " AND CONVERT(VARCHAR,TRANNO) = '" & txtBillNo.Text & "'" + vbCrLf
        If txtItemId.Text <> "" Then qry += " AND ITEMID = " & Val(txtItemId.Text) & "" + vbCrLf
        If cmbMetalName.Text <> "" And cmbMetalName.Text <> "ALL" Then qry += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'))" + vbCrLf
        If cmbCostCentre.Enabled And cmbCostCentre.Text <> "ALL" Then qry += " AND COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") & "'" + vbCrLf
        If cmbCashCounter.Enabled And cmbCashCounter.Text <> "ALL" Then qry += "  AND CASHID = '" & objGPack.GetSqlValue("SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'") & "'"
        If txtCustName.Text <> "" Then
            qry += vbCrLf + "  AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO IN (SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME LIKE '" & txtCustName.Text & "%'))"
        End If
        If cmbUserName.Enabled And cmbUserName.Text <> "ALL" Then qry += vbCrLf + "  AND USERID = " & musrid
        qry += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        qry += " UNION ALL"
        Return qry
    End Function


    Private Sub InsertIssRecView()
        Dim issType As String = ""
        Dim recType As String = ""
        Dim recTranType As String = ""
        Dim payTranType As String = ""
        Dim ordType As String = ""
        Dim repType As String = ""
        issType = "'SA','MI','AI','OD','RD'"
        recType = "'SR','PU','AR'"
        recTranType = "'DR','AR','MR','OR'"
        payTranType = "'DP','AP','MP'"
        ordType = "OD"
        repType = "RD"

        strSql = " INSERT INTO TEMP" & systemId & "ISSRECVIEW"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SNO,ITRANNO,ITEMID"
        strSql += vbCrLf + "  ,ITAGNO,IPCS,IGRSWT,INETWT,IAMOUNT"
        strSql += vbCrLf + "  ,RTRANNO,RTAGNO,RPCS,RGRSWT,RNETWT,RAMOUNT"
        strSql += vbCrLf + "  ,ITEM,SUBITEM,TAGGRSWT,TAGNETWT,MCHARGE,WASTAGE"
        strSql += vbCrLf + "  ,RATE,DIAWT,STNPCS,STNWT,TAGSTNPCS,TAGSTNWT,STNAMT,MISCAMT,FINDISCOUNT,COUNTER,CATEGORY"
        strSql += vbCrLf + "  ,TRANTYPE,BATCHNO,EMPNAME,USERNAME,REFERENCE"
        strSql += vbCrLf + "  ,REMARK1,REMARK2,UPDATED,CANCEL,TRANMODE,RESULT,COSTID,UPTIME,PICPATH"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  SNO,CONVERT(VARCHAR(20),ITRANNO)ITRANNO,CASE WHEN ITEMID <> 0 THEN ITEMID ELSE NULL END AS ITEMID"
        strSql += vbCrLf + "  ,ITAGNO,IPCS,IGRSWT,INETWT,IAMOUNT"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),RTRANNO)RTRANNO,RTAGNO,RPCS,RGRSWT,RNETWT,RAMOUNT"
        strSql += vbCrLf + "  ,ITEM,SUBITEM,TAGGRSWT,TAGNETWT,MCHARGE,WASTAGE"
        strSql += vbCrLf + "  ,RATE,DIAWT,STNPCS,STNWT,TAGSTNPCS,TAGSTNWT"
        strSql += vbCrLf + "  ,STNAMT,MISCAMT,FINDISCOUNT,COUNTER,CATEGORY"
        strSql += vbCrLf + "  ,TRANTYPE,BATCHNO,EMPNAME,USERNAME,REFERENCE"
        strSql += vbCrLf + "  ,REMARK1,REMARK2,CONVERT(VARCHAR(12),UPDATED,103)UPDATED,CASE WHEN CANCEL = 'Y' THEN 'CANCEL' ELSE CANCEL END AS CANCEL"
        strSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'SA' THEN 'SALES' + CASE WHEN ISNULL(JND,0) = 1 THEN '-JND' ELSE '' END"
        strSql += vbCrLf + "                 WHEN 'MI' THEN 'MISC ISSUE'"
        strSql += vbCrLf + "                 WHEN 'AI' THEN 'APPROVAL ISSUE'"
        strSql += vbCrLf + "                 WHEN 'SR' THEN 'RETURN'"
        strSql += vbCrLf + "                 WHEN 'PU' THEN 'PURCHASE'"
        strSql += vbCrLf + "                 WHEN 'AR' THEN 'APPROVAL RECEIPT'"
        strSql += vbCrLf + "                 WHEN 'RDR' THEN 'REC-CREDIT'"
        strSql += vbCrLf + "                 WHEN 'RAR' THEN 'REC-ADVANCE'"
        strSql += vbCrLf + "                 WHEN 'RMR' THEN 'REC-OTHER'"
        strSql += vbCrLf + "                 WHEN 'PDP' THEN 'PAY-PU/RETURN'"
        strSql += vbCrLf + "                 WHEN 'PAP' THEN 'PAY-ADV REPAY'"
        strSql += vbCrLf + "                 WHEN 'PMP' THEN 'PAY-OTHER'"
        strSql += vbCrLf + "                 WHEN 'OD' THEN 'ORDER DELIVERY' + CASE WHEN ISNULL(JND,0) = 1 THEN '-JND' ELSE '' END"
        strSql += vbCrLf + "                 WHEN 'RD' THEN 'REPAIR DELIVERY' + CASE WHEN ISNULL(JND,0) = 1 THEN '-JND' ELSE '' END"
        strSql += vbCrLf + "                 WHEN 'OB' THEN 'ORDER BOOKING'"
        strSql += vbCrLf + "                 WHEN 'RB' THEN 'REPAIR BOOKING'"
        strSql += vbCrLf + "                 WHEN 'RFOR' THEN 'REC-FURTHER'"
        strSql += vbCrLf + "                 WHEN 'RHR' THEN 'SCHEME RECEIPT'"
        strSql += vbCrLf + "                 WHEN 'PHP' THEN 'SCHEME PAYMENT'"
        strSql += vbCrLf + "  	       ELSE TRANTYPE END AS TRANMODE"
        strSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'SA' THEN 1"
        strSql += vbCrLf + "                 WHEN 'SR' THEN 2"
        strSql += vbCrLf + "                 WHEN 'PU' THEN 3"
        strSql += vbCrLf + "                 WHEN 'MI' THEN 4"
        strSql += vbCrLf + "                 WHEN 'AI' THEN 5"
        strSql += vbCrLf + "                 WHEN 'AR' THEN 6"
        strSql += vbCrLf + "                 WHEN 'RDR' THEN 7"
        strSql += vbCrLf + "                 WHEN 'RAR' THEN 7"
        strSql += vbCrLf + "                 WHEN 'RMR' THEN 7"
        strSql += vbCrLf + "                 WHEN 'RFOR'THEN 7"
        strSql += vbCrLf + "                 WHEN 'PDP' THEN 8"
        strSql += vbCrLf + "                 WHEN 'PAP' THEN 8"
        strSql += vbCrLf + "                 WHEN 'PMP' THEN 8"
        strSql += vbCrLf + "                 WHEN 'OD' THEN 9"
        strSql += vbCrLf + "                 WHEN 'RD' THEN 10"
        strSql += vbCrLf + "                 WHEN 'OB' THEN 11"
        strSql += vbCrLf + "                 WHEN 'RB' THEN 12"
        strSql += vbCrLf + "                 WHEN 'RHR' THEN 13"
        strSql += vbCrLf + "                 WHEN 'PHP' THEN 14"
        strSql += vbCrLf + "  	       ELSE 15 END AS RESULT"
        strSql += vbCrLf + "  ,COSTID"
        strSql += vbCrLf + "  ,RTRIM(LTRIM(SUBSTRING(CONVERT(VARCHAR,UPTIME,0),12,10)))UPTIME,PICPATH"
        strSql += vbCrLf + "  FROM "
        strSql += vbCrLf + "  ("
        If issType <> "" Then
            strSql += vbCrLf + "  SELECT I.SNO,CONVERT(VARCHAR(15),I.TRANNO) ITRANNO,ITEMID"
            strSql += vbCrLf + "  ,TAGNO ITAGNO,I.PCS IPCS,I.GRSWT IGRSWT,I.NETWT INETWT,I.AMOUNT IAMOUNT"
            strSql += vbCrLf + "  ,NULL RTRANNO,NULL RTAGNO,NULL RPCS,NULL RGRSWT,NULL RNETWT,NULL RAMOUNT"
            strSql += vbCrLf + "  ,(SELECT top 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            strSql += vbCrLf + "  ,(SELECT top 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + "  ,TAGGRSWT,TAGNETWT,MCHARGE,WASTAGE,I.RATE"
            strSql += vbCrLf + "  ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,(SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS STNWT"
            strSql += vbCrLf + "  ,(SELECT SUM(ISNULL(TAGSTNPCS,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(ISNULL(TAGSTNWT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO)AS TAGSTNWT"
            strSql += vbCrLf + "  ,STNAMT,MISCAMT"
            strSql += vbCrLf + "  ,DISCOUNT FINDISCOUNT"
            strSql += vbCrLf + "  ,(SELECT top 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID)AS COUNTER"
            strSql += vbCrLf + "  ,(SELECT top 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATEGORY"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(3),TRANTYPE)TRANTYPE"
            strSql += vbCrLf + "  ,I.BATCHNO"
            strSql += vbCrLf + "  ,(SELECT top 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPNAME"
            strSql += vbCrLf + "  ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID)AS USERNAME"
            strSql += vbCrLf + "  ,I.REFNO + CASE WHEN I.REFNO <> '' THEN ';' ELSE '' END + CONVERT(VARCHAR,I.REFDATE,103)REFERENCE"
            strSql += vbCrLf + "  ,I.REMARK1"
            strSql += vbCrLf + "  ,I.REMARK2"
            strSql += vbCrLf + "  ,I.UPDATED,I.UPTIME,I.CANCEL,I.COSTID"
            strSql += vbCrLf + "  ,(SELECT top 1 PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PICPATH"
            strSql += vbCrLf + "  ,(SELECT top 1 1 FROM " & cnAdminDb & "..ITEMDETAIL WHERE ISSSNO = I.SNO)AS JND"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + "  LEFT JOIN " & cnStockDb & "..ACCTRAN AC ON I.BATCHNO=AC.BATCHNO"
            strSql += vbCrLf + "  WHERE AC.PAYMODE='DU' "
            If Advsearch1 = "" Then
                strSql += vbCrLf + "  AND  I.TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' "
            Else
                strSql += vbCrLf + Advsearch1
            End If
            If Advsearch2 <> "" Then strSql += vbCrLf + Advsearch2
            strSql += vbCrLf + "  AND I.TRANTYPE IN (" & issType & ""
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  AND I.BATCHNO NOT IN (SELECT DISTINCT ODBATCHNO FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ODBATCHNO,'') <> '')"
            If txtSystemId.Text <> "" Then strSql += vbCrLf + "  AND SYSTEMID = '" & txtSystemId.Text & "'"
            If txtBillNo.Text <> "" Then strSql += vbCrLf + "  AND CONVERT(VARCHAR,I.TRANNO) = '" & txtBillNo.Text & "'"
            If txtItemId.Text <> "" Then strSql += vbCrLf + "  AND I.ITEMID = " & Val(txtItemId.Text) & ""
            If cmbMetalName.Text <> "" And cmbMetalName.Text <> "ALL" Then strSql += vbCrLf + "  AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'))"
            If cmbCostCentre.Enabled And cmbCostCentre.Text <> "ALL" Then strSql += vbCrLf + "  AND I.COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") & "'"
            If cmbCashCounter.Enabled And cmbCashCounter.Text <> "ALL" Then strSql += vbCrLf + "  AND I.CASHID = '" & objGPack.GetSqlValue("SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'") & "'"
            If txtCustName.Text <> "" Then
                strSql += vbCrLf + "  AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO IN (SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME LIKE '" & txtCustName.Text & "%'))"
            End If
            If cmbUserName.Enabled And cmbUserName.Text <> "ALL" Then strSql += vbCrLf + "  AND I.USERID = " & musrid
            strSql += vbCrLf + "  AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  UNION ALL"
        End If
        If ordType <> "" Then strSql += OrderQry()
        If repType <> "" Then strSql += RepairQry()

        If strSql.EndsWith("UNION ALL") Then strSql = Microsoft.VisualBasic.Left(strSql, strSql.Length - 9)
        strSql += vbCrLf + "  )X"
        strSql += vbCrLf + "  ORDER BY BATCHNO,RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub CreateAccView()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCVIEW')> 0 DROP TABLE TEMP" & systemId & "ACCVIEW"
        strsql += vbcrlf + "  SELECT BATCHNO"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'SA' THEN AMOUNT ELSE 0 END)AS SALES"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'SV' THEN AMOUNT ELSE 0 END)AS SALESVAT"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'SR' THEN AMOUNT ELSE 0 END)AS SRETURN"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'RV' THEN AMOUNT ELSE 0 END)AS RETURNVAT"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'PU' THEN AMOUNT ELSE 0 END)AS PURCHASE"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'PV' THEN AMOUNT ELSE 0 END)AS PURCHASEVAT"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE = 'CA' THEN CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END ELSE 0 END)AS CASH"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE = 'CH' THEN CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END ELSE 0 END)AS CHEQUE"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE = 'CC' THEN AMOUNT ELSE 0 END)AS CARD"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('CB','CZ','CG','SS') THEN AMOUNT WHEN PAYMODE = 'CD' THEN -1*AMOUNT ELSE 0 END)AS CHITCARD"
        'strSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND PAYMODE IN ('CB','CZ','CG','CD','SS'))AS CHITCARD"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'DU' THEN AMOUNT ELSE 0 END)AS CREDIT"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'AA' THEN AMOUNT ELSE 0 END)AS ADVANCE"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'DI' THEN AMOUNT ELSE 0 END)AS DISCOUNT"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'HC' THEN AMOUNT ELSE 0 END)AS HANDLING"
        strsql += vbcrlf + "  ,SUM(CASE WHEN PAYMODE = 'GV' AND TRANMODE = 'D' THEN AMOUNT ELSE 0 END)AS GIFT"
        strsql += vbcrlf + "  ,(SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = I.CASHID)AS CASHCOUNTER"
        strsql += vbcrlf + "  ,FROMFLAG"
        strsql += vbcrlf + "  INTO TEMP" & systemId & "ACCVIEW"
        strsql += vbcrlf + "  FROM"
        strsql += vbcrlf + "  ("
        strsql += vbcrlf + "  SELECT BATCHNO,TRANMODE,AMOUNT,PAYMODE,CASHID,FROMFLAG FROM " & cnStockDb & "..ACCTRAN"
        strsql += vbcrlf + "  WHERE TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO IN (SELECT DISTINCT BATCHNO TEMP" & systemId & "ISSRECVIEW)"
        strsql += vbcrlf + "  )I"
        strsql += vbcrlf + "  GROUP BY BATCHNO,CASHID,FROMFLAG"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE TEMP" & systemId & "ACCVIEW SET DISCOUNT=0 WHERE DISCOUNT IS NULL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ISSRECVIEW SET"
        strsql += vbcrlf + "  SALES = A.SALES"
        strsql += vbcrlf + "  ,SALESVAT = A.SALESVAT"
        strsql += vbcrlf + "  ,SRETURN = A.SRETURN"
        strsql += vbcrlf + "  ,RETURNVAT = A.RETURNVAT"
        strsql += vbcrlf + "  ,PURCHASE = A.PURCHASE"
        strsql += vbcrlf + "  ,PURCHASEVAT = A.PURCHASEVAT"
        strsql += vbcrlf + "  ,CASH = A.CASH"
        strsql += vbcrlf + "  ,CHEQUE = A.CHEQUE"
        strsql += vbcrlf + "  ,CARD = A.CARD"
        strsql += vbcrlf + "  ,CHITCARD = A.CHITCARD"
        strsql += vbcrlf + "  ,CREDIT = A.CREDIT"
        strsql += vbcrlf + "  ,ADVANCE = A.ADVANCE"
        strsql += vbcrlf + "  ,DISCOUNT = A.DISCOUNT"
        strsql += vbcrlf + "  ,HANDLING = A.HANDLING"
        strsql += vbcrlf + "  ,GIFT = A.GIFT"
        strsql += vbcrlf + "  ,CASHCOUNTER = A.CASHCOUNTER"
        strsql += vbcrlf + "  ,FROMFLAG = A.FROMFLAG"
        strsql += vbcrlf + "  FROM TEMP" & systemId & "ISSRECVIEW I,TEMP" & systemId & "ACCVIEW A"
        strsql += vbcrlf + "  WHERE I.BATCHNO = A.BATCHNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "ISSRECVIEW"
        strSql += " SET ORDREPNO = (SELECT TOP 1 ORNO FROM " & cnadmindb & "..ORMAST WHERE ODBATCHNO = I.BATCHNO)"
        strSql += " FROM TEMP" & systemId & "ISSRECVIEW i"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        ''FOR PARTLY SALES
        strSql = " UPDATE TEMP" & systemId & "ISSRECVIEW SET PARTLY = 'Y' WHERE BATCHNO IN"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  	SELECT BATCHNO FROM TEMP" & systemId & "ISSRECVIEW "
        strSql += vbCrLf + "  	WHERE ISNULL(TAGGRSWT,0) <> 0 "
        strSql += vbCrLf + "  	AND ("
        strSql += vbCrLf + "  	ISNULL(IGRSWT,0) <> ISNULL(TAGGRSWT,0)"
        strSql += vbCrLf + "  	OR ISNULL(INETWT,0) <> ISNULL(TAGNETWT,0)"
        strSql += vbCrLf + "  	OR ISNULL(STNPCS,0) <> ISNULL(TAGSTNPCS,0)"
        strSql += vbCrLf + "  	OR ISNULL(STNWT,0) <> ISNULL(TAGSTNWT,0)"
        strSql += vbCrLf + "  	)"
        strSql += vbCrLf + "  )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE TEMP" & systemId & "ISSRECVIEW SET DISCOUNT=0 WHERE DISCOUNT IS NULL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            musrid = 0
            If cmbUserName.Enabled And cmbUserName.Text <> "ALL" Then musrid = objGPack.GetSqlValue("SELECT USERID FROM " & cnAdminDb & "..usermaster where Username = '" & cmbUserName.Text & "'")
            gridFullView.DataSource = Nothing
            gridTot.DataSource = Nothing
            funcClearOtherDetails()
            pnlTotGrid.Visible = False
            btnView_Search.Enabled = False
            Dim dt As New DataTable
            CreateIssRecView()
            InsertIssRecView()
            CreateAccView()
            If cmbCashCounter.Enabled And cmbCashCounter.Text <> "ALL" Then
                strSql = "DELETE FROM TEMP" & systemId & "ISSRECVIEW WHERE ISNULL(CASHCOUNTER,'')<>'" & cmbCashCounter.Text & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

            strSql = " SELECT * FROM TEMP" & systemId & "ISSRECVIEW "
            strSql += vbCrLf + "  ORDER BY CONVERT(INT,SUBSTRING(BATCHNO," & (6 + cnCostId.Length).ToString & " ,LEN(BATCHNO))),RESULT"
            'strsql += vbcrlf + "  ORDER BY BATCHNO,RESULT"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                dtpBillDate.Focus()
                Exit Sub
            End If
            gridFullView.DataSource = dt
            StyleGridFullView()
            gridFullView.Focus()
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BILLVIEW_TOTAL'")) = "Y" Then
                Dim totPcs As Integer = Val(dt.Compute("sum(ipcs)", String.Empty).ToString)
                strSql = " SELECT 'TOTAL'IDES"
                strSql += vbCrLf + "  ,'" & Val(dt.Compute("SUM(IPCS)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString) & "'IPCS"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IGRSWT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.000") & "'IGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(INETWT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.000") & "'INETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IAMOUNT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.00") & "'IAMOUNT"
                strSql += vbCrLf + "  ,''RDES"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RGRSWT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.000") & "'RGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RNETWT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.000") & "'RNETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RAMOUNT)", "ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('OB','RB')").ToString), "0.00") & "'RAMOUNT "
                strSql += vbCrLf + "  UNION ALL"

                strSql += vbCrLf + "  SELECT 'SALES 'IDES"
                strSql += vbCrLf + "  ,'" & Val(dt.Compute("SUM(IPCS)", "RESULT IN('1','9','10') AND TRANTYPE  IN ('SA','OD','RD')").ToString) & "'IPCS"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IGRSWT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('SA','OD','RD')").ToString), "0.000") & "'IGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(INETWT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('SA','OD','RD')").ToString), "0.000") & "'INETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IAMOUNT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('SA','OD','RD')").ToString), "0.00") & "'IAMOUNT"
                strSql += vbCrLf + "  ,''RDES"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RGRSWT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')").ToString), "0.000") & "'RGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RNETWT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')").ToString), "0.000") & "'RNETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RAMOUNT)", "RESULT IN('1','9','10') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('SA','OD','RD')").ToString), "0.00") & "'RAMOUNT "
                strSql += vbCrLf + "  UNION ALL"

                strSql += vbCrLf + "  SELECT 'APPROVAL ISS/REC'IDES"
                strSql += vbCrLf + "  ,'" & Val(dt.Compute("SUM(IPCS)", "RESULT IN('5','6') AND TRANTYPE  IN ('AI','AR')").ToString) & "'IPCS"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IGRSWT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('AI','AR')").ToString), "0.000") & "'IGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(INETWT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('AI','AR')").ToString), "0.000") & "'INETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IAMOUNT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('AI','AR')").ToString), "0.00") & "'IAMOUNT"
                strSql += vbCrLf + "  ,''RDES"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RGRSWT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('AI','AR')").ToString), "0.000") & "'RGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RNETWT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('AI','AR')").ToString), "0.000") & "'RNETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RAMOUNT)", "RESULT IN('5','6') AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('AI','AR')").ToString), "0.00") & "'RAMOUNT "
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT 'MISC ISSUE'IDES"
                strSql += vbCrLf + "  ,'" & Val(dt.Compute("SUM(IPCS)", "RESULT = '4' AND TRANTYPE  IN ('MI')").ToString) & "'IPCS"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IGRSWT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('MI')").ToString), "0.000") & "'IGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(INETWT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('MI')").ToString), "0.000") & "'INETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(IAMOUNT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE  IN ('MI')").ToString), "0.00") & "'IAMOUNT"
                strSql += vbCrLf + "  ,''RDES"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RGRSWT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('MI')").ToString), "0.000") & "'RGRSWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RNETWT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('MI')").ToString), "0.000") & "'RNETWT"
                strSql += vbCrLf + "  ,'" & Format(Val(dt.Compute("SUM(RAMOUNT)", "RESULT = '4' AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('MI')").ToString), "0.00") & "'RAMOUNT "
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT 'CURSOR TOT'IDES"
                strSql += vbCrLf + "  ,''IPCS,''IGRSWT,''INETWT,''IAMOUNT,''RDES,''RGRSWT,''RNETWT,''RAMOUNT"
                Dim dtTot As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTot)
                pnlTotGrid.Visible = True
                gridTot.DataSource = dtTot
                StyleGridTot()
            End If
            funcGridHeadStyle()
            Dim strTitle As String = Nothing
            If Advsearch1 = "" Then
                strTitle = "VIEW ON " & dtpBillDate.Text & " "
            Else
                strTitle = "VIEW " & Replace(Advsearch1, "AND", "")
            End If
            lblTitle.Text = strTitle
            lblTitle.Visible = True
            'Panel7.Visible = False
            gridHead.Focus()
            gridFullView.Focus()
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        Finally
            btnView_Search.Enabled = True
        End Try
        btnView_Search.Enabled = True
    End Sub

    Private Sub gridFullView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridFullView.ColumnWidthChanged
        If gridHead.ColumnCount > 0 Then
            With gridHead
                .Columns("ISSUE").Width = gridFullView.Columns("ITRANNO").Width + gridFullView.Columns("ITEMID").Width + gridFullView.Columns("ITAGNO").Width + gridFullView.Columns("IPCS").Width + gridFullView.Columns("IGRSWT").Width + gridFullView.Columns("INETWT").Width + gridFullView.Columns("IAMOUNT").Width
                .Columns("RECEIPT").Width = gridFullView.Columns("RTRANNO").Width + gridFullView.Columns("RGRSWT").Width + gridFullView.Columns("RNETWT").Width + gridFullView.Columns("RAMOUNT").Width
            End With
        End If
        If gridTot.ColumnCount > 0 Then
            With gridTot
                .Columns("IDES").Width = gridFullView.Columns("ITRANNO").Width + gridFullView.Columns("ITEMID").Width + gridFullView.Columns("ITAGNO").Width
                .Columns("IPCS").Width = gridFullView.Columns("IPCS").Width
                .Columns("IGRSWT").Width = gridFullView.Columns("IGRSWT").Width
                .Columns("INETWT").Width = gridFullView.Columns("INETWT").Width
                .Columns("IAMOUNT").Width = gridFullView.Columns("IAMOUNT").Width
                .Columns("RDES").Width = gridFullView.Columns("RTRANNO").Width
                .Columns("RGRSWT").Width = gridFullView.Columns("RGRSWT").Width
                .Columns("RNETWT").Width = gridFullView.Columns("RNETWT").Width
                .Columns("RAMOUNT").Width = gridFullView.Columns("RAMOUNT").Width
            End With
        End If
    End Sub

    Private Sub gridFullView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridFullView.DataError

    End Sub

    Private Sub gridFullView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridFullView.CellFormatting
        If gridFullView.Item("CANCEL", e.RowIndex).Value.ToString = "CANCEL" Then
            gridFullView.Rows(e.RowIndex).Cells("ITRANNO").Style.BackColor = Color.Red
            gridFullView.Rows(e.RowIndex).Cells("ITRANNO").Style.SelectionBackColor = Color.Red
        ElseIf gridFullView.Item("PARTLY", e.RowIndex).Value.ToString = "Y" Then
            gridFullView.Rows(e.RowIndex).Cells("ITRANNO").Style.BackColor = Color.LightGreen
            gridFullView.Rows(e.RowIndex).Cells("ITRANNO").Style.SelectionBackColor = Color.LightGreen
        End If
    End Sub

    Private Sub DeleteBill(ByVal batchno As String)
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        objRemark = New frmBillRemark
        objRemark.Text = "Delete Remark"
        If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        tran = Nothing
        Try
            Dim costId As String = gridFullView.Item("costid", gridFullView.CurrentRow.Index).Value.ToString
            tran = cn.BeginTransaction
            strSql = " INSERT INTO " & cnStockDb & "..CANCELLEDTRAN"
            strSql += vbCrLf + "  (TRANDATE,BATCHNO,UPDATED,UPTIME,FLAG,REMARK1,REMARK2,USERID)"
            strSql += vbCrLf + "  VALUES"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,'" & batchno & "'"
            strSql += vbCrLf + "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,'" & Date.Now.ToLongTimeString & "'"
            strSql += vbCrLf + "  ,'D'" 'FLAG CANCEL OR DELETE
            strSql += vbCrLf + "  ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
            strSql += vbCrLf + "  ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK2
            strSql += vbCrLf + "  ," & userId & "" 'USERID
            strSql += vbCrLf + "  )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
            'strSql = " DELETE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..ISSMISC WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..ISSMETAL WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..RECEIPTMISC WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..RECEIPTMETAL WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnadmindb & "..ORMAST WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnadmindb & "..ORSTONE WHERE BATCHNO = '" & batchno & "'"
            'strsql += vbcrlf + "  DELETE FROM " & cnadmindb & "..ORSAMPLE WHERE BATCHNO = '" & batchno & "'"
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
            tran.Commit()
            tran = Nothing
            MsgBox(batchno + " Successfully Deleted...")
            btnView_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then Exit Sub
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridFullView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridFullView.KeyDown
        If e.Control And e.KeyCode = Keys.D Then
            If type = BillViewType.DulicateBillPrint Then Exit Sub
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If gridFullView.Item("CANCEL", gridFullView.CurrentRow.Index).Value.ToString <> "CANCEL" Then
                MsgBox("Cancel Entry Only you can Delete", MsgBoxStyle.Information)
                Exit Sub
            End If
            DeleteBill(gridFullView.Item("BatchNo", gridFullView.CurrentRow.Index).Value.ToString)
        ElseIf e.KeyCode = Keys.F12 Then
            Call btnExit_Click(sender, e) : Exit Sub
        End If
    End Sub

    Private Function ValidateOrderBook(ByVal batchno As String) As Boolean
        If objGPack.GetSqlValue("SELECT SNO FROM " & cnadmindb & "..ORMAST WHERE BATCHNO = '" & batchno & "' AND ISNULL(ODBATCHNO,'') <> ''", , "-1") <> "-1" Then
            MsgBox("Sales Entry made against this order" + vbCrLf + "You cannot cancel this booked order", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Private Sub gridFullView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridFullView.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Space) Then
                If type = BillViewType.DulicateBillPrint Then Exit Sub
                Dim mfrmname As String = "frmBillToJnt~OWN~AUT"
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, mfrmname, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                Dim batchNo As String = gridFullView.Item("BatchNo", gridFullView.CurrentRow.Index).Value.ToString
                Dim SNO As String = gridFullView.Item("SNO", gridFullView.CurrentRow.Index).Value.ToString
                strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMDETAIL WHERE ISSSNO='" & SNO & "'"
                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    MsgBox(batchNo + " Already there in ToBe...")
                    Exit Sub
                End If
                strSql = "SELECT 1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & batchNo & "' AND TRANDATE='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND PAYMODE='DU'"
                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    If MessageBox.Show("Do you want to change into ToBe", "ToBe Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                        strSql = vbCrLf + " SELECT I.SNO,I.TRANNO,I.TRANDATE,IT.ITEMNAME ITEMNAME,PCS,I.GRSWT,I.NETWT,I.AMOUNT,I.BATCHNO,O.RUNNO FROM " & cnStockDb & "..ISSUE AS I"
                        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON I.ITEMID=IT.ITEMID"
                        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING AS O ON I.BATCHNO=O.BATCHNO"
                        strSql += vbCrLf + " WHERE I.BATCHNO = '" & batchNo & "'"
                        strSql += vbCrLf + " AND I.TRANDATE = '" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "'"
                        Dim dtItemDetail As New DataTable
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtItemDetail)

                        Dim costId As String = gridFullView.Item("costid", gridFullView.CurrentRow.Index).Value.ToString
                        Try
                            tran = cn.BeginTransaction
                            strSql = " INSERT INTO " & cnAdminDb & "..ITEMDETAIL"
                            strSql += " ("
                            strSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE,"
                            strSql += " PCS,GRSWT,NETWT,AMOUNT,REMARK1,"
                            strSql += " REMARK2,BATCHNO,CANCEL,FLAG,APPVER,"
                            strSql += " COMPANYID,UPDATED,UPTIME,RECPAY"
                            strSql += " ,RUNNO"
                            strSql += " )"
                            strSql += " VALUES"
                            strSql += " ("
                            strSql += " '" & GetNewSno(TranSnoType.ITEMDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                            strSql += " ,'" & dtItemDetail.Rows(0).Item("SNO").ToString & "'" 'ISSSNO
                            strSql += " ," & dtItemDetail.Rows(0).Item("TRANNO").ToString & "" 'TRANNO
                            strSql += " ,'" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ,'TB'" 'TRANTYPE
                            strSql += " ," & dtItemDetail.Rows(0).Item("PCS").ToString & "" 'PCS
                            strSql += " ," & dtItemDetail.Rows(0).Item("GRSWT").ToString & "" 'GRSWT
                            strSql += " ," & dtItemDetail.Rows(0).Item("NETWT").ToString & "" 'NETWT
                            strSql += " ," & dtItemDetail.Rows(0).Item("AMOUNT").ToString & "" 'AMOUNT
                            strSql += " ,'" & dtItemDetail.Rows(0).Item("ITEMNAME").ToString & "'" 'REMARK1
                            strSql += " ,'PENDING WITH US'" 'REMARK2
                            strSql += " ,'" & batchNo & "'" 'BATCHNO
                            strSql += " ,''" 'CANCEL
                            strSql += " ,''" 'FLAG
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strSql += " ,'R'" 'RECPAY
                            strSql += " ,'" & dtItemDetail.Rows(0).Item("RUNNO").ToString & "'" 'RUNNO
                            strSql += " )"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            tran.Commit()
                            tran = Nothing
                            MsgBox(batchNo + " Successfully change into ToBe...")
                        Catch ex As Exception
                            If tran IsNot Nothing Then tran.Rollback()
                            MsgBox("Error   :" + ex.Message + vbCrLf + ex.StackTrace)
                        End Try
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Function getchkotherdetails(ByVal mtdate As String, ByVal mbatchNo As String) As String
        Dim returnstr As String = Nothing
        Dim dtvalid As New DataTable
        strSql = " SELECT RUNNO,TRANNO,TRANTYPE,RECPAY,TRANDATE FROM " & cnAdminDb & "..OUTSTANDING AS T"
        strSql += vbCrLf + " WHERE isnull(CANCEL,'') <> 'Y' and T.BATCHNO = '" & mbatchNo & "' AND T.TRANDATE = '" & mtdate & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtvalid)
        If dtvalid.Rows.Count > 0 Then
            Dim chkrecpay As String = ""
            Dim Chkrunno As String = dtvalid.Rows(0).Item("RUNNO")
            Dim chktrantype As String = dtvalid.Rows(0).Item("TRANTYPE")
            If chktrantype = "D" Then
                If dtvalid.Rows(0).Item("RECPAY") = "P" Then chkrecpay = "R"
            ElseIf chktrantype = "A" Then
                If dtvalid.Rows(0).Item("RECPAY") = "R" Then chkrecpay = "P"
            End If
            If chkrecpay = "" Then Return Nothing
            Dim dtCheck As New DataTable
            strSql = " SELECT count(*) FROM " & cnAdminDb & "..OUTSTANDING AS T"
            strSql += vbCrLf + " WHERE isnull(CANCEL,'') <> 'Y' and T.RUNNO= '" & Chkrunno & "' AND TRANTYPE = '" & chktrantype & "' AND recpay= '" & chkrecpay & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCheck)
            If dtCheck.Rows(0).Item(0) > 0 Then
                returnstr = IIf(chktrantype = "D", "Credit", "Advance") & " has been adjusted"
            End If
            Return returnstr
        End If
        Return Nothing
    End Function
    Private Sub ShowAddressDia(ByVal billDate As Date, ByVal costId As String, ByVal batchNO As String)
        Dim objAddressDia As New frmAddressDia(billDate, costId, batchNO, True)
        objAddressDia.BackColor = SystemColors.InactiveCaption
        objAddressDia.MaximizeBox = False
        objAddressDia.grpAddress.BackgroundColor = Color.Lavender
        objAddressDia.ShowDialog()
    End Sub
    Private Sub gridMiscDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridMiscDetails.CellFormatting
        Try
            Select Case e.ColumnIndex
                Case 0, 2, 4
                    e.CellStyle.SelectionBackColor = System.Drawing.SystemColors.Control
                    e.CellStyle.SelectionForeColor = Color.Black
                Case 1, 3, 5
                    e.CellStyle.SelectionBackColor = Color.White
                    e.CellStyle.SelectionForeColor = Color.Black
            End Select
            If e.ColumnIndex = 1 Then
                Select Case e.RowIndex
                    Case 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End Select
                Select Case e.RowIndex
                    Case 2, 4, 5, 6, 8, 9, 11, 12, 13
                        If Val(e.Value.ToString) <> 0 Then
                            e.CellStyle.BackColor = Color.LightBlue
                            e.CellStyle.ForeColor = System.Drawing.SystemColors.HotTrack
                            e.CellStyle.Format = "#,##0.00"
                        Else
                            e.Value = DBNull.Value
                        End If
                    Case 14
                        e.CellStyle.ForeColor = Color.Blue
                        e.CellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End Select
            End If
            If e.ColumnIndex = 3 Then
                Select Case e.RowIndex
                    Case 2, 3, 4, 5, 6, 7, 8
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End Select
                Select Case e.RowIndex
                    Case 2, 4, 5, 6, 8
                        If Val(e.Value.ToString) <> 0 Then
                            e.CellStyle.BackColor = Color.LightBlue
                            e.CellStyle.ForeColor = System.Drawing.SystemColors.HotTrack
                            e.CellStyle.Format = "#,##0.00"
                        Else
                            e.Value = DBNull.Value
                        End If
                End Select
            End If
            If e.ColumnIndex = 5 Then
                Select Case e.RowIndex
                    Case 0, 1, 2, 3, 4, 5, 6, 8, 9
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If Val(e.Value.ToString) = 0 Then
                            e.Value = DBNull.Value
                        Else
                            e.CellStyle.BackColor = Color.LightBlue
                            e.CellStyle.ForeColor = System.Drawing.SystemColors.HotTrack
                            e.CellStyle.Format = "#,##0.00"
                        End If
                End Select
                Select Case e.RowIndex
                    Case 7
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If e.Value.ToString.Contains("0.00|0.00") Or e.Value.ToString = "" Then
                            e.Value = DBNull.Value
                        Else
                            e.CellStyle.BackColor = Color.LightBlue
                            e.CellStyle.ForeColor = System.Drawing.SystemColors.HotTrack
                            e.CellStyle.Format = "#,##0.00"
                        End If
                End Select
            End If
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub
    Private Sub gridTot_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridTot.CellFormatting
        Try
            Select Case e.ColumnIndex
                Case 1, 2, 3, 4, 6, 7, 8
                    If Val(e.Value.ToString) = 0 Then
                        e.Value = DBNull.Value
                    End If
            End Select
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridFullView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridFullView.RowEnter
        Try
            If gridFullView.RowCount > 0 Then
                Dim rw As Integer = e.RowIndex
                txtAddress_OWN.Clear()
                PictureBox1.Image = Nothing
                If gridFullView.Rows(rw).Cells("PICPATH").Value.ToString <> "" Then
                    If IO.File.Exists(PicPath & gridFullView.Rows(rw).Cells("PICPATH").Value.ToString) Then
                        AutoImageSizer(PicPath & gridFullView.Rows(rw).Cells("PICPATH").Value.ToString, PictureBox1, PictureBoxSizeMode.CenterImage)
                    End If
                End If
                strSql = " SELECT ACCODE"
                strsql += vbcrlf + "  ,CASE WHEN TITLE <> '' THEN TITLE + '.' ELSE '' END +"
                strsql += vbcrlf + "  CASE WHEN INITIAL <> '' THEN INITIAL + '.' ELSE '' END + PNAME AS NAME"
                strsql += vbcrlf + "  ,DOORNO"
                strsql += vbcrlf + "  ,ADDRESS1,ADDRESS2,ADDRESS3,AREA"
                strsql += vbcrlf + "  ,CITY,STATE,COUNTRY,PINCODE"
                strsql += vbcrlf + "  ,PHONERES,MOBILE,EMAIL,FAX"
                strsql += vbcrlf + "  FROM " & cnAdminDb & "..PERSONALINFO"
                strsql += vbcrlf + "  WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & gridFullView.Rows(e.RowIndex).Cells("BATCHNO").Value.ToString & "')"
                Dim dt As New DataTable
                dt.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        If .Item("ACCODE").ToString <> "" Then
                            txtAddress_OWN.Text = " " + .Item("ACCODE").ToString + vbCrLf
                        End If
                        If .Item("Name").ToString <> "" Then
                            txtAddress_OWN.Text += " " + .Item("Name").ToString
                        End If
                        If .Item("DoorNo").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("DoorNo").ToString
                        End If
                        If .Item("Address1").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Address1").ToString
                        End If
                        If .Item("Address2").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Address2").ToString
                        End If
                        If .Item("Address3").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Address3").ToString
                        End If
                        If .Item("Area").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Area").ToString
                        End If
                        If .Item("City").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("City").ToString
                        End If
                        If .Item("Pincode").ToString <> "" Then
                            txtAddress_OWN.Text += "-" + .Item("Pincode").ToString
                        End If
                        If .Item("PhoneRes").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("PhoneRes").ToString
                        End If
                        If .Item("Mobile").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Mobile").ToString
                        End If
                        If .Item("Email").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Email").ToString
                        End If
                        If .Item("Fax").ToString <> "" Then
                            txtAddress_OWN.Text += " " + vbCrLf + .Item("Fax").ToString
                        End If
                    End With
                End If
                With gridFullView
                    strSql = " SELECT 'ITEM'COL1"
                    strsql += vbcrlf + "  ,'" & .Item("ITEM", rw).Value.ToString & "'COL2"
                    strsql += vbcrlf + "  ,'COUNTER'COL3,'" & .Item("COUNTER", rw).Value.ToString & "'COL4"
                    strsql += vbcrlf + "  ,'CASH'COL5,'" & .Item("CASH", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  SELECT 'SUB ITEM'COL1,'" & .Item("SUBITEM", rw).Value.ToString & "'COL2"
                    strsql += vbcrlf + "  ,'CATEGORY'COL3,'" & .Item("CATEGORY", rw).Value.ToString & "'COL4"
                    strsql += vbcrlf + "  ,'CHEQUE'COL5,'" & .Item("CHEQUE", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  SELECT 'TAG WEIGHT'COL1,'" & .Item("TAGNETWT", rw).Value.ToString & "'COL2"
                    strsql += vbcrlf + "  ,'PUR.WASTAGE'COL3,'" & .Item("WASTAGE", rw).Value.ToString & "'COL4"
                    strsql += vbcrlf + "  ,'CARD'COL5,'" & .Item("CARD", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  SELECT 'DIA WEIGHT'COL1,'" & .Item("DIAWT", rw).Value.ToString & "'COL2"
                    strsql += vbcrlf + "  ,''COL3,''COL4"
                    strSql += vbCrLf + "  ,'SCHEME'COL5,'" & .Item("CHITCARD", rw).Value.ToString & "'COL6"

                    Dim saAmt As Double = Val(.Item("SALES", rw).Value.ToString)
                    Dim saVat As Double = Val(.Item("SALESVAT", rw).Value.ToString)
                    Dim purAmt As Double = Val(.Item("PURCHASE", rw).Value.ToString) + Val(.Item("SRETURN", rw).Value.ToString)
                    Dim purVat As Double = Val(.Item("PURCHASEVAT", rw).Value.ToString) + Val(.Item("RETURNVAT", rw).Value.ToString)
                    Dim purWithVat As Double = purAmt - purVat

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'MK CHARGE'Col1,'" & .Item("MCHARGE", rw).Value.ToString & "'Col2"
                    strsql += vbcrlf + "  ,'TOT PUR AMT'Col3,'" & IIf(purAmt <> 0, Format(purAmt, "0.00"), "") & "'Col4"
                    strsql += vbcrlf + "  ,'CREDIT'COL5,'" & .Item("CREDIT", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  SELECT 'WASTAGE'COL1,'" & IIf(Val(.Item("ITEMID", rw).Value.ToString) > 0, .Item("WASTAGE", rw).Value.ToString, "") & "'COL2"
                    strsql += vbcrlf + "  ,'TOT PUR VAT'Col3,'" & IIf(purVat <> 0, Format(purVat, "0.00"), "") & "'Col4"
                    strsql += vbcrlf + "  ,'GIFT VOUCHER'Col5,'" & .Item("GIFT", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  SELECT 'RATE'COL1,'" & .Item("RATE", rw).Value.ToString & "'COL2"
                    strsql += vbcrlf + "  ,'PUR AMT+VAT'COL3,'" & IIf(purWithVat <> 0, Format(purWithVat, "0.00"), "") & "'COL4"
                    strsql += vbcrlf + "  ,'ADVANCE'COL5,'" & .Item("ADVANCE", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select ''Col1,''Col2"
                    strsql += vbcrlf + "  ,''Col3,''Col4"
                    strSql += vbCrLf + "  ,'DISC|FIN-DISC'Col5,'" & .Item("DISCOUNT", rw).Value.ToString & "|" & .Item("FINDISCOUNT", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'STONE AMT'Col1,'" & .Item("STNAMT", rw).Value.ToString & "'Col2"
                    strsql += vbcrlf + "  ,'SALES-PUR'Col3,'" & IIf(saAmt + saVat - purWithVat <> 0, Format(saAmt + saVat - purWithVat, "0.00"), "") & "'Col4"
                    strsql += vbcrlf + "  ,'HAND CHARGE'Col5,'" & .Item("HANDLING", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'OTHER CHAR'Col1,'" & .Item("MISCAMT", rw).Value.ToString & "'Col2"
                    strsql += vbcrlf + "  ,'BILL STATUS'COL3,'" & .Item("CANCEL", rw).Value.ToString & "'COL4"
                    strsql += vbcrlf + "  ,'UPTIME'Col5,'" & .Item("UPTIME", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select ''Col1,''Col2"
                    strSql += vbCrLf + "  ,'ORDER/REPAIR'Col3,'" & .Item("ORDREPNO", rw).Value.ToString & "'Col4"
                    strsql += vbcrlf + "  ,'UPDATED'COL5,'" & .Item("UPDATED", rw).Value.ToString & "'COL6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'TOT SALE AMT'Col1,'" & IIf(saAmt <> 0, Format(saAmt, "0.00"), "") & "'Col2"
                    strsql += vbcrlf + "  ,'REFERENCE'Col3,'" & .Item("REFERENCE", rw).Value.ToString & "'Col4"
                    strsql += vbcrlf + "  ,'NARATION'Col5,'" & .Item("REMARK1", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'TOT SALE VAT'Col1,'" & IIf(saVat <> 0, Format(saVat, "0.00"), "") & "'Col2"
                    strsql += vbcrlf + "  ,'OPERATOR'Col3,'" & .Item("USERNAME", rw).Value.ToString & "'Col4"
                    strsql += vbcrlf + "  ,'BILL-REMARK'Col5,'---'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'SALE AMT+VAT'Col1,'" & IIf(saAmt + saVat <> 0, Format(saAmt + saVat, "0.00"), "") & "'Col2"
                    strsql += vbcrlf + "  ,'SALES PERSON'Col3,'" & .Item("EMPNAME", rw).Value.ToString & "'Col4"
                    strsql += vbcrlf + "  ,'CASH COUNTER'Col5,'" & .Item("CASHCOUNTER", rw).Value.ToString & "'Col6"

                    strsql += vbcrlf + "  UNION ALL"
                    strsql += vbcrlf + "  Select 'TRANTYPE'Col1,'" & .Item("TRANMODE", rw).Value.ToString & "'Col2"
                    strSql += vbCrLf + "  ,'DISC AUTORIZE'Col3,'---'Col4"
                    strSql += vbCrLf + "  ,'BATCHNO'Col5,'" & .Item("BATCHNO", rw).Value.ToString & "'Col6"
                    dtOtherDetails.Clear()
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtOtherDetails)
                    If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BILLVIEW_TOTAL'")) = "Y" Then
                        ''Cursor Total
                        Dim iPcs As Integer = 0
                        Dim iGrsWt As Double = 0
                        Dim iNetWt As Double = 0
                        Dim iAmount As Double = 0
                        Dim rGrsWt As Double = 0
                        Dim rNetWt As Double = 0
                        Dim rAmount As Double = 0
                        For cnt As Integer = 0 To rw
                            If .Item("TRANTYPE", cnt).Value.ToString = "OB" Or .Item("TRANTYPE", cnt).Value.ToString = "RB" Then Continue For
                            If .Item("CANCEL", cnt).Value.ToString = "" Then
                                iPcs += Val(.Item("iPcs", cnt).Value.ToString)
                                iGrsWt += Val(.Item("iGrsWt", cnt).Value.ToString)
                                iNetWt += Val(.Item("iNetWt", cnt).Value.ToString)
                                iAmount += Val(.Item("iAmount", cnt).Value.ToString)
                            End If
                            If .Item("CANCEL", cnt).Value.ToString = "" Then
                                rGrsWt += Val(.Item("rGrsWt", cnt).Value.ToString)
                                rNetWt += Val(.Item("rNetWt", cnt).Value.ToString)
                                rAmount += Val(.Item("rAmount", cnt).Value.ToString)
                            End If
                        Next
                        If gridTot.RowCount - 1 >= 4 Then
                            With gridTot.Rows(4)
                                .Cells("iDes").Value = "CURSOR TOTAL"
                                .Cells("iPcs").Value = iPcs
                                .Cells("iGrsWt").Value = Format(iGrsWt, "0.000")
                                .Cells("iNetWt").Value = Format(iNetWt, "0.000")
                                .Cells("iAmount").Value = Format(iAmount, "0.00")

                                .Cells("rDes").Value = ""
                                .Cells("rGrsWt").Value = Format(rGrsWt, "0.000")
                                .Cells("rNetWt").Value = Format(rNetWt, "0.000")
                                .Cells("rAmount").Value = Format(rAmount, "0.00")
                            End With
                        End If
                    End If
                End With
            End If
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridFullView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridFullView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
            gridTot.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub


    Private Sub txtAddress_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress_OWN.GotFocus
        gridFullView.Select()
    End Sub


    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridFullView, BrightPosting.GExport.GExportType.Export)
    End Sub



    Private Sub gridFullView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridFullView.CellContentClick

    End Sub

    Private Sub gridMiscDetails_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridMiscDetails.CellContentClick

    End Sub

    Private Sub BtnAdvanced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAdvanced.Click

        Me.objAdvscr.dtpBillDatef.Value = Today.Date
        Me.objAdvscr.dtpBillDatet.Value = Today.Date
        objAdvscr.txtWt_From.Text = 0.0
        objAdvscr.txtWt_To.Text = 0.0
        objAdvscr.ShowDialog()
        Advsearch1 = " AND I.TRANDATE BETWEEN '" & Me.objAdvscr.dtpBillDatef.Value.ToString("yyyy-MM-dd") & "' AND '" & Me.objAdvscr.dtpBillDatet.Value.ToString("yyyy-MM-dd") & "'"
        If Val(objAdvscr.txtWt_From.Text) <> 0 Or Val(objAdvscr.txtWt_To.Text) <> 0 Then
            Advsearch2 = " AND " & IIf(objAdvscr.cmbOGrsNet.Text = "GRS", " GRSWT ", " NETWT ") & "BETWEEN " & Val(objAdvscr.txtWt_From.Text) & " AND " & Val(Me.objAdvscr.txtWt_To.Text)
        End If
        btnView_Search.Focus()
        Exit Sub
    End Sub

    Private Sub dtpBillDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpBillDate.GotFocus
        Advsearch1 = ""
        Advsearch2 = ""
    End Sub

End Class
