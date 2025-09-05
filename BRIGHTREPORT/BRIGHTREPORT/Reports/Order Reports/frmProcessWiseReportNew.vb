Imports System.Data.OleDb
Public Class frmProcessWiseReportNew
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub AccSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function LoadCostcentre()
        Dim dtCostCentre As New DataTable
        'chkcmbCostcentre.Items.Clear()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        ' BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostCentre, "COSTNAME", , "ALL")

    End Function
    Function LoadProcess()
        Dim dtCostCentre As New DataTable
        ' chkcmbCostcentre.Items.Clear()
        strSql = " SELECT 'ALL' ORDSTATE_NAME,1 DISPORDER"
        strSql += " UNION ALL"
        strSql += " SELECT ORDSTATE_NAME , DISPORDER  FROM " & cnAdminDb & "..ORDERSTATUS   ORDER BY DISPORDER , ORDSTATE_NAME "
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(cmbProcess, dtCostCentre, "ORDSTATE_NAME", True)
    End Function
    Function LoadCompany()
        ' cmbCompany.Items.Add("ALL")
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE ISNULL(ACTIVE,'') <> 'N'  ORDER BY COMPANYNAME"
        'objGPack.FillCombo(strSql, cmbCompany, False, False)
    End Function
    Function LoadAcname()
        Dim dtCategory As New DataTable
        strSql = " SELECT 'ALL' ACNAME UNION ALL "
        strSql += " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
        dtCategory = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCategory)
        BrighttechPack.GlobalMethods.FillCombo(cmbSmith, dtCategory, "ACNAME", True)
    End Function
    Function LoadOrderNo()
        strSql = " SELECT 'ALL' ORNO UNION ALL "
        strSql += " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(CANCEL,'') = ''-- ORDER BY (CONVERT(INT,SUBSTRING(ORNO,10,10)))"
        objGPack.FillCombo(strSql, cmbOrderNo, , True)
    End Function
    Private Sub AccSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY SHORTNAME"
        objGPack.FillCombo(strSql, cmbCategory)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Function LoadCategory()
        Dim dtCategory As New DataTable
        strSql = " SELECT 'ALL' CATNAME UNION ALL SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        dtCategory = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCategory)
        BrighttechPack.GlobalMethods.FillCombo(cmbCategory, dtCategory, "CATNAME", True)
    End Function
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        tabMain.SelectedTab = TabView
        lblTitle.Text = ""
        gridView.DataSource = Nothing

        strSql = vbCrLf + " IF (SELECT ID FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORIRDETAILS' AND  XTYPE = 'U') > 0  DROP TABLE TEMPTABLEDB..TEMPORIRDETAILS  "
        strSql += vbCrLf + " SELECT ACCODE ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME,I.ORDSTATE_ID "
        strSql += vbCrLf + " ,S.ORDSTATE_NAME+CASE WHEN ISNULL(GS12,'') = 'Y' THEN '_GS12' ELSE '_GS11' END ORDSTATE_NAME"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,I.ORDSTATE_ID)+CASE WHEN ISNULL(GS12,'') = 'Y' THEN '_GS12' ELSE '_GS11' END ORDID"
        strSql += vbCrLf + " ,I.ORNO  ,TRANNO,I.CATCODE,C.CATNAME,CASE WHEN ISNULL(GS12,'') = 'Y' THEN 'GS12' ELSE 'GS11' END CATTYPE , TRANDATE"
        strSql += vbCrLf + " , SUM(I.GRSWT) GRSWT, SUM(I.NETWT) NETWT , SUM(I.WASTAGE) WASTAGE, SUM(I.MC) MC "
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) STNWT "
        strSql += vbCrLf + " ,I.ORSTATUS,I.BATCHNO,I.ENTRYORDER,I.SNO INTO TEMPTABLEDB..TEMPORIRDETAILS"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ORIRDETAIL I  "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORMAST O ON O.SNO = I.ORSNO AND I.ORNO = O.ORNO AND ISNULL(O.CANCEL,'') = ''"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORDERSTATUS AS S ON I.ORDSTATE_ID = S.ORDSTATE_ID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C   ON I.CATCODE = C.CATCODE"
        strSql += vbCrLf + " WHERE 1 = 1 "
        If chkAsOnDate.Checked Then strSql += vbCrLf + "AND  I.TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        If Not chkAsOnDate.Checked Then strSql += vbCrLf + "AND  I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.ORNO <> '" & IIf(cmbOrderNo.Text <> "" And cmbOrderNo.Text <> "ALL", cmbOrderNo.Text, "") & "' "
        If cmbCategory.Text <> "" And cmbCategory.Text <> "ALL" Then
            strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        If cmbProcess.Text <> "" And cmbProcess.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.ORDSTATE_NAME = '" & cmbProcess.Text & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + "  GROUP BY I.ORNO,I.ACCODE,I.CATCODE,C.GS12 ,C.CATNAME, S.ORDSTATE_NAME,I.ORDSTATE_ID , I.TRANDATE, I.TRANNO,I.ORSTATUS,I.BATCHNO,I.ENTRYORDER,I.SNO"
        strSql += vbCrLf + "  ORDER BY ENTRYORDER "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = vbCrLf + " IF (SELECT ID FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPPROCESS' AND  XTYPE = 'U') > 0  DROP TABLE TEMPTABLEDB..TEMPPROCESS  "
        strSql += vbCrLf + " SELECT DISTINCT T.ORDSTATE_ID, T.ORDSTATE_NAME,ORDID,O.DISPORDER, IDENTITY (INT,1,1) SNO  INTO  TEMPTABLEDB..TEMPPROCESS  "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPORIRDETAILS T INNER JOIN " & cnAdminDb & "..ORDERSTATUS O  ON T.ORDSTATE_ID = O.ORDSTATE_ID "
        strSql += vbCrLf + " ORDER BY DISPORDER "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = vbCrLf + " IF (SELECT ID FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORIRDETAILS_FINAL' AND  XTYPE = 'U') > 0  DROP TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL  "
        strSql += vbCrLf + " SELECT  ORNO,SUBSTRING(ORNO,6,10) REFNO,(SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORNO) TOTAL,CONVERT(VARCHAR,'')COLHEAD,1 RESULT "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPORIRDETAILS_FINAL FROM TEMPTABLEDB..TEMPORIRDETAILS T GROUP BY ORNO ORDER BY CONVERT(INT,SUBSTRING(ORNO,9,10))  "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000


        strSql = vbCrLf + "DECLARE @SNO INT = 1"
        strSql += vbCrLf + " DECLARE @PROCESS VARCHAR(15)"
        strSql += vbCrLf + " DECLARE @ORDID VARCHAR(15)"
        strSql += vbCrLf + " DECLARE @QRY VARCHAR(8000)"
        strSql += vbCrLf + " WHILE (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPPROCESS) <> 0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " 	IF @SNO > (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPPROCESS) BREAK"
        strSql += vbCrLf + "	SELECT @ORDID = ORDID FROM TEMPTABLEDB..TEMPPROCESS WHERE SNO =  @SNO"
        strSql += vbCrLf + " 	SET @QRY = 'ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_ACNAME] VARCHAR(100)'"
        strSql += vbCrLf + "	/*SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_TRANDATE] SMALLDATETIME'*/"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_ISSWT] NUMERIC(15,3)'"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_RECWT] NUMERIC(15,3)'"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_WASTAGE] NUMERIC(15,3)'"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_MCHARGE] NUMERIC(15,3)'"
        strSql += vbCrLf + "	IF (SELECT ISNULL(SUM(ISNULL(DIAWT,0)),0) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R' ) > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_DIAWT] VARCHAR(100)'"
        strSql += vbCrLf + "	IF (SELECT ISNULL(SUM(ISNULL(DIAPCS ,0)),0) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R' ) > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_DIAPCS] VARCHAR(100)'"
        strSql += vbCrLf + "	IF (SELECT ISNULL(SUM(ISNULL(STNWT,0)),0) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R' ) > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_STNWT] VARCHAR(100)'"
        strSql += vbCrLf + "	IF (SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R' ) > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_STNPCS] VARCHAR(100)'"
        strSql += vbCrLf + "	SET @QRY = @QRY + ' ALTER TABLE TEMPTABLEDB..TEMPORIRDETAILS_FINAL ADD ['+@ORDID+'_BALANCE] VARCHAR(100)'"
        strSql += vbCrLf + " 	EXEC (@QRY) "
        strSql += vbCrLf + " 	PRINT @QRY"
        strSql += vbCrLf + " 	SET @SNO = @SNO+1"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " SET @ORDID = ''"
        strSql += vbCrLf + " SET @SNO = 1"
        strSql += vbCrLf + " WHILE (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPPROCESS) <> 0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " 	IF @SNO > (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPPROCESS) BREAK"
        strSql += vbCrLf + "	SELECT @ORDID = ORDID FROM TEMPTABLEDB..TEMPPROCESS WHERE SNO =  @SNO"
        strSql += vbCrLf + "	SET @QRY = 'UPDATE F SET ['+@ORDID+'_ACNAME] = (SELECT DISTINCT TOP 1 ACNAME FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' ) FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_ISSWT] = (SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''I'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_RECWT] = (SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_WASTAGE] = (SELECT SUM(WASTAGE) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_MCHARGE] = (SELECT SUM(MC) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	IF (SELECT SUM(ISNULL(DIAPCS,0)) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R') > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_DIAPCS] =(SELECT SUM(DIAPCS) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	IF (SELECT SUM(ISNULL(DIAWT,0)) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R') > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_DIAWT] = (SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	IF (SELECT SUM(ISNULL(STNPCS,0)) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R') > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_STNPCS] = (SELECT SUM(STNPCS) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F ' "
        strSql += vbCrLf + "	IF (SELECT SUM(ISNULL(STNWT,0)) FROM TEMPTABLEDB..TEMPORIRDETAILS WHERE ORDID = @ORDID AND ORSTATUS = 'R') > 0"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_STNWT] = (SELECT SUM(STNWT) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''' AND ORSTATUS = ''R'') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	SET @QRY = @QRY +' UPDATE F SET ['+@ORDID+'_BALANCE] = (SELECT SUM(CASE WHEN ORSTATUS = ''R'' THEN ISNULL(GRSWT,0)+ISNULL(WASTAGE,0) ELSE (ISNULL(GRSWT,0)+ISNULL(WASTAGE,0))*-1 END ) FROM TEMPTABLEDB..TEMPORIRDETAILS T WHERE ORNO = F.ORNO AND ORDID =  '''+@ORDID+''') FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL F '"
        strSql += vbCrLf + "	EXEC (@QRY) "
        strSql += vbCrLf + "	PRINT @QRY"
        strSql += vbCrLf + " 	SET @SNO = @SNO+1"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORIRDETAILS_FINAL (REFNO,TOTAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT 'TOTAL',SUM(TOTAL),'2','S' FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL WHERE RESULT = 1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000


        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPORIRDETAILS_FINAL ORDER BY RESULT"
        Dim dtGrid As New DataTable
        Dim dCol As New DataColumn("KEYNO")
        dCol.AutoIncrement = True
        dCol.AutoIncrementSeed = 0
        dCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(0)
        gridView.DataSource = dtGrid
        Dim tit As String = Nothing
        tit = "Process Abstract from " & dtpFrom.Text & " to " & dtpTo.Text
        lblTitle.Text = tit
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("ORNO").Visible = False
            For cnt As Integer = 0 To .ColumnCount - 1
                gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
        End With
        FillGridGroupStyle_KeyNoWise(gridView)
        FormatGridColumns(gridView, False, False, True, False)
        GridHeaderStyle(gridHead)
        gridView.Focus()
    End Sub

    Private Sub GridHeaderStyle(ByVal gridheaderview As DataGridView)
        Dim DtHead As New DataTable
        Dim HeadString As String = ""
        Dim HeadColName As String = ""
        Dim HeadColName1 As String = ""
        gridheaderview.DataSource = Nothing
        Dim STR As String = ""
        Dim Oldstr As String = ""
        With gridView
            For Each Col As DataGridViewColumn In .Columns
                ''''''''''For Getting  Second position (_)
                STR = Col.Name.ToString
                Dim Fristindex As Integer = STR.IndexOf("_", 0) + 2
                Dim Secondindex As Integer = 0
                STR = Mid(STR, Fristindex)
                Secondindex = STR.IndexOf("_", 0) + 2
                STR = Mid(Col.Name.ToString, 1, (Fristindex + Secondindex) - 2)
                If Col.Name.Contains(STR) And STR = Oldstr And Col.Visible Then
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                ElseIf Col.Visible Then
                    If STR <> Oldstr Then Oldstr = STR
                    If HeadString <> "" Then DtHead.Columns.Add(HeadString, GetType(String)) : HeadString = ""
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                End If
                gridView.Columns(Col.Name).HeaderText = Replace(Col.Name, STR, "")
            Next
            If HeadString <> "" Then DtHead.Columns.Add(HeadString, GetType(String)) : HeadString = ""
        End With
        DtHead.Columns.Add("SCROLL", GetType(String))
        gridheaderview.DataSource = DtHead
        Dim HealColWidth As Integer = 0 : HeadString = ""
        Dim HeadString1 = "" : Oldstr = "" : STR = ""
        With gridView
            For Each Col As DataGridViewColumn In .Columns
                STR = Col.Name.ToString
                Dim Fristindex As Integer = STR.IndexOf("_", 0) + 2
                Dim Secondindex As Integer = 0
                STR = Mid(STR, Fristindex)
                Secondindex = STR.IndexOf("_", 0) + 2
                STR = Mid(Col.Name.ToString, 1, (Fristindex + Secondindex) - 2)
                If Col.Name.Contains(STR) And STR = Oldstr And Col.Visible Then
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                    HealColWidth = HealColWidth + Col.Width
                ElseIf Col.Visible Then
                    If HeadString <> "" Then
                        gridheaderview.Columns(HeadString).Width = HealColWidth
                        If STR <> Oldstr And Oldstr <> "" Then
                            HeadString1 = Val(Mid(HeadString, 1, IIf(InStr(HeadString, "_") <> 0, InStr(HeadString, "_") - 1, 0)))
                            strSql = "SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = '" & HeadString1 & "'"
                            HeadString1 = GetSqlValue(cn, strSql)
                            If HeadString1 Is Nothing Then HeadString1 = ""
                            gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
                        Else
                            HeadString1 = ""
                            gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
                        End If
                        HeadString = "" : HeadColName = "" : HealColWidth = 0
                    End If
                    If STR <> Oldstr Then Oldstr = STR
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                    HealColWidth = HealColWidth + Col.Width
                End If
            Next
            If HeadString <> "" And HealColWidth <> 0 Then
                gridheaderview.Columns(HeadString).Width = HealColWidth
                If STR <> "" Then
                    HeadString1 = Val(Mid(HeadString, 1, IIf(InStr(HeadString, "_") <> 0, InStr(HeadString, "_") - 1, 0)))
                    strSql = "SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = '" & HeadString1 & "'"
                    HeadString1 = GetSqlValue(cn, strSql)
                    If HeadString1 Is Nothing Then HeadString1 = ""
                    gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
                End If
            End If
            gridheaderview.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridheaderview.Columns("SCROLL").Visible = False
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridheaderview.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheaderview.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridheaderview.Columns("SCROLL").Visible = False
            End If
            gridheaderview.Columns("SCROLL").HeaderText = ""
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        lblTitle.Text = ""
        LoadCompany()
        LoadCostcentre()
        LoadCategory()
        LoadAcname()
        LoadOrderNo()
        LoadProcess()
        cmbCategory.Text = "ALL"
        cmbSmith.Text = "ALL"
        cmbProcess.Text = "ALL"
        cmbOrderNo.Text = "ALL"
        'dtpFrom.Value = Today.Date
        'dtpTo.Value = Today.Date
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        Me.tabMain.SelectedTab = TabGeneral
        chkAsOnDate.Checked = True
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = TabGeneral
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkAsOnDate.CheckedChanged
        dtpTo.Visible = Not chkAsOnDate.Checked
        Label2.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked = False Then chkAsOnDate.Text = "Date" Else chkAsOnDate.Text = "As On Date"
    End Sub
    Private Sub gridView_Scroll(sender As Object, e As ScrollEventArgs) Handles gridView.Scroll
        If gridHead Is Nothing Then Exit Sub
        If Not gridHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
