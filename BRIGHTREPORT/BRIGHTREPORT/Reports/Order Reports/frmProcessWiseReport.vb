Imports System.Data.OleDb
Public Class frmProcessWiseReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub AccSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AccSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
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
    Function LoadCostcentre()
        Dim dtCostCentre As New DataTable
        chkcmbCostcentre.Items.Clear()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostCentre, "COSTNAME", , "ALL")

    End Function
    Function LoadProcess()
        Dim dtCostCentre As New DataTable
        chkcmbCostcentre.Items.Clear()
        strSql = " SELECT 'ALL' ORDSTATE_NAME,1 DISPORDER"
        strSql += " UNION ALL"
        strSql += " SELECT ORDSTATE_NAME , DISPORDER  FROM " & cnAdminDb & "..ORDERSTATUS   ORDER BY DISPORDER , ORDSTATE_NAME "
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(cmbProcess, dtCostCentre, "ORDSTATE_NAME", True)
    End Function
    Function LoadCompany()
        cmbCompany.Items.Add("ALL")
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE ISNULL(ACTIVE,'') <> 'N'  ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, cmbCompany, False, False)
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
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        tabMain.SelectedTab = TabView
        lblTitle.Text = ""
        gridView.DataSource = Nothing
        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORIRDETAIL' AND XTYPE = 'U') > 0  DROP TABLE TEMPTABLEDB..TEMPORIRDETAIL"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000
        Dim CostName As String = ""
        CostName = Replace(chkcmbCostcentre.Text, ",", "','")
        Dim FilterString As String = ""
        If cmbCompany.Text <> "" And cmbCompany.Text <> "ALL" Then
            FilterString += vbCrLf + " AND O.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME ='" & cmbCompany.Text & "')"
        End If
        If chkcmbCostcentre.Text <> "ALL" And chkcmbCostcentre.Text <> "" Then
            FilterString += vbCrLf + " AND O.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYNAME IN('" & CostName & "')) "
        End If
        If cmbOrderNo.Text <> "" And cmbOrderNo.Text <> "ALL" Then
            FilterString += vbCrLf + " AND O.ORNO = '" & cmbOrderNo.Text & "'"
        End If
        If cmbCategory.Text <> "" And cmbCategory.Text <> "ALL" Then
            FilterString += vbCrLf + " AND O.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        If cmbProcess.Text <> "" And cmbProcess.Text <> "ALL" Then
            FilterString += " AND O.ORDSTATE_ID IN (SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & cmbProcess.Text & "')"
        End If
        If cmbSmith.Text <> "" And cmbSmith.Text <> "ALL" Then
            FilterString += " AND A.ACNAME = '" & cmbSmith.Text & "'"
        End If
        If chkAsOn.Checked Then
            FilterString += vbCrLf + " AND O.TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            FilterString += vbCrLf + " AND O.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("YYYY-MM-DD") & "' AND '" & dtpTo.Value.ToString("YYYY-MM-DD") & "'"
        End If
        strSql = vbCrLf + " WITH ORIRLASTRECORD AS ("
        strSql += vbCrLf + " SELECT ENTRYORDER,BATCHNO,ORNO FROM ("
        strSql += vbCrLf + "  SELECT O.ENTRYORDER  , O.BATCHNO,O.ORNO"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORIRDETAIL AS O "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ORMAST OM ON O.ORSNO = OM.SNO AND ISNULL(OM.CANCEL ,'') = '' "
        strSql += vbCrLf + "  WHERE 1=1"
        If chkAsOn.Checked Then
            strSql += vbCrLf + " AND O.TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " AND O.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("YYYY-MM-DD") & "' AND '" & dtpTo.Value.ToString("YYYY-MM-DD") & "'"
        End If
        strSql += vbCrLf + " And ISNULL(O.CANCEL,'') = ''"
        strSql += vbCrLf + "  AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
        If chkcmbCostcentre.Text <> "ALL" And chkcmbCostcentre.Text <> "" Then
            strSql += vbCrLf + " AND O.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYNAME IN('" & CostName & "')) "
        End If
        If cmbCompany.Text <> "ALL" And cmbCompany.Text <> "" Then
            strSql += vbCrLf + "  AND O.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME ='" & cmbCompany.Text & "')"
        End If
        strSql += vbCrLf + "  GROUP BY O.ENTRYORDER  , O.BATCHNO,O.ORNO )X WHERE "
        strSql += vbCrLf + "  ENTRYORDER = (SELECT MAX(ENTRYORDER) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = X.ORNO ) )"
        strSql += vbCrLf + "  SELECT "
        If rbtGroupByProcess.Checked Then
            strSql += vbCrLf + "  A.ACNAME  AS PARTICULAR"
        Else
            strSql += vbCrLf + "  (SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = O.ORDSTATE_ID) AS PARTICULAR"
        End If
        strSql += vbCrLf + "  ,OM.ORNO,OM.ORDATE,OM.DUEDATE"
        strSql += vbCrLf + "  ,OM.STYLENO"
        strSql += vbCrLf + "  ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,A.ACNAME "
        strSql += vbCrLf + "  ,SUM(O.PCS) PCS,SUM(O.GRSWT) GRSWT,SUM(O.NETWT)NETWT,CONVERT(VARCHAR,O.SNO) ORIRSNO"
        strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + "  ,SUM(OM.WAST) WASTAGE,SUM(OM.MC)MC,O.ACCODE "
        strSql += vbCrLf + "  ,(SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = O.ORDSTATE_ID) STATUS"
        strSql += vbCrLf + "  ,O.BATCHNO,'' COLHEAD, 2 RESULT"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORIRDETAIL"
        strSql += vbCrLf + "   FROM " & cnAdminDb & "..ORIRDETAIL AS O "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ORMAST OM ON O.ORSNO = OM.SNO AND ISNULL(OM.CANCEL ,'') = '' "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = O.CATCODE"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACHEAD  AS A ON A.ACCODE  = O.ACCODE  "
        strSql += vbCrLf + "  INNER JOIN ORIRLASTRECORD AS LR ON LR.BATCHNO = O.BATCHNO "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
        strSql += vbCrLf + "  WHERE 1=1 "
        strSql += vbCrLf + FilterString
        If rbtPending.Checked Then
            strSql += vbCrLf + " AND O.ORSTATUS = 'I'"
        End If
        strSql += vbCrLf + "  GROUP BY OM.ORNO,OM.STYLENO,OM.SNO,O.ACCODE,O.SNO,O.ORDSTATE_ID,O.ENTRYORDER"
        strSql += vbCrLf + "  ,IM.ITEMNAME,SM.SUBITEMNAME,LR.BATCHNO ,O.BATCHNO,O.ACCODE ,A.ACNAME,OM.ORDATE,OM.DUEDATE  "
        strSql += vbCrLf + "  ORDER BY O.ENTRYORDER DESC"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        If rbtDelivered.Checked Then
            strSql = "DELETE FROM TEMPTABLEDB..TEMPORIRDETAIL  WHERE STATUS <> 'DELIVERED'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000
        Else
            strSql = "DELETE FROM TEMPTABLEDB..TEMPORIRDETAIL  WHERE STATUS = 'DELIVERED'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000
        End If
        If rbtGroupByProcess.Checked Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPORIRDETAIL (PARTICULAR,STATUS,RESULT,COLHEAD   )"
            strSql += vbCrLf + "SELECT DISTINCT STATUS,STATUS,2,'T'  FROM TEMPTABLEDB..TEMPORIRDETAIL"
        Else
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPORIRDETAIL (PARTICULAR,ACNAME,RESULT,COLHEAD   )"
            strSql += vbCrLf + "SELECT DISTINCT ACNAME,ACNAME,2,'T'  FROM TEMPTABLEDB..TEMPORIRDETAIL"
        End If

        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPORIRDETAIL (PARTICULAR,ACNAME,ORNO,STATUS,PCS, GRSWT, NETWT , WASTAGE , MC, DIAPCS , DIAWT , STNPCS, STNWT, RESULT,COLHEAD   )"
        If rbtGroupByProcess.Checked Then
            strSql += vbCrLf + "SELECT 'SUBTOTAL','ZZZZZ'ACNAME,COUNT(ORNO),STATUS, SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT)NETWT"
            strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MC)MC ,SUM(DIAPCS) DIAPCS,SUM(DIAWT) DIAWT, SUM(STNPCS)STNPCS,SUM(STNWT) STNWT "
            strSql += vbCrLf + ",3 RESULT,'S' COLHEAD"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPORIRDETAIL WHERE RESULT = 2 GROUP BY STATUS"
        Else
            strSql += vbCrLf + "SELECT 'SUBTOTAL',ACNAME,COUNT(ORNO),'ZZZZZ'STATUS, SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT)NETWT"
            strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MC)MC ,SUM(DIAPCS) DIAPCS,SUM(DIAWT) DIAWT, SUM(STNPCS)STNPCS,SUM(STNWT) STNWT "
            strSql += vbCrLf + ",3 RESULT,'S' COLHEAD"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPORIRDETAIL WHERE RESULT = 2 GROUP BY ACNAME"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPORIRDETAIL (PARTICULAR,ACNAME,STATUS,PCS, GRSWT, NETWT , WASTAGE , MC, DIAPCS , DIAWT , STNPCS, STNWT, RESULT,COLHEAD   )"
        strSql += vbCrLf + "SELECT 'GRANDTOTAL','ZZZZZ'ACNAME,'ZZZZZ'STATUS, SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT)NETWT"
        strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MC)MC ,SUM(DIAPCS) DIAPCS,SUM(DIAWT) DIAWT, SUM(STNPCS)STNPCS,SUM(STNWT) STNWT "
        strSql += vbCrLf + ",4 RESULT,'S' COLHEAD"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMPORIRDETAIL WHERE RESULT = 2 "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        strSql = " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET GRSWT = NULL WHERE GRSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET PCS = NULL WHERE PCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET NETWT = NULL WHERE NETWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET WASTAGE = NULL WHERE WASTAGE = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET MC = NULL WHERE MC = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET DIAPCS = NULL WHERE DIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET DIAWT = NULL WHERE DIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET STNPCS = NULL WHERE STNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPORIRDETAIL SET STNWT = NULL WHERE STNWT = 0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

        If rbtGroupByProcess.Checked Then
            strSql = vbCrLf + "SELECT PARTICULAR, ACNAME , ORNO,ORDATE,DUEDATE ,PCS, GRSWT, NETWT , WASTAGE , MC, DIAPCS , DIAWT , STNPCS, STNWT ,STATUS AS PROCESS,RESULT , COLHEAD "
            strSql += vbCrLf + " FROM  TEMPTABLEDB..TEMPORIRDETAIL ORDER BY STATUS,ACNAME , RESULT  "
        Else
            strSql = vbCrLf + "SELECT PARTICULAR, ACNAME , ORNO,ORDATE,DUEDATE ,PCS, GRSWT, NETWT , WASTAGE , MC, DIAPCS , DIAWT , STNPCS, STNWT ,STATUS AS PROCESS,RESULT , COLHEAD "
            strSql += vbCrLf + " FROM  TEMPTABLEDB..TEMPORIRDETAIL ORDER BY ACNAME,STATUS , RESULT  "
        End If
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
        dtGrid.Columns("KEYNO").SetOrdinal(3)
        gridView.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise(gridView)
        Dim tit As String = Nothing
        tit = "Process Report  " & IIf(chkAsOn.Checked, " As On Date " & dtpFrom.Text & "", " Date From" & dtpFrom.Text & " to " & dtpTo.Text)
        lblTitle.Text = tit
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("ACNAME").Visible = False
            .Columns("PROCESS").Visible = False
            For cnt As Integer = 0 To .ColumnCount - 1
                gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        gridView.Focus()
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
        cmbCompany.Text = "ALL"
        rbtGroupBySmith.Checked = True
        dtpFrom.Value = Today.Date
        dtpTo.Value = Today.Date
        rbtGroupByProcess.Checked = True
        rbtAll.Checked = True
        chkAsOn.Checked = True
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.NAME, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        BrightPosting.GExport.Post(Me.NAME, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = TabGeneral
    End Sub

    Private Sub chkAsOn_CheckedChanged(sender As Object, e As EventArgs) Handles chkAsOn.CheckedChanged
        dtpTo.Visible = Not chkAsOn.Checked
        Label2.Visible = Not chkAsOn.Checked
    End Sub
End Class

