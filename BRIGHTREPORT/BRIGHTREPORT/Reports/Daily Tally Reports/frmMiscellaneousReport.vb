Imports System.Data.OleDb
Public Class frmMiscellaneousReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dcmd As OleDbCommand
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim das As New OleDbDataAdapter
    Dim CCENTRE As String = Nothing
    Dim NOID As String = Nothing
    Dim filItemCtrId As String = Nothing
    Dim SelectedCompany As String
    Dim dtItemCounter As New DataTable

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        funcAddMetalName()
        funcLoadDesigner()
        rbtDetailWise.Checked = True
        lblTitle.Visible = False
    End Sub

    Private Sub frmMiscellaneousReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        LoadCompany(chkLstCompany)
        ProcAddItemCounter()
        GrbControls.Location = New Point((ScreenWid - GrbControls.Width) / 2, ((ScreenHit - 128) - GrbControls.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ProcAddItemCounter()
        strSql = "SELECT ITEMCTRID, ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRID"
        da = New OleDbDataAdapter(strSql, cn)
        dtItemCounter = New DataTable
        da.Fill(dtItemCounter)
        If dtItemCounter.Rows.Count > 0 Then
            chkLstItemCounter.Items.Add("ALL", True)
            For cnt As Integer = 0 To dtItemCounter.Rows.Count - 1
                chkLstItemCounter.Items.Add(dtItemCounter.Rows(cnt).Item(1).ToString)
            Next
        Else
            chkLstItemCounter.Items.Clear()
            chkLstItemCounter.Enabled = False
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        chkCompanySelectAll.Checked = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        If cmbCategory.Items.Count > 0 Then
            cmbCategory.SelectedIndex = 0
        End If
        If cmbItemName.Items.Count > 0 Then
            cmbItemName.SelectedIndex = 0
        End If
        If cmbMetalName.Items.Count > 0 Then
            cmbMetalName.SelectedIndex = 0
        End If
        strSql = " SELECT 'ALL' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'MANUFACTURING' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'TRADING' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'EXEMPTED' TRANS "
        Dim dtTran = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTran)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbStkType, dtTran, "TRANS", , "ALL")
        funcAddCostName()
        'funcAddNodeId()
        'chkStuddedStone.Checked = False
        chkSubItem.Enabled = True
        'rbtDetailWise.Checked = True
        Prop_Gets()
        dtpFrom.Select()
        lblTitle.Visible = False
        gridView.DataSource = Nothing
    End Sub

    Public Function funcAddCostName() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                'For cnt As Integer = 0 To dt.Rows.Count - 1
                '    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                'Next
                For i As Integer = 0 To dt.Rows.Count - 1
                    If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            End If

        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Private Sub funcAddNodeId()
        chkLstNodeId.Items.Clear()
        chkLstNodeId.Items.Add("ALL", True)

        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        End If
    End Sub

    Function funcFilter() As Integer
        ''COSTCENTRE
        CCENTRE = ""
        If chkLstCostCentre.Enabled Then
            If chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstCostCentre.CheckedItems.Count - 1
                    CCENTRE += "'" + chkLstCostCentre.CheckedItems.Item(CNT).ToString + "'"
                    If Not (CNT = chkLstCostCentre.CheckedItems.Count - 1) Then CCENTRE += ","
                Next
            End If
        End If

        ''NODE ID
        NOID = ""
        If chkLstNodeId.CheckedItems.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 0 To chkLstNodeId.CheckedItems.Count - 1
                NOID += "'" + chkLstNodeId.CheckedItems.Item(CNT).ToString + "'"
                If Not (CNT = chkLstNodeId.CheckedItems.Count - 1) Then NOID += ","
            Next
        End If

        'ITEM COUNTER
        filItemCtrId = ""
        If chkLstItemCounter.Items.Count > 0 Then
            If chkLstItemCounter.CheckedItems.Count > 0 And chkLstItemCounter.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstItemCounter.CheckedItems.Count - 1
                    filItemCtrId += "'" + chkLstItemCounter.CheckedItems.Item(CNT).ToString + "'"
                    If Not (CNT = chkLstItemCounter.CheckedItems.Count - 1) Then filItemCtrId += ","
                Next
            End If
        End If
    End Function

    Private Sub frmMiscellaneousReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub NewReport()
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, True)
        Dim StkType As String = ""
        If chkCmbStkType.Text <> "ALL" And chkCmbStkType.Text <> "" Then
            If chkCmbStkType.Text.Contains("MANUFACTURING") Then
                StkType += "'M',"
            End If
            If chkCmbStkType.Text.Contains("EXEMPTED") Then
                StkType += "'E',"
            End If
            If chkCmbStkType.Text.Contains("TRADING") Then
                StkType += "'T','',"
            End If
            If StkType <> "" Then
                StkType = Mid(StkType, 1, StkType.Length - 1)
            End If
        End If
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPMI')>0 DROP TABLE TEMPTABLEDB..TEMPMI"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "SELECT DISTINCT ISS.ISSSNO,I.SNO,I.TRANNO,I.TRANDATE,CATNAME,ITEMNAME,SUBITEMNAME,I.TAGNO,PCS,I.GRSWT,NETWT"
        strSql += vbCrLf + " ,ISS.STNPCS,ISS.STNWT"
        strSql += vbCrLf + " ,ISS.STNRATE,ISS.STNAMT"
        strSql += vbCrLf + " ,P.PURNETWT,P.PURWASTAGE,P.PURTOUCH,P.PURMC"
        'strSql += vbCrLf + " ,PS.STNPCS AS PSTNPCS,PS.STNWT AS PSTNWT,PS.PURRATE,PS.PURAMT"
        strSql += vbCrLf + " ,REMARK1,REMARK2,E.EMPNAME,U.USERNAME"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO,' ' AS COLHEAD "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPMI"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSSTONE ISS ON I.SNO=ISS.ISSSNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=I.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.SUBITEMID=I.SUBITEMID AND SM.ITEMID=I.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITEMTAG P ON P.TAGNO=I.TAGNO AND P.ITEMID=I.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON E.EMPID=I.EMPID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=I.USERID"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND I.TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompany & ")"
        If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
            strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'))"
        End If
        If cmbCategory.Text <> "ALL" And cmbMetalName.Text <> "" Then
            strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        If ChkCmbDesigner.Text <> "ALL" And ChkCmbDesigner.Text <> "" Then
            strSql += vbCrLf + " AND I.TAGDESIGNER IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN(" & GetChecked_CheckedList(ChkCmbDesigner, True) & "))"
        End If
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            strSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        If CCENTRE <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & CCENTRE & "))"
        End If
        If NOID <> "" Then
            strSql += vbCrLf + " AND I.SYSTEMID IN (" & NOID & ")"
        End If
        If filItemCtrId.ToString <> "" Then
            strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & filItemCtrId.ToString & "))"
        End If
        If StkType <> "" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') IN (" & StkType & ")"
        End If
        strSql += vbCrLf + " ORDER BY I.TRANDATE,I.TRANNO,I.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "DECLARE @SNO VARCHAR(15)"
        strSql += vbCrLf + "DECLARE @ID VARCHAR(15)"
        strSql += vbCrLf + "DECLARE @PREVSNO VARCHAR(15)"
        strSql += vbCrLf + "SET @PREVSNO=''"
        strSql += vbCrLf + "DECLARE CUR CURSOR FOR SELECT SNO,KEYNO FROM TEMPTABLEDB..TEMPMI ORDER BY SNO,KEYNO"
        strSql += vbCrLf + "OPEN CUR "
        strSql += vbCrLf + "FETCH NEXT FROM CUR INTO @SNO,@ID"
        strSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
        strSql += vbCrLf + "BEGIN"
        strSql += vbCrLf + "	IF @SNO=@PREVSNO"
        strSql += vbCrLf + "	BEGIN"
        strSql += vbCrLf + "		UPDATE TEMPTABLEDB..TEMPMI SET TRANNO=NULL,TRANDATE=NULL,PCS=NULL,GRSWT=NULL,NETWT=NULL"
        strSql += vbCrLf + "		,CATNAME=NULL,ITEMNAME=NULL,SUBITEMNAME=NULL,TAGNO=NULL,EMPNAME=NULL,USERNAME=NULL"
        strSql += vbCrLf + "		,REMARK1=NULL,REMARK2=NULL,PURNETWT=NULL WHERE SNO=@SNO AND KEYNO=@ID"
        strSql += vbCrLf + "	END"
        strSql += vbCrLf + "	ELSE"
        strSql += vbCrLf + "	BEGIN"
        strSql += vbCrLf + "		SET @PREVSNO=@SNO"
        strSql += vbCrLf + "	END"
        strSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @SNO,@ID"
        strSql += vbCrLf + "END"
        strSql += vbCrLf + "CLOSE CUR"
        strSql += vbCrLf + "DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPMI(CATNAME,PCS,GRSWT,NETWT,STNPCS,STNWT,STNAMT,PURNETWT,SNO,COLHEAD)"
        strSql += vbCrLf + "SELECT 'TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT)"
        strSql += vbCrLf + ",SUM(STNAMT),SUM(PURNETWT),'','G'"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPMI"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPMI ORDER BY KEYNO"
        Dim dtMisecellaneous As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMisecellaneous)
        If dtMisecellaneous.Rows.Count < 1 Then
            btnView_Search.Enabled = True
            MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnView_Search.Focus()
            Exit Sub
        End If
        With gridView
            tabView.Show()
            .DataSource = dtMisecellaneous
            GridViewFormat()
            FormatGridColumns(gridView, False, False)
            .Columns("SNO").Visible = False
            .Columns("ISSSNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("CATNAME").HeaderText = "PARTICULARS"
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        lblTitle.Visible = True
        Dim strTitle As String = Nothing
        strTitle = " MISCELLANEOUS REPORT"
        strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkStuddedStone.Checked = True Then
            strTitle += " WITH STUDDED STONE"
        End If
        If Strings.Right(strTitle, 3) = "AND" Then
            strTitle = Strings.Left(strTitle, strTitle.Length - 3)
        End If
        If rbtDetailWise.Checked = True Then
            strTitle += "(DETAILWISE) "
        ElseIf rbtSummaryWise.Checked = True Then
            strTitle += "(SUMMARYWISE) "
        End If
        lblTitle.Text = strTitle
        lblTitle.Height = gridView.ColumnHeadersHeight
        If dtMisecellaneous.Rows.Count > 0 Then tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub
    Private Sub Report()
        If rbtDetailWise.Checked = True Then chkStuddedStone.Checked = True
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, True)
        Dim StkType As String = ""
        If chkCmbStkType.Text <> "ALL" And chkCmbStkType.Text <> "" Then
            If chkCmbStkType.Text.Contains("MANUFACTURING") Then
                StkType += "'M',"
            End If
            If chkCmbStkType.Text.Contains("EXEMPTED") Then
                StkType += "'E',"
            End If
            If chkCmbStkType.Text.Contains("TRADING") Then
                StkType += "'T','',"
            End If
            If StkType <> "" Then
                StkType = Mid(StkType, 1, StkType.Length - 1)
            End If
        End If
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPMI')>0 DROP TABLE TEMPTABLEDB..TEMPMI"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "  SELECT "
        strSql += vbCrLf + "  I.TRANNO,I.TRANDATE,CA.CATNAME"
        strSql += vbCrLf + "  ,DI.DESIGNERNAME"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(250),IM.ITEMNAME) AS ITEM,SM.SUBITEMNAME AS SUBITEM,IC.ITEMCTRNAME AS COUNTER"
        strSql += vbCrLf + "  ,I.TAGNO,SUM(I.PCS)PCS,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.PUREWT)PUREWT,CONVERT(VARCHAR(15),I.SNO)SNO"
        ' strSql += vbCrLf + ",SUM(IST.STNPCS)STPCS,SUM(IST.STNWT)STWT"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),'')PCPCS,CONVERT(VARCHAR(20),'')PCGRSWT"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),'')PCNETWT"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),'')PCPUREWT,CONVERT(INT,NULL)STNPCS"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(20,3),NULL)STNGRSWT,CONVERT(NUMERIC(20,3),NULL)STNNETWT"
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)DIPCS,CONVERT(NUMERIC(20,3),NULL)DIGRSWT"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(20,3),NULL)DINETWT,CONVERT(INT,1)ORD,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,NULL)RESULT"
        strSql += vbCrLf + "  ,EM.EMPNAME,I.REMARK1,I.REMARK2,CONVERT(VARCHAR(10),TAG.ORDREPNO) AS ORNO,CA.CATNAME AS TCATNAME,IM.ITEMNAME AS TITEM"
        strSql += vbCrLf + "  ,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.ITEMID) METALID,IM.DIASTONE "
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.ACCODE,'')<>'' THEN AC.ACNAME ELSE ISNULL(PIO.PNAME,'') END  AS CUSTOMER"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.USERID,0)<>0 THEN U.USERNAME ELSE '' END  AS USERNAME"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(I.STKTYPE,'')='E' THEN 'EXEMPTED' WHEN ISNULL(I.STKTYPE,'')='M' THEN 'MANUFACTURING' ELSE 'TRADING' END AS STKTYPE"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPMI"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON IC.ITEMCTRID = I.ITEMCTRID "
        ' strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSSTONE AS IST ON IST.ISSSNO = I.SNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..EMPMASTER AS EM ON EM.EMPID = I.EMPID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CU ON CU.BATCHNO =I.BATCHNO "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS PIO ON PIO.SNO=CU.PSNO "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AS AC ON ISNULL(AC.ACCODE,'') =ISNULL(I.ACCODE ,'')"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..DESIGNER AS DI ON DI.DESIGNERID=I.TAGDESIGNER"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER AS U ON U.USERID =I.USERID"
        If rbtBoth.Checked Then
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS TAG ON TAG.ITEMID = I.ITEMID AND TAG.TAGNO = I.TAGNO AND TAG.BATCHNO = I.BATCHNO AND TAG.COSTID = I.COSTID"
        ElseIf rbtNormal.Checked Then
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TAG ON TAG.ITEMID = I.ITEMID AND TAG.TAGNO = I.TAGNO AND TAG.BATCHNO = I.BATCHNO  AND TAG.COSTID = I.COSTID"
            strSql += vbCrLf + " AND ISNULL(TAG.ORDREPNO,'') = ''"
        ElseIf rbtOrder.Checked Then
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TAG ON TAG.ITEMID = I.ITEMID AND TAG.TAGNO = I.TAGNO AND TAG.BATCHNO = I.BATCHNO  AND TAG.COSTID = I.COSTID"
            strSql += vbCrLf + " AND ISNULL(TAG.ORDREPNO,'') <> ''"
        End If
        strSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "  AND I.TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + "  AND I.COMPANYID IN (" & SelectedCompany & ")"
        If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
            strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'))"
        End If
        If cmbCategory.Text <> "ALL" And cmbMetalName.Text <> "" Then
            strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        If ChkCmbDesigner.Text <> "ALL" And ChkCmbDesigner.Text <> "" Then
            strSql += vbCrLf + " AND I.TAGDESIGNER IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN(" & GetChecked_CheckedList(ChkCmbDesigner, True) & "))"
        End If
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            strSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        If CCENTRE <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & CCENTRE & "))"
        End If
        If NOID <> "" Then
            strSql += vbCrLf + " AND I.SYSTEMID IN (" & NOID & ")"
        End If
        If filItemCtrId.ToString <> "" Then
            strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & filItemCtrId.ToString & "))"
        End If
        If StkType <> "" Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') IN (" & StkType & ")"
        End If
        strSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,CA.CATNAME,TAG.ORDREPNO"
        strSql += vbCrLf + "  ,IM.ITEMNAME,SM.SUBITEMNAME,IC.ITEMCTRNAME"
        strSql += vbCrLf + "  ,I.TAGNO,I.PCS,I.GRSWT,I.NETWT,I.PUREWT,I.SNO,EM.EMPNAME"
        strSql += vbCrLf + "  ,I.REMARK1,I.REMARK2,I.ITEMID,IM.DIASTONE,PIO.PNAME,I.ACCODE,AC.ACNAME"
        strSql += vbCrLf + "  ,DI.DESIGNERNAME,I.USERID,U.USERNAME,I.STKTYPE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "ALTER TABLE TEMPTABLEDB..TEMPMI ALTER COLUMN STKTYPE VARCHAR(35)NULL"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = ""
        If chkStuddedStone.Checked And chksepcol.Checked = True Then
            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPMI)>0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPMI(TRANNO,TRANDATE,CATNAME,SUBITEM,COUNTER,STNPCS,STNGRSWT,STNNETWT,PUREWT,PCPCS,PCGRSWT,PCNETWT,PCPUREWT,SNO,ORD,TITEM,TCATNAME,DIASTONE)"
            strSql += vbCrLf + "  SELECT T.TRANNO,T.TRANDATE,CA.CATNAME,SM.SUBITEMNAME AS SUBITEM,COUNTER,ST.STNPCS,CONVERT(NUMERIC(10,3),ST.STNWT),CONVERT(NUMERIC(10,3),ST.STNWT),NULL,NULL,NULL,NULL,NULL,ST.ISSSNO AS SNO,2 ORD,T.TITEM,T.TCATNAME"
            strSql += vbCrLf + "  ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ST.STNITEMID)"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
            strSql += vbCrLf + "  INNER JOIN TEMPTABLEDB..TEMPMI AS T ON T.SNO = ST.ISSSNO"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
            strSql += vbCrLf + "  WHERE IM.METALID = 'T' "
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPMI(TRANNO,TRANDATE,CATNAME,SUBITEM,COUNTER,DIPCS,DIGRSWT,DINETWT,PUREWT,PCPCS,PCGRSWT,PCNETWT,PCPUREWT,SNO,ORD,TITEM,TCATNAME,DIASTONE)"
            strSql += vbCrLf + "  SELECT T.TRANNO,T.TRANDATE,CA.CATNAME,SM.SUBITEMNAME AS SUBITEM,COUNTER,ST.STNPCS,CONVERT(NUMERIC(10,3),ST.STNWT),CONVERT(NUMERIC(10,3),ST.STNWT),NULL,NULL,NULL,NULL,NULL,ST.ISSSNO AS SNO,2 ORD,T.TITEM,T.TCATNAME"
            strSql += vbCrLf + "  ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ST.STNITEMID)"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
            strSql += vbCrLf + "  INNER JOIN TEMPTABLEDB..TEMPMI AS T ON T.SNO = ST.ISSSNO"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
            strSql += vbCrLf + "  WHERE IM.METALID = 'D' "
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPMI SET ITEM = CASE WHEN ORD = 2 THEN '  ' + ITEM ELSE ITEM END"
            If chkSubItem.Checked Then
                ' strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPMI SET SUBITEM = NULL"
            End If
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  "

        ElseIf chkStuddedStone.Checked = True Then
            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPMI)>0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPMI(TRANNO,TRANDATE,CATNAME,ITEM,SUBITEM,COUNTER,STNPCS,STNGRSWT,STNNETWT,PUREWT,PCPCS,PCGRSWT,PCNETWT,PCPUREWT,SNO,ORD,TITEM,TCATNAME,DIASTONE)"
            strSql += vbCrLf + "  SELECT T.TRANNO,T.TRANDATE,CA.CATNAME,IM.ITEMNAME,SM.SUBITEMNAME AS SUBITEM,COUNTER,ST.STNPCS,ST.STNWT,ST.STNWT,NULL,NULL,NULL,NULL,NULL,ST.ISSSNO AS SNO,2 ORD,T.TITEM,T.TCATNAME"
            strSql += vbCrLf + "  ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ST.STNITEMID)"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
            strSql += vbCrLf + "  INNER JOIN TEMPTABLEDB..TEMPMI AS T ON T.SNO = ST.ISSSNO"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPMI SET ITEM = CASE WHEN ORD = 2 THEN '  ' + ITEM ELSE ITEM END"
            If chkSubItem.Checked Then
                'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPMI SET SUBITEM = NULL"
            End If
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  "
        End If
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        Dim SQL As String = Nothing
        SQL = "  IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP')>0 DROP TABLE TEMP"
        SQL += "  SELECT * INTO TEMP FROM ("
        SQL += " SELECT DISTINCT IM.STNITEMID,T.SNO,IM.STNPCS,CONVERT(NUMERIC(10,3),IM.STNWT)STNWT,IM.STONEUNIT,IM.SNO IMSNO FROM " & cnStockDb & "..ISSSTONE IM"
        SQL += " LEFT OUTER JOIN TEMPTABLEDB..TEMPMI T ON T.SNO=IM.ISSSNO )X"
        dcmd = New OleDbCommand(SQL, cn) : dcmd.CommandTimeout = 1000
        dcmd.CommandTimeout = 1000
        dcmd.ExecuteNonQuery()

        SQL = " SELECT STNITEMID,SNO,SUM(STNPCS)STNPCS,SUM(CONVERT(NUMERIC(10,3),STNWT))STNWT,IM.DIASTONE,T.STONEUNIT FROM TEMP T "
        SQL += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=T.STNITEMID"
        SQL += " GROUP BY STNITEMID,SNO,IM.DIASTONE,T.STONEUNIT "
        SQL += " ORDER BY SNO"
        das = New OleDbDataAdapter(SQL, cn)
        Dim dts As New DataTable()
        dts.Clear()
        das.Fill(dts)

        If rbtDetailWise.Checked Then
            If chkGroupCounter.Checked Then
                strSql = vbCrLf + "   SELECT * FROM (   "
                strSql += vbCrLf + "   SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,CATNAME,ITEM,SUBITEM,COUNTER,TAGNO"
                strSql += vbCrLf + "   ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,EMPNAME,DESIGNERNAME,REMARK1,REMARK2,ORNO,COLHEAD,1 RESULT,ORD,SNO,TRANNO TTRANNO,TRANDATE TTRANDATE,TITEM,TCATNAME,DIASTONE,CUSTOMER,USERNAME,STKTYPE AS STOCKTYPE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + "   GROUP BY TRANNO,TRANDATE,CATNAME,ITEM,SUBITEM,COUNTER,TAGNO,EMPNAME,DESIGNERNAME,REMARK1,REMARK2,ORNO,COLHEAD,RESULT,ORD,SNO,TITEM,TCATNAME,DIASTONE,CUSTOMER,USERNAME,STKTYPE"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   SELECT DISTINCT NULL ,NULL ,COUNTER ,NULL ,NULL ,COUNTER ,NULL,NULL ,NULL,NULL ,NULL,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PUPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT ,NULL "
                strSql += vbCrLf + "   ,NULL ,NULL ,NULL,NULL ,NULL ,0 ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,NULL,NULL,NULL FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   SELECT NULL TRANNO,NULL TRANDATE,'SUB TOTAL' CATNAME,NULL,NULL SUBITEM,K.COUNTER,NULL TAGNO"
                strSql += vbCrLf + "   ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G1' COLHEAD,2 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME,'' DIASTONE,'' CUSTOMER,''USERNAME,NULL"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPMI AS K WHERE ORD = 1 GROUP BY K.COUNTER"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   SELECT NULL TRANNO,NULL TRANDATE,'ZZZZZZ' CATNAME,'GRAND TOTAL',NULL SUBITEM,'ZZZZZZ' COUNTER,NULL TAGNO"
                strSql += vbCrLf + "   ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G1' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME,'' DIASTONE,'' CUSTOMER,''USERNAME,NULL"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   SELECT NULL TRANNO,NULL TRANDATE,'ZZZZZZ' CATNAME,CASE WHEN DIASTONE ='S' THEN 'STUD. STONE' WHEN DIASTONE ='D' THEN 'STUD. DIAMOND' ELSE 'STUD. PRECIOUS' END "
                strSql += vbCrLf + "   ,NULL SUBITEM,'ZZZZZZ' COUNTER,NULL TAGNO ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G2' COLHEAD,5 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME, DIASTONE,'' CUSTOMER,''USERNAME,NULL"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPMI WHERE ORD = 2"
                strSql += vbCrLf + "   GROUP BY DIASTONE)X"
                strSql += vbCrLf + "   ORDER BY COUNTER,RESULT,CATNAME,ITEM,SUBITEM"
            Else
                strSql = vbCrLf + "    SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,CATNAME,ITEM,SUBITEM,COUNTER,TAGNO"
                strSql += vbCrLf + "   ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,EMPNAME,DESIGNERNAME,REMARK1,REMARK2,ORNO,COLHEAD,1 RESULT,ORD,SNO,TRANNO TTRANNO,TRANDATE TTRANDATE,TITEM,TCATNAME,DIASTONE,CUSTOMER,USERNAME,STKTYPE AS STOCKTYPE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + "   GROUP BY TRANNO,TRANDATE,CATNAME,ITEM,SUBITEM,COUNTER,TAGNO,EMPNAME,DESIGNERNAME,REMARK1,REMARK2,ORNO,COLHEAD,RESULT,ORD,SNO,TITEM,TCATNAME,DIASTONE,CUSTOMER,USERNAME,STKTYPE"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "    SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,'GRAND TOTAL',NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + "   ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G1' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME,'' DIASTONE,'' CUSTOMER,''USERNAME,NULL"
                strSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                'strSql += vbCrLf + " GROUP BY DIASTONE"

                strSql += vbCrLf + "   UNION ALL"

                strSql += vbCrLf + "   SELECT NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL, NULL,NULL,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT ,NULL "
                strSql += vbCrLf + "   ,NULL,NULL ,NULL ,NULL ,NULL ,4 ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,NULL,NULL,NULL "

                strSql += vbCrLf + "   UNION ALL"

                strSql += vbCrLf + "  SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,CASE WHEN DIASTONE ='S' THEN 'STUD. STONE' WHEN DIASTONE ='D' THEN 'STUD. DIAMOND' ELSE 'STUD. PRECIOUS' END "
                strSql += vbCrLf + "  ,NULL SUBITEM,NULL COUNTER,NULL TAGNO ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G2' COLHEAD,5 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME, DIASTONE,'' CUSTOMER,''USERNAME,NULL"
                strSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPMI WHERE ORD = 2"
                strSql += vbCrLf + "    GROUP BY DIASTONE"

                strSql += vbCrLf + "   UNION ALL"

                strSql += vbCrLf + "   SELECT NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL ,NULL,NULL ,NULL,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PUPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT ,NULL "
                strSql += vbCrLf + "   ,NULL ,NULL ,NULL,NULL ,NULL ,6 ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,null,NULL,NULL "
                If chkitemwisesubtot.Checked = True Then
                    strSql += vbCrLf + "   UNION ALL"
                    strSql += vbCrLf + "  SELECT  TRANNO,NULL TRANDATE,NULL CATNAME,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=T.METALID) "
                    strSql += vbCrLf + "  ,NULL SUBITEM,NULL COUNTER,NULL TAGNO ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                    strSql += vbCrLf + "   ,NULL EMPNAME,NULL DESIGNERNAME,NULL REMARK1,NULL ORNO,NULL REMARK2,'G3' COLHEAD,1 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL TCATNAME,DIASTONE,''CUSTOMER,''USERNAME,NULL"
                    strSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPMI T WHERE ORD = 1"
                    strSql += vbCrLf + "    GROUP BY METALID,DIASTONE,TRANNO"
                    strSql += vbCrLf + "  ORDER BY RESULT,TRANNO,TTRANDATE DESC" 'RESULT,TTRANDATE,TTRANNO,SNO,ORD,DIASTONE"
                End If
            End If
        Else
            If rbtBillDateWise.Checked Then
                strSql = vbCrLf + "  SELECT NULL TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,NULL CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" ',SUM(STPCS)STPCS,SUM(STWT)STWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,COLHEAD,RESULT,ORD,NULL SNO,NULL TTRANNO,TRANDATE TTRANDATE,DIASTONE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + " GROUP BY TRANDATE,COLHEAD,RESULT,ORD,DIASTONE"

                strSql += vbCrLf + " UNION ALL"

                strSql += vbCrLf + "  SELECT NULL TRANNO,'GRAND TOTAL' TRANDATE,NULL CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" ',SUM(STPCS),SUM(STWT)"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'G' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,DIASTONE"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + " GROUP BY DIASTONE"
                strSql += vbCrLf + "  ORDER BY RESULT,TTRANDATE,ORD,DIASTONE"
            ElseIf rbtBillNoWise.Checked Then
                strSql = vbCrLf + "  SELECT CONVERT(VARCHAR,TRANNO)TRANNO,CONVERT(VARCHAR,NULL)TRANDATE,NULL CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,''PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS)STPCS,SUM(STWT)STWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,COLHEAD,RESULT,ORD,NULL SNO,TRANNO TTRANNO,NULL TTRANDATE,DIASTONE,STKTYPE AS STOCKTYPE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + " GROUP BY TRANNO,COLHEAD,RESULT,ORD,DIASTONE,STKTYPE"

                strSql += vbCrLf + " UNION ALL"

                strSql += vbCrLf + "  SELECT 'GRAND TOTAL' TRANNO,NULL TRANDATE,NULL CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS),SUM(STWT)"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'G' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,DIASTONE"
                strSql += vbCrLf + "  ,'' AS STOCKTYPE FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + " GROUP BY DIASTONE"
                strSql += vbCrLf + "  ORDER BY RESULT,TTRANNO,ORD"

            ElseIf rbtCategoryWise.Checked Then
                strSql = vbCrLf + "  SELECT NULL TRANNO,NULL TRANDATE,CATNAME CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS)STPCS,SUM(STWT)STWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,COLHEAD,RESULT,ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,TCATNAME,DIASTONE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + " GROUP BY CATNAME,TCATNAME,COLHEAD,RESULT,ORD,DIASTONE"

                strSql += vbCrLf + " UNION ALL"

                strSql += vbCrLf + "  SELECT NULL TRANNO,NULL TRANDATE,'GRAND TOTAL'CATNAME,NULL ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(PUREWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,''PCPUREWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS),SUM(STWT)"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'G' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TCATNAME,DIASTONE"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + " GROUP BY DIASTONE"
                strSql += vbCrLf + "  ORDER BY RESULT,TCATNAME,ORD"
            ElseIf rbtItemNameWise.Checked Then
                strSql = vbCrLf + "  SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS)STPCS,SUM(STWT)STWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,COLHEAD,RESULT,ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,TITEM,DIASTONE FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + " GROUP BY ITEM,TITEM,COLHEAD,RESULT,ORD,DIASTONE"

                strSql += vbCrLf + " UNION ALL"

                strSql += vbCrLf + "  SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,'GRAND TOTAL' ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT" '--,SUM(STPCS),SUM(STWT)"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'G' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,DIASTONE"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + " GROUP BY DIASTONE"
                strSql += vbCrLf + "  ORDER BY RESULT,TITEM,ORD"
            ElseIf rbtMetalNameWise.Checked Then
                strSql = vbCrLf + " SELECT DISTINCT NULL TRANNO,NULL TRANDATE,NULL CATNAME,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=T.METALID) ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,NULL PCS,NULL GRSWT,NULL NETWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'T' COLHEAD,0 RESULT,0 ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,NULL DIASTONE,METALID FROM TEMPTABLEDB..TEMPMI T"
                strSql += vbCrLf + " GROUP BY METALID"
                strSql += vbCrLf + " UNION ALL		"
                strSql += vbCrLf + " SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,COLHEAD,1 RESULT,ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,TITEM,DIASTONE,METALID FROM TEMPTABLEDB..TEMPMI"
                strSql += vbCrLf + " GROUP BY ITEM,TITEM,COLHEAD,RESULT,ORD,DIASTONE,METALID"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=T.METALID) + ' TOTAL' ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'S' COLHEAD,2 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,DIASTONE,METALID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPMI T WHERE ORD = 1"
                strSql += vbCrLf + " GROUP BY DIASTONE,METALID"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT NULL TRANNO,NULL TRANDATE,NULL CATNAME,'GRAND TOTAL' ITEM,NULL SUBITEM,NULL COUNTER,NULL TAGNO"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),'' PCPCS,'' PCGRSWT,'' PCNETWT,'' STNPCS,'' STNGRSWT,'' STNNETWT,'' DIPCS,'' DIGRSWT,'' DINETWT"
                strSql += vbCrLf + " ,NULL EMPNAME,NULL REMARK1,NULL REMARK2,NULL ORNO,'G' COLHEAD,3 RESULT,NULL ORD,NULL SNO,NULL TTRANNO,NULL TTRANDATE,NULL TITEM,'' DIASTONE,'Z' METALID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPMI WHERE ORD = 1"
                strSql += vbCrLf + " ORDER BY METALID,RESULT,TITEM,ORD"
            End If
        End If
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        If rbtDetailWise.Checked = True And chksepcol.Checked = True And rbtBoth.Checked = True Then
            Dim PCPCSTOT, PCGWTTOT, PCGCTTOT, STPCSTOT, STGTWTTOT, STGTCTTOT, DIPCSTOT, DIGTWTTOT, DIGTCTTOT As Decimal
            Dim dupdt As New DataTable
            dupdt = dtGrid.Copy()
            Dim rowcnt(dupdt.Rows.Count - 1) As Int64
            For i As Integer = 0 To dupdt.Rows.Count - 1
                For k As Integer = 0 To dts.Rows.Count - 1
                    If dupdt.Rows(i).Item("ORD").ToString() = "1" Then
                        If dupdt.Rows(i).Item("SNO").ToString = dts.Rows(k).Item("SNO").ToString Then
                            If dts.Rows(k).Item("DIASTONE").ToString() = "D" Then
                                If chksepcol.Checked = True Then
                                    If Val(dts.Rows(k).Item("STNPCS").ToString) <> 0 Then
                                        dupdt.Rows(i).Item("DIPCS") = Val(dupdt.Rows(i).Item("DIPCS").ToString) + Val(dts.Rows(k).Item("STNPCS").ToString) 'Val(dtGrid.Rows(i).Item("PCS").ToString())
                                    End If
                                    DIPCSTOT = DIPCSTOT + Val(dts.Rows(k).Item("STNPCS").ToString)
                                    If dts.Rows(k).Item("STONEUNIT").ToString() = "C" Then
                                        dupdt.Rows(i).Item("DINETWT") = Format(Val(dupdt.Rows(i).Item("DINETWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000") 'Val(dtGrid.Rows(i).Item("GRSWT").ToString())
                                        DIGTCTTOT = DIGTCTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    Else
                                        dupdt.Rows(i).Item("DIGRSWT") = Format(Val(dupdt.Rows(i).Item("DIGRSWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000") 'Val(dtGrid.Rows(i).Item("GRSWT").ToString())
                                        DIGTWTTOT = DIGTWTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    End If
                                Else
                                    dupdt.Rows(i).Item("DIPCS") = Val(dts.Rows(k).Item("STNPCS").ToString) 'Val(dtGrid.Rows(i).Item("PCS").ToString())
                                    dupdt.Rows(i).Item("DIGRSWT") = Val(dts.Rows(k).Item("STNWT").ToString) 'Val(dtGrid.Rows(i).Item("GRSWT").ToString())
                                    DIPCSTOT = DIPCSTOT + dupdt.Rows(i).Item("DIPCS")
                                    DIGTWTTOT = DIGTWTTOT + dupdt.Rows(i).Item("DIGRSWT")
                                End If

                                'dupdt.Rows(i).Item("PCS") = 0
                                'dupdt.Rows(i).Item("GRSWT") = 0
                                'dupdt.Rows(i).Item("NETWT") = 0
                                rowcnt(i) = Val(dupdt.Rows(i).Item("KEYNO").ToString())
                            End If
                            If dts.Rows(k).Item("DIASTONE").ToString() = "S" Then
                                If chksepcol.Checked = True Then
                                    If Val(dts.Rows(k).Item("STNPCS").ToString) <> 0 Then
                                        dupdt.Rows(i).Item("STNPCS") = Val(dupdt.Rows(i).Item("STNPCS").ToString) + Val(dts.Rows(k).Item("STNPCS").ToString)
                                    End If
                                    STPCSTOT = STPCSTOT + Val(dts.Rows(k).Item("STNPCS").ToString)
                                    If dts.Rows(k).Item("STONEUNIT").ToString() = "C" Then
                                        dupdt.Rows(i).Item("STNNETWT") = Format(Val(dupdt.Rows(i).Item("STNNETWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000")
                                        STGTCTTOT = STGTCTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    Else
                                        dupdt.Rows(i).Item("STNGRSWT") = Format(Val(dupdt.Rows(i).Item("STNGRSWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000")
                                        STGTWTTOT = STGTWTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    End If
                                Else
                                    dupdt.Rows(i).Item("STNPCS") = Val(dts.Rows(k).Item("STNPCS").ToString)
                                    dupdt.Rows(i).Item("STNGRSWT") = (dts.Rows(k).Item("STNWT").ToString)
                                    STPCSTOT = STPCSTOT + dupdt.Rows(i).Item("STNPCS")
                                    STGTWTTOT = STGTWTTOT + dupdt.Rows(i).Item("STNGRSWT")
                                End If
                                'dupdt.Rows(i).Item("PCS") = 0
                                'dupdt.Rows(i).Item("GRSWT") = 0
                                'dupdt.Rows(i).Item("NETWT") = 0
                                rowcnt(i) = Val(dupdt.Rows(i).Item("KEYNO").ToString())
                            End If
                            If dts.Rows(k).Item("DIASTONE").ToString() = "P" Then
                                If chksepcol.Checked = True Then
                                    If Val(dts.Rows(k).Item("STNPCS").ToString) <> 0 Then
                                        dupdt.Rows(i).Item("PCPCS") = Val(dupdt.Rows(i).Item("PCPCS").ToString) + Val(dts.Rows(k).Item("STNPCS").ToString)
                                    End If
                                    PCPCSTOT = PCPCSTOT + Val(dts.Rows(k).Item("STNPCS").ToString)
                                    If dts.Rows(k).Item("STONEUNIT").ToString() = "C" Then
                                        dupdt.Rows(i).Item("PCNETWT") = Format(Val(dupdt.Rows(i).Item("PCNETWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000")
                                        PCGCTTOT = PCGCTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    Else
                                        dupdt.Rows(i).Item("PCGRSWT") = Format(Val(dupdt.Rows(i).Item("PCGRSWT").ToString) + Val(dts.Rows(k).Item("STNWT").ToString), "#0.000")
                                        PCGWTTOT = PCGWTTOT + Val(dts.Rows(k).Item("STNWT").ToString)
                                    End If
                                Else
                                    dupdt.Rows(i).Item("PCPCS") = Val(dts.Rows(k).Item("STNPCS").ToString)
                                    dupdt.Rows(i).Item("PCGRSWT") = Val(dts.Rows(k).Item("STNWT").ToString)
                                    PCPCSTOT = PCPCSTOT + dupdt.Rows(i).Item("PCPCS")
                                    PCGWTTOT = PCGWTTOT + dupdt.Rows(i).Item("PCGRSWT")
                                End If
                                'dupdt.Rows(i).Item("PCS") = 0
                                'dupdt.Rows(i).Item("GRSWT") = 0
                                'dupdt.Rows(i).Item("NETWT") = 0
                                rowcnt(i) = Val(dupdt.Rows(i).Item("KEYNO").ToString())
                            End If
                        End If
                    End If
                    If dupdt.Rows(i).Item("ITEM").ToString() = "GRAND TOTAL" Then
                        dupdt.Rows(i).Item("DIPCS") = IIf(Val(DIPCSTOT) <> 0, DIPCSTOT, "")
                        dupdt.Rows(i).Item("DIGRSWT") = IIf(Val(DIGTWTTOT) <> 0, Format(DIGTWTTOT, "#0.000"), "")
                        dupdt.Rows(i).Item("DINETWT") = IIf(Val(DIGTCTTOT) <> 0, Format(DIGTCTTOT, "#0.000"), "")
                        dupdt.Rows(i).Item("STNPCS") = IIf(Val(STPCSTOT) <> 0, STPCSTOT, "")
                        dupdt.Rows(i).Item("STNGRSWT") = IIf(Val(STGTWTTOT) <> 0, Format(STGTWTTOT, "#0.000"), "")
                        dupdt.Rows(i).Item("STNNETWT") = IIf(Val(STGTCTTOT) <> 0, Format(STGTCTTOT, "#0.000"), "")
                        dupdt.Rows(i).Item("PCPCS") = IIf(Val(PCPCSTOT) <> 0, PCPCSTOT, "")
                        dupdt.Rows(i).Item("PCGRSWT") = IIf(Val(PCGWTTOT) <> 0, Format(PCGWTTOT, "#0.000"), "")
                        dupdt.Rows(i).Item("PCNETWT") = IIf(Val(PCGCTTOT) <> 0, Format(PCGCTTOT, "#0.000"), "")
                    End If
                Next
            Next
            Dim findval As Integer = dtGrid.Rows.Count - 1
            For m As Integer = 0 To findval
                If dtGrid.Rows(m).Item("ORD").ToString() = "2" Then
                    dupdt.Rows(m).Delete()
                    findval = findval - 1
                End If
            Next
            dtGrid = New DataTable()
            dtGrid = dupdt.Copy()
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "MISCELLANEOUS REPORT "
        Dim tit As String = "MISCELLANEOUS REPORT "
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        BrighttechPack.GlobalMethods.FormatGridColumns(objGridShower.gridView, True, , True)
        With objGridShower.gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            If rbtDetailWise.Checked Then
                If .Columns.Contains("STOCKTYPE") Then .Columns("STOCKTYPE").Visible = True
                .Columns("TRANNO").Visible = True
                .Columns("TRANDATE").Visible = True
                .Columns("ITEM").Visible = True
                .Columns("SUBITEM").Visible = chkSubItem.Checked
                .Columns("TAGNO").Visible = True
                .Columns("PCS").Visible = True
                .Columns("GRSWT").Visible = True
                .Columns("NETWT").Visible = True
                .Columns("PUREWT").Visible = True
                .Columns("EMPNAME").Visible = True
                .Columns("DESIGNERNAME").Visible = True
                .Columns("REMARK1").Visible = True
                .Columns("REMARK2").Visible = True
                .Columns("CATNAME").Visible = True
                .Columns("CUSTOMER").Visible = True
                .Columns("USERNAME").Visible = True
                .Columns("ORNO").Visible = rbtBoth.Checked Or rbtOrder.Checked
                If chksepcol.Checked = True And rbtBoth.Checked = True Then
                    .Columns("PCPCS").Visible = True
                    .Columns("PCGRSWT").Visible = True
                    .Columns("PCNETWT").Visible = True
                    .Columns("PCPUREWT").Visible = False
                    .Columns("STNPCS").Visible = True
                    .Columns("STNGRSWT").Visible = True
                    .Columns("STNNETWT").Visible = True
                    .Columns("DIPCS").Visible = True
                    .Columns("DIGRSWT").Visible = True
                    .Columns("DINETWT").Visible = True
                    .Columns("DIASTONE").Visible = False

                    .Columns("PCPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PCGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PCNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DINETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                    .Columns("PCPCS").HeaderText = "PSTN.PCS"
                    .Columns("PCGRSWT").HeaderText = "PSTN.GM"
                    .Columns("PCNETWT").HeaderText = "PSTN.CT"
                    ''.Columns("PCPUREWT").HeaderText = "PSTN.WT"
                    .Columns("STNPCS").HeaderText = "STN.PCS"
                    .Columns("STNGRSWT").HeaderText = "STN.GM"
                    .Columns("STNNETWT").HeaderText = "STN.CT"
                    .Columns("DIPCS").HeaderText = "DIA.PCS"
                    .Columns("DIGRSWT").HeaderText = "DIA.GM"
                    .Columns("DINETWT").HeaderText = "DIA.CT"
                End If

            Else
                If rbtBillDateWise.Checked Then
                    .Columns("TRANDATE").Visible = True
                ElseIf rbtBillNoWise.Checked Then
                    .Columns("TRANNO").Visible = True
                ElseIf rbtCategoryWise.Checked Then
                    .Columns("CATNAME").Visible = True
                ElseIf rbtItemNameWise.Checked Then
                    .Columns("ITEM").Visible = True
                ElseIf rbtMetalNameWise.Checked Then
                    .Columns("ITEM").Visible = True
                End If
                .Columns("PCS").Visible = True
                .Columns("GRSWT").Visible = True
                .Columns("NETWT").Visible = True
                .Columns("STNPCS").Visible = True
                .Columns("STNGRSWT").Visible = True
            End If
        End With
        For j As Integer = 0 To objGridShower.gridView.Rows.Count - 1
            If objGridShower.gridView("ITEM", j).Value.ToString <> "" Then
                If objGridShower.gridView("ITEM", j).Value.ToString = "GRAND TOTAL" Then
                    objGridShower.gridView.Rows(j).DefaultCellStyle.BackColor = Color.LightGreen
                    objGridShower.gridView.Rows(j).DefaultCellStyle.ForeColor = Color.Black
                    objGridShower.gridView.Rows(j).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
            End If
        Next
        objGridShower.Show()
        If chkGroupCounter.Checked Then
            For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
                With dgvRow
                    Select Case .Cells("RESULT").Value.ToString
                        Case "0"
                            .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                        Case "2"
                            .Cells("CATNAME").Style.BackColor = reportHeadStyle1.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle1.Font
                        Case "3"
                            .Cells("CATNAME").Value = ""
                            .Cells("ITEM").Style.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                        Case "5"
                            .Cells("CATNAME").Value = ""
                            .Cells("ITEM").Style.BackColor = reportHeadStyle1.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle1.Font
                        Case "6"
                            .Cells("CATNAME").Value = ""
                            .Cells("ITEM").Style.BackColor = reportHeadStyle2.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle2.Font
                    End Select
                End With
            Next
        ElseIf rbtMetalNameWise.Checked Then
            For j As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView("COLHEAD", j).Value.ToString = "" Then Continue For
                If objGridShower.gridView("COLHEAD", j).Value.ToString = "G" Then
                    objGridShower.gridView.Rows(j).DefaultCellStyle.BackColor = Color.LightBlue
                    objGridShower.gridView.Rows(j).DefaultCellStyle.ForeColor = Color.Red
                    objGridShower.gridView.Rows(j).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf objGridShower.gridView("COLHEAD", j).Value.ToString = "T" Then
                    objGridShower.gridView.Rows(j).Cells("ITEM").Style.BackColor = Color.LightBlue
                    objGridShower.gridView.Rows(j).DefaultCellStyle.ForeColor = Color.Black
                    objGridShower.gridView.Rows(j).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf objGridShower.gridView("COLHEAD", j).Value.ToString = "S" Then
                    objGridShower.gridView.Rows(j).DefaultCellStyle.BackColor = Color.LightGreen
                    objGridShower.gridView.Rows(j).DefaultCellStyle.ForeColor = Color.Black
                    objGridShower.gridView.Rows(j).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
            Next
        End If


        'globalMethods.FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkLstCostCentre.Enabled = True Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkLstCostCentre.SetItemChecked(0, True)
            End If
        End If
        If Not chkLstNodeId.CheckedItems.Count > 0 Then
            If chkLstNodeId.Items.Count = 0 Then
                funcAddNodeId()
            End If
            chkLstNodeId.SetItemChecked(0, True)
        End If
        If chkLstItemCounter.Items.Count > 0 Then
            If Not chkLstItemCounter.CheckedItems.Count > 0 Then chkLstItemCounter.SetItemChecked(0, True)
        End If
        funcFilter()
        If ChkPurDet.Checked Then
            NewReport()
            Exit Sub
        End If
        Report()
        Exit Sub
        Dim dtMisecellaneous As New DataTable
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        'Me.Cursor = Cursors.WaitCursor
        btnView_Search.Enabled = False
        dtMisecellaneous.Clear()
        gridView.DataSource = Nothing
        Try
            Dim SummaryDetail As String = Nothing
            If rbtSummaryWise.Checked = True Then
                SummaryDetail = "S"
            ElseIf rbtDetailWise.Checked = True Then
                SummaryDetail = "D"
            End If
            Dim SummaryWise As String = Nothing
            If rbtBillDateWise.Checked = True Then
                SummaryWise = "A"
            ElseIf rbtBillNoWise.Checked = True Then
                SummaryWise = "N"
            ElseIf rbtCategoryWise.Checked = True Then
                SummaryWise = "C"
            ElseIf rbtItemNameWise.Checked = True Then
                SummaryWise = "I"
            End If
            Dim StudStone As String = Nothing
            If chkStuddedStone.Checked = True Then
                StudStone = "Y"
            Else
                StudStone = "N"
            End If
            ''cmd.CommandType = CommandType.StoredProcedure
            strSql = " EXEC " & cnStockDb & "..SP_RPT_MISCELLANEOUS"
            strSql += vbCrLf + "  @DATEFROM = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@DATETO = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@SUMMARYDETAIL ='" & SummaryDetail & "'"
            strSql += vbCrLf + " ,@SUMMARYWISE='" & SummaryWise & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += vbCrLf + " ,@ITEMNAME = '" & Replace(cmbItemName.Text, "'", "''''") & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & Replace(CCENTRE, "'", "''") & "'"
            strSql += vbCrLf + " ,@CATEGORY = '" & Replace(cmbCategory.Text, "'", "''''") & "'"
            strSql += vbCrLf + " ,@NODEID = '" & Replace(NOID, "'", "''") & "'"
            strSql += vbCrLf + " ,@STUDSTONE='" & StudStone & "'"
            strSql += vbCrLf + " ,@SystemId='" & systemId & "'"
            strSql += vbCrLf + " ,@cnAdminDB='" & cnAdminDb & "'"
            strSql += vbCrLf + " ,@cnStockDB='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@COMPANYID='" & SelectedCompany & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If rbtDetailWise.Checked = True Then
                strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCDREP')"
                strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "MISCDREP"
                strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "MISCDREP("
                strSql += vbCrLf + " SNO VARCHAR(15),"
                strSql += vbCrLf + " BILLNO INT,"
                strSql += vbCrLf + " BILLDATE VARCHAR(12),"
                strSql += vbCrLf + " ITEMNAME VARCHAR(50),"
                strSql += vbCrLf + " SUBITEMNAME VARCHAR(50),"
                strSql += vbCrLf + " TAGNO VARCHAR(20),"
                strSql += vbCrLf + " PCS INT,"
                strSql += vbCrLf + " GRSWT NUMERIC(15,3),"
                strSql += vbCrLf + " NETWT NUMERIC(15,3),"
                strSql += vbCrLf + " BATCHNO VARCHAR(20),"
                strSql += vbCrLf + " RESULT INT,"
                strSql += vbCrLf + " ITEMID INT,"
                strSql += vbCrLf + " COLHEAD VARCHAR(1),"
                strSql += vbCrLf + " SSNO INT IDENTITY)"
            Else
                strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCSREP')"
                strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "MISCSREP"
                strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "MISCSREP("
                strSql += vbCrLf + " BILLNO INT,"
                strSql += vbCrLf + " BILLDATE VARCHAR(12),"
                strSql += vbCrLf + " DUMMYDATE SMALLDATETIME,"
                strSql += vbCrLf + " CATNAME VARCHAR(50),"
                strSql += vbCrLf + " CatCode VARCHAR(12),"
                strSql += vbCrLf + " ITEMNAME VARCHAR(50),"
                strSql += vbCrLf + " ItemId INT,"
                strSql += vbCrLf + " PCS INT,"
                strSql += vbCrLf + " GRSWT NUMERIC(15,3),"
                strSql += vbCrLf + " NETWT NUMERIC(15,3),"
                strSql += vbCrLf + " STONE VARCHAR(10),"
                strSql += vbCrLf + " RESULT INT,"
                strSql += vbCrLf + " COLHEAD VARCHAR(1),"
                strSql += vbCrLf + " SSNO INT IDENTITY)"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If rbtDetailWise.Checked = True Then
                strSql = " ALTER TABLE TEMP" & systemId & "MISEDETRPT ADD COLHEAD VARCHAR(1) "
            Else
                strSql = " ALTER TABLE TEMP" & systemId & "MISESUMRPT ADD COLHEAD VARCHAR(1) "
                strSql += vbCrLf + " ALTER TABLE TEMP" & systemId & "MISESUMRPT ADD DUMMYBILL INT DEFAULT 0"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If rbtDetailWise.Checked = True Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISEDETRPT)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISEDETRPT(PCS,GRSWT,NETWT,COLHEAD,RESULT,SNO)"
                strSql += vbCrLf + " SELECT  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'G'COLHEAD,3 RESULT, 999999"
                strSql += vbCrLf + " FROM TEMP" & systemId & "MISEDETRPT WHERE RESULT = 1"
                strSql += vbCrLf + " END "
            ElseIf rbtSummaryWise.Checked = True Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISESUMRPT)>0"
                strSql += vbCrLf + " BEGIN "
                If rbtBillDateWise.Checked = True Then
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISESUMRPT(PCS,GRSWT,NETWT,COLHEAD,RESULT,STONE,DUMMYDATE)"
                    strSql += vbCrLf + " SELECT  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'G'COLHEAD,3 RESULT,'','2078-09-30'DUMMYDATE "
                ElseIf rbtBillNoWise.Checked = True Then
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISESUMRPT(PCS,GRSWT,NETWT,COLHEAD,RESULT,STONE,DUMMYBILL)"
                    strSql += vbCrLf + " SELECT  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'G'COLHEAD,3 RESULT,'',999999"
                ElseIf rbtCategoryWise.Checked = True Then
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISESUMRPT(PCS,GRSWT,NETWT,COLHEAD,RESULT,STONE,CATCODE)"
                    strSql += vbCrLf + " SELECT  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'G'COLHEAD,3 RESULT,'','ZZZZZ' CATCODE"
                ElseIf rbtItemNameWise.Checked = True Then
                    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISESUMRPT(PCS,GRSWT,NETWT,COLHEAD,RESULT,STONE,ITEMID)"
                    strSql += vbCrLf + " SELECT  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,'G'COLHEAD,3 RESULT,'',999999"
                End If
                strSql += vbCrLf + " FROM TEMP" & systemId & "MISESUMRPT WHERE RESULT = 1"
                strSql += vbCrLf + " END "
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If rbtDetailWise.Checked = True Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISEDETRPT)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISCDREP(SNO,BILLNO,BILLDATE,ITEMNAME,"
                strSql += vbCrLf + " SUBITEMNAME,TAGNO,PCS,GRSWT,NETWT,BATCHNO,ITEMID,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT  SNO,BILLNO,BILLDATE,ITEMNAME,SUBITEMNAME,"
                strSql += vbCrLf + " TAGNO,PCS,GRSWT,NETWT,BATCHNO,ITEMID,COLHEAD,RESULT"
                strSql += vbCrLf + " FROM TEMP" & systemId & "MISEDETRPT ORDER BY SNO, RESULT, BILLNO"
                strSql += vbCrLf + " END "
            ElseIf rbtSummaryWise.Checked = True Then
                strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "MISESUMRPT)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "MISCSREP(PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT,"
                strSql += vbCrLf + " BILLDATE,DUMMYDATE,BILLNO,CATNAME,CATCODE,ITEMNAME,ITEMID)"
                strSql += " SELECT PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT,"
                If rbtBillDateWise.Checked = True Then
                    strSql += vbCrLf + " BILLDATE,DUMMYDATE,0 BILLNO,''CATNAME,''CATCODE,''ITEMNAME,0 ITEMID"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "MISESUMRPT "
                    strSql += vbCrLf + " ORDER BY DUMMYDATE ,RESULT ASC "
                ElseIf rbtBillNoWise.Checked = True Then
                    strSql += vbCrLf + " '' BILLDATE,NULL DUMMYDATE,BILLNO,''CATNAME,''CATCODE,''ITEMNAME,0 ITEMID"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "MISESUMRPT "
                    strSql += vbCrLf + " ORDER BY DUMMYBILL,BILLNO,RESULT ASC"
                ElseIf rbtCategoryWise.Checked = True Then
                    strSql += vbCrLf + " '' BILLDATE,NULL DUMMYDATE,0 BILLNO,CATNAME,CATCODE,''ITEMNAME,0 ITEMID"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "MISESUMRPT "
                    strSql += vbCrLf + " ORDER BY CATCODE,RESULT ASC"
                ElseIf rbtItemNameWise.Checked = True Then
                    strSql += vbCrLf + " '' BILLDATE,NULL DUMMYDATE,0 BILLNO,''CATNAME,''CATCODE,ITEMNAME,ITEMID"
                    strSql += vbCrLf + " FROM TEMP" & systemId & "MISESUMRPT "
                    strSql += vbCrLf + " ORDER BY ITEMID,RESULT ASC"
                End If
                strSql += vbCrLf + " END "
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            If rbtDetailWise.Checked = True Then
                strSql = "SELECT * FROM TEMP" & systemId & "MISCDREP ORDER BY RESULT,SSNO"
            Else
                strSql = " SELECT "
                If rbtBillDateWise.Checked = True Then
                    strSql += vbCrLf + " BILLDATE,PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT FROM TEMP" & systemId & "MISCSREP ORDER BY DUMMYDATE,RESULT"
                ElseIf rbtBillNoWise.Checked = True Then
                    strSql += vbCrLf + " BILLNO,PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT FROM TEMP" & systemId & "MISCSREP ORDER BY SSNO"
                ElseIf rbtCategoryWise.Checked = True Then
                    strSql += vbCrLf + " CATNAME,PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT FROM TEMP" & systemId & "MISCSREP ORDER BY SSNO"
                ElseIf rbtItemNameWise.Checked = True Then
                    strSql += vbCrLf + " ITEMNAME,PCS,GRSWT,NETWT,STONE,COLHEAD,RESULT FROM TEMP" & systemId & "MISCSREP ORDER BY SSNO"
                End If
            End If

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMisecellaneous)
            If dtMisecellaneous.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnView_Search.Focus()
                Exit Sub
            End If

            gridView.DataSource = dtMisecellaneous
            tabView.Show()
            GridViewFormat()
            funcGridStyle()
            lblTitle.Visible = True
            Dim strTitle As String = Nothing
            strTitle = " MISCELLANEOUSREPORT"
            strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            If chkStuddedStone.Checked = True Then
                strTitle += " WITH STUDDED STONE"
            End If
            If Strings.Right(strTitle, 3) = "AND" Then
                strTitle = Strings.Left(strTitle, strTitle.Length - 3)
            End If
            If rbtDetailWise.Checked = True Then
                strTitle += "(DETAILWISE) "
            ElseIf rbtSummaryWise.Checked = True Then
                strTitle += "(SUMMARYWISE) "
            End If
            lblTitle.Text = strTitle
            lblTitle.Height = gridView.ColumnHeadersHeight
            If dtMisecellaneous.Rows.Count > 0 Then tabMain.SelectedTab = tabView
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Arrow
            btnView_Search.Enabled = True
        End Try
        Prop_Sets()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub rtbDetailWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetailWise.CheckedChanged
        If rbtDetailWise.Checked = True Then
            grbSummaryWise.Visible = False
            chkSubItem.Checked = True
            chkSubItem.Enabled = True
            chkStuddedStone.Checked = True
            chkStuddedStone.Enabled = False
        Else
            grbSummaryWise.Visible = True
        End If
    End Sub

    Private Sub rtbSummaryWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummaryWise.CheckedChanged
        If rbtSummaryWise.Checked = True Then
            grbSummaryWise.Visible = True
            rbtBillNoWise.Checked = True
            chkSubItem.Enabled = False
            chkStuddedStone.Enabled = True
        Else
            grbSummaryWise.Visible = False
        End If
        chkGroupItem.Checked = Not rbtSummaryWise.Checked
        chkGroupItem.Enabled = Not rbtSummaryWise.Checked
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Public Function funcAddMetalName() As Integer
        strSql = "select DISTINCT metalname from  " & cnAdminDb & "..metalmast ORDER BY METALNAME"
        cmbMetalName.Items.Clear()
        cmbMetalName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetalName, False, False)
        cmbMetalName.Text = "ALL"
    End Function
    Public Function funcLoadDesigner() As Integer
        strSql = vbCrLf + " SELECT 'ALL' DESIGNERNAME,0 AS RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + "SELECT DISTINCT DESIGNERNAME,1 AS RESULT FROM  " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'Y')='Y' ORDER BY RESULT,DESIGNERNAME"
        Dim dtDes As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDes)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbDesigner, dtDes, "DESIGNERNAME", , "ALL")
    End Function

    Public Function funcAddCategory() As Integer
        strSql = "select DISTINCT CatName from " & cnAdminDb & "..Category "
        If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
            strSql += " where MetalId=(select MetalId from " & cnAdminDb & "..MetalMast where MetalName='" & cmbMetalName.Text & "')"
        End If
        strSql += " ORDER BY CATNAME"
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"
    End Function

    Public Function funcAddItemName() As Integer
        strSql = "select DISTINCT itemname from  " & cnAdminDb & "..itemmast"
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " where Catcode = (select catcode from " & cnAdminDb & "..category where catname = '" & cmbCategory.Text & "')"
        End If
        strSql += "  ORDER BY ITEMNAME"
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemName, False, False)
        cmbItemName.Text = "ALL"
    End Function

    Private Sub cmbMetalName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetalName.SelectedIndexChanged
        funcAddCategory()
    End Sub

    Private Sub cmbCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
        funcAddItemName()
    End Sub

    Public Function funcGridStyle() As Integer
        With gridView
            If rbtDetailWise.Checked = True Then
                If chkSubItem.Checked = True Then
                    .Columns("SUBITEMNAME").Visible = True
                Else
                    .Columns("SUBITEMNAME").Visible = False
                End If
                .Columns("SNO").Visible = False
                .Columns("BATCHNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("ITEMID").Visible = False
                With .Columns("BILLNO")
                    .DisplayIndex = 0
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("BILLDATE")
                    .DisplayIndex = 1
                    .Width = 75
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("ITEMNAME")
                    .DisplayIndex = 2
                    .Width = 150
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("SUBITEMNAME")
                    .DisplayIndex = 3
                    .Width = 150
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("TAGNO")
                    .DisplayIndex = 4
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("PCS")
                    .DisplayIndex = 5
                    .Width = 40
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("GRSWT")
                    .DisplayIndex = 6
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("NETWT")
                    .DisplayIndex = 7
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            ElseIf rbtSummaryWise.Checked = True Then
                .Columns("RESULT").Visible = False
                If rbtCategoryWise.Checked = True Then
                    '.Columns("CATCODE").Visible = False
                ElseIf rbtItemNameWise.Checked = True Then
                    '.Columns("ITEMID").Visible = False
                ElseIf rbtBillDateWise.Checked = True Then
                    ' .Columns("DUMMYDATE").Visible = False
                End If
                If rbtCategoryWise.Checked = True Or rbtItemNameWise.Checked = True Then
                    With .Columns(0)
                        .Width = 200
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
                If rbtBillNoWise.Checked = True Or rbtBillDateWise.Checked = True Then
                    With .Columns(0)
                        .Width = 80
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                End If
                With .Columns("PCS")
                    .Width = 40
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("GRSWT")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("NETWT")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("STONE")
                    .Width = 50
                    .DefaultCellStyle.BackColor = Color.AliceBlue
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            .Columns("COLHEAD").Visible = False
            If rbtDetailWise.Checked = True Then .Columns("SSNO").Visible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridMiscellaneous_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    '================================================================================================================================
    '   QUERY FOR WITHOUTSTORED PROCEDURE
    '====================================
    ''Summary wise Main Query from Issue table
    'If rbtSummaryWise.Checked = True Then
    '    strsql = "if (select 1 from sysobjects where name ='Temp" & SystemId & "MiseSumRpt') > 0 "
    '    strsql += " drop table Temp" & SystemId & "MiseSumRpt"
    '    strsql += " select"
    '    If rbtBillDateWise.Checked = True Then
    '        strsql += " Convert(varchar,I.TranDate,103) as BILLDATE,I.TRANDATE as DUMMYDATE,"
    '    ElseIf rbtBillNoWise.Checked = True Then
    '        strsql += " I.TranNo as BILLNO,"
    '    ElseIf rbtCategoryWise.Checked = True Then
    '        strsql += " (select CatName from " & cnAdminDb & "..Category where CatCode=I.CatCode) as CATNAME,I.CatCode,"
    '    ElseIf rbtItemNameWise.Checked = True Then
    '        strsql += " (select ItemName from " & cnAdminDb & "..ItemMast where ItemId=I.ItemId) as ITEMNAME,I.ItemId,"
    '    End If
    '    strsql += "Sum(PCS) as PCS,Sum(GRSWT) as GRSWT,Sum(NETWT) as NETWT,'     ' as STONE,1 RESULT"
    '    strsql += " into Temp" & SystemId & "MiseSumRpt from " & cnStockDb & "..Issue as I where I.TranType ='MI'"
    '    'Detail wise Main query from Issue Table
    'ElseIf rbtDetailWise.Checked = True Then
    '    strsql = "if(select 1 from sysobjects where name='Temp" & SystemId & "MiseDetRpt')>0"
    '    strsql += " drop table Temp" & SystemId & "MiseDetRpt"
    '    strsql += " select SNo as SNo,TranNo as BILLNO,Convert(varchar,TranDate,103) as BILLDATE,"
    '    strsql += "(select ItemName from " & cnAdminDb & "..ItemMast where ItemId=I.ItemId) as ITEMNAME,"
    '    strsql += "(select SubItemName from " & cnAdminDb & "..SubItemMast where SubItemId = I.SubItemId) as SUBITEMNAME,"
    '    strsql += "TAGNO,PCS,GRSWT,NETWT,BATCHNO,1 RESULT,ITEMID"
    '    strsql += " into Temp" & SystemId & "MiseDetRpt"
    '    strsql += " from " & cnStockDb & "..Issue as I where I.TranType ='MI'"
    'End If
    'strsql += " and I.TranDate between '" & dtpdatefrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpdateto.Value.Date.ToString("yyyy-MM-dd") & "'"
    ''Comman Conditions Checking Query
    'If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
    '    strsql += " and I.MetalId=(select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & Replace(cmbMetalName.Text, "'", "''") & "')"
    'End If
    'If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
    '    strsql += " and I.CatCode=(select CatCode from " & cnAdminDb & "..Category where CatName ='" & Replace(cmbCategory.Text, "'", "''") & "')"
    'End If
    'If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
    '    strsql += " and I.ItemId=(select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & Replace(cmbItemName.Text, "'", "''") & "')"
    'End If
    'If cmbCostName.Text <> "ALL" And cmbCostName.Text <> "" Then
    '    strsql += " and I.CostId=(select CostId from " & cnAdminDb & "..CostCentre where CostName='" & Replace(cmbCostName.Text, "'", "''") & "')"
    'End If
    'If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
    '    strsql += " and I.systemid ='" & Replace(txtNodeId.Text, "'", "''") & "'"
    'End If
    ''Summary wise Grouping Query
    'If rbtSummaryWise.Checked = True Then
    '    If rbtBillDateWise.Checked = True Then
    '        strsql += " group by I.TranDate"
    '    ElseIf rbtBillNoWise.Checked = True Then
    '        strsql += " group by I.TranNo"
    '    ElseIf rbtCategoryWise.Checked = True Then
    '        strsql += " group by I.CatCode"
    '    ElseIf rbtItemNameWise.Checked = True Then
    '        strsql += " group by I.ItemId"
    '    End If
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    
    '    cmd.ExecuteNonQuery()
    '    
    '    'Summary Wise Adding Stone Query
    '    If chkStuddedStone.Checked = True Then
    '        strsql = " if(select count(*) from Temp" & SystemId & "MiseSumRpt)>0 "
    '        strsql += " BEGIN"
    '        strsql += " insert into Temp" & SystemId & "MiseSumRpt("
    '        If rbtBillDateWise.Checked = True Then
    '            strsql += "BILLDATE,DUMMYDATE,"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += "BILLNO,"
    '        ElseIf rbtCategoryWise.Checked = True Then
    '            strsql += "CATNAME,CATCODE,"
    '        ElseIf rbtItemNameWise.Checked = True Then
    '            strsql += "ITEMNAME,ItemId,"
    '        End If
    '        strsql += "PCS,GRSWT,NETWT,STONE,RESULT)"
    '        'Query for Summation of Stone values 
    '        strsql += " select"
    '        If rbtBillDateWise.Checked = True Then
    '            strsql += " X.BILLDATE,X.DUMMYDATE,"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += " X.BILLNO,"
    '        ElseIf rbtCategoryWise.Checked = True Then
    '            strsql += " X.CATNAME,X.CatCode,"
    '        ElseIf rbtItemNameWise.Checked = True Then
    '            strsql += " X.ITEMNAME,X.ItemId,"
    '        End If
    '        strsql += "Sum(X.Pcs) as PCS,sum(x.grswt) as GRSWT, sum(x.netwt) as NETWT,"
    '        strsql += "case when X.result =2 then 'STONE' end as STONE,X.result as RESULT"
    '        strsql += " from"
    '        'Query for convert Stone Unit Carat to Gram
    '        strsql += " (select"
    '        If rbtBillDateWise.Checked = True Then
    '            strsql += " Convert(varchar,I.TranDate,103) as BILLDATE,I.TRANDATE as DUMMYDATE,"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += " '     '+I.TranNo as BILLNO,"
    '        ElseIf rbtCategoryWise.Checked = True Then
    '            strsql += " '     '+(select CatName from " & cnAdminDb & "..Category where CatCode=I.CatCode) as CATNAME,I.CatCode,"
    '        ElseIf rbtItemNameWise.Checked = True Then
    '            strsql += " '     '+(select ItemName from " & cnAdminDb & "..ItemMast where ItemId=I.ItemId) as ITEMNAME,I.ItemId,"
    '        End If
    '        strsql += "Sum(StnPCS) as PCS,case when S.StoneUnit='C' then Sum(StnWt)/5 else Sum(StnWt) end as GRSWT,"
    '        strsql += "case when S.StoneUnit='C' then Sum(StnWt)/5 else Sum(StnWt) end as NETWT,2 RESULT"
    '        strsql += " from " & cnStockDb & "..Issue as I," & cnStockDb & "..IssStone as S where I.Sno=S.IssSNo and I.TranType ='MI'"
    '        If rbtBillDateWise.Checked = True Then
    '            strsql += " group by I.TranDate,S.StoneUnit"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += " group by I.TranNo,S.StoneUnit"
    '        ElseIf rbtCategoryWise.Checked = True Then
    '            strsql += " group by I.CatCode,S.StoneUnit"
    '        ElseIf rbtItemNameWise.Checked = True Then
    '            strsql += " group by I.ItemId,S.StoneUnit"
    '        End If

    '        strsql += ")X"

    '        If rbtBillDateWise.Checked = True Then
    '            strsql += " group by X.BILLDATE,X.DUMMYDATE,X.RESULT"
    '        ElseIf rbtBillNoWise.Checked = True Then
    '            strsql += " group by X.Billno,X.RESULT"
    '        ElseIf rbtCategoryWise.Checked = True Then
    '            strsql += " group by X.CATNAME,x.CatCode,X.RESULT"
    '        ElseIf rbtItemNameWise.Checked = True Then
    '            strsql += " group by X.ITEMNAME,x.ItemId,X.RESULT"
    '        End If
    '        strsql += " END"
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        
    '        cmd.ExecuteNonQuery()
    '        
    '    End If
    '    'Summary Wise Final Query
    '    strsql = "select * from Temp" & SystemId & "MiseSumRpt"
    '    If rbtBillDateWise.Checked = True Then
    '        strsql += " order by DUMMYDATE,RESULT asc "
    '    ElseIf rbtBillNoWise.Checked = True Then
    '        strsql += " order by BILLNO,RESULT asc"
    '    ElseIf rbtCategoryWise.Checked = True Then
    '        strsql += " order by CatCode,RESULT asc"
    '    ElseIf rbtItemNameWise.Checked = True Then
    '        strsql += " order by ITEMID,RESULT asc"
    '    End If
    '    'Detail Wise Stone Query
    'ElseIf rbtDetailWise.Checked = True Then
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    
    '    cmd.ExecuteNonQuery()
    '    
    '    If chkStuddedStone.Checked = True Then
    '        strsql = " if(select count(*) from sysobjects where name ='Temp" & SystemId & "MiseDetRpt')>0"
    '        strsql += " BEGIN"
    '        strsql += " insert into Temp" & SystemId & "MiseDetRpt"
    '        strsql += " select IssSNo as SNo,S.TranNo as BILLNO,Convert(varchar,S.TranDate,103) as BILLDATE,"
    '        strsql += "'      '+ (select ItemName from " & cnAdminDb & "..ItemMast where ItemId=S.StnItemId) as ITEMNAME,"
    '        strsql += "'      '+(select SubItemName from " & cnAdminDb & "..SubItemMast where SubItemId = S.StnSubItemId) as SUBITEMNAME,"
    '        strsql += "I.TagNo as TAGNO,StnPCS as PCS,StnWt as GRSWT,StnWt as NETWT,S.BatchNo as BATCHNO,2 RESULT,i.itemid"
    '        strsql += " from " & cnStockDb & "..IssStone as S,Temp" & SystemId & "MiseDetRpt as I"
    '        strsql += " where S.IssSNo = I.SNo "
    '        strsql += " END"
    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        
    '        cmd.ExecuteNonQuery()
    '        
    '    End If
    '    'Detail Wise Final Query
    '    strsql = "select * from Temp" & SystemId & "MiseDetRpt order by sno,result,BillNo"
    'End If
    '================================================================================================================================

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub dtpfrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmMiscellaneousReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmMiscellaneousReport_Properties))
        cmbMetalName.Text = obj.p_cmbMetalName
        cmbCategory.Text = obj.p_cmbCategory
        cmbItemName.Text = obj.p_cmbItemName
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
        rbtDetailWise.Checked = obj.p_rbtDetailWise
        rbtSummaryWise.Checked = obj.p_rbtSummaryWise
        chkSubItem.Checked = obj.p_chkSubItem
        chkStuddedStone.Checked = obj.p_chkStuddedStone
        rbtBoth.Checked = obj.p_rbtBoth
        rbtNormal.Checked = obj.p_rbtNormal
        rbtOrder.Checked = obj.p_rbtOrder
        rbtBillNoWise.Checked = obj.p_rbtBillNoWise
        rbtBillDateWise.Checked = obj.p_rbtBillDateWise
        rbtCategoryWise.Checked = obj.p_rbtCategoryWise
        rbtItemNameWise.Checked = obj.p_rbtItemNameWise
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmMiscellaneousReport_Properties
        obj.p_cmbMetalName = cmbMetalName.Text
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_rbtDetailWise = rbtDetailWise.Checked
        obj.p_rbtSummaryWise = rbtSummaryWise.Checked
        obj.p_chkSubItem = chkSubItem.Checked
        obj.p_chkStuddedStone = chkStuddedStone.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtNormal = rbtNormal.Checked
        obj.p_rbtOrder = rbtOrder.Checked
        obj.p_rbtBillNoWise = rbtBillNoWise.Checked
        obj.p_rbtBillDateWise = rbtBillDateWise.Checked
        obj.p_rbtCategoryWise = rbtCategoryWise.Checked
        obj.p_rbtItemNameWise = rbtItemNameWise.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmMiscellaneousReport_Properties))
    End Sub

    Private Sub dtpTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.LostFocus
        funcAddNodeId()
    End Sub

    Private Sub chkLstNodeId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstNodeId.GotFocus
        If chkLstNodeId.Items.Count = 0 Then
            funcAddNodeId()
        End If
    End Sub
End Class

Public Class frmMiscellaneousReport_Properties
    Private cmbMetalName As String = "ALL"
    Public Property p_cmbMetalName() As String
        Get
            Return cmbMetalName
        End Get
        Set(ByVal value As String)
            cmbMetalName = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private chkCompanySelectAll As Boolean
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
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private rbtDetailWise As Boolean = True
    Public Property p_rbtDetailWise() As Boolean
        Get
            Return rbtDetailWise
        End Get
        Set(ByVal value As Boolean)
            rbtDetailWise = value
        End Set
    End Property
    Private rbtSummaryWise As Boolean = False
    Public Property p_rbtSummaryWise() As Boolean
        Get
            Return rbtSummaryWise
        End Get
        Set(ByVal value As Boolean)
            rbtSummaryWise = value
        End Set
    End Property
    Private chkSubItem As Boolean = True
    Public Property p_chkSubItem() As Boolean
        Get
            Return chkSubItem
        End Get
        Set(ByVal value As Boolean)
            chkSubItem = value
        End Set
    End Property
    Private chkStuddedStone As Boolean = False
    Public Property p_chkStuddedStone() As Boolean
        Get
            Return chkStuddedStone
        End Get
        Set(ByVal value As Boolean)
            chkStuddedStone = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtNormal As Boolean = False
    Public Property p_rbtNormal() As Boolean
        Get
            Return rbtNormal
        End Get
        Set(ByVal value As Boolean)
            rbtNormal = value
        End Set
    End Property
    Private rbtOrder As Boolean = False
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property
    Private rbtBillNoWise As Boolean = False
    Public Property p_rbtBillNoWise() As Boolean
        Get
            Return rbtBillNoWise
        End Get
        Set(ByVal value As Boolean)
            rbtBillNoWise = value
        End Set
    End Property
    Private rbtBillDateWise As Boolean = True
    Public Property p_rbtBillDateWise() As Boolean
        Get
            Return rbtBillDateWise
        End Get
        Set(ByVal value As Boolean)
            rbtBillDateWise = value
        End Set
    End Property
    Private rbtCategoryWise As Boolean = False
    Public Property p_rbtCategoryWise() As Boolean
        Get
            Return rbtCategoryWise
        End Get
        Set(ByVal value As Boolean)
            rbtCategoryWise = value
        End Set
    End Property
    Private rbtItemNameWise As Boolean = True
    Public Property p_rbtItemNameWise() As Boolean
        Get
            Return rbtItemNameWise
        End Get
        Set(ByVal value As Boolean)
            rbtItemNameWise = value
        End Set
    End Property


End Class